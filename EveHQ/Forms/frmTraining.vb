Imports System.Text

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
'=========================================================================#
Imports System.IO

Public Class frmTraining

    Dim oldNodeIndex As Integer = -1
    Dim oNodes As New ArrayList
    Dim omitQueuedSkills As Boolean = False
    Dim selQTime As Double = 0
    Dim totalQTime As Double = 0
    Dim activeQueueName As String = ""
    Dim activeQueue As New EveHQ.Core.SkillQueue
    Dim activeTime As Label
    Dim activeLVW As EveHQ.DragAndDropListView
    Dim startTime As DateTime
    Dim endTime As DateTime
    Dim TrainingThreshold As Integer = 1600000
    Dim TrainingBonus As Double = 1
    Dim suggestedQueues As New SortedList
    Dim usingFilter As Boolean = True
    Dim skillListNodes As New SortedList
    Dim certListNodes As New SortedList
    Dim CertGrades() As String = New String() {"", "Basic", "Standard", "Improved", "Advanced", "Elite"}
    Dim displayPilot As New EveHQ.Core.Pilot
    Dim cDisplayPilotName As String = ""
    Dim startup As Boolean = False

    Delegate Sub UpdateSuggestionUIDelegate(ByVal ActQueueName As String)
    Private SetSuggUIToCalc As UpdateSuggestionUIDelegate = New UpdateSuggestionUIDelegate(AddressOf SetSuggestionUIToCalc)
    Delegate Sub FinaliseSuggestionUIDelegate(ByVal ActQueueName As String, ByVal ActQueueTime As Double, ByVal sugQueueTime As Double)
    Private SetSuggUIResult As FinaliseSuggestionUIDelegate = New FinaliseSuggestionUIDelegate(AddressOf SetSuggestionUIResult)

    Public Property DisplayPilotName() As String
        Get
            Return displayPilot.Name
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            If cboPilots.Items.Contains(value) Then
                cboPilots.SelectedItem = value
            End If
        End Set
    End Property

#Region "Form Loading and Setup Routines"
    Private Sub frmTraining_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the startup flag
        startup = True

        ' Load the pilots
        Call UpdatePilots()

        ' Set up the queue information
        cboFilter.SelectedIndex = 0
        cboCertFilter.SelectedIndex = 0
        Call Me.SetupQueues()
        Call Me.RefreshAllTrainingQueues()
        tsQueueOptions.Enabled = False
        mnuAddToQueue.Enabled = False
        AddHandler EveHQ.Core.SkillQueueFunctions.RefreshQueue, AddressOf Me.RefreshAllTraining
        ' Set the first initial primary one as active

        ' Disable the startup flag
        startup = False

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
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next
        cboPilots.EndUpdate()

        ' Select a pilot
        If cDisplayPilotName <> "" Then
            If cboPilots.Items.Contains(cDisplayPilotName) = True Then
                cboPilots.SelectedItem = cDisplayPilotName
            Else
                cboPilots.SelectedIndex = 0
            End If
        Else
            If oldPilot = "" Then
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = True Then
                        cboPilots.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
                    Else
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            Else
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(oldPilot) = True Then
                        If Not (CStr(cboPilots.SelectedItem) = oldPilot) Then
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
            If tabQueues.TabPages.ContainsKey(oldQueue) = True Then
                tabQueues.SelectedTab = tabQueues.TabPages(oldQueue)
            End If
        End If

    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilots.SelectedItem.ToString) = True Then
            displayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            ' Only update if we are not starting up
            If startup = False Then
                tabQueues.SuspendLayout()
                For i As Integer = tabQueues.TabPages.Count To 2 Step -1
                    Dim tp As TabPage = tabQueues.TabPages(1)
                    tabQueues.TabPages.RemoveAt(1)
                    tp.Dispose()
                Next
                tabQueues.ResumeLayout()
                Call Me.RefreshAllTraining()
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

        Me.tabQueues.SuspendLayout()
        ' Remove all but the summary tab on the tabQueues
        For Each tp As TabPage In Me.tabQueues.TabPages
            If tp.Name <> "tabSummary" And Not displayPilot.TrainingQueues.ContainsKey(tp.Name) Then
                Me.tabQueues.TabPages.Remove(tp)
                tp.Dispose()
            End If
        Next

        ' Delete the Shared SuggestionStatus
        suggestedQueues.Clear()

        Dim i As Integer = 1
        For Each newQ As EveHQ.Core.SkillQueue In displayPilot.TrainingQueues.Values
            Dim newQTab As TabPage
            If Not tabQueues.TabPages.ContainsKey(newQ.Name) Then
                newQTab = New TabPage
                newQTab.Name = newQ.Name
                newQTab.Text = newQ.Name

                Dim tq As TrainingQueue = New TrainingQueue()
                tq.Dock = DockStyle.Fill
                tq.Name = "TQ" & newQ.Name
                tq.lvQueue.IncludeCurrentTraining = newQ.IncCurrentTraining
                tq.lvQueue.ContextMenuStrip = Me.ctxQueue

                newQTab.Controls.Add(tq)

                AddHandler tq.lvQueue.Click, AddressOf activeLVW_Click
                AddHandler tq.lvQueue.DoubleClick, AddressOf activeLVW_DoubleClick
                AddHandler tq.lvQueue.DragDrop, AddressOf activeLVW_DragDrop
                AddHandler tq.lvQueue.DragEnter, AddressOf activeLVW_DragEnter
                AddHandler tq.lvQueue.ItemDrag, AddressOf activeLVW_ItemDrag
                AddHandler tq.lvQueue.ColumnClick, AddressOf activeLVW_ColumnClick
                AddHandler tq.lvQueue.SelectedIndexChanged, AddressOf activeLVW_SelectedIndexChanged

                Call Me.DrawColumns(tq.lvQueue)

                AddHandler tq.newSuggestionPB.Click, AddressOf Me.SuggestionIconClick
                tabQueues.TabPages.Insert(i, newQTab)
            End If

            i = i + 1
        Next

        tabQueues_SelectedIndexChanged(Me, EventArgs.Empty)

        Me.tabQueues.ResumeLayout()
    End Sub
    Private Sub DrawColumns(ByVal lv As EveHQ.DragAndDropListView)

        lv.Columns.Clear()
        ' Add the 6 standard columns
        lv.Columns.Add("Name", "Skill Name", 180, HorizontalAlignment.Left, "")
        lv.Columns.Add("Curl", "Cur Lvl", 50, HorizontalAlignment.Center, "")
        lv.Columns.Add("From", "From Lvl", 55, HorizontalAlignment.Center, "")
        lv.Columns.Add("Tole", "To Lvl", 55, HorizontalAlignment.Center, "")
        lv.Columns.Add("Perc", "%", 30, HorizontalAlignment.Center, "")
        lv.Columns.Add("Trai", "Training Time", 100, HorizontalAlignment.Left, "")
        ' Now find the other ones
        For a As Integer = 6 To 16
            If EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0) IsNot Nothing Then
                If CBool(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 1)) = True Then
                    Select Case EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0)
                        Case "Date"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "Date Completed", 150, HorizontalAlignment.Left, "")
                        Case "Rank"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "Rank", 60, HorizontalAlignment.Center, "")
                        Case "PAtt"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "Pri Attr", 60, HorizontalAlignment.Center, "")
                        Case "SAtt"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "Sec Attr", 60, HorizontalAlignment.Center, "")
                        Case "SPRH"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "SP /hour", 60, HorizontalAlignment.Center, "")
                        Case "SPRD"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "SP /day", 60, HorizontalAlignment.Center, "")
                        Case "SPRW"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "SP /week", 60, HorizontalAlignment.Center, "")
                        Case "SPRM"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "SP /mnth", 60, HorizontalAlignment.Center, "")
                        Case "SPRY"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "SP /year", 60, HorizontalAlignment.Center, "")
                        Case "SPAd"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "SP Added", 100, HorizontalAlignment.Center, "")
                        Case "SPTo"
                            lv.Columns.Add(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0), "SP @ End", 100, HorizontalAlignment.Center, "")
                    End Select
                End If
            Else
                ' Reset the columns and attempt a redraw
                Call EveHQ.Core.EveHQSettingsFunctions.ResetColumns()
                Call Me.DrawColumns(lv)
                Exit For
            End If
        Next
    End Sub
    Public Sub RefreshAllTrainingQueues()
        For Each newQ As EveHQ.Core.SkillQueue In displayPilot.TrainingQueues.Values
            Call Me.RefreshTraining(newQ.Name)
        Next
        Call Me.DrawQueueSummary()
    End Sub
    Private Sub DrawQueueSummary()
        ' Check if we have a Primary Queue
        If displayPilot.TrainingQueues.Count > 0 Then
            If displayPilot.PrimaryQueue = "" Then
                displayPilot.PrimaryQueue = displayPilot.TrainingQueues.GetKey(0).ToString
                Dim selQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(displayPilot.PrimaryQueue), EveHQ.Core.SkillQueue)
                selQueue.Primary = True
            End If
        End If
        ' Setup the summary column
        lvQueues.Items.Clear()
        Dim totalQTime As Double = 0
        For Each newQ As EveHQ.Core.SkillQueue In displayPilot.TrainingQueues.Values
            Dim newItem As New ListViewItem
            Dim PrimaryFont As Font = New Font(newItem.Font, FontStyle.Bold)
            newItem.Name = newQ.Name
            newItem.Text = newQ.Name
            If newQ.Primary = True Then
                newItem.Font = PrimaryFont
            End If
            newItem.SubItems.Add(newQ.Queue.Count.ToString)
            Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(newQ.Name).Controls("TQ" & newQ.Name), TrainingQueue)
            Dim tTime As Double = CDbl(tq.lblQueueTime.Tag)
            Dim tTimeItem As New ListViewItem.ListViewSubItem
            tTimeItem.Tag = tTime
            tTimeItem.Text = EveHQ.Core.SkillFunctions.TimeToString(tTime)
            newItem.SubItems.Add(tTimeItem)
            Dim qTime As Double = 0
            If displayPilot.Training = True And newQ.IncCurrentTraining = True Then
                qTime = tTime - displayPilot.TrainingCurrentTime
            Else
                qTime = tTime
            End If
            Dim qTimeItem As New ListViewItem.ListViewSubItem
            qTimeItem.Tag = tTime
            qTimeItem.Text = EveHQ.Core.SkillFunctions.TimeToString(qTime)
            newItem.SubItems.Add(qTimeItem)
            totalQTime += qTime
            Dim eTime As Date = Now.AddSeconds(tTime)
            newItem.SubItems.Add(Format(eTime, "ddd") & " " & FormatDateTime(eTime, DateFormat.GeneralDate))
            lvQueues.Items.Add(newItem)
        Next
    End Sub
    Private Sub tabQueues_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabQueues.SelectedIndexChanged
        If tabQueues.SelectedTab.Name <> "tabSummary" Then
            activeQueueName = tabQueues.SelectedTab.Name
            displayPilot.ActiveQueueName = activeQueueName
            Dim aq As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(activeQueueName), Core.SkillQueue)
            activeQueue = aq
            displayPilot.ActiveQueue = activeQueue
            Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(activeQueueName).Controls("TQ" & activeQueueName), TrainingQueue)
            activeTime = tq.lblQueueTime
            activeLVW = tq.lvQueue
            activeLVW.IncludeCurrentTraining = aq.IncCurrentTraining
            Call RedrawOptions()
            tsQueueOptions.Enabled = True
            activeLVW.Select()
            mnuAddToQueue.Enabled = True
        Else
            activeQueueName = ""
            activeQueue = Nothing
            displayPilot.ActiveQueueName = activeQueueName
            tsQueueOptions.Enabled = False
            mnuAddToQueue.Enabled = False
        End If
        If frmNeuralRemap.IsHandleCreated = True Then
            frmNeuralRemap.QueueName = activeQueueName
        End If
        If frmImplants.IsHandleCreated = True Then
            frmImplants.QueueName = activeQueueName
        End If
    End Sub
