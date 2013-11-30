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
Imports DevComponents.AdvTree
Imports System.Windows.Forms
Imports System.Globalization

Public Class AdvTreeSorter

    Shared sortByTag As Boolean = False

    Public Shared Function Sort(ByVal column As DevComponents.AdvTree.ColumnHeader, ByVal sortChildNodes As Boolean, ByVal retainLastSortOrder As Boolean) As AdvTreeSortResult
        Dim hostedTree As AdvTree = column.AdvTree
        Dim columnDisplayIndex As Integer = column.DisplayIndex
        Return Sort(hostedTree, columnDisplayIndex, sortChildNodes, retainLastSortOrder)
    End Function

    Public Shared Function Sort(ByVal hostedTree As AdvTree, ByVal sortResult As AdvTreeSortResult, ByVal sortChildNodes As Boolean) As AdvTreeSortResult
        Return Sort(hostedTree, sortResult.SortedIndex, sortResult.SortedOrder, sortChildNodes, False)
    End Function

    Public Shared Function Sort(ByVal hostedTree As AdvTree, ByVal columnDisplayIndex As Integer, ByVal sortChildNodes As Boolean, ByVal retainLastSortOrder As Boolean) As AdvTreeSortResult
        Return Sort(hostedTree, columnDisplayIndex, AdvTreeSortOrder.Default, sortChildNodes, retainLastSortOrder)
    End Function

    Private Shared Function Sort(ByVal hostedTree As AdvTree, ByVal columnDisplayIndex As Integer, ByVal sortOrder As AdvTreeSortOrder, ByVal sortChildNodes As Boolean, ByVal retainLastSortOrder As Boolean) As AdvTreeSortResult

        ' Determine sorting logic based on method parameters
        Dim lastSortResult As AdvTreeSortResult = CType(hostedTree.Tag, AdvTreeSortResult)
        Dim indexToSort As Integer = columnDisplayIndex
        Dim orderToSort As AdvTreeSortOrder = sortOrder
        If sortOrder = AdvTreeSortOrder.Default Then
            orderToSort = AdvTreeSortOrder.Ascending
        End If

        If lastSortResult IsNot Nothing Then
            If retainLastSortOrder = True Then
                indexToSort = lastSortResult.SortedIndex
                orderToSort = lastSortResult.SortedOrder
            Else
                If sortOrder = AdvTreeSortOrder.Default Then
                    If indexToSort = lastSortResult.SortedIndex Then
                        orderToSort = CType(-lastSortResult.SortedOrder, AdvTreeSortOrder)
                    Else
                        orderToSort = lastSortResult.SortedOrder
                    End If
                Else
                    orderToSort = sortOrder
                End If
            End If
        End If

        ' Set the "true" column index
        Dim colIdx As Integer = indexToSort - 1

        ' Begin UI update
        hostedTree.Cursor = Cursors.WaitCursor
        hostedTree.BeginUpdate()

        ' Check if we are to look at the tag instead of text
        If hostedTree.Columns(colIdx).EditorType = eCellEditorType.Custom Then
            sortByTag = True
        Else
            sortByTag = False
        End If

        ' Reset the old image
        If lastSortResult IsNot Nothing Then
            hostedTree.Columns(lastSortResult.SortedIndex - 1).SortDirection = eSortDirection.None
        End If

        ' Set new image
        hostedTree.Columns(colIdx).ImageAlignment = eColumnImageAlignment.Right
        If orderToSort = AdvTreeSortOrder.Ascending Then
            hostedTree.Columns(colIdx).SortDirection = eSortDirection.Ascending
        Else
            hostedTree.Columns(colIdx).SortDirection = eSortDirection.Descending
        End If

        hostedTree.Nodes.Sort(New AdvTreeSortComparer(colIdx, orderToSort, sortByTag))
        If sortChildNodes = True Then
            Call PerformIterativeSort(hostedTree.Nodes, colIdx, orderToSort)
        End If

        ' End the update
        hostedTree.EndUpdate()
        hostedTree.Cursor = Cursors.Default

        ' Return and store the result
        Dim newSortResult As New AdvTreeSortResult(indexToSort, orderToSort)
        hostedTree.Tag = newSortResult
        Return newSortResult

    End Function


    Private Shared Sub PerformIterativeSort(ByVal sortNodeCollection As NodeCollection, ByVal colIdx As Integer, ByVal order As AdvTreeSortOrder)
        For Each sortNode As Node In sortNodeCollection
            If sortNode.Nodes.Count > 0 Then
                ' Do a sort on these
                sortNode.Nodes.Sort(New AdvTreeSortComparer(colIdx, order, sortByTag))
                ' Check for additional nodes to sort
                PerformIterativeSort(sortNode.Nodes, colIdx, order)
            End If
        Next
    End Sub

