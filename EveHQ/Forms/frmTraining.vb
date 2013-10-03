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
'=========================================================================#
Imports System.Text
Imports System.IO
Imports System.Xml
Imports EveHQ.EveData

Public Class frmTraining

    Dim oldNodeIndex As Integer = -1
    Dim oNodes As New ArrayList
    Dim omitQueuedSkills As Boolean = False
    Dim selQTime As Double = 0
    Dim totalQTime As Double = 0
    Dim activeQueueName As String = ""
    Dim activeQueue As New Core.EveHQSkillQueue
    Dim activeTime As Label
    Dim activeLVW As EveHQ.DragAndDropListView
    Dim startTime As DateTime
    Dim endTime As DateTime
    Dim TrainingThreshold As Integer = 1600000
    Dim TrainingBonus As Double = 1
    Dim usingFilter As Boolean = True
    Dim skillListNodes As New SortedList
    Dim certListNodes As New SortedList
    Dim CertGrades() As String = New String() {"", "Basic", "Standard", "Improved", "Advanced", "Elite"}
    Dim displayPilot As New Core.EveHQPilot
    Dim cDisplayPilotName As String = ""
    Dim startup As Boolean = False
    Dim redrawingOptions As Boolean = False
    Dim sortedQueues As New SortedList(Of String, ArrayList)
    Dim RetainQueue As Boolean = False
    Dim OldTabName As String = "tabSummary"

    'Protected Overrides ReadOnly Property CreateParams() As CreateParams
    '    Get
    '        Dim cp As CreateParams = MyBase.CreateParams
    '        cp.ExStyle = cp.ExStyle Or &H2000000
    '        Return cp
    '    End Get
    'End Property 'CreateParams

    Public Property DisplayPilotName() As String
        Get
            Return cDisplayPilotName
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
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

    End Sub
    Private Sub frmTraining_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Core.HQ.Settings.SkillQueuePanelWidth = panelInfo.Width
        RemoveHandler Core.SkillQueueFunctions.RefreshQueue, AddressOf Me.RefreshAllTraining
    End Sub

    Private Sub frmTraining_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the startup flag
        startup = True

        panelInfo.Width = Core.HQ.Settings.SkillQueuePanelWidth

        ' Load the pilots
        Call UpdatePilots()

        ' Set up the queue information
        cboFilter.SelectedIndex = 0
        cboCertFilter.SelectedIndex = 0
        Call Me.SetupReqsAndDepends()
        Call Me.SetupQueues()
        Call Me.RefreshAllTrainingQueues()
        AddHandler Core.SkillQueueFunctions.RefreshQueue, AddressOf Me.RefreshAllTraining

        ' Disable the startup flag
        startup = False

    End Sub

    Private Sub frmTraining_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ' Force an update of the ribbon
        panelInfo.Visible = True
        tabQueues.Visible = True
    End Sub

    Private Sub SetupReqsAndDepends()
        ' Reset data
        Core.SkillQueueFunctions.SkillDepends.Clear()
        Core.SkillQueueFunctions.SkillPrereqs.Clear()
        Dim Depends As New SortedList(Of String, Integer)
        Dim PreReqs As New SortedList(Of String, Integer)
        Dim preReqName As String = ""
        ' Cycle through each skill and extract pre-req and build dependancy information
        For Each cSkill As Core.EveSkill In Core.HQ.SkillListID.Values
            PreReqs = New SortedList(Of String, Integer)
            For Each preReqID As Integer In cSkill.PreReqSkills.Keys
                If StaticData.Types.ContainsKey(preReqID) = True Then
                    preReqName = StaticData.Types(preReqID).Name
                    PreReqs.Add(preReqName, cSkill.PreReqSkills(preReqID))
                    If Core.SkillQueueFunctions.SkillDepends.ContainsKey(preReqName) = True Then
                        Depends = Core.SkillQueueFunctions.SkillDepends(preReqName)
                    Else
                        Depends = New SortedList(Of String, Integer)
                        Core.SkillQueueFunctions.SkillDepends.Add(preReqName, Depends)
                    End If
                    Depends.Add(cSkill.Name, cSkill.PreReqSkills(preReqID))
                End If
            Next
            Core.SkillQueueFunctions.SkillPrereqs.Add(cSkill.Name, PreReqs)
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
        Dim oldQueue As String = activeQueueName

        ' Update the pilots combo box
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each cPilot As Core.EveHQPilot In Core.HQ.Settings.Pilots.Values
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next
        cboPilots.EndUpdate()

        ' Select a pilot
        If cDisplayPilotName <> "" Then
            If cboPilots.Items.Contains(cDisplayPilotName) = True Then
                RetainQueue = True
                cboPilots.SelectedItem = cDisplayPilotName
            Else
                If cboPilots.Items.Count > 0 Then
                    cboPilots.SelectedIndex = 0
                End If
            End If
        Else
            If oldPilot = "" Then
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(Core.HQ.Settings.StartupPilot) = True Then
                        cboPilots.SelectedItem = Core.HQ.Settings.StartupPilot
                    Else
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            Else
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(oldPilot) = True Then
                        If Not (CStr(cboPilots.SelectedItem) = oldPilot) Then
                            RetainQueue = True
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
                Dim ti As DevComponents.DotNetBar.TabItem = tabQueues.Tabs.Item(oldQueue)
                tabQueues.SelectedTab = ti
            End If
        End If

    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        If Core.HQ.Settings.Pilots.ContainsKey(cboPilots.SelectedItem.ToString) = True Then
            displayPilot = Core.HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
            cDisplayPilotName = displayPilot.Name
            ' Only update if we are not starting up
            If startup = False Then
                startup = True
                Call Me.RefreshAllTraining()
                startup = False
                ' See if the Neural Remapping form is open
                If frmNeuralRemap.IsHandleCreated = True Then
                    frmNeuralRemap.PilotName = displayPilot.Name
                End If
                ' See if the Implants form is open
                If frmImplants.IsHandleCreated = True Then
                    frmImplants.PilotName = displayPilot.Name
                End If
            End If
        End If
    End Sub

    Public Sub SetupQueues()

        ' Save the current active queue name
        If RetainQueue = False Then
            OldTabName = "tabSummary"
        Else
            OldTabName = tabQueues.SelectedTab.Name
        End If
        RetainQueue = False

        ' Remove all but the summary tab on the tabQueues
        Dim ti As New DevComponents.DotNetBar.TabItem
        For tidx As Integer = tabQueues.Tabs.Count - 1 To 1 Step -1
            ti = tabQueues.Tabs(tidx)
            If ti.Name <> "tabSummary" Then
                If TypeOf ti.AttachedControl Is TrainingQueue Then
                    Dim tq As TrainingQueue = CType(ti.AttachedControl, TrainingQueue)
                    RemoveHandler tq.lvQueue.KeyDown, AddressOf activeLVW_KeyDown
                    RemoveHandler tq.lvQueue.Click, AddressOf activeLVW_Click
                    RemoveHandler tq.lvQueue.DoubleClick, AddressOf activeLVW_DoubleClick
                    RemoveHandler tq.lvQueue.DragDrop, AddressOf activeLVW_DragDrop
                    RemoveHandler tq.lvQueue.DragEnter, AddressOf activeLVW_DragEnter
                    RemoveHandler tq.lvQueue.ItemDrag, AddressOf activeLVW_ItemDrag
                    RemoveHandler tq.lvQueue.ColumnClick, AddressOf activeLVW_ColumnClick
                    RemoveHandler tq.lvQueue.SelectedIndexChanged, AddressOf activeLVW_SelectedIndexChanged
                End If
                Me.tabQueues.Tabs.Remove(ti)
                ti.Dispose()
            End If
        Next

        If displayPilot IsNot Nothing Then
            If displayPilot.TrainingQueues IsNot Nothing Then
                For Each newQ As Core.EveHQSkillQueue In displayPilot.TrainingQueues.Values
                    Dim newQTab As DevComponents.DotNetBar.TabItem
                    If Not tabQueues.Controls.ContainsKey(newQ.Name) Then
                        newQTab = New DevComponents.DotNetBar.TabItem
                        newQTab.Name = newQ.Name
                        newQTab.Text = newQ.Name

                        Dim tq As TrainingQueue = New TrainingQueue()
                        tq.Dock = DockStyle.Fill
                        tq.Name = "TQ" & newQ.Name
                        tq.lvQueue.IncludeCurrentTraining = newQ.IncCurrentTraining
                        tq.lvQueue.ContextMenuStrip = Me.ctxQueue
                        newQTab.AttachedControl = tq

                        AddHandler tq.lvQueue.KeyDown, AddressOf activeLVW_KeyDown
                        AddHandler tq.lvQueue.Click, AddressOf activeLVW_Click
                        AddHandler tq.lvQueue.DoubleClick, AddressOf activeLVW_DoubleClick
                        AddHandler tq.lvQueue.DragDrop, AddressOf activeLVW_DragDrop
                        AddHandler tq.lvQueue.DragEnter, AddressOf activeLVW_DragEnter
                        AddHandler tq.lvQueue.ItemDrag, AddressOf activeLVW_ItemDrag
                        AddHandler tq.lvQueue.ColumnClick, AddressOf activeLVW_ColumnClick
                        AddHandler tq.lvQueue.SelectedIndexChanged, AddressOf activeLVW_SelectedIndexChanged

                        Call Me.DrawColumns(tq.lvQueue)

                        tabQueues.Tabs.Add(newQTab)

                    End If
                Next
            End If
        End If

        Dim OldTab As DevComponents.DotNetBar.TabItem = tabQueues.Tabs(OldTabName)
        If Core.HQ.Settings.StartWithPrimaryQueue = True And OldTabName = "tabSummary" Then
            OldTab = tabQueues.Tabs(displayPilot.PrimaryQueue)
        End If

        If OldTab IsNot Nothing Then
            tabQueues.SelectedTab = OldTab
        Else
            tabQueues.SelectedTab = tabQueues.Tabs(0)
        End If

    End Sub

    Private Sub DrawColumns(ByVal lv As EveHQ.DragAndDropListView)

        lv.Columns.Clear()
        ' Add the standard column
        lv.Columns.Add("Name", "Skill Name", 180, HorizontalAlignment.Left, "")

        ' Add subitems based on the user selected columns
        Dim colName As String = ""
        For Each col As String In Core.HQ.Settings.UserQueueColumns
            If col.EndsWith("1") = True Then
                colName = col.Substring(0, col.Length - 1)
                Dim newSI As New ListViewItem.ListViewSubItem
                Select Case colName
                    Case "Current"
                        lv.Columns.Add(colName, "Cur Lvl", 50, HorizontalAlignment.Center, "")
                    Case "From"
                        lv.Columns.Add(colName, "From Lvl", 55, HorizontalAlignment.Center, "")
                    Case "To"
                        lv.Columns.Add(colName, "To Lvl", 55, HorizontalAlignment.Center, "")
                    Case "Percent"
                        lv.Columns.Add(colName, "%", 30, HorizontalAlignment.Center, "")
                    Case "TrainTime"
                        lv.Columns.Add(colName, "Training Time", 100, HorizontalAlignment.Left, "")
                    Case "TimeToComplete"
                        lv.Columns.Add(colName, "Time To Complete", 100, HorizontalAlignment.Left, "")
                    Case "DateEnded"
                        lv.Columns.Add(colName, "Date Completed", 150, HorizontalAlignment.Left, "")
                    Case "Rank"
                        lv.Columns.Add(colName, "Rank", 60, HorizontalAlignment.Center, "")
                    Case "PAtt"
                        lv.Columns.Add(colName, "Pri Attr", 60, HorizontalAlignment.Left, "")
                    Case "SAtt"
                        lv.Columns.Add(colName, "Sec Attr", 60, HorizontalAlignment.Left, "")
                    Case "SPHour"
                        lv.Columns.Add(colName, "SP /hour", 60, HorizontalAlignment.Right, "")
                    Case "SPDay"
                        lv.Columns.Add(colName, "SP /day", 60, HorizontalAlignment.Right, "")
                    Case "SPWeek"
                        lv.Columns.Add(colName, "SP /week", 60, HorizontalAlignment.Right, "")
                    Case "SPMonth"
                        lv.Columns.Add(colName, "SP /mnth", 60, HorizontalAlignment.Right, "")
                    Case "SPYear"
                        lv.Columns.Add(colName, "SP /year", 60, HorizontalAlignment.Right, "")
                    Case "SPAdded"
                        lv.Columns.Add(colName, "SP Added", 100, HorizontalAlignment.Right, "")
                    Case "SPTotal"
                        lv.Columns.Add(colName, "SP @ End", 100, HorizontalAlignment.Right, "")
                    Case "Notes"
                        lv.Columns.Add(colName, "Notes", 200, HorizontalAlignment.Left, "")
                    Case "Priority"
                        lv.Columns.Add(colName, "Priority", 50, HorizontalAlignment.Center, "")
                End Select
            End If
        Next
    End Sub
    Public Sub RefreshAllTrainingQueues()
        For Each newQ As Core.EveHQSkillQueue In displayPilot.TrainingQueues.Values
            Call Me.RefreshTraining(newQ.Name)
        Next
        Call Me.DrawQueueSummary()
    End Sub
    Private Sub DrawQueueSummary()
        ' Check if we have a Primary Queue
        If displayPilot.TrainingQueues.Count > 0 Then
            If displayPilot.PrimaryQueue = "" Then
                displayPilot.PrimaryQueue = displayPilot.TrainingQueues.Keys(0).ToString
                Dim selQueue As Core.EveHQSkillQueue = displayPilot.TrainingQueues(displayPilot.PrimaryQueue)
                selQueue.Primary = True
            End If
        End If
        ' Setup the summary column
        lvQueues.Items.Clear()
        Dim totalQTime As Double = 0
        For Each newQ As Core.EveHQSkillQueue In displayPilot.TrainingQueues.Values
            Dim newItem As New ListViewItem
            Dim PrimaryFont As Font = New Font(newItem.Font, FontStyle.Bold)
            newItem.Name = newQ.Name
            newItem.Text = newQ.Name
            If newQ.Primary = True Then
                newItem.Font = PrimaryFont
            End If
            newItem.SubItems.Add(newQ.Queue.Count.ToString)
            Dim tq As TrainingQueue = CType(tabQueues.Tabs.Item(newQ.Name).AttachedControl, TrainingQueue)
            Dim tTime As Double = CDbl(tq.lblQueueTime.Tag)
            Dim tTimeItem As New ListViewItem.ListViewSubItem
            tTimeItem.Tag = tTime
            tTimeItem.Text = Core.SkillFunctions.TimeToString(tTime)
            newItem.SubItems.Add(tTimeItem)
            Dim qTime As Double = 0
            If displayPilot.Training = True And newQ.IncCurrentTraining = True Then
                qTime = tTime - displayPilot.TrainingCurrentTime
            Else
                qTime = tTime
            End If
            Dim qTimeItem As New ListViewItem.ListViewSubItem
            qTimeItem.Tag = tTime
            qTimeItem.Text = Core.SkillFunctions.TimeToString(qTime)
            newItem.SubItems.Add(qTimeItem)
            totalQTime += qTime
            Dim eTime As Date = Now.AddSeconds(tTime)
            newItem.SubItems.Add(Format(eTime, "ddd") & " " & eTime.ToString)
            lvQueues.Items.Add(newItem)
        Next
    End Sub
    Private Sub tabQueues_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabQueues.SelectedTabChanged
        If tabQueues.SelectedTab.Name IsNot Nothing Then
            If displayPilot.TrainingQueues.ContainsKey(tabQueues.SelectedTab.Name) = True Then
                activeQueueName = tabQueues.SelectedTab.Name
                displayPilot.ActiveQueueName = activeQueueName
                Dim aq As Core.EveHQSkillQueue = displayPilot.TrainingQueues(activeQueueName)
                activeQueue = aq
                displayPilot.ActiveQueue = activeQueue
                Dim tq As TrainingQueue = CType(tabQueues.Tabs.Item(activeQueueName).AttachedControl, TrainingQueue)
                activeTime = tq.lblQueueTime
                activeLVW = tq.lvQueue
                activeLVW.IncludeCurrentTraining = aq.IncCurrentTraining
                Call RedrawOptions()
                activeLVW.Select()
                mnuAddToQueue.Enabled = True
            Else
                activeQueueName = ""
                activeQueue = Nothing
                displayPilot.ActiveQueueName = activeQueueName
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
            If frmNeuralRemap.IsHandleCreated = True Then
                frmNeuralRemap.QueueName = activeQueueName
            End If
            If frmImplants.IsHandleCreated = True Then
                frmImplants.QueueName = activeQueueName
            End If

        End If
    End Sub
