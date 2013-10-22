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
'=========================================================================
Imports DevComponents.AdvTree
Imports EveHQ.Forms
Imports DevComponents.DotNetBar

Namespace Controls

    Public Class EveHQTrainingQueue

        Public Event QueueUpdated()

#Region "Class Variables"

        Dim _queuePilotName As String
        Dim _queuePilot As Core.EveHQPilot
        Dim _queueName As String
        Dim _queue As Core.EveHQSkillQueue

        ReadOnly _startup As Boolean

#End Region

#Region "Properties"

        Public Property QueuePilotName As String
            Get
                Return _queuePilotName
            End Get
            Set(value As String)
                _queuePilotName = value
                _queuePilot = Core.HQ.Settings.Pilots(_queuePilotName)
            End Set
        End Property

        Public Property QueueName As String
            Get
                Return _queueName
            End Get
            Set(value As String)
                _queueName = value
                _queue = _queuePilot.TrainingQueues(_queueName)
            End Set
        End Property

        Public Property IncludesCurrentTraining As Boolean
            Get
                Return _queue.IncCurrentTraining
            End Get
            Set(value As Boolean)
                _queue.IncCurrentTraining = value
            End Set
        End Property

#End Region

#Region "Constructors and Destructors"

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

        End Sub

        Public Sub New(pilotName As String, skillQueueName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Set the colour of the drag drop marker
            DirectCast(DevComponents.DotNetBar.Rendering.GlobalManager.Renderer, DevComponents.DotNetBar.Rendering.Office2007Renderer).ColorTable.AdvTree.DragDropMarker = Color.Red

            _startup = True

            ' Add any initialization after the InitializeComponent() call.
            QueuePilotName = pilotName
            QueueName = skillQueueName

            chkShowCompletedSkills.Checked = _queue.ShowCompletedSkills

            _startup = False

        End Sub

        Private _oldParent As Control

        Private Sub TrainingQueue_ParentChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.ParentChanged
            If _oldParent IsNot Nothing Then
                RemoveHandler _oldParent.Disposed, AddressOf Parent_Disposed
            End If

            If Parent IsNot Nothing Then
                AddHandler Parent.Disposed, AddressOf Parent_Disposed
            End If

            _oldParent = Parent
        End Sub

        Private Sub Parent_Disposed(ByVal sender As Object, ByVal e As EventArgs)
            Dispose()
        End Sub

#End Region

#Region "Queue Drawing Routines"

        Public Sub DrawColumnHeadings()

            adtQueue.Columns.Clear()
            ' Add the standard column

            Dim ch As New ColumnHeader("Skill Name")
            ch.SortingEnabled = False
            ch.Name = "Name" : ch.Width.Absolute = 180
            adtQueue.Columns.Add(ch)

            ' Add subitems based on the user selected columns
            Dim colName As String
            For Each col As String In Core.HQ.Settings.UserQueueColumns
                If col.EndsWith("1", StringComparison.Ordinal) = True Then
                    colName = col.Substring(0, col.Length - 1)
                    ' Define a new column header
                    Dim ach As New ColumnHeader
                    ach.SortingEnabled = False
                    ach.Name = colName : ach.Width.Absolute = 100
                    Select Case colName
                        Case "Current"
                            ach.Text = "Cur Lvl" : ach.Width.Absolute = 60
                        Case "From"
                            ach.Text = "From Lvl" : ach.Width.Absolute = 60
                        Case "To"
                            ach.Text = "To Lvl" : ach.Width.Absolute = 60
                        Case "Percent"
                            ach.Text = "%" : ach.Width.Absolute = 30
                        Case "TrainTime"
                            ach.Text = "Training Time" : ach.Width.Absolute = 125
                        Case "TimeToComplete"
                            ach.Text = "Time to Complete" : ach.Width.Absolute = 125
                        Case "DateEnded"
                            ach.Text = "Date Completed" : ach.Width.Absolute = 150
                        Case "Rank"
                            ach.Text = "Rank" : ach.Width.Absolute = 50
                        Case "PAtt"
                            ach.Text = "Pri Att" : ach.Width.Absolute = 80
                        Case "SAtt"
                            ach.Text = "Sec Att" : ach.Width.Absolute = 80
                        Case "SPHour"
                            ach.Text = "SP /hour" : ach.Width.Absolute = 80
                        Case "SPDay"
                            ach.Text = "SP /day" : ach.Width.Absolute = 80
                        Case "SPWeek"
                            ach.Text = "SP /week" : ach.Width.Absolute = 80
                        Case "SPMonth"
                            ach.Text = "SP /mnth" : ach.Width.Absolute = 80
                        Case "SPYear"
                            ach.Text = "SP /year" : ach.Width.Absolute = 80
                        Case "SPAdded"
                            ach.Text = "SP Added" : ach.Width.Absolute = 100
                        Case "SPTotal"
                            ach.Text = "SP @ End" : ach.Width.Absolute = 100
                        Case "Notes"
                            ach.Text = "Notes" : ach.Width.Absolute = 200
                        Case "Priority"
                            ach.Text = "Priority" : ach.Width.Absolute = 50
                    End Select
                    adtQueue.Columns.Add(ach)
                End If
            Next
        End Sub

        Public Sub DrawQueue()

            If _queuePilot.PilotSkills.Count <> 0 Then

                ' Define node background colours
                Dim styleCurrentTraining As ElementStyle = adtQueue.Styles(0).Copy
                Dim styleIsPreReq As ElementStyle = adtQueue.Styles(0).Copy
                Dim styleHasPreReq As ElementStyle = adtQueue.Styles(0).Copy
                Dim styleBothPreReq As ElementStyle = adtQueue.Styles(0).Copy
                Dim stylePartialTraining As ElementStyle = adtQueue.Styles(0).Copy
                Dim styleReadySkill As ElementStyle = adtQueue.Styles(0).Copy

                styleCurrentTraining.BackColor = Color.LimeGreen
                styleIsPreReq.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.IsPreReqColor))
                styleHasPreReq.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.HasPreReqColor))
                styleBothPreReq.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.BothPreReqColor))
                stylePartialTraining.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.PartialTrainColor))
                styleReadySkill.BackColor = Color.FromArgb(CInt(Core.HQ.Settings.ReadySkillColor))

                ' Save the selected items
                'For Each selNode As Node In adtQueue.SelectedNodes
                '    _selectedNodes.Add(selNode)
                'Next

                ' Clear the visible training queue

                ' Remember the first visible item?
                'Dim FVI As Integer = -1
                'If lvwQueue.TopItem IsNot Nothing Then
                '    FVI = lvwQueue.TopItem.Index
                'End If

                ' Set the queue up for redrawing
                adtQueue.SuspendLayout()
                adtQueue.BeginUpdate()
                adtQueue.Nodes.Clear()

                ' Call the main procedure
                Dim aq As Core.EveHQSkillQueue = _queuePilot.TrainingQueues(_queueName)
                Dim sortedQueue As ArrayList = Core.SkillQueueFunctions.BuildQueue(_queuePilot, aq, False, True)
                'If sortedQueues.ContainsKey(QueueName) = True Then
                '    sortedQueues(QueueName) = sortedQueue
                'Else
                '    sortedQueues.Add(QueueName, sortedQueue)
                'End If
                Dim qItem As Core.SortedQueueItem
                Dim totalTime As Long = 0
                Dim totalSP As Long = _queuePilot.SkillPoints

                ' Create the columns according to the selection in the settings
                Call DrawColumnHeadings()

                If sortedQueue IsNot Nothing Then
                    For Each qItem In sortedQueue
                        Dim newskill As New Node
                        newskill.Name = qItem.Key
                        newskill.Image = My.Resources.SkillBook16
                        If qItem.Done = False Or (qItem.Done = True And _queue.ShowCompletedSkills = True) Then
                            newskill.Visible = True
                        Else
                            newskill.Visible = False
                        End If
                        If qItem.Done = True Then newskill.Style = adtQueue.Styles("SkillCompleted")
                        If qItem.IsPrereq = True Then
                            If qItem.HasPrereq = True Then
                                newskill.Tooltip &= qItem.Prereq & ControlChars.CrLf & qItem.Reqs
                                newskill.Style = styleBothPreReq
                                Else
                                newskill.Tooltip = qItem.Prereq
                                newskill.Style = styleIsPreReq
                            End If
                        Else
                            If qItem.HasPrereq = True Then
                                newskill.Tooltip = qItem.Reqs
                                newskill.Style = styleHasPreReq
                            Else
                                If qItem.PartTrained = True Then
                                    newskill.Style = stylePartialTraining
                                Else
                                    newskill.Style = styleReadySkill
                                End If
                            End If
                        End If
                        If qItem.IsTraining = True Then
                            newskill.DragDropEnabled = False
                            newskill.Style = styleCurrentTraining
                            ' Set a flag in the listview of the listviewitem name for later checking
                            adtQueue.Tag = newskill.Name
                        End If
                        ' Do some additional calcs
                        totalSP += CLng(qItem.SPTrained)
                        totalTime += CLng(qItem.TrainTime)
                        newskill.Text = qItem.Name
                        newskill.Tag = qItem.ID
                        Call AddUserColumns(newskill, qItem, totalSP)
                        adtQueue.Nodes.Add(newskill)
                    Next
                End If

                lblQueueTime.Tag = totalTime.ToString
                lblQueueTime.Text = Core.SkillFunctions.TimeToString(totalTime)

                CountVisibleSkills()

                ' Select the old selected items
                'For Each selNode As Node In _selectedNodes
                '    If adtQueue.Nodes.Contains(selNode) Then
                '        adtQueue.Nodes(selItem.Name).Selected = True
                '        adtQueue.Nodes(selItem.Name).Focused = True
                '    End If
                'Next

                ' Tidy up afterwards
                adtQueue.RecalcLayout()
                adtQueue.EndUpdate()
                adtQueue.ResumeLayout()

                ' Restore the first visible item
                'If FVI >= 0 And lvwQueue.Items.Count > 0 And FVI < lvwQueue.Items.Count Then
                '    adtQueue.TopItem = lvwQueue.Items(FVI)
                '    lvwQueue.TopItem = lvwQueue.Items(FVI)
                'End If

                Call Core.SkillQueueFunctions.TidyQueue(_queuePilot, aq, sortedQueue)

                RaiseEvent QueueUpdated()

            End If
        End Sub

        Private Sub AddUserColumns(ByVal newskill As Node, ByVal qitem As Core.SortedQueueItem, ByVal totalSP As Long)
            ' Add subitems based on the user selected columns
            Dim colName As String
            For Each col As String In Core.HQ.Settings.UserQueueColumns
                If col.EndsWith("1", StringComparison.Ordinal) = True Then
                    colName = col.Substring(0, col.Length - 1)
                    Dim newSi As New Cell
                    Select Case colName
                        Case "Current"
                            If (qitem.IsInjected) Then
                                newSi.Name = qitem.CurLevel.ToString
                                newSi.Text = qitem.CurLevel.ToString
                            Else
                                newSi.Name = ""
                                newSi.Text = ""
                            End If
                        Case "From"
                            newSi.Name = qitem.FromLevel.ToString
                            newSi.Text = qitem.FromLevel.ToString
                        Case "To"
                            newSi.Name = qitem.ToLevel.ToString
                            newSi.Text = qitem.ToLevel.ToString
                        Case "Percent"
                            Dim skillPct As Double
                            If _queuePilot.PilotSkills.ContainsKey(qitem.Name) Then
                                Dim myCurSkill As Core.EveHQPilotSkill = _queuePilot.PilotSkills(qitem.Name)
                                Dim baseSkill As Core.EveSkill = Core.HQ.SkillListID(myCurSkill.ID)
                                Dim clevel As Integer = CInt(qitem.FromLevel)
                                Dim nextLevelSp As Integer = baseSkill.LevelUp(clevel + 1) - baseSkill.LevelUp(clevel)

                                If clevel <> myCurSkill.Level Then
                                    skillPct = 0
                                Else
                                    If qitem.Name = _queuePilot.TrainingSkillName Then
                                        skillPct = CInt(Int((myCurSkill.SP + _queuePilot.TrainingCurrentSP - baseSkill.LevelUp(clevel)) / nextLevelSp * 100))
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
                            newSi.Name = "Percent"
                            newSi.Text = CStr(skillPct)
                        Case "TrainTime"
                            newSi.Name = "TrainTime"
                            newSi.Text = Core.SkillFunctions.TimeToString(CDbl(qitem.TrainTime))
                            newSi.Tag = qitem.TrainTime
                        Case "TimeToComplete"
                            newSi.Name = "TimeToComplete"
                            newSi.Text = Core.SkillFunctions.TimeToString(CDbl(qitem.TimeBeforeTrained))
                            newSi.Tag = qitem.TimeBeforeTrained
                        Case "DateEnded"
                            newSi.Name = qitem.DateFinished.ToBinary.ToString
                            newSi.Text = Format(qitem.DateFinished, "ddd") & " " & qitem.DateFinished.ToString
                        Case "Rank"
                            newSi.Name = qitem.Rank.ToString
                            newSi.Text = qitem.Rank.ToString
                        Case "PAtt"
                            newSi.Name = qitem.PAtt
                            newSi.Text = qitem.PAtt
                        Case "SAtt"
                            newSi.Name = qitem.SAtt
                            newSi.Text = qitem.SAtt
                        Case "SPHour"
                            newSi.Name = qitem.SPRate.ToString
                            newSi.Text = qitem.SPRate.ToString("N0")
                        Case "SPDay"
                            newSi.Name = CStr(CDbl(qitem.SPRate) * 24)
                            newSi.Text = (qitem.SPRate * 24).ToString("N0")
                        Case "SPWeek"
                            newSi.Name = CStr(CDbl(qitem.SPRate) * 24 * 7)
                            newSi.Text = (qitem.SPRate * 24 * 7).ToString("N0")
                        Case "SPMonth"
                            newSi.Name = CStr(CDbl(qitem.SPRate) * 24 * 30)
                            newSi.Text = (qitem.SPRate * 24 * 30).ToString("N0")
                        Case "SPYear"
                            newSi.Name = CStr(CDbl(qitem.SPRate) * 24 * 365)
                            newSi.Text = (qitem.SPRate * 24 * 365).ToString("N0")
                        Case "SPAdded"
                            newSi.Name = qitem.SPTrained.ToString
                            newSi.Text = qitem.SPTrained.ToString("N0")
                        Case "SPTotal"
                            newSi.Name = CStr(totalSP)
                            newSi.Text = totalSP.ToString("N0")
                        Case "Notes"
                            newSi.Name = "Notes"
                            newSi.Text = qitem.Notes
                        Case "Priority"
                            newSi.Name = "Priority"
                            newSi.Text = qitem.Priority.ToString("N0")
                    End Select
                    newskill.Cells.Add(newSi)
                End If
            Next
        End Sub

        Private Sub CountVisibleSkills()
            Dim count As Integer = 0
            For Each n As Node In adtQueue.Nodes
                If n.IsVisible Then count += 1
            Next
            lblSkillCount.Text = count.ToString("N0")
        End Sub

        Private Sub ReOrderSkillQueue()

            For Each n As Node In adtQueue.Nodes
                Dim keyName As String = n.Name
                If _queue.IncCurrentTraining = True And _queuePilot.Training = True Then
                    If n.Index = 0 Then
                        Continue For
                    Else
                        _queue.Queue(keyName).Pos = n.Index
                    End If
                Else
                    _queue.Queue(keyName).Pos = n.Index + 1
                End If
            Next

        End Sub

