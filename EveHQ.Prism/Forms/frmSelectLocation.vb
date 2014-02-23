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

Namespace Forms

    Public Class FrmSelectLocation

        Private _location As String
        Private _bpLocations As List(Of String)
        Private _includeBPOs As Boolean = False

        Public ReadOnly Property BPLocation() As String
            Get
                Return _location
            End Get
        End Property

        Public Property BPLocations() As List(Of String)
            Get
                Return _bpLocations
            End Get
            Set(ByVal value As List(Of String))
                _bpLocations = value
            End Set
        End Property

        Public ReadOnly Property IncludeBPOs() As Boolean
            Get
                Return _includeBPOs
            End Get
        End Property

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            If cboLocations.SelectedItem IsNot Nothing Then
                _location = cboLocations.SelectedItem.ToString
            End If
            Close()
        End Sub

        Private Sub chkIncludeBPOs_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkIncludeBPOs.CheckedChanged
            _includeBPOs = chkIncludeBPOs.Checked
        End Sub

        Private Sub frmSelectLocation_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' List all locations of blueprints
            cboLocations.Items.Clear()
            BPLocations.Sort()
            cboLocations.BeginUpdate()
            For Each bpLoc As String In BPLocations
                cboLocations.Items.Add(bpLoc)
            Next
            cboLocations.EndUpdate()
        End Sub

    End Class
End NameSpace