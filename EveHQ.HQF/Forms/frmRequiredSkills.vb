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
#End Region

#Region "Properties"

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
            If reqPilot.PilotSkills.Contains(rSkill.Name) = True Then
                aSkill = CType(reqPilot.PilotSkills(rSkill.Name), Core.PilotSkill)
                newSkill.SubItems(2).Text = aSkill.Level.ToString
            Else
                newSkill.SubItems(2).Text = "0"
            End If
            newSkill.SubItems(3).Text = rSkill.CurLevel.ToString
            newSkill.SubItems(4).Text = rSkill.NeededFor
            If CInt(newSkill.SubItems(3).Text) < CInt(newSkill.SubItems(1).Text) Then
                newSkill.ForeColor = Drawing.Color.Red
            Else
                newSkill.ForeColor = Drawing.Color.LimeGreen
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
        Dim pSkill As EveHQ.Core.EveSkill = CType(EveHQ.Core.HQ.SkillListID(pSkillID), Core.EveSkill)

        If pSkill.PreReqSkills.Count > 0 Then
            For Each preReqSkill As String In pSkill.PreReqSkills.Keys
                If EveHQ.Core.HQ.SkillListID.Contains(preReqSkill) Then
                    Dim newSkill As New ContainerListViewItem
                    parentSkill.Items.Add(newSkill)
                    newSkill.Text = EveHQ.Core.SkillFunctions.SkillIDToName(preReqSkill)
                    Dim rSkill As HQFSkill = CType(reqHPilot.SkillSet(newSkill.Text), HQFSkill)
                    newSkill.SubItems(1).Text = pSkill.PreReqSkills(preReqSkill).ToString
                    If reqPilot.PilotSkills.Contains(newSkill.Text) = True Then
                        aSkill = CType(reqPilot.PilotSkills(newSkill.Text), Core.PilotSkill)
                        newSkill.SubItems(2).Text = aSkill.Level.ToString
                    Else
                        newSkill.SubItems(2).Text = "0"
                    End If
                    newSkill.SubItems(3).Text = rSkill.Level.ToString
                    If CInt(newSkill.SubItems(3).Text) < CInt(newSkill.SubItems(1).Text) Then
                        newSkill.ForeColor = Drawing.Color.Red
                    Else
                        newSkill.ForeColor = Drawing.Color.LimeGreen
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
#End Region

End Class