' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Drawing

' Based on the Blitterchip class by Adam "wossname" Ward

Public NotInheritable Class ScreenGrab

    'Define API Functions
    Private Declare Auto Function BitBlt Lib "gdi32" Alias "BitBlt" (ByVal hDestDC As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal xSrc As Integer, ByVal ySrc As Integer, ByVal dwRop As Integer) As Integer
    Declare Function GetDC Lib "user32" Alias "GetDC" (ByVal hwnd As Integer) As Integer

    Public Shared Function GrabScreen(ByVal GrabRect As Rectangle) As Bitmap
        'Returns a screenshot of a defined area of the desktop

        Dim destBMP As Bitmap = New Bitmap(GrabRect.Width, GrabRect.Height)
        Dim gDest As Graphics = Graphics.FromImage(destBMP)
        Dim DestHDC As IntPtr = gDest.GetHdc()

        BitBlt(DestHDC.ToInt32, 0, 0, GrabRect.Width, GrabRect.Height, GetDC(0), GrabRect.Left, GrabRect.Top, &HCC0020)

        gDest.ReleaseHdc(DestHDC)
        gDest.Dispose()

        Return destBMP

    End Function
End Class
