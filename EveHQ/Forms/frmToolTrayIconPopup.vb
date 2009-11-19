Imports System.IO
Imports System.Runtime.InteropServices

Public Class frmToolTrayIconPopup
   
    Dim currentLabel As New Label
    Dim currentPilot As EveHQ.Core.Pilot
    Dim currentDate As Date

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnClosed(ByVal e As EventArgs)
        Me.displayTimer.Stop()
        MyBase.OnClosed(e)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)
        Me.displayTimer.Start()
        Me.UpdateForm()
    End Sub

    Protected Overrides Sub OnShown(ByVal e As EventArgs)
        MyBase.OnShown(e)
        NativeMethods.SetWindowPos(MyBase.Handle, -1, 0, 0, 0, 0, &H13)
        NativeMethods.ShowWindow(MyBase.Handle, 4)
    End Sub

    Private Sub displayTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles displayTimer.Tick
        Me.UpdateSkillTimes()
    End Sub

    Private Sub UpdateForm()
        EveHQ.Core.EveHQIcon.SetToolTipLocation(Me)
    End Sub

    Private Sub ConfigureForm()
        For Each dPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If dPilot.Training = True And dPilot.Active = True Then
                Dim newCharBlock As New CharacterBlock(dPilot.Name)
                newCharBlock.Name = dPilot.Name
                AGP1.Controls.Add(newCharBlock)
            End If
        Next
    End Sub

    Public Sub UpdateSkillTimes()
        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If AGP1.Controls.ContainsKey(currentPilot.Name) Then
                currentLabel = CType(AGP1.Controls(currentPilot.Name), CharacterBlock).lblTime
                currentDate = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(currentPilot.TrainingEndTime)
                currentLabel.Text = Format(currentDate, "ddd") & " " & currentDate & " (" & EveHQ.Core.SkillFunctions.TimeToString(currentPilot.TrainingCurrentTime) & ")"
            End If
        Next
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.SuspendLayout()
        Me.ConfigureForm()
        Me.ResumeLayout()
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
