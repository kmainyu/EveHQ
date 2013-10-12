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
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports EveHQ.EveData
Imports EveHQ.Core

''' <summary>
''' Class for holding an instance of a EveHQ HQF fitting used for processing
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Fitting

#Region "Constructors"
    ''' <summary>
    ''' Creates a new instance of the fitting class for internal storage and processing
    ''' </summary>
    ''' <param name="ShipName">The name of the ship to be used for the fitting</param>
    ''' <param name="FittingName">The unique name of the fitting (must be unique within the ship type)</param>
    ''' <param name="PilotName">the name of the pilot to be used for the fitting</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ShipName As String, ByVal FittingName As String, ByVal PilotName As String)
        ' Set the default parameters
        Me.ShipName = ShipName
        Me.FittingName = FittingName
        Me.PilotName = PilotName
        ' Create the base ship
        Me.BaseShip = CType(ShipLists.shipList(Me.ShipName), Ship).Clone
    End Sub

    ''' <summary>
    ''' Creates a new instance of the fitting class for internal storage and processing
    ''' </summary>
    ''' <param name="ShipName">The name of the ship to be used for the fitting</param>
    ''' <param name="FittingName">The unique name of the fitting (must be unique within the ship type)</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ShipName As String, ByVal FittingName As String)
        ' Set the default parameters
        Me.ShipName = ShipName
        Me.FittingName = FittingName
        ' Create the base ship
        Me.BaseShip = CType(ShipLists.shipList(Me.ShipName), Ship).Clone
    End Sub

#End Region

#Region "Property variables"

    Dim cShipName As String = ""
    Dim cFittingName As String = ""
    Dim cKeyName As String = ""

    Dim cPilotName As String = ""
    Dim cDamageProfileName As String = ""
    Dim cDefenceProfileName As String = ""

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

    Dim cBaseShip As Ship
    Dim cFittedShip As Ship
    Dim cShipSlotCtrl As ShipSlotControl
    Dim cShipInfoCtrl As ShipInfoControl

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
            Call Me.UpdateKeyName()
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
            Call Me.UpdateKeyName()
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
            If FittingPilots.HQFPilots.ContainsKey(value) = False Then
                '  MessageBox.Show("The pilot '" & value & "' is not a listed pilot. The system will now try to use your configured default pilot instead for this fit (" & Me.FittingName & ").", "Unknown Pilot", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If FittingPilots.HQFPilots.ContainsKey(Settings.HQFSettings.DefaultPilot) Then
                    'Fall back to the configured default pilot if they are valid.
                    cPilotName = Settings.HQFSettings.DefaultPilot
                Else
                    ' Even the configured default isn't valid... fallback to the first valid pilot in the collection
                    If FittingPilots.HQFPilots.Count > 0 Then
                        cPilotName = FittingPilots.HQFPilots.Values(0).PilotName
                        ' Thankfully there is already a check for pilots when HQF starts up...
                    End If
                End If
            Else
                ' original pilot name is good... use it.
                cPilotName = value
            End If
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
            If cDamageProfileName <> "" Then
                If HQFDamageProfiles.ProfileList.ContainsKey(cDamageProfileName) = False Then
                    cDamageProfileName = "<Omni-Damage>"
                End If
                Dim curProfile As HQFDamageProfile = HQFDamageProfiles.ProfileList.Item(cDamageProfileName)
                If cBaseShip IsNot Nothing Then
                    cBaseShip.DamageProfile = curProfile
                End If
                If cFittedShip IsNot Nothing Then
                    cFittedShip.DamageProfile = curProfile
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the name of the defence profile used for the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the defence profile used for the fitting</returns>
    ''' <remarks></remarks>
    Public Property DefenceProfileName() As String
        Get
            Return cDefenceProfileName
        End Get
        Set(ByVal value As String)
            cDefenceProfileName = value
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
    ''' Gets or sets a collection of remote effects to be applied to the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of fleet effects to be applied to the fitting</returns>
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
    ''' Gets or sets the ship to use as the basis for starting a calculation
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of the ship class used as a basis for the fitting calculation</returns>
    ''' <remarks></remarks>
    Public Property BaseShip() As Ship
        Get
            Return cBaseShip
        End Get
        Set(ByVal value As Ship)
            cBaseShip = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the final ship which has been processed from the BaseShip property
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of the ship class which has been processed from the BaseShip property</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FittedShip() As Ship
        Get
            Return cFittedShip
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the ShipInfoControl used when the fitting is open for configuration
    ''' </summary>
    ''' <value></value>
    ''' <returns>The ShipInfoControl used when the fitting is open for configuration</returns>
    ''' <remarks></remarks>
    Public Property ShipInfoCtrl() As ShipInfoControl
        Get
            Return cShipInfoCtrl
        End Get
        Set(ByVal value As ShipInfoControl)
            cShipInfoCtrl = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the ShipSlotControl used when the fitting is open for configuration
    ''' </summary>
    ''' <value></value>
    ''' <returns>The ShipSlotControl used when the fitting is open for configuration</returns>
    ''' <remarks></remarks>
    Public Property ShipSlotCtrl() As ShipSlotControl
        Get
            Return cShipSlotCtrl
        End Get
        Set(ByVal value As ShipSlotControl)
            cShipSlotCtrl = value
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
    Public Property Tags As New List(Of String)

#End Region

#Region "Fitting Mapping Collections"
    Private ReadOnly _skillEffectsTable As New SortedList(Of Integer, List(Of FinalEffect))
    Private ReadOnly _moduleEffectsTable As New SortedList(Of Integer, List(Of FinalEffect))
    Private ReadOnly _chargeEffectsTable As New SortedList(Of Integer, List(Of FinalEffect))
#End Region

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

#Region "Public Fitting Methods"

    ''' <summary>
    ''' Calculates the final ship stats based on the internal base ship and pilot
    ''' </summary>
    ''' <param name="buildMethod"></param>
    ''' <remarks></remarks>
    Public Sub ApplyFitting(Optional ByVal buildMethod As BuildType = BuildType.BuildEverything, Optional ByVal visualUpdates As Boolean = True)
        ' Update the pilot from the pilot name

        Dim baseShip As Ship = Me.BaseShip
        Dim shipPilot As FittingPilot = FittingPilots.HQFPilots(PilotName)

        ' Setup performance info - just in case!
        Const stages As Integer = 23
        Dim pStages(stages) As String
        Dim pStageTime(stages) As DateTime
        pStages(0) = "Start Timing: "
        pStages(1) = "Building Skill Effects: "
        pStages(2) = "Building Implant Effects: "
        pStages(3) = "Building Ship Bonuses: "
        pStages(4) = "Building External Influences: "
        pStages(5) = "Collecting Modules: "
        pStages(6) = "Applying Skill Effects to Ship: "
        pStages(7) = "Applying Skill Effects to Modules: "
        pStages(8) = "Applying Skill Effects to Drones: "
        pStages(9) = "Building Module Effects: "
        pStages(10) = "Applying Stacking Penalties: "
        pStages(11) = "Applying Module Effects to Charges: "
        pStages(12) = "Build Charge Effects: "
        pStages(13) = "Applying Charge Effects to Modules: "
        pStages(14) = "Applying Charge Effects to Ship: "
        pStages(15) = "Rebuilding Module Effects: "
        pStages(16) = "Recalculating Stacking Penalties: "
        pStages(17) = "Applying Module Effects to Modules: "
        pStages(18) = "Rebuilding Module Effects: "
        pStages(19) = "Recalculating Stacking Penalties: "
        pStages(20) = "Applying Module Effects to Drones: "
        pStages(21) = "Applying Module Effects to Ship: "
        pStages(22) = "Calculating Damage Statistics: "
        pStages(23) = "Calculating Defence Statistics: "
        ' Apply the pilot skills to the ship
        Dim newShip As New Ship
        Select Case buildMethod
            Case BuildType.BuildEverything
                pStageTime(0) = Now
                Me.BuildSkillEffects(shipPilot)
                pStageTime(1) = Now
                Me.BuildImplantEffects(shipPilot)
                pStageTime(2) = Now
                Me.BuildShipBonuses(shipPilot, baseShip)
                pStageTime(3) = Now
                Me.BuildExternalModules()
                pStageTime(4) = Now
                newShip = Me.CollectModules(CType(baseShip.Clone, Ship))
                pStageTime(5) = Now
                Me.ApplySkillEffectsToShip(newShip)
                pStageTime(6) = Now
                Me.ApplySkillEffectsToModules(newShip)
                pStageTime(7) = Now
                Me.ApplySkillEffectsToDrones(newShip)
                pStageTime(8) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(9) = Now
                Me.ApplyStackingPenalties()
                pStageTime(10) = Now
                Me.ApplyModuleEffectsToCharges(newShip)
                pStageTime(11) = Now
                Me.BuildChargeEffects(newShip)
                pStageTime(12) = Now
                Me.ApplyChargeEffectsToModules(newShip)
                pStageTime(13) = Now
                Me.ApplyChargeEffectsToShip(newShip)
                pStageTime(14) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(15) = Now
                Me.ApplyStackingPenalties()
                pStageTime(16) = Now
                Me.ApplyModuleEffectsToModules(newShip)
                pStageTime(17) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(18) = Now
                Me.ApplyStackingPenalties()
                pStageTime(19) = Now
                Me.ApplyModuleEffectsToDrones(newShip)
                pStageTime(20) = Now
                Me.ApplyModuleEffectsToShip(newShip)
                pStageTime(21) = Now
                Me.CalculateDamageStatistics(newShip)
                pStageTime(22) = Now
                Ship.MapShipAttributes(newShip)
                Me.CalculateDefenceStatistics(newShip)
                pStageTime(23) = Now
            Case BuildType.BuildEffectsMaps
                pStageTime(0) = Now
                Me.BuildSkillEffects(shipPilot)
                pStageTime(1) = Now
                Me.BuildImplantEffects(shipPilot)
                pStageTime(2) = Now
                Me.BuildShipBonuses(shipPilot, baseShip)
                pStageTime(3) = Now
                Me.BuildExternalModules()
                pStageTime(4) = Now
                'newShip = Me.CollectModules(CType(baseShip.Clone, Ship))
                pStageTime(5) = Now
                'Me.ApplySkillEffectsToShip(newShip)
                pStageTime(6) = Now
                'Me.ApplySkillEffectsToModules(newShip)
                pStageTime(7) = Now
                'Me.ApplySkillEffectsToDrones(newShip)
                pStageTime(8) = Now
                'Me.BuildModuleEffects(newShip)
                pStageTime(9) = Now
                'Me.ApplyStackingPenalties()
                pStageTime(10) = Now
                'Me.ApplyModuleEffectsToCharges(newShip)
                pStageTime(11) = Now
                'Me.BuildChargeEffects(newShip)
                pStageTime(12) = Now
                'Me.ApplyChargeEffectsToModules(newShip)
                pStageTime(13) = Now
                'Me.ApplyChargeEffectsToShip(newShip)
                pStageTime(14) = Now
                'Me.BuildModuleEffects(newShip)
                pStageTime(15) = Now
                'Me.ApplyStackingPenalties()
                pStageTime(16) = Now
                'Me.ApplyModuleEffectsToModules(newShip)
                pStageTime(17) = Now
                'Me.BuildModuleEffects(newShip)
                pStageTime(18) = Now
                'Me.ApplyStackingPenalties()
                pStageTime(19) = Now
                'Me.ApplyModuleEffectsToDrones(newShip)
                pStageTime(20) = Now
                'Me.ApplyModuleEffectsToShip(newShip)
                pStageTime(21) = Now
                'Me.CalculateDamageStatistics(newShip)
                pStageTime(22) = Now
                'Ship.MapShipAttributes(newShip)
                'Me.CalculateDefenceStatistics(newShip)
                pStageTime(23) = Now
            Case BuildType.BuildFromEffectsMaps
                pStageTime(0) = Now
                'Me.BuildSkillEffects(shipPilot)
                pStageTime(1) = Now
                'Me.BuildImplantEffects(shipPilot)
                pStageTime(2) = Now
                'Me.BuildShipBonuses(shipPilot, baseShip)
                pStageTime(3) = Now
                'Me.BuildExternalModules()
                pStageTime(4) = Now
                newShip = Me.CollectModules(CType(baseShip.Clone, Ship))
                pStageTime(5) = Now
                Me.ApplySkillEffectsToShip(newShip)
                pStageTime(6) = Now
                Me.ApplySkillEffectsToModules(newShip)
                pStageTime(7) = Now
                Me.ApplySkillEffectsToDrones(newShip)
                pStageTime(8) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(9) = Now
                Me.ApplyStackingPenalties()
                pStageTime(10) = Now
                Me.ApplyModuleEffectsToCharges(newShip)
                pStageTime(11) = Now
                Me.BuildChargeEffects(newShip)
                pStageTime(12) = Now
                Me.ApplyChargeEffectsToModules(newShip)
                pStageTime(13) = Now
                Me.ApplyChargeEffectsToShip(newShip)
                pStageTime(14) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(15) = Now
                Me.ApplyStackingPenalties()
                pStageTime(16) = Now
                Me.ApplyModuleEffectsToModules(newShip)
                pStageTime(17) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(18) = Now
                Me.ApplyStackingPenalties()
                pStageTime(19) = Now
                Me.ApplyModuleEffectsToDrones(newShip)
                pStageTime(20) = Now
                Me.ApplyModuleEffectsToShip(newShip)
                pStageTime(21) = Now
                Me.CalculateDamageStatistics(newShip)
                pStageTime(22) = Now
                Ship.MapShipAttributes(newShip)
                Me.CalculateDefenceStatistics(newShip)
                pStageTime(23) = Now
        End Select
        If Settings.HQFSettings.ShowPerformanceData = True Then
            Dim dTime As TimeSpan
            Dim perfMsg As String = ""
            For stage As Integer = 1 To stages
                perfMsg &= pStages(stage)
                dTime = pStageTime(stage) - pStageTime(stage - 1)
                perfMsg &= dTime.TotalMilliseconds.ToString("N2") & "ms" & ControlChars.CrLf
            Next
            dTime = pStageTime(stages) - pStageTime(0)
            perfMsg &= "Total Time: " & dTime.TotalMilliseconds.ToString("N2") & "ms" & ControlChars.CrLf
            MessageBox.Show(perfMsg, "Performance Data Results: Method " & buildMethod, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Ship.MapShipAttributes(newShip)
        cFittedShip = newShip

        If Me.ShipSlotCtrl IsNot Nothing And VisualUpdates = True Then
            'Dim SSC As New Threading.Thread(AddressOf Me.ShipSlotCtrl.UpdateAllSlotLocations)
            'SSC.Priority = Threading.ThreadPriority.Highest
            'SSC.Start()
            Me.ShipSlotCtrl.UpdateAllSlotLocations()
        End If

        If Me.ShipInfoCtrl IsNot Nothing And VisualUpdates = True Then
            'Dim SIC As New Threading.Thread(AddressOf Me.ShipInfoCtrl.UpdateInfoDisplay)
            'SIC.Priority = Threading.ThreadPriority.Highest
            'SIC.Start()
            Me.ShipInfoCtrl.UpdateInfoDisplay()
        End If

    End Sub

#End Region

#Region "Private Fitting Routines"
    Private Sub BuildSkillEffects(ByVal hPilot As FittingPilot)
        ' Clear the Effects Table
        _skillEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim aSkill As New Skill
        Dim fEffect As New FinalEffect
        Dim fEffectList As New List(Of FinalEffect)
        For Each hSkill As FittingSkill In hPilot.SkillSet.Values
            If SkillLists.SkillList.ContainsKey(hSkill.ID) = True Then
                If hSkill.Level <> 0 Then
                    ' Go through the attributes
                    aSkill = SkillLists.SkillList(hSkill.ID)
                    Try
                        For Each att As Integer In aSkill.Attributes.Keys
                            If Engine.EffectsMap.ContainsKey(att) = True Then
                                For Each chkEffect As Effect In Engine.EffectsMap(att)
                                    If chkEffect.AffectingType = HQFEffectType.Item And chkEffect.AffectingID = CInt(aSkill.ID) Then
                                        fEffect = New FinalEffect
                                        fEffect.AffectedAtt = chkEffect.AffectedAtt
                                        fEffect.AffectedType = chkEffect.AffectedType
                                        fEffect.AffectedID = chkEffect.AffectedID
                                        Select Case chkEffect.CalcType
                                            Case EffectCalcType.SkillLevel, EffectCalcType.SkillLevelxAtt
                                                fEffect.AffectedValue = hSkill.Level
                                            Case Else
                                                fEffect.AffectedValue = CDbl(aSkill.Attributes(att)) * hSkill.Level
                                        End Select
                                        fEffect.StackNerf = chkEffect.StackNerf
                                        fEffect.Cause = hSkill.Name & " (Level " & hSkill.Level & ")"
                                        fEffect.CalcType = chkEffect.CalcType
                                        fEffect.Status = chkEffect.Status
                                        If _skillEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                            fEffectList = New List(Of FinalEffect)
                                            _skillEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                                        Else
                                            fEffectList = _skillEffectsTable(fEffect.AffectedAtt)
                                        End If
                                        fEffectList.Add(fEffect)
                                    End If
                                Next
                            End If
                        Next
                    Catch e As Exception
                        MessageBox.Show("An error occured trying to process the skill effects for " & hSkill.Name & "!", "HQF Engine Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Try
                End If
            End If
        Next
    End Sub
    Private Sub BuildImplantEffects(ByVal hPilot As FittingPilot)
        ' Run through the implants and see if we have any pirate implants
        Dim hImplant As String
        Dim aImplant As ShipModule
        Dim piGroup As String
        Dim cPirateImplantGroups As SortedList = CType(Engine.PirateImplantGroups.Clone, SortedList)
        For slotNo As Integer = 1 To 10
            hImplant = hPilot.ImplantName(slotNo)
            If Engine.PirateImplants.ContainsKey(hImplant) = True Then
                ' We have a pirate implant so let's work out the group and the set bonus
                piGroup = CStr(Engine.PirateImplants.Item(hImplant))
                aImplant = ModuleLists.ModuleList(ModuleLists.ModuleListName(hImplant))
                Select Case piGroup
                    Case "Centurion", "Low-grade Centurion"
                        cPirateImplantGroups.Item("Centurion") = CDbl(cPirateImplantGroups.Item("Centurion")) * CDbl(aImplant.Attributes(1293))
                        cPirateImplantGroups.Item("Low-grade Centurion") = CDbl(cPirateImplantGroups.Item("Low-grade Centurion")) * CDbl(aImplant.Attributes(1293))
                    Case "Crystal", "Low-grade Crystal"
                        cPirateImplantGroups.Item("Crystal") = CDbl(cPirateImplantGroups.Item("Crystal")) * CDbl(aImplant.Attributes(838))
                        cPirateImplantGroups.Item("Low-grade Crystal") = CDbl(cPirateImplantGroups.Item("Low-grade Crystal")) * CDbl(aImplant.Attributes(838))
                    Case "Edge", "Low-grade Edge"
                        cPirateImplantGroups.Item("Edge") = CDbl(cPirateImplantGroups.Item("Edge")) * CDbl(aImplant.Attributes(1291))
                        cPirateImplantGroups.Item("Low-grade Edge") = CDbl(cPirateImplantGroups.Item("Low-grade Edge")) * CDbl(aImplant.Attributes(1291))
                    Case "Grail"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1550))
                    Case "Low-grade Grail"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1569))
                    Case "Halo", "Low-grade Halo"
                        cPirateImplantGroups.Item("Halo") = CDbl(cPirateImplantGroups.Item("Halo")) * CDbl(aImplant.Attributes(863))
                        cPirateImplantGroups.Item("Low-grade Halo") = CDbl(cPirateImplantGroups.Item("Low-grade Halo")) * CDbl(aImplant.Attributes(863))
                    Case "Harvest", "Low-grade Harvest"
                        cPirateImplantGroups.Item("Harvest") = CDbl(cPirateImplantGroups.Item("Harvest")) * CDbl(aImplant.Attributes(1292))
                        cPirateImplantGroups.Item("Low-grade Harvest") = CDbl(cPirateImplantGroups.Item("Low-grade Harvest")) * CDbl(aImplant.Attributes(1292))
                    Case "Jackal"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1554))
                    Case "Low-grade Jackal"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1572))
                    Case "Nomad", "Low-grade Nomad"
                        cPirateImplantGroups.Item("Nomad") = CDbl(cPirateImplantGroups.Item("Nomad")) * CDbl(aImplant.Attributes(1282))
                        cPirateImplantGroups.Item("Low-grade Nomad") = CDbl(cPirateImplantGroups.Item("Low-grade Nomad")) * CDbl(aImplant.Attributes(1282))
                    Case "Slave", "Low-grade Slave"
                        cPirateImplantGroups.Item("Slave") = CDbl(cPirateImplantGroups.Item("Slave")) * CDbl(aImplant.Attributes(864))
                        cPirateImplantGroups.Item("Low-grade Slave") = CDbl(cPirateImplantGroups.Item("Low-grade Slave")) * CDbl(aImplant.Attributes(864))
                    Case "Snake", "Low-grade Snake"
                        cPirateImplantGroups.Item("Snake") = CDbl(cPirateImplantGroups.Item("Snake")) * CDbl(aImplant.Attributes(802))
                        cPirateImplantGroups.Item("Low-grade Snake") = CDbl(cPirateImplantGroups.Item("Low-grade Snake")) * CDbl(aImplant.Attributes(802))
                    Case "Spur"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1553))
                    Case "Low-grade Spur"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1570))
                    Case "Talisman", "Low-grade Talisman"
                        cPirateImplantGroups.Item("Talisman") = CDbl(cPirateImplantGroups.Item("Talisman")) * CDbl(aImplant.Attributes(799))
                        cPirateImplantGroups.Item("Low-grade Talisman") = CDbl(cPirateImplantGroups.Item("Low-grade Talisman")) * CDbl(aImplant.Attributes(799))
                    Case "Talon"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1552))
                    Case "Low-grade Talon"
                        cPirateImplantGroups.Item(piGroup) = CDbl(cPirateImplantGroups.Item(piGroup)) * CDbl(aImplant.Attributes(1571))
                    Case "Virtue", "Low-grade Virtue"
                        cPirateImplantGroups.Item("Virtue") = CDbl(cPirateImplantGroups.Item("Virtue")) * CDbl(aImplant.Attributes(1284))
                        cPirateImplantGroups.Item("Low-grade Virtue") = CDbl(cPirateImplantGroups.Item("Low-grade Virtue")) * CDbl(aImplant.Attributes(1284))
                    Case "Genolution Core Augmentation"
                        cPirateImplantGroups("Genolution Core Augmentation") = CDbl(cPirateImplantGroups.Item("Genolution Core Augmentation")) * CDbl(aImplant.Attributes(1799))
                End Select
            End If
        Next

        ' Go through all the implants and see what needs to be mapped
        Dim fEffect As New FinalEffect
        Dim fEffectList As New List(Of FinalEffect)

        For slotNo As Integer = 1 To 10
            hImplant = hPilot.ImplantName(slotNo)
            If hImplant <> "" Then
                ' Go through the attributes
                If ModuleLists.moduleListName.ContainsKey(hImplant) = True Then
                    aImplant = CType(ModuleLists.moduleList(ModuleLists.moduleListName(hImplant)), ShipModule)
                    If Engine.ImplantEffectsMap.Contains(hImplant) = True Then
                        For Each chkEffect As ImplantEffect In CType(Engine.ImplantEffectsMap(hImplant), ArrayList)
                            If chkEffect.IsGang = False Then
                                If aImplant.Attributes.ContainsKey(chkEffect.AffectingAtt) = True Then
                                    fEffect = New FinalEffect
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    fEffect.AffectedID = chkEffect.AffectedID
                                    If Engine.PirateImplants.ContainsKey(aImplant.Name) = True Then
                                        piGroup = CStr(Engine.PirateImplants.Item(hImplant))
                                        fEffect.AffectedValue = CDbl(aImplant.Attributes(chkEffect.AffectingAtt)) * CDbl(cPirateImplantGroups.Item(piGroup))
                                        fEffect.Cause = aImplant.Name & " (Set Bonus: " & CDbl(cPirateImplantGroups.Item(piGroup)).ToString("N3") & "x)"
                                    Else
                                        fEffect.AffectedValue = CDbl(aImplant.Attributes(chkEffect.AffectingAtt))
                                        fEffect.Cause = aImplant.Name
                                    End If
                                    fEffect.StackNerf = 0
                                    fEffect.CalcType = chkEffect.CalcType
                                    If _skillEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                        fEffectList = New List(Of FinalEffect)
                                        _skillEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                                    Else
                                        fEffectList = _skillEffectsTable(fEffect.AffectedAtt)
                                    End If
                                    fEffectList.Add(fEffect)
                                End If
                            End If
                        Next
                    End If
                Else
                    Dim msg As String = "Unable to find the implant: " & hImplant & "." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "This implant will be removed from the pilot's implant list. "
                    msg &= "Please go into the Pilot Manager and replace the implant if required."
                    MessageBox.Show(msg, "Unable to Locate Implant", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    hPilot.ImplantName(slotNo) = ""
                End If
            End If
        Next
    End Sub
    Private Sub BuildShipBonuses(ByVal hPilot As FittingPilot, ByVal hShip As Ship)
        If hShip IsNot Nothing Then
            ' Go through all the skills and see what needs to be mapped
            Dim shipRoles As List(Of ShipEffect)
            Dim hSkill As FittingSkill
            Dim fEffect As FinalEffect
            Dim fEffectList As List(Of FinalEffect)
            If Engine.ShipBonusesMap.ContainsKey(hShip.ID) = True Then
                shipRoles = Engine.ShipBonusesMap(hShip.ID)
                If shipRoles IsNot Nothing Then
                    For Each chkEffect As ShipEffect In shipRoles
                        If chkEffect.Status <> 16 Then
                            fEffect = New FinalEffect
                            If hPilot.SkillSet.ContainsKey(EveHQ.Core.SkillFunctions.SkillIDToName(chkEffect.AffectingID)) = True Then
                                hSkill = hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(chkEffect.AffectingID))
                                If chkEffect.IsPerLevel = True Then
                                    fEffect.AffectedValue = chkEffect.Value * hSkill.Level
                                    fEffect.Cause = "Ship Bonus - " & hSkill.Name & " (Level " & hSkill.Level & ")"
                                Else
                                    fEffect.AffectedValue = chkEffect.Value
                                    fEffect.Cause = "Ship Role - "
                                End If
                            Else
                                fEffect.AffectedValue = chkEffect.Value
                                fEffect.Cause = "Ship Role - "
                            End If
                            fEffect.AffectedAtt = chkEffect.AffectedAtt
                            fEffect.AffectedType = chkEffect.AffectedType
                            fEffect.AffectedID = chkEffect.AffectedID
                            fEffect.StackNerf = chkEffect.StackNerf
                            fEffect.CalcType = chkEffect.CalcType
                            If _skillEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                fEffectList = New List(Of FinalEffect)
                                _skillEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                            Else
                                fEffectList = _skillEffectsTable(fEffect.AffectedAtt)
                            End If
                            fEffectList.Add(fEffect)
                        End If
                    Next
                End If
            End If
            ' Get the ship effects
            Dim processData As Boolean
            For Each att As Integer In hShip.Attributes.Keys
                If Engine.ShipEffectsMap.ContainsKey(att) = True Then
                    For Each chkEffect As Effect In Engine.ShipEffectsMap(att)
                        processData = False
                        Select Case chkEffect.AffectingType
                            Case HQFEffectType.All
                                processData = True
                            Case HQFEffectType.Item
                                If chkEffect.AffectingID = hShip.ID Then
                                    processData = True
                                End If
                            Case HQFEffectType.Group
                                If chkEffect.AffectingID = hShip.DatabaseGroup Then
                                    processData = True
                                End If
                            Case HQFEffectType.Category
                                If chkEffect.AffectingID = hShip.DatabaseCategory Then
                                    processData = True
                                End If
                            Case HQFEffectType.MarketGroup
                                If chkEffect.AffectingID = hShip.MarketGroup Then
                                    processData = True
                                End If
                            Case HQFEffectType.Skill
                                If hShip.RequiredSkills.ContainsKey(chkEffect.AffectingID.ToString) Then
                                    processData = True
                                End If
                            Case HQFEffectType.Slot
                                processData = True
                            Case HQFEffectType.Attribute
                                If hShip.Attributes.ContainsKey(chkEffect.AffectingID) Then
                                    processData = True
                                End If
                        End Select
                        If processData = True Then
                            fEffect = New FinalEffect
                            fEffect.AffectedAtt = chkEffect.AffectedAtt
                            fEffect.AffectedType = chkEffect.AffectedType
                            fEffect.AffectedID = chkEffect.AffectedID
                            fEffect.AffectedValue = hShip.Attributes(att)
                            fEffect.StackNerf = chkEffect.StackNerf
                            fEffect.Cause = hShip.Name
                            fEffect.CalcType = chkEffect.CalcType
                            If _skillEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                fEffectList = New List(Of FinalEffect)
                                _skillEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                            Else
                                fEffectList = _skillEffectsTable(fEffect.AffectedAtt)
                            End If
                            fEffectList.Add(fEffect)
                        End If
                    Next
                End If
            Next
            ' Get the bonuses from the subsystems
            If hShip.SubSlotsUsed > 0 Then
                For slot As Integer = 1 To hShip.SubSlots
                    If hShip.SubSlot(slot) IsNot Nothing Then
                        shipRoles = Engine.SubSystemEffectsMap(hShip.SubSlot(slot).ID)
                        If shipRoles IsNot Nothing Then
                            For Each chkEffect As ShipEffect In shipRoles
                                If chkEffect.Status <> 16 Then
                                    fEffect = New FinalEffect
                                    If hPilot.SkillSet.ContainsKey(EveHQ.Core.SkillFunctions.SkillIDToName(chkEffect.AffectingID)) = True Then
                                        hSkill = hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(chkEffect.AffectingID))
                                        If chkEffect.IsPerLevel = True Then
                                            fEffect.AffectedValue = chkEffect.Value * hSkill.Level
                                            fEffect.Cause = "Subsystem Bonus - " & hSkill.Name & " (Level " & hSkill.Level & ")"
                                        Else
                                            fEffect.AffectedValue = chkEffect.Value
                                            fEffect.Cause = "Subsystem Role - "
                                        End If
                                    Else
                                        fEffect.AffectedValue = chkEffect.Value
                                        fEffect.Cause = "Subsystem Role - "
                                    End If
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    fEffect.AffectedID = chkEffect.AffectedID
                                    fEffect.StackNerf = chkEffect.StackNerf
                                    fEffect.CalcType = chkEffect.CalcType
                                    If _skillEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                        fEffectList = New List(Of FinalEffect)
                                        _skillEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                                    Else
                                        fEffectList = _skillEffectsTable(fEffect.AffectedAtt)
                                    End If
                                    fEffectList.Add(fEffect)
                                End If
                            Next
                        End If
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub BuildChargeEffects(ByRef newShip As Ship)
        ' Clear the Effects Table
        _chargeEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim fEffect As New FinalEffect
        Dim fEffectList As New List(Of FinalEffect)
        Dim processData As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
            If aModule.LoadedCharge IsNot Nothing Then
                For Each att As Integer In aModule.LoadedCharge.Attributes.Keys
                    If Engine.EffectsMap.ContainsKey(att) = True Then
                        For Each chkEffect As Effect In Engine.EffectsMap(att)
                            processData = False
                            Select Case chkEffect.AffectingType
                                Case HQFEffectType.All
                                    processData = True
                                Case HQFEffectType.Item
                                    If chkEffect.AffectingID = aModule.LoadedCharge.ID Then
                                        processData = True
                                    End If
                                Case HQFEffectType.Group
                                    If chkEffect.AffectingID = aModule.LoadedCharge.DatabaseGroup Then
                                        processData = True
                                    End If
                                Case HQFEffectType.Category
                                    If chkEffect.AffectingID = aModule.LoadedCharge.DatabaseCategory Then
                                        processData = True
                                    End If
                                Case HQFEffectType.MarketGroup
                                    If chkEffect.AffectingID = aModule.LoadedCharge.MarketGroup Then
                                        processData = True
                                    End If
                                Case HQFEffectType.Skill
                                    If aModule.LoadedCharge.RequiredSkills.ContainsKey(chkEffect.AffectingID.ToString) Then
                                        processData = True
                                    End If
                                Case HQFEffectType.Slot
                                    processData = True
                                Case HQFEffectType.Attribute
                                    If aModule.LoadedCharge.Attributes.ContainsKey(chkEffect.AffectingID) Then
                                        processData = True
                                    End If
                            End Select
                            If processData = True And (aModule.LoadedCharge.ModuleState And chkEffect.Status) = aModule.LoadedCharge.ModuleState Then
                                fEffect = New FinalEffect
                                fEffect.AffectedAtt = chkEffect.AffectedAtt
                                fEffect.AffectedType = chkEffect.AffectedType
                                If chkEffect.AffectedType = HQFEffectType.Slot Then
                                    fEffect.AffectedID.Add(CInt(aModule.SlotType & aModule.SlotNo))
                                Else
                                    fEffect.AffectedID = chkEffect.AffectedID
                                End If
                                fEffect.AffectedValue = CDbl(aModule.LoadedCharge.Attributes(att))
                                fEffect.StackNerf = chkEffect.StackNerf
                                fEffect.Cause = aModule.LoadedCharge.Name
                                fEffect.CalcType = chkEffect.CalcType
                                If _chargeEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                    fEffectList = New List(Of FinalEffect)
                                    _chargeEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                                Else
                                    fEffectList = _chargeEffectsTable(fEffect.AffectedAtt)
                                End If
                                fEffectList.Add(fEffect)
                            End If
                        Next
                    End If
                Next
            End If ' End of LoadedCharge checking
        Next
    End Sub
    Private Sub BuildModuleEffects(ByRef newShip As Ship)
        ' Clear the Effects Table
        _moduleEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim fEffect As New FinalEffect
        Dim fEffectList As New List(Of FinalEffect)
        Dim processData As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
            For Each att As Integer In aModule.Attributes.Keys
                If Engine.EffectsMap.ContainsKey(att) = True Then
                    For Each chkEffect As Effect In Engine.EffectsMap(att)
                        processData = False
                        Select Case chkEffect.AffectingType
                            Case HQFEffectType.All
                                processData = True
                            Case HQFEffectType.Item
                                If chkEffect.AffectingID = aModule.ID Then
                                    processData = True
                                End If
                            Case HQFEffectType.Group
                                If chkEffect.AffectingID = aModule.DatabaseGroup Then
                                    processData = True
                                End If
                            Case HQFEffectType.Category
                                If chkEffect.AffectingID = aModule.DatabaseCategory Then
                                    processData = True
                                End If
                            Case HQFEffectType.MarketGroup
                                If chkEffect.AffectingID = aModule.MarketGroup Then
                                    processData = True
                                End If
                            Case HQFEffectType.Skill
                                If aModule.RequiredSkills.ContainsKey(chkEffect.AffectingID.ToString) Then
                                    processData = True
                                End If
                            Case HQFEffectType.Slot
                                processData = True
                            Case HQFEffectType.Attribute
                                If aModule.Attributes.ContainsKey(chkEffect.AffectingID) Then
                                    processData = True
                                End If
                        End Select
                        ' Check for Ancillary Armor Repairer charge because Nanite Repair Paste has no effect-mappable attribute
                        Dim cause As String = ""
                        If aModule.DatabaseGroup = ModuleEnum.GroupFueledArmorRepairers And att = AttributeEnum.ModuleArmorBoostedRepairMultiplier Then
                            If aModule.LoadedCharge Is Nothing Then
                                processData = False
                            Else
                                cause = aModule.LoadedCharge.Name
                            End If
                        End If
                        If processData = True And (aModule.ModuleState And chkEffect.Status) = aModule.ModuleState Then
                            fEffect = New FinalEffect
                            fEffect.AffectedAtt = chkEffect.AffectedAtt
                            fEffect.AffectedType = chkEffect.AffectedType
                            If chkEffect.AffectedType = HQFEffectType.Slot Then
                                fEffect.AffectedID.Add(CInt(aModule.SlotType & aModule.SlotNo))
                            Else
                                fEffect.AffectedID = chkEffect.AffectedID
                            End If
                            fEffect.AffectedValue = aModule.Attributes(att)
                            fEffect.StackNerf = chkEffect.StackNerf
                            fEffect.Cause = If(cause = "", aModule.Name, cause)
                            fEffect.CalcType = chkEffect.CalcType
                            If _moduleEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                fEffectList = New List(Of FinalEffect)
                                _moduleEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                            Else
                                fEffectList = _moduleEffectsTable(fEffect.AffectedAtt)
                            End If
                            fEffectList.Add(fEffect)
                        End If
                    Next
                End If
            Next
        Next
    End Sub
    Public Function BuildSubSystemEffects(ByVal cShip As Ship) As Ship

        Dim newShip As Ship = ShipLists.ShipList(cShip.Name).Clone

        If newShip.SubSlots > 0 Then
            For slot As Integer = 1 To newShip.SubSlots
                newShip.SubSlot(slot) = cShip.SubSlot(slot)
            Next
        End If

        ' Clear the Effects Table
        Dim ssEffectsTable As New SortedList(Of Integer, List(Of FinalEffect))
        ' Go through all the skills and see what needs to be mapped
        Dim att As Integer
        Dim fEffect As FinalEffect
        Dim fEffectList As List(Of FinalEffect)
        Dim aModule As ShipModule
        Dim processData As Boolean
        If newShip.SubSlots > 0 Then
            For s As Integer = 1 To newShip.SubSlots
                aModule = newShip.SubSlot(s)
                If aModule IsNot Nothing Then
                    For Each att In aModule.Attributes.Keys
                        If Engine.EffectsMap.ContainsKey(att) = True Then
                            For Each chkEffect As Effect In Engine.EffectsMap(att)
                                processData = False
                                Select Case chkEffect.AffectingType
                                    Case HQFEffectType.All
                                        processData = True
                                    Case HQFEffectType.Item
                                        If chkEffect.AffectingID = aModule.ID Then
                                            processData = True
                                        End If
                                    Case HQFEffectType.Group
                                        If chkEffect.AffectingID = aModule.DatabaseGroup Then
                                            processData = True
                                        End If
                                    Case HQFEffectType.Category
                                        If chkEffect.AffectingID = aModule.DatabaseCategory Then
                                            processData = True
                                        End If
                                    Case HQFEffectType.MarketGroup
                                        If chkEffect.AffectingID = aModule.MarketGroup Then
                                            processData = True
                                        End If
                                    Case HQFEffectType.Skill
                                        If aModule.RequiredSkills.ContainsKey(chkEffect.AffectingID.ToString) Then
                                            processData = True
                                        End If
                                    Case HQFEffectType.Slot
                                        processData = True
                                    Case HQFEffectType.Attribute
                                        If aModule.Attributes.ContainsKey(chkEffect.AffectingID) Then
                                            processData = True
                                        End If
                                End Select
                                If processData = True And (aModule.ModuleState And chkEffect.Status) = aModule.ModuleState Then
                                    fEffect = New FinalEffect
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    If chkEffect.AffectedType = HQFEffectType.Slot Then
                                        fEffect.AffectedID.Add(CInt(aModule.SlotType & aModule.SlotNo))
                                    Else
                                        fEffect.AffectedID = chkEffect.AffectedID
                                    End If
                                    fEffect.AffectedValue = aModule.Attributes(att)
                                    fEffect.StackNerf = chkEffect.StackNerf
                                    fEffect.Cause = aModule.Name
                                    fEffect.CalcType = chkEffect.CalcType
                                    If ssEffectsTable.ContainsKey(fEffect.AffectedAtt) = False Then
                                        fEffectList = New List(Of FinalEffect)
                                        ssEffectsTable.Add(fEffect.AffectedAtt, fEffectList)
                                    Else
                                        fEffectList = ssEffectsTable(fEffect.AffectedAtt)
                                    End If
                                    fEffectList.Add(fEffect)
                                End If
                            Next
                        End If
                    Next
                End If
            Next
        End If

        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = newShip.Attributes.Keys(attNo)
            If ssEffectsTable.ContainsKey(att) = True Then
                For Each fEffect In ssEffectsTable(att)
                    If ProcessFinalEffectForShip(newShip, fEffect) = True Then
                        Call ApplyFinalEffectToShip(newShip, fEffect, att)
                    End If
                Next
            End If
        Next

        Ship.MapShipAttributes(newShip)

        ' Copy the fittings from the old ship
        If newShip.HiSlots > 0 Then
            For slot As Integer = 1 To Math.Min(cShip.HiSlots, newShip.HiSlots)
                newShip.HiSlot(slot) = cShip.HiSlot(slot)
            Next
        End If
        If newShip.MidSlots > 0 Then
            For slot As Integer = 1 To Math.Min(cShip.MidSlots, newShip.MidSlots)
                newShip.MidSlot(slot) = cShip.MidSlot(slot)
            Next
        End If
        If newShip.LowSlots > 0 Then
            For slot As Integer = 1 To Math.Min(cShip.LowSlots, newShip.LowSlots)
                newShip.LowSlot(slot) = cShip.LowSlot(slot)
            Next
        End If
        If newShip.RigSlots > 0 Then
            For slot As Integer = 1 To Math.Min(cShip.RigSlots, newShip.RigSlots)
                newShip.RigSlot(slot) = cShip.RigSlot(slot)
            Next
        End If
        For Each cbidx As Integer In cShip.CargoBayItems.Keys
            newShip.CargoBayItems.Add(cbidx, cShip.CargoBayItems(cbidx))
        Next
        For Each dbidx As Integer In cShip.DroneBayItems.Keys
            newShip.DroneBayItems.Add(dbidx, cShip.DroneBayItems(dbidx))
        Next
        For Each sbidx As Integer In cShip.ShipBayItems.Keys
            newShip.ShipBayItems.Add(sbidx, cShip.ShipBayItems(sbidx))
        Next
        newShip.CargoBayUsed = cShip.CargoBayUsed
        newShip.DroneBayUsed = cShip.DroneBayUsed
        newShip.ShipBayUsed = cShip.ShipBayUsed
        For Each shipMod As ShipModule In cShip.FleetSlotCollection
            newShip.FleetSlotCollection.Add(shipMod.Clone)
        Next
        For Each shipMod As ShipModule In cShip.RemoteSlotCollection
            newShip.RemoteSlotCollection.Add(shipMod.Clone)
        Next
        For Each shipMod As ShipModule In cShip.EnviroSlotCollection
            newShip.EnviroSlotCollection.Add(shipMod.Clone)
        Next
        For Each shipMod As ShipModule In cShip.BoosterSlotCollection
            newShip.BoosterSlotCollection.Add(shipMod.Clone)
        Next
        If cShip.DamageProfile IsNot Nothing Then
            newShip.DamageProfile = cShip.DamageProfile
        Else
            newShip.DamageProfile = HQFDamageProfiles.ProfileList("<Omni-Damage>")
        End If
        Return newShip
    End Function
    Private Sub BuildExternalModules()
        ' Builds module information from WH, gang and fleet info so we can include it in results without using UI

        ' Build WH information
        Me.BaseShip.EnviroSlotCollection.Clear()
        ' Set the WH Class combo if it's not activated
        If Me.cWHEffect <> "" And Me.cWHLevel > 0 Then
            Dim modName As String = ""
            If Me.cWHEffect = "Red Giant" Then
                modName = Me.cWHEffect & " Beacon Class " & Me.cWHLevel.ToString
            Else
                If Me.cWHEffect.StartsWith("Incursion") Then
                    modName = Me.cWHEffect.Replace("-", "ship attributes effects")
                Else
                    modName = Me.cWHEffect & " Effect Beacon Class " & Me.cWHLevel
                End If
            End If
            Dim modID As Integer = ModuleLists.ModuleListName(modName)
            Dim eMod As ShipModule = ModuleLists.ModuleList(modID).Clone
            Me.BaseShip.EnviroSlotCollection.Add(eMod)
        End If

    End Sub
    Private Function CollectModules(ByVal newShip As Ship) As Ship
        newShip.SlotCollection.Clear()
        For Each remoteObject As Object In newShip.RemoteSlotCollection
            If TypeOf remoteObject Is ShipModule Then
                newShip.SlotCollection.Add(CType(remoteObject, ShipModule))
            Else
                Dim remoteDrones As DroneBayItem = CType(remoteObject, DroneBayItem)
                For drone As Integer = 1 To remoteDrones.Quantity
                    newShip.SlotCollection.Add(remoteDrones.DroneType)
                Next
            End If
        Next
        For Each fleetObject As Object In newShip.FleetSlotCollection
            If TypeOf fleetObject Is ShipModule Then
                newShip.SlotCollection.Add(CType(fleetObject, ShipModule))
            End If
        Next
        For Each enviroObject As Object In newShip.EnviroSlotCollection
            If TypeOf enviroObject Is ShipModule Then
                newShip.SlotCollection.Add(CType(enviroObject, ShipModule))
            End If
        Next
        For Each boosterObject As Object In newShip.BoosterSlotCollection
            If TypeOf boosterObject Is ShipModule Then
                newShip.SlotCollection.Add(CType(boosterObject, ShipModule))
            End If
        Next
        For slot As Integer = 1 To newShip.HiSlots
            If newShip.HiSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.HiSlot(slot))
            End If
        Next
        For slot As Integer = 1 To newShip.MidSlots
            If newShip.MidSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.MidSlot(slot))
                ' Recalculate Cap Booster calcs if reload time is included
                If Settings.HQFSettings.IncludeCapReloadTime = True And newShip.MidSlot(slot).DatabaseGroup = 76 Then
                    Dim cModule As ShipModule = newShip.MidSlot(slot)
                    If cModule.LoadedCharge IsNot Nothing Then
                        Dim reloadEffect As Double = 10 / (CInt(Int(cModule.Capacity / cModule.LoadedCharge.Volume)))
                        cModule.Attributes(73) += reloadEffect
                    End If
                End If
            End If
        Next
        For slot As Integer = 1 To newShip.LowSlots
            If newShip.LowSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.LowSlot(slot))
            End If
        Next
        For slot As Integer = 1 To newShip.RigSlots
            If newShip.RigSlot(slot) IsNot Nothing Then
                newShip.SlotCollection.Add(newShip.RigSlot(slot))
            End If
        Next
        ' Reset max gang links status
        newShip.Attributes(10063) = 1
        Return newShip
    End Function
    Private Sub ApplySkillEffectsToShip(ByRef newShip As Ship)
        Dim att As Integer
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = newShip.Attributes.Keys(attNo)
            If _skillEffectsTable.ContainsKey(att) = True Then
                For Each fEffect As FinalEffect In _skillEffectsTable(att)
                    If ProcessFinalEffectForShip(newShip, fEffect) = True Then
                        Call ApplyFinalEffectToShip(newShip, fEffect, att)
                    End If
                Next
            End If
        Next
        Ship.MapShipAttributes(newShip)
    End Sub
    Private Sub ApplySkillEffectsToModules(ByRef newShip As Ship)
        For Each aModule As ShipModule In newShip.SlotCollection
            Call ApplySkillEffectsToModule(aModule, False)
        Next
    End Sub
    Public Sub ApplySkillEffectsToModule(ByRef aModule As ShipModule, ByVal MapAttributes As Boolean)
        Dim att As Integer
        If aModule.ModuleState < 16 Then
            For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                att = aModule.Attributes.Keys(attNo)
                If _skillEffectsTable.ContainsKey(att) = True Then
                    For Each fEffect As FinalEffect In _skillEffectsTable(att)
                        If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                            Call ApplyFinalEffectToModule(aModule, fEffect, att)
                        End If
                    Next
                End If
            Next
            If aModule.LoadedCharge IsNot Nothing Then
                For attNo As Integer = 0 To aModule.LoadedCharge.Attributes.Keys.Count - 1
                    att = aModule.LoadedCharge.Attributes.Keys(attNo)
                    If _skillEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In _skillEffectsTable(att)
                            If ProcessFinalEffectForModule(aModule.LoadedCharge, fEffect) = True Then
                                Call ApplyFinalEffectToModule(aModule.LoadedCharge, fEffect, att)
                            End If
                        Next
                    End If
                Next
            End If
            If MapAttributes = True Then
                ShipModule.MapModuleAttributes(aModule)
            End If
        End If
    End Sub
    Private Sub ApplySkillEffectsToDrones(ByRef newShip As Ship)
        Dim aModule As New ShipModule
        Dim att As Integer
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            aModule = DBI.DroneType
            If aModule.ModuleState < 16 Then
                For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                    att = aModule.Attributes.Keys(attNo)
                    If _skillEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In _skillEffectsTable(att)
                            If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                                Call ApplyFinalEffectToModule(aModule, fEffect, att)
                            End If
                        Next
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub ApplyChargeEffectsToModules(ByRef newShip As Ship)
        Dim att As Integer
        For Each aModule As ShipModule In newShip.SlotCollection
            If aModule.ModuleState < 16 Then
                For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                    att = aModule.Attributes.Keys(attNo)
                    If _chargeEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In _chargeEffectsTable(att)
                            If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                                Call ApplyFinalEffectToModule(aModule, fEffect, att)
                            End If
                        Next
                    End If
                Next
                ShipModule.MapModuleAttributes(aModule)
            End If
        Next
    End Sub
    Private Sub ApplyChargeEffectsToShip(ByRef newShip As Ship)
        Dim att As Integer
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = newShip.Attributes.Keys(attNo)
            If _chargeEffectsTable.ContainsKey(att) = True Then
                For Each fEffect As FinalEffect In _chargeEffectsTable(att)
                    If ProcessFinalEffectForShip(newShip, fEffect) = True Then
                        Call ApplyFinalEffectToShip(newShip, fEffect, att)
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub ApplyStackingPenalties()
        Call PrioritiseEffects()
        Dim baseEffectList As List(Of FinalEffect)
        Dim finalEffectList As List(Of FinalEffect)
        Dim stackingGroupsP As New SortedList(Of Integer, SortedList(Of Integer, FinalEffect)) ' Positive Effect Stacking Groups
        Dim stackingGroupsN As New SortedList(Of Integer, SortedList(Of Integer, FinalEffect)) ' Negative Effect Stacking Groups
        Dim attOrder(,) As Double
        Dim att As Integer
        For attNumber As Integer = 0 To _moduleEffectsTable.Keys.Count - 1
            att = _moduleEffectsTable.Keys(attNumber)
            baseEffectList = _moduleEffectsTable(att)
            stackingGroupsP.Clear()
            stackingGroupsN.Clear()
            finalEffectList = New List(Of FinalEffect)
            For Each fEffect As FinalEffect In baseEffectList
                Select Case fEffect.StackNerf
                    Case 0
                        finalEffectList.Add(fEffect)
                    Case Else
                        If fEffect.AffectedValue >= 0 Then
                            If stackingGroupsP.ContainsKey(fEffect.StackNerf) = False Then
                                stackingGroupsP.Add(fEffect.StackNerf, New SortedList(Of Integer, FinalEffect))
                            End If
                            stackingGroupsP(fEffect.StackNerf).Add(stackingGroupsP(fEffect.StackNerf).Count, fEffect)
                        Else
                            If stackingGroupsN.ContainsKey(fEffect.StackNerf) = False Then
                                stackingGroupsN.Add(fEffect.StackNerf, New SortedList(Of Integer, FinalEffect))
                            End If
                            stackingGroupsN(fEffect.StackNerf).Add(stackingGroupsN(fEffect.StackNerf).Count, fEffect)
                        End If
                End Select
            Next
            For Each stackingGroup As SortedList(Of Integer, FinalEffect) In stackingGroupsP.Values
                If stackingGroup.Count > 0 Then
                    ReDim attOrder(stackingGroup.Count - 1, 1)
                    Dim sEffect As FinalEffect
                    For Each attNo As Integer In stackingGroup.Keys
                        sEffect = stackingGroup(attNo)
                        attOrder(attNo, 0) = attNo
                        attOrder(attNo, 1) = sEffect.AffectedValue
                    Next
                    ' Create a tag array ready to sort the effects
                    Dim tagArray(stackingGroup.Count - 1) As Integer
                    For a As Integer = 0 To stackingGroup.Count - 1
                        tagArray(a) = a
                    Next
                    ' Initialize the comparer and sort
                    Dim myComparer As New FittingEffectComparer(attOrder)
                    Array.Sort(tagArray, myComparer)
                    Array.Reverse(tagArray)
                    ' Go through the data and apply the stacking penalty
                    Dim idx As Integer
                    Dim penalty As Double
                    For i As Integer = 0 To tagArray.Length - 1
                        idx = tagArray(i)
                        sEffect = stackingGroup(idx)
                        penalty = Math.Exp(-(i ^ 2 / 7.1289))
                        Select Case sEffect.CalcType
                            Case EffectCalcType.Multiplier
                                sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                            Case Else
                                sEffect.AffectedValue = sEffect.AffectedValue * penalty
                        End Select
                        sEffect.Cause &= " (Stacking - " & (penalty * 100).ToString("N4") & "%)"
                        finalEffectList.Add(sEffect)
                    Next
                End If
            Next
            For Each stackingGroup As SortedList(Of Integer, FinalEffect) In stackingGroupsN.Values
                If stackingGroup.Count > 0 Then
                    ReDim attOrder(stackingGroup.Count - 1, 1)
                    Dim sEffect As FinalEffect
                    For Each attNo As Integer In stackingGroup.Keys
                        sEffect = stackingGroup(attNo)
                        attOrder(attNo, 0) = attNo
                        attOrder(attNo, 1) = sEffect.AffectedValue
                    Next
                    ' Create a tag array ready to sort the effects
                    Dim tagArray(stackingGroup.Count - 1) As Integer
                    For a As Integer = 0 To stackingGroup.Count - 1
                        tagArray(a) = a
                    Next
                    ' Initialize the comparer and sort
                    Dim myComparer As New FittingEffectComparer(attOrder)
                    Array.Sort(tagArray, myComparer)
                    ' Go through the data and apply the stacking penalty
                    Dim idx As Integer
                    Dim penalty As Double
                    For i As Integer = 0 To tagArray.Length - 1
                        idx = tagArray(i)
                        sEffect = stackingGroup(idx)
                        penalty = Math.Exp(-(i ^ 2 / 7.1289))
                        Select Case sEffect.CalcType
                            Case EffectCalcType.Multiplier
                                sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                            Case Else
                                sEffect.AffectedValue = sEffect.AffectedValue * penalty
                        End Select
                        sEffect.Cause &= " (Stacking - " & (penalty * 100).ToString("N4") & "%)"
                        finalEffectList.Add(sEffect)
                    Next
                End If
            Next
            _moduleEffectsTable(att) = finalEffectList
        Next
    End Sub
    Private Sub PrioritiseEffects()
        Dim baseEffectList As New List(Of FinalEffect)
        Dim HiPEffectList As New List(Of FinalEffect)
        Dim LowPEffectList As New List(Of FinalEffect)
        Dim finalEffectList As New List(Of FinalEffect)
        Dim att As Integer
        For attNumber As Integer = 0 To _moduleEffectsTable.Keys.Count - 1
            att = _moduleEffectsTable.Keys(attNumber)
            baseEffectList = _moduleEffectsTable(att)
            HiPEffectList.Clear() : LowPEffectList.Clear()
            For Each fEffect As FinalEffect In baseEffectList
                Select Case fEffect.CalcType
                    Case EffectCalcType.Addition
                        HiPEffectList.Add(fEffect)
                    Case Else
                        LowPEffectList.Add(fEffect)
                End Select
            Next
            finalEffectList = New List(Of FinalEffect)
            For Each fEffect As FinalEffect In HiPEffectList
                finalEffectList.Add(fEffect)
            Next
            For Each fEffect As FinalEffect In LowPEffectList
                finalEffectList.Add(fEffect)
            Next
            _moduleEffectsTable(att) = finalEffectList
        Next
    End Sub
    Private Sub ApplyModuleEffectsToCharges(ByRef newShip As Ship)
        Dim att As Integer
        For Each aModule As ShipModule In newShip.SlotCollection
            If aModule.LoadedCharge IsNot Nothing Then
                For attNo As Integer = 0 To aModule.LoadedCharge.Attributes.Keys.Count - 1
                    att = aModule.LoadedCharge.Attributes.Keys(attNo)
                    If _moduleEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In _moduleEffectsTable(att)
                            If ProcessFinalEffectForModule(aModule.LoadedCharge, fEffect) = True Then
                                Call ApplyFinalEffectToModule(aModule.LoadedCharge, fEffect, att)
                            End If
                        Next
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub ApplyModuleEffectsToModules(ByRef newShip As Ship)
        Dim att As Integer
        For Each aModule As ShipModule In newShip.SlotCollection
            For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                att = aModule.Attributes.Keys(attNo)
                If _moduleEffectsTable.ContainsKey(att) = True Then
                    For Each fEffect As FinalEffect In _moduleEffectsTable(att)
                        If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                            Call ApplyFinalEffectToModule(aModule, fEffect, att)
                        End If
                    Next
                End If
            Next
            ShipModule.MapModuleAttributes(aModule)
        Next
    End Sub
    Private Sub ApplyModuleEffectsToDrones(ByRef newShip As Ship)
        Dim aModule As ShipModule
        Dim att As Integer
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            aModule = DBI.DroneType
            For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                att = aModule.Attributes.Keys(attNo)
                If _moduleEffectsTable.ContainsKey(att) = True Then
                    For Each fEffect As FinalEffect In _moduleEffectsTable(att)
                        If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                            Call ApplyFinalEffectToModule(aModule, fEffect, att)
                        End If
                    Next
                End If
            Next
        Next
    End Sub
    Private Sub ApplyModuleEffectsToShip(ByRef newShip As Ship)
        Dim att As Integer
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = newShip.Attributes.Keys(attNo)
            If _moduleEffectsTable.ContainsKey(att) = True Then
                For Each fEffect As FinalEffect In _moduleEffectsTable(att)
                    If ProcessFinalEffectForShip(newShip, fEffect) = True Then
                        Call ApplyFinalEffectToShip(newShip, fEffect, att)
                    End If
                Next
            End If
        Next
    End Sub
    Public Sub CalculateDamageStatistics(ByRef newShip As Ship)
        Dim cModule As ShipModule
        Dim dmgMod As Double = 1
        Dim ROF As Double = 1
        newShip.Attributes(AttributeEnum.ShipFighterControl) = 0
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                cModule = DBI.DroneType
                newShip.Attributes(AttributeEnum.ShipFighterControl) += DBI.Quantity
                Select Case cModule.DatabaseGroup
                    Case ModuleEnum.GroupMiningDrones
                        cModule.Attributes(AttributeEnum.ModuleDroneOreMiningRate) = cModule.Attributes(AttributeEnum.ModuleMiningAmount) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                        newShip.Attributes(AttributeEnum.ShipOreMiningAmount) += cModule.Attributes(AttributeEnum.ModuleMiningAmount) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipDroneOreMiningAmount) += cModule.Attributes(AttributeEnum.ModuleMiningAmount) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipDroneOreMiningRate) += cModule.Attributes(AttributeEnum.ModuleDroneOreMiningRate) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipOreMiningRate) += cModule.Attributes(AttributeEnum.ModuleDroneOreMiningRate) * DBI.Quantity
                    Case ModuleEnum.GroupLogisticDrones
                        Dim repAmount As Double = 0
                        If cModule.Attributes.ContainsKey(AttributeEnum.ModuleShieldHPRepaired) = True Then
                            repAmount = cModule.Attributes(AttributeEnum.ModuleShieldHPRepaired)
                        Else
                            repAmount = cModule.Attributes(AttributeEnum.ModuleArmorHPRepaired)
                        End If
                        cModule.Attributes(AttributeEnum.ModuleTransferRate) = repAmount / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                        newShip.Attributes(AttributeEnum.ShipDroneTransferRate) += cModule.Attributes(AttributeEnum.ModuleTransferRate) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipTransferRate) += cModule.Attributes(AttributeEnum.ModuleTransferRate) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipDroneTransferAmount) += repAmount * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipTransferAmount) += repAmount * DBI.Quantity
                    Case Else
                        ' Not mining or logistic drone
                        If cModule.Attributes.ContainsKey(AttributeEnum.ModuleROF) = True Then
                            ROF = cModule.Attributes(AttributeEnum.ModuleROF)
                            dmgMod = cModule.Attributes(AttributeEnum.ModuleDamageMod)
                        ElseIf cModule.Attributes.ContainsKey(AttributeEnum.ModuleMissileROF) = True Then
                            ROF = cModule.Attributes(AttributeEnum.ModuleMissileROF)
                            dmgMod = cModule.Attributes(AttributeEnum.ModuleMissileDamageMod)
                        Else
                            dmgMod = 0
                            ROF = 1
                        End If
                        If cModule.LoadedCharge IsNot Nothing Then
                            cModule.Attributes(AttributeEnum.ModuleBaseDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseEMDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseExpDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseKinDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseThermDamage)
                            cModule.Attributes(AttributeEnum.ModuleEMDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseEMDamage) * dmgMod
                            cModule.Attributes(AttributeEnum.ModuleExpDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseExpDamage) * dmgMod
                            cModule.Attributes(AttributeEnum.ModuleKinDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseKinDamage) * dmgMod
                            cModule.Attributes(AttributeEnum.ModuleThermDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseThermDamage) * dmgMod
                            cModule.Attributes(AttributeEnum.ModuleVolleyDamage) = dmgMod * cModule.Attributes(AttributeEnum.ModuleBaseDamage)
                            cModule.Attributes(AttributeEnum.ModuleDPS) = cModule.Attributes(AttributeEnum.ModuleVolleyDamage) / ROF
                        Else
                            cModule.Attributes(AttributeEnum.ModuleBaseDamage) = 0
                            If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseEMDamage) Then
                                cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseEMDamage)
                                cModule.Attributes(AttributeEnum.ModuleEMDamage) = cModule.Attributes(AttributeEnum.ModuleBaseEMDamage) * dmgMod
                            Else
                                cModule.Attributes(AttributeEnum.ModuleEMDamage) = 0
                            End If
                            If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseExpDamage) Then
                                cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseExpDamage)
                                cModule.Attributes(AttributeEnum.ModuleExpDamage) = cModule.Attributes(AttributeEnum.ModuleBaseExpDamage) * dmgMod
                            Else
                                cModule.Attributes(AttributeEnum.ModuleExpDamage) = 0
                            End If
                            If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseKinDamage) Then
                                cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseKinDamage)
                                cModule.Attributes(AttributeEnum.ModuleKinDamage) = cModule.Attributes(AttributeEnum.ModuleBaseKinDamage) * dmgMod
                            Else
                                cModule.Attributes(AttributeEnum.ModuleKinDamage) = 0
                            End If
                            If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseThermDamage) Then
                                cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseThermDamage)
                                cModule.Attributes(AttributeEnum.ModuleThermDamage) = cModule.Attributes(AttributeEnum.ModuleBaseThermDamage) * dmgMod
                            Else
                                cModule.Attributes(AttributeEnum.ModuleThermDamage) = 0
                            End If
                            cModule.Attributes(AttributeEnum.ModuleVolleyDamage) = dmgMod * cModule.Attributes(AttributeEnum.ModuleBaseDamage)
                            cModule.Attributes(AttributeEnum.ModuleDPS) = cModule.Attributes(AttributeEnum.ModuleVolleyDamage) / ROF
                        End If
                        newShip.Attributes(AttributeEnum.ShipDroneVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipDroneDPS) += cModule.Attributes(AttributeEnum.ModuleDPS) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipDPS) += cModule.Attributes(AttributeEnum.ModuleDPS) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipEMDamage) += cModule.Attributes(AttributeEnum.ModuleEMDamage) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipExpDamage) += cModule.Attributes(AttributeEnum.ModuleExpDamage) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipKinDamage) += cModule.Attributes(AttributeEnum.ModuleKinDamage) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipThermDamage) += cModule.Attributes(AttributeEnum.ModuleThermDamage) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipEMDPS) += (cModule.Attributes(AttributeEnum.ModuleEMDamage) / ROF) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipExpDPS) += (cModule.Attributes(AttributeEnum.ModuleExpDamage) / ROF) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipKinDPS) += (cModule.Attributes(AttributeEnum.ModuleKinDamage) / ROF) * DBI.Quantity
                        newShip.Attributes(AttributeEnum.ShipThermDPS) += (cModule.Attributes(AttributeEnum.ModuleThermDamage) / ROF) * DBI.Quantity
                End Select
            End If
        Next
        For slot As Integer = 1 To newShip.HiSlots
            cModule = newShip.HiSlot(slot)
            If cModule IsNot Nothing Then
                If (cModule.ModuleState Or 12) = 12 Then
                    Select Case cModule.MarketGroup
                        Case ModuleEnum.MarketgroupMiningLasers, ModuleEnum.MarketgroupStripMiners
                            newShip.Attributes(AttributeEnum.ShipTurretOreMiningAmount) += cModule.Attributes(AttributeEnum.ModuleMiningAmount)
                            newShip.Attributes(AttributeEnum.ShipOreMiningAmount) += cModule.Attributes(AttributeEnum.ModuleMiningAmount)
                            cModule.Attributes(AttributeEnum.ModuleTurretOreMiningRate) = cModule.Attributes(AttributeEnum.ModuleMiningAmount) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                            newShip.Attributes(AttributeEnum.ShipTurretOreMiningRate) += cModule.Attributes(AttributeEnum.ModuleTurretOreMiningRate)
                            newShip.Attributes(AttributeEnum.ShipOreMiningRate) += cModule.Attributes(AttributeEnum.ModuleTurretOreMiningRate)
                        Case ModuleEnum.MarketgroupIceHarvesters
                            newShip.Attributes(AttributeEnum.ShipTurretIceMiningAmount) += cModule.Attributes(AttributeEnum.ModuleMiningAmount)
                            newShip.Attributes(AttributeEnum.ShipIceMiningAmount) += cModule.Attributes(AttributeEnum.ModuleMiningAmount)
                            cModule.Attributes(AttributeEnum.ModuleTurretIceMiningRate) = cModule.Attributes(AttributeEnum.ModuleMiningAmount) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                            newShip.Attributes(AttributeEnum.ShipTurretIceMiningRate) += cModule.Attributes(AttributeEnum.ModuleTurretIceMiningRate)
                            newShip.Attributes(AttributeEnum.ShipIceMiningRate) += cModule.Attributes(AttributeEnum.ModuleTurretIceMiningRate)
                        Case ModuleEnum.MarketgroupGasHarvesters
                            newShip.Attributes(AttributeEnum.ShipGasMiningAmount) += cModule.Attributes(AttributeEnum.ModuleMiningAmount)
                            cModule.Attributes(AttributeEnum.ModuleTurretGasMiningRate) = cModule.Attributes(AttributeEnum.ModuleMiningAmount) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                            newShip.Attributes(AttributeEnum.ShipGasMiningRate) += cModule.Attributes(AttributeEnum.ModuleTurretGasMiningRate)
                        Case Else
                            If cModule.IsTurret Or cModule.IsLauncher Then
                                If cModule.LoadedCharge IsNot Nothing Then
                                    cModule.Attributes(AttributeEnum.ModuleLoadedCharge) = CDbl(cModule.LoadedCharge.ID)
                                    ' If LoadedCharge doesn't have damage attributes set damage to 0 (required for orbital bombardment and festival ammo)
                                    Dim noDamageAmmo As Boolean = False
                                    If cModule.LoadedCharge.Attributes.ContainsKey(AttributeEnum.ModuleBaseEMDamage) And cModule.LoadedCharge.Attributes.ContainsKey(AttributeEnum.ModuleBaseExpDamage) And cModule.LoadedCharge.Attributes.ContainsKey(AttributeEnum.ModuleBaseKinDamage) And cModule.LoadedCharge.Attributes.ContainsKey(AttributeEnum.ModuleBaseThermDamage) Then
                                        cModule.Attributes(AttributeEnum.ModuleBaseDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseEMDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseExpDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseKinDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseThermDamage)
                                    Else
                                        noDamageAmmo = True
                                        cModule.Attributes(AttributeEnum.ModuleBaseDamage) = 0
                                    End If
                                    If cModule.IsTurret = True Then
                                        ' Adjust for reload time if required
                                        Dim reloadEffect As Double = 0
                                        If HQF.Settings.HQFSettings.IncludeAmmoReloadTime = True Then
                                            If cModule.DatabaseGroup <> ModuleEnum.GroupEnergyTurrets Then
                                                If cModule.DatabaseGroup = ModuleEnum.GroupHybridTurrets Then
                                                    reloadEffect = 5 / (cModule.Capacity / cModule.LoadedCharge.Volume)
                                                Else
                                                    reloadEffect = 10 / (cModule.Capacity / cModule.LoadedCharge.Volume)
                                                End If
                                            End If
                                        End If
                                        Select Case cModule.DatabaseGroup
                                            Case ModuleEnum.GroupEnergyTurrets
                                                dmgMod = cModule.Attributes(AttributeEnum.ModuleEnergyDmgMod)
                                                ROF = cModule.Attributes(AttributeEnum.ModuleEnergyROF) + reloadEffect
                                            Case ModuleEnum.GroupHybridTurrets
                                                dmgMod = cModule.Attributes(AttributeEnum.ModuleHybridDmgMod)
                                                ROF = cModule.Attributes(AttributeEnum.ModuleHybridROF) + reloadEffect
                                            Case ModuleEnum.GroupProjectileTurrets
                                                dmgMod = cModule.Attributes(AttributeEnum.ModuleProjectileDmgMod)
                                                ROF = cModule.Attributes(AttributeEnum.ModuleProjectileROF) + reloadEffect
                                        End Select
                                        cModule.Attributes(AttributeEnum.ModuleVolleyDamage) = dmgMod * cModule.Attributes(AttributeEnum.ModuleBaseDamage)
                                        cModule.Attributes(AttributeEnum.ModuleDPS) = cModule.Attributes(AttributeEnum.ModuleVolleyDamage) / ROF
                                        newShip.Attributes(AttributeEnum.ShipTurretVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                        newShip.Attributes(AttributeEnum.ShipTurretDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                        newShip.Attributes(AttributeEnum.ShipVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                        newShip.Attributes(AttributeEnum.ShipDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                    Else
                                        ' Adjust for reload time if required
                                        Dim reloadEffect As Double = 0
                                        If HQF.Settings.HQFSettings.IncludeAmmoReloadTime = True Then
                                            reloadEffect = 10 / (cModule.Capacity / cModule.LoadedCharge.Volume)
                                        End If
                                        dmgMod = 1
                                        ROF = cModule.Attributes(AttributeEnum.ModuleROF) + reloadEffect
                                        cModule.Attributes(AttributeEnum.ModuleVolleyDamage) = dmgMod * cModule.Attributes(AttributeEnum.ModuleBaseDamage)
                                        cModule.Attributes(AttributeEnum.ModuleDPS) = cModule.Attributes(AttributeEnum.ModuleVolleyDamage) / ROF
                                        newShip.Attributes(AttributeEnum.ShipMissileVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                        newShip.Attributes(AttributeEnum.ShipMissileDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                        newShip.Attributes(AttributeEnum.ShipVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                        newShip.Attributes(AttributeEnum.ShipDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                        If cModule.LoadedCharge IsNot Nothing Then
                                            cModule.Attributes(AttributeEnum.ModuleOptimalRange) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleMaxVelocity) * cModule.LoadedCharge.Attributes(AttributeEnum.ModuleMaxFlightTime) * HQF.Settings.HQFSettings.MissileRangeConstant
                                        End If
                                    End If
                                    If noDamageAmmo = False Then
                                        cModule.Attributes(AttributeEnum.ModuleEMDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseEMDamage) * dmgMod
                                        cModule.Attributes(AttributeEnum.ModuleExpDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseExpDamage) * dmgMod
                                        cModule.Attributes(AttributeEnum.ModuleKinDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseKinDamage) * dmgMod
                                        cModule.Attributes(AttributeEnum.ModuleThermDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseThermDamage) * dmgMod
                                    End If
                                    newShip.Attributes(AttributeEnum.ShipEMDamage) += cModule.Attributes(AttributeEnum.ModuleEMDamage)
                                    newShip.Attributes(AttributeEnum.ShipExpDamage) += cModule.Attributes(AttributeEnum.ModuleExpDamage)
                                    newShip.Attributes(AttributeEnum.ShipKinDamage) += cModule.Attributes(AttributeEnum.ModuleKinDamage)
                                    newShip.Attributes(AttributeEnum.ShipThermDamage) += cModule.Attributes(AttributeEnum.ModuleThermDamage)
                                    newShip.Attributes(AttributeEnum.ShipEMDPS) += (cModule.Attributes(AttributeEnum.ModuleEMDamage) / ROF)
                                    newShip.Attributes(AttributeEnum.ShipExpDPS) += (cModule.Attributes(AttributeEnum.ModuleExpDamage) / ROF)
                                    newShip.Attributes(AttributeEnum.ShipKinDPS) += (cModule.Attributes(AttributeEnum.ModuleKinDamage) / ROF)
                                    newShip.Attributes(AttributeEnum.ShipThermDPS) += (cModule.Attributes(AttributeEnum.ModuleThermDamage) / ROF)
                                End If
                            Else
                                Select Case cModule.DatabaseGroup
                                    Case ModuleEnum.GroupSmartbombs
                                        ' Do smartbomb code
                                        ROF = cModule.Attributes(AttributeEnum.ModuleActivationTime)
                                        cModule.Attributes(AttributeEnum.ModuleBaseDamage) = 0
                                        If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseEMDamage) Then
                                            cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseEMDamage)
                                            cModule.Attributes(AttributeEnum.ModuleEMDamage) = cModule.Attributes(AttributeEnum.ModuleBaseEMDamage)
                                        Else
                                            cModule.Attributes(AttributeEnum.ModuleEMDamage) = 0
                                        End If
                                        If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseExpDamage) Then
                                            cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseExpDamage)
                                            cModule.Attributes(AttributeEnum.ModuleExpDamage) = cModule.Attributes(AttributeEnum.ModuleBaseExpDamage)
                                        Else
                                            cModule.Attributes(AttributeEnum.ModuleExpDamage) = 0
                                        End If
                                        If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseKinDamage) Then
                                            cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseKinDamage)
                                            cModule.Attributes(AttributeEnum.ModuleKinDamage) = cModule.Attributes(AttributeEnum.ModuleBaseKinDamage)
                                        Else
                                            cModule.Attributes(AttributeEnum.ModuleKinDamage) = 0
                                        End If
                                        If cModule.Attributes.ContainsKey(AttributeEnum.ModuleBaseThermDamage) Then
                                            cModule.Attributes(AttributeEnum.ModuleBaseDamage) += cModule.Attributes(AttributeEnum.ModuleBaseThermDamage)
                                            cModule.Attributes(AttributeEnum.ModuleThermDamage) = cModule.Attributes(AttributeEnum.ModuleBaseThermDamage)
                                        Else
                                            cModule.Attributes(AttributeEnum.ModuleThermDamage) = 0
                                        End If
                                        cModule.Attributes(AttributeEnum.ModuleVolleyDamage) = cModule.Attributes(AttributeEnum.ModuleBaseDamage)
                                        cModule.Attributes(AttributeEnum.ModuleDPS) = cModule.Attributes(AttributeEnum.ModuleVolleyDamage) / ROF
                                        newShip.Attributes(AttributeEnum.ShipSmartbombVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                        newShip.Attributes(AttributeEnum.ShipSmartbombDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                        newShip.Attributes(AttributeEnum.ShipVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                        newShip.Attributes(AttributeEnum.ShipDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                        newShip.Attributes(AttributeEnum.ShipEMDamage) += cModule.Attributes(AttributeEnum.ModuleEMDamage)
                                        newShip.Attributes(AttributeEnum.ShipExpDamage) += cModule.Attributes(AttributeEnum.ModuleExpDamage)
                                        newShip.Attributes(AttributeEnum.ShipKinDamage) += cModule.Attributes(AttributeEnum.ModuleKinDamage)
                                        newShip.Attributes(AttributeEnum.ShipThermDamage) += cModule.Attributes(AttributeEnum.ModuleThermDamage)
                                        newShip.Attributes(AttributeEnum.ShipEMDPS) += (cModule.Attributes(AttributeEnum.ModuleEMDamage) / ROF)
                                        newShip.Attributes(AttributeEnum.ShipExpDPS) += (cModule.Attributes(AttributeEnum.ModuleExpDamage) / ROF)
                                        newShip.Attributes(AttributeEnum.ShipKinDPS) += (cModule.Attributes(AttributeEnum.ModuleKinDamage) / ROF)
                                        newShip.Attributes(AttributeEnum.ShipThermDPS) += (cModule.Attributes(AttributeEnum.ModuleThermDamage) / ROF)
                                    Case ModuleEnum.GroupBombLaunchers
                                        ' Do Bomb Launcher Code
                                        If cModule.LoadedCharge IsNot Nothing Then
                                            dmgMod = 1
                                            ' No ROF adjustment for reload time because reload happens during reactivation delay
                                            ROF = cModule.Attributes(AttributeEnum.ModuleROF) + cModule.Attributes(AttributeEnum.ModuleReactivationDelay)
                                            cModule.Attributes(AttributeEnum.ModuleBaseDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseEMDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseExpDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseKinDamage) + cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseThermDamage)
                                            cModule.Attributes(AttributeEnum.ModuleVolleyDamage) = dmgMod * cModule.Attributes(AttributeEnum.ModuleBaseDamage)
                                            cModule.Attributes(AttributeEnum.ModuleDPS) = cModule.Attributes(AttributeEnum.ModuleVolleyDamage) / ROF
                                            newShip.Attributes(AttributeEnum.ShipMissileVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                            newShip.Attributes(AttributeEnum.ShipMissileDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                            newShip.Attributes(AttributeEnum.ShipVolleyDamage) += cModule.Attributes(AttributeEnum.ModuleVolleyDamage)
                                            newShip.Attributes(AttributeEnum.ShipDPS) += cModule.Attributes(AttributeEnum.ModuleDPS)
                                            cModule.Attributes(AttributeEnum.ModuleOptimalRange) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleMaxVelocity) * cModule.LoadedCharge.Attributes(AttributeEnum.ModuleMaxFlightTime) * HQF.Settings.HQFSettings.MissileRangeConstant
                                            cModule.Attributes(AttributeEnum.ModuleEMDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseEMDamage) * dmgMod
                                            cModule.Attributes(AttributeEnum.ModuleExpDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseExpDamage) * dmgMod
                                            cModule.Attributes(AttributeEnum.ModuleKinDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseKinDamage) * dmgMod
                                            cModule.Attributes(AttributeEnum.ModuleThermDamage) = cModule.LoadedCharge.Attributes(AttributeEnum.ModuleBaseThermDamage) * dmgMod
                                            newShip.Attributes(AttributeEnum.ShipEMDamage) += cModule.Attributes(AttributeEnum.ModuleEMDamage)
                                            newShip.Attributes(AttributeEnum.ShipExpDamage) += cModule.Attributes(AttributeEnum.ModuleExpDamage)
                                            newShip.Attributes(AttributeEnum.ShipKinDamage) += cModule.Attributes(AttributeEnum.ModuleKinDamage)
                                            newShip.Attributes(AttributeEnum.ShipThermDamage) += cModule.Attributes(AttributeEnum.ModuleThermDamage)
                                            newShip.Attributes(AttributeEnum.ShipEMDPS) += (cModule.Attributes(AttributeEnum.ModuleEMDamage) / ROF)
                                            newShip.Attributes(AttributeEnum.ShipExpDPS) += (cModule.Attributes(AttributeEnum.ModuleExpDamage) / ROF)
                                            newShip.Attributes(AttributeEnum.ShipKinDPS) += (cModule.Attributes(AttributeEnum.ModuleKinDamage) / ROF)
                                            newShip.Attributes(AttributeEnum.ShipThermDPS) += (cModule.Attributes(AttributeEnum.ModuleThermDamage) / ROF)
                                        End If
                                    Case ModuleEnum.GroupShieldTransporters, ModuleEnum.GroupRemoteArmorRepairers, ModuleEnum.GroupRemoteHullRepairers
                                        Dim repAmount As Double = 0
                                        If cModule.Attributes.ContainsKey(AttributeEnum.ModuleShieldHPRepaired) = True Then
                                            repAmount = cModule.Attributes(AttributeEnum.ModuleShieldHPRepaired)
                                        ElseIf cModule.Attributes.ContainsKey(AttributeEnum.ModuleArmorHPRepaired) = True Then
                                            repAmount = cModule.Attributes(AttributeEnum.ModuleArmorHPRepaired)
                                        Else
                                            repAmount = cModule.Attributes(AttributeEnum.ModuleHullHPRepaired)
                                        End If
                                        cModule.Attributes(AttributeEnum.ModuleTransferRate) = repAmount / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                                        newShip.Attributes(AttributeEnum.ShipModuleTransferRate) += cModule.Attributes(AttributeEnum.ModuleTransferRate)
                                        newShip.Attributes(AttributeEnum.ShipTransferRate) += cModule.Attributes(AttributeEnum.ModuleTransferRate)
                                        newShip.Attributes(AttributeEnum.ShipModuleTransferAmount) += repAmount
                                        newShip.Attributes(AttributeEnum.ShipTransferAmount) += repAmount
                                End Select
                            End If
                    End Select
                End If
            End If
        Next
    End Sub
    Public Function CalculateDamageStatsForDefenceProfile(ByRef newShip As Ship) As HQFDefenceProfileResults

        Dim dpr As New HQFDefenceProfileResults
        Dim dp As HQFDefenceProfile = HQFDefenceProfiles.ProfileList(Me.DefenceProfileName)

        If dp IsNot Nothing Then
            Dim SEM As Double = newShip.Attributes(AttributeEnum.ShipEMDPS) * (1 - (dp.SEM / 100))
            Dim SEx As Double = newShip.Attributes(AttributeEnum.ShipExpDPS) * (1 - (dp.SExplosive / 100))
            Dim SKi As Double = newShip.Attributes(AttributeEnum.ShipKinDPS) * (1 - (dp.SKinetic / 100))
            Dim STh As Double = newShip.Attributes(AttributeEnum.ShipThermDPS) * (1 - (dp.SThermal / 100))
            Dim ST As Double = SEM + SEx + SKi + STh
            dpr.ShieldDPS = ST

            Dim AEM As Double = newShip.Attributes(AttributeEnum.ShipEMDPS) * (1 - (dp.AEM / 100))
            Dim AEx As Double = newShip.Attributes(AttributeEnum.ShipExpDPS) * (1 - (dp.AExplosive / 100))
            Dim AKi As Double = newShip.Attributes(AttributeEnum.ShipKinDPS) * (1 - (dp.AKinetic / 100))
            Dim ATh As Double = newShip.Attributes(AttributeEnum.ShipThermDPS) * (1 - (dp.AThermal / 100))
            Dim AT As Double = AEM + AEx + AKi + ATh
            dpr.ArmorDPS = AT

            Dim HEM As Double = newShip.Attributes(AttributeEnum.ShipEMDPS) * (1 - (dp.HEM / 100))
            Dim HEx As Double = newShip.Attributes(AttributeEnum.ShipExpDPS) * (1 - (dp.HExplosive / 100))
            Dim HKi As Double = newShip.Attributes(AttributeEnum.ShipKinDPS) * (1 - (dp.HKinetic / 100))
            Dim HTh As Double = newShip.Attributes(AttributeEnum.ShipThermDPS) * (1 - (dp.HThermal / 100))
            Dim HT As Double = HEM + HEx + HKi + HTh
            dpr.HullDPS = HT
        End If

        Return dpr

    End Function
    Public Sub CalculateDefenceStatistics(ByRef newShip As Ship)
        Dim sRP, sRA, aR, hR As Double
        For Each cModule As ShipModule In newShip.SlotCollection
            ' Calculate shield boosting
            If (cModule.DatabaseGroup = ModuleEnum.GroupShieldBoosters Or cModule.DatabaseGroup = ModuleEnum.GroupFueledShieldBoosters) And (cModule.ModuleState And 12) = cModule.ModuleState Then
                sRA = sRA + cModule.Attributes(AttributeEnum.ModuleShieldHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
            End If
            ' Calculate remote shield boosting
            If cModule.DatabaseGroup = ModuleEnum.GroupShieldTransporters And (cModule.ModuleState And 16) = cModule.ModuleState Then
                sRA = sRA + cModule.Attributes(AttributeEnum.ModuleShieldHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
            End If
            ' Calculate shield maintenance drones
            If cModule.DatabaseGroup = ModuleEnum.GroupLogisticDrones And (cModule.ModuleState And 16) = cModule.ModuleState Then
                If cModule.Attributes.ContainsKey(AttributeEnum.ModuleShieldHPRepaired) Then
                    sRA = sRA + cModule.Attributes(AttributeEnum.ModuleShieldHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                End If
            End If
            ' Calculate armor repairing
            If (cModule.DatabaseGroup = ModuleEnum.GroupArmorRepairers Or cModule.DatabaseGroup = ModuleEnum.GroupFueledArmorRepairers) And (cModule.ModuleState And 12) = cModule.ModuleState Then
                aR = aR + cModule.Attributes(AttributeEnum.ModuleArmorHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
            End If
            ' Calculate remote armor repairing
            If cModule.DatabaseGroup = ModuleEnum.GroupRemoteArmorRepairers And (cModule.ModuleState And 16) = cModule.ModuleState Then
                aR = aR + cModule.Attributes(AttributeEnum.ModuleArmorHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
            End If
            ' Calculate armor maintenance drones
            If cModule.DatabaseGroup = ModuleEnum.GroupLogisticDrones And (cModule.ModuleState And 16) = cModule.ModuleState Then
                If cModule.Attributes.ContainsKey(AttributeEnum.ModuleArmorHPRepaired) Then
                    aR = aR + cModule.Attributes(AttributeEnum.ModuleArmorHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
                End If
            End If
            ' Calculate hull repairing
            If cModule.DatabaseGroup = ModuleEnum.GroupHullRepairers And (cModule.ModuleState And 12) = cModule.ModuleState Then
                hR = hR + cModule.Attributes(AttributeEnum.ModuleHullHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
            End If
            ' Calculate remote hull repairing
            If cModule.DatabaseGroup = ModuleEnum.GroupRemoteHullRepairers And (cModule.ModuleState And 16) = cModule.ModuleState Then
                hR = hR + cModule.Attributes(AttributeEnum.ModuleHullHPRepaired) / cModule.Attributes(AttributeEnum.ModuleActivationTime)
            End If
        Next
        sRP = (newShip.ShieldCapacity / newShip.ShieldRecharge * HQF.Settings.HQFSettings.ShieldRechargeConstant)
        ' Calculate the actual tanking ability
        Dim sTA As Double = sRA / ((newShip.DamageProfileEM * (1 - newShip.ShieldEMResist / 100)) + (newShip.DamageProfileEx * (1 - newShip.ShieldExResist / 100)) + (newShip.DamageProfileKi * (1 - newShip.ShieldKiResist / 100)) + (newShip.DamageProfileTh * (1 - newShip.ShieldThResist / 100)))
        Dim sTP As Double = sRP / ((newShip.DamageProfileEM * (1 - newShip.ShieldEMResist / 100)) + (newShip.DamageProfileEx * (1 - newShip.ShieldExResist / 100)) + (newShip.DamageProfileKi * (1 - newShip.ShieldKiResist / 100)) + (newShip.DamageProfileTh * (1 - newShip.ShieldThResist / 100)))
        Dim aT As Double = aR / ((newShip.DamageProfileEM * (1 - newShip.ArmorEMResist / 100)) + (newShip.DamageProfileEx * (1 - newShip.ArmorExResist / 100)) + (newShip.DamageProfileKi * (1 - newShip.ArmorKiResist / 100)) + (newShip.DamageProfileTh * (1 - newShip.ArmorThResist / 100)))
        Dim hT As Double = hR / ((newShip.DamageProfileEM * (1 - newShip.StructureEMResist / 100)) + (newShip.DamageProfileEx * (1 - newShip.StructureExResist / 100)) + (newShip.DamageProfileKi * (1 - newShip.StructureKiResist / 100)) + (newShip.DamageProfileTh * (1 - newShip.StructureThResist / 100)))
        newShip.Attributes(AttributeEnum.ShipShieldTankActive) = sTA
        newShip.Attributes(AttributeEnum.ShipArmorTank) = aT
        newShip.Attributes(AttributeEnum.ShipHullTank) = hT
        newShip.Attributes(AttributeEnum.ShipTankMax) = Math.Max(sTA + sTP, sTA + aT + hT)
        newShip.Attributes(AttributeEnum.ShipShieldTankPassive) = sTP
        newShip.Attributes(AttributeEnum.ShipShieldRepair) = sRA + sRP
        newShip.Attributes(AttributeEnum.ShipArmorRepair) = aR
        newShip.Attributes(AttributeEnum.ShipHullRepair) = hR
        newShip.Attributes(AttributeEnum.ShipRepairTotal) = sRA + sRP + aR + hR
    End Sub
#End Region

#Region "Common Private Supporting Fitting Routines"

    Private Function ProcessFinalEffectForShip(ByVal NewShip As Ship, ByVal FEffect As FinalEffect) As Boolean
        Select Case FEffect.AffectedType
            Case HQFEffectType.All
                Return True
            Case HQFEffectType.Item
                If FEffect.AffectedID.Contains(NewShip.ID) Then
                    Return True
                End If
            Case HQFEffectType.Group
                If FEffect.AffectedID.Contains(NewShip.DatabaseGroup) Then
                    Return True
                End If
            Case HQFEffectType.Category
                If FEffect.AffectedID.Contains(NewShip.DatabaseCategory) Then
                    Return True
                End If
            Case HQFEffectType.MarketGroup
                If FEffect.AffectedID.Contains(NewShip.MarketGroup) Then
                    Return True
                End If
            Case HQFEffectType.Skill
                If NewShip.RequiredSkills.ContainsKey(SkillFunctions.SkillIDToName(FEffect.AffectedID(0))) Then
                    Return True
                End If
            Case HQFEffectType.Attribute
                If NewShip.Attributes.ContainsKey(FEffect.AffectedID(0)) Then
                    Return True
                End If
        End Select
        Return False
    End Function

    Private Function ProcessFinalEffectForModule(ByVal NewModule As ShipModule, ByVal FEffect As FinalEffect) As Boolean
        Select Case FEffect.AffectedType
            Case HQFEffectType.All
                Return True
            Case HQFEffectType.Item
                If FEffect.AffectedID.Contains(NewModule.ID) Then
                    Return True
                End If
            Case HQFEffectType.Group
                If NewModule.ModuleState = ModuleStates.Gang And FEffect.AffectedID.Contains(-CInt(NewModule.DatabaseGroup)) Then
                    Return True
                End If
                If FEffect.AffectedID.Contains(NewModule.DatabaseGroup) Then
                    Return True
                End If
            Case HQFEffectType.Category
                If FEffect.AffectedID.Contains(NewModule.DatabaseCategory) Then
                    Return True
                End If
            Case HQFEffectType.MarketGroup
                If FEffect.AffectedID.Contains(NewModule.MarketGroup) Then
                    Return True
                End If
            Case HQFEffectType.Skill
                If NewModule.RequiredSkills.ContainsKey(SkillFunctions.SkillIDToName(FEffect.AffectedID(0))) Then
                    Return True
                End If
            Case HQFEffectType.Slot
                If FEffect.AffectedID.Contains(CInt(NewModule.SlotType & NewModule.SlotNo)) Then
                    Return True
                End If
            Case HQFEffectType.Attribute
                If NewModule.Attributes.ContainsKey(FEffect.AffectedID(0)) Then
                    Return True
                End If
        End Select
        Return False
    End Function

    Private Sub ApplyFinalEffectToShip(ByVal NewShip As Ship, ByVal FEffect As FinalEffect, ByVal Att As Integer)
        Dim log As String = Attributes.AttributeQuickList(Att).ToString & "# " & FEffect.Cause
        If NewShip.Name = FEffect.Cause Then
            log &= " (Overloading)"
        End If
        Dim oldAtt As String = NewShip.Attributes(Att).ToString()
        log &= "# " & oldAtt
        Select Case FEffect.CalcType
            Case EffectCalcType.Percentage
                NewShip.Attributes(Att) = NewShip.Attributes(Att) * ((100 + FEffect.AffectedValue) / 100.0)
            Case EffectCalcType.Addition
                NewShip.Attributes(Att) = NewShip.Attributes(Att) + FEffect.AffectedValue
            Case EffectCalcType.Difference ' Used for resistances
                If FEffect.AffectedValue <= 0 Then
                    NewShip.Attributes(Att) = ((100 - NewShip.Attributes(Att)) * (-FEffect.AffectedValue / 100)) + NewShip.Attributes(Att)
                Else
                    NewShip.Attributes(Att) = (NewShip.Attributes(Att) * (-FEffect.AffectedValue / 100)) + NewShip.Attributes(Att)
                End If
            Case EffectCalcType.Velocity
                NewShip.Attributes(Att) = CDbl(NewShip.Attributes(Att)) + (CDbl(NewShip.Attributes(Att)) * (CDbl(NewShip.Attributes(10010)) / CDbl(NewShip.Attributes(10002)) * (FEffect.AffectedValue / 100)))
            Case EffectCalcType.Absolute
                NewShip.Attributes(Att) = FEffect.AffectedValue
            Case EffectCalcType.Multiplier
                NewShip.Attributes(Att) = NewShip.Attributes(Att) * FEffect.AffectedValue
            Case EffectCalcType.AddPositive
                If FEffect.AffectedValue > 0 Then
                    NewShip.Attributes(Att) = NewShip.Attributes(Att) + FEffect.AffectedValue
                End If
            Case EffectCalcType.AddNegative
                If FEffect.AffectedValue < 0 Then
                    NewShip.Attributes(Att) = NewShip.Attributes(Att) + FEffect.AffectedValue
                End If
            Case EffectCalcType.Subtraction
                NewShip.Attributes(Att) = NewShip.Attributes(Att) - FEffect.AffectedValue
            Case EffectCalcType.CloakedVelocity
                NewShip.Attributes(Att) = -100 + ((100 + NewShip.Attributes(Att)) * (FEffect.AffectedValue / 100))
            Case EffectCalcType.SkillLevel
                NewShip.Attributes(Att) = FEffect.AffectedValue
            Case EffectCalcType.SkillLevelxAtt
                NewShip.Attributes(Att) = FEffect.AffectedValue
            Case EffectCalcType.AbsoluteMax
                NewShip.Attributes(Att) = Math.Max(FEffect.AffectedValue, NewShip.Attributes(Att))
            Case EffectCalcType.AbsoluteMin
                NewShip.Attributes(Att) = Math.Min(FEffect.AffectedValue, NewShip.Attributes(Att))
            Case EffectCalcType.CapBoosters
                NewShip.Attributes(Att) = Math.Min(NewShip.Attributes(Att) - FEffect.AffectedValue, 0)
        End Select
        ' Use only 2 decimal places of precision for PG and CPU output
        If Att = AttributeEnum.ShipPowergridOutput Or Att = AttributeEnum.ShipCpuOutput Then
            NewShip.Attributes(Att) = Math.Round(NewShip.Attributes(Att), 2, MidpointRounding.AwayFromZero)
        End If
        log &= "# " & NewShip.Attributes(Att).ToString
        If oldAtt <> NewShip.Attributes(Att).ToString Then
            NewShip.AuditLog.Add(log)
        End If
    End Sub

    Private Sub ApplyFinalEffectToModule(ByVal NewModule As ShipModule, ByVal FEffect As FinalEffect, ByVal Att As Integer)
        Dim log As String = Attributes.AttributeQuickList(Att).ToString & ": " & FEffect.Cause
        If NewModule.Name = FEffect.Cause Then
            log &= " (Overloading)"
        End If
        Dim oldAtt As String = NewModule.Attributes(Att).ToString()
        log &= ": " & oldAtt
        Select Case FEffect.CalcType
            Case EffectCalcType.Percentage
                NewModule.Attributes(Att) = NewModule.Attributes(Att) * ((100 + FEffect.AffectedValue) / 100.0)
            Case EffectCalcType.Addition
                NewModule.Attributes(Att) = NewModule.Attributes(Att) + FEffect.AffectedValue
            Case EffectCalcType.Difference  ' Used for resistances
                If FEffect.AffectedValue <= 0 Then
                    NewModule.Attributes(Att) = ((100 - NewModule.Attributes(Att)) * (-FEffect.AffectedValue / 100)) + NewModule.Attributes(Att)
                Else
                    NewModule.Attributes(Att) = (NewModule.Attributes(Att) * (-FEffect.AffectedValue / 100)) + NewModule.Attributes(Att)
                End If
            Case EffectCalcType.Velocity
                NewModule.Attributes(Att) = CDbl(NewModule.Attributes(Att)) + (CDbl(NewModule.Attributes(Att)) * (CDbl(NewModule.Attributes(10010)) / CDbl(NewModule.Attributes(10002)) * (FEffect.AffectedValue / 100)))
            Case EffectCalcType.Absolute
                NewModule.Attributes(Att) = FEffect.AffectedValue
            Case EffectCalcType.Multiplier
                NewModule.Attributes(Att) = NewModule.Attributes(Att) * FEffect.AffectedValue
            Case EffectCalcType.AddPositive
                If FEffect.AffectedValue > 0 Then
                    NewModule.Attributes(Att) = NewModule.Attributes(Att) + FEffect.AffectedValue
                End If
            Case EffectCalcType.AddNegative
                If FEffect.AffectedValue < 0 Then
                    NewModule.Attributes(Att) = NewModule.Attributes(Att) + FEffect.AffectedValue
                End If
            Case EffectCalcType.Subtraction
                NewModule.Attributes(Att) = NewModule.Attributes(Att) - FEffect.AffectedValue
            Case EffectCalcType.CloakedVelocity
                NewModule.Attributes(Att) = -100 + ((100 + NewModule.Attributes(Att)) * (FEffect.AffectedValue / 100))
            Case EffectCalcType.SkillLevel
                NewModule.Attributes(Att) = FEffect.AffectedValue
            Case EffectCalcType.SkillLevelxAtt
                NewModule.Attributes(Att) = NewModule.Attributes(Att) * FEffect.AffectedValue
            Case EffectCalcType.AbsoluteMax
                NewModule.Attributes(Att) = Math.Max(FEffect.AffectedValue, NewModule.Attributes(Att))
            Case EffectCalcType.AbsoluteMin
                NewModule.Attributes(Att) = Math.Min(FEffect.AffectedValue, NewModule.Attributes(Att))
            Case EffectCalcType.CapBoosters
                NewModule.Attributes(Att) = Math.Min(NewModule.Attributes(Att) - FEffect.AffectedValue, 0)
        End Select
        ' Use only 2 decimal places of precision for PG and CPU usage
        If Att = AttributeEnum.ModulePowergridUsage Or Att = AttributeEnum.ModuleCpuUsage Then
            NewModule.Attributes(Att) = Math.Round(NewModule.Attributes(Att), 2, MidpointRounding.AwayFromZero)
        End If
        log &= " --> " & NewModule.Attributes(Att).ToString
        If oldAtt <> NewModule.Attributes(Att).ToString Then
            NewModule.AuditLog.Add(log)
        End If
    End Sub

#End Region

#Region "Cloning Function"

    ''' <summary>
    ''' Clones a fitting for use in experimentation without affecting the default fitting
    ''' </summary>
    ''' <returns>A copy of the current Fitting instance</returns>
    ''' <remarks></remarks>
    Public Function Clone() As Fitting
        ' Quite an elaborate method due to the fact that properties cannot be marked as non-serialised when
        ' attempting to clone using a memory stream and binary formatter. So instead, we create an instance
        ' of a new class which has all but the properties we don't want. The property values are copied,
        ' then the new class is serialised and deserialised in memory to create a true clone.
        Dim ClonedFit As New FittingClone(Me)
        Return ClonedFit.Clone
    End Function

    Public Function Clone(ShipSlot As ShipSlotControl, ShipInfo As ShipInfoControl) As Fitting
        ' Quite an elaborate method due to the fact that properties cannot be marked as non-serialised when
        ' attempting to clone using a memory stream and binary formatter. So instead, we create an instance
        ' of a new class which has all but the properties we don't want. The property values are copied,
        ' then the new class is serialised and deserialised in memory to create a true clone.
        ' This overloaded method restores the ShipSlot and ShipInfo controls after cloning
        Dim ClonedFit As New FittingClone(Me)
        Dim ClonedFitting As Fitting = ClonedFit.Clone
        ClonedFitting.ShipSlotCtrl = ShipSlot
        ClonedFitting.ShipInfoCtrl = ShipInfo
        Return ClonedFitting
    End Function

#End Region

#Region "Data/BaseShip Conversion Routines"

    ''' <summary>
    ''' Takes the modules etc and adds them to the base ship for processing
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateBaseShipFromFitting()

        ApplyFitting(BuildType.BuildEverything, False) ' Do this to build the fitted ship bonuses!

        Call Me.ReorderModules()

        ' Add the modules
        For Each MWS As ModuleWithState In Me.Modules
            Dim NewMod As ShipModule = ModuleLists.ModuleList(CInt(MWS.ID)).Clone
            If MWS.ChargeID <> "" Then
                NewMod.LoadedCharge = ModuleLists.ModuleList(CInt(MWS.ChargeID)).Clone
            End If
            NewMod.ModuleState = MWS.State
            Call Me.AddModule(NewMod, 0, True, True, Nothing, False, False)
        Next

        ApplyFitting(BuildType.BuildFromEffectsMaps, False) ' Add modules/subsystems to FittedShip

        ' Add the drones
        For Each MWS As ModuleQWithState In Me.Drones
            Dim NewMod As ShipModule = ModuleLists.ModuleList(CInt(MWS.ID)).Clone
            NewMod.ModuleState = MWS.State
            If MWS.State = ModuleStates.Active Then
                Call Me.AddDrone(NewMod, MWS.Quantity, True, True)
            Else
                Call Me.AddDrone(NewMod, MWS.Quantity, False, True)
            End If
        Next

        ' Add items
        For Each MWS As ModuleQWithState In Me.Items
            Dim NewMod As ShipModule = ModuleLists.ModuleList(CInt(MWS.ID)).Clone
            NewMod.ModuleState = MWS.State
            Call Me.AddItem(NewMod, MWS.Quantity, True)
        Next

        ' Add ships
        For Each MWS As ModuleQWithState In Me.Ships
            Dim NewMod As Ship = ShipLists.ShipList(ShipLists.ShipListKeyID(CInt(MWS.ID))).Clone
            Call Me.AddShip(NewMod, MWS.Quantity, True)
        Next

        ' Add Boosters
        Me.BaseShip.BoosterSlotCollection.Clear()
        For Each MWS As ModuleWithState In Me.Boosters
            Dim sMod As ShipModule = ModuleLists.ModuleList(CInt(MWS.ID)).Clone
            Me.BaseShip.BoosterSlotCollection.Add(sMod)
        Next

    End Sub

    ''' <summary>
    ''' Updates the actual fitting from the base ship
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateFittingFromBaseShip()

        ' Add the pilot name
        If Me.ShipInfoCtrl IsNot Nothing Then
            If Me.ShipInfoCtrl.cboPilots.SelectedItem IsNot Nothing Then
                Me.PilotName = Me.ShipInfoCtrl.cboPilots.SelectedItem.ToString
            End If
        End If

        ' Clear the modules
        Me.Modules.Clear()

        ' Add subsystem slots
        For slot As Integer = 1 To Me.BaseShip.SubSlots
            If Me.BaseShip.SubSlot(slot) IsNot Nothing Then
                If Me.BaseShip.SubSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.SubSlot(slot).ID), CStr(Me.BaseShip.SubSlot(slot).LoadedCharge.ID), Me.BaseShip.SubSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.SubSlot(slot).ID), Nothing, Me.BaseShip.SubSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add rig slots
        For slot As Integer = 1 To Me.BaseShip.RigSlots
            If Me.BaseShip.RigSlot(slot) IsNot Nothing Then
                If Me.BaseShip.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.RigSlot(slot).ID), CStr(Me.BaseShip.RigSlot(slot).LoadedCharge.ID), Me.BaseShip.RigSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.RigSlot(slot).ID), Nothing, Me.BaseShip.RigSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add low slots
        For slot As Integer = 1 To Me.BaseShip.LowSlots
            If Me.BaseShip.LowSlot(slot) IsNot Nothing Then
                If Me.BaseShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.LowSlot(slot).ID), CStr(Me.BaseShip.LowSlot(slot).LoadedCharge.ID), Me.BaseShip.LowSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.LowSlot(slot).ID), Nothing, Me.BaseShip.LowSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add mid slots
        For slot As Integer = 1 To Me.BaseShip.MidSlots
            If Me.BaseShip.MidSlot(slot) IsNot Nothing Then
                If Me.BaseShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.MidSlot(slot).ID), CStr(Me.BaseShip.MidSlot(slot).LoadedCharge.ID), Me.BaseShip.MidSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.MidSlot(slot).ID), Nothing, Me.BaseShip.MidSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add high slots
        For slot As Integer = 1 To Me.BaseShip.HiSlots
            If Me.BaseShip.HiSlot(slot) IsNot Nothing Then
                If Me.BaseShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.HiSlot(slot).ID), CStr(Me.BaseShip.HiSlot(slot).LoadedCharge.ID), Me.BaseShip.HiSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(CStr(Me.BaseShip.HiSlot(slot).ID), Nothing, Me.BaseShip.HiSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add drones
        Me.Drones.Clear()
        For Each DBI As DroneBayItem In Me.BaseShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                Me.Drones.Add(New ModuleQWithState(CStr(DBI.DroneType.ID), ModuleStates.Active, DBI.Quantity))
            Else
                Me.Drones.Add(New ModuleQWithState(CStr(DBI.DroneType.ID), ModuleStates.Inactive, DBI.Quantity))
            End If
        Next

        ' Add items
        Me.Items.Clear()
        For Each CBI As CargoBayItem In Me.BaseShip.CargoBayItems.Values
            Me.Items.Add(New ModuleQWithState(CStr(CBI.ItemType.ID), ModuleStates.Active, CBI.Quantity))
        Next

        ' Add ships
        Me.Ships.Clear()
        For Each SBI As ShipBayItem In Me.BaseShip.ShipBayItems.Values
            Me.Ships.Add(New ModuleQWithState(CStr(SBI.ShipType.ID), ModuleStates.Active, SBI.Quantity))
        Next

        ' Add boosters
        Me.Boosters.Clear()
        For Each Booster As ShipModule In Me.BaseShip.BoosterSlotCollection
            Me.Boosters.Add(New ModuleWithState(CStr(Booster.ID), Nothing, ModuleStates.Active))
        Next

    End Sub

    ''' <summary>
    ''' Re-orders modules to allow correct update procedures
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReorderModules()
        Dim subs, mods As New ArrayList
        For Each MWS As ModuleWithState In Me.Modules
            If ModuleLists.ModuleList.ContainsKey(CInt(MWS.ID)) = True Then
                If ModuleLists.ModuleList(CInt(MWS.ID)).SlotType = 16 Then
                    subs.Add(MWS)
                Else
                    mods.Add(MWS)
                End If
            Else
                mods.Add(MWS)
            End If
        Next
        ' Recreate the current fit
        Me.Modules.Clear()
        For Each MWS As ModuleWithState In subs
            Me.Modules.Add(MWS)
        Next
        For Each MWS As ModuleWithState In mods
            Me.Modules.Add(MWS)
        Next
        subs.Clear()
        mods.Clear()
    End Sub

#End Region

#Region "Base Ship Item Adding Routines"

    Public Sub AddModule(ByVal shipMod As ShipModule, ByVal slotNo As Integer, ByVal UpdateShip As Boolean, ByVal UpdateAll As Boolean, ByVal repMod As ShipModule, ByVal SuppressUndo As Boolean, ByVal IsSwappingModules As Boolean)
        ' Check for command processors as this affects the fitting!
        If shipMod.ID = ModuleEnum.ItemCommandProcessorI And shipMod.ModuleState = ModuleStates.Active Then
            Me.BaseShip.Attributes(AttributeEnum.ShipMaxGangLinks) += 1
        End If

        ' Check slot availability (only if not adding in a specific slot?)
        If IsSwappingModules = False Then
            If IsSlotAvailable(shipMod, repMod) = False Then
                Exit Sub
            End If
        End If

        ' Check fitting constraints
        If IsSwappingModules = False Then
            If IsModulePermitted(shipMod, False, repMod) = False Then
                Exit Sub
            End If
        End If

        ' Get old module if applicable
        Dim OldModName As String = ""
        Dim OldChargeName As String = ""
        If slotNo <> 0 Then
            Dim LoadedModule As New ShipModule
            Select Case shipMod.SlotType
                Case SlotTypes.Rig
                    LoadedModule = BaseShip.RigSlot(slotNo)
                Case SlotTypes.Low
                    LoadedModule = BaseShip.LowSlot(slotNo)
                Case SlotTypes.Mid
                    LoadedModule = BaseShip.MidSlot(slotNo)
                Case SlotTypes.High
                    LoadedModule = BaseShip.HiSlot(slotNo)
                Case SlotTypes.Subsystem
                    LoadedModule = BaseShip.SubSlot(slotNo)
            End Select
            If LoadedModule IsNot Nothing Then
                OldModName = LoadedModule.Name
                If LoadedModule.LoadedCharge IsNot Nothing Then
                    OldChargeName = LoadedModule.LoadedCharge.Name
                End If
            End If
        End If

        ' Add Module to the next slot
        If slotNo = 0 Then
            slotNo = AddModuleInNextSlot(shipMod.Clone)
        Else
            AddModuleInSpecifiedSlot(shipMod.Clone, slotNo)
        End If

        ' Check if we need to update
        If UpdateAll = False Then
            If UpdateShip = True Then
                ' What sort of update do we need? Check for subsystems enabled
                If shipMod.DatabaseCategory = ModuleEnum.CategorySubsystems Then
                    Me.BaseShip = Me.BuildSubSystemEffects(Me.BaseShip)
                    If Me.ShipSlotCtrl IsNot Nothing Then
                        Call Me.ShipSlotCtrl.UpdateShipSlotLayout()
                    End If
                    Me.ApplyFitting(BuildType.BuildEverything)
                Else
                    Me.ApplyFitting(BuildType.BuildFromEffectsMaps)
                End If
            End If
            ' Update the Undo stack
            If SuppressUndo = False Then
                Dim chargeName As String = ""
                If shipMod.LoadedCharge IsNot Nothing Then
                    chargeName = shipMod.LoadedCharge.Name
                End If
                Dim transType As UndoInfo.TransType = UndoInfo.TransType.AddModule
                If IsSwappingModules = True Then
                    transType = UndoInfo.TransType.SwapModules
                ElseIf repMod IsNot Nothing Then
                    transType = UndoInfo.TransType.ReplacedModule
                End If
                Me.ShipSlotCtrl.UndoStack.Push(New UndoInfo(transType, shipMod.SlotType, slotNo, OldModName, OldChargeName, slotNo, shipMod.Name, chargeName))
                Me.ShipSlotCtrl.UpdateHistory()
            End If
        Else
            ' Need to rebuild the ship in order to account for the new modules as they're being added
            If shipMod.DatabaseCategory = ModuleEnum.CategorySubsystems Then
                Me.BaseShip = Me.BuildSubSystemEffects(Me.BaseShip)
            End If
        End If
    End Sub

    Public Sub AddDrone(ByVal Drone As ShipModule, ByVal Qty As Integer, ByVal Active As Boolean, ByVal UpdateAll As Boolean)
        ' Set grouping flag
        Dim grouped As Boolean = False
        ' See if there is sufficient space
        Dim vol As Double = Drone.Volume
        Dim myShip As New Ship
        If FittedShip IsNot Nothing Then
            myShip = FittedShip
        Else
            myShip = Me.BaseShip
        End If
        If myShip.DroneBay - Me.BaseShip.DroneBayUsed >= vol * Qty Then
            ' Scan through existing items and see if we can group this new one
            For Each droneGroup As DroneBayItem In Me.BaseShip.DroneBayItems.Values
                If Drone.Name = droneGroup.DroneType.Name And Active = droneGroup.IsActive And UpdateAll = False Then
                    ' Add to existing drone group
                    droneGroup.Quantity += Qty
                    grouped = True
                    Exit For
                End If
            Next
            ' Put the drone into the drone bay if not grouped
            If grouped = False Then
                Dim bw As Double = Drone.Attributes(AttributeEnum.ModuleDroneBandwidthNeeded)
                Dim DBI As New DroneBayItem
                DBI.DroneType = Drone
                DBI.Quantity = Qty
                If Active = True And myShip.MaxDrones - Me.BaseShip.UsedDrones >= Qty And myShip.DroneBandwidth - Me.BaseShip.DroneBandwidthUsed >= Qty * bw Then
                    DBI.IsActive = True
                Else
                    DBI.IsActive = False
                End If
                Me.BaseShip.DroneBayItems.Add(Me.BaseShip.DroneBayItems.Count, DBI)
            End If
            ' Update stuff
            If UpdateAll = False Then
                Me.ApplyFitting(BuildType.BuildFromEffectsMaps)
                If Me.ShipSlotCtrl IsNot Nothing Then
                    Call Me.ShipSlotCtrl.UpdateDroneBay()
                End If
            Else
                Me.BaseShip.DroneBayUsed += vol * Qty
            End If
        Else
            MessageBox.Show("There is not enough space in the Drone Bay to hold " & Qty & " unit(s) of " & Drone.Name & " on '" & FittingName & "' (" & ShipName & ").", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Public Sub AddItem(ByVal Item As ShipModule, ByVal Qty As Integer, ByVal UpdateAll As Boolean)
        If Me.BaseShip IsNot Nothing Then
            ' Set grouping flag
            Dim grouped As Boolean = False
            ' See if there is sufficient space
            Dim vol As Double = Item.Volume
            Dim myShip As New Ship
            If FittedShip IsNot Nothing Then
                myShip = FittedShip
            Else
                myShip = Me.BaseShip
            End If
            If myShip.CargoBay - Me.BaseShip.CargoBayUsed >= vol * Qty Then
                ' Scan through existing items and see if we can group this new one
                For Each itemGroup As CargoBayItem In Me.BaseShip.CargoBayItems.Values
                    If Item.Name = itemGroup.ItemType.Name And UpdateAll = False Then
                        ' Add to existing item group
                        itemGroup.Quantity += Qty
                        grouped = True
                        Exit For
                    End If
                Next
                ' Put the item into the cargo bay if not grouped
                If grouped = False Then
                    Dim CBI As New CargoBayItem
                    CBI.ItemType = Item
                    CBI.Quantity = Qty
                    Me.BaseShip.CargoBayItems.Add(Me.BaseShip.CargoBayItems.Count, CBI)
                End If
                ' Update stuff
                If UpdateAll = False Then
                    Me.ApplyFitting(BuildType.BuildFromEffectsMaps)
                    If Me.ShipSlotCtrl IsNot Nothing Then
                        Call Me.ShipSlotCtrl.UpdateItemBay()
                    End If
                Else
                    Me.BaseShip.CargoBayUsed += vol * Qty
                End If
            Else
                MessageBox.Show("There is not enough space in the Cargo Bay to hold " & Qty & " unit(s) of " & Item.Name & " on '" & FittingName & "' (" & ShipName & ").", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Public Sub AddShip(ByVal Item As Ship, ByVal Qty As Integer, ByVal UpdateAll As Boolean)
        If Me.BaseShip IsNot Nothing Then
            ' Set grouping flag
            Dim grouped As Boolean = False
            ' See if there is sufficient space
            Dim vol As Double = Item.Volume
            Dim myShip As New Ship
            If FittedShip IsNot Nothing Then
                myShip = FittedShip
            Else
                myShip = Me.BaseShip
            End If
            If myShip.ShipBay - Me.BaseShip.ShipBayUsed >= vol * Qty Then
                ' Scan through existing items and see if we can group this new one
                For Each itemGroup As ShipBayItem In Me.BaseShip.ShipBayItems.Values
                    If Item.Name = itemGroup.ShipType.Name And UpdateAll = False Then
                        ' Add to existing item group
                        itemGroup.Quantity += Qty
                        grouped = True
                        Exit For
                    End If
                Next
                ' Put the item into the ship maintenance bay if not grouped
                If grouped = False Then
                    Dim sBI As New ShipBayItem
                    sBI.ShipType = Item
                    sBI.Quantity = Qty
                    Me.BaseShip.ShipBayItems.Add(Me.BaseShip.ShipBayItems.Count, sBI)
                End If
                ' Update stuff
                If UpdateAll = False Then
                    Me.ApplyFitting(BuildType.BuildFromEffectsMaps)
                    If Me.ShipSlotCtrl IsNot Nothing Then
                        Call Me.ShipSlotCtrl.UpdateShipBay()
                    End If
                Else
                    Me.BaseShip.ShipBayUsed += vol * Qty
                End If
            Else
                MessageBox.Show("There is not enough space in the Ship Maintenance Bay to hold " & Qty & " unit(s) of " & Item.Name & " on '" & FittingName & "' (" & ShipName & ").", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

#End Region

#Region "Item Fitting Check Routines"

    Private Function IsSlotAvailable(ByVal shipMod As ShipModule, Optional ByVal repShipMod As ShipModule = Nothing) As Boolean
        Dim cSub, cRig, cLow, cMid, cHi, cTurret, cLauncher As Integer

        If repShipMod IsNot Nothing Then
            Select Case repShipMod.SlotType
                Case SlotTypes.Rig
                    cRig = Me.BaseShip.RigSlotsUsed - 1
                Case SlotTypes.Low
                    cLow = Me.BaseShip.LowSlotsUsed - 1
                Case SlotTypes.Mid
                    cMid = Me.BaseShip.MidSlotsUsed - 1
                Case SlotTypes.High
                    cHi = Me.BaseShip.HiSlotsUsed - 1
                Case SlotTypes.Subsystem
                    cSub = Me.BaseShip.SubSlotsUsed - 1
            End Select
            If repShipMod.IsTurret = True Then
                cTurret = Me.BaseShip.TurretSlotsUsed - 1
            End If
            If repShipMod.IsLauncher = True Then
                cLauncher = Me.BaseShip.LauncherSlotsUsed - 1
            End If
        Else
            cSub = Me.BaseShip.SubSlotsUsed
            cRig = Me.BaseShip.RigSlotsUsed
            cLow = Me.BaseShip.LowSlotsUsed
            cMid = Me.BaseShip.MidSlotsUsed
            cHi = Me.BaseShip.HiSlotsUsed
            cTurret = Me.BaseShip.TurretSlotsUsed
            cLauncher = Me.BaseShip.LauncherSlotsUsed
        End If
        ' First, check slot layout
        Select Case shipMod.SlotType
            Case SlotTypes.Rig
                If cRig = Me.BaseShip.RigSlots Then
                    MessageBox.Show("There are no available rig slots remaining on '" & FittingName & "' (" & ShipName & ").", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.Low
                If cLow = Me.BaseShip.LowSlots Then
                    MessageBox.Show("There are no available low slots remaining on '" & FittingName & "' (" & ShipName & ").", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.Mid
                If cMid = Me.BaseShip.MidSlots Then
                    MessageBox.Show("There are no available mid slots remaining on '" & FittingName & "' (" & ShipName & ").", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.High
                If cHi = Me.BaseShip.HiSlots Then
                    MessageBox.Show("There are no available high slots remaining on '" & FittingName & "' (" & ShipName & ").", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.Subsystem
                If cSub = Me.BaseShip.SubSlots Then
                    MessageBox.Show("There are no available subsystem slots remaining on '" & FittingName & "' (" & ShipName & ").", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
        End Select

        ' Now check launcher slots
        If shipMod.IsLauncher Then
            If cLauncher = Me.BaseShip.LauncherSlots Then
                MessageBox.Show("There are no available launcher slots remaining on '" & FittingName & "' (" & ShipName & ").", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        ' Now check turret slots
        If shipMod.IsTurret Then
            If cTurret = Me.BaseShip.TurretSlots Then
                MessageBox.Show("There are no available turret slots remaining on '" & FittingName & "' (" & ShipName & ").", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        Return True
    End Function
    Public Function IsModulePermitted(ByRef shipMod As ShipModule, ByVal search As Boolean, Optional ByVal repMod As ShipModule = Nothing) As Boolean
        ' Check for subsystem restrictions
        If shipMod.DatabaseCategory = ModuleEnum.CategorySubsystems Then
            ' Check for subsystem type restriction
            If CStr(shipMod.Attributes(AttributeEnum.ModuleFitsToShipType)) <> CStr(Me.BaseShip.ID) Then
                If search = False Then
                    MessageBox.Show("You cannot fit a subsystem module designed for a " & StaticData.Types(CInt(shipMod.Attributes(AttributeEnum.ModuleFitsToShipType))).Name & " to your " & ShipName & " ('" & FittingName & "').", "Ship Type Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return False
            End If
            ' Check for subsystem group restriction
            Dim subReplace As Boolean = False
            If repMod IsNot Nothing Then
                If repMod.Attributes(AttributeEnum.ModuleSubsystemSlot) = shipMod.Attributes(AttributeEnum.ModuleSubsystemSlot) Then
                    subReplace = True
                End If
            End If
            If subReplace = False Then
                For s As Integer = 1 To Me.BaseShip.SubSlots
                    If Me.BaseShip.SubSlot(s) IsNot Nothing Then
                        If CStr(shipMod.Attributes(AttributeEnum.ModuleSubsystemSlot)) = CStr(Me.BaseShip.SubSlot(s).Attributes(AttributeEnum.ModuleSubsystemSlot)) Then
                            If search = False Then
                                MessageBox.Show("You already have a subsystem of this group fitted to your " & ShipName & " ('" & FittingName & "').", "Subsystem Group Duplication", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                            Return False
                        End If
                    End If
                Next
            End If
        End If

        ' Check for Rig restrictions
        If shipMod.SlotType = SlotTypes.Rig Then
            If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleRigSize) And Me.BaseShip.Attributes.ContainsKey(AttributeEnum.ShipRigSize) Then
                If CInt(shipMod.Attributes(AttributeEnum.ModuleRigSize)) <> CInt(Me.BaseShip.Attributes(AttributeEnum.ShipRigSize)) Then
                    Dim requiredSize As String = ""
                    Select Case CInt(Me.BaseShip.Attributes(AttributeEnum.ShipRigSize))
                        Case 1
                            requiredSize = "Small"
                        Case 2
                            requiredSize = "Medium"
                        Case 3
                            requiredSize = "Large"
                        Case 4
                            requiredSize = "Capital"
                    End Select
                    Dim baseModName As String = requiredSize & shipMod.Name.Remove(0, shipMod.Name.IndexOf(" "))
                    If search = False Then
                        If ModuleLists.moduleListName.ContainsKey(baseModName) = True Then
                            MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & ShipName & ". HQF has therefore substituted the " & requiredSize & " variant instead.", "Rig Size Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            shipMod = CType(ModuleLists.moduleList(ModuleLists.moduleListName(baseModName)), ShipModule)
                        Else
                            MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & ShipName & " ('" & FittingName & "').", "Rig Size Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return False
                        End If
                    Else
                        Return False
                    End If
                End If
            End If
        End If

        ' Check for ship group restrictions
        Dim ShipGroups As New List(Of Integer)
        Dim shipGroupAttributes() As Integer = {AttributeEnum.ModuleCanFitShipGroup1, AttributeEnum.ModuleCanFitShipGroup2, AttributeEnum.ModuleCanFitShipGroup3, AttributeEnum.ModuleCanFitShipGroup4, AttributeEnum.ModuleCanFitShipGroup5, AttributeEnum.ModuleCanFitShipGroup6, AttributeEnum.ModuleCanFitShipGroup7, AttributeEnum.ModuleCanFitShipGroup8}
        For Each att As Integer In shipGroupAttributes
            If shipMod.Attributes.ContainsKey(att) = True Then
                ShipGroups.Add(CInt(shipMod.Attributes(att)))
            End If
        Next
        ' Check for ship type restrictions
        Dim ShipTypes As New List(Of Integer)
        Dim shipTypeAttributes() As Integer = {AttributeEnum.ModuleCanFitShipType1, AttributeEnum.ModuleCanFitShipType2, AttributeEnum.ModuleCanFitShipType3, AttributeEnum.ModuleCanFitShipType4}
        For Each att As Integer In shipTypeAttributes
            If shipMod.Attributes.ContainsKey(att) = True Then
                ShipTypes.Add(CInt(shipMod.Attributes(att)))
            End If
        Next
        ' Apply ship group and type restrictions
        If ShipGroups.Count > 0 Then
            If ShipGroups.Contains(Me.BaseShip.DatabaseGroup) = False Then
                If ShipTypes.Contains(Me.BaseShip.ID) = False Then
                    If search = False Then
                        MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & ShipName & " ('" & FittingName & "').", "Ship Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Return False
                End If
            ElseIf Me.BaseShip.DatabaseGroup = ModuleEnum.GroupStrategicCruisers Then
                ' Check for correct subsystems
                Dim allowed As Boolean = False
                Dim subID As Integer
                If shipMod.DatabaseGroup = ModuleEnum.GroupGangLinks Then
                    For slotNo As Integer = 1 To 5
                        If Me.BaseShip.SubSlot(slotNo) IsNot Nothing Then
                            subID = Me.BaseShip.SubSlot(slotNo).ID
                            If subID = ModuleEnum.ItemLegionWarfareProcessor Or subID = ModuleEnum.ItemLokiWarfareProcessor Or subID = ModuleEnum.ItemProteusWarfareProcessor Or subID = ModuleEnum.ItemTenguWarfareProcessor Then
                                allowed = True
                                Exit For
                            End If
                        End If
                    Next
                    If allowed = False Then
                        If search = False Then
                            MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & ShipName & " ('" & FittingName & "') without Warfare Processor subsystem.", "Subsystem Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return False
                    End If
                ElseIf shipMod.DatabaseGroup = ModuleEnum.GroupCloakingDevices Or shipMod.DatabaseGroup = ModuleEnum.GroupCynosuralFields Then
                    For slotNo As Integer = 1 To 5
                        If Me.BaseShip.SubSlot(slotNo) IsNot Nothing Then
                            subID = Me.BaseShip.SubSlot(slotNo).ID
                            If subID = ModuleEnum.ItemLegionCovertReconfiguration Or subID = ModuleEnum.ItemLokiCovertReconfiguration Or subID = ModuleEnum.ItemProteusCovertReconfiguration Or subID = ModuleEnum.ItemTenguCovertReconfiguration Then
                                allowed = True
                                Exit For
                            End If
                        End If
                    Next
                    If allowed = False Then
                        If search = False Then
                            MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & ShipName & " ('" & FittingName & "') without Covert Reconfiguration subsystem.", "Subsystem Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                        Return False
                    End If
                End If
            End If
        ElseIf ShipTypes.Count > 0 And ShipTypes.Contains(Me.BaseShip.ID) = False Then
            If search = False Then
                MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & ShipName & " ('" & FittingName & "').", "Ship Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return False
        End If
        ShipGroups.Clear()

        ' Check for maxGroupFitted flag
        If shipMod.Attributes.ContainsKey(AttributeEnum.ModuleMaxGroupFitted) = True Then
            Dim groupReplace As Boolean = False
            If repMod IsNot Nothing AndAlso repMod.DatabaseGroup = shipMod.DatabaseGroup Then
                groupReplace = True
            End If
            If IsModuleGroupLimitExceeded(shipMod, Not groupReplace, AttributeEnum.ModuleMaxGroupFitted) = True Then
                If search = False Then
                    MessageBox.Show("You cannot fit more than " & shipMod.Attributes(AttributeEnum.ModuleMaxGroupFitted) & " module(s) of this group to a ship ('" & FittingName & "', " & ShipName & ").", "Module Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Return False
            End If
        End If

        ' Check for maxGroupActive flag
        If search = False AndAlso shipMod.Attributes.ContainsKey(AttributeEnum.ModuleMaxGroupActive) = True Then
            Dim groupReplace As Boolean = False
            If repMod IsNot Nothing AndAlso repMod.DatabaseGroup = shipMod.DatabaseGroup Then
                groupReplace = True
            End If
            If shipMod.DatabaseGroup <> ModuleEnum.GroupGangLinks Then
                If IsModuleGroupLimitExceeded(shipMod, Not groupReplace, AttributeEnum.ModuleMaxGroupActive) = True Then
                    ' Set the module offline
                    shipMod.ModuleState = ModuleStates.Inactive
                End If
            Else
                ' Check active command relay bonus (attID=435) on ship
                If IsModuleGroupLimitExceeded(shipMod, Not groupReplace, AttributeEnum.ModuleMaxGroupActive) = True Then
                    ' Set the module offline
                    shipMod.ModuleState = ModuleStates.Inactive
                Else
                    If CountActiveTypeModules(shipMod.ID) >= CInt(shipMod.Attributes(AttributeEnum.ModuleMaxGroupActive)) Then
                        ' Set the module offline
                        shipMod.ModuleState = ModuleStates.Inactive
                    End If
                End If
            End If
        End If

        Return True
    End Function
    Public Function IsModuleGroupLimitExceeded(ByVal testMod As ShipModule, ByVal includeTestMod As Boolean, ByVal attribute As Integer) As Boolean
        Dim count As Integer = 0
        Dim fittedMod As ShipModule = testMod.Clone
        Me.ApplySkillEffectsToModule(fittedMod, True)
        Dim maxAllowed As Integer = 1
        Dim moduleState As Integer = ModuleStates.Offline
        Select Case attribute
            Case AttributeEnum.ModuleMaxGroupFitted
                maxAllowed = CInt(fittedMod.Attributes(AttributeEnum.ModuleMaxGroupFitted))
            Case AttributeEnum.ModuleMaxGroupActive
                moduleState = ModuleStates.Active
                If fittedMod.DatabaseGroup = ModuleEnum.GroupGangLinks Then
                    If Me.FittedShip.Attributes.ContainsKey(AttributeEnum.ShipMaxGangLinks) = True Then
                        maxAllowed = CInt(Me.FittedShip.Attributes(AttributeEnum.ShipMaxGangLinks))
                    End If
                Else
                    maxAllowed = CInt(fittedMod.Attributes(AttributeEnum.ModuleMaxGroupActive))
                End If
        End Select

        For slot As Integer = 1 To Me.BaseShip.HiSlots
            If Me.BaseShip.HiSlot(slot) IsNot Nothing Then
                If Me.BaseShip.HiSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.HiSlot(slot).ModuleState >= moduleState Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.MidSlots
            If Me.BaseShip.MidSlot(slot) IsNot Nothing Then
                If Me.BaseShip.MidSlot(slot).ID <> ModuleEnum.ItemCommandProcessorI Then
                    If Me.BaseShip.MidSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.MidSlot(slot).ModuleState >= moduleState Then
                        count += 1
                    End If
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.LowSlots
            If Me.BaseShip.LowSlot(slot) IsNot Nothing Then
                If Me.BaseShip.LowSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.LowSlot(slot).ModuleState >= moduleState Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.RigSlots
            If Me.BaseShip.RigSlot(slot) IsNot Nothing Then
                If Me.BaseShip.RigSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.RigSlot(slot).ModuleState >= moduleState Then
                    count += 1
                End If
            End If
        Next
        If includeTestMod = True Then
            If count >= maxAllowed Then
                Return True
            Else
                Return False
            End If
        Else
            If count > maxAllowed Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function CountActiveTypeModules(ByVal typeID As Integer) As Integer
        Dim count As Integer = 0
        For slot As Integer = 1 To Me.BaseShip.HiSlots
            If Me.BaseShip.HiSlot(slot) IsNot Nothing Then
                If Me.BaseShip.HiSlot(slot).ID = typeID And Me.BaseShip.HiSlot(slot).ModuleState >= ModuleStates.Active Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.MidSlots
            If Me.BaseShip.MidSlot(slot) IsNot Nothing Then
                If Me.BaseShip.MidSlot(slot).ID = typeID And Me.BaseShip.MidSlot(slot).ModuleState >= ModuleStates.Active Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.LowSlots
            If Me.BaseShip.LowSlot(slot) IsNot Nothing Then
                If Me.BaseShip.LowSlot(slot).ID = typeID And Me.BaseShip.LowSlot(slot).ModuleState >= ModuleStates.Active Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.RigSlots
            If Me.BaseShip.RigSlot(slot) IsNot Nothing Then
                If Me.BaseShip.RigSlot(slot).ID = typeID And Me.BaseShip.RigSlot(slot).ModuleState >= ModuleStates.Active Then
                    count += 1
                End If
            End If
        Next
        Return count
    End Function
    Private Function AddModuleInNextSlot(ByVal shipMod As ShipModule) As Integer
        Select Case shipMod.SlotType
            Case SlotTypes.Rig
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.RigSlot(slotNo) Is Nothing Then
                        Me.BaseShip.RigSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available rig slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.Low
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.LowSlot(slotNo) Is Nothing Then
                        Me.BaseShip.LowSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available low slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.Mid
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.MidSlot(slotNo) Is Nothing Then
                        Me.BaseShip.MidSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available mid slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.High
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.HiSlot(slotNo) Is Nothing Then
                        Me.BaseShip.HiSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available high slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.Subsystem
                For slotNo As Integer = 1 To 5
                    If Me.BaseShip.SubSlot(slotNo) Is Nothing Then
                        Me.BaseShip.SubSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available subsystem slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
        Return 0
    End Function
    Private Function AddModuleInSpecifiedSlot(ByVal shipMod As ShipModule, ByVal slotNo As Integer) As Integer
        Select Case shipMod.SlotType
            Case SlotTypes.Rig
                Me.BaseShip.RigSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.Low
                Me.BaseShip.LowSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.Mid
                Me.BaseShip.MidSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.High
                Me.BaseShip.HiSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.Subsystem
                Me.BaseShip.SubSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
        End Select
        Return 0
    End Function

#End Region

#Region "Skill Requirements"
    Public Function CalculateNeededSkills(ByVal pilotName As String) As NeededSkillsCollection
        Dim allSkills As SortedList = CollectNeededSkills(Me.BaseShip)
        Dim shipPilot As FittingPilot = FittingPilots.HQFPilots(pilotName)
        Dim truePilot As EveHQ.Core.EveHQPilot = EveHQ.Core.HQ.Settings.Pilots(pilotName)
        Dim shipPilotSkills As New ArrayList
        Dim truePilotSkills As New ArrayList

        For Each rSkill As ReqSkill In allSkills.Values
            ' Check for shipPilot match
            If shipPilot.SkillSet.ContainsKey(rSkill.Name) = True Then
                If shipPilot.SkillSet(rSkill.Name).Level < rSkill.ReqLevel Then
                    shipPilotSkills.Add(rSkill)
                End If
            Else
                shipPilotSkills.Add(rSkill)
            End If
            ' Check for truePilot match
            If truePilot.PilotSkills.ContainsKey(rSkill.Name) = True Then
                If truePilot.PilotSkills(rSkill.Name).Level < rSkill.ReqLevel Then
                    truePilotSkills.Add(rSkill)
                End If
            Else
                truePilotSkills.Add(rSkill)
            End If
        Next
        Dim neededSkills As New NeededSkillsCollection
        neededSkills.ShipPilotSkills = shipPilotSkills
        neededSkills.TruePilotSkills = truePilotSkills
        Return neededSkills
    End Function
    Private Function CollectNeededSkills(ByVal cShip As Ship) As SortedList

        Dim nSkills As New SortedList
        Dim rSkill As New ReqSkill

        ' Get Ship Skills
        Dim count As Integer = 0
        For Each nSkill As ItemSkills In cShip.RequiredSkills.Values
            count += 1
            rSkill = New ReqSkill
            rSkill.Name = nSkill.Name
            rSkill.ID = nSkill.ID
            rSkill.ReqLevel = nSkill.Level
            rSkill.CurLevel = 0
            rSkill.NeededFor = cShip.Name
            nSkills.Add("Ship" & count.ToString, rSkill)
        Next

        ' Get Subsystem Skills
        For slot As Integer = 1 To cShip.SubSlots
            If cShip.SubSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In cShip.SubSlot(slot).RequiredSkills.Values
                    count += 1
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = 0
                    rSkill.NeededFor = cShip.SubSlot(slot).Name
                    nSkills.Add("SubSlot" & slot.ToString & count.ToString, rSkill)
                Next
            End If
        Next

        ' Get Rig Skills
        For slot As Integer = 1 To cShip.RigSlots
            If cShip.RigSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In cShip.RigSlot(slot).RequiredSkills.Values
                    count += 1
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = 0
                    rSkill.NeededFor = cShip.RigSlot(slot).Name
                    nSkills.Add("RigSlot" & slot.ToString & count.ToString, rSkill)
                Next
            End If
        Next

        ' Get Low Slot Skills
        For slot As Integer = 1 To cShip.LowSlots
            If cShip.LowSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In cShip.LowSlot(slot).RequiredSkills.Values
                    count += 1
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = 0
                    rSkill.NeededFor = cShip.LowSlot(slot).Name
                    nSkills.Add("LowSlot" & slot.ToString & count.ToString, rSkill)
                Next
                If cShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    count = 0
                    For Each nSkill As ItemSkills In cShip.LowSlot(slot).LoadedCharge.RequiredSkills.Values
                        count += 1
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = 0
                        rSkill.NeededFor = cShip.LowSlot(slot).LoadedCharge.Name
                        nSkills.Add("LowSlot Charge" & slot.ToString & count.ToString, rSkill)
                    Next
                End If
            End If
        Next

        ' Get Mid Slot Skills
        For slot As Integer = 1 To cShip.MidSlots
            If cShip.MidSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In cShip.MidSlot(slot).RequiredSkills.Values
                    count += 1
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = 0
                    rSkill.NeededFor = cShip.MidSlot(slot).Name
                    nSkills.Add("MidSlot" & slot.ToString & count.ToString, rSkill)
                Next
                If cShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    count = 0
                    For Each nSkill As ItemSkills In cShip.MidSlot(slot).LoadedCharge.RequiredSkills.Values
                        count += 1
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = 0
                        rSkill.NeededFor = cShip.MidSlot(slot).LoadedCharge.Name
                        nSkills.Add("MidSlot Charge" & slot.ToString & count.ToString, rSkill)
                    Next
                End If
            End If
        Next

        ' Get High Slot Skills
        For slot As Integer = 1 To cShip.HiSlots
            If cShip.HiSlot(slot) IsNot Nothing Then
                count = 0
                For Each nSkill As ItemSkills In cShip.HiSlot(slot).RequiredSkills.Values
                    count += 1
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = 0
                    rSkill.NeededFor = cShip.HiSlot(slot).Name
                    nSkills.Add("HiSlot" & slot.ToString & count.ToString, rSkill)
                Next
                If cShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    count = 0
                    For Each nSkill As ItemSkills In cShip.HiSlot(slot).LoadedCharge.RequiredSkills.Values
                        count += 1
                        rSkill = New ReqSkill
                        rSkill.Name = nSkill.Name
                        rSkill.ID = nSkill.ID
                        rSkill.ReqLevel = nSkill.Level
                        rSkill.CurLevel = 0
                        rSkill.NeededFor = cShip.HiSlot(slot).LoadedCharge.Name
                        nSkills.Add("HiSlot Charge" & slot.ToString & count.ToString, rSkill)
                    Next
                End If
            End If
        Next

        ' Get Drone skills
        count = 0
        For Each DBI As DroneBayItem In cShip.DroneBayItems.Values
            For Each nSkill As ItemSkills In DBI.DroneType.RequiredSkills.Values
                count += 1
                rSkill = New ReqSkill
                rSkill.Name = nSkill.Name
                rSkill.ID = nSkill.ID
                rSkill.ReqLevel = nSkill.Level
                rSkill.CurLevel = 0
                rSkill.NeededFor = DBI.DroneType.Name
                nSkills.Add("Drone" & count.ToString, rSkill)
            Next
        Next

        ' Get Implant Skills
        Dim shipPilot As FittingPilot = FittingPilots.HQFPilots(PilotName)
        Dim FittedImplantName As String
        Dim FittedImplant As ShipModule
        For ImplantSlot As Integer = 1 To 10
            count = 0
            If shipPilot.ImplantName(ImplantSlot) <> "" Then
                FittedImplantName = shipPilot.ImplantName(ImplantSlot)
                FittedImplant = CType(ModuleLists.moduleList(ModuleLists.moduleListName(FittedImplantName)), ShipModule)
                For Each nSkill As ItemSkills In FittedImplant.RequiredSkills.Values
                    count += 1
                    rSkill = New ReqSkill
                    rSkill.Name = nSkill.Name
                    rSkill.ID = nSkill.ID
                    rSkill.ReqLevel = nSkill.Level
                    rSkill.CurLevel = 0
                    rSkill.NeededFor = FittedImplant.Name
                    nSkills.Add("Implant" & ImplantSlot.ToString & count.ToString, rSkill)
                Next

            End If
        Next

        Return nSkills
    End Function

#End Region

End Class

<Serializable()> Public Class FittingClone

    Dim cShipName As String = ""
    Dim cFittingName As String = ""
    Dim cKeyName As String = ""

    Dim cPilotName As String = ""
    Dim cDamageProfileName As String = ""
    Dim cDefenceProfileName As String = ""

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

    ''' <summary>
    ''' Gets or sets the the Ship Name used for the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the ship type used for the fitting</returns>
    ''' <remarks></remarks>
    Public Property ShipName() As String
        Get
            Return cShipName
        End Get
        Set(ByVal value As String)
            cShipName = value
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
    ''' Gets or sets the name of the defence profile used for the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>The name of the defence profile used for the fitting</returns>
    ''' <remarks></remarks>
    Public Property DefenceProfileName() As String
        Get
            Return cDefenceProfileName
        End Get
        Set(ByVal value As String)
            cDefenceProfileName = value
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
    ''' Gets or sets a collection of remote effects to be applied to the fitting
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of fleet effects to be applied to the fitting</returns>
    ''' <remarks></remarks>
    Public Property RemoteEffects() As List(Of RemoteEffect)
        Get
            Return cRemoteEffects
        End Get
        Set(ByVal value As List(Of RemoteEffect))
            cRemoteEffects = value
        End Set
    End Property

    Public Sub New(ByVal FitToClone As Fitting)

        Dim typ As Type = Me.GetType()
        Dim PI As Reflection.PropertyInfo() = typ.GetProperties()
        For Each p As Reflection.PropertyInfo In PI
            Dim FitPI As System.Reflection.PropertyInfo = FitToClone.GetType().GetProperty(p.Name)
            If p.CanWrite Then
                p.SetValue(Me, FitPI.GetValue(FitToClone, Nothing), Nothing)
            End If
        Next

    End Sub

    Public Function Clone() As Fitting
        Dim FitMemoryStream As New MemoryStream
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(FitMemoryStream, Me)
        FitMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim NewFitClone As FittingClone = CType(objBinaryFormatter.Deserialize(FitMemoryStream), FittingClone)
        FitMemoryStream.Close()

        Dim NewFit As New Fitting(NewFitClone.ShipName, NewFitClone.FittingName, NewFitClone.PilotName)

        Dim typ As Type = NewFit.GetType()
        Dim PI As Reflection.PropertyInfo() = typ.GetProperties()
        For Each p As Reflection.PropertyInfo In PI
            If NewFitClone.GetType.GetProperty(p.Name) IsNot Nothing Then
                Dim FitPI As System.Reflection.PropertyInfo = NewFitClone.GetType().GetProperty(p.Name)
                If p.CanWrite Then
                    p.SetValue(NewFit, FitPI.GetValue(NewFitClone, Nothing), Nothing)
                End If
            End If
        Next

        Return NewFit

    End Function

End Class

Class FittingEffectComparer
    Implements IComparer

    ' maintain a reference to the 2-dimensional array being sorted
    Private ReadOnly _sortArray(,) As Double

    ' constructor initializes the sortArray reference
    Public Sub New(ByVal theArray(,) As Double)
        _sortArray = theArray
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        ' x and y are integer row numbers into the sortArray
        Dim i1 As Double = CDbl(DirectCast(x, Integer))
        Dim i2 As Double = CDbl(DirectCast(y, Integer))
        ' compare the items in the sortArray
        Return _sortArray(CInt(i1), 1).CompareTo(_sortArray(CInt(i2), 1))
    End Function
End Class
