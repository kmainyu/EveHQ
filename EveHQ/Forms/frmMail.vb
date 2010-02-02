Imports System.Xml
Imports System.Data
Imports System.Text
Imports DotNetLib.Windows.Forms

Public Class frmMail
    Dim displayPilot As New EveHQ.Core.Pilot
    Dim cDisplayPilotName As String = ""
    Dim mailStatus As String = ""

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

    Private Sub frmMail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Adds handlers for the external mail events
        AddHandler frmEveHQ.MailUpdateStarted, AddressOf MailUpdateStarted
        AddHandler frmEveHQ.MailUpdateCompleted, AddressOf MailUpdateCompleted
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
        Threading.ThreadPool.QueueUserWorkItem(AddressOf MailUpdateThread)
    End Sub

    Private Sub MailUpdateThread(ByVal state As Object)
        Dim myMail As New EveHQ.Core.EveMail
        AddHandler myMail.MailProgress, AddressOf DisplayMailProgress
        Me.Invoke(New MethodInvoker(AddressOf MailUpdateStarted))
        Call myMail.GetMail()
        Me.Invoke(New MethodInvoker(AddressOf MailUpdateCompleted))
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
        Call frmEveHQ.UpdateEveMailButton()
        lblDownloadMailStatus.Text = "Mail Processing Complete!"
    End Sub

    Private Sub btnGetEveIDs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetEveIDs.Click
        ' Fetch all the emails in the database
        Dim strSQL As String = "SELECT * FROM eveMail ORDER BY messageID DESC;"
        Dim mailData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                Dim IDs As New ArrayList
                For Each MailRow As DataRow In mailData.Tables(0).Rows
                    ' Get Sender IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(MailRow.Item("senderID")))
                    ' Get Character IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(MailRow.Item("toCharacterIDs")))
                    ' Get Corp/Alliance IDs
                    EveHQ.Core.DataFunctions.ParseIDs(IDs, CStr(MailRow.Item("toCorpOrAllianceID")))
                Next
                Call EveHQ.Core.DataFunctions.WriteEveIDsToDatabase(IDs)
            End If
        End If
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
    End Sub

    Private Sub UpdateMails()
        ' Get the mails for the selected pilot and the corp
        Dim strSQL As String = "SELECT * FROM eveMAIL WHERE originatorID=" & displayPilot.ID & " ORDER BY messageID DESC;"
        Dim mailData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        Dim allMails As New SortedList(Of Long, EveHQ.Core.EveMailMessage)
        Dim FinalIDs As New SortedList(Of Long, String)
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                Dim IDs As New ArrayList
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
        clvMail.BeginUpdate()
        clvMail.Items.Clear()
        Dim UnreadFont As New Font(clvMail.Font, FontStyle.Bold)
        For Each newMail As EveHQ.Core.EveMailMessage In allMails.Values
            Dim mailItem As New ContainerListViewItem
            If FinalIDs.ContainsKey(newMail.SenderID) = True Then
                mailItem.Text = FinalIDs(newMail.SenderID)
            Else
                'TODO: Replace this with a routine to get the name from the API??
                mailItem.Text = "Unknown"
            End If
            clvMail.Items.Add(mailItem)
            Dim strTo As String = ""
            If newMail.ToListIDs = "" Then
                strTo = newMail.ToCharacterIDs & ", " & newMail.ToCorpAllianceIDs
                Dim IDs As New ArrayList
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
            mailItem.SubItems(1).Text = strTo
            mailItem.SubItems(2).Text = newMail.MessageTitle
            mailItem.SubItems(3).Text = FormatDateTime(newMail.MessageDate)
            If newMail.ReadFlag = False Then
                mailItem.Font = UnreadFont
            End If
        Next
        clvMail.EndUpdate()
    End Sub

    Private Sub UpdateNotifications()
        ' Get the notifications for the selected pilot and the corp
        Dim strSQL As String = "SELECT * FROM eveNotifications WHERE originatorID=" & displayPilot.ID & " ORDER BY messageID DESC;"
        Dim NoticeData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        Dim allNotices As New SortedList(Of Long, EveHQ.Core.EveNotification)
        Dim FinalIDs As New SortedList(Of Long, String)
        If NoticeData IsNot Nothing Then
            If NoticeData.Tables(0).Rows.Count > 0 Then
                Dim IDs As New ArrayList
                For Each NoticeRow As DataRow In NoticeData.Tables(0).Rows
                    Dim newNotice As New EveHQ.Core.EveNotification
                    newNotice.MessageKey = CStr(NoticeRow.Item("messageKey"))
                    newNotice.MessageID = CLng(NoticeRow.Item("messageID"))
                    newNotice.OriginatorID = CLng(NoticeRow.Item("originatorID"))
                    newNotice.SenderID = CLng(NoticeRow.Item("senderID"))
                    newNotice.TypeID = CLng(NoticeRow.Item("typeID"))
                    newNotice.MessageDate = CDate(NoticeRow.Item("sentDate"))
                    newNotice.ReadFlag = CBool(NoticeRow.Item("readMail"))
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
        clvNotifications.BeginUpdate()
        clvNotifications.Items.Clear()
        Dim UnreadFont As New Font(clvMail.Font, FontStyle.Bold)
        For Each newNotice As EveHQ.Core.EveNotification In allNotices.Values
            Dim mailItem As New ContainerListViewItem
            If FinalIDs.ContainsKey(newNotice.SenderID) = True Then
                mailItem.Text = FinalIDs(newNotice.SenderID)
            Else
                'TODO: Replace this with a routine to get the name from the API
                mailItem.Text = "Unknown"
            End If
            clvNotifications.Items.Add(mailItem)
            Dim strNotice As String = [Enum].GetName(GetType(EveHQ.Core.EveNotificationTypes), newNotice.TypeID)
            strNotice = strNotice.Replace("_", " ")
            mailItem.SubItems(1).Text = displayPilot.Name
            mailItem.SubItems(2).Text = strNotice
            mailItem.SubItems(3).Text = FormatDateTime(newNotice.MessageDate)
            If newNotice.ReadFlag = False Then
                mailItem.Font = UnreadFont
            End If
        Next
        clvNotifications.EndUpdate()
    End Sub


End Class

