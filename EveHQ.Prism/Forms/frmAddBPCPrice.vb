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

Public Class frmAddBPCPrice

    Dim BlueprintID As Integer

#Region "Form Constructor"

    Public Sub New(ByVal BPID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        BlueprintID = BPID
        pbBP.ImageLocation = Core.ImageHandler.GetImageLocation(BlueprintID)
        lblBPName.Text = EveData.StaticData.Types(BlueprintID).Name

        If Settings.PrismSettings.BPCCosts.ContainsKey(BlueprintID) = True Then
            nudMinRunCost.Value = Settings.PrismSettings.BPCCosts(BlueprintID).MinRunCost
            nudMaxRunCost.Value = Settings.PrismSettings.BPCCosts(BlueprintID).MaxRunCost
        End If

    End Sub

#End Region

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        ' Check the min cost isn't greater than the max cost

        If nudMinRunCost.Value > nudMaxRunCost.Value Then
            MessageBox.Show("Minimum Value cannot exceed the Maximum Run Value - please adjust the values.", "Value Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        ' Save the value
        If Settings.PrismSettings.BPCCosts.ContainsKey(BlueprintID) = False Then
            Dim BPCInfo As New BPCCostInfo(BlueprintID, nudMinRunCost.Value, nudMaxRunCost.Value)
            Settings.PrismSettings.BPCCosts.Add(BlueprintID, BPCInfo)
        Else
            Dim BPCinfo As BPCCostInfo = Settings.PrismSettings.BPCCosts(BlueprintID)
            BPCinfo.MinRunCost = nudMinRunCost.Value
            BPCinfo.MaxRunCost = nudMaxRunCost.Value
        End If

        ' Close the form
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

End Class