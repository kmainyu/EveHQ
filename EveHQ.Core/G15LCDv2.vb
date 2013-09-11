'' ========================================================================
'' EveHQ - An Eve-Online™ character assistance application
'' Copyright © 2005-2012  EveHQ Development Team
'' 
'' This file is part of EveHQ.
''
'' EveHQ is free software: you can redistribute it and/or modify
'' it under the terms of the GNU General Public License as published by
'' the Free Software Foundation, either version 3 of the License, or
'' (at your option) any later version.
''
'' EveHQ is distributed in the hope that it will be useful,
'' but WITHOUT ANY WARRANTY; without even the implied warranty of
'' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'' GNU General Public License for more details.
''
'' You should have received a copy of the GNU General Public License
'' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
''=========================================================================

Imports GammaJul.LgLcd
Imports System.Threading
Imports System.Drawing
Imports System.IO
Imports System.Reflection

Public Class G15LCDv2

    ' Fields
    Private Shared _monoArrived As Boolean
    Private Shared _mustExit As Boolean
    Private Shared _qvgaArrived As Boolean
    Private Shared ReadOnly _random As Random
    Private Shared ReadOnly _waitAre As New AutoResetEvent(False)
    Public Shared WithEvents tmrLCDChar As New System.Windows.Forms.Timer
    Public Shared SplashFlag As Boolean = True
    Public Shared Event UpdateAPI()

    Shared Property StartAPIUpdate() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateAPI()
            End If
        End Set
    End Property

    Public Shared Function InitLCD() As Boolean

        Try
            Dim applet As New LcdApplet("EveHQ LCD Display", LcdAppletCapabilities.Both)
            AddHandler applet.Configure, New EventHandler(AddressOf Applet_Configure)
            AddHandler applet.DeviceArrival, New EventHandler(Of LcdDeviceTypeEventArgs)(AddressOf Applet_DeviceArrival)
            AddHandler applet.DeviceRemoval, New EventHandler(Of LcdDeviceTypeEventArgs)(AddressOf Applet_DeviceRemoval)
            AddHandler applet.IsEnabledChanged, New EventHandler(AddressOf Applet_IsEnabledChanged)

            applet.Connect()

            EveHQ.Core.HQ.IsG15LCDActive = True
            _mustExit = False

            Threading.ThreadPool.QueueUserWorkItem(AddressOf MainLCDProcess, applet)

        Catch e As Exception
            EveHQ.Core.HQ.IsG15LCDActive = False
        End Try
    End Function

    Private Shared Sub MainLCDProcess(state As Object)

        Dim applet As LcdApplet = CType(state, LcdApplet)
        _waitAre.WaitOne()
        Dim monoDevice As LcdDeviceMonochrome = Nothing

        Do
            If _monoArrived Then
                If (monoDevice Is Nothing) Then
                    monoDevice = DirectCast(applet.OpenDeviceByType(LcdDeviceType.Monochrome), LcdDeviceMonochrome)
                    AddHandler monoDevice.SoftButtonsChanged, New EventHandler(Of LcdSoftButtonsEventArgs)(AddressOf MonoDevice_SoftButtonsChanged)
                    CreateMonochromeGdiPages(monoDevice)
                Else
                    monoDevice.ReOpen()
                End If
                _monoArrived = False
            End If
            If _qvgaArrived Then
                _qvgaArrived = False
            End If
            If Not ((Not applet.IsEnabled OrElse (monoDevice Is Nothing)) OrElse monoDevice.IsDisposed) Then
                monoDevice.DoUpdateAndDraw()
            End If
            Thread.Sleep(50)
        Loop While Not _mustExit

        monoDevice.Dispose()
        applet.Dispose()

    End Sub

    Public Shared Sub CloseLCD()
        _mustExit = True
    End Sub

    Private Shared Sub Applet_Configure(ByVal sender As Object, ByVal e As EventArgs)
        'Console.WriteLine("Configure button clicked!")
    End Sub

    Private Shared Sub Applet_DeviceArrival(ByVal sender As Object, ByVal e As LcdDeviceTypeEventArgs)
        'Console.WriteLine(("A device of type " & e.DeviceType & " was added."))
        If (e.DeviceType = LcdDeviceType.Monochrome) Then
            _monoArrived = True
        End If
        _waitAre.Set()
    End Sub

    Private Shared Sub Applet_DeviceRemoval(ByVal sender As Object, ByVal e As LcdDeviceTypeEventArgs)
        'Console.WriteLine(("A device of type " & e.DeviceType & " was removed."))
    End Sub

    Private Shared Sub Applet_IsEnabledChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Console.WriteLine(IIf(DirectCast(sender, LcdApplet).IsEnabled, "Applet was enabled.", "Applet was disabled"))
    End Sub

    Private Shared Sub CreateMonochromeGdiPages(ByVal monoDevice As LcdDevice)

        ' Creates first page
        Dim page1 As LcdGdiPage = New LcdGdiPage(monoDevice)
        page1.Children.Add(New LcdGdiImage(IntroScreenImage))

        ' Creates second page
        'Dim page2 As LcdGdiPage = New LcdGdiPage(monoDevice)
        'page2.Children.Add(New LcdGdiImage(DrawCharacterInfo(EveHQ.Core.HQ.lcdPilot)))

        'page1.Children.Add(New LcdGdiScrollViewer(New LcdGdiText("Hello there! Please press the fourth soft button to exit the program.")) With {.HorizontalAlignment = LcdGdiHorizontalAlignment.Stretch, .VerticalAlignment = LcdGdiVerticalAlignment.Stretch, .Margin = New MarginF(34.0F, 0.0F, 2.0F, 0.0F), .AutoScrollX = True})
        'page1.Children.Add(New LcdGdiProgressBar With {.HorizontalAlignment = LcdGdiHorizontalAlignment.Stretch, .VerticalAlignment = LcdGdiVerticalAlignment.Stretch, .Margin = New MarginF(34, 14, 2, 0), .Size = New SizeF(0, 7)})
        'page1.Children.Add(New LcdGdiPolygon(Pens.Black, Brushes.White, {New PointF(0.0F, 10.0F), New PointF(5.0F, 0.0F), New PointF(10.0F, 10.0F)}, False))
        AddHandler page1.Updating, New EventHandler(Of UpdateEventArgs)(AddressOf Page_Updating)

        ' Creates second page
        'Dim page2 As LcdGdiPage = New LcdGdiPage(monoDevice)
        'page2.Children.Add(New LcdGdiRectangle With {.Pen = Pens.Black, .HorizontalAlignment = LcdGdiHorizontalAlignment.Stretch, .VerticalAlignment = LcdGdiVerticalAlignment.Stretch})
        'page2.Children.Add(New LcdGdiLine(Pens.Black, New PointF(0.0F, 0.0F), New PointF(159.0F, 42.0F)) With {.HorizontalAlignment = LcdGdiHorizontalAlignment.Stretch, .VerticalAlignment = LcdGdiVerticalAlignment.Stretch})
        'page2.Children.Add(New LcdGdiLine(Pens.Black, New PointF(0.0F, 42.0F), New PointF(159.0F, 0.0F)) With {.HorizontalAlignment = LcdGdiHorizontalAlignment.Stretch, .VerticalAlignment = LcdGdiVerticalAlignment.Stretch})
        'AddHandler page2.GdiDrawing, New EventHandler(Of GdiDrawingEventArgs)(AddressOf Page2_GdiDrawing)

        monoDevice.Pages.Add(page1)
        'monoDevice.Pages.Add(page2)
        monoDevice.CurrentPage = page1
        monoDevice.SetAsForegroundApplet = True

    End Sub

    Private Shared Sub MonoDevice_SoftButtonsChanged(ByVal sender As Object, ByVal e As LcdSoftButtonsEventArgs)
        Dim device As LcdDevice = DirectCast(sender, LcdDevice)
        Console.WriteLine(e.SoftButtons)
        If ((e.SoftButtons And LcdSoftButtons.Button0) = LcdSoftButtons.Button0) Then
            ' Select the next char
            Call G15LCDv2.SelectNextChar()
        End If
        If ((e.SoftButtons And LcdSoftButtons.Button1) = LcdSoftButtons.Button1) Then
            ' Toggle cycle pilots
            If EveHQ.Core.HQ.Settings.CycleG15Pilots = False Then
                EveHQ.Core.HQ.Settings.CycleG15Pilots = True
                G15LCDv2.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.Settings.CycleG15Time)
                G15LCDv2.tmrLCDChar.Enabled = True
                G15LCDv2.tmrLCDChar.Start()
            Else
                EveHQ.Core.HQ.Settings.CycleG15Pilots = False
                G15LCDv2.tmrLCDChar.Stop()
            End If
        End If
        If ((e.SoftButtons And LcdSoftButtons.Button2) = LcdSoftButtons.Button2) Then
            ' Change character mode
            EveHQ.Core.HQ.lcdCharMode += 1
            If EveHQ.Core.HQ.lcdCharMode > 1 Then EveHQ.Core.HQ.lcdCharMode = 0
        End If
        If ((e.SoftButtons And LcdSoftButtons.Button3) = LcdSoftButtons.Button3) Then
            ' Update the API
            G15LCDv2.StartAPIUpdate = True
        End If
    End Sub

    Private Shared Sub Page_Updating(ByVal sender As Object, ByVal e As UpdateEventArgs)
        Dim page As LcdGdiPage = DirectCast(sender, LcdGdiPage)
        If SplashFlag = True Then
            page.Children(0) = New LcdGdiImage(IntroScreenImage)
        Else
            Select Case EveHQ.Core.HQ.lcdCharMode
                Case 0
                    page.Children(0) = New LcdGdiImage(DrawSkillTrainingInfo(EveHQ.Core.HQ.lcdPilot))
                Case 1
                    page.Children(0) = New LcdGdiImage(DrawCharacterInfo(EveHQ.Core.HQ.lcdPilot))
            End Select
        End If

        'page.Children(0) = New LcdGdiImage(IntroScreenImage)
        'Dim progressBar As LcdGdiProgressBar = DirectCast(page.Children.Item(2), LcdGdiProgressBar)
        'progressBar.Value = CInt(((e.ElapsedTotalTime.TotalSeconds Mod 10) * 10))
        'Dim polygon As LcdGdiPolygon = DirectCast(page.Children.Item(3), LcdGdiPolygon)
        'polygon.Brush = CType(IIf((e.ElapsedTotalTime.Milliseconds < 500), Brushes.White, Brushes.Black), Brush)

    End Sub

    'Private Shared Sub Page2_GdiDrawing(ByVal sender As Object, ByVal e As GdiDrawingEventArgs)
    '    Dim i As Integer
    '    For i = 0 To 10 - 1
    '        e.Graphics.DrawLine(Pens.Black, _random.Next(160), _random.Next(&H2B), _random.Next(160), _random.Next(&H2B))
    '    Next i
    '    DirectCast(sender, LcdGdiPage).Invalidate()
    'End Sub

    Public Shared Function IntroScreenImage() As Image
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

        ' Return the image
        Return img

    End Function

    Public Shared Function DrawSkillTrainingInfo(ByVal lcdPilot As String) As Image
        If EveHQ.Core.HQ.Settings.Pilots.ContainsKey(lcdPilot) = True Then
            Dim cPilot As EveHQ.Core.EveHQPilot = EveHQ.Core.HQ.Settings.Pilots.Item(lcdPilot)
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
            Dim localdate As Date = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingEndTime)
            Dim trainingTime As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
            strLCD &= (Format(localdate, "ddd") & " " & localdate) & ControlChars.CrLf
            strLCD &= EveHQ.Core.SkillFunctions.TimeToString(trainingTime)
            screen.DrawString(strLCD, lcdFont, Brushes.Black, New RectangleF(0, 0, 160, 43), strformat)
            If EveHQ.Core.HQ.Settings.CycleG15Pilots = True Then
                screen.DrawImage(My.Resources.refresh, 144, 27, 16, 16)
            End If
            If EveHQ.Core.HQ.APIUpdateAvailable = True Then
                screen.DrawString("(U)", lcdFont, Brushes.Black, 128, 32, strformat)
            End If

            'Draw to the LCD bitmap
            Return img

        Else

            Return Nothing

        End If
    End Function

    Public Shared Function DrawCharacterInfo(ByVal lcdPilot As String) As Image
        If EveHQ.Core.HQ.Settings.Pilots.ContainsKey(lcdPilot) = True Then
            Dim cPilot As EveHQ.Core.EveHQPilot = EveHQ.Core.HQ.Settings.Pilots.Item(lcdPilot)
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
            Dim SP As String = (cPilot.SkillPoints + cPilot.TrainingCurrentSP).ToString("N0")
            strLCD &= "SP: " & SP & ControlChars.CrLf
            strLCD &= "ISK: " & cPilot.Isk.ToString("N2")
            screen.DrawString(strLCD, lcdFont, Brushes.Black, New RectangleF(0, 0, 160, 43), strformat)
            If EveHQ.Core.HQ.Settings.CycleG15Pilots = True Then
                screen.DrawImage(My.Resources.refresh, 144, 27, 16, 16)
            End If
            If EveHQ.Core.HQ.APIUpdateAvailable = True Then
                screen.DrawString("(U)", lcdFont, Brushes.Black, 128, 32, strformat)
            End If

            'Draw to the LCD bitmap
            Return img

        Else

            Return Nothing

        End If
    End Function

    Private Shared Sub tmrLCDChar_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLCDChar.Tick
        If EveHQ.Core.HQ.Settings.CycleG15Pilots = True And EveHQ.Core.HQ.Settings.Pilots.Count > 0 Then
            Call SelectNextChar()
        End If
    End Sub

    Private Shared Sub SelectNextChar()
        ' Check for the next character
        Dim startSearch As Boolean = False
        Dim startSearchIndex As Integer = 0
        Dim searchChar As Integer = -1
        Dim cPilot As New EveHQ.Core.EveHQPilot
        Do
            searchChar += 1
            If searchChar = EveHQ.Core.HQ.Settings.Pilots.Count Then searchChar = 0
            cPilot = EveHQ.Core.HQ.Settings.Pilots.Values(searchChar)
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
    End Sub

End Class
