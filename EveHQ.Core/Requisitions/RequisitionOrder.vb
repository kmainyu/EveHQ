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