'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports ProtoBuf
Imports System.Runtime.Serialization

<ProtoContract()>
<Serializable()>
Public Class ShipModule
    
#Region "Properties"

    ' Name and Classification
    <ProtoMember(1)> Public Property Name() As String
    <ProtoMember(2)> Public Property ID() As Integer
    <ProtoMember(3)> Public Property Description() As String
    <ProtoMember(4)> Public Property MarketGroup() As Integer
    <ProtoMember(5)> Public Property DatabaseGroup() As Integer
    <ProtoMember(6)> Public Property DatabaseCategory() As Integer
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
    <ProtoMember(18)> Public Property Cpu() As Double
    <ProtoMember(19)> Public Property Pg() As Double
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
    <ProtoMember(34)> Public Property Attributes() As New SortedList(Of Integer, Double) ' AttributeID, AttributeValue

    ' Charges
    <ProtoMember(35)> Public Property Charges() As New List(Of Integer)

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
        Using shipMemoryStream As New MemoryStream(10000)
            Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
            objBinaryFormatter.Serialize(shipMemoryStream, Me)
            shipMemoryStream.Seek(0, SeekOrigin.Begin)
            Return CType(objBinaryFormatter.Deserialize(shipMemoryStream), ShipModule)
        End Using
    End Function
#End Region

#Region "Map Attributes to Properties"
    Public Shared Sub MapModuleAttributes(ByVal newModule As ShipModule)
        Dim attValue As Double
        ' Amend for remote effects capacitor use
        If (newModule.ModuleState And 16) = 16 Then
            Select Case newModule.DatabaseGroup
                Case ModuleEnum.GroupEnergyTransfers
                    newModule.Attributes(AttributeEnum.ModuleCapacitorNeed) = CDbl(newModule.Attributes(AttributeEnum.ModulePowerTransferAmount)) * -1
                Case ModuleEnum.GroupEnergyVampires
                    newModule.Attributes(AttributeEnum.ModuleCapacitorNeed) = CDbl(newModule.Attributes(AttributeEnum.ModulePowerTransferAmount))
                Case ModuleEnum.GroupEnergyNeutralizers
                    newModule.Attributes(AttributeEnum.ModuleCapacitorNeed) = CDbl(newModule.Attributes(AttributeEnum.ModuleEnergyNeutAmount))
                Case ModuleEnum.GroupEnergyNeutralizerDrones
                    newModule.Attributes(AttributeEnum.ModuleCapacitorNeed) = CDbl(newModule.Attributes(AttributeEnum.ModuleEnergyNeutAmount))
                Case Else
                    newModule.Attributes(AttributeEnum.ModuleCapacitorNeed) = 0
            End Select
        End If
        ' Parse values
        For Each att As Integer In newModule.Attributes.Keys
            attValue = CDbl(newModule.Attributes(att))
            Select Case att
                Case AttributeEnum.ModuleCapacitorNeed
                    newModule.CapUsage = attValue
                Case AttributeEnum.ModulePowergridUsage
                    newModule.PG = attValue
                Case AttributeEnum.ModuleCpuUsage
                    newModule.CPU = attValue
                Case AttributeEnum.ModuleActivationTime
                    newModule.ActivationTime = attValue
                Case AttributeEnum.ModuleReactivationDelay
                    newModule.ReactivationDelay = attValue
                Case AttributeEnum.ModuleCalibrationCost
                    newModule.Calibration = CInt(attValue)
            End Select
        Next
        If newModule.Attributes.ContainsKey(AttributeEnum.ModuleCapUsageRate) = True Then
            If newModule.Attributes.ContainsKey(AttributeEnum.ModuleROF) = True Then
                newModule.Attributes(AttributeEnum.ModuleCapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(AttributeEnum.ModuleROF))
            ElseIf newModule.Attributes.ContainsKey(AttributeEnum.ModuleEnergyROF) = True Then
                newModule.Attributes(AttributeEnum.ModuleCapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(AttributeEnum.ModuleEnergyROF))
            ElseIf newModule.Attributes.ContainsKey(AttributeEnum.ModuleHybridROF) = True Then
                newModule.Attributes(AttributeEnum.ModuleCapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(AttributeEnum.ModuleHybridROF))
            ElseIf newModule.Attributes.ContainsKey(AttributeEnum.ModuleProjectileROF) = True Then
                newModule.Attributes(AttributeEnum.ModuleCapUsageRate) = newModule.CapUsage / CDbl(newModule.Attributes(AttributeEnum.ModuleProjectileROF))
            Else
                newModule.Attributes(AttributeEnum.ModuleCapUsageRate) = newModule.CapUsage / (newModule.ActivationTime + newModule.ReactivationDelay)
            End If
            newModule.CapUsageRate = CDbl(newModule.Attributes(AttributeEnum.ModuleCapUsageRate))
        End If
        If newModule.Attributes.ContainsKey(AttributeEnum.ModuleMiningAmount) = True Then
            Select Case newModule.MarketGroup
                Case ModuleEnum.MarketgroupIceHarvesters
                    newModule.Attributes(AttributeEnum.ModuleTurretIceMiningRate) = CDbl(newModule.Attributes(AttributeEnum.ModuleMiningAmount)) / CDbl(newModule.Attributes(AttributeEnum.ModuleActivationTime))
                Case ModuleEnum.MarketgroupMiningLasers, ModuleEnum.MarketgroupStripMiners
                    newModule.Attributes(AttributeEnum.ModuleTurretOreMiningRate) = CDbl(newModule.Attributes(AttributeEnum.ModuleMiningAmount)) / CDbl(newModule.Attributes(AttributeEnum.ModuleActivationTime))
                Case ModuleEnum.MarketgroupMiningDrones
                    newModule.Attributes(AttributeEnum.ModuleDroneOreMiningRate) = CDbl(newModule.Attributes(AttributeEnum.ModuleMiningAmount)) / CDbl(newModule.Attributes(AttributeEnum.ModuleActivationTime))
            End Select
        End If
    End Sub
