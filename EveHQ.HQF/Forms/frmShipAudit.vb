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
        Me.lvwAudit.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(2, Windows.Forms.SortOrder.Ascending)
        Me.lvwAudit.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(0, Windows.Forms.SortOrder.Ascending)
        lvwAudit.Sort()
        lvwAudit.Items(0).Selected = True
        lvwAudit.Items(0).Focused = True
    End Sub
End Class