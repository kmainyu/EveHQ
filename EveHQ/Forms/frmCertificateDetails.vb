Public Class frmCertificateDetails

    Dim CertGrades() As String = New String() {"", "Basic", "Standard", "Improved", "Advanced", "Elite"}
    Dim cDisplayPilotName As String
    Dim displayPilot As New EveHQ.Core.Pilot
    Public Property DisplayPilotName() As String
        Get
            Return cDisplayPilotName
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            DisplayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(value), Core.Pilot)
        End Set
    End Property

    Public Sub ShowCertDetails(ByVal certID As String)

        Dim cCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(certID), Core.Certificate)
        Me.Text = CType(EveHQ.Core.HQ.CertificateClasses(cCert.ClassID.ToString), EveHQ.Core.CertificateClass).Name & " (" & CertGrades(cCert.Grade) & ")"
        Call Me.PrepareDescription(certID)
        Call Me.PrepareTree(certID)
        Call Me.PrepareCerts(certID)
        Call Me.PrepareDepends(certID)

        If Me.IsHandleCreated = False Then
            Me.Show()
        Else
            Me.BringToFront()
        End If

    End Sub

    Private Sub PrepareDescription(ByVal certID As String)
        Dim cCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(certID), Core.Certificate)
        Me.lblDescription.Text = cCert.Description
    End Sub

    Private Sub PrepareCerts(ByVal certID As String)
        Dim cCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(certID), Core.Certificate)
        Dim cRCert As EveHQ.Core.Certificate
        Dim newCert As New ListViewItem
        lvwCerts.BeginUpdate()
        lvwCerts.Items.Clear()
        For Each cReqCert As String In cCert.RequiredCerts.Keys
            cRCert = CType(EveHQ.Core.HQ.Certificates(cReqCert), Core.Certificate)
            newCert = New ListViewItem
            newCert.Text = CType(EveHQ.Core.HQ.CertificateClasses(cRCert.ClassID.ToString), EveHQ.Core.CertificateClass).Name
            newCert.Name = cRCert.ID.ToString
            newCert.SubItems.Add(CertGrades(cRCert.Grade))
            If displayPilot.Certificates.Contains(cRCert.ID.ToString) = True Then
                newCert.ForeColor = Color.Green
            Else
                newCert.ForeColor = Color.Red
            End If
            lvwCerts.Items.Add(newCert)
        Next
        lvwCerts.EndUpdate()
    End Sub

    Private Sub PrepareTree(ByVal certID As String)
        Dim cCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(certID), Core.Certificate)
        tvwReqs.BeginUpdate()
        tvwReqs.Nodes.Clear()

        For Each skillID As String In cCert.RequiredSkills.Keys

            Dim level As Integer = 1
            Dim pointer(20) As Integer
            Dim parent(20) As Integer
            Dim skillName(20) As String
            Dim skillLevel(20) As String
            pointer(level) = 1
            parent(level) = CInt(skillID)

            Dim strTree As String = ""
            Dim cSkill As EveHQ.Core.EveSkill = CType(EveHQ.Core.HQ.SkillListID(skillID), Core.EveSkill)
            Dim curSkill As Integer = CInt(skillID)
            Dim curLevel As Integer = CInt(cCert.RequiredSkills(skillID))
            Dim counter As Integer = 0
            Dim curNode As TreeNode = New TreeNode

            ' Write the skill we are querying as the first (parent) node
            curNode.Text = cSkill.Name & " (Level " & CStr(cCert.RequiredSkills(skillID)) & ")"
            Dim skillTrained As Boolean = False
            Dim myLevel As Integer = 0
            skillTrained = False
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
                If displayPilot.PilotSkills.Contains(cSkill.Name) Then
                    Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                    mySkill = CType(displayPilot.PilotSkills(cSkill.Name), Core.PilotSkill)
                    myLevel = CInt(mySkill.Level)
                    If myLevel >= curLevel Then skillTrained = True
                    If skillTrained = True Then
                        curNode.ForeColor = Color.LimeGreen
                        curNode.ToolTipText = "Already Trained"
                    Else
                        Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
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
                    Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
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
                Dim subSkill As EveHQ.Core.EveSkill
                For Each subSkillID As String In cSkill.PreReqSkills.Keys
                    subSkill = CType(EveHQ.Core.HQ.SkillListID(subSkillID), EveHQ.Core.EveSkill)
                    Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode)
                Next
            End If
        Next
        tvwReqs.ExpandAll()
        tvwReqs.EndUpdate()
    End Sub

    Private Sub AddPreReqsToTree(ByVal newSkill As EveHQ.Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode)
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        Dim newNode As TreeNode = New TreeNode
        newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
        newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
        ' Check status of this skill
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
            skillTrained = False
            myLevel = 0
            If displayPilot.PilotSkills.Contains(newSkill.Name) Then
                Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                mySkill = CType(displayPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
            End If
            If skillTrained = True Then
                newNode.ForeColor = Color.LimeGreen
                newNode.ToolTipText = "Already Trained"
            Else
                Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, newSkill.Name, curLevel)
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
        curNode = newNode

        If newSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As EveHQ.Core.EveSkill
            For Each subSkillID As String In newSkill.PreReqSkills.Keys
                subSkill = CType(EveHQ.Core.HQ.SkillListID(subSkillID), EveHQ.Core.EveSkill)
                Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode)
            Next
        End If
    End Sub

    Private Sub PrepareDepends(ByVal certID As String)
        ' Add the certificate unlocks
        lvwDepend.BeginUpdate()
        lvwDepend.Items.Clear()
        Dim certUnlocks As ArrayList = CType(EveHQ.Core.HQ.CertUnlockCerts(certID), ArrayList)
        If certUnlocks IsNot Nothing Then
            For Each item As String In certUnlocks
                Dim itemGrade As String = ""
                Dim newItem As New ListViewItem
                newItem.Group = lvwDepend.Groups("CatCerts")
                Dim cert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(item), Core.Certificate)
                Dim certName As String = CType(EveHQ.Core.HQ.CertificateClasses(cert.ClassID.ToString), EveHQ.Core.CertificateClass).Name
                Dim certGrade As String = CertGrades(cert.Grade)
                For Each reqCertID As String In cert.RequiredCerts.Keys
                    Dim reqCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(reqCertID), Core.Certificate)
                    If reqCert.ID.ToString <> certID Then
                        newItem.ToolTipText &= CType(EveHQ.Core.HQ.CertificateClasses(reqCert.ClassID.ToString), EveHQ.Core.CertificateClass).Name
                        newItem.ToolTipText &= " (" & CertGrades(reqCert.Grade) & "), "
                    End If
                Next
                If newItem.ToolTipText <> "" Then
                    newItem.ToolTipText = "Also Requires: " & newItem.ToolTipText
                    newItem.ToolTipText = newItem.ToolTipText.TrimEnd(", ".ToCharArray)
                End If
                If displayPilot.Certificates.Contains(cert.ID.ToString) = True Then
                    newItem.ForeColor = Color.Green
                Else
                    newItem.ForeColor = Color.Red
                End If
                newItem.Text = certName
                newItem.Name = cert.ID.ToString
                newItem.SubItems.Add(certGrade)
                newItem.Name = item
                lvwDepend.Items.Add(newItem)
            Next
        End If
        lvwDepend.EndUpdate()
    End Sub

    Private Sub ctxSkills_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSkills.Opening
        Dim curNode As TreeNode = New TreeNode
        curNode = tvwReqs.SelectedNode
        Dim skillName As String = ""
        Dim skillID As String = ""
        skillName = curNode.Text
        If InStr(skillName, "(Level") <> 0 Then
            skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
        End If
        skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
        mnuSkillName.Text = skillName
        mnuSkillName.Tag = skillID
    End Sub

    Private Sub mnuViewSkillDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewSkillDetails.Click
        Dim skillID As String = mnuSkillName.Tag.ToString
        frmSkillDetails.DisplayPilotName = cDisplayPilotName
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub

    Private Sub tvwReqs_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwReqs.NodeMouseClick
        tvwReqs.SelectedNode = e.Node
    End Sub

    Private Sub ctxCerts_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxCerts.Opening
        Dim curNode As New ListViewItem
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

    Private Sub mnuViewCertDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewCertDetails.Click
        Dim certID As String = mnuCertName.Tag.ToString
        Dim cCert As EveHQ.Core.Certificate = CType(EveHQ.Core.HQ.Certificates(certID), Core.Certificate)
        Me.Text = CType(EveHQ.Core.HQ.CertificateClasses(cCert.ClassID.ToString), EveHQ.Core.CertificateClass).Name & " (" & CertGrades(cCert.Grade) & ")"
        Call Me.PrepareDescription(certID)
        Call Me.PrepareTree(certID)
        Call Me.PrepareCerts(certID)
        Call Me.PrepareDepends(certID)
    End Sub

End Class