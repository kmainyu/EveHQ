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
