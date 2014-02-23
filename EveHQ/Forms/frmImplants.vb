'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports EveHQ.Core.CoreReports
Imports EveHQ.Core

Namespace Forms
    Public Class FrmImplants

        Dim _iPilot As New EveHQPilot
        ReadOnly _nPilot As New EveHQPilot
        Dim _cPilotName As String = ""
        Dim _queueName As String = ""
        Dim _skillQueue As New EveHQSkillQueue
        Dim _updateAllBases As Boolean = False

        Public Property PilotName() As String
            Get
                Return _cPilotName
            End Get
            Set(ByVal value As String)
                _cPilotName = value
                _iPilot = HQ.Settings.Pilots(_cPilotName)
                Call InitialiseForm()
            End Set
        End Property
        Public Property QueueName() As String
            Get
                Return _queueName
            End Get
            Set(ByVal value As String)
                _queueName = value
                _skillQueue = _iPilot.TrainingQueues(_queueName)
                _nPilot.TrainingQueues.Clear()
                Call DisplayQueueInfo()
            End Set
        End Property

        Private Sub frmNeuralRemap_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            Call InitialiseForm()

        End Sub

        Private Sub InitialiseForm()
            Text = "Implants - " & _iPilot.Name
            ' Create a dummy pilot with which to check new attributes & skill queues

            _nPilot.PilotSkills = _iPilot.PilotSkills
            _nPilot.SkillPoints = _iPilot.SkillPoints
            PilotParseFunctions.LoadKeySkillsForPilot(_nPilot)
            _nPilot.IntAtt = _iPilot.IntAtt : _nPilot.IntImplant = _iPilot.IntImplant : _nPilot.IntAttT = _iPilot.IntAttT
            _nPilot.PAtt = _iPilot.PAtt : _nPilot.PImplant = _iPilot.PImplant : _nPilot.PAttT = _iPilot.PAttT
            _nPilot.CAtt = _iPilot.CAtt : _nPilot.CImplant = _iPilot.CImplant : _nPilot.CAttT = _iPilot.CAttT
            _nPilot.WAtt = _iPilot.WAtt : _nPilot.WImplant = _iPilot.WImplant : _nPilot.WAttT = _iPilot.WAttT
            _nPilot.MAtt = _iPilot.MAtt : _nPilot.MImplant = _iPilot.MImplant : _nPilot.MAttT = _iPilot.MAttT

            Call RecalcAttributes()
            Call DisplayAtributes()
        End Sub
        Private Sub RecalcAttributes()

            _nPilot.WAttT = _nPilot.WAtt + _nPilot.WImplant

            _nPilot.CAttT = _nPilot.CAtt + _nPilot.CImplant

            _nPilot.IntAttT = _nPilot.IntAtt + _nPilot.IntImplant

            _nPilot.MAttT = _nPilot.MAtt + _nPilot.MImplant

            _nPilot.PAttT = _nPilot.PAtt + _nPilot.PImplant

        End Sub
        Private Sub DisplayAtributes()
            ' Display Intelligence Info
            lblIBase.Text = _nPilot.IntAtt.ToString
            nudIImplant.Value = _nPilot.IntImplant
            lblIBase.Text = "Implant: " & _nPilot.IntImplant.ToString
            lblITotal.Text = "Total: " & _nPilot.IntAttT.ToString

            ' Display Perception Info
            nudPImplant.Value = _nPilot.PImplant
            lblPBase.Text = _nPilot.PAtt.ToString
            lblPBase.Text = "Implant: " & _nPilot.PImplant.ToString
            lblPTotal.Text = "Total: " & _nPilot.PAttT.ToString

            ' Display Charisma Info
            nudCImplant.Value = _nPilot.CImplant
            lblCBase.Text = _nPilot.CAtt.ToString
            lblCBase.Text = "Implant: " & _nPilot.CImplant.ToString
            lblCTotal.Text = "Total: " & _nPilot.CAttT.ToString

            ' Display Willpower Info
            nudWImplant.Value = _nPilot.WImplant
            lblWBase.Text = _nPilot.WAtt.ToString
            lblWBase.Text = "Implant: " & _nPilot.WImplant.ToString
            lblWTotal.Text = "Total: " & _nPilot.WAttT.ToString

            ' Display Memory Info
            nudMImplant.Value = _nPilot.MImplant
            lblMBase.Text = _nPilot.MAtt.ToString
            lblMBase.Text = "Implant: " & _nPilot.MImplant.ToString
            lblMTotal.Text = "Total: " & _nPilot.MAttT.ToString

        End Sub

        Private Sub DisplayQueueInfo()
            If _queueName <> "" Then
                ' Display basic queue name and time remaining
                lblActiveSkillQueue.Text = "No Queue Selected"
                lblSkillQueuePointsAnalysis.Text = "Skill Queue Points Analysis"
                lblActiveSkillQueue.Text = _skillQueue.Name
                SkillQueueFunctions.BuildQueue(_iPilot, _iPilot.TrainingQueues(_queueName), False, True)
                Dim iTime As Long = _iPilot.TrainingQueues(_queueName).QueueTime
                If _iPilot.TrainingQueues(_queueName).IncCurrentTraining = True Then
                    iTime += _iPilot.TrainingCurrentTime
                End If
                lblActiveQueueTime.Text = "Time Remaining: " & SkillFunctions.TimeToString(iTime)
                If _nPilot.TrainingQueues.ContainsKey(_queueName) = False Then
                    _nPilot.TrainingQueues.Add(_queueName, CType(_skillQueue.Clone, EveHQSkillQueue))
                End If
                Call SyncTraining()
                ' Add the pilot training info!
                Dim nQueue As ArrayList = SkillQueueFunctions.BuildQueue(_nPilot, _nPilot.TrainingQueues(_queueName), False, False)
                Dim nTime As Long = _nPilot.TrainingQueues(_queueName).QueueTime
                If _nPilot.TrainingQueues(_queueName).IncCurrentTraining = True Then
                    If _iPilot.CAttT = _nPilot.CAttT And _iPilot.IntAttT = _nPilot.IntAttT And _iPilot.MAttT = _nPilot.MAttT And _iPilot.PAttT = _nPilot.PAttT And _iPilot.WAttT = _nPilot.WAttT Then
                        nTime += _nPilot.TrainingCurrentTime
                    Else
                        If HQ.SkillListID.ContainsKey(_nPilot.TrainingSkillID) = True Then
                            Dim mySkill As EveSkill = HQ.SkillListID(_nPilot.TrainingSkillID)
                            Dim ctime As Integer = CInt(SkillFunctions.CalcTimeToLevel(_nPilot, mySkill, _nPilot.TrainingSkillLevel, -1))
                            If Math.Abs(ctime - _nPilot.TrainingCurrentTime) > 5 Then
                                nTime += CInt(SkillFunctions.CalcTimeToLevel(_nPilot, mySkill, _nPilot.TrainingSkillLevel, -1))
                            Else
                                nTime += _nPilot.TrainingCurrentTime
                            End If
                        End If
                    End If
                End If
                lblRevisedQueueTime.Text = "Revised Time: " & SkillFunctions.TimeToString(nTime)
                Dim pointScores(4, 1) As Long
                For a As Integer = 0 To 4
                    pointScores(a, 0) = a
                Next
                For Each skill As SortedQueueItem In nQueue
                    Select Case skill.PAtt
                        Case "Charisma"
                            pointScores(0, 1) += CLng(skill.SPTrained) * 2
                        Case "Intelligence"
                            pointScores(1, 1) += CLng(skill.SPTrained) * 2
                        Case "Memory"
                            pointScores(2, 1) += CLng(skill.SPTrained) * 2
                        Case "Perception"
                            pointScores(3, 1) += CLng(skill.SPTrained) * 2
                        Case "Willpower"
                            pointScores(4, 1) += CLng(skill.SPTrained) * 2
                    End Select
                    Select Case skill.SAtt
                        Case "Charisma"
                            pointScores(0, 1) += CLng(skill.SPTrained)
                        Case "Intelligence"
                            pointScores(1, 1) += CLng(skill.SPTrained)
                        Case "Memory"
                            pointScores(2, 1) += CLng(skill.SPTrained)
                        Case "Perception"
                            pointScores(3, 1) += CLng(skill.SPTrained)
                        Case "Willpower"
                            pointScores(4, 1) += CLng(skill.SPTrained)
                    End Select
                Next
                ' Sort the list
                ' Create a tag array ready to sort the skill times
                Dim tagArray(4) As Integer
                For a As Integer = 0 To 4
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New Reports.RectangularComparer(pointScores)
                Array.Sort(tagArray, myComparer)
                Array.Reverse(tagArray)
                For att As Integer = 0 To 4
                    Select Case tagArray(att)
                        Case 0
                            gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Charisma:"
                        Case 1
                            gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Intelligence:"
                        Case 2
                            gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Memory:"
                        Case 3
                            gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Perception:"
                        Case 4
                            gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Willpower:"
                    End Select
                    gpSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = pointScores(tagArray(att), 1).ToString("N2")
                Next
                If nTime <= iTime Then
                    lblTimeSaving.Text = "Time Saving: " & SkillFunctions.TimeToString(iTime - nTime, False)
                Else
                    lblTimeSaving.Text = "Time Loss: " & SkillFunctions.TimeToString(nTime - iTime, False)
                End If
            Else
                If IsHandleCreated = True Then
                    lblActiveSkillQueue.Text = "No Queue Selected"
                    lblSkillQueuePointsAnalysis.Text = ""
                    For att As Integer = 0 To 4
                        gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = ""
                        gpSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = ""
                    Next
                    lblActiveQueueTime.Text = ""
                    lblRevisedQueueTime.Text = ""
                    lblTimeSaving.Text = ""
                End If
            End If
        End Sub

        Private Sub SyncTraining()
            _nPilot.Training = _iPilot.Training
            _nPilot.TrainingCurrentSP = _iPilot.TrainingCurrentSP
            _nPilot.TrainingCurrentTime = _iPilot.TrainingCurrentTime
            _nPilot.TrainingSkillID = _iPilot.TrainingSkillID
            _nPilot.TrainingSkillName = _iPilot.TrainingSkillName
            _nPilot.TrainingSkillLevel = _iPilot.TrainingSkillLevel
            _nPilot.TrainingStartSP = _iPilot.TrainingStartSP
            _nPilot.TrainingEndSP = _iPilot.TrainingEndSP
            _nPilot.TrainingStartTime = _iPilot.TrainingStartTime
            _nPilot.TrainingEndTime = _iPilot.TrainingEndTime
        End Sub

        Private Sub nudIImplant_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudIImplant.ValueChanged
            If _updateAllBases = False Then
                _nPilot.IntImplant = CInt(nudIImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                _nPilot.IntImplant = CInt(nudIImplant.Value)
            End If
        End Sub

        Private Sub nudPImplant_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudPImplant.ValueChanged
            If _updateAllBases = False Then
                _nPilot.PImplant = CInt(nudPImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                _nPilot.PImplant = CInt(nudPImplant.Value)
            End If
        End Sub

        Private Sub nudCImplant_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudCImplant.ValueChanged
            If _updateAllBases = False Then
                _nPilot.CImplant = CInt(nudCImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                _nPilot.CImplant = CInt(nudCImplant.Value)
            End If
        End Sub

        Private Sub nudWImplant_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudWImplant.ValueChanged
            If _updateAllBases = False Then
                _nPilot.WImplant = CInt(nudWImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                _nPilot.WImplant = CInt(nudWImplant.Value)
            End If
        End Sub

        Private Sub nudMImplant_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMImplant.ValueChanged
            If _updateAllBases = False Then
                _nPilot.MImplant = CInt(nudMImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                _nPilot.MImplant = CInt(nudMImplant.Value)
            End If
        End Sub

        Private Sub btnResetImplants_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetImplants.Click
            _updateAllBases = True
            _nPilot.IntImplant = _iPilot.IntImplant
            _nPilot.PImplant = _iPilot.PImplant
            _nPilot.CImplant = _iPilot.CImplant
            _nPilot.WImplant = _iPilot.WImplant
            _nPilot.MImplant = _iPilot.MImplant
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
            _updateAllBases = False
        End Sub
    End Class
End Namespace