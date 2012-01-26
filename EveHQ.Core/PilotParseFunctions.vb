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
Imports System
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Xml
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Data
Imports System.Web
Imports System.Windows.Forms

Public Class PilotParseFunctions

    Shared SkillTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
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

    Public Shared Sub BuildAttributeData(ByRef cpilot As EveHQ.Core.Pilot)
        cpilot.WAttT = cpilot.WAtt + cpilot.WImplant
        cpilot.CAttT = cpilot.CAtt + cpilot.CImplant
        cpilot.IAttT = cpilot.IAtt + cpilot.IImplant
        cpilot.MAttT = cpilot.MAtt + cpilot.MImplant
        cpilot.PAttT = cpilot.PAtt + cpilot.PImplant
    End Sub           'BuildAttributeData    

    Public Shared Sub CopyTempPilotsToMain()

        ' Save pilot specific data first!!
        Call EveHQ.Core.EveHQSettingsFunctions.SaveTraining()

        ' Copy new pilot data
        Dim oldPilot, newPilot As EveHQ.Core.Pilot
        For Each newPilot In EveHQ.Core.HQ.TPilots.Values
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(newPilot.Name) Then
                oldPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(newPilot.Name), Pilot)
                ' Transfer old information first (stuff that isn't picked up in the XML download)!!
                newPilot.UseManualImplants = oldPilot.UseManualImplants
                newPilot.CImplantM = oldPilot.CImplantM
                newPilot.IImplantM = oldPilot.IImplantM
                newPilot.MImplantM = oldPilot.MImplantM
                newPilot.PImplantM = oldPilot.PImplantM
                newPilot.WImplantM = oldPilot.WImplantM
                newPilot.ActiveQueueName = oldPilot.ActiveQueueName
                newPilot.ActiveQueue = CType(oldPilot.ActiveQueue.Clone, SkillQueue)
                newPilot.Active = oldPilot.Active
                newPilot.PrimaryQueue = oldPilot.PrimaryQueue
				newPilot.Standings = oldPilot.Standings
				' Check if the old pilot has an account if using manual mode!!
                If oldPilot.Account <> "" And newPilot.Account = "" Then
                    newPilot.Account = oldPilot.Account
                    newPilot.AccountPosition = oldPilot.AccountPosition
                End If
                EveHQ.Core.HQ.EveHQSettings.Pilots.Remove(oldPilot.Name)
            End If
        Next
        For Each newPilot In EveHQ.Core.HQ.TPilots.Values
            ' Add the update info first to indicate it has been updated
            ' Check for some attribute that should not be blank or zero!
            If newPilot.SkillPoints <> 0 And newPilot.Corp <> "" Then
                newPilot.Updated = True
                newPilot.LastUpdate = Now.ToString
            End If
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(newPilot.Name) = False Then
                EveHQ.Core.HQ.EveHQSettings.Pilots.Add(newPilot, newPilot.Name)
            End If
        Next

        ' Reload pilot specific data!!
        Call EveHQ.Core.EveHQSettingsFunctions.LoadTraining()
        ' Reload pilot key skills
        Call LoadKeySkills()

    End Sub          'CopyTempPilotsToMain

    Public Shared Sub CopyTempCorpsToMain()

        ' Remove corps that will be replaced
        For Each NewCorp As Corporation In EveHQ.Core.HQ.TCorps.Values
            If EveHQ.Core.HQ.EveHQSettings.Corporations.ContainsKey(NewCorp.Name) Then
                EveHQ.Core.HQ.EveHQSettings.Corporations.Remove(NewCorp.Name)
            End If
        Next

        ' Copy new corp data
        For Each NewCorp As Corporation In EveHQ.Core.HQ.TCorps.Values
            ' Add the update info first to indicate it has been updated
            If EveHQ.Core.HQ.EveHQSettings.Corporations.ContainsKey(NewCorp.Name) = False Then
                EveHQ.Core.HQ.EveHQSettings.Corporations.Add(NewCorp.Name, NewCorp)
            End If
        Next

    End Sub

    Private Shared Sub GetPilotCachedInfo()
        Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        For Each currentPilot In EveHQ.Core.HQ.TPilots.Values
            ' Check if the 2 files exist in the cache
            Dim cXML As Boolean = False
            Dim tXML As Boolean = False
            Dim cXMLDoc, tXMLDoc As New XmlDocument
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.CharacterSheet.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml")) = True Then
                cXML = True
            End If
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.SkillQueue.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml")) = True Then
                tXML = True
            End If

            ' Only load in and parse if both files are available
            If cXML = True Then
                cXMLDoc.Load(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.CharacterSheet.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml"))
                If tXML = True Then
                    tXMLDoc.Load(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.SkillQueue.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml"))
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
        Dim curPilot As EveHQ.Core.Pilot
        For Each curPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If LoadKeySkillsForPilot(curPilot) = False Then
                Return False
            End If
        Next
        Return True
    End Function
    Public Shared Function LoadKeySkillsForPilot(ByVal curPilot As EveHQ.Core.Pilot) As Boolean
        Try
            Dim curSkill As EveHQ.Core.PilotSkill
            For Each curSkill In curPilot.PilotSkills
                Select Case curSkill.Name
                    Case "Mining"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Mining) = CStr(curSkill.Level)
                    Case "Mining Upgrades"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MiningUpgrades) = CStr(curSkill.Level)
                    Case "Astrogeology"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Astrogeology) = CStr(curSkill.Level)
                    Case "Mining Barge"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MiningBarge) = CStr(curSkill.Level)
                    Case "Mining Drone Operation"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MiningDrone) = CStr(curSkill.Level)
                    Case "Exhumers"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Exhumers) = CStr(curSkill.Level)
                    Case "Refining"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining) = CStr(curSkill.Level)
                    Case "Refinery Efficiency"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency) = CStr(curSkill.Level)
                    Case "Metallurgy"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Metallurgy) = CStr(curSkill.Level)
                    Case "Research"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Research) = CStr(curSkill.Level)
                    Case "Science"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Science) = CStr(curSkill.Level)
                    Case "Industry"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Industry) = CStr(curSkill.Level)
                    Case "Production Efficiency"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ProductionEfficiency) = CStr(curSkill.Level)
                    Case "Arkonor Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ArkonorProc) = CStr(curSkill.Level)
                    Case "Bistot Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BistotProc) = CStr(curSkill.Level)
                    Case "Crokite Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.CrokiteProc) = CStr(curSkill.Level)
                    Case "Dark Ochre Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.DarkOchreProc) = CStr(curSkill.Level)
                    Case "Gneiss Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.GneissProc) = CStr(curSkill.Level)
                    Case "Hedbergite Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.HedbergiteProc) = CStr(curSkill.Level)
                    Case "Hemorphite Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.HemorphiteProc) = CStr(curSkill.Level)
                    Case "Jaspet Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JaspetProc) = CStr(curSkill.Level)
                    Case "Kernite Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.KerniteProc) = CStr(curSkill.Level)
                    Case "Mercoxit Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MercoxitProc) = CStr(curSkill.Level)
                    Case "Omber Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.OmberProc) = CStr(curSkill.Level)
                    Case "Plagioclase Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.PlagioclaseProc) = CStr(curSkill.Level)
                    Case "Pyroxeres Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.PyroxeresProc) = CStr(curSkill.Level)
                    Case "Scordite Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ScorditeProc) = CStr(curSkill.Level)
                    Case "Spodumain Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.SpodumainProc) = CStr(curSkill.Level)
                    Case "Veldspar Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.VeldsparProc) = CStr(curSkill.Level)
                    Case "Ice Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.IceProc) = CStr(curSkill.Level)
                    Case "Ice Harvesting"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.IceHarvesting) = CStr(curSkill.Level)
                    Case "Deep Core Mining"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.DeepCoreMining) = CStr(curSkill.Level)
                    Case "Mining Foreman"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MiningForeman) = CStr(curSkill.Level)
                    Case "Mining Director"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MiningDirector) = CStr(curSkill.Level)
                    Case "Learning"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Learning) = CStr(curSkill.Level)
                    Case "Jump Drive Operation"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpDriveOperation) = CStr(curSkill.Level)
                    Case "Jump Drive Calibration"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration) = CStr(curSkill.Level)
                    Case "Jump Fuel Conservation"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFuelConservation) = CStr(curSkill.Level)
                    Case "Jump Freighters"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFreighters) = CStr(curSkill.Level)
                    Case "Scrapmetal Processing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ScrapMetalProc) = CStr(curSkill.Level)
                    Case "Accounting"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Accounting) = CStr(curSkill.Level)
                    Case "Broker Relations"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BrokerRelations) = CStr(curSkill.Level)
                    Case "Daytrading"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Daytrading) = CStr(curSkill.Level)
                    Case "Margin Trading"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MarginTrading) = CStr(curSkill.Level)
                    Case "Marketing"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Marketing) = CStr(curSkill.Level)
                    Case "Procurement"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Procurement) = CStr(curSkill.Level)
                    Case "Retail"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Retail) = CStr(curSkill.Level)
                    Case "Trade"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Trade) = CStr(curSkill.Level)
                    Case "Tycoon"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Tycoon) = CStr(curSkill.Level)
                    Case "Visibility"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Visibility) = CStr(curSkill.Level)
                    Case "Wholesale"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Wholesale) = CStr(curSkill.Level)
                    Case "Diploamcy"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Diplomacy) = CStr(curSkill.Level)
                    Case "Connections"
                        curPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Connections) = CStr(curSkill.Level)
                End Select
            Next
            Return True
        Catch e As Exception
            Return False
        End Try
    End Function
    Private Shared Sub ResetKeySkills()
        Dim curPilot As EveHQ.Core.Pilot
        For Each curPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            ReDim curPilot.KeySkills([Enum].GetValues(GetType(EveHQ.Core.Pilot.KeySkill)).Length)
            For keyskill As Integer = 0 To curPilot.KeySkills.GetUpperBound(0)
                curPilot.KeySkills(keyskill) = "0"
            Next
        Next
    End Sub

