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

Public Class CharacterTrainingBlock

    Dim displayPilotName As String = ""
    Dim UsingAccount As String = ""

    Public Sub New(ByVal objectName As String, ByVal IsAccount As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        If IsAccount = True Then
            Dim cAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(objectName), Core.EveAccount)
            UsingAccount = cAccount.userID
            If cAccount.APIAccountStatus = Core.APIAccountStatuses.Disabled Then
                ' Prepare block for a blank account
                pbPilot.SizeMode = PictureBoxSizeMode.StretchImage
                pbPilot.Image = My.Resources.Warning64
                lblSkill.Text = "Account: " & cAccount.FriendlyName
                lblTime.Text = "ACCOUNT HAS EXPIRED!"
                ToolTip1.SetToolTip(lblTime, "Account '" & cAccount.FriendlyName & "' has expired!")
                lblQueue.Text = "Click here to buy Eve GTCs!"
                ToolTip1.SetToolTip(lblQueue, "Purchase your GTCs from BattleClinic Deep Space Supply and help support EveHQ!")
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
            displayPilotName = objectName
            UsingAccount = ""
            Dim dPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(displayPilotName), Core.Pilot)

            ' Update skill info before displaying
            dPilot.TrainingCurrentSP = CInt(EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(dPilot))
            dPilot.TrainingCurrentTime = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(dPilot)

            ' Draw image
            pbPilot.SizeMode = PictureBoxSizeMode.StretchImage
            pbPilot.InitialImage = EveHQ.Core.ImageHandler.GetPortraitImage(dPilot.ID)
            pbPilot.Image = EveHQ.Core.ImageHandler.GetPortraitImage(dPilot.ID)

            ' Create pilot image tooltip
            Dim STTI As New DevComponents.DotNetBar.SuperTooltipInfo
            STTI.BodyImage = New Bitmap(EveHQ.Core.ImageHandler.GetPortraitImage(dPilot.ID), 48, 48)
            STTI.BodyText = "Click the pilot image to view the pilot information for " & dPilot.Name
            STTI.FooterText = "View Pilot Information"
            STTI.FooterImage = My.Resources.Aura32
            STTI.Color = DevComponents.DotNetBar.eTooltipColor.Yellow
            STT.SetSuperTooltip(pbPilot, STTI)

            ' Check for overlay
            Call Me.OverlayAccountTime()

            ' Add labels
            lblSkill.Text = dPilot.Name & " - " & dPilot.TrainingSkillName
            Dim currentDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(dPilot.TrainingEndTime)
            lblTime.Text = "Training Lvl " & EveHQ.Core.SkillFunctions.Roman(dPilot.TrainingSkillLevel) & ": " & EveHQ.Core.SkillFunctions.TimeToString(dPilot.TrainingCurrentTime)
            lblQueue.Text = "Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(dPilot.TrainingCurrentTime + dPilot.QueuedSkillTime)
            If dPilot.QueuedSkillTime + dPilot.TrainingCurrentTime < 86400 Then
                lblQueue.LinkColor = Color.Red
                'lblQueue.Text &= " (" & EveHQ.Core.SkillFunctions.TimeToString(86400 - dPilot.QueuedSkillTime - dPilot.TrainingCurrentTime) & ")"
            Else
                lblQueue.LinkColor = Color.Black
            End If
            ' Set label tooltips
            Dim STTT As New DevComponents.DotNetBar.SuperTooltipInfo
            STTT.BodyImage = New Bitmap(EveHQ.Core.ImageHandler.GetPortraitImage(dPilot.ID), 48, 48)
            STTT.BodyText = "Click the hyperlink to view the skill training information for " & dPilot.Name
            STTT.FooterText = "View Skill Training Information"
            STTT.FooterImage = My.Resources.SkillBook32
            STTT.Color = DevComponents.DotNetBar.eTooltipColor.Yellow
            STT.SetSuperTooltip(lblSkill, STTT)
            STT.SetSuperTooltip(lblTime, STTT)
            STT.SetSuperTooltip(lblQueue, STTT)
            ' Set names
            pbPilot.Name = displayPilotName
            lblSkill.Name = displayPilotName
            lblTime.Name = displayPilotName
            lblQueue.Name = displayPilotName
            ' Start the timer
            tmrUpdate.Enabled = True
            tmrUpdate.Start()
        End If
    End Sub

    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(displayPilotName) = True Then
            Dim dPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(displayPilotName), Core.Pilot)
            lblSkill.Text = dPilot.Name & " - " & dPilot.TrainingSkillName
            Dim currentDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(dPilot.TrainingEndTime)
            lblTime.Text = "Training Lvl " & EveHQ.Core.SkillFunctions.Roman(dPilot.TrainingSkillLevel) & ": " & EveHQ.Core.SkillFunctions.TimeToString(dPilot.TrainingCurrentTime)
            lblQueue.Text = "Queue Time: " & EveHQ.Core.SkillFunctions.TimeToString(dPilot.TrainingCurrentTime + dPilot.QueuedSkillTime)
            If dPilot.QueuedSkillTime + dPilot.TrainingCurrentTime < 86400 Then
                lblQueue.LinkColor = Color.Red
                'lblQueue.Text &= " (" & EveHQ.Core.SkillFunctions.TimeToString(86400 - dPilot.QueuedSkillTime - dPilot.TrainingCurrentTime) & ")"
            Else
                lblQueue.LinkColor = Color.Black
            End If
            Call Me.OverlayAccountTime()
        End If
    End Sub

    Private Sub lblQueue_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblQueue.LinkClicked
        If UsingAccount <> "" Then
            ' Start a link to the DSS GTC site
            Try
                Process.Start("https://shop.battleclinic.com/product_info.php?ref=33&products_id=47")
            Catch ex As Exception
                MessageBox.Show("Unable to browse to the selected website", "Account Expiry Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub OverlayAccountTime()
        If pbPilot.InitialImage IsNot Nothing Then
			If EveHQ.Core.HQ.EveHQSettings.NotifyAccountTime = True Then
				If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(displayPilotName) Then
					Dim dPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(displayPilotName), Core.Pilot)
					If EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(dPilot.Account) Then
						Dim AccountTime As Date = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(dPilot.Account), EveHQ.Core.EveAccount).PaidUntil
						If AccountTime.Year > 2000 And (AccountTime - Now).TotalHours <= EveHQ.Core.HQ.EveHQSettings.AccountTimeLimit Then
                            ' Check exactly how much time is left (i.e. less than an hour?)
                            Dim OverlayText As String = ""
                            Dim TimeRemaining As Double = (AccountTime - Now).TotalHours
                            If TimeRemaining > 2400 Then
                                TimeRemaining = Int(TimeRemaining / 24) * 24
                            End If
                            Select Case TimeRemaining
                                Case Is <= 0
                                    OverlayText = "Expired"
                                Case Is >= 1
                                    OverlayText = EveHQ.Core.SkillFunctions.TimeToString(Int(TimeRemaining) * 3600, False)
                                Case Else
                                    OverlayText = " < 1h"
                            End Select
                            Dim OverlayFont As Font = New Font(Me.Font.FontFamily, 7)
							Dim OverlayBrush As New SolidBrush(Drawing.Color.FromArgb(192, 255, 0, 0))
							' Define a new image
							Dim OLImage As Bitmap = New Bitmap(pbPilot.InitialImage, pbPilot.Width, pbPilot.Height)
							Dim MyGraphics As Graphics = Graphics.FromImage(OLImage)
							' Draw a rectangle for the text background
							MyGraphics.FillRectangle(OverlayBrush, 0, pbPilot.Height - 10, pbPilot.Width, 10)
							' Add the text to the new bitmap.
							MyGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
							Dim ts As Size = Size.Round(MyGraphics.MeasureString(OverlayText, OverlayFont, 40))
							MyGraphics.DrawString(OverlayText, OverlayFont, New SolidBrush(Color.White), CInt((40 - ts.Width) / 2), 30)
							pbPilot.Image = OLImage
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

