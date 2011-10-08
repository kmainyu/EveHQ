' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Text
Imports System.Xml
Imports System.Windows.Forms

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

    Dim MailTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Public Event MailProgress(ByVal Status As String)

    Public Sub GetMail()
        Call Me.GetEveMail()
        Call Me.GetNotifications()
    End Sub

    Private Sub GetEveMail()
        ' Stage 1: Download the latest EveMail API using the standard API method
        ' Stage 2: Populate the class with our EveMail
        ' Stage 3: Check the messages which have already been posted
        ' Stage 4: Post all new messages to the database
        ' Stage 5: Get all the IDs and parse them

        ' Add to the auto timer
        EveHQ.Core.HQ.NextAutoMailAPITime = Now.AddMinutes(30)
        Dim Mails As New SortedList(Of String, EveHQ.Core.EveMailMessage)
        Dim MailingListIDs As New SortedList(Of Long, String)
        For Each mPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            ' Stage 1: Download the latest EveMail API using the standard API method
            If mPilot.Active = True Then
                Dim accountName As String = mPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim mAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                    ' Add in the data for mailing lists
                    RaiseEvent MailProgress("Processing Mailing Lists for " & mPilot.Name & "...")
                    MailingListIDs = EveHQ.Core.DataFunctions.WriteMailingListIDsToDatabase(mPilot)
                    ' Make a call to the EveHQ.Core.API to fetch the EveMail
                    RaiseEvent MailProgress("Fetching EveMails for " & mPilot.Name & "...")
                    Dim mailXML As New XmlDocument
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    mailXML = APIReq.GetAPIXML(EveAPI.APITypes.MailMessages, mAccount.ToAPIAccount, mPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
                    If mailXML IsNot Nothing Then
                        ' Stage 2: Populate the class with our EveMails
                        If mailXML.SelectNodes("/eveapi/error").Count = 0 Then
                            Dim mailList As XmlNodeList
                            Dim mail As XmlNode
                            mailList = mailXML.SelectNodes("/eveapi/result/rowset/row")
							If mailList.Count > 0 Then
								Dim mailIDs As New List(Of String)
								For Each mail In mailList
									Dim nMail As New EveHQ.Core.EveMailMessage
									nMail.OriginatorID = CLng(mPilot.ID)
									nMail.MessageID = CLng(mail.Attributes.GetNamedItem("messageID").Value)
									nMail.SenderID = CLng(mail.Attributes.GetNamedItem("senderID").Value)
									nMail.MessageDate = DateTime.ParseExact(mail.Attributes.GetNamedItem("sentDate").Value, MailTimeFormat, culture, Globalization.DateTimeStyles.None)
									nMail.MessageTitle = mail.Attributes.GetNamedItem("title").Value
									nMail.ToCharacterIDs = mail.Attributes.GetNamedItem("toCharacterIDs").Value
									nMail.ToCorpAllianceIDs = mail.Attributes.GetNamedItem("toCorpOrAllianceID").Value
                                    nMail.ToListIDs = mail.Attributes.GetNamedItem("toListID").Value
                                    ' Set mail flag according to whether the pilot is the sender i.e. flag sent mail as read
                                    If nMail.SenderID = nMail.OriginatorID Then
                                        nMail.ReadFlag = True
                                    Else
                                        nMail.ReadFlag = False
                                    End If
                                    nMail.MessageKey = nMail.MessageID.ToString & "_" & nMail.OriginatorID.ToString
									If Mails.ContainsKey(nMail.MessageKey) = False Then
										Mails.Add(nMail.MessageKey, nMail)
									End If
									mailIDs.Add(nMail.MessageID.ToString)
								Next
								' Get the mail bodies
								If mailIDs.Count > 0 Then
									Dim strID As New StringBuilder
									For Each ID As String In mailIDs
										strID.Append("," & ID)
									Next
									strID.Remove(0, 1)
									Dim IDList As String = strID.ToString
									Dim bodyXML As New XmlDocument
                                    Dim BodyReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
									bodyXML = BodyReq.GetAPIXML(EveAPI.APITypes.MailBodies, mAccount.ToAPIAccount, mPilot.ID, IDList, EveAPI.APIReturnMethods.ReturnActual)
									If bodyXML IsNot Nothing Then
										If bodyXML.SelectNodes("/eveapi/error").Count = 0 Then
											Dim bodyList As XmlNodeList
											Dim body As XmlNode
											bodyList = bodyXML.SelectNodes("/eveapi/result/rowset/row")
											If bodyList.Count > 0 Then
												For Each body In bodyList
													Dim searchKey As String = body.Attributes.GetNamedItem("messageID").Value & "_" & mPilot.ID
													If Mails.ContainsKey(searchKey) = True Then
														Mails(searchKey).MessageBody = body.ChildNodes(0).InnerText
													End If
												Next
											End If
										End If
									End If
								End If
							End If
							
                            ' Set the cache time
                            Dim CacheList As XmlNodeList
                            Dim cache As XmlNode
                            CacheList = mailXML.SelectNodes("/eveapi/cachedUntil")
                            cache = CacheList(0)
                            Dim cacheTime As Date = DateTime.ParseExact(cache.InnerText, MailTimeFormat, culture, Globalization.DateTimeStyles.None)
                            If cacheTime < EveHQ.Core.HQ.NextAutoMailAPITime And cacheTime > Now Then
                                EveHQ.Core.HQ.NextAutoMailAPITime = cacheTime
                            End If
                        End If
                    End If
                End If
            End If
        Next

        ' Stage 3: Check the messages which have already been posted
        RaiseEvent MailProgress("Checking for new EveMails for all characters...")
        Dim existingMails As New ArrayList
        Dim strExistingMails As New StringBuilder
        If Mails.Count > 0 Then
            For Each messageKey As String In Mails.Keys
                strExistingMails.Append(",'" & messageKey & "'")
            Next
            strExistingMails.Remove(0, 1)
            Dim strSQL As String = "SELECT messageKey FROM eveMail WHERE messageKey IN (" & strExistingMails.ToString & ");"
            Dim mailData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
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
		Dim NewMails As New ArrayList
		Dim strInsert As String = "INSERT INTO eveMail (messageKey, messageID, originatorID, senderID, sentDate, title, toCorpOrAllianceID, toCharacterIDs, toListIDs, readMail, messageBody) VALUES "
		For Each mailKey As String In Mails.Keys
			Dim cMail As EveHQ.Core.EveMailMessage = Mails(mailKey)
			If existingMails.Contains(mailKey) = False Then
				' Add the message to the database
				Dim uSQL As New StringBuilder
				uSQL.Append(strInsert)
				uSQL.Append("(")
				uSQL.Append("'" & cMail.MessageKey & "', ")
				uSQL.Append(cMail.MessageID & ", ")
				uSQL.Append(cMail.OriginatorID & ", ")
				uSQL.Append(cMail.SenderID & ", ")
                uSQL.Append("'" & cMail.MessageDate.ToString(MailTimeFormat, culture) & "', ")
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
				If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = False Then
					MessageBox.Show("There was an error writing data to the Eve Mail database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing EveMails", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				Else
					' This may require an EveMail notification, so store it for later
					NewMails.Add(cMail)
				End If
            End If
		Next

		' Stage 5: Get all the IDs and parse them
		RaiseEvent MailProgress("Posting EveMail IDs to the database...")
		Dim IDs As New List(Of String)
		For Each cMail As EveHQ.Core.EveMailMessage In Mails.Values
			' Get Sender IDs
			EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.SenderID.ToString)
			' Get Originator IDs
			EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.OriginatorID.ToString)
			' Get Character IDs
			EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.ToCharacterIDs)
			' Get Corp/Alliance IDs
			EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.ToCorpAllianceIDs)
        Next
        ' Remove any mailing list IDs
        For Each MailingListID As Long In MailingListIDs.Keys
            If IDs.Contains(MailingListID.ToString) = True Then
                IDs.Remove(MailingListID.ToString)
            End If
        Next
		Call EveHQ.Core.DataFunctions.WriteEveIDsToDatabase(IDs)

		' Add in the Mailing List IDs
		For Each cMail As EveHQ.Core.EveMailMessage In Mails.Values
			EveHQ.Core.DataFunctions.ParseIDs(IDs, cMail.ToListIDs)
		Next

		' Send E-mail notification of new mails if required
		If EveHQ.Core.HQ.EveHQSettings.NotifyEveMail = True And NewMails.Count > 0 Then
			RaiseEvent MailProgress("Sending notification of new mails...")
			Call SendEmailForNewEveMails(NewMails, IDs)
		End If

		' Just check the timer to make sure we're not gonna be bombarded with tons of short-lived requests!
		If EveHQ.Core.HQ.NextAutoMailAPITime < Now.AddSeconds(60) Then
			EveHQ.Core.HQ.NextAutoMailAPITime = EveHQ.Core.HQ.NextAutoMailAPITime.AddSeconds(60)
		End If
	End Sub

    Private Sub GetNotifications()
        ' Stage 1: Download the latest EveNotifications API using the standard API method
        ' Stage 2: Populate the class with our Eve Notifications
        ' Stage 3: Check the messages which have already been posted
        ' Stage 4: Post all new messages to the database
        ' Stage 5: Get all the IDs and parse them

        ' Add to the auto timer
        EveHQ.Core.HQ.NextAutoMailAPITime = Now.AddMinutes(30)
        Dim Notices As New SortedList(Of String, EveHQ.Core.EveNotification)
        For Each mPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            ' Stage 1: Download the latest EveMail API using the standard API method
            If mPilot.Active = True Then
                Dim accountName As String = mPilot.Account
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(accountName) = True Then
                    Dim mAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                    ' Make a call to the EveHQ.Core.API to fetch the EveMail
                    RaiseEvent MailProgress("Fetching Eve Notifications for " & mPilot.Name & "...")
                    Dim mailXML As New XmlDocument
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    mailXML = APIReq.GetAPIXML(EveAPI.APITypes.Notifications, mAccount.ToAPIAccount, mPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)
                    If mailXML IsNot Nothing Then
                        If mailXML.SelectNodes("/eveapi/error").Count = 0 Then
                            ' Stage 2: Populate the class with our EveMails
                            Dim mailList As XmlNodeList
                            Dim mail As XmlNode
                            mailList = mailXML.SelectNodes("/eveapi/result/rowset/row")
                            If mailList.Count > 0 Then
                                Dim mailIDs As New List(Of String)
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
                                    mailIDs.Add(nMail.MessageID.ToString)
                                Next

                                ' Get the notification bodies
                                If mailIDs.Count > 0 Then
                                    Dim strID As New StringBuilder
                                    For Each ID As String In mailIDs
                                        strID.Append("," & ID)
                                    Next
                                    strID.Remove(0, 1)
                                    Dim IDList As String = strID.ToString
                                    Dim bodyXML As New XmlDocument
                                    Dim BodyReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                                    bodyXML = BodyReq.GetAPIXML(EveAPI.APITypes.NotificationTexts, mAccount.ToAPIAccount, mPilot.ID, IDList, EveAPI.APIReturnMethods.ReturnActual)
                                    If bodyXML IsNot Nothing Then
                                        If bodyXML.SelectNodes("/eveapi/error").Count = 0 Then
                                            Dim bodyList As XmlNodeList
                                            Dim body As XmlNode
                                            bodyList = bodyXML.SelectNodes("/eveapi/result/rowset/row")
                                            If bodyList.Count > 0 Then
                                                For Each body In bodyList
                                                    Dim searchKey As String = body.Attributes.GetNamedItem("notificationID").Value & "_" & mPilot.ID
                                                    If Notices.ContainsKey(searchKey) = True Then
                                                        Notices(searchKey).MessageBody = body.ChildNodes(0).InnerText
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If

                            End If
                            ' Set the cache time
                            Dim CacheList As XmlNodeList
                            Dim cache As XmlNode
                            CacheList = mailXML.SelectNodes("/eveapi/cachedUntil")
                            cache = CacheList(0)
                            Dim cacheTime As Date = DateTime.ParseExact(cache.InnerText, MailTimeFormat, culture, Globalization.DateTimeStyles.None)
                            If cacheTime < EveHQ.Core.HQ.NextAutoMailAPITime And cacheTime > Now Then
                                EveHQ.Core.HQ.NextAutoMailAPITime = cacheTime
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
        If Notices.Count > 0 Then
            For Each messageKey As String In Notices.Keys
                strExistingMails.Append(",'" & messageKey & "'")
            Next
            strExistingMails.Remove(0, 1)
            Dim strSQL As String = "SELECT messageKey FROM eveNotifications WHERE messageKey IN (" & strExistingMails.ToString & ");"
            Dim mailData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
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
        Dim strInsert As String = "INSERT INTO eveNotifications (messageKey, messageID, originatorID, senderID, typeID, sentDate, readMail, messageBody) VALUES "
        For Each NoticeKey As String In Notices.Keys
            Dim cMail As EveHQ.Core.EveNotification = Notices(NoticeKey)
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
                uSQL.Append("'" & cMail.MessageDate.ToString(MailTimeFormat, culture) & "', ")
                uSQL.Append(CInt(cMail.ReadFlag) & ", ")
                If cMail.MessageBody IsNot Nothing Then
                    uSQL.Append("'" & cMail.MessageBody.Replace("'", "''") & "');")
                Else
                    uSQL.Append("'');")
                End If
                If EveHQ.Core.DataFunctions.SetData(uSQL.ToString) = False Then
                    MessageBox.Show("There was an error writing data to the Eve Notifications database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & uSQL.ToString, "Error Writing Eve Notifications", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' This may require an EveMail notification, so store it
                    newNotifys.Add(cMail)
                End If
            End If
        Next

        ' Stage 5: Get all the IDs and parse them
        RaiseEvent MailProgress("Posting Eve Notification IDs to the database...")
        Dim IDs As New List(Of String)
        For Each cNotice As EveHQ.Core.EveNotification In Notices.Values
            ' Get Sender IDs
            EveHQ.Core.DataFunctions.ParseIDs(IDs, cNotice.SenderID.ToString)
            ' Get Originator IDs
            EveHQ.Core.DataFunctions.ParseIDs(IDs, cNotice.OriginatorID.ToString)
        Next
        Call EveHQ.Core.DataFunctions.WriteEveIDsToDatabase(IDs)

        ' Send E-mail notification of new mails if required
        If EveHQ.Core.HQ.EveHQSettings.NotifyEveMail = True And newNotifys.Count > 0 Then
            RaiseEvent MailProgress("Sending notification of new notices...")
            Call SendEmailForNewEveNotifications(newNotifys, IDs)
        End If

        ' Just check the timer to make sure we're not gonna be bombarded with tons of short-lived requests!
        If EveHQ.Core.HQ.NextAutoMailAPITime < Now.AddSeconds(60) Then
            EveHQ.Core.HQ.NextAutoMailAPITime = EveHQ.Core.HQ.NextAutoMailAPITime.AddSeconds(60)
        End If

    End Sub

    Private Sub SendEmailForNewEveMails(ByVal NewMails As ArrayList, ByVal IDs As List(Of String))
        ' Get the name data from the DB
        Dim strID As New StringBuilder
        For Each ID As String In IDs
            If ID <> "" Then
                strID.Append(ID & ",")
            End If
        Next
        strID.Append("0")
        Dim FinalIDs As New SortedList(Of Long, String)
        Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
        Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        If IDData IsNot Nothing Then
            If IDData.Tables(0).Rows.Count > 0 Then
                For Each IDRow As DataRow In IDData.Tables(0).Rows
                    FinalIDs.Add(CLng(IDRow.Item("eveID")), CStr(IDRow.Item("eveName")))
                Next
            End If
        End If
        Dim strBody As New StringBuilder
        strBody.AppendLine("EveHQ has collected the following new Eve mail messages:")
        strBody.AppendLine("")
        Dim messageTotal As Integer = 0
        For Each cMail As EveHQ.Core.EveMailMessage In NewMails
            If cMail.SenderID <> cMail.OriginatorID Then
                messageTotal += 1
            End If
        Next
        If messageTotal > 0 Then
            Dim MessageCount As Integer = 1
            For Each cMail As EveHQ.Core.EveMailMessage In NewMails
                If cMail.SenderID <> cMail.OriginatorID Then
                    strBody.AppendLine("Mail " & MessageCount.ToString & " of " & messageTotal.ToString)
                    strBody.AppendLine("From: " & FinalIDs(cMail.SenderID))
                    If cMail.ToCharacterIDs <> "" Then
                        strBody.AppendLine("To: " & FinalIDs(cMail.OriginatorID))
                    Else
                        If cMail.ToCorpAllianceIDs <> "" Then
                            strBody.AppendLine("To: " & FinalIDs(CLng(cMail.ToCorpAllianceIDs)))
                        Else
                            strBody.AppendLine("To: Mailing List")
                        End If
                    End If
                    strBody.AppendLine("Date: " & cMail.MessageDate.ToString)
					strBody.AppendLine("Subject: " & cMail.MessageTitle)
					strBody.AppendLine("Message:")
					strBody.AppendLine(cMail.MessageBody.Replace("<br>", "<br />").Replace("<BR>", "<br />").Replace("<br />", ControlChars.CrLf))
					strBody.AppendLine("")
                    MessageCount += 1
                End If
            Next
            Call SendEveHQMail("New Eve Mail Messages Notification", strBody.ToString)
        End If
    End Sub

    Private Sub SendEmailForNewEveNotifications(ByVal NewNotifys As ArrayList, ByVal IDs As List(Of String))
        ' Get the name data from the DB
        Dim strID As New StringBuilder
        For Each ID As String In IDs
            If ID <> "" Then
                strID.Append(ID & ",")
            End If
        Next
        strID.Append("0")
        Dim FinalIDs As New SortedList(Of Long, String)
        Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
        Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        If IDData IsNot Nothing Then
            If IDData.Tables(0).Rows.Count > 0 Then
                For Each IDRow As DataRow In IDData.Tables(0).Rows
                    FinalIDs.Add(CLng(IDRow.Item("eveID")), CStr(IDRow.Item("eveName")))
                Next
            End If
        End If
        Dim strBody As New StringBuilder
        strBody.AppendLine("EveHQ has collected the following new Eve notifications:")
        strBody.AppendLine("")
        Dim MessageCount As Integer = 1
        For Each cMail As EveHQ.Core.EveNotification In NewNotifys
            strBody.AppendLine("Mail " & MessageCount.ToString & " of " & NewNotifys.Count.ToString)
            strBody.AppendLine("From: " & FinalIDs(cMail.SenderID))
            strBody.AppendLine("To: " & FinalIDs(cMail.OriginatorID))
            strBody.AppendLine("Date: " & cMail.MessageDate.ToString)
            Dim strNotice As String = [Enum].GetName(GetType(EveHQ.Core.EveNotificationTypes), cMail.TypeID)
            strNotice = strNotice.Replace("_", " ")
            strBody.AppendLine("Subject: " & strNotice)
            strBody.AppendLine("")
            MessageCount += 1
        Next
        Call SendEveHQMail("New Eve Notification Messages Notification", strBody.ToString)
    End Sub

    Private Sub SendEveHQMail(ByVal mailSubject As String, ByVal mailText As String)
        Dim eveHQMail As New System.Net.Mail.SmtpClient
        Try
            eveHQMail.Host = EveHQ.Core.HQ.EveHQSettings.EMailServer
            eveHQMail.Port = EveHQ.Core.HQ.EveHQSettings.EMailPort
            eveHQMail.EnableSsl = EveHQ.Core.HQ.EveHQSettings.UseSSL
            If EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = True Then
                Dim newCredentials As New System.Net.NetworkCredential
                newCredentials.UserName = EveHQ.Core.HQ.EveHQSettings.EMailUsername
                newCredentials.Password = EveHQ.Core.HQ.EveHQSettings.EMailPassword
                eveHQMail.Credentials = newCredentials
            End If
            Dim recList As String = EveHQ.Core.HQ.EveHQSettings.EMailAddress.Replace(ControlChars.CrLf, "").Replace(" ", "").Replace(";", ",")
            Dim eveHQMsg As New System.Net.Mail.MailMessage(EveHQ.Core.HQ.EveHQSettings.EmailSenderAddress, recList)
            eveHQMsg.Subject = mailSubject
            eveHQMsg.Body = mailText
            eveHQMail.Send(eveHQMsg)
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
