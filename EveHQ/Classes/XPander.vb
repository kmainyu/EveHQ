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
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Drawing.Imaging
<Designer(GetType(XPanderDesigner)), DesignTimeVisibleAttribute(True)> _
Public Class XPander
    Inherits System.Windows.Forms.UserControl

#Region " Constants "

#End Region

#Region " Members "
    ' Size Related
    Private m_CaptionHeight As Integer = 25
    Private m_ControlHeight As Integer = 10
    Private m_TransitionSizeDelta As Integer = 0
    Private m_TransitionAlphaChannel As Integer = 0
    Private m_CaptionCurveRadius As Integer = 7
    Private m_MinCaptionHeight As Integer = 25

    'State related
    Private m_Expanded As Boolean = True
    Private m_IsCaptionHighlighted As Boolean = False

    ' Color Related
    Private m_PaneTopLeftColor As Color = Color.White
    Private m_PaneBottomRightColor As Color = Color.FromArgb(214, 223, 247)
    Private m_PaneOutlineColor As Color = Color.White
    Private m_GrayAttributes As ImageAttributes
    Private m_GrayMatrix As ColorMatrix

    ' Behavior
    Private m_AnimationTime As Integer = 100
    Private m_Tooltip As New ToolTip
    Private m_bCanToggle As Boolean = True
    Private m_bShowTooltips As Boolean = True
    Private m_bAnimated As Boolean = False

    'Caption related Properties
    Private m_bDrawChevrons As Boolean = False
    Private m_CollapsedImage As Bitmap = My.Resources.Collapse
    Private m_CollapsedHImage As Bitmap = My.Resources.Collapse_h
    Private m_ExpandedImage As Bitmap = My.Resources.Expand
    Private m_ExpandedHImage As Bitmap = My.Resources.Expand_h
    Private m_CaptionLeftColor As Color = Color.White
    Private m_CaptionRightColor As Color = Color.FromArgb(198, 210, 248)
    Private m_CaptionTextColor As Color = Color.FromArgb(33, 93, 198)
    Private m_CaptionTextHighlightColor As Color = Color.FromArgb(66, 142, 255)
    Private m_CaptionText As String = "XPander Group"
    Private m_CaptionFont As New Font("Microsoft Sans Serif", 8, FontStyle.Bold)
    Private m_CaptionColor As Color = Color.FromArgb(198, 210, 248)
    Private m_HeaderImage As Image ' Header Image

    Private m_CaptionStyle As CaptionStyleEnum = CaptionStyleEnum.Normal
    Private m_TooltipText As String
    Private m_BorderStyle As Border3DStyle = Border3DStyle.Flat
    Private m_bDrawBorder As Boolean = False
    Private m_ChevronStyle As ChevronStyleEnum = ChevronStyleEnum.Image
    Private m_CaptionTextAlign As CaptionTextAlignment = CaptionTextAlignment.Left
    Private m_ChevronAlign As ChevronAlignment = ChevronAlignment.Right
    Private m_CaptionFormatFlag As FormatFlag = FormatFlag.NoWrap
    Private m_ImageOffsetY As Integer = 10
    Enum GroupState
        Expanding
        Collapsing
        Standby
    End Enum
    Enum CaptionStyleEnum
        Normal
        FlatLine
        WrapAroundLine
        None
    End Enum
    Enum ChevronStyleEnum
        Image
        ArrowsInCircle
        Triangle
        PlusMinus
        None
    End Enum
    Enum CaptionTextAlignment
        Left
        Middle
        Right
    End Enum
    Enum ChevronAlignment
        Left
        Right
    End Enum
    Enum FormatFlag
        NoWrap
        Wrap
    End Enum

    Private m_GroupState As GroupState = GroupState.Standby
#End Region

#Region " Custom Events "
    ' Repeatedly fired as the XPander is being collapsed
    Delegate Sub XPanderCollapsingHandler(ByVal x As XPander, ByVal delta As Integer)
    Public Event XPanderCollapsing As XPanderCollapsingHandler

    ' Fired when the XPander is completely collapsed
    Delegate Sub XPanderCollapsedHandler(ByVal x As XPander)
    Public Event XPanderCollapsed As XPanderCollapsedHandler

    Delegate Sub XPanderExpandingHandler(ByVal x As XPander, ByVal delta As Integer)
    Public Event XPanderExpanding As XPanderCollapsingHandler

    Delegate Sub XPanderExpandedHandler(ByVal x As XPander)
    Public Event XPanderExpanded As XPanderExpandedHandler
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        ' We draw everything, and repaint when resized.
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.ContainerControl, True)

        Me.BackColor = Color.FromArgb(214, 223, 247) ' Default
    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (m_Tooltip Is Nothing) Then
                m_Tooltip.Dispose()
            End If
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
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        '
        'Timer1
        '
        '
        'XPander
        '
        Me.Name = "XPander"
        Me.Size = New System.Drawing.Size(8, 136)

    End Sub

#End Region

#Region " Public Properties "
    ' properties
    <Description("Determines the ending (dark) color of the caption gradient fill."), _
    DefaultValue(GetType(Color), "198, 210, 248"), _
Category("Caption")> _
Public Property CaptionRightColor() As Color
        Get
            Return m_CaptionRightColor
        End Get

        Set(ByVal Value As Color)
            m_CaptionRightColor = Value
            Invalidate()
        End Set
    End Property
    <Description("Offset for image above the caption."), _
DefaultValue(10), _
Category("Caption")> _
Public Property ImageOffset() As Integer
        Get
            Return m_ImageOffsetY
        End Get

        Set(ByVal Value As Integer)
            m_ImageOffsetY = Value
            Invalidate()
        End Set
    End Property
    <Description("Determines the border style."), _
    DefaultValue(GetType(Border3DStyle), "Border3DStyle.Flat"), _
Category("Appearance")> _
Public Overloads Property BorderStyle() As Border3DStyle
        Get
            Return m_BorderStyle
        End Get

        Set(ByVal Value As Border3DStyle)
            m_BorderStyle = Value
            Invalidate()
        End Set
    End Property
    <Description("Determines the Caption Text Alignment."), _
    DefaultValue(GetType(CaptionTextAlignment), "CaptionTextAlignment.Left"), _
