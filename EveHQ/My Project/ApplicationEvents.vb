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
Namespace My

    'The following events are available for MyApplication
    '
    'Startup: Raised when the application starts, before the startup form is created.
    'Shutdown: Raised after all application forms are closed.  This event is not raised if the application is terminating abnormally.
    'UnhandledException: Raised if the application encounters an unhandled exception.
    'StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    'NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Class MyApplication

        Private Sub MyApplication_NetworkAvailabilityChanged(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged
            Try
                If e.IsNetworkAvailable = False Then
                    frmEveHQ.EveStatusIcon.BalloonTipIcon = ToolTipIcon.Info
                    frmEveHQ.EveStatusIcon.BalloonTipTitle = "Network Status Notification"
                    frmEveHQ.EveStatusIcon.BalloonTipText = "EveHQ has detected that the connection to the network has been lost. This will affect the responses from the Eve Servers."
                    frmEveHQ.EveStatusIcon.ShowBalloonTip(3000)
                End If
            Catch ex As Exception
                ' Some form of error here so move on and see if things still work ok?
            End Try
        End Sub

        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown

        End Sub

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            frmEveHQ.WindowState = FormWindowState.Minimized
        End Sub

        Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            'MessageBox.Show("You can only run one copy of EveHQ at a time!", "EveHQ Already Running", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ' Can we get /params?
            For Each param As String In e.CommandLine
                ' Check for the fitting protocol
                If param.StartsWith(EveHQ.Core.HQ.FittingProtocol) Then
                    ' Now see if HQF is available
                    Dim PluginName As String = "EveHQ Fitter"
                    Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
                    myPlugIn.PostStartupData = param
                    If myPlugIn.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
                        Dim mainTab As DevComponents.DotNetBar.TabStrip = CType(EveHQ.Core.HQ.MainForm.Controls("tabEveHQMDI"), DevComponents.DotNetBar.TabStrip)
                        Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(PluginName)
                        If tp IsNot Nothing Then
                            mainTab.SelectedTab = tp
                        Else
                            Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
                            plugInForm.MdiParent = EveHQ.Core.HQ.MainForm
                            plugInForm.Show()
                        End If
                        myPlugIn.Instance.GetPlugInData(myPlugIn.PostStartupData, 0)
                    Else
                        ' Try to load an open the plug-in here
                        Threading.ThreadPool.QueueUserWorkItem(AddressOf frmEveHQ.LoadAndOpenPlugIn, myPlugIn)
                    End If
                End If
            Next
            If frmEveHQ.WindowState = FormWindowState.Minimized Then
                frmEveHQ.WindowState = FormWindowState.Maximized
                frmEveHQ.Show()
                frmEveHQ.BringToFront()
            End If
            e.BringToForeground = True
        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Try
                Dim myException As New frmException
                myException.lblVersion.Text = "Version: " & My.Application.Info.Version.ToString
                myException.lblError.Text = e.Exception.Message
                Dim trace As New System.Text.StringBuilder
                trace.AppendLine(e.Exception.StackTrace.ToString)
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
                    e.ExitApplication = False
                Else
                    Call frmEveHQ.ShutdownRoutine()
                    e.ExitApplication = True
                End If
            Catch ex As Exception
                Dim msg As New System.Text.StringBuilder
                msg.AppendLine("A critical error has occurred which has prevented the UI from displaying! The following message should have been copied to the clipboard but you may need to provide this message in a screenshot for any bug report.")
                msg.AppendLine("")
                msg.AppendLine("Original Error: " & e.Exception.Message)
                msg.AppendLine("")
                msg.AppendLine("Original Stacktrace: " & e.Exception.StackTrace)
                msg.AppendLine("")
                msg.AppendLine("Error Form Error: " & ex.Message)
                msg.AppendLine("")
                msg.AppendLine("Error Form Stacktrace: " & ex.StackTrace)
                msg.AppendLine("")
                If ex.InnerException IsNot Nothing Then
                    msg.AppendLine("Error Form Inner Exception: " & ex.InnerException.Message)
                End If
                Try
                    Clipboard.SetText(msg.ToString)
                Catch ex2 As Exception
                    ' do nothing
                End Try
                MessageBox.Show(msg.ToString, "Farnsworth is Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Sub
    End Class

End Namespace
