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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

<Serializable()> Public Class ShipModule

#Region "Constants"
    ' itemIDs (see invTypes)
    Public Const Item_CommandProcessorI As String = "11014"
    Public Const Item_LegionCovertReconfiguration As String = "30120"
    Public Const Item_LegionWarfareProcessor As String = "29967"
    Public Const Item_LokiCovertReconfiguration As String = "30135"
    Public Const Item_LokiWarfareProcessor As String = "29977"
    Public Const Item_NaniteRepairPaste As String = "28668"
    Public Const Item_ProteusCovertReconfiguration As String = "30130"
    Public Const Item_ProteusWarfareProcessor As String = "29982"
    Public Const Item_SiegeModuleI As String = "20280"
    Public Const Item_SiegeModuleII As String = "4292"
    Public Const Item_TenguCovertReconfiguration As String = "30125"
    Public Const Item_TenguWarfareProcessor As String = "29972"
    Public Const Item_TriageModuleI As String = "27951"
    Public Const Item_TriageModuleII As String = "4294"

    ' categoryIDs (see invCategories)
    Public Const Category_Celestials As String = "2"
    Public Const Category_Charges As String = "8"
    Public Const Category_Drones As String = "18"
    Public Const Category_Implants As String = "20"
    Public Const Category_Subsystems As String = "32"

    ' groupIDs (see invGroups)
    Public Const Group_ArmorRepairers As String = "62"
    Public Const Group_ArmorResistShiftHardener As String = "1150"
    Public Const Group_BlockadeRunners As String = "1202"
    Public Const Group_BombLaunchers As String = "862"
    Public Const Group_Boosters As String = "303"
    Public Const Group_CapBoosters As String = "76"
    Public Const Group_CloakingDevices As String = "330"
    Public Const Group_CynosuralFields As String = "658"
    Public Const Group_DamageControls As String = "60"
    Public Const Group_DeepSpaceTransports As String = "380"
    Public Const Group_DNAMutators As String = "304"
    Public Const Group_EnergyNeutralizers As String = "71"
    Public Const Group_EnergyNeutralizerDrones As String = "544"
    Public Const Group_EnergyTransfers As String = "67"
    Public Const Group_EnergyTurrets As String = "53"
    Public Const Group_EnergyVampires As String = "68"
    Public Const Group_Exhumers As String = "543"
    Public Const Group_Freighters As String = "513"
    Public Const Group_FueledArmorRepairers As String = "1199"
    Public Const Group_FueledShieldBoosters As String = "1156"
    Public Const Group_GangLinks As String = "316"
    Public Const Group_HullRepairers As String = "63"
    Public Const Group_HybridTurrets As String = "74"
    Public Const Group_IndustrialCommandShips As String = "941"
    Public Const Group_Industrials As String = "28"
    Public Const Group_JumpFreighters As String = "902"
    Public Const Group_LogisticDrones As String = "640"
    Public Const Group_MiningBarges As String = "463"
    Public Const Group_MiningDrones As String = "101"
    Public Const Group_ProbeLaunchers As String = "481"
    Public Const Group_ProjectileTurrets As String = "55"
    Public Const Group_RemoteArmorRepairers As String = "325"
    Public Const Group_RemoteHullRepairers As String = "585"
    Public Const Group_ShieldBoosters As String = "40"
    Public Const Group_ShieldTransporters As String = "41"
    Public Const Group_Smartbombs As String = "72"
    Public Const Group_StrategicCruisers As String = "963"

    ' marketGroupIDs (see invMarketGroups)
    Public Const Marketgroup_IceHarvesters As String = "1038"
    Public Const Marketgroup_MiningDrones As String = "158"
    Public Const Marketgroup_MiningLasers As String = "1039"
    Public Const Marketgroup_OrbitalEnergyAmmo As String = "1599"
    Public Const Marketgroup_OrbitalHybridAmmo As String = "1600"
    Public Const Marketgroup_OrbitalProjectileAmmo As String = "1598"
    Public Const Marketgroup_StripMiners As String = "1040"

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
    Private cMetaType As Integer
    Private cMetaLevel As Integer
    Private cRaceID As Integer
	Private cIcon As String

    ' Fitting Details
    Private cSlotType As SlotTypes ' 1=Rig, 2=Low, 4=Mid, 8=High, 16=Subsystem
    Private cSlotNo As Integer
    Private cImplantSlot As Integer
    Private cBoosterSlot As Integer
    Private cVolume As Double
    Private cCapacity As Double
    Private cCPU As Double
    Private cPG As Double
    Private cCalibration As Integer
    Private cCapUsage As Double
    Private cCapUsageRate As Double
    Private cActivationTime As Double
    Private cIsLauncher As Boolean
    Private cIsTurret As Boolean
    Private cIsDrone As Boolean
    Private cIsCharge As Boolean
    Private cIsImplant As Boolean
    Private cIsBooster As Boolean
    Private cIsContainer As Boolean

    ' Skills
    Private cRequiredSkills As New SortedList
    Private cRequiredSkillList As New SortedList

    ' Named Attributes
    Private cChargeSize As Integer

    ' Attributes
    Private cAttributes As New SortedList(Of String, Double)

    ' Charges
    Private cCharges As New ArrayList
    Private cLoadedCharge As ShipModule

    ' Audit Log
    Private cAuditLog As New ArrayList

    ' Module State
    Private cModuleState As ModuleStates = ModuleStates.Active ' Default to Active

    ' Implant Groups
    Private cImplantGroups As New ArrayList

    ' Affected by
    Private cAffects As New ArrayList

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
    Public Property MetaType() As Integer
        Get
            Return cMetaType
        End Get
        Set(ByVal value As Integer)
            cMetaType = value
        End Set
    End Property
    Public Property MetaLevel() As Integer
        Get
            Return cMetaLevel
        End Get
        Set(ByVal value As Integer)
            cMetaLevel = value
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

    ' Fitting Details
    Public Property SlotType() As SlotTypes
        Get
            Return cSlotType
        End Get
        Set(ByVal value As SlotTypes)
            cSlotType = value
        End Set
    End Property
    Public Property SlotNo() As Integer
        Get
            Return cSlotNo
        End Get
        Set(ByVal value As Integer)
            cSlotNo = value
        End Set
    End Property
    Public Property ImplantSlot() As Integer
        Get
            Return cImplantSlot
        End Get
        Set(ByVal value As Integer)
            cImplantSlot = value
        End Set
    End Property
    Public Property BoosterSlot() As Integer
        Get
            Return cBoosterSlot
        End Get
        Set(ByVal value As Integer)
            cBoosterSlot = value
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
    Public Property Capacity() As Double
        Get
            Return cCapacity
        End Get
        Set(ByVal value As Double)
            cCapacity = value
        End Set
    End Property

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
    Public Property Calibration() As Integer
        Get
            Return cCalibration
        End Get
        Set(ByVal value As Integer)
            cCalibration = value
        End Set
    End Property
    Public Property CapUsage() As Double
        Get
            Return cCapUsage
        End Get
        Set(ByVal value As Double)
            cCapUsage = value
        End Set
    End Property
    Public Property CapUsageRate() As Double
        Get
            Return cCapUsageRate
        End Get
        Set(ByVal value As Double)
            cCapUsageRate = value
        End Set
    End Property
    Public Property ActivationTime() As Double
        Get
            Return cActivationTime
        End Get
        Set(ByVal value As Double)
            cActivationTime = value
        End Set
    End Property
    Public Property IsLauncher() As Boolean
        Get
            Return cIsLauncher
        End Get
        Set(ByVal value As Boolean)
            cIsLauncher = value
        End Set
    End Property
    Public Property IsTurret() As Boolean
        Get
            Return cIsTurret
        End Get
        Set(ByVal value As Boolean)
            cIsTurret = value
        End Set
    End Property
    Public Property IsDrone() As Boolean
        Get
            Return cIsDrone
        End Get
        Set(ByVal value As Boolean)
            cIsDrone = value
        End Set
    End Property
    Public Property IsCharge() As Boolean
        Get
            Return cIsCharge
        End Get
        Set(ByVal value As Boolean)
            cIsCharge = value
        End Set
    End Property
    Public Property IsImplant() As Boolean
        Get
            Return cIsImplant
        End Get
        Set(ByVal value As Boolean)
            cIsImplant = value
        End Set
    End Property
    Public Property IsBooster() As Boolean
        Get
            Return cIsBooster
        End Get
        Set(ByVal value As Boolean)
            cIsBooster = value
        End Set
    End Property
    Public Property IsContainer() As Boolean
        Get
            Return cIsContainer
        End Get
        Set(ByVal value As Boolean)
            cIsContainer = value
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

    ' Named Attributes
    Public Property ChargeSize() As Integer
        Get
            Return cChargeSize
        End Get
        Set(ByVal value As Integer)
            cChargeSize = value
        End Set
    End Property

    ' Attributes
    Public Property Attributes() As SortedList(Of String, Double)
        Get
            Return cAttributes
        End Get
        Set(ByVal value As SortedList(Of String, Double))
            cAttributes = value
        End Set
    End Property

    ' Charges
    Public Property Charges() As ArrayList
        Get
            Return cCharges
        End Get
        Set(ByVal value As ArrayList)
            cCharges = value
        End Set
    End Property
    Public Property LoadedCharge() As ShipModule
        Get
            Return cLoadedCharge
        End Get
        Set(ByVal value As ShipModule)
            cLoadedCharge = value
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

    ' Module State
    Public Property ModuleState() As ModuleStates
        Get
            Return cModuleState
        End Get
        Set(ByVal value As ModuleStates)
            cModuleState = value
        End Set
    End Property

    ' Implant Groups
    Public Property ImplantGroups() As ArrayList
        Get
            Return cImplantGroups
        End Get
        Set(ByVal value As ArrayList)
            cImplantGroups = value
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