#End Region

#Region "Training Refresh Routines"
    Public Sub RefreshAllTraining()
        If Me.IsHandleCreated = True Then
            suggestedQueues.Clear()
            Call Me.SetupQueues()
            Call Me.RefreshAllTrainingQueues()
            Call Me.ResetQueueButtons()
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
        If tvwSkillList.SelectedNode Is Nothing Then
            btnShowDetails.Enabled = False
        End If
    End Sub
    Private Sub LoadSkillGroups()
        skillListNodes.Clear()
        Dim newSkillGroup As EveHQ.Core.SkillGroup
        For Each newSkillGroup In EveHQ.Core.HQ.SkillGroups.Values
            If newSkillGroup.ID <> "505" Then
                Dim groupNode As TreeNode = New TreeNode
                groupNode.Name = newSkillGroup.ID
                groupNode.Text = newSkillGroup.Name.Trim
                groupNode.ImageIndex = 8
                groupNode.SelectedImageIndex = 8
                skillListNodes.Add(groupNode.Name, groupNode)
            End If
        Next
    End Sub
    Private Sub LoadFilteredSkills(ByVal filter As Integer)
        Dim newSkill As EveHQ.Core.EveSkill
        Dim groupNode As New TreeNode
        For Each newSkill In EveHQ.Core.HQ.SkillListID.Values
            Dim gID As String = newSkill.GroupID
            groupNode = CType(skillListNodes.Item(gID), TreeNode)
            If gID <> "505" Then
                Dim skillNode As TreeNode = New TreeNode
                skillNode.Text = newSkill.Name
                skillNode.Name = newSkill.ID
                If displayPilot.PilotSkills.Contains(newSkill.Name) = True Then
                    Dim mySkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
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
                            If displayPilot.PilotSkills.Contains(newSkill.Name) = True Then
                                addSkill = True
                            End If
                        Case 4
                            If newSkill.Published = True And displayPilot.PilotSkills.Contains(newSkill.Name) = False Then
                                addSkill = True
                            End If
                        Case 5
                            Dim trainable As Boolean = False
                            If displayPilot.PilotSkills.Contains(newSkill.Name) = False And newSkill.Published = True Then
                                trainable = True
                                For Each preReq As String In newSkill.PreReqSkills.Keys
                                    If newSkill.PreReqSkills(preReq) <> 0 Then
                                        Dim ps As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(preReq)
                                        If displayPilot.PilotSkills.Contains(ps.Name) = True Then
                                            Dim psp As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(ps.Name), EveHQ.Core.PilotSkill)
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
                            If displayPilot.PilotSkills.Contains(newSkill.Name) = True Then
                                Dim mySkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                                Dim partTrained As Boolean = True
                                For level As Integer = 0 To 5
                                    If mySkill.SP = newSkill.LevelUp(level) Or mySkill.SP = newSkill.LevelUp(level) + 1 Then
                                        partTrained = False
                                        Exit For
                                    End If
                                Next
                                If (partTrained = True And filter = 7) Or (partTrained = False And filter = 6) Then
                                    addSkill = True
                                End If
                            End If
                        Case 8 To 12
                            Dim requiredLevel As Integer = filter - 8
                            If displayPilot.PilotSkills.Contains(newSkill.Name) = True Then
                                Dim mySkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                                If requiredLevel = mySkill.Level Then
                                    addSkill = True
                                End If
                            End If
                        Case 13 To 28
                            If newSkill.Published = True And newSkill.Rank = (filter - 12) Then
                                addSkill = True
                            End If
                        Case 29
                            If newSkill.Published = True And newSkill.PA = "Charisma" Then
                                addSkill = True
                            End If
                        Case 30
                            If newSkill.Published = True And newSkill.PA = "Intelligence" Then
                                addSkill = True
                            End If
                        Case 31
                            If newSkill.Published = True And newSkill.PA = "Memory" Then
                                addSkill = True
                            End If
                        Case 32
                            If newSkill.Published = True And newSkill.PA = "Perception" Then
                                addSkill = True
                            End If
                        Case 33
                            If newSkill.Published = True And newSkill.PA = "Willpower" Then
                                addSkill = True
                            End If
                        Case 34
                            If newSkill.Published = True And newSkill.SA = "Charisma" Then
                                addSkill = True
                            End If
                        Case 35
                            If newSkill.Published = True And newSkill.SA = "Intelligence" Then
                                addSkill = True
                            End If
                        Case 36
                            If newSkill.Published = True And newSkill.SA = "Memory" Then
                                addSkill = True
                            End If
                        Case 37
                            If newSkill.Published = True And newSkill.SA = "Perception" Then
                                addSkill = True
                            End If
                        Case 38
                            If newSkill.Published = True And newSkill.SA = "Willpower" Then
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
                            For Each skillQ As EveHQ.Core.SkillQueue In displayPilot.TrainingQueues.Values
                                If inQ = True Then Exit For
                                Dim sQ As Collection = skillQ.Queue
                                For Each skillQueueItem As EveHQ.Core.SkillQueueItem In sQ
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

        If Me.tabQueues.TabPages(QueueName) Is Nothing Then
            Exit Sub
        End If

        Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(QueueName).Controls("TQ" & QueueName), TrainingQueue)
        Dim lvwQueue As EveHQ.DragAndDropListView = tq.lvQueue

        If displayPilot.PilotSkills.Count <> 0 Then
            ' Clear the visible training queue
            lvwQueue.SuspendLayout()
            lvwQueue.BeginUpdate()
            lvwQueue.Items.Clear()

            ' Prep a new font ready for completed training queues
            Dim doneFont As Font = New Font("MS Sans Serif", 8, FontStyle.Strikeout)

            ' Call the main procedure
            Dim aq As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(QueueName), Core.SkillQueue)
            Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(displayPilot, aq)
            Dim qItem As EveHQ.Core.SortedQueue = New EveHQ.Core.SortedQueue
            Dim totalTime As Long = 0
            Dim totalSP As Long = displayPilot.SkillPoints

            ' Create the columns according to the selection in the settings
            Call Me.DrawColumns(lvwQueue)

            If arrQueue IsNot Nothing Then
                For Each qItem In arrQueue
                    Dim newskill As ListViewItem = New ListViewItem
                    newskill.Name = qItem.Key
                    If qItem.Done = False Or (qItem.Done = True And EveHQ.Core.HQ.EveHQSettings.ShowCompletedSkills = True) Then
                        If qItem.Done = True Then newskill.Font = doneFont
                        If qItem.IsPrereq = True Then
                            If qItem.HasPrereq = True Then
                                newskill.ToolTipText &= qItem.Prereq & ControlChars.CrLf & qItem.Reqs
                                newskill.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.BothPreReqColor))
                            Else
                                newskill.ToolTipText = qItem.Prereq
                                newskill.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.IsPreReqColor))
                            End If
                        Else
                            If qItem.HasPrereq = True Then
                                newskill.ToolTipText = qItem.Reqs
                                newskill.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.HasPreReqColor))
                            Else
                                If qItem.PartTrained = True Then
                                    newskill.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PartialTrainColor))
                                Else
                                    newskill.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.ReadySkillColor))
                                End If
                            End If
                        End If
                        If qItem.IsTraining = True Then
                            newskill.BackColor = Color.LimeGreen
                            ' Set a flag in the listview of the listviewitem name for later checking
                            lvwQueue.Tag = newskill.Name
                        End If
                        Dim clashTime As DateTime = EveHQ.Core.SkillFunctions.ConvertLocalTimeToEve(qItem.DateFinished)
                        If clashTime.Hour = 11 Then
                            newskill.ForeColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DTClashColor))
                            If newskill.ToolTipText <> "" Then
                                newskill.ToolTipText &= ControlChars.CrLf & ControlChars.CrLf
                            End If
                            newskill.ToolTipText &= "WARNING: Skill end time occurs during Eve Downtime"
                        End If
                        ' Do some additional calcs
                        totalSP += CLng(qItem.SPTrained)
                        totalTime += CLng(qItem.TrainTime)
                        ' Add the first 6 standard items
                        newskill.Text = qItem.Name
                        newskill.Tag = qItem.ID
                        Dim newSI As New ListViewItem.ListViewSubItem
                        If (qItem.IsInjected) Then
                            newSI.Name = qItem.CurLevel : newSI.Text = qItem.CurLevel : newskill.SubItems.Add(newSI)
                        Else
                            newSI.Name = "" : newSI.Text = "" : newskill.SubItems.Add(newSI)
                        End If
                        newSI = New ListViewItem.ListViewSubItem
                        newSI.Name = qItem.FromLevel : newSI.Text = qItem.FromLevel : newskill.SubItems.Add(newSI)
                        newSI = New ListViewItem.ListViewSubItem
                        newSI.Name = qItem.ToLevel : newSI.Text = qItem.ToLevel : newskill.SubItems.Add(newSI)
                        newSI = New ListViewItem.ListViewSubItem
                        Dim skillPct As Double
                        If displayPilot.PilotSkills.Contains(qItem.Name) Then
                            Dim myCurSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(qItem.Name), Core.PilotSkill)
                            Dim clevel As Integer = CInt(qItem.FromLevel)
                            Dim nextLevelSp As Integer = myCurSkill.LevelUp(clevel + 1) - myCurSkill.LevelUp(clevel)

                            If clevel <> myCurSkill.Level Then
                                skillPct = 0
                            Else
                                If qItem.Name = displayPilot.TrainingSkillName Then
                                    skillPct = CInt(Int((myCurSkill.SP + displayPilot.TrainingCurrentSP - myCurSkill.LevelUp(clevel)) / nextLevelSp * 100))
                                Else
                                    skillPct = CInt(Int((myCurSkill.SP - myCurSkill.LevelUp(clevel)) / nextLevelSp * 100))
                                End If
                            End If

                            If skillPct > 100 Then
                                skillPct = 100
                            End If
                        Else
                            skillPct = 0
                        End If

                        newSI.Name = CStr(skillPct) : newSI.Text = CStr(skillPct) : newskill.SubItems.Add(newSI)
                        newSI = New ListViewItem.ListViewSubItem
                        newSI.Name = qItem.TrainTime : newSI.Tag = qItem.TrainTime : newSI.Text = EveHQ.Core.SkillFunctions.TimeToString(CDbl(qItem.TrainTime)) : newskill.SubItems.Add(newSI)
                        ' Now add the others as required
                        For a As Integer = 6 To 16
                            If CBool(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 1)) = True Then
                                newSI = New ListViewItem.ListViewSubItem
                                Select Case EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0)
                                    Case "Date"
                                        newSI.Name = qItem.DateFinished.ToBinary.ToString
                                        newSI.Text = Format(qItem.DateFinished, "ddd") & " " & FormatDateTime(qItem.DateFinished, DateFormat.GeneralDate)
                                    Case "Rank"
                                        newSI.Name = qItem.Rank
                                        newSI.Text = qItem.Rank
                                    Case "PAtt"
                                        newSI.Name = qItem.PAtt
                                        newSI.Text = qItem.PAtt
                                    Case "SAtt"
                                        newSI.Name = qItem.SAtt
                                        newSI.Text = qItem.SAtt
                                    Case "SPRH"
                                        newSI.Name = qItem.SPRate
                                        newSI.Text = FormatNumber(qItem.SPRate, 0, , , TriState.True)
                                    Case "SPRD"
                                        newSI.Name = CStr(CDbl(qItem.SPRate) * 24)
                                        newSI.Text = FormatNumber(CDbl(qItem.SPRate) * 24, 0, , , TriState.True)
                                    Case "SPRW"
                                        newSI.Name = CStr(CDbl(qItem.SPRate) * 24 * 7)
                                        newSI.Text = FormatNumber(CDbl(qItem.SPRate) * 24 * 7, 0, , , TriState.True)
                                    Case "SPRM"
                                        newSI.Name = CStr(CDbl(qItem.SPRate) * 24 * 30)
                                        newSI.Text = FormatNumber(CDbl(qItem.SPRate) * 24 * 30, 0, , , TriState.True)
                                    Case "SPRY"
                                        newSI.Name = CStr(CDbl(qItem.SPRate) * 24 * 365)
                                        newSI.Text = FormatNumber(CDbl(qItem.SPRate) * 24 * 365, 0, , , TriState.True)
                                    Case "SPAd"
                                        newSI.Name = qItem.SPTrained
                                        newSI.Text = FormatNumber(qItem.SPTrained, 0, , , TriState.True)
                                    Case "SPTo"
                                        newSI.Name = CStr(totalSP)
                                        newSI.Text = FormatNumber(totalSP, 0, , , TriState.True)
                                End Select
                                newskill.SubItems.Add(newSI)
                            End If
                        Next
                        lvwQueue.Items.Add(newskill)
                    End If
                Next
            End If

            Dim lblQueue As Label = tq.lblQueueTime
            lblQueue.Tag = totalTime.ToString
            lblQueue.Text = EveHQ.Core.SkillFunctions.TimeToString(totalTime)

            ' Tidy up afterwards
            lvwQueue.EndUpdate()
            lvwQueue.ResumeLayout()
            Call EveHQ.Core.SkillQueueFunctions.TidyQueue(displayPilot, aq, arrQueue)
            Call Me.RedrawOptions()

            ' Get a suggestion for a quicker skill queue
            Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.MakeQueueSuggestions, aq)

        End If
    End Sub
    Private Sub RedrawOptions()
        ' Determines what buttons and menus are available from the listview!
        If activeLVW IsNot Nothing Then
            ' Check the queue status
            btnICT.Checked = activeLVW.IncludeCurrentTraining
            btnICT.Enabled = True
            If activeLVW.SelectedItems.Count <> 0 Then
                Select Case activeLVW.SelectedItems.Count
                    Case 1
                        Dim skillName As String = ""
                        Dim skillID As String = ""
                        skillName = activeLVW.SelectedItems(0).Text
                        skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
                        mnuSkillName.Text = skillName
                        mnuSkillName.Tag = skillID
                        ' Check if we can increase or decrease levels

                        Dim curLevel, curFLevel, curTLevel As Integer
                        curFLevel = CInt(activeLVW.SelectedItems(0).SubItems(2).Text)
                        curTLevel = CInt(activeLVW.SelectedItems(0).SubItems(3).Text)
                        Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                        If displayPilot.PilotSkills.Contains(skillName) = False Then
                            curLevel = 0
                        Else
                            mySkill = CType(displayPilot.PilotSkills(skillName), Core.PilotSkill)
                            curLevel = mySkill.Level
                        End If
                        mnuIncreaseLevel.Enabled = True : btnLevelUp.Enabled = True
                        mnuDecreaseLevel.Enabled = True : btnLevelDown.Enabled = True
                        mnuDeleteFromQueue.Enabled = True : btnDeleteSkill.Enabled = True
                        mnuMoveUpQueue.Enabled = True : btnMoveUp.Enabled = True
                        mnuMoveDownQueue.Enabled = True : btnMoveDown.Enabled = True
                        btnShowDetails.Enabled = True : btnAddSkill.Enabled = False
                        Me.mnuViewDetails.Enabled = True
                        Me.mnuForceTraining.Enabled = True
                        If curTLevel = 5 Or curLevel = 5 Then
                            mnuIncreaseLevel.Enabled = False : btnLevelUp.Enabled = False
                        End If
                        If curTLevel - 1 <= curFLevel Or curTLevel <= curLevel Then
                            mnuDecreaseLevel.Enabled = False : btnLevelDown.Enabled = False
                        End If
                        If activeLVW.SelectedItems(0).Index = 0 Then
                            mnuMoveUpQueue.Enabled = False : btnMoveUp.Enabled = False
                        End If

                        ' Check if the skill is a pre-req
                        If activeLVW.SelectedItems(0).BackColor = Color.LightSteelBlue Then
                            If activeLVW.SelectedItems(0).SubItems(4).Text = "100" Then
                                mnuDeleteFromQueue.Enabled = True : btnDeleteSkill.Enabled = True
                            Else
                                mnuDeleteFromQueue.Enabled = False : btnDeleteSkill.Enabled = False
                            End If
                        End If
                        ' Check if the skill is at the bottom of the list
                        If activeLVW.SelectedItems(0).Index = activeLVW.Items.Count - 1 Then
                            mnuMoveDownQueue.Enabled = False : btnMoveDown.Enabled = False
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
                                mnuIncreaseLevel.Enabled = False : btnLevelUp.Enabled = False
                                mnuDecreaseLevel.Enabled = False : btnLevelDown.Enabled = False
                                mnuDeleteFromQueue.Enabled = False : btnDeleteSkill.Enabled = False
                                mnuMoveUpQueue.Enabled = False : btnMoveUp.Enabled = False
                                mnuMoveDownQueue.Enabled = False : btnMoveDown.Enabled = False
                            Else
                                If activeLVW.SelectedItems(0).Index = 1 Then
                                    mnuMoveUpQueue.Enabled = False : btnMoveUp.Enabled = False
                                End If
                            End If
                        End If
                    Case Is > 1
                        mnuIncreaseLevel.Enabled = False : btnLevelUp.Enabled = False
                        mnuDecreaseLevel.Enabled = False : btnLevelDown.Enabled = False
                        mnuDeleteFromQueue.Enabled = True : btnDeleteSkill.Enabled = True
                        mnuMoveUpQueue.Enabled = False : btnMoveUp.Enabled = False
                        mnuMoveDownQueue.Enabled = False : btnMoveDown.Enabled = False
                        btnShowDetails.Enabled = False : btnAddSkill.Enabled = False
                        Me.mnuViewDetails.Enabled = False
                        Me.mnuForceTraining.Enabled = False
                        Me.mnuSeperateLevelSep.Visible = True
                End Select
            Else
                btnLevelUp.Enabled = False
                btnLevelDown.Enabled = False
                btnDeleteSkill.Enabled = False
                btnMoveUp.Enabled = False
                btnMoveDown.Enabled = False
                btnShowDetails.Enabled = False
                btnAddSkill.Enabled = False
            End If
        Else
            btnLevelUp.Enabled = False
            btnLevelDown.Enabled = False
            btnDeleteSkill.Enabled = False
            btnMoveUp.Enabled = False
            btnMoveDown.Enabled = False
            btnShowDetails.Enabled = False
            btnAddSkill.Enabled = False
            btnICT.Checked = False
            btnICT.Enabled = False
        End If
    End Sub
    Public Sub UpdateTraining()
        ' Only perform this if the form isn't starting
        If startup = False Then
            If displayPilot.Training = True Then
                If Me.tabQueues.TabPages.Count > 1 Then
                    For Each tp As TabPage In Me.tabQueues.TabPages
                        If tp.Name <> "tabSummary" Then
                            Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(tp.Name).Controls("TQ" & tp.Name), TrainingQueue)
                            Dim cLabel As Label = tq.lblQueueTime
                            Dim cLVW As EveHQ.DragAndDropListView = tq.lvQueue
                            Dim newQ As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(tp.Name), Core.SkillQueue)
                            Dim bIncludeSkill As Boolean = False
                            For Each skill As EveHQ.Core.SkillQueueItem In newQ.Queue
                                If (skill.Name = displayPilot.TrainingSkillName) And displayPilot.TrainingSkillLevel > skill.FromLevel And displayPilot.TrainingSkillLevel <= skill.ToLevel Then
                                    bIncludeSkill = True
                                    Exit For
                                End If
                            Next
                            If bIncludeSkill Then
                                If cLVW.Items.Count > 0 Then
                                    Dim myCurSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)), Core.PilotSkill)
                                    Dim clevel As Integer = displayPilot.TrainingSkillLevel
                                    Dim cTime As Double = displayPilot.TrainingCurrentTime
                                    Dim strTime As String = EveHQ.Core.SkillFunctions.TimeToString(cTime)
                                    Dim endtime As Date = displayPilot.TrainingEndTime
                                    Dim percent As Integer = 0
                                    percent = CInt(Int((myCurSkill.SP + displayPilot.TrainingCurrentSP - myCurSkill.LevelUp(clevel - 1)) / (myCurSkill.LevelUp(clevel) - myCurSkill.LevelUp(clevel - 1)) * 100))
                                    If (percent > 100) Then
                                        percent = 100
                                    End If

                                    Dim lvi As ListViewItem = Nothing
                                    For Each lvi In cLVW.Items
                                        If lvi.Text = myCurSkill.Name Then
                                            Exit For
                                        End If
                                    Next
                                    lvi.SubItems(4).Text = CStr(percent)
                                    lvi.SubItems(5).Tag = cTime
                                    lvi.SubItems(5).Text = strTime

                                    ' Calculate total time
                                    If cLVW.Items.Count > 0 Then
                                        Dim totalTime As Double = 0
                                        For count As Integer = 0 To cLVW.Items.Count - 1
                                            totalTime += CLng(cLVW.Items(count).SubItems(5).Tag)
                                        Next
                                        cLabel.Tag = totalTime.ToString
                                        cLabel.Text = EveHQ.Core.SkillFunctions.TimeToString(totalTime)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
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
                                totalTime = CLng(totalTime + EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, toLevel, toLevel - 1, , TrainingBonus))
                        End Select
                        lvwTimes.Items(toLevel - 1).SubItems(3).Text = EveHQ.Core.SkillFunctions.TimeToString(totalTime)
                    Next
                End If
                ' Update the queue summary data
                For Each newQ As EveHQ.Core.SkillQueue In displayPilot.TrainingQueues.Values
                    Try
                        Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(activeQueueName).Controls("TQ" & activeQueueName), TrainingQueue)
                        Dim tTime As Double = CDbl(tq.lblQueueTime.Tag)
                        lvQueues.Items(newQ.Name).SubItems(2).Tag = tTime
                        lvQueues.Items(newQ.Name).SubItems(2).Text = (EveHQ.Core.SkillFunctions.TimeToString(tTime))
                        Dim qTime As Double = tTime
                        Dim bIncludeSkill As Boolean = False
                        For Each skill As EveHQ.Core.SkillQueueItem In newQ.Queue
                            If (skill.Name = displayPilot.TrainingSkillName) And displayPilot.TrainingSkillLevel > skill.FromLevel And displayPilot.TrainingSkillLevel <= skill.ToLevel Then
                                bIncludeSkill = True
                                Exit For
                            End If
                        Next
                        If bIncludeSkill Then
                            qTime = tTime - displayPilot.TrainingCurrentTime
                        End If
                        lvQueues.Items(newQ.Name).SubItems(3).Tag = qTime
                        lvQueues.Items(newQ.Name).SubItems(3).Text = EveHQ.Core.SkillFunctions.TimeToString(qTime)
                        Dim eTime As Date = Now.AddSeconds(tTime)
                        lvQueues.Items(newQ.Name).SubItems(4).Text = (Format(eTime, "ddd") & " " & FormatDateTime(eTime, DateFormat.GeneralDate))
                    Catch e As Exception
                        ' Error will most likely be if a skill queue is in the process of deletion.
                    End Try
                Next
                If Me.selQTime > 0 Then
                    lblTotalQueueTime.Text = "Selected Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(Me.selQTime + displayPilot.TrainingCurrentTime) & " (" & EveHQ.Core.SkillFunctions.TimeToString(Me.selQTime) & ")"
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
        btnLevelUp.Enabled = False
        btnLevelDown.Enabled = False
        btnDeleteSkill.Enabled = False
        btnMoveUp.Enabled = False
        btnMoveDown.Enabled = False
        btnShowDetails.Enabled = True
        btnAddSkill.Enabled = True
        If e.Node.Parent IsNot Nothing Or (e.Node.Parent Is Nothing And usingFilter = False) Then
            Dim skillID As String = e.Node.Name
            Call Me.ShowSkillDetails(skillID)
        End If
    End Sub
    Private Sub tvwSkillList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvwSkillList.DoubleClick
        If tvwSkillList.SelectedNode.Level = 1 Or (tvwSkillList.SelectedNode.Level = 0 And usingFilter = False) Then
            Dim skillID As String
            skillID = tvwSkillList.SelectedNode.Name
            frmSkillDetails.DisplayPilotName = displayPilot.Name
            Call frmSkillDetails.ShowSkillDetails(skillID)
        End If
    End Sub
    Private Sub tvwSkillList_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles tvwSkillList.ItemDrag
        Dim addSkill As DragItemData = New DragItemData(Me.activeLVW)
        Dim newLVItem As ListViewItem = ConvertTreeNode(CType(e.Item, TreeNode))
        addSkill.DragItems.Add(newLVItem)
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
#End Region

