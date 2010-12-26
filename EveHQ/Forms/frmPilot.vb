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
Imports DotNetLib.Windows.Forms
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions

Public Class frmPilot
    Dim TrainingSkill As ContainerListViewItem
    Dim TrainingGroup As ContainerListViewItem
    Dim displayPilot As New EveHQ.Core.Pilot
    Dim cDisplayPilotName As String = ""

    Public Property DisplayPilotName() As String
        Get
            Return displayPilot.Name
        End Get
        Set(ByVal value As String)
            cDisplayPilotName = value
            If cboPilots.Items.Contains(value) Then
                cboPilots.SelectedItem = value
            End If
        End Set
    End Property

    Private Sub frmPilot_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        EveHQ.Core.StandingsCacheDecoder.SaveStandings()
    End Sub

    Private Sub frmPilot_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call UpdatePilots()
    End Sub

    Public Sub UpdatePilots()

        ' Save old Pilot info
        Dim oldPilot As String = ""
        If cboPilots.SelectedItem IsNot Nothing Then
            oldPilot = cboPilots.SelectedItem.ToString
        End If

        ' Update the pilots combo box
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next
        cboPilots.EndUpdate()

        If EveHQ.Core.HQ.AllStandings.Count = 0 Then
            Call EveHQ.Core.StandingsCacheDecoder.LoadStandings()
        End If
        Call Me.UpdateOwners()
        If cboOwner.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = True Then
            cboOwner.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
        Else
            If cboOwner.Items.Count > 0 Then
                cboOwner.SelectedIndex = 0
            End If
        End If

        ' Select a pilot
        If cDisplayPilotName <> "" Then
            If cboPilots.Items.Contains(cDisplayPilotName) = True Then
                cboPilots.SelectedItem = cDisplayPilotName
            Else
                cboPilots.SelectedIndex = 0
            End If
        Else
            If oldPilot = "" Then
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = True Then
                        cboPilots.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
                    Else
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            Else
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(oldPilot) = True Then
                        cboPilots.SelectedItem = oldPilot
                    Else
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            End If
        End If

    End Sub

    Public Sub UpdatePilotInfo()
        ' Check if the pilot has had data and therefore able to be displayed properly

        If displayPilot.PilotSkills.Count > 0 Then

            ' Get image from cache
            Try
                Dim imgFilename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, displayPilot.ID & ".png")
                If My.Computer.FileSystem.FileExists(imgFilename) = True Then
                    picPilot.ImageLocation = imgFilename
                Else
                    picPilot.Image = My.Resources.noitem
                End If

                Call EveHQ.Core.PilotParseFunctions.SwitchImplants(displayPilot)

            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
                MessageBox.Show(msg, "Error Retrieving Cached Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Information
            Try
                lvPilot.Items.Clear()
                Dim newCharItem As New ListViewItem
                newCharItem.Name = "Name" : newCharItem.Text = "Name" : newCharItem.SubItems.Add(displayPilot.Name)
                lvPilot.Items.Add(newCharItem)
                newCharItem = New ListViewItem
                newCharItem.Name = "ID" : newCharItem.Text = "ID" : newCharItem.SubItems.Add(displayPilot.ID)
                lvPilot.Items.Add(newCharItem)
                newCharItem = New ListViewItem
                newCharItem.Name = "Race" : newCharItem.Text = "Race" : newCharItem.SubItems.Add(displayPilot.Race & " (" & displayPilot.Blood & ")")
                lvPilot.Items.Add(newCharItem)
                newCharItem = New ListViewItem
                newCharItem.Name = "Corp" : newCharItem.Text = "Corp" : newCharItem.SubItems.Add(displayPilot.Corp)
                lvPilot.Items.Add(newCharItem)
                newCharItem = New ListViewItem
                newCharItem.Name = "Wealth" : newCharItem.Text = "Wealth" : newCharItem.SubItems.Add(FormatNumber(displayPilot.Isk, 2, , , TriState.True))
                lvPilot.Items.Add(newCharItem)
                newCharItem = New ListViewItem
                newCharItem.Name = "Skill Points" : newCharItem.Text = "Skill Points" : newCharItem.SubItems.Add(FormatNumber(displayPilot.SkillPoints + EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(displayPilot), 0, , , TriState.True))
                lvPilot.Items.Add(newCharItem)
                newCharItem = New ListViewItem
                newCharItem.Name = "Clone" : newCharItem.Text = "Clone" : newCharItem.SubItems.Add(displayPilot.CloneName & " (" & FormatNumber(displayPilot.CloneSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " SP)")
                lvPilot.Items.Add(newCharItem)
                ' Check Clone
                If (displayPilot.SkillPoints + displayPilot.TrainingCurrentSP) > CLng(displayPilot.CloneSP) Then
                    lvPilot.Items("Clone").ForeColor = Color.Red
                Else
                    lvPilot.Items("Clone").ForeColor = Color.Black
                End If
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Implants
            Try
                lvImplants.Items.Clear()
                lvImplants.Items.Add("Charisma")
                lvImplants.Items(0).SubItems.Add("+" & displayPilot.CImplant)
                lvImplants.Items.Add("Intelligence")
                lvImplants.Items(1).SubItems.Add("+" & displayPilot.IImplant)
                lvImplants.Items.Add("Memory")
                lvImplants.Items(2).SubItems.Add("+" & displayPilot.MImplant)
                lvImplants.Items.Add("Perception")
                lvImplants.Items(3).SubItems.Add("+" & displayPilot.PImplant)
                lvImplants.Items.Add("Willpower")
                lvImplants.Items(4).SubItems.Add("+" & displayPilot.WImplant)
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Implants", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Implant method
            If displayPilot.UseManualImplants = True Then
                Me.chkManualImplants.Checked = True
            Else
                Me.chkManualImplants.Checked = False
            End If

            ' Display Attributes
            Try
                lvAttributes.Items.Clear()
                lvAttributes.Items.Add("Charisma")
                lvAttributes.Items(0).SubItems.Add("+" & FormatNumber(displayPilot.CAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Intelligence")
                lvAttributes.Items(1).SubItems.Add("+" & FormatNumber(displayPilot.IAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Memory")
                lvAttributes.Items(2).SubItems.Add("+" & FormatNumber(displayPilot.MAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Perception")
                lvAttributes.Items(3).SubItems.Add("+" & FormatNumber(displayPilot.PAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvAttributes.Items.Add("Willpower")
                lvAttributes.Items(4).SubItems.Add("+" & FormatNumber(displayPilot.WAttT, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Attributes", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            Try
                For a As Integer = 0 To 4
                    Dim msg As String = ""
                    Select Case lvAttributes.Items(a).Text
                        Case ("Charisma")
                            msg = "Charisma Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & displayPilot.CAtt & vbCrLf
                            msg &= "Implant: " & displayPilot.CImplant & vbCrLf
                            msg &= "TOTAL: " & displayPilot.CAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Intelligence")
                            msg = "Intelligence Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & displayPilot.IAtt & vbCrLf
                            msg &= "Implant: " & displayPilot.IImplant & vbCrLf
                            msg &= "TOTAL: " & displayPilot.IAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Memory")
                            msg = "Memory Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & displayPilot.MAtt & vbCrLf
                            msg &= "Implant: " & displayPilot.MImplant & vbCrLf
                            msg &= "TOTAL: " & displayPilot.MAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Perception")
                            msg = "Perception Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & displayPilot.PAtt & vbCrLf
                            msg &= "Implant: " & displayPilot.PImplant & vbCrLf
                            msg &= "TOTAL: " & displayPilot.PAttT
                            lvAttributes.Items(a).ToolTipText = msg
                        Case ("Willpower")
                            msg = "Willpower Attribute Breakdown" & vbCrLf & vbCrLf
                            msg &= "Starting Attribute: " & displayPilot.WAtt & vbCrLf
                            msg &= "Implant: " & displayPilot.WImplant & vbCrLf
                            msg &= "TOTAL: " & displayPilot.WAttT
                            lvAttributes.Items(a).ToolTipText = msg
                    End Select
                Next
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
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
                If displayPilot.Training = True Then
                    Dim currentSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)), Core.PilotSkill)
                    If displayPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)) = True Then
                        currentSkill = CType(displayPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)), Core.PilotSkill)
                    Else
                        MessageBox.Show("Missing the training skill from the skills!!")
                    End If
                    lvTraining.Items(0).SubItems.Add("Yes")
                    lvTraining.Items(1).SubItems.Add(currentSkill.Name & " (Level " & EveHQ.Core.SkillFunctions.Roman(displayPilot.TrainingSkillLevel) & ")")
                    lvTraining.Items(2).SubItems.Add("Rank " & currentSkill.Rank & " @ " & FormatNumber(EveHQ.Core.SkillFunctions.CalculateSPRate(displayPilot, EveHQ.Core.HQ.SkillListID(currentSkill.ID)), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " SP/Hr")
                    Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(displayPilot.TrainingEndTime)
                    lvTraining.Items(3).SubItems.Add(Format(localdate, "ddd") & " " & localdate & " (" & EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime) & ")")
                Else
                    lvTraining.Items(0).SubItems.Add("No")
                    lvTraining.Items(1).SubItems.Add("n/a")
                    lvTraining.Items(2).SubItems.Add("n/a")
                    lvTraining.Items(3).SubItems.Add("n/a")
                End If
                Dim cacheDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(displayPilot.CacheExpirationTime)
                Dim cacheItem As New ListViewItem.ListViewSubItem
                cacheItem.Text = (Format(cacheDate, "ddd") & " " & cacheDate)
                If cacheDate < Now Then
                    lvTraining.Items(4).ForeColor = Color.Green
                    EveHQ.Core.HQ.APIUpdateAvailable = True
                Else
                    lvTraining.Items(4).ForeColor = Color.Red
                    EveHQ.Core.HQ.APIUpdateAvailable = False
                End If
                lvTraining.Items(4).SubItems.Add(cacheItem)
            Catch e As Exception
                ' Possible cache corruption
                If frmEveHQ.CacheErrorHandler() = True Then Exit Sub
            End Try

            ' Display skills & stuff
            Call Me.DisplaySkills()
            Call Me.DisplayCertificates()
            Call Me.DisplaySkillQueue()

            ' Update Standings stuff
            If cboOwner.Items.Contains(cboPilots.SelectedItem.ToString) Then
                cboOwner.SelectedItem = cboPilots.SelectedItem.ToString
            End If

        Else
            lvPilot.Items.Clear()
            lvImplants.Items.Clear()
            lvTraining.Items.Clear()
            lvAttributes.Items.Clear()
            clvSkills.Items.Clear()
            clvCerts.Items.Clear()
            ' Get image from cache
            If displayPilot.ID = "" Then
                picPilot.Image = My.Resources.noitem
            End If
        End If
    End Sub

    Private Sub DisplaySkills()
        Dim maxGroups As Integer = 16
        Dim groupHeaders(maxGroups, 3) As String
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
        groupHeaders(11, 0) = "Planet Management"
        groupHeaders(12, 0) = "Science"
        groupHeaders(13, 0) = "Social"
        groupHeaders(14, 0) = "Spaceship Command"
        groupHeaders(15, 0) = "Subsystems"
        groupHeaders(16, 0) = "Trade"
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
        groupHeaders(11, 1) = "1044"
        groupHeaders(12, 1) = "270"
        groupHeaders(13, 1) = "278"
        groupHeaders(14, 1) = "257"
        groupHeaders(15, 1) = "989"
        groupHeaders(16, 1) = "274"

        ' Set up Groups
        clvSkills.Items.Clear()
        clvSkills.Refresh()
        clvSkills.BeginUpdate()
        clvSkills.ItemSelectedColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor))

        Dim groupStructure As New SortedList
        If chkGroupSkills.Checked = True Then
            For group As Integer = 0 To maxGroups
                Dim newCLVGroup As New ContainerListViewItem
                newCLVGroup.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor))
                newCLVGroup.ForeColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor))
                newCLVGroup.Text = groupHeaders(group, 0)
                clvSkills.Items.Add(newCLVGroup)
                groupStructure.Add(groupHeaders(group, 1), newCLVGroup)
            Next
        End If

        ' Set up items
        Dim TrainingBonus As Double = 1
        Dim TrainingThreshold As Long = 1600000

        If displayPilot.SkillPoints + displayPilot.TrainingCurrentSP < TrainingThreshold Then
            TrainingBonus = 2
        End If
        For Each cSkill As EveHQ.Core.PilotSkill In displayPilot.PilotSkills
            Try
                Dim groupCLV As ContainerListViewItem = CType(groupStructure(cSkill.GroupID), ContainerListViewItem)
                Dim newCLVItem As New ContainerListViewItem
                newCLVItem.Text = cSkill.Name
                newCLVItem.ForeColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
                If chkGroupSkills.Checked = True Then
                    groupCLV.Items.Add(newCLVItem)
                Else
                    clvSkills.Items.Add(newCLVItem)
                End If
                newCLVItem.SubItems(1).Text = cSkill.Rank.ToString
                newCLVItem.SubItems(1).Tag = cSkill.Rank

                Dim pb As New PictureBox
                pb.Image = CType(My.Resources.ResourceManager.GetObject("level" & cSkill.Level.ToString), Image)
                pb.Width = 48 : pb.Height = 8
                newCLVItem.SubItems(2).ItemControl = pb
                newCLVItem.SubItems(2).Tag = cSkill.Level

                Dim percent As Double
                Dim partially As Boolean = False
                If cSkill.Level = 5 Then
                    percent = 100
                Else
                    If displayPilot.TrainingSkillID = cSkill.ID Then
                        percent = CDbl((cSkill.SP + displayPilot.TrainingCurrentSP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100)
                        If cSkill.SP + displayPilot.TrainingCurrentSP > cSkill.LevelUp(cSkill.Level) + 1 Then
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
                newCLVItem.SubItems(3).Tag = FormatNumber(percent, 0)
                newCLVItem.SubItems(3).Text = FormatNumber(percent, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"

                ' Write skillpoints
                newCLVItem.SubItems(4).Tag = cSkill.SP
                newCLVItem.SubItems(4).Text = FormatNumber(cSkill.SP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

                If chkGroupSkills.Checked = True Then
                    For skillGroup As Integer = 0 To maxGroups
                        If cSkill.GroupID = groupHeaders(skillGroup, 1) Then
                            'newLine.Group = lvSkills.Groups.Item(skillGroup)
                            groupHeaders(skillGroup, 2) = CStr(CDbl(groupHeaders(skillGroup, 2)) + cSkill.SP)
                            groupHeaders(skillGroup, 3) = CStr(CDbl(groupHeaders(skillGroup, 3)) + 1)
                            groupCLV.Text = groupHeaders(skillGroup, 0) & " (" & groupHeaders(skillGroup, 3) & " skills)"
                            groupCLV.Tag = groupCLV.Text
                            groupCLV.SubItems(4).Text = FormatNumber(groupHeaders(skillGroup, 2), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                            groupCLV.SubItems(4).Tag = groupHeaders(skillGroup, 2)
                            Exit For
                        End If
                    Next
                End If
                ' Write time to next level - adjusted if 0 to put completed skills to the bottom
                Dim TimeSubItem As New ListViewItem.ListViewSubItem
                Dim currentTime As Long = 0
                If displayPilot.TrainingSkillID = cSkill.ID Then
                    TimeSubItem.Text = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    currentTime = displayPilot.TrainingCurrentTime
                    pb.Image = CType(My.Resources.ResourceManager.GetObject("level" & (cSkill.Level + 1).ToString & "_act"), Image)
                    TrainingSkill = newCLVItem
                    TrainingGroup = groupCLV
                Else
                    currentTime = CLng(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, EveHQ.Core.HQ.SkillListID(cSkill.ID), 0, ))
                    TimeSubItem.Text = EveHQ.Core.SkillFunctions.TimeToString(currentTime)
                End If
                If currentTime = 0 Then currentTime = 9999999999
                newCLVItem.SubItems(5).Tag = currentTime.ToString
                newCLVItem.SubItems(5).Text = TimeSubItem.Text
                newCLVItem.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor))

                ' Select colours for line background
                If cSkill.Level = 5 Then
                    newCLVItem.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor))
                Else
                    If displayPilot.TrainingSkillID = cSkill.ID Then
                        Dim lvFont As Font = New Font(clvSkills.Font, FontStyle.Bold)
                        newCLVItem.Font = lvFont
                        newCLVItem.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor))
                        If chkGroupSkills.Checked = True Then
                            groupCLV.Text = groupCLV.Tag.ToString & " - Training"
                            groupCLV.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
                        End If
                    Else
                        If partially = True Then
                            newCLVItem.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor))
                        End If
                    End If
                End If

            Catch e As Exception
                If frmEveHQ.CacheErrorHandler() = True Then Exit Sub
            End Try
        Next

        ' Remove empty groups
        If chkGroupSkills.Checked = True Then
            Dim SG As New ContainerListViewItem
            Dim SGNo As Integer = 0
            Do
                SG = clvSkills.Items(SGNo)
                If SG.Items.Count = 0 Then
                    clvSkills.Items.Remove(SG)
                    SGNo -= 1
                End If
                SGNo += 1
            Loop Until SGNo = clvSkills.Items.Count
        End If

        clvSkills.Sort(0, SortOrder.Ascending, True)
        clvSkills.EndUpdate()
        If chkGroupSkills.Checked = True Then
            If TrainingGroup IsNot Nothing Then
                TrainingGroup.SubItems(4).Text = FormatNumber(CLng(TrainingGroup.SubItems(4).Tag) + displayPilot.TrainingCurrentSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                TrainingGroup.Text = TrainingGroup.Tag.ToString & " - Training"
                TrainingGroup.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
            End If
        End If
    End Sub

    Private Sub DisplayCertificates()

        Dim cCert As EveHQ.Core.Certificate

        ' Filter out the lower end certificates
        Dim certList As New SortedList
        For Each cCertID As String In displayPilot.Certificates
            If EveHQ.Core.HQ.Certificates.ContainsKey(cCertID) Then
                cCert = CType(EveHQ.Core.HQ.Certificates(cCertID), Core.Certificate)
                If certList.Contains(cCert.ClassID) = False Then
                    certList.Add(cCert.ClassID, cCert)
                Else
                    Dim storedGrade As Integer = CType(certList(cCert.ClassID), Core.Certificate).Grade
                    If cCert.Grade > storedGrade Then
                        certList(cCert.ClassID) = cCert
                    End If
                End If
            End If
        Next

        'Set up Groups

        clvCerts.Items.Clear()
        clvCerts.Refresh()
        clvCerts.BeginUpdate()
        clvCerts.Columns.Clear()
        clvCerts.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colCertificate, Me.colCertGrade, Me.colCertLevel})
        clvCerts.ItemSelectedColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor))

        Dim certGroups As New SortedList

        If chkGroupSkills.Checked = True Then
            For Each cCategory As EveHQ.Core.CertificateCategory In EveHQ.Core.HQ.CertificateCategories.Values
                Dim newCertGroup As New ContainerListViewItem
                newCertGroup.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor))
                newCertGroup.ForeColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor))
                newCertGroup.Text = cCategory.Name
                newCertGroup.Tag = 0
                certGroups.Add(cCategory.ID.ToString, newCertGroup)
                clvCerts.Items.Add(newCertGroup)
            Next
        End If

        'Set up items

        For Each cCert In certList.Values
            Dim certGroup As ContainerListViewItem = CType(certGroups(cCert.CategoryID.ToString), ContainerListViewItem)
            Dim newCLVItem As New ContainerListViewItem
            newCLVItem.Text = CType(EveHQ.Core.HQ.CertificateClasses(cCert.ClassID.ToString), EveHQ.Core.CertificateClass).Name
            newCLVItem.Tag = cCert.ID
            newCLVItem.ForeColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
            If chkGroupSkills.Checked = True Then
                certGroup.Items.Add(newCLVItem)
                certGroup.Tag = CInt(certGroup.Tag) + 1
            Else
                clvCerts.Items.Add(newCLVItem)
            End If
            newCLVItem.SubItems(1).Tag = cCert.Grade
            Select Case cCert.Grade
                Case 1
                    newCLVItem.SubItems(1).Text = "Basic"
                Case 2
                    newCLVItem.SubItems(1).Text = "Standard"
                Case 3
                    newCLVItem.SubItems(1).Text = "Improved"
                Case 4
                    newCLVItem.SubItems(1).Text = "Advanced"
                Case 5
                    newCLVItem.SubItems(1).Text = "Elite"
            End Select

            Dim pb As New PictureBox
            pb.Image = CType(My.Resources.ResourceManager.GetObject("level" & cCert.Grade.ToString), Image)
            pb.Width = 48 : pb.Height = 8
            newCLVItem.SubItems(2).ItemControl = pb
            newCLVItem.SubItems(2).Tag = cCert.Grade
        Next

        ' Add certificate count and remove empty groups
        If chkGroupSkills.Checked = True Then
            For Each certGroup As ContainerListViewItem In clvCerts.Items
                certGroup.Text &= " (" & certGroup.Tag.ToString & " certificates)"
            Next
            Dim SG As New ContainerListViewItem
            Dim SGNo As Integer = 0
            Do
                SG = clvCerts.Items(SGNo)
                If SG.Items.Count = 0 Then
                    clvCerts.Items.Remove(SG)
                    SGNo -= 1
                End If
                SGNo += 1
            Loop Until SGNo = clvCerts.Items.Count
        End If

        clvCerts.Sort(0, SortOrder.Ascending, True)
        clvCerts.EndUpdate()
    End Sub

    Private Sub DisplaySkillQueue()
        clvQueue.BeginUpdate()
        clvQueue.Items.Clear()
        If displayPilot.QueuedSkills IsNot Nothing Then
            For Each QueuedSkill As EveHQ.Core.PilotQueuedSkill In displayPilot.QueuedSkills.Values
                Dim newitem As New ContainerListViewItem
                newitem.Text = EveHQ.Core.SkillFunctions.SkillIDToName(QueuedSkill.SkillID.ToString)
                clvQueue.Items.Add(newitem)
                newitem.SubItems(1).Text = QueuedSkill.Level.ToString
                Dim startdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.StartTime)
                Dim enddate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(QueuedSkill.EndTime)
                newitem.SubItems(2).Text = Format(startdate, "ddd") & " " & startdate
                newitem.SubItems(3).Text = Format(enddate, "ddd") & " " & enddate
            Next
        End If
        clvQueue.AutoSizeColumnWidths(True)
        clvQueue.EndUpdate()
    End Sub

