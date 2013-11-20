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
Imports DevComponents.DotNetBar
Imports EveHQ.EveData
Imports EveHQ.Core
Imports EveHQ.Common.Extensions
Imports DevComponents.AdvTree
Imports EveHQ.Core.ItemBrowser

Namespace Forms

    Public Class FrmCertificateDetails

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
            Call PrepareImage(certID)
            Call PrepareDescription(certID)
            Call PrepareTree(certID)
            Call PrepareRecommendations(certID)

            If IsHandleCreated = False Then
                Show()
            Else
                BringToFront()
            End If
        End Sub

        Private Sub PrepareImage(certID As Integer)
            Dim certGrade As Integer = 0
            If _displayPilot.QualifiedCertificates.ContainsKey(certID) = True Then
                certGrade = _displayPilot.QualifiedCertificates(certID)
            End If
            riCert.Image = CType(My.Resources.ResourceManager.GetObject("Cert" & certGrade.ToString), Image)
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

                Dim currentTreeView As TreeView
                Select Case grade
                    Case CertificateGrade.Basic
                        currentTreeView = tvwBasicReqs
                    Case CertificateGrade.Standard
                        currentTreeView = tvwStandardReqs
                    Case CertificateGrade.Improved
                        currentTreeView = tvwImprovedReqs
                    Case CertificateGrade.Advanced
                        currentTreeView = tvwAdvancedReqs
                    Case CertificateGrade.Elite
                        currentTreeView = tvwEliteReqs
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
                    currentTreeView.Nodes.Add(curNode)

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

        Private Sub PrepareRecommendations(certID As Integer)
            Dim certGroupStyle As ElementStyle = adtShips.Styles("SkillGroup").Copy
            certGroupStyle.BackColor = Color.FromArgb(CInt(HQ.Settings.PilotGroupBackgroundColor))
            certGroupStyle.BackColor2 = Color.Black
            certGroupStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotGroupTextColor))
            Dim selSkillStyle As ElementStyle = adtShips.Styles("Skill").Copy
            selSkillStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotSkillHighlightColor))
            selSkillStyle.BackColor = Color.FromArgb(32, selSkillStyle.BackColor2)

            Dim groupNodes As New SortedList(Of String, Node) ' groupName, groupNode
            Dim cert As Certificate = StaticData.Certificates(certID)
            adtShips.BeginUpdate()
            adtShips.Nodes.Clear()
            For Each shipID As Integer In cert.RecommendedTypes
                Dim ship As EveType = StaticData.Types(shipID)
                Dim shipNode As New Node(ship.Name)
                shipNode.FullRowBackground = True
                shipNode.StyleSelected = selSkillStyle
                If groupNodes.ContainsKey(StaticData.TypeGroups(ship.Group)) = False Then
                    ' Create a new node
                    Dim groupNode As New Node(StaticData.TypeGroups(ship.Group))
                    groupNode.Style = certGroupStyle
                    groupNode.FullRowBackground = True
                    groupNodes.Add(groupNode.Text, groupNode)
                End If
                ' Add the ship to the group node
                groupNodes(StaticData.TypeGroups(ship.Group)).Nodes.Add(shipNode)
                shipNode.Image = ImageHandler.GetImage(shipID, 32)
                shipNode.ContextMenu = ctxShips
            Next
            ' Add all the group nodes
            For Each groupNode As Node In groupNodes.Values
                adtShips.Nodes.Add(groupNode)
                groupNode.Text &= " [" & groupNode.Nodes.Count.ToString & "]"
            Next
            adtShips.EndUpdate()
            ' Hide the tab if no recommendations
            If adtShips.Nodes.Count = 0 Then
                recommendedTab.Visible = False
            Else
                recommendedTab.Visible = True
            End If
        End Sub

        Private Sub ctxSkills_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxSkills.Opening
            Dim ctx As ContextMenuStrip = CType(sender, ContextMenuStrip)
            Dim curNode As TreeNode = CType(ctx.SourceControl, TreeView).SelectedNode
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
            FrmSkillDetails.DisplayPilotName = _displayPilotName
            Call FrmSkillDetails.ShowSkillDetails(skillID)
        End Sub

        Private Sub tvwBasicReqs_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwBasicReqs.NodeMouseClick
            tvwBasicReqs.SelectedNode = e.Node
        End Sub

        Private Sub tvwStandardReqs_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwStandardReqs.NodeMouseClick
            tvwStandardReqs.SelectedNode = e.Node
        End Sub

        Private Sub tvwImprovedReqs_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwImprovedReqs.NodeMouseClick
            tvwImprovedReqs.SelectedNode = e.Node
        End Sub

        Private Sub tvwAdvancedReqs_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwAdvancedReqs.NodeMouseClick
            tvwAdvancedReqs.SelectedNode = e.Node
        End Sub

        Private Sub tvwEliteReqs_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles tvwEliteReqs.NodeMouseClick
            tvwEliteReqs.SelectedNode = e.Node
        End Sub

        Private Sub mnuShowInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuShowInfo.Click
            Dim typeID As Integer = CInt(mnuShipName.Tag)
            Using myIB As New FrmIB(typeID)
                myIB.ShowDialog()
            End Using
        End Sub

        Private Sub adtShips_NodeClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtShips.NodeClick
            If e.Node.Level = 0 Then
                e.Node.Toggle()
            End If
        End Sub

        Private Sub ctxShips_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ctxShips.Opening
            Dim ctx As ContextMenuStrip = CType(sender, ContextMenuStrip)
            Dim selNode As Node = CType(ctx.SourceControl, AdvTree).SelectedNode
            If selNode IsNot Nothing Then
                mnuShipName.Text = selNode.Text
                mnuShipName.Tag = StaticData.TypeNames(selNode.Text)
            Else
                e.Cancel = True
            End If
        End Sub
    End Class
End Namespace