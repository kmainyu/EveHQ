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
Imports DevComponents.DotNetBar

Public Class frmEveHQ
    Dim WithEvents eveTQWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents IGBWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents SkillWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents ImportWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents BackupWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents EveHQBackupWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim WithEvents ReportWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Private Delegate Sub QueryMyEveServerDelegate()
    Dim EveHQMLW As New SortedList
    Dim EveHQMLF As New frmMarketPrices
    Dim appStartUp As Boolean = True
    Private EveHQTrayForm As Form = Nothing

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
            ' Set the training bar position, after checking for null!
            If EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = DevComponents.DotNetBar.eDockSide.None Then
                EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = DevComponents.DotNetBar.eDockSide.Bottom
            End If
            Me.Bar1.DockSide = CType(EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition, DevComponents.DotNetBar.eDockSide)
            DockContainerItem1.Height = EveHQ.Core.HQ.EveHQSettings.TrainingBarHeight
            DockContainerItem1.Width = EveHQ.Core.HQ.EveHQSettings.TrainingBarWidth
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
    Private Sub ctxExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxExit.Click
        Call Me.ShutdownRoutine()
    End Sub
    Private Sub ctxAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxAbout.Click
        If frmAbout.Visible = False Then
            frmAbout.ShowDialog()
        End If
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
        lblEveTime.Text = "EVE Time: " & now.ToString(fi.ShortDatePattern + " HH:mm")
    End Sub
    Private Sub eveTQWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles eveTQWorker.DoWork
        ' Defines what work the thread has to do
        Call EveHQ.Core.HQ.myTQServer.GetServerStatus()
    End Sub
    Private Sub eveTQWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles eveTQWorker.RunWorkerCompleted
        ' Sub raised on the completion of a call to read the Eve TQ data

        ' Check if the server status has changed since the last result and notify user
        If EveHQ.Core.HQ.myTQServer.Status <> EveHQ.Core.HQ.myTQServer.LastStatus Then

            ' Depending on server status, set the notify icon text and the statusbar text
            Select Case EveHQ.Core.HQ.myTQServer.Status
                Case EveHQ.Core.EveServer.ServerStatus.Down
                    'EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Unable to connect to server"
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_offline IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_offline
                    End If
                Case EveHQ.Core.EveServer.ServerStatus.Starting
                    EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_starting IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_starting
                    End If
                Case EveHQ.Core.EveServer.ServerStatus.Shutting
                    EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_starting IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_starting
                    End If
                Case EveHQ.Core.EveServer.ServerStatus.Full
                    EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_online IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_online
                    End If
                Case EveHQ.Core.EveServer.ServerStatus.ProxyDown
                    lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                    EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_offline IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_offline
                    End If
                Case EveHQ.Core.EveServer.ServerStatus.Unknown
                    EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Status unknown"
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ
                    End If
                Case EveHQ.Core.EveServer.ServerStatus.Up
                    lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Online (" & EveHQ.Core.HQ.myTQServer.Players & " Players)"
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_online IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_online
                    End If
            End Select

            If EveStatusIcon IsNot Nothing Then
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
        Else
            ' Report the players regardless
            If EveHQ.Core.HQ.myTQServer.Status = EveHQ.Core.EveServer.ServerStatus.Up Then
                lblTQStatus.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Online (" & EveHQ.Core.HQ.myTQServer.Players & " Players)"
            End If
        End If
        ' Update last status
        EveHQ.Core.HQ.myTQServer.LastStatus = EveHQ.Core.HQ.myTQServer.Status

    End Sub

#End Region

