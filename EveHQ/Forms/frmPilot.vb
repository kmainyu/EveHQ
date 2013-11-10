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
Imports System.ComponentModel
Imports DevComponents.DotNetBar
Imports EveHQ.EveData
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports DevComponents.AdvTree
Imports System.IO
Imports SearchOption = Microsoft.VisualBasic.FileIO.SearchOption

Namespace Forms

    Public Class FrmPilot
        Dim _trainingSkill As Node
        Dim _trainingGroup As Node
        Dim _displayPilot As New EveHQPilot
        Dim _displayPilotName As String = ""

        Public Property DisplayPilotName() As String
            Get
                Return _displayPilot.Name
            End Get
            Set(ByVal value As String)
                _displayPilotName = value
                If cboPilots.Items.Contains(value) Then
                    cboPilots.SelectedItem = value
                End If
            End Set
        End Property

        Private Sub frmPilot_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Call UpdatePilots()
        End Sub

        Public Sub UpdatePilots()

            ' Update standings filter
            cboFilter.SelectedIndex = 0

            ' Save old Pilot info
            Dim oldPilot As String = ""
            If cboPilots.SelectedItem IsNot Nothing Then
                oldPilot = cboPilots.SelectedItem.ToString
            End If

            ' Update the pilots combo box
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboPilots.Items.Add(cPilot.Name)
                End If
            Next
            cboPilots.EndUpdate()

            ' Select a pilot
            If _displayPilotName <> "" Then
                If cboPilots.Items.Count > 0 Then
                    If cboPilots.Items.Contains(_displayPilotName) = True Then
                        cboPilots.SelectedItem = _displayPilotName
                    Else
                        cboPilots.SelectedIndex = 0
                    End If
                End If
            Else
                If oldPilot = "" Then
                    If cboPilots.Items.Count > 0 Then
                        If cboPilots.Items.Contains(HQ.Settings.StartupPilot) = True Then
                            cboPilots.SelectedItem = HQ.Settings.StartupPilot
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

            If _displayPilot.PilotSkills.Count > 0 Then

                ' Get image from cache
                Try
                    picPilot.Image = ImageHandler.GetPortraitImage(_displayPilot.ID)

                    Call PilotParseFunctions.SwitchImplants(_displayPilot)

                Catch e As Exception
                    Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Pilot Name: " & _displayPilot.Name
                    MessageBox.Show(msg, "Error Retrieving Cached Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                ' Update Race image
                picRace.Image = CType(My.Resources.ResourceManager.GetObject(_displayPilot.Race.Replace("-", "_") & "Race"), Image)
                ' Update Blood image
                picBlood.Image = CType(My.Resources.ResourceManager.GetObject(_displayPilot.Blood.Replace("-", "_") & "Blood"), Image)

                ' Display Information
                Try
                    lblPilotName.Text = _displayPilot.Name
                    lblPilotID.Text = _displayPilot.ID
                    lblPilotCorp.Text = _displayPilot.Corp
                    lblPilotIsk.Text = _displayPilot.Isk.ToString("N2")
                    lblPilotSP.Text = (_displayPilot.SkillPoints + SkillFunctions.CalcCurrentSkillPoints(_displayPilot)).ToString("N0")
                    lblPilotClone.Text = _displayPilot.CloneName & " (" & CLng(_displayPilot.CloneSP).ToString("N0") & " SP)"
                    ' Check Clone
                    If (_displayPilot.SkillPoints + _displayPilot.TrainingCurrentSP) > CLng(_displayPilot.CloneSP) Then
                        lblPilotClone.ForeColor = Color.Red
                    Else
                        lblPilotClone.ForeColor = Color.Black
                    End If
                Catch e As Exception
                    Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Pilot Name: " & _displayPilot.Name
                    MessageBox.Show(msg, "Error Displaying Pilot Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                ' Display Implant method
                If _displayPilot.UseManualImplants = True Then
                    chkManImplants.Checked = True
                Else
                    chkManImplants.Checked = False
                End If

                ' Display Attribute & Implant Information
                Try
                    lblCharismaTotal.Text = _displayPilot.CAttT.ToString("N1")
                    lblIntelligenceTotal.Text = _displayPilot.IntAttT.ToString("N1")
                    lblMemoryTotal.Text = _displayPilot.MAttT.ToString("N1")
                    lblPerceptionTotal.Text = _displayPilot.PAttT.ToString("N1")
                    lblWillpowerTotal.Text = _displayPilot.WAttT.ToString("N1")

                    lblCharismaDetail.Text = "( " & _displayPilot.CAtt.ToString & " Base +  " & _displayPilot.CImplant.ToString & " Implant)"
                    lblIntelligenceDetail.Text = "( " & _displayPilot.IntAtt.ToString & " Base +  " & _displayPilot.IntImplant.ToString & " Implant)"
                    lblMemoryDetail.Text = "( " & _displayPilot.MAtt.ToString & " Base +  " & _displayPilot.MImplant.ToString & " Implant)"
                    lblPerceptionDetail.Text = "( " & _displayPilot.PAtt.ToString & " Base +  " & _displayPilot.PImplant.ToString & " Implant)"
                    lblWillpowerDetail.Text = "( " & _displayPilot.WAtt.ToString & " Base +  " & _displayPilot.WImplant.ToString & " Implant)"

                Catch e As Exception
                    Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Pilot Name: " & _displayPilot.Name
                    MessageBox.Show(msg, "Error Displaying Pilot Attributes", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                ' Display Skill Training
                Try
                    If _displayPilot.Training = True Then

                        ' Establish which skill is training
                        Dim currentQueuedSkill As New EveHQPilotQueuedSkill
                        For Each queuedSkill As EveHQPilotQueuedSkill In _displayPilot.QueuedSkills.Values
                            If SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) >= Now And SkillFunctions.ConvertEveTimeToLocal(queuedSkill.StartTime) <= Now Then
                                currentQueuedSkill = queuedSkill
                            End If
                        Next

                        Dim currentSkill As EveHQPilotSkill
                        Dim endTime As Long
                        If currentQueuedSkill.SkillID <> 0 Then
                            currentSkill = _displayPilot.PilotSkills.Item(SkillFunctions.SkillIDToName(currentQueuedSkill.SkillID))
                            lblTrainingSkill.Text = SkillFunctions.SkillIDToName(currentQueuedSkill.SkillID) & " (Level " & SkillFunctions.Roman(currentQueuedSkill.Level) & ")"
                            Dim localdate As Date = SkillFunctions.ConvertEveTimeToLocal(currentQueuedSkill.EndTime)
                            lblTrainingEnds.Text = Format(localdate, "ddd") & " " & localdate
                            endTime = CLng((currentQueuedSkill.EndTime - Now).TotalSeconds)
                        Else
                            currentSkill = _displayPilot.PilotSkills.Item(SkillFunctions.SkillIDToName(_displayPilot.TrainingSkillID))
                            lblTrainingSkill.Text = SkillFunctions.SkillIDToName(_displayPilot.TrainingSkillID) & " (Level " & SkillFunctions.Roman(_displayPilot.TrainingSkillLevel) & ")"
                            Dim localdate As Date = SkillFunctions.ConvertEveTimeToLocal(_displayPilot.TrainingEndTime)
                            lblTrainingEnds.Text = Format(localdate, "ddd") & " " & localdate
                            endTime = _displayPilot.TrainingCurrentTime
                        End If
                        lblTrainingRate.Text = "Rank " & currentSkill.Rank & " @ " & SkillFunctions.CalculateSPRate(_displayPilot, HQ.SkillListID(currentSkill.ID)).ToString("N0") & " SP/Hr"
                        lblTrainingTime.Text = SkillFunctions.TimeToString(endTime)
                        Select Case endTime
                            Case 0 To 86400
                                lblTrainingTime.ForeColor = Color.Red
                            Case Else
                                lblTrainingTime.ForeColor = Color.Black
                        End Select
                    Else
                        lblTrainingSkill.Text = "Not currently training"
                        lblTrainingTime.Text = ""
                        lblTrainingEnds.Text = ""
                        lblTrainingRate.Text = ""
                    End If
                Catch e As Exception
                    ' Possible cache corruption
                    If frmEveHQ.CacheErrorHandler() = True Then Exit Sub
                End Try

                ' Display Account Info
                lblAccountExpiry.ForeColor = Color.Black
                If HQ.Settings.Accounts.ContainsKey(_displayPilot.Account) = True Then
                    Dim dAccount As EveHQAccount = HQ.Settings.Accounts(_displayPilot.Account)
                    If (dAccount.ApiKeySystem = APIKeySystems.Version2 And dAccount.CanUseCharacterAPI(CharacterAccessMasks.AccountStatus)) Then
                        lblAccountExpiry.Text = "Expiry: " & dAccount.PaidUntil.ToString & " (" & SkillFunctions.TimeToString((dAccount.PaidUntil - Now).TotalSeconds) & ")"
                        lblAccountLogins.Text = "Login Count: " & dAccount.LogonCount & " (" & SkillFunctions.TimeToString(dAccount.LogonMinutes * 60, False) & ")"
                        If HQ.Settings.NotifyAccountTime = True Then
                            Dim accountTime As Date = dAccount.PaidUntil
                            If accountTime.Year > 2000 And (accountTime - Now).TotalHours <= HQ.Settings.AccountTimeLimit Then
                                lblAccountExpiry.ForeColor = Color.Red
                            End If
                        End If
                        grpAccount.Visible = True
                    Else
                        grpAccount.Visible = False
                    End If
                Else
                    grpAccount.Visible = False
                End If

                btnUpdateAPI.Enabled = False

                ' Display skills & stuff
                Call DisplaySkills()
                Call DisplayCertificates()

                ' Update skill queue
                sqcEveQueue.PilotName = _displayPilot.Name

                ' Update Standings stuff
                Call UpdateStandingsList()

            Else
                adtSkills.Nodes.Clear()
                adtCerts.Nodes.Clear()
                ' Get image from cache
                If _displayPilot.ID = "" Then
                    picPilot.Image = My.Resources.noitem
                End If
            End If
        End Sub

        Private Sub DisplaySkills()
            Const MaxGroups As Integer = 21
            Dim groupHeaders(MaxGroups, 3) As String
            groupHeaders(0, 0) = "Armor"
            groupHeaders(1, 0) = "Corporation Management"
            groupHeaders(2, 0) = "Drones"
            groupHeaders(3, 0) = "Electronic Systems"
            groupHeaders(4, 0) = "Engineering"
            groupHeaders(5, 0) = "Gunnery"
            groupHeaders(6, 0) = "Leadership"
            groupHeaders(7, 0) = "Missiles"
            groupHeaders(8, 0) = "Navigation"
            groupHeaders(9, 0) = "Neural Enhancement"
            groupHeaders(10, 0) = "Planet Management"
            groupHeaders(11, 0) = "Production"
            groupHeaders(12, 0) = "Resource Processing"
            groupHeaders(13, 0) = "Rigging"
            groupHeaders(14, 0) = "Scanning"
            groupHeaders(15, 0) = "Science"
            groupHeaders(16, 0) = "Shields"
            groupHeaders(17, 0) = "Social"
            groupHeaders(18, 0) = "Spaceship Command"
            groupHeaders(19, 0) = "Subsystems"
            groupHeaders(20, 0) = "Targeting"
            groupHeaders(21, 0) = "Trade"
            groupHeaders(0, 1) = "1210"
            groupHeaders(1, 1) = "266"
            groupHeaders(2, 1) = "273"
            groupHeaders(3, 1) = "272"
            groupHeaders(4, 1) = "1216"
            groupHeaders(5, 1) = "255"
            groupHeaders(6, 1) = "258"
            groupHeaders(7, 1) = "256"
            groupHeaders(8, 1) = "275"
            groupHeaders(9, 1) = "1220"
            groupHeaders(10, 1) = "1241"
            groupHeaders(11, 1) = "268"
            groupHeaders(12, 1) = "1218"
            groupHeaders(13, 1) = "269"
            groupHeaders(14, 1) = "1217"
            groupHeaders(15, 1) = "270"
            groupHeaders(16, 1) = "1209"
            groupHeaders(17, 1) = "278"
            groupHeaders(18, 1) = "257"
            groupHeaders(19, 1) = "1240"
            groupHeaders(20, 1) = "1213"
            groupHeaders(21, 1) = "274"

            ' Set Styles
            Dim skillGroupStyle As ElementStyle = adtSkills.Styles("SkillGroup").Copy
            skillGroupStyle.BackColor = Color.FromArgb(CInt(HQ.Settings.PilotGroupBackgroundColor))
            skillGroupStyle.BackColor2 = Color.Black
            skillGroupStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotGroupTextColor))
            Dim normalSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
            normalSkillStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotStandardSkillColor))
            normalSkillStyle.BackColor = Color.FromArgb(128, normalSkillStyle.BackColor2)
            normalSkillStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotSkillTextColor))
            Dim partialSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
            partialSkillStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotPartTrainedSkillColor))
            partialSkillStyle.BackColor = Color.FromArgb(128, partialSkillStyle.BackColor2)
            partialSkillStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotSkillTextColor))
            Dim level5SkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
            level5SkillStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotLevel5SkillColor))
            level5SkillStyle.BackColor = Color.FromArgb(128, level5SkillStyle.BackColor2)
            level5SkillStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotSkillTextColor))
            Dim trainingSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
            trainingSkillStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotCurrentTrainSkillColor))
            trainingSkillStyle.BackColor = Color.FromArgb(128, trainingSkillStyle.BackColor2)
            trainingSkillStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotSkillTextColor))
            Dim selSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
            selSkillStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotSkillHighlightColor))
            selSkillStyle.BackColor = Color.FromArgb(32, selSkillStyle.BackColor2)

            ' Set up Groups

            adtSkills.Refresh()
            adtSkills.BeginUpdate()
            adtSkills.Nodes.Clear()

            Dim groupStructure As New SortedList
            If chkGroupSkills.Checked = True Then
                For group As Integer = 0 To MaxGroups
                    Dim newSkillGroup As New Node("", skillGroupStyle)
                    newSkillGroup.FullRowBackground = True
                    For cell As Integer = 1 To 5
                        newSkillGroup.Cells.Add(New Cell)
                        newSkillGroup.Cells(cell).Tag = 0
                    Next
                    newSkillGroup.Text = groupHeaders(group, 0)
                    adtSkills.Nodes.Add(newSkillGroup)
                    groupStructure.Add(groupHeaders(group, 1), newSkillGroup)
                Next
            End If

            ' Parse in-game skill queue
            Dim eveSkillsQueued As New SortedList(Of Integer, Integer)
            For Each queuedSkill As EveHQPilotQueuedSkill In _displayPilot.QueuedSkills.Values
                If eveSkillsQueued.ContainsKey(queuedSkill.SkillID) = False Then
                    eveSkillsQueued.Add(queuedSkill.SkillID, queuedSkill.Level)
                Else
                    If queuedSkill.Level > eveSkillsQueued(queuedSkill.SkillID) Then
                        eveSkillsQueued(queuedSkill.SkillID) = queuedSkill.Level
                    End If
                End If
            Next

            ' Set up items
            For Each cSkill As EveHQPilotSkill In _displayPilot.PilotSkills.Values
                Dim baseSkill As EveSkill = HQ.SkillListName(cSkill.Name)
                Try
                    Dim groupClv As Node = CType(groupStructure(CStr(cSkill.GroupID)), Node)
                    Dim newClvItem As New Node
                    newClvItem.FullRowBackground = True
                    newClvItem.Text = cSkill.Name
                    newClvItem.Style = normalSkillStyle
                    newClvItem.StyleSelected = selSkillStyle
                    If chkGroupSkills.Checked = True Then
                        groupClv.Nodes.Add(newClvItem)
                    Else
                        adtSkills.Nodes.Add(newClvItem)
                    End If
                    newClvItem.Cells.Add(New Cell(cSkill.Rank.ToString))
                    newClvItem.Cells(1).Tag = cSkill.Rank

                    newClvItem.Cells.Add(New Cell)
                    If eveSkillsQueued.ContainsKey(cSkill.ID) Then
                        If eveSkillsQueued(cSkill.ID) > cSkill.Level Then
                            newClvItem.Cells(2).Images.Image = CType(My.Resources.ResourceManager.GetObject("level_" & cSkill.Level.ToString & eveSkillsQueued(cSkill.ID).ToString & "0"), Image)
                            If groupClv IsNot Nothing Then
                                If groupClv.Cells(2).Tag IsNot Nothing Then
                                    groupClv.Cells(2).Tag = CInt(groupClv.Cells(2).Tag) + 1
                                Else
                                    groupClv.Cells(2).Tag = 1
                                End If
                            End If
                        Else
                            newClvItem.Cells(2).Images.Image = CType(My.Resources.ResourceManager.GetObject("level_" & cSkill.Level.ToString & "00"), Image)
                        End If
                    Else
                        newClvItem.Cells(2).Images.Image = CType(My.Resources.ResourceManager.GetObject("level_" & cSkill.Level.ToString & "00"), Image)
                    End If
                    newClvItem.Cells(2).Tag = cSkill.Level

                    Dim percent As Double
                    Dim partially As Boolean = False
                    If cSkill.Level = 5 Then
                        percent = 100
                    Else
                        If _displayPilot.TrainingSkillID = cSkill.ID Then
                            percent = CDbl((cSkill.SP + _displayPilot.TrainingCurrentSP - baseSkill.LevelUp(cSkill.Level)) / (baseSkill.LevelUp(cSkill.Level + 1) - baseSkill.LevelUp(cSkill.Level)) * 100)
                            If cSkill.SP + _displayPilot.TrainingCurrentSP > baseSkill.LevelUp(cSkill.Level) + 1 Then
                                partially = True
                            End If
                        Else
                            percent = (Math.Min(Math.Max(CDbl((cSkill.SP - baseSkill.LevelUp(cSkill.Level)) / (baseSkill.LevelUp(cSkill.Level + 1) - baseSkill.LevelUp(cSkill.Level)) * 100), 0), 100))
                            If cSkill.SP > baseSkill.LevelUp(cSkill.Level) + 1 Then
                                partially = True
                            End If
                        End If
                    End If
                    ' Write percentage
                    newClvItem.Cells.Add(New Cell(percent.ToString("N0") & "%"))
                    newClvItem.Cells(3).Tag = percent.ToString("N2")

                    ' Write skillpoints
                    newClvItem.Cells.Add(New Cell(cSkill.SP.ToString("N0")))
                    newClvItem.Cells(4).Tag = cSkill.SP

                    If chkGroupSkills.Checked = True Then
                        For group As Integer = 0 To MaxGroups
                            If cSkill.GroupID = CInt(groupHeaders(group, 1)) Then
                                'newLine.Group = lvSkills.Groups.Item(skillGroup)
                                groupHeaders(group, 2) = CStr(CDbl(groupHeaders(group, 2)) + cSkill.SP)
                                groupHeaders(group, 3) = CStr(CDbl(groupHeaders(group, 3)) + 1)
                                groupClv.Text = groupHeaders(group, 0) & " - skills: " & groupHeaders(group, 3)
                                If groupClv.Cells(2).Tag IsNot Nothing Then
                                    If CInt(groupClv.Cells(2).Tag) > 0 Then
                                        groupClv.Text &= "<font color=""#0085AB"">  (" & groupClv.Cells(2).TagString & " in queue)</font>"
                                    End If
                                End If
                                groupClv.Tag = groupClv.Text
                                groupClv.Cells(4).Text = CDbl(groupHeaders(group, 2)).ToString("N0")
                                groupClv.Cells(4).Tag = groupHeaders(group, 2)
                                Exit For
                            End If
                        Next
                    End If

                    ' Write time to next level - adjusted if 0 to put completed skills to the bottom
                    Dim timeSubItem As New ListViewItem.ListViewSubItem
                    Dim currentTime As Long
                    If _displayPilot.TrainingSkillID = cSkill.ID Then
                        timeSubItem.Text = SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                        currentTime = _displayPilot.TrainingCurrentTime
                        _trainingSkill = newClvItem
                        _trainingGroup = groupClv
                    Else
                        currentTime = CLng(SkillFunctions.CalcTimeToLevel(_displayPilot, HQ.SkillListID(cSkill.ID), 0, ))
                        timeSubItem.Text = SkillFunctions.TimeToString(currentTime)
                    End If
                    If currentTime = 0 Then currentTime = 9999999999
                    newClvItem.Cells.Add(New Cell(timeSubItem.Text))
                    newClvItem.Cells(5).Tag = currentTime.ToString

                    ' Select colours for line background
                    If cSkill.Level = 5 Then
                        newClvItem.Style = level5SkillStyle
                    Else
                        If _displayPilot.TrainingSkillID = cSkill.ID Then
                            newClvItem.Style = trainingSkillStyle
                            If chkGroupSkills.Checked = True Then
                                groupClv.Text = groupClv.Tag.ToString & "<font color=""#FFD700"">  - Training</font>"
                                groupClv.Cells(4).Text = "<font color=""#FFD700"">" & groupClv.Cells(4).Text & "</font>"
                                'groupCLV.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
                            End If
                        Else
                            If partially = True Then
                                newClvItem.Style = partialSkillStyle
                            End If
                        End If
                    End If

                Catch e As Exception
                    If frmEveHQ.CacheErrorHandler() = True Then Exit Sub
                End Try
            Next

            ' Remove empty groups
            If chkGroupSkills.Checked = True Then
                Dim sg As Node
                Dim sgNo As Integer = 0
                Do
                    sg = adtSkills.Nodes(sgNo)
                    If sg.Nodes.Count = 0 Then
                        adtSkills.Nodes.Remove(sg)
                        sgNo -= 1
                    End If
                    sgNo += 1
                Loop Until sgNo = adtSkills.Nodes.Count
            End If

            AdvTreeSorter.Sort(adtSkills, 1, True, True)
            adtSkills.EndUpdate()
            If chkGroupSkills.Checked = True Then
                If _trainingGroup IsNot Nothing Then
                    _trainingGroup.Cells(4).Text = "<font color=""#FFD700"">" & (CLng(_trainingGroup.Cells(4).Tag) + _displayPilot.TrainingCurrentSP).ToString("N0") & "</font>"
                    _trainingGroup.Text = _trainingGroup.Tag.ToString & "<font color=""#FFD700"">  - Training</font>"
                    'TrainingGroup.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
                End If
            End If
        End Sub

        Private Sub DisplayCertificates()

            Dim cCert As Certificate

            ' Filter out the lower end certificates
            Dim certList As New SortedList
            For Each cCertID As Integer In _displayPilot.Certificates
                If StaticData.Certificates.ContainsKey(cCertID) Then
                    cCert = StaticData.Certificates(cCertID)
                    If certList.Contains(cCert.ClassId) = False Then
                        certList.Add(cCert.ClassId, cCert)
                    Else
                        Dim storedGrade As Integer = CType(certList(cCert.ClassId), Certificate).Grade
                        If cCert.Grade > storedGrade Then
                            certList(cCert.ClassId) = cCert
                        End If
                    End If
                End If
            Next

            ' Set Styles
            Dim certGroupStyle As ElementStyle = adtSkills.Styles("SkillGroup").Copy
            certGroupStyle.BackColor = Color.FromArgb(CInt(HQ.Settings.PilotGroupBackgroundColor))
            certGroupStyle.BackColor2 = Color.Black
            certGroupStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotGroupTextColor))
            Dim normalCertStyle As ElementStyle = adtSkills.Styles("Skill").Copy
            normalCertStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotStandardSkillColor))
            normalCertStyle.BackColor = Color.FromArgb(128, normalCertStyle.BackColor2)
            normalCertStyle.TextColor = Color.FromArgb(CInt(HQ.Settings.PilotSkillTextColor))
            Dim selCertStyle As ElementStyle = adtSkills.Styles("Skill").Copy
            selCertStyle.BackColor2 = Color.FromArgb(CInt(HQ.Settings.PilotSkillHighlightColor))
            selCertStyle.BackColor = Color.FromArgb(32, selCertStyle.BackColor2)

            'Set up Groups
            adtCerts.BeginUpdate()
            adtCerts.Nodes.Clear()

            Dim certGroups As New SortedList

            If chkGroupSkills.Checked = True Then
                For Each cCategory As CertificateCategory In StaticData.CertificateCategories.Values
                    Dim newCertGroup As New Node("", certGroupStyle)
                    newCertGroup.FullRowBackground = True
                    For cell As Integer = 1 To 1
                        newCertGroup.Cells.Add(New Cell)
                        newCertGroup.Cells(cell).Tag = 0
                    Next
                    newCertGroup.Text = cCategory.Name
                    newCertGroup.Tag = 0
                    certGroups.Add(cCategory.Id.ToString, newCertGroup)
                    adtCerts.Nodes.Add(newCertGroup)
                Next
            End If

            'Set up items

            For Each cCert In certList.Values
                Dim certGroup As Node = CType(certGroups(cCert.CategoryId.ToString), Node)
                Dim newCert As New Node("", normalCertStyle)
                newCert.FullRowBackground = True
                newCert.Text = StaticData.CertificateClasses(cCert.ClassId.ToString).Name
                newCert.Tag = cCert.Id
                If chkGroupSkills.Checked = True Then
                    certGroup.Nodes.Add(newCert)
                    certGroup.Tag = CInt(certGroup.Tag) + 1
                Else
                    adtCerts.Nodes.Add(newCert)
                End If
                newCert.StyleSelected = selCertStyle
                newCert.Cells.Add(New Cell)
                newCert.Cells(1).Tag = cCert.Grade
                newCert.Image = CType(My.Resources.ResourceManager.GetObject("Cert" & cCert.Grade.ToString), Image)
                Select Case cCert.Grade
                    Case 1
                        newCert.Cells(1).Text = "Basic"
                    Case 2
                        newCert.Cells(1).Text = "Standard"
                    Case 3
                        newCert.Cells(1).Text = "Improved"
                    Case 4
                        newCert.Cells(1).Text = "Advanced"
                    Case 5
                        newCert.Cells(1).Text = "Elite"
                End Select

            Next

            ' Add certificate count and remove empty groups
            If chkGroupSkills.Checked = True Then
                For Each certGroup As Node In adtCerts.Nodes
                    certGroup.Text &= " (" & certGroup.Tag.ToString & " certificates)"
                Next
                Dim sg As Node
                Dim sgNo As Integer = 0
                Do
                    sg = adtCerts.Nodes(sgNo)
                    If sg.Nodes.Count = 0 Then
                        adtCerts.Nodes.Remove(sg)
                        sgNo -= 1
                    End If
                    sgNo += 1
                Loop Until sgNo = adtCerts.Nodes.Count
            End If

            AdvTreeSorter.Sort(adtCerts, 1, True, True)
            adtCerts.EndUpdate()
        End Sub

