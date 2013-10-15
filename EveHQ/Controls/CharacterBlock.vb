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

    Public Class CharacterBlock

        Public Sub New(ByVal pilotName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.

            ' Get pilot
            Dim dPilot As Core.EveHQPilot = Core.HQ.Settings.Pilots(pilotName)

            ' Draw image
            pbPilot.SizeMode = PictureBoxSizeMode.StretchImage
            Dim imgFilename As String = Path.Combine(Core.HQ.imageCacheFolder, dPilot.ID & ".png")
            If My.Computer.FileSystem.FileExists(imgFilename) = True Then
                pbPilot.ImageLocation = imgFilename
            Else
                pbPilot.Image = My.Resources.nochar
            End If

            ' Add labels
            lblPilotName.Text = dPilot.Name
            lblSkill.Text = dPilot.TrainingSkillName & " " & Core.SkillFunctions.Roman(dPilot.TrainingSkillLevel)
            Dim currentDate As Date = Core.SkillFunctions.ConvertEveTimeToLocal(dPilot.TrainingEndTime)
            lblTime.Text = Format(currentDate, "ddd") & " " & currentDate & " (" & Core.SkillFunctions.TimeToString(dPilot.TrainingCurrentTime) & ")"
            lblIsk.Text = "Isk: " & dPilot.Isk.ToString("N2")

        End Sub
    End Class
End NameSpace