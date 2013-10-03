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
Option Strict Off
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports EveHQ.EveData


Public Class frmShowInfo

    Dim oldNodeIndex As Integer = -1
    Dim itemType As Object
    Dim itemName As String = ""
    Dim hPilot As Core.EveHQPilot
    Dim SkillsNeeded As New SortedList(Of String, Integer)
    Dim ItemUsable As Boolean = True

    Public Sub ShowItemDetails(ByVal itemObject As Object, ByVal iPilot As Core.EveHQPilot)

        hPilot = iPilot

        If TypeOf itemObject Is Ship Then
            itemType = CType(itemObject, Ship)
            itemName = CType(itemObject, Ship).Name
            ' Check if a custom ship
            Dim baseID As String = ""
            If CustomHQFClasses.CustomShipIDs.ContainsKey(itemObject.ID) Then
                baseID = ShipLists.shipListKeyName(CustomHQFClasses.CustomShips(itemObject.name).BaseShipName)
            Else
                baseID = itemObject.id
            End If
            picItem.ImageLocation = Core.ImageHandler.GetImageLocation(baseID)
        Else
            If TypeOf itemObject Is ShipModule Then
                itemType = CType(itemObject, ShipModule)
                itemName = CType(itemObject, ShipModule).Name
                If itemType.IsDrone = True Then
                    picItem.ImageLocation = Core.ImageHandler.GetImageLocation(itemObject.ID)
                Else
                    picItem.Image = ImageHandler.IconImage48(itemType.Icon, itemType.MetaType)
                End If
            End If
        End If

        ' Get image from cache 
        Dim imgFilename As String = Path.Combine(Core.HQ.imageCacheFolder, hPilot.ID & ".png")
        If My.Computer.FileSystem.FileExists(imgFilename) = True Then
            pbPilot.ImageLocation = imgFilename
        Else
            pbPilot.Image = My.Resources.noitem
        End If

        ' Alter form header text & Item label
        Me.Text = "Info - " & itemObject.Name
        Me.lblItemName.Text = itemObject.Name

        Call Me.PrepareDescription(itemType)
        Call Me.GenerateSkills(itemType)
        Call Me.ShowAttributes(itemType)
        Call Me.ShowAffects(itemType)
        Call Me.ShowAudit(itemType)

        Me.Show()

    End Sub

    Public Sub GenerateSkills(ByVal itemObject As Object)

        ItemUsable = True
        tvwReqs.Nodes.Clear()
        Dim skillsRequired As Boolean = False

        For Each itemSkill As ItemSkills In itemObject.RequiredSkills.values
            If itemSkill.ID <> 0 Then
                If itemSkill.Level <> 0 Then
                    skillsRequired = True
                    Dim skillID As String = itemSkill.ID
                    Dim strTree As String = ""
                    Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)
                    Dim curSkill As Integer = CInt(skillID)
                    Dim curLevel As Integer = itemSkill.Level
                    Dim curNode As New DevComponents.AdvTree.Node
                    curNode.Style = New DevComponents.DotNetBar.ElementStyle()

                    ' Write the skill we are querying as the first (parent) node
                    curNode.Text = cSkill.Name & " (Level " & curLevel & ")"
                    Dim skillTrained As Boolean = False
                    Dim myLevel As Integer = 0
                    skillTrained = False
                    If Core.HQ.Settings.Pilots.Count > 0 And hPilot.Updated = True Then
                        If hPilot.PilotSkills.ContainsKey(cSkill.Name) Then
                            Dim mySkill As Core.EveHQPilotSkill = hPilot.PilotSkills(cSkill.Name)
                            myLevel = CInt(mySkill.Level)
                            If myLevel >= curLevel Then skillTrained = True
                            If skillTrained = True Then
                                curNode.Style.TextColor = Color.LimeGreen
                                SuperTooltip1.SetSuperTooltip(curNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Already Trained", cSkill.Name, "This skill has already been trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                            Else
                                Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(hPilot, cSkill.Name, curLevel)
                                If planLevel = 0 Then
                                    curNode.Style.TextColor = Color.Red
                                    SuperTooltip1.SetSuperTooltip(curNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill has not been trained to the required level and it is not part of a skill queue.", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                                Else
                                    If planLevel >= curLevel Then
                                        curNode.Style.TextColor = Color.Blue
                                        SuperTooltip1.SetSuperTooltip(curNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained but is in a skill queue to be trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                                    Else
                                        curNode.Style.TextColor = Color.Orange
                                        SuperTooltip1.SetSuperTooltip(curNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained and is in a skill queue but is only planned to be trained to level " & planLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                                    End If
                                End If
                                SkillsNeeded.Add(cSkill.Name, curLevel)
                                ItemUsable = False
                            End If
                        Else
                            Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(hPilot, cSkill.Name, curLevel)
                            If planLevel = 0 Then
                                curNode.Style.TextColor = Color.Red
                                SuperTooltip1.SetSuperTooltip(curNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill has not been trained and it is not part of a skill queue.", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                            Else
                                If planLevel >= curLevel Then
                                    curNode.Style.TextColor = Color.Blue
                                    SuperTooltip1.SetSuperTooltip(curNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained but is in a skill queue to be trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                                Else
                                    curNode.Style.TextColor = Color.Orange
                                    SuperTooltip1.SetSuperTooltip(curNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained and is in a skill queue but is only planned to be trained to level " & planLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                                End If
                            End If
                            SkillsNeeded.Add(cSkill.Name, curLevel)
                            ItemUsable = False
                        End If
                    End If
                    tvwReqs.Nodes.Add(curNode)

                    If cSkill.PreReqSkills.Count > 0 Then
                        Dim subSkill As Core.EveSkill
                        For Each subSkillID As String In cSkill.PreReqSkills.Keys
                            subSkill = Core.HQ.SkillListID(subSkillID)
                            Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                        Next
                    End If
                    tvwReqs.ExpandAll()
                End If
            End If
        Next

        If skillsRequired = True Then
            tvwReqs.ExpandAll()
            If hPilot.Name <> "" Then
                If ItemUsable = True Then
                    lblUsable.Text = hPilot.Name & " has the skills to use this item."
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
                    For Each skill As String In SkillsNeeded.Keys
                        Dim skillName As String = skill.Substring(0, skill.Length - 1)
                        Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                        Dim cSkill As Core.EveSkill = Core.HQ.SkillListName(skillName)
                        usableTime += Core.SkillFunctions.CalcTimeToLevel(hPilot, cSkill, skillLvl)
                    Next
                    lblUsable.Text = hPilot.Name & " doesn't have the skills to use this item."
                    lblUsableTime.Text = "Training Time: " & Core.SkillFunctions.TimeToString(usableTime)
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
    Private Sub AddPreReqsToTree(ByVal newSkill As Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As DevComponents.AdvTree.Node)
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        Dim newNode As New DevComponents.AdvTree.Node
        newNode.Style = New DevComponents.DotNetBar.ElementStyle()
        newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
        newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
        ' Check status of this skill
        If Core.HQ.Settings.Pilots.Count > 0 And hPilot.Updated = True Then
            skillTrained = False
            myLevel = 0
            If hPilot.PilotSkills.ContainsKey(newSkill.Name) Then
                Dim mySkill As Core.EveHQPilotSkill = hPilot.PilotSkills(newSkill.Name)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
            End If
            If skillTrained = True Then
                newNode.Style.TextColor = Color.LimeGreen
                SuperTooltip1.SetSuperTooltip(newNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Already Trained", newSkill.Name, "This skill has already been trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
            Else
                Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(hPilot, newSkill.Name, curLevel)
                If planLevel = 0 Then
                    newNode.Style.TextColor = Color.Red
                    SuperTooltip1.SetSuperTooltip(newNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", newSkill.Name, "This skill has not been trained and it is not part of a skill queue.", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                Else
                    If planLevel >= curLevel Then
                        newNode.Style.TextColor = Color.Blue
                        SuperTooltip1.SetSuperTooltip(newNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", newSkill.Name, "This skill is not trained but is in a skill queue to be trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                    Else
                        newNode.Style.TextColor = Color.Orange
                        SuperTooltip1.SetSuperTooltip(newNode, New DevComponents.DotNetBar.SuperTooltipInfo("Skill Not Trained", newSkill.Name, "This skill is not trained and is in a skill queue but is only planned to be trained to level " & planLevel.ToString & ".", My.Resources.SkillBook64, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                    End If
                End If
                SkillsNeeded.Add(newSkill.Name, curLevel)
                ItemUsable = False
            End If
        End If
        curNode.Nodes.Add(newNode)
        curNode = newNode

        If newSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As Core.EveSkill
            For Each subSkillID As String In newSkill.PreReqSkills.Keys
                If subSkillID <> newSkill.ID Then
                    subSkill = Core.HQ.SkillListID(subSkillID)
                    Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
                End If
            Next
        End If
    End Sub

    Private Sub PrepareDescription(ByVal itemType As Object)
        Dim value As String = CType(itemType.Description, String)

        txtDescription.Text = FormatDescriptionText(value)
    End Sub

    Public Shared Function FormatDescriptionText(ByVal description As String) As String
        Dim value As String = description

        ' adjust bare LFs to be CRLFs
        ' turn already existing CRLFs into LFs first so they don't end up CRCRLFs
        value = value.Replace(vbCrLf, vbLf).Replace(vbLf, vbCrLf)

        'use CRLFs instead of <br> tags 
        value = value.Replace("<br>", vbCrLf)

        ' remove double spaces
        value = value.Replace("  ", " ")

        'remove spaces before CRLFs
        value = value.Replace(" " & vbCrLf, vbCrLf)

        Dim charlist As List(Of Char) = New List(Of Char)
        Dim skip As Boolean
        ' remove any HTML markup/format tags
        For Each letter As Char In value
            If letter = "<" Then
                skip = True
            ElseIf letter = ">" Then
                skip = False
            ElseIf skip = False Then
                charlist.Add(letter)
            End If
        Next

        value = charlist.ToArray()
        Return value
    End Function

    Private Sub tvwReqs_NodeClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles tvwReqs.NodeClick
        tvwReqs.SelectedNode = e.Node
    End Sub

    Private Sub ShowAttributes(ByVal itemObject As Object)
        Dim attGroups(16) As String
        attGroups(0) = "Miscellaneous" : attGroups(1) = "Structure" : attGroups(2) = "Armor" : attGroups(3) = "Shield"
        attGroups(4) = "Capacitor" : attGroups(5) = "Targetting" : attGroups(6) = "Propulsion" : attGroups(7) = "Required Skills"
        attGroups(8) = "Fitting Requirements" : attGroups(9) = "Damage" : attGroups(10) = "Entity Targetting" : attGroups(11) = "Entity Kill"
        attGroups(12) = "Entity EWar" : attGroups(13) = "Usage" : attGroups(14) = "Skill Information" : attGroups(15) = "Blueprint Information"
        attGroups(16) = "Miscellaneous"
        For attGroup As Integer = 0 To 16
            Dim lvGroup As New ListViewGroup
            lvGroup.Name = attGroups(attGroup)
            lvGroup.Header = attGroups(attGroup)
            lvwAttributes.Groups.Add(lvGroup)
        Next
        Dim stdItem As New ShipModule
        Dim stdShip As New Ship
        If TypeOf itemObject Is ShipModule Then
            stdItem = CType(ModuleLists.moduleList(itemObject.ID), ShipModule)
        ElseIf TypeOf itemObject Is Ship Then
            stdShip = CType(ShipLists.shipList(ShipLists.shipListKeyID(itemObject.id)), Ship)
        End If
        lvwAttributes.BeginUpdate()
        lvwAttributes.Items.Clear()
        Dim attData As New Attribute
        Dim itemData As String = ""
        For Each att As String In itemObject.Attributes.Keys
            Dim newItem As New ListViewItem
            attData = Attributes.AttributeList(att)
            If attData.DisplayName <> "" Then
                newItem.Text = attData.DisplayName
            Else
                newItem.Text = attData.Name
            End If
            If CInt(attData.AttributeGroup) = 0 Then
                newItem.Group = lvwAttributes.Groups(16)
            Else
                newItem.Group = lvwAttributes.Groups(CInt(attData.AttributeGroup))
            End If
            If TypeOf itemObject Is ShipModule Then
                Select Case attData.UnitName
                    Case "typeID"
                        If stdItem.Attributes.Item(att).ToString <> "0" Then
                            newItem.SubItems.Add(StaticData.Types(stdItem.Attributes.Item(att)).Name)
                        Else
                            newItem.SubItems.Add("n/a")
                        End If
                    Case "groupID"
                        newItem.SubItems.Add(StaticData.TypeGroups(stdItem.Attributes.Item(att)))
                    Case Else
                        If stdItem.Attributes.ContainsKey(att) = True Then
                            newItem.SubItems.Add(stdItem.Attributes.Item(att) & " " & attData.UnitName)
                        Else
                            newItem.SubItems.Add("n/a")
                        End If
                End Select
            ElseIf TypeOf itemObject Is Ship Then
                Select Case attData.UnitName
                    Case "typeID"
                        If stdShip.Attributes.Item(att).ToString <> "0" Then
                            newItem.SubItems.Add(StaticData.Types(stdShip.Attributes.Item(att)).Name)
                        Else
                            newItem.SubItems.Add("n/a")
                        End If
                    Case "groupID"
                        newItem.SubItems.Add(StaticData.TypeGroups(stdShip.Attributes.Item(att)))
                    Case Else
                        If stdShip.Attributes.ContainsKey(att) = True Then
                            newItem.SubItems.Add(stdShip.Attributes.Item(att) & " " & attData.UnitName)
                        Else
                            newItem.SubItems.Add("n/a")
                        End If
                End Select
            End If
            Select Case attData.UnitName
                Case "typeID"
                    If itemObject.Attributes.Item(att).ToString <> "0" Then
                        newItem.UseItemStyleForSubItems = False
                        itemData = StaticData.Types(itemObject.Attributes.Item(att).ToString).Name
                    Else
                        itemData = "n/a"
                        newItem.SubItems.Add("n/a")
                    End If
                Case "groupID"
                    itemData = StaticData.TypeGroups(itemObject.Attributes.Item(att).ToString)
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

    Private Sub ShowAffects(ByVal itemObject As Object)
        If itemObject.Affects.Count = 0 Then
            tabShowInfo.Tabs.Remove(tabAffects)
        Else
            lvwAffects.BeginUpdate()
            lvwAffects.Items.Clear()
            Dim effects(3) As String
            Dim newEffect As New ListViewItem
            For Each item As String In itemObject.Affects
                newEffect = New ListViewItem
                effects = item.Split(";")
                newEffect.Text = effects(0)
                For subItem As Integer = 1 To 2
                    newEffect.SubItems.Add(effects(subItem))
                Next
                lvwAffects.Items.Add(newEffect)
            Next
            lvwAffects.EndUpdate()
        End If
    End Sub

    Private Sub ShowAudit(ByVal itemObject As Object)
        If itemObject.AuditLog.Count = 0 Then
            tabShowInfo.Tabs.Remove(tabAudit)
        Else
            lvwAudit.BeginUpdate()
            lvwAudit.Items.Clear()
            For Each log As String In itemObject.AuditLog
                lvwAudit.Items.Add(log)
            Next
            lvwAudit.EndUpdate()
        End If
    End Sub

    Private Sub lvwAffects_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwAffects.ColumnClick
        If CInt(lvwAffects.Tag) = e.Column Then
            Me.lvwAffects.ListViewItemSorter = New Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwAffects.Tag = -1
        Else
            Me.lvwAffects.ListViewItemSorter = New Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwAffects.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwAffects.Sort()
    End Sub

    Private Sub lblUsableTime_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblUsableTime.LinkClicked
        Dim selQ As New Core.frmSelectQueue(hPilot.Name, SkillsNeeded, "HQF: " & itemName)
        selQ.TopMost = True
        selQ.ShowDialog()
        Core.SkillQueueFunctions.StartQueueRefresh = True
        Call Me.GenerateSkills(itemType)
    End Sub

End Class