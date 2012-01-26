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

''' <summary>
''' Class for storing details of a specific custom ship
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CustomShip

    Dim cID As String
    Dim cName As String
    Dim cDescription As String
    Dim cAutoBonusDescription As Boolean
    Dim cBaseShipName As String
    Dim cShipClass As String
    Dim cBonuses As New List(Of ShipEffect)
    Dim cShipData As New Ship

    ''' <summary>
    ''' Gets or sets the ID of the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the ID of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property ID() As String
        Get
            Return cID
        End Get
        Set(ByVal value As String)
            cID = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the name of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property Name() As String
        Get
            Return cName
        End Get
        Set(ByVal value As String)
            cName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the base description of the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the base description of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property Description() As String
        Get
            Return cDescription
        End Get
        Set(ByVal value As String)
            cDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the ship bonuses are automatically included as part of the description
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value stating whether the ship bonuses are automatically inlcuded as part of the description</returns>
    ''' <remarks></remarks>
    Public Property AutoBonusDescription() As Boolean
        Get
            Return cAutoBonusDescription
        End Get
        Set(ByVal value As Boolean)
            cAutoBonusDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the ship used as the basis for the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string value containing the name to be used for the basis of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property BaseShipName() As String
        Get
            Return cBaseShipName
        End Get
        Set(ByVal value As String)
            cBaseShipName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Ship Class used for the ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the class of the custom ship</returns>
    ''' <remarks></remarks>
    Public Property ShipClass() As String
        Get
            Return cShipClass
        End Get
        Set(ByVal value As String)
            cShipClass = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the bonuses for the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>A List(of ShipEffect) containing the bonuses for the custom ship</returns>
    ''' <remarks></remarks>
    Public Property Bonuses() As List(Of ShipEffect)
        Get
            Return cBonuses
        End Get
        Set(ByVal value As List(Of ShipEffect))
            cBonuses = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the actual data for the custom ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of the Ship class that contains the data for the custom ship</returns>
    ''' <remarks></remarks>
    Public Property ShipData() As Ship
        Get
            Return cShipData
        End Get
        Set(ByVal value As Ship)
            cShipData = value
        End Set
    End Property

End Class
