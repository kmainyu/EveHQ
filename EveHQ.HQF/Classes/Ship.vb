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
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports ProtoBuf
Imports System.Runtime.Serialization

''' <summary>
''' Public class for storing details of a ship used for processing
''' </summary>
''' <remarks></remarks>
<ProtoContract()> <Serializable()> Public Class Ship

#Region "Constants"
    Const MaxBasicSlots As Integer = 8
    Const MaxRigSlots As Integer = 3
    Const MaxSubSlots As Integer = 5
#End Region

    ' ReSharper disable InconsistentNaming - for MS serialization compatability

#Region "Property Variables"

    Private _cName As String
    Private _cID As Integer
    Private _cMarketGroup As Integer
    Private _cDatabaseGroup As Integer
    Private _cDatabaseCategory As Integer
    Private _cDescription As String
    Private _cBasePrice As Double
    Private _cMarketPrice As Double
    Private _cRaceID As Integer
    Private _cIcon As String
    Private _cFittingBasePrice As Double
    Private _cFittingMarketPrice As Double
    Private _cHiSlots As Integer
    Private _cMidSlots As Integer
    Private _cLowSlots As Integer
    Private _cRigSlots As Integer
    Private _cSubSlots As Integer
    Private _cTurretSlots As Integer
    Private _cLauncherSlots As Integer
    Private _cCalibration As Integer
    Private _cOverrideFittingRules As Boolean = False
    Private _cSlotCollection As New List(Of ShipModule)
    Private _cRemoteSlotCollection As New List(Of ShipModule)
    Private _cFleetSlotCollection As New List(Of ShipModule)
    Private _cEnviroSlotCollection As New List(Of ShipModule)
    Private _cBoosterSlotCollection As New List(Of ShipModule)
    Private _cCPU As Double
    Private _cPG As Double
    Private _cCapCapacity As Double
    Private _cCapRecharge As Double
    Private _cShieldCapacity As Double
    Private _cShieldRecharge As Double
    Private _cShieldEMResist As Double
    Private _cShieldExResist As Double
    Private _cShieldKiResist As Double
    Private _cShieldThResist As Double
    Private _cArmorCapacity As Double
    Private _cArmorEMResist As Double
    Private _cArmorExResist As Double
    Private _cArmorKiResist As Double
    Private _cArmorThResist As Double
    Private _cStructureCapacity As Double
    Private _cStructureEMResist As Double
    Private _cStructureExResist As Double
    Private _cStructureKiResist As Double
    Private _cStructureThResist As Double
    Private _cCargoBay As Double
    Private _cMass As Double
    Private _cVolume As Double
    Private _cDroneBay As Double
    Private _cDroneBandwidth As Double
    Private _cUsedDrones As Integer
    Private _cMaxDrones As Integer
    Private _cShipBay As Double
    Private _cMaxLockedTargets As Double
    Private _cMaxTargetRange As Double
    Private _cTargetingSpeed As Double
    Private _cScanResolution As Double
    Private _cSigRadius As Double
    Private _cGravSensorStrenth As Double
    Private _cLadarSensorStrenth As Double
    Private _cMagSensorStrenth As Double
    Private _cRadarSensorStrenth As Double
    Private _cMaxVelocity As Double
    Private _cInertia As Double
    Private _cFusionPropStrength As Double
    Private _cIonPropStrength As Double
    Private _cMagpulsePropStrength As Double
    Private _cPlasmaPropStrength As Double
    Private _cWarpSpeed As Double
    Private _cWarpCapNeed As Double
    Private _cHiSlot(8) As ShipModule
    Private ReadOnly _cMidSlot(8) As ShipModule
    Private ReadOnly _cLowSlot(8) As ShipModule
    Private ReadOnly _cRigSlot(8) As ShipModule
    Private ReadOnly _cSubSlot(5) As ShipModule
    Private _cRequiredSkills As New SortedList(Of String, ItemSkills)
    Private _cRequiredSkillList As New SortedList(Of String, ItemSkills)
    Private _cAttributes As New SortedList(Of Integer, Double)
    Private _cHiSlotsUsed As Integer
    Private _cMidSlotsUsed As Integer
    Private _cLowSlotsUsed As Integer
    Private _cRigSlotsUsed As Integer
    Private _cSubSlotsUsed As Integer
    Private _cTurretSlotsUsed As Integer
    Private _cLauncherSlotsUsed As Integer
    Private _cCalibrationUsed As Integer
    Private _cCPUUsed As Double
    Private _cPGUsed As Double
    Private _cCargoBayUsed As Double
    Private _cCargoBayAdditional As Double
    Private _cDroneBayUsed As Double
    Private _cShipBayUsed As Double
    Private _cDroneBandwidthUsed As Double
    Private _cCargoBayItems As New SortedList(Of Integer, CargoBayItem)
    Private _cDroneBayItems As New SortedList(Of Integer, DroneBayItem)
    Private _cShipBayItems As New SortedList(Of Integer, ShipBayItem)
    Private _cEffectiveShieldHp As Double
    Private _cEffectiveArmorHp As Double
    Private _cEffectiveStructureHp As Double
    Private _cEffectiveHp As Double
    Private _cEveEffectiveShieldHp As Double
    Private _cEveEffectiveArmorHp As Double
    Private _cEveEffectiveStructureHp As Double
    Private _cEveEffectiveHp As Double
    Private _cDamageProfile As HQFDamageProfile
    Private _cEM As Double
    Private _cEx As Double
    Private _cKi As Double
    Private _cTh As Double
    Private _cEMExKiTh As Double
    Private _cAffects As New List(Of String)
    Private _cGlobalAffects As List(Of String)

    Private _cTurretVolley As Double
    Private _cMissileVolley As Double
    Private _cSmartbombVolley As Double
    Private _cBombVolley As Double
    Private _cDroneVolley As Double
    Private _cTurretDPS As Double
    Private _cMissileDPS As Double
    Private _cSmartbombDPS As Double
    Private _cBombDPS As Double
    Private _cDroneDPS As Double
    Private _cTotalVolley As Double
    Private _cTotalDPS As Double
    Private _cOreTurretAmount As Double
    Private _cOreDroneAmount As Double
    Private _cOreTotalAmount As Double
    Private _cIceTurretAmount As Double
    Private _cIceDroneAmount As Double
    Private _cIceTotalAmount As Double
    Private _cOreTurretRate As Double
    Private _cOreDroneRate As Double
    Private _cOreTotalRate As Double
    Private _cIceTurretRate As Double
    Private _cIceDroneRate As Double
    Private _cIceTotalRate As Double
    Private _cAuditLog As New List(Of String)

#End Region

#Region "Properties"

#Region "Database Properties"

    ' Note: These properties (except the RaceID) should not be directly editable in the property editor, but still visible

    <ProtoMember(109)> <[ReadOnly](True)> _
    <Description("The name of the ship")> <Category("Database")> Public Property Name() As String
        Get
            Return _cName
        End Get
        Set(ByVal value As String)
            _cName = value
        End Set
    End Property

    <ProtoMember(1)> <[ReadOnly](True)> _
    <Description("The ID of the ship")> <Category("Database")> Public Property ID() As Integer
        Get
            Return _cID
        End Get
        Set(ByVal value As Integer)
            _cID = value
        End Set
    End Property

    <ProtoMember(2)> <[ReadOnly](True)> _
    <Description("The market group ID of the ship")> <Category("Database")> Public Property MarketGroup() As Integer
        Get
            Return _cMarketGroup
        End Get
        Set(ByVal value As Integer)
            _cMarketGroup = value
        End Set
    End Property

    <ProtoMember(3)> <[ReadOnly](True)> _
    <Description("The database group ID of the ship")> <Category("Database")> Public Property DatabaseGroup() As Integer
        Get
            Return _cDatabaseGroup
        End Get
        Set(ByVal value As Integer)
            _cDatabaseGroup = value
        End Set
    End Property

    <ProtoMember(4)> <[ReadOnly](True)> _
    <Description("The database category ID of the ship")> <Category("Database")> Public Property DatabaseCategory() As Integer
        Get
            Return _cDatabaseCategory
        End Get
        Set(ByVal value As Integer)
            _cDatabaseCategory = value
        End Set
    End Property

    <ProtoMember(5)> <[ReadOnly](True)> _
    <Description("The description of the ship")> <Category("Database")> Public Property Description() As String
        Get
            Return _cDescription
        End Get
        Set(ByVal value As String)
            _cDescription = value
        End Set
    End Property

    <ProtoMember(6)> <Description("The raceID of the ship")> <Category("Database")> Public Property RaceID() As Integer
        Get
            Return _cRaceID
        End Get
        Set(ByVal value As Integer)
            _cRaceID = value
        End Set
    End Property

    <ProtoMember(7)> <[ReadOnly](True)> _
    <Description("The icon ID of the ship")> <Category("Database")> Public Property Icon() As String
        Get
            Return _cIcon
        End Get
        Set(ByVal value As String)
            _cIcon = value
        End Set
    End Property

