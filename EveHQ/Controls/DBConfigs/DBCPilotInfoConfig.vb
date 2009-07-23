Public Class DBCPilotInfoConfig

#Region "Properties"

    Dim cDBWidget As New DBCPilotInfo
    Public Property DBWidget() As DBCPilotInfo
        Get
            Return cDBWidget
        End Get
        Set(ByVal value As DBCPilotInfo)
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
    End Sub

    Private Sub DBCPilotInfoConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load the combo box with the pilot info
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If pilot.Active = True Then
                cboPilots.Items.Add(pilot.Name)
            End If
        Next
        cboPilots.EndUpdate()
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
        End If
        ' Now close the form
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class