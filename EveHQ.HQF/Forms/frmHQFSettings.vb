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
Imports System.Xml
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmHQFSettings
    Dim redrawColumns As Boolean = False

#Region "Form Opening & Closing"
    Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Call Settings.HQFSettings.SaveHQFSettings()
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.UpdateGeneralOptions()
        Call Me.UpdateSlotColourOptions()

        If Me.Tag IsNot Nothing Then
            If Me.Tag.ToString = "" Then
                Me.Tag = "tabGeneral"
            End If
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
#End Region

#Region "General Options"
    Private Sub UpdateGeneralOptions()
        cboStartupPilot.Items.Clear()
        Dim myPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        For Each myPilot In EveHQ.Core.HQ.Pilots
            If myPilot.Active = True Then
                cboStartupPilot.Items.Add(myPilot.Name)
            End If
        Next
        If EveHQ.Core.HQ.Pilots.Contains(Settings.HQFSettings.DefaultPilot) = False Then
            If EveHQ.Core.HQ.Pilots.Count > 0 Then
                cboStartupPilot.SelectedIndex = 0
            End If
        Else
            cboStartupPilot.SelectedItem = Settings.HQFSettings.DefaultPilot
        End If
        chkRestoreLastSession.Checked = Settings.HQFSettings.RestoreLastSession
        chkAutoUpdateHQFSkills.Checked = Settings.HQFSettings.AutoUpdateHQFSkills
    End Sub
    Private Sub cboStartupPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartupPilot.SelectedIndexChanged
        Settings.HQFSettings.DefaultPilot = CStr(cboStartupPilot.SelectedItem)
    End Sub
    Private Sub chkRestoreLastSession_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRestoreLastSession.CheckedChanged
        Settings.HQFSettings.RestoreLastSession = chkRestoreLastSession.Checked
    End Sub
    Private Sub chkAutoUpdateHQFSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoUpdateHQFSkills.CheckedChanged
        Settings.HQFSettings.AutoUpdateHQFSkills = chkAutoUpdateHQFSkills.Checked
    End Sub
#End Region

#Region "Slot Colour Options"

    Private Sub UpdateSlotColourOptions()
        Dim HColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.HiSlotColour))
        Me.pbHiSlotColour.BackColor = HColor
        Dim MColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.MidSlotColour))
        Me.pbMidSlotColour.BackColor = MColor
        Dim LColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.LowSlotColour))
        Me.pbLowSlotColour.BackColor = LColor
        Dim RColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.RigSlotColour))
        Me.pbRigSlotColour.BackColor = RColor
    End Sub

    Private Sub pbHiSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbHiSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbHiSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.HiSlotColour = cd1.Color.ToArgb
        End If
    End Sub

    Private Sub pbMidSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbMidSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbMidSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.MidSlotColour = cd1.Color.ToArgb
        End If
    End Sub

    Private Sub pbLowSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLowSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbLowSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.LowSlotColour = cd1.Color.ToArgb
        End If
    End Sub

    Private Sub pbRigSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbRigSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbRigSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.RigSlotColour = cd1.Color.ToArgb
        End If
    End Sub

#End Region

#Region "Treeview Routines"
    Private Sub tvwSettings_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSettings.NodeMouseClick
        Dim nodeName As String = e.Node.Name
        Dim gbName As String = nodeName.TrimStart("node".ToCharArray)
        gbName = "gb" & gbName
        For Each setControl As Control In Me.Controls
            If setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or setControl.Name = gbName Then
                Me.Controls(gbName).Top = 12
                Me.Controls(gbName).Left = 195
                Me.Controls(gbName).Width = 500
                Me.Controls(gbName).Height = 500
                Me.Controls(gbName).Visible = True
            Else
                setControl.Visible = False
            End If
        Next
    End Sub

#End Region

#Region "Data Cache Options"
    Private Sub btnDeleteCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteCache.Click
        My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
    End Sub
#End Region


    
    
End Class
