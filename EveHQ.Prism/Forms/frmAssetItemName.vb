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

Public Class frmAssetItemName

    Dim cAssetID As Integer
    Dim cAssetName As String = ""
    Dim cAssetItemName As String = ""
    Dim cEditMode As Boolean = False
    Public Property AssetID() As Integer
        Get
            Return cAssetID
        End Get
        Set(ByVal value As Integer)
            cAssetID = value
        End Set
    End Property
    Public Property AssetName() As String
        Get
            Return cAssetName
        End Get
        Set(ByVal value As String)
            cAssetName = value
        End Set
    End Property
    Public Property AssetItemName() As String
        Get
            Return cAssetItemName
        End Get
        Set(ByVal value As String)
            cAssetItemName = value
        End Set
    End Property
    Public Property EditMode() As Boolean
        Get
            Return cEditMode
        End Get
        Set(ByVal value As Boolean)
            cEditMode = value
        End Set
    End Property

    Private Sub frmAssetItemName_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblDescription.Text = "Please enter a name for the " & AssetName & " (assetID #" & cAssetID & ")"
        If cEditMode = True Then
            txtAssetItemName.Text = PlugInData.AssetItemNames(cAssetID)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        If txtAssetItemName.Text = "" Then
            MessageBox.Show("You must enter some valid text to set a name", "Text Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            cAssetItemName = txtAssetItemName.Text
            ' Get the mode we are using
            If cEditMode = False Then
                ' Adding a new name
                If Me.AddAssetItemName(cAssetID, cAssetItemName.Replace("'", "''")) = True Then
                    PlugInData.AssetItemNames.Add(cAssetID, cAssetItemName)
                Else
                    cAssetItemName = ""
                End If
            Else
                ' Editing a name
                If Me.EditAssetItemName(cAssetID, cAssetItemName.Replace("'", "''")) = True Then
                    PlugInData.AssetItemNames(cAssetID) = cAssetItemName
                Else
                    cAssetItemName = ""
                End If
            End If
        End If
        Me.Close()
    End Sub

    Private Function AddAssetItemName(ByVal assetID As Integer, ByVal assetName As String) As Boolean
        Dim assetSQL As String = "INSERT INTO assetItemNames (itemID, itemName) VALUES (" & assetID & ", '" & assetName & "');"
        If Core.CustomDataFunctions.SetCustomData(assetSQL) = -2 Then
            MessageBox.Show("There was an error writing data to the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            Return True
        End If
    End Function

    Private Function EditAssetItemName(ByVal assetID As Integer, ByVal assetName As String) As Boolean
        Dim assetSQL As String = "UPDATE assetItemNames SET itemName='" & assetName & "' WHERE itemID=" & assetID & ";"
        If Core.CustomDataFunctions.SetCustomData(assetSQL) = -2 Then
            MessageBox.Show("There was an error writing data to the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            Return True
        End If
    End Function
End Class