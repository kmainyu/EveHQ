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
Imports EveHQ.Core

Namespace Forms

    Public Class FrmAssetItemName

        Dim _assetID As Integer
        Dim _assetName As String = ""
        Dim _assetItemName As String = ""
        Dim _editMode As Boolean = False
        Public Property AssetID() As Integer
            Get
                Return _assetID
            End Get
            Set(ByVal value As Integer)
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
            lblDescription.Text = "Please enter a name for the " & AssetName & " (assetID #" & _assetID & ")"
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

        Private Function AddAssetItemName(ByVal aID As Integer, ByVal aName As String) As Boolean
            Dim assetSQL As String = "INSERT INTO assetItemNames (itemID, itemName) VALUES (" & aID & ", '" & aName & "');"
            If CustomDataFunctions.SetCustomData(assetSQL) = -2 Then
                MessageBox.Show("There was an error writing data to the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End Function

        Private Function EditAssetItemName(ByVal aID As Integer, ByVal aName As String) As Boolean
            Dim assetSQL As String = "UPDATE assetItemNames SET itemName='" & aName & "' WHERE itemID=" & aID & ";"
            If CustomDataFunctions.SetCustomData(assetSQL) = -2 Then
                MessageBox.Show("There was an error writing data to the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        End Function
    End Class
End NameSpace