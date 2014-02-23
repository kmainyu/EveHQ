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

Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.Core

Namespace Forms

    Public Class FrmPilotManager

        Public PilotName As String = ""
        Dim _currentPilotName As String = ""
        Dim _currentPilot As FittingPilot
        Dim _currentGroup As ImplantCollection
        Dim _startUp As Boolean = False
        ReadOnly _queueSkills As New Dictionary(Of String, Integer)
        ReadOnly _standardSkillStyle As ElementStyle
        ReadOnly _higherSkillStyle As ElementStyle
        ReadOnly _lowerSkillStyle As ElementStyle

        Private WriteOnly Property ForceUpdate() As Boolean
            Set(ByVal value As Boolean)
                If value = True And _startUp = False Then
                    HQFEvents.StartUpdateShipInfo = cboPilots.SelectedItem.ToString
                End If
            End Set
        End Property

#Region "Form Constructor"

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Set Styles
            _standardSkillStyle = adtSkills.Styles("Skill").Copy
            _higherSkillStyle = adtSkills.Styles("Skill").Copy
            _lowerSkillStyle = adtSkills.Styles("Skill").Copy
            _standardSkillStyle.TextColor = Color.Black
            _higherSkillStyle.TextColor = Color.LimeGreen
            _lowerSkillStyle.TextColor = Color.Red
        End Sub

#End Region

#Region "Form Loading & Closing Routines"

        Private Sub frmPilotManager_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            Call FittingPilots.SaveHQFPilotData()
        End Sub
        Private Sub frmPilotManager_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            _startUp = True

            ' Load the Implant Group Filters
            cboImplantFilter.Items.Clear()
            cboImplantFilter.Items.Add("<All Groups>")
            For Each cImplant As ShipModule In Implants.ImplantList.Values
                For Each implantGroup As String In cImplant.ImplantGroups
                    If cboImplantFilter.Items.Contains(implantGroup) = False Then
                        cboImplantFilter.Items.Add(implantGroup)
                    End If
                Next
            Next

            ' Load the Implant Manager groups
            Call LoadImplantManagerGroups()
            Call ShowImplantManagerGroups()

            ' Load the Implant Group Selection
            Call LoadImplantGroups()

            ' Add the current list of pilots to the combobox
            cboPilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboPilots.Items.Add(cPilot.Name)
                End If
            Next

            ' Set the list to the pilot name (or the first item if pilotname is blank)
            If PilotName <> "" Then
                If cboPilots.Items.Contains(PilotName) = True Then
                    cboPilots.SelectedItem = PilotName
                Else
                    If cboPilots.Items.Count > 0 Then
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            Else
                If cboPilots.Items.Count > 0 Then
                    cboPilots.SelectedIndex = 0
                End If
            End If

            _startUp = False

        End Sub
#End Region

