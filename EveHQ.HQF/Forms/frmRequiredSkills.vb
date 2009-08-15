' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports DotNetLib.Windows.Forms
Imports System.Windows.Forms

Public Class frmRequiredSkills

#Region "Property Variables"
    Private reqSkills As New SortedList
    Private reqPilot As EveHQ.Core.Pilot
    Private reqHPilot As HQFPilot
    Private SkillList As New SortedList(Of String, Integer)
#End Region

#Region "Properties"

    Private WriteOnly Property ForceUpdate() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                HQFEvents.StartUpdateShipInfo = reqPilot.Name
            End If
        End Set
    End Property

    Public Property Skills() As SortedList
        Get
            Return reqSkills
        End Get
        Set(ByVal value As SortedList)
            reqSkills = value
            Call Me.DrawSkillsTable()
        End Set
    End Property

    Public Property Pilot() As EveHQ.Core.Pilot
        Get
            Return reqPilot
        End Get
        Set(ByVal value As EveHQ.Core.Pilot)
            reqPilot = value
            reqHPilot = CType(HQFPilotCollection.HQFPilots(reqPilot.Name), HQFPilot)
            Me.Text = "Required Skills - " & reqPilot.Name
        End Set
    End Property

#End Region

#Region "Skill Display Routines"

    Private Sub DrawSkillsTable()
        Dim aSkill As EveHQ.Core.PilotSkill
        Dim aLevel As Integer = 0

        ' Compress the list of required skills into the smallest possible list
        Dim newSkills As New SortedList
        For Each rSkill As ReqSkill In reqSkills.Values
            If newSkills.Contains(rSkill.Name & " (Lvl " & rSkill.ReqLevel & ") - " & rSkill.NeededFor) = False Then
                newSkills.Add(rSkill.Name & " (Lvl " & rSkill.ReqLevel & ") - " & rSkill.NeededFor, rSkill)
            End If
        Next

        ' Draw the list
        clvSkills.BeginUpdate()
        clvSkills.Items.Clear()
        For Each rSkill As ReqSkill In newSkills.Values
            Dim newSkill As New ContainerListViewItem
            clvSkills.Items.Add(newSkill)
            newSkill.Text = rSkill.Name
            newSkill.SubItems(1).Text = rSkill.ReqLevel.ToString
            If SkillList.ContainsKey(rSkill.Name) = False Then
                SkillList.Add(rSkill.Name, rSkill.ReqLevel)
            Else
                If SkillList(rSkill.Name) < rSkill.ReqLevel Then
                    SkillList(rSkill.Name) = rSkill.ReqLevel
                End If
            End If
            If reqPilot.PilotSkills.Contains(rSkill.Name) = True Then
                aSkill = CType(reqPilot.PilotSkills(rSkill.Name), Core.PilotSkill)
                newSkill.SubItems(2).Text = aSkill.Level.ToString
            Else
                newSkill.SubItems(2).Text = "0"
            End If
            newSkill.SubItems(3).Text = rSkill.CurLevel.ToString
            newSkill.SubItems(4).Text = rSkill.NeededFor
            Dim reqLevel As Integer = CInt(newSkill.SubItems(1).Text)
            Dim actLevel As Integer = CInt(newSkill.SubItems(2).Text)
            Dim hqfLevel As Integer = CInt(newSkill.SubItems(3).Text)
            If actLevel >= reqLevel And hqfLevel > reqLevel Then
                newSkill.ForeColor = Drawing.Color.LimeGreen
            Else
                If hqfLevel > reqLevel Then
                    newSkill.ForeColor = Drawing.Color.Orange
                Else
                    newSkill.ForeColor = Drawing.Color.Red
                End If
            End If
            ' Check for sub skills
            Call Me.DisplaySubSkills(newSkill, rSkill.ID)
        Next
        clvSkills.EndUpdate()

        ' Calculate the Queue Time
        Call Me.CalculateQueueTime()

    End Sub

    Private Sub DisplaySubSkills(ByVal parentSkill As ContainerListViewItem, ByVal pSkillID As String)
        Dim aSkill As EveHQ.Core.PilotSkill
        Dim pSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(pSkillID)

        If pSkill.PreReqSkills.Count > 0 Then
            For Each preReqSkill As String In pSkill.PreReqSkills.Keys
                If EveHQ.Core.HQ.SkillListID.ContainsKey(preReqSkill) Then
                    Dim newSkill As New ContainerListViewItem
                    parentSkill.Items.Add(newSkill)
                    newSkill.Text = EveHQ.Core.SkillFunctions.SkillIDToName(preReqSkill)
                    Dim rSkill As HQFSkill = CType(reqHPilot.SkillSet(newSkill.Text), HQFSkill)
                    newSkill.SubItems(1).Text = pSkill.PreReqSkills(preReqSkill).ToString
                    If SkillList.ContainsKey(newSkill.Text) = False Then
                        SkillList.Add(newSkill.Text, pSkill.PreReqSkills(preReqSkill))
                    Else
                        If SkillList(newSkill.Text) < pSkill.PreReqSkills(preReqSkill) Then
                            SkillList(newSkill.Text) = pSkill.PreReqSkills(preReqSkill)
                        End If
                    End If
                    If reqPilot.PilotSkills.Contains(newSkill.Text) = True Then
                        aSkill = CType(reqPilot.PilotSkills(newSkill.Text), Core.PilotSkill)
                        newSkill.SubItems(2).Text = aSkill.Level.ToString
                    Else
                        newSkill.SubItems(2).Text = "0"
                    End If
                    newSkill.SubItems(3).Text = rSkill.Level.ToString
                    Dim reqLevel As Integer = CInt(newSkill.SubItems(1).Text)
                    Dim actLevel As Integer = CInt(newSkill.SubItems(2).Text)
                    Dim hqfLevel As Integer = CInt(newSkill.SubItems(3).Text)
                    If actLevel >= reqLevel And hqfLevel > reqLevel Then
                        newSkill.ForeColor = Drawing.Color.LimeGreen
                    Else
                        If hqfLevel > reqLevel Then
                            newSkill.ForeColor = Drawing.Color.Orange
                        Else
                            newSkill.ForeColor = Drawing.Color.Red
                        End If
                    End If
                    Call Me.DisplaySubSkills(newSkill, preReqSkill)
                End If

            Next
        End If

    End Sub

    Private Sub CalculateQueueTime()
        Dim nPilot As EveHQ.Core.Pilot = reqPilot
        Dim newQueue As New EveHQ.Core.SkillQueue
        newQueue.Name = "HQFQueue"
        newQueue.IncCurrentTraining = False
        newQueue.Primary = False

        ' Add the skills we have to the training queue (in any order, no learning skills will be applied)
        Dim skill As Integer = 0
        For Each rSkill As ReqSkill In reqSkills.Values
            Dim skillName As String = rSkill.Name
            Dim skillLevel As Integer = CInt(rSkill.ReqLevel)
            Dim qItem As New EveHQ.Core.SkillQueueItem
            qItem.Name = skillName
            qItem.FromLevel = 0
            qItem.ToLevel = skillLevel
            qItem.Pos = Skill + 1
            qItem.Key = qItem.Name & qItem.FromLevel & qItem.ToLevel
            newQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(nPilot, skillName, skill + 1, newQueue, skillLevel, True, True)
        Next

        ' Build the Queue
        Dim aQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(nPilot, newQueue)

        ' Now Let's optimize this queue, because it won't be optimal!
        Dim optimalQueue As EveHQ.Core.SkillQueue = EveHQ.Core.SkillQueueFunctions.FindSuggestions(nPilot, newQueue)

        ' Display the time results
        lblQueueTime.Text = "Estimated Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(newQueue.QueueTime) & " (Optimal Time: " & EveHQ.Core.SkillFunctions.TimeToString(optimalQueue.QueueTime) & ")"

    End Sub

