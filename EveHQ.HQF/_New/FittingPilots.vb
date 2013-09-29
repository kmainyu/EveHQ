Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> Class FittingPilots

    Public Shared HQFPilots As New SortedList(Of String, FittingPilot)

    Public Shared Sub CheckForMissingSkills(ByVal hPilot As FittingPilot)
        If Core.HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            Dim cpilot As Core.EveHQPilot = Core.HQ.Settings.Pilots(hPilot.PilotName)
            For Each newSkill As Core.EveSkill In Core.HQ.SkillListID.Values
                If hPilot.SkillSet.ContainsKey(newSkill.Name) = False Then
                    ' Ooo, a new skill!
                    Dim myHQFSkill As New FittingSkill
                    myHQFSkill.ID = newSkill.ID
                    myHQFSkill.Name = newSkill.Name
                    If cpilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                        Dim mySkill As Core.EveHQPilotSkill = cpilot.PilotSkills(newSkill.Name)
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
        If Core.HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            ' validate that all the skills in the HQF pilot record exist in the core skill list
            For Each skill As FittingSkill In hPilot.SkillSet.Values
                If Core.HQ.SkillListName.Keys.Contains(skill.Name) = False Then
                    ' HQF record has a skill that doesn't exist anymore (or was renamed)
                    ' the pilot will be reset to default
                    ResetSkillsToDefault(hPilot)
                    MessageBox.Show(String.Format("The pilot '{0}', was found to have a skill that either has been renamed or no longer exists ({1}). In order to ensure a proper experience for fitting calculations, this pilot has had their HQFitter skills reset back to match what the Eve API has reported. If you had some custom values set on this pilot, they will have to be recreated.", hPilot.PilotName, skill.Name))
                End If
            Next
        End If
    End Sub

    Public Shared Sub ResetSkillsToDefault(ByVal hPilot As FittingPilot)
        Dim cpilot As Core.EveHQPilot = Core.HQ.Settings.Pilots(hPilot.PilotName)
        hPilot.SkillSet.Clear()
        For Each newSkill As Core.EveSkill In Core.HQ.SkillListID.Values
            Dim myHQFSkill As New FittingSkill
            myHQFSkill.ID = newSkill.ID
            myHQFSkill.Name = newSkill.Name
            If cpilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                Dim mySkill As Core.EveHQPilotSkill = cpilot.PilotSkills(newSkill.Name)
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
        If Core.HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            Dim cpilot As Core.EveHQPilot = Core.HQ.Settings.Pilots(hPilot.PilotName)
            For Each newSkill As Core.EveSkill In Core.HQ.SkillListID.Values
                If hPilot.SkillSet.ContainsKey(newSkill.Name) = True Then
                    Dim myHQFSkill As FittingSkill = hPilot.SkillSet(newSkill.Name)
                    If cpilot.PilotSkills.ContainsKey(newSkill.Name) = True Then
                        Dim mySkill As Core.EveHQPilotSkill = cpilot.PilotSkills(newSkill.Name)
                        If myHQFSkill.Level < mySkill.Level Then
                            myHQFSkill.Level = mySkill.Level
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Shared Sub SetSkillsToSkillQueue(ByVal hPilot As FittingPilot, ByVal queueName As String)
        If Core.HQ.Settings.Pilots.ContainsKey(hPilot.PilotName) = True Then
            Dim cpilot As Core.EveHQPilot = Core.HQ.Settings.Pilots(hPilot.PilotName)
            If cpilot.TrainingQueues.ContainsKey(queueName) = True Then
                For Each sqi As Core.EveHQSkillQueueItem In cpilot.TrainingQueues(queueName).Queue.Values
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
                myHQFSkill.ID = Core.HQ.SkillListName(skillName).ID
                myHQFSkill.Name = skillName
                myHQFSkill.Level = skillList(skillName)
                hPilot.SkillSet.Add(myHQFSkill.Name, myHQFSkill)
            End If
        Next
    End Sub

    Public Shared Sub SaveHQFPilotData()
        Dim s As New FileStream(Path.Combine(Settings.HQFFolder, "HQFPilotSettings.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, HQFPilots)
        s.Flush()
        s.Close()
    End Sub

    Public Shared Sub LoadHQFPilotData()
        If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFFolder, "HQFPilotSettings.bin")) = True Then
            Dim s As FileStream = Nothing
            Try
                s = New FileStream(Path.Combine(Settings.HQFFolder, "HQFPilotSettings.bin"), FileMode.Open)
                Dim f As BinaryFormatter = New BinaryFormatter
                HQFPilots = CType(f.Deserialize(s), SortedList(Of String, FittingPilot))
            Catch ex As Exception
                MessageBox.Show("There was an loading the pilot settings file. It appears to be corrupted. A new file will be created however the old data is lost.")
                HQFPilots = New SortedList(Of String, FittingPilot)
            Finally
                s.Close()
            End Try

        End If
    End Sub

End Class