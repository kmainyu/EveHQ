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
Imports System.Xml
Imports System.Data
Imports System.Text
Imports DevComponents.DotNetBar
Imports DevComponents.AdvTree
Imports System.Text.RegularExpressions

Public Class frmMail
    Dim displayPilot As New EveHQ.Core.Pilot
    Dim cDisplayPilotName As String = ""
    Dim mailStatus As String = ""
    Dim groupStyle As New ElementStyle()
    Dim subItemStyle As New ElementStyle()
    Dim ReadItemStyle As New ElementStyle()
    Dim UnreadItemStyle As New ElementStyle()
    Dim allMails As New SortedList(Of Long, EveHQ.Core.EveMailMessage)
    Dim allNotices As New SortedList(Of Long, EveHQ.Core.EveNotification)

    Dim CurrentUnreadMails As Integer = 0
    Dim CurrentUnreadNotices As Integer = 0

    Public Property DisplayPilotName() As String
        Get
            Return displayPilot.Name
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            If cboPilots.Items.Contains(value) Then
                cboPilots.SelectedItem = value
            End If
        End Set
    End Property

    Private Sub frmMail_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Adds handlers for the external mail events
        RemoveHandler EveHQ.Core.EveMailEvents.MailUpdateStarted, AddressOf MailUpdateStarted
        RemoveHandler EveHQ.Core.EveMailEvents.MailUpdateCompleted, AddressOf MailUpdateCompleted
    End Sub

    Private Sub frmMail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Adds handlers for the external mail events
        AddHandler EveHQ.Core.EveMailEvents.MailUpdateStarted, AddressOf MailUpdateStarted
        AddHandler EveHQ.Core.EveMailEvents.MailUpdateCompleted, AddressOf MailUpdateCompleted
        ' Check for the existence of our 2 required tables
        Call EveHQ.Core.DataFunctions.CheckForEveMailTable()
        Call EveHQ.Core.DataFunctions.CheckForEveNotificationTable()
        Call EveHQ.Core.DataFunctions.CheckForIDNameTable()
        Call Me.UpdatePilots()
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
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next
        cboPilots.EndUpdate()

        ' Select a pilot
        If cDisplayPilotName <> "" Then
            If cboPilots.Items.Contains(cDisplayPilotName) = True Then
                cboPilots.SelectedItem = cDisplayPilotName
            Else
                cboPilots.SelectedIndex = 0
            End If
        Else
            If oldPilot = "" Then
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = True Then
                        cboPilots.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
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

    Private Sub btnDownloadMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownloadMail.Click
        If EveHQ.Core.EveMailEvents.MailIsBeingProcessed = False Then
            Threading.ThreadPool.QueueUserWorkItem(AddressOf MailUpdateThread)
        Else
            MessageBox.Show("EveMail and Notifications are currently being downloaded automatically. Please wait until this has finished before checking again.", "EveMail Update In Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub MailUpdateThread(ByVal state As Object)
        Dim myMail As New EveHQ.Core.EveMail
        AddHandler myMail.MailProgress, AddressOf DisplayMailProgress
        Try
            EveHQ.Core.EveMailEvents.MailIsBeingProcessed = True
            Me.Invoke(New MethodInvoker(AddressOf MailUpdateStarted))
            Call myMail.GetMail()
            Me.Invoke(New MethodInvoker(AddressOf MailUpdateCompleted))
            EveHQ.Core.EveMailEvents.UpdateMailNumbers()
        Catch e As Exception
            ' Handles cases where application is sluggish due to plug-in loading etc
            ' Simply abort
        Finally
            RemoveHandler myMail.MailProgress, AddressOf DisplayMailProgress
            EveHQ.Core.EveMailEvents.MailIsBeingProcessed = False
        End Try
    End Sub

    Private Sub DisplayMailProgress(ByVal Status As String)
        mailStatus = Status
        Me.Invoke(New MethodInvoker(AddressOf UpdateMailProgress))
    End Sub

    Private Sub UpdateMailProgress()
        lblDownloadMailStatus.Text = mailStatus
    End Sub

    Public Sub MailUpdateStarted()
        lblDownloadMailStatus.Text = "Processing EveMails and Notifications..."
        btnDownloadMail.Enabled = False
    End Sub

    Public Sub MailUpdateCompleted()
        ' Update the display with EveMail
        lblDownloadMailStatus.Text = "Updating EveMail display!"
        Call Me.UpdateMailInfo()
        btnDownloadMail.Enabled = True
        lblDownloadMailStatus.Text = "Mail Processing Complete!"
    End Sub

    Public Sub UpdateMailNumbers()
        EveHQ.Core.EveMailEvents.UpdateMailNumbers()
    End Sub

    Private Sub RemoteMailUpdateStarted()
        Me.Invoke(New MethodInvoker(AddressOf MailUpdateStarted))
    End Sub

    Private Sub RemoteMailUpdateCompleted()
        Me.Invoke(New MethodInvoker(AddressOf MailUpdateCompleted))
    End Sub

    Private Sub btnGetEveIDs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetEveIDs.Click
        ' Fetch all the emails in the database
        Try
            Dim MailingListIDs As New SortedList(Of Long, String)
            For Each mPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                ' Stage 1: Download the latest EveMail API using the standard API method
                Dim NewMailingListIDs As New SortedList(Of Long, String)
                If mPilot.Active = True Then
                    NewMailingListIDs = EveHQ.Core.DataFunctions.WriteMailingListIDsToDatabase(mPilot)
                End If
                If NewMailingListIDs.Count > 0 Then
                    For Each ID As Long In NewMailingListIDs.Keys
                        If MailingListIDs.ContainsKey(ID) = False Then
                            MailingListIDs.Add(ID, NewMailingListIDs(ID))
                        End If
                    Next
                End If
            Next
            Dim strSQL As String = "SELECT * FROM eveMail ORDER BY messageID DESC;"
            Dim mailData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
            If mailData IsNot Nothing Then
                If mailData.Tables(0).Rows.Count > 0 Then
                    Dim IDs As New List(Of String)
                    For Each MailRow As DataRow In mailData.Tables(0).Rows
                        ' Get Sender IDs
                        EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(MailRow.Item("senderID")))
                        ' Get Character IDs
                        EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(MailRow.Item("toCharacterIDs")))
                        ' Get Corp/Alliance IDs
                        EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(MailRow.Item("toCorpOrAllianceID")))
                    Next
                    ' Remove any mailing list IDs
                    For Each MailingListID As Long In MailingListIDs.Keys
                        If IDs.Contains(MailingListID.ToString) = True Then
                            IDs.Remove(MailingListID.ToString)
                        End If
                    Next
                    Call EveHQ.Core.DataFunctions.WriteEveIDsToDatabase(IDs)
                End If
            End If
            MessageBox.Show("Successfully fetched Eve IDs!", "ID Retrieval Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("An error occurred while fetching the Eve IDs. The error was: " & ex.Message, "Error Fetching IDs", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilots.SelectedItem.ToString) = True Then
            displayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            Call UpdateMailInfo()
        End If
    End Sub

    Public Sub UpdateMailInfo()
        Call Me.UpdateMails()
        Call Me.UpdateNotifications()
        lblMail.Text = ""
    End Sub

    Private Sub UpdateMails()
        ' Get the mails for the selected pilot and the corp
        Dim strSQL As String = "SELECT * FROM eveMAIL WHERE originatorID=" & displayPilot.ID & " ORDER BY messageID DESC;"
        Dim mailData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

        Dim FinalIDs As New SortedList(Of Long, String)
        allMails.Clear()
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                Dim IDs As New List(Of String)
                For Each mailRow As DataRow In mailData.Tables(0).Rows
                    Dim newMail As New EveHQ.Core.EveMailMessage
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
                    allMails.Add(-newMail.MessageID, newMail)
                    ' Get Sender IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(mailRow.Item("senderID")))
                    ' Get Character IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(mailRow.Item("toCharacterIDs")))
                    ' Get Corp/Alliance IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(mailRow.Item("toCorpOrAllianceID")))
                    ' Get Mailing List IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(mailRow.Item("toListIDs")))
                Next
                Dim strID As New StringBuilder
                For Each ID As String In IDs
                    If ID <> "" Then
                        strID.Append(ID & ",")
                    End If
                Next
                strID.Append("0")
                ' Get the name data from the DB
                strSQL = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If IDData IsNot Nothing Then
                    If IDData.Tables(0).Rows.Count > 0 Then
                        For Each IDRow As DataRow In IDData.Tables(0).Rows
                            FinalIDs.Add(CLng(IDRow.Item("eveID")), CStr(IDRow.Item("eveName")))
                        Next
                    End If
                End If
            End If
        End If

        ' New code for saving into the treeview
        adtMails.BeginUpdate()
        adtMails.Nodes.Clear()
        adtMails.TileSize = New Size(275, 50)

        Dim InboxNode As New Node("EveMail Inbox", groupStyle)
        adtMails.Nodes.Add(InboxNode)

        Dim SentNode As New Node("EveMail Sent Items", groupStyle)
        SentNode.Expanded = False
        adtMails.Nodes.Add(SentNode)

        CurrentUnreadMails = 0
        For Each newMail As EveHQ.Core.EveMailMessage In allMails.Values
            If newMail.SenderID = CDbl(displayPilot.ID) Then
                ' Sent Items
                Dim strTo As String = ""
                If newMail.ToListIDs = "" Or newMail.ToListIDs = "0" Then
                    strTo = newMail.ToCharacterIDs & ", " & newMail.ToCorpAllianceIDs
                    Dim IDs As New List(Of String)
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, strTo)
                    strTo = ""
                    For Each ID As String In IDs
                        If FinalIDs.ContainsKey(CLng(ID)) = True Then
                            strTo &= "; " & FinalIDs(CLng(ID))
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
                SentNode.Nodes.Add(CreateChildNode(newMail.MessageID.ToString, strTo, newMail.MessageTitle, newMail.MessageDate.ToString, My.Resources.EveMail32, ReadItemStyle, subItemStyle))
            Else
                ' Inbox
                Dim SenderName As String = "ID: " & newMail.SenderID.ToString
                If FinalIDs.ContainsKey(newMail.SenderID) = True Then
                    SenderName = FinalIDs(newMail.SenderID)
                End If
                If newMail.ReadFlag = False Then
                    InboxNode.Nodes.Add(CreateChildNode(newMail.MessageID.ToString, SenderName, newMail.MessageTitle, newMail.MessageDate.ToString, My.Resources.EveMail32, UnreadItemStyle, subItemStyle))
                    CurrentUnreadMails += 1
                Else
                    InboxNode.Nodes.Add(CreateChildNode(newMail.MessageID.ToString, SenderName, newMail.MessageTitle, newMail.MessageDate.ToString, My.Resources.EveMail32, ReadItemStyle, subItemStyle))
                End If
            End If
        Next
        InboxNode.Text = "EveMail Inbox (" & CurrentUnreadMails.ToString & ")"
        If CurrentUnreadMails > 0 Then
            InboxNode.Expanded = True
        Else
            InboxNode.Expanded = False
        End If
        adtMails.EndUpdate()

    End Sub

    Private Function CreateChildNode(ByVal Key As String, ByVal nodeText As String, ByVal subText As String, ByVal dateText As String, ByVal image As Image, ByVal ItemStyle As ElementStyle, ByVal subItemStyle As ElementStyle) As Node
        Dim childNode As New Node(nodeText, ItemStyle)
        childNode.Name = Key
        childNode.Image = image
        childNode.Cells.Add(New Cell(dateText, subItemStyle))
        childNode.Cells.Add(New Cell(subText, subItemStyle))
        Return childNode
    End Function

    Private Sub UpdateNotifications()
        ' Get the notifications for the selected pilot and the corp
        Dim strSQL As String = "SELECT * FROM eveNotifications WHERE originatorID=" & displayPilot.ID & " ORDER BY messageID DESC;"
        Dim NoticeData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)

        Dim FinalIDs As New SortedList(Of Long, String)
        allNotices.Clear()
        If NoticeData IsNot Nothing Then
            If NoticeData.Tables(0).Rows.Count > 0 Then
                Dim IDs As New List(Of String)
                For Each NoticeRow As DataRow In NoticeData.Tables(0).Rows
                    Dim newNotice As New EveHQ.Core.EveNotification
                    newNotice.MessageKey = CStr(NoticeRow.Item("messageKey"))
                    newNotice.MessageID = CLng(NoticeRow.Item("messageID"))
                    newNotice.OriginatorID = CLng(NoticeRow.Item("originatorID"))
                    newNotice.SenderID = CLng(NoticeRow.Item("senderID"))
                    newNotice.TypeID = CLng(NoticeRow.Item("typeID"))
                    newNotice.MessageDate = CDate(NoticeRow.Item("sentDate"))
                    newNotice.ReadFlag = CBool(NoticeRow.Item("readMail"))
                    If IsDBNull(NoticeRow.Item("messageBody")) = False Then
                        newNotice.MessageBody = CStr(NoticeRow.Item("messageBody"))
                    Else
                        newNotice.MessageBody = ("<Message body not available>")
                    End If
                    allNotices.Add(-newNotice.MessageID, newNotice)
                    ' Get Sender IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(NoticeRow.Item("senderID")))
                Next
                Dim strID As New StringBuilder
                For Each ID As String In IDs
                    If ID <> "" Then
                        strID.Append(ID & ",")
                    End If
                Next
                strID.Append("0")
                ' Get the name data from the DB
                strSQL = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If IDData IsNot Nothing Then
                    If IDData.Tables(0).Rows.Count > 0 Then
                        For Each IDRow As DataRow In IDData.Tables(0).Rows
                            FinalIDs.Add(CLng(IDRow.Item("eveID")), CStr(IDRow.Item("eveName")))
                        Next
                    End If
                End If
            End If
        End If

        ' New code for saving into the treeview
        adtMails.BeginUpdate()

        Dim NoticeNode As New Node("Eve Notifications", groupStyle)
        NoticeNode.Expanded = False
        adtMails.Nodes.Add(NoticeNode)

        CurrentUnreadNotices = 0
        For Each newNotice As EveHQ.Core.EveNotification In allNotices.Values
            Dim strNotice As String = ""
            If [Enum].IsDefined(GetType(EveHQ.Core.EveNotificationTypes), CInt(newNotice.TypeID)) = True Then
                strNotice = [Enum].GetName(GetType(EveHQ.Core.EveNotificationTypes), newNotice.TypeID)
            Else
                strNotice = "Unknown Notification"
            End If
            Dim SenderName As String = "ID: " & newNotice.SenderID.ToString
            If FinalIDs.ContainsKey(newNotice.SenderID) = True Then
                SenderName = FinalIDs(newNotice.SenderID)
            End If
            If newNotice.ReadFlag = False Then
                NoticeNode.Nodes.Add(CreateChildNode(newNotice.MessageID.ToString, SenderName, strNotice, newNotice.MessageDate.ToString, My.Resources.EveMail32, UnreadItemStyle, subItemStyle))
                CurrentUnreadNotices += 1
            Else
                NoticeNode.Nodes.Add(CreateChildNode(newNotice.MessageID.ToString, SenderName, strNotice, newNotice.MessageDate.ToString, My.Resources.EveMail32, ReadItemStyle, subItemStyle))
            End If
        Next
        NoticeNode.Text = "Eve Notifications (" & CurrentUnreadNotices.ToString & ")"
        adtMails.EndUpdate()

    End Sub

    Private Sub btnMarkAllMailsRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarkAllMailsRead.Click
        ' Confirm we want to mark them all as read
        Dim reply As DialogResult = MessageBox.Show("This will mark all EveMails as read for all characters. Are you sure you want to mark all EveMails as read?", "Read All EveMails?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            Dim updateSQL As String = "UPDATE eveMail SET readMail=1 WHERE readMail=0;"
            If EveHQ.Core.DataFunctions.SetData(updateSQL) = -1 Then
                MessageBox.Show("There was an error setting the read status of the EveMails. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting EveMail Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            Call Me.UpdateMails()
            Call Me.MailUpdateCompleted()
        End If
    End Sub

    Private Sub btnMarkAllNoticesRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarkAllNoticesRead.Click
        ' Confirm we want to mark them all as read
        Dim reply As DialogResult = MessageBox.Show("This will mark all Notifications as read for all characters. Are you sure you want to mark all Notifications as read?", "Read All Notifications?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            Dim updateSQL As String = "UPDATE eveNotifications SET readMail=1 WHERE readMail=0;"
            If EveHQ.Core.DataFunctions.SetData(updateSQL) = -1 Then
                MessageBox.Show("There was an error setting the read status of the Eve Notifications. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting Eve Notification Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            Call Me.UpdateNotifications()
            Call Me.MailUpdateCompleted()
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Define group node style
        groupStyle = New ElementStyle()
        groupStyle.TextColor = Color.Navy
        groupStyle.Font = New Font(adtMails.Font.FontFamily, 9.0F)
        groupStyle.Name = "groupstyle"
        adtMails.Styles.Add(groupStyle)

        ' Define sub-item style, simply to make text gray
        subItemStyle = New ElementStyle()
        subItemStyle.TextColor = Color.Gray
        subItemStyle.Name = "subitemstyle"
        adtMails.Styles.Add(subItemStyle)

        ' Define "read" style
        ReadItemStyle = New ElementStyle()
        ReadItemStyle.TextColor = Color.Black
        ReadItemStyle.Font = New Font(adtMails.Font.FontFamily, 8.0F)
        ReadItemStyle.Name = "readitemstyle"
        adtMails.Styles.Add(ReadItemStyle)

        ' Define "unread" style
        UnreadItemStyle = New ElementStyle()
        UnreadItemStyle.TextColor = Color.Black
        UnreadItemStyle.Font = New Font(adtMails.Font.FontFamily, 8.0F, FontStyle.Bold)
        UnreadItemStyle.Name = "unreaditemstyle"
        adtMails.Styles.Add(UnreadItemStyle)
    End Sub

    Private Sub adtMails_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtMails.SelectionChanged
        If adtMails.SelectedNodes.Count > 0 Then
            Dim KeyNode As Node = adtMails.SelectedNodes(0)
            Dim key As String = adtMails.SelectedNodes(0).Name
            ' Select whether this is an evemail or notification
            If KeyNode.Parent IsNot Nothing Then
                If KeyNode.Parent.Text.StartsWith("EveMail") = True Then
                    ' Is an Evemail
                    Dim mailtext As String = CleanMessage(allMails(-CLng(key)).MessageBody)
                    lblMail.Text = mailtext
                    ' Update the style of the item to read
                    If KeyNode.Parent.Text.StartsWith("EveMail Inbox") And KeyNode.Style.Name = "unreaditemstyle" Then
                        KeyNode.Style = ReadItemStyle
                        CurrentUnreadMails -= 1
                        KeyNode.Parent.Text = "EveMail Inbox (" & CurrentUnreadMails.ToString & ")"
                        Dim updateSQL As String = "UPDATE eveMail SET readMail=1 WHERE messageKey='" & allMails(-CLng(key)).MessageKey & "';"
                        If EveHQ.Core.DataFunctions.SetData(updateSQL) = -1 Then
                            MessageBox.Show("There was an error setting the read status of the EveMails. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting EveMail Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                        Call frmEveHQ.UpdateEveMailButton()
                    End If
                Else
                    Dim newnotice As EveHQ.Core.EveNotification = allNotices(-CLng(key))
                    lblMail.Text = CleanMessage(newnotice.MessageBody)
                    'If [Enum].IsDefined(GetType(EveHQ.Core.EveNotificationTypes), CInt(newnotice.TypeID)) = True Then
                    '   lblMail.Text = CleanMessage(newnotice.MessageBody)
                    'Else
                    '	lblMail.Text = "Unknown Notification"
                    'End If
                    If KeyNode.Style.Name = "unreaditemstyle" Then
                        KeyNode.Style = ReadItemStyle
                        CurrentUnreadNotices -= 1
                        KeyNode.Parent.Text = "Eve Notifications (" & CurrentUnreadNotices.ToString & ")"
                        Dim updateSQL As String = "UPDATE eveNotifications SET readMail=1 WHERE messageKey='" & allNotices(-CLng(key)).MessageKey & "';"
                        If EveHQ.Core.DataFunctions.SetData(updateSQL) = -1 Then
                            MessageBox.Show("There was an error setting the read status of the Eve Notifications. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & updateSQL.ToString, "Error Setting Eve Notification Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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

    Private Function CleanMessage(ByVal Message As String) As String
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

    Private Sub btnCopyEvemail_Click(sender As System.Object, e As System.EventArgs) Handles btnCopyEvemail.Click
        Try
            Clipboard.SetText(lblMail.Text.Replace("<br />", ControlChars.CrLf))
        Catch ex As Exception
            ' Error - don't bother doing anything
        End Try
    End Sub

End Class
