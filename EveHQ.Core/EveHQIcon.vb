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

Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Timer = System.Threading.Timer

Public Class EveHQIcon
    Inherits Component

    Private _iconText As String
    Private _mouseHoverTime As Integer
    ' ReSharper disable once NotAccessedField.Local
    Private _mouseState As MouseState
    Private _notifyIcon As NotifyIcon
    
#Region "Constructors"

#End Region

#Region "Methods"
    Public Sub ShowBalloonTip(ByVal timeout As Integer)
        Call _notifyIcon.ShowBalloonTip(timeout)
    End Sub
    Public Sub ShowBalloonTip(ByVal timeout As Integer, ByVal tipTitle As String, ByVal tipText As String, ByVal tipIcon As ToolTipIcon)
        Call _notifyIcon.ShowBalloonTip(timeout, tipTitle, tipText, tipIcon)
    End Sub
#End Region

#Region "Properties"
    <Category("Appearance"), Description("The title of balloon popup")> _
    Public Property BalloonTipIcon() As ToolTipIcon
        Get
            Return _notifyIcon.BalloonTipIcon
        End Get
        Set(ByVal value As ToolTipIcon)
            _notifyIcon.BalloonTipIcon = value
        End Set
    End Property
    <Category("Appearance"), Description("The title of balloon popup")> _
    Public Property BalloonTipText() As String
        Get
            Return _notifyIcon.BalloonTipText
        End Get
        Set(ByVal value As String)
            _notifyIcon.BalloonTipText = value
        End Set
    End Property
    <Category("Appearance"), Description("The title of balloon popup")> _
    Public Property BalloonTipTitle() As String
        Get
            Return _notifyIcon.BalloonTipTitle
        End Get
        Set(ByVal value As String)
            _notifyIcon.BalloonTipTitle = value
        End Set
    End Property
    <Category("Behaviour")> _
    Public Property ContextMenuStrip() As ContextMenuStrip
        Get
            Return _notifyIcon.ContextMenuStrip
        End Get
        Set(ByVal value As ContextMenuStrip)
            _notifyIcon.ContextMenuStrip = value
        End Set
    End Property
    <Description("The icon to display in the system tray"), Category("Appearance")> _
    Public Property Icon() As Icon
        Get
            Return _notifyIcon.Icon
        End Get
        Set(ByVal value As Icon)
            _notifyIcon.Icon = value
        End Set
    End Property
    <Category("Behaviour"), DefaultValue(250), Description("The length of time, in milliseconds, for which the mouse must remain stationary over the control before the MouseHover event is raised")> _
    Public Property MouseHoverTime() As Integer
        Get
            Return _mouseHoverTime
        End Get
        Set(ByVal value As Integer)
            _mouseHoverTime = value
        End Set
    End Property
    <Category("Appearance"), Description("The text that will be displayed when the mouse hovers over the icon")> _
    Public Property [Text]() As String
        Get
            Return _notifyIcon.Text
        End Get
        Set(ByVal value As String)
            _notifyIcon.Text = value
        End Set
    End Property
    <Description("Determines whether the control is visible or hidden"), Category("Behaviour"), DefaultValue(False)> _
    Public Property Visible() As Boolean
        Get
            Return _notifyIcon.Visible
        End Get
        Set(ByVal value As Boolean)
            _notifyIcon.Visible = value
        End Set
    End Property
#End Region

#Region "Events"
    ' ReSharper disable EventNeverInvoked - events are invoked in the event handlers!
    <Category("Action"), Description("Occurs when the icon is clicked")> _
    Public Event Click As EventHandler
    <Category("Mouse"), Description("Occurs when the mouse remains stationary inside the control for an amount of time")> _
    Public Event MouseHover As EventHandler
    <Category("Mouse"), Description("Occurs when the mouse leaves the visible part of the control")> _
    Public Event MouseLeave As EventHandler
    ' ReSharper restore EventNeverInvoked
#End Region

