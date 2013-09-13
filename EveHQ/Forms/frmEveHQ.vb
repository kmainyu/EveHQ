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
Imports System.ComponentModel
Imports System.Net.Cache
Imports System.Data
Imports EveHQ.EveAPI
Imports EveHQ.Common
Imports DevComponents.AdvTree
Imports EveHQ.Core
Imports DevComponents.DotNetBar
Imports System.Xml
Imports System.Globalization
Imports System.Net
Imports System.Threading
Imports System.IO
Imports System.Net.Mail
Imports System.Reflection
Imports System.Text
Imports System.Net.Http
Imports EveHQ.Common.Extensions
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.FileIO
Imports System.Threading.Tasks

Public Class frmEveHQ
    Dim WithEvents eveTQWorker As BackgroundWorker = New BackgroundWorker
    Dim WithEvents IGBWorker As BackgroundWorker = New BackgroundWorker
    Dim WithEvents SkillWorker As BackgroundWorker = New BackgroundWorker
    Dim WithEvents BackupWorker As BackgroundWorker = New BackgroundWorker
    Dim WithEvents EveHQBackupWorker As BackgroundWorker = New BackgroundWorker

    Private Delegate Sub QueryMyEveServerDelegate()

    Dim EveHQMLW As New SortedList
    Dim EveHQMLF As New frmMarketPrices
    Dim appStartUp As Boolean = True
    Private EveHQTrayForm As Form = Nothing
    Dim IconShutdown As Boolean = False
    Dim saveTrainingBarSize As Boolean = True

#Region "Icon Routines"

    Private Sub EveHQIcon1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles EveStatusIcon.Click
        If _
            Not _
            (TypeOf e Is MouseEventArgs AndAlso
             (Not TypeOf e Is MouseEventArgs OrElse (TryCast(e, MouseEventArgs).Button = MouseButtons.Right))) Then
            MyBase.Visible = True
            saveTrainingBarSize = False
            Select Case HQ.Settings.MainFormWindowState
                Case FormWindowState.Maximized
                    Me.WindowState = FormWindowState.Maximized
                Case FormWindowState.Normal
                    HQ.Settings.MainFormLocation = Me.Location
                    HQ.Settings.MainFormSize = Me.Size
                    Me.WindowState = FormWindowState.Normal
            End Select
            saveTrainingBarSize = True
            ' Set the training bar position, after checking for null!
            If HQ.Settings.TrainingBarDockPosition = eDockSide.None Then
                HQ.Settings.TrainingBarDockPosition = eDockSide.Bottom
            End If
            If HQ.Settings.DisableTrainingBar = False Then
                Me.Bar1.DockSide = CType(HQ.Settings.TrainingBarDockPosition, eDockSide)
                DockContainerItem1.Height = HQ.Settings.TrainingBarHeight
                DockContainerItem1.Width = HQ.Settings.TrainingBarWidth
            End If
            MyBase.ShowInTaskbar = True
            MyBase.Activate()
            If EveHQTrayForm IsNot Nothing Then
                EveHQTrayForm.Close()
                EveHQTrayForm = Nothing
            End If
        End If
    End Sub

    Private Sub EveHQIcon1_MouseHover(ByVal sender As Object, ByVal e As EventArgs) Handles EveStatusIcon.MouseHover
        ' Only display the pop up window if the context menu isn't showing
        If Not Me.EveIconMenu.Visible And HQ.Settings.TaskbarIconMode = 1 Then
            EveHQTrayForm = New frmToolTrayIconPopup
            EveHQTrayForm.Show()
        End If
    End Sub

    Private Sub EveHQIcon1_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles EveStatusIcon.MouseLeave
        ' Remove the popup if its showing
        If EveHQTrayForm IsNot Nothing Then
            EveHQTrayForm.Close()
            EveHQTrayForm = Nothing
        End If
    End Sub

#End Region

#Region "Menu Click Routines"

    Private Sub ForceServerCheckToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles ForceServerCheckToolStripMenuItem.Click
        Call GetServerStatus()
    End Sub

    Private Sub HideWhenMinimisedToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles HideWhenMinimisedToolStripMenuItem.Click
        If HideWhenMinimisedToolStripMenuItem.Checked = True Then
            HideWhenMinimisedToolStripMenuItem.Checked = False
            frmSettings.chkAutoHide.Checked = False
            HQ.Settings.AutoHide = False
        Else
            HideWhenMinimisedToolStripMenuItem.Checked = True
            frmSettings.chkAutoHide.Checked = True
            HQ.Settings.AutoHide = True
        End If
    End Sub

    Private Sub ctxExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxExit.Click
        IconShutdown = True
        Me.Close()
    End Sub

    Private Sub ctxAbout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxAbout.Click
        If frmAbout.Visible = False Then
            frmAbout.ShowDialog()
        End If
    End Sub

    Private Sub RestoreWindowToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles RestoreWindowToolStripMenuItem.Click
        ' Restores the window
        Me.Show()
        Select Case HQ.Settings.MainFormWindowState
            Case FormWindowState.Maximized
                Me.WindowState = FormWindowState.Maximized
            Case FormWindowState.Normal
                Me.Location = HQ.Settings.MainFormLocation
                Me.Size = HQ.Settings.MainFormSize
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

    Private Sub tmrEve_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrEve.Tick
        tmrEve.Interval = 120000
        Call GetServerStatus()
    End Sub

    Private Sub UpdateEveTime()
        Dim now As DateTime = DateTime.Now.ToUniversalTime()
        Dim fi As DateTimeFormatInfo = CultureInfo.CurrentCulture.DateTimeFormat
        lblEveTime.Text = "EVE Time: " & now.ToString(fi.ShortDatePattern + " HH:mm")
    End Sub

    Private Sub eveTQWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles eveTQWorker.DoWork
        ' Defines what work the thread has to do
        Call HQ.myTQServer.GetServerStatus()
    End Sub

    Private Sub eveTQWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles eveTQWorker.RunWorkerCompleted
        ' Sub raised on the completion of a call to read the Eve TQ data

        ' Check if the server status has changed since the last result and notify user
        If HQ.myTQServer.Status <> HQ.myTQServer.LastStatus Then

            ' Depending on server status, set the notify icon text and the statusbar text
            Select Case HQ.myTQServer.Status
                Case EveServer.ServerStatus.Down
                    'EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    lblTQStatus.Text = HQ.myTQServer.ServerName & ": Unable to connect to server"
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_offline IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_offline
                    End If
                Case EveServer.ServerStatus.Starting
                    EveStatusIcon.Text = HQ.myTQServer.StatusText
                    lblTQStatus.Text = HQ.myTQServer.ServerName & ": " & HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_starting IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_starting
                    End If
                Case EveServer.ServerStatus.Shutting
                    EveStatusIcon.Text = HQ.myTQServer.StatusText
                    lblTQStatus.Text = HQ.myTQServer.ServerName & ": " & HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_starting IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_starting
                    End If
                Case EveServer.ServerStatus.Full
                    EveStatusIcon.Text = HQ.myTQServer.StatusText
                    lblTQStatus.Text = HQ.myTQServer.ServerName & ": " & HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_online IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_online
                    End If
                Case EveServer.ServerStatus.ProxyDown
                    lblTQStatus.Text = HQ.myTQServer.ServerName & ": " & HQ.myTQServer.StatusText
                    EveStatusIcon.Text = HQ.myTQServer.StatusText
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_offline IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_offline
                    End If
                Case EveServer.ServerStatus.Unknown
                    EveStatusIcon.Text = HQ.myTQServer.StatusText
                    lblTQStatus.Text = HQ.myTQServer.ServerName & ": Status unknown"
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ
                    End If
                Case EveServer.ServerStatus.Up
                    lblTQStatus.Text = HQ.myTQServer.ServerName & ": Online (" & HQ.myTQServer.Players & " Players)"
                    If EveStatusIcon IsNot Nothing And My.Resources.EveHQ_online IsNot Nothing Then
                        EveStatusIcon.Icon = My.Resources.EveHQ_online
                    End If
            End Select

            If EveStatusIcon IsNot Nothing Then
                EveStatusIcon.BalloonTipIcon = ToolTipIcon.Info
                EveStatusIcon.BalloonTipTitle = HQ.myTQServer.ServerName & " Status Notification"
                Select Case HQ.myTQServer.Status
                    Case EveServer.ServerStatus.Down
                        EveStatusIcon.BalloonTipText = HQ.myTQServer.ServerName & " is Down"
                    Case EveServer.ServerStatus.Starting
                        EveStatusIcon.BalloonTipText = HQ.myTQServer.ServerName & " is Starting Up"
                    Case EveServer.ServerStatus.Unknown
                        EveStatusIcon.BalloonTipText = HQ.myTQServer.ServerName & " status is Unknown"
                    Case EveServer.ServerStatus.Up
                        EveStatusIcon.BalloonTipText = HQ.myTQServer.ServerName & " is Up"
                End Select
                EveStatusIcon.ShowBalloonTip(3000)
            End If
        Else
            ' Report the players regardless
            If HQ.myTQServer.Status = EveServer.ServerStatus.Up Then
                lblTQStatus.Text = HQ.myTQServer.ServerName & ": Online (" & HQ.myTQServer.Players & " Players)"
            End If
        End If
        ' Update last status
        HQ.myTQServer.LastStatus = HQ.myTQServer.Status
    End Sub

#End Region

