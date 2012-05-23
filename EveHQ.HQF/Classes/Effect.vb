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
Public Class Effect
    Public AffectingAtt As Integer
    Public AffectingType As EffectType
    Public AffectingID As Integer
    Public AffectedAtt As Integer
    Public AffectedType As EffectType
    Public AffectedID As New List(Of String)
    Public StackNerf As EffectStackType
    Public IsPerLevel As Boolean
    Public CalcType As EffectCalcType
    Public Status As Integer
End Class

<Serializable()> Public Class ShipEffect
    Public ShipID As Integer
    Public AffectingType As EffectType
    Public AffectingID As Integer
    Public AffectedAtt As Integer
    Public AffectedType As EffectType
    Public AffectedID As New List(Of String)
    Public StackNerf As EffectStackType
    Public IsPerLevel As Boolean
    Public CalcType As EffectCalcType
    Public Status As Integer
    Public Value As Double

    Public Function Clone() As ShipEffect
        Dim NewEffect As New ShipEffect
        NewEffect.ShipID = Me.ShipID
        NewEffect.AffectingType = Me.AffectingType
        NewEffect.AffectingID = Me.AffectingID
        NewEffect.AffectedAtt = Me.AffectedAtt
        NewEffect.AffectedType = Me.AffectedType
        For Each id As String In Me.AffectedID
            NewEffect.AffectedID.Add(id)
        Next
        NewEffect.StackNerf = Me.StackNerf
        NewEffect.IsPerLevel = Me.IsPerLevel
        NewEffect.CalcType = Me.CalcType
        NewEffect.Status = Me.Status
        NewEffect.Value = Me.Value
        Return NewEffect
    End Function

End Class

Public Class ImplantEffect
    Public ImplantName As String
    Public AffectingAtt As Integer
    Public AffectedAtt As Integer
    Public AffectedType As EffectType
    Public AffectedID As New List(Of String)
    Public CalcType As EffectCalcType
    Public Status As Integer
    Public Value As Double
    Public IsGang As Boolean
    Public Groups As New ArrayList
End Class

<Serializable()> Public Class FinalEffect
    Public AffectedAtt As Integer
    Public AffectedType As EffectType
    Public AffectedID As New List(Of String)
    Public AffectedValue As Double
    Public StackNerf As EffectStackType
    Public Cause As String
    Public CalcType As EffectCalcType
    Public Status As Integer
End Class

Public Enum EffectType As Integer
    All = 0
    Item = 1
    Group = 2
    Category = 3
    MarketGroup = 4
    Skill = 5
    Slot = 6
    Attribute = 7
End Enum

Public Enum EffectCalcType As Integer
    Percentage = 0 ' Simply percentage variation (+/-)
    Addition = 1 ' For adding values
    Difference = 2 ' For resistances
    Velocity = 3 ' For AB/MWD calculations
    Absolute = 4 ' For setting values
    Multiplier = 5 ' Damage multiplier
    AddPositive = 6 ' Adding positive values only
    AddNegative = 7 ' Adding negative values only
    Subtraction = 8 ' Subtracting positive values
    CloakedVelocity = 9 ' Bonus for dealing with cloaked velocity
    SkillLevel = 10 ' Add one to the existing value
    SkillLevelxAtt = 11 ' Multiply the attribute by the skill level
    AbsoluteMax = 12 ' Set value only if higher than the existing value
    AbsoluteMin = 13 ' Set value only if lower than the existing value
    CapBoosters = 14 ' For cap and fueled shield boosters
End Enum

Public Enum EffectStackType As Integer
    None = 0
    Standard = 1
    Group = 2
    Item = 3
End Enum

