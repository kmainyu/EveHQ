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


''' <summary>
''' Class for storing details of a specific custom ship
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CustomShip

    ''' <summary>
    ''' Gets or sets the ID of the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the ID of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property ID() As Integer
      
    ''' <summary>
    ''' Gets or sets the name of the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the name of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property Name() As String
       
    ''' <summary>
    ''' Gets or sets the base description of the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the base description of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property Description() As String
       
    ''' <summary>
    ''' Gets or sets whether the ship bonuses are automatically included as part of the description
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value stating whether the ship bonuses are automatically inlcuded as part of the description</returns>
    ''' <remarks></remarks>
    Public Property AutoBonusDescription() As Boolean
      
    ''' <summary>
    ''' Gets or sets the name of the ship used as the basis for the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string value containing the name to be used for the basis of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property BaseShipName() As String
      
    ''' <summary>
    ''' Gets or sets the Ship Class used for the ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the class of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property ShipClass() As String
     
    ''' <summary>
    ''' Gets or sets the bonuses for the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A List(of ShipEffect) containing the bonuses for the custom ship</returns>
    ''' <remarks></remarks>
    Public Property Bonuses() As List(Of ShipEffect)
        
    ''' <summary>
    ''' Gets or sets the actual data for the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of the Ship class that contains the data for the custom ship</returns>
    ''' <remarks></remarks>
    Public Property ShipData() As Ship
        
End Class
