' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================

Imports System.Text
Imports System.Drawing
Imports System.Windows.Forms

Public Class EveSpace

    Dim cx, cy As Integer
    Dim cl, cl2 As Point
    Dim di As DraggableImage
    Dim DIs As New SortedList(Of String, DraggableImage)

#Region "Public Events"
    Public Event CalculationsChanged()
    Public Event GraphUpdateRequired()

#End Region

#Region "Calculation Variables"

    Dim cHorizontalAngle As Double
    Dim cVerticalAngle As Double
    Dim cLAxisIntersect As Point
    Dim cRAxisIntersect As Point
    Dim cTransversal As Double
    Public ReadOnly Property Transversal() As Double
        Get
            Return cTransversal
        End Get
    End Property
    Dim cRange As Double
    Public ReadOnly Property Range() As Double
        Get
            Return cRange
        End Get
    End Property
    Dim cVelocityScale As Double = 1
    Public Property VelocityScale() As Double
        Get
            Return cVelocityScale
        End Get
        Set(ByVal value As Double)
            cVelocityScale = value
            CalculateData(Nothing)
            Invalidate(False)
        End Set
    End Property
    Dim cRangeScale As Double = 1
    Public Property RangeScale() As Double
        Get
            Return cRangeScale
        End Get
        Set(ByVal value As Double)
            cRangeScale = value
            CalculateData(Nothing)
            Invalidate(False)
        End Set
    End Property
    Dim cUseIntegratedVelocity As Boolean = True
    Public Property UseIntegratedVelocity() As Boolean
        Get
            Return cUseIntegratedVelocity
        End Get
        Set(ByVal value As Boolean)
            cUseIntegratedVelocity = value
            Me.Invalidate()
            Call Me.CalculateData(Nothing)
        End Set
    End Property
    Public Property AttackerVelocity() As Double
        Get
            Return cSourceShip.Velocity
        End Get
        Set(ByVal value As Double)
            If cSourceShip IsNot Nothing Then
                cSourceShip.Velocity = value
                If cUseIntegratedVelocity = False Then
                    Call Me.CalculateData(Nothing)
                End If
            End If
        End Set
    End Property
    Public Property TargetVelocity() As Double
        Get
            Return cTargetShip.Velocity
        End Get
        Set(ByVal value As Double)
            If cTargetShip IsNot Nothing Then
                cTargetShip.Velocity = value
                If cUseIntegratedVelocity = False Then
                    Call Me.CalculateData(Nothing)
                End If
            End If
        End Set
    End Property
#End Region

#Region "Control Properties"

    Dim cSourceShip As ShipStatus
    Public Property SourceShip() As ShipStatus
        Get
            Return cSourceShip
        End Get
        Set(ByVal value As ShipStatus)
            cSourceShip = value
            If value IsNot Nothing Then
                Dim DI1 As DraggableImage
                Dim baseImage As Image = EveHQ.Core.ImageHandler.GetImage(SourceShip.Ship.ID)
                Dim img As Bitmap
                If baseImage IsNot Nothing Then
                    img = New Bitmap(baseImage, 32, 32)
                Else
                    img = New Bitmap(My.Resources.EveSpaceAttacker, 32, 32)
                End If
                If DIs.ContainsKey("SourceShip") = False Then
                    DIs.Add("SourceShip", New DraggableImage("SourceShip", img, value.Location))
                    DI1 = DIs("SourceShip")
                    DIs("SourceShip").Location = New Point(CInt(DI1.Location.X - DI1.img.Width / 2), CInt(DI1.Location.Y - DI1.img.Height / 2))
                Else
                    DIs("SourceShip").img = img
                End If
                If DIs.ContainsKey("SourceHeading") = False Then
                    DIs.Add("SourceHeading", New DraggableImage("SourceHeading", My.Resources.EveSpaceTarget, value.Heading))
                    DI1 = DIs("SourceHeading")
                    DIs("SourceHeading").Location = New Point(CInt(DI1.Location.X - DI1.img.Width / 2), CInt(DI1.Location.Y - DI1.img.Height / 2))
                Else
                    DIs("SourceHeading").img = My.Resources.EveSpaceTarget
                End If
                CalculateData(Nothing)
                Invalidate(False)
            End If
        End Set
    End Property

    Dim cTargetShip As ShipStatus
    Public Property TargetShip() As ShipStatus
        Get
            Return cTargetShip
        End Get
        Set(ByVal value As ShipStatus)
            cTargetShip = value
            If value IsNot Nothing Then
                Dim DI1 As DraggableImage
                Dim baseImage As Image = EveHQ.Core.ImageHandler.GetImage(TargetShip.Ship.ID)
                Dim img As Bitmap
                If baseImage IsNot Nothing Then
                    img = New Bitmap(baseImage, 32, 32)
                Else
                    img = New Bitmap(My.Resources.EveSpaceAttacker, 32, 32)
                End If
                If DIs.ContainsKey("TargetShip") = False Then
                    DIs.Add("TargetShip", New DraggableImage("TargetShip", img, value.Location))
                    DI1 = DIs("TargetShip")
                    DIs("TargetShip").Location = New Point(CInt(DI1.Location.X - DI1.img.Width / 2), CInt(DI1.Location.Y - DI1.img.Height / 2))
                Else
                    DIs("TargetShip").img = img
                End If
                If DIs.ContainsKey("TargetHeading") = False Then
                    DIs.Add("TargetHeading", New DraggableImage("TargetHeading", My.Resources.EveSpaceTarget, value.Heading))
                    DI1 = DIs("TargetHeading")
                    DIs("TargetHeading").Location = New Point(CInt(DI1.Location.X - DI1.img.Width / 2), CInt(DI1.Location.Y - DI1.img.Height / 2))
                Else
                    DIs("TargetHeading").img = My.Resources.EveSpaceTarget
                End If
                CalculateData(Nothing)
                Invalidate(False)
            End If
        End Set
    End Property


