' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports NineBit.LgLcd_Net
Imports System.Drawing
Imports System.Windows.Forms

Public Class G15LCD
    'The LCD context objects
    Private connectContext As LgLcd.lgLcdConnectContext = New LgLcd.lgLcdConnectContext()
    Private openContext As LgLcd.lgLcdOpenContext = New LgLcd.lgLcdOpenContext()

    'The lgLcdBitmap160x43x1 used to represent the screen
    Private LCDBitmap As LgLcd.lgLcdBitmap160x43x1 = New LgLcd.lgLcdBitmap160x43x1

    'The delegates used to recieve the ConfigPress and ButtonPress routines
    Public Delegate Function ConfigPress(ByVal connection As Integer, ByVal pcontext As System.IntPtr) As Integer
    Public Delegate Function Butpress(ByVal device As Integer, ByVal dwButtons As Integer, ByVal pContext As System.IntPtr) As Integer
    Private LCDButtonPress As Butpress
    Private LCDConfigPress As ConfigPress
    Public WithEvents tmrLCDChar As New System.Windows.Forms.Timer
   
    Public Sub New(ByVal ButtonPressSubroutine As Butpress, ByVal ConfigSubroutine As ConfigPress)
        'When an instance of the class is created, the delegates are passed the routines to return to.
        LCDButtonPress = ButtonPressSubroutine
        LCDConfigPress = ConfigSubroutine
        tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
    End Sub

    Public Sub InitLCD(ByVal apptitle As String)
        Dim res As Integer
        ' Initialize the LCD library
        res = LgLcd.lgLcdInit()
        HandleError(res, "lcdInit failed")

        ' Connect to the LCD library
        connectContext.appFriendlyName = apptitle
        connectContext.isAutostartable = True

        ' Use this so that when the application is run in the interface it won't be autostartable.
        ' That way you want get Visual Studio starting each time you start your PC.
#If DEBUG Then
        connectContext.isAutostartable = False
