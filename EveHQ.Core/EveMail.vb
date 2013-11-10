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
Imports System.Globalization
Imports System.Xml
Imports EveHQ.EveAPI
Imports System.Text
Imports System.Windows.Forms
Imports System.Net.Mail
Imports System.Net

Public Class EveMailEvents
    Public Shared Event MailUpdateStarted()
    Public Shared Event MailUpdateCompleted()
    Public Shared Event MailUpdateNumbers()
    Public Shared Sub MailUpdateStart()
        RaiseEvent MailUpdateStarted()
    End Sub
    Public Shared Sub MailUpdateComplete()
        RaiseEvent MailUpdateCompleted()
    End Sub
    Public Shared Sub UpdateMailNumbers()
        RaiseEvent MailUpdateNumbers()
    End Sub
    Public Shared MailIsBeingProcessed As Boolean = False
End Class

Public Class EveMail
    Private Const MailTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    ReadOnly _culture As CultureInfo = New CultureInfo("en-GB")
    Public Event MailProgress(ByVal status As String)

    Public Sub GetMail()
        Call GetEveMail()
        Call GetNotifications()
    End Sub

    Private Sub GetEveMail()
        ' Stage 1: Download the latest EveMail API using the standard API method
        ' Stage 2: Populate the class with our EveMail
        ' Stage 3: Check the messages which have already been posted
        ' Stage 4: Post all new messages to the database
        ' Stage 5: Get all the IDs and parse them

        ' Add to the auto timer
        HQ.NextAutoMailAPITime = Now.AddMinutes(30)
        Dim mails As New SortedList(Of String, EveMailMessage)
        Dim mailingListIDs As New SortedList(Of Long, String)
        For Each mPilot As EveHQPilot In HQ.Settings.Pilots.Values
            ' Stage 1: Download the latest EveMail API using the standard API method
            If mPilot.Active = True Then
                Dim accountName As String = mPilot.Account
                If HQ.Settings.Accounts.ContainsKey(accountName) = True Then
                    Dim mAccount As EveHQAccount = HQ.Settings.Accounts.Item(accountName)
                    ' Add in the data for mailing lists
                    RaiseEvent MailProgress("Processing Mailing Lists for " & mPilot.Name & "...")
                    mailingListIDs = CustomDataFunctions.WriteMailingListIDsToDatabase(mPilot)
                    ' Make a call to the API to fetch the EveMail
                    RaiseEvent MailProgress("Fetching EveMails for " & mPilot.Name & "...")
                    Dim mailMessages As EveServiceResponse(Of IEnumerable(Of MailHeader)) = HQ.ApiProvider.Character.MailMessages(mAccount.UserID, mAccount.APIKey, Integer.Parse(mPilot.ID))
                    If mailMessages.ResultData IsNot Nothing Then
                        ' Stage 2: Populate the class with our EveMails

                        If mailMessages.ResultData.Any() Then
                            Dim mailIDs As New List(Of String)
                            For Each message As MailHeader In mailMessages.ResultData
                                Dim nMail As New EveHQ.Core.EveMailMessage
                                nMail.OriginatorID = CLng(mPilot.ID)
                                nMail.MessageID = message.MessageId
                                nMail.SenderID = message.SenderId
                                nMail.MessageDate = message.SentDate.DateTime
                                nMail.MessageTitle = message.Title
                                nMail.ToCharacterIDs = String.Join(",", message.ToCharacterIds())
                                nMail.ToCorpAllianceIDs = String.Join(",", message.ToCorpOrAllianceId)
                                nMail.ToListIDs = String.Join(",", message.ToListListIds())
                                ' Set mail flag according to whether the pilot is the sender i.e. flag sent mail as read
                                If nMail.SenderID = nMail.OriginatorID Then
                                    nMail.ReadFlag = True
                                Else
                                    nMail.ReadFlag = False
                                End If
                                nMail.MessageKey = nMail.MessageID.ToString & "_" & nMail.OriginatorID.ToString
                                If mails.ContainsKey(nMail.MessageKey) = False Then
                                    mails.Add(nMail.MessageKey, nMail)
                                End If
                                mailIDs.Add(nMail.MessageID.ToString)
                            Next
                            ' Get the mail bodies
                            If mailIDs.Count > 0 Then
                                Dim idsToQuery As List(Of Integer) = (From id In mailIDs Select Integer.Parse(id)).ToList()

                                Dim bodies As EveServiceResponse(Of IEnumerable(Of MailBody)) = HQ.ApiProvider.Character.MailBodies(mAccount.UserID, mAccount.APIKey, Integer.Parse(mPilot.ID), idsToQuery)

                                
                                If bodies IsNot Nothing Then
                                    If bodies.ResultData IsNot Nothing Then
                                        If bodies.ResultData.Any() Then
                                            For Each body As MailBody In bodies.ResultData
                                                Dim searchKey As String = body.MessageId & "_" & mPilot.ID
                                                If mails.ContainsKey(searchKey) = True Then
                                                    mails(searchKey).MessageBody = body.Body
                                                End If
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        ' Set the cache time
                        Dim cacheTime As Date = mailMessages.CacheUntil.DateTime
                        If cacheTime < HQ.NextAutoMailAPITime And cacheTime > Now Then
                            HQ.NextAutoMailAPITime = cacheTime
                        End If
                    End If

                End If
            End If
        Next

        ' Stage 3: Check the messages which have already been posted
        RaiseEvent MailProgress("Checking for new EveMails for all characters...")
        Dim existingMails As New ArrayList
        Dim strExistingMails As New StringBuilder
        If mails.Count > 0 Then
            For Each messageKey As String In mails.Keys
                strExistingMails.Append(",'" & messageKey & "'")
            Next
            strExistingMails.Remove(0, 1)
            Dim strSQL As String = "SELECT messageKey FROM eveMail WHERE messageKey IN (" & strExistingMails.ToString & ");"
            Dim mailData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
            If mailData IsNot Nothing Then
                If mailData.Tables(0).Rows.Count > 0 Then
                    For Each mailRow As DataRow In mailData.Tables(0).Rows
                        existingMails.Add(mailRow.Item("messageKey").ToString)
                    Next
                End If
            End If
        End If

        ' Stage 4: Post all new messages to the database
        RaiseEvent MailProgress("Posting new EveMails to the database...")
        Dim newMails As New ArrayList
        Const strInsert As String = "INSERT INTO eveMail (messageKey, messageID, originatorID, senderID, sentDate, title, toCorpOrAllianceID, toCharacterIDs, toListIDs, readMail, messageBody) VALUES "
        For Each mailKey As String In mails.Keys
            Dim cMail As EveMailMessage = mails(mailKey)
            If existingMails.Contains(mailKey) = False Then
                ' Add the message to the database
                Dim uSQL As New StringBuilder
                uSQL.Append(strInsert)
                uSQL.Append("(")
                uSQL.Append("'" & cMail.MessageKey & "', ")
                uSQL.Append(cMail.MessageID & ", ")
                uSQL.Append(cMail.OriginatorID & ", ")
                uSQL.Append(cMail.SenderID & ", ")
                uSQL.Append("'" & cMail.MessageDate.ToString(MailTimeFormat, _culture) & "', ")
                uSQL.Append("'" & cMail.MessageTitle.Replace("'", "''") & "', ")
                uSQL.Append("'" & cMail.ToCorpAllianceIDs & "', ")
                uSQL.Append("'" & cMail.ToCharacterIDs & "', ")
                uSQL.Append("'" & cMail.ToListIDs & "', ")
                uSQL.Append(CInt(cMail.ReadFlag) & ", ")
                If cMail.MessageBody IsNot Nothing Then
                    uSQL.Append("'" & cMail.MessageBody.Replace("'", "''") & "');")
                Else
                    uSQL.Append("'');")
                End If
                If CustomDataFunctions.SetCustomData(uSQL.ToString) = -2 Then
                    MessageBox.Show("There was an error writing data to the Eve Mail database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing EveMails", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' This may require an EveMail notification, so store it for later
                    newMails.Add(cMail)
                End If
            End If
        Next

        ' Stage 5: Get all the IDs and parse them
        RaiseEvent MailProgress("Posting EveMail IDs to the database...")
        Dim ids As New List(Of String)
        For Each cMail As EveMailMessage In mails.Values
            ' Get Sender IDs
            CustomDataFunctions.ParseIDs(ids, cMail.SenderID.ToString)
            ' Get Originator IDs
            CustomDataFunctions.ParseIDs(ids, cMail.OriginatorID.ToString)
            ' Get Character IDs
            CustomDataFunctions.ParseIDs(ids, cMail.ToCharacterIDs)
            ' Get Corp/Alliance IDs
            CustomDataFunctions.ParseIDs(ids, cMail.ToCorpAllianceIDs)
        Next
        ' Remove any mailing list IDs
        For Each mailingListID As Long In mailingListIDs.Keys
            If ids.Contains(mailingListID.ToString) = True Then
                ids.Remove(mailingListID.ToString)
            End If
        Next
        Call CustomDataFunctions.WriteEveIDsToDatabase(ids)

        ' Add in the Mailing List IDs
        For Each cMail As EveMailMessage In mails.Values
            CustomDataFunctions.ParseIDs(ids, cMail.ToListIDs)
        Next

        ' Send E-mail notification of new mails if required
        If HQ.Settings.NotifyEveMail = True And newMails.Count > 0 Then
            RaiseEvent MailProgress("Sending notification of new mails...")
            Call SendEmailForNewEveMails(newMails, ids)
        End If

        ' Just check the timer to make sure we're not gonna be bombarded with tons of short-lived requests!
        If HQ.NextAutoMailAPITime < Now.AddSeconds(60) Then
            HQ.NextAutoMailAPITime = HQ.NextAutoMailAPITime.AddSeconds(60)
        End If
    End Sub

    Private Sub GetNotifications()
        ' Stage 1: Download the latest EveNotifications API using the standard API method
        ' Stage 2: Populate the class with our Eve Notifications
        ' Stage 3: Check the messages which have already been posted
        ' Stage 4: Post all new messages to the database
        ' Stage 5: Get all the IDs and parse them

        ' Add to the auto timer
        HQ.NextAutoMailAPITime = Now.AddMinutes(30)
        Dim notices As New SortedList(Of String, EveNotification)
        For Each mPilot As EveHQPilot In HQ.Settings.Pilots.Values
            ' Stage 1: Download the latest EveMail API using the standard API method
            If mPilot.Active = True Then
                Dim accountName As String = mPilot.Account
                If HQ.Settings.Accounts.ContainsKey(accountName) = True Then
                    Dim mAccount As EveHQAccount = HQ.Settings.Accounts.Item(accountName)
                    ' Make a call to the API to fetch the EveMail
                    RaiseEvent MailProgress("Fetching Eve Notifications for " & mPilot.Name & "...")

                    Dim notifications As EveServiceResponse(Of IEnumerable(Of Notification)) = HQ.ApiProvider.Character.Notifications(mAccount.UserID, mAccount.APIKey, Integer.Parse(mPilot.ID))

                    If notifications IsNot Nothing Then
                        If notifications.ResultData IsNot Nothing Then
                            If notifications.ResultData.Any() Then
                                Dim notificationIds As New List(Of String)
                                For Each notification As Notification In notifications.ResultData
                                    Dim nMail As New EveNotification
                                    nMail.OriginatorID = CLng(mPilot.ID)
                                    nMail.MessageID = notification.NotificationId
                                    nMail.SenderID = notification.SenderId
                                    nMail.MessageDate = notification.SentDate.DateTime
                                    nMail.TypeID = notification.TypeId
                                    nMail.ReadFlag = notification.IsRead
                                    nMail.MessageKey = nMail.MessageID.ToString & "_" & nMail.OriginatorID.ToString
                                    If notices.ContainsKey(nMail.MessageKey) = False Then
                                        notices.Add(nMail.MessageKey, nMail)
                                    End If
                                    notificationIds.Add(nMail.MessageID.ToString)
                                Next

                                ' Get the notification bodies
                                If notificationIds.Count > 0 Then

                                    Dim idsToQuery As List(Of Long) = (From id In notificationIds Select Long.Parse(id)).ToList()

                                    Dim notificationTextResponse As EveServiceResponse(Of IEnumerable(Of NotificationText)) = HQ.ApiProvider.Character.NotificationTexts(mAccount.UserID, mAccount.APIKey, Integer.Parse(mPilot.ID), idsToQuery)

                                    If notificationTextResponse IsNot Nothing Then
                                        If notificationTextResponse.IsSuccess And notificationTextResponse.ResultData IsNot Nothing Then
                                            
                                            If notificationTextResponse.ResultData.Any() Then

                                                For Each body As NotificationText In notificationTextResponse.ResultData
                                                    Dim searchKey As String = body.NotificationId & "_" & mPilot.ID
                                                    If notices.ContainsKey(searchKey) = True Then
                                                        notices(searchKey).MessageBody = body.Text
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If

                            End If
                            ' Set the cache time
                            Dim cacheTime As Date = notifications.CacheUntil.DateTime
                            If cacheTime < HQ.NextAutoMailAPITime And cacheTime > Now Then
                                HQ.NextAutoMailAPITime = cacheTime
                            End If
                        End If
                    End If
                End If
            End If
        Next

        ' Stage 3: Check the messages which have already been posted
        RaiseEvent MailProgress("Checking for new Eve Notifications for all characters...")
        Dim existingMails As New ArrayList
        Dim strExistingMails As New StringBuilder
        If notices.Count > 0 Then
            For Each messageKey As String In notices.Keys
                strExistingMails.Append(",'" & messageKey & "'")
            Next
            strExistingMails.Remove(0, 1)
            Dim strSQL As String = "SELECT messageKey FROM eveNotifications WHERE messageKey IN (" & strExistingMails.ToString & ");"
            Dim mailData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
            If mailData IsNot Nothing Then
                If mailData.Tables(0).Rows.Count > 0 Then
                    For Each mailRow As DataRow In mailData.Tables(0).Rows
                        existingMails.Add(mailRow.Item("messageKey").ToString)
                    Next
                End If
            End If
        End If

        ' Stage 4: Post all new messages to the database
        RaiseEvent MailProgress("Posting new Eve Notifications to the database...")
        Dim newNotifys As New ArrayList
        Const strInsert As String = "INSERT INTO eveNotifications (messageKey, messageID, originatorID, senderID, typeID, sentDate, readMail, messageBody) VALUES "
        For Each noticeKey As String In notices.Keys
            Dim cMail As EveNotification = notices(noticeKey)
            If existingMails.Contains(cMail.MessageKey) = False Then
                ' Add the message to the database
                Dim uSQL As New StringBuilder
                uSQL.Append(strInsert)
                uSQL.Append("(")
                uSQL.Append("'" & cMail.MessageKey & "', ")
                uSQL.Append(cMail.MessageID & ", ")
                uSQL.Append(cMail.OriginatorID & ", ")
                uSQL.Append(cMail.SenderID & ", ")
                uSQL.Append(cMail.TypeID & ", ")
                uSQL.Append("'" & cMail.MessageDate.ToString(MailTimeFormat, _culture) & "', ")
                uSQL.Append(CInt(cMail.ReadFlag) & ", ")
                If cMail.MessageBody IsNot Nothing Then
                    uSQL.Append("'" & cMail.MessageBody.Replace("'", "''") & "');")
                Else
                    uSQL.Append("'');")
                End If
                If CustomDataFunctions.SetCustomData(uSQL.ToString) = -2 Then
                    MessageBox.Show("There was an error writing data to the Eve Notifications database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve Notifications", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' This may require an EveMail notification, so store it
                    newNotifys.Add(cMail)
                End If
            End If
        Next

        ' Stage 5: Get all the IDs and parse them
        RaiseEvent MailProgress("Posting Eve Notification IDs to the database...")
        Dim ids As New List(Of String)
        For Each cNotice As EveNotification In notices.Values
            ' Get Sender IDs
            CustomDataFunctions.ParseIDs(ids, cNotice.SenderID.ToString)
            ' Get Originator IDs
            CustomDataFunctions.ParseIDs(ids, cNotice.OriginatorID.ToString)
        Next
        Call CustomDataFunctions.WriteEveIDsToDatabase(ids)

        ' Send E-mail notification of new mails if required
        If HQ.Settings.NotifyEveMail = True And newNotifys.Count > 0 Then
            RaiseEvent MailProgress("Sending notification of new notices...")
            Call SendEmailForNewEveNotifications(newNotifys, ids)
        End If

        ' Just check the timer to make sure we're not gonna be bombarded with tons of short-lived requests!
        If HQ.NextAutoMailAPITime < Now.AddSeconds(60) Then
            HQ.NextAutoMailAPITime = HQ.NextAutoMailAPITime.AddSeconds(60)
        End If

    End Sub

    Private Sub SendEmailForNewEveMails(ByVal newMails As ArrayList, ByVal ds As IEnumerable(Of String))
        ' Get the name data from the DB
        Dim strID As New StringBuilder
        For Each id As String In ds
            If id <> "" Then
                strID.Append(id & ",")
            End If
        Next
        strID.Append("0")
        Dim finalIDs As New SortedList(Of Long, String)
        Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
        Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
        If idData IsNot Nothing Then
            If idData.Tables(0).Rows.Count > 0 Then
                For Each idRow As DataRow In idData.Tables(0).Rows
                    finalIDs.Add(CLng(idRow.Item("eveID")), CStr(idRow.Item("eveName")))
                Next
            End If
        End If
        Dim strBody As New StringBuilder
        strBody.AppendLine("EveHQ has collected the following new Eve mail messages:")
        strBody.AppendLine("")
        Dim messageTotal As Integer = 0
        For Each cMail As EveMailMessage In newMails
            If cMail.SenderID <> cMail.OriginatorID Then
                messageTotal += 1
            End If
        Next
        If messageTotal > 0 Then
            Dim messageCount As Integer = 1
            For Each cMail As EveMailMessage In newMails
                If cMail.SenderID <> cMail.OriginatorID Then
                    strBody.AppendLine("Mail " & messageCount.ToString & " of " & messageTotal.ToString)
                    strBody.AppendLine("From: " & finalIDs(cMail.SenderID))
                    If cMail.ToCharacterIDs <> "" Then
                        strBody.AppendLine("To: " & finalIDs(cMail.OriginatorID))
                    Else
                        If cMail.ToCorpAllianceIDs <> "" Then
                            strBody.AppendLine("To: " & finalIDs(CLng(cMail.ToCorpAllianceIDs)))
                        Else
                            strBody.AppendLine("To: Mailing List")
                        End If
                    End If
                    strBody.AppendLine("Date: " & cMail.MessageDate.ToString)
                    strBody.AppendLine("Subject: " & cMail.MessageTitle)
                    strBody.AppendLine("Message:")
                    strBody.AppendLine(cMail.MessageBody.Replace("<br>", "<br />").Replace("<BR>", "<br />").Replace("<br />", ControlChars.CrLf))
                    strBody.AppendLine("")
                    messageCount += 1
                End If
            Next
            Call SendEveHQMail("New Eve Mail Messages Notification", strBody.ToString)
        End If
    End Sub

    Private Sub SendEmailForNewEveNotifications(ByVal newNotifys As ArrayList, ByVal ids As IEnumerable(Of String))
        ' Get the name data from the DB
        Dim strID As New StringBuilder
        For Each id As String In ids
            If id <> "" Then
                strID.Append(id & ",")
            End If
        Next
        strID.Append("0")
        Dim finalIDs As New SortedList(Of Long, String)
        Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
        Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
        If idData IsNot Nothing Then
            If idData.Tables(0).Rows.Count > 0 Then
                For Each idRow As DataRow In idData.Tables(0).Rows
                    finalIDs.Add(CLng(idRow.Item("eveID")), CStr(idRow.Item("eveName")))
                Next
            End If
        End If
        Dim strBody As New StringBuilder
        strBody.AppendLine("EveHQ has collected the following new Eve notifications:")
        strBody.AppendLine("")
        Dim messageCount As Integer = 1
        For Each cMail As EveNotification In newNotifys
            strBody.AppendLine("Mail " & messageCount.ToString & " of " & newNotifys.Count.ToString)
            strBody.AppendLine("From: " & finalIDs(cMail.SenderID))
            strBody.AppendLine("To: " & finalIDs(cMail.OriginatorID))
            strBody.AppendLine("Date: " & cMail.MessageDate.ToString)
            Dim strNotice As String = [Enum].GetName(GetType(EveNotificationTypes), cMail.TypeID)
            strNotice = strNotice.Replace("_", " ")
            strBody.AppendLine("Subject: " & strNotice)
            strBody.AppendLine("")
            messageCount += 1
        Next
        Call SendEveHQMail("New Eve Notification Messages Notification", strBody.ToString)
    End Sub

    Private Sub SendEveHQMail(ByVal mailSubject As String, ByVal mailText As String)
        Dim evehqMail As New SmtpClient
        Try
            evehqMail.Host = HQ.Settings.EMailServer
            evehqMail.Port = HQ.Settings.EMailPort
            evehqMail.EnableSsl = HQ.Settings.UseSsl
            If HQ.Settings.UseSmtpAuth = True Then
                Dim newCredentials As New NetworkCredential
                newCredentials.UserName = HQ.Settings.EMailUsername
                newCredentials.Password = HQ.Settings.EMailPassword
                evehqMail.Credentials = newCredentials
            End If
            Dim recList As String = HQ.Settings.EMailAddress.Replace(ControlChars.CrLf, "").Replace(" ", "").Replace(";", ",")
            Dim evehqMsg As New MailMessage(HQ.Settings.EmailSenderAddress, recList)
            evehqMsg.Subject = mailSubject
            evehqMsg.Body = mailText
            evehqMail.Send(evehqMsg)
        Catch ex As Exception
            MessageBox.Show("The mail notification sending process failed. Please check that the server, port, address, username and password are correct." & ControlChars.CrLf & ControlChars.CrLf & "The error was: " & ex.Message, "EveHQ Email Notification Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
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
    Public MessageBody As String
End Class

Public Class EveNotification
    Public MessageKey As String
    Public MessageID As Long
    Public OriginatorID As Long
    Public TypeID As Long
    Public SenderID As Long
    Public MessageDate As Date
    Public ReadFlag As Boolean
    Public MessageBody As String
End Class

''' <summary>
''' Eve Notification types
''' IMPORTANT: Leave with underscores in to permit correct parsing
''' </summary>
''' <remarks></remarks>
Public Enum EveNotificationTypes As Integer
    ' ReSharper disable InconsistentNaming
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
    Corporation_Liquidation = 85
    Territorial_Claim_Unit_Under_Attack = 86
    Sovereignty_Blockade_Unit_Under_Attack = 87
    Infrastructure_Hub_Under_Attack = 88
    Contact_Notification = 89
    ' ReSharper restore InconsistentNaming
End Enum