#End Region

#Region "Drag/Drop Routines"

    Private Sub MyMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        For Each cdi As DraggableImage In DIs.Values
            If UseIntegratedVelocity = True Or (UseIntegratedVelocity = False And (cdi.Name = "SourceShip" Or cdi.Name = "TargetShip")) Then
                If (e.X > cdi.Location.X And e.X < cdi.Location.X + cdi.Width And e.Y > cdi.Location.Y And e.Y < cdi.Location.Y + cdi.Height) Then
                    di = cdi
                    cl = New Point(e.X - di.Location.X, e.Y - di.Location.Y)
                    'Invalidate(False)
                End If
            End If
        Next
    End Sub

    Private Sub MyMouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If di IsNot Nothing Then
            di.Location = New Point(e.X - cl.X, e.Y - cl.Y)
            ' Write in bounding conditions
            If di.Location.X < 0 Then di.Location = New Point(0, di.Location.Y)
            If di.Location.Y < 0 Then di.Location = New Point(di.Location.X, 0)
            If di.Location.X > Me.Width - di.Width Then di.Location = New Point(Me.Width - di.Width, di.Location.Y)
            If di.Location.Y > Me.Height - di.Height Then di.Location = New Point(di.Location.X, Me.Height - di.Height)
            Select Case di.Name
                Case "SourceShip"
                    cSourceShip.Location = di.Centre
                Case "TargetShip"
                    cTargetShip.Location = di.Centre
                Case "SourceHeading"
                    cSourceShip.Heading = di.Centre
                Case "TargetHeading"
                    cTargetShip.Heading = di.Centre
            End Select
            Invalidate(False)
            Me.CalculateData(Nothing)
        End If
    End Sub

    Private Sub MyMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        di = Nothing
        RaiseEvent GraphUpdateRequired()
    End Sub

#End Region

