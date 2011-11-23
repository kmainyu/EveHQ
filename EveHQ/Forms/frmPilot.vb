' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar

Public Class frmPilot
    Dim TrainingSkill As Node
    Dim TrainingGroup As Node
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

    Private Sub frmPilot_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call UpdatePilots()
    End Sub

    Public Sub UpdatePilots()

        ' Update standings filter
        Me.cboFilter.SelectedIndex = 0

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

        ' Select a pilot
        If cDisplayPilotName <> "" Then
            If cboPilots.Items.Count > 0 Then
                If cboPilots.Items.Contains(cDisplayPilotName) = True Then
                    cboPilots.SelectedItem = cDisplayPilotName
                Else
                    cboPilots.SelectedIndex = 0
                End If
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
                picPilot.Image = EveHQ.Core.ImageHandler.GetPortraitImage(displayPilot)

                Call EveHQ.Core.PilotParseFunctions.SwitchImplants(displayPilot)

            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
                MessageBox.Show(msg, "Error Retrieving Cached Image", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Update Race image
            picRace.Image = CType(My.Resources.ResourceManager.GetObject(displayPilot.Race.Replace("-", "_") & "Race"), Image)
            ' Update Blood image
            picBlood.Image = CType(My.Resources.ResourceManager.GetObject(displayPilot.Blood.Replace("-", "_") & "Blood"), Image)

            ' Display Information
            Try
                lblPilotName.Text = displayPilot.Name
                lblPilotID.Text = displayPilot.ID
                lblPilotCorp.Text = displayPilot.Corp
                lblPilotIsk.Text = displayPilot.Isk.ToString("N2")
                lblPilotSP.Text = (displayPilot.SkillPoints + EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(displayPilot)).ToString("N0")
                lblPilotClone.Text = displayPilot.CloneName & " (" & CLng(displayPilot.CloneSP).ToString("N0") & " SP)"
                ' Check Clone
                If (displayPilot.SkillPoints + displayPilot.TrainingCurrentSP) > CLng(displayPilot.CloneSP) Then
                    lblPilotClone.ForeColor = Color.Red
                Else
                    lblPilotClone.ForeColor = Color.Black
                End If
            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Information", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Implant method
            If displayPilot.UseManualImplants = True Then
                Me.chkManImplants.Checked = True
            Else
                Me.chkManImplants.Checked = False
            End If

            ' Display Attribute & Implant Information
            Try
                lblCharismaTotal.Text = displayPilot.CAttT.ToString("N1")
                lblIntelligenceTotal.Text = displayPilot.IAttT.ToString("N1")
                lblMemoryTotal.Text = displayPilot.MAttT.ToString("N1")
                lblPerceptionTotal.Text = displayPilot.PAttT.ToString("N1")
                lblWillpowerTotal.Text = displayPilot.WAttT.ToString("N1")

                lblCharismaDetail.Text = "( " & displayPilot.CAtt.ToString & " Base +  " & displayPilot.CImplant.ToString & " Implant)"
                lblIntelligenceDetail.Text = "( " & displayPilot.IAtt.ToString & " Base +  " & displayPilot.IImplant.ToString & " Implant)"
                lblMemoryDetail.Text = "( " & displayPilot.MAtt.ToString & " Base +  " & displayPilot.MImplant.ToString & " Implant)"
                lblPerceptionDetail.Text = "( " & displayPilot.PAtt.ToString & " Base +  " & displayPilot.PImplant.ToString & " Implant)"
                lblWillpowerDetail.Text = "( " & displayPilot.WAtt.ToString & " Base +  " & displayPilot.WImplant.ToString & " Implant)"

            Catch e As Exception
                Dim msg As String = "An error has occurred:" & ControlChars.CrLf & ControlChars.CrLf & e.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Pilot Name: " & displayPilot.Name
                MessageBox.Show(msg, "Error Displaying Pilot Attributes", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            ' Display Skill Training
            Try
                If displayPilot.Training = True Then
                    Dim currentSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)), Core.PilotSkill)
                    If displayPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)) = True Then
                        currentSkill = CType(displayPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)), Core.PilotSkill)
                    Else
                        MessageBox.Show("Missing the training skill from the skills!!")
                    End If
                    lblTrainingSkill.Text = currentSkill.Name & " (Level " & EveHQ.Core.SkillFunctions.Roman(displayPilot.TrainingSkillLevel) & ")"
                    lblTrainingRate.Text = "Rank " & currentSkill.Rank & " @ " & EveHQ.Core.SkillFunctions.CalculateSPRate(displayPilot, EveHQ.Core.HQ.SkillListID(currentSkill.ID)).ToString("N0") & " SP/Hr"
                    Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(displayPilot.TrainingEndTime)
                    lblTrainingEnds.Text = Format(localdate, "ddd") & " " & localdate
                    lblTrainingTime.Text = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    Select Case displayPilot.TrainingCurrentTime
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
            If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(displayPilot.Account) = True Then
                Dim dAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(displayPilot.Account), Core.EveAccount)
                If (dAccount.APIKeySystem = Core.APIKeySystems.Version2 And dAccount.CanUseCharacterAPI(EveAPI.CharacterAccessMasks.AccountStatus)) Then
                    lblAccountExpiry.Text = "Expiry: " & dAccount.PaidUntil.ToString & " (" & EveHQ.Core.SkillFunctions.TimeToString((dAccount.PaidUntil - Now).TotalSeconds) & ")"
                    lblAccountLogins.Text = "Login Count: " & dAccount.LogonCount & " (" & EveHQ.Core.SkillFunctions.TimeToString(dAccount.LogonMinutes * 60, False) & ")"
                    If EveHQ.Core.HQ.EveHQSettings.NotifyAccountTime = True Then
                        Dim AccountTime As Date = dAccount.PaidUntil
                        If AccountTime.Year > 2000 And (AccountTime - Now).TotalHours <= EveHQ.Core.HQ.EveHQSettings.AccountTimeLimit Then
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
            Call Me.DisplaySkills()
            Call Me.DisplayCertificates()

            ' Update skill queue
            Me.sqcEveQueue.PilotName = displayPilot.Name

            ' Update Standings stuff
            Call Me.UpdateStandingsList()

        Else
            adtSkills.Nodes.Clear()
            adtCerts.Nodes.Clear()
            ' Get image from cache
            If displayPilot.ID = "" Then
                picPilot.Image = My.Resources.noitem
            End If
        End If
    End Sub

    Private Sub DisplaySkills()
        Dim maxGroups As Integer = 16
        Dim groupHeaders(16, 3) As String
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

        ' Set Styles
        Dim SkillGroupStyle As ElementStyle = adtSkills.Styles("SkillGroup").Copy
        SkillGroupStyle.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor))
        SkillGroupStyle.BackColor2 = Color.Black
        SkillGroupStyle.TextColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor))
        Dim NormalSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
        NormalSkillStyle.BackColor2 = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor))
        NormalSkillStyle.BackColor = Color.FromArgb(128, NormalSkillStyle.BackColor2)
        NormalSkillStyle.TextColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
        Dim PartialSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
        PartialSkillStyle.BackColor2 = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor))
        PartialSkillStyle.BackColor = Color.FromArgb(128, PartialSkillStyle.BackColor2)
        PartialSkillStyle.TextColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
        Dim Level5SkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
        Level5SkillStyle.BackColor2 = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor))
        Level5SkillStyle.BackColor = Color.FromArgb(128, Level5SkillStyle.BackColor2)
        Level5SkillStyle.TextColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
        Dim TrainingSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
        TrainingSkillStyle.BackColor2 = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor))
        TrainingSkillStyle.BackColor = Color.FromArgb(128, TrainingSkillStyle.BackColor2)
        TrainingSkillStyle.TextColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
        Dim SelSkillStyle As ElementStyle = adtSkills.Styles("Skill").Copy
        SelSkillStyle.BackColor2 = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor))
        SelSkillStyle.BackColor = Color.FromArgb(32, SelSkillStyle.BackColor2)

        ' Set up Groups

        adtSkills.Refresh()
        adtSkills.BeginUpdate()
        adtSkills.Nodes.Clear()

        Dim groupStructure As New SortedList
        If chkGroupSkills.Checked = True Then
            For group As Integer = 0 To maxGroups
                Dim newSkillGroup As New DevComponents.AdvTree.Node("", SkillGroupStyle)
                newSkillGroup.FullRowBackground = True
                For Cell As Integer = 1 To 5
                    newSkillGroup.Cells.Add(New Cell)
                    newSkillGroup.Cells(Cell).Tag = 0
                Next
                newSkillGroup.Text = groupHeaders(group, 0)
                adtSkills.Nodes.Add(newSkillGroup)
                groupStructure.Add(groupHeaders(group, 1), newSkillGroup)
            Next
        End If

        ' Parse in-game skill queue
        Dim EveSkillsQueued As New SortedList(Of String, Integer)
        For Each QueuedSkill As EveHQ.Core.PilotQueuedSkill In displayPilot.QueuedSkills.Values
            If EveSkillsQueued.ContainsKey(QueuedSkill.SkillID.ToString) = False Then
                EveSkillsQueued.Add(QueuedSkill.SkillID.ToString, QueuedSkill.Level)
            Else
                If QueuedSkill.Level > EveSkillsQueued(QueuedSkill.SkillID.ToString) Then
                    EveSkillsQueued(QueuedSkill.SkillID.ToString) = QueuedSkill.Level
                End If
            End If
        Next

        ' Set up items
        For Each cSkill As EveHQ.Core.PilotSkill In displayPilot.PilotSkills
            Try
                Dim groupCLV As Node = CType(groupStructure(cSkill.GroupID), Node)
                Dim newCLVItem As New DevComponents.AdvTree.Node
                newCLVItem.FullRowBackground = True
                newCLVItem.Text = cSkill.Name
                newCLVItem.Style = NormalSkillStyle
                newCLVItem.StyleSelected = SelSkillStyle
                If chkGroupSkills.Checked = True Then
                    groupCLV.Nodes.Add(newCLVItem)
                Else
                    adtSkills.Nodes.Add(newCLVItem)
                End If
                newCLVItem.Cells.Add(New Cell(cSkill.Rank.ToString))
                newCLVItem.Cells(1).Tag = cSkill.Rank

                newCLVItem.Cells.Add(New Cell)
                If EveSkillsQueued.ContainsKey(cSkill.ID) Then
                    If EveSkillsQueued(cSkill.ID) > cSkill.Level Then
                        newCLVItem.Cells(2).Images.Image = CType(My.Resources.ResourceManager.GetObject("level_" & cSkill.Level.ToString & EveSkillsQueued(cSkill.ID).ToString & "0"), Image)
                        If groupCLV IsNot Nothing Then
                            If groupCLV.Cells(2).Tag IsNot Nothing Then
                                groupCLV.Cells(2).Tag = CInt(groupCLV.Cells(2).Tag) + 1
                            Else
                                groupCLV.Cells(2).Tag = 1
                            End If
                        End If
                    Else
                        newCLVItem.Cells(2).Images.Image = CType(My.Resources.ResourceManager.GetObject("level_" & cSkill.Level.ToString & "00"), Image)
                    End If
                Else
                    newCLVItem.Cells(2).Images.Image = CType(My.Resources.ResourceManager.GetObject("level_" & cSkill.Level.ToString & "00"), Image)
                End If
                newCLVItem.Cells(2).Tag = cSkill.Level

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
                newCLVItem.Cells.Add(New Cell(percent.ToString("N0") & "%"))
                newCLVItem.Cells(3).Tag = percent.ToString("N2")

                ' Write skillpoints
                newCLVItem.Cells.Add(New Cell(cSkill.SP.ToString("N0")))
                newCLVItem.Cells(4).Tag = cSkill.SP

                If chkGroupSkills.Checked = True Then
                    For skillGroup As Integer = 0 To maxGroups
                        If cSkill.GroupID = groupHeaders(skillGroup, 1) Then
                            'newLine.Group = lvSkills.Groups.Item(skillGroup)
                            groupHeaders(skillGroup, 2) = CStr(CDbl(groupHeaders(skillGroup, 2)) + cSkill.SP)
                            groupHeaders(skillGroup, 3) = CStr(CDbl(groupHeaders(skillGroup, 3)) + 1)
                            groupCLV.Text = groupHeaders(skillGroup, 0) & " - skills: " & groupHeaders(skillGroup, 3)
                            If groupCLV.Cells(2).Tag IsNot Nothing Then
                                If CInt(groupCLV.Cells(2).Tag) > 0 Then
                                    groupCLV.Text &= "<font color=""#0085AB"">  (" & groupCLV.Cells(2).TagString & " in queue)</font>"
                                End If
                            End If
                            groupCLV.Tag = groupCLV.Text
                            groupCLV.Cells(4).Text = CDbl(groupHeaders(skillGroup, 2)).ToString("N0")
                            groupCLV.Cells(4).Tag = groupHeaders(skillGroup, 2)
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
                    TrainingSkill = newCLVItem
                    TrainingGroup = groupCLV
                Else
                    currentTime = CLng(EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, EveHQ.Core.HQ.SkillListID(cSkill.ID), 0, ))
                    TimeSubItem.Text = EveHQ.Core.SkillFunctions.TimeToString(currentTime)
                End If
                If currentTime = 0 Then currentTime = 9999999999
                newCLVItem.Cells.Add(New Cell(TimeSubItem.Text))
                newCLVItem.Cells(5).Tag = currentTime.ToString

                ' Select colours for line background
                If cSkill.Level = 5 Then
                    newCLVItem.Style = Level5SkillStyle
                Else
                    If displayPilot.TrainingSkillID = cSkill.ID Then
                        Dim lvFont As Font = New Font(adtSkills.Font, FontStyle.Bold)
                        'newCLVItem.Font = lvFont
                        newCLVItem.Style = TrainingSkillStyle
                        If chkGroupSkills.Checked = True Then
                            groupCLV.Text = groupCLV.Tag.ToString & "<font color=""#FFD700"">  - Training</font>"
                            groupCLV.Cells(4).Text = "<font color=""#FFD700"">" & groupCLV.Cells(4).Text & "</font>"
                            'groupCLV.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
                        End If
                    Else
                        If partially = True Then
                            newCLVItem.Style = PartialSkillStyle
                        End If
                    End If
                End If

            Catch e As Exception
                If frmEveHQ.CacheErrorHandler() = True Then Exit Sub
            End Try
        Next

        ' Remove empty groups
        If chkGroupSkills.Checked = True Then
            Dim SG As New Node
            Dim SGNo As Integer = 0
            Do
                SG = adtSkills.Nodes(SGNo)
                If SG.Nodes.Count = 0 Then
                    adtSkills.Nodes.Remove(SG)
                    SGNo -= 1
                End If
                SGNo += 1
            Loop Until SGNo = adtSkills.Nodes.Count
        End If

        EveHQ.Core.AdvTreeSorter.Sort(adtSkills, 1, True, True)
        adtSkills.EndUpdate()
        If chkGroupSkills.Checked = True Then
            If TrainingGroup IsNot Nothing Then
                TrainingGroup.Cells(4).Text = "<font color=""#FFD700"">" & (CLng(TrainingGroup.Cells(4).Tag) + displayPilot.TrainingCurrentSP).ToString("N0") & "</font>"
                TrainingGroup.Text = TrainingGroup.Tag.ToString & "<font color=""#FFD700"">  - Training</font>"
                'TrainingGroup.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
            End If
        End If
    End Sub

    Private Sub DisplayCertificates()

        Dim cCert As EveHQ.Core.Certificate

        ' Filter out the lower end certificates
        Dim certList As New SortedList
        For Each cCertID As String In displayPilot.Certificates
            If EveHQ.Core.HQ.Certificates.ContainsKey(cCertID) Then
                cCert = EveHQ.Core.HQ.Certificates(cCertID)
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

        ' Set Styles
        Dim CertGroupStyle As ElementStyle = adtSkills.Styles("SkillGroup").Copy
        CertGroupStyle.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor))
        CertGroupStyle.BackColor2 = Color.Black
        CertGroupStyle.TextColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor))
        Dim NormalCertStyle As ElementStyle = adtSkills.Styles("Skill").Copy
        NormalCertStyle.BackColor2 = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor))
        NormalCertStyle.BackColor = Color.FromArgb(128, NormalCertStyle.BackColor2)
        NormalCertStyle.TextColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
        Dim SelCertStyle As ElementStyle = adtSkills.Styles("Skill").Copy
        SelCertStyle.BackColor2 = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor))
        SelCertStyle.BackColor = Color.FromArgb(32, SelCertStyle.BackColor2)

        'Set up Groups
        adtCerts.BeginUpdate()
        adtCerts.Nodes.Clear()

        Dim certGroups As New SortedList

        If chkGroupSkills.Checked = True Then
            For Each cCategory As EveHQ.Core.CertificateCategory In EveHQ.Core.HQ.CertificateCategories.Values
                Dim newCertGroup As New DevComponents.AdvTree.Node("", CertGroupStyle)
                newCertGroup.FullRowBackground = True
                For Cell As Integer = 1 To 1
                    newCertGroup.Cells.Add(New Cell)
                    newCertGroup.Cells(Cell).Tag = 0
                Next
                newCertGroup.Text = cCategory.Name
                newCertGroup.Tag = 0
                certGroups.Add(cCategory.ID.ToString, newCertGroup)
                adtCerts.Nodes.Add(newCertGroup)
            Next
        End If

        'Set up items

        For Each cCert In certList.Values
            Dim certGroup As Node = CType(certGroups(cCert.CategoryID.ToString), Node)
            Dim newCert As New Node("", NormalCertStyle)
            newCert.FullRowBackground = True
            newCert.Text = EveHQ.Core.HQ.CertificateClasses(cCert.ClassID.ToString).Name
            newCert.Tag = cCert.ID
            If chkGroupSkills.Checked = True Then
                certGroup.Nodes.Add(newCert)
                certGroup.Tag = CInt(certGroup.Tag) + 1
            Else
                adtCerts.Nodes.Add(newCert)
            End If
            newCert.StyleSelected = SelCertStyle
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
            Dim SG As New Node
            Dim SGNo As Integer = 0
            Do
                SG = adtCerts.Nodes(SGNo)
                If SG.Nodes.Count = 0 Then
                    adtCerts.Nodes.Remove(SG)
                    SGNo -= 1
                End If
                SGNo += 1
            Loop Until SGNo = adtCerts.Nodes.Count
        End If

        EveHQ.Core.AdvTreeSorter.Sort(adtCerts, 1, True, True)
        adtCerts.EndUpdate()
    End Sub

