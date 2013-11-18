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
Imports System.ComponentModel
Imports EveHQ.EveData
Imports EveHQ.Core
Imports System.Text
Imports EveHQ.Common.Extensions

Namespace Forms

    Public Class FrmCertificateDetails

        ReadOnly _certGrades() As String = New String() {"", "Basic", "Standard", "Improved", "Advanced", "Elite"}
        Dim _displayPilotName As String
        Dim _displayPilot As New EveHQPilot
        Public Property DisplayPilotName() As String
            Get
                Return _displayPilotName
            End Get
            Set(ByVal value As String)
                _displayPilotName = value
                _displayPilot = HQ.Settings.Pilots(value)
            End Set
        End Property

        Public Sub ShowCertDetails(ByVal certID As Integer)

            Dim cCert As Certificate = StaticData.Certificates(certID)
            Text = cCert.Name
            Call PrepareDescription(certID)
            Call PrepareTree(certID)

            If IsHandleCreated = False Then
                Show()
            Else
                BringToFront()
            End If

        End Sub

        Private Sub PrepareDescription(ByVal certID As Integer)
            Dim cCert As Certificate = StaticData.Certificates(certID)
            lblDescription.Text = cCert.Description
        End Sub

        Private Sub PrepareTree(ByVal certID As Integer)
            Dim cCert As Certificate = StaticData.Certificates(certID)
            tvwBasicReqs.BeginUpdate()
            tvwBasicReqs.Nodes.Clear()

            For Each grade As CertificateGrade In cCert.GradesAndSkills.Keys

                Dim currentTreeVite As TreeView
                Select Case grade
                    Case CertificateGrade.Basic
                        currentTreeVite = tvwBasicReqs
                    Case CertificateGrade.Standard
                        currentTreeVite = tvwStandardReqs
                    Case CertificateGrade.Improved
                        currentTreeVite = tvwImprovedReqs
                    Case CertificateGrade.Advanced
                        currentTreeVite = tvwAdvancedReqs
                    Case CertificateGrade.Elite
                        currentTreeVite = tvwEliteReqs
                    Case Else
                        Throw New InvalidOperationException("Invalid Certificate grade data was found when processing '{0}'".FormatInvariant(cCert.Name))
                End Select


                For Each skillID As Integer In cCert.GradesAndSkills(grade).Keys
                    Const Level As Integer = 1
                    Dim pointer(20) As Integer
                    Dim parentItem(20) As Integer
                    pointer(Level) = 1
                    parentItem(Level) = CInt(skillID)

                    Dim cSkill As EveSkill = HQ.SkillListID(skillID)
                    Dim curLevel As Integer = CInt(cCert.GradesAndSkills(grade)(skillID))
                    Dim curNode As TreeNode = New TreeNode

                    ' Write the skill we are querying as the first (parent) node
                    curNode.Text = cSkill.Name & " (Level " & CStr(cCert.GradesAndSkills(grade)(skillID)) & ")"
                    Dim skillTrained As Boolean
                    Dim myLevel As Integer
                    skillTrained = False
                    If HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                        If _displayPilot.PilotSkills.ContainsKey(cSkill.Name) Then
                            Dim mySkill As EveHQPilotSkill
                            mySkill = _displayPilot.PilotSkills(cSkill.Name)
                            myLevel = CInt(mySkill.Level)
                            If myLevel >= curLevel Then skillTrained = True
                            If skillTrained = True Then
                                curNode.ForeColor = Color.LimeGreen
                                curNode.ToolTipText = "Already Trained"
                            Else
                                Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, curLevel)
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
                            Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, curLevel)
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
                    currentTreeVite.Nodes.Add(curNode)

                    If cSkill.PreReqSkills.Count > 0 Then
                        Dim subSkill As EveSkill
                        For Each subSkillID As Integer In cSkill.PreReqSkills.Keys
                            subSkill = HQ.SkillListID(subSkillID)
                            Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                        Next
                    End If
                Next
            Next
            tvwBasicReqs.ExpandAll()
            tvwStandardReqs.ExpandAll()
            tvwImprovedReqs.ExpandAll()
            tvwAdvancedReqs.ExpandAll()
            tvwEliteReqs.ExpandAll()
            tvwBasicReqs.EndUpdate()
        End Sub

        Private Sub AddPreReqsToTree(ByVal newSkill As EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode)
            Dim skillTrained As Boolean
            Dim newNode As TreeNode = New TreeNode
            newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
            newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
            ' Check status of this skill
            If HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                skillTrained = False
                Dim myLevel As Integer
                If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) Then
                    Dim mySkill As EveHQPilotSkill = _displayPilot.PilotSkills(newSkill.Name)
                    myLevel = CInt(mySkill.Level)
                    If myLevel >= curLevel Then skillTrained = True
                End If
                If skillTrained = True Then
                    newNode.ForeColor = Color.LimeGreen
                    newNode.ToolTipText = "Already Trained"
                Else
                    Dim planLevel As Integer = SkillQueueFunctions.IsPlanned(_displayPilot, newSkill.Name, curLevel)
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

            If newSkill.PreReqSkills.Count > 0 Then
                Dim subSkill As EveSkill
                For Each subSkillID As Integer In newSkill.PreReqSkills.Keys
                    subSkill = HQ.SkillListID(subSkillID)
                    Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
                Next
            End If
        End Sub

    

        Private Sub ctxSkills_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxSkills.Opening
            Dim curNode As TreeNode
            curNode = tvwBasicReqs.SelectedNode
            Dim skillName As String
            Dim skillID As Integer
            skillName = curNode.Text
            If InStr(skillName, "(Level") <> 0 Then
                skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
            End If
            skillID = SkillFunctions.SkillNameToID(skillName)
            mnuSkillName.Text = skillName
            mnuSkillName.Tag = skillID
        End Sub

        Private Sub mnuViewSkillDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewSkillDetails.Click
            Dim skillID As Integer = CInt(mnuSkillName.Tag)
            frmSkillDetails.DisplayPilotName = _displayPilotName
            Call frmSkillDetails.ShowSkillDetails(skillID)
        End Sub

        Private Sub tvwReqs_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwBasicReqs.NodeMouseClick
            tvwBasicReqs.SelectedNode = e.Node
        End Sub

       

        Private Sub mnuViewCertDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewCertDetails.Click
            Dim certID As Integer = CInt(mnuCertName.Tag)
            Dim cCert As Certificate = StaticData.Certificates(certID)
            Text = cCert.Name
            Call PrepareDescription(certID)
            Call PrepareTree(certID)
           
        End Sub

       
    End Class
End NameSpace