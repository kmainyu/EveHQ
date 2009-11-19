Imports System.IO
Imports System.Runtime.InteropServices

Public Class frmToolTrayIconPopup
   
    Dim currentLabel As New Label
    Dim currentPilot As EveHQ.Core.Pilot
    Dim currentDate As Date

    Private m_autoRefresh As Boolean
    Private m_tooltip As String

    Protected Overrides Sub OnClosed(ByVal e As EventArgs)
        Me.displayTimer.Stop()
        MyBase.OnClosed(e)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)
        Me.m_autoRefresh = True
        Me.UpdateForm()
    End Sub

    Protected Overrides Sub OnShown(ByVal e As EventArgs)
        MyBase.OnShown(e)
        If Me.m_autoRefresh Then
            Me.displayTimer.Start()
        End If
        NativeMethods.SetWindowPos(MyBase.Handle, -1, 0, 0, 0, 0, &H13)
        NativeMethods.ShowWindow(MyBase.Handle, 4)
    End Sub

    Private Sub displayTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles displayTimer.Tick
        Me.UpdateSkillTimes()
    End Sub

    Private Sub UpdateForm()
        MyBase.SuspendLayout()
        Me.ConfigureForm()
        MyBase.ResumeLayout()
        MyBase.PerformLayout()
        EveHQ.Core.EveHQIcon.SetToolTipLocation(Me)
    End Sub

    Private Sub ConfigureForm()
        Dim charCount As Integer = 0
        Dim textY As Integer = 20
        AGP1.Controls.Clear()
        For Each dPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If dPilot.Training = True Then
                ' Add image
                Dim pb As New PictureBox
                pb.Width = 64 : pb.Height = 64
                pb.Location = New Point(20, 20 + (65 * charCount))
                pb.SizeMode = PictureBoxSizeMode.StretchImage
                Dim imgFilename As String = Path.Combine(EveHQ.Core.HQ.imageCacheFolder, dPilot.ID & ".png")
                pb.ImageLocation = imgFilename
                AGP1.Controls.Add(pb)
                textY = 20 + (65 * charCount)
                ' Add name label
                Dim NameLabel As New Label
                NameLabel.AutoSize = True
                NameLabel.Location = New Point(90, textY)
                NameLabel.Font = New Font("tahoma", 12, FontStyle.Bold)
                NameLabel.Text = dPilot.Name
                AGP1.Controls.Add(NameLabel)
                textY += NameLabel.Height - 1
                ' Add Training Skill
                Dim SkillLabel As New Label
                SkillLabel.AutoSize = True
                SkillLabel.Location = New Point(90, textY)
                SkillLabel.Font = New Font("tahoma", 8, FontStyle.Regular)
                SkillLabel.Text = dPilot.TrainingSkillName & " " & EveHQ.Core.SkillFunctions.Roman(dPilot.TrainingSkillLevel)
                AGP1.Controls.Add(SkillLabel)
                textY += SkillLabel.Height - 1
                ' Add Training Skill End Time
                Dim SkillTimeLabel As New Label
                SkillTimeLabel.Name = dPilot.Name
                SkillTimeLabel.AutoSize = True
                SkillTimeLabel.Location = New Point(90, textY)
                SkillTimeLabel.Font = New Font("tahoma", 8, FontStyle.Regular)
                currentDate = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(dPilot.TrainingEndTime)
                SkillTimeLabel.Text = Format(currentDate, "ddd") & " " & currentDate & " (" & EveHQ.Core.SkillFunctions.TimeToString(dPilot.TrainingCurrentTime) & ")"
                AGP1.Controls.Add(SkillTimeLabel)
                textY += SkillTimeLabel.Height - 1
                ' Add Isk label
                Dim IskLabel As New Label
                IskLabel.AutoSize = True
                IskLabel.Location = New Point(90, textY)
                IskLabel.Font = New Font("tahoma", 8, FontStyle.Regular)
                IskLabel.Text = "Isk: " & FormatNumber(dPilot.Isk, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                AGP1.Controls.Add(IskLabel)
                textY += IskLabel.Height - 1
                charCount += 1
            End If
        Next
        Me.Height = (65 * (charCount + 1))
        textY = 24 + (65 * charCount)
        ' Add Status if we have it enabled
        If EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = True Then
            Dim TQLabel As New Label
            TQLabel.AutoSize = True
            TQLabel.Location = New Point(20, textY)
            TQLabel.Font = New Font("tahoma", 8, FontStyle.Regular)
            Select Case EveHQ.Core.HQ.myTQServer.Status
                Case EveHQ.Core.EveServer.ServerStatus.Down
                    TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Unable to connect to server"
                Case EveHQ.Core.EveServer.ServerStatus.Starting
                    TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.Shutting
                    TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.Full
                    TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.ProxyDown
                    TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.Unknown
                    TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Status unknown"
                Case EveHQ.Core.EveServer.ServerStatus.Up
                    TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Online (" & EveHQ.Core.HQ.myTQServer.Players & " Players)"
            End Select
            AGP1.Controls.Add(TQLabel)
        End If
    End Sub

    Public Sub UpdateSkillTimes()
        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If currentPilot.Training = True Then
                currentLabel = CType(AGP1.Controls(currentPilot.Name), Label)
                currentDate = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(currentPilot.TrainingEndTime)
                currentLabel.Text = Format(currentDate, "ddd") & " " & currentDate & " (" & EveHQ.Core.SkillFunctions.TimeToString(currentPilot.TrainingCurrentTime) & ")"
            End If
        Next
    End Sub
End Class

Friend Class NativeMethods
    ' Methods
    <DllImport("user32.dll")> _
    Public Shared Function SetWindowPos(ByVal hWnd As IntPtr, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInt32) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Public Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal flags As Integer) As Boolean
    End Function

    ' Fields
    Public Const HWND_TOPMOST As Integer = -1
    Public Const SW_SHOWNOACTIVATE As Integer = 4
    Public Const SWP_NOACTIVATE As Integer = &H10
    Public Const SWP_NOMOVE As Integer = 2
    Public Const SWP_NOSIZE As Integer = 1
End Class
