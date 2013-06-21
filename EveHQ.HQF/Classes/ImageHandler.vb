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
                ItemIcons24.Add(BaseIcon & "_" & MetaIcon, CreateIcon(BaseIcon, MetaIcon, IconSize))
            Next
        Next

        Return True

    End Function

    Public Shared Function CombineIcons48() As Boolean
        Dim IconSize As Integer = 48
        ItemIcons48.Clear()

        For Each BaseIcon As String In BaseIcons.Keys
            For Each MetaIcon As String In MetaIcons.Keys
                ItemIcons48.Add(BaseIcon & "_" & MetaIcon, CreateIcon(BaseIcon, MetaIcon, IconSize))
            Next
        Next

        Return True

    End Function

    Public Shared Function CreateIcon(ByVal BaseIcon As String, ByVal MetaIcon As String, ByVal IconSize As Integer, Optional ByVal IsTypeID As Boolean = False) As Bitmap
        Dim OI As Image

        If IsTypeID = True Then
            OI = EveHQ.Core.ImageHandler.GetImage(BaseIcon)
        Else
            OI = ImageHandler.BaseIcons(BaseIcon)
        End If

        If OI Is Nothing Then
            Return Nothing
        End If

        ' Resize the image
        Dim icon As Bitmap = New Bitmap(OI, IconSize, IconSize)

        If MetaIcon <> "1" Then
            Dim IO As Bitmap = ImageHandler.MetaIcons(MetaIcon)

            ' Apply the overlay
            Dim g As Graphics = Graphics.FromImage(icon)
            g.DrawImage(IO, 0, 0)
        End If

        Return icon

    End Function

End Class
