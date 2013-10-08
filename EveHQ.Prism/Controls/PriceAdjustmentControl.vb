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
Imports EveHQ.EveData

Namespace Controls

    Public Class PriceAdjustmentControl
        Dim _typeID As Integer = 0
        Dim _price As Double = 0

        Public Property TypeID() As Integer
            Get
                Return _typeID
            End Get
            Set(ByVal value As Integer)
                _typeID = value
                If value > 0 And StaticData.Types.ContainsKey(_typeID) = True Then
                    STT.SetSuperTooltip(pbPAC, New DevComponents.DotNetBar.SuperTooltipInfo("Modify Price", StaticData.Types(_typeID).Name, "Click here to modify the price of this item", My.Resources.pound32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                Else
                    STT.SetSuperTooltip(pbPAC, New DevComponents.DotNetBar.SuperTooltipInfo("Modify Price", "", "An item has not been allocated for price modification. Make sure a relevant item is selected first.", My.Resources.pound32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                End If
            End Set
        End Property

        Public Property Price() As Double
            Get
                Return _price
            End Get
            Set(ByVal value As Double)
                _price = value
            End Set
        End Property

        Public Event PriceUpdated()

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            STT.SetSuperTooltip(pbPAC, New DevComponents.DotNetBar.SuperTooltipInfo("Modify Price", "", "An item has not been allocated for price modification. Make sure a relevant item is selected first.", My.Resources.pound32, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))

        End Sub

        Private Sub pbPAC_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbPAC.Click
            If _typeID > 0 And StaticData.Types.ContainsKey(_typeID) = True Then
                Dim newPriceForm As New Core.frmModifyPrice(_typeID, _price)
                newPriceForm.ShowDialog()
                RaiseEvent PriceUpdated()
                newPriceForm.Dispose()
            End If
        End Sub

    End Class
End NameSpace