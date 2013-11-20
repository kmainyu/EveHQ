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
Imports EveHQ.Core.CoreReports
Imports EveHQ.Core

Namespace Forms
    Public Class FrmNeuralRemap

        Dim _iPilot As New EveHQPilot
        ReadOnly _nPilot As New EveHQPilot
        Dim _unused As Integer = 0
        Dim _cPilotName As String = ""
        Dim _cQueueName As String = ""
        Dim _skillQueue As New EveHQSkillQueue
        Dim _updateAllBases As Boolean = False
        Dim _cMin, _intMin, _mMin, _pMin, _wMin As Integer

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
                Return _cQueueName
            End Get
            Set(ByVal value As String)
                _cQueueName = value
                _skillQueue = _iPilot.TrainingQueues(_cQueueName)
                _nPilot.TrainingQueues.Clear()
                Call DisplayQueueInfo()
            End Set
        End Property

        Private Sub frmNeuralRemap_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            Call InitialiseForm()

        End Sub

        Private Sub InitialiseForm()

            _updateAllBases = True
            Const MinAtt As Integer = 17
            Const MaxAtt As Integer = 27

            Text = "Neural Remapping - " & _iPilot.Name
            ' Create a dummy pilot with which to check new attributes & skill queues

            ' Reset NUDs
            nudIBase.Minimum = MinAtt : nudIBase.Maximum = MaxAtt : nudIBase.Value = MinAtt
            nudPBase.Minimum = MinAtt : nudPBase.Maximum = MaxAtt : nudPBase.Value = MinAtt
            nudCBase.Minimum = MinAtt : nudCBase.Maximum = MaxAtt : nudCBase.Value = MinAtt
            nudWBase.Minimum = MinAtt : nudWBase.Maximum = MaxAtt : nudWBase.Value = MinAtt
            nudMBase.Minimum = MinAtt : nudMBase.Maximum = MaxAtt : nudMBase.Value = MinAtt

            _nPilot.PilotSkills = _iPilot.PilotSkills
            _nPilot.SkillPoints = _iPilot.SkillPoints
            PilotParseFunctions.LoadKeySkillsForPilot(_nPilot)
            _nPilot.IntAtt = _iPilot.IntAtt : _nPilot.IntImplant = _iPilot.IntImplant : _nPilot.IntAttT = _iPilot.IntAttT
            _nPilot.PAtt = _iPilot.PAtt : _nPilot.PImplant = _iPilot.PImplant : _nPilot.PAttT = _iPilot.PAttT
            _nPilot.CAtt = _iPilot.CAtt : _nPilot.CImplant = _iPilot.CImplant : _nPilot.CAttT = _iPilot.CAttT
            _nPilot.WAtt = _iPilot.WAtt : _nPilot.WImplant = _iPilot.WImplant : _nPilot.WAttT = _iPilot.WAttT
            _nPilot.MAtt = _iPilot.MAtt : _nPilot.MImplant = _iPilot.MImplant : _nPilot.MAttT = _iPilot.MAttT

            ' Check for the maximum allowable base units - API errors
            If _nPilot.IntAtt > MaxAtt Or _nPilot.PAtt > MaxAtt Or _nPilot.CAtt > MaxAtt Or _nPilot.WAtt > MaxAtt Or _nPilot.MAtt > MaxAtt Then
                MessageBox.Show("It would appear that your base attributes contain incorrect values. The Neural Remapper cannot continue until these have been resolved.", "Base Attributes Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Set the minimum allowable units
            _intMin = MinAtt
            _pMin = MinAtt
            _cMin = MinAtt
            _wMin = MinAtt
            _mMin = MinAtt

            ' Set minimums on NUDs
            nudIBase.Minimum = _intMin
            nudPBase.Minimum = _pMin
            nudCBase.Minimum = _cMin
            nudWBase.Minimum = _wMin
            nudMBase.Minimum = _mMin

            ' Check if any base attributes are less than 5 as this is not permitted under the remapping rules
            _unused = 0
            If _nPilot.IntAtt < MinAtt Then
                _unused += MinAtt - _nPilot.IntAtt
                _nPilot.IntAtt = MinAtt
            End If
            If _nPilot.PAtt < MinAtt Then
                _unused += MinAtt - _nPilot.PAtt
                _nPilot.PAtt = MinAtt
            End If
            If _nPilot.CAtt < MinAtt Then
                _unused += MinAtt - _nPilot.CAtt
                _nPilot.CAtt = MinAtt
            End If
            If _nPilot.WAtt < MinAtt Then
                _unused += MinAtt - _nPilot.WAtt
                _nPilot.WAtt = MinAtt
            End If
            If _nPilot.MAtt < MinAtt Then
                _unused += MinAtt - _nPilot.MAtt
                _nPilot.MAtt = MinAtt
            End If

            ' Now reallocate the unused against larger items
            Dim available As Integer
            If _unused > 0 And _nPilot.IntAtt > MinAtt Then
                available = _nPilot.IntAtt - MinAtt
                If available >= _unused Then
                    _nPilot.IntAtt = _nPilot.IntAtt - _unused
                    _unused = 0
                Else
                    _nPilot.IntAtt = MinAtt
                    _unused = _unused - available
                End If
            End If
            If _unused > 0 And _nPilot.PAtt > MinAtt Then
                available = _nPilot.PAtt - MinAtt
                If available >= _unused Then
                    _nPilot.PAtt = _nPilot.PAtt - _unused
                    _unused = 0
                Else
                    _nPilot.PAtt = MinAtt
                    _unused = _unused - available
                End If
            End If
            If _unused > 0 And _nPilot.CAtt > MinAtt Then
                available = _nPilot.CAtt - MinAtt
                If available >= _unused Then
                    _nPilot.CAtt = _nPilot.CAtt - _unused
                    _unused = 0
                Else
                    _nPilot.CAtt = MinAtt
                    _unused = _unused - available
                End If
            End If
            If _unused > 0 And _nPilot.WAtt > MinAtt Then
                available = _nPilot.WAtt - MinAtt
                If available >= _unused Then
                    _nPilot.WAtt = _nPilot.WAtt - _unused
                    _unused = 0
                Else
                    _nPilot.WAtt = MinAtt
                    _unused = _unused - available
                End If
            End If
            If _unused > 0 And _nPilot.MAtt > MinAtt Then
                available = _nPilot.MAtt - MinAtt
                If available >= _unused Then
                    _nPilot.MAtt = _nPilot.MAtt - _unused
                    _unused = 0
                Else
                    _nPilot.MAtt = MinAtt
                    _unused = _unused - available
                End If
            End If

            Call RecalcAttributes()
            Call DisplayAtributes()

            _updateAllBases = False

        End Sub
        Private Sub RecalcAttributes()
            _nPilot.WAttT = _nPilot.WAtt + _nPilot.WImplant
            _nPilot.CAttT = _nPilot.CAtt + _nPilot.CImplant
            _nPilot.IntAttT = _nPilot.IntAtt + _nPilot.IntImplant
            _nPilot.MAttT = _nPilot.MAtt + _nPilot.MImplant
            _nPilot.PAttT = _nPilot.PAtt + _nPilot.PImplant
            _unused = 99 - (_nPilot.IntAtt + _nPilot.PAtt + _nPilot.CAtt + _nPilot.WAtt + _nPilot.MAtt)
        End Sub
        Private Sub DisplayAtributes()
            ' Display Intelligence Info
            lblIntelligence.Text = "Intelligence (Default Base: " & _iPilot.IntAtt.ToString & ")"
            nudIBase.Value = _nPilot.IntAtt
            lblIImplant.Text = "Implant: " & _nPilot.IntImplant.ToString
            lblITotal.Text = "Total: " & _nPilot.IntAttT.ToString

            ' Display Perception Info
            lblPerception.Text = "Perception (Default Base: " & _iPilot.PAtt.ToString & ")"
            nudPBase.Value = _nPilot.PAtt
            lblPImplant.Text = "Implant: " & _nPilot.PImplant.ToString
            lblPTotal.Text = "Total: " & _nPilot.PAttT.ToString

            ' Display Charisma Info
            lblCharisma.Text = "Charisma (Default Base: " & _iPilot.CAtt.ToString & ")"
            nudCBase.Value = _nPilot.CAtt
            lblCImplant.Text = "Implant: " & _nPilot.CImplant.ToString
            lblCTotal.Text = "Total: " & _nPilot.CAttT.ToString

            ' Display Willpower Info
            lblWillpower.Text = "Willpower (Default Base: " & _iPilot.WAtt.ToString & ")"
            nudWBase.Value = _nPilot.WAtt
            lblWImplant.Text = "Implant: " & _nPilot.WImplant.ToString
            lblWTotal.Text = "Total: " & _nPilot.WAttT.ToString

            ' Display Memory Info
            lblMemory.Text = "Memory (Default Base: " & _iPilot.MAtt.ToString & ")"
            nudMBase.Value = _nPilot.MAtt
            lblMImplant.Text = "Implant: " & _nPilot.MImplant.ToString
            lblMTotal.Text = "Total: " & _nPilot.MAttT.ToString

            ' Display remaining attribute points
            lblUnusedPoints.Text = _unused.ToString

        End Sub

        Private Sub DisplayQueueInfo()
            If _cQueueName <> "" Then
                ' Display basic queue name and time remaining
                lblActiveSkillQueue.Text = "No Queue Selected"
                lblSkillQueuePointsAnalysis.Text = "Skill Queue Points Analysis"
                lblActiveSkillQueue.Text = _skillQueue.Name
                SkillQueueFunctions.BuildQueue(_iPilot, _iPilot.TrainingQueues(_cQueueName), False, True)
                Dim iTime As Long = _iPilot.TrainingQueues(_cQueueName).QueueTime
                If _iPilot.TrainingQueues(_cQueueName).IncCurrentTraining = True Then
                    iTime += _iPilot.TrainingCurrentTime
                End If
                lblActiveQueueTime.Text = "Time Remaining: " & SkillFunctions.TimeToString(iTime)
                If _nPilot.TrainingQueues.ContainsKey(_cQueueName) = False Then
                    _nPilot.TrainingQueues.Add(_cQueueName, CType(_skillQueue.Clone, EveHQSkillQueue))
                End If
                Call SyncTraining()
                ' Add the pilot training info!
                Dim nQueue As ArrayList = SkillQueueFunctions.BuildQueue(_nPilot, _nPilot.TrainingQueues(_cQueueName), False, True)
                Dim nTime As Long = _nPilot.TrainingQueues(_cQueueName).QueueTime
                If _nPilot.TrainingQueues(_cQueueName).IncCurrentTraining = True Then
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
                btnOptimise.Enabled = True
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
                    btnOptimise.Enabled = False
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

        Private Sub nudIBase_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudIBase.ValueChanged
            If _updateAllBases = False Then
                If _nPilot.IntAtt - nudIBase.Value + _unused >= 0 Then
                    _nPilot.IntAtt = CInt(nudIBase.Value)
                    Call RecalcAttributes()
                    Call DisplayAtributes()
                    Call DisplayQueueInfo()
                Else
                    nudIBase.Value = _nPilot.IntAtt
                End If
            Else
                _nPilot.IntAtt = CInt(nudIBase.Value)
            End If
        End Sub

        Private Sub nudPBase_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudPBase.ValueChanged
            If _updateAllBases = False Then
                If _nPilot.PAtt - nudPBase.Value + _unused >= 0 Then
                    _nPilot.PAtt = CInt(nudPBase.Value)
                    Call RecalcAttributes()
                    Call DisplayAtributes()
                    Call DisplayQueueInfo()
                Else
                    nudPBase.Value = _nPilot.PAtt
                End If
            Else
                _nPilot.PAtt = CInt(nudPBase.Value)
            End If
        End Sub

        Private Sub nudCBase_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudCBase.ValueChanged
            If _updateAllBases = False Then
                If _nPilot.CAtt - nudCBase.Value + _unused >= 0 Then
                    _nPilot.CAtt = CInt(nudCBase.Value)
                    Call RecalcAttributes()
                    Call DisplayAtributes()
                    Call DisplayQueueInfo()
                Else
                    nudCBase.Value = _nPilot.CAtt
                End If
            Else
                _nPilot.CAtt = CInt(nudCBase.Value)
            End If
        End Sub

        Private Sub nudWBase_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudWBase.ValueChanged
            If _updateAllBases = False Then
                If _nPilot.WAtt - nudWBase.Value + _unused >= 0 Then
                    _nPilot.WAtt = CInt(nudWBase.Value)
                    Call RecalcAttributes()
                    Call DisplayAtributes()
                    Call DisplayQueueInfo()
                Else
                    nudWBase.Value = _nPilot.WAtt
                End If
            Else
                _nPilot.WAtt = CInt(nudWBase.Value)
            End If
        End Sub

        Private Sub nudMBase_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMBase.ValueChanged
            If _updateAllBases = False Then
                If _nPilot.MAtt - nudMBase.Value + _unused >= 0 Then
                    _nPilot.MAtt = CInt(nudMBase.Value)
                    Call RecalcAttributes()
                    Call DisplayAtributes()
                    Call DisplayQueueInfo()
                Else
                    nudMBase.Value = _nPilot.MAtt
                End If
            Else
                _nPilot.MAtt = CInt(nudMBase.Value)
            End If
        End Sub

        Private Sub btnOptimise_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOptimise.Click
            Cursor = Cursors.WaitCursor
            Dim bestTime As Long = _skillQueue.QueueTime * 2
            Dim calcTime As Long
            Dim bestI As Integer = _nPilot.IntAtt
            Dim bestP As Integer = _nPilot.PAtt
            Dim bestC As Integer = _nPilot.CAtt
            Dim bestW As Integer = _nPilot.WAtt
            Dim bestM As Integer = _nPilot.MAtt
            ' Maximum distributable points (39 - (5 x 5))
            Const MaxAtt As Integer = 27 ' Max = 27

            For intAtt As Integer = _intMin To MaxAtt
                _nPilot.IntAtt = intAtt
                For pAtt As Integer = _pMin To MaxAtt
                    _nPilot.PAtt = pAtt
                    For cAtt As Integer = _cMin To MaxAtt
                        _nPilot.CAtt = cAtt
                        For wAtt As Integer = _wMin To MaxAtt
                            _nPilot.WAtt = wAtt
                            For mAtt As Integer = _pMin To MaxAtt
                                _nPilot.MAtt = mAtt
                                If intAtt + pAtt + cAtt + wAtt + mAtt = 99 Then
                                    Call RecalcAttributes()
                                    calcTime = SkillQueueFunctions.GetQueueTime(_nPilot, _nPilot.TrainingQueues(_cQueueName))
                                    If calcTime <= bestTime Then
                                        bestTime = calcTime
                                        bestI = intAtt
                                        bestP = pAtt
                                        bestC = cAtt
                                        bestW = wAtt
                                        bestM = mAtt
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            Next
            _updateAllBases = True
            _nPilot.IntAtt = bestI
            _nPilot.PAtt = bestP
            _nPilot.CAtt = bestC
            _nPilot.WAtt = bestW
            _nPilot.MAtt = bestM
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
            _updateAllBases = False
            Cursor = Cursors.Default
        End Sub

        Private Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
            Call InitialiseForm()
            Call DisplayQueueInfo()
        End Sub
    End Class
End Namespace