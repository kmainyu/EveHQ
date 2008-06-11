
' An instance of this class is created by the designer for the XPanderList control at
' design time.  That instance controls the behavior of the control at design-time.
Public Class XPanderListDesigner
    Inherits System.Windows.Forms.Design.ParentControlDesigner

    Private _borderPen As New Pen(Color.FromKnownColor(KnownColor.ControlDarkDark))
    Private _control As XPanderList

    Public Sub New()
        _borderPen.DashStyle = Drawing.Drawing2D.DashStyle.Dash
    End Sub

    Public Overrides Sub Initialize(ByVal component As System.ComponentModel.IComponent)
        MyBase.Initialize(component)
        _control = CType(Me.Control, XPanderList)

        ' Disable the autoscroll feature for the control during design time.  The control
        ' itself sets this property to true when it initializes at run time.  Trying to position
        ' controls in this control with the autoscroll property set to True is problematic.
        _control.AutoScroll = False
    End Sub

    Protected Overrides Sub OnPaintAdornments(ByVal pe As PaintEventArgs)
        MyBase.OnPaintAdornments(pe)
        pe.Graphics.DrawRectangle(_borderPen, 0, 0, _control.Width - 2, _control.Height - 2)
    End Sub
End Class