#Region "Pilot Change Routines"
        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            _currentPilotName = cboPilots.SelectedItem.ToString
            _currentPilot = FittingPilots.HQFPilots.Item(_currentPilotName)
            Call UpdateSkillQueues(_currentPilotName)
            ' Display the pilot skills
            Call DisplayPilotSkills(chkShowModifiedSkills.Checked)
            ' Check if we have an implant group selected
            If _currentPilot.ImplantName(0) <> "" Then
                If _currentPilot.ImplantName(0) = "*Custom*" Then
                    cboImplantGroup.SelectedIndex = 0
                Else
                    If cboImplantGroup.Items.Contains(_currentPilot.ImplantName(0)) Then
                        cboImplantGroup.SelectedItem = _currentPilot.ImplantName(0)
                    Else
                        cboImplantGroup.SelectedIndex = 0
                    End If
                End If
            Else
                ' Select the Custom section
                cboImplantGroup.SelectedIndex = 0
            End If
            ' Set the SelectedIndex to 0 which will trigger the re-drawing of the implant list
            cboImplantFilter.SelectedIndex = 0
        End Sub
        Private Sub DisplayPilotSkills(ByVal showOnlyModified As Boolean)
            ' Loads the pilot skills - both defaults and revised
            If HQ.Settings.Pilots.ContainsKey(_currentPilotName) = True Then
                ' Get Core pilot
                Dim cPilot As EveHQPilot = HQ.Settings.Pilots(_currentPilotName)
                ' Get HQF pilot
                Dim hSkill As FittingSkill
                ' Display the skill groups
                adtSkills.BeginUpdate()
                adtSkills.Nodes.Clear()
                _queueSkills.Clear()
                Dim newSkillGroup As SkillGroup
                Dim newSkill As EveSkill
                Dim skillsModified As Boolean = False
                For Each newSkillGroup In HQ.SkillGroups.Values
                    If newSkillGroup.ID <> 505 Then
                        Dim groupNode As New Node
                        groupNode.Tag = newSkillGroup.ID
                        groupNode.Text = newSkillGroup.Name.Trim
                        groupNode.ImageIndex = 8
                        adtSkills.Nodes.Add(groupNode)
                        ' Now cycle through the list to get the skills
                        For Each newSkill In HQ.SkillListName.Values
                            If newSkill.GroupID <> 505 Then
                                If newSkill.GroupID = newSkillGroup.ID And newSkill.Published = True Then
                                    Dim skillNode As New Node
                                    skillNode.Text = newSkill.Name
                                    skillNode.Tag = newSkill.ID
                                    STT.SetSuperTooltip(skillNode, New SuperTooltipInfo(newSkill.Name, "Skill Description", newSkill.Description, Nothing, My.Resources.imgInfo1, eTooltipColor.Yellow))
                                    hSkill = _currentPilot.SkillSet.Item(newSkill.Name)
                                    If cPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                                        Dim mySkill As EveHQPilotSkill = cPilot.PilotSkills(newSkill.Name)
                                        skillNode.ImageIndex = mySkill.Level
                                        groupNode.Nodes.Add(skillNode)
                                        skillNode.Cells.Add(New Cell(mySkill.Level.ToString))
                                        skillNode.Cells.Add(New Cell(hSkill.Level.ToString))
                                    Else
                                        skillNode.ImageIndex = 10
                                        groupNode.Nodes.Add(skillNode)
                                        skillNode.Cells.Add(New Cell("0"))
                                        skillNode.Cells.Add(New Cell(hSkill.Level.ToString))
                                    End If
                                    ' Check for colouring to indicate changed skills
                                    If CInt(skillNode.Cells(1).Text) > CInt(skillNode.Cells(2).Text) Then
                                        ' Default is higher than HQF - red
                                        skillNode.Style = _lowerSkillStyle
                                        skillsModified = True
                                    Else
                                        If CInt(skillNode.Cells(1).Text) < CInt(skillNode.Cells(2).Text) Then
                                            ' HQF is higher than default - green
                                            skillNode.Style = _higherSkillStyle
                                            skillsModified = True
                                            ' add to the queue skills
                                            Dim r As New ReqSkill
                                            r.Name = hSkill.Name
                                            r.ID = hSkill.ID
                                            r.ReqLevel = hSkill.Level
                                            r.CurLevel = CInt(skillNode.Cells(1).Text)
                                            r.NeededFor = ""
                                            _queueSkills.Add(r.Name, r.ReqLevel)
                                        Else
                                            ' Default = HQF
                                            skillNode.Style = _standardSkillStyle
                                            If showOnlyModified = True Then
                                                groupNode.Nodes.Remove(skillNode)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        groupNode.Nodes.Sort()
                    End If
                Next
                ' Remove parents with no children
                Dim delList As New ArrayList
                For Each parentItem As Node In adtSkills.Nodes
                    If parentItem.Nodes.Count = 0 Then
                        delList.Add(parentItem)
                    End If
                Next
                For Each parentItem As Node In delList
                    adtSkills.Nodes.Remove(parentItem)
                Next
                adtSkills.Nodes.Sort()
                If adtSkills.Nodes.Count = 0 Then
                    adtSkills.Nodes.Add(New Node("(No Skills Modified)"))
                End If
                adtSkills.EndUpdate()
                If skillsModified = True Then
                    lblSkillsModified.Visible = True
                Else
                    lblSkillsModified.Visible = False
                End If
            End If
        End Sub
        Private Sub UpdateSkillQueues(ByVal currentPilotName As String)
            cboSkillQueue.BeginUpdate()
            cboSkillQueue.Items.Clear()
            For Each queueName As String In HQ.Settings.Pilots(currentPilotName).TrainingQueues.Keys
                cboSkillQueue.Items.Add(queueName)
            Next
            cboSkillQueue.EndUpdate()
            btnSetToSkillQueue.Enabled = False
        End Sub
#End Region

