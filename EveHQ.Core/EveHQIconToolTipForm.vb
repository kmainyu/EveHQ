Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports System.Text

Public Class EveHQIconToolTipForm

    Private m_autoRefresh As Boolean
    Private m_tooltip As String

    Protected Overrides Sub OnClosed(ByVal e As EventArgs)
        Me.displayTimer.Stop()
        MyBase.OnClosed(e)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)
        Me.m_autoRefresh = False
        Me.m_tooltip = "This is a tooltip label!"
        Me.UpdateForm()
    End Sub

    Protected Overrides Sub OnShown(ByVal e As EventArgs)
        MyBase.OnShown(e)
        If Me.m_autoRefresh Then
            Me.displayTimer.Start()
        End If
        NativeMethods.SetWindowPos(MyBase.Handle, -1, 0, 0, 0, 0, &H13)
        NativeMethods.ShowWindow(MyBase.Handle, 4)
    End Sub

    Private Sub displayTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        Me.UpdateForm()
    End Sub

    Private Sub UpdateForm()
        MyBase.SuspendLayout()
        Dim toolTip As String = Me.m_tooltip
        Me.lblToolTip.Text = toolTip
        MyBase.ResumeLayout()
        EveHQIcon.SetToolTipLocation(Me)
    End Sub

End Class

Friend Class NativeMethods
    ' Methods
    <DllImport("user32.dll")> _
    Public Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInt32) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal flags As Integer) As Boolean
    End Function

    ' Fields
    Public Const HWND_TOPMOST As Integer = -1
    Public Const SW_SHOWNOACTIVATE As Integer = 4
    Public Const SWP_NOACTIVATE As Integer = &H10
    Public Const SWP_NOMOVE As Integer = 2
    Public Const SWP_NOSIZE As Integer = 1
End Class