#End Region

#Region "Training Refresh Routines"
    Public Sub RefreshAllTraining()
        If Me.IsHandleCreated = True Then
            Call Me.SetupQueues()
            Call Me.RefreshAllTrainingQueues()
            Call Me.ResetQueueOptions()
            Call Me.LoadSkillTree()
            Call Me.LoadCertificateTree()
        End If
    End Sub
    Public Sub LoadSkillTree()
        Dim filter As Integer = cboFilter.SelectedIndex
        ' Save current open nodes
        oNodes.Clear()
        For Each oNode As TreeNode In tvwSkillList.Nodes
            If oNode.IsExpanded = True Then
                oNodes.Add(oNode.Name)
            End If
        Next
        tvwSkillList.BeginUpdate()
        tvwSkillList.Nodes.Clear()
        tvwSkillList.Sorted = False
        Call Me.LoadSkillGroups()
        Call Me.LoadFilteredSkills(filter)
        Call Me.ShowSkillGroups()
        For Each oNode As String In oNodes
            If tvwSkillList.Nodes.ContainsKey(oNode) = True Then
                tvwSkillList.Nodes(oNode).Expand()
            End If
        Next
        tvwSkillList.Sorted = True
        tvwSkillList.EndUpdate()
        tvwSkillList.Refresh()
    End Sub
    Private Sub LoadSkillGroups()
        skillListNodes.Clear()
        Dim newSkillGroup As Core.SkillGroup
        For Each newSkillGroup In Core.HQ.SkillGroups.Values
            If newSkillGroup.ID <> 505 Then
                Dim groupNode As TreeNode = New TreeNode
                groupNode.Name = CStr(newSkillGroup.ID)
                groupNode.Text = newSkillGroup.Name.Trim
                groupNode.ImageIndex = 8
                groupNode.SelectedImageIndex = 8
                skillListNodes.Add(groupNode.Name, groupNode)
            End If
        Next
    End Sub
    Private Sub LoadFilteredSkills(ByVal filter As Integer)
        Dim newSkill As Core.EveSkill
        Dim groupNode As New TreeNode
        For Each newSkill In Core.HQ.SkillListID.Values
            Dim gID As Integer = newSkill.GroupID
            groupNode = CType(skillListNodes.Item(gID), TreeNode)
            If gID <> 505 Then
                Dim skillNode As TreeNode = New TreeNode
                skillNode.Text = newSkill.Name
                skillNode.Name = CStr(newSkill.ID)
                If displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                    Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(newSkill.Name)
                    skillNode.ImageIndex = mySkill.Level
                    skillNode.SelectedImageIndex = mySkill.Level
                Else
                    skillNode.ImageIndex = 10
                    skillNode.SelectedImageIndex = 10
                End If
                If displayPilot.Name <> "" Then
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
                            If displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                                addSkill = True
                            End If
                        Case 4
                            If newSkill.Published = True And displayPilot.PilotSkills.ContainsKey(newSkill.Name) = False Then
                                addSkill = True
                            End If
                        Case 5
                            Dim trainable As Boolean = False
                            If displayPilot.PilotSkills.ContainsKey(newSkill.Name) = False And newSkill.Published = True Then
                                trainable = True
                                For Each preReq As Integer In newSkill.PreReqSkills.Keys
                                    If newSkill.PreReqSkills(preReq) <> 0 Then
                                        Dim ps As Core.EveSkill = Core.HQ.SkillListID(preReq)
                                        If displayPilot.PilotSkills.ContainsKey(ps.Name) = True Then
                                            Dim psp As Core.EveHQPilotSkill = displayPilot.PilotSkills(ps.Name)
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
                            If displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                                Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(newSkill.Name)
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
                            If displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                                Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(newSkill.Name)
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
                        If omitQueuedSkills = False Then
                            If groupNode IsNot Nothing Then
                                groupNode.Nodes.Add(skillNode)
                            End If
                        Else
                            Dim inQ As Boolean = False
                            For Each skillQ As Core.EveHQSkillQueue In displayPilot.TrainingQueues.Values
                                If inQ = True Then Exit For
                                Dim sQ As Dictionary(Of String, Core.EveHQSkillQueueItem) = skillQ.Queue
                                For Each skillQueueItem As Core.EveHQSkillQueueItem In sQ.Values
                                    If newSkill.Name = skillQueueItem.Name Then
                                        inQ = True
                                        Exit For
                                    End If
                                Next
                            Next
                            If inQ = False Then
                                If groupNode IsNot Nothing Then
                                    groupNode.Nodes.Add(skillNode)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub ShowSkillGroups()
        For Each groupNode As TreeNode In skillListNodes.Values
            If groupNode.Nodes.Count > 0 Then
                tvwSkillList.Nodes.Add(groupNode)
            End If
        Next
    End Sub
    Public Sub RefreshTraining(ByVal QueueName As String)

        Dim ti As DevComponents.DotNetBar.TabItem = tabQueues.Tabs.Item(QueueName)
        If ti Is Nothing Then
            Exit Sub
        End If

        startTime = Now
        Dim selectedItems As New ArrayList
        Dim tq As TrainingQueue = CType(tabQueues.Tabs.Item(QueueName).AttachedControl, TrainingQueue)
        Dim lvwQueue As EveHQ.DragAndDropListView = tq.lvQueue

        If displayPilot.PilotSkills.Count <> 0 Then
            ' Save the selected items
            For Each selItem As ListViewItem In lvwQueue.SelectedItems
                selectedItems.Add(selItem)
            Next

            ' Clear the visible training queue

            ' Remember the first visible item?
            Dim FVI As Integer = -1
            If lvwQueue.TopItem IsNot Nothing Then
                FVI = lvwQueue.TopItem.Index
            End If
            lvwQueue.SuspendLayout()
            lvwQueue.BeginUpdate()
            lvwQueue.Items.Clear()

            ' Prep a new font ready for completed training queues
            Dim doneFont As Font = New Font("Tahoma", 8, FontStyle.Strikeout)

            ' Call the main procedure
            Dim aq As Core.EveHQSkillQueue = displayPilot.TrainingQueues(QueueName)
            Dim sortedQueue As ArrayList = Core.SkillQueueFunctions.BuildQueue(displayPilot, aq, False, True)
            If sortedQueues.ContainsKey(QueueName) = True Then
                sortedQueues(QueueName) = sortedQueue
            Else
                sortedQueues.Add(QueueName, sortedQueue)
            End If
            Dim qItem As New Core.SortedQueueItem
            Dim totalTime As Long = 0
            Dim totalSP As Long = displayPilot.SkillPoints

            ' Create the columns according to the selection in the settings
            Call Me.DrawColumns(lvwQueue)

            If sortedQueue IsNot Nothing Then
                For Each qItem In sortedQueue
                    Dim newskill As ListViewItem = New ListViewItem
                    newskill.Name = qItem.Key
                    If qItem.Done = False Or (qItem.Done = True And Core.HQ.Settings.ShowCompletedSkills = True) Then
                        If qItem.Done = True Then newskill.Font = doneFont
                        If qItem.IsPrereq = True Then
                            If qItem.HasPrereq = True Then
                                newskill.ToolTipText &= qItem.Prereq & ControlChars.CrLf & qItem.Reqs
                                newskill.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.BothPreReqColor))
                            Else
                                newskill.ToolTipText = qItem.Prereq
                                newskill.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.IsPreReqColor))
                            End If
                        Else
                            If qItem.HasPrereq = True Then
                                newskill.ToolTipText = qItem.Reqs
                                newskill.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.HasPreReqColor))
                            Else
                                If qItem.PartTrained = True Then
                                    newskill.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.PartialTrainColor))
                                Else
                                    newskill.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.ReadySkillColor))
                                End If
                            End If
                        End If
                        If qItem.IsTraining = True Then
                            newskill.BackColor = Color.LimeGreen
                            ' Set a flag in the listview of the listviewitem name for later checking
                            lvwQueue.Tag = newskill.Name
                        End If
                        Dim clashTime As DateTime = Core.SkillFunctions.ConvertLocalTimeToEve(qItem.DateFinished)
                        If clashTime.Hour = 11 And (clashTime.Minute >= 0 And clashTime.Minute <= 30) Then
                            newskill.ForeColor = Color.FromArgb(CInt(Core.HQ.Settings.DtClashColor))
                            If newskill.ToolTipText <> "" Then
                                newskill.ToolTipText &= ControlChars.CrLf & ControlChars.CrLf
                            End If
                            newskill.ToolTipText &= "WARNING: Skill end time occurs during Eve Downtime"
                        End If
                        ' Do some additional calcs
                        totalSP += CLng(qItem.SPTrained)
                        totalTime += CLng(qItem.TrainTime)
                        newskill.Text = qItem.Name
                        newskill.Tag = qItem.ID
                        Call Me.AddUserColumns(newskill, qItem, totalSP)
                        lvwQueue.Items.Add(newskill)
                    End If
                Next
            End If

            Dim lblQueue As Label = tq.lblQueueTime
            lblQueue.Tag = totalTime.ToString
            lblQueue.Text = Core.SkillFunctions.TimeToString(totalTime)

            Dim lblNumSkills As Label = tq.lblSkillCount
            lblNumSkills.Text = lvwQueue.Items.Count.ToString

            ' Select the old selected items
            For Each selItem As ListViewItem In selectedItems
                If lvwQueue.Items.ContainsKey(selItem.Name) Then
                    lvwQueue.Items(selItem.Name).Selected = True
                    lvwQueue.Items(selItem.Name).Focused = True
                End If
            Next

            ' Tidy up afterwards
            lvwQueue.EndUpdate()
            lvwQueue.ResumeLayout()
            If FVI >= 0 And lvwQueue.Items.Count > 0 And FVI < lvwQueue.Items.Count Then
                lvwQueue.TopItem = lvwQueue.Items(FVI)
                lvwQueue.TopItem = lvwQueue.Items(FVI)
            End If
            Call Core.SkillQueueFunctions.TidyQueue(displayPilot, aq, sortedQueue)
            Call Me.RedrawOptions()

            endTime = Now
            Dim timeTaken As TimeSpan = endTime - startTime
            'MessageBox.Show("Time taken: " & timeTaken.TotalMilliseconds.ToString & "ms", "Refresh Training Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If
    End Sub
    Private Sub AddUserColumns(ByVal newskill As ListViewItem, ByVal qitem As Core.SortedQueueItem, ByVal totalSP As Long)
        ' Add subitems based on the user selected columns
        Dim colName As String = ""
        For Each col As String In Core.HQ.Settings.UserQueueColumns
            If col.EndsWith("1") = True Then
                colName = col.Substring(0, col.Length - 1)
                Dim newSI As New ListViewItem.ListViewSubItem
                Select Case colName
                    Case "Current"
                        If (qitem.IsInjected) Then
                            newSI.Name = qitem.CurLevel.ToString
                            newSI.Text = qitem.CurLevel.ToString
                        Else
                            newSI.Name = ""
                            newSI.Text = ""
                        End If
                    Case "From"
                        newSI.Name = qitem.FromLevel.ToString
                        newSI.Text = qitem.FromLevel.ToString
                    Case "To"
                        newSI.Name = qitem.ToLevel.ToString
                        newSI.Text = qitem.ToLevel.ToString
                    Case "Percent"
                        Dim skillPct As Double
                        If displayPilot.PilotSkills.ContainsKey(qitem.Name) Then
                            Dim myCurSkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(qitem.Name)
                            Dim baseSkill As Core.EveSkill = Core.HQ.SkillListName(myCurSkill.Name)
                            Dim clevel As Integer = CInt(qitem.FromLevel)
                            Dim nextLevelSp As Integer = baseSkill.LevelUp(clevel + 1) - baseSkill.LevelUp(clevel)

                            If clevel <> myCurSkill.Level Then
                                skillPct = 0
                            Else
                                If qitem.Name = displayPilot.TrainingSkillName Then
                                    skillPct = CInt(Int((myCurSkill.SP + displayPilot.TrainingCurrentSP - baseSkill.LevelUp(clevel)) / nextLevelSp * 100))
                                Else
                                    skillPct = CInt(Int((myCurSkill.SP - baseSkill.LevelUp(clevel)) / nextLevelSp * 100))
                                End If
                            End If

                            If skillPct > 100 Then
                                skillPct = 100
                            End If
                        Else
                            skillPct = 0
                        End If
                        newSI.Name = "Percent"
                        newSI.Text = CStr(skillPct)
                    Case "TrainTime"
                        newSI.Name = "TrainTime"
                        newSI.Text = Core.SkillFunctions.TimeToString(CDbl(qitem.TrainTime))
                        newSI.Tag = qitem.TrainTime
                    Case "TimeToComplete"
                        newSI.Name = "TimeToComplete"
                        newSI.Text = Core.SkillFunctions.TimeToString(CDbl(qitem.TimeBeforeTrained))
                        newSI.Tag = qitem.TimeBeforeTrained
                    Case "DateEnded"
                        newSI.Name = qitem.DateFinished.ToBinary.ToString
                        newSI.Text = Format(qitem.DateFinished, "ddd") & " " & qitem.DateFinished.ToString
                    Case "Rank"
                        newSI.Name = qitem.Rank.ToString
                        newSI.Text = qitem.Rank.ToString
                    Case "PAtt"
                        newSI.Name = qitem.PAtt
                        newSI.Text = qitem.PAtt
                    Case "SAtt"
                        newSI.Name = qitem.SAtt
                        newSI.Text = qitem.SAtt
                    Case "SPHour"
                        newSI.Name = qitem.SPRate.ToString
                        newSI.Text = qitem.SPRate.ToString("N0")
                    Case "SPDay"
                        newSI.Name = CStr(CDbl(qitem.SPRate) * 24)
                        newSI.Text = (qitem.SPRate * 24).ToString("N0")
                    Case "SPWeek"
                        newSI.Name = CStr(CDbl(qitem.SPRate) * 24 * 7)
                        newSI.Text = (qitem.SPRate * 24 * 7).ToString("N0")
                    Case "SPMonth"
                        newSI.Name = CStr(CDbl(qitem.SPRate) * 24 * 30)
                        newSI.Text = (qitem.SPRate * 24 * 30).ToString("N0")
                    Case "SPYear"
                        newSI.Name = CStr(CDbl(qitem.SPRate) * 24 * 365)
                        newSI.Text = (qitem.SPRate * 24 * 365).ToString("N0")
                    Case "SPAdded"
                        newSI.Name = qitem.SPTrained.ToString
                        newSI.Text = qitem.SPTrained.ToString("N0")
                    Case "SPTotal"
                        newSI.Name = CStr(totalSP)
                        newSI.Text = totalSP.ToString("N0")
                    Case "Notes"
                        newSI.Name = "Notes"
                        newSI.Text = qitem.Notes
                    Case "Priority"
                        newSI.Name = "Priority"
                        newSI.Text = qitem.Priority.ToString("N0")
                End Select
                newskill.SubItems.Add(newSI)
            End If
        Next
    End Sub
    Private Sub RedrawOptions()
        ' Set the redraw flag to avoid triggering a recalc
        redrawingOptions = True
        ' Determines what buttons and menus are available from the listview!
        If activeLVW IsNot Nothing Then
            ' Check the queue status
            btnIncTraining.Checked = activeLVW.IncludeCurrentTraining
            btnImplants.Enabled = True
            btnRemap.Enabled = True
            btnIncTraining.Enabled = True
            If activeLVW.SelectedItems.Count <> 0 Then
                Select Case activeLVW.SelectedItems.Count
                    Case 1
                        Dim skillKey As String = activeLVW.SelectedItems(0).Name
                        Dim skillName As String
                        Dim curFLevel As Integer = CInt(skillKey.Substring(skillKey.Length - 2, 1))
                        Dim curTLevel As Integer = CInt(skillKey.Substring(skillKey.Length - 1, 1))
                        Dim skillID As Integer
                        skillName = activeLVW.SelectedItems(0).Text
                        skillID = Core.SkillFunctions.SkillNameToID(skillName)
                        mnuSkillName.Text = skillName
                        mnuSkillName.Tag = skillID
                        ' Check if we can increase or decrease levels

                        Dim curLevel As Integer

                        Dim mySkill As New Core.EveHQPilotSkill
                        If displayPilot.PilotSkills.ContainsKey(skillName) = False Then
                            curLevel = 0
                        Else
                            mySkill = displayPilot.PilotSkills(skillName)
                            curLevel = mySkill.Level
                        End If
                        mnuIncreaseLevel.Enabled = True : btnRBIncreaseLevel.Enabled = True
                        mnuDecreaseLevel.Enabled = True : btnRBDecreaseLevel.Enabled = True
                        mnuDeleteFromQueue.Enabled = True : btnRBDeleteSkill.Enabled = True
                        mnuMoveUpQueue.Enabled = True : btnRBMoveUpQueue.Enabled = True
                        mnuMoveDownQueue.Enabled = True : btnRBMoveDownQueue.Enabled = True
                        btnRBAddSkill.Enabled = False : btnRBSplitQueue.Enabled = True
                        Me.mnuViewDetails.Enabled = True
                        Me.mnuForceTraining.Enabled = True
                        If curTLevel = 5 Or curLevel = 5 Then
                            mnuIncreaseLevel.Enabled = False : btnRBIncreaseLevel.Enabled = False
                        End If
                        If curTLevel - 1 <= curFLevel Or curTLevel <= curLevel Then
                            mnuDecreaseLevel.Enabled = False : btnRBDecreaseLevel.Enabled = False
                        End If
                        If activeLVW.SelectedItems(0).Index = 0 Then
                            mnuMoveUpQueue.Enabled = False : btnRBMoveUpQueue.Enabled = False
                        End If

                        ' Check if the skill is a pre-req
                        If activeLVW.SelectedItems(0).BackColor = Color.LightSteelBlue Then
                            If activeLVW.SelectedItems(0).SubItems(4).Text = "100" Then
                                mnuDeleteFromQueue.Enabled = True : btnRBDeleteSkill.Enabled = True
                            Else
                                mnuDeleteFromQueue.Enabled = False : btnRBDeleteSkill.Enabled = False
                            End If
                        End If
                        ' Check if the skill is at the bottom of the list
                        If activeLVW.SelectedItems(0).Index = activeLVW.Items.Count - 1 Then
                            mnuMoveDownQueue.Enabled = False : btnRBMoveDownQueue.Enabled = False
                        End If

                        ' Check if there are multiple levels trained on the skill
                        Select Case curTLevel - curFLevel
                            Case 1
                                Me.mnuSeparateAllLevels.Enabled = False
                                Me.mnuSeparateBottomLevel.Enabled = False
                                Me.mnuSeparateTopLevel.Enabled = False
                                Me.mnuSeperateLevelSep.Visible = True
                            Case 2
                                Me.mnuSeparateAllLevels.Enabled = True
                                Me.mnuSeparateBottomLevel.Enabled = False
                                Me.mnuSeparateTopLevel.Enabled = False
                                Me.mnuSeperateLevelSep.Visible = True
                            Case Is > 2
                                Me.mnuSeparateAllLevels.Enabled = True
                                Me.mnuSeparateBottomLevel.Enabled = True
                                Me.mnuSeparateTopLevel.Enabled = True
                                Me.mnuSeperateLevelSep.Visible = True
                        End Select
                        If mnuSeparateAllLevels.Enabled = False And mnuSeparateBottomLevel.Enabled = False And mnuSeparateTopLevel.Enabled = False Then
                            mnuSeparateLevels.Enabled = False
                        Else
                            mnuSeparateLevels.Enabled = True
                        End If

                        ' Adjust for if the training skill
                        If displayPilot.Training = True And activeLVW.IncludeCurrentTraining = True Then
                            If activeLVW.SelectedItems(0).Index = 0 Then
                                mnuIncreaseLevel.Enabled = False : btnRBIncreaseLevel.Enabled = False
                                mnuDecreaseLevel.Enabled = False : btnRBDecreaseLevel.Enabled = False
                                mnuDeleteFromQueue.Enabled = False : btnRBDeleteSkill.Enabled = False
                                mnuMoveUpQueue.Enabled = False : btnRBMoveUpQueue.Enabled = False
                                mnuMoveDownQueue.Enabled = False : btnRBMoveDownQueue.Enabled = False
                            Else
                                If activeLVW.SelectedItems(0).Index = 1 Then
                                    mnuMoveUpQueue.Enabled = False : btnRBMoveUpQueue.Enabled = False
                                End If
                            End If
                        End If
                    Case Is > 1
                        mnuIncreaseLevel.Enabled = False : btnRBIncreaseLevel.Enabled = False
                        mnuDecreaseLevel.Enabled = False : btnRBDecreaseLevel.Enabled = False
                        mnuDeleteFromQueue.Enabled = True : btnRBDeleteSkill.Enabled = True
                        mnuMoveUpQueue.Enabled = False : btnRBMoveUpQueue.Enabled = False
                        mnuMoveDownQueue.Enabled = False : btnRBMoveDownQueue.Enabled = False
                        btnRBAddSkill.Enabled = False : btnRBSplitQueue.Enabled = True
                        btnExportEMPFile.Enabled = True
                        Me.mnuViewDetails.Enabled = False
                        Me.mnuForceTraining.Enabled = False
                        mnuSeparateLevels.Enabled = True
                        Me.mnuSeparateAllLevels.Enabled = True
                        Me.mnuSeparateBottomLevel.Enabled = False
                        Me.mnuSeparateTopLevel.Enabled = False
                        Me.mnuSeperateLevelSep.Visible = True
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
        ' Reset the redraw flag
        redrawingOptions = False
    End Sub
    Public Sub UpdateTraining()
        ' Only perform this if the form isn't starting
        If startup = False Then
            If displayPilot.Training = True Then
                If Me.tabQueues.Tabs.Count > 1 Then
                    For Each ti As DevComponents.DotNetBar.TabItem In Me.tabQueues.Tabs
                        If ti.Name <> "tabSummary" Then
                            Dim tq As TrainingQueue = CType(tabQueues.Tabs.Item(ti.Name).AttachedControl, TrainingQueue)
                            Dim cLabel As Label = tq.lblQueueTime
                            Dim cLVW As EveHQ.DragAndDropListView = tq.lvQueue
                            Dim newQ As Core.EveHQSkillQueue = displayPilot.TrainingQueues(ti.Name)
                            If newQ IsNot Nothing Then
                                Dim bIncludeSkill As Boolean = newQ.IncCurrentTraining
                                If bIncludeSkill Then
                                    If cLVW.Items.Count > 0 Then
                                        If Core.HQ.SkillListID.ContainsKey(displayPilot.TrainingSkillID) = True Then
                                            Dim myCurSkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID))
                                            Dim baseSkill As Core.EveSkill = Core.HQ.SkillListName(myCurSkill.Name)
                                            Dim clevel As Integer = displayPilot.TrainingSkillLevel
                                            Dim cTime As Double = displayPilot.TrainingCurrentTime
                                            Dim strTime As String = Core.SkillFunctions.TimeToString(cTime)
                                            Dim endtime As Date = displayPilot.TrainingEndTime
                                            Dim percent As Integer = 0
                                            percent = CInt(Int((myCurSkill.SP + displayPilot.TrainingCurrentSP - baseSkill.LevelUp(clevel - 1)) / (baseSkill.LevelUp(clevel) - baseSkill.LevelUp(clevel - 1)) * 100))
                                            If (percent > 100) Then
                                                percent = 100
                                            End If

                                            Dim lvi As ListViewItem = Nothing
                                            For Each lvi In cLVW.Items
                                                If lvi.Text = myCurSkill.Name Then
                                                    Exit For
                                                End If
                                            Next
                                            If lvi.SubItems.ContainsKey("Percent") Then
                                                lvi.SubItems("Percent").Text = CStr(percent)
                                            End If
                                            If lvi.SubItems.ContainsKey("TrainTime") Then
                                                lvi.SubItems("TrainTime").Tag = cTime
                                                lvi.SubItems("TrainTime").Text = CStr(strTime)
                                            End If

                                            ' Calculate total time
                                            If cLVW.Items.Count > 0 Then
                                                Dim totalTime As Double = 0
                                                For count As Integer = 0 To cLVW.Items.Count - 1
                                                    If lvi.SubItems.ContainsKey("TrainTime") Then
                                                        totalTime += CLng(cLVW.Items(count).SubItems("TrainTime").Tag)
                                                    End If
                                                Next
                                                cLabel.Tag = totalTime.ToString
                                                cLabel.Text = Core.SkillFunctions.TimeToString(totalTime)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                If Core.HQ.SkillListID.ContainsKey(displayPilot.TrainingSkillID) = True Then
                    Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(displayPilot.TrainingSkillID)
                    If displayPilot.Training = True And lvwDetails.Items(0).SubItems(1).Text = displayPilot.TrainingSkillName Then
                        lvwDetails.Items(8).SubItems(1).Text = Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                        Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(cSkill.Name)
                        lvwDetails.Items(7).SubItems(1).Text = (mySkill.SP + displayPilot.TrainingCurrentSP).ToString("N0")
                        Dim totalTime As Long = 0
                        For toLevel As Integer = 1 To 5
                            Select Case toLevel
                                Case displayPilot.TrainingSkillLevel
                                    totalTime += displayPilot.TrainingCurrentTime
                                Case Is > displayPilot.TrainingSkillLevel
                                    totalTime = CLng(totalTime + Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, toLevel, toLevel - 1))
                            End Select
                            lvwTimes.Items(toLevel - 1).SubItems(3).Text = Core.SkillFunctions.TimeToString(totalTime)
                        Next
                    End If
                End If
                ' Update the queue summary data
                For Each newQ As Core.EveHQSkillQueue In displayPilot.TrainingQueues.Values
                    Try
                        Dim tq As TrainingQueue = CType(Me.tabQueues.Tabs(newQ.Name).AttachedControl, TrainingQueue)
                        Dim tTime As Double = CDbl(tq.lblQueueTime.Tag)
                        lvQueues.Items(newQ.Name).SubItems(2).Tag = tTime
                        lvQueues.Items(newQ.Name).SubItems(2).Text = (Core.SkillFunctions.TimeToString(tTime))
                        Dim qTime As Double = tTime
                        Dim bIncludeSkill As Boolean = newQ.IncCurrentTraining
                        If bIncludeSkill Then
                            qTime = tTime - displayPilot.TrainingCurrentTime
                        End If
                        lvQueues.Items(newQ.Name).SubItems(3).Tag = qTime
                        lvQueues.Items(newQ.Name).SubItems(3).Text = Core.SkillFunctions.TimeToString(qTime)
                        Dim eTime As Date = Now.AddSeconds(tTime)
                        lvQueues.Items(newQ.Name).SubItems(4).Text = (Format(eTime, "ddd") & " " & eTime.ToString)
                    Catch e As Exception
                        ' Error will most likely be if a skill queue is in the process of deletion.
                    End Try
                Next
                If Me.selQTime > 0 Then
                    lblTotalQueueTime.Text = "Selected Queue Time: " & Core.SkillFunctions.TimeToString(Me.selQTime + displayPilot.TrainingCurrentTime) & " (" & Core.SkillFunctions.TimeToString(Me.selQTime) & ")"
                Else
                    lblTotalQueueTime.Text = "No Queue Selected"
                End If
            End If
        End If

    End Sub