#End Region

#Region "UI Routines"

        Private Sub chkShowCompletedSkills_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowCompletedSkills.CheckedChanged
            _queue.ShowCompletedSkills = chkShowCompletedSkills.Checked
            If _startup = False Then
                Call DrawQueue()
            End If
        End Sub

        Private Sub adtQueue_AfterNodeDrop(sender As Object, e As TreeDragDropEventArgs) Handles adtQueue.AfterNodeDrop
            If e.IsCopy = False Then
                ' Drag/Drop is within the same control
                ReOrderSkillQueue()
                DrawQueue()
            End If
        End Sub

        Private Sub adtQueue_BeforeNodeDrop(sender As Object, e As TreeDragDropEventArgs) Handles adtQueue.BeforeNodeDrop
            ' Determine if the node being dropped comes from the same tree
            ' Move the node if from the same tree, or copy it if it comes from another
            If e.Node.TreeControl.Name = adtQueue.Name Then
                ' Drag/Drop is within the same control
                e.IsCopy = False
            Else
                ' Source node is from another control
                e.IsCopy = True
                If _queuePilot.Training = True And _queue.IncCurrentTraining = True Then
                    _queue = Core.SkillQueueFunctions.AddSkillToQueue(_queuePilot, e.Node.Text, e.InsertPosition + 1, _queue, 0, False, False, "")
                Else
                    _queue = Core.SkillQueueFunctions.AddSkillToQueue(_queuePilot, e.Node.Text, e.InsertPosition + 2, _queue, 0, False, False, "")
                End If
                ' We don't want to physically drop the node into the queue - this will be done via the DrawQueue method
                e.Cancel = True
                ' Rebuild the queue now
                Call ReOrderSkillQueue()
                Call DrawQueue()
            End If
        End Sub

        Private Sub adtQueue_DragDrop(sender As Object, e As DragEventArgs) Handles adtQueue.DragDrop

            ' We don't need to add code to handle the final drag/drop
            ' All required code is handled in the BeforeNodeDrop and AfterNodeDrop methods

        End Sub

        Private Sub adtQueue_NodeDragFeedback(sender As Object, e As TreeDragFeedbackEventArgs) Handles adtQueue.NodeDragFeedback
            If e.DragNode.TagString = "#" Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.Move
            End If
        End Sub

