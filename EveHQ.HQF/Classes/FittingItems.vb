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
''' Class to store an itemID together with a state
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ModuleWithState
    Dim cID As String
    Dim cChargeID As String
    Dim cState As ModuleStates

    ''' <summary>
    ''' Gets or sets the ID of the module
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the typeID of the module</returns>
    ''' <remarks></remarks>
    Public Property ID() As String
        Get
            Return cID
        End Get
        Set(ByVal value As String)
            cID = value
        End Set
    End Property

    Public Property ChargeID() As String
        Get
            Return cChargeID
        End Get
        Set(ByVal value As String)
            cChargeID = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the state of the module
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value containing the state of the module</returns>
    ''' <remarks></remarks>
    Public Property State() As ModuleStates
        Get
            Return cState
        End Get
        Set(ByVal value As ModuleStates)
            cState = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new ModuleWithState
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        ' Default constructor
    End Sub

    ''' <summary>
    ''' Creates a new ModuleWithState with the complete data
    ''' </summary>
    ''' <param name="ModuleID">The ID of the module</param>
    ''' <param name="ModuleChargeID">The ID of the loaded charge</param>
    ''' <param name="ModuleState">The state of the module</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ModuleID As String, ByVal ModuleChargeID As String, ByVal ModuleState As ModuleStates)
        ' Parse the data
        cID = ModuleID
        cChargeID = ModuleChargeID
        cState = ModuleState
    End Sub

End Class

''' <summary>
''' Class to store an itemID together with a state and quantity
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ModuleQWithState
    Dim cID As String
    Dim cState As ModuleStates
    Dim cQuantity As Integer

    ''' <summary>
    ''' Gets or sets the ID of the module
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the typeID of the module</returns>
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
    ''' Gets or sets the state of the module
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value containing the state of the module</returns>
    ''' <remarks></remarks>
    Public Property State() As ModuleStates
        Get
            Return cState
        End Get
        Set(ByVal value As ModuleStates)
            cState = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the quantity of modules
    ''' </summary>
    ''' <value></value>
    ''' <returns>An integer containing the number of modules in the collection</returns>
    ''' <remarks></remarks>
    Public Property Quantity() As Integer
        Get
            Return cQuantity
        End Get
        Set(ByVal value As Integer)
            cQuantity = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new ModuleQWithState
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        ' Default constructor
    End Sub

    ''' <summary>
    ''' Creates a new ModuleQState with the complete data
    ''' </summary>
    ''' <param name="ModuleID">The ID of the module</param>
    ''' <param name="ModuleState">The state of the module</param>
    ''' <param name="ModuleQuantity">The quantity of the modules in the group</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ModuleID As String, ByVal ModuleState As ModuleStates, ByVal ModuleQuantity As Integer)
        ' Parse the data
        cID = ModuleID
        cState = ModuleState
        cQuantity = ModuleQuantity
    End Sub

End Class

''' <summary>
''' Enumeration containing the various states a ShipModule can exist in
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Enum ModuleStates
    Offline = 1
    Inactive = 2
    Active = 4
    Overloaded = 8
    Gang = 16
    Fleet = 32
End Enum

''' <summary>
''' Class to store details of a fleet effect applied to the fitting
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class FleetEffect
    Dim cFleetPosition As FleetPositions
    Dim cFittingName As String
    Dim cPilotName As String
    Dim cImplants As List(Of ModuleWithState)
    Dim cIsActive As Boolean

    ''' <summary>
    ''' Gets or sets the position in the fleet of the effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating the position in the fleet of the effect</returns>
    ''' <remarks></remarks>
    Public Property FleetPosition() As FleetPositions
        Get
            Return cFleetPosition
        End Get
        Set(ByVal value As FleetPositions)
            cFleetPosition = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the fitting for the fleet effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the name of the fitting for the fleet effect</returns>
    ''' <remarks></remarks>
    Public Property FittingName() As String
        Get
            Return cFittingName
        End Get
        Set(ByVal value As String)
            cFittingName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the pilot for the fleet effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the name of the pilot for the fleet effect</returns>
    ''' <remarks></remarks>
    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a collection of implants to be used by the pilot for the fleet effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of implants to be used by the pilot for the fleet effect</returns>
    ''' <remarks></remarks>
    Public Property Implants() As List(Of ModuleWithState)
        Get
            Return cImplants
        End Get
        Set(ByVal value As List(Of ModuleWithState))
            cImplants = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the fleet effect is active
    ''' </summary>
    ''' <value></value>
    ''' <returns>A boolean value indication whether the fleet effect is active</returns>
    ''' <remarks></remarks>
    Public Property IsActive() As Boolean
        Get
            Return cIsActive
        End Get
        Set(ByVal value As Boolean)
            cIsActive = value
        End Set
    End Property

End Class

''' <summary>
''' Enumeration containing the possible positions of gang boosters/commanders
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Enum FleetPositions As Integer
    Fleet = 1
    Wing = 2
    Squad = 3
End Enum

''' <summary>
''' Class to store details of a remote effect applied to the fitting
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class RemoteEffect
    Dim cFittingName As String
    Dim cPilotName As String
    Dim cImplants As List(Of ModuleWithState)
    Dim cModules As List(Of ModuleWithState)
    Dim cIsActive As Boolean

    ''' <summary>
    ''' Gets or sets the name of the fitting for the remote effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the name of the fitting for the remote effect</returns>
    ''' <remarks></remarks>
    Public Property FittingName() As String
        Get
            Return cFittingName
        End Get
        Set(ByVal value As String)
            cFittingName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the pilot for the remote effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the name of the pilot for the remote effect</returns>
    ''' <remarks></remarks>
    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a collection of implants to be used by the pilot for the remote effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of implants to be used by the pilot for the remote effect</returns>
    ''' <remarks></remarks>
    Public Property Implants() As List(Of ModuleWithState)
        Get
            Return cImplants
        End Get
        Set(ByVal value As List(Of ModuleWithState))
            cImplants = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a collection of modules to be used for the remote effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of modules to be used for the remote effect</returns>
    ''' <remarks></remarks>
    Public Property Modules() As List(Of ModuleWithState)
        Get
            Return cModules
        End Get
        Set(ByVal value As List(Of ModuleWithState))
            cModules = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the remote effect is active
    ''' </summary>
    ''' <value></value>
    ''' <returns>A boolean value indication whether the remote effect is active</returns>
    ''' <remarks></remarks>
    Public Property IsActive() As Boolean
        Get
            Return cIsActive
        End Get
        Set(ByVal value As Boolean)
            cIsActive = value
        End Set
    End Property

End Class