#End Region

#Region "Skill Tree UI Functions"
    Private Sub cboFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFilter.SelectedIndexChanged
        Call LoadSkillTree()
    End Sub
    Private Sub tvwSkillList_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwSkillList.AfterSelect
        btnRBIncreaseLevel.Enabled = False
        btnRBDecreaseLevel.Enabled = False
        btnRBDeleteSkill.Enabled = False
        btnRBMoveUpQueue.Enabled = False
        btnRBMoveDownQueue.Enabled = False
        If activeQueue IsNot Nothing Then
            btnRBAddSkill.Enabled = True
        Else
            btnRBAddSkill.Enabled = False
        End If
        If e.Node.Parent IsNot Nothing Or (e.Node.Parent Is Nothing And usingFilter = False) Then
            Dim skillID As Integer = CInt(e.Node.Name)
            Call ShowSkillDetails(skillID)
        End If
    End Sub
    Private Sub tvwSkillList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvwSkillList.DoubleClick
        If tvwSkillList.SelectedNode.Level = 1 Or (tvwSkillList.SelectedNode.Level = 0 And usingFilter = False) Then
            Dim skillID As Integer = CInt(tvwSkillList.SelectedNode.Name)
            frmSkillDetails.DisplayPilotName = displayPilot.Name
            Call frmSkillDetails.ShowSkillDetails(skillID)
        End If
    End Sub
    Private Sub tvwSkillList_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles tvwSkillList.ItemDrag
        Dim addSkill As DragItemData = New DragItemData(Me.activeLVW)
        Dim newLvItem As ListViewItem = ConvertTreeNode(CType(e.Item, TreeNode))
        addSkill.DragItems.Add(newLvItem)
        DoDragDrop(addSkill, (DragDropEffects.Move Or (DragDropEffects.Copy Or DragDropEffects.Scroll)))
    End Sub
    Private Sub tvwSkillList_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSkillList.NodeMouseClick
        tvwSkillList.SelectedNode = e.Node
    End Sub
    Private Function ConvertTreeNode(ByVal skillNode As TreeNode) As ListViewItem
        Dim newItem As ListViewItem = New ListViewItem
        newItem.Text = "##" & skillNode.Text
        Return newItem
    End Function
    Private Sub cboFilter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilter.TextChanged
        If cboFilter.Items.Contains(cboFilter.Text) = True Then
            usingFilter = True
        Else
            usingFilter = False
            Call LoadSkillTreeSearch()
        End If
    End Sub
    Private Sub chkOmitQueuesSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOmitQueuesSkills.CheckedChanged
        If chkOmitQueuesSkills.Checked = True Then
            omitQueuedSkills = True
        Else
            omitQueuedSkills = False
        End If
        If usingFilter = True Then
            Call LoadSkillTree()
        Else
            Call LoadSkillTreeSearch()
        End If
    End Sub
    Private Sub LoadSkillTreeSearch()
        If Len(cboFilter.Text) > 1 Then
            Dim strSearch As String = cboFilter.Text.Trim.ToLower
            Dim results As New SortedList(Of String, String)
            Dim newSkill As Core.EveSkill
            For Each newSkill In Core.HQ.SkillListID.Values
                If newSkill.Name.ToLower.Contains(strSearch) Or newSkill.Description.ToLower.Contains(strSearch) Then
                    results.Add(newSkill.Name, newSkill.Name)
                End If
            Next

            tvwSkillList.BeginUpdate()
            tvwSkillList.Nodes.Clear()
            For Each item As String In results.Values
                newSkill = Core.HQ.SkillListName(item)
                If newSkill.GroupID <> 505 And newSkill.Published = True Then
                    Dim skillNode As TreeNode = New TreeNode
                    skillNode.Text = newSkill.Name
                    skillNode.Name = CStr(newSkill.ID)
                    If displayPilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                        Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(newSkill.Name)
                        skillNode.ImageIndex = mySkill.Level
                        skillNode.SelectedImageIndex = mySkill.Level
                    Else
                        skillNode.ImageIndex = 10
                        skillNode.SelectedImageIndex = 10
                    End If

                    If omitQueuedSkills = False Then
                        tvwSkillList.Nodes.Add(skillNode)
                    Else
                        Dim inQ As Boolean = False
                        For Each skillQ As Core.EveHQSkillQueue In displayPilot.TrainingQueues.Values
                            If inQ = True Then Exit For
                            Dim sQ As Dictionary(Of String, Core.EveHQSkillQueueItem) = skillQ.Queue
                            For Each skillQueueItem As Core.EveHQSkillQueueItem In sQ.Values
                                If newSkill.Name = skillQueueItem.Name Then
                                    inQ = True
                                    Exit For
                                End If
                            Next
                        Next
                        If inQ = False Then
                            tvwSkillList.Nodes.Add(skillNode)
                        End If
                    End If

                End If
            Next
            tvwSkillList.EndUpdate()
        End If
    End Sub
    Private Sub btnExpandAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExpandAll.Click
        tvwSkillList.BeginUpdate()
        tvwSkillList.ExpandAll()
        tvwSkillList.EndUpdate()
    End Sub
    Private Sub btnCollapseAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCollapseAll.Click
        tvwSkillList.BeginUpdate()
        tvwSkillList.CollapseAll()
        tvwSkillList.EndUpdate()
    End Sub
