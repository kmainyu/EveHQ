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
Imports System.Reflection
Imports System.Globalization
Imports EveHQ.Core

Namespace Controls.DBControls

    Public Class Widget

        ''' <summary>
        ''' Initialises a new widget
        ''' Any UI updates can be done in the inheriting class (e.g. populating combo boxes)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

#Region "Public Property Variables"
        ' The interface property variables hold local copies of the public property data
        ' and also set default size and positions of the widget
        ' ReSharper disable InconsistentNaming - retain names for old serialisation routines
        Dim cControlSize As Size = New Size(300, 220)
        Dim cControlLocation As Point = New Point(0, 0)
        Dim cControlConfig As New SortedList(Of String, Object)
        Dim cControlConfigForm As String = ""
        Dim cControlConfigInfo As String = ""
        ' ReSharper restore InconsistentNaming
#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Stores the configuration of the control as a key/value pair.
        ''' The configuration key represents an actual property name in the control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ControlConfiguration() As SortedList(Of String, Object)
            Get
                ' Check for ControlName
                Call SetConfig("ControlName", ControlName)
                Return cControlConfig
            End Get
            Set(ByVal value As SortedList(Of String, Object))
                cControlConfig = value
                Call ReadFromConfig()
            End Set
        End Property

        ''' <summary>
        ''' Stores the location of the control relative to the edge of the dashboard
        ''' </summary>
        ''' <value></value>
        ''' <returns>The location of the control</returns>
        ''' <remarks></remarks>
        Public Property ControlLocation() As Point
            Get
                ' Return the current value
                Return cControlLocation
            End Get
            Set(ByVal value As Point)
                ' Set the current value
                cControlLocation = value
                ' Set the control location
                Location = value
                ' Store the current location in the configuration data
                If ReadConfig = False Then
                    SetConfig("ControlLocation", value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' This property should return the name of the control as a string.
        ''' This can be held in a private variable or return directly from the Property Get method
        ''' </summary>
        ''' <value></value>
        ''' <returns>The name of the control</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property ControlName() As String
            Get
                ' Return the name of the control
                Return "Base Control"
            End Get
        End Property

        ''' <summary>
        ''' Stores the size of the control in pixels
        ''' </summary>
        ''' <value></value>
        ''' <returns>The size of the control</returns>
        ''' <remarks></remarks>
        Public Property ControlSize() As Size
            Get
                ' Return the current value
                Return cControlSize
            End Get
            Set(ByVal value As Size)
                ' Set the current value
                cControlSize = value
                ' Set the control size
                Size = cControlSize
                ' Store the current size in the configuration data
                If ReadConfig = False Then
                    SetConfig("ControlSize", value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Stores the name of the form to be used as a configuration screen for the widget
        ''' </summary>
        ''' <value></value>
        ''' <returns>The name of the widget configuration form</returns>
        ''' <remarks></remarks>
        Public Property ControlConfigForm() As String
            Get
                ' Return the current value
                Return cControlConfigForm
            End Get
            Set(ByVal value As String)
                ' Set the current value
                cControlConfigForm = value
                If value = "" Then
                    pbConfig.Visible = False
                Else
                    pbConfig.Visible = True
                    pbConfig.BringToFront()
                End If
                ' Store the current size in the configuration data
                If ReadConfig = False Then
                    SetConfig("ControlConfigForm", value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the details to be shown in a configuration settings summary
        ''' Should be overridden by the inheriting class and set on initialisation
        ''' </summary>
        ''' <value></value>
        ''' <returns>A string containing a summary of the configuration</returns>
        ''' <remarks></remarks>
        Public Property ControlConfigInfo() As String
            Get
                ' Return the current value
                Return cControlConfigInfo
            End Get
            Set(ByVal value As String)
                ' Set the current value
                cControlConfigInfo = value
                ' Store the current size in the configuration data
                If ReadConfig = False Then
                    SetConfig("ControlConfigInfo", value)
                End If
            End Set
        End Property

#End Region

#Region "Class Variables"
        ' ReadConfig is required to store the state of the configuration reader so not as to keep setting
        ' properties when reading and writing configuration data
        Protected ReadConfig As Boolean = False
#End Region

#Region "Control Configuration"
        ' These 2 routines must be kept as they are to allow configuration data to be read and written correctly

        ''' <summary>
        ''' Reads configuration data from the ControlConfiguration property and sets the relevant properties using Reflection
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub ReadFromConfig()
            ReadConfig = True
            Dim oldConfigProperties As New ArrayList
            For config As Integer = 0 To cControlConfig.Keys.Count - 1
                Dim configProperty As String = cControlConfig.Keys(config)
                Dim pi As PropertyInfo = Me.GetType().GetProperty(configProperty)
                If configProperty <> "ControlName" Then
                    ' Adjust for size and location not interpreted correctly due to weakly typed references in the control config
                    Select Case configProperty
                        Case "ControlLocation"
                            If TypeOf cControlConfig(configProperty) Is String Then
                                Dim baseValue As String = CStr(cControlConfig(configProperty)).Trim("{}".ToCharArray)
                                Dim parts As List(Of String) = baseValue.Split(",".ToCharArray).ToList
                                cControlConfig(configProperty) = New Point(CInt(parts(0).Split("=".ToCharArray).Last), CInt(parts(1).Split("=".ToCharArray).Last))
                            End If
                        Case "ControlSize"
                            If TypeOf cControlConfig(configProperty) Is String Then
                                Dim baseValue As String = CStr(cControlConfig(configProperty)).Trim("{}".ToCharArray)
                                Dim parts As List(Of String) = baseValue.Split(",".ToCharArray).ToList
                                cControlConfig(configProperty) = New Size(CInt(parts(0).Split("=".ToCharArray).Last), CInt(parts(1).Split("=".ToCharArray).Last))
                            End If
                    End Select
                Try
                    pi.SetValue(Me, Convert.ChangeType(cControlConfig(configProperty), pi.PropertyType, CultureInfo.InvariantCulture), Nothing)
                Catch e As Exception
                    oldConfigProperties.Add(configProperty)
                    Continue For
                End Try
                End If
            Next
            ' Delete any old config properties
            For Each configProperty As String In oldConfigProperties
                cControlConfig.Remove(configProperty)
            Next
            ReadConfig = False
        End Sub

        ''' <summary>
        ''' Updates the ControlConfiguration data with the relevant data
        ''' </summary>
        ''' <param name="configProperty"></param>
        ''' <param name="configData"></param>
        ''' <remarks></remarks>
        Protected Sub SetConfig(ByVal configProperty As String, ByVal configData As Object)
            If cControlConfig.ContainsKey(configProperty) = False Then
                cControlConfig.Add(configProperty, ConfigData.ToString)
            Else
                cControlConfig(configProperty) = ConfigData.ToString
            End If
        End Sub
#End Region

#Region "Private Methods"
        Private Sub pbConfig_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pbConfig.MouseDoubleClick
            ' Open the configuration form if one exists
            If ControlConfigForm <> "" Then
                Dim configType As Type = Assembly.GetExecutingAssembly.GetType(ControlConfigForm)
                Using configForm As Form = CType(Activator.CreateInstance(configType), Form)
                    Dim pi As PropertyInfo = configForm.GetType().GetProperty("DBWidget")
                    pi.SetValue(configForm, Me, Nothing)
                    configForm.ShowDialog()
                End Using
            End If
        End Sub
#End Region

#Region "Shadow Code"

        'Private Enum ShadowDirections As Integer
        '    TOP_RIGHT = 1
        '    BOTTOM_RIGHT = 2
        '    BOTTOM_LEFT = 3
        '    TOP_LEFT = 4
        'End Enum

        'Private Sub DropShadow(ByRef SourceControl As DevComponents.DotNetBar.LabelX, ByVal ShadowColor As Drawing.Color, ByVal BackgroundColor As Drawing.Color, Optional ByVal ShadowDirection As ShadowDirections = ShadowDirections.BOTTOM_RIGHT, Optional ByVal ShadowOpacity As Integer = 190, Optional ByVal ShadowSoftness As Integer = 4, Optional ByVal ShadowDistance As Integer = 5, Optional ByVal ShadowRoundedEdges As Boolean = True)
        '    Dim ImgTarget As Bitmap = Nothing
        '    Dim ImgShadow As Bitmap = Nothing
        '    Dim g As Graphics = Nothing
        '    Try
        '        If SourceControl IsNot Nothing Then
        '            If ShadowOpacity < 0 Then
        '                ShadowOpacity = 0
        '            ElseIf ShadowOpacity > 255 Then
        '                ShadowOpacity = 255
        '            End If
        '            If ShadowSoftness < 1 Then
        '                ShadowSoftness = 1
        '            ElseIf ShadowSoftness > 30 Then
        '                ShadowSoftness = 30
        '            End If
        '            If ShadowDistance < 1 Then
        '                ShadowDistance = 1
        '            ElseIf ShadowDistance > 50 Then
        '                ShadowDistance = 50
        '            End If
        '            If ShadowColor = Color.Transparent Then
        '                ShadowColor = Color.Black
        '            End If
        '            If BackgroundColor = Color.Transparent Then
        '                BackgroundColor = Color.White
        '            End If

        '            'get shadow
        '            Dim shWidth As Integer = CInt(SourceControl.Width / ShadowSoftness)
        '            Dim shHeight As Integer = CInt(SourceControl.Height / ShadowSoftness)
        '            ImgShadow = New Bitmap(shWidth, shHeight)
        '            g = Graphics.FromImage(ImgShadow)
        '            g.Clear(Color.Transparent)
        '            g.InterpolationMode = InterpolationMode.HighQualityBicubic
        '            g.SmoothingMode = SmoothingMode.AntiAlias
        '            Dim sre As Integer = 0
        '            If ShadowRoundedEdges = True Then sre = 1
        '            g.FillRectangle(New SolidBrush(Color.FromArgb(ShadowOpacity, ShadowColor)), _
        '            sre, sre, shWidth, shHeight)
        '            g.Dispose()

        '            'draw shadow
        '            Dim d_shWidth As Integer = SourceControl.Width + ShadowDistance
        '            Dim d_shHeight As Integer = SourceControl.Height + ShadowDistance
        '            ImgTarget = New Bitmap(d_shWidth, d_shHeight)
        '            g = AGPContent.CreateGraphics
        '            g.InterpolationMode = InterpolationMode.HighQualityBicubic
        '            g.SmoothingMode = SmoothingMode.AntiAlias
        '            g.DrawImage(ImgShadow, New Rectangle(SourceControl.Left, SourceControl.Top, d_shWidth, d_shHeight), _
        '            0, 0, ImgShadow.Width, ImgShadow.Height, GraphicsUnit.Pixel)

        '            g.Dispose()
        '            g = Nothing
        '            ImgShadow.Dispose()
        '            ImgShadow = Nothing

        '            ImgTarget.Dispose()
        '            ImgTarget = Nothing
        '        End If

        '    Catch ex As Exception
        '        If g IsNot Nothing Then
        '            g.Dispose()
        '            g = Nothing
        '        End If
        '        If ImgShadow IsNot Nothing Then
        '            ImgShadow.Dispose()
        '            ImgShadow = Nothing
        '        End If
        '        If ImgTarget IsNot Nothing Then
        '            ImgTarget.Dispose()
        '            ImgTarget = Nothing
        '        End If
        '    End Try

        'End Sub

        'Private Sub Shadow_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles lblHeader.Paint
        '    Call DropShadow(lblHeader, Color.Black, Color.White, ShadowDirections.BOTTOM_RIGHT, 90, 4, 5, True)
        'End Sub

#End Region

#Region "Custom Control Properties"
        ' Add any public properties that need to be exposed so that the control can be updated with external data
#End Region

#Region "Custom Control Variables"
        ' Holds the local copy of the custom control properties
#End Region

#Region "Widget Removal Code"

        Private Sub RemoveWidgetToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveWidgetToolStripMenuItem.Click
            ' Go through the widgets and see if we can match the widget from the control config info
            Dim removeIndex As Integer = -1
            For Each checkConfig As SortedList(Of String, Object) In HQ.Settings.DashboardConfiguration
                Dim ccInfo As String = CStr(checkConfig("ControlConfigInfo"))
                If ccInfo = cControlConfigInfo And CStr(checkConfig("ControlName")) = ControlName Then
                    If Location.ToString = checkConfig("ControlLocation").ToString Then
                        ' This matches our type, config and location, so must be the one!
                        removeIndex = HQ.Settings.DashboardConfiguration.IndexOf(checkConfig)
                        Exit For
                    End If
                End If
            Next
            If removeIndex > -1 Then
                HQ.Settings.DashboardConfiguration.RemoveAt(removeIndex)
                ' Update the dashboard
                Forms.frmDashboard.UpdateWidgets()
            End If
        End Sub

#End Region


    End Class
End NameSpace