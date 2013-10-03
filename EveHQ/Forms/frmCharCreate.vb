' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2012-2013 EveHQ Development Team
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
Imports System.IO
Imports EveHQ.Core.CoreReports
Imports EveHQ.EveAPI

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
                skillName = Core.SkillFunctions.SkillIDToName(CInt(skillID))
                skillPoints = CLng(Math.Ceiling(Core.SkillFunctions.CalculateSkillSPLevel(CInt(skillID), skillLevel)))
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
            Dim nPilot As New Core.EveHQPilot
            nPilot.ID = lblCharID.Text
            ' Check name isn't blank
            If txtCharName.Text.Trim = "" Then
                MessageBox.Show("Please ensure you have entered a pilot name before importing into EveHQ.", "Pilot Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                nPilot.Name = txtCharName.Text
            End If
            ' Check if name exists
            If Core.HQ.Settings.Pilots.ContainsKey(nPilot.Name) = True Then
                MessageBox.Show("Pilot name already exists. Please choose an alternative name.", "Pilot Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            nPilot.Race = _sRaceName
            nPilot.Blood = _sBloodName
            nPilot.CAtt = CInt(nudC.Value)
            nPilot.IAtt = CInt(nudI.Value)
            nPilot.MAtt = CInt(nudM.Value)
            nPilot.PAtt = CInt(nudP.Value)
            nPilot.WAtt = CInt(nudW.Value)
            nPilot.Isk = 0
            nPilot.Active = True
            nPilot.Corp = "EveHQ Import Corp"
            nPilot.CorpID = "1000000"
            nPilot.Gender = "Male"
            nPilot.CloneName = "Clone Grade Alpha"
            nPilot.CloneSP = 900000
            For Each skillItem As ListViewItem In lvwSkills.Items
                Dim pilotSkill As New Core.EveHQPilotSkill
                pilotSkill.ID = CInt(skillItem.Name)
                pilotSkill.Name = skillItem.Text
                pilotSkill.Level = CInt(skillItem.SubItems(1).Text)
                pilotSkill.SP = CInt(skillItem.SubItems(2).Text)
                nPilot.PilotSkills.Add(pilotSkill.Name, pilotSkill)
            Next
            nPilot.Updated = True

            ' Write the XML files
            Dim xmlFile As String = Path.Combine(Core.HQ.CacheFolder, "EveHQAPI_" & APITypes.CharacterSheet.ToString & "_" & nPilot.Account & "_" & nPilot.ID & ".xml")
            Dim txmlFile As String = Path.Combine(Core.HQ.CacheFolder, "EveHQAPI_" & APITypes.SkillQueue.ToString & "_" & nPilot.Account & "_" & nPilot.ID & ".xml")
            Dim strXML As String
            Dim writer As StreamWriter

            ' Write Character XML
            strXML = ""
            strXML &= Reports.CurrentPilotXML_New(nPilot)
            writer = New StreamWriter(xmlFile)
            writer.Write(strXML)
            writer.Flush()
            writer.Close()

            ' Write Training XML
            strXML = ""
            strXML &= Reports.CurrentTrainingXML_New(nPilot)
            writer = New StreamWriter(txmlFile)
            writer.Write(strXML)
            writer.Flush()
            writer.Close()

            ' Import the data!
            Call Core.PilotParseFunctions.ImportPilotFromXML(xmlFile, txmlFile)

            ' Refresh the list of pilots in EveHQ
            Core.PilotParseFunctions.StartPilotRefresh = True

            ' Close the form
            Close()
        End Sub

    End Class
End Namespace