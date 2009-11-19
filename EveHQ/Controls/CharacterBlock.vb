Imports System.IO

Public Class CharacterBlock

    Public Sub New(ByVal pilotName As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        ' Get pilot
        Dim dPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(pilotName), Core.Pilot)

        ' Draw image
        pbPilot.SizeMode = PictureBoxSizeMode.StretchImage
        Dim imgFilename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, dPilot.ID & ".png")
        pbPilot.ImageLocation = imgFilename

        ' Add labels
        lblPilotName.Text = dPilot.Name
        lblSkill.Text = dPilot.TrainingSkillName & " " & EveHQ.Core.SkillFunctions.Roman(dPilot.TrainingSkillLevel)
        Dim currentDate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(dPilot.TrainingEndTime)
        lblTime.Text = Format(currentDate, "ddd") & " " & currentDate & " (" & EveHQ.Core.SkillFunctions.TimeToString(dPilot.TrainingCurrentTime) & ")"
        lblIsk.Text = "Isk: " & FormatNumber(dPilot.Isk, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

    End Sub
End Class
