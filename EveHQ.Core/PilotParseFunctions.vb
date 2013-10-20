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
Imports System.IO
Imports System.Windows.Forms

Public Class PilotParseFunctions
    Private Const SkillTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Shared ReadOnly Culture As CultureInfo = New CultureInfo("en-GB")
    Public Shared Event RefreshPilots()

    Shared Property StartPilotRefresh() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent RefreshPilots()
            End If
        End Set
    End Property

    Public Shared Sub BuildAttributeData(ByRef cpilot As EveHQPilot)
        cpilot.WAttT = cpilot.WAtt + cpilot.WImplant
        cpilot.CAttT = cpilot.CAtt + cpilot.CImplant
        cpilot.IntAttT = cpilot.IntAtt + cpilot.IntImplant
        cpilot.MAttT = cpilot.MAtt + cpilot.MImplant
        cpilot.PAttT = cpilot.PAtt + cpilot.PImplant
    End Sub           'BuildAttributeData    

    Public Shared Sub CopyTempPilotsToMain()

        ' Copy new pilot data
        Dim oldPilot, newPilot As EveHQPilot
        For Each newPilot In HQ.TempPilots.Values
            If HQ.Settings.Pilots.ContainsKey(newPilot.Name) Then
                oldPilot = HQ.Settings.Pilots(newPilot.Name)
                ' Transfer old information first (stuff that isn't picked up in the XML download)!!
                newPilot.UseManualImplants = oldPilot.UseManualImplants
                newPilot.CImplantM = oldPilot.CImplantM
                newPilot.IntImplantM = oldPilot.IntImplantM
                newPilot.MImplantM = oldPilot.MImplantM
                newPilot.PImplantM = oldPilot.PImplantM
                newPilot.WImplantM = oldPilot.WImplantM
                newPilot.ActiveQueueName = oldPilot.ActiveQueueName
                newPilot.ActiveQueue = CType(oldPilot.ActiveQueue.Clone, EveHQSkillQueue)
                newPilot.Active = oldPilot.Active
                newPilot.PrimaryQueue = oldPilot.PrimaryQueue
                newPilot.Standings = oldPilot.Standings
                newPilot.TrainingQueues = oldPilot.TrainingQueues
                ' Check if the old pilot has an account if using manual mode!!
                If oldPilot.Account <> "" And newPilot.Account = "" Then
                    newPilot.Account = oldPilot.Account
                    newPilot.AccountPosition = oldPilot.AccountPosition
                End If

                'Bug EVEHQ-56 ... check that the pilot is in the collection before attempting to remove.
                If HQ.Settings.Pilots.ContainsKey(oldPilot.Name) Then
                    HQ.Settings.Pilots.Remove(oldPilot.Name)
                End If

            End If
        Next
        For Each newPilot In HQ.TempPilots.Values
            ' Add the update info first to indicate it has been updated
            ' Check for some attribute that should not be blank or zero!
            If newPilot.SkillPoints <> 0 And newPilot.Corp <> "" Then
                newPilot.Updated = True
                newPilot.LastUpdate = Now.ToString
            End If
            If HQ.Settings.Pilots.ContainsKey(newPilot.Name) = False Then
                HQ.Settings.Pilots.Add(newPilot.Name, newPilot)
            End If
        Next

        ' Reload pilot key skills
        Call LoadKeySkills()

    End Sub          'CopyTempPilotsToMain

    Public Shared Sub CopyTempCorpsToMain()

        ' Remove corps that will be replaced
        For Each newCorp As Corporation In HQ.TempCorps.Values
            If HQ.Settings.Corporations.ContainsKey(newCorp.Name) Then
                HQ.Settings.Corporations.Remove(newCorp.Name)
            End If
        Next

        ' Copy new corp data
        For Each newCorp As Corporation In HQ.TempCorps.Values
            ' Add the update info first to indicate it has been updated
            If HQ.Settings.Corporations.ContainsKey(newCorp.Name) = False Then
                HQ.Settings.Corporations.Add(newCorp.Name, newCorp)
            End If
        Next

    End Sub

    Private Shared Sub GetPilotCachedInfo()
        For Each currentPilot As EveHQPilot In HQ.TempPilots.Values
            ' Check if the 2 files exist in the cache
            Dim cXML As Boolean = False
            Dim tXML As Boolean = False
            Dim cXMLDoc, tXMLDoc As New XmlDocument
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.CharacterSheet.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml")) = True Then
                cXML = True
            End If
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.SkillQueue.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml")) = True Then
                tXML = True
            End If

            ' Only load in and parse if both files are available
            If cXML = True Then
                cXMLDoc.Load(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.CharacterSheet.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml"))
                If tXML = True Then
                    tXMLDoc.Load(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.SkillQueue.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml"))
                End If
                Call ParsePilotSkills(currentPilot, cXMLDoc)
                Call ParsePilotXML(currentPilot, cXMLDoc)
                Call ParseTrainingXML(currentPilot, tXMLDoc)
                Call BuildAttributeData(currentPilot)
            Else
                Dim msg As String = ""
                If cXML = False Then
                    msg &= "Character XML file for " & currentPilot.Name & " not found." & ControlChars.CrLf
                End If
                msg &= ControlChars.CrLf
                If CDbl(currentPilot.AccountPosition) = 0 Then
                    msg &= "This pilot is to be updated manually. Please do this via the integrated web browser or the import XML option."
                Else
                    msg &= "This pilot is linked to an account. Please update the account via the Tools > Retrieve Account Data menu option."
                End If
                MessageBox.Show(msg, "Missing XML File(s)", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Next
    End Sub            'GetPilotCachedInfo
    Public Shared Function LoadKeySkills() As Boolean
        Call ResetKeySkills()
        For Each curPilot As EveHQPilot In HQ.Settings.Pilots.Values
            If LoadKeySkillsForPilot(curPilot) = False Then
                Return False
            End If
        Next
        Return True
    End Function
    Public Shared Function LoadKeySkillsForPilot(ByVal curPilot As EveHQPilot) As Boolean
        Try
            For Each curSkill As EveHQPilotSkill In curPilot.PilotSkills.Values
                Select Case curSkill.Name
                    Case "Mining"
                        curPilot.KeySkills(KeySkill.Mining) = curSkill.Level
                    Case "Mining Upgrades"
                        curPilot.KeySkills(KeySkill.MiningUpgrades) = curSkill.Level
                    Case "Astrogeology"
                        curPilot.KeySkills(KeySkill.Astrogeology) = curSkill.Level
                    Case "Mining Barge"
                        curPilot.KeySkills(KeySkill.MiningBarge) = curSkill.Level
                    Case "Mining Drone Operation"
                        curPilot.KeySkills(KeySkill.MiningDrone) = curSkill.Level
                    Case "Exhumers"
                        curPilot.KeySkills(KeySkill.Exhumers) = curSkill.Level
                    Case "Refining"
                        curPilot.KeySkills(KeySkill.Refining) = curSkill.Level
                    Case "Refinery Efficiency"
                        curPilot.KeySkills(KeySkill.RefiningEfficiency) = curSkill.Level
                    Case "Metallurgy"
                        curPilot.KeySkills(KeySkill.Metallurgy) = curSkill.Level
                    Case "Research"
                        curPilot.KeySkills(KeySkill.Research) = curSkill.Level
                    Case "Science"
                        curPilot.KeySkills(KeySkill.Science) = curSkill.Level
                    Case "Industry"
                        curPilot.KeySkills(KeySkill.Industry) = curSkill.Level
                    Case "Production Efficiency"
                        curPilot.KeySkills(KeySkill.ProductionEfficiency) = curSkill.Level
                    Case "Arkonor Processing"
                        curPilot.KeySkills(KeySkill.ArkonorProc) = curSkill.Level
                    Case "Bistot Processing"
                        curPilot.KeySkills(KeySkill.BistotProc) = curSkill.Level
                    Case "Crokite Processing"
                        curPilot.KeySkills(KeySkill.CrokiteProc) = curSkill.Level
                    Case "Dark Ochre Processing"
                        curPilot.KeySkills(KeySkill.DarkOchreProc) = curSkill.Level
                    Case "Gneiss Processing"
                        curPilot.KeySkills(KeySkill.GneissProc) = curSkill.Level
                    Case "Hedbergite Processing"
                        curPilot.KeySkills(KeySkill.HedbergiteProc) = curSkill.Level
                    Case "Hemorphite Processing"
                        curPilot.KeySkills(KeySkill.HemorphiteProc) = curSkill.Level
                    Case "Jaspet Processing"
                        curPilot.KeySkills(KeySkill.JaspetProc) = curSkill.Level
                    Case "Kernite Processing"
                        curPilot.KeySkills(KeySkill.KerniteProc) = curSkill.Level
                    Case "Mercoxit Processing"
                        curPilot.KeySkills(KeySkill.MercoxitProc) = curSkill.Level
                    Case "Omber Processing"
                        curPilot.KeySkills(KeySkill.OmberProc) = curSkill.Level
                    Case "Plagioclase Processing"
                        curPilot.KeySkills(KeySkill.PlagioclaseProc) = curSkill.Level
                    Case "Pyroxeres Processing"
                        curPilot.KeySkills(KeySkill.PyroxeresProc) = curSkill.Level
                    Case "Scordite Processing"
                        curPilot.KeySkills(KeySkill.ScorditeProc) = curSkill.Level
                    Case "Spodumain Processing"
                        curPilot.KeySkills(KeySkill.SpodumainProc) = curSkill.Level
                    Case "Veldspar Processing"
                        curPilot.KeySkills(KeySkill.VeldsparProc) = curSkill.Level
                    Case "Ice Processing"
                        curPilot.KeySkills(KeySkill.IceProc) = curSkill.Level
                    Case "Ice Harvesting"
                        curPilot.KeySkills(KeySkill.IceHarvesting) = curSkill.Level
                    Case "Deep Core Mining"
                        curPilot.KeySkills(KeySkill.DeepCoreMining) = curSkill.Level
                    Case "Mining Foreman"
                        curPilot.KeySkills(KeySkill.MiningForeman) = curSkill.Level
                    Case "Mining Director"
                        curPilot.KeySkills(KeySkill.MiningDirector) = curSkill.Level
                    Case "Learning"
                        curPilot.KeySkills(KeySkill.Learning) = curSkill.Level
                    Case "Jump Drive Operation"
                        curPilot.KeySkills(KeySkill.JumpDriveOperation) = curSkill.Level
                    Case "Jump Drive Calibration"
                        curPilot.KeySkills(KeySkill.JumpDriveCalibration) = curSkill.Level
                    Case "Jump Fuel Conservation"
                        curPilot.KeySkills(KeySkill.JumpFuelConservation) = curSkill.Level
                    Case "Jump Freighters"
                        curPilot.KeySkills(KeySkill.JumpFreighters) = curSkill.Level
                    Case "Scrapmetal Processing"
                        curPilot.KeySkills(KeySkill.ScrapMetalProc) = curSkill.Level
                    Case "Accounting"
                        curPilot.KeySkills(KeySkill.Accounting) = curSkill.Level
                    Case "Broker Relations"
                        curPilot.KeySkills(KeySkill.BrokerRelations) = curSkill.Level
                    Case "Daytrading"
                        curPilot.KeySkills(KeySkill.Daytrading) = curSkill.Level
                    Case "Margin Trading"
                        curPilot.KeySkills(KeySkill.MarginTrading) = curSkill.Level
                    Case "Marketing"
                        curPilot.KeySkills(KeySkill.Marketing) = curSkill.Level
                    Case "Procurement"
                        curPilot.KeySkills(KeySkill.Procurement) = curSkill.Level
                    Case "Retail"
                        curPilot.KeySkills(KeySkill.Retail) = curSkill.Level
                    Case "Trade"
                        curPilot.KeySkills(KeySkill.Trade) = curSkill.Level
                    Case "Tycoon"
                        curPilot.KeySkills(KeySkill.Tycoon) = curSkill.Level
                    Case "Visibility"
                        curPilot.KeySkills(KeySkill.Visibility) = curSkill.Level
                    Case "Wholesale"
                        curPilot.KeySkills(KeySkill.Wholesale) = curSkill.Level
                    Case "Diploamcy"
                        curPilot.KeySkills(KeySkill.Diplomacy) = curSkill.Level
                    Case "Connections"
                        curPilot.KeySkills(KeySkill.Connections) = curSkill.Level
                End Select
            Next
            Return True
        Catch e As Exception
            Return False
        End Try
    End Function
    Private Shared Sub ResetKeySkills()
        For Each curPilot As EveHQPilot In HQ.Settings.Pilots.Values
            curPilot.KeySkills = New Dictionary(Of KeySkill, Integer)
            For key As Integer = 0 To [Enum].GetValues(GetType(KeySkill)).Length
                curPilot.KeySkills.Add(CType(key, KeySkill), 0)
            Next
        Next
    End Sub

#Region "Eve API Retrieval Methods"

    Public Shared Sub GetCharactersInAccount(ByVal caccount As EveHQAccount)

        If caccount Is Nothing Then
            Return
        End If

        ' Check the API key status
        caccount.CheckAPIKey()
        ' Check Account Status
        If caccount.APIKeyType <> APIKeyTypes.Corporation Then
            Call GetAccountStatus(caccount)
        End If

        ' Fetch the characters on account XML file
        Dim apiReq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
        Dim accountXML As XmlDocument = apiReq.GetAPIXML(APITypes.Characters, caccount.ToAPIAccount, APIReturnMethods.ReturnStandard)
        If apiReq.LastAPIResult = APIResults.CCPError Then
            HQ.APIResults.Add(caccount.UserID.ToString, -apiReq.LastAPIError)
        Else
            HQ.APIResults.Add(caccount.UserID.ToString, apiReq.LastAPIResult)
        End If

        ' Ignore for APIv2 corp keys
        If Not (caccount.ApiKeySystem = APIKeySystems.Version2 And caccount.APIKeyType = APIKeyTypes.Corporation) Then

            If apiReq.LastAPIResult = APIResults.ReturnedActual Or apiReq.LastAPIResult = APIResults.ReturnedCached Or apiReq.LastAPIResult = APIResults.ReturnedNew Then
                If accountXML IsNot Nothing Then
                    ' Get characters
                    Dim charlist As XmlNodeList
                    Dim toon As XmlNode
                    Dim currToon As Integer = 0

                    ' Get the list of characters and the character IDs
                    charlist = accountXML.SelectNodes("/eveapi/result/rowset/row")
                    ' Clear the current characters on the account
                    caccount.Characters = New List(Of String)
                    For Each toon In charlist
                        currToon += 1
                        ' Add the pilot details into the collection
                        Dim newPilot As New EveHQPilot
                        newPilot.Name = toon.Attributes.GetNamedItem("name").Value
                        newPilot.ID = toon.Attributes.GetNamedItem("characterID").Value
                        newPilot.AccountPosition = CStr(currToon)
                        newPilot.Account = caccount.UserID
                        ' Copy notification data if available - we reset this after checking the API request if not cached
                        If HQ.Settings.Pilots.ContainsKey(newPilot.Name) = True Then
                            newPilot.TrainingNotifiedEarly = HQ.Settings.Pilots(newPilot.Name).TrainingNotifiedEarly
                            newPilot.TrainingNotifiedNow = HQ.Settings.Pilots(newPilot.Name).TrainingNotifiedNow
                        End If
                        ' Download any updated portrait for this character
                        ImageHandler.DownloadPortrait(newPilot.ID)
                        If HQ.TempPilots.ContainsKey(newPilot.Name) = False Then
                            HQ.TempPilots.Add(newPilot.Name, newPilot)
                        End If
                        caccount.Characters.Add(newPilot.Name)
                        Call GetCharacterXmLs(caccount, newPilot)
                    Next

                    ' Check if we have any old pilots that the account does not have anymore
                    Dim oldPilots As String = ""
                    If HQ.Settings.Pilots Is Nothing Then
                        Return
                    End If
                    For Each oldPilot As EveHQPilot In HQ.Settings.Pilots.Values
                        If (oldPilot Is Nothing) Then
                            Continue For
                        End If
                        If oldPilot.Account = caccount.UserID Then
                            Dim validPilot As Boolean = False
                            For Each toon In charlist
                                If toon.Attributes.GetNamedItem("name").Value = oldPilot.Name Then
                                    validPilot = True
                                    Exit For
                                End If
                            Next
                            If validPilot = False Then
                                oldPilots &= oldPilot.Name & ","
                                oldPilot.Account = ""
                                oldPilot.AccountPosition = "0"
                            End If
                        End If
                    Next
                    oldPilots = oldPilots.Trim(CChar(","))
                    If oldPilots <> "" Then
                        Dim msg As String = ""
                        msg &= "You have pilots registered in EveHQ that were previously assigned to account '" & caccount.UserID & "'" & ControlChars.CrLf
                        msg &= "but are no longer part of that account. The following pilots have been converted to manual pilots:" & ControlChars.CrLf & ControlChars.CrLf
                        Dim olderPilots() As String = oldPilots.Split(CChar(","))
                        Dim dPilot As String
                        For Each dPilot In olderPilots
                            msg &= dPilot & ControlChars.CrLf
                        Next
                        MessageBox.Show(msg, "Unused Pilots", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                End If
            End If
        Else
            Dim corpList As XmlNodeList
            Dim corp As XmlNode
            ' Add a corporation to the settings
            ' Get the list of characters and the character IDs
            corpList = accountXML.SelectNodes("/eveapi/result/rowset/row")
            ' Clear the current characters on the account
            caccount.Characters = New List(Of String)
            If corpList.Count > 0 Then
                corp = corpList(0)
                Dim newCorp As New Corporation
                ' Get the existing corp if appropriate
                If HQ.TempCorps.ContainsKey(corp.Attributes.GetNamedItem("corporationName").Value) = True Then
                    newCorp = HQ.TempCorps(corp.Attributes.GetNamedItem("corporationName").Value)
                Else
                    newCorp.Name = corp.Attributes.GetNamedItem("corporationName").Value
                    newCorp.ID = corp.Attributes.GetNamedItem("corporationID").Value
                    HQ.TempCorps.Add(newCorp.Name, newCorp)
                    caccount.Characters.Add(newCorp.Name)
                End If
                If newCorp.CharacterIDs.Contains(corp.Attributes.GetNamedItem("characterID").Value) = False Then
                    newCorp.CharacterIDs.Add(corp.Attributes.GetNamedItem("characterID").Value)
                End If
                If newCorp.CharacterNames.Contains(corp.Attributes.GetNamedItem("name").Value) = False Then
                    newCorp.CharacterNames.Add(corp.Attributes.GetNamedItem("name").Value)
                End If
                If newCorp.Accounts.Contains(caccount.UserID) = False Then
                    newCorp.Accounts.Add(caccount.UserID)
                End If
            End If
        End If
    End Sub
    Private Shared Sub GetAccountStatus(ByRef cAccount As EveHQAccount)
        ' Attempts to get the AccountStatus API for additional information and for checking API key status
        Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
        Dim accountXML As XmlDocument = apiReq.GetAPIXML(APITypes.AccountStatus, cAccount.ToAPIAccount, APIReturnMethods.ReturnStandard)
        Select Case cAccount.ApiKeySystem
            Case APIKeySystems.Version2
                Select Case apiReq.LastAPIError
                    Case -1
                        If accountXML IsNot Nothing Then
                            cAccount.LastAccountStatusCheck = Now
                            ' Parse the account information
                            If accountXML.GetElementsByTagName("createDate").Item(0).InnerText <> "" Then
                                Dim cd As Date = DateTime.ParseExact(accountXML.GetElementsByTagName("createDate").Item(0).InnerText, SkillTimeFormat, Culture, DateTimeStyles.None)
                                cAccount.CreateDate = SkillFunctions.ConvertEveTimeToLocal(cd)
                                Dim pu As Date = DateTime.ParseExact(accountXML.GetElementsByTagName("paidUntil").Item(0).InnerText, SkillTimeFormat, Culture, DateTimeStyles.None)
                                cAccount.PaidUntil = SkillFunctions.ConvertEveTimeToLocal(pu)
                                cAccount.LogonCount = CLng(accountXML.GetElementsByTagName("logonCount").Item(0).InnerText)
                                cAccount.LogonMinutes = CLng(accountXML.GetElementsByTagName("logonMinutes").Item(0).InnerText)
                                If cAccount.PaidUntil < Now Then
                                    ' Account has expired
                                    cAccount.APIAccountStatus = APIAccountStatuses.Disabled
                                Else
                                    cAccount.APIAccountStatus = APIAccountStatuses.Active
                                End If
                            End If
                        End If
                    Case 211
                        ' Account has expired
                        cAccount.APIAccountStatus = APIAccountStatuses.Disabled
                    Case 200
                        ' Should be limited key
                        cAccount.APIKeyType = APIKeyTypes.Limited
                        cAccount.APIAccountStatus = APIAccountStatuses.Active
                End Select
        End Select
    End Sub
    Private Shared Sub GetCharacterXmLs(ByVal cAccount As EveHQAccount, ByVal cPilot As EveHQPilot)

        ' Set up an API Request for this character
        Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)

        ' Get the Character Sheet
        Dim cXML As XmlDocument = apiReq.GetAPIXML(APITypes.CharacterSheet, cAccount.ToAPIAccount, cPilot.ID, APIReturnMethods.ReturnStandard)

        ' Store the Character Sheet API result
        If apiReq.LastAPIResult = APIResults.CCPError Then
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet) = False Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet, -apiReq.LastAPIError)
            End If
        Else
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet) = False Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet, apiReq.LastAPIResult)
            End If
        End If

        ' Get the Skill Queue
        Dim tXML As XmlDocument = apiReq.GetAPIXML(APITypes.SkillQueue, cAccount.ToAPIAccount, cPilot.ID, APIReturnMethods.ReturnStandard)

        ' Store the Skill Queue API result
        If apiReq.LastAPIResult = APIResults.CCPError Then
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue) = False Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue, -apiReq.LastAPIError)
            End If
        Else
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue) = False Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue, apiReq.LastAPIResult)
            End If
        End If

        ' Only parse the sheets if both the CharacterSheet and TrainingQueue APIs are not null
        If cXML IsNot Nothing And tXML IsNot Nothing Then
            ' Check if we need to reset the pilot notifications (if not a cached response)
            If apiReq.LastAPIResult = APIResults.ReturnedNew Or apiReq.LastAPIResult = APIResults.ReturnedActual Then
                cPilot.TrainingNotifiedEarly = False
                cPilot.TrainingNotifiedNow = False
            End If

            ' Parse the API data
            Call ParsePilotSkills(cPilot, cXML)
            Call ParsePilotXML(cPilot, cXML)
            Call ParseTrainingXML(cPilot, tXML)
            Call BuildAttributeData(cPilot)
        End If

    End Sub
    Private Shared Sub ParsePilotXML(ByRef cPilot As EveHQPilot, ByVal cxmlDoc As XmlDocument)

        Dim charDetails As XmlNodeList
        Dim toon As XmlNode
        Const toonNo As Integer = 0

        charDetails = cxmlDoc.SelectNodes("/eveapi/result")
        toon = charDetails(toonNo)
        If toon IsNot Nothing Then
            ' Get the Pilot name & charID in the character node
            With cPilot
                ' Get the additional pilot data nodes
                .Name = CXMLDoc.GetElementsByTagName("name").Item(0).InnerText
                .Race = CXMLDoc.GetElementsByTagName("race").Item(0).InnerText
                .Blood = CXMLDoc.GetElementsByTagName("bloodLine").Item(0).InnerText
                .Gender = CXMLDoc.GetElementsByTagName("gender").Item(0).InnerText
                .Corp = CXMLDoc.GetElementsByTagName("corporationName").Item(0).InnerText
                .CorpID = CXMLDoc.GetElementsByTagName("corporationID").Item(0).InnerText
                .CloneName = CXMLDoc.GetElementsByTagName("cloneName").Item(0).InnerText
                .CloneSP = CInt(CXMLDoc.GetElementsByTagName("cloneSkillPoints").Item(0).InnerText)
                Dim isk As Double = Double.Parse(CXMLDoc.GetElementsByTagName("balance").Item(0).InnerText, NumberStyles.Any, Culture)
                .Isk = isk
                ' Put cache info here??
            End With

            ' Get the implant details
            charDetails = CXMLDoc.SelectNodes("/eveapi/result/attributeEnhancers")
            ' Get the relevant node!
            toon = charDetails(0)       ' This is zero because there is only 1 occurence of the attributeEnhancers node in each XML doc
            If toon.HasChildNodes Then
                For implant As Integer = 0 To toon.ChildNodes.Count - 1
                    Select Case toon.ChildNodes(implant).Name
                        ' Save it in the Automatic implant sections
                        Case "perceptionBonus"
                            cPilot.PImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
                        Case "willpowerBonus"
                            cPilot.WImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
                        Case "intelligenceBonus"
                            cPilot.IntImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
                        Case "memoryBonus"
                            cPilot.MImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
                        Case "charismaBonus"
                            cPilot.CImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
                    End Select
                Next
            End If

            ' Decide whether to use Auto or Manual Implants
            If cPilot.UseManualImplants = True Then
                cPilot.CImplant = cPilot.CImplantM
                cPilot.IntImplant = cPilot.IntImplantM
                cPilot.MImplant = cPilot.MImplantM
                cPilot.PImplant = cPilot.PImplantM
                cPilot.WImplant = cPilot.WImplantM
            Else
                cPilot.CImplant = cPilot.CImplantA
                cPilot.IntImplant = cPilot.IntImplantA
                cPilot.MImplant = cPilot.MImplantA
                cPilot.PImplant = cPilot.PImplantA
                cPilot.WImplant = cPilot.WImplantA
            End If

            ' Get the attribute details
            charDetails = CXMLDoc.SelectNodes("/eveapi/result/attributes")
            ' Get the relevant node!
            toon = charDetails(0)       ' This is zero because there is only 1 occurence of the attributes node in each XML doc
            If toon.HasChildNodes Then
                For implant As Integer = 0 To toon.ChildNodes.Count - 1
                    Select Case toon.ChildNodes(implant).Name
                        Case "perception"
                            cPilot.PAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "willpower"
                            cPilot.WAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "intelligence"
                            cPilot.IntAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "memory"
                            cPilot.MAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "charisma"
                            cPilot.CAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                    End Select
                Next
            End If

            ' Get Cache details
            charDetails = CXMLDoc.SelectNodes("/eveapi")
            cPilot.CacheFileTime = CDate(charDetails(0).ChildNodes(0).InnerText)
            cPilot.CacheExpirationTime = CDate(charDetails(0).ChildNodes(2).InnerText)
        Else
            cPilot.CacheFileTime = DateTime.Now.AddHours(1)
            cPilot.CacheExpirationTime = DateTime.Now.AddHours(1)
        End If

    End Sub                'ParsePilotXML
    Private Shared Sub ParseTrainingXML(ByRef cPilot As EveHQPilot, ByVal txmlDoc As XmlDocument)
        ' Get the training details
        If TXMLDoc IsNot Nothing Then
            Dim trainingDetails As XmlNodeList
            Dim trainingNode As XmlNode
            trainingDetails = txmlDoc.SelectNodes("/eveapi/result/rowset/row")
            ' Check if a training file has been loaded!
            If trainingDetails.Count > 0 Then
                ' Get the first node i.e. the current training skill
                trainingNode = trainingDetails(0)       ' This is zero because there is only 1 occurence of the skillTraining node in each XML doc
                With cPilot
                    If trainingNode.Attributes("startTime").Value <> "" And trainingNode.Attributes("endTime").Value <> "" Then
                        .Training = True
                        .TrainingSkillID = CInt(trainingNode.Attributes("typeID").Value)
                        .TrainingSkillName = SkillFunctions.SkillIDToName(.TrainingSkillID)
                        Dim dt As Date = DateTime.ParseExact(trainingNode.Attributes("startTime").Value, SkillTimeFormat, Culture, DateTimeStyles.None)
                        .TrainingStartTimeActual = dt
                        .TrainingStartTime = .TrainingStartTimeActual.AddSeconds(HQ.Settings.ServerOffset)
                        dt = DateTime.ParseExact(trainingNode.Attributes("endTime").Value, SkillTimeFormat, Culture, DateTimeStyles.None)
                        .TrainingEndTimeActual = dt
                        .TrainingEndTime = .TrainingEndTimeActual.AddSeconds(HQ.Settings.ServerOffset)
                        .TrainingStartSP = CInt(trainingNode.Attributes("startSP").Value)
                        .TrainingEndSP = CInt(trainingNode.Attributes("endSP").Value)
                        .TrainingSkillLevel = CInt(trainingNode.Attributes("level").Value)
                        Call CheckMissingTrainingSkill(cPilot)
                    Else
                        cPilot.Training = False
                        cPilot.QueuedSkills.Clear()
                        cPilot.QueuedSkillTime = 0
                    End If
                End With
                ' Now get any additional results and add them to the pilot queued skills
                cPilot.QueuedSkills.Clear()
                If trainingDetails.Count > 0 Then
                    For queuedSkillNo As Integer = 0 To trainingDetails.Count - 1
                        trainingNode = trainingDetails(queuedSkillNo)
                        If trainingNode.Attributes("startTime").Value <> "" And trainingNode.Attributes("endTime").Value <> "" Then
                            Dim queuedSkill As New EveHQPilotQueuedSkill
                            queuedSkill.Position = CInt(trainingNode.Attributes("queuePosition").Value)
                            queuedSkill.SkillID = CInt(trainingNode.Attributes("typeID").Value)
                            queuedSkill.StartTime = DateTime.ParseExact(trainingNode.Attributes("startTime").Value, SkillTimeFormat, Culture, DateTimeStyles.None).AddSeconds(HQ.Settings.ServerOffset)
                            queuedSkill.EndTime = DateTime.ParseExact(trainingNode.Attributes("endTime").Value, SkillTimeFormat, Culture, DateTimeStyles.None).AddSeconds(HQ.Settings.ServerOffset)
                            queuedSkill.StartSP = CLng(trainingNode.Attributes("startSP").Value)
                            queuedSkill.EndSP = CLng(trainingNode.Attributes("endSP").Value)
                            queuedSkill.Level = CInt(trainingNode.Attributes("level").Value)
                            cPilot.QueuedSkills.Add(queuedSkill.Position, queuedSkill)
                            cPilot.QueuedSkillTime = CLng((queuedSkill.EndTime - cPilot.TrainingEndTime).TotalSeconds)
                        End If
                    Next
                End If
            Else
                cPilot.Training = False
                cPilot.QueuedSkills.Clear()
                cPilot.QueuedSkillTime = 0
            End If
            ' Get Cache details
            trainingDetails = txmlDoc.SelectNodes("/eveapi")
            If trainingDetails.Count > 0 Then
                cPilot.TrainingFileTime = CDate(trainingDetails(0).ChildNodes(0).InnerText)
                cPilot.TrainingExpirationTime = CDate(trainingDetails(0).ChildNodes(2).InnerText)
            End If
        Else
            cPilot.Training = False
        End If
    End Sub
    Private Shared Sub ParsePilotSkills(ByRef cPilot As EveHQPilot, ByVal xmlDoc As XmlDocument)
        Dim charDetails As XmlNodeList
        Dim toon As XmlNode
        Dim sp As Integer
        Dim missingSkills As String = ""
        cPilot.PilotSkills.Clear()

        ' Start new nodelist
        charDetails = xmlDoc.SelectNodes("/eveapi/result/rowset")
        For section As Integer = 0 To charDetails.Count - 1
            Select Case charDetails(section).Attributes.GetNamedItem("name").Value
                Case "skills"
                    toon = charDetails(section)
                    sp = 0
                    ' Get list of skills within the groups!
                    For a As Integer = 0 To toon.ChildNodes.Count - 1
                        Dim newSkill As New EveHQPilotSkill
                        ' Check if the skill exists in the database we have

                        newSkill.ID = CInt(toon.ChildNodes.Item(a).Attributes.GetNamedItem("typeID").Value)
                        newSkill.SP = CInt(toon.ChildNodes.Item(a).Attributes.GetNamedItem("skillpoints").Value)
                        If toon.ChildNodes.Item(a).Attributes.GetNamedItem("level") IsNot Nothing Then
                            newSkill.Level = CInt(toon.ChildNodes.Item(a).Attributes.GetNamedItem("level").Value)
                        Else
                            newSkill.Level = SkillFunctions.CalcLevelFromSP(newSkill.ID, newSkill.SP)
                        End If

                        sp = sp + newSkill.SP

                        ' Load API Data if we are a skill missing!
                        If HQ.SkillListID.ContainsKey(newSkill.ID) = False Then
                            Call SkillFunctions.LoadEveSkillDataFromAPI()
                        End If

                        If HQ.SkillListID.ContainsKey(newSkill.ID) = True Then
                            Dim thisSkill As EveSkill = HQ.SkillListID(newSkill.ID)
                            newSkill.Name = thisSkill.Name
                            newSkill.GroupID = thisSkill.GroupID
                            newSkill.Rank = thisSkill.Rank
                            If cPilot.PilotSkills.ContainsKey(newSkill.Name) = False Then
                                cPilot.PilotSkills.Add(newSkill.Name, newSkill)
                            End If
                            ' Check if a pilot skill is missing from the global skill list
                        Else
                            Dim missingSkill As New EveSkill
                            missingSkill.ID = newSkill.ID
                            missingSkill.Name = "Skill " & newSkill.ID
                            newSkill.Name = "Skill " & newSkill.ID         ' temp line to avoid error
                            missingSkill.GroupID = 267 : newSkill.GroupID = 267
                            missingSkill.Rank = 20 : newSkill.Rank = 20
                            missingSkill.Level = newSkill.Level
                            missingSkill.Pa = "Intelligence" : missingSkill.Sa = "Memory"
                            missingSkill.LevelUp(0) = 0
                            missingSkill.LevelUp(1) = 5000
                            missingSkill.LevelUp(2) = 28284
                            missingSkill.LevelUp(3) = 160000
                            missingSkill.LevelUp(4) = 905097
                            missingSkill.LevelUp(5) = 5120000
                            HQ.SkillListName.Add(missingSkill.Name, missingSkill)
                            HQ.SkillListID.Add(missingSkill.ID, missingSkill)
                            missingSkills &= newSkill.Name & ControlChars.CrLf
                            If cPilot.PilotSkills.ContainsKey(newSkill.Name) = False Then
                                cPilot.PilotSkills.Add(newSkill.Name, newSkill)
                            End If
                        End If
                        ' Check if the skillID is present but the skillname is different (CCP changing bloody skill names!!!)
                        If HQ.SkillListID.ContainsKey(newSkill.ID) = True And HQ.SkillListName.ContainsKey(newSkill.Name) = False Then
                            Dim changeSkill As EveSkill = HQ.SkillListID(newSkill.ID)
                            Dim oldName As String = changeSkill.Name
                            changeSkill.Name = newSkill.Name
                            HQ.SkillListID.Remove(newSkill.ID) : HQ.SkillListID.Add(changeSkill.ID, changeSkill)
                            HQ.SkillListName.Remove(oldName) : HQ.SkillListName.Add(changeSkill.Name, changeSkill)
                        End If
                    Next
                    cPilot.SkillPoints = sp
                Case "certificates"
                    toon = charDetails(section)
                    For Each certNode As XmlNode In toon.ChildNodes
                        cPilot.Certificates.Add(CInt(certNode.Attributes.GetNamedItem("certificateID").Value))
                    Next
                Case "corporationRoles"
                    For Each roleNode As XmlNode In charDetails(section).ChildNodes
                        cPilot.CorpRoles.Add(CType(CLng(roleNode.Attributes.GetNamedItem("roleID").Value), CorporationRoles))
                    Next
            End Select
        Next

        ' If missing skills were identified then report that fact!
        If missingSkills <> "" Then
            Dim msg As String = ""
            msg &= cPilot.Name & " has skills that are not listed in the database. These skills are:" & ControlChars.CrLf & ControlChars.CrLf
            msg &= missingSkills & ControlChars.CrLf
            msg &= "These skills have been added to the database on a temporary basis but the information is incomplete." & ControlChars.CrLf
            msg &= "Any calcaulations performed on the above skills will contain errors until the main database is updated." & ControlChars.CrLf
            msg &= "This includes level-up times and skill training schedules." & ControlChars.CrLf
            msg &= "Please check the EveHQ website for any available update."
            MessageBox.Show(msg, "Missing Skill Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub             'ParsePilotSkills
    Public Shared Sub CheckMissingTrainingSkills()
        For Each curPilot As EveHQPilot In HQ.Settings.Pilots.Values
            For Each cSkill As EveHQPilotSkill In curPilot.PilotSkills.Values
                If HQ.SkillListID.ContainsKey(cSkill.ID) = False Then
                    Call SkillFunctions.LoadEveSkillDataFromAPI()
                    Exit Sub
                End If
            Next
        Next
    End Sub
    Private Shared Sub CheckMissingTrainingSkill(ByRef cPilot As EveHQPilot)
        Dim pilotSkill As New EveHQPilotSkill
        ' Check if the main skill list has the skill we are checking for
        If HQ.SkillListID.ContainsKey(cPilot.TrainingSkillID) = False Then
            Call SkillFunctions.LoadEveSkillDataFromAPI()
        End If
        If cPilot.PilotSkills.ContainsKey(SkillFunctions.SkillIDToName(cPilot.TrainingSkillID)) = False Then
            ' The pilot doesn't have this skill so let's add it manually
            Dim baseSkill As EveSkill = HQ.SkillListID(cPilot.TrainingSkillID)
            pilotSkill.ID = baseSkill.ID
            pilotSkill.Name = baseSkill.Name
            pilotSkill.Rank = baseSkill.Rank
            pilotSkill.GroupID = baseSkill.GroupID
            pilotSkill.Level = cPilot.TrainingSkillLevel - 1
            pilotSkill.SP = cPilot.TrainingStartSP
            cPilot.PilotSkills.Add(pilotSkill.Name, pilotSkill)
        Else
            Dim sq As EveHQPilotSkill = cPilot.PilotSkills(SkillFunctions.SkillIDToName(cPilot.TrainingSkillID))
            sq.Level = cPilot.TrainingSkillLevel - 1
        End If

    End Sub

#End Region

    Public Shared Sub ImportPilotFromXML(ByVal pilotXMLFile As String, ByVal pilotTxmlFile As String)
        ' Need to determine which pilot we are going to import

        ' Load the XML file
        Dim pilotName As String = ""
        Dim pilotID As String = ""
        Dim pilotXML As XmlDocument = New XmlDocument
        Dim pilotTxml As XmlDocument = New XmlDocument
        pilotXML.Load(pilotXMLFile)
        pilotTxml.Load(pilotTxmlFile)

        ' Find out which char has child nodes - this is the one we want
        Dim charDetails As XmlNodeList
        Dim toonNo As Integer

        charDetails = pilotXML.SelectNodes("/eveapi/result")
        If charDetails.Count <> 0 Then
            If charDetails(0).ChildNodes(0).OuterXml.StartsWith("<characterID>") = True Then
                ' Get the relevant node!
                For toonNo = 0 To charDetails.Count - 1
                    If charDetails(toonNo).HasChildNodes = True Then
                        pilotID = charDetails(toonNo).ChildNodes(0).InnerText
                        pilotName = charDetails(toonNo).ChildNodes(1).InnerText
                    End If
                Next

                Dim newPilot As New EveHQPilot
                newPilot.Name = pilotName
                newPilot.ID = pilotID
                newPilot.Account = ""
                newPilot.AccountPosition = "0"
                newPilot.Updated = True
                HQ.TempPilots.Clear()
                HQ.TempPilots.Add(newPilot.Name, newPilot)
                pilotXML.Save(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.CharacterSheet.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                If pilotTxml IsNot Nothing Then
                    pilotTxml.Save(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.SkillQueue.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                End If

                For Each currentPilot As EveHQPilot In HQ.TempPilots.Values
                    Call ParsePilotSkills(currentPilot, pilotXML)
                    Call ParsePilotXML(currentPilot, pilotXML)
                    Call ParseTrainingXML(currentPilot, pilotTxml)
                    Call BuildAttributeData(currentPilot)
                    Call CopyTempPilotsToMain()
                Next
            Else
                MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub
    Public Shared Sub LoadPilotCachedInfo()
        Call GetPilotCachedInfo()
        Call CopyTempPilotsToMain()
    End Sub
    Public Shared Sub LoadPilotFromXML()
        Dim pilotXMLFile As String
        Dim ofd1 As New OpenFileDialog
        With ofd1
            .Title = "Select XML file for Pilot Import"
            .FileName = ""
            .InitialDirectory = "c:\"
            .Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = DialogResult.OK Then
                If My.Computer.FileSystem.FileExists(.FileName) = False Then
                    MessageBox.Show("File does not exist. Please re-try.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                pilotXMLFile = .FileName
                Call ExaminePilotXML(pilotXMLFile)
            Else
                MessageBox.Show("Import cancelled by user.", "Pilot Import Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End With
    End Sub
    Private Shared Sub ExaminePilotXML(ByVal pilotXMLFile As String)
        ' Need to determine which pilot we are going to import

        ' Load the XML file
        Dim pilotName As String = ""
        Dim pilotID As String = ""
        Dim pilotXML As XmlDocument = New XmlDocument
        Dim pilotTxml As XmlDocument = New XmlDocument
        pilotXML.Load(pilotXMLFile)

        ' Find out which char has child nodes - this is the one we want
        Dim charDetails As XmlNodeList
        Dim toonNo As Integer
        Dim reply As Integer

        charDetails = pilotXML.SelectNodes("/eveapi/result")
        If charDetails.Count <> 0 Then
            If charDetails(0).ChildNodes(0).OuterXml.StartsWith("<characterID>") = True Then
                ' Get the relevant node!
                For toonNo = 0 To charDetails.Count - 1
                    If charDetails(toonNo).HasChildNodes = True Then
                        pilotID = charDetails(toonNo).ChildNodes(0).InnerText
                        pilotName = charDetails(toonNo).ChildNodes(1).InnerText
                        ' Check if this pilot already exists
                        If HQ.Settings.Pilots.ContainsKey(pilotName) = True Then
                            reply = MessageBox.Show("This pilot already exists, would you like to update the pilot?", "Update Pilot?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If reply = DialogResult.No Then
                                Exit Sub
                            End If
                        End If
                        reply = MessageBox.Show("You will be importing pilot " & pilotName & " (id: " & pilotID & "). Do you want to import the training XML?", "Import Training XML?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.Yes Then
                            pilotTxml = ImportTrainingXML()
                            If pilotTxml Is Nothing Then
                                MessageBox.Show("Import of Training XML failed.", "Import Training XML failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                        End If
                    End If
                Next

                Dim newPilot As New EveHQPilot
                newPilot.Name = pilotName
                newPilot.ID = pilotID
                newPilot.Account = ""
                newPilot.AccountPosition = "0"
                HQ.TempPilots.Clear()
                HQ.TempPilots.Add(newPilot.Name, newPilot)
                pilotXML.Save(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.CharacterSheet.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                If pilotTxml IsNot Nothing Then
                    If pilotTxml.InnerText <> "" Then
                        pilotTxml.Save(Path.Combine(HQ.cacheFolder, "EVEHQAPI_" & APITypes.SkillQueue.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                    End If
                End If

                Dim currentPilot As EveHQPilot
                For Each currentPilot In HQ.TempPilots.Values
                    Call ParsePilotSkills(currentPilot, pilotXML)
                    Call ParsePilotXML(currentPilot, pilotXML)
                    Call ParseTrainingXML(currentPilot, pilotTxml)
                    Call BuildAttributeData(currentPilot)
                    Call CopyTempPilotsToMain()
                Next
            Else
                MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub
    Private Shared Function ImportTrainingXML() As XmlDocument
        Dim retry As Boolean = False
        Dim pilotXMLFile As String
        Dim pilotXML As XmlDocument = New XmlDocument
        Dim ofd1 As New OpenFileDialog
        Do
            With ofd1
                .Title = "Select Training XML file for Pilot Import"
                .FileName = ""
                .InitialDirectory = "c:\"
                .Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
                .FilterIndex = 1
                .RestoreDirectory = True
                If .ShowDialog() = DialogResult.OK Then
                    If My.Computer.FileSystem.FileExists(.FileName) = False Then
                        MessageBox.Show("File does not exist. Please re-try.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                    pilotXMLFile = .FileName
                    ' Examine the training XML file to see if it is viable
                    pilotXML.Load(pilotXMLFile)
                    Dim charDetails As XmlNodeList
                    Dim reply As Integer

                    ' Check if the file contains a valid "skillTraining" node
                    charDetails = pilotXML.SelectNodes("/eveapi/result")
                    If charDetails.Count = 0 Then
                        reply = MessageBox.Show("The XML file does not appear to be a valid Training XML file. Would you like to try again?", "Invalid XML File", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.Yes Then
                            retry = True
                        Else
                            Return Nothing
                        End If
                    Else
                        ' Check for a matching valid character
                        charDetails = pilotXML.SelectNodes("/eveapi/result")
                        If charDetails(0).ChildNodes(0).OuterXml.StartsWith("<trainingEndTime>") = True Then
                            ' Get the relevant node!
                            'For toonNo = 0 To CharDetails.Count - 1
                            '    If CharDetails(toonNo).HasChildNodes = True Then
                            '        Dim pilotName As String = CharDetails(toonNo).Attributes("name").Value
                            '        Dim pilotID As String = CharDetails(toonNo).Attributes("characterID").Value
                            '        ' Check if this pilot matches the original import
                            '        If pID <> pilotID Then
                            '            reply = MessageBox.Show("The Training XML does not match the original pilot, would you like to retry?", "Retry Training Import?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            '            If reply = Windows.Forms.DialogResult.Yes Then
                            '                Retry = True
                            '            Else
                            '                Retry = False
                            '                Return Nothing
                            '                Exit Function
                            '            End If
                            '        Else
                            '            Retry = False
                            '            Success = CStr(True)
                            '        End If
                            '    End If
                            'Next
                        End If
                    End If
                Else
                    MessageBox.Show("Import cancelled by user.", "Pilot Training Import Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return Nothing
                End If
            End With
        Loop Until retry = False
        Return pilotXML
    End Function
    Public Shared Sub SwitchImplants(ByVal iPilot As EveHQPilot)
        ' Decide whether to use Auto or Manual Implants
        If iPilot.UseManualImplants = True Then
            iPilot.CImplant = iPilot.CImplantM
            iPilot.IntImplant = iPilot.IntImplantM
            iPilot.MImplant = iPilot.MImplantM
            iPilot.PImplant = iPilot.PImplantM
            iPilot.WImplant = iPilot.WImplantM
        Else
            iPilot.CImplant = iPilot.CImplantA
            iPilot.IntImplant = iPilot.IntImplantA
            iPilot.MImplant = iPilot.MImplantA
            iPilot.PImplant = iPilot.PImplantA
            iPilot.WImplant = iPilot.WImplantA
        End If
        ' Rebuild the attribute data
        Call BuildAttributeData(iPilot)
    End Sub

End Class
