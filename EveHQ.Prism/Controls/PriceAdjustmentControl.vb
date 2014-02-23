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
Imports DevComponents.DotNetBar
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
                    STT.SetSuperTooltip(pbPAC, New SuperTooltipInfo("Modify Price", StaticData.Types(_typeID).Name, "Click here to modify the price of this item", My.Resources.pound32, Nothing, eTooltipColor.Yellow))
                Else
                    STT.SetSuperTooltip(pbPAC, New SuperTooltipInfo("Modify Price", "", "An item has not been allocated for price modification. Make sure a relevant item is selected first.", My.Resources.pound32, Nothing, eTooltipColor.Yellow))
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
            STT.SetSuperTooltip(pbPAC, New SuperTooltipInfo("Modify Price", "", "An item has not been allocated for price modification. Make sure a relevant item is selected first.", My.Resources.pound32, Nothing, eTooltipColor.Yellow))

        End Sub

        Private Sub pbPAC_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbPAC.Click
            If _typeID > 0 And StaticData.Types.ContainsKey(_typeID) = True Then
                Using newPriceForm As New FrmModifyPrice(_typeID, _price)
                    newPriceForm.ShowDialog()
                    RaiseEvent PriceUpdated()
                End Using
            End If
        End Sub

    End Class
End NameSpace