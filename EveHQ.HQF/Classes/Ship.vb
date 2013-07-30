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

    ' Name and Classification
    Private cName As String
    Private cID As String
    Private cMarketGroup As String
    Private cDatabaseGroup As String
    Private cDatabaseCategory As String
    Private cDescription As String
    Private cBasePrice As Double
    Private cMarketPrice As Double
    Private cRaceID As Integer
    Private cIcon As String
    Private cFittingBasePrice As Double
    Private cFittingMarketPrice As Double

    ' Max Fitting Layout
    Private cHiSlots As Integer
    Private cMidSlots As Integer
    Private cLowSlots As Integer
    Private cRigSlots As Integer
    Private cSubSlots As Integer
    Private cTurretSlots As Integer
    Private cLauncherSlots As Integer
    Private cCalibration As Integer
    Private cOverrideFittingRules As Boolean = False
    Private cSlotCollection As New ArrayList
    Private cRemoteSlotCollection As New ArrayList
    Private cFleetSlotCollection As New ArrayList
    Private cEnviroSlotCollection As New ArrayList
    Private cBoosterSlotCollection As New ArrayList

    ' CPU, Power & Capacitor
    Private cCPU As Double
    Private cPG As Double
    Private cCapCapacity As Double
    Private cCapRecharge As Double

    ' Shield
    Private cShieldCapacity As Double
    Private cShieldRecharge As Double
    Private cShieldEMResist As Double
    Private cShieldExResist As Double
    Private cShieldKiResist As Double
    Private cShieldThResist As Double

    ' Armor
    Private cArmorCapacity As Double
    Private cArmorEMResist As Double
    Private cArmorExResist As Double
    Private cArmorKiResist As Double
    Private cArmorThResist As Double

    ' Structure
    Private cStructureCapacity As Double
    Private cStructureEMResist As Double
    Private cStructureExResist As Double
    Private cStructureKiResist As Double
    Private cStructureThResist As Double

    ' Space & Volume
    Private cCargoBay As Double
    Private cMass As Double
    Private cVolume As Double

    ' Drones
    Private cDroneBay As Double
    Private cDroneBandwidth As Double
    Private cUsedDrones As Integer
    Private cMaxDrones As Integer

    ' Ship Bay
    Private cShipBay As Double

    ' Targeting
    Private cMaxLockedTargets As Double
    Private cMaxTargetRange As Double
    Private cTargetingSpeed As Double
    Private cScanResolution As Double
    Private cSigRadius As Double
    Private cGravSensorStrenth As Double
    Private cLadarSensorStrenth As Double
    Private cMagSensorStrenth As Double
    Private cRadarSensorStrenth As Double

    ' Propulsion
    Private cMaxVelocity As Double
    Private cInertia As Double
    Private cFusionPropStrength As Double
    Private cIonPropStrength As Double
    Private cMagpulsePropStrength As Double
    Private cPlasmaPropStrength As Double
    Private cWarpSpeed As Double
    Private cWarpCapNeed As Double

    ' Module Slots
    Private cHiSlot(8) As ShipModule
    Private cMidSlot(8) As ShipModule
    Private cLowSlot(8) As ShipModule
    Private cRigSlot(8) As ShipModule
    Private cSubSlot(5) As ShipModule

    ' Skills
    Private cRequiredSkills As New SortedList
    Private cRequiredSkillList As New SortedList

    ' Attributes
    Private cAttributes As New SortedList(Of String, Double)

    ' "Used" Attributes
    Private cHiSlots_Used As Integer
    Private cMidSlots_Used As Integer
    Private cLowSlots_Used As Integer
    Private cRigSlots_Used As Integer
    Private cSubSlots_Used As Integer
    Private cTurretSlots_Used As Integer
    Private cLauncherSlots_Used As Integer
    Private cCalibration_Used As Integer
    Private cCPU_Used As Double
    Private cPG_Used As Double
    Private cCargoBay_Used As Double
    Private cCargoBay_Additional As Double
    Private cDroneBay_Used As Double
    Private cShipBay_Used As Double
    Private cDroneBandwidth_Used As Double
    Private cCargoBayItems As New SortedList
    Private cDroneBayItems As New SortedList
    Private cShipBayItems As New SortedList

    ' Effective Resists
    Private cEffectiveShieldHP As Double
    Private cEffectiveArmorHP As Double
    Private cEffectiveStructureHP As Double
    Private cEffectiveHP As Double
    Private cEveEffectiveShieldHP As Double
    Private cEveEffectiveArmorHP As Double
    Private cEveEffectiveStructureHP As Double
    Private cEveEffectiveHP As Double

    ' Damage
    Private cTurretVolley As Double
    Private cMissileVolley As Double
    Private cSBVolley As Double
    Private cBombVolley As Double
    Private cDroneVolley As Double
    Private cTurretDPS As Double
    Private cMissileDPS As Double
    Private cSBDPS As Double
    Private cBombDPS As Double
    Private cDroneDPS As Double
    Private cTotalVolley As Double
    Private cTotalDPS As Double

    ' Mining
    Private cOreTurretAmount As Double
    Private cOreDroneAmount As Double
    Private cOreTotalAmount As Double
    Private cIceTurretAmount As Double
    Private cIceDroneAmount As Double
    Private cIceTotalAmount As Double
    Private cOreTurretRate As Double
    Private cOreDroneRate As Double
    Private cOreTotalRate As Double
    Private cIceTurretRate As Double
    Private cIceDroneRate As Double
    Private cIceTotalRate As Double
    Private cGasTotalAmount As Double
    Private cGasTotalRate As Double

    ' Logistics
    Private cModuleTransferAmount As Double
    Private cModuleTransferRate As Double
    Private cDroneTransferAmount As Double
    Private cDroneTransferRate As Double
    Private cTransferAmount As Double
    Private cTransferRate As Double

    ' Audit Log
    Private cAuditLog As New ArrayList

    ' Damage Profile
    Private cDamageProfile As DamageProfile
    Private cEM As Double
    Private cEx As Double
    Private cKi As Double
    Private cTh As Double
    Private cEMExKiTh As Double

    ' Affected by
    Private cAffects As New ArrayList
    Private cGlobalAffects As New ArrayList

#End Region

#Region "Properties"

#Region "Database Properties"

    ' Note: These properties (except the RaceID) should not be directly editable in the property editor, but still visible

    <[ReadOnly](True)> _
    <Description("The name of the ship")> <Category("Database")> Public Property Name() As String
        Get
            Return cName
        End Get
        Set(ByVal value As String)
            cName = value
        End Set
    End Property

    <[ReadOnly](True)> _
    <Description("The ID of the ship")> <Category("Database")> Public Property ID() As String
        Get
            Return cID
        End Get
        Set(ByVal value As String)
            cID = value
        End Set
    End Property

    <[ReadOnly](True)> _
    <Description("The market group ID of the ship")> <Category("Database")> Public Property MarketGroup() As String
        Get
            Return cMarketGroup
        End Get
        Set(ByVal value As String)
            cMarketGroup = value
        End Set
    End Property

    <[ReadOnly](True)> _
    <Description("The database group ID of the ship")> <Category("Database")> Public Property DatabaseGroup() As String
        Get
            Return cDatabaseGroup
        End Get
        Set(ByVal value As String)
            cDatabaseGroup = value
        End Set
    End Property

    <[ReadOnly](True)> _
    <Description("The database category ID of the ship")> <Category("Database")> Public Property DatabaseCategory() As String
        Get
            Return cDatabaseCategory
        End Get
        Set(ByVal value As String)
            cDatabaseCategory = value
        End Set
    End Property

    <[ReadOnly](True)> _
    <Description("The description of the ship")> <Category("Database")> Public Property Description() As String
        Get
            Return cDescription
        End Get
        Set(ByVal value As String)
            cDescription = value
        End Set
    End Property

    <Description("The raceID of the ship")> <Category("Database")> Public Property RaceID() As Integer
        Get
            Return cRaceID
        End Get
        Set(ByVal value As Integer)
            cRaceID = value
        End Set
    End Property

    <[ReadOnly](True)> _
    <Description("The icon ID of the ship")> <Category("Database")> Public Property Icon() As String
        Get
            Return cIcon
        End Get
        Set(ByVal value As String)
            cIcon = value
        End Set
    End Property

