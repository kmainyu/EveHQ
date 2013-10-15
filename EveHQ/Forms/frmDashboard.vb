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
Imports EveHQ.Core
Imports System.Reflection

Namespace Forms

    Public Class FrmDashboard
        Dim _isDragging As Boolean = False
        Dim _isResizing As Boolean = False
        Dim _isResizable As Boolean = False
        Dim _resizeDirection As ResizeDirections
        Dim _resizePosition As ResizePositions
        Private Const ControlBorder As Integer = 4
        Private Const BorderOffset As Integer = 1
        Dim _initialCoords As Point
        Dim _initialLocation As Point
        Dim _initialSize As Size
        Dim _endPoint As Point
        Dim _sourceControl As Control
        Dim _parentControl As Control
        Friend Ticker1 As New Ticker

#Region "Form Loading Routines"

        Private Sub frmDashboard_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Set the options
            Ticker1.Visible = HQ.Settings.DBTicker
            Select Case HQ.Settings.DBTickerLocation
                Case "Top"
                    Ticker1.Dock = DockStyle.Top
                Case "Bottom"
                    Ticker1.Dock = DockStyle.Bottom
            End Select

            ' Add the controls to the panel
            Call UpdateWidgets()

        End Sub
#End Region

#Region "Widget Update Routines"

        Public Sub UpdateWidgets()

            panelDB.SuspendLayout()
            ' Remove event handlers
            For c As Integer = panelDB.Controls.Count - 1 To 0 Step -1
                Dim control As Control = panelDB.Controls(c)
                RemoveHandler control.Controls("AGPContent").MouseDown, AddressOf MyMouseDown
                RemoveHandler control.Controls("AGPContent").MouseUp, AddressOf MyMouseUp
                RemoveHandler control.Controls("AGPContent").MouseMove, AddressOf MyMouseMove
                RemoveHandler control.Controls("AGPContent").MouseLeave, AddressOf MyMouseLeave
                'RemoveHandler control.Resize, AddressOf MyControlResize
                'RemoveHandler control.Move, AddressOf MyControlMove
                Try
                    RemoveHandler control.Controls("AGPContent").Controls("lblHeader").MouseDown, AddressOf MyMouseDown
                    RemoveHandler control.Controls("AGPContent").Controls("lblHeader").MouseUp, AddressOf MyMouseUp
                    RemoveHandler control.Controls("AGPContent").Controls("lblHeader").MouseMove, AddressOf MyMouseMove
                Catch e As Exception
                End Try
                control.Dispose()
            Next

            ' Clear Controls
            panelDB.Controls.Clear()
            For Each config As SortedList(Of String, Object) In HQ.Settings.DashboardConfiguration
                Call AddWidget(config)
            Next

            If panelDB.Controls.Count = 0 Then
                panelDB.Text = "Right-click this panel and configure the dashboard to add or edit widgets."
            Else
                panelDB.Text = ""
            End If

            panelDB.ResumeLayout()

        End Sub

        Private Sub AddWidget(config As SortedList(Of String, Object))
            Dim widgetName As String = CStr(config("ControlName"))
            If HQ.Widgets.ContainsKey(widgetName) = True Then
                Dim className As String = HQ.Widgets(widgetName)
                Dim myType As Type = Assembly.GetExecutingAssembly.GetType(className)
                Dim newWidget As Object = Activator.CreateInstance(myType)
                Dim pi As PropertyInfo = myType.GetProperty("ControlConfiguration")
                pi.SetValue(newWidget, config, Nothing)
                Dim control As Control = CType(newWidget, Control)
                panelDB.Controls.Add(control)
                AddHandler control.Controls("AGPContent").MouseDown, AddressOf MyMouseDown
                AddHandler control.Controls("AGPContent").MouseUp, AddressOf MyMouseUp
                AddHandler control.Controls("AGPContent").MouseMove, AddressOf MyMouseMove
                AddHandler control.Controls("AGPContent").MouseLeave, AddressOf MyMouseLeave
                'AddHandler control.Resize, AddressOf MyControlResize
                'AddHandler control.Move, AddressOf MyControlMove
                Try
                    AddHandler control.Controls("AGPContent").Controls("lblHeader").MouseDown, AddressOf MyMouseDown
                    AddHandler control.Controls("AGPContent").Controls("lblHeader").MouseUp, AddressOf MyMouseUp
                    AddHandler control.Controls("AGPContent").Controls("lblHeader").MouseMove, AddressOf MyMouseMove
                Catch e As Exception
                End Try
            End If
        End Sub

