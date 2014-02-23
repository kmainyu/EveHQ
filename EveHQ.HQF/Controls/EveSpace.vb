'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Namespace Controls

    Public Class EveSpace
        Dim _cl As Point
        Dim _di As DraggableImage
        ReadOnly _diList As New SortedList(Of String, DraggableImage)

#Region "Public Events"

        Public Event CalculationsChanged()
        Public Event GraphUpdateRequired()

#End Region

#Region "Calculation Variables"

        Dim _horizontalAngle As Double
        Dim _lAxisIntersect As Point
        Dim _rAxisIntersect As Point
        Dim _transversal As Double

        Public ReadOnly Property Transversal() As Double
            Get
                Return _transversal
            End Get
        End Property

        Dim _range As Double

        Public ReadOnly Property Range() As Double
            Get
                Return _range
            End Get
        End Property

        Dim _velocityScale As Double = 1

        Public Property VelocityScale() As Double
            Get
                Return _velocityScale
            End Get
            Set(ByVal value As Double)
                _velocityScale = value
                CalculateData()
                Invalidate(False)
            End Set
        End Property

        Dim _rangeScale As Double = 1

        Public Property RangeScale() As Double
            Get
                Return _rangeScale
            End Get
            Set(ByVal value As Double)
                _rangeScale = value
                CalculateData()
                Invalidate(False)
            End Set
        End Property

        Dim _useIntegratedVelocity As Boolean = True

        Public Property UseIntegratedVelocity() As Boolean
            Get
                Return _useIntegratedVelocity
            End Get
            Set(ByVal value As Boolean)
                _useIntegratedVelocity = value
                Invalidate()
                Call CalculateData()
            End Set
        End Property

        Public Property AttackerVelocity() As Double
            Get
                Return _sourceShip.Velocity
            End Get
            Set(ByVal value As Double)
                If _sourceShip IsNot Nothing Then
                    _sourceShip.Velocity = value
                    If _useIntegratedVelocity = False Then
                        Call CalculateData()
                    End If
                End If
            End Set
        End Property

        Public Property TargetVelocity() As Double
            Get
                Return _targetShip.Velocity
            End Get
            Set(ByVal value As Double)
                If _targetShip IsNot Nothing Then
                    _targetShip.Velocity = value
                    If _useIntegratedVelocity = False Then
                        Call CalculateData()
                    End If
                End If
            End Set
        End Property

#End Region

#Region "Control Properties"

        Dim _sourceShip As ShipStatus

        Public Property SourceShip() As ShipStatus
            Get
                Return _sourceShip
            End Get
            Set(ByVal value As ShipStatus)
                _sourceShip = value
                If value IsNot Nothing Then
                    Dim di1 As DraggableImage
                    Dim baseImage As Image = Core.ImageHandler.GetImage(CInt(SourceShip.Ship.ID))
                    Dim img As Bitmap
                    If baseImage IsNot Nothing Then
                        img = New Bitmap(baseImage, 32, 32)
                    Else
                        img = New Bitmap(My.Resources.EveSpaceAttacker, 32, 32)
                    End If
                    If _diList.ContainsKey("SourceShip") = False Then
                        _diList.Add("SourceShip", New DraggableImage("SourceShip", img, value.Location))
                        di1 = _diList("SourceShip")
                        _diList("SourceShip").Location = New Point(CInt(di1.Location.X - di1.Img.Width / 2), CInt(di1.Location.Y - di1.Img.Height / 2))
                    Else
                        _diList("SourceShip").Img = img
                    End If
                    If _diList.ContainsKey("SourceHeading") = False Then
                        _diList.Add("SourceHeading", New DraggableImage("SourceHeading", My.Resources.EveSpaceTarget, value.Heading))
                        di1 = _diList("SourceHeading")
                        _diList("SourceHeading").Location = New Point(CInt(di1.Location.X - di1.Img.Width / 2), CInt(di1.Location.Y - di1.Img.Height / 2))
                    Else
                        _diList("SourceHeading").Img = My.Resources.EveSpaceTarget
                    End If
                    CalculateData()
                    Invalidate(False)
                End If
            End Set
        End Property

        Dim _targetShip As ShipStatus

        Public Property TargetShip() As ShipStatus
            Get
                Return _targetShip
            End Get
            Set(ByVal value As ShipStatus)
                _targetShip = value
                If value IsNot Nothing Then
                    Dim di1 As DraggableImage
                    Dim baseImage As Image = Core.ImageHandler.GetImage(CInt(TargetShip.Ship.ID))
                    Dim img As Bitmap
                    If baseImage IsNot Nothing Then
                        img = New Bitmap(baseImage, 32, 32)
                    Else
                        img = New Bitmap(My.Resources.EveSpaceAttacker, 32, 32)
                    End If
                    If _diList.ContainsKey("TargetShip") = False Then
                        _diList.Add("TargetShip", New DraggableImage("TargetShip", img, value.Location))
                        di1 = _diList("TargetShip")
                        _diList("TargetShip").Location = New Point(CInt(di1.Location.X - di1.Img.Width / 2), CInt(di1.Location.Y - di1.Img.Height / 2))
                    Else
                        _diList("TargetShip").Img = img
                    End If
                    If _diList.ContainsKey("TargetHeading") = False Then
                        _diList.Add("TargetHeading", New DraggableImage("TargetHeading", My.Resources.EveSpaceTarget, value.Heading))
                        di1 = _diList("TargetHeading")
                        _diList("TargetHeading").Location = New Point(CInt(di1.Location.X - di1.Img.Width / 2), CInt(di1.Location.Y - di1.Img.Height / 2))
                    Else
                        _diList("TargetHeading").Img = My.Resources.EveSpaceTarget
                    End If
                    CalculateData()
                    Invalidate(False)
                End If
            End Set
        End Property


