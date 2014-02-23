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

Namespace Forms

    Public Class FrmShipClassEditor

        Dim _newShipClass As CustomShipClass = Nothing
        Dim _editMode As Boolean = False

        ''' <summary>
        ''' Gets the instance of the CustomShipClass that was generated from the Ship Class Editor form
        ''' </summary>
        ''' <value></value>
        ''' <returns>An instance of the CustomShipClass that was created from the Ship Class Editor form</returns>
        ''' <remarks></remarks>
        Public Property NewShipClass() As CustomShipClass
            Get
                Return _newShipClass
            End Get
            Set(ByVal value As CustomShipClass)
                _newShipClass = value
                _editMode = True
                txtClassName.Text = _newShipClass.Name
                txtDescription.Text = _newShipClass.Description
            End Set
        End Property

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Are we adding or editing this?
            If _editMode = False Then
                ' Check if we have a valid name
                If txtClassName.Text.Trim = "" Then
                    MessageBox.Show("Class Name cannot be blank. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' Check the name doesn't already exist in the basic ship groups
                    Dim className As String = txtClassName.Text.Trim
                    If Market.MarketShipList.Contains(className) = True Then
                        MessageBox.Show("This class name already exists from the ship market groups. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        ' Check it doesn't exist in our custom classes
                        If CustomHQFClasses.CustomShipClasses.ContainsKey(className) = True Then
                            MessageBox.Show("This class name already exists in the custom ship classes. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Else
                            ' Seems ok to add this and add it to the form properties, then close
                            ' Leave it to the caller to handle the response if required
                            _newShipClass = New CustomShipClass
                            _newShipClass.Name = className
                            _newShipClass.Description = txtDescription.Text.Trim
                            DialogResult = DialogResult.OK
                            Close()
                        End If
                    End If
                End If
            Else
                ' Check if we have a valid name
                If txtClassName.Text.Trim = "" Then
                    MessageBox.Show("Class Name cannot be blank. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    ' Check the name doesn't already exist in the basic ship groups
                    Dim className As String = txtClassName.Text.Trim
                    If Market.MarketShipList.Contains(className) = True Then
                        MessageBox.Show("This class name already exists from the ship market groups. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        ' Check it doesn't exist in our custom classes
                        If CustomHQFClasses.CustomShipClasses.ContainsKey(className) = True And className <> _newShipClass.Name Then
                            MessageBox.Show("This class name already exists in the custom ship classes. Please try again.", "Invalid Ship Class Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Else
                            ' Seems ok to add this and add it to the form properties, then close
                            ' Leave it to the caller to handle the response if required
                            _newShipClass = New CustomShipClass
                            _newShipClass.Name = className
                            _newShipClass.Description = txtDescription.Text.Trim
                            DialogResult = DialogResult.OK
                            Close()
                        End If
                    End If
                End If
            End If
        End Sub
    End Class
End NameSpace