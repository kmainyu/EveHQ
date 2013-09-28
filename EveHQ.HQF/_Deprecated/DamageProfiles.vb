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
'Imports System.Runtime.Serialization.Formatters.Binary
'Imports System.IO
'Imports System.Windows.Forms

<Serializable()> Public Class DamageProfile
    Public Name As String
    Public Type As Integer ' = DamageProfileTypes
    Public EM As Double
    Public Explosive As Double
    Public Kinetic As Double
    Public Thermal As Double
    Public DPS As Double
    Public Fitting As String
    Public Pilot As String
    Public NPCs As New ArrayList
End Class

'<Serializable()> Public Class DamageProfiles
'    Public Shared ProfileList As New SortedList

'    Public Shared Sub ResetDamageProfiles()
'        DamageProfiles.ProfileList.Clear()
'        Dim ProfileList As String = My.Resources.DamageProfiles.ToString
'        Dim Profiles() As String = ProfileList.Split(ControlChars.CrLf.ToCharArray)
'        Dim ProfileData() As String
'        For Each Profile As String In Profiles
'            If Profile.Trim <> "" Then
'                ProfileData = Profile.Split(",".ToCharArray)
'                Dim newProfile As New DamageProfile
'                newProfile.Name = ProfileData(0)
'                newProfile.Type = 0
'                newProfile.EM = CDbl(ProfileData(1)) / 100
'                newProfile.Explosive = CDbl(ProfileData(2)) / 100
'                newProfile.Kinetic = CDbl(ProfileData(3)) / 100
'                newProfile.Thermal = CDbl(ProfileData(4)) / 100
'                newProfile.DPS = 0
'                newProfile.Fitting = ""
'                newProfile.Pilot = ""
'                newProfile.NPCs.Clear()
'                DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
'            End If
'        Next
'        DamageProfiles.SaveProfiles()
'    End Sub

'    Public Shared Sub LoadProfiles()
'        ' Check for the profiles file so we can load it
'        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin")) = True Then
'            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin"), FileMode.Open)
'            Try
'                Dim f As BinaryFormatter = New BinaryFormatter
'                DamageProfiles.ProfileList = CType(f.Deserialize(s), SortedList)
'            Catch ex As Exception
'                MessageBox.Show("The Damage Profiles file appears to be corrupt. A new file will be created, however any customizations will be lost.")
'                Call ResetDamageProfiles()
'            Finally
'                s.Close()
'            End Try
'        Else
'            ' Need to create the profiles file and the standard custom profile (omni-damage)
'            Call DamageProfiles.ResetDamageProfiles()
'        End If
'    End Sub

'    Public Shared Sub SaveProfiles()
'        ' Save the Profiles
'        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin"), FileMode.Create)
'        Dim f As New BinaryFormatter
'        f.Serialize(s, DamageProfiles.ProfileList)
'        s.Flush()
'        s.Close()
'    End Sub
'End Class

'Public Enum DamageProfileTypes
'    Manual = 0
'    Fitting = 1
'    NPCs = 2
'End Enum
