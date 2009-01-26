Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class ScrollingMarquee
    Inherits System.Windows.Forms.UserControl

    Friend WithEvents tmrMain As System.Windows.Forms.Timer

    Private startPosition As Single = 0
    Private m_MarqueeText As String = ""
    Private m_LeftToRight As Direction = Direction.Right
    Private m_ScrollSpeed As Integer = 5
    Private m_ShadowColor As Color = Color.White
    Private hDC As IntPtr

    Private Structure tSize
        Dim X As Long
        Dim Y As Long
    End Structure

    Public Enum Direction
        Left
        Right
    End Enum

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        Me.DoubleBuffered = True
        AddHandler MyBase.Paint, AddressOf OnPaint
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.tmrMain = New System.Windows.Forms.Timer(Me.components)
        Me.tmrMain.Enabled = True
        Me.Name = "ScrollingMarquee"
        Me.Size = New System.Drawing.Size(120, 13)
    End Sub

    <Category("Marquee")> _
    <Description("Gets/Sets the text that scrolls accross the marquee.")> _
    Public Property MarqueeText() As String
        Get
            MarqueeText = m_MarqueeText
        End Get
        Set(ByVal Value As String)
            m_MarqueeText = Value
            Invalidate()
        End Set
    End Property

    <Category("Marquee")> _
    <Description("Gets/Sets the direction of the control")> _
    Public Property ScrollLeftToRight() As Direction
        Get
            Return Me.m_LeftToRight
        End Get
        Set(ByVal Value As Direction)
            m_LeftToRight = Value
            Invalidate()
        End Set
    End Property

    <Category("Marquee")> _
    <Description("Gets/Sets the scroll speed of the control. Values can be from 1 to 10.")> _
    Public Property ScrollSpeed() As Integer
        Get
            ScrollSpeed = m_ScrollSpeed
        End Get
        Set(ByVal Value As Integer)

            If Value < 1 Then
                m_ScrollSpeed = 1
            ElseIf Value > 10 Then
                m_ScrollSpeed = 10
            Else
                m_ScrollSpeed = Value
            End If

            Me.tmrMain.Interval = Value * 10
            Invalidate()
        End Set
    End Property

    <Category("Marquee")> _
    <Description("Gets/Sets the color of the shadow text.")> _
    Public Property ShadowColor() As Color
        Get
            ShadowColor = m_ShadowColor
        End Get
        Set(ByVal Value As Color)
            m_ShadowColor = Value
            Invalidate()
        End Set
    End Property

    Private Sub ScrollingMarquee_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
    End Sub

    Private Sub tmrMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMain.Tick
        If m_MarqueeText.Length = 0 Then Exit Sub
        Invalidate()
    End Sub

    Private Sub ScrollingMarquee_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Invalidate()
    End Sub

    Private Sub ScrollingMarquee_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        Invalidate()
    End Sub

    Private Overloads Sub OnPaint(ByVal sender As Object, ByVal e As PaintEventArgs)
        Dim str As String = m_MarqueeText
        Dim g As Graphics = e.Graphics
        Dim szf As SizeF

        g.SmoothingMode = SmoothingMode.HighQuality
        szf = g.MeasureString(m_MarqueeText, Me.Font)

        If m_LeftToRight = Direction.Right Then
            If startPosition > Me.Width Then
                startPosition = CInt(-szf.Width)
            Else
                startPosition += 1
            End If
        ElseIf m_LeftToRight = Direction.Left Then
            If startPosition < -szf.Width Then
                startPosition = CInt(szf.Width)
            Else
                startPosition -= 1
            End If
        End If
        g.DrawString(m_MarqueeText, Me.Font, New SolidBrush(Me.ForeColor), startPosition, CSng(0 + (Me.Height / 2) - (szf.Height / 2)))
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
