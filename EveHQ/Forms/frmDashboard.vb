Imports System.Reflection

Public Class frmDashboard

#Region "Form Loading Routines"

    Private Sub frmDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the Panel colour
        Me.FLP1.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBColor))

        ' Add the controls to the FLP
        Call Me.UpdateWidgets()
        
    End Sub
#End Region

#Region "Widget Update Routines"

    Public Sub UpdateWidgets()

        FLP1.Controls.Clear()
        FLP1.SuspendLayout()
        For Each config As SortedList(Of String, Object) In EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration
            Dim WidgetName As String = CStr(config("ControlName"))
            Select Case WidgetName
                Case "Pilot Information"
                    Dim newWidget As New DBCPilotInfo
                    newWidget.ControlConfiguration = config
                    FLP1.Controls.Add(newWidget)
            End Select
        Next
        FLP1.ResumeLayout()

        ' Add a handler to the controls
        For Each control As Control In FLP1.Controls
            AddHandler control.MouseDown, AddressOf MyMouseDown
            For Each subcontrol As Control In control.Controls
                AddHandler subcontrol.MouseDown, AddressOf MyMouseDown
            Next
        Next

    End Sub
    Public Sub UpdateDashboardColours()
        Me.FLP1.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBColor))
        For Each c As Control In FLP1.Controls
            Dim mi As System.Reflection.MethodInfo = c.GetType().GetMethod("UpdateColours")
            mi.Invoke(c, Nothing)
        Next
    End Sub
#End Region

#Region "Panel Drag/Drop Routines"

    Private Sub MyMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim source As Control = CType(sender, Control)
        source = source.Parent
        source.DoDragDrop(New MyWrapper(source), DragDropEffects.Move)
    End Sub

    Private Sub FLP1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles FLP1.DragDrop
        Dim wrapper As MyWrapper = CType(e.Data.GetData(GetType(MyWrapper)), MyWrapper)
        Dim source As Control = wrapper.Control

        Dim mousePosition As Point = FLP1.PointToClient(New Point(e.X, e.Y))
        Dim destination As Control = FLP1.GetChildAtPoint(mousePosition)

        Dim indexDestination As Integer = FLP1.Controls.IndexOf(destination)
        FLP1.Controls.SetChildIndex(source, indexDestination)
    End Sub

    Private Sub FLP1_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles FLP1.DragEnter
        If (e.Data.GetDataPresent(GetType(MyWrapper))) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub FLP1_Layout(ByVal sender As Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles FLP1.Layout
        Dim index As Integer = 0
        For Each c As Control In FLP1.Controls
            Dim pi As System.Reflection.PropertyInfo = c.GetType().GetProperty("ControlPosition")
            pi.SetValue(c, index, Nothing)
        Next

        ' If we have a matching count then update the ControlConfiguration details
        If FLP1.Controls.Count = EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Count Then
            EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Clear()
            For Each c As Control In FLP1.Controls
                Dim pi As System.Reflection.PropertyInfo = c.GetType().GetProperty("ControlConfiguration")
                EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Add(pi.GetValue(c, Nothing))
            Next
        End If
    End Sub

#End Region

    Private Sub mnuConfigureDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuConfigureDB.Click
        Dim EveHQSettings As New frmSettings
        EveHQSettings.Tag = "nodeDashboard"
        EveHQSettings.ShowDialog()
        EveHQSettings.Dispose()
    End Sub
End Class

Public Class MyWrapper
    Dim cControl As New Control

    Public Sub New(ByVal control As Control)
        Me.Control = control
    End Sub

    Public Property Control() As Control
        Get
            Return cControl
        End Get
        Set(ByVal value As Control)
            cControl = value
        End Set
    End Property

End Class