#End Region

#Region "Button Routines"

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnAddToQueue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddToQueue.Click
        Call Me.AddNeededSkillsToQueue()
    End Sub

    Private Sub AddNeededSkillsToQueue()
        Dim selQ As New frmSelectQueue
        selQ.rPilot = reqPilot
        selQ.skillsNeeded = reqSkills
        selQ.ShowDialog()
        EveHQ.Core.SkillQueueFunctions.StartQueueRefresh = True
    End Sub

    Private Sub btnSetSkillsToRequirements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetSkillsToRequirements.Click
        For Each requiredSkill As String In SkillList.Keys
            Dim MyHQFSkill As HQFSkill = CType(reqHPilot.SkillSet(requiredSkill), HQFSkill)
            If MyHQFSkill.Level < SkillList(requiredSkill) Then
                MyHQFSkill.Level = SkillList(requiredSkill)
            End If
        Next
        ForceUpdate = True
        Call Me.UpdateReqSkills()
        Call Me.DrawSkillsTable()
    End Sub

    Private Sub UpdateReqSkills()
        For Each rSkill As ReqSkill In reqSkills.Values
            If SkillList.ContainsKey(rSkill.Name) = True Then
                rSkill.CurLevel = SkillList(rSkill.Name)
            End If
        Next
    End Sub

#End Region

    
End Class