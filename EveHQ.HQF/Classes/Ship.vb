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
Imports System.ComponentModel

''' <summary>
''' Public class for storing details of a ship used for processing
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Ship
    
#Region "Constants"
    Const MaxBasicSlots As Integer = 8
    Const MaxRigSlots As Integer = 3
    Const MaxSubSlots As Integer = 5

#End Region

#Region "Property Variables"

  ' Max Fitting Layout
    Private _hiSlots As Integer
    Private _midSlots As Integer
    Private _lowSlots As Integer
    Private _rigSlots As Integer
    Private _subSlots As Integer
    Private _turretSlots As Integer
    Private _launcherSlots As Integer
    Private _calibration As Integer

    ' CPU, Power & Capacitor
    Private _cpu As Double
    Private _pg As Double
    Private _capCapacity As Double
    Private _capRecharge As Double

    ' Shield
    Private _shieldCapacity As Double
    Private _shieldRecharge As Double
    Private _shieldEMResist As Double
    Private _shieldExResist As Double
    Private _shieldKiResist As Double
    Private _shieldThResist As Double

    ' Armor
    Private _armorCapacity As Double
    Private _armorEMResist As Double
    Private _armorExResist As Double
    Private _armorKiResist As Double
    Private _armorThResist As Double

    ' Structure
    Private _structureCapacity As Double
    Private _structureEMResist As Double
    Private _structureExResist As Double
    Private _structureKiResist As Double
    Private _structureThResist As Double

    ' Space & Volume
    Private _cargoBay As Double
    Private _mass As Double
    Private _volume As Double

    ' Drones
    Private _droneBay As Double
    Private _droneBandwidth As Double
    Private _usedDrones As Integer
    Private _maxDrones As Integer

    ' Ship Bay
    Private _shipBay As Double

    ' Targeting
    Private _maxLockedTargets As Double
    Private _maxTargetRange As Double
    Private _targetingSpeed As Double
    Private _scanResolution As Double
    Private _sigRadius As Double
    Private _gravSensorStrenth As Double
    Private _ladarSensorStrenth As Double
    Private _magSensorStrenth As Double
    Private _radarSensorStrenth As Double

    ' Propulsion
    Private _maxVelocity As Double
    Private _inertia As Double
    Private _fusionPropStrength As Double
    Private _ionPropStrength As Double
    Private _magpulsePropStrength As Double
    Private _plasmaPropStrength As Double
    Private _warpSpeed As Double
    Private _warpCapNeed As Double

    ' Module Slots
    Private _hiSlot(8) As ShipModule
    Private _midSlot(8) As ShipModule
    Private _lowSlot(8) As ShipModule
    Private _rigSlot(8) As ShipModule
    Private _subSlot(5) As ShipModule

    ' Effective Resists
    Private _effectiveShieldHP As Double
    Private _effectiveArmorHP As Double
    Private _effectiveStructureHP As Double
    Private _effectiveHP As Double
    Private _eveEffectiveShieldHP As Double
    Private _eveEffectiveArmorHP As Double
    Private _eveEffectiveStructureHP As Double
    Private _eveEffectiveHP As Double

    ' Damage Profile
    Private _damageProfile As HQFDamageProfile
    Private _em As Double
    Private _ex As Double
    Private _ki As Double
    Private _th As Double
    Private _emExKiTh As Double

#End Region

#Region "Properties"

#Region "Database Properties"

    ' Note: These properties (except the RaceID) should not be directly editable in the property editor, but still visible

    <[ReadOnly](True)> _
    <Description("The name of the ship")> <Category("Database")> Public Property Name() As String

    <[ReadOnly](True)> _
    <Description("The ID of the ship")> <Category("Database")> Public Property ID() As String

    <[ReadOnly](True)> _
    <Description("The market group ID of the ship")> <Category("Database")> Public Property MarketGroup() As String

    <[ReadOnly](True)> _
    <Description("The database group ID of the ship")> <Category("Database")> Public Property DatabaseGroup() As String

    <[ReadOnly](True)> _
    <Description("The database category ID of the ship")> <Category("Database")> Public Property DatabaseCategory() As String

    <[ReadOnly](True)> _
    <Description("The description of the ship")> <Category("Database")> Public Property Description() As String

    <Description("The raceID of the ship")> <Category("Database")> Public Property RaceID() As Integer

    <[ReadOnly](True)> _
    <Description("The icon ID of the ship")> <Category("Database")> Public Property Icon() As String

#End Region

#Region "Price Properties"

    ' Nore: These properties should not be visible in the property editor as they are irrelevant

    <Browsable(False)> _
    <Description("The base price of the ship")> <Category("Price")> Public Property BasePrice() As Double

    <Browsable(False)> _
    <Description("The market price of the ship")> <Category("Price")> Public Property MarketPrice() As Double

    <Browsable(False)> _
    <Description("The base price of the ship including all fittings")> <Category("Price")> Public Property FittingBasePrice() As Double

    <Browsable(False)> _
    <Description("The market price of the ship including all fittings")> <Category("Price")> Public Property FittingMarketPrice() As Double

#End Region

