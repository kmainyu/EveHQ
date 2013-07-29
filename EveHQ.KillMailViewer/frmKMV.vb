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

Imports System.Windows.Forms
Imports System.Xml
Imports System.Text
Imports System.IO
Imports System.Net
Imports DevComponents.AdvTree

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
                        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(Character) Then
                            newPilot.Name = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Character), EveHQ.Core.Pilot).ID
                        Else
                            If EveHQ.Core.HQ.EveHQSettings.Corporations.ContainsKey(Character) Then
                                newPilot.Name = EveHQ.Core.HQ.EveHQSettings.Corporations(Character).ID
                            Else
                                newPilot.Name = Character
                            End If
                        End If
                        lvwCharacters.Items.Add(newPilot)
                    Next
                    lvwCharacters.Sort()
                    lvwCharacters.EndUpdate()
                    ' Enable the Fetch Killmails button
                    btnFetchKillMails.Enabled = True
                    ' Check the account - disable the Get Corp Killmails box for v2
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

        '  Create an instance of the API Request class
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

        ' Create an XML document for retrieving characters
        Dim CharactersXML As New XmlDocument
        CharactersXML = APIReq.GetAPIXML(EveAPI.APITypes.Characters, KMAccount.ToAPIAccount, EveAPI.APIReturnMethods.ReturnStandard)

        ' Check for errors
        If APIReq.LastAPIErrorText <> "" Then
            Select Case APIReq.LastAPIResult
                Case EveAPI.APIResults.APIServerDownReturnedCached, EveAPI.APIResults.APIServerDownReturnedNull, EveAPI.APIResults.CCPError, EveAPI.APIResults.PageNotFound, EveAPI.APIResults.TimedOut, EveAPI.APIResults.UnknownError
                    MessageBox.Show("There was an error retrieving character information from the API. The error was " & APIReq.LastAPIErrorText, "API Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            If KMAccount.APIKeyType = Core.APIKeyTypes.Corporation Then
                KMXML = APIReq.GetAPIXML(EveAPI.APITypes.KillLogCorp, KMAccount.ToAPIAccount, charID, lastKillID, EveAPI.APIReturnMethods.ReturnStandard)
            Else
                KMXML = APIReq.GetAPIXML(EveAPI.APITypes.KillLogChar, KMAccount.ToAPIAccount, charID, lastKillID, EveAPI.APIReturnMethods.ReturnStandard)
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
                If (String.IsNullOrEmpty(lastKillID)) Then
                    MessageBox.Show("A null XML document was returned. Check the API Server and internet connection.", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Exit Do 'Work around for bug EVEHQ-165: KillLog API only returns up to the last month of data now.
                End If
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
                newVictim.damageTaken = Double.Parse(.Attributes.GetNamedItem("damageTaken").Value, Globalization.NumberStyles.Any, culture)
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
                newAttacker.secStatus = Double.Parse(attackerNode.Attributes.GetNamedItem("securityStatus").Value, Globalization.NumberStyles.Any, culture)
                newAttacker.damageDone = Double.Parse(attackerNode.Attributes.GetNamedItem("damageDone").Value, Globalization.NumberStyles.Any, culture)
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
                newItem.flag = Integer.Parse(itemNode.Attributes.GetNamedItem("flag").Value, Globalization.NumberStyles.Any, culture)
                newItem.qtyDropped = Integer.Parse(itemNode.Attributes.GetNamedItem("qtyDropped").Value, Globalization.NumberStyles.Any, culture)
                newItem.qtyDestroyed = Integer.Parse(itemNode.Attributes.GetNamedItem("qtyDestroyed").Value, Globalization.NumberStyles.Any, culture)
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
        adtKillmails.BeginUpdate()
        adtKillmails.Nodes.Clear()
        For Each charKM As KillMail In KMs.Values
            Dim newKM As New Node
            newKM.Tag = charKM.killID
            If charKM.Victim.charName = "" Then
                newKM.Text = charKM.Victim.corpName
            Else
                newKM.Text = charKM.Victim.charName
            End If
            adtKillmails.Nodes.Add(newKM)
            newKM.Cells.Add(New Cell(EveHQ.Core.HQ.itemData(charKM.Victim.shipTypeID).Name))
            newKM.Cells.Add(New Cell(charKM.killTime.ToString))
        Next
        adtKillmails.EndUpdate()
        ' Update the summary label
        lblKMSummary.Text = "Killmail Summary for " & charName & " (" & adtKillmails.Nodes.Count & " items)"
        ' Clear the killmail detail text
        txtKillMailDetails.Text = ""
        EveHQ.Core.AdvTreeSorter.Sort(adtKillmails, 1, True, True)
    End Sub
#End Region

#Region "Killmail Detail Routines"

    Private Sub adtKillmails_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtKillmails.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub adtKillmails_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtKillmails.SelectionChanged
        If adtKillmails.SelectedNodes.Count > 0 Then
            ' Get the killID of the selected Killmail
            Dim killID As String = CStr(adtKillmails.SelectedNodes(0).Tag)
            Dim selKM As KillMail = CType(KMs(killID), KillMail)
            ' Write the killmail detail
            Call DrawKMDetail(selKM)
            btnCopyKillmail.Enabled = True
            btnExportToHQF.Enabled = True
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
        If selKM.Victim.charName = "" Then
            KMText.AppendLine("Victim: " & selKM.Victim.corpName)
        Else
            KMText.AppendLine("Victim: " & selKM.Victim.charName)
        End If
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
        KMText.AppendLine("Destroyed: " & EveHQ.Core.HQ.itemData(selKM.Victim.shipTypeID).Name)
        KMText.AppendLine("System: " & ss.Name)
        KMText.AppendLine("Security: " & Math.Max(ss.Security, 0).ToString("N1"))
        KMText.AppendLine("Damage Taken: " & selKM.Victim.damageTaken.ToString)

        ' Put the attackers into the correct order
        Dim attackers As New List(Of KMAttacker)
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
                KMText.AppendLine("Security: " & attacker.secStatus.ToString("N1"))
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
                If attacker.shipTypeID = "0" Then
                    KMText.AppendLine("Ship: Unknown")
                Else
                    KMText.AppendLine("Ship: " & EveHQ.Core.HQ.itemData(attacker.shipTypeID).Name)
                End If
                KMText.AppendLine("Weapon: " & EveHQ.Core.HQ.itemData(attacker.weaponTypeID).Name)
            Else
                If attacker.corpName = "" Then
                    KMText.Append("Name: " & EveHQ.Core.HQ.itemData(attacker.shipTypeID).Name & " / Unknown")
                Else
                    KMText.Append("Name: " & EveHQ.Core.HQ.itemData(attacker.shipTypeID).Name & " / " & attacker.corpName)
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
        Dim droppedItems, destroyedItems As New List(Of String)
        Dim itemName As String = ""
        For Each item As KMItem In selKM.Items
            itemName = EveHQ.Core.HQ.itemData(item.typeID).Name
            If item.qtyDestroyed > 0 Then
                If item.qtyDestroyed = 1 Then
                    If item.flag = 0 Then
                        destroyedItems.Add(itemName)
                    Else
                        destroyedItems.Add(itemName & " (Cargo)")
                    End If
                Else
                    If item.flag = 0 Then
                        destroyedItems.Add(itemName & ", Qty: " & item.qtyDestroyed.ToString)
                    Else
                        destroyedItems.Add(itemName & ", Qty: " & item.qtyDestroyed.ToString & " (Cargo)")
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
                        droppedItems.Add(itemName & ", Qty: " & item.qtyDropped.ToString)
                    Else
                        droppedItems.Add(itemName & ", Qty: " & item.qtyDropped.ToString & " (Cargo)")
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
        Dim URI As String = "http://eve.battleclinic.com/killboard/index.php"

        If adtKillmails.SelectedNodes.Count > 0 Then
            ' Get the killID of the selected Killmail
            Dim killID As String = CStr(adtKillmails.SelectedNodes(0).Tag)
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
            Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
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

#Region "Export Options"

    Private Sub btnCopyKillmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyKillmail.Click
        If txtKillMailDetails.Text <> "" Then
            Try
                Clipboard.SetText(txtKillMailDetails.Text)
            Catch ex As Exception
                MessageBox.Show("There was an error copying the killmail details to the clipboard. The error was: " & ex.Message, "Killmail Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Please select a killmail before copying to the clipboard", "Killmail Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnExportToHQF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToHQF.Click

        If adtKillmails.SelectedNodes.Count > 0 Then

            Dim FittedItems As New SortedList(Of String, Integer)

            ' Get the killID of the selected Killmail
            Dim killID As String = CStr(adtKillmails.SelectedNodes(0).Tag)
            Dim selKM As KillMail = CType(KMs(killID), KillMail)

            ' Make a list of dropped and destroyed items

            Dim itemName As String = ""
            For Each item As KMItem In selKM.Items
                If item.flag = 0 Then
                    ' This is a fitted item
                    If (item.qtyDestroyed + item.qtyDropped) > 0 Then
                        FittedItems.Add(item.typeID, item.qtyDestroyed + item.qtyDropped)
                    End If
                End If
            Next

            ' Create Fitting DNA string
            Dim DNA As New StringBuilder
            DNA.Append("fitting://evehq/")
            DNA.Append(selKM.Victim.shipTypeID.ToString)
            For Each item As String In FittedItems.Keys
                DNA.Append(":" & item & "*" & FittedItems(item).ToString)
            Next
            ' Add a basic loadout name
            DNA.Append("?LoadoutName=" & selKM.Victim.charName & "'s " & EveHQ.Core.HQ.itemData(selKM.Victim.shipTypeID).Name)

            ' Start the HQF plug-in if it's active
            Dim PluginName As String = "EveHQ Fitter"
            Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
            If myPlugIn.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
                Dim mainTab As DevComponents.DotNetBar.TabStrip = CType(EveHQ.Core.HQ.MainForm.Controls("tabEveHQMDI"), DevComponents.DotNetBar.TabStrip)
                Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(PluginName)
                If tp IsNot Nothing Then
                    mainTab.SelectedTab = tp
                Else
                    Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
                    plugInForm.MdiParent = EveHQ.Core.HQ.MainForm
                    plugInForm.Show()
                End If
                myPlugIn.Instance.GetPlugInData(DNA.ToString, 0)
            Else
                ' Plug-in is not loaded so best not try to access it!
                Dim msg As String = ""
                msg &= "The " & myPlugIn.MainMenuText & " Plug-in is not currently active." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Please load the plug-in before proceeding."
                MessageBox.Show(msg, "Error Starting " & myPlugIn.MainMenuText & " Plug-in!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End If

    End Sub

#End Region

End Class