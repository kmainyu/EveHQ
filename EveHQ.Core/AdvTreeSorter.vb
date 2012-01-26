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
Imports DevComponents.AdvTree
Imports System.Globalization

Public Class AdvTreeSorter

    Shared SortByTag As Boolean = False

    Public Shared Function Sort(ByVal Column As DevComponents.AdvTree.ColumnHeader, ByVal SortChildNodes As Boolean, ByVal RetainLastSortOrder As Boolean) As AdvTreeSortResult
        Dim HostedTree As AdvTree = Column.AdvTree
        Dim ColumnDisplayIndex As Integer = Column.DisplayIndex
        Return Sort(HostedTree, ColumnDisplayIndex, SortChildNodes, RetainLastSortOrder)
    End Function

    Public Shared Function Sort(ByVal HostedTree As AdvTree, ByVal SortResult As AdvTreeSortResult, ByVal SortChildNodes As Boolean) As AdvTreeSortResult
        Return Sort(HostedTree, SortResult.SortedIndex, SortResult.SortedOrder, SortChildNodes, False)
    End Function

    Public Shared Function Sort(ByVal HostedTree As AdvTree, ByVal ColumnDisplayIndex As Integer, ByVal SortChildNodes As Boolean, ByVal RetainLastSortOrder As Boolean) As AdvTreeSortResult
        Return Sort(HostedTree, ColumnDisplayIndex, AdvTreeSortOrder.Default, SortChildNodes, RetainLastSortOrder)
    End Function

    Private Shared Function Sort(ByVal HostedTree As AdvTree, ByVal ColumnDisplayIndex As Integer, ByVal SortOrder As AdvTreeSortOrder, ByVal SortChildNodes As Boolean, ByVal RetainLastSortOrder As Boolean) As AdvTreeSortResult

        ' Determine sorting logic based on method parameters
        Dim LastSortResult As AdvTreeSortResult = CType(HostedTree.Tag, AdvTreeSortResult)
        Dim IndexToSort As Integer = ColumnDisplayIndex
        Dim OrderToSort As AdvTreeSortOrder = SortOrder
        If SortOrder = AdvTreeSortOrder.Default Then
            OrderToSort = AdvTreeSortOrder.Ascending
        End If

        If LastSortResult IsNot Nothing Then
            If RetainLastSortOrder = True Then
                IndexToSort = LastSortResult.SortedIndex
                OrderToSort = LastSortResult.SortedOrder
            Else
                If SortOrder = AdvTreeSortOrder.Default Then
                    If IndexToSort = LastSortResult.SortedIndex Then
                        OrderToSort = CType(-LastSortResult.SortedOrder, AdvTreeSortOrder)
                    Else
                        OrderToSort = CType(LastSortResult.SortedOrder, AdvTreeSortOrder)
                    End If
                Else
                    OrderToSort = SortOrder
                End If
            End If
        End If

        ' Set the "true" column index
        Dim ColIdx As Integer = IndexToSort - 1

        ' Begin UI update
        HostedTree.Cursor = Cursors.WaitCursor
        HostedTree.BeginUpdate()

        ' Check if we are to look at the tag instead of text
        If HostedTree.Columns(ColIdx).EditorType = eCellEditorType.Custom Then
            SortByTag = True
        Else
            SortByTag = False
        End If

        ' Reset the old image
        If LastSortResult IsNot Nothing Then
            HostedTree.Columns(LastSortResult.SortedIndex - 1).Image = Nothing
        End If

        ' Set new image
        HostedTree.Columns(ColIdx).ImageAlignment = eColumnImageAlignment.Right
        If OrderToSort = AdvTreeSortOrder.Ascending Then
            HostedTree.Columns(ColIdx).Image = My.Resources.ArrowUp
        Else
            HostedTree.Columns(ColIdx).Image = My.Resources.ArrowDown
        End If

        HostedTree.Nodes.Sort(New AdvTreeSortComparer(ColIdx, OrderToSort, SortByTag))
        If SortChildNodes = True Then
            Call PerformIterativeSort(HostedTree.Nodes, ColIdx, OrderToSort)
        End If

        ' End the update
        HostedTree.EndUpdate()
        HostedTree.Cursor = Cursors.Default

        ' Return and store the result
        Dim NewSortResult As New AdvTreeSortResult(IndexToSort, OrderToSort)
        HostedTree.Tag = NewSortResult
        Return NewSortResult

    End Function


    Private Shared Sub PerformIterativeSort(ByVal SortNodeCollection As NodeCollection, ByVal ColIdx As Integer, ByVal Order As AdvTreeSortOrder)
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
    Private order As AdvTreeSortOrder
    Private sorttag As Boolean = False
    Dim DoubleX, DoubleY As Double
    Dim DateX, DateY As Date

    Public Sub New()
        col = 0
        order = AdvTreeSortOrder.Ascending
        sorttag = False
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As AdvTreeSortOrder, ByVal SortTag As Boolean)
        Me.col = column
        Me.order = order
        Me.sorttag = SortTag
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1
        Dim TempDbl As Double = 0
        Dim TempDate As Date = Now
        Dim NumberStyle As NumberStyles = NumberStyles.Number Or NumberStyles.AllowParentheses

        If sorttag = False Then
            If Double.TryParse(CType(x, Node).Cells(col).Text, NumberStyle, Nothing, TempDbl) And Double.TryParse(CType(y, Node).Cells(col).Text, NumberStyle, Nothing, TempDbl) Then
                'Parse the two objects passed as a parameter as a Double
                DoubleX = Double.Parse((CType(x, Node).Cells(col).Text), NumberStyle)
                DoubleY = Double.Parse((CType(y, Node).Cells(col).Text), NumberStyle)
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
            If Double.TryParse(CType(x, Node).Cells(col).Tag.ToString, NumberStyle, Nothing, TempDbl) And Double.TryParse(CType(y, Node).Cells(col).Tag.ToString, NumberStyle, Nothing, TempDbl) Then
                'Parse the two objects passed as a parameter as a Double
                DoubleX = Double.Parse((CType(x, Node).Cells(col).Tag.ToString), NumberStyle)
                DoubleY = Double.Parse((CType(y, Node).Cells(col).Tag.ToString), NumberStyle)
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
        If order = AdvTreeSortOrder.Descending Then
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

    Public Sub New(ByVal ColumnIndex As Integer, ByVal SortOrder As AdvTreeSortOrder)
        SortedIndex = ColumnIndex
        SortedOrder = SortOrder
    End Sub
End Class

Public Enum AdvTreeSortOrder
    Ascending = -1
    [Default] = 0
    Descending = 1
End Enum
