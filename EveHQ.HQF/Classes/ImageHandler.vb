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

    Public Shared Function IconImage24(ByVal iconName As String, ByVal metaLevel As Integer) As Image

        If iconName <> "" Then
            If ItemIcons24.ContainsKey(iconName & "_" & MetaLevel.ToString) Then
                Return ItemIcons24(iconName & "_" & metaLevel.ToString)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function IconImage48(ByVal iconName As String, ByVal metaLevel As Integer) As Image

        If iconName <> "" Then
            If ItemIcons48.ContainsKey(iconName & "_" & MetaLevel.ToString) Then
                Return ItemIcons48(iconName & "_" & metaLevel.ToString)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function CombineIcons24() As Boolean
        Const IconSize As Integer = 24
        ItemIcons24.Clear()

        For Each baseIcon As String In BaseIcons.Keys
            For Each metaIcon As String In MetaIcons.Keys
                ItemIcons24.Add(baseIcon & "_" & metaIcon, CreateIcon(baseIcon, metaIcon, IconSize))
            Next
        Next

        Return True

    End Function

    Public Shared Function CombineIcons48() As Boolean
        Const IconSize As Integer = 48
        ItemIcons48.Clear()

        For Each baseIcon As String In BaseIcons.Keys
            For Each metaIcon As String In MetaIcons.Keys
                ItemIcons48.Add(baseIcon & "_" & metaIcon, CreateIcon(baseIcon, metaIcon, IconSize))
            Next
        Next

        Return True

    End Function

    Public Shared Function CreateIcon(ByVal baseIcon As String, ByVal metaIcon As String, ByVal iconSize As Integer, Optional ByVal isTypeID As Boolean = False) As Bitmap
        Dim oi As Image

        If isTypeID = True Then
            oi = Core.ImageHandler.GetImage(CInt(baseIcon))
        Else
            oi = BaseIcons(baseIcon)
        End If

        If oi Is Nothing Then
            Return Nothing
        End If

        ' Resize the image
        Dim icon As Bitmap = New Bitmap(oi, iconSize, iconSize)

        If metaIcon <> "1" Then
            Dim io As Bitmap = MetaIcons(metaIcon)

            ' Apply the overlay
            Dim g As Graphics = Graphics.FromImage(icon)
            g.DrawImage(io, 0, 0)
        End If

        Return icon

    End Function

End Class
