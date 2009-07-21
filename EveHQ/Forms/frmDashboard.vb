Public Class frmDashboard

#Region "Form Loading Routines"

    Private Sub frmDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the Panel colour
        Me.FLP1.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DBColor))

        ' Add the controls to the FLP
        Dim myDBCPilotInfo1 As New DBCPilotInfo
        myDBCPilotInfo1.PilotName = "Vessper"
        FLP1.Controls.Add(myDBCPilotInfo1)

        ' Add a handler to the controls
        For Each control As Control In FLP1.Controls
            AddHandler control.MouseDown, AddressOf MyMouseDown
            For Each subcontrol As Control In control.Controls
                AddHandler subcontrol.MouseDown, AddressOf MyMouseDown
            Next
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
#End Region

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