#End Region

#Region "Drag/Drop Routines"

        Private Sub MyMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDown
            For Each cdi As DraggableImage In _diList.Values
                If UseIntegratedVelocity = True Or (UseIntegratedVelocity = False And (cdi.Name = "SourceShip" Or cdi.Name = "TargetShip")) Then
                    If (e.X > cdi.Location.X And e.X < cdi.Location.X + cdi.Width And e.Y > cdi.Location.Y And e.Y < cdi.Location.Y + cdi.Height) Then
                        _di = cdi
                        _cl = New Point(e.X - _di.Location.X, e.Y - _di.Location.Y)
                        'Invalidate(False)
                    End If
                End If
            Next
        End Sub

        Private Sub MyMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseMove
            If _di IsNot Nothing Then
                _di.Location = New Point(e.X - _cl.X, e.Y - _cl.Y)
                ' Write in bounding conditions
                If _di.Location.X < 0 Then _di.Location = New Point(0, _di.Location.Y)
                If _di.Location.Y < 0 Then _di.Location = New Point(_di.Location.X, 0)
                If _di.Location.X > Width - _di.Width Then _di.Location = New Point(Width - _di.Width, _di.Location.Y)
                If _di.Location.Y > Height - _di.Height Then _di.Location = New Point(_di.Location.X, Height - _di.Height)
                Select Case _di.Name
                    Case "SourceShip"
                        _sourceShip.Location = _di.Centre
                    Case "TargetShip"
                        _targetShip.Location = _di.Centre
                    Case "SourceHeading"
                        _sourceShip.Heading = _di.Centre
                    Case "TargetHeading"
                        _targetShip.Heading = _di.Centre
                End Select
                Invalidate(False)
                CalculateData()
            End If
        End Sub

        Private Sub MyMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseUp
            _di = Nothing
            RaiseEvent GraphUpdateRequired()
        End Sub

#End Region

