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
Imports EveHQ.Controls
Imports EveHQ.Core
Imports System.Runtime.InteropServices

Namespace Forms

    Public Class FrmToolTrayIconPopup

        Dim _currentLabel As New Label
        Dim _currentDate As Date

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            MyBase.OnPaint(e)
        End Sub

        Protected Overrides Sub OnClosed(ByVal e As EventArgs)
            displayTimer.Stop()
            MyBase.OnClosed(e)
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            MyBase.OnLoad(e)
            displayTimer.Start()
            UpdateForm()
        End Sub

        Protected Overrides Sub OnShown(ByVal e As EventArgs)
            MyBase.OnShown(e)
            NativeMethods.SetWindowPos(Handle, -1, 0, 0, 0, 0, &H13)
            NativeMethods.ShowWindow(Handle, 4)
        End Sub

        Private Sub displayTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles displayTimer.Tick
            UpdateSkillTimes()
        End Sub

        Private Sub UpdateForm()
            EveHQIcon.SetToolTipLocation(Me)
        End Sub

        Private Sub ConfigureForm()
            Dim count As Integer = 0
            For Each dPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If dPilot.Training = True And dPilot.Active = True Then
                    count += 1
                    Dim newCharBlock As New CharacterBlock(dPilot.Name)
                    newCharBlock.Name = dPilot.Name
                    newCharBlock.Location = New Point(5, 5 + (newCharBlock.Height * (count - 1)))
                    AGP1.Controls.Add(newCharBlock)
                End If
            Next
        End Sub

        Public Sub UpdateSkillTimes()
            For Each currentPilot In HQ.Settings.Pilots.Values
                If AGP1.Controls.ContainsKey(currentPilot.Name) Then
                    _currentLabel = CType(AGP1.Controls(currentPilot.Name), CharacterBlock).lblTime
                    _currentDate = SkillFunctions.ConvertEveTimeToLocal(currentPilot.TrainingEndTime)
                    _currentLabel.Text = Format(_currentDate, "ddd") & " " & _currentDate & " (" & SkillFunctions.TimeToString(currentPilot.TrainingCurrentTime) & ")"
                End If
            Next
        End Sub

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            SuspendLayout()
            ConfigureForm()
            ResumeLayout()

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
        ' ReSharper disable InconsistentNaming - best leave native items alone!
        Public Const HWND_TOPMOST As Integer = -1
        Public Const SW_SHOWNOACTIVATE As Integer = 4
        Public Const SWP_NOACTIVATE As Integer = &H10
        Public Const SWP_NOMOVE As Integer = 2
        Public Const SWP_NOSIZE As Integer = 1
        ' ReSharper restore InconsistentNaming
    End Class
End NameSpace