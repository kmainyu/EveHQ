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
Imports System.Xml
Imports System.Windows.Forms
Imports System.IO
Imports System.Data

Public Class frmCharCreate

    Dim createXML As XmlDocument = New Xml.XmlDocument
    Dim sRace As Integer = 0
    Dim sRaceID As Integer = 0
    Dim sRaceName As String = ""
    Dim sBlood As Integer = 0
    Dim sBloodID As Integer = 0
    Dim sBloodName As String = ""
    Dim sAncestry As Integer = 0
    Dim sAncestryID As Integer = 0
    Dim sAncestryName As String = ""
    Dim sC As Integer = 0
    Dim sI As Integer = 0
    Dim sM As Integer = 0
    Dim sP As Integer = 0
    Dim sW As Integer = 0
    Dim skillsRace As Collection = New Collection
    Dim skills As New SortedList
    Dim eveRaces As SortedList = New SortedList
    Dim eveBloodlines As SortedList = New SortedList
    Dim eveAncestries As SortedList = New SortedList
    Dim raceData As New DataSet
    Dim bloodData As New DataSet
    Dim ancestryData As New DataSet

    Private Sub frmCharCreate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Load the data
        Call Me.LoadData()

        ' Load up the creation
        Dim raceName As String = ""
        For raceNo As Integer = 0 To raceData.Tables(0).Rows.Count - 1
            raceName = CStr(raceData.Tables(0).Rows(raceNo).Item("raceName"))
            If raceName <> "Jove" And raceName <> "Pirate" Then
                eveRaces.Add(raceName, raceData.Tables(0).Rows(raceNo).Item("raceID"))
                cboRace.Items.Add(raceName)
            End If
        Next

    End Sub

    Private Sub cboRace_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRace.SelectedIndexChanged

        ' If we have chosen something, then activate the next control
        If cboRace.SelectedIndex > -1 Then
            Select Case cboRace.SelectedItem.ToString
                Case "Amarr"
                    sBloodName = "Amarr" : sAncestryName = "Liberal Holders"
                Case "Caldari"
                    sBloodName = "Deteis" : sAncestryName = "Scientists"
                Case "Gallente"
                    sBloodName = "Gallente" : sAncestryName = "Miners"
                Case "Minmatar"
                    sBloodName = "Brutor" : sAncestryName = "Workers"
            End Select
        End If


        ' Store the raceID and raceName
        sRaceName = CStr(cboRace.SelectedItem)
        sRaceID = CInt(eveRaces.Item(sRaceName))

        Call CalcRaceSkills(CStr(sRaceID))

        ' Set the list of skills & attributes
        lvwSkills.Items.Clear()
        sC = 7 : sI = 8 : sM = 8 : sP = 8 : sW = 8

        ' Display skills
        lvwSkills.Items.Clear()
        Dim skillPoints As Integer = 0
        For Each skill As ListViewItem In skillsRace
            lvwSkills.Items.Add(skill)
            skillPoints += CInt(skill.SubItems(2).Text)
        Next
        lblSP.Text = "Skillpoints: " & FormatNumber(skillPoints, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Enable character options
        lblSelectAttributes.Enabled = True
        lblSelectChar.Enabled = True
        nudC.Enabled = True
        nudI.Enabled = True
        nudM.Enabled = True
        nudP.Enabled = True
        nudW.Enabled = True
        txtCharName.Enabled = True
        Me.btnAddPilot.Enabled = True

        ' Generate an ID based on date and time
        Dim CharID As String = Format(Now, "MddHHmmss")
        lblCharID.Text = CharID

    End Sub

    Private Sub CalcRaceSkills(ByVal raceID As String)
        ' Extract RaceSkills from resources
        Dim RaceSkills As New ArrayList
        Dim RaceSkillsTable As String = My.Resources.RaceSkillsTable
        Dim RaceSkillLines() As String = RaceSkillsTable.Split(ControlChars.CrLf.ToCharArray)
        For Each RaceSkill As String In RaceSkillLines
            Dim RaceSkillData() As String = RaceSkill.Split(",".ToCharArray)
            If RaceSkillData(0) = raceID Then
                RaceSkills.Add(RaceSkillData(1) & "," & RaceSkillData(2))
            End If
        Next

        ' Load up the skills for the selected race
        skillsRace.Clear()
        Dim skillID As String = ""
        Dim skillName As String = ""
        Dim skillLevel As Integer = 0
        Dim skillPoints As Long = 0
        For Each raceskill As String In RaceSkills
            Dim RaceSkillData() As String = raceskill.Split(",".ToCharArray)
            skillID = RaceSkillData(0)
            skillLevel = CInt(RaceSkillData(1))
            skillName = EveHQ.Core.SkillFunctions.SkillIDToName(skillID)
            skillPoints = CLng(Math.Ceiling(EveHQ.Core.SkillFunctions.CalculateSkillSPLevel(skillID, skillLevel)))
            Dim skillItem As New ListViewItem
            skillItem.Text = skillName
            skillItem.Name = skillID
            skillItem.SubItems.Add(skillLevel.ToString)
            skillItem.SubItems.Add(skillPoints.ToString)
            skillsRace.Add(skillItem, skillItem.Text)
        Next
    End Sub

    Private Sub nud_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudC.ValueChanged, nudI.ValueChanged, nudM.ValueChanged, nudP.ValueChanged, nudW.ValueChanged
        Dim attTotal As Integer = CInt(nudC.Value + nudI.Value + nudM.Value + nudP.Value + nudW.Value)
        Me.lblAttTotal.Text = CStr(attTotal)
        If attTotal = 39 Then
            btnAddPilot.Enabled = True
        Else
            btnAddPilot.Enabled = False
        End If
    End Sub

    Private Sub btnAddPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPilot.Click

        ' Create a new pilot
        Dim nPilot As New EveHQ.Core.Pilot
        nPilot.ID = Me.lblCharID.Text
        ' Check name isn't blank
        If Me.txtCharName.Text.Trim = "" Then
            MessageBox.Show("Please ensure you have entered a pilot name before importing into EveHQ.", "Pilot Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            nPilot.Name = Me.txtCharName.Text
        End If
        ' Check if name exists
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(nPilot.Name) = True Then
            MessageBox.Show("Pilot name already exists. Please choose an alternative name.", "Pilot Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        nPilot.Race = sRaceName
        nPilot.Blood = sBloodName
        nPilot.CAtt = CInt(Me.nudC.Value)
        nPilot.IAtt = CInt(Me.nudI.Value)
        nPilot.MAtt = CInt(Me.nudM.Value)
        nPilot.PAtt = CInt(Me.nudP.Value)
        nPilot.WAtt = CInt(Me.nudW.Value)
        nPilot.Isk = 0
        nPilot.Active = True
        nPilot.Corp = "EveHQ Import Corp"
        nPilot.CorpID = "1000000"
        nPilot.Gender = "Male"
        nPilot.CloneName = "Clone Grade Alpha"
        nPilot.CloneSP = "900000"
        For Each skillItem As ListViewItem In lvwSkills.Items
            Dim pilotSkill As New EveHQ.Core.PilotSkill
            pilotSkill.ID = skillItem.Name
            pilotSkill.Name = skillItem.Text
            pilotSkill.Level = CInt(skillItem.SubItems(1).Text)
            pilotSkill.SP = CInt(skillItem.SubItems(2).Text)
            nPilot.PilotSkills.Add(pilotSkill, pilotSkill.Name)
        Next
        nPilot.Updated = True

        ' Write the XML files
        Dim xmlFile As String = Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.CharacterSheet.ToString & "_" & nPilot.Account & "_" & nPilot.ID & ".xml")
        Dim txmlFile As String = Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.SkillQueue.ToString & "_" & nPilot.Account & "_" & nPilot.ID & ".xml")
        Dim strXML As String = ""
        Dim sw As IO.StreamWriter

        ' Write Character XML
        strXML = ""
        strXML &= EveHQ.Core.Reports.CurrentPilotXML_New(nPilot)
        sw = New IO.StreamWriter(xmlFile)
        sw.Write(strXML)
        sw.Flush()
        sw.Close()

        ' Write Training XML
        strXML = ""
        strXML &= EveHQ.Core.Reports.CurrentTrainingXML_New(nPilot)
        sw = New IO.StreamWriter(txmlFile)
        sw.Write(strXML)
        sw.Flush()
        sw.Close()

        ' Import the data!
        Call EveHQ.Core.PilotParseFunctions.ImportPilotFromXML(xmlFile, txmlFile)

        ' Refresh the list of pilots in evehq
        EveHQ.Core.PilotParseFunctions.StartPilotRefresh = True

        ' Close the form
        Me.Close()
    End Sub

#Region "Data Loading Routines"

    Private Function LoadData() As Boolean
        raceData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrRaces")
        If raceData Is Nothing Then
            MessageBox.Show("chrRaces table returned a null dataset.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            If raceData.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("chrRaces table returned no rows.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
        Return True
    End Function

#End Region

End Class