#Region "Skill Queue UI Routines"
    Private Sub activeLVW_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim skillID As String
        skillID = EveHQ.Core.SkillFunctions.SkillNameToID(activeLVW.SelectedItems(0).Text)
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
        If CInt(activeLVW.SortColumn) = e.Column Then
            Me.activeLVW.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, Windows.Forms.SortOrder.Ascending)
            activeLVW.SortColumn = -1
        Else
            Me.activeLVW.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, Windows.Forms.SortOrder.Descending)
            activeLVW.SortColumn = e.Column
        End If
        ' Call the sort method to manually sort.
        activeLVW.Sort()
    End Sub
    Private Sub activeLVW_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Call Me.RedrawOptions()
    End Sub
    Private Sub activeLVW_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If activeLVW.SelectedItems.Count <> 0 Then
            Call Me.RedrawOptions()
            Dim skillName As String = activeLVW.SelectedItems(0).Text
            Dim skillID As String = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
            Call Me.ShowSkillDetails(skillID)
        End If
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
        If tvwCertList.SelectedNode Is Nothing Then
            btnShowDetails.Enabled = False
        End If
    End Sub
    Private Sub LoadCertGroups()
        certListNodes.Clear()
        For Each CertCat As Core.CertificateCategory In EveHQ.Core.HQ.CertificateCategories.Values
            Dim groupNode As TreeNode = New TreeNode
            groupNode.Name = CertCat.ID.ToString
            groupNode.Text = CertCat.Name
            groupNode.ImageIndex = 8
            groupNode.SelectedImageIndex = 8
            certListNodes.Add(groupNode.Name, groupNode)
        Next
    End Sub
    Private Sub LoadFilteredCerts(ByVal filter As Integer)
        Dim groupNode As New TreeNode
        Dim addCert As Boolean = False
        For Each newCert As Core.Certificate In Core.HQ.Certificates.Values
            addCert = False
            groupNode = CType(certListNodes.Item(newCert.CategoryID.ToString), TreeNode)
            Select Case filter
                Case 0
                    addCert = True
                Case 1
                    If displayPilot.Certificates.Contains(newCert.ID.ToString) = True Then
                        addCert = True
                    End If
                Case 2
                    If displayPilot.Certificates.Contains(newCert.ID.ToString) = False Then
                        addCert = True
                    End If
                Case 3 To 7
                    If newCert.Grade = filter - 2 Then
                        addCert = True
                    End If
                Case 8 To 12
                    If newCert.Grade = filter - 7 And displayPilot.Certificates.Contains(newCert.ID.ToString) = False Then
                        addCert = True
                    End If
            End Select
            If addCert = True Then
                Dim certNode As New TreeNode
                certNode.Text = CType(Core.HQ.CertificateClasses(newCert.ClassID.ToString), Core.CertificateClass).Name & " (" & newCert.Grade & " - " & CertGrades(newCert.Grade) & ")"
                certNode.Name = newCert.ID.ToString
                If displayPilot.Certificates.Contains(newCert.ID.ToString) = True Then
                    certNode.ImageIndex = newCert.Grade
                    certNode.SelectedImageIndex = newCert.Grade
                Else
                    certNode.ImageIndex = 10
                    certNode.SelectedImageIndex = 10
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

    Private Sub tvwCertList_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwCertList.NodeMouseClick
        tvwCertList.SelectedNode = e.Node
    End Sub

    Private Sub ctxCertDetails_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxCertDetails.Opening
        Dim curNode As TreeNode = New TreeNode
        curNode = tvwCertList.SelectedNode
        If curNode IsNot Nothing Then
            ' Reset grades
            For grade As Integer = 1 To 5
                mnuAddCertToQueue.DropDownItems("mnuAddCertToQueue" & grade).Enabled = False
            Next
            Dim certName As String = ""
            Dim certID As String = ""
            certName = curNode.Text
            certID = curNode.Name
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
                Dim selCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(certID), Core.Certificate)
                Dim selCertClass As Integer = selCert.ClassID
                For Each testCert As EveHQ.Core.Certificate In EveHQ.Core.HQ.Certificates.Values
                    If testCert.ClassID = selCertClass Then
                        ' Check if the pilot has it
                        If displayPilot.Certificates.Contains(testCert.ID.ToString) = False Then
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
        Dim certID As String = mnuCertName.Tag.ToString
        frmCertificateDetails.DisplayPilotName = displayPilot.Name
        frmCertificateDetails.ShowCertDetails(certID)
    End Sub

    Private Sub mnuAddCertToQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCertToQueue1.Click, mnuAddCertToQueue2.Click, mnuAddCertToQueue3.Click, mnuAddCertToQueue4.Click, mnuAddCertToQueue5.Click
        ' Get the certificate details
        Dim grade As Integer = CInt(CType(sender, ToolStripItem).Name.Substring(CType(sender, ToolStripItem).Name.Length - 1, 1))
        Dim certID As String = mnuCertName.Tag.ToString
        Dim certClass As Integer = CType(EveHQ.Core.HQ.Certificates(certID), EveHQ.Core.Certificate).ClassID
        For Each cert As EveHQ.Core.Certificate In EveHQ.Core.HQ.Certificates.Values
            If cert.ClassID = certClass Then
                If cert.Grade = grade Then
                    Call AddCertSkills(cert)
                End If
            End If
        Next
        ' Refresh our training queue
        Call Me.RefreshTraining(activeQueueName)
    End Sub

    Private Sub AddCertSkills(ByVal cert As EveHQ.Core.Certificate)
        Dim reqSkills As SortedList = cert.RequiredSkills
        For Each reqSkill As String In reqSkills.Keys
            EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, CStr(EveHQ.Core.SkillFunctions.SkillIDToName(reqSkill)), activeQueue.Queue.Count + 1, activeQueue, CInt(reqSkills(reqSkill)), True, True)
        Next
        ' Get a list of the certs that are required
        For Each reqCertID As String In cert.RequiredCerts.Keys
            Call AddCertSkills(CType(EveHQ.Core.HQ.Certificates(reqCertID), Core.Certificate))
        Next
    End Sub

    Private Sub mnuAddCertGroupToQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCertGroupToQueue1.Click, mnuAddCertGroupToQueue2.Click, mnuAddCertGroupToQueue3.Click, mnuAddCertGroupToQueue4.Click, mnuAddCertGroupToQueue5.Click
        ' Get the Grade required
        Dim grade As Integer = CInt(CType(sender, ToolStripItem).Name.Substring(CType(sender, ToolStripItem).Name.Length - 1, 1))
        Dim certCat As String = mnuCertName.Tag.ToString
        For Each cert As EveHQ.Core.Certificate In EveHQ.Core.HQ.Certificates.Values
            If cert.CategoryID = CInt(certCat) Then
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

    Private Sub ShowSkillDetails(ByVal skillID As String)

        If displayPilot.SkillPoints + displayPilot.TrainingCurrentSP < TrainingThreshold Then
            TrainingBonus = 2
        Else
            TrainingBonus = 1
        End If

        Call Me.PrepareDetails(skillID)
        Call Me.PrepareTree(skillID)
        Call Me.PrepareDepends(skillID)
        Call Me.PrepareDescription(skillID)
        Call Me.PrepareSPs(skillID)
        Call Me.PrepareTimes(skillID)

    End Sub
    Private Sub PrepareDetails(ByVal skillID As String)

        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
        lvwDetails.Groups(1).Header = "Pilot Specific - " & displayPilot.Name

        With Me.lvwDetails
            Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
            Dim myGroup As EveHQ.Core.SkillGroup = New EveHQ.Core.SkillGroup
            Dim groupName As String = EveHQ.Core.HQ.itemGroups(cSkill.GroupID)
            If EveHQ.Core.HQ.SkillGroups.ContainsKey(groupName) = True Then
                myGroup = EveHQ.Core.HQ.SkillGroups(groupName)
            Else
                myGroup = Nothing
            End If
            Dim cLevel, cSP, cTime, cRate As String
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
                If displayPilot.PilotSkills.Contains(cSkill.Name) = False Then
                    cLevel = "0" : cSP = "0" : cTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, 1, , , TrainingBonus))
                    cRate = CStr(EveHQ.Core.SkillFunctions.CalculateSPRate(displayPilot, cSkill))
                Else
                    mySkill = CType(displayPilot.PilotSkills(cSkill.Name), Core.PilotSkill)
                    cLevel = mySkill.Level.ToString
                    If displayPilot.Training = True And displayPilot.TrainingSkillID = EveHQ.Core.SkillFunctions.SkillNameToID(cSkill.Name) Then
                        cSP = CStr(mySkill.SP + displayPilot.TrainingCurrentSP)
                    Else
                        cSP = mySkill.SP.ToString
                    End If
                    If displayPilot.Training = True And displayPilot.TrainingSkillName = cSkill.Name Then
                        cTime = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    Else
                        cTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, 0, , , TrainingBonus))
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
                Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
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
                Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
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
                    newItem.Tag = itemData(0)
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
                    newItem.Group = lvwDepend.Groups("CatCerts")
                    Dim cert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(item), Core.Certificate)
                    newItem.Tag = cert.ID
                    certName = CType(EveHQ.Core.HQ.CertificateClasses(cert.ClassID.ToString), EveHQ.Core.CertificateClass).Name
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
                        Dim reqCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(reqCertID), Core.Certificate)
                        If reqCert.ID.ToString <> item Then
                            newItem.ToolTipText &= CType(EveHQ.Core.HQ.CertificateClasses(reqCert.ClassID.ToString), EveHQ.Core.CertificateClass).Name
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
                    If displayPilot.Certificates.Contains(cert.ID.ToString) = True Then
                        newItem.ForeColor = Color.Green
                    Else
                        newItem.ForeColor = Color.Red
                    End If
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
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, toLevel - 1, , TrainingBonus)))
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, 0, , TrainingBonus)))
                newGroup.SubItems.Add(EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cskill, toLevel, -1, , TrainingBonus)))
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
        Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
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
                            totalTime = CLng(totalTime + EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, toLevel, toLevel - 1, , TrainingBonus))
                    End Select
                    lvwTimes.Items(toLevel - 1).SubItems(3).Text = EveHQ.Core.SkillFunctions.TimeToString(totalTime)
                Next
            End If
        End If
    End Sub

