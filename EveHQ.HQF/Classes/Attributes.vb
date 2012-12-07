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
<Serializable()> Public Class Attributes

    Public Shared AttributeList As New SortedList
    Public Shared AttributeQuickList As New SortedList

#Region "Constants"
    ' DB attributeIDs (see dgmAttributeTypes)
    ' ship attributes
    Public Const Ship_CpuOutput As String = "48"
    Public Const Ship_PowergridOutput As String = "11"
    ' module attributes
    Public Const Module_ActivationTime As String = "73"
    Public Const Module_BaseEMDamage As String = "114"
    Public Const Module_BaseExpDamage As String = "116"
    Public Const Module_BaseKinDamage As String = "117"
    Public Const Module_BaseThermDamage As String = "118"
    Public Const Module_CpuUsage As String = "50"
    Public Const Module_DamageMod As String = "64"
    Public Const Module_DroneBandwidthNeeded As String = "1272"
    Public Const Module_MaxFlightTime As String = "281"
    Public Const Module_MaxGroupActive As String = "763"
    Public Const Module_MaxVelocity As String = "37"
    Public Const Module_MiningAmount As String = "77"
    Public Const Module_MissileDamageMod As String = "212"
    Public Const Module_MissileROF As String = "506"
    Public Const Module_OptimalRange As String = "54"
    Public Const Module_PowergridUsage As String = "30"
    Public Const Module_ROF As String = "51"

    ' custom attributeIDs (see HQF/Resources/Attributes.csv)
    ' ship attributes
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
    Public Const Ship_IceMiningAmount As String = "10036"
    Public Const Ship_IceMiningRate As String = "10048"
    Public Const Ship_KinDamage As String = "10057"
    Public Const Ship_KinDPS As String = "10072"
    Public Const Ship_MaxGangLinks As String = "10063"
    Public Const Ship_MissileDPS As String = "10025"
    Public Const Ship_MissileVolleyDamage As String = "10021"
    Public Const Ship_OreMiningAmount As String = "10033"
    Public Const Ship_OreMiningRate As String = "10047"
    Public Const Ship_SmartbombDPS As String = "10026"
    Public Const Ship_SmartbombVolleyDamage As String = "10022"
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
    Public Const Module_ProjectileDmgMod As String = "10016"
    Public Const Module_ProjectileROF As String = "10013"
    Public Const Module_ThermDamage As String = "10054"
    Public Const Module_TurretIceMiningRate As String = "10041"
    Public Const Module_TurretOreMiningRate As String = "10039"
    Public Const Module_VolleyDamage As String = "10018"

#End Region

End Class

<Serializable()> Public Class Attribute
    Public ID As String
    Public Name As String
    Public DisplayName As String
    Public UnitName As String
    Public AttributeGroup As String
End Class