#Region "NotifyIcon Event Handlers"
    Private Sub notifyIcon_Click(ByVal sender As Object, ByVal e As EventArgs)
        OnClick(e)
    End Sub
#End Region

#Region "Event Handlers"
    Protected Overridable Sub OnClick(ByVal e As EventArgs)
        FireEvent(ClickEvent, e)
    End Sub

    Protected Overridable Sub OnMouseHover(ByVal e As EventArgs)
        FireEvent(MouseHoverEvent, e)
    End Sub

    Protected Overridable Sub OnMouseLeave(ByVal e As EventArgs)
        FireEvent(MouseLeaveEvent, e)
    End Sub

    Private Sub FireEvent(ByVal mainHandler As EventHandler, ByVal e As EventArgs)
        If (Not mainHandler Is Nothing) Then
            Dim handler As EventHandler
            For Each handler In mainHandler.GetInvocationList
                Dim sync As ISynchronizeInvoke = TryCast(handler.Target, ISynchronizeInvoke)
                If ((Not sync Is Nothing) AndAlso sync.InvokeRequired) Then
                    Dim result As IAsyncResult = sync.BeginInvoke(handler, New Object() {Me, e})
                    sync.EndInvoke(result)
                Else
                    handler.Invoke(Me, e)
                End If
            Next
        End If
    End Sub
#End Region

#Region "State Management"
    Private MustInherit Class MouseState
        ' Methods
        Public Sub New(ByVal trayIcon As EveHQIcon, ByVal mousePosition As Point)
            Me.trayIcon = trayIcon
            Me.mousePosition = mousePosition
            Me.syncLock = New Object
        End Sub

        Protected Sub ChangeState(ByVal state As States)
            Select Case state
                Case States.MouseOut
                    trayIcon._mouseState = New MouseStateOut(trayIcon)
                    Exit Select
                Case States.MouseOver
                    trayIcon._mouseState = New MouseStateOver(trayIcon, mousePosition)
                    Exit Select
                Case States.MouseHovering
                    TrayIcon._mouseState = New MouseStateHovering(TrayIcon, MousePosition)
                    Exit Select
            End Select
        End Sub

        Protected Sub DisableMouseTracking()
            SyncLock Me.syncLock
                If _mouseTrackingEnabled Then
                    RemoveHandler trayIcon._notifyIcon.MouseMove, New MouseEventHandler(AddressOf notifyIcon_MouseMove)
                    _mouseTrackingEnabled = False
                End If
            End SyncLock
        End Sub

        Protected Sub EnableMouseTracking()
            SyncLock Me.syncLock
                AddHandler trayIcon._notifyIcon.MouseMove, New MouseEventHandler(AddressOf notifyIcon_MouseMove)
                _mouseTrackingEnabled = True
            End SyncLock
        End Sub

        Private Sub notifyIcon_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            SyncLock Me.syncLock
                If _mouseTrackingEnabled Then
                    MousePosition = Control.MousePosition
                    OnMouseMove()
                End If
            End SyncLock
        End Sub

        Protected Overridable Sub OnMouseMove()
        End Sub

        ' Fields
        Protected MousePosition As Point
        Private _mouseTrackingEnabled As Boolean = False
        Protected ReadOnly [SyncLock] As Object
        Protected ReadOnly TrayIcon As EveHQIcon

        ' Nested Types
        Protected Enum States
            ' Fields
            MouseHovering = 2
            MouseOut = 0
            MouseOver = 1
        End Enum
    End Class

    Private Class MouseStateOut
        Inherits MouseState
        ' Methods
        Public Sub New(ByVal trayIcon As EveHQIcon)
            MyBase.New(trayIcon, New Point(0, 0))
            EnableMouseTracking()
        End Sub

        Protected Overrides Sub OnMouseMove()
            DisableMouseTracking()
            ChangeState(States.MouseOver)
        End Sub

    End Class

    Private Class MouseStateOver
        Inherits MouseState
        ' Methods
        Public Sub New(ByVal trayIcon As EveHQIcon, ByVal mousePosition As Point)
            MyBase.New(trayIcon, mousePosition)
            trayIcon._iconText = trayIcon._notifyIcon.Text
            trayIcon._notifyIcon.Text = ""
            SyncLock MyBase.syncLock
                _timer = New Timer(New TimerCallback(AddressOf HoverTimeout), Nothing, MyBase.TrayIcon.MouseHoverTime, -1)
                EnableMouseTracking()
            End SyncLock
        End Sub

        Private Sub HoverTimeout(ByVal state As Object)
            SyncLock MyBase.SyncLock
                If (Not _timer Is Nothing) Then
                    Try
                        _timer.Change(-1, -1)
                    Finally
                        _timer.Dispose()
                        _timer = Nothing
                    End Try
                    DisableMouseTracking()
                    If (Control.MousePosition = MousePosition) Then
                        ChangeState(States.MouseHovering)
                    Else
                        ChangeState(States.MouseOut)
                    End If
                End If
            End SyncLock
        End Sub

        Protected Overrides Sub OnMouseMove()
            Try
                _timer.Change(TrayIcon.MouseHoverTime, -1)
            Catch exception1 As ObjectDisposedException
            End Try
        End Sub

        ' Fields
        Private _timer As Timer
    End Class

    Private Class MouseStateHovering
        Inherits MouseState
        ' Methods
        Public Sub New(ByVal trayicon As EveHQIcon, ByVal mousePosition As Point)
            MyBase.New(trayicon, mousePosition)
            MyBase.trayIcon.OnMouseHover(New EventArgs)
            SyncLock MyBase.syncLock
                EnableMouseTracking()
                _timer = New Timer(New TimerCallback(AddressOf MouseMonitor), Nothing, 100, -1)
            End SyncLock
        End Sub

        Private Sub MouseMonitor(ByVal state As Object)
            SyncLock MyBase.SyncLock
                If (Control.MousePosition = MousePosition) Then
                    _timer.Change(100, -1)
                Else
                    _timer.Dispose()
                    DisableMouseTracking()
                    TrayIcon._notifyIcon.Text = TrayIcon._iconText
                    TrayIcon.OnMouseLeave(New EventArgs)
                    ChangeState(States.MouseOut)
                End If
            End SyncLock
        End Sub

        ' Fields
        Private ReadOnly _timer As Timer
    End Class
