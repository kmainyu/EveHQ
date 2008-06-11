' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2008  Lee Vessey
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
Public Class frmSkillDetails

    Dim oldNodeIndex As Integer = -1

    Public Sub ShowSkillDetails(ByVal skillID As String)

        Call Me.PrepareDetails(skillID)
        Call Me.PrepareTree(skillID)
        Call Me.PrepareDepends(skillID)
        Call Me.PrepareDescription(skillID)
        Call Me.PrepareSPs(skillID)
        Call Me.PrepareTimes(skillID)

        Me.ShowDialog()

    End Sub

    Private Sub PrepareDetails(ByVal skillID As String)

        Dim cSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
        cSkill = CType(EveHQ.Core.HQ.SkillListID(skillID), Core.SkillList)

        Me.Text = "Skill Details - " & cSkill.Name
        lvwDetails.Groups(1).Header = "Pilot Specific - " & EveHQ.Core.HQ.myPilot.Name

        With Me.lvwDetails
            Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
            Dim myGroup As EveHQ.Core.SkillGroup = New EveHQ.Core.SkillGroup
            myGroup = CType(EveHQ.Core.HQ.SkillGroups(cSkill.GroupID), Core.SkillGroup)
            Dim cLevel, cSP, cTime, cRate As String
            If EveHQ.Core.HQ.Pilots.Count > 0 And EveHQ.Core.HQ.myPilot.Updated = True Then
                If EveHQ.Core.HQ.myPilot.PilotSkills.Contains(cSkill.Name) = False Then
                    cLevel = "0" : cSP = "0" : cTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, cSkill, 1))
                    cRate = CStr(EveHQ.Core.SkillFunctions.CalculateSPRate(EveHQ.Core.HQ.myPilot, cSkill))
                Else
                    mySkill = CType(EveHQ.Core.HQ.myPilot.PilotSkills(cSkill.Name), Core.Skills)
                    cLevel = CStr(mySkill.Level)
                    If EveHQ.Core.HQ.myPilot.Training = True And EveHQ.Core.HQ.myPilot.TrainingSkillID = EveHQ.Core.SkillFunctions.SkillNameToID(cSkill.Name) Then
                        cSP = CStr(mySkill.SP + EveHQ.Core.HQ.myPilot.TrainingCurrentSP)
                    Else
                        cSP = CStr(mySkill.SP)
                    End If
                    If EveHQ.Core.HQ.myPilot.Training = True And EveHQ.Core.HQ.myPilot.TrainingSkillName = cSkill.Name Then
                        cTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.HQ.myPilot.TrainingCurrentTime)
                    Else
                        cTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, cSkill, 0))
                    End If
                    cRate = CStr(EveHQ.Core.SkillFunctions.CalculateSPRate(EveHQ.Core.HQ.myPilot, cSkill))
                End If
            Else
                cLevel = "n/a" : cSP = "0" : cTime = "n/a" : cRate = "0"
            End If

            .Items(0).SubItems(1).Text = (cSkill.Name)
            .Items(1).SubItems(1).Text = CStr((cSkill.Rank))
            .Items(2).SubItems(1).Text = (myGroup.Name)
            .Items(3).SubItems(1).Text = FormatNumber(cSkill.BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            .Items(4).SubItems(1).Text = (cSkill.PA)
            .Items(5).SubItems(1).Text = (cSkill.SA)
            .Items(6).SubItems(1).Text = (cLevel)
            .Items(7).SubItems(1).Text = (FormatNumber(cSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            .Items(8).SubItems(1).Text = (cTime)
            .Items(9).SubItems(1).Text = (FormatNumber(cRate, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
        End With

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
    Private Sub PrepareDepends(ByVal skillID As String)
        lvwDepend.Items.Clear()
        Dim catID As String = ""
        Dim itemID As Integer = 0
        Dim skillName As String = ""
        Dim itemData(1) As String
        Dim skillData(1) As String
        For lvl As Integer = 1 To 5
            If EveHQ.Core.HQ.SkillUnlocks.ContainsKey(skillID & "." & CStr(lvl)) = True Then
                Dim itemUnlocked As ArrayList = CType(EveHQ.Core.HQ.SkillUnlocks(skillID & "." & CStr(lvl)), ArrayList)
                For Each item As String In itemUnlocked
                    Dim newItem As New ListViewItem
                    itemData = item.Split(CChar("_"))
                    catID = CStr(EveHQ.Core.HQ.groupCats.Item(itemData(1)))
                    newItem.Group = lvwDepend.Groups("Cat" & catID)
                    itemID = EveHQ.Core.HQ.itemList.IndexOfValue(itemData(0))
                    newItem.Text = EveHQ.Core.HQ.itemList.GetKey(itemID).ToString
                    Dim skillUnlocked As ArrayList = CType(EveHQ.Core.HQ.ItemUnlocks(itemData(0)), ArrayList)
                    Dim allTrained As Boolean = True
                    For Each skillPair As String In skillUnlocked
                        skillData = skillPair.Split(CChar("."))
                        skillName = EveHQ.Core.SkillFunctions.SkillIDToName(skillData(0))
                        If skillData(0) <> skillID Then
                            newItem.ToolTipText &= skillName & " (Level " & skillData(1) & "), "
                        End If
                        If EveHQ.Core.SkillFunctions.IsSkillTrained(EveHQ.Core.HQ.myPilot, skillName, CInt(skillData(1))) = False Then
                            allTrained = False
                        End If
                    Next
                    If newItem.ToolTipText <> "" Then
                        newItem.ToolTipText = "Also Requires: " & newItem.ToolTipText
                    End If
                    If allTrained = True Then
                        newItem.ForeColor = Color.Green
                    Else
                        newItem.ForeColor = Color.Red
                    End If
                    newItem.ToolTipText = newItem.ToolTipText.TrimEnd(", ".ToCharArray)
                    newItem.SubItems.Add("Level " & lvl)
                    lvwDepend.Items.Add(newItem)
                Next
            End If
        Next
    End Sub

    Private Sub PrepareDescription(ByVal skillID As String)
        Dim cSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
        cSkill = CType(EveHQ.Core.HQ.SkillListID(skillID), Core.SkillList)
        Me.lblDescription.Text = cSkill.Description

    End Sub

    Private Sub PrepareSPs(ByVal skillID As String)
        lvwSPs.Items.Clear()
        Dim cSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
        cSkill = CType(EveHQ.Core.HQ.SkillListID(skillID), Core.SkillList)
        Dim lastSP As Long = 0
        For toLevel As Integer = 1 To 5
            Dim newGroup As ListViewItem = New ListViewItem
            newGroup.Text = toLevel.ToString
            Dim SP As Long = CLng(EveHQ.Core.SkillFunctions.CalculateSPLevel(cSkill.Rank, toLevel))
            newGroup.SubItems.Add(FormatNumber(SP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newGroup.SubItems.Add(FormatNumber(SP - lastSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            lastSP = SP
            lvwSPs.Items.Add(newGroup)
        Next
    End Sub

    Private Sub PrepareTimes(ByVal skillID As String)
        lvwTimes.Items.Clear()

        If EveHQ.Core.HQ.Pilots.Count > 0 And EveHQ.Core.HQ.myPilot.Updated = True Then
            Dim cskill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
            cskill = CType(EveHQ.Core.HQ.SkillListID(skillID), Core.SkillList)

            Dim timeToTrain As Long = 0

            For toLevel As Integer = 1 To 5
                Dim newGroup As ListViewItem = New ListViewItem
                newGroup.Text = toLevel.ToString
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, cskill, toLevel, toLevel - 1)))
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, cskill, toLevel, 0)))
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, cskill, toLevel, -1)))
                lvwTimes.Items.Add(newGroup)
            Next
        Else
            For toLevel As Integer = 1 To 5
                Dim newGroup As ListViewItem = New ListViewItem
                newGroup.Text = toLevel.ToString
                newGroup.SubItems.Add("n/a")
                newGroup.SubItems.Add("n/a")
                newGroup.SubItems.Add("n/a")
                lvwTimes.Items.Add(newGroup)
            Next
        End If
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

    Private Sub mnuViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetailsInIB.Click
        Dim PluginName As String = "EveHQ Item Browser"
        Dim itemID As String = mnuItemName.Tag.ToString
        Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.PlugIns(PluginName), Core.PlugIn)
        Dim PluginFile As String = myPlugIn.FileName
        Dim PluginType As String = myPlugIn.FileType
        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn

        If frmEveHQ.tabMDI.TabPages.ContainsKey(PluginName) = False Then
            Dim myAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(PluginFile)
            Dim t As Type = myAssembly.GetType(PluginType)
            myPlugIn.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
            runPlugIn = myPlugIn.Instance
            Dim plugInForm As Form = runPlugIn.RunEveHQPlugIn
            plugInForm.MdiParent = frmEveHQ
            plugInForm.Show()
        Else
            runPlugIn = myPlugIn.Instance
            frmEveHQ.tabMDI.SelectTab(PluginName)
        End If

        runPlugIn.SetPlugInData = itemID
    End Sub
    Private Sub ctxDepend_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxDepend.Opening
        If ctxDepend.SourceControl Is Me.lvwDepend Then
            If lvwDepend.SelectedItems.Count <> 0 Then
                Dim itemName As String = ""
                Dim itemID As String = ""
                Dim item As ListViewItem = lvwDepend.SelectedItems(0)
                itemName = item.Text
                itemID = CStr(EveHQ.Core.HQ.itemList(itemName))
                If item.Group.Name = "Cat16" Then
                    mnuViewItemDetails.Visible = True
                Else
                    mnuViewItemDetails.Visible = False
                End If
                mnuItemName.Text = itemName
                mnuItemName.Tag = itemID
            Else
                e.Cancel = True
            End If
        End If
    End Sub
    Public Sub UpdateSkillDetails()
        If EveHQ.Core.HQ.myPilot.Training = True Then
            Dim cSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListName(EveHQ.Core.HQ.myPilot.TrainingSkillName), Core.SkillList)
            If EveHQ.Core.HQ.myPilot.Training = True And lvwDetails.Items(0).SubItems(1).Text = EveHQ.Core.HQ.myPilot.TrainingSkillName Then
                lvwDetails.Items(8).SubItems(1).Text = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.HQ.myPilot.TrainingCurrentTime)
                Dim mySkill As EveHQ.Core.Skills = CType(EveHQ.Core.HQ.myPilot.PilotSkills(cSkill.Name), Core.Skills)
                lvwDetails.Items(7).SubItems(1).Text = FormatNumber(mySkill.SP + EveHQ.Core.HQ.myPilot.TrainingCurrentSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                Dim totalTime As Long = 0
                For toLevel As Integer = 1 To 5
                    Select Case toLevel
                        Case EveHQ.Core.HQ.myPilot.TrainingSkillLevel
                            totalTime += EveHQ.Core.HQ.myPilot.TrainingCurrentTime
                        Case Is > EveHQ.Core.HQ.myPilot.TrainingSkillLevel
                            totalTime = CLng(totalTime + EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, cSkill, toLevel, toLevel - 1))
                    End Select
                    lvwTimes.Items(toLevel - 1).SubItems(3).Text = EveHQ.Core.SkillFunctions.TimeToString(totalTime)
                Next
            End If
        End If
    End Sub

    Private Sub mnuViewSkillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSkillDetails.Click
        Dim skillID As String
        skillID = CStr(mnuSkillName.Tag)
        Call Me.PrepareDetails(skillID)
        Call Me.PrepareTree(skillID)
        Call Me.PrepareDepends(skillID)
        Call Me.PrepareDescription(skillID)
        Call Me.PrepareTimes(skillID)
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
        Call Me.PrepareDetails(skillID)
        Call Me.PrepareTree(skillID)
        Call Me.PrepareDepends(skillID)
        Call Me.PrepareDescription(skillID)
        Call Me.PrepareTimes(skillID)
    End Sub
End Class