End Class

Public Class AdvTreeSortComparer
    Implements IComparer
    Private ReadOnly _col As Integer
    Private ReadOnly _order As AdvTreeSortOrder
    Private ReadOnly _sortTag As Boolean = False
    Dim _doubleX, _doubleY As Double
    Dim _dateX, _dateY As Date

    Public Sub New()
        _col = 0
        _order = AdvTreeSortOrder.Ascending
        _sortTag = False
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As AdvTreeSortOrder, ByVal sortTag As Boolean)
        _col = column
        _order = order
        _sortTag = SortTag
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer
        Dim tempDbl As Double = 0
        Const NumberStyle As NumberStyles = NumberStyles.Number Or NumberStyles.AllowParentheses

        If _sortTag = False Then
            If Double.TryParse(CType(x, Node).Cells(_col).Text, NumberStyle, Nothing, tempDbl) And Double.TryParse(CType(y, Node).Cells(_col).Text, NumberStyle, Nothing, tempDbl) Then
                'Parse the two objects passed as a parameter as a Double
                _doubleX = Double.Parse((CType(x, Node).Cells(_col).Text), NumberStyle)
                _doubleY = Double.Parse((CType(y, Node).Cells(_col).Text), NumberStyle)
                'Compare the two numbers
                returnVal = _doubleX.CompareTo(_doubleY)
            ElseIf IsDate(CType(x, Node).Cells(_col).Text) And IsDate(CType(y, Node).Cells(_col).Text) Then
                ' Parse the two objects passed as a parameter as a Date
                _dateX = CDate((CType(x, Node).Cells(_col).Text))
                _dateY = CDate((CType(y, Node).Cells(_col).Text))
                ' Compare the two numbers
                returnVal = _dateX.CompareTo(_dateY)
            Else
                returnVal = System.String.Compare(CType(x, Node).Cells(_col).Text, CType(y, Node).Cells(_col).Text, StringComparison.Ordinal)
            End If
        Else
            If Double.TryParse(CType(x, Node).Cells(_col).Tag.ToString, NumberStyle, Nothing, tempDbl) And Double.TryParse(CType(y, Node).Cells(_col).Tag.ToString, NumberStyle, Nothing, tempDbl) Then
                'Parse the two objects passed as a parameter as a Double
                _doubleX = Double.Parse((CType(x, Node).Cells(_col).Tag.ToString), NumberStyle)
                _doubleY = Double.Parse((CType(y, Node).Cells(_col).Tag.ToString), NumberStyle)
                'Compare the two numbers
                returnVal = _doubleX.CompareTo(_doubleY)
            ElseIf IsDate(CType(x, Node).Cells(_col).Tag) And IsDate(CType(y, Node).Cells(_col).Tag) Then
                ' Parse the two objects passed as a parameter as a Date
                _dateX = CDate((CType(x, Node).Cells(_col).Tag))
                _dateY = CDate((CType(y, Node).Cells(_col).Tag))
                ' Compare the two numbers
                returnVal = _dateX.CompareTo(_dateY)
            Else
                returnVal = System.String.Compare(CType(x, Node).Cells(_col).Tag.ToString, CType(y, Node).Cells(_col).Tag.ToString, StringComparison.Ordinal)
            End If
        End If

        ' Determine whether the sort order is descending.
        If _order = AdvTreeSortOrder.Descending Then
            ' Invert the value returned by String.Compare.
            returnVal *= -1
        End If
        Return returnVal
    End Function

End Class

<Serializable()> Public Class AdvTreeSortResult
    Public SortedIndex As Integer
    Public SortedOrder As AdvTreeSortOrder

    Public Sub New()
        SortedIndex = 1
        SortedOrder = AdvTreeSortOrder.Default
    End Sub

    Public Sub New(ByVal columnIndex As Integer, ByVal sortOrder As AdvTreeSortOrder)
        SortedIndex = columnIndex
        SortedOrder = SortOrder
    End Sub
End Class

Public Enum AdvTreeSortOrder
    Ascending = -1
    [Default] = 0
    Descending = 1
End Enum
