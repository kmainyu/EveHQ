Public Class frmToolTrayIconPopup
    Private Declare Function SHAppBarMessage Lib "shell32.dll" Alias "SHAppBarMessage" (ByVal dwMessage As Int32, ByRef pData As APPBARDATA) As Int32

    Friend Structure RECT
        Friend Left As Int32
        Friend Top As Int32
        Friend Right As Int32
        Friend Bottom As Int32
        Friend Sub New(ByVal Left%, ByVal Top%, ByVal Right%, ByVal Bottom%)
            Me.Left = Left : Me.Bottom = Bottom : Me.Right = Right : Me.Top = Top
        End Sub
        Public Shared Widening Operator CType(ByVal a As Rectangle) As RECT
            Return New RECT(a.Left, a.Top, a.Right, a.Bottom)
        End Operator
        Public Shared Widening Operator CType(ByVal a As RECT) As Rectangle
            Return New Rectangle(a.Left, a.Top, a.Right - a.Left, a.Bottom - a.Top)
        End Operator
    End Structure
    Private Const ABM_GETTASKBARPOS As Int32 = &H5
    Private Const ABM_GETSTATE As Int32 = &H4
    Private Const ABS_AUTOHIDE As Int32 = &H1
    Private Const ABS_ALWAYSONTOP As Int32 = &H2
    Private Structure APPBARDATA
        Dim cbSize As Int32
        Dim hwnd As IntPtr
        Dim uCallbackMessage As [Delegate]
        Dim uEdge As Int32
        Dim rc As RECT
        Dim lParam As Int32
    End Structure
    Dim currentLabel As New Label
    Dim currentPilot As EveHQ.Core.Pilot
    Dim currentDate As Date

    Private Sub frmToolTrayIconPopup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim workingRectangle As System.Drawing.Rectangle = Screen.PrimaryScreen.WorkingArea
        Dim TaskBarLocation As String = Me.GetTaskbarLocation
        Select Case TaskBarLocation
            Case "Bottom"
                Me.Location = New System.Drawing.Point(workingRectangle.Width - Me.Width - 5, workingRectangle.Height - Me.Height - 5)
            Case "Top"
                Me.Location = New System.Drawing.Point(workingRectangle.Width - Me.Width - 5, workingRectangle.Y + 5)
            Case "Left"
                Me.Location = New System.Drawing.Point(workingRectangle.X + 5, workingRectangle.Height - Me.Height - 5)
            Case "Right"
                Me.Location = New System.Drawing.Point(workingRectangle.Width - Me.Width - 5, workingRectangle.Height - Me.Height - 5)
            Case "Error"
                Me.Close()
        End Select
        tmrSkill.Start()
    End Sub

#Region "Taskbar Locating Routines"
    Private Function GetTaskbarLocation() As String
        Dim tbLeft As Int32
        Dim tbTop As Int32
        Dim tbRight As Int32
        Dim tbBottom As Int32
        Dim location As String = ""

        ' Get task bar position and state
        Dim Result As String = GetTaskbarState(Me.Handle, tbLeft, tbTop, tbRight, tbBottom)

        ' Get screen dimensions
        Dim sX, sY, sW, sH As Int32
        sX = 0
        sY = 0
        sW = Screen.PrimaryScreen.Bounds.Width
        sH = Screen.PrimaryScreen.Bounds.Height
        ' Check for error
        If sW = 0 And sH = 0 Then
            location = "Error"
            Return location
        End If
        ' Work out position
        If tbBottom = sH Then
            If tbTop <> sY Then
                location = "Bottom"
            Else
                If tbRight = sW Then
                    location = "Right"
                Else
                    location = "Left"
                End If
            End If
        Else
            location = "Top"
        End If
        Return location
    End Function
    Private Function GetTaskbarState(ByVal ParentHandle As IntPtr, ByRef tbLeft As Int32, ByRef tbTop As Int32, ByRef tbRight As Int32, ByRef tbBottom As Int32) As String
        Dim Result As Int32
        Dim abd As New APPBARDATA
        Dim state As String = ""
        Try
            Call SHAppBarMessage(ABM_GETTASKBARPOS, abd)
            Result = SHAppBarMessage(ABM_GETSTATE, abd)
            If CBool((Result And ABS_AUTOHIDE)) Then
                state = "Autohide is Enabled"
            ElseIf CBool((Result And ABS_ALWAYSONTOP)) Then
                state = "Always on Top"
            End If
            With abd.rc
                tbLeft = .Left
                tbTop = .Top
                tbRight = .Right
                tbBottom = .Bottom
            End With
            Return state
        Catch e As Exception
            tbLeft = 0
            tbTop = 0
            tbRight = 0
            tbBottom = 0
            Return state
        End Try
    End Function
