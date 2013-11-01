' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Namespace Requisitions
    ''' <summary>
    ''' Storage class for a single item within a Requisition
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RequisitionOrder
        Implements ICloneable

        Dim _cID As String ' Key for Requisition, matches ID in DB
        Dim _cItemID As String
        Dim _cItemName As String
        Dim _cItemQuantity As Integer
        Dim _cSource As String ' Core feature or Plug-in
        Dim _cRequestDate As Date

        ''' <summary>
        ''' Returns the unique ID of the item in the requisition list
        ''' </summary>
        ''' <value></value>
        ''' <returns>The unique ID of the item</returns>
        ''' <remarks>This is the key for the Requisition.Orders collection and also matches the ID in the DB</remarks>
        Public Property ID() As String
            Get
                Return _cID
            End Get
            Set(ByVal value As String)
                _cID = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the CCP typeID of the item in the order
        ''' </summary>
        ''' <value></value>
        ''' <returns>The typeName of the item in the order</returns>
        ''' <remarks></remarks>
        Public Property ItemName() As String
            Get
                Return _cItemName
            End Get
            Set(ByVal value As String)
                _cItemName = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the CCP typeID of the item in the order
        ''' </summary>
        ''' <value></value>
        ''' <returns>The typeID of the item in the order</returns>
        ''' <remarks></remarks>
        Public Property ItemID() As String
            Get
                Return _cItemID
            End Get
            Set(ByVal value As String)
                _cItemID = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the quantity of the item in the order
        ''' </summary>
        ''' <value></value>
        ''' <returns>The order quantity</returns>
        ''' <remarks></remarks>
        Public Property ItemQuantity() As Integer
            Get
                Return _cItemQuantity
            End Get
            Set(ByVal value As Integer)
                _cItemQuantity = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the EveHQ core feature or plug-in used to place or modify this order
        ''' </summary>
        ''' <value></value>
        ''' <returns>The core feature or plug-in used to place or modify the order</returns>
        ''' <remarks></remarks>
        Public Property Source() As String
            Get
                Return _cSource
            End Get
            Set(ByVal value As String)
                _cSource = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the date on which the order was originally made
        ''' </summary>
        ''' <value></value>
        ''' <returns>The date the order was originally made</returns>
        ''' <remarks></remarks>
        Public Property RequestDate() As Date
            Get
                Return _cRequestDate
            End Get
            Set(ByVal value As Date)
                _cRequestDate = value
            End Set
        End Property

        ''' <summary>
        ''' Method for cloning a requisition 
        ''' </summary>
        ''' <returns>A copy of the instance of Requisition from where the function was called</returns>
        ''' <remarks></remarks>
        Public Function Clone() As Object Implements ICloneable.Clone
            Return CType(MemberwiseClone(), RequisitionOrder)
        End Function
    End Class
End Namespace