' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
'=========================================================================#
Imports System.ComponentModel
Imports System.IO.Compression
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.EveData
Imports EveHQ.Controls
Imports EveHQ.Core
Imports System.Text
Imports EveHQ.Core.ItemBrowser
Imports System.IO
Imports System.Xml
Imports EveHQ.Core.Requisitions

Namespace Forms

    Public Class FrmTraining

        Dim _oldNodeIndex As Integer = -1
        ReadOnly _openNodes As New List(Of String)
        Dim _hideQueuedSkills As Boolean = False
        Dim _hideLevel5Skills As Boolean = False
        Dim _selQTime As Double = 0
        Dim _activeQueueName As String = ""
        Dim _activeQueueControl As EveHQTrainingQueue
        Dim _activeQueueTree As AdvTree
        Dim _usingFilter As Boolean = True
        ReadOnly _skillListNodes As New SortedList(Of String, Node)
        ReadOnly _certListNodes As New SortedList
        ReadOnly _certGrades() As String = New String() {"", "Basic", "Standard", "Improved", "Advanced", "Elite"}
        Dim _displayPilot As New EveHQPilot
        Dim _cDisplayPilotName As String = ""
        Dim _startup As Boolean = False
        Dim _redrawingOptions As Boolean = False
        Dim _retainQueue As Boolean = False
        Dim _oldTabName As String = "tabSummary"

        Public Property DisplayPilotName() As String
            Get
                Return _cDisplayPilotName
            End Get
            Set(ByVal value As String)
                _cDisplayPilotName = value
                If cboPilots.Items.Contains(value) Then
                    cboPilots.SelectedItem = value
                End If
            End Set
        End Property

#Region "Form Loading and Setup Routines"

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            adtSkillList.AllowDrop = False

        End Sub
        Private Sub frmTraining_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            HQ.Settings.SkillQueuePanelWidth = panelInfo.Width
            RemoveHandler SkillQueueFunctions.RefreshQueue, AddressOf RefreshAllTraining
        End Sub

        Private Sub frmTraining_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Set the startup flag
            _startup = True

            panelInfo.Width = HQ.Settings.SkillQueuePanelWidth

            ' Load the pilots
            Call UpdatePilots()

            ' Set up the queue information
            cboFilter.SelectedIndex = 0
            cboCertFilter.SelectedIndex = 0
            Call SetupReqsAndDepends()
            Call SetupQueues()
            Call RefreshAllTrainingQueues()
            AddHandler SkillQueueFunctions.RefreshQueue, AddressOf RefreshAllTraining

            ' Disable the startup flag
            _startup = False

        End Sub

        Private Sub frmTraining_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
            ' Force an update of the ribbon
            panelInfo.Visible = True
            tabQueues.Visible = True
        End Sub

        Private Sub SetupReqsAndDepends()
            ' Reset data
            SkillQueueFunctions.SkillDepends.Clear()
            SkillQueueFunctions.SkillPrereqs.Clear()
            Dim depends As SortedList(Of String, Integer)
            Dim preReqs As SortedList(Of String, Integer)
            Dim preReqID As Integer
            Dim preReqName As String
            ' Cycle through each skill and extract pre-req and build dependancy information
            For Each cSkill As EveSkill In HQ.SkillListID.Values
                preReqs = New SortedList(Of String, Integer)
                For Each preReqID In cSkill.PreReqSkills.Keys
                    If StaticData.Types.ContainsKey(preReqID) = True Then
                        preReqName = StaticData.Types(preReqID).Name
                        preReqs.Add(preReqName, cSkill.PreReqSkills(preReqID))
                        If SkillQueueFunctions.SkillDepends.ContainsKey(preReqName) = True Then
                            depends = SkillQueueFunctions.SkillDepends(preReqName)
                        Else
                            depends = New SortedList(Of String, Integer)
                            SkillQueueFunctions.SkillDepends.Add(preReqName, depends)
                        End If
                        depends.Add(cSkill.Name, cSkill.PreReqSkills(preReqID))
                    End If
                Next
                SkillQueueFunctions.SkillPrereqs.Add(cSkill.Name, preReqs)
            Next
            ' Add the category groups into the listview
            lvwDepend.Groups.Clear()
            For Each cat As Integer In StaticData.TypeCats.Keys
                lvwDepend.Groups.Add("Cat" & cat, StaticData.TypeCats(cat))
            Next
            lvwDepend.Groups.Add("CatCerts", "Certificates")
        End Sub
        Public Sub UpdatePilots()

            ' Save old Pilot info
            Dim oldPilot As String = ""
            If cboPilots.SelectedItem IsNot Nothing Then
                oldPilot = cboPilots.SelectedItem.ToString
            End If

            ' Save old queue info
            Dim oldQueue As String = _activeQueueName

            ' Update the pilots combo box
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboPilots.Items.Add(cPilot.Name)
                End If
            Next
            cboPilots.EndUpdate()

            ' Select a pilot
            If _cDisplayPilotName <> "" Then
                If cboPilots.Items.Contains(_cDisplayPilotName) = True Then
                    _retainQueue = True
                    cboPilots.SelectedItem = _cDisplayPilotName
                Else
                    If cboPilots.Items.Count > 0 Then
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            Else
                If oldPilot = "" Then
                    If cboPilots.Items.Count > 0 Then
                        If cboPilots.Items.Contains(HQ.Settings.StartupPilot) = True Then
                            cboPilots.SelectedItem = HQ.Settings.StartupPilot
                        Else
                            cboPilots.SelectedIndex = 0
                        End If
                    End If
                Else
                    If cboPilots.Items.Count > 0 Then
                        If cboPilots.Items.Contains(oldPilot) = True Then
                            If Not (CStr(cboPilots.SelectedItem) = oldPilot) Then
                                _retainQueue = True
                                cboPilots.SelectedItem = oldPilot
                            End If
                        Else
                            cboPilots.SelectedIndex = 0
                        End If
                    End If
                End If
            End If

            ' Select a queue
            If oldQueue <> "" Then
                If tabQueues.Controls.ContainsKey(oldQueue) = True Then
                    Dim ti As TabItem = tabQueues.Tabs.Item(oldQueue)
                    tabQueues.SelectedTab = ti
                End If
            End If

        End Sub

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            If HQ.Settings.Pilots.ContainsKey(cboPilots.SelectedItem.ToString) = True Then
                _displayPilot = HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
                _cDisplayPilotName = _displayPilot.Name
                ' Only update if we are not starting up
                If _startup = False Then
                    _startup = True
                    Call RefreshAllTraining()
                    _startup = False
                    ' See if the Neural Remapping form is open
                    If FrmNeuralRemap.IsHandleCreated = True Then
                        FrmNeuralRemap.PilotName = _displayPilot.Name
                    End If
                    ' See if the Implants form is open
                    If FrmImplants.IsHandleCreated = True Then
                        FrmImplants.PilotName = _displayPilot.Name
                    End If
                End If
            End If
        End Sub

        Public Sub SetupQueues()

            ' Save the current active queue name
            If _retainQueue = False Then
                _oldTabName = "tabSummary"
            Else
                _oldTabName = tabQueues.SelectedTab.Name
            End If
            _retainQueue = False

            ' Remove all but the summary tab on the tabQueues
            Dim ti As TabItem
            For tidx As Integer = tabQueues.Tabs.Count - 1 To 1 Step -1
                ti = tabQueues.Tabs(tidx)
                If ti.Name <> "tabSummary" Then
                    Dim tq As EveHQTrainingQueue = TryCast(ti.AttachedControl, EveHQTrainingQueue)
                    If tq IsNot Nothing Then
                        RemoveHandler tq.adtQueue.KeyDown, AddressOf activeLVW_KeyDown
                        RemoveHandler tq.adtQueue.Click, AddressOf activeLVW_Click
                        RemoveHandler tq.adtQueue.DoubleClick, AddressOf activeLVW_DoubleClick
                        RemoveHandler tq.adtQueue.DragEnter, AddressOf activeLVW_DragEnter
                        RemoveHandler tq.adtQueue.ColumnHeaderMouseDown, AddressOf activeLVW_ColumnClick
                        RemoveHandler tq.adtQueue.SelectionChanged, AddressOf activeLVW_SelectionChanged
                        RemoveHandler tq.QueueUpdated, AddressOf QueueUpdated
                    End If
                    tabQueues.Tabs.Remove(ti)
                    ti.Dispose()
                End If
            Next

            If _displayPilot IsNot Nothing Then
                If _displayPilot.TrainingQueues IsNot Nothing Then
                    For Each newQ As EveHQSkillQueue In _displayPilot.TrainingQueues.Values
                        Dim newQTab As TabItem
                        If Not tabQueues.Controls.ContainsKey(newQ.Name) Then
                            newQTab = New TabItem
                            newQTab.Name = newQ.Name
                            newQTab.Text = newQ.Name.Replace("&", "&&")

                            Dim tq As New EveHQTrainingQueue(_displayPilot.Name, newQ.Name)
                            tq.Dock = DockStyle.Fill
                            tq.Name = "TQ" & newQ.Name
                            newQTab.AttachedControl = tq

                            AddHandler tq.adtQueue.KeyDown, AddressOf activeLVW_KeyDown
                            AddHandler tq.adtQueue.Click, AddressOf activeLVW_Click
                            AddHandler tq.adtQueue.DoubleClick, AddressOf activeLVW_DoubleClick
                            AddHandler tq.adtQueue.DragEnter, AddressOf activeLVW_DragEnter
                            AddHandler tq.adtQueue.ColumnHeaderMouseDown, AddressOf activeLVW_ColumnClick
                            AddHandler tq.adtQueue.SelectionChanged, AddressOf activeLVW_SelectionChanged
                            AddHandler tq.QueueUpdated, AddressOf QueueUpdated

                            Call tq.DrawColumnHeadings()

                            tabQueues.Tabs.Add(newQTab)

                        End If
                    Next
                End If
            End If

            Dim oldTab As TabItem = tabQueues.Tabs(_oldTabName)
            If HQ.Settings.StartWithPrimaryQueue = True And _oldTabName = "tabSummary" Then
                oldTab = tabQueues.Tabs(_displayPilot.PrimaryQueue)
            End If

            If oldTab IsNot Nothing Then
                tabQueues.SelectedTab = oldTab
            Else
                tabQueues.SelectedTab = tabQueues.Tabs(0)
            End If

            tabQueues.Refresh()

        End Sub

        Private Sub QueueUpdated()
            RedrawOptions()
            DrawQueueSummary()
        End Sub

        Public Sub RefreshAllTrainingQueues()
            For Each newQ As EveHQSkillQueue In _displayPilot.TrainingQueues.Values
                Call RefreshTraining(newQ.Name, True)
            Next
            Call DrawQueueSummary()
        End Sub

        Private Sub tabQueues_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tabQueues.SelectedTabChanged
            If tabQueues.SelectedTab.Name IsNot Nothing Then
                If _displayPilot.TrainingQueues.ContainsKey(tabQueues.SelectedTab.Name) = True Then
                    _activeQueueName = tabQueues.SelectedTab.Name
                    _displayPilot.ActiveQueueName = _activeQueueName
                    _activeQueueControl = CType(tabQueues.Tabs.Item(_activeQueueName).AttachedControl, EveHQTrainingQueue)
                    _activeQueueTree = _activeQueueControl.adtQueue
                    _displayPilot.ActiveQueue = _activeQueueControl.Queue
                    Call RedrawOptions()
                    _activeQueueControl.RedrawMenuOptions()
                    _activeQueueTree.Select()
                    mnuAddToQueue.Enabled = True
                Else
                    _activeQueueName = ""
                    _activeQueueControl = Nothing
                    _displayPilot.ActiveQueueName = _activeQueueName
                    mnuAddToQueue.Enabled = False
                    btnRBIncreaseLevel.Enabled = False
                    btnRBDecreaseLevel.Enabled = False
                    btnRBDeleteSkill.Enabled = False
                    btnRBMoveUpQueue.Enabled = False
                    btnRBMoveDownQueue.Enabled = False
                    btnRBAddSkill.Enabled = False
                    btnRBSplitQueue.Enabled = False
                    btnIncTraining.Checked = False
                    btnIncTraining.Enabled = False
                    btnRBClearQueue.Enabled = False
                    btnImplants.Enabled = False
                    btnRemap.Enabled = False
                    btnExportEMPFile.Enabled = False
                End If
                If FrmNeuralRemap.IsHandleCreated = True Then
                    FrmNeuralRemap.QueueName = _activeQueueName
                End If
                If FrmImplants.IsHandleCreated = True Then
                    FrmImplants.QueueName = _activeQueueName
                End If

            End If
        End Sub