#End Region

#Region "Skill Queue Modification Functions"
    Private Sub AddSkillToQueueOption()
        activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue)
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub DeleteFromQueueOption()
        ' Get the skill name and levels
        For selItem As Integer = 0 To activeLVW.SelectedItems.Count - 1
            Dim skillName As String = activeLVW.SelectedItems(selItem).Text
            Dim fromLevel As String = activeLVW.SelectedItems(selItem).SubItems(2).Text
            Dim toLevel As String = activeLVW.SelectedItems(selItem).SubItems(3).Text
            Dim keyName As String = skillName & fromLevel & toLevel
            ' Remove it from the queue
            Dim mySkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            If activeQueue.Queue.Contains(keyName) = True Then
                mySkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
                ' Delete the Skill
                Call Me.DeleteFromQueue(mySkill)
            End If
        Next
        ' Refresh the training view!
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub DeleteFromQueue(ByVal mySkill As EveHQ.Core.SkillQueueItem)
        Dim delPOS As Integer = mySkill.Pos
        activeQueue.Queue.Remove(mySkill.Name & mySkill.FromLevel & mySkill.ToLevel)
        ' Reshuffle all the positions below
        For Each mySkill In activeQueue.Queue
            If mySkill.Pos > delPOS Then
                mySkill.Pos -= 1
            End If
        Next
    End Sub
    Private Sub IncreaseLevel()
        ' Store the index being used
        Dim oldIndex As Integer = activeLVW.SelectedItems(0).Index
        ' Get the skill name
        Dim skillName As String = activeLVW.SelectedItems(0).Text
        Dim curLevel As String = activeLVW.SelectedItems(0).SubItems(1).Text
        Dim fromLevel As String = activeLVW.SelectedItems(0).SubItems(2).Text
        Dim toLevel As String = activeLVW.SelectedItems(0).SubItems(3).Text
        Dim keyName As String = skillName & fromLevel & toLevel
        Dim myTSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        myTSkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)

        ' Check if we have another skill that can be affected by us increasing the level i.e. the same skill!
        Dim checkSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each checkSkill In activeQueue.Queue
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
                        activeQueue.Queue.Add(checkSkill, checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel)
                    End If
                End If
            End If
        Next

        ' Check if the skill has been trained and we wish to increase it to the next level
        If activeLVW.SelectedItems(0).Font.Strikeout = True Then
            myTSkill.FromLevel = Math.Max(CInt(curLevel), myTSkill.FromLevel)
        End If

        ' Delete the existing item
        activeQueue.Queue.Remove(keyName)
        ' Adjust the "to" level
        myTSkill.ToLevel += 1
        ' Add the item back in at its new levels
        activeQueue.Queue.Add(myTSkill, myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel)
        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(oldIndex).Selected = True
    End Sub
    Private Sub DecreaseLevel()
        ' Store the index being used
        Dim oldIndex As Integer = activeLVW.SelectedItems(0).Index
        ' Get the skill name
        Dim skillName As String = activeLVW.SelectedItems(0).Text
        Dim fromLevel As String = activeLVW.SelectedItems(0).SubItems(2).Text
        Dim toLevel As String = activeLVW.SelectedItems(0).SubItems(3).Text
        Dim keyName As String = skillName & fromLevel & toLevel
        Dim myTSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        myTSkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
        ' Delete the existing item
        activeQueue.Queue.Remove(keyName)
        ' Adjust the "to" level
        myTSkill.ToLevel -= 1
        ' Add the item back in at its new levels
        activeQueue.Queue.Add(myTSkill, myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel)
        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(oldIndex).Selected = True
    End Sub
    Private Sub MoveUpQueue()
        ' Store the keyname being used
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim sourceSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        sourceSkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
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
            Dim destSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            For Each destSkill In activeQueue.Queue
                If destSkill.Pos = di Then Exit For
            Next

            Dim din As String = destSkill.Name & destSkill.FromLevel & destSkill.ToLevel
            Dim sin As String = sourceSkill.Name & sourceSkill.FromLevel & sourceSkill.ToLevel

            Dim mySSkill As EveHQ.Core.SkillQueueItem
            mySSkill = CType(activeQueue.Queue(sin), Core.SkillQueueItem)
            ' Move all the items up or down depending on position
            If si > di Then
                Dim moveSkill As EveHQ.Core.SkillQueueItem
                For Each moveSkill In activeQueue.Queue
                    If moveSkill.Pos >= di And moveSkill.Pos < si Then
                        moveSkill.Pos += 1
                    End If
                Next
            Else
                Dim moveSkill As EveHQ.Core.SkillQueueItem
                For Each moveSkill In activeQueue.Queue
                    If moveSkill.Pos > si And moveSkill.Pos <= di Then
                        moveSkill.Pos -= 1
                    End If
                Next

            End If
            ' Set the source skill to the new location
            mySSkill.Pos = di

            ' Check for movement in the queue
            Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(displayPilot, activeQueue)
            Dim posSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            posSkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
            newPOS = posSkill.Pos

        Loop Until oldPOS <> newPOS Or di = maxJump

        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(keyName).Selected = True
    End Sub
    Private Sub MoveDownQueue()
        ' Store the keyname being used
        Dim keyName As String = activeLVW.SelectedItems(0).Name
        Dim sourceSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        sourceSkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
        Dim oldpos As Integer = sourceSkill.Pos
        Dim newpos As Integer = 0
        Dim di As Integer = 0
        Dim maxJump As Integer = activeQueue.Queue.Count
        Dim queueJump As Integer = 0
        Do
            queueJump += 1
            Dim si As Integer = sourceSkill.Pos
            di = si + queueJump
            Dim destSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            For Each destSkill In activeQueue.Queue
                If destSkill.Pos = di Then Exit For
            Next

            Dim din As String = destSkill.Name & destSkill.FromLevel & destSkill.ToLevel
            Dim sin As String = sourceSkill.Name & sourceSkill.FromLevel & sourceSkill.ToLevel

            Dim mySSkill As EveHQ.Core.SkillQueueItem
            mySSkill = CType(activeQueue.Queue(sin), Core.SkillQueueItem)
            ' Move all the items up or down depending on position
            If si > di Then
                Dim moveSkill As EveHQ.Core.SkillQueueItem
                For Each moveSkill In activeQueue.Queue
                    If moveSkill.Pos >= di And moveSkill.Pos < si Then
                        moveSkill.Pos += 1
                    End If
                Next
            Else
                Dim moveSkill As EveHQ.Core.SkillQueueItem
                For Each moveSkill In activeQueue.Queue
                    If moveSkill.Pos > si And moveSkill.Pos <= di Then
                        moveSkill.Pos -= 1
                    End If
                Next

            End If
            ' Set the source skill to the new location
            mySSkill.Pos = di

            ' Check for movement in the queue
            Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(displayPilot, activeQueue)
            Dim posSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            posSkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
            newpos = posSkill.Pos

        Loop Until oldpos <> newpos Or di = maxJump

        Call Me.RefreshTraining(activeQueueName)
        activeLVW.Items(keyName).Selected = True
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
        Dim skillName As String = activeLVW.SelectedItems(0).Text
        Dim curLevel As String = activeLVW.SelectedItems(0).SubItems(1).Text
        Dim fromLevel As String = activeLVW.SelectedItems(0).SubItems(2).Text
        Dim toLevel As String = activeLVW.SelectedItems(0).SubItems(3).Text
        Dim keyName As String = skillName & fromLevel & toLevel
        Dim myTSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        If activeQueue.Queue.Contains(keyName) = True Then
            myTSkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
            If selectedLevel < CInt(toLevel) Then
                ' Delete the existing item
                activeQueue.Queue.Remove(keyName)
                ' Adjust the "to" level
                myTSkill.ToLevel = selectedLevel
                ' Add the item back in at its new levels
                activeQueue.Queue.Add(myTSkill, myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel)
                Call Me.RefreshTraining(activeQueueName)
                activeLVW.Items(oldIndex).Selected = True
            End If
            If selectedLevel > CInt(toLevel) Then
                ' Check if we have another skill that can be affected by us increasing the level i.e. the same skill!
                Dim checkSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                For Each checkSkill In activeQueue.Queue
                    If myTSkill.Name = checkSkill.Name Then ' Matched skill name
                        ' Check the "from" skill level matches
                        If checkSkill.FromLevel >= myTSkill.ToLevel Then
                            ' We have to decide what to do here, either increase the levels or merge
                            If checkSkill.ToLevel <= selectedLevel Then
                                ' We have to merge the items here so delete the new found one
                                Call Me.DeleteFromQueue(checkSkill)
                            Else
                                ' We have to increase the levels so decrease the new found one
                                ' Delete the existing item
                                activeQueue.Queue.Remove(checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel)
                                ' Adjust the "to" level
                                checkSkill.FromLevel = selectedLevel + 1
                                ' Add the item back in at its new levels
                                activeQueue.Queue.Add(checkSkill, checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel)
                            End If
                        End If
                    End If
                Next

                ' Check if the skill has been trained and we wish to increase it to the next level
                If activeLVW.SelectedItems(0).Font.Strikeout = True Then
                    myTSkill.FromLevel = Math.Max(CInt(curLevel), myTSkill.FromLevel)
                End If

                ' Delete the existing item
                activeQueue.Queue.Remove(keyName)
                ' Adjust the "to" level
                myTSkill.ToLevel = selectedLevel
                ' Add the item back in at its new levels
                activeQueue.Queue.Add(myTSkill, myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel)
                Call Me.RefreshTraining(activeQueueName)
                activeLVW.Items(oldIndex).Selected = True
            End If
        End If
    End Sub
