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

Imports System.Windows.Forms
Imports EveHQ.Core
Imports EveHQ.EveData
Imports EveHQ.Prism.Classes

Namespace Forms

    Public Class FrmAddBPCPrice

        ReadOnly _blueprintID As Integer

#Region "Form Constructor"

        Public Sub New(ByVal bpid As Integer)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _blueprintID = BPID
            pbBP.ImageLocation = ImageHandler.GetImageLocation(_blueprintID)
            lblBPName.Text = StaticData.Types(_blueprintID).Name

            If PrismSettings.UserSettings.BPCCosts.ContainsKey(_blueprintID) = True Then
                nudMinRunCost.Value = PrismSettings.UserSettings.BPCCosts(_blueprintID).MinRunCost
                nudMaxRunCost.Value = PrismSettings.UserSettings.BPCCosts(_blueprintID).MaxRunCost
            End If

        End Sub

#End Region

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click

            ' Check the min cost isn't greater than the max cost

            If nudMinRunCost.Value > nudMaxRunCost.Value Then
                MessageBox.Show("Minimum Value cannot exceed the Maximum Run Value - please adjust the values.", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            ' Save the value
            If PrismSettings.UserSettings.BPCCosts.ContainsKey(_blueprintID) = False Then
                Dim bpcInfo As New BlueprintCopyCostInfo(_blueprintID, nudMinRunCost.Value, nudMaxRunCost.Value)
                PrismSettings.UserSettings.BPCCosts.Add(_blueprintID, bpcInfo)
            Else
                Dim bpCinfo As BlueprintCopyCostInfo = PrismSettings.UserSettings.BPCCosts(_blueprintID)
                bpCinfo.MinRunCost = nudMinRunCost.Value
                bpCinfo.MaxRunCost = nudMaxRunCost.Value
            End If

            ' Close the form
            DialogResult = DialogResult.OK
            Close()

        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

            DialogResult = DialogResult.Cancel
            Close()

        End Sub

    End Class
End NameSpace