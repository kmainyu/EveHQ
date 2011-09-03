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
Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Runtime.InteropServices

Public Class EveHQIcon
    Inherits Component

    Private iconText As String
    Private cMouseHoverTime As Integer
    Private cMouseState As MouseState
    Private notifyIcon As NotifyIcon


#Region "Constructors"
   
#End Region

#Region "Methods"
    Public Sub ShowBalloonTip(ByVal timeout As Integer)
        Call Me.notifyIcon.ShowBalloonTip(timeout)
    End Sub
    Public Sub ShowBalloonTip(ByVal timeout As Integer, ByVal tipTitle As String, ByVal tipText As String, ByVal tipIcon As ToolTipIcon)
        Call Me.notifyIcon.ShowBalloonTip(timeout, tipTitle, tipText, tipIcon)
    End Sub
#End Region

#Region "Properties"
    <Category("Appearance"), Description("The title of balloon popup")> _
  Public Property BalloonTipIcon() As ToolTipIcon
        Get
            Return Me.notifyIcon.BalloonTipIcon
        End Get
        Set(ByVal value As ToolTipIcon)
            Me.notifyIcon.BalloonTipIcon = value
        End Set
    End Property
    <Category("Appearance"), Description("The title of balloon popup")> _
   Public Property BalloonTipText() As String
        Get
            Return Me.notifyIcon.BalloonTipText
        End Get
        Set(ByVal value As String)
            Me.notifyIcon.BalloonTipText = value
        End Set
    End Property
    <Category("Appearance"), Description("The title of balloon popup")> _
   Public Property BalloonTipTitle() As String
        Get
            Return Me.notifyIcon.BalloonTipTitle
        End Get
        Set(ByVal value As String)
            Me.notifyIcon.BalloonTipTitle = value
        End Set
    End Property
    <Category("Behaviour")> _
    Public Property ContextMenuStrip() As ContextMenuStrip
        Get
            Return Me.notifyIcon.ContextMenuStrip
        End Get
        Set(ByVal value As ContextMenuStrip)
            Me.notifyIcon.ContextMenuStrip = value
        End Set
    End Property
    <Description("The icon to display in the system tray"), Category("Appearance")> _
    Public Property Icon() As Icon
        Get
            Return Me.notifyIcon.Icon
        End Get
        Set(ByVal value As Icon)
            Me.notifyIcon.Icon = New Icon(value, New Size(&H10, &H10))
        End Set
    End Property
    <Category("Behaviour"), DefaultValue(250), Description("The length of time, in milliseconds, for which the mouse must remain stationary over the control before the MouseHover event is raised")> _
    Public Property MouseHoverTime() As Integer
        Get
            Return Me.cMouseHoverTime
        End Get
        Set(ByVal value As Integer)
            Me.cMouseHoverTime = value
        End Set
    End Property
    <Category("Appearance"), Description("The text that will be displayed when the mouse hovers over the icon")> _
    Public Property [Text]() As String
        Get
            Return Me.notifyIcon.Text
        End Get
        Set(ByVal value As String)
            Me.notifyIcon.Text = value
        End Set
    End Property
    <Description("Determines whether the control is visible or hidden"), Category("Behaviour"), DefaultValue(False)> _
    Public Property Visible() As Boolean
        Get
            Return Me.notifyIcon.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.notifyIcon.Visible = value
        End Set
    End Property
#End Region

#Region "Events"
    <Category("Action"), Description("Occurs when the icon is clicked")> _
   Public Event Click As EventHandler
    <Category("Mouse"), Description("Occurs when the mouse remains stationary inside the control for an amount of time")> _
    Public Event MouseHover As EventHandler
    <Category("Mouse"), Description("Occurs when the mouse leaves the visible part of the control")> _
    Public Event MouseLeave As EventHandler
#End Region

#Region "NotifyIcon Event Handlers"
    Private Sub notifyIcon_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.OnClick(e)
    End Sub
#End Region