#End Region

#Region "Skill Tree UI Functions"
    Private Sub cboFilter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFilter.TextChanged
        If cboFilter.Items.Contains(cboFilter.Text) = True Then
            usingFilter = True
        Else
            usingFilter = False
            Call Me.LoadSkillTreeSearch()
        End If
    End Sub
    Private Sub chkOmitQueuesSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOmitQueuesSkills.CheckedChanged
        If Me.chkOmitQueuesSkills.Checked = True Then
            omitQueuedSkills = True
        Else
            omitQueuedSkills = False
        End If
        If usingFilter = True Then
            Call Me.LoadSkillTree()
        Else
            Call Me.LoadSkillTreeSearch()
        End If
    End Sub
    Private Sub LoadSkillTreeSearch()
        If Len(cboFilter.Text) > 1 Then
            Dim strSearch As String = cboFilter.Text.Trim.ToLower
            Dim results As New SortedList(Of String, String)
            Dim newSkill As New EveHQ.Core.EveSkill
            For Each newSkill In EveHQ.Core.HQ.SkillListID.Values
                If newSkill.Name.ToLower.Contains(strSearch) Then
                    results.Add(newSkill.Name, newSkill.Name)
                End If
            Next

            tvwSkillList.BeginUpdate()
            tvwSkillList.Nodes.Clear()
            For Each item As String In results.Values
                newSkill = EveHQ.Core.HQ.SkillListName(item)
                If newSkill.GroupID <> "505" And newSkill.Published = True Then
                    Dim skillNode As TreeNode = New TreeNode
                    skillNode.Text = newSkill.Name
                    skillNode.Name = newSkill.ID
                    If displayPilot.PilotSkills.Contains(newSkill.Name) = True Then
                        Dim mySkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
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
                        For Each skillQ As EveHQ.Core.SkillQueue In displayPilot.TrainingQueues.Values
                            If inQ = True Then Exit For
                            Dim sQ As Collection = skillQ.Queue
                            For Each skillQueueItem As EveHQ.Core.SkillQueueItem In sQ
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
#End Region

