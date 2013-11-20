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
Imports System.Windows.Forms
Imports EveHQ.Core

Namespace Forms

    Public Class FrmShipAudit

      Private Sub frmShipAudit_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
            ' Sorts the audit logs by attribute followed by old value (sort on old value first!)
            lvwAudit.ListViewItemSorter = New ListViewItemComparerText(2, SortOrder.Ascending)
            lvwAudit.ListViewItemSorter = New ListViewItemComparerText(0, SortOrder.Ascending)
            lvwAudit.Sort()
            lvwAudit.Items(0).Selected = True
            lvwAudit.Items(0).Focused = True
        End Sub

    End Class

End NameSpace