Category("Caption")> _
Public Property CaptionTextAlign() As CaptionTextAlignment
        Get
            Return m_CaptionTextAlign
        End Get

        Set(ByVal Value As CaptionTextAlignment)
            m_CaptionTextAlign = Value
            Invalidate()
        End Set
    End Property
    <Description("Format flags for Caption Text."), _
DefaultValue(GetType(FormatFlag), "FormatFlag.NoWrap"), _
Category("Caption")> _
Public Property CaptionFormatFlag() As FormatFlag
        Get
            Return m_CaptionFormatFlag
        End Get

        Set(ByVal Value As FormatFlag)
            m_CaptionFormatFlag = Value
            Invalidate()
        End Set
    End Property
    <Description("Specify whether to draw a border or not."), _
        DefaultValue(False), _
Category("Appearance")> _
Public Property DrawBorder() As Boolean
        Get
            Return m_bDrawBorder
        End Get

        Set(ByVal Value As Boolean)
            m_bDrawBorder = Value
            Invalidate()
        End Set
    End Property
    <Description("Determines the starting (light) color of the caption gradient fill."), _
        DefaultValue(GetType(Color), "255,255,255"), _
Category("Caption")> _
Public Property CaptionLeftColor() As Color
        Get
            Return m_CaptionLeftColor
        End Get

        Set(ByVal Value As Color)
            m_CaptionLeftColor = Value
            Invalidate()
        End Set
    End Property
    <Description("Height of caption."), _
    DefaultValue(25), _
    Category("Caption")> _
    Public Property CaptionHeight() As Integer
        Get
            Return m_CaptionHeight
        End Get

        Set(ByVal Value As Integer)
            RepositionChildControls(Value - m_CaptionHeight)
            m_CaptionHeight = Value
            Invalidate()
        End Set
    End Property
    <Description("Caption curve radius."), _
DefaultValue(7), _
Category("Caption")> _
Public Property CaptionCurveRadius() As Integer
        Get
            Return m_CaptionCurveRadius
        End Get

        Set(ByVal Value As Integer)
            If Value < 0 Then
                m_CaptionCurveRadius = 0
            Else
                m_CaptionCurveRadius = Value
            End If
            Invalidate()
        End Set
    End Property
    <Description("Caption text."), _
    DefaultValue(""), _
    Category("Caption"), _
    Localizable(True)> _
    Public Property CaptionText() As String
        Get
            Return m_CaptionText
        End Get

        Set(ByVal Value As String)
            m_CaptionText = Value
            Invalidate()
        End Set
    End Property
    <Description("Pane Outline color."), _
DefaultValue(GetType(Color), "255, 255, 255"), _
Category("Appearance")> _
Public Property PaneOutlineColor() As Color
        Get
            Return m_PaneOutlineColor
        End Get

        Set(ByVal Value As Color)
            m_PaneOutlineColor = Value
            Invalidate()
        End Set
    End Property
    <Description("Pane Top Left color."), _
    DefaultValue(GetType(Color), "255, 255, 255"), _
    Category("Appearance")> _
    Public Property PaneTopLeftColor() As Color
        Get
            Return m_PaneTopLeftColor
        End Get

        Set(ByVal Value As Color)
            m_PaneTopLeftColor = Value
            Invalidate()
        End Set
    End Property
    <Description("Pane Bottom Right color."), _
DefaultValue(GetType(Color), "214, 223, 247"), Category("Appearance")> _
Public Property PaneBottomRightColor() As Color
        Get
            Return m_PaneBottomRightColor
        End Get

        Set(ByVal Value As Color)
            m_PaneBottomRightColor = Value
            Invalidate()
        End Set
    End Property
    <Description("Caption text color."), _
    DefaultValue(GetType(Color), "33,93,198"), _
    Category("Caption")> _
    Public Property CaptionTextColor() As Color
        Get
            Return m_CaptionTextColor
        End Get

        Set(ByVal Value As Color)
            m_CaptionTextColor = Value
            Invalidate()
        End Set
    End Property
    <Description("Caption text color when the mouse is hovering over it."), _
    DefaultValue(GetType(Color), "66, 142, 255"), _
    Category("Caption")> _
    Public Property CaptionTextHighlightColor() As Color
        Get
            Return m_CaptionTextHighlightColor
        End Get

        Set(ByVal Value As Color)
            m_CaptionTextHighlightColor = Value
            Invalidate()
        End Set
    End Property
    <Description("Image to be shown in Header"), _
    DefaultValue(GetType(Image), "Nothing"), _
    Category("Caption")> _
    Public Property Image() As Image
        Get
            Return m_HeaderImage
        End Get

        Set(ByVal Value As Image)
            ' If we changed nothing to nothing, then do nothing !
            If m_HeaderImage Is Nothing And Value Is Nothing Then
                Return
            End If
            m_HeaderImage = Value
            Invalidate()
        End Set
    End Property
    <Description("Image of Collapsed Chevron"), _
Category("Chevron")> _
Public Property CollapsedImage() As Bitmap
        Get
            Return m_CollapsedImage
        End Get

        Set(ByVal Value As Bitmap)
            If Value Is Nothing Then
                m_CollapsedImage = My.Resources.Collapse
            Else
                m_CollapsedImage = Value
            End If
            Invalidate()
        End Set
    End Property
    <Description("Image of Expanded Chevron"), _
Category("Chevron")> _
Public Property ExpandedImage() As Bitmap
        Get
            Return m_ExpandedImage
        End Get

        Set(ByVal Value As Bitmap)
            If Value Is Nothing Then
                m_ExpandedImage = My.Resources.Expand
            Else
                m_ExpandedImage = Value
            End If
            Invalidate()
        End Set
    End Property
    <Description("Image of Collapsed Chevron when highlighted"), _
Category("Chevron")> _
Public Property CollapsedHighlightImage() As Bitmap
        Get
            Return m_CollapsedHImage
        End Get

        Set(ByVal Value As Bitmap)
            If Value Is Nothing Then
                m_CollapsedHImage = New Bitmap(Me.GetType(), "Collapse_h.jpg")
            Else
                m_CollapsedHImage = Value
            End If
            Invalidate()
        End Set
    End Property
    <Description("Image of Expanded Chevron when highlighted"), _
