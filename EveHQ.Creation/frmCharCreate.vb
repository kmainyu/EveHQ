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
Imports System.Xml
Imports System.Windows.Forms

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
    Dim sCareer As Integer = 0
    Dim sCareerID As Integer = 0
    Dim sCareerName As String = ""
    Dim sSpec As Integer = 0
    Dim sSpecID As Integer = 0
    Dim sSpecName As String = ""
    Dim siC As Integer = 0
    Dim siI As Integer = 0
    Dim siM As Integer = 0
    Dim siP As Integer = 0
    Dim siW As Integer = 0
    Dim sC As Integer = 0
    Dim sI As Integer = 0
    Dim sM As Integer = 0
    Dim sP As Integer = 0
    Dim sW As Integer = 0
   
    Dim skillsRace As Collection = New Collection
    Dim skillsBloodline As Collection = New Collection
    Dim skillsAncestry As Collection = New Collection
    Dim skillsSpecT As Collection = New Collection
    Dim skills As New SortedList
    Dim fullSkillList As New SortedList
    Dim skillsToChar As New SortedList
    Dim currentChars As New ArrayList
    Dim eveRaces As SortedList = New SortedList
    Dim eveBloodlines As SortedList = New SortedList
    Dim eveAncestries As SortedList = New SortedList
    Dim eveCareers As SortedList = New SortedList
    Dim eveSpecs As SortedList = New SortedList
    Shared NewCharacters As New SortedList
    Shared CharGoalVariations As New ArrayList
    Dim WithEvents CharGoalWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker

    Private Sub frmCharCreate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.LoadOptions()
        Call Me.LoadSkillTree()
        Call Me.LoadCharGoalGrid()

        ' Load up the creation
        Dim raceName As String = ""
        For raceNo As Integer = 0 To PlugInData.raceData.Tables(0).Rows.Count - 1
            raceName = CStr(PlugInData.raceData.Tables(0).Rows(raceNo).Item("raceName"))
            If raceName <> "Jove" And raceName <> "Pirate" Then
                eveRaces.Add(raceName, PlugInData.raceData.Tables(0).Rows(raceNo).Item("raceID"))
                cboRace.Items.Add(raceName)
            End If
        Next
        cboMethod.SelectedIndex = 0

    End Sub

    Private Sub cboRace_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRace.SelectedIndexChanged

        ' If we have chosen something, then activate the next control
        If cboRace.SelectedIndex > -1 Then
            lblStep2.Enabled = True : cboBloodline.Enabled = True
        End If

        ' Clear any subsequent controls as these will no longer be relevant
        lblStep3.Enabled = False : lblStep4.Enabled = False : lblStep5.Enabled = False
        cboAncestry.Items.Clear() : cboCareer.Items.Clear() : cboSpec.Items.Clear()
        cboAncestry.Enabled = False : cboCareer.Enabled = False : cboSpec.Enabled = False

        ' Store the raceID and raceName
        sRaceName = CStr(cboRace.SelectedItem)
        sRaceID = CInt(eveRaces.Item(sRaceName))

        ' Load up the options for the bloodlines
        cboBloodline.Items.Clear()
        eveBloodlines.Clear()
        Dim bloodset() As DataRow = PlugInData.bloodData.Tables(0).Select("raceID=" & sRaceID)
        Dim bloodName As String = ""
        For bloodNo As Integer = 0 To bloodset.GetUpperBound(0)
            bloodName = CStr(bloodset(bloodNo).Item("bloodlineName"))
            eveBloodlines.Add(bloodName, bloodset(bloodNo).Item("bloodlineID"))
            cboBloodline.Items.Add(bloodName)
        Next

        Call CalcRaceSkills(CStr(sRaceID))

        ' Clear the list of skills & attributes
        lvwSkills.Items.Clear()
        lblCha.Text = "Charisma: n/a"
        lblInt.Text = "Intelligence: n/a"
        lblMem.Text = "Memory: n/a"
        lblPer.Text = "Perception: n/a"
        lblWil.Text = "Willpower: n/a"
        lblSP.Text = "Skillpoints: n/a"

        ' Display skills
        lvwSkills.Items.Clear()
        Dim skillPoints As Integer = 0
        For Each skill As ListViewItem In skillsRace
            lvwSkills.Items.Add(skill)
            skillPoints += CInt(skill.SubItems(2).Text)
        Next
        lblSP.Text = "Skillpoints: " & FormatNumber(skillPoints, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Disable the transfer character button
        Me.btnAddPilot.Enabled = False

    End Sub
    Private Sub cboBloodline_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBloodline.SelectedIndexChanged

        ' If we have chosen something, then activate the next control
        If cboBloodline.SelectedIndex > -1 Then
            lblStep3.Enabled = True : cboAncestry.Enabled = True
        End If

        ' Clear any subsequent controls as these will no longer be relevant
        lblStep4.Enabled = False : lblStep5.Enabled = False
        cboCareer.Items.Clear() : cboSpec.Items.Clear()
        cboCareer.Enabled = False : cboSpec.Enabled = False

        ' Store the raceID and raceName
        sBloodName = CStr(cboBloodline.SelectedItem)
        sBloodID = CInt(eveBloodlines.Item(sBloodName))

        ' Load up the options for the ancestries
        cboAncestry.Items.Clear()
        eveAncestries.Clear()
        Dim ancSet() As DataRow = PlugInData.ancestryData.Tables(0).Select("bloodlineID=" & sBloodID)
        Dim ancName As String = ""
        For ancNo As Integer = 0 To ancSet.GetUpperBound(0)
            ancName = CStr(ancSet(ancNo).Item("ancestryName"))
            Me.eveAncestries.Add(ancName, ancSet(ancNo).Item("ancestryID"))
            Me.cboAncestry.Items.Add(ancName)
        Next

        Call CalcBloodlineAttributes(CStr(sBloodID))

        ' Display attributes
        lblCha.Text = "Charisma: " & sC & " (" & sC - 1 & " - " & sC + 2 & ")"
        lblInt.Text = "Intelligence: " & sI & " (" & sI - 1 & " - " & sI + 2 & ")"
        lblMem.Text = "Memory: " & sM & " (" & sM - 1 & " - " & sM + 2 & ")"
        lblPer.Text = "Perception: " & sP & " (" & sP - 1 & " - " & sP + 2 & ")"
        lblWil.Text = "Willpower: " & sW & " (" & sW - 1 & " - " & sW + 2 & ")"

        ' Display skills
        lvwSkills.Items.Clear()
        Dim skillPoints As Integer = 0
        For Each skill As ListViewItem In skillsBloodline
            lvwSkills.Items.Add(skill)
            skillPoints += CInt(skill.SubItems(2).Text)
        Next
        lblSP.Text = "Skillpoints: " & FormatNumber(skillPoints, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Disable the transfer character button
        Me.btnAddPilot.Enabled = False

    End Sub
    Private Sub cboAncestry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAncestry.SelectedIndexChanged

        ' If we have chosen something, then activate the next control
        'If cboAncestry.SelectedIndex > -1 Then
        '    lblStep4.Enabled = True : cboCareer.Enabled = True
        'End If

        ' Clear any subsequent controls as these will no longer be relevant
        lblStep5.Enabled = False
        cboSpec.Items.Clear()
        cboSpec.Enabled = False

        ' Store the raceID and raceName
        sAncestryName = CStr(cboAncestry.SelectedItem)
        sAncestryID = CInt(eveAncestries.Item(sAncestryName))

        ' Load up the options for the careers
        'cboCareer.Items.Clear()
        'eveCareers.Clear()
        'Dim careerset() As DataRow = PlugInData.careerData.Tables(0).Select("raceID=" & sRaceID)
        'Dim careerName As String = ""
        'For careerNo As Integer = 0 To careerset.GetUpperBound(0)
        '    careerName = CStr(careerset(careerNo).Item("careerName"))
        '    Me.eveCareers.Add(careerName, careerset(careerNo).Item("careerID"))
        '    Me.cboCareer.Items.Add(careerName)
        'Next

        Call CalcAncestryAttributes(CStr(sAncestryID))

        ' Display attributes
        lblCha.Text = "Charisma: " & sC & " (" & sC - 1 & " - " & sC + 2 & ")"
        lblInt.Text = "Intelligence: " & sI & " (" & sI - 1 & " - " & sI + 2 & ")"
        lblMem.Text = "Memory: " & sM & " (" & sM - 1 & " - " & sM + 2 & ")"
        lblPer.Text = "Perception: " & sP & " (" & sP - 1 & " - " & sP + 2 & ")"
        lblWil.Text = "Willpower: " & sW & " (" & sW - 1 & " - " & sW + 2 & ")"

        ' Display skills
        lvwSkills.Items.Clear()
        Dim skillPoints As Integer = 0
        For Each skill As ListViewItem In skillsAncestry
            lvwSkills.Items.Add(skill)
            skillPoints += CInt(skill.SubItems(2).Text)
        Next
        lblSP.Text = "Skillpoints: " & FormatNumber(skillPoints, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        ' Disable the transfer character button
        Me.btnAddPilot.Enabled = True

    End Sub

    Private Sub CalcRaceSkills(ByVal raceID As String)

        ' Load up the skills for the selected race
        skillsRace.Clear()
        Dim skillRows() As DataRow
        skillRows = PlugInData.raceSkillData.Tables(0).Select("raceID = " & raceID)
        Dim skillID As String = ""
        Dim skillName As String = ""
        Dim skillLevel As Integer = 0
        Dim skillPoints As Long = 0
        For skillNo As Integer = 0 To skillRows.GetUpperBound(0)
            skillID = CStr(skillRows(skillNo).Item("skillTypeID"))
            skillName = EveHQ.Core.SkillFunctions.SkillIDToName(skillID)
            skillLevel = CInt(skillRows(skillNo).Item("levels"))
            skillPoints = CLng(Math.Round(EveHQ.Core.SkillFunctions.CalculateSkillSPLevel(skillID, skillLevel), 0))
            Dim skillItem As New ListViewItem
            skillItem.Text = skillName
            skillItem.Name = skillID
            skillItem.SubItems.Add(skillLevel.ToString)
            skillItem.SubItems.Add(skillPoints.ToString)
            skillsRace.Add(skillItem, skillItem.Text)
        Next
    End Sub
    Private Sub CalcBloodlineAttributes(ByVal bloodID As String)

        ' Load up the attributes for the selected bloodline
        Dim attRows() As DataRow
        attRows = PlugInData.bloodData.Tables(0).Select("bloodlineID = " & bloodID)
        ' Get attributes of the bloodline & display them
        siC = CInt(attRows(0).Item("charisma"))
        siI = CInt(attRows(0).Item("intelligence"))
        siM = CInt(attRows(0).Item("memory"))
        siP = CInt(attRows(0).Item("perception"))
        siW = CInt(attRows(0).Item("willpower"))

        siC += 1 : siI += 1 : siM += 1 : siP += 1 : siW += 1
        sC = siC : sI = siI : sM = siM : sP = siP : sW = siW

        ' Get list of skills acquired
        skillsBloodline.Clear()
        For Each skill As ListViewItem In skillsRace
            skillsBloodline.Add(skill, skill.Text)
        Next
    End Sub
    Private Sub CalcAncestryAttributes(ByVal ancestryID As String)

        ' Load up the attributes for the selected bloodline
        Dim attRows() As DataRow
        attRows = PlugInData.ancestryData.Tables(0).Select("ancestryID = " & ancestryID)
        ' Get attributes of the bloodline & display them
        sC = siC + CInt(attRows(0).Item("charisma"))
        sI = siI + CInt(attRows(0).Item("intelligence"))
        sM = siM + CInt(attRows(0).Item("memory"))
        sP = siP + CInt(attRows(0).Item("perception"))
        sW = siW + CInt(attRows(0).Item("willpower"))

        ' Get list of skills acquired
        skillsAncestry.Clear()
        For Each skill As ListViewItem In skillsBloodline
            skillsAncestry.Add(skill, skill.Text)
        Next
    End Sub

    Private Sub LoadOptions()
        fullSkillList.Clear()
        NewCharacters.Clear()
        Dim count As Integer = 0
        For race As Integer = 0 To PlugInData.raceData.Tables(0).Rows.Count - 1
            Dim raceName As String = CStr(PlugInData.raceData.Tables(0).Rows(race).Item("raceName"))
            Dim raceID As String = CStr(PlugInData.raceData.Tables(0).Rows(race).Item("raceID"))
            If raceName <> "Jove" And raceName <> "Pirate" Then
                ' Get bloodlines
                Dim bloodRows() As DataRow
                bloodRows = PlugInData.bloodData.Tables(0).Select("raceID = " & raceID)
                For blood As Integer = 0 To bloodRows.GetUpperBound(0)
                    Dim bloodName As String = CStr(bloodRows(blood).Item("bloodlineName"))
                    Dim bloodID As String = CStr(bloodRows(blood).Item("bloodlineID"))
                    ' Get ancestry
                    Dim ancestryRows() As DataRow
                    ancestryRows = PlugInData.ancestryData.Tables(0).Select("bloodlineID = " & bloodID)
                    For ancestry As Integer = 0 To ancestryRows.GetUpperBound(0)
                        Dim ancestryName As String = CStr(ancestryRows(ancestry).Item("ancestryName"))
                        Dim ancestryID As String = CStr(ancestryRows(ancestry).Item("ancestryID"))
                        ' Get careers
                        'Dim careerRows() As DataRow
                        'careerRows = PlugInData.careerData.Tables(0).Select("raceID = " & raceID)
                        'For career As Integer = 0 To careerRows.GetUpperBound(0)
                        '    Dim careerName As String = CStr(careerRows(career).Item("careerName"))
                        '    Dim careerID As String = CStr(careerRows(career).Item("careerID"))
                        '    ' Get specs
                        '    Dim specRows() As DataRow
                        '    specRows = PlugInData.specData.Tables(0).Select("careerID = " & careerID)
                        '    For spec As Integer = 0 To specRows.GetUpperBound(0)
                        '        Dim specName As String = CStr(specRows(spec).Item("specialityName"))
                        '        Dim specID As String = CStr(specRows(spec).Item("specialityID"))
                        ' Create the listviewitem
                        Call CalcRaceSkills(raceID)
                        Call CalcBloodlineAttributes(bloodID)
                        Call CalcAncestryAttributes(ancestryID)
                        count += 1
                        ' Add this to the retained list of characters for later use if required
                        Dim newChar As New NewCharacter
                        newChar.CharID = count
                        newChar.RaceName = raceName
                        newChar.BloodLine = bloodName
                        newChar.Ancestry = ancestryName
                        'newChar.Career = careerName
                        'newChar.Speciality = specName
                        newChar.Charisma = sC
                        newChar.Intelligence = sI
                        newChar.Memory = sM
                        newChar.Perception = sP
                        newChar.Willpower = sW

                        ' Add this to the listview
                        Dim character As New ListViewItem
                        character.Text = count.ToString
                        character.Name = "C" & count
                        character.SubItems.Add(raceName)
                        character.SubItems.Add(bloodName)
                        character.SubItems.Add(ancestryName)
                        character.SubItems.Add(sC.ToString)
                        character.SubItems.Add(sI.ToString)
                        character.SubItems.Add(sM.ToString)
                        character.SubItems.Add(sP.ToString)
                        character.SubItems.Add(sW.ToString)

                        Dim skillList As String = ""
                        Dim skillPoints As Long = 0
                        skills.Clear()
                        newChar.Skills.Clear()
                        skillList = "Acquired Skills:"
                        For Each skill As ListViewItem In skillsRace
                            skills.Add(skill.Text, skill.Text & " " & EveHQ.Core.SkillFunctions.Roman(CInt(skill.SubItems(1).Text)) & ";")
                            skillPoints += CLng(skill.SubItems(2).Text)
                        Next
                        For Each skill As String In skills.Values
                            If fullSkillList.Contains(skill) = False Then
                                fullSkillList.Add(skill, skill)
                                lstStartSkills.Items.Add(skill)
                                Dim skills As New ArrayList
                                skills.Add(count)
                                skillsToChar.Add(skill, skills)
                            Else
                                ' Get the list of chars with this skill
                                Dim skills As ArrayList = CType(skillsToChar.Item(skill), ArrayList)
                                skills.Add(count)
                            End If
                            skillList &= ControlChars.CrLf & skill
                            Dim charSkill As String = skill
                            charSkill = charSkill.Replace(" I;", "1")
                            charSkill = charSkill.Replace(" II;", "2")
                            charSkill = charSkill.Replace(" III;", "3")
                            charSkill = charSkill.Replace(" IV;", "4")
                            charSkill = charSkill.Replace(" V;", "5")
                            newChar.Skills.Add(charSkill)
                        Next
                        character.SubItems.Add(skillPoints.ToString)
                        character.ToolTipText = skillList
                        Me.lvwChars.Items.Add(character)
                        NewCharacters.Add(newChar.CharID, newChar)
                    Next
                    'Next
                    '    Next
                Next
            End If
        Next

    End Sub
    Private Sub LoadCharGoalGrid()
        CharGoalVariations.Clear()
        Dim StartChar(4) As Integer
        For att As Integer = 0 To 4
            StartChar(att) = 1
        Next
        CharGoalVariations.Add(StartChar)
        For place3 As Integer = 0 To 4
            Dim VarArray(4) As Integer
            VarArray(place3) = 3
            For place2 As Integer = 0 To 4
                Dim NewVariations() As Integer = CType(VarArray.Clone, Integer())
                If NewVariations(place2) <> 3 Then
                    NewVariations(place2) = 2
                    CharGoalVariations.Add(NewVariations)
                End If
            Next
        Next
    End Sub

    Private Sub lvwChars_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwChars.ColumnClick
        If CInt(lvwChars.Tag) = e.Column Then
            Me.lvwChars.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwChars.Tag = -1
        Else
            Me.lvwChars.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwChars.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwChars.Sort()
    End Sub
    Private Sub lvwChars2_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwChars2.ColumnClick
        If CInt(lvwChars2.Tag) = e.Column Then
            Me.lvwChars2.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwChars2.Tag = -1
        Else
            Me.lvwChars2.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwChars2.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwChars2.Sort()
    End Sub
    Private Sub lstStartSkills_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstStartSkills.SelectedIndexChanged
        If lstStartSkills.SelectedItems.Count > 10 Then
            lstStartSkills.SetSelected(lstStartSkills.SelectedIndices(10), False)
            Exit Sub
        End If
        lvwChars2.Items.Clear()
        If lstStartSkills.SelectedItems.Count > 0 Then
            If lstStartSkills.SelectedItems.Count = 1 Then
                ' If only 1 selected
                Dim skills As ArrayList = CType(skillsToChar.Item(lstStartSkills.SelectedItem), ArrayList)
                For Each skill As Integer In skills
                    Dim chars As ListViewItem = CType(lvwChars.Items("C" & skill).Clone, ListViewItem)
                    lvwChars2.Items.Add(chars)
                Next
            Else
                ' If more than 1 selected
                currentChars.Clear()
                Dim firsttime As Boolean = True
                For Each skilllist As String In lstStartSkills.SelectedItems
                    If firsttime = True Then
                        ' If the first run
                        Dim skills As ArrayList = CType(skillsToChar.Item(skilllist), ArrayList)
                        For Each skill As Integer In skills
                            currentChars.Add(skill)
                        Next
                        firsttime = False
                    Else
                        ' If not the first run
                        Dim newChars As New ArrayList
                        Dim skills As ArrayList = CType(skillsToChar.Item(skilllist), ArrayList)
                        For Each character As Integer In currentChars
                            If skills.Contains(character) = True Then
                                newChars.Add(character)
                            End If
                        Next
                        currentChars = newChars
                    End If
                Next
                For Each charNo As Integer In currentChars
                    Dim chars As ListViewItem = CType(lvwChars.Items("C" & charNo).Clone, ListViewItem)
                    lvwChars2.Items.Add(chars)
                Next
            End If
        End If
        ' Update the information label:
        Me.lblSelectedSkills.Text = "Selected Skills:" & ControlChars.CrLf & ControlChars.CrLf
        For Each skilllist As String In lstStartSkills.SelectedItems
            Me.lblSelectedSkills.Text &= skilllist & ControlChars.CrLf
        Next
        Me.lblSelectedSkills.Text &= ControlChars.CrLf & "Matching Characters: " & lvwChars2.Items.Count
    End Sub

    Private Sub btnAddPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPilot.Click
        Dim confirmAtts As New frmConfirmAtts
        With confirmAtts
            .nudC.Value = sC
            .nudI.Value = sI
            .nudM.Value = sM
            .nudP.Value = sP
            .nudW.Value = sW
            .lblC.Text = "Charisma (" & sC - 1 & " to " & sC + 2 & ")"
            .lblI.Text = "Intelligence (" & sI - 1 & " to " & sI + 2 & ")"
            .lblM.Text = "Memory (" & sM - 1 & " to " & sM + 2 & ")"
            .lblP.Text = "Perception (" & sP - 1 & " to " & sP + 2 & ")"
            .lblW.Text = "Willpower (" & sW - 1 & " to " & sW + 2 & ")"
            .nudC.Minimum = sC - 1 : .nudC.Maximum = sC + 2 : .nudC.Increment = 1
            .nudI.Minimum = sI - 1 : .nudI.Maximum = sI + 2 : .nudI.Increment = 1
            .nudM.Minimum = sM - 1 : .nudM.Maximum = sM + 2 : .nudM.Increment = 1
            .nudP.Minimum = sP - 1 : .nudP.Maximum = sP + 2 : .nudP.Increment = 1
            .nudW.Minimum = sW - 1 : .nudW.Maximum = sW + 2 : .nudW.Increment = 1
            .race = sRaceName
            .bloodline = sBloodName
            .gender = "Male"
            .ancestry = sAncestryName
            .career = sCareerName
            .spec = sSpecName
            .skills = skillsRace
            .ShowDialog()
        End With
    End Sub

    Private Sub mnuAddCharacter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCharacter.Click
        Dim lvwchar As ListView = CType(ctxChars.SourceControl, ListView)

        Dim confirmAtts As New frmConfirmAtts
        Dim C, I, M, P, W As Integer
        Dim sC As NewCharacter = CType(NewCharacters(CInt(lvwchar.SelectedItems(0).Text)), NewCharacter)
        With confirmAtts
            C = CInt(lvwchar.SelectedItems(0).SubItems(4).Text)
            I = CInt(lvwchar.SelectedItems(0).SubItems(5).Text)
            M = CInt(lvwchar.SelectedItems(0).SubItems(6).Text)
            P = CInt(lvwchar.SelectedItems(0).SubItems(7).Text)
            W = CInt(lvwchar.SelectedItems(0).SubItems(8).Text)
            .nudC.Value = C
            .nudI.Value = I
            .nudM.Value = M
            .nudP.Value = P
            .nudW.Value = W
            .lblC.Text = "Charisma (" & sC.Charisma - 1 & " to " & sC.Charisma + 2 & ")"
            .lblI.Text = "Intelligence (" & sC.Intelligence - 1 & " to " & sC.Intelligence + 2 & ")"
            .lblM.Text = "Memory (" & sC.Memory - 1 & " to " & sC.Memory + 2 & ")"
            .lblP.Text = "Perception (" & sC.Perception - 1 & " to " & sC.Perception + 2 & ")"
            .lblW.Text = "Willpower (" & sC.Willpower - 1 & " to " & sC.Willpower + 2 & ")"
            .nudC.Minimum = sC.Charisma - 1 : .nudC.Maximum = sC.Charisma + 2 : .nudC.Increment = 1
            .nudI.Minimum = sC.Intelligence - 1 : .nudI.Maximum = sC.Intelligence + 2 : .nudI.Increment = 1
            .nudM.Minimum = sC.Memory - 1 : .nudM.Maximum = sC.Memory + 2 : .nudM.Increment = 1
            .nudP.Minimum = sC.Perception - 1 : .nudP.Maximum = sC.Perception + 2 : .nudP.Increment = 1
            .nudW.Minimum = sC.Willpower - 1 : .nudW.Maximum = sC.Willpower + 2 : .nudW.Increment = 1
            .race = sC.RaceName
            .bloodline = sC.BloodLine
            .gender = "Male"
            .ancestry = sC.Ancestry
            Dim skillList As String = lvwchar.SelectedItems(0).ToolTipText
            Dim skills() As String = skillList.Split(ControlChars.CrLf.ToCharArray)
            skills(0) = ""
            skillsSpecT.Clear()
            For Each skill As String In sC.Skills
                If skill <> "" Then
                    Dim skillLevel As Integer = CInt(skill.Substring(skill.Length - 1))
                    Dim skillName As String = skill.Substring(0, skill.Length - 1)
                    Dim skillID As String = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
                    Dim skillPoints As Long = CLng(EveHQ.Core.SkillFunctions.CalculateSkillSPLevel(skillID, skillLevel))
                    Dim skillItem As New ListViewItem
                    skillItem.Name = skillID
                    skillItem.Text = skillName
                    skillItem.SubItems.Add(skillLevel.ToString)
                    skillItem.SubItems.Add(skillPoints.ToString)
                    skillsSpecT.Add(skillItem)
                End If
            Next
            .skills = skillsSpecT
            .ShowDialog()
        End With
    End Sub

    Private Sub ctxChars_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxChars.Opening
        Dim lvwchar As ListView = CType(ctxChars.SourceControl, ListView)
        If lvwchar.SelectedItems.Count = 0 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub btnSeek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeek.Click
        If lvwSkillSelection.Items.Count > 0 Then
            CharGoalWorker.WorkerReportsProgress = True
            CharGoalWorker.WorkerSupportsCancellation = True
            btnSeek.Enabled = False
            btnCancelSeek.Enabled = True
            cboMethod.Enabled = False
            CharGoalWorker.RunWorkerAsync()
        Else
            MessageBox.Show("You must select some target skills before starting the Goal Seek!", "Skills Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub lstChars_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lstChars.ColumnClick
        If CInt(lstChars.Tag) = e.Column Then
            Me.lstChars.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Ascending)
            lstChars.Tag = -1
        Else
            Me.lstChars.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Descending)
            lstChars.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lstChars.Sort()
    End Sub

#Region "Goals TreeView Subs"
    Public Sub LoadSkillTree()
        Call Me.LoadSkillGroups()
        Call Me.LoadFilteredSkills()
        Call Me.RemoveEmptyGroups()
    End Sub

    Private Sub LoadSkillGroups()
        tvwSkillList.Nodes.Clear()
        Dim newSkillGroup As EveHQ.Core.SkillGroup
        For Each newSkillGroup In EveHQ.Core.HQ.SkillGroups.Values
            If newSkillGroup.ID <> "505" Then
                Dim groupNode As TreeNode = New TreeNode
                groupNode.Name = newSkillGroup.ID
                groupNode.Text = newSkillGroup.Name.Trim
                groupNode.ImageIndex = 8
                groupNode.SelectedImageIndex = 8
                tvwSkillList.Nodes.Add(groupNode)
            End If
        Next
    End Sub

    Private Sub LoadFilteredSkills()
        Dim newSkill As EveHQ.Core.EveSkill
        For Each newSkill In EveHQ.Core.HQ.SkillListID.Values
            Dim gID As String = newSkill.GroupID
            If gID <> "505" Then
                Dim addSkill As Boolean = False
                If newSkill.Published = True Then
                    Dim skillNode As TreeNode = New TreeNode
                    skillNode.Text = newSkill.Name
                    skillNode.Name = newSkill.ID

                    tvwSkillList.Nodes(gID).Nodes.Add(skillNode)
                End If
            End If
        Next
    End Sub

    Private Sub RemoveEmptyGroups()
        For Each newSkillGroup As EveHQ.Core.SkillGroup In EveHQ.Core.HQ.SkillGroups.Values
            If newSkillGroup.ID <> "505" Then
                Dim groupNode As TreeNode = tvwSkillList.Nodes(newSkillGroup.ID)
                If groupNode.Nodes.Count = 0 Then
                    tvwSkillList.Nodes.RemoveByKey(newSkillGroup.ID)
                End If
            End If
        Next
    End Sub
#End Region

    Private Sub tvwSkillList_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSkillList.NodeMouseClick
        tvwSkillList.SelectedNode = e.Node
    End Sub

    Private Sub tvwSkillList_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSkillList.NodeMouseDoubleClick
        Dim selSkill As String = e.Node.Text
        ' Check if this skill is already in the list
        If lvwSkillSelection.Items.ContainsKey(selSkill) = False Then
            Dim newGoal As New ListViewItem
            newGoal.Name = selSkill
            newGoal.Text = selSkill
            newGoal.SubItems.Add("1")
            lvwSkillSelection.Items.Add(newGoal)
        End If
    End Sub

    Private Sub btnClearGoals_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearGoals.Click
        Dim msg As String = "Are you sure you want to delete all the skill goals?"
        Dim reply As Integer = MessageBox.Show(msg, "Clear All Goals?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = System.Windows.Forms.DialogResult.Yes Then
            lvwSkillSelection.Items.Clear()
        End If
    End Sub

    Private Sub btnRemoveGoal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveGoal.Click
        If lvwSkillSelection.SelectedItems.Count > 0 Then
            lvwSkillSelection.BeginUpdate()
            For Each delItem As ListViewItem In lvwSkillSelection.SelectedItems
                lvwSkillSelection.Items.Remove(delItem)
            Next
            lvwSkillSelection.EndUpdate()
        End If
    End Sub

    Private Sub btnIncreaseLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIncreaseLevel.Click
        If lvwSkillSelection.SelectedItems.Count > 0 Then
            For change As Integer = 0 To lvwSkillSelection.SelectedItems.Count - 1
                Dim selSkill As String = lvwSkillSelection.SelectedItems(change).Text
                Dim lvl As Integer = CInt(lvwSkillSelection.Items(selSkill).SubItems(1).Text)
                If lvl < 5 Then
                    lvwSkillSelection.Items(selSkill).SubItems(1).Text = CStr(lvl + 1)
                End If
            Next
        End If
        lvwSkillSelection.Focus()
    End Sub

    Private Sub btnDecreaseLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDecreaseLevel.Click
        If lvwSkillSelection.SelectedItems.Count > 0 Then
            For change As Integer = 0 To lvwSkillSelection.SelectedItems.Count - 1
                Dim selSkill As String = lvwSkillSelection.SelectedItems(change).Text
                Dim lvl As Integer = CInt(lvwSkillSelection.Items(selSkill).SubItems(1).Text)
                If lvl > 1 Then
                    lvwSkillSelection.Items(selSkill).SubItems(1).Text = CStr(lvl - 1)
                End If
            Next
        End If
        lvwSkillSelection.Focus()
    End Sub

    Private Sub CharGoalWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles CharGoalWorker.DoWork
        Call SearchChars(CharGoalWorker, e)
    End Sub

    Private Sub SearchChars(ByVal worker As System.ComponentModel.BackgroundWorker, ByVal e As System.ComponentModel.DoWorkEventArgs)
        lstChars.BeginUpdate()
        lstChars.Items.Clear()
        Dim varyAtts As Integer = 0
        If cboMethod.SelectedIndex > 0 Then
            Call LoadCharGoalGrid()
            If cboMethod.SelectedIndex = 1 Then
                varyAtts = CharGoalVariations.Count - 1
            End If
        End If
        Dim CharAtts(4) As Integer
        Dim attC, attI, attM, attP, attW As Integer

        ' Go through each character and find the results!
        For pilot As Integer = 1 To NewCharacters.Count
            ' Check for a cancellation request
            If worker.CancellationPending = True Then
                lstChars.Items.Clear()
                lstChars.EndUpdate()
                pbPilots.Value = 0
                Exit Sub
            End If
            ' Do the main loop
            Dim nPilot As New EveHQ.Core.Pilot
            Dim newQueue As New EveHQ.Core.SkillQueue
            newQueue.Name = "EveHQCreationTestQueue"
            newQueue.IncCurrentTraining = True
            newQueue.Primary = False

            Dim startChar As NewCharacter = CType(NewCharacters(pilot), NewCharacter)

            nPilot.TrainingQueues.Clear()
            nPilot.PilotSkills.Clear()
            nPilot.Updated = True
            For Each pilotSkill As String In startChar.Skills
                Dim newSkill As New EveHQ.Core.PilotSkill
                Dim skillName As String = pilotSkill.Substring(0, pilotSkill.Length - 1)
                Dim skillLevel As String = pilotSkill.Substring(pilotSkill.Length - 1, 1)
                newSkill.Name = skillName
                newSkill.ID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
                newSkill.Level = CInt(skillLevel)
                Dim refSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(newSkill.ID)
                newSkill.Rank = refSkill.Rank
                newSkill.SP = CInt(EveHQ.Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, newSkill.Level))
                nPilot.PilotSkills.Add(newSkill, newSkill.Name)
            Next

            ' Add the skills we have to the training queue (in any order, no learning skills will be applied)
            For skill As Integer = 0 To lvwSkillSelection.Items.Count - 1
                Dim skillName As String = lvwSkillSelection.Items(skill).Text
                Dim skillLevel As Integer = CInt(lvwSkillSelection.Items(skill).SubItems(1).Text)
                Dim qItem As New EveHQ.Core.SkillQueueItem
                qItem.Name = skillName
                qItem.FromLevel = 0
                qItem.ToLevel = skillLevel
                qItem.Pos = skill + 1
                qItem.Key = qItem.Name & qItem.FromLevel & qItem.ToLevel
                newQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(nPilot, skillName, skill + 1, newQueue, skillLevel, True, True)
            Next

            ' Check the skill list and see which is actually the best attributes to add point to
            If cboMethod.SelectedIndex = 0 Then
                ' Reset the char variations
                CharGoalVariations.Clear()

                ' Build a queue
                Dim sQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(nPilot, newQueue)
                Dim pointScores(4, 1) As Long
                For a As Integer = 0 To 4
                    pointScores(a, 0) = a
                Next
                For Each skill As EveHQ.Core.SortedQueue In sQueue
                    Select Case skill.PAtt
                        Case "Charisma"
                            pointScores(0, 1) += CLng(skill.SPTrained) * 2
                        Case "Intelligence"
                            pointScores(1, 1) += CLng(skill.SPTrained) * 2
                        Case "Memory"
                            pointScores(2, 1) += CLng(skill.SPTrained) * 2
                        Case "Perception"
                            pointScores(3, 1) += CLng(skill.SPTrained) * 2
                        Case "Willpower"
                            pointScores(4, 1) += CLng(skill.SPTrained) * 2
                    End Select
                    Select Case skill.SAtt
                        Case "Charisma"
                            pointScores(0, 1) += CLng(skill.SPTrained)
                        Case "Intelligence"
                            pointScores(1, 1) += CLng(skill.SPTrained)
                        Case "Memory"
                            pointScores(2, 1) += CLng(skill.SPTrained)
                        Case "Perception"
                            pointScores(3, 1) += CLng(skill.SPTrained)
                        Case "Willpower"
                            pointScores(4, 1) += CLng(skill.SPTrained)
                    End Select
                Next
                ' Sort the list
                ' Create a tag array ready to sort the skill times
                Dim tagArray(4) As Integer
                For a As Integer = 0 To 4
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New EveHQ.Core.Reports.RectangularComparer(pointScores)
                Array.Sort(tagArray, myComparer)
                Array.Reverse(tagArray)
                ' Increase the attributes
                varyAtts = 0
                Dim BestChar(4) As Integer
                BestChar(tagArray(0)) = 3
                BestChar(tagArray(1)) = 2
                CharGoalVariations.Add(BestChar)
            End If

            For charatt As Integer = 0 To varyAtts
                CharAtts = CType(CharGoalVariations(charatt), Integer())
                attC = CharAtts(0)
                attI = CharAtts(1)
                attM = CharAtts(2)
                attP = CharAtts(3)
                attW = CharAtts(4)
                nPilot.CAtt = startChar.Charisma - 1 + attC
                nPilot.IAtt = startChar.Intelligence - 1 + attI
                nPilot.MAtt = startChar.Memory - 1 + attM
                nPilot.PAtt = startChar.Perception - 1 + attP
                nPilot.WAtt = startChar.Willpower - 1 + attW

                ' Build the attribute data
                EveHQ.Core.PilotParseFunctions.BuildAttributeData(nPilot)
                EveHQ.Core.PilotParseFunctions.LoadKeySkillsForPilot(nPilot)

                ' Build the Queue
                Dim aQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(nPilot, newQueue)

                ' Now Let's optimize this queue, because it won't be optimal!
                Dim optimalQueue As EveHQ.Core.SkillQueue = EveHQ.Core.SkillQueueFunctions.FindSuggestions(nPilot, newQueue)

                ' Add pilot to the list
                Dim ttt As String = ""
                For Each skill As EveHQ.Core.SortedQueue In aQueue
                    ttt &= skill.Name & " (" & skill.FromLevel & " to " & skill.ToLevel & ")" & ControlChars.CrLf
                Next
                Dim nLVI As New ListViewItem
                nLVI.Text = pilot.ToString
                nLVI.Name = pilot.ToString
                nLVI.ToolTipText = ttt
                Dim nLVIr As New ListViewItem.ListViewSubItem
                nLVIr.Text = startChar.RaceName
                nLVIr.Name = startChar.RaceName
                nLVI.SubItems.Add(nLVIr)
                Dim nLVIb As New ListViewItem.ListViewSubItem
                nLVIb.Text = startChar.BloodLine
                nLVIb.Name = startChar.BloodLine
                nLVI.SubItems.Add(nLVIb)
                Dim nLVIa As New ListViewItem.ListViewSubItem
                nLVIa.Text = startChar.Ancestry
                nLVIa.Name = startChar.Ancestry
                nLVI.SubItems.Add(nLVIa)
                Dim nLVIac As New ListViewItem.ListViewSubItem
                nLVIac.Text = CStr(nPilot.CAtt)
                nLVIac.Name = CStr(nPilot.CAtt)
                nLVI.SubItems.Add(nLVIac)
                Dim nLVIai As New ListViewItem.ListViewSubItem
                nLVIai.Text = CStr(nPilot.IAtt)
                nLVIai.Name = CStr(nPilot.IAtt)
                nLVI.SubItems.Add(nLVIai)
                Dim nLVIam As New ListViewItem.ListViewSubItem
                nLVIam.Text = CStr(nPilot.MAtt)
                nLVIam.Name = CStr(nPilot.MAtt)
                nLVI.SubItems.Add(nLVIam)
                Dim nLVIap As New ListViewItem.ListViewSubItem
                nLVIap.Text = CStr(nPilot.PAtt)
                nLVIap.Name = CStr(nPilot.PAtt)
                nLVI.SubItems.Add(nLVIap)
                Dim nLVIaw As New ListViewItem.ListViewSubItem
                nLVIaw.Text = CStr(nPilot.WAtt)
                nLVIaw.Name = CStr(nPilot.WAtt)
                nLVI.SubItems.Add(nLVIaw)
                Dim nLVIt As New ListViewItem.ListViewSubItem
                nLVIt.Text = EveHQ.Core.SkillFunctions.TimeToString(newQueue.QueueTime)
                nLVIt.Name = CStr(newQueue.QueueTime)
                nLVI.SubItems.Add(nLVIt)
                Dim nLVIo As New ListViewItem.ListViewSubItem
                nLVIo.Text = EveHQ.Core.SkillFunctions.TimeToString(optimalQueue.QueueTime)
                nLVIo.Name = CStr(optimalQueue.QueueTime)
                nLVI.SubItems.Add(nLVIo)
                lstChars.Items.Add(nLVI)
            Next
            worker.ReportProgress(CInt(pilot / NewCharacters.Count * 100))
        Next
        lstChars.EndUpdate()
    End Sub

    Private Sub CharGoalWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles CharGoalWorker.ProgressChanged

        pbPilots.Value = e.ProgressPercentage
    End Sub

    Private Sub CharGoalWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles CharGoalWorker.RunWorkerCompleted
        btnSeek.Enabled = True
        btnCancelSeek.Enabled = False
        cboMethod.Enabled = True
    End Sub

    Private Sub btnCancelSeek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelSeek.Click
        CharGoalWorker.CancelAsync()
    End Sub

    Private Sub mnuAddLevelx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddLevel1.Click, mnuAddLevel2.Click, mnuAddLevel3.Click, mnuAddLevel4.Click, mnuAddLevel5.Click
        Dim mnuAddLevel As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim level As Integer = CInt(mnuAddLevel.Name.Substring(mnuAddLevel.Name.Length - 1, 1))
        Dim selSkill As String = mnuAddSkillName.Text
        ' Check if this skill is already in the list
        If lvwSkillSelection.Items.ContainsKey(selSkill) = False Then
            Dim newGoal As New ListViewItem
            newGoal.Name = selSkill
            newGoal.Text = selSkill
            newGoal.SubItems.Add(level.ToString)
            lvwSkillSelection.Items.Add(newGoal)
        End If
    End Sub

    Private Sub mnuEditLevelx_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditLevel1.Click, mnuEditLevel2.Click, mnuEditLevel3.Click, mnuEditLevel4.Click, mnuEditLevel5.Click
        Dim mnuEditLevel As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim level As Integer = CInt(mnuEditLevel.Name.Substring(mnuEditLevel.Name.Length - 1, 1))
        Dim selSkill As String = mnuEditSkillName.Text
        For Each editItem As ListViewItem In lvwSkillSelection.SelectedItems
            editItem.SubItems(1).Text = level.ToString
        Next
    End Sub

    Private Sub ctxAddSkills_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxAddSkills.Opening
        If tvwSkillList.SelectedNode.Nodes.Count = 0 Then
            mnuAddSkillName.Text = tvwSkillList.SelectedNode.Text
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub ctxEditSkills_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxEditSkills.Opening
        If lvwSkillSelection.SelectedItems.Count > 0 Then
            If lvwSkillSelection.SelectedItems.Count = 1 Then
                mnuEditSkillName.Text = lvwSkillSelection.SelectedItems(0).Text
            Else
                mnuEditSkillName.Text = "(Multiple Skills)"
            End If
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub mnuDeleteSkill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteSkill.Click
        If lvwSkillSelection.SelectedItems.Count > 0 Then
            lvwSkillSelection.BeginUpdate()
            For Each delItem As ListViewItem In lvwSkillSelection.SelectedItems
                lvwSkillSelection.Items.Remove(delItem)
            Next
            lvwSkillSelection.EndUpdate()
        End If
    End Sub

    Private Sub mnuExportToCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExportToCSV.Click
        Dim lvwchar As ListView = CType(ctxChars.SourceControl, ListView)

        Dim fileName As String = ""
        With sfd1
            .Title = "Please choose a filename to save the CSV file..."
            .FileName = ""
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            .Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                fileName = .FileName
                If fileName.EndsWith(".csv") = False Then
                    fileName &= ".csv"
                End If
            Else
                Exit Sub
            End If
        End With

        Try
            Dim sw As New IO.StreamWriter(fileName)
            If lvwchar.Name = "lstChars" Then
                sw.WriteLine("EveHQ Character Creation Goal Seek Export")
                sw.WriteLine("")
                sw.WriteLine("Goal Skills:")
                For Each lvwSkill As ListViewItem In lvwSkillSelection.Items
                    sw.WriteLine(lvwSkill.Text & " " & lvwSkill.SubItems(1).Text)
                Next
                sw.WriteLine("")
            Else
                sw.WriteLine("EveHQ Character Creation Export")
                sw.WriteLine("")
            End If

            Dim line As String = ""
            For Each col As ColumnHeader In lvwchar.Columns
                line &= col.Text & ","
            Next
            line = line.TrimEnd(",".ToCharArray)
            sw.WriteLine(line)
            For Each lvwPilot As ListViewItem In lvwchar.Items
                line = ""
                For Each subI As ListViewItem.ListViewSubItem In lvwPilot.SubItems
                    line &= subI.Text & ","
                Next
                line = line.TrimEnd(",".ToCharArray)
                sw.WriteLine(line)
            Next
            sw.Flush()
            sw.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Exporting CSV File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub
End Class

Public Class NewCharacter

    Public CharID As Integer
    Public RaceName As String
    Public BloodLine As String
    Public Ancestry As String
    Public Charisma As Integer
    Public Intelligence As Integer
    Public Memory As Integer
    Public Perception As Integer
    Public Willpower As Integer
    Public Skills As New ArrayList

End Class