#Region "Skill Update Routine"
    Public Sub UpdateSkillInfo()
        If displayPilot.PilotSkills.Count <> 0 Then
            If displayPilot.Training = True Then
                lvPilot.Items("Skill Points").SubItems(1).Text = (FormatNumber(displayPilot.SkillPoints + displayPilot.TrainingCurrentSP, 0, , , TriState.True))
                If displayPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)) = True Then
                    Dim cSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)), Core.PilotSkill)
                    Dim percent As Double
                    If cSkill.Level = 5 Then
                        percent = 100
                    Else
                        If displayPilot.TrainingSkillID = cSkill.ID Then
                            percent = CDbl((cSkill.SP + displayPilot.TrainingCurrentSP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100)
                        Else
                            percent = (Math.Min(Math.Max(CDbl((cSkill.SP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100), 0), 100))
                        End If
                    End If
                    TrainingSkill.SubItems(3).Text = FormatNumber(percent, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                    TrainingSkill.SubItems(3).Tag = percent
                    TrainingSkill.SubItems(4).Text = FormatNumber(cSkill.SP + displayPilot.TrainingCurrentSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    TrainingSkill.SubItems(4).Tag = cSkill.SP
                    TrainingSkill.SubItems(5).Text = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    TrainingSkill.SubItems(5).Tag = displayPilot.TrainingCurrentTime
                    TrainingSkill.SubItems(2).ItemControl.Refresh()
                    If chkGroupSkills.Checked = True Then
                        TrainingGroup.SubItems(4).Text = FormatNumber(CLng(TrainingGroup.SubItems(4).Tag) + displayPilot.TrainingCurrentSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                        TrainingGroup.Text = TrainingGroup.Tag.ToString & " - Training"
                        TrainingGroup.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
                    End If
                    Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(displayPilot.TrainingEndTime)
                    lvTraining.Items(3).SubItems(1).Text = Format(localdate, "ddd") & " " & localdate & " (" & EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime) & ")"
                    If displayPilot.TrainingCurrentTime = 0 Then
                        lvTraining.Items(0).SubItems(1).Text = ("Finished")
                        lvTraining.Items(1).Text = "Just Finished Training"
                        lvTraining.Items(2).Text = "Trained To"
                    End If
                Else
                    ' Cache corruption here??
                    If frmEveHQ.CacheErrorHandler() = True Then Exit Sub
                End If
            End If

            ' Check Clone
            If (displayPilot.SkillPoints + displayPilot.TrainingCurrentSP) > CLng(displayPilot.CloneSP) Then
                lvPilot.Items("Clone").ForeColor = Color.Red
            Else
                lvPilot.Items("Clone").ForeColor = Color.Black
            End If

            ' Check Cache details!
            Dim cacheDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(displayPilot.CacheExpirationTime)
            Dim cacheTimeLeft As TimeSpan = cacheDate - Now
            Dim cacheText As String = (Format(cacheDate, "ddd") & " " & cacheDate & " (" & EveHQ.Core.SkillFunctions.CacheTimeToString(cacheTimeLeft.TotalSeconds) & ")")
            If cacheDate < Now Then
                lvTraining.Items(4).ForeColor = Color.Green
                EveHQ.Core.HQ.APIUpdateAvailable = True
            Else
                lvTraining.Items(4).ForeColor = Color.Red
                EveHQ.Core.HQ.APIUpdateAvailable = False
            End If
            lvTraining.Items(4).SubItems(1).Text = cacheText
        End If

    End Sub
#End Region

#Region "UI Routines"

    Private Sub mnuForceTraining_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuForceTraining.Click
        Dim skillID As String
        skillID = mnuSkillName.Tag.ToString
        If EveHQ.Core.SkillFunctions.ForceSkillTraining(displayPilot, skillID, False) = True Then
            Call Me.UpdatePilotInfo()
            If frmTraining.IsHandleCreated = True Then
                Call frmTraining.RefreshAllTrainingQueues()
                Call frmTraining.LoadSkillTree()
            End If
        End If
    End Sub

    Private Sub mnuViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewDetails.Click
        Dim skillID As String
        skillID = mnuSkillName.Tag.ToString
        frmSkillDetails.DisplayPilotName = displayPilot.Name
        Call frmSkillDetails.ShowSkillDetails(skillID)
    End Sub

    Private Sub ctxSkills_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSkills.Opening
        If clvSkills.SelectedItems.Count <> 0 Then
            If clvSkills.SelectedItems(0).Items.Count = 0 Then
                Dim skillName As String = ""
                Dim skillID As String = ""
                skillName = clvSkills.SelectedItems(0).Text
                skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
                mnuSkillName.Text = skillName
                mnuSkillName.Tag = skillID
            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub clvSkills_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles clvSkills.DoubleClick
        If clvSkills.SelectedItems.Count > 0 Then
            If clvSkills.SelectedItems(0).Depth = 2 Then
                Dim skillID As String = ""
                skillID = EveHQ.Core.SkillFunctions.SkillNameToID(clvSkills.SelectedItems(0).Text)
                frmSkillDetails.DisplayPilotName = displayPilot.Name
                Call frmSkillDetails.ShowSkillDetails(skillID)
            End If
        End If
    End Sub

    Private Sub btnCharXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCharXML.Click
        If displayPilot.PilotSkills.Count = 0 Then
            MessageBox.Show("Please select the pilot whose XML you wish to view", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim newReport As New frmReportViewer
            Call EveHQ.Core.Reports.GenerateCharXML(displayPilot)
            newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharXML (" & displayPilot.Name & ").xml"))
            frmEveHQ.DisplayReport(newReport, "Imported Character XML - " & displayPilot.Name)
        End If
    End Sub
    Private Sub btnTrainingXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrainingXML.Click
        If displayPilot.PilotSkills.Count = 0 Then
            MessageBox.Show("Please select the pilot whose XML you wish to view", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim newReport As New frmReportViewer
            Call EveHQ.Core.Reports.GenerateTrainXML(displayPilot)
            newReport.wbReport.Navigate(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML (" & displayPilot.Name & ").xml"))
            frmEveHQ.DisplayReport(newReport, "Imported Training XML - " & displayPilot.Name)
        End If
    End Sub

    Private Sub lvAttributes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvAttributes.DoubleClick
        Dim msg As String = ""
        Dim caption As String = ""

        Select Case lvAttributes.SelectedItems(0).Text
            Case ("Charisma")
                msg = "Starting Attribute: " & displayPilot.CAtt & vbCrLf
                msg = msg & "Implant: " & displayPilot.CImplant & vbCrLf
                msg = msg & "TOTAL: " & displayPilot.CAttT
                caption = "Charisma Attribute Breakdown"
            Case ("Intelligence")
                msg = "Starting Attribute: " & displayPilot.IAtt & vbCrLf
                msg = msg & "Implant: " & displayPilot.IImplant & vbCrLf
                msg = msg & "TOTAL: " & displayPilot.IAttT
                caption = "Intelligence Attribute Breakdown"
            Case ("Memory")
                msg = "Starting Attribute: " & displayPilot.MAtt & vbCrLf
                msg = msg & "Implant: " & displayPilot.MImplant & vbCrLf
                msg = msg & "TOTAL: " & displayPilot.MAttT
                caption = "Memory Attribute Breakdown"
            Case ("Perception")
                msg = "Starting Attribute: " & displayPilot.PAtt & vbCrLf
                msg = msg & "Implant: " & displayPilot.PImplant & vbCrLf
                msg = msg & "TOTAL: " & displayPilot.PAttT
                caption = "Perception Attribute Breakdown"
            Case ("Willpower")
                msg = "Starting Attribute: " & displayPilot.WAtt & vbCrLf
                msg = msg & "Implant: " & displayPilot.WImplant & vbCrLf
                msg = msg & "TOTAL: " & displayPilot.WAttT
                caption = "Willpower Attribute Breakdown"
        End Select
        MessageBox.Show(msg, caption)
    End Sub

#End Region

#Region "Implant Related Stuff"

    Private Sub chkManualImplants_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkManualImplants.CheckedChanged
        If chkManualImplants.Checked = True Then
            displayPilot.UseManualImplants = True
            btnEditImplants.Enabled = True
        Else
            displayPilot.UseManualImplants = False
            btnEditImplants.Enabled = False
        End If
        Call Me.UpdatePilotInfo()
    End Sub

    Private Sub btnEditImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditImplants.Click
        frmEditImplants.DisplayPilotName = displayPilot.Name
        frmEditImplants.ShowDialog()
        Call Me.UpdatePilotInfo()
    End Sub

    Private Sub UpdateImplants()
        lvImplants.Items(0).SubItems(1).Text = ("+" & displayPilot.CImplant)
        lvImplants.Items(1).SubItems(1).Text = ("+" & displayPilot.IImplant)
        lvImplants.Items(2).SubItems(1).Text = ("+" & displayPilot.MImplant)
        lvImplants.Items(3).SubItems(1).Text = ("+" & displayPilot.PImplant)
        lvImplants.Items(4).SubItems(1).Text = ("+" & displayPilot.WImplant)
    End Sub

#End Region

#Region "Portrait Related Routines"

    Private Sub mnuCtxPicGetPortraitFromServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCtxPicGetPortraitFromServer.Click
        If displayPilot.ID <> "" Then
            picPilot.ImageLocation = "http://image.eveonline.com/Character/" & displayPilot.ID & "_256.jpg"
        End If
    End Sub
    Private Sub mnuCtxPicGetPortraitFromLocal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCtxPicGetPortraitFromLocal.Click
        ' If double-clicked, see if we can get it from the eve portrait folder
        For folder As Integer = 1 To 4
            Dim folderName As String
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folder) = False Then
                Dim eveSettingsFolder As String = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder)
                If eveSettingsFolder IsNot Nothing Then
                    eveSettingsFolder = eveSettingsFolder.Replace("\", "_").Replace(":", "").Replace(" ", "_").ToLower & "_tranquility"
                    Dim eveFolder As String = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "EVE")
                    folderName = Path.Combine(Path.Combine(Path.Combine(Path.Combine(eveFolder, eveSettingsFolder), "cache"), "Pictures"), "Portraits")
                Else
                    folderName = ""
                End If
            Else
                folderName = Path.Combine(Path.Combine(Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(folder), "cache"), "Pictures"), "Portraits")
            End If
            If My.Computer.FileSystem.DirectoryExists(folderName) = True Then
                For Each foundFile As String In My.Computer.FileSystem.GetFiles(folderName, FileIO.SearchOption.SearchTopLevelOnly, "*.png")
                    If foundFile.Contains(displayPilot.ID & "_") = True Then
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
        MessageBox.Show("The requested portrait was not found within the Eve cache locations.", "Portrait Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub mnuSavePortrait_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSavePortrait.Click
        Dim imgFilename As String = displayPilot.ID & ".png"
        picPilot.Image.Save(Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imgFilename))
    End Sub
#End Region

#Region "OwnerDrawn Routines For SkillList"
    'Private Sub lvSkills_DrawColumnHeader(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs) Handles lvSkills.DrawColumnHeader
    '    e.DrawDefault = True
    'End Sub
    'Private Sub lvSkills_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvSkills.DrawItem
    '    If Not (e.State And ListViewItemStates.Selected) = 0 Then
    '        ' Draw the background for a selected item.
    '        e.Graphics.FillRectangle(Brushes.LightSteelBlue, e.Bounds)
    '        e.Graphics.DrawRectangle(New Pen(Color.DarkBlue), e.Bounds)
    '        lvSkills.Items(0).Text = lvSkills.Items(0).Text
    '    Else
    '        ' Draw the background for an unselected item.
    '        ' Default brush
    '        Dim brush As New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
    '        If e.Item.SubItems(2).Text = "5" Then
    '            ' If skill @ Level 5
    '            brush = New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
    '        Else
    '            If e.Item.Font.Bold = True Then
    '                ' If skill is training
    '                brush = New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
    '            Else
    '                If CBool(e.Item.SubItems(3).Tag) = True Then
    '                    ' If partially trained
    '                    brush = New LinearGradientBrush(e.Bounds, Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor)), Color.White, LinearGradientMode.ForwardDiagonal)
    '                End If
    '            End If
    '        End If
    '        Try
    '            e.Graphics.FillRectangle(brush, e.Bounds)
    '        Finally
    '            brush.Dispose()
    '        End Try
    '    End If
    '    Dim flags As TextFormatFlags = TextFormatFlags.VerticalCenter
    '    e.DrawText(flags)
    'End Sub
    'Private Sub lvSkills_DrawSubItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewSubItemEventArgs) Handles lvSkills.DrawSubItem
    '    If e.ColumnIndex > 0 Then
    '        If e.ColumnIndex = 2 Then
    '            ' Establish image
    '            Select Case e.Item.SubItems(2).Text
    '                Case "0"
    '                    e.Graphics.DrawImage(My.Resources.level0, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
    '                    'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level0.Width, e.SubItem.Bounds.Location.Y)
    '                Case "1"
    '                    e.Graphics.DrawImage(My.Resources.level1, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
    '                    'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level1.Width, e.SubItem.Bounds.Location.Y)
    '                Case "2"
    '                    e.Graphics.DrawImage(My.Resources.level2, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
    '                    'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level2.Width, e.SubItem.Bounds.Location.Y)
    '                Case "3"
    '                    e.Graphics.DrawImage(My.Resources.level3, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
    '                    'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level3.Width, e.SubItem.Bounds.Location.Y)
    '                Case "4"
    '                    e.Graphics.DrawImage(My.Resources.level4, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
    '                    'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level4.Width, e.SubItem.Bounds.Location.Y)
    '                Case "5"
    '                    e.Graphics.DrawImage(My.Resources.level5, e.SubItem.Bounds.Location.X, e.SubItem.Bounds.Location.Y + 5)
    '                    'e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, New SolidBrush(e.SubItem.ForeColor), e.SubItem.Bounds.Location.X + My.Resources.level5.Width, e.SubItem.Bounds.Location.Y)
    '            End Select
    '        Else
    '            Dim flags As TextFormatFlags = TextFormatFlags.VerticalCenter Or TextFormatFlags.HidePrefix Or TextFormatFlags.NoPadding
    '            e.SubItem.Font = e.Item.Font
    '            e.DrawText(flags)
    '        End If
    '    End If
    'End Sub
    'Private Sub lvSkills_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles lvSkills.MouseMove
    '    Dim item As ListViewItem = lvSkills.GetItemAt(e.X, e.Y)
    '    If item IsNot Nothing AndAlso item.Tag Is Nothing Then
    '        lvSkills.Invalidate(item.GetBounds(ItemBoundsPortion.Entire), False)
    '        'item.Tag = "tagged"
    '    End If
    'End Sub
    'Private Sub chkGraphicalView_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGraphicalView.CheckedChanged
    '    lvSkills.OwnerDraw = chkGraphicalView.Checked
    'End Sub

