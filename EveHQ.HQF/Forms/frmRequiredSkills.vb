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
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar

Public Class frmRequiredSkills

    ReadOnly _trainedSkillStyle As ElementStyle
    ReadOnly _hqfSkillStyle As ElementStyle
    ReadOnly _notTrainedSkillStyle As ElementStyle
    ReadOnly _fittingName As String

#Region "Property Variables"
    Private _reqSkills As New ArrayList
    Private _reqPilot As Core.EveHQPilot
    Private _reqHPilot As FittingPilot
    Private ReadOnly _skillList As New SortedList(Of String, Integer)
#End Region

#Region "Properties"

    Private WriteOnly Property ForceUpdate() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                HQFEvents.StartUpdateShipInfo = _reqPilot.Name
            End If
        End Set
    End Property

    Public Property Skills() As ArrayList
        Get
            Return _reqSkills
        End Get
        Set(ByVal value As ArrayList)
            _reqSkills = value
            Call DrawSkillsTable()
        End Set
    End Property

    Public Property Pilot() As Core.EveHQPilot
        Get
            Return _reqPilot
        End Get
        Set(ByVal value As Core.EveHQPilot)
            _reqPilot = value
            _reqHPilot = FittingPilots.HQFPilots(_reqPilot.Name)
            Text = "Required Skills - " & _reqPilot.Name
        End Set
    End Property

#End Region

