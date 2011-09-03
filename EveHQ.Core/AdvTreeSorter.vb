' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports DevComponents.AdvTree

Public Class AdvTreeSorter

    Shared SortByTag As Boolean = False

    Public Shared Sub Sort(ByVal HostedTree As AdvTree, ByVal ColumnDisplayIndex As Integer, ByVal SortChildNodes As Boolean, ByVal RetainLastSortOrder As Boolean)
        Dim ColIdx As Integer = ColumnDisplayIndex - 1
        Dim SortOrder As Integer = CInt(HostedTree.Tag)
        If RetainLastSortOrder = True And SortOrder <> 0 Then
            ColumnDisplayIndex = Math.Abs(SortOrder)
            ColIdx = ColumnDisplayIndex - 1
            SortOrder = -SortOrder
        End If
        Dim SortedOrder As SortOrder
        HostedTree.Cursor = Cursors.WaitCursor
        HostedTree.BeginUpdate()
        HostedTree.Columns(ColIdx).ImageAlignment = eColumnImageAlignment.Right
        If HostedTree.Columns(ColIdx).EditorType = eCellEditorType.Custom Then
            SortByTag = True
        Else
            SortByTag = False
        End If
        If ColumnDisplayIndex = Math.Abs(SortOrder) Then
            ' Invert the sort
            If SortOrder < 0 Then
                SortedOrder = System.Windows.Forms.SortOrder.Ascending
                HostedTree.Tag = ColumnDisplayIndex
                HostedTree.Columns(ColIdx).Image = My.Resources.ArrowUp
            Else
                SortedOrder = System.Windows.Forms.SortOrder.Descending
                HostedTree.Tag = -ColumnDisplayIndex
                HostedTree.Columns(ColIdx).Image = My.Resources.ArrowDown
            End If
        Else
            If SortOrder <> 0 Then
                HostedTree.Columns(Math.Abs(SortOrder) - 1).Image = Nothing
            End If
            If SortOrder < 0 Then
                SortedOrder = System.Windows.Forms.SortOrder.Descending
                HostedTree.Tag = -ColumnDisplayIndex
                HostedTree.Columns(ColIdx).Image = My.Resources.ArrowDown
            Else
                SortedOrder = System.Windows.Forms.SortOrder.Ascending
                HostedTree.Tag = ColumnDisplayIndex
                HostedTree.Columns(ColIdx).Image = My.Resources.ArrowUp
            End If
        End If
        HostedTree.Nodes.Sort(New AdvTreeSortComparer(ColIdx, SortedOrder, SortByTag))
        If SortChildNodes = True Then
            Call PerformIterativeSort(HostedTree.Nodes, ColIdx, SortedOrder)
        End If
        HostedTree.EndUpdate()
        HostedTree.Cursor = Cursors.Default
    End Sub

    Public Shared Sub Sort(ByVal Column As DevComponents.AdvTree.ColumnHeader, ByVal SortChildNodes As Boolean, ByVal RetainLastSortOrder As Boolean)
        Dim HostedTree As AdvTree = Column.AdvTree
        Dim ColumnDisplayIndex As Integer = Column.DisplayIndex
        If Column.EditorType = eCellEditorType.Custom Then
            SortByTag = True
        Else
            SortByTag = False
        End If
        Call Sort(HostedTree, ColumnDisplayIndex, SortChildNodes, RetainLastSortOrder)
    End Sub

    Private Shared Sub PerformIterativeSort(ByVal SortNodeCollection As NodeCollection, ByVal ColIdx As Integer, ByVal Order As SortOrder)
        For Each SortNode As Node In SortNodeCollection
            If SortNode.Nodes.Count > 0 Then
                ' Do a sort on these
                SortNode.Nodes.Sort(New AdvTreeSortComparer(ColIdx, Order, SortByTag))
                ' Check for additional nodes to sort
                PerformIterativeSort(SortNode.Nodes, ColIdx, Order)
            End If
        Next
    End Sub

End Class

Public Class AdvTreeSortComparer
    Implements IComparer
    Private col As Integer
    Private order As SortOrder
    Private sorttag As Boolean = False
    Dim DoubleX, DoubleY As Double
    Dim DateX, DateY As Date

    Public Sub New()
        col = 0
        order = SortOrder.Ascending
        sorttag = False
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As SortOrder, ByVal SortTag As Boolean)
        Me.col = column
        Me.order = order
        Me.sorttag = SortTag
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1

        If sorttag = False Then
            If IsNumeric(CType(x, Node).Cells(col).Text) And IsNumeric(CType(y, Node).Cells(col).Text) Then
                'Parse the two objects passed as a parameter as a Double
                DoubleX = CDbl((CType(x, Node).Cells(col).Text))
                DoubleY = CDbl((CType(y, Node).Cells(col).Text))
                'Compare the two numbers
                returnVal = DoubleX.CompareTo(DoubleY)
            ElseIf IsDate(CType(x, Node).Cells(col).Text) And IsDate(CType(y, Node).Cells(col).Text) Then
                ' Parse the two objects passed as a parameter as a Date
                DateX = CDate((CType(x, Node).Cells(col).Text))
                DateY = CDate((CType(y, Node).Cells(col).Text))
                ' Compare the two numbers
                returnVal = DateX.CompareTo(DateY)
            Else
                returnVal = CType(x, Node).Cells(col).Text.CompareTo(CType(y, Node).Cells(col).Text)
            End If
        Else
            If IsNumeric(CType(x, Node).Cells(col).Tag) And IsNumeric(CType(y, Node).Cells(col).Tag) Then
                'Parse the two objects passed as a parameter as a Double
                DoubleX = CDbl((CType(x, Node).Cells(col).Tag))
                DoubleY = CDbl((CType(y, Node).Cells(col).Tag))
                'Compare the two numbers
                returnVal = DoubleX.CompareTo(DoubleY)
            ElseIf IsDate(CType(x, Node).Cells(col).Tag) And IsDate(CType(y, Node).Cells(col).Tag) Then
                ' Parse the two objects passed as a parameter as a Date
                DateX = CDate((CType(x, Node).Cells(col).Tag))
                DateY = CDate((CType(y, Node).Cells(col).Tag))
                ' Compare the two numbers
                returnVal = DateX.CompareTo(DateY)
            Else
                returnVal = CType(x, Node).Cells(col).Tag.ToString.CompareTo(CType(y, Node).Cells(col).Tag.ToString)
            End If
        End If

        ' Determine whether the sort order is descending.
        If order = SortOrder.Descending Then
            ' Invert the value returned by String.Compare.
            returnVal *= -1
        End If
        Return returnVal
    End Function

End Class
