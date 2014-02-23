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
Imports EveHQ.Core

Namespace Forms

    Public Class FrmAssetItemName

        Dim _assetID As Long
        Dim _assetName As String = ""
        Dim _assetItemName As String = ""
        Dim _editMode As Boolean = False
        Public Property AssetID() As Long
            Get
                Return _assetID
            End Get
            Set(ByVal value As Long)
                _assetID = value
            End Set
        End Property
        Public Property AssetName() As String
            Get
                Return _assetName
            End Get
            Set(ByVal value As String)
                _assetName = value
            End Set
        End Property
        Public Property AssetItemName() As String
            Get
                Return _assetItemName
            End Get
            Set(ByVal value As String)
                _assetItemName = value
            End Set
        End Property
        Public Property EditMode() As Boolean
            Get
                Return _editMode
            End Get
            Set(ByVal value As Boolean)
                _editMode = value
            End Set
        End Property

        Private Sub frmAssetItemName_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            lblDescription.Text = "Please enter a name for the " & AssetName & ControlChars.CrLf & "AssetID #" & _assetID & ")"
            If _editMode = True Then
                txtAssetItemName.Text = PlugInData.AssetItemNames(_assetID)
            End If
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            If txtAssetItemName.Text = "" Then
                MessageBox.Show("You must enter some valid text to set a name", "Text Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                _assetItemName = txtAssetItemName.Text
                ' Get the mode we are using
                If _editMode = False Then
                    ' Adding a new name
                    If AddAssetItemName(_assetID, _assetItemName.Replace("'", "''")) = True Then
                        PlugInData.AssetItemNames.Add(_assetID, _assetItemName)
                    Else
                        _assetItemName = ""
                    End If
                Else
                    ' Editing a name
                    If EditAssetItemName(_assetID, _assetItemName.Replace("'", "''")) = True Then
                        PlugInData.AssetItemNames(_assetID) = _assetItemName
                    Else
                        _assetItemName = ""
                    End If
                End If
            End If
            Close()
        End Sub

        Private Function AddAssetItemName(ByVal aID As Long, ByVal aName As String) As Boolean
            Dim assetSQL As String = "INSERT INTO assetItemNames (itemID, itemName) VALUES (" & aID & ", '" & aName & "');"
            If CustomDataFunctions.SetCustomData(assetSQL) = -2 Then
                MessageBox.Show("There was an error writing data to the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End Function

        Private Function EditAssetItemName(ByVal aID As Long, ByVal aName As String) As Boolean
            Dim assetSQL As String = "UPDATE assetItemNames SET itemName='" & aName & "' WHERE itemID=" & aID & ";"
            If CustomDataFunctions.SetCustomData(assetSQL) = -2 Then
                MessageBox.Show("There was an error writing data to the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.DataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End Function
    End Class
End NameSpace