#End Region

#Region "Popup Management Methods"
    Public Shared Sub SetToolTipLocation(ByVal tooltipForm As Form)
        Dim mp As Point = Control.MousePosition
        Dim appBarData As NativeMethods.APPBARDATA = NativeMethods.APPBARDATA.Create
        NativeMethods.SHAppBarMessage(NativeMethods.ABM_GETTASKBARPOS, appBarData)
        Dim taskBarLocation As NativeMethods.RECT = appBarData.rc
        Dim winPoint As Point = mp
        Dim curScreen As Screen = Screen.FromPoint(mp)
        Dim slideLeftRight As Boolean = True
        Select Case appBarData.uEdge
            Case NativeMethods.ABE_BOTTOM
                winPoint = New Point(mp.X, taskBarLocation.Top - tooltipForm.Height)
                slideLeftRight = True
                Exit Select
            Case NativeMethods.ABE_TOP
                winPoint = New Point(mp.X, taskBarLocation.Bottom)
                slideLeftRight = True
                Exit Select
            Case NativeMethods.ABE_LEFT
                winPoint = New Point(taskBarLocation.Right, mp.Y)
                slideLeftRight = False
                Exit Select
            Case NativeMethods.ABE_RIGHT
                winPoint = New Point(taskBarLocation.Left - tooltipForm.Width, mp.Y)
                slideLeftRight = False
                Exit Select
        End Select
        If slideLeftRight Then
            If ((winPoint.X + tooltipForm.Width) > curScreen.Bounds.Right) Then
                winPoint = New Point(((curScreen.Bounds.Right - tooltipForm.Width) - 1), winPoint.Y)
            End If
            If (winPoint.X < curScreen.Bounds.Left) Then
                winPoint = New Point((curScreen.Bounds.Left + 2), winPoint.Y)
            End If
        Else
            If ((winPoint.Y + tooltipForm.Height) > curScreen.Bounds.Bottom) Then
                winPoint = New Point(winPoint.X, ((curScreen.Bounds.Bottom - tooltipForm.Height) - 1))
            End If
            If (winPoint.Y < curScreen.Bounds.Top) Then
                winPoint = New Point(winPoint.X, (curScreen.Bounds.Top + 2))
            End If
        End If
        tooltipForm.Location = winPoint
    End Sub