#Region "Eve API Retrieval Methods"

    Public Shared Sub GetCharactersInAccount(ByVal caccount As EveAccount)

        ' Check Account Status
        Call GetAccountStatus(caccount)

        ' Fetch the characters on account XML file
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        Dim accountXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.Characters, caccount.ToAPIAccount, EveAPI.APIReturnMethods.ReturnStandard)
        If APIReq.LastAPIResult = EveAPI.APIResults.CCPError Then
            EveHQ.Core.HQ.APIResults.Add(caccount.userID.ToString, -APIReq.LastAPIError)
        Else
            EveHQ.Core.HQ.APIResults.Add(caccount.userID.ToString, APIReq.LastAPIResult)
        End If

        ' Ignore for APIv2 corp keys
        If Not (caccount.APIKeySystem = APIKeySystems.Version2 And caccount.APIKeyType = APIKeyTypes.Corporation) Then

            If APIReq.LastAPIResult = EveAPI.APIResults.ReturnedActual Or APIReq.LastAPIResult = EveAPI.APIResults.ReturnedCached Or APIReq.LastAPIResult = EveAPI.APIResults.ReturnedNew Then
                If accountXML IsNot Nothing Then
                    ' Get characters
                    Dim charlist As XmlNodeList
                    Dim toon As XmlNode
                    Dim curr_toon As Integer = 0

                    ' Get the list of characters and the character IDs
                    charlist = accountXML.SelectNodes("/eveapi/result/rowset/row")
                    ' Clear the current characters on the account
                    caccount.Characters = New ArrayList
                    For Each toon In charlist
                        curr_toon += 1
                        ' Add the pilot details into the collection
                        Dim newPilot As New EveHQ.Core.Pilot
                        newPilot.Name = toon.Attributes.GetNamedItem("name").Value
                        newPilot.ID = toon.Attributes.GetNamedItem("characterID").Value
                        newPilot.AccountPosition = CStr(curr_toon)
                        newPilot.Account = caccount.userID
                        ' Copy notification data if available - we reset this after checking the API request if not cached
                        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(newPilot.Name) = True Then
                            newPilot.TrainingNotifiedEarly = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(newPilot.Name), EveHQ.Core.Pilot).TrainingNotifiedEarly
                            newPilot.TrainingNotifiedNow = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(newPilot.Name), EveHQ.Core.Pilot).TrainingNotifiedNow
                        Else
                            ' New character - let's download the image for them automatically :)
                            EveHQ.Core.ImageHandler.DownloadPortrait(newPilot.ID)
                        End If
                        If EveHQ.Core.HQ.TPilots.ContainsKey(newPilot.Name) = False Then
                            EveHQ.Core.HQ.TPilots.Add(newPilot.Name, newPilot)
                        End If
                        caccount.Characters.Add(newPilot.Name)
                        Call GetCharacterXMLs(caccount, newPilot)
                    Next

                    ' Check if we have any old pilots that the account does not have anymore
                    Dim oldPilots As String = ""
                    Dim oldPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                    For Each oldPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                        If oldPilot.Account = caccount.userID Then
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
                        msg &= "You have pilots registered in EveHQ that were previously assigned to account '" & caccount.userID & "'" & ControlChars.CrLf
                        msg &= "but are no longer part of that account. The following pilots have been converted to manual pilots:" & ControlChars.CrLf & ControlChars.CrLf
                        Dim olderPilots() As String = oldPilots.Split(CChar(","))
                        Dim dPilot As String = ""
                        For Each dPilot In olderPilots
                            msg &= dPilot & ControlChars.CrLf
                        Next
                        MessageBox.Show(msg, "Unused Pilots", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                End If
            End If
        Else
            Dim CorpList As XmlNodeList
            Dim Corp As XmlNode
            ' Add a corporation to the settings
            ' Get the list of characters and the character IDs
            CorpList = accountXML.SelectNodes("/eveapi/result/rowset/row")
            ' Clear the current characters on the account
            caccount.Characters = New ArrayList
            If CorpList.Count > 0 Then
                Corp = CorpList(0)
                Dim NewCorp As New EveHQ.Core.Corporation
                ' Get the existing corp if appropriate
                If EveHQ.Core.HQ.TCorps.ContainsKey(Corp.Attributes.GetNamedItem("corporationName").Value) = True Then
                    NewCorp = EveHQ.Core.HQ.TCorps(Corp.Attributes.GetNamedItem("corporationName").Value)
                Else
                    NewCorp.Name = Corp.Attributes.GetNamedItem("corporationName").Value
                    NewCorp.ID = Corp.Attributes.GetNamedItem("corporationID").Value
                    EveHQ.Core.HQ.TCorps.Add(NewCorp.Name, NewCorp)
                    caccount.Characters.Add(NewCorp.Name)
                End If
                If NewCorp.CharacterIDs.Contains(Corp.Attributes.GetNamedItem("characterID").Value) = False Then
                    NewCorp.CharacterIDs.Add(Corp.Attributes.GetNamedItem("characterID").Value)
                End If
                If NewCorp.CharacterNames.Contains(Corp.Attributes.GetNamedItem("name").Value) = False Then
                    NewCorp.CharacterNames.Add(Corp.Attributes.GetNamedItem("name").Value)
                End If
                If NewCorp.Accounts.Contains(caccount.userID) = False Then
                    NewCorp.Accounts.Add(caccount.userID)
                End If
            End If
        End If
    End Sub
    Private Shared Sub GetAccountStatus(ByRef cAccount As EveAccount)
        ' Attempts to get the AccountStatus API for additional information and for checking API key status
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        Dim accountXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.AccountStatus, cAccount.ToAPIAccount, EveAPI.APIReturnMethods.ReturnStandard)
        Select Case cAccount.APIKeySystem
            Case APIKeySystems.Version2
                Select Case APIReq.LastAPIError
                    Case -1
                        If accountXML IsNot Nothing Then
                            cAccount.LastAccountStatusCheck = Now
                            ' Parse the account information
                            If accountXML.GetElementsByTagName("createDate").Item(0).InnerText <> "" Then
                                Dim cd As Date = DateTime.ParseExact(accountXML.GetElementsByTagName("createDate").Item(0).InnerText, SkillTimeFormat, culture, System.Globalization.DateTimeStyles.None)
                                cAccount.CreateDate = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cd)
                                Dim pu As Date = DateTime.ParseExact(accountXML.GetElementsByTagName("paidUntil").Item(0).InnerText, SkillTimeFormat, culture, System.Globalization.DateTimeStyles.None)
                                cAccount.PaidUntil = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(pu)
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
                        cAccount.APIKeyType = Core.APIKeyTypes.Limited
                        cAccount.APIAccountStatus = APIAccountStatuses.Active
                    Case Else
                        ' Ignore
                End Select
        End Select     
    End Sub
	Private Shared Sub GetCharacterXMLs(ByVal cAccount As EveAccount, ByVal cPilot As EveHQ.Core.Pilot)

		' Set up an API Request for this character
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

		' Get the Character Sheet
		Dim cXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.CharacterSheet, cAccount.ToAPIAccount, cPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)

		' Store the Character Sheet API result
		If APIReq.LastAPIResult = EveAPI.APIResults.CCPError Then
			If EveHQ.Core.HQ.APIResults.ContainsKey(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.CharacterSheet) = False Then
				EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.CharacterSheet, -APIReq.LastAPIError)
			End If
		Else
			If EveHQ.Core.HQ.APIResults.ContainsKey(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.CharacterSheet) = False Then
				EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.CharacterSheet, APIReq.LastAPIResult)
			End If
		End If

		' Get the Skill Queue
		Dim tXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.SkillQueue, cAccount.ToAPIAccount, cPilot.ID, EveAPI.APIReturnMethods.ReturnStandard)

		' Store the Skill Queue API result
		If APIReq.LastAPIResult = EveAPI.APIResults.CCPError Then
			If EveHQ.Core.HQ.APIResults.ContainsKey(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.SkillQueue) = False Then
				EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.SkillQueue, -APIReq.LastAPIError)
			End If
		Else
			If EveHQ.Core.HQ.APIResults.ContainsKey(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.SkillQueue) = False Then
				EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveAPI.APITypes.SkillQueue, APIReq.LastAPIResult)
			End If
		End If

		' Only parse the sheets if both the CharacterSheet and TrainingQueue APIs are not null
		If cXML IsNot Nothing And tXML IsNot Nothing Then
			' Check if we need to reset the pilot notifications (if not a cached response)
			If APIReq.LastAPIResult = EveAPI.APIResults.ReturnedNew Or APIReq.LastAPIResult = EveAPI.APIResults.ReturnedActual Then
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
    Private Shared Sub ParsePilotXML(ByRef cPilot As EveHQ.Core.Pilot, ByVal CXMLDoc As XmlDocument)

        Dim CharDetails As XmlNodeList
        Dim toon As XmlNode
        Dim curr_toon As Integer = 0
        Dim toonNo As Integer = 0

        Dim nPilot As New EveHQ.Core.Pilot
        CharDetails = CXMLDoc.SelectNodes("/eveapi/result")
        toon = CharDetails(toonNo)
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
                .CloneSP = CXMLDoc.GetElementsByTagName("cloneSkillPoints").Item(0).InnerText
                Dim isk As Double = Double.Parse(CXMLDoc.GetElementsByTagName("balance").Item(0).InnerText, Globalization.NumberStyles.Any, culture)
                .Isk = isk
                ' Put cache info here??
            End With

            ' Get the implant details
            CharDetails = CXMLDoc.SelectNodes("/eveapi/result/attributeEnhancers")
            ' Get the relevant node!
            toon = CharDetails(0)       ' This is zero because there is only 1 occurence of the attributeEnhancers node in each XML doc
            If toon.HasChildNodes Then
                For implant As Integer = 0 To toon.ChildNodes.Count - 1
                    Select Case toon.ChildNodes(implant).Name
                        ' Save it in the Automatic implant sections
                        Case "perceptionBonus"
                            cPilot.PImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
                        Case "willpowerBonus"
                            cPilot.WImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
                        Case "intelligenceBonus"
                            cPilot.IImplantA = CInt(toon.ChildNodes(implant).ChildNodes.Item(1).InnerText)
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
                cPilot.IImplant = cPilot.IImplantM
                cPilot.MImplant = cPilot.MImplantM
                cPilot.PImplant = cPilot.PImplantM
                cPilot.WImplant = cPilot.WImplantM
            Else
                cPilot.CImplant = cPilot.CImplantA
                cPilot.IImplant = cPilot.IImplantA
                cPilot.MImplant = cPilot.MImplantA
                cPilot.PImplant = cPilot.PImplantA
                cPilot.WImplant = cPilot.WImplantA
            End If

            ' Get the attribute details
            CharDetails = CXMLDoc.SelectNodes("/eveapi/result/attributes")
            ' Get the relevant node!
            toon = CharDetails(0)       ' This is zero because there is only 1 occurence of the attributes node in each XML doc
            If toon.HasChildNodes Then
                For implant As Integer = 0 To toon.ChildNodes.Count - 1
                    Select Case toon.ChildNodes(implant).Name
                        Case "perception"
                            cPilot.PAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "willpower"
                            cPilot.WAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "intelligence"
                            cPilot.IAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "memory"
                            cPilot.MAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                        Case "charisma"
                            cPilot.CAtt = CInt(toon.ChildNodes(implant).ChildNodes.Item(0).InnerText)
                    End Select
                Next
            End If

            ' Get Cache details
            CharDetails = CXMLDoc.SelectNodes("/eveapi")
            cPilot.CacheFileTime = CDate(CharDetails(0).ChildNodes(0).InnerText)
            cPilot.CacheExpirationTime = CDate(CharDetails(0).ChildNodes(2).InnerText)
        Else
            cPilot.CacheFileTime = DateTime.Now.AddHours(1)
            cPilot.CacheExpirationTime = DateTime.Now.AddHours(1)
        End If

    End Sub                'ParsePilotXML
    Private Shared Sub ParseTrainingXML(ByRef cPilot As EveHQ.Core.Pilot, ByVal TXMLDoc As XmlDocument)
        ' Get the training details
        If TXMLDoc IsNot Nothing Then
            Dim TrainingDetails As XmlNodeList
            Dim trainingNode As XmlNode
            TrainingDetails = TXMLDoc.SelectNodes("/eveapi/result/rowset/row")
            ' Check if a training file has been loaded!
            If TrainingDetails.Count > 0 Then
                ' Get the first node i.e. the current training skill
                trainingNode = TrainingDetails(0)       ' This is zero because there is only 1 occurence of the skillTraining node in each XML doc
                With cPilot
                    If trainingNode.Attributes("startTime").Value <> "" And trainingNode.Attributes("endTime").Value <> "" Then
                        .Training = True
                        .TrainingSkillID = trainingNode.Attributes("typeID").Value
                        .TrainingSkillName = EveHQ.Core.SkillFunctions.SkillIDToName(.TrainingSkillID)
                        Dim dt As Date = DateTime.ParseExact(trainingNode.Attributes("startTime").Value, SkillTimeFormat, culture, System.Globalization.DateTimeStyles.None)
                        .TrainingStartTimeActual = dt
                        .TrainingStartTime = .TrainingStartTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                        dt = DateTime.ParseExact(trainingNode.Attributes("endTime").Value, SkillTimeFormat, culture, System.Globalization.DateTimeStyles.None)
                        .TrainingEndTimeActual = dt
                        .TrainingEndTime = .TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
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
                If TrainingDetails.Count > 0 Then
                    For queuedSkillNo As Integer = 0 To TrainingDetails.Count - 1
                        trainingNode = TrainingDetails(queuedSkillNo)
                        If trainingNode.Attributes("startTime").Value <> "" And trainingNode.Attributes("endTime").Value <> "" Then
                            Dim QueuedSkill As New PilotQueuedSkill
                            QueuedSkill.Position = CInt(trainingNode.Attributes("queuePosition").Value)
                            QueuedSkill.SkillID = CInt(trainingNode.Attributes("typeID").Value)
                            QueuedSkill.StartTime = DateTime.ParseExact(trainingNode.Attributes("startTime").Value, SkillTimeFormat, culture, System.Globalization.DateTimeStyles.None).AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                            QueuedSkill.EndTime = DateTime.ParseExact(trainingNode.Attributes("endTime").Value, SkillTimeFormat, culture, System.Globalization.DateTimeStyles.None).AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                            QueuedSkill.StartSP = CLng(trainingNode.Attributes("startSP").Value)
                            QueuedSkill.EndSP = CLng(trainingNode.Attributes("endSP").Value)
                            QueuedSkill.Level = CInt(trainingNode.Attributes("level").Value)
                            cPilot.QueuedSkills.Add(QueuedSkill.Position, QueuedSkill)
                            cPilot.QueuedSkillTime = CLng((QueuedSkill.EndTime - cPilot.TrainingEndTime).TotalSeconds)
                        End If
                    Next
                End If
            Else
                cPilot.Training = False
                cPilot.QueuedSkills.Clear()
                cPilot.QueuedSkillTime = 0
            End If
            ' Get Cache details
            TrainingDetails = TXMLDoc.SelectNodes("/eveapi")
            cPilot.TrainingFileTime = CDate(TrainingDetails(0).ChildNodes(0).InnerText)
            cPilot.TrainingExpirationTime = CDate(TrainingDetails(0).ChildNodes(2).InnerText)
        Else
            cPilot.Training = False
        End If
    End Sub
    Private Shared Sub ParsePilotSkills(ByRef cPilot As EveHQ.Core.Pilot, ByVal XMLDoc As XmlDocument)
        Dim CharDetails As XmlNodeList
        Dim toon As XmlNode
        Dim sp As Integer = 0
        Dim missingSkills As String = ""
        cPilot.PilotSkills.Clear()

        ' Start new nodelist
        CharDetails = XMLDoc.SelectNodes("/eveapi/result/rowset")
        For section As Integer = 0 To CharDetails.Count - 1
            Select Case CharDetails(section).Attributes.GetNamedItem("name").Value
                Case "skills"
                    toon = CharDetails(section)
                    sp = 0
                    ' Get list of skills within the groups!
                    For a As Integer = 0 To toon.ChildNodes.Count - 1
                        Dim newSkill As New EveHQ.Core.PilotSkill
                        ' Check if the skill exists in the database we have

                        newSkill.ID = toon.ChildNodes.Item(a).Attributes.GetNamedItem("typeID").Value
                        newSkill.SP = CInt(toon.ChildNodes.Item(a).Attributes.GetNamedItem("skillpoints").Value)
                        If toon.ChildNodes.Item(a).Attributes.GetNamedItem("level") IsNot Nothing Then
                            newSkill.Level = CInt(toon.ChildNodes.Item(a).Attributes.GetNamedItem("level").Value)
                        Else
                            newSkill.Level = EveHQ.Core.SkillFunctions.CalcLevelFromSP(newSkill.ID, newSkill.SP)
                        End If

                        sp = sp + newSkill.SP

                        ' Load API Data if we are a skill missing!
                        If EveHQ.Core.HQ.SkillListID.ContainsKey(newSkill.ID) = False Then
                            Call EveHQ.Core.SkillFunctions.LoadEveSkillDataFromAPI()
                        End If

                        If EveHQ.Core.HQ.SkillListID.ContainsKey(newSkill.ID) = True Then
                            Dim thisSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(newSkill.ID)
                            newSkill.Name = thisSkill.Name
                            newSkill.GroupID = thisSkill.GroupID
                            newSkill.Flag = 0
                            newSkill.Rank = thisSkill.Rank
                            newSkill.LevelUp(0) = 0
                            newSkill.LevelUp(1) = thisSkill.LevelUp(1)
                            newSkill.LevelUp(2) = thisSkill.LevelUp(2)
                            newSkill.LevelUp(3) = thisSkill.LevelUp(3)
                            newSkill.LevelUp(4) = thisSkill.LevelUp(4)
                            newSkill.LevelUp(5) = thisSkill.LevelUp(5)
                            cPilot.PilotSkills.Add(newSkill, newSkill.Name)
                            ' Check if a pilot skill is missing from the global skill list
                        Else
                            Dim missingSkill As EveHQ.Core.EveSkill = New EveHQ.Core.EveSkill
                            missingSkill.ID = newSkill.ID
                            missingSkill.Name = "Skill " & newSkill.ID
                            newSkill.Name = "Skill " & newSkill.ID         ' temp line to avoid error
                            missingSkill.GroupID = "267" : newSkill.GroupID = "267"
                            missingSkill.Rank = 20 : newSkill.Rank = 20
                            missingSkill.Level = newSkill.Level
                            missingSkill.PA = "Intelligence" : missingSkill.SA = "Memory"
                            newSkill.LevelUp(1) = 5000
                            newSkill.LevelUp(2) = 28284
                            newSkill.LevelUp(3) = 160000
                            newSkill.LevelUp(4) = 905097
                            newSkill.LevelUp(5) = 5120000
                            missingSkill.LevelUp(0) = newSkill.LevelUp(0)
                            missingSkill.LevelUp(1) = newSkill.LevelUp(1)
                            missingSkill.LevelUp(2) = newSkill.LevelUp(2)
                            missingSkill.LevelUp(3) = newSkill.LevelUp(3)
                            missingSkill.LevelUp(4) = newSkill.LevelUp(4)
                            missingSkill.LevelUp(5) = newSkill.LevelUp(5)
                            EveHQ.Core.HQ.SkillListName.Add(missingSkill.Name, missingSkill)
                            EveHQ.Core.HQ.SkillListID.Add(missingSkill.ID, missingSkill)
                            missingSkills &= newSkill.Name & ControlChars.CrLf
                            cPilot.PilotSkills.Add(newSkill, newSkill.Name)
                        End If
                        ' Check if the skillID is present but the skillname is different (CCP changing bloody skill names!!!)
                        If EveHQ.Core.HQ.SkillListID.ContainsKey(newSkill.ID) = True And EveHQ.Core.HQ.SkillListName.ContainsKey(newSkill.Name) = False Then
                            Dim changeSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(newSkill.ID)
                            Dim oldName As String = changeSkill.Name
                            changeSkill.Name = newSkill.Name
                            EveHQ.Core.HQ.SkillListID.Remove(newSkill.ID) : EveHQ.Core.HQ.SkillListID.Add(changeSkill.ID, changeSkill)
                            EveHQ.Core.HQ.SkillListName.Remove(oldName) : EveHQ.Core.HQ.SkillListName.Add(changeSkill.Name, changeSkill)
                        End If
                    Next
                    cPilot.SkillPoints = sp
                Case "certificates"
                    toon = CharDetails(section)
                    For Each certNode As XmlNode In toon.ChildNodes
                        cPilot.Certificates.Add(certNode.Attributes.GetNamedItem("certificateID").Value)
                    Next
                Case "corporationRoles"
                    For Each roleNode As XmlNode In CharDetails(section).ChildNodes
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
        Dim curPilot As EveHQ.Core.Pilot
        For Each curPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            For Each cSkill As EveHQ.Core.PilotSkill In curPilot.PilotSkills
                If EveHQ.Core.HQ.SkillListID.ContainsKey(cSkill.ID) = False Then
                    Call EveHQ.Core.SkillFunctions.LoadEveSkillDataFromAPI()
                    Exit Sub
                End If
            Next
        Next
    End Sub
    Private Shared Sub CheckMissingTrainingSkill(ByRef cPilot As EveHQ.Core.Pilot)
        Dim pilotSkill As New EveHQ.Core.PilotSkill
        ' Check if the main skill list has the skill we are checking for
        If EveHQ.Core.HQ.SkillListID.ContainsKey(cPilot.TrainingSkillID) = False Then
            Call EveHQ.Core.SkillFunctions.LoadEveSkillDataFromAPI()
        End If
        If cPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(cPilot.TrainingSkillID)) = False Then
            ' The pilot doesn't have this skill so let's add it manually
            Dim baseSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(cPilot.TrainingSkillID)
            pilotSkill.ID = baseSkill.ID
            pilotSkill.Name = baseSkill.Name
            pilotSkill.Flag = 0
            pilotSkill.Rank = baseSkill.Rank
            pilotSkill.GroupID = baseSkill.GroupID
            pilotSkill.Level = cPilot.TrainingSkillLevel - 1
            For l As Integer = 0 To 5
                pilotSkill.LevelUp(l) = baseSkill.LevelUp(l)
            Next
            pilotSkill.SP = cPilot.TrainingStartSP
            cPilot.PilotSkills.Add(pilotSkill, pilotSkill.Name)
        Else
            Dim sq As Core.PilotSkill = CType(cPilot.PilotSkills(EveHQ.Core.SkillFunctions.SkillIDToName(cPilot.TrainingSkillID)), Core.PilotSkill)
            sq.Level = cPilot.TrainingSkillLevel - 1
        End If

    End Sub

