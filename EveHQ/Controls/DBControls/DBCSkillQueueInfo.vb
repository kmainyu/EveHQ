' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Public Class DBCSkillQueueInfo
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Initialise configuration form name
        Me.ControlConfigForm = "EveHQ.DBCSkillQueueInfoConfig"

        ' Load the combo box with the pilot info
        cboPilot.BeginUpdate()
        cboPilot.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
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
            Return cDefaultPilotName
        End Get
        Set(ByVal value As String)
            cDefaultPilotName = value
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(DefaultPilotName) Then
                cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(DefaultPilotName), Core.Pilot)
            End If
            If cboPilot.Items.Contains(DefaultPilotName) = True Then cboPilot.SelectedItem = DefaultPilotName
            If ReadConfig = False Then
                Me.SetConfig("DefaultPilotName", value)
                If Me.EveQueue = True Then
                    Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName & ", Eve Queue")
                Else
                    Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName & ", EveHQ Queue (" & Me.DefaultQueueName & ")")
                End If
            End If
        End Set
    End Property

    Public Property DefaultQueueName() As String
        Get
            Return cDefaultQueueName
        End Get
        Set(ByVal value As String)
            cDefaultQueueName = value
            If cboSkillQueue.Items.Contains(DefaultQueueName) = True Then cboSkillQueue.SelectedItem = DefaultQueueName
            If ReadConfig = False Then
                Me.SetConfig("DefaultQueueName", value)
                If Me.EveQueue = True Then
                    Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName & ", Eve Queue")
                Else
                    Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName & ", EveHQ Queue (" & Me.DefaultQueueName & ")")
                End If
            End If
        End Set
    End Property

    Public Property EveQueue() As Boolean
        Get
            Return cEveQueue
        End Get
        Set(ByVal value As Boolean)
            cEveQueue = value
            If value = True Then
                radEveQueue.Checked = True
            Else
                radEveHQQueue.Checked = True
            End If
            If ReadConfig = False Then
                Me.SetConfig("EveQueue", value)
                If value = True Then
                    Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName & ", Eve Queue")
                Else
                    Me.SetConfig("ControlConfigInfo", "Default Pilot: " & Me.DefaultPilotName & ", EveHQ Queue (" & Me.DefaultQueueName & ")")
                End If
            End If
        End Set
    End Property

#End Region

#Region "Custom Control Variables"
    Dim cDefaultPilotName As String = ""
    Dim cDefaultQueueName As String = ""
    Dim cEveQueue As Boolean = True
#End Region

#Region "Class Variables"
    Dim cPilot As EveHQ.Core.Pilot
#End Region

#Region "Private Methods"
    Private Sub cboPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilot.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            cPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem.ToString), Core.Pilot)
            ' Update the list of EveHQ Skill Queues
            Call Me.UpdateQueueList()
            ' Update the details
            Call Me.UpdateQueueInfo()
        End If
    End Sub

    Private Sub UpdateQueueList()
        cboSkillQueue.BeginUpdate()
        cboSkillQueue.Items.Clear()
        For Each sq As EveHQ.Core.SkillQueue In cPilot.TrainingQueues.Values
            cboSkillQueue.Items.Add(sq.Name)
        Next
        cboSkillQueue.EndUpdate()
        If cboSkillQueue.Items.Count > 0 Then
            If cboSkillQueue.Items.Contains(cDefaultQueueName) = True Then
                cboSkillQueue.SelectedItem = cDefaultQueueName
            Else
                cboSkillQueue.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboSkillQueue_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSkillQueue.SelectedIndexChanged
        If radEveHQQueue.Checked = True Then
            Call Me.UpdateQueueInfo()
        End If
    End Sub

    Private Sub radEveQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEveQueue.CheckedChanged
        Call Me.UpdateQueueInfo()
    End Sub

    Private Sub radEveHQQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEveHQQueue.CheckedChanged
        Call Me.UpdateQueueInfo()
    End Sub

    Private Sub UpdateQueueInfo()
        If cPilot IsNot Nothing Then
            If radEveQueue.Checked = True Then
                ' Draw Eve skill queue
                lvwSkills.BeginUpdate()
                lvwSkills.Items.Clear()
                If cPilot.QueuedSkills IsNot Nothing Then
                    For Each QueuedSkill As EveHQ.Core.PilotQueuedSkill In cPilot.QueuedSkills.Values
                        Dim newitem As New ListViewItem
                        newitem.Text = EveHQ.Core.SkillFunctions.SkillIDToName(QueuedSkill.SkillID.ToString) & " (Lvl " & EveHQ.Core.SkillFunctions.Roman(QueuedSkill.Level) & ")"
                        Dim enddate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime)
                        newitem.ToolTipText = "Skill ends: " & Format(enddate, "ddd") & " " & enddate
                        newitem.Name = QueuedSkill.SkillID.ToString
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
                    If cPilot.TrainingQueues.ContainsKey(cboSkillQueue.SelectedItem.ToString) = True Then
                        Dim cQueue As EveHQ.Core.SkillQueue = CType(cPilot.TrainingQueues(cboSkillQueue.SelectedItem.ToString), Core.SkillQueue)
                        Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(cPilot, cQueue, False, True)
                        For skill As Integer = 0 To arrQueue.Count - 1
                            Dim qItem As EveHQ.Core.SortedQueueItem = CType(arrQueue(skill), EveHQ.Core.SortedQueueItem)
                            If qItem.Done = False Then
                                Dim newitem As New ListViewItem
                                newitem.Text = qItem.Name & " (" & EveHQ.Core.SkillFunctions.Roman(CInt(qItem.FromLevel)) & " -> " & EveHQ.Core.SkillFunctions.Roman(CInt(qItem.ToLevel)) & ")"
                                newitem.Name = qItem.ID
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

    Private Sub lblPilot_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPilot.LinkClicked
        frmPilot.DisplayPilotName = cPilot.Name
        frmEveHQ.OpenPilotInfoForm()
    End Sub

    Private Sub lvwSkills_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwSkills.Resize
        ' Alter the column size to reflect the new listview size
        lvwSkills.Columns(0).Width = lvwSkills.Width - 30
    End Sub
#End Region

End Class