#End Region

#Region "Price Properties"

    ' Nore: These properties should not be visible in the property editor as they are irrelevant

    <ProtoMember(8)> <Browsable(False)> _
    <Description("The base price of the ship")> <Category("Price")> Public Property BasePrice() As Double
        Get
            Return _cBasePrice
        End Get
        Set(ByVal value As Double)
            _cBasePrice = value
        End Set
    End Property

    <ProtoMember(9)> <Browsable(False)> _
    <Description("The market price of the ship")> <Category("Price")> Public Property MarketPrice() As Double
        Get
            Return _cMarketPrice
        End Get
        Set(ByVal value As Double)
            _cMarketPrice = value
        End Set
    End Property

    <ProtoMember(10)> <Browsable(False)> _
    <Description("The base price of the ship including all fittings")> <Category("Price")> Public Property FittingBasePrice() As Double
        Get
            Return _cFittingBasePrice
        End Get
        Set(ByVal value As Double)
            _cFittingBasePrice = value
        End Set
    End Property

    <ProtoMember(11)> <Browsable(False)> _
    <Description("The market price of the ship including all fittings")> <Category("Price")> Public Property FittingMarketPrice() As Double
        Get
            Return _cFittingMarketPrice
        End Get
        Set(ByVal value As Double)
            _cFittingMarketPrice = value
        End Set
    End Property

#End Region

