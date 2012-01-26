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

Public Class frmSelectQuantity

    Private cQuantity As Integer

#Region "Form Constructor"

    Public Sub New(ByVal DefaultValue As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cQuantity = DefaultValue

    End Sub

#End Region

    Public Property Quantity() As Integer
        Get
            Return cQuantity
        End Get
        Set(ByVal value As Integer)
            cQuantity = value
        End Set
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmSelectQuantity_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        nudQuantity.Value = cQuantity
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        cQuantity = nudQuantity.Value
        Me.Close()
    End Sub


   
End Class