' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2011  EveHQ Development Team
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

Public Class frmSkillDetails

    Dim oldNodeIndex As Integer = -1
    Dim TrainingThreshold As Integer = 1600000
    Dim TrainingBonus As Double = 1
    Dim cDisplayPilotName As String
    Dim displayPilot As New EveHQ.Core.Pilot
    Public Property DisplayPilotName() As String
        Get
            Return cDisplayPilotName
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            DisplayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(value), Core.Pilot)
        End Set
    End Property

    Public Sub ShowSkillDetails(ByVal skillID As String)

        If displayPilot.SkillPoints + displayPilot.TrainingCurrentSP < TrainingThreshold Then
            TrainingBonus = 2
        End If

        Call Me.PrepareDetails(skillID)
        Call Me.PrepareTree(skillID)
        Call Me.PrepareDepends(skillID)
        Call Me.PrepareDescription(skillID)
        Call Me.PrepareSPs(skillID)
        Call Me.PrepareTimes(skillID)

        If Me.IsHandleCreated = False Then
            Me.Show()
        Else
            Me.BringToFront()
        End If

    End Sub

    Private Sub PrepareDetails(ByVal skillID As String)

        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)

        Me.Text = "Skill Details - " & cSkill.Name
        lvwDetails.Groups(1).Header = "Pilot Specific - " & displayPilot.Name

        With Me.lvwDetails
            Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
            Dim myGroup As EveHQ.Core.SkillGroup = New EveHQ.Core.SkillGroup
            If EveHQ.Core.HQ.itemGroups.ContainsKey(cSkill.GroupID) = True Then
                Dim groupName As String = EveHQ.Core.HQ.itemGroups(cSkill.GroupID)
                If EveHQ.Core.HQ.SkillGroups.ContainsKey(groupName) = True Then
                    myGroup = EveHQ.Core.HQ.SkillGroups(groupName)
                Else
                    myGroup = Nothing
                End If
            Else
                myGroup = Nothing
            End If
            Dim cLevel, cSP, cTime, cRate As String
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
                If displayPilot.PilotSkills.Contains(cSkill.Name) = False Then
                    cLevel = "0" : cSP = "0" : cTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, 1, ))
                    cRate = CStr(EveHQ.Core.SkillFunctions.CalculateSPRate(displayPilot, cSkill))
                Else
                    mySkill = CType(displayPilot.PilotSkills(cSkill.Name), Core.PilotSkill)
                    cLevel = CStr(mySkill.Level)
                    If displayPilot.Training = True And displayPilot.TrainingSkillID = EveHQ.Core.SkillFunctions.SkillNameToID(cSkill.Name) Then
                        cSP = CStr(mySkill.SP + displayPilot.TrainingCurrentSP)
                    Else
                        cSP = CStr(mySkill.SP)
                    End If
                    If displayPilot.Training = True And displayPilot.TrainingSkillName = cSkill.Name Then
                        cTime = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    Else
                        cTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, 0, ))
                    End If
                    cRate = CStr(EveHQ.Core.SkillFunctions.CalculateSPRate(displayPilot, cSkill))
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
        tvwReqs.BeginUpdate()
        tvwReqs.Nodes.Clear()

        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
        Dim curSkill As Integer = CInt(skillID)
        Dim curLevel As Integer = 0
        Dim counter As Integer = 0
        Dim curNode As TreeNode = New TreeNode

        ' Write the skill we are querying as the first (parent) node
        curNode.Text = cSkill.Name
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
            If displayPilot.PilotSkills.Contains(cSkill.Name) Then
                Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                mySkill = CType(displayPilot.PilotSkills(cSkill.Name), Core.PilotSkill)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
                If skillTrained = True Then
                    curNode.ForeColor = Color.LimeGreen
                    curNode.ToolTipText = "Already Trained"
                Else
                    Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
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
                Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
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
            Dim subSkill As EveHQ.Core.EveSkill
            For Each subSkillID As String In cSkill.PreReqSkills.Keys
                subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
                If subSkill.ID <> cSkill.ID Then
                    Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                End If
            Next
        End If
        tvwReqs.ExpandAll()
        tvwReqs.EndUpdate()
    End Sub
    Private Sub AddPreReqsToTree(ByVal newSkill As EveHQ.Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode)
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        Dim newNode As TreeNode = New TreeNode
        newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
        newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
        ' Check status of this skill
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
            skillTrained = False
            myLevel = 0
            If displayPilot.PilotSkills.Contains(newSkill.Name) Then
                Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                mySkill = CType(displayPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
            End If
            If skillTrained = True Then
                newNode.ForeColor = Color.LimeGreen
                newNode.ToolTipText = "Already Trained"
            Else
                Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, newSkill.Name, curLevel)
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

        If newSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As EveHQ.Core.EveSkill
            For Each subSkillID As String In newSkill.PreReqSkills.Keys
                subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
                If subSkill.ID <> newSkill.ID Then
                    Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
                End If
            Next
        End If
    End Sub
    Private Sub PrepareDepends(ByVal skillID As String)
        lvwDepend.BeginUpdate()
        lvwDepend.Items.Clear()
        Dim catID As String = ""
        Dim skillName As String = ""
        Dim certName As String = ""
        Dim certGrade As String = ""
        Dim itemData(1) As String
        Dim skillData(1) As String
        For lvl As Integer = 1 To 5
            ' Add the skill unlocks
            If EveHQ.Core.HQ.SkillUnlocks.ContainsKey(skillID & "." & CStr(lvl)) = True Then
                Dim itemUnlocked As ArrayList = EveHQ.Core.HQ.SkillUnlocks(skillID & "." & CStr(lvl))
                For Each item As String In itemUnlocked
                    Dim newItem As New ListViewItem
                    Dim toolTipText As New StringBuilder

                    itemData = item.Split(CChar("_"))
                    catID = EveHQ.Core.HQ.groupCats.Item(itemData(1))
                    newItem.Group = lvwDepend.Groups("Cat" & catID)
                    newItem.Text = EveHQ.Core.HQ.itemData(itemData(0)).Name
                    newItem.Name = itemData(0)
                    Dim skillUnlocked As ArrayList = EveHQ.Core.HQ.ItemUnlocks(itemData(0))
                    Dim allTrained As Boolean = True
                    For Each skillPair As String In skillUnlocked
                        skillData = skillPair.Split(CChar("."))
                        skillName = EveHQ.Core.SkillFunctions.SkillIDToName(skillData(0))
                        If skillData(0) <> skillID Then
                            toolTipText.Append(skillName)
                            toolTipText.Append(" (Level ")
                            toolTipText.Append(skillData(1))
                            toolTipText.Append("), ")
                        End If
                        If EveHQ.Core.SkillFunctions.IsSkillTrained(displayPilot, skillName, CInt(skillData(1))) = False Then
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
            If EveHQ.Core.HQ.CertUnlockSkills.ContainsKey(skillID & "." & CStr(lvl)) = True Then
                Dim certUnlocked As ArrayList = EveHQ.Core.HQ.CertUnlockSkills(skillID & "." & CStr(lvl))
                For Each item As String In certUnlocked
                    Dim newItem As New ListViewItem
                    Dim toolTipText As New StringBuilder

                    newItem.Group = lvwDepend.Groups("CatCerts")
                    Dim cert As EveHQ.Core.Certificate = EveHQ.Core.HQ.Certificates(item)
                    certName = EveHQ.Core.HQ.CertificateClasses(cert.ClassID.ToString).Name
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
                    For Each reqCertID As String In cert.RequiredCerts.Keys
                        Dim reqCert As EveHQ.Core.Certificate = EveHQ.Core.HQ.Certificates(reqCertID)
                        If reqCert.ID.ToString <> item Then
                            toolTipText.Append(EveHQ.Core.HQ.CertificateClasses(reqCert.ClassID.ToString).Name)
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
                    If displayPilot.Certificates.Contains(cert.ID.ToString) = True Then
                        newItem.ForeColor = Color.Green
                    Else
                        newItem.ForeColor = Color.Red
                    End If
                    newItem.ToolTipText = toolTipText.ToString()
                    newItem.Text = certName & " (" & certGrade & ")"
                    newItem.Name = item
                    newItem.SubItems.Add("Level " & lvl)
                    lvwDepend.Items.Add(newItem)
                Next
            End If
        Next
        lvwDepend.EndUpdate()
    End Sub

    Private Sub PrepareDescription(ByVal skillID As String)
        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
        Me.lblDescription.Text = cSkill.Description
    End Sub

    Private Sub PrepareSPs(ByVal skillID As String)
        lvwSPs.BeginUpdate()
        lvwSPs.Items.Clear()
        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
        Dim lastSP As Long = 0
        For toLevel As Integer = 1 To 5
            Dim newGroup As ListViewItem = New ListViewItem
            newGroup.Text = toLevel.ToString
            Dim SP As Long = CLng(Math.Ceiling(EveHQ.Core.SkillFunctions.CalculateSPLevel(cSkill.Rank, toLevel)))
            newGroup.SubItems.Add(FormatNumber(SP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            newGroup.SubItems.Add(FormatNumber(SP - lastSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            lastSP = SP
            lvwSPs.Items.Add(newGroup)
        Next
        lvwSPs.EndUpdate()
    End Sub

    Private Sub PrepareTimes(ByVal skillID As String)
        lvwTimes.BeginUpdate()
        lvwTimes.Items.Clear()

        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
            Dim cskill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)

            Dim timeToTrain As Long = 0

            For toLevel As Integer = 1 To 5
                Dim newGroup As ListViewItem = New ListViewItem
                newGroup.Text = toLevel.ToString
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, toLevel - 1)))
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, 0)))
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, -1)))
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
        Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
        Dim PluginFile As String = myPlugIn.FileName
        Dim PluginType As String = myPlugIn.FileType
        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn

        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(PluginName)
        If tp Is Nothing Then
            Dim myAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(PluginFile)
            Dim t As Type = myAssembly.GetType(PluginType)
            myPlugIn.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
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
        If ctxDepend.SourceControl Is Me.lvwDepend Then
            If lvwDepend.SelectedItems.Count <> 0 Then
                Dim itemName As String = ""
                Dim itemID As String = ""
                Dim item As ListViewItem = lvwDepend.SelectedItems(0)
                itemName = item.Text
                itemID = item.Name
                If item.Group.Name = "CatCerts" = True Then
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
        If displayPilot.Training = True Then
            Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(displayPilot.TrainingSkillName)
            If displayPilot.Training = True And lvwDetails.Items(0).SubItems(1).Text = displayPilot.TrainingSkillName Then
                lvwDetails.Items(8).SubItems(1).Text = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                Dim mySkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(cSkill.Name), Core.PilotSkill)
                lvwDetails.Items(7).SubItems(1).Text = FormatNumber(mySkill.SP + displayPilot.TrainingCurrentSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                Dim totalTime As Long = 0
                For toLevel As Integer = 1 To 5
                    Select Case toLevel
                        Case displayPilot.TrainingSkillLevel
                            totalTime += displayPilot.TrainingCurrentTime
                        Case Is > displayPilot.TrainingSkillLevel
                            totalTime = CLng(totalTime + EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, toLevel, toLevel - 1))
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

    Private Sub mnuViewCertDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewCertDetails.Click
        Dim certID As String = mnuItemName.Tag.ToString
        frmCertificateDetails.Text = mnuItemName.Text
        frmCertificateDetails.DisplayPilotName = cDisplayPilotName
        frmCertificateDetails.ShowCertDetails(certID)
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add the category groups into the listview
        lvwDepend.Groups.Clear()
        For Each cat As String In EveHQ.Core.HQ.itemCats.Keys
            lvwDepend.Groups.Add("Cat" & cat, EveHQ.Core.HQ.itemCats(cat))
        Next
        lvwDepend.Groups.Add("CatCerts", "Certificates")

    End Sub
End Class