#Region "Form Constructor"

    Public Sub New(fitting As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Set Fitting Name
        _fittingName = Fitting

        ' Set Styles
        _trainedSkillStyle = adtSkills.Styles("Skill").Copy
        _hqfSkillStyle = adtSkills.Styles("Skill").Copy
        _notTrainedSkillStyle = adtSkills.Styles("Skill").Copy
        _trainedSkillStyle.TextColor = Drawing.Color.LimeGreen
        _hqfSkillStyle.TextColor = Drawing.Color.Orange
        _notTrainedSkillStyle.TextColor = Drawing.Color.Red
    End Sub

#End Region

#Region "Skill Display Routines"

    Private Sub DrawSkillsTable()
        Dim aSkill As Core.EveHQPilotSkill
        Dim hSkill As FittingSkill

        ' Compress the list of required skills into the smallest possible list
        Dim newSkills As New SortedList
        For Each rSkill As ReqSkill In _reqSkills
            If newSkills.Contains(rSkill.Name & " (Lvl " & rSkill.ReqLevel & ") - " & rSkill.NeededFor) = False Then
                newSkills.Add(rSkill.Name & " (Lvl " & rSkill.ReqLevel & ") - " & rSkill.NeededFor, rSkill)
            End If
        Next

        ' Draw the list
        adtSkills.BeginUpdate()
        adtSkills.Nodes.Clear()
        For Each rSkill As ReqSkill In newSkills.Values
            Dim newSkill As New Node
            newSkill.Text = rSkill.Name
            newSkill.Cells.Add(New Cell(rSkill.ReqLevel.ToString))
            If _skillList.ContainsKey(rSkill.Name) = False Then
                _skillList.Add(rSkill.Name, rSkill.ReqLevel)
            Else
                If _skillList(rSkill.Name) < rSkill.ReqLevel Then
                    _skillList(rSkill.Name) = rSkill.ReqLevel
                End If
            End If
            If _reqPilot.PilotSkills.ContainsKey(rSkill.Name) = True Then
                aSkill = _reqPilot.PilotSkills(rSkill.Name)
                newSkill.Cells.Add(New Cell(aSkill.Level.ToString))
            Else
                newSkill.Cells.Add(New Cell("0"))
            End If
            If _reqHPilot.SkillSet.ContainsKey(rSkill.Name) = True Then
                hSkill = _reqHPilot.SkillSet(rSkill.Name)
                newSkill.Cells.Add(New Cell(hSkill.Level.ToString))
            Else
                newSkill.Cells.Add(New Cell("0"))
            End If
            newSkill.Cells.Add(New Cell(rSkill.NeededFor))
            newSkill.Cells.Add(New Cell(Core.SkillFunctions.TimeToString(Core.SkillFunctions.TimeBeforeCanTrain(_reqPilot, rSkill.ID, rSkill.ReqLevel))))
            Dim reqLevel As Integer = CInt(newSkill.Cells(1).Text)
            Dim actLevel As Integer = CInt(newSkill.Cells(2).Text)
            Dim hqfLevel As Integer = CInt(newSkill.Cells(3).Text)
            If actLevel >= reqLevel And hqfLevel >= reqLevel Then
                newSkill.Style = _trainedSkillStyle
            Else
                If hqfLevel >= reqLevel Then
                    newSkill.Style = _hqfSkillStyle
                Else
                    newSkill.Style = _notTrainedSkillStyle
                End If
            End If
            adtSkills.Nodes.Add(newSkill)
            newSkill.Tooltip = Core.SkillFunctions.TimeToString(Core.SkillFunctions.TimeBeforeCanTrain(_reqPilot, rSkill.ID, rSkill.ReqLevel))
            ' Check for sub skills
            Call DisplaySubSkills(newSkill, rSkill.ID)
        Next
        adtSkills.EndUpdate()

        ' Calculate the Queue Time
        Call CalculateQueueTime()

    End Sub

    Private Sub DisplaySubSkills(ByVal parentSkill As Node, ByVal pSkillID As Integer)
        Dim aSkill As Core.EveHQPilotSkill
        Dim pSkill As Core.EveSkill = Core.HQ.SkillListID(pSkillID)

        If pSkill.PreReqSkills.Count > 0 Then
            For Each preReqSkill As Integer In pSkill.PreReqSkills.Keys
                If Core.HQ.SkillListID.ContainsKey(preReqSkill) Then
                    Dim newSkill As New Node
                    newSkill.Text = Core.SkillFunctions.SkillIDToName(preReqSkill)
                    Dim rSkill As FittingSkill = _reqHPilot.SkillSet(newSkill.Text)
                    newSkill.Cells.Add(New Cell(pSkill.PreReqSkills(preReqSkill).ToString))
                    If _skillList.ContainsKey(newSkill.Text) = False Then
                        _skillList.Add(newSkill.Text, pSkill.PreReqSkills(preReqSkill))
                    Else
                        If _skillList(newSkill.Text) < pSkill.PreReqSkills(preReqSkill) Then
                            _skillList(newSkill.Text) = pSkill.PreReqSkills(preReqSkill)
                        End If
                    End If
                    If _reqPilot.PilotSkills.ContainsKey(newSkill.Text) = True Then
                        aSkill = _reqPilot.PilotSkills(newSkill.Text)
                        newSkill.Cells.Add(New Cell(aSkill.Level.ToString))
                    Else
                        newSkill.Cells.Add(New Cell("0"))
                    End If
                    newSkill.Cells.Add(New Cell(rSkill.Level.ToString))
                    Dim reqLevel As Integer = CInt(newSkill.Cells(1).Text)
                    Dim actLevel As Integer = CInt(newSkill.Cells(2).Text)
                    Dim hqfLevel As Integer = CInt(newSkill.Cells(3).Text)
                    If actLevel >= reqLevel And hqfLevel >= reqLevel Then
                        newSkill.Style = _trainedSkillStyle
                    Else
                        If hqfLevel >= reqLevel Then
                            newSkill.Style = _hqfSkillStyle
                        Else
                            newSkill.Style = _notTrainedSkillStyle
                        End If
                    End If
                    newSkill.Cells.Add(New Cell(""))
                    newSkill.Cells.Add(New Cell(Core.SkillFunctions.TimeToString(Core.SkillFunctions.TimeBeforeCanTrain(_reqPilot, rSkill.ID, reqLevel))))
                    parentSkill.Nodes.Add(newSkill)

                    Call DisplaySubSkills(newSkill, preReqSkill)
                End If

            Next
        End If

    End Sub

    Private Sub CalculateQueueTime()
        Dim nPilot As Core.EveHQPilot = _reqPilot
        Dim newQueue As New Core.EveHQSkillQueue
        newQueue.Name = "HQFQueue"
        newQueue.IncCurrentTraining = False
        newQueue.Primary = False

        ' Add the skills we have to the training queue (in any order, no learning skills will be applied)
        Dim skillPos As Integer = 0
        For Each rSkill As ReqSkill In _reqSkills
            Dim skillName As String = rSkill.Name
            Dim skillLevel As Integer = CInt(rSkill.ReqLevel)
            Dim qItem As New Core.SkillQueueItem
            qItem.Name = skillName
            qItem.FromLevel = 0
            qItem.ToLevel = skillLevel
            qItem.Pos = skillPos + 1
            qItem.Key = qItem.Name & qItem.FromLevel & qItem.ToLevel
            newQueue = Core.SkillQueueFunctions.AddSkillToQueue(nPilot, skillName, skillPos + 1, newQueue, skillLevel, True, True, "HQF: " & _fittingName)
            skillPos += 1
        Next

        ' Build the Queue
        Core.SkillQueueFunctions.BuildQueue(nPilot, newQueue, False, True)

        ' Display the time results
        lblQueueTime.Text = "Estimated Queue Time: " & Core.SkillFunctions.TimeToString(newQueue.QueueTime)

    End Sub

#End Region

#Region "Button Routines"

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Private Sub btnAddToQueue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddToQueue.Click
        Call AddNeededSkillsToQueue()
    End Sub

    Private Sub AddNeededSkillsToQueue()
        Dim neededSkills As New SortedList(Of String, Integer)
        For Each neededSkill As ReqSkill In _reqSkills
            If neededSkills.ContainsKey(neededSkill.Name) = False Then
                neededSkills.Add(neededSkill.Name, neededSkill.ReqLevel)
            Else
                If neededSkill.ReqLevel > neededSkills(neededSkill.Name) Then
                    neededSkills(neededSkill.Name) = neededSkill.ReqLevel
                End If
            End If
        Next
        Dim selQ As New Core.frmSelectQueue(_reqPilot.Name, neededSkills, "HQF: " & _fittingName)
        selQ.ShowDialog()
        Core.SkillQueueFunctions.StartQueueRefresh = True
    End Sub

    Private Sub btnSetSkillsToRequirements_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetSkillsToRequirements.Click
        For Each requiredSkill As String In _skillList.Keys
            Dim myHQFSkill As FittingSkill = _reqHPilot.SkillSet(requiredSkill)
            If myHQFSkill.Level < _skillList(requiredSkill) Then
                myHQFSkill.Level = _skillList(requiredSkill)
            End If
        Next
        ForceUpdate = True
        Call UpdateReqSkills()
        Call DrawSkillsTable()
    End Sub

    Private Sub UpdateReqSkills()
        For Each rSkill As ReqSkill In _reqSkills
            If _skillList.ContainsKey(rSkill.Name) = True Then
                rSkill.CurLevel = _skillList(rSkill.Name)
            End If
        Next
    End Sub

#End Region

End Class