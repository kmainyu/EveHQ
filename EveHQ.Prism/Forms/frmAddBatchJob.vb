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

Public Class frmAddBatchJob

    Dim cJobName As String = ""
    Dim EditMode As Boolean = False

    Public Property JobName() As String
        Get
            Return cJobName
        End Get
        Set(ByVal value As String)
            cJobName = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cJobName = ""
        EditMode = False

    End Sub

    Public Sub New(ByVal JobName As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cJobName = JobName
        EditMode = True
        txtJobName.Text = cJobName

    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
        If txtJobName.Text = "" Then
            MessageBox.Show("You must enter some valid text to set a name!", "Batch Name Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Get the mode we are using
            If EditMode = False Then
                ' Adding a new name
                If BatchJobs.Jobs.ContainsKey(txtJobName.Text) = False Then
                    cJobName = txtJobName.Text
                Else
                    MessageBox.Show("Batch Name already exists - please choose another name.", "Batch Name Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            Else
                ' Editing a name
                If BatchJobs.Jobs.ContainsKey(txtJobName.Text) = False Or (BatchJobs.Jobs.ContainsKey(txtJobName.Text) = True And txtJobName.Text = cJobName) Then
                    cJobName = txtJobName.Text
                Else
                    MessageBox.Show("Batch Name already exists - please choose another name.", "Batch Name Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            End If
        End If
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class