#End Region

#Region "Training Refresh Routines"
        Public Sub RefreshAllTraining()
            If IsHandleCreated = True Then
                Call SetupQueues()
                Call RefreshAllTrainingQueues()
                Call ResetQueueOptions()
                Call LoadSkillTree()
                Call LoadCertificateTree()
            End If
        End Sub
        Public Sub LoadSkillTree()
            Dim filter As Integer = cboFilter.SelectedIndex
            ' Save current open nodes
            _openNodes.Clear()
            For Each oNode As Node In adtSkillList.Nodes
                If oNode.Expanded = True Then
                    _openNodes.Add(oNode.Name)
                End If
            Next
            adtSkillList.BeginUpdate()
            adtSkillList.Nodes.Clear()
            Call LoadSkillGroups()
            Call LoadFilteredSkills(filter)
            Call ShowSkillGroups()
            For Each oNode As String In _openNodes
                If adtSkillList.FindNodeByName(oNode) IsNot Nothing Then
                    adtSkillList.FindNodeByName(oNode).Expand()
                End If
            Next
            AdvTreeSorter.Sort(adtSkillList, New AdvTreeSortResult(1, AdvTreeSortOrder.Ascending), True)
            adtSkillList.EndUpdate()
            adtSkillList.Refresh()
        End Sub
        Private Sub LoadSkillGroups()
            _skillListNodes.Clear()
            Dim newSkillGroup As SkillGroup
            For Each newSkillGroup In HQ.SkillGroups.Values
                If newSkillGroup.ID <> 505 Then
                    Dim groupNode As New Node
                    groupNode.Name = CStr(newSkillGroup.ID)
                    groupNode.Text = newSkillGroup.Name.Trim
                    groupNode.ImageIndex = 8
                    _skillListNodes.Add(groupNode.Name, groupNode)
                End If
            Next
        End Sub
        Private Sub LoadFilteredSkills(ByVal filter As Integer)
            Dim newSkill As EveSkill
            Dim groupNode As Node
            For Each newSkill In HQ.SkillListID.Values
                Dim gID As Integer = newSkill.GroupID
                If gID <> 505 Then
                    groupNode = _skillListNodes(CStr(gID))
                    Dim skillNode As New Node
                    skillNode.Text = newSkill.Name
                    skillNode.Name = CStr(newSkill.ID)
                    skillNode.Cells.Add(New Cell(newSkill.Rank.ToString))
                    If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                        Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(newSkill.Name)
                        skillNode.ImageIndex = mySkill.Level
                    Else
                        skillNode.ImageIndex = 10
                    End If
                    If _displayPilot.Name <> "" Then
                        Dim addSkill As Boolean = False
                        Select Case filter
                            Case 0
                                If newSkill.Published = True Then
                                    addSkill = True
                                End If
                            Case 1
                                addSkill = True
                            Case 2
                                If newSkill.Published = False Then
                                    addSkill = True
                                End If
                            Case 3
                                If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                                    addSkill = True
                                End If
                            Case 4
                                If newSkill.Published = True And _displayPilot.PilotSkills.ContainsKey(newSkill.Name) = False Then
                                    addSkill = True
                                End If
                            Case 5
                                Dim trainable As Boolean = False
                                If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) = False And newSkill.Published = True Then
                                    trainable = True
                                    For Each preReq As Integer In newSkill.PreReqSkills.Keys
                                        If newSkill.PreReqSkills(preReq) <> 0 Then
                                            Dim ps As EveSkill = HQ.SkillListID(preReq)
                                            If _displayPilot.PilotSkills.ContainsKey(ps.Name) = True Then
                                                Dim psp As EveHQPilotSkill = _displayPilot.PilotSkills(ps.Name)
                                                If psp.Level < newSkill.PreReqSkills(preReq) Then
                                                    trainable = False
                                                    Exit For
                                                End If
                                            Else
                                                trainable = False
                                                Exit For
                                            End If
                                        End If
                                    Next
                                End If
                                If trainable = True Then
                                    addSkill = True
                                End If

                            Case 6 To 7
                                ' 6 = exact level, 7 = partially trained
                                If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                                    Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(newSkill.Name)
                                    Dim partTrained As Boolean = True
                                    For level As Integer = 0 To 5
                                        If mySkill.SP = newSkill.LevelUp(level) Or mySkill.SP = newSkill.LevelUp(level) + 1 Then
                                            partTrained = False
                                            Exit For
                                        End If
                                    Next
                                    If (partTrained = True And filter = 7) Or (partTrained = False And filter = 6 And mySkill.Level < 5) Then
                                        addSkill = True
                                    End If
                                End If
                            Case 8 To 12
                                Dim requiredLevel As Integer = filter - 8
                                If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                                    Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(newSkill.Name)
                                    If requiredLevel = mySkill.Level Then
                                        addSkill = True
                                    End If
                                End If
                            Case 13 To 28
                                If newSkill.Published = True And newSkill.Rank = (filter - 12) Then
                                    addSkill = True
                                End If
                            Case 29
                                If newSkill.Published = True And newSkill.Pa = "Charisma" Then
                                    addSkill = True
                                End If
                            Case 30
                                If newSkill.Published = True And newSkill.Pa = "Intelligence" Then
                                    addSkill = True
                                End If
                            Case 31
                                If newSkill.Published = True And newSkill.Pa = "Memory" Then
                                    addSkill = True
                                End If
                            Case 32
                                If newSkill.Published = True And newSkill.Pa = "Perception" Then
                                    addSkill = True
                                End If
                            Case 33
                                If newSkill.Published = True And newSkill.Pa = "Willpower" Then
                                    addSkill = True
                                End If
                            Case 34
                                If newSkill.Published = True And newSkill.Sa = "Charisma" Then
                                    addSkill = True
                                End If
                            Case 35
                                If newSkill.Published = True And newSkill.Sa = "Intelligence" Then
                                    addSkill = True
                                End If
                            Case 36
                                If newSkill.Published = True And newSkill.Sa = "Memory" Then
                                    addSkill = True
                                End If
                            Case 37
                                If newSkill.Published = True And newSkill.Sa = "Perception" Then
                                    addSkill = True
                                End If
                            Case 38
                                If newSkill.Published = True And newSkill.Sa = "Willpower" Then
                                    addSkill = True
                                End If
                        End Select
                        If addSkill = True Then

                            If HideSkill(newSkill) = False Then
                                If groupNode IsNot Nothing Then
                                    groupNode.Nodes.Add(skillNode)
                                End If
                            End If

                        End If
                    End If
                End If
            Next
        End Sub
        Public Function HideSkill(skill As EveSkill) As Boolean
            If _hideQueuedSkills = False Then
                If _hideLevel5Skills = False Then
                    Return False
                Else
                    Return SkillFunctions.IsSkillTrained(_displayPilot, skill.Name, 5)
                End If
            Else
                Dim inQ As Boolean = False
                For Each skillQ As EveHQSkillQueue In _displayPilot.TrainingQueues.Values
                    If inQ = True Then Exit For
                    Dim sQ As Dictionary(Of String, EveHQSkillQueueItem) = skillQ.Queue
                    For Each skillQueueItem As EveHQSkillQueueItem In sQ.Values
                        If skill.Name = skillQueueItem.Name Then
                            inQ = True
                            Exit For
                        End If
                    Next
                Next
                If inQ = False Then
                    If _hideLevel5Skills = False Then
                        Return False
                    Else
                        Return SkillFunctions.IsSkillTrained(_displayPilot, skill.Name, 5)
                    End If
                Else
                    Return True
                End If
            End If
        End Function

        Private Sub ShowSkillGroups()
            For Each groupNode As Node In _skillListNodes.Values
                If groupNode.Nodes.Count > 0 Then
                    adtSkillList.Nodes.Add(groupNode)
                End If
            Next
        End Sub

        Public Sub RefreshTraining(ByVal queueName As String, updateColumnHeaders As Boolean)

            Dim ti As TabItem = tabQueues.Tabs.Item(queueName)
            If ti Is Nothing Then
                Exit Sub
            End If

            Dim tq As EveHQTrainingQueue = CType(tabQueues.Tabs.Item(queueName).AttachedControl, EveHQTrainingQueue)

            tq.DrawQueue(updateColumnHeaders)

            Call RedrawOptions()

            If _activeQueueControl IsNot Nothing Then
                _activeQueueControl.RedrawMenuOptions()
            End If

            ' Update the queue summary
            DrawQueueSummary()

        End Sub
        Private Sub RedrawOptions()
            ' Set the redraw flag to avoid triggering a recalc
            _redrawingOptions = True
            ' Determines what buttons and menus are available from the listview!
            If _activeQueueControl IsNot Nothing Then
                ' Check the queue status
                btnIncTraining.Checked = _activeQueueControl.IncludesCurrentTraining
                btnImplants.Enabled = True
                btnRemap.Enabled = True
                btnIncTraining.Enabled = True
                If _activeQueueTree.SelectedNodes.Count <> 0 Then
                    Select Case _activeQueueTree.SelectedNodes.Count
                        Case 1
                            Dim skillKey As String = _activeQueueTree.SelectedNodes(0).Name
                            Dim skillName As String
                            Dim curFLevel As Integer = CInt(skillKey.Substring(skillKey.Length - 2, 1))
                            Dim curTLevel As Integer = CInt(skillKey.Substring(skillKey.Length - 1, 1))
                            skillName = _activeQueueTree.SelectedNodes(0).Text
                            ' Check if we can increase or decrease levels

                            Dim curLevel As Integer

                            Dim mySkill As EveHQPilotSkill
                            If _displayPilot.PilotSkills.ContainsKey(skillName) = False Then
                                curLevel = 0
                            Else
                                mySkill = _displayPilot.PilotSkills(skillName)
                                curLevel = mySkill.Level
                            End If
                            btnRBIncreaseLevel.Enabled = True
                            btnRBDecreaseLevel.Enabled = True
                            btnRBDeleteSkill.Enabled = True
                            btnRBMoveUpQueue.Enabled = True
                            btnRBMoveDownQueue.Enabled = True
                            btnRBAddSkill.Enabled = False : btnRBSplitQueue.Enabled = True
                            If curTLevel = 5 Or curLevel = 5 Then
                                btnRBIncreaseLevel.Enabled = False
                            End If
                            If curTLevel - 1 <= curFLevel Or curTLevel <= curLevel Then
                                btnRBDecreaseLevel.Enabled = False
                            End If
                            If _activeQueueTree.SelectedNodes(0).Index = 0 Then
                                btnRBMoveUpQueue.Enabled = False
                            End If

                            ' Check if the skill is a pre-req
                            If _activeQueueTree.SelectedNodes(0).Style IsNot Nothing Then
                                If _activeQueueTree.SelectedNodes(0).Style.BackColor = Color.LightSteelBlue Then
                                    If _activeQueueTree.SelectedNodes(0).Cells(4).Text = "100" Then
                                        btnRBDeleteSkill.Enabled = True
                                    Else
                                        btnRBDeleteSkill.Enabled = False
                                    End If
                                End If
                            End If
                            ' Check if the skill is at the bottom of the list
                            If _activeQueueTree.SelectedNodes(0).Index = _activeQueueTree.Nodes.Count - 1 Then
                                btnRBMoveDownQueue.Enabled = False
                            End If

                            ' Adjust for if the training skill
                            If _displayPilot.Training = True And _activeQueueControl.IncludesCurrentTraining = True Then
                                If _activeQueueTree.SelectedNodes(0).Index = 0 Then
                                    btnRBIncreaseLevel.Enabled = False
                                    btnRBDecreaseLevel.Enabled = False
                                    btnRBDeleteSkill.Enabled = False
                                    btnRBMoveUpQueue.Enabled = False
                                    btnRBMoveDownQueue.Enabled = False
                                Else
                                    If _activeQueueTree.SelectedNodes(0).Index = 1 Then
                                        btnRBMoveUpQueue.Enabled = False
                                    End If
                                End If
                            End If
                        Case Is > 1
                            btnRBIncreaseLevel.Enabled = False
                            btnRBDecreaseLevel.Enabled = False
                            btnRBDeleteSkill.Enabled = True
                            btnRBMoveUpQueue.Enabled = False
                            btnRBMoveDownQueue.Enabled = False
                            btnRBAddSkill.Enabled = False : btnRBSplitQueue.Enabled = True
                            btnExportEMPFile.Enabled = True
                    End Select
                Else
                    btnRBIncreaseLevel.Enabled = False
                    btnRBDecreaseLevel.Enabled = False
                    btnRBDeleteSkill.Enabled = False
                    btnRBMoveUpQueue.Enabled = False
                    btnRBMoveDownQueue.Enabled = False
                    btnRBAddSkill.Enabled = False
                    btnRBSplitQueue.Enabled = False
                    btnExportEMPFile.Enabled = True
                End If
            Else
                btnRBIncreaseLevel.Enabled = False
                btnRBDecreaseLevel.Enabled = False
                btnRBDeleteSkill.Enabled = False
                btnRBMoveUpQueue.Enabled = False
                btnRBMoveDownQueue.Enabled = False
                btnRBAddSkill.Enabled = False
                btnRBSplitQueue.Enabled = False
                btnIncTraining.Checked = False
                btnIncTraining.Enabled = False
                btnRBClearQueue.Enabled = False
                btnImplants.Enabled = False
                btnRemap.Enabled = False
                btnExportEMPFile.Enabled = False
            End If
            btnAddRequisition.Enabled = False
            If _activeQueueControl IsNot Nothing Then
                For Each skill As EveHQSkillQueueItem In _activeQueueControl.Queue.Queue.Values
                    If SkillFunctions.IsSkillTrained(_displayPilot, skill.Name) = False Then
                        btnAddRequisition.Enabled = True
                        Exit For
                    End If
                Next
            End If
            ' Reset the redraw flag
            _redrawingOptions = False
        End Sub
        Public Sub UpdateTraining()
            ' Only perform this if the form isn't starting
            If _startup = False Then
                If _displayPilot.Training = True Then
                    If tabQueues.Tabs.Count > 1 Then
                        For Each ti As TabItem In tabQueues.Tabs
                            If ti.Name <> "tabSummary" Then
                                Dim tq As EveHQTrainingQueue = CType(tabQueues.Tabs.Item(ti.Name).AttachedControl, EveHQTrainingQueue)
                                Dim cLabel As Label = tq.lblQueueTime
                                Dim cQueue As AdvTree = tq.adtQueue
                                Dim newQ As EveHQSkillQueue = _displayPilot.TrainingQueues(ti.Name)
                                If newQ IsNot Nothing Then
                                    Dim bIncludeSkill As Boolean = newQ.IncCurrentTraining
                                    If bIncludeSkill Then
                                        If cQueue.Nodes.Count > 0 Then
                                            If HQ.SkillListID.ContainsKey(_displayPilot.TrainingSkillID) = True Then
                                                Dim myCurSkill As EveHQPilotSkill = _displayPilot.PilotSkills(SkillFunctions.SkillIDToName(_displayPilot.TrainingSkillID))
                                                Dim baseSkill As EveSkill = HQ.SkillListID(myCurSkill.ID)
                                                Dim clevel As Integer = _displayPilot.TrainingSkillLevel
                                                Dim cTime As Double = _displayPilot.TrainingCurrentTime
                                                Dim strTime As String = SkillFunctions.TimeToString(cTime)
                                                Dim percent As Integer
                                                percent = CInt(Int((myCurSkill.SP + _displayPilot.TrainingCurrentSP - baseSkill.LevelUp(clevel - 1)) / (baseSkill.LevelUp(clevel) - baseSkill.LevelUp(clevel - 1)) * 100))
                                                If (percent > 100) Then
                                                    percent = 100
                                                End If

                                                Dim lvi As Node = Nothing
                                                For Each lvt As Node In cQueue.Nodes
                                                    If lvt.Text = myCurSkill.Name Then
                                                        lvi = lvt
                                                        Exit For
                                                    End If
                                                Next
                                                If lvi.Cells.GetByColumnName("Percent") IsNot Nothing Then
                                                    lvi.Cells("Percent").Text = CStr(percent)
                                                End If
                                                If lvi.Cells.GetByColumnName("TrainTime") IsNot Nothing Then
                                                    lvi.Cells("TrainTime").Tag = cTime
                                                    lvi.Cells("TrainTime").Text = CStr(strTime)
                                                End If

                                                ' Calculate total time
                                                If cQueue.Nodes.Count > 0 Then
                                                    Dim totalTime As Double = 0
                                                    For count As Integer = 0 To cQueue.Nodes.Count - 1
                                                        If lvi.Cells.GetByColumnName("TrainTime") IsNot Nothing Then
                                                            totalTime += CLng(cQueue.Nodes(count).Cells("TrainTime").Tag)
                                                        End If
                                                    Next
                                                    cLabel.Tag = totalTime.ToString
                                                    cLabel.Text = SkillFunctions.TimeToString(totalTime)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                    If HQ.SkillListID.ContainsKey(_displayPilot.TrainingSkillID) = True Then
                        Dim cSkill As EveSkill = HQ.SkillListID(_displayPilot.TrainingSkillID)
                        If _displayPilot.Training = True And lvwDetails.Items(0).SubItems(1).Text = _displayPilot.TrainingSkillName Then
                            lvwDetails.Items(8).SubItems(1).Text = SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                            Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(cSkill.Name)
                            lvwDetails.Items(7).SubItems(1).Text = (mySkill.SP + _displayPilot.TrainingCurrentSP).ToString("N0")
                            Dim totalTime As Long = 0
                            For toLevel As Integer = 1 To 5
                                Select Case toLevel
                                    Case _displayPilot.TrainingSkillLevel
                                        totalTime += _displayPilot.TrainingCurrentTime
                                    Case Is > _displayPilot.TrainingSkillLevel
                                        totalTime = totalTime + SkillFunctions.CalcTimeToLevel(_displayPilot, cSkill, toLevel, toLevel - 1)
                                End Select
                                lvwTimes.Items(toLevel - 1).SubItems(3).Text = SkillFunctions.TimeToString(totalTime)
                            Next
                        End If
                    End If

                    ' Update the queue summary data
                    For Each newQ As EveHQSkillQueue In _displayPilot.TrainingQueues.Values
                        Try
                            Dim tq As EveHQTrainingQueue = CType(tabQueues.Tabs(newQ.Name).AttachedControl, EveHQTrainingQueue)
                            Dim tTime As Double = CDbl(tq.lblQueueTime.Tag)
                            lvQueues.Items(newQ.Name).SubItems(2).Tag = tTime
                            lvQueues.Items(newQ.Name).SubItems(2).Text = (SkillFunctions.TimeToString(tTime))
                            Dim qTime As Double = tTime
                            Dim bIncludeSkill As Boolean = newQ.IncCurrentTraining
                            If bIncludeSkill Then
                                qTime = tTime - _displayPilot.TrainingCurrentTime
                            End If
                            lvQueues.Items(newQ.Name).SubItems(3).Tag = qTime
                            lvQueues.Items(newQ.Name).SubItems(3).Text = SkillFunctions.TimeToString(qTime)
                            Dim eTime As Date = Now.AddSeconds(tTime)
                            lvQueues.Items(newQ.Name).SubItems(4).Text = (Format(eTime, "ddd") & " " & eTime.ToString)
                        Catch e As Exception
                            ' Error will most likely be if a skill queue is in the process of deletion.
                        End Try
                    Next
                    If _selQTime > 0 Then
                        lblTotalQueueTime.Text = "Selected Queue Time: " & SkillFunctions.TimeToString(_selQTime + _displayPilot.TrainingCurrentTime) & " (" & SkillFunctions.TimeToString(_selQTime) & ")"
                    Else
                        lblTotalQueueTime.Text = "No Queue Selected"
                    End If

                End If
            End If
        End Sub