#End Region

    Private Sub chkGroupSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGroupSkills.CheckedChanged
        If DisplayPilotName <> "" Then
            Call Me.DisplaySkills()
            Call Me.DisplayCertificates()
        End If
    End Sub

    Private Sub ctxCerts_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxCerts.Opening
        If clvCerts.SelectedItems.Count <> 0 Then
            If clvCerts.SelectedItems(0).Items.Count = 0 Then
                Dim skillName As String = ""
                Dim skillID As String = ""
                Dim certName As String = clvCerts.SelectedItems(0).Text
                Dim certGrade As String = clvCerts.SelectedItems(0).SubItems(1).Text
                Dim certID As String = clvCerts.SelectedItems(0).Tag.ToString
                mnuCertName.Text = certName & " (" & certGrade & ")"
                mnuCertName.Tag = certID
            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub mnuViewCertDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewCertDetails.Click
        Dim certID As String = mnuCertName.Tag.ToString
        frmCertificateDetails.Text = mnuCertName.Text
        frmCertificateDetails.DisplayPilotName = displayPilot.Name
        frmCertificateDetails.ShowCertDetails(certID)
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilots.SelectedItem.ToString) = True Then
            displayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            Call UpdatePilotInfo()
        End If
    End Sub

#Region "Standings Routines"

    Private Sub btnGetStandings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetStandings.Click
        ' First, let's check out the cache location based on the value of the settings
        Dim cacheFileList As New ArrayList
        Dim baseCacheDir As String = ""
        Dim EveClient As DirectoryInfo
        Dim cacheDir As String = ""
        Dim folder As String = ""
        Dim cacheFileData As String = ""
        Dim CharStandingsRegex As New System.Text.RegularExpressions.Regex("GetCharStandings.*", (RegexOptions.Compiled Or RegexOptions.IgnoreCase))
        Dim CorpStandingsRegex As New System.Text.RegularExpressions.Regex("GetCorpStandings.*", (RegexOptions.Compiled Or RegexOptions.IgnoreCase))
        Dim MyStandings As New EveHQ.Core.StandingsData

        For folderNo As Integer = 1 To 4
            Cursor = Cursors.WaitCursor
            btnGetStandings.Enabled = False
            If EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo) <> "" Then
                ' Define the base cache dir depending on the /LUA switch
                If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folderNo) = True Then
                    EveClient = New DirectoryInfo(EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo))
                    baseCacheDir = EveClient.FullName
                    cacheDir = Path.Combine(Path.Combine(Path.Combine(baseCacheDir, "cache"), "machonet"), "87.237.38.200")
                Else
                    baseCacheDir = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "EVE")
                    ' Check the location
                    folder = EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo)
                    folder = folder.Replace(":", "")
                    folder = folder.Replace(" ", "_")
                    folder = folder.Replace("\", "_")
                    folder &= "_tranquility"
                    cacheDir = Path.Combine(baseCacheDir, folder)
                    cacheDir = Path.Combine(Path.Combine(Path.Combine(cacheDir, "cache"), "machonet"), "87.237.38.200")
                End If
                ' Search the cache dir for files containing the text "GetCharStandings"
                If My.Computer.FileSystem.DirectoryExists(cacheDir) = True Then
                    ' Get the newest directory (protocol version)
                    Dim protocols As New ArrayList
                    For Each protocolDir As String In My.Computer.FileSystem.GetDirectories(cacheDir)
                        protocols.Add(protocolDir)
                    Next
                    protocols.Sort()
                    protocols.Reverse()
                    cacheDir = CStr(protocols(0))
                    Dim fileError As Boolean = False
                    For Each cacheFile As String In My.Computer.FileSystem.GetFiles(cacheDir, FileIO.SearchOption.SearchAllSubDirectories, "*.cache")
                        Dim sr As New StreamReader(cacheFile)
                        Try
                            ' Load the contents of the cachefile to check for text
                            cacheFileData = sr.ReadToEnd
                            sr.Close()
                            Dim cacheMatch As MatchCollection = CharStandingsRegex.Matches(cacheFileData)
                            If cacheMatch.Count > 0 And cacheFile.Contains("CachedObjects") = True Then
                                cacheFileList.Add(cacheFile)
                            End If
                            cacheMatch = CorpStandingsRegex.Matches(cacheFileData)
                            If cacheMatch.Count > 0 And cacheFile.Contains("CachedObjects") = True Then
                                cacheFileList.Add(cacheFile)
                            End If
                        Catch ex As Exception
                            fileError = True
                        Finally
                            ' Try and close the stream but ignore if it's already closed (and therefore errors)
                            Try
                                sr.Close()
                            Catch ex As Exception
                            End Try
                        End Try
                    Next
                    If fileError = True Then
                        Cursor = Cursors.Default
                        btnGetStandings.Enabled = True
                        Dim msg As String = "There were some errors reading some applicable cache files. This could be due to corruption or some other problem."
                        msg &= "As a result, all the standings information may not be present or correct." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "If this problem persists, consider clearing your EveHQ cache folder."
                        MessageBox.Show(msg, "Errors Found In Cache Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    Cursor = Cursors.Default
                    btnGetStandings.Enabled = True
                    Dim msg As String = "Unable to locate cache folder for your " & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(folderNo) & " installation." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "EveHQ is checking the location: " & cacheDir & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Please check the location of your Eve Client and/or log in to Eve to create it. If you think this message is in error, please file a bug report."
                    MessageBox.Show(msg, "Missing Cache Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        Next

        If cacheFileList.Count > 0 Then
            Dim lastWrites As Dictionary(Of String, Date) = New Dictionary(Of String, Date)

            Cursor = Cursors.WaitCursor
            Dim StandingsDecoder As New EveHQ.Core.StandingsCacheDecoder
            EveHQ.Core.HQ.AllStandings.Clear()
            For Each cachefile As String In cacheFileList
                MyStandings = StandingsDecoder.FetchStandings(cachefile)
                If MyStandings.OwnerID IsNot Nothing Then
                    Dim last As Date = File.GetLastWriteTime(cachefile)
                    If EveHQ.Core.HQ.AllStandings.ContainsKey(MyStandings.OwnerID) = False Then
                        EveHQ.Core.HQ.AllStandings.Add(MyStandings.OwnerID, MyStandings)
                        lastWrites.Add(MyStandings.OwnerID, last)
                    Else
                        If last > lastWrites(MyStandings.OwnerID) Then
                            EveHQ.Core.HQ.AllStandings.Remove(MyStandings.OwnerID)
                            EveHQ.Core.HQ.AllStandings.Add(MyStandings.OwnerID, MyStandings)
                            lastWrites(MyStandings.OwnerID) = last
                        End If
                    End If
                End If
            Next
            Call EveHQ.Core.StandingsCacheDecoder.SaveStandings()
            Call UpdateOwners()
            Cursor = Cursors.Default
            btnGetStandings.Enabled = True
        Else
            MessageBox.Show("Unable to locate any valid cache files in any of your Eve installations. Please check the location of your Eve Client and/or log in to Eve and view your standings to create the relevant cache files.", "No Cache Files Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        Cursor = Cursors.Default
        btnGetStandings.Enabled = True

    End Sub
    Private Sub UpdateOwners()
        cboOwner.Items.Clear()
        lvwStandings.Items.Clear()
        If EveHQ.Core.HQ.AllStandings.Count > 0 Then
            ' Create the list of owners in the combobox
            cboOwner.BeginUpdate()
            Try
                For Each MyStandings As EveHQ.Core.StandingsData In EveHQ.Core.HQ.AllStandings.Values
                    ' Get Either Pilot or Corp Name
                    Dim ownerID As String = MyStandings.OwnerID
                    ' Cycle through the pilots to see if we have a match
                    For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                        Select Case MyStandings.CacheType
                            Case "GetCharStandings"
                                If ownerID = cPilot.ID Then
                                    If cboOwner.Items.Contains(cPilot.Name) = False Then
                                        cboOwner.Items.Add(cPilot.Name)
                                        MyStandings.OwnerName = cPilot.Name
                                    End If
                                End If
                            Case "GetCorpStandings"
                                If ownerID = cPilot.CorpID Then
                                    If cboOwner.Items.Contains(cPilot.Corp) = False Then
                                        cboOwner.Items.Add(cPilot.Corp)
                                        MyStandings.OwnerName = cPilot.Corp
                                    End If
                                End If
                        End Select
                    Next
                Next
                ' Enable everything
                cboOwner.Enabled = True
                lblSelectOwner.Enabled = True
                lvwStandings.Enabled = True
                lblTypeFilter.Enabled = True
                cboFilter.Enabled = True
            Catch e As Exception
                EveHQ.Core.HQ.AllStandings.Clear()
                ' Disable everything
                cboOwner.Enabled = False
                lblSelectOwner.Enabled = False
                lvwStandings.Enabled = False
                lblTypeFilter.Enabled = False
                cboFilter.Enabled = False
            End Try
            cboOwner.EndUpdate()
        Else
            'MessageBox.Show("Unable to find any valid cache files!", "No Cache Files Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub cboOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOwner.SelectedIndexChanged
        Call UpdateStandingsList()
    End Sub
    Private Sub lvwStandings_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwStandings.ColumnClick
        If CInt(lvwStandings.Tag) = e.Column Then
            Me.lvwStandings.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwStandings.Tag = -1
        Else
            Me.lvwStandings.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwStandings.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwStandings.Sort()
    End Sub
    Private Sub btExportStandings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btExportStandings.Click
        Try
            If cboOwner.SelectedItem IsNot Nothing Then
                If lvwStandings.Items.Count > 0 Then
                    ' Export the current list of standings
                    Dim sw As New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "Standings (" & cboOwner.SelectedItem.ToString & ").csv"))
                    sw.WriteLine("Standings Export for " & cboOwner.SelectedItem.ToString & " (dated: " & FormatDateTime(Now, DateFormat.GeneralDate) & ")")
                    sw.WriteLine("Entity Name,Entity ID,Entity Type,Raw Standing Value,Actual Standing Value")
                    For Each iStanding As ListViewItem In lvwStandings.Items
                        sw.Write(iStanding.Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                        sw.Write(iStanding.SubItems(1).Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                        sw.Write(iStanding.SubItems(2).Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                        sw.WriteLine(iStanding.SubItems(3).Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar & iStanding.SubItems(4).Text)
                    Next
                    sw.Flush()
                    sw.Close()
                    MessageBox.Show("CSV Standings file for " & cboOwner.SelectedItem.ToString & " successfully written to the EveHQ report folder!", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("There are no standings to export for " & cboOwner.SelectedItem.ToString & "!", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("You need to select an Owner before exporting standings!", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Export of CSV Standings file failed:" & ControlChars.CrLf & ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnClearCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearCache.Click
        Dim msg As String = "This will clear your cache of all files. Please make sure the cache folders are not in use and you are not logged into Eve." & ControlChars.CrLf & ControlChars.CrLf
        msg &= "Are you sure you wish to proceed?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Standings Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            Dim cacheFileList As New ArrayList
            Dim baseCacheDir As String = ""
            Dim EveClient As DirectoryInfo
            Dim cacheDir As String = ""
            Dim folder As String = ""
            Dim fail As Boolean = False
            For folderNo As Integer = 1 To 4
                If EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo) <> "" Then
                    ' Define the base cache dir depending on the /LUA switch
                    If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folderNo) = True Then
                        EveClient = New DirectoryInfo(EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo))
                        baseCacheDir = EveClient.FullName
                        cacheDir = Path.Combine(Path.Combine(Path.Combine(baseCacheDir, "cache"), "machonet"), "87.237.38.200")
                    Else
                        baseCacheDir = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "EVE")
                        ' Check the location
                        folder = EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo)
                        folder = folder.Replace(":", "")
                        folder = folder.Replace(" ", "_")
                        folder = folder.Replace("\", "_")
                        folder &= "_tranquility"
                        cacheDir = Path.Combine(baseCacheDir, folder)
                        cacheDir = Path.Combine(Path.Combine(Path.Combine(cacheDir, "cache"), "machonet"), "87.237.38.200")
                    End If
                    ' Search the cache dir for files containing the text "GetCharStandings"
                    If My.Computer.FileSystem.DirectoryExists(cacheDir) = True Then
                        Try
                            My.Computer.FileSystem.DeleteDirectory(cacheDir, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                        Catch ex As Exception
                            fail = True
                        End Try
                    End If
                Else
                    ' Do nothing if we can't find a folder! Assume complete!
                End If
            Next
            If fail = False Then
                MessageBox.Show("All Cache files deleted from the cache folders!", "Delete Cache Files Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Unable to delete all Cache folders. Please check that you have the required access, the folder isn't being used and that Eve is not running.", "Delete Cache Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub
    Private Sub cboFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFilter.SelectedIndexChanged
        If cboFilter.Tag.ToString <> "0" Then
            Call Me.UpdateStandingsList()
        End If
        cboFilter.Tag = "1"
    End Sub
    Private Sub UpdateStandingsList()
        If cboOwner.Items.Count > 0 Then
            If cboFilter.SelectedItem Is Nothing Then
                cboFilter.SelectedIndex = 0
            End If
            If cboOwner.SelectedItem IsNot Nothing Then
                Dim ownerName As String = cboOwner.SelectedItem.ToString
                Dim DiplomacyLevel As Integer = 0
                Dim ConnectionsLevel As Integer = 0
                Dim standing As Double = 0
                ' Iterate through the list and find the rightID
                For Each MyStandings As EveHQ.Core.StandingsData In EveHQ.Core.HQ.AllStandings.Values
                    If ownerName = MyStandings.OwnerName Then
                        ' Check if this is a character and whether we need to get the Connections and Diplomacy skills
                        If MyStandings.CacheType = "GetCharStandings" Then
                            Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(ownerName), Core.Pilot)
                            For Each cSkill As EveHQ.Core.PilotSkill In cPilot.PilotSkills
                                If cSkill.Name = "Diplomacy" Then
                                    DiplomacyLevel = cSkill.Level
                                End If
                                If cSkill.Name = "Connections" Then
                                    ConnectionsLevel = cSkill.Level
                                End If
                            Next
                        Else
                            DiplomacyLevel = 0
                            ConnectionsLevel = 0
                        End If
                        ' Populate the standings list
                        lvwStandings.BeginUpdate()
                        lvwStandings.Items.Clear()
                        For Each iStanding As String In MyStandings.StandingValues.Keys
                            ' Determine if we need to filter
                            Dim show As Boolean = False
                            Select Case cboFilter.SelectedItem.ToString
                                Case "<All>"
                                    show = True
                                Case "Agent"
                                    If CLng(iStanding) >= 3000000 And CLng(iStanding) <= 3099999 Then
                                        show = True
                                    End If
                                Case "Corporation"
                                    If CLng(iStanding) >= 1000000 And CLng(iStanding) <= 1099999 Then
                                        show = True
                                    End If
                                Case "Faction"
                                    If CLng(iStanding) >= 500000 And CLng(iStanding) <= 599999 Then
                                        show = True
                                    End If
                                Case "Player/Corp"
                                    If CLng(iStanding) >= 4000000 Then
                                        show = True
                                    End If
                            End Select
                            If show = True Then
                                Dim newStanding As New ListViewItem
                                Try
                                    newStanding.Text = MyStandings.StandingNames(iStanding).ToString
                                    newStanding.Name = MyStandings.OwnerName
                                    newStanding.SubItems.Add(iStanding.ToString)
                                    Select Case CLng(iStanding)
                                        Case 500000 To 599999
                                            newStanding.SubItems.Add("Faction")
                                        Case 1000000 To 1099999
                                            newStanding.SubItems.Add("Corporation")
                                        Case 3000000 To 3099999
                                            newStanding.SubItems.Add("Agent")
                                        Case Else
                                            newStanding.SubItems.Add("Player/Corp")
                                    End Select
                                    standing = CDbl(MyStandings.StandingValues(iStanding))
                                    Dim rawStanding As New ListViewItem.ListViewSubItem
                                    rawStanding.Text = FormatNumber(standing, CInt(nudPrecision.Value), TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    rawStanding.Name = "RawStanding"
                                    rawStanding.Tag = standing
                                    newStanding.SubItems.Add(rawStanding)
                                    If CLng(iStanding) < 70000000 Then
                                        If standing < 0 Then
                                            standing = standing + ((10 - standing) * (DiplomacyLevel * 4 / 100))
                                        Else
                                            standing = standing + ((10 - standing) * (ConnectionsLevel * 4 / 100))
                                        End If
                                    End If
                                    Dim actualStanding As New ListViewItem.ListViewSubItem
                                    actualStanding.Text = FormatNumber(standing, CInt(nudPrecision.Value), TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                                    actualStanding.Name = "ActualStanding"
                                    actualStanding.Tag = standing
                                    newStanding.SubItems.Add(actualStanding)
                                    lvwStandings.Items.Add(newStanding)
                                Catch e As Exception
                                    ' Assume a null agent/corp etc and move on
                                    'Dim msg As String = "There was an error processing the standings details for:" & ControlChars.CrLf & "Standing ID: " & iStanding & ControlChars.CrLf
                                    'msg &= "If this continues, please clear the Eve cache and retry."
                                    'MessageBox.Show(msg, "Standings Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End Try
                            End If
                        Next
                        lvwStandings.EndUpdate()
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub nudPrecision_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPrecision.ValueChanged
        Call Me.UpdateStandingsList()
    End Sub
    Private Sub ctxStandings_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxStandings.Opening
        If lvwStandings.SelectedItems.Count = 0 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuExtrapolateStandings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExtrapolateStandings.Click
        If lvwStandings.SelectedItems.Count >= 1 Then
            Dim standingsLine As ListViewItem = lvwStandings.SelectedItems(0)
            'Dim ownerID As String = standingsLine.Name
            'Dim standingID As String = standingsLine.Tag.ToString
            Dim extraStandings As New frmExtraStandings
            extraStandings.Pilot = standingsLine.Name
            extraStandings.Party = standingsLine.Text
            extraStandings.Standing = CDbl(standingsLine.SubItems("ActualStanding").Tag)
            extraStandings.BaseStanding = CDbl(standingsLine.SubItems("RawStanding").Tag)
            extraStandings.ShowDialog()
        End If
    End Sub

#End Region

End Class