#Region "Control Paint Routines"

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            'MyBase.OnPaint(e)
            If _diList.Count > 0 Then

                If _diList.ContainsKey("SourceShip") And _diList.ContainsKey("TargetShip") Then
                    ' Define vector pens
                    Dim v1Pen As New Pen(Color.Green, 5)
                    v1Pen.EndCap = LineCap.ArrowAnchor
                    Dim v2Pen As New Pen(Color.Blue, 5)
                    v2Pen.EndCap = LineCap.ArrowAnchor

                    ' Define boundary checking pens
                    Dim bPen As New Pen(Color.FromArgb(255, 32, 32, 32), 1)
                    bPen.DashStyle = DashStyle.Dash
                    Dim rPen As New Pen(Color.FromArgb(255, 64, 64, 64), 1)
                    rPen.DashStyle = DashStyle.Dash

                    ' Set the graphics modes
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

                    ' Get images
                    Dim diS1 As DraggableImage = _diList("SourceShip")
                    Dim diH1 As DraggableImage = _diList("SourceHeading")
                    Dim diS2 As DraggableImage = _diList("TargetShip")
                    Dim diH2 As DraggableImage = _diList("TargetHeading")

                    ' Draw the horizontal and vertical vector lines
                    e.Graphics.DrawLine(bPen, 0, diS1.Centre.Y, Width, diS1.Centre.Y)
                    e.Graphics.DrawLine(bPen, diS1.Centre.X, 0, diS1.Centre.X, Height)
                    e.Graphics.DrawLine(bPen, 0, diS2.Centre.Y, Width, diS2.Centre.Y)
                    e.Graphics.DrawLine(bPen, diS2.Centre.X, 0, diS2.Centre.X, Height)

                    ' Draw some range circles
                    Dim transFont As New Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Pixel)
                    For dist As Integer = CInt(Width / 5) To CInt(Width * 6 / 5) Step CInt(Width / 5)
                        Dim strDist As String = (dist * _rangeScale).ToString & "km"
                        e.Graphics.DrawEllipse(rPen, diS1.Centre.X - dist, diS1.Centre.Y - dist, 2 * dist, 2 * dist)
                        Dim strWidth As SizeF = e.Graphics.MeasureString(strDist, transFont, 100)
                        e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X - strWidth.Width / 2, diS1.Centre.Y + dist)
                        e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X - strWidth.Width / 2, diS1.Centre.Y - dist)
                        e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X - dist, diS1.Centre.Y)
                        e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X + dist, diS1.Centre.Y)
                    Next

                    ' Draw Vector Lines
                    If _useIntegratedVelocity = True Then
                        e.Graphics.DrawLine(v1Pen, New Point(diS1.Centre.X, diS1.Centre.Y), New Point(diH1.Centre.X, diH1.Centre.Y))
                        e.Graphics.DrawLine(v2Pen, New Point(diS2.Centre.X, diS2.Centre.Y), New Point(diH2.Centre.X, diH2.Centre.Y))
                    End If

                    ' Draw the large intersection line
                    e.Graphics.DrawLine(New Pen(Color.Red), _lAxisIntersect, _rAxisIntersect)

                    ' Draw the points
                    e.Graphics.DrawImage(diS1.Img, diS1.Location)
                    e.Graphics.DrawImage(diS2.Img, diS2.Location)
                    If _useIntegratedVelocity = True Then
                        e.Graphics.DrawImage(diH1.Img, diH1.Location)
                        e.Graphics.DrawImage(diH2.Img, diH2.Location)
                    End If

                End If
            End If
        End Sub

#End Region

#Region "Control Initialisation Routines"

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            SetStyle(ControlStyles.DoubleBuffer, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.UserPaint, True)
        End Sub

#End Region