Category("Chevron")> _
Public Property ExpandedHighlightImage() As Bitmap
        Get
            Return m_ExpandedHImage
        End Get

        Set(ByVal Value As Bitmap)
            If Value Is Nothing Then
                m_ExpandedHImage = New Bitmap(Me.GetType(), "Expand_h.jpg")
            Else
                m_ExpandedHImage = Value
            End If
            Invalidate()
        End Set
    End Property
    <Description("Caption Font."), Category("Caption")> _
    Public Property CaptionFont() As Font
        Get
            Return m_CaptionFont
        End Get

        Set(ByVal Value As Font)
            m_CaptionFont = Value
            Invalidate()
        End Set
    End Property
    <Description("Time taken to Collapse or Expand"), _
    DefaultValue(100), _
Category("Behavior")> _
Public Property AnimationTime() As Integer
        Get
            Return m_AnimationTime
        End Get

        Set(ByVal Value As Integer)
            m_AnimationTime = Value
        End Set
    End Property
    <Description("Can the user toggle between collapse/expand states."), _
    DefaultValue(True), _
Category("Behavior")> _
Public Property CanToggle() As Boolean
        Get
            Return m_bCanToggle
        End Get

        Set(ByVal Value As Boolean)
            m_bCanToggle = Value
            Invalidate()
        End Set
    End Property
    <Description("Chevron Style."), _
    DefaultValue(GetType(ChevronStyleEnum), "ChevronStyleEnum.Image"), _
Category("Chevron")> _
Public Property ChevronStyle() As ChevronStyleEnum
        Get
            Return m_ChevronStyle
        End Get

        Set(ByVal Value As ChevronStyleEnum)
            m_ChevronStyle = Value
            Invalidate()
        End Set
    End Property
    <Description("Caption Style."), _
    DefaultValue(GetType(CaptionStyleEnum), "CaptionStyleEnum.Normal"), _
Category("Caption")> _
Public Property CaptionStyle() As CaptionStyleEnum
        Get
            Return m_CaptionStyle
        End Get

        Set(ByVal Value As CaptionStyleEnum)
            m_CaptionStyle = Value
            If m_CaptionStyle = CaptionStyleEnum.WrapAroundLine Then
                'm_bCanToggle = False
            End If
            Invalidate()
        End Set
    End Property
    <Description("Animation during collapse/expand."), _
    DefaultValue(False), _
Category("Behavior")> _
Public Property Animated() As Boolean
        Get
            Return m_bAnimated
        End Get

        Set(ByVal Value As Boolean)
            m_bAnimated = Value
        End Set
    End Property
    <Description("Show tooltips."), _
    DefaultValue(True), _
Category("Behavior")> _
Public Property ShowTooltips() As Boolean
        Get
            Return m_bShowTooltips
        End Get

        Set(ByVal Value As Boolean)
            m_bShowTooltips = Value
            If m_bShowTooltips = False Then
                m_Tooltip.Active = False
            End If
        End Set
    End Property
    <Description("Tooltip Text"), _
Category("Behavior")> _
Public Property TooltipText() As String
        Get
            Return m_TooltipText
        End Get

        Set(ByVal Value As String)
            m_TooltipText = Value
        End Set
    End Property
    <Description("Height of the control when expanded"), _
        Browsable(False), _
         DesignOnly(True)> _
        Public Property ExpandedHeight() As Integer
        Get
            Return m_ControlHeight
        End Get
        Set(ByVal Value As Integer)
            m_ControlHeight = Value
        End Set
    End Property
    Public ReadOnly Property IsExpanded() As Boolean
        Get
            Return m_Expanded
        End Get

    End Property
#End Region

#Region " Overrides"
    Private Sub XPander_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.ContainerControl, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        Me.DockPadding.Top = m_CaptionHeight
        Me.BackColor = Color.Transparent

        ' Setup the ColorMatrix and ImageAttributes for grayscale images.
        m_GrayMatrix = New ColorMatrix
        m_GrayMatrix.Matrix00 = 1 / 3.0F
        m_GrayMatrix.Matrix01 = 1 / 3.0F
        m_GrayMatrix.Matrix02 = 1 / 3.0F
        m_GrayMatrix.Matrix10 = 1 / 3.0F
        m_GrayMatrix.Matrix11 = 1 / 3.0F
        m_GrayMatrix.Matrix12 = 1 / 3.0F
        m_GrayMatrix.Matrix20 = 1 / 3.0F
        m_GrayMatrix.Matrix21 = 1 / 3.0F
        m_GrayMatrix.Matrix22 = 1 / 3.0F
        m_GrayAttributes = New ImageAttributes
        m_GrayAttributes.SetColorMatrix(m_GrayMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)

        ' Set up the delays for the ToolTip.
        m_Tooltip.AutoPopDelay = 2000
        m_Tooltip.InitialDelay = 500
        m_Tooltip.ReshowDelay = 300

        If (m_bShowTooltips) Then
            ' Set up the ToolTip text for the Button and Checkbox.
            m_Tooltip.SetToolTip(Me, m_TooltipText)
            m_Tooltip.Active = False
        End If

    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        ' Do AntiAlias smoothing
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

        ' Caption Color
        Dim capcolor As New Color
        If Me.Enabled Then
            If m_IsCaptionHighlighted Then
                capcolor = m_CaptionTextHighlightColor
            Else
                capcolor = m_CaptionTextColor
            End If
        Else
            capcolor = SystemColors.GrayText
        End If
        ' Draw based upon Caption Style
        Select Case m_CaptionStyle
            Case CaptionStyleEnum.Normal
                DrawNormalStyleCaption(e.Graphics, capcolor)
                ' Draw the outline around the work area
                If Me.Height > m_CaptionHeight Then
                    e.Graphics.DrawLine(New Pen(m_PaneOutlineColor), _
                        1, Me.CaptionHeight, 1, Me.Height) 'Left line from top to bottom
                    e.Graphics.DrawLine(New Pen(m_PaneOutlineColor), _
                        Me.Width - 1, Me.CaptionHeight, Me.Width - 1, Me.Height)
                    'Right side from top to bottom
                    e.Graphics.DrawLine(New Pen(m_PaneOutlineColor), _
                        0, Me.Height - 1, Me.Width - 1, Me.Height - 1) ' Bottom line from left to right
                End If
            Case CaptionStyleEnum.FlatLine
                DrawFlatLineStyleCaption(e.Graphics, capcolor)
            Case CaptionStyleEnum.WrapAroundLine
                DrawWrapAroundLineStyleCaption(e.Graphics, capcolor)
            Case CaptionStyleEnum.None
                DrawNoneStyleCaption(e.Graphics, capcolor)
            Case Else
                DrawNoneStyleCaption(e.Graphics, capcolor)
        End Select

    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
        MyBase.OnPaintBackground(pevent)
        If Me.Height > CaptionHeight Then
            Dim rect As New Rectangle(0, CaptionHeight, Me.Width, Me.Height - CaptionHeight)
            Dim b As New LinearGradientBrush(rect, m_PaneTopLeftColor, m_PaneBottomRightColor, LinearGradientMode.ForwardDiagonal)
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            pevent.Graphics.FillRectangle(b, rect)
        End If
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        ' Change cursor to hand when over caption area.
        If e.Y <= Me.CaptionHeight Then
            If m_bCanToggle Then
                Windows.Forms.Cursor.Current = Cursors.Hand
            End If
            m_IsCaptionHighlighted = True
            ' Activate Tooltips only when mouse is over the caption 
            If m_bShowTooltips Then
                m_Tooltip.Active = True
            End If
        Else
            Windows.Forms.Cursor.Current = Cursors.Default
            m_IsCaptionHighlighted = False
            m_Tooltip.Active = False
        End If
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        ' Don't do anything if did not click on caption.
        ' Also ignore the right mouse clicks
        If e.Button <> Windows.Forms.MouseButtons.Left Or e.Y > Me.CaptionHeight Or m_GroupState <> GroupState.Standby Or m_bCanToggle = False Then
            Return
        End If
        ChangeHeight()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        If m_IsCaptionHighlighted Then
            m_IsCaptionHighlighted = False
            Windows.Forms.Cursor.Current = Cursors.Default
            Invalidate()
        End If
    End Sub