#End Region

#Region "Skill Queue UI Routines"
    Private Sub activeLVW_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Delete
                If activeQueue.IncCurrentTraining = True And activeLVW.SelectedItems.Count = 1 Then
                    If activeLVW.SelectedItems(0).Index = 0 Then
                        ' Do nothing
                    Else
                        Call Me.DeleteFromQueueOption()
                    End If
                Else
                    Call Me.DeleteFromQueueOption()
                End If
            Case Keys.Up
                If e.Control = True Then
                    Call Me.MoveUpQueue()
                    e.SuppressKeyPress = True
                End If
            Case Keys.Down
                If e.Control = True Then
                    Call Me.MoveDownQueue()
                    e.SuppressKeyPress = True
                End If
        End Select
    End Sub
    Private Sub activeLVW_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim skillID As Integer = Core.SkillFunctions.SkillNameToID(activeLVW.SelectedItems(0).Text)
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub activeLVW_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
        Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub activeLVW_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
        ' Make sure that the format is a treenode
        If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", False) = True Or e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", False) = True Then
            ' Allow drop.
            e.Effect = DragDropEffects.Copy
        Else
            ' Do not allow drop.
            e.Effect = DragDropEffects.None
        End If
    End Sub
    Private Sub activeLVW_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs)
        DoDragDrop(e.Item, DragDropEffects.Copy)
    End Sub
    Private Sub activeLVW_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        ' Establish the sort direction and store it for later comparison and use
        Dim SortDirection As New Core.SortDirection
        If CInt(activeLVW.SortColumn) = e.Column Then
            activeLVW.SortColumn = -1
            SortDirection = Core.SortDirection.Descending
        Else
            activeLVW.SortColumn = e.Column
            SortDirection = Core.SortDirection.Ascending
        End If
        ' We are going to get the column text and base the sort on this!
        Select Case activeLVW.Columns(e.Column).Text
            Case "Skill Name"
                Call SortQueue("Name", SortDirection) ' Sort on skill name
            Case "Cur Lvl"
                Call SortQueue("CurLevel", SortDirection)
            Case "From Lvl"
                Call SortQueue("FromLevel", SortDirection)
            Case "To Lvl"
                Call SortQueue("ToLevel", SortDirection)
            Case "%"
                Call SortQueue("Percent", SortDirection)
            Case "Training Time"
                Call SortQueue("TrainTime", SortDirection) ' Sort on training time
            Case "Time To Complete"
                Call SortQueue("TimeBeforeTrained", SortDirection) ' Sort on time to level
            Case "Rank"
                Call SortQueue("Rank", SortDirection)
            Case "Pri Attr"
                Call SortQueue("PAtt", SortDirection)
            Case "Sec Attr"
                Call SortQueue("SAtt", SortDirection)
            Case "SP /hour", "SP /day", "SP /week", "SP /mnth", "SP /year"
                Call SortQueue("SPRate", SortDirection)
            Case "SP Added"
                Call SortQueue("SPTrained", SortDirection)
            Case "Notes"
                Call SortQueue("Notes", SortDirection)
        End Select
    End Sub
    Private Sub activeLVW_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Call Me.RedrawOptions()
    End Sub
    Private Sub activeLVW_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If activeLVW.SelectedItems.Count <> 0 Then
            Call Me.RedrawOptions()
            Dim skillName As String = activeLVW.SelectedItems(0).Text
            Dim skillID As Integer = Core.SkillFunctions.SkillNameToID(skillName)
            Call Me.ShowSkillDetails(skillID)
        End If
    End Sub
    Private Sub SortQueue(ByVal PrimarySortColumn As String, ByVal SortDirection As Core.SortDirection)
        ' Get the sorted queue list from the list of saved sorted queues
        Dim testQueue As ArrayList = sortedQueues(activeQueueName)
        ' Initialise a new ClassSorter instance and add a standard SortClass (i.e. sort method)
        Dim myClassSorter As New Core.ClassSorter(PrimarySortColumn, SortDirection)
        ' Always sort by name to handle similarly ranked items in the first sort
        myClassSorter.SortClasses.Add(New Core.SortClass("Name", SortDirection))
        ' Sort the class
        testQueue.Sort(myClassSorter)
        ' Call the TidyQueue function to set the pre-built queue to the revised sorted one
        Core.SkillQueueFunctions.TidyQueue(displayPilot, activeQueue, testQueue)
        ' Now we need to refresh the queue again to calculate the correct skill orders and pre-reqs
        Call Me.RefreshTraining(activeQueueName)
    End Sub
#End Region

#Region "Certificate Planning"

    Private Sub cboCertFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCertFilter.SelectedIndexChanged
        Call Me.LoadCertificateTree()
    End Sub
    Public Sub LoadCertificateTree()
        Dim filter As Integer = cboCertFilter.SelectedIndex
        ' Save current open nodes
        oNodes.Clear()
        For Each oNode As TreeNode In tvwCertList.Nodes
            If oNode.IsExpanded = True Then
                oNodes.Add(oNode.Name)
            End If
        Next
        tvwCertList.BeginUpdate()
        tvwCertList.Nodes.Clear()
        tvwCertList.Sorted = False
        Call Me.LoadCertGroups()
        Call Me.LoadFilteredCerts(filter)
        Call Me.ShowCertGroups()
        For Each oNode As String In oNodes
            If tvwCertList.Nodes.ContainsKey(oNode) = True Then
                tvwCertList.Nodes(oNode).Expand()
            End If
        Next
        tvwCertList.Sorted = True
        tvwCertList.EndUpdate()
        tvwCertList.Refresh()
    End Sub
    Private Sub LoadCertGroups()
        certListNodes.Clear()
        For Each CertCat As CertificateCategory In StaticData.CertificateCategories.Values
            Dim groupNode As TreeNode = New TreeNode
            groupNode.Name = CertCat.Id.ToString
            groupNode.Text = CertCat.Name
            groupNode.ImageIndex = 8
            groupNode.SelectedImageIndex = 8
            certListNodes.Add(groupNode.Name, groupNode)
        Next
    End Sub
    Private Sub LoadFilteredCerts(ByVal filter As Integer)
        Dim groupNode As TreeNode
        Dim addCert As Boolean
        For Each newCert As Certificate In StaticData.Certificates.Values
            addCert = False
            groupNode = CType(certListNodes.Item(newCert.CategoryId.ToString), TreeNode)
            Select Case filter
                Case 0
                    addCert = True
                Case 1
                    If displayPilot.Certificates.Contains(newCert.Id) = True Then
                        addCert = True
                    End If
                Case 2
                    If displayPilot.Certificates.Contains(newCert.Id) = False Then
                        addCert = True
                    End If
                Case 3 To 7
                    If newCert.Grade = filter - 2 Then
                        addCert = True
                    End If
                Case 8 To 12
                    If newCert.Grade = filter - 7 And displayPilot.Certificates.Contains(newCert.Id) = False Then
                        addCert = True
                    End If
            End Select
            If addCert = True Then
                Dim certNode As New TreeNode
                certNode.Text = StaticData.CertificateClasses(newCert.ClassId.ToString).Name & " (" & newCert.Grade & " - " & CertGrades(newCert.Grade) & ")"
                certNode.Name = newCert.Id.ToString
                If displayPilot.Certificates.Contains(newCert.Id) = True Then
                    certNode.ImageIndex = newCert.Grade
                    certNode.SelectedImageIndex = newCert.Grade
                Else
                    certNode.ImageIndex = 10
                    certNode.SelectedImageIndex = 10
                    ' Check if we have the pre-reqs for the certificate
                    Dim CanClaimCert As Boolean = True
                    For Each reqSkill As Integer In newCert.RequiredSkills.Keys
                        If Core.SkillFunctions.IsSkillTrained(displayPilot, Core.SkillFunctions.SkillIDToName(reqSkill), newCert.RequiredSkills(reqSkill)) = False Then
                            CanClaimCert = False
                            Exit For
                        End If
                    Next
                    If CanClaimCert = True Then
                        For Each ReqCert As String In newCert.RequiredCertificates.Keys
                            If displayPilot.Certificates.Contains(CInt(ReqCert)) = False Then
                                CanClaimCert = False
                                Exit For
                            End If
                        Next
                    End If
                    If CanClaimCert = True Then
                        certNode.ForeColor = Color.LimeGreen
                    End If
                End If
                groupNode.Nodes.Add(certNode)
            End If
        Next
    End Sub
    Private Sub ShowCertGroups()
        For Each groupNode As TreeNode In certListNodes.Values
            If groupNode.Nodes.Count > 0 Then
                tvwCertList.Nodes.Add(groupNode)
            End If
        Next
    End Sub

#End Region