#End Region

#Region "Skill Tree UI Functions"
        Private Sub cboFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFilter.SelectedIndexChanged
            Call LoadSkillTree()
        End Sub
        Private Sub adtSkillList_AfterNodeSelect(sender As Object, e As AdvTreeNodeEventArgs) Handles adtSkillList.AfterNodeSelect
            If e.Node IsNot Nothing Then
                btnRBIncreaseLevel.Enabled = False
                btnRBDecreaseLevel.Enabled = False
                btnRBDeleteSkill.Enabled = False
                btnRBMoveUpQueue.Enabled = False
                btnRBMoveDownQueue.Enabled = False
                If _activeQueueControl IsNot Nothing Then
                    btnRBAddSkill.Enabled = True
                Else
                    btnRBAddSkill.Enabled = False
                End If
                If e.Node.Level = 1 Or (e.Node.Level = 0 And _usingFilter = False) Then
                    Dim skillID As Integer = CInt(e.Node.Name)
                    Call ShowSkillDetails(skillID)
                End If
            End If
        End Sub
        Private Sub adtSkillList_NodeClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtSkillList.NodeClick
            adtSkillList.SelectedNode = e.Node
        End Sub
        Private Sub adtSkillList_NodeDoubleClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtSkillList.NodeDoubleClick
            If adtSkillList.SelectedNode.Level = 1 Or (adtSkillList.SelectedNode.Level = 0 And _usingFilter = False) Then
                Dim skillID As Integer = CInt(adtSkillList.SelectedNode.Name)
                FrmSkillDetails.DisplayPilotName = _displayPilot.Name
                Call FrmSkillDetails.ShowSkillDetails(skillID)
            End If
        End Sub
        Private Sub cboFilter_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFilter.TextChanged
            If cboFilter.Items.Contains(cboFilter.Text) = True Then
                _usingFilter = True
            Else
                _usingFilter = False
                Call LoadSkillTreeSearch()
            End If
        End Sub
        Private Sub chkOmitQueuedSkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOmitQueuedSkills.CheckedChanged
            _hideQueuedSkills = chkOmitQueuedSkills.Checked
            If _usingFilter = True Then
                Call LoadSkillTree()
            Else
                Call LoadSkillTreeSearch()
            End If
        End Sub
        Private Sub chkOmitLevel5Skills_CheckedChanged(sender As Object, e As EventArgs) Handles chkOmitLevel5Skills.CheckedChanged
            _hideLevel5Skills = chkOmitLevel5Skills.Checked
            If _usingFilter = True Then
                Call LoadSkillTree()
            Else
                Call LoadSkillTreeSearch()
            End If
        End Sub
        Private Sub LoadSkillTreeSearch()
            If Len(cboFilter.Text) > 1 Then
                Dim strSearch As String = cboFilter.Text.Trim.ToLower
                Dim results As New SortedList(Of String, String)
                Dim newSkill As EveSkill
                For Each newSkill In HQ.SkillListID.Values
                    If newSkill.Name.ToLower.Contains(strSearch) Or newSkill.Description.ToLower.Contains(strSearch) Then
                        results.Add(newSkill.Name, newSkill.Name)
                    End If
                Next

                adtSkillList.BeginUpdate()
                adtSkillList.Nodes.Clear()
                For Each item As String In results.Values
                    newSkill = HQ.SkillListName(item)
                    If newSkill.GroupID <> 505 And newSkill.Published = True Then
                        Dim skillNode As New Node
                        skillNode.Text = newSkill.Name
                        skillNode.Name = CStr(newSkill.ID)
                        skillNode.Cells.Add(New Cell(newSkill.Rank.ToString))
                        If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                            Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(newSkill.Name)
                            skillNode.ImageIndex = mySkill.Level
                        Else
                            skillNode.ImageIndex = 10
                        End If

                        If HideSkill(newSkill) = False Then
                            adtSkillList.Nodes.Add(skillNode)
                        End If

                    End If
                Next
                adtSkillList.EndUpdate()
            End If
        End Sub
        Private Sub btnExpandAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpandAll.Click
            adtSkillList.BeginUpdate()
            adtSkillList.ExpandAll()
            adtSkillList.EndUpdate()
        End Sub
        Private Sub btnCollapseAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCollapseAll.Click
            adtSkillList.BeginUpdate()
            adtSkillList.CollapseAll()
            adtSkillList.EndUpdate()
        End Sub
#End Region

