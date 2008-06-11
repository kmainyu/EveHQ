' This code has portions "borrowed" from or "inspired" from the following sources
'http://www.codeproject.com/useritems/xpgroupbox.asp
'http://www.codeproject.com/cs/miscctrl/collapsiblepanelbar.asp?target=collapsible
'http://www.codeproject.com/cs/miscctrl/officeline.asp
'http://www.windowsforms.net/default.aspx?tabIndex=7&tabId=44
'
'Credits:
'Daren May, Derek Lakin, Rob Tomson and the people who made TaskVision.
'

Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections

<Designer(GetType(XPanderListDesigner)), _
DesignTimeVisibleAttribute(True)> _
Public Class XPanderList
    Inherits System.Windows.Forms.UserControl

#Region "Constants"
    Private Const c_InitialVerticalSpacing As Integer = 10
    Private Const c_HorizontalSpacing As Integer = 8
    Private Const c_VerticalSpacing As Integer = 14      ' Y gap between XPander controls

    Private Shared ReadOnly c_LightBackColor As Color = Color.FromArgb(123, 162, 239)
    Private Shared ReadOnly c_DarkBackColor As Color = Color.FromArgb(99, 117, 222)
#End Region

#Region " Members"
    ' Private member variables
    Private m_BgColorLight As Color = c_LightBackColor
    Private m_BgColorDark As Color = c_DarkBackColor
    Private m_XPanderComparer As XPanderComparer
    Private m_ControlList As New SortedList(m_XPanderComparer)
    Private m_NextControlKey As Integer = 0
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.ResizeRedraw, False)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'XPanderList
        '
        Me.AutoScroll = True
        Me.Name = "XPanderList"

    End Sub

#End Region

#Region " Public Properties"
    ' Public Properties
    <Description("Light color used in gradient for background."), _
     DefaultValue(GetType(Color), "123,162,239"), _
     Category("Appearance")> _
    Public Property BackColorLight() As Color
        Get
            Return m_BgColorLight
        End Get
        Set(ByVal Value As Color)
            m_BgColorLight = Value
            Invalidate()
        End Set
    End Property
    <Description("Dark color used in gradient for background."), _
     DefaultValue(GetType(Color), "99,117,222"), _
     Category("Appearance")> _
    Public Property BackColorDark() As Color
        Get
            Return m_BgColorDark
        End Get
        Set(ByVal Value As Color)
            m_BgColorDark = Value
            Invalidate()
        End Set
    End Property
#End Region

#Region " Overrides"
    Private Sub XPanderList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackColor = m_BgColorDark
        m_NextControlKey = m_ControlList.Count
        AutoScroll = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)

        Dim rect As Rectangle = New Rectangle(0, AutoScrollPosition.Y, Me.Width, Me.Height)

        Dim b As LinearGradientBrush = New LinearGradientBrush(Me.DisplayRectangle, m_BgColorLight, m_BgColorDark, LinearGradientMode.Vertical)

        pevent.Graphics.FillRectangle(b, Me.DisplayRectangle)
    End Sub
    Private Sub XPanderList_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        Invalidate()
    End Sub
    Protected Overrides Sub OnControlAdded(ByVal e As System.Windows.Forms.ControlEventArgs)
        MyBase.OnControlAdded(e)

        ' We'll only keep track of XPander controls in our list.
        If e.Control.GetType() Is GetType(XPander) Then

            If e.Control.Width <= Me.Width Then
                e.Control.Left = c_HorizontalSpacing
                e.Control.Width = Me.Width - 2 * c_HorizontalSpacing
                e.Control.Top = GetNextTopPosition()
                e.Control.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
            End If

            ' Add a handler so we know when the control is expanded and collapsed
            Dim x As XPander = CType(e.Control, XPander)
            AddHandler x.XPanderCollapsed, AddressOf ControlCollapsed
            AddHandler x.XPanderExpanded, AddressOf ControlExpanded
            AddHandler x.XPanderCollapsing, AddressOf ControlCollapsing
            AddHandler x.XPanderExpanding, AddressOf ControlExpanding
            ' Add the control to our control list so we can keep track of it
            ' Store the key of the control in it's tag property so we can reference(it)
            ' later when we remove it
            e.Control.Tag = m_NextControlKey
            m_NextControlKey += 1
            m_ControlList.Add(e.Control.Tag, e.Control)
        End If
    End Sub
    Protected Overrides Sub OnControlRemoved(ByVal e As System.Windows.Forms.ControlEventArgs)
        Dim ctl As Control
        Dim prevTop As Integer = e.Control.Top
        Dim newTop As Integer = 0

        If e.Control.GetType() Is GetType(XPander) Then
            Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()
            While enumerator.MoveNext
                ctl = CType(enumerator.Value, Control)
                If ctl.Top > prevTop Then
                    newTop = prevTop
                    prevTop = ctl.Top
                    ctl.Top = newTop
                End If
            End While

            Dim x As XPander = CType(e.Control, XPander)

            ' Remove the custom event handlers for this control
            RemoveHandler x.XPanderCollapsed, AddressOf ControlCollapsed
            RemoveHandler x.XPanderExpanded, AddressOf ControlExpanded
            RemoveHandler x.XPanderCollapsing, AddressOf ControlCollapsing
            RemoveHandler x.XPanderExpanding, AddressOf ControlExpanding
            ' Remove the control from the list
            m_ControlList.Remove(e.Control.Tag)
        End If
    End Sub