#End Region

#Region "Price Properties"

    ' Nore: These properties should not be visible in the property editor as they are irrelevant

    <Browsable(False)> _
    <Description("The base price of the ship")> <Category("Price")> Public Property BasePrice() As Double
        Get
            Return cBasePrice
        End Get
        Set(ByVal value As Double)
            cBasePrice = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The market price of the ship")> <Category("Price")> Public Property MarketPrice() As Double
        Get
            Return cMarketPrice
        End Get
        Set(ByVal value As Double)
            cMarketPrice = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The base price of the ship including all fittings")> <Category("Price")> Public Property FittingBasePrice() As Double
        Get
            Return cFittingBasePrice
        End Get
        Set(ByVal value As Double)
            cFittingBasePrice = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The market price of the ship including all fittings")> <Category("Price")> Public Property FittingMarketPrice() As Double
        Get
            Return cFittingMarketPrice
        End Get
        Set(ByVal value As Double)
            cFittingMarketPrice = value
        End Set
    End Property

#End Region

#Region "Fitting Properties"

    <Description("The number of available high slots on the ship")> <Category("Fitting")> Public Property HiSlots() As Integer
        Get
            Return cHiSlots
        End Get
        Set(ByVal value As Integer)
            If cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("High slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cHiSlots = value
                End If
            Else
                cHiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available mid slots on the ship")> <Category("Fitting")> Public Property MidSlots() As Integer
        Get
            Return cMidSlots
        End Get
        Set(ByVal value As Integer)
            If cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Mid slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cMidSlots = value
                End If
            Else
                cHiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available low slots on the ship")> <Category("Fitting")> Public Property LowSlots() As Integer
        Get
            Return cLowSlots
        End Get
        Set(ByVal value As Integer)
            If cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Low slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cLowSlots = value
                End If
            Else
                cHiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available rig slots on the ship")> <Category("Fitting")> Public Property RigSlots() As Integer
        Get
            Return cRigSlots
        End Get
        Set(ByVal value As Integer)
            If cOverrideFittingRules = False Then
                If value < 0 Or value > MaxRigSlots Then
                    MessageBox.Show("Rig slots must be between 0 and " & MaxRigSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cRigSlots = value
                End If
            Else
                cHiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available subsystem slots on the ship")> <Category("Fitting")> Public Property SubSlots() As Integer
        Get
            Return cSubSlots
        End Get
        Set(ByVal value As Integer)
            If cOverrideFittingRules = False Then
                If value <> MaxSubSlots And value <> 0 Then
                    MessageBox.Show("The number of subsystem slots is currently restricted to " & MaxSubSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cSubSlots = value
                End If
            Else
                cHiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available turret slots on the ship")> <Category("Fitting")> Public Property TurretSlots() As Integer
        Get
            Return cTurretSlots
        End Get
        Set(ByVal value As Integer)
            If cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Turret slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cTurretSlots = value
                End If
            Else
                cHiSlots = value
            End If
        End Set
    End Property

    <Description("The number of available launcher slots on the ship")> <Category("Fitting")> Public Property LauncherSlots() As Integer
        Get
            Return cLauncherSlots
        End Get
        Set(ByVal value As Integer)
            If cOverrideFittingRules = False Then
                If value < 0 Or value > MaxBasicSlots Then
                    MessageBox.Show("Launcher slots must be between 0 and " & MaxBasicSlots.ToString, "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    cLauncherSlots = value
                End If
            Else
                cHiSlots = value
            End If
        End Set
    End Property

    <Description("The maximum CPU available for all modules on the ship")> <Category("Fitting")> Public Property CPU() As Double
        Get
            Return cCPU
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("CPU must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cCPU = value
            End If
        End Set
    End Property

    <Description("The maximum powergrid available for all modules on the ship")> <Category("Fitting")> Public Property PG() As Double
        Get
            Return cPG
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Powergrid must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cPG = value
            End If
        End Set
    End Property

    <Description("The maximum available calibration units available for rigs")> <Category("Fitting")> Public Property Calibration() As Integer
        Get
            Return cCalibration
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Calibration must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cCalibration = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("Allows fitting rules to be relaxed to increase fitting limits")> <Category("Fitting")> Public Property OverrideFittingRules() As Boolean
        Get
            Return cOverrideFittingRules
        End Get
        Set(ByVal value As Boolean)
            cOverrideFittingRules = value
        End Set
    End Property

#End Region

#Region "Slot Collection Properties"

    ' Note: None of these properties should be visible in the property editor

    <Browsable(False)> _
    <Description("The collection of internal modules for the ship")> <Category("Slot Collection")> Public Property SlotCollection() As ArrayList
        Get
            Return cSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cSlotCollection = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The collection of remote modules for the ship")> <Category("Slot Collection")> Public Property RemoteSlotCollection() As ArrayList
        Get
            Return cRemoteSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cRemoteSlotCollection = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The collection of fleet-based modules for the ship")> <Category("Slot Collection")> Public Property FleetSlotCollection() As ArrayList
        Get
            Return cFleetSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cFleetSlotCollection = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The collection of external environment modules for the ship")> <Category("Slot Collection")> Public Property EnviroSlotCollection() As ArrayList
        Get
            Return cEnviroSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cEnviroSlotCollection = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The collection of combat boosters for the ship")> <Category("Slot Collection")> Public Property BoosterSlotCollection() As ArrayList
        Get
            Return cBoosterSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cBoosterSlotCollection = value
        End Set
    End Property

#End Region

#Region "Capacitor Properties"

    <Description("The total available capacitor of the ship")> <Category("Capacitor")> Public Property CapCapacity() As Double
        Get
            Return cCapCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Capacitor capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cCapCapacity = value
            End If
        End Set
    End Property

    <Description("The capacitor recharge time of the ship")> <Category("Capacitor")> Public Property CapRecharge() As Double
        Get
            Return cCapRecharge
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Capacitor recharge time must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cCapRecharge = value
            End If
        End Set
    End Property

#End Region
    
#Region "Shield Properties"

    <Description("The shield hitpoint capacity of the ship")> <Category("Shield")> Public Property ShieldCapacity() As Double
        Get
            Return cShieldCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Shield capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cShieldCapacity = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield recharge time of the ship")> <Category("Shield")> Public Property ShieldRecharge() As Double
        Get
            Return cShieldRecharge
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Shield recharge time must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cShieldRecharge = value
            End If
        End Set
    End Property

    <Description("The shield EM resistance of the ship")> <Category("Shield")> Public Property ShieldEMResist() As Double
        Get
            Return cShieldEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cShieldEMResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield Explosive resistance of the ship")> <Category("Shield")> Public Property ShieldExResist() As Double
        Get
            Return cShieldExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cShieldExResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield Kinetic resistance of the ship")> <Category("Shield")> Public Property ShieldKiResist() As Double
        Get
            Return cShieldKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cShieldKiResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

    <Description("The shield Thermal resistance of the ship")> <Category("Shield")> Public Property ShieldThResist() As Double
        Get
            Return cShieldThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Shield resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cShieldThResist = value
                Call CalculateEffectiveShieldHP()
            End If
        End Set
    End Property

#End Region

#Region "Armor Properties"

    <Description("The armor hitpoint capacity of the ship")> <Category("Armor")> Public Property ArmorCapacity() As Double
        Get
            Return cArmorCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Armor capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cArmorCapacity = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor EM resistance of the ship")> <Category("Armor")> Public Property ArmorEMResist() As Double
        Get
            Return cArmorEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cArmorEMResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor Explosive resistance of the ship")> <Category("Armor")> Public Property ArmorExResist() As Double
        Get
            Return cArmorExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cArmorExResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor Kinetic resistance of the ship")> <Category("Armor")> Public Property ArmorKiResist() As Double
        Get
            Return cArmorKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cArmorKiResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property
    <Description("The armor Thermal resistance of the ship")> <Category("Armor")> Public Property ArmorThResist() As Double
        Get
            Return cArmorThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Armor resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cArmorThResist = value
                Call CalculateEffectiveArmorHP()
            End If
        End Set
    End Property

#End Region

#Region "Hull Properties"

    <Description("The hull hitpoint capacity of the ship")> <Category("Hull")> Public Property StructureCapacity() As Double
        Get
            Return cStructureCapacity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Structure capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cStructureCapacity = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull EM resistance of the ship")> <Category("Hull")> Public Property StructureEMResist() As Double
        Get
            Return cStructureEMResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cStructureEMResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull Explosive resistance of the ship")> <Category("Hull")> Public Property StructureExResist() As Double
        Get
            Return cStructureExResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cStructureExResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull Kinetic resistance of the ship")> <Category("Hull")> Public Property StructureKiResist() As Double
        Get
            Return cStructureKiResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cStructureKiResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

    <Description("The hull Thermal resistance of the ship")> <Category("Hull")> Public Property StructureThResist() As Double
        Get
            Return cStructureThResist
        End Get
        Set(ByVal value As Double)
            If value < 0 Or value > 100 Then
                MessageBox.Show("Structure resistances must be between 0 and 100.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cStructureThResist = value
                Call CalculateEffectiveStructureHP()
            End If
        End Set
    End Property

#End Region

#Region "Structure Properties"

    <Description("The mass of the ship")> <Category("Structure")> Public Property Mass() As Double
        Get
            Return cMass
        End Get
        Set(ByVal value As Double)
            cMass = value
        End Set
    End Property

    <Description("The unpacked volume of the ship")> <Category("Structure")> Public Property Volume() As Double
        Get
            Return cVolume
        End Get
        Set(ByVal value As Double)
            cVolume = value
        End Set
    End Property

#End Region

#Region "Storage Bay Properties"

    <Description("The cargo bay capacity of the ship")> <Category("Storage")> Public Property CargoBay() As Double
        Get
            Return cCargoBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Cargo Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cCargoBay = value
            End If
        End Set
    End Property

    <Description("The ship maintenance bay capacity of the ship")> <Category("Storage")> Public Property ShipBay() As Double
        Get
            Return cShipBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Ship Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cShipBay = value
            End If
        End Set
    End Property

#End Region

#Region "Drone Properties"

    <Description("The drone bay capacity of the ship")> <Category("Drones")> Public Property DroneBay() As Double
        Get
            Return cDroneBay
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Drone Bay capacity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cDroneBay = value
            End If
        End Set
    End Property

    <Description("The maxmium drone bandwidth capability of the ship")> <Category("Drones")> Public Property DroneBandwidth() As Double
        Get
            Return cDroneBandwidth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Drone bandwidth must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cDroneBandwidth = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of drones currently used by the ship")> <Category("Drones")> Public Property UsedDrones() As Integer
        Get
            Return cUsedDrones
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Used Drones must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cUsedDrones = value
            End If
        End Set
    End Property

    <Description("The maximum amount of drones to be used by the ship")> <Category("Drones")> Public Property MaxDrones() As Integer
        Get
            Return cMaxDrones
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                MessageBox.Show("Maximum drones must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cMaxDrones = value
            End If
        End Set
    End Property

#End Region

#Region "Targeting Properties"

    <Description("The maximum number of locked targets allowed by the ship")> <Category("Targeting")> Public Property MaxLockedTargets() As Double
        Get
            Return cMaxLockedTargets
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Locked Targets must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cMaxLockedTargets = value
            End If
        End Set
    End Property

    <Description("The maximum targeting range of the ship")> <Category("Targeting")> Public Property MaxTargetRange() As Double
        Get
            Return cMaxTargetRange
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Targeting Range must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cMaxTargetRange = value
            End If
        End Set
    End Property

    <Description("The base targeting speed of the ship")> <Category("Targeting")> Public Property TargetingSpeed() As Double
        Get
            Return cTargetingSpeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Base Targeting Speed must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cTargetingSpeed = value
            End If
        End Set
    End Property

    <Description("The scan resolution of the ship")> <Category("Targeting")> Public Property ScanResolution() As Double
        Get
            Return cScanResolution
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Scan Resolution must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cScanResolution = value
            End If
        End Set
    End Property

    <Description("The signature radius of the ship")> <Category("Targeting")> Public Property SigRadius() As Double
        Get
            Return cSigRadius
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Signature Radius must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cSigRadius = value
            End If
        End Set
    End Property

    <Description("The Gravimetric sensor strength of the ship")> <Category("Targeting")> Public Property GravSensorStrenth() As Double
        Get
            Return cGravSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cGravSensorStrenth = value
            End If
        End Set
    End Property

    <Description("The LADAR sensor strength of the ship")> <Category("Targeting")> Public Property LadarSensorStrenth() As Double
        Get
            Return cLadarSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cLadarSensorStrenth = value
            End If
        End Set
    End Property

    <Description("The Magnetometric sensor strength of the ship")> <Category("Targeting")> Public Property MagSensorStrenth() As Double
        Get
            Return cMagSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cMagSensorStrenth = value
            End If
        End Set
    End Property

    <Description("The RADAR sensor strength of the ship")> <Category("Targeting")> Public Property RadarSensorStrenth() As Double
        Get
            Return cRadarSensorStrenth
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Sensor Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cRadarSensorStrenth = value
            End If
        End Set
    End Property

#End Region

#Region "Propulsion Properties"

    <Description("The maxmium velocity of the ship")> <Category("Propulsion")> Public Property MaxVelocity() As Double
        Get
            Return cMaxVelocity
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Maximum Velocity must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cMaxVelocity = value
            End If
        End Set
    End Property

    <Description("The inertia (agility) modifier of the ship")> <Category("Propulsion")> Public Property Inertia() As Double
        Get
            Return cInertia
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Inertia Modifier must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cInertia = value
            End If
        End Set
    End Property

    <Description("The Fusion propulsion strength of the ship")> <Category("Propulsion")> Public Property FusionPropStrength() As Double
        Get
            Return cFusionPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cFusionPropStrength = value
            End If
        End Set
    End Property

    <Description("The Ion propulsion strength of the ship")> <Category("Propulsion")> Public Property IonPropStrength() As Double
        Get
            Return cIonPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cIonPropStrength = value
            End If
        End Set
    End Property

    <Description("The Magpulse propulsion strength of the ship")> <Category("Propulsion")> Public Property MagpulsePropStrength() As Double
        Get
            Return cMagpulsePropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cMagpulsePropStrength = value
            End If
        End Set
    End Property

    <Description("The Plasma propulsion strength of the ship")> <Category("Propulsion")> Public Property PlasmaPropStrength() As Double
        Get
            Return cPlasmaPropStrength
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Propulsion Strength must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cPlasmaPropStrength = value
            End If
        End Set
    End Property

    <Description("The maxmium warp velocity of the ship")> <Category("Propulsion")> Public Property WarpSpeed() As Double
        Get
            Return cWarpSpeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Warp Speed must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cWarpSpeed = value
            End If
        End Set
    End Property

    <Description("The capacitor required to move 1kg a distance of 1au")> <Category("Propulsion")> Public Property WarpCapNeed() As Double
        Get
            Return cWarpCapNeed
        End Get
        Set(ByVal value As Double)
            If value < 0 Then
                MessageBox.Show("Warp Capacitor Need must be a zero or positive value.", "Ship Properties Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cWarpCapNeed = value
            End If
        End Set
    End Property

#End Region

#Region "Fitted Slot Properties"

    <Browsable(False)> _
    <Description("The fitted high slots of the ship")> <Category("Fitted Slots")> Public Property HiSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > cHiSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("High Slot index must be in the range 1 to " & cHiSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cHiSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > cHiSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("High Slot index must be in the range 1 to " & cHiSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If cHiSlots > 8 Then ReDim Preserve cHiSlot(cHiSlots) ' Used if we artifically expand the slot count for weapon analysis
                If value Is Nothing Then
                    If cHiSlot(index) IsNot Nothing Then
                        cHiSlots_Used -= 1
                        If cHiSlot(index).IsLauncher Then
                            cLauncherSlots_Used -= 1
                        ElseIf cHiSlot(index).IsTurret Then
                            cTurretSlots_Used -= 1
                        End If
                        cFittingBasePrice -= cHiSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cHiSlot(index).ID)
                    End If
                Else
                    If cHiSlot(index) IsNot Nothing Then
                        cHiSlots_Used -= 1
                        If cHiSlot(index).IsLauncher Then
                            cLauncherSlots_Used -= 1
                        ElseIf cHiSlot(index).IsTurret Then
                            cTurretSlots_Used -= 1
                        End If
                        cFittingBasePrice -= cHiSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cHiSlot(index).ID)
                    End If
                    cHiSlots_Used += 1
                    If value.IsLauncher Then
                        cLauncherSlots_Used += 1
                    ElseIf value.IsTurret Then
                        cTurretSlots_Used += 1
                    End If
                    cFittingBasePrice += value.BasePrice
                    cFittingMarketPrice += EveHQ.Core.DataFunctions.GetPrice(value.ID)
                End If
                cHiSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted mid slots of the ship")> <Category("Fitted Slots")> Public Property MidSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > cMidSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Mid Slot index must be in the range 1 to " & cMidSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cMidSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > cMidSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Mid Slot index must be in the range 1 to " & cMidSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If cMidSlot(index) IsNot Nothing Then
                        cMidSlots_Used -= 1
                        cFittingBasePrice -= cMidSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cMidSlot(index).ID)
                    End If
                Else
                    If cMidSlot(index) IsNot Nothing Then
                        cMidSlots_Used -= 1
                        cFittingBasePrice -= cMidSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cMidSlot(index).ID)
                    End If
                    cMidSlots_Used += 1
                    cFittingBasePrice += value.BasePrice
                    cFittingMarketPrice += EveHQ.Core.DataFunctions.GetPrice(value.ID)
                End If
                cMidSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted low slots of the ship")> <Category("Fitted Slots")> Public Property LowSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > cLowSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Low Slot index must be in the range 1 to " & cLowSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cLowSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > cLowSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Low Slot index must be in the range 1 to " & cLowSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If cLowSlot(index) IsNot Nothing Then
                        cLowSlots_Used -= 1
                        cFittingBasePrice -= cLowSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cLowSlot(index).ID)
                    End If
                Else
                    If cLowSlot(index) IsNot Nothing Then
                        cLowSlots_Used -= 1
                        cFittingBasePrice -= cLowSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cLowSlot(index).ID)
                    End If
                    cLowSlots_Used += 1
                    cFittingBasePrice += value.BasePrice
                    cFittingMarketPrice += EveHQ.Core.DataFunctions.GetPrice(value.ID)
                End If
                cLowSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted rig slots of the ship")> <Category("Fitted Slots")> Public Property RigSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > cRigSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Rig Slot index must be in the range 1 to " & cRigSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cRigSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > cRigSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Rig Slot index must be in the range 1 to " & cRigSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If cRigSlot(index) IsNot Nothing Then
                        cRigSlots_Used -= 1
                        cFittingBasePrice -= cRigSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cRigSlot(index).ID)
                    End If
                Else
                    If cRigSlot(index) IsNot Nothing Then
                        cRigSlots_Used -= 1
                        cFittingBasePrice -= cRigSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cRigSlot(index).ID)
                    End If
                    cRigSlots_Used += 1
                    cFittingBasePrice += value.BasePrice
                    cFittingMarketPrice += EveHQ.Core.DataFunctions.GetPrice(value.ID)
                End If
                cRigSlot(index) = value
            End If
        End Set
    End Property

    <Browsable(False)> _
    <Description("The fitted subsystem slots of the ship")> <Category("Fitted Slots")> Public Property SubSlot(ByVal index As Integer) As ShipModule
        Get
            If (index < 1 Or index > cSubSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Subsystem Slot index must be in the range 1 to " & cSubSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cSubSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If (index < 1 Or index > cSubSlots) And cOverrideFittingRules = False Then
                MessageBox.Show("Subsystem Slot index must be in the range 1 to " & cSubSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If value Is Nothing Then
                    If cSubSlot(index) IsNot Nothing Then
                        cSubSlots_Used -= 1
                        cFittingBasePrice -= cSubSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cSubSlot(index).ID)
                    End If
                Else
                    If cSubSlot(index) IsNot Nothing Then
                        cSubSlots_Used -= 1
                        cFittingBasePrice -= cSubSlot(index).BasePrice
                        cFittingMarketPrice -= EveHQ.Core.DataFunctions.GetPrice(cSubSlot(index).ID)
                    End If
                    cSubSlots_Used += 1
                    cFittingBasePrice += value.BasePrice
                    cFittingMarketPrice += EveHQ.Core.DataFunctions.GetPrice(value.ID)
                End If
                cSubSlot(index) = value
            End If
        End Set
    End Property

#End Region

#Region "Fitting Used Properties"

    <Browsable(False)> _
    <Description("The number of fitted high slots on the ship")> <Category("Fitting Used")> Public Property HiSlots_Used() As Integer
        Get
            Return cHiSlots_Used
        End Get
        Set(ByVal value As Integer)
            cHiSlots_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of fitted mide slots on the ship")> <Category("Fitting Used")> Public Property MidSlots_Used() As Integer
        Get
            Return cMidSlots_Used
        End Get
        Set(ByVal value As Integer)
            cMidSlots_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of fitted low slots on the ship")> <Category("Fitting Used")> Public Property LowSlots_Used() As Integer
        Get
            Return cLowSlots_Used
        End Get
        Set(ByVal value As Integer)
            cLowSlots_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of fitted rig slots on the ship")> <Category("Fitting Used")> Public Property RigSlots_Used() As Integer
        Get
            Return cRigSlots_Used
        End Get
        Set(ByVal value As Integer)
            cRigSlots_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of fitted subsystem slots on the ship")> <Category("Fitting Used")> Public Property SubSlots_Used() As Integer
        Get
            Return cSubSlots_Used
        End Get
        Set(ByVal value As Integer)
            cSubSlots_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of fitted turret slots on the ship")> <Category("Fitting Used")> Public Property TurretSlots_Used() As Integer
        Get
            Return cTurretSlots_Used
        End Get
        Set(ByVal value As Integer)
            cTurretSlots_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The number of fitted launcher slots on the ship")> <Category("Fitting Used")> Public Property LauncherSlots_Used() As Integer
        Get
            Return cLauncherSlots_Used
        End Get
        Set(ByVal value As Integer)
            cLauncherSlots_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of calibration points used on the ship")> <Category("Fitting Used")> Public Property Calibration_Used() As Integer
        Get
            Return cCalibration_Used
        End Get
        Set(ByVal value As Integer)
            cCalibration_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of CPU used on the ship")> <Category("Fitting Used")> Public Property CPU_Used() As Double
        Get
            Return cCPU_Used
        End Get
        Set(ByVal value As Double)
            cCPU_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of powergrid used on the ship")> <Category("Fitting Used")> Public Property PG_Used() As Double
        Get
            Return cPG_Used
        End Get
        Set(ByVal value As Double)
            cPG_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of cargo bay capacity used on the ship")> <Category("Fitting Used")> Public Property CargoBay_Used() As Double
        Get
            Return cCargoBay_Used
        End Get
        Set(ByVal value As Double)
            cCargoBay_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of additional cargo bay capacity available on the ship")> <Category("Fitting Used")> Public Property CargoBay_Additional() As Double
        Get
            Return cCargoBay_Additional
        End Get
        Set(ByVal value As Double)
            cCargoBay_Additional = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of drone bay capacity used on the ship")> <Category("Fitting Used")> Public Property DroneBay_Used() As Double
        Get
            Return cDroneBay_Used
        End Get
        Set(ByVal value As Double)
            cDroneBay_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of ship maintenance bay capacity used on the ship")> <Category("Fitting Used")> Public Property ShipBay_Used() As Double
        Get
            Return cShipBay_Used
        End Get
        Set(ByVal value As Double)
            cShipBay_Used = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The amount of drone bandwidth used on the ship")> <Category("Fitting Used")> Public Property DroneBandwidth_Used() As Double
        Get
            Return cDroneBandwidth_Used
        End Get
        Set(ByVal value As Double)
            cDroneBandwidth_Used = value
        End Set
    End Property

#End Region

#Region "Skill Properties"

    <Description("The minimum skills required to fly the ship hull")> <Category("Skills")> Public Property RequiredSkills() As SortedList
        Get
            Return cRequiredSkills
        End Get
        Set(ByVal value As SortedList)
            cRequiredSkills = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The minimum skills required to fly the ship (inlcuding all modules)")> <Category("Skills")> Public Property RequiredSkillList() As SortedList
        Get
            Return cRequiredSkillList
        End Get
        Set(ByVal value As SortedList)
            cRequiredSkillList = value
        End Set
    End Property

#End Region

#Region "Attribute Properties"

    <Description("The detailed attributes of the ship")> <Category("Attributes")> Public Property Attributes() As SortedList(Of String, Double)
        Get
            Return cAttributes
        End Get
        Set(ByVal value As SortedList(Of String, Double))
            cAttributes = value
        End Set
    End Property

#End Region

#Region "Storage Bay Items"

    <Browsable(False)> _
    <Description("The collection of items stored in the cargo bay of the ship")> <Category("Storage Bay Items")> Public Property CargoBayItems() As SortedList
        Get
            Return cCargoBayItems
        End Get
        Set(ByVal value As SortedList)
            cCargoBayItems = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The collection of items stored in the drone bay of the ship")> <Category("Storage Bay Items")> Public Property DroneBayItems() As SortedList
        Get
            Return cDroneBayItems
        End Get
        Set(ByVal value As SortedList)
            cDroneBayItems = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The collection of items stored in the ship maintenance bay of the ship")> <Category("Storage Bay Items")> Public Property ShipBayItems() As SortedList
        Get
            Return cShipBayItems
        End Get
        Set(ByVal value As SortedList)
            cShipBayItems = value
        End Set
    End Property

#End Region

#Region "Effective HP Properties (Read Only)"

    <Browsable(False)> _
    <Description("The effective shield HP based on shield resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveShieldHP() As Double
        Get
            Return cEffectiveShieldHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The effective armor HP based on armor resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveArmorHP() As Double
        Get
            Return cEffectiveArmorHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The effective hull HP based on hull resistances")> <Category("Effective HP")> Public ReadOnly Property EffectiveStructureHP() As Double
        Get
            Return cEffectiveStructureHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The overall effective HP of the ship")> <Category("Effective HP")> Public ReadOnly Property EffectiveHP() As Double
        Get
            Return cEffectiveHP
        End Get
    End Property

    <Browsable(False)> _
    <Description("The overall effective HP of the ship, as stated by Eve")> <Category("Effective HP")> Public ReadOnly Property EveEffectiveHP() As Double
        Get
            Return cEveEffectiveHP
        End Get
    End Property

#End Region

#Region "Volley Damage Properties"

    <Browsable(False)> _
    <Description("The combined turret volley damage for the ship")> <Category("Volley Damage")> Public Property TurretVolley() As Double
        Get
            Return cTurretVolley
        End Get
        Set(ByVal value As Double)
            cTurretVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined missile volley damage for the ship")> <Category("Volley Damage")> Public Property MissileVolley() As Double
        Get
            Return cMissileVolley
        End Get
        Set(ByVal value As Double)
            cMissileVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined smartbomb volley damage for the ship")> <Category("Volley Damage")> Public Property SBVolley() As Double
        Get
            Return cSBVolley
        End Get
        Set(ByVal value As Double)
            cSBVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined bomb volley damage for the ship")> <Category("Volley Damage")> Public Property BombVolley() As Double
        Get
            Return cBombVolley
        End Get
        Set(ByVal value As Double)
            cBombVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone volley damage for the ship")> <Category("Volley Damage")> Public Property DroneVolley() As Double
        Get
            Return cDroneVolley
        End Get
        Set(ByVal value As Double)
            cDroneVolley = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total volley damage for the ship")> <Category("Volley Damage")> Public Property TotalVolley() As Double
        Get
            Return cTotalVolley
        End Get
        Set(ByVal value As Double)
            cTotalVolley = value
        End Set
    End Property

#End Region

#Region "DPS Properties"

    <Browsable(False)> _
    <Description("The combined turret DPS for the ship")> <Category("DPS")> Public Property TurretDPS() As Double
        Get
            Return cTurretDPS
        End Get
        Set(ByVal value As Double)
            cTurretDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined missile DPS for the ship")> <Category("DPS")> Public Property MissileDPS() As Double
        Get
            Return cMissileDPS
        End Get
        Set(ByVal value As Double)
            cMissileDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined smartbomb DPS for the ship")> <Category("DPS")> Public Property SBDPS() As Double
        Get
            Return cSBDPS
        End Get
        Set(ByVal value As Double)
            cSBDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined bomb DPS for the ship")> <Category("DPS")> Public Property BombDPS() As Double
        Get
            Return cBombDPS
        End Get
        Set(ByVal value As Double)
            cBombDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone DPS for the ship")> <Category("DPS")> Public Property DroneDPS() As Double
        Get
            Return cDroneDPS
        End Get
        Set(ByVal value As Double)
            cDroneDPS = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total DPS for the ship")> <Category("DPS")> Public Property TotalDPS() As Double
        Get
            Return cTotalDPS
        End Get
        Set(ByVal value As Double)
            cTotalDPS = value
        End Set
    End Property

#End Region

#Region "Ore Mining Properties"

    <Browsable(False)> _
    <Description("The combined turret ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreTurretAmount() As Double
        Get
            Return cOreTurretAmount
        End Get
        Set(ByVal value As Double)
            cOreTurretAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreDroneAmount() As Double
        Get
            Return cOreDroneAmount
        End Get
        Set(ByVal value As Double)
            cOreDroneAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ore mining amount for the ship")> <Category("Ore Mining")> Public Property OreTotalAmount() As Double
        Get
            Return cOreTotalAmount
        End Get
        Set(ByVal value As Double)
            cOreTotalAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined turret ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreTurretRate() As Double
        Get
            Return cOreTurretRate
        End Get
        Set(ByVal value As Double)
            cOreTurretRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreDroneRate() As Double
        Get
            Return cOreDroneRate
        End Get
        Set(ByVal value As Double)
            cOreDroneRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ore mining rate for the ship")> <Category("Ore Mining")> Public Property OreTotalRate() As Double
        Get
            Return cOreTotalRate
        End Get
        Set(ByVal value As Double)
            cOreTotalRate = value
        End Set
    End Property

#End Region

#Region "Ice Mining Properties"

    <Browsable(False)> _
    <Description("The combined turret ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceTurretAmount() As Double
        Get
            Return cIceTurretAmount
        End Get
        Set(ByVal value As Double)
            cIceTurretAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceDroneAmount() As Double
        Get
            Return cIceDroneAmount
        End Get
        Set(ByVal value As Double)
            cIceDroneAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ice mining amount for the ship")> <Category("Ice Mining")> Public Property IceTotalAmount() As Double
        Get
            Return cIceTotalAmount
        End Get
        Set(ByVal value As Double)
            cIceTotalAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined turret ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceTurretRate() As Double
        Get
            Return cIceTurretRate
        End Get
        Set(ByVal value As Double)
            cIceTurretRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined drone ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceDroneRate() As Double
        Get
            Return cIceDroneRate
        End Get
        Set(ByVal value As Double)
            cIceDroneRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total ice mining rate for the ship")> <Category("Ice Mining")> Public Property IceTotalRate() As Double
        Get
            Return cIceTotalRate
        End Get
        Set(ByVal value As Double)
            cIceTotalRate = value
        End Set
    End Property

#End Region

#Region "Gas Mining Properties"

    <Browsable(False)> _
    <Description("The total gas mining amount for the ship")> <Category("Gas Mining")> Public Property GasTotalAmount() As Double
        Get
            Return cGasTotalAmount
        End Get
        Set(ByVal value As Double)
            cGasTotalAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total gas mining rate for the ship")> <Category("Gas Mining")> Public Property GasTotalRate() As Double
        Get
            Return cGasTotalRate
        End Get
        Set(ByVal value As Double)
            cGasTotalRate = value
        End Set
    End Property

#End Region

#Region "Logistics Properties"

    <Browsable(False)> _
    <Description("The combined logistics module transfer amount for the ship")> <Category("Logistics")> Public Property ModuleTransferAmount() As Double
        Get
            Return cModuleTransferAmount
        End Get
        Set(ByVal value As Double)
            cModuleTransferAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined logistics module transfer rate for the ship")> <Category("Logistics")> Public Property ModuleTransferRate() As Double
        Get
            Return cModuleTransferRate
        End Get
        Set(ByVal value As Double)
            cModuleTransferRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined logistics drone transfer amount for the ship")> <Category("Logistics")> Public Property DroneTransferAmount() As Double
        Get
            Return cDroneTransferAmount
        End Get
        Set(ByVal value As Double)
            cDroneTransferAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The combined logistics drone transfer rate for the ship")> <Category("Logistics")> Public Property DroneTransferRate() As Double
        Get
            Return cDroneTransferRate
        End Get
        Set(ByVal value As Double)
            cDroneTransferRate = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total logistics transfer amount for the ship")> <Category("Logistics")> Public Property TransferAmount() As Double
        Get
            Return cTransferAmount
        End Get
        Set(ByVal value As Double)
            cTransferAmount = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The total logistics transfer rate for the ship")> <Category("Logistics")> Public Property TransferRate() As Double
        Get
            Return cTransferRate
        End Get
        Set(ByVal value As Double)
            cTransferRate = value
        End Set
    End Property

#End Region

#Region "Audit Log Properties"

    <Browsable(False)> _
    <Description("The list of audit log entries for the fitted ship")> <Category("Audit Log")> Public Property AuditLog() As ArrayList
        Get
            Return cAuditLog
        End Get
        Set(ByVal value As ArrayList)
            cAuditLog = value
        End Set
    End Property

#End Region

#Region "Damage Profile Properties"

    <Browsable(False)> _
    <Description("The damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfile() As DamageProfile
        Get
            Return cDamageProfile
        End Get
        Set(ByVal value As DamageProfile)
            If value Is Nothing Then
                value = CType(DamageProfiles.ProfileList.Item("<Omni-Damage>"), DamageProfile)
            End If
            cDamageProfile = value
            cEMExKiTh = cDamageProfile.EM + cDamageProfile.Explosive + cDamageProfile.Kinetic + cDamageProfile.Thermal
            cEM = cDamageProfile.EM / cEMExKiTh
            cEx = cDamageProfile.Explosive / cEMExKiTh
            cKi = cDamageProfile.Kinetic / cEMExKiTh
            cTh = cDamageProfile.Thermal / cEMExKiTh
        End Set
    End Property

    <Browsable(False)> _
    <Description("The EM element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileEM() As Double
        Get
            Return cEM
        End Get
        Set(ByVal value As Double)
            cEM = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The Explosive element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileEX() As Double
        Get
            Return cEx
        End Get
        Set(ByVal value As Double)
            cEx = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The Kinetic element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileKI() As Double
        Get
            Return cKi
        End Get
        Set(ByVal value As Double)
            cKi = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The Thermal element of the damage profile used to calculate the effective HP of the ship")> <Category("Damage Profile")> Public Property DamageProfileTH() As Double
        Get
            Return cTh
        End Get
        Set(ByVal value As Double)
            cTh = value
        End Set
    End Property

#End Region

#Region "Affects"

    <Browsable(False)> _
    <Description("The items which are affected by this ship")> <Category("Affects")> Public Property Affects() As ArrayList
        Get
            Return cAffects
        End Get
        Set(ByVal value As ArrayList)
            cAffects = value
        End Set
    End Property

    <Browsable(False)> _
    <Description("The items which are globally affected by this ship")> <Category("Affects")> Public Property GlobalAffects() As ArrayList
        Get
            Return cGlobalAffects
        End Get
        Set(ByVal value As ArrayList)
            cGlobalAffects = value
        End Set
    End Property

#End Region

#End Region

#Region "Cloning"
    Public Function Clone() As Ship
        Dim ShipMemoryStream As New MemoryStream
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(ShipMemoryStream, Me)
        ShipMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newShip As Ship = CType(objBinaryFormatter.Deserialize(ShipMemoryStream), Ship)
        ShipMemoryStream.Close()
        Return newShip
    End Function
#End Region

#Region "Effective HP Calculations"

    Private Sub CalculateEffectiveShieldHP()
        cEffectiveShieldHP = cShieldCapacity * 100 / (cEM * (100 - cShieldEMResist) + cEx * (100 - cShieldExResist) + cKi * (100 - cShieldKiResist) + cTh * (100 - cShieldThResist))
        Dim LowResist As Double = Math.Min(Math.Min(Math.Min(cShieldEMResist, cShieldExResist), cShieldKiResist), cShieldThResist)
        cEveEffectiveShieldHP = cShieldCapacity * 100 / (cEM * (100 - LowResist) + cEx * (100 - LowResist) + cKi * (100 - LowResist) + cTh * (100 - LowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveArmorHP()
        cEffectiveArmorHP = cArmorCapacity * 100 / (cEM * (100 - cArmorEMResist) + cEx * (100 - cArmorExResist) + cKi * (100 - cArmorKiResist) + cTh * (100 - cArmorThResist))
        Dim LowResist As Double = Math.Min(Math.Min(Math.Min(cArmorEMResist, cArmorExResist), cArmorKiResist), cArmorThResist)
        cEveEffectiveArmorHP = cArmorCapacity * 100 / (cEM * (100 - LowResist) + cEx * (100 - LowResist) + cKi * (100 - LowResist) + cTh * (100 - LowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveStructureHP()
        cEffectiveStructureHP = cStructureCapacity * 100 / (cEM * (100 - cStructureEMResist) + cEx * (100 - cStructureExResist) + cKi * (100 - cStructureKiResist) + cTh * (100 - cStructureThResist))
        Dim LowResist As Double = Math.Min(Math.Min(Math.Min(cStructureEMResist, cStructureExResist), cStructureKiResist), cStructureThResist)
        cEveEffectiveStructureHP = cStructureCapacity * 100 / (cEM * (100 - LowResist) + cEx * (100 - LowResist) + cKi * (100 - LowResist) + cTh * (100 - LowResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveHP()
        cEffectiveHP = cEffectiveShieldHP + cEffectiveArmorHP + cEffectiveStructureHP
        cEveEffectiveHP = Int(cEveEffectiveShieldHP + cEveEffectiveArmorHP + cEveEffectiveStructureHP)
    End Sub
    Public Sub RecalculateEffectiveHP()
        Dim LowResist As Double = 0
        ' Calculate Shield EHP
        cEffectiveShieldHP = cShieldCapacity * 100 / (cEM * (100 - cShieldEMResist) + cEx * (100 - cShieldExResist) + cKi * (100 - cShieldKiResist) + cTh * (100 - cShieldThResist))
        LowResist = Math.Min(Math.Min(Math.Min(cShieldEMResist, cShieldExResist), cShieldKiResist), cShieldThResist)
        cEveEffectiveShieldHP = cShieldCapacity * 100 / (cEM * (100 - LowResist) + cEx * (100 - LowResist) + cKi * (100 - LowResist) + cTh * (100 - LowResist))
        ' Calculate Armor EHP
        cEffectiveArmorHP = cArmorCapacity * 100 / (cEM * (100 - cArmorEMResist) + cEx * (100 - cArmorExResist) + cKi * (100 - cArmorKiResist) + cTh * (100 - cArmorThResist))
        LowResist = Math.Min(Math.Min(Math.Min(cArmorEMResist, cArmorExResist), cArmorKiResist), cArmorThResist)
        cEveEffectiveArmorHP = cArmorCapacity * 100 / (cEM * (100 - LowResist) + cEx * (100 - LowResist) + cKi * (100 - LowResist) + cTh * (100 - LowResist))
        ' Calculate Structure EHP
        cEffectiveStructureHP = cStructureCapacity * 100 / (cEM * (100 - cStructureEMResist) + cEx * (100 - cStructureExResist) + cKi * (100 - cStructureKiResist) + cTh * (100 - cStructureThResist))
        LowResist = Math.Min(Math.Min(Math.Min(cStructureEMResist, cStructureExResist), cStructureKiResist), cStructureThResist)
        cEveEffectiveStructureHP = cStructureCapacity * 100 / (cEM * (100 - LowResist) + cEx * (100 - LowResist) + cKi * (100 - LowResist) + cTh * (100 - LowResist))
        ' Calculate Total EHP
        cEffectiveHP = cEffectiveShieldHP + cEffectiveArmorHP + cEffectiveStructureHP
        cEveEffectiveHP = Int(cEveEffectiveShieldHP + cEveEffectiveArmorHP + cEveEffectiveStructureHP)
    End Sub

#End Region

#Region "Add Custom Attributes"
    Public Sub AddCustomShipAttributes()
        ' Add the custom attributes to the list
        Me.Attributes.Add("10002", Me.Mass)
        Me.Attributes.Add("10003", Me.Volume)
        Me.Attributes.Add("10004", Me.CargoBay)
        Me.Attributes.Add("10005", 0)
        Me.Attributes.Add("10006", 0)
        Me.Attributes.Add("10007", 20000)
        Me.Attributes.Add("10008", 20000)
        Me.Attributes.Add("10009", 1)
        Me.Attributes.Add("10010", 0)
        Me.Attributes.Add("10020", 0)
        Me.Attributes.Add("10021", 0)
        Me.Attributes.Add("10022", 0)
        Me.Attributes.Add("10023", 0)
        Me.Attributes.Add("10024", 0)
        Me.Attributes.Add("10025", 0)
        Me.Attributes.Add("10026", 0)
        Me.Attributes.Add("10027", 0)
        Me.Attributes.Add("10028", 0)
        Me.Attributes.Add("10029", 0)
        Me.Attributes.Add("10031", 0)
        Me.Attributes.Add("10033", 0)
        Me.Attributes.Add("10034", 0)
        Me.Attributes.Add("10035", 0)
        Me.Attributes.Add("10036", 0)
        Me.Attributes.Add("10037", 0)
        Me.Attributes.Add("10038", 0)
        Me.Attributes.Add("10043", 0)
        Me.Attributes.Add("10044", 0)
        Me.Attributes.Add("10045", 0)
        Me.Attributes.Add("10046", 0)
        Me.Attributes.Add("10047", 0)
        Me.Attributes.Add("10048", 0)
        Me.Attributes.Add("10049", 0)
        Me.Attributes.Add("10050", 0)
        Me.Attributes.Add("10055", 0)
        Me.Attributes.Add("10056", 0)
        Me.Attributes.Add("10057", 0)
        Me.Attributes.Add("10058", 0)
        Me.Attributes.Add("10059", 0)
        Me.Attributes.Add("10060", 0)
        Me.Attributes.Add("10061", 0)
        Me.Attributes.Add("10062", 0)
        Me.Attributes.Add("10063", 1)
        Me.Attributes.Add("10064", 2)
        Me.Attributes.Add("10065", 0)
        Me.Attributes.Add("10066", 0)
        Me.Attributes.Add("10067", 0)
        Me.Attributes.Add("10068", 0)
        Me.Attributes.Add("10069", 0)
        Me.Attributes.Add("10070", 0)
        Me.Attributes.Add("10071", 0)
        Me.Attributes.Add("10072", 0)
        Me.Attributes.Add("10073", 0)
        Me.Attributes.Add("10075", 0)
        Me.Attributes.Add("10076", 0)
        Me.Attributes.Add("10077", 0)
        Me.Attributes.Add("10078", 0)
        Me.Attributes.Add("10079", 0)
        Me.Attributes.Add("10080", 0)
        Me.Attributes.Add("10081", 0)
        Me.Attributes.Add("10083", 0)
        ' Add unused attribute for calibration used
        Me.Attributes.Add("1152", 0)
        ' Check for slot attributes (missing for T3)
        If Me.Attributes.ContainsKey("12") = False Then
            Me.Attributes.Add("12", 0)
            Me.Attributes.Add("13", 0)
            Me.Attributes.Add("14", 0)
        End If
        ' Check for cloak reactivation attribute
        If Me.Attributes.ContainsKey("1034") = False Then
            Me.Attributes.Add("1034", 30)
        End If

    End Sub
#End Region

#Region "Map Attributes to Properties"
    Public Shared Sub MapShipAttributes(ByVal newShip As Ship)
        Dim attValue As Double = 0
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
                    newShip.PG_Used = attValue
                Case 1132
                    newShip.Calibration = CInt(attValue)
                Case 1152
                    newShip.Calibration_Used = CInt(attValue)
                Case 11
                    newShip.PG = attValue
                Case 48
                    newShip.CPU = attValue
                Case 49
                    newShip.CPU_Used = attValue
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
                    newShip.SBVolley = attValue
                Case 10023
                    newShip.DroneVolley = attValue
                Case 10024
                    newShip.TurretDPS = attValue
                Case 10025
                    newShip.MissileDPS = attValue
                Case 10026
                    newShip.SBDPS = attValue
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
        Me.Attributes("12") = Me.LowSlots
        Me.Attributes("13") = Me.MidSlots
        Me.Attributes("14") = Me.HiSlots
        Me.Attributes("1137") = Me.RigSlots
        Me.Attributes("1367") = Me.SubSlots
        Me.Attributes("15") = Me.PG_Used
        Me.Attributes("1132") = Me.Calibration
        Me.Attributes("1152") = Me.Calibration_Used
        Me.Attributes("11") = Me.PG
        Me.Attributes("48") = Me.CPU
        Me.Attributes("49") = Me.CPU_Used
        Me.Attributes("101") = Me.LauncherSlots
        Me.Attributes("102") = Me.TurretSlots
        Me.Attributes("55") = Me.CapRecharge
        Me.Attributes("482") = Me.CapCapacity
        Me.Attributes("9") = Me.StructureCapacity
        Me.Attributes("113") = Me.StructureEMResist
        Me.Attributes("111") = Me.StructureExResist
        Me.Attributes("109") = Me.StructureKiResist
        Me.Attributes("110") = Me.StructureThResist
        Me.Attributes("265") = Me.ArmorCapacity
        Me.Attributes("267") = Me.ArmorEMResist
        Me.Attributes("268") = Me.ArmorExResist
        Me.Attributes("269") = Me.ArmorKiResist
        Me.Attributes("270") = Me.ArmorThResist
        Me.Attributes("263") = Me.ShieldCapacity
        Me.Attributes("479") = Me.ShieldRecharge
        Me.Attributes("271") = Me.ShieldEMResist
        Me.Attributes("272") = Me.ShieldExResist
        Me.Attributes("273") = Me.ShieldKiResist
        Me.Attributes("274") = Me.ShieldThResist
        Me.Attributes("76") = Me.MaxTargetRange
        Me.Attributes("79") = Me.TargetingSpeed
        Me.Attributes("192") = Me.MaxLockedTargets
        Me.Attributes("552") = Me.SigRadius
        Me.Attributes("564") = Me.ScanResolution
        Me.Attributes("211") = Me.GravSensorStrenth
        Me.Attributes("209") = Me.LadarSensorStrenth
        Me.Attributes("210") = Me.MagSensorStrenth
        Me.Attributes("208") = Me.RadarSensorStrenth
        Me.Attributes("37") = Me.MaxVelocity
        Me.Attributes("70") = Me.Inertia
        Me.Attributes("153") = Me.WarpCapNeed
        If Me.Attributes.ContainsKey("600") = False Then
            Me.Attributes("1281") = Me.WarpSpeed
        Else
            Me.Attributes("1281") = Me.WarpSpeed * CDbl(Me.Attributes("600"))
        End If
        Me.Attributes("283") = Me.DroneBay
        Me.Attributes("908") = Me.ShipBay
        Me.Attributes("1271") = Me.DroneBandwidth
        Me.Attributes("10002") = Me.Mass
        Me.Attributes("10004") = Me.CargoBay
        Me.Attributes("10005") = Me.MaxDrones
        Me.Attributes("10006") = Me.UsedDrones
        Me.Attributes("10020") = Me.TurretVolley
        Me.Attributes("10021") = Me.MissileVolley
        Me.Attributes("10022") = Me.SBVolley
        Me.Attributes("10023") = Me.DroneVolley
        Me.Attributes("10024") = Me.TurretDPS
        Me.Attributes("10025") = Me.MissileDPS
        Me.Attributes("10026") = Me.SBDPS
        Me.Attributes("10027") = Me.DroneDPS
        Me.Attributes("10028") = Me.TotalVolley
        Me.Attributes("10029") = Me.TotalDPS
        Me.Attributes("10033") = Me.OreTotalAmount
        Me.Attributes("10034") = Me.OreTurretAmount
        Me.Attributes("10035") = Me.OreDroneAmount
        Me.Attributes("10036") = Me.IceTotalAmount
        Me.Attributes("10037") = Me.IceTurretAmount
        Me.Attributes("10038") = Me.IceDroneAmount
        Me.Attributes("10043") = Me.OreTurretRate
        Me.Attributes("10044") = Me.OreDroneRate
        Me.Attributes("10045") = Me.IceTurretRate
        Me.Attributes("10046") = Me.IceDroneRate
        Me.Attributes("10047") = Me.OreTotalRate
        Me.Attributes("10048") = Me.IceTotalRate
        Me.Attributes("10075") = Me.ModuleTransferAmount
        Me.Attributes("10076") = Me.DroneTransferAmount
        Me.Attributes("10077") = Me.ModuleTransferRate
        Me.Attributes("10078") = Me.DroneTransferRate
        Me.Attributes("10079") = Me.TransferAmount
        Me.Attributes("10080") = Me.TransferRate
        Me.Attributes("10081") = Me.GasTotalAmount
        Me.Attributes("10083") = Me.GasTotalRate
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

    Public Shared shipListKeyName As New SortedList(Of String, String)
    Public Shared shipListKeyID As New SortedList(Of String, String)
    Public Shared shipList As New SortedList   ' Key = ship name
    Public Shared fittedShipList As New SortedList   ' Key = fitting key

End Class

<Serializable()> Public Class DroneBayItem
    Public DroneType As ShipModule
    Public Quantity As Integer
    Public IsActive As Boolean
End Class

<Serializable()> Public Class CargoBayItem
    Public ItemType As ShipModule
    Public Quantity As Integer
End Class

<Serializable()> Public Class ShipBayItem
    Public ShipType As Ship
    Public Quantity As Integer
End Class
