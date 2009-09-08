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
Imports DotNetLib.Windows.Forms
Imports System.Windows.Forms

Public Class frmPilotManager

    Public pilotName As String = ""
    Dim currentPilotName As String = ""
    Dim currentPilot As HQFPilot
    Dim currentGroup As ImplantGroup
    Dim StartUp As Boolean = False

    Private WriteOnly Property ForceUpdate() As Boolean
        Set(ByVal value As Boolean)
            If value = True And StartUp = False Then
                HQFEvents.StartUpdateShipInfo = cboPilots.SelectedItem.ToString
            End If
        End Set
    End Property

    Private WriteOnly Property ForceImplantUpdate() As Boolean
        Set(ByVal value As Boolean)
            If value = True And StartUp = False Then
                HQFEvents.StartUpdateAllImplantLists = True
            End If
        End Set
    End Property

#Region "Form Loading & Closing Routines"

    Private Sub frmPilotManager_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Call HQFPilotCollection.SaveHQFPilotData()
    End Sub
    Private Sub frmPilotManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        StartUp = True

        ' Load the Implant Group Filters
        cboImplantFilter.Items.Clear()
        cboImplantFilter.Items.Add("<All Groups>")
        For Each cImplant As ShipModule In Implants.implantList.Values
            For Each implantGroup As String In cImplant.ImplantGroups
                If cboImplantFilter.Items.Contains(implantGroup) = False Then
                    cboImplantFilter.Items.Add(implantGroup)
                End If
            Next
        Next

        ' Load the Implant Manager groups
        Call Me.LoadImplantManagerGroups()
        Call Me.ShowImplantManagerGroups()

        ' Load the Implant Group Selection
        Call Me.LoadImplantGroups()

        ' Add the current list of pilots to the combobox
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next

        ' Set the list to the pilot name (or the first item if pilotname is blank)
        If pilotName <> "" Then
            If cboPilots.Items.Contains(pilotName) = True Then
                cboPilots.SelectedItem = pilotName
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

        StartUp = False

    End Sub
#End Region