#Region "Skill Queue Summary UI Functions"
    Private Sub btnDeleteQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteQueue.Click
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
                Call Me.RefreshAllTraining()
            Else
                lvQueues.Select()
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnAddQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddQueue.Click
        ' Clear the text boxes
        Dim myQueue As frmModifyQueues = New frmModifyQueues
        With myQueue
            .txtQueueName.Text = "" : .txtQueueName.Enabled = True
            .btnAccept.Text = "Add" : .Tag = "Add"
            .Text = "Add New Queue"
            .DisplayPilotName = displayPilot.Name
            .ShowDialog()           ' New Queue checking and adding is done on the modal form!
        End With
        Call Me.RefreshAllTraining()
    End Sub
    Private Sub btnEditQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditQueue.Click
        ' Check for some selection on the listview
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to edit!", "Cannot Edit Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            Dim myQueue As frmModifyQueues = New frmModifyQueues
            With myQueue
                ' Load the account details into the text boxes
                Dim selQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name), EveHQ.Core.SkillQueue)
                .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Name
                .btnAccept.Text = "Edit" : .Tag = "Edit"
                .Text = "Edit '" & selQueue.Name & "' Queue Details"
                .DisplayPilotName = displayPilot.Name
                .ShowDialog()
            End With
            Call Me.RefreshAllTraining()
        End If
    End Sub
    Private Sub btnCopyQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyQueue.Click
        ' Check for some selection on the listview
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to copy!", "Cannot Copy Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            Dim myQueue As frmModifyQueues = New frmModifyQueues
            With myQueue
                ' Load the account details into the text boxes
                Dim selQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name), EveHQ.Core.SkillQueue)
                .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Name
                .btnAccept.Text = "Copy" : .Tag = "Copy"
                .Text = "Copy '" & selQueue.Name & "' Queue Details"
                .DisplayPilotName = displayPilot.Name
                .ShowDialog()
            End With
            Call Me.RefreshAllTraining()
        End If
    End Sub
    Private Sub btnMergeQueues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMergeQueues.Click
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
    Private Sub btnImportEveMon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportEveMon.Click
        ' Set recalc flag
        Dim RecalcQueues As Boolean = False
        ' Try to find the EveMon settings file
        Dim EveMonLocation As String = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveMon"), "Settings.xml")
        If My.Computer.FileSystem.FileExists(EveMonLocation) = False Then
            MessageBox.Show("EveMon Settings File Not Found." & ControlChars.CrLf & ControlChars.CrLf & "Please check the EveMon installation.", "EveMon Settings Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        Else
            Try
                ' Load the Settings file into an XMLDocument
                Dim EMXML As New Xml.XmlDocument
                EMXML.Load(EveMonLocation)
                ' Get a list of the characters that are in files (not API)
                Dim EMPilots As New SortedList
                Dim CharDetails As Xml.XmlNodeList
                Dim CharNode As Xml.XmlNode
                CharDetails = EMXML.SelectNodes("/logindata2/CharFileList/CharFileInfo")
                If CharDetails.Count > 0 Then
                    ' Need to add details of pilots here
                    For Each CharNode In CharDetails
                        EMPilots.Add(CharNode.ChildNodes(0).InnerText, CharNode.ChildNodes(3).InnerText)
                    Next
                End If
                ' Don't need to get a list of the characters that are in the API - this info is retrieved direct from the plan!
                'CharDetails = EMXML.SelectNodes("/logindata2/CharacterList/CharLoginInfo")
                'If CharDetails.Count > 0 Then
                '    ' Need to add details of pilots here
                '    For Each CharNode In CharDetails
                '        ' Check char nodes for the "CharacterName" node
                '        For Each cNode As Xml.XmlNode In CharNode.ChildNodes
                '            If cNode.InnerXml = "CharacterName" Then
                '                MessageBox.Show("Fuck, yeah!")
                '                'EMPilots.Add("API" & CharNode.ChildNodes(3).InnerText, CharNode.ChildNodes(3).InnerText)
                '            End If
                '        Next
                '    Next
                'End If

                ' Try and get the plan information
                Dim PlanDetails As Xml.XmlNodeList
                Dim PlanNode As Xml.XmlNode
                Dim PlansItemNode As Xml.XmlNode
                Dim PlansParentNode As Xml.XmlNode
                PlanDetails = EMXML.SelectNodes("/logindata2/Plans/PairOfStringPlan")
                If PlanDetails.Count > 0 Then
                    Dim PlanInfo(PlanDetails.Count, 2) As String
                    Dim count As Integer = -1
                    For Each PlanNode In PlanDetails
                        count += 1
                        Dim planText As String = PlanNode.ChildNodes(0).InnerText.Replace("::", "#")
                        Dim planTexts() As String = planText.Split("#".ToCharArray)
                        Dim pilotName As String = ""
                        Dim planName As String = planTexts(1)
                        If EMPilots.Contains(planTexts(0)) = True Then
                            ' File pilot
                            pilotName = CStr(EMPilots.Item(planTexts(0)))
                        Else
                            ' API pilot
                            pilotName = planTexts(0)
                        End If
                        PlanInfo(count, 0) = pilotName : PlanInfo(count, 1) = planName
                        PlansParentNode = PlanNode.ChildNodes(1)
                        Dim SQ As New Collection
                        Dim SQCount As Integer = 0
                        For Each PlansItemNode In PlansParentNode.ChildNodes(0)
                            SQCount += 1
                            Dim SQI As New EveHQ.Core.SkillQueueItem
                            SQI.Name = PlansItemNode.ChildNodes(0).InnerText
                            SQI.ToLevel = CInt(PlansItemNode.ChildNodes(1).InnerText)
                            SQI.FromLevel = SQI.ToLevel - 1
                            SQI.Pos = SQCount
                            SQI.Key = SQI.Name & SQI.FromLevel & SQI.ToLevel
                            SQ.Add(SQI, SQI.Key)
                        Next
                        PlanInfo(count, 2) = SQ.Count.ToString

                        ' Check if we have a relevant pilot!
                        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(PlanInfo(count, 0)) = True Then
                            ' Ok, load up the plan
                            Dim newSQ As New EveHQ.Core.SkillQueue
                            newSQ.Name = PlanInfo(count, 1)
                            newSQ.IncCurrentTraining = True
                            newSQ.Primary = False
                            newSQ.Queue = SQ
                            Dim QPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(PlanInfo(count, 0)), Core.Pilot)
                            If QPilot.TrainingQueues.Contains(PlanInfo(count, 1)) = False Then
                                QPilot.TrainingQueues.Add(newSQ.Name, newSQ)
                                RecalcQueues = True
                            End If
                        End If
                    Next
                End If

                ' Recalc the queues if appropriate
                If RecalcQueues = True Then
                    Call Me.RefreshAllTraining()
                End If
            Catch ex As Exception
                MessageBox.Show("Error importing EveMon plans." & ControlChars.CrLf & ControlChars.CrLf & "Error: " & ex.Message & ControlChars.CrLf & ex.StackTrace, "Import EveMon Plans Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End If
    End Sub
    Private Sub btnSetPrimary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetPrimary.Click
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to call Primary!", "Cannot Set Primary Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            ' Remove the current primary queue (if exists!)
            Dim oldPQ As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(displayPilot.PrimaryQueue), EveHQ.Core.SkillQueue)
            If oldPQ IsNot Nothing Then
                oldPQ.Primary = False
            End If
            displayPilot.PrimaryQueue = ""
            ' Select the new primary queue
            Dim selQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name), EveHQ.Core.SkillQueue)
            selQueue.Primary = True
            displayPilot.PrimaryQueue = selQueue.Name
            Call Me.DrawQueueSummary()
        End If
    End Sub
    Private Sub btnCopyToPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyToPilot.Click
        ' Check for some selection on the listview
        If lvQueues.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a Queue to copy!", "Cannot Copy Queue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvQueues.Select()
        Else
            Dim myQueue As frmSelectQueuePilot = New frmSelectQueuePilot
            With myQueue
                ' Load the account details into the text boxes
                Dim selQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(lvQueues.SelectedItems(0).Name), EveHQ.Core.SkillQueue)
                .DisplayPilotName = displayPilot.Name
                .cboPilots.Tag = selQueue.Name
                .ShowDialog()
            End With
        End If
    End Sub
    Private Sub GetSelectedQueueTimes()
        Try
            Dim newQueue As New EveHQ.Core.SkillQueue
            newQueue.Name = "tempMerge"
            newQueue.IncCurrentTraining = True
            newQueue.Primary = False
            newQueue.Queue = New Collection
            For Each item As ListViewItem In lvQueues.SelectedItems
                Dim queueName As String = item.Name
                Dim oldQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(queueName), EveHQ.Core.SkillQueue)
                If oldQueue.Primary = True Then newQueue.Primary = True
                For Each queueItem As EveHQ.Core.SkillQueueItem In oldQueue.Queue
                    Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                    If newQueue.Queue.Contains(keyName) = False Then
                        newQueue.Queue.Add(queueItem, keyName)
                    End If
                Next
            Next
            Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            For Each curSkill In newQueue.Queue
                If displayPilot.PilotSkills.Contains(curSkill.Name) Then
                    Dim fromLevel As Integer = curSkill.FromLevel
                    Dim toLevel As Integer = curSkill.ToLevel
                    Dim mySkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(curSkill.Name), Core.PilotSkill)
                    Dim pilotLevel As Integer = mySkill.Level
                    If pilotLevel >= toLevel Then
                        Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                        newQueue.Queue.Remove(oldKey)
                    End If
                End If
            Next
            Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(displayPilot, newQueue)
            Dim qItem As EveHQ.Core.SortedQueue = New EveHQ.Core.SortedQueue
            Dim QTime As Double = 0
            For Each qItem In arrQueue
                QTime += CLng(qItem.TrainTime)
            Next
            Me.selQTime = QTime - displayPilot.TrainingCurrentTime
            lblTotalQueueTime.Text = "Selected Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(Me.selQTime + displayPilot.TrainingCurrentTime) & " (" & EveHQ.Core.SkillFunctions.TimeToString(Me.selQTime) & ")"
        Catch e As Exception
            MessageBox.Show("There was an error calculating the selected queue times.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
    Private Sub GetAllQueueTimes()
        Dim newQueue As New EveHQ.Core.SkillQueue
        newQueue.Name = "tempMerge"
        newQueue.IncCurrentTraining = True
        newQueue.Primary = False
        newQueue.Queue = New Collection
        For Each item As ListViewItem In lvQueues.Items
            Dim queueName As String = item.Name
            Dim oldQueue As EveHQ.Core.SkillQueue = CType(displayPilot.TrainingQueues(queueName), EveHQ.Core.SkillQueue)
            For Each queueItem As EveHQ.Core.SkillQueueItem In oldQueue.Queue
                Dim keyName As String = queueItem.Name & queueItem.FromLevel & queueItem.ToLevel
                If newQueue.Queue.Contains(keyName) = False Then
                    newQueue.Queue.Add(queueItem, keyName)
                End If
            Next
        Next
        Dim curSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
        For Each curSkill In newQueue.Queue
            If displayPilot.PilotSkills.Contains(curSkill.Name) Then
                Dim fromLevel As Integer = curSkill.FromLevel
                Dim toLevel As Integer = curSkill.ToLevel
                Dim mySkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(curSkill.Name), Core.PilotSkill)
                Dim pilotLevel As Integer = mySkill.Level
                If pilotLevel >= toLevel Then
                    Dim oldKey As String = curSkill.Name & curSkill.FromLevel & curSkill.ToLevel
                    newQueue.Queue.Remove(oldKey)
                End If
            End If
        Next
        Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(displayPilot, newQueue)
        Dim qItem As EveHQ.Core.SortedQueue = New EveHQ.Core.SortedQueue
        Dim QTime As Double = 0
        For Each qItem In arrQueue
            QTime += CLng(qItem.TrainTime)
        Next
        Me.totalQTime = QTime - displayPilot.TrainingCurrentTime
        lblTotalQueueTime.Text = "Total Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(Me.totalQTime + displayPilot.TrainingCurrentTime) & " (" & EveHQ.Core.SkillFunctions.TimeToString(Me.totalQTime) & ")"
    End Sub
    Private Sub lvQueues_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvQueues.Click
        Select Case lvQueues.SelectedItems.Count
            Case 0
                Call ResetQueueButtons()
                Me.selQTime = 0
                lblTotalQueueTime.Text = "No Queue Selected"
            Case 1
                btnAddQueue.Enabled = True
                btnEditQueue.Enabled = True
                btnDeleteQueue.Enabled = True
                btnCopyQueue.Enabled = True
                btnCopyToPilot.Enabled = True
                btnMergeQueues.Enabled = False
                btnSetPrimary.Enabled = True
                Me.selQTime = CDbl(Me.lvQueues.Items(Me.lvQueues.SelectedIndices(0)).SubItems(2).Tag) - displayPilot.TrainingCurrentTime
                lblTotalQueueTime.Text = "Selected Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(Me.selQTime + displayPilot.TrainingCurrentTime) & " (" & EveHQ.Core.SkillFunctions.TimeToString(Me.selQTime) & ")"
            Case Is > 1
                btnAddQueue.Enabled = True
                btnEditQueue.Enabled = False
                btnDeleteQueue.Enabled = False
                btnCopyQueue.Enabled = False
                btnCopyToPilot.Enabled = False
                btnMergeQueues.Enabled = True
                btnSetPrimary.Enabled = False
                Call Me.GetSelectedQueueTimes()
        End Select
    End Sub
    Private Sub lvQueues_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvQueues.DoubleClick
        Dim selQ As String = lvQueues.SelectedItems(0).Text
        Dim tp As TabPage = Me.tabQueues.TabPages(selQ)
        Me.tabQueues.SelectedTab = tp
    End Sub
    Public Sub ResetQueueButtons()
        btnAddQueue.Enabled = True
        btnEditQueue.Enabled = False
        btnDeleteQueue.Enabled = False
        btnCopyQueue.Enabled = False
        btnCopyToPilot.Enabled = False
        btnMergeQueues.Enabled = False
        btnSetPrimary.Enabled = False
    End Sub