#Region "Skill Update Routine"
        Public Sub UpdateSkillInfo()
            If _displayPilot.PilotSkills.Count <> 0 Then
                If _displayPilot.Training = True Then
                    lblPilotSP.Text = (_displayPilot.SkillPoints + _displayPilot.TrainingCurrentSP).ToString("N0")
                    If _displayPilot.PilotSkills.ContainsKey(SkillFunctions.SkillIDToName(_displayPilot.TrainingSkillID)) = True Then
                        Dim cSkill As EveHQPilotSkill = _displayPilot.PilotSkills(SkillFunctions.SkillIDToName(_displayPilot.TrainingSkillID))
                        Dim baseSkill As EveSkill = HQ.SkillListName(cSkill.Name)
                        Dim percent As Double
                        If cSkill.Level = 5 Then
                            percent = 100
                        Else
                            If _displayPilot.TrainingSkillID = cSkill.ID Then
                                percent = CDbl((cSkill.SP + _displayPilot.TrainingCurrentSP - baseSkill.LevelUp(cSkill.Level)) / (baseSkill.LevelUp(cSkill.Level + 1) - baseSkill.LevelUp(cSkill.Level)) * 100)
                            Else
                                percent = (Math.Min(Math.Max(CDbl((cSkill.SP - baseSkill.LevelUp(cSkill.Level)) / (baseSkill.LevelUp(cSkill.Level + 1) - baseSkill.LevelUp(cSkill.Level)) * 100), 0), 100))
                            End If
                        End If
                        If _trainingSkill IsNot Nothing Then
                            _trainingSkill.Cells(3).Text = percent.ToString("N0") & "%"
                            _trainingSkill.Cells(3).Tag = percent
                            _trainingSkill.Cells(4).Text = (cSkill.SP + _displayPilot.TrainingCurrentSP).ToString("N0")
                            _trainingSkill.Cells(4).Tag = cSkill.SP
                            _trainingSkill.Cells(5).Text = SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                            _trainingSkill.Cells(5).Tag = _displayPilot.TrainingCurrentTime
                        End If
                        'TrainingSkill.Cells(2).HostedControl.Refresh()
                        If chkGroupSkills.Checked = True And _trainingGroup IsNot Nothing Then
                            _trainingGroup.Cells(4).Text = "<font color=""#FFD700"">" & (CLng(_trainingGroup.Cells(4).Tag) + _displayPilot.TrainingCurrentSP).ToString("N0") & "</font>"
                            _trainingGroup.Text = _trainingGroup.Tag.ToString & "<font color=""#FFD700"">  - Training</font>"
                            'TrainingGroup.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
                        End If
                        lblTrainingTime.Text = SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                        Select Case _displayPilot.TrainingCurrentTime
                            Case 0 To 86400
                                lblTrainingTime.ForeColor = Color.Red
                            Case Else
                                lblTrainingTime.ForeColor = Color.Black
                        End Select
                    Else
                        ' Cache corruption here??
                        If frmEveHQ.CacheErrorHandler() = True Then Exit Sub
                    End If
                End If

                ' Check Clone
                If (_displayPilot.SkillPoints + _displayPilot.TrainingCurrentSP) > CLng(_displayPilot.CloneSP) Then
                    lblPilotClone.ForeColor = Color.Red
                Else
                    lblPilotClone.ForeColor = Color.Black
                End If

                ' Display Account Info
                If grpAccount.Visible = True Then
                    Dim dAccount As EveHQAccount = HQ.Settings.Accounts(_displayPilot.Account)
                    lblAccountExpiry.Text = "Expiry: " & dAccount.PaidUntil.ToString & " (" & SkillFunctions.TimeToString((dAccount.PaidUntil - Now).TotalSeconds) & ")"
                    If HQ.Settings.NotifyAccountTime = True Then
                        Dim accountTime As Date = dAccount.PaidUntil
                        If accountTime.Year > 2000 And (accountTime - Now).TotalHours <= HQ.Settings.AccountTimeLimit Then
                            lblAccountExpiry.ForeColor = Color.Red
                        Else
                            lblAccountExpiry.ForeColor = Color.Black
                        End If
                    End If
                End If

                ' Check Cache details!
                Dim cacheDate As Date = SkillFunctions.ConvertEveTimeToLocal(_displayPilot.CacheExpirationTime)
                Dim cacheTimeLeft As TimeSpan = cacheDate - Now
                Dim cacheText As String = Format(cacheDate, "ddd") & " " & cacheDate & ControlChars.CrLf & SkillFunctions.CacheTimeToString(cacheTimeLeft.TotalSeconds)
                If cacheDate < Now Then
                    lblCharacterXML.ForeColor = Color.Green
                    HQ.APIUpdateAvailable = True
                    btnUpdateAPI.Enabled = True
                Else
                    lblCharacterXML.ForeColor = Color.Red
                    HQ.APIUpdateAvailable = False
                    btnUpdateAPI.Enabled = False
                End If
                lblCharacterXML.Text = cacheText
            End If

        End Sub
