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
    Dim WithEvents IGBWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Public WithEvents APIRSWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents SkillWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents ImportWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents BackupWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents EveHQBackupWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
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
    Private EveHQTrayForm As Form = Nothing
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

#Region "Icon Routines"

    Private Sub EveHQIcon1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles EveStatusIcon.Click
        If Not (TypeOf e Is MouseEventArgs AndAlso (Not TypeOf e Is MouseEventArgs OrElse (TryCast(e, MouseEventArgs).Button = MouseButtons.Right))) Then
            MyBase.Visible = True
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
            MyBase.ShowInTaskbar = True
            MyBase.Activate()
            If EveHQTrayForm IsNot Nothing Then
                EveHQTrayForm.Close()
                EveHQTrayForm = Nothing
            End If
        End If
    End Sub

    Private Sub EveHQIcon1_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles EveStatusIcon.MouseHover
        ' Only display the pop up window if the context menu isn't showing
        If Not Me.EveIconMenu.Visible And EveHQ.Core.HQ.EveHQSettings.TaskbarIconMode = 1 Then
            EveHQTrayForm = New frmToolTrayIconPopup
            EveHQTrayForm.Show()
        End If
    End Sub

    Private Sub EveHQIcon1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles EveStatusIcon.MouseLeave
        ' Remove the popup if its showing
        If EveHQTrayForm IsNot Nothing Then
            EveHQTrayForm.Close()
            EveHQTrayForm = Nothing
        End If
    End Sub

#End Region

