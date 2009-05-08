Imports System.Windows.Forms
Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Net

Public Class frmKMV

    Dim KillTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Dim KMAccount As New EveHQ.Core.EveAccount
    Dim charName As String = ""
    Dim charID As String = ""
    Dim KMs As New SortedList

#Region "Form Loading Routines"
    Private Sub frmKMV_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.UpdateAccounts()
    End Sub
    Private Sub UpdateAccounts()
        cboAccount.BeginUpdate()
        cboAccount.Items.Clear()
        For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            If cAccount.FriendlyName <> "" Then
                cboAccount.Items.Add(cAccount.FriendlyName)
            Else
                cboAccount.Items.Add(cAccount.userID)
            End If
        Next
        cboAccount.EndUpdate()
    End Sub
#End Region

#Region "Account Select Method Routines"
    Private Sub radUseAccount_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radUseAccount.CheckedChanged
        cboAccount.Enabled = radUseAccount.Checked
    End Sub

    Private Sub radUseAPI_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radUseAPI.CheckedChanged
        lblUserID.Enabled = radUseAPI.Checked
        txtUserID.Enabled = radUseAPI.Checked
        lblAPIKey.Enabled = radUseAPI.Checked
        txtAPIKey.Enabled = radUseAPI.Checked
        lblAPIStatus.Enabled = radUseAPI.Checked
        btnGetCharacters.Enabled = radUseAPI.Checked
    End Sub

    Private Sub cboAccount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAccount.SelectedIndexChanged

        ' Get the accountID and pilots
        For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            If cAccount.FriendlyName = cboAccount.SelectedItem.ToString Or cAccount.userID = cboAccount.SelectedItem.ToString Then
                KMAccount = cAccount
                ' Get the list of characters and the character IDs
                If cAccount.Characters IsNot Nothing Then
                    lvwCharacters.BeginUpdate()
                    lvwCharacters.Items.Clear()
                    For Each Character As String In cAccount.Characters
                        Dim newPilot As New ListViewItem
                        newPilot.Text = Character
                        newPilot.Name = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Character), EveHQ.Core.Pilot).ID
                        lvwCharacters.Items.Add(newPilot)
                    Next
                    lvwCharacters.Sort()
                    lvwCharacters.EndUpdate()
                    ' Enable the Fetch Killmails button
                    btnFetchKillMails.Enabled = True
                    Exit For
                Else
                    MessageBox.Show("The list of characaters in this account is blank. Please connect to the API to get the latest character information.", "Characters required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Next

    End Sub

#End Region

#Region "Character Retrieval Routines"

    Private Sub btnGetCharacters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetCharacters.Click

        ' Check for blank UserID and APIKey
        If txtUserID.Text = "" Or txtAPIKey.Text = "" Then
            MessageBox.Show("UserID and APIKey cannot be blank. Please enter the required information.", "API Information Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Create a dummy account for the information
        KMAccount = New EveHQ.Core.EveAccount
        KMAccount.userID = txtUserID.Text
        KMAccount.APIKey = txtAPIKey.Text
        KMAccount.FriendlyName = "Killmail Viewing Account"

        ' Create an XML document for retrieving characters
        Dim CharactersXML As New XmlDocument
        CharactersXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Characters, KMAccount, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)

        ' Check for errors
        If EveHQ.Core.EveAPI.LastAPIErrorText <> "" Then
            Select Case EveHQ.Core.EveAPI.LastAPIResult
                Case EveHQ.Core.EveAPI.APIResults.APIServerDownReturnedCached, EveHQ.Core.EveAPI.APIResults.APIServerDownReturnedNull, EveHQ.Core.EveAPI.APIResults.CCPError, EveHQ.Core.EveAPI.APIResults.PageNotFound, EveHQ.Core.EveAPI.APIResults.TimedOut, EveHQ.Core.EveAPI.APIResults.UnknownError
                    MessageBox.Show("There was an error retrieving character information from the API. The error was " & EveHQ.Core.EveAPI.LastAPIErrorText, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    lblAPIStatus.Text = "API Status: Character retrieval failed." : lblAPIStatus.Refresh()
                    Exit Sub
            End Select
        End If

        If CharactersXML IsNot Nothing Then
            ' Seems ok so let's add the characters to the list
            Dim CharacterList As XmlNodeList
            Dim Character As XmlNode

            ' Get the list of characters and the character IDs
            CharacterList = CharactersXML.SelectNodes("/eveapi/result/rowset/row")
            lvwCharacters.BeginUpdate()
            lvwCharacters.Items.Clear()
            For Each Character In CharacterList
                Dim newPilot As New ListViewItem
                newPilot.Text = Character.Attributes.GetNamedItem("name").Value
                newPilot.Name = Character.Attributes.GetNamedItem("characterID").Value
                lvwCharacters.Items.Add(newPilot)
            Next
            lvwCharacters.Sort()
            lvwCharacters.EndUpdate()

            ' Enable the Fetch Killmails button
            btnFetchKillMails.Enabled = True
            lblAPIStatus.Text = "API Status: Successfully retrieved characters." : lblAPIStatus.Refresh()
        End If

    End Sub

#End Region

#Region "Fetch KillMails Routines"

    Private Sub btnFetchKillMails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFetchKillMails.Click

        ' Check we have a character selected
        If lvwCharacters.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select a character before fetching the killmails", "Character required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Get the name and ID of the character
        charName = lvwCharacters.SelectedItems(0).Text
        charID = lvwCharacters.SelectedItems(0).Name

        ' Clear the list of Killmails
        KMs.Clear()
        Dim allKMsDownloaded As Boolean = False
        Dim lastKillID As String = ""
        Dim KMXML As New XmlDocument

        Do
            ' Let's try and get the killmail details (use the standard caching method for this)
            If chkUseCorp.Checked = True Then
                KMXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.KillLogCorp, KMAccount, charID, lastKillID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
            Else
                KMXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.KillLogChar, KMAccount, charID, lastKillID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
            End If

            ' Check for any Errors
            If KMXML IsNot Nothing Then
                ' Check XML for any error codes (do we have the full API key?)
                Dim errlist As XmlNodeList = KMXML.SelectNodes("/eveapi/error")
                If errlist.Count <> 0 Then
                    Dim errNode As XmlNode = errlist(0)
                    ' Get error code
                    Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                    Dim errMsg As String = errNode.InnerText
                    'MessageBox.Show("There was an error retrieving the Killmail API Data. The error was: " & errMsg, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    allKMsDownloaded = True
                Else
                    ' Parse the current killmail XML
                    Call ParseKMXML(KMXML)
                    ' Get the last killmailID
                    Dim KMList As XmlNodeList = KMXML.SelectNodes("/eveapi/result/rowset/row")
                    If KMList.Count > 0 Then
                        lastKillID = KMList(KMList.Count - 1).Attributes.GetNamedItem("killID").Value
                    Else
                        allKMsDownloaded = True
                    End If
                End If
            Else
                MessageBox.Show("A null XML document was returned. Check the API Server and internet connection.", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Loop Until allKMsDownloaded = True

        ' Do a summary of the killmails
        Call DrawKMSummary()

    End Sub

#End Region

#Region "Killmail XML Parsing Routines"

    Private Sub ParseKMXML(ByVal KM As XmlDocument)

        Dim KMList As XmlNodeList
        Dim KMInfo As XmlNode

        ' Get the list of characters and the character IDs
        KMList = KM.SelectNodes("/eveapi/result/rowset/row")

        For Each KMInfo In KMList
            ' Start a new KM class
            Dim newKM As New KillMail
            newKM.killID = KMInfo.Attributes.GetNamedItem("killID").Value
            newKM.systemID = KMInfo.Attributes.GetNamedItem("solarSystemID").Value
            newKM.killTime = DateTime.ParseExact(KMInfo.Attributes.GetNamedItem("killTime").Value, KillTimeFormat, culture, Globalization.DateTimeStyles.None)
            newKM.moonID = KMInfo.Attributes.GetNamedItem("moonID").Value

            ' Parse the victim data
            Dim newVictim As New KMVictim
            With KMInfo.ChildNodes(0)
                newVictim.charID = .Attributes.GetNamedItem("characterID").Value
                newVictim.charName = .Attributes.GetNamedItem("characterName").Value
                newVictim.corpID = .Attributes.GetNamedItem("corporationID").Value
                newVictim.corpName = .Attributes.GetNamedItem("corporationName").Value
                newVictim.allianceID = .Attributes.GetNamedItem("allianceID").Value
                newVictim.allianceName = .Attributes.GetNamedItem("allianceName").Value
                newVictim.factionID = .Attributes.GetNamedItem("factionID").Value
                newVictim.factionName = .Attributes.GetNamedItem("factionName").Value
                newVictim.damageTaken = Double.Parse(.Attributes.GetNamedItem("damageTaken").Value, Globalization.NumberStyles.Number, culture)
                newVictim.shipTypeID = .Attributes.GetNamedItem("shipTypeID").Value
            End With
            newKM.Victim = newVictim

            ' Parse the attackers data
            For Each attackerNode As XmlNode In KMInfo.ChildNodes(1).ChildNodes
                Dim newAttacker As New KMAttacker
                newAttacker.charID = attackerNode.Attributes.GetNamedItem("characterID").Value
                newAttacker.charName = attackerNode.Attributes.GetNamedItem("characterName").Value
                newAttacker.corpID = attackerNode.Attributes.GetNamedItem("corporationID").Value
                newAttacker.corpName = attackerNode.Attributes.GetNamedItem("corporationName").Value
                newAttacker.allianceID = attackerNode.Attributes.GetNamedItem("allianceID").Value
                newAttacker.allianceName = attackerNode.Attributes.GetNamedItem("allianceName").Value
                newAttacker.factionID = attackerNode.Attributes.GetNamedItem("factionID").Value
                newAttacker.factionName = attackerNode.Attributes.GetNamedItem("factionName").Value
                newAttacker.secStatus = Double.Parse(attackerNode.Attributes.GetNamedItem("securityStatus").Value, Globalization.NumberStyles.Number, culture)
                newAttacker.damageDone = Double.Parse(attackerNode.Attributes.GetNamedItem("damageDone").Value, Globalization.NumberStyles.Number, culture)
                newAttacker.finalBlow = CBool(attackerNode.Attributes.GetNamedItem("finalBlow").Value)
                newAttacker.weaponTypeID = attackerNode.Attributes.GetNamedItem("weaponTypeID").Value
                newAttacker.shipTypeID = attackerNode.Attributes.GetNamedItem("shipTypeID").Value
                ' Setup the key for sorting
                Dim key As String = Format(newAttacker.damageDone, "0000000000") & newAttacker.charName & newAttacker.corpID
                newKM.Attackers.Add(key, newAttacker)
            Next

            ' Parse the item data
            For Each itemNode As XmlNode In KMInfo.ChildNodes(2).ChildNodes
                Dim newItem As New KMItem
                newItem.typeID = itemNode.Attributes.GetNamedItem("typeID").Value
                newItem.flag = Integer.Parse(itemNode.Attributes.GetNamedItem("flag").Value, Globalization.NumberStyles.Number, culture)
                newItem.qtyDropped = Integer.Parse(itemNode.Attributes.GetNamedItem("qtyDropped").Value, Globalization.NumberStyles.Number, culture)
                newItem.qtyDestroyed = Integer.Parse(itemNode.Attributes.GetNamedItem("qtyDestroyed").Value, Globalization.NumberStyles.Number, culture)
                newKM.Items.Add(newItem)
            Next

            ' Add the killmail to the collection
            If KMs.Contains(newKM.killID) = False Then
                KMs.Add(newKM.killID, newKM)
            End If

        Next

    End Sub

#End Region

#Region "Killmail Summary Routines"
    Private Sub DrawKMSummary()
        ' Update the list of killmails
        lvwKillMails.BeginUpdate()
        lvwKillMails.Items.Clear()
        For Each charKM As KillMail In KMs.Values
            Dim newKM As New ListViewItem
            newKM.Name = charKM.killID
            newKM.ToolTipText = "KillID: " & charKM.killID
            newKM.Text = charKM.Victim.charName
            newKM.SubItems.Add(CType(EveHQ.Core.HQ.itemData(charKM.Victim.shipTypeID), EveHQ.Core.EveItem).Name)
            newKM.SubItems.Add(FormatDateTime(charKM.killTime, DateFormat.GeneralDate))
            lvwKillMails.Items.Add(newKM)
        Next
        lvwKillMails.EndUpdate()
        ' Update the summary label
        If chkUseCorp.Checked = True Then
            lblKMSummary.Text = "Corp Killmail Summary for " & charName & " (" & lvwKillMails.Items.Count & " items)"
        Else
            lblKMSummary.Text = "Killmail Summary for " & charName & " (" & lvwKillMails.Items.Count & " items)"
        End If
        ' Clear the killmail detail text
        txtKillMailDetails.Text = ""
    End Sub
#End Region

#Region "Killmail Detail Routines"

    Private Sub lvwKillMails_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwKillMails.SelectedIndexChanged
        If lvwKillMails.SelectedItems.Count > 0 Then
            ' Get the killID of the selected Killmail
            Dim killID As String = lvwKillMails.SelectedItems(0).Name
            Dim selKM As KillMail = CType(KMs(killID), KillMail)
            ' Write the killmail detail
            Call DrawKMDetail(selKM)
        End If

    End Sub

    Private Sub DrawKMDetail(ByVal selKM As KillMail)

        ' Write the killmail text to the label
        txtKillMailDetails.Text = BuildKMDetails(selKM)

    End Sub

    Private Function BuildKMDetails(ByVal selKM As KillMail) As String
        Dim KMText As New StringBuilder

        ' Write the time
        KMText.AppendLine(Format(selKM.killTime, "yyyy.MM.dd HH:mm:ss"))
        KMText.AppendLine("")

        ' Get the solar system details
        Dim ss As SolarSystem = PlugInData.Systems(selKM.systemID)

        ' Write the victim details
        KMText.AppendLine("Victim: " & selKM.Victim.charName)
        KMText.AppendLine("Corp: " & selKM.Victim.corpName)
        If selKM.Victim.allianceName = "" Then
            KMText.AppendLine("Alliance: NONE")
        Else
            KMText.AppendLine("Alliance: " & selKM.Victim.allianceName)
        End If
        If selKM.Victim.factionName = "" Then
            KMText.AppendLine("Faction: NONE")
        Else
            KMText.AppendLine("Faction: " & selKM.Victim.factionName)
        End If
        KMText.AppendLine("Destroyed: " & CType(EveHQ.Core.HQ.itemData(selKM.Victim.shipTypeID), EveHQ.Core.EveItem).Name)
        KMText.AppendLine("System: " & ss.Name)
        KMText.AppendLine("Security: " & FormatNumber(Math.Max(ss.Security, 0), 1))
        KMText.AppendLine("Damage Taken: " & selKM.Victim.damageTaken.ToString)

        ' Put the attackers into the correct order
        Dim attackers As New ArrayList
        For Each attacker As KMAttacker In selKM.Attackers.Values
            attackers.Add(attacker)
        Next
        attackers.Reverse()

        ' Write the Attackers details
        KMText.AppendLine("")
        KMText.AppendLine("Involved parties:")
        KMText.AppendLine("")
        For Each attacker As KMAttacker In attackers
            If attacker.charName <> "" Then
                KMText.Append("Name: " & attacker.charName)
                If attacker.finalBlow = True Then
                    KMText.AppendLine(" (laid the final blow)")
                Else
                    KMText.AppendLine("")
                End If
                KMText.AppendLine("Security: " & FormatNumber(attacker.secStatus, 1))
                KMText.AppendLine("Corp: " & attacker.corpName)
                If attacker.allianceName = "" Then
                    KMText.AppendLine("Alliance: NONE")
                Else
                    KMText.AppendLine("Alliance: " & attacker.allianceName)
                End If
                If attacker.factionName = "" Then
                    KMText.AppendLine("Faction: NONE")
                Else
                    KMText.AppendLine("Faction: " & attacker.factionName)
                End If
                KMText.AppendLine("Ship: " & CType(EveHQ.Core.HQ.itemData(attacker.shipTypeID), EveHQ.Core.EveItem).Name)
                KMText.AppendLine("Weapon: " & CType(EveHQ.Core.HQ.itemData(attacker.weaponTypeID), EveHQ.Core.EveItem).Name)
            Else
                If attacker.corpName = "" Then
                    KMText.Append("Name: " & CType(EveHQ.Core.HQ.itemData(attacker.shipTypeID), EveHQ.Core.EveItem).Name & " / Unknown")
                Else
                    KMText.Append("Name: " & CType(EveHQ.Core.HQ.itemData(attacker.shipTypeID), EveHQ.Core.EveItem).Name & " / " & attacker.corpName)
                End If
                If attacker.finalBlow = True Then
                    KMText.AppendLine(" (laid the final blow)")
                Else
                    KMText.AppendLine("")
                End If
            End If
            KMText.AppendLine("Damage Done: " & attacker.damageDone.ToString)
            KMText.AppendLine("")
        Next

        ' Make a list of dropped and destroyed items
        Dim droppedItems, destroyedItems As New ArrayList
        Dim itemName As String = ""
        For Each item As KMItem In selKM.Items
            itemName = CType(EveHQ.Core.HQ.itemData(item.typeID), EveHQ.Core.EveItem).Name
            If item.qtyDestroyed > 0 Then
                If item.qtyDestroyed = 1 Then
                    If item.flag = 0 Then
                        destroyedItems.Add(itemName)
                    Else
                        destroyedItems.Add(itemName & " (Cargo)")
                    End If
                Else
                    If item.flag = 0 Then
                        destroyedItems.Add(itemName & ", Qty:" & item.qtyDestroyed.ToString)
                    Else
                        destroyedItems.Add(itemName & ", Qty:" & item.qtyDestroyed.ToString & " (Cargo)")
                    End If
                End If
            End If
            If item.qtyDropped > 0 Then
                If item.qtyDropped = 1 Then
                    If item.flag = 0 Then
                        droppedItems.Add(itemName)
                    Else
                        droppedItems.Add(itemName & " (Cargo)")
                    End If
                Else
                    If item.flag = 0 Then
                        droppedItems.Add(itemName & ", Qty:" & item.qtyDropped.ToString)
                    Else
                        droppedItems.Add(itemName & ", Qty:" & item.qtyDropped.ToString & " (Cargo)")
                    End If
                End If
            End If
        Next

        ' Write the Destroyed items if applicable
        If destroyedItems.Count > 0 Then
            KMText.AppendLine("Destroyed items:")
            KMText.AppendLine("")
            For Each dItem As String In destroyedItems
                KMText.AppendLine(dItem)
            Next
            KMText.AppendLine("")
        End If

        ' Write the Dropped items if applicable
        If droppedItems.Count > 0 Then
            KMText.AppendLine("Dropped items:")
            KMText.AppendLine("")
            For Each dItem As String In droppedItems
                KMText.AppendLine(dItem)
            Next
            KMText.AppendLine("")
        End If

        Return KMText.ToString

    End Function
#End Region

#Region "Killmail Upload Routines"
    Private Sub btnUploadToBC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUploadToBC.Click
        ' Only do selected KM for now for testing purposes
        Dim URI As String = "http://www.battleclinic.com/eve_online/pk/submit.php"

        If lvwKillMails.SelectedItems.Count > 0 Then
            ' Get the killID of the selected Killmail
            Dim killID As String = lvwKillMails.SelectedItems(0).Name
            Dim selKM As KillMail = CType(KMs(killID), KillMail)
            ' Check for valid attackers (i.e. not all NPC ones)
            Dim validKM As Boolean = False
            For Each Attacker As KMAttacker In selKM.Attackers.Values
                If Attacker.charName <> "" Then
                    validKM = True
                    Exit For
                End If
            Next
            If validKM = False Then
                MessageBox.Show("There does not appear to be any valid Attackers on this killmail other than NPCs. The killmail will therefore not be uploaded.", "Non-NPC Attackers Required.", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' Write the killmail detail
                Call Me.UploadKillmail(URI, BuildKMDetails(selKM))
            End If
        End If

    End Sub

    Private Function UploadKillmail(ByVal RemoteURL As String, ByVal KMText As String) As Boolean

        ' Build the POST data
        Dim postData As String = ""
        postData &= "mail=" & KMText
        postData &= "&option=return"
        postData &= "&Submit=Submit"
        Try
            ' Create the requester
            ServicePointManager.DefaultConnectionLimit = 20
            ServicePointManager.Expect100Continue = False
            Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(RemoteURL))
            Dim request As HttpWebRequest = CType(WebRequest.Create(RemoteURL), HttpWebRequest)
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                request.Proxy = EveHQProxy
            End If
            ' Setup request parameters
            request.Method = "POST"
            request.ContentLength = postData.Length
            request.ContentType = "application/x-www-form-urlencoded"
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
            ' Setup a stream to write the HTTP "POST" data
            Dim WebEncoding As New ASCIIEncoding()
            Dim byte1 As Byte() = WebEncoding.GetBytes(postData)
            Dim newStream As Stream = request.GetRequestStream()
            newStream.Write(byte1, 0, byte1.Length)
            newStream.Close()
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, System.Text.Encoding.UTF8)
            Dim webdata As String = readStream.ReadToEnd()
            ' Need to check here for bad responses!

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


#End Region

End Class