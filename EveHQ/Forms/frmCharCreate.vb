' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports EveHQ.Core.CoreReports
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports System.IO
Imports EveHQ.Common.Extensions
Imports Newtonsoft.Json

Namespace Forms

    Public Class FrmCharCreate
        Dim _sRaceID As Integer = 0
        Dim _sRaceName As String = ""
        Dim _sBloodName As String = ""
        ReadOnly _skillsRace As Collection = New Collection
        ReadOnly _eveRaces As SortedList = New SortedList

        Private Sub frmCharCreate_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            _eveRaces.Add("Caldari", 1)
            _eveRaces.Add("Minmatar", 2)
            _eveRaces.Add("Amarr", 4)
            _eveRaces.Add("Gallente", 8)

        End Sub

        Private Sub cboRace_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboRace.SelectedIndexChanged

            ' If we have chosen something, then activate the next control
            If cboRace.SelectedIndex > -1 Then
                Select Case cboRace.SelectedItem.ToString
                    Case "Amarr"
                        _sBloodName = "Amarr"
                    Case "Caldari"
                        _sBloodName = "Deteis"
                    Case "Gallente"
                        _sBloodName = "Gallente"
                    Case "Minmatar"
                        _sBloodName = "Brutor"
                End Select
            End If


            ' Store the raceID and raceName
            _sRaceName = CStr(cboRace.SelectedItem)
            _sRaceID = CInt(_eveRaces.Item(_sRaceName))

            Call CalcRaceSkills(CStr(_sRaceID))

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
            nPilot.Race = _sRaceName
            nPilot.BloodLine = _sBloodName
            nPilot.Charisma = CInt(nudC.Value)
            nPilot.Intelligence = CInt(nudI.Value)
            nPilot.Memory = CInt(nudM.Value)
            nPilot.Perception = CInt(nudP.Value)
            nPilot.Willpower = CInt(nudW.Value)
            nPilot.Balance = 0
            'nPilot. = True
            nPilot.CorporationName = "EveHQ Import Corp"
            nPilot.CorporationId = 1000000
            nPilot.Gender = "Male"
            nPilot.CloneName = "Clone Grade Alpha"
            nPilot.CloneSkillPoints = 900000

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