#Region "Menu Click Routines"

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub
    Private Sub TileVerticleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub
    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub
    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub
    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
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
    Private Sub mnuToolsGetAccountInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsGetAccountInfo.Click, tsbRetrieveData.Click
        Call QueryMyEveServer()
    End Sub
    Private Sub SkillTrainingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SkillTrainingToolStripMenuItem.Click, tsbSkillTraining.Click
        Call OpenSkillTrainingForm()
    End Sub
    Private Sub tsbMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbMail.Click
        Call Me.OpenEveHQMailForm()
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
    End Sub
    Private Sub tmrEve_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrEve.Tick
        tmrEve.Interval = 120000
        Call GetServerStatus()
    End Sub
    Private Sub UpdateEveTime()
        Dim now As DateTime = DateTime.Now.ToUniversalTime()
        Dim fi As Globalization.DateTimeFormatInfo = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat
        tsEveTime.Text = "EVE Time: " & now.ToString(fi.ShortDatePattern + " HH:mm")
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
                'EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
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
                EveStatusIcon.Icon = My.Resources.EveHQ_online
            Case EveHQ.Core.EveServer.ServerStatus.ProxyDown
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                EveStatusIcon.Icon = My.Resources.EveHQ_offline
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
            Case EveHQ.Core.EveServer.ServerStatus.Unknown
                EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Status unknown"
                EveStatusIcon.Icon = My.Resources.EveHQ
            Case EveHQ.Core.EveServer.ServerStatus.Up
                tsTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Online (" & EveHQ.Core.HQ.myTQServer.Players & " Players)"
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
                ' Check if there are updates available
                If EveHQ.Core.HQ.AppUpdateAvailable = True Then
                    Dim msg As String = "There are pending updates available - these will be installed now."
                    MessageBox.Show(msg, "Update Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Call Me.UpdateNow()
                Else
                    Call Me.ShutdownRoutine()
                End If
            End If
        Else
            ' Check if there are updates available
            If EveHQ.Core.HQ.AppUpdateAvailable = True Then
                Dim msg As String = "There are pending updates available - these will be installed now."
                MessageBox.Show(msg, "Update Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Call Me.UpdateNow()
            Else
                Call Me.ShutdownRoutine()
            End If
        End If
    End Sub
    Private Sub frmEveHQ_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Hide()

        Me.EveStatusIcon.Visible = True

        Me.MenuStrip.MdiWindowListItem = Nothing

        ' Add the pilot refresh handler
        AddHandler EveHQ.Core.PilotParseFunctions.RefreshPilots, AddressOf Me.RemoteRefreshPilots
        AddHandler EveHQ.Core.G15LCDB.UpdateAPI, AddressOf Me.RemoteUpdate
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

        ' Determine which view to display!
        Select Case EveHQ.Core.HQ.EveHQSettings.StartupView
            Case "EveHQ Dashboard"
                ' Open the dashboard
                Call Me.OpenDashboard()
            Case "Pilot Information"
                If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                    ' Open the pilot info form
                    Call OpenPilotInfoForm()
                End If
            Case "Pilot Summary Report"
                ' Show the pilot summary report form!
                Dim newReport As New EveHQ.frmReportViewer
                Call EveHQ.Core.Reports.GenerateCharSummary()
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PilotSummary.html"))
                Call DisplayReport(newReport, "Pilot Summary")
            Case "Skill Training"
                If EveHQ.Core.HQ.EveHQSettings.StartupPilot <> "" Then
                    ' Open the skill training form
                    Call OpenSkillTrainingForm()
                End If
            Case Else
                ' Show the pilot summary report form!
                Dim newReport As New EveHQ.frmReportViewer
                Call EveHQ.Core.Reports.GenerateCharSummary()
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PilotSummary.html"))
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

        ' Set the tab position
        Select Case EveHQ.Core.HQ.EveHQSettings.MDITabPosition
            Case "Top"
                Me.tabMDI.Dock = DockStyle.Top
            Case "Bottom"
                Me.tabMDI.Dock = DockStyle.Bottom
        End Select

        ' Set the toolbar position
        Select Case EveHQ.Core.HQ.EveHQSettings.ToolbarPosition
            Case "Top"
                Me.EveHQToolStrip.Dock = DockStyle.Top
            Case "Bottom"
                Me.EveHQToolStrip.Dock = DockStyle.Bottom
            Case "Left"
                Me.EveHQToolStrip.Dock = DockStyle.Left
            Case "Right"
                Me.EveHQToolStrip.Dock = DockStyle.Right
        End Select

        ' Set the training bar position
        Select Case EveHQ.Core.HQ.EveHQSettings.TrainingBarPosition
            Case "Top"
                Me.panelTrainingStatus.Dock = DockStyle.Top
                Me.ssTraining.Dock = DockStyle.Top
            Case "Bottom"
                Me.panelTrainingStatus.Dock = DockStyle.Bottom
                Me.ssTraining.Dock = DockStyle.Bottom
            Case "Left"
                Me.panelTrainingStatus.Dock = DockStyle.Left
                Me.ssTraining.Dock = DockStyle.Left
            Case "Right"
                Me.panelTrainingStatus.Dock = DockStyle.Right
                Me.ssTraining.Dock = DockStyle.Right
            Case "None"
                Me.panelTrainingStatus.Visible = False
                Me.ssTraining.Visible = False
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

        ' Start the timers
        If EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = True Then
            tmrEve.Enabled = True
        End If
        tmrSkillUpdate.Enabled = True
        tmrModules.Enabled = True

        Call EveHQ.Core.HQ.ReduceMemory()
        tmrMemory.Enabled = True

        ' Start the update check on a new thread
        If EveHQ.Core.HQ.EveHQSettings.DisableAutoWebConnections = False Then
            Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.CheckForUpdates)
        End If

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
    Private Sub EveStatusIcon_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
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

        ' Check if Shutdown Notification is active
        If EveHQ.Core.HQ.EveHQSettings.ShutdownNotify = True Then
            Dim accounts As New ArrayList
            Dim strNotify As String = ""
            Dim strCharNotify As String = ""
            For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If cPilot.Training = True Then
                    Dim timeLimit As Date = Now.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod * 3600)
                    If cPilot.TrainingEndTime < timeLimit Then
                        If cPilot.QueuedSkillTime > 0 Then
                            If cPilot.TrainingEndTime.AddSeconds(cPilot.QueuedSkillTime) < timeLimit Then
                                strCharNotify &= cPilot.Name & " - " & cPilot.TrainingSkillName & " (Skill Queue ends in " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.QueuedSkillTime) & ")" & ControlChars.CrLf
                            End If
                        Else
                            If cPilot.TrainingCurrentTime > 0 Then
                                strCharNotify &= cPilot.Name & " - " & cPilot.TrainingSkillName & " (Training ends in " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime) & ")" & ControlChars.CrLf
                            Else
                                strCharNotify &= cPilot.Name & " - " & cPilot.TrainingSkillName & " (Training already complete)" & ControlChars.CrLf
                            End If
                        End If
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
                    If cAccount.FriendlyName <> "" Then
                        strAccountNotify &= cAccount.FriendlyName & " (UserID: " & cAccount.userID & ")" & ControlChars.CrLf
                    Else
                        strAccountNotify &= "UserID: " & cAccount.userID & ControlChars.CrLf
                    End If
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

        ' Close the tabs if they are open, forcing the correct closure of each plug-in and form
        For Each tp As TabPage In tabMDI.TabPages
            TryCast(tp.Tag, Form).Close()
        Next

        ' Check for backup warning expiry
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode = 1 Then
            Dim backupDate As Date = EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast.AddDays(EveHQ.Core.HQ.EveHQSettings.EveHQBackupWarnFreq)
            If backupDate < Now Then
                Dim timeElapsed As TimeSpan = Now - EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast
                Dim msg As String = "You haven't backed up your EveHQ Settings for " & timeElapsed.Days & " days. Would you like to do this now?"
                Dim reply As Integer = MessageBox.Show(msg, "Backup EveHQ Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.Yes Then
                    Call frmBackupEveHQ.BackupEveHQSettings()
                End If
            End If
        Else
            Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()
        End If

        ' Remove the icons
        EveStatusIcon.Visible = False : iconEveHQMLW.Visible = False
        iconEveHQMLW.Icon = Nothing
        iconEveHQMLW.Dispose() : EveStatusIcon.Dispose()
        'MessageBox.Show("Shutdown Routine is complete. Smell ya later!", "Shut Down Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End

    End Sub

#End Region

#Region "Skill Display Updater & Notification Routines"

    Private Sub SkillWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles SkillWorker.DoWork
        For Each tPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If tPilot.Active = True Then
                tPilot.TrainingCurrentSP = CInt(EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(tPilot))
                tPilot.TrainingCurrentTime = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(tPilot)
            End If
        Next
    End Sub
    Private Sub SkillWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles SkillWorker.RunWorkerCompleted

        Call UpdateEveTime()

        Call CheckNotifications()

        Call Me.UpdateTrainingStatus()

        If frmPilot.IsHandleCreated = True Then
            Call frmPilot.UpdateSkillInfo()
        End If
        If frmTraining.IsHandleCreated = True Then
            Call frmTraining.UpdateTraining()
        End If
        If frmSkillDetails.IsHandleCreated = True Then
            Call frmSkillDetails.UpdateSkillDetails()
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

            Dim updateRequired As Boolean = False
            For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If cPilot.Name <> "" And cPilot.Account <> "" Then
                    Dim cacheCDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.CacheExpirationTime)
                    Dim cacheTDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingExpirationTime)
                    If cacheCDate < Now Or cacheTDate < Now Then
                        updateRequired = True
                        Exit For
                    Else
                        If cacheCDate < EveHQ.Core.HQ.NextAutoAPITime Then
                            EveHQ.Core.HQ.NextAutoAPITime = cacheCDate
                        End If
                        If cacheTDate < EveHQ.Core.HQ.NextAutoAPITime Then
                            EveHQ.Core.HQ.NextAutoAPITime = cacheTDate
                        End If
                        If EveHQ.Core.HQ.AutoRetryAPITime > EveHQ.Core.HQ.NextAutoAPITime Then
                            EveHQ.Core.HQ.NextAutoAPITime = EveHQ.Core.HQ.AutoRetryAPITime
                        End If
                    End If
                End If
            Next
            If Now > EveHQ.Core.HQ.AutoRetryAPITime Then
                If updateRequired = True Then
                    ' Invoke the API Caller
                    EveHQ.Core.HQ.NextAutoAPITime = Now.AddMinutes(60)
                    EveHQ.Core.HQ.AutoRetryAPITime = Now.AddMinutes(5)
                    Call QueryMyEveServer()
                End If
                ' Display time until autoAPI download
                Dim TimeLeft As TimeSpan = EveHQ.Core.HQ.NextAutoAPITime - Now
                tsAPITime.Text = EveHQ.Core.SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
            Else
                Dim TimeLeft As TimeSpan = EveHQ.Core.HQ.NextAutoAPITime - Now
                Dim TimeLeft2 As TimeSpan = EveHQ.Core.HQ.AutoRetryAPITime - Now
                tsAPITime.Text = EveHQ.Core.SkillFunctions.TimeToString(Math.Max(TimeLeft.TotalSeconds, TimeLeft2.TotalSeconds), False)
            End If
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
    Private Sub CheckNotifications()

        ' Only do this if at least one notification is enabled
        If EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = True Or EveHQ.Core.HQ.EveHQSettings.NotifyDialog = True Or EveHQ.Core.HQ.EveHQSettings.NotifyEMail = True Or EveHQ.Core.HQ.EveHQSettings.NotifySound = True Then
            Dim notifyText As String = ""
            For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If cPilot.Active = True And cPilot.Training = True Then
                    notifyText = ""
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

                        ' Show the notifications
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
                                ' Expand the details with some additional information
                                If cPilot.QueuedSkills.Count > 0 Then
                                    notifyText &= ControlChars.CrLf
                                    notifyText &= "Next skill in Eve skill queue: " & EveHQ.Core.SkillFunctions.SkillIDToName(CStr(cPilot.QueuedSkills.Values(0).SkillID)) & " " & EveHQ.Core.SkillFunctions.Roman(cPilot.QueuedSkills.Values(0).Level)
                                    notifyText &= ControlChars.CrLf
                                Else
                                    notifyText &= ControlChars.CrLf
                                    notifyText &= "Next skill in Eve skill queue: No skill queued"
                                    notifyText &= ControlChars.CrLf
                                End If
                                If cPilot.TrainingQueues.Count > 0 Then
                                    notifyText &= ControlChars.CrLf
                                    notifyText &= "EveHQ Skill Queue Info: " & ControlChars.CrLf
                                    For Each sq As EveHQ.Core.SkillQueue In cPilot.TrainingQueues.Values
                                        Dim nq As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(cPilot, sq)
                                        If sq.IncCurrentTraining = True Then
                                            If nq.Count > 1 Then
                                                For q As Integer = 1 To nq.Count - 1
                                                    If CType(nq(1), EveHQ.Core.SortedQueue).Done = False Then
                                                        notifyText &= sq.Name & ": " & CType(nq(q), EveHQ.Core.SortedQueue).Name
                                                        notifyText &= " (" & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueue).FromLevel))
                                                        notifyText &= " to " & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueue).FromLevel) + 1) & ")" & ControlChars.CrLf
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        Else
                                            If nq.Count > 0 Then
                                                For q As Integer = 0 To nq.Count - 1
                                                    If CType(nq(1), EveHQ.Core.SortedQueue).Done = False Then
                                                        notifyText &= sq.Name & ": " & CType(nq(q), EveHQ.Core.SortedQueue).Name
                                                        notifyText &= " (" & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueue).FromLevel))
                                                        notifyText &= " to " & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueue).FromLevel) + 1) & ")" & ControlChars.CrLf
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        End If
                                    Next
                                End If
                                Call SendEveHQMail(cPilot, notifyText)
                            End If
                        End If
                    End If
                End If
            Next
        End If

    End Sub
    Private Sub SendEveHQMail(ByVal cpilot As EveHQ.Core.Pilot, ByVal mailText As String)
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
            Dim eveHQMsg As New System.Net.Mail.MailMessage(EveHQ.Core.HQ.EveHQSettings.EmailSenderAddress, EveHQ.Core.HQ.EveHQSettings.EMailAddress)
            eveHQMsg.Subject = "Eve Training Notification: " & cpilot.Name & " (" & cpilot.TrainingSkillName & " " & EveHQ.Core.SkillFunctions.Roman(cpilot.TrainingSkillLevel) & ")"
            eveHQMsg.Body = mailText
            eveHQMail.Send(eveHQMsg)
        Catch ex As Exception
            MessageBox.Show("The mail notification sending process failed. Please check that the server, port, address, username and password are correct." & ControlChars.CrLf & ControlChars.CrLf & "The error was: " & ex.Message, "EveHQ Email Notification Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
    Public Sub UpdateToNextLevel()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Training = True Then
                If cPilot.PilotSkills.Contains(cPilot.TrainingSkillName) = True Then
                    Dim trainSkill As EveHQ.Core.PilotSkill = CType(cPilot.PilotSkills(cPilot.TrainingSkillName), Core.PilotSkill)
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

    Private Sub RemoteUpdate()
        Me.Invoke(New QueryMyEveServerDelegate(AddressOf QueryMyEveServer))
    End Sub

    Public Sub QueryMyEveServer()
        tsbRetrieveData.Enabled = False
        mnuToolsGetAccountInfo.Enabled = False
        frmSettings.btnGetData.Enabled = False
        Threading.ThreadPool.QueueUserWorkItem(AddressOf StartCharacterAPIThread)
    End Sub

    Public Sub StartCharacterAPIThread(ByVal state As Object)
        Dim curSelPilot As String = ""

        ' If we have accounts to query then get the data for them
        If EveHQ.Core.HQ.EveHQSettings.Accounts.Count = 0 Then
            tsAPIStatus.Text = "API Status: No accounts entered into settings!! (" & Now.ToString & ")"
            Exit Sub
        Else
            tsAPIStatus.Text = "API Status: Fetching Character Data..."
            EveHQStatusStrip.Refresh()
            ' Clear the current list of pilots
            EveHQ.Core.HQ.TPilots.Clear()
            EveHQ.Core.HQ.APIResults.Clear()
            ' get the details for the account
            Dim CurrentAccount As New EveHQ.Core.EveAccount
            For Each CurrentAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                tsAPIStatus.Text = "API Status: Updating Account '" & CurrentAccount.FriendlyName & "' (ID=" & CurrentAccount.userID & ")..."
                EveHQStatusStrip.Refresh()
                Call EveHQ.Core.PilotParseFunctions.GetCharactersInAccount(CurrentAccount)
            Next
            Call EveHQ.Core.PilotParseFunctions.CopyTempPilotsToMain()
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

        ' Enable the option again
        tsbRetrieveData.Enabled = True
        mnuToolsGetAccountInfo.Enabled = True
        Me.Invoke(New MethodInvoker(AddressOf ResetSettingsButton))
        ' Update if we have retrieved new data
        If ContainsNew = True Then
            Me.Invoke(New MethodInvoker(AddressOf UpdatePilotInfo))
        End If
    End Sub

    Public Sub ResetSettingsButton()
        Call frmSettings.FinaliseAPIServerUpdate()
    End Sub

    Public Sub UpdatePilotInfo(Optional ByVal startUp As Boolean = False)
        ' Creates a list of all available pilots and enters it into the pilot selection area
        Dim currentPilot As New EveHQ.Core.Pilot
        Dim allPilots As SortedList = New SortedList
        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If currentPilot.Active = True Then
                allPilots.Add(currentPilot.Name, currentPilot.Name)
            End If
        Next

        ' Setup the Training Status Bar
        Call Me.SetupTrainingStatus()

        ' Update the cbopilots in the pilot form and the training form
        If frmPilot.IsHandleCreated = True Then
            frmPilot.UpdatePilots()
        End If
        If frmTraining.IsHandleCreated = True Then
            frmTraining.UpdatePilots()
        End If

        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count = 0 Then
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
        End If

        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count = 0 Then
            mnuReportsHTMLChar.Enabled = False
        Else
            mnuReportsHTMLChar.Enabled = True
        End If

        ' Update the dashboard if applicable
        If frmDashboard.IsHandleCreated = True Then
            Call frmDashboard.UpdateWidgets()
        End If

        ' Redraw Reports menu with new pilots and queues options?
        Call Me.DrawReportsMenu(allPilots)

    End Sub

    Private Sub SetupTrainingStatus()

        ' Clear the list of Status Labels
        ssTraining.SuspendLayout()
        ssTraining.Items.Clear()

        Dim accounts As New ArrayList
        Dim pilotCount As Integer = 0
        Dim pilotTimes As New SortedList
        Dim pilotTime As Long = 0

        ' Get a list of the training pilots and build some status panels
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Training = True Then
                If cPilot.Active = True Then
                    ' Build a status panel
                    Dim newSSLabel As New ToolStripStatusLabel
                    newSSLabel.Name = "TrainingStatus" & pilotCount.ToString
                    newSSLabel.TextAlign = ContentAlignment.MiddleLeft
                    newSSLabel.IsLink = True
                    newSSLabel.LinkColor = Color.Black
                    newSSLabel.LinkBehavior = LinkBehavior.HoverUnderline
                    AddHandler newSSLabel.Click, AddressOf Me.TrainingStatusLabelClick
                    Dim ssPadding As New Padding(8, 2, 8, 2)
                    newSSLabel.Padding = ssPadding
                    pilotTime = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
                    If pilotTimes.ContainsKey(Format(pilotTime, "0000000000") & "_" & cPilot.ID) = False Then
                        pilotTimes.Add(Format(pilotTime, "0000000000") & "_" & cPilot.ID, cPilot)
                        pilotCount += 1
                        ssTraining.Items.Add(newSSLabel)
                    End If
                End If
                accounts.Add(cPilot.Account)
            End If
        Next

        ' Check each account to see if something is training. 
        For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            If accounts.Contains(cAccount.userID) = False Then
                ' Build a status panel
                Dim newSSLabel As New ToolStripStatusLabel
                newSSLabel.Name = "TrainingStatus" & pilotCount.ToString
                newSSLabel.TextAlign = ContentAlignment.MiddleLeft
                newSSLabel.Text = "Account: " & cAccount.FriendlyName & ControlChars.CrLf & "NOT CURRENTLY TRAINING!"
                newSSLabel.IsLink = True
                newSSLabel.LinkColor = Color.Red
                newSSLabel.LinkBehavior = LinkBehavior.HoverUnderline
                Dim ssPadding As New Padding(8, 2, 8, 2)
                newSSLabel.Padding = ssPadding
                pilotCount += 1
                ssTraining.Items.Add(newSSLabel)
            End If
        Next

        ' Put the details into the training panels
        pilotCount = 0
        For Each cPilot As EveHQ.Core.Pilot In pilotTimes.Values
            Dim newSSLabel As ToolStripStatusLabel = CType(ssTraining.Items(pilotCount), ToolStripStatusLabel)
            newSSLabel.ToolTipText = "Click to view skill training for " & cPilot.Name
            Dim strSS As New StringBuilder
            strSS.AppendLine(cPilot.Name & " - " & cPilot.TrainingSkillName)
            strSS.Append("Training Lvl " & EveHQ.Core.SkillFunctions.Roman(cPilot.TrainingSkillLevel) & ": " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime))
            If cPilot.QueuedSkillTime > 0 Then
                strSS.Append(ControlChars.CrLf & "Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.QueuedSkillTime))
                If cPilot.QueuedSkillTime + cPilot.TrainingCurrentTime < (24 * 60 * 60) Then
                    strSS.Append(" (" & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime) & ")")
                    newSSLabel.ToolTipText &= ControlChars.CrLf & "Queue Time of " & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime) & " is available"
                End If
            Else
                If cPilot.QueuedSkillTime + cPilot.TrainingCurrentTime < (24 * 60 * 60) Then
                    strSS.Append(ControlChars.CrLf & "Queue Time Available: " & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime))
                    newSSLabel.ToolTipText &= ControlChars.CrLf & "Queue Time of " & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime) & " is available"
                End If
            End If
            newSSLabel.Text = strSS.ToString
            newSSLabel.Tag = cPilot.Name
            Dim g As Graphics = Me.CreateGraphics
            newSSLabel.Width = Math.Max(CInt(g.MeasureString(newSSLabel.Text, ssTraining.Font).Width + newSSLabel.Padding.Left + newSSLabel.Padding.Right), newSSLabel.Width)
            newSSLabel.Tag = cPilot.Name
            pilotCount += 1
        Next

        ssTraining.ResumeLayout()
    End Sub

    Private Sub UpdateTrainingStatus()

        Dim accounts As New ArrayList
        Dim statusText As New StringBuilder
        Dim pilotTimes As New SortedList
        Dim pilotTime As Long = 0

        If frmSettings.IsHandleCreated = False Then
            ' Get a list of the training pilots and record training times
            For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If cPilot.Training = True Then
                    If cPilot.Active = True Then
                        pilotTime = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
                        If pilotTimes.ContainsKey(Format(pilotTime, "0000000000") & "_" & cPilot.ID) = False Then
                            pilotTimes.Add(Format(pilotTime, "0000000000") & "_" & cPilot.ID, cPilot)
                        End If
                    End If
                    accounts.Add(cPilot.Account)
                End If
            Next

            ' Put the details into the training panels
            Dim pilotCount As Integer = 0
            For Each cPilot As EveHQ.Core.Pilot In pilotTimes.Values
                If pilotCount < ssTraining.Items.Count Then
                    Dim newSSLabel As ToolStripStatusLabel = CType(ssTraining.Items(pilotCount), ToolStripStatusLabel)
                    newSSLabel.ToolTipText = "Click to view skill training for " & cPilot.Name
                    Dim strSS As New StringBuilder
                    strSS.AppendLine(cPilot.Name & " - " & cPilot.TrainingSkillName)
                    strSS.Append("Training Lvl " & EveHQ.Core.SkillFunctions.Roman(cPilot.TrainingSkillLevel) & ": " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime))
                    If cPilot.QueuedSkillTime > 0 Then
                        strSS.Append(ControlChars.CrLf & "Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(cPilot.QueuedSkillTime))
                        If cPilot.QueuedSkillTime + cPilot.TrainingCurrentTime < (24 * 60 * 60) Then
                            strSS.Append(" (" & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime) & ")")
                            newSSLabel.ToolTipText &= ControlChars.CrLf & "Queue Time of " & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime) & " is available"
                        End If
                    Else
                        If cPilot.QueuedSkillTime + cPilot.TrainingCurrentTime < (24 * 60 * 60) Then
                            strSS.Append(ControlChars.CrLf & "Queue Time Available: " & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime))
                            newSSLabel.ToolTipText &= ControlChars.CrLf & "Queue Time of " & EveHQ.Core.SkillFunctions.TimeToString(86400 - cPilot.QueuedSkillTime - cPilot.TrainingCurrentTime) & " is available"
                        End If
                    End If
                    newSSLabel.Text = strSS.ToString
                    newSSLabel.Tag = cPilot.Name
                    newSSLabel.AutoSize = False
                    Dim g As Graphics = Me.CreateGraphics
                    newSSLabel.Width = Math.Max(CInt(g.MeasureString(newSSLabel.Text, ssTraining.Font).Width + newSSLabel.Padding.Left + newSSLabel.Padding.Right), newSSLabel.Width)
                    pilotCount += 1
                End If
            Next

            ' Check each account to see if something is training. 
            For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                If pilotCount < ssTraining.Items.Count Then
                    If accounts.Contains(cAccount.userID) = False Then
                        ' Build a status panel
                        Dim newSSLabel As ToolStripStatusLabel = CType(ssTraining.Items(pilotCount), ToolStripStatusLabel)
                        newSSLabel.Text = "Account: " & cAccount.FriendlyName & ControlChars.CrLf & "NOT CURRENTLY TRAINING!"
                        pilotCount += 1
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub TrainingStatusLabelClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim selectedLabel As ToolStripStatusLabel = CType(sender, ToolStripStatusLabel)
        Call Me.OpenSkillTrainingForm()
        If selectedLabel.Tag IsNot Nothing Then
            frmTraining.DisplayPilotName = selectedLabel.Tag.ToString
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

        For menu As Integer = 0 To 4
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
                Case 4
                    currentMenu = mnuReportsPHPBBChar
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
                            If currentPilot IsNot Nothing Then
                                If currentPilot.TrainingQueues IsNot Nothing Then
                                    For Each qItem As EveHQ.Core.SkillQueue In currentPilot.TrainingQueues.Values
                                        Dim queueMenu As New ToolStripMenuItem
                                        queueMenu.Text = qItem.Name
                                        queueMenu.Name = pilotMenu.Name & "_" & qItem.Name
                                        AddHandler queueMenu.Click, AddressOf Me.ReportsMenuHandler
                                        pilotMenu.DropDownItems.Add(queueMenu)
                                    Next
                                End If
                            End If
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
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharSheet (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
            Case "mnuReportCharTraintimes"
                Call EveHQ.Core.Reports.GenerateTrainingTime(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainTime (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Training Times - " & rPilot.Name)
            Case "mnuReportTimeToLevel5"
                Call EveHQ.Core.Reports.GenerateTimeToLevel5(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
            Case "mnureportCharSkillLevels"
                Call EveHQ.Core.Reports.GenerateSkillLevels(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
            Case "mnuReportCharXML"
                Call EveHQ.Core.Reports.GenerateCharXML(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharXML (" & rPilot.Name & ").xml"))
                DisplayReport(newReport, "Imported Character XML - " & rPilot.Name)
            Case "mnuReportTrainXML"
                Call EveHQ.Core.Reports.GenerateTrainXML(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML (" & rPilot.Name & ").xml"))
                DisplayReport(newReport, "Imported Training XML - " & rPilot.Name)
            Case "mnuReportTrainingQueue"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportQueueShoppingList"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportSkillsAvailable"
                Call EveHQ.Core.Reports.GenerateSkillsAvailable(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
            Case "mnuReportSkillsNotTrained"
                Call EveHQ.Core.Reports.GenerateSkillsNotTrained(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
            Case "mnuReportCurrentCharXMLOld"
                Call EveHQ.Core.Reports.GenerateCurrentPilotXML_Old(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CurrentXML - Old (" & rPilot.Name & ").xml"))
                DisplayReport(newReport, "Old Style Character XML - " & rPilot.Name)
            Case "mnuReportCurrentCharXMLNew"
                Call EveHQ.Core.Reports.GenerateCurrentPilotXML_New(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CurrentXML - New (" & rPilot.Name & ").xml"))
                DisplayReport(newReport, "Current Character XML - " & rPilot.Name)
            Case "mnuReportCurrentTrainingXMLOld"
                Call EveHQ.Core.Reports.GenerateCurrentTrainingXML_Old(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML - Old (" & rPilot.Name & ").xml"))
                DisplayReport(newReport, "Old Style Training XML - " & rPilot.Name)
            Case "mnuReportSkillGroupChart"
                Dim newChartForm As New frmChartViewer
                newChartForm.Controls.Add(EveHQ.Core.Reports.SkillGroupChart(rPilot))
                Call Me.DisplayChartReport(newChartForm, "Skill Group Chart - " & rPilot.Name)
            Case "mnuReportsTextCharSheet"
                Call EveHQ.Core.Reports.GenerateTextCharSheet(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharSheet (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
            Case "mnuReportsTextTrainTimes"
                Call EveHQ.Core.Reports.GenerateTextTrainingTime(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainTime (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Training Times - " & rPilot.Name)
            Case "mnuReportsTextTimeToLevel5"
                Call EveHQ.Core.Reports.GenerateTextTimeToLevel5(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
            Case "mnuReportsTextSkillLevels"
                Call EveHQ.Core.Reports.GenerateTextSkillLevels(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
            Case "mnuReportsTextSkillsAvailable"
                Call EveHQ.Core.Reports.GenerateTextSkillsAvailable(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
            Case "mnuReportsTextSkillsNotTrained"
                Call EveHQ.Core.Reports.GenerateTextSkillsNotTrained(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
            Case "mnuReportsTextTrainingQueue"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTextTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportsTextShoppingList"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(menuParts(2)), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTextShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "mnuReportPartiallyTrainedSkills"
                Call EveHQ.Core.Reports.GeneratePartialSkills(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
            Case "mnuReportsTextPartiallyTrainedSkills"
                Call EveHQ.Core.Reports.GenerateTextPartialSkills(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
            Case "mnuReportECMExport"
                Call EveHQ.Core.Reports.GenerateECMExportReports(rPilot)
            Case "mnuReportSkillsCost"
                Call EveHQ.Core.Reports.GenerateSkillsCost(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsCost (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Skills Cost - " & rPilot.Name)
            Case "mnuReportsTextSkillsCost"
                Call EveHQ.Core.Reports.GenerateTextSkillsCost(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsCost (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Skills Cost - " & rPilot.Name)
            Case "mnuReportsPHPBBCharSheet"
                Call EveHQ.Core.Reports.GeneratePHPBBCharSheet(rPilot)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PHPBBCharSheet (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "PHPBB Character Sheet - " & rPilot.Name)
        End Select
    End Sub

    Private Sub mnuRepCharSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRepCharSummary.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCharSummary()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PilotSummary.html"))
        DisplayReport(newReport, "Pilot Summary")
    End Sub

    Private Sub mnuReportSPSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportSPSummary.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateSPSummary()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SPSummary.html"))
        DisplayReport(newReport, "Skill Point Summary")
    End Sub

    Private Sub mnuReportAsteroidAlloys_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportAsteroidAlloys.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateAlloyReport()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "AlloyReport.html"))
        DisplayReport(newReport, "Alloy Composition")
    End Sub

    Private Sub mnuReportAsteroidRocks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportAsteroidRocks.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateRockReport()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "OreReport.html"))
        DisplayReport(newReport, "Asteroid Composition")
    End Sub

    Private Sub mnuReportAsteroidIce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReportAsteroidIce.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateIceReport()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "IceReport.html"))
        DisplayReport(newReport, "Ice Composition")
    End Sub

#End Region

    Private Sub EveIconMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles EveIconMenu.Opening

        ' Hide the tooltip form
        If EveHQTrayForm IsNot Nothing Then
            EveHQTrayForm.Close()
            EveHQTrayForm = Nothing
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(1) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(1), "Eve.exe")) = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(1) = False Then
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
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(2) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(2), "Eve.exe")) = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(2) = False Then
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
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(3) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(3), "Eve.exe")) = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(3) = False Then
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
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(4) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(4), "Eve.exe")) = True And EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(4) = False Then
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
        End If

    End Sub

    Private Sub mnuBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuBackup.Click
        Call Me.OpenBackUpForm()
    End Sub

    Private Sub mnuEveHQBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEveHQBackup.Click
        Call Me.OpenEveHQBackupForm()
    End Sub

    Private Sub DashboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DashboardToolStripMenuItem.Click
        Call Me.OpenDashboard()
    End Sub

    Private Sub mnuToolsAPIChecker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsAPIChecker.Click
        Call Me.OpenAPICheckerForm()
    End Sub

    Private Sub mnuToolsMarketPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsMarketPrices.Click
        Call Me.OpenMarketPricesForm()
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
        If EveHQBackupWorker.IsBusy = False Then
            If EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode = 2 Then
                Dim nextBackup As Date = EveHQ.Core.HQ.EveHQSettings.EveHQBackupStart
                If EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast > nextBackup Then
                    nextBackup = EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast
                End If
                nextBackup = DateAdd(DateInterval.Day, EveHQ.Core.HQ.EveHQSettings.EveHQBackupFreq, nextBackup)
                If Now >= nextBackup Then
                    EveHQBackupWorker.RunWorkerAsync()
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
            tsAPIStatus.Text = "Eve Settings Backup Successful: " & FormatDateTime(EveHQ.Core.HQ.EveHQSettings.BackupLast, DateFormat.GeneralDate)
        Else
            tsAPIStatus.Text = "Eve Settings Backup Aborted - No Source Folders"
        End If
    End Sub

    Private Sub EveHQBackupWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles EveHQBackupWorker.DoWork
        Call frmBackupEveHQ.BackupEveHQSettings()
    End Sub
    Private Sub EveHQBackupWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles EveHQBackupWorker.RunWorkerCompleted
        Call frmBackupEveHQ.CalcNextBackup()
        Call frmBackupEveHQ.ScanBackups()
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupLastResult = -1 Then
            tsAPIStatus.Text = "EveHQ Settings Backup Successful: " & FormatDateTime(EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast, DateFormat.GeneralDate)
        Else
            tsAPIStatus.Text = "EveHQ Settings Backup Failed!"
        End If
    End Sub

#End Region

    Private Sub mnuHelpAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpAbout.Click
        frmAbout.ShowDialog()
    End Sub

    Private Sub mnuHelpCheckUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpCheckUpdates.Click
        Call Me.ShowUpdateForm()
    End Sub

    Private Sub VersionHistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VersionHistoryToolStripMenuItem.Click
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "History.txt"))
        sw.Write(My.Resources.History)
        sw.Flush()
        sw.Close()
        Dim newReport As New frmReportViewer
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "History.txt"))
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
                    Dim t As New Thread(AddressOf Me.RunModuleStartUps)
                    t.IsBackground = True
                    t.Start(PlugInInfo)
                    'ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
                End If
            ElseIf PlugInInfo.Available = True And PlugInInfo.Disabled = True Then
                ' Check for initialisation from a parameter
                If PlugInInfo.PostStartupData IsNot Nothing Then
                    Dim msg As String = PlugInInfo.Name & " is not configured to run at startup but EveHQ was started with data specifcally for that Plug-In." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Would you like to initialise the Plug-in so the data can be viewed?"
                    If MessageBox.Show(msg, "Confirm Load Plug-In", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
                    End If
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
    Public Sub RunModuleStartUps(ByVal State As Object)
        Dim plugInInfo As EveHQ.Core.PlugIn = CType(State, EveHQ.Core.PlugIn)
        Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
        Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
        plugInInfo.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn = plugInInfo.Instance
        Dim modMenu As ToolStripMenuItem = CType(mnuModules.DropDownItems(plugInInfo.Name), ToolStripMenuItem)
        Dim tsbMenu As ToolStripButton = CType(EveHQToolStrip.Items(plugInInfo.Name), ToolStripButton)
        modMenu.DropDownItems(0).Enabled = False
        modMenu.DropDownItems(1).Enabled = False
        tsbMenu.BackColor = Color.FromArgb(255, 255, 255, 110)
        plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Loading
        Try
            Dim PlugInResponse As String = ""
            PlugInResponse = runPlugIn.EveHQStartUp().ToString
            If CBool(PlugInResponse) = False Then
                modMenu.DropDownItems(0).Enabled = True
                modMenu.DropDownItems(1).Enabled = False
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Failed
                tsbMenu.BackColor = Color.FromKnownColor(KnownColor.Control)
                tsbMenu.Enabled = False
            Else
                Dim hitError As Boolean = False
                Do
                    hitError = False
                    Try
                        If EveHQToolStrip.Items.ContainsKey(plugInInfo.Name) = True Then
                            tsbMenu.BackColor = Color.FromKnownColor(KnownColor.Control)
                            tsbMenu.Enabled = True
                        End If
                    Catch e As Exception
                        hitError = True
                    End Try
                Loop Until hitError = False
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
                modMenu.DropDownItems(0).Enabled = False
                modMenu.DropDownItems(1).Enabled = True
            End If
            ' Clean up after loading the plugin
            Call EveHQ.Core.HQ.ReduceMemory()
            ' Check if we should open the plug-in by reference to any PostLoadData
            If plugInInfo.PostStartupData IsNot Nothing Then
                ' Open the Plug-in
                Dim myDelegate As New OpenPlugInDelegate(AddressOf OpenPlugIn)
                Me.Invoke(myDelegate, New Object() {plugInInfo.Name})
            End If
        Catch et As ThreadAbortException
            modMenu.DropDownItems(0).Enabled = True
            modMenu.DropDownItems(1).Enabled = False
        Catch ex As Exception
            MessageBox.Show("Unable to load plugin: " & plugInInfo.Name & ControlChars.CrLf & ex.Message, "Plugin error")
            modMenu.DropDownItems(0).Enabled = True
            modMenu.DropDownItems(1).Enabled = False
        End Try
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
                    Dim newModuleMenu As ToolStripMenuItem = New ToolStripMenuItem(PlugInInfo.MainMenuText, Nothing, Nothing, PlugInInfo.Name)
                    Dim newTSBItem As ToolStripButton = New ToolStripButton(PlugInInfo.MainMenuText, PlugInInfo.MenuImage, New System.EventHandler(AddressOf ModuleTSBClick), PlugInInfo.Name)
                    newTSBItem.ToolTipText = PlugInInfo.MainMenuText & ": " & PlugInInfo.Description
                    newTSBItem.AutoSize = False
                    newTSBItem.Width = 36 : newTSBItem.Height = 36
                    newTSBItem.DisplayStyle = ToolStripItemDisplayStyle.Image
                    newTSBItem.ImageScaling = ToolStripItemImageScaling.None
                    AddHandler newTSBItem.DoubleClick, AddressOf Me.mnuUpdate_Click
                    newModuleMenu.Text = PlugInInfo.MainMenuText
                    newModuleMenu.Image = PlugInInfo.MenuImage
                    newModuleMenu.Tag = PlugInInfo.Name
                    newModuleMenu.DropDownItems.Add("Load", Nothing, AddressOf Me.LoadPlugin)
                    newModuleMenu.DropDownItems.Add("Run", Nothing, AddressOf Me.RunPlugin)
                    newModuleMenu.DropDownItems(0).Name = PlugInInfo.Name
                    newModuleMenu.DropDownItems(1).Name = PlugInInfo.Name
                    If PlugInInfo.RunAtStartup = True Then
                        newModuleMenu.DropDownItems(0).Enabled = True
                        newModuleMenu.DropDownItems(1).Enabled = False
                        newTSBItem.Enabled = False
                        PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                    Else
                        If PlugInInfo.Disabled = False Then
                            newModuleMenu.DropDownItems(0).Enabled = False
                            newModuleMenu.DropDownItems(1).Enabled = True
                            newTSBItem.Enabled = True
                            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
                        Else
                            newModuleMenu.DropDownItems(0).Enabled = True
                            newModuleMenu.DropDownItems(1).Enabled = False
                            newTSBItem.Enabled = False
                            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                        End If
                    End If
                    mnuModules.DropDownItems.Add(newModuleMenu)
                    ' Only add if we have an image
                    If PlugInInfo.MenuImage IsNot Nothing Then
                        EveHQToolStrip.Items.Add(newTSBItem)
                    End If
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
    Private Sub LoadPlugin(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim PlugInInfo As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins.Item(mnu.Name), Core.PlugIn)
        If PlugInInfo.RunAtStartup = True Then
            ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
        Else
            Dim modMenu As ToolStripMenuItem = CType(mnuModules.DropDownItems(PlugInInfo.Name), ToolStripMenuItem)
            If EveHQToolStrip.Items.ContainsKey(PlugInInfo.Name) = True Then
                Dim tsbMenu As ToolStripButton = CType(EveHQToolStrip.Items(PlugInInfo.Name), ToolStripButton)
                tsbMenu.Enabled = True
            End If
            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
            modMenu.DropDownItems(0).Enabled = False
            modMenu.DropDownItems(1).Enabled = True
        End If
    End Sub
    Private Sub RunPlugin(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
    Public Sub LoadAndOpenPlugIn(ByVal State As Object)
        ' Called usually from an instance
        Dim plugInInfo As EveHQ.Core.PlugIn = CType(State, EveHQ.Core.PlugIn)
        Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
        Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
        plugInInfo.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn = plugInInfo.Instance
        Dim modMenu As ToolStripMenuItem = CType(mnuModules.DropDownItems(plugInInfo.Name), ToolStripMenuItem)
        Dim tsbMenu As ToolStripButton = CType(EveHQToolStrip.Items(plugInInfo.Name), ToolStripButton)
        modMenu.DropDownItems(0).Enabled = False
        modMenu.DropDownItems(1).Enabled = False
        tsbMenu.BackColor = Color.FromArgb(255, 255, 255, 110)
        plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Loading
        Try
            Dim PlugInResponse As String = ""
            PlugInResponse = runPlugIn.EveHQStartUp().ToString
            If CBool(PlugInResponse) = False Then
                modMenu.DropDownItems(0).Enabled = True
                modMenu.DropDownItems(1).Enabled = False
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Failed
            Else
                If EveHQToolStrip.Items.ContainsKey(plugInInfo.Name) = True Then
                    tsbMenu.BackColor = Color.FromKnownColor(KnownColor.Control)
                    tsbMenu.Enabled = True
                End If
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
                modMenu.DropDownItems(0).Enabled = False
                modMenu.DropDownItems(1).Enabled = True
            End If
            ' Clean up after loading the plugin
            Call EveHQ.Core.HQ.ReduceMemory()
            ' Open the Plug-in
            Dim myDelegate As New OpenPlugInDelegate(AddressOf OpenPlugIn)
            Me.Invoke(myDelegate, New Object() {plugInInfo.Name})
        Catch ex As Exception
            MessageBox.Show("Unable to load plugin: " & plugInInfo.Name & ControlChars.CrLf & ex.Message, "Plugin error")
            modMenu.DropDownItems(0).Enabled = True
            modMenu.DropDownItems(1).Enabled = False
        End Try
    End Sub

    Delegate Sub OpenPlugInDelegate(ByVal PlugInName As String)
    Private Sub OpenPlugIn(ByVal PlugInName As String)
        Dim PlugInInfo As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PlugInName), Core.PlugIn)
        If PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
            Dim mainTab As TabControl = CType(EveHQ.Core.HQ.MainForm.Controls("tabMDI"), TabControl)
            If mainTab.TabPages.ContainsKey(PlugInInfo.Name) = True Then
                mainTab.SelectTab(PlugInInfo.Name)
            Else
                Dim plugInForm As Form = PlugInInfo.Instance.RunEveHQPlugIn
                plugInForm.MdiParent = EveHQ.Core.HQ.MainForm
                plugInForm.Show()
            End If
            PlugInInfo.Instance.GetPlugInData(PlugInInfo.PostStartupData, 0)
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
            If Not MyBase.ActiveMdiChild.WindowState = FormWindowState.Maximized Then
                MyBase.ActiveMdiChild.WindowState = FormWindowState.Maximized
            End If
            Dim tp As TabPage = TryCast(MyBase.ActiveMdiChild.Tag, TabPage)
            If (tp Is Nothing) Then
                tp = New TabPage(MyBase.ActiveMdiChild.Text)
                tp.Tag = MyBase.ActiveMdiChild
                tp.Name = MyBase.ActiveMdiChild.Text
                tp.Parent = Me.tabMDI
                tp.Show()
                MyBase.ActiveMdiChild.Tag = tp
                AddHandler MyBase.ActiveMdiChild.FormClosed, New FormClosedEventHandler(AddressOf Me.ActiveMdiChild_FormClosed)
            End If

            If Not Me.tabMDI.SelectedTab.Equals(tp) Then
                Me.tabMDI.SelectedTab = tp
            End If
            If Not Me.tabMDI.Visible Then
                Me.tabMDI.Visible = True
            End If
        End If
    End Sub
    Private Sub ActiveMdiChild_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        TryCast(TryCast(sender, Form).Tag, TabPage).Dispose()
        TryCast(sender, Form).Dispose()
    End Sub
    Public Sub OpenPilotInfoForm()
        If tabMDI.TabPages.ContainsKey(frmPilot.Text) = False Then
            frmPilot.MdiParent = Me
            frmPilot.Show()
        Else
            tabMDI.SelectTab(frmPilot.Text)
        End If
    End Sub
    Public Sub OpenSkillTrainingForm()
        If tabMDI.TabPages.ContainsKey(frmTraining.Text) = False Then
            frmTraining.MdiParent = Me
            frmTraining.Show()
        Else
            tabMDI.SelectTab(frmTraining.Text)
        End If
    End Sub
    Public Sub OpenEveHQMailForm()
        If tabMDI.TabPages.ContainsKey(frmMail.Text) = False Then
            frmMail.MdiParent = Me
            frmMail.Show()
        Else
            tabMDI.SelectTab(frmMail.Text)
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
    Private Sub OpenEveHQBackUpForm()
        If tabMDI.TabPages.ContainsKey(frmBackupEveHQ.Text) = False Then
            frmBackupEveHQ.MdiParent = Me
            frmBackupEveHQ.Show()
        Else
            tabMDI.SelectTab(frmBackupEveHQ.Text)
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
    Private Sub OpenDashboard()
        If tabMDI.TabPages.ContainsKey(frmDashboard.Text) = False Then
            frmDashboard.MdiParent = Me
            frmDashboard.Show()
        Else
            tabMDI.SelectTab(frmDashboard.Text)
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
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Right
                Dim TabIndex As Integer
                ' Get index of tab clicked
                TabIndex = TabControlHitTest(tabMDI, e.Location)
                ' If a tab was clicked display it's index
                If TabIndex >= 0 Then
                    tabMDI.Tag = TabIndex
                    Dim tp As TabPage = tabMDI.TabPages(CInt(tabMDI.Tag))
                    mnuCloseMDITab.Text = "Close " & tp.Text
                End If
            Case Windows.Forms.MouseButtons.Middle
                Dim TabIndex As Integer
                ' Get index of tab clicked
                TabIndex = TabControlHitTest(tabMDI, e.Location)
                ' If a tab was clicked display it's index
                If TabIndex >= 0 Then
                    tabMDI.Tag = TabIndex
                    Dim tp As TabPage = tabMDI.TabPages(CInt(tabMDI.Tag))
                    TryCast(tp.Tag, Form).Close()
                End If
        End Select
    End Sub

#End Region

    Private Sub RemoteRefreshPilots()
        Call Me.UpdatePilotInfo()
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
            Process.Start(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(1), "Eve.exe"))
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub ctxLaunchEve2Normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve2Normal.Click
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(2), "Eve.exe"))
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub ctxLaunchEve3Normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve3Normal.Click
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(3), "Eve.exe"))
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub ctxLaunchEve4Normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxLaunchEve4Normal.Click
        Me.WindowState = FormWindowState.Minimized
        Try
            Process.Start(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(4), "Eve.exe"))
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
            Process.Start(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(EveFolder), "Eve.exe"))
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
                    Dim EvePath As String = Path.Combine(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(CInt(tmrEveWindow.Tag)), "bin"), "Exefile.exe")
                    If process.MainModule.FileName.ToUpper = EvePath.ToUpper Then
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