#Region "Skill Queue UI Routines"
        Private Sub activeLVW_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            Select Case e.KeyCode
                Case Keys.Delete
                    If _activeQueueControl.Queue.IncCurrentTraining = True And _activeQueueTree.SelectedNodes.Count = 1 Then
                        If _activeQueueTree.SelectedNodes(0).Index = 0 Then
                            ' Do nothing
                        Else
                            Call _activeQueueControl.DeleteFromQueueOption()
                        End If
                    Else
                        Call _activeQueueControl.DeleteFromQueueOption()
                    End If
                Case Keys.Up
                    If e.Control = True Then
                        Call _activeQueueControl.MoveUpQueue()
                        e.SuppressKeyPress = True
                    End If
                Case Keys.Down
                    If e.Control = True Then
                        Call _activeQueueControl.MoveDownQueue()
                        e.SuppressKeyPress = True
                    End If
            End Select
        End Sub
        Private Sub activeLVW_DoubleClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim skillID As Integer = SkillFunctions.SkillNameToID(_activeQueueTree.SelectedNodes(0).Text)
            FrmSkillDetails.DisplayPilotName = _displayPilot.Name
            Call FrmSkillDetails.ShowSkillDetails(skillID)
        End Sub
        Private Sub activeLVW_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
            ' Make sure that the format is a treenode
            If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", False) = True Or e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", False) = True Then
                ' Allow drop.
                e.Effect = DragDropEffects.Copy
            Else
                ' Do not allow drop.
                e.Effect = DragDropEffects.None
            End If
        End Sub
        Private Sub activeLVW_ColumnClick(ByVal sender As Object, e As MouseEventArgs)

            Dim ch As ColumnHeader = CType(sender, ColumnHeader)

            ' Establish the sort direction and store it for later comparison and use
            Dim sortDirection As SortDirection
            If ch.SortDirection = eSortDirection.None Or ch.SortDirection = eSortDirection.Descending Then
                sortDirection = sortDirection.Ascending
                ch.SortDirection = eSortDirection.Ascending
            Else
                sortDirection = sortDirection.Descending
                ch.SortDirection = eSortDirection.Descending
            End If

            For Each col As ColumnHeader In _activeQueueTree.Columns
                col.SortDirection = eSortDirection.None
            Next
            ch.SortDirection = CType(sortDirection, eSortDirection)

            ' We are going to get the column text and base the sort on this!
            Select Case ch.Text
                Case "Skill Name"
                    Call SortQueue("Name", sortDirection) ' Sort on skill name
                Case "Cur Lvl"
                    Call SortQueue("CurLevel", sortDirection)
                Case "From Lvl"
                    Call SortQueue("FromLevel", sortDirection)
                Case "To Lvl"
                    Call SortQueue("ToLevel", sortDirection)
                Case "%"
                    Call SortQueue("Percent", sortDirection)
                Case "Training Time"
                    Call SortQueue("TrainTime", sortDirection) ' Sort on training time
                Case "Time to Complete"
                    Call SortQueue("TimeBeforeTrained", sortDirection) ' Sort on time to level
                Case "Date Completed"
                    ch.SortDirection = eSortDirection.None
                Case "Rank"
                    Call SortQueue("Rank", sortDirection)
                Case "Pri Attr"
                    Call SortQueue("PAtt", sortDirection)
                Case "Sec Attr"
                    Call SortQueue("SAtt", sortDirection)
                Case "SP /hour", "SP /day", "SP /week", "SP /mnth", "SP /year"
                    Call SortQueue("SPRate", sortDirection)
                Case "SP Added"
                    Call SortQueue("SPTrained", sortDirection)
                Case "SP @ End"
                    ch.SortDirection = eSortDirection.None
                Case "Notes"
                    Call SortQueue("Notes", sortDirection)
                Case "Priority"
                    Call SortQueue("Priority", sortDirection)
            End Select
        End Sub
        Private Sub activeLVW_Click(ByVal sender As Object, ByVal e As EventArgs)
            Call RedrawOptions()
            _activeQueueControl.RedrawMenuOptions()
        End Sub
        Private Sub activeLVW_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
            If _activeQueueTree.SelectedNodes.Count <> 0 Then
                Call RedrawOptions()
                _activeQueueControl.RedrawMenuOptions()
                Dim skillName As String = _activeQueueTree.SelectedNodes(0).Text
                Dim skillID As Integer = SkillFunctions.SkillNameToID(skillName)
                Call ShowSkillDetails(skillID)
            End If
        End Sub
        Private Sub SortQueue(ByVal primarySortColumn As String, ByVal sortDirection As SortDirection)
            ' Get the sorted queue list from the list of saved sorted queues
            Dim testQueue As ArrayList = SkillQueueFunctions.BuildQueue(_displayPilot, _activeQueueControl.Queue, False, True)
            ' Initialise a new ClassSorter instance and add a standard SortClass (i.e. sort method)
            Dim myClassSorter As New ClassSorter(primarySortColumn, sortDirection)
            ' Always sort by name to handle similarly ranked items in the first sort
            myClassSorter.SortClasses.Add(New SortClass("Name", sortDirection))
            ' Sort the class
            testQueue.Sort(myClassSorter)
            ' Call the TidyQueue function to set the pre-built queue to the revised sorted one
            SkillQueueFunctions.TidyQueue(_displayPilot, _activeQueueControl.Queue, testQueue)
            ' Now we need to refresh the queue again to calculate the correct skill orders and pre-reqs
            Call RefreshTraining(_activeQueueName, False)
        End Sub
#End Region

#Region "Certificate Planning"

        Private Sub cboCertFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboCertFilter.SelectedIndexChanged
            Call LoadCertificateTree()
        End Sub
        Public Sub LoadCertificateTree()
            Dim filter As Integer = cboCertFilter.SelectedIndex
            ' Save current open nodes
            _openNodes.Clear()
            For Each oNode As TreeNode In tvwCertList.Nodes
                If oNode.IsExpanded = True Then
                    _openNodes.Add(oNode.Name)
                End If
            Next
            tvwCertList.BeginUpdate()
            tvwCertList.Nodes.Clear()
            tvwCertList.Sorted = False
            Call LoadCertGroups()
            Call LoadFilteredCerts(filter)
            Call ShowCertGroups()
            For Each oNode As String In _openNodes
                If tvwCertList.Nodes.ContainsKey(oNode) = True Then
                    tvwCertList.Nodes(oNode).Expand()
                End If
            Next
            tvwCertList.Sorted = True
            tvwCertList.EndUpdate()
            tvwCertList.Refresh()
        End Sub
        Private Sub LoadCertGroups()
            _certListNodes.Clear()
            For Each certCat As CertificateCategory In StaticData.CertificateCategories.Values
                Dim groupNode As TreeNode = New TreeNode
                groupNode.Name = certCat.Id.ToString
                groupNode.Text = certCat.Name
                groupNode.ImageIndex = 8
                groupNode.SelectedImageIndex = 8
                _certListNodes.Add(groupNode.Name, groupNode)
            Next
        End Sub
        Private Sub LoadFilteredCerts(ByVal filter As Integer)
            Dim groupNode As TreeNode
            Dim addCert As Boolean
            For Each newCert As Certificate In StaticData.Certificates.Values
                For Each grade As CertificateGrade In newCert.GradesAndSkills.Keys
                    ' since each cert contains the 5 grades instead of having 5 different certs, loop for each grade
                    addCert = False
                    groupNode = CType(_certListNodes.Item(newCert.GroupID.ToString), TreeNode)
                    Select Case filter
                        Case 0
                            addCert = True
                        Case 1
                            If _displayPilot.QualifiedCertificates.ContainsKey(newCert.Id) = True Then
                                addCert = True
                            End If
                        Case 2
                            If _displayPilot.QualifiedCertificates.ContainsKey(newCert.Id) = False Then
                                addCert = True
                            End If


                        Case 3 To 7
                            If grade = filter - 2 Then
                                addCert = True
                            End If
                        Case 8 To 12
                            If grade = filter - 7 And _displayPilot.QualifiedCertificates.Contains(New KeyValuePair(Of Integer, CertificateGrade)(newCert.Id, grade)) = False Then
                                addCert = True
                            End If
                    End Select
                    If addCert = True Then
                        Dim certNode As New TreeNode
                        Dim rank = CInt(grade)
                        certNode.Text = newCert.Name & " (" & rank & " - " & _certGrades(rank) & ")"
                        certNode.Name = newCert.Id.ToString
                        If _displayPilot.QualifiedCertificates.Contains(New KeyValuePair(Of Integer, CertificateGrade)(newCert.Id, grade)) = True Then
                            certNode.ImageIndex = rank
                            certNode.SelectedImageIndex = rank
                        Else
                            certNode.ImageIndex = 10
                            certNode.SelectedImageIndex = 10
                            ' Check if we have the pre-reqs for the certificate
                            Dim canClaimCert As Boolean = True
                            For Each reqSkillAndLevel In newCert.GradesAndSkills(grade)
                                If SkillFunctions.IsSkillTrained(_displayPilot, SkillFunctions.SkillIDToName(reqSkillAndLevel.Key), reqSkillAndLevel.Value) = False Then
                                    canClaimCert = False
                                    Exit For
                                End If
                            Next

                            If canClaimCert = True Then
                                certNode.ForeColor = Color.LimeGreen
                            End If
                        End If
                        groupNode.Nodes.Add(certNode)
                    End If
                Next
            Next
        End Sub
        Private Sub ShowCertGroups()
            For Each groupNode As TreeNode In _certListNodes.Values
                If groupNode.Nodes.Count > 0 Then
                    tvwCertList.Nodes.Add(groupNode)
                End If
            Next
        End Sub

#End Region

#Region "Certificate Menu Routines"

        Private Sub tvwCertList_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwCertList.NodeMouseClick
            tvwCertList.SelectedNode = e.Node
        End Sub

        Private Sub ctxCertDetails_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxCertDetails.Opening
            Dim curNode As TreeNode
            curNode = tvwCertList.SelectedNode
            If curNode IsNot Nothing Then
                ' Reset grades
                For grade As Integer = 1 To 5
                    mnuAddCertToQueue.DropDownItems("mnuAddCertToQueue" & grade).Enabled = False
                Next
                Dim certName As String
                Dim certID As Integer
                certName = curNode.Text
                certID = CInt(curNode.Name)
                mnuCertName.Text = certName
                mnuCertName.Tag = certID
                ' Determine if this is a parent node or not
                If curNode.Parent Is Nothing Then
                    ' Group Node
                    mnuAddCertGroupToQueue.Visible = True
                    mnuAddCertToQueue.Visible = False
                Else
                    ' Skill Node
                    mnuAddCertGroupToQueue.Visible = False
                    mnuAddCertToQueue.Visible = True
                End If
                If _activeQueueName = "" Then
                    mnuAddCertGroupToQueue.Enabled = False
                    mnuAddCertToQueue.Enabled = False
                Else
                    mnuAddCertGroupToQueue.Enabled = True
                    mnuAddCertToQueue.Enabled = True
                    ' Determine enabled menu items of adding to queue
                    Dim selCert As Certificate = StaticData.Certificates(certID)
                    Dim selCertClass As Integer = selCert.Id
                    For Each testCert As Certificate In StaticData.Certificates.Values
                        If testCert.Id = selCertClass Then
                            For Each grade In testCert.GradesAndSkills.Keys

                                ' Check if the pilot has it
                                If _displayPilot.QualifiedCertificates.Contains(New KeyValuePair(Of Integer, CertificateGrade)(testCert.Id, grade)) = False Then
                                    mnuAddCertToQueue.DropDownItems("mnuAddCertToQueue" & grade.ToString()).Enabled = True
                                Else
                                    mnuAddCertToQueue.DropDownItems("mnuAddCertToQueue" & grade.ToString()).Enabled = False
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        End Sub

        Private Sub mnuViewCertDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewCertDetails.Click
            Dim certID As Integer = CInt(mnuCertName.Tag)
            FrmCertificateDetails.DisplayPilotName = _displayPilot.Name
            FrmCertificateDetails.ShowCertDetails(certID)
        End Sub

        Private Sub mnuAddCertToQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddCertToQueue1.Click, mnuAddCertToQueue2.Click, mnuAddCertToQueue3.Click, mnuAddCertToQueue4.Click, mnuAddCertToQueue5.Click
            ' Get the certificate details
            Dim grade As Integer = CInt(CType(sender, ToolStripItem).Name.Substring(CType(sender, ToolStripItem).Name.Length - 1, 1))
            Dim certID As Integer = CInt(mnuCertName.Tag)
            For Each cert As Certificate In StaticData.Certificates.Values
                If cert.Id = certID Then
                    Call AddCertSkills(cert, CType(grade, CertificateGrade))
                End If
            Next
            ' Refresh our training queue
            Call RefreshTraining(_activeQueueName, False)
        End Sub

        Private Sub AddCertSkills(ByVal cert As Certificate, ByVal grade As CertificateGrade)
            Dim reqSkills As SortedList(Of Integer, Integer) = cert.GradesAndSkills(grade)
            For Each reqSkill As Integer In reqSkills.Keys
                SkillQueueFunctions.AddSkillToQueue(_displayPilot, CStr(SkillFunctions.SkillIDToName(reqSkill)), _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, CInt(reqSkills(reqSkill)), True, True, "Certificate: " & cert.Name)
            Next
        End Sub

        Private Sub mnuAddCertGroupToQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddCertGroupToQueue1.Click, mnuAddCertGroupToQueue2.Click, mnuAddCertGroupToQueue3.Click, mnuAddCertGroupToQueue4.Click, mnuAddCertGroupToQueue5.Click
            ' Get the Grade required
            Dim grade As Integer = CInt(CType(sender, ToolStripItem).Name.Substring(CType(sender, ToolStripItem).Name.Length - 1, 1))
            Dim certCat As String = mnuCertName.Tag.ToString
            For Each cert As Certificate In StaticData.Certificates.Values
                If cert.GroupID = CInt(certCat) Then
                    Call AddCertSkills(cert, CType(grade, CertificateGrade))
                End If
            Next
            ' Refresh our training queue
            Call RefreshTraining(_activeQueueName, False)
        End Sub

