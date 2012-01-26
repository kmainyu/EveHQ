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
Public Class G15LCDB

    'Public Shared Event UpdateAPI()

    'Shared Property StartAPIUpdate() As Boolean
    '    Get
    '    End Get
    '    Set(ByVal value As Boolean)
    '        If value = True Then
    '            RaiseEvent UpdateAPI()
    '        End If
    '    End Set
    'End Property

    'Public Shared Function ButtonPress(ByVal device As Integer, ByVal dwButtons As Integer, ByVal pContext As System.IntPtr) As Integer
    '    'Button presses are passed to this routine
    '    Select Case dwButtons
    '        Case 1
    '            ' Button 1
    '            Call EveHQ.Core.HQ.EveHQLCD.SelectNextChar()
    '        Case 2
    '            ' Button 2
    '            If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = False Then
    '                EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True
    '            Else
    '                EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = False
    '            End If
    '            Select Case EveHQ.Core.HQ.lcdCharMode
    '                Case 0
    '                    Call EveHQ.Core.HQ.EveHQLCD.DrawSkillTrainingInfo(EveHQ.Core.HQ.lcdPilot)
    '                Case 1
    '                    Call EveHQ.Core.HQ.EveHQLCD.DrawCharacterInfo(EveHQ.Core.HQ.lcdPilot)
    '            End Select
    '        Case 4
    '            ' Button 3
    '            EveHQ.Core.HQ.lcdCharMode += 1
    '            If EveHQ.Core.HQ.lcdCharMode > 1 Then EveHQ.Core.HQ.lcdCharMode = 0
    '            Select Case EveHQ.Core.HQ.lcdCharMode
    '                Case 0
    '                    Call EveHQ.Core.HQ.EveHQLCD.DrawSkillTrainingInfo(EveHQ.Core.HQ.lcdPilot)
    '                Case 1
    '                    Call EveHQ.Core.HQ.EveHQLCD.DrawCharacterInfo(EveHQ.Core.HQ.lcdPilot)
    '            End Select
    '        Case 8
    '            ' Button 4
    '            G15LCDv2.StartAPIUpdate = True
    '    End Select
    'End Function

    Public Shared Function ConfigureOptions(ByVal connection As Integer, ByVal pcontext As System.IntPtr) As Integer
        ' This function is called when the "Config" button is pressed in the Logitech LCD Manager

        ' If you want a form to appear to provide a GUI, then the following line works:
        'ConfigureScreen.ShowDialog() ' Note: Just using .Show will not work [try it and see ;-)]

        'However, the above line blocks the running of the application (No LCD updates)
        'So instead, you can open this dialog in a new thread.
        'To do this use the following code:

        ''Dim a As New Threading.Thread(AddressOf Showdialog)
        ''a.Start()

        ''Then create the following new subroutine
        ''Public Sub Showdialog()
        ''ConfigureScreen.ShowDialog()
        ''lcdmenu.UpdateScreen()
        ''End Sub
    End Function

End Class
