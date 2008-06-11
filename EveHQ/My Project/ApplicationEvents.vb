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
            If e.IsNetworkAvailable = False Then
                frmEveHQ.EveStatusIcon.BalloonTipIcon = ToolTipIcon.Info
                frmEveHQ.EveStatusIcon.BalloonTipTitle = "Network Status Notification"
                frmEveHQ.EveStatusIcon.BalloonTipText = "EveHQ has detected that the connection to the network has been lost. This will affect the responses from the Eve Servers."
                frmEveHQ.EveStatusIcon.ShowBalloonTip(3000)
            End If
        End Sub

        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown

        End Sub

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            frmEveHQ.WindowState = FormWindowState.Minimized
        End Sub

        Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            'MessageBox.Show("You can only run one copy of EveHQ at a time!", "EveHQ Already Running", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            If frmEveHQ.WindowState = FormWindowState.Minimized Then
                frmEveHQ.WindowState = FormWindowState.Normal
                frmEveHQ.Show()
                frmEveHQ.BringToFront()
            End If
            e.BringToForeground = True
        End Sub
    End Class

End Namespace
