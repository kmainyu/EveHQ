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
Imports System.Windows.Forms
Imports System
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Xml
Imports System.Reflection
Imports System.Runtime.InteropServices

Public Class frmEveHQ
    Dim WithEvents eveTQWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents eveSisiWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents IGBWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Public WithEvents APIRSWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents SkillWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents ImportWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents BackupWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents ReportWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Private Declare Auto Function SetWindowPos Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal x As Int32, ByVal y As Int32, ByVal cx As Int32, ByVal cy As Int32, ByVal wFlags As Int32) As Int32
    Private Declare Auto Function MoveWindow Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal x As Int32, ByVal y As Int32, ByVal nWidth As Int32, ByVal nHeight As Int32, ByVal bRepaint As Boolean) As Int32
    Private Declare Function GetWindowRect Lib "user32.dll" (ByVal hwnd As Int32, ByRef lpRect As RECT) As Boolean
    Private Declare Function GetClientRect Lib "user32.dll" (ByVal hwnd As Int32, ByRef lpRect As RECT) As Int32
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Long
    Private Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal hWnd1 As Integer, ByVal hWnd2 As Integer, ByVal lpsz1 As String, ByVal lpsz2 As String) As Integer
    Private Declare Function SHAppBarMessage Lib "shell32.dll" Alias "SHAppBarMessage" (ByVal dwMessage As Int32, ByRef pData As APPBARDATA) As Int32
    Private Delegate Sub QueryMyEveServerDelegate()
    Private m_ChildFormNumber As Integer = 0
    Private childFormCount As Integer = 0
    Dim EveHQMLW As New SortedList
    Dim EveHQMLF As New frmMarketPrices
    Friend Structure RECT
        Friend Left As Int32
        Friend Top As Int32
        Friend Right As Int32
        Friend Bottom As Int32
        Friend Sub New(ByVal Left%, ByVal Top%, ByVal Right%, ByVal Bottom%)
            Me.Left = Left : Me.Bottom = Bottom : Me.Right = Right : Me.Top = Top
        End Sub
        Public Shared Widening Operator CType(ByVal a As Rectangle) As RECT
            Return New RECT(a.Left, a.Top, a.Right, a.Bottom)
        End Operator
        Public Shared Widening Operator CType(ByVal a As RECT) As Rectangle
            Return New Rectangle(a.Left, a.Top, a.Right - a.Left, a.Bottom - a.Top)
        End Operator
    End Structure
    Private Const ABM_GETTASKBARPOS As Int32 = &H5
    Private Const ABM_GETSTATE As Int32 = &H4
    Private Const ABS_AUTOHIDE As Int32 = &H1
    Private Const ABS_ALWAYSONTOP As Int32 = &H2
    Private Structure APPBARDATA
        Dim cbSize As Int32
        Dim hwnd As IntPtr
        Dim uCallbackMessage As [Delegate]
        Dim uEdge As Int32
        Dim rc As RECT
        Dim lParam As Int32
    End Structure

#Region "Menu Click Routines"

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Global.System.Windows.Forms.Application.Exit()
    End Sub
    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
    Private Sub TileVerticleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub
    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub
    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub
    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click, tsbSettings.Click
        frmSettings.ShowDialog()
    End Sub
    Private Sub PilotInfoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PilotInfoToolStripMenuItem.Click, tsbPilotInfo.Click
        Call OpenPilotInfoForm()
    End Sub
    Private Sub RunIGBToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunIGBToolStripMenuItem.Click, tsbIGB.Click

        If RunIGBToolStripMenuItem.Checked = False Then
            If IGBWorker.CancellationPending = True Then
                MessageBox.Show("The IGB Server is still shutting down. Please wait a few moments", "IGB Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                IGBWorker.Dispose()
                IGBWorker = New System.ComponentModel.BackgroundWorker
            Else
                IGBWorker.WorkerSupportsCancellation = True
                IGBWorker.RunWorkerAsync()
                RunIGBToolStripMenuItem.Checked = True
                EveHQ.Core.HQ.IGBActive = True
                tsbIGB.Checked = True
            End If
        Else
            IGBWorker.CancelAsync()
            RunIGBToolStripMenuItem.Checked = False
            EveHQ.Core.HQ.IGBActive = False
            tsbIGB.Checked = False
        End If
    End Sub
    Private Sub ForceServerCheckToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForceServerCheckToolStripMenuItem.Click
        Call GetServerStatus()
    End Sub
    Private Sub HideWhenMinimisedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideWhenMinimisedToolStripMenuItem.Click
        If HideWhenMinimisedToolStripMenuItem.Checked = True Then
            HideWhenMinimisedToolStripMenuItem.Checked = False
            frmSettings.chkAutoHide.Checked = False
            EveHQ.Core.HQ.EveHQSettings.AutoHide = False
        Else
            HideWhenMinimisedToolStripMenuItem.Checked = True
            frmSettings.chkAutoHide.Checked = True
            EveHQ.Core.HQ.EveHQSettings.AutoHide = True
        End If
    End Sub
    Private Sub WebBrowserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebBrowserToolStripMenuItem.Click, tsbWebBrowser.Click
        Call OpenWebBrowserForm()
    End Sub
    Private Sub mnuToolsGetAccountInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsGetAccountInfo.Click, tsbRetrieveData.Click
        Call QueryMyEveServer()
    End Sub
    Private Sub SkillTrainingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkillTrainingToolStripMenuItem.Click, tsbSkillTraining.Click
        Call OpenSkillTrainingForm()
    End Sub
    Private Sub ctxExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxExit.Click
        Call Me.ShutdownRoutine()
    End Sub
    Private Sub ctxAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxAbout.Click
        If frmAbout.Visible = False Then
            frmAbout.ShowDialog()
        End If
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()
        End
    End Sub
    Private Sub RestoreWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestoreWindowToolStripMenuItem.Click
        ' Restores the window
        Me.Show()
        Select Case EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4)
            Case FormWindowState.Maximized
                Me.WindowState = FormWindowState.Maximized
            Case FormWindowState.Normal
                Me.Left = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(0)
                Me.Top = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(1)
                Me.Width = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(2)
                Me.Height = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(3)
                Me.WindowState = FormWindowState.Normal
        End Select
    End Sub

#End Region

#Region "Server Status Routines"

    Private Sub GetServerStatus()
        If eveTQWorker.IsBusy Then
        Else
            eveTQWorker.RunWorkerAsync()
        End If
        If eveSisiWorker.IsBusy Then
        Else
            eveSisiWorker.RunWorkerAsync()
        End If
    End Sub
    Private Sub tmrEve_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrEve.Tick
        tmrEve.Interval = 60000
        Call GetServerStatus()
    End Sub
    Private Sub eveTQWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles eveTQWorker.DoWork
        ' Defines what work the thread has to do
        Call EveHQ.Core.HQ.myTQServer.GetServerStatus()
    End Sub
    Private Sub eveTQWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles eveTQWorker.RunWorkerCompleted
        ' Sub raised on the completion of a call to read the Eve TQ data
        ' Depending on server status, set the notify icon text and the statusbar text
        Select Case EveHQ.Core.HQ.myTQServer.Status
            Case EveHQ.Core.EveServer.ServerStatus.Down
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Unable to connect to server"
                EveStatusIcon.Icon = My.Resources.EveHQ_offline
            Case EveHQ.Core.EveServer.ServerStatus.Starting
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                EveStatusIcon.Icon = My.Resources.EveHQ_starting
            Case EveHQ.Core.EveServer.ServerStatus.Shutting
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                EveStatusIcon.Icon = My.Resources.EveHQ_starting
            Case EveHQ.Core.EveServer.ServerStatus.Full
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                EveStatusIcon.Icon = My.Resources.EveHQ_starting
            Case EveHQ.Core.EveServer.ServerStatus.ProxyDown
                tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
                EveStatusIcon.Icon = My.Resources.EveHQ_starting
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
            Case EveHQ.Core.EveServer.ServerStatus.Unknown
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Status unknown"
                EveStatusIcon.Icon = My.Resources.EveHQ
            Case EveHQ.Core.EveServer.ServerStatus.Up
                Dim msg As String = EveHQ.Core.HQ.myTQServer.ServerName & ":" & vbCrLf
                msg = msg & "Version: " & EveHQ.Core.HQ.myTQServer.Version & vbCrLf
                msg = msg & "Players: " & EveHQ.Core.HQ.myTQServer.Players
                If msg.Length > 50 Then
                    EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.ServerName & ":" & vbCrLf & "Server currently initialising"
                    tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Server currently initialising"
                Else
                    EveStatusIcon.Text = msg
                    tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": version " & EveHQ.Core.HQ.myTQServer.Version & " (" & EveHQ.Core.HQ.myTQServer.Players & " pilots)"
                End If
                EveStatusIcon.Icon = My.Resources.EveHQ_online
        End Select

        ' Check if the server status has changed since the last result and notify user
        If EveHQ.Core.HQ.myTQServer.Status <> EveHQ.Core.HQ.myTQServer.LastStatus Then
            EveStatusIcon.BalloonTipIcon = ToolTipIcon.Info
            EveStatusIcon.BalloonTipTitle = EveHQ.Core.HQ.myTQServer.ServerName & " Status Notification"
            Select Case EveHQ.Core.HQ.myTQServer.Status
                Case EveHQ.Core.EveServer.ServerStatus.Down
                    EveStatusIcon.BalloonTipText = EveHQ.Core.HQ.myTQServer.ServerName & " is Down"
                Case EveHQ.Core.EveServer.ServerStatus.Starting
                    EveStatusIcon.BalloonTipText = EveHQ.Core.HQ.myTQServer.ServerName & " is Starting Up"
                Case EveHQ.Core.EveServer.ServerStatus.Unknown
                    EveStatusIcon.BalloonTipText = EveHQ.Core.HQ.myTQServer.ServerName & " status is Unknown"
                Case EveHQ.Core.EveServer.ServerStatus.Up
                    EveStatusIcon.BalloonTipText = EveHQ.Core.HQ.myTQServer.ServerName & " is Up"
            End Select
            EveStatusIcon.ShowBalloonTip(3000)
        End If
        ' Update last status
        EveHQ.Core.HQ.myTQServer.LastStatus = EveHQ.Core.HQ.myTQServer.Status

    End Sub
    Private Sub eveSisiWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles eveSisiWorker.DoWork
        ' Defines what work the thread has to do
        Call EveHQ.Core.HQ.mySiSiServer.GetServerStatus()
    End Sub
    Private Sub eveSisiWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles eveSisiWorker.RunWorkerCompleted
        ' Sub raised on the completion of a call to read the Eve Sisi data
        ' For the singularity server, we don't want to give details in the icon area!!
        ' We also don't need notification of Sisi server status changes at this point

        ' Depending on server status, set the notify icon text and the statusbar text
        Select Case EveHQ.Core.HQ.mySiSiServer.Status
            Case EveHQ.Core.EveServer.ServerStatus.Down
                tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": Unable to connect to server"
            Case EveHQ.Core.EveServer.ServerStatus.Starting
                tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
            Case EveHQ.Core.EveServer.ServerStatus.Shutting
                tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
            Case EveHQ.Core.EveServer.ServerStatus.Full
                tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
            Case EveHQ.Core.EveServer.ServerStatus.Unknown
                tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": Status unknown"
            Case EveHQ.Core.EveServer.ServerStatus.ProxyDown
                tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
            Case EveHQ.Core.EveServer.ServerStatus.Up

                Dim msg As String = EveHQ.Core.HQ.mySiSiServer.ServerName & ": version " & EveHQ.Core.HQ.mySiSiServer.Version & " (" & EveHQ.Core.HQ.mySiSiServer.Players & " pilots)"
                If msg.Length > 50 Then
                    tsSisiStatus.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": Server currently initialising"
                Else
                    tsSisiStatus.Text = msg
                End If
        End Select

    End Sub