#End Region

#Region "Skill Information Display"

        Private Sub ShowSkillDetails(ByVal skillID As Integer)

            Call PrepareDetails(skillID)
            Call PrepareTree(skillID)
            Call PrepareDepends(skillID)
            Call PrepareDescription(skillID)
            Call PrepareSPs(skillID)
            Call PrepareTimes(skillID)

        End Sub
        Private Sub PrepareDetails(ByVal skillID As Integer)

            Dim cSkill As EveSkill = HQ.SkillListID(skillID)
            lvwDetails.Groups(1).Header = "Pilot Specific - " & _displayPilot.Name

            With lvwDetails
                Dim mySkill As EveHQPilotSkill
                Dim myGroup As SkillGroup
                If StaticData.TypeGroups.ContainsKey(CInt(cSkill.GroupID)) = True Then
                    Dim groupName As String = StaticData.TypeGroups(CInt(cSkill.GroupID))
                    If HQ.SkillGroups.ContainsKey(groupName) = True Then
                        myGroup = HQ.SkillGroups(groupName)
                    Else
                        myGroup = Nothing
                    End If
                Else
                    myGroup = Nothing
                End If
                Dim cLevel, cSP, cTime, cRate As String
                If HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                    If _displayPilot.PilotSkills.ContainsKey(cSkill.Name) = False Then
                        cLevel = "0" : cSP = "0" : cTime = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(_displayPilot, cSkill, 1, ))
                        cRate = CStr(SkillFunctions.CalculateSPRate(_displayPilot, cSkill))
                    Else
                        mySkill = _displayPilot.PilotSkills(cSkill.Name)
                        cLevel = mySkill.Level.ToString
                        If _displayPilot.Training = True And _displayPilot.TrainingSkillID = SkillFunctions.SkillNameToID(cSkill.Name) Then
                            cSP = CStr(mySkill.SP + _displayPilot.TrainingCurrentSP)
                        Else
                            cSP = mySkill.SP.ToString
                        End If
                        If _displayPilot.Training = True And _displayPilot.TrainingSkillName = cSkill.Name Then
                            cTime = SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                        Else
                            cTime = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(_displayPilot, cSkill, 0, ))
                        End If
                        cRate = CStr(SkillFunctions.CalculateSPRate(_displayPilot, cSkill))
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
                .Items(6).SubItems(1).Text = cLevel
                .Items(7).SubItems(1).Text = CLng(cSP).ToString("N0")
                .Items(8).SubItems(1).Text = cTime
                .Items(9).SubItems(1).Text = CDbl(cRate).ToString("N0")
            End With

        End Sub
        Private Sub PrepareTree(ByVal skillID As Integer)
            tvwReqs.BeginUpdate()
            tvwReqs.Nodes.Clear()

            Dim cSkill As EveSkill = HQ.SkillListID(skillID)
            Const CurLevel As Integer = 0
            Dim curNode As TreeNode = New TreeNode

            ' Write the skill we are querying as the first (parent) node
            curNode.Text = cSkill.Name
            Dim skillTrained As Boolean = False
            Dim myLevel As Integer
            If HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                If _displayPilot.PilotSkills.ContainsKey(cSkill.Name) Then
                    Dim mySkill As EveHQPilotSkill
                    mySkill = _displayPilot.PilotSkills(cSkill.Name)
                    myLevel = CInt(mySkill.Level)
                    If myLevel >= CurLevel Then skillTrained = True
                    If skillTrained = True Then
                        curNode.ForeColor = Color.LimeGreen
                        curNode.ToolTipText = "Already Trained"
                    Else
                        Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, CurLevel)
                        If planLevel = 0 Then
                            curNode.ForeColor = Color.Red
                            curNode.ToolTipText = "Not trained & no planned training"
                        Else
                            curNode.ToolTipText = "Planned training to Level " & planLevel
                            If planLevel >= CurLevel Then
                                curNode.ForeColor = Color.Blue
                            Else
                                curNode.ForeColor = Color.Orange
                            End If
                        End If
                    End If
                Else
                    Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, CurLevel)
                    If planLevel = 0 Then
                        curNode.ForeColor = Color.Red
                        curNode.ToolTipText = "Not trained & no planned training"
                    Else
                        curNode.ToolTipText = "Planned training to Level " & planLevel
                        If planLevel >= CurLevel Then
                            curNode.ForeColor = Color.Blue
                        Else
                            curNode.ForeColor = Color.Orange
                        End If
                    End If
                End If
            End If
            tvwReqs.Nodes.Add(curNode)

            If cSkill.PreReqSkills.Count > 0 Then
                Dim subSkill As EveSkill
                For Each subSkillID As Integer In cSkill.PreReqSkills.Keys
                    subSkill = HQ.SkillListID(subSkillID)
                    Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                Next
            End If
            tvwReqs.ExpandAll()
            tvwReqs.EndUpdate()
        End Sub
        Private Sub AddPreReqsToTree(ByVal newSkill As EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode)
            Dim skillTrained As Boolean
            Dim myLevel As Integer
            Dim newNode As TreeNode = New TreeNode
            newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
            newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
            ' Check status of this skill
            If HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                skillTrained = False
                If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) Then
                    Dim mySkill As EveHQPilotSkill
                    mySkill = _displayPilot.PilotSkills(newSkill.Name)
                    myLevel = CInt(mySkill.Level)
                    If myLevel >= curLevel Then skillTrained = True
                End If
                If skillTrained = True Then
                    newNode.ForeColor = Color.LimeGreen
                    newNode.ToolTipText = "Already Trained"
                Else
                    Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_displayPilot, newSkill.Name, curLevel)
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
                Dim subSkill As EveSkill
                For Each subSkillID As Integer In newSkill.PreReqSkills.Keys
                    subSkill = HQ.SkillListID(subSkillID)
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
            Dim itemData(1) As String
            Dim skillData(1) As String
            For lvl As Integer = 1 To 5
                If StaticData.SkillUnlocks.ContainsKey(skillID & "." & CStr(lvl)) = True Then
                    Dim itemUnlocked As List(Of String) = StaticData.SkillUnlocks(skillID & "." & CStr(lvl))
                    For Each item As String In itemUnlocked
                        Dim newItem As New ListViewItem
                        Dim toolTipText As New StringBuilder
                        itemData = item.Split(CChar("_"))
                        catID = StaticData.GroupCats.Item(CInt(itemData(1)))
                        newItem.Group = lvwDepend.Groups("Cat" & catID)
                        newItem.Text = StaticData.Types(CInt(itemData(0))).Name
                        newItem.Name = itemData(0)
                        newItem.Tag = itemData(0)
                        Dim skillUnlocked As List(Of String) = StaticData.ItemUnlocks(itemData(0))
                        Dim allTrained As Boolean = True
                        For Each skillPair As String In skillUnlocked
                            skillData = skillPair.Split(CChar("."))
                            skillName = SkillFunctions.SkillIDToName(CInt(skillData(0)))
                            If CInt(skillData(0)) <> skillID Then
                                toolTipText.Append(skillName)
                                toolTipText.Append(" (Level ")
                                toolTipText.Append(skillData(1))
                                toolTipText.Append("), ")
                            End If
                            If SkillFunctions.IsSkillTrained(_displayPilot, skillName, CInt(skillData(1))) = False Then
                                allTrained = False
                            End If
                        Next
                        If toolTipText.Length > 0 Then
                            toolTipText.Insert(0, "Also Requires: ")

                            If toolTipText.ToString().EndsWith(", ", StringComparison.Ordinal) Then
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
                        newItem.Group = lvwDepend.Groups("CatCerts")
                        Dim cert As Certificate = StaticData.Certificates(item)
                        For Each grade In cert.GradesAndSkills.Keys
                            newItem.Tag = cert.Id
                            certName = cert.Name

                            If newItem.ToolTipText <> "" Then
                                newItem.ToolTipText = "Also Requires: " & newItem.ToolTipText
                                newItem.ToolTipText = newItem.ToolTipText.TrimEnd(", ".ToCharArray)
                            End If
                            If _displayPilot.QualifiedCertificates.ContainsKey(cert.Id) = True Then
                                newItem.ForeColor = Color.Green
                            Else
                                newItem.ForeColor = Color.Red
                            End If
                            newItem.Text = certName & " (" & grade.ToString() & ")"
                            newItem.Name = CStr(item)
                            newItem.SubItems.Add("Level " & lvl)
                            lvwDepend.Items.Add(newItem)
                        Next
                    Next
                End If
            Next
            lvwDepend.EndUpdate()
        End Sub
        Private Sub PrepareDescription(ByVal skillID As Integer)
            Dim cSkill As EveSkill = HQ.SkillListID(skillID)
            lblDescription.Text = cSkill.Description
        End Sub
        Private Sub PrepareSPs(ByVal skillID As Integer)
            lvwSPs.BeginUpdate()
            lvwSPs.Items.Clear()
            Dim cSkill As EveSkill = HQ.SkillListID(skillID)
            Dim lastSP As Long = 0
            For toLevel As Integer = 1 To 5
                Dim newGroup As ListViewItem = New ListViewItem
                newGroup.Text = toLevel.ToString
                Dim sp As Long = CLng(Math.Ceiling(SkillFunctions.CalculateSPLevel(cSkill.Rank, toLevel)))
                newGroup.SubItems.Add(sp.ToString("N0"))
                newGroup.SubItems.Add((sp - lastSP).ToString("N0"))
                lastSP = sp
                lvwSPs.Items.Add(newGroup)
            Next
            lvwSPs.EndUpdate()
        End Sub
        Private Sub PrepareTimes(ByVal skillID As Integer)
            lvwTimes.BeginUpdate()
            lvwTimes.Items.Clear()

            If HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                Dim cskill As EveSkill = HQ.SkillListID(skillID)

                For toLevel As Integer = 1 To 5
                    Dim newGroup As ListViewItem = New ListViewItem
                    newGroup.Text = toLevel.ToString
                    newGroup.SubItems.Add(SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(_displayPilot, cskill, toLevel, toLevel - 1)))
                    newGroup.SubItems.Add(SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(_displayPilot, cskill, toLevel, 0)))
                    newGroup.SubItems.Add(SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(_displayPilot, cskill, toLevel, -1)))
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
        Private Sub tvwReqs_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles tvwReqs.MouseMove
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
        Private Sub tvwReqs_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwReqs.NodeMouseClick
            tvwReqs.SelectedNode = e.Node
        End Sub
        Private Sub mnuViewItemDetailsInIB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewItemDetailsInIB.Click

            Dim typeID As Integer = CInt(mnuItemName.Tag)
            Using myIB As New FrmIB(typeID)
                myIB.ShowDialog()
            End Using

        End Sub
        Private Sub ctxDepend_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxDepend.Opening
            If ctxDepend.SourceControl Is lvwDepend Then
                If lvwDepend.SelectedItems.Count <> 0 Then
                    Dim item As ListViewItem = lvwDepend.SelectedItems(0)
                    Dim itemName As String = item.Text
                    Dim itemID As String = item.Tag.ToString

                    If item.Group.Name = "Cat16" Then
                        mnuViewItemDetails.Visible = True
                        mnuViewItemDetailsHere.Visible = True
                    Else
                        mnuViewItemDetails.Visible = False
                        mnuViewItemDetailsHere.Visible = False
                    End If
                    mnuViewItemDetailsInIB.Visible = Not (item.Group.Name = "CatCerts")
                    mnuViewItemDetailsInCertScreen.Visible = (item.Group.Name = "CatCerts")

                    mnuItemName.Text = itemName
                    mnuItemName.Tag = itemID
                Else
                    e.Cancel = True
                End If
            End If
        End Sub
        Public Sub UpdateSkillDetails()
            If _displayPilot.Training = True Then
                Dim cSkill As EveSkill = HQ.SkillListID(_displayPilot.TrainingSkillID)
                If _displayPilot.Training = True And lvwDetails.Items(0).SubItems(1).Text = _displayPilot.TrainingSkillName Then
                    lvwDetails.Items(8).SubItems(1).Text = SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                    Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(cSkill.Name)
                    lvwDetails.Items(7).SubItems(1).Text = (mySkill.SP + _displayPilot.TrainingCurrentSP).ToString("N0")
                    Dim totalTime As Long = 0
                    For toLevel As Integer = 1 To 5
                        Select Case toLevel
                            Case _displayPilot.TrainingSkillLevel
                                totalTime += _displayPilot.TrainingCurrentTime
                            Case Is > _displayPilot.TrainingSkillLevel
                                totalTime = totalTime + SkillFunctions.CalcTimeToLevel(_displayPilot, cSkill, toLevel, toLevel - 1)
                        End Select
                        lvwTimes.Items(toLevel - 1).SubItems(3).Text = SkillFunctions.TimeToString(totalTime)
                    Next
                End If
            End If
        End Sub