#Region "Fitting Properties"

    <ProtoMember(12)> <Description("The number of available high slots on the ship")> <Category("Fitting")> Public Property HiSlots() As Integer
        Get
            Return _cHiSlots
        End Get
        Set(ByVal value As Integer)
            If _cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("High slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _cHiSlots = value
                End If
            Else
                _cHiSlots = value
            End If
        End Set
    End Property

    <ProtoMember(13)> <Description("The number of available mid slots on the ship")> <Category("Fitting")> Public Property MidSlots() As Integer
        Get
            Return _cMidSlots
        End Get
        Set(ByVal value As Integer)
            If _cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Mid slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _cMidSlots = value
                End If
            Else
                _cHiSlots = value
            End If
        End Set
    End Property

    <ProtoMember(14)> <Description("The number of available low slots on the ship")> <Category("Fitting")> Public Property LowSlots() As Integer
        Get
            Return _cLowSlots
        End Get
        Set(ByVal value As Integer)
            If _cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Low slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _cLowSlots = value
                End If
            Else
                _cHiSlots = value
            End If
        End Set
    End Property

    <ProtoMember(15)> <Description("The number of available rig slots on the ship")> <Category("Fitting")> Public Property RigSlots() As Integer
        Get
            Return _cRigSlots
        End Get
        Set(ByVal value As Integer)
            If _cOverrideFittingRules = False Then
                If value < 0 Or value > MaxRigSlots Then
                    MessageBox.Show("Rig slots must be between 0 and " & MaxRigSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _cRigSlots = value
                End If
            Else
                _cHiSlots = value
            End If
        End Set
    End Property

    <ProtoMember(16)> <Description("The number of available subsystem slots on the ship")> <Category("Fitting")> Public Property SubSlots() As Integer
        Get
            Return _cSubSlots
        End Get
        Set(ByVal value As Integer)
            If _cOverrideFittingRules = False Then
                If value <> MaxSubSlots And value <> 0 Then
                    MessageBox.Show("The number of subsystem slots is currently restricted to " & MaxSubSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _cSubSlots = value
                End If
            Else
                _cHiSlots = value
            End If
        End Set
    End Property

    <ProtoMember(17)> <Description("The number of available turret slots on the ship")> <Category("Fitting")> Public Property TurretSlots() As Integer
        Get
            Return _cTurretSlots
        End Get
        Set(ByVal value As Integer)
            If _cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Turret slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _cTurretSlots = value
                End If
            Else
                _cHiSlots = value
            End If
        End Set
    End Property

    <ProtoMember(18)> <Description("The number of available launcher slots on the ship")> <Category("Fitting")> Public Property LauncherSlots() As Integer
        Get
            Return _cLauncherSlots
        End Get
        Set(ByVal value As Integer)
            If _cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Launcher slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _cLauncherSlots = value
                End If
            Else
                _cHiSlots = value
            End If
        End Set
    End Property

    <ProtoMember(19)> <Description("The maximum CPU available for all modules on the ship")> <Category("Fitting")> Public Property CPU() As Double
        Get
            Return _cCPU
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("CPU must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cCPU = value
            End If
        End Set
    End Property

    <ProtoMember(20)> <Description("The maximum powergrid available for all modules on the ship")> <Category("Fitting")> Public Property PG() As Double
        Get
            Return _cPG
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Powergrid must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cPG = value
            End If
        End Set
    End Property

    <ProtoMember(21)> <Description("The maximum available calibration units available for rigs")> <Category("Fitting")> Public Property Calibration() As Integer
        Get
            Return _cCalibration
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Calibration must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cCalibration = value
            End If
        End Set
    End Property

    <ProtoMember(22)> <Browsable(False)> _
    <Description("Allows fitting rules to be relaxed to increase fitting limits")> <Category("Fitting")> Public Property OverrideFittingRules() As Boolean
        Get
            Return _cOverrideFittingRules
        End Get
        Set(ByVal value As Boolean)
            _cOverrideFittingRules = value
        End Set
    End Property

#End Region

#Region "Slot Collection Properties"

    ' Note: None of these properties should be visible in the property editor

    <ProtoMember(23)> <Browsable(False)> _
    <Description("The collection of internal modules for the ship")> <Category("Slot Collection")> Public Property SlotCollection() As List(Of ShipModule)
        Get
            Return _cSlotCollection
        End Get
        Set(ByVal value As List(Of ShipModule))
            _cSlotCollection = value
        End Set
    End Property

    <ProtoMember(24)> <Browsable(False)> _
    <Description("The collection of remote modules for the ship")> <Category("Slot Collection")> Public Property RemoteSlotCollection() As List(Of ShipModule)
        Get
            Return _cRemoteSlotCollection
        End Get
        Set(ByVal value As List(Of ShipModule))
            _cRemoteSlotCollection = value
        End Set
    End Property

    <ProtoMember(25)> <Browsable(False)> _
    <Description("The collection of fleet-based modules for the ship")> <Category("Slot Collection")> Public Property FleetSlotCollection() As List(Of ShipModule)
        Get
            Return _cFleetSlotCollection
        End Get
        Set(ByVal value As List(Of ShipModule))
            _cFleetSlotCollection = value
        End Set
    End Property

    <ProtoMember(26)> <Browsable(False)> _
    <Description("The collection of external environment modules for the ship")> <Category("Slot Collection")> Public Property EnviroSlotCollection() As List(Of ShipModule)
        Get
            Return _cEnviroSlotCollection
        End Get
        Set(ByVal value As List(Of ShipModule))
            _cEnviroSlotCollection = value
        End Set
    End Property

    <ProtoMember(27)> <Browsable(False)> _
    <Description("The collection of combat boosters for the ship")> <Category("Slot Collection")> Public Property BoosterSlotCollection() As List(Of ShipModule)
        Get
            Return _cBoosterSlotCollection
        End Get
        Set(ByVal value As List(Of ShipModule))
            _cBoosterSlotCollection = value
        End Set
    End Property

#End Region

#Region "Capacitor Properties"

    <ProtoMember(28)> <Description("The total available capacitor of the ship")> <Category("Capacitor")> Public Property CapCapacity() As Double
        Get
            Return _cCapCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Capacitor capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cCapCapacity = value
            End If
        End Set
    End Property

    <ProtoMember(29)> <Description("The capacitor recharge time of the ship")> <Category("Capacitor")> Public Property CapRecharge() As Double
        Get
            Return _cCapRecharge
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Capacitor recharge time must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cCapRecharge = value
            End If
        End Set
    End Property

#End Region

#Region "Shield Properties"

    <ProtoMember(30)> <Description("The shield hitpoint capacity of the ship")> <Category("Shield")> Public Property ShieldCapacity() As Double
        Get
            Return _cShieldCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Shield capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cShieldCapacity = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <ProtoMember(31)> <Description("The shield recharge time of the ship")> <Category("Shield")> Public Property ShieldRecharge() As Double
        Get
            Return _cShieldRecharge
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Shield recharge time must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cShieldRecharge = value
            End If
        End Set
    End Property

    <ProtoMember(32)> <Description("The shield EM resistance of the ship")> <Category("Shield")> Public Property ShieldEMResist() As Double
        Get
            Return _cShieldEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cShieldEMResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <ProtoMember(33)> <Description("The shield Explosive resistance of the ship")> <Category("Shield")> Public Property ShieldExResist() As Double
        Get
            Return _cShieldExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cShieldExResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <ProtoMember(34)> <Description("The shield Kinetic resistance of the ship")> <Category("Shield")> Public Property ShieldKiResist() As Double
        Get
            Return _cShieldKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cShieldKiResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <ProtoMember(35)> <Description("The shield Thermal resistance of the ship")> <Category("Shield")> Public Property ShieldThResist() As Double
        Get
            Return _cShieldThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cShieldThResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

#End Region

#Region "Armor Properties"

    <ProtoMember(36)> <Description("The armor hitpoint capacity of the ship")> <Category("Armor")> Public Property ArmorCapacity() As Double
        Get
            Return _cArmorCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Armor capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cArmorCapacity = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <ProtoMember(37)> <Description("The armor EM resistance of the ship")> <Category("Armor")> Public Property ArmorEMResist() As Double
        Get
            Return _cArmorEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cArmorEMResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <ProtoMember(38)> <Description("The armor Explosive resistance of the ship")> <Category("Armor")> Public Property ArmorExResist() As Double
        Get
            Return _cArmorExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cArmorExResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <ProtoMember(39)> <Description("The armor Kinetic resistance of the ship")> <Category("Armor")> Public Property ArmorKiResist() As Double
        Get
            Return _cArmorKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cArmorKiResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <ProtoMember(40)> <Description("The armor Thermal resistance of the ship")> <Category("Armor")> Public Property ArmorThResist() As Double
        Get
            Return _cArmorThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cArmorThResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property

#End Region

#Region "Hull Properties"

    <ProtoMember(41)> <Description("The hull hitpoint capacity of the ship")> <Category("Hull")> Public Property StructureCapacity() As Double
        Get
            Return _cStructureCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Structure capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cStructureCapacity = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <ProtoMember(42)> <Description("The hull EM resistance of the ship")> <Category("Hull")> Public Property StructureEMResist() As Double
        Get
            Return _cStructureEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cStructureEMResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <ProtoMember(43)> <Description("The hull Explosive resistance of the ship")> <Category("Hull")> Public Property StructureExResist() As Double
        Get
            Return _cStructureExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cStructureExResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <ProtoMember(44)> <Description("The hull Kinetic resistance of the ship")> <Category("Hull")> Public Property StructureKiResist() As Double
        Get
            Return _cStructureKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cStructureKiResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <ProtoMember(45)> <Description("The hull Thermal resistance of the ship")> <Category("Hull")> Public Property StructureThResist() As Double
        Get
            Return _cStructureThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cStructureThResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

#End Region

#Region "Structure Properties"

    <ProtoMember(46)> <Description("The mass of the ship")> <Category("Structure")> Public Property Mass() As Double
        Get
            Return _cMass
        End Get
        Set(ByVal value As Double)
            _cMass = value
        End Set
    End Property

    <ProtoMember(47)> <Description("The unpacked volume of the ship")> <Category("Structure")> Public Property Volume() As Double
        Get
            Return _cVolume
        End Get
        Set(ByVal value As Double)
            _cVolume = value
        End Set
    End Property

#End Region

#Region "Storage Bay Properties"

    <ProtoMember(48)> <Description("The cargo bay capacity of the ship")> <Category("Storage")> Public Property CargoBay() As Double
        Get
            Return _cCargoBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Cargo Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cCargoBay = value
            End If
        End Set
    End Property

    <ProtoMember(49)> <Description("The ship maintenance bay capacity of the ship")> <Category("Storage")> Public Property ShipBay() As Double
        Get
            Return _cShipBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Ship Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cShipBay = value
            End If
        End Set
    End Property

#End Region

#Region "Drone Properties"

    <ProtoMember(50)> <Description("The drone bay capacity of the ship")> <Category("Drones")> Public Property DroneBay() As Double
        Get
            Return _cDroneBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Drone Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cDroneBay = value
            End If
        End Set
    End Property

    <ProtoMember(51)> <Description("The maxmium drone bandwidth capability of the ship")> <Category("Drones")> Public Property DroneBandwidth() As Double
        Get
            Return _cDroneBandwidth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Drone bandwidth must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cDroneBandwidth = value
            End If
        End Set
    End Property

    <ProtoMember(52)> <Browsable(False)> _
    <Description("The number of drones currently used by the ship")> <Category("Drones")> Public Property UsedDrones() As Integer
        Get
            Return _cUsedDrones
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Used Drones must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cUsedDrones = value
            End If
        End Set
    End Property

    <ProtoMember(53)> <Description("The maximum amount of drones to be used by the ship")> <Category("Drones")> Public Property MaxDrones() As Integer
        Get
            Return _cMaxDrones
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Maximum drones must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cMaxDrones = value
            End If
        End Set
    End Property

#End Region

#Region "Targeting Properties"

    <ProtoMember(54)> <Description("The maximum number of locked targets allowed by the ship")> <Category("Targeting")> Public Property MaxLockedTargets() As Double
        Get
            Return _cMaxLockedTargets
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Locked Targets must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cMaxLockedTargets = value
            End If
        End Set
    End Property

    <ProtoMember(55)> <Description("The maximum targeting range of the ship")> <Category("Targeting")> Public Property MaxTargetRange() As Double
        Get
            Return _cMaxTargetRange
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Targeting Range must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cMaxTargetRange = value
            End If
        End Set
    End Property

    <ProtoMember(56)> <Description("The base targeting speed of the ship")> <Category("Targeting")> Public Property TargetingSpeed() As Double
        Get
            Return _cTargetingSpeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Base Targeting Speed must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cTargetingSpeed = value
            End If
        End Set
    End Property

    <ProtoMember(57)> <Description("The scan resolution of the ship")> <Category("Targeting")> Public Property ScanResolution() As Double
        Get
            Return _cScanResolution
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Scan Resolution must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cScanResolution = value
            End If
        End Set
    End Property

    <ProtoMember(58)> <Description("The signature radius of the ship")> <Category("Targeting")> Public Property SigRadius() As Double
        Get
            Return _cSigRadius
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Signature Radius must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cSigRadius = value
            End If
        End Set
    End Property

    <ProtoMember(59)> <Description("The Gravimetric sensor strength of the ship")> <Category("Targeting")> Public Property GravSensorStrenth() As Double
        Get
            Return _cGravSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cGravSensorStrenth = value
            End If
        End Set
    End Property

    <ProtoMember(60)> <Description("The LADAR sensor strength of the ship")> <Category("Targeting")> Public Property LadarSensorStrenth() As Double
        Get
            Return _cLadarSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cLadarSensorStrenth = value
            End If
        End Set
    End Property

    <ProtoMember(61)> <Description("The Magnetometric sensor strength of the ship")> <Category("Targeting")> Public Property MagSensorStrenth() As Double
        Get
            Return _cMagSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cMagSensorStrenth = value
            End If
        End Set
    End Property

    <ProtoMember(62)> <Description("The RADAR sensor strength of the ship")> <Category("Targeting")> Public Property RadarSensorStrenth() As Double
        Get
            Return _cRadarSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cRadarSensorStrenth = value
            End If
        End Set
    End Property

#End Region

#Region "Propulsion Properties"

    <ProtoMember(63)> <Description("The maxmium velocity of the ship")> <Category("Propulsion")> Public Property MaxVelocity() As Double
        Get
            Return _cMaxVelocity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Velocity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cMaxVelocity = value
            End If
        End Set
    End Property

    <ProtoMember(64)> <Description("The inertia (agility) modifier of the ship")> <Category("Propulsion")> Public Property Inertia() As Double
        Get
            Return _cInertia
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Inertia Modifier must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cInertia = value
            End If
        End Set
    End Property

    <ProtoMember(65)> <Description("The Fusion propulsion strength of the ship")> <Category("Propulsion")> Public Property FusionPropStrength() As Double
        Get
            Return _cFusionPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cFusionPropStrength = value
            End If
        End Set
    End Property

    <ProtoMember(66)> <Description("The Ion propulsion strength of the ship")> <Category("Propulsion")> Public Property IonPropStrength() As Double
        Get
            Return _cIonPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cIonPropStrength = value
            End If
        End Set
    End Property

    <ProtoMember(67)> <Description("The Magpulse propulsion strength of the ship")> <Category("Propulsion")> Public Property MagpulsePropStrength() As Double
        Get
            Return _cMagpulsePropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cMagpulsePropStrength = value
            End If
        End Set
    End Property

    <ProtoMember(68)> <Description("The Plasma propulsion strength of the ship")> <Category("Propulsion")> Public Property PlasmaPropStrength() As Double
        Get
            Return _cPlasmaPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cPlasmaPropStrength = value
            End If
        End Set
    End Property

    <ProtoMember(69)> <Description("The maxmium warp velocity of the ship")> <Category("Propulsion")> Public Property WarpSpeed() As Double
        Get
            Return _cWarpSpeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Warp Speed must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cWarpSpeed = value
            End If
        End Set
    End Property

    <ProtoMember(70)> <Description("The capacitor required to move 1kg a distance of 1au")> <Category("Propulsion")> Public Property WarpCapNeed() As Double
        Get
            Return _cWarpCapNeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Warp Capacitor Need must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cWarpCapNeed = value
            End If
        End Set
    End Property

#End Region

#Region "Fitted Slot Properties"

    '<ProtoMember(71)> <Browsable(False)> _
    <Description("The fitted high slots of the ship")> <Category("Fitted Slots")> Public Property HiSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _cHiSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("High Slot index must be in the range 1 to " & _cHiSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _cHiSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _cHiSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("High Slot index must be in the range 1 to " & _cHiSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If _cHiSlots > 8 Then ReDim Preserve _cHiSlot(_cHiSlots) ' Used if we artifically expand the slot count for weapon analysis
                If value Is Nothing Then
                    If _cHiSlot(index) IsNot Nothing Then
                        _cHiSlotsUsed -= 1
                        If _cHiSlot(index).IsLauncher Then
                            _cLauncherSlotsUsed -= 1
                        ElseIf _cHiSlot(index).IsTurret Then
                            _cTurretSlotsUsed -= 1
                        End If
                        _cFittingBasePrice -= _cHiSlot(index).BasePrice
                    End If
                Else
                    If _cHiSlot(index) IsNot Nothing Then
                        _cHiSlotsUsed -= 1
                        If _cHiSlot(index).IsLauncher Then
                            _cLauncherSlotsUsed -= 1
                        ElseIf _cHiSlot(index).IsTurret Then
                            _cTurretSlotsUsed -= 1
                        End If
                        _cFittingBasePrice -= _cHiSlot(index).BasePrice
                    End If
                    _cHiSlotsUsed += 1
                    If value.IsLauncher Then
                        _cLauncherSlotsUsed += 1
                    ElseIf value.IsTurret Then
                        _cTurretSlotsUsed += 1
                    End If
                    _cFittingBasePrice += value.BasePrice
                End If
                _cHiSlot(index) = value
            End If
        End Set
    End Property

    '<ProtoMember(72)> <Browsable(False)> _
    <Description("The fitted mid slots of the ship")> <Category("Fitted Slots")> Public Property MidSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _cMidSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Mid Slot index must be in the range 1 to " & _cMidSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _cMidSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _cMidSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Mid Slot index must be in the range 1 to " & _cMidSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _cMidSlot(index) IsNot Nothing Then
                        _cMidSlotsUsed -= 1
                        _cFittingBasePrice -= _cMidSlot(index).BasePrice
                    End If
                Else
                    If _cMidSlot(index) IsNot Nothing Then
                        _cMidSlotsUsed -= 1
                        _cFittingBasePrice -= _cMidSlot(index).BasePrice
                    End If
                    _cMidSlotsUsed += 1
                    _cFittingBasePrice += value.BasePrice
                End If
                _cMidSlot(index) = value
            End If
        End Set
    End Property

    ' <ProtoMember(73)> <Browsable(False)> _
    <Description("The fitted low slots of the ship")> <Category("Fitted Slots")> Public Property LowSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _cLowSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Low Slot index must be in the range 1 to " & _cLowSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _cLowSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _cLowSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Low Slot index must be in the range 1 to " & _cLowSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _cLowSlot(index) IsNot Nothing Then
                        _cLowSlotsUsed -= 1
                        _cFittingBasePrice -= _cLowSlot(index).BasePrice
                    End If
                Else
                    If _cLowSlot(index) IsNot Nothing Then
                        _cLowSlotsUsed -= 1
                        _cFittingBasePrice -= _cLowSlot(index).BasePrice
                    End If
                    _cLowSlotsUsed += 1
                    _cFittingBasePrice += value.BasePrice
                End If
                _cLowSlot(index) = value
            End If
        End Set
    End Property

    '<ProtoMember(74)> <Browsable(False)> _
    <Description("The fitted rig slots of the ship")> <Category("Fitted Slots")> Public Property RigSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _cRigSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Rig Slot index must be in the range 1 to " & _cRigSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _cRigSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _cRigSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Rig Slot index must be in the range 1 to " & _cRigSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _cRigSlot(index) IsNot Nothing Then
                        _cRigSlotsUsed -= 1
                        _cFittingBasePrice -= _cRigSlot(index).BasePrice
                    End If
                Else
                    If _cRigSlot(index) IsNot Nothing Then
                        _cRigSlotsUsed -= 1
                        _cFittingBasePrice -= _cRigSlot(index).BasePrice
                    End If
                    _cRigSlotsUsed += 1
                    _cFittingBasePrice += value.BasePrice
                End If
                _cRigSlot(index) = value
            End If
        End Set
    End Property

    '<ProtoMember(75)> <Browsable(False)> _
    <Description("The fitted subsystem slots of the ship")> <Category("Fitted Slots")> Public Property SubSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _cSubSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Subsystem Slot index must be in the range 1 to " & _cSubSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _cSubSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _cSubSlots) And _cOverrideFittingRules = False Then
                MessageBox.Show("Subsystem Slot index must be in the range 1 to " & _cSubSlots & " for " & _cName, "HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _cSubSlot(index) IsNot Nothing Then
                        _cSubSlotsUsed -= 1
                        _cFittingBasePrice -= _cSubSlot(index).BasePrice
                    End If
                Else
                    If _cSubSlot(index) IsNot Nothing Then
                        _cSubSlotsUsed -= 1
                        _cFittingBasePrice -= _cSubSlot(index).BasePrice
                    End If
                    _cSubSlotsUsed += 1
                    _cFittingBasePrice += value.BasePrice
                End If
                _cSubSlot(index) = value
            End If
        End Set
    End Property

#End Region

#Region "Fitting Used Properties"

    <ProtoMember(76)> <Browsable(False)> _
    <Description("The number of fitted high slots on the ship")> <Category("Fitting Used")> Public Property HiSlotsUsed() As Integer
        Get
            Return _cHiSlotsUsed
        End Get
        Set(ByVal value As Integer)
            _cHiSlotsUsed = value
        End Set
    End Property

    <ProtoMember(77)> <Browsable(False)> _
    <Description("The number of fitted mide slots on the ship")> <Category("Fitting Used")> Public Property MidSlotsUsed() As Integer
        Get
            Return _cMidSlotsUsed
        End Get
        Set(ByVal value As Integer)
            _cMidSlotsUsed = value
        End Set
    End Property

    <ProtoMember(78)> <Browsable(False)> _
    <Description("The number of fitted low slots on the ship")> <Category("Fitting Used")> Public Property LowSlotsUsed() As Integer
        Get
            Return _cLowSlotsUsed
        End Get
        Set(ByVal value As Integer)
            _cLowSlotsUsed = value
        End Set
    End Property

    <ProtoMember(79)> <Browsable(False)> _
    <Description("The number of fitted rig slots on the ship")> <Category("Fitting Used")> Public Property RigSlotsUsed() As Integer
        Get
            Return _cRigSlotsUsed
        End Get
        Set(ByVal value As Integer)
            _cRigSlotsUsed = value
        End Set
    End Property

    <ProtoMember(80)> <Browsable(False)> _
    <Description("The number of fitted subsystem slots on the ship")> <Category("Fitting Used")> Public Property SubSlotsUsed() As Integer
        Get
            Return _cSubSlotsUsed
        End Get
        Set(ByVal value As Integer)
            _cSubSlotsUsed = value
        End Set
    End Property

    <ProtoMember(81)> <Browsable(False)> _
    <Description("The number of fitted turret slots on the ship")> <Category("Fitting Used")> Public Property TurretSlotsUsed() As Integer
        Get
            Return _cTurretSlotsUsed
        End Get
        Set(ByVal value As Integer)
            _cTurretSlotsUsed = value
        End Set
    End Property

    <ProtoMember(82)> <Browsable(False)> _
    <Description("The number of fitted launcher slots on the ship")> <Category("Fitting Used")> Public Property LauncherSlotsUsed() As Integer
        Get
            Return _cLauncherSlotsUsed
        End Get
        Set(ByVal value As Integer)
            _cLauncherSlotsUsed = value
        End Set
    End Property

    <ProtoMember(83)> <Browsable(False)> _
    <Description("The amount of calibration points used on the ship")> <Category("Fitting Used")> Public Property CalibrationUsed() As Integer
        Get
            Return _cCalibrationUsed
        End Get
        Set(ByVal value As Integer)
            _cCalibrationUsed = value
        End Set
    End Property

    <ProtoMember(84)> <Browsable(False)> _
    <Description("The amount of CPU used on the ship")> <Category("Fitting Used")> Public Property CPUUsed() As Double
        Get
            Return _cCPUUsed
        End Get
        Set(ByVal value As Double)
            _cCPUUsed = value
        End Set
    End Property

    <ProtoMember(85)> <Browsable(False)> _
    <Description("The amount of powergrid used on the ship")> <Category("Fitting Used")> Public Property PGUsed() As Double
        Get
            Return _cPGUsed
        End Get
        Set(ByVal value As Double)
            _cPGUsed = value
        End Set
    End Property

    <ProtoMember(86)> <Browsable(False)> _
    <Description("The amount of cargo bay capacity used on the ship")> <Category("Fitting Used")> Public Property CargoBayUsed() As Double
        Get
            Return _cCargoBayUsed
        End Get
        Set(ByVal value As Double)
            _cCargoBayUsed = value
        End Set
    End Property

    <ProtoMember(87)> <Browsable(False)> _
    <Description("The amount of additional cargo bay capacity available on the ship")> <Category("Fitting Used")> Public Property CargoBayAdditional() As Double
        Get
            Return _cCargoBayAdditional
        End Get
        Set(ByVal value As Double)
            _cCargoBayAdditional = value
        End Set
    End Property

    <ProtoMember(88)> <Browsable(False)> _
    <Description("The amount of drone bay capacity used on the ship")> <Category("Fitting Used")> Public Property DroneBayUsed() As Double
        Get
            Return _cDroneBayUsed
        End Get
        Set(ByVal value As Double)
            _cDroneBayUsed = value
        End Set
    End Property

    <ProtoMember(89)> <Browsable(False)> _
    <Description("The amount of ship maintenance bay capacity used on the ship")> <Category("Fitting Used")> Public Property ShipBayUsed() As Double
        Get
            Return _cShipBayUsed
        End Get
        Set(ByVal value As Double)
            _cShipBayUsed = value
        End Set
    End Property

    <ProtoMember(90)> <Browsable(False)> _
    <Description("The amount of drone bandwidth used on the ship")> <Category("Fitting Used")> Public Property DroneBandwidthUsed() As Double
        Get
            Return _cDroneBandwidthUsed
        End Get
        Set(ByVal value As Double)
            _cDroneBandwidthUsed = value
        End Set
    End Property

#End Region

#Region "Skill Properties"

    <ProtoMember(91)> <Description("The minimum skills required to fly the ship hull")> <Category("Skills")> Public Property RequiredSkills() As SortedList(Of String, ItemSkills)
        Get
            Return _cRequiredSkills
        End Get
        Set(ByVal value As SortedList(Of String, ItemSkills))
            _cRequiredSkills = value
        End Set
    End Property

    <ProtoMember(92)> <Browsable(False)> _
    <Description("The minimum skills required to fly the ship (inlcuding all modules)")> <Category("Skills")> Public Property RequiredSkillList() As SortedList(Of String, ItemSkills)
        Get
            Return _cRequiredSkillList
        End Get
        Set(ByVal value As SortedList(Of String, ItemSkills))
            _cRequiredSkillList = value
        End Set
    End Property

#End Region

#Region "Attribute Properties"

    <ProtoMember(93)> <Description("The detailed attributes of the ship")> <Category("Attributes")> Public Property Attributes() As SortedList(Of Integer, Double)
        Get
            Return _cAttributes
        End Get
        Set(ByVal value As SortedList(Of Integer, Double))
            _cAttributes = value
        End Set
    End Property

#End Region

#Region "Storage Bay Items"

    <ProtoMember(94)> <Browsable(False)> _
    <Description("The collection of items stored in the cargo bay of the ship")> <Category("Storage Bay Items")> Public Property CargoBayItems() As SortedList(Of Integer, CargoBayItem)
        Get
            Return _cCargoBayItems
        End Get
        Set(ByVal value As SortedList(Of Integer, CargoBayItem))
            _cCargoBayItems = value
        End Set
    End Property

    <ProtoMember(95)> <Browsable(False)> _
    <Description("The collection of items stored in the drone bay of the ship")> <Category("Storage Bay Items")> Public Property DroneBayItems() As SortedList(Of Integer, DroneBayItem)
        Get
            Return _cDroneBayItems
        End Get
        Set(ByVal value As SortedList(Of Integer, DroneBayItem))
            _cDroneBayItems = value
        End Set
    End Property

    <ProtoMember(96)> <Browsable(False)> _
    <Description("The collection of items stored in the ship maintenance bay of the ship")> <Category("Storage Bay Items")> Public Property ShipBayItems() As SortedList(Of Integer, ShipBayItem)
        Get
            Return _cShipBayItems
        End Get
        Set(ByVal value As SortedList(Of Integer, ShipBayItem))
            _cShipBayItems = value
        End Set
    End Property

#End Region

#Region "Effective HP Properties (Read Only)"

    <Browsable(False)> _
    <Description("The effective shield HP based on shield resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveShieldHP() As Double
        Get
            Return _cEffectiveShieldHp
        End Get
    End Property

    <Browsable(False)> _
    <Description("The effective armor HP based on armor resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveArmorHP() As Double
        Get
            Return _cEffectiveArmorHp
        End Get
    End Property

    <Browsable(False)> _
    <Description("The effective hull HP based on hull resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveStructureHP() As Double
        Get
            Return _cEffectiveStructureHp
        End Get
    End Property

    <Browsable(False)> _
    <Description("The overall effective HP of the ship")> <Category("Effective HP")> Public ReadOnly Property EffectiveHP() As Double
        Get
            Return _cEffectiveHp
        End Get
    End Property

    <Browsable(False)> _
    <Description("The overall effective HP of the ship, as stated by Eve")> <Category("Effective HP")> Public ReadOnly Property EveEffectiveHP() As Double
        Get
            Return _cEveEffectiveHp
        End Get
    End Property

#End Region

#Region "Volley Damage Properties"

    <Browsable(False)> _
    <Description("The combined turret volley damage for the ship")> <Category("Volley Damage")> Public Property TurretVolley() As Double
        Get
            Return _cTurretVolley
        End Get
        Set(ByVal value As Double)
            _cTurretVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined missile volley damage for the ship")> <Category("Volley Damage")> Public Property MissileVolley() As Double
        Get
            Return _cMissileVolley
        End Get
        Set(ByVal value As Double)
            _cMissileVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined smartbomb volley damage for the ship")> <Category("Volley Damage")> Public Property SmartbombVolley() As Double
        Get
            Return _cSmartbombVolley
        End Get
        Set(ByVal value As Double)
            _cSmartbombVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined bomb volley damage for the ship")> <Category("Volley Damage")> Public Property BombVolley() As Double
        Get
            Return _cBombVolley
        End Get
        Set(ByVal value As Double)
            _cBombVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone volley damage for the ship")> <Category("Volley Damage")> Public Property DroneVolley() As Double
        Get
            Return _cDroneVolley
        End Get
        Set(ByVal value As Double)
            _cDroneVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total volley damage for the ship")> <Category("Volley Damage")> Public Property TotalVolley() As Double
        Get
            Return _cTotalVolley
        End Get
        Set(ByVal value As Double)
            _cTotalVolley = value
        End Set
    End Property

#End Region

#Region "DPS Properties"

    <Browsable(False)> _
    <Description("The combined turret DPS for the ship")> <Category("DPS")> Public Property TurretDPS() As Double
        Get
            Return _cTurretDPS
        End Get
        Set(ByVal value As Double)
            _cTurretDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined missile DPS for the ship")> <Category("DPS")> Public Property MissileDPS() As Double
        Get
            Return _cMissileDPS
        End Get
        Set(ByVal value As Double)
            _cMissileDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined smartbomb DPS for the ship")> <Category("DPS")> Public Property SmartbombDPS() As Double
        Get
            Return _cSmartbombDPS
        End Get
        Set(ByVal value As Double)
            _cSmartbombDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined bomb DPS for the ship")> <Category("DPS")> Public Property BombDPS() As Double
        Get
            Return _cBombDPS
        End Get
        Set(ByVal value As Double)
            _cBombDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone DPS for the ship")> <Category("DPS")> Public Property DroneDPS() As Double
        Get
            Return _cDroneDPS
        End Get
        Set(ByVal value As Double)
            _cDroneDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total DPS for the ship")> <Category("DPS")> Public Property TotalDPS() As Double
        Get
            Return _cTotalDPS
        End Get
        Set(ByVal value As Double)
            _cTotalDPS = value
        End Set
    End Property

#End Region

#Region "Ore Mining Properties"

    <Browsable(False)> _
    <Description("The combined turret ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreTurretAmount() As Double
        Get
            Return _cOreTurretAmount
        End Get
        Set(ByVal value As Double)
            _cOreTurretAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreDroneAmount() As Double
        Get
            Return _cOreDroneAmount
        End Get
        Set(ByVal value As Double)
            _cOreDroneAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreTotalAmount() As Double
        Get
            Return _cOreTotalAmount
        End Get
        Set(ByVal value As Double)
            _cOreTotalAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined turret ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreTurretRate() As Double
        Get
            Return _cOreTurretRate
        End Get
        Set(ByVal value As Double)
            _cOreTurretRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreDroneRate() As Double
        Get
            Return _cOreDroneRate
        End Get
        Set(ByVal value As Double)
            _cOreDroneRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreTotalRate() As Double
        Get
            Return _cOreTotalRate
        End Get
        Set(ByVal value As Double)
            _cOreTotalRate = value
        End Set
    End Property

#End Region

#Region "Ice Mining Properties"

    <Browsable(False)> _
    <Description("The combined turret ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceTurretAmount() As Double
        Get
            Return _cIceTurretAmount
        End Get
        Set(ByVal value As Double)
            _cIceTurretAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceDroneAmount() As Double
        Get
            Return _cIceDroneAmount
        End Get
        Set(ByVal value As Double)
            _cIceDroneAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceTotalAmount() As Double
        Get
            Return _cIceTotalAmount
        End Get
        Set(ByVal value As Double)
            _cIceTotalAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined turret ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceTurretRate() As Double
        Get
            Return _cIceTurretRate
        End Get
        Set(ByVal value As Double)
            _cIceTurretRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceDroneRate() As Double
        Get
            Return _cIceDroneRate
        End Get
        Set(ByVal value As Double)
            _cIceDroneRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceTotalRate() As Double
        Get
            Return _cIceTotalRate
        End Get
        Set(ByVal value As Double)
            _cIceTotalRate = value
        End Set
    End Property

#End Region

#Region "Gas Mining Properties"

    <Browsable(False)> _
    <Description("The total gas mining amount for the ship")> <Category("Gas Mining")> Public Property GasTotalAmount() As Double

    <Browsable(False)> _
    <Description("The total gas mining rate for the ship")> <Category("Gas Mining")> Public Property GasTotalRate() As Double

#End Region

#Region "Logistics Properties"

    <Browsable(False)> _
    <Description("The combined logistics module transfer amount for the ship")> <Category("Logistics")> Public Property ModuleTransferAmount() As Double

    <Browsable(False)> _
    <Description("The combined logistics module transfer rate for the ship")> <Category("Logistics")> Public Property ModuleTransferRate() As Double

    <Browsable(False)> _
    <Description("The combined logistics drone transfer amount for the ship")> <Category("Logistics")> Public Property DroneTransferAmount() As Double

    <Browsable(False)> _
    <Description("The combined logistics drone transfer rate for the ship")> <Category("Logistics")> Public Property DroneTransferRate() As Double

    <Browsable(False)> _
    <Description("The total logistics transfer amount for the ship")> <Category("Logistics")> Public Property TransferAmount() As Double

    <Browsable(False)> _
    <Description("The total logistics transfer rate for the ship")> <Category("Logistics")> Public Property TransferRate() As Double

#End Region

#Region "Audit Log Properties"

    <Browsable(False)> _
    <Description("The list of audit log entries for the fitted ship")> <Category("Audit Log")> Public Property AuditLog() As List(Of String)
        Get
            Return _cAuditLog
        End Get
        Set(ByVal value As List(Of String))
            _cAuditLog = value
        End Set
    End Property

#End Region

#Region "Damage Profile Properties"

    <ProtoMember(102)> <Browsable(False)> _
    <Description("The damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfile() As HQFDamageProfile
        Get
            Return _cDamageProfile
        End Get
        Set(ByVal value As HQFDamageProfile)
            If value Is Nothing Then
                value = HQFDamageProfiles.ProfileList.Item("<Omni-Damage>")
            End If
            _cDamageProfile = value
            _cEMExKiTh = _cDamageProfile.EM + _cDamageProfile.Explosive + _cDamageProfile.Kinetic + _cDamageProfile.Thermal
            _cEM = _cDamageProfile.EM / _cEMExKiTh
            _cEx = _cDamageProfile.Explosive / _cEMExKiTh
            _cKi = _cDamageProfile.Kinetic / _cEMExKiTh
            _cTh = _cDamageProfile.Thermal / _cEMExKiTh
        End Set
    End Property

    <ProtoMember(103)> <Browsable(False)> _
    <Description("The EM element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileEM() As Double
        Get
            Return _cEM
        End Get
        Set(ByVal value As Double)
            _cEM = value
        End Set
    End Property

    <ProtoMember(104)> <Browsable(False)> _
    <Description("The Explosive element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileEx() As Double
        Get
            Return _cEx
        End Get
        Set(ByVal value As Double)
            _cEx = value
        End Set
    End Property

    <ProtoMember(105)> <Browsable(False)> _
    <Description("The Kinetic element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileKi() As Double
        Get
            Return _cKi
        End Get
        Set(ByVal value As Double)
            _cKi = value
        End Set
    End Property

    <ProtoMember(106)> <Browsable(False)> _
    <Description("The Thermal element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileTh() As Double
        Get
            Return _cTh
        End Get
        Set(ByVal value As Double)
            _cTh = value
        End Set
    End Property

#End Region

#Region "Affects"

    <ProtoMember(107)> <Browsable(False)> _
    <Description("The items which are affected by this ship")> <Category("Affects")> Public Property Affects() As List(Of String)
        Get
            Return _cAffects
        End Get
        Set(ByVal value As List(Of String))
            _cAffects = value
        End Set
    End Property

    <ProtoMember(108)> <Browsable(False)> _
    <Description("The items which are globally affected by this ship")> <Category("Affects")> Public Property GlobalAffects() As List(Of String)
        Get
            Return _cGlobalAffects
        End Get
        Set(ByVal value As List(Of String))
            _cGlobalAffects = value
        End Set
    End Property

#End Region

#End Region

    ' ReSharper restore InconsistentNaming

#Region "Cloning"
    Public Function Clone() As Ship
        Using shipMemoryStream As New MemoryStream
            Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
            objBinaryFormatter.Serialize(shipMemoryStream, Me)
            shipMemoryStream.Seek(0, SeekOrigin.Begin)
            Return CType(objBinaryFormatter.Deserialize(shipMemoryStream), Ship)
        End Using
    End Function
#End Region

#Region "Effective HP Calculations"

    Private Sub CalculateEffectiveShieldHP()
        _cEffectiveShieldHp = _cShieldCapacity * 100 / (_cEM * (100 - _cShieldEMResist) + _cEx * (100 - _cShieldExResist) + _cKi * (100 - _cShieldKiResist) + _cTh * (100 - _cShieldThResist))
        Dim lowResist As Double = Math.Min(Math.Min(Math.Min(_cShieldEMResist, _cShieldExResist), _cShieldKiResist), _cShieldThResist)
        _cEveEffectiveShieldHp = _cShieldCapacity * 100 / (_cEM * (100 - lowResist) + _cEx * (100 - lowResist) + _cKi * (100 - lowResist) + _cTh * (100 - lowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveArmorHP()
        _cEffectiveArmorHp = _cArmorCapacity * 100 / (_cEM * (100 - _cArmorEMResist) + _cEx * (100 - _cArmorExResist) + _cKi * (100 - _cArmorKiResist) + _cTh * (100 - _cArmorThResist))
        Dim lowResist As Double = Math.Min(Math.Min(Math.Min(_cArmorEMResist, _cArmorExResist), _cArmorKiResist), _cArmorThResist)
        _cEveEffectiveArmorHp = _cArmorCapacity * 100 / (_cEM * (100 - lowResist) + _cEx * (100 - lowResist) + _cKi * (100 - lowResist) + _cTh * (100 - lowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveStructureHP()
        _cEffectiveStructureHp = _cStructureCapacity * 100 / (_cEM * (100 - _cStructureEMResist) + _cEx * (100 - _cStructureExResist) + _cKi * (100 - _cStructureKiResist) + _cTh * (100 - _cStructureThResist))
        Dim lowResist As Double = Math.Min(Math.Min(Math.Min(_cStructureEMResist, _cStructureExResist), _cStructureKiResist), _cStructureThResist)
        _cEveEffectiveStructureHp = _cStructureCapacity * 100 / (_cEM * (100 - lowResist) + _cEx * (100 - lowResist) + _cKi * (100 - lowResist) + _cTh * (100 - lowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveHP()
        _cEffectiveHp = _cEffectiveShieldHp + _cEffectiveArmorHp + _cEffectiveStructureHp
        _cEveEffectiveHp = Int(_cEveEffectiveShieldHp + _cEveEffectiveArmorHp + _cEveEffectiveStructureHp)
    End Sub
    Public Sub RecalculateEffectiveHP()
        Dim lowResist As Double
        ' Calculate Shield EHP
        _cEffectiveShieldHp = _cShieldCapacity * 100 / (_cEM * (100 - _cShieldEMResist) + _cEx * (100 - _cShieldExResist) + _cKi * (100 - _cShieldKiResist) + _cTh * (100 - _cShieldThResist))
        lowResist = Math.Min(Math.Min(Math.Min(_cShieldEMResist, _cShieldExResist), _cShieldKiResist), _cShieldThResist)
        _cEveEffectiveShieldHp = _cShieldCapacity * 100 / (_cEM * (100 - lowResist) + _cEx * (100 - lowResist) + _cKi * (100 - lowResist) + _cTh * (100 - lowResist))
        ' Calculate Armor EHP
        _cEffectiveArmorHp = _cArmorCapacity * 100 / (_cEM * (100 - _cArmorEMResist) + _cEx * (100 - _cArmorExResist) + _cKi * (100 - _cArmorKiResist) + _cTh * (100 - _cArmorThResist))
        lowResist = Math.Min(Math.Min(Math.Min(_cArmorEMResist, _cArmorExResist), _cArmorKiResist), _cArmorThResist)
        _cEveEffectiveArmorHp = _cArmorCapacity * 100 / (_cEM * (100 - lowResist) + _cEx * (100 - lowResist) + _cKi * (100 - lowResist) + _cTh * (100 - lowResist))
        ' Calculate Structure EHP
        _cEffectiveStructureHp = _cStructureCapacity * 100 / (_cEM * (100 - _cStructureEMResist) + _cEx * (100 - _cStructureExResist) + _cKi * (100 - _cStructureKiResist) + _cTh * (100 - _cStructureThResist))
        lowResist = Math.Min(Math.Min(Math.Min(_cStructureEMResist, _cStructureExResist), _cStructureKiResist), _cStructureThResist)
        _cEveEffectiveStructureHp = _cStructureCapacity * 100 / (_cEM * (100 - lowResist) + _cEx * (100 - lowResist) + _cKi * (100 - lowResist) + _cTh * (100 - lowResist))
        ' Calculate Total EHP
        _cEffectiveHp = _cEffectiveShieldHp + _cEffectiveArmorHp + _cEffectiveStructureHp
        _cEveEffectiveHp = Int(_cEveEffectiveShieldHp + _cEveEffectiveArmorHp + _cEveEffectiveStructureHp)
    End Sub

#End Region

#Region "Add Custom Attributes"
    Public Sub AddCustomShipAttributes()
        ' Add the custom attributes to the list
        Attributes.Add(10002, Mass)
        Attributes.Add(10003, Volume)
        Attributes.Add(10004, CargoBay)
        Attributes.Add(10005, 0)
        Attributes.Add(10006, 0)
        Attributes.Add(10007, 20000)
        Attributes.Add(10008, 20000)
        Attributes.Add(10009, 1)
        Attributes.Add(10010, 0)
        Attributes.Add(10020, 0)
        Attributes.Add(10021, 0)
        Attributes.Add(10022, 0)
        Attributes.Add(10023, 0)
        Attributes.Add(10024, 0)
        Attributes.Add(10025, 0)
        Attributes.Add(10026, 0)
        Attributes.Add(10027, 0)
        Attributes.Add(10028, 0)
        Attributes.Add(10029, 0)
        Attributes.Add(10031, 0)
        Attributes.Add(10033, 0)
        Attributes.Add(10034, 0)
        Attributes.Add(10035, 0)
        Attributes.Add(10036, 0)
        Attributes.Add(10037, 0)
        Attributes.Add(10038, 0)
        Attributes.Add(10043, 0)
        Attributes.Add(10044, 0)
        Attributes.Add(10045, 0)
        Attributes.Add(10046, 0)
        Attributes.Add(10047, 0)
        Attributes.Add(10048, 0)
        Attributes.Add(10049, 0)
        Attributes.Add(10050, 0)
        Attributes.Add(10055, 0)
        Attributes.Add(10056, 0)
        Attributes.Add(10057, 0)
        Attributes.Add(10058, 0)
        Attributes.Add(10059, 0)
        Attributes.Add(10060, 0)
        Attributes.Add(10061, 0)
        Attributes.Add(10062, 0)
        Attributes.Add(10063, 1)
        Attributes.Add(10064, 2)
        Attributes.Add(10065, 0)
        Attributes.Add(10066, 0)
        Attributes.Add(10067, 0)
        Attributes.Add(10068, 0)
        Attributes.Add(10069, 0)
        Attributes.Add(10070, 0)
        Attributes.Add(10071, 0)
        Attributes.Add(10072, 0)
        Attributes.Add(10073, 0)
        Attributes.Add(10075, 0)
        Attributes.Add(10076, 0)
        Attributes.Add(10077, 0)
        Attributes.Add(10078, 0)
        Attributes.Add(10079, 0)
        Attributes.Add(10080, 0)
        Attributes.Add(10081, 0)
        Attributes.Add(10083, 0)
        ' Add unused attribute for calibration used
        Attributes.Add(1152, 0)
        ' Check for slot attributes (missing for T3)
        If Attributes.ContainsKey(12) = False Then
            Attributes.Add(12, 0)
            Attributes.Add(13, 0)
            Attributes.Add(14, 0)
        End If
        ' Check for cloak reactivation attribute
        If Attributes.ContainsKey(1034) = False Then
            Attributes.Add(1034, 30)
        End If

    End Sub
#End Region

#Region "Map Attributes to Properties"
    Public Shared Sub MapShipAttributes(ByVal newShip As Ship)
        Dim attValue As Double
        For Each att As Integer In newShip.Attributes.Keys
            attValue = CDbl(newShip.Attributes(att))
            Select Case CInt(CInt(att))
                Case 12
                    newShip.LowSlots = CInt(attValue)
                Case 13
                    newShip.MidSlots = CInt(attValue)
                Case 14
                    If newShip.HiSlots < 9 Then ' Condition for testing if we are using ammo analysis
                        newShip.HiSlots = CInt(attValue)
                    End If
                Case 1137
                    newShip.RigSlots = CInt(attValue)
                Case 1367
                    newShip.SubSlots = CInt(attValue)
                Case 15
                    newShip.PGUsed = attValue
                Case 1132
                    newShip.Calibration = CInt(attValue)
                Case 1152
                    newShip.CalibrationUsed = CInt(attValue)
                Case 11
                    newShip.PG = attValue
                Case 48
                    newShip.CPU = attValue
                Case 49
                    newShip.CPUUsed = attValue
                Case 101
                    newShip.LauncherSlots = CInt(attValue)
                Case 102
                    newShip.TurretSlots = CInt(attValue)
                Case 55
                    newShip.CapRecharge = attValue
                Case 482
                    newShip.CapCapacity = attValue
                Case 9
                    newShip.StructureCapacity = attValue
                Case 113
                    newShip.StructureEMResist = attValue
                Case 111
                    newShip.StructureExResist = attValue
                Case 109
                    newShip.StructureKiResist = attValue
                Case 110
                    newShip.StructureThResist = attValue
                Case 265
                    newShip.ArmorCapacity = attValue
                Case 267
                    newShip.ArmorEMResist = attValue
                Case 268
                    newShip.ArmorExResist = attValue
                Case 269
                    newShip.ArmorKiResist = attValue
                Case 270
                    newShip.ArmorThResist = attValue
                Case 263
                    newShip.ShieldCapacity = attValue
                Case 479
                    newShip.ShieldRecharge = attValue
                Case 271
                    newShip.ShieldEMResist = attValue
                Case 272
                    newShip.ShieldExResist = attValue
                Case 273
                    newShip.ShieldKiResist = attValue
                Case 274
                    newShip.ShieldThResist = attValue
                Case 76
                    newShip.MaxTargetRange = attValue
                Case 79
                    newShip.TargetingSpeed = attValue
                Case 192
                    newShip.MaxLockedTargets = attValue
                Case 552
                    newShip.SigRadius = attValue
                Case 564
                    newShip.ScanResolution = attValue
                Case 211
                    newShip.GravSensorStrenth = attValue
                Case 209
                    newShip.LadarSensorStrenth = attValue
                Case 210
                    newShip.MagSensorStrenth = attValue
                Case 208
                    newShip.RadarSensorStrenth = attValue
                Case 37
                    newShip.MaxVelocity = attValue
                Case 819
                    newShip.FusionPropStrength = attValue
                Case 820
                    newShip.IonPropStrength = attValue
                Case 821
                    newShip.MagpulsePropStrength = attValue
                Case 822
                    newShip.PlasmaPropStrength = attValue
                Case 70
                    newShip.Inertia = attValue
                Case 153
                    newShip.WarpCapNeed = attValue
                Case 1281
                    If newShip.Attributes.ContainsKey(600) = False Then
                        newShip.WarpSpeed = attValue
                    Else
                        newShip.WarpSpeed = attValue * CDbl(newShip.Attributes(600))
                    End If
                Case 283
                    newShip.DroneBay = attValue
                Case 908
                    newShip.ShipBay = attValue
                Case 1271
                    newShip.DroneBandwidth = attValue
                Case 10002
                    newShip.Mass = attValue
                Case 10004
                    newShip.CargoBay = CInt(attValue)
                Case 10005
                    newShip.MaxDrones = CInt(attValue)
                Case 10006
                    newShip.UsedDrones = CInt(attValue)
                Case 10020
                    newShip.TurretVolley = attValue
                Case 10021
                    newShip.MissileVolley = attValue
                Case 10022
                    newShip.SmartbombVolley = attValue
                Case 10023
                    newShip.DroneVolley = attValue
                Case 10024
                    newShip.TurretDPS = attValue
                Case 10025
                    newShip.MissileDPS = attValue
                Case 10026
                    newShip.SmartbombDPS = attValue
                Case 10027
                    newShip.DroneDPS = attValue
                Case 10028
                    newShip.TotalVolley = attValue
                Case 10029
                    newShip.TotalDPS = attValue
                Case 10033
                    newShip.OreTotalAmount = attValue
                Case 10034
                    newShip.OreTurretAmount = attValue
                Case 10035
                    newShip.OreDroneAmount = attValue
                Case 10036
                    newShip.IceTotalAmount = attValue
                Case 10037
                    newShip.IceTurretAmount = attValue
                Case 10038
                    newShip.IceDroneAmount = attValue
                Case 10043
                    newShip.OreTurretRate = attValue
                Case 10044
                    newShip.OreDroneRate = attValue
                Case 10045
                    newShip.IceTurretRate = attValue
                Case 10046
                    newShip.IceDroneRate = attValue
                Case 10047
                    newShip.OreTotalRate = attValue
                Case 10048
                    newShip.IceTotalRate = attValue
                Case 10075
                    newShip.ModuleTransferAmount = attValue
                Case 10076
                    newShip.DroneTransferAmount = attValue
                Case 10077
                    newShip.ModuleTransferRate = attValue
                Case 10078
                    newShip.DroneTransferRate = attValue
                Case 10079
                    newShip.TransferAmount = attValue
                Case 10080
                    newShip.TransferRate = attValue
                Case 10081
                    newShip.GasTotalAmount = attValue
                Case 10083
                    newShip.GasTotalRate = attValue
            End Select
        Next
    End Sub
#End Region

#Region "Map Properties to Attributes"
    Public Sub MapShipProperties()
        Attributes(12) = LowSlots
        Attributes(13) = MidSlots
        Attributes(14) = HiSlots
        Attributes(1137) = RigSlots
        Attributes(1367) = SubSlots
        Attributes(15) = PGUsed
        Attributes(1132) = Calibration
        Attributes(1152) = CalibrationUsed
        Attributes(11) = PG
        Attributes(48) = CPU
        Attributes(49) = CPUUsed
        Attributes(101) = LauncherSlots
        Attributes(102) = TurretSlots
        Attributes(55) = CapRecharge
        Attributes(482) = CapCapacity
        Attributes(9) = StructureCapacity
        Attributes(113) = StructureEMResist
        Attributes(111) = StructureExResist
        Attributes(109) = StructureKiResist
        Attributes(110) = StructureThResist
        Attributes(265) = ArmorCapacity
        Attributes(267) = ArmorEMResist
        Attributes(268) = ArmorExResist
        Attributes(269) = ArmorKiResist
        Attributes(270) = ArmorThResist
        Attributes(263) = ShieldCapacity
        Attributes(479) = ShieldRecharge
        Attributes(271) = ShieldEMResist
        Attributes(272) = ShieldExResist
        Attributes(273) = ShieldKiResist
        Attributes(274) = ShieldThResist
        Attributes(76) = MaxTargetRange
        Attributes(79) = TargetingSpeed
        Attributes(192) = MaxLockedTargets
        Attributes(552) = SigRadius
        Attributes(564) = ScanResolution
        Attributes(211) = GravSensorStrenth
        Attributes(209) = LadarSensorStrenth
        Attributes(210) = MagSensorStrenth
        Attributes(208) = RadarSensorStrenth
        Attributes(37) = MaxVelocity
        Attributes(70) = Inertia
        Attributes(153) = WarpCapNeed
        If Attributes.ContainsKey(600) = False Then
            Attributes(1281) = WarpSpeed
        Else
            Attributes(1281) = WarpSpeed * CDbl(Attributes(600))
        End If
        Attributes(283) = DroneBay
        Attributes(908) = ShipBay
        Attributes(1271) = DroneBandwidth
        Attributes(10002) = Mass
        Attributes(10004) = CargoBay
        Attributes(10005) = MaxDrones
        Attributes(10006) = UsedDrones
        Attributes(10020) = TurretVolley
        Attributes(10021) = MissileVolley
        Attributes(10022) = SmartbombVolley
        Attributes(10023) = DroneVolley
        Attributes(10024) = TurretDPS
        Attributes(10025) = MissileDPS
        Attributes(10026) = SmartbombDPS
        Attributes(10027) = DroneDPS
        Attributes(10028) = TotalVolley
        Attributes(10029) = TotalDPS
        Attributes(10033) = OreTotalAmount
        Attributes(10034) = OreTurretAmount
        Attributes(10035) = OreDroneAmount
        Attributes(10036) = IceTotalAmount
        Attributes(10037) = IceTurretAmount
        Attributes(10038) = IceDroneAmount
        Attributes(10043) = OreTurretRate
        Attributes(10044) = OreDroneRate
        Attributes(10045) = IceTurretRate
        Attributes(10046) = IceDroneRate
        Attributes(10047) = OreTotalRate
        Attributes(10048) = IceTotalRate
        Attributes(10075) = ModuleTransferAmount
        Attributes(10076) = DroneTransferAmount
        Attributes(10077) = ModuleTransferRate
        Attributes(10078) = DroneTransferRate
        Attributes(10079) = TransferAmount
        Attributes(10080) = TransferRate
        Attributes(10081) = GasTotalAmount
        Attributes(10083) = GasTotalRate
    End Sub
#End Region

End Class

<ProtoContract()> Public Enum SlotTypes As Integer
    <ProtoMember(1)> Rig = 1
    <ProtoMember(2)> Low = 2
    <ProtoMember(3)> Mid = 4
    <ProtoMember(4)> High = 8
    <ProtoMember(5)> Subsystem = 16
End Enum

<ProtoContract()> <Serializable()> Public Class ShipLists

    <ProtoMember(1)> Public Shared ShipListKeyName As New SortedList(Of String, Integer) ' Ship.Name, Ship.Id
    <ProtoMember(2)> Public Shared ShipListKeyID As New SortedList(Of Integer, String) ' Ship.Id, Ship.Name
    <ProtoMember(3)> Public Shared ShipList As New Dictionary(Of String, Ship)   ' Key = ship name
    <ProtoMember(4)> Public Shared FittedShipList As New SortedList(Of String, Ship)   ' Key = fitting key

End Class

<ProtoContract()> <Serializable()> Public Class DroneBayItem
    <ProtoMember(1)> Public DroneType As ShipModule
    <ProtoMember(2)> Public Quantity As Integer
    <ProtoMember(3)> Public IsActive As Boolean
End Class

<ProtoContract()> <Serializable()> Public Class CargoBayItem
    <ProtoMember(1)> Public ItemType As ShipModule
    <ProtoMember(2)> Public Quantity As Integer
End Class

<ProtoContract()> <Serializable()> Public Class ShipBayItem
    <ProtoMember(1)> Public ShipType As Ship
    <ProtoMember(2)> Public Quantity As Integer
End Class
