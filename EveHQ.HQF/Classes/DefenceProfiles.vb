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
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

<Serializable()> Public Class DefenceProfile
    Public Name As String
    Public Type As Integer ' = DefenceProfileTypes
    Public SEM As Double
    Public SExplosive As Double
    Public SKinetic As Double
    Public SThermal As Double
    Public AEM As Double
    Public AExplosive As Double
    Public AKinetic As Double
    Public AThermal As Double
    Public HEM As Double
    Public HExplosive As Double
    Public HKinetic As Double
    Public HThermal As Double
    Public DPS As Double
    Public Fitting As String
    Public Pilot As String
End Class

<Serializable()> Public Class DefenceProfiles
    Public Shared ProfileList As New SortedList

    Public Shared Sub ResetDefenceProfiles()
        DefenceProfiles.ProfileList.Clear()
        Dim ProfileList As String = My.Resources.DefenceProfiles.ToString
        Dim Profiles() As String = ProfileList.Split(ControlChars.CrLf.ToCharArray)
        Dim ProfileData() As String
        For Each Profile As String In Profiles
            If Profile.Trim <> "" Then
                ProfileData = Profile.Split(",".ToCharArray)
                Dim newProfile As New DefenceProfile
                newProfile.Name = ProfileData(0)
                newProfile.Type = 0
                newProfile.SEM = CDbl(ProfileData(1)) / 100
                newProfile.SExplosive = CDbl(ProfileData(2)) / 100
                newProfile.SKinetic = CDbl(ProfileData(3)) / 100
                newProfile.SThermal = CDbl(ProfileData(4)) / 100
                newProfile.AEM = CDbl(ProfileData(5)) / 100
                newProfile.AExplosive = CDbl(ProfileData(6)) / 100
                newProfile.AKinetic = CDbl(ProfileData(7)) / 100
                newProfile.AThermal = CDbl(ProfileData(8)) / 100
                newProfile.HEM = CDbl(ProfileData(9)) / 100
                newProfile.HExplosive = CDbl(ProfileData(10)) / 100
                newProfile.HKinetic = CDbl(ProfileData(11)) / 100
                newProfile.HThermal = CDbl(ProfileData(12)) / 100
                newProfile.DPS = 0
                newProfile.Fitting = ""
                newProfile.Pilot = ""
                DefenceProfiles.ProfileList.Add(newProfile.Name, newProfile)
            End If
        Next
        DefenceProfiles.SaveProfiles()
    End Sub

    Public Shared Sub LoadProfiles()
        ' Check for the profiles file so we can load it
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFDefenceProfiles.bin")) = True Then
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFDefenceProfiles.bin"), FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            DefenceProfiles.ProfileList = CType(f.Deserialize(s), SortedList)
            s.Close()
        Else
            ' Need to create the profiles file and the standard custom profile (omni-damage)
            Call DamageProfiles.ResetDamageProfiles()
        End If
    End Sub

    Public Shared Sub SaveProfiles()
        ' Save the Profiles
        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFDefenceProfiles.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, DefenceProfiles.ProfileList)
        s.Flush()
        s.Close()
    End Sub
End Class

Public Enum DefenceProfileTypes
    Manual = 0
    Fitting = 1
    NPCs = 2
End Enum

Public Class DefenceProfileResults
    Public ShieldDPS As Double
    Public ArmorDPS As Double
    Public HullDPS As Double
End Class
