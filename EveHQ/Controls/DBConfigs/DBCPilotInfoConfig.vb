Public Class DBCPilotInfoConfig

#Region "Properties"

    Dim cDBControl As New DBCPilotInfo
    Public Property DBControl() As DBCPilotInfo
        Get
            Return cDBControl
        End Get
        Set(ByVal value As DBCPilotInfo)
            cDBControl = value
            Call SetControlInfo()
        End Set
    End Property

#End Region

    Private Sub SetControlInfo()
        If cDBControl.ControlWidth < nudWidth.Minimum Then
            cDBControl.ControlWidth = CInt(nudWidth.Minimum)
        End If
        If cDBControl.ControlHeight < nudHeight.Minimum Then
            cDBControl.ControlHeight = CInt(nudHeight.Minimum)
        End If
        nudWidth.Value = cDBControl.ControlWidth
        nudHeight.Value = cDBControl.ControlHeight
        If cboPilots.Items.Contains(cDBControl.DefaultPilotName) = True Then
            cboPilots.SelectedItem = cDBControl.DefaultPilotName
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
        cDBControl.ControlWidth = CInt(nudWidth.Value)
        cDBControl.ControlHeight = CInt(nudHeight.Value)
        If cboPilots.SelectedItem IsNot Nothing Then
            cDBControl.DefaultPilotName = cboPilots.SelectedItem.ToString
        End If
        ' Now close the form
        Me.Close()
    End Sub
End Class