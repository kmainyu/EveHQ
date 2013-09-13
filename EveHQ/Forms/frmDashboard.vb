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

Public Class frmDashboard
    Dim IsDragging As Boolean = False
    Dim IsResizing As Boolean = False
    Dim IsResizable As Boolean = False
    Dim ResizeDirection As ResizeDirections
    Dim ResizePosition As ResizePositions
    Dim controlBorder As Integer = 4
    Dim borderOffset As Integer = 1
    Dim initialCoords As Point
    Dim initialLocation As Point
    Dim initialSize As Size
    Dim endPoint As Point
    Dim sourceControl As Control
    Dim parentControl As Control
    Friend ticker1 As New EveHQ.Core.Ticker

#Region "Form Loading Routines"

    Private Sub frmDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the options
        Me.Ticker1.Visible = EveHQ.Core.HQ.Settings.DBTicker
        Select Case EveHQ.Core.HQ.Settings.DBTickerLocation
            Case "Top"
                Me.ticker1.Dock = DockStyle.Top
            Case "Bottom"
                Me.ticker1.Dock = DockStyle.Bottom
        End Select

        ' Add the controls to the panel
        Call Me.UpdateWidgets()

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
        For Each config As SortedList(Of String, Object) In EveHQ.Core.HQ.Settings.DashboardConfiguration
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
        Dim WidgetName As String = CStr(config("ControlName"))
        If EveHQ.Core.HQ.Widgets.ContainsKey(WidgetName) = True Then
            Dim ClassName As String = EveHQ.Core.HQ.Widgets(WidgetName)
            Dim myType As Type = Assembly.GetExecutingAssembly.GetType(ClassName)
            Dim newWidget As Object = Activator.CreateInstance(myType)
            Dim pi As System.Reflection.PropertyInfo = myType.GetProperty("ControlConfiguration")
            pi.SetValue(newWidget, config, Nothing)
            Dim control As Control = CType(newWidget, Windows.Forms.Control)
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
        sourceControl = CType(sender, Control)
        parentControl = sourceControl
        Do
            parentControl = parentControl.Parent
        Loop Until parentControl.Parent Is Me.panelDB
        parentControl.BringToFront()
        ' Establish cursor position to determine routine
        If IsResizable = True Then
            IsResizing = True
            initialCoords = e.Location
            initialLocation = parentControl.Location
            initialSize = parentControl.Size
            endPoint = New Point(initialLocation.X + initialSize.Width, initialLocation.Y + initialSize.Height)
            'lblStatus.Text = "Status: Resizing " & parentControl.Name & " (" & ResizePosition.ToString & ") from (" & initialSize.Width.ToString & "," & initialSize.Height.ToString & ") to (" & parentControl.Width.ToString & "," & parentControl.Height.ToString & ")"
        Else
            ' Disable dragging from the AGPContent panel, only allow from the Header
            If sourceControl.Name = "lblHeader" Then
                IsDragging = True
                parentControl.Cursor = Cursors.SizeAll
                initialCoords = e.Location
                initialLocation = parentControl.Location
                initialSize = parentControl.Size
                endPoint = New Point(initialLocation.X + initialSize.Width, initialLocation.Y + initialSize.Height)
                'lblStatus.Text = "Status: Dragging " & parentControl.Name & " from (" & initialLocation.X.ToString & "," & initialLocation.Y.ToString & ") to (" & parentControl.Location.X.ToString & "," & parentControl.Location.Y.ToString & ")"
            End If
        End If
    End Sub

    Private Sub MyMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        ' Get the source control
        sourceControl = CType(sender, Control)
        parentControl = sourceControl
        Do
            parentControl = parentControl.Parent
        Loop Until parentControl.Parent Is Me.panelDB
        If IsDragging = True Then
            ' Dragging code
            Dim pi As System.Reflection.PropertyInfo = parentControl.GetType().GetProperty("ControlLocation")
            pi.SetValue(parentControl, parentControl.Location, Nothing)
            IsDragging = False
        ElseIf IsResizing = True Then
            ' Resizing Code
            Dim pi As System.Reflection.PropertyInfo = parentControl.GetType().GetProperty("ControlSize")
            pi.SetValue(parentControl, parentControl.Size, Nothing)
            IsResizing = False
        End If
        'lblStatus.Text = "Status: Waiting..."
        parentControl.Cursor = Cursors.Default
    End Sub

    Private Sub MyMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        ' Get the source control
        sourceControl = CType(sender, Control)
        parentControl = sourceControl
        Do
            parentControl = parentControl.Parent
        Loop Until parentControl.Parent Is Me.panelDB
        If IsDragging = True Then
            ' Dragging code
            Dim cx As Integer = parentControl.Location.X - initialCoords.X + e.Location.X
            Dim cy As Integer = parentControl.Location.Y - initialCoords.Y + e.Location.Y
            parentControl.Location = New Point(Math.Max(0, Math.Min(cx, panelDB.Width - 32)), Math.Max(0, Math.Min(cy, panelDB.Height - 32)))
            panelDB.Update()
        Else
            If IsResizing = True Then
                ' Resizing code
                Dim Direction As ResizeDirections = ResizeDirection
                Select Case Direction
                    Case ResizeDirections.None
                        ' Do nothing!
                    Case ResizeDirections.Horizontal
                        Select Case ResizePosition
                            Case ResizePositions.Left
                                parentControl.Left = Math.Min(parentControl.Location.X - initialCoords.X + e.Location.X, endPoint.X - parentControl.MinimumSize.Width)
                                parentControl.Width = endPoint.X - parentControl.Left
                            Case ResizePositions.Right
                                parentControl.Width = initialSize.Width + (e.Location.X - initialCoords.X)
                        End Select
                    Case ResizeDirections.Vertical
                        Select Case ResizePosition
                            Case ResizePositions.Top
                                parentControl.Top = Math.Min(parentControl.Location.Y - initialCoords.Y + e.Location.Y, endPoint.Y - parentControl.MinimumSize.Height)
                                parentControl.Height = endPoint.Y - parentControl.Top
                            Case ResizePositions.Bottom
                                parentControl.Height = initialSize.Height + (e.Location.Y - initialCoords.Y)
                        End Select
                    Case ResizeDirections.Both
                        Select Case ResizePosition
                            Case ResizePositions.BottomRight
                                parentControl.Height = initialSize.Height + (e.Location.Y - initialCoords.Y)
                                parentControl.Width = initialSize.Width + (e.Location.X - initialCoords.X)
                            Case ResizePositions.BottomLeft
                                parentControl.Height = initialSize.Height + (e.Location.Y - initialCoords.Y)
                                parentControl.Left = Math.Min(parentControl.Location.X - initialCoords.X + e.Location.X, endPoint.X - parentControl.MinimumSize.Width)
                                parentControl.Width = endPoint.X - parentControl.Left
                            Case ResizePositions.TopRight
                                parentControl.Top = Math.Min(parentControl.Location.Y - initialCoords.Y + e.Location.Y, endPoint.Y - parentControl.MinimumSize.Height)
                                parentControl.Height = endPoint.Y - parentControl.Top
                                parentControl.Width = initialSize.Width + (e.Location.X - initialCoords.X)
                            Case ResizePositions.TopLeft
                                parentControl.Top = Math.Min(parentControl.Location.Y - initialCoords.Y + e.Location.Y, endPoint.Y - parentControl.MinimumSize.Height)
                                parentControl.Height = endPoint.Y - parentControl.Top
                                parentControl.Left = Math.Min(parentControl.Location.X - initialCoords.X + e.Location.X, endPoint.X - parentControl.MinimumSize.Width)
                                parentControl.Width = endPoint.X - parentControl.Left
                        End Select
                End Select
                panelDB.Update()
            Else
                ' We should just be moving above the control
                ' get co-ords of parent
                Dim cPos As Point = New Point(sourceControl.Location.X + e.Location.X, sourceControl.Location.Y + e.Location.Y)
                Select Case cPos.X
                    Case Is <= controlBorder - borderOffset ' Left 
                        Select Case cPos.Y
                            Case Is <= controlBorder - borderOffset ' Top left
                                parentControl.Cursor = Cursors.SizeNWSE
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Both
                                ResizePosition = ResizePositions.Top Or ResizePositions.Left
                            Case Is > parentControl.Height - borderOffset - controlBorder ' Bottom left
                                parentControl.Cursor = Cursors.SizeNESW
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Both
                                ResizePosition = ResizePositions.Bottom Or ResizePositions.Left
                            Case Else ' Left only
                                parentControl.Cursor = Cursors.SizeWE
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Horizontal
                                ResizePosition = ResizePositions.Left
                        End Select
                    Case Is > parentControl.Width - borderOffset - controlBorder ' right hand 
                        Select Case cPos.Y
                            Case Is <= controlBorder - borderOffset ' Top right 
                                parentControl.Cursor = Cursors.SizeNESW
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Both
                                ResizePosition = ResizePositions.Top Or ResizePositions.Right
                            Case Is > parentControl.Height - borderOffset - controlBorder ' Bottom right
                                parentControl.Cursor = Cursors.SizeNWSE
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Both
                                ResizePosition = ResizePositions.Bottom Or ResizePositions.Right
                            Case Else ' Right only
                                parentControl.Cursor = Cursors.SizeWE
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Horizontal
                                ResizePosition = ResizePositions.Right
                        End Select
                    Case Else ' Inbetween
                        Select Case cPos.Y
                            Case Is <= controlBorder - borderOffset ' Top only
                                parentControl.Cursor = Cursors.SizeNS
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Vertical
                                ResizePosition = ResizePositions.Top
                            Case Is > parentControl.Height - borderOffset - controlBorder ' Bottom only
                                parentControl.Cursor = Cursors.SizeNS
                                IsResizable = True
                                ResizeDirection = ResizeDirections.Vertical
                                ResizePosition = ResizePositions.Bottom
                            Case Else ' In the middle
                                parentControl.Cursor = Cursors.Default
                                IsResizable = False
                                ResizeDirection = ResizeDirections.None
                                ResizePosition = ResizePositions.None
                        End Select
                End Select
            End If
        End If
    End Sub

    Private Sub MyMouseLeave(ByVal sender As Object, ByVal e As EventArgs)
        panelDB.Cursor = Cursors.Default
    End Sub

#End Region

    Private Sub mnuConfigureDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuConfigureDB.Click
        Dim EveHQSettings As New frmSettings
        EveHQSettings.Tag = "nodeDashboard"
        EveHQSettings.ShowDialog()
        EveHQSettings.Dispose()
    End Sub

    Private Sub mnuRefreshDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRefreshDB.Click
        Call Me.UpdateWidgets()
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

    Private Sub mnuClearDashboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClearDashboard.Click
        Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to clear the dashboard of all configured widgets?", "Confirm Dashboard Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            EveHQ.Core.HQ.Settings.DashboardConfiguration.Clear()
            Call Me.UpdateWidgets()
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