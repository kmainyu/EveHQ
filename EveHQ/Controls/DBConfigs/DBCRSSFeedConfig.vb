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
Imports EveHQ.Controls.DBControls

Namespace Controls.DBConfigs
    Public Class DBCRSSFeedConfig

#Region "Properties"

        Dim _dbWidget As New DBCRSSFeed
        Public Property DBWidget() As DBCRSSFeed
            Get
                Return _dbWidget
            End Get
            Set(ByVal value As DBCRSSFeed)
                _dbWidget = value
                Call SetControlInfo()
            End Set
        End Property

#End Region

        Private Sub SetControlInfo()
            txtURL.Text = _dbWidget.RSSFeed
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Just close the form and do nothing
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Update the control properties
            If txtURL.Text = "" Then
                MessageBox.Show("You must enter a URL before adding this widget.", "URL Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            _dbWidget.RSSFeed = txtURL.Text
            ' Now close the form
            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End NameSpace