#Region "Certificate Menu Routines"

    Private Sub tvwCertList_NodeMouseClick(ByVal sender As Object, ByVal e As Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwCertList.NodeMouseClick
        tvwCertList.SelectedNode = e.Node
    End Sub

    Private Sub ctxCertDetails_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxCertDetails.Opening
        Dim curNode As TreeNode
        curNode = tvwCertList.SelectedNode
        If curNode IsNot Nothing Then
            ' Reset grades
            For grade As Integer = 1 To 5
                mnuAddCertToQueue.DropDownItems("mnuAddCertToQueue" & grade).Enabled = False
            Next
            Dim certName As String = ""
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
            If activeQueueName = "" Then
                mnuAddCertGroupToQueue.Enabled = False
                mnuAddCertToQueue.Enabled = False
            Else
                mnuAddCertGroupToQueue.Enabled = True
                mnuAddCertToQueue.Enabled = True
                ' Determine enabled menu items of adding to queue
                Dim selCert As Certificate = StaticData.Certificates(certID)
                Dim selCertClass As Integer = selCert.ClassID
                For Each testCert As Certificate In StaticData.Certificates.Values
                    If testCert.ClassId = selCertClass Then
                        ' Check if the pilot has it
                        If displayPilot.Certificates.Contains(testCert.Id) = False Then
                            mnuAddCertToQueue.DropDownItems("mnuAddCertToQueue" & testCert.Grade).Enabled = True
                        Else
                            mnuAddCertToQueue.DropDownItems("mnuAddCertToQueue" & testCert.Grade).Enabled = False
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub mnuViewCertDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewCertDetails.Click
        Dim certID As Integer = CInt(mnuCertName.Tag)
        frmCertificateDetails.DisplayPilotName = displayPilot.Name
        frmCertificateDetails.ShowCertDetails(certID)
    End Sub

    Private Sub mnuAddCertToQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCertToQueue1.Click, mnuAddCertToQueue2.Click, mnuAddCertToQueue3.Click, mnuAddCertToQueue4.Click, mnuAddCertToQueue5.Click
        ' Get the certificate details
        Dim grade As Integer = CInt(CType(sender, ToolStripItem).Name.Substring(CType(sender, ToolStripItem).Name.Length - 1, 1))
        Dim certID As Integer = CInt(mnuCertName.Tag)
        Dim certClass As Integer = StaticData.Certificates(certID).ClassId
        For Each cert As Certificate In StaticData.Certificates.Values
            If cert.ClassId = certClass Then
                If cert.Grade = grade Then
                    Call AddCertSkills(cert)
                End If
            End If
        Next
        ' Refresh our training queue
        Call Me.RefreshTraining(activeQueueName)
    End Sub

    Private Sub AddCertSkills(ByVal cert As Certificate)
        Dim reqSkills As SortedList(Of Integer, Integer) = cert.RequiredSkills
        For Each reqSkill As Integer In reqSkills.Keys
            Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, CStr(Core.SkillFunctions.SkillIDToName(reqSkill)), activeQueue.Queue.Count + 1, activeQueue, CInt(reqSkills(reqSkill)), True, True, "Certificate: " & StaticData.CertificateClasses(cert.ClassId.ToString).Name)
        Next
        ' Get a list of the certs that are required
        For Each reqCertID As Integer In cert.RequiredCertificates.Keys
            Call AddCertSkills(StaticData.Certificates(reqCertID))
        Next
    End Sub

    Private Sub mnuAddCertGroupToQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCertGroupToQueue1.Click, mnuAddCertGroupToQueue2.Click, mnuAddCertGroupToQueue3.Click, mnuAddCertGroupToQueue4.Click, mnuAddCertGroupToQueue5.Click
        ' Get the Grade required
        Dim grade As Integer = CInt(CType(sender, ToolStripItem).Name.Substring(CType(sender, ToolStripItem).Name.Length - 1, 1))
        Dim certCat As String = mnuCertName.Tag.ToString
        For Each cert As Certificate In StaticData.Certificates.Values
            If cert.CategoryId = CInt(certCat) Then
                If cert.Grade = grade Then
                    Call AddCertSkills(cert)
                End If
            End If
        Next
        ' Refresh our training queue
        Call Me.RefreshTraining(activeQueueName)
    End Sub

#End Region

#Region "Skill Information Display"

    Private Sub ShowSkillDetails(ByVal skillID As Integer)

        If displayPilot.SkillPoints + displayPilot.TrainingCurrentSP < TrainingThreshold Then
            TrainingBonus = 2
        Else
            TrainingBonus = 1
        End If

        Call PrepareDetails(skillID)
        Call PrepareTree(skillID)
        Call PrepareDepends(skillID)
        Call PrepareDescription(skillID)
        Call PrepareQueues(skillID)
        Call PrepareSPs(skillID)
        Call PrepareTimes(skillID)

    End Sub
    Private Sub PrepareDetails(ByVal skillID As Integer)

        Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)
        lvwDetails.Groups(1).Header = "Pilot Specific - " & displayPilot.Name

        With lvwDetails
            Dim mySkill As Core.EveHQPilotSkill
            Dim myGroup As Core.SkillGroup
            If StaticData.TypeGroups.ContainsKey(cSkill.GroupID) = True Then
                Dim groupName As String = StaticData.TypeGroups(cSkill.GroupID)
                If Core.HQ.SkillGroups.ContainsKey(groupName) = True Then
                    myGroup = Core.HQ.SkillGroups(groupName)
                Else
                    myGroup = Nothing
                End If
            Else
                myGroup = Nothing
            End If
            Dim cLevel, cSP, cTime, cRate As String
            If Core.HQ.Settings.Pilots.Count > 0 And displayPilot.Updated = True Then
                If displayPilot.PilotSkills.ContainsKey(cSkill.Name) = False Then
                    cLevel = "0" : cSP = "0" : cTime = Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, 1, ))
                    cRate = CStr(Core.SkillFunctions.CalculateSPRate(displayPilot, cSkill))
                Else
                    mySkill = displayPilot.PilotSkills(cSkill.Name)
                    cLevel = mySkill.Level.ToString
                    If displayPilot.Training = True And displayPilot.TrainingSkillID = Core.SkillFunctions.SkillNameToID(cSkill.Name) Then
                        cSP = CStr(mySkill.SP + displayPilot.TrainingCurrentSP)
                    Else
                        cSP = mySkill.SP.ToString
                    End If
                    If displayPilot.Training = True And displayPilot.TrainingSkillName = cSkill.Name Then
                        cTime = Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    Else
                        cTime = Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, 0, ))
                    End If
                    cRate = CStr(Core.SkillFunctions.CalculateSPRate(displayPilot, cSkill))
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

        Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)
        Dim curSkill As Integer = CInt(skillID)
        Dim curLevel As Integer = 0
        Dim counter As Integer = 0
        Dim curNode As TreeNode = New TreeNode

        ' Write the skill we are querying as the first (parent) node
        curNode.Text = cSkill.Name
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        If Core.HQ.Settings.Pilots.Count > 0 And displayPilot.Updated = True Then
            If displayPilot.PilotSkills.ContainsKey(cSkill.Name) Then
                Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(cSkill.Name)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
                If skillTrained = True Then
                    curNode.ForeColor = Color.LimeGreen
                    curNode.ToolTipText = "Already Trained"
                Else
                    Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
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
                Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
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
                Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
            Next
        End If
        tvwReqs.ExpandAll()
        tvwReqs.EndUpdate()
    End Sub
    Private Sub AddPreReqsToTree(ByVal newSkill As Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode)
        Dim skillTrained As Boolean
        Dim myLevel As Integer
        Dim newNode As TreeNode = New TreeNode
        newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
        newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
        ' Check status of this skill
        If Core.HQ.Settings.Pilots.Count > 0 And displayPilot.Updated = True Then
            skillTrained = False
            myLevel = 0
            If displayPilot.PilotSkills.ContainsKey(newSkill.Name) Then
                Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(newSkill.Name)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
            End If
            If skillTrained = True Then
                newNode.ForeColor = Color.LimeGreen
                newNode.ToolTipText = "Already Trained"
            Else
                Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(displayPilot, newSkill.Name, curLevel)
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
            If StaticData.SkillUnlocks.ContainsKey(skillID & "." & CStr(lvl)) = True Then
                Dim itemUnlocked As List(Of String) = StaticData.SkillUnlocks(skillID & "." & CStr(lvl))
                For Each item As String In itemUnlocked
                    Dim newItem As New ListViewItem
                    Dim toolTipText As New StringBuilder
                    itemData = item.Split(CChar("_"))
                    catID = StaticData.GroupCats(CInt(itemData(1)))
                    newItem.Group = lvwDepend.Groups("Cat" & catID)
                    newItem.Text = StaticData.Types(CInt(itemData(0))).Name
                    newItem.Name = itemData(0)
                    newItem.Tag = itemData(0)
                    Dim skillUnlocked As List(Of String) = StaticData.ItemUnlocks(itemData(0))
                    Dim allTrained As Boolean = True
                    For Each skillPair As String In skillUnlocked
                        skillData = skillPair.Split(CChar("."))
                        skillName = Core.SkillFunctions.SkillIDToName(CInt(skillData(0)))
                        If CInt(skillData(0)) <> skillID Then
                            toolTipText.Append(skillName)
                            toolTipText.Append(" (Level ")
                            toolTipText.Append(skillData(1))
                            toolTipText.Append("), ")
                        End If
                        If Core.SkillFunctions.IsSkillTrained(displayPilot, skillName, CInt(skillData(1))) = False Then
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
                    newItem.Group = lvwDepend.Groups("CatCerts")
                    Dim cert As Certificate = StaticData.Certificates(item)
                    newItem.Tag = cert.Id
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
                            newItem.ToolTipText &= StaticData.CertificateClasses(reqCert.ClassId.ToString).Name
                            Select Case reqCert.Grade
                                Case 1
                                    newItem.ToolTipText &= " (Basic), "
                                Case 2
                                    newItem.ToolTipText &= " (Standard), "
                                Case 3
                                    newItem.ToolTipText &= " (Improved), "
                                Case 4
                                    newItem.ToolTipText &= " (Advanced), "
                                Case 5
                                    newItem.ToolTipText &= " (Elite), "
                            End Select
                        End If
                    Next
                    If newItem.ToolTipText <> "" Then
                        newItem.ToolTipText = "Also Requires: " & newItem.ToolTipText
                        newItem.ToolTipText = newItem.ToolTipText.TrimEnd(", ".ToCharArray)
                    End If
                    If displayPilot.Certificates.Contains(cert.Id) = True Then
                        newItem.ForeColor = Color.Green
                    Else
                        newItem.ForeColor = Color.Red
                    End If
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
        For Each skillQ As Core.EveHQSkillQueue In displayPilot.TrainingQueues.Values
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
        Me.lblDescription.Text = cSkill.Description
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

        If Core.HQ.Settings.Pilots.Count > 0 And displayPilot.Updated = True Then
            Dim cskill As Core.EveSkill = Core.HQ.SkillListID(skillID)

            Dim timeToTrain As Long = 0

            For toLevel As Integer = 1 To 5
                Dim newGroup As ListViewItem = New ListViewItem
                newGroup.Text = toLevel.ToString
                newGroup.SubItems.Add(Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, toLevel - 1)))
                newGroup.SubItems.Add(Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, 0)))
                newGroup.SubItems.Add(Core.SkillFunctions.TimeToString(Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, -1)))
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
    Private Sub mnuViewItemDetailsInIB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetailsInIB.Click

        Dim PluginName As String = "EveHQ Item Browser"
        Dim itemID As String = mnuItemName.Tag.ToString
        Dim myPlugIn As Core.EveHQPlugIn = Core.HQ.Plugins(PluginName)
        Dim PluginFile As String = myPlugIn.FileName
        Dim PluginType As String = myPlugIn.FileType
        Dim runPlugIn As Core.IEveHQPlugIn

        Dim tp As DevComponents.DotNetBar.TabItem = Core.HQ.GetMDITab(PluginName)
        If tp Is Nothing Then
            Dim myAssembly As Reflection.Assembly = Reflection.Assembly.LoadFrom(PluginFile)
            Dim t As Type = myAssembly.GetType(PluginType)
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
        If ctxDepend.SourceControl Is Me.lvwDepend Then
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
        If displayPilot.Training = True Then
            Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(displayPilot.TrainingSkillID)
            If displayPilot.Training = True And lvwDetails.Items(0).SubItems(1).Text = displayPilot.TrainingSkillName Then
                lvwDetails.Items(8).SubItems(1).Text = Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(cSkill.Name)
                lvwDetails.Items(7).SubItems(1).Text = (mySkill.SP + displayPilot.TrainingCurrentSP).ToString("N0")
                Dim totalTime As Long = 0
                For toLevel As Integer = 1 To 5
                    Select Case toLevel
                        Case displayPilot.TrainingSkillLevel
                            totalTime += displayPilot.TrainingCurrentTime
                        Case Is > displayPilot.TrainingSkillLevel
                            totalTime = CLng(totalTime + Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, toLevel, toLevel - 1))
                    End Select
                    lvwTimes.Items(toLevel - 1).SubItems(3).Text = Core.SkillFunctions.TimeToString(totalTime)
                Next
            End If
        End If
    End Sub

#End Region

