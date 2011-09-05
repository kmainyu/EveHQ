﻿' ========================================================================
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
Imports System
Imports System.Collections
Imports System.Reflection

Public Class ClassSorter
    Implements IComparer
    Dim _sortClasses As ArrayList
    Public ReadOnly Property SortClasses() As ArrayList
        Get
            Return _sortClasses
        End Get
    End Property
    Public Sub New()
        _sortClasses = New ArrayList
    End Sub
    Public Sub New(ByVal SortClasses As ArrayList)
        _sortClasses = SortClasses
    End Sub
    Public Sub New(ByVal SortColumn As String, ByVal SortDirection As SortDirection)
        _sortClasses = New ArrayList
        _sortClasses.Add(New SortClass(SortColumn, SortDirection))
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        If (SortClasses.Count = 0) Then
            Return 0
        Else
            Return CheckSort(0, x, y)
        End If
    End Function

    Private Function CheckSort(ByVal SortLevel As Integer, ByVal MyObject1 As Object, ByVal MyObject2 As Object) As Integer
        Dim returnVal As Integer = 0
        If SortClasses.Count - 1 >= SortLevel Then
            Dim valueOf1 As Object = MyObject1.GetType().GetProperty(CType(SortClasses(SortLevel), SortClass).SortColumn).GetValue(MyObject1, Nothing)
            Dim valueOf2 As Object = MyObject2.GetType().GetProperty(CType(SortClasses(SortLevel), SortClass).SortColumn).GetValue(MyObject2, Nothing)
            If valueOf1 IsNot Nothing And valueOf2 IsNot Nothing Then
                If CType(SortClasses(SortLevel), SortClass).SortDirection = SortDirection.Ascending Then
                    returnVal = (CType(valueOf1, IComparable)).CompareTo(valueOf2)
                Else
                    returnVal = (CType(valueOf2, IComparable)).CompareTo(valueOf1)
                End If
            End If
            If returnVal = 0 Then
                returnVal = CheckSort(SortLevel + 1, MyObject1, MyObject2)
            End If
        End If
        Return returnVal
    End Function
End Class

Public Enum SortDirection
    Ascending = 1
    Descending = 2
End Enum

Public Class SortClass
    Dim _sortColumn As String
    Dim _sortDirection As SortDirection
    Public Property SortColumn() As String
        Get
            Return _sortColumn
        End Get
        Set(ByVal value As String)
            _sortColumn = value
        End Set
    End Property
    Public Property SortDirection() As SortDirection
        Get
            Return _sortDirection
        End Get
        Set(ByVal value As SortDirection)
            _sortDirection = value
        End Set
    End Property

    Public Sub New(ByVal Sortcolumn As String, ByVal SortDirection As SortDirection)
        Me.SortColumn = Sortcolumn
        Me.SortDirection = SortDirection
    End Sub
End Class