#End If

        'Set up connection and set the paramaters and callbacks
        connectContext.isPersistent = True
        connectContext.onConfigure.configCallback = AddressOf LCDConfigPress.Invoke
        connectContext.onConfigure.configContext = IntPtr.Zero
        connectContext.connection = LgLcd.LGLCD_INVALID_CONNECTION
        res = LgLcd.lgLcdConnect(connectContext)
        HandleError(res, "lgLcdConnect failed")

        ' Open the first LCD found (index 0)
        openContext.connection = connectContext.connection
        openContext.index = 0
        openContext.onSoftbuttonsChanged.softbuttonsChangedCallback = AddressOf LCDButtonPress.Invoke
        openContext.onSoftbuttonsChanged.softbuttonsChangedContext = IntPtr.Zero
        openContext.device = LgLcd.LGLCD_INVALID_DEVICE
        res = LgLcd.lgLcdOpen(openContext)
        LCDBitmap.hdr.Format = LgLcd.LGLCD_BMP_FORMAT_160x43x1
        ReDim LCDBitmap.pixels(160 * 43)
        HandleError(res, "lgLcdOpen")
    End Sub

    Public Sub DrawLCD(ByVal lcdscreen As Bitmap)
        ' Create a new bitmap.
        Dim bmp As New Bitmap(lcdscreen)
        Dim rect As New Rectangle(0, 0, bmp.Width, bmp.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = bmp.LockBits(rect, _
            Drawing.Imaging.ImageLockMode.ReadWrite, Drawing.Imaging.PixelFormat.Format24bppRgb)
        Dim ptr As IntPtr = bmpData.Scan0
        Dim bytes As Integer = bmp.Width * bmp.Height * 3
        Dim rgbValues(bytes - 1) As Byte
        Dim pix(lcdscreen.Width * lcdscreen.Height) As Byte
        Dim i As Integer = 0

        System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)
        For counter As Integer = 0 To rgbValues.Length - 1 Step 3
            LCDBitmap.pixels(i) = rgbValues(counter)
            i = i + 1
        Next
        bmp.UnlockBits(bmpData)
    End Sub

    Public Sub UpdateLCD_Normal()
        LgLcd.lgLcdUpdateBitmap(openContext.device, LCDBitmap, LgLcd.LGLCD_PRIORITY_NORMAL)
    End Sub
    Public Sub UpdateLCD_Alert()
        LgLcd.lgLcdUpdateBitmap(openContext.device, LCDBitmap, LgLcd.LGLCD_PRIORITY_ALERT)
    End Sub

    Public Sub CloseLCD()
        Dim res As Integer
        ' Close the LCD
        res = LgLcd.lgLcdClose(openContext.device)
        HandleError(res, "lgLcdClose")
        res = LgLcd.lgLcdDisconnect(connectContext.connection)
        HandleError(res, "lgLcdDisconnect failed")
        res = LgLcd.lgLcdDeInit()
        HandleError(res, "lcdDeInit failed")
    End Sub

    Private Sub HandleError(ByVal res As Integer, ByVal msg As String)
        If res <> 0 Then
            'Throw New Exception(msg)
            EveHQ.Core.HQ.IsG15LCDActive = False
        Else
            EveHQ.Core.HQ.IsG15LCDActive = True
        End If
    End Sub


    Public Sub DrawIntroScreen()
        ''IMPORTANT NOTE: You must remember to use pens.white and brushes.white when you want black!
        '' B&W are inverted when they are drawn on the LCD!

        'Declare a Bitmap to work with
        Dim img As New System.Drawing.Bitmap(160, 43)
        'Declare a Graphics object to use as the 'tool' for drawing on the bitmap
        Dim screen As Drawing.Graphics = Graphics.FromImage(img)

        'Create and set properties of a StringFormat object used for DrawString
        Dim strformat As New Drawing.StringFormat
        strformat.Alignment = StringAlignment.Center
        strformat.LineAlignment = StringAlignment.Center

        screen.DrawImage(My.Resources.EveHQ_LCDLogo, 0, 0, 160, 42)

        'Draw to the LCD bitmap
        EveHQ.Core.HQ.EveHQLCD.DrawLCD(img)
        'Update the LCD
        EveHQ.Core.HQ.EveHQLCD.UpdateLCD_Alert()
    End Sub

    Public Sub DrawSkillTrainingInfo(ByVal lcdPilot As String)
        If EveHQ.Core.HQ.Pilots.Contains(lcdPilot) = True Then
            Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.Pilots.Item(lcdPilot), Pilot)
            Dim lcdFont As Font = New Font("Tahoma", 9, FontStyle.Regular, GraphicsUnit.Pixel)
            Dim img As New System.Drawing.Bitmap(160, 43)
            'Declare a Graphics object to use as the 'tool' for drawing on the bitmap
            Dim screen As Drawing.Graphics = Graphics.FromImage(img)
            screen.TextRenderingHint = Text.TextRenderingHint.SingleBitPerPixelGridFit

            'Create and set properties of a StringFormat object used for DrawString
            Dim strformat As New Drawing.StringFormat
            strformat.Alignment = StringAlignment.Near
            strformat.LineAlignment = StringAlignment.Near
            Dim strLCD As String = ""
            strLCD &= cPilot.Name & ControlChars.CrLf
            strLCD &= cPilot.TrainingSkillName
            strLCD &= " (Lvl " & cPilot.TrainingSkillLevel & ")" & ControlChars.CrLf
            Dim trainingTime As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
            strLCD &= EveHQ.Core.SkillFunctions.TimeToString(trainingTime) & ControlChars.CrLf & ControlChars.CrLf
            screen.DrawString(strLCD, lcdFont, Brushes.White, New RectangleF(0, 0, 160, 43), strformat)
            If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
                screen.DrawImage(My.Resources.refresh, 144, 27, 16, 16)
            End If
            If EveHQ.Core.HQ.UpdateAvailable = True Then
                screen.DrawString("(U)", lcdFont, Brushes.White, 128, 32, strformat)
            End If
            'Draw to the LCD bitmap
            EveHQ.Core.HQ.EveHQLCD.DrawLCD(img)
            'Update the LCD
            EveHQ.Core.HQ.EveHQLCD.UpdateLCD_Normal()
        End If
    End Sub

    Public Sub DrawCharacterInfo(ByVal lcdPilot As String)
        If EveHQ.Core.HQ.Pilots.Contains(lcdPilot) = True Then
            Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.Pilots.Item(lcdPilot), Pilot)
            'Dim lcdFont As Font = New Font("Microsoft Sans Serif", 13.5F, FontStyle.Regular, GraphicsUnit.Point)
            Dim lcdFont As Font = New Font("Tahoma", 9, FontStyle.Regular, GraphicsUnit.Pixel)
            Dim img As New System.Drawing.Bitmap(160, 43)
            'Declare a Graphics object to use as the 'tool' for drawing on the bitmap
            Dim screen As Drawing.Graphics = Graphics.FromImage(img)
            screen.TextRenderingHint = Text.TextRenderingHint.SingleBitPerPixelGridFit

            'Create and set properties of a StringFormat object used for DrawString
            Dim strformat As New Drawing.StringFormat
            strformat.Alignment = StringAlignment.Near
            strformat.LineAlignment = StringAlignment.Near
            Dim strLCD As String = ""
            strLCD &= cPilot.Name & ControlChars.CrLf
            strLCD &= cPilot.Corp & ControlChars.CrLf
            Dim SP As String = FormatNumber(cPilot.SkillPoints + cPilot.TrainingCurrentSP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            strLCD &= "SP: " & SP & ControlChars.CrLf
            strLCD &= "ISK: " & FormatNumber(cPilot.Isk, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            screen.DrawString(strLCD, lcdFont, Brushes.White, New RectangleF(0, 0, 160, 43), strformat)
            If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
                screen.DrawImage(My.Resources.refresh, 144, 27, 16, 16)
            End If
            If EveHQ.Core.HQ.UpdateAvailable = True Then
                screen.DrawString("(U)", lcdFont, Brushes.White, 128, 32, strformat)
            End If
            'Draw to the LCD bitmap
            EveHQ.Core.HQ.EveHQLCD.DrawLCD(img)
            'Update the LCD
            EveHQ.Core.HQ.EveHQLCD.UpdateLCD_Normal()
        End If
    End Sub

    Private Sub tmrLCDChar_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLCDChar.Tick
        If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
            Call SelectNextChar()
        End If
    End Sub

    Public Sub SelectNextChar()
        ' Check for the next character
        Dim startSearch As Boolean = False
        Dim startSearchIndex As Integer = 0
        Dim searchChar As Integer = 0
        Dim cPilot As New EveHQ.Core.Pilot
        Do
            searchChar += 1
            If searchChar = EveHQ.Core.HQ.Pilots.Count + 1 Then searchChar = 1
            cPilot = CType(EveHQ.Core.HQ.Pilots.Item(searchChar), Pilot)
            If startSearch = True And cPilot.Training = True Then
                Exit Do
            End If
            ' This test will exit if we come to the same pilot and have not found one that is training
            If searchChar = startSearchIndex Then
                Exit Sub
            End If
            If cPilot.Name = EveHQ.Core.HQ.lcdPilot Then
                startSearch = True
                startSearchIndex = searchChar
            End If
        Loop
        EveHQ.Core.HQ.lcdPilot = cPilot.Name
        ' Force an update
        Select Case EveHQ.Core.HQ.lcdCharMode
            Case 0
                Call EveHQ.Core.HQ.EveHQLCD.DrawSkillTrainingInfo(EveHQ.Core.HQ.lcdPilot)
            Case 1
                Call EveHQ.Core.HQ.EveHQLCD.DrawCharacterInfo(EveHQ.Core.HQ.lcdPilot)
        End Select
    End Sub
End Class
