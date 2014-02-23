'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Globalization
Imports System.Xml
Imports EveHQ.EveApi
Imports EveHQ.EveData
Imports System.IO
Imports System.Windows.Forms
Imports EveHQ.Common.Extensions
Imports System.Xml.Linq

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
                If oldPilot.ActiveQueue IsNot Nothing Then
                    newPilot.ActiveQueue = CType(oldPilot.ActiveQueue.Clone, EveHQSkillQueue)
                End If

                newPilot.Active = oldPilot.Active
                newPilot.PrimaryQueue = oldPilot.PrimaryQueue
                newPilot.Standings = oldPilot.Standings
                newPilot.TrainingQueues = oldPilot.TrainingQueues
                ' Check if the old pilot has an account if using manual mode!!
                If oldPilot.Account <> String.Empty And newPilot.Account = String.Empty Then
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

    Public Shared Sub GetCharactersInAccount(ByVal caccount As EveHQAccount)

        If caccount Is Nothing Then
            Return
        End If

        ' TODO: Troubleshoot cached API result deserialization issue.

        ' Check the API key status
        caccount.CheckAPIKey()
        ' Check Account Status
        If caccount.APIKeyType <> APIKeyTypes.Corporation Then
            Call GetAccountStatus(caccount)
        End If

        ' Fetch the characters on account XML file
        Dim characterServiceResponse As EveServiceResponse(Of IEnumerable(Of AccountCharacter)) =
                HQ.ApiProvider.Account.Characters(caccount.UserID, caccount.APIKey)

        ' Ignore for APIv2 corp keys
        If Not (caccount.ApiKeySystem = APIKeySystems.Version2 And caccount.APIKeyType = APIKeyTypes.Corporation) Then

            'If APIReq.LastAPIResult = EveAPI.APIResults.ReturnedActual Or APIReq.LastAPIResult = EveAPI.APIResults.ReturnedCached Or APIReq.LastAPIResult = EveAPI.APIResults.ReturnedNew Then
            If characterServiceResponse.IsSuccessfulHttpStatus Then
                If characterServiceResponse.ResultData IsNot Nothing Then
                    ' Get characters
                    Dim currToon As Integer = 0
                    ' Clear the current characters on the account
                    caccount.Characters = New List(Of String)
                    For Each toon As AccountCharacter In characterServiceResponse.ResultData
                        currToon += 1
                        ' Add the pilot details into the collection
                        Dim newPilot As New EveHQPilot
                        newPilot.Name = toon.Name
                        newPilot.ID = toon.CharacterId.ToInvariantString()
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
                            For Each toon As AccountCharacter In characterServiceResponse.ResultData
                                If toon.Name = oldPilot.Name Then
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
                        msg &= "You have pilots registered in EveHQ that were previously assigned to account '" &
                               caccount.UserID & "'" & ControlChars.CrLf
                        msg &=
                            "but are no longer part of that account. The following pilots have been converted to manual pilots:" &
                            ControlChars.CrLf & ControlChars.CrLf
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
            Dim CorpList As List(Of AccountCharacter)
            If (characterServiceResponse IsNot Nothing And characterServiceResponse.IsSuccess) Then
                ' Add a corporation to the settings
                ' Get the list of characters and the character IDs
                CorpList = characterServiceResponse.ResultData.ToList()
                ' Clear the current characters on the account6
                caccount.Characters = New List(Of String)
                If CorpList.Count > 0 Then
                    Dim corp As AccountCharacter = CorpList(0)
                    Dim newCorp As New Corporation
                    ' Get the existing corp if appropriate
                    If HQ.TempCorps.ContainsKey(corp.CorporationName) = True Then
                        newCorp = HQ.TempCorps(corp.CorporationName)
                    Else
                        newCorp.Name = corp.CorporationName
                        newCorp.ID = corp.CorporationId.ToInvariantString()
                        HQ.TempCorps.Add(newCorp.Name, newCorp)
                        caccount.Characters.Add(newCorp.Name)
                    End If
                    If newCorp.CharacterIDs.Contains(corp.CharacterId.ToInvariantString()) = False Then
                        newCorp.CharacterIDs.Add(corp.CharacterId.ToInvariantString())
                    End If
                    If newCorp.CharacterNames.Contains(corp.Name) = False Then
                        newCorp.CharacterNames.Add(corp.Name)
                    End If
                    If newCorp.Accounts.Contains(caccount.UserID) = False Then
                        newCorp.Accounts.Add(caccount.UserID)
                    End If
                End If
            End If
        End If
    End Sub

    Private Shared Sub GetAccountStatus(ByRef cAccount As EveHQAccount)
        ' Attempts to get the AccountStatus API for additional information and for checking API key status
        'Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHqSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        'Dim accountXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.AccountStatus, cAccount.ToAPIAccount, EveAPI.APIReturnMethods.ReturnStandard)
        Dim response As EveServiceResponse(Of Account) = HQ.ApiProvider.Account.AccountStatus(cAccount.UserID,
                                                                                              cAccount.APIKey)

        If response.IsSuccessfulHttpStatus Then
            If response.IsSuccess Then
                cAccount.LastAccountStatusCheck = Now
                ' Parse the account information

                Dim cd As Date = response.ResultData.CreateDate.DateTime
                cAccount.CreateDate = SkillFunctions.ConvertEveTimeToLocal(cd)
                Dim pu As Date = response.ResultData.ExpiryDate.DateTime
                cAccount.PaidUntil = SkillFunctions.ConvertEveTimeToLocal(pu)
                cAccount.LogonCount = response.ResultData.LogOnCount
                cAccount.LogonMinutes = CLng(response.ResultData.LoggedInTime.TotalMinutes)
                If cAccount.PaidUntil < Now Then
                    ' Account has expired
                    cAccount.APIAccountStatus = APIAccountStatuses.Disabled
                Else
                    cAccount.APIAccountStatus = APIAccountStatuses.Active
                End If
            Else
                If response.EveErrorCode > 0 Then
                    Select response.EveErrorCode
                        Case 211
                            '' Account has expired
                            cAccount.APIAccountStatus = APIAccountStatuses.Disabled
                        Case 200
                            ' Should be limited key
                            cAccount.APIKeyType = Core.APIKeyTypes.Limited
                            cAccount.APIAccountStatus = APIAccountStatuses.Active

                    End Select
                End If
            End If
        End If
    End Sub

    Private Shared Sub GetCharacterXmLs(ByVal cAccount As EveHQAccount, ByVal cPilot As EveHQPilot)

        ' Set up an API Request for this character
        'Dim _
        '    apiReq As _
        '        New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)

        ' Get the Character Sheet
        'Dim cXML As XmlDocument = apiReq.GetAPIXML(APITypes.CharacterSheet, cAccount.ToAPIAccount, cPilot.ID,
        '                                           APIReturnMethods.ReturnStandard)

        Dim characterSheetResponse As EveServiceResponse(Of CharacterData) =
                HQ.ApiProvider.Character.CharacterSheet(cAccount.UserID, cAccount.APIKey, cPilot.ID.ToInt32())

        ' Store the Character Sheet API result
        If characterSheetResponse.EveErrorCode > 0 Then
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet) = False _
                Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet,
                                  -characterSheetResponse.EveErrorCode)
            End If
        Else
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet) = False _
                Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.CharacterSheet,
                                  characterSheetResponse.HttpStatusCode)
            End If
        End If


        ' Get the Skill Queue
        Dim skillResponse As EveServiceResponse(Of IEnumerable(Of QueuedSkill)) = HQ.ApiProvider.Character.SkillQueue(cAccount.UserID, cAccount.APIKey, cPilot.ID.ToInt32())

        ' Store the Skill Queue API result
        If skillResponse.EveErrorCode > 0 Then
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue) = False Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue, -skillResponse.EveErrorCode)
            End If
        Else
            If HQ.APIResults.ContainsKey(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue) = False Then
                HQ.APIResults.Add(cAccount.UserID & "_" & cPilot.Name & "_" & APITypes.SkillQueue, skillResponse.HttpStatusCode)
            End If
        End If

        ' Only parse the sheets if both the CharacterSheet and TrainingQueue APIs are not null
        If characterSheetResponse.ResultData IsNot Nothing And skillResponse.ResultData IsNot Nothing Then
            ' Check if we need to reset the pilot notifications (if not a cached response)
            If characterSheetResponse.CachedResponse = False Then
                cPilot.TrainingNotifiedEarly = False
                cPilot.TrainingNotifiedNow = False
            End If

            ' Parse the API data
            Call ParsePilotSkills(cPilot, characterSheetResponse.ResultData)
            Call ParsePilotXML(cPilot, characterSheetResponse)
            Call ParseTrainingXML(cPilot, skillResponse)
            Call BuildAttributeData(cPilot)
        End If
    End Sub

    Private Shared Sub ParsePilotXML(ByRef cPilot As EveHQPilot, ByVal data As EveServiceResponse(Of CharacterData))

        If data Is Nothing Then
            Return
        End If

        Dim charDetails As XmlNodeList
        Dim toon As XmlNode
        Const ToonNo As Integer = 0
        Dim charData As CharacterData = data.ResultData

        If charData IsNot Nothing Then
            ' Get the Pilot name & charID in the character node
            With cPilot
                ' Get the additional pilot data nodes
                .Name = charData.Name
                .Race = charData.Race
                .Blood = charData.BloodLine
                .Gender = charData.Gender
                .Corp = charData.CorporationName
                .CorpID = charData.CorporationId.ToInvariantString
                .CloneName = charData.CloneName
                .CloneSP = charData.CloneSkillPoints
                Dim isk As Double = charData.Balance
                .Isk = isk
                ' Put cache info here??
            End With

            ' Get the implant details
            cPilot.PImplantA = charData.PerceptionBonus.Value

            cPilot.WImplantA = charData.WillpowerBonus.Value

            cPilot.IntImplantA = charData.IntelligenceBonus.Value

            cPilot.MImplantA = charData.MemoryBonus.Value

            cPilot.CImplantA = charData.CharismaBonus.Value

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
            cPilot.PAtt = charData.Perception
            cPilot.WAtt = charData.Willpower
            cPilot.IntAtt = charData.Intelligence
            cPilot.MAtt = charData.Memory
            cPilot.CAtt = charData.Charisma


            ' Get Cache details

            cPilot.CacheFileTime = data.CacheUntil.DateTime
            cPilot.CacheExpirationTime = data.CacheUntil.DateTime
        Else
            cPilot.CacheFileTime = DateTime.Now.AddHours(1)
            cPilot.CacheExpirationTime = DateTime.Now.AddHours(1)
        End If
    End Sub                'ParsePilotXML
    Private Shared Sub ParseTrainingXML(ByRef cPilot As EveHQPilot, ByVal data As EveServiceResponse(Of IEnumerable(Of QueuedSkill)))
        ' Get the training details
        If data IsNot Nothing Then
            Dim skills As IEnumerable(Of QueuedSkill) = data.ResultData

            Dim trainingDetails As XmlNodeList
            Dim trainingNode As XmlNode

            Dim skillTraining As QueuedSkill = Nothing
            Dim queuedTraining As IEnumerable(Of QueuedSkill) = Nothing

            If skills.Any() Then
                ' Get the first node i.e. the current training skill
                skillTraining = skills.First()
                queuedTraining = skills
            End If

            ' Check if a training file has been loaded!
            If skillTraining IsNot Nothing Then
                With cPilot
                    If _
                       skillTraining.StartTime <> DateTimeOffset.MinValue And
                        skillTraining.EndTime <> DateTimeOffset.MinValue Then
                        .Training = True
                        .TrainingSkillID = skillTraining.TypeId
                        .TrainingSkillName = SkillFunctions.SkillIDToName(.TrainingSkillID)
                        'Dim dt As Date = DateTime.ParseExact(trainingNode.Attributes("startTime").Value, SkillTimeFormat,
                        '                                    Culture, DateTimeStyles.None)
                        .TrainingStartTimeActual = skillTraining.StartTime.UtcDateTime
                        .TrainingStartTime = .TrainingStartTimeActual.AddSeconds(HQ.Settings.ServerOffset)
                        'dt = DateTime.ParseExact(trainingNode.Attributes("endTime").Value, SkillTimeFormat, Culture,
                        '                        DateTimeStyles.None)
                        .TrainingEndTimeActual = skillTraining.EndTime.UtcDateTime
                        .TrainingEndTime = .TrainingEndTimeActual.AddSeconds(HQ.Settings.ServerOffset)
                        .TrainingStartSP = skillTraining.StartSP
                        .TrainingEndSP = skillTraining.EndSP
                        .TrainingSkillLevel = skillTraining.Level
                        Call CheckMissingTrainingSkill(cPilot)
                    Else
                        cPilot.Training = False
                        cPilot.QueuedSkills.Clear()
                        cPilot.QueuedSkillTime = 0
                    End If
                End With
                ' Now get any additional results and add them to the pilot queued skills
                cPilot.QueuedSkills.Clear()
                If queuedTraining.Any() Then
                    For Each skill As QueuedSkill In queuedTraining
                        If _
                            skill.StartTime <> DateTimeOffset.MinValue And
                            skill.EndTime <> DateTimeOffset.MinValue Then
                            Dim queuedSkill As New EveHQPilotQueuedSkill
                            queuedSkill.Position = skill.QueuePosition
                            queuedSkill.SkillID = skill.TypeId
                            queuedSkill.StartTime = skill.StartTime.UtcDateTime.AddSeconds(HQ.Settings.ServerOffset)

                            queuedSkill.EndTime = skill.EndTime.UtcDateTime.AddSeconds(HQ.Settings.ServerOffset)

                            queuedSkill.StartSP = skill.StartSP
                            queuedSkill.EndSP = skill.EndSP
                            queuedSkill.Level = skill.Level
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
            cPilot.TrainingFileTime = data.CacheUntil.Date
            cPilot.TrainingExpirationTime = data.CacheUntil.Date
        Else
            cPilot.Training = False
        End If
    End Sub

    Private Shared Sub ParsePilotSkills(ByRef cPilot As EveHQPilot, ByVal characterData As CharacterData)
        Dim totalSkillPoints As Integer
        Dim missingSkills As String = ""
        cPilot.PilotSkills.Clear()



        '   charDetails = xmlDoc.SelectNodes("/eveapi/result/rowset")

        ' Case "skills"

        ' Get list of skills within the groups!
        For Each skill As CharacterSkillRecord In characterData.Skills
            Dim newSkill As New EveHQPilotSkill
            ' Check if the skill exists in the database we have

            newSkill.ID = skill.SkillId
            newSkill.SP = skill.SkillPoints
            If skill.Level > 0 Then
                newSkill.Level = skill.Level
            Else
                newSkill.Level = SkillFunctions.CalcLevelFromSP(newSkill.ID, newSkill.SP)
            End If

            totalSkillPoints = totalSkillPoints + newSkill.SP

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
                missingSkill.GroupID = 267
                newSkill.GroupID = 267
                missingSkill.Rank = 20
                newSkill.Rank = 20
                missingSkill.Level = newSkill.Level
                missingSkill.Pa = "Intelligence"
                missingSkill.Sa = "Memory"
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
            If _
                HQ.SkillListID.ContainsKey(newSkill.ID) = True And
                HQ.SkillListName.ContainsKey(newSkill.Name) = False Then
                Dim changeSkill As EveSkill = HQ.SkillListID(newSkill.ID)
                Dim oldName As String = changeSkill.Name
                changeSkill.Name = newSkill.Name
                HQ.SkillListID.Remove(newSkill.ID)
                HQ.SkillListID.Add(changeSkill.ID, changeSkill)
                HQ.SkillListName.Remove(oldName)
                HQ.SkillListName.Add(changeSkill.Name, changeSkill)
            End If
        Next

        cPilot.SkillPoints = totalSkillPoints

        'Case "corporationRoles"
        For Each roleNode As CharacterCorporationRoles In characterData.CorporationRoles
            cPilot.CorpRoles.Add(CType(roleNode.RoleId, CorporationRoles))
        Next

        cPilot.QualifiedCertificates = RetrieveCertificates(cPilot)


        ' If missing skills were identified then report that fact!
        If missingSkills <> "" Then
            Dim msg As String = ""
            msg &= cPilot.Name & " has skills that are not listed in the database. These skills are:" &
                   ControlChars.CrLf & ControlChars.CrLf
            msg &= missingSkills & ControlChars.CrLf
            msg &=
                "These skills have been added to the database on a temporary basis but the information is incomplete." &
                ControlChars.CrLf
            msg &=
                "Any calcaulations performed on the above skills will contain errors until the main database is updated." &
                ControlChars.CrLf
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

    Public Shared Function RetrieveCertificates(ByRef cPilot As EveHQPilot) As Dictionary(Of Integer, CertificateGrade)
        Dim qualifiedCerts As New Dictionary(Of Integer, CertificateGrade)

        For Each cert In StaticData.Certificates.Values
            For Each grade In cert.GradesAndSkills.Keys.ToList()
                If CheckPilotSkillsForCertGrade(cert.GradesAndSkills(grade), cPilot) Then
                    If qualifiedCerts.ContainsKey(cert.Id) Then
                        qualifiedCerts(cert.Id) = grade
                    Else
                        qualifiedCerts.Add(cert.Id, grade)
                    End If
                End If
            Next
        Next
        Return qualifiedCerts
    End Function

    Private Shared Function CheckPilotSkillsForCertGrade(ByRef reqSkills As SortedList(Of Integer, Integer), ByRef pilot As EveHQPilot) As Boolean
        Dim qualifications As New SortedList(Of Integer, Boolean)
        For Each skill In reqSkills.Keys

            qualifications.Add(skill, False)
            Dim pSkill = pilot.PilotSkills.FirstOrDefault(Function(s) s.Value.ID = skill)
            If pSkill.Value IsNot Nothing Then
                If pSkill.Value.Level >= reqSkills(skill) Then
                    qualifications(skill) = True
                End If
            End If
        Next
        Return qualifications.Values.All(Function(q) q = True)
    End Function


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

    Public Shared Sub ImportPilotFromXML(ByVal pilotData As EveServiceResponse(Of CharacterData), ByVal trainingData As EveServiceResponse(Of IEnumerable(Of QueuedSkill)))
        ' Need to determine which pilot we are going to import

        If pilotData IsNot Nothing Then
            Dim newPilot As New EveHQPilot
            newPilot.Name = pilotData.ResultData.Name
            newPilot.ID = pilotData.ResultData.CharacterId.ToString
            newPilot.Account = "0"
            newPilot.AccountPosition = "0"
            newPilot.Updated = True
            HQ.TempPilots.Clear()
            HQ.TempPilots.Add(newPilot.Name, newPilot)

            For Each currentPilot As EveHQPilot In HQ.TempPilots.Values
                Call ParsePilotSkills(currentPilot, pilotData.ResultData)
                Call ParsePilotXML(currentPilot, pilotData)
                Call ParseTrainingXML(currentPilot, trainingData)
                Call BuildAttributeData(currentPilot)
                Call CopyTempPilotsToMain()
            Next
        Else
            MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
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
                    MessageBox.Show("File does not exist. Please re-try.", "File Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation)
                End If
                pilotXMLFile = .FileName
                Call ExaminePilotXML(pilotXMLFile)
            Else
                MessageBox.Show("Import cancelled by user.", "Pilot Import Cancelled", MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                Exit Sub
            End If
        End With
    End Sub

    Private Shared Sub ExaminePilotXML(ByVal pilotXMLFile As String)
        ' Need to determine which pilot we are going to import

        ' Load the XML file
        Dim pilotName As String = ""
        Dim pilotID As String = ""
        Dim pilotXML As XDocument = XDocument.Load(pilotXMLFile)
        Dim pilotTxml As XDocument = New XDocument



        ' Find out which char has child nodes - this is the one we want
        Dim charDetails As XElement
        Dim toonNo As Integer
        Dim reply As Integer

        charDetails = pilotXML.Root.Element("result")
        If charDetails IsNot Nothing Then
            Dim charData As CharacterData = CharacterClient.ParseCharacterSheetResponse(charDetails)
            If charData IsNot Nothing Then
                ' Get the relevant node!

                pilotID = charData.CharacterId.ToInvariantString()
                pilotName = charData.Name
                ' Check if this pilot already exists
                If HQ.Settings.Pilots.ContainsKey(pilotName) = True Then
                    reply = MessageBox.Show("This pilot already exists, would you like to update the pilot?",
                                            "Update Pilot?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = DialogResult.No Then
                        Exit Sub
                    End If
                End If
                reply =
                    MessageBox.Show(
                        "You will be importing pilot " & pilotName & " (id: " & pilotID &
                        "). Do you want to import the training XML?", "Import Training XML?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.Yes Then
                    pilotTxml = ImportTrainingXML()
                    If pilotTxml Is Nothing Then
                        MessageBox.Show("Import of Training XML failed.", "Import Training XML failed",
                                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If


                Dim newPilot As New EveHQPilot
                newPilot.Name = pilotName
                newPilot.ID = pilotID
                newPilot.Account = "0"
                newPilot.AccountPosition = "0"
                HQ.TempPilots.Clear()
                HQ.TempPilots.Add(newPilot.Name, newPilot)

                Dim fakecharService As New EveServiceResponse(Of CharacterData)
                fakecharService.ResultData = charData
                fakecharService.CacheUntil = DateTimeOffset.Now.AddYears(10)
                fakecharService.HttpStatusCode = Net.HttpStatusCode.OK
                fakecharService.IsSuccessfulHttpStatus = True
                fakecharService.EveErrorCode = 0

                Dim skillQueue As IEnumerable(Of QueuedSkill) = CorpCharBaseClient.ProcessSkillQueueResponse(pilotTxml.Root.Element("result"))
                Dim fakeTrainingResponse As New EveServiceResponse(Of IEnumerable(Of QueuedSkill))
                fakeTrainingResponse.ResultData = skillQueue
                fakeTrainingResponse.CacheUntil = DateTimeOffset.Now.AddYears(10)
                fakeTrainingResponse.HttpStatusCode = Net.HttpStatusCode.OK
                fakeTrainingResponse.IsSuccessfulHttpStatus = True
                fakeTrainingResponse.EveErrorCode = 0

                'pilotXML.Save(Path.Combine(HQ.CacheFolder,
                '                           "EVEHQAPI_" & APITypes.CharacterSheet.ToString & "_" & newPilot.Account & "_" &
                '                           newPilot.ID & ".xml"))
                'If pilotTxml IsNot Nothing Then
                '    If pilotTxml.InnerText <> "" Then
                '        pilotTxml.Save(Path.Combine(HQ.CacheFolder,
                '                                    "EVEHQAPI_" & APITypes.SkillQueue.ToString & "_" & newPilot.Account &
                '                                    "_" & newPilot.ID & ".xml"))
                '    End If
                'End If

                Dim currentPilot As EveHQPilot
                For Each currentPilot In HQ.TempPilots.Values
                    Call ParsePilotSkills(currentPilot, fakecharService.ResultData)
                    Call ParsePilotXML(currentPilot, fakecharService)
                    Call ParseTrainingXML(currentPilot, fakeTrainingResponse)
                    Call BuildAttributeData(currentPilot)
                    Call CopyTempPilotsToMain()
                Next
            Else
                MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Shared Function ImportTrainingXML() As XDocument
        Dim retry As Boolean = False
        Dim pilotXMLFile As String
        Dim pilotXML As XDocument = New XDocument
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
                        MessageBox.Show("File does not exist. Please re-try.", "File Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation)
                    End If
                    pilotXMLFile = .FileName
                    ' Examine the training XML file to see if it is viable
                    pilotXML = XDocument.Load(pilotXMLFile)
                    Dim charDetails As XElement
                    Dim reply As Integer

                    ' Check if the file contains a valid "skillTraining" node
                    Try
                        charDetails = pilotXML.Root.Element("result")
                    Catch ex As Exception

                    End Try

                    If charDetails Is Nothing Then
                        reply =
                            MessageBox.Show(
                                "The XML file does not appear to be a valid Training XML file. Would you like to try again?",
                                "Invalid XML File", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.Yes Then
                            retry = True
                        Else
                            Return Nothing
                        End If
                    End If
                Else
                    MessageBox.Show("Import cancelled by user.", "Pilot Training Import Cancelled", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
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