#Region "Skill Queue Modification Functions"
    Private Sub AddSkillToQueueOption()
        activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 0, False, False, "")
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub DeleteFromQueueOption()

        Dim lowestIndex As Integer = activeLVW.Items.Count
        Dim highestIndex As Integer = 0

        ' Get the skill name and levels
        For selItem As Integer = 0 To activeLVW.SelectedItems.Count - 1

            Dim keyName As String = activeLVW.SelectedItems(selItem).Name
            Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
            Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
            Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))

            ' Set the lowest and highest index
            lowestIndex = Math.Min(activeLVW.SelectedItems(selItem).Index, lowestIndex)
            highestIndex = Math.Max(activeLVW.SelectedItems(selItem).Index, highestIndex)

            ' Remove it from the queue
            Dim mySkill As New Core.EveHQSkillQueueItem
            If activeQueue.Queue.ContainsKey(keyName) = True Then
                mySkill = activeQueue.Queue(keyName)
                ' Delete the Skill
                Call Me.DeleteFromQueue(mySkill)
            End If
        Next
        ' See if we can set the next item, or the previous item if not
        If highestIndex + 1 <= activeLVW.Items.Count - 1 Then
            activeLVW.Items(highestIndex + 1).Selected = True
        Else
            If lowestIndex <> 0 Then
                activeLVW.Items(lowestIndex - 1).Selected = True
            End If
        End If
        ' Refresh the training view!
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub DeleteFromQueue(ByVal mySkill As Core.EveHQSkillQueueItem)
        Dim delPos As Integer = mySkill.Pos
        activeQueue.Queue.Remove(mySkill.Name & mySkill.FromLevel & mySkill.ToLevel)
        ' Reshuffle all the positions below
        For Each mySkill In activeQueue.Queue.Values
            If mySkill.Pos > delPos Then
                mySkill.Pos -= 1
            End If
        Next
    End Sub
    Private Sub IncreaseLevel()
        ' Store the index being used
        Dim oldIndex As Integer = activeLVW.SelectedItems(0).Index
        ' Get the skill name
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
        Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
        Dim ToLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))
        Dim myTSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)

        ' Check if we have another skill that can be affected by us increasing the level i.e. the same skill!
        For Each checkSkill As Core.EveHQSkillQueueItem In activeQueue.Queue.Values
            If myTSkill.Name = checkSkill.Name Then
                ' Check the "from" skill level matches
                If checkSkill.FromLevel = myTSkill.ToLevel Then
                    ' We have to decide what to do here, either increase the levels or merge
                    If checkSkill.ToLevel = checkSkill.FromLevel + 1 Then
                        ' We have to merge the items here so delete the new found one
                        Call Me.DeleteFromQueue(checkSkill)
                    Else
                        ' We have to increase the levels so decrease the new found one
                        ' Delete the existing item
                        activeQueue.Queue.Remove(checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel)
                        ' Adjust the "to" level
                        checkSkill.FromLevel += 1
                        ' Add the item back in at its new levels
                        activeQueue.Queue.Add(checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel, checkSkill)
                    End If
                End If
            End If
        Next

        ' Check if the skill has been trained and we wish to increase it to the next level
        If activeLVW.SelectedItems(0).Font.Strikeout = True Then
            Dim curLevel As Integer = 0
            If displayPilot.PilotSkills.ContainsKey(myTSkill.Name) Then
                curLevel = displayPilot.PilotSkills(myTSkill.Name).Level
            End If
            myTSkill.FromLevel = Math.Max(CInt(curLevel), myTSkill.FromLevel)
        End If

        ' Delete the existing item
        activeQueue.Queue.Remove(keyName)
        ' Adjust the "to" level
        myTSkill.ToLevel += 1
        ' Add the item back in at its new levels
        activeQueue.Queue.Add(myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel, myTSkill)
        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(oldIndex).Selected = True
        activeLVW.Items(oldIndex).EnsureVisible()
    End Sub
    Private Sub DecreaseLevel()
        ' Store the index being used
        Dim oldIndex As Integer = activeLVW.SelectedItems(0).Index
        ' Get the skill name
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
        Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
        Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))
        Dim myTSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
        ' Delete the existing item
        activeQueue.Queue.Remove(keyName)
        ' Adjust the "to" level
        myTSkill.ToLevel -= 1
        ' Add the item back in at its new levels
        activeQueue.Queue.Add(myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel, myTSkill)
        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(oldIndex).Selected = True
        activeLVW.Items(oldIndex).EnsureVisible()
    End Sub
    Private Sub MoveUpQueue()
        ' Store the keyname being used
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim sourceSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
        Dim oldPOS As Integer = sourceSkill.Pos
        Dim queueJump As Integer = 0
        Dim maxJump As Integer = 0
        If displayPilot.Training = True Then
            maxJump = 1
        Else
            maxJump = 0
        End If
        Dim si As Integer = 0
        Dim di As Integer = 0
        Dim newPOS As Integer = 0
        Do
            queueJump += 1
            si = sourceSkill.Pos
            di = si - queueJump
            Dim destSkill As New Core.EveHQSkillQueueItem
            For Each destSkill In activeQueue.Queue.Values
                If destSkill.Pos = di Then Exit For
            Next

            Dim din As String = destSkill.Name & destSkill.FromLevel & destSkill.ToLevel
            Dim sin As String = sourceSkill.Name & sourceSkill.FromLevel & sourceSkill.ToLevel

            Dim mySSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(sin)
            ' Move all the items up or down depending on position
            If si > di Then
                For Each moveSkill As Core.EveHQSkillQueueItem In activeQueue.Queue.Values
                    If moveSkill.Pos >= di And moveSkill.Pos < si Then
                        moveSkill.Pos += 1
                    End If
                Next
            Else
                For Each moveSkill As Core.EveHQSkillQueueItem In activeQueue.Queue.Values
                    If moveSkill.Pos > si And moveSkill.Pos <= di Then
                        moveSkill.Pos -= 1
                    End If
                Next
            End If
            ' Set the source skill to the new location
            mySSkill.Pos = di

            ' Check for movement in the queue
            Dim arrQueue As ArrayList = Core.SkillQueueFunctions.BuildQueue(displayPilot, activeQueue, False, True)
            Dim posSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
            newPOS = posSkill.Pos

        Loop Until oldPOS <> newPOS Or di = maxJump

        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(keyName).Selected = True
        activeLVW.Items(keyName).EnsureVisible()
    End Sub
    Private Sub MoveDownQueue()
        ' Store the keyname being used
        Dim skillsMoved As Integer = activeLVW.SelectedItems.Count
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim sourceSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
        Dim oldpos As Integer = sourceSkill.Pos
        Dim newpos As Integer = 0
        Dim di As Integer = 0
        Dim maxJump As Integer = activeQueue.Queue.Count
        Dim queueJump As Integer = 0
        Do
            queueJump += 1
            Dim si As Integer = sourceSkill.Pos
            di = si + queueJump
            Dim destSkill As New Core.EveHQSkillQueueItem
            For Each destSkill In activeQueue.Queue.Values
                If destSkill.Pos = di Then Exit For
            Next

            Dim din As String = destSkill.Name & destSkill.FromLevel & destSkill.ToLevel
            Dim sin As String = sourceSkill.Name & sourceSkill.FromLevel & sourceSkill.ToLevel

            Dim mySSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(sin)
            ' Move all the items up or down depending on position
            If si > di Then
                For Each moveSkill As Core.EveHQSkillQueueItem In activeQueue.Queue.Values
                    If moveSkill.Pos >= di And moveSkill.Pos < si Then
                        moveSkill.Pos += 1
                    End If
                Next
            Else
                For Each moveSkill As Core.EveHQSkillQueueItem In activeQueue.Queue.Values
                    If moveSkill.Pos > si And moveSkill.Pos <= di Then
                        moveSkill.Pos -= 1
                    End If
                Next

            End If
            ' Set the source skill to the new location
            mySSkill.Pos = di

            ' Check for movement in the queue
            Dim arrQueue As ArrayList = Core.SkillQueueFunctions.BuildQueue(displayPilot, activeQueue, False, True)
            Dim posSkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
            newpos = posSkill.Pos

        Loop Until oldpos <> newpos Or di = maxJump

        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(keyName).Selected = True
        activeLVW.Items(keyName).EnsureVisible()
    End Sub
    Private Sub ClearTrainingQueue()
        Dim reply As Integer
        reply = MessageBox.Show("Are you sure you want to delete the entire training queue?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            activeQueue.Queue.Clear()
            Call Me.RefreshTraining(activeQueueName)
        End If
    End Sub
    Private Sub ChangeLevel(ByVal selectedLevel As Integer)
        ' Store the index being used
        Dim oldIndex As Integer = activeLVW.SelectedItems(0).Index
        ' Get the skill name
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
        Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
        Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))
        Dim myTSkill As New Core.EveHQSkillQueueItem
        If activeQueue.Queue.ContainsKey(keyName) = True Then
            myTSkill = activeQueue.Queue(keyName)
            If selectedLevel < CInt(toLevel) Then
                ' Delete the existing item
                activeQueue.Queue.Remove(keyName)
                ' Adjust the "to" level
                myTSkill.ToLevel = selectedLevel
                ' Add the item back in at its new levels
                activeQueue.Queue.Add(myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel, myTSkill)
                Call Me.RefreshTraining(activeQueueName)
                activeLVW.Items(oldIndex).Selected = True
                activeLVW.Items(oldIndex).EnsureVisible()
            End If
            If selectedLevel > CInt(toLevel) Then
                ' Check if we have another skill that can be affected by us increasing the level i.e. the same skill!
                Dim checkSkill As New Core.EveHQSkillQueueItem
                For Each checkSkill In activeQueue.Queue.Values
                    If myTSkill.Name = checkSkill.Name Then ' Matched skill name
                        ' Check the "from" skill level matches
                        If checkSkill.FromLevel >= myTSkill.ToLevel Then
                            ' We have to decide what to do here, either increase the levels or merge
                            If checkSkill.ToLevel <= selectedLevel Then
                                ' We have to merge the items here so delete the new found one
                                Call Me.DeleteFromQueue(checkSkill)
                            Else
                                ' We have to increase the levels so decrease the new found one
                                ' (Only if the start skill is smaller than the selected skill)
                                If checkSkill.FromLevel < selectedLevel Then
                                    ' Delete the existing item
                                    activeQueue.Queue.Remove(checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel)
                                    ' Adjust the "to" level
                                    checkSkill.FromLevel = selectedLevel
                                    ' Add the item back in at its new levels
                                    activeQueue.Queue.Add(checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel, checkSkill)
                                End If
                            End If
                        End If
                    End If
                Next

                ' Check if the skill has been trained and we wish to increase it to the next level
                If activeLVW.SelectedItems(0).Font.Strikeout = True Then
                    Dim curLevel As Integer = 0
                    If displayPilot.PilotSkills.ContainsKey(myTSkill.Name) Then
                        curLevel = displayPilot.PilotSkills(myTSkill.Name).Level
                    End If
                    myTSkill.FromLevel = Math.Max(CInt(curLevel), myTSkill.FromLevel)
                End If

                ' Delete the existing item
                activeQueue.Queue.Remove(keyName)
                ' Adjust the "to" level
                myTSkill.ToLevel = selectedLevel
                ' Add the item back in at its new levels
                activeQueue.Queue.Add(myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel, myTSkill)
                Call Me.RefreshTraining(activeQueueName)
                activeLVW.Items(oldIndex).Selected = True
                activeLVW.Items(oldIndex).EnsureVisible()
            End If
        End If
    End Sub
    Private Sub SplitQueue()

        ' Check for some selection on the listview
        If activeLVW IsNot Nothing Then
            If activeLVW.SelectedItems.Count > 0 Then
                ' Build a new queue
                Dim selQueue As New Core.SkillQueue
                For Each LVI As ListViewItem In activeLVW.SelectedItems
                    Dim keyName As String = LVI.Name
                    Dim splitSkillQueueItem As Core.EveHQSkillQueueItem = CType(activeQueue.Queue(keyName).Clone, Core.EveHQSkillQueueItem)
                    selQueue.Queue.Add(splitSkillQueueItem, keyName)
                Next
                Dim myQueue As frmModifyQueues = New frmModifyQueues
                With myQueue
                    ' Load the account details into the text boxes
                    .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Queue
                    .btnAccept.Text = "Split" : .Tag = "Split"
                    .Text = "Split '" & activeQueue.Name & "' Queue Details"
                    .DisplayPilotName = displayPilot.Name
                    .ShowDialog()
                End With
                Call Me.RefreshAllTraining()
            End If
        End If

    End Sub
#End Region

#Region "Skill Queue Summary UI Functions"
    Private Sub GetSelectedQueueTimes()
        Try
            Dim newQueue As New Core.EveHQSkillQueue
            newQueue.Name = "tempMerge"
            newQueue.IncCurrentTraining = True
            newQueue.Primary = False
            newQueue.Queue = New Dictionary(Of String, Core.EveHQSkillQueueItem)
            For Each item As ListViewItem In lvQueues.SelectedItems
                Dim queueName As String = item.Name
                Dim oldQueue As Core.EveHQSkillQueue = displayPilot.TrainingQueues(queueName)
                If oldQueue.Primary = True Then newQueue.Primary = True
                For Each queueItem As Core.EveHQSkillQueueItem In oldQueue.Queue.Values
                    Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                    If newQueue.Queue.ContainsKey(keyName) = False Then
                        newQueue.Queue.Add(keyName, queueItem)
                    End If
                Next
            Next
            For Each curSkill As Core.EveHQSkillQueueItem In newQueue.Queue.Values
                If displayPilot.PilotSkills.ContainsKey(curSkill.Name) Then
                    Dim fromLevel As Integer = curSkill.FromLevel
                    Dim toLevel As Integer = curSkill.ToLevel
                    Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(curSkill.Name)
                    Dim pilotLevel As Integer = mySkill.Level
                    If pilotLevel >= toLevel Then
                        Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                        newQueue.Queue.Remove(oldKey)
                    End If
                End If
            Next
            Dim arrQueue As ArrayList = Core.SkillQueueFunctions.BuildQueue(displayPilot, newQueue, True, True)
            Dim qItem As New Core.SortedQueueItem
            Dim qTime As Double = 0
            For Each qItem In arrQueue
                qTime += CLng(qItem.TrainTime)
            Next
            Me.selQTime = qTime - displayPilot.TrainingCurrentTime
            lblTotalQueueTime.Text = "Selected Queue Time: " & Core.SkillFunctions.TimeToString(Me.selQTime + displayPilot.TrainingCurrentTime) & " (" & Core.SkillFunctions.TimeToString(Me.selQTime) & ")"
        Catch e As Exception
            MessageBox.Show("There was an error calculating the selected queue times.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
    Private Sub GetAllQueueTimes()
        Dim newQueue As New Core.EveHQSkillQueue
        newQueue.Name = "tempMerge"
        newQueue.IncCurrentTraining = True
        newQueue.Primary = False
        newQueue.Queue = New Dictionary(Of String, Core.EveHQSkillQueueItem)
        For Each item As ListViewItem In lvQueues.Items
            Dim queueName As String = item.Name
            Dim oldQueue As Core.EveHQSkillQueue = displayPilot.TrainingQueues(queueName)
            For Each queueItem As Core.EveHQSkillQueueItem In oldQueue.Queue.Values
                Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                If newQueue.Queue.ContainsKey(keyName) = False Then
                    newQueue.Queue.Add(keyName, queueItem)
                End If
            Next
        Next
        For Each curSkill As Core.EveHQSkillQueueItem In newQueue.Queue.Values
            If displayPilot.PilotSkills.ContainsKey(curSkill.Name) Then
                Dim fromLevel As Integer = curSkill.FromLevel
                Dim toLevel As Integer = curSkill.ToLevel
                Dim mySkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(curSkill.Name)
                Dim pilotLevel As Integer = mySkill.Level
                If pilotLevel >= toLevel Then
                    Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                    newQueue.Queue.Remove(oldKey)
                End If
            End If
        Next
        Dim arrQueue As ArrayList = Core.SkillQueueFunctions.BuildQueue(displayPilot, newQueue, True, True)
        Dim qItem As New Core.SortedQueueItem
        Dim QTime As Double = 0
        For Each qItem In arrQueue
            QTime += CLng(qItem.TrainTime)
        Next
        Me.totalQTime = QTime - displayPilot.TrainingCurrentTime
        lblTotalQueueTime.Text = "Total Queue Time: " & Core.SkillFunctions.TimeToString(Me.totalQTime + displayPilot.TrainingCurrentTime) & " (" & Core.SkillFunctions.TimeToString(Me.totalQTime) & ")"
    End Sub
    Private Sub lvQueues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvQueues.Click
        Select Case lvQueues.SelectedItems.Count
            Case 0
                Call ResetQueueOptions()
                Me.selQTime = 0
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
                Me.selQTime = CDbl(Me.lvQueues.Items(Me.lvQueues.SelectedIndices(0)).SubItems(2).Tag) - displayPilot.TrainingCurrentTime
                lblTotalQueueTime.Text = "Selected Queue Time: " & Core.SkillFunctions.TimeToString(Me.selQTime + displayPilot.TrainingCurrentTime) & " (" & Core.SkillFunctions.TimeToString(Me.selQTime) & ")"
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
                Call Me.GetSelectedQueueTimes()
        End Select
    End Sub
    Private Sub lvQueues_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvQueues.DoubleClick
        Dim selQ As String = lvQueues.SelectedItems(0).Text
        Dim ti As DevComponents.DotNetBar.TabItem = Me.tabQueues.Tabs.Item(selQ)
        Me.tabQueues.SelectedTab = ti
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

#Region "Skill Toolbar Functions"
    Private Sub btnShowDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' Find out which control it relates to!
        Dim skillName As String = ""
        Dim skillID As Integer
        If activeLVW IsNot Nothing Then
            If activeLVW.Focused = False Then
                Dim curNode As TreeNode
                curNode = tvwSkillList.SelectedNode
                If curNode IsNot Nothing Then
                    skillName = curNode.Text
                End If
            Else
                If activeLVW.SelectedItems.Count > 0 Then
                    skillName = activeLVW.SelectedItems(0).Text
                End If
            End If
        End If
        If skillName <> "" Then
            skillID = Core.SkillFunctions.SkillNameToID(skillName)
            frmSkillDetails.DisplayPilotName = displayPilot.Name
            Call frmSkillDetails.ShowSkillDetails(skillID)
        End If
    End Sub
#End Region

#Region "Skill Tree Context Menu Functions"
    Private Sub ctxDetails_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxDetails.Opening
        Dim curNode As TreeNode
        curNode = tvwSkillList.SelectedNode
        If curNode IsNot Nothing Then
            Dim skillName As String = ""
            Dim skillID As Integer
            skillName = curNode.Text
            skillID = Core.SkillFunctions.SkillNameToID(skillName)
            mnuSkillName2.Text = skillName
            mnuSkillName2.Tag = skillID
            ' Determine if this is a parent node or not
            If curNode.Parent Is Nothing And usingFilter = True Then
                ' Group Node
                mnuAddGroupToQueue.Visible = True
                mnuAddToQueue.Visible = False
            Else
                ' Skill Node
                mnuAddGroupToQueue.Visible = False
                mnuAddToQueue.Visible = True
            End If
            If activeQueueName = "" Then
                mnuAddGroupToQueue.Enabled = False
                mnuAddToQueue.Enabled = False
            Else
                mnuAddGroupToQueue.Enabled = True
                mnuAddToQueue.Enabled = True
                ' Determine enabled menu items of adding to queue
                Dim currentLevel As Integer = 0
                If displayPilot.PilotSkills.ContainsKey(skillName) = True Then
                    Dim cSkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(skillName)
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
    Private Sub mnuViewDetails2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewDetails2.Click
        Dim skillID As Integer = CInt(mnuSkillName2.Tag)
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuAddToQueueNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueueNext.Click
        If activeQueue IsNot Nothing Then
            Call Me.AddSkillToQueueOption()
        End If
    End Sub
    Private Sub mnuAddToQueue1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue1.Click
        If activeQueue IsNot Nothing Then
            If tvwSkillList.SelectedNode IsNot Nothing Then
                activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 1, False, False, "")
                Call Me.RefreshTraining(activeQueueName)
            End If
        End If
    End Sub
    Private Sub mnuAddToQueue2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue2.Click
        If activeQueue IsNot Nothing Then
            If tvwSkillList.SelectedNode IsNot Nothing Then
                activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 2, False, False, "")
                Call Me.RefreshTraining(activeQueueName)
            End If
        End If
    End Sub
    Private Sub mnuAddToQueue3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue3.Click
        If activeQueue IsNot Nothing Then
            If tvwSkillList.SelectedNode IsNot Nothing Then
                activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 3, False, False, "")
                Call Me.RefreshTraining(activeQueueName)
            End If
        End If
    End Sub
    Private Sub mnuAddToQueue4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue4.Click
        If tvwSkillList.SelectedNode IsNot Nothing Then
            If activeQueue IsNot Nothing Then
                activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 4, False, False, "")
                Call Me.RefreshTraining(activeQueueName)
            End If
        End If
    End Sub
    Private Sub mnuAddToQueue5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue5.Click
        If tvwSkillList.SelectedNode IsNot Nothing Then
            If activeQueue IsNot Nothing Then
                activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 5, False, False, "")
                Call Me.RefreshTraining(activeQueueName)
            End If
        End If
    End Sub
    Private Sub mnuAddGroupToQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddGroupLevel1.Click, mnuAddGroupLevel2.Click, mnuAddGroupLevel3.Click, mnuAddGroupLevel4.Click, mnuAddGroupLevel5.Click
        If activeQueue IsNot Nothing Then
            Dim menu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
            Dim level As Integer = CInt(menu.Text.Replace("To Level ", ""))
            Dim parentNode As New TreeNode
            Dim curNode As New TreeNode
            If tvwSkillList.SelectedNode IsNot Nothing Then
                parentNode = tvwSkillList.SelectedNode
                For Each curNode In parentNode.Nodes
                    activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, curNode.Text, activeQueue.Queue.Count + 1, activeQueue, level, True, True, "")
                Next
                Call Me.RefreshTraining(activeQueueName)
            End If
        End If
    End Sub
    Private Sub mnuExpandAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExpandAll.Click
        tvwSkillList.BeginUpdate()
        tvwSkillList.ExpandAll()
        tvwSkillList.EndUpdate()
    End Sub
    Private Sub mnuCollapseAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCollapseAll.Click
        tvwSkillList.BeginUpdate()
        tvwSkillList.CollapseAll()
        tvwSkillList.EndUpdate()
    End Sub
