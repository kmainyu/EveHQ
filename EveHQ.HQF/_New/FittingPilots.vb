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


Imports EveHQ.Core
Imports System.Windows.Forms
Imports System.IO
Imports Newtonsoft.Json

<Serializable()> Class FittingPilots

    Public Shared HQFPilots As New SortedList(Of String, FittingPilot)

    Public Shared Sub CheckForMissingSkills(ByVal hPilot As FittingPilot)
        If HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            Dim cpilot As EveHQPilot = HQ.Settings.Pilots(hPilot.PilotName)
            For Each newSkill As EveSkill In HQ.SkillListID.Values
                If hPilot.SkillSet.ContainsKey(newSkill.Name) = False Then
                    ' Ooo, a new skill!
                    Dim myHQFSkill As New FittingSkill
                    myHQFSkill.ID = newSkill.ID
                    myHQFSkill.Name = newSkill.Name
                    If cpilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                        Dim mySkill As EveHQPilotSkill = cpilot.PilotSkills(newSkill.Name)
                        myHQFSkill.Level = mySkill.Level
                    Else
                        myHQFSkill.Level = 0
                    End If
                    hPilot.SkillSet.Add(myHQFSkill.Name, myHQFSkill)
                End If
            Next
        End If
    End Sub

    'Bug 40: HQF skills are never checked for invalid/renamed skills so if a pilot's data persists though a database change they can have skills that were changed, and that results in a doubling
    ' or otherwise inaccurate calculation for bonuses and effects.
    Public Shared Sub CheckForInvalidSkills(ByVal hPilot As FittingPilot)
        If HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            ' validate that all the skills in the HQF pilot record exist in the core skill list
            Dim resetRequired As Boolean = False
            For Each skill As FittingSkill In hPilot.SkillSet.Values
                If HQ.SkillListName.Keys.Contains(skill.Name) = False Then
                    ' HQF record has a skill that doesn't exist anymore (or was renamed) the pilot will be reset to default
                    resetRequired = True
                End If
            Next
            If resetRequired = True Then
                ResetSkillsToDefault(hPilot)
                MessageBox.Show(String.Format("The pilot '{0}', was found to have a skill that either has been renamed or no longer exists. In order to ensure a proper experience for fitting calculations, this pilot has had their HQFitter skills reset back to match what the Eve API has reported. If you had some custom values set on this pilot, they will have to be recreated.", hPilot.PilotName))
            End If
        End If
    End Sub

    Public Shared Sub ResetSkillsToDefault(ByVal hPilot As FittingPilot)
        Dim cpilot As EveHQPilot = HQ.Settings.Pilots(hPilot.PilotName)
        hPilot.SkillSet.Clear()
        For Each newSkill As EveSkill In HQ.SkillListID.Values
            Dim myHQFSkill As New FittingSkill
            myHQFSkill.ID = newSkill.ID
            myHQFSkill.Name = newSkill.Name
            If cpilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                Dim mySkill As EveHQPilotSkill = cpilot.PilotSkills(newSkill.Name)
                myHQFSkill.Level = mySkill.Level
            Else
                myHQFSkill.Level = 0
            End If
            hPilot.SkillSet.Add(myHQFSkill.Name, myHQFSkill)
        Next
    End Sub

    Public Shared Sub SetAllSkillsToLevel5(ByVal hPilot As FittingPilot)
        For Each hSkill As FittingSkill In hPilot.SkillSet.Values
            hSkill.Level = 5
        Next
    End Sub

    Public Shared Sub UpdateHQFSkillsToActual(ByVal hPilot As FittingPilot)
        ' If the HQF skill < Actual, this routine makes HQF = Actual
        If HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            Dim cpilot As EveHQPilot = HQ.Settings.Pilots(hPilot.PilotName)
            For Each newSkill As EveSkill In HQ.SkillListID.Values
                If hPilot.SkillSet.ContainsKey(newSkill.Name) = True Then
                    Dim myHQFSkill As FittingSkill = hPilot.SkillSet(newSkill.Name)
                    If cpilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                        Dim mySkill As EveHQPilotSkill = cpilot.PilotSkills(newSkill.Name)
                        myHQFSkill.Level = mySkill.Level
                    Else
                        myHQFSkill.Level = 0
                    End If
                End If
            Next
        End If
    End Sub

    Public Shared Sub SetSkillsToSkillQueue(ByVal hPilot As FittingPilot, ByVal queueName As String)
        If HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            Dim cpilot As EveHQPilot = HQ.Settings.Pilots(hPilot.PilotName)
            If cpilot.TrainingQueues.ContainsKey(queueName) = True Then
                For Each sqi As EveHQSkillQueueItem In cpilot.TrainingQueues(queueName).Queue.Values
                    If hPilot.SkillSet.ContainsKey(sqi.Name) = True Then
                        Dim myHQFSkill As FittingSkill = hPilot.SkillSet(sqi.Name)
                        If myHQFSkill.Level < sqi.ToLevel Then
                            myHQFSkill.Level = sqi.ToLevel
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    Public Shared Sub SetSkillsToSkillList(ByVal hPilot As FittingPilot, ByVal skillList As SortedList(Of String, Integer))
        For Each skillName As String In skillList.Keys
            If hPilot.SkillSet.ContainsKey(skillName) = True Then
                hPilot.SkillSet(skillName).Level = skillList(skillName)
            Else
                Dim myHQFSkill As New FittingSkill
                myHQFSkill.ID = HQ.SkillListName(skillName).ID
                myHQFSkill.Name = skillName
                myHQFSkill.Level = skillList(skillName)
                hPilot.SkillSet.Add(myHQFSkill.Name, myHQFSkill)
            End If
        Next
    End Sub

    Public Shared Sub SaveHQFPilotData()

        ' Create a JSON string for writing
        Dim json As String = JsonConvert.SerializeObject(HQFPilots, Formatting.Indented)

        ' Write the JSON version of the settings
        Try
            Using s As New StreamWriter(Path.Combine(PluginSettings.HQFFolder, "HQFPilotSettings.json"), False)
                s.Write(json)
                s.Flush()
            End Using
        Catch e As Exception
        End Try

    End Sub

    Public Shared Sub LoadHQFPilotData()

        If My.Computer.FileSystem.FileExists(Path.Combine(PluginSettings.HQFFolder, "HQFPilotSettings.json")) = True Then
            Try
                Using s As New StreamReader(Path.Combine(PluginSettings.HQFFolder, "HQFPilotSettings.json"))
                    Dim json As String = s.ReadToEnd
                    HQFPilots = JsonConvert.DeserializeObject(Of SortedList(Of String, FittingPilot))(json)
                End Using
            Catch ex As Exception
                MessageBox.Show("There was an error loading the HQF Pilots file. The file appears corrupt, so it cannot be loaded at this time.")
            End Try
        End If

    End Sub

End Class