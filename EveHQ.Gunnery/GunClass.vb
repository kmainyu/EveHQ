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
Public Class GunClass
    Implements System.ICloneable

    Public typeID As String
    Public typeName As String
    Public groupID As String
    Public metaLevel As Integer
    Public metaType As String
    Public PGUsage As Double
    Public CPUUsage As Double
    Public CapUsage As Double
    Public OptimalRange As Double
    Public DamageModifier As Double
    Public AccuracyFalloff As Double
    Public TrackingSpeed As Double
    Public RateOfFire As Double
    Public Size As Integer
    Public EMDamage As Double
    Public ExDamage As Double
    Public KiDamage As Double
    Public ThDamage As Double
    Public Damage As Double
    Public DPS As Double
    Public PrimarySkill As String
    Public SecondarySkill As String
    Public TertiarySkill As String
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As GunClass = CType(Me.MemberwiseClone, GunClass)
        Return R
    End Function

End Class

Public Class AmmoClass
    Implements System.ICloneable

    Public typeID As String
    Public typeName As String
    Public groupID As String
    Public metaLevel As Integer
    Public metaType As String
    Public RangeBonus As Double
    Public CapBonus As Double
    Public ROFBonus As Double
    Public TrackingBonus As Double
    Public FalloffBonus As Double
    Public EMDamage As Double
    Public ExDamage As Double
    Public KiDamage As Double
    Public ThDamage As Double
    Public Size As Integer
    Public PrimarySkill As String
    Public SecondarySkill As String
    Public TertiarySkill As String
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As AmmoClass = CType(Me.MemberwiseClone, AmmoClass)
        Return R
    End Function

End Class

Public Class DamageModClass
    Implements System.ICloneable

    Public typeID As String
    Public typeName As String
    Public groupID As String
    Public metaLevel As Integer
    Public metaType As String
    Public ROFBonus As Double
    Public DamageBonus As Double
    Public PrimarySkill As String
    Public SecondarySkill As String
    Public TertiarySkill As String
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As DamageModClass = CType(Me.MemberwiseClone, DamageModClass)
        Return R
    End Function

End Class

Public Class TrackingModClass
    Implements System.ICloneable

    Public typeID As String
    Public typeName As String
    Public groupID As String
    Public metaLevel As Integer
    Public metaType As String
    Public RangeBonus As Double
    Public TrackingBonus As Double
    Public PrimarySkill As String
    Public SecondarySkill As String
    Public TertiarySkill As String
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As TrackingModClass = CType(Me.MemberwiseClone, TrackingModClass)
        Return R
    End Function

End Class

Public Class GunnerySkills
    Public AWU As Integer
    Public CapEnergy As Integer
    Public CapHybrid As Integer
    Public CapProjectile As Integer
    Public ControlledBursts As Integer
    Public Gunnery As Integer
    Public LargeArtSpec As Integer
    Public LargeACSpec As Integer
    Public LargeBLSpec As Integer
    Public LargePLSpec As Integer
    Public LargeRailSpec As Integer
    Public LargeBlasterSpec As Integer
    Public LargeEnergy As Integer
    Public LargeHybrid As Integer
    Public LargeProjectile As Integer
    Public MedArtSpec As Integer
    Public MedACSpec As Integer
    Public MedBLSpec As Integer
    Public MedPLSpec As Integer
    Public MedRailSpec As Integer
    Public MedBlasterSpec As Integer
    Public MedEnergy As Integer
    Public MedHybrid As Integer
    Public MedProjectile As Integer
    Public MotionPredict As Integer
    Public RapidFiring As Integer
    Public Sharpshooter As Integer
    Public SmallArtSpec As Integer
    Public SmallACSpec As Integer
    Public SmallBLSpec As Integer
    Public SmallPLSpec As Integer
    Public SmallRailSpec As Integer
    Public SmallBlasterSpec As Integer
    Public SmallEnergy As Integer
    Public SmallHybrid As Integer
    Public SmallProjectile As Integer
    Public SurgicalStrike As Integer
    Public TWR As Integer
    Public TrajectoryAnal As Integer
    Public WU As Integer
End Class
