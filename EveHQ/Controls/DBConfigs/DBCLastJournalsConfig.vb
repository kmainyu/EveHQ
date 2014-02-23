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
Imports EveHQ.Controls.DBControls

Namespace Controls.DBConfigs
    Public Class DBCLastJournalsConfig
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Load the combo box with the pilot info
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()

            For Each pilot As EveHQPilot In HQ.Settings.Pilots.Values
                If pilot.Active = True And pilot.Account <> "" Then
                    cboPilots.Items.Add(pilot.Name)
                End If
            Next

            cboPilots.EndUpdate()

        End Sub

#Region "Properties"

        Dim _dbWidget As New DBCLastJournals
        Public Property DBWidget() As DBCLastJournals
            Get
                Return _dbWidget
            End Get
            Set(ByVal value As DBCLastJournals)
                _dbWidget = value
                Call SetControlInfo()
            End Set
        End Property

#End Region

        Private Sub SetControlInfo()
            If cboPilots.Items.Contains(_dbWidget.DBCDefaultPilotName) = True Then
                cboPilots.SelectedItem = _dbWidget.DBCDefaultPilotName
            Else
                If cboPilots.Items.Count > 0 Then
                    cboPilots.SelectedIndex = 0
                End If
            End If
            spinDefaultJournal.Value = _dbWidget.DBCDefaultTransactionsCount

        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Update the control properties
            If cboPilots.SelectedItem IsNot Nothing Then
                _dbWidget.DBCDefaultPilotName = cboPilots.SelectedItem.ToString
            Else
                MessageBox.Show("You must select a valid Pilot before adding this widget.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            _dbWidget.DBCDefaultTransactionsCount = CInt(spinDefaultJournal.Value)
            ' Now close the form
            DialogResult = DialogResult.OK
            Close()
        End Sub
    End Class
End NameSpace