#Region "Skill Update Routine"
    Public Sub UpdateSkillInfo()
        If displayPilot.PilotSkills.Count <> 0 Then
            If displayPilot.Training = True Then
                lblPilotSP.Text = (displayPilot.SkillPoints + displayPilot.TrainingCurrentSP).ToString("N0")
                If displayPilot.PilotSkills.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)) = True Then
                    Dim cSkill As EveHQ.Core.PilotSkill = CType(displayPilot.PilotSkills(EveHQ.Core.SkillFunctions.SkillIDToName(displayPilot.TrainingSkillID)), Core.PilotSkill)
                    Dim percent As Double = 0
                    If cSkill.Level = 5 Then
                        percent = 100
                    Else
                        If displayPilot.TrainingSkillID = cSkill.ID Then
                            percent = CDbl((cSkill.SP + displayPilot.TrainingCurrentSP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100)
                        Else
                            percent = (Math.Min(Math.Max(CDbl((cSkill.SP - cSkill.LevelUp(cSkill.Level)) / (cSkill.LevelUp(cSkill.Level + 1) - cSkill.LevelUp(cSkill.Level)) * 100), 0), 100))
                        End If
                    End If
                    TrainingSkill.Cells(3).Text = percent.ToString("N0") & "%"
                    TrainingSkill.Cells(3).Tag = percent
                    TrainingSkill.Cells(4).Text = (cSkill.SP + displayPilot.TrainingCurrentSP).ToString("N0")
                    TrainingSkill.Cells(4).Tag = cSkill.SP
                    TrainingSkill.Cells(5).Text = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    TrainingSkill.Cells(5).Tag = displayPilot.TrainingCurrentTime
                    'TrainingSkill.Cells(2).HostedControl.Refresh()
                    If chkGroupSkills.Checked = True Then
                        TrainingGroup.Cells(4).Text = "<font color=""#FFD700"">" & (CLng(TrainingGroup.Cells(4).Tag) + displayPilot.TrainingCurrentSP).ToString("N0") & "</font>"
                        TrainingGroup.Text = TrainingGroup.Tag.ToString & "<font color=""#FFD700"">  - Training</font>"
                        'TrainingGroup.Font = New Font(TrainingGroup.Font, FontStyle.Bold)
                    End If
                    Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(displayPilot.TrainingEndTime)
                    lblTrainingTime.Text = EveHQ.Core.SkillFunctions.TimeToString(displayPilot.TrainingCurrentTime)
                    Select Case displayPilot.TrainingCurrentTime
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
            If (displayPilot.SkillPoints + displayPilot.TrainingCurrentSP) > CLng(displayPilot.CloneSP) Then
                lblPilotClone.ForeColor = Color.Red
            Else
                lblPilotClone.ForeColor = Color.Black
            End If

            ' Display Account Info
            If grpAccount.Visible = True Then
                Dim dAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(displayPilot.Account), Core.EveAccount)
                lblAccountExpiry.Text = "Expiry: " & dAccount.PaidUntil.ToString & " (" & EveHQ.Core.SkillFunctions.TimeToString((dAccount.PaidUntil - Now).TotalSeconds) & ")"
                If EveHQ.Core.HQ.EveHQSettings.NotifyAccountTime = True Then
                    Dim AccountTime As Date = dAccount.PaidUntil
                    If AccountTime.Year > 2000 And (AccountTime - Now).TotalHours <= EveHQ.Core.HQ.EveHQSettings.AccountTimeLimit Then
                        lblAccountExpiry.ForeColor = Color.Red
                    Else
                        lblAccountExpiry.ForeColor = Color.Black
                    End If
                End If
            End If

            ' Check Cache details!
            Dim cacheDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(displayPilot.CacheExpirationTime)
            Dim cacheTimeLeft As TimeSpan = cacheDate - Now
            Dim cacheText As String = Format(cacheDate, "ddd") & " " & cacheDate & ControlChars.CrLf & EveHQ.Core.SkillFunctions.CacheTimeToString(cacheTimeLeft.TotalSeconds)
            If cacheDate < Now Then
                lblCharacterXML.ForeColor = Color.Green
                EveHQ.Core.HQ.APIUpdateAvailable = True
                btnUpdateAPI.Enabled = True
            Else
                lblCharacterXML.ForeColor = Color.Red
                EveHQ.Core.HQ.APIUpdateAvailable = False
                btnUpdateAPI.Enabled = False
            End If
            lblCharacterXML.Text = cacheText
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
        If adtSkills.SelectedNodes.Count <> 0 Then
            If adtSkills.SelectedNodes(0).Nodes.Count = 0 Then
                Dim skillName As String = ""
                Dim skillID As String = ""
                skillName = adtSkills.SelectedNodes(0).Text
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

    Private Sub adtSkills_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtSkills.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub adtSkills_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSkills.NodeDoubleClick
        Dim OpenSkillDetails As Boolean = False
        If chkGroupSkills.Checked = True Then
            If e.Node.Level = 1 Then
                OpenSkillDetails = True
            End If
        Else
            If e.Node.Level = 0 Then
                OpenSkillDetails = True
            End If
        End If

        If OpenSkillDetails = True Then
            Dim skillID As String = ""
            skillID = EveHQ.Core.SkillFunctions.SkillNameToID(e.Node.Text)
            frmSkillDetails.DisplayPilotName = displayPilot.Name
            Call frmSkillDetails.ShowSkillDetails(skillID)
        End If
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
                            picPilot.Image = EveHQ.Core.ImageHandler.GetPortraitImage(displayPilot)
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

    Private Sub chkGroupSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkGroupSkills.CheckedChanged
        If DisplayPilotName <> "" Then
            Call Me.DisplaySkills()
            Call Me.DisplayCertificates()
        End If
    End Sub

    Private Sub ctxCerts_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxCerts.Opening
        If adtCerts.SelectedNodes.Count <> 0 Then
            If adtCerts.SelectedNodes(0).Nodes.Count = 0 Then
                Dim skillName As String = ""
                Dim skillID As String = ""
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

        ' Establish which pilot we are talking about
        If DisplayPilotName <> "" Then
            Cursor = Cursors.WaitCursor
            btnGetStandings.Enabled = False
            EveHQ.Core.Standings.GetStandings(DisplayPilotName)
            Call UpdateStandingsList()
            Cursor = Cursors.Default
            btnGetStandings.Enabled = True
        End If

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
            If cboPilots.SelectedItem IsNot Nothing Then
                If lvwStandings.Items.Count > 0 Then
                    ' Export the current list of standings
                    Dim sw As New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "Standings (" & cboPilots.SelectedItem.ToString & ").csv"))
                    sw.WriteLine("Standings Export for " & cboPilots.SelectedItem.ToString & " (dated: " & Now.ToString & ")")
                    sw.WriteLine("Entity Name,Entity ID,Entity Type,Raw Standing Value,Actual Standing Value")
                    For Each iStanding As ListViewItem In lvwStandings.Items
                        sw.Write(iStanding.Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                        sw.Write(iStanding.SubItems(1).Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                        sw.Write(iStanding.SubItems(2).Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar)
                        sw.WriteLine(iStanding.SubItems(3).Text & EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar & iStanding.SubItems(4).Text)
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
    Private Sub cboFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFilter.SelectedIndexChanged
        If cboFilter.Tag.ToString <> "0" Then
            Call Me.UpdateStandingsList()
        End If
        cboFilter.Tag = "1"
    End Sub
    Private Sub UpdateStandingsList()

        Dim DiplomacyLevel As Integer = 0
        Dim ConnectionsLevel As Integer = 0
        Dim RawStanding As Double = 0
        Dim EffStanding As Double = 0

        ' Check if this is a character and whether we need to get the Connections and Diplomacy skills
        For Each cSkill As EveHQ.Core.PilotSkill In displayPilot.PilotSkills
            If cSkill.Name = "Diplomacy" Then
                DiplomacyLevel = cSkill.Level
            End If
            If cSkill.Name = "Connections" Then
                ConnectionsLevel = cSkill.Level
            End If
        Next

        lvwStandings.BeginUpdate()
        lvwStandings.Items.Clear()

        For Each Standing As EveHQ.Core.PilotStanding In displayPilot.Standings.Values

            If Standing.Standing <> 0 Then

                RawStanding = Standing.Standing

                Select Case Standing.Type
                    Case Core.StandingType.Agent, Core.StandingType.Faction, Core.StandingType.NPCCorporation
                        If RawStanding < 0 Then
                            EffStanding = RawStanding + ((10 - RawStanding) * (DiplomacyLevel * 4 / 100))
                        Else
                            EffStanding = RawStanding + ((10 - RawStanding) * (ConnectionsLevel * 4 / 100))
                        End If
                    Case Core.StandingType.PlayerCorp, Core.StandingType.Unknown
                        EffStanding = RawStanding
                End Select

                Dim show As Boolean = False
                Select Case cboFilter.SelectedItem.ToString
                    Case "<All>"
                        show = True
                    Case "Agent"
                        If Standing.Type = Core.StandingType.Agent Then
                            show = True
                        End If
                    Case "Corporation"
                        If Standing.Type = Core.StandingType.NPCCorporation Then
                            show = True
                        End If
                    Case "Faction"
                        If Standing.Type = Core.StandingType.Faction Then
                            show = True
                        End If
                    Case "Player/Corp"
                        If Standing.Type = Core.StandingType.PlayerCorp Then
                            show = True
                        End If
                End Select

                If show = True Then
                    Dim newStanding As New ListViewItem(Standing.Name)
                    newStanding.SubItems.Add(Standing.ID.ToString)
                    newStanding.SubItems.Add(Standing.Type.ToString)
                    newStanding.SubItems.Add(RawStanding.ToString("N2"))
                    newStanding.SubItems.Add(EffStanding.ToString("N2"))
                    newStanding.SubItems(2).Tag = RawStanding
                    newStanding.SubItems(3).Tag = EffStanding
                    lvwStandings.Items.Add(newStanding)
                End If

            End If

        Next

        lvwStandings.EndUpdate()

    End Sub
    Private Sub ctxStandings_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxStandings.Opening
        If lvwStandings.SelectedItems.Count = 0 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuExtrapolateStandings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExtrapolateStandings.Click
        If lvwStandings.SelectedItems.Count >= 1 Then
            Dim standingsLine As ListViewItem = lvwStandings.SelectedItems(0)
            Dim extraStandings As New frmExtraStandings
            extraStandings.Pilot = standingsLine.Name
            extraStandings.Party = standingsLine.Text
            extraStandings.Standing = CDbl(standingsLine.SubItems(2).Tag)
            extraStandings.BaseStanding = CDbl(standingsLine.SubItems(3).Tag)
            extraStandings.ShowDialog()
        End If
    End Sub

#End Region

    Private Sub chkManImplants_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkManImplants.CheckedChanged
        If chkManImplants.Checked = True Then
            displayPilot.UseManualImplants = True
            btnEditManualImplants.Enabled = True
        Else
            displayPilot.UseManualImplants = False
            btnEditManualImplants.Enabled = False
        End If
        Call Me.UpdatePilotInfo()
    End Sub

    Private Sub btnEditManualImplants_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditManualImplants.Click
        frmEditImplants.DisplayPilotName = displayPilot.Name
        frmEditImplants.ShowDialog()
        Call Me.UpdatePilotInfo()
    End Sub

    Private Sub btnUpdateAPI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateAPI.Click
        btnUpdateAPI.Enabled = False
        Call frmEveHQ.QueryMyEveServer()
    End Sub

    Private Sub adtCerts_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtCerts.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub adtSkills_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtSkills.SelectionChanged
        adtSkills.Refresh()
    End Sub

    Private Sub pnlInfo_MouseEnter(sender As Object, e As System.EventArgs) Handles pnlInfo.MouseEnter
        pnlInfo.Focus()
    End Sub

    Private Sub adtSkills_NodeClick(sender As Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtSkills.NodeClick
        If e.Node.Level = 0 Then
            e.Node.Toggle()
        End If
    End Sub

    Private Sub adtCerts_NodeClick(sender As Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtCerts.NodeClick
        If e.Node.Level = 0 Then
            e.Node.Toggle()
        End If
    End Sub

End Class