#Region "Control Paint Routines"

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        'MyBase.OnPaint(e)
        If DIs.Count > 0 Then

            If DIs.ContainsKey("SourceShip") And DIs.ContainsKey("TargetShip") Then
                ' Define vector pens
                Dim V1Pen As New Pen(Color.Green, 5)
                V1Pen.EndCap = Drawing2D.LineCap.ArrowAnchor
                Dim V2Pen As New Pen(Color.Blue, 5)
                V2Pen.EndCap = Drawing2D.LineCap.ArrowAnchor

                ' Define boundary checking pens
                Dim BPen As New Pen(Color.FromArgb(255, 32, 32, 32), 1) : BPen.DashStyle = Drawing2D.DashStyle.Dash
                Dim RPen As New Pen(Color.FromArgb(255, 64, 64, 64), 1) : RPen.DashStyle = Drawing2D.DashStyle.Dash

                ' Set the graphics modes
                e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

                ' Get images
                Dim diS1 As DraggableImage = DIs("SourceShip")
                Dim diH1 As DraggableImage = DIs("SourceHeading")
                Dim diS2 As DraggableImage = DIs("TargetShip")
                Dim diH2 As DraggableImage = DIs("TargetHeading")

                ' Draw the horizontal and vertical vector lines
                e.Graphics.DrawLine(BPen, 0, diS1.Centre.Y, Me.Width, diS1.Centre.Y)
                e.Graphics.DrawLine(BPen, diS1.Centre.X, 0, diS1.Centre.X, Me.Height)
                e.Graphics.DrawLine(BPen, 0, diS2.Centre.Y, Me.Width, diS2.Centre.Y)
                e.Graphics.DrawLine(BPen, diS2.Centre.X, 0, diS2.Centre.X, Me.Height)

                ' Draw some range circles
                Dim transFont As New Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Pixel)
                For dist As Integer = CInt(Me.Width / 5) To CInt(Me.Width * 6 / 5) Step CInt(Me.Width / 5)
                    Dim strDist As String = (dist * cRangeScale).ToString & "km"
                    e.Graphics.DrawEllipse(RPen, diS1.Centre.X - dist, diS1.Centre.Y - dist, 2 * dist, 2 * dist)
                    Dim strWidth As SizeF = e.Graphics.MeasureString(strDist, transFont, 100)
                    e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X - strWidth.Width / 2, diS1.Centre.Y + dist)
                    e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X - strWidth.Width / 2, diS1.Centre.Y - dist)
                    e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X - dist, diS1.Centre.Y)
                    e.Graphics.DrawString(strDist, transFont, Brushes.DarkGray, diS1.Centre.X + dist, diS1.Centre.Y)
                Next

                ' Draw Vector Lines
                If cUseIntegratedVelocity = True Then
                    e.Graphics.DrawLine(V1Pen, New Point(diS1.Centre.X, diS1.Centre.Y), New Point(diH1.Centre.X, diH1.Centre.Y))
                    e.Graphics.DrawLine(V2Pen, New Point(diS2.Centre.X, diS2.Centre.Y), New Point(diH2.Centre.X, diH2.Centre.Y))
                End If

                ' Draw the large intersection line
                e.Graphics.DrawLine(New Pen(Color.Red), cLAxisIntersect, cRAxisIntersect)

                ' Draw the points
                e.Graphics.DrawImage(diS1.img, diS1.Location)
                e.Graphics.DrawImage(diS2.img, diS2.Location)
                If cUseIntegratedVelocity = True Then
                    e.Graphics.DrawImage(diH1.img, diH1.Location)
                    e.Graphics.DrawImage(diH2.img, diH2.Location)
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
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)

    End Sub

#End Region

