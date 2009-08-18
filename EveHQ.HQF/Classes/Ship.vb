' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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

<Serializable()> Public Class Ship

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
    Private cPilotName As String

    ' Max Fitting Layout
    Private cHiSlots As Integer
    Private cMidSlots As Integer
    Private cLowSlots As Integer
    Private cRigSlots As Integer
    Private cSubSlots As Integer
    Private cTurretSlots As Integer
    Private cLauncherSlots As Integer
    Private cCalibration As Integer
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
    Private cRadius As Double

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
    Private cAttributes As New SortedList

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
    ' Name and Classification
    Public Property Name() As String
        Get
            Return cName
        End Get
        Set(ByVal value As String)
            cName = value
        End Set
    End Property
    Public Property ID() As String
        Get
            Return cID
        End Get
        Set(ByVal value As String)
            cID = value
        End Set
    End Property
    Public Property MarketGroup() As String
        Get
            Return cMarketGroup
        End Get
        Set(ByVal value As String)
            cMarketGroup = value
        End Set
    End Property
    Public Property DatabaseGroup() As String
        Get
            Return cDatabaseGroup
        End Get
        Set(ByVal value As String)
            cDatabaseGroup = value
        End Set
    End Property
    Public Property DatabaseCategory() As String
        Get
            Return cDatabaseCategory
        End Get
        Set(ByVal value As String)
            cDatabaseCategory = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return cDescription
        End Get
        Set(ByVal value As String)
            cDescription = value
        End Set
    End Property
    Public Property BasePrice() As Double
        Get
            Return cBasePrice
        End Get
        Set(ByVal value As Double)
            cBasePrice = value
        End Set
    End Property
    Public Property MarketPrice() As Double
        Get
            Return cMarketPrice
        End Get
        Set(ByVal value As Double)
            cMarketPrice = value
        End Set
    End Property
    Public Property RaceID() As Integer
        Get
            Return cRaceID
        End Get
        Set(ByVal value As Integer)
            cRaceID = value
        End Set
    End Property
    Public Property Icon() As String
        Get
            Return cIcon
        End Get
        Set(ByVal value As String)
            cIcon = value
        End Set
    End Property
    Public Property FittingBasePrice() As Double
        Get
            Return cFittingBasePrice
        End Get
        Set(ByVal value As Double)
            cFittingBasePrice = value
        End Set
    End Property
    Public Property FittingMarketPrice() As Double
        Get
            Return cFittingMarketPrice
        End Get
        Set(ByVal value As Double)
            cFittingMarketPrice = value
        End Set
    End Property
    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
        End Set
    End Property

    ' Max Fitting Layout
    Public Property HiSlots() As Integer
        Get
            Return cHiSlots
        End Get
        Set(ByVal value As Integer)
            cHiSlots = value
        End Set
    End Property
    Public Property MidSlots() As Integer
        Get
            Return cMidSlots
        End Get
        Set(ByVal value As Integer)
            cMidSlots = value
        End Set
    End Property
    Public Property LowSlots() As Integer
        Get
            Return cLowSlots
        End Get
        Set(ByVal value As Integer)
            cLowSlots = value
        End Set
    End Property
    Public Property RigSlots() As Integer
        Get
            Return cRigSlots
        End Get
        Set(ByVal value As Integer)
            cRigSlots = value
        End Set
    End Property
    Public Property SubSlots() As Integer
        Get
            Return cSubSlots
        End Get
        Set(ByVal value As Integer)
            cSubSlots = value
        End Set
    End Property
    Public Property TurretSlots() As Integer
        Get
            Return cTurretSlots
        End Get
        Set(ByVal value As Integer)
            cTurretSlots = value
        End Set
    End Property
    Public Property LauncherSlots() As Integer
        Get
            Return cLauncherSlots
        End Get
        Set(ByVal value As Integer)
            cLauncherSlots = value
        End Set
    End Property
    Public Property Calibration() As Integer
        Get
            Return cCalibration
        End Get
        Set(ByVal value As Integer)
            cCalibration = value
        End Set
    End Property
    Public Property SlotCollection() As ArrayList
        Get
            Return cSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cSlotCollection = value
        End Set
    End Property
    Public Property RemoteSlotCollection() As ArrayList
        Get
            Return cRemoteSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cRemoteSlotCollection = value
        End Set
    End Property
    Public Property FleetSlotCollection() As ArrayList
        Get
            Return cFleetSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cFleetSlotCollection = value
        End Set
    End Property
    Public Property EnviroSlotCollection() As ArrayList
        Get
            Return cEnviroSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cEnviroSlotCollection = value
        End Set
    End Property
    Public Property BoosterSlotCollection() As ArrayList
        Get
            Return cBoosterSlotCollection
        End Get
        Set(ByVal value As ArrayList)
            cBoosterSlotCollection = value
        End Set
    End Property

    ' CPU, Power & Capacitor
    Public Property CPU() As Double
        Get
            Return cCPU
        End Get
        Set(ByVal value As Double)
            cCPU = value
        End Set
    End Property
    Public Property PG() As Double
        Get
            Return cPG
        End Get
        Set(ByVal value As Double)
            cPG = value
        End Set
    End Property
    Public Property CapCapacity() As Double
        Get
            Return cCapCapacity
        End Get
        Set(ByVal value As Double)
            cCapCapacity = value
        End Set
    End Property
    Public Property CapRecharge() As Double
        Get
            Return cCapRecharge
        End Get
        Set(ByVal value As Double)
            cCapRecharge = value
        End Set
    End Property

    ' Shield
    Public Property ShieldCapacity() As Double
        Get
            Return cShieldCapacity
        End Get
        Set(ByVal value As Double)
            cShieldCapacity = value
            Call CalculateEffectiveShieldHP()
        End Set
    End Property
    Public Property ShieldRecharge() As Double
        Get
            Return cShieldRecharge
        End Get
        Set(ByVal value As Double)
            cShieldRecharge = value
        End Set
    End Property
    Public Property ShieldEMResist() As Double
        Get
            Return cShieldEMResist
        End Get
        Set(ByVal value As Double)
            cShieldEMResist = value
            Call CalculateEffectiveShieldHP()
        End Set
    End Property
    Public Property ShieldExResist() As Double
        Get
            Return cShieldExResist
        End Get
        Set(ByVal value As Double)
            cShieldExResist = value
            Call CalculateEffectiveShieldHP()
        End Set
    End Property
    Public Property ShieldKiResist() As Double
        Get
            Return cShieldKiResist
        End Get
        Set(ByVal value As Double)
            cShieldKiResist = value
            Call CalculateEffectiveShieldHP()
        End Set
    End Property
    Public Property ShieldThResist() As Double
        Get
            Return cShieldThResist
        End Get
        Set(ByVal value As Double)
            cShieldThResist = value
            Call CalculateEffectiveShieldHP()
        End Set
    End Property

    ' Armor
    Public Property ArmorCapacity() As Double
        Get
            Return cArmorCapacity
        End Get
        Set(ByVal value As Double)
            cArmorCapacity = value
            Call CalculateEffectiveArmorHP()
        End Set
    End Property
    Public Property ArmorEMResist() As Double
        Get
            Return cArmorEMResist
        End Get
        Set(ByVal value As Double)
            cArmorEMResist = value
            Call CalculateEffectiveArmorHP()
        End Set
    End Property
    Public Property ArmorExResist() As Double
        Get
            Return cArmorExResist
        End Get
        Set(ByVal value As Double)
            cArmorExResist = value
            Call CalculateEffectiveArmorHP()
        End Set
    End Property
    Public Property ArmorKiResist() As Double
        Get
            Return cArmorKiResist
        End Get
        Set(ByVal value As Double)
            cArmorKiResist = value
            Call CalculateEffectiveArmorHP()
        End Set
    End Property
    Public Property ArmorThResist() As Double
        Get
            Return cArmorThResist
        End Get
        Set(ByVal value As Double)
            cArmorThResist = value
            Call CalculateEffectiveArmorHP()
        End Set
    End Property

    ' Structure
    Public Property StructureCapacity() As Double
        Get
            Return cStructureCapacity
        End Get
        Set(ByVal value As Double)
            cStructureCapacity = value
            Call CalculateEffectiveStructureHP()
        End Set
    End Property
    Public Property StructureEMResist() As Double
        Get
            Return cStructureEMResist
        End Get
        Set(ByVal value As Double)
            cStructureEMResist = value
            Call CalculateEffectiveStructureHP()
        End Set
    End Property
    Public Property StructureExResist() As Double
        Get
            Return cStructureExResist
        End Get
        Set(ByVal value As Double)
            cStructureExResist = value
            Call CalculateEffectiveStructureHP()
        End Set
    End Property
    Public Property StructureKiResist() As Double
        Get
            Return cStructureKiResist
        End Get
        Set(ByVal value As Double)
            cStructureKiResist = value
            Call CalculateEffectiveStructureHP()
        End Set
    End Property
    Public Property StructureThResist() As Double
        Get
            Return cStructureThResist
        End Get
        Set(ByVal value As Double)
            cStructureThResist = value
            Call CalculateEffectiveStructureHP()
        End Set
    End Property

    ' Space & Volume
    Public Property CargoBay() As Double
        Get
            Return cCargoBay
        End Get
        Set(ByVal value As Double)
            cCargoBay = value
        End Set
    End Property
    Public Property Mass() As Double
        Get
            Return cMass
        End Get
        Set(ByVal value As Double)
            cMass = value
        End Set
    End Property
    Public Property Volume() As Double
        Get
            Return cVolume
        End Get
        Set(ByVal value As Double)
            cVolume = value
        End Set
    End Property
    Public Property Radius() As Double
        Get
            Return cRadius
        End Get
        Set(ByVal value As Double)
            cRadius = value
        End Set
    End Property

    ' Drones
    Public Property DroneBay() As Double
        Get
            Return cDroneBay
        End Get
        Set(ByVal value As Double)
            cDroneBay = value
        End Set
    End Property
    Public Property DroneBandwidth() As Double
        Get
            Return cDroneBandwidth
        End Get
        Set(ByVal value As Double)
            cDroneBandwidth = value
        End Set
    End Property
    Public Property UsedDrones() As Integer
        Get
            Return cUsedDrones
        End Get
        Set(ByVal value As Integer)
            cUsedDrones = value
        End Set
    End Property
    Public Property MaxDrones() As Integer
        Get
            Return cMaxDrones
        End Get
        Set(ByVal value As Integer)
            cMaxDrones = value
        End Set
    End Property

    ' Ship Bay
    Public Property ShipBay() As Double
        Get
            Return cShipBay
        End Get
        Set(ByVal value As Double)
            cShipBay = value
        End Set
    End Property

    ' Targeting
    Public Property MaxLockedTargets() As Double
        Get
            Return cMaxLockedTargets
        End Get
        Set(ByVal value As Double)
            cMaxLockedTargets = value
        End Set
    End Property
    Public Property MaxTargetRange() As Double
        Get
            Return cMaxTargetRange
        End Get
        Set(ByVal value As Double)
            cMaxTargetRange = value
        End Set
    End Property
    Public Property TargetingSpeed() As Double
        Get
            Return cTargetingSpeed
        End Get
        Set(ByVal value As Double)
            cTargetingSpeed = value
        End Set
    End Property
    Public Property ScanResolution() As Double
        Get
            Return cScanResolution
        End Get
        Set(ByVal value As Double)
            cScanResolution = value
        End Set
    End Property
    Public Property SigRadius() As Double
        Get
            Return cSigRadius
        End Get
        Set(ByVal value As Double)
            cSigRadius = value
        End Set
    End Property
    Public Property GravSensorStrenth() As Double
        Get
            Return cGravSensorStrenth
        End Get
        Set(ByVal value As Double)
            cGravSensorStrenth = value
        End Set
    End Property
    Public Property LadarSensorStrenth() As Double
        Get
            Return cLadarSensorStrenth
        End Get
        Set(ByVal value As Double)
            cLadarSensorStrenth = value
        End Set
    End Property
    Public Property MagSensorStrenth() As Double
        Get
            Return cMagSensorStrenth
        End Get
        Set(ByVal value As Double)
            cMagSensorStrenth = value
        End Set
    End Property
    Public Property RadarSensorStrenth() As Double
        Get
            Return cRadarSensorStrenth
        End Get
        Set(ByVal value As Double)
            cRadarSensorStrenth = value
        End Set
    End Property

    ' Propulsion
    Public Property MaxVelocity() As Double
        Get
            Return cMaxVelocity
        End Get
        Set(ByVal value As Double)
            cMaxVelocity = value
        End Set
    End Property
    Public Property Inertia() As Double
        Get
            Return cInertia
        End Get
        Set(ByVal value As Double)
            cInertia = value
        End Set
    End Property
    Public Property FusionPropStrength() As Double
        Get
            Return cFusionPropStrength
        End Get
        Set(ByVal value As Double)
            cFusionPropStrength = value
        End Set
    End Property
    Public Property IonPropStrength() As Double
        Get
            Return cIonPropStrength
        End Get
        Set(ByVal value As Double)
            cIonPropStrength = value
        End Set
    End Property
    Public Property MagpulsePropStrength() As Double
        Get
            Return cMagpulsePropStrength
        End Get
        Set(ByVal value As Double)
            cMagpulsePropStrength = value
        End Set
    End Property
    Public Property PlasmaPropStrength() As Double
        Get
            Return cPlasmaPropStrength
        End Get
        Set(ByVal value As Double)
            cPlasmaPropStrength = value
        End Set
    End Property
    Public Property WarpSpeed() As Double
        Get
            Return cWarpSpeed
        End Get
        Set(ByVal value As Double)
            cWarpSpeed = value
        End Set
    End Property
    Public Property WarpCapNeed() As Double
        Get
            Return cWarpCapNeed
        End Get
        Set(ByVal value As Double)
            cWarpCapNeed = value
        End Set
    End Property

    ' Module Slots
    Public Property HiSlot(ByVal index As Integer) As ShipModule
        Get
            If index < 1 Or index > cHiSlots Then
                MessageBox.Show("High Slot index must be in the range 1 to " & cHiSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cHiSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If index < 1 Or index > cHiSlots Then
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
    Public Property MidSlot(ByVal index As Integer) As ShipModule
        Get
            If index < 1 Or index > cMidSlots Then
                MessageBox.Show("Mid Slot index must be in the range 1 to " & cMidSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cMidSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If index < 1 Or index > cMidSlots Then
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
    Public Property LowSlot(ByVal index As Integer) As ShipModule
        Get
            If index < 1 Or index > cLowSlots Then
                MessageBox.Show("Low Slot index must be in the range 1 to " & cLowSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cLowSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If index < 1 Or index > cLowSlots Then
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
    Public Property RigSlot(ByVal index As Integer) As ShipModule
        Get
            If index < 1 Or index > cRigSlots Then
                MessageBox.Show("Rig Slot index must be in the range 1 to " & cRigSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cRigSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If index < 1 Or index > cRigSlots Then
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
    Public Property SubSlot(ByVal index As Integer) As ShipModule
        Get
            If index < 1 Or index > cSubSlots Then
                MessageBox.Show("Subsystem Slot index must be in the range 1 to " & cSubSlots & " for " & cName, "EveHQ HQF Slot Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Nothing
            Else
                Return cSubSlot(index)
            End If
        End Get
        Set(ByVal value As ShipModule)
            If index < 1 Or index > cSubSlots Then
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

    ' Skills
    Public Property RequiredSkills() As SortedList
        Get
            Return cRequiredSkills
        End Get
        Set(ByVal value As SortedList)
            cRequiredSkills = value
        End Set
    End Property
    Public Property RequiredSkillList() As SortedList
        Get
            Return cRequiredSkillList
        End Get
        Set(ByVal value As SortedList)
            cRequiredSkillList = value
        End Set
    End Property

    ' Attributes
    Public Property Attributes() As SortedList
        Get
            Return cAttributes
        End Get
        Set(ByVal value As SortedList)
            cAttributes = value
        End Set
    End Property

    ' "Used" Attributes
    Public Property HiSlots_Used() As Integer
        Get
            Return cHiSlots_Used
        End Get
        Set(ByVal value As Integer)
            cHiSlots_Used = value
        End Set
    End Property
    Public Property MidSlots_Used() As Integer
        Get
            Return cMidSlots_Used
        End Get
        Set(ByVal value As Integer)
            cMidSlots_Used = value
        End Set
    End Property
    Public Property LowSlots_Used() As Integer
        Get
            Return cLowSlots_Used
        End Get
        Set(ByVal value As Integer)
            cLowSlots_Used = value
        End Set
    End Property
    Public Property RigSlots_Used() As Integer
        Get
            Return cRigSlots_Used
        End Get
        Set(ByVal value As Integer)
            cRigSlots_Used = value
        End Set
    End Property
    Public Property SubSlots_Used() As Integer
        Get
            Return cSubSlots_Used
        End Get
        Set(ByVal value As Integer)
            cSubSlots_Used = value
        End Set
    End Property
    Public Property TurretSlots_Used() As Integer
        Get
            Return cTurretSlots_Used
        End Get
        Set(ByVal value As Integer)
            cTurretSlots_Used = value
        End Set
    End Property
    Public Property LauncherSlots_Used() As Integer
        Get
            Return cLauncherSlots_Used
        End Get
        Set(ByVal value As Integer)
            cLauncherSlots_Used = value
        End Set
    End Property
    Public Property Calibration_Used() As Integer
        Get
            Return cCalibration_Used
        End Get
        Set(ByVal value As Integer)
            cCalibration_Used = value
        End Set
    End Property
    Public Property CPU_Used() As Double
        Get
            Return cCPU_Used
        End Get
        Set(ByVal value As Double)
            cCPU_Used = value
        End Set
    End Property
    Public Property PG_Used() As Double
        Get
            Return cPG_Used
        End Get
        Set(ByVal value As Double)
            cPG_Used = value
        End Set
    End Property
    Public Property CargoBay_Used() As Double
        Get
            Return cCargoBay_Used
        End Get
        Set(ByVal value As Double)
            cCargoBay_Used = value
        End Set
    End Property
    Public Property CargoBay_Additional() As Double
        Get
            Return cCargoBay_Additional
        End Get
        Set(ByVal value As Double)
            cCargoBay_Additional = value
        End Set
    End Property
    Public Property DroneBay_Used() As Double
        Get
            Return cDroneBay_Used
        End Get
        Set(ByVal value As Double)
            cDroneBay_Used = value
        End Set
    End Property
    Public Property ShipBay_Used() As Double
        Get
            Return cShipBay_Used
        End Get
        Set(ByVal value As Double)
            cShipBay_Used = value
        End Set
    End Property
    Public Property DroneBandwidth_Used() As Double
        Get
            Return cDroneBandwidth_Used
        End Get
        Set(ByVal value As Double)
            cDroneBandwidth_Used = value
        End Set
    End Property
    Public Property CargoBayItems() As SortedList
        Get
            Return cCargoBayItems
        End Get
        Set(ByVal value As SortedList)
            cCargoBayItems = value
        End Set
    End Property
    Public Property DroneBayItems() As SortedList
        Get
            Return cDroneBayItems
        End Get
        Set(ByVal value As SortedList)
            cDroneBayItems = value
        End Set
    End Property
    Public Property ShipBayItems() As SortedList
        Get
            Return cShipBayItems
        End Get
        Set(ByVal value As SortedList)
            cShipBayItems = value
        End Set
    End Property

    ' Effective Resists (Read Only!)
    Public ReadOnly Property EffectiveShieldHP() As Double
        Get
            Return cEffectiveShieldHP
        End Get
    End Property
    Public ReadOnly Property EffectiveArmorHP() As Double
        Get
            Return cEffectiveArmorHP
        End Get
    End Property
    Public ReadOnly Property EffectiveStructureHP() As Double
        Get
            Return cEffectiveStructureHP
        End Get
    End Property
    Public ReadOnly Property EffectiveHP() As Double
        Get
            Return cEffectiveHP
        End Get
    End Property

    'Damage
    Public Property TurretVolley() As Double
        Get
            Return cTurretVolley
        End Get
        Set(ByVal value As Double)
            cTurretVolley = value
        End Set
    End Property
    Public Property MissileVolley() As Double
        Get
            Return cMissileVolley
        End Get
        Set(ByVal value As Double)
            cMissileVolley = value
        End Set
    End Property
    Public Property SBVolley() As Double
        Get
            Return cSBVolley
        End Get
        Set(ByVal value As Double)
            cSBVolley = value
        End Set
    End Property
    Public Property BombVolley() As Double
        Get
            Return cBombVolley
        End Get
        Set(ByVal value As Double)
            cBombVolley = value
        End Set
    End Property
    Public Property DroneVolley() As Double
        Get
            Return cDroneVolley
        End Get
        Set(ByVal value As Double)
            cDroneVolley = value
        End Set
    End Property
    Public Property TurretDPS() As Double
        Get
            Return cTurretDPS
        End Get
        Set(ByVal value As Double)
            cTurretDPS = value
        End Set
    End Property
    Public Property MissileDPS() As Double
        Get
            Return cMissileDPS
        End Get
        Set(ByVal value As Double)
            cMissileDPS = value
        End Set
    End Property
    Public Property SBDPS() As Double
        Get
            Return cSBDPS
        End Get
        Set(ByVal value As Double)
            cSBDPS = value
        End Set
    End Property
    Public Property BombDPS() As Double
        Get
            Return cBombDPS
        End Get
        Set(ByVal value As Double)
            cBombDPS = value
        End Set
    End Property
    Public Property DroneDPS() As Double
        Get
            Return cDroneDPS
        End Get
        Set(ByVal value As Double)
            cDroneDPS = value
        End Set
    End Property
    Public Property TotalVolley() As Double
        Get
            Return cTotalVolley
        End Get
        Set(ByVal value As Double)
            cTotalVolley = value
        End Set
    End Property
    Public Property TotalDPS() As Double
        Get
            Return cTotalDPS
        End Get
        Set(ByVal value As Double)
            cTotalDPS = value
        End Set
    End Property

    ' Mining
    Public Property OreTurretAmount() As Double
        Get
            Return cOreTurretAmount
        End Get
        Set(ByVal value As Double)
            cOreTurretAmount = value
        End Set
    End Property
    Public Property OreDroneAmount() As Double
        Get
            Return cOreDroneAmount
        End Get
        Set(ByVal value As Double)
            cOreDroneAmount = value
        End Set
    End Property
    Public Property OreTotalAmount() As Double
        Get
            Return cOreTotalAmount
        End Get
        Set(ByVal value As Double)
            cOreTotalAmount = value
        End Set
    End Property
    Public Property IceTurretAmount() As Double
        Get
            Return cIceTurretAmount
        End Get
        Set(ByVal value As Double)
            cIceTurretAmount = value
        End Set
    End Property
    Public Property IceDroneAmount() As Double
        Get
            Return cIceDroneAmount
        End Get
        Set(ByVal value As Double)
            cIceDroneAmount = value
        End Set
    End Property
    Public Property IceTotalAmount() As Double
        Get
            Return cIceTotalAmount
        End Get
        Set(ByVal value As Double)
            cIceTotalAmount = value
        End Set
    End Property
    Public Property OreTurretRate() As Double
        Get
            Return cOreTurretRate
        End Get
        Set(ByVal value As Double)
            cOreTurretRate = value
        End Set
    End Property
    Public Property OreDroneRate() As Double
        Get
            Return cOreDroneRate
        End Get
        Set(ByVal value As Double)
            cOreDroneRate = value
        End Set
    End Property
    Public Property OreTotalRate() As Double
        Get
            Return cOreTotalRate
        End Get
        Set(ByVal value As Double)
            cOreTotalRate = value
        End Set
    End Property
    Public Property IceTurretRate() As Double
        Get
            Return cIceTurretRate
        End Get
        Set(ByVal value As Double)
            cIceTurretRate = value
        End Set
    End Property
    Public Property IceDroneRate() As Double
        Get
            Return cIceDroneRate
        End Get
        Set(ByVal value As Double)
            cIceDroneRate = value
        End Set
    End Property
    Public Property IceTotalRate() As Double
        Get
            Return cIceTotalRate
        End Get
        Set(ByVal value As Double)
            cIceTotalRate = value
        End Set
    End Property

    'Audit Log
    Public Property AuditLog() As ArrayList
        Get
            Return cAuditLog
        End Get
        Set(ByVal value As ArrayList)
            cAuditLog = value
        End Set
    End Property

    ' Damage Profile
    Public Property DamageProfile() As DamageProfile
        Get
            Return cDamageProfile
        End Get
        Set(ByVal value As DamageProfile)
            cDamageProfile = value
            cEMExKiTh = cDamageProfile.EM + cDamageProfile.Explosive + cDamageProfile.Kinetic + cDamageProfile.Thermal
            cEM = cDamageProfile.EM / cEMExKiTh
            cEx = cDamageProfile.Explosive / cEMExKiTh
            cKi = cDamageProfile.Kinetic / cEMExKiTh
            cTh = cDamageProfile.Thermal / cEMExKiTh
        End Set
    End Property
    Public Property DamageProfileEM() As Double
        Get
            Return cEM
        End Get
        Set(ByVal value As Double)
            cEM = value
        End Set
    End Property
    Public Property DamageProfileEX() As Double
        Get
            Return cEx
        End Get
        Set(ByVal value As Double)
            cEx = value
        End Set
    End Property
    Public Property DamageProfileKI() As Double
        Get
            Return cKi
        End Get
        Set(ByVal value As Double)
            cKi = value
        End Set
    End Property
    Public Property DamageProfileTH() As Double
        Get
            Return cTh
        End Get
        Set(ByVal value As Double)
            cTh = value
        End Set
    End Property

    ' Affected by
    Public Property Affects() As ArrayList
        Get
            Return cAffects
        End Get
        Set(ByVal value As ArrayList)
            cAffects = value
        End Set
    End Property
    Public Property GlobalAffects() As ArrayList
        Get
            Return cGlobalAffects
        End Get
        Set(ByVal value As ArrayList)
            cGlobalAffects = value
        End Set
    End Property

#End Region

#Region "Cloning"
    Public Function Clone() As Ship
        Dim ShipMemoryStream As New MemoryStream(10000)
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
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveArmorHP()
        cEffectiveArmorHP = cArmorCapacity * 100 / (cEM * (100 - cArmorEMResist) + cEx * (100 - cArmorExResist) + cKi * (100 - cArmorKiResist) + cTh * (100 - cArmorThResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveStructureHP()
        cEffectiveStructureHP = cStructureCapacity * 100 / (cEM * (100 - cStructureEMResist) + cEx * (100 - cStructureExResist) + cKi * (100 - cStructureKiResist) + cTh * (100 - cStructureThResist))
        Call CalculateEffectiveHP()
    End Sub
    Private Sub CalculateEffectiveHP()
        cEffectiveHP = cEffectiveShieldHP + cEffectiveArmorHP + cEffectiveStructureHP
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
                Case 1154 ' This is a changed attribute as it duplicates 1132 in the database!!
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
                    If CDbl(newShip.Attributes("600")) <> 0 Then
                        newShip.WarpSpeed = attValue * CDbl(newShip.Attributes("600"))
                    Else
                        newShip.WarpSpeed = attValue
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
            End Select
        Next
    End Sub
#End Region

End Class

<Serializable()> Public Class ShipLists

    Public Shared shipListKeyName As New SortedList
    Public Shared shipListKeyID As New SortedList
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