#End Region

#Region "Panel Drag/Drop Routines"

        'Private Sub MyControlResize(ByVal sender As Object, ByVal e As EventArgs)
        '    lblStatus.Text = "Status: Resizing " & parentControl.Name & " (" & ResizePosition.ToString & ") from (" & initialSize.Width.ToString & "," & initialSize.Height.ToString & ") to (" & parentControl.Width.ToString & "," & parentControl.Height.ToString & ")"
        'End Sub

        'Private Sub MyControlMove(ByVal sender As Object, ByVal e As EventArgs)
        '    lblStatus.Text = "Status: Dragging " & parentControl.Name & " from (" & initialLocation.X.ToString & "," & initialLocation.Y.ToString & ") to (" & parentControl.Location.X.ToString & "," & parentControl.Location.Y.ToString & ")"
        'End Sub

        Private Sub MyMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
            ' Get the source control
            _sourceControl = CType(sender, Control)
            _parentControl = _sourceControl
            Do
                _parentControl = _parentControl.Parent
            Loop Until _parentControl.Parent Is panelDB
            _parentControl.BringToFront()
            ' Establish cursor position to determine routine
            If _isResizable = True Then
                _isResizing = True
                _initialCoords = e.Location
                _initialLocation = _parentControl.Location
                _initialSize = _parentControl.Size
                _endPoint = New Point(_initialLocation.X + _initialSize.Width, _initialLocation.Y + _initialSize.Height)
                'lblStatus.Text = "Status: Resizing " & parentControl.Name & " (" & ResizePosition.ToString & ") from (" & initialSize.Width.ToString & "," & initialSize.Height.ToString & ") to (" & parentControl.Width.ToString & "," & parentControl.Height.ToString & ")"
            Else
                ' Disable dragging from the AGPContent panel, only allow from the Header
                If _sourceControl.Name = "lblHeader" Then
                    _isDragging = True
                    _parentControl.Cursor = Cursors.SizeAll
                    _initialCoords = e.Location
                    _initialLocation = _parentControl.Location
                    _initialSize = _parentControl.Size
                    _endPoint = New Point(_initialLocation.X + _initialSize.Width, _initialLocation.Y + _initialSize.Height)
                    'lblStatus.Text = "Status: Dragging " & parentControl.Name & " from (" & initialLocation.X.ToString & "," & initialLocation.Y.ToString & ") to (" & parentControl.Location.X.ToString & "," & parentControl.Location.Y.ToString & ")"
                End If
            End If
        End Sub

        Private Sub MyMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
            ' Get the source control
            _sourceControl = CType(sender, Control)
            _parentControl = _sourceControl
            Do
                _parentControl = _parentControl.Parent
            Loop Until _parentControl.Parent Is panelDB
            If _isDragging = True Then
                ' Dragging code
                Dim pi As PropertyInfo = _parentControl.GetType().GetProperty("ControlLocation")
                pi.SetValue(_parentControl, _parentControl.Location, Nothing)
                _isDragging = False
            ElseIf _isResizing = True Then
                ' Resizing Code
                Dim pi As PropertyInfo = _parentControl.GetType().GetProperty("ControlSize")
                pi.SetValue(_parentControl, _parentControl.Size, Nothing)
                _isResizing = False
            End If
            'lblStatus.Text = "Status: Waiting..."
            _parentControl.Cursor = Cursors.Default
        End Sub

        Private Sub MyMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            ' Get the source control
            _sourceControl = CType(sender, Control)
            _parentControl = _sourceControl
            Do
                _parentControl = _parentControl.Parent
            Loop Until _parentControl.Parent Is panelDB
            If _isDragging = True Then
                ' Dragging code
                Dim cx As Integer = _parentControl.Location.X - _initialCoords.X + e.Location.X
                Dim cy As Integer = _parentControl.Location.Y - _initialCoords.Y + e.Location.Y
                _parentControl.Location = New Point(Math.Max(0, Math.Min(cx, panelDB.Width - 32)), Math.Max(0, Math.Min(cy, panelDB.Height - 32)))
                panelDB.Update()
            Else
                If _isResizing = True Then
                    ' Resizing code
                    Dim direction As ResizeDirections = _resizeDirection
                    Select Case direction
                        Case ResizeDirections.None
                            ' Do nothing!
                        Case ResizeDirections.Horizontal
                            Select Case _resizePosition
                                Case ResizePositions.Left
                                    _parentControl.Left = Math.Min(_parentControl.Location.X - _initialCoords.X + e.Location.X, _endPoint.X - _parentControl.MinimumSize.Width)
                                    _parentControl.Width = _endPoint.X - _parentControl.Left
                                Case ResizePositions.Right
                                    _parentControl.Width = _initialSize.Width + (e.Location.X - _initialCoords.X)
                            End Select
                        Case ResizeDirections.Vertical
                            Select Case _resizePosition
                                Case ResizePositions.Top
                                    _parentControl.Top = Math.Min(_parentControl.Location.Y - _initialCoords.Y + e.Location.Y, _endPoint.Y - _parentControl.MinimumSize.Height)
                                    _parentControl.Height = _endPoint.Y - _parentControl.Top
                                Case ResizePositions.Bottom
                                    _parentControl.Height = _initialSize.Height + (e.Location.Y - _initialCoords.Y)
                            End Select
                        Case ResizeDirections.Both
                            Select Case _resizePosition
                                Case ResizePositions.BottomRight
                                    _parentControl.Height = _initialSize.Height + (e.Location.Y - _initialCoords.Y)
                                    _parentControl.Width = _initialSize.Width + (e.Location.X - _initialCoords.X)
                                Case ResizePositions.BottomLeft
                                    _parentControl.Height = _initialSize.Height + (e.Location.Y - _initialCoords.Y)
                                    _parentControl.Left = Math.Min(_parentControl.Location.X - _initialCoords.X + e.Location.X, _endPoint.X - _parentControl.MinimumSize.Width)
                                    _parentControl.Width = _endPoint.X - _parentControl.Left
                                Case ResizePositions.TopRight
                                    _parentControl.Top = Math.Min(_parentControl.Location.Y - _initialCoords.Y + e.Location.Y, _endPoint.Y - _parentControl.MinimumSize.Height)
                                    _parentControl.Height = _endPoint.Y - _parentControl.Top
                                    _parentControl.Width = _initialSize.Width + (e.Location.X - _initialCoords.X)
                                Case ResizePositions.TopLeft
                                    _parentControl.Top = Math.Min(_parentControl.Location.Y - _initialCoords.Y + e.Location.Y, _endPoint.Y - _parentControl.MinimumSize.Height)
                                    _parentControl.Height = _endPoint.Y - _parentControl.Top
                                    _parentControl.Left = Math.Min(_parentControl.Location.X - _initialCoords.X + e.Location.X, _endPoint.X - _parentControl.MinimumSize.Width)
                                    _parentControl.Width = _endPoint.X - _parentControl.Left
                            End Select
                    End Select
                    panelDB.Update()
                Else
                    ' We should just be moving above the control
                    ' get co-ords of parent
                    Dim cPos As Point = New Point(_sourceControl.Location.X + e.Location.X, _sourceControl.Location.Y + e.Location.Y)
                    Select Case cPos.X
                        Case Is <= ControlBorder - BorderOffset ' Left 
                            Select Case cPos.Y
                                Case Is <= ControlBorder - BorderOffset ' Top left
                                    _parentControl.Cursor = Cursors.SizeNWSE
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Both
                                    _resizePosition = ResizePositions.Top Or ResizePositions.Left
                                Case Is > _parentControl.Height - BorderOffset - ControlBorder ' Bottom left
                                    _parentControl.Cursor = Cursors.SizeNESW
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Both
                                    _resizePosition = ResizePositions.Bottom Or ResizePositions.Left
                                Case Else ' Left only
                                    _parentControl.Cursor = Cursors.SizeWE
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Horizontal
                                    _resizePosition = ResizePositions.Left
                            End Select
                        Case Is > _parentControl.Width - BorderOffset - ControlBorder ' right hand 
                            Select Case cPos.Y
                                Case Is <= ControlBorder - BorderOffset ' Top right 
                                    _parentControl.Cursor = Cursors.SizeNESW
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Both
                                    _resizePosition = ResizePositions.Top Or ResizePositions.Right
                                Case Is > _parentControl.Height - BorderOffset - ControlBorder ' Bottom right
                                    _parentControl.Cursor = Cursors.SizeNWSE
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Both
                                    _resizePosition = ResizePositions.Bottom Or ResizePositions.Right
                                Case Else ' Right only
                                    _parentControl.Cursor = Cursors.SizeWE
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Horizontal
                                    _resizePosition = ResizePositions.Right
                            End Select
                        Case Else ' Inbetween
                            Select Case cPos.Y
                                Case Is <= ControlBorder - BorderOffset ' Top only
                                    _parentControl.Cursor = Cursors.SizeNS
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Vertical
                                    _resizePosition = ResizePositions.Top
                                Case Is > _parentControl.Height - BorderOffset - ControlBorder ' Bottom only
                                    _parentControl.Cursor = Cursors.SizeNS
                                    _isResizable = True
                                    _resizeDirection = ResizeDirections.Vertical
                                    _resizePosition = ResizePositions.Bottom
                                Case Else ' In the middle
                                    _parentControl.Cursor = Cursors.Default
                                    _isResizable = False
                                    _resizeDirection = ResizeDirections.None
                                    _resizePosition = ResizePositions.None
                            End Select
                    End Select
                End If
            End If
        End Sub

        Private Sub MyMouseLeave(ByVal sender As Object, ByVal e As EventArgs)
            panelDB.Cursor = Cursors.Default
        End Sub

