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

Public Class frmShipAudit

    'Private Sub lvwAudit_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwAudit.ColumnClick
    '    If CInt(lvwAudit.Tag) = e.Column Then
    '        Me.lvwAudit.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, Windows.Forms.SortOrder.Ascending)
    '        lvwAudit.Tag = -1
    '    Else
    '        Me.lvwAudit.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, Windows.Forms.SortOrder.Descending)
    '        lvwAudit.Tag = e.Column
    '    End If
    '    ' Call the sort method to manually sort.
    '    lvwAudit.Sort()
    'End Sub

    Private Sub frmShipAudit_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ' Sorts the audit logs by attribute followed by old value (sort on old value first!)
        Me.lvwAudit.ListViewItemSorter = New EveHQ.Core.ListViewItemComparerText(2, Windows.Forms.SortOrder.Ascending)
        Me.lvwAudit.ListViewItemSorter = New EveHQ.Core.ListViewItemComparerText(0, Windows.Forms.SortOrder.Ascending)
        lvwAudit.Sort()
        lvwAudit.Items(0).Selected = True
        lvwAudit.Items(0).Focused = True
    End Sub
End Class