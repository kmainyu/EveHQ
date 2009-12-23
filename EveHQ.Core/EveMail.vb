Imports System.Text
Imports System.Xml
Imports System.Windows.Forms

Public Class EveMail

    Shared MailTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    Public Shared Sub GetEveMail()
        ' Stage 1: Download the latest EveMail API using the standard API method
        ' Stage 2: Populate the class with our EveMail
        ' Stage 3: Check the last messageID posted to our database
        ' Stage 4: Post all new messages to the database
        ' Stage 5: Get all the IDs and parse them

        ' Add to the auto timer
        EveHQ.Core.HQ.NextAutoMailAPITime = EveHQ.Core.HQ.NextAutoMailAPITime.AddMinutes(30)
        Dim Mails As New SortedList(Of String, EveHQ.Core.EveMailMessage)
        For Each mPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            ' Stage 1: Download the latest EveMail API using the standard API method
            If mPilot.Active = True Then
                Dim accountName As String = mPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim mAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                    ' Make a call to the EveHQ.Core.API to fetch the EveMail
                    Dim mailXML As New XmlDocument
                    mailXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.MailMessages, mAccount, mPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
                    If mailXML IsNot Nothing Then
                        ' Stage 2: Populate the class with our EveMails
                        If mailXML.SelectNodes("/eveapi/error").Count = 0 Then
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
                            ' Set the cache time
                            Dim CacheList As XmlNodeList
                            Dim cache As XmlNode
                            CacheList = mailXML.SelectNodes("/eveapi/cachedUntil")
                            cache = CacheList(0)
                            Dim cacheTime As Date = DateTime.ParseExact(cache.InnerText, MailTimeFormat, culture, Globalization.DateTimeStyles.None)
                            If cacheTime < EveHQ.Core.HQ.NextAutoMailAPITime Then
                                EveHQ.Core.HQ.NextAutoMailAPITime = cacheTime
                            End If
                        End If
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
                If EveHQ.Core.HQ.dataError.Contains("Cannot insert duplicate key") = True Then
                    ' Try an update
                    Dim updateSQL As String = "UPDATE eveMail SET readMail=" & CInt(cMail.ReadFlag) & " WHERE messageKey='" & cMail.MessageKey & "';"
                    If EveHQ.Core.DataFunctions.SetData(updateSQL) = False Then
                        MessageBox.Show("There was an error updating data in the Eve Mail database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing EveMails", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Else
                    MessageBox.Show("There was an error writing data to the Eve Mail database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing EveMails", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        Next

        ' Stage 5: Get all the IDs and parse them
        Dim IDs As New ArrayList
        For Each cMail As EveHQ.Core.EveMailMessage In Mails.Values
            ' Get Sender IDs
            EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.SenderID.ToString)
            ' Get Character IDs
            EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.ToCharacterIDs)
            ' Get Corp/Alliance IDs
            EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.ToCorpAllianceIDs)
        Next
        Call EveHQ.Core.DataFunctions.WriteEveIDsToDatabase(IDs)

        ' Just check the timer to make sure we're not gonna be bombarded with tons of short-lived requests!
        If EveHQ.Core.HQ.NextAutoMailAPITime < Now.AddSeconds(60) Then
            EveHQ.Core.HQ.NextAutoMailAPITime = EveHQ.Core.HQ.NextAutoMailAPITime.AddSeconds(60)
        End If
    End Sub

    Public Shared Sub GetNotifications()
        ' Stage 1: Download the latest EveNotifications API using the standard API method
        ' Stage 2: Populate the class with our Eve Notifications
        ' Stage 3: Check the last messageID posted to our database
        ' Stage 4: Post all new messages to the database
        ' Stage 5: Get all the IDs and parse them

        ' Add to the auto timer
        EveHQ.Core.HQ.NextAutoMailAPITime = EveHQ.Core.HQ.NextAutoMailAPITime.AddMinutes(30)
        Dim Notices As New SortedList(Of String, EveHQ.Core.EveNotification)
        For Each mPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            ' Stage 1: Download the latest EveMail API using the standard API method
            If mPilot.Active = True Then
                Dim accountName As String = mPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim mAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                    ' Make a call to the EveHQ.Core.API to fetch the EveMail
                    Dim mailXML As New XmlDocument
                    mailXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Notifications, mAccount, mPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
                    If mailXML IsNot Nothing Then
                        If mailXML.SelectNodes("/eveapi/error").Count = 0 Then
                            ' Stage 2: Populate the class with our EveMails
                            Dim mailList As XmlNodeList
                            Dim mail As XmlNode
                            mailList = mailXML.SelectNodes("/eveapi/result/rowset/row")
                            If mailList.Count > 0 Then
                                For Each mail In mailList
                                    Dim nMail As New EveHQ.Core.EveNotification
                                    nMail.OriginatorID = CLng(mPilot.ID)
                                    nMail.MessageID = CLng(mail.Attributes.GetNamedItem("notificationID").Value)
                                    nMail.SenderID = CLng(mail.Attributes.GetNamedItem("senderID").Value)
                                    nMail.MessageDate = DateTime.ParseExact(mail.Attributes.GetNamedItem("sentDate").Value, MailTimeFormat, culture, Globalization.DateTimeStyles.None)
                                    nMail.TypeID = CLng(mail.Attributes.GetNamedItem("typeID").Value)
                                    nMail.ReadFlag = CBool(mail.Attributes.GetNamedItem("read").Value)
                                    nMail.MessageKey = nMail.MessageID.ToString & "_" & nMail.OriginatorID.ToString
                                    If Notices.ContainsKey(nMail.MessageKey) = False Then
                                        Notices.Add(nMail.MessageKey, nMail)
                                    End If
                                Next
                            End If
                            ' Set the cache time
                            Dim CacheList As XmlNodeList
                            Dim cache As XmlNode
                            CacheList = mailXML.SelectNodes("/eveapi/cachedUntil")
                            cache = CacheList(0)
                            Dim cacheTime As Date = DateTime.ParseExact(cache.InnerText, MailTimeFormat, culture, Globalization.DateTimeStyles.None)
                            If cacheTime < EveHQ.Core.HQ.NextAutoMailAPITime Then
                                EveHQ.Core.HQ.NextAutoMailAPITime = cacheTime
                            End If
                        End If
                    End If
                End If
            End If
        Next

        ' Stage 3: Check the last messageID posted to our database
        Dim lastMessageID As Long = -1
        Dim strSQL As String = "SELECT TOP 1 * FROM eveNotifications ORDER BY messageID DESC;"
        Dim NoticeData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        If NoticeData IsNot Nothing Then
            If NoticeData.Tables(0).Rows.Count > 0 Then
                lastMessageID = CLng(NoticeData.Tables(0).Rows(0).Item("messageID"))
            End If
        End If

        ' Stage 4: Post all new messages to the database
        Dim strInsert As String = "INSERT INTO eveNotifications (messageKey, messageID, originatorID, senderID, typeID, sentDate, readMail) VALUES "
        For Each NoticeKey As String In Notices.Keys
            Dim cMail As EveHQ.Core.EveNotification = Notices(NoticeKey)
            Dim uSQL As New StringBuilder
            uSQL.Append(strInsert)
            uSQL.Append("(")
            uSQL.Append("'" & cMail.MessageKey & "', ")
            uSQL.Append(cMail.MessageID & ", ")
            uSQL.Append(cMail.OriginatorID & ", ")
            uSQL.Append(cMail.SenderID & ", ")
            uSQL.Append(cMail.TypeID & ", ")
            uSQL.Append("'" & Format(cMail.MessageDate, "yyyy-MM-dd HH:mm:ss") & "', ")
            uSQL.Append(CInt(cMail.ReadFlag) & ");")
            If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = False Then
                If EveHQ.Core.HQ.dataError.Contains("Cannot insert duplicate key") = True Then
                    ' Try an update
                    Dim updateSQL As String = "UPDATE eveNotifications SET readMail=" & CInt(cMail.ReadFlag) & " WHERE messageKey='" & cMail.MessageKey & "';"
                    If EveHQ.Core.DataFunctions.SetData(updateSQL) = False Then
                        MessageBox.Show("There was an error updating data in the Eve Notifications database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve Notifications", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Else
                    MessageBox.Show("There was an error writing data to the Eve Notifications database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve Notifications", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        Next

        ' Stage 5: Get all the IDs and parse them
        Dim IDs As New ArrayList
        For Each cNotice As EveHQ.Core.EveNotification In Notices.Values
            ' Get Sender IDs
            EveHQ.Core.DataFunctions.ParseIDs(IDs, cNotice.SenderID.ToString)
        Next
        Call EveHQ.Core.DataFunctions.WriteEveIDsToDatabase(IDs)

        ' Just check the timer to make sure we're not gonna be bombarded with tons of short-lived requests!
        If EveHQ.Core.HQ.NextAutoMailAPITime < Now.AddSeconds(60) Then
            EveHQ.Core.HQ.NextAutoMailAPITime = EveHQ.Core.HQ.NextAutoMailAPITime.AddSeconds(60)
        End If

    End Sub

End Class

Public Class EveMailMessage
    Public MessageKey As String
    Public MessageID As Long
    Public OriginatorID As Long
    Public SenderID As Long
    Public MessageDate As Date
    Public MessageTitle As String
    Public ToCorpAllianceIDs As String
    Public ToCharacterIDs As String
    Public ToListIDs As String
    Public ReadFlag As Boolean
End Class

Public Class EveNotification
    Public MessageKey As String
    Public MessageID As Long
    Public OriginatorID As Long
    Public TypeID As Long
    Public SenderID As Long
    Public MessageDate As Date
    Public ReadFlag As Boolean
End Class

Public Enum EveNotificationTypes As Integer
    Character_Deleted = 2
    Give_Medal_To_Character = 3
    Alliance_Maintenance_Bill = 4
    Alliance_War_Declared = 5
    Alliance_War_Surrender = 6
    Alliance_War_Retracted = 7
    Alliance_War_Invalidated_By_Concord = 8
    Bill_Issued_To_A_Character = 9
    Bill_Issued_To_Corporation_Or_Alliance = 10
    Bill_Not_Paid_Because_Not_Enough_ISK_Available = 11
    Bill_Issued_By_A_Character_Paid = 12
    Bill_Issued_By_A_Corporation_Or_Alliance_Paid = 13
    Bounty_Claimed = 14
    Clone_Activated = 15
    New_Corp_Member_Application = 16
    Corp_Application_Rejected = 17
    Corp_Application_Accepted = 18
    Corp_Tax_Rate_Changed = 19
    Corp_News_Report_Typically_For_Shareholders = 20
    Player_Leaves_Corp = 21
    Corp_News_New_CEO = 22
    Corp_Dividend_Liquidation_Sent_To_Shareholders = 23
    Corp_Dividend_Payout_Sent_To_Shareholders = 24
    Corp_Vote_Created = 25
    Corp_CEO_Votes_Revoked_During_Voting = 26
    Corp_Declares_War = 27
    Corp_War_Has_Started = 28
    Corp_Surrenders_War = 29
    Corp_Retracts_War = 30
    Corp_War_Invalidated_By_Concord = 31
    Container_Password_Retrieval = 32
    Contraband_Or_Low_Standings_Cause_An_Attack_Or_Items_Being_Confiscated = 33
    First_Ship_Insurance = 34
    Ship_Destroyed_Insurance_Payed = 35
    Insurance_Contract_Invalidated_Runs_Out = 36
    Sovereignty_Claim_Fails_Alliance = 37
    Sovereignty_Claim_Fails_Corporation = 38
    Sovereignty_Bill_Late_Alliance = 39
    Sovereignty_Bill_Late_Corporation = 40
    Sovereignty_Claim_Lost_Alliance = 41
    Sovereignty_Claim_Lost_Corporation = 42
    Sovereignty_Claim_Aquired_Alliance = 43
    Sovereignty_Claim_Aquired_Corporation = 44
    Alliance_Anchoring_Alert = 45
    Alliance_Structure_Turns_Vulnerable = 46
    Alliance_Structure_Turns_Invulnerable = 47
    Sovereignty_Disruptor_Anchored = 48
    Structure_Won_Lost = 49
    Corp_Office_Lease_Expiration_Notice = 50
    Clone_Contract_Revoked_By_Station_Manager = 51
    Corp_Member_Clones_Moved_Between_Stations = 52
    Clone_Contract_Revoked_By_Station_Manager2 = 53
    Insurance_Contract_Expired = 54
    Insurance_Contract_Issued = 55
    Jump_Clone_Destroyed = 56
    Jump_Clone_Destroyed2 = 57
    Corporation_Joining_Factional_Warfare = 58
    Corporation_Leaving_Factional_Warfare = 59
    Corporation_Kicked_From_Factional_Warfare_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 60
    Character_Kicked_From_Factional_Warfare_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 61
    Corporation_In_Factional_Warfare_Warned_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 62
    Character_In_Factional_Warfare_Warned_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 63
    Character_Loses_Factional_Warfare_Rank = 64
    Character_Gains_Factional_Warfare_Rank = 65
    Agent_Has_Moved = 66
    Mass_Transaction_Reversal_Message = 67
    Reimbursement_Message = 68
    Agent_Locates_A_Character = 69
    Research_Mission_Becomes_Available_From_An_Agent = 70
    Agent_Mission_Offer_Expires = 71
    Agent_Mission_Times_Out = 72
    Agent_Offers_A_Storyline_Mission = 73
    Tutorial_Message_Sent_On_Character_Creation = 74
    Tower_Alert = 75
    Tower_Resource_Alert = 76
    Station_Aggression_Message = 77
    Station_State_Change_Message = 78
    Station_Conquered_Message = 79
    Station_Aggression_Message2 = 80
    Corporation_Requests_Joining_Factional_Warfare = 81
    Corporation_Requests_Leaving_Factional_Warfare = 82
    Corporation_Withdrawing_A_Request_To_Join_Factional_Warfare = 83
    Corporation_Withdrawing_A_Request_To_Leave_Factional_Warfare = 84
End Enum
