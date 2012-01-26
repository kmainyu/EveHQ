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
Imports System.Windows.Forms
Imports System.Drawing

Public Class FleetDashboard
    Dim IsDragging As Boolean = False
    Dim initialCoords As Point
    Dim sourceControl As Control
    Dim parentControl As Control
    Dim LinkPen As New Pen(Color.FromArgb(16, Color.Black), 2)
    Dim RLinkPen As New Pen(Color.FromArgb(64, Color.Red), 2)
    Dim GLinkPen As New Pen(Color.FromArgb(255, Color.LightGreen), 2)

#Region "Form Loading Routines"

    Private Sub FleetDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.DoubleBuffered = True

        ' Add the controls to the panel
        Me.SuspendLayout()

        For a As Integer = 1 To 10
            Call Me.AddShipWidget()
        Next

        Me.ResumeLayout()

    End Sub
#End Region

#Region "Widget Update Routines"

    Private Sub AddShipWidget()

        If Fittings.FittingList.ContainsKey("Guardian, Test") Then
            Dim ShipFit As Fitting = Fittings.FittingList("Guardian, Test").Clone
            Dim NewSW As New ShipWidget(ShipFit)
            NewSW.Name = Now.Ticks.ToString

            AddHandler NewSW.Controls("pnlHeader").MouseDown, AddressOf MyMouseDown
            AddHandler NewSW.Controls("pnlHeader").MouseUp, AddressOf MyMouseUp
            AddHandler NewSW.Controls("pnlHeader").MouseMove, AddressOf MyMouseMove
            AddHandler NewSW.Controls("pnlHeader").MouseLeave, AddressOf MyMouseLeave
            Me.Controls.Add(NewSW)
        End If
    End Sub

#End Region

#Region "Panel Drag/Drop Routines"

    Private Sub MyMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        ' Get the source control
        sourceControl = CType(sender, Control)
        parentControl = sourceControl
        Do
            parentControl = parentControl.Parent
        Loop Until parentControl.Parent Is Me
        parentControl.BringToFront()
        ' Disable dragging from the AGPContent panel, only allow from the Header
        If sourceControl.Name = "pnlHeader" Then
            IsDragging = True
            parentControl.Cursor = Cursors.SizeAll
            initialCoords = e.Location
        End If
    End Sub

    Private Sub MyMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        ' Get the source control
        sourceControl = CType(sender, Control)
        parentControl = sourceControl
        Do
            parentControl = parentControl.Parent
        Loop Until parentControl.Parent Is Me
        If IsDragging = True Then
            ' Dragging code
            IsDragging = False
            Me.Invalidate()
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
        Loop Until parentControl.Parent Is Me
        If IsDragging = True Then
            ' Dragging code
            Dim cx As Integer = parentControl.Location.X - initialCoords.X + e.Location.X
            Dim cy As Integer = parentControl.Location.Y - initialCoords.Y + e.Location.Y
            parentControl.Location = New Point(Math.Max(0, Math.Min(cx, Me.Width - 32)), Math.Max(0, Math.Min(cy, Me.Height - 32)))
            Me.Invalidate()
        End If
    End Sub

    Private Sub MyMouseLeave(ByVal sender As Object, ByVal e As EventArgs)
        Me.Cursor = Cursors.Default
    End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        'MyBase.OnPaint(e)

        Call PaintShipLinks(e.Graphics)

    End Sub

    Private Sub PaintShipLinks(ByVal e As Graphics)

        If IsDragging = True Then
            e.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        Else
            e.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        End If

        ' Draw from the right hand edge to the left
        For Each c As Control In Me.Controls
            For Each c2 As Control In Me.Controls
                If c.Name <> c2.Name Then
                    Dim SX As Integer = c.Location.X + c.Width
                    Dim SY As Integer = c.Location.Y + 81
                    Dim FX As Integer = c2.Location.X
                    Dim FY As Integer = c2.Location.Y + 10

                    If parentControl IsNot Nothing Then
                        If c.Name = parentControl.Name Then
                            e.DrawLine(GLinkPen, SX, SY, FX, FY)
                        ElseIf c2.Name = parentControl.Name Then
                            e.DrawLine(RLinkPen, SX, SY, FX, FY)
                        Else
                            e.DrawLine(LinkPen, SX, SY, FX, FY)
                        End If
                    Else
                        e.DrawLine(LinkPen, SX, SY, FX, FY)
                    End If

                End If
            Next
        Next

        'For Each c As Control In Me.Controls

        'Next

        Me.Update()

    End Sub

#End Region
End Class
