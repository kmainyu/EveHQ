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
''' Class for holding an instance of a EveHQ HQF fitting in storage
''' Collection of the class are serialized to the disk for later sessions
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class SavedFitting

    ' ReSharper disable InconsistentNaming - for MS serialization compatability

#Region "Property variables"

    Dim cShipName As String = ""
    Dim cFittingName As String = ""
    Dim cKeyName As String = ""

    Dim cPilotName As String = ""
    Dim cDamageProfileName As String = ""

    Dim cModules As New List(Of ModuleWithState)
    Dim cDrones As New List(Of ModuleQWithState)
    Dim cItems As New List(Of ModuleQWithState)
    Dim cShips As New List(Of ModuleQWithState)

    Dim cImplantGroup As String = ""
    Dim cImplants As New List(Of ModuleWithState)
    Dim cBoosters As New List(Of ModuleWithState)

    Dim cWHEffect As String = ""
    Dim cWHLevel As Integer = 0

    Dim cFleetEffects As New List(Of FleetEffect)
    Dim cRemoteEffects As New List(Of RemoteEffect)

#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets or sets the the Ship Name used for the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the ship used for the fitting</returns>
    ''' <remarks></remarks>
    Public Property ShipName() As String
        Get
            Return cShipName
        End Get
        Set(ByVal value As String)
            cShipName = value
            Call UpdateKeyName()
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the specific fit for the selected ship
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the fitting</returns>
    ''' <remarks></remarks>
    Public Property FittingName() As String
        Get
            Return cFittingName
        End Get
        Set(ByVal value As String)
            cFittingName = value
            Call UpdateKeyName()
        End Set
    End Property

    ''' <summary>
    ''' Gets the unique key name of the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>The unique name of the fitting</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property KeyName() As String
        Get
            Return cKeyName
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the name of the pilot used for the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the pilot used for the fitting</returns>
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
    ''' Gets or sets the name of the profile used for the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the profile used for the fitting</returns>
    ''' <remarks></remarks>
    Public Property DamageProfileName() As String
        Get
            Return cDamageProfileName
        End Get
        Set(ByVal value As String)
            cDamageProfileName = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the collection of basic modules used for the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of modules used in the fitting</returns>
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
    ''' Gets or sets the collection of drones used in the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of drones used in the fitting</returns>
    ''' <remarks></remarks>
    Public Property Drones() As List(Of ModuleQWithState)
        Get
            Return cDrones
        End Get
        Set(ByVal value As List(Of ModuleQWithState))
            cDrones = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the collection of items stored in the cargo bay
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of items stored in the cargo bay</returns>
    ''' <remarks></remarks>
    Public Property Items() As List(Of ModuleQWithState)
        Get
            Return cItems
        End Get
        Set(ByVal value As List(Of ModuleQWithState))
            cItems = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the collection of ships stored in the ship maintenance bay
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of ships stored in the ship maintenance bay</returns>
    ''' <remarks></remarks>
    Public Property Ships() As List(Of ModuleQWithState)
        Get
            Return cShips
        End Get
        Set(ByVal value As List(Of ModuleQWithState))
            cShips = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the implant group used for the pilot
    ''' May be overridden by the contents of the Implants property
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the implant group used for the pilot</returns>
    ''' <remarks></remarks>
    Public Property ImplantGroup() As String
        Get
            Return cImplantGroup
        End Get
        Set(ByVal value As String)
            cImplantGroup = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the collection of implants used for the pilot
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of implants used for the pilot</returns>
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
    ''' Gets or sets the collection of combat boosters used for the pilot
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of combat boosters used for the pilot</returns>
    ''' <remarks></remarks>
    Public Property Boosters() As List(Of ModuleWithState)
        Get
            Return cBoosters
        End Get
        Set(ByVal value As List(Of ModuleWithState))
            cBoosters = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the wormhole effect name
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the wormwhole effect to apply to the fitting</returns>
    ''' <remarks></remarks>
    Public Property WHEffect() As String
        Get
            Return cWHEffect
        End Get
        Set(ByVal value As String)
            cWHEffect = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the level of the wormhole effect
    ''' </summary>
    ''' <value></value>
    ''' <returns>The level of the wormhole effect to apply to the fitting</returns>
    ''' <remarks></remarks>
    Public Property WHLevel() As Integer
        Get
            Return cWHLevel
        End Get
        Set(ByVal value As Integer)
            cWHLevel = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a collection of fleet effects to be applied to the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of fleet effects to be applied to the fitting</returns>
    ''' <remarks></remarks>
    Public Property FleetEffects() As List(Of FleetEffect)
        Get
            Return cFleetEffects
        End Get
        Set(ByVal value As List(Of FleetEffect))
            cFleetEffects = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a collection of remote effects to be applied to the fitting.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of fleet effects to be applied to the fitting.</returns>
    ''' <remarks></remarks>
    Public Property RemoteEffects() As List(Of RemoteEffect)
        Get
            Return cRemoteEffects
        End Get
        Set(ByVal value As List(Of RemoteEffect))
            cRemoteEffects = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets user notes specific to this fitting.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing user notes specific to the fitting.</returns>
    ''' <remarks></remarks>
    Public Property Notes As String

    ''' <summary>
    ''' Gets or sets a list of tags for the fitting.
    ''' </summary>
    ''' <value></value>
    ''' <returns>A list of tags for the fitting.</returns>
    ''' <remarks></remarks>
    Public Property Tags As List(Of String)

    ''' <summary>
    ''' Gets or sets the user rating of a fitting (0-10)
    ''' </summary>
    ''' <value></value>
    ''' <returns>The rating for a fitting</returns>
    ''' <remarks></remarks>
    Public Property Rating As Integer

    

#End Region

    ' ReSharper restore InconsistentNaming

#Region "Class Methods"

    ''' <summary>
    ''' Method to update the key name when changes to the ship name or fitting name occur
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateKeyName()
        If cShipName <> "" And cFittingName <> "" Then
            cKeyName = cShipName & ", " & cFittingName
        End If
    End Sub

#End Region

End Class
