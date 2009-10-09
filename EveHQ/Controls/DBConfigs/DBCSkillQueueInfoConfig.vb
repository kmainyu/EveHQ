Public Class DBCSkillQueueInfoConfig
#Region "Properties"

    Dim cDBWidget As New DBCSkillQueueInfo
    Public Property DBWidget() As DBCSkillQueueInfo
        Get
            Return cDBWidget
        End Get
        Set(ByVal value As DBCSkillQueueInfo)
            cDBWidget = value
            Call SetControlInfo()
        End Set
    End Property

#End Region

    Private Sub SetControlInfo()
        If cDBWidget.ControlWidth < nudWidth.Minimum Then
            cDBWidget.ControlWidth = CInt(nudWidth.Minimum)
        End If
        If cDBWidget.ControlHeight < nudHeight.Minimum Then
            cDBWidget.ControlHeight = CInt(nudHeight.Minimum)
        End If
        nudWidth.Value = cDBWidget.ControlWidth
        nudHeight.Value = cDBWidget.ControlHeight
        If cboPilots.Items.Contains(cDBWidget.DefaultPilotName) = True Then
            cboPilots.SelectedItem = cDBWidget.DefaultPilotName
        Else
            If cboPilots.Items.Count > 0 Then
                cboPilots.SelectedIndex = 0
            End If
        End If
        If cDBWidget.EveQueue = True Then
            radEve.Checked = True
        Else
            radEveHQ.Checked = True
        End If
        If DBWidget.DefaultQueueName <> "" Then
            If cboSkillQueue.Items.Contains(DBWidget.DefaultQueueName) = True Then
                cboSkillQueue.SelectedItem = DBWidget.DefaultQueueName
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Just close the form and do nothing
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Update the control properties
        cDBWidget.ControlWidth = CInt(nudWidth.Value)
        cDBWidget.ControlHeight = CInt(nudHeight.Value)
        If cboPilots.SelectedItem IsNot Nothing Then
            cDBWidget.DefaultPilotName = cboPilots.SelectedItem.ToString
        Else
            MessageBox.Show("You must select a valid Pilot before adding this widget.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If cboSkillQueue.SelectedItem IsNot Nothing Then
            cDBWidget.DefaultQueueName = cboSkillQueue.SelectedItem.ToString
        Else
            cDBWidget.DefaultQueueName = ""
        End If
        cDBWidget.EveQueue = radEve.Checked
        ' Now close the form
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        ' Update the queue information
        Call Me.UpdateQueueList()
    End Sub

    Private Sub UpdateQueueList()
        cboSkillQueue.BeginUpdate()
        cboSkillQueue.Items.Clear()
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilots.SelectedItem.ToString) = True Then
            Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            For Each sq As EveHQ.Core.SkillQueue In cPilot.TrainingQueues.Values
                cboSkillQueue.Items.Add(sq.Name)
            Next
            cboSkillQueue.EndUpdate()
        End If
    End Sub

    Private Sub radEveHQ_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEveHQ.CheckedChanged
        cboSkillQueue.Enabled = radEveHQ.Checked
    End Sub

    Private Sub radEve_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEve.CheckedChanged
        cboSkillQueue.Enabled = Not (radEve.Checked)
    End Sub
End Class