#End Region

#Region " Private Helpers"
    Private Function GetNextTopPosition() As Integer
        Dim ctl As Control
        Dim max As Integer = c_InitialVerticalSpacing
        Dim YPos As Integer = 0

        ' The next top position is the highest top value + that controls height, with a
        ' little vertical spacing thrown in for good measure
        Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()
        While enumerator.MoveNext
            ctl = CType(enumerator.Value, Control)
            YPos = ctl.Top + ctl.Height
            If YPos > max Then
                max = YPos
            End If
        End While

        If max <> c_InitialVerticalSpacing Then
            max += c_VerticalSpacing
        End If

        Return max
    End Function
    Private Function FixRGB(ByVal RGBValue As Integer) As Integer
        If RGBValue >= 0 And RGBValue <= 255 Then
            Return RGBValue
        ElseIf RGBValue < 0 Then
            Return 0
        Else
            Return 255
        End If
    End Function
#End Region

#Region " Public Methods"
    Public Sub ControlExpanding(ByVal x As XPander, ByVal delta As Integer)
        Dim ctl As Control

        Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()
        While enumerator.MoveNext
            ctl = CType(enumerator.Value, Control)
            If ctl.Top > x.Top Then
                ctl.Top += delta
            End If
        End While
    End Sub
    Public Sub ControlCollapsing(ByVal x As XPander, ByVal delta As Integer)
        Dim ctl As Control

        Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()
        While enumerator.MoveNext
            ctl = CType(enumerator.Value, Control)
            If ctl.Top > x.Top Then
                ctl.Top -= delta
            End If
        End While
    End Sub
    Public Sub ControlExpanded(ByVal x As XPander)
        Dim ctl As Control
        Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()

        While enumerator.MoveNext
            ctl = CType(enumerator.Value, Control)
            If ctl.Top > x.Top Then
                ctl.Top += x.ExpandedHeight - x.CaptionHeight
            End If
        End While
    End Sub
    Public Sub ControlCollapsed(ByVal x As XPander)
        Dim ctl As Control
        Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()
        While enumerator.MoveNext
            ctl = CType(enumerator.Value, Control)
            If ctl.Top > x.Top Then
                ctl.Top -= x.ExpandedHeight - x.CaptionHeight
            End If
        End While

    End Sub
    Public Sub ExpandAll()
        Dim ctl As XPander

        Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()
        While enumerator.MoveNext
            ctl = CType(enumerator.Value, XPander)
            ctl.Expand()
        End While
    End Sub
    Public Sub CollapseAll()
        Dim ctl As XPander

        Dim enumerator As IDictionaryEnumerator = m_ControlList.GetEnumerator()
        While enumerator.MoveNext
            ctl = CType(enumerator.Value, XPander)
            ctl.Collapse()
        End While
    End Sub
#End Region

End Class

Public Class XPanderComparer
    Implements IComparer

#Region " Public Methods"
    ' If x < y, a -1 is returned
    ' If x = y, 0 is returned
    ' If x > y, 1 is returned
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim xp1 As XPander = CType(x, XPander)
        Dim xp2 As XPander = CType(y, XPander)
        Dim result As Integer = 0

        If xp1.Top < xp2.Top Then result = -1
        If xp1.Top > xp2.Top Then result = 1

        Return result
    End Function
#End Region

End Class

