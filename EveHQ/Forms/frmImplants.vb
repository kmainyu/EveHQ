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
Namespace Forms
    Public Class frmImplants
        Dim iPilot As New EveHQ.Core.EveHQPilot
        Dim nPilot As New EveHQ.Core.EveHQPilot
        Dim cPilotName As String = ""
        Dim cQueueName As String = ""
        Dim SkillQueue As New EveHQ.Core.EveHQSkillQueue
        Dim UpdateAllBases As Boolean = False

        Public Property PilotName() As String
            Get
                Return cPilotName
            End Get
            Set(ByVal value As String)
                cPilotName = value
                iPilot = EveHQ.Core.HQ.Settings.Pilots(cPilotName)
                Call Me.InitialiseForm()
            End Set
        End Property
        Public Property QueueName() As String
            Get
                Return cQueueName
            End Get
            Set(ByVal value As String)
                cQueueName = value
                SkillQueue = iPilot.TrainingQueues(cQueueName)
                nPilot.TrainingQueues.Clear()
                Call Me.DisplayQueueInfo()
            End Set
        End Property

        Private Sub frmNeuralRemap_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Call Me.InitialiseForm()

        End Sub

        Private Sub InitialiseForm()
            Me.Text = "Implants - " & iPilot.Name
            ' Create a dummy pilot with which to check new attributes & skill queues

            nPilot.PilotSkills = iPilot.PilotSkills
            nPilot.SkillPoints = iPilot.SkillPoints
            EveHQ.Core.PilotParseFunctions.LoadKeySkillsForPilot(nPilot)
            nPilot.IAtt = iPilot.IAtt : nPilot.IImplant = iPilot.IImplant : nPilot.IAttT = iPilot.IAttT
            nPilot.PAtt = iPilot.PAtt : nPilot.PImplant = iPilot.PImplant : nPilot.PAttT = iPilot.PAttT
            nPilot.CAtt = iPilot.CAtt : nPilot.CImplant = iPilot.CImplant : nPilot.CAttT = iPilot.CAttT
            nPilot.WAtt = iPilot.WAtt : nPilot.WImplant = iPilot.WImplant : nPilot.WAttT = iPilot.WAttT
            nPilot.MAtt = iPilot.MAtt : nPilot.MImplant = iPilot.MImplant : nPilot.MAttT = iPilot.MAttT

            Call RecalcAttributes()
            Call DisplayAtributes()
        End Sub
        Private Sub RecalcAttributes()

            nPilot.WAttT = nPilot.WAtt + nPilot.WImplant

            nPilot.CAttT = nPilot.CAtt + nPilot.CImplant

            nPilot.IAttT = nPilot.IAtt + nPilot.IImplant

            nPilot.MAttT = nPilot.MAtt + nPilot.MImplant

            nPilot.PAttT = nPilot.PAtt + nPilot.PImplant

        End Sub
        Private Sub DisplayAtributes()
            ' Display Intelligence Info
            lblIBase.Text = nPilot.IAtt.ToString
            nudIImplant.Value = nPilot.IImplant
            lblIBase.Text = "Implant: " & nPilot.IImplant.ToString
            lblITotal.Text = "Total: " & nPilot.IAttT.ToString

            ' Display Perception Info
            nudPImplant.Value = nPilot.PImplant
            lblPBase.Text = nPilot.PAtt.ToString
            lblPBase.Text = "Implant: " & nPilot.PImplant.ToString
            lblPTotal.Text = "Total: " & nPilot.PAttT.ToString

            ' Display Charisma Info
            nudCImplant.Value = nPilot.CImplant
            lblCBase.Text = nPilot.CAtt.ToString
            lblCBase.Text = "Implant: " & nPilot.CImplant.ToString
            lblCTotal.Text = "Total: " & nPilot.CAttT.ToString

            ' Display Willpower Info
            nudWImplant.Value = nPilot.WImplant
            lblWBase.Text = nPilot.WAtt.ToString
            lblWBase.Text = "Implant: " & nPilot.WImplant.ToString
            lblWTotal.Text = "Total: " & nPilot.WAttT.ToString

            ' Display Memory Info
            nudMImplant.Value = nPilot.MImplant
            lblMBase.Text = nPilot.MAtt.ToString
            lblMBase.Text = "Implant: " & nPilot.MImplant.ToString
            lblMTotal.Text = "Total: " & nPilot.MAttT.ToString

        End Sub

        Private Sub DisplayQueueInfo()
            If cQueueName <> "" Then
                ' Display basic queue name and time remaining
                lblActiveSkillQueue.Text = "No Queue Selected"
                lblSkillQueuePointsAnalysis.Text = "Skill Queue Points Analysis"
                lblActiveSkillQueue.Text = SkillQueue.Name
                Dim iQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(iPilot, iPilot.TrainingQueues(cQueueName), False, True)
                Dim iTime As Long = iPilot.TrainingQueues(cQueueName).QueueTime
                If iPilot.TrainingQueues(cQueueName).IncCurrentTraining = True Then
                    iTime += iPilot.TrainingCurrentTime
                End If
                lblActiveQueueTime.Text = "Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(iTime)
                If nPilot.TrainingQueues.ContainsKey(cQueueName) = False Then
                    nPilot.TrainingQueues.Add(cQueueName, CType(SkillQueue.Clone, Core.EveHQSkillQueue))
                End If
                Call Me.SyncTraining()
                ' Add the pilot training info!
                Dim nQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(nPilot, nPilot.TrainingQueues(cQueueName), False, False)
                Dim nTime As Long = nPilot.TrainingQueues(cQueueName).QueueTime
                If nPilot.TrainingQueues(cQueueName).IncCurrentTraining = True Then
                    If iPilot.CAttT = nPilot.CAttT And iPilot.IAttT = nPilot.IAttT And iPilot.MAttT = nPilot.MAttT And iPilot.PAttT = nPilot.PAttT And iPilot.WAttT = nPilot.WAttT Then
                        nTime += nPilot.TrainingCurrentTime
                    Else
                        If EveHQ.Core.HQ.SkillListID.ContainsKey(nPilot.TrainingSkillID) = True Then
                            Dim mySkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(nPilot.TrainingSkillID)
                            Dim ctime As Integer = CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(nPilot, mySkill, nPilot.TrainingSkillLevel, -1))
                            If Math.Abs(ctime - nPilot.TrainingCurrentTime) > 5 Then
                                nTime += CInt(EveHQ.Core.SkillFunctions.CalcTimeToLevel(nPilot, mySkill, nPilot.TrainingSkillLevel, -1))
                            Else
                                nTime += nPilot.TrainingCurrentTime
                            End If
                        End If
                    End If
                End If
                lblRevisedQueueTime.Text = "Revised Time: " & EveHQ.Core.SkillFunctions.TimeToString(nTime)
                Dim pointScores(4, 1) As Long
                For a As Integer = 0 To 4
                    pointScores(a, 0) = a
                Next
                For Each skill As EveHQ.Core.SortedQueueItem In nQueue
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
                Dim myComparer As New EveHQ.Core.Reports.RectangularComparer(pointScores)
                Array.Sort(tagArray, myComparer)
                Array.Reverse(tagArray)
                For att As Integer = 0 To 4
                    Select Case tagArray(att)
                        Case 0
                            Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Charisma:"
                        Case 1
                            Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Intelligence:"
                        Case 2
                            Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Memory:"
                        Case 3
                            Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Perception:"
                        Case 4
                            Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = "Willpower:"
                    End Select
                    Me.gpSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = pointScores(tagArray(att), 1).ToString("N2")
                Next
                If nTime <= iTime Then
                    lblTimeSaving.Text = "Time Saving: " & EveHQ.Core.SkillFunctions.TimeToString(iTime - nTime, False)
                Else
                    lblTimeSaving.Text = "Time Loss: " & EveHQ.Core.SkillFunctions.TimeToString(nTime - iTime, False)
                End If
            Else
                If Me.IsHandleCreated = True Then
                    lblActiveSkillQueue.Text = "No Queue Selected"
                    lblSkillQueuePointsAnalysis.Text = ""
                    For att As Integer = 0 To 4
                        Me.gpSkillQueue.Controls("lblAttribute" & (att + 1).ToString).Text = ""
                        Me.gpSkillQueue.Controls("lblAttributePoints" & (att + 1).ToString).Text = ""
                    Next
                    lblActiveQueueTime.Text = ""
                    lblRevisedQueueTime.Text = ""
                    lblTimeSaving.Text = ""
                End If
            End If
        End Sub

        Private Sub SyncTraining()
            nPilot.Training = iPilot.Training
            nPilot.TrainingCurrentSP = iPilot.TrainingCurrentSP
            nPilot.TrainingCurrentTime = iPilot.TrainingCurrentTime
            nPilot.TrainingSkillID = iPilot.TrainingSkillID
            nPilot.TrainingSkillName = iPilot.TrainingSkillName
            nPilot.TrainingSkillLevel = iPilot.TrainingSkillLevel
            nPilot.TrainingStartSP = iPilot.TrainingStartSP
            nPilot.TrainingEndSP = iPilot.TrainingEndSP
            nPilot.TrainingStartTime = iPilot.TrainingStartTime
            nPilot.TrainingEndTime = iPilot.TrainingEndTime
        End Sub

        Private Sub nudIImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIImplant.ValueChanged
            If UpdateAllBases = False Then
                nPilot.IImplant = CInt(nudIImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nPilot.IImplant = CInt(nudIImplant.Value)
            End If
        End Sub

        Private Sub nudPImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPImplant.ValueChanged
            If UpdateAllBases = False Then
                nPilot.PImplant = CInt(nudPImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nPilot.PImplant = CInt(nudPImplant.Value)
            End If
        End Sub

        Private Sub nudCImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCImplant.ValueChanged
            If UpdateAllBases = False Then
                nPilot.CImplant = CInt(nudCImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nPilot.CImplant = CInt(nudCImplant.Value)
            End If
        End Sub

        Private Sub nudWImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudWImplant.ValueChanged
            If UpdateAllBases = False Then
                nPilot.WImplant = CInt(nudWImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nPilot.WImplant = CInt(nudWImplant.Value)
            End If
        End Sub

        Private Sub nudMImplant_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMImplant.ValueChanged
            If UpdateAllBases = False Then
                nPilot.MImplant = CInt(nudMImplant.Value)
                Call RecalcAttributes()
                Call DisplayAtributes()
                Call DisplayQueueInfo()
            Else
                nPilot.MImplant = CInt(nudMImplant.Value)
            End If
        End Sub

        Private Sub btnResetImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetImplants.Click
            UpdateAllBases = True
            nPilot.IImplant = iPilot.IImplant
            nPilot.PImplant = iPilot.PImplant
            nPilot.CImplant = iPilot.CImplant
            nPilot.WImplant = iPilot.WImplant
            nPilot.MImplant = iPilot.MImplant
            Call RecalcAttributes()
            Call DisplayAtributes()
            Call DisplayQueueInfo()
            UpdateAllBases = False
        End Sub
    End Class
End NameSpace