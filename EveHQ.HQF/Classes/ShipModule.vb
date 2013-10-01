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
Imports ProtoBuf

<ProtoContract()>
<Serializable()>
Public Class ShipModule

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
    Public Const Marketgroup_GasHarvesters As String = "1037"
    Public Const Marketgroup_IceHarvesters As String = "1038"
    Public Const Marketgroup_MiningDrones As String = "158"
    Public Const Marketgroup_MiningLasers As String = "1039"
    Public Const Marketgroup_OrbitalEnergyAmmo As String = "1599"
    Public Const Marketgroup_OrbitalHybridAmmo As String = "1600"
    Public Const Marketgroup_OrbitalProjectileAmmo As String = "1598"
    Public Const Marketgroup_ORECapitalIndustrials As String = "1048"
    Public Const Marketgroup_StripMiners As String = "1040"

#End Region

#Region "Properties"

    ' Name and Classification
    <ProtoMember(1)> Public Property Name() As String
    <ProtoMember(2)> Public Property ID() As String
    <ProtoMember(3)> Public Property Description() As String
    <ProtoMember(4)> Public Property MarketGroup() As String
    <ProtoMember(5)> Public Property DatabaseGroup() As String
    <ProtoMember(6)> Public Property DatabaseCategory() As String
    <ProtoMember(7)> Public Property BasePrice() As Double
    <ProtoMember(8)> Public Property MarketPrice() As Double
    <ProtoMember(9)> Public Property MetaType() As Integer
    <ProtoMember(10)> Public Property MetaLevel() As Integer
    <ProtoMember(11)> Public Property Icon() As String

    ' Fitting Details
    <ProtoMember(12)> Public Property SlotType() As SlotTypes
    <ProtoMember(13)> Public Property SlotNo() As Integer
    <ProtoMember(14)> Public Property ImplantSlot() As Integer
    <ProtoMember(15)> Public Property BoosterSlot() As Integer
    <ProtoMember(16)> Public Property Volume() As Double
    <ProtoMember(17)> Public Property Capacity() As Double
    <ProtoMember(18)> Public Property CPU() As Double
    <ProtoMember(19)> Public Property PG() As Double
    <ProtoMember(20)> Public Property Calibration() As Integer
    <ProtoMember(21)> Public Property CapUsage() As Double
    <ProtoMember(22)> Public Property CapUsageRate() As Double
    <ProtoMember(23)> Public Property ActivationTime() As Double
    <ProtoMember(24)> Public Property ReactivationDelay() As Double
    <ProtoMember(25)> Public Property IsLauncher() As Boolean
    <ProtoMember(26)> Public Property IsTurret() As Boolean
    <ProtoMember(27)> Public Property IsDrone() As Boolean
    <ProtoMember(28)> Public Property IsCharge() As Boolean
    <ProtoMember(29)> Public Property IsImplant() As Boolean
    <ProtoMember(30)> Public Property IsBooster() As Boolean
    <ProtoMember(31)> Public Property IsContainer() As Boolean

    ' Skills
    <ProtoMember(32)> Public Property RequiredSkills() As New SortedList(Of String, ItemSkills)

    ' Named Attributes
    <ProtoMember(33)> Public Property ChargeSize() As Integer

    ' Attributes
    <ProtoMember(34)> Public Property Attributes() As New SortedList(Of String, Double)

    ' Charges
    <ProtoMember(35)> Public Property Charges() As New List(Of String)

    <ProtoMember(36)> Public Property LoadedCharge() As ShipModule

    'Audit Log
    <ProtoMember(37)> Public Property AuditLog() As New List(Of String)

    ' Module State
    <ProtoMember(38)> Public Property ModuleState() As ModuleStates = ModuleStates.Active

    ' Implant Groups
    <ProtoMember(39)> Public Property ImplantGroups() As New List(Of String)

    ' Affected by
    <ProtoMember(40)> Public Property Affects() As New List(Of String)

#End Region

#Region "Cloning"
    Public Function Clone() As ShipModule
        Dim shipMemoryStream As New MemoryStream(10000)
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(shipMemoryStream, Me)
        shipMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newModule As ShipModule = CType(objBinaryFormatter.Deserialize(shipMemoryStream), ShipModule)
        shipMemoryStream.Close()
        Return newModule
    End Function
#End Region

#Region "Map Attributes to Properties"
    Public Shared Sub MapModuleAttributes(ByVal newModule As ShipModule)
        Dim attValue As Double 
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
                Case attributes.Module_ReactivationDelay
                    newModule.ReactivationDelay = attValue
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
                newModule.Attributes(attributes.Module_CapUsageRate) = newModule.CapUsage / (newModule.ActivationTime + newModule.ReactivationDelay)
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

Public Enum MetaTypes As Integer
    Tech1 = 1
    Tech2 = 2
    Storyline = 4
    Faction = 8
    Officer = 16
    Deadspace = 32
    Tech3 = 8192
End Enum

<ProtoContract()>
<Serializable()>
Public Class ModuleLists
    <ProtoMember(1)> Public Shared ModuleMetaTypes As New SortedList(Of String, String)
    <ProtoMember(2)> Public Shared ModuleMetaGroups As New SortedList(Of String, String)
    <ProtoMember(3)> Public Shared ModuleList As New SortedList(Of String, ShipModule)   ' Key = module ID, Value = ShipModule
    <ProtoMember(4)> Public Shared ModuleListName As New SortedList(Of String, String) ' Key = moduleName, Value = ID (for quick name to ID conversions)
    <ProtoMember(5)> Public Shared TypeGroups As New SortedList(Of Integer, String) ' groupID, groupName
    <ProtoMember(6)> Public Shared TypeCats As New SortedList(Of Integer, String) ' catID, catName
    <ProtoMember(7)> Public Shared GroupCats As New SortedList(Of Integer, Integer) ' groupID, catID
End Class