#End Region

#Region "UI Routines"

        Private Sub mnuViewDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewDetails.Click
            Dim skillID As Integer = CInt(mnuSkillName.Tag)
            frmSkillDetails.DisplayPilotName = _displayPilot.Name
            Call frmSkillDetails.ShowSkillDetails(skillID)
        End Sub

        Private Sub ctxSkills_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxSkills.Opening
            If adtSkills.SelectedNodes.Count <> 0 Then
                If adtSkills.SelectedNodes(0).Nodes.Count = 0 Then
                    Dim skillName As String = adtSkills.SelectedNodes(0).Text
                    Dim skillID As Integer = SkillFunctions.SkillNameToID(skillName)
                    mnuSkillName.Text = skillName
                    mnuSkillName.Tag = skillID
                Else
                    e.Cancel = True
                End If
            Else
                e.Cancel = True
            End If
        End Sub

        Private Sub adtSkills_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtSkills.ColumnHeaderMouseDown
            Dim ch As ColumnHeader = CType(sender, ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

        Private Sub adtSkills_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtSkills.NodeDoubleClick
            Dim openSkillDetails As Boolean = False
            If chkGroupSkills.Checked = True Then
                If e.Node.Level = 1 Then
                    openSkillDetails = True
                End If
            Else
                If e.Node.Level = 0 Then
                    openSkillDetails = True
                End If
            End If

            If openSkillDetails = True Then
                Dim skillID As Integer = SkillFunctions.SkillNameToID(e.Node.Text)
                frmSkillDetails.DisplayPilotName = _displayPilot.Name
                Call frmSkillDetails.ShowSkillDetails(skillID)
            End If
        End Sub

#End Region

#Region "Portrait Related Routines"

        Private Sub mnuCtxPicGetPortraitFromServer_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuCtxPicGetPortraitFromServer.Click
            If _displayPilot.ID <> "" Then
                picPilot.ImageLocation = "http://image.eveonline.com/Character/" & _displayPilot.ID & "_256.jpg"
            End If
        End Sub
        Private Sub mnuCtxPicGetPortraitFromLocal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuCtxPicGetPortraitFromLocal.Click
            ' If double-clicked, see if we can get it from the eve portrait folder
            For folder As Integer = 1 To 4
                Dim folderName As String
                If HQ.Settings.EveFolderLua(folder) = False Then
                    Dim eveSettingsFolder As String = HQ.Settings.EveFolder(folder)
                    If eveSettingsFolder IsNot Nothing Then
                        eveSettingsFolder = eveSettingsFolder.Replace("\", "_").Replace(":", "").Replace(" ", "_").ToLower & "_tranquility"
                        Dim eveFolder As String = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "EVE")
                        folderName = Path.Combine(Path.Combine(Path.Combine(Path.Combine(eveFolder, eveSettingsFolder), "cache"), "Pictures"), "Portraits")
                    Else
                        folderName = ""
                    End If
                Else
                    folderName = Path.Combine(Path.Combine(Path.Combine(HQ.Settings.EveFolder(folder), "cache"), "Pictures"), "Portraits")
                End If
                If My.Computer.FileSystem.DirectoryExists(folderName) = True Then
                    For Each foundFile As String In My.Computer.FileSystem.GetFiles(folderName, SearchOption.SearchTopLevelOnly, "*.png")
                        If foundFile.Contains(_displayPilot.ID & "_") = True Then
                            ' Get the dimensions of the file
                            Dim myFile As New FileInfo(foundFile)
                            Dim fileData As String() = myFile.Name.Split(New Char() {CChar("_"), CChar(".")})
                            If CInt(fileData(1)) >= 128 And CInt(fileData(1)) <= 256 Then
                                picPilot.Image = ImageHandler.GetPortraitImage(_displayPilot.ID)
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            Next
            MessageBox.Show("The requested portrait was not found within the Eve cache locations.", "Portrait Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub
        Private Sub mnuSavePortrait_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuSavePortrait.Click
            Dim imgFilename As String = _displayPilot.ID & ".png"
            picPilot.Image.Save(Path.Combine(HQ.imageCacheFolder, imgFilename))
        End Sub
#End Region

        Private Sub chkGroupSkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkGroupSkills.CheckedChanged
            If DisplayPilotName <> "" Then
                Call DisplaySkills()
                Call DisplayCertificates()
            End If
        End Sub

        Private Sub ctxCerts_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxCerts.Opening
            If adtCerts.SelectedNodes.Count <> 0 Then
                If adtCerts.SelectedNodes(0).Nodes.Count = 0 Then
                    Dim certName As String = adtCerts.SelectedNodes(0).Text
                    Dim certGrade As String = adtCerts.SelectedNodes(0).Cells(1).Text
                    Dim certID As String = adtCerts.SelectedNodes(0).Tag.ToString
                    mnuCertName.Text = certName & " (" & certGrade & ")"
                    mnuCertName.Tag = certID
                Else
                    e.Cancel = True
                End If
            Else
                e.Cancel = True
            End If
        End Sub

        Private Sub mnuViewCertDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuViewCertDetails.Click
            Dim certID As Integer = CInt(mnuCertName.Tag)
            frmCertificateDetails.Text = mnuCertName.Text
            frmCertificateDetails.DisplayPilotName = _displayPilot.Name
            frmCertificateDetails.ShowCertDetails(certID)
        End Sub

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            If HQ.Settings.Pilots.ContainsKey(cboPilots.SelectedItem.ToString) = True Then
                _displayPilot = HQ.Settings.Pilots(cboPilots.SelectedItem.ToString)
                Call UpdatePilotInfo()
            End If
        End Sub

#Region "Standings Routines"

        Private Sub btnGetStandings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetStandings.Click

            ' Establish which pilot we are talking about
            If DisplayPilotName <> "" Then
                Cursor = Cursors.WaitCursor
                btnGetStandings.Enabled = False
                Standings.GetStandings(DisplayPilotName)
                Call UpdateStandingsList()
                Cursor = Cursors.Default
                btnGetStandings.Enabled = True
            End If

        End Sub
        Private Sub btExportStandings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btExportStandings.Click
            Try
                If cboPilots.SelectedItem IsNot Nothing Then
                    If adtStandings.Nodes.Count > 0 Then
                        ' Export the current list of standings
                        Dim sw As New StreamWriter(Path.Combine(HQ.reportFolder, "Standings (" & cboPilots.SelectedItem.ToString & ").csv"))
                        sw.WriteLine("Standings Export for " & cboPilots.SelectedItem.ToString & " (dated: " & Now.ToString & ")")
                        sw.WriteLine("Entity Name,Entity ID,Entity Type,Raw Standing Value,Actual Standing Value")
                        For Each iStanding As Node In adtStandings.Nodes
                            sw.Write(iStanding.Text & HQ.Settings.CsvSeparatorChar)
                            sw.Write(iStanding.Cells(1).Text & HQ.Settings.CsvSeparatorChar)
                            sw.Write(iStanding.Cells(2).Text & HQ.Settings.CsvSeparatorChar)
                            sw.WriteLine(iStanding.Cells(3).Text & HQ.Settings.CsvSeparatorChar & iStanding.Cells(4).Text)
                        Next
                        sw.Flush()
                        sw.Close()
                        MessageBox.Show("CSV Standings file for " & cboPilots.SelectedItem.ToString & " successfully written to the EveHQ report folder!", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        MessageBox.Show("There are no standings to export for " & cboPilots.SelectedItem.ToString & "!", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    MessageBox.Show("You need to select an Owner before exporting standings!", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show("Export of CSV Standings file failed:" & ControlChars.CrLf & ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub
        Private Sub cboFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFilter.SelectedIndexChanged
            If cboFilter.Tag.ToString <> "0" Then
                Call UpdateStandingsList()
            End If
            cboFilter.Tag = "1"
        End Sub
        Private Sub UpdateStandingsList()

            Dim diplomacyLevel As Integer = 0
            Dim connectionsLevel As Integer = 0
            Dim rawStanding As Double
            Dim effStanding As Double = 0

            ' Check if this is a character and whether we need to get the Connections and Diplomacy skills
            For Each cSkill As EveHQPilotSkill In _displayPilot.PilotSkills.Values
                If cSkill.Name = "Diplomacy" Then
                    diplomacyLevel = cSkill.Level
                End If
                If cSkill.Name = "Connections" Then
                    connectionsLevel = cSkill.Level
                End If
            Next

            adtStandings.BeginUpdate()
            adtStandings.Nodes.Clear()

            For Each standing As PilotStanding In _displayPilot.Standings.Values

                If standing.Standing <> 0 Then

                    rawStanding = standing.Standing

                    Select Case standing.Type
                        Case StandingType.Agent, StandingType.Faction, StandingType.NPCCorporation
                            If rawStanding < 0 Then
                                effStanding = rawStanding + ((10 - rawStanding) * (diplomacyLevel * 4 / 100))
                            Else
                                effStanding = rawStanding + ((10 - rawStanding) * (connectionsLevel * 4 / 100))
                            End If
                        Case StandingType.PlayerCorp, StandingType.Unknown
                            effStanding = rawStanding
                    End Select

                    Dim show As Boolean = False
                    Select Case cboFilter.SelectedItem.ToString
                        Case "<All>"
                            show = True
                        Case "Agent"
                            If standing.Type = StandingType.Agent Then
                                show = True
                            End If
                        Case "Corporation"
                            If standing.Type = StandingType.NPCCorporation Then
                                show = True
                            End If
                        Case "Faction"
                            If standing.Type = StandingType.Faction Then
                                show = True
                            End If
                        Case "Player/Corp"
                            If standing.Type = StandingType.PlayerCorp Then
                                show = True
                            End If
                    End Select

                    If show = True Then
                        Dim newStanding As New Node(standing.Name)
                        'Select Case Standing.Type
                        '    Case Core.StandingType.Agent
                        '        newStanding.Image = Core.ImageHandler.GetPortraitImage(Standing.ID.ToString, 32)
                        '    Case Core.StandingType.PlayerCorp
                        '        newStanding.Image = Core.ImageHandler.GetPortraitImage(Standing.ID.ToString, 32)
                        '    Case Core.StandingType.Faction
                        '        newStanding.Image = Core.ImageHandler.GetCorpImage(Standing.ID.ToString, 32)
                        '    Case Core.StandingType.NPCCorporation
                        '        newStanding.Image = Core.ImageHandler.GetCorpImage(Standing.ID.ToString, 32)
                        'End Select
                        newStanding.Cells.Add(New Cell(standing.ID.ToString))
                        newStanding.Cells.Add(New Cell(standing.Type.ToString))
                        newStanding.Cells.Add(New Cell(rawStanding.ToString("N2")))
                        newStanding.Cells.Add(New Cell(effStanding.ToString("N2")))
                        newStanding.Cells(2).Tag = rawStanding
                        newStanding.Cells(3).Tag = effStanding
                        adtStandings.Nodes.Add(newStanding)
                    End If

                End If

            Next
            AdvTreeSorter.Sort(adtStandings, New AdvTreeSortResult(5, AdvTreeSortOrder.Descending), False)
            adtStandings.EndUpdate()
        End Sub
        Private Sub ctxStandings_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxStandings.Opening
            If adtStandings.SelectedNodes.Count = 0 Then
                e.Cancel = True
            End If
        End Sub
        Private Sub mnuExtrapolateStandings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuExtrapolateStandings.Click
            If adtStandings.SelectedNodes.Count >= 1 Then
                Dim standingsLine As Node = adtStandings.SelectedNodes(0)
                Using extraStandings As New FrmExtraStandings
                    extraStandings.Pilot = standingsLine.Name
                    extraStandings.Party = standingsLine.Text
                    extraStandings.Standing = CDbl(standingsLine.Cells(2).Tag)
                    extraStandings.BaseStanding = CDbl(standingsLine.Cells(3).Tag)
                    extraStandings.ShowDialog()
                End Using
            End If
        End Sub
        Private Sub adtStandings_ColumnHeaderMouseUp(sender As Object, e As MouseEventArgs) Handles adtStandings.ColumnHeaderMouseUp
            Dim ch As ColumnHeader = CType(sender, ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

#End Region

        Private Sub chkManImplants_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkManImplants.CheckedChanged
            If chkManImplants.Checked = True Then
                _displayPilot.UseManualImplants = True
                btnEditManualImplants.Enabled = True
            Else
                _displayPilot.UseManualImplants = False
                btnEditManualImplants.Enabled = False
            End If
            Call UpdatePilotInfo()
        End Sub

        Private Sub btnEditManualImplants_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditManualImplants.Click
            frmEditImplants.DisplayPilotName = _displayPilot.Name
            frmEditImplants.ShowDialog()
            Call UpdatePilotInfo()
        End Sub

        Private Sub btnUpdateAPI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateAPI.Click
            btnUpdateAPI.Enabled = False
            Call frmEveHQ.QueryMyEveServer()
        End Sub

        Private Sub adtCerts_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtCerts.ColumnHeaderMouseDown
            Dim ch As ColumnHeader = CType(sender, ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

        Private Sub adtSkills_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtSkills.SelectionChanged
            adtSkills.Refresh()
        End Sub

        Private Sub pnlInfo_MouseEnter(sender As Object, e As EventArgs) Handles pnlInfo.MouseEnter
            pnlInfo.Focus()
        End Sub

        Private Sub adtSkills_NodeClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtSkills.NodeClick
            If e.Node.Level = 0 Then
                e.Node.Toggle()
            End If
        End Sub

        Private Sub adtCerts_NodeClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtCerts.NodeClick
            If e.Node.Level = 0 Then
                e.Node.Toggle()
            End If
        End Sub

    End Class
End Namespace