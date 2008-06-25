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
Option Strict Off
Imports System.Windows.Forms
Imports System.Drawing


Public Class frmShowInfo

    Dim oldNodeIndex As Integer = -1
    Dim itemType As Object

    Public Sub ShowItemDetails(ByVal itemObject As Object)

        If TypeOf itemObject Is Ship Then
            itemType = CType(itemObject, Ship)
            picItem.ImageLocation = "http://www.eve-online.com/bitmaps/icons/itemdb/shiptypes/128_128/" & itemObject.ID & ".png"
        Else
            If TypeOf itemObject Is ShipModule Then
                itemType = CType(itemObject, ShipModule)
                picItem.ImageLocation = "http://www.eve-online.com/bitmaps/icons/itemdb/black/64_64/icon" & itemObject.Icon & ".png"
            Else
                If TypeOf itemObject Is Implant Then
                    itemType = CType(itemObject, Implant)
                    picItem.ImageLocation = "http://www.eve-online.com/bitmaps/icons/itemdb/black/64_64/icon" & itemObject.Icon & ".png"
                Else
                    ' Insert remaining item types here
                End If
            End If
        End If

        ' Get image from cache 
        Dim imgFilename As String = EveHQ.Core.HQ.cacheFolder & "\i" & EveHQ.Core.HQ.myPilot.ID & ".png"
        If My.Computer.FileSystem.FileExists(imgFilename) = True Then
            pbPilot.ImageLocation = imgFilename
        Else
            pbPilot.Image = My.Resources.noitem
        End If

        ' Alter form header text & Item label
        Me.Text = "Show Info - " & itemObject.Name
        Me.lblItemName.Text = itemObject.Name

        Call Me.PrepareDescription(itemType)
        Call Me.GenerateSkills(itemType)
        Call Me.ShowAttributes(itemType)
        Call Me.ShowAudit(itemType)

        Me.ShowDialog()

    End Sub

    Private Sub PrepareTree(ByVal skillID As String)
        tvwReqs.Nodes.Clear()

        Dim level As Integer = 1
        Dim pointer(20) As Integer
        Dim parent(20) As Integer
        Dim skillName(20) As String
        Dim skillLevel(20) As String
        pointer(level) = 1
        parent(level) = CInt(skillID)

        Dim strTree As String = ""
        Dim cSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(skillID), Core.SkillList)
        Dim curSkill As Integer = CInt(skillID)
        Dim curLevel As Integer = 0
        Dim counter As Integer = 0
        Dim curNode As TreeNode = New TreeNode

        ' Write the skill we are querying as the first (parent) node
        curNode.Text = cSkill.Name '& " (Level " & curLevel & ")"
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        skillTrained = False
        If EveHQ.Core.HQ.Pilots.Count > 0 And EveHQ.Core.HQ.myPilot.Updated = True Then
            If EveHQ.Core.HQ.myPilot.PilotSkills.Contains(cSkill.Name) Then
                Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
                mySkill = CType(EveHQ.Core.HQ.myPilot.PilotSkills(cSkill.Name), Core.Skills)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
                If skillTrained = True Then
                    curNode.ForeColor = Color.LimeGreen
                    curNode.ToolTipText = "Already Trained"
                Else
                    Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(EveHQ.Core.HQ.myPilot, cSkill.Name, curLevel)
                    If planLevel = 0 Then
                        curNode.ForeColor = Color.Red
                        curNode.ToolTipText = "Not trained & no planned training"
                    Else
                        curNode.ToolTipText = "Planned training to Level " & planLevel
                        If planLevel >= curLevel Then
                            curNode.ForeColor = Color.Blue
                        Else
                            curNode.ForeColor = Color.Orange
                        End If
                    End If
                End If
            Else
                Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(EveHQ.Core.HQ.myPilot, cSkill.Name, curLevel)
                If planLevel = 0 Then
                    curNode.ForeColor = Color.Red
                    curNode.ToolTipText = "Not trained & no planned training"
                Else
                    curNode.ToolTipText = "Planned training to Level " & planLevel
                    If planLevel >= curLevel Then
                        curNode.ForeColor = Color.Blue
                    Else
                        curNode.ForeColor = Color.Orange
                    End If
                End If
            End If
        End If
        tvwReqs.Nodes.Add(curNode)

        Do Until level = 0
            ' Start @ root!
            cSkill = CType(EveHQ.Core.HQ.SkillListID(CStr(curSkill)), Core.SkillList)

            ' Read pointer @ level
            Select Case pointer(level)
                Case 1
                    If CDbl(cSkill.PS) = curSkill Then Exit Do
                    pointer(level) = 2
                    curSkill = CInt(cSkill.PS)
                    curLevel = cSkill.PSL
                Case 2
                    If CDbl(cSkill.SS) = curSkill Then Exit Do
                    pointer(level) = 3
                    curSkill = CInt(cSkill.SS)
                    curLevel = cSkill.SSL
                Case 3
                    If CDbl(cSkill.TS) = curSkill Then Exit Do
                    pointer(level) = 4
                    curSkill = CInt(cSkill.TS)
                    curLevel = cSkill.TSL
                Case 4
                    curSkill = 0
            End Select
            If curSkill = 0 Then
                level -= 1
                curSkill = parent(level)
                curNode = curNode.Parent
            Else
                level += 1
                parent(level) = curSkill
                pointer(level) = 1
                Dim newSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
                newSkill = CType(EveHQ.Core.HQ.SkillListID(CStr(curSkill)), Core.SkillList)
                skillName(level) = newSkill.Name
                skillLevel(level) = CStr(curLevel)
                Dim newNode As TreeNode = New TreeNode
                counter += 1
                newNode.Name = CStr(counter)
                newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
                ' Check if the current pilot has the skill
                If EveHQ.Core.HQ.Pilots.Count > 0 And EveHQ.Core.HQ.myPilot.Updated = True Then
                    skillTrained = False
                    myLevel = 0
                    If EveHQ.Core.HQ.myPilot.PilotSkills.Contains(newSkill.Name) Then
                        Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
                        mySkill = CType(EveHQ.Core.HQ.myPilot.PilotSkills(newSkill.Name), Core.Skills)
                        myLevel = CInt(mySkill.Level)
                        If myLevel >= curLevel Then skillTrained = True
                    End If
                    If skillTrained = True Then
                        newNode.ForeColor = Color.LimeGreen
                        newNode.ToolTipText = "Already Trained"
                    Else
                        Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(EveHQ.Core.HQ.myPilot, newSkill.Name, curLevel)
                        If planLevel = 0 Then
                            newNode.ForeColor = Color.Red
                            newNode.ToolTipText = "Not trained & no planned training"
                        Else
                            newNode.ToolTipText = "Planned training to Level " & planLevel
                            If planLevel >= curLevel Then
                                newNode.ForeColor = Color.Blue
                            Else
                                newNode.ForeColor = Color.Orange
                            End If
                        End If
                    End If
                End If
                curNode.Nodes.Add(newNode)
                curNode = newNode
            End If
        Loop
        tvwReqs.ExpandAll()
    End Sub

    Private Sub PrepareDescription(ByVal itemType As Object)
        Me.lblDescription.Text = itemType.Description
    End Sub

    Private Sub tvwReqs_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwReqs.MouseMove
        Dim tn As TreeNode = tvwReqs.GetNodeAt(e.X, e.Y)
        If Not (tn Is Nothing) Then
            Dim currentNodeIndex As Integer = tn.Index
            If currentNodeIndex <> oldNodeIndex Then
                oldNodeIndex = currentNodeIndex
                If Not (Me.SkillToolTip Is Nothing) And Me.SkillToolTip.Active Then
                    Me.SkillToolTip.Active = False 'turn it off 
                End If
                Me.SkillToolTip.SetToolTip(tvwReqs, tn.ToolTipText)
                Me.SkillToolTip.Active = True 'make it active so it can show 
            End If
        End If
    End Sub

    Private Sub tvwReqs_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwReqs.NodeMouseClick
        tvwReqs.SelectedNode = e.Node
    End Sub

    Private Sub mnuViewSkillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSkillDetails.Click
        Dim skillID As String
        skillID = CStr(mnuSkillName.Tag)
        'Call Me.PrepareDetails(skillID)
        Call Me.PrepareTree(skillID)
        'Call Me.PrepareDepends(skillID)
        Call Me.PrepareDescription(skillID)
    End Sub

    Private Sub ctxReqs_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxReqs.Opening
        Dim curNode As TreeNode = New TreeNode
        curNode = tvwReqs.SelectedNode
        Dim skillName As String = ""
        Dim skillID As String = ""
        skillName = curNode.Text
        If InStr(skillName, "(Level") <> 0 Then
            skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
        End If
        skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
        mnuSkillName.Text = skillName
        mnuSkillName.Tag = skillID
    End Sub

    Private Sub mnuViewItemDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetails.Click
        Dim skillID As String
        skillID = CStr(mnuItemName.Tag)
        'Call Me.PrepareDetails(skillID)
        Call Me.PrepareTree(skillID)
        'Call Me.PrepareDepends(skillID)
        Call Me.PrepareDescription(skillID)
    End Sub

    Public Sub GenerateSkills(ByVal itemObject As Object)

        Dim ItemUsable As Boolean = True
        Dim skillsNeeded As New ArrayList

        tvwReqs.Nodes.Clear()
        Dim skillsRequired As Boolean = False

        For Each itemSkill As ItemSkills In itemObject.RequiredSkills.values
            If itemSkill.ID <> 0 Then
                If itemSkill.Level <> 0 Then
                    skillsRequired = True
                    Dim skillID As String = itemSkill.ID
                    Dim level As Integer = 1
                    Dim pointer(20) As Integer
                    Dim parent(20) As Integer
                    Dim skillName(20) As String
                    Dim skillLevel(20) As String
                    pointer(level) = 1
                    parent(level) = CInt(skillID)

                    Dim strTree As String = ""
                    Dim cSkill As EveHQ.Core.SkillList = EveHQ.Core.HQ.SkillListID(skillID)
                    Dim curSkill As Integer = CInt(skillID)
                    Dim curLevel As Integer = itemSkill.Level
                    Dim counter As Integer = 0
                    Dim curNode As TreeNode = New TreeNode

                    ' Write the skill we are querying as the first (parent) node
                    curNode.Text = cSkill.Name & " (Level " & curLevel & ")"
                    Dim skillTrained As Boolean = False
                    Dim myLevel As Integer = 0
                    skillTrained = False
                    If EveHQ.Core.HQ.Pilots.Count > 0 And EveHQ.Core.HQ.myPilot.Updated = True Then
                        If EveHQ.Core.HQ.myPilot.PilotSkills.Contains(cSkill.Name) Then
                            Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
                            mySkill = EveHQ.Core.HQ.myPilot.PilotSkills(cSkill.Name)
                            myLevel = CInt(mySkill.Level)
                            If myLevel >= curLevel Then skillTrained = True
                            If skillTrained = True Then
                                curNode.ForeColor = Color.LimeGreen
                                curNode.ToolTipText = "Already Trained"
                            Else
                                Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(EveHQ.Core.HQ.myPilot, cSkill.Name, curLevel)
                                If planLevel = 0 Then
                                    curNode.ForeColor = Color.Red
                                    curNode.ToolTipText = "Not trained & no planned training"
                                Else
                                    curNode.ToolTipText = "Planned training to Level " & planLevel
                                    If planLevel >= curLevel Then
                                        curNode.ForeColor = Color.Blue
                                    Else
                                        curNode.ForeColor = Color.Orange
                                    End If
                                End If
                                skillsNeeded.Add(cSkill.Name & curLevel)
                                ItemUsable = False
                            End If
                        Else
                            Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(EveHQ.Core.HQ.myPilot, cSkill.Name, curLevel)
                            If planLevel = 0 Then
                                curNode.ForeColor = Color.Red
                                curNode.ToolTipText = "Not trained & no planned training"
                            Else
                                curNode.ToolTipText = "Planned training to Level " & planLevel
                                If planLevel >= curLevel Then
                                    curNode.ForeColor = Color.Blue
                                Else
                                    curNode.ForeColor = Color.Orange
                                End If
                            End If
                            skillsNeeded.Add(cSkill.Name & curLevel)
                            ItemUsable = False
                        End If
                    End If
                    tvwReqs.Nodes.Add(curNode)

                    Do Until level = 0
                        ' Start @ root!
                        cSkill = CType(EveHQ.Core.HQ.SkillListID(CStr(curSkill)), Core.SkillList)

                        ' Read pointer @ level
                        Select Case pointer(level)
                            Case 1
                                If CDbl(cSkill.PS) = curSkill Then Exit Do
                                pointer(level) = 2
                                curSkill = CInt(cSkill.PS)
                                curLevel = cSkill.PSL
                            Case 2
                                If CDbl(cSkill.SS) = curSkill Then Exit Do
                                pointer(level) = 3
                                curSkill = CInt(cSkill.SS)
                                curLevel = cSkill.SSL
                            Case 3
                                If CDbl(cSkill.TS) = curSkill Then Exit Do
                                pointer(level) = 4
                                curSkill = CInt(cSkill.TS)
                                curLevel = cSkill.TSL
                            Case 4
                                curSkill = 0
                        End Select
                        If curSkill = 0 Then
                            level -= 1
                            curSkill = parent(level)
                            curNode = curNode.Parent
                        Else
                            level += 1
                            parent(level) = curSkill
                            pointer(level) = 1
                            Dim newSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
                            newSkill = CType(EveHQ.Core.HQ.SkillListID(CStr(curSkill)), Core.SkillList)
                            skillName(level) = newSkill.Name
                            skillLevel(level) = curLevel
                            Dim newNode As TreeNode = New TreeNode
                            counter += 1
                            newNode.Name = CStr(counter)
                            newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
                            ' Check if the current pilot has the skill
                            If EveHQ.Core.HQ.Pilots.Count > 0 And EveHQ.Core.HQ.myPilot.Updated = True Then
                                skillTrained = False
                                myLevel = 0
                                If EveHQ.Core.HQ.myPilot.PilotSkills.Contains(newSkill.Name) Then
                                    Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
                                    mySkill = CType(EveHQ.Core.HQ.myPilot.PilotSkills(newSkill.Name), Core.Skills)
                                    myLevel = CInt(mySkill.Level)
                                    If myLevel >= curLevel Then skillTrained = True
                                End If
                                If skillTrained = True Then
                                    newNode.ForeColor = Color.LimeGreen
                                    newNode.ToolTipText = "Already Trained"
                                Else
                                    Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(EveHQ.Core.HQ.myPilot, newSkill.Name, curLevel)
                                    If planLevel = 0 Then
                                        newNode.ForeColor = Color.Red
                                        newNode.ToolTipText = "Not trained & no planned training"
                                    Else
                                        newNode.ToolTipText = "Planned training to Level " & planLevel
                                        If planLevel >= curLevel Then
                                            newNode.ForeColor = Color.Blue
                                        Else
                                            newNode.ForeColor = Color.Orange
                                        End If
                                    End If
                                    skillsNeeded.Add(newSkill.Name & curLevel)
                                    ItemUsable = False
                                End If
                            End If
                            curNode.Nodes.Add(newNode)
                            curNode = newNode
                        End If
                    Loop
                End If
            End If
        Next

        If skillsRequired = True Then
            tvwReqs.ExpandAll()
            If EveHQ.Core.HQ.myPilot.Name <> "" Then
                If ItemUsable = True Then
                    lblUsable.Text = EveHQ.Core.HQ.myPilot.Name & " has the skills to use this item."
                    lblUsableTime.Text = ""
                Else
                    Dim usableTime As Long = 0
                    Dim skillNo As Integer = 0
                    If skillsNeeded.Count > 1 Then
                        Do
                            Dim skill As String = skillsNeeded(skillNo)
                            Dim skillName As String = skill.Substring(0, skill.Length - 1)
                            Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                            Dim highestLevel As Integer = 0
                            Dim skillno2 As Integer = skillNo + 1
                            Do
                                If skillno2 < skillsNeeded.Count Then
                                    Dim skill2 As String = skillsNeeded(skillno2)
                                    Dim skillName2 As String = skill2.Substring(0, skill2.Length - 1)
                                    Dim skillLvl2 As Integer = CInt(skill2.Substring(skill2.Length - 1, 1))
                                    If skillName = skillName2 Then
                                        If skillLvl >= skillLvl2 Then
                                            skillsNeeded.RemoveAt(skillno2)
                                        Else
                                            skillsNeeded.RemoveAt(skillNo)
                                            skillNo = -1 : skillno2 = 0
                                            Exit Do
                                        End If
                                    Else
                                        skillno2 += 1
                                    End If
                                End If
                            Loop Until skillno2 >= skillsNeeded.Count
                            skillNo += 1
                        Loop Until skillNo >= skillsNeeded.Count - 1
                    End If
                    skillsNeeded.Reverse()
                    For Each skill As String In skillsNeeded
                        Dim skillName As String = skill.Substring(0, skill.Length - 1)
                        Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                        Dim cSkill As EveHQ.Core.SkillList = EveHQ.Core.HQ.SkillListName(skillName)
                        usableTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, cSkill, skillLvl)
                    Next
                    lblUsable.Text = EveHQ.Core.HQ.myPilot.Name & " doesn't have the skills to use this item."
                    lblUsableTime.Text = "Training Time: " & EveHQ.Core.SkillFunctions.TimeToString(usableTime)
                End If
            Else
                lblUsable.Text = "No pilot selected to calculate skill time."
                lblUsableTime.Text = ""
            End If
        Else
            lblUsable.Text = "No skills required for this item."
            lblUsableTime.Text = ""
        End If

    End Sub

    Private Sub ShowAttributes(ByVal itemObject As Object)
        Dim attGroups(15) As String
        attGroups(0) = "Miscellaneous" : attGroups(1) = "Structure" : attGroups(2) = "Armor" : attGroups(3) = "Shield"
        attGroups(4) = "Capacitor" : attGroups(5) = "Targetting" : attGroups(6) = "Propulsion" : attGroups(7) = "Required Skills"
        attGroups(8) = "Fitting Requirements" : attGroups(9) = "Damage" : attGroups(10) = "Entity Targetting" : attGroups(11) = "Entity Kill"
        attGroups(12) = "Entity EWar" : attGroups(13) = "Usage" : attGroups(14) = "Skill Information" : attGroups(15) = "Blueprint Information"
        For attGroup As Integer = 0 To 15
            Dim lvGroup As New ListViewGroup
            lvGroup.Name = attGroups(attGroup)
            lvGroup.Header = attGroups(attGroup)
            lvwAttributes.Groups.Add(lvGroup)
        Next
        Dim stdItem As New ShipModule
        If TypeOf itemObject Is ShipModule Then
            stdItem = CType(ModuleLists.moduleList(itemObject.ID), ShipModule)
        End If
        lvwAttributes.BeginUpdate()
        lvwAttributes.Items.Clear()
        Dim attData As New Attribute
        Dim idx As Integer
        Dim itemData As String = ""
        For Each att As String In itemObject.Attributes.Keys
            Dim newItem As New ListViewItem
            attData = Attributes.AttributeList(att)
            If attData.DisplayName <> "" Then
                newItem.Text = attData.DisplayName
            Else
                newItem.Text = attData.Name
            End If
            newItem.Group = lvwAttributes.Groups(CInt(attData.AttributeGroup))
            Select Case attData.UnitName
                Case "typeID"
                    If stdItem.Attributes.Item(att).ToString <> "0" Then
                        idx = EveHQ.Core.HQ.itemList.IndexOfValue(stdItem.Attributes.Item(att).ToString)
                        newItem.SubItems.Add(EveHQ.Core.HQ.itemList.GetKey(idx))
                    Else
                        newItem.SubItems.Add("n/a")
                    End If
                Case "groupID"
                    idx = EveHQ.Core.HQ.groupList.IndexOfValue(stdItem.Attributes.Item(att).ToString)
                    newItem.SubItems.Add(EveHQ.Core.HQ.groupList.GetKey(idx))
                Case Else
                    newItem.SubItems.Add(stdItem.Attributes.Item(att) & " " & attData.UnitName)
            End Select
            Select Case attData.UnitName
                Case "typeID"
                    If itemObject.Attributes.Item(att).ToString <> "0" Then
                        newItem.UseItemStyleForSubItems = False
                        idx = EveHQ.Core.HQ.itemList.IndexOfValue(itemObject.Attributes.Item(att).ToString)
                        itemData = EveHQ.Core.HQ.itemList.GetKey(idx)
                    Else
                        itemData = "n/a"
                        newItem.SubItems.Add("n/a")
                    End If
                Case "groupID"
                    idx = EveHQ.Core.HQ.groupList.IndexOfValue(itemObject.Attributes.Item(att).ToString)
                    itemData = EveHQ.Core.HQ.groupList.GetKey(idx)
                Case Else
                    itemData = itemObject.Attributes.Item(att) & " " & attData.UnitName
            End Select
            If itemData.Trim = newItem.SubItems(1).Text.Trim Then
                newItem.SubItems.Add(itemData)
            Else
                newItem.UseItemStyleForSubItems = False
                newItem.SubItems.Add(itemData, Color.Black, Color.LightSteelBlue, lvwAttributes.Font)
            End If
            lvwAttributes.Items.Add(newItem)
        Next
        lvwAttributes.EndUpdate()
    End Sub

    Private Sub ShowAudit(ByVal itemObject As Object)
        If itemObject.AuditLog.Count = 0 Then
            tabShowInfo.TabPages.Remove(tabAudit)
        Else
            lvwAudit.BeginUpdate()
            lvwAudit.Items.Clear()
            For Each log As String In itemObject.AuditLog
                lvwAudit.Items.Add(log)
            Next
            lvwAudit.EndUpdate()
        End If
    End Sub

End Class