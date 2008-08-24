Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

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
<Serializable()> Public Class HQFPilot

#Region "Property Variables"

    Private cPilotName As String
    Private cSkillSet As New Collection
    Private cImplantName(10) As String

#End Region

#Region "Properties"

    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
        End Set
    End Property

    Public Property SkillSet() As Collection
        Get
            Return cSkillSet
        End Get
        Set(ByVal value As Collection)
            cSkillSet = value
        End Set
    End Property

    Public Property ImplantName(ByVal index As Integer) As String
        ' Use ImplantName(0) as the GroupName identifer
        Get
            Return cImplantName(index)
        End Get
        Set(ByVal value As String)
            cImplantName(index) = value
        End Set
    End Property

#End Region

End Class

<Serializable()> Class HQFPilotCollection
    Public Shared HQFPilots As New SortedList

    Public Shared Sub ResetSkillsToDefault(ByVal hPilot As HQFPilot)
        Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.Pilots(hpilot.PilotName), Core.Pilot)
        hPilot.SkillSet.Clear()
        For Each newSkill As EveHQ.Core.SkillList In EveHQ.Core.HQ.SkillListID
            Dim MyHQFSkill As New HQFSkill
            MyHQFSkill.ID = newSkill.ID
            MyHQFSkill.Name = newSkill.Name
            If cpilot.PilotSkills.Contains(newSkill.Name) = True Then
                Dim mySkill As EveHQ.Core.Skills = CType(cpilot.PilotSkills(newSkill.Name), Core.Skills)
                MyHQFSkill.Level = mySkill.Level
            Else
                MyHQFSkill.Level = 0
            End If
            hPilot.SkillSet.Add(MyHQFSkill, MyHQFSkill.Name)
        Next
    End Sub

    Public Shared Sub SetAllSkillsToLevel5(ByVal hPilot As HQFPilot)
        For Each hSkill As HQFSkill In hPilot.SkillSet
            hSkill.Level = 5
        Next
    End Sub

    Public Shared Sub UpdateHQFSkillsToActual(ByVal hPilot As HQFPilot)
        ' If the HQF skill < Actual, this routine makes HQF = Actual
        If EveHQ.Core.HQ.Pilots.Contains(hPilot.PilotName) = True Then
            Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.Pilots(hPilot.PilotName), Core.Pilot)
            For Each newSkill As EveHQ.Core.SkillList In EveHQ.Core.HQ.SkillListID
                If hPilot.SkillSet.Contains(newSkill.Name) = True Then
                    Dim MyHQFSkill As HQFSkill = CType(hPilot.SkillSet(newSkill.Name), HQFSkill)
                    If cpilot.PilotSkills.Contains(newSkill.Name) = True Then
                        Dim mySkill As EveHQ.Core.Skills = CType(cpilot.PilotSkills(newSkill.Name), Core.Skills)
                        If MyHQFSkill.Level < mySkill.Level Then
                            MyHQFSkill.Level = mySkill.Level
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Shared Sub SaveHQFPilotData()
        Dim s As New FileStream(HQF.Settings.HQFFolder & "\HQFPilotSettings.bin", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, HQFPilotCollection.HQFPilots)
        s.Close()
    End Sub

    Public Shared Sub LoadHQFPilotData()
        If My.Computer.FileSystem.FileExists(HQF.Settings.HQFFolder & "\HQFPilotSettings.bin") = True Then
            Dim s As New FileStream(HQF.Settings.HQFFolder & "\HQFPilotSettings.bin", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            HQFPilotCollection.HQFPilots = CType(f.Deserialize(s), SortedList)
            s.Close()
        End If
    End Sub
End Class

<Serializable()> Public Class HQFSkill
    Public ID As String
    Public Name As String
    Public Level As Integer
End Class

