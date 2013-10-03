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
Imports System.Text
Imports EveHQ.EveData

Public Class frmCertificateDetails

    ReadOnly _certGrades() As String = New String() {"", "Basic", "Standard", "Improved", "Advanced", "Elite"}
    Dim _displayPilotName As String
    Dim _displayPilot As New Core.EveHQPilot
    Public Property DisplayPilotName() As String
        Get
            Return _displayPilotName
        End Get
        Set(ByVal value As String)
            _displayPilotName = value
            _displayPilot = Core.HQ.Settings.Pilots(value)
        End Set
    End Property

    Public Sub ShowCertDetails(ByVal certID As Integer)

        Dim cCert As Certificate = StaticData.Certificates(certID)
        Text = StaticData.CertificateClasses(cCert.ClassId.ToString).Name & " (" & _certGrades(cCert.Grade) & ")"
        Call PrepareDescription(certID)
        Call PrepareTree(certID)
        Call PrepareCerts(certID)
        Call PrepareDepends(certID)

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

    Private Sub PrepareCerts(ByVal certID As Integer)
        Dim cCert As Certificate = StaticData.Certificates(certID)
        Dim cRCert As Certificate
        Dim newCert As ListViewItem
        lvwCerts.BeginUpdate()
        lvwCerts.Items.Clear()
        For Each cReqCert As Integer In cCert.RequiredCertificates.Keys
            cRCert = StaticData.Certificates(cReqCert)
            newCert = New ListViewItem
            newCert.Text = StaticData.CertificateClasses(cRCert.ClassId.ToString).Name
            newCert.Name = cRCert.Id.ToString
            newCert.SubItems.Add(_certGrades(cRCert.Grade))
            If _displayPilot.Certificates.Contains(cRCert.Id) = True Then
                newCert.ForeColor = Color.Green
            Else
                newCert.ForeColor = Color.Red
            End If
            lvwCerts.Items.Add(newCert)
        Next
        lvwCerts.EndUpdate()
    End Sub

    Private Sub PrepareTree(ByVal certID As Integer)
        Dim cCert As Certificate = StaticData.Certificates(certID)
        tvwReqs.BeginUpdate()
        tvwReqs.Nodes.Clear()

        For Each skillID As Integer In cCert.RequiredSkills.Keys

            Const level As Integer = 1
            Dim pointer(20) As Integer
            Dim parentItem(20) As Integer
            pointer(level) = 1
            parentItem(level) = CInt(skillID)

            Dim cSkill As Core.EveSkill = Core.HQ.SkillListID(skillID)
            Dim curLevel As Integer = CInt(cCert.RequiredSkills(skillID))
            Dim curNode As TreeNode = New TreeNode

            ' Write the skill we are querying as the first (parent) node
            curNode.Text = cSkill.Name & " (Level " & CStr(cCert.RequiredSkills(skillID)) & ")"
            Dim skillTrained As Boolean
            Dim myLevel As Integer
            skillTrained = False
            If Core.HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
                If _displayPilot.PilotSkills.ContainsKey(cSkill.Name) Then
                    Dim mySkill As Core.EveHQPilotSkill
                    mySkill = _displayPilot.PilotSkills(cSkill.Name)
                    myLevel = CInt(mySkill.Level)
                    If myLevel >= curLevel Then skillTrained = True
                    If skillTrained = True Then
                        curNode.ForeColor = Color.LimeGreen
                        curNode.ToolTipText = "Already Trained"
                    Else
                        Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, curLevel)
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
                    Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(_displayPilot, cSkill.Name, curLevel)
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
            tvwReqs.Nodes.Add(curNode)

            If cSkill.PreReqSkills.Count > 0 Then
                Dim subSkill As Core.EveSkill
                For Each subSkillID As Integer In cSkill.PreReqSkills.Keys
                    subSkill = Core.HQ.SkillListID(subSkillID)
                    Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                Next
            End If
        Next
        tvwReqs.ExpandAll()
        tvwReqs.EndUpdate()
    End Sub

    Private Sub AddPreReqsToTree(ByVal newSkill As Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode)
        Dim skillTrained As Boolean
        Dim newNode As TreeNode = New TreeNode
        newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
        newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
        ' Check status of this skill
        If Core.HQ.Settings.Pilots.Count > 0 And _displayPilot.Updated = True Then
            skillTrained = False
            Dim myLevel As Integer
            If _displayPilot.PilotSkills.ContainsKey(newSkill.Name) Then
                Dim mySkill As Core.EveHQPilotSkill = _displayPilot.PilotSkills(newSkill.Name)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
            End If
            If skillTrained = True Then
                newNode.ForeColor = Color.LimeGreen
                newNode.ToolTipText = "Already Trained"
            Else
                Dim planLevel As Integer = Core.SkillQueueFunctions.IsPlanned(_displayPilot, newSkill.Name, curLevel)
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
            Dim subSkill As Core.EveSkill
            For Each subSkillID As Integer In newSkill.PreReqSkills.Keys
                subSkill = Core.HQ.SkillListID(subSkillID)
                Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
            Next
        End If
    End Sub

    Private Sub PrepareDepends(ByVal certID As Integer)
        ' Add the certificate unlocks
        lvwDepend.BeginUpdate()
        lvwDepend.Items.Clear()
        If StaticData.CertUnlockCertificates.ContainsKey(certID) = True Then
            Dim certUnlocks As List(Of Integer) = StaticData.CertUnlockCertificates(certID)
            If certUnlocks IsNot Nothing Then
                For Each item As Integer In certUnlocks
                    Dim newItem As New ListViewItem
                    Dim toolTipText As New StringBuilder
                    newItem.Group = lvwDepend.Groups("CatCerts")
                    Dim cert As Certificate = StaticData.Certificates(item)
                    Dim certName As String = StaticData.CertificateClasses(cert.ClassId.ToString).Name
                    Dim certGrade As String = _certGrades(cert.Grade)
                    For Each reqCertID As Integer In cert.RequiredCertificates.Keys
                        Dim requiredCert As Certificate = StaticData.Certificates(reqCertID)
                        If requiredCert.Id <> certID Then
                            toolTipText.Append(StaticData.CertificateClasses(requiredCert.ClassId.ToString).Name)
                            toolTipText.Append(" (")
                            toolTipText.Append(_certGrades(requiredCert.Grade))
                            toolTipText.Append("), ")
                        End If
                    Next
                    If toolTipText.Length > 0 Then
                        toolTipText.Insert(0, "Also Requires: ")

                        If (toolTipText.ToString().EndsWith(", ")) Then
                            toolTipText.Remove(toolTipText.Length - 2, 2)
                        End If
                    End If

                    If _displayPilot.Certificates.Contains(cert.Id) = True Then
                        newItem.ForeColor = Color.Green
                    Else
                        newItem.ForeColor = Color.Red
                    End If

                    newItem.ToolTipText = toolTipText.ToString()
                    newItem.Text = certName
                    newItem.Name = cert.Id.ToString
                    newItem.SubItems.Add(certGrade)
                    newItem.Name = CStr(item)
                    lvwDepend.Items.Add(newItem)
                Next
            End If
        End If
        lvwDepend.EndUpdate()
    End Sub

    Private Sub ctxSkills_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSkills.Opening
        Dim curNode As TreeNode
        curNode = tvwReqs.SelectedNode
        Dim skillName As String
        Dim skillID As Integer
        skillName = curNode.Text
        If InStr(skillName, "(Level") <> 0 Then
            skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
        End If
        skillID = Core.SkillFunctions.SkillNameToID(skillName)
        mnuSkillName.Text = skillName
        mnuSkillName.Tag = skillID
    End Sub

    Private Sub mnuViewSkillDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewSkillDetails.Click
        Dim skillID As Integer = CInt(mnuSkillName.Tag)
        frmSkillDetails.DisplayPilotName = _displayPilotName
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub

    Private Sub tvwReqs_NodeMouseClick(ByVal sender As Object, ByVal e As Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwReqs.NodeMouseClick
        tvwReqs.SelectedNode = e.Node
    End Sub

    Private Sub ctxCerts_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxCerts.Opening
        If ctxCerts.SourceControl.Name = "lvwCerts" Then
            If lvwCerts.SelectedItems.Count > 0 Then
                mnuCertName.Text = lvwCerts.SelectedItems(0).Text & " (" & lvwCerts.SelectedItems(0).SubItems(1).Text & ")"
                mnuCertName.Tag = lvwCerts.SelectedItems(0).Name
            End If
        Else
            If lvwDepend.SelectedItems.Count > 0 Then
                mnuCertName.Text = lvwDepend.SelectedItems(0).Text & " (" & lvwDepend.SelectedItems(0).SubItems(1).Text & ")"
                mnuCertName.Tag = lvwDepend.SelectedItems(0).Name
            End If
        End If
    End Sub

    Private Sub mnuViewCertDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewCertDetails.Click
        Dim certID As Integer = CInt(mnuCertName.Tag)
        Dim cCert As Certificate = StaticData.Certificates(certID)
        Text = StaticData.CertificateClasses(cCert.ClassId.ToString).Name & " (" & _certGrades(cCert.Grade) & ")"
        Call PrepareDescription(certID)
        Call PrepareTree(certID)
        Call PrepareCerts(certID)
        Call PrepareDepends(certID)
    End Sub

End Class