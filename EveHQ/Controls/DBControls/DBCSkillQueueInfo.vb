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
Imports EveHQ.Core

Namespace Controls.DBControls
    Public Class DBCSkillQueueInfo
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Initialise configuration form name
            ControlConfigForm = "EveHQ.Controls.DBConfigs.DBCSkillQueueInfoConfig"

            ' Load the combo box with the pilot info
            cboPilot.BeginUpdate()
            cboPilot.Items.Clear()
            For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                If pilot.Active = True Then
                    cboPilot.Items.Add(pilot.Name)
                End If
            Next
            cboPilot.EndUpdate()

        End Sub

#Region "Interface Properties"

        Public Overrides ReadOnly Property ControlName() As String
            Get
                Return "Skill Queue Information"
            End Get
        End Property

#End Region

#Region "Custom Control Properties"

        Public Property DefaultPilotName() As String
            Get
                Return _defaultPilotName
            End Get
            Set(ByVal value As String)
                _defaultPilotName = value
                If HQ.Settings.Pilots.ContainsKey(DefaultPilotName) Then
                    _pilot = HQ.Settings.Pilots(DefaultPilotName)
                End If
                If cboPilot.Items.Contains(DefaultPilotName) = True Then cboPilot.SelectedItem = DefaultPilotName
                If ReadConfig = False Then
                    SetConfig("DefaultPilotName", value)
                    If EveQueue = True Then
                        SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName & ", Eve Queue")
                    Else
                        SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName & ", EveHQ Queue (" & DefaultQueueName & ")")
                    End If
                End If
            End Set
        End Property

        Public Property DefaultQueueName() As String
            Get
                Return _defaultQueueName
            End Get
            Set(ByVal value As String)
                _defaultQueueName = value
                If cboSkillQueue.Items.Contains(DefaultQueueName) = True Then cboSkillQueue.SelectedItem = DefaultQueueName
                If ReadConfig = False Then
                    SetConfig("DefaultQueueName", value)
                    If EveQueue = True Then
                        SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName & ", Eve Queue")
                    Else
                        SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName & ", EveHQ Queue (" & DefaultQueueName & ")")
                    End If
                End If
            End Set
        End Property

        Public Property EveQueue() As Boolean
            Get
                Return _eveQueue
            End Get
            Set(ByVal value As Boolean)
                _eveQueue = value
                If value = True Then
                    radEveQueue.Checked = True
                Else
                    radEveHQQueue.Checked = True
                End If
                If ReadConfig = False Then
                    SetConfig("EveQueue", value)
                    If value = True Then
                        SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName & ", Eve Queue")
                    Else
                        SetConfig("ControlConfigInfo", "Default Pilot: " & DefaultPilotName & ", EveHQ Queue (" & DefaultQueueName & ")")
                    End If
                End If
            End Set
        End Property

#End Region

#Region "Custom Control Variables"
        Dim _defaultPilotName As String = ""
        Dim _defaultQueueName As String = ""
        Dim _eveQueue As Boolean = True
#End Region

#Region "Class Variables"
        Dim _pilot As EveHQPilot
#End Region

#Region "Private Methods"
        Private Sub cboPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilot.SelectedIndexChanged
            If HQ.Settings.Pilots.ContainsKey(cboPilot.SelectedItem.ToString) Then
                _pilot = HQ.Settings.Pilots(cboPilot.SelectedItem.ToString)
                ' Update the list of EveHQ Skill Queues
                Call UpdateQueueList()
                ' Update the details
                Call UpdateQueueInfo()
            End If
        End Sub

        Private Sub UpdateQueueList()
            cboSkillQueue.BeginUpdate()
            cboSkillQueue.Items.Clear()
            For Each sq As EveHQSkillQueue In _pilot.TrainingQueues.Values
                cboSkillQueue.Items.Add(sq.Name)
            Next
            cboSkillQueue.EndUpdate()
            If cboSkillQueue.Items.Count > 0 Then
                If cboSkillQueue.Items.Contains(_defaultQueueName) = True Then
                    cboSkillQueue.SelectedItem = _defaultQueueName
                Else
                    cboSkillQueue.SelectedIndex = 0
                End If
            End If
        End Sub

        Private Sub cboSkillQueue_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboSkillQueue.SelectedIndexChanged
            If radEveHQQueue.Checked = True Then
                Call UpdateQueueInfo()
            End If
        End Sub

        Private Sub radEveQueue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radEveQueue.CheckedChanged
            Call UpdateQueueInfo()
        End Sub

        Private Sub radEveHQQueue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radEveHQQueue.CheckedChanged
            Call UpdateQueueInfo()
        End Sub

        Private Sub UpdateQueueInfo()
            If _pilot IsNot Nothing Then
                If radEveQueue.Checked = True Then
                    ' Draw Eve skill queue
                    lvwSkills.BeginUpdate()
                    lvwSkills.Items.Clear()
                    If _pilot.QueuedSkills IsNot Nothing Then
                        For Each queuedSkill As EveHQPilotQueuedSkill In _pilot.QueuedSkills.Values
                            Dim newitem As New ListViewItem
                            newitem.Text = SkillFunctions.SkillIDToName(queuedSkill.SkillID) & " (Lvl " & SkillFunctions.Roman(queuedSkill.Level) & ")"
                            Dim enddate As Date = SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime)
                            newitem.ToolTipText = "Skill ends: " & Format(enddate, "ddd") & " " & enddate
                            newitem.Name = queuedSkill.SkillID.ToString
                            lvwSkills.Items.Add(newitem)
                            'newitem.SubItems.Add(QueuedSkill.Level.ToString)
                        Next
                    End If
                    lvwSkills.EndUpdate()
                Else
                    ' Draw an EveHQ skill queue
                    lvwSkills.BeginUpdate()
                    lvwSkills.Items.Clear()
                    If cboSkillQueue.SelectedItem IsNot Nothing Then
                        If _pilot.TrainingQueues.ContainsKey(cboSkillQueue.SelectedItem.ToString) = True Then
                            Dim cQueue As EveHQSkillQueue = _pilot.TrainingQueues(cboSkillQueue.SelectedItem.ToString)
                            Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(_pilot, cQueue, False, True)
                            For skill As Integer = 0 To arrQueue.Count - 1
                                Dim qItem As SortedQueueItem = CType(arrQueue(skill), SortedQueueItem)
                                If qItem.Done = False Then
                                    Dim newitem As New ListViewItem
                                    newitem.Text = qItem.Name & " (" & SkillFunctions.Roman(CInt(qItem.FromLevel)) & " -> " & SkillFunctions.Roman(CInt(qItem.ToLevel)) & ")"
                                    newitem.Name = CStr(qItem.ID)
                                    lvwSkills.Items.Add(newitem)
                                    newitem.ToolTipText = "Skill ends: " & Format(qItem.DateFinished, "ddd") & " " & qItem.DateFinished.ToString
                                End If
                            Next
                        End If
                    End If
                    lvwSkills.EndUpdate()
                End If
            End If
        End Sub

        Private Sub lblPilot_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblPilot.LinkClicked
            Forms.FrmPilot.DisplayPilotName = _pilot.Name
            Forms.frmEveHQ.OpenPilotInfoForm()
        End Sub

        Private Sub lvwSkills_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles lvwSkills.Resize
            ' Alter the column size to reflect the new listview size
            lvwSkills.Columns(0).Width = lvwSkills.Width - 30
        End Sub
#End Region

    End Class
End NameSpace