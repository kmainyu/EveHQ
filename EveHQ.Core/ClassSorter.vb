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


Public Class ClassSorter
    Implements IComparer
    ReadOnly _sortClasses As ArrayList
    Public ReadOnly Property SortClasses() As ArrayList
        Get
            Return _sortClasses
        End Get
    End Property
    Public Sub New()
        _sortClasses = New ArrayList
    End Sub
    Public Sub New(ByVal sortClasses As ArrayList)
        _sortClasses = sortClasses
    End Sub
    Public Sub New(ByVal sortColumn As String, ByVal sortDirection As SortDirection)
        _sortClasses = New ArrayList
        _sortClasses.Add(New SortClass(sortColumn, SortDirection))
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        If (SortClasses.Count = 0) Then
            Return 0
        Else
            Return CheckSort(0, x, y)
        End If
    End Function

    Private Function CheckSort(ByVal sortLevel As Integer, ByVal myObject1 As Object, ByVal myObject2 As Object) As Integer
        Dim returnVal As Integer = 0
        If SortClasses.Count - 1 >= sortLevel Then
            Dim valueOf1 As Object = myObject1.GetType().GetProperty(CType(SortClasses(sortLevel), SortClass).SortColumn).GetValue(myObject1, Nothing)
            Dim valueOf2 As Object = MyObject2.GetType().GetProperty(CType(SortClasses(sortLevel), SortClass).SortColumn).GetValue(MyObject2, Nothing)
            If valueOf1 IsNot Nothing And valueOf2 IsNot Nothing Then
                If CType(SortClasses(sortLevel), SortClass).SortDirection = SortDirection.Ascending Then
                    returnVal = (CType(valueOf1, IComparable)).CompareTo(valueOf2)
                Else
                    returnVal = (CType(valueOf2, IComparable)).CompareTo(valueOf1)
                End If
            End If
            If returnVal = 0 Then
                returnVal = CheckSort(sortLevel + 1, myObject1, MyObject2)
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

    Public Sub New(ByVal sortColumn As String, ByVal sortDirection As SortDirection)
        Me.SortColumn = sortColumn
        Me.SortDirection = sortDirection
    End Sub
End Class

