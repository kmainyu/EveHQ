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
Imports System
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Xml
Imports System.Drawing.Drawing2D

Public Class frmPilot

    Public Sub UpdatePilotInfo()
        ' Check if the pilot has had data and therefore able to be displayed properly

        If EveHQ.Core.HQ.myPilot.PilotData.OuterXml <> "" Then

            ' Get image from cache
            Try
                Dim imgFilename As String = EveHQ.Core.HQ.cacheFolder & "\i" & EveHQ.Core.HQ.myPilot.ID & ".png"
                If My.Computer.FileSystem.FileExists(imgFilename) = True Then
                    picPilot.ImageLocation = imgFilename
                Else
                    picPilot.Image = My.Resources.noitem
                End If

                Call EveHQ.Core.PilotParseFunctions.SwitchImplants()

                If frmTraining.IsHandleCreated = True Then
                    Call frmTraining.RefreshAllTraining()
                End If
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & EveHQ.Core.HQ.myPilot.Name
                MessageBox.Show(msg, "Error Retrieving Cached Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Information
            Try
                lvPilot.Items.Clear()
                lvPilot.Items.Add("Name")
                lvPilot.Items(0).SubItems.Add(EveHQ.Core.HQ.myPilot.Name)
                lvPilot.Items.Add("Gender")
                lvPilot.Items(1).SubItems.Add(EveHQ.Core.HQ.myPilot.Gender)
                lvPilot.Items.Add("Char. ID")
                lvPilot.Items(2).SubItems.Add(EveHQ.Core.HQ.myPilot.ID)
                lvPilot.Items.Add("Corporation")
                lvPilot.Items(3).SubItems.Add(EveHQ.Core.HQ.myPilot.Corp)
                lvPilot.Items.Add("Race")
                lvPilot.Items(4).SubItems.Add(EveHQ.Core.HQ.myPilot.Race & " (" & EveHQ.Core.HQ.myPilot.Blood & ")")
                lvPilot.Items.Add("Wealth")
                lvPilot.Items(5).SubItems.Add(FormatNumber(EveHQ.Core.HQ.myPilot.Isk, 2, , , TriState.True))
                lvPilot.Items.Add("Skill Points")
                lvPilot.Items(6).SubItems.Add(FormatNumber(EveHQ.Core.HQ.myPilot.SkillPoints + EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(EveHQ.Core.HQ.myPilot), 0, , , TriState.True))
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & EveHQ.Core.HQ.myPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Implants
            Try
                lvImplants.Items.Clear()
                lvImplants.Items.Add("Charisma")
                lvImplants.Items(0).SubItems.Add("+" & EveHQ.Core.HQ.myPilot.CImplant)
                lvImplants.Items.Add("Intelligence")
                lvImplants.Items(1).SubItems.Add("+" & EveHQ.Core.HQ.myPilot.IImplant)
                lvImplants.Items.Add("Memory")
                lvImplants.Items(2).SubItems.Add("+" & EveHQ.Core.HQ.myPilot.MImplant)
                lvImplants.Items.Add("Perception")
                lvImplants.Items(3).SubItems.Add("+" & EveHQ.Core.HQ.myPilot.PImplant)
                lvImplants.Items.Add("Willpower")
                lvImplants.Items(4).SubItems.Add("+" & EveHQ.Core.HQ.myPilot.WImplant)
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & EveHQ.Core.HQ.myPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Implants", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Implant method
            If EveHQ.Core.HQ.myPilot.UseManualImplants = True Then
                Me.chkManualImplants.Checked = True
            Else
                Me.chkManualImplants.Checked = False
            End If

            ' Display Attributes
            Try
                lvAttributes.Items.Clear()
                lvAttributes.Items.Add("Charisma")
                lvAttributes.Items(0).SubItems.Add("+" & FormatNumber(EveHQ.Core.HQ.myPilot.CAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Intelligence")
                lvAttributes.Items(1).SubItems.Add("+" & FormatNumber(EveHQ.Core.HQ.myPilot.IAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Memory")
                lvAttributes.Items(2).SubItems.Add("+" & FormatNumber(EveHQ.Core.HQ.myPilot.MAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Perception")
                lvAttributes.Items(3).SubItems.Add("+" & FormatNumber(EveHQ.Core.HQ.myPilot.PAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Willpower")
                lvAttributes.Items(4).SubItems.Add("+" & FormatNumber(EveHQ.Core.HQ.myPilot.WAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & EveHQ.Core.HQ.myPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Attributes", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            Try
                For a As Integer = 0 To 4
                    Dim msg As String = ""
                    Select Case lvAttributes.Items(a).Text
                        Case ("Charisma")
                            msg = "Charisma Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & EveHQ.Core.HQ.myPilot.CAtt & vbCrLf
                            msg &= "Basic Learning: " & EveHQ.Core.HQ.myPilot.LCAtt & vbCrLf
                            msg &= "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALCAtt & vbCrLf
                            msg &= "Implant: " & EveHQ.Core.HQ.myPilot.CImplant & vbCrLf
                            msg &= "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSCAtt & vbCrLf
                            msg &= "TOTAL: " & EveHQ.Core.HQ.myPilot.CAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Intelligence")
                            msg = "Intelligence Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & EveHQ.Core.HQ.myPilot.IAtt & vbCrLf
                            msg &= "Basic Learning: " & EveHQ.Core.HQ.myPilot.LIAtt & vbCrLf
                            msg &= "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALIAtt & vbCrLf
                            msg &= "Implant: " & EveHQ.Core.HQ.myPilot.IImplant & vbCrLf
                            msg &= "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSIAtt & vbCrLf
                            msg &= "TOTAL: " & EveHQ.Core.HQ.myPilot.IAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Memory")
                            msg = "Memory Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & EveHQ.Core.HQ.myPilot.MAtt & vbCrLf
                            msg &= "Basic Learning: " & EveHQ.Core.HQ.myPilot.LMAtt & vbCrLf
                            msg &= "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALMAtt & vbCrLf
                            msg &= "Implant: " & EveHQ.Core.HQ.myPilot.MImplant & vbCrLf
                            msg &= "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSMAtt & vbCrLf
                            msg &= "TOTAL: " & EveHQ.Core.HQ.myPilot.MAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Perception")
                            msg = "Perception Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & EveHQ.Core.HQ.myPilot.PAtt & vbCrLf
                            msg &= "Basic Learning: " & EveHQ.Core.HQ.myPilot.LPAtt & vbCrLf
                            msg &= "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALPAtt & vbCrLf
                            msg &= "Implant: " & EveHQ.Core.HQ.myPilot.PImplant & vbCrLf
                            msg &= "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSPAtt & vbCrLf
                            msg &= "TOTAL: " & EveHQ.Core.HQ.myPilot.PAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Willpower")
                            msg = "Willpower Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & EveHQ.Core.HQ.myPilot.WAtt & vbCrLf
                            msg &= "Basic Learning: " & EveHQ.Core.HQ.myPilot.LWAtt & vbCrLf
                            msg &= "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALWAtt & vbCrLf
                            msg &= "Implant: " & EveHQ.Core.HQ.myPilot.WImplant & vbCrLf
                            msg &= "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSWAtt & vbCrLf
                            msg &= "TOTAL: " & EveHQ.Core.HQ.myPilot.WAttT
                            lvAttributes.Items(a).ToolTipText = msg
                    End Select
                Next
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & EveHQ.Core.HQ.myPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Attribute Tooltips", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Skill Training
            Try
                lvTraining.Items.Clear()
                lvTraining.Items.Add("Training?")
                lvTraining.Items.Add("Currently Training")
                lvTraining.Items.Add("Skill Rank/Rate")
                lvTraining.Items.Add("Training End Time")
                lvTraining.Items.Add("XML Expiration")
                If EveHQ.Core.HQ.myPilot.Training = True Then
                    Dim currentSkill As EveHQ.Core.Skills = CType(EveHQ.Core.HQ.myPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(EveHQ.Core.HQ.myPilot.TrainingSkillID)), Core.Skills)
                    lvTraining.Items(0).SubItems.Add("Yes")
                    lvTraining.Items(1).SubItems.Add(currentSkill.Name & " (Level " & EveHQ.Core.SkillFunctions.Roman(EveHQ.Core.HQ.myPilot.TrainingSkillLevel) & ")")
                    lvTraining.Items(2).SubItems.Add("Rank " & currentSkill.Rank & " @ " & FormatNumber(EveHQ.Core.SkillFunctions.CalculateSPRate(EveHQ.Core.HQ.myPilot, CType(EveHQ.Core.HQ.SkillListName(currentSkill.Name), Core.SkillList)), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " SP/Hr")
                    Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(EveHQ.Core.HQ.myPilot.TrainingEndTime)
                    lvTraining.Items(3).SubItems.Add(Format(localdate, "ddd") & " " & localdate & " (" & EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.HQ.myPilot.TrainingCurrentTime) & ")")
                Else
                    lvTraining.Items(0).SubItems.Add("No")
                    lvTraining.Items(1).SubItems.Add("n/a")
                    lvTraining.Items(2).SubItems.Add("n/a")
                    lvTraining.Items(3).SubItems.Add("n/a")
                End If
                Dim cacheDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(EveHQ.Core.HQ.myPilot.CacheExpirationTime)
                Dim cacheItem As New ListViewItem.ListViewSubItem
                cacheItem.Text = (Format(cacheDate, "ddd") & " " & cacheDate)
                If cacheDate < Now Then
                    lvTraining.Items(4).ForeColor = Color.Green
                    EveHQ.Core.HQ.UpdateAvailable = True
                Else
                    lvTraining.Items(4).ForeColor = Color.Red
                    EveHQ.Core.HQ.UpdateAvailable = False
                End If
                lvTraining.Items(4).SubItems.Add(cacheItem)
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & EveHQ.Core.HQ.myPilot.Name
                msg &= "Training Skill ID: " & EveHQ.Core.HQ.myPilot.TrainingSkillID & ControlChars.CrLf
                msg &= "Pilot Has Skill: " & EveHQ.Core.HQ.myPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(EveHQ.Core.HQ.myPilot.TrainingSkillID))
                msg &= "EveHQ Has Skill: " & EveHQ.Core.HQ.SkillListID.Contains(EveHQ.Core.HQ.myPilot.TrainingSkillID)
                MessageBox.Show(msg, "Error Displaying Skill Training Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Skills
            Dim groupHeaders(14, 3) As String
            groupHeaders(0, 0) = "Corporation Management"
            groupHeaders(1, 0) = "Drones"
            groupHeaders(2, 0) = "Electronics"
            groupHeaders(3, 0) = "Engineering"
            groupHeaders(4, 0) = "Gunnery"
            groupHeaders(5, 0) = "Industry"
            groupHeaders(6, 0) = "Leadership"
            groupHeaders(7, 0) = "Learning"
            groupHeaders(8, 0) = "Mechanic"
            groupHeaders(9, 0) = "Missiles"
            groupHeaders(10, 0) = "Navigation"
            groupHeaders(11, 0) = "Science"
            groupHeaders(12, 0) = "Social"
            groupHeaders(13, 0) = "Spaceship Command"
            groupHeaders(14, 0) = "Trade"
            groupHeaders(0, 1) = "266"
            groupHeaders(1, 1) = "273"
            groupHeaders(2, 1) = "272"
            groupHeaders(3, 1) = "271"
            groupHeaders(4, 1) = "255"
            groupHeaders(5, 1) = "268"
            groupHeaders(6, 1) = "258"
            groupHeaders(7, 1) = "267"
            groupHeaders(8, 1) = "269"
            groupHeaders(9, 1) = "256"
            groupHeaders(10, 1) = "275"
            groupHeaders(11, 1) = "270"
            groupHeaders(12, 1) = "278"
            groupHeaders(13, 1) = "257"
            groupHeaders(14, 1) = "274"
            lvSkills.BeginUpdate()
            lvSkills.Items.Clear()
            For Each cSkill As EveHQ.Core.Skills In EveHQ.Core.HQ.myPilot.PilotSkills
                Try
                    Dim newLine As New ListViewItem
                    newLine.Text = cSkill.Name
                    newLine.Name = cSkill.Name
                    Dim rankSubItem As New ListViewItem.ListViewSubItem
                    rankSubItem.Text = cSkill.Rank.ToString
                    rankSubItem.Name = cSkill.Rank.ToString
                    newLine.SubItems.Add(rankSubItem)
                    Dim levelSubItem As New ListViewItem.ListViewSubItem
                    levelSubItem.Text = cSkill.Level.ToString
                    levelSubItem.Name = cSkill.Level.ToString
                    newLine.SubItems.Add(levelSubItem)
                    Dim percent As Double
                    Dim partially As Boolean = False
                    If cSkill.Level = 5 Then
                        percent = 100
                    Else
                        If EveHQ.Core.HQ.myPilot.TrainingSkillID = cSkill.ID Then
                            percent = CDbl((cSkill.SP + EveHQ.Core.HQ.myPilot.TrainingCurrentSP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100)
                            If cSkill.SP + EveHQ.Core.HQ.myPilot.TrainingCurrentSP > cSkill.LevelUp(cSkill.Level) + 1 Then
                                partially = True
                            End If
                        Else
                            percent = (Math.Min(Math.Max(CDbl((cSkill.SP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100), 0), 100))
                            If cSkill.SP > cSkill.LevelUp(cSkill.Level) + 1 Then
                                partially = True
                            End If
                        End If
                    End If
                    ' Write percentage
                    Dim pcSubItem As New ListViewItem.ListViewSubItem
                    pcSubItem.Text = FormatNumber(percent, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                    pcSubItem.Tag = partially
                    pcSubItem.Name = percent.ToString
                    newLine.SubItems.Add(pcSubItem)
                    ' Write skillpoints
                    Dim SPSubItem As New ListViewItem.ListViewSubItem
                    SPSubItem.Name = cSkill.SP.ToString
                    SPSubItem.Text = FormatNumber(cSkill.SP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newLine.SubItems.Add(SPSubItem)
                    For skillGroup As Integer = 0 To 14
                        If cSkill.GroupID = groupHeaders(skillGroup, 1) Then
                            newLine.Group = lvSkills.Groups.Item(skillGroup)
                            groupHeaders(skillGroup, 2) = CStr(CDbl(groupHeaders(skillGroup, 2)) + cSkill.SP)
                            groupHeaders(skillGroup, 3) = CStr(CDbl(groupHeaders(skillGroup, 3)) + 1)
                            Exit For
                        End If
                    Next
                    ' Write time to next level - adjusted if 0 to put completed skills to the bottom
                    Dim TimeSubItem As New ListViewItem.ListViewSubItem
                    Dim currentTime As Long = 0
                    If EveHQ.Core.HQ.myPilot.TrainingSkillID = cSkill.ID Then
                        TimeSubItem.Text = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.HQ.myPilot.TrainingCurrentTime)
                        currentTime = EveHQ.Core.HQ.myPilot.TrainingCurrentTime
                    Else
                        TimeSubItem.Text = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, CType(EveHQ.Core.HQ.SkillListName(cSkill.Name), EveHQ.Core.SkillList), 0))
                        currentTime = CLng(EveHQ.Core.SkillFunctions.CalcTimeToLevel(EveHQ.Core.HQ.myPilot, CType(EveHQ.Core.HQ.SkillListName(cSkill.Name), EveHQ.Core.SkillList), 0))
                    End If
                    If currentTime = 0 Then currentTime = 9999999999
                    TimeSubItem.Name = currentTime.ToString
                    newLine.SubItems.Add(TimeSubItem)
                    newLine.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor))
                    ' Select colours for line background
                    If cSkill.Level = 5 Then
                        newLine.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor))
                    Else
                        If EveHQ.Core.HQ.myPilot.TrainingSkillID = cSkill.ID Then
                            Dim lvFont As Font = New Font(lvSkills.Font, FontStyle.Bold)
                            newLine.Font = lvFont
                            newLine.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor))
                        Else
                            If partially = True Then
                                newLine.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor))
                            End If
                        End If
                    End If
                    lvSkills.Items.Add(newLine)
                Catch e As Exception
                    Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Pilot Name: " & EveHQ.Core.HQ.myPilot.Name
                    msg &= "Training Skill Name: " & cSkill.Name & ControlChars.CrLf
                    msg &= "Pilot Has Skill: " & EveHQ.Core.HQ.myPilot.PilotSkills.Contains(cSkill.Name)
                    msg &= "EveHQ Has Skill: " & EveHQ.Core.HQ.SkillListName.Contains(cSkill.Name)
                    MessageBox.Show(msg, "Error Displaying Skill Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Next
            lvSkills.EndUpdate()
        Else
            lvPilot.Items.Clear()
            lvImplants.Items.Clear()
            lvTraining.Items.Clear()
            lvAttributes.Items.Clear()
            lvSkills.Items.Clear()
            ' Get image from cache
            If EveHQ.Core.HQ.myPilot.ID = "" Then
                picPilot.Image = My.Resources.noitem
            End If
        End If
    End Sub
    Public Sub UpdateSkillInfo()
        If EveHQ.Core.HQ.myPilot.PilotData.InnerText <> "" Then
            If EveHQ.Core.HQ.myPilot.Training = True Then
                lvPilot.Items(6).SubItems(1).Text = (FormatNumber(EveHQ.Core.HQ.myPilot.SkillPoints + EveHQ.Core.HQ.myPilot.TrainingCurrentSP, 0, , , TriState.True))
                Dim cSkill As EveHQ.Core.Skills = CType(EveHQ.Core.HQ.myPilot.PilotSkills(EveHQ.Core.SkillFunctions.SkillIDToName(EveHQ.Core.HQ.myPilot.TrainingSkillID)), Core.Skills)

                Dim percent As Double
                If cSkill.Level = 5 Then
                    percent = 100
                Else
                    If EveHQ.Core.HQ.myPilot.TrainingSkillID = cSkill.ID Then
                        percent = CDbl((cSkill.SP + EveHQ.Core.HQ.myPilot.TrainingCurrentSP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100)
                    Else
                        percent = (Math.Min(Math.Max(CDbl((cSkill.SP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100), 0), 100))
                    End If
                End If
                lvSkills.Items(cSkill.Name).SubItems(3).Text = FormatNumber(percent, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lvSkills.Items(cSkill.Name).SubItems(3).Name = percent.ToString
                lvSkills.Items(cSkill.Name).SubItems(4).Text = FormatNumber(cSkill.SP + EveHQ.Core.HQ.myPilot.TrainingCurrentSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                lvSkills.Items(cSkill.Name).SubItems(4).Name = CStr(cSkill.SP + EveHQ.Core.HQ.myPilot.TrainingCurrentSP)
                lvSkills.Items(cSkill.Name).SubItems(5).Text = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.HQ.myPilot.TrainingCurrentTime)
                lvSkills.Items(cSkill.Name).SubItems(5).Name = CStr(EveHQ.Core.HQ.myPilot.TrainingCurrentTime)
                Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(EveHQ.Core.HQ.myPilot.TrainingEndTime)
                lvTraining.Items(3).SubItems(1).Text = Format(localdate, "ddd") & " " & localdate & " (" & EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.HQ.myPilot.TrainingCurrentTime) & ")"
                If EveHQ.Core.HQ.myPilot.TrainingCurrentTime = 0 Then
                    lvTraining.Items(0).SubItems(1).Text = ("Finished")
                    lvTraining.Items(1).Text = "Just Finished Training"
                    lvTraining.Items(2).Text = "Trained To"
                End If
            End If

            ' Check Cache details!
            Dim cacheDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(EveHQ.Core.HQ.myPilot.CacheExpirationTime)
            Dim cacheTimeLeft As TimeSpan = cacheDate - Now
            Dim cacheText As String = (Format(cacheDate, "ddd") & " " & cacheDate & " (" & EveHQ.Core.SkillFunctions.CacheTimeToString(cacheTimeLeft.TotalSeconds) & ")")
            If cacheDate < Now Then
                lvTraining.Items(4).ForeColor = Color.Green
                EveHQ.Core.HQ.UpdateAvailable = True
            Else
                lvTraining.Items(4).ForeColor = Color.Red
                EveHQ.Core.HQ.UpdateAvailable = False
            End If
            lvTraining.Items(4).SubItems(1).Text = cacheText
        End If

    End Sub
    Private Sub btnCharXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCharXML.Click
        If EveHQ.Core.HQ.myPilot.PilotData.OuterXml = "" Then
            MessageBox.Show("Please select the pilot whose XML you wish to view", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim newReport As New frmReportViewer
            Call EveHQ.Core.Reports.GenerateCharXML(EveHQ.Core.HQ.myPilot)
            newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\CharXML (" & EveHQ.Core.HQ.myPilot.Name & ").xml")
            frmEveHQ.DisplayReport(newReport, "Imported Character XML - " & EveHQ.Core.HQ.myPilot.Name)
        End If
    End Sub
    Private Sub btnTrainingXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrainingXML.Click
        If EveHQ.Core.HQ.myPilot.PilotTrainingData.OuterXml = "" Then
            MessageBox.Show("Please select the pilot whose XML you wish to view", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim newReport As New frmReportViewer
            Call EveHQ.Core.Reports.GenerateTrainXML(EveHQ.Core.HQ.myPilot)
            newReport.wbReport.Navigate(EveHQ.Core.HQ.reportFolder & "\TrainingXML (" & EveHQ.Core.HQ.myPilot.Name & ").xml")
            frmEveHQ.DisplayReport(newReport, "Imported Training XML - " & EveHQ.Core.HQ.myPilot.Name)
        End If
    End Sub

    Private Sub LoadPortrait()
        ' If double-clicked, see if we can get it from the eve portrait folder
        For folder As Integer = 1 To 4
            Dim folderName As String = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder) & "\cache\Pictures\Portraits"
            If My.Computer.FileSystem.DirectoryExists(folderName) = True Then
                For Each foundFile As String In My.Computer.FileSystem.GetFiles(folderName, FileIO.SearchOption.SearchTopLevelOnly, "*.png")
                    If foundFile.Contains(EveHQ.Core.HQ.myPilot.ID & "_") = True Then
                        ' Get the dimensions of the file
                        Dim myFile As New FileInfo(foundFile)
                        Dim fileData As String() = myFile.Name.Split(New Char(1) {CChar("_"), CChar(".")})
                        If CInt(fileData(1)) >= 128 Then
                            picPilot.ImageLocation = foundFile
                            Exit Sub
                        End If
                    End If
                Next
            End If
        Next
        ' If we haven't found a matching image then get it from the server
        If EveHQ.Core.HQ.myPilot.ID <> "" Then
            picPilot.ImageLocation = "http://img.eve.is/serv.asp?s=64&c=" & EveHQ.Core.HQ.myPilot.ID
        End If
    End Sub

    Private Sub lvAttributes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvAttributes.DoubleClick
        Dim msg As String = ""
        Dim caption As String = ""

        Select Case lvAttributes.SelectedItems(0).Text
            Case ("Charisma")
                msg = "Starting Attribute: " & EveHQ.Core.HQ.myPilot.CAtt & vbCrLf
                msg = msg & "Basic Learning: " & EveHQ.Core.HQ.myPilot.LCAtt & vbCrLf
                msg = msg & "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALCAtt & vbCrLf
                msg = msg & "Implant: " & EveHQ.Core.HQ.myPilot.CImplant & vbCrLf
                msg = msg & "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSCAtt & vbCrLf
                msg = msg & "TOTAL: " & EveHQ.Core.HQ.myPilot.CAttT
                caption = "Charisma Attribute Breakdown"
            Case ("Intelligence")
                msg = "Starting Attribute: " & EveHQ.Core.HQ.myPilot.IAtt & vbCrLf
                msg = msg & "Basic Learning: " & EveHQ.Core.HQ.myPilot.LIAtt & vbCrLf
                msg = msg & "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALIAtt & vbCrLf
                msg = msg & "Implant: " & EveHQ.Core.HQ.myPilot.IImplant & vbCrLf
                msg = msg & "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSIAtt & vbCrLf
                msg = msg & "TOTAL: " & EveHQ.Core.HQ.myPilot.IAttT
                caption = "Intelligence Attribute Breakdown"
            Case ("Memory")
                msg = "Starting Attribute: " & EveHQ.Core.HQ.myPilot.MAtt & vbCrLf
                msg = msg & "Basic Learning: " & EveHQ.Core.HQ.myPilot.LMAtt & vbCrLf
                msg = msg & "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALMAtt & vbCrLf
                msg = msg & "Implant: " & EveHQ.Core.HQ.myPilot.MImplant & vbCrLf
                msg = msg & "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSMAtt & vbCrLf
                msg = msg & "TOTAL: " & EveHQ.Core.HQ.myPilot.MAttT
                caption = "Memory Attribute Breakdown"
            Case ("Perception")
                msg = "Starting Attribute: " & EveHQ.Core.HQ.myPilot.PAtt & vbCrLf
                msg = msg & "Basic Learning: " & EveHQ.Core.HQ.myPilot.LPAtt & vbCrLf
                msg = msg & "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALPAtt & vbCrLf
                msg = msg & "Implant: " & EveHQ.Core.HQ.myPilot.PImplant & vbCrLf
                msg = msg & "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSPAtt & vbCrLf
                msg = msg & "TOTAL: " & EveHQ.Core.HQ.myPilot.PAttT
                caption = "Perception Attribute Breakdown"
            Case ("Willpower")
                msg = "Starting Attribute: " & EveHQ.Core.HQ.myPilot.WAtt & vbCrLf
                msg = msg & "Basic Learning: " & EveHQ.Core.HQ.myPilot.LWAtt & vbCrLf
                msg = msg & "Advanced Learning: " & EveHQ.Core.HQ.myPilot.ALWAtt & vbCrLf
                msg = msg & "Implant: " & EveHQ.Core.HQ.myPilot.WImplant & vbCrLf
                msg = msg & "Learning Skill: " & EveHQ.Core.HQ.myPilot.LSWAtt & vbCrLf
                msg = msg & "TOTAL: " & EveHQ.Core.HQ.myPilot.WAttT
                caption = "Willpower Attribute Breakdown"
        End Select
        MessageBox.Show(msg, caption)
    End Sub

    Private Sub frmPilot_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call UpdatePilotInfo()
    End Sub

    Private Sub mnuViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewDetails.Click
        Dim skillID As String
        skillID = mnuSkillName.Tag.ToString
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub

    Private Sub ctxSkills_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSkills.Opening
        If lvSkills.SelectedItems.Count <> 0 Then
            Dim skillName As String = ""
            Dim skillID As String = ""
            skillName = lvSkills.SelectedItems(0).Text
            skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
            mnuSkillName.Text = skillName
            mnuSkillName.Tag = skillID
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub lvSkills_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvSkills.ColumnClick
        If CInt(lvSkills.Tag) = e.Column Then
            Me.lvSkills.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Ascending)
            lvSkills.Tag = -1
        Else
            Me.lvSkills.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Descending)
            lvSkills.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvSkills.Sort()
    End Sub

    Private Sub lvSkills_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvSkills.DoubleClick
        Dim skillID As String
        skillID = EveHQ.Core.SkillFunctions.SkillNameToID(lvSkills.SelectedItems(0).Text)
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub

#Region "Implant Related Stuff"

    Private Sub chkManualImplants_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkManualImplants.CheckedChanged
        If chkManualImplants.Checked = True Then
            EveHQ.Core.HQ.myPilot.UseManualImplants = True
            btnEditImplants.Enabled = True
        Else
            EveHQ.Core.HQ.myPilot.UseManualImplants = False
            btnEditImplants.Enabled = False
        End If
        Call Me.UpdatePilotInfo()
    End Sub

    Private Sub btnEditImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditImplants.Click
        frmEditImplants.ShowDialog()
        Call Me.UpdatePilotInfo()
    End Sub

    Private Sub UpdateImplants()
        lvImplants.Items(0).SubItems(1).Text = ("+" & EveHQ.Core.HQ.myPilot.CImplant)
        lvImplants.Items(1).SubItems(1).Text = ("+" & EveHQ.Core.HQ.myPilot.IImplant)
        lvImplants.Items(2).SubItems(1).Text = ("+" & EveHQ.Core.HQ.myPilot.MImplant)
        lvImplants.Items(3).SubItems(1).Text = ("+" & EveHQ.Core.HQ.myPilot.PImplant)
        lvImplants.Items(4).SubItems(1).Text = ("+" & EveHQ.Core.HQ.myPilot.WImplant)
    End Sub

#End Region

    Private Sub chkSkillGroups_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSkillGroups.CheckedChanged
        If chkSkillGroups.Checked = True Then
            lvSkills.ShowGroups = True
        Else
            lvSkills.ShowGroups = False
        End If
    End Sub

    Private Sub mnuCtxPicGetPortraitFromServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCtxPicGetPortraitFromServer.Click
        If EveHQ.Core.HQ.myPilot.ID <> "" Then
            picPilot.ImageLocation = "http://img.eve.is/serv.asp?s=256&c=" & EveHQ.Core.HQ.myPilot.ID
        End If
    End Sub

    Private Sub mnuCtxPicGetPortraitFromLocal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCtxPicGetPortraitFromLocal.Click
        ' If double-clicked, see if we can get it from the eve portrait folder
        For folder As Integer = 1 To 4
            Dim folderName As String = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder) & "\cache\Pictures\Portraits"
            If My.Computer.FileSystem.DirectoryExists(folderName) = True Then
                For Each foundFile As String In My.Computer.FileSystem.GetFiles(folderName, FileIO.SearchOption.SearchTopLevelOnly, "*.png")
                    If foundFile.Contains(EveHQ.Core.HQ.myPilot.ID & "_") = True Then
                        ' Get the dimensions of the file
                        Dim myFile As New FileInfo(foundFile)
                        Dim fileData As String() = myFile.Name.Split(New Char(1) {CChar("_"), CChar(".")})
                        If CInt(fileData(1)) >= 128 And CInt(fileData(1)) <= 256 Then
                            picPilot.ImageLocation = foundFile
                            Exit Sub
                        End If
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub mnuSavePortrait_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSavePortrait.Click
        Dim imgFilename As String = "i" & EveHQ.Core.HQ.myPilot.ID & ".png"
        picPilot.Image.Save(EveHQ.Core.HQ.cacheFolder & "\" & imgFilename)
    End Sub

    Private Sub mnuForceTraining_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuForceTraining.Click
        Dim skillID As String
        skillID = mnuSkillName.Tag.ToString
        If EveHQ.Core.SkillFunctions.ForceSkillTraining(EveHQ.Core.HQ.myPilot, skillID, False) = True Then
            Call Me.UpdatePilotInfo()
            If frmTraining.IsHandleCreated = True Then
                Call frmTraining.RefreshAllTrainingQueues()
                Call frmTraining.LoadSkillTree()
            End If
        End If
    End Sub

#Region "OwnerDrawn Routines For SkillList"
    Private Sub lvSkills_DrawColumnHeader(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles lvSkills.DrawColumnHeader
        e.DrawDefault = True
    End Sub
    Private Sub lvSkills_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvSkills.DrawItem
        If Not (e.State And ListViewItemStates.Selected) = 0 Then
            ' Draw the background for a selected item.
            e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds)
            e.Graphics.DrawRectangle(New Pen(Color.DarkBlue), e.Bounds)
            lvSkills.Items(0).Text = lvSkills.Items(0).Text
        Else
            ' Draw the background for an unselected item.
            ' Default brush
            Dim brush As New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
            If e.Item.SubItems(2).Text = "5" Then
                ' If skill @ Level 5
                brush = New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
            Else
                If e.Item.Font.Bold = True Then
                    ' If skill is training
                    brush = New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
                Else
                    If CBool(e.Item.SubItems(3).Tag) = True Then
                        ' If partially trained
                        brush = New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
                    End If
                End If
            End If
            Try
                e.Graphics.FillRectangle(brush, e.Bounds)
            Finally
                brush.Dispose()
            End Try
        End If
        Dim flags As TextFormatFlags = TextFormatFlags.VerticalCenter
        e.DrawText(flags)
    End Sub
    Private Sub lvSkills_DrawSubItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvSkills.DrawSubItem
        If e.ColumnIndex > 0 Then
            If e.ColumnIndex = 2 Then
                ' Establish image
                Select Case e.Item.SubItems(2).Text
                    Case "0"
                        e.Graphics.DrawImage(My.Resources.level0, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
                        'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level0.Width, e.SubItem.Bounds.Location.Y)
                    Case "1"
                        e.Graphics.DrawImage(My.Resources.level1, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
                        'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level1.Width, e.SubItem.Bounds.Location.Y)
                    Case "2"
                        e.Graphics.DrawImage(My.Resources.level2, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
                        'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level2.Width, e.SubItem.Bounds.Location.Y)
                    Case "3"
                        e.Graphics.DrawImage(My.Resources.level3, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
                        'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level3.Width, e.SubItem.Bounds.Location.Y)
                    Case "4"
                        e.Graphics.DrawImage(My.Resources.level4, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
                        'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level4.Width, e.SubItem.Bounds.Location.Y)
                    Case "5"
                        e.Graphics.DrawImage(My.Resources.level5, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
                        'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level5.Width, e.SubItem.Bounds.Location.Y)
                End Select
            Else
                Dim flags As TextFormatFlags = TextFormatFlags.VerticalCenter Or TextFormatFlags.HidePrefix Or TextFormatFlags.NoPadding
                e.SubItem.Font = e.Item.Font
                e.DrawText(flags)
            End If
        End If
    End Sub
    Private Sub lvSkills_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles lvSkills.MouseMove
        Dim item As ListViewItem = lvSkills.GetItemAt(e.X, e.Y)
        If item IsNot Nothing AndAlso item.Tag Is Nothing Then
            lvSkills.Invalidate(item.GetBounds(ItemBoundsPortion.Entire), False)
            'item.Tag = "tagged"
        End If
    End Sub
    'Private Sub lvSkills_Invalidated(ByVal sender As Object, ByVal e As InvalidateEventArgs) Handles lvSkills.Invalidated
    '    For Each item As ListViewItem In lvSkills.Items
    '        If item Is Nothing Then Return
    '        item.Tag = Nothing
    '    Next
    'End Sub

    Private Sub chkGraphicalView_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGraphicalView.CheckedChanged
        lvSkills.OwnerDraw = chkGraphicalView.Checked
    End Sub

#End Region

End Class

