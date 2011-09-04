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

Public Class ImageHandler

    Public Shared BaseIcons As New SortedList(Of String, Bitmap)
    Public Shared MetaIcons As New SortedList(Of String, Bitmap)
    Public Shared ItemIcons24 As New SortedList(Of String, Bitmap)
    Public Shared ItemIcons48 As New SortedList(Of String, Bitmap)

    Public Shared Function IconImage24(ByVal IconName As String, ByVal MetaLevel As Integer) As Image

        If IconName <> "" Then
            If ItemIcons24.ContainsKey(IconName & "_" & MetaLevel.ToString) Then
                Return ImageHandler.ItemIcons24(IconName & "_" & MetaLevel.ToString)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function IconImage48(ByVal IconName As String, ByVal MetaLevel As Integer) As Image

        If IconName <> "" Then
            If ItemIcons48.ContainsKey(IconName & "_" & MetaLevel.ToString) Then
                Return ImageHandler.ItemIcons48(IconName & "_" & MetaLevel.ToString)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function CombineIcons24() As Boolean
        Dim IconSize As Integer = 24
        ItemIcons24.Clear()

        For Each BaseIcon As String In BaseIcons.Keys
            For Each MetaIcon As String In MetaIcons.Keys

                If MetaIcon = "1" Then
                    Dim OI As Bitmap = ImageHandler.BaseIcons(BaseIcon)

                    ' Resize the image
                    Dim II As New Bitmap(OI, IconSize, IconSize)

                    ItemIcons24.Add(BaseIcon & "_" & MetaIcon, II)
                Else
                    Dim OI As Bitmap = ImageHandler.BaseIcons(BaseIcon)
                    Dim IO As Bitmap = ImageHandler.MetaIcons(MetaIcon)

                    ' Resize the image
                    Dim II As New Bitmap(OI, IconSize, IconSize)

                    ' Apply the overlay
                    Dim g As Graphics = Graphics.FromImage(II)
                    g.DrawImage(IO, 0, 0)
                    ItemIcons24.Add(BaseIcon & "_" & MetaIcon, II)

                End If

            Next
        Next

        Return True

    End Function

    Public Shared Function CombineIcons48() As Boolean
        Dim IconSize As Integer = 48
        ItemIcons48.Clear()

        For Each BaseIcon As String In BaseIcons.Keys
            For Each MetaIcon As String In MetaIcons.Keys

                If MetaIcon = "1" Then
                    Dim OI As Bitmap = ImageHandler.BaseIcons(BaseIcon)

                    ' Resize the image
                    Dim II As New Bitmap(OI, IconSize, IconSize)

                    ItemIcons48.Add(BaseIcon & "_" & MetaIcon, II)
                Else
                    Dim OI As Bitmap = ImageHandler.BaseIcons(BaseIcon)
                    Dim IO As Bitmap = ImageHandler.MetaIcons(MetaIcon)

                    ' Resize the image
                    Dim II As New Bitmap(OI, IconSize, IconSize)

                    ' Apply the overlay
                    Dim g As Graphics = Graphics.FromImage(II)
                    g.DrawImage(IO, 0, 0)
                    ItemIcons48.Add(BaseIcon & "_" & MetaIcon, II)
                End If

            Next
        Next

        Return True

    End Function

End Class
