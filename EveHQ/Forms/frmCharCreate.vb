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

Imports EveHQ.Core.CoreReports
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports System.IO
Imports EveHQ.Common.Extensions
Imports Newtonsoft.Json

Namespace Forms

    Public Class FrmCharCreate
        Dim _raceID As Integer = 0
        Dim _raceName As String = ""
        Dim _bloodName As String = ""
        ReadOnly _skillsRace As New Collection
        ReadOnly _eveRaces As New SortedList(Of String, Integer)

        Private Sub frmCharCreate_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            _eveRaces.Add("Caldari", 1)
            _eveRaces.Add("Minmatar", 2)
            _eveRaces.Add("Amarr", 4)
            _eveRaces.Add("Gallente", 8)

            cboRace.BeginUpdate()
            cboRace.Items.Clear()
            For Each race As String In _eveRaces.Keys
                cboRace.Items.Add(race)
            Next
            cboRace.EndUpdate()

        End Sub

        Private Sub cboRace_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboRace.SelectedIndexChanged

            ' If we have chosen something, then activate the next control
            If cboRace.SelectedIndex > -1 Then
                Select Case cboRace.SelectedItem.ToString
                    Case "Amarr"
                        _bloodName = "Amarr"
                    Case "Caldari"
                        _bloodName = "Deteis"
                    Case "Gallente"
                        _bloodName = "Gallente"
                    Case "Minmatar"
                        _bloodName = "Brutor"
                End Select
            End If


            ' Store the raceID and raceName
            _raceName = CStr(cboRace.SelectedItem)
            _raceID = CInt(_eveRaces.Item(_raceName))

            Call CalcRaceSkills(CStr(_raceID))

            ' Set the list of skills & attributes
            lvwSkills.Items.Clear()


            ' Display skills
            lvwSkills.Items.Clear()
            Dim skillPoints As Integer = 0
            For Each skill As ListViewItem In _skillsRace
                lvwSkills.Items.Add(skill)
                skillPoints += CInt(skill.SubItems(2).Text)
            Next
            lblSP.Text = "Skillpoints: " & skillPoints.ToString("N0")

            ' Enable character options
            lblSelectAttributes.Enabled = True
            lblSelectChar.Enabled = True
            nudC.Enabled = True
            nudI.Enabled = True
            nudM.Enabled = True
            nudP.Enabled = True
            nudW.Enabled = True
            txtCharName.Enabled = True
            btnAddPilot.Enabled = True

            ' Generate an ID based on date and time
            Dim charID As String = Format(Now, "MddHHmmss")
            lblCharID.Text = charID
        End Sub

        Private Sub CalcRaceSkills(ByVal raceID As String)
            ' Extract RaceSkills from resources
            Dim raceSkills As New ArrayList
            Dim raceSkillsTable As String = My.Resources.RaceSkillsTable
            Dim raceSkillLines() As String = raceSkillsTable.Split(ControlChars.CrLf.ToCharArray)
            For Each raceSkill As String In raceSkillLines
                Dim raceSkillData() As String = raceSkill.Split(",".ToCharArray)
                If raceSkillData(0) = raceID Then
                    raceSkills.Add(raceSkillData(1) & "," & raceSkillData(2))
                End If
            Next

            ' Load up the skills for the selected race
            _skillsRace.Clear()
            Dim skillID As String
            Dim skillName As String
            Dim skillLevel As Integer
            Dim skillPoints As Long
            For Each raceskill As String In raceSkills
                Dim raceSkillData() As String = raceskill.Split(",".ToCharArray)
                skillID = raceSkillData(0)
                skillLevel = CInt(raceSkillData(1))
                skillName = SkillFunctions.SkillIDToName(CInt(skillID))
                skillPoints = CLng(Math.Ceiling(SkillFunctions.CalculateSkillSPLevel(CInt(skillID), skillLevel)))
                Dim skillItem As New ListViewItem
                skillItem.Text = skillName
                skillItem.Name = skillID
                skillItem.SubItems.Add(skillLevel.ToString)
                skillItem.SubItems.Add(skillPoints.ToString)
                _skillsRace.Add(skillItem, skillItem.Text)
            Next
        End Sub

        Private Sub nud_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudC.ValueChanged, nudI.ValueChanged, nudM.ValueChanged, nudP.ValueChanged, nudW.ValueChanged
            Dim attTotal As Integer = CInt(nudC.Value + nudI.Value + nudM.Value + nudP.Value + nudW.Value)
            lblAttTotal.Text = CStr(attTotal)
            If attTotal = 39 Then
                btnAddPilot.Enabled = True
            Else
                btnAddPilot.Enabled = False
            End If
        End Sub

        Private Sub btnAddPilot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddPilot.Click

            ' Create a new pilot
            Dim nPilot As New CharacterData

            nPilot.CharacterId = lblCharID.Text.ToInt32()
            ' Check name isn't blank
            If txtCharName.Text.Trim = "" Then
                MessageBox.Show("Please ensure you have entered a pilot name before importing into EveHQ.", "Pilot Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                nPilot.Name = txtCharName.Text
            End If
            ' Check if name exists
            If HQ.Settings.Pilots.ContainsKey(nPilot.Name) = True Then
                MessageBox.Show("Pilot name already exists. Please choose an alternative name.", "Pilot Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            nPilot.Race = _raceName
            nPilot.BloodLine = _bloodName
            nPilot.Charisma = CInt(nudC.Value)
            nPilot.Intelligence = CInt(nudI.Value)
            nPilot.Memory = CInt(nudM.Value)
            nPilot.Perception = CInt(nudP.Value)
            nPilot.Willpower = CInt(nudW.Value)
            nPilot.Balance = 0
            'nPilot. = True
            nPilot.CorporationName = "EveHQ Import Corp"
            nPilot.CorporationId = 1000000
            nPilot.AllianceName = ""
            nPilot.Gender = "Male"
            nPilot.Ancestry = ""
            nPilot.CloneName = "Clone Grade Alpha"
            nPilot.CloneSkillPoints = 900000
            nPilot.MemoryBonus = New AttributeEnhancer()
            nPilot.PerceptionBonus = New AttributeEnhancer()
            nPilot.WillpowerBonus = New AttributeEnhancer()
            nPilot.IntelligenceBonus = New AttributeEnhancer()
            nPilot.CharismaBonus = New AttributeEnhancer()
            nPilot.Certificates = New List(Of Integer)
            nPilot.CorporationRoles = New CharacterCorporationRoles() {}
            nPilot.CorporationRolesAtHq = New CharacterCorporationRoles() {}
            nPilot.CorporationRolesAtBase = New CharacterCorporationRoles() {}
            nPilot.CorporationRolesAtOthers = New CharacterCorporationRoles() {}
            nPilot.CorporationTitles = New CharacterCorporationTitles() {}

            Dim skills As New List(Of CharacterSkillRecord)

            For Each skillItem As ListViewItem In lvwSkills.Items
                Dim pilotSkill As New CharacterSkillRecord
                pilotSkill.SkillId = CInt(skillItem.Name)

                pilotSkill.Level = CInt(skillItem.SubItems(1).Text)
                pilotSkill.SkillPoints = CInt(skillItem.SubItems(2).Text)
                skills.Add(pilotSkill)
            Next
            nPilot.Skills = skills

            'nPilot.Updated = True

            ' Write the json files
            Dim xmlFile As String = Path.Combine(HQ.ApiCacheFolder, "CharacterSheet0" & "_" & nPilot.CharacterId & ".json.txt")
            Dim txmlFile As String = Path.Combine(HQ.ApiCacheFolder, "SkillQueue0" & "_" & nPilot.CharacterId & ".json.txt")

            'Dim writer As StreamWriter

            ' Write Character JSON
            Dim fakeServiceResponse As New EveServiceResponse(Of CharacterData)
            fakeServiceResponse.ResultData = nPilot
            fakeServiceResponse.CacheUntil = DateTimeOffset.Now.AddYears(10)
            fakeServiceResponse.HttpStatusCode = Net.HttpStatusCode.OK
            fakeServiceResponse.IsSuccessfulHttpStatus = True
            fakeServiceResponse.EveErrorCode = 0



            Dim charData As String = JsonConvert.SerializeObject(fakeServiceResponse)

            ' Write fake training JSON
            Dim fakeTrainingResponse As New EveServiceResponse(Of IEnumerable(Of QueuedSkill))
            fakeTrainingResponse.ResultData = New List(Of QueuedSkill)
            fakeTrainingResponse.CacheUntil = DateTimeOffset.Now.AddYears(10)
            fakeTrainingResponse.HttpStatusCode = Net.HttpStatusCode.OK
            fakeTrainingResponse.IsSuccessfulHttpStatus = True
            fakeTrainingResponse.EveErrorCode = 0

            Dim trainingData As String = JsonConvert.SerializeObject(fakeTrainingResponse)

            'strXML = ""
            'strXML &= Reports.CurrentPilotXML_New(nPilot)
            Using writer As New StreamWriter(xmlFile)

                writer.Write(charData)
                writer.Flush()
                writer.Close()

            End Using
            ' Write Training XML
          
            Using writer As New StreamWriter(txmlFile)
                writer.Write(trainingData)
                writer.Flush()
                writer.Close()
            End Using

            ' Import the data!
            Call PilotParseFunctions.ImportPilotFromXML(fakeServiceResponse, fakeTrainingResponse)

            ' Refresh the list of pilots in EveHQ
            PilotParseFunctions.StartPilotRefresh = True

            ' Close the form
            Close()
        End Sub

    End Class
End Namespace