#Region "Form Opening & Closing & Resizing (+ Icon)"

    Private Sub frmEveHQ_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing

        ' Check we aren't updating
        If HQ.EveHQIsUpdating = True Then
            MessageBox.Show(
                "You can't exit EveHQ while an update is in progress. Please wait until the update has completed and try again.",
                "Update in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
            e.Cancel = True
            Exit Sub
        End If

        Try

            HQ.WriteLogEvent("Shutdown: EveHQ Form Closure request made")

            ' Are we shutting down to restore settings?
            If HQ.RestoredSettings = False Then
                ' Check if we should minimise rather than exit?
                If e.CloseReason <> CloseReason.TaskManagerClosing And e.CloseReason <> CloseReason.WindowsShutDown Then
                    If HQ.Settings.MinimiseExit = True And IconShutdown = False Then
                        Me.WindowState = FormWindowState.Minimized
                        HQ.WriteLogEvent("Shutdown: EveHQ Form Closure aborted due to 'Minimise on Exit' setting")
                        e.Cancel = True
                        Exit Sub
                    Else
                        ' Check if there are updates available
                        If HQ.AppUpdateAvailable = True Then
                            Dim msg As String = "There are pending updates available - these will be installed now."
                            MessageBox.Show(msg, "Update Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Call Me.UpdateNow()
                        Else
                            HQ.WriteLogEvent("Shutdown: Calling main shutdown routine")
                            Call Me.ShutdownRoutine()
                        End If
                    End If
                Else
                    ' Check if there are updates available
                    If HQ.AppUpdateAvailable = True Then
                        Dim msg As String = "There are pending updates available - these will be installed now."
                        MessageBox.Show(msg, "Update Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Call Me.UpdateNow()
                    Else
                        HQ.WriteLogEvent("Shutdown: Calling main shutdown routine")
                        Call Me.ShutdownRoutine()
                    End If
                End If
            Else
                ' Close and flush the timer file
                Trace.Flush()
                Trace.Listeners.Remove(HQ.EveHqTracer)
            End If
        Catch ex As Exception
            MessageBox.Show("An error occured while closing EveHQ: " & ex.Message & "- " & ex.StackTrace,
                            "Error Closing EveHQ", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If (HQ.LoggingStream.CanWrite) Then


                HQ.LoggingStream.Flush()
                HQ.LoggingStream.Close()
                HQ.LoggingStream.Dispose()
            End If
        End Try
    End Sub

    Private Sub frmEveHQ_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        Me.Hide()

        ' error handlers for any unhandled error forground or background
        AddHandler Application.ThreadException, AddressOf frmEveHQ.CatchUIThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf frmEveHQ.CatchAppDomainUnhandledException

        ' Set global AdvTree settings
        AdvTreeSettings.SelectedScrollIntoViewHorizontal = False

        ' Disable resizing of bar
        appStartUp = True

        Me.EveStatusIcon.Visible = True

        ' Set Theme Stuff
        UpdateTheme(HQ.Settings.ThemeStyle, HQ.Settings.ThemeTint)
        Dim ThemeBtn As ButtonItem = CType(btnTheme.SubItems("btn" & HQ.Settings.ThemeStyle.ToString), ButtonItem)
        ThemeBtn.Checked = True

        ' Add the pilot refresh handler
        AddHandler PilotParseFunctions.RefreshPilots, AddressOf Me.RemoteRefreshPilots
        AddHandler G15LCDv2.UpdateAPI, AddressOf Me.RemoteUpdate
        AddHandler HQ.ShutDownEveHQ, AddressOf Me.ShutdownRoutine
        AddHandler EveMailEvents.MailUpdateNumbers, AddressOf Me.UpdateEveMailButton

        ' Check if "Hide When Minimised" is active
        HideWhenMinimisedToolStripMenuItem.Checked = HQ.Settings.AutoHide

        'Setup the Modules menu if applicable
        Call Me.SetupModuleMenu()

        ' Update the QAT config if applicable
        If HQ.Settings.QATLayout <> "" Then
            RibbonControl1.QatLayout = HQ.Settings.QATLayout
        End If

        ' Check if the IGB should be started here
        If IGBCanBeInitialised() = True Then
            If HQ.Settings.IGBAutoStart = True Then
                If Not HttpListener.IsSupported Then
                    btnIGB.Enabled = False
                    btnIGB.Checked = False
                Else
                    IGBWorker.WorkerSupportsCancellation = True
                    IGBWorker.RunWorkerAsync()
                    btnIGB.Checked = True
                    HQ.IGBActive = True
                End If
            End If
        End If

        ' Set the tab position
        Select Case HQ.Settings.MDITabPosition
            Case "Top"
                Me.tabEveHQMDI.Dock = DockStyle.Top
                Me.tabEveHQMDI.TabAlignment = eTabStripAlignment.Top
            Case "Bottom"
                Me.tabEveHQMDI.Dock = DockStyle.Bottom
                Me.tabEveHQMDI.TabAlignment = eTabStripAlignment.Bottom
        End Select

        ' Check for ribbon status
        RibbonControl1.Expanded = Not HQ.Settings.RibbonMinimised

        ' Close the splash screen
        frmSplash.Close()

        ' Check if the form needs to be minimised on startup
        If HQ.Settings.AutoMinimise = True Then
            Me.WindowState = FormWindowState.Minimized
            'Me.Show()
        Else
            Select Case HQ.Settings.MainFormWindowState
                Case FormWindowState.Normal
                    Me.Show()
                    Me.Location = HQ.Settings.MainFormLocation
                    Me.Size = HQ.Settings.MainFormSize
                    Me.WindowState = FormWindowState.Normal
                Case FormWindowState.Maximized
                    Me.WindowState = FormWindowState.Maximized
                    Me.Show()
            End Select
        End If

        If RibbonControl1.QatPositionedBelowRibbon = True Then
            RibbonControl1.QatPositionedBelowRibbon = False
            RibbonControl1.QatPositionedBelowRibbon = True
        End If

        ' Start the timers
        If HQ.Settings.EnableEveStatus = True Then
            tmrEve.Start()
            lblTQStatus.Text = "Tranquility Status: Not Updated"
        Else
            lblTQStatus.Text = "Tranquility Status: Updates Disabled"
        End If
        tmrSkillUpdate.Start()
        tmrModules.Start()
        If HQ.Settings.EnableAutomaticSave = True Then
            tmrSave.Interval = HQ.Settings.AutomaticSaveTime * 60000
            tmrSave.Start()
        End If

        Call HQ.ReduceMemory()
        tmrMemory.Start()

        ' Update the EveMailNotice button
        Call Me.UpdateEveMailButton()

        ' Update the pilots in the report
        Call Me.UpdateReportPilots()

        ' Set the training bar position, after checking for null!
        If HQ.Settings.DisableTrainingBar = False Then
            If HQ.Settings.TrainingBarDockPosition = eDockSide.None Then
                HQ.Settings.TrainingBarDockPosition = eDockSide.Bottom
            End If
            Me.Bar1.DockSide = CType(HQ.Settings.TrainingBarDockPosition, eDockSide)
            DockContainerItem1.Height = HQ.Settings.TrainingBarHeight
            DockContainerItem1.Width = HQ.Settings.TrainingBarWidth
        Else
            Me.Bar1.Visible = False
        End If

        appStartUp = False

        ' Display server message if applicable
        If HQ.EveHQServerMessage IsNot Nothing Then
            If _
                HQ.EveHQServerMessage.MessageDate > HQ.Settings.LastMessageDate Or
                (HQ.EveHQServerMessage.MessageDate = HQ.Settings.LastMessageDate And
                 HQ.Settings.IgnoreLastMessage = False) Then
                Dim NewMsg As New frmEveHQMessage
                NewMsg.lblMessage.Text = HQ.EveHQServerMessage.Message
                NewMsg.lblTitle.Text = HQ.EveHQServerMessage.MessageTitle
                HQ.Settings.LastMessageDate = HQ.EveHQServerMessage.MessageDate
                If HQ.EveHQServerMessage.AllowIgnore = False Then
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
        If HQ.Settings.Accounts.Count = 0 And HQ.Settings.Pilots.Count = 0 Then
            Dim wMsg As String = "EveHQ has detected that you have not yet setup any API accounts." & ControlChars.CrLf &
                                 ControlChars.CrLf
            wMsg &= "Would you like to do this now?"
            Dim reply As Integer = MessageBox.Show(wMsg, "Welcome to EveHQ!", MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                wMsg =
                    "You can add API accounts using the 'Manage API Account' button on the ribbon bar or by going into Settings and choosing the Eve Accounts section."
                MessageBox.Show(wMsg, "API Account Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim EveHQSettings As New frmSettings
                EveHQSettings.Tag = "nodeEveAccounts"
                EveHQSettings.ShowDialog()
                EveHQSettings.Dispose()
            End If
        End If

        ' Start the update check on a new thread
        If HQ.Settings.DisableAutoWebConnections = False Then
            ThreadPool.QueueUserWorkItem(AddressOf Me.CheckForUpdates)
        End If
    End Sub

    Private Sub UpdateTheme(Theme As eStyle, Tint As Color)
        StyleManager.ChangeStyle(Theme, Tint)
    End Sub

    Private Sub UpdateTint(Tint As Color)
        StyleManager.ColorTint = CType(Tint, Color)
    End Sub

    Private Sub frmEveHQ_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
        ' Determine which view to display!
        If HQ.Settings.StartupView = "" Then
            HQ.Settings.StartupView = "EveHQ Dashboard"
        End If
        Select Case HQ.Settings.StartupView
            Case "EveHQ Dashboard"
                ' Open the dashboard
                Call Me.OpenDashboard()
            Case "Pilot Information"
                If HQ.Settings.StartupPilot <> "" Then
                    ' Open the pilot info form
                    Call OpenPilotInfoForm()
                End If
            Case "Pilot Summary Report"
                ' Show the pilot summary report form!
                Dim newReport As New frmReportViewer
                Call Reports.GenerateCharSummary()
                newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "PilotSummary.html"))
                Call DisplayReport(newReport, "Pilot Summary")
            Case "Skill Training"
                If HQ.Settings.StartupPilot <> "" Then
                    ' Open the skill training form
                    Call OpenSkillTrainingForm()
                End If
            Case Else
                ' Open the dashboard
                Call Me.OpenDashboard()
        End Select
    End Sub

    Private Sub frmEveHQ_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        If HideWhenMinimisedToolStripMenuItem.Checked = True Then
            If Me.WindowState = FormWindowState.Minimized Then
                Me.Hide()
            Else
                If Me.ShowInTaskbar = False Then
                    Me.ShowInTaskbar = True
                End If
            End If
        End If

        ' Save the window position if possible
        If HQ.Settings IsNot Nothing Then
            Select Case Me.WindowState
                Case FormWindowState.Normal
                    HQ.Settings.MainFormLocation = Me.Location
                    HQ.Settings.MainFormSize = Me.Size
                    HQ.Settings.MainFormWindowState = Me.WindowState
                Case FormWindowState.Maximized
                    HQ.Settings.MainFormWindowState = Me.WindowState
            End Select
        End If

    End Sub

    Private Sub frmEveHQ_Move(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Move
        ' Save the window position if possible
        If HQ.Settings IsNot Nothing Then
            Select Case Me.WindowState
                 Case FormWindowState.Normal
                    HQ.Settings.MainFormLocation = Me.Location
                    HQ.Settings.MainFormSize = Me.Size
                    HQ.Settings.MainFormWindowState = Me.WindowState
                Case FormWindowState.Maximized
                    HQ.Settings.MainFormWindowState = Me.WindowState
            End Select
        End If
    End Sub

    Public Sub ShutdownRoutine()

        Try
            HQ.MarketCacheUploader.Stop()
            ' Check we aren't updating
            If HQ.EveHQIsUpdating = True Then
                MessageBox.Show(
                    "You can't exit EveHQ while an update is in progress. Please wait until the update has completed and try again.",
                    "Update in Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Disable timers
            HQ.WriteLogEvent("Shutdown: Disabling timers...")
            Me.tmrMemory.Stop()
            Me.tmrEve.Stop()
            HQ.WriteLogEvent("Shutdown: Disabled TQ Status timer")
            Me.tmrSkillUpdate.Stop()
            HQ.WriteLogEvent("Shutdown: Disabled Skill Update timer")
            Me.tmrSave.Stop()
            HQ.WriteLogEvent("Shutdown: Disabled Automatic Save timer")

            ' Check if Shutdown Notification is active (only if not shutting down on request on the updater
            If HQ.Settings.ShutdownNotify = True And HQ.UpdateShutDownRequest = False Then
                HQ.WriteLogEvent("Shutdown: Processing shutdown notifications")
                Dim accounts As New ArrayList
                Dim strNotify As String = ""
                Dim strCharNotify As String = ""
                For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                    If cPilot.Training = True Then
                        Dim timeLimit As Date = Now.AddSeconds(HQ.Settings.ShutdownNotifyPeriod * 3600)
                        If cPilot.TrainingEndTime < timeLimit Then
                            If cPilot.QueuedSkillTime > 0 Then
                                If cPilot.TrainingEndTime.AddSeconds(cPilot.QueuedSkillTime) < timeLimit Then
                                    strCharNotify &= cPilot.Name & " - " & cPilot.TrainingSkillName &
                                                     " (Skill Queue ends in " &
                                                     SkillFunctions.TimeToString(cPilot.QueuedSkillTime) & ")" &
                                                     ControlChars.CrLf
                                End If
                            Else
                                If cPilot.TrainingCurrentTime > 0 Then
                                    strCharNotify &= cPilot.Name & " - " & cPilot.TrainingSkillName &
                                                     " (Training ends in " &
                                                     SkillFunctions.TimeToString(cPilot.TrainingCurrentTime) & ")" &
                                                     ControlChars.CrLf
                                Else
                                    strCharNotify &= cPilot.Name & " - " & cPilot.TrainingSkillName &
                                                     " (Training already complete)" & ControlChars.CrLf
                                End If
                            End If
                        End If
                        accounts.Add(cPilot.Account)
                    End If
                Next
                If strCharNotify <> "" Then
                    strCharNotify = "The following pilots have skills due to end within " &
                                    HQ.Settings.ShutdownNotifyPeriod & " hours:" & ControlChars.CrLf &
                                    ControlChars.CrLf & strCharNotify
                    strNotify &= strCharNotify
                End If
                ' Check each account to see if something is training.
                Dim strAccountNotify As String = ""
                For Each cAccount As EveHQAccount In HQ.Settings.Accounts.Values
                    If cAccount.APIKeyType <> APIKeyTypes.Corporation Then
                        If accounts.Contains(cAccount.userID) = False Then
                            If cAccount.FriendlyName <> "" Then
                                strAccountNotify &= cAccount.FriendlyName & " (UserID: " & cAccount.userID & ")" &
                                                    ControlChars.CrLf
                            Else
                                strAccountNotify &= "UserID: " & cAccount.userID & ControlChars.CrLf
                            End If
                        End If
                    End If
                Next
                If strAccountNotify <> "" Then
                    strAccountNotify = ControlChars.CrLf &
                                       "The following accounts do not appear to have any skill training:" &
                                       ControlChars.CrLf & ControlChars.CrLf & strAccountNotify
                    strNotify &= strAccountNotify
                End If
                If strNotify <> "" Then
                    MessageBox.Show(strNotify, "EveHQ Skill Notification", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
                End If
            End If

            ' Close all the open tabs first
            Dim mainTab As TabStrip = CType(HQ.MainForm.Controls("tabEveHQMDI"), TabStrip)
            If mainTab.Tabs.Count > 0 Then
                For tab As Integer = mainTab.Tabs.Count - 1 To 0 Step -1
                    HQ.WriteLogEvent("Shutdown: Closing tab: " & mainTab.Tabs(tab).Text)
                    CType(mainTab.Tabs(tab).AttachedControl, Form).Close()
                Next
            End If

            ' Save the QAT config if applicable
            HQ.WriteLogEvent("Shutdown: Storing ribbon QAT layout")
            HQ.Settings.QATLayout = RibbonControl1.QatLayout

            ' Check for backup warning expiry
            If HQ.UpdateShutDownRequest = True Then
                If HQ.Settings.EveHQBackupMode = 1 Then
                    Dim backupDate As Date =
                            HQ.Settings.EveHQBackupLast.AddDays(HQ.Settings.EveHQBackupWarnFreq)
                    If backupDate < Now Then
                        Dim timeElapsed As TimeSpan = Now - HQ.Settings.EveHQBackupLast
                        Dim msg As String = "You haven't backed up your EveHQ Settings for " & timeElapsed.Days &
                                            " days. Would you like to do this now?"
                        Dim reply As Integer = MessageBox.Show(msg, "Backup EveHQ Settings?", MessageBoxButtons.YesNo,
                                                               MessageBoxIcon.Question)
                        If reply = DialogResult.Yes Then
                            HQ.WriteLogEvent("Shutdown: Request to backup EveHQ Settings before update")
                            Call EveHQBackup.BackupEveHQSettings()
                        End If
                    End If
                End If
                HQ.WriteLogEvent("Shutdown: Request to save EveHQ Settings before update")
                Call HQ.Settings.Save()
            Else
                If HQ.Settings.EveHQBackupMode = 1 Then
                    HQ.WriteLogEvent("Shutdown: Checking EveHQ backup status before exit")
                    Dim backupDate As Date =
                            HQ.Settings.EveHQBackupLast.AddDays(HQ.Settings.EveHQBackupWarnFreq)
                    If backupDate < Now Then
                        Dim timeElapsed As TimeSpan = Now - HQ.Settings.EveHQBackupLast
                        Dim msg As String = "You haven't backed up your EveHQ Settings for " & timeElapsed.Days &
                                            " days. Would you like to do this now?"
                        Dim reply As Integer = MessageBox.Show(msg, "Backup EveHQ Settings?", MessageBoxButtons.YesNo,
                                                               MessageBoxIcon.Question)
                        If reply = DialogResult.Yes Then
                            HQ.WriteLogEvent("Shutdown: User accepted request to backup EveHQ Settings before exit")
                            Call EveHQBackup.BackupEveHQSettings()
                        Else
                            HQ.WriteLogEvent("Shutdown: User rejected request to backup EveHQ Settings before exit")
                        End If
                    Else
                        HQ.WriteLogEvent("Shutdown: EveHQ backup not required")
                    End If
                End If
                HQ.WriteLogEvent("Shutdown: Request to save EveHQ Settings before exit")
                Call HQ.Settings.Save()
            End If

            ' Remove the icons
            HQ.WriteLogEvent("Shutdown: Dispose of EveHQ icons")
            EveStatusIcon.Visible = False
            iconEveHQMLW.Visible = False
            iconEveHQMLW.Icon = Nothing
            iconEveHQMLW.Dispose()
            EveStatusIcon.Dispose()

            HQ.WriteLogEvent("Shutdown: Shutdown complete")
            ' Close log files

            Trace.Flush()
            Trace.Listeners.Remove(HQ.EveHqTracer)

            'End

        Catch e As Exception
            MessageBox.Show(
                "An error occurred calling the shutdown routine for EveHQ: " & e.Message & " - " & e.StackTrace,
                "Error Closing EveHQ", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If (HQ.LoggingStream.CanWrite) Then


                HQ.LoggingStream.Flush()
                HQ.LoggingStream.Close()
            End If
        End Try
    End Sub

    Private Sub SaveEverything(ByVal silent As Boolean)
        Dim savesDone As List(Of String) = New List(Of String)()
        ' Save Core data and settings
        HQ.Settings.QATLayout = RibbonControl1.QatLayout
        Call HQ.Settings.Save()
        savesDone.Add("EveHQ Core")

        ' Save Plug-in data and settings
        For Each myPlugIn As EveHQ.Core.EveHQPlugIn In HQ.Plugins.Values
            If myPlugIn.Status = PlugIn.PlugInStatus.Active Then
                Dim dataSaved As Boolean = myPlugIn.Instance.SaveAll()
                If dataSaved = True Then
                    savesDone.Add(myPlugIn.Name)
                End If
            End If
        Next

        ' Report result to user
        If silent = False Then
            If savesDone.Count > 0 Then
                Dim msg As String = "Data and settings of the following modules have been saved:"
                For Each moduleName As String In savesDone
                    msg &= vbCrLf & " " & Chr(149) & " " & moduleName
                Next
                MessageBox.Show(msg, "Data Saving Finished", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("No data and settings have been saved.", "Data Saving Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

#End Region

#Region "Skill Display Updater & Notification Routines"

    Private Sub SkillWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles SkillWorker.DoWork
        For Each tPilot As EveHQPilot In HQ.Settings.Pilots.Values
            If tPilot.Active = True Then
                tPilot.TrainingCurrentSP = CInt(SkillFunctions.CalcCurrentSkillPoints(tPilot))
                tPilot.TrainingCurrentTime = SkillFunctions.CalcCurrentSkillTime(tPilot)
            End If
        Next
    End Sub

    Private Sub SkillWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles SkillWorker.RunWorkerCompleted

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
        'If EveHQ.Core.HQ.Settings.ActivateG15 = True And EveHQ.Core.HQ.IsG15LCDActive = True Then
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

    Private Sub tmrSkillUpdate_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrSkillUpdate.Tick
        If SkillWorker.IsBusy = False Then
            SkillWorker.RunWorkerAsync()
            tmrSkillUpdate.Interval = 1000
        End If
    End Sub

    Private Sub CheckNotifications()

        'Mitigation for EVEHQ-92 ... null check the pilots collection before trying to use it.
        If HQ.Settings.Pilots Is Nothing Then
            Return
        End If

        ' Only do this if at least one notification is enabled
        If _
            HQ.Settings.NotifyToolTip = True Or HQ.Settings.NotifyDialog = True Or
            HQ.Settings.NotifyEMail = True Or HQ.Settings.NotifySound = True Then
            Dim notifyText As String = ""

            If HQ.Settings.Pilots Is Nothing Or HQ.Settings.Pilots.Count = 0 Then
                Return ' no pilots not reason to continue.
            End If

            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True And cPilot.Training = True Then
                    notifyText = ""
                    Dim trainingTime As Long = SkillFunctions.CalcCurrentSkillTime(cPilot)
                    ' See if we need to notify about this pilot
                    If trainingTime <= HQ.Settings.NotifyOffset Then
                        If cPilot.TrainingNotifiedEarly = False Then
                            If cPilot.TrainingCurrentTime <= 0 And cPilot.TrainingNotifiedNow = False Then
                                If HQ.Settings.NotifyNow = True Then
                                    notifyText &= cPilot.Name & " has completed training of " & cPilot.TrainingSkillName &
                                                  " to Level " & cPilot.TrainingSkillLevel & "." & ControlChars.CrLf
                                    cPilot.TrainingNotifiedEarly = True
                                    cPilot.TrainingNotifiedNow = True
                                End If
                            Else
                                If HQ.Settings.NotifyEarly = True Then
                                    Dim strTime As String = SkillFunctions.TimeToString(cPilot.TrainingCurrentTime)
                                    strTime = strTime.Replace("s", " seconds").Replace("m", " minutes")
                                    notifyText &= cPilot.Name & " has approximately " & strTime & " before training of " &
                                                  cPilot.TrainingSkillName & " to Level " & cPilot.TrainingSkillLevel &
                                                  " completes." & ControlChars.CrLf
                                    cPilot.TrainingNotifiedEarly = True
                                    cPilot.TrainingNotifiedNow = False
                                End If
                            End If
                        Else
                            If cPilot.TrainingCurrentTime <= 0 And cPilot.TrainingNotifiedNow = False Then
                                If HQ.Settings.NotifyNow = True Then
                                    notifyText &= cPilot.Name & " has completed training of " & cPilot.TrainingSkillName &
                                                  " to Level " & cPilot.TrainingSkillLevel & "." & ControlChars.CrLf
                                    cPilot.TrainingNotifiedEarly = True
                                    cPilot.TrainingNotifiedNow = True
                                End If
                            End If
                        End If

                        ' Show the notifications
                        If notifyText <> "" Then
                            ' If sound is required: Play first as this is automatically put on a separate thread
                            If HQ.Settings.NotifySound = True Then
                                Try
                                    My.Computer.Audio.Play(HQ.Settings.NotifySoundFile, AudioPlayMode.Background)
                                Catch ex As Exception
                                End Try
                            End If
                            ' If tooltip is required:
                            If HQ.Settings.NotifyToolTip = True Then
                                EveStatusIcon.ShowBalloonTip(3000, "Training Notification", notifyText, ToolTipIcon.Info)
                            End If
                            ' If dialog box is required:
                            If HQ.Settings.NotifyDialog = True Then
                                MessageBox.Show(notifyText, "Training Notification", MessageBoxButtons.OK,
                                                MessageBoxIcon.Information)
                            End If
                            ' If email is required:
                            If HQ.Settings.NotifyEMail = True Then
                                ' Expand the details with some additional information
                                If cPilot.QueuedSkills.Count > 0 Then
                                    notifyText &= ControlChars.CrLf
                                    notifyText &= "Next skill in Eve skill queue: " &
                                                  SkillFunctions.SkillIDToName(
                                                      CStr(cPilot.QueuedSkills.Values(0).SkillID)) & " " &
                                                  SkillFunctions.Roman(cPilot.QueuedSkills.Values(0).Level)
                                    notifyText &= ControlChars.CrLf
                                Else
                                    notifyText &= ControlChars.CrLf
                                    notifyText &= "Next skill in Eve skill queue: No skill queued"
                                    notifyText &= ControlChars.CrLf
                                End If
                                If cPilot.TrainingQueues.Count > 0 Then
                                    notifyText &= ControlChars.CrLf
                                    notifyText &= "EveHQ Skill Queue Info: " & ControlChars.CrLf
                                    For Each sq As EveHQSkillQueue In cPilot.TrainingQueues.Values
                                        Dim nq As ArrayList = SkillQueueFunctions.BuildQueue(cPilot, sq, False, True)
                                        If sq.IncCurrentTraining = True Then
                                            If nq.Count > 1 Then
                                                For q As Integer = 1 To nq.Count - 1
                                                    If CType(nq(q), SortedQueueItem).Done = False Then
                                                        notifyText &= sq.Name & ": " &
                                                                      CType(nq(q), SortedQueueItem).Name
                                                        notifyText &= " (" &
                                                                      SkillFunctions.Roman(
                                                                          CInt(CType(nq(q), SortedQueueItem).FromLevel))
                                                        notifyText &= " to " &
                                                                      SkillFunctions.Roman(
                                                                          CInt(CType(nq(q), SortedQueueItem).FromLevel) +
                                                                          1) & ")" & ControlChars.CrLf
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        Else
                                            If nq.Count > 0 Then
                                                For q As Integer = 0 To nq.Count - 1
                                                    If CType(nq(q), SortedQueueItem).Done = False Then
                                                        notifyText &= sq.Name & ": " &
                                                                      CType(nq(q), SortedQueueItem).Name
                                                        notifyText &= " (" &
                                                                      SkillFunctions.Roman(
                                                                          CInt(CType(nq(q), SortedQueueItem).FromLevel))
                                                        notifyText &= " to " &
                                                                      SkillFunctions.Roman(
                                                                          CInt(CType(nq(q), SortedQueueItem).FromLevel) +
                                                                          1) & ")" & ControlChars.CrLf
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

    Private Sub SendEveHQMail(ByVal cpilot As EveHQPilot, ByVal mailText As String)
        Dim eveHQMail As New SmtpClient
        Try
            eveHQMail.Host = HQ.Settings.EMailServer
            eveHQMail.Port = HQ.Settings.EMailPort
            eveHQMail.EnableSsl = HQ.Settings.UseSsl
            If HQ.Settings.UseSmtpAuth = True Then
                Dim newCredentials As New NetworkCredential
                newCredentials.UserName = HQ.Settings.EMailUsername
                newCredentials.Password = HQ.Settings.EMailPassword
                eveHQMail.Credentials = newCredentials
            End If
            Dim eveHQMsg As New MailMessage(HQ.Settings.EmailSenderAddress, HQ.Settings.EMailAddress)
            eveHQMsg.Subject = "Eve Training Notification: " & cpilot.Name & " (" & cpilot.TrainingSkillName & " " &
                               SkillFunctions.Roman(cpilot.TrainingSkillLevel) & ")"
            eveHQMsg.Body = mailText
            eveHQMail.Send(eveHQMsg)
        Catch ex As Exception
            MessageBox.Show(
                "The mail notification sending process failed. Please check that the server, port, address, username and password are correct." &
                ControlChars.CrLf & ControlChars.CrLf & "The error was: " & ex.Message, "EveHQ Email Notification Error",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Public Sub UpdateToNextLevel()
        For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
            If cPilot.Training = True Then
                If cPilot.PilotSkills.ContainsKey(cPilot.TrainingSkillName) = True Then
                    Dim trainSkill As EveHQPilotSkill = cPilot.PilotSkills(cPilot.TrainingSkillName)
                    Dim trainingTime As Long = SkillFunctions.CalcCurrentSkillTime(cPilot)
                    ' See if we need to "update" this level
                    If trainingTime <= 0 And cPilot.TrainingSkillLevel <> trainSkill.Level Then
                        Dim strXML As String = ""

                        ' Browse the skill queue and pick the next available skill
                        Dim pq As EveHQSkillQueue = cPilot.TrainingQueues(cPilot.PrimaryQueue)
                        If pq IsNot Nothing Then
                            Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(cPilot, pq, False, True)
                            Dim qItem As New SortedQueueItem
                            For Each qItem In arrQueue
                                If qItem.Done = False Then
                                    If qItem.IsTraining = False Then
                                        ' Update the skill and move on
                                        If SkillFunctions.ForceSkillTraining(cPilot, qItem.ID, True) = True Then
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
        If HQ.Settings.AutoAPI = True Then
            Dim updateRequired As Boolean = False
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Name <> "" And cPilot.Account <> "" Then
                    Dim cacheCDate As Date = SkillFunctions.ConvertEveTimeToLocal(cPilot.CacheExpirationTime)
                    Dim cacheTDate As Date = SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingExpirationTime)
                    If cacheCDate < Now Or cacheTDate < Now Then
                        updateRequired = True
                        Exit For
                    Else
                        If cacheCDate < HQ.NextAutoAPITime Then
                            HQ.NextAutoAPITime = cacheCDate
                        End If
                        If cacheTDate < HQ.NextAutoAPITime Then
                            HQ.NextAutoAPITime = cacheTDate
                        End If
                        If HQ.AutoRetryAPITime > HQ.NextAutoAPITime Then
                            HQ.NextAutoAPITime = HQ.AutoRetryAPITime
                        End If
                    End If
                End If
            Next
            If Now > HQ.AutoRetryAPITime Then
                If updateRequired = True Then
                    ' Invoke the API Caller
                    HQ.NextAutoAPITime = Now.AddMinutes(60)
                    HQ.AutoRetryAPITime = Now.AddMinutes(5)
                    Call QueryMyEveServer()
                End If
                ' Display time until autoAPI download
                Dim TimeLeft As TimeSpan = HQ.NextAutoAPITime - Now
                lblCharAPITime.Text = SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
            Else
                Dim TimeLeft As TimeSpan = HQ.NextAutoAPITime - Now
                Dim TimeLeft2 As TimeSpan = HQ.AutoRetryAPITime - Now
                lblCharAPITime.Text = SkillFunctions.TimeToString(
                    Math.Max(TimeLeft.TotalSeconds, TimeLeft2.TotalSeconds), False)
            End If
        Else
            lblCharAPITime.Text = ""
        End If
    End Sub

    Private Sub CheckForMailAPIUpdate()
        ' Check if the mail download is in progress
        If EveMailEvents.MailIsBeingProcessed = False Then
            ' Check for an API update if applicable
            If HQ.Settings.AutoMailAPI = True Then
                If Now > HQ.NextAutoMailAPITime Then
                    ' Invoke the API Caller
                    Call Me.UpdateMailNotifications()
                    ' Display time until autoMailAPI download
                    Dim TimeLeft As TimeSpan = HQ.NextAutoMailAPITime - Now
                    lblMailAPITime.Text = SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
                Else
                    Dim TimeLeft As TimeSpan = HQ.NextAutoMailAPITime - Now
                    lblMailAPITime.Text = SkillFunctions.TimeToString(TimeLeft.TotalSeconds, False)
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
        If HQ.APIUpdateInProgress = False Then
            HQ.APIUpdateInProgress = True
            btnQueryAPI.Enabled = False
            frmSettings.btnGetData.Enabled = False
            Dim charThread As New Thread(AddressOf StartCharacterAPIThread)
            charThread.SetApartmentState(ApartmentState.STA) 'Bug EveHQ-118 .. character thread needs to be set to STA
            charThread.IsBackground = True
            charThread.Start()
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
            If HQ.Settings.Accounts.Count = 0 Then
                lblAPIStatus.Text = "API Status: No accounts entered into settings!! (" & Now.ToString & ")"
                Exit Sub
            Else
                lblAPIStatus.Text = "API Status: Fetching Character Data..."
                barStatus.Refresh()
                ' Clear the current list of pilots
                HQ.TPilots.Clear()
                HQ.TCorps.Clear()
                HQ.APIResults.Clear()
                ' get the details for the account
                For Each currentAccount As EveHQAccount In HQ.Settings.Accounts.Values
                    If currentAccount.APIAccountStatus <> APIAccountStatuses.ManualDisabled Then
                        lblAPIStatus.Text = "API Status: Updating Account '" & currentAccount.FriendlyName & "' (ID=" &
                                            currentAccount.UserID & ")..."
                        barStatus.Refresh()
                        Call PilotParseFunctions.GetCharactersInAccount(currentAccount)
                    End If
                Next
                Call PilotParseFunctions.CopyTempPilotsToMain()
                Call PilotParseFunctions.CopyTempCorpsToMain()
            End If

            ' Determine API responses and display appropriate message
            Dim AllCached As Boolean = True
            Dim ContainsNew As Boolean = False
            Dim ContainsErrors As Boolean = False
            For Each result As Integer In HQ.APIResults.Values
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
                lblAPIStatus.Text = "API Status: Last Download - " & Now.ToString &
                                    " (Errors occured - double-click for details)"
            Else
                If AllCached = True Then
                    lblAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (No new updates)"
                Else
                    lblAPIStatus.Text = "API Status: Last Download - " & Now.ToString & " (Update successful)"
                End If
            End If

            If IsHandleCreated Then
                ' Save the settings
                Me.Invoke(New MethodInvoker(AddressOf EveHQ.Core.HQ.Settings.Save))

                ' Enable the option again
                btnQueryAPI.Enabled = True
                Me.Invoke(New MethodInvoker(AddressOf ResetSettingsButton))

                ' Update data
                Me.Invoke(New MethodInvoker(AddressOf UpdatePilotInfo))
            End If
        Catch e As Exception
            If IsHandleCreated Then
                Me.Invoke(Sub()
                              CatchGeneralException(e)
                          End Sub)
            End If

        End Try
        ' We've finished our update routine so we can now release the flag
        HQ.APIUpdateInProgress = False
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

        If HQ.Settings.Pilots.Count = 0 Then
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

        If HQ.Settings.DisableTrainingBar = False Then
            ' Setup a collection for sorting
            Dim PilotTrainingTimes As New ArrayList
            Dim TrainingAccounts As New ArrayList
            Dim DisabledAccounts As New ArrayList
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                ' Check for disabled accounts
                If HQ.Settings.Accounts.ContainsKey(cPilot.Account) Then
                    If HQ.Settings.Accounts(cPilot.Account).APIAccountStatus = APIAccountStatuses.Disabled Then
                        DisabledAccounts.Add(cPilot.Account)
                    Else
                        ' Check for training accounts
                        If cPilot.Training = True Then
                            Dim p As New PilotSortTrainingTime
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
            Dim myClassSorter As New ClassSorter("TrainingEndTime", SortDirection.Ascending)
            ' Always sort by name to handle similarly ranked items in the first sort
            myClassSorter.SortClasses.Add(New SortClass("Name", SortDirection.Ascending))
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
            For Each cAccount As EveHQAccount In HQ.Settings.Accounts.Values
                If DisabledAccounts.Contains(cAccount.userID) = True Then
                    ' Build a status panel if the account is not manually disabled
                    If cAccount.APIAccountStatus <> APIAccountStatuses.ManualDisabled Then
                        Dim cb As New CharacterTrainingBlock(cAccount.userID, True)
                        pdc1.Controls.Add(cb)
                        If Bar1.DockSide = eDockSide.Bottom Or Bar1.DockSide = eDockSide.Top Then
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
                        If _
                            Not _
                            (cAccount.APIKeySystem = APIKeySystems.Version2 And
                             cAccount.APIKeyType = APIKeyTypes.Corporation) Then
                            ' Build a status panel if the account is not manually disabled
                            If cAccount.APIAccountStatus <> APIAccountStatuses.ManualDisabled Then
                                Dim cb As New CharacterTrainingBlock(cAccount.userID, True)
                                pdc1.Controls.Add(cb)
                                If Bar1.DockSide = eDockSide.Bottom Or Bar1.DockSide = eDockSide.Top Then
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
            For Each cPilot As PilotSortTrainingTime In PilotTrainingTimes
                Dim cb As New CharacterTrainingBlock(cPilot.Name, False)
                AddHandler cb.lblSkill.Click, AddressOf Me.TrainingStatusLabelClick
                AddHandler cb.pbPilot.Click, AddressOf Me.PilotPicClick
                AddHandler cb.lblTime.Click, AddressOf Me.TrainingStatusLabelClick
                AddHandler cb.lblQueue.Click, AddressOf Me.TrainingStatusLabelClick
                pdc1.Controls.Add(cb)
                If Bar1.DockSide = eDockSide.Bottom Or Bar1.DockSide = eDockSide.Top Then
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

    Public Sub PilotPicClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim selectedPic As PictureBox = CType(sender, PictureBox)
        Call Me.OpenPilotInfoForm()
        If selectedPic.Name <> "" Then
            frmPilot.DisplayPilotName = selectedPic.Name
        End If
    End Sub

    Public Sub TrainingStatusLabelClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim selectedLabel As LinkLabel = CType(sender, LinkLabel)
        Call Me.OpenSkillTrainingForm()
        If selectedLabel.Name <> "" Then
            frmTraining.DisplayPilotName = selectedLabel.Name
        End If
    End Sub

    Private Sub EveIconMenu_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles EveIconMenu.Opening

        ' Hide the tooltip form
        If EveHQTrayForm IsNot Nothing Then
            EveHQTrayForm.Close()
            EveHQTrayForm = Nothing
        End If

        If HQ.Settings.EveFolder(1) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.Settings.EveFolder(1), "Eve.exe")) = True Then
                If HQ.Settings.EveFolderLabel(1) <> "" Then
                    ctxmnuLaunchEve1.Text = "Launch Eve (" & HQ.Settings.EveFolderLabel(1) & ")"
                End If
                ctxmnuLaunchEve1.Enabled = True
            Else
                ctxmnuLaunchEve1.Enabled = False
            End If
        End If

        If HQ.Settings.EveFolder(2) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.Settings.EveFolder(2), "Eve.exe")) = True Then
                If HQ.Settings.EveFolderLabel(2) <> "" Then
                    ctxmnuLaunchEve2.Text = "Launch Eve (" & HQ.Settings.EveFolderLabel(2) & ")"
                End If
                ctxmnuLaunchEve2.Enabled = True
            Else
                ctxmnuLaunchEve2.Enabled = False
            End If
        End If

        If HQ.Settings.EveFolder(3) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.Settings.EveFolder(3), "Eve.exe")) = True Then
                If HQ.Settings.EveFolderLabel(3) <> "" Then
                    ctxmnuLaunchEve3.Text = "Launch Eve (" & HQ.Settings.EveFolderLabel(3) & ")"
                End If
                ctxmnuLaunchEve3.Enabled = True
            Else
                ctxmnuLaunchEve3.Enabled = False
            End If
        End If

        If HQ.Settings.EveFolder(4) IsNot Nothing Then
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.Settings.EveFolder(4), "Eve.exe")) = True Then
                If HQ.Settings.EveFolderLabel(4) <> "" Then
                    ctxmnuLaunchEve4.Text = "Launch Eve (" & HQ.Settings.EveFolderLabel(4) & ")"
                End If
                ctxmnuLaunchEve4.Enabled = True
            Else
                ctxmnuLaunchEve4.Enabled = False
            End If
        End If
    End Sub

#Region "Backup Worker routines"

    Private Sub tmrBackup_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrBackup.Tick
        If BackupWorker.IsBusy = False Then
            If HQ.Settings.BackupAuto = True Then
                Dim nextBackup As Date = HQ.Settings.BackupStart
                If HQ.Settings.BackupLast > nextBackup Then
                    nextBackup = HQ.Settings.BackupLast
                End If
                nextBackup = DateAdd(DateInterval.Day, HQ.Settings.BackupFreq, nextBackup)
                If Now >= nextBackup Then
                    BackupWorker.RunWorkerAsync()
                End If
            End If
        End If
        If EveHQBackupWorker.IsBusy = False Then
            If HQ.Settings.EveHQBackupMode = 2 Then
                Dim nextBackup As Date = HQ.Settings.EveHQBackupStart
                If HQ.Settings.EveHQBackupLast > nextBackup Then
                    nextBackup = HQ.Settings.EveHQBackupLast
                End If
                nextBackup = DateAdd(DateInterval.Day, HQ.Settings.EveHQBackupFreq, nextBackup)
                If Now >= nextBackup Then
                    EveHQBackupWorker.RunWorkerAsync()
                End If
            End If
        End If
    End Sub

    Private Sub BackupWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BackupWorker.DoWork
        Call frmBackup.BackupEveSettings()
    End Sub

    Private Sub BackupWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles BackupWorker.RunWorkerCompleted

        If HQ.Settings.BackupLastResult = -1 Then
            frmBackup.lblLastBackup.Text = HQ.Settings.BackupLast.ToString
        End If
        Call frmBackup.CalcNextBackup()
        Call frmBackup.ScanBackups()
        If HQ.Settings.BackupLastResult = -1 Then
            lblAPIStatus.Text = "Eve Settings Backup Successful: " & HQ.Settings.BackupLast.ToString
        Else
            lblAPIStatus.Text = "Eve Settings Backup Aborted - No Source Folders"
        End If
    End Sub

    Private Sub EveHQBackupWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) _
        Handles EveHQBackupWorker.DoWork
        Call EveHQBackup.BackupEveHQSettings()
    End Sub

    Private Sub EveHQBackupWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
        Handles EveHQBackupWorker.RunWorkerCompleted
        Call EveHQBackup.CalcNextBackup()
        If frmBackupEveHQ.IsHandleCreated = True Then
            Call frmBackupEveHQ.ScanBackups()
        End If
        If HQ.Settings.EveHQBackupLastResult = -1 Then
            lblAPIStatus.Text = "EveHQ Settings Backup Successful: " & HQ.Settings.EveHQBackupLast.ToString
        Else
            lblAPIStatus.Text = "EveHQ Settings Backup Failed!"
        End If
    End Sub

#End Region

    Private Sub IGBWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles IGBWorker.DoWork
        HQ.myIGB.RunIGB(IGBWorker, e)
    End Sub

#Region "Background Module Loading"

    Private Sub tmrModules_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrModules.Tick
        CheckForIllegalCrossThreadCalls = False
        tmrModules.Stop()
        For Each PlugInInfo As EveHQPlugIn In HQ.Plugins.Values
            ' Override settings if the remote server says so
            Dim serverOverride As Boolean = False
            If HQ.EveHQServerMessage IsNot Nothing Then
                If HQ.EveHQServerMessage.DisabledPlugins.ContainsKey(PlugInInfo.Name) = True Then
                    If PlugInInfo.Version <> "" Then
                        If _
                            CompareVersions(PlugInInfo.Version, HQ.EveHQServerMessage.DisabledPlugins(PlugInInfo.Name)) =
                            True Then
                            serverOverride = True
                        End If
                    End If
                End If
            End If
            If serverOverride = False Then
                If PlugInInfo.Available = True And PlugInInfo.Disabled = False Then
                    'If PlugInInfo.Available = True Then
                    If PlugInInfo.RunAtStartup = True Then
                        'Dim t As New Thread(AddressOf Me.RunModuleStartUps)
                        't.IsBackground = True
                        't.Start(PlugInInfo)
                        ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
                    End If
                ElseIf PlugInInfo.Available = True And PlugInInfo.Disabled = True Then
                    ' Check for initialisation from a parameter
                    If PlugInInfo.PostStartupData IsNot Nothing Then
                        Dim msg As String = PlugInInfo.Name &
                                            " is not configured to run at startup but EveHQ was started with data specifcally for that Plug-In." &
                                            ControlChars.CrLf & ControlChars.CrLf
                        msg &= "Would you like to initialise the Plug-in so the data can be viewed?"
                        If _
                            MessageBox.Show(msg, "Confirm Load Plug-In", MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question) = DialogResult.Yes Then
                            ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub RunModuleStartUps(ByVal State As Object)
        Dim plugInInfo As EveHQPlugIn = CType(State, EveHQPlugIn)
        Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
        Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
        plugInInfo.Instance = CType(Activator.CreateInstance(t), IEveHQPlugIn)
        Dim runPlugIn As IEveHQPlugIn = plugInInfo.Instance
        Dim pluginContainer As ItemContainer = CType(rbPlugins.Items.Item(plugInInfo.Name), ItemContainer)
        Dim LoadPlugInButton As ButtonItem = CType(pluginContainer.SubItems("LPI" & plugInInfo.Name), ButtonItem)
        Dim RunPlugInButton As ButtonItem = CType(pluginContainer.SubItems("RPI" & plugInInfo.Name), ButtonItem)
        LoadPlugInButton.Enabled = False
        RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Loading..."
        LoadPlugInButton.Text = "Loading..."
        RunPlugInButton.Refresh()
        plugInInfo.Status = EveHQPlugInStatus.Loading
        Try
            Dim PlugInResponse As String = ""
            PlugInResponse = runPlugIn.EveHQStartUp().ToString
            If CBool(PlugInResponse) = False Then
                LoadPlugInButton.Enabled = True
                RunPlugInButton.Enabled = False
                LoadPlugInButton.Text = "Load Plug-in"
                RunPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Failed"
                plugInInfo.Status = EveHQPlugInStatus.Failed
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
                plugInInfo.Status = EveHQPlugInStatus.Active
            End If
            ' Clean up after loading the plugin
            Call HQ.ReduceMemory()
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
        If HQ.Settings.Plugins.Count <> 0 Then
            ' Clear the Plug-ins ribbon
            rbPlugins.Items.Clear()
            Dim modCount As Integer = 0
            For Each PlugInInfo As EveHQPlugIn In HQ.Plugins.Values
                If PlugInInfo.Available = True Then
                    modCount += 1
                    ' Create the plug-in container and orientations
                    Dim pluginContainer As New ItemContainer
                    pluginContainer.Name = PlugInInfo.Name
                    pluginContainer.LayoutOrientation = eOrientation.Vertical
                    pluginContainer.MinimumSize = New Size(80, 25)
                    pluginContainer.HorizontalItemAlignment = eHorizontalItemsAlignment.Left
                    pluginContainer.VerticalItemAlignment = eVerticalItemsAlignment.Top

                    ' Create a new plug-in button for the item
                    Dim RunPlugInButton As New ButtonItem
                    RunPlugInButton.ButtonStyle = eButtonStyle.ImageAndText
                    RunPlugInButton.ImagePosition = eImagePosition.Left
                    RunPlugInButton.Image = PlugInInfo.MenuImage
                    RunPlugInButton.ImageFixedSize = New Size(40, 40)
                    RunPlugInButton.Name = "RPI" & PlugInInfo.Name
                    RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Not Loaded"

                    ' Add a shiny tooltip
                    Dim stt As New SuperTooltipInfo
                    stt.FooterText = "EveHQ Plug-in: " & PlugInInfo.Name
                    stt.BodyText = PlugInInfo.Description & ControlChars.CrLf & ControlChars.CrLf
                    stt.BodyText &= "Author: " & PlugInInfo.Author
                    stt.Color = eTooltipColor.Yellow
                    stt.BodyImage = CType(My.Resources.Info32, Image)
                    stt.FooterImage = PlugInInfo.MenuImage
                    SuperTooltip1.SetSuperTooltip(RunPlugInButton, stt)

                    AddHandler RunPlugInButton.Click, AddressOf PlugInIconClick
                    pluginContainer.SubItems.Add(RunPlugInButton)
                    ' Add a load item for each disabled plug-in
                    Dim LoadPlugInButton As New ButtonItem
                    LoadPlugInButton.Name = "LPI" & PlugInInfo.Name
                    LoadPlugInButton.Text = "Load Plug-in"
                    LoadPlugInButton.Tooltip = "Load the " & PlugInInfo.MainMenuText & " Plug-in"
                    LoadPlugInButton.ImageFixedSize = New Size(2, 2)
                    LoadPlugInButton.ButtonStyle = eButtonStyle.TextOnlyAlways
                    LoadPlugInButton.ImagePosition = eImagePosition.Top
                    LoadPlugInButton.CanCustomize = False
                    LoadPlugInButton.Size = New Size(20, 40)
                    AddHandler LoadPlugInButton.Click, AddressOf LoadPlugin
                    pluginContainer.SubItems.Add(LoadPlugInButton)

                    ' Override settings if the remote server says so
                    Dim ServerOverride As Boolean = False
                    If HQ.EveHQServerMessage IsNot Nothing Then
                        If HQ.EveHQServerMessage.DisabledPlugins.ContainsKey(PlugInInfo.Name) = True Then
                            If _
                                CompareVersions(PlugInInfo.Version,
                                                HQ.EveHQServerMessage.DisabledPlugins(PlugInInfo.Name)) = True Then
                                ServerOverride = True
                            End If
                        End If
                    End If

                    If ServerOverride = False Then
                        If PlugInInfo.RunAtStartup = True Then
                            LoadPlugInButton.Enabled = True
                            RunPlugInButton.Enabled = False
                            PlugInInfo.Status = EveHQPlugInStatus.Uninitialised
                        Else
                            If PlugInInfo.Disabled = False Then
                                RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Ready"
                                LoadPlugInButton.Enabled = False
                                LoadPlugInButton.Text = ""
                                RunPlugInButton.Enabled = True
                                PlugInInfo.Status = EveHQPlugInStatus.Active
                            Else
                                LoadPlugInButton.Enabled = True
                                RunPlugInButton.Enabled = False
                                PlugInInfo.Status = EveHQPlugInStatus.Uninitialised
                            End If
                        End If
                    Else
                        RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Disabled"
                        RunPlugInButton.Tooltip = PlugInInfo.MainMenuText &
                                                  " has been disabled remotely due to critical issues!"
                        LoadPlugInButton.Enabled = False
                        LoadPlugInButton.Text = "Disabled"
                        LoadPlugInButton.Tooltip = PlugInInfo.MainMenuText &
                                                   " has been disabled remotely due to critical issues!"

                        RunPlugInButton.Enabled = False
                        PlugInInfo.Status = EveHQPlugInStatus.Uninitialised
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

    Private Sub ModuleMenuItemClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim tp As TabItem = HQ.GetMDITab(mnu.Name)
        If tp IsNot Nothing Then
            tabEveHQMDI.SelectedTab = tp
        Else
            Dim myPlugIn As EveHQPlugIn = HQ.Plugins(mnu.Name)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            Call DisplayChildForm(plugInForm)
        End If
    End Sub

    Private Sub PlugInIconClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As ButtonItem = DirectCast(sender, ButtonItem)
        Dim PlugInName As String = btn.Name.Remove(0, 3)
        Dim tp As TabItem = HQ.GetMDITab(PlugInName)
        If tp IsNot Nothing Then
            tabEveHQMDI.SelectedTab = tp
        Else
            Dim myPlugIn As EveHQPlugIn = HQ.Plugins(PlugInName)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            Call Me.DisplayChildForm(plugInForm)
        End If
    End Sub

    Private Sub LoadPlugin(ByVal sender As Object, ByVal e As EventArgs)
        Dim PIB As ButtonItem = DirectCast(sender, ButtonItem)
        Dim plugInName As String = PIB.Name.Remove(0, 3)
        Dim PlugInInfo As EveHQPlugIn = HQ.Plugins.Item(plugInName)
        If PlugInInfo.RunAtStartup = True Then
            ThreadPool.QueueUserWorkItem(AddressOf Me.RunModuleStartUps, PlugInInfo)
        Else
            Dim pluginContainer As ItemContainer = CType(rbPlugins.Items.Item(PlugInInfo.Name), ItemContainer)
            Dim LoadPlugInButton As ButtonItem = CType(pluginContainer.SubItems("LPI" & PlugInInfo.Name), ButtonItem)
            Dim RunPlugInButton As ButtonItem = CType(pluginContainer.SubItems("RPI" & PlugInInfo.Name), ButtonItem)
            RunPlugInButton.Text = PlugInInfo.Name & ControlChars.CrLf & "Status: Ready"
            LoadPlugInButton.Text = ""
            PlugInInfo.Status = EveHQPlugInStatus.Active
            LoadPlugInButton.Enabled = False
            RunPlugInButton.Enabled = True
        End If
        rbPlugins.Refresh()
    End Sub

    Private Sub RunPlugin(ByVal sender As Object, ByVal e As EventArgs)
        Dim mnu As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim tp As TabItem = HQ.GetMDITab(mnu.Name)
        If tp IsNot Nothing Then
            tabEveHQMDI.SelectedTab = tp
        Else
            Dim myPlugIn As EveHQPlugIn = HQ.Plugins(mnu.Name)
            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
            Call DisplayChildForm(plugInForm)
        End If
    End Sub

    Public Sub LoadAndOpenPlugIn(ByVal State As Object)
        ' Called usually from an instance
        Dim plugInInfo As PlugIn = CType(State, PlugIn)
        Dim myAssembly As Assembly = Assembly.LoadFrom(plugInInfo.FileName)
        Dim t As Type = myAssembly.GetType(plugInInfo.FileType)
        plugInInfo.Instance = CType(Activator.CreateInstance(t), IEveHQPlugIn)
        Dim runPlugIn As IEveHQPlugIn = plugInInfo.Instance
        Dim pluginContainer As ItemContainer = CType(rbPlugins.Items.Item(plugInInfo.Name), ItemContainer)
        Dim loadPlugInButton As ButtonItem = CType(pluginContainer.SubItems("LPI" & plugInInfo.Name), ButtonItem)
        Dim runPlugInButton As ButtonItem = CType(pluginContainer.SubItems("RPI" & plugInInfo.Name), ButtonItem)
        loadPlugInButton.Enabled = False
        runPlugInButton.Enabled = False
        runPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Loading..."
        plugInInfo.Status = EveHQPlugInStatus.Loading
        Try
            Dim plugInResponse As String = ""
            plugInResponse = runPlugIn.EveHQStartUp().ToString
            If CBool(plugInResponse) = False Then
                loadPlugInButton.Enabled = True
                runPlugInButton.Enabled = False
                runPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Failed"
                plugInInfo.Status = EveHQPlugInStatus.Failed
            Else
                runPlugInButton.Text = plugInInfo.Name & ControlChars.CrLf & "Status: Ready"
                plugInInfo.Status = EveHQPlugInStatus.Active
                loadPlugInButton.Enabled = False
                runPlugInButton.Enabled = True
            End If
            ' Clean up after loading the plugin
            Call HQ.ReduceMemory()
            ' Open the Plug-in
            Dim myDelegate As New OpenPlugInDelegate(AddressOf OpenPlugIn)
            Me.Invoke(myDelegate, New Object() {plugInInfo.Name})
        Catch ex As Exception
            MessageBox.Show("Unable to load plugin: " & plugInInfo.Name & ControlChars.CrLf & ex.Message, "Plugin error")
            loadPlugInButton.Enabled = True
            runPlugInButton.Enabled = False
        End Try
    End Sub

    Delegate Sub OpenPlugInDelegate(ByVal plugInName As String)

    Private Sub OpenPlugIn(ByVal plugInName As String)
        Dim plugInInfo As EveHQPlugIn = HQ.Plugins(plugInName)
        If plugInInfo.Status = EveHQPlugInStatus.Active Then
            Dim mainTab As TabStrip = CType(HQ.MainForm.Controls("tabEveHQMDI"), TabStrip)
            Dim tp As TabItem = HQ.GetMDITab(plugInName)
            If tp IsNot Nothing Then
                mainTab.SelectedTab = tp
            Else
                Dim plugInForm As Form = plugInInfo.Instance.RunEveHQPlugIn
                plugInForm.MdiParent = HQ.MainForm
                plugInForm.Show()
            End If
            plugInInfo.Instance.GetPlugInData(plugInInfo.PostStartupData, 0)
        End If
    End Sub

#End Region

#Region "TabbedMDI Window Routines"

    Public Sub OpenPilotInfoForm()
        Dim tp As TabItem = HQ.GetMDITab(frmPilot.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmPilot)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Public Sub OpenSkillTrainingForm()
        Dim tp As TabItem = HQ.GetMDITab(frmTraining.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmTraining)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Public Sub OpenEveHQMailForm()
        Dim tp As TabItem = HQ.GetMDITab(frmMail.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmMail)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenBackUpForm()
        Dim tp As TabItem = HQ.GetMDITab(frmBackup.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmBackup)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenEveHQBackUpForm()
        Dim tp As TabItem = HQ.GetMDITab(frmBackupEveHQ.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmBackupEveHQ)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenAPICheckerForm()
        Dim tp As TabItem = HQ.GetMDITab(frmAPIChecker.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmAPIChecker)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenMarketPricesForm()
        Dim tp As TabItem = HQ.GetMDITab(EveHQMLF.Text)
        If tp Is Nothing Then
            EveHQMLF = New frmMarketPrices
            Call DisplayChildForm(EveHQMLF)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenDashboard()
        Dim tp As TabItem = HQ.GetMDITab(frmDashboard.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmDashboard)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenRequisitions()
        Dim tp As TabItem = HQ.GetMDITab("EveHQ Requisitions")
        If tp Is Nothing Then
            Dim myReq As New frmRequisitions
            Call DisplayChildForm(myReq)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenSQLQueryForm()
        Dim tp As TabItem = HQ.GetMDITab(frmSQLQuery.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmSQLQuery)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub OpenInfoHelpForm()
        Dim tp As TabItem = HQ.GetMDITab(frmHelp.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(frmHelp)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Public Sub DisplayReport(ByRef reportForm As frmReportViewer, ByVal reportText As String)
        reportForm.Text = reportText
        Dim tp As TabItem = HQ.GetMDITab(reportForm.Text)
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

    Public Sub DisplayChartReport(ByRef chartForm As frmChartViewer, ByVal formTitle As String)
        chartForm.Text = formTitle
        Dim tp As TabItem = HQ.GetMDITab(chartForm.Text)
        If tp Is Nothing Then
            Call DisplayChildForm(chartForm)
        Else
            tabEveHQMDI.SelectedTab = tp
        End If
    End Sub

    Private Sub ctxmnuLaunchEve1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxmnuLaunchEve1.Click
        Call LaunchEveInNormalWindow(1)
    End Sub

    Private Sub ctxmnuLaunchEve2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxmnuLaunchEve2.Click
        Call LaunchEveInNormalWindow(2)
    End Sub

    Private Sub ctxmnuLaunchEve3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxmnuLaunchEve3.Click
        Call LaunchEveInNormalWindow(3)
    End Sub

    Private Sub ctxmnuLaunchEve4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ctxmnuLaunchEve4.Click
        Call LaunchEveInNormalWindow(4)
    End Sub

    Private Sub LaunchEveInNormalWindow(ByVal folder As Integer)
        Me.WindowState = FormWindowState.Minimized
        Try
            If HQ.Settings.EveFolderLUA(folder) = True Then
                Process.Start(Path.Combine(HQ.Settings.EveFolder(folder), "Eve.exe"), "/LUA:OFF")
            Else
                Process.Start(Path.Combine(HQ.Settings.EveFolder(folder), "Eve.exe"))
            End If
        Catch ex As Exception
            MessageBox.Show(
                "Unable to start Eve. Please ensure that the location is correctly specified in the EveHQ settings.",
                "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Public Function CacheErrorHandler() As Boolean
        ' Stop the timer from reporting multiple errors
        tmrSkillUpdate.Stop()
        Dim msg As New StringBuilder
        msg.Append("EveHQ has detected that there is an error in the character cache files. ")
        msg.AppendLine(
            "This could be due to a corrupt cache file or a conflict with another skill training application.")
        msg.AppendLine("")
        msg.AppendLine(
            "The issue may be resolved by clearing the EveHQ cache and connecting back to the API. Would you like to do this now?")
        msg.AppendLine("")

        Dim reply As Integer = MessageBox.Show(msg.ToString, "Skill Error", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question)
        If reply = DialogResult.No Then
            ' Don't do anything with the cache but restart the timer
            tmrSkillUpdate.Start()
            Return False
        Else
            ' Close all open forms
            If tabEveHQMDI.Tabs.Count > 0 Then
                For tab As Integer = tabEveHQMDI.Tabs.Count - 1 To 0
                    Dim tp As TabItem = tabEveHQMDI.Tabs(tab)
                    tabEveHQMDI.Tabs.Remove(tp)
                Next
            End If

            ' Clear the EveHQ cache
            Try
                If My.Computer.FileSystem.DirectoryExists(HQ.cacheFolder) Then
                    My.Computer.FileSystem.DeleteDirectory(HQ.cacheFolder, DeleteDirectoryOption.DeleteAllContents)
                End If
            Catch e As Exception
            End Try

            ' Recreate the EveHQ cache folder
            Try
                If My.Computer.FileSystem.DirectoryExists(HQ.cacheFolder) = False Then
                    My.Computer.FileSystem.CreateDirectory(HQ.cacheFolder)
                End If
            Catch e As Exception
            End Try

            ' Clear the EveHQ Pilot Data
            Try
                HQ.Settings.Pilots.Clear()
                HQ.Settings.Corporations.Clear()
                HQ.TPilots.Clear()
                HQ.TCorps.Clear()
            Catch ex As Exception
            End Try

            ' Update the pilot lists
            Call Me.UpdatePilotInfo(True)

            ' Restart the timer
            tmrSkillUpdate.Start()

            ' Call the API
            Call Me.QueryMyEveServer()

            Return True
        End If
    End Function

    Private Sub lblAPIStatus_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles lblAPIStatus.DoubleClick
        If HQ.APIResults.Count > 0 Then
            Dim APIStatus As New EveAPIStatusForm
            APIStatus.ShowDialog()
            APIStatus.Dispose()
        End If
    End Sub

    Private Sub tmrMemory_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrMemory.Tick
        Call HQ.ReduceMemory()
    End Sub

    Private Sub tmrSave_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrSave.Tick
        Call Me.SaveEverything(True)
    End Sub

#Region "Update Check & Menu"

    Private Sub CheckForUpdates(ByVal state As Object)
        Trace.TraceInformation("Checking For Updates")
        Dim DatabaseUpgradeAvailable As Boolean = False
        Dim CurrentComponents As New SortedList
        Dim UpdateXML As XmlDocument = FetchUpdateXML()

        Dim currentVersion As Version
        If UpdateXML Is Nothing Then
            Exit Sub
        Else
            Dim UpdateRequired As Boolean = False
            ' Get a current list of components
            CurrentComponents.Clear()
            Dim msg As String = ""
            currentVersion = My.Application.Info.Version

            ' Try parsing the update file 
            Try
                Dim updateDetails As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/lastUpdated")
                Dim lastUpdate As String = updateDetails(0).InnerText

                Dim updateVersion As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/version")

                Dim installerLocation As XmlNodeList = UpdateXML.SelectNodes("/eveHQUpdate/location")

                If (IsUpdateAvailable(currentVersion.ToString, updateVersion(0).InnerText)) Then
                    Trace.TraceInformation("Update Available")
                    btnUpdateEveHQ.Enabled = True
                    Dim reply As Integer =
                            MessageBox.Show(
                                "There is an update for EveHQ. Would you like to download the latest version?",
                                "Update EveHQ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = DialogResult.No Then
                        Exit Sub
                    Else
                        Me.Invoke(Sub()
                                      ShowUpdateForm(installerLocation(0).InnerText)
                                  End Sub)
                    End If
                Else
                    Trace.TraceInformation("No Update Available")
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub ShowUpdateForm(installerUrl As String)
        Dim myUpdater As New newUpdater(installerUrl, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ"), HQ.RemoteProxy.ProxyServer,
                                                                                        HQ.RemoteProxy.
                                                                                           UseDefaultCredentials,
                                                                                        HQ.RemoteProxy.ProxyUsername,
                                                                                        HQ.RemoteProxy.ProxyPassword,
                                                                                        HQ.RemoteProxy.UseBasicAuthentication)
        myUpdater.Show()
    End Sub

    Private Shared Function IsUpdateAvailable(ByVal localVer As String, ByVal remoteVer As String) As Boolean
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
        Dim UpdateServer As String = HQ.Settings.UpdateURL
        Dim remoteURL As String = UpdateServer & "_evehqupdate.xml"
        Dim UpdateXML As New XmlDocument
        Trace.TraceInformation("Fetching Update XML document from {0}".FormatInvariant(remoteURL))
        Try
            ' Create the requester
            Dim temp As Uri = Nothing
            If Uri.TryCreate(remoteURL, UriKind.Absolute, temp) = False Then
                Return Nothing
            End If

            Dim tempProxy As Uri = Nothing
            If (Uri.TryCreate(HQ.RemoteProxy.ProxyServer, UriKind.Absolute, tempProxy)) = False Then
                tempProxy = Nothing
            End If

            Dim requestTask As Task(Of HttpResponseMessage) = HttpRequestProvider.Default.GetAsync(temp,
                                                                                        tempProxy,
                                                                                        HQ.RemoteProxy.
                                                                                           UseDefaultCredentials,
                                                                                        HQ.RemoteProxy.ProxyUsername,
                                                                                        HQ.RemoteProxy.ProxyPassword,
                                                                                        HQ.RemoteProxy.
                                                                                           UseBasicAuthentication)

            requestTask.Wait()
            If (requestTask.IsFaulted Or requestTask.Exception IsNot Nothing) Then
                Return Nothing
            End If
            Dim readTask As Task(Of String) = requestTask.Result.Content.ReadAsStringAsync()
            readTask.Wait()
            ' Check response string for any error codes?
            UpdateXML.LoadXml(readTask.Result)
            Return UpdateXML
        Catch e As Exception
            Trace.TraceError(e.FormatException())
            Return Nothing
        End Try
    End Function

    Private Sub UpdateNow()
        ' Try and download patchfile
        Dim PatcherLocation As String = HQ.AppDataFolder

        Dim patcherFile As String = Path.Combine(PatcherLocation, "EveHQPatcher.exe")
        Try
            Call Me.DownloadPatcherFile("EveHQPatcher.exe")
            ' Copy the CoreControls.dll file to the same location
            Dim oldCCfile As String = Path.Combine(HQ.appFolder, "EveHQ.CoreControls.dll")
            Dim newCCfile As String = Path.Combine(PatcherLocation, "EveHQ.CoreControls.dll")
            My.Computer.FileSystem.CopyFile(oldCCfile, newCCfile, True)
            'MessageBox.Show("Patcher Deployment Successful!", "Patcher Deployment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch Excep As COMException
            Dim errMsg As String = "Unable to copy Patcher to " & ControlChars.CrLf & ControlChars.CrLf & patcherFile &
                                   ControlChars.CrLf & ControlChars.CrLf
            errMsg &= "Please make sure this file is in the EveHQ program directory before continuing."
            MessageBox.Show(errMsg, "Error Copying Patcher", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End Try
        Dim startInfo As ProcessStartInfo = New ProcessStartInfo()
        startInfo.UseShellExecute = True
        startInfo.WorkingDirectory = Environment.CurrentDirectory
        startInfo.FileName = patcherFile
        Dim args As String = " /App;" & ControlChars.Quote & HQ.appFolder & ControlChars.Quote
        If HQ.IsUsingLocalFolders = True Then
            args &= " /Local;True"
        Else
            args &= " /Local;False"
        End If
        If HQ.Settings.DBFormat = 0 Then
            args &= " /DB;" & ControlChars.Quote & HQ.Settings.DBFilename & ControlChars.Quote
        Else
            args &= " /DB;None"
        End If
        startInfo.Arguments = args
        Dim osInfo As OperatingSystem = Environment.OSVersion
        If osInfo.Version.Major > 5 Then
            startInfo.Verb = "runas"
        End If
        Process.Start(startInfo)
        HQ.StartShutdownEveHQ = True
    End Sub

    Private Function DownloadPatcherFile(ByVal FileNeeded As String) As Boolean

        ' Set a default policy level for the "http:" and "https" schemes.
        Dim policy As HttpRequestCachePolicy = New HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore)

        Dim httpURI As String = HQ.Settings.UpdateURL & FileNeeded
        Dim localFile As String = Path.Combine(HQ.AppDataFolder, FileNeeded)

        ' Create the request to access the server and set credentials
        ServicePointManager.DefaultConnectionLimit = 10
        ServicePointManager.Expect100Continue = False
        Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(httpURI))
        Dim request As HttpWebRequest = CType(HttpWebRequest.Create(httpURI), HttpWebRequest)
        request.CachePolicy = policy
        ' Setup proxy server (if required)
        Call ProxyServerFunctions.SetupWebProxy(request)
        request.CachePolicy = policy
        request.Method = WebRequestMethods.File.DownloadFile
        request.Timeout = 900000
        Try
            Using response As HttpWebResponse = CType(request.GetResponse, HttpWebResponse)
                Dim filesize As Long = CLng(response.ContentLength)
                Using responseStream As Stream = response.GetResponseStream
                    'loop to read & write to file
                    Using fs As New FileStream(localFile, FileMode.Create)
                        Dim buffer(16383) As Byte
                        Dim read As Integer = 0
                        Dim totalBytes As Long = 0
                        Dim percent As Integer = 0
                        Do
                            read = responseStream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, read)
                            totalBytes += read
                            percent = CInt(totalBytes / filesize * 100)
                        Loop Until read = 0
                        'see Note(1)
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

        Dim strSQL As String = ""
        Dim mailData As DataSet
        Dim UnreadMail As Integer = 0

        ' Get a list of the mail messages that are unread
        Try
            strSQL = "SELECT COUNT(*) FROM eveMail WHERE readMail=0;"
            mailData = DataFunctions.GetCustomData(strSQL)
            If mailData IsNot Nothing Then
                If mailData.Tables(0).Rows.Count > 0 Then
                    UnreadMail = CInt(mailData.Tables(0).Rows(0).Item(0))
                End If
            End If
        Catch ex As Exception
            Dim msg As String = ex.Message & ControlChars.CrLf & ControlChars.CrLf
            If ex.InnerException IsNot Nothing Then
                msg &= ex.InnerException.Message & ControlChars.CrLf & ex.InnerException.StackTrace
            End If
            MessageBox.Show(msg, "Update EveMail Button Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' Get a list of the notifications that are unread
        strSQL = "SELECT COUNT(*) FROM eveNotifications WHERE readMail=0;"
        mailData = DataFunctions.GetCustomData(strSQL)
        Dim unreadNotices As Integer = 0
        If mailData IsNot Nothing Then
            If mailData.Tables(0).Rows.Count > 0 Then
                unreadNotices = CInt(mailData.Tables(0).Rows(0).Item(0))
            End If
        End If

        lblEveMail.Text = "EveMail: " & unreadMail.ToString & ControlChars.CrLf & "Notices: " & unreadNotices.ToString
        btnEveMail.Tooltip = "View Mail && Notifications" & ControlChars.CrLf & "Unread: " & unreadMail.ToString &
                             " mails, " & unreadNotices.ToString & " notifications"
    End Sub

    Private Sub UpdateMailNotifications()
        If EveMailEvents.MailIsBeingProcessed = False Then
            ThreadPool.QueueUserWorkItem(AddressOf MailUpdateThread, frmMail.IsHandleCreated)
        End If
    End Sub

    Private Sub MailUpdateThread(ByVal MailFormOpen As Object)

        ' Set the processing flag
        EveMailEvents.MailIsBeingProcessed = True

        ' Check for the AutoMailAPI flag
        Dim requiresAutoDisable As Boolean = False
        If HQ.Settings.AutoMailAPI = True Then
            requiresAutoDisable = True
        End If
        ' Disable the AutoMailAPI flag if required
        If requiresAutoDisable = True Then
            HQ.Settings.AutoMailAPI = False
        End If

        Me.Invoke(New MethodInvoker(AddressOf Me.UpdateMailAPILabelStart))
        EveMailEvents.MailUpdateStart()

        ' Call the main routines!
        Dim myMail As New EveMail
        Call myMail.GetMail()

        Me.Invoke(New MethodInvoker(AddressOf Me.UpdateMailAPILabelEnd))
        EveMailEvents.MailUpdateComplete()

        ' Update the main EveMail button
        Call Me.UpdateEveMailButton()

        ' Set the AutoMailAPI flag if required
        If requiresAutoDisable = True Then
            HQ.Settings.AutoMailAPI = True
        End If

        EveMailEvents.MailIsBeingProcessed = False
    End Sub

    Private Sub UpdateMailAPILabelStart()
        lblMailAPITime.Text = "Processing..."
    End Sub

    Private Sub UpdateMailAPILabelEnd()
        lblMailAPITime.Text = "Updating..."
    End Sub

#End Region

    Public Shared Sub CatchUIThreadException(ByVal sender As Object, ByVal t As ThreadExceptionEventArgs)
        Trace.TraceWarning("Unhandled Exception was caught from AppDomain.")
        CatchGeneralException(t.Exception)
    End Sub

    Public Shared Sub CatchAppDomainUnhandledException(sender As Object, args As UnhandledExceptionEventArgs)
        Trace.TraceWarning("Unhandled Exception was caught from AppDomain.")
        CatchGeneralException(CType(args.ExceptionObject, Exception))
    End Sub

    Public Shared Sub CatchGeneralException(ByRef e As Exception)

        Diagnostics.Trace.TraceError(e.FormatException())

        Dim handle As Form
        For Each window As Form In Application.OpenForms
            handle = window
            Exit For
        Next

        Dim myException As New frmException
        myException.lblVersion.Text = "Version: " & My.Application.Info.Version.ToString
        myException.lblError.Text = e.Message
        Dim trace As New StringBuilder
        trace.AppendLine(e.FormatException)
        trace.AppendLine("")
        trace.AppendLine("========== Plug-ins ==========")
        trace.AppendLine("")
        For Each myPlugIn As EveHQPlugIn In HQ.Plugins.Values
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
        trace.AppendLine("EveHQ Location: " & HQ.appFolder)
        trace.AppendLine("EveHQ Cache Locations: " & HQ.AppDataFolder)
        myException.txtStackTrace.Text = trace.ToString
        Dim result As Integer = myException.ShowDialog()
        If result = DialogResult.Ignore Then
        Else
            Call frmEveHQ.ShutdownRoutine()
        End If
    End Sub

#Region "Ribbon Button Routines"

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Call Me.SaveEverything(False)
    End Sub

    Private Sub btnManageAPI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnManageAPI.Click
        Dim EveHQSettings As New frmSettings
        EveHQSettings.Tag = "nodeEveAccounts"
        EveHQSettings.ShowDialog()
        EveHQSettings.Dispose()
    End Sub

    Private Sub btnQueryAPI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnQueryAPI.Click
        Call Me.QueryMyEveServer()
    End Sub

    Private Sub btnViewPilotInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewPilotInfo.Click
        Call Me.OpenPilotInfoForm()
    End Sub

    Private Sub btnViewSkillTraining_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnViewSkillTraining.Click
        Call Me.OpenSkillTrainingForm()
    End Sub

    Private Sub btnViewPrices_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewPrices.Click
        Call Me.OpenMarketPricesForm()
    End Sub

    Private Sub btnViewDashboard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewDashboard.Click
        Call Me.OpenDashboard()
    End Sub

    Private Sub btnEveMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEveMail.Click
        Call Me.OpenEveHQMailForm()
    End Sub

    Private Sub btnViewReqs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewReqs.Click
        Call Me.OpenRequisitions()
    End Sub

    Private Sub btnIGB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIGB.Click
        If HQ.IGBActive = False Then
            If IGBWorker.CancellationPending = True Then
                MessageBox.Show("The IGB Server is still shutting down. Please wait a few moments", "IGB Server Busy",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                IGBWorker.Dispose()
                IGBWorker = New BackgroundWorker
            Else
                If IGBCanBeInitialised() = True Then
                    IGBWorker.WorkerSupportsCancellation = True
                    IGBWorker.RunWorkerAsync()
                    HQ.IGBActive = True
                    btnIGB.Checked = True
                    lblIGB.Text = "Port: " & HQ.Settings.IGBPort.ToString & ControlChars.CrLf & "Status: On"
                End If
            End If
        Else
            IGBWorker.CancelAsync()
            HQ.IGBActive = False
            btnIGB.Checked = False
            lblIGB.Text = "Port: " & HQ.Settings.IGBPort.ToString & ControlChars.CrLf & "Status: Off"
        End If
    End Sub

    Private Sub btnBackupEveHQ_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackupEveHQ.Click
        Call Me.OpenEveHQBackUpForm()
    End Sub

    Private Sub btnBackupEve_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackupEve.Click
        Call Me.OpenBackUpForm()
    End Sub

    Private Sub btnFileSettings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFileSettings.Click
        frmSettings.ShowDialog()
    End Sub

    Private Sub btnFileExit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFileExit.Click
        Me.Close()
    End Sub

    Private Sub btnAPIChecker_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAPIChecker.Click
        Call Me.OpenAPICheckerForm()
    End Sub

    Private Sub btnOpenCacheFolder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpenCacheFolder.Click
        Try
            Process.Start(HQ.AppDataFolder)
        Catch ex As Exception
            MessageBox.Show("Unable to start Windows Explorer: " & ex.Message, "Error Starting External Process",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub btnClearCharacterCache_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnClearCharacterCache.Click
        Dim msg As String =
                "This will delete the character specific XML files, clear the pilot data and reconnect to the API." &
                ControlChars.CrLf & ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Cache", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Close all open forms
                If tabEveHQMDI.Tabs.Count > 0 Then
                    For tab As Integer = tabEveHQMDI.Tabs.Count - 1 To 0
                        Dim tp As TabItem = tabEveHQMDI.Tabs(tab)
                        tabEveHQMDI.Tabs.Remove(tp)
                    Next
                End If

                ' Clear the character XML files
                Try
                    For Each charFile As String In _
                        My.Computer.FileSystem.GetFiles(HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly,
                                                        "EVEHQAPI_" & APITypes.CharacterSheet.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the skill training XML files
                Try
                    For Each charFile As String In _
                        My.Computer.FileSystem.GetFiles(HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly,
                                                        "EVEHQAPI_" & APITypes.SkillTraining.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the skill queue XML files
                Try
                    For Each charFile As String In _
                        My.Computer.FileSystem.GetFiles(HQ.cacheFolder, FileIO.SearchOption.SearchTopLevelOnly,
                                                        "EVEHQAPI_" & APITypes.SkillQueue.ToString & "*")
                        My.Computer.FileSystem.DeleteFile(charFile)
                    Next
                Catch ex As Exception
                End Try

                ' Clear the EveHQ Pilot Data
                Try
                    HQ.Settings.Pilots.Clear()
                    HQ.Settings.Corporations.Clear()
                    HQ.TPilots.Clear()
                    HQ.TCorps.Clear()
                Catch ex As Exception
                End Try

                ' Update the pilot lists
                Call Me.UpdatePilotInfo(True)

                ' Restart the timer
                tmrSkillUpdate.Start()

                ' Call the API
                Call Me.QueryMyEveServer()

            Catch ex As Exception
                MessageBox.Show(
                    "Error Deleting the EveHQ Cache Folder, please try to delete the following location manually: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.cacheFolder, "Error Deleting Cache", MessageBoxButtons.OK,
                    MessageBoxIcon.Information)
            End Try
        End If
    End Sub

    Private Sub btnClearImageCache_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearImageCache.Click
        Dim msg As String = "This will delete the entire contents of the image cache folder." & ControlChars.CrLf &
                            ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Image Cache", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Clear the EveHQ image cache
                Try
                    If My.Computer.FileSystem.DirectoryExists(HQ.imageCacheFolder) Then
                        My.Computer.FileSystem.DeleteDirectory(HQ.imageCacheFolder,
                                                               DeleteDirectoryOption.DeleteAllContents)
                    End If
                Catch ex As Exception
                End Try

                ' Recreate the EveHQ image cache folder
                Try
                    If My.Computer.FileSystem.DirectoryExists(HQ.imageCacheFolder) = False Then
                        My.Computer.FileSystem.CreateDirectory(HQ.imageCacheFolder)
                    End If
                Catch ex As Exception
                End Try

            Catch ex As Exception
                MessageBox.Show(
                    "Error Deleting the EveHQ Image Cache Folder, please try to delete the following location manually: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.imageCacheFolder, "Error Deleting Cache",
                    MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End If
    End Sub

    Private Sub btnClearAllCache_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearAllCache.Click
        Dim msg As String =
                "This will delete the entire contents of the cache folder, clear the pilot data and reconnect to the API." &
                ControlChars.CrLf & ControlChars.CrLf & "Are you sure you wish to continue?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Cache", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Try
                ' Close all open forms
                If tabEveHQMDI.Tabs.Count > 0 Then
                    For tab As Integer = tabEveHQMDI.Tabs.Count - 1 To 0
                        Dim tp As TabItem = tabEveHQMDI.Tabs(tab)
                        tabEveHQMDI.Tabs.Remove(tp)
                    Next
                End If

                ' Clear the EveHQ cache
                Try
                    If My.Computer.FileSystem.DirectoryExists(HQ.cacheFolder) Then
                        My.Computer.FileSystem.DeleteDirectory(HQ.cacheFolder, DeleteDirectoryOption.DeleteAllContents)
                    End If
                Catch ex As Exception
                End Try

                ' Recreate the EveHQ cache folder
                Try
                    If My.Computer.FileSystem.DirectoryExists(HQ.cacheFolder) = False Then
                        My.Computer.FileSystem.CreateDirectory(HQ.cacheFolder)
                    End If
                Catch ex As Exception
                End Try

                ' Clear the EveHQ Pilot Data
                Try
                    HQ.Settings.Pilots.Clear()
                    HQ.Settings.Corporations.Clear()
                    HQ.TPilots.Clear()
                    HQ.TCorps.Clear()
                Catch ex As Exception
                End Try

                ' Update the pilot lists
                Call Me.UpdatePilotInfo(True)

                ' Restart the timer
                tmrSkillUpdate.Start()

                ' Call the API
                Call Me.QueryMyEveServer()

            Catch ex As Exception
                MessageBox.Show(
                    "Error Deleting the EveHQ Cache Folder, please try to delete the following location manually: " &
                    ControlChars.CrLf & ControlChars.CrLf & HQ.cacheFolder, "Error Deleting Cache", MessageBoxButtons.OK,
                    MessageBoxIcon.Information)
            End Try
        End If
    End Sub

    Private Sub btnCheckForUpdates_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCheckForUpdates.Click
        Call CheckForUpdates(Nothing)
    End Sub

    Private Sub btnUpdateEveHQ_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateEveHQ.Click
        Call Me.UpdateNow()
        Me.Close()
    End Sub

    Private Sub btnViewHistory_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnViewHistory.Click
        Try
            Process.Start("http://evehq.net/wiki/doku.php?id=guide:history")
        Catch ex As Exception
            ' Guess the user needs to reset the http protocol in the OS - not much EveHQ can do here!
        End Try
    End Sub

    Private Sub btnAbout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAbout.Click
        Dim AboutForm As New frmAbout
        AboutForm.ShowDialog()
        AboutForm.Dispose()
    End Sub

    Private Sub btnSQLQueryTool_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSQLQueryTool.Click
        Call Me.OpenSQLQueryForm()
    End Sub

    Private Sub btnInfoHelp_Click(sender As Object, e As EventArgs) Handles btnInfoHelp.Click
        Call Me.OpenInfoHelpForm()
    End Sub

#Region "Ribbon Report Functions"

#Region "Report Options Routines"

    Private Sub UpdateReportPilots()
        cboReportPilot.Items.Clear()
        For Each rPilot As EveHQPilot In HQ.Settings.Pilots.Values
            If rPilot.Active = True Then
                cboReportPilot.Items.Add(rPilot.Name)
            End If
        Next
        If cboReportPilot.Items.Count > 0 Then
            If cboReportPilot.Items.Contains(HQ.Settings.StartupPilot) = True Then
                cboReportPilot.SelectedItem = HQ.Settings.StartupPilot
            Else
                cboReportPilot.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cboReportPilot_SelectedIndexChaged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles cboReportPilot.SelectedIndexChanged
        Call Me.BuildQueueReportsMenu()
    End Sub

    Private Sub cboReportFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles cboReportFormat.SelectedIndexChanged
        ' Get the name of the selected item
        Dim selItem As String = cboReportFormat.SelectedItem.ToString
        ' Cycle through the ribbon bars to hide the non-applicable ones
        For Each rb As RibbonBar In rpReports.Controls
            If rb.Name = "rb" & selItem Or rb.Name = "rbReportOptions" Or rb.Name = "rbStandard" Then
                rb.Visible = True
            Else
                rb.Visible = False
            End If
        Next
        rpReports.Refresh()
    End Sub

    Private Sub btnOpenReportFolder_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnOpenReportFolder.Click
        Try
            Process.Start(HQ.reportFolder)
        Catch ex As Exception
            MessageBox.Show("Unable to start Windows Explorer: " & ex.Message, "Error Starting External Process",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Public Sub BuildQueueReportsMenu()
        ' Clear option for btnHTMLTrainingQueue
        For Each queueBtn As ButtonItem In btnHTMLTrainingQueue.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear option for btnHTMLQueueShoppingList
        For Each queueBtn As ButtonItem In btnHTMLQueueShoppingList.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear option for btnTextTrainingQueue
        For Each queueBtn As ButtonItem In btnTextTrainingQueue.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear option for btnTextQueueShoppingList
        For Each queueBtn As ButtonItem In btnTextQueueShoppingList.SubItems
            RemoveHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
        Next
        ' Clear the existing options
        btnHTMLTrainingQueue.SubItems.Clear()
        btnHTMLQueueShoppingList.SubItems.Clear()
        btnTextTrainingQueue.SubItems.Clear()
        btnTextQueueShoppingList.SubItems.Clear()
        ' Rebuild the queue and shopping list options based on the pilot
        If cboReportPilot.SelectedItem IsNot Nothing Then
            If HQ.Settings.Pilots.ContainsKey(cboReportPilot.SelectedItem.ToString) Then
                Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
                If rPilot IsNot Nothing Then
                    If rPilot.TrainingQueues IsNot Nothing Then
                        For Each qItem As EveHQSkillQueue In rPilot.TrainingQueues.Values
                            Dim queueBtn As New ButtonItem
                            queueBtn.CanCustomize = False
                            queueBtn.Text = qItem.Name
                            queueBtn.Name = qItem.Name
                            queueBtn.Image = My.Resources.SkillBook16
                            AddHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
                            btnHTMLTrainingQueue.SubItems.Add(queueBtn)
                            queueBtn = New ButtonItem
                            queueBtn.CanCustomize = False
                            queueBtn.Text = qItem.Name
                            queueBtn.Name = qItem.Name
                            queueBtn.Image = My.Resources.SkillBook16
                            AddHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
                            btnHTMLQueueShoppingList.SubItems.Add(queueBtn)
                            queueBtn = New ButtonItem
                            queueBtn.CanCustomize = False
                            queueBtn.Text = qItem.Name
                            queueBtn.Name = qItem.Name
                            queueBtn.Image = My.Resources.SkillBook16
                            AddHandler queueBtn.Click, AddressOf Me.ReportsMenuHandler
                            btnTextTrainingQueue.SubItems.Add(queueBtn)
                            queueBtn = New ButtonItem
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
        End If
    End Sub

    Private Sub ReportsMenuHandler(ByVal sender As Object, ByVal e As EventArgs)
        ' Identify queue name
        Dim queueBtn As ButtonItem = CType(sender, ButtonItem)
        Dim queueName As String = queueBtn.Text
        ' Find parent button name
        Dim reportType As String = queueBtn.Parent.Name
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Setup report details
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Dim rQueue As EveHQSkillQueue = rPilot.TrainingQueues(queueName)
        Select Case reportType
            Case "btnHTMLTrainingQueue"
                Call Reports.GenerateTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder,
                                                         "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "btnHTMLQueueShoppingList"
                Call Reports.GenerateShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder,
                                                         "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").html"))
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "btnTextTrainingQueue"
                Call Reports.GenerateTextTrainQueue(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder,
                                                         "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            Case "btnTextQueueShoppingList"
                Call Reports.GenerateTextShoppingList(rPilot, rQueue)
                newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder,
                                                         "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
                DisplayReport(newReport, "Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
        End Select
    End Sub

#End Region

#Region "Standard Reports"

    Private Sub btnStdCharSummary_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStdCharSummary.Click
        Dim newReport As New frmReportViewer
        Call Reports.GenerateCharSummary()
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "PilotSummary.html"))
        DisplayReport(newReport, "Pilot Summary")
    End Sub

    Private Sub btnStdSkillLevels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStdSkillLevels.Click
        Dim newReport As New frmReportViewer
        Call Reports.GenerateSPSummary()
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SPSummary.html"))
        DisplayReport(newReport, "Skill Point Summary")
    End Sub

    Private Sub btnStdAlloyReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStdAlloyReport.Click
        Dim newReport As New frmReportViewer
        Call Reports.GenerateAlloyReport()
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "AlloyReport.html"))
        DisplayReport(newReport, "Alloy Composition")
    End Sub

    Private Sub btnStdAsteroidReport_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnStdAsteroidReport.Click
        Dim newReport As New frmReportViewer
        Call Reports.GenerateRockReport()
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "OreReport.html"))
        DisplayReport(newReport, "Asteroid Composition")
    End Sub

    Private Sub btnStdIceReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStdIceReport.Click
        Dim newReport As New frmReportViewer
        Call Reports.GenerateIceReport()
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "IceReport.html"))
        DisplayReport(newReport, "Ice Composition")
    End Sub

#End Region

#Region "HTML Reports"

    Private Sub btnHTMLCharSheet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHTMLCharSheet.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateCharSheet(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "CharSheet (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLTrainingTimes_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnHTMLTrainingTimes.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTrainingTime(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "TrainTime (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Training Times - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLTimeToLvl5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHTMLTimeToLvl5.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTimeToLevel5(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillLevels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHTMLSkillLevels.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateSkillLevels(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillsAvailable_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnHTMLSkillsAvailable.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateSkillsAvailable(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillsNotTrained_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnHTMLSkillsNotTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateSkillsNotTrained(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLPartiallyTrained_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnHTMLPartiallyTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GeneratePartialSkills(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
    End Sub

    Private Sub btnHTMLSkillsCost_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHTMLSkillsCost.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateSkillsCost(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillsCost (" & rPilot.Name & ").html"))
        DisplayReport(newReport, "Skills Cost - " & rPilot.Name)
    End Sub

#End Region

#Region "Text Reports"

    Private Sub btnTextCharacterSheet_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnTextCharacterSheet.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextCharSheet(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "CharSheet (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Character Sheet - " & rPilot.Name)
    End Sub

    Private Sub btnTextTrainingTimes_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnTextTrainingTimes.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextTrainingTime(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "TrainTime (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Training Times - " & rPilot.Name)
    End Sub

    Private Sub btnTextTimeToLvl5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTextTimeToLvl5.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextTimeToLevel5(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Time To Level 5 - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillLevels_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTextSkillLevels.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextSkillLevels(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skill Levels - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillsAvailable_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnTextSkillsAvailable.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextSkillsAvailable(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skills Available to Train - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillsNotTrained_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnTextSkillsNotTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextSkillsNotTrained(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skills Not Trained - " & rPilot.Name)
    End Sub

    Private Sub btnTextPartiallyTrained_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnTextPartiallyTrained.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextPartialSkills(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Partially Trained Skills - " & rPilot.Name)
    End Sub

    Private Sub btnTextSkillsCost_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTextSkillsCost.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTextSkillsCost(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "SkillsCost (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "Skills Cost - " & rPilot.Name)
    End Sub

#End Region

#Region "XML Reports"

    Private Sub btnXMLCharacterXML_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnXMLCharacterXML.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateCharXML(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "CharXML (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Imported Character XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLTrainingXML_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnXMLTrainingXML.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateTrainXML(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "TrainingXML (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Imported Training XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLCurrentCharOld_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnXMLCurrentCharOld.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateCurrentPilotXML_Old(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "CurrentXML - Old (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Old Style Character XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLCurrentCharNew_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnXMLCurrentCharNew.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateCurrentPilotXML_New(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "CurrentXML - New (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Current Character XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLCurrentTrainingOld_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnXMLCurrentTrainingOld.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GenerateCurrentTrainingXML_Old(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "TrainingXML - Old (" & rPilot.Name & ").xml"))
        DisplayReport(newReport, "Old Style Training XML - " & rPilot.Name)
    End Sub

    Private Sub btnXMLECMExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnXMLECMExport.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Call Reports.GenerateECMExportReports(rPilot)
    End Sub

#End Region

#Region "PHPBB Reports"

    Private Sub btnPHPBBCharacterSheet_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnPHPBBCharacterSheet.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newReport As New frmReportViewer
        Call Reports.GeneratePHPBBCharSheet(rPilot)
        newReport.wbReport.Navigate(Path.Combine(HQ.reportFolder, "PHPBBCharSheet (" & rPilot.Name & ").txt"))
        DisplayReport(newReport, "PHPBB Character Sheet - " & rPilot.Name)
    End Sub

#End Region

#Region "Chart Reports"

    Private Sub btnChartSkillGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChartSkillGroup.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newChartForm As New frmChartViewer
        newChartForm.Controls.Add(Reports.SkillGroupChart(rPilot))
        Call Me.DisplayChartReport(newChartForm, "Skill Group Chart - " & rPilot.Name)
    End Sub

    Private Sub btnChartSkillCost_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChartSkillCost.Click
        If cboReportPilot.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a pilot before running this report!", "Pilot Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rPilot As EveHQPilot = HQ.Settings.Pilots(cboReportPilot.SelectedItem.ToString)
        Dim newChartForm As New frmChartViewer
        newChartForm.Controls.Add(Reports.SkillCostChart(rPilot))
        Call Me.DisplayChartReport(newChartForm, "Skill Cost Chart - " & rPilot.Name)
    End Sub

#End Region

#End Region

#End Region

#Region "Training Bar Routines"

    Private Sub Bar1_BarDock(ByVal sender As Object, ByVal e As EventArgs) Handles Bar1.BarDock
        HQ.Settings.TrainingBarDockPosition = Bar1.DockSide
        Select Case Bar1.DockSide
            Case eDockSide.Top, eDockSide.Bottom
                'DockContainerItem1.Height = 75
            Case eDockSide.Left, eDockSide.Right
                'DockContainerItem1.Width = 320
        End Select
        Call Me.SetupTrainingStatus()
    End Sub

    Private Sub Bar1_BarUndock(ByVal sender As Object, ByVal e As EventArgs) Handles Bar1.BarUndock
        HQ.Settings.TrainingBarDockPosition = Bar1.DockSide
        Call Me.SetupTrainingStatus()
    End Sub

    Private Sub Bar1_SizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Bar1.SizeChanged
        If appStartUp = False And saveTrainingBarSize = True Then
            HQ.Settings.TrainingBarHeight = DockContainerItem1.Height + 3
            HQ.Settings.TrainingBarWidth = DockContainerItem1.Width
        End If
    End Sub

#End Region


#Region "Theme Modification and Automatic Color Scheme creation based on the selected color table"

    Private m_ColorSelected As Boolean = False
    Private m_ManagerStyle As eStyle = eStyle.Office2007Black

    Private Sub buttonStyleCustom_ExpandChange(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnCustomTheme.ExpandChange
        If btnCustomTheme.Expanded Then
            ' Remember the starting color scheme to apply if no color is selected during live-preview
            m_ColorSelected = False
            m_ManagerStyle = StyleManager.Style
        Else
            If Not m_ColorSelected Then
                UpdateTint(Color.Empty)
                UpdateTheme(m_ManagerStyle, Color.Empty)
            End If
        End If
    End Sub

    Private Sub buttonStyleCustom_ColorPreview(ByVal sender As Object, ByVal e As ColorPreviewEventArgs) _
        Handles btnCustomTheme.ColorPreview
        Try
            UpdateTint(e.Color)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub buttonStyleCustom_SelectedColorChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnCustomTheme.SelectedColorChanged
        m_ColorSelected = True
        ' Indicate that color was selected for buttonStyleCustom_ExpandChange method
        btnCustomTheme.CommandParameter = btnCustomTheme.SelectedColor
    End Sub

    Private Sub AppCommandTheme_Executed(ByVal sender As Object, ByVal e As EventArgs) Handles AppCommandTheme.Executed
        Dim source As ICommandSource = CType(sender, ICommandSource)
        If TypeOf (source.CommandParameter) Is String Then
            Dim cs As eStyle = CType([Enum].Parse(GetType(eStyle), source.CommandParameter.ToString()), eStyle)
            ' This is all that is needed to change the color table for all controls on the form
            UpdateTheme(cs, Color.Empty)
            HQ.Settings.ThemeStyle = cs
            HQ.Settings.ThemeSetByUser = True
            UpdateTint(Color.Empty)
            HQ.Settings.ThemeTint = Color.Empty
        ElseIf TypeOf (source.CommandParameter) Is Color Then
            UpdateTint(CType(source.CommandParameter, Color))
            HQ.Settings.ThemeTint = StyleManager.ColorTint
            HQ.Settings.ThemeSetByUser = True
        End If
        Me.Invalidate()
    End Sub

#End Region

    Private Sub RibbonControl1_ExpandedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles RibbonControl1.ExpandedChanged
        HQ.Settings.RibbonMinimised = Not RibbonControl1.Expanded
    End Sub

    Private Sub DisplayChildForm(ByVal childForm As Form)
        If ActiveMdiChild IsNot Nothing Then
            If ActiveMdiChild.WindowState = FormWindowState.Maximized Then
                ActiveMdiChild.WindowState = FormWindowState.Normal
            End If
        End If
        ChildForm.MdiParent = Me
        ChildForm.WindowState = FormWindowState.Maximized
        ChildForm.Show()
    End Sub

    Private Function IGBCanBeInitialised() As Boolean
        Dim prefixes(0) As String
        prefixes(0) = "http://localhost:" & HQ.Settings.IGBPort & "/"

        ' URI prefixes are required
        If prefixes Is Nothing OrElse prefixes.Length = 0 Then
            Throw New ArgumentException("prefixes")
        End If

        ' Create a listener and add the prefixes.
        Dim listener As New HttpListener()
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
            Dim msg As String = "The IGB Server has been disabled due to a failure to initialise correctly." &
                                ControlChars.CrLf & ControlChars.CrLf
            msg &=
                "This is usually caused by insufficient permissions on the host machine or an incompatible (older) operating system." &
                ControlChars.CrLf & ControlChars.CrLf
            msg &=
                "More information and resolutions can be found at http://forum.battleclinic.com/index.php/topic,42896.0/IGB-not-working.html"
            Dim _
                STI As _
                    New SuperTooltipInfo("IGB Server Access Error", "IGB Server Disabled", msg, Nothing,
                                         My.Resources.Info32, eTooltipColor.Yellow)
            SuperTooltip1.SetSuperTooltip(btnIGB, STI)
        Finally
            listener = Nothing
        End Try

        Return IGBCanBeInitialised
    End Function

    Private Sub btnCreateCoreCache_Click(sender As Object, e As EventArgs) Handles btnCreateCoreCache.Click
        Call DataFunctions.CreateCoreCache()
    End Sub

    Private Sub DeleteCacheFolders()
        If My.Computer.FileSystem.DirectoryExists(HQ.coreCacheFolder) = True Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.DeleteDirectory(HQ.coreCacheFolder, DeleteDirectoryOption.DeleteAllContents)
        End If

        If My.Computer.FileSystem.DirectoryExists(HQ.cacheFolder) = True Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.DeleteDirectory(HQ.cacheFolder, DeleteDirectoryOption.DeleteAllContents)
        End If

        'Delete core plugin cache files too. TODO: extend plugin interface to support dropping cache.
        Dim fitterCache As String = Path.Combine(HQ.AppDataFolder, "HQF", "Cache")
        If (Directory.Exists(fitterCache)) Then
            Directory.Delete(fitterCache, True)
        End If
    End Sub

    Private Sub btnDeleteCoreCache_Click(sender As Object, e As EventArgs) Handles btnDeleteCoreCache.Click
        Try
            DeleteCacheFolders()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnRebuildCoreCache_Click(sender As Object, e As EventArgs) Handles btnRebuildCoreCache.Click
        Try
            DeleteCacheFolders()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Call DataFunctions.LoadItems()
        Call DataFunctions.LoadSolarSystems()
        Call DataFunctions.LoadStations()
        Call DataFunctions.CreateCoreCache()
    End Sub
End Class

