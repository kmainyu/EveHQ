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
Imports System.Windows.Forms

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
            ' Check if we can set the implants from the group listing
            If index = 0 Then
                If value <> "*Custom*" Then
                    If HQF.Settings.HQFSettings.ImplantGroups.ContainsKey(value) Then
                        Dim ImplantSet As ImplantGroup = CType(HQF.Settings.HQFSettings.ImplantGroups(value), ImplantGroup)
                        For slot As Integer = 1 To 10
                            cImplantName(slot) = ImplantSet.ImplantName(slot)
                        Next
                    End If
                End If
            End If
        End Set
    End Property

#End Region

End Class

<Serializable()> Class HQFPilotCollection
    Public Shared HQFPilots As New SortedList

    Public Shared Sub CheckForMissingSkills(ByVal hPilot As HQFPilot)
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(hPilot.PilotName) = True Then
            Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(hPilot.PilotName), Core.Pilot)
            For Each newSkill As EveHQ.Core.EveSkill In EveHQ.Core.HQ.SkillListID.Values
                If hPilot.SkillSet.Contains(newSkill.Name) = False Then
                    ' Ooo, a new skill!
                    Dim MyHQFSkill As New HQFSkill
                    MyHQFSkill.ID = newSkill.ID
                    MyHQFSkill.Name = newSkill.Name
                    If cpilot.PilotSkills.Contains(newSkill.Name) = True Then
                        Dim mySkill As EveHQ.Core.PilotSkill = CType(cpilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                        MyHQFSkill.Level = mySkill.Level
                    Else
                        MyHQFSkill.Level = 0
                    End If
                    hPilot.SkillSet.Add(MyHQFSkill, MyHQFSkill.Name)
                End If
            Next
        End If
    End Sub

    'Bug 40: HQF skills are never checked for invalid/renamed skills so if a pilot's data persists though a database change they can have skills that were changed, and that results in a doubling
    ' or otherwise inaccurate calculation for bonuses and effects.
    Public Shared Sub CheckForInvalidSkills(ByVal hPilot As HQFPilot)
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(hPilot.PilotName) = True Then
            Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(hPilot.PilotName), Core.Pilot)
            ' validate that all the skills in the HQF pilot record exist in the core skill list
            For Each skill As HQFSkill In hPilot.SkillSet
                If EveHQ.Core.HQ.SkillListName.Keys.Contains(skill.Name) = False Then
                    ' HQF record has a skill that doesn't exist anymore (or was renamed)
                    ' the pilot will be reset to default
                    ResetSkillsToDefault(hPilot)
                    MessageBox.Show(String.Format("The pilot '{0}', was found to have a skill that either has been renamed or no longer exists ({1}). In order to ensure a proper experience for fitting calculations, this pilot has had their HQFitter skills reset back to match what the Eve API has reported. If you had some custom values set on this pilot, they will have to be recreated.", hPilot.PilotName, skill.Name))
                End If
            Next
        End If
    End Sub

    Public Shared Sub ResetSkillsToDefault(ByVal hPilot As HQFPilot)
        Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(hPilot.PilotName), Core.Pilot)
        hPilot.SkillSet.Clear()
        For Each newSkill As EveHQ.Core.EveSkill In EveHQ.Core.HQ.SkillListID.Values
            Dim MyHQFSkill As New HQFSkill
            MyHQFSkill.ID = newSkill.ID
            MyHQFSkill.Name = newSkill.Name
            If cpilot.PilotSkills.Contains(newSkill.Name) = True Then
                Dim mySkill As EveHQ.Core.PilotSkill = CType(cpilot.PilotSkills(newSkill.Name), Core.PilotSkill)
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
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(hPilot.PilotName) = True Then
            Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(hPilot.PilotName), Core.Pilot)
            For Each newSkill As EveHQ.Core.EveSkill In EveHQ.Core.HQ.SkillListID.Values
                If hPilot.SkillSet.Contains(newSkill.Name) = True Then
                    Dim MyHQFSkill As HQFSkill = CType(hPilot.SkillSet(newSkill.Name), HQFSkill)
                    If cpilot.PilotSkills.Contains(newSkill.Name) = True Then
                        Dim mySkill As EveHQ.Core.PilotSkill = CType(cpilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                        If MyHQFSkill.Level < mySkill.Level Then
                            MyHQFSkill.Level = mySkill.Level
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Shared Sub SetSkillsToSkillQueue(ByVal hPilot As HQFPilot, ByVal queueName As String)
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(hPilot.PilotName) = True Then
            Dim cpilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(hPilot.PilotName), Core.Pilot)
            If cpilot.TrainingQueues.ContainsKey(queueName) = True Then
                For Each sqi As EveHQ.Core.SkillQueueItem In CType(cpilot.TrainingQueues(queueName), EveHQ.Core.SkillQueue).Queue
                    If hPilot.SkillSet.Contains(sqi.Name) = True Then
                        Dim MyHQFSkill As HQFSkill = CType(hPilot.SkillSet(sqi.Name), HQFSkill)
                        If MyHQFSkill.Level < sqi.ToLevel Then
                            MyHQFSkill.Level = sqi.ToLevel
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    Public Shared Sub SetSkillsToSkillList(ByVal hPilot As HQFPilot, ByVal SkillList As SortedList(Of String, Integer))
        For Each SkillName As String In SkillList.Keys
            If hPilot.SkillSet.Contains(SkillName) = True Then
                CType(hPilot.SkillSet(SkillName), HQFSkill).Level = SkillList(SkillName)
            Else
                Dim MyHQFSkill As New HQFSkill
                MyHQFSkill.ID = CType(EveHQ.Core.HQ.SkillListName(SkillName), EveHQ.Core.EveSkill).ID
                MyHQFSkill.Name = SkillName
                MyHQFSkill.Level = SkillList(SkillName)
                hPilot.SkillSet.Add(MyHQFSkill, MyHQFSkill.Name)
            End If
        Next
    End Sub


    Public Shared Sub SaveHQFPilotData()
        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFPilotSettings.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, HQFPilotCollection.HQFPilots)
        s.Flush()
        s.Close()
    End Sub

    Public Shared Sub LoadHQFPilotData()
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFPilotSettings.bin")) = True Then
            Dim s As FileStream
            Try
            s = New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFPilotSettings.bin"), FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
                HQFPilotCollection.HQFPilots = CType(f.Deserialize(s), SortedList)
            Catch ex As Exception
                MessageBox.Show("There was an loading the pilot settings file. It appears to be corrupted. A new file will be created however the old data is lost.")
                HQFPilots = New SortedList
            Finally
                s.Close()
            End Try

        End If
    End Sub

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class HQFSkill
    Public ID As String
    Public Name As String
    Public Level As Integer
End Class