#End Region

    Public Shared Sub ImportPilotFromXML(ByVal pilotXMLFile As String, ByVal pilotTXMLFile As String)
        ' Need to determine which pilot we are going to import

        ' Load the XML file
        Dim pilotName As String = ""
        Dim pilotID As String = ""
        Dim pilotXML As XmlDocument = New XmlDocument
        Dim pilotTXML As XmlDocument = New XmlDocument
        pilotXML.Load(pilotXMLFile)
        pilotTXML.Load(pilotTXMLFile)

        ' Find out which char has child nodes - this is the one we want
        Dim CharDetails As XmlNodeList
        Dim toonNo As Integer

        CharDetails = pilotXML.SelectNodes("/eveapi/result")
        If CharDetails.Count <> 0 Then
            If CharDetails(0).ChildNodes(0).OuterXml.StartsWith("<characterID>") = True Then
                ' Get the relevant node!
                For toonNo = 0 To CharDetails.Count - 1
                    If CharDetails(toonNo).HasChildNodes = True Then
                        pilotID = CharDetails(toonNo).ChildNodes(0).InnerText
                        pilotName = CharDetails(toonNo).ChildNodes(1).InnerText
                    End If
                Next

                Dim newPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                newPilot.Name = pilotName
                newPilot.ID = pilotID
                newPilot.Account = ""
                newPilot.AccountPosition = "0"
                newPilot.Updated = True
                EveHQ.Core.HQ.TPilots.Clear()
                EveHQ.Core.HQ.TPilots.Add(newPilot.Name, newPilot)
                pilotXML.Save(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.CharacterSheet.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                If pilotTXML IsNot Nothing Then
                    pilotTXML.Save(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.SkillQueue.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                End If

                Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                For Each currentPilot In EveHQ.Core.HQ.TPilots.Values
                    Call EveHQ.Core.PilotParseFunctions.ParsePilotSkills(currentPilot, pilotXML)
                    Call EveHQ.Core.PilotParseFunctions.ParsePilotXML(currentPilot, pilotXML)
                    Call EveHQ.Core.PilotParseFunctions.ParseTrainingXML(currentPilot, pilotTXML)
                    Call EveHQ.Core.PilotParseFunctions.BuildAttributeData(currentPilot)
                    Call EveHQ.Core.PilotParseFunctions.CopyTempPilotsToMain()
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
        Dim pilotXMLFile As String = ""
        Dim ofd1 As New Windows.Forms.OpenFileDialog
        With ofd1
            .Title = "Select XML file for Pilot Import"
            .FileName = ""
            .InitialDirectory = "c:\"
            .Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
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
    Private Shared Sub ExaminePilotXML(ByVal PilotXMLFile As String)
        ' Need to determine which pilot we are going to import

        ' Load the XML file
        Dim pilotName As String = ""
        Dim pilotID As String = ""
        Dim pilotXML As XmlDocument = New XmlDocument
        Dim pilotTXML As XmlDocument = New XmlDocument
        pilotXML.Load(PilotXMLFile)

        ' Find out which char has child nodes - this is the one we want
        Dim CharDetails As XmlNodeList
        Dim toonNo As Integer
        Dim reply As Integer

        CharDetails = pilotXML.SelectNodes("/eveapi/result")
        If CharDetails.Count <> 0 Then
            If CharDetails(0).ChildNodes(0).OuterXml.StartsWith("<characterID>") = True Then
                ' Get the relevant node!
                For toonNo = 0 To CharDetails.Count - 1
                    If CharDetails(toonNo).HasChildNodes = True Then
                        pilotID = CharDetails(toonNo).ChildNodes(0).InnerText
                        pilotName = CharDetails(toonNo).ChildNodes(1).InnerText
                        ' Check if this pilot already exists
                        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(pilotName) = True Then
                            reply = MessageBox.Show("This pilot already exists, would you like to update the pilot?", "Update Pilot?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If reply = Windows.Forms.DialogResult.No Then
                                Exit Sub
                            End If
                        End If
                        reply = MessageBox.Show("You will be importing pilot " & pilotName & " (id: " & pilotID & "). Do you have want to import the training XML?", "Import Training XML?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = Windows.Forms.DialogResult.Yes Then
                            pilotTXML = ImportTrainingXML(pilotID)
                            If pilotTXML Is Nothing Then
                                MessageBox.Show("Import of Training XML failed.", "Import Training XML failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                        End If
                    End If
                Next

                Dim newPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                newPilot.Name = pilotName
                newPilot.ID = pilotID
                newPilot.Account = ""
                newPilot.AccountPosition = "0"
                EveHQ.Core.HQ.TPilots.Clear()
                EveHQ.Core.HQ.TPilots.Add(newPilot.Name, newPilot)
                pilotXML.Save(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.CharacterSheet.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                If pilotTXML IsNot Nothing Then
                    If pilotTXML.InnerText <> "" Then
                        pilotTXML.Save(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.SkillQueue.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml"))
                    End If
                End If

                Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                For Each currentPilot In EveHQ.Core.HQ.TPilots.Values
                    Call EveHQ.Core.PilotParseFunctions.ParsePilotSkills(currentPilot, pilotXML)
                    Call EveHQ.Core.PilotParseFunctions.ParsePilotXML(currentPilot, pilotXML)
                    Call EveHQ.Core.PilotParseFunctions.ParseTrainingXML(currentPilot, pilotTXML)
                    Call EveHQ.Core.PilotParseFunctions.BuildAttributeData(currentPilot)
                    Call EveHQ.Core.PilotParseFunctions.CopyTempPilotsToMain()
                Next
            Else
                MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            MessageBox.Show("The XML file does not appear to be a valid Character XML file.", "Invalid XML File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub
    Private Shared Function ImportTrainingXML(ByVal pID As String) As XmlDocument
        Dim Success As String = ""
        Dim Retry As Boolean = False
        Dim pilotXMLFile As String = ""
        Dim pilotXML As XmlDocument = New XmlDocument
        Dim ofd1 As New Windows.Forms.OpenFileDialog
        Do
            With ofd1
                .Title = "Select Training XML file for Pilot Import"
                .FileName = ""
                .InitialDirectory = "c:\"
                .Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
                .FilterIndex = 1
                .RestoreDirectory = True
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    If My.Computer.FileSystem.FileExists(.FileName) = False Then
                        MessageBox.Show("File does not exist. Please re-try.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                    pilotXMLFile = .FileName
                    ' Examine the training XML file to see if it is viable
                    pilotXML.Load(pilotXMLFile)
                    Dim CharDetails As XmlNodeList
                    Dim reply As Integer

                    ' Check if the file contains a valid "skillTraining" node
                    CharDetails = pilotXML.SelectNodes("/eveapi/result")
                    If CharDetails.Count = 0 Then
                        reply = MessageBox.Show("The XML file does not appear to be a valid Training XML file. Would you like to try again?", "Invalid XML File", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = Windows.Forms.DialogResult.Yes Then
                            Retry = True
                        Else
                            Retry = False
                            Return Nothing
                            Exit Function
                        End If
                    Else
                        ' Check for a matching valid character
                        CharDetails = pilotXML.SelectNodes("/eveapi/result")
                        If CharDetails(0).ChildNodes(0).OuterXml.StartsWith("<trainingEndTime>") = True Then
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
                    Exit Function
                End If
            End With
        Loop Until Retry = False
        Return pilotXML
    End Function
    Public Shared Sub SwitchImplants(ByVal iPilot As EveHQ.Core.Pilot)
        ' Decide whether to use Auto or Manual Implants
        If iPilot.UseManualImplants = True Then
            iPilot.CImplant = iPilot.CImplantM
            iPilot.IImplant = iPilot.IImplantM
            iPilot.MImplant = iPilot.MImplantM
            iPilot.PImplant = iPilot.PImplantM
            iPilot.WImplant = iPilot.WImplantM
        Else
            iPilot.CImplant = iPilot.CImplantA
            iPilot.IImplant = iPilot.IImplantA
            iPilot.MImplant = iPilot.MImplantA
            iPilot.PImplant = iPilot.PImplantA
            iPilot.WImplant = iPilot.WImplantA
        End If
        ' Rebuild the attribute data
        Call EveHQ.Core.PilotParseFunctions.BuildAttributeData(iPilot)
    End Sub

End Class