#End Region

#Region "Form Opening & Closing & Resizing (+ Icon)"

    Private Sub frmEveHQ_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' Check if we should minimise rather than exit?
        If e.CloseReason <> CloseReason.TaskManagerClosing And e.CloseReason <> CloseReason.WindowsShutDown Then
            If EveHQ.Core.HQ.EveHQSettings.MinimiseExit = True Then
                Me.WindowState = FormWindowState.Minimized
                e.Cancel = True
                Exit Sub
            Else
                Call Me.ShutdownRoutine()
            End If
        Else
            Call Me.ShutdownRoutine()
        End If
    End Sub
    Private Sub frmEveHQ_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Hide()

        Me.MenuStrip.MdiWindowListItem = Nothing

        ' Add the pilot refresh handler
        AddHandler EveHQ.Core.PilotParseFunctions.RefreshPilots, AddressOf Me.RemoteRefreshPilots
        AddHandler EveHQ.Core.G15LCDB.UpdateAPI, AddressOf Me.RemoteUpdate
        AddHandler EveHQ.Core.HQ.CloseInfoPanel, AddressOf Me.CloseInfoPanel
        AddHandler EveHQ.Core.HQ.ShutDownEveHQ, AddressOf Me.ShutdownRoutine

        ' Check if "Hide When Minimised" is active
        HideWhenMinimisedToolStripMenuItem.Checked = EveHQ.Core.HQ.EveHQSettings.AutoHide

        'Setup the Modules menu if applicable
        Call Me.SetupModuleMenu()

        ' Check if the IGB should be started here
        If EveHQ.Core.HQ.EveHQSettings.IGBAutoStart = True Then
            If Not System.Net.HttpListener.IsSupported Then
                RunIGBToolStripMenuItem.Checked = False
                RunIGBToolStripMenuItem.Enabled = False
                tsbIGB.Enabled = False
                tsbIGB.Checked = False
            Else
                IGBWorker.WorkerSupportsCancellation = True
                IGBWorker.RunWorkerAsync()
                RunIGBToolStripMenuItem.Checked = True
                tsbIGB.Checked = True
                EveHQ.Core.HQ.IGBActive = True
            End If
        End If

        ' Check if the APIRS should be started 
        If EveHQ.Core.HQ.EveHQSettings.APIRSAutoStart = True Then
            If System.Net.HttpListener.IsSupported Then
                APIRSWorker.WorkerSupportsCancellation = True
                APIRSWorker.RunWorkerAsync()
                EveHQ.Core.HQ.APIRSActive = True
            End If
        End If

        ' Update the panel colours
        Call Me.UpdatePanelColours()

        ' Determine which view to display!
        Select Case EveHQ.Core.HQ.EveHQSettings.StartupView
            Case "Pilot Information"
                If EveHQ.Core.HQ.myPilot.Name <> "" And EveHQ.Core.HQ.myPilot.PilotSkills.Count <> 0 Then
                    ' Open the pilot info form
                    Call OpenPilotInfoForm()
                End If
            Case "Pilot Summary Report"
                ' Show the pilot summary report form!
                Dim newReport As New EveHQ.frmReportViewer
                Call EveHQ.Core.Reports.GenerateCharSummary()
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\PilotSummary.html")
                Call DisplayReport(newReport, "Pilot Summary")
            Case "Skill Training"
                If EveHQ.Core.HQ.myPilot.Name <> "" And EveHQ.Core.HQ.myPilot.PilotSkills.Count <> 0 Then
                    ' Open the skill training form
                    Call OpenSkillTrainingForm()
                End If
            Case Else
                ' Show the pilot summary report form!
                Dim newReport As New EveHQ.frmReportViewer
                Call EveHQ.Core.Reports.GenerateCharSummary()
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\PilotSummary.html")
                Call DisplayReport(newReport, "Pilot Summary")
        End Select

        ' Set the tab style
        Select Case EveHQ.Core.HQ.EveHQSettings.MDITabStyle
            Case 0
                Me.tabMDI.Appearance = TabAppearance.FlatButtons
            Case 1
                Me.tabMDI.Appearance = TabAppearance.Buttons
            Case 2
                Me.tabMDI.Appearance = TabAppearance.Normal
        End Select

        ' Close the splash screen
        frmSplash.Close()

        ' Check if the form needs to be minimised on startup
        If EveHQ.Core.HQ.EveHQSettings.AutoMinimise = True Then
            Me.WindowState = FormWindowState.Minimized
        Else
            Select Case EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4)
                Case FormWindowState.Normal
                    Me.Show()
                    Me.Left = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(0)
                    Me.Top = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(1)
                    Me.Width = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(2)
                    Me.Height = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(3)
                    Me.WindowState = FormWindowState.Normal
                Case FormWindowState.Maximized
                    Me.WindowState = FormWindowState.Maximized
                    Me.Show()
            End Select
        End If

        ' Show the training overlay if required
        If EveHQ.Core.HQ.EveHQSettings.OverlayStartup = True Then
            Call Me.ShowTrainingOverlay()
        End If

        ' Start the timers
        If EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = True Then
            tmrEve.Enabled = True
        End If
        tmrSkillUpdate.Enabled = True
        tmrModules.Enabled = True

    End Sub
    Private Sub frmEveHQ_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If HideWhenMinimisedToolStripMenuItem.Checked = True Then
            If Me.WindowState = FormWindowState.Minimized Then
                Me.Hide()
            Else
                If Me.ShowInTaskbar = False Then
                    Me.ShowInTaskbar = True
                End If
            End If
        End If

        Select Case Me.WindowState
            Case FormWindowState.Normal
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(0) = Me.Left
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(1) = Me.Top
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(2) = Me.Width
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(3) = Me.Height
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4) = FormWindowState.Normal
            Case FormWindowState.Maximized
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4) = FormWindowState.Maximized
        End Select

    End Sub
    Private Sub frmEveHQ_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
        Select Case Me.WindowState
            Case FormWindowState.Normal
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(0) = Me.Left
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(1) = Me.Top
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(2) = Me.Width
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(3) = Me.Height
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4) = FormWindowState.Normal
            Case FormWindowState.Maximized
                EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4) = FormWindowState.Maximized
        End Select
    End Sub
    Private Sub EveStatusIcon_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles EveStatusIcon.DoubleClick
        ' Restores the window
        Me.TopMost = True
        Me.Show()
        Me.BringToFront()
        Select Case EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4)
            Case FormWindowState.Maximized
                Me.WindowState = FormWindowState.Maximized
            Case FormWindowState.Normal
                Me.Left = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(0)
                Me.Top = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(1)
                Me.Width = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(2)
                Me.Height = EveHQ.Core.HQ.EveHQSettings.MainFormPosition(3)
                Me.WindowState = FormWindowState.Normal
        End Select
        Me.TopMost = False
    End Sub
    Public Sub ShutdownRoutine()
        Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()

        ' Check if Shutdown Notification is active
        If EveHQ.Core.HQ.EveHQSettings.ShutdownNotify = True Then
            Dim accounts As New ArrayList
            Dim strNotify As String = ""
            Dim strCharNotify As String = ""
            For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If cPilot.Training = True Then
                    Dim timeRemaining As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
                    If timeRemaining <= EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod * 60 * 60 Then
                        strCharNotify &= cPilot.Name & " - " & cPilot.TrainingSkillName & " (" & EveHQ.Core.SkillFunctions.TimeToString(timeRemaining) & ")" & ControlChars.CrLf
                    End If
                    accounts.Add(cPilot.Account)
                End If
            Next
            If strCharNotify <> "" Then
                strCharNotify = "The following pilots have skills due to end within " & EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod & " hours:" & ControlChars.CrLf & ControlChars.CrLf & strCharNotify
                strNotify &= strCharNotify
            End If
            ' Check each account to see if something is training.
            Dim strAccountNotify As String = ""
            For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                If accounts.Contains(cAccount.userID) = False Then
                    strAccountNotify &= cAccount.userID & ControlChars.CrLf
                End If
            Next
            If strAccountNotify <> "" Then
                strAccountNotify = ControlChars.CrLf & "The following accounts do not appear to have any skill training:" & ControlChars.CrLf & ControlChars.CrLf & strAccountNotify
                strNotify &= strAccountNotify
            End If
            If strNotify <> "" Then
                MessageBox.Show(strNotify, "EveHQ Skill Notification", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

        ' Close the training overlay form if it is still open
        frmTrainingInfo.Close()

        ' Close the tabs if they are open, forcing the correct closure of each plug-in and form
        For Each tp As TabPage In tabMDI.TabPages
            TryCast(tp.Tag, Form).Close()
        Next

        ' Remove the icons
        EveStatusIcon.Visible = False : iconEveHQMLW.Visible = False
        EveStatusIcon.Icon = Nothing : iconEveHQMLW.Icon = Nothing
        EveStatusIcon.Dispose() : iconEveHQMLW.Dispose()
        End

    End Sub

#End Region

#Region "Skill Display Updater Routines"

    Private Sub SkillWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles SkillWorker.DoWork

        EveHQ.Core.HQ.myPilot.TrainingCurrentSP = CInt(EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(EveHQ.Core.HQ.myPilot))
        EveHQ.Core.HQ.myPilot.TrainingCurrentTime = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(EveHQ.Core.HQ.myPilot)

    End Sub
    Private Sub SkillWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles SkillWorker.RunWorkerCompleted

        Call UpdateTrainingStatus()

        Call CheckNotifications()
        If EveHQ.Core.HQ.EveHQSettings.ContinueTraining = True Then
            Call UpdateToNextLevel()
        End If

        If frmPilot.IsHandleCreated = True Then
            Call frmPilot.UpdateSkillInfo()
        End If
        If frmTraining.IsHandleCreated = True Then
            Call frmTraining.UpdateTraining()
        End If
        If frmSkillDetails.IsHandleCreated = True Then
            Call frmSkillDetails.UpdateSkillDetails()
        End If
        If frmSettings.IsHandleCreated = True Then
            Call frmSettings.UpdateTimeOffset()
        End If
        If frmTrainingInfo.IsHandleCreated = True Then
            Call frmTrainingInfo.UpdateTraining()
        End If
        If frmToolTrayIconPopup.IsHandleCreated = True Then
            Call frmToolTrayIconPopup.UpdateSkillTimes()
        End If
        ' Update the G15 LCD if applicable
        If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True And EveHQ.Core.HQ.IsG15LCDActive = True Then
            Select Case EveHQ.Core.HQ.lcdCharMode
                Case 0
                    Call EveHQ.Core.HQ.EveHQLCD.DrawSkillTrainingInfo(EveHQ.Core.HQ.lcdPilot)
                Case 1
                    Call EveHQ.Core.HQ.EveHQLCD.DrawCharacterInfo(EveHQ.Core.HQ.lcdPilot)
            End Select
        End If
        ' Check for an API update if applicable
        If EveHQ.Core.HQ.EveHQSettings.AutoAPI = True Then
            If EveHQ.Core.HQ.LastAutoAPITime.AddSeconds(EveHQ.Core.HQ.AutoAPITimeSpan) < Now Then
                Dim updateRequired As Boolean = False
                For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                    If cPilot.Name <> "" And cPilot.Account <> "" Then
                        Dim cacheCDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.CacheExpirationTime)
                        Dim cacheTDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingExpirationTime)
                        If cacheCDate < Now Or cacheTDate < Now Then
                            updateRequired = True
                            Exit For
                        End If
                    End If
                Next
                If updateRequired = True Then
                    ' Invoke the API Caller
                    Call QueryMyEveServer()
                    EveHQ.Core.HQ.LastAutoAPITime = Now
                End If
            End If
            ' Display time until autoAPI download
            Dim TimeLeft As TimeSpan = EveHQ.Core.HQ.LastAutoAPITime.AddSeconds(EveHQ.Core.HQ.AutoAPITimeSpan) - Now
            tsAPITime.Text = EveHQ.Core.SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
        Else
            tsAPITime.Text = ""
        End If
    End Sub
    Private Sub tmrSkillUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSkillUpdate.Tick
        If SkillWorker.IsBusy = False Then
            SkillWorker.RunWorkerAsync()
            tmrSkillUpdate.Interval = 1000
        End If
    End Sub
    Private Sub UpdateTrainingStatus()
        Dim accounts As New ArrayList
        lblTrainingStatus.Text = ""
        Dim notifyText As String = ""
        ' Check each pilot
        Dim pilotTimes As New SortedList
        Dim defer As Integer = 0
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Training = True Then
                If cPilot.Active = True Then
                    defer += 1
                    If pilotTimes.ContainsKey(Format(EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot), "00000000")) = True Then
                        pilotTimes.Add(Format(EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot) + defer, "00000000"), cPilot)
                    Else
                        pilotTimes.Add(Format(EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot), "00000000"), cPilot)
                    End If
                End If
                accounts.Add(cPilot.Account)
            End If
        Next
        For Each cPilot As EveHQ.Core.Pilot In pilotTimes.Values
            lblTrainingStatus.Text &= cPilot.Name & ":" & ControlChars.CrLf
            lblTrainingStatus.Text &= cPilot.TrainingSkillName
            lblTrainingStatus.Text &= " (Lvl " & cPilot.TrainingSkillLevel & ")" & ControlChars.CrLf
            Dim trainingTime As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
            Dim skillpoints As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(cPilot)
            lblTrainingStatus.Text &= EveHQ.Core.SkillFunctions.TimeToString(trainingTime) & ControlChars.CrLf & ControlChars.CrLf
        Next
        ' Check each account to see if something is training.
        For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            If accounts.Contains(cAccount.userID) = False Then
                lblTrainingStatus.Text &= "Account '" & cAccount.FriendlyName & "' is not training!" & ControlChars.CrLf & ControlChars.CrLf
            End If
        Next
        ' Check for label size and adjust if need be
        If XPTraining.IsExpanded = True Then
            If XPTraining.Height <> lblTrainingStatus.Height + 40 Then
                XPTraining.Height = lblTrainingStatus.Height + 40
                XPPilots.Top = XPTraining.Top + XPTraining.Height + 10
            End If
        End If
    End Sub
    Private Sub CheckNotifications()

        ' Only do this if at least one notification is enabled
        If EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = True Or EveHQ.Core.HQ.EveHQSettings.NotifyDialog = True Or EveHQ.Core.HQ.EveHQSettings.NotifyEMail = True Or EveHQ.Core.HQ.EveHQSettings.NotifySound = True Then

            Dim NeedToNotifyNow As Boolean = False
            Dim NeedToNotifyEarly As Boolean = False
            Dim notifyText As String = ""

            For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If cPilot.Training = True Then
                    Dim trainingTime As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
                    ' See if we need to notify about this pilot
                    If trainingTime <= EveHQ.Core.HQ.EveHQSettings.NotifyOffset Then
                        If cPilot.TrainingNotifiedEarly = False Then
                            If cPilot.TrainingCurrentTime <= 0 And cPilot.TrainingNotifiedNow = False Then
                                If EveHQ.Core.HQ.EveHQSettings.NotifyNow = True Then
                                    notifyText &= cPilot.Name & " has completed training of " & cPilot.TrainingSkillName & " to Level " & cPilot.TrainingSkillLevel & "." & ControlChars.CrLf
                                    cPilot.TrainingNotifiedEarly = True : cPilot.TrainingNotifiedNow = True
                                End If
                            Else
                                If EveHQ.Core.HQ.EveHQSettings.NotifyEarly = True Then
                                    Dim strTime As String = EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime)
                                    strTime = strTime.Replace("s", " seconds").Replace("m", " minutes")
                                    notifyText &= cPilot.Name & " has approximately " & strTime & " before training of " & cPilot.TrainingSkillName & " to Level " & cPilot.TrainingSkillLevel & " completes." & ControlChars.CrLf
                                    cPilot.TrainingNotifiedEarly = True : cPilot.TrainingNotifiedNow = False
                                End If
                            End If
                        Else
                            If cPilot.TrainingCurrentTime <= 0 And cPilot.TrainingNotifiedNow = False Then
                                If EveHQ.Core.HQ.EveHQSettings.NotifyNow = True Then
                                    notifyText &= cPilot.Name & " has completed training of " & cPilot.TrainingSkillName & " to Level " & cPilot.TrainingSkillLevel & "." & ControlChars.CrLf
                                    cPilot.TrainingNotifiedEarly = True : cPilot.TrainingNotifiedNow = True
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            If notifyText <> "" Then
                ' If sound is required: Play first as this is automatically put on a separate thread
                If EveHQ.Core.HQ.EveHQSettings.NotifySound = True Then
                    Try
                        My.Computer.Audio.Play(EveHQ.Core.HQ.EveHQSettings.NotifySoundFile, AudioPlayMode.Background)
                    Catch ex As Exception
                    End Try
                End If
                ' If tooltip is required:
                If EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = True Then
                    EveStatusIcon.ShowBalloonTip(3000, "Training Notification", notifyText, ToolTipIcon.Info)
                End If
                ' If dialog box is required:
                If EveHQ.Core.HQ.EveHQSettings.NotifyDialog = True Then
                    MessageBox.Show(notifyText, "Training Notification", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                ' If email is required:
                If EveHQ.Core.HQ.EveHQSettings.NotifyEMail = True Then
                    Call SendEveHQMail(notifyText)
                End If
            End If
        End If

    End Sub
    Private Sub SendEveHQMail(ByVal mailText As String)
        Dim eveHQMail As New System.Net.Mail.SmtpClient
        Try
            eveHQMail.Host = EveHQ.Core.HQ.EveHQSettings.EMailServer
            eveHQMail.Port = EveHQ.Core.HQ.EveHQSettings.EMailPort
            If EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = True Then
                Dim newCredentials As New System.Net.NetworkCredential
                newCredentials.UserName = EveHQ.Core.HQ.EveHQSettings.EMailUsername
                newCredentials.Password = EveHQ.Core.HQ.EveHQSettings.EMailPassword
                eveHQMail.Credentials = newCredentials
            End If
            Dim eveHQMsg As New System.Net.Mail.MailMessage("notifications@evehq.net", EveHQ.Core.HQ.EveHQSettings.EMailAddress)
            eveHQMsg.Subject = "Eve Skill Training Notification"
            eveHQMsg.Body = mailText
            eveHQMail.Send(eveHQMsg)
        Catch ex As Exception
            MessageBox.Show("The mail notification sending process failed. Please check that the server, port, address, username and password are correct." & ControlChars.CrLf & ControlChars.CrLf & "The error was: " & ex.Message, "EveHQ Test Email Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Public Sub UpdateToNextLevel()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Training = True Then
                If cPilot.PilotSkills.Contains(cPilot.TrainingSkillName) = True Then
                    Dim trainSkill As EveHQ.Core.Skills = CType(cPilot.PilotSkills(cPilot.TrainingSkillName), Core.Skills)
                    Dim trainingTime As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
                    ' See if we need to "update" this level
                    If trainingTime <= 0 And cPilot.TrainingSkillLevel <> trainSkill.Level Then
                        Dim strXML As String = ""

                        ' Browse the skill queue and pick the next available skill
                        Dim pq As EveHQ.Core.SkillQueue = CType(cPilot.TrainingQueues(cPilot.PrimaryQueue), Core.SkillQueue)
                        If pq IsNot Nothing Then
                            Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(cPilot, pq)
                            Dim qItem As EveHQ.Core.SortedQueue = New EveHQ.Core.SortedQueue
                            For Each qItem In arrQueue
                                If qItem.Done = False Then
                                    If qItem.IsTraining = False Then
                                        ' Update the skill and move on
                                        If EveHQ.Core.SkillFunctions.ForceSkillTraining(cPilot, qItem.ID, True) = True Then
                                            Call frmPilot.UpdatePilotInfo()
                                            Call frmTraining.LoadSkillTree()
                                        End If
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If
            End If
        Next
    End Sub

#End Region

#Region "Panel Colour Routines"
    Public Sub UpdatePanelColours()
        ' Update Panel Background
        XPanderList1.BackColorDark = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor))
        XPanderList1.BackColorLight = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor))
        ' Update XPModules
        XPModules.PaneOutlineColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor))
        XPModules.PaneBottomRightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor))
        XPModules.PaneTopLeftColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor))
        XPModules.CaptionLeftColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelLeftColor))
        XPModules.CaptionRightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelRightColor))
        XPModules.CaptionTextColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTextColor))
        XPModules.CaptionTextHighlightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor))
        ' Update XPTraining
        XPTraining.PaneOutlineColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor))
        XPTraining.PaneBottomRightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor))
        XPTraining.PaneTopLeftColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor))
        XPTraining.CaptionLeftColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelLeftColor))
        XPTraining.CaptionRightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelRightColor))
        XPTraining.CaptionTextColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTextColor))
        XPTraining.CaptionTextHighlightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor))
        ' Update XPPilots
        XPPilots.PaneOutlineColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor))
        XPPilots.PaneBottomRightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor))
        XPPilots.PaneTopLeftColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor))
        XPPilots.CaptionLeftColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelLeftColor))
        XPPilots.CaptionRightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelRightColor))
        XPPilots.CaptionTextColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTextColor))
        XPPilots.CaptionTextHighlightColor = Drawing.Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor))
    End Sub