#Region "Event Handlers"
    Protected Overridable Sub OnClick(ByVal e As EventArgs)
        Me.FireEvent(Me.ClickEvent, e)
    End Sub

    Protected Overridable Sub OnMouseHover(ByVal e As EventArgs)
        Me.FireEvent(Me.MouseHoverEvent, e)
    End Sub

    Protected Overridable Sub OnMouseLeave(ByVal e As EventArgs)
        Me.FireEvent(Me.MouseLeaveEvent, e)
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
                    Me.trayIcon.cMouseState = New MouseStateOut(Me.trayIcon)
                    Exit Select
                Case States.MouseOver
                    Me.trayIcon.cMouseState = New MouseStateOver(Me.trayIcon, Me.mousePosition)
                    Exit Select
                Case States.MouseHovering
                    Me.trayIcon.cMouseState = New MouseStateHovering(Me.trayIcon, Me.mousePosition)
                    Exit Select
            End Select
        End Sub

        Protected Sub DisableMouseTracking()
            SyncLock Me.syncLock
                If Me.mouseTrackingEnabled Then
                    RemoveHandler Me.trayIcon.notifyIcon.MouseMove, New MouseEventHandler(AddressOf Me.notifyIcon_MouseMove)
                    Me.mouseTrackingEnabled = False
                End If
            End SyncLock
        End Sub

        Protected Sub EnableMouseTracking()
            SyncLock Me.syncLock
                AddHandler Me.trayIcon.notifyIcon.MouseMove, New MouseEventHandler(AddressOf Me.notifyIcon_MouseMove)
                Me.mouseTrackingEnabled = True
            End SyncLock
        End Sub

        Private Sub notifyIcon_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            SyncLock Me.syncLock
                If Me.mouseTrackingEnabled Then
                    Me.mousePosition = Control.MousePosition
                    Me.OnMouseMove()
                End If
            End SyncLock
        End Sub

        Protected Overridable Sub OnMouseMove()
        End Sub

        ' Fields
        Protected mousePosition As Point
        Private mouseTrackingEnabled As Boolean = False
        Protected [syncLock] As Object
        Protected trayIcon As EveHQIcon

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
            MyBase.EnableMouseTracking()
        End Sub

        Protected Overrides Sub OnMouseMove()
            MyBase.DisableMouseTracking()
            MyBase.ChangeState(States.MouseOver)
        End Sub

    End Class

    Private Class MouseStateOver
        Inherits MouseState
        ' Methods
        Public Sub New(ByVal trayIcon As EveHQIcon, ByVal mousePosition As Point)
            MyBase.New(trayIcon, mousePosition)
            trayIcon.iconText = trayIcon.notifyIcon.Text
            trayIcon.notifyIcon.Text = ""
            SyncLock MyBase.syncLock
                Me.timer = New Threading.Timer(New Threading.TimerCallback(AddressOf Me.HoverTimeout), Nothing, MyBase.trayIcon.MouseHoverTime, -1)
                MyBase.EnableMouseTracking()
            End SyncLock
        End Sub

        Private Sub HoverTimeout(ByVal state As Object)
            SyncLock MyBase.syncLock
                If (Not Me.timer Is Nothing) Then
                    Try
                        Me.timer.Change(-1, -1)
                    Finally
                        Me.timer.Dispose()
                        Me.timer = Nothing
                    End Try
                    MyBase.DisableMouseTracking()
                    If (Control.MousePosition = MyBase.mousePosition) Then
                        MyBase.ChangeState(States.MouseHovering)
                    Else
                        MyBase.ChangeState(States.MouseOut)
                    End If
                End If
            End SyncLock
        End Sub

        Protected Overrides Sub OnMouseMove()
            Try
                Me.timer.Change(MyBase.trayIcon.MouseHoverTime, -1)
            Catch exception1 As ObjectDisposedException
            End Try
        End Sub

        ' Fields
        Private timer As Threading.Timer
    End Class

    Private Class MouseStateHovering
        Inherits MouseState
        ' Methods
        Public Sub New(ByVal trayicon As EveHQIcon, ByVal mousePosition As Point)
            MyBase.New(trayicon, mousePosition)
            MyBase.trayIcon.OnMouseHover(New EventArgs)
            SyncLock MyBase.syncLock
                MyBase.EnableMouseTracking()
                Me.timer = New Threading.Timer(New Threading.TimerCallback(AddressOf Me.MouseMonitor), Nothing, 100, -1)
            End SyncLock
        End Sub

        Private Sub MouseMonitor(ByVal state As Object)
            SyncLock MyBase.syncLock
                If (Control.MousePosition = MyBase.mousePosition) Then
                    Me.timer.Change(100, -1)
                Else
                    Me.timer.Dispose()
                    MyBase.DisableMouseTracking()
                    MyBase.trayIcon.notifyIcon.Text = MyBase.trayIcon.iconText
                    MyBase.trayIcon.OnMouseLeave(New EventArgs)
                    MyBase.ChangeState(States.MouseOut)
                End If
            End SyncLock
        End Sub

        ' Fields
        Private timer As Threading.Timer
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
            Public Left As Integer
            Public Top As Integer
            Public Right As Integer
            Public Bottom As Integer
            Public Sub New(ByVal left_ As Integer, ByVal top_ As Integer, ByVal right_ As Integer, ByVal bottom_ As Integer)
                Me.Left = left_
                Me.Top = top_
                Me.Right = right_
                Me.Bottom = bottom_
            End Sub

            Public ReadOnly Property Height() As Integer
                Get
                    Return ((Me.Bottom - Me.Top) + 1)
                End Get
            End Property

            Public ReadOnly Property Width() As Integer
                Get
                    Return ((Me.Right - Me.Left) + 1)
                End Get
            End Property

            Public ReadOnly Property Size() As Size
                Get
                    Return New Size(Me.Width, Me.Height)
                End Get
            End Property

            Public ReadOnly Property Location() As Point
                Get
                    Return New Point(Me.Left, Me.Top)
                End Get
            End Property

            Public Function ToRectangle() As Rectangle
                Return Rectangle.FromLTRB(Me.Left, Me.Top, Me.Right, Me.Bottom)
            End Function

            Public Shared Function FromRectangle(ByVal rectangle As Rectangle) As RECT
                Return New RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
            End Function

            Public Overrides Function GetHashCode() As Integer
                Return (((Me.Left Xor ((Me.Top << 13) Or (Me.Top >> &H13))) Xor ((Me.Width << &H1A) Or (Me.Width >> 6))) Xor ((Me.Height << 7) Or (Me.Height >> &H19)))
            End Function

            Public Shared Widening Operator CType(ByVal rect As RECT) As Rectangle
                Return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom)
            End Operator

            Public Shared Widening Operator CType(ByVal rect As Rectangle) As RECT
                Return New RECT(rect.Left, rect.Top, rect.Right, rect.Bottom)
            End Operator
        End Structure
    End Class


End Class
