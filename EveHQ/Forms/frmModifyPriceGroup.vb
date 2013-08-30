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
Public Class frmModifyPriceGroup

    Dim EditMode As Boolean = False
    Dim OldPriceGroupName As String = ""
    Dim cNewPriceGroupName As String = ""

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        EditMode = False
        Me.TitleText = "Add Price Group"
        lblDescription.Text = "Enter the name of the new Price Group..."
        OldPriceGroupName = ""

    End Sub

    Public Sub New(PriceGroupName As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        EditMode = True
        Me.TitleText = "Edit Price Group"
        lblDescription.Text = "Enter the new name of the Price Group..."
        OldPriceGroupName = PriceGroupName

    End Sub

    Public ReadOnly Property NewPriceGroupName As String
        Get
            Return cNewPriceGroupName
        End Get
    End Property

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if the input is valid i.e. not blank
        If txtPriceGroup.Text = "" Then
            Dim reply As Integer = MessageBox.Show("Price Group Name cannot be blank! Would you like to try again?", "Name Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Retry Then
                Exit Sub
            Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.cNewPriceGroupName = ""
                Me.Close()
                Exit Sub
            End If
        End If

        ' Check if the name already exists
        Dim TestPriceGroupName As String = txtPriceGroup.Text
        If EveHQ.Core.HQ.EveHqSettings.PriceGroups.ContainsKey(TestPriceGroupName) Then
            Dim reply As Integer = MessageBox.Show("Price Group Name already exists! Would you like to try again?", "Name Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Retry Then
                Exit Sub
            Else
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.cNewPriceGroupName = ""
                Me.Close()
                Exit Sub
            End If
        End If

        If EditMode = False Then
            ' Create a new price group
            Dim NewPriceGroup As New EveHQ.Core.PriceGroup
            NewPriceGroup.Name = TestPriceGroupName
            EveHQ.Core.HQ.EveHqSettings.PriceGroups.Add(NewPriceGroup.Name, NewPriceGroup)
        Else
            Dim OldGroup As New EveHQ.Core.PriceGroup
            ' Get the data from the old price group and place into a new key
            If EveHQ.Core.HQ.EveHqSettings.PriceGroups.ContainsKey(OldPriceGroupName) = True Then
                OldGroup = EveHQ.Core.HQ.EveHqSettings.PriceGroups(OldPriceGroupName)
                EveHQ.Core.HQ.EveHqSettings.PriceGroups.Remove(OldPriceGroupName)
            Else
                MessageBox.Show("Ooops! EveHQ cannot find the original Price Group but will create a new one anyway.", "Old Price Group Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            ' Create the new group
            OldGroup.Name = TestPriceGroupName
            EveHQ.Core.HQ.EveHqSettings.PriceGroups.Add(OldGroup.Name, OldGroup)
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.cNewPriceGroupName = TestPriceGroupName
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class