#Region "Portrait Related Routines"

    Private Sub mnuCtxPicGetPortraitFromServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCtxPicGetPortraitFromServer.Click
        Dim dPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(displayPilotName), Core.Pilot)
        pbPilot.ImageLocation = "http://image.eveonline.com/Character/" & dPilot.ID & "_256.jpg"
    End Sub
    Private Sub mnuCtxPicGetPortraitFromLocal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCtxPicGetPortraitFromLocal.Click
        ' If double-clicked, see if we can get it from the eve portrait folder
        Dim dPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(displayPilotName), Core.Pilot)
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
                    If foundFile.Contains(dPilot.ID & "_") = True Then
                        ' Get the dimensions of the file
                        Dim myFile As New FileInfo(foundFile)
                        Dim fileData As String() = myFile.Name.Split(New Char(1) {CChar("_"), CChar(".")})
                        If CInt(fileData(1)) >= 128 And CInt(fileData(1)) <= 256 Then
                            pbPilot.Image = EveHQ.Core.ImageHandler.GetPortraitImage(dPilot.ID)
                            Exit Sub
                        End If
                    End If
                Next
            End If
        Next
        MessageBox.Show("The requested portrait was not found within the Eve cache locations.", "Portrait Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Sub mnuSavePortrait_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSavePortrait.Click
        Dim dPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(displayPilotName), Core.Pilot)
        Dim imgFilename As String = dPilot.ID & ".png"
        imgFilename = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, imgFilename)
        ' Save the file
        Try
            pbPilot.InitialImage.Save(imgFilename)
        Catch ex As Exception
        End Try
    End Sub
#End Region

    Private Sub pbPilot_LoadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles pbPilot.LoadCompleted
        pbPilot.InitialImage = pbPilot.Image
        Call OverlayAccountTime()
    End Sub
End Class
