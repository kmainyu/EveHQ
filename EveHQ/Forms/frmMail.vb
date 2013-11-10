' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================
Imports System.Data
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.Core
Imports System.Threading
Imports System.Text
Imports System.Text.RegularExpressions
Imports EveHQ.Common.Extensions

Namespace Forms

    Public Class FrmMail
        Dim _displayPilot As New EveHQPilot
        Dim _cDisplayPilotName As String = ""
        Dim _mailStatus As String = ""
        ReadOnly _groupStyle As New ElementStyle()
        ReadOnly _subItemStyle As New ElementStyle()
        ReadOnly _readItemStyle As New ElementStyle()
        ReadOnly _unreadItemStyle As New ElementStyle()
        ReadOnly _allMails As New SortedList(Of Long, EveMailMessage)
        ReadOnly _allNotices As New SortedList(Of Long, EveNotification)

        Dim _currentUnreadMails As Integer = 0
        Dim _currentUnreadNotices As Integer = 0

        Public Property DisplayPilotName() As String
            Get
                Return _displayPilot.Name
            End Get
            Set(ByVal value As String)
                _cDisplayPilotName = value
                If cboPilots.Items.Contains(value) Then
                    cboPilots.SelectedItem = value
                End If
            End Set
        End Property

        Private Sub frmMail_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            ' Adds handlers for the external mail events
            RemoveHandler EveMailEvents.MailUpdateStarted, AddressOf MailUpdateStarted
            RemoveHandler EveMailEvents.MailUpdateCompleted, AddressOf MailUpdateCompleted
        End Sub

        Private Sub frmMail_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' Adds handlers for the external mail events
            AddHandler EveMailEvents.MailUpdateStarted, AddressOf MailUpdateStarted
            AddHandler EveMailEvents.MailUpdateCompleted, AddressOf MailUpdateCompleted
            Call UpdatePilots()
        End Sub

        Public Sub UpdatePilots()

            ' Save old Pilot info
            Dim oldPilot As String = ""
            If cboPilots.SelectedItem IsNot Nothing Then
                oldPilot = cboPilots.SelectedItem.ToString
            End If

            ' Update the pilots combo box
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboPilots.Items.Add(cPilot.Name)
                End If
            Next
            cboPilots.EndUpdate()

            ' Select a pilot
            If _cDisplayPilotName <> "" Then
                If cboPilots.Items.Contains(_cDisplayPilotName) = True Then
                    cboPilots.SelectedItem = _cDisplayPilotName
                Else
                    cboPilots.SelectedIndex = 0
                End If
            Else
                If oldPilot = "" Then
                    If cboPilots.Items.Count > 0 Then
                        If cboPilots.Items.Contains(HQ.Settings.StartupPilot) = True Then
                            cboPilots.SelectedItem = HQ.Settings.StartupPilot
                        Else
                            cboPilots.SelectedIndex = 0
                        End If
                    End If
                Else
                    If cboPilots.Items.Count > 0 Then
                        If cboPilots.Items.Contains(oldPilot) = True Then
                            cboPilots.SelectedItem = oldPilot
                        Else
                            cboPilots.SelectedIndex = 0
                        End If
                    End If
                End If
            End If

        End Sub

        Private Sub btnDownloadMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDownloadMail.Click
            If EveMailEvents.MailIsBeingProcessed = False Then
                ThreadPool.QueueUserWorkItem(AddressOf MailUpdateThread)
            Else
                MessageBox.Show("EveMail and Notifications are currently being downloaded automatically. Please wait until this has finished before checking again.", "EveMail Update In Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

        Private Sub MailUpdateThread(ByVal state As Object)
            Dim myMail As New EveMail
            AddHandler myMail.MailProgress, AddressOf DisplayMailProgress
            Try
                EveMailEvents.MailIsBeingProcessed = True
                Invoke(New MethodInvoker(AddressOf MailUpdateStarted))
                Call myMail.GetMail()
                Invoke(New MethodInvoker(AddressOf MailUpdateCompleted))
                EveMailEvents.UpdateMailNumbers()
            Catch e As Exception
                ' Handles cases where application is sluggish due to plug-in loading etc
                ' Simply abort
                Trace.TraceError(e.FormatException())
            Finally
                RemoveHandler myMail.MailProgress, AddressOf DisplayMailProgress
                EveMailEvents.MailIsBeingProcessed = False
            End Try
        End Sub

        Private Sub DisplayMailProgress(ByVal status As String)
            _mailStatus = status
            Invoke(New MethodInvoker(AddressOf UpdateMailProgress))
        End Sub

        Private Sub UpdateMailProgress()
            lblDownloadMailStatus.Text = _mailStatus
        End Sub

        Public Sub MailUpdateStarted()
            lblDownloadMailStatus.Text = "Processing EveMails and Notifications..."
            btnDownloadMail.Enabled = False
        End Sub

        Public Sub MailUpdateCompleted()
            ' Update the display with EveMail
            lblDownloadMailStatus.Text = "Updating EveMail display!"
            Call UpdateMailInfo()
            btnDownloadMail.Enabled = True
            lblDownloadMailStatus.Text = "Mail Processing Complete!"
        End Sub

        Public Sub UpdateMailNumbers()
            EveMailEvents.UpdateMailNumbers()
        End Sub

        Private Sub btnGetEveIDs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetEveIDs.Click
            ' Fetch all the emails in the database
            Try
                Dim mailingListIDs As New SortedList(Of Long, String)
                For Each mPilot As EveHQPilot In HQ.Settings.Pilots.Values
                    ' Stage 1: Download the latest EveMail API using the standard API method
                    Dim newMailingListIDs As New SortedList(Of Long, String)
                    If mPilot.Active = True Then
                        newMailingListIDs = CustomDataFunctions.WriteMailingListIDsToDatabase(mPilot)
                    End If
                    If newMailingListIDs.Count > 0 Then
                        For Each id As Long In newMailingListIDs.Keys
                            If mailingListIDs.ContainsKey(id) = False Then
                                mailingListIDs.Add(id, newMailingListIDs(id))
                            End If
                        Next
                    End If
                Next
                Const strSQL As String = "SELECT * FROM eveMail ORDER BY messageID DESC;"
                Dim mailData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
                If mailData IsNot Nothing Then
                    If mailData.Tables(0).Rows.Count > 0 Then
                        Dim ids As New List(Of String)
                        For Each mailRow As DataRow In mailData.Tables(0).Rows
                            ' Get Sender IDs
                            CustomDataFunctions.ParseIDs(ids, CStr(mailRow.Item("senderID")))
                            ' Get Character IDs
                            CustomDataFunctions.ParseIDs(ids, CStr(mailRow.Item("toCharacterIDs")))
                            ' Get Corp/Alliance IDs
                            CustomDataFunctions.ParseIDs(ids, CStr(mailRow.Item("toCorpOrAllianceID")))
                        Next
                        ' Remove any mailing list IDs
                        For Each mailingListID As Long In mailingListIDs.Keys
                            If ids.Contains(mailingListID.ToString) = True Then
                                ids.Remove(mailingListID.ToString)
                            End If
                        Next
                        Call CustomDataFunctions.WriteEveIDsToDatabase(ids)
                    End If
                End If
                MessageBox.Show("Successfully fetched Eve IDs!", "ID Retrieval Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("An error occurred while fetching the Eve IDs. The error was: " & ex.Message, "Error Fetching IDs", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            If HQ.Settings.Pilots.ContainsKey(cboPilots.SelectedItem.ToString) = True Then
                _displayPilot = HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
                Call UpdateMailInfo()
            End If
        End Sub

        Public Sub UpdateMailInfo()
            Call UpdateMails()
            Call UpdateNotifications()
            txtMail.Text = ""
        End Sub

        Private Sub UpdateMails()
            ' Get the mails for the selected pilot and the corp
            Dim strSQL As String = "SELECT * FROM eveMAIL WHERE originatorID=" & _displayPilot.ID & " ORDER BY messageID DESC;"
            Dim mailData As DataSet = CustomDataFunctions.GetCustomData(strSQL)

            Dim finalIDs As New SortedList(Of Long, String)
            _allMails.Clear()
            If mailData IsNot Nothing Then
                If mailData.Tables(0).Rows.Count > 0 Then
                    Dim ids As New List(Of String)
                    For Each mailRow As DataRow In mailData.Tables(0).Rows
                        Dim newMail As New EveMailMessage
                        newMail.MessageKey = CStr(mailRow.Item("messageKey"))
                        newMail.MessageID = CLng(mailRow.Item("messageID"))
                        newMail.OriginatorID = CLng(mailRow.Item("originatorID"))
                        newMail.SenderID = CLng(mailRow.Item("senderID"))
                        newMail.MessageDate = CDate(mailRow.Item("sentDate"))
                        newMail.MessageTitle = CStr(mailRow.Item("title"))
                        newMail.ToCorpAllianceIDs = CStr(mailRow.Item("toCorpOrAllianceID"))
                        newMail.ToCharacterIDs = CStr(mailRow.Item("toCharacterIDs"))
                        newMail.ToListIDs = CStr(mailRow.Item("toListIDs"))
                        newMail.ReadFlag = CBool(mailRow.Item("readMail"))
                        If IsDBNull(mailRow.Item("messageBody")) = False Then
                            newMail.MessageBody = CStr(mailRow.Item("messageBody"))
                        Else
                            newMail.MessageBody = ("<Message body not available>")
                        End If
                        _allMails.Add(-newMail.MessageID, newMail)
                        ' Get Sender IDs
                        CustomDataFunctions.ParseIDs(ids, CStr(mailRow.Item("senderID")))
                        ' Get Character IDs
                        CustomDataFunctions.ParseIDs(ids, CStr(mailRow.Item("toCharacterIDs")))
                        ' Get Corp/Alliance IDs
                        CustomDataFunctions.ParseIDs(ids, CStr(mailRow.Item("toCorpOrAllianceID")))
                        ' Get Mailing List IDs
                        CustomDataFunctions.ParseIDs(ids, CStr(mailRow.Item("toListIDs")))
                    Next
                    Dim strID As New StringBuilder
                    For Each id As String In ids
                        If id <> "" Then
                            strID.Append(id & ",")
                        End If
                    Next
                    strID.Append("0")
                    ' Get the name data from the DB
                    strSQL = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                    Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
                    If idData IsNot Nothing Then
                        If idData.Tables(0).Rows.Count > 0 Then
                            For Each idRow As DataRow In idData.Tables(0).Rows
                                finalIDs.Add(CLng(idRow.Item("eveID")), CStr(idRow.Item("eveName")))
                            Next
                        End If
                    End If
                End If
            End If

            ' New code for saving into the treeview
            adtMails.BeginUpdate()
            adtMails.Nodes.Clear()
            adtMails.TileSize = New Size(275, 50)

            Dim inboxNode As New Node("EveMail Inbox", _groupStyle)
            adtMails.Nodes.Add(inboxNode)

            Dim sentNode As New Node("EveMail Sent Items", _groupStyle)
            sentNode.Expanded = False
            adtMails.Nodes.Add(sentNode)

            _currentUnreadMails = 0
            For Each newMail As EveMailMessage In _allMails.Values
                If newMail.SenderID = CDbl(_displayPilot.ID) Then
                    ' Sent Items
                    Dim strTo As String
                    If newMail.ToListIDs = "" Or newMail.ToListIDs = "0" Then
                        strTo = newMail.ToCharacterIDs & ", " & newMail.ToCorpAllianceIDs
                        Dim ids As New List(Of String)
                        CustomDataFunctions.ParseIDs(ids, strTo)
                        strTo = ""
                        For Each id As String In ids
                            If finalIDs.ContainsKey(CLng(id)) = True Then
                                strTo &= "; " & finalIDs(CLng(id))
                            End If
                        Next
                        If strTo = "" Then
                            strTo = "IDs: " & newMail.ToCharacterIDs & "," & newMail.ToCorpAllianceIDs
                        Else
                            If strTo.Length > 1 Then
                                strTo = strTo.Remove(0, 2)
                            End If
                        End If
                    Else
                        strTo = "<Mailing List>"
                    End If
                    sentNode.Nodes.Add(CreateChildNode(newMail.MessageID.ToString, strTo, newMail.MessageTitle, newMail.MessageDate.ToString, My.Resources.EveMail32, _readItemStyle, _subItemStyle))
                Else
                    ' Inbox
                    Dim senderName As String = "ID: " & newMail.SenderID.ToString
                    If finalIDs.ContainsKey(newMail.SenderID) = True Then
                        senderName = finalIDs(newMail.SenderID)
                    End If
                    If newMail.ReadFlag = False Then
                        inboxNode.Nodes.Add(CreateChildNode(newMail.MessageID.ToString, senderName, newMail.MessageTitle, newMail.MessageDate.ToString, My.Resources.EveMail32, _unreadItemStyle, _subItemStyle))
                        _currentUnreadMails += 1
                    Else
                        inboxNode.Nodes.Add(CreateChildNode(newMail.MessageID.ToString, senderName, newMail.MessageTitle, newMail.MessageDate.ToString, My.Resources.EveMail32, _readItemStyle, _subItemStyle))
                    End If
                End If
            Next
            inboxNode.Text = "EveMail Inbox (" & _currentUnreadMails.ToString & ")"
            If _currentUnreadMails > 0 Then
                inboxNode.Expanded = True
            Else
                inboxNode.Expanded = False
            End If
            adtMails.EndUpdate()

        End Sub

        Private Function CreateChildNode(ByVal key As String, ByVal nodeText As String, ByVal subText As String, ByVal dateText As String, ByVal image As Image, ByVal itemStyle As ElementStyle, ByVal childItemStyle As ElementStyle) As Node
            Dim childNode As New Node(nodeText, itemStyle)
            childNode.Name = key
            childNode.Image = image
            childNode.Cells.Add(New Cell(dateText, childItemStyle))
            childNode.Cells.Add(New Cell(subText, childItemStyle))
            Return childNode
        End Function

        Private Sub UpdateNotifications()
            ' Get the notifications for the selected pilot and the corp
            Dim strSQL As String = "SELECT * FROM eveNotifications WHERE originatorID=" & _displayPilot.ID & " ORDER BY messageID DESC;"
            Dim noticeData As DataSet = CustomDataFunctions.GetCustomData(strSQL)

            Dim finalIDs As New SortedList(Of Long, String)
            _allNotices.Clear()
            If noticeData IsNot Nothing Then
                If noticeData.Tables(0).Rows.Count > 0 Then
                    Dim ids As New List(Of String)
                    For Each noticeRow As DataRow In noticeData.Tables(0).Rows
                        Dim newNotice As New EveNotification
                        newNotice.MessageKey = CStr(noticeRow.Item("messageKey"))
                        newNotice.MessageID = CLng(noticeRow.Item("messageID"))
                        newNotice.OriginatorID = CLng(noticeRow.Item("originatorID"))
                        newNotice.SenderID = CLng(noticeRow.Item("senderID"))
                        newNotice.TypeID = CLng(noticeRow.Item("typeID"))
                        newNotice.MessageDate = CDate(noticeRow.Item("sentDate"))
                        newNotice.ReadFlag = CBool(noticeRow.Item("readMail"))
                        If IsDBNull(noticeRow.Item("messageBody")) = False Then
                            newNotice.MessageBody = CStr(noticeRow.Item("messageBody"))
                        Else
                            newNotice.MessageBody = ("<Message body not available>")
                        End If
                        _allNotices.Add(-newNotice.MessageID, newNotice)
                        ' Get Sender IDs
                        CustomDataFunctions.ParseIDs(ids, CStr(noticeRow.Item("senderID")))
                    Next
                    Dim strID As New StringBuilder
                    For Each id As String In ids
                        If id <> "" Then
                            strID.Append(id & ",")
                        End If
                    Next
                    strID.Append("0")
                    ' Get the name data from the DB
                    strSQL = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                    Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
                    If idData IsNot Nothing Then
                        If idData.Tables(0).Rows.Count > 0 Then
                            For Each idRow As DataRow In idData.Tables(0).Rows
                                finalIDs.Add(CLng(idRow.Item("eveID")), CStr(idRow.Item("eveName")))
                            Next
                        End If
                    End If
                End If
            End If

            ' New code for saving into the treeview
            adtMails.BeginUpdate()

            Dim noticeNode As New Node("Eve Notifications", _groupStyle)
            noticeNode.Expanded = False
            adtMails.Nodes.Add(noticeNode)

            _currentUnreadNotices = 0
            For Each newNotice As EveNotification In _allNotices.Values
                Dim strNotice As String
                If [Enum].IsDefined(GetType(EveNotificationTypes), CInt(newNotice.TypeID)) = True Then
                    strNotice = [Enum].GetName(GetType(EveNotificationTypes), newNotice.TypeID)
                Else
                    strNotice = "Unknown Notification"
                End If
                Dim senderName As String = "ID: " & newNotice.SenderID.ToString
                If finalIDs.ContainsKey(newNotice.SenderID) = True Then
                    senderName = finalIDs(newNotice.SenderID)
                End If
                If newNotice.ReadFlag = False Then
                    noticeNode.Nodes.Add(CreateChildNode(newNotice.MessageID.ToString, senderName, strNotice, newNotice.MessageDate.ToString, My.Resources.EveMail32, _unreadItemStyle, _subItemStyle))
                    _currentUnreadNotices += 1
                Else
                    noticeNode.Nodes.Add(CreateChildNode(newNotice.MessageID.ToString, senderName, strNotice, newNotice.MessageDate.ToString, My.Resources.EveMail32, _readItemStyle, _subItemStyle))
                End If
            Next
            noticeNode.Text = "Eve Notifications (" & _currentUnreadNotices.ToString & ")"
            adtMails.EndUpdate()

        End Sub

        Private Sub btnMarkAllMailsRead_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMarkAllMailsRead.Click
            ' Confirm we want to mark them all as read
            Dim reply As DialogResult = MessageBox.Show("This will mark all EveMails as read for all characters. Are you sure you want to mark all EveMails as read?", "Read All EveMails?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                Const updateSQL As String = "UPDATE eveMail SET readMail=1 WHERE readMail=0;"
                If CustomDataFunctions.SetCustomData(updateSQL) = -2 Then
                    MessageBox.Show("There was an error setting the read status of the EveMails. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting EveMail Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                Call UpdateMails()
                Call MailUpdateCompleted()
            End If
        End Sub

        Private Sub btnMarkAllNoticesRead_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMarkAllNoticesRead.Click
            ' Confirm we want to mark them all as read
            Dim reply As DialogResult = MessageBox.Show("This will mark all Notifications as read for all characters. Are you sure you want to mark all Notifications as read?", "Read All Notifications?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                Const updateSQL As String = "UPDATE eveNotifications SET readMail=1 WHERE readMail=0;"
                If CustomDataFunctions.SetCustomData(updateSQL) = -2 Then
                    MessageBox.Show("There was an error setting the read status of the Eve Notifications. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting Eve Notification Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                Call UpdateNotifications()
                Call MailUpdateCompleted()
            End If
        End Sub

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Define group node style
            _groupStyle = New ElementStyle()
            _groupStyle.TextColor = Color.Navy
            _groupStyle.Font = New Font(adtMails.Font.FontFamily, 9.0F)
            _groupStyle.Name = "groupstyle"
            adtMails.Styles.Add(_groupStyle)

            ' Define sub-item style, simply to make text gray
            _subItemStyle = New ElementStyle()
            _subItemStyle.TextColor = Color.Gray
            _subItemStyle.Name = "subitemstyle"
            adtMails.Styles.Add(_subItemStyle)

            ' Define "read" style
            _readItemStyle = New ElementStyle()
            _readItemStyle.TextColor = Color.Black
            _readItemStyle.Font = New Font(adtMails.Font.FontFamily, 8.0F)
            _readItemStyle.Name = "readitemstyle"
            adtMails.Styles.Add(_readItemStyle)

            ' Define "unread" style
            _unreadItemStyle = New ElementStyle()
            _unreadItemStyle.TextColor = Color.Black
            _unreadItemStyle.Font = New Font(adtMails.Font.FontFamily, 8.0F, FontStyle.Bold)
            _unreadItemStyle.Name = "unreaditemstyle"
            adtMails.Styles.Add(_unreadItemStyle)
        End Sub

        Private Sub adtMails_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtMails.SelectionChanged
            If adtMails.SelectedNodes.Count > 0 Then
                Dim keyNode As Node = adtMails.SelectedNodes(0)
                Dim key As String = adtMails.SelectedNodes(0).Name
                ' Select whether this is an evemail or notification
                If keyNode.Parent IsNot Nothing Then
                    If keyNode.Parent.Text.StartsWith("EveMail", StringComparison.Ordinal) = True Then
                        ' Is an Evemail
                        Dim mailtext As String = CleanMessage(_allMails(-CLng(key)).MessageBody)
                        txtMail.Text = mailtext
                        ' Update the style of the item to read
                        If keyNode.Parent.Text.StartsWith("EveMail Inbox", StringComparison.Ordinal) And keyNode.Style.Name = "unreaditemstyle" Then
                            keyNode.Style = _readItemStyle
                            _currentUnreadMails -= 1
                            keyNode.Parent.Text = "EveMail Inbox (" & _currentUnreadMails.ToString & ")"
                            Dim updateSQL As String = "UPDATE eveMail SET readMail=1 WHERE messageKey='" & _allMails(-CLng(key)).MessageKey & "';"
                            If CustomDataFunctions.SetCustomData(updateSQL) = -2 Then
                                MessageBox.Show("There was an error setting the read status of the EveMails. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting EveMail Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                            Call frmEveHQ.UpdateEveMailButton()
                        End If
                    Else
                        Dim newnotice As EveNotification = _allNotices(-CLng(key))
                        txtMail.Text = CleanMessage(newnotice.MessageBody)
                        'If [Enum].IsDefined(GetType(Core.EveNotificationTypes), CInt(newnotice.TypeID)) = True Then
                        '   lblMail.Text = CleanMessage(newnotice.MessageBody)
                        'Else
                        '	lblMail.Text = "Unknown Notification"
                        'End If
                        If keyNode.Style.Name = "unreaditemstyle" Then
                            keyNode.Style = _readItemStyle
                            _currentUnreadNotices -= 1
                            keyNode.Parent.Text = "Eve Notifications (" & _currentUnreadNotices.ToString & ")"
                            Dim updateSQL As String = "UPDATE eveNotifications SET readMail=1 WHERE messageKey='" & _allNotices(-CLng(key)).MessageKey & "';"
                            If CustomDataFunctions.SetCustomData(updateSQL) = -2 Then
                                MessageBox.Show("There was an error setting the read status of the Eve Notifications. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting Eve Notification Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                            Call frmEveHQ.UpdateEveMailButton()
                        End If
                    End If
                    btnCopyEvemail.Enabled = True
                End If
            Else
                btnCopyEvemail.Enabled = False
            End If
        End Sub

        Private Function CleanMessage(ByVal message As String) As String
            Dim output As String = Message.Trim()

            output = output.Replace("<br>", "<br />").Replace("<BR>", "<br />")
            output = output.Replace("<br />", ControlChars.CrLf)
            output = output.Replace("&#39;", "'")
            output = output.Replace("&amp;", "&")
            output = output.Replace("&quot;", """")
            output = output.Replace("&nbsp;", " ")
            output = Regex.Replace(output, "<[^<>]+>", "")

            Return output
        End Function

        Private Sub btnCopyEvemail_Click(sender As Object, e As EventArgs) Handles btnCopyEvemail.Click
            Try
                Clipboard.SetText(txtMail.Text.Replace("<br />", ControlChars.CrLf))
            Catch ex As Exception
                ' Error - don't bother doing anything
            End Try
        End Sub

    End Class
End NameSpace