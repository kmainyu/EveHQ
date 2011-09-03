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

''' <summary>
''' Class for holding the details of owned assets for use with Requisitions
''' </summary>
''' <remarks></remarks>
Public Class RequisitionAsset

    Dim cTotalQuantity As Long = 0
    Dim cLocations As New SortedList(Of String, Long)

    ''' <summary>
    ''' Returns the total quantity of all the items in the locations
    ''' </summary>
    ''' <value></value>
    ''' <returns>The total quantity of items owned</returns>
    ''' <remarks></remarks>
    Public Property TotalQuantity() As Long
        Get
            Return cTotalQuantity
        End Get
        Set(ByVal value As Long)
            cTotalQuantity = value
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
            Return cLocations
        End Get
        Set(ByVal value As SortedList(Of String, Long))
            cLocations = value
        End Set
    End Property

    ''' <summary>
    ''' Standard initialiser for a RequisitionAsset
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        cTotalQuantity = 0
        cLocations.Clear()
    End Sub

    ''' <summary>
    ''' Initialises a RequisitionAsset with new information
    ''' </summary>
    ''' <param name="LocationID">The LocationID of the asset</param>
    ''' <param name="Quantity">The number of items held at the LocationID</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal LocationID As String, ByVal Quantity As Long)
        cTotalQuantity = 0
        cLocations.Clear()
        cLocations.Add(LocationID, Quantity)
        cTotalQuantity = Quantity
    End Sub

    ''' <summary>
    ''' Routine to add an item into the locations
    ''' </summary>
    ''' <param name="LocationID">The LocationID of the asset</param>
    ''' <param name="Quantity">The number of items held at the LocationID</param>
    ''' <remarks></remarks>
    Public Sub AddAsset(ByVal LocationID As String, ByVal Quantity As Long)
        If cLocations.ContainsKey(LocationID) = False Then
            cLocations.Add(LocationID, Quantity)
        Else
            cLocations(LocationID) += Quantity
        End If
        cTotalQuantity += Quantity
    End Sub

    ''' <summary>
    ''' Routine to recalculate the total number of items held
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RecalculateTotal()
        Dim quantity As Long = 0
        For Each q As Long In cLocations.Values
            quantity += q
        Next
        cTotalQuantity = quantity
    End Sub

   
End Class
