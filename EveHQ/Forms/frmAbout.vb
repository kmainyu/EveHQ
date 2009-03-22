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
Public Class frmAbout

    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the image for the splash screen
        Dim r As New Random
        Dim img As Integer = r.Next(1, 6)
        Panel1.BackgroundImage = CType(My.Resources.ResourceManager.GetObject("Splashv" & img.ToString), Image)
        ' Insert the version number to the splash screen
        lblVersion.Text = "Version " & My.Application.Info.Version.ToString
        lblCopyright.Text = My.Application.Info.Copyright
        lblDate.Text = My.Application.Info.Trademark
        ' Add the credits to the WBControl
        Dim credits As New System.Text.StringBuilder
        credits.Append("<html><body>")
        credits.Append("<table style='font-family: Arial; font-size: 10px;'>")
        credits.Append("<tr><td colspan=2 style='font-family: Arial; font-size: 12px;'><b>EveHQ Credits</b></td><tr>")
        credits.Append("<tr><td>Lead Developer</td><td align='right'>Vessper</td></tr>")
        credits.Append("<tr><td>Other Development</td><td align='right'>Darkwolf</td></tr>")
        credits.Append("<tr><td></td><td align='right'>Darmed Khan</td></tr>")
        credits.Append("<tr><td></td><td align='right'>Mdram</td></tr>")
        credits.Append("<tr><td></td><td align='right'>MoWe79</td></tr>")
        credits.Append("<tr><td></td><td align='right'>Nauvus3x7</td></tr>")
        credits.Append("<tr><td>Images</td><td align='right'><a href='http://jadeo.hexium.net' target='_blank'>JadeO</a></td></tr>")
        credits.Append("</table></body></html>")
        wbCredits.DocumentText = credits.ToString
    End Sub

    Private Sub lblEveHQLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblEveHQLink.LinkClicked
        Try
            Process.Start("http://www.evehq.net")
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class