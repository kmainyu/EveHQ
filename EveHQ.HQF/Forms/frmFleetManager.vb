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
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports EveHQ.HQF.Controls

Namespace Forms

    Public Class FrmFleetManager
        Dim _isDragging As Boolean = False
        Dim _initialCoords As Point
        Dim _sourceControl As Control
        Dim _parentControl As Control
        ReadOnly _linkPen As New Pen(Color.FromArgb(16, 0, 0, 0), 2)
        ReadOnly _activeLinkPen As New Pen(Color.FromArgb(255, 0, 0, 0), 2)

#Region "Form Loading Routines"

        Private Sub frmFleetManager_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            DoubleBuffered = True

            ' Add the controls to the panel
            SuspendLayout()

            ' ReSharper disable once RedundantAssignment - Incorrectly warned by R#
            For a As Integer = 1 To 10
                Call AddShipWidget()
            Next

            ResumeLayout()

        End Sub
#End Region

#Region "Widget Update Routines"

        Private Sub AddShipWidget()

            Dim shipFit As Fitting = Fittings.FittingList("Guardian, Test").Clone
            Dim newSw As New ShipWidget(shipFit)
            newSw.Name = Now.Ticks.ToString

            AddHandler newSw.Controls("pnlHeader").MouseDown, AddressOf MyMouseDown
            AddHandler newSw.Controls("pnlHeader").MouseUp, AddressOf MyMouseUp
            AddHandler newSw.Controls("pnlHeader").MouseMove, AddressOf MyMouseMove
            AddHandler newSw.Controls("pnlHeader").MouseLeave, AddressOf MyMouseLeave
            Controls.Add(newSw)
        End Sub

#End Region

#Region "Panel Drag/Drop Routines"

        Private Sub MyMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
            ' Get the source control
            _sourceControl = CType(sender, Control)
            _parentControl = _sourceControl
            Do
                _parentControl = _parentControl.Parent
            Loop Until _parentControl.Parent Is Me
            _parentControl.BringToFront()
            ' Disable dragging from the AGPContent panel, only allow from the Header
            If _sourceControl.Name = "pnlHeader" Then
                _isDragging = True
                _parentControl.Cursor = Cursors.SizeAll
                _initialCoords = e.Location
            End If
        End Sub

        Private Sub MyMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
            ' Get the source control
            _sourceControl = CType(sender, Control)
            _parentControl = _sourceControl
            Do
                _parentControl = _parentControl.Parent
            Loop Until _parentControl.Parent Is Me
            If _isDragging = True Then
                ' Dragging code
                _isDragging = False
                Invalidate()
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
            Loop Until _parentControl.Parent Is Me
            If _isDragging = True Then
                ' Dragging code
                Dim cx As Integer = _parentControl.Location.X - _initialCoords.X + e.Location.X
                Dim cy As Integer = _parentControl.Location.Y - _initialCoords.Y + e.Location.Y
                _parentControl.Location = New Point(Math.Max(0, Math.Min(cx, Width - 32)), Math.Max(0, Math.Min(cy, Height - 32)))
                Invalidate()
            End If
        End Sub

        Private Sub MyMouseLeave(ByVal sender As Object, ByVal e As EventArgs)
            Cursor = Cursors.Default
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            'MyBase.OnPaint(e)

            Call PaintShipLinks(e.Graphics)

        End Sub

        Private Sub PaintShipLinks(ByVal e As Graphics)

            If _isDragging = True Then
                e.SmoothingMode = SmoothingMode.HighSpeed
            Else
                e.SmoothingMode = SmoothingMode.HighQuality
            End If

            ' Draw from the right hand edge to the left
            For Each c As Control In Controls
                For Each c2 As Control In Controls
                    If c.Name <> c2.Name Then
                        Dim sx As Integer = c.Location.X + c.Width
                        Dim sy As Integer = c.Location.Y + 81
                        Dim fx As Integer = c2.Location.X
                        Dim fy As Integer = c2.Location.Y + 10

                        If _parentControl IsNot Nothing Then
                            If c.Name = _parentControl.Name Then
                                e.DrawLine(_activeLinkPen, sx, sy, fx, fy)
                            Else
                                e.DrawLine(_linkPen, sx, sy, fx, fy)
                            End If
                        Else
                            e.DrawLine(_linkPen, sx, sy, fx, fy)
                        End If

                    End If
                Next
            Next

            'For Each c As Control In Me.Controls

            'Next

            Update()

        End Sub

#End Region

    End Class
End NameSpace