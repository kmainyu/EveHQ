'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Threading
Imports System.Drawing
Imports GammaJul.LgLcd
Imports System.Drawing.Text
Imports Timer = System.Windows.Forms.Timer

Public Class G15Lcd

    ' Fields
    Private Shared monoArrived As Boolean
    Private Shared mustExit As Boolean
    Private Shared qvgaArrived As Boolean
    Private Shared ReadOnly WaitAre As New AutoResetEvent(False)
    Public Shared WithEvents TmrLcdChar As New Timer
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

    Public Shared Function InitLcd() As Boolean

        Try
            Dim applet As New LcdApplet("EveHQ LCD Display", LcdAppletCapabilities.Both)
            AddHandler applet.Configure, New EventHandler(AddressOf Applet_Configure)
            AddHandler applet.DeviceArrival, New EventHandler(Of LcdDeviceTypeEventArgs)(AddressOf Applet_DeviceArrival)
            AddHandler applet.DeviceRemoval, New EventHandler(Of LcdDeviceTypeEventArgs)(AddressOf Applet_DeviceRemoval)
            AddHandler applet.IsEnabledChanged, New EventHandler(AddressOf Applet_IsEnabledChanged)

            applet.Connect()

            HQ.IsG15LcdActive = True
            mustExit = False

            ThreadPool.QueueUserWorkItem(AddressOf MainLcdProcess, applet)

        Catch e As Exception
            HQ.IsG15LcdActive = False
        End Try
    End Function

    Private Shared Sub MainLcdProcess(state As Object)

        Dim applet As LcdApplet = CType(state, LcdApplet)
        WaitAre.WaitOne()
        Dim monoDevice As LcdDeviceMonochrome = Nothing

        Do
            If monoArrived Then
                If (monoDevice Is Nothing) Then
                    monoDevice = DirectCast(applet.OpenDeviceByType(LcdDeviceType.Monochrome), LcdDeviceMonochrome)
                    AddHandler monoDevice.SoftButtonsChanged, New EventHandler(Of LcdSoftButtonsEventArgs)(AddressOf MonoDevice_SoftButtonsChanged)
                    CreateMonochromeGdiPages(monoDevice)
                Else
                    monoDevice.ReOpen()
                End If
                monoArrived = False
            End If
            If qvgaArrived Then
                qvgaArrived = False
            End If
            If Not ((Not applet.IsEnabled OrElse (monoDevice Is Nothing)) OrElse monoDevice.IsDisposed) Then
                monoDevice.DoUpdateAndDraw()
            End If
            Thread.Sleep(50)
        Loop While Not mustExit

        monoDevice.Dispose()
        applet.Dispose()

    End Sub

    Public Shared Sub CloseLcd()
        mustExit = True
    End Sub

    Private Shared Sub Applet_Configure(ByVal sender As Object, ByVal e As EventArgs)
        'Console.WriteLine("Configure button clicked!")
    End Sub

    Private Shared Sub Applet_DeviceArrival(ByVal sender As Object, ByVal e As LcdDeviceTypeEventArgs)
        'Console.WriteLine(("A device of type " & e.DeviceType & " was added."))
        If (e.DeviceType = LcdDeviceType.Monochrome) Then
            monoArrived = True
        End If
        WaitAre.Set()
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
        Console.WriteLine(e.SoftButtons)
        If ((e.SoftButtons And LcdSoftButtons.Button0) = LcdSoftButtons.Button0) Then
            ' Select the next char
            Call SelectNextChar()
        End If
        If ((e.SoftButtons And LcdSoftButtons.Button1) = LcdSoftButtons.Button1) Then
            ' Toggle cycle pilots
            If HQ.Settings.CycleG15Pilots = False Then
                HQ.Settings.CycleG15Pilots = True
                TmrLcdChar.Interval = (1000 * HQ.Settings.CycleG15Time)
                TmrLcdChar.Enabled = True
                TmrLcdChar.Start()
            Else
                HQ.Settings.CycleG15Pilots = False
                TmrLcdChar.Stop()
            End If
        End If
        If ((e.SoftButtons And LcdSoftButtons.Button2) = LcdSoftButtons.Button2) Then
            ' Change character mode
            HQ.lcdCharMode += 1
            If HQ.lcdCharMode > 1 Then HQ.lcdCharMode = 0
        End If
        If ((e.SoftButtons And LcdSoftButtons.Button3) = LcdSoftButtons.Button3) Then
            ' Update the API
            StartAPIUpdate = True
        End If
    End Sub

    Private Shared Sub Page_Updating(ByVal sender As Object, ByVal e As UpdateEventArgs)
        Dim page As LcdGdiPage = DirectCast(sender, LcdGdiPage)
        If SplashFlag = True Then
            page.Children(0) = New LcdGdiImage(IntroScreenImage)
        Else
            Select Case HQ.lcdCharMode
                Case 0
                    page.Children(0) = New LcdGdiImage(DrawSkillTrainingInfo(HQ.lcdPilot))
                Case 1
                    page.Children(0) = New LcdGdiImage(DrawCharacterInfo(HQ.lcdPilot))
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
        Dim img As New Bitmap(160, 43)
        'Declare a Graphics object to use as the 'tool' for drawing on the bitmap
        Dim screen As Graphics = Graphics.FromImage(img)

        'Create and set properties of a StringFormat object used for DrawString
        Dim strformat As New StringFormat
        strformat.Alignment = StringAlignment.Center
        strformat.LineAlignment = StringAlignment.Center

        screen.DrawImage(My.Resources.EveHQ_LCDLogo, 0, 0, 160, 42)

        ' Return the image
        Return img

    End Function

    Public Shared Function DrawSkillTrainingInfo(ByVal lcdPilot As String) As Image
        If HQ.Settings.Pilots.ContainsKey(lcdPilot) = True Then
            Dim cPilot As EveHQPilot = HQ.Settings.Pilots.Item(lcdPilot)
            Dim lcdFont As Font = New Font("Tahoma", 9, FontStyle.Regular, GraphicsUnit.Pixel)
            Dim img As New Bitmap(160, 43)
            'Declare a Graphics object to use as the 'tool' for drawing on the bitmap
            Dim screen As Graphics = Graphics.FromImage(img)
            screen.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit

            'Create and set properties of a StringFormat object used for DrawString
            Dim strformat As New StringFormat
            strformat.Alignment = StringAlignment.Near
            strformat.LineAlignment = StringAlignment.Near
            Dim strLcd As String = ""
            strLcd &= cPilot.Name & ControlChars.CrLf
            strLcd &= cPilot.TrainingSkillName
            strLcd &= " (Lvl " & cPilot.TrainingSkillLevel & ")" & ControlChars.CrLf
            Dim localdate As Date = SkillFunctions.ConvertEveTimeToLocal(cPilot.TrainingEndTime)
            Dim trainingTime As Long = SkillFunctions.CalcCurrentSkillTime(cPilot)
            strLcd &= (Format(localdate, "ddd") & " " & localdate) & ControlChars.CrLf
            strLcd &= SkillFunctions.TimeToString(trainingTime)
            screen.DrawString(strLcd, lcdFont, Brushes.Black, New RectangleF(0, 0, 160, 43), strformat)
            If HQ.Settings.CycleG15Pilots = True Then
                screen.DrawImage(My.Resources.refresh, 144, 27, 16, 16)
            End If
            If HQ.APIUpdateAvailable = True Then
                screen.DrawString("(U)", lcdFont, Brushes.Black, 128, 32, strformat)
            End If

            'Draw to the LCD bitmap
            Return img

        Else

            Return Nothing

        End If
    End Function

    Public Shared Function DrawCharacterInfo(ByVal lcdPilot As String) As Image
        If HQ.Settings.Pilots.ContainsKey(lcdPilot) = True Then
            Dim cPilot As EveHQPilot = HQ.Settings.Pilots.Item(lcdPilot)
            'Dim lcdFont As Font = New Font("Microsoft Sans Serif", 13.5F, FontStyle.Regular, GraphicsUnit.Point)
            Dim lcdFont As Font = New Font("Tahoma", 9, FontStyle.Regular, GraphicsUnit.Pixel)
            Dim img As New Bitmap(160, 43)
            'Declare a Graphics object to use as the 'tool' for drawing on the bitmap
            Dim screen As Graphics = Graphics.FromImage(img)
            screen.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit

            'Create and set properties of a StringFormat object used for DrawString
            Dim strformat As New StringFormat
            strformat.Alignment = StringAlignment.Near
            strformat.LineAlignment = StringAlignment.Near
            Dim strLcd As String = ""
            strLcd &= cPilot.Name & ControlChars.CrLf
            strLcd &= cPilot.Corp & ControlChars.CrLf
            Dim sp As String = (cPilot.SkillPoints + cPilot.TrainingCurrentSP).ToString("N0")
            strLcd &= "SP: " & sp & ControlChars.CrLf
            strLcd &= "ISK: " & cPilot.Isk.ToString("N2")
            screen.DrawString(strLcd, lcdFont, Brushes.Black, New RectangleF(0, 0, 160, 43), strformat)
            If HQ.Settings.CycleG15Pilots = True Then
                screen.DrawImage(My.Resources.refresh, 144, 27, 16, 16)
            End If
            If HQ.APIUpdateAvailable = True Then
                screen.DrawString("(U)", lcdFont, Brushes.Black, 128, 32, strformat)
            End If

            'Draw to the LCD bitmap
            Return img

        Else

            Return Nothing

        End If
    End Function

    Private Shared Sub tmrLCDChar_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrLCDChar.Tick
        If HQ.Settings.CycleG15Pilots = True And HQ.Settings.Pilots.Count > 0 Then
            Call SelectNextChar()
        End If
    End Sub

    Private Shared Sub SelectNextChar()
        ' Check for the next character
        Dim startSearch As Boolean = False
        Dim startSearchIndex As Integer = 0
        Dim searchChar As Integer = -1
        Dim cPilot As EveHQPilot
        Do
            searchChar += 1
            If searchChar = HQ.Settings.Pilots.Count Then searchChar = 0
            cPilot = HQ.Settings.Pilots.Values(searchChar)
            If startSearch = True And cPilot.Training = True Then
                Exit Do
            End If
            ' This test will exit if we come to the same pilot and have not found one that is training
            If searchChar = startSearchIndex Then
                Exit Sub
            End If
            If cPilot.Name = HQ.lcdPilot Then
                startSearch = True
                startSearchIndex = searchChar
            End If
        Loop
        HQ.lcdPilot = cPilot.Name
    End Sub

End Class
