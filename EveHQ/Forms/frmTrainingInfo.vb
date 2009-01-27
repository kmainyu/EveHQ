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
Imports System.Runtime.InteropServices

Public Class frmTrainingInfo

    Private _InitialStyle As Integer
    Dim displayPilot As Integer = 1
    Dim updateRequired As Boolean = True

    Private Sub Panel1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel1.DoubleClick
        frmEveHQ.tsbTrainingOverlay.Checked = False
        frmEveHQ.TrainingInformationToolStripMenuItem.Checked = False
        Me.Close()
    End Sub

    Public Sub UpdateTraining()
        If tmrAccount.Enabled = False Then
            tmrAccount.Enabled = True
        End If
        If displayPilot > 0 And displayPilot <= EveHQ.Core.HQ.EveHQSettings.Pilots.Count Then
            Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots.Item(displayPilot), Core.Pilot)
            If pbPilot.Image Is Nothing Then
                Dim imgFilename As String = EveHQ.Core.HQ.cacheFolder & "\i" & cpilot.ID & ".png"
                If My.Computer.FileSystem.FileExists(imgFilename) = True Then
                    pbPilot.ImageLocation = imgFilename
                Else
                    pbPilot.Image = My.Resources.noitem
                End If
            End If
            If cpilot.Training = True Then
                lblTrainingStatus.Text = ""
                lblPilot.Text = cpilot.Name
                lblTrainingStatus.Text &= "Currently training: " & cpilot.TrainingSkillName & " (Lvl " & cpilot.TrainingSkillLevel & ")" & ControlChars.CrLf
                Dim trainingTime As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cpilot)
                Dim trainingSP As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(cpilot)
                lblTrainingStatus.Text &= EveHQ.Core.SkillFunctions.TimeToString(trainingTime) & " remaining" & ControlChars.CrLf
                lblTrainingStatus.Text &= FormatNumber(cpilot.TrainingStartSP + trainingSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " of " & FormatNumber(cpilot.TrainingEndSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " SP trained"
            Else
                If updateRequired = True Then
                    lblTrainingStatus.Text = ""
                    lblPilot.Text = cpilot.Name
                    lblTrainingStatus.Text &= "Not currently training!" & ControlChars.CrLf
                    lblTrainingStatus.Text &= "Current skillpoints: " & FormatNumber(cpilot.SkillPoints, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ControlChars.CrLf
                    lblTrainingStatus.Text &= "Current isk: " & FormatNumber(cpilot.Isk, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    updateRequired = False
                End If
            End If
        End If
    End Sub

    Private Sub tmrAccount_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAccount.Tick
        Dim cpilot As EveHQ.Core.Pilot
        Do
            displayPilot += 1
            If displayPilot > EveHQ.Core.HQ.EveHQSettings.Pilots.Count Then
                displayPilot = 1
            End If
            cpilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots.Item(displayPilot), Core.Pilot)
        Loop Until cpilot.Active = True
        updateRequired = True
        lblPilot.Text = cpilot.Name
        Dim imgFilename As String = EveHQ.Core.HQ.cacheFolder & "\i" & cpilot.ID & ".png"
        If My.Computer.FileSystem.FileExists(imgFilename) = True Then
            pbPilot.ImageLocation = imgFilename
        Else
            pbPilot.Image = My.Resources.noitem
        End If
        Call Me.UpdateTraining()
    End Sub

    Private Sub frmTrainingInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Grab the Extended Style information for this window and store it.
        _InitialStyle = TransparantWindow.GetWindowLong(Me.Handle, TransparantWindow.GWL.ExStyle)

        ' Set this window to Transparent (to the mouse that is!)
        If EveHQ.Core.HQ.EveHQSettings.OverlayClickThru = True Then
            SetFormToTransparent()
        End If

        ' Just for giggles, set this window to stay on top of all others so we can see what's happening.
        Me.TopMost = True
    End Sub
    Private Sub SetFormToTransparent()
        TransparantWindow.SetWindowLong(Me.Handle, TransparantWindow.GWL.ExStyle, CType(_InitialStyle Or TransparantWindow.WS_EX.Layered Or TransparantWindow.WS_EX.Transparent, TransparantWindow.WS_EX))
        TransparantWindow.SetLayeredWindowAttributes(Me.Handle, 0, CByte(255 * 0.7), TransparantWindow.LWA.Alpha)
    End Sub
    Private Sub SetFormToOpaque()
        ' Turn off the Transparent Extended Style.
        TransparantWindow.SetWindowLong(Me.Handle, TransparantWindow.GWL.ExStyle, CType(_InitialStyle Or TransparantWindow.WS_EX.Layered, TransparantWindow.WS_EX))

        ' Set the Alpha back to 100% opaque.
        TransparantWindow.SetLayeredWindowAttributes(Me.Handle, 0, 255, TransparantWindow.LWA.Alpha)
    End Sub
End Class

Public Class TransparantWindow

    Public Enum GWL As Integer
        ExStyle = -20
    End Enum

    Public Enum WS_EX As Integer
        Transparent = &H20
        Layered = &H80000
    End Enum

    Public Enum LWA As Integer
        ColorKey = &H1
        Alpha = &H2
    End Enum

    <DllImport("user32.dll", EntryPoint:="GetWindowLong")> _
    Public Shared Function GetWindowLong( _
        ByVal hWnd As IntPtr, _
        ByVal nIndex As GWL _
            ) As Integer
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowLong")> _
    Public Shared Function SetWindowLong( _
        ByVal hWnd As IntPtr, _
        ByVal nIndex As GWL, _
        ByVal dwNewLong As WS_EX _
            ) As Integer
    End Function

    <DllImport("user32.dll", _
      EntryPoint:="SetLayeredWindowAttributes")> _
    Public Shared Function SetLayeredWindowAttributes( _
        ByVal hWnd As IntPtr, _
        ByVal crKey As Integer, _
        ByVal alpha As Byte, _
        ByVal dwFlags As LWA _
            ) As Boolean
    End Function
End Class