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