#Region "Fitting Properties"

    <Description("The number of available high slots on the ship")> <Category("Fitting")> Public Property HiSlots() As Integer
        Get
            Return _hiSlots
        End Get
        Set(ByVal value As Integer)
            If OverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("High slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _hiSlots = value
                End If
            Else
                _hiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available mid slots on the ship")> <Category("Fitting")> Public Property MidSlots() As Integer
        Get
            Return _midSlots
        End Get
        Set(ByVal value As Integer)
            If OverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Mid slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _midSlots = value
                End If
            Else
                _hiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available low slots on the ship")> <Category("Fitting")> Public Property LowSlots() As Integer
        Get
            Return _lowSlots
        End Get
        Set(ByVal value As Integer)
            If OverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Low slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _lowSlots = value
                End If
            Else
                _hiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available rig slots on the ship")> <Category("Fitting")> Public Property RigSlots() As Integer
        Get
            Return _rigSlots
        End Get
        Set(ByVal value As Integer)
            If OverrideFittingRules = False Then
                If value < 0 Or value > MaxRigSlots Then
                    MessageBox.Show("Rig slots must be between 0 and " & MaxRigSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _rigSlots = value
                End If
            Else
                _hiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available subsystem slots on the ship")> <Category("Fitting")> Public Property SubSlots() As Integer
        Get
            Return _subSlots
        End Get
        Set(ByVal value As Integer)
            If OverrideFittingRules = False Then
                If value <> MaxSubSlots And value <> 0 Then
                    MessageBox.Show("The number of subsystem slots is currently restricted to " & MaxSubSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _subSlots = value
                End If
            Else
                _hiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available turret slots on the ship")> <Category("Fitting")> Public Property TurretSlots() As Integer
        Get
            Return _turretSlots
        End Get
        Set(ByVal value As Integer)
            If OverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Turret slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _turretSlots = value
                End If
            Else
                _hiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available launcher slots on the ship")> <Category("Fitting")> Public Property LauncherSlots() As Integer
        Get
            Return _launcherSlots
        End Get
        Set(ByVal value As Integer)
            If OverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Launcher slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    _launcherSlots = value
                End If
            Else
                _hiSlots = value
            End If
        End Set
    End Property

    <Description("The maximum CPU available for all modules on the ship")> <Category("Fitting")> Public Property Cpu() As Double
        Get
            Return _cpu
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("CPU must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cpu = value
            End If
        End Set
    End Property

    <Description("The maximum powergrid available for all modules on the ship")> <Category("Fitting")> Public Property Pg() As Double
        Get
            Return _pg
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Powergrid must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _pg = value
            End If
        End Set
    End Property

    <Description("The maximum available calibration units available for rigs")> <Category("Fitting")> Public Property Calibration() As Integer
        Get
            Return _calibration
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Calibration must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _calibration = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("Allows fitting rules to be relaxed to increase fitting limits")> <Category("Fitting")> Public Property OverrideFittingRules() As Boolean

#End Region

#Region "Slot Collection Properties"

    ' Note: None of these properties should be visible in the property editor

    <Browsable(False)> _
    <Description("The collection of internal modules for the ship")> <Category("Slot Collection")> Public Property SlotCollection() As ArrayList

    <Browsable(False)> _
    <Description("The collection of remote modules for the ship")> <Category("Slot Collection")> Public Property RemoteSlotCollection() As ArrayList

    <Browsable(False)> _
    <Description("The collection of fleet-based modules for the ship")> <Category("Slot Collection")> Public Property FleetSlotCollection() As ArrayList

    <Browsable(False)> _
    <Description("The collection of external environment modules for the ship")> <Category("Slot Collection")> Public Property EnviroSlotCollection() As ArrayList

    <Browsable(False)> _
    <Description("The collection of combat boosters for the ship")> <Category("Slot Collection")> Public Property BoosterSlotCollection() As ArrayList

#End Region

#Region "Capacitor Properties"

    <Description("The total available capacitor of the ship")> <Category("Capacitor")> Public Property CapCapacity() As Double
        Get
            Return _capCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Capacitor capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _capCapacity = value
            End If
        End Set
    End Property

    <Description("The capacitor recharge time of the ship")> <Category("Capacitor")> Public Property CapRecharge() As Double
        Get
            Return _capRecharge
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Capacitor recharge time must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _capRecharge = value
            End If
        End Set
    End Property

#End Region

#Region "Shield Properties"

    <Description("The shield hitpoint capacity of the ship")> <Category("Shield")> Public Property ShieldCapacity() As Double
        Get
            Return _shieldCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Shield capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _shieldCapacity = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield recharge time of the ship")> <Category("Shield")> Public Property ShieldRecharge() As Double
        Get
            Return _shieldRecharge
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Shield recharge time must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _shieldRecharge = value
            End If
        End Set
    End Property

    <Description("The shield EM resistance of the ship")> <Category("Shield")> Public Property ShieldEMResist() As Double
        Get
            Return _shieldEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _shieldEMResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield Explosive resistance of the ship")> <Category("Shield")> Public Property ShieldExResist() As Double
        Get
            Return _shieldExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _shieldExResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield Kinetic resistance of the ship")> <Category("Shield")> Public Property ShieldKiResist() As Double
        Get
            Return _shieldKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _shieldKiResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield Thermal resistance of the ship")> <Category("Shield")> Public Property ShieldThResist() As Double
        Get
            Return _shieldThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _shieldThResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

#End Region

#Region "Armor Properties"

    <Description("The armor hitpoint capacity of the ship")> <Category("Armor")> Public Property ArmorCapacity() As Double
        Get
            Return _armorCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Armor capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _armorCapacity = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor EM resistance of the ship")> <Category("Armor")> Public Property ArmorEMResist() As Double
        Get
            Return _armorEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _armorEMResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor Explosive resistance of the ship")> <Category("Armor")> Public Property ArmorExResist() As Double
        Get
            Return _armorExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _armorExResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor Kinetic resistance of the ship")> <Category("Armor")> Public Property ArmorKiResist() As Double
        Get
            Return _armorKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _armorKiResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor Thermal resistance of the ship")> <Category("Armor")> Public Property ArmorThResist() As Double
        Get
            Return _armorThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _armorThResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property

#End Region

#Region "Hull Properties"

    <Description("The hull hitpoint capacity of the ship")> <Category("Hull")> Public Property StructureCapacity() As Double
        Get
            Return _structureCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Structure capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _structureCapacity = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull EM resistance of the ship")> <Category("Hull")> Public Property StructureEMResist() As Double
        Get
            Return _structureEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _structureEMResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull Explosive resistance of the ship")> <Category("Hull")> Public Property StructureExResist() As Double
        Get
            Return _structureExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _structureExResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull Kinetic resistance of the ship")> <Category("Hull")> Public Property StructureKiResist() As Double
        Get
            Return _structureKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _structureKiResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull Thermal resistance of the ship")> <Category("Hull")> Public Property StructureThResist() As Double
        Get
            Return _structureThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _structureThResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

#End Region

#Region "Structure Properties"

    <Description("The mass of the ship")> <Category("Structure")> Public Property Mass() As Double
        Get
            Return _mass
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Mass must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _mass = value
            End If
        End Set
    End Property

    <Description("The unpacked volume of the ship")> <Category("Structure")> Public Property Volume() As Double
        Get
            Return _volume
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Volume must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _volume = value
            End If
        End Set
    End Property

#End Region

#Region "Storage Bay Properties"

    <Description("The cargo bay capacity of the ship")> <Category("Storage")> Public Property CargoBay() As Double
        Get
            Return _cargoBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Cargo Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _cargoBay = value
            End If
        End Set
    End Property

    <Description("The ship maintenance bay capacity of the ship")> <Category("Storage")> Public Property ShipBay() As Double
        Get
            Return _shipBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Ship Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _shipBay = value
            End If
        End Set
    End Property

#End Region

#Region "Drone Properties"

    <Description("The drone bay capacity of the ship")> <Category("Drones")> Public Property DroneBay() As Double
        Get
            Return _droneBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Drone Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _droneBay = value
            End If
        End Set
    End Property

    <Description("The maxmium drone bandwidth capability of the ship")> <Category("Drones")> Public Property DroneBandwidth() As Double
        Get
            Return _droneBandwidth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Drone bandwidth must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _droneBandwidth = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of drones currently used by the ship")> <Category("Drones")> Public Property UsedDrones() As Integer
        Get
            Return _usedDrones
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Used Drones must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _usedDrones = value
            End If
        End Set
    End Property

    <Description("The maximum amount of drones to be used by the ship")> <Category("Drones")> Public Property MaxDrones() As Integer
        Get
            Return _maxDrones
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Maximum drones must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _maxDrones = value
            End If
        End Set
    End Property

#End Region

#Region "Targeting Properties"

    <Description("The maximum number of locked targets allowed by the ship")> <Category("Targeting")> Public Property MaxLockedTargets() As Double
        Get
            Return _maxLockedTargets
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Locked Targets must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _maxLockedTargets = value
            End If
        End Set
    End Property

    <Description("The maximum targeting range of the ship")> <Category("Targeting")> Public Property MaxTargetRange() As Double
        Get
            Return _maxTargetRange
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Targeting Range must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _maxTargetRange = value
            End If
        End Set
    End Property

    <Description("The base targeting speed of the ship")> <Category("Targeting")> Public Property TargetingSpeed() As Double
        Get
            Return _targetingSpeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Base Targeting Speed must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _targetingSpeed = value
            End If
        End Set
    End Property

    <Description("The scan resolution of the ship")> <Category("Targeting")> Public Property ScanResolution() As Double
        Get
            Return _scanResolution
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Scan Resolution must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _scanResolution = value
            End If
        End Set
    End Property

    <Description("The signature radius of the ship")> <Category("Targeting")> Public Property SigRadius() As Double
        Get
            Return _sigRadius
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Signature Radius must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _sigRadius = value
            End If
        End Set
    End Property

    <Description("The Gravimetric sensor strength of the ship")> <Category("Targeting")> Public Property GravSensorStrenth() As Double
        Get
            Return _gravSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _gravSensorStrenth = value
            End If
        End Set
    End Property

    <Description("The LADAR sensor strength of the ship")> <Category("Targeting")> Public Property LadarSensorStrenth() As Double
        Get
            Return _ladarSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _ladarSensorStrenth = value
            End If
        End Set
    End Property

    <Description("The Magnetometric sensor strength of the ship")> <Category("Targeting")> Public Property MagSensorStrenth() As Double
        Get
            Return _magSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _magSensorStrenth = value
            End If
        End Set
    End Property

    <Description("The RADAR sensor strength of the ship")> <Category("Targeting")> Public Property RadarSensorStrenth() As Double
        Get
            Return _radarSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _radarSensorStrenth = value
            End If
        End Set
    End Property

#End Region

#Region "Propulsion Properties"

    <Description("The maxmium velocity of the ship")> <Category("Propulsion")> Public Property MaxVelocity() As Double
        Get
            Return _maxVelocity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Velocity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _maxVelocity = value
            End If
        End Set
    End Property

    <Description("The inertia (agility) modifier of the ship")> <Category("Propulsion")> Public Property Inertia() As Double
        Get
            Return _inertia
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Inertia Modifier must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _inertia = value
            End If
        End Set
    End Property

    <Description("The Fusion propulsion strength of the ship")> <Category("Propulsion")> Public Property FusionPropStrength() As Double
        Get
            Return _fusionPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _fusionPropStrength = value
            End If
        End Set
    End Property

    <Description("The Ion propulsion strength of the ship")> <Category("Propulsion")> Public Property IonPropStrength() As Double
        Get
            Return _ionPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _ionPropStrength = value
            End If
        End Set
    End Property

    <Description("The Magpulse propulsion strength of the ship")> <Category("Propulsion")> Public Property MagpulsePropStrength() As Double
        Get
            Return _magpulsePropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _magpulsePropStrength = value
            End If
        End Set
    End Property

    <Description("The Plasma propulsion strength of the ship")> <Category("Propulsion")> Public Property PlasmaPropStrength() As Double
        Get
            Return _plasmaPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _plasmaPropStrength = value
            End If
        End Set
    End Property

    <Description("The maxmium warp velocity of the ship")> <Category("Propulsion")> Public Property WarpSpeed() As Double
        Get
            Return _warpSpeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Warp Speed must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _warpSpeed = value
            End If
        End Set
    End Property

    <Description("The capacitor required to move 1kg a distance of 1au")> <Category("Propulsion")> Public Property WarpCapNeed() As Double
        Get
            Return _warpCapNeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Warp Capacitor Need must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _warpCapNeed = value
            End If
        End Set
    End Property

#End Region

#Region "Fitted Slot Properties"

    <Browsable(False)> _
    <Description("The fitted high slots of the ship")> <Category("Fitted Slots")> Public Property HiSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _hiSlots) And OverrideFittingRules = False Then
                MessageBox.Show("High Slot index must be in the range 1 to " & _hiSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _hiSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _hiSlots) And OverrideFittingRules = False Then
                MessageBox.Show("High Slot index must be in the range 1 to " & _hiSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If _hiSlots > 8 Then ReDim Preserve _hiSlot(_hiSlots) ' Used if we artifically expand the slot count for weapon analysis
                If value Is Nothing Then
                    If _hiSlot(index) IsNot Nothing Then
                        HiSlotsUsed -= 1
                        If _hiSlot(index).IsLauncher Then
                            LauncherSlotsUsed -= 1
                        ElseIf _hiSlot(index).IsTurret Then
                            TurretSlotsUsed -= 1
                        End If
                        FittingBasePrice -= _hiSlot(index).BasePrice

                    End If
                Else
                    If _hiSlot(index) IsNot Nothing Then
                        HiSlotsUsed -= 1
                        If _hiSlot(index).IsLauncher Then
                            LauncherSlotsUsed -= 1
                        ElseIf _hiSlot(index).IsTurret Then
                            TurretSlotsUsed -= 1
                        End If
                        FittingBasePrice -= _hiSlot(index).BasePrice

                    End If
                    HiSlotsUsed += 1
                    If value.IsLauncher Then
                        LauncherSlotsUsed += 1
                    ElseIf value.IsTurret Then
                        TurretSlotsUsed += 1
                    End If
                    FittingBasePrice += value.BasePrice

                End If
                _hiSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted mid slots of the ship")> <Category("Fitted Slots")> Public Property MidSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _midSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Mid Slot index must be in the range 1 to " & _midSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _midSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _midSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Mid Slot index must be in the range 1 to " & _midSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _midSlot(index) IsNot Nothing Then
                        MidSlotsUsed -= 1
                        FittingBasePrice -= _midSlot(index).BasePrice

                    End If
                Else
                    If _midSlot(index) IsNot Nothing Then
                        MidSlotsUsed -= 1
                        FittingBasePrice -= _midSlot(index).BasePrice

                    End If
                    MidSlotsUsed += 1
                    FittingBasePrice += value.BasePrice

                End If
                _midSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted low slots of the ship")> <Category("Fitted Slots")> Public Property LowSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _lowSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Low Slot index must be in the range 1 to " & _lowSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _lowSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _lowSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Low Slot index must be in the range 1 to " & _lowSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _lowSlot(index) IsNot Nothing Then
                        LowSlotsUsed -= 1
                        FittingBasePrice -= _lowSlot(index).BasePrice

                    End If
                Else
                    If _lowSlot(index) IsNot Nothing Then
                        LowSlotsUsed -= 1
                        FittingBasePrice -= _lowSlot(index).BasePrice

                    End If
                    LowSlotsUsed += 1
                    FittingBasePrice += value.BasePrice

                End If
                _lowSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted rig slots of the ship")> <Category("Fitted Slots")> Public Property RigSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _rigSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Rig Slot index must be in the range 1 to " & _rigSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _rigSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _rigSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Rig Slot index must be in the range 1 to " & _rigSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _rigSlot(index) IsNot Nothing Then
                        RigSlotsUsed -= 1
                        FittingBasePrice -= _rigSlot(index).BasePrice
                    End If
                Else
                    If _rigSlot(index) IsNot Nothing Then
                        RigSlotsUsed -= 1
                        FittingBasePrice -= _rigSlot(index).BasePrice

                    End If
                    RigSlotsUsed += 1
                    FittingBasePrice += value.BasePrice
                End If
                _rigSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted subsystem slots of the ship")> <Category("Fitted Slots")> Public Property SubSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > _subSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Subsystem Slot index must be in the range 1 to " & _subSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return _subSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > _subSlots) And OverrideFittingRules = False Then
                MessageBox.Show("Subsystem Slot index must be in the range 1 to " & _subSlots & " for " & Name, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If _subSlot(index) IsNot Nothing Then
                        SubSlotsUsed -= 1
                        FittingBasePrice -= _subSlot(index).BasePrice

                    End If
                Else
                    If _subSlot(index) IsNot Nothing Then
                        SubSlotsUsed -= 1
                        FittingBasePrice -= _subSlot(index).BasePrice

                    End If
                    SubSlotsUsed += 1
                    FittingBasePrice += value.BasePrice

                End If
                _subSlot(index) = value
            End If
        End Set
    End Property

#End Region

#Region "Fitting Used Properties"

    <Browsable(False)> _
    <Description("The number of fitted high slots on the ship")> <Category("Fitting Used")> Public Property HiSlotsUsed() As Integer

    <Browsable(False)> _
    <Description("The number of fitted mide slots on the ship")> <Category("Fitting Used")> Public Property MidSlotsUsed() As Integer

    <Browsable(False)> _
    <Description("The number of fitted low slots on the ship")> <Category("Fitting Used")> Public Property LowSlotsUsed() As Integer

    <Browsable(False)> _
    <Description("The number of fitted rig slots on the ship")> <Category("Fitting Used")> Public Property RigSlotsUsed() As Integer

    <Browsable(False)> _
    <Description("The number of fitted subsystem slots on the ship")> <Category("Fitting Used")> Public Property SubSlotsUsed() As Integer

    <Browsable(False)> _
    <Description("The number of fitted turret slots on the ship")> <Category("Fitting Used")> Public Property TurretSlotsUsed() As Integer

    <Browsable(False)> _
    <Description("The number of fitted launcher slots on the ship")> <Category("Fitting Used")> Public Property LauncherSlotsUsed() As Integer

    <Browsable(False)> _
    <Description("The amount of calibration points used on the ship")> <Category("Fitting Used")> Public Property CalibrationUsed() As Integer

    <Browsable(False)> _
    <Description("The amount of CPU used on the ship")> <Category("Fitting Used")> Public Property CpuUsed() As Double

    <Browsable(False)> _
    <Description("The amount of powergrid used on the ship")> <Category("Fitting Used")> Public Property PgUsed() As Double

    <Browsable(False)> _
    <Description("The amount of cargo bay capacity used on the ship")> <Category("Fitting Used")> Public Property CargoBayUsed() As Double

    <Browsable(False)> _
    <Description("The amount of additional cargo bay capacity available on the ship")> <Category("Fitting Used")> Public Property CargoBayAdditional() As Double

    <Browsable(False)> _
    <Description("The amount of drone bay capacity used on the ship")> <Category("Fitting Used")> Public Property DroneBayUsed() As Double

    <Browsable(False)> _
    <Description("The amount of ship maintenance bay capacity used on the ship")> <Category("Fitting Used")> Public Property ShipBayUsed() As Double

    <Browsable(False)> _
    <Description("The amount of drone bandwidth used on the ship")> <Category("Fitting Used")> Public Property DroneBandwidthUsed() As Double

#End Region

#Region "Skill Properties"

    <Description("The minimum skills required to fly the ship hull")> <Category("Skills")> Public Property RequiredSkills() As SortedList

    <Browsable(False)> _
    <Description("The minimum skills required to fly the ship (inlcuding all modules)")> <Category("Skills")> Public Property RequiredSkillList() As SortedList

#End Region

#Region "Attribute Properties"

    <Description("The detailed attributes of the ship")> <Category("Attributes")> Public Property Attributes() As SortedList(Of String, Double)

#End Region

#Region "Storage Bay Items"

    <Browsable(False)> _
    <Description("The collection of items stored in the cargo bay of the ship")> <Category("Storage Bay Items")> Public Property CargoBayItems() As SortedList

    <Browsable(False)> _
    <Description("The collection of items stored in the drone bay of the ship")> <Category("Storage Bay Items")> Public Property DroneBayItems() As SortedList

    <Browsable(False)> _
    <Description("The collection of items stored in the ship maintenance bay of the ship")> <Category("Storage Bay Items")> Public Property ShipBayItems() As SortedList

#End Region

#Region "Effective HP Properties (Read Only)"

    <Browsable(False)> _
    <Description("The effective shield HP based on shield resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveShieldHP() As Double
        Get
            Return _effectiveShieldHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The effective armor HP based on armor resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveArmorHP() As Double
        Get
            Return _effectiveArmorHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The effective hull HP based on hull resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveStructureHP() As Double
        Get
            Return _effectiveStructureHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The overall effective HP of the ship")> <Category("Effective HP")> Public ReadOnly Property EffectiveHP() As Double
        Get
            Return _effectiveHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The overall effective HP of the ship, as stated by Eve")> <Category("Effective HP")> Public ReadOnly Property EveEffectiveHP() As Double
        Get
            Return _eveEffectiveHP
        End Get
    End Property

#End Region

#Region "Volley Damage Properties"

    <Browsable(False)> _
    <Description("The combined turret volley damage for the ship")> <Category("Volley Damage")> Public Property TurretVolley() As Double

    <Browsable(False)> _
    <Description("The combined missile volley damage for the ship")> <Category("Volley Damage")> Public Property MissileVolley() As Double

    <Browsable(False)> _
    <Description("The combined smartbomb volley damage for the ship")> <Category("Volley Damage")> Public Property SmartbombVolley() As Double

    <Browsable(False)> _
    <Description("The combined bomb volley damage for the ship")> <Category("Volley Damage")> Public Property BombVolley() As Double

    <Browsable(False)> _
    <Description("The combined drone volley damage for the ship")> <Category("Volley Damage")> Public Property DroneVolley() As Double

    <Browsable(False)> _
    <Description("The total volley damage for the ship")> <Category("Volley Damage")> Public Property TotalVolley() As Double

#End Region

#Region "DPS Properties"

    <Browsable(False)> _
    <Description("The combined turret DPS for the ship")> <Category("DPS")> Public Property TurretDPS() As Double

    <Browsable(False)> _
    <Description("The combined missile DPS for the ship")> <Category("DPS")> Public Property MissileDPS() As Double

    <Browsable(False)> _
    <Description("The combined smartbomb DPS for the ship")> <Category("DPS")> Public Property SmartbombDPS() As Double

    <Browsable(False)> _
    <Description("The combined bomb DPS for the ship")> <Category("DPS")> Public Property BombDPS() As Double

    <Browsable(False)> _
    <Description("The combined drone DPS for the ship")> <Category("DPS")> Public Property DroneDPS() As Double

    <Browsable(False)> _
    <Description("The total DPS for the ship")> <Category("DPS")> Public Property TotalDPS() As Double

#End Region

#Region "Ore Mining Properties"

    <Browsable(False)> _
    <Description("The combined turret ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreTurretAmount() As Double

    <Browsable(False)> _
    <Description("The combined drone ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreDroneAmount() As Double

    <Browsable(False)> _
    <Description("The total ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreTotalAmount() As Double

    <Browsable(False)> _
    <Description("The combined turret ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreTurretRate() As Double

    <Browsable(False)> _
    <Description("The combined drone ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreDroneRate() As Double

    <Browsable(False)> _
    <Description("The total ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreTotalRate() As Double

#End Region

#Region "Ice Mining Properties"

    <Browsable(False)> _
    <Description("The combined turret ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceTurretAmount() As Double

    <Browsable(False)> _
    <Description("The combined drone ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceDroneAmount() As Double

    <Browsable(False)> _
    <Description("The total ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceTotalAmount() As Double

    <Browsable(False)> _
    <Description("The combined turret ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceTurretRate() As Double

    <Browsable(False)> _
    <Description("The combined drone ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceDroneRate() As Double

    <Browsable(False)> _
    <Description("The total ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceTotalRate() As Double

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
    <Description("The list of audit log entries for the fitted ship")> <Category("Audit Log")> Public Property AuditLog() As ArrayList

#End Region

#Region "Damage Profile Properties"

    <Browsable(False)> _
    <Description("The damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfile() As HQFDamageProfile
        Get
            Return _damageProfile
        End Get
        Set(ByVal value As HQFDamageProfile)
            If value Is Nothing Then
                value = HQFDamageProfiles.ProfileList.Item("<Omni-Damage>")
            End If
            _damageProfile = value
            _EMExKiTh = _damageProfile.EM + _damageProfile.Explosive + _damageProfile.Kinetic + _damageProfile.Thermal
            _EM = _damageProfile.EM / _EMExKiTh
            _Ex = _damageProfile.Explosive / _EMExKiTh
            _Ki = _damageProfile.Kinetic / _EMExKiTh
            _Th = _damageProfile.Thermal / _EMExKiTh
        End Set
    End Property

    <Browsable(False)> _
    <Description("The EM element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileEM() As Double

    <Browsable(False)> _
    <Description("The Explosive element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileEx() As Double

    <Browsable(False)> _
    <Description("The Kinetic element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileKi() As Double

    <Browsable(False)> _
    <Description("The Thermal element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileTh() As Double

#End Region

#Region "Affects"

    <Browsable(False)> _
    <Description("The items which are affected by this ship")> <Category("Affects")> Public Property Affects() As ArrayList

    <Browsable(False)> _
    <Description("The items which are globally affected by this ship")> <Category("Affects")> Public Property GlobalAffects() As ArrayList

#End Region

#End Region

#Region "Cloning"
    Public Function Clone() As Ship
        Dim shipMemoryStream As New MemoryStream
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(shipMemoryStream, Me)
        shipMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newShip As Ship = CType(objBinaryFormatter.Deserialize(shipMemoryStream), Ship)
        shipMemoryStream.Close()
        Return newShip
    End Function
#End Region

#Region "Effective HP Calculations"

    Private Sub CalculateEffectiveShieldHP()
        _effectiveShieldHP = _shieldCapacity * 100 / (_EM * (100 - _shieldEMResist) + _Ex * (100 - _shieldExResist) + _Ki * (100 - _shieldKiResist) + _Th * (100 - _shieldThResist))
        Dim lowResist As Double = Math.Min(Math.Min(Math.Min(_shieldEMResist, _shieldExResist), _shieldKiResist), _shieldThResist)
        _eveEffectiveShieldHP = _shieldCapacity * 100 / (_EM * (100 - lowResist) + _Ex * (100 - lowResist) + _Ki * (100 - lowResist) + _Th * (100 - lowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveArmorHP()
        _effectiveArmorHP = _armorCapacity * 100 / (_EM * (100 - _armorEMResist) + _Ex * (100 - _armorExResist) + _Ki * (100 - _armorKiResist) + _Th * (100 - _armorThResist))
        Dim lowResist As Double = Math.Min(Math.Min(Math.Min(_armorEMResist, _armorExResist), _armorKiResist), _armorThResist)
        _eveEffectiveArmorHP = _armorCapacity * 100 / (_EM * (100 - lowResist) + _Ex * (100 - lowResist) + _Ki * (100 - lowResist) + _Th * (100 - lowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveStructureHP()
        _effectiveStructureHP = _structureCapacity * 100 / (_EM * (100 - _structureEMResist) + _Ex * (100 - _structureExResist) + _Ki * (100 - _structureKiResist) + _Th * (100 - _structureThResist))
        Dim lowResist As Double = Math.Min(Math.Min(Math.Min(_structureEMResist, _structureExResist), _structureKiResist), _structureThResist)
        _eveEffectiveStructureHP = _structureCapacity * 100 / (_EM * (100 - lowResist) + _Ex * (100 - lowResist) + _Ki * (100 - lowResist) + _Th * (100 - lowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveHP()
        _effectiveHP = _effectiveShieldHP + _effectiveArmorHP + _effectiveStructureHP
        _eveEffectiveHP = Int(_eveEffectiveShieldHP + _eveEffectiveArmorHP + _eveEffectiveStructureHP)
    End Sub
    Public Sub RecalculateEffectiveHP()
        Dim lowResist As Double
        ' Calculate Shield EHP
        _effectiveShieldHP = _shieldCapacity * 100 / (_EM * (100 - _shieldEMResist) + _Ex * (100 - _shieldExResist) + _Ki * (100 - _shieldKiResist) + _Th * (100 - _shieldThResist))
        lowResist = Math.Min(Math.Min(Math.Min(_shieldEMResist, _shieldExResist), _shieldKiResist), _shieldThResist)
        _eveEffectiveShieldHP = _shieldCapacity * 100 / (_EM * (100 - lowResist) + _Ex * (100 - lowResist) + _Ki * (100 - lowResist) + _Th * (100 - lowResist))
        ' Calculate Armor EHP
        _effectiveArmorHP = _armorCapacity * 100 / (_EM * (100 - _armorEMResist) + _Ex * (100 - _armorExResist) + _Ki * (100 - _armorKiResist) + _Th * (100 - _armorThResist))
        lowResist = Math.Min(Math.Min(Math.Min(_armorEMResist, _armorExResist), _armorKiResist), _armorThResist)
        _eveEffectiveArmorHP = _armorCapacity * 100 / (_EM * (100 - lowResist) + _Ex * (100 - lowResist) + _Ki * (100 - lowResist) + _Th * (100 - lowResist))
        ' Calculate Structure EHP
        _effectiveStructureHP = _structureCapacity * 100 / (_EM * (100 - _structureEMResist) + _Ex * (100 - _structureExResist) + _Ki * (100 - _structureKiResist) + _Th * (100 - _structureThResist))
        lowResist = Math.Min(Math.Min(Math.Min(_structureEMResist, _structureExResist), _structureKiResist), _structureThResist)
        _eveEffectiveStructureHP = _structureCapacity * 100 / (_EM * (100 - lowResist) + _Ex * (100 - lowResist) + _Ki * (100 - lowResist) + _Th * (100 - lowResist))
        ' Calculate Total EHP
        _effectiveHP = _effectiveShieldHP + _effectiveArmorHP + _effectiveStructureHP
        _eveEffectiveHP = Int(_eveEffectiveShieldHP + _eveEffectiveArmorHP + _eveEffectiveStructureHP)
    End Sub

#End Region

#Region "Add Custom Attributes"
    Public Sub AddCustomShipAttributes()
        ' Add the custom attributes to the list
        Attributes.Add("10002", Mass)
        Attributes.Add("10003", Volume)
        Attributes.Add("10004", CargoBay)
        Attributes.Add("10005", 0)
        Attributes.Add("10006", 0)
        Attributes.Add("10007", 20000)
        Attributes.Add("10008", 20000)
        Attributes.Add("10009", 1)
        Attributes.Add("10010", 0)
        Attributes.Add("10020", 0)
        Attributes.Add("10021", 0)
        Attributes.Add("10022", 0)
        Attributes.Add("10023", 0)
        Attributes.Add("10024", 0)
        Attributes.Add("10025", 0)
        Attributes.Add("10026", 0)
        Attributes.Add("10027", 0)
        Attributes.Add("10028", 0)
        Attributes.Add("10029", 0)
        Attributes.Add("10031", 0)
        Attributes.Add("10033", 0)
        Attributes.Add("10034", 0)
        Attributes.Add("10035", 0)
        Attributes.Add("10036", 0)
        Attributes.Add("10037", 0)
        Attributes.Add("10038", 0)
        Attributes.Add("10043", 0)
        Attributes.Add("10044", 0)
        Attributes.Add("10045", 0)
        Attributes.Add("10046", 0)
        Attributes.Add("10047", 0)
        Attributes.Add("10048", 0)
        Attributes.Add("10049", 0)
        Attributes.Add("10050", 0)
        Attributes.Add("10055", 0)
        Attributes.Add("10056", 0)
        Attributes.Add("10057", 0)
        Attributes.Add("10058", 0)
        Attributes.Add("10059", 0)
        Attributes.Add("10060", 0)
        Attributes.Add("10061", 0)
        Attributes.Add("10062", 0)
        Attributes.Add("10063", 1)
        Attributes.Add("10064", 2)
        Attributes.Add("10065", 0)
        Attributes.Add("10066", 0)
        Attributes.Add("10067", 0)
        Attributes.Add("10068", 0)
        Attributes.Add("10069", 0)
        Attributes.Add("10070", 0)
        Attributes.Add("10071", 0)
        Attributes.Add("10072", 0)
        Attributes.Add("10073", 0)
        Attributes.Add("10075", 0)
        Attributes.Add("10076", 0)
        Attributes.Add("10077", 0)
        Attributes.Add("10078", 0)
        Attributes.Add("10079", 0)
        Attributes.Add("10080", 0)
        Attributes.Add("10081", 0)
        Attributes.Add("10083", 0)
        ' Add unused attribute for calibration used
        Attributes.Add("1152", 0)
        ' Check for slot attributes (missing for T3)
        If Attributes.ContainsKey("12") = False Then
            Attributes.Add("12", 0)
            Attributes.Add("13", 0)
            Attributes.Add("14", 0)
        End If
        ' Check for cloak reactivation attribute
        If Attributes.ContainsKey("1034") = False Then
            Attributes.Add("1034", 30)
        End If

    End Sub
#End Region

#Region "Map Attributes to Properties"
    Public Shared Sub MapShipAttributes(ByVal newShip As Ship)
        Dim attValue As Double
        For Each att As String In newShip.Attributes.Keys
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
                    newShip.PgUsed = attValue
                Case 1132
                    newShip.Calibration = CInt(attValue)
                Case 1152
                    newShip.CalibrationUsed = CInt(attValue)
                Case 11
                    newShip.PG = attValue
                Case 48
                    newShip.CPU = attValue
                Case 49
                    newShip.CpuUsed = attValue
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
                    If newShip.Attributes.ContainsKey("600") = False Then
                        newShip.WarpSpeed = attValue
                    Else
                        newShip.WarpSpeed = attValue * CDbl(newShip.Attributes("600"))
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
        Attributes("12") = LowSlots
        Attributes("13") = MidSlots
        Attributes("14") = HiSlots
        Attributes("1137") = RigSlots
        Attributes("1367") = SubSlots
        Attributes("15") = PgUsed
        Attributes("1132") = Calibration
        Attributes("1152") = CalibrationUsed
        Attributes("11") = PG
        Attributes("48") = CPU
        Attributes("49") = CpuUsed
        Attributes("101") = LauncherSlots
        Attributes("102") = TurretSlots
        Attributes("55") = CapRecharge
        Attributes("482") = CapCapacity
        Attributes("9") = StructureCapacity
        Attributes("113") = StructureEMResist
        Attributes("111") = StructureExResist
        Attributes("109") = StructureKiResist
        Attributes("110") = StructureThResist
        Attributes("265") = ArmorCapacity
        Attributes("267") = ArmorEMResist
        Attributes("268") = ArmorExResist
        Attributes("269") = ArmorKiResist
        Attributes("270") = ArmorThResist
        Attributes("263") = ShieldCapacity
        Attributes("479") = ShieldRecharge
        Attributes("271") = ShieldEMResist
        Attributes("272") = ShieldExResist
        Attributes("273") = ShieldKiResist
        Attributes("274") = ShieldThResist
        Attributes("76") = MaxTargetRange
        Attributes("79") = TargetingSpeed
        Attributes("192") = MaxLockedTargets
        Attributes("552") = SigRadius
        Attributes("564") = ScanResolution
        Attributes("211") = GravSensorStrenth
        Attributes("209") = LadarSensorStrenth
        Attributes("210") = MagSensorStrenth
        Attributes("208") = RadarSensorStrenth
        Attributes("37") = MaxVelocity
        Attributes("70") = Inertia
        Attributes("153") = WarpCapNeed
        If Attributes.ContainsKey("600") = False Then
            Attributes("1281") = WarpSpeed
        Else
            Attributes("1281") = WarpSpeed * CDbl(Attributes("600"))
        End If
        Attributes("283") = DroneBay
        Attributes("908") = ShipBay
        Attributes("1271") = DroneBandwidth
        Attributes("10002") = Mass
        Attributes("10004") = CargoBay
        Attributes("10005") = MaxDrones
        Attributes("10006") = UsedDrones
        Attributes("10020") = TurretVolley
        Attributes("10021") = MissileVolley
        Attributes("10022") = SmartbombVolley
        Attributes("10023") = DroneVolley
        Attributes("10024") = TurretDPS
        Attributes("10025") = MissileDPS
        Attributes("10026") = SmartbombDPS
        Attributes("10027") = DroneDPS
        Attributes("10028") = TotalVolley
        Attributes("10029") = TotalDPS
        Attributes("10033") = OreTotalAmount
        Attributes("10034") = OreTurretAmount
        Attributes("10035") = OreDroneAmount
        Attributes("10036") = IceTotalAmount
        Attributes("10037") = IceTurretAmount
        Attributes("10038") = IceDroneAmount
        Attributes("10043") = OreTurretRate
        Attributes("10044") = OreDroneRate
        Attributes("10045") = IceTurretRate
        Attributes("10046") = IceDroneRate
        Attributes("10047") = OreTotalRate
        Attributes("10048") = IceTotalRate
        Attributes("10075") = ModuleTransferAmount
        Attributes("10076") = DroneTransferAmount
        Attributes("10077") = ModuleTransferRate
        Attributes("10078") = DroneTransferRate
        Attributes("10079") = TransferAmount
        Attributes("10080") = TransferRate
        Attributes("10081") = GasTotalAmount
        Attributes("10083") = GasTotalRate
    End Sub
#End Region

End Class

Public Enum SlotTypes As Integer
    Rig = 1
    Low = 2
    Mid = 4
    High = 8
    Subsystem = 16
End Enum

<Serializable()> Public Class ShipLists

    Public Shared Property ShipListKeyName As New SortedList(Of String, String)
    Public Shared Property ShipListKeyID As New SortedList(Of String, String)
    Public Shared Property ShipList As New SortedList   ' Key = ship name
    Public Shared Property FittedShipList As New SortedList   ' Key = fitting key

End Class

<Serializable()> Public Class DroneBayItem
    Public Property DroneType As ShipModule
    Public Property Quantity As Integer
    Public Property IsActive As Boolean
End Class

<Serializable()> Public Class CargoBayItem
    Public Property ItemType As ShipModule
    Public Property Quantity As Integer
End Class

<Serializable()> Public Class ShipBayItem
    Public Property ShipType As Ship
    Public Property Quantity As Integer
End Class
