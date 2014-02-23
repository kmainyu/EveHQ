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

Imports EveHQ.Core

Namespace Forms
    Public Class FrmEditImplants

        Dim _displayPilotName As String
        Dim _displayPilot As EveHQPilot

        Public Property DisplayPilotName() As String
            Get
                Return _displayPilotName
            End Get
            Set(ByVal value As String)
                _displayPilotName = value
                _displayPilot = HQ.Settings.Pilots(value)
            End Set
        End Property

        Private Sub nudC_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudC.ValueChanged
            _displayPilot.CImplantM = CInt(nudC.Value)
        End Sub

        Private Sub nudI_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudI.ValueChanged
            _displayPilot.IntImplantM = CInt(nudI.Value)
        End Sub

        Private Sub nudM_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudM.ValueChanged
            _displayPilot.MImplantM = CInt(nudM.Value)
        End Sub

        Private Sub nudP_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudP.ValueChanged
            _displayPilot.PImplantM = CInt(nudP.Value)
        End Sub

        Private Sub nudW_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudW.ValueChanged
            _displayPilot.WImplantM = CInt(nudW.Value)
        End Sub

        Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
            Close()
        End Sub

        Private Sub frmEditImplants_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            nudC.Value = _displayPilot.CImplantM
            nudI.Value = _displayPilot.IntImplantM
            nudM.Value = _displayPilot.MImplantM
            nudP.Value = _displayPilot.PImplantM
            nudW.Value = _displayPilot.WImplantM
        End Sub
    End Class
End NameSpace