#Region "Pilot Change Routines"
    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        currentPilotName = cboPilots.SelectedItem.ToString
        currentPilot = CType(HQF.HQFPilotCollection.HQFPilots.Item(currentPilotName), HQFPilot)
        Call Me.UpdateSkillQueues(currentPilotName)
        ' Display the pilot skills
        Call Me.DisplayPilotSkills(chkShowModifiedSkills.Checked)
        ' Check if we have an implant group selected
        If currentPilot.ImplantName(0) <> "" Then
            If currentPilot.ImplantName(0) = "*Custom*" Then
                cboImplantGroup.SelectedIndex = 0
            Else
                If cboImplantGroup.Items.Contains(currentPilot.ImplantName(0)) Then
                    cboImplantGroup.SelectedItem = currentPilot.ImplantName(0)
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
    Private Sub DisplayPilotSkills(ByVal ShowOnlyModified As Boolean)
        ' Loads the pilot skills - both defaults and revised
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(currentPilotName) = True Then
            ' Get Core pilot
            Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentPilotName), Core.Pilot)
            ' Get HQF pilot
            Dim hSkill As HQFSkill
            ' Display the skill groups
            clvSkills.BeginUpdate()
            clvSkills.Items.Clear()
            Dim newSkillGroup As EveHQ.Core.SkillGroup
            Dim newSkill As EveHQ.Core.EveSkill
            Dim SkillsModified As Boolean = False
            For Each newSkillGroup In EveHQ.Core.HQ.SkillGroups.Values
                If newSkillGroup.ID <> "505" Then
                    Dim groupNode As New ContainerListViewItem
                    groupNode.Tag = newSkillGroup.ID
                    groupNode.Text = newSkillGroup.Name.Trim
                    groupNode.ImageIndex = 8
                    groupNode.SelectedImageIndex = 8
                    clvSkills.Items.Add(groupNode)
                    ' Now cycle through the list to get the skills
                    For Each newSkill In EveHQ.Core.HQ.SkillListID.Values
                        If newSkill.GroupID <> "505" Then
                            If newSkill.GroupID = newSkillGroup.ID And newSkill.Published = True Then
                                Dim skillNode As New ContainerListViewItem
                                skillNode.Text = newSkill.Name
                                skillNode.Tag = newSkill.ID
                                hSkill = CType(currentPilot.SkillSet.Item(newSkill.Name), HQFSkill)
                                If cPilot.PilotSkills.Contains(newSkill.Name) = True Then
                                    Dim mySkill As EveHQ.Core.PilotSkill = CType(cPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                                    skillNode.ImageIndex = mySkill.Level
                                    skillNode.SelectedImageIndex = mySkill.Level
                                    groupNode.Items.Add(skillNode)
                                    skillNode.SubItems(1).Text = mySkill.Level.ToString
                                    skillNode.SubItems(2).Text = hSkill.Level.ToString
                                Else
                                    skillNode.ImageIndex = 10
                                    skillNode.SelectedImageIndex = 10
                                    groupNode.Items.Add(skillNode)
                                    skillNode.SubItems(1).Text = "0"
                                    skillNode.SubItems(2).Text = hSkill.Level.ToString
                                End If
                                ' Check for colouring to indicate changed skills
                                If CInt(skillNode.SubItems(1).Text) > CInt(skillNode.SubItems(2).Text) Then
                                    ' Default is higher than HQF - red
                                    skillNode.ForeColor = Drawing.Color.Red
                                    SkillsModified = True
                                Else
                                    If CInt(skillNode.SubItems(1).Text) < CInt(skillNode.SubItems(2).Text) Then
                                        ' HQF is higher than default - green
                                        skillNode.ForeColor = Drawing.Color.LimeGreen
                                        SkillsModified = True
                                    Else
                                        ' Default = HQF
                                        skillNode.ForeColor = Drawing.Color.Black
                                        If ShowOnlyModified = True Then
                                            groupNode.Items.Remove(skillNode)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Next
            ' Remove parents with no children
            Dim delList As New ArrayList
            For Each parentItem As ContainerListViewItem In clvSkills.Items
                If parentItem.Items.Count = 0 Then
                    delList.Add(parentItem)
                End If
            Next
            For Each parentItem As ContainerListViewItem In delList
                clvSkills.Items.Remove(parentItem)
            Next
            If clvSkills.Items.Count = 0 Then
                clvSkills.Items.Add("(No Skills Modified)")
            End If
            clvSkills.EndUpdate()
            If SkillsModified = True Then
                lblSkillsModified.Visible = True
            Else
                lblSkillsModified.Visible = False
            End If
        End If
    End Sub
    Private Sub UpdateSkillQueues(ByVal currentPilotName As String)
        cboSkillQueue.BeginUpdate()
        cboSkillQueue.Items.Clear()
        For Each queueName As String In CType(EveHQ.Core.HQ.EveHQSettings.Pilots(currentPilotName), EveHQ.Core.Pilot).TrainingQueues.Keys
            cboSkillQueue.Items.Add(queueName)
        Next
        cboSkillQueue.EndUpdate()
        btnSetToSkillQueue.Enabled = False
    End Sub
#End Region

#Region "Skill Context Menu Routines"
    Private Sub ctxHQFLevel_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxHQFLevel.Opening
        If clvSkills.SelectedItems.Count > 0 Then
            Dim selItem As ContainerListViewItem = clvSkills.SelectedItems(0)
            ' Cancel if a parent item (i.e. is a skill group)
            If selItem.Items.Count > 0 Then
                e.Cancel = True
            Else
                mnuSetSkillName.Text = selItem.Text
            End If
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuSetLevel0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSetLevel0.Click
        Dim skillName As String = mnuSetSkillName.Text
        Dim hSkill As HQFSkill = CType(currentPilot.SkillSet.Item(skillName), HQFSkill)
        hSkill.Level = 0
        clvSkills.SelectedItems(0).SubItems(2).Text = hSkill.Level.ToString
        Call Me.ChangeSkillStatus(clvSkills.SelectedItems(0))
    End Sub

    Private Sub mnuSetLevel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSetLevel1.Click
        Dim skillName As String = mnuSetSkillName.Text
        Dim hSkill As HQFSkill = CType(currentPilot.SkillSet.Item(skillName), HQFSkill)
        hSkill.Level = 1
        clvSkills.SelectedItems(0).SubItems(2).Text = hSkill.Level.ToString
        Call Me.ChangeSkillStatus(clvSkills.SelectedItems(0))
    End Sub

    Private Sub mnuSetLevel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSetLevel2.Click
        Dim skillName As String = mnuSetSkillName.Text
        Dim hSkill As HQFSkill = CType(currentPilot.SkillSet.Item(skillName), HQFSkill)
        hSkill.Level = 2
        clvSkills.SelectedItems(0).SubItems(2).Text = hSkill.Level.ToString
        Call Me.ChangeSkillStatus(clvSkills.SelectedItems(0))
    End Sub

    Private Sub mnuSetLevel3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSetLevel3.Click
        Dim skillName As String = mnuSetSkillName.Text
        Dim hSkill As HQFSkill = CType(currentPilot.SkillSet.Item(skillName), HQFSkill)
        hSkill.Level = 3
        clvSkills.SelectedItems(0).SubItems(2).Text = hSkill.Level.ToString
        Call Me.ChangeSkillStatus(clvSkills.SelectedItems(0))
    End Sub

    Private Sub mnuSetLevel4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSetLevel4.Click
        Dim skillName As String = mnuSetSkillName.Text
        Dim hSkill As HQFSkill = CType(currentPilot.SkillSet.Item(skillName), HQFSkill)
        hSkill.Level = 4
        clvSkills.SelectedItems(0).SubItems(2).Text = hSkill.Level.ToString
        Call Me.ChangeSkillStatus(clvSkills.SelectedItems(0))
    End Sub

    Private Sub mnuSetLevel5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSetLevel5.Click
        Dim skillName As String = mnuSetSkillName.Text
        Dim hSkill As HQFSkill = CType(currentPilot.SkillSet.Item(skillName), HQFSkill)
        hSkill.Level = 5
        clvSkills.SelectedItems(0).SubItems(2).Text = hSkill.Level.ToString
        Call Me.ChangeSkillStatus(clvSkills.SelectedItems(0))
    End Sub

    Private Sub mnuSetDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSetDefault.Click
        Dim skillName As String = mnuSetSkillName.Text
        Dim hSkill As HQFSkill = CType(currentPilot.SkillSet.Item(skillName), HQFSkill)
        hSkill.Level = CInt(clvSkills.SelectedItems(0).SubItems(1).Text)
        clvSkills.SelectedItems(0).SubItems(2).Text = hSkill.Level.ToString
        Call Me.ChangeSkillStatus(clvSkills.SelectedItems(0))
    End Sub

    Private Sub ChangeSkillStatus(ByVal skillNode As ContainerListViewItem)

        If CInt(skillNode.SubItems(1).Text) > CInt(skillNode.SubItems(2).Text) Then
            ' Default is higher than HQF - red
            skillNode.ForeColor = Drawing.Color.Red
        Else
            If CInt(skillNode.SubItems(1).Text) < CInt(skillNode.SubItems(2).Text) Then
                ' HQF is higher than default - green
                skillNode.ForeColor = Drawing.Color.LimeGreen
            Else
                ' Default = HQF
                skillNode.ForeColor = Drawing.Color.Black
                If chkShowModifiedSkills.Checked = True Then
                    skillNode.ParentItem.Items.Remove(skillNode)
                End If
            End If
        End If
        ' Remove parents with no children
        Dim delList As New ArrayList
        For Each parentItem As ContainerListViewItem In clvSkills.Items
            If parentItem.Items.Count = 0 Then
                delList.Add(parentItem)
            End If
        Next
        For Each parentItem As ContainerListViewItem In delList
            clvSkills.Items.Remove(parentItem)
        Next
        If clvSkills.Items.Count = 0 Then
            clvSkills.Items.Add("(No Skills Modified)")
        End If
        ' Test if anything else is modified
        Dim SkillsModified As Boolean = False
        For Each parentItem As ContainerListViewItem In clvSkills.Items
            For Each skillItem As ContainerListViewItem In parentItem.Items
                If skillItem.ForeColor <> Drawing.Color.Black Then
                    SkillsModified = True
                    Exit For
                End If
            Next
        Next
        lblSkillsModified.Visible = SkillsModified
        ForceUpdate = True
    End Sub

#End Region

#Region "Skill Routines"

    Private Sub btnResetAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetAll.Click
        Call HQFPilotCollection.ResetSkillsToDefault(currentPilot)
        Call Me.DisplayPilotSkills(chkShowModifiedSkills.Checked)
        forceUpdate = True
    End Sub

    Private Sub btnSetAllToLevel5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetAllToLevel5.Click
        Call HQFPilotCollection.SetAllSkillsToLevel5(currentPilot)
        Call Me.DisplayPilotSkills(chkShowModifiedSkills.Checked)
        forceUpdate = True
    End Sub

    Private Sub chkShowModifiedSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowModifiedSkills.CheckedChanged
        Call Me.DisplayPilotSkills(chkShowModifiedSkills.Checked)
    End Sub

    Private Sub btnUpdateSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateSkills.Click
        Call HQFPilotCollection.UpdateHQFSkillsToActual(currentPilot)
        Call Me.DisplayPilotSkills(chkShowModifiedSkills.Checked)
        forceUpdate = True
    End Sub

    Private Sub btnSetToSkillQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetToSkillQueue.Click
        If cboSkillQueue.SelectedItem IsNot Nothing Then
            Call HQFPilotCollection.SetSkillsToSkillQueue(currentPilot, cboSkillQueue.SelectedItem.ToString)
            Call Me.DisplayPilotSkills(chkShowModifiedSkills.Checked)
            ForceUpdate = True
        End If
    End Sub

    Private Sub cboSkillQueue_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSkillQueue.SelectedIndexChanged
        If cboSkillQueue.SelectedIndex <> -1 Then
            btnSetToSkillQueue.Enabled = True
        End If
    End Sub

#End Region

#Region "Implant Routines"
    Private Sub cboUseImplantGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboImplantGroup.SelectedIndexChanged
        cboImplantFilter.Enabled = True
        If cboImplantFilter.SelectedItem Is Nothing Then
            cboImplantFilter.SelectedIndex = 0
        End If
        currentPilot.ImplantName(0) = cboImplantGroup.SelectedItem.ToString
        If cboImplantGroup.SelectedItem.ToString <> "*Custom*" Then
            Dim currentImplantGroup As ImplantGroup = CType(Implants.implantGroups(cboImplantGroup.SelectedItem.ToString), ImplantGroup)
            For imp As Integer = 1 To 10
                currentPilot.ImplantName(imp) = currentImplantGroup.ImplantName(imp)
            Next
        End If
        Call DrawImplantTree()
        forceUpdate = True
    End Sub
    Private Sub DrawImplantTree()
        tvwImplants.BeginUpdate()
        tvwImplants.Nodes.Clear()
        For slot As Integer = 1 To 10
            If currentPilot.ImplantName(slot) = "" Then
                tvwImplants.Nodes.Add("Slot " & slot.ToString, "Slot " & slot.ToString)
            Else
                tvwImplants.Nodes.Add("Slot " & slot.ToString, currentPilot.ImplantName(slot))
            End If
            If cboImplantFilter.SelectedItem.ToString = "<All Groups>" Then
                tvwImplants.Nodes("Slot " & slot.ToString).Nodes.Add("No Implant")
            End If
        Next
        For Each cImplant As ShipModule In Implants.implantList.Values
            If cboImplantFilter.SelectedItem.ToString = "<All Groups>" Then
                tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                ' Check if this is the selected one!
                If tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                    tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Drawing.Color.LimeGreen
                End If
            Else
                If cImplant.ImplantGroups.Contains(cboImplantFilter.SelectedItem.ToString) = True Then
                    tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                    ' Check if this is the selected one!
                    If tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                        tvwImplants.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Drawing.Color.LimeGreen
                    End If
                End If
            End If
        Next
        tvwImplants.EndUpdate()
    End Sub
    Private Sub cboImplantGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboImplantFilter.SelectedIndexChanged
        Call Me.DrawImplantTree()
    End Sub
    Private Sub tvwImplants_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwImplants.NodeMouseClick
        If e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
            Dim implantName As String = e.Node.Text
            Dim cImplant As ShipModule = CType(Implants.implantList.Item(implantName), ShipModule)
            lblImplantDescription.Text = cImplant.Description
        Else
            lblImplantDescription.Text = ""
        End If
    End Sub
    Private Sub tvwImplants_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwImplants.NodeMouseDoubleClick
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
            Dim cImplant As ShipModule = CType(Implants.implantList.Item(implantName), ShipModule)
            lblImplantDescription.Text = cImplant.Description
            e.Node.Parent.Text = cImplant.Name
            currentPilot.ImplantName(currentSlot) = cImplant.Name
            ' Set the node colour
            e.Node.ForeColor = Drawing.Color.LimeGreen
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
                        currentPilot.ImplantName(currentSlot) = ""
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
        forceUpdate = True
    End Sub
    Private Sub btnCollapseAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCollapseAll.Click
        tvwImplants.CollapseAll()
    End Sub
    Private Sub btnSaveGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveGroup.Click
        ' Clear the text boxes and show the dialog box
        Dim myGroup As New frmModifyImplantGroups
        With myGroup
            .txtGroupName.Text = "" : .txtGroupName.Enabled = True
            .btnAccept.Text = "Add" : .Tag = "Add"
            .Text = "Add New Group"
            .ShowDialog()
        End With

        ' Set the implant group name is successful
        If myGroup.txtGroupName.Tag IsNot Nothing Then
            Dim implantGroupName As String = myGroup.txtGroupName.Tag.ToString
            Dim implantgroup As ImplantGroup = CType(Implants.implantGroups.Item(implantGroupName), ImplantGroup)
            ' Add the implants to the implant group
            For Each impNode As TreeNode In tvwImplants.Nodes
                If impNode.Text <> "No Implant" And impNode.Text.StartsWith("Slot") = False Then
                    implantgroup.ImplantName(impNode.Index + 1) = impNode.Text
                Else
                    implantgroup.ImplantName(impNode.Index + 1) = ""
                End If
            Next

            ' Update the group list in the Implant Manager
            Call Me.LoadImplantManagerGroups()
            Call Me.ShowImplantManagerGroups()

            ' Update the group list
            Call Me.LoadImplantGroups()

            ' Set the correct group name
            cboImplantGroup.SelectedItem = implantGroupName

        End If

        ' Dispose of the form
        myGroup = Nothing

    End Sub
    Private Sub LoadImplantGroups()
        cboImplantGroup.BeginUpdate()
        cboImplantGroup.Items.Clear()
        cboImplantGroup.Items.Add("*Custom*")
        For Each iG As ImplantGroup In Implants.implantGroups.Values
            cboImplantGroup.Items.Add(iG.GroupName)
        Next
        cboImplantGroup.EndUpdate()
    End Sub

#End Region

#Region "Implant Manager Routines"
    Private Sub LoadImplantManagerGroups()
        cboImplantGroupsM.Items.Clear()
        cboImplantGroupsM.Items.Add("<All Groups>")
        For Each cImplant As ShipModule In Implants.implantList.Values
            For Each implantGroup As String In cImplant.ImplantGroups
                If cboImplantGroupsM.Items.Contains(implantGroup) = False Then
                    cboImplantGroupsM.Items.Add(implantGroup)
                End If
            Next
        Next
        cboImplantGroupsM.Enabled = False
    End Sub
    Private Sub ShowImplantManagerGroups()
        lstImplantGroups.BeginUpdate()
        lstImplantGroups.Items.Clear()
        For Each iG As ImplantGroup In Implants.implantGroups.Values
            lstImplantGroups.Items.Add(iG.GroupName)
        Next
        lstImplantGroups.EndUpdate()
    End Sub
    Private Sub DrawImplantManagerTree()
        If currentGroup IsNot Nothing Then
            tvwImplantsM.BeginUpdate()
            tvwImplantsM.Nodes.Clear()
            For slot As Integer = 1 To 10
                If currentGroup.ImplantName(slot) = "" Then
                    tvwImplantsM.Nodes.Add("Slot " & slot.ToString, "Slot " & slot.ToString)
                Else
                    tvwImplantsM.Nodes.Add("Slot " & slot.ToString, currentGroup.ImplantName(slot))
                End If
                If cboImplantGroupsM.SelectedItem.ToString = "<All Groups>" Then
                    tvwImplantsM.Nodes("Slot " & slot.ToString).Nodes.Add("No Implant")
                End If
            Next
            For Each cImplant As ShipModule In Implants.implantList.Values
                If cboImplantGroupsM.SelectedItem.ToString = "<All Groups>" Then
                    tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                    ' Check if this is the selected one!
                    If tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                        tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Drawing.Color.LimeGreen
                    End If
                Else
                    If cImplant.ImplantGroups.Contains(cboImplantGroupsM.SelectedItem.ToString) = True Then
                        tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes.Add(cImplant.Name, cImplant.Name)
                        ' Check if this is the selected one!
                        If tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).Text = tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Text Then
                            tvwImplantsM.Nodes("Slot " & cImplant.ImplantSlot.ToString).Nodes(cImplant.Name).ForeColor = Drawing.Color.LimeGreen
                        End If
                    End If
                End If

            Next
            tvwImplantsM.EndUpdate()
        End If
    End Sub
    Private Sub tvwImplantsM_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwImplantsM.NodeMouseClick
        If e.Node.Text <> "No Implant" And e.Node.Text.StartsWith("Slot") = False Then
            Dim implantName As String = e.Node.Text
            Dim cImplant As ShipModule = CType(Implants.implantList.Item(implantName), ShipModule)
            lblImplantDescriptionM.Text = cImplant.Description
        End If
    End Sub
    Private Sub tvwImplantsM_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwImplantsM.NodeMouseDoubleClick
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
            Dim cImplant As ShipModule = CType(Implants.implantList.Item(implantName), ShipModule)
            lblImplantDescriptionM.Text = cImplant.Description
            e.Node.Parent.Text = cImplant.Name
            currentGroup.ImplantName(currentSlot) = cImplant.Name
            ' Set the node colour
            e.Node.ForeColor = Drawing.Color.LimeGreen
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
                        currentGroup.ImplantName(currentSlot) = ""
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
    End Sub
    Private Sub cboImplantGroupsM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboImplantGroupsM.SelectedIndexChanged
        Call DrawImplantManagerTree()
    End Sub
    Private Sub btnCollapseAllM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCollapseAllM.Click
        tvwImplantsM.CollapseAll()
    End Sub
    Private Sub btnAddImplantGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddImplantGroup.Click
        ' Clear the text boxes
        Dim myGroup As New frmModifyImplantGroups
        With myGroup
            .txtGroupName.Text = "" : .txtGroupName.Enabled = True
            .btnAccept.Text = "Add" : .Tag = "Add"
            .Text = "Add New Group"
            .ShowDialog()
        End With
        Call Me.ShowImplantManagerGroups()
        ' Redraw implant groups in the implant selection combobox
        Dim oldImplantGroup As String = cboImplantGroup.SelectedItem.ToString
        Call Me.LoadImplantGroups()
        cboImplantGroup.SelectedItem = oldImplantGroup
        ForceImplantUpdate = True
    End Sub
    Private Sub btnEditImplantGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditImplantGroup.Click
        ' Check for some selection on the listview
        Dim oldImplantGroup As String = cboImplantGroup.SelectedItem.ToString
        If lstImplantGroups.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Group to edit!", "Cannot Edit Implant Group", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lstImplantGroups.Select()
        Else
            Dim myGroup As New frmModifyImplantGroups
            With myGroup
                ' Load the account details into the text boxes
                Dim selGroup As String = lstImplantGroups.SelectedItems(0).ToString
                .txtGroupName.Text = selGroup : .txtGroupName.Tag = selGroup
                .btnAccept.Text = "Edit" : .Tag = "Edit"
                .Text = "Edit '" & selGroup & "' Queue Details"
                .ShowDialog()
            End With
            Call Me.ShowImplantManagerGroups()
            ' Redraw implant groups in the implant selection combobox
            Dim newImplantGroup As String = myGroup.txtGroupName.Tag.ToString
            Call Me.LoadImplantGroups()
            ForceImplantUpdate = True
            If Implants.implantGroups.ContainsKey(oldImplantGroup) = True Then
                cboImplantGroup.SelectedItem = oldImplantGroup
            Else
                cboImplantGroup.SelectedItem = newImplantGroup
            End If
            lblCurrentGroup.Text = "Current Group: " & newImplantGroup
        End If
    End Sub
    Private Sub btnRemoveImplantGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveImplantGroup.Click
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
            If confirm = Windows.Forms.DialogResult.Yes Then
                ' Delete the queue the accounts collection
                Implants.implantGroups.Remove(selGroup)
                Call Me.ShowImplantManagerGroups()
                ' Redraw implant groups in the implant selection combobox
                Call Me.LoadImplantGroups()
                ForceImplantUpdate = True
                If Implants.implantGroups.ContainsKey(oldImplantGroup) = True Then
                    cboImplantGroup.SelectedItem = oldImplantGroup
                Else
                    cboImplantGroup.SelectedItem = "*Custom*"
                End If
            Else
                lstImplantGroups.Select()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub lstImplantGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstImplantGroups.SelectedIndexChanged
        If lstImplantGroups.SelectedItems.Count > 0 Then
            cboImplantGroupsM.Enabled = True
            If cboImplantGroupsM.SelectedItem Is Nothing Then
                cboImplantGroupsM.SelectedIndex = 0
            End If
            currentGroup = CType(Implants.implantGroups(lstImplantGroups.SelectedItem.ToString), ImplantGroup)
            lblCurrentGroup.Text = "Current Group: " & currentGroup.GroupName
            Call DrawImplantManagerTree()
        End If
    End Sub
#End Region

   
   
End Class