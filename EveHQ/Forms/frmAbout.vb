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
Imports System.Text

Namespace Forms
    Public Class FrmAbout

        Private Sub frmAbout_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Insert the version number to the splash screen
            lblVersion.Text = "Version " & My.Application.Info.Version.ToString
            lblCopyright.Text = My.Application.Info.Copyright
            lblDate.Text = My.Application.Info.Trademark
            ' Add the credits to the WBControl
            Dim credits As New StringBuilder
            credits.Append("<html><body>")
            credits.Append("<table style='font-family: Tahoma; font-size: 10px;'>")
            credits.Append("<tr><td colspan=2 style='font-family: Tahoma; font-size: 12px;'><b><u>EveHQ Credits</u></b></td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Lead Developer:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Quantix Blackstar</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Current Contributors:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Rob Crowley (Arkan), Quantix Blackstar, Vessper</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Former Contributors:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Darkwolf, Darmed Khan, Eowarian, farlin, geniusfreak, Mdram, Modescond, MoWe79, MrCue, Nauvus3x7, Saulvin, Taedrin, Thorien</td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td align='left' colspan=2>EveHQ Created By:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Vessper</td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td align='left' colspan=2>EVECacheParser Library By:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Desmont McCallock</td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td>Artwork</td><td align='right'><a href='http://foxgguy2001.deviantart.com' target='_blank'>Foxgguy2001</a></td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td colspan=2>Isk donations to Quantix Blackstar gratefully accepted! Alternatively, help fund EveHQ development by <a href='http://pledgie.com/campaigns/18228' target='_blank'>donating to cover costs</a>.</td></tr>")
            credits.Append("</table></body></html>")
            wbCredits.DocumentText = credits.ToString
        End Sub

        Private Sub lblEveHQLink_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblEveHQLink.LinkClicked
            Try
                Process.Start("http://www.evehq.net")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Sub
    End Class
End NameSpace