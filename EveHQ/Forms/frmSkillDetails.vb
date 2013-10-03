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
Imports System.Text
Imports EveHQ.EveData

Public Class frmSkillDetails

    Dim _oldNodeIndex As Integer = -1
    Private Const TrainingThreshold As Integer = 1600000
    Dim _trainingBonus As Double = 1
    Dim _displayPilotName As String
    Dim _displayPilot As New Core.EveHQPilot
    Public Property DisplayPilotName() As String
        Get
            Return _displayPilotName
        End Get
        Set(ByVal value As String)
            _displayPilotName = value
            _displayPilot = Core.HQ.Settings.Pilots(value)
        End Set
    End Property

    Public Sub ShowSkillDetails(ByVal skillID As Integer)

        If _displayPilot.SkillPoints + _displayPilot.TrainingCurrentSP < TrainingThreshold Then
            _trainingBonus = 2
        End If

        Call PrepareDetails(skillID)
        Call PrepareTree(skillID)
        Call PrepareDepends(skillID)
        Call PrepareDescription(skillID)
        Call PrepareQueues(skillID)
        Call PrepareSPs(skillID)
        Call PrepareTimes(skillID)

        If IsHandleCreated = False Then
            Show()
        Else
            BringToFront()
        End If

    End Sub

    Private Sub PrepareDetails(ByVal skillID As Integer)

        Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)

        Text = "Skill Details - " & cSkill.Name
        lvwDetails.Groups(1).Header = "Pilot Specific - " & _displayPilot.Name

        With lvwDetails
            Dim mySkill As Core.EveHQPilotSkill
            Dim myGroup As Core.SkillGroup
            If Core.HQ.itemGroups.ContainsKey(cSkill.GroupID) = True Then
                Dim groupName As String = Core.HQ.itemGroups(cSkill.GroupID)
                If Core.HQ.SkillGroups.ContainsKey(groupName) = True Then
                    myGroup = Core.HQ.SkillGroups(groupName)
                Else
                    myGroup = Nothing
                End If
            Else
                myGroup = Nothing
            End If
            Dim cLevel, cSP, cTime, cRate As String
            If Core.HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                If _displayPilot.PilotSkills.ContainsKey(cSkill.Name) = False Then
                    cLevel = "0" : cSP = "0" : cTime = Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(_displayPilot, cSkill, 1, ))
                    cRate = CStr(Core.SkillFunctions.CalculateSPRate(_displayPilot, cSkill))
                Else
                    mySkill = _displayPilot.PilotSkills(cSkill.Name)
                    cLevel = CStr(mySkill.Level)
                    If _displayPilot.Training = True And _displayPilot.TrainingSkillID = Core.SkillFunctions.SkillNameToID(cSkill.Name) Then
                        cSP = CStr(mySkill.SP + _displayPilot.TrainingCurrentSP)
                    Else
                        cSP = CStr(mySkill.SP)
                    End If
                    If _displayPilot.Training = True And _displayPilot.TrainingSkillName = cSkill.Name Then
                        cTime = Core.SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                    Else
                        cTime = Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(_displayPilot, cSkill, 0, ))
                    End If
                    cRate = CStr(Core.SkillFunctions.CalculateSPRate(_displayPilot, cSkill))
                End If
            Else
                cLevel = "n/a" : cSP = "0" : cTime = "n/a" : cRate = "0"
            End If

            .Items(0).SubItems(1).Text = (cSkill.Name)
            .Items(1).SubItems(1).Text = CStr((cSkill.Rank))
            If myGroup IsNot Nothing Then
                .Items(2).SubItems(1).Text = (myGroup.Name)
            Else
                .Items(2).SubItems(1).Text = "<Unknown>"
            End If
            .Items(3).SubItems(1).Text = cSkill.BasePrice.ToString("N2")
            .Items(4).SubItems(1).Text = cSkill.Pa
            .Items(5).SubItems(1).Text = cSkill.Sa
            .Items(6).SubItems(1).Text = cLevel.ToString
            .Items(7).SubItems(1).Text = CLng(cSP).ToString("N0")
            .Items(8).SubItems(1).Text = cTime
            .Items(9).SubItems(1).Text = CDbl(cRate).ToString("N0")
        End With

    End Sub

    Private Sub PrepareTree(ByVal skillID As Integer)
        tvwReqs.BeginUpdate()
        tvwReqs.Nodes.Clear()

        Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)
        Const curLevel As Integer = 0
        Dim curNode As TreeNode = New TreeNode

        ' Write the skill we are querying as the first (parent) node
        curNode.Text = cSkill.Name
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer
        If Core.HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
            If _displayPilot.PilotSkills.ContainsKey(cSkill.Name) Then
                Dim mySkill As Core.EveHQPilotSkill = _displayPilot.PilotSkills(cSkill.Name)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
                If skillTrained = True Then
                    curNode.ForeColor = Color.LimeGreen
                    curNode.ToolTipText = "Already Trained"
                Else
                    Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, curLevel)
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
                Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, curLevel)
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

        If cSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As Core.EveSkill
            For Each subSkillID As Integer In cSkill.PreReqSkills.Keys
                subSkill = Core.HQ.SkillListID(subSkillID)
                If subSkill.ID <> cSkill.ID Then
                    Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                End If
            Next
        End If
        tvwReqs.ExpandAll()
        tvwReqs.EndUpdate()
    End Sub
    Private Sub AddPreReqsToTree(ByVal newSkill As Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode)
        Dim skillTrained As Boolean
        Dim newNode As TreeNode = New TreeNode
        newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
        newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
        ' Check status of this skill
        If Core.HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
            skillTrained = False
            Dim myLevel As Integer
            If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) Then
                Dim mySkill As Core.EveHQPilotSkill = _displayPilot.PilotSkills(newSkill.Name)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
            End If
            If skillTrained = True Then
                newNode.ForeColor = Color.LimeGreen
                newNode.ToolTipText = "Already Trained"
            Else
                Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(_displayPilot, newSkill.Name, curLevel)
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
        
        If newSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As Core.EveSkill
            For Each subSkillID As Integer In newSkill.PreReqSkills.Keys
                subSkill = Core.HQ.SkillListID(subSkillID)
                If subSkill.ID <> newSkill.ID Then
                    Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
                End If
            Next
        End If
    End Sub
    Private Sub PrepareDepends(ByVal skillID As Integer)
        lvwDepend.BeginUpdate()
        lvwDepend.Items.Clear()
        Dim catID As Integer
        Dim skillName As String
        Dim certName As String
        Dim certGrade As String = ""
        Dim itemData(1) As String
        Dim skillData(1) As String
        For lvl As Integer = 1 To 5
            ' Add the skill unlocks
            If StaticData.SkillUnlocks.ContainsKey(skillID & "." & CStr(lvl)) = True Then
                Dim itemUnlocked As List(Of String) = StaticData.SkillUnlocks(skillID & "." & CStr(lvl))
                For Each item As String In itemUnlocked
                    Dim newItem As New ListViewItem
                    Dim toolTipText As New StringBuilder

                    itemData = item.Split(CChar("_"))
                    catID = Core.HQ.groupCats.Item(CInt(itemData(1)))
                    newItem.Group = lvwDepend.Groups("Cat" & catID)
                    newItem.Text = Core.HQ.itemData(itemData(0)).Name
                    newItem.Name = itemData(0)
                    Dim skillUnlocked As List(Of String) = StaticData.ItemUnlocks(itemData(0))
                    Dim allTrained As Boolean = True
                    For Each skillPair As String In skillUnlocked
                        skillData = skillPair.Split(CChar("."))
                        skillName = Core.SkillFunctions.SkillIDToName(CInt(skillData(0)))
                        If skillData(0) <> CStr(skillID) Then
                            toolTipText.Append(skillName)
                            toolTipText.Append(" (Level ")
                            toolTipText.Append(skillData(1))
                            toolTipText.Append("), ")
                        End If
                        If Core.SkillFunctions.IsSkillTrained(_displayPilot, skillName, CInt(skillData(1))) = False Then
                            allTrained = False
                        End If
                    Next
                    If toolTipText.Length > 0 Then
                        toolTipText.Insert(0, "Also Requires: ")

                        If (toolTipText.ToString().EndsWith(", ")) Then
                            toolTipText.Remove(toolTipText.Length - 2, 2)
                        End If
                    End If
                    If allTrained = True Then
                        newItem.ForeColor = Color.Green
                    Else
                        newItem.ForeColor = Color.Red
                    End If
                    newItem.ToolTipText = toolTipText.ToString()
                    newItem.SubItems.Add("Level " & lvl)
                    lvwDepend.Items.Add(newItem)
                Next
            End If
            ' Add the certificate unlocks
            If StaticData.CertUnlockSkills.ContainsKey(skillID & "." & CStr(lvl)) = True Then
                Dim certUnlocked As List(Of Integer) = StaticData.CertUnlockSkills(skillID & "." & CStr(lvl))
                For Each item As Integer In certUnlocked
                    Dim newItem As New ListViewItem
                    Dim toolTipText As New StringBuilder

                    newItem.Group = lvwDepend.Groups("CatCerts")
                    Dim cert As Certificate = StaticData.Certificates(item)
                    certName = StaticData.CertificateClasses(cert.ClassId.ToString).Name
                    Select Case cert.Grade
                        Case 1
                            certGrade = "Basic"
                        Case 2
                            certGrade = "Standard"
                        Case 3
                            certGrade = "Improved"
                        Case 4
                            certGrade = "Advanced"
                        Case 5
                            certGrade = "Elite"
                    End Select
                    For Each reqCertID As Integer In cert.RequiredCertificates.Keys
                        Dim reqCert As Certificate = StaticData.Certificates(reqCertID)
                        If reqCert.Id <> item Then
                            toolTipText.Append(StaticData.CertificateClasses(reqCert.ClassId.ToString).Name)
                            Select Case reqCert.Grade
                                Case 1
                                    toolTipText.Append(" (Basic), ")
                                Case 2
                                    toolTipText.Append(" (Standard), ")
                                Case 3
                                    toolTipText.Append(" (Improved), ")
                                Case 4
                                    toolTipText.Append(" (Advanced), ")
                                Case 5
                                    toolTipText.Append(" (Elite), ")
                            End Select
                        End If
                    Next
                    If toolTipText.Length > 0 Then
                        toolTipText.Insert(0, "Also Requires: ")

                        If (toolTipText.ToString().EndsWith(", ")) Then
                            toolTipText.Remove(toolTipText.Length - 2, 2)
                        End If
                    End If
                    If _displayPilot.Certificates.Contains(cert.Id) = True Then
                        newItem.ForeColor = Color.Green
                    Else
                        newItem.ForeColor = Color.Red
                    End If
                    newItem.ToolTipText = toolTipText.ToString()
                    newItem.Text = certName & " (" & certGrade & ")"
                    newItem.Name = CStr(item)
                    newItem.SubItems.Add("Level " & lvl)
                    lvwDepend.Items.Add(newItem)
                Next
            End If
        Next
        lvwDepend.EndUpdate()
    End Sub
    Private Sub PrepareQueues(ByVal skillID As Integer)
        lvwQueues.BeginUpdate()
        lvwQueues.Items.Clear()
        For Each skillQ As Core.EveHQSkillQueue In _displayPilot.TrainingQueues.Values
            Dim maxLevel As Integer = 0
            For Each s As Core.EveHQSkillQueueItem In skillQ.Queue.Values
                If s.Name = Core.SkillFunctions.SkillIDToName(skillID) Then
                    maxLevel = Math.Max(maxLevel, s.ToLevel)
                End If
            Next
            If maxLevel > 0 Then
                Dim newItem As New ListViewItem
                newItem.Name = skillQ.Name
                newItem.Text = skillQ.Name
                newItem.SubItems.Add(maxLevel.ToString)
                lvwQueues.Items.Add(newItem)
            End If
        Next
        lvwQueues.EndUpdate()
    End Sub
    Private Sub PrepareDescription(ByVal skillID As Integer)
        Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)
        lblDescription.Text = cSkill.Description
    End Sub
    Private Sub PrepareSPs(ByVal skillID As Integer)
        lvwSPs.BeginUpdate()
        lvwSPs.Items.Clear()
        Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)
        Dim lastSP As Long = 0
        For toLevel As Integer = 1 To 5
            Dim newGroup As ListViewItem = New ListViewItem
            newGroup.Text = toLevel.ToString
            Dim SP As Long = CLng(Math.Ceiling(Core.SkillFunctions.CalculateSPLevel(cSkill.Rank, toLevel)))
            newGroup.SubItems.Add(SP.ToString("N0"))
            newGroup.SubItems.Add((SP - lastSP).ToString("N0"))
            lastSP = SP
            lvwSPs.Items.Add(newGroup)
        Next
        lvwSPs.EndUpdate()
    End Sub
    Private Sub PrepareTimes(ByVal skillID As Integer)
        lvwTimes.BeginUpdate()
        lvwTimes.Items.Clear()

        If Core.HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
            Dim cskill As Core.EveSkill = Core.HQ.SkillListID(skillID)

            Dim timeToTrain As Long = 0

            For toLevel As Integer = 1 To 5
                Dim newGroup As ListViewItem = New ListViewItem
                newGroup.Text = toLevel.ToString
                newGroup.SubItems.Add(Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(_displayPilot, cskill, toLevel, toLevel - 1)))
                newGroup.SubItems.Add(Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(_displayPilot, cskill, toLevel, 0)))
                newGroup.SubItems.Add(Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(_displayPilot, cskill, toLevel, -1)))
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
        lvwTimes.EndUpdate()
    End Sub

    Private Sub tvwReqs_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwReqs.MouseMove
        Dim tn As TreeNode = tvwReqs.GetNodeAt(e.X, e.Y)
        If Not (tn Is Nothing) Then
            Dim currentNodeIndex As Integer = tn.Index
            If currentNodeIndex <> _oldNodeIndex Then
                _oldNodeIndex = currentNodeIndex
                If Not (SkillToolTip Is Nothing) And SkillToolTip.Active Then
                    SkillToolTip.Active = False 'turn it off 
                End If
                SkillToolTip.SetToolTip(tvwReqs, tn.ToolTipText)
                SkillToolTip.Active = True 'make it active so it can show 
            End If
        End If
    End Sub

    Private Sub tvwReqs_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwReqs.NodeMouseClick
        tvwReqs.SelectedNode = e.Node
    End Sub

    Private Sub mnuViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetailsInIB.Click
        Const pluginName As String = "EveHQ Item Browser"
        Dim itemID As String = mnuItemName.Tag.ToString
        Dim myPlugIn As Core.EveHQPlugIn = Core.HQ.Plugins(pluginName)
        Dim pluginFile As String = myPlugIn.FileName
        Dim pluginType As String = myPlugIn.FileType
        Dim runPlugIn As Core.IEveHQPlugIn

        Dim tp As DevComponents.DotNetBar.TabItem = Core.HQ.GetMDITab(pluginName)
        If tp Is Nothing Then
            Dim myAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(pluginFile)
            Dim t As Type = myAssembly.GetType(pluginType)
            myPlugIn.Instance = CType(Activator.CreateInstance(t), Core.IEveHQPlugIn)
            runPlugIn = myPlugIn.Instance
            Dim plugInForm As Form = runPlugIn.RunEveHQPlugIn
            plugInForm.MdiParent = frmEveHQ
            plugInForm.Show()
        Else
            runPlugIn = myPlugIn.Instance
            frmEveHQ.tabEveHQMDI.SelectedTab = tp
        End If

        runPlugIn.GetPlugInData(itemID, 0)
    End Sub
    Private Sub ctxDepend_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxDepend.Opening
        If ctxDepend.SourceControl Is lvwDepend Then
            If lvwDepend.SelectedItems.Count <> 0 Then
                Dim itemName As String = ""
                Dim itemID As String = ""
                Dim item As ListViewItem = lvwDepend.SelectedItems(0)
                itemName = item.Text
                itemID = item.Name
                If item.Group.Name = "CatCerts" Then
                    mnuViewItemDetails.Visible = False
                    mnuViewCertDetails.Visible = True
                    mnuViewItemDetailsInIB.Visible = False
                Else
                    mnuViewCertDetails.Visible = False
                    mnuViewItemDetailsInIB.Visible = True
                    If item.Group.Name = "Cat16" Then
                        mnuViewItemDetails.Visible = True
                    Else
                        mnuViewItemDetails.Visible = False
                    End If
                End If
                mnuItemName.Text = itemName
                mnuItemName.Tag = itemID
            Else
                e.Cancel = True
            End If
        End If
    End Sub
    Public Sub UpdateSkillDetails()
        If _displayPilot.Training = True Then
            Dim cSkill As Core.EveSkill = Core.HQ.SkillListName(_displayPilot.TrainingSkillName)
            If _displayPilot.Training = True And lvwDetails.Items(0).SubItems(1).Text = _displayPilot.TrainingSkillName Then
                lvwDetails.Items(8).SubItems(1).Text = Core.SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                Dim mySkill As Core.EveHQPilotSkill = _displayPilot.PilotSkills(cSkill.Name)
                lvwDetails.Items(7).SubItems(1).Text = (mySkill.SP + _displayPilot.TrainingCurrentSP).ToString("N0")
                Dim totalTime As Long = 0
                For toLevel As Integer = 1 To 5
                    Select Case toLevel
                        Case _displayPilot.TrainingSkillLevel
                            totalTime += _displayPilot.TrainingCurrentTime
                        Case Is > _displayPilot.TrainingSkillLevel
                            totalTime = CLng(totalTime + Core.SkillFunctions.CalcTimeToLevel(_displayPilot, cSkill, toLevel, toLevel - 1))
                    End Select
                    lvwTimes.Items(toLevel - 1).SubItems(3).Text = Core.SkillFunctions.TimeToString(totalTime)
                Next
            End If
        End If
    End Sub

    Private Sub mnuViewSkillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSkillDetails.Click
        Dim skillID As Integer
        skillID = CInt(mnuSkillName.Tag)
        Call PrepareDetails(skillID)
        Call PrepareTree(skillID)
        Call PrepareDepends(skillID)
        Call PrepareDescription(skillID)
        Call PrepareTimes(skillID)
    End Sub

    Private Sub ctxReqs_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxReqs.Opening
        Dim curNode As TreeNode
        curNode = tvwReqs.SelectedNode
        Dim skillName As String
        Dim skillID As Integer
        skillName = curNode.Text
        If InStr(skillName, "(Level") <> 0 Then
            skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
        End If
        skillID = Core.SkillFunctions.SkillNameToID(skillName)
        mnuSkillName.Text = skillName
        mnuSkillName.Tag = skillID
    End Sub

    Private Sub mnuViewItemDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetails.Click
        Dim skillID As Integer
        skillID = CInt(mnuItemName.Tag)
        Call PrepareDetails(skillID)
        Call PrepareTree(skillID)
        Call PrepareDepends(skillID)
        Call PrepareDescription(skillID)
        Call PrepareTimes(skillID)
    End Sub

    Private Sub mnuViewCertDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewCertDetails.Click
        Dim certID As Integer = CInt(mnuItemName.Tag)
        frmCertificateDetails.Text = mnuItemName.Text
        frmCertificateDetails.DisplayPilotName = _displayPilotName
        frmCertificateDetails.ShowCertDetails(certID)
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add the category groups into the listview
        lvwDepend.Groups.Clear()
        For Each cat As Integer In Core.HQ.itemCats.Keys
            lvwDepend.Groups.Add("Cat" & cat, Core.HQ.itemCats(cat))
        Next
        lvwDepend.Groups.Add("CatCerts", "Certificates")

    End Sub
End Class