#End Region

#Region "Context Menu Routines"

        Public Sub RedrawMenuOptions()
            ' Determines what buttons and menus are available from the listview!
            ' Check the queue status
            If adtQueue.SelectedNodes.Count <> 0 Then
                Select Case adtQueue.SelectedNodes.Count
                    Case 1
                        Dim skillKey As String = adtQueue.SelectedNodes(0).Name
                        Dim skillName As String
                        Dim curFLevel As Integer = CInt(skillKey.Substring(skillKey.Length - 2, 1))
                        Dim curTLevel As Integer = CInt(skillKey.Substring(skillKey.Length - 1, 1))
                        Dim skillID As Integer
                        skillName = adtQueue.SelectedNodes(0).Text
                        skillID = Core.SkillFunctions.SkillNameToID(skillName)
                        mnuSkillName.Text = skillName
                        mnuSkillName.Tag = skillID
                        ' Check if we can increase or decrease levels

                        Dim curLevel As Integer

                        Dim mySkill As Core.EveHQPilotSkill
                        If _queuePilot.PilotSkills.ContainsKey(skillName) = False Then
                            curLevel = 0
                        Else
                            mySkill = _queuePilot.PilotSkills(skillName)
                            curLevel = mySkill.Level
                        End If
                        mnuIncreaseLevel.Enabled = True
                        mnuDecreaseLevel.Enabled = True
                        mnuDeleteFromQueue.Enabled = True
                        mnuMoveUpQueue.Enabled = True
                        mnuMoveDownQueue.Enabled = True
                        mnuViewDetails.Enabled = True
                        If curTLevel = 5 Or curLevel = 5 Then
                            mnuIncreaseLevel.Enabled = False
                        End If
                        If curTLevel - 1 <= curFLevel Or curTLevel <= curLevel Then
                            mnuDecreaseLevel.Enabled = False
                        End If
                        If adtQueue.SelectedNodes(0).Index = 0 Then
                            mnuMoveUpQueue.Enabled = False
                        End If

                        ' Check if the skill is a pre-req
                        If adtQueue.SelectedNodes(0).Style IsNot Nothing Then
                            If adtQueue.SelectedNodes(0).Style.BackColor = Color.LightSteelBlue Then
                                If adtQueue.SelectedNodes(0).Cells(4).Text = "100" Then
                                    mnuDeleteFromQueue.Enabled = True
                                Else
                                    mnuDeleteFromQueue.Enabled = False
                                End If
                            End If
                        End If
                        ' Check if the skill is at the bottom of the list
                        If adtQueue.SelectedNodes(0).Index = adtQueue.Nodes.Count - 1 Then
                            mnuMoveDownQueue.Enabled = False
                        End If

                        ' Check if there are multiple levels trained on the skill
                        Select Case curTLevel - curFLevel
                            Case 1
                                mnuSeparateAllLevels.Enabled = False
                                mnuSeparateBottomLevel.Enabled = False
                                mnuSeparateTopLevel.Enabled = False
                                mnuSeperateLevelSep.Visible = True
                            Case 2
                                mnuSeparateAllLevels.Enabled = True
                                mnuSeparateBottomLevel.Enabled = False
                                mnuSeparateTopLevel.Enabled = False
                                mnuSeperateLevelSep.Visible = True
                            Case Is > 2
                                mnuSeparateAllLevels.Enabled = True
                                mnuSeparateBottomLevel.Enabled = True
                                mnuSeparateTopLevel.Enabled = True
                                mnuSeperateLevelSep.Visible = True
                        End Select
                        If mnuSeparateAllLevels.Enabled = False And mnuSeparateBottomLevel.Enabled = False And mnuSeparateTopLevel.Enabled = False Then
                            mnuSeparateLevels.Enabled = False
                        Else
                            mnuSeparateLevels.Enabled = True
                        End If

                        ' Adjust for if the training skill
                        If _queuePilot.Training = True And IncludesCurrentTraining = True Then
                            If adtQueue.SelectedNodes(0).Index = 0 Then
                                mnuIncreaseLevel.Enabled = False
                                mnuDecreaseLevel.Enabled = False
                                mnuDeleteFromQueue.Enabled = False
                                mnuMoveUpQueue.Enabled = False
                                mnuMoveDownQueue.Enabled = False
                            Else
                                If adtQueue.SelectedNodes(0).Index = 1 Then
                                    mnuMoveUpQueue.Enabled = False
                                End If
                            End If
                        End If
                    Case Is > 1
                        mnuIncreaseLevel.Enabled = False
                        mnuDecreaseLevel.Enabled = False
                        mnuDeleteFromQueue.Enabled = True
                        mnuMoveUpQueue.Enabled = False
                        mnuMoveDownQueue.Enabled = False
                        mnuViewDetails.Enabled = False
                        mnuSeparateLevels.Enabled = True
                        mnuSeparateAllLevels.Enabled = True
                        mnuSeparateBottomLevel.Enabled = False
                        mnuSeparateTopLevel.Enabled = False
                        mnuSeperateLevelSep.Visible = True
                End Select
            End If
        End Sub

        Private Sub ctxQueue_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxQueue.Opening
            ' Cancel the menu opening if there are no skills selected
            If adtQueue.SelectedNodes.Count = 0 Then
                e.Cancel = True
                Exit Sub
            End If
            Call RedrawMenuOptions()
            ' Determine enabled menu items of adding to queue
            Dim keyName As String = adtQueue.SelectedNodes(0).Name
            Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
            Dim skillName As String = mnuSkillName.Text
            Dim currentLevel As Integer = 0
            If _queuePilot.PilotSkills.ContainsKey(skillName) = True Then
                Dim cSkill As Core.EveHQPilotSkill = _queuePilot.PilotSkills(skillName)
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
            If _queue.Queue.ContainsKey(keyName) = False Then
                mnuEditNote.Enabled = False
            Else
                mnuEditNote.Enabled = True
            End If
        End Sub

        Private Sub mnuChangeLevel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuChangeLevel1.Click
            Call ChangeLevel(1)
        End Sub
        Private Sub mnuChangeLevel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuChangeLevel2.Click
            Call ChangeLevel(2)
        End Sub
        Private Sub mnuChangeLevel3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuChangeLevel3.Click
            Call ChangeLevel(3)
        End Sub
        Private Sub mnuChangeLevel4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuChangeLevel4.Click
            Call ChangeLevel(4)
        End Sub
        Private Sub mnuChangeLevel5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuChangeLevel5.Click
            Call ChangeLevel(5)
        End Sub
        Private Sub mnuIncreaseLevel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuIncreaseLevel.Click
            Call IncreaseLevel()
        End Sub
        Private Sub mnuDecreaseLevel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuDecreaseLevel.Click
            Call DecreaseLevel()
        End Sub
        Private Sub mnuMoveUpQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuMoveUpQueue.Click
            Call MoveUpQueue()
        End Sub
        Private Sub mnuMoveDownQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuMoveDownQueue.Click
            Call MoveDownQueue()
        End Sub
        Private Sub mnuSeparateAllLevels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSeparateAllLevels.Click
            For selItem As Integer = 0 To adtQueue.SelectedNodes.Count - 1
                Dim keyName As String = adtQueue.SelectedNodes(selItem).Name
                Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
                Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
                Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))
                If _queue.Queue.ContainsKey(keyName) = True Then
                    ' Remove it from the queue
                    Dim mySkill As Core.EveHQSkillQueueItem
                    mySkill = _queue.Queue(keyName)
                    Dim mySkillPos As Integer = mySkill.Pos - 1
                    Call DeleteFromQueue(mySkill)
                    ' Add all the sublevels
                    For level As Integer = CInt(fromLevel) To CInt(toLevel) - 1
                        mySkillPos += 1
                        _queue = Core.SkillQueueFunctions.AddSkillToQueue(_queuePilot, skillName, mySkillPos, _queue, level + 1, False, False, "")
                    Next
                End If
            Next selItem
            Call DrawQueue()
        End Sub
        Private Sub mnuSeparateTopLevel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSeparateTopLevel.Click
            For selItem As Integer = 0 To adtQueue.SelectedNodes.Count - 1
                Dim keyName As String = adtQueue.SelectedNodes(selItem).Name
                Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
                Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))

                ' Remove it from the queue
                Dim mySkill As Core.EveHQSkillQueueItem
                mySkill = _queue.Queue(keyName)
                Dim mySkillPos As Integer = mySkill.Pos
                Call DeleteFromQueue(mySkill)

                ' Add the new levels, 
                _queue = Core.SkillQueueFunctions.AddSkillToQueue(_queuePilot, skillName, mySkillPos, _queue, CInt(toLevel) - 1, False, False, "")
                _queue = Core.SkillQueueFunctions.AddSkillToQueue(_queuePilot, skillName, mySkillPos + 1, _queue, CInt(toLevel), False, False, "")
            Next selItem

            Call DrawQueue()
        End Sub
        Private Sub mnuSeparateBottomLevel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSeparateBottomLevel.Click
            For selItem As Integer = 0 To adtQueue.SelectedNodes.Count - 1
                Dim keyName As String = adtQueue.SelectedNodes(selItem).Name
                Dim skillName As String = CStr(keyName.Substring(0, keyName.Length - 2))
                Dim fromLevel As Integer = CInt(keyName.Substring(keyName.Length - 2, 1))
                Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))

                ' Remove it from the queue
                Dim mySkill As Core.EveHQSkillQueueItem
                mySkill = _queue.Queue(keyName)
                Dim mySkillPos As Integer = mySkill.Pos
                Call DeleteFromQueue(mySkill)

                ' Add the new levels, 
                _queue = Core.SkillQueueFunctions.AddSkillToQueue(_queuePilot, skillName, mySkillPos, _queue, CInt(fromLevel) + 1, False, False, "")
                _queue = Core.SkillQueueFunctions.AddSkillToQueue(_queuePilot, skillName, mySkillPos + 1, _queue, CInt(toLevel), False, False, "")
            Next selItem
            Call DrawQueue()
        End Sub
        Private Sub mnuDeleteFromQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuDeleteFromQueue.Click
            Call DeleteFromQueueOption()
        End Sub
        Private Sub mnuRemoveTrainedSkills_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRemoveTrainedSkills.Click
            Dim reply As Integer
            reply = MessageBox.Show("Are you sure you want to remove trained skills from the queue?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                Call Core.SkillQueueFunctions.RemoveTrainedSkills(_queuePilot, _queue)
                ' Refresh the training view!
                Call DrawQueue()
            End If
        End Sub
        Private Sub mnuClearTrainingQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuClearTrainingQueue.Click
            Call ClearTrainingQueue()
            Call DrawQueue()
        End Sub
        Private Sub mnuViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewDetails.Click
            Dim skillID As Integer = CInt(mnuSkillName.Tag)
            FrmSkillDetails.DisplayPilotName = _queuePilot.Name
            Call FrmSkillDetails.ShowSkillDetails(skillID)
        End Sub
        Private Sub mnuSplitQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSplitQueue.Click
            Call SplitQueue()
            DrawQueue()
        End Sub
        Private Sub mnuEditNote_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuEditNote.Click
            ' Try to get the keys of the skill(s) we are changing
            If adtQueue.SelectedNodes.Count > 0 Then
                Dim keys As New List(Of String)
                For Each selItem As ListViewItem In adtQueue.SelectedNodes
                    keys.Add(selItem.Name)
                Next
                Dim noteForm As New FrmSkillNote
                If keys.Count > 1 Then
                    noteForm.lblDescription.Text = "Editing description for multiple queue entries..."
                    noteForm.Text = "Skill Note - Multiple Skills"
                Else
                    Dim curToLevel As String = CStr(keys(0)).Substring(CStr(keys(0)).Length - 1, 1)
                    Dim curSkillName As String = CStr(keys(0)).Substring(0, CStr(keys(0)).Length - 2)
                    noteForm.lblDescription.Text = "Editing description for " & curSkillName & " (Lvl " & curToLevel & ")"
                    noteForm.Text = "Skill Note - " & curSkillName & " (Lvl " & curToLevel & ")"
                End If
                noteForm.txtNotes.Text = _queue.Queue(keys(0)).Notes
                noteForm.txtNotes.SelectAll()
                noteForm.ShowDialog()
                If noteForm.DialogResult = DialogResult.OK Then
                    For Each key As String In keys
                        _queue.Queue(key).Notes = noteForm.txtNotes.Text
                    Next
                    Call DrawQueue()
                End If
            End If
        End Sub

#End Region

#Region "Skill Queue UI Functions"

        Private Sub ChangeLevel(ByVal selectedLevel As Integer)
            ' Store the index being used
            Dim oldIndex As Integer = adtQueue.SelectedNodes(0).Index
            ' Get the skill name
            Dim keyName As String = adtQueue.SelectedNodes(0).Name
            Dim toLevel As Integer = CInt(keyName.Substring(keyName.Length - 1, 1))
            Dim myTSkill As Core.EveHQSkillQueueItem
            If _queue.Queue.ContainsKey(keyName) = True Then
                myTSkill = _queue.Queue(keyName)
                If selectedLevel < CInt(toLevel) Then
                    ' Delete the existing item
                    _queue.Queue.Remove(keyName)
                    ' Adjust the "to" level
                    myTSkill.ToLevel = selectedLevel
                    ' Add the item back in at its new levels
                    _queue.Queue.Add(myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel, myTSkill)
                    Call DrawQueue()
                    adtQueue.SelectedNodes.Add(adtQueue.Nodes(oldIndex))
                    adtQueue.Nodes(oldIndex).EnsureVisible()
                End If
                If selectedLevel > CInt(toLevel) Then
                    ' Check if we have another skill that can be affected by us increasing the level i.e. the same skill!
                    Dim checkSkill As Core.EveHQSkillQueueItem
                    Dim idx As Integer = 0
                    Do
                        checkSkill = _queue.Queue(_queue.Queue.Keys(idx))

                        If myTSkill.Name = checkSkill.Name Then ' Matched skill name
                            ' Check the "from" skill level matches
                            If checkSkill.FromLevel >= myTSkill.ToLevel Then
                                ' We have to decide what to do here, either increase the levels or merge
                                If checkSkill.ToLevel <= selectedLevel Then
                                    ' We have to merge the items here so delete the new found one
                                    Call DeleteFromQueue(checkSkill)
                                    idx -= 1
                                Else
                                    ' We have to increase the levels so decrease the new found one
                                    ' (Only if the start skill is smaller than the selected skill)
                                    If checkSkill.FromLevel < selectedLevel Then
                                        ' Delete the existing item
                                        _queue.Queue.Remove(checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel)
                                        ' Adjust the "to" level
                                        checkSkill.FromLevel = selectedLevel
                                        ' Add the item back in at its new levels
                                        _queue.Queue.Add(checkSkill.Name & checkSkill.FromLevel & checkSkill.ToLevel, checkSkill)
                                    End If
                                End If
                            End If
                        End If

                        idx += 1
                    Loop Until idx > _queue.Queue.Count - 1

                    ' Check if the skill has been trained and we wish to increase it to the next level
                    If adtQueue.SelectedNodes(0).Style Is adtQueue.Styles("SkillCompleted") Then
                        Dim curLevel As Integer = 0
                        If _queuePilot.PilotSkills.ContainsKey(myTSkill.Name) Then
                            curLevel = _queuePilot.PilotSkills(myTSkill.Name).Level
                        End If
                        myTSkill.FromLevel = Math.Max(CInt(curLevel), myTSkill.FromLevel)
                    End If

                    ' Delete the existing item
                    _queue.Queue.Remove(keyName)
                    ' Adjust the "to" level
                    myTSkill.ToLevel = selectedLevel
                    ' Add the item back in at its new levels
                    _queue.Queue.Add(myTSkill.Name & myTSkill.FromLevel & myTSkill.ToLevel, myTSkill)
                    Call DrawQueue()
                    adtQueue.SelectedNodes.Add(adtQueue.Nodes(oldIndex))
                    adtQueue.Nodes(oldIndex).EnsureVisible()
                End If
            End If
        End Sub

        Public Sub DeleteFromQueue(ByVal mySkill As Core.EveHQSkillQueueItem)
            Dim delPos As Integer = mySkill.Pos
            _queue.Queue.Remove(mySkill.Name & mySkill.FromLevel & mySkill.ToLevel)
            ' Reshuffle all the positions below
            For Each mySkill In _queue.Queue.Values
                If mySkill.Pos > delPos Then
                    mySkill.Pos -= 1
                End If
            Next
        End Sub

        Public Sub ClearTrainingQueue()
            Dim reply As Integer
            reply = MessageBox.Show("Are you sure you want to delete the entire training queue?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                _queue.Queue.Clear()
            End If
        End Sub

        Public Sub SplitQueue()

            ' Check for some selection on the listview
            If adtQueue IsNot Nothing Then
                If adtQueue.SelectedNodes.Count > 0 Then
                    ' Build a new queue
                    Dim selQueue As New Core.EveHQSkillQueue
                    For Each lvi As ListViewItem In adtQueue.SelectedNodes
                        Dim keyName As String = lvi.Name
                        Dim splitSkillQueueItem As Core.EveHQSkillQueueItem = CType(_queue.Queue(keyName).Clone, Core.EveHQSkillQueueItem)
                        selQueue.Queue.Add(keyName, splitSkillQueueItem)
                    Next
                    Dim myQueue As New Core.FrmModifyQueues
                    With myQueue
                        ' Load the account details into the text boxes
                        .txtQueueName.Text = selQueue.Name : .txtQueueName.Tag = selQueue.Queue
                        .btnAccept.Text = "Split" : .Tag = "Split"
                        .Text = "Split '" & _queue.Name & "' Queue Details"
                        .DisplayPilotName = _queuePilot.Name
                        .ShowDialog()
                    End With
                End If
            End If

        End Sub

        Public Sub IncreaseLevel()

            ' Get the skill name
            Dim keyName As String = adtQueue.SelectedNodes(0).Name
            Dim myTSkill As Core.EveHQSkillQueueItem = _queue.Queue(keyName)

            If myTSkill.ToLevel < 5 Then
                ChangeLevel(myTSkill.ToLevel + 1)
            End If

        End Sub

        Public Sub DecreaseLevel()

            ' Get the skill name
            Dim keyName As String = adtQueue.SelectedNodes(0).Name
            Dim myTSkill As Core.EveHQSkillQueueItem = _queue.Queue(keyName)

            If myTSkill.ToLevel > 1 Then
                ChangeLevel(myTSkill.ToLevel - 1)
            End If

        End Sub

        Public Sub MoveUpQueue()
            ' Store the keyname being used
            Dim keyName As String = adtQueue.SelectedNodes(0).Name
            Dim sourceSkill As Core.EveHQSkillQueueItem
            sourceSkill = _queue.Queue(keyName)
            Dim oldPos As Integer = sourceSkill.Pos
            Dim queueJump As Integer = 0
            Dim maxJump As Integer
            If _queuePilot.Training = True Then
                maxJump = 1
            Else
                maxJump = 0
            End If
            Dim si As Integer
            Dim di As Integer
            Dim newPos As Integer
            Do
                queueJump += 1
                si = sourceSkill.Pos
                di = si - queueJump
                Dim destSkill As Core.EveHQSkillQueueItem
                For Each destSkill In _queue.Queue.Values
                    If destSkill.Pos = di Then Exit For
                Next

                'Dim din As String = destSkill.Name & destSkill.FromLevel & destSkill.ToLevel
                Dim sin As String = sourceSkill.Name & sourceSkill.FromLevel & sourceSkill.ToLevel

                Dim mySSkill As Core.EveHQSkillQueueItem
                mySSkill = _queue.Queue(sin)
                ' Move all the items up or down depending on position
                If si > di Then
                    Dim moveSkill As Core.EveHQSkillQueueItem
                    For Each moveSkill In _queue.Queue.Values
                        If moveSkill.Pos >= di And moveSkill.Pos < si Then
                            moveSkill.Pos += 1
                        End If
                    Next
                Else
                    Dim moveSkill As Core.EveHQSkillQueueItem
                    For Each moveSkill In _queue.Queue.Values
                        If moveSkill.Pos > si And moveSkill.Pos <= di Then
                            moveSkill.Pos -= 1
                        End If
                    Next

                End If
                ' Set the source skill to the new location
                mySSkill.Pos = di

                ' Check for movement in the queue
                Core.SkillQueueFunctions.BuildQueue(_queuePilot, _queue, False, True)
                Dim posSkill As Core.EveHQSkillQueueItem
                posSkill = _queue.Queue(keyName)
                newPos = posSkill.Pos

            Loop Until oldPos <> newPos Or di = maxJump

            Call DrawQueue()
            adtQueue.SelectedNodes.Add(adtQueue.FindNodeByName(keyName))
            adtQueue.FindNodeByName(keyName).EnsureVisible()
        End Sub

        Public Sub MoveDownQueue()
            ' Store the keyname being used
            Dim keyName As String = adtQueue.SelectedNodes(0).Name
            Dim sourceSkill As Core.EveHQSkillQueueItem
            sourceSkill = _queue.Queue(keyName)
            Dim oldpos As Integer = sourceSkill.Pos
            Dim newpos As Integer
            Dim di As Integer
            Dim maxJump As Integer = _queue.Queue.Count
            Dim queueJump As Integer = 0
            Do
                queueJump += 1
                Dim si As Integer = sourceSkill.Pos
                di = si + queueJump
                Dim destSkill As Core.EveHQSkillQueueItem
                For Each destSkill In _queue.Queue.Values
                    If destSkill.Pos = di Then Exit For
                Next

                'Dim din As String = destSkill.Name & destSkill.FromLevel & destSkill.ToLevel
                Dim sin As String = sourceSkill.Name & sourceSkill.FromLevel & sourceSkill.ToLevel

                Dim mySSkill As Core.EveHQSkillQueueItem
                mySSkill = _queue.Queue(sin)
                ' Move all the items up or down depending on position
                If si > di Then
                    Dim moveSkill As Core.EveHQSkillQueueItem
                    For Each moveSkill In _queue.Queue.Values
                        If moveSkill.Pos >= di And moveSkill.Pos < si Then
                            moveSkill.Pos += 1
                        End If
                    Next
                Else
                    Dim moveSkill As Core.EveHQSkillQueueItem
                    For Each moveSkill In _queue.Queue.Values
                        If moveSkill.Pos > si And moveSkill.Pos <= di Then
                            moveSkill.Pos -= 1
                        End If
                    Next

                End If
                ' Set the source skill to the new location
                mySSkill.Pos = di

                ' Check for movement in the queue
                Core.SkillQueueFunctions.BuildQueue(_queuePilot, _queue, False, True)
                Dim posSkill As Core.EveHQSkillQueueItem
                posSkill = _queue.Queue(keyName)
                newpos = posSkill.Pos

            Loop Until oldpos <> newpos Or di = maxJump

            Call DrawQueue()
            adtQueue.SelectedNodes.Add(adtQueue.FindNodeByName(keyName))
            adtQueue.FindNodeByName(keyName).EnsureVisible()
        End Sub

        Public Sub DeleteFromQueueOption()

            Dim lowestIndex As Integer = adtQueue.Nodes.Count
            Dim highestIndex As Integer = 0

            ' Get the skill name and levels
            For selItem As Integer = 0 To adtQueue.SelectedNodes.Count - 1

                Dim keyName As String = adtQueue.SelectedNodes(selItem).Name

                ' Set the lowest and highest index
                lowestIndex = Math.Min(adtQueue.SelectedNodes(selItem).Index, lowestIndex)
                highestIndex = Math.Max(adtQueue.SelectedNodes(selItem).Index, highestIndex)

                ' Remove it from the queue
                Dim mySkill As Core.EveHQSkillQueueItem
                If _queue.Queue.ContainsKey(keyName) = True Then
                    mySkill = _queue.Queue(keyName)
                    ' Delete the Skill
                    Call DeleteFromQueue(mySkill)
                End If
            Next
            ' See if we can set the next item, or the previous item if not
            If highestIndex + 1 <= adtQueue.Nodes.Count - 1 Then
                adtQueue.SelectedNodes.Add(adtQueue.Nodes(highestIndex + 1))
            Else
                If lowestIndex <> 0 Then
                    adtQueue.SelectedNodes.Add(adtQueue.Nodes(lowestIndex - 1))
                End If
            End If
            ' Refresh the training view!
            Call DrawQueue()
        End Sub

#End Region

    End Class
End Namespace