#End Region

#Region "Skill Queue Modification Functions"

        Private Sub AddSkillToQueueOption()
            _activeQueueControl.Queue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, adtSkillList.SelectedNode.Text, _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, 0, False, False, "")
            Call RefreshTraining(_activeQueueName, False)
        End Sub

#End Region

#Region "Skill Queue Summary UI Functions"

        Private Sub DrawQueueSummary()
            ' Check if we have a Primary Queue
            If _displayPilot.TrainingQueues.Count > 0 Then
                If _displayPilot.PrimaryQueue = "" Then
                    _displayPilot.PrimaryQueue = _displayPilot.TrainingQueues.Keys(0).ToString
                    Dim selQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(_displayPilot.PrimaryQueue)
                    selQueue.Primary = True
                End If
            End If
            ' Setup the summary column
            lvQueues.BeginUpdate()
            lvQueues.Items.Clear()
            For Each newQ As EveHQSkillQueue In _displayPilot.TrainingQueues.Values
                Dim newItem As New ListViewItem
                Dim primaryFont As Font = New Font(newItem.Font, FontStyle.Bold)
                newItem.Name = newQ.Name
                newItem.Text = newQ.Name
                If newQ.Primary = True Then
                    newItem.Font = primaryFont
                End If
                newItem.SubItems.Add(newQ.Queue.Count.ToString)
                Dim tq As EveHQTrainingQueue = CType(tabQueues.Tabs.Item(newQ.Name).AttachedControl, EveHQTrainingQueue)
                Dim tTime As Double = CDbl(tq.lblQueueTime.Tag)
                Dim tTimeItem As New ListViewItem.ListViewSubItem
                tTimeItem.Tag = tTime
                tTimeItem.Text = SkillFunctions.TimeToString(tTime)
                newItem.SubItems.Add(tTimeItem)
                Dim qTime As Double
                If _displayPilot.Training = True And newQ.IncCurrentTraining = True Then
                    qTime = tTime - _displayPilot.TrainingCurrentTime
                Else
                    qTime = tTime
                End If
                Dim qTimeItem As New ListViewItem.ListViewSubItem
                qTimeItem.Tag = tTime
                qTimeItem.Text = SkillFunctions.TimeToString(qTime)
                newItem.SubItems.Add(qTimeItem)
                Dim eTime As Date = Now.AddSeconds(tTime)
                newItem.SubItems.Add(Format(eTime, "ddd") & " " & eTime.ToString)
                lvQueues.Items.Add(newItem)
            Next
            lvQueues.EndUpdate()
        End Sub

        Private Sub GetSelectedQueueTimes()
            Try
                Dim newQueue As New EveHQSkillQueue
                newQueue.Name = "tempMerge"
                newQueue.IncCurrentTraining = True
                newQueue.Primary = False
                newQueue.Queue = New Dictionary(Of String, EveHQSkillQueueItem)
                For Each item As ListViewItem In lvQueues.SelectedItems
                    Dim queueName As String = item.Name
                    Dim oldQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(queueName)
                    If oldQueue.Primary = True Then newQueue.Primary = True
                    For Each queueItem As EveHQSkillQueueItem In oldQueue.Queue.Values
                        Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                        If newQueue.Queue.ContainsKey(keyName) = False Then
                            newQueue.Queue.Add(keyName, queueItem)
                        End If
                    Next
                Next
                Dim curSkill As EveHQSkillQueueItem
                For Each curSkill In newQueue.Queue.Values
                    If _displayPilot.PilotSkills.ContainsKey(curSkill.Name) Then
                        Dim toLevel As Integer = curSkill.ToLevel
                        Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(curSkill.Name)
                        Dim pilotLevel As Integer = mySkill.Level
                        If pilotLevel >= toLevel Then
                            Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                            newQueue.Queue.Remove(oldKey)
                        End If
                    End If
                Next
                Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(_displayPilot, newQueue, True, True)
                Dim qItem As SortedQueueItem
                Dim qTime As Double = 0
                For Each qItem In arrQueue
                    qTime += CLng(qItem.TrainTime)
                Next
                _selQTime = qTime - _displayPilot.TrainingCurrentTime
                lblTotalQueueTime.Text = "Selected Queue Time: " & SkillFunctions.TimeToString(_selQTime + _displayPilot.TrainingCurrentTime) & " (" & SkillFunctions.TimeToString(_selQTime) & ")"
            Catch e As Exception
                MessageBox.Show("There was an error calculating the selected queue times.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub
        Private Sub lvQueues_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lvQueues.Click
            Select Case lvQueues.SelectedItems.Count
                Case 0
                    Call ResetQueueOptions()
                    _selQTime = 0
                    lblTotalQueueTime.Text = "No Queue Selected"
                Case 1
                    ' Set Buttons
                    btnRBAddQueue.Enabled = True
                    btnRBEditQueue.Enabled = True
                    btnRBDeleteQueue.Enabled = True
                    btnRBCopyQueue.Enabled = True
                    btnRBCopyQueueToPilot.Enabled = True
                    btnRBMergeQueues.Enabled = False
                    btnRBSetPrimaryQueue.Enabled = True
                    ' Set Menus
                    mnuAddQueue.Enabled = True
                    mnuEditQueue.Enabled = True
                    mnuDeleteQueue.Enabled = True
                    mnuCopyQueue.Enabled = True
                    mnuCopyQueueToPilot.Enabled = True
                    mnuMergeQueues.Enabled = False
                    mnuSetPrimary.Enabled = True
                    ' Display queue times
                    _selQTime = CDbl(lvQueues.Items(lvQueues.SelectedIndices(0)).SubItems(2).Tag) - _displayPilot.TrainingCurrentTime
                    lblTotalQueueTime.Text = "Selected Queue Time: " & SkillFunctions.TimeToString(_selQTime + _displayPilot.TrainingCurrentTime) & " (" & SkillFunctions.TimeToString(_selQTime) & ")"
                Case Is > 1
                    ' Set Buttons
                    btnRBAddQueue.Enabled = True
                    btnRBEditQueue.Enabled = False
                    btnRBDeleteQueue.Enabled = False
                    btnRBCopyQueue.Enabled = False
                    btnRBCopyQueueToPilot.Enabled = False
                    btnRBMergeQueues.Enabled = True
                    btnRBSetPrimaryQueue.Enabled = False
                    ' Set Menus
                    mnuAddQueue.Enabled = True
                    mnuEditQueue.Enabled = False
                    mnuDeleteQueue.Enabled = False
                    mnuCopyQueue.Enabled = False
                    mnuCopyQueueToPilot.Enabled = False
                    mnuMergeQueues.Enabled = True
                    mnuSetPrimary.Enabled = False
                    ' Display queue times
                    Call GetSelectedQueueTimes()
            End Select
        End Sub
        Private Sub lvQueues_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles lvQueues.DoubleClick
            Dim selQ As String = lvQueues.SelectedItems(0).Text
            Dim ti As TabItem = tabQueues.Tabs.Item(selQ)
            tabQueues.SelectedTab = ti
        End Sub
        Public Sub ResetQueueOptions()
            ' Set Buttons
            btnRBAddQueue.Enabled = True
            btnRBEditQueue.Enabled = False
            btnRBDeleteQueue.Enabled = False
            btnRBCopyQueue.Enabled = False
            btnRBCopyQueueToPilot.Enabled = False
            btnRBMergeQueues.Enabled = False
            btnRBSetPrimaryQueue.Enabled = False
            btnRemap.Enabled = False
            btnImplants.Enabled = False
            ' Set Menus
            mnuAddQueue.Enabled = True
            mnuEditQueue.Enabled = False
            mnuDeleteQueue.Enabled = False
            mnuCopyQueue.Enabled = False
            mnuCopyQueueToPilot.Enabled = False
            mnuMergeQueues.Enabled = False
            mnuSetPrimary.Enabled = False
        End Sub
#End Region

#Region "Skill Tree Context Menu Functions"
        Private Sub ctxDetails_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxDetails.Opening
            Dim curNode As Node
            curNode = adtSkillList.SelectedNode
            If curNode IsNot Nothing Then
                Dim skillName As String
                Dim skillID As Integer
                skillName = curNode.Text
                skillID = SkillFunctions.SkillNameToID(skillName)
                mnuSkillName2.Text = skillName
                mnuSkillName2.Tag = skillID
                ' Determine if this is a parent node or not
                If curNode.Parent Is Nothing And _usingFilter = True Then
                    ' Group Node
                    mnuAddGroupToQueue.Visible = True
                    mnuAddToQueue.Visible = False
                Else
                    ' Skill Node
                    mnuAddGroupToQueue.Visible = False
                    mnuAddToQueue.Visible = True
                End If
                If _activeQueueName = "" Then
                    mnuAddGroupToQueue.Enabled = False
                    mnuAddToQueue.Enabled = False
                Else
                    mnuAddGroupToQueue.Enabled = True
                    mnuAddToQueue.Enabled = True
                    ' Determine enabled menu items of adding to queue
                    Dim currentLevel As Integer = 0
                    If _displayPilot.PilotSkills.ContainsKey(skillName) = True Then
                        Dim cSkill As EveHQPilotSkill = _displayPilot.PilotSkills(skillName)
                        currentLevel = cSkill.Level
                    End If
                    For a As Integer = 1 To 5
                        If a <= currentLevel Then
                            mnuAddToQueue.DropDownItems("mnuAddToQueue" & a).Enabled = False
                        Else
                            mnuAddToQueue.DropDownItems("mnuAddToQueue" & a).Enabled = True
                        End If
                    Next
                    If currentLevel = 5 Then
                        mnuAddToQueueNext.Enabled = False
                        mnuAddToQueue.Enabled = False
                    Else
                        mnuAddToQueueNext.Enabled = True
                        mnuAddToQueue.Enabled = True
                    End If
                End If
            End If
        End Sub
        Private Sub mnuViewDetails2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewDetails2.Click
            Dim skillID As Integer = CInt(mnuSkillName2.Tag)
            FrmSkillDetails.DisplayPilotName = _displayPilot.Name
            Call FrmSkillDetails.ShowSkillDetails(skillID)
        End Sub
        Private Sub mnuAddToQueueNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToQueueNext.Click
            If _activeQueueControl IsNot Nothing Then
                Call AddSkillToQueueOption()
            End If
        End Sub
        Private Sub mnuAddToQueue1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToQueue1.Click
            If _activeQueueControl IsNot Nothing Then
                If adtSkillList.SelectedNode IsNot Nothing Then
                    _activeQueueControl.Queue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, adtSkillList.SelectedNode.Text, _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, 1, False, False, "")
                    Call RefreshTraining(_activeQueueName, False)
                End If
            End If
        End Sub
        Private Sub mnuAddToQueue2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToQueue2.Click
            If _activeQueueControl.Queue IsNot Nothing Then
                If adtSkillList.SelectedNode IsNot Nothing Then
                    _activeQueueControl.Queue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, adtSkillList.SelectedNode.Text, _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, 2, False, False, "")
                    Call RefreshTraining(_activeQueueName, False)
                End If
            End If
        End Sub
        Private Sub mnuAddToQueue3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToQueue3.Click
            If _activeQueueControl.Queue IsNot Nothing Then
                If adtSkillList.SelectedNode IsNot Nothing Then
                    _activeQueueControl.Queue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, adtSkillList.SelectedNode.Text, _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, 3, False, False, "")
                    Call RefreshTraining(_activeQueueName, False)
                End If
            End If
        End Sub
        Private Sub mnuAddToQueue4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToQueue4.Click
            If adtSkillList.SelectedNode IsNot Nothing Then
                If _activeQueueControl.Queue IsNot Nothing Then
                    _activeQueueControl.Queue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, adtSkillList.SelectedNode.Text, _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, 4, False, False, "")
                    Call RefreshTraining(_activeQueueName, False)
                End If
            End If
        End Sub
        Private Sub mnuAddToQueue5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToQueue5.Click
            If adtSkillList.SelectedNode IsNot Nothing Then
                If _activeQueueControl.Queue IsNot Nothing Then
                    _activeQueueControl.Queue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, adtSkillList.SelectedNode.Text, _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, 5, False, False, "")
                    Call RefreshTraining(_activeQueueName, False)
                End If
            End If
        End Sub
        Private Sub mnuAddGroupToQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddGroupLevel1.Click, mnuAddGroupLevel2.Click, mnuAddGroupLevel3.Click, mnuAddGroupLevel4.Click, mnuAddGroupLevel5.Click
            If _activeQueueControl.Queue IsNot Nothing Then
                Dim men As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
                Dim level As Integer = CInt(men.Text.Replace("To Level ", ""))
                Dim parentNode As Node
                Dim curNode As Node
                If adtSkillList.SelectedNode IsNot Nothing Then
                    parentNode = adtSkillList.SelectedNode
                    For Each curNode In parentNode.Nodes
                        _activeQueueControl.Queue = SkillQueueFunctions.AddSkillToQueue(_displayPilot, curNode.Text, _activeQueueControl.Queue.Queue.Count + 1, _activeQueueControl.Queue, level, True, True, "")
                    Next
                    Call RefreshTraining(_activeQueueName, False)
                End If
            End If
        End Sub
        Private Sub mnuExpandAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExpandAll.Click
            adtSkillList.BeginUpdate()
            adtSkillList.ExpandAll()
            adtSkillList.EndUpdate()
        End Sub
        Private Sub mnuCollapseAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuCollapseAll.Click
            adtSkillList.BeginUpdate()
            adtSkillList.CollapseAll()
            adtSkillList.EndUpdate()
        End Sub
