' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
        ' Learning Skill = 3374 = "Learning"
        ' Basic Willpower = 3375 = "Iron Will"
        ' Basic Charisma = 3376 = "Empathy"
        ' Basic Intelligence = 3377 = "Analytical Mind"
        ' Basic Memory = 3378 = "Instant Recall"
        ' Basic Perception = 3379 = "Spatial Awareness"
        ' Advanced Willpower = 12386 = "Focus"
        ' Charisma = 12383 = "Presence"
        ' Intelligence = 12376 = "Logic"
        ' Memory = 12385 = "Eidetic Memory"
        ' Perception = 12387 = "Clarity"

        Dim basicSkill As EveHQ.Core.Skills = New EveHQ.Core.Skills

        ' Process Basic Learning Skills
        If cpilot.PilotSkills.Contains("Iron Will") = True Then
            basicSkill = CType(cpilot.PilotSkills("Iron Will"), Skills)
            cpilot.LWAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Empathy") = True Then
            basicSkill = CType(cpilot.PilotSkills("Empathy"), Skills)
            cpilot.LCAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Analytical Mind") = True Then
            basicSkill = CType(cpilot.PilotSkills("Analytical Mind"), Skills)
            cpilot.LIAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Instant Recall") = True Then
            basicSkill = CType(cpilot.PilotSkills("Instant Recall"), Skills)
            cpilot.LMAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Spatial Awareness") = True Then
            basicSkill = CType(cpilot.PilotSkills("Spatial Awareness"), Skills)
            cpilot.LPAtt = basicSkill.Level
        End If

        ' Process Advanced Learning Skills
        If cpilot.PilotSkills.Contains("Focus") = True Then
            basicSkill = CType(cpilot.PilotSkills("Focus"), Skills)
            cpilot.ALWAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Presence") = True Then
            basicSkill = CType(cpilot.PilotSkills("Presence"), Skills)
            cpilot.ALCAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Logic") = True Then
            basicSkill = CType(cpilot.PilotSkills("Logic"), Skills)
            cpilot.ALIAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Eidetic Memory") = True Then
            basicSkill = CType(cpilot.PilotSkills("Eidetic Memory"), Skills)
            cpilot.ALMAtt = basicSkill.Level
        End If
        If cpilot.PilotSkills.Contains("Clarity") = True Then
            basicSkill = CType(cpilot.PilotSkills("Clarity"), Skills)
            cpilot.ALPAtt = basicSkill.Level
        End If

        ' Get learning skill details
        Dim learningFactor As Integer = 0
        If cpilot.PilotSkills.Contains("Learning") = True Then
            basicSkill = CType(cpilot.PilotSkills("Learning"), Skills)
            learningFactor = basicSkill.Level
        End If

        ' Calculate learning skill increase
        Dim WT, CT, IT, MT, PT As Double
        WT = cpilot.WAtt + cpilot.LWAtt + cpilot.ALWAtt + cpilot.WImplant
        cpilot.LSWAtt = WT * (learningFactor / 50)
        cpilot.WAttT = WT + cpilot.LSWAtt

        CT = cpilot.CAtt + cpilot.LCAtt + cpilot.ALCAtt + cpilot.CImplant
        cpilot.LSCAtt = CT * (learningFactor / 50)
        cpilot.CAttT = CT + cpilot.LSCAtt

        IT = cpilot.IAtt + cpilot.LIAtt + cpilot.ALIAtt + cpilot.IImplant
        cpilot.LSIAtt = IT * (learningFactor / 50)
        cpilot.IAttT = IT + cpilot.LSIAtt

        MT = cpilot.MAtt + cpilot.LMAtt + cpilot.ALMAtt + cpilot.MImplant
        cpilot.LSMAtt = MT * (learningFactor / 50)
        cpilot.MAttT = MT + cpilot.LSMAtt

        PT = cpilot.PAtt + cpilot.LPAtt + cpilot.ALPAtt + cpilot.PImplant
        cpilot.LSPAtt = PT * (learningFactor / 50)
        cpilot.PAttT = PT + cpilot.LSPAtt

    End Sub           'BuildAttributeData    

    Public Shared Sub CopyTempPilotsToMain()

        ' Save pilot specific data first!!
        Call EveHQ.Core.EveHQSettingsFunctions.SaveTraining()

        ' Copy new pilot data
        Dim oldPilot, newPilot As EveHQ.Core.Pilot
        For Each newPilot In EveHQ.Core.HQ.TPilots
            For Each oldPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If oldPilot.Name = newPilot.Name Then
                    ' Transfer old information first (stuff that isn't picked up in the XML download)!!
                    newPilot.UseManualImplants = oldPilot.UseManualImplants
                    newPilot.CImplantM = oldPilot.CImplantM
                    newPilot.IImplantM = oldPilot.IImplantM
                    newPilot.MImplantM = oldPilot.MImplantM
                    newPilot.PImplantM = oldPilot.PImplantM
                    newPilot.WImplantM = oldPilot.WImplantM
                    newPilot.Active = oldPilot.Active
                    ' Check if the old pilot has an account if using manual mode!!
                    If oldPilot.Account <> "" And newPilot.Account = "" Then
                        newPilot.Account = oldPilot.Account
                        newPilot.AccountPosition = oldPilot.AccountPosition
                    End If
                    EveHQ.Core.HQ.EveHQSettings.Pilots.Remove(oldPilot.Name)
                End If
            Next
        Next
        For Each newPilot In EveHQ.Core.HQ.TPilots
            ' Add the update info first to indicate it has been updated
            ' Check for some attribute that should not be blank or zero!
            If newPilot.SkillPoints <> 0 And newPilot.Corp <> "" Then
                newPilot.Updated = True
                newPilot.LastUpdate = Now.ToString
            End If
            EveHQ.Core.HQ.EveHQSettings.Pilots.Add(newPilot, newPilot.Name)
        Next

        ' Reload pilot specific data!!
        Call EveHQ.Core.EveHQSettingsFunctions.LoadTraining()
        ' Reload pilot key skills
        Call LoadKeySkills()

    End Sub          'CopyTempPilotsToMain
    Private Shared Sub GetPilotCachedInfo()
        Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        For Each currentPilot In EveHQ.Core.HQ.TPilots
            ' Check if the 2 files exist in the cache
            Dim cXML As Boolean = False
            Dim tXML As Boolean = False
            Dim cXMLDoc, tXMLDoc As New XmlDocument
            If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml") = True Then
                cXML = True
            End If
            If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.SkillTraining.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml") = True Then
                tXML = True
            End If

            ' Only load in and parse if both files are available
            If cXML = True Then
                cXMLDoc.Load(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml")
                If tXML = True Then
                    tXMLDoc.Load(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.SkillTraining.ToString & "_" & currentPilot.Account & "_" & currentPilot.ID & ".xml")
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
    Public Shared Sub LoadKeySkills()
        Call ResetKeySkills()
        Dim curPilot As EveHQ.Core.Pilot
        For Each curPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Call LoadKeySkillsForPilot(curPilot)
        Next
    End Sub
    Public Shared Sub LoadKeySkillsForPilot(ByVal curPilot As EveHQ.Core.Pilot)

        Dim curSkill As EveHQ.Core.Skills
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
            End Select
        Next
    End Sub
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

    Public Shared Function GetCharactersInAccount(ByVal cAccount As EveAccount) As Integer

        ' Fetch the characters on account XML file
        Dim accountXML As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Characters, cAccount, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
        If EveHQ.Core.EveAPI.LastAPIResult = EveAPI.APIResults.CCPError Then
            EveHQ.Core.HQ.APIResults.Add(cAccount.userID.ToString, -EveAPI.LastAPIError)
        Else
            EveHQ.Core.HQ.APIResults.Add(cAccount.userID.ToString, EveAPI.LastAPIResult)
        End If

        If EveAPI.LastAPIResult = EveAPI.APIResults.ReturnedActual Or EveAPI.LastAPIResult = EveAPI.APIResults.ReturnedCached Or EveAPI.LastAPIResult = EveAPI.APIResults.ReturnedNew Then

            ' Get characters
            Dim charlist As XmlNodeList
            Dim toon As XmlNode
            Dim curr_toon As Integer = 0

            ' Get the list of characters and the character IDs
            charlist = accountXML.SelectNodes("/eveapi/result/rowset/row")
            For Each toon In charlist
                curr_toon += 1
                ' Add the pilot details into the collection
                Dim newPilot As New EveHQ.Core.Pilot
                newPilot.Name = toon.Attributes.GetNamedItem("name").Value
                newPilot.ID = toon.Attributes.GetNamedItem("characterID").Value
                newPilot.AccountPosition = CStr(curr_toon)
                newPilot.Account = cAccount.userID
                EveHQ.Core.HQ.TPilots.Add(newPilot, newPilot.Name)
                Call GetCharacterXMLs(cAccount, newPilot)
            Next

            ' Check if we have any old pilots that the account does not have anymore
            Dim oldPilots As String = ""
            Dim oldPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
            For Each oldPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If oldPilot.Account = cAccount.userID Then
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
                msg &= "You have pilots registered in EveHQ that were previously assigned to account '" & cAccount.userID & "'" & ControlChars.CrLf
                msg &= "but are no longer part of that account. The following pilots have been converted to manual pilots:" & ControlChars.CrLf & ControlChars.CrLf
                Dim olderPilots() As String = oldPilots.Split(CChar(","))
                Dim dPilot As String = ""
                For Each dPilot In olderPilots
                    msg &= dPilot & ControlChars.CrLf
                Next
                MessageBox.Show(msg, "Unused Pilots", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End If
        Return EveAPI.LastAPIResult

    End Function
    Private Shared Sub GetCharacterXMLs(ByVal cAccount As EveAccount, ByVal cPilot As EveHQ.Core.Pilot)

        Dim cXML As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.CharacterSheet, cAccount, cPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
        If EveHQ.Core.EveAPI.LastAPIResult = EveAPI.APIResults.CCPError Then
            EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet, -EveHQ.Core.EveAPI.LastAPIError)
        Else
            EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet, EveHQ.Core.EveAPI.LastAPIResult)
        End If
        Dim tXML As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.SkillTraining, cAccount, cPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnStandard)
        If EveHQ.Core.EveAPI.LastAPIResult = EveAPI.APIResults.CCPError Then
            EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveHQ.Core.EveAPI.APIRequest.SkillTraining, -EveHQ.Core.EveAPI.LastAPIError)
        Else
            EveHQ.Core.HQ.APIResults.Add(cAccount.userID & "_" & cPilot.Name & "_" & EveHQ.Core.EveAPI.APIRequest.SkillTraining, EveHQ.Core.EveAPI.LastAPIResult)
        End If
        Call ParsePilotSkills(cPilot, cXML)
        Call ParsePilotXML(cPilot, cXML)
        Call ParseTrainingXML(cPilot, tXML)
        Call BuildAttributeData(cPilot)

    End Sub
    Private Shared Sub ParsePilotXML(ByRef cPilot As EveHQ.Core.Pilot, ByVal CXMLDoc As XmlDocument)

        Dim CharDetails As XmlNodeList
        Dim toon As XmlNode
        Dim curr_toon As Integer = 0
        Dim toonNo As Integer = 0

        Dim name As XmlNodeList = CXMLDoc.GetElementsByTagName("name")

        Dim nPilot As New EveHQ.Core.Pilot
        CharDetails = CXMLDoc.SelectNodes("/eveapi/result")
        toon = CharDetails(toonNo)
        ' Get the Pilot name & charID in the character node
        With cPilot
            ' Get the additional pilot data nodes
            .Name = toon.ChildNodes.Item(1).InnerText
            .Race = toon.ChildNodes.Item(2).InnerText
            .Blood = toon.ChildNodes.Item(3).InnerText
            .Gender = toon.ChildNodes.Item(4).InnerText
            .Corp = toon.ChildNodes.Item(5).InnerText
            .CorpID = toon.ChildNodes.Item(6).InnerText
            .CloneName = toon.ChildNodes.Item(7).InnerText
            .CloneSP = toon.ChildNodes.Item(8).InnerText
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            Dim isk As Double = Double.Parse(toon.ChildNodes.Item(9).InnerText, Globalization.NumberStyles.Number, culture)
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

    End Sub                'ParsePilotXML
    Private Shared Sub ParseTrainingXML(ByRef cPilot As EveHQ.Core.Pilot, ByVal TXMLDoc As XmlDocument)
        ' Get the training details
        If TXMLDoc IsNot Nothing Then
            Dim CharDetails As XmlNodeList
            Dim toon As XmlNode
            CharDetails = TXMLDoc.SelectNodes("/eveapi/result")
            ' Check if a training file has been loaded!
            If CharDetails.Count <> 0 Then
                ' Get the relevant node!
                toon = CharDetails(0)       ' This is zero because there is only 1 occurence of the skillTraining node in each XML doc
                If toon.HasChildNodes Then
                    With cPilot
                        If toon.ChildNodes.Count = 8 Then
                            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
                            .Training = True
                            .TrainingSkillID = toon.ChildNodes(3).InnerText
                            .TrainingSkillName = EveHQ.Core.SkillFunctions.SkillIDToName(.TrainingSkillID)
                            Dim dt As DateTime = DateTime.Parse(toon.ChildNodes(2).InnerText, culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                            .TrainingStartTimeActual = CDate(dt.ToString)
                            .TrainingStartTime = .TrainingStartTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                            dt = DateTime.Parse(toon.ChildNodes(1).InnerText, culture, System.Globalization.DateTimeStyles.NoCurrentDateDefault)
                            .TrainingEndTimeActual = CDate(dt.ToString)
                            .TrainingEndTime = .TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                            .TrainingStartSP = CInt(toon.ChildNodes(4).InnerText)
                            .TrainingEndSP = CInt(toon.ChildNodes(5).InnerText)
                            .TrainingSkillLevel = CInt(toon.ChildNodes(6).InnerText)
                            Call CheckMissingTrainingSkill(cPilot)
                        Else
                            .Training = False
                        End If
                    End With
                End If
            Else
                cPilot.Training = False
            End If
            ' Get Cache details
            CharDetails = TXMLDoc.SelectNodes("/eveapi")
            cPilot.TrainingFileTime = CDate(CharDetails(0).ChildNodes(0).InnerText)
            cPilot.TrainingExpirationTime = CDate(CharDetails(0).ChildNodes(2).InnerText)
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
                        Dim newSkill As New EveHQ.Core.Skills
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
                        If EveHQ.Core.HQ.SkillListID.Contains(newSkill.ID) = False Then
                            Call EveHQ.Core.SkillFunctions.LoadEveSkillDataFromAPI()
                        End If

                        If EveHQ.Core.HQ.SkillListID.Contains(newSkill.ID) = True Then
                            Dim thisSkill As SkillList = CType(EveHQ.Core.HQ.SkillListID(newSkill.ID), SkillList)
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
                            Dim missingSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
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
                            EveHQ.Core.HQ.SkillListName.Add(missingSkill, missingSkill.Name)
                            EveHQ.Core.HQ.SkillListID.Add(missingSkill, missingSkill.ID)
                            missingSkills &= newSkill.Name & ControlChars.CrLf
                            cPilot.PilotSkills.Add(newSkill, newSkill.Name)
                        End If
                        ' Check if the skillID is present but the skillname is different (CCP changing bloody skill names!!!)
                        If EveHQ.Core.HQ.SkillListID.Contains(newSkill.ID) = True And EveHQ.Core.HQ.SkillListName.Contains(newSkill.Name) = False Then
                            Dim changeSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(newSkill.ID), SkillList)
                            Dim oldName As String = changeSkill.Name
                            changeSkill.Name = newSkill.Name
                            EveHQ.Core.HQ.SkillListID.Remove(newSkill.ID) : EveHQ.Core.HQ.SkillListID.Add(changeSkill, changeSkill.ID)
                            EveHQ.Core.HQ.SkillListName.Remove(oldName) : EveHQ.Core.HQ.SkillListName.Add(changeSkill, changeSkill.Name)
                        End If
                    Next
                    cPilot.SkillPoints = sp
                Case "certificates"
                    toon = CharDetails(section)
                    For Each certNode As XmlNode In toon.ChildNodes
                        cPilot.Certificates.Add(certNode.Attributes.GetNamedItem("certificateID").Value)
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
            For Each cSkill As EveHQ.Core.Skills In curPilot.PilotSkills
                If EveHQ.Core.HQ.SkillListID.Contains(cSkill.ID) = False Then
                    Call EveHQ.Core.SkillFunctions.LoadEveSkillDataFromAPI()
                    Exit Sub
                End If
            Next
        Next
    End Sub
    Private Shared Sub CheckMissingTrainingSkill(ByRef cPilot As EveHQ.Core.Pilot)
        Dim pilotSkill As New EveHQ.Core.Skills
        ' Check if the main skill list has the skill we are checking for
        If EveHQ.Core.HQ.SkillListID.Contains(cPilot.TrainingSkillID) = False Then
            Call EveHQ.Core.SkillFunctions.LoadEveSkillDataFromAPI()
        End If
        If cPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(cPilot.TrainingSkillID)) = False Then
            ' The pilot doesn't have this skill so let's add it manually
            Dim baseSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(cPilot.TrainingSkillID), Core.SkillList)
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
                EveHQ.Core.HQ.TPilots.Add(newPilot)
                pilotXML.Save(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml")
                If pilotTXML IsNot Nothing Then
                    pilotTXML.Save(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.SkillTraining.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml")
                End If

                Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                For Each currentPilot In EveHQ.Core.HQ.TPilots
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
                EveHQ.Core.HQ.TPilots.Add(newPilot)
                pilotXML.Save(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml")
                If pilotTXML IsNot Nothing Then
                    If pilotTXML.InnerText <> "" Then
                        pilotTXML.Save(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.SkillTraining.ToString & "_" & newPilot.Account & "_" & newPilot.ID & ".xml")
                    End If
                End If

                Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                For Each currentPilot In EveHQ.Core.HQ.TPilots
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
    Public Shared Sub SwitchImplants()
        ' Decide whether to use Auto or Manual Implants
        If EveHQ.Core.HQ.myPilot.UseManualImplants = True Then
            EveHQ.Core.HQ.myPilot.CImplant = EveHQ.Core.HQ.myPilot.CImplantM
            EveHQ.Core.HQ.myPilot.IImplant = EveHQ.Core.HQ.myPilot.IImplantM
            EveHQ.Core.HQ.myPilot.MImplant = EveHQ.Core.HQ.myPilot.MImplantM
            EveHQ.Core.HQ.myPilot.PImplant = EveHQ.Core.HQ.myPilot.PImplantM
            EveHQ.Core.HQ.myPilot.WImplant = EveHQ.Core.HQ.myPilot.WImplantM
        Else
            EveHQ.Core.HQ.myPilot.CImplant = EveHQ.Core.HQ.myPilot.CImplantA
            EveHQ.Core.HQ.myPilot.IImplant = EveHQ.Core.HQ.myPilot.IImplantA
            EveHQ.Core.HQ.myPilot.MImplant = EveHQ.Core.HQ.myPilot.MImplantA
            EveHQ.Core.HQ.myPilot.PImplant = EveHQ.Core.HQ.myPilot.PImplantA
            EveHQ.Core.HQ.myPilot.WImplant = EveHQ.Core.HQ.myPilot.WImplantA
        End If
        ' Rebuild the attribute data
        Call EveHQ.Core.PilotParseFunctions.BuildAttributeData(EveHQ.Core.HQ.myPilot)
    End Sub

End Class