#End Region

    Private Sub RemoteUpdate()
        Me.Invoke(New QueryMyEveServerDelegate(AddressOf QueryMyEveServer))
    End Sub

    Public Sub QueryMyEveServer()

        Dim curSelPilot As String = ""
        ' If a pilot is selected, make a note of it for later viewing
        If cboPilots.SelectedIndex <> -1 Then
            curSelPilot = cboPilots.SelectedItem.ToString
        End If

        ' If we have accounts to query then get the data for them
        If EveHQ.Core.HQ.EveHQSettings.Accounts.Count = 0 Then
            tsAPIStatus.Text = "API Status: No accounts entered into settings!! (" & Now.ToString & ")"
            Exit Sub
        Else
            tsAPIStatus.Text = "API Status: Fetching Character Data..."
            EveHQStatusStrip.Refresh()
            Me.Cursor = Cursors.WaitCursor
            ' Clear the current list of pilots
            EveHQ.Core.HQ.TPilots.Clear()
            EveHQ.Core.HQ.APIResults.Clear()
            ' get the details for the account
            Dim CurrentAccount As New EveHQ.Core.EveAccount
            For Each CurrentAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                tsAPIStatus.Text = "API Status: Updating Account '" & CurrentAccount.FriendlyName & "' (ID=" & CurrentAccount.userID & ")..."
                EveHQStatusStrip.Refresh()
                EveHQ.Core.PilotParseFunctions.GetCharactersInAccount(CurrentAccount)
            Next
            Call EveHQ.Core.PilotParseFunctions.CopyTempPilotsToMain()
            Me.Cursor = Cursors.Default
        End If

        ' Determine API responses and display appropriate message
        Dim AllCached As Boolean = True
        Dim ContainsNew As Boolean = False
        Dim ContainsErrors As Boolean = False
        For Each result As Integer In EveHQ.Core.HQ.APIResults.Values
            If result = 0 Then ContainsNew = True
            If result <> 1 Then AllCached = False
            Select Case result
                Case 2, 3, 4, 5, 6, 8, 9
                    ContainsErrors = True
                Case Is < 0
                    ContainsErrors = True
            End Select
        Next

        ' Display the results
        If ContainsErrors = True Then
            tsAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (Errors occured - double-click for details)"
        Else
            If AllCached = True Then
                tsAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (No new updates)"
            Else
                tsAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (Update successful)"
            End If
        End If

        ' Update if we have retrieved new data
        If ContainsNew = True Then
            Call UpdatePilotInfo()
        End If

        ' Show details of pilot if previously selected
        If cboPilots.Items.Contains(curSelPilot) Then
            cboPilots.SelectedItem = curSelPilot
        End If

    End Sub

    Private Sub RetreiveCharacterInfo(ByVal state As Object)

    End Sub

    Public Sub UpdatePilotInfo(Optional ByVal startUp As Boolean = False)
        ' Creates a list of all available pilots and enters it into the pilot selection area
        ' Note the name of the current pilot displayed
        Dim oldPilot As String = ""
        If cboPilots.Items.Count > 0 Then
            If cboPilots.SelectedItem IsNot Nothing Then
                oldPilot = cboPilots.SelectedItem.ToString()
            End If
        End If
        Dim currentPilot As New EveHQ.Core.Pilot
        Dim allPilots As SortedList = New SortedList

        ' Make a list of all the current pilots in the list
        Dim cList As New ArrayList
        For Each pilot As String In cboPilots.Items
            cList.Add(pilot)
        Next
        cboPilots.BeginUpdate()
        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If currentPilot.Active = True Then
                allPilots.Add(currentPilot.Name, currentPilot.Name)
                If cboPilots.Items.Contains(currentPilot.Name) = False Then
                    Me.cboPilots.Items.Add(currentPilot.Name)
                End If
                If cList.Contains(currentPilot.Name) = True Then
                    cList.Remove(currentPilot.Name)
                End If
            End If
        Next
        ' Get rid of pilots still in the list that we don't need
        For Each pilot As String In cList
            If cboPilots.Items.Contains(pilot) = True Then
                cboPilots.Items.Remove(pilot)
            End If
        Next
        cboPilots.EndUpdate()

        Dim pilotCount As Integer = 0
        XPPilots.Controls.Clear()
        For Each pilotName As String In allPilots.Values
            ' Add a label to the active pilots list in the task bar
            pilotCount += 1
            Dim newPilot As New LinkLabel
            AddHandler newPilot.Click, AddressOf Me.XPPilotLabels_Click
            newPilot.Name = "lblPilot" & pilotName
            newPilot.Text = pilotName
            newPilot.TextAlign = ContentAlignment.MiddleLeft
            newPilot.ImageAlign = ContentAlignment.MiddleLeft
            newPilot.Left = 8
            newPilot.Top = 20 + (20 * pilotCount)
            newPilot.Height = 13
            XPPilots.Controls.Add(newPilot)
        Next
        XPPilots.Height = 50 + (20 * pilotCount)

        ' Restore the pilot if it still in the list
        If oldPilot <> Nothing Then
            If cboPilots.Items.Contains(oldPilot) Then
                cboPilots.SelectedItem = oldPilot
            Else
                If cboPilots.Items.Count > 0 Then
                    cboPilots.SelectedIndex = 0
                End If
            End If
        Else
            If cboPilots.Items.Count > 0 Then
                If cboPilots.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = True Then
                    cboPilots.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
                Else
                    cboPilots.SelectedIndex = 0
                End If
            End If
        End If

        If cboPilots.Items.Count = 0 Then
            EveHQ.Core.HQ.myPilot = New EveHQ.Core.Pilot
            tsbPilotInfo.Enabled = False
            tsbSkillTraining.Enabled = False
            If frmPilot IsNot Nothing Then
                If frmPilot.IsHandleCreated = True Then
                    frmPilot.Close()
                End If
            End If
            If frmTraining IsNot Nothing Then
                If frmTraining.IsHandleCreated = True Then
                    frmTraining.Close()
                End If
            End If
        Else
            tsbPilotInfo.Enabled = True
            tsbSkillTraining.Enabled = True
            If startUp = False Then
                EveHQ.Core.HQ.myPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(EveHQ.Core.HQ.myPilot.Name), Core.Pilot)
                Call frmPilot.UpdatePilotInfo()
            End If
        End If

        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count = 0 Then
            mnuReportsHTMLChar.Enabled = False
        Else
            mnuReportsHTMLChar.Enabled = True
        End If

        ' Redraw Reports menu with new pilots and queues options?
        Call Me.DrawReportsMenu(allPilots)

    End Sub

    Private Sub XPPilotLabels_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clickedLabel As Label = CType(sender, Label)
        Me.cboPilots.SelectedItem = clickedLabel.Text
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        Dim curPilot As String = cboPilots.SelectedItem.ToString
        EveHQ.Core.HQ.myPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(curPilot), Core.Pilot)
        ' Update the info on the pilot form
        Call frmPilot.UpdatePilotInfo()
        ' Check if there is anything we need to disable
        If EveHQ.Core.HQ.myPilot.Updated = False Then
            mnuReportsHTMLChar.Enabled = False
        Else
            mnuReportsHTMLChar.Enabled = True
        End If
        If EveHQ.Core.HQ.myPilot.PilotSkills.Count = 0 Then
            Me.PilotInfoToolStripMenuItem.Enabled = False
            Me.SkillTrainingToolStripMenuItem.Enabled = False
            Me.tsbPilotInfo.Enabled = False
            Me.tsbSkillTraining.Enabled = False
            If frmTraining.IsHandleCreated = True Then
                frmTraining.Close()
            End If
            If frmPilot.IsHandleCreated = True Then
                frmPilot.Close()
            End If
        Else
            Me.PilotInfoToolStripMenuItem.Enabled = True
            Me.SkillTrainingToolStripMenuItem.Enabled = True
            Me.tsbPilotInfo.Enabled = True
            Me.tsbSkillTraining.Enabled = True
        End If
        ' See if the Neural Remapping form is open
        If frmNeuralRemap.IsHandleCreated = True Then
            frmNeuralRemap.PilotName = curPilot
        End If
        ' See if the Implants form is open
        If frmImplants.IsHandleCreated = True Then
            frmImplants.PilotName = curPilot
        End If
    End Sub

    Private Sub mnuReportOpenfolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportOpenfolder.Click
        Try
            Process.Start(EveHQ.Core.HQ.reportFolder)
        Catch ex As Exception
            MessageBox.Show("Unable to start Windows Explorer: " & ex.Message, "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

#Region "Reports"

    Private Sub DrawReportsMenu(ByVal allPilots As SortedList)

        For menu As Integer = 0 To 3
            Dim currentMenu As New ToolStripMenuItem
            Select Case menu
                Case 0
                    currentMenu = mnuReportsHTMLChar
                Case 1
                    currentMenu = mnuReportsXMLChar
                Case 2
                    currentMenu = mnuReportsTextChar
                Case 3
                    currentMenu = mnuReportsChartsChar
            End Select
            For Each reportMenu As ToolStripMenuItem In currentMenu.DropDownItems
                reportMenu.DropDownItems.Clear()
                For Each curPilot As String In allPilots.Values
                    Dim currentPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(curPilot), Core.Pilot)
                    If currentPilot.Active = True Then
                        Dim pilotMenu As New ToolStripMenuItem
                        pilotMenu.Name = reportMenu.Name & "_" & currentPilot.Name
                        pilotMenu.Text = currentPilot.Name
                        If reportMenu.Name <> "mnuReportTrainingQueue" And reportMenu.Name <> "mnuReportQueueShoppingList" And reportMenu.Name <> "mnuReportsTextTrainingQueue" And reportMenu.Name <> "mnuReportsTextShoppingList" Then
                            AddHandler pilotMenu.Click, AddressOf Me.ReportsMenuHandler
                        End If
                        reportMenu.DropDownItems.Add(pilotMenu)
                        If reportMenu.Name = "mnuReportTrainingQueue" Or reportMenu.Name = "mnuReportQueueShoppingList" Or reportMenu.Name = "mnuReportsTextTrainingQueue" Or reportMenu.Name = "mnuReportsTextShoppingList" Then
                            For Each qItem As EveHQ.Core.SkillQueue In currentPilot.TrainingQueues.Values
                                Dim queueMenu As New ToolStripMenuItem
                                queueMenu.Text = qItem.Name
                                queueMenu.Name = pilotMenu.Name & "_" & qItem.Name
                                AddHandler queueMenu.Click, AddressOf Me.ReportsMenuHandler
                                pilotMenu.DropDownItems.Add(queueMenu)
                            Next
                        End If
                    End If
                Next
            Next
        Next

    End Sub
    Private Sub ReportsMenuHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim reportMenu As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim menuParts() As String = reportMenu.Name.Split("_".ToCharArray)
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(menuParts(1)), Core.Pilot)
        Dim newReport As New frmReportViewer
        Select Case menuParts(0)
            Case "mnuReportsCharCharsheet"
                Call EveHQ.Core.Reports.GenerateCharSheet(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\CharSheet (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
            Case "mnuReportCharTraintimes"
                Call EveHQ.Core.Reports.GenerateTrainingTime(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TrainTime (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Training Times - " & rPilot.Name)
            Case "mnuReportTimeToLevel5"
                Call EveHQ.Core.Reports.GenerateTimeToLevel5(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TimeToLevel5 (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
            Case "mnureportCharSkillLevels"
                Call EveHQ.Core.Reports.GenerateSkillLevels(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\SkillLevels (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
            Case "mnuReportCharXML"
                Call EveHQ.Core.Reports.GenerateCharXML(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\CharXML (" & rPilot.Name & ").xml")
                DisplayReport(newReport, "Imported Character XML - " & rPilot.Name)
            Case "mnuReportTrainXML"
                Call EveHQ.Core.Reports.GenerateTrainXML(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TrainingXML (" & rPilot.Name & ").xml")
                DisplayReport(newReport, "Imported Training XML - " & rPilot.Name)
            Case "mnuReportTrainingQueue"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportQueueShoppingList"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportSkillsAvailable"
                Call EveHQ.Core.Reports.GenerateSkillsAvailable(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\SkillsToTrain (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
            Case "mnuReportSkillsNotTrained"
                Call EveHQ.Core.Reports.GenerateSkillsNotTrained(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\SkillsNotTrained (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
            Case "mnuReportCurrentCharXMLOld"
                Call EveHQ.Core.Reports.GenerateCurrentPilotXML_Old(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\CurrentXML - Old (" & rPilot.Name & ").xml")
                DisplayReport(newReport, "Old Style Character XML - " & rPilot.Name)
            Case "mnuReportCurrentCharXMLNew"
                Call EveHQ.Core.Reports.GenerateCurrentPilotXML_New(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\CurrentXML - New (" & rPilot.Name & ").xml")
                DisplayReport(newReport, "Current Character XML - " & rPilot.Name)
            Case "mnuReportCurrentTrainingXMLOld"
                Call EveHQ.Core.Reports.GenerateCurrentTrainingXML_Old(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TrainingXML - Old (" & rPilot.Name & ").xml")
                DisplayReport(newReport, "Old Style Training XML - " & rPilot.Name)
            Case "mnuReportSkillGroupChart"
                Dim newChartForm As New frmChartViewer
                newChartForm.Controls.Add(EveHQ.Core.Reports.SkillGroupChart(rPilot))
                Call Me.DisplayChartReport(newChartForm, "Skill Group Chart - " & rPilot.Name)
            Case "mnuReportsTextCharSheet"
                Call EveHQ.Core.Reports.GenerateTextCharSheet(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\CharSheet (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
            Case "mnuReportsTextTrainTimes"
                Call EveHQ.Core.Reports.GenerateTextTrainingTime(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TrainTime (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Training Times - " & rPilot.Name)
            Case "mnuReportsTextTimeToLevel5"
                Call EveHQ.Core.Reports.GenerateTextTimeToLevel5(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TimeToLevel5 (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
            Case "mnuReportsTextSkillLevels"
                Call EveHQ.Core.Reports.GenerateTextSkillLevels(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\SkillLevels (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
            Case "mnuReportsTextSkillsAvailable"
                Call EveHQ.Core.Reports.GenerateTextSkillsAvailable(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\SkillsToTrain (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
            Case "mnuReportsTextSkillsNotTrained"
                Call EveHQ.Core.Reports.GenerateTextSkillsNotTrained(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\SkillsNotTrained (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
            Case "mnuReportsTextTrainingQueue"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTextTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportsTextShoppingList"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTextShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportPartiallyTrainedSkills"
                Call EveHQ.Core.Reports.GeneratePartialSkills(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\PartialSkills (" & rPilot.Name & ").html")
                DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
            Case "mnuReportsTextPartiallyTrainedSkills"
                Call EveHQ.Core.Reports.GenerateTextPartialSkills(rPilot)
                newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\PartialSkills (" & rPilot.Name & ").txt")
                DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
        End Select

    End Sub

    Private Sub mnuRepCharSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepCharSummary.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCharSummary()
        newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\PilotSummary.html")
        DisplayReport(newReport, "Pilot Summary")
    End Sub

    Private Sub mnuReportSPSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportSPSummary.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateSPSummary()
        newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\SPSummary.html")
        DisplayReport(newReport, "Skill Point Summary")
    End Sub

    Private Sub mnuReportAsteroidAlloys_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportAsteroidAlloys.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateAlloyReport()
        newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\AlloyReport.html")
        DisplayReport(newReport, "Alloy Composition")
    End Sub

    Private Sub mnuReportAsteroidRocks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportAsteroidRocks.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateRockReport()
        newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\OreReport.html")
        DisplayReport(newReport, "Asteroid Composition")
    End Sub

    Private Sub mnuReportAsteroidIce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportAsteroidIce.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateIceReport()
        newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\IceReport.html")
        DisplayReport(newReport, "Ice Composition")
    End Sub

#End Region

    Private Sub EveIconMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles EveIconMenu.Opening
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.EveFolder(1) & "\Eve.exe") = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(1) = False Then
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(1) <> "" Then
                ctxmnuLaunchEve1.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(1) & ")"
            End If
            ctxmnuLaunchEve1.Enabled = True
            ctxLaunchEve1Full.Visible = True
            ctxLaunchEve1Normal.Visible = True
        Else
            ctxmnuLaunchEve1.Enabled = False
            ctxLaunchEve1Full.Visible = False
            ctxLaunchEve1Normal.Visible = False
        End If
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.EveFolder(2) & "\Eve.exe") = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(2) = False Then
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(2) <> "" Then
                ctxmnuLaunchEve2.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(2) & ")"
            End If
            ctxmnuLaunchEve2.Enabled = True
            ctxLaunchEve2Full.Visible = True
            ctxLaunchEve2Normal.Visible = True
        Else
            ctxmnuLaunchEve2.Enabled = False
            ctxLaunchEve2Full.Visible = False
            ctxLaunchEve2Normal.Visible = False
        End If
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.EveFolder(3) & "\Eve.exe") = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(3) = False Then
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(3) <> "" Then
                ctxmnuLaunchEve3.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(3) & ")"
            End If
            ctxmnuLaunchEve3.Enabled = True
            ctxLaunchEve3Full.Visible = True
            ctxLaunchEve3Normal.Visible = True
        Else
            ctxmnuLaunchEve3.Enabled = False
            ctxLaunchEve3Full.Visible = False
            ctxLaunchEve3Normal.Visible = False
        End If
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.EveFolder(4) & "\Eve.exe") = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(4) = False Then
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(4) <> "" Then
                ctxmnuLaunchEve4.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(4) & ")"
            End If
            ctxmnuLaunchEve4.Enabled = True
            ctxLaunchEve4Full.Visible = True
            ctxLaunchEve4Normal.Visible = True
        Else
            ctxmnuLaunchEve4.Enabled = False
            ctxLaunchEve4Full.Visible = False
            ctxLaunchEve4Normal.Visible = False
        End If
    End Sub

    Private Sub mnuBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBackup.Click, tsbSettingsBackup.Click
        Call Me.OpenBackUpForm()
    End Sub

    Private Sub mnuToolsAPIChecker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsAPIChecker.Click
        Call Me.OpenAPICheckerForm()
    End Sub

    Private Sub mnuToolsMarketPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsMarketPrices.Click
        Call Me.OpenMarketPricesForm()
    End Sub

    Private Sub mnuInEveUploader_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuInEveUploader.Click
        Call Me.OpenInEveUploaderForm()
    End Sub

#Region "Backup Worker routines"

    Private Sub tmrBackup_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrBackup.Tick
        If BackupWorker.IsBusy = False Then
            If EveHQ.Core.HQ.EveHQSettings.BackupAuto = True Then
                Dim nextBackup As Date = EveHQ.Core.HQ.EveHQSettings.BackupStart
                If EveHQ.Core.HQ.EveHQSettings.BackupLast > nextBackup Then
                    nextBackup = EveHQ.Core.HQ.EveHQSettings.BackupLast
                End If
                nextBackup = DateAdd(DateInterval.Day, EveHQ.Core.HQ.EveHQSettings.BackupFreq, nextBackup)
                If Now >= nextBackup Then
                    BackupWorker.RunWorkerAsync()
                End If
            End If
        End If
    End Sub
    Private Sub BackupWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackupWorker.DoWork
        Call frmBackup.BackupEveSettings()
    End Sub
    Private Sub BackupWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackupWorker.RunWorkerCompleted

        If EveHQ.Core.HQ.EveHQSettings.BackupLastResult = -1 Then
            frmBackup.lblLastBackup.Text = Format(EveHQ.Core.HQ.EveHQSettings.BackupLast, "dd/MM/yyyy HH:mm")
        End If
        Call frmBackup.CalcNextBackup()
        Call frmBackup.ScanBackups()
        If EveHQ.Core.HQ.EveHQSettings.BackupLastResult = -1 Then
            tsAPIStatus.Text = "Settings Backup Successful: " & Format(EveHQ.Core.HQ.EveHQSettings.BackupLast, "dd/MM/yyyy HH:mm")
        Else
            tsAPIStatus.Text = "Settings Backup Aborted - No Source Folders"
        End If
    End Sub

#End Region

    Private Sub mnuHelpAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpAbout.Click, tsbAbout.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub mnuHelpCheckUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpCheckUpdates.Click, tsbCheckUpdates.Click
        Dim myUpdater As New frmUpdater
        myUpdater.ShowDialog()
    End Sub

    Private Sub VersionHistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VersionHistoryToolStripMenuItem.Click
        Dim sw As StreamWriter = New StreamWriter(EveHQ.Core.HQ.appDataFolder & "History.txt")
        sw.Write(My.Resources.History)
        sw.Flush()
        sw.Close()
        Dim newReport As New frmReportViewer
        newReport.wbReport.Navigate(EveHQ.Core.HQ.appDataFolder & "History.txt")
        DisplayReport(newReport, "EveHQ Version History")
    End Sub

    Private Sub IGBWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles IGBWorker.DoWork
        EveHQ.Core.HQ.myIGB.RunIGB(IGBWorker, e)
    End Sub

#Region "Background Module Loading"
    Private Sub tmrModules_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrModules.Tick
        frmEveHQ.CheckForIllegalCrossThreadCalls = False
        tmrModules.Enabled = False
        For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            If PlugInInfo.Available = True And PlugInInfo.Disabled = False Then
                'If PlugInInfo.Available = True Then
                If PlugInInfo.RunAtStartup = True Then
                    ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
                End If
            End If
        Next
        ' Check for existing pilots and accounts
        If EveHQ.Core.HQ.EveHQSettings.Accounts.Count = 0 And EveHQ.Core.HQ.EveHQSettings.Pilots.Count = 0 Then
            Dim wMsg As String = "EveHQ has detected that you have not yet setup any API accounts." & ControlChars.CrLf & ControlChars.CrLf
            wMsg &= "Would you like to do this now?"
            Dim reply As Integer = MessageBox.Show(wMsg, "Welcome to EveHQ!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                wMsg = "You can add API accounts using the 'Add API Account' button on the toolbar or by going into Settings and choosing the Eve Accounts section."
                MessageBox.Show(wMsg, "API Account Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim EveHQSettings As New frmSettings
                EveHQSettings.Tag = "nodeEveAccounts"
                EveHQSettings.ShowDialog()
                EveHQSettings.Dispose()
            End If
        End If
    End Sub
    Private Sub RunModuleStartUps(ByVal State As Object)
        Dim plugInInfo As EveHQ.Core.PlugIn = CType(State, EveHQ.Core.PlugIn)
        Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
        Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
        plugInInfo.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn = plugInInfo.Instance
        Dim modStatus As Label = CType(XPModules.Controls("lblMod" & plugInInfo.Name), Label)
        modStatus.Image = My.Resources.Status_yellow
        modStatus.ContextMenuStrip = Nothing
        plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Loading
        Try
            Dim PlugInResponse As String = ""
            PlugInResponse = runPlugIn.EveHQStartUp().ToString
            If CBool(PlugInResponse) = False Then
                modStatus.Image = My.Resources.Status_red
                modStatus.ContextMenuStrip = ctxPlugin
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Failed
            Else
                modStatus.Image = My.Resources.Status_green
                modStatus.ContextMenuStrip = Nothing
                Dim modMenu As ToolStripMenuItem = CType(mnuModules.DropDownItems(plugInInfo.Name), ToolStripMenuItem)
                If ToolStrip.Items.ContainsKey(plugInInfo.Name) = True Then
                    Dim tsbMenu As ToolStripButton = CType(ToolStrip.Items(plugInInfo.Name), ToolStripButton)
                    tsbMenu.Enabled = True
                End If
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
                modMenu.Enabled = True
            End If
        Catch ex As Exception
            MessageBox.Show("Unable to load plugin: " & plugInInfo.Name & ControlChars.CrLf & ex.Message, "Plugin error")
            modStatus.Image = My.Resources.Status_red
            modStatus.ContextMenuStrip = ctxPlugin
        End Try
    End Sub
    Private Sub ctxPlugin_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxPlugin.Opening
        ' Changes the menu to show the correct plug-in name
        Dim lbl As Label = CType(ctxPlugin.SourceControl, Label)
        mnuLoadPlugin.Tag = lbl.Tag
        mnuLoadPlugin.Text = "Load " & lbl.Text.Trim
    End Sub
    Private Sub mnuLoadPlugin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLoadPlugin.Click
        Dim PlugInInfo As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins.Item(mnuLoadPlugin.Tag), Core.PlugIn)
        If PlugInInfo.RunAtStartup = True Then
            ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
        Else
            Dim modStatus As Label = CType(ctxPlugin.SourceControl, Label)
            modStatus.Image = My.Resources.Status_green
            modStatus.ContextMenuStrip = Nothing
            Dim modMenu As ToolStripMenuItem = CType(mnuModules.DropDownItems(PlugInInfo.Name), ToolStripMenuItem)
            If ToolStrip.Items.ContainsKey(PlugInInfo.Name) = True Then
                Dim tsbMenu As ToolStripButton = CType(ToolStrip.Items(PlugInInfo.Name), ToolStripButton)
                tsbMenu.Enabled = True
            End If
            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
            modMenu.Enabled = True
        End If
    End Sub
#End Region

#Region "Plug-in Routines"
    Private Sub SetupModuleMenu()
        If EveHQ.Core.HQ.EveHQSettings.Plugins.Count <> 0 Then
            mnuModules.DropDownItems.Clear()
            Dim modCount As Integer = 0
            For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
                'If PlugInInfo.Available = True And PlugInInfo.Disabled = False Then
                If PlugInInfo.Available = True Then
                    modCount += 1
                    Dim newModuleMenu As ToolStripMenuItem = New ToolStripMenuItem(PlugInInfo.MainMenuText, Nothing, New System.EventHandler(AddressOf ModuleMenuItemClick), PlugInInfo.Name)
                    Dim newModuleStatus As New Label
                    Dim newTSBItem As ToolStripButton = New ToolStripButton(PlugInInfo.MainMenuText, PlugInInfo.MenuImage, New System.EventHandler(AddressOf ModuleTSBClick), PlugInInfo.Name)
                    newTSBItem.ToolTipText = PlugInInfo.MainMenuText
                    newTSBItem.DisplayStyle = ToolStripItemDisplayStyle.Image
                    newModuleMenu.Text = PlugInInfo.MainMenuText
                    newModuleMenu.ToolTipText = PlugInInfo.Description
                    newModuleMenu.Image = PlugInInfo.MenuImage
                    newModuleMenu.Tag = PlugInInfo.Name
                    If PlugInInfo.RunAtStartup = True Then
                        newModuleMenu.Enabled = False
                        newTSBItem.Enabled = False
                        newModuleStatus.Image = My.Resources.Status_red
                        newModuleStatus.ContextMenuStrip = ctxPlugin
                        PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                    Else
                        If PlugInInfo.Disabled = False Then
                            newModuleMenu.Enabled = True
                            newTSBItem.Enabled = True
                            newModuleStatus.Image = My.Resources.Status_green
                            newModuleStatus.ContextMenuStrip = Nothing
                            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
                        Else
                            newModuleMenu.Enabled = False
                            newTSBItem.Enabled = False
                            newModuleStatus.Image = My.Resources.Status_red
                            newModuleStatus.ContextMenuStrip = ctxPlugin
                            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                        End If
                    End If
                    mnuModules.DropDownItems.Add(newModuleMenu)
                    ' Only add if we have an image
                    If PlugInInfo.MenuImage IsNot Nothing Then
                        ToolStrip.Items.Add(newTSBItem)
                    End If
                    newModuleStatus.Name = "lblMod" & PlugInInfo.Name
                    newModuleStatus.Text = "    " & PlugInInfo.MainMenuText
                    newModuleStatus.Tag = PlugInInfo.Name
                    newModuleStatus.TextAlign = ContentAlignment.MiddleLeft
                    newModuleStatus.ImageAlign = ContentAlignment.MiddleLeft
                    newModuleStatus.Left = 8
                    newModuleStatus.Top = 20 + (20 * modCount)
                    newModuleStatus.Height = 13
                    newModuleStatus.Width = 125
                    XPModules.Controls.Add(newModuleStatus)
                    XPModules.Height = 50 + (20 * modCount)
                    XPTraining.Top = XPModules.Top + XPModules.Height + 10
                    XPPilots.Top = XPTraining.Top + XPTraining.Height + 10
                Else
                    ' Need anything here if the plug-in is disabled?
                End If
            Next
        End If
    End Sub
    Private Sub ModuleMenuItemClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        If tabMDI.TabPages.ContainsKey(mnu.Name) = True Then
            tabMDI.SelectTab(mnu.Name)
        Else
            Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(mnu.Name), Core.PlugIn)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            plugInForm.MdiParent = Me
            plugInForm.Show()
        End If
    End Sub
    Private Sub ModuleTSBClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As ToolStripButton = DirectCast(sender, ToolStripButton)
        If tabMDI.TabPages.ContainsKey(btn.Name) = True Then
            tabMDI.SelectTab(btn.Name)
        Else
            Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(btn.Name), Core.PlugIn)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            plugInForm.MdiParent = Me
            plugInForm.Show()
        End If

    End Sub
#End Region

#Region "TabbedMDI Window Routines"
    Private Sub tabMDI_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMDI.SelectedIndexChanged
        If ((Not Me.tabMDI.SelectedTab Is Nothing) AndAlso (Not Me.tabMDI.SelectedTab.Tag Is Nothing)) Then
            TryCast(Me.tabMDI.SelectedTab.Tag, Form).Select()
        End If
    End Sub
    Private Sub frmEveHQ_MdiChildActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MdiChildActivate
        If (MyBase.ActiveMdiChild Is Nothing) Then
            Me.tabMDI.Visible = False
        Else
            MyBase.ActiveMdiChild.WindowState = FormWindowState.Maximized
            If (MyBase.ActiveMdiChild.Tag Is Nothing) Then
                Dim tp As New TabPage(MyBase.ActiveMdiChild.Text)
                tp.Tag = MyBase.ActiveMdiChild
                tp.Name = MyBase.ActiveMdiChild.Text
                tp.Parent = Me.tabMDI
                tp.Show()
                MyBase.ActiveMdiChild.Tag = tp
                AddHandler MyBase.ActiveMdiChild.FormClosed, New FormClosedEventHandler(AddressOf Me.ActiveMdiChild_FormClosed)
            End If
            Me.tabMDI.SelectedTab = TryCast(MyBase.ActiveMdiChild.Tag, TabPage)
            If Not Me.tabMDI.Visible Then
                Me.tabMDI.Visible = True
            End If
        End If
    End Sub
    Private Sub ActiveMdiChild_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        TryCast(TryCast(sender, Form).Tag, TabPage).Dispose()
        TryCast(sender, Form).Dispose()
    End Sub
    Private Sub OpenPilotInfoForm()
        If tabMDI.TabPages.ContainsKey(frmPilot.Text) = False Then
            frmPilot.MdiParent = Me
            frmPilot.Show()
        Else
            tabMDI.SelectTab(frmPilot.Text)
        End If
    End Sub
    Private Sub OpenSkillTrainingForm()
        If tabMDI.TabPages.ContainsKey(frmTraining.Text) = False Then
            frmTraining.MdiParent = Me
            frmTraining.Show()
        Else
            tabMDI.SelectTab(frmTraining.Text)
        End If
    End Sub
    Private Sub OpenWebBrowserForm()
        If tabMDI.TabPages.ContainsKey(frmWebBrowser.Text) = False Then
            frmWebBrowser.MdiParent = Me
            frmWebBrowser.Show()
        Else
            tabMDI.SelectTab(frmWebBrowser.Text)
        End If
    End Sub
    Private Sub OpenBackUpForm()
        If tabMDI.TabPages.ContainsKey(frmBackup.Text) = False Then
            frmBackup.MdiParent = Me
            frmBackup.Show()
        Else
            tabMDI.SelectTab(frmBackup.Text)
        End If
    End Sub
    Private Sub OpenAPICheckerForm()
        If tabMDI.TabPages.ContainsKey(frmAPIChecker.Text) = False Then
            frmAPIChecker.MdiParent = Me
            frmAPIChecker.Show()
        Else
            tabMDI.SelectTab(frmAPIChecker.Text)
        End If
    End Sub
    Private Sub OpenMarketPricesForm()
        If tabMDI.TabPages.ContainsKey(EveHQMLF.Text) = False Then
            EveHQMLF = New frmMarketPrices
            EveHQMLF.MdiParent = Me
            EveHQMLF.Show()
        Else
            tabMDI.SelectTab(EveHQMLF.Text)
        End If
    End Sub
    Private Sub OpenInEveUploaderForm()
        If tabMDI.TabPages.ContainsKey(frmInEveUploader.Text) = False Then
            frmInEveUploader.MdiParent = Me
            frmInEveUploader.Show()
        Else
            tabMDI.SelectTab(frmInEveUploader.Text)
        End If
    End Sub
    Public Sub DisplayReport(ByRef reportForm As EveHQ.frmReportViewer, ByVal reportText As String)
        reportForm.Text = reportText
        If tabMDI.TabPages.ContainsKey(reportForm.Text) = False Then
            reportForm.MdiParent = Me
            reportForm.Show()
        Else
            tabMDI.SelectTab(reportForm.Text)
        End If
    End Sub
    Private Sub mnuCloseMDITab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCloseMDITab.Click
        Dim tp As TabPage = tabMDI.TabPages(CInt(tabMDI.Tag))
        TryCast(tp.Tag, Form).Close()
    End Sub
    Private Function TabControlHitTest(ByVal TabCtrl As TabControl, ByVal pt As Point) As Integer
        ' Test each tabs rectangle to see if our point is contained within it.
        For x As Integer = 0 To TabCtrl.TabPages.Count - 1
            ' If tab contians our rectangle return it's index.
            If TabCtrl.GetTabRect(x).Contains(pt) Then Return x
        Next
        ' A tab was not located at specified point.
        Return -1
    End Function
    Private Sub TabMDI_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tabMDI.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim TabIndex As Integer
            ' Get index of tab clicked
            TabIndex = TabControlHitTest(tabMDI, e.Location)
            ' If a tab was clicked display it's index
            If TabIndex >= 0 Then
                tabMDI.Tag = TabIndex
                Dim tp As TabPage = tabMDI.TabPages(CInt(tabMDI.Tag))
                mnuCloseMDITab.Text = "Close " & tp.Text
            End If
        End If
    End Sub

#End Region

    Private Sub TrainingInformationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrainingInformationToolStripMenuItem.Click
        If TrainingInformationToolStripMenuItem.Checked = True Then
            If frmTrainingInfo.IsHandleCreated = False Then
                tsbTrainingOverlay.Checked = True
                TrainingInformationToolStripMenuItem.Checked = True
                Call Me.ShowTrainingOverlay()
                Me.Select()
            End If
        Else
            If frmTrainingInfo.IsHandleCreated = True Then
                tsbTrainingOverlay.Checked = False
                TrainingInformationToolStripMenuItem.Checked = False
                frmTrainingInfo.Close()
            End If
        End If
    End Sub
    Private Sub tsbTrainingOverlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbTrainingOverlay.Click
        If tsbTrainingOverlay.Checked = True Then
            If frmTrainingInfo.IsHandleCreated = False Then
                tsbTrainingOverlay.Checked = True
                TrainingInformationToolStripMenuItem.Checked = True
                Call Me.ShowTrainingOverlay()
                Me.Select()
            End If
        Else
            If frmTrainingInfo.IsHandleCreated = True Then
                tsbTrainingOverlay.Checked = False
                TrainingInformationToolStripMenuItem.Checked = False
                frmTrainingInfo.Close()
            End If
        End If
    End Sub
    Private Sub ShowTrainingOverlay()
        ' Get x,y co-ords
        Dim wx As Integer = Screen.PrimaryScreen.WorkingArea.Right
        Dim wy As Integer = Screen.PrimaryScreen.WorkingArea.Bottom
        Dim x As Integer = 0
        Dim y As Integer = 0
        Select Case EveHQ.Core.HQ.EveHQSettings.OverlayPosition
            Case 0
                x = EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                y = EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
            Case 1
                x = wx - frmTrainingInfo.Width - EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                y = EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
            Case 2
                x = EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                y = wy - frmTrainingInfo.Height - EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
            Case Else
                x = wx - frmTrainingInfo.Width - EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                y = wy - frmTrainingInfo.Height - EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
        End Select
        frmTrainingInfo.Left = x
        frmTrainingInfo.Top = y
        frmTrainingInfo.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayBorderColor))
        frmTrainingInfo.Panel1.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayPanelColor))
        frmTrainingInfo.lblPilot.ForeColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayFontColor))
        frmTrainingInfo.lblTrainingStatus.ForeColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayFontColor))
        frmTrainingInfo.Opacity = (100 - EveHQ.Core.HQ.EveHQSettings.OverlayTransparancy) / 100
        frmTrainingInfo.Show()
    End Sub
    Private Sub RemoteRefreshPilots()
        Call Me.UpdatePilotInfo()
    End Sub

    Private Sub btnTogglePanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTogglePanel.Click
        If btnTogglePanel.Checked = True Then
            ' If the panel is open
            btnTogglePanel.Image = My.Resources.panel_close
            XPanderList1.Visible = True
        Else
            ' If the panel is closed
            btnTogglePanel.Image = My.Resources.panel_open
            XPanderList1.Visible = False
        End If
    End Sub

    Private Sub CloseInfoPanel()
        btnTogglePanel.Image = My.Resources.panel_open
        XPanderList1.Visible = False
        btnTogglePanel.Checked = False
    End Sub

    Private Sub mnuECMExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuECMExport.Click
        Dim ECMLocation As String = ""
        Dim result As Integer = 0
        With fbd1
            .ShowNewFolderButton = False
            If EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation <> "" Then
                If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation) = True Then
                    .Description = "Please select the folder where the ECM XML files are located..." & ControlChars.CrLf & "Default is: " & EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation
                    .SelectedPath = EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation
                Else
                    .Description = "Please select the folder where the ECM XML files are located..."
                    .RootFolder = Environment.SpecialFolder.Desktop
                End If
            Else
                .Description = "Please select the folder where the ECM XML files are located..."
                .RootFolder = Environment.SpecialFolder.Desktop
            End If
            result = .ShowDialog()
            ECMLocation = .SelectedPath
        End With

        If ECMLocation <> "" And result = 1 Then

            ' Generate the Old Style XML Report
            Call EveHQ.Core.Reports.GenerateCurrentPilotXML_Old(EveHQ.Core.HQ.myPilot)
            ' Generate the Old Style Training XML
            Call EveHQ.Core.Reports.GenerateCurrentTrainingXML_Old(EveHQ.Core.HQ.myPilot)

            ' Copy these to the selected folder
            My.Computer.FileSystem.CopyFile(EveHQ.Core.HQ.reportFolder & "\CurrentXML - Old (" & EveHQ.Core.HQ.myPilot.Name & ").xml", ECMLocation & "\" & EveHQ.Core.HQ.myPilot.ID.ToString & ".xml", True)
            My.Computer.FileSystem.CopyFile(EveHQ.Core.HQ.reportFolder & "\TrainingXML - Old (" & EveHQ.Core.HQ.myPilot.Name & ").xml", ECMLocation & "\" & EveHQ.Core.HQ.myPilot.ID.ToString & ".training.xml", True)
            EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation = ECMLocation
            MessageBox.Show("Export of ECM-compatible files completed!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

            MessageBox.Show("Export of ECM-compatible aborted!", "Export Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub

    Public Sub DisplayChartReport(ByRef chartForm As EveHQ.frmChartViewer, ByVal formTitle As String)
        chartForm.Text = formTitle
        If tabMDI.TabPages.ContainsKey(chartForm.Text) = False Then
            chartForm.MdiParent = Me
            chartForm.Show()
        Else
            tabMDI.SelectTab(chartForm.Text)
        End If
    End Sub

    Private Sub ctxLaunchEve1Normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve1Normal.Click
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(EveHQ.Core.HQ.EveHQSettings.EveFolder(1) & "\Eve.exe")
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub ctxLaunchEve2Normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve2Normal.Click
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(EveHQ.Core.HQ.EveHQSettings.EveFolder(2) & "\Eve.exe")
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub ctxLaunchEve3Normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve3Normal.Click
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(EveHQ.Core.HQ.EveHQSettings.EveFolder(3) & "\Eve.exe")
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub ctxLaunchEve4Normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve4Normal.Click
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(EveHQ.Core.HQ.EveHQSettings.EveFolder(4) & "\Eve.exe")
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub ctxLaunchEve1Full_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve1Full.Click
        Call Me.CheckEveHandle(1)
    End Sub

    Private Sub ctxLaunchEve2Full_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve2Full.Click
        Call Me.CheckEveHandle(2)
    End Sub

    Private Sub ctxLaunchEve3Full_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve3Full.Click
        Call Me.CheckEveHandle(3)
    End Sub

    Private Sub ctxLaunchEve4Full_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve4Full.Click
        Call Me.CheckEveHandle(4)
    End Sub

    Private Sub CheckEveHandle(ByVal EveFolder As Integer)
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(EveHQ.Core.HQ.EveHQSettings.EveFolder(EveFolder) & "\Eve.exe")
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
        tmrEveWindow.Tag = EveFolder
        tmrEveWindow.Enabled = True
    End Sub

    Private Sub tmrEveWindow_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrEveWindow.Tick
        Dim processes() As Process = Process.GetProcesses()
        Dim windowHandles As New ArrayList
        For Each process As Process In processes
            If process.MainWindowTitle = "EVE" Then
                If tmrEveWindow.Tag.ToString <> "" Then
                    If process.MainModule.FileName.ToUpper = (EveHQ.Core.HQ.EveHQSettings.EveFolder(CInt(tmrEveWindow.Tag)) & "\bin\Exefile.exe").ToUpper Then
                        Dim screenW As Integer = My.Computer.Screen.Bounds.Width
                        Dim screenH As Integer = My.Computer.Screen.Bounds.Height
                        Dim windowRect, clientRect As RECT
                        GetWindowRect(CInt(process.MainWindowHandle), windowRect)
                        GetClientRect(CInt(process.MainWindowHandle), clientRect)

                        Dim eveWindowWidth, eveWindowHeight As Integer
                        Dim eveClientWidth, eveClientHeight As Integer
                        eveWindowWidth = windowRect.Right - windowRect.Left
                        eveWindowHeight = windowRect.Bottom - windowRect.Top
                        eveClientWidth = clientRect.Right - clientRect.Left
                        eveClientHeight = clientRect.Bottom - clientRect.Top

                        Dim eveBorderWidth As Integer = CInt((eveWindowWidth - eveClientWidth) / 2)
                        Dim eveTitleBarHeight As Integer = eveWindowHeight - eveClientHeight - (2 * eveBorderWidth)
                        Dim BorderWidth As Integer = CInt((Me.Width - Me.ClientSize.Width) / 2)
                        Dim TitlebarHeight As Integer = Me.Height - Me.ClientSize.Height - 2 * BorderWidth
                        If eveTitleBarHeight > 0 Then
                            MoveWindow(process.MainWindowHandle, 0 - eveBorderWidth, 0 - eveTitleBarHeight - eveBorderWidth, screenW + (eveBorderWidth * 2), screenH + eveTitleBarHeight + (eveBorderWidth * 2), True)
                            tmrEveWindow.Tag = ""
                            tmrEveWindow.Enabled = False
                        End If
                    End If
                End If
            End If
        Next
    End Sub
    Private Sub ResizeEveWindow(ByVal eveProcess As Process)

        Dim screenW As Integer = My.Computer.Screen.Bounds.Width
        Dim screenH As Integer = My.Computer.Screen.Bounds.Height
        Dim windowRect, clientRect As RECT
        GetWindowRect(CInt(eveProcess.MainWindowHandle), windowRect)
        GetClientRect(CInt(eveProcess.MainWindowHandle), clientRect)

        Dim eveWindowWidth, eveWindowHeight As Integer
        Dim eveClientWidth, eveClientHeight As Integer
        eveWindowWidth = windowRect.Right - windowRect.Left
        eveWindowHeight = windowRect.Bottom - windowRect.Top
        eveClientWidth = clientRect.Right - clientRect.Left
        eveClientHeight = clientRect.Bottom - clientRect.Top

        Dim eveBorderWidth As Integer = CInt((eveWindowWidth - eveClientWidth) / 2)
        Dim eveTitleBarHeight As Integer = eveWindowHeight - eveClientHeight - (2 * eveBorderWidth)
        Dim BorderWidth As Integer = CInt((Me.Width - Me.ClientSize.Width) / 2)
        Dim TitlebarHeight As Integer = Me.Height - Me.ClientSize.Height - 2 * BorderWidth

        MoveWindow(eveProcess.MainWindowHandle, 0 - eveBorderWidth, 0 - eveTitleBarHeight - eveBorderWidth, screenW + (eveBorderWidth * 2), screenH + eveTitleBarHeight + (eveBorderWidth * 2), True)

    End Sub

#Region "APIRS Routines"
    Private Sub APIRSWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles APIRSWorker.DoWork
        EveHQ.Core.HQ.myAPIRS.RunAPIRS(IGBWorker, e)
    End Sub
#End Region

#Region "New Popup Routines"
    Private Sub EveStatusIcon_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles EveStatusIcon.MouseMove
        Select Case EveHQ.Core.HQ.EveHQSettings.TaskbarIconMode
            Case 0 ' Simple (tooltip

            Case 1 ' Enhanced (form)
                EveStatusIcon.Text = ""
                If frmToolTrayIconPopup.IsHandleCreated = False Then
                    frmToolTrayIconPopup.ConfigureForm()
                    Dim workingRectangle As System.Drawing.Rectangle = Screen.PrimaryScreen.WorkingArea
                    Dim TaskBarLocation As String = Me.GetTaskbarLocation
                    Select Case TaskBarLocation
                        Case "Bottom"
                            frmToolTrayIconPopup.Location = New System.Drawing.Point(workingRectangle.Width - frmToolTrayIconPopup.Width - 5, workingRectangle.Height - frmToolTrayIconPopup.Height - 5)
                        Case "Top"
                            frmToolTrayIconPopup.Location = New System.Drawing.Point(workingRectangle.Width - frmToolTrayIconPopup.Width - 5, workingRectangle.Y + 5)
                        Case "Left"
                            frmToolTrayIconPopup.Location = New System.Drawing.Point(workingRectangle.X + 5, workingRectangle.Height - frmToolTrayIconPopup.Height - 5)
                        Case "Right"
                            frmToolTrayIconPopup.Location = New System.Drawing.Point(workingRectangle.Width - frmToolTrayIconPopup.Width - 5, workingRectangle.Height - frmToolTrayIconPopup.Height - 5)
                        Case Else
                            Exit Sub
                    End Select
                    Try
                        frmToolTrayIconPopup.Show()
                    Catch ex As Exception
                    End Try
                Else
                    frmToolTrayIconPopup.tmrSkill.Stop()
                    frmToolTrayIconPopup.tmrSkill.Start()
                End If
        End Select

    End Sub
    Private Function GetTaskbarLocation() As String
        Dim tbLeft As Int32
        Dim tbTop As Int32
        Dim tbRight As Int32
        Dim tbBottom As Int32
        Dim location As String = ""

        ' Get task bar position and state
        Dim Result As String = GetTaskbarState(Me.Handle, tbLeft, tbTop, tbRight, tbBottom)

        If Result <> "Error" Then

            ' Get screen dimensions
            Dim sX, sY, sW, sH As Int32
            sX = 0
            sY = 0
            sW = Screen.PrimaryScreen.Bounds.Width
            sH = Screen.PrimaryScreen.Bounds.Height

            ' Work out position
            If tbBottom = sH Then
                If tbTop <> sY Then
                    location = "Bottom"
                Else
                    If tbRight = sW Then
                        location = "Right"
                    Else
                        location = "Left"
                    End If
                End If
            Else
                location = "Top"
            End If
            Return location
        Else
            Return "Error"
        End If
    End Function
    Private Function GetTaskbarState(ByVal ParentHandle As IntPtr, ByRef tbLeft As Int32, ByRef tbTop As Int32, ByRef tbRight As Int32, ByRef tbBottom As Int32) As String
        Try
            Dim Result As Int32
            Dim abd As New APPBARDATA
            Dim state As String = ""
            Call SHAppBarMessage(ABM_GETTASKBARPOS, abd)
            Result = SHAppBarMessage(ABM_GETSTATE, abd)
            If CBool((Result And ABS_AUTOHIDE)) Then
                state = "Autohide is Enabled"
            ElseIf CBool((Result And ABS_ALWAYSONTOP)) Then
                state = "Always on Top"
            End If
            With abd.rc
                tbLeft = .Left
                tbTop = .Top
                tbRight = .Right
                tbBottom = .Bottom
            End With
            Return state
        Catch e As Exception
            Return "Error"
        End Try
    End Function

#End Region

    Private Sub ClearEveHQCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearEveHQCache.Click
        Dim msg As String = "This will delete the entire contents of the cache folder, clear the pilot data and reconnect to the API." & ControlChars.CrLf & ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Cache", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Close all open forms
                For Each tp As TabPage In tabMDI.TabPages
                    TryCast(tp.Tag, Form).Close()
                Next

                ' Clear the EveHQ cache
                Try
                    If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.cacheFolder) Then
                        My.Computer.FileSystem.DeleteDirectory(EveHQ.Core.HQ.cacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                Catch ex As Exception
                End Try

                ' Recreate the EveHQ cache folder
                Try
                    If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.cacheFolder) = False Then
                        My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.cacheFolder)
                    End If
                Catch ex As Exception
                End Try

                ' Clear the EveHQ Pilot Data
                Try
                    EveHQ.Core.HQ.EveHQSettings.Pilots.Clear()
                    EveHQ.Core.HQ.TPilots.Clear()
                    EveHQ.Core.HQ.myPilot = Nothing
                Catch ex As Exception
                End Try

                ' Update the pilot lists
                Call Me.UpdatePilotInfo(True)

                ' Restart the timer
                tmrSkillUpdate.Enabled = True

                ' Call the API
                Call Me.QueryMyEveServer()

            Catch ex As Exception
                MessageBox.Show("Error Deleting the EveHQ Cache Folder, please try to delete the following location manually: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.cacheFolder, "Error Deleting Cache", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End If
    End Sub

    Private Sub mnuToolsTriggerError_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsTriggerError.Click
        Dim errPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots("O M G"), Core.Pilot)
        MessageBox.Show(errPilot.Name)
    End Sub

    Private Sub btnAddAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAccount.Click
        Dim EveHQSettings As New frmSettings
        EveHQSettings.Tag = "nodeEveAccounts"
        EveHQSettings.ShowDialog()
        EveHQSettings.Dispose()
    End Sub

    Public Function CacheErrorHandler() As Boolean
        ' Stop the timer from reporting multiple errors
        tmrSkillUpdate.Enabled = False
        Dim cXML, tXML As New XmlDocument
        cXML.Load(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_5_" & EveHQ.Core.HQ.myPilot.Account & "_" & EveHQ.Core.HQ.myPilot.ID & ".xml")
        tXML.Load(EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_6_" & EveHQ.Core.HQ.myPilot.Account & "_" & EveHQ.Core.HQ.myPilot.ID & ".xml")
        Dim msg As New StringBuilder
        msg.Append("EveHQ has detected that the skill that " & EveHQ.Core.HQ.myPilot.Name & " is supposedly training is not in the list of their skills. ")
        msg.AppendLine("This could be due to a corrupt cache file or a conflict with another skill training application. The relevant data was:")
        msg.AppendLine("")
        msg.AppendLine("Skill ID: " & EveHQ.Core.HQ.myPilot.TrainingSkillID.ToString)
        msg.AppendLine("Skill Name: " & EveHQ.Core.SkillFunctions.SkillIDToName(EveHQ.Core.HQ.myPilot.TrainingSkillID))
        msg.AppendLine("")
        msg.AppendLine("This data, together with the character and training XML have been copied to the clipboard. If you would like to assist in trying to correct this error, please copy the details into an email to the EveHQ Developers.")
        msg.AppendLine("")
        msg.AppendLine("The issue may be resolved by clearing the EveHQ cache and connecting back to the API. Would you like to do this now?")
        msg.AppendLine("")
        Try
            Clipboard.SetText(msg.ToString & cXML.InnerXml & ControlChars.CrLf & ControlChars.CrLf & tXML.InnerXml)
        Catch e As Exception
        End Try

        Dim reply As Integer = MessageBox.Show(msg.ToString, "Skill Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.No Then
            ' Don't do anything with the cache but restart the timer
            tmrSkillUpdate.Enabled = True
            Return False
        Else
            ' Close all open forms
            For Each tp As TabPage In tabMDI.TabPages
                TryCast(tp.Tag, Form).Close()
            Next

            ' Clear the EveHQ cache
            Try
                If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.cacheFolder) Then
                    My.Computer.FileSystem.DeleteDirectory(EveHQ.Core.HQ.cacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                End If
            Catch e As Exception
            End Try

            ' Recreate the EveHQ cache folder
            Try
                If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.cacheFolder) = False Then
                    My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.cacheFolder)
                End If
            Catch e As Exception
            End Try

            ' Clear the EveHQ Pilot Data
            Try
                EveHQ.Core.HQ.EveHQSettings.Pilots.Clear()
                EveHQ.Core.HQ.TPilots.Clear()
                EveHQ.Core.HQ.myPilot = Nothing
            Catch ex As Exception
            End Try

            ' Update the pilot lists
            Call Me.UpdatePilotInfo(True)

            ' Restart the timer
            tmrSkillUpdate.Enabled = True

            ' Call the API
            Call Me.QueryMyEveServer()

            Return True
        End If
    End Function

#Region "Market Log Watcher Routines"
    Public Function InitialiseWatchers() As Boolean
        ' Clear the list of watchers, just in case
        Call Me.CancelWatchers()
        Dim MLFolder As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Eve\logs\Marketlogs\"
        If My.Computer.FileSystem.DirectoryExists(MLFolder) = True Then
            Dim emeFSW As New FileSystemWatcher
            emeFSW = New FileSystemWatcher
            emeFSW.Path = MLFolder
            emeFSW.IncludeSubdirectories = True
            emeFSW.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)
            emeFSW.Filter = "*.txt"
            AddHandler emeFSW.Created, AddressOf OnMarketLogCreated
            emeFSW.EnableRaisingEvents = True
            EveHQMLW.Add(MLFolder, emeFSW)
            Me.iconEveHQMLW.Text = "EveHQ Market Export - Awaiting exports..."
            Me.iconEveHQMLW.Visible = True
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub CancelWatchers()
        ' Stop and dispose of the watchers before clearing the list
        For Each emeFSW As FileSystemWatcher In EveHQMLW.Values
            emeFSW.EnableRaisingEvents = False
            emeFSW.Dispose()
        Next
        EveHQMLW.Clear()
        Me.iconEveHQMLW.Visible = False
    End Sub

    Private Sub OnMarketLogCreated(ByVal source As Object, ByVal e As FileSystemEventArgs)
        ' Specify what is done when a file is created
        If EveHQMLF.IsHandleCreated = True Then
            Call EveHQMLF.DisplayLogDetails(e.FullPath)
            Call EveHQMLF.ResortLogs()
        End If
        If EveHQ.Core.HQ.EveHQSettings.MarketLogUpdatePrice = True Or EveHQ.Core.HQ.EveHQSettings.MarketLogUpdateData = True Then
            ' Get the price information
            Dim priceData As ArrayList = EveHQ.Core.DataFunctions.ProcessMarketExportFile(e.FullPath)
            Dim USerPrice As Double = CDbl(priceData(12)) : Dim typeID As Long = CLng(priceData(13))
            If EveHQ.Core.HQ.EveHQSettings.MarketLogUpdatePrice = True Then
                ' Update the market price
                EveHQ.Core.DataFunctions.SetCustomPrice(typeID, USerPrice, False)
            End If
            If EveHQ.Core.HQ.EveHQSettings.MarketLogToolTipConfirm = True = True Then
                iconEveHQMLW.BalloonTipTitle = "Market Export Processing Completed"
                iconEveHQMLW.BalloonTipText = "The file: " & e.Name & " has been successfully processed!"
                iconEveHQMLW.BalloonTipIcon = ToolTipIcon.Info
                iconEveHQMLW.ShowBalloonTip(10)
            End If
            If EveHQ.Core.HQ.EveHQSettings.MarketLogPopupConfirm = True Then
                MessageBox.Show("The file: " & e.Name & " has been successfully processed!", "Market Export Processing Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

    End Sub


#End Region

    Private Sub tsAPIStatus_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsAPIStatus.DoubleClick
        If EveHQ.Core.HQ.APIResults.Count > 0 Then
            Dim APIStatus As New EveHQ.Core.EveAPIStatusForm
            APIStatus.ShowDialog()
            APIStatus.Dispose()
        End If
    End Sub
End Class

