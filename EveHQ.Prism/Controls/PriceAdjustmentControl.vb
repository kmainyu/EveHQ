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

Public Class PriceAdjustmentControl
    Dim cTypeID As Long = 0
    Dim cPrice As Double = 0

    Public Property TypeID() As Long
        Get
            Return cTypeID
        End Get
        Set(ByVal value As Long)
            cTypeID = value
            If value > 0 And EveHQ.Core.HQ.itemData.ContainsKey(cTypeID.ToString) = True Then
                STT.SetSuperTooltip(Me.pbPAC, New DevComponents.DotNetBar.SuperTooltipInfo("Modify Price", EveHQ.Core.HQ.itemData(cTypeID.ToString).Name, "Click here to modify the price of this item", My.Resources.pound32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
            Else
                STT.SetSuperTooltip(Me.pbPAC, New DevComponents.DotNetBar.SuperTooltipInfo("Modify Price", "", "An item has not been allocated for price modification. Make sure a relevant item is selected first.", My.Resources.pound32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
            End If
        End Set
    End Property

    Public Property Price() As Double
        Get
            Return cPrice
        End Get
        Set(ByVal value As Double)
            cPrice = value
        End Set
    End Property

    Public Event PriceUpdated()

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        STT.SetSuperTooltip(Me.pbPAC, New DevComponents.DotNetBar.SuperTooltipInfo("Modify Price", "", "An item has not been allocated for price modification. Make sure a relevant item is selected first.", My.Resources.pound32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))

    End Sub

    Private Sub pbPAC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbPAC.Click
        If cTypeID > 0 And EveHQ.Core.HQ.itemData.ContainsKey(cTypeID.ToString) = True Then
            Dim NewPriceForm As New EveHQ.Core.frmModifyPrice(cTypeID.ToString, cPrice)
            NewPriceForm.ShowDialog()
            RaiseEvent PriceUpdated()
            NewPriceForm.Dispose()
        End If
    End Sub

End Class