#Region "Skill Context Menu Routines"
        Private Sub ctxHQFLevel_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxHQFLevel.Opening
            If adtSkills.SelectedNodes.Count > 0 Then
                Dim selItem As Node = adtSkills.SelectedNodes(0)
                ' Cancel if a parent item (i.e. is a skill group)
                If selItem.Nodes.Count > 0 Then
                    e.Cancel = True
                Else
                    mnuSetSkillName.Text = selItem.Text
                End If
            Else
                e.Cancel = True
            End If
        End Sub
        Private Sub mnuSetLevel0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSetLevel0.Click
            Dim skillName As String = mnuSetSkillName.Text
            Dim hSkill As FittingSkill = _currentPilot.SkillSet.Item(skillName)
            hSkill.Level = 0
            adtSkills.SelectedNodes(0).Cells(2).Text = hSkill.Level.ToString
            Call ChangeSkillStatus(adtSkills.SelectedNodes(0))
        End Sub

        Private Sub mnuSetLevel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSetLevel1.Click
            Dim skillName As String = mnuSetSkillName.Text
            Dim hSkill As FittingSkill = _currentPilot.SkillSet.Item(skillName)
            hSkill.Level = 1
            adtSkills.SelectedNodes(0).Cells(2).Text = hSkill.Level.ToString
            Call ChangeSkillStatus(adtSkills.SelectedNodes(0))
        End Sub

        Private Sub mnuSetLevel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSetLevel2.Click
            Dim skillName As String = mnuSetSkillName.Text
            Dim hSkill As FittingSkill = _currentPilot.SkillSet.Item(skillName)
            hSkill.Level = 2
            adtSkills.SelectedNodes(0).Cells(2).Text = hSkill.Level.ToString
            Call ChangeSkillStatus(adtSkills.SelectedNodes(0))
        End Sub

        Private Sub mnuSetLevel3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSetLevel3.Click
            Dim skillName As String = mnuSetSkillName.Text
            Dim hSkill As FittingSkill = _currentPilot.SkillSet.Item(skillName)
            hSkill.Level = 3
            adtSkills.SelectedNodes(0).Cells(2).Text = hSkill.Level.ToString
            Call ChangeSkillStatus(adtSkills.SelectedNodes(0))
        End Sub

        Private Sub mnuSetLevel4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSetLevel4.Click
            Dim skillName As String = mnuSetSkillName.Text
            Dim hSkill As FittingSkill = _currentPilot.SkillSet.Item(skillName)
            hSkill.Level = 4
            adtSkills.SelectedNodes(0).Cells(2).Text = hSkill.Level.ToString
            Call ChangeSkillStatus(adtSkills.SelectedNodes(0))
        End Sub

        Private Sub mnuSetLevel5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSetLevel5.Click
            Dim skillName As String = mnuSetSkillName.Text
            Dim hSkill As FittingSkill = _currentPilot.SkillSet.Item(skillName)
            hSkill.Level = 5
            adtSkills.SelectedNodes(0).Cells(2).Text = hSkill.Level.ToString
            Call ChangeSkillStatus(adtSkills.SelectedNodes(0))
        End Sub

        Private Sub mnuSetDefault_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSetDefault.Click
            Dim skillName As String = mnuSetSkillName.Text
            Dim hSkill As FittingSkill = _currentPilot.SkillSet.Item(skillName)
            hSkill.Level = CInt(adtSkills.SelectedNodes(0).Cells(1).Text)
            adtSkills.SelectedNodes(0).Cells(2).Text = hSkill.Level.ToString
            Call ChangeSkillStatus(adtSkills.SelectedNodes(0))
        End Sub

        Private Sub ChangeSkillStatus(ByVal skillNode As Node)

            If CInt(skillNode.Cells(1).Text) > CInt(skillNode.Cells(2).Text) Then
                ' Default is higher than HQF - red
                skillNode.Style = _lowerSkillStyle
            Else
                If CInt(skillNode.Cells(1).Text) < CInt(skillNode.Cells(2).Text) Then
                    ' HQF is higher than default - green
                    skillNode.Style = _higherSkillStyle
                Else
                    ' Default = HQF
                    skillNode.Style = _standardSkillStyle
                    If chkShowModifiedSkills.Checked = True Then
                        skillNode.Parent.Nodes.Remove(skillNode)
                    End If
                End If
            End If
            ' Remove parents with no children
            Dim delList As New ArrayList
            For Each parentItem As Node In adtSkills.Nodes
                If parentItem.Nodes.Count = 0 Then
                    delList.Add(parentItem)
                End If
            Next
            For Each parentItem As Node In delList
                adtSkills.Nodes.Remove(parentItem)
            Next
            If adtSkills.Nodes.Count = 0 Then
                adtSkills.Nodes.Add(New Node("(No Skills Modified)"))
            End If
            ' Test if anything else is modified
            Dim skillsModified As Boolean = False
            For Each parentItem As Node In adtSkills.Nodes
                For Each skillItem As Node In parentItem.Nodes
                    If skillItem.Style.TextColor <> Color.Black Then
                        skillsModified = True
                        Exit For
                    End If
                Next
            Next
            lblSkillsModified.Visible = skillsModified
            ForceUpdate = True
        End Sub

