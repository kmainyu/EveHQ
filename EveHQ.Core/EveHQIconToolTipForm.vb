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
Imports System.Runtime.InteropServices

Public Class EveHQIconToolTipForm

    Private _autoRefresh As Boolean
    Private _tooltip As String

    Protected Overrides Sub OnClosed(ByVal e As EventArgs)
        displayTimer.Stop()
        MyBase.OnClosed(e)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)
        _autoRefresh = False
        _tooltip = "This is a tooltip label!"
        UpdateForm()
    End Sub

    Protected Overrides Sub OnShown(ByVal e As EventArgs)
        MyBase.OnShown(e)
        If _autoRefresh Then
            displayTimer.Start()
        End If
        NativeMethods.SetWindowPos(Handle, -1, 0, 0, 0, 0, &H13)
        NativeMethods.ShowWindow(Handle, 4)
    End Sub

   Private Sub UpdateForm()
        SuspendLayout()
        Dim toolTip As String = _tooltip
        lblToolTip.Text = toolTip
        ResumeLayout()
        EveHQIcon.SetToolTipLocation(Me)
    End Sub

End Class

Friend Class NativeMethods
    ' Methods
    <DllImport("user32.dll")> _
    Public Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInt32) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal flags As Integer) As Boolean
    End Function

    ' Fields
    ' ReSharper disable InconsistentNaming - leave for native methods
    Public Const HWND_TOPMOST As Integer = -1
    Public Const SW_SHOWNOACTIVATE As Integer = 4
    Public Const SWP_NOACTIVATE As Integer = &H10
    Public Const SWP_NOMOVE As Integer = 2
    Public Const SWP_NOSIZE As Integer = 1
    ' ReSharper restore InconsistentNaming
End Class
