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

Namespace Requisitions
    ''' <summary>
    ''' Class for holding the details of owned assets for use with Requisitions
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RequisitionAsset

        Dim _cTotalQuantity As Long = 0
        Dim _cLocations As New SortedList(Of String, Long)

        ''' <summary>
        ''' Returns the total quantity of all the items in the locations
        ''' </summary>
        ''' <value></value>
        ''' <returns>The total quantity of items owned</returns>
        ''' <remarks></remarks>
        Public Property TotalQuantity() As Long
            Get
                Return _cTotalQuantity
            End Get
            Set(ByVal value As Long)
                _cTotalQuantity = value
            End Set
        End Property

        ''' <summary>
        ''' Holds the individual locations of each asset, together with the quantity
        ''' </summary>
        ''' <value></value>
        ''' <returns>A SortedList containing the locations of the assets</returns>
        ''' <remarks></remarks>
        Public Property Locations() As SortedList(Of String, Long)
            Get
                Return _cLocations
            End Get
            Set(ByVal value As SortedList(Of String, Long))
                _cLocations = value
            End Set
        End Property

        ''' <summary>
        ''' Standard initialiser for a RequisitionAsset
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _cTotalQuantity = 0
            _cLocations.Clear()
        End Sub

        ''' <summary>
        ''' Initialises a RequisitionAsset with new information
        ''' </summary>
        ''' <param name="locationID">The LocationID of the asset</param>
        ''' <param name="quantity">The number of items held at the LocationID</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal locationID As String, ByVal quantity As Long)
            _cTotalQuantity = 0
            _cLocations.Clear()
            _cLocations.Add(locationID, Quantity)
            _cTotalQuantity = Quantity
        End Sub

        ''' <summary>
        ''' Routine to add an item into the locations
        ''' </summary>
        ''' <param name="locationID">The LocationID of the asset</param>
        ''' <param name="quantity">The number of items held at the LocationID</param>
        ''' <remarks></remarks>
        Public Sub AddAsset(ByVal locationID As String, ByVal quantity As Long)
            If _cLocations.ContainsKey(locationID) = False Then
                _cLocations.Add(locationID, Quantity)
            Else
                _cLocations(locationID) += Quantity
            End If
            _cTotalQuantity += Quantity
        End Sub

        ''' <summary>
        ''' Routine to recalculate the total number of items held
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RecalculateTotal()
            Dim quantity As Long = 0
            For Each q As Long In _cLocations.Values
                quantity += q
            Next
            _cTotalQuantity = quantity
        End Sub


    End Class
End Namespace