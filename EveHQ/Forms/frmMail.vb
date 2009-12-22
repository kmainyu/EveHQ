Imports System.Xml
Imports System.Data
Imports System.Text
Imports DotNetLib.Windows.Forms

Public Class frmMail
    Dim MailTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Dim displayPilot As New EveHQ.Core.Pilot
    Dim cDisplayPilotName As String = ""

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
        ' Check for the existence of our 2 required tables
        Call EveHQ.Core.DataFunctions.CheckForEveMailTable()
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
        ' Stage 1: Download the latest EveMail API using the standard API method
        ' Stage 2: Populate the class with our EveMail
        ' Stage 3: Check the last messageID posted to our database
        ' Stage 4: Post all new messages to the database
        ' Stage 5: Get all the IDs and parse them
        ' Stage 6: Update the display with EveMail

        Me.Cursor = Cursors.WaitCursor

        Dim Mails As New SortedList(Of String, EveHQ.Core.EveMailMessage)
        For Each mPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            ' Stage 1: Download the latest EveMail API using the standard API method
            If mPilot.Active = True Then
                Dim accountName As String = mPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim mAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                    ' Make a call to the EveHQ.Core.API to fetch the assets
                    Dim mailXML As New XmlDocument
                    mailXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.MailMessages, mAccount, mPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
                    ' Stage 2: Populate the class with our EveMails
                    Dim mailList As XmlNodeList
                    Dim mail As XmlNode
                    mailList = mailXML.SelectNodes("/eveapi/result/rowset/row")
                    If mailList.Count > 0 Then
                        For Each mail In mailList
                            Dim nMail As New EveHQ.Core.EveMailMessage
                            nMail.OriginatorID = CLng(mPilot.ID)
                            nMail.MessageID = CLng(mail.Attributes.GetNamedItem("messageID").Value)
                            nMail.SenderID = CLng(mail.Attributes.GetNamedItem("senderID").Value)
                            nMail.MessageDate = DateTime.ParseExact(mail.Attributes.GetNamedItem("sentDate").Value, MailTimeFormat, culture, Globalization.DateTimeStyles.None)
                            nMail.MessageTitle = mail.Attributes.GetNamedItem("title").Value
                            nMail.ToCharacterIDs = mail.Attributes.GetNamedItem("toCharacterIDs").Value
                            nMail.ToCorpAllianceIDs = mail.Attributes.GetNamedItem("toCorpOrAllianceID").Value
                            nMail.ToListIDs = mail.Attributes.GetNamedItem("toListIDs").Value
                            nMail.ReadFlag = CBool(mail.Attributes.GetNamedItem("read").Value)
                            nMail.MessageKey = nMail.MessageID.ToString & "_" & nMail.OriginatorID.ToString
                            If Mails.ContainsKey(nMail.MessageKey) = False Then
                                Mails.Add(nMail.MessageKey, nMail)
                            End If
                        Next
                    End If
                End If
            End If
        Next

        ' Stage 3: Check the last messageID posted to our database
        Dim lastMessageID As Long = -1
        Dim strSQL As String = "SELECT TOP 1 * FROM eveMail ORDER BY messageID DESC;"
        Dim mailData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                lastMessageID = CLng(mailData.Tables(0).Rows(0).Item("messageID"))
            End If
        End If

        ' Stage 4: Post all new messages to the database
        Dim strInsert As String = "INSERT INTO eveMail (messageKey, messageID, originatorID, senderID, sentDate, title, toCorpOrAllianceID, toCharacterIDs, toListIDs, readMail) VALUES "
        For Each mailKey As String In Mails.Keys
            Dim cMail As EveHQ.Core.EveMailMessage = Mails(mailKey)
            Dim uSQL As New StringBuilder
            uSQL.Append(strInsert)
            uSQL.Append("(")
            uSQL.Append("'" & cMail.MessageKey & "', ")
            uSQL.Append(cMail.MessageID & ", ")
            uSQL.Append(cMail.OriginatorID & ", ")
            uSQL.Append(cMail.SenderID & ", ")
            uSQL.Append("'" & Format(cMail.MessageDate, "yyyy-MM-dd HH:mm:ss") & "', ")
            uSQL.Append("'" & cMail.MessageTitle & "', ")
            uSQL.Append("'" & cMail.ToCorpAllianceIDs & "', ")
            uSQL.Append("'" & cMail.ToCharacterIDs & "', ")
            uSQL.Append("'" & cMail.ToListIDs & "', ")
            uSQL.Append(CInt(cMail.ReadFlag) & ");")
            If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = False Then
                MessageBox.Show("There was an error writing data to the Eve Mail database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing EveMails", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Next

        ' Stage 5: Get all the IDs and parse them
        Dim IDs As New ArrayList
        For Each cMail As EveHQ.Core.EveMailMessage In Mails.Values
            ' Get Sender IDs
            ParseIDs(IDs, cMail.SenderID.ToString)
            ' Get Character IDs
            ParseIDs(IDs, cMail.ToCharacterIDs)
            ' Get Corp/Alliance IDs
            ParseIDs(IDs, cMail.ToCorpAllianceIDs)
        Next
        Call Me.WriteEveIDsToDatabase(IDs)

        ' Stage 6: Update the display with EveMail
        Call Me.UpdateMailInfo()

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub ParseIDs(ByRef IDs As ArrayList, ByVal strID As String)
        Dim strIDs() As String = strID.Split(",".ToCharArray)
        For Each ID As String In strIDs
            If IDs.Contains(ID) = False Then
                IDs.Add(ID)
            End If
        Next
    End Sub

    Private Sub WriteEveIDsToDatabase(ByVal IDs As ArrayList)
        Dim strID As New StringBuilder
        For Each ID As String In IDs
            strID.Append(ID & ",")
        Next
        strID.Append("0")
        ' Send this to the API
        Dim IDXML As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IDToName, strID.ToString, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
        ' Parse this XML
        Dim FinalIDs As New SortedList(Of Long, String)
        Dim IDList As XmlNodeList
        Dim IDNode As XmlNode
        Dim eveID As Long = 0
        Dim eveName As String = ""
        IDList = IDXML.SelectNodes("/eveapi/result/rowset/row")
        If IDList.Count > 0 Then
            For Each IDNode In IDList
                eveID = CLng(IDNode.Attributes.GetNamedItem("characterID").Value)
                eveName = IDNode.Attributes.GetNamedItem("name").Value
                If FinalIDs.ContainsKey(eveID) = False Then
                    FinalIDs.Add(eveID, eveName)
                End If
            Next
        End If
        ' Add all the data to the database
        Dim strIDInsert As String = "INSERT INTO eveIDToName (eveID, eveName) VALUES "
        For Each eveID In FinalIDs.Keys
            eveName = FinalIDs(eveID)
            Dim uSQL As New StringBuilder
            uSQL.Append(strIDInsert)
            uSQL.Append("(" & eveID & ", ")
            uSQL.Append("'" & eveName & "');")
            If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = False Then
                'MessageBox.Show("There was an error writing data to the Eve ID database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve IDs", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Next
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
                    ParseIDs(IDs, CStr(MailRow.Item("senderID")))
                    ' Get Character IDs
                    ParseIDs(IDs, CStr(MailRow.Item("toCharacterIDs")))
                    ' Get Corp/Alliance IDs
                    ParseIDs(IDs, CStr(MailRow.Item("toCorpOrAllianceID")))
                Next
                Call Me.WriteEveIDsToDatabase(IDs)
            End If
        End If
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilots.SelectedItem.ToString) = True Then
            displayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            Call UpdateMailInfo()
        End If
    End Sub

    Private Sub UpdateMailInfo()
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
                    ParseIDs(IDs, CStr(mailRow.Item("senderID")))
                    ' Get Character IDs
                    ParseIDs(IDs, CStr(mailRow.Item("toCharacterIDs")))
                    ' Get Corp/Alliance IDs
                    ParseIDs(IDs, CStr(mailRow.Item("toCorpOrAllianceID")))
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
            mailItem.Text = FinalIDs(newMail.SenderID)
            clvMail.Items.Add(mailItem)
            mailItem.SubItems(1).Text = newMail.MessageTitle
            mailItem.SubItems(2).Text = FormatDateTime(newMail.MessageDate)
            If newMail.ReadFlag = False Then
                mailItem.Font = UnreadFont
            End If
        Next
        clvMail.EndUpdate()
    End Sub
End Class