#End Region

#Region "Skill Info Context Menu Functions"
        Private Sub ctxReqs_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxReqs.Opening
            Dim curNode As TreeNode = tvwReqs.SelectedNode
            If curNode IsNot Nothing Then
                Dim skillName As String
                skillName = curNode.Text
                If InStr(skillName, "(Level") <> 0 Then
                    skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
                End If
                Dim skillID As Integer = SkillFunctions.SkillNameToID(skillName)
                mnuReqsSkillName.Text = skillName
                mnuReqsSkillName.Tag = skillID
            Else
                e.Cancel = True
            End If
        End Sub
        Private Sub mnuViewItemDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewItemDetails.Click
            Dim skillID As Integer = CInt(mnuItemName.Tag)
            FrmSkillDetails.DisplayPilotName = _displayPilot.Name
            Call FrmSkillDetails.ShowSkillDetails(skillID)
        End Sub
        Private Sub mnuViewItemDetailsHere_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewItemDetailsHere.Click
            Dim skillID As Integer = CInt(mnuItemName.Tag)
            Call ShowSkillDetails(skillID)
        End Sub
        Private Sub mnuReqsViewDetailsHere_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuReqsViewDetailsHere.Click
            Dim skillID As Integer = CInt(mnuReqsSkillName.Tag)
            Call ShowSkillDetails(skillID)
        End Sub
        Private Sub mnuReqsViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuReqsViewDetails.Click
            Dim skillID As Integer = CInt(mnuReqsSkillName.Tag)
            FrmSkillDetails.DisplayPilotName = _displayPilot.Name
            Call FrmSkillDetails.ShowSkillDetails(skillID)
        End Sub
        Private Sub mnuViewItemDetailsInCertScreen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewItemDetailsInCertScreen.Click
            Dim certID As Integer = CInt(mnuItemName.Tag)
            FrmCertificateDetails.DisplayPilotName = _displayPilot.Name
            FrmCertificateDetails.ShowCertDetails(certID)
        End Sub
#End Region