#End Region

#Region " Public Methods"
    Public Sub Expand()
        If m_Expanded = False And m_GroupState = GroupState.Standby Then
            ChangeHeight()
        End If
    End Sub
    Public Sub Collapse()
        If m_Expanded And m_GroupState = GroupState.Standby Then
            ChangeHeight()
        End If
    End Sub
#End Region

#Region " Private Helpers"
    Private Sub ChangeHeight()
        If (m_Expanded) Then
            m_ControlHeight = Me.Height
            m_GroupState = GroupState.Collapsing
        Else
            m_GroupState = GroupState.Expanding
        End If

        ' If animation enabled then use the timer
        If m_bAnimated = True And m_AnimationTime >= 5 Then
            Timer1.Interval = m_AnimationTime
            Timer1.Enabled = True
        Else
            QuickToggle()
        End If

    End Sub

    ' Timer method to handle animation
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        Timer1.Enabled = False
        'Initializes the transition delta
        If m_TransitionSizeDelta = 0 Then
            m_TransitionSizeDelta = 1
        End If
        ' Reduce the interval between timer events - this gives the visual effect of the
        ' control slowly starting to collapse/expand then accelerating
        If (Timer1.Interval > (m_AnimationTime / 5)) Then
            If (Timer1.Interval - (m_AnimationTime / 5)) <= 0 Then
                Timer1.Interval = 1
            Else
                Timer1.Interval = CInt(Timer1.Interval - m_AnimationTime / 5)
            End If
            'if timer1.Interval =
        Else
            m_TransitionSizeDelta += 2
        End If


        'Initialises the control transparency
        If (m_TransitionAlphaChannel = 0) Then
            m_TransitionAlphaChannel = 10
        Else
            If (m_TransitionAlphaChannel + 10 < 255) Then
                'Increase control transparency as it collapses
                m_TransitionAlphaChannel += 10
            End If
        End If

        If m_GroupState = GroupState.Expanding Then  ' Expanding

            If ((Me.Height + m_TransitionSizeDelta) < m_ControlHeight) Then
                SetControlsOpacity(m_TransitionAlphaChannel)
                m_PaneBottomRightColor = Color.FromArgb(m_TransitionAlphaChannel, m_PaneBottomRightColor)
                m_PaneTopLeftColor = Color.FromArgb(m_TransitionAlphaChannel, m_PaneTopLeftColor)
                m_PaneOutlineColor = Color.FromArgb(m_TransitionAlphaChannel, m_PaneOutlineColor)
                Me.Height += m_TransitionSizeDelta
                SetControlsVisible()
            Else
                SetControlsOpacity(255)
                m_PaneBottomRightColor = Color.FromArgb(255, m_PaneBottomRightColor)
                m_PaneTopLeftColor = Color.FromArgb(255, m_PaneTopLeftColor)
                m_PaneOutlineColor = Color.FromArgb(255, m_PaneOutlineColor)
                m_TransitionAlphaChannel = 0
                m_TransitionSizeDelta = (m_ControlHeight - Me.Height)
                Me.Height = m_ControlHeight
                m_Expanded = True
                m_GroupState = GroupState.Standby
                SetControlsVisible()

            End If
            RaiseEvent XPanderExpanding(Me, m_TransitionSizeDelta)
            Invalidate()
            Timer1.Enabled = True
        ElseIf m_GroupState = GroupState.Collapsing Then  ' Collapsing

            If ((Me.Height - m_TransitionSizeDelta) > CaptionHeight) Then
                SetControlsOpacity(m_TransitionAlphaChannel)
                Me.Height -= m_TransitionSizeDelta
                m_PaneBottomRightColor = Color.FromArgb(255 - m_TransitionAlphaChannel, m_PaneBottomRightColor)
                m_PaneTopLeftColor = Color.FromArgb(255 - m_TransitionAlphaChannel, m_PaneTopLeftColor)
                m_PaneOutlineColor = Color.FromArgb(255 - m_TransitionAlphaChannel, m_PaneOutlineColor)
                SetControlsVisible()
            Else
                m_TransitionAlphaChannel = 0
                SetControlsOpacity(0)
                m_PaneBottomRightColor = Color.FromArgb(0, m_PaneBottomRightColor)
                m_PaneTopLeftColor = Color.FromArgb(0, m_PaneTopLeftColor)
                m_PaneOutlineColor = Color.FromArgb(0, m_PaneOutlineColor)
                m_TransitionSizeDelta = (CaptionHeight - Me.Height) * -1
                Me.Height = CaptionHeight
                m_Expanded = False
                m_GroupState = GroupState.Standby
                SetControlsVisible()
            End If
            RaiseEvent XPanderCollapsing(Me, m_TransitionSizeDelta)
            Invalidate()
            Timer1.Enabled = True
        ElseIf m_GroupState = GroupState.Standby Then
            Timer1.Enabled = False
            m_TransitionSizeDelta = 0
            Invalidate()
        End If

    End Sub
    Private Sub SetControlsOpacity(ByVal opacity As Integer)
        Dim c As Control
        'Dim clr As Color
        'Dim tp As Type
        'Dim tp2 As Type
        Dim args() As [Object] = {ControlStyles.SupportsTransparentBackColor}
        'Dim sbc As Object
        Try
            For Each c In Me.Controls
                ' Check specifically for text and combo-boxes as they don't support transparent backgrounds
                If Not c.GetType() Is GetType(TextBox) And Not c.GetType() Is GetType(ComboBox) Then
                    If m_GroupState = GroupState.Collapsing Then
                        'Collapsing
                        If (c.BackColor.ToArgb() <> Color.Transparent.ToArgb()) Then
                            c.BackColor = Color.FromArgb(255 - opacity, c.BackColor)
                        End If
                        c.ForeColor = Color.FromArgb(255 - opacity, c.ForeColor)
                    ElseIf m_GroupState = GroupState.Expanding Then
                        ' Expanding
                        If (c.BackColor.ToArgb() <> Color.Transparent.ToArgb()) Then
                            c.BackColor = Color.FromArgb(opacity, c.BackColor)
                        End If
                        c.ForeColor = Color.FromArgb(opacity, c.ForeColor)
                    End If
                End If
            Next c
        Catch ex As System.ArgumentException
            ' If we reach here its most probably because we have a control that doesn't support
            ' transparent backgrounds. So for now just ignore it!
        End Try
    End Sub
    Private Sub SetControlsVisible()
        Dim c As Control
        For Each c In Me.Controls
            If (c.Top < CaptionHeight) Then
                c.Visible = False
            Else
                c.Visible = True
            End If
        Next c
    End Sub

    ' Toggle between Expand/Collapse states WITHOUT any animation    
    Private Sub QuickToggle()
        If m_GroupState = GroupState.Expanding Then  ' Expanding
            SetControlsOpacity(255)
            m_PaneBottomRightColor = Color.FromArgb(255, m_PaneBottomRightColor)
            m_PaneTopLeftColor = Color.FromArgb(255, m_PaneTopLeftColor)
            m_PaneOutlineColor = Color.FromArgb(255, m_PaneOutlineColor)
            m_TransitionAlphaChannel = 0
            Me.Height = m_ControlHeight
            m_Expanded = True
            m_GroupState = GroupState.Standby
            SetControlsVisible()
            RaiseEvent XPanderExpanded(Me)
        ElseIf m_GroupState = GroupState.Collapsing Then  ' Collapsing
            m_TransitionAlphaChannel = 0
            SetControlsOpacity(0)
            m_PaneBottomRightColor = Color.FromArgb(0, m_PaneBottomRightColor)
            m_PaneTopLeftColor = Color.FromArgb(0, m_PaneTopLeftColor)
            m_PaneOutlineColor = Color.FromArgb(0, m_PaneOutlineColor)
            Me.Height = CaptionHeight
            m_Expanded = False
            m_GroupState = GroupState.Standby
            SetControlsVisible()
            RaiseEvent XPanderCollapsed(Me)
        End If
        Invalidate()
    End Sub

    ' Reposition child controls if the caption height changes
    Private Sub RepositionChildControls(ByVal Offset As Integer)
        Dim ctl As Control
        If Offset = 0 Then
            Return
        End If
        For Each ctl In Me.Controls
            ctl.Top += Offset
        Next ctl

    End Sub

    ' Draw Caption Text using specified rectangle and text alignment
    Private Function DrawCaptionText(ByVal g As Graphics, ByVal rect As RectangleF, ByVal capcolor As Color) As RectangleF
        ' Format with ellipses at the end, in case the text is too large to display
        Dim format As StringFormat = New StringFormat
        format.Trimming = StringTrimming.EllipsisCharacter
        If m_CaptionFormatFlag = FormatFlag.NoWrap Then
            format.FormatFlags = StringFormatFlags.NoWrap
        End If

        Dim invcolor As Color = Color.FromArgb(70, capcolor.R, capcolor.G, capcolor.B)

        ' Find out the size required to display the caption
        Dim size As SizeF = g.MeasureString(CaptionText, m_CaptionFont, New SizeF(rect.Width, rect.Height), format)
        Dim lft As Single = rect.Left
        ' Reposition left co-ordinate according to specified caption text alignment
        If m_CaptionTextAlign = CaptionTextAlignment.Right Then
            lft += rect.Width - size.Width
        ElseIf m_CaptionTextAlign = CaptionTextAlignment.Middle Then
            lft += (rect.Width / 2) - (size.Width / 2)
        End If

        Dim top As Single = 0
        Dim CaptionRectF As RectangleF = New RectangleF(lft, rect.Top, size.Width, rect.Height)
        ' Draw in Gray Text, if disabled
        If (Me.Enabled) Then
            g.DrawString(CaptionText, m_CaptionFont, New SolidBrush(capcolor), CaptionRectF, format)
            ' Draw in inverse color for a little shadow effect
            'Dim invrect As RectangleF = CaptionRectF
            'invrect.X += 1
            'invrect.Y += 1
            'g.DrawString(CaptionText, m_CaptionFont, New SolidBrush(invcolor), invrect, format)
        Else
            ControlPaint.DrawStringDisabled(g, CaptionText, m_CaptionFont, SystemColors.GrayText, CaptionRectF, format)
        End If
        Return CaptionRectF
    End Function