#End Region

#Region "Cloning"
    Public Function Clone() As ShipModule
        Dim ShipMemoryStream As New MemoryStream(10000)
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(ShipMemoryStream, Me)
        ShipMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newModule As ShipModule = CType(objBinaryFormatter.Deserialize(ShipMemoryStream), ShipModule)
        ShipMemoryStream.Close()
        Return newModule
    End Function
#End Region

#Region "Map Attributes to Properties"
    Public Shared Sub MapModuleAttributes(ByVal newModule As ShipModule)
        Dim attValue As Double = 0
        Dim attributes As New Attributes
        ' Amend for remote effects capacitor use
        If (newModule.ModuleState And 16) = 16 Then
            Select Case newModule.DatabaseGroup
                Case ShipModule.Group_EnergyTransfers
                    newModule.Attributes(attributes.Module_CapacitorNeed) = CDbl(newModule.Attributes(attributes.Module_PowerTransferAmount)) * -1
                Case ShipModule.Group_EnergyVampires
                    newModule.Attributes(attributes.Module_CapacitorNeed) = CDbl(newModule.Attributes(attributes.Module_PowerTransferAmount))
                Case ShipModule.Group_EnergyNeutralizers
                    newModule.Attributes(attributes.Module_CapacitorNeed) = CDbl(newModule.Attributes(attributes.Module_EnergyNeutAmount))
                Case ShipModule.Group_EnergyNeutralizerDrones
                    newModule.Attributes(attributes.Module_CapacitorNeed) = CDbl(newModule.Attributes(attributes.Module_EnergyNeutAmount))
                Case Else
                    newModule.Attributes(attributes.Module_CapacitorNeed) = 0
            End Select
        End If
        ' Parse values
        For Each att As String In newModule.Attributes.Keys
            attValue = CDbl(newModule.Attributes(att))
            Select Case att
                Case attributes.Module_CapacitorNeed
                    newModule.CapUsage = attValue
                Case attributes.Module_PowergridUsage
                    newModule.PG = attValue
                Case attributes.Module_CpuUsage
                    newModule.CPU = attValue
                Case attributes.Module_ActivationTime
                    newModule.ActivationTime = attValue
                Case attributes.Module_CalibrationCost
                    newModule.Calibration = CInt(attValue)
            End Select
        Next
        If newModule.Attributes.ContainsKey(attributes.Module_CapUsageRate) = True Then
            If newModule.Attributes.ContainsKey(attributes.Module_ROF) = True Then
                newModule.Attributes(attributes.Module_CapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(attributes.Module_ROF))
            ElseIf newModule.Attributes.ContainsKey(attributes.Module_EnergyROF) = True Then
                newModule.Attributes(attributes.Module_CapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(attributes.Module_EnergyROF))
            ElseIf newModule.Attributes.ContainsKey(attributes.Module_HybridROF) = True Then
                newModule.Attributes(attributes.Module_CapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(attributes.Module_HybridROF))
            ElseIf newModule.Attributes.ContainsKey(attributes.Module_ProjectileROF) = True Then
                newModule.Attributes(attributes.Module_CapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(attributes.Module_ProjectileROF))
            Else
                newModule.Attributes(attributes.Module_CapUsageRate) = newModule.CapUsage / newModule.ActivationTime
            End If
            newModule.CapUsageRate = CDbl(newModule.Attributes(attributes.Module_CapUsageRate))
        End If
        If newModule.Attributes.ContainsKey(attributes.Module_MiningAmount) = True Then
            Select Case newModule.MarketGroup
                Case ShipModule.Marketgroup_IceHarvesters
                    newModule.Attributes(attributes.Module_TurretIceMiningRate) = CDbl(newModule.Attributes(attributes.Module_MiningAmount)) / CDbl(newModule.Attributes(attributes.Module_ActivationTime))
                Case ShipModule.Marketgroup_MiningLasers, ShipModule.Marketgroup_StripMiners
                    newModule.Attributes(attributes.Module_TurretOreMiningRate) = CDbl(newModule.Attributes(attributes.Module_MiningAmount)) / CDbl(newModule.Attributes(attributes.Module_ActivationTime))
                Case ShipModule.Marketgroup_MiningDrones
                    newModule.Attributes(attributes.Module_DroneOreMiningRate) = CDbl(newModule.Attributes(attributes.Module_MiningAmount)) / CDbl(newModule.Attributes(attributes.Module_ActivationTime))
            End Select
        End If
    End Sub
#End Region

End Class

<Serializable()> Public Class ModuleLists
    Public Shared moduleMetaTypes As New SortedList
    Public Shared moduleMetaGroups As New SortedList
    Public Shared moduleList As New SortedList   ' Key = module ID, Value = ShipModule
    Public Shared moduleListName As New SortedList ' Key = moduleName, Value = ID (for quick name to ID conversions)
End Class