#End Region

#Region "Skill Suggestion Functions"

    Private Sub MakeQueueSuggestions(ByVal objActQueue As Object)
        Dim ActQueue As EveHQ.Core.SkillQueue = CType(objActQueue, EveHQ.Core.SkillQueue)
        'If ActQueue.QueueSkills > 0 Then
        If displayPilot.TrainingQueues.ContainsKey(ActQueue.Name) = True Then
            Me.Invoke(SetSuggUIToCalc, New Object() {ActQueue.Name})
            Dim sugQueue As EveHQ.Core.SkillQueue = EveHQ.Core.SkillQueueFunctions.FindSuggestions(displayPilot, CType(ActQueue.Clone, Core.SkillQueue))
            If suggestedQueues.ContainsKey(ActQueue.Name) = False Then
                suggestedQueues.Add(ActQueue.Name, sugQueue)
            Else
                suggestedQueues(ActQueue.Name) = sugQueue
            End If
            Try
                Me.Invoke(SetSuggUIResult, New Object() {ActQueue.Name, ActQueue.QueueTime, sugQueue.QueueTime})
            Catch
                ' Window most likely closed during the suggestion calculation
            End Try
        Else
            ' Pilot changed before thread processing began therefore disregard the queue routine entirely
        End If
    End Sub
    Private Sub SetSuggestionUIToCalc(ByVal QueueName As String)
        If Me.tabQueues.Controls.ContainsKey(QueueName) = True Then
            Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(QueueName).Controls("TQ" & QueueName), TrainingQueue)
            Dim activePB As PictureBox = tq.newSuggestionPB
            Dim activeLabel As Label = tq.lblSuggestionLabel

            activePB.Image = My.Resources.info_grey
            activePB.Enabled = False
            activePB.Visible = True
            activeLabel.Text = "Calculating, Please Wait..."
            activeLabel.Visible = True
        End If
    End Sub
    Private Sub SetSuggestionUIResult(ByVal QueueName As String, ByVal ActQueueTime As Double, ByVal SugQueueTime As Double)
        If Me.tabQueues.Controls.ContainsKey(QueueName) = True Then
            Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(QueueName).Controls("TQ" & QueueName), TrainingQueue)
            Dim activePB As PictureBox = tq.newSuggestionPB
            Dim activeLabel As Label = tq.lblSuggestionLabel

            If SugQueueTime < ActQueueTime - 10 Then
                activePB.Image = My.Resources.info_icon
                activePB.Enabled = True
                activePB.Visible = True
                activeLabel.Text = "EveHQ can help decrease your training time, click the icon for more details..."
                activeLabel.Visible = True
            Else
                suggestedQueues.Remove(QueueName)
                activePB.Visible = False
                activeLabel.Visible = False
            End If
        End If
    End Sub
    Private Sub SuggestionIconClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim sugQueue As EveHQ.Core.SkillQueue = CType(suggestedQueues(activeQueueName), Core.SkillQueue)
        If sugQueue.QueueTime < activeQueue.QueueTime Then
            Dim newQueue As EveHQ.Core.SkillQueue = CType(sugQueue.Clone, Core.SkillQueue)
            Dim msg As String = ""
            msg &= "EveHQ can save you training time by adding learning skills to your current training queue." & ControlChars.CrLf
            msg &= "Training time can be reduced from " & EveHQ.Core.SkillFunctions.TimeToString(activeQueue.QueueTime) & ControlChars.CrLf
            msg &= " to " & EveHQ.Core.SkillFunctions.TimeToString(newQueue.QueueTime) & "." & ControlChars.CrLf
            msg &= "This is a saving of " & EveHQ.Core.SkillFunctions.TimeToString(activeQueue.QueueTime - newQueue.QueueTime) & "." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Would you like to add these skills to your training queue?" & ControlChars.CrLf
            Dim reply As Integer = MessageBox.Show(msg, "Time Savings Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If reply = Windows.Forms.DialogResult.Yes Then
                activeQueue = CType(newQueue.Clone, Core.SkillQueue)
                displayPilot.ActiveQueue = activeQueue
                displayPilot.TrainingQueues(activeQueue.Name) = activeQueue
                Call Me.RefreshTraining(activeQueueName)

                Dim tq As TrainingQueue = CType(Me.tabQueues.TabPages(activeQueueName).Controls("TQ" & activeQueueName), TrainingQueue)
                tq.newSuggestionPB.Visible = False
                tq.lblSuggestionLabel.Visible = False
            End If
        End If
    End Sub

#End Region

#Region "Skill Toolbar Functions"
    Private Sub btnShowDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowDetails.Click
        ' Find out which control it relates to!
        Dim skillName As String = ""
        Dim skillID As String = ""
        If activeLVW.Focused = False Then
            Dim curNode As TreeNode = New TreeNode
            curNode = tvwSkillList.SelectedNode
            If curNode IsNot Nothing Then
                skillName = curNode.Text
            End If
        Else
            skillName = activeLVW.SelectedItems(0).Text
        End If
        If skillName <> "" Then
            skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
            frmSkillDetails.DisplayPilotName = displayPilot.Name
            Call frmSkillDetails.ShowSkillDetails(skillID)
        End If
    End Sub
    Private Sub btnAddSkill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSkill.Click
        Call Me.AddSkillToQueueOption()
    End Sub
    Private Sub btnDeleteSkill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteSkill.Click
        Call Me.DeleteFromQueueOption()
    End Sub
    Private Sub btnLevelUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLevelUp.Click
        Call Me.IncreaseLevel()
    End Sub
    Private Sub btnLevelDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLevelDown.Click
        Call Me.DecreaseLevel()
    End Sub
    Private Sub btnMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveUp.Click
        Call Me.MoveUpQueue()
    End Sub
    Private Sub btnMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveDown.Click
        Call Me.MoveDownQueue()
    End Sub
    Private Sub btnClearQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearQueue.Click
        Call Me.ClearTrainingQueue()
    End Sub
    Private Sub btnICT_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnICT.CheckedChanged
        activeLVW.IncludeCurrentTraining = btnICT.Checked
        activeQueue.IncCurrentTraining = btnICT.Checked
        If activeQueue.Name IsNot Nothing Then
            RefreshTraining(activeQueue.Name)
        End If
    End Sub
    Private Sub tsbNeuralRemap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbNeuralRemap.Click
        If frmNeuralRemap.IsHandleCreated = True Then
            frmNeuralRemap.Select()
        Else
            frmNeuralRemap.PilotName = displayPilot.Name
            frmNeuralRemap.QueueName = activeQueueName
            frmNeuralRemap.Show()
        End If
    End Sub
    Private Sub tsbImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbImplants.Click
        If frmImplants.IsHandleCreated = True Then
            frmImplants.Select()
        Else
            frmImplants.PilotName = displayPilot.Name
            frmImplants.QueueName = activeQueueName
            frmImplants.Show()
        End If
    End Sub
#End Region

#Region "Skill Tree Context Menu Functions"
    Private Sub ctxDetails_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxDetails.Opening
        Dim curNode As TreeNode = New TreeNode
        curNode = tvwSkillList.SelectedNode
        If curNode IsNot Nothing Then
            Dim skillName As String = ""
            Dim skillID As String = ""
            skillName = curNode.Text
            skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
            mnuSkillName2.Text = skillName
            mnuSkillName2.Tag = skillID
            ' Determine if this is a parent node or not
            If curNode.Parent Is Nothing Then
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
                If displayPilot.PilotSkills.Contains(skillName) = True Then
                    Dim cSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(skillName), Core.PilotSkill)
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
        Dim skillID As String
        skillID = mnuSkillName2.Tag.ToString
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuForceTraining2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuForceTraining2.Click
        Dim skillID As String
        skillID = mnuSkillName2.Tag.ToString
        If EveHQ.Core.SkillFunctions.ForceSkillTraining(displayPilot, skillID, False) = True Then
            Call frmPilot.UpdatePilotInfo()
            Call Me.LoadSkillTree()
        End If
    End Sub
    Private Sub mnuAddToQueueNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueueNext.Click
        Call Me.AddSkillToQueueOption()
    End Sub
    Private Sub mnuAddToQueue1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue1.Click
        activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 1)
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuAddToQueue2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue2.Click
        activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 2)
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuAddToQueue3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue3.Click
        activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 3)
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuAddToQueue4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue4.Click
        activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 4)
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuAddToQueue5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToQueue5.Click
        activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, tvwSkillList.SelectedNode.Text, activeQueue.Queue.Count + 1, activeQueue, 5)
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuAddGroupToQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddGroupLevel1.Click, mnuAddGroupLevel2.Click, mnuAddGroupLevel3.Click, mnuAddGroupLevel4.Click, mnuAddGroupLevel5.Click
        Dim menu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim level As Integer = CInt(menu.Text.Replace("To Level ", ""))
        Dim parentNode As New TreeNode
        Dim curNode As New TreeNode
        parentNode = tvwSkillList.SelectedNode
        For Each curNode In parentNode.Nodes
            activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, curNode.Text, activeQueue.Queue.Count + 1, activeQueue, level, True, True)
        Next
        Call Me.RefreshTraining(activeQueueName)
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
        Dim fromLevel As String = activeLVW.SelectedItems(0).SubItems(2).Text
        Dim skillName As String = mnuSkillName.Text
        Dim currentLevel As Integer = 0
        If displayPilot.PilotSkills.Contains(skillName) = True Then
            Dim cSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(skillName), Core.PilotSkill)
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
            Dim skillName As String = activeLVW.SelectedItems(selItem).Text
            Dim fromLevel As String = activeLVW.SelectedItems(selItem).SubItems(2).Text
            Dim toLevel As String = activeLVW.SelectedItems(selItem).SubItems(3).Text
            Dim keyName As String = skillName & fromLevel & toLevel

            If activeQueue.Queue.Contains(keyName) = True Then
                ' Remove it from the queue
                Dim mySkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                mySkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
                Dim mySkillPos As Integer = mySkill.Pos - 1
                Call Me.DeleteFromQueue(mySkill)

                ' Add all the sublevels
                For level As Integer = CInt(fromLevel) To CInt(toLevel) - 1
                    mySkillPos += 1
                    activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos, activeQueue, level + 1)
                Next
            End If
        Next selItem
        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuSeparateTopLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSeparateTopLevel.Click
        For selItem As Integer = 0 To activeLVW.SelectedItems.Count - 1
            Dim skillName As String = activeLVW.SelectedItems(selItem).Text
            Dim fromLevel As String = activeLVW.SelectedItems(selItem).SubItems(2).Text
            Dim toLevel As String = activeLVW.SelectedItems(selItem).SubItems(3).Text
            Dim keyName As String = skillName & fromLevel & toLevel

            ' Remove it from the queue
            Dim mySkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            mySkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
            Dim mySkillPos As Integer = mySkill.Pos
            Call Me.DeleteFromQueue(mySkill)

            ' Add the new levels, 
            activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos, activeQueue, CInt(toLevel) - 1)
            activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos + 1, activeQueue, CInt(toLevel))
        Next selItem

        Call Me.RefreshTraining(activeQueueName)
    End Sub
    Private Sub mnuSeparateBottomLevel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSeparateBottomLevel.Click
        For selItem As Integer = 0 To activeLVW.SelectedItems.Count - 1
            Dim skillName As String = activeLVW.SelectedItems(selItem).Text
            Dim fromLevel As String = activeLVW.SelectedItems(selItem).SubItems(2).Text
            Dim toLevel As String = activeLVW.SelectedItems(selItem).SubItems(3).Text
            Dim keyName As String = skillName & fromLevel & toLevel

            ' Remove it from the queue
            Dim mySkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
            mySkill = CType(activeQueue.Queue(keyName), Core.SkillQueueItem)
            Dim mySkillPos As Integer = mySkill.Pos
            Call Me.DeleteFromQueue(mySkill)

            ' Add the new levels, 
            activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos, activeQueue, CInt(fromLevel) + 1)
            activeQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, mySkillPos + 1, activeQueue, CInt(toLevel))
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
            Call EveHQ.Core.SkillQueueFunctions.RemoveTrainedSkills(displayPilot, activeQueue)
            ' Refresh the training view!
            Call Me.RefreshTraining(activeQueueName)
        End If
    End Sub
    Private Sub mnuClearTrainingQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClearTrainingQueue.Click
        Call Me.ClearTrainingQueue()
    End Sub
    Private Sub mnuViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewDetails.Click
        Dim skillID As String
        skillID = mnuSkillName.Tag.ToString
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuForceTraining_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuForceTraining.Click
        Dim skillID As String
        skillID = mnuSkillName.Tag.ToString
        If EveHQ.Core.SkillFunctions.ForceSkillTraining(displayPilot, skillID, False) = True Then
            Call frmPilot.UpdatePilotInfo()
            Call Me.LoadSkillTree()
        End If
    End Sub
