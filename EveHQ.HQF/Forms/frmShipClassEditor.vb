' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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

Public Class frmShipClassEditor

    Dim cNewShipClass As CustomShipClass = Nothing
    Dim EditMode As Boolean = False

    ''' <summary>
    ''' Gets the instance of the CustomShipClass that was generated from the Ship Class Editor form
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of the CustomShipClass that was created from the Ship Class Editor form</returns>
    ''' <remarks></remarks>
    Public Property NewShipClass() As CustomShipClass
        Get
            Return cNewShipClass
        End Get
        Set(ByVal value As CustomShipClass)
            cNewShipClass = value
            EditMode = True
            txtClassName.Text = cNewShipClass.Name
            txtDescription.Text = cNewShipClass.Description
        End Set
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Are we adding or editing this?
        If EditMode = False Then
            ' Check if we have a valid name
            If txtClassName.Text.Trim = "" Then
                MessageBox.Show("Class Name cannot be blank. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                ' Check the name doesn't already exist in the basic ship groups
                Dim ClassName As String = txtClassName.Text.Trim
                If Market.MarketShipList.Contains(ClassName) = True Then
                    MessageBox.Show("This class name already exists from the ship market groups. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' Check it doesn't exist in our custom classes
                    If CustomHQFClasses.CustomShipClasses.ContainsKey(ClassName) = True Then
                        MessageBox.Show("This class name already exists in the custom ship classes. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        ' Seems ok to add this and add it to the form properties, then close
                        ' Leave it to the caller to handle the response if required
                        cNewShipClass = New CustomShipClass
                        cNewShipClass.Name = ClassName
                        cNewShipClass.Description = txtDescription.Text.Trim
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    End If
                End If
            End If
        Else
            ' Check if we have a valid name
            If txtClassName.Text.Trim = "" Then
                MessageBox.Show("Class Name cannot be blank. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                ' Check the name doesn't already exist in the basic ship groups
                Dim ClassName As String = txtClassName.Text.Trim
                If Market.MarketShipList.Contains(ClassName) = True Then
                    MessageBox.Show("This class name already exists from the ship market groups. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' Check it doesn't exist in our custom classes
                    If CustomHQFClasses.CustomShipClasses.ContainsKey(ClassName) = True And ClassName <> cNewShipClass.Name Then
                        MessageBox.Show("This class name already exists in the custom ship classes. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        ' Seems ok to add this and add it to the form properties, then close
                        ' Leave it to the caller to handle the response if required
                        cNewShipClass = New CustomShipClass
                        cNewShipClass.Name = ClassName
                        cNewShipClass.Description = txtDescription.Text.Trim
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    End If
                End If
            End If
        End If
    End Sub
End Class