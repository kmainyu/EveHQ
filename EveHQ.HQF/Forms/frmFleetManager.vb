' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmFleetManager
    Dim IsDragging As Boolean = False
    Dim controlBorder As Integer = 4
    Dim borderOffset As Integer = 1
    Dim initialCoords As Point
    Dim initialLocation As Point
    Dim initialSize As Size
    Dim endPoint As Point
    Dim sourceControl As Control
    Dim parentControl As Control

#Region "Form Loading Routines"

    Private Sub frmFleetManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Add the controls to the panel
        Call Me.UpdateWidgets()

        panelDB.SuspendLayout()

        For a As Integer = 1 To 10
            Call Me.AddShipWidget()
        Next

        panelDB.ResumeLayout()

        Me.DoubleBuffered = True

    End Sub
#End Region

#Region "Widget Update Routines"

    Public Sub UpdateWidgets()

        panelDB.SuspendLayout()
        ' Remove event handlers
        'For c As Integer = panelDB.Controls.Count - 1 To 0 Step -1
        '    Dim control As Control = panelDB.Controls(c)
        '    RemoveHandler control.Controls("AGPContent").MouseDown, AddressOf MyMouseDown
        '    RemoveHandler control.Controls("AGPContent").MouseUp, AddressOf MyMouseUp
        '    RemoveHandler control.Controls("AGPContent").MouseMove, AddressOf MyMouseMove
        '    RemoveHandler control.Controls("AGPContent").MouseLeave, AddressOf MyMouseLeave
        '    'RemoveHandler control.Resize, AddressOf MyControlResize
        '    'RemoveHandler control.Move, AddressOf MyControlMove
        '    Try
        '        RemoveHandler control.Controls("AGPContent").Controls("lblHeader").MouseDown, AddressOf MyMouseDown
        '        RemoveHandler control.Controls("AGPContent").Controls("lblHeader").MouseUp, AddressOf MyMouseUp
        '        RemoveHandler control.Controls("AGPContent").Controls("lblHeader").MouseMove, AddressOf MyMouseMove
        '    Catch e As Exception
        '    End Try
        '    control.Dispose()
        'Next

        '' Clear Controls
        'panelDB.Controls.Clear()
        'For Each config As SortedList(Of String, Object) In EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration
        '    Call AddWidget(config)
        'Next

        If panelDB.Controls.Count = 0 Then
            panelDB.Text = "Right-click this panel and configure the dashboard to add or edit widgets."
        Else
            panelDB.Text = ""
        End If

        panelDB.ResumeLayout()

    End Sub

    Private Sub AddShipWidget()

        Dim ShipFit As Fitting = Fittings.FittingList("Guardian, Test").Clone
        Dim NewSW As New ShipWidget(ShipFit)

        AddHandler NewSW.Controls("pnlHeader").MouseDown, AddressOf MyMouseDown
        AddHandler NewSW.Controls("pnlHeader").MouseUp, AddressOf MyMouseUp
        AddHandler NewSW.Controls("pnlHeader").MouseMove, AddressOf MyMouseMove
        AddHandler NewSW.Controls("pnlHeader").MouseLeave, AddressOf MyMouseLeave
        panelDB.Controls.Add(NewSW)
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
        ' Disable dragging from the AGPContent panel, only allow from the Header
            If sourceControl.Name = "pnlHeader" Then
                IsDragging = True
                parentControl.Cursor = Cursors.SizeAll
                initialCoords = e.Location
                initialLocation = parentControl.Location
                initialSize = parentControl.Size
                endPoint = New Point(initialLocation.X + initialSize.Width, initialLocation.Y + initialSize.Height)
                'lblStatus.Text = "Status: Dragging " & parentControl.Name & " from (" & initialLocation.X.ToString & "," & initialLocation.Y.ToString & ") to (" & parentControl.Location.X.ToString & "," & parentControl.Location.Y.ToString & ")"
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
            'Dim pi As System.Reflection.PropertyInfo = parentControl.GetType().GetProperty("ControlLocation")
            'pi.SetValue(parentControl, parentControl.Location, Nothing)
            IsDragging = False
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
        End If
    End Sub

    Private Sub MyMouseLeave(ByVal sender As Object, ByVal e As EventArgs)
        panelDB.Cursor = Cursors.Default
    End Sub

#End Region

    Private Sub mnuRefreshDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRefreshDB.Click
        Call Me.UpdateWidgets()
    End Sub

    Private Sub mnuClearDashboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClearDashboard.Click
        Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to clear the dashboard of all configured widgets?", "Confirm Dashboard Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Clear()
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