#End Region

#Region "Skill ListView Context Menu Functions"
    Private Sub ctxQueue_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxQueue.Opening
        ' Cancel the menu opening if there are no skills selected
        If activeLVW.SelectedItems.Count = 0 Then
            e.Cancel = True
            Exit Sub
        End If
        ' Determine enabled menu items of adding to queue
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
        Dim ToLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))
        Dim skillName As String = mnuSkillName.Text
        Dim currentLevel As Integer = 0
        If displayPilot.PilotSkills.ContainsKey(skillName) = True Then
            Dim cSkill As Core.EveHQPilotSkill = displayPilot.PilotSkills(skillName)
            currentLevel = cSkill.Level
        End If
        For a As Integer = 1 To 5
            If a <= CInt(fromLevel) Then
                mnuChangeLevel.DropDownItems("mnuChangeLevel" & a).Enabled = False
            Else
                mnuChangeLevel.DropDownItems("mnuChangeLevel" & a).Enabled = True
            End If
        Next
        If currentLevel = 4 Then
            mnuChangeLevel.Enabled = False
        Else
            mnuChangeLevel.Enabled = True
        End If
        If activeQueue.Queue.ContainsKey(keyName) = False Then
            mnuEditNote.Enabled = False
        Else
            mnuEditNote.Enabled = True
        End If
    End Sub
    Private Sub mnuChangeLevel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChangeLevel1.Click
        Call Me.ChangeLevel(1)
    End Sub
    Private Sub mnuChangeLevel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChangeLevel2.Click
        Call Me.ChangeLevel(2)
    End Sub
    Private Sub mnuChangeLevel3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChangeLevel3.Click
        Call Me.ChangeLevel(3)
    End Sub
    Private Sub mnuChangeLevel4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChangeLevel4.Click
        Call Me.ChangeLevel(4)
    End Sub
    Private Sub mnuChangeLevel5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuChangeLevel5.Click
        Call Me.ChangeLevel(5)
    End Sub
    Private Sub mnuIncreaseLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuIncreaseLevel.Click
        Call Me.IncreaseLevel()
    End Sub
    Private Sub mnuDecreaseLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDecreaseLevel.Click
        Call Me.DecreaseLevel()
    End Sub
    Private Sub mnuMoveUpQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMoveUpQueue.Click
        Call Me.MoveUpQueue()
    End Sub
    Private Sub mnuMoveDownQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMoveDownQueue.Click
        Call Me.MoveDownQueue()
    End Sub
    Private Sub mnuSeparateAllLevels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSeparateAllLevels.Click
        For selItem As Integer = 0 To activeLVW.SelectedItems.Count - 1
            Dim keyName As String = activeLVW.SelectedItems(selItem).Name
            Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
            Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
            Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))
            If activeQueue.Queue.ContainsKey(keyName) = True Then
                ' Remove it from the queue
                Dim mySkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
                Dim mySkillPos As Integer = mySkill.Pos - 1
                Call Me.DeleteFromQueue(mySkill)

                ' Add all the sublevels
                For level As Integer = CInt(fromLevel) To CInt(toLevel) - 1
                    mySkillPos += 1
                    activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos, activeQueue, level + 1, False, False, "")
                Next
            End If
        Next selItem
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuSeparateTopLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSeparateTopLevel.Click
        For selItem As Integer = 0 To activeLVW.SelectedItems.Count - 1
            Dim keyName As String = activeLVW.SelectedItems(selItem).Name
            Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
            Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
            Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))

            ' Remove it from the queue
            Dim mySkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
            Dim mySkillPos As Integer = mySkill.Pos
            Call Me.DeleteFromQueue(mySkill)

            ' Add the new levels, 
            activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos, activeQueue, CInt(toLevel) - 1, False, False, "")
            activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos + 1, activeQueue, CInt(toLevel), False, False, "")
        Next selItem

        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuSeparateBottomLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSeparateBottomLevel.Click
        For selItem As Integer = 0 To activeLVW.SelectedItems.Count - 1
            Dim keyName As String = activeLVW.SelectedItems(selItem).Name
            Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
            Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
            Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))

            ' Remove it from the queue
            Dim mySkill As Core.EveHQSkillQueueItem = activeQueue.Queue(keyName)
            Dim mySkillPos As Integer = mySkill.Pos
            Call Me.DeleteFromQueue(mySkill)

            ' Add the new levels, 
            activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos, activeQueue, CInt(fromLevel) + 1, False, False, "")
            activeQueue = Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos + 1, activeQueue, CInt(toLevel), False, False, "")
        Next selItem
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuDeleteFromQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteFromQueue.Click
        Call Me.DeleteFromQueueOption()
    End Sub
    Private Sub mnuRemoveTrainedSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveTrainedSkills.Click
        Dim reply As Integer
        reply = MessageBox.Show("Are you sure you want to remove trained skills from the queue?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            Call Core.SkillQueueFunctions.RemoveTrainedSkills(displayPilot, activeQueue)
            ' Refresh the training view!
            Call Me.RefreshTraining(activeQueueName)
        End If
    End Sub
    Private Sub mnuClearTrainingQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClearTrainingQueue.Click
        Call Me.ClearTrainingQueue()
    End Sub
    Private Sub mnuViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewDetails.Click
        Dim skillID As Integer = CInt(mnuSkillName.Tag)
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuSplitQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSplitQueue.Click
        Call Me.SplitQueue()
    End Sub
#End Region

#Region "Skill Info Context Menu Functions"
    Private Sub ctxReqs_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxReqs.Opening
        Dim curNode As TreeNode = tvwReqs.SelectedNode
        If curNode IsNot Nothing Then
            Dim skillName As String = ""
            Dim skillID As Integer
            skillName = curNode.Text
            If InStr(skillName, "(Level") <> 0 Then
                skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
            End If
            skillID = Core.SkillFunctions.SkillNameToID(skillName)
            mnuReqsSkillName.Text = skillName
            mnuReqsSkillName.Tag = skillID
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuViewItemDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetails.Click
        Dim skillID As Integer = CInt(mnuItemName.Tag)
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuViewItemDetailsHere_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetailsHere.Click
        Dim skillID As Integer = CInt(mnuItemName.Tag)
        Call Me.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuReqsViewDetailsHere_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReqsViewDetailsHere.Click
        Dim skillID As Integer = CInt(mnuReqsSkillName.Tag)
        Call Me.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuReqsViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReqsViewDetails.Click
        Dim skillID As Integer = CInt(mnuReqsSkillName.Tag)
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuViewItemDetailsInCertScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetailsInCertScreen.Click
        Dim certID As Integer = CInt(mnuItemName.Tag)
        frmCertificateDetails.DisplayPilotName = displayPilot.Name
        frmCertificateDetails.ShowCertDetails(certID)
    End Sub
#End Region

#Region "Skill Queue Import/Export"

    Private Sub ImportAllEveMonPlans()
        ' Set recalc flag
        Dim RecalcQueues As Boolean = False
        ' Try to find the EveMon settings file
        Dim EveMonLocation As String = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveMon"), "Settings.xml")
        If My.Computer.FileSystem.FileExists(EveMonLocation) = False Then
            ' Try for the settings-debug file which is common during development
            EveMonLocation = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveMon"), "Settings-debug.xml")
            If My.Computer.FileSystem.FileExists(EveMonLocation) = False Then
                MessageBox.Show("EveMon Settings File Not Found." & ControlChars.CrLf & ControlChars.CrLf & "Please check the EveMon installation.", "EveMon Settings Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        End If
        Try
            ' Load the Settings file into an XMLDocument
            Dim EMXML As New Xml.XmlDocument
            EMXML.Load(EveMonLocation)
            ' Get a list of the characters that are in files (not API)
            Dim EMPilots As New SortedList
            Dim CharDetails As Xml.XmlNodeList
            Dim CharNode As Xml.XmlNode
            ' Get details of characters assigned to an API account
            CharDetails = EMXML.SelectNodes("Settings/characters/ccp")
            If CharDetails.Count > 0 Then
                ' Need to add details of pilots here
                For Each CharNode In CharDetails
                    EMPilots.Add(CharNode.Attributes.GetNamedItem("guid").Value, CharNode.ChildNodes(1).InnerText) ' Adds the GUID and name
                Next
            End If
            ' Get details of characters added by a file
            CharDetails = EMXML.SelectNodes("Settings/characters/uri")
            If CharDetails.Count > 0 Then
                ' Need to add details of pilots here
                For Each CharNode In CharDetails
                    EMPilots.Add(CharNode.Attributes.GetNamedItem("guid").Value, CharNode.ChildNodes(1).InnerText) ' Adds the GUID and name
                Next
            End If

            ' Try and get the plan information
            Dim PlanDetails As Xml.XmlNodeList
            Dim PlanNode As Xml.XmlNode
            Dim PlansItemNode As Xml.XmlNode
            Dim PlansParentNode As Xml.XmlNode
            PlanDetails = EMXML.SelectNodes("Settings/plans/plan")
            If PlanDetails.Count > 0 Then
                Dim PlanInfo(PlanDetails.Count, 2) As String
                Dim count As Integer = -1
                For Each PlanNode In PlanDetails
                    count += 1
                    Dim planOwner As String = PlanNode.Attributes.GetNamedItem("owner").Value
                    If EMPilots.ContainsKey(planOwner) = True Then
                        Dim pilotName As String = CStr(EMPilots(planOwner))
                        If Core.HQ.Settings.Pilots.ContainsKey(pilotName) = True Then
                            Dim planName As String = PlanNode.Attributes.GetNamedItem("name").Value
                            PlanInfo(count, 0) = pilotName : PlanInfo(count, 1) = planName
                            PlansParentNode = PlanNode.ChildNodes(1)
                            Dim SQ As New Dictionary(Of String, Core.EveHQSkillQueueItem)
                            Dim SQCount As Integer = 0
                            Dim planItems As XmlNodeList = PlanNode.SelectNodes("entry")
                            If planItems.Count > 0 Then
                                For Each PlansItemNode In planItems
                                    SQCount += 1
                                    Dim SQI As New Core.EveHQSkillQueueItem
                                    SQI.Name = PlansItemNode.Attributes.GetNamedItem("skill").Value
                                    SQI.ToLevel = CInt(PlansItemNode.Attributes.GetNamedItem("level").Value)
                                    SQI.FromLevel = SQI.ToLevel - 1
                                    SQI.Pos = SQCount
                                    SQI.Notes = PlansItemNode.ChildNodes(0).InnerText
                                    SQ.Add(SQI.Key, SQI)
                                Next
                                PlanInfo(count, 2) = SQ.Count.ToString

                                ' Ok, load up the plan
                                Dim newSQ As New Core.EveHQSkillQueue
                                newSQ.Name = PlanInfo(count, 1)
                                newSQ.IncCurrentTraining = True
                                newSQ.Primary = False
                                newSQ.Queue = SQ
                                Dim QPilot As Core.EveHQPilot = Core.HQ.Settings.Pilots(PlanInfo(count, 0))
                                If QPilot.TrainingQueues.ContainsKey(PlanInfo(count, 1)) = False Then
                                    QPilot.TrainingQueues.Add(newSQ.Name, newSQ)
                                    RecalcQueues = True
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            ' Recalc the queues if appropriate
            If RecalcQueues = True Then
                Call Me.RefreshAllTraining()
            End If
            MessageBox.Show("Import of " & PlanDetails.Count & " EveMon plans complete!", "EveMon Skill Queue Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                If My.Computer.FileSystem.FileExists(.FileName) = False Then
                    MessageBox.Show("Specified file does not exist. Please try again.", "Error Finding File", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else
                    ' Open the file for reading
                    Dim planXML As New XmlDocument
                    Try
                        ' UnGZip the file
                        Dim fs As FileStream = New FileStream(.FileName, FileMode.Open, FileAccess.Read)
                        Dim compstream As New System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Decompress)
                        Dim sr As New StreamReader(compstream)
                        Dim strEMP As String = sr.ReadToEnd()
                        sr.Close()
                        fs.Close()
                        planXML.LoadXml(strEMP)
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
                    Dim selectQueue As New Core.FrmSelectQueue(displayPilot.Name, planSkills, "Import from EveMon")
                    selectQueue.ShowDialog()
                    Call Me.RefreshAllTraining()
                End If
            End If
        End With
    End Sub

    Private Sub ExportEveMonPlan()
        If activeQueue IsNot Nothing Then
            Dim arrQueue As ArrayList = Core.SkillQueueFunctions.BuildQueue(displayPilot, activeQueue, False, True)
            Dim qItem As New Core.SortedQueueItem
            If arrQueue IsNot Nothing Then
                Dim EMPAtt As XmlAttribute
                ' Create XML Document
                Dim EMPXML As New XmlDocument
                ' Create XML Declaration
                Dim dec As XmlDeclaration = EMPXML.CreateXmlDeclaration("1.0", Nothing, Nothing)
                EMPXML.AppendChild(dec)
                ' Create plan root
                Dim EMPRoot As XmlElement = EMPXML.CreateElement("plan")
                EMPXML.AppendChild(EMPRoot)

                EMPAtt = EMPXML.CreateAttribute("name")
                EMPAtt.Value = activeQueue.Name
                EMPRoot.Attributes.Append(EMPAtt)

                'EMPAtt = EMPXML.CreateAttribute("owner")
                'EMPAtt.Value = displayPilot.Name
                'EMPRoot.Attributes.Append(EMPAtt)

                EMPAtt = EMPXML.CreateAttribute("revision")
                EMPAtt.Value = "1968"
                EMPRoot.Attributes.Append(EMPAtt)

                Dim EMPSort As XmlElement = EMPXML.CreateElement("sorting")
                EMPRoot.AppendChild(EMPSort)

                EMPAtt = EMPXML.CreateAttribute("criteria")
                EMPAtt.Value = "None"
                EMPSort.Attributes.Append(EMPAtt)

                EMPAtt = EMPXML.CreateAttribute("order")
                EMPAtt.Value = "None"
                EMPSort.Attributes.Append(EMPAtt)

                EMPAtt = EMPXML.CreateAttribute("optimizeLearning")
                EMPAtt.Value = "false"
                EMPSort.Attributes.Append(EMPAtt)

                EMPAtt = EMPXML.CreateAttribute("groupByPriority")
                EMPAtt.Value = "false"
                EMPSort.Attributes.Append(EMPAtt)

                ' Create individual entries
                Dim EMPElement As XmlElement

                For Each qItem In arrQueue
                    Dim EMPEntry As XmlNode = EMPXML.CreateElement("entry")

                    EMPAtt = EMPXML.CreateAttribute("skillID")
                    EMPAtt.Value = CStr(qItem.ID)
                    EMPEntry.Attributes.Append(EMPAtt)

                    EMPAtt = EMPXML.CreateAttribute("skill")
                    EMPAtt.Value = qItem.Name
                    EMPEntry.Attributes.Append(EMPAtt)

                    EMPAtt = EMPXML.CreateAttribute("level")
                    EMPAtt.Value = qItem.ToLevel.ToString
                    EMPEntry.Attributes.Append(EMPAtt)

                    EMPAtt = EMPXML.CreateAttribute("priority")
                    EMPAtt.Value = "3"
                    EMPEntry.Attributes.Append(EMPAtt)

                    EMPAtt = EMPXML.CreateAttribute("type")
                    If qItem.IsPrereq Then
                        EMPAtt.Value = "Prerequisite"
                    Else
                        EMPAtt.Value = "Planned"
                    End If
                    EMPEntry.Attributes.Append(EMPAtt)

                    EMPElement = EMPXML.CreateElement("notes")
                    EMPElement.InnerText = qItem.Notes
                    EMPEntry.AppendChild(EMPElement)

                    EMPRoot.AppendChild(EMPEntry)
                Next

                ' Form a string of the XML
                Dim strXML As String = EMPXML.InnerXml

                ' Get a file name
                Dim sfd1 As New SaveFileDialog
                With sfd1
                    .Title = "Save as EveMon Plan File..."
                    .FileName = ""
                    .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                    .Filter = "EveMon Plan Files (*.emp)|*.emp|All Files (*.*)|*.*"
                    .FilterIndex = 1
                    .RestoreDirectory = True
                    If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                        If .FileName <> "" Then
                            ' Output the file as GZip
                            Dim buffer() As Byte
                            Dim enc As New System.Text.ASCIIEncoding
                            buffer = enc.GetBytes(strXML)
                            Dim outfile As System.IO.FileStream = File.Create(.FileName)
                            Dim gzipStream As New Compression.GZipStream(outfile, Compression.CompressionMode.Compress)
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

    Private Sub mnuEditNote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEditNote.Click
        ' Try to get the keys of the skill(s) we are changing
        If activeLVW.SelectedItems.Count > 0 Then
            Dim keys As New List(Of String)
            For Each selItem As ListViewItem In activeLVW.SelectedItems
                keys.Add(selItem.Name)
            Next
            Dim NoteForm As New frmSkillNote
            If keys.Count > 1 Then
                NoteForm.lblDescription.Text = "Editing description for multiple queue entries..."
                NoteForm.Text = "Skill Note - Multiple Skills"
            Else
                Dim curToLevel As String = CStr(keys(0)).Substring(CStr(keys(0)).Length - 1, 1)
                Dim curFromLevel As String = CStr(keys(0)).Substring(CStr(keys(0)).Length - 2, 1)
                Dim curSkillName As String = CStr(keys(0)).Substring(0, CStr(keys(0)).Length - 2)
                NoteForm.lblDescription.Text = "Editing description for " & curSkillName & " (Lvl " & curToLevel & ")"
                NoteForm.Text = "Skill Note - " & curSkillName & " (Lvl " & curToLevel & ")"
            End If
            NoteForm.txtNotes.Text = activeQueue.Queue(keys(0)).Notes
            NoteForm.txtNotes.SelectAll()
            NoteForm.ShowDialog()
            If NoteForm.DialogResult = Windows.Forms.DialogResult.OK Then
                For Each key As String In keys
                    activeQueue.Queue(key).Notes = NoteForm.txtNotes.Text
                Next
                Call Me.RefreshTraining(activeQueueName)
            End If
        End If
    End Sub

#Region "Ribbon Button Functions"
    Private Sub btnEveMonImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEveMonImport.Click
        Call Me.ImportAllEveMonPlans()
    End Sub

    Private Sub btnImportEMPFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEMPFile.Click
        Call Me.ImportEveMonPlan()
    End Sub

    Private Sub btnExportEMPFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportEMPFile.Click
        Call Me.ExportEveMonPlan()
    End Sub

    Private Sub btnImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImplants.Click
        If frmImplants.IsHandleCreated = True Then
            frmImplants.Select()
        Else
            frmImplants.PilotName = displayPilot.Name
            frmImplants.QueueName = activeQueueName
            frmImplants.Show()
        End If
    End Sub

    Private Sub btnRemap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemap.Click
        If frmNeuralRemap.IsHandleCreated = True Then
            frmNeuralRemap.Select()
        Else
            frmNeuralRemap.PilotName = displayPilot.Name
            frmNeuralRemap.QueueName = activeQueueName
            frmNeuralRemap.Show()
        End If
    End Sub

    Private Sub btnRBAddQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBAddQueue.Click, mnuAddQueue.Click
        ' Clear the text boxes
        Dim myQueue As frmModifyQueues = New frmModifyQueues
        With myQueue
            .txtQueueName.Text = "" : .txtQueueName.Enabled = True
            .btnAccept.Text = "Add" : .Tag = "Add"
            .Text = "Add New Queue"
            .DisplayPilotName = displayPilot.Name
            .ShowDialog()           ' New Queue checking and adding is done on the modal form!
        End With
        frmEveHQ.BuildQueueReportsMenu()
        Call Me.RefreshAllTraining()
    End Sub

    Private Sub btnRBEditQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBEditQueue.Click, mnuEditQueue.Click
        ' Check for some selection on the listview
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to edit!", "Cannot Edit Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            Dim myQueue As frmModifyQueues = New frmModifyQueues
            With myQueue
                ' Load the account details into the text boxes
                Dim selQueue As Core.EveHQSkillQueue = displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name)
                .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Name
                .btnAccept.Text = "Edit" : .Tag = "Edit"
                .Text = "Edit '" & selQueue.Name & "' Queue Details"
                .DisplayPilotName = displayPilot.Name
                .ShowDialog()
            End With
            frmEveHQ.BuildQueueReportsMenu()
            Call Me.RefreshAllTraining()
        End If
    End Sub

    Private Sub btnRBDeleteQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBDeleteQueue.Click, mnuDeleteQueue.Click
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
            If confirm = Windows.Forms.DialogResult.Yes Then
                ' Delete the queue the accounts collection
                displayPilot.TrainingQueues.Remove(selQ)
                If displayPilot.PrimaryQueue = selQ Then
                    displayPilot.PrimaryQueue = ""
                End If
                If displayPilot.TrainingQueues.Count = 0 Then
                    displayPilot.PrimaryQueue = ""
                    displayPilot.ActiveQueueName = ""
                    displayPilot.ActiveQueue = Nothing
                End If
                ' Remove the item from the list
                frmEveHQ.BuildQueueReportsMenu()
                Call Me.RefreshAllTraining()
            Else
                lvQueues.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub btnRBSetPrimaryQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBSetPrimaryQueue.Click, mnuSetPrimary.Click
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to call Primary!", "Cannot Set Primary Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            ' Remove the current primary queue (if exists!)
            Dim oldPQ As Core.EveHQSkillQueue = displayPilot.TrainingQueues(displayPilot.PrimaryQueue)
            If oldPQ IsNot Nothing Then
                oldPQ.Primary = False
            End If
            displayPilot.PrimaryQueue = ""
            ' Select the new primary queue
            Dim selQueue As Core.EveHQSkillQueue = displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name)
            selQueue.Primary = True
            displayPilot.PrimaryQueue = selQueue.Name
            Call Me.DrawQueueSummary()
        End If
    End Sub

    Private Sub btnRBMergeQueues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBMergeQueues.Click, mnuMergeQueues.Click
        If lvQueues.SelectedItems.Count < 2 Then
            MessageBox.Show("Please select 2 or more Queues to merge!", "Cannot Merge Queues", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            Dim myQueue As frmModifyQueues = New frmModifyQueues
            With myQueue
                .txtQueueName.Text = "" : .txtQueueName.Tag = lvQueues.SelectedItems
                .btnAccept.Text = "Merge" : .Tag = "Merge"
                .Text = "Merge Skill Queues"
                .DisplayPilotName = displayPilot.Name
                .ShowDialog()           ' Queue checking and merging is done on the modal form!
            End With
            Call Me.RefreshAllTraining()
        End If
    End Sub

    Private Sub btnRBCopyQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBCopyQueue.Click, mnuCopyQueue.Click
        ' Check for some selection on the listview
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to copy!", "Cannot Copy Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            Dim myQueue As frmModifyQueues = New frmModifyQueues
            With myQueue
                ' Load the account details into the text boxes
                Dim selQueue As Core.EveHQSkillQueue = displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name)
                .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Name
                .btnAccept.Text = "Copy" : .Tag = "Copy"
                .Text = "Copy '" & selQueue.Name & "' Queue Details"
                .DisplayPilotName = displayPilot.Name
                .ShowDialog()
            End With
            Call Me.RefreshAllTraining()
        End If
    End Sub

    Private Sub btnRBCopyQueueToPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBCopyQueueToPilot.Click, mnuCopyQueueToPilot.Click
        ' Check for some selection on the listview
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to copy!", "Cannot Copy Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            Dim myQueue As frmSelectQueuePilot = New frmSelectQueuePilot
            With myQueue
                ' Load the account details into the text boxes
                Dim selQueue As Core.EveHQSkillQueue = displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name)
                .DisplayPilotName = displayPilot.Name
                .cboPilots.Tag = selQueue.Name
                .ShowDialog()
            End With
        End If
    End Sub

    Private Sub btnIncTraining_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIncTraining.Click
        If redrawingOptions = False Then
            If activeQueue IsNot Nothing Then
                activeLVW.IncludeCurrentTraining = btnIncTraining.Checked
                activeQueue.IncCurrentTraining = btnIncTraining.Checked
                If activeQueue.Name IsNot Nothing Then
                    RefreshTraining(activeQueue.Name)
                End If
            End If
        End If
    End Sub

    Private Sub btnRBAddSkill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBAddSkill.Click
        Call Me.AddSkillToQueueOption()
    End Sub

    Private Sub btnRBDeleteSkill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBDeleteSkill.Click
        Call Me.DeleteFromQueueOption()
    End Sub

    Private Sub btnRBIncreaseLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBIncreaseLevel.Click
        Call Me.IncreaseLevel()
    End Sub

    Private Sub btnRBDecreaseLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBDecreaseLevel.Click
        Call Me.DecreaseLevel()
    End Sub

    Private Sub btnRBMoveUpQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBMoveUpQueue.Click
        Call Me.MoveUpQueue()
    End Sub

    Private Sub btnRBMoveDownQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBMoveDownQueue.Click
        Call Me.MoveDownQueue()
    End Sub

    Private Sub btnRBClearQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBClearQueue.Click
        Call Me.ClearTrainingQueue()
    End Sub

    Private Sub btnQueueSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueueSettings.Click
        Dim EveHQSettings As New frmSettings
        EveHQSettings.Tag = "nodeTrainingQueue"
        EveHQSettings.ShowDialog()
        EveHQSettings.Dispose()
    End Sub

    Private Sub btnRBSplitQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRBSplitQueue.Click
        Call Me.SplitQueue()
    End Sub

#End Region

End Class