#End Region

#Region "Skill Info Context Menu Functions"
    Private Sub ctxReqs_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxReqs.Opening
        Dim curNode As TreeNode = New TreeNode
        curNode = tvwReqs.SelectedNode
        Dim skillName As String = ""
        Dim skillID As String = ""
        skillName = curNode.Text
        If InStr(skillName, "(Level") <> 0 Then
            skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
        End If
        skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
        mnuReqsSkillName.Text = skillName
        mnuReqsSkillName.Tag = skillID
    End Sub
    Private Sub mnuViewItemDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetails.Click
        Dim skillID As String = mnuItemName.Tag.ToString
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuViewItemDetailsHere_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetailsHere.Click
        Dim skillID As String = mnuItemName.Tag.ToString
        Call Me.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuReqsViewDetailsHere_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReqsViewDetailsHere.Click
        Dim skillID As String = mnuReqsSkillName.Tag.ToString
        Call Me.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuReqsViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReqsViewDetails.Click
        Dim skillID As String = mnuReqsSkillName.Tag.ToString
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub
    Private Sub mnuViewItemDetailsInCertScreen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewItemDetailsInCertScreen.Click
        Dim certID As String = mnuItemName.Tag.ToString
        frmCertificateDetails.DisplayPilotName = displayPilot.Name
        frmCertificateDetails.ShowCertDetails(certID)
    End Sub
#End Region
End Class