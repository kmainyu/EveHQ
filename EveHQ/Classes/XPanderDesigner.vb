Public Class XPanderDesigner
    Inherits System.Windows.Forms.Design.ParentControlDesigner

    Private m_BorderPen As New Pen(Color.FromKnownColor(KnownColor.ControlDarkDark))
    Private m_Control As XPander

    Public Sub New()
        m_BorderPen.DashStyle = Drawing.Drawing2D.DashStyle.Dash
    End Sub

    Public Overrides Sub Initialize(ByVal component As System.ComponentModel.IComponent)
        MyBase.Initialize(component)
        m_Control = CType(Me.Control, XPander)
    End Sub

    Protected Overrides Sub OnPaintAdornments(ByVal pe As PaintEventArgs)
        MyBase.OnPaintAdornments(pe)
        pe.Graphics.DrawRectangle(m_BorderPen, 0, 0, m_Control.Width - 2, m_Control.Height - 2)

        m_Control.ExpandedHeight = m_Control.Height
    End Sub
End Class