#End Region

    Public Function GetChargeList() As SortedList(Of String, String)

        ' Get the charge group and item data
        Dim chargeGroups As New ArrayList
        Dim chargeGroupData() As String
        Dim chargeItems As New SortedList(Of String, String)
        Dim groupName As String = ""
        For Each chargeGroup As String In HQF.Charges.ChargeGroups.Values
            chargeGroupData = chargeGroup.Split("_".ToCharArray)
            If Charges.Contains(CInt(chargeGroupData(1))) = True Then
                If Market.MarketGroupList.ContainsKey(chargeGroupData(0)) = True Then
                    Select Case Market.MarketGroupList.Item(chargeGroupData(0))
                        Case "Small", "Medium", "Large", "Extra Large"
                            Dim pathLine As String = Market.MarketGroupPath(chargeGroupData(0))
                            Dim paths() As String = pathLine.Split("\".ToCharArray)
                            groupName = paths(paths.GetUpperBound(0) - 1)
                        Case Else
                            groupName = Market.MarketGroupList.Item(chargeGroupData(0))
                    End Select
                End If
                If chargeGroups.Contains(groupName) = False Then
                    chargeGroups.Add(groupName)
                End If
                If IsTurret Then
                    If ChargeSize = CInt(chargeGroupData(3)) Then
                        chargeItems.Add(chargeGroupData(2), groupName)
                    End If
                Else
                    chargeItems.Add(chargeGroupData(2), groupName)
                End If
            End If
        Next

        Return chargeItems

    End Function

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
    <ProtoMember(1)> Public Shared ModuleMetaTypes As New SortedList(Of Integer, Integer) ' Key = typeID, Value = parentTypeID
    <ProtoMember(2)> Public Shared ModuleMetaGroups As New SortedList(Of Integer, Integer) ' Key = typeID, Value = metaGroupID
    <ProtoMember(3)> Public Shared ModuleList As New SortedList(Of Integer, ShipModule)   ' Key = module ID, Value = ShipModule
    <ProtoMember(4)> Public Shared ModuleListName As New Dictionary(Of String, Integer) ' Key = moduleName, Value = ID (for quick name to ID conversions)
    <ProtoMember(5)> Public Shared TypeGroups As New SortedList(Of Integer, String) ' groupID, groupName
    <ProtoMember(6)> Public Shared TypeCats As New SortedList(Of Integer, String) ' catID, catName
    <ProtoMember(7)> Public Shared GroupCats As New SortedList(Of Integer, Integer) ' groupID, catID
End Class

Public Enum ModuleEnum

    ' itemIDs (see invTypes)
    ItemBastionModuleI = 33400
    ItemCommandProcessorI = 11014
    ItemLegionCovertReconfiguration = 30120
    ItemLegionWarfareProcessor = 29967
    ItemLokiCovertReconfiguration = 30135
    ItemLokiWarfareProcessor = 29977
    ItemNaniteRepairPaste = 28668
    ItemProteusCovertReconfiguration = 30130
    ItemProteusWarfareProcessor = 29982
    ItemSiegeModuleI = 20280
    ItemSiegeModuleT2 = 4292
    ItemTenguCovertReconfiguration = 30125
    ItemTenguWarfareProcessor = 29972
    ItemTriageModuleI = 27951
    ItemTriageModuleT2 = 4294

    ' categoryIDs (see invCategories)
    CategoryCelestials = 2
    CategoryCharges = 8
    CategoryDrones = 18
    CategoryImplants = 20
    CategorySubsystems = 32

    ' groupIDs (see invGroups)
    GroupArmorRepairers = 62
    GroupArmorResistShiftHardener = 1150
    GroupBlockadeRunners = 1202
    GroupBombLaunchers = 862
    GroupBoosters = 303
    GroupCapBoosters = 76
    GroupCloakingDevices = 330
    GroupCynosuralFields = 658
    GroupDamageControls = 60
    GroupDeepSpaceTransports = 380
    GroupDNAMutators = 304
    GroupEnergyNeutralizers = 71
    GroupEnergyNeutralizerDrones = 544
    GroupEnergyTransfers = 67
    GroupEnergyTurrets = 53
    GroupEnergyVampires = 68
    GroupExhumers = 543
    GroupFreighters = 513
    GroupFueledArmorRepairers = 1199
    GroupFueledShieldBoosters = 1156
    GroupGangLinks = 316
    GroupHullRepairers = 63
    GroupHybridTurrets = 74
    GroupIndustrialCommandShips = 941
    GroupIndustrials = 28
    GroupJumpFreighters = 902
    GroupLogisticDrones = 640
    GroupMiningBarges = 463
    GroupMiningDrones = 101
    GroupProbeLaunchers = 481
    GroupProjectileTurrets = 55
    GroupRemoteArmorRepairers = 325
    GroupRemoteHullRepairers = 585
    GroupShieldBoosters = 40
    GroupShieldTransporters = 41
    GroupSmartbombs = 72
    GroupStrategicCruisers = 963

    ' marketGroupIDs (see invMarketGroups)
    MarketgroupGasHarvesters = 1037
    MarketgroupIceHarvesters = 1038
    MarketgroupMiningDrones = 158
    MarketgroupMiningLasers = 1039
    MarketgroupOrbitalEnergyAmmo = 1599
    MarketgroupOrbitalHybridAmmo = 1600
    MarketgroupOrbitalProjectileAmmo = 1598
    MarketgroupOreCapitalIndustrials = 1048
    MarketgroupStripMiners = 1040

End Enum