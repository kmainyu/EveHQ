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
Public Class RSSFeedItem
    Private Sub lblFeedItemTitle_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblFeedItemTitle.LinkClicked
        Try
            If lblFeedItemTitle.Tag IsNot Nothing Then
                Dim URL As String = lblFeedItemTitle.Tag.ToString
                If URL <> "" Then
                    Process.Start(URL)
                End If
            End If
        Catch ex As Exception
            ' Suppress any error message and do nothing
        End Try
    End Sub
End Class
