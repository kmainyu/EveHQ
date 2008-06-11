' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports System
Imports System.Text
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles
Imports System.Drawing
Imports System.ComponentModel

Public Class SplitButton
    Inherits Button

    ' Fields
    Private _state As PushButtonState
    Private Shared BorderSize As Integer = (SystemInformation.Border3DSize.Width * 2)
    Private dropDownRectangle As Rectangle = New Rectangle
    Private m_SplitMenu As ContextMenuStrip
    Private Const PushButtonWidth As Integer = 14
    Private m_showSplit As Boolean
    Private skipNextOpen As Boolean

    ' Methods
    Public Sub New()
        Me.AutoSize = True
    End Sub

    Public Overrides Function GetPreferredSize(ByVal proposedSize As Size) As Size
        Dim preferredSize As Size = MyBase.GetPreferredSize(proposedSize)
        If ((Me.m_showSplit AndAlso Not String.IsNullOrEmpty(Me.Text)) AndAlso ((TextRenderer.MeasureText(Me.Text, Me.Font).Width + 14) > preferredSize.Width)) Then
            Return (preferredSize + New Size((14 + (SplitButton.BorderSize * 2)), 0))
        End If
        Return preferredSize
    End Function

    Protected Overrides Function IsInputKey(ByVal keyData As Keys) As Boolean
        Return ((keyData.Equals(Keys.Down) AndAlso Me.m_showSplit) OrElse MyBase.IsInputKey(keyData))
    End Function

    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If MyBase.Enabled Then
            Me.State = PushButtonState.Normal
        Else
            Me.State = PushButtonState.Disabled
        End If
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        If Not Me.m_showSplit Then
            MyBase.OnGotFocus(e)
        ElseIf (Not Me.State.Equals(PushButtonState.Pressed) AndAlso Not Me.State.Equals(PushButtonState.Disabled)) Then
            Me.State = PushButtonState.Default
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal kevent As KeyEventArgs)
        If Me.m_showSplit Then
            If kevent.KeyCode.Equals(Keys.Down) Then
                Me.ShowContextMenuStrip()
            ElseIf (kevent.KeyCode.Equals(Keys.Space) AndAlso (kevent.Modifiers = Keys.None)) Then
                Me.State = PushButtonState.Pressed
            End If
        End If
        MyBase.OnKeyDown(kevent)
    End Sub

    Protected Overrides Sub OnKeyUp(ByVal kevent As KeyEventArgs)
        If kevent.KeyCode.Equals(Keys.Space) Then
            If (Control.MouseButtons = Windows.Forms.MouseButtons.None) Then
                Me.State = PushButtonState.Normal
            End If
        ElseIf (kevent.KeyCode.Equals(Keys.Apps) AndAlso (Control.MouseButtons = Windows.Forms.MouseButtons.None)) Then
            Me.ShowContextMenuStrip()
        End If
        MyBase.OnKeyUp(kevent)
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As EventArgs)
        If Not Me.m_showSplit Then
            MyBase.OnLostFocus(e)
        ElseIf (Not Me.State.Equals(PushButtonState.Pressed) AndAlso Not Me.State.Equals(PushButtonState.Disabled)) Then
            Me.State = PushButtonState.Normal
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Not Me.m_showSplit Then
            MyBase.OnMouseDown(e)
        ElseIf ((Me.dropDownRectangle.Contains(e.Location) AndAlso Not Me.m_SplitMenu.Visible) AndAlso (e.Button = Windows.Forms.MouseButtons.Left)) Then
            Me.ShowContextMenuStrip()
        Else
            Me.State = PushButtonState.Pressed
        End If
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        If Not Me.m_showSplit Then
            MyBase.OnMouseEnter(e)
        ElseIf (Not Me.State.Equals(PushButtonState.Pressed) AndAlso Not Me.State.Equals(PushButtonState.Disabled)) Then
            Me.State = PushButtonState.Hot
        End If
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        If Not Me.m_showSplit Then
            MyBase.OnMouseLeave(e)
        ElseIf (Not Me.State.Equals(PushButtonState.Pressed) AndAlso Not Me.State.Equals(PushButtonState.Disabled)) Then
            If Me.Focused Then
                Me.State = PushButtonState.Default
            Else
                Me.State = PushButtonState.Normal
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal mevent As MouseEventArgs)
        If Not Me.m_showSplit Then
            MyBase.OnMouseUp(mevent)
        ElseIf (mevent.Button = Windows.Forms.MouseButtons.Right) Then
            Me.ShowContextMenuStrip()
        ElseIf ((Me.m_SplitMenu Is Nothing) OrElse Not Me.m_SplitMenu.Visible) Then
            Me.SetButtonDrawState()
            If (MyBase.Bounds.Contains(MyBase.Parent.PointToClient(Windows.Forms.Cursor.Position)) AndAlso Not Me.dropDownRectangle.Contains(mevent.Location)) Then
                Me.OnClick(New EventArgs)
            End If
        End If
    End Sub

    Protected Overrides Sub OnPaint(ByVal pevent As PaintEventArgs)
        MyBase.OnPaint(pevent)
        If Me.m_showSplit Then
            Dim g As Graphics = pevent.Graphics
            Dim clientRectangle As Rectangle = MyBase.ClientRectangle
            If (((Me.State <> PushButtonState.Pressed) AndAlso MyBase.IsDefault) AndAlso Not Application.RenderWithVisualStyles) Then
                Dim rectangle2 As Rectangle = clientRectangle
                rectangle2.Inflate(-1, -1)
                ButtonRenderer.DrawButton(g, rectangle2, Me.State)
                g.DrawRectangle(SystemPens.WindowFrame, 0, 0, (clientRectangle.Width - 1), (clientRectangle.Height - 1))
            Else
                ButtonRenderer.DrawButton(g, clientRectangle, Me.State)
            End If
            Me.dropDownRectangle = New Rectangle((clientRectangle.Right - 14), 0, 14, clientRectangle.Height)
            Dim borderSize As Integer = SplitButton.BorderSize
            Dim bounds As New Rectangle((borderSize - 1), (borderSize - 1), ((clientRectangle.Width - Me.dropDownRectangle.Width) - borderSize), ((clientRectangle.Height - (borderSize * 2)) + 2))
            Dim flag As Boolean = (((Me.State = PushButtonState.Hot) OrElse (Me.State = PushButtonState.Pressed)) OrElse Not Application.RenderWithVisualStyles)
            If (Me.RightToLeft = Windows.Forms.RightToLeft.Yes) Then
                Me.dropDownRectangle.X = (clientRectangle.Left + 1)
                bounds.X = Me.dropDownRectangle.Right
                If flag Then
                    g.DrawLine(SystemPens.ButtonShadow, (clientRectangle.Left + 14), SplitButton.BorderSize, (clientRectangle.Left + 14), (clientRectangle.Bottom - SplitButton.BorderSize))
                    g.DrawLine(SystemPens.ButtonFace, ((clientRectangle.Left + 14) + 1), SplitButton.BorderSize, ((clientRectangle.Left + 14) + 1), (clientRectangle.Bottom - SplitButton.BorderSize))
                End If
            ElseIf flag Then
                g.DrawLine(SystemPens.ButtonShadow, (clientRectangle.Right - 14), SplitButton.BorderSize, (clientRectangle.Right - 14), (clientRectangle.Bottom - SplitButton.BorderSize))
                g.DrawLine(SystemPens.ButtonFace, ((clientRectangle.Right - 14) - 1), SplitButton.BorderSize, ((clientRectangle.Right - 14) - 1), (clientRectangle.Bottom - SplitButton.BorderSize))
            End If
            Me.PaintArrow(g, Me.dropDownRectangle)
            Dim flags As TextFormatFlags = (TextFormatFlags.VerticalCenter Or TextFormatFlags.HorizontalCenter)
            If Not MyBase.UseMnemonic Then
                flags = (flags Or TextFormatFlags.NoPrefix)
            ElseIf Not Me.ShowKeyboardCues Then
                flags = (flags Or TextFormatFlags.HidePrefix)
            End If
            If Not String.IsNullOrEmpty(Me.Text) Then
                If MyBase.Enabled Then
                    TextRenderer.DrawText(g, Me.Text, Me.Font, bounds, SystemColors.ControlText, flags)
                Else
                    TextRenderer.DrawText(g, Me.Text, Me.Font, bounds, SystemColors.GrayText, flags)
                End If
            End If
            If (((Me.State <> PushButtonState.Pressed) AndAlso Me.Focused) AndAlso Me.ShowFocusCues) Then
                ControlPaint.DrawFocusRectangle(g, bounds)
            End If
        End If
    End Sub

    Private Sub PaintArrow(ByVal g As Graphics, ByVal dropDownRect As Rectangle)
        Dim point As New Point(Convert.ToInt32(CInt((dropDownRect.Left + (dropDownRect.Width / 2)))), Convert.ToInt32(CInt((dropDownRect.Top + (dropDownRect.Height / 2)))))
        point.X = (point.X + (dropDownRect.Width Mod 2))
        Dim points As Point() = New Point() {New Point((point.X - 2), (point.Y - 1)), New Point((point.X + 3), (point.Y - 1)), New Point(point.X, (point.Y + 2))}
        If MyBase.Enabled Then
            g.FillPolygon(SystemBrushes.ControlText, points)
        Else
            g.FillPolygon(SystemBrushes.ButtonShadow, points)
        End If
    End Sub

    Private Sub SetButtonDrawState()
        If MyBase.Bounds.Contains(MyBase.Parent.PointToClient(Windows.Forms.Cursor.Position)) Then
            Me.State = PushButtonState.Hot
        ElseIf Me.Focused Then
            Me.State = PushButtonState.Default
        ElseIf Not MyBase.Enabled Then
            Me.State = PushButtonState.Disabled
        Else
            Me.State = PushButtonState.Normal
        End If
    End Sub

    Private Sub ShowContextMenuStrip()
        If Me.skipNextOpen Then
            Me.skipNextOpen = False
        Else
            Me.State = PushButtonState.Pressed
            If (Not Me.m_SplitMenu Is Nothing) Then
                Me.m_SplitMenu.Show(Me, New Point(0, MyBase.Height), ToolStripDropDownDirection.BelowRight)
            End If
        End If
    End Sub

    Private Sub SplitMenu_Closing(ByVal sender As Object, ByVal e As ToolStripDropDownClosingEventArgs)
        Try
            Me.SetButtonDrawState()
        Catch exception1 As Exception
        End Try
        If (e.CloseReason = ToolStripDropDownCloseReason.AppClicked) Then
            Me.skipNextOpen = (Me.dropDownRectangle.Contains(MyBase.PointToClient(Windows.Forms.Cursor.Position)) AndAlso (Control.MouseButtons = Windows.Forms.MouseButtons.Left))
        End If
    End Sub

    Private Sub SplitMenu_Opening(ByVal sender As Object, ByVal e As CancelEventArgs)
        If Me.m_SplitMenu.Visible Then
            e.Cancel = True
        End If
    End Sub


    ' Properties
    <Browsable(False)> _
    Public Overrides Property ContextMenuStrip() As ContextMenuStrip
        Get
            Return Me.m_SplitMenu
        End Get
        Set(ByVal value As ContextMenuStrip)
            Me.m_SplitMenu = value
        End Set
    End Property

    <DefaultValue(False)> _
    Public WriteOnly Property ShowSplit() As Boolean
        Set(ByVal value As Boolean)
            If (value <> Me.m_showSplit) Then
                Me.m_showSplit = value
                MyBase.Invalidate()
                If (Not MyBase.Parent Is Nothing) Then
                    MyBase.Parent.PerformLayout()
                End If
            End If
        End Set
    End Property

    <DefaultValue(CStr(Nothing))> _
    Public Property SplitMenu() As ContextMenuStrip
        Get
            Return Me.m_SplitMenu
        End Get
        Set(ByVal value As ContextMenuStrip)
            Me.m_SplitMenu = value
            If (Not Me.m_SplitMenu Is Nothing) Then
                RemoveHandler Me.m_SplitMenu.Closing, New ToolStripDropDownClosingEventHandler(AddressOf Me.SplitMenu_Closing)
                AddHandler Me.m_SplitMenu.Closing, New ToolStripDropDownClosingEventHandler(AddressOf Me.SplitMenu_Closing)
                RemoveHandler Me.m_SplitMenu.Opening, New CancelEventHandler(AddressOf Me.SplitMenu_Opening)
                AddHandler Me.m_SplitMenu.Opening, New CancelEventHandler(AddressOf Me.SplitMenu_Opening)
                Me.m_showSplit = True
            Else
                Me.m_showSplit = False
            End If
        End Set
    End Property

    Private Property State() As PushButtonState
        Get
            Return Me._state
        End Get
        Set(ByVal value As PushButtonState)
            If Not Me._state.Equals(value) Then
                Me._state = value
                MyBase.Invalidate()
            End If
        End Set
    End Property
End Class