#Region "Skill Queue Import/Export"

        Private Sub ImportAllEveMonPlans()
            ' Set recalc flag
            Dim recalcQueues As Boolean = False
            ' Try to find the EveMon settings file
            Dim eveMonLocation As String = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveMon"), "Settings.xml")
            If My.Computer.FileSystem.FileExists(eveMonLocation) = False Then
                ' Try for the settings-debug file which is common during development
                eveMonLocation = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveMon"), "Settings-debug.xml")
                If My.Computer.FileSystem.FileExists(eveMonLocation) = False Then
                    MessageBox.Show("EveMon Settings File Not Found." & ControlChars.CrLf & ControlChars.CrLf & "Please check the EveMon installation.", "EveMon Settings Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
            End If
            Try
                ' Load the Settings file into an XMLDocument
                Dim emxml As New XmlDocument
                emxml.Load(eveMonLocation)
                ' Get a list of the characters that are in files (not API)
                Dim emPilots As New SortedList
                Dim charDetails As XmlNodeList
                Dim charNode As XmlNode
                ' Get details of characters assigned to an API account
                charDetails = emxml.SelectNodes("Settings/characters/ccp")
                If charDetails.Count > 0 Then
                    ' Need to add details of pilots here
                    For Each charNode In charDetails
                        emPilots.Add(charNode.Attributes.GetNamedItem("guid").Value, charNode.ChildNodes(1).InnerText) ' Adds the GUID and name
                    Next
                End If
                ' Get details of characters added by a file
                charDetails = emxml.SelectNodes("Settings/characters/uri")
                If charDetails.Count > 0 Then
                    ' Need to add details of pilots here
                    For Each charNode In charDetails
                        emPilots.Add(charNode.Attributes.GetNamedItem("guid").Value, charNode.ChildNodes(1).InnerText) ' Adds the GUID and name
                    Next
                End If

                ' Try and get the plan information
                Dim planDetails As XmlNodeList
                Dim planNode As XmlNode
                Dim plansItemNode As XmlNode
                planDetails = emxml.SelectNodes("Settings/plans/plan")
                If planDetails.Count > 0 Then
                    Dim planInfo(planDetails.Count, 2) As String
                    Dim count As Integer = -1
                    For Each planNode In planDetails
                        count += 1
                        Dim planOwner As String = planNode.Attributes.GetNamedItem("owner").Value
                        If emPilots.ContainsKey(planOwner) = True Then
                            Dim pilotName As String = CStr(emPilots(planOwner))
                            If HQ.Settings.Pilots.ContainsKey(pilotName) = True Then
                                Dim planName As String = planNode.Attributes.GetNamedItem("name").Value
                                planInfo(count, 0) = pilotName : planInfo(count, 1) = planName
                                Dim sq As New Dictionary(Of String, EveHQSkillQueueItem)
                                Dim sqCount As Integer = 0
                                Dim planItems As XmlNodeList = planNode.SelectNodes("entry")
                                If planItems.Count > 0 Then
                                    For Each plansItemNode In planItems
                                        sqCount += 1
                                        Dim sqi As New EveHQSkillQueueItem
                                        sqi.Name = plansItemNode.Attributes.GetNamedItem("skill").Value
                                        sqi.ToLevel = CInt(plansItemNode.Attributes.GetNamedItem("level").Value)
                                        sqi.FromLevel = sqi.ToLevel - 1
                                        sqi.Pos = sqCount
                                        sqi.Notes = plansItemNode.ChildNodes(0).InnerText
                                        sq.Add(sqi.Key, sqi)
                                    Next
                                    planInfo(count, 2) = sq.Count.ToString

                                    ' Ok, load up the plan
                                    Dim newSq As New EveHQSkillQueue
                                    newSq.Name = planInfo(count, 1)
                                    newSq.IncCurrentTraining = True
                                    newSq.Primary = False
                                    newSq.Queue = sq
                                    Dim qPilot As EveHQPilot = HQ.Settings.Pilots(planInfo(count, 0))
                                    If qPilot.TrainingQueues.ContainsKey(planInfo(count, 1)) = False Then
                                        qPilot.TrainingQueues.Add(newSq.Name, newSq)
                                        recalcQueues = True
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If

                ' Recalc the queues if appropriate
                If recalcQueues = True Then
                    Call RefreshAllTraining()
                End If
                MessageBox.Show("Import of " & planDetails.Count & " EveMon plans complete!", "EveMon Skill Queue Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error importing EveMon plans." & ControlChars.CrLf & ControlChars.CrLf & "Error: " & ex.Message & ControlChars.CrLf & ex.StackTrace, "Import EveMon Plans Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub

        Private Sub ImportEveMonPlan()
            ' Create a new file dialog
            Dim ofd1 As New OpenFileDialog
            With ofd1
                .Title = "Select EveMon Plan file..."
                .FileName = ""
                .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                .Filter = "EveMon Plan Files (*.emp)|*.emp|All Files (*.*)|*.*"
                .FilterIndex = 1
                .RestoreDirectory = True
                If .ShowDialog() = DialogResult.OK Then
                    If My.Computer.FileSystem.FileExists(.FileName) = False Then
                        MessageBox.Show("Specified file does not exist. Please try again.", "Error Finding File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    Else
                        ' Open the file for reading
                        Dim planXML As New XmlDocument
                        Try
                            ' UnGZip the file
                            Dim fs As FileStream = New FileStream(.FileName, FileMode.Open, FileAccess.Read)
                            Dim compstream As New GZipStream(fs, CompressionMode.Decompress)
                            Dim sr As New StreamReader(compstream)
                            Dim strEmp As String = sr.ReadToEnd()
                            sr.Close()
                            fs.Close()
                            planXML.LoadXml(strEmp)
                        Catch ex As Exception
                            MessageBox.Show("Unable to read file data. Please check the file is not corrupted and you have permissions to access this file", "File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End Try
                        ' Get the list of skills from the plan
                        Dim skillList As XmlNodeList = planXML.SelectNodes("/plan/entry")
                        Dim planSkills As New SortedList(Of String, Integer)
                        For Each skill As XmlNode In skillList
                            Dim skillName As String = skill.Attributes.GetNamedItem("skill").Value
                            Dim skillLevel As Integer = CInt(skill.Attributes.GetNamedItem("level").Value)
                            planSkills.Add(skillName, skillLevel)
                        Next
                        ' Get a dialog for the new skills
                        Using selectQueue As New FrmSelectQueue(_displayPilot.Name, planSkills, "Import from EveMon")
                            selectQueue.ShowDialog()
                        End Using
                        Call RefreshAllTraining()
                    End If
                End If
            End With
        End Sub

        Private Sub ExportEveMonPlan()
            If _activeQueueControl.Queue IsNot Nothing Then
                Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(_displayPilot, _activeQueueControl.Queue, False, True)
                Dim qItem As SortedQueueItem
                If arrQueue IsNot Nothing Then
                    Dim empAtt As XmlAttribute
                    ' Create XML Document
                    Dim empxml As New XmlDocument
                    ' Create XML Declaration
                    Dim dec As XmlDeclaration = empxml.CreateXmlDeclaration("1.0", Nothing, Nothing)
                    empxml.AppendChild(dec)
                    ' Create plan root
                    Dim empRoot As XmlElement = empxml.CreateElement("plan")
                    empxml.AppendChild(empRoot)

                    empAtt = empxml.CreateAttribute("name")
                    empAtt.Value = _activeQueueControl.Queue.Name
                    empRoot.Attributes.Append(empAtt)

                    'EMPAtt = EMPXML.CreateAttribute("owner")
                    'EMPAtt.Value = displayPilot.Name
                    'EMPRoot.Attributes.Append(EMPAtt)

                    empAtt = empxml.CreateAttribute("revision")
                    empAtt.Value = "1968"
                    empRoot.Attributes.Append(empAtt)

                    Dim empSort As XmlElement = empxml.CreateElement("sorting")
                    empRoot.AppendChild(empSort)

                    empAtt = empxml.CreateAttribute("criteria")
                    empAtt.Value = "None"
                    empSort.Attributes.Append(empAtt)

                    empAtt = empxml.CreateAttribute("order")
                    empAtt.Value = "None"
                    empSort.Attributes.Append(empAtt)

                    empAtt = empxml.CreateAttribute("optimizeLearning")
                    empAtt.Value = "false"
                    empSort.Attributes.Append(empAtt)

                    empAtt = empxml.CreateAttribute("groupByPriority")
                    empAtt.Value = "false"
                    empSort.Attributes.Append(empAtt)

                    ' Create individual entries
                    Dim empElement As XmlElement

                    For Each qItem In arrQueue
                        Dim empEntry As XmlNode = empxml.CreateElement("entry")

                        empAtt = empxml.CreateAttribute("skillID")
                        empAtt.Value = qItem.ID.ToString
                        empEntry.Attributes.Append(empAtt)

                        empAtt = empxml.CreateAttribute("skill")
                        empAtt.Value = qItem.Name
                        empEntry.Attributes.Append(empAtt)

                        empAtt = empxml.CreateAttribute("level")
                        empAtt.Value = qItem.ToLevel.ToString
                        empEntry.Attributes.Append(empAtt)

                        empAtt = empxml.CreateAttribute("priority")
                        empAtt.Value = "3"
                        empEntry.Attributes.Append(empAtt)

                        empAtt = empxml.CreateAttribute("type")
                        If qItem.IsPrereq Then
                            empAtt.Value = "Prerequisite"
                        Else
                            empAtt.Value = "Planned"
                        End If
                        empEntry.Attributes.Append(empAtt)

                        empElement = empxml.CreateElement("notes")
                        empElement.InnerText = qItem.Notes
                        empEntry.AppendChild(empElement)

                        empRoot.AppendChild(empEntry)
                    Next

                    ' Form a string of the XML
                    Dim strXML As String = empxml.InnerXml

                    ' Get a file name
                    Dim sfd1 As New SaveFileDialog
                    With sfd1
                        .Title = "Save as EveMon Plan File..."
                        .FileName = ""
                        .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                        .Filter = "EveMon Plan Files (*.emp)|*.emp|All Files (*.*)|*.*"
                        .FilterIndex = 1
                        .RestoreDirectory = True
                        If .ShowDialog() = DialogResult.OK Then
                            If .FileName <> "" Then
                                ' Output the file as GZip
                                Dim buffer() As Byte
                                Dim enc As New ASCIIEncoding
                                buffer = enc.GetBytes(strXML)
                                Dim outfile As FileStream = File.Create(.FileName)
                                Dim gzipStream As New GZipStream(outfile, CompressionMode.Compress)
                                gzipStream.Write(buffer, 0, buffer.Length)
                                gzipStream.Flush()
                                gzipStream.Close()
                            End If
                        End If
                    End With
                End If
            Else
                MessageBox.Show("Please select a skill queue tab before exporting the data.", "Skill Queue Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

#End Region

#Region "Ribbon Button Functions"
        Private Sub btnEveMonImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEveMonImport.Click
            Call ImportAllEveMonPlans()
        End Sub

        Private Sub btnImportEMPFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImportEMPFile.Click
            Call ImportEveMonPlan()
        End Sub

        Private Sub btnExportEMPFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportEMPFile.Click
            Call ExportEveMonPlan()
        End Sub

        Private Sub btnImplants_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImplants.Click
            If FrmImplants.IsHandleCreated = True Then
                FrmImplants.Select()
            Else
                FrmImplants.PilotName = _displayPilot.Name
                FrmImplants.QueueName = _activeQueueName
                FrmImplants.Show()
            End If
        End Sub

        Private Sub btnRemap_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemap.Click
            If FrmNeuralRemap.IsHandleCreated = True Then
                FrmNeuralRemap.Select()
            Else
                FrmNeuralRemap.PilotName = _displayPilot.Name
                FrmNeuralRemap.QueueName = _activeQueueName
                FrmNeuralRemap.Show()
            End If
        End Sub

        Private Sub btnRBAddQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBAddQueue.Click, mnuAddQueue.Click
            ' Clear the text boxes
            Using myQueue As New FrmModifyQueues
                With myQueue
                    .txtQueueName.Text = "" : .txtQueueName.Enabled = True
                    .btnAccept.Text = "Add" : .Tag = "Add"
                    .Text = "Add New Queue"
                    .DisplayPilotName = _displayPilot.Name
                    .ShowDialog()           ' New Queue checking and adding is done on the modal form!
                End With
            End Using
            FrmEveHQ.BuildQueueReportsMenu()
            Call RefreshAllTraining()
        End Sub

        Private Sub btnRBEditQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBEditQueue.Click, mnuEditQueue.Click
            ' Check for some selection on the listview
            If lvQueues.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a Queue to edit!", "Cannot Edit Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lvQueues.Select()
            Else
                Using myQueue As New FrmModifyQueues
                    With myQueue
                        ' Load the account details into the text boxes
                        Dim selQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name)
                        .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Name
                        .btnAccept.Text = "Edit" : .Tag = "Edit"
                        .Text = "Edit '" & selQueue.Name & "' Queue Details"
                        .DisplayPilotName = _displayPilot.Name
                        .ShowDialog()
                    End With
                End Using
                FrmEveHQ.BuildQueueReportsMenu()
                Call RefreshAllTraining()
            End If
        End Sub

        Private Sub btnRBDeleteQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBDeleteQueue.Click, mnuDeleteQueue.Click
            ' Check for some selection on the listview
            If lvQueues.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a skill queue to delete!", "Cannot Delete Skill Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lvQueues.Select()
            Else
                Dim selQ As String = lvQueues.SelectedItems(0).Text
                ' Confirm deletion
                Dim msg As String = ""
                msg &= "Are you sure you wish to delete the '" & selQ & "' skill queue?"
                Dim confirm As Integer = MessageBox.Show(msg, "Confirm Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If confirm = DialogResult.Yes Then
                    ' Delete the queue the accounts collection
                    _displayPilot.TrainingQueues.Remove(selQ)
                    If _displayPilot.PrimaryQueue = selQ Then
                        _displayPilot.PrimaryQueue = ""
                    End If
                    If _displayPilot.TrainingQueues.Count = 0 Then
                        _displayPilot.PrimaryQueue = ""
                        _displayPilot.ActiveQueueName = ""
                        _displayPilot.ActiveQueue = Nothing
                    End If
                    ' Remove the item from the list
                    FrmEveHQ.BuildQueueReportsMenu()
                    Call RefreshAllTraining()
                Else
                    lvQueues.Select()
                    Exit Sub
                End If
            End If
        End Sub

        Private Sub btnRBSetPrimaryQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBSetPrimaryQueue.Click, mnuSetPrimary.Click
            If lvQueues.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a Queue to call Primary!", "Cannot Set Primary Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lvQueues.Select()
            Else
                For Each tq As EveHQSkillQueue In _displayPilot.TrainingQueues.Values
                    If tq.Name = lvQueues.SelectedItems(0).Name Then
                        tq.Primary = True
                        _displayPilot.PrimaryQueue = tq.Name
                    Else
                        tq.Primary = False
                    End If
                Next
                Call DrawQueueSummary()
            End If
        End Sub

        Private Sub btnRBMergeQueues_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBMergeQueues.Click, mnuMergeQueues.Click
            If lvQueues.SelectedItems.Count < 2 Then
                MessageBox.Show("Please select 2 or more Queues to merge!", "Cannot Merge Queues", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lvQueues.Select()
            Else
                Using myQueue As New FrmModifyQueues
                    With myQueue
                        .txtQueueName.Text = "" : .txtQueueName.Tag = lvQueues.SelectedItems
                        .btnAccept.Text = "Merge" : .Tag = "Merge"
                        .Text = "Merge Skill Queues"
                        .DisplayPilotName = _displayPilot.Name
                        .ShowDialog()           ' Queue checking and merging is done on the modal form!
                    End With
                End Using
                Call RefreshAllTraining()
            End If
        End Sub

        Private Sub btnRBCopyQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBCopyQueue.Click, mnuCopyQueue.Click
            ' Check for some selection on the listview
            If lvQueues.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a Queue to copy!", "Cannot Copy Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lvQueues.Select()
            Else
                Using myQueue As New FrmModifyQueues
                    With myQueue
                        ' Load the account details into the text boxes
                        Dim selQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name)
                        .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Name
                        .btnAccept.Text = "Copy" : .Tag = "Copy"
                        .Text = "Copy '" & selQueue.Name & "' Queue Details"
                        .DisplayPilotName = _displayPilot.Name
                        .ShowDialog()
                    End With
                End Using
                Call RefreshAllTraining()
            End If
        End Sub

        Private Sub btnRBCopyQueueToPilot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBCopyQueueToPilot.Click, mnuCopyQueueToPilot.Click
            ' Check for some selection on the listview
            If lvQueues.SelectedItems.Count = 0 Then
                MessageBox.Show("Please select a Queue to copy!", "Cannot Copy Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                lvQueues.Select()
            Else
                Using myQueue As FrmSelectQueuePilot = New FrmSelectQueuePilot
                    With myQueue
                        ' Load the account details into the text boxes
                        Dim selQueue As EveHQSkillQueue = _displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name)
                        .DisplayPilotName = _displayPilot.Name
                        .cboPilots.Tag = selQueue.Name
                        .ShowDialog()
                    End With
                End Using
            End If
        End Sub

        Private Sub btnIncTraining_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIncTraining.Click
            If _redrawingOptions = False Then
                If _activeQueueControl.Queue IsNot Nothing Then
                    _activeQueueControl.IncludesCurrentTraining = btnIncTraining.Checked
                    _activeQueueControl.Queue.IncCurrentTraining = btnIncTraining.Checked
                    If _activeQueueControl.Queue.Name IsNot Nothing Then
                        RefreshTraining(_activeQueueControl.Queue.Name, False)
                    End If
                End If
            End If
        End Sub

        Private Sub btnRBAddSkill_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBAddSkill.Click
            Call AddSkillToQueueOption()
        End Sub

        Private Sub btnRBDeleteSkill_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBDeleteSkill.Click
            Call _activeQueueControl.DeleteFromQueueOption()
        End Sub

        Private Sub btnRBIncreaseLevel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBIncreaseLevel.Click
            Call _activeQueueControl.IncreaseLevel()
        End Sub

        Private Sub btnRBDecreaseLevel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBDecreaseLevel.Click
            Call _activeQueueControl.DecreaseLevel()
        End Sub

        Private Sub btnRBMoveUpQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBMoveUpQueue.Click
            Call _activeQueueControl.MoveUpQueue()
        End Sub

        Private Sub btnRBMoveDownQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBMoveDownQueue.Click
            Call _activeQueueControl.MoveDownQueue()
        End Sub

        Private Sub btnRBClearQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBClearQueue.Click
            Call _activeQueueControl.ClearTrainingQueue()
        End Sub

        Private Sub btnQueueSettings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnQueueSettings.Click
            Using mySettings As New FrmSettings
                mySettings.Tag = "nodeTrainingQueue"
                mySettings.ShowDialog()
            End Using
        End Sub

        Private Sub btnRBSplitQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRBSplitQueue.Click
            Call _activeQueueControl.SplitQueue()
            Call RefreshAllTraining()
        End Sub

        Private Sub btnAddRequisition_Click(sender As Object, e As EventArgs) Handles btnAddRequisition.Click
            Dim requiredSkills As New List(Of String)
            For Each skill As EveHQSkillQueueItem In _activeQueueControl.Queue.Queue.Values
                If SkillFunctions.IsSkillTrained(_displayPilot, skill.Name) = False Then
                    If requiredSkills.Contains(skill.Name) = False Then
                        requiredSkills.Add(skill.Name)
                    End If
                End If
            Next
            ' Set up a new Sortedlist to store the required items
            Dim orders As New SortedList(Of String, Integer)
            For Each skill As String In requiredSkills
                orders.Add(skill, 1)
            Next
            ' Setup the Requisition form for HQF and open it
            Using newReq As New FrmAddRequisition("Skill Queue", orders)
                newReq.ShowDialog()
            End Using
        End Sub

#End Region

    End Class

End Namespace