#Region "Calculation Routines"

        Private Sub CalculateData()
            If _sourceShip IsNot Nothing And _targetShip IsNot Nothing Then
                Dim xDist, yDist As Double
                Dim x1Dist, y1Dist, x2Dist, y2Dist As Double
                Dim xShDist, xThDist, yShDist, yThDist As Double
                xDist = (_targetShip.Location.X - _sourceShip.Location.X) * _rangeScale
                yDist = (_targetShip.Location.Y - _sourceShip.Location.Y) * _rangeScale
                _horizontalAngle = -Math.Atan(yDist / xDist)

                If yDist <> 0 Then
                    _range = yDist / Math.Sin(_horizontalAngle)
                Else
                    _range = xDist / Math.Cos(_horizontalAngle)
                End If
                _range = Math.Abs(_range)

                Dim minShip, maxShip As ShipStatus

                ' Calculate intersection with the axes
                If _sourceShip.Location.X > _targetShip.Location.X Then
                    maxShip = _sourceShip
                    minShip = _targetShip
                Else
                    minShip = _sourceShip
                    maxShip = _targetShip
                End If
                x1Dist = maxShip.Location.X
                x2Dist = Width - minShip.Location.X
                y1Dist = x1Dist * Math.Tan(_horizontalAngle)
                y2Dist = x2Dist * Math.Tan(-_horizontalAngle)
                If Math.Abs(_horizontalAngle / (2 * Math.PI) * 360) = 45 Then
                    Debug.Print(_horizontalAngle.ToString)
                End If
                If y1Dist < 1.0E+15 And y1Dist > -1.0E+15 Then
                    _lAxisIntersect = New Point(0, CInt(y1Dist + maxShip.Location.Y))
                    _rAxisIntersect = New Point(Width, CInt(y2Dist + minShip.Location.Y))
                Else
                    ' Catch overflow error where we never hit the x-axis i.e. is vertical
                    _lAxisIntersect = New Point(minShip.Location.X, 0)
                    _rAxisIntersect = New Point(maxShip.Location.X, Height)
                End If

                ' Calculate velocities based on headings
                If _useIntegratedVelocity = True Then
                    xShDist = (_sourceShip.Location.X - _sourceShip.Heading.X) * _velocityScale
                    yShDist = (_sourceShip.Location.Y - _sourceShip.Heading.Y) * _velocityScale
                    _sourceShip.Velocity = Math.Sqrt(Math.Pow(xShDist, 2) + Math.Pow(yShDist, 2))
                    xThDist = (_targetShip.Location.X - _targetShip.Heading.X) * _velocityScale
                    yThDist = (_targetShip.Location.Y - _targetShip.Heading.Y) * _velocityScale
                    _targetShip.Velocity = Math.Sqrt(Math.Pow(xThDist, 2) + Math.Pow(yThDist, 2))

                    ' Calculate formula of line of sight
                    Dim a, b, c, ms, ns, pds, mt, nt, pdt As Double
                    'Dim graphEquation As String = ""
                    If _rAxisIntersect.X = _lAxisIntersect.X Then
                        pds = xShDist
                        pdt = xThDist
                        'graphEquation = "x = " & (-c).ToString
                    Else
                        a = -(_rAxisIntersect.Y - _lAxisIntersect.Y) / (_rAxisIntersect.X - _lAxisIntersect.X)
                        b = 1
                        c = -_lAxisIntersect.Y
                        ms = _sourceShip.Heading.X
                        ns = _sourceShip.Heading.Y
                        pds = ((a * ms) + (b * ns) + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) * _velocityScale
                        mt = _targetShip.Heading.X
                        nt = _targetShip.Heading.Y
                        pdt = ((a * mt) + (b * nt) + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) * _velocityScale
                        'graphEquation = "y = "
                        If -a <> 0 Then
                            'graphEquation &= (-a).ToString & "x "
                            If -c < 0 Then
                                'graphEquation &= "- " & (c).ToString
                            Else
                                'graphEquation &= "+ " & (-c).ToString
                            End If
                        Else
                            'graphEquation &= (-c).ToString
                        End If
                    End If

                    ' Now calculate the transversal velocity and range
                    _transversal = Math.Abs(pds - pdt)
                Else
                    _transversal = _sourceShip.Velocity + _targetShip.Velocity
                End If

                RaiseEvent CalculationsChanged()

            End If
        End Sub

#End Region
    End Class

    Class DraggableImage

        Public Name As String
        Public Img As Bitmap
        Public Centre As Point
        Public Width As Integer
        Public Height As Integer
        Public CentreOffset As Point

        Dim _location As Point
        Public Property Location() As Point
            Get
                Return _location
            End Get
            Set(ByVal value As Point)
                _location = value
                Centre = New Point(_location.X + CentreOffset.X, _location.Y + CentreOffset.Y)
            End Set
        End Property

        Public Sub New(ByVal imageName As String, ByVal bmp As Bitmap, ByVal loc As Point)
            Img = bmp
            Name = imageName
            Height = bmp.Height
            Width = bmp.Width
            CentreOffset = New Point(CInt(Width / 2), CInt(Height / 2))
            Location = loc
        End Sub

    End Class

    Public Class ShipStatus
        Public Name As String
        Public Ship As Ship
        Public Location As New Point
        Public Heading As New Point
        Public Velocity As Double

        Public Sub New(ByVal statusName As String, ByVal fittedShip As Ship, ByVal startLocation As Point, ByVal startHeading As Point)
            Name = statusName
            Ship = fittedShip
            Location = startLocation
            Heading = StartHeading
        End Sub
    End Class

End Namespace