#End Region

#Region "Skill Routines"

        Private Sub btnResetAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetAll.Click
            Call FittingPilots.ResetSkillsToDefault(_currentPilot)
            Call DisplayPilotSkills(chkShowModifiedSkills.Checked)
            ForceUpdate = True
        End Sub

        Private Sub btnSetAllToLevel5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetAllToLevel5.Click
            Call FittingPilots.SetAllSkillsToLevel5(_currentPilot)
            Call DisplayPilotSkills(chkShowModifiedSkills.Checked)
            ForceUpdate = True
        End Sub

        Private Sub chkShowModifiedSkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkShowModifiedSkills.CheckedChanged
            Call DisplayPilotSkills(chkShowModifiedSkills.Checked)
        End Sub

        Private Sub btnUpdateSkills_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateSkills.Click
            Call FittingPilots.UpdateHQFSkillsToActual(_currentPilot)
            Call DisplayPilotSkills(chkShowModifiedSkills.Checked)
            ForceUpdate = True
        End Sub

        Private Sub btnSetToSkillQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetToSkillQueue.Click
            If cboSkillQueue.SelectedItem IsNot Nothing Then
                Call FittingPilots.SetSkillsToSkillQueue(_currentPilot, cboSkillQueue.SelectedItem.ToString)
                Call DisplayPilotSkills(chkShowModifiedSkills.Checked)
                ForceUpdate = True
            End If
        End Sub

        Private Sub cboSkillQueue_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboSkillQueue.SelectedIndexChanged
            If cboSkillQueue.SelectedIndex <> -1 Then
                btnSetToSkillQueue.Enabled = True
            End If
        End Sub

        Private Sub btnAddHQFSkillstoQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddHQFSkillsToQueue.Click
            Call AddNeededSkillsToQueue()
        End Sub

        Private Sub AddNeededSkillsToQueue()
            Using selQ As New FrmSelectQueue(_currentPilot.PilotName, _queueSkills, "HQF: Pilot Manager")
                selQ.ShowDialog()
            End Using
            SkillQueueFunctions.StartQueueRefresh = True
        End Sub

        Private Sub btnImportSkillsFromEFT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImportSkillsFromEFT.Click
            ' Create a new file dialog
            Dim ofd1 As New OpenFileDialog
            With ofd1
                .Title = "Select EFT Character File..."
                .FileName = ""
                .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                .Filter = "EFT Character Files (*.chr)|*.chr|All Files (*.*)|*.*"
                .FilterIndex = 1
                .RestoreDirectory = True
                If .ShowDialog() = DialogResult.OK Then
                    If My.Computer.FileSystem.FileExists(.FileName) = False Then
                        MessageBox.Show("Specified file does not exist. Please try again.", "Error Finding File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    Else
                        ' Open the file for reading
                        Dim sr As New StreamReader(.FileName)
                        Dim charFile As String = sr.ReadToEnd
                        sr.Close()
                        ' Parse the file
                        Dim skillList() As String = charFile.Split(ControlChars.CrLf.ToCharArray)
                        Dim skillName As String
                        Dim skillLevel As Integer
                        Dim newSkills As New SortedList(Of String, Integer)
                        For Each eftSkill As String In skillList
                            If eftSkill.Trim <> "" Then
                                ' Get the skill and level
                                If eftSkill.Substring(eftSkill.Length - 2, 1) = "=" Then
                                    skillName = eftSkill.Substring(0, eftSkill.Length - 2)
                                    skillLevel = CInt(eftSkill.Substring(eftSkill.Length - 1, 1))
                                    ' Check if this is a valid skill and skill level
                                    If HQ.SkillListName.ContainsKey(skillName) = True Then
                                        If skillLevel >= 0 And skillLevel < 6 Then
                                            ' Seems valid - add it to our list
                                            newSkills.Add(skillName, skillLevel)
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        If newSkills.Count > 0 Then
                            ' Add these skills to our HQF pilot
                            Call FittingPilots.SetSkillsToSkillList(_currentPilot, newSkills)
                            Call DisplayPilotSkills(chkShowModifiedSkills.Checked)
                            ForceUpdate = True
                            MessageBox.Show("Successfully imported " & newSkills.Count.ToString & " skills from the EFT Character file.", "Import Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("This file does not contain any valid Eve skills and skill levels", "Import Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            End With
        End Sub

#End Region

#Region "Implant Routines"
        Private Sub cboImplantGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboImplantGroup.SelectedIndexChanged
            cboImplantFilter.Enabled = True
            If cboImplantFilter.SelectedItem Is Nothing Then
                cboImplantFilter.SelectedIndex = 0
            End If
            _currentPilot.ImplantName(0) = cboImplantGroup.SelectedItem.ToString
            If cboImplantGroup.SelectedItem.ToString <> "*Custom*" Then
                Dim currentImplantGroup As ImplantCollection = PluginSettings.HQFSettings.ImplantGroups(cboImplantGroup.SelectedItem.ToString)
                For imp As Integer = 1 To 10
                    _currentPilot.ImplantName(imp) = currentImplantGroup.ImplantName(imp)
                Next
            End If
            Call DrawImplantTree()
            ForceUpdate = True
        End Sub
        Private Sub DrawImplantTree()
            tvwImplants.BeginUpdate()
            tvwImplants.Nodes.Clear()
            For slot As Integer = 1 To 10
                If ModuleLists.ModuleListName.ContainsKey(_currentPilot.ImplantName(slot)) = False Then
                    _currentPilot.ImplantName(slot) = ""
                End If
                If _currentPilot.ImplantName(slot) = "" Then
                    tvwImplants.Nodes.Add("Slot " & slot.ToString, "Slot " & slot.ToString)
                Else
                    tvwImplants.Nodes.Add("Slot " & slot.ToString, _currentPilot.ImplantName(slot))
                End If
                tvwImplants.Nodes("Slot " & slot.ToString).Nodes.Add("No Implant")
            Next
            For Each cImplant As ShipModule In Implants.ImplantList.Values
                If cImplant.ImplantSlot <= 10 Then
                    If cboImplantFilter.SelectedItem.ToString = "<All Groups>" Then
                        tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                        ' Check if this is the selected one!
                        If tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                            tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Color.LimeGreen
                        End If
                    Else
                        If cImplant.ImplantGroups.Contains(cboImplantFilter.SelectedItem.ToString) = True Then
                            tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                            ' Check if this is the selected one!
                            If tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                                tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Color.LimeGreen
                            End If
                        End If
                    End If
                End If
            Next
            tvwImplants.EndUpdate()
        End Sub
        Private Sub cboImplantFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboImplantFilter.SelectedIndexChanged
            Call DrawImplantTree()
        End Sub
        Private Sub tvwImplants_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvwImplants.AfterSelect
            If e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
                Dim implantName As String = e.Node.Text
                Dim cImplant As ShipModule = Implants.ImplantList.Item(implantName)
                lblImplantDescription.Text = cImplant.Description
            Else
                lblImplantDescription.Text = ""
            End If
        End Sub
        Private Sub tvwImplants_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwImplants.NodeMouseClick
            If e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
                Dim implantName As String = e.Node.Text
                Dim cImplant As ShipModule = Implants.ImplantList.Item(implantName)
                lblImplantDescription.Text = cImplant.Description
            Else
                lblImplantDescription.Text = ""
            End If
        End Sub
        Private Sub tvwImplants_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwImplants.NodeMouseDoubleClick
            'Work out if we need to replace an implant
            Dim currentImplant As String = ""
            Dim currentSlot As Integer = 0
            If e.Node.Parent IsNot Nothing Then
                currentSlot = CInt(e.Node.Parent.Name.Substring(5))
                If e.Node.Parent.Text.StartsWith("Slot") = False Then
                    currentImplant = e.Node.Parent.Text
                End If
            End If
            If e.Node.Nodes.Count = 0 And e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
                Dim implantName As String = e.Node.Text
                Dim cImplant As ShipModule = Implants.ImplantList.Item(implantName)
                lblImplantDescription.Text = cImplant.Description
                e.Node.Parent.Text = cImplant.Name
                _currentPilot.ImplantName(currentSlot) = cImplant.Name
                ' Set the node colour
                e.Node.ForeColor = Color.LimeGreen
                ' Remove the old node colour
                If currentImplant <> "" And currentImplant <> cImplant.Name Then
                    If e.Node.Parent.Nodes.ContainsKey(currentImplant) = True Then
                        e.Node.Parent.Nodes(currentImplant).ForeColor = tvwImplants.ForeColor
                    End If
                End If
            Else
                If e.Node.Text = "No Implant" Then
                    If e.Node.Parent IsNot Nothing Then
                        If e.Node.Parent.Text.StartsWith("Slot") = False Then
                            currentImplant = e.Node.Parent.Text
                            _currentPilot.ImplantName(currentSlot) = ""
                        End If
                    End If
                    e.Node.Parent.Text = e.Node.Parent.Name
                    If currentImplant <> "" Then
                        e.Node.Parent.Nodes(currentImplant).ForeColor = tvwImplants.ForeColor
                    End If
                End If
            End If
            ' Switch to the custom group
            cboImplantGroup.SelectedIndex = 0
            ForceUpdate = True
        End Sub
        Private Sub btnCollapseAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCollapseAll.Click
            tvwImplants.CollapseAll()
        End Sub
        Private Sub btnSaveGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveGroup.Click
            ' Clear the text boxes and show the dialog box
            Using myGroup As New FrmModifyImplantGroups
                With myGroup
                    .txtGroupName.Text = "" : .txtGroupName.Enabled = True
                    .btnAccept.Text = "Add" : .Tag = "Add"
                    .Text = "Add New Group"
                    .ShowDialog()
                End With
        
                ' Set the implant group name is successful
                If myGroup.txtGroupName.Tag IsNot Nothing Then
                    Dim implantGroupName As String = myGroup.txtGroupName.Tag.ToString
                    Dim implantgroup As ImplantCollection = PluginSettings.HQFSettings.ImplantGroups.Item(implantGroupName)
                    ' Add the implants to the implant group
                    For Each impNode As TreeNode In tvwImplants.Nodes
                        If impNode.Text <> "No Implant" And impNode.Text.StartsWith("Slot") = False Then
                            implantgroup.ImplantName(impNode.Index + 1) = impNode.Text
                        Else
                            implantgroup.ImplantName(impNode.Index + 1) = ""
                        End If
                    Next

                    ' Update the group list in the Implant Manager
                    Call LoadImplantManagerGroups()
                    Call ShowImplantManagerGroups()

                    ' Update the group list
                    Call LoadImplantGroups()

                    ' Set the correct group name
                    cboImplantGroup.SelectedItem = implantGroupName

                End If

                ' Dispose of the form
            End Using
        End Sub
        Private Sub LoadImplantGroups()
            cboImplantGroup.BeginUpdate()
            cboImplantGroup.Items.Clear()
            cboImplantGroup.Items.Add("*Custom*")
            For Each iG As ImplantCollection In PluginSettings.HQFSettings.ImplantGroups.Values
                cboImplantGroup.Items.Add(iG.GroupName)
            Next
            cboImplantGroup.EndUpdate()
        End Sub

#End Region

#Region "Implant Manager Routines"
        Private Sub LoadImplantManagerGroups()
            cboImplantFilterM.Items.Clear()
            cboImplantFilterM.Items.Add("<All Groups>")
            For Each cImplant As ShipModule In Implants.ImplantList.Values
                For Each implantGroup As String In cImplant.ImplantGroups
                    If cboImplantFilterM.Items.Contains(implantGroup) = False Then
                        cboImplantFilterM.Items.Add(implantGroup)
                    End If
                Next
            Next
            cboImplantFilterM.Enabled = False
        End Sub
        Private Sub ShowImplantManagerGroups()
            lstImplantGroups.BeginUpdate()
            lstImplantGroups.Items.Clear()
            For Each iG As ImplantCollection In PluginSettings.HQFSettings.ImplantGroups.Values
                lstImplantGroups.Items.Add(iG.GroupName)
            Next
            lstImplantGroups.EndUpdate()
        End Sub
        Private Sub DrawImplantManagerTree()
            If _currentGroup IsNot Nothing Then
                tvwImplantsM.BeginUpdate()
                tvwImplantsM.Nodes.Clear()
                For slot As Integer = 1 To 10
                    If ModuleLists.ModuleListName.ContainsKey(_currentGroup.ImplantName(slot)) = False Then
                        _currentGroup.ImplantName(slot) = ""
                    End If
                    If _currentGroup.ImplantName(slot) = "" Then
                        tvwImplantsM.Nodes.Add("Slot " & slot.ToString, "Slot " & slot.ToString)
                    Else
                        tvwImplantsM.Nodes.Add("Slot " & slot.ToString, _currentGroup.ImplantName(slot))
                    End If
                    tvwImplantsM.Nodes("Slot " & slot.ToString).Nodes.Add("No Implant")
                Next
                For Each cImplant As ShipModule In Implants.ImplantList.Values
                    If cImplant.ImplantSlot <= 10 Then
                        If cboImplantFilterM.SelectedItem.ToString = "<All Groups>" Then
                            tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                            ' Check if this is the selected one!
                            If tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                                tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Color.LimeGreen
                            End If
                        Else
                            If cImplant.ImplantGroups.Contains(cboImplantFilterM.SelectedItem.ToString) = True Then
                                tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                                ' Check if this is the selected one!
                                If tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                                    tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Color.LimeGreen
                                End If
                            End If
                        End If
                    End If
                Next
                tvwImplantsM.EndUpdate()
            End If
        End Sub
        Private Sub tvwImplantsM_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvwImplantsM.AfterSelect
            If e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
                Dim implantName As String = e.Node.Text
                Dim cImplant As ShipModule = Implants.ImplantList.Item(implantName)
                lblImplantDescriptionM.Text = cImplant.Description
            End If
        End Sub
        Private Sub tvwImplantsM_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwImplantsM.NodeMouseClick
            If e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
                Dim implantName As String = e.Node.Text
                Dim cImplant As ShipModule = Implants.ImplantList.Item(implantName)
                lblImplantDescriptionM.Text = cImplant.Description
            End If
        End Sub
        Private Sub tvwImplantsM_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwImplantsM.NodeMouseDoubleClick
            'Work out if we need to replace an image
            Dim currentImplant As String = ""
            Dim currentSlot As Integer = 0
            If e.Node.Parent IsNot Nothing Then
                currentSlot = CInt(e.Node.Parent.Name.Substring(5))
                If e.Node.Parent.Text.StartsWith("Slot") = False Then
                    currentImplant = e.Node.Parent.Text
                End If
            End If
            If e.Node.Nodes.Count = 0 And e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
                Dim implantName As String = e.Node.Text
                Dim cImplant As ShipModule = Implants.ImplantList.Item(implantName)
                lblImplantDescriptionM.Text = cImplant.Description
                e.Node.Parent.Text = cImplant.Name
                _currentGroup.ImplantName(currentSlot) = cImplant.Name
                ' Set the node colour
                e.Node.ForeColor = Color.LimeGreen
                ' Remove the old node colour
                If currentImplant <> "" And currentImplant <> cImplant.Name Then
                    If e.Node.Parent.Nodes.ContainsKey(currentImplant) = True Then
                        e.Node.Parent.Nodes(currentImplant).ForeColor = tvwImplantsM.ForeColor
                    End If
                End If
            Else
                If e.Node.Text = "No Implant" Then
                    If e.Node.Parent IsNot Nothing Then
                        If e.Node.Parent.Text.StartsWith("Slot") = False Then
                            currentImplant = e.Node.Parent.Text
                            _currentGroup.ImplantName(currentSlot) = ""
                        End If
                    End If
                    e.Node.Parent.Text = e.Node.Parent.Name
                    If currentImplant <> "" Then
                        If e.Node.Parent.Nodes.ContainsKey(currentImplant) = True Then
                            e.Node.Parent.Nodes(currentImplant).ForeColor = tvwImplantsM.ForeColor
                        End If
                    End If
                End If
            End If
            ' Update the pilots for this group
            For Each cPilot As FittingPilot In FittingPilots.HQFPilots.Values
                If cPilot.ImplantName(0) = _currentGroup.GroupName Then
                    If e.Node.Nodes.Count = 0 And e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
                        cPilot.ImplantName(currentSlot) = e.Node.Text
                    Else
                        cPilot.ImplantName(currentSlot) = ""
                    End If
                End If
            Next
            Call DrawImplantTree()
            ForceUpdate = True
        End Sub
        Private Sub cboImplantFilterM_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboImplantFilterM.SelectedIndexChanged
            Call DrawImplantManagerTree()
        End Sub
        Private Sub btnCollapseAllM_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCollapseAllM.Click
            tvwImplantsM.CollapseAll()
        End Sub
        Private Sub btnAddImplantGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddImplantGroup.Click
            ' Clear the text boxes
            Using myGroup As New FrmModifyImplantGroups
                With myGroup
                    .txtGroupName.Text = "" : .txtGroupName.Enabled = True
                    .btnAccept.Text = "Add" : .Tag = "Add"
                    .Text = "Add New Group"
                    .ShowDialog()
                End With
                Call ShowImplantManagerGroups()
                ' Redraw implant groups in the implant selection combobox
                Dim oldImplantGroup As String = cboImplantGroup.SelectedItem.ToString
                Call LoadImplantGroups()
                cboImplantGroup.SelectedItem = oldImplantGroup
            End Using
            HQFEvents.StartUpdateImplantComboBox = True
            ForceUpdate = True
        End Sub
        Private Sub btnEditImplantGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditImplantGroup.Click
            ' Check for some selection on the listview
            Dim oldImplantGroup As String = cboImplantGroup.SelectedItem.ToString
            If lstImplantGroups.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a Group to edit!", "Cannot Edit Implant Group", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lstImplantGroups.Select()
            Else
                Using myGroup As New FrmModifyImplantGroups
                    With myGroup
                        ' Load the account details into the text boxes
                        Dim selGroup As String = lstImplantGroups.SelectedItems(0).ToString
                        .txtGroupName.Text = selGroup : .txtGroupName.Tag = selGroup
                        .btnAccept.Text = "Edit" : .Tag = "Edit"
                        .Text = "Edit '" & selGroup & "' Queue Details"
                        .ShowDialog()
                    End With
                    Call ShowImplantManagerGroups()
                    ' Redraw implant groups in the implant selection combobox
                    Dim newImplantGroup As String = myGroup.txtGroupName.Tag.ToString
                    Call LoadImplantGroups()
                    If PluginSettings.HQFSettings.ImplantGroups.ContainsKey(oldImplantGroup) = True Then
                        cboImplantGroup.SelectedItem = oldImplantGroup
                    Else
                        cboImplantGroup.SelectedItem = newImplantGroup
                    End If
                    lblCurrentGroup.Text = "Current Group: " & newImplantGroup
                    HQFEvents.StartUpdateImplantComboBox = True
                End Using
            End If
        End Sub
        Private Sub btnRemoveImplantGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveImplantGroup.Click
            ' Check for some selection on the listview
            If lstImplantGroups.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a Group to delete!", "Cannot Delete Implant Group", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lstImplantGroups.Select()
            Else
                Dim oldImplantGroup As String = cboImplantGroup.SelectedItem.ToString
                Dim selGroup As String = lstImplantGroups.SelectedItems(0).ToString
                ' Confirm deletion
                Dim msg As String = ""
                msg &= "Are you sure you wish to delete the '" & selGroup & "' Implant Group?"
                Dim confirm As Integer = MessageBox.Show(msg, "Confirm Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If confirm = DialogResult.Yes Then
                    ' Delete the queue the accounts collection
                    PluginSettings.HQFSettings.ImplantGroups.Remove(selGroup)
                    Call ShowImplantManagerGroups()
                    ' Redraw implant groups in the implant selection combobox
                    Call LoadImplantGroups()
                    If PluginSettings.HQFSettings.ImplantGroups.ContainsKey(oldImplantGroup) = True Then
                        cboImplantGroup.SelectedItem = oldImplantGroup
                    Else
                        cboImplantGroup.SelectedItem = "*Custom*"
                    End If
                    HQFEvents.StartUpdateImplantComboBox = True
                Else
                    lstImplantGroups.Select()
                    Exit Sub
                End If
            End If
        End Sub
        Private Sub lstImplantGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstImplantGroups.SelectedIndexChanged
            If lstImplantGroups.SelectedItems.Count > 0 Then
                cboImplantFilterM.Enabled = True
                If cboImplantFilterM.SelectedItem Is Nothing Then
                    cboImplantFilterM.SelectedIndex = 0
                End If
                _currentGroup = PluginSettings.HQFSettings.ImplantGroups(lstImplantGroups.SelectedItem.ToString)
                lblCurrentGroup.Text = "Current Group: " & _currentGroup.GroupName
                Call DrawImplantManagerTree()
            End If
        End Sub
#End Region

    End Class
End NameSpace