#End Region

    Public Sub ConfigureForm()
        Dim charCount As Integer = 0
        Dim textY As Integer = 20
        AGP1.Controls.Clear()
        For Each dPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
            If dPilot.Training = True Then
                ' Add image
                Dim pb As New PictureBox
                pb.Width = 64 : pb.Height = 64
                pb.Location = New Point(20, 20 + (65 * charCount))
                pb.SizeMode = PictureBoxSizeMode.StretchImage
                Dim imgFilename As String = EveHQ.Core.HQ.cacheFolder & "\i" & dPilot.ID & ".png"
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
                    Dim msg As String = EveHQ.Core.HQ.myTQServer.ServerName & ":" & vbCrLf
                    msg = msg & "Version: " & EveHQ.Core.HQ.myTQServer.Version & vbCrLf
                    msg = msg & "Players: " & EveHQ.Core.HQ.myTQServer.Players
                    If msg.Length > 50 Then
                        TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": Server currently initialising"
                    Else
                        TQLabel.Text = EveHQ.Core.HQ.myTQServer.ServerName & ": " & EveHQ.Core.HQ.myTQServer.Version & ", " & EveHQ.Core.HQ.myTQServer.Players & " online"
                    End If
            End Select
            AGP1.Controls.Add(TQLabel)
            textY += TQLabel.Height - 1
            Dim SisiLabel As New Label
            SisiLabel.AutoSize = True
            SisiLabel.Location = New Point(20, textY)
            SisiLabel.Font = New Font("tahoma", 8, FontStyle.Regular)
            Select Case EveHQ.Core.HQ.mySiSiServer.Status
                Case EveHQ.Core.EveServer.ServerStatus.Down
                    SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": Unable to connect to server"
                Case EveHQ.Core.EveServer.ServerStatus.Starting
                    SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.Shutting
                    SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.Full
                    SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.ProxyDown
                    SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.StatusText
                Case EveHQ.Core.EveServer.ServerStatus.Unknown
                    SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": Status unknown"
                Case EveHQ.Core.EveServer.ServerStatus.Up
                    Dim msg As String = EveHQ.Core.HQ.mySiSiServer.ServerName & ":" & vbCrLf
                    msg = msg & "Version: " & EveHQ.Core.HQ.mySiSiServer.Version & vbCrLf
                    msg = msg & "Players: " & EveHQ.Core.HQ.mySiSiServer.Players
                    If msg.Length > 50 Then
                        SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": Server currently initialising"
                    Else
                        SisiLabel.Text = EveHQ.Core.HQ.mySiSiServer.ServerName & ": " & EveHQ.Core.HQ.mySiSiServer.Version & ", " & EveHQ.Core.HQ.mySiSiServer.Players & " online"
                    End If
            End Select
            AGP1.Controls.Add(SisiLabel)
        End If
    End Sub
   
    Private Sub AGP1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AGP1.Click
        Me.Close()
    End Sub

    Public Sub UpdateSkillTimes()
        For Each currentPilot In EveHQ.Core.HQ.Pilots
            If currentPilot.Training = True Then
                currentLabel = CType(AGP1.Controls(currentPilot.Name), Label)
                currentDate = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(currentPilot.TrainingEndTime)
                currentLabel.Text = Format(currentDate, "ddd") & " " & currentDate & " (" & EveHQ.Core.SkillFunctions.TimeToString(currentPilot.TrainingCurrentTime) & ")"
            End If
        Next
    End Sub

    Private Sub tmrSkill_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSkill.Tick
        Me.Close()
    End Sub
End Class