#Region "Cache Clearing Routines"

    Private Sub mnuClearAllXMLCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClearAllXMLCache.Click
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

    Private Sub mnuClearCharacterXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClearCharacterXML.Click
        Dim msg As String = "This will delete the character specific XML files, clear the pilot data and reconnect to the API." & ControlChars.CrLf & ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Cache", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Close all open forms
                For Each tp As TabPage In tabMDI.TabPages
                    TryCast(tp.Tag, Form).Close()
                Next

                ' Clear the character XML files
                Try
                    For Each charFile As String In My.Computer.FileSystem.GetFiles(EveHQ.Core.HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly, "EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the skill training XML files
                Try
                    For Each charFile As String In My.Computer.FileSystem.GetFiles(EveHQ.Core.HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly, "EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.SkillTraining.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the skill queue XML files
                Try
                    For Each charFile As String In My.Computer.FileSystem.GetFiles(EveHQ.Core.HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly, "EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.SkillQueue.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the EveHQ Pilot Data
                Try
                    EveHQ.Core.HQ.EveHQSettings.Pilots.Clear()
                    EveHQ.Core.HQ.TPilots.Clear()
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

    Private Sub mnuClearImageCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuClearImageCache.Click
        Dim msg As String = "This will delete the entire contents of the image cache folder." & ControlChars.CrLf & ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Image Cache", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Clear the EveHQ image cache
                Try
                    If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.imageCacheFolder) Then
                        My.Computer.FileSystem.DeleteDirectory(EveHQ.Core.HQ.imageCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                Catch ex As Exception
                End Try

                ' Recreate the EveHQ image cache folder
                Try
                    If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.imageCacheFolder) = False Then
                        My.Computer.FileSystem.CreateDirectory(EveHQ.Core.HQ.imageCacheFolder)
                    End If
                Catch ex As Exception
                End Try

            Catch ex As Exception
                MessageBox.Show("Error Deleting the EveHQ Image Cache Folder, please try to delete the following location manually: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.imageCacheFolder, "Error Deleting Cache", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End If
    End Sub

#End Region

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
        Dim msg As New StringBuilder
        msg.Append("EveHQ has detected that there is an error in the character cache files. ")
        msg.AppendLine("This could be due to a corrupt cache file or a conflict with another skill training application.")
        msg.AppendLine("")
        msg.AppendLine("The issue may be resolved by clearing the EveHQ cache and connecting back to the API. Would you like to do this now?")
        msg.AppendLine("")

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
        Dim MLFolder As String = Path.Combine(Path.Combine(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eve"), "logs"), "Marketlogs")
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
            If priceData IsNot Nothing Then
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

    Private Sub tmrMemory_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMemory.Tick
        Call EveHQ.Core.HQ.ReduceMemory()
    End Sub

#Region "Update Check & Menu"
    Private Sub CheckForUpdates(ByVal state As Object)
        Dim DatabaseUpgradeAvailable As Boolean = False
        Dim CurrentComponents As New SortedList
        Dim UpdateXML As XmlDocument = FetchUpdateXML()
        If UpdateXML Is Nothing Then
            Exit Sub
        Else
            Dim UpdateRequired As Boolean = False
            ' Get a current list of components
            CurrentComponents.Clear()
            Dim msg As String = ""
            For Each myAssembly As AssemblyName In Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                If CurrentComponents.Contains(myAssembly.Name & ".dll") = False Then
                    If myAssembly.Name.StartsWith("EveHQ") Then
                        CurrentComponents.Add(myAssembly.Name & ".dll", myAssembly.Version.ToString)
                        CurrentComponents.Add(myAssembly.Name & ".pdb", myAssembly.Version.ToString)
                    End If
                End If
            Next
            ' Add to that a list of the plug-ins used
            For Each myPlugIn As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
                If myPlugIn.ShortFileName IsNot Nothing Then
                    If CurrentComponents.Contains(myPlugIn.ShortFileName) = False Then
                        CurrentComponents.Add(myPlugIn.ShortFileName, myPlugIn.Version)
                        CurrentComponents.Add(System.IO.Path.GetFileNameWithoutExtension(myPlugIn.FileName) & ".pdb", myPlugIn.Version)
                    End If
                End If
            Next
            ' Add the current executable!
            CurrentComponents.Add("EveHQ.exe", My.Application.Info.Version.ToString)
            CurrentComponents.Add("EveHQ.pdb", My.Application.Info.Version.ToString)
            ' Add the LgLcd.dll - unique as not a .Net assembly
            If My.Computer.FileSystem.FileExists(Path.Combine(Application.StartupPath, "LgLcd.dll")) = True Then
                CurrentComponents.Add("LgLcd.dll", "Any")
            Else
                CurrentComponents.Add("LgLcd.dll", "Not Present")
            End If
            ' Try and add the database version (if using Access)
            If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 Then
                Dim databaseData As Data.DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM EveHQVersion;")
                If databaseData IsNot Nothing Then
                    If databaseData.Tables(0).Rows.Count > 0 Then
                        CurrentComponents.Add("EveHQ.mdb.zip", databaseData.Tables(0).Rows(0).Item("Version").ToString)
                    Else
                        CurrentComponents.Add("EveHQ.mdb.zip", "1.0.0.0")
                    End If
                Else
                    CurrentComponents.Add("EveHQ.mdb.zip", "1.0.0.0")
                End If
            End If

            ' Try parsing the update file 
            Try
                Dim updateDetails As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/lastUpdated")
                Dim lastUpdate As String = updateDetails(0).InnerText
                Dim requiredFiles As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/files/file")
                For Each updateFile As XmlNode In requiredFiles
                    ' Check if the plug-in is available
                    If CStr(CurrentComponents.Item(updateFile.ChildNodes(0).InnerText)) IsNot Nothing Then
                        ' Check which is the later version
                        If IsUpdateAvailable(CStr(CurrentComponents.Item(updateFile.ChildNodes(0).InnerText)), updateFile.ChildNodes(2).InnerText) = True Then
                            UpdateRequired = True
                        End If
                    Else
                        If updateFile.ChildNodes(0).InnerText <> "EveHQ.mdb.zip" Or (updateFile.ChildNodes(0).InnerText = "EveHQ.mdb.zip" And EveHQ.Core.HQ.EveHQSettings.DBFormat = 0) Then
                            UpdateRequired = True
                        End If
                    End If
                Next
                If UpdateRequired = True Then
                    mnuUpdate.Enabled = True
                    Dim reply As Integer = MessageBox.Show("There are updates available. Would you like to update EveHQ now?", "Update EveHQ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = DialogResult.No Then
                        Exit Sub
                    Else
                        Me.Invoke(New MethodInvoker(AddressOf Me.ShowUpdateForm))
                    End If
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub
    Private Sub ShowUpdateForm()
        Dim myUpdater As New frmUpdater
        If Screen.PrimaryScreen.Bounds.Height <= 650 Then
            myUpdater.Height = Screen.PrimaryScreen.Bounds.Height - 100
        End If
        myUpdater.Show()
    End Sub
    Private Function IsUpdateAvailable(ByVal localVer As String, ByVal remoteVer As String) As Boolean
        If localVer = "Not Used" Then
            Return False
        Else
            If localVer = remoteVer Then
                Return False
            Else
                Dim localVers() As String = localVer.Split(CChar("."))
                Dim remoteVers() As String = remoteVer.Split(CChar("."))
                Dim requiresUpdate As Boolean = False
                For ver As Integer = 0 To 3
                    If CInt(remoteVers(ver)) <> CInt(localVers(ver)) Then
                        If CInt(remoteVers(ver)) > CInt(localVers(ver)) Then
                            requiresUpdate = True
                            Exit For
                        Else
                            requiresUpdate = False
                            Exit For
                        End If
                    End If
                Next
                Return requiresUpdate
            End If
        End If
    End Function
    Private Function FetchUpdateXML() As XmlDocument
        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)
        Dim UpdateServer As String = EveHQ.Core.HQ.EveHQSettings.UpdateURL
        Dim remoteURL As String = UpdateServer & "/_updates.xml"
        Dim webdata As String = ""
        Dim UpdateXML As New XmlDocument
        Try
            ' Create the requester
            ServicePointManager.DefaultConnectionLimit = 10
            ServicePointManager.Expect100Continue = False
            Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(remoteURL))
            Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
            request.UserAgent = "EveHQ Updater " & My.Application.Info.Version.ToString
            request.CachePolicy = policy
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                request.Proxy = EveHQProxy
            End If
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            webdata = readStream.ReadToEnd()
            ' Check response string for any error codes?
            UpdateXML.LoadXml(webdata)
            Return UpdateXML
        Catch e As Exception
            Return Nothing
        End Try
    End Function
    Private Sub mnuUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUpdate.Click
        Dim myUpdater As New frmUpdater
        myUpdater.ShowDialog()
    End Sub
    Private Sub mnuUpdateNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuUpdateNow.Click
        Call Me.UpdateNow()
        Me.Close()
    End Sub
    Private Sub UpdateNow()
        ' Try and download patchfile
        Dim PatcherLocation As String = EveHQ.Core.HQ.appDataFolder

        Dim patcherFile As String = Path.Combine(PatcherLocation, "EveHQPatcher.exe")
        Try
            Call Me.DownloadPatcherFile("EveHQPatcher.exe")
            ' Copy the CoreControls.dll file to the same location
            Dim oldCCfile As String = Path.Combine(EveHQ.Core.HQ.appFolder, "EveHQ.CoreControls.dll")
            Dim newCCfile As String = Path.Combine(PatcherLocation, "EveHQ.CoreControls.dll")
            My.Computer.FileSystem.CopyFile(oldCCfile, newCCfile, True)
            'MessageBox.Show("Patcher Deployment Successful!", "Patcher Deployment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch Excep As System.Runtime.InteropServices.COMException
            Dim errMsg As String = "Unable to copy Patcher to " & ControlChars.CrLf & ControlChars.CrLf & patcherFile & ControlChars.CrLf & ControlChars.CrLf
            errMsg &= "Please make sure this file is in the EveHQ program directory before continuing."
            MessageBox.Show(errMsg, "Error Copying Patcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End Try
        Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
        startInfo.UseShellExecute = True
        startInfo.WorkingDirectory = Environment.CurrentDirectory
        startInfo.FileName = patcherFile
        Dim args As String = " /App;" & ControlChars.Quote & EveHQ.Core.HQ.appFolder & ControlChars.Quote
        If EveHQ.Core.HQ.IsUsingLocalFolders = True Then
            args &= " /Local;True"
        Else
            args &= " /Local;False"
        End If
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = 0 Then
            args &= " /DB;" & ControlChars.Quote & EveHQ.Core.HQ.EveHQSettings.DBFilename & ControlChars.Quote
        Else
            args &= " /DB;None"
        End If
        startInfo.Arguments = args
        Dim osInfo As OperatingSystem = System.Environment.OSVersion
        If osInfo.Version.Major > 5 Then
            startInfo.Verb = "runas"
        End If
        Process.Start(startInfo)
        EveHQ.Core.HQ.StartShutdownEveHQ = True
        Me.Close()
    End Sub

    Private Function DownloadPatcherFile(ByVal FileNeeded As String) As Boolean

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        Dim httpURI As String = EveHQ.Core.HQ.EveHQSettings.UpdateURL & "/" & FileNeeded
        Dim localFile As String = Path.Combine(EveHQ.Core.HQ.appDataFolder, FileNeeded)

        ' Create the request to access the server and set credentials
        ServicePointManager.DefaultConnectionLimit = 10
        ServicePointManager.Expect100Continue = False
        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(httpURI))
        Dim request As HttpWebRequest = CType(HttpWebRequest.Create(httpURI), HttpWebRequest)
        request.CachePolicy = policy
        ' Setup proxy server (if required)
        If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
            Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
            If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                EveHQProxy.UseDefaultCredentials = True
            Else
                EveHQProxy.UseDefaultCredentials = False
                EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
            End If
            request.Proxy = EveHQProxy
        End If
        request.CachePolicy = policy
        request.Method = WebRequestMethods.File.DownloadFile
        request.Timeout = 900000
        Try
            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
                Dim filesize As Long = CLng(response.ContentLength)
                Using responseStream As IO.Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New IO.FileStream(localFile, IO.FileMode.Create)
                        Dim buffer(16383) As Byte
                        Dim read As Integer = 0
                        Dim totalBytes As Long = 0
                        Dim percent As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                            totalBytes += read
                            percent = CInt(totalBytes / filesize * 100)
                        Loop Until read = 0 'see Note(1)
                        responseStream.Close()
                        fs.Flush()
                        fs.Close()
                    End Using
                    responseStream.Close()
                End Using
                response.Close()
            End Using
            Return True
        Catch e As WebException
            Dim errMsg As String = "An error has occurred:" & ControlChars.CrLf
            errMsg &= "Status: " & e.Status & ControlChars.CrLf
            errMsg &= "Message: " & e.Message & ControlChars.CrLf
            MessageBox.Show(errMsg, "Error Downloading Patcher File", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End Try

    End Function
#End Region

#Region "Eve Mail Timer Functions"
    Private Sub tmrEveMail_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrEveMail.Tick
        Call Me.UpdateEveMailButton()
    End Sub

    Public Sub UpdateEveMailButton()
        ' Get a list of the mail messages that are unread
        tmrEveMail.Interval = 600000
        Dim strSQL As String = "SELECT COUNT(*) FROM eveMail WHERE readMail=0;"
        Dim mailData As Data.DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        Dim unreadMail As Integer = 0
        Dim unreadNotices As Integer = 0
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                unreadMail = CInt(mailData.Tables(0).Rows(0).Item(0))
            End If
        End If
        If unreadMail > 0 Or unreadNotices > 0 Then
            tsbMail.Text = unreadMail.ToString & "/" & unreadNotices.ToString
            tsbMail.BackColor = Color.Lime
        Else
            tsbMail.Text = ""
            tsbMail.BackColor = Color.Transparent
        End If
        tsbMail.ToolTipText = "View Mail & Notifications" & ControlChars.CrLf & "(" & unreadMail.ToString & " mails / " & unreadNotices.ToString & " notices)"
    End Sub
#End Region

End Class