#Region " Captions"
    Private Sub DrawWrapAroundLineStyleCaption(ByVal g As Graphics, ByVal capcolor As Color)

        Dim path As New GraphicsPath
        Dim path2 As New GraphicsPath
        ' Caption text.
        Dim LineCaptionGap As Integer = 1
        Dim x As Single = m_CaptionCurveRadius + LineCaptionGap + 5
        Dim hgt As Single = CSng(m_CaptionHeight)
        Dim y As Single = 5
        Dim wdth As Single = Me.Width - (x * 2)
        Dim CaptionRectF As RectangleF = DrawCaptionText(g, New RectangleF(x, y, wdth, hgt), capcolor)
        Dim toprow As Single = CaptionRectF.Top + 5

        ' Now draw the caption areas with the rounded corners at the top
        ' Top line from left to right(After Caption)
        If (CaptionRectF.Right + LineCaptionGap) < (Me.Width - (m_CaptionCurveRadius * 2)) Then
            path.AddLine(CaptionRectF.Right + LineCaptionGap, toprow, Me.Width - (m_CaptionCurveRadius * 2), toprow)
        End If
        ' Curved Edge on Top right
        If (m_CaptionCurveRadius > 0) Then
            path.AddArc(Me.Width - (m_CaptionCurveRadius * 2), toprow, (m_CaptionCurveRadius * 2), toprow + (m_CaptionCurveRadius * 2), 270, 90)
        End If

        If m_Expanded = True Then
            ' Right side line from top to bottom
            path.AddLine(Me.Width - 1, m_CaptionCurveRadius + toprow, Me.Width - 1, Me.Height - 1)
            ' Bottom line from right to left
            path.AddLine(Me.Width - 1, Me.Height - 1, 1, Me.Height - 1)
            ' Left side line from bottom to top
            path2.AddLine(1, Me.Height - 1, 1, m_CaptionCurveRadius + toprow + 2)
        Else
            ' Right side line from top to bottom
            path.AddLine(Me.Width - 1, m_CaptionCurveRadius + toprow, Me.Width - 1, Me.CaptionHeight - 1)
            ' Bottom line from right to left
            path.AddLine(Me.Width - 1, Me.CaptionHeight - 1, 1, Me.CaptionHeight - 1)
            ' Left side line from bottom to top
            path2.AddLine(1, Me.CaptionHeight - 1, 1, m_CaptionCurveRadius + toprow + 2)
        End If

        'Curved edge on left top
        If (m_CaptionCurveRadius > 0) Then
            path2.AddArc(1, toprow, (m_CaptionCurveRadius * 2) + 1, toprow + (m_CaptionCurveRadius * 2), 180, 90)
        End If
        ' Top line from left to right(Before Caption)
        path2.AddLine(m_CaptionCurveRadius, toprow, CaptionRectF.Left - LineCaptionGap, toprow)

        ' Now draw the outline
        g.DrawPath(New Pen(m_PaneOutlineColor), path)
        g.DrawPath(New Pen(m_PaneOutlineColor), path2)
    End Sub
    ''******************* Drawing in the Flat Line Caption Style **************************
    Private Sub DrawFlatLineStyleCaption(ByVal g As Graphics, ByVal capcolor As Color)

        ' Find out the valid rectangle to display the caption
        Dim left As Single = 0
        Dim top As Single = 0
        Dim wdth As Single
        Dim chevwidth As Integer = 0
        Dim LineCaptionGap As Integer = 10
        Dim MinLineWidth As Integer = 8
        Dim margin As Integer = 4

        If m_bCanToggle Then
            Dim ptX As Integer = 0
            If m_CaptionTextAlign = CaptionTextAlignment.Left Or m_CaptionTextAlign = CaptionTextAlignment.Middle Then
                ptX = Me.Width - MinLineWidth
            End If
            ' Draw little triangular chevrons
            If m_ChevronStyle = ChevronStyleEnum.Triangle Then
                chevwidth = DrawChevronTriangle(g, ptX, CInt(m_CaptionHeight / 2), MinLineWidth, 2, capcolor)
            ElseIf m_ChevronStyle = ChevronStyleEnum.PlusMinus Then
                chevwidth = DrawChevronPlusMinus(g, ptX, CInt(m_CaptionHeight / 2 - (MinLineWidth / 2)), MinLineWidth, capcolor)
            End If
        End If
        wdth = CSng(Me.Width - chevwidth - margin)
        ' Reposition left co-ordinate according to specified caption text alignment
        If m_CaptionTextAlign = CaptionTextAlignment.Middle Then
            left = MinLineWidth + LineCaptionGap
            wdth -= CSng(MinLineWidth + (2 * LineCaptionGap))
        ElseIf m_CaptionTextAlign = CaptionTextAlignment.Right Then
            left = chevwidth + margin ' Chevron Width
        End If

        Dim CaptionRectF As RectangleF
        CaptionRectF = DrawCaptionText(g, New RectangleF(left, top, wdth, m_CaptionHeight), capcolor)

        ' Draw the line 
        Dim x1, y1, x2, y2 As Integer

        y1 = CInt(m_CaptionHeight / 2)
        y2 = CInt(m_CaptionHeight / 2)
        ' Draw the line according to text alignment
        If m_CaptionTextAlign = CaptionTextAlignment.Right Then
            x1 = 0
            x2 = CInt(CaptionRectF.Left - LineCaptionGap)
            If x2 < MinLineWidth Then
                x2 = MinLineWidth
            End If
        ElseIf m_CaptionTextAlign = CaptionTextAlignment.Left Then
            x1 = CInt(CaptionRectF.Width + LineCaptionGap)
            x2 = Me.Width
            If (x2 - x1) < MinLineWidth Then
                x1 = x2 - MinLineWidth
            End If
        ElseIf m_CaptionTextAlign = CaptionTextAlignment.Middle Then ' Draw both left and right
            x1 = 0
            x2 = CInt(CaptionRectF.Left - LineCaptionGap)
            If x2 < MinLineWidth Then
                x2 = MinLineWidth
            End If
            g.DrawLine(New Pen(capcolor), x1, y1, x2, y2)
            x1 = CInt(CaptionRectF.Left + CaptionRectF.Width + LineCaptionGap)
            x2 = Me.Width
            If (x2 - x1) < MinLineWidth Then
                x1 = x2 - MinLineWidth
            End If
        End If
        g.DrawLine(New Pen(capcolor), x1, y1, x2, y2)
    End Sub

    ''******************* Drawing in the Normal Caption Style **************************
    Private Sub DrawNormalStyleCaption(ByVal g As Graphics, ByVal capcolor As Color)
        Dim rc As New Rectangle(0, 0, Me.Width, CaptionHeight)
        ' Calculate the offset for image (if any)
        Dim path As New GraphicsPath
        Dim imageOffsetY As Integer = 0
        If Not Me.Image Is Nothing Then
            imageOffsetY = m_ImageOffsetY
        End If

        ' Now draw the caption areas with the rounded corners at the top
        path.AddLine(m_CaptionCurveRadius, imageOffsetY, Me.Width - (m_CaptionCurveRadius * 2), imageOffsetY)  ' Top line from left to right
        If (m_CaptionCurveRadius > 0) Then
            path.AddArc(Me.Width - (m_CaptionCurveRadius * 2) - 1, imageOffsetY, (m_CaptionCurveRadius * 2), (m_CaptionCurveRadius * 2), 270, 90) ' Curved Edge on Top right
        End If
        path.AddLine(Me.Width, m_CaptionCurveRadius - imageOffsetY, Me.Width, CaptionHeight) ' Right side line from top to bottom
        path.AddLine(Me.Width, CaptionHeight, 0, CaptionHeight) ' Bottom line from right to left
        path.AddLine(0, CaptionHeight, 0, m_CaptionCurveRadius - imageOffsetY) ' Left side line from bottom to top
        If (m_CaptionCurveRadius > 0) Then
            path.AddArc(0, imageOffsetY, (m_CaptionCurveRadius * 2), (m_CaptionCurveRadius * 2), 180, 90) 'Curved edge on left top
        End If

        ' Now fill the caption rectangle with the selected colors
        Dim CaptionBrush As New LinearGradientBrush(rc, m_CaptionLeftColor, m_CaptionRightColor, LinearGradientMode.Vertical)
        g.FillPath(CaptionBrush, path)

        ' Draw the border if any. This border is not drawn around the caption rectangle
        If m_bDrawBorder Then
            ControlPaint.DrawBorder3D(g, New Rectangle(0, Me.CaptionHeight, Me.Width, Me.Height), m_BorderStyle)
        End If

        ' Draw the header icon, if there is one
        Dim graphicsUnit As GraphicsUnit = System.Drawing.GraphicsUnit.Display
        Dim iconBorder As Integer = 2
        Dim imageOffsetX As Integer = 2 ' iconBorder

        If Not Me.Image Is Nothing Then
            imageOffsetX += Me.Image.Width + iconBorder
            Dim srcRectF As RectangleF = Me.Image.GetBounds(graphicsUnit)
            Dim destRect As Rectangle = New Rectangle(iconBorder, iconBorder, Me.Image.Width, Me.Image.Height)
            If Me.Enabled Then
                g.DrawImage(Me.Image, destRect, srcRectF.Left, srcRectF.Top, srcRectF.Width, srcRectF.Height, graphicsUnit)
            Else ' Draw Disabled
                g.DrawImage(Me.Image, destRect, srcRectF.Left, srcRectF.Top, srcRectF.Width, srcRectF.Height, graphicsUnit, m_GrayAttributes)
                'ControlPaint.DrawImageDisabled(e.Graphics, Me.Image, srcRectF.Left, srcRectF.Top, SystemColors.GrayText)
            End If
        End If

        Dim ChevronWidth As Integer = 8
        Dim ChevronMargin As Integer = 7
        ' Draw the Chevrons or the Toggle Buttons
        If m_bCanToggle Then
            If m_ChevronStyle = ChevronStyleEnum.Image Then
                ChevronWidth = DrawChevronImage(g, 2 + imageOffsetY)
            ElseIf m_ChevronStyle = ChevronStyleEnum.ArrowsInCircle Then
                ChevronWidth = DrawChevronArrowsInCircle(g, imageOffsetY)
            ElseIf m_ChevronStyle = ChevronStyleEnum.Triangle Then
                ChevronWidth = DrawChevronTriangle(g, Me.Width - ChevronWidth - ChevronMargin, imageOffsetY + 10, ChevronWidth, 0, capcolor)
            ElseIf m_ChevronStyle = ChevronStyleEnum.PlusMinus Then
                ChevronWidth = DrawChevronPlusMinus(g, Me.Width - ChevronWidth - ChevronMargin, imageOffsetY + 4, ChevronWidth, capcolor)
            End If
        End If

        ' Caption text.
        Dim x As Single = 10 + imageOffsetX
        Dim hgt As Single = CSng(m_CaptionHeight - imageOffsetY)
        Dim y As Single = CSng(imageOffsetY + 4)
        Dim wdth As Single
        If m_bCanToggle Then
            wdth = CSng(Me.Width - x - ChevronMargin - ChevronWidth)
        Else
            wdth = CSng(Me.Width - x - ChevronMargin)
        End If

        DrawCaptionText(g, New RectangleF(x, y, wdth, hgt), capcolor)
    End Sub

    ''******************* Drawing just the plain caption text **************************
    Private Sub DrawNoneStyleCaption(ByVal g As Graphics, ByVal capcolor As Color)

        ' Find out the valid rectangle to display the caption
        Dim left As Single = 0
        Dim top As Single = 0
        Dim wdth As Single
        Dim chevwidth As Integer = 0
        Dim margin As Integer = 0
        Dim DesiredChevWidth As Integer = 8

        If m_bCanToggle Then
            Dim ptX As Integer = 0
            If m_CaptionTextAlign = CaptionTextAlignment.Left Or m_CaptionTextAlign = CaptionTextAlignment.Middle Then
                ptX = Me.Width - DesiredChevWidth
            End If
            ' Draw little triangular chevrons
            If m_ChevronStyle = ChevronStyleEnum.Triangle Then
                chevwidth = DrawChevronTriangle(g, ptX, CInt(m_CaptionHeight / 2), DesiredChevWidth, 2, capcolor)
                margin = 4
            ElseIf m_ChevronStyle = ChevronStyleEnum.PlusMinus Then
                chevwidth = DrawChevronPlusMinus(g, ptX, CInt(m_CaptionHeight / 2 - (DesiredChevWidth / 2) - 1), DesiredChevWidth, capcolor)
                margin = 4
            End If
        End If
        wdth = CSng(Me.Width - chevwidth - margin)
        ' Reposition left co-ordinate according to specified caption text alignment
        If m_CaptionTextAlign = CaptionTextAlignment.Right Then
            left = chevwidth + margin ' Chevron Width
        End If

        DrawCaptionText(g, New RectangleF(left, top, wdth, m_CaptionHeight), capcolor)
    End Sub