#Region "Calculation Routines"

    Private Sub CalculateData(ByVal state As Object)
        If cSourceShip IsNot Nothing And cTargetShip IsNot Nothing Then
            Dim xDist, yDist As Double
            Dim x1Dist, y1Dist, x2Dist, y2Dist As Double
            Dim xSHDist, xTHDist, ySHDist, yTHDist As Double
            xDist = (cTargetShip.Location.X - cSourceShip.Location.X) * cRangeScale
            yDist = (cTargetShip.Location.Y - cSourceShip.Location.Y) * cRangeScale
            cHorizontalAngle = -Math.Atan(yDist / xDist)
            cVerticalAngle = (Math.PI / 2) - cHorizontalAngle

            If yDist <> 0 Then
                cRange = yDist / Math.Sin(cHorizontalAngle)
            Else
                cRange = xDist / Math.Cos(cHorizontalAngle)
            End If
            cRange = Math.Abs(cRange)

            Dim minShip, maxShip As ShipStatus

            ' Calculate intersection with the axes
            If cSourceShip.Location.X > cTargetShip.Location.X Then
                maxShip = cSourceShip
                minShip = cTargetShip
            Else
                minShip = cSourceShip
                maxShip = cTargetShip
            End If
            x1Dist = maxShip.Location.X
            x2Dist = Me.Width - minShip.Location.X
            y1Dist = x1Dist * Math.Tan(cHorizontalAngle)
            y2Dist = x2Dist * Math.Tan(-cHorizontalAngle)
            If Math.Abs(cHorizontalAngle / (2 * Math.PI) * 360) = 45 Then
                Debug.Print(cHorizontalAngle.ToString)
            End If
            If y1Dist < 1.0E+15 And y1Dist > -1.0E+15 Then
                cLAxisIntersect = New Point(0, CInt(y1Dist + maxShip.Location.Y))
                cRAxisIntersect = New Point(Me.Width, CInt(y2Dist + minShip.Location.Y))
            Else
                ' Catch overflow error where we never hit the x-axis i.e. is vertical
                cLAxisIntersect = New Point(minShip.Location.X, 0)
                cRAxisIntersect = New Point(maxShip.Location.X, Me.Height)
            End If

            ' Calculate velocities based on headings
            If cUseIntegratedVelocity = True Then
                xSHDist = (cSourceShip.Location.X - cSourceShip.Heading.X) * cVelocityScale
                ySHDist = (cSourceShip.Location.Y - cSourceShip.Heading.Y) * cVelocityScale
                cSourceShip.Velocity = Math.Sqrt(Math.Pow(xSHDist, 2) + Math.Pow(ySHDist, 2))
                xTHDist = (cTargetShip.Location.X - cTargetShip.Heading.X) * cVelocityScale
                yTHDist = (cTargetShip.Location.Y - cTargetShip.Heading.Y) * cVelocityScale
                cTargetShip.Velocity = Math.Sqrt(Math.Pow(xTHDist, 2) + Math.Pow(yTHDist, 2))

                ' Calculate formula of line of sight
                Dim a, b, c, ms, ns, pds, mt, nt, pdt As Double
                Dim graphEquation As String = ""
                If cRAxisIntersect.X = cLAxisIntersect.X Then
                    a = 0
                    b = 0
                    c = -cSourceShip.Location.X
                    pds = xSHDist
                    pdt = xTHDist
                    graphEquation = "x = " & (-c).ToString
                Else
                    a = -(cRAxisIntersect.Y - cLAxisIntersect.Y) / (cRAxisIntersect.X - cLAxisIntersect.X)
                    b = 1
                    c = -cLAxisIntersect.Y
                    ms = cSourceShip.Heading.X
                    ns = cSourceShip.Heading.Y
                    pds = ((a * ms) + (b * ns) + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) * cVelocityScale
                    mt = cTargetShip.Heading.X
                    nt = cTargetShip.Heading.Y
                    pdt = ((a * mt) + (b * nt) + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) * cVelocityScale
                    graphEquation = "y = "
                    If -a <> 0 Then
                        graphEquation &= (-a).ToString & "x "
                        If -c < 0 Then
                            graphEquation &= "- " & (c).ToString
                        Else
                            graphEquation &= "+ " & (-c).ToString
                        End If
                    Else
                        graphEquation &= (-c).ToString
                    End If
                End If

                ' Now calculate the transversal velocity and range
                cTransversal = Math.Abs(pds - pdt)
            Else
                cTransversal = cSourceShip.Velocity + cTargetShip.Velocity
            End If

            RaiseEvent CalculationsChanged()

        End If
    End Sub

#End Region

End Class

Class DraggableImage

    Public Name As String
    Public img As Bitmap
    Public Centre As Point
    Public Width As Integer
    Public Height As Integer
    Public CentreOffset As Point

    Dim cLocation As Point
    Public Property Location() As Point
        Get
            Return cLocation
        End Get
        Set(ByVal value As Point)
            cLocation = value
            Me.Centre = New Point(cLocation.X + CentreOffset.X, cLocation.Y + CentreOffset.Y)
        End Set
    End Property

    Public Sub New(ByVal imageName As String, ByVal bmp As Bitmap, ByVal loc As Point)
        Me.img = bmp
        Me.Name = imageName
        Me.Height = bmp.Height
        Me.Width = bmp.Width
        Me.CentreOffset = New Point(CInt(Me.Width / 2), CInt(Me.Height / 2))
        Me.Location = loc
    End Sub

End Class

Public Class ShipStatus
    Public Name As String
    Public Ship As HQF.Ship
    Public Location As New Point
    Public Heading As New Point
    Public Velocity As Double

    Public Sub New(ByVal StatusName As String, ByVal FittedShip As Ship, ByVal StartLocation As Point, ByVal StartHeading As Point)
        Me.Name = StatusName
        Me.Ship = FittedShip
        Me.Location = StartLocation
        Me.Heading = StartHeading
    End Sub
End Class


