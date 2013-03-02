﻿' ========================================================================
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
<Serializable()> Public Class Attributes

    Public Shared AttributeList As New SortedList
    Public Shared AttributeQuickList As New SortedList

#Region "Constants"
    ' DB attributeIDs (see dgmAttributeTypes)
    ' ship attributes
    Public Const Ship_ArmorEMResistance As String = "267"
    Public Const Ship_ArmorExpResistance As String = "268"
    Public Const Ship_ArmorKinResistance As String = "269"
    Public Const Ship_ArmorThermResistance As String = "270"
    Public Const Ship_CapRechargeTime As String = "55"
    Public Const Ship_CloakReactivationDelay As String = "1034"
    Public Const Ship_CpuOutput As String = "48"
    Public Const Ship_HullEMResistance As String = "113"
    Public Const Ship_HullExpResistance As String = "111"
    Public Const Ship_HullKinResistance As String = "109"
    Public Const Ship_HullThermResistance As String = "110"
    Public Const Ship_PowergridOutput As String = "11"
    Public Const Ship_ReqSkill1 As String = "182"
    Public Const Ship_ReqSkill1Level As String = "277"
    Public Const Ship_ReqSkill2 As String = "183"
    Public Const Ship_ReqSkill2Level As String = "278"
    Public Const Ship_ReqSkill3 As String = "184"
    Public Const Ship_ReqSkill3Level As String = "279"
    Public Const Ship_RigSize As String = "1547"
    Public Const Ship_ShieldEMResistance As String = "271"
    Public Const Ship_ShieldExpResistance As String = "272"
    Public Const Ship_ShieldKinResistance As String = "273"
    Public Const Ship_ShieldRechargeTime As String = "479"
    Public Const Ship_ShieldThermResistance As String = "274"
    Public Const Ship_UpgradeHardpoints As String = "1154"
    Public Const Ship_WarpSpeed As String = "1281"
    ' module attributes
    Public Const Module_ActivationTime As String = "73"
    Public Const Module_AoERadius As String = "99"
    Public Const Module_ArmorBoostedRepairMultiplier As String = "1886"
    Public Const Module_ArmorEMResistance As String = "267"
    Public Const Module_ArmorExpResistance As String = "268"
    Public Const Module_ArmorHPRepaired As String = "84"
    Public Const Module_ArmorKinResistance As String = "269"
    Public Const Module_ArmorThermResistance As String = "270"
    Public Const Module_BaseEMDamage As String = "114"
    Public Const Module_BaseExpDamage As String = "116"
    Public Const Module_BaseKinDamage As String = "117"
    Public Const Module_BaseThermDamage As String = "118"
    Public Const Module_BoosterSlot As String = "1087"
    Public Const Module_CalibrationCost As String = "1153"
    Public Const Module_CanFitShipGroup1 As String = "1298"
    Public Const Module_CanFitShipGroup2 As String = "1299"
    Public Const Module_CanFitShipGroup3 As String = "1300"
    Public Const Module_CanFitShipGroup4 As String = "1301"
    Public Const Module_CanFitShipGroup5 As String = "1872"
    Public Const Module_CanFitShipGroup6 As String = "1879"
    Public Const Module_CanFitShipGroup7 As String = "1880"
    Public Const Module_CanFitShipGroup8 As String = "1881"
    Public Const Module_CanFitShipType1 As String = "1302"
    Public Const Module_CanFitShipType2 As String = "1303"
    Public Const Module_CanFitShipType3 As String = "1304"
    Public Const Module_CanFitShipType4 As String = "1305"
    Public Const Module_CapacitorNeed As String = "6"
    Public Const Module_ChargeGroup1 As String = "604"
    Public Const Module_ChargeGroup2 As String = "605"
    Public Const Module_ChargeGroup3 As String = "606"
    Public Const Module_ChargeGroup4 As String = "609"
    Public Const Module_ChargeGroup5 As String = "610"
    Public Const Module_ChargeSize As String = "128"
    Public Const Module_CommandBonus As String = "833"
    Public Const Module_CommandBonusECM As String = "1320"
    Public Const Module_ConsumptionType As String = "713"
    Public Const Module_CpuUsage As String = "50"
    Public Const Module_DamageMod As String = "64"
    Public Const Module_DroneBandwidthNeeded As String = "1272"
    Public Const Module_ECMBurstRadius As String = "142"
    Public Const Module_EnergyNeutAmount As String = "97"
    Public Const Module_EnergyNeutRange As String = "98"
    Public Const Module_ExplosionRadius As String = "654"
    Public Const Module_ExplosionVelocity As String = "653"
    Public Const Module_Falloff As String = "158"
    Public Const Module_FitsToShipType As String = "1380"
    Public Const Module_HeatDamage As String = "1211"
    Public Const Module_HighSlotModifier As String = "1374"
    Public Const Module_HullEMResistance As String = "974"
    Public Const Module_HullExpResistance As String = "975"
    Public Const Module_HullHPRepaired As String = "83"
    Public Const Module_HullKinResistance As String = "976"
    Public Const Module_HullThermResistance As String = "977"
    Public Const Module_ImplantSlot As String = "331"
    Public Const Module_LowSlotModifier As String = "1376"
    Public Const Module_MaxFlightTime As String = "281"
    Public Const Module_MaxGroupActive As String = "763"
    Public Const Module_MaxGroupFitted As String = "1544"
    Public Const Module_MaxVelocity As String = "37"
    Public Const Module_MetaLevel As String = "633"
    Public Const Module_MidSlotModifier As String = "1375"
    Public Const Module_MiningAmount As String = "77"
    Public Const Module_MissileDamageMod As String = "212"
    Public Const Module_MissileROF As String = "506"
    Public Const Module_MissileTypeID As String = "507"
    Public Const Module_OptimalRange As String = "54"
    Public Const Module_PowergridUsage As String = "30"
    Public Const Module_PowerTransferAmount As String = "90"
    Public Const Module_PowerTransferRange As String = "91"
    Public Const Module_ReactivationDelay As String = "669"
    Public Const Module_ReqSkill1 As String = "182"
    Public Const Module_ReqSkill1Level As String = "277"
    Public Const Module_ReqSkill2 As String = "183"
    Public Const Module_ReqSkill2Level As String = "278"
    Public Const Module_ReqSkill3 As String = "184"
    Public Const Module_ReqSkill3Level As String = "279"
    Public Const Module_RigSize As String = "1547"
    Public Const Module_ROF As String = "51"
    Public Const Module_ROFBonus As String = "204"
    Public Const Module_ShieldEMResistance As String = "271"
    Public Const Module_ShieldExpResistance As String = "272"
    Public Const Module_ShieldHPRepaired As String = "68"
    Public Const Module_ShieldKinResistance As String = "273"
    Public Const Module_ShieldThermResistance As String = "274"
    Public Const Module_ShieldTransferRange As String = "87"
    Public Const Module_SubsystemSlot As String = "1366"
    Public Const Module_TechLevel As String = "422"
    Public Const Module_TrackingSpeed As String = "160"
    Public Const Module_WarpScrambleRange As String = "103"

    ' custom attributeIDs (see HQF/Resources/Attributes.csv)
    ' ship attributes
    Public Const Ship_ArmorRepair As String = "10066"
    Public Const Ship_ArmorTank As String = "10060"
    Public Const Ship_DPS As String = "10029"
    Public Const Ship_DroneDPS As String = "10027"
    Public Const Ship_DroneOreMiningAmount As String = "10035"
    Public Const Ship_DroneOreMiningRate As String = "10044"
    Public Const Ship_DroneVolleyDamage As String = "10023"
    Public Const Ship_EMDamage As String = "10055"
    Public Const Ship_EMDPS As String = "10070"
    Public Const Ship_ExpDamage As String = "10056"
    Public Const Ship_ExpDPS As String = "10071"
    Public Const Ship_FighterControl As String = "10006"
    Public Const Ship_HullRepair As String = "10067"
    Public Const Ship_HullTank As String = "10061"
    Public Const Ship_IceMiningAmount As String = "10036"
    Public Const Ship_IceMiningRate As String = "10048"
    Public Const Ship_KinDamage As String = "10057"
    Public Const Ship_KinDPS As String = "10072"
    Public Const Ship_MaxGangLinks As String = "10063"
    Public Const Ship_MissileDPS As String = "10025"
    Public Const Ship_MissileVolleyDamage As String = "10021"
    Public Const Ship_OreMiningAmount As String = "10033"
    Public Const Ship_OreMiningRate As String = "10047"
    Public Const Ship_RepairTotal As String = "10068"
    Public Const Ship_ShieldRepair As String = "10065"
    Public Const Ship_ShieldTankActive As String = "10059"
    Public Const Ship_ShieldTankPassive As String = "10069"
    Public Const Ship_SmartbombDPS As String = "10026"
    Public Const Ship_SmartbombVolleyDamage As String = "10022"
    Public Const Ship_TankMax As String = "10062"
    Public Const Ship_ThermDamage As String = "10058"
    Public Const Ship_ThermDPS As String = "10073"
    Public Const Ship_TurretDPS As String = "10024"
    Public Const Ship_TurretIceMiningAmount As String = "10037"
    Public Const Ship_TurretIceMiningRate As String = "10045"
    Public Const Ship_TurretOreMiningAmount As String = "10034"
    Public Const Ship_TurretOreMiningRate As String = "10043"
    Public Const Ship_TurretVolleyDamage As String = "10020"
    Public Const Ship_VolleyDamage As String = "10028"
    ' module attributes
    Public Const Module_BaseDamage As String = "10017"
    Public Const Module_Capacity As String = "10004"
    Public Const Module_CapUsageRate As String = "10032"
    Public Const Module_DPS As String = "10019"
    Public Const Module_DroneOreMiningRate As String = "10040"
    Public Const Module_EMDamage As String = "10051"
    Public Const Module_EnergyDmgMod As String = "10014"
    Public Const Module_EnergyROF As String = "10011"
    Public Const Module_ExpDamage As String = "10052"
    Public Const Module_HybridDmgMod As String = "10015"
    Public Const Module_HybridROF As String = "10012"
    Public Const Module_KinDamage As String = "10053"
    Public Const Module_LoadedCharge As String = "10030"
    Public Const Module_Mass As String = "10002"
    Public Const Module_ProjectileDmgMod As String = "10016"
    Public Const Module_ProjectileROF As String = "10013"
    Public Const Module_ThermDamage As String = "10054"
    Public Const Module_TurretIceMiningRate As String = "10041"
    Public Const Module_TurretOreMiningRate As String = "10039"
    Public Const Module_VolleyDamage As String = "10018"

    ' DB effectIDs (see dgmEffects)
    Public Const Effect_HighSlot As String = "12"
    Public Const Effect_LauncherFitted As String = "40"
    Public Const Effect_LowSlot As String = "11"
    Public Const Effect_MidSlot As String = "13"
    Public Const Effect_ProjectileFired As String = "34"
    Public Const Effect_RigSlot As String = "2663"
    Public Const Effect_SubsystemSlot As String = "3772"
    Public Const Effect_TargetAttack As String = "10"
    Public Const Effect_TurretFitted As String = "42"

    ' DB unitIDs (see eveUnits)
    Public Const Unit_InverseAbsolutePercent As String = "108"
    Public Const Unit_InverseModifierPercent As String = "111"
    Public Const Unit_Milliseconds As String = "101"
    Public Const Unit_ModifierPercent As String = "109"

#End Region

End Class

<Serializable()> Public Class Attribute
    Public ID As String
    Public Name As String
    Public DisplayName As String
    Public UnitName As String
    Public AttributeGroup As String
End Class
