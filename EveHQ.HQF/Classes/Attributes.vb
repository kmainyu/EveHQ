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

Imports ProtoBuf

<ProtoContract()> <Serializable()> Public Class Attributes

    <ProtoMember(1)> Public Shared AttributeList As New SortedList(Of Integer, Attribute) ' Attribute.ID, Attribute
    <ProtoMember(2)> Public Shared AttributeQuickList As New SortedList(Of Integer, String) ' AttributeID, Attribute.DisplayName

End Class

<ProtoContract()> <Serializable()> Public Class Attribute
    <ProtoMember(1)> Public ID As Integer
    <ProtoMember(2)> Public Name As String
    <ProtoMember(3)> Public DisplayName As String
    <ProtoMember(4)> Public UnitName As String
    <ProtoMember(5)> Public AttributeGroup As String
End Class

Public Enum AttributeEnum

    ' DB attributeIDs (see dgmAttributeTypes)

    ' ship attributes
    ShipAmmoHold = 1573
    ShipArmorEMResistance = 267
    ShipArmorExpResistance = 268
    ShipArmorKinResistance = 269
    ShipArmorThermResistance = 270
    ShipCapRechargeTime = 55
    ShipCloakReactivationDelay = 1034
    ShipCommandCenterHold = 1646
    ShipCpuOutput = 48
    ShipFleetHangar = 912
    ShipFuelBay = 1549
    ShipHighSlots = 14
    ShipHullEMResistance = 113
    ShipHullExpResistance = 111
    ShipHullKinResistance = 109
    ShipHullThermResistance = 110
    ShipMineralHold = 1558
    ShipOreHold = 1556
    ShipPiCommoditiesHold = 1653
    ShipPowergridOutput = 11
    ShipReqSkill1 = 182
    ShipReqSkill1Level = 277
    ShipReqSkill2 = 183
    ShipReqSkill2Level = 278
    ShipReqSkill3 = 184
    ShipReqSkill3Level = 279
    ShipRigSize = 1547
    ShipShieldEMResistance = 271
    ShipShieldExpResistance = 272
    ShipShieldKinResistance = 273
    ShipShieldRechargeTime = 479
    ShipShieldThermResistance = 274
    ShipTurretHardpoints = 102
    ShipWarpSpeed = 1281

    ' module attributes
    ModuleActivationTime = 73
    ModuleAoERadius = 99
    ModuleArmorBoostedRepairMultiplier = 1886
    ModuleArmorEMResistance = 267
    ModuleArmorExpResistance = 268
    ModuleArmorHPRepaired = 84
    ModuleArmorKinResistance = 269
    ModuleArmorThermResistance = 270
    ModuleBaseEMDamage = 114
    ModuleBaseExpDamage = 116
    ModuleBaseKinDamage = 117
    ModuleBaseThermDamage = 118
    ModuleBoosterSlot = 1087
    ModuleCalibrationCost = 1153
    ModuleCanFitShipGroup1 = 1298
    ModuleCanFitShipGroup2 = 1299
    ModuleCanFitShipGroup3 = 1300
    ModuleCanFitShipGroup4 = 1301
    ModuleCanFitShipGroup5 = 1872
    ModuleCanFitShipGroup6 = 1879
    ModuleCanFitShipGroup7 = 1880
    ModuleCanFitShipGroup8 = 1881
    ModuleCanFitShipType1 = 1302
    ModuleCanFitShipType2 = 1303
    ModuleCanFitShipType3 = 1304
    ModuleCanFitShipType4 = 1305
    ModuleCapacitorNeed = 6
    ModuleChargeGroup1 = 604
    ModuleChargeGroup2 = 605
    ModuleChargeGroup3 = 606
    ModuleChargeGroup4 = 609
    ModuleChargeGroup5 = 610
    ModuleChargeSize = 128
    ModuleCommandBonus = 833
    ModuleCommandBonusECM = 1320
    ModuleConsumptionType = 713
    ModuleCpuUsage = 50
    ModuleDamageMod = 64
    ModuleDroneBandwidthNeeded = 1272
    ModuleECMBurstRadius = 142
    ModuleEnergyNeutAmount = 97
    ModuleEnergyNeutRange = 98
    ModuleExplosionRadius = 654
    ModuleExplosionVelocity = 653
    ModuleFalloff = 158
    ModuleFitsToShipType = 1380
    ModuleHeatDamage = 1211
    ModuleHighSlotModifier = 1374
    ModuleHullEMResistance = 974
    ModuleHullExpResistance = 975
    ModuleHullHPRepaired = 83
    ModuleHullKinResistance = 976
    ModuleHullThermResistance = 977
    ModuleImplantSlot = 331
    ModuleLowSlotModifier = 1376
    ModuleMaxFlightTime = 281
    ModuleMaxGroupActive = 763
    ModuleMaxGroupFitted = 1544
    ModuleMaxVelocity = 37
    ModuleMetaLevel = 633
    ModuleMidSlotModifier = 1375
    ModuleMiningAmount = 77
    ModuleMissileDamageMod = 212
    ModuleMissileRof = 506
    ModuleMissileTypeID = 507
    ModuleOptimalRange = 54
    ModulePowergridUsage = 30
    ModulePowerTransferAmount = 90
    ModulePowerTransferRange = 91
    ModuleReactivationDelay = 669
    ModuleReloadTime = 1795
    ModuleReqSkill1 = 182
    ModuleReqSkill1Level = 277
    ModuleReqSkill2 = 183
    ModuleReqSkill2Level = 278
    ModuleReqSkill3 = 184
    ModuleReqSkill3Level = 279
    ModuleRigSize = 1547
    ModuleRof = 51
    ModuleRofBonus = 204
    ModuleShieldEMResistance = 271
    ModuleShieldExpResistance = 272
    ModuleShieldHPRepaired = 68
    ModuleShieldKinResistance = 273
    ModuleShieldThermResistance = 274
    ModuleShieldTransferRange = 87
    ModuleSubsystemSlot = 1366
    ModuleTechLevel = 422
    ModuleTrackingSpeed = 160
    ModuleWarpScrambleRange = 103

    ' skill attributes
    SkillAgilityBonus = 151
    SkillArmorHpBonus = 335
    SkillArmorHpBonus2 = 1083
    SkillMiningAmountBonus = 434
    SkillScanResBonus = 566
    SkillShieldHpBonus = 337
    SkillTargetRangeBonus = 309

    ' custom attributeIDs (see HQF/Resources/Attributes.csv)
    ' ship attributes
    ShipArmorRepair = 10066
    ShipArmorTank = 10060
    ShipDPS = 10029
    ShipDroneDPS = 10027
    ShipDroneOreMiningAmount = 10035
    ShipDroneOreMiningRate = 10044
    ShipDroneTransferAmount = 10076
    ShipDroneTransferRate = 10078
    ShipDroneVolleyDamage = 10023
    ShipEMDamage = 10055
    ShipEmDPS = 10070
    ShipExpDamage = 10056
    ShipExpDPS = 10071
    ShipFighterControl = 10006
    ShipGasMiningAmount = 10081
    ShipGasMiningRate = 10083
    ShipHullRepair = 10067
    ShipHullTank = 10061
    ShipIceMiningAmount = 10036
    ShipIceMiningRate = 10048
    ShipKinDamage = 10057
    ShipKinDPS = 10072
    ShipMaxGangLinks = 10063
    ShipMissileDPS = 10025
    ShipMissileVolleyDamage = 10021
    ShipModuleTransferAmount = 10075
    ShipModuleTransferRate = 10077
    ShipOreMiningAmount = 10033
    ShipOreMiningRate = 10047
    ShipRepairTotal = 10068
    ShipShieldRepair = 10065
    ShipShieldTankActive = 10059
    ShipShieldTankPassive = 10069
    ShipSmartbombDPS = 10026
    ShipSmartbombVolleyDamage = 10022
    ShipTankMax = 10062
    ShipThermDamage = 10058
    ShipThermDPS = 10073
    ShipTransferAmount = 10079
    ShipTransferRate = 10080
    ShipTurretDPS = 10024
    ShipTurretIceMiningAmount = 10037
    ShipTurretIceMiningRate = 10045
    ShipTurretOreMiningAmount = 10034
    ShipTurretOreMiningRate = 10043
    ShipTurretVolleyDamage = 10020
    ShipVolleyDamage = 10028

    ' module attributes
    ModuleBaseDamage = 10017
    ModuleCapacity = 10004
    ModuleCapUsageRate = 10032
    ModuleDPS = 10019
    ModuleDroneOreMiningRate = 10040
    ModuleEMDamage = 10051
    ModuleEnergyDmgMod = 10014
    ModuleEnergyRof = 10011
    ModuleExpDamage = 10052
    ModuleHybridDmgMod = 10015
    ModuleHybridRof = 10012
    ModuleKinDamage = 10053
    ModuleLoadedCharge = 10030
    ModuleMass = 10002
    ModuleProjectileDmgMod = 10016
    ModuleProjectileRof = 10013
    ModuleThermDamage = 10054
    ModuleTransferRate = 10074
    ModuleTurretGasMiningRate = 10082
    ModuleTurretIceMiningRate = 10041
    ModuleTurretOreMiningRate = 10039
    ModuleVolleyDamage = 10018

End Enum

Public Enum EffectEnum
    ' DB effectIDs (see dgmEffects)
    EffectHighSlot = 12
    EffectLauncherFitted = 40
    EffectLowSlot = 11
    EffectMidSlot = 13
    EffectProjectileFired = 34
    EffectRigSlot = 2663
    EffectSubsystemSlot = 3772
    EffectTargetAttack = 10
    EffectTurretFitted = 42
End Enum

Public Enum UnitEnum
    ' DB unitIDs (see eveUnits)
    UnitInverseAbsolutePercent = 108
    UnitInverseModifierPercent = 111
    UnitMilliseconds = 101
    UnitModifierPercent = 109
End Enum