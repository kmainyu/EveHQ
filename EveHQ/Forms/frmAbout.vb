'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Text
Imports System.Reflection

Namespace Forms
    Public Class FrmAbout

        Private Sub frmAbout_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Insert the version number to the splash screen
            lblVersion.Text = "Version " & FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion
            lblCopyright.Text = My.Application.Info.Copyright
            lblDate.Text = My.Application.Info.Trademark
            ' Add the credits to the WBControl
            Dim credits As New StringBuilder
            credits.Append("<html><body>")
            credits.Append("<table style='font-family: Tahoma; font-size: 10px;'>")
            credits.Append("<tr><td colspan=2 style='font-family: Tahoma; font-size: 12px;'><b><u>EveHQ Credits</u></b></td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Lead Developer:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2><a href='https://gate.eveonline.com/Profile/Drailen' target='_blank'>Drailen</a></td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Current Contributors:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Drailen, Warlof Tutsimo</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Former Contributors:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Darkwolf, Darmed Khan, Eowarian, farlin, geniusfreak, Mdram, Modescond, MoWe79, MrCue, Nauvus3x7, Quantix Blackstar, Rob Crowley, Saulvin, Taedrin, Thorien</td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td align='left' colspan=2>EveHQ Created By:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2><a href='https://gate.eveonline.com/Profile/Drailen' target='_blank'>Drailen</a></td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td align='left' colspan=2>EVECacheParser Library By:</td></tr>")
            credits.Append("<tr><td align='left' colspan=2>Desmont McCallock</td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td>Artwork</td><td align='right'><a href='http://foxgguy2001.deviantart.com' target='_blank'>Foxgguy2001</a></td></tr>")
            credits.Append("<tr><td colspan=2><br /></td></tr>")
            credits.Append("<tr><td colspan=2>Isk donations to <a href='https://gate.eveonline.com/Profile/Drailen' target='_blank'>Drailen</a> gratefully accepted! Alternatively, help fund EveHQ development by <a href='https://pledgie.com/campaigns/26849' target='_blank'>donating to cover costs.</a></td></tr>")
            credits.Append("</table></body></html>")
            wbCredits.DocumentText = credits.ToString
        End Sub

        Private Sub lblEveHQLink_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblEveHQLink.LinkClicked
            Try
                Process.Start("http://newedentech.com")
            Catch ex As Exception
                MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Sub
    End Class
End NameSpace