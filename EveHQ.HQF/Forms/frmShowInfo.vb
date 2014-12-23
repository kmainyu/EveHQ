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

Option Strict Off

Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports System.Drawing
Imports EveHQ.EveData
Imports EveHQ.Core
Imports System.Windows.Forms
Imports System.IO
Imports System.Text.RegularExpressions
Imports MarkupLinkClickEventArgs = DevComponents.DotNetBar.MarkupLinkClickEventArgs

Namespace Forms

    Public Class FrmShowInfo
        Dim _itemType As Object
        Dim _itemName As String = ""
        Dim _hPilot As EveHQPilot
        ReadOnly _skillsNeeded As New Dictionary(Of String, Integer)
        Dim _itemUsable As Boolean = True

        Public Sub ShowItemDetails(ByVal itemObject As Object, ByVal iPilot As EveHQPilot)

            _hPilot = iPilot

            If TypeOf itemObject Is Ship Then
                _itemType = CType(itemObject, Ship)
                _itemName = CType(itemObject, Ship).Name
                ' Check if a custom ship
                Dim baseID As String = itemObject.id
                picItem.ImageLocation = Core.ImageHandler.GetImageLocation(baseID)
            Else
                If TypeOf itemObject Is ShipModule Then
                    _itemType = CType(itemObject, ShipModule)
                    _itemName = CType(itemObject, ShipModule).Name
                    If _itemType.IsDrone = True Then
                        picItem.ImageLocation = Core.ImageHandler.GetImageLocation(itemObject.ID)
                    Else
                        picItem.Image = ImageHandler.IconImage48(_itemType.Icon, _itemType.MetaType)
                    End If
                End If
            End If

            ' Get image from cache 
            Dim imgFilename As String = Path.Combine(HQ.ImageCacheFolder, _hPilot.ID & ".png")
            If My.Computer.FileSystem.FileExists(imgFilename) = True Then
                pbPilot.ImageLocation = imgFilename
            Else
                pbPilot.Image = My.Resources.noitem
            End If

            ' Alter form header text & Item label
            Text = "Info - " & itemObject.Name
            lblItemName.Text = itemObject.Name

            Call PrepareTraits(_itemType)
            Call PrepareDescription(_itemType)
            Call GenerateSkills(_itemType)
            Call ShowAttributes(_itemType)
            Call ShowAffects(_itemType)
            Call ShowAudit(_itemType)

            Show()
        End Sub

        Public Sub GenerateSkills(ByVal itemObject As Object)

            _itemUsable = True
            tvwReqs.Nodes.Clear()
            Dim skillsRequired As Boolean = False

            For Each itemSkill As ItemSkills In itemObject.RequiredSkills.values
                If itemSkill.ID <> 0 Then
                    If itemSkill.Level <> 0 Then
                        skillsRequired = True
                        Dim skillID As String = itemSkill.ID
                        Dim cSkill As EveSkill = HQ.SkillListID(skillID)
                        Dim curLevel As Integer = itemSkill.Level
                        Dim curNode As New Node
                        curNode.Style = New ElementStyle()

                        ' Write the skill we are querying as the first (parent) node
                        curNode.Text = cSkill.Name & " (Level " & curLevel & ")"
                        Dim skillTrained As Boolean
                        Dim myLevel As Integer
                        skillTrained = False
                        If HQ.Settings.Pilots.Count > 0 And _hPilot.Updated = True Then
                            If _hPilot.PilotSkills.ContainsKey(cSkill.Name) Then
                                Dim mySkill As EveHQPilotSkill
                                mySkill = _hPilot.PilotSkills(cSkill.Name)
                                myLevel = CInt(mySkill.Level)
                                If myLevel >= curLevel Then skillTrained = True
                                If skillTrained = True Then
                                    curNode.Style.TextColor = Color.LimeGreen
                                    SuperTooltip1.SetSuperTooltip(curNode, New SuperTooltipInfo("Skill Already Trained", cSkill.Name, "This skill has already been trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                                Else
                                    Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_hPilot, cSkill.Name, curLevel)
                                    If planLevel = 0 Then
                                        curNode.Style.TextColor = Color.Red
                                        SuperTooltip1.SetSuperTooltip(curNode, New SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill has not been trained to the required level and it is not part of a skill queue.", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                                    Else
                                        If planLevel >= curLevel Then
                                            curNode.Style.TextColor = Color.Blue
                                            SuperTooltip1.SetSuperTooltip(curNode, New SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained but is in a skill queue to be trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                                        Else
                                            curNode.Style.TextColor = Color.Orange
                                            SuperTooltip1.SetSuperTooltip(curNode, New SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained and is in a skill queue but is only planned to be trained to level " & planLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                                        End If
                                    End If
                                    If _skillsNeeded.ContainsKey(cSkill.Name) = False Then
                                        _skillsNeeded.Add(cSkill.Name, curLevel)
                                    Else
                                        If curLevel > _skillsNeeded(cSkill.Name) Then
                                            _skillsNeeded(cSkill.Name) = curLevel
                                        End If
                                    End If
                                    _itemUsable = False
                                End If
                            Else
                                Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_hPilot, cSkill.Name, curLevel)
                                If planLevel = 0 Then
                                    curNode.Style.TextColor = Color.Red
                                    SuperTooltip1.SetSuperTooltip(curNode, New SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill has not been trained and it is not part of a skill queue.", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                                Else
                                    If planLevel >= curLevel Then
                                        curNode.Style.TextColor = Color.Blue
                                        SuperTooltip1.SetSuperTooltip(curNode, New SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained but is in a skill queue to be trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                                    Else
                                        curNode.Style.TextColor = Color.Orange
                                        SuperTooltip1.SetSuperTooltip(curNode, New SuperTooltipInfo("Skill Not Trained", cSkill.Name, "This skill is not trained and is in a skill queue but is only planned to be trained to level " & planLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                                    End If
                                End If
                                If _skillsNeeded.ContainsKey(cSkill.Name) = False Then
                                    _skillsNeeded.Add(cSkill.Name, curLevel)
                                Else
                                    If curLevel > _skillsNeeded(cSkill.Name) Then
                                        _skillsNeeded(cSkill.Name) = curLevel
                                    End If
                                End If
                                _itemUsable = False
                            End If
                        End If
                        tvwReqs.Nodes.Add(curNode)

                        If cSkill.PreReqSkills.Count > 0 Then
                            Dim subSkill As EveSkill
                            For Each subSkillID As String In cSkill.PreReqSkills.Keys
                                subSkill = HQ.SkillListID(subSkillID)
                                Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                            Next
                        End If
                        tvwReqs.ExpandAll()
                    End If
                End If
            Next

            If skillsRequired = True Then
                tvwReqs.ExpandAll()
                If _hPilot.Name <> "" Then
                    If _itemUsable = True Then
                        lblUsable.Text = _hPilot.Name & " has the skills to use this item."
                        lblUsableTime.Text = ""
                    Else
                        Dim usableTime As Long = 0
                        For Each skillName As String In _skillsNeeded.Keys
                            Dim skillLvl As Integer = _skillsNeeded(skillName)
                            Dim cSkill As EveSkill = HQ.SkillListName(skillName)
                            usableTime += SkillFunctions.CalcTimeToLevel(_hPilot, cSkill, skillLvl)
                        Next
                        lblUsable.Text = _hPilot.Name & " doesn't have the skills to use this item."
                        lblUsableTime.Text = "Training Time: " & SkillFunctions.TimeToString(usableTime)
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

        Private Sub AddPreReqsToTree(ByVal newSkill As EveSkill, ByVal curLevel As Integer, ByVal curNode As Node)
            Dim skillTrained As Boolean
            Dim myLevel As Integer
            Dim newNode As New Node
            newNode.Style = New ElementStyle()
            newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
            newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
            ' Check status of this skill
            If HQ.Settings.Pilots.Count > 0 And _hPilot.Updated = True Then
                skillTrained = False
                If _hPilot.PilotSkills.ContainsKey(newSkill.Name) Then
                    Dim mySkill As EveHQPilotSkill
                    mySkill = _hPilot.PilotSkills(newSkill.Name)
                    myLevel = CInt(mySkill.Level)
                    If myLevel >= curLevel Then skillTrained = True
                End If
                If skillTrained = True Then
                    newNode.Style.TextColor = Color.LimeGreen
                    SuperTooltip1.SetSuperTooltip(newNode, New SuperTooltipInfo("Skill Already Trained", newSkill.Name, "This skill has already been trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                Else
                    Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_hPilot, newSkill.Name, curLevel)
                    If planLevel = 0 Then
                        newNode.Style.TextColor = Color.Red
                        SuperTooltip1.SetSuperTooltip(newNode, New SuperTooltipInfo("Skill Not Trained", newSkill.Name, "This skill has not been trained and it is not part of a skill queue.", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                    Else
                        If planLevel >= curLevel Then
                            newNode.Style.TextColor = Color.Blue
                            SuperTooltip1.SetSuperTooltip(newNode, New SuperTooltipInfo("Skill Not Trained", newSkill.Name, "This skill is not trained but is in a skill queue to be trained to the required level of " & curLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                        Else
                            newNode.Style.TextColor = Color.Orange
                            SuperTooltip1.SetSuperTooltip(newNode, New SuperTooltipInfo("Skill Not Trained", newSkill.Name, "This skill is not trained and is in a skill queue but is only planned to be trained to level " & planLevel.ToString & ".", My.Resources.SkillBook64, Nothing, eTooltipColor.Yellow))
                        End If
                    End If
                    If _skillsNeeded.ContainsKey(newSkill.Name) = False Then
                        _skillsNeeded.Add(newSkill.Name, curLevel)
                    Else
                        If curLevel > _skillsNeeded(newSkill.Name) Then
                            _skillsNeeded(newSkill.Name) = curLevel
                        End If
                    End If
                    _itemUsable = False
                End If
            End If
            curNode.Nodes.Add(newNode)

            If newSkill.PreReqSkills.Count > 0 Then
                Dim subSkill As EveSkill
                For Each subSkillID As String In newSkill.PreReqSkills.Keys
                    subSkill = HQ.SkillListID(subSkillID)
                    Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
                Next
            End If
        End Sub

        Public Shared Function ComposeTraits(ByVal shipID As Integer) As String

            Dim traits As String = ""
            If StaticData.Traits.ContainsKey(shipID) Then
                For Each skillTraitList In StaticData.Traits(shipID)
                    Dim skillID As Integer = skillTraitList.Key
                    Select Case skillID
                        Case -2
                            traits &= "<b>Misc Bonus:</b>" & vbCrLf
                        Case -1
                            traits &= "<b>Role Bonus:</b>" & vbCrLf
                        Case Else
                            If StaticData.Types.ContainsKey(skillID) Then
                                traits &= "<b><a href=showinfo:" & skillID & ">" & StaticData.Types(skillID).Name & "</a> bonuses (per skill level):</b>" & vbCrLf
                            Else
                                Continue For
                            End If
                    End Select
                    For Each bonus In skillTraitList.Value
                        traits &= "    " & bonus & vbCrLf
                    Next
                    traits &= vbCrLf
                Next
            End If
            Return traits

        End Function

        Private Sub PrepareTraits(ByVal itemObject As Object)

            Dim traits As String = ""
            If TypeOf (itemObject) Is Ship Then
                traits = ComposeTraits(CType(itemObject, Ship).ID)
            End If

            If traits = "" Then
                tabShowInfo.Tabs.Remove(tabTraits)
                Exit Sub
            End If

            ' Need to replace the CRLF with HTML to display correctly in all cases
            traits = traits.Replace(ControlChars.CrLf, "<br />").Replace("<br>", "<br />").Replace("</b></u>", "</u></b>")

            ' Identify internal CCP "showinfo" links and replace with something that we can actually use internally for EveHQ :)
            Dim matches As MatchCollection = Regex.Matches(traits, "<a\shref=(?<url>.*?)>(?<text>.*?)</a>")
            For Each m As Match In matches
                Dim typeID As String = m.Groups("url").Value.TrimStart("<showinfo:".ToCharArray)
                traits = traits.Replace(m.Value, "<a href='http://" & typeID & "'>" & m.Groups("text").Value & "</a>")
            Next
            lblTraits.Text = traits

        End Sub

        Private Sub PrepareDescription(ByVal item As Object)

            Dim description As String
            If TypeOf (item) Is ShipModule Then
                description = CType(item, ShipModule).Description
            Else
                description = CType(item, Ship).Description
            End If

            ' Need to replace the CRLF with HTML to display correctly in all cases
            description = description.Replace(ControlChars.CrLf, "<br />").Replace("<br>", "<br />")

            ' Identify internal CCP "showinfo" links and replace with something that we can actually use internally for EveHQ :)
            Dim matches As MatchCollection = Regex.Matches(description, "<a\shref=(?<url>.*?)>(?<text>.*?)</a>")
            For Each m As Match In matches
                Dim typeID As String = m.Groups("url").Value.TrimStart("<showinfo:".ToCharArray)
                description = description.Replace(m.Value, "<a href='http://" & typeID & "'>" & m.Groups("text").Value & "</a>")
            Next
            lblDescription.Text = description

        End Sub

        Private Sub tvwReqs_NodeClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles tvwReqs.NodeClick
            tvwReqs.SelectedNode = e.Node
        End Sub

        Private Sub ShowAttributes(ByVal itemObject As Object)
            Dim attGroups(16) As String
            attGroups(0) = "Miscellaneous"
            attGroups(1) = "Structure"
            attGroups(2) = "Armor"
            attGroups(3) = "Shield"
            attGroups(4) = "Capacitor"
            attGroups(5) = "Targetting"
            attGroups(6) = "Propulsion"
            attGroups(7) = "Required Skills"
            attGroups(8) = "Fitting Requirements"
            attGroups(9) = "Damage"
            attGroups(10) = "Entity Targetting"
            attGroups(11) = "Entity Kill"
            attGroups(12) = "Entity EWar"
            attGroups(13) = "Usage"
            attGroups(14) = "Skill Information"
            attGroups(15) = "Blueprint Information"
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
                stdItem = ModuleLists.ModuleList(itemObject.ID)
            ElseIf TypeOf itemObject Is Ship Then
                stdShip = ShipLists.ShipList(ShipLists.ShipListKeyID(itemObject.id))
            End If
            lvwAttributes.BeginUpdate()
            lvwAttributes.Items.Clear()
            Dim attData As Attribute
            Dim itemData As String
            For Each att As Integer In itemObject.Attributes.Keys
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
                                If EveData.StaticData.Types.ContainsKey(CInt(stdItem.Attributes.Item(att))) Then
                                    newItem.SubItems.Add(EveData.StaticData.Types(CInt(stdItem.Attributes.Item(att))).Name)
                                Else
                                    newItem.SubItems.Add("Unknown Item!")
                                End If
                            Else
                                newItem.SubItems.Add("n/a")
                            End If
                        Case "groupID"
                            newItem.SubItems.Add(ModuleLists.TypeGroups(CInt(stdItem.Attributes.Item(att))))
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
                                If EveData.StaticData.Types.ContainsKey(CInt(stdShip.Attributes.Item(att))) Then
                                    newItem.SubItems.Add(EveData.StaticData.Types(CInt(stdShip.Attributes.Item(att))).Name)
                                Else
                                    newItem.SubItems.Add("Unknown Item!")
                                End If
                            Else
                                newItem.SubItems.Add("n/a")
                            End If
                        Case "groupID"
                            newItem.SubItems.Add(ModuleLists.TypeGroups(CInt(stdShip.Attributes.Item(att))))
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
                            If EveData.StaticData.Types.ContainsKey(CInt(itemObject.Attributes.Item(att))) Then
                                itemData = EveData.StaticData.Types(CInt(itemObject.Attributes.Item(att))).Name
                            Else
                                itemData = "Unknown Item!"
                            End If
                        Else
                            itemData = "n/a"
                        End If
                        newItem.SubItems.Add(itemData)
                    Case "groupID"
                        itemData = ModuleLists.TypeGroups(CInt(itemObject.Attributes.Item(att)))
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
                adtAffects.BeginUpdate()
                adtAffects.Nodes.Clear()
                Dim effects(3) As String
                Dim newEffect As Node
                For Each item As String In itemObject.Affects
                    newEffect = New Node
                    effects = item.Split(";")
                    newEffect.Text = effects(0)
                    For subItem As Integer = 1 To 2
                        newEffect.Cells.Add(New Cell(effects(subItem)))
                    Next
                    adtAffects.Nodes.Add(newEffect)
                Next
                adtAffects.EndUpdate()
            End If
        End Sub

        Private Sub ShowAudit(ByVal itemObject As Object)
            If itemObject.AuditLog.Count = 0 Then
                tabShowInfo.Tabs.Remove(tabAudit)
            Else
                adtAudit.BeginUpdate()
                adtAudit.Nodes.Clear()
                For Each log As String In itemObject.AuditLog
                    adtAudit.Nodes.Add(New Node(log))
                Next
                adtAudit.EndUpdate()
            End If
        End Sub

        Private Sub lblUsableTime_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblUsableTime.LinkClicked
            Using selQ As New FrmSelectQueue(_hPilot.Name, _skillsNeeded, "HQF: " & _itemName)
                selQ.TopMost = True
                selQ.ShowDialog()
                SkillQueueFunctions.StartQueueRefresh = True
                Call GenerateSkills(_itemType)
            End Using
        End Sub

        Private Sub lblDescription_MarkupLinkClick(sender As Object, e As MarkupLinkClickEventArgs) Handles lblDescription.MarkupLinkClick
            Dim typeID As String = e.HRef.TrimStart("http://".ToCharArray)
            Using myIB As New ItemBrowser.FrmIB(typeID)
                myIB.ShowDialog()
            End Using
        End Sub

        Private Sub lblTraits_MarkupLinkClick(sender As Object, e As MarkupLinkClickEventArgs) Handles lblTraits.MarkupLinkClick
            Dim typeID As String = e.HRef.TrimStart("http://".ToCharArray)
            Using myIB As New ItemBrowser.FrmIB(typeID)
                myIB.ShowDialog()
            End Using
        End Sub

        Private Sub adtAffects_ColumnHeaderMouseDown(sender As Object, e As MouseEventArgs) Handles adtAffects.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub
    End Class

End Namespace