#End Region

    Friend Class NativeMethods
        ' Methods
        <DllImport("user32.dll")> _
        Public Shared Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
        End Function

        <DllImport("shell32.dll")> _
        Public Shared Function SHAppBarMessage(ByVal dwMessage As UInt32, ByRef pData As APPBARDATA) As IntPtr
        End Function

        ' Fields
        ' ReSharper disable InconsistentNaming
        Public Const ABE_BOTTOM As Integer = 3
        Public Const ABE_LEFT As Integer = 0
        Public Const ABE_RIGHT As Integer = 2
        Public Const ABE_TOP As Integer = 1
        Public Const ABM_GETTASKBARPOS As Integer = 5
        Public Const ABM_QUERYPOS As Integer = 2
        Public Const TaskbarClass As String = "Shell_TrayWnd"

        ' Nested Types
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure APPBARDATA
            Public cbSize As Integer
            Public hWnd As IntPtr
            Public uCallbackMessage As UInt32
            Public uEdge As UInt32
            Public rc As RECT
            Public lParam As Integer
            Public Shared Function Create() As APPBARDATA
                Dim appBarData As New APPBARDATA
                appBarData.cbSize = Marshal.SizeOf(GetType(APPBARDATA))
                Return appBarData
            End Function
        End Structure

        <Serializable(), StructLayout(LayoutKind.Sequential)> _
        Public Structure RECT
            Public ReadOnly Left As Integer
            Public ReadOnly Top As Integer
            Public Right As Integer
            Public Bottom As Integer
            Public Sub New(ByVal left_ As Integer, ByVal top_ As Integer, ByVal right_ As Integer, ByVal bottom_ As Integer)
                Left = left_
                Top = top_
                Right = right_
                Bottom = bottom_
            End Sub

            Public ReadOnly Property Height() As Integer
                Get
                    Return ((Bottom - Top) + 1)
                End Get
            End Property

            Public ReadOnly Property Width() As Integer
                Get
                    Return ((Right - Left) + 1)
                End Get
            End Property

            Public ReadOnly Property Size() As Size
                Get
                    Return New Size(Width, Height)
                End Get
            End Property

            Public ReadOnly Property Location() As Point
                Get
                    Return New Point(Left, Top)
                End Get
            End Property

            Public Function ToRectangle() As Rectangle
                Return Rectangle.FromLTRB(Left, Top, Right, Bottom)
            End Function

            Public Shared Function FromRectangle(ByVal rectangle As Rectangle) As RECT
                Return New RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
            End Function

            Public Overrides Function GetHashCode() As Integer
                Return (((Left Xor ((Top << 13) Or (Top >> &H13))) Xor ((Width << &H1A) Or (Width >> 6))) Xor ((Height << 7) Or (Height >> &H19)))
            End Function

            Public Shared Widening Operator CType(ByVal rect As RECT) As Rectangle
                Return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom)
            End Operator

            Public Shared Widening Operator CType(ByVal rect As Rectangle) As RECT
                Return New RECT(rect.Left, rect.Top, rect.Right, rect.Bottom)
            End Operator
        End Structure
        ' ReSharper restore InconsistentNaming
    End Class


End Class