#Region "Form Opening & Closing & Resizing (+ Icon)"

    Private Sub frmEveHQ_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' Check we aren't updating
        If EveHQ.Core.HQ.EveHQIsUpdating = True Then
            MessageBox.Show("You can't exit EveHQ while an update is in progress. Please wait until the update has completed and try again.", "Update in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
            e.Cancel = True
            Exit Sub
        End If

        Try

            EveHQ.Core.HQ.WriteLogEvent("Shutdown: EveHQ Form Closure request made")

            ' Are we shutting down to restore settings?
            If EveHQ.Core.HQ.RestoredSettings = False Then
                ' Check if we should minimise rather than exit?
                If e.CloseReason <> CloseReason.TaskManagerClosing And e.CloseReason <> CloseReason.WindowsShutDown Then
                    If EveHQ.Core.HQ.EveHQSettings.MinimiseExit = True Then
                        Me.WindowState = FormWindowState.Minimized
                        EveHQ.Core.HQ.WriteLogEvent("Shutdown: EveHQ Form Closure aborted due to 'Minimise on Exit' setting")
                        e.Cancel = True
                        Exit Sub
                    Else
                        ' Check if there are updates available
                        If EveHQ.Core.HQ.AppUpdateAvailable = True Then
                            Dim msg As String = "There are pending updates available - these will be installed now."
                            MessageBox.Show(msg, "Update Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Call Me.UpdateNow()
                        Else
                            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Calling main shutdown routine")
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
                        EveHQ.Core.HQ.WriteLogEvent("Shutdown: Calling main shutdown routine")
                        Call Me.ShutdownRoutine()
                    End If
                End If
            Else
                ' Close and flush the timer file
                Try
                    EveHQ.Core.HQ.EveHQLogFile.Flush()
                    EveHQ.Core.HQ.EveHQLogFile.Close()
                Catch ex As Exception
                    ' Do nothing?
                End Try
            End If
        Catch ex As Exception
            MessageBox.Show("An error occured while closing EveHQ: " & ex.Message & "- " & ex.StackTrace, "Error Closing EveHQ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub frmEveHQ_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Hide()

        ' Disable resizing of bar
        appStartUp = True

        Me.EveStatusIcon.Visible = True

        ' Set Theme Stuff
        DevComponents.DotNetBar.StyleManager.ChangeStyle(EveHQ.Core.HQ.EveHQSettings.ThemeStyle, EveHQ.Core.HQ.EveHQSettings.ThemeTint)
        Dim ThemeBtn As DevComponents.DotNetBar.ButtonItem = CType(btnTheme.SubItems("btn" & EveHQ.Core.HQ.EveHQSettings.ThemeStyle.ToString), DevComponents.DotNetBar.ButtonItem)
        ThemeBtn.Checked = True

        ' Add the pilot refresh handler
        AddHandler EveHQ.Core.PilotParseFunctions.RefreshPilots, AddressOf Me.RemoteRefreshPilots
        AddHandler EveHQ.Core.G15LCDv2.UpdateAPI, AddressOf Me.RemoteUpdate
        AddHandler EveHQ.Core.HQ.ShutDownEveHQ, AddressOf Me.ShutdownRoutine
        AddHandler EveHQ.Core.EveMailEvents.MailUpdateNumbers, AddressOf Me.UpdateEveMailButton

        ' Check if "Hide When Minimised" is active
        HideWhenMinimisedToolStripMenuItem.Checked = EveHQ.Core.HQ.EveHQSettings.AutoHide

        'Setup the Modules menu if applicable
        Call Me.SetupModuleMenu()

        ' Check if the IGB should be started here
        If IGBCanBeInitialised() = True Then
            If EveHQ.Core.HQ.EveHQSettings.IGBAutoStart = True Then
                If Not System.Net.HttpListener.IsSupported Then
                    btnIGB.Enabled = False
                    btnIGB.Checked = False
                Else
                    IGBWorker.WorkerSupportsCancellation = True
                    IGBWorker.RunWorkerAsync()
                    btnIGB.Checked = True
                    EveHQ.Core.HQ.IGBActive = True
                End If
            End If
        End If

        ' Create a new instance of the APIRS and check if the APIRS should be started 
        EveHQ.Core.HQ.myAPIRS = New EveHQ.EveAPI.EveAPIProxy(EveHQ.Core.HQ.EveHQSettings.APIRSPort, EveHQ.Core.HQ.cacheFolder)
        If EveHQ.Core.HQ.EveHQSettings.APIRSAutoStart = True Then
            If System.Net.HttpListener.IsSupported Then
                EveHQ.Core.HQ.myAPIRS.StartServer()
                EveHQ.Core.HQ.APIRSActive = True
            End If
        End If

        ' Set the tab position
        Select Case EveHQ.Core.HQ.EveHQSettings.MDITabPosition
            Case "Top"
                Me.tabEveHQMDI.Dock = DockStyle.Top
                Me.tabEveHQMDI.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Top
            Case "Bottom"
                Me.tabEveHQMDI.Dock = DockStyle.Bottom
                Me.tabEveHQMDI.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom
        End Select

        ' Check for ribbon status
        RibbonControl1.Expanded = Not EveHQ.Core.HQ.EveHQSettings.RibbonMinimised

        ' Close the splash screen
        frmSplash.Close()

        ' Check if the form needs to be minimised on startup
        If EveHQ.Core.HQ.EveHQSettings.AutoMinimise = True Then
            Me.WindowState = FormWindowState.Minimized
            'Me.Show()
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

        ' Update the QAT config if applicable
        If EveHQ.Core.HQ.EveHQSettings.QATLayout <> "" Then
            RibbonControl1.QatLayout = EveHQ.Core.HQ.EveHQSettings.QATLayout
        End If

        ' Start the timers
        If EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = True Then
            tmrEve.Enabled = True
            lblTQStatus.Text = "Tranquility Status: Not Updated"
        Else
            lblTQStatus.Text = "Tranquility Status: Updates Disabled"
        End If
        tmrSkillUpdate.Enabled = True
        tmrModules.Enabled = True

        Call EveHQ.Core.HQ.ReduceMemory()
        tmrMemory.Enabled = True

        ' Update the EveMailNotice button
        Call Me.UpdateEveMailButton()

        ' Update the pilots in the report
        Call Me.UpdateReportPilots()

        ' Set the training bar position, after checking for null!
        If EveHQ.Core.HQ.EveHQSettings.DisableTrainingBar = False Then
            If EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = DevComponents.DotNetBar.eDockSide.None Then
                EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = DevComponents.DotNetBar.eDockSide.Bottom
            End If
            Me.Bar1.DockSide = CType(EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition, DevComponents.DotNetBar.eDockSide)
            DockContainerItem1.Height = EveHQ.Core.HQ.EveHQSettings.TrainingBarHeight
            DockContainerItem1.Width = EveHQ.Core.HQ.EveHQSettings.TrainingBarWidth
        Else
            Me.Bar1.Visible = False
        End If

        appStartUp = False

        ' Display server message if applicable
        If EveHQ.Core.HQ.EveHQServerMessage IsNot Nothing Then
            If EveHQ.Core.HQ.EveHQServerMessage.MessageDate > EveHQ.Core.HQ.EveHQSettings.LastMessageDate Or (EveHQ.Core.HQ.EveHQServerMessage.MessageDate = EveHQ.Core.HQ.EveHQSettings.LastMessageDate And EveHQ.Core.HQ.EveHQSettings.IgnoreLastMessage = False) Then
                Dim NewMsg As New frmEveHQMessage
                NewMsg.lblMessage.Text = EveHQ.Core.HQ.EveHQServerMessage.Message
                NewMsg.lblTitle.Text = EveHQ.Core.HQ.EveHQServerMessage.MessageTitle
                EveHQ.Core.HQ.EveHQSettings.LastMessageDate = EveHQ.Core.HQ.EveHQServerMessage.MessageDate
                If EveHQ.Core.HQ.EveHQServerMessage.AllowIgnore = False Then
                    NewMsg.chkIgnore.Checked = False
                    NewMsg.chkIgnore.Enabled = False
                Else
                    NewMsg.chkIgnore.Checked = False
                    NewMsg.chkIgnore.Enabled = True
                End If
                NewMsg.ShowDialog()
            End If
        End If

        ' Check for existing pilots and accounts
        If EveHQ.Core.HQ.EveHQSettings.Accounts.Count = 0 And EveHQ.Core.HQ.EveHQSettings.Pilots.Count = 0 Then
            Dim wMsg As String = "EveHQ has detected that you have not yet setup any API accounts." & ControlChars.CrLf & ControlChars.CrLf
            wMsg &= "Would you like to do this now?"
            Dim reply As Integer = MessageBox.Show(wMsg, "Welcome to EveHQ!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                wMsg = "You can add API accounts using the 'Manage API Account' button on the ribbon bar or by going into Settings and choosing the Eve Accounts section."
                MessageBox.Show(wMsg, "API Account Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim EveHQSettings As New frmSettings
                EveHQSettings.Tag = "nodeEveAccounts"
                EveHQSettings.ShowDialog()
                EveHQSettings.Dispose()
            End If
        End If

        ' Start the update check on a new thread
        If EveHQ.Core.HQ.EveHQSettings.DisableAutoWebConnections = False Then
            Threading.ThreadPool.QueueUserWorkItem(AddressOf Me.CheckForUpdates)
        End If

    End Sub
    Private Sub frmEveHQ_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ' Determine which view to display!
        If EveHQ.Core.HQ.EveHQSettings.StartupView = "" Then
            EveHQ.Core.HQ.EveHQSettings.StartupView = "EveHQ Dashboard"
        End If
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
                ' Open the dashboard
                Call Me.OpenDashboard()
        End Select
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
    Public Sub ShutdownRoutine()

        Try

            ' Check we aren't updating
            If EveHQ.Core.HQ.EveHQIsUpdating = True Then
                MessageBox.Show("You can't exit EveHQ while an update is in progress. Please wait until the update has completed and try again.", "Update in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Disable timers
            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Disabling timers...")
            Me.tmrMemory.Stop() : Me.tmrMemory.Enabled = False
            Me.tmrEve.Stop() : Me.tmrEve.Enabled = False
            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Disabled TQ Status timer")
            Me.tmrSkillUpdate.Stop() : Me.tmrSkillUpdate.Enabled = False
            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Disabled Skill Update timer")

            ' Check if Shutdown Notification is active (only if not shutting down on request on the updater
            If EveHQ.Core.HQ.EveHQSettings.ShutdownNotify = True And EveHQ.Core.HQ.UpdateShutDownRequest = False Then
                EveHQ.Core.HQ.WriteLogEvent("Shutdown: Processing shutdown notifications")
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

            ' Close all the open tabs first
            Dim mainTab As DevComponents.DotNetBar.TabStrip = CType(EveHQ.Core.HQ.MainForm.Controls("tabEveHQMDI"), DevComponents.DotNetBar.TabStrip)
            If mainTab.Tabs.Count > 0 Then
                For tab As Integer = mainTab.Tabs.Count - 1 To 0 Step -1
                    EveHQ.Core.HQ.WriteLogEvent("Shutdown: Closing tab: " & mainTab.Tabs(tab).Text)
                    CType(mainTab.Tabs(tab).AttachedControl, Form).Close()
                Next
            End If

            ' Save the QAT config if applicable
            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Storing ribbon QAT layout")
            EveHQ.Core.HQ.EveHQSettings.QATLayout = RibbonControl1.QatLayout

            ' Check for backup warning expiry
            If EveHQ.Core.HQ.UpdateShutDownRequest = True Then
                If EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode = 1 Then
                    Dim backupDate As Date = EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast.AddDays(EveHQ.Core.HQ.EveHQSettings.EveHQBackupWarnFreq)
                    If backupDate < Now Then
                        Dim timeElapsed As TimeSpan = Now - EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast
                        Dim msg As String = "You haven't backed up your EveHQ Settings for " & timeElapsed.Days & " days. Would you like to do this now?"
                        Dim reply As Integer = MessageBox.Show(msg, "Backup EveHQ Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.Yes Then
                            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Request to backup EveHQ Settings before update")
                            Call EveHQ.Core.EveHQBackup.BackupEveHQSettings()
                        End If
                    End If
                End If
                EveHQ.Core.HQ.WriteLogEvent("Shutdown: Request to save EveHQ Settings before update")
                Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()
            Else
                If EveHQ.Core.HQ.EveHQSettings.EveHQBackupMode = 1 Then
                    EveHQ.Core.HQ.WriteLogEvent("Shutdown: Checking EveHQ backup status before exit")
                    Dim backupDate As Date = EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast.AddDays(EveHQ.Core.HQ.EveHQSettings.EveHQBackupWarnFreq)
                    If backupDate < Now Then
                        Dim timeElapsed As TimeSpan = Now - EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast
                        Dim msg As String = "You haven't backed up your EveHQ Settings for " & timeElapsed.Days & " days. Would you like to do this now?"
                        Dim reply As Integer = MessageBox.Show(msg, "Backup EveHQ Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.Yes Then
                            EveHQ.Core.HQ.WriteLogEvent("Shutdown: User accepted request to backup EveHQ Settings before exit")
                            Call EveHQ.Core.EveHQBackup.BackupEveHQSettings()
                        Else
                            EveHQ.Core.HQ.WriteLogEvent("Shutdown: User rejected request to backup EveHQ Settings before exit")
                        End If
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("Shutdown: EveHQ backup not required")
                    End If
                End If
                EveHQ.Core.HQ.WriteLogEvent("Shutdown: Request to save EveHQ Settings before exit")
                Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()
            End If

            ' Remove the icons
            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Dispose of EveHQ icons")
            EveStatusIcon.Visible = False : iconEveHQMLW.Visible = False
            iconEveHQMLW.Icon = Nothing
            iconEveHQMLW.Dispose() : EveStatusIcon.Dispose()

            EveHQ.Core.HQ.WriteLogEvent("Shutdown: Shutdown complete")
            ' Close log files
            Try
                EveHQ.Core.HQ.EveHQLogFile.Flush()
                EveHQ.Core.HQ.EveHQLogFile.Close()
            Catch ex As Exception
            End Try

            'End

        Catch e As Exception
            MessageBox.Show("An error occurred calling the shutdown routine for EveHQ: " & e.Message & " - " & e.StackTrace, "Error Closing EveHQ", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

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
        'If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True And EveHQ.Core.HQ.IsG15LCDActive = True Then
        '    Select Case EveHQ.Core.HQ.lcdCharMode
        '        Case 0
        '            Call EveHQ.Core.HQ.EveHQLCD.DrawSkillTrainingInfo(EveHQ.Core.HQ.lcdPilot)
        '        Case 1
        '            Call EveHQ.Core.HQ.EveHQLCD.DrawCharacterInfo(EveHQ.Core.HQ.lcdPilot)
        '    End Select
        'End If

        Call Me.CheckForCharAPIUpdate()

        Call Me.CheckForMailAPIUpdate()

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
                                        Dim nq As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(cPilot, sq, False, True)
                                        If sq.IncCurrentTraining = True Then
                                            If nq.Count > 1 Then
                                                For q As Integer = 1 To nq.Count - 1
                                                    If CType(nq(q), EveHQ.Core.SortedQueueItem).Done = False Then
                                                        notifyText &= sq.Name & ": " & CType(nq(q), EveHQ.Core.SortedQueueItem).Name
                                                        notifyText &= " (" & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel))
                                                        notifyText &= " to " & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel) + 1) & ")" & ControlChars.CrLf
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        Else
                                            If nq.Count > 0 Then
                                                For q As Integer = 0 To nq.Count - 1
                                                    If CType(nq(q), EveHQ.Core.SortedQueueItem).Done = False Then
                                                        notifyText &= sq.Name & ": " & CType(nq(q), EveHQ.Core.SortedQueueItem).Name
                                                        notifyText &= " (" & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel))
                                                        notifyText &= " to " & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel) + 1) & ")" & ControlChars.CrLf
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
            eveHQMail.EnableSsl = EveHQ.Core.HQ.EveHQSettings.UseSSL
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
                            Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(cPilot, pq, False, True)
                            Dim qItem As New EveHQ.Core.SortedQueueItem
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
    Private Sub CheckForCharAPIUpdate()
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
                lblCharAPITime.Text = EveHQ.Core.SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
            Else
                Dim TimeLeft As TimeSpan = EveHQ.Core.HQ.NextAutoAPITime - Now
                Dim TimeLeft2 As TimeSpan = EveHQ.Core.HQ.AutoRetryAPITime - Now
                lblCharAPITime.Text = EveHQ.Core.SkillFunctions.TimeToString(Math.Max(TimeLeft.TotalSeconds, TimeLeft2.TotalSeconds), False)
            End If
        Else
            lblCharAPITime.Text = ""
        End If
    End Sub
    Private Sub CheckForMailAPIUpdate()
        ' Check if the mail download is in progress
        If EveHQ.Core.EveMailEvents.MailIsBeingProcessed = False Then
            ' Check for an API update if applicable
            If EveHQ.Core.HQ.EveHQSettings.AutoMailAPI = True Then
                If Now > EveHQ.Core.HQ.NextAutoMailAPITime Then
                    ' Invoke the API Caller
                    Call Me.UpdateMailNotifications()
                    ' Display time until autoMailAPI download
                    Dim TimeLeft As TimeSpan = EveHQ.Core.HQ.NextAutoMailAPITime - Now
                    lblMailAPITime.Text = EveHQ.Core.SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
                Else
                    Dim TimeLeft As TimeSpan = EveHQ.Core.HQ.NextAutoMailAPITime - Now
                    lblMailAPITime.Text = EveHQ.Core.SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
                End If
            Else
                lblMailAPITime.Text = ""
            End If
        Else
            lblMailAPITime.Text = "Processing..."
        End If
    End Sub

#End Region

    Private Sub RemoteUpdate()
        Me.Invoke(New QueryMyEveServerDelegate(AddressOf QueryMyEveServer))
    End Sub

    Public Sub QueryMyEveServer()
        If EveHQ.Core.HQ.APIUpdateInProgress = False Then
            EveHQ.Core.HQ.APIUpdateInProgress = True
            btnQueryAPI.Enabled = False
            frmSettings.btnGetData.Enabled = False
            Threading.ThreadPool.QueueUserWorkItem(AddressOf StartCharacterAPIThread)
        Else
            ' Do we want to add a user message here?
            ' Maybe some form of logging to see why this would be happening?
            'MessageBox.Show("A method attempted to run the Character API Update before the previous attempt was completed. The stack trace is: " & ControlChars.CrLf & ControlChars.CrLf & System.Environment.StackTrace, "Character API Routine Duplication", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Public Sub StartCharacterAPIThread(ByVal state As Object)
        Try
            Dim curSelPilot As String = ""

            ' If we have accounts to query then get the data for them
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Count = 0 Then
                lblAPIStatus.Text = "API Status: No accounts entered into settings!! (" & Now.ToString & ")"
                Exit Sub
            Else
                lblAPIStatus.Text = "API Status: Fetching Character Data..."
                barStatus.Refresh()
                ' Clear the current list of pilots
                EveHQ.Core.HQ.TPilots.Clear()
                EveHQ.Core.HQ.TCorps.Clear()
                EveHQ.Core.HQ.APIResults.Clear()
                ' get the details for the account
                Dim CurrentAccount As New EveHQ.Core.EveAccount
                For Each CurrentAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                    If CurrentAccount.APIAccountStatus <> EveHQ.Core.APIAccountStatuses.ManualDisabled Then
                        lblAPIStatus.Text = "API Status: Updating Account '" & CurrentAccount.FriendlyName & "' (ID=" & CurrentAccount.userID & ")..."
                        barStatus.Refresh()
                        Call EveHQ.Core.PilotParseFunctions.GetCharactersInAccount(CurrentAccount)
                    End If
                Next
                Call EveHQ.Core.PilotParseFunctions.CopyTempPilotsToMain()
                Call EveHQ.Core.PilotParseFunctions.CopyTempCorpsToMain()
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
                lblAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (Errors occured - double-click for details)"
            Else
                If AllCached = True Then
                    lblAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (No new updates)"
                Else
                    lblAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (Update successful)"
                End If
            End If

            ' Save the settings
            Me.Invoke(New MethodInvoker(AddressOf EveHQ.Core.EveHQSettingsFunctions.SaveSettings))

            ' Enable the option again
            btnQueryAPI.Enabled = True
            Me.Invoke(New MethodInvoker(AddressOf ResetSettingsButton))

            ' Update data
            Me.Invoke(New MethodInvoker(AddressOf UpdatePilotInfo))

        Catch e As Exception
            Call CatchGeneralException(e)
        End Try
        ' We've finished our update routine so we can now release the flag
        EveHQ.Core.HQ.APIUpdateInProgress = False
    End Sub

    Public Sub ResetSettingsButton()
        Call frmSettings.FinaliseAPIServerUpdate()
    End Sub

    Public Sub UpdatePilotInfo(Optional ByVal startUp As Boolean = False)

        ' Setup the Training Status Bar
        Call Me.SetupTrainingStatus()

        ' Update the cbopilots in the pilot form and the training form
        If frmPilot.IsHandleCreated = True Then
            frmPilot.UpdatePilots()
        End If
        If frmTraining.IsHandleCreated = True Then
            frmTraining.UpdatePilots()
        End If
        If frmSettings.IsHandleCreated = True Then
            frmSettings.UpdatePilots()
        End If

        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count = 0 Then
            btnViewPilotInfo.Enabled = False
            btnViewSkillTraining.Enabled = False
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
            btnViewPilotInfo.Enabled = True
            btnViewSkillTraining.Enabled = True
        End If

        ' Update the dashboard if applicable
        If frmDashboard.IsHandleCreated = True Then
            Call frmDashboard.UpdateWidgets()
        End If

    End Sub

    Private Sub SetupTrainingStatus()

        If EveHQ.Core.HQ.EveHQSettings.DisableTrainingBar = False Then
            ' Setup a collection for sorting
            Dim PilotTrainingTimes As New ArrayList
            Dim TrainingAccounts As New ArrayList
            Dim DisabledAccounts As New ArrayList
            For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                ' Check for disabled accounts
                If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(cPilot.Account) Then
                    If CType(EveHQ.Core.HQ.EveHQSettings.Accounts(cPilot.Account), EveHQ.Core.EveAccount).APIAccountStatus = Core.APIAccountStatuses.Disabled Then
                        DisabledAccounts.Add(cPilot.Account)
                    Else
                        ' Check for training accounts
                        If cPilot.Training = True Then
                            Dim p As New EveHQ.Core.PilotSortTrainingTime
                            p.Name = cPilot.Name
                            p.TrainingEndTime = cPilot.TrainingEndTime
                            ' Only add active pilots!
                            If cPilot.Active = True Then
                                PilotTrainingTimes.Add(p)
                            End If
                            TrainingAccounts.Add(cPilot.Account)
                        End If
                    End If
                End If
            Next

            ' Initialise a new ClassSorter instance and add a standard SortClass (i.e. sort method)
            Dim myClassSorter As New EveHQ.Core.ClassSorter("TrainingEndTime", Core.SortDirection.Ascending)
            ' Always sort by name to handle similarly ranked items in the first sort
            myClassSorter.SortClasses.Add(New EveHQ.Core.SortClass("Name", Core.SortDirection.Ascending))
            ' Sort the class
            PilotTrainingTimes.Sort(myClassSorter)

            ' Clear old event handlers
            For c As Integer = pdc1.Controls.Count - 1 To 0 Step -1
                Dim cb As CharacterTrainingBlock = CType(pdc1.Controls(c), CharacterTrainingBlock)
                RemoveHandler cb.lblSkill.Click, AddressOf Me.TrainingStatusLabelClick
                RemoveHandler cb.lblTime.Click, AddressOf Me.TrainingStatusLabelClick
                RemoveHandler cb.lblQueue.Click, AddressOf Me.TrainingStatusLabelClick
                RemoveHandler cb.pbPilot.Click, AddressOf Me.PilotPicClick
                cb.Dispose()
            Next

            ' Initialise the x-location and clear items
            pdc1.Controls.Clear()
            Dim startloc As Integer = 0

            ' Add non-training accounts to the training bar
            For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
                If DisabledAccounts.Contains(cAccount.userID) = True Then
                    ' Build a status panel if the account is not manually disabled
                    If cAccount.APIAccountStatus <> Core.APIAccountStatuses.ManualDisabled Then
                        Dim cb As New CharacterTrainingBlock(cAccount.userID, True)
                        pdc1.Controls.Add(cb)
                        If Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Bottom Or Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Top Then
                            cb.Left = startloc
                            cb.BringToFront()
                            startloc += cb.Width + 20
                        Else
                            cb.Top = startloc
                            cb.BringToFront()
                            startloc += cb.Height + 5
                        End If
                    End If
                Else
                    If TrainingAccounts.Contains(cAccount.userID) = False Then
                        ' Only add if not a APIv2 corp account
                        If Not (cAccount.APIKeySystem = Core.APIKeySystems.Version2 And cAccount.APIKeyType = Core.APIKeyTypes.Corporation) Then
                            ' Build a status panel if the account is not manually disabled
                            If cAccount.APIAccountStatus <> Core.APIAccountStatuses.ManualDisabled Then
                                Dim cb As New CharacterTrainingBlock(cAccount.userID, True)
                                pdc1.Controls.Add(cb)
                                If Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Bottom Or Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Top Then
                                    cb.Left = startloc
                                    cb.BringToFront()
                                    startloc += cb.Width + 20
                                Else
                                    cb.Top = startloc
                                    cb.BringToFront()
                                    startloc += cb.Height + 5
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            ' Add training pilots to the training bar
            For Each cPilot As EveHQ.Core.PilotSortTrainingTime In PilotTrainingTimes
                Dim cb As New CharacterTrainingBlock(cPilot.Name, False)
                AddHandler cb.lblSkill.Click, AddressOf Me.TrainingStatusLabelClick
                AddHandler cb.pbPilot.Click, AddressOf Me.PilotPicClick
                AddHandler cb.lblTime.Click, AddressOf Me.TrainingStatusLabelClick
                AddHandler cb.lblQueue.Click, AddressOf Me.TrainingStatusLabelClick
                pdc1.Controls.Add(cb)
                If Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Bottom Or Bar1.DockSide = DevComponents.DotNetBar.eDockSide.Top Then
                    cb.Left = startloc
                    cb.BringToFront()
                    startloc += cb.Width + 20
                Else
                    cb.Top = startloc
                    cb.BringToFront()
                    startloc += cb.Height + 5
                End If
            Next
        End If
    End Sub

    Public Sub PilotPicClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim selectedPic As PictureBox = CType(sender, PictureBox)
        Call Me.OpenPilotInfoForm()
        If selectedPic.Name <> "" Then
            frmPilot.DisplayPilotName = selectedPic.Name
        End If
    End Sub

    Public Sub TrainingStatusLabelClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim selectedLabel As LinkLabel = CType(sender, LinkLabel)
        Call Me.OpenSkillTrainingForm()
        If selectedLabel.Name <> "" Then
            frmTraining.DisplayPilotName = selectedLabel.Name
        End If
    End Sub

    Private Sub EveIconMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles EveIconMenu.Opening

        ' Hide the tooltip form
        If EveHQTrayForm IsNot Nothing Then
            EveHQTrayForm.Close()
            EveHQTrayForm = Nothing
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(1) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(1), "Eve.exe")) = True Then
                If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(1) <> "" Then
                    ctxmnuLaunchEve1.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(1) & ")"
                End If
                ctxmnuLaunchEve1.Enabled = True
            Else
                ctxmnuLaunchEve1.Enabled = False
            End If
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(2) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(2), "Eve.exe")) = True Then
                If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(2) <> "" Then
                    ctxmnuLaunchEve2.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(2) & ")"
                End If
                ctxmnuLaunchEve2.Enabled = True
            Else
                ctxmnuLaunchEve2.Enabled = False
            End If
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(3) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(3), "Eve.exe")) = True Then
                If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(3) <> "" Then
                    ctxmnuLaunchEve3.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(3) & ")"
                End If
                ctxmnuLaunchEve3.Enabled = True
            Else
                ctxmnuLaunchEve3.Enabled = False
            End If
        End If

        If EveHQ.Core.HQ.EveHQSettings.EveFolder(4) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(4), "Eve.exe")) = True Then
                If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(4) <> "" Then
                    ctxmnuLaunchEve4.Text = "Launch Eve (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(4) & ")"
                End If
                ctxmnuLaunchEve4.Enabled = True
            Else
                ctxmnuLaunchEve4.Enabled = False
            End If
        End If

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
            lblAPIStatus.Text = "Eve Settings Backup Successful: " & FormatDateTime(EveHQ.Core.HQ.EveHQSettings.BackupLast, DateFormat.GeneralDate)
        Else
            lblAPIStatus.Text = "Eve Settings Backup Aborted - No Source Folders"
        End If
    End Sub

    Private Sub EveHQBackupWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles EveHQBackupWorker.DoWork
        Call EveHQ.Core.EveHQBackup.BackupEveHQSettings()
    End Sub

    Private Sub EveHQBackupWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles EveHQBackupWorker.RunWorkerCompleted
        Call EveHQ.Core.EveHQBackup.CalcNextBackup()
        If frmBackupEveHQ.IsHandleCreated = True Then
            Call frmBackupEveHQ.ScanBackups()
        End If
        If EveHQ.Core.HQ.EveHQSettings.EveHQBackupLastResult = -1 Then
            lblAPIStatus.Text = "EveHQ Settings Backup Successful: " & FormatDateTime(EveHQ.Core.HQ.EveHQSettings.EveHQBackupLast, DateFormat.GeneralDate)
        Else
            lblAPIStatus.Text = "EveHQ Settings Backup Failed!"
        End If
    End Sub