#End Region

#Region " Chevrons"
    ''******************* Draw different type of Chevrons *********************************
    ' Triangular Chevron
    Private Function DrawChevronTriangle(ByVal g As Graphics, ByVal ptX As Integer, ByVal ptY As Integer, ByVal TBase As Integer, ByVal OffsetX As Integer, ByVal clr As Color) As Integer
        Dim p As New GraphicsPath
        Dim th As Integer = TBase   ' Triangle Base Size
        Dim ofst As Integer = OffsetX ' Offset

        If (m_Expanded) Then
            p.AddLine(ptX, ptY - ofst, ptX + th, ptY - ofst)
            p.AddLine(ptX + th, ptY - ofst, ptX + CInt(th / 2), ptY - th)
            p.AddLine(ptX + CInt(th / 2), ptY - th, ptX, ptY - ofst)
        Else
            p.AddLine(ptX, ptY + ofst, ptX + th, ptY + ofst)
            p.AddLine(ptX + th, ptY + ofst, ptX + CInt(th / 2), ptY + th)
            p.AddLine(ptX + CInt(th / 2), ptY + th, ptX, ptY + ofst)
        End If
        g.FillPath(New SolidBrush(clr), p)
        Return TBase
    End Function
    ' PlusMinus Chevron
    Private Function DrawChevronPlusMinus(ByVal g As Graphics, ByVal ptX As Integer, ByVal ptY As Integer, ByVal wdth As Integer, ByVal clr As Color) As Integer
        Dim p As New GraphicsPath
        Dim hgt As Integer = wdth
        Dim margin As Integer = 2
        g.DrawRectangle(New Pen(clr), New Rectangle(ptX, ptY, wdth, hgt))
        If m_Expanded = False Then
            g.DrawLine(New Pen(clr), CInt(ptX + (wdth / 2)), ptY + margin, CInt(ptX + (wdth / 2)), ptY + hgt - margin)
        End If
        g.DrawLine(New Pen(clr), ptX + margin, CInt(ptY + (hgt / 2)), ptX + wdth - margin, CInt(ptY + (hgt / 2)))
        Return wdth
    End Function
    ' Custom Image Chevron
    Private Function DrawChevronImage(ByVal g As Graphics, ByVal imageOffsetY As Single) As Integer
        Dim ChevronWidth As Integer
        Dim ChevronMargin As Integer = 7
        ' Expand / Collapse Icon
        If m_Expanded Then
            ' Gray Scale drawing for the images
            If Me.Enabled = False Then
                ControlPaint.DrawImageDisabled(g, m_ExpandedImage, Me.Width - m_ExpandedImage.Width - ChevronMargin, CInt(2 + imageOffsetY), Me.CaptionRightColor)
                ChevronWidth = m_ExpandedImage.Width
            ElseIf m_IsCaptionHighlighted Then
                g.DrawImage(m_ExpandedHImage, Me.Width - m_ExpandedHImage.Width - ChevronMargin, 2 + imageOffsetY)
                ChevronWidth = m_ExpandedHImage.Width
            Else
                g.DrawImage(m_ExpandedImage, Me.Width - m_ExpandedImage.Width - ChevronMargin, 2 + imageOffsetY)
                ChevronWidth = m_ExpandedImage.Width
            End If
        Else ' Collapsed
            If Me.Enabled = False Then
                ControlPaint.DrawImageDisabled(g, m_CollapsedImage, Me.Width - m_CollapsedImage.Width - ChevronMargin, CInt(2 + imageOffsetY), Me.CaptionRightColor)
                ChevronWidth = m_CollapsedImage.Width
            ElseIf m_IsCaptionHighlighted Then
                g.DrawImage(m_CollapsedHImage, Me.Width - m_CollapsedHImage.Width - ChevronMargin, 2 + imageOffsetY)
                ChevronWidth = m_CollapsedHImage.Width
            Else
                g.DrawImage(m_CollapsedImage, Me.Width - m_CollapsedImage.Width - ChevronMargin, 2 + imageOffsetY)
                ChevronWidth = m_CollapsedImage.Width
            End If
        End If
        Return ChevronWidth
    End Function
    ' Arrows in a circle Chevron
    Private Sub DrawChevronsAIC(ByVal g As Graphics, ByVal x As Integer, ByVal y As Integer, ByVal offset As Integer)
        ' Determine the orientation of the pseudo-button
        If (m_Expanded = False) Then
            DrawChevronAIC(g, x + offset, y + 1 * offset, -offset)
            DrawChevronAIC(g, x + offset, y + 2 * offset, -offset)
        Else
            DrawChevronAIC(g, x + offset, y + 2 * offset, offset)
            DrawChevronAIC(g, x + offset, y + 3 * offset, offset)
        End If
    End Sub
    Private Sub DrawChevronAIC(ByVal g As Graphics, ByVal x As Integer, ByVal y As Integer, ByVal offset As Integer)
        Dim p As Pen

        If (m_IsCaptionHighlighted) Then
            p = New Pen(m_CaptionTextHighlightColor)
        Else
            p = New Pen(m_CaptionTextColor)

        End If
        Dim points() As Point = {New Point(x, y), _
              New Point(x + Math.Abs(offset), y - offset), _
              New Point(x + 2 * Math.Abs(offset), y)}
        g.DrawLines(p, points)
    End Sub
    Private Function DrawChevronArrowsInCircle(ByVal g As Graphics, ByVal OffsetY As Integer) As Integer
        Dim dm As Integer = 15
        Dim btnOrigin As Point = New Point(Me.Width - dm - 5, 5 + OffsetY)
        Dim btnSize As Size = New Size(dm, dm)
        Dim btnRect As Rectangle = New Rectangle(btnOrigin, btnSize)
        g.DrawEllipse(New Pen(CaptionTextColor), btnRect)
        DrawChevronsAIC(g, btnRect.X, btnRect.Y, CInt(btnRect.Width / 4))
        Return dm
    End Function
#End Region
#End Region

End Class