#End Region

        Private Sub mnuConfigureDB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuConfigureDB.Click
            Dim eveHQSettings As New FrmSettings
            eveHQSettings.Tag = "nodeDashboard"
            eveHQSettings.ShowDialog()
            eveHQSettings.Dispose()
        End Sub

        Private Sub mnuRefreshDB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRefreshDB.Click
            Call UpdateWidgets()
        End Sub

        Private Enum ResizeDirections As Integer
            None = 0
            Horizontal = 1
            Vertical = 2
            Both = 3
        End Enum

        Private Enum ResizePositions As Integer
            None = 0
            Top = 1
            Bottom = 2
            Left = 4
            Right = 8
            TopLeft = 5
            BottomLeft = 6
            TopRight = 9
            BottomRight = 10
        End Enum

        Private Sub mnuClearDashboard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuClearDashboard.Click
            Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to clear the dashboard of all configured widgets?", "Confirm Dashboard Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.Yes Then
                HQ.Settings.DashboardConfiguration.Clear()
                Call UpdateWidgets()
            End If
        End Sub

    End Class

    'Public Class MyWrapper
    '    Dim cControl As New Control

    '    Public Sub New(ByVal control As Control)
    '        Me.Control = control
    '    End Sub

    '    Public Property Control() As Control
    '        Get
    '            Return cControl
    '        End Get
    '        Set(ByVal value As Control)
    '            cControl = value
    '        End Set
    '    End Property

    'End Class
End Namespace