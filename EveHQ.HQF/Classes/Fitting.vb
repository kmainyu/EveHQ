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
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

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
            If cDamageProfileName <> "" Then
                If DamageProfiles.ProfileList.ContainsKey(cDamageProfileName) = False Then
                    cDamageProfileName = "<Omni-Damage>"
                End If
                Dim curProfile As DamageProfile = CType(DamageProfiles.ProfileList.Item(cDamageProfileName), DamageProfile)
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

#End Region

#Region "Fitting Mapping Collections"
    Private SkillEffectsTable As New SortedList(Of String, List(Of FinalEffect))
    Private BaseSkillEffectsTable As New SortedList(Of String, List(Of FinalEffect))
    Private ModuleEffectsTable As New SortedList(Of String, List(Of FinalEffect))
    Private ChargeEffectsTable As New SortedList(Of String, List(Of FinalEffect))
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
    ''' <param name="BuildMethod"></param>
    ''' <remarks></remarks>
    Public Sub ApplyFitting(Optional ByVal BuildMethod As BuildType = BuildType.BuildEverything)
        ' Update the pilot from the pilot name
        If HQFPilotCollection.HQFPilots.ContainsKey(Me.PilotName) = False Then
            MessageBox.Show("The pilot '" & Me.PilotName & "' is not a listed pilot - please check this pilot exists.", "Unknown Pilot", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.cFittedShip = Me.BaseShip
        End If
        Dim baseShip As Ship = Me.BaseShip
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(Me.PilotName), HQFPilot)

        ' Setup performance info - just in case!
        Dim stages As Integer = 20
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
        pStages(9) = "Build Charge Effects: "
        pStages(10) = "Applying Charge Effects to Modules: "
        pStages(11) = "Applying Charge Effects to Ship: "
        pStages(12) = "Building Module Effects: "
        pStages(13) = "Applying Stacking Penalties: "
        pStages(14) = "Applying Module Effects to Modules: "
        pStages(15) = "Rebuilding Module Effects: "
        pStages(16) = "Recalculating Stacking Penalties: "
        pStages(17) = "Applying Module Effects to Drones: "
        pStages(18) = "Applying Module Effects to Ship: "
        pStages(19) = "Calculating Damage Statistics: "
        pStages(20) = "Calculating Defence Statistics: "
        ' Apply the pilot skills to the ship
        Dim newShip As New Ship
        Select Case BuildMethod
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
                Me.BuildChargeEffects(newShip)
                pStageTime(9) = Now
                Me.ApplyChargeEffectsToModules(newShip)
                pStageTime(10) = Now
                Me.ApplyChargeEffectsToShip(newShip)
                pStageTime(11) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(12) = Now
                Me.ApplyStackingPenalties()
                pStageTime(13) = Now
                Me.ApplyModuleEffectsToModules(newShip)
                pStageTime(14) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(15) = Now
                Me.ApplyStackingPenalties()
                pStageTime(16) = Now
                Me.ApplyModuleEffectsToDrones(newShip)
                pStageTime(17) = Now
                Me.ApplyModuleEffectsToShip(newShip)
                pStageTime(18) = Now
                Me.CalculateDamageStatistics(newShip)
                pStageTime(19) = Now
                Ship.MapShipAttributes(newShip)
                Me.CalculateDefenceStatistics(newShip)
                pStageTime(20) = Now
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
                'Me.ApplyModuleEffectsToModules(newShip)
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
                'Me.ApplyModuleEffectsToDrones(newShip)
                pStageTime(17) = Now
                'Me.ApplyModuleEffectsToShip(newShip)
                pStageTime(18) = Now
                'Me.CalculateDamageStatistics(newShip)
                pStageTime(19) = Now
                'Ship.MapShipAttributes(newShip)
                'Me.CalculateDefenceStatistics(newShip)
                pStageTime(20) = Now
            Case BuildType.BuildFromEffectsMaps
                pStageTime(0) = Now
                'Me.BuildSkillEffects(shipPilot)
                pStageTime(1) = Now
                'Me.BuildImplantEffects(shipPilot)
                pStageTime(2) = Now
                'Me.BuildShipEffects(shipPilot, baseShip)
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
                Me.BuildChargeEffects(newShip)
                pStageTime(9) = Now
                Me.ApplyChargeEffectsToModules(newShip)
                pStageTime(10) = Now
                Me.ApplyChargeEffectsToShip(newShip)
                pStageTime(11) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(12) = Now
                Me.ApplyStackingPenalties()
                pStageTime(13) = Now
                Me.ApplyModuleEffectsToModules(newShip)
                pStageTime(14) = Now
                Me.BuildModuleEffects(newShip)
                pStageTime(15) = Now
                Me.ApplyStackingPenalties()
                pStageTime(16) = Now
                Me.ApplyModuleEffectsToDrones(newShip)
                pStageTime(17) = Now
                Me.ApplyModuleEffectsToShip(newShip)
                pStageTime(18) = Now
                Me.CalculateDamageStatistics(newShip)
                pStageTime(19) = Now
                Ship.MapShipAttributes(newShip)
                Me.CalculateDefenceStatistics(newShip)
                pStageTime(20) = Now
        End Select
        If Settings.HQFSettings.ShowPerformanceData = True Then
            Dim dTime As TimeSpan
            Dim perfMsg As String = ""
            For stage As Integer = 1 To stages
                perfMsg &= pStages(stage)
                dTime = pStageTime(stage) - pStageTime(stage - 1)
                perfMsg &= FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms" & ControlChars.CrLf
            Next
            dTime = pStageTime(stages) - pStageTime(0)
            perfMsg &= "Total Time: " & FormatNumber(dTime.TotalMilliseconds, 2, TriState.True, TriState.True, TriState.True) & "ms" & ControlChars.CrLf
            MessageBox.Show(perfMsg, "Performance Data Results: Method " & BuildMethod, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Ship.MapShipAttributes(newShip)
        cFittedShip = newShip

        If Me.ShipSlotCtrl IsNot Nothing Then
            'Dim SSC As New Threading.Thread(AddressOf Me.ShipSlotCtrl.UpdateAllSlotLocations)
            'SSC.Priority = Threading.ThreadPriority.Highest
            'SSC.Start()
            Me.ShipSlotCtrl.UpdateAllSlotLocations()
        End If

        If Me.ShipInfoCtrl IsNot Nothing Then
            'Dim SIC As New Threading.Thread(AddressOf Me.ShipInfoCtrl.UpdateInfoDisplay)
            'SIC.Priority = Threading.ThreadPriority.Highest
            'SIC.Start()
            Me.ShipInfoCtrl.UpdateInfoDisplay()
        End If

    End Sub

#End Region

#Region "Private Fitting Routines"
    Private Sub BuildSkillEffects(ByVal hPilot As HQFPilot)
        ' Clear the Effects Table
        SkillEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim aSkill As New Skill
        Dim fEffect As New FinalEffect
        Dim fEffectList As New List(Of FinalEffect)
        For Each hSkill As HQFSkill In hPilot.SkillSet
            If SkillLists.SkillList.ContainsKey(hSkill.ID) = True Then
                If hSkill.Level <> 0 Then
                    ' Go through the attributes
                    aSkill = CType(SkillLists.SkillList(hSkill.ID), Skill)
                    Try
                        For Each att As String In aSkill.Attributes.Keys
                            If Engine.EffectsMap.Contains(att) = True Then
                                For Each chkEffect As Effect In CType(Engine.EffectsMap(att), ArrayList)
                                    If chkEffect.AffectingType = EffectType.Item And chkEffect.AffectingID = CInt(aSkill.ID) Then
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
                                        If SkillEffectsTable.ContainsKey(fEffect.AffectedAtt.ToString) = False Then
                                            fEffectList = New List(Of FinalEffect)
                                            SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                        Else
                                            fEffectList = SkillEffectsTable(fEffect.AffectedAtt.ToString)
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
    Private Sub BuildImplantEffects(ByVal hPilot As HQFPilot)
        ' Run through the implants and see if we have any pirate implants
        Dim hImplant As String = ""
        Dim aImplant As ShipModule
        Dim PIGroup As String = ""
        Dim cPirateImplantGroups As SortedList = CType(Engine.PirateImplantGroups.Clone, Collections.SortedList)
        For slotNo As Integer = 1 To 10
            hImplant = hPilot.ImplantName(slotNo)
            If Engine.PirateImplants.ContainsKey(hImplant) = True Then
                ' We have a pirate implant so let's work out the group and the set bonus
                PIGroup = CStr(Engine.PirateImplants.Item(hImplant))
                aImplant = CType(ModuleLists.moduleList(ModuleLists.moduleListName(hImplant)), ShipModule)
                Select Case PIGroup
                    Case "Centurion", "Low-grade Centurion"
                        cPirateImplantGroups.Item("Centurion") = CDbl(cPirateImplantGroups.Item("Centurion")) * CDbl(aImplant.Attributes("1293"))
                        cPirateImplantGroups.Item("Low-grade Centurion") = CDbl(cPirateImplantGroups.Item("Low-grade Centurion")) * CDbl(aImplant.Attributes("1293"))
                    Case "Crystal", "Low-grade Crystal"
                        cPirateImplantGroups.Item("Crystal") = CDbl(cPirateImplantGroups.Item("Crystal")) * CDbl(aImplant.Attributes("838"))
                        cPirateImplantGroups.Item("Low-grade Crystal") = CDbl(cPirateImplantGroups.Item("Low-grade Crystal")) * CDbl(aImplant.Attributes("838"))
                    Case "Edge", "Low-grade Edge"
                        cPirateImplantGroups.Item("Edge") = CDbl(cPirateImplantGroups.Item("Edge")) * CDbl(aImplant.Attributes("1291"))
                        cPirateImplantGroups.Item("Low-grade Edge") = CDbl(cPirateImplantGroups.Item("Low-grade Edge")) * CDbl(aImplant.Attributes("1291"))
                    Case "Grail"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1550"))
                    Case "Low-grade Grail"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1569"))
                    Case "Halo", "Low-grade Halo"
                        cPirateImplantGroups.Item("Halo") = CDbl(cPirateImplantGroups.Item("Halo")) * CDbl(aImplant.Attributes("863"))
                        cPirateImplantGroups.Item("Low-grade Halo") = CDbl(cPirateImplantGroups.Item("Low-grade Halo")) * CDbl(aImplant.Attributes("863"))
                    Case "Harvest", "Low-grade Harvest"
                        cPirateImplantGroups.Item("Harvest") = CDbl(cPirateImplantGroups.Item("Harvest")) * CDbl(aImplant.Attributes("1292"))
                        cPirateImplantGroups.Item("Low-grade Harvest") = CDbl(cPirateImplantGroups.Item("Low-grade Harvest")) * CDbl(aImplant.Attributes("1292"))
                    Case "Jackal"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1554"))
                    Case "Low-grade Jackal"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1572"))
                    Case "Nomad", "Low-grade Nomad"
                        cPirateImplantGroups.Item("Nomad") = CDbl(cPirateImplantGroups.Item("Nomad")) * CDbl(aImplant.Attributes("1282"))
                        cPirateImplantGroups.Item("Low-grade Nomad") = CDbl(cPirateImplantGroups.Item("Low-grade Nomad")) * CDbl(aImplant.Attributes("1282"))
                    Case "Slave", "Low-grade Slave"
                        cPirateImplantGroups.Item("Slave") = CDbl(cPirateImplantGroups.Item("Slave")) * CDbl(aImplant.Attributes("864"))
                        cPirateImplantGroups.Item("Low-grade Slave") = CDbl(cPirateImplantGroups.Item("Low-grade Slave")) * CDbl(aImplant.Attributes("864"))
                    Case "Snake", "Low-grade Snake"
                        cPirateImplantGroups.Item("Snake") = CDbl(cPirateImplantGroups.Item("Snake")) * CDbl(aImplant.Attributes("802"))
                        cPirateImplantGroups.Item("Low-grade Snake") = CDbl(cPirateImplantGroups.Item("Low-grade Snake")) * CDbl(aImplant.Attributes("802"))
                    Case "Spur"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1553"))
                    Case "Low-grade Spur"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1570"))
                    Case "Talisman", "Low-grade Talisman"
                        cPirateImplantGroups.Item("Talisman") = CDbl(cPirateImplantGroups.Item("Talisman")) * CDbl(aImplant.Attributes("799"))
                        cPirateImplantGroups.Item("Low-grade Talisman") = CDbl(cPirateImplantGroups.Item("Low-grade Talisman")) * CDbl(aImplant.Attributes("799"))
                    Case "Talon"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1552"))
                    Case "Low-grade Talon"
                        cPirateImplantGroups.Item(PIGroup) = CDbl(cPirateImplantGroups.Item(PIGroup)) * CDbl(aImplant.Attributes("1571"))
                    Case "Virtue", "Low-grade Virtue"
                        cPirateImplantGroups.Item("Virtue") = CDbl(cPirateImplantGroups.Item("Virtue")) * CDbl(aImplant.Attributes("1284"))
                        cPirateImplantGroups.Item("Low-grade Virtue") = CDbl(cPirateImplantGroups.Item("Low-grade Virtue")) * CDbl(aImplant.Attributes("1284"))
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
                                If aImplant.Attributes.ContainsKey(chkEffect.AffectingAtt.ToString) = True Then
                                    fEffect = New FinalEffect
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    fEffect.AffectedID = chkEffect.AffectedID
                                    If Engine.PirateImplants.ContainsKey(aImplant.Name) = True Then
                                        PIGroup = CStr(Engine.PirateImplants.Item(hImplant))
                                        fEffect.AffectedValue = CDbl(aImplant.Attributes(chkEffect.AffectingAtt.ToString)) * CDbl(cPirateImplantGroups.Item(PIGroup))
                                        fEffect.Cause = aImplant.Name & " (Set Bonus: " & FormatNumber(cPirateImplantGroups.Item(PIGroup), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "x)"
                                    Else
                                        fEffect.AffectedValue = CDbl(aImplant.Attributes(chkEffect.AffectingAtt.ToString))
                                        fEffect.Cause = aImplant.Name
                                    End If
                                    fEffect.StackNerf = 0
                                    fEffect.CalcType = chkEffect.CalcType
                                    If SkillEffectsTable.ContainsKey(fEffect.AffectedAtt.ToString) = False Then
                                        fEffectList = New List(Of FinalEffect)
                                        SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                    Else
                                        fEffectList = SkillEffectsTable(fEffect.AffectedAtt.ToString)
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
    Private Sub BuildShipBonuses(ByVal hPilot As HQFPilot, ByVal hShip As Ship)
        If hShip IsNot Nothing Then
            ' Go through all the skills and see what needs to be mapped
            Dim shipRoles As New List(Of ShipEffect)
            Dim hSkill As New HQFSkill
            Dim fEffect As New FinalEffect
            Dim fEffectList As New List(Of FinalEffect)
            If Engine.ShipBonusesMap.ContainsKey(hShip.ID) = True Then
                shipRoles = Engine.ShipBonusesMap(hShip.ID)
                If shipRoles IsNot Nothing Then
                    For Each chkEffect As ShipEffect In shipRoles
                        If chkEffect.Status <> 16 Then
                            fEffect = New FinalEffect
                            If hPilot.SkillSet.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))) = True Then
                                hSkill = CType(hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))), HQFSkill)
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
                            If SkillEffectsTable.ContainsKey(fEffect.AffectedAtt.ToString) = False Then
                                fEffectList = New List(Of FinalEffect)
                                SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                            Else
                                fEffectList = SkillEffectsTable(fEffect.AffectedAtt.ToString)
                            End If
                            fEffectList.Add(fEffect)
                        End If
                    Next
                End If
            End If
            ' Get the ship effects
            Dim processData As Boolean = False
            For Each att As String In hShip.Attributes.Keys
                If Engine.ShipEffectsMap.Contains(att) = True Then
                    For Each chkEffect As Effect In CType(Engine.ShipEffectsMap(att), ArrayList)
                        processData = False
                        Select Case chkEffect.AffectingType
                            Case EffectType.All
                                processData = True
                            Case EffectType.Item
                                If chkEffect.AffectingID.ToString = hShip.ID Then
                                    processData = True
                                End If
                            Case EffectType.Group
                                If chkEffect.AffectingID.ToString = hShip.DatabaseGroup Then
                                    processData = True
                                End If
                            Case EffectType.Category
                                If chkEffect.AffectingID.ToString = hShip.DatabaseCategory Then
                                    processData = True
                                End If
                            Case EffectType.MarketGroup
                                If chkEffect.AffectingID.ToString = hShip.MarketGroup Then
                                    processData = True
                                End If
                            Case EffectType.Skill
                                If hShip.RequiredSkills.Contains(chkEffect.AffectingID.ToString) Then
                                    processData = True
                                End If
                            Case EffectType.Slot
                                processData = True
                            Case EffectType.Attribute
                                If hShip.Attributes.ContainsKey(chkEffect.AffectingID.ToString) Then
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
                            If SkillEffectsTable.ContainsKey(fEffect.AffectedAtt.ToString) = False Then
                                fEffectList = New List(Of FinalEffect)
                                SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                            Else
                                fEffectList = SkillEffectsTable(fEffect.AffectedAtt.ToString)
                            End If
                            fEffectList.Add(fEffect)
                        End If
                    Next
                End If
            Next
            ' Get the bonuses from the subsystems
            If hShip.SubSlots_Used > 0 Then
                For slot As Integer = 1 To hShip.SubSlots
                    If hShip.SubSlot(slot) IsNot Nothing Then
                        shipRoles = Engine.SubSystemEffectsMap(hShip.SubSlot(slot).ID)
                        If shipRoles IsNot Nothing Then
                            For Each chkEffect As ShipEffect In shipRoles
                                If chkEffect.Status <> 16 Then
                                    fEffect = New FinalEffect
                                    If hPilot.SkillSet.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))) = True Then
                                        hSkill = CType(hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))), HQFSkill)
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
                                    If SkillEffectsTable.ContainsKey(fEffect.AffectedAtt.ToString) = False Then
                                        fEffectList = New List(Of FinalEffect)
                                        SkillEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                    Else
                                        fEffectList = SkillEffectsTable(fEffect.AffectedAtt.ToString)
                                    End If
                                    fEffectList.Add(fEffect)
                                End If
                            Next
                        End If
                    End If
                Next
            End If
            BaseSkillEffectsTable = CloneEffectList(SkillEffectsTable)
        End If
    End Sub
    Private Sub BuildChargeEffects(ByRef newShip As Ship)
        ' Clear the Effects Table
        ChargeEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim fEffect As New FinalEffect
        Dim fEffectList As New List(Of FinalEffect)
        Dim processData As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
            If aModule.LoadedCharge IsNot Nothing Then
                For Each att As String In aModule.LoadedCharge.Attributes.Keys
                    If Engine.EffectsMap.Contains(att) = True Then
                        For Each chkEffect As Effect In CType(Engine.EffectsMap(att), ArrayList)
                            processData = False
                            Select Case chkEffect.AffectingType
                                Case EffectType.All
                                    processData = True
                                Case EffectType.Item
                                    If chkEffect.AffectingID.ToString = aModule.LoadedCharge.ID Then
                                        processData = True
                                    End If
                                Case EffectType.Group
                                    If chkEffect.AffectingID.ToString = aModule.LoadedCharge.DatabaseGroup Then
                                        processData = True
                                    End If
                                Case EffectType.Category
                                    If chkEffect.AffectingID.ToString = aModule.LoadedCharge.DatabaseCategory Then
                                        processData = True
                                    End If
                                Case EffectType.MarketGroup
                                    If chkEffect.AffectingID.ToString = aModule.LoadedCharge.MarketGroup Then
                                        processData = True
                                    End If
                                Case EffectType.Skill
                                    If aModule.LoadedCharge.RequiredSkills.Contains(chkEffect.AffectingID.ToString) Then
                                        processData = True
                                    End If
                                Case EffectType.Slot
                                    processData = True
                                Case EffectType.Attribute
                                    If aModule.LoadedCharge.Attributes.ContainsKey(chkEffect.AffectingID.ToString) Then
                                        processData = True
                                    End If
                            End Select
                            If processData = True And (aModule.LoadedCharge.ModuleState And chkEffect.Status) = aModule.LoadedCharge.ModuleState Then
                                fEffect = New FinalEffect
                                fEffect.AffectedAtt = chkEffect.AffectedAtt
                                fEffect.AffectedType = chkEffect.AffectedType
                                If chkEffect.AffectedType = EffectType.Slot Then
                                    fEffect.AffectedID.Add(aModule.SlotType & aModule.SlotNo)
                                Else
                                    fEffect.AffectedID = chkEffect.AffectedID
                                End If
                                fEffect.AffectedValue = CDbl(aModule.LoadedCharge.Attributes(att))
                                fEffect.StackNerf = chkEffect.StackNerf
                                fEffect.Cause = aModule.LoadedCharge.Name
                                fEffect.CalcType = chkEffect.CalcType
                                If ChargeEffectsTable.ContainsKey(fEffect.AffectedAtt.ToString) = False Then
                                    fEffectList = New List(Of FinalEffect)
                                    ChargeEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                Else
                                    fEffectList = ChargeEffectsTable(fEffect.AffectedAtt.ToString)
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
        ModuleEffectsTable.Clear()
        ' Go through all the skills and see what needs to be mapped
        Dim fEffect As New FinalEffect
        Dim fEffectList As New List(Of FinalEffect)
        Dim processData As Boolean = False
        For Each aModule As ShipModule In newShip.SlotCollection
            For Each att As String In aModule.Attributes.Keys
                If Engine.EffectsMap.Contains(att) = True Then
                    For Each chkEffect As Effect In CType(Engine.EffectsMap(att), ArrayList)
                        processData = False
                        Select Case chkEffect.AffectingType
                            Case EffectType.All
                                processData = True
                            Case EffectType.Item
                                If chkEffect.AffectingID.ToString = aModule.ID Then
                                    processData = True
                                End If
                            Case EffectType.Group
                                If chkEffect.AffectingID.ToString = aModule.DatabaseGroup Then
                                    processData = True
                                End If
                            Case EffectType.Category
                                If chkEffect.AffectingID.ToString = aModule.DatabaseCategory Then
                                    processData = True
                                End If
                            Case EffectType.MarketGroup
                                If chkEffect.AffectingID.ToString = aModule.MarketGroup Then
                                    processData = True
                                End If
                            Case EffectType.Skill
                                If aModule.RequiredSkills.Contains(chkEffect.AffectingID.ToString) Then
                                    processData = True
                                End If
                            Case EffectType.Slot
                                processData = True
                            Case EffectType.Attribute
                                If aModule.Attributes.ContainsKey(chkEffect.AffectingID.ToString) Then
                                    processData = True
                                End If
                        End Select
                        If processData = True And (aModule.ModuleState And chkEffect.Status) = aModule.ModuleState Then
                            fEffect = New FinalEffect
                            fEffect.AffectedAtt = chkEffect.AffectedAtt
                            fEffect.AffectedType = chkEffect.AffectedType
                            If chkEffect.AffectedType = EffectType.Slot Then
                                fEffect.AffectedID.Add(aModule.SlotType & aModule.SlotNo)
                            Else
                                fEffect.AffectedID = chkEffect.AffectedID
                            End If
                            fEffect.AffectedValue = aModule.Attributes(att)
                            fEffect.StackNerf = chkEffect.StackNerf
                            fEffect.Cause = aModule.Name
                            fEffect.CalcType = chkEffect.CalcType
                            If ModuleEffectsTable.ContainsKey(fEffect.AffectedAtt.ToString) = False Then
                                fEffectList = New List(Of FinalEffect)
                                ModuleEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                            Else
                                fEffectList = ModuleEffectsTable(fEffect.AffectedAtt.ToString)
                            End If
                            fEffectList.Add(fEffect)
                        End If
                    Next
                End If
            Next
        Next
    End Sub
    Public Function BuildSubSystemEffects(ByVal cShip As Ship) As Ship

        Dim newShip As Ship = CType(ShipLists.shipList(cShip.Name), Ship).Clone

        If newShip.SubSlots > 0 Then
            For slot As Integer = 1 To newShip.SubSlots
                newShip.SubSlot(slot) = cShip.SubSlot(slot)
            Next
        End If

        ' Clear the Effects Table
        Dim SSEffectsTable As New SortedList
        ' Go through all the skills and see what needs to be mapped
        Dim att As String = ""
        Dim attData As New ArrayList
        Dim fEffect As New FinalEffect
        Dim fEffectList As New ArrayList
        Dim aModule As New ShipModule
        Dim processData As Boolean = False
        If newShip.SubSlots > 0 Then
            For s As Integer = 1 To newShip.SubSlots
                aModule = newShip.SubSlot(s)
                If aModule IsNot Nothing Then
                    For Each att In aModule.Attributes.Keys
                        If Engine.EffectsMap.Contains(att) = True Then
                            For Each chkEffect As Effect In CType(Engine.EffectsMap(att), ArrayList)
                                processData = False
                                Select Case chkEffect.AffectingType
                                    Case EffectType.All
                                        processData = True
                                    Case EffectType.Item
                                        If chkEffect.AffectingID.ToString = aModule.ID Then
                                            processData = True
                                        End If
                                    Case EffectType.Group
                                        If chkEffect.AffectingID.ToString = aModule.DatabaseGroup Then
                                            processData = True
                                        End If
                                    Case EffectType.Category
                                        If chkEffect.AffectingID.ToString = aModule.DatabaseCategory Then
                                            processData = True
                                        End If
                                    Case EffectType.MarketGroup
                                        If chkEffect.AffectingID.ToString = aModule.MarketGroup Then
                                            processData = True
                                        End If
                                    Case EffectType.Skill
                                        If aModule.RequiredSkills.Contains(chkEffect.AffectingID.ToString) Then
                                            processData = True
                                        End If
                                    Case EffectType.Slot
                                        processData = True
                                    Case EffectType.Attribute
                                        If aModule.Attributes.ContainsKey(chkEffect.AffectingID.ToString) Then
                                            processData = True
                                        End If
                                End Select
                                If processData = True And (aModule.ModuleState And chkEffect.Status) = aModule.ModuleState Then
                                    fEffect = New FinalEffect
                                    fEffect.AffectedAtt = chkEffect.AffectedAtt
                                    fEffect.AffectedType = chkEffect.AffectedType
                                    If chkEffect.AffectedType = EffectType.Slot Then
                                        fEffect.AffectedID.Add(aModule.SlotType & aModule.SlotNo)
                                    Else
                                        fEffect.AffectedID = chkEffect.AffectedID
                                    End If
                                    fEffect.AffectedValue = aModule.Attributes(att)
                                    fEffect.StackNerf = chkEffect.StackNerf
                                    fEffect.Cause = aModule.Name
                                    fEffect.CalcType = chkEffect.CalcType
                                    If SSEffectsTable.Contains(fEffect.AffectedAtt.ToString) = False Then
                                        fEffectList = New ArrayList
                                        SSEffectsTable.Add(fEffect.AffectedAtt.ToString, fEffectList)
                                    Else
                                        fEffectList = CType(SSEffectsTable(fEffect.AffectedAtt.ToString), Collections.ArrayList)
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
            If SSEffectsTable.Contains(att) = True Then
                For Each fEffect In CType(SSEffectsTable(att), ArrayList)
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
        newShip.CargoBayItems = CType(cShip.CargoBayItems.Clone, Collections.SortedList)
        newShip.CargoBay_Used = cShip.CargoBay_Used
        newShip.DroneBayItems = CType(cShip.DroneBayItems.Clone, Collections.SortedList)
        newShip.DroneBay_Used = cShip.DroneBay_Used
        newShip.ShipBayItems = CType(cShip.ShipBayItems.Clone, Collections.SortedList)
        newShip.ShipBay_Used = cShip.ShipBay_Used
        newShip.FleetSlotCollection = CType(cShip.FleetSlotCollection.Clone, ArrayList)
        newShip.RemoteSlotCollection = CType(cShip.RemoteSlotCollection.Clone, ArrayList)
        newShip.EnviroSlotCollection = CType(cShip.EnviroSlotCollection.Clone, ArrayList)
        newShip.BoosterSlotCollection = CType(cShip.BoosterSlotCollection.Clone, ArrayList)
        If cShip.DamageProfile IsNot Nothing Then
            newShip.DamageProfile = cShip.DamageProfile
        Else
            newShip.DamageProfile = CType(DamageProfiles.ProfileList("<Omni-Damage>"), DamageProfile)
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
                modName = Me.cWHEffect & " Effect Beacon Class " & Me.cWHLevel.ToString
            End If
            Dim modID As String = CStr(ModuleLists.moduleListName(modName))
            Dim eMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
            Me.BaseShip.EnviroSlotCollection.Add(eMod)
        End If

    End Sub
    Private Function CollectModules(ByVal newShip As Ship) As Ship
        newShip.SlotCollection.Clear()
        For Each RemoteObject As Object In newShip.RemoteSlotCollection
            If TypeOf RemoteObject Is ShipModule Then
                newShip.SlotCollection.Add(RemoteObject)
            Else
                Dim remoteDrones As DroneBayItem = CType(RemoteObject, DroneBayItem)
                For drone As Integer = 1 To remoteDrones.Quantity
                    newShip.SlotCollection.Add(remoteDrones.DroneType)
                Next
            End If
        Next
        For Each FleetObject As Object In newShip.FleetSlotCollection
            If TypeOf FleetObject Is ShipModule Then
                newShip.SlotCollection.Add(FleetObject)
            End If
        Next
        For Each EnviroObject As Object In newShip.EnviroSlotCollection
            If TypeOf EnviroObject Is ShipModule Then
                newShip.SlotCollection.Add(EnviroObject)
            End If
        Next
        For Each BoosterObject As Object In newShip.BoosterSlotCollection
            If TypeOf BoosterObject Is ShipModule Then
                newShip.SlotCollection.Add(BoosterObject)
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
                If HQF.Settings.HQFSettings.IncludeCapReloadTime = True And newShip.MidSlot(slot).DatabaseGroup = "76" Then
                    Dim cModule As ShipModule = newShip.MidSlot(slot)
                    If cModule.LoadedCharge IsNot Nothing Then
                        Dim reloadEffect As Double = 10 / (CInt(Int(cModule.Capacity / cModule.LoadedCharge.Volume)))
                        cModule.Attributes("73") += reloadEffect
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
        newShip.Attributes("10063") = 1
        If newShip.Attributes.ContainsKey("435") = True Then
            newShip.Attributes("10063") = newShip.Attributes("10063") + Me.BaseShip.Attributes("435")
        End If
        Return newShip
    End Function
    Private Sub ApplySkillEffectsToShip(ByRef newShip As Ship)
        Dim att As String = ""
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = newShip.Attributes.Keys(attNo)
            If SkillEffectsTable.ContainsKey(att) = True Then
                For Each fEffect As FinalEffect In SkillEffectsTable(att)
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
        Dim att As String = ""
        If aModule.ModuleState < 16 Then
            For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                att = aModule.Attributes.Keys(attNo)
                If SkillEffectsTable.ContainsKey(att) = True Then
                    For Each fEffect As FinalEffect In SkillEffectsTable(att)
                        If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                            Call ApplyFinalEffectToModule(aModule, fEffect, att)
                        End If
                    Next
                End If
            Next
            If aModule.LoadedCharge IsNot Nothing Then
                For attNo As Integer = 0 To aModule.LoadedCharge.Attributes.Keys.Count - 1
                    att = aModule.LoadedCharge.Attributes.Keys(attNo)
                    If SkillEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In SkillEffectsTable(att)
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
        Dim att As String = ""
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            aModule = DBI.DroneType
            If aModule.ModuleState < 16 Then
                For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                    att = aModule.Attributes.Keys(attNo)
                    If SkillEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In SkillEffectsTable(att)
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
        Dim att As String = ""
        For Each aModule As ShipModule In newShip.SlotCollection
            If aModule.ModuleState < 16 Then
                For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                    att = aModule.Attributes.Keys(attNo)
                    If ChargeEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In ChargeEffectsTable(att)
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
        Dim att As String = ""
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = newShip.Attributes.Keys(attNo)
            If ChargeEffectsTable.ContainsKey(att) = True Then
                For Each fEffect As FinalEffect In ChargeEffectsTable(att)
                    If ProcessFinalEffectForShip(newShip, fEffect) = True Then
                        Call ApplyFinalEffectToShip(newShip, fEffect, att)
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub ApplyStackingPenalties()
        Call Me.PrioritiseEffects()
        Dim sTime, eTime As Date
        sTime = Now
        Dim baseEffectList As New List(Of FinalEffect)
        Dim finalEffectList As New List(Of FinalEffect)
        Dim tempPEffectList As New SortedList
        Dim tempNEffectList As New SortedList
        Dim groupPEffectList As New SortedList
        Dim groupNEffectList As New SortedList
        Dim attOrder(,) As Double
        Dim att As String
        For attNumber As Integer = 0 To ModuleEffectsTable.Keys.Count - 1
            att = ModuleEffectsTable.Keys(attNumber)
            baseEffectList = ModuleEffectsTable(att)
            tempPEffectList.Clear() : tempNEffectList.Clear()
            groupPEffectList.Clear() : groupNEffectList.Clear()
            finalEffectList = New List(Of FinalEffect)
            For Each fEffect As FinalEffect In baseEffectList
                Select Case fEffect.StackNerf
                    Case EffectStackType.None
                        finalEffectList.Add(fEffect)
                    Case EffectStackType.Standard
                        If fEffect.AffectedValue >= 0 Then
                            tempPEffectList.Add(tempPEffectList.Count.ToString, fEffect)
                        Else
                            tempNEffectList.Add(tempNEffectList.Count.ToString, fEffect)
                        End If
                    Case EffectStackType.Group
                        If fEffect.AffectedValue >= 0 Then
                            groupPEffectList.Add(groupPEffectList.Count.ToString, fEffect)
                        Else
                            groupNEffectList.Add(groupNEffectList.Count.ToString, fEffect)
                        End If
                End Select
            Next
            If tempPEffectList.Count > 0 Then
                ReDim attOrder(tempPEffectList.Count - 1, 1)
                Dim sEffect As FinalEffect
                For Each attNo As String In tempPEffectList.Keys
                    sEffect = CType(tempPEffectList(attNo), FinalEffect)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sEffect.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(tempPEffectList.Count - 1) As Integer
                For a As Integer = 0 To tempPEffectList.Count - 1
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New EveHQ.Core.Reports.ArrayComparerDouble(attOrder)
                Array.Sort(tagArray, myComparer)
                Array.Reverse(tagArray)
                ' Go through the data and apply the stacking penalty
                Dim idx As Integer = 0
                Dim penalty As Double = 0
                For i As Integer = 0 To tagArray.Length - 1
                    idx = tagArray(i)
                    sEffect = CType(tempPEffectList(idx.ToString), FinalEffect)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    Select Case sEffect.CalcType
                        Case EffectCalcType.Multiplier
                            sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                        Case Else
                            sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    End Select
                    sEffect.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalEffectList.Add(sEffect)
                Next
            End If
            If tempNEffectList.Count > 0 Then
                ReDim attOrder(tempNEffectList.Count - 1, 1)
                Dim sEffect As FinalEffect
                For Each attNo As String In tempNEffectList.Keys
                    sEffect = CType(tempNEffectList(attNo), FinalEffect)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sEffect.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(tempNEffectList.Count - 1) As Integer
                For a As Integer = 0 To tempNEffectList.Count - 1
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New EveHQ.Core.Reports.ArrayComparerDouble(attOrder)
                Array.Sort(tagArray, myComparer)
                ' Go through the data and apply the stacking penalty
                Dim idx As Integer = 0
                Dim penalty As Double = 0
                For i As Integer = 0 To tagArray.Length - 1
                    idx = tagArray(i)
                    sEffect = CType(tempNEffectList(idx.ToString), FinalEffect)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    Select Case sEffect.CalcType
                        Case EffectCalcType.Multiplier
                            sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                        Case Else
                            sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    End Select
                    sEffect.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalEffectList.Add(sEffect)
                Next
            End If
            If groupPEffectList.Count > 0 Then
                ReDim attOrder(groupPEffectList.Count - 1, 1)
                Dim sEffect As FinalEffect
                For Each attNo As String In groupPEffectList.Keys
                    sEffect = CType(groupPEffectList(attNo), FinalEffect)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sEffect.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(groupPEffectList.Count - 1) As Integer
                For a As Integer = 0 To groupPEffectList.Count - 1
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New EveHQ.Core.Reports.ArrayComparerDouble(attOrder)
                Array.Sort(tagArray, myComparer)
                Array.Reverse(tagArray)
                ' Go through the data and apply the stacking penalty
                Dim idx As Integer = 0
                Dim penalty As Double = 0
                For i As Integer = 0 To tagArray.Length - 1
                    idx = tagArray(i)
                    sEffect = CType(groupPEffectList(idx.ToString), FinalEffect)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    Select Case sEffect.CalcType
                        Case EffectCalcType.Multiplier
                            sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                        Case Else
                            sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    End Select
                    sEffect.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalEffectList.Add(sEffect)
                Next
            End If
            If groupNEffectList.Count > 0 Then
                ReDim attOrder(groupNEffectList.Count - 1, 1)
                Dim sEffect As FinalEffect
                For Each attNo As String In groupNEffectList.Keys
                    sEffect = CType(groupNEffectList(attNo), FinalEffect)
                    attOrder(CInt(attNo), 0) = CDbl(attNo)
                    attOrder(CInt(attNo), 1) = sEffect.AffectedValue
                Next
                ' Create a tag array ready to sort the skill times
                Dim tagArray(groupNEffectList.Count - 1) As Integer
                For a As Integer = 0 To groupNEffectList.Count - 1
                    tagArray(a) = a
                Next
                ' Initialize the comparer and sort
                Dim myComparer As New EveHQ.Core.Reports.ArrayComparerDouble(attOrder)
                Array.Sort(tagArray, myComparer)
                ' Go through the data and apply the stacking penalty
                Dim idx As Integer = 0
                Dim penalty As Double = 0
                For i As Integer = 0 To tagArray.Length - 1
                    idx = tagArray(i)
                    sEffect = CType(groupNEffectList(idx.ToString), FinalEffect)
                    penalty = Math.Exp(-(i ^ 2 / 7.1289))
                    Select Case sEffect.CalcType
                        Case EffectCalcType.Multiplier
                            sEffect.AffectedValue = ((sEffect.AffectedValue - 1) * penalty) + 1
                        Case Else
                            sEffect.AffectedValue = sEffect.AffectedValue * penalty
                    End Select
                    sEffect.Cause &= " (Stacking - " & FormatNumber(penalty * 100, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%)"
                    finalEffectList.Add(sEffect)
                Next
            End If
            ModuleEffectsTable(att) = finalEffectList
        Next
        eTime = Now
        Dim dTime As TimeSpan = eTime - sTime
    End Sub
    Private Sub PrioritiseEffects()
        Dim baseEffectList As New List(Of FinalEffect)
        Dim HiPEffectList As New List(Of FinalEffect)
        Dim LowPEffectList As New List(Of FinalEffect)
        Dim finalEffectList As New List(Of FinalEffect)
        Dim att As String = ""
        For attNumber As Integer = 0 To ModuleEffectsTable.Keys.Count - 1
            att = ModuleEffectsTable.Keys(attNumber)
            baseEffectList = ModuleEffectsTable(att)
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
            ModuleEffectsTable(att) = finalEffectList
        Next
    End Sub
    Private Sub ApplyModuleEffectsToModules(ByRef newShip As Ship)
        Dim att As String = ""
        For Each aModule As ShipModule In newShip.SlotCollection
            For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                att = aModule.Attributes.Keys(attNo)
                If ModuleEffectsTable.ContainsKey(att) = True Then
                    For Each fEffect As FinalEffect In ModuleEffectsTable(att)
                        If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                            Call ApplyFinalEffectToModule(aModule, fEffect, att)
                        End If
                    Next
                End If
            Next
            If aModule.LoadedCharge IsNot Nothing Then
                For attNo As Integer = 0 To aModule.LoadedCharge.Attributes.Keys.Count - 1
                    att = aModule.LoadedCharge.Attributes.Keys(attNo)
                    If ModuleEffectsTable.ContainsKey(att) = True Then
                        For Each fEffect As FinalEffect In ModuleEffectsTable(att)
                            If ProcessFinalEffectForModule(aModule.LoadedCharge, fEffect) = True Then
                                Call ApplyFinalEffectToModule(aModule.LoadedCharge, fEffect, att)
                            End If
                        Next
                    End If
                Next
            End If
            ShipModule.MapModuleAttributes(aModule)
        Next
    End Sub
    Private Sub ApplyModuleEffectsToDrones(ByRef newShip As Ship)
        Dim aModule As New ShipModule
        Dim att As String = ""
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            aModule = DBI.DroneType
            For attNo As Integer = 0 To aModule.Attributes.Keys.Count - 1
                att = aModule.Attributes.Keys(attNo)
                If ModuleEffectsTable.ContainsKey(att) = True Then
                    For Each fEffect As FinalEffect In ModuleEffectsTable(att)
                        If ProcessFinalEffectForModule(aModule, fEffect) = True Then
                            Call ApplyFinalEffectToModule(aModule, fEffect, att)
                        End If
                    Next
                End If
            Next
        Next
    End Sub
    Private Sub ApplyModuleEffectsToShip(ByRef newShip As Ship)
        Dim att As String = ""
        For attNo As Integer = 0 To newShip.Attributes.Keys.Count - 1
            att = newShip.Attributes.Keys(attNo)
            If ModuleEffectsTable.ContainsKey(att) = True Then
                For Each fEffect As FinalEffect In ModuleEffectsTable(att)
                    If ProcessFinalEffectForShip(newShip, fEffect) = True Then
                        Call ApplyFinalEffectToShip(newShip, fEffect, att)
                    End If
                Next
            End If
        Next
    End Sub
    Public Sub CalculateDamageStatistics(ByRef newShip As Ship)
        Dim cModule As New ShipModule
        Dim dgmMod As Double = 1
        Dim ROF As Double = 1
        newShip.Attributes("10006") = 0
        For Each DBI As DroneBayItem In newShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                cModule = DBI.DroneType
                If CInt(cModule.DatabaseGroup) <> 101 Then
                    ' Not mining drone
                    If cModule.Attributes.ContainsKey("51") = True Then
                        ROF = cModule.Attributes("51")
                        dgmMod = cModule.Attributes("64")
                    Else
                        If cModule.Attributes.ContainsKey("506") = True Then
                            ROF = cModule.Attributes("506")
                            dgmMod = cModule.Attributes("212")
                        Else
                            dgmMod = 0
                            ROF = 1
                        End If
                    End If
                    If cModule.LoadedCharge IsNot Nothing Then
                        cModule.Attributes("10017") = cModule.LoadedCharge.Attributes("114") + cModule.LoadedCharge.Attributes("116") + cModule.LoadedCharge.Attributes("117") + cModule.LoadedCharge.Attributes("118")
                        cModule.Attributes("10051") = cModule.LoadedCharge.Attributes("114") * dgmMod
                        cModule.Attributes("10052") = cModule.LoadedCharge.Attributes("116") * dgmMod
                        cModule.Attributes("10053") = cModule.LoadedCharge.Attributes("117") * dgmMod
                        cModule.Attributes("10054") = cModule.LoadedCharge.Attributes("118") * dgmMod
                        newShip.Attributes("10006") = newShip.Attributes("10006") + DBI.Quantity
                        cModule.Attributes("10018") = dgmMod * cModule.Attributes("10017")
                        cModule.Attributes("10019") = cModule.Attributes("10018") / ROF
					Else
						cModule.Attributes("10017") = 0
						If cModule.Attributes.ContainsKey("114") Then
							cModule.Attributes("10017") += cModule.Attributes("114")
							cModule.Attributes("10051") = cModule.Attributes("114") * dgmMod
						Else
							cModule.Attributes("10051") = 0
						End If
						If cModule.Attributes.ContainsKey("116") Then
							cModule.Attributes("10017") += cModule.Attributes("116")
							cModule.Attributes("10052") = cModule.Attributes("116") * dgmMod
						Else
							cModule.Attributes("10052") = 0
						End If
						If cModule.Attributes.ContainsKey("117") Then
							cModule.Attributes("10017") += cModule.Attributes("117")
							cModule.Attributes("10053") = cModule.Attributes("117") * dgmMod
						Else
							cModule.Attributes("10053") = 0
						End If
						If cModule.Attributes.ContainsKey("118") Then
							cModule.Attributes("10017") += cModule.Attributes("118")
							cModule.Attributes("10054") = cModule.Attributes("118") * dgmMod
						Else
							cModule.Attributes("10054") = 0
						End If
						newShip.Attributes("10006") = newShip.Attributes("10006") + DBI.Quantity
						cModule.Attributes("10018") = dgmMod * cModule.Attributes("10017")
						cModule.Attributes("10019") = cModule.Attributes("10018") / ROF
                    End If
                    newShip.Attributes("10023") = newShip.Attributes("10023") + cModule.Attributes("10018") * DBI.Quantity
                    newShip.Attributes("10027") = newShip.Attributes("10027") + cModule.Attributes("10019") * DBI.Quantity
                    newShip.Attributes("10028") = newShip.Attributes("10028") + cModule.Attributes("10018") * DBI.Quantity
                    newShip.Attributes("10029") = newShip.Attributes("10029") + cModule.Attributes("10019") * DBI.Quantity
                    newShip.Attributes("10055") = newShip.Attributes("10055") + cModule.Attributes("10051") * DBI.Quantity
                    newShip.Attributes("10056") = newShip.Attributes("10056") + cModule.Attributes("10052") * DBI.Quantity
                    newShip.Attributes("10057") = newShip.Attributes("10057") + cModule.Attributes("10053") * DBI.Quantity
                    newShip.Attributes("10058") = newShip.Attributes("10058") + cModule.Attributes("10054") * DBI.Quantity
                    newShip.Attributes("10070") = newShip.Attributes("10070") + (cModule.Attributes("10051") / ROF) * DBI.Quantity
                    newShip.Attributes("10071") = newShip.Attributes("10071") + (cModule.Attributes("10052") / ROF) * DBI.Quantity
                    newShip.Attributes("10072") = newShip.Attributes("10072") + (cModule.Attributes("10053") / ROF) * DBI.Quantity
                    newShip.Attributes("10073") = newShip.Attributes("10073") + (cModule.Attributes("10054") / ROF) * DBI.Quantity
                Else
                    ' Mining drone
                    newShip.Attributes("10006") = newShip.Attributes("10006") + DBI.Quantity
                    cModule.Attributes("10040") = cModule.Attributes("77") / cModule.Attributes("73")
                    newShip.Attributes("10033") = newShip.Attributes("10033") + cModule.Attributes("77") * DBI.Quantity
                    newShip.Attributes("10035") = newShip.Attributes("10035") + cModule.Attributes("77") * DBI.Quantity
                    newShip.Attributes("10044") = newShip.Attributes("10044") + cModule.Attributes("10040") * DBI.Quantity
                    newShip.Attributes("10047") = newShip.Attributes("10047") + cModule.Attributes("10040") * DBI.Quantity
                End If
            End If
        Next
        For slot As Integer = 1 To newShip.HiSlots
            cModule = newShip.HiSlot(slot)
            If cModule IsNot Nothing Then
                If (cModule.ModuleState Or 12) = 12 Then
                    Select Case CInt(cModule.MarketGroup)
                        Case 1039, 1040 ' Ore Mining Turret
                            newShip.Attributes("10034") = newShip.Attributes("10034") + cModule.Attributes("77")
                            newShip.Attributes("10033") = newShip.Attributes("10033") + cModule.Attributes("77")
                            cModule.Attributes("10039") = cModule.Attributes("77") / cModule.Attributes("73")
                            newShip.Attributes("10043") = newShip.Attributes("10043") + cModule.Attributes("10039")
                            newShip.Attributes("10047") = newShip.Attributes("10047") + cModule.Attributes("10039")
                        Case 1038 ' Ice Mining Turret
                            newShip.Attributes("10037") = newShip.Attributes("10037") + cModule.Attributes("77")
                            newShip.Attributes("10036") = newShip.Attributes("10036") + cModule.Attributes("77")
                            cModule.Attributes("10041") = cModule.Attributes("77") / cModule.Attributes("73")
                            newShip.Attributes("10045") = newShip.Attributes("10045") + cModule.Attributes("10041")
                            newShip.Attributes("10048") = newShip.Attributes("10048") + cModule.Attributes("10041")
                        Case Else
                            If cModule.IsTurret Or cModule.IsLauncher Then
                                If cModule.LoadedCharge IsNot Nothing Then
                                    cModule.Attributes("10030") = CDbl(cModule.LoadedCharge.ID)
                                    cModule.Attributes("10017") = cModule.LoadedCharge.Attributes("114") + cModule.LoadedCharge.Attributes("116") + cModule.LoadedCharge.Attributes("117") + cModule.LoadedCharge.Attributes("118")
                                    If cModule.IsTurret = True Then
                                        ' Adjust for reload time if required
                                        Dim reloadEffect As Double = 0
                                        If HQF.Settings.HQFSettings.IncludeAmmoReloadTime = True Then
                                            If cModule.DatabaseGroup <> "53" Then
                                                reloadEffect = 10 / (cModule.Capacity / cModule.LoadedCharge.Volume)
                                            End If
                                        End If
                                        Select Case cModule.DatabaseGroup
                                            Case "53" ' Energy Turret 
                                                dgmMod = cModule.Attributes("10014")
                                                ROF = cModule.Attributes("10011") + reloadEffect
                                            Case "74" ' Hybrid Turret
                                                dgmMod = cModule.Attributes("10015")
                                                ROF = cModule.Attributes("10012") + reloadEffect
                                            Case "55" ' Projectile Turret
                                                dgmMod = cModule.Attributes("10016")
                                                ROF = cModule.Attributes("10013") + reloadEffect
                                        End Select
                                        cModule.Attributes("10018") = dgmMod * cModule.Attributes("10017")
                                        cModule.Attributes("10019") = cModule.Attributes("10018") / ROF
                                        newShip.Attributes("10020") = newShip.Attributes("10020") + cModule.Attributes("10018")
                                        newShip.Attributes("10024") = newShip.Attributes("10024") + cModule.Attributes("10019")
                                        newShip.Attributes("10028") = newShip.Attributes("10028") + cModule.Attributes("10018")
                                        newShip.Attributes("10029") = newShip.Attributes("10029") + cModule.Attributes("10019")
                                    Else
                                        ' Adjust for reload time if required
                                        Dim reloadEffect As Double = 0
                                        If HQF.Settings.HQFSettings.IncludeAmmoReloadTime = True Then
                                            reloadEffect = 10 / (cModule.Capacity / cModule.LoadedCharge.Volume)
                                        End If
                                        dgmMod = 1
                                        ROF = cModule.Attributes("51") + reloadEffect
                                        cModule.Attributes("10018") = dgmMod * cModule.Attributes("10017")
                                        cModule.Attributes("10019") = cModule.Attributes("10018") / ROF
                                        newShip.Attributes("10021") = newShip.Attributes("10021") + cModule.Attributes("10018")
                                        newShip.Attributes("10025") = newShip.Attributes("10025") + cModule.Attributes("10019")
                                        newShip.Attributes("10028") = newShip.Attributes("10028") + cModule.Attributes("10018")
                                        newShip.Attributes("10029") = newShip.Attributes("10029") + cModule.Attributes("10019")
                                        If cModule.LoadedCharge IsNot Nothing Then
                                            cModule.Attributes("54") = cModule.LoadedCharge.Attributes("37") * cModule.LoadedCharge.Attributes("281") * HQF.Settings.HQFSettings.MissileRangeConstant
                                        End If
                                    End If
                                    cModule.Attributes("10051") = cModule.LoadedCharge.Attributes("114") * dgmMod
                                    cModule.Attributes("10052") = cModule.LoadedCharge.Attributes("116") * dgmMod
                                    cModule.Attributes("10053") = cModule.LoadedCharge.Attributes("117") * dgmMod
                                    cModule.Attributes("10054") = cModule.LoadedCharge.Attributes("118") * dgmMod
                                    newShip.Attributes("10055") = newShip.Attributes("10055") + cModule.Attributes("10051")
                                    newShip.Attributes("10056") = newShip.Attributes("10056") + cModule.Attributes("10052")
                                    newShip.Attributes("10057") = newShip.Attributes("10057") + cModule.Attributes("10053")
                                    newShip.Attributes("10058") = newShip.Attributes("10058") + cModule.Attributes("10054")
                                    newShip.Attributes("10070") = newShip.Attributes("10070") + (cModule.Attributes("10051") / ROF)
                                    newShip.Attributes("10071") = newShip.Attributes("10071") + (cModule.Attributes("10052") / ROF)
                                    newShip.Attributes("10072") = newShip.Attributes("10072") + (cModule.Attributes("10053") / ROF)
                                    newShip.Attributes("10073") = newShip.Attributes("10073") + (cModule.Attributes("10054") / ROF)
                                End If
                            Else
                                If cModule.DatabaseGroup = "72" Then
                                    ' Do smartbomb code
                                    ROF = cModule.Attributes("73")
                                    cModule.Attributes("10017") = 0
                                    If cModule.Attributes.ContainsKey("114") Then
                                        cModule.Attributes("10017") += cModule.Attributes("114")
                                        cModule.Attributes("10051") = cModule.Attributes("114")
                                    Else
                                        cModule.Attributes("10051") = 0
                                    End If
                                    If cModule.Attributes.ContainsKey("116") Then
                                        cModule.Attributes("10017") += cModule.Attributes("116")
                                        cModule.Attributes("10052") = cModule.Attributes("116")
                                    Else
                                        cModule.Attributes("10052") = 0
                                    End If
                                    If cModule.Attributes.ContainsKey("117") Then
                                        cModule.Attributes("10017") += cModule.Attributes("117")
                                        cModule.Attributes("10053") = cModule.Attributes("117")
                                    Else
                                        cModule.Attributes("10053") = 0
                                    End If
                                    If cModule.Attributes.ContainsKey("118") Then
                                        cModule.Attributes("10017") += cModule.Attributes("118")
                                        cModule.Attributes("10054") = cModule.Attributes("118")
                                    Else
                                        cModule.Attributes("10054") = 0
                                    End If
                                    cModule.Attributes("10018") = cModule.Attributes("10017")
                                    cModule.Attributes("10019") = cModule.Attributes("10018") / ROF
                                    newShip.Attributes("10022") = newShip.Attributes("10022") + cModule.Attributes("10018")
                                    newShip.Attributes("10026") = newShip.Attributes("10026") + cModule.Attributes("10019")
                                    newShip.Attributes("10028") = newShip.Attributes("10028") + cModule.Attributes("10018")
                                    newShip.Attributes("10029") = newShip.Attributes("10029") + cModule.Attributes("10019")
                                    newShip.Attributes("10055") = newShip.Attributes("10055") + cModule.Attributes("10051")
                                    newShip.Attributes("10056") = newShip.Attributes("10056") + cModule.Attributes("10052")
                                    newShip.Attributes("10057") = newShip.Attributes("10057") + cModule.Attributes("10053")
                                    newShip.Attributes("10058") = newShip.Attributes("10058") + cModule.Attributes("10054")
                                    newShip.Attributes("10070") = newShip.Attributes("10070") + (cModule.Attributes("10051") / ROF)
                                    newShip.Attributes("10071") = newShip.Attributes("10071") + (cModule.Attributes("10052") / ROF)
                                    newShip.Attributes("10072") = newShip.Attributes("10072") + (cModule.Attributes("10053") / ROF)
                                    newShip.Attributes("10073") = newShip.Attributes("10073") + (cModule.Attributes("10054") / ROF)
                                ElseIf cModule.DatabaseGroup = "862" Then
                                    ' Do Bomb Launcher Code
                                    If cModule.LoadedCharge IsNot Nothing Then
                                        ' Adjust for reload time if required
                                        Dim reloadEffect As Double = 0
                                        If HQF.Settings.HQFSettings.IncludeAmmoReloadTime = True Then
                                            reloadEffect = 10 / (cModule.Capacity / cModule.LoadedCharge.Volume)
                                        End If
                                        dgmMod = 1
                                        ROF = cModule.Attributes("51")
                                        cModule.Attributes("10017") = cModule.LoadedCharge.Attributes("114") + cModule.LoadedCharge.Attributes("116") + cModule.LoadedCharge.Attributes("117") + cModule.LoadedCharge.Attributes("118")
                                        cModule.Attributes("10018") = dgmMod * cModule.Attributes("10017")
                                        cModule.Attributes("10019") = cModule.Attributes("10018") / ROF
                                        newShip.Attributes("10021") = newShip.Attributes("10021") + cModule.Attributes("10018")
                                        newShip.Attributes("10025") = newShip.Attributes("10025") + cModule.Attributes("10019")
                                        newShip.Attributes("10028") = newShip.Attributes("10028") + cModule.Attributes("10018")
                                        newShip.Attributes("10029") = newShip.Attributes("10029") + cModule.Attributes("10019")
                                        If cModule.LoadedCharge IsNot Nothing Then
                                            cModule.Attributes("54") = cModule.LoadedCharge.Attributes("37") * cModule.LoadedCharge.Attributes("281") * HQF.Settings.HQFSettings.MissileRangeConstant
                                        End If
                                        cModule.Attributes("10051") = cModule.LoadedCharge.Attributes("114") * dgmMod
                                        cModule.Attributes("10052") = cModule.LoadedCharge.Attributes("116") * dgmMod
                                        cModule.Attributes("10053") = cModule.LoadedCharge.Attributes("117") * dgmMod
                                        cModule.Attributes("10054") = cModule.LoadedCharge.Attributes("118") * dgmMod
                                        newShip.Attributes("10055") = newShip.Attributes("10055") + cModule.Attributes("10051")
                                        newShip.Attributes("10056") = newShip.Attributes("10056") + cModule.Attributes("10052")
                                        newShip.Attributes("10057") = newShip.Attributes("10057") + cModule.Attributes("10053")
                                        newShip.Attributes("10058") = newShip.Attributes("10058") + cModule.Attributes("10054")
                                        newShip.Attributes("10070") = newShip.Attributes("10070") + (cModule.Attributes("10051") / ROF)
                                        newShip.Attributes("10071") = newShip.Attributes("10071") + (cModule.Attributes("10052") / ROF)
                                        newShip.Attributes("10072") = newShip.Attributes("10072") + (cModule.Attributes("10053") / ROF)
                                        newShip.Attributes("10073") = newShip.Attributes("10073") + (cModule.Attributes("10054") / ROF)
                                    End If
                                End If
                            End If
                    End Select
                End If
            End If
        Next
    End Sub
    Public Function CalculateDamageStatsForDefenceProfile(ByRef newShip As Ship) As DefenceProfileResults

        Dim dpr As New DefenceProfileResults
        Dim DP As DefenceProfile = CType(DefenceProfiles.ProfileList(Me.DefenceProfileName), DefenceProfile)

        If DP IsNot Nothing Then
            Dim SEM As Double = newShip.Attributes("10070") * (1 - (DP.SEM / 100))
            Dim SEx As Double = newShip.Attributes("10071") * (1 - (DP.SExplosive / 100))
            Dim SKi As Double = newShip.Attributes("10072") * (1 - (DP.SKinetic / 100))
            Dim STh As Double = newShip.Attributes("10073") * (1 - (DP.SThermal / 100))
            Dim ST As Double = SEM + SEx + SKi + STh
            dpr.ShieldDPS = ST

            Dim AEM As Double = newShip.Attributes("10070") * (1 - (DP.AEM / 100))
            Dim AEx As Double = newShip.Attributes("10071") * (1 - (DP.AExplosive / 100))
            Dim AKi As Double = newShip.Attributes("10072") * (1 - (DP.AKinetic / 100))
            Dim ATh As Double = newShip.Attributes("10073") * (1 - (DP.AThermal / 100))
            Dim AT As Double = AEM + AEx + AKi + ATh
            dpr.ArmorDPS = AT

            Dim HEM As Double = newShip.Attributes("10070") * (1 - (DP.HEM / 100))
            Dim HEx As Double = newShip.Attributes("10071") * (1 - (DP.HExplosive / 100))
            Dim HKi As Double = newShip.Attributes("10072") * (1 - (DP.HKinetic / 100))
            Dim HTh As Double = newShip.Attributes("10073") * (1 - (DP.HThermal / 100))
            Dim HT As Double = HEM + HEx + HKi + HTh
            dpr.HullDPS = HT
        End If

        Return dpr

    End Function
    Public Sub CalculateDefenceStatistics(ByRef newShip As Ship)
        Dim sRP, sRA, aR, hR As Double
        For Each cModule As ShipModule In newShip.SlotCollection
            ' Calculate shield boosting
            If cModule.DatabaseGroup = "40" And (cModule.ModuleState And 12) = cModule.ModuleState Then
                sRA = sRA + cModule.Attributes("68") / cModule.Attributes("73")
            End If
            ' Calculate remote shield boosting
            If cModule.DatabaseGroup = "41" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                sRA = sRA + cModule.Attributes("68") / cModule.Attributes("73")
            End If
            ' Calculate shield maintenance drones
            If cModule.DatabaseGroup = "640" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                If cModule.Attributes.ContainsKey("68") Then
                    sRA = sRA + cModule.Attributes("68") / cModule.Attributes("73")
                End If
            End If
            ' Calculate armor repairing
            If cModule.DatabaseGroup = "62" And (cModule.ModuleState And 12) = cModule.ModuleState Then
                aR = aR + cModule.Attributes("84") / cModule.Attributes("73")
            End If
            ' Calculate remote armor repairing
            If cModule.DatabaseGroup = "325" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                aR = aR + cModule.Attributes("84") / cModule.Attributes("73")
            End If
            ' Calculate armor maintenance drones
            If cModule.DatabaseGroup = "640" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                If cModule.Attributes.ContainsKey("84") Then
                    aR = aR + cModule.Attributes("84") / cModule.Attributes("73")
                End If
            End If
            ' Calculate hull repairing
            If cModule.DatabaseGroup = "63" And (cModule.ModuleState And 12) = cModule.ModuleState Then
                hR = hR + cModule.Attributes("83") / cModule.Attributes("73")
            End If
            ' Calculate remote hull repairing
            If cModule.DatabaseGroup = "585" And (cModule.ModuleState And 16) = cModule.ModuleState Then
                hR = hR + cModule.Attributes("83") / cModule.Attributes("73")
            End If
        Next
        sRP = (newShip.ShieldCapacity / newShip.ShieldRecharge * HQF.Settings.HQFSettings.ShieldRechargeConstant)
        ' Calculate the actual tanking ability
        Dim sTA As Double = sRA / ((newShip.DamageProfileEM * (1 - newShip.ShieldEMResist / 100)) + (newShip.DamageProfileEX * (1 - newShip.ShieldExResist / 100)) + (newShip.DamageProfileKI * (1 - newShip.ShieldKiResist / 100)) + (newShip.DamageProfileTH * (1 - newShip.ShieldThResist / 100)))
        Dim sTP As Double = sRP / ((newShip.DamageProfileEM * (1 - newShip.ShieldEMResist / 100)) + (newShip.DamageProfileEX * (1 - newShip.ShieldExResist / 100)) + (newShip.DamageProfileKI * (1 - newShip.ShieldKiResist / 100)) + (newShip.DamageProfileTH * (1 - newShip.ShieldThResist / 100)))
        Dim aT As Double = aR / ((newShip.DamageProfileEM * (1 - newShip.ArmorEMResist / 100)) + (newShip.DamageProfileEX * (1 - newShip.ArmorExResist / 100)) + (newShip.DamageProfileKI * (1 - newShip.ArmorKiResist / 100)) + (newShip.DamageProfileTH * (1 - newShip.ArmorThResist / 100)))
        Dim hT As Double = hR / ((newShip.DamageProfileEM * (1 - newShip.StructureEMResist / 100)) + (newShip.DamageProfileEX * (1 - newShip.StructureExResist / 100)) + (newShip.DamageProfileKI * (1 - newShip.StructureKiResist / 100)) + (newShip.DamageProfileTH * (1 - newShip.StructureThResist / 100)))
        newShip.Attributes("10059") = sTA
        newShip.Attributes("10060") = aT
        newShip.Attributes("10061") = hT
        newShip.Attributes("10062") = Math.Max(sTA + sTP, sTA + aT + hT)
        newShip.Attributes("10069") = sTP
        newShip.Attributes("10065") = sRA + sRP
        newShip.Attributes("10066") = aR
        newShip.Attributes("10067") = hR
        newShip.Attributes("10068") = sRA + sRP + aR + hR
    End Sub
#End Region

#Region "Common Private Supporting Fitting Routines"

    Private Function ProcessFinalEffectForShip(ByVal NewShip As Ship, ByVal FEffect As FinalEffect) As Boolean
        Select Case FEffect.AffectedType
            Case EffectType.All
                Return True
            Case EffectType.Item
                If FEffect.AffectedID.Contains(NewShip.ID) Then
                    Return True
                End If
            Case EffectType.Group
                If FEffect.AffectedID.Contains(NewShip.DatabaseGroup) Then
                    Return True
                End If
            Case EffectType.Category
                If FEffect.AffectedID.Contains(NewShip.DatabaseCategory) Then
                    Return True
                End If
            Case EffectType.MarketGroup
                If FEffect.AffectedID.Contains(NewShip.MarketGroup) Then
                    Return True
                End If
            Case EffectType.Skill
                If NewShip.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(FEffect.AffectedID(0)))) Then
                    Return True
                End If
            Case EffectType.Attribute
                If NewShip.Attributes.ContainsKey(FEffect.AffectedID(0)) Then
                    Return True
                End If
        End Select
        Return False
    End Function

    Private Function ProcessFinalEffectForModule(ByVal NewModule As ShipModule, ByVal FEffect As FinalEffect) As Boolean
        Select Case FEffect.AffectedType
            Case EffectType.All
                Return True
            Case EffectType.Item
                If FEffect.AffectedID.Contains(NewModule.ID) Then
                    Return True
                End If
            Case EffectType.Group
                If FEffect.AffectedID.Contains(NewModule.DatabaseGroup) Then
                    Return True
                End If
            Case EffectType.Category
                If FEffect.AffectedID.Contains(NewModule.DatabaseCategory) Then
                    Return True
                End If
            Case EffectType.MarketGroup
                If FEffect.AffectedID.Contains(NewModule.MarketGroup) Then
                    Return True
                End If
            Case EffectType.Skill
                If NewModule.RequiredSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(FEffect.AffectedID(0)))) Then
                    Return True
                End If
            Case EffectType.Slot
                If FEffect.AffectedID.Contains(NewModule.SlotType & NewModule.SlotNo) Then
                    Return True
                End If
            Case EffectType.Attribute
                If NewModule.Attributes.ContainsKey(CStr(FEffect.AffectedID(0))) Then
                    Return True
                End If
        End Select
        Return False
    End Function

    Private Sub ApplyFinalEffectToShip(ByVal NewShip As Ship, ByVal FEffect As FinalEffect, ByVal Att As String)
        Dim log As String = Attributes.AttributeQuickList(Att).ToString & "# " & FEffect.Cause
        If NewShip.Name = FEffect.Cause Then
            log &= " (Overloading)"
        End If
        Dim oldAtt As String = NewShip.Attributes(Att).ToString()
        log &= "# " & oldAtt
        Select Case FEffect.CalcType
            Case EffectCalcType.Percentage
                NewShip.Attributes(Att) = NewShip.Attributes(Att) * (1 + (FEffect.AffectedValue / 100))
            Case EffectCalcType.Addition
                NewShip.Attributes(Att) = NewShip.Attributes(Att) + FEffect.AffectedValue
            Case EffectCalcType.Difference ' Used for resistances
                If FEffect.AffectedValue <= 0 Then
                    NewShip.Attributes(Att) = ((100 - NewShip.Attributes(Att)) * (-FEffect.AffectedValue / 100)) + NewShip.Attributes(Att)
                Else
                    NewShip.Attributes(Att) = (NewShip.Attributes(Att) * (-FEffect.AffectedValue / 100)) + NewShip.Attributes(Att)
                End If
            Case EffectCalcType.Velocity
                NewShip.Attributes(Att) = CDbl(NewShip.Attributes(Att)) + (CDbl(NewShip.Attributes(Att)) * (CDbl(NewShip.Attributes("10010")) / CDbl(NewShip.Attributes("10002")) * (FEffect.AffectedValue / 100)))
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
        End Select
        log &= "# " & NewShip.Attributes(Att).ToString
        If oldAtt <> NewShip.Attributes(Att).ToString Then
            NewShip.AuditLog.Add(log)
        End If
    End Sub

    Private Sub ApplyFinalEffectToModule(ByVal NewModule As ShipModule, ByVal FEffect As FinalEffect, ByVal Att As String)
        Dim log As String = Attributes.AttributeQuickList(Att).ToString & ": " & FEffect.Cause
        If NewModule.Name = FEffect.Cause Then
            log &= " (Overloading)"
        End If
        Dim oldAtt As String = NewModule.Attributes(Att).ToString()
        log &= ": " & oldAtt
        Select Case FEffect.CalcType
            Case EffectCalcType.Percentage
                NewModule.Attributes(Att) = NewModule.Attributes(Att) * (1 + (FEffect.AffectedValue / 100))
            Case EffectCalcType.Addition
                NewModule.Attributes(Att) = NewModule.Attributes(Att) + FEffect.AffectedValue
            Case EffectCalcType.Difference ' Used for resistances
                If FEffect.AffectedValue <= 0 Then
                    NewModule.Attributes(Att) = ((100 - NewModule.Attributes(Att)) * (-FEffect.AffectedValue / 100)) + NewModule.Attributes(Att)
                Else
                    NewModule.Attributes(Att) = (NewModule.Attributes(Att) * (-FEffect.AffectedValue / 100)) + NewModule.Attributes(Att)
                End If
            Case EffectCalcType.Velocity
                NewModule.Attributes(Att) = CDbl(NewModule.Attributes(Att)) + (CDbl(NewModule.Attributes(Att)) * (CDbl(NewModule.Attributes("10010")) / CDbl(NewModule.Attributes("10002")) * (FEffect.AffectedValue / 100)))
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
        End Select
        log &= " --> " & NewModule.Attributes(Att).ToString
        If oldAtt <> NewModule.Attributes(Att).ToString Then
            NewModule.AuditLog.Add(log)
        End If
    End Sub

#End Region

#Region "Cloning Function"

    Private Function CloneEffectList(ByVal oldSortedList As SortedList(Of String, List(Of FinalEffect))) As SortedList(Of String, List(Of FinalEffect))
        Dim myMemoryStream As New MemoryStream()
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(myMemoryStream, oldSortedList)
        myMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newSortedList As SortedList(Of String, List(Of FinalEffect)) = CType(objBinaryFormatter.Deserialize(myMemoryStream), SortedList(Of String, List(Of FinalEffect)))
        myMemoryStream.Close()
        Return newSortedList
    End Function

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

#End Region

#Region "Data/BaseShip Conversion Routines"

    ''' <summary>
    ''' Takes the modules etc and adds them to the base ship for processing
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateBaseShipFromFitting()

        Call Me.ReorderModules()

        ' Add the modules
        For Each MWS As ModuleWithState In Me.Modules
            Dim NewMod As ShipModule = CType(ModuleLists.moduleList(MWS.ID), ShipModule).Clone
            If MWS.ChargeID <> "" Then
                NewMod.LoadedCharge = CType(ModuleLists.moduleList(MWS.ChargeID), ShipModule).Clone
            End If
            NewMod.ModuleState = MWS.State
            Call Me.AddModule(NewMod, 0, True, True, Nothing, False, False)
        Next

        ' Add the drones
        For Each MWS As ModuleQWithState In Me.Drones
            Dim NewMod As ShipModule = CType(ModuleLists.moduleList(MWS.ID), ShipModule).Clone
            NewMod.ModuleState = MWS.State
            If MWS.State = ModuleStates.Active Then
                Call Me.AddDrone(NewMod, MWS.Quantity, True, True)
            Else
                Call Me.AddDrone(NewMod, MWS.Quantity, False, True)
            End If
        Next

        ' Add items
        For Each MWS As ModuleQWithState In Me.Items
            Dim NewMod As ShipModule = CType(ModuleLists.moduleList(MWS.ID), ShipModule).Clone
            NewMod.ModuleState = MWS.State
            Call Me.AddItem(NewMod, MWS.Quantity, True)
        Next

        ' Add ships
        For Each MWS As ModuleQWithState In Me.Ships
            Dim NewMod As Ship = CType(ShipLists.shipList(ShipLists.shipListKeyID(MWS.ID)), Ship).Clone
            Call Me.AddShip(NewMod, MWS.Quantity, True)
        Next

        ' Add Boosters
        Me.BaseShip.BoosterSlotCollection.Clear()
        For Each MWS As ModuleWithState In Me.Boosters
            Dim sMod As ShipModule = CType(ModuleLists.moduleList(MWS.ID), ShipModule).Clone
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
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.SubSlot(slot).ID, Me.BaseShip.SubSlot(slot).LoadedCharge.ID, Me.BaseShip.SubSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.SubSlot(slot).ID, Nothing, Me.BaseShip.SubSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add rig slots
        For slot As Integer = 1 To Me.BaseShip.RigSlots
            If Me.BaseShip.RigSlot(slot) IsNot Nothing Then
                If Me.BaseShip.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.RigSlot(slot).ID, Me.BaseShip.RigSlot(slot).LoadedCharge.ID, Me.BaseShip.RigSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.RigSlot(slot).ID, Nothing, Me.BaseShip.RigSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add low slots
        For slot As Integer = 1 To Me.BaseShip.LowSlots
            If Me.BaseShip.LowSlot(slot) IsNot Nothing Then
                If Me.BaseShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.LowSlot(slot).ID, Me.BaseShip.LowSlot(slot).LoadedCharge.ID, Me.BaseShip.LowSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.LowSlot(slot).ID, Nothing, Me.BaseShip.LowSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add mid slots
        For slot As Integer = 1 To Me.BaseShip.MidSlots
            If Me.BaseShip.MidSlot(slot) IsNot Nothing Then
                If Me.BaseShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.MidSlot(slot).ID, Me.BaseShip.MidSlot(slot).LoadedCharge.ID, Me.BaseShip.MidSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.MidSlot(slot).ID, Nothing, Me.BaseShip.MidSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add high slots
        For slot As Integer = 1 To Me.BaseShip.HiSlots
            If Me.BaseShip.HiSlot(slot) IsNot Nothing Then
                If Me.BaseShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.HiSlot(slot).ID, Me.BaseShip.HiSlot(slot).LoadedCharge.ID, Me.BaseShip.HiSlot(slot).ModuleState))
                Else
                    Me.Modules.Add(New ModuleWithState(Me.BaseShip.HiSlot(slot).ID, Nothing, Me.BaseShip.HiSlot(slot).ModuleState))
                End If
            End If
        Next

        ' Add drones
        Me.Drones.Clear()
        For Each DBI As DroneBayItem In Me.BaseShip.DroneBayItems.Values
            If DBI.IsActive = True Then
                Me.Drones.Add(New ModuleQWithState(DBI.DroneType.ID, ModuleStates.Active, DBI.Quantity))
            Else
                Me.Drones.Add(New ModuleQWithState(DBI.DroneType.ID, ModuleStates.Inactive, DBI.Quantity))
            End If
        Next

        ' Add items
        Me.Items.Clear()
        For Each CBI As CargoBayItem In Me.BaseShip.CargoBayItems.Values
            Me.Items.Add(New ModuleQWithState(CBI.ItemType.ID, ModuleStates.Active, CBI.Quantity))
        Next

        ' Add ships
        Me.Ships.Clear()
        For Each SBI As ShipBayItem In Me.BaseShip.ShipBayItems.Values
            Me.Ships.Add(New ModuleQWithState(SBI.ShipType.ID, ModuleStates.Active, SBI.Quantity))
        Next

        ' Add boosters
        Me.Boosters.Clear()
        For Each Booster As ShipModule In Me.BaseShip.BoosterSlotCollection
            Me.Boosters.Add(New ModuleWithState(Booster.ID, Nothing, ModuleStates.Active))
        Next

    End Sub

    ''' <summary>
    ''' Re-orders modules to allow correct update procedures
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReorderModules()
        Dim subs, mods As New ArrayList
        For Each MWS As ModuleWithState In Me.Modules
            If ModuleLists.moduleList.ContainsKey(MWS.ID) = True Then
                If CType(ModuleLists.moduleList(MWS.ID), ShipModule).SlotType = 16 Then
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
        If shipMod.ID = "11014" And shipMod.ModuleState = ModuleStates.Active Then
            Me.BaseShip.Attributes("10063") += 1
        End If

        ' Check slot availability (only if not adding in a specific slot?)
        If IsSwappingModules = False Then
            If IsSlotAvailable(shipMod, repMod) = False Then
                Exit Sub
            End If
        End If

        ' Check fitting constraints
        If IsModulePermitted(shipMod) = False Then
            Exit Sub
        End If

        ' Get old module if applicable
        Dim OldModName As String = ""
        Dim OldChargeName As String = ""
        If slotNo <> 0 Then
            Dim LoadedModule As New ShipModule
            Select Case shipMod.SlotType
                Case SlotTypes.Rig  ' Rig
                    LoadedModule = BaseShip.RigSlot(slotNo)
                Case SlotTypes.Low  ' Low
                    LoadedModule = BaseShip.LowSlot(slotNo)
                Case SlotTypes.Mid  ' Mid
                    LoadedModule = BaseShip.MidSlot(slotNo)
                Case SlotTypes.High  ' High
                    LoadedModule = BaseShip.HiSlot(slotNo)
                Case SlotTypes.Subsystem  ' Subsystem
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
            slotNo = AddModuleInNextSlot(CType(shipMod.Clone, ShipModule))
        Else
            AddModuleInSpecifiedSlot(CType(shipMod.Clone, ShipModule), slotNo)
        End If

        ' Check if we need to update
        If UpdateAll = False Then
            If UpdateShip = True Then
                ' What sort of update do we need? Check for subsystems enabled
                If CDbl(shipMod.DatabaseCategory) = 32 Then
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
                If shipMod.LoadedCharge IsNot Nothing Then
                    Me.ShipSlotCtrl.UndoStack.Push(New UndoInfo(UndoInfo.TransType.AddModule, shipMod.SlotType, slotNo, OldModName, OldChargeName, slotNo, shipMod.Name, shipMod.LoadedCharge.Name))
                Else
                    Me.ShipSlotCtrl.UndoStack.Push(New UndoInfo(UndoInfo.TransType.AddModule, shipMod.SlotType, slotNo, OldModName, OldChargeName, slotNo, shipMod.Name, ""))
                End If
                Me.ShipSlotCtrl.UpdateHistory()
            End If
        Else
            ' Need to rebuild the ship in order to account for the new modules as they're being added
            If CDbl(shipMod.DatabaseCategory) = 32 Then
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
        If myShip.DroneBay - Me.BaseShip.DroneBay_Used >= vol Then
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
                Dim DBI As New DroneBayItem
                DBI.DroneType = Drone
                DBI.Quantity = Qty
                DBI.IsActive = Active
                Me.BaseShip.DroneBayItems.Add(Me.BaseShip.DroneBayItems.Count, DBI)
            End If
            ' Update stuff
            If UpdateAll = False Then
                Me.ApplyFitting(BuildType.BuildFromEffectsMaps)
                If Me.ShipSlotCtrl IsNot Nothing Then
                    Call Me.ShipSlotCtrl.UpdateDroneBay()
                End If
            End If
        Else
            MessageBox.Show("There is not enough space in the Drone Bay to hold 1 unit of " & Drone.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            If myShip.CargoBay - Me.BaseShip.CargoBay_Used >= vol Then
                ' Scan through existing items and see if we can group this new one
                For Each itemGroup As CargoBayItem In Me.BaseShip.CargoBayItems.Values
                    If Item.Name = itemGroup.ItemType.Name And UpdateAll = False Then
                        ' Add to existing drone group
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
                End If
            Else
                MessageBox.Show("There is not enough space in the Cargo Bay to hold 1 unit of " & Item.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            If myShip.ShipBay - Me.BaseShip.ShipBay_Used >= vol Then
                ' Scan through existing items and see if we can group this new one
                For Each itemGroup As ShipBayItem In Me.BaseShip.ShipBayItems.Values
                    If Item.Name = itemGroup.ShipType.Name And UpdateAll = False Then
                        ' Add to existing drone group
                        itemGroup.Quantity += Qty
                        grouped = True
                        Exit For
                    End If
                Next
                ' Put the item into the cargo bay if not grouped
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
                End If
            Else
                MessageBox.Show("There is not enough space in the Ship Maintenance Bay to hold 1 unit of " & Item.Name & ".", "Insufficient Space", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

#End Region

#Region "Item Fitting Check Routines"

    Private Function IsSlotAvailable(ByVal shipMod As ShipModule, Optional ByVal repShipMod As ShipModule = Nothing) As Boolean
        Dim cSub, cRig, cLow, cMid, cHi, cTurret, cLauncher As Integer

        If repShipMod IsNot Nothing Then
            Select Case repShipMod.SlotType
                Case SlotTypes.Rig  ' Rig
                    cRig = Me.BaseShip.RigSlots_Used - 1
                Case SlotTypes.Low  ' Low
                    cLow = Me.BaseShip.LowSlots_Used - 1
                Case SlotTypes.Mid  ' Mid
                    cMid = Me.BaseShip.MidSlots_Used - 1
                Case SlotTypes.High  ' High
                    cHi = Me.BaseShip.HiSlots_Used - 1
                Case SlotTypes.Subsystem  ' High
                    cSub = Me.BaseShip.SubSlots_Used - 1
            End Select
            If repShipMod.IsTurret = True Then
                cTurret = Me.BaseShip.TurretSlots_Used - 1
            End If
            If repShipMod.IsLauncher = True Then
                cLauncher = Me.BaseShip.LauncherSlots_Used - 1
            End If
        Else
            cSub = Me.BaseShip.SubSlots_Used
            cRig = Me.BaseShip.RigSlots_Used
            cLow = Me.BaseShip.LowSlots_Used
            cMid = Me.BaseShip.MidSlots_Used
            cHi = Me.BaseShip.HiSlots_Used
            cTurret = Me.BaseShip.TurretSlots_Used
            cLauncher = Me.BaseShip.LauncherSlots_Used
        End If
        ' First, check slot layout
        Select Case shipMod.SlotType
            Case SlotTypes.Rig  ' Rig
                If cRig = Me.BaseShip.RigSlots Then
                    MessageBox.Show("There are no available rig slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.Low  ' Low
                If cLow = Me.BaseShip.LowSlots Then
                    MessageBox.Show("There are no available low slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.Mid  ' Mid
                If cMid = Me.BaseShip.MidSlots Then
                    MessageBox.Show("There are no available mid slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.High  ' High
                If cHi = Me.BaseShip.HiSlots Then
                    MessageBox.Show("There are no available high slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case SlotTypes.Subsystem  ' Subsystem
                If cSub = Me.BaseShip.SubSlots Then
                    MessageBox.Show("There are no available subsystem slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
        End Select

        ' Now check launcher slots
        If shipMod.IsLauncher Then
            If cLauncher = Me.BaseShip.LauncherSlots Then
                MessageBox.Show("There are no available launcher slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        ' Now check turret slots
        If shipMod.IsTurret Then
            If cTurret = Me.BaseShip.TurretSlots Then
                MessageBox.Show("There are no available turret slots remaining.", "Slot Allocation Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        End If

        Return True
    End Function
	Private Function IsModulePermitted(ByRef shipMod As ShipModule) As Boolean
		' Check for subsystem restrictions
		If shipMod.DatabaseCategory = "32" Then
			' Check for subsystem type restriction
			If CStr(shipMod.Attributes("1380")) <> CStr(Me.BaseShip.ID) Then
				MessageBox.Show("You cannot fit a subsystem module designed for a " & EveHQ.Core.HQ.itemData(CStr(shipMod.Attributes("1380"))).Name & " to your " & Me.BaseShip.Name & ".", "Ship Type Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return False
			End If
			' Check for subsystem group restriction
			For s As Integer = 1 To Me.BaseShip.SubSlots
				If Me.BaseShip.SubSlot(s) IsNot Nothing Then
					If CStr(shipMod.Attributes("1366")) = CStr(Me.BaseShip.SubSlot(s).Attributes("1366")) Then
						MessageBox.Show("You already have a subsystem of this type fitted to your ship.", "Subsystem Group Duplication", MessageBoxButtons.OK, MessageBoxIcon.Information)
						Return False
					End If
				End If
			Next
		End If

		' Check for Rig restrictions
		If shipMod.SlotType = 1 Then
			If shipMod.Attributes.ContainsKey("1547") Then
				If CInt(shipMod.Attributes("1547")) <> CInt(Me.BaseShip.Attributes("1547")) Then
					Dim requiredSize As String = ""
					Select Case CInt(Me.BaseShip.Attributes("1547"))
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
					MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & Me.BaseShip.Name & ". HQF has therefore substituted the " & requiredSize & " variant instead.", "Rig Size Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
					shipMod = CType(ModuleLists.moduleList(ModuleLists.moduleListName(baseModName)), ShipModule)
					'Return False
				End If
			End If
		End If

		' Check for ship group restrictions
		Dim ShipGroups As New ArrayList
		For att As Integer = 1298 To 1301
			If shipMod.Attributes.ContainsKey(att.ToString) = True Then
				ShipGroups.Add(CStr(shipMod.Attributes(att.ToString)))
			End If
		Next
		If ShipGroups.Count > 0 And ShipGroups.Contains(Me.BaseShip.DatabaseGroup) = False Then
			MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & Me.BaseShip.Name & ".", "Ship Group Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return False
		End If
		ShipGroups.Clear()

		' Check for ship type restrictions
		Dim ShipTypes As New ArrayList
		For att As Integer = 1302 To 1305
			If shipMod.Attributes.ContainsKey(att.ToString) = True Then
				ShipTypes.Add(CStr(shipMod.Attributes(att.ToString)))
			End If
		Next
		If ShipTypes.Count > 0 And ShipTypes.Contains(Me.BaseShip.ID) = False Then
			MessageBox.Show("You cannot fit a " & shipMod.Name & " to your " & Me.BaseShip.Name & ".", "Ship Type Restriction", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return False
		End If

		' Check for maxGroupActive flag
		If shipMod.Attributes.ContainsKey("763") = True Then
			If shipMod.DatabaseGroup <> "316" Then
				If IsModuleGroupLimitExceeded(shipMod, True) = True Then
					' Set the module offline
					shipMod.ModuleState = ModuleStates.Inactive
				End If
			Else
				' Check active command relay bonus (attID=435) on ship
				If IsModuleGroupLimitExceeded(shipMod, True) = True Then
					' Set the module offline
					shipMod.ModuleState = ModuleStates.Inactive
				Else
					If CountActiveTypeModules(shipMod.ID) >= CInt(shipMod.Attributes("763")) Then
						' Set the module offline
						shipMod.ModuleState = ModuleStates.Inactive
					End If
				End If
			End If
		End If

		Return True
	End Function
    Public Function IsModuleGroupLimitExceeded(ByVal testMod As ShipModule, ByVal excludeTestMod As Boolean) As Boolean
        Dim count As Integer = 0
        Dim fittedMod As ShipModule = testMod.Clone
        Me.ApplySkillEffectsToModule(fittedMod, True)
        Dim maxAllowed As Integer = 0
        If fittedMod.DatabaseGroup = "316" Then
            maxAllowed = CInt(Me.BaseShip.Attributes("10063"))
            If Me.BaseShip.Attributes.ContainsKey("435") = True Then
                maxAllowed += CInt(Me.BaseShip.Attributes("435"))
            End If
        Else
            maxAllowed = CInt(fittedMod.Attributes("763"))
        End If
        For slot As Integer = 1 To Me.BaseShip.HiSlots
            If Me.BaseShip.HiSlot(slot) IsNot Nothing Then
                If Me.BaseShip.HiSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.HiSlot(slot).ModuleState >= 4 Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.MidSlots
            If Me.BaseShip.MidSlot(slot) IsNot Nothing Then
                If Me.BaseShip.MidSlot(slot).ID <> "11014" Then
                    If Me.BaseShip.MidSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.MidSlot(slot).ModuleState >= 4 Then
                        count += 1
                    End If
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.LowSlots
            If Me.BaseShip.LowSlot(slot) IsNot Nothing Then
                If Me.BaseShip.LowSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.LowSlot(slot).ModuleState >= 4 Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.RigSlots
            If Me.BaseShip.RigSlot(slot) IsNot Nothing Then
                If Me.BaseShip.RigSlot(slot).DatabaseGroup = testMod.DatabaseGroup And Me.BaseShip.RigSlot(slot).ModuleState >= 4 Then
                    count += 1
                End If
            End If
        Next
        If excludeTestMod = True Then
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
    Public Function CountActiveTypeModules(ByVal typeID As String) As Integer
        Dim count As Integer = 0
        For slot As Integer = 1 To Me.BaseShip.HiSlots
            If Me.BaseShip.HiSlot(slot) IsNot Nothing Then
                If Me.BaseShip.HiSlot(slot).ID = typeID And Me.BaseShip.HiSlot(slot).ModuleState >= 4 Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.MidSlots
            If Me.BaseShip.MidSlot(slot) IsNot Nothing Then
                If Me.BaseShip.MidSlot(slot).ID = typeID And Me.BaseShip.MidSlot(slot).ModuleState >= 4 Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.LowSlots
            If Me.BaseShip.LowSlot(slot) IsNot Nothing Then
                If Me.BaseShip.LowSlot(slot).ID = typeID And Me.BaseShip.LowSlot(slot).ModuleState >= 4 Then
                    count += 1
                End If
            End If
        Next
        For slot As Integer = 1 To Me.BaseShip.RigSlots
            If Me.BaseShip.RigSlot(slot) IsNot Nothing Then
                If Me.BaseShip.RigSlot(slot).ID = typeID And Me.BaseShip.RigSlot(slot).ModuleState >= 4 Then
                    count += 1
                End If
            End If
        Next
        Return count
    End Function
    Private Function AddModuleInNextSlot(ByVal shipMod As ShipModule) As Integer
        Select Case shipMod.SlotType
            Case SlotTypes.Rig  ' Rig
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.RigSlot(slotNo) Is Nothing Then
                        Me.BaseShip.RigSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available rig slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.Low  ' Low
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.LowSlot(slotNo) Is Nothing Then
                        Me.BaseShip.LowSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available low slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.Mid  ' Mid
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.MidSlot(slotNo) Is Nothing Then
                        Me.BaseShip.MidSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available mid slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.High  ' High
                For slotNo As Integer = 1 To 8
                    If Me.BaseShip.HiSlot(slotNo) Is Nothing Then
                        Me.BaseShip.HiSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available high slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Case SlotTypes.Subsystem  ' Subsystem
                For slotNo As Integer = 1 To 5
                    If Me.BaseShip.SubSlot(slotNo) Is Nothing Then
                        Me.BaseShip.SubSlot(slotNo) = shipMod
                        shipMod.SlotNo = slotNo
                        Return slotNo
                    End If
                Next
                MessageBox.Show("There was an error finding the next available high slot.", "Slot Location Issue", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
        Return 0
    End Function
    Private Function AddModuleInSpecifiedSlot(ByVal shipMod As ShipModule, ByVal slotNo As Integer) As Integer
        Select Case shipMod.SlotType
            Case SlotTypes.Rig  ' Rig
                Me.BaseShip.RigSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.Low  ' Low
                Me.BaseShip.LowSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.Mid  ' Mid
                Me.BaseShip.MidSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.High  ' High
                Me.BaseShip.HiSlot(slotNo) = shipMod
                shipMod.SlotNo = slotNo
                Return slotNo
            Case SlotTypes.Subsystem  ' Subsystem
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
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(pilotName), HQFPilot)
        Dim truePilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(pilotName), Core.Pilot)
        Dim shipPilotSkills As New ArrayList
        Dim truePilotSkills As New ArrayList

        For Each rSkill As ReqSkill In allSkills.Values
            ' Check for shipPilot match
            If shipPilot.SkillSet.Contains(rSkill.Name) = True Then
                If CType(shipPilot.SkillSet(rSkill.Name), HQFSkill).Level < rSkill.ReqLevel Then
                    shipPilotSkills.Add(rSkill)
                End If
            Else
                shipPilotSkills.Add(rSkill)
            End If
            ' Check for truePilot match
            If truePilot.PilotSkills.Contains(rSkill.Name) = True Then
                If CType(truePilot.PilotSkills(rSkill.Name), EveHQ.Core.PilotSkill).Level < rSkill.ReqLevel Then
                    truePilotSkills.Add(rSkill)
                End If
            Else
                truePilotSkills.Add(rSkill)
            End If
        Next
        Dim NeededSkills As New NeededSkillsCollection
        NeededSkills.ShipPilotSkills = shipPilotSkills
        NeededSkills.TruePilotSkills = truePilotSkills
        Return NeededSkills
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
        Dim shipPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(Me.PilotName), HQFPilot)
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
    ''' <returns>The name of the ship used for the fitting</returns>
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



