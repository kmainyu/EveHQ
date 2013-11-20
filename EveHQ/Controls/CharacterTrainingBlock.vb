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
Imports System.IO

Namespace Controls

    Public Class CharacterTrainingBlock

        ReadOnly _displayPilot As Core.EveHQPilot
        ReadOnly _displayPilotName As String = ""
        ReadOnly _usingAccount As String = ""

        Public Sub New(ByVal objectName As String, ByVal isAccount As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.

            If IsAccount = True Then
                Dim cAccount As Core.EveHQAccount = Core.HQ.Settings.Accounts(objectName)
                _usingAccount = cAccount.userID
                If cAccount.APIAccountStatus = Core.APIAccountStatuses.Disabled Then
                    ' Prepare block for a blank account
                    pbPilot.SizeMode = PictureBoxSizeMode.StretchImage
                    pbPilot.Image = My.Resources.Warning64
                    lblSkill.Text = "Account: " & cAccount.FriendlyName
                    lblTime.Text = "ACCOUNT HAS EXPIRED!"
                    ToolTip1.SetToolTip(lblTime, "Account '" & cAccount.FriendlyName & "' has expired!")
                    lblQueue.Text = "Click here to buy Eve GTCs!"
                    ToolTip1.SetToolTip(lblQueue, "Purchase your GTCs from EveTimeCode.com and help support EveHQ!")
                    lblSkill.ForeColor = Color.Red
                    lblTime.LinkColor = Color.Red
                    lblQueue.LinkColor = Color.Red
                    lblSkill.Name = ""
                    lblTime.Name = ""
                    lblQueue.Name = ""
                Else
                    ' Prepare block for a blank account
                    pbPilot.SizeMode = PictureBoxSizeMode.StretchImage
                    pbPilot.Image = My.Resources.Warning64
                    lblSkill.Text = "Account: " & cAccount.FriendlyName
                    lblTime.Text = "NOT CURRENTLY TRAINING!"
                    ToolTip1.SetToolTip(lblTime, "Account '" & cAccount.FriendlyName & "' is not training!")
                    lblQueue.Text = ""
                    ToolTip1.SetToolTip(lblQueue, "")
                    lblSkill.ForeColor = Color.Red
                    lblTime.LinkColor = Color.Red
                    lblQueue.LinkColor = Color.Red
                    lblSkill.Name = ""
                    lblTime.Name = ""
                    lblQueue.Name = ""
                End If
            Else
                ' Prepare block for a training character
                _displayPilotName = objectName
                _usingAccount = ""
                _displayPilot = Core.HQ.Settings.Pilots(_displayPilotName)

                ' Update skill info before displaying
                _displayPilot.TrainingCurrentSP = CInt(Core.SkillFunctions.CalcCurrentSkillPoints(_displayPilot))
                _displayPilot.TrainingCurrentTime = Core.SkillFunctions.CalcCurrentSkillTime(_displayPilot)

                ' Draw image
                pbPilot.SizeMode = PictureBoxSizeMode.StretchImage
                pbPilot.InitialImage = Core.ImageHandler.GetPortraitImage(_displayPilot.ID)
                pbPilot.Image = Core.ImageHandler.GetPortraitImage(_displayPilot.ID)

                ' Create pilot image tooltip
                Dim stti As New DevComponents.DotNetBar.SuperTooltipInfo
                stti.BodyImage = New Bitmap(Core.ImageHandler.GetPortraitImage(_displayPilot.ID), 48, 48)
                stti.BodyText = "Click the pilot image to view the pilot information for " & _displayPilot.Name
                stti.FooterText = "View Pilot Information"
                stti.FooterImage = My.Resources.Aura32
                stti.Color = DevComponents.DotNetBar.eTooltipColor.Yellow
                STT.SetSuperTooltip(pbPilot, stti)

                ' Check for overlay
                Call OverlayAccountTime()

                ' Establish which skill is training
                Dim currentTrainingSkill As New Core.EveHQPilotQueuedSkill
                Dim lastQueueTime As DateTime = Now
                For Each queuedSkill As Core.EveHQPilotQueuedSkill In _displayPilot.QueuedSkills.Values
                    If Core.SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) >= Now And Core.SkillFunctions.ConvertEveTimeToLocal(queuedSkill.StartTime) <= Now Then
                        currentTrainingSkill = queuedSkill
                    End If
                    lastQueueTime = queuedSkill.EndTime
                Next

                ' Add labels
                Dim baseTime As DateTime = Core.SkillFunctions.ConvertLocalTimeToEve(New Date(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second))
                Dim queueTimeRemaining As Long
                If currentTrainingSkill.SkillID <> 0 Then
                    lblSkill.Text = _displayPilot.Name & " - " & Core.SkillFunctions.SkillIDToName(currentTrainingSkill.SkillID)
                    lblTime.Text = "Training Lvl " & Core.SkillFunctions.Roman(currentTrainingSkill.Level) & ": " & Core.SkillFunctions.TimeToString((currentTrainingSkill.EndTime - baseTime).TotalSeconds)
                    queueTimeRemaining = CLng((lastQueueTime - baseTime).TotalSeconds)
                Else
                    lblSkill.Text = _displayPilot.Name & " - " & _displayPilot.TrainingSkillName
                    lblTime.Text = "Training Lvl " & Core.SkillFunctions.Roman(_displayPilot.TrainingSkillLevel) & ": " & Core.SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                    queueTimeRemaining = _displayPilot.TrainingCurrentTime + _displayPilot.QueuedSkillTime
                End If
                lblQueue.Text = "Queue Time: " & Core.SkillFunctions.TimeToString(queueTimeRemaining)
                If queueTimeRemaining < 86400 Then
                    lblQueue.LinkColor = Color.Red
                Else
                    lblQueue.LinkColor = Color.Black
                End If

                ' Set label tooltips
                Dim sttt As New DevComponents.DotNetBar.SuperTooltipInfo
                sttt.BodyImage = New Bitmap(Core.ImageHandler.GetPortraitImage(_displayPilot.ID), 48, 48)
                sttt.BodyText = "Click the hyperlink to view the skill training information for " & _displayPilot.Name
                sttt.FooterText = "View Skill Training Information"
                sttt.FooterImage = My.Resources.SkillBook32
                sttt.Color = DevComponents.DotNetBar.eTooltipColor.Yellow
                STT.SetSuperTooltip(lblSkill, sttt)
                STT.SetSuperTooltip(lblTime, sttt)
                STT.SetSuperTooltip(lblQueue, sttt)

                ' Set names
                pbPilot.Name = _displayPilotName
                lblSkill.Name = _displayPilotName
                lblTime.Name = _displayPilotName
                lblQueue.Name = _displayPilotName

                ' Start the timers
                tmrUpdate.Enabled = True
                tmrUpdate.Start()
                tmrUpdateOverlays.Enabled = True
                tmrUpdateOverlays.Start()

            End If
        End Sub

        Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As EventArgs) Handles tmrUpdate.Tick
            If Core.HQ.Settings.Pilots.ContainsKey(_displayPilotName) = True Then

                ' Establish which skill is training
                Dim currentTrainingSkill As New Core.EveHQPilotQueuedSkill
                Dim lastQueueTime As DateTime = Now
                For Each queuedSkill As Core.EveHQPilotQueuedSkill In _displayPilot.QueuedSkills.Values
                    If Core.SkillFunctions.ConvertEveTimeToLocal(queuedSkill.EndTime) >= Now And Core.SkillFunctions.ConvertEveTimeToLocal(queuedSkill.StartTime) <= Now Then
                        currentTrainingSkill = queuedSkill
                    End If
                    lastQueueTime = queuedSkill.EndTime
                Next

                ' Add labels
                Dim baseTime As DateTime = Core.SkillFunctions.ConvertLocalTimeToEve(New Date(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second))
                Dim queueTimeRemaining As Long
                If currentTrainingSkill.SkillID <> 0 Then
                    lblSkill.Text = _displayPilot.Name & " - " & Core.SkillFunctions.SkillIDToName(currentTrainingSkill.SkillID)
                    lblTime.Text = "Training Lvl " & Core.SkillFunctions.Roman(currentTrainingSkill.Level) & ": " & Core.SkillFunctions.TimeToString((currentTrainingSkill.EndTime - baseTime).TotalSeconds)
                    queueTimeRemaining = CLng((lastQueueTime - baseTime).TotalSeconds)
                Else
                    lblSkill.Text = _displayPilot.Name & " - " & _displayPilot.TrainingSkillName
                    lblTime.Text = "Training Lvl " & Core.SkillFunctions.Roman(_displayPilot.TrainingSkillLevel) & ": " & Core.SkillFunctions.TimeToString(_displayPilot.TrainingCurrentTime)
                    queueTimeRemaining = _displayPilot.TrainingCurrentTime + _displayPilot.QueuedSkillTime
                End If

                lblQueue.Text = "Queue Time: " & Core.SkillFunctions.TimeToString(queueTimeRemaining)
                If queueTimeRemaining < 86400 Then
                    lblQueue.LinkColor = Color.Red
                Else
                    lblQueue.LinkColor = Color.Black
                End If
            End If
        End Sub

        Private Sub lblQueue_LinkClicked(ByVal sender As System.Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblQueue.LinkClicked
            If _usingAccount <> "" Then
                ' Start a link to the eve time code site via the sharesale gateway url
                Try
                    Dim sInfo As ProcessStartInfo = New ProcessStartInfo("http://www.shareasale.com/r.cfm?B=284450&U=879816&M=31243&urllink=")
                    Process.Start(sInfo)
                Catch ex As Exception
                    MessageBox.Show("Unable to browse to the selected website", "Account Expiry Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub ApplyOverlays()
            Call OverlayAccountTime()
            Call OverlayInsuffClone()
        End Sub

        Private Sub OverlayAccountTime()
            If pbPilot.InitialImage IsNot Nothing Then
                If Core.HQ.Settings.NotifyAccountTime = True Then
                    If Core.HQ.Settings.Pilots.ContainsKey(_displayPilotName) Then
                        If Core.HQ.Settings.Accounts.ContainsKey(_displayPilot.Account) Then
                            Dim accountTime As Date = Core.HQ.Settings.Accounts(_displayPilot.Account).PaidUntil
                            If accountTime.Year > 2000 And (accountTime - Now).TotalHours <= Core.HQ.Settings.AccountTimeLimit Then
                                ' Check exactly how much time is left (i.e. less than an hour?)
                                Dim overlayText As String
                                Dim timeRemaining As Double = (accountTime - Now).TotalHours
                                If timeRemaining > 2400 Then
                                    timeRemaining = Int(timeRemaining / 24) * 24
                                End If
                                Select Case timeRemaining
                                    Case Is <= 0
                                        overlayText = "Expired"
                                    Case Is >= 1
                                        overlayText = Core.SkillFunctions.TimeToString(Int(timeRemaining) * 3600, False)
                                    Case Else
                                        overlayText = " < 1h"
                                End Select
                                Dim overlayFont As Font = New Font(Font.FontFamily, 7)
                                Dim overlayBrush As New SolidBrush(Color.FromArgb(192, 255, 0, 0))
                                ' Define a new image
                                Dim olImage As Bitmap = New Bitmap(pbPilot.InitialImage, pbPilot.Width, pbPilot.Height)
                                Dim myGraphics As Graphics = Graphics.FromImage(olImage)
                                ' Draw a rectangle for the text background
                                myGraphics.FillRectangle(overlayBrush, 0, pbPilot.Height - 10, pbPilot.Width, 10)
                                ' Add the text to the new bitmap.
                                myGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                                Dim ts As Size = Size.Round(myGraphics.MeasureString(overlayText, overlayFont, 40))
                                myGraphics.DrawString(overlayText, overlayFont, New SolidBrush(Color.White), CInt((40 - ts.Width) / 2), 30)
                                pbPilot.Image = olImage
                            Else
                                pbPilot.Image = pbPilot.InitialImage
                            End If
                        End If
                    End If
                Else
                    pbPilot.Image = pbPilot.InitialImage
                End If
            End If
        End Sub

        Private Sub OverlayInsuffClone()
            If pbPilot.Image IsNot Nothing Then
                If Core.HQ.Settings.NotifyInsuffClone = True Then
                    If Core.HQ.Settings.Pilots.ContainsKey(_displayPilotName) Then
                        If (_displayPilot.SkillPoints + Core.SkillFunctions.CalcCurrentSkillPoints(_displayPilot)) > CLng(_displayPilot.CloneSP) Then
                            Const OverlayText As String = "Clone"
                            Dim overlayFont As Font = New Font(Font.FontFamily, 7)
                            Dim overlayBrush As New SolidBrush(Color.Coral)
                            ' Define a new image
                            Dim olImage As Bitmap = New Bitmap(pbPilot.Image, pbPilot.Width, pbPilot.Height)
                            Dim myGraphics As Graphics = Graphics.FromImage(olImage)
                            ' Draw a rectangle for the text background
                            myGraphics.FillRectangle(overlayBrush, 0, pbPilot.Height - 20, pbPilot.Width, 10)
                            ' Add the text to the new bitmap.
                            myGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                            Dim ts As Size = Size.Round(myGraphics.MeasureString(OverlayText, overlayFont, 40))
                            myGraphics.DrawString(OverlayText, overlayFont, New SolidBrush(Color.Black), CInt((40 - ts.Width) / 2), pbPilot.Height - 20)
                            pbPilot.Image = olImage
                        End If
                    End If
                End If
            End If
        End Sub

#Region "Portrait Related Routines"

        Private Sub mnuCtxPicGetPortraitFromServer_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mnuCtxPicGetPortraitFromServer.Click
            Dim dPilot As Core.EveHQPilot = Core.HQ.Settings.Pilots(_displayPilotName)
            pbPilot.ImageLocation = "http://image.eveonline.com/Character/" & dPilot.ID & "_256.jpg"
        End Sub
        Private Sub mnuCtxPicGetPortraitFromLocal_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mnuCtxPicGetPortraitFromLocal.Click
            ' If double-clicked, see if we can get it from the eve portrait folder
            For folder As Integer = 1 To 4
                Dim folderName As String
                If Core.HQ.Settings.EveFolderLua(folder) = False Then
                    Dim eveSettingsFolder As String = Core.HQ.Settings.EveFolder(folder)
                    If eveSettingsFolder IsNot Nothing Then
                        eveSettingsFolder = eveSettingsFolder.Replace("\", "_").Replace(":", "").Replace(" ", "_").ToLower & "_tranquility"
                        Dim eveFolder As String = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "EVE")
                        folderName = Path.Combine(Path.Combine(Path.Combine(Path.Combine(eveFolder, eveSettingsFolder), "cache"), "Pictures"), "Portraits")
                    Else
                        folderName = ""
                    End If
                Else
                    folderName = Path.Combine(Path.Combine(Path.Combine(Core.HQ.Settings.EveFolder(folder), "cache"), "Pictures"), "Portraits")
                End If
                If My.Computer.FileSystem.DirectoryExists(folderName) = True Then
                    For Each foundFile As String In My.Computer.FileSystem.GetFiles(folderName, FileIO.SearchOption.SearchTopLevelOnly, "*.png")
                        If foundFile.Contains(_displayPilot.ID & "_") = True Then
                            ' Get the dimensions of the file
                            Dim myFile As New FileInfo(foundFile)
                            Dim fileData As String() = myFile.Name.Split(New Char() {CChar("_"), CChar(".")})
                            If CInt(fileData(1)) >= 128 And CInt(fileData(1)) <= 256 Then
                                pbPilot.Image = Core.ImageHandler.GetPortraitImage(_displayPilot.ID)
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            Next
            MessageBox.Show("The requested portrait was not found within the Eve cache locations.", "Portrait Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Private Sub mnuSavePortrait_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles mnuSavePortrait.Click
            Dim imgFilename As String = _displayPilot.ID & ".png"
            imgFilename = Path.Combine(Core.HQ.imageCacheFolder, imgFilename)
            ' Save the file
            Try
                pbPilot.InitialImage.Save(imgFilename)
            Catch ex As Exception
            End Try
        End Sub

#End Region

        Private Sub pbPilot_LoadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles pbPilot.LoadCompleted
            pbPilot.InitialImage = pbPilot.Image
            Call ApplyOverlays()
        End Sub

        Private Sub tmrUpdateOverlays_Tick(sender As System.Object, e As EventArgs) Handles tmrUpdateOverlays.Tick
            If Core.HQ.Settings.Pilots.ContainsKey(_displayPilotName) = True Then
                Call ApplyOverlays()
            End If
        End Sub
    End Class
End Namespace