#End Region

    Private Sub IGBWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles IGBWorker.DoWork
        EveHQ.Core.HQ.myIGB.RunIGB(IGBWorker, e)
    End Sub

#Region "Background Module Loading"
    Private Sub tmrModules_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrModules.Tick
        frmEveHQ.CheckForIllegalCrossThreadCalls = False
        tmrModules.Enabled = False
        For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            ' Override settings if the remote server says so
            Dim ServerOverride As Boolean = False
            If EveHQ.Core.HQ.EveHQServerMessage IsNot Nothing Then
                If EveHQ.Core.HQ.EveHQServerMessage.DisabledPlugins.ContainsKey(PlugInInfo.Name) = True Then
                    If PlugInInfo.Version <> "" Then
                        If CompareVersions(PlugInInfo.Version, EveHQ.Core.HQ.EveHQServerMessage.DisabledPlugins(PlugInInfo.Name)) = True Then
                            ServerOverride = True
                        End If
                    End If
                End If
            End If
            If ServerOverride = False Then
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
            End If
        Next
    End Sub
    Public Sub RunModuleStartUps(ByVal State As Object)
        Dim plugInInfo As EveHQ.Core.PlugIn = CType(State, EveHQ.Core.PlugIn)
        Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
        Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
        plugInInfo.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn = plugInInfo.Instance
        Dim pluginContainer As DevComponents.DotNetBar.ItemContainer = CType(rbPlugins.Items.Item(plugInInfo.Name), DevComponents.DotNetBar.ItemContainer)
        Dim LoadPlugInButton As DevComponents.DotNetBar.ButtonItem = CType(pluginContainer.SubItems("LPI" & plugInInfo.Name), DevComponents.DotNetBar.ButtonItem)
        Dim RunPlugInButton As DevComponents.DotNetBar.ButtonItem = CType(pluginContainer.SubItems("RPI" & plugInInfo.Name), DevComponents.DotNetBar.ButtonItem)
        LoadPlugInButton.Enabled = False
        RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Loading..."
        LoadPlugInButton.Text = "Loading..." : RunPlugInButton.Refresh()
        plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Loading
        Try
            Dim PlugInResponse As String = ""
            PlugInResponse = runPlugIn.EveHQStartUp().ToString
            If CBool(PlugInResponse) = False Then
                LoadPlugInButton.Enabled = True
                RunPlugInButton.Enabled = False
                LoadPlugInButton.Text = "Load Plug-in"
                RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Failed"
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Failed
            Else
                Dim hitError As Boolean = False
                Do
                    hitError = False
                    Try
                        If RunPlugInButton IsNot Nothing Then
                            RunPlugInButton.Enabled = True
                        End If
                    Catch e As Exception
                        hitError = True
                    End Try
                Loop Until hitError = False
                LoadPlugInButton.Enabled = False
                RunPlugInButton.Enabled = True
                RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Ready"
                LoadPlugInButton.Text = ""
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
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
            LoadPlugInButton.Enabled = True
            RunPlugInButton.Enabled = False
        Catch ex As Exception
            MessageBox.Show("Unable to load plugin: " & plugInInfo.Name & ControlChars.CrLf & ex.Message, "Plugin error")
            LoadPlugInButton.Enabled = True
            RunPlugInButton.Enabled = False
        End Try
        rbPlugins.Refresh()
    End Sub
#End Region

#Region "Plug-in Routines"
    Private Sub SetupModuleMenu()
        If EveHQ.Core.HQ.EveHQSettings.Plugins.Count <> 0 Then
            ' Clear the Plug-ins ribbon
            rbPlugins.Items.Clear()
            Dim modCount As Integer = 0
            For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
                If PlugInInfo.Available = True Then
                    modCount += 1
                    ' Create the plug-in container and orientations
                    Dim pluginContainer As New DevComponents.DotNetBar.ItemContainer
                    pluginContainer.Name = PlugInInfo.Name
                    pluginContainer.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical
                    pluginContainer.MinimumSize = New Size(80, 25)
                    pluginContainer.HorizontalItemAlignment = DevComponents.DotNetBar.eHorizontalItemsAlignment.Left
                    pluginContainer.VerticalItemAlignment = DevComponents.DotNetBar.eVerticalItemsAlignment.Top

                    ' Create a new plug-in button for the item
                    Dim RunPlugInButton As New DevComponents.DotNetBar.ButtonItem
                    RunPlugInButton.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText
                    RunPlugInButton.ImagePosition = DevComponents.DotNetBar.eImagePosition.Left
                    RunPlugInButton.Image = PlugInInfo.MenuImage
                    RunPlugInButton.ImageFixedSize = New Size(40, 40)
                    RunPlugInButton.Name = "RPI" & PlugInInfo.Name
                    RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Not Loaded"

                    ' Add a shiny tooltip
                    Dim stt As New DevComponents.DotNetBar.SuperTooltipInfo
                    stt.FooterText = "EveHQ Plug-in: " & PlugInInfo.Name
                    stt.BodyText = PlugInInfo.Description & ControlChars.CrLf & ControlChars.CrLf
                    stt.BodyText &= "Author: " & PlugInInfo.Author
                    stt.Color = DevComponents.DotNetBar.eTooltipColor.Yellow
                    stt.BodyImage = CType(My.Resources.Info32, Image)
                    stt.FooterImage = PlugInInfo.MenuImage
                    SuperTooltip1.SetSuperTooltip(RunPlugInButton, stt)

                    AddHandler RunPlugInButton.Click, AddressOf PlugInIconClick
                    pluginContainer.SubItems.Add(RunPlugInButton)
                    ' Add a load item for each disabled plug-in
                    Dim LoadPlugInButton As New DevComponents.DotNetBar.ButtonItem
                    LoadPlugInButton.Name = "LPI" & PlugInInfo.Name
                    LoadPlugInButton.Text = "Load Plug-in"
                    LoadPlugInButton.Tooltip = "Load the " & PlugInInfo.MainMenuText & " Plug-in"
                    LoadPlugInButton.ImageFixedSize = New Size(2, 2)
                    LoadPlugInButton.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.TextOnlyAlways
                    LoadPlugInButton.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
                    LoadPlugInButton.CanCustomize = False
                    LoadPlugInButton.Size = New Size(20, 40)
                    AddHandler LoadPlugInButton.Click, AddressOf LoadPlugin
                    pluginContainer.SubItems.Add(LoadPlugInButton)

                    ' Override settings if the remote server says so
                    Dim ServerOverride As Boolean = False
                    If EveHQ.Core.HQ.EveHQServerMessage IsNot Nothing Then
                        If EveHQ.Core.HQ.EveHQServerMessage.DisabledPlugins.ContainsKey(PlugInInfo.Name) = True Then
                            If CompareVersions(PlugInInfo.Version, EveHQ.Core.HQ.EveHQServerMessage.DisabledPlugins(PlugInInfo.Name)) = True Then
                                ServerOverride = True
                            End If
                        End If
                    End If

                    If ServerOverride = False Then
                        If PlugInInfo.RunAtStartup = True Then
                            LoadPlugInButton.Enabled = True
                            RunPlugInButton.Enabled = False
                            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                        Else
                            If PlugInInfo.Disabled = False Then
                                RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Ready"
                                LoadPlugInButton.Enabled = False
                                LoadPlugInButton.Text = ""
                                RunPlugInButton.Enabled = True
                                PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
                            Else
                                LoadPlugInButton.Enabled = True
                                RunPlugInButton.Enabled = False
                                PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                            End If
                        End If
                    Else
                        RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Disabled"
                        RunPlugInButton.Tooltip = PlugInInfo.MainMenuText & " has been disabled remotely due to critical issues!"
                        LoadPlugInButton.Enabled = False
                        LoadPlugInButton.Text = "Disabled"
                        LoadPlugInButton.Tooltip = PlugInInfo.MainMenuText & " has been disabled remotely due to critical issues!"

                        RunPlugInButton.Enabled = False
                        PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                    End If
                    rbPlugins.Items.Add(pluginContainer)
                Else
                    ' Need anything here if the plug-in is disabled?
                End If
            Next
            RibbonControl1.RecalcLayout()
        End If
    End Sub
    Private Function CompareVersions(ByVal thisVersion As String, ByVal requiredVersion As String) As Boolean
        Dim requiresUpdate As Boolean = False
        Try
            Dim localVers() As String = thisVersion.Split(CChar("."))
            Dim remoteVers() As String = requiredVersion.Split(CChar("."))
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
        Catch ex As Exception
            Return requiresUpdate
        End Try
    End Function
    Private Sub ModuleMenuItemClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(mnu.Name)
        If tp IsNot Nothing Then
            tabEveHQMDI.SelectedTab = tp
        Else
            Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(mnu.Name), Core.PlugIn)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            Call DisplayChildForm(plugInForm)
        End If
    End Sub
    Private Sub PlugInIconClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As DevComponents.DotNetBar.ButtonItem = DirectCast(sender, DevComponents.DotNetBar.ButtonItem)
        Dim PlugInName As String = btn.Name.Remove(0, 3)
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(PlugInName)
        If tp IsNot Nothing Then
            tabEveHQMDI.SelectedTab = tp
        Else
            Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PlugInName), Core.PlugIn)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            Call Me.DisplayChildForm(plugInForm)
        End If
    End Sub
    Private Sub LoadPlugin(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim PIB As DevComponents.DotNetBar.ButtonItem = DirectCast(sender, DevComponents.DotNetBar.ButtonItem)
        Dim plugInName As String = PIB.Name.Remove(0, 3)
        Dim PlugInInfo As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins.Item(plugInName), Core.PlugIn)
        If PlugInInfo.RunAtStartup = True Then
            ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
        Else
            Dim pluginContainer As DevComponents.DotNetBar.ItemContainer = CType(rbPlugins.Items.Item(PlugInInfo.Name), DevComponents.DotNetBar.ItemContainer)
            Dim LoadPlugInButton As DevComponents.DotNetBar.ButtonItem = CType(pluginContainer.SubItems("LPI" & PlugInInfo.Name), DevComponents.DotNetBar.ButtonItem)
            Dim RunPlugInButton As DevComponents.DotNetBar.ButtonItem = CType(pluginContainer.SubItems("RPI" & PlugInInfo.Name), DevComponents.DotNetBar.ButtonItem)
            RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Ready"
            LoadPlugInButton.Text = ""
            PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
            LoadPlugInButton.Enabled = False
            RunPlugInButton.Enabled = True
        End If
        rbPlugins.Refresh()
    End Sub
    Private Sub RunPlugin(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(mnu.Name)
        If tp IsNot Nothing Then
            tabEveHQMDI.SelectedTab = tp
        Else
            Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(mnu.Name), Core.PlugIn)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            Call DisplayChildForm(plugInForm)
        End If
    End Sub
    Public Sub LoadAndOpenPlugIn(ByVal State As Object)
        ' Called usually from an instance
        Dim plugInInfo As EveHQ.Core.PlugIn = CType(State, EveHQ.Core.PlugIn)
        Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
        Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
        plugInInfo.Instance = CType(Activator.CreateInstance(t), EveHQ.Core.IEveHQPlugIn)
        Dim runPlugIn As EveHQ.Core.IEveHQPlugIn = plugInInfo.Instance
        Dim pluginContainer As DevComponents.DotNetBar.ItemContainer = CType(rbPlugins.Items.Item(plugInInfo.Name), DevComponents.DotNetBar.ItemContainer)
        Dim LoadPlugInButton As DevComponents.DotNetBar.ButtonItem = CType(pluginContainer.SubItems("LPI" & plugInInfo.Name), DevComponents.DotNetBar.ButtonItem)
        Dim RunPlugInButton As DevComponents.DotNetBar.ButtonItem = CType(pluginContainer.SubItems("RPI" & plugInInfo.Name), DevComponents.DotNetBar.ButtonItem)
        LoadPlugInButton.Enabled = False
        RunPlugInButton.Enabled = False
        RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Loading..."
        plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Loading
        Try
            Dim PlugInResponse As String = ""
            PlugInResponse = runPlugIn.EveHQStartUp().ToString
            If CBool(PlugInResponse) = False Then
                LoadPlugInButton.Enabled = True
                RunPlugInButton.Enabled = False
                RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Failed"
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Failed
            Else
                RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Ready"
                plugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active
                LoadPlugInButton.Enabled = False
                RunPlugInButton.Enabled = True
            End If
            ' Clean up after loading the plugin
            Call EveHQ.Core.HQ.ReduceMemory()
            ' Open the Plug-in
            Dim myDelegate As New OpenPlugInDelegate(AddressOf OpenPlugIn)
            Me.Invoke(myDelegate, New Object() {plugInInfo.Name})
        Catch ex As Exception
            MessageBox.Show("Unable to load plugin: " & plugInInfo.Name & ControlChars.CrLf & ex.Message, "Plugin error")
            LoadPlugInButton.Enabled = True
            RunPlugInButton.Enabled = False
        End Try
    End Sub

    Delegate Sub OpenPlugInDelegate(ByVal PlugInName As String)
    Private Sub OpenPlugIn(ByVal PlugInName As String)
        Dim PlugInInfo As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PlugInName), Core.PlugIn)
        If PlugInInfo.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
            Dim mainTab As DevComponents.DotNetBar.TabStrip = CType(EveHQ.Core.HQ.MainForm.Controls("tabEveHQMDI"), DevComponents.DotNetBar.TabStrip)
            Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(PlugInName)
            If tp IsNot Nothing Then
                mainTab.SelectedTab = tp
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
    Public Sub OpenPilotInfoForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmPilot.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmPilot)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Public Sub OpenSkillTrainingForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmTraining.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmTraining)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Public Sub OpenEveHQMailForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmMail.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmMail)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenBackUpForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmBackup.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmBackup)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenEveHQBackUpForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmBackupEveHQ.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmBackupEveHQ)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenAPICheckerForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmAPIChecker.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmAPIChecker)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenMarketPricesForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(EveHQMLF.Text)
        If tp Is Nothing Then
            EveHQMLF = New frmMarketPrices
            Call DisplayChildForm(EveHQMLF)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenDashboard()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmDashboard.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmDashboard)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenRequisitions()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab("EveHQ Requisitions")
        If tp Is Nothing Then
            Dim myReq As New EveHQ.Core.frmRequisitions
            Call DisplayChildForm(myReq)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenSQLQueryForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmSQLQuery.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmSQLQuery)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Private Sub OpenInfoHelpForm()
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(frmHelp.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmHelp)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub
    Public Sub DisplayReport(ByRef reportForm As EveHQ.frmReportViewer, ByVal reportText As String)
        reportForm.Text = reportText
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(reportForm.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(reportForm)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

#End Region

    Private Sub RemoteRefreshPilots()
        Call Me.UpdatePilotInfo()
    End Sub

    Public Sub DisplayChartReport(ByRef chartForm As EveHQ.frmChartViewer, ByVal formTitle As String)
        chartForm.Text = formTitle
        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(chartForm.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(chartForm)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub ctxmnuLaunchEve1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxmnuLaunchEve1.Click
        Call LaunchEveInNormalWindow(1)
    End Sub

    Private Sub ctxmnuLaunchEve2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxmnuLaunchEve2.Click
        Call LaunchEveInNormalWindow(2)
    End Sub

    Private Sub ctxmnuLaunchEve3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxmnuLaunchEve3.Click
        Call LaunchEveInNormalWindow(3)
    End Sub

    Private Sub ctxmnuLaunchEve4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ctxmnuLaunchEve4.Click
        Call LaunchEveInNormalWindow(4)
    End Sub

    Private Sub LaunchEveInNormalWindow(ByVal folder As Integer)
        Me.WindowState = FormWindowState.Minimized
        Try
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folder) = True Then
                Process.Start(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(folder), "Eve.exe"), "/LUA:OFF")
            Else
                Process.Start(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(folder), "Eve.exe"))
            End If
        Catch ex As Exception
            MessageBox.Show("Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
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
            If tabEveHQMDI.Tabs.Count > 0 Then
                For tab As Integer = tabEveHQMDI.Tabs.Count - 1 To 0
                    Dim tp As DevComponents.DotNetBar.TabItem = tabEveHQMDI.Tabs(tab)
                    tabEveHQMDI.Tabs.Remove(tp)
                Next
            End If

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
                EveHQ.Core.HQ.EveHQSettings.Corporations.Clear()
                EveHQ.Core.HQ.TPilots.Clear()
                EveHQ.Core.HQ.TCorps.Clear()
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
            RemoveHandler emeFSW.Created, AddressOf OnMarketLogCreated
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
                Dim UserPrice As Double = CDbl(priceData(12)) : Dim typeID As Long = CLng(priceData(13))
                If EveHQ.Core.HQ.EveHQSettings.MarketLogUpdatePrice = True Then
                    If Not Double.IsNaN(UserPrice) And Not Double.IsInfinity(UserPrice) Then
                        ' Update the market price
                        If EveHQ.Core.DataFunctions.SetCustomPrice(typeID, UserPrice, False) = True Then
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
                End If
            End If
        End If
    End Sub

#End Region

    Private Sub lblAPIStatus_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAPIStatus.DoubleClick
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
                    ElseIf myAssembly.Name.StartsWith("DevComponents") Then
                        CurrentComponents.Add(myAssembly.Name & ".dll", myAssembly.Version.ToString)
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
                        CurrentComponents.Add("EveHQ.sdf.zip", databaseData.Tables(0).Rows(0).Item("Version").ToString)
                    Else
                        CurrentComponents.Add("EveHQ.sdf.zip", "1.0.0.0")
                    End If
                Else
                    CurrentComponents.Add("EveHQ.sdf.zip", "1.0.0.0")
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
                        If updateFile.ChildNodes(0).InnerText <> "EveHQ.sdf.zip" Or (updateFile.ChildNodes(0).InnerText = "EveHQ.sdf.zip" And EveHQ.Core.HQ.EveHQSettings.DBFormat = 0) Then
                            UpdateRequired = True
                        End If
                    End If
                Next
                If UpdateRequired = True Then
                    btnUpdateEveHQ.Enabled = True
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
            Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
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
        Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
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

#Region "Eve Mail Functions"

    Public Sub UpdateEveMailButton()
        ' Get a list of the mail messages that are unread
        Dim strSQL As String = "SELECT COUNT(*) FROM eveMail WHERE readMail=0;"
        Dim mailData As Data.DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        Dim unreadMail As Integer = 0
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                unreadMail = CInt(mailData.Tables(0).Rows(0).Item(0))
            End If
        End If

        ' Get a list of the notifications that are unread
        strSQL = "SELECT COUNT(*) FROM eveNotifications WHERE readMail=0;"
        mailData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
        Dim unreadNotices As Integer = 0
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                unreadNotices = CInt(mailData.Tables(0).Rows(0).Item(0))
            End If
        End If

        lblEveMail.Text = "EveMail: " & unreadMail.ToString & ControlChars.CrLf & "Notices: " & unreadNotices.ToString
        btnEveMail.Tooltip = "View Mail && Notifications" & ControlChars.CrLf & "Unread: " & unreadMail.ToString & " mails, " & unreadNotices.ToString & " notifications"
    End Sub

    Private Sub UpdateMailNotifications()
        If EveHQ.Core.EveMailEvents.MailIsBeingProcessed = False Then
            Threading.ThreadPool.QueueUserWorkItem(AddressOf MailUpdateThread, frmMail.IsHandleCreated)
        End If
    End Sub

    Private Sub MailUpdateThread(ByVal MailFormOpen As Object)

        ' Set the processing flag
        EveHQ.Core.EveMailEvents.MailIsBeingProcessed = True

        ' Check for the AutoMailAPI flag
        Dim requiresAutoDisable As Boolean = False
        If EveHQ.Core.HQ.EveHQSettings.AutoMailAPI = True Then
            requiresAutoDisable = True
        End If
        ' Disable the AutoMailAPI flag if required
        If requiresAutoDisable = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoMailAPI = False
        End If

        Me.Invoke(New MethodInvoker(AddressOf Me.UpdateMailAPILabelStart))
        EveHQ.Core.EveMailEvents.MailUpdateStart()

        ' Call the main routines!
        Dim myMail As New EveHQ.Core.EveMail
        Call myMail.GetMail()

        Me.Invoke(New MethodInvoker(AddressOf Me.UpdateMailAPILabelEnd))
        EveHQ.Core.EveMailEvents.MailUpdateComplete()

        ' Update the main EveMail button
        Call Me.UpdateEveMailButton()

        ' Set the AutoMailAPI flag if required
        If requiresAutoDisable = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoMailAPI = True
        End If

        EveHQ.Core.EveMailEvents.MailIsBeingProcessed = False

    End Sub

    Private Sub UpdateMailAPILabelStart()
        lblMailAPITime.Text = "Processing..."
    End Sub

    Private Sub UpdateMailAPILabelEnd()
        lblMailAPITime.Text = "Updating..."
    End Sub

#End Region

    Public Shared Sub CatchGeneralException(ByRef e As Exception)
        Dim myException As New frmException
        myException.lblVersion.Text = "Version: " & My.Application.Info.Version.ToString
        myException.lblError.Text = e.Message
        Dim trace As New System.Text.StringBuilder
        trace.AppendLine(e.StackTrace.ToString)
        trace.AppendLine("")
        trace.AppendLine("========== Plug-ins ==========")
        trace.AppendLine("")
        For Each myPlugIn As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            If myPlugIn.ShortFileName IsNot Nothing Then
                trace.AppendLine(myPlugIn.ShortFileName & " (" & myPlugIn.Version & ")")
            End If
        Next
        trace.AppendLine("")
        trace.AppendLine("")
        trace.AppendLine("========= System Info =========")
        trace.AppendLine("")
        trace.AppendLine("Operating System: " & Environment.OSVersion.ToString)
        trace.AppendLine(".Net Framework Version: " & Environment.Version.ToString)
        trace.AppendLine("EveHQ Location: " & EveHQ.Core.HQ.appFolder)
        trace.AppendLine("EveHQ Cache Locations: " & EveHQ.Core.HQ.appDataFolder)
        myException.txtStackTrace.Text = trace.ToString
        Dim result As Integer = myException.ShowDialog()
        If result = DialogResult.Ignore Then
        Else
            Call frmEveHQ.ShutdownRoutine()
        End If
    End Sub

#Region "Ribbon Button Routines"

    Private Sub btnManageAPI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManageAPI.Click
        Dim EveHQSettings As New frmSettings
        EveHQSettings.Tag = "nodeEveAccounts"
        EveHQSettings.ShowDialog()
        EveHQSettings.Dispose()
    End Sub

    Private Sub btnQueryAPI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQueryAPI.Click
        Call Me.QueryMyEveServer()
    End Sub

    Private Sub btnViewPilotInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewPilotInfo.Click
        Call Me.OpenPilotInfoForm()
    End Sub

    Private Sub btnViewSkillTraining_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewSkillTraining.Click
        Call Me.OpenSkillTrainingForm()
    End Sub

    Private Sub btnViewPrices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewPrices.Click
        Call Me.OpenMarketPricesForm()
    End Sub

    Private Sub btnViewDashboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewDashboard.Click
        Call Me.OpenDashboard()
    End Sub

    Private Sub btnEveMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEveMail.Click
        Call Me.OpenEveHQMailForm()
    End Sub

    Private Sub btnViewReqs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewReqs.Click
        Call Me.OpenRequisitions()
    End Sub

    Private Sub btnIGB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIGB.Click
        If EveHQ.Core.HQ.IGBActive = False Then
            If IGBWorker.CancellationPending = True Then
                MessageBox.Show("The IGB Server is still shutting down. Please wait a few moments", "IGB Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                IGBWorker.Dispose()
                IGBWorker = New System.ComponentModel.BackgroundWorker
            Else
                If IGBCanBeInitialised() = True Then
                    IGBWorker.WorkerSupportsCancellation = True
                    IGBWorker.RunWorkerAsync()
                    EveHQ.Core.HQ.IGBActive = True
                    btnIGB.Checked = True
                    lblIGB.Text = "Port: " & EveHQ.Core.HQ.EveHQSettings.IGBPort.ToString & ControlChars.CrLf & "Status: On"
                End If
            End If
        Else
            IGBWorker.CancelAsync()
            EveHQ.Core.HQ.IGBActive = False
            btnIGB.Checked = False
            lblIGB.Text = "Port: " & EveHQ.Core.HQ.EveHQSettings.IGBPort.ToString & ControlChars.CrLf & "Status: Off"
        End If
    End Sub

    Private Sub btnBackupEveHQ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackupEveHQ.Click
        Call Me.OpenEveHQBackUpForm()
    End Sub

    Private Sub btnBackupEve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackupEve.Click
        Call Me.OpenBackUpForm()
    End Sub

    Private Sub btnFileSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFileSettings.Click
        frmSettings.ShowDialog()
    End Sub

    Private Sub btnFileExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFileExit.Click
        Me.Close()
    End Sub

    Private Sub btnAPIChecker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAPIChecker.Click
        Call Me.OpenAPICheckerForm()
    End Sub

    Private Sub btnOpenCacheFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenCacheFolder.Click
        Try
            Process.Start(EveHQ.Core.HQ.appDataFolder)
        Catch ex As Exception
            MessageBox.Show("Unable to start Windows Explorer: " & ex.Message, "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub btnClearCharacterCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearCharacterCache.Click
        Dim msg As String = "This will delete the character specific XML files, clear the pilot data and reconnect to the API." & ControlChars.CrLf & ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Cache", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Close all open forms
                If tabEveHQMDI.Tabs.Count > 0 Then
                    For tab As Integer = tabEveHQMDI.Tabs.Count - 1 To 0
                        Dim tp As DevComponents.DotNetBar.TabItem = tabEveHQMDI.Tabs(tab)
                        tabEveHQMDI.Tabs.Remove(tp)
                    Next
                End If

                ' Clear the character XML files
                Try
                    For Each charFile As String In My.Computer.FileSystem.GetFiles(EveHQ.Core.HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly, "EVEHQAPI_" & EveAPI.APITypes.CharacterSheet.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the skill training XML files
                Try
                    For Each charFile As String In My.Computer.FileSystem.GetFiles(EveHQ.Core.HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly, "EVEHQAPI_" & EveAPI.APITypes.SkillTraining.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the skill queue XML files
                Try
                    For Each charFile As String In My.Computer.FileSystem.GetFiles(EveHQ.Core.HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly, "EVEHQAPI_" & EveAPI.APITypes.SkillQueue.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the EveHQ Pilot Data
                Try
                    EveHQ.Core.HQ.EveHQSettings.Pilots.Clear()
                    EveHQ.Core.HQ.EveHQSettings.Corporations.Clear()
                    EveHQ.Core.HQ.TPilots.Clear()
                    EveHQ.Core.HQ.TCorps.Clear()
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

    Private Sub btnClearImageCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearImageCache.Click
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

    Private Sub btnClearAllCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAllCache.Click
        Dim msg As String = "This will delete the entire contents of the cache folder, clear the pilot data and reconnect to the API." & ControlChars.CrLf & ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Cache", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Close all open forms
                If tabEveHQMDI.Tabs.Count > 0 Then
                    For tab As Integer = tabEveHQMDI.Tabs.Count - 1 To 0
                        Dim tp As DevComponents.DotNetBar.TabItem = tabEveHQMDI.Tabs(tab)
                        tabEveHQMDI.Tabs.Remove(tp)
                    Next
                End If

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
                    EveHQ.Core.HQ.EveHQSettings.Corporations.Clear()
                    EveHQ.Core.HQ.TPilots.Clear()
                    EveHQ.Core.HQ.TCorps.Clear()
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

    Private Sub btnCheckForUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckForUpdates.Click
        Call Me.ShowUpdateForm()
    End Sub

    Private Sub btnUpdateEveHQ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateEveHQ.Click
        Call Me.UpdateNow()
        Me.Close()
    End Sub

    Private Sub btnViewHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewHistory.Click
        Try
            Process.Start("http://wiki.indiceve.com/doku.php?id=guide:history")
        Catch ex As Exception
            ' Guess the user needs to reset the http protocol in the OS - not much EveHQ can do here!
        End Try
    End Sub

    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        Dim AboutForm As New frmAbout
        AboutForm.ShowDialog()
        AboutForm.Dispose()
    End Sub

    Private Sub btnSQLQueryTool_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSQLQueryTool.Click
        Call Me.OpenSQLQueryForm()
    End Sub

    Private Sub btnInfoHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnInfoHelp.Click
        Call Me.OpenInfoHelpForm()
    End Sub

#Region "Ribbon Report Functions"

#Region "Report Options Routines"
    Private Sub UpdateReportPilots()
        cboReportPilot.Items.Clear()
        For Each rPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If rPilot.Active = True Then
                cboReportPilot.Items.Add(rPilot.Name)
            End If
        Next
        If cboReportPilot.Items.Count > 0 Then
            If cboReportPilot.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = True Then
                cboReportPilot.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
            Else
                cboReportPilot.SelectedIndex = 0
            End If
        End If
    End Sub
    Private Sub cboReportPilot_SelectedIndexChaged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReportPilot.SelectedIndexChanged
        Call Me.BuildQueueReportsMenu()
    End Sub
    Private Sub cboReportFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReportFormat.SelectedIndexChanged
        ' Get the name of the selected item
        Dim selItem As String = cboReportFormat.SelectedItem.ToString
        ' Cycle through the ribbon bars to hide the non-applicable ones
        For Each rb As DevComponents.DotNetBar.RibbonBar In rpReports.Controls
            If rb.Name = "rb" & selItem Or rb.Name = "rbReportOptions" Or rb.Name = "rbStandard" Then
                rb.Visible = True
            Else
                rb.Visible = False
            End If
        Next
        rpReports.Refresh()
    End Sub
    Private Sub btnOpenReportFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenReportFolder.Click
        Try
            Process.Start(EveHQ.Core.HQ.reportFolder)
        Catch ex As Exception
            MessageBox.Show("Unable to start Windows Explorer: " & ex.Message, "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Public Sub BuildQueueReportsMenu()
        ' Clear option for btnHTMLTrainingQueue
        For Each queueBtn As DevComponents.DotNetBar.ButtonItem In btnHTMLTrainingQueue.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear option for btnHTMLQueueShoppingList
        For Each queueBtn As DevComponents.DotNetBar.ButtonItem In btnHTMLQueueShoppingList.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear option for btnTextTrainingQueue
        For Each queueBtn As DevComponents.DotNetBar.ButtonItem In btnTextTrainingQueue.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear option for btnTextQueueShoppingList
        For Each queueBtn As DevComponents.DotNetBar.ButtonItem In btnTextQueueShoppingList.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear the existing options
        btnHTMLTrainingQueue.SubItems.Clear()
        btnHTMLQueueShoppingList.SubItems.Clear()
        btnTextTrainingQueue.SubItems.Clear()
        btnTextQueueShoppingList.SubItems.Clear()
		' Rebuild the queue and shopping list options based on the pilot
		If cboReportPilot.SelectedItem IsNot Nothing Then
			Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
			If rPilot IsNot Nothing Then
				If rPilot.TrainingQueues IsNot Nothing Then
					For Each qItem As EveHQ.Core.SkillQueue In rPilot.TrainingQueues.Values
                        Dim queueBtn As New DevComponents.DotNetBar.ButtonItem
                        queueBtn.CanCustomize = False
						queueBtn.Text = qItem.Name
						queueBtn.Name = qItem.Name
						queueBtn.Image = My.Resources.SkillBook16
						AddHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
						btnHTMLTrainingQueue.SubItems.Add(queueBtn)
                        queueBtn = New DevComponents.DotNetBar.ButtonItem
                        queueBtn.CanCustomize = False
						queueBtn.Text = qItem.Name
						queueBtn.Name = qItem.Name
						queueBtn.Image = My.Resources.SkillBook16
						AddHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
						btnHTMLQueueShoppingList.SubItems.Add(queueBtn)
                        queueBtn = New DevComponents.DotNetBar.ButtonItem
                        queueBtn.CanCustomize = False
						queueBtn.Text = qItem.Name
						queueBtn.Name = qItem.Name
						queueBtn.Image = My.Resources.SkillBook16
						AddHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
						btnTextTrainingQueue.SubItems.Add(queueBtn)
                        queueBtn = New DevComponents.DotNetBar.ButtonItem
                        queueBtn.CanCustomize = False
						queueBtn.Text = qItem.Name
						queueBtn.Name = qItem.Name
						queueBtn.Image = My.Resources.SkillBook16
						AddHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
						btnTextQueueShoppingList.SubItems.Add(queueBtn)
					Next
				End If
			End If
		End If
	End Sub
    Private Sub ReportsMenuHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Identify queue name
        Dim queueBtn As DevComponents.DotNetBar.ButtonItem = CType(sender, DevComponents.DotNetBar.ButtonItem)
        Dim queueName As String = queueBtn.Text
        ' Find parent button name
        Dim reportType As String = queueBtn.Parent.Name
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Setup report details
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Select Case reportType
            Case "btnHTMLTrainingQueue"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(queueName), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "btnHTMLQueueShoppingList"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(queueName), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "btnTextTrainingQueue"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(queueName), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTextTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "btnTextQueueShoppingList"
                Dim rQueue As EveHQ.Core.SkillQueue = CType(rPilot.TrainingQueues(queueName), Core.SkillQueue)
                Call EveHQ.Core.Reports.GenerateTextShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
        End Select
    End Sub
#End Region

#Region "Standard Reports"

    Private Sub btnStdCharSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStdCharSummary.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCharSummary()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PilotSummary.html"))
        DisplayReport(newReport, "Pilot Summary")
    End Sub

    Private Sub btnStdSkillLevels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStdSkillLevels.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateSPSummary()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SPSummary.html"))
        DisplayReport(newReport, "Skill Point Summary")
    End Sub

    Private Sub btnStdAlloyReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStdAlloyReport.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateAlloyReport()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "AlloyReport.html"))
        DisplayReport(newReport, "Alloy Composition")
    End Sub

    Private Sub btnStdAsteroidReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStdAsteroidReport.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateRockReport()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "OreReport.html"))
        DisplayReport(newReport, "Asteroid Composition")
    End Sub

    Private Sub btnStdIceReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStdIceReport.Click
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateIceReport()
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "IceReport.html"))
        DisplayReport(newReport, "Ice Composition")
    End Sub

#End Region

#Region "HTML Reports"

    Private Sub btnHTMLCharSheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLCharSheet.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCharSheet(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharSheet (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLTrainingTimes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLTrainingTimes.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTrainingTime(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainTime (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Training Times - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLTimeToLvl5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLTimeToLvl5.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTimeToLevel5(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillLevels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLSkillLevels.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateSkillLevels(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillsAvailable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLSkillsAvailable.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateSkillsAvailable(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillsNotTrained_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLSkillsNotTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateSkillsNotTrained(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLPartiallyTrained_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLPartiallyTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GeneratePartialSkills(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillsCost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTMLSkillsCost.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateSkillsCost(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsCost (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skills Cost - " & rPilot.Name)
    End Sub

#End Region

#Region "Text Reports"

    Private Sub btnTextCharacterSheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextCharacterSheet.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextCharSheet(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharSheet (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
    End Sub

    Private Sub btnTextTrainingTimes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextTrainingTimes.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextTrainingTime(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainTime (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Training Times - " & rPilot.Name)
    End Sub

    Private Sub btnTextTimeToLvl5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextTimeToLvl5.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextTimeToLevel5(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillLevels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextSkillLevels.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextSkillLevels(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillsAvailable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextSkillsAvailable.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextSkillsAvailable(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillsNotTrained_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextSkillsNotTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextSkillsNotTrained(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
    End Sub

    Private Sub btnTextPartiallyTrained_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextPartiallyTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextPartialSkills(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillsCost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextSkillsCost.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTextSkillsCost(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsCost (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skills Cost - " & rPilot.Name)
    End Sub

#End Region

#Region "XML Reports"

    Private Sub btnXMLCharacterXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXMLCharacterXML.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCharXML(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharXML (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Imported Character XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLTrainingXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXMLTrainingXML.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateTrainXML(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Imported Training XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLCurrentCharOld_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXMLCurrentCharOld.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCurrentPilotXML_Old(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CurrentXML - Old (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Old Style Character XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLCurrentCharNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXMLCurrentCharNew.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCurrentPilotXML_New(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CurrentXML - New (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Current Character XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLCurrentTrainingOld_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXMLCurrentTrainingOld.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GenerateCurrentTrainingXML_Old(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML - Old (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Old Style Training XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLECMExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnXMLECMExport.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Call EveHQ.Core.Reports.GenerateECMExportReports(rPilot)
    End Sub

#End Region

#Region "PHPBB Reports"

    Private Sub btnPHPBBCharacterSheet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPHPBBCharacterSheet.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newReport As New frmReportViewer
        Call EveHQ.Core.Reports.GeneratePHPBBCharSheet(rPilot)
        newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "PHPBBCharSheet (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "PHPBB Character Sheet - " & rPilot.Name)
    End Sub

#End Region

#Region "Chart Reports"

    Private Sub btnChartSkillGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChartSkillGroup.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newChartForm As New frmChartViewer
        newChartForm.Controls.Add(EveHQ.Core.Reports.SkillGroupChart(rPilot))
        Call Me.DisplayChartReport(newChartForm, "Skill Group Chart - " & rPilot.Name)
    End Sub

    Private Sub btnChartSkillCost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChartSkillCost.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboReportPilot.SelectedItem.ToString), Core.Pilot)
        Dim newChartForm As New frmChartViewer
        newChartForm.Controls.Add(EveHQ.Core.Reports.SkillCostChart(rPilot))
        Call Me.DisplayChartReport(newChartForm, "Skill Cost Chart - " & rPilot.Name)
    End Sub

#End Region

#End Region

#End Region

#Region "Training Bar Routines"

    Private Sub Bar1_BarDock(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bar1.BarDock
        EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = Bar1.DockSide
        Select Case Bar1.DockSide
            Case DevComponents.DotNetBar.eDockSide.Top, DevComponents.DotNetBar.eDockSide.Bottom
                'DockContainerItem1.Height = 75
            Case DevComponents.DotNetBar.eDockSide.Left, DevComponents.DotNetBar.eDockSide.Right
                'DockContainerItem1.Width = 320
        End Select
        Call Me.SetupTrainingStatus()
    End Sub

    Private Sub Bar1_BarUndock(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bar1.BarUndock
        EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = Bar1.DockSide
        Call Me.SetupTrainingStatus()
    End Sub

    Private Sub Bar1_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bar1.SizeChanged
        If appStartUp = False Then
            EveHQ.Core.HQ.EveHQSettings.TrainingBarHeight = DockContainerItem1.Height + 3
            EveHQ.Core.HQ.EveHQSettings.TrainingBarWidth = DockContainerItem1.Width
        End If
    End Sub

#End Region


#Region "Theme Modification and Automatic Color Scheme creation based on the selected color table"

    Private m_ColorSelected As Boolean = False
    Private m_ManagerStyle As DevComponents.DotNetBar.eStyle = DevComponents.DotNetBar.eStyle.Office2007Black
    Private Sub buttonStyleCustom_ExpandChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomTheme.ExpandChange
        If btnCustomTheme.Expanded Then
            ' Remember the starting color scheme to apply if no color is selected during live-preview
            m_ColorSelected = False
            m_ManagerStyle = DevComponents.DotNetBar.StyleManager.Style
        Else
            If Not m_ColorSelected Then
                DevComponents.DotNetBar.StyleManager.ColorTint = Color.Empty
                DevComponents.DotNetBar.StyleManager.ChangeStyle(m_ManagerStyle, Color.Empty)
            End If
        End If
    End Sub

    Private Sub buttonStyleCustom_ColorPreview(ByVal sender As Object, ByVal e As DevComponents.DotNetBar.ColorPreviewEventArgs) Handles btnCustomTheme.ColorPreview
        Try
            DevComponents.DotNetBar.StyleManager.ColorTint = e.Color
        Catch ex As Exception
        End Try
    End Sub

    Private Sub buttonStyleCustom_SelectedColorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCustomTheme.SelectedColorChanged
        m_ColorSelected = True ' Indicate that color was selected for buttonStyleCustom_ExpandChange method
        btnCustomTheme.CommandParameter = btnCustomTheme.SelectedColor
    End Sub

    Private Sub AppCommandTheme_Executed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AppCommandTheme.Executed
        Dim source As DevComponents.DotNetBar.ICommandSource = CType(sender, DevComponents.DotNetBar.ICommandSource)
        If TypeOf (source.CommandParameter) Is String Then
            Dim cs As DevComponents.DotNetBar.eStyle = CType(System.Enum.Parse(GetType(DevComponents.DotNetBar.eStyle), source.CommandParameter.ToString()), DevComponents.DotNetBar.eStyle)
            ' This is all that is needed to change the color table for all controls on the form
            DevComponents.DotNetBar.StyleManager.ChangeStyle(cs, Color.Empty)
            EveHQ.Core.HQ.EveHQSettings.ThemeStyle = cs
            EveHQ.Core.HQ.EveHQSettings.ThemeSetByUser = True
            DevComponents.DotNetBar.StyleManager.ColorTint = Color.Empty
            EveHQ.Core.HQ.EveHQSettings.ThemeTint = DevComponents.DotNetBar.StyleManager.ColorTint
        ElseIf TypeOf (source.CommandParameter) Is Color Then
            DevComponents.DotNetBar.StyleManager.ColorTint = CType(source.CommandParameter, Color)
            EveHQ.Core.HQ.EveHQSettings.ThemeTint = DevComponents.DotNetBar.StyleManager.ColorTint
            EveHQ.Core.HQ.EveHQSettings.ThemeSetByUser = True
        End If
        Me.Invalidate()
    End Sub
#End Region

    Private Sub RibbonControl1_ExpandedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonControl1.ExpandedChanged
        EveHQ.Core.HQ.EveHQSettings.RibbonMinimised = Not RibbonControl1.Expanded
    End Sub

    Private Sub DisplayChildForm(ByVal ChildForm As Form)
        Dim max As Boolean = False
        If Me.ActiveMdiChild IsNot Nothing Then
            If Me.ActiveMdiChild.WindowState = FormWindowState.Maximized Then
                Me.ActiveMdiChild.WindowState = FormWindowState.Normal
                max = True
            End If
        End If
        ChildForm.MdiParent = Me
        ChildForm.WindowState = FormWindowState.Maximized
        ChildForm.Show()
    End Sub

    Private Function IGBCanBeInitialised() As Boolean
        Dim prefixes(0) As String
        prefixes(0) = "http://*:" & EveHQ.Core.HQ.EveHQSettings.IGBPort & "/"

        ' URI prefixes are required
        If prefixes Is Nothing OrElse prefixes.Length = 0 Then
            Throw New ArgumentException("prefixes")
        End If

        ' Create a listener and add the prefixes.
        Dim listener As New System.Net.HttpListener()
        For Each s As String In prefixes
            listener.Prefixes.Add(s)
        Next

        Try
            ' Attempt to open the listener
            listener.Start()
            listener.Stop()
            listener.Close()
            IGBCanBeInitialised = True
        Catch e As Exception
            ' We have an initialisation error - disable it
            IGBCanBeInitialised = False
            btnIGB.Checked = False
            btnIGB.Enabled = False
            Dim msg As String = "The IGB Server has been disabled due to a failure to initialise correctly." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "This is usually caused by insufficient permissions on the host machine or an incompatible (older) operating system." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "More information and resolutions can be found at http://forum.battleclinic.com/index.php/topic,42896.0/IGB-not-working.html"
            Dim STI As New SuperTooltipInfo("IGB Server Access Error", "IGB Server Disabled", msg, Nothing, My.Resources.Info32, eTooltipColor.Yellow)
            SuperTooltip1.SetSuperTooltip(btnIGB, STI)
        Finally
            listener = Nothing
        End Try

        Return IGBCanBeInitialised

    End Function

   
End Class

