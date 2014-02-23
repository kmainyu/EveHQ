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

Imports System.IO
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar

Namespace Forms

    Public Class FrmShipEditor

        ReadOnly _shipRaces As New SortedList(Of String, String)

#Region "Form Loading Routines"

        Private Sub frmShipEditor_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
            ' Implement the data before we close the form
            Call CustomHQFClasses.ImplementCustomShips()
        End Sub

        Private Sub frmShipEditor_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' Note: Loading of the custom ship classes and ships is done on loading HQF to allow them to be implemented in the main fitter
            Call LoadShipInformation()
            Call LoadCustomShipClasses()
            Call LoadCustomShips()
        End Sub

        Private Sub LoadShipInformation()
            Dim sr As New StreamReader(Path.Combine(PluginSettings.HQFCacheFolder, "ShipGroups.bin"))
            Dim shipGroups As String = sr.ReadToEnd
            Dim pathLines() As String = shipGroups.Split(ControlChars.CrLf.ToCharArray)
            Dim data() As String
            sr.Close()
            _shipRaces.Clear()
            Market.ShipClasses.Clear()
            For Each pathline As String In pathLines
                If pathline.Trim <> "" Then
                    data = pathline.Split("\".ToCharArray)
                    ' Check if the last item is a ship type
                    If ShipLists.ShipListKeyName.ContainsKey(data(data.Length - 1)) = True Then
                        If _shipRaces.ContainsKey(data(data.Length - 1)) = False Then
                            _shipRaces.Add(data(data.Length - 1), data(data.Length - 2))
                        End If
                        If Market.ShipClasses.ContainsKey(data(data.Length - 1)) = False Then
                            Market.ShipClasses.Add(data(data.Length - 1), data(data.Length - 3))
                        End If
                    End If
                End If
            Next
        End Sub

        Private Sub LoadCustomShipClasses()
            ' Load the classes from storage
            CustomHQFClasses.LoadCustomShipClasses()
            ' Display the details
            Call UpdateCustomShipClassList()
        End Sub

        Private Sub UpdateCustomShipClassList()
            adtShipClasses.BeginUpdate()
            adtShipClasses.Nodes.Clear()
            For Each newClass As String In CustomHQFClasses.CustomShipClasses.Keys
                Dim newNode As New Node
                newNode.Text = newClass
                adtShipClasses.Nodes.Add(newNode)
                sttInfo.SetSuperTooltip(newNode, New SuperTooltipInfo(newNode.Text, "", CustomHQFClasses.CustomShipClasses(newClass).Description, My.Resources.imgInfo2, Nothing, eTooltipColor.Yellow))
            Next
            adtShipClasses.EndUpdate()
        End Sub

        Private Sub LoadCustomShips()
            ' Load the custom ships from storage
            CustomHQFClasses.LoadCustomShips()
            ' Display the ships
            Call UpdateCustomShipList()
        End Sub

        Private Sub UpdateCustomShipList()
            adtShips.BeginUpdate()
            adtShips.Nodes.Clear()
            For Each newShip As CustomShip In CustomHQFClasses.CustomShips.Values
                Dim newNode As New Node
                newNode.Text = newShip.Name
                Dim hullTypeCell As New Cell(newShip.BaseShipName)
                Dim shipClassCell As New Cell(newShip.ShipClass)
                newNode.Cells.Add(hullTypeCell)
                newNode.Cells.Add(shipClassCell)
                adtShips.Nodes.Add(newNode)
                sttInfo.SetSuperTooltip(newNode, New SuperTooltipInfo(newNode.Text, "", newShip.ShipData.Description, My.Resources.imgInfo2, Nothing, eTooltipColor.Yellow))
            Next
            adtShips.EndUpdate()
        End Sub

#End Region

#Region "Ship Class Button Routines"

        Private Sub btnAddClass_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddClass.Click
            ' Add a new ShipClassEditor form
            Using newClassForm As New FrmShipClassEditor
                newClassForm.ShowDialog()
                If newClassForm.DialogResult = DialogResult.OK Then
                    If newClassForm.NewShipClass IsNot Nothing Then
                        ' User clicked Accept and the NewShipClass property is not blank, so add the new class
                        CustomHQFClasses.CustomShipClasses.Add(newClassForm.NewShipClass.Name, newClassForm.NewShipClass)
                        ' Update the list
                        Call UpdateCustomShipClassList()
                        ' Save the list
                        Call CustomHQFClasses.SaveCustomShipClasses()
                    End If
                End If
                ' Dispose of the form
            End Using
        End Sub

        Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
            Dim reply As DialogResult = MessageBox.Show("This will reset the class type of all your custom ships. Are you sure you wish to clear all your custom classes?", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.Yes Then
                ' Reset any custom ship classes to the basic hull type
                Dim classList As New List(Of String)
                For Each delClass As String In CustomHQFClasses.CustomShipClasses.Keys
                    classList.Add(delClass)
                Next
                Call ResetShipClasses(classList)
                ' Delete all the classes
                CustomHQFClasses.CustomShipClasses.Clear()
                ' Update the list
                Call UpdateCustomShipClassList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShipClasses()
            End If
        End Sub

        Private Sub btnEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEdit.Click
            If adtShipClasses.SelectedNodes.Count > 0 Then
                ' Get the existing class and set it in the form
                Dim newShipClass As CustomShipClass = CustomHQFClasses.CustomShipClasses(adtShipClasses.SelectedNodes(0).Text)
                Call EditShipClass(newShipClass)
            End If
        End Sub

        Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
            If adtShipClasses.SelectedNodes.Count > 0 Then
                Dim reply As DialogResult = MessageBox.Show("This will reset the class type of the custom ships with the selected classes. Are you sure you wish to delete these your custom classes?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.Yes Then
                    ' Remove the items from teh custom list
                    Dim classList As New List(Of String)
                    For Each delNode As Node In adtShipClasses.SelectedNodes
                        classList.Add(delNode.Text)
                        CustomHQFClasses.CustomShipClasses.Remove(delNode.Text)
                    Next
                    ' Reset any custom ship classes to the basic hull type
                    Call ResetShipClasses(classList)
                    ' Update the list
                    Call UpdateCustomShipClassList()
                    ' Save the list
                    Call CustomHQFClasses.SaveCustomShipClasses()
                End If
            End If
        End Sub

        Private Sub adtShipClasses_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtShipClasses.NodeDoubleClick
            ' Get the existing class and set it in the form
            Dim newShipClass As CustomShipClass = CustomHQFClasses.CustomShipClasses(e.Node.Text)
            Call EditShipClass(newShipClass)
        End Sub

        Private Sub EditShipClass(ByVal newShipClass As CustomShipClass)
            ' Add a new ShipClassEditor form
            Using newClassForm As New FrmShipClassEditor
                newClassForm.NewShipClass = newShipClass
                newClassForm.ShowDialog()
                If newClassForm.DialogResult = DialogResult.OK Then
                    If newClassForm.NewShipClass IsNot Nothing Then
                        ' Remove the old class
                        CustomHQFClasses.CustomShipClasses.Remove(adtShipClasses.SelectedNodes(0).Text)
                        ' Add the new class
                        CustomHQFClasses.CustomShipClasses.Add(newClassForm.NewShipClass.Name, newClassForm.NewShipClass)
                        ' Update the list
                        Call UpdateCustomShipClassList()
                        ' Save the list
                        Call CustomHQFClasses.SaveCustomShipClasses()
                    End If
                End If
            End Using
        End Sub

        Private Sub adtShipClasses_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtShipClasses.SelectionChanged
            ' Enable or disable the buttons based on the number of selected nodes
            If adtShipClasses.SelectedNodes.Count > 0 Then
                btnDelete.Enabled = True
                btnEdit.Enabled = True
            Else
                btnDelete.Enabled = False
                btnEdit.Enabled = False
            End If
        End Sub

        Private Sub ResetShipClasses(ByVal classList As List(Of String))
            ' Go through each custom ship and see if we need to reset the class
            For Each cShip As CustomShip In CustomHQFClasses.CustomShips.Values
                If ClassList.Contains(cShip.ShipClass) Then
                    ' We need to reset the ship class to the original hull type
                    cShip.ShipClass = Market.ShipClasses(cShip.BaseShipName)
                End If
            Next
            ' Update the ship list
            Call UpdateCustomShipList()
            ' Save the ship list
            Call CustomHQFClasses.SaveCustomShips()
        End Sub

#End Region

#Region "Ship Button Routines"

        Private Sub btnAddShip_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddShip.Click
            ' Add a new ShipEditorAttributes form
            Using newShipForm As New FrmShipEditorAttributes(Nothing)
                newShipForm.ShowDialog()
                ' Check result
                If newShipForm.DialogResult = DialogResult.OK Then
                    ' We should have a valid custom ship here
                    Dim newCustomShip As CustomShip = newShipForm.CustomShip
                    If CustomHQFClasses.CustomShips.ContainsKey(newCustomShip.Name) = False Then
                        CustomHQFClasses.CustomShips.Add(newCustomShip.Name, newCustomShip)
                        CustomHQFClasses.CustomShipIDs.Add(newCustomShip.ID, newCustomShip.Name)
                        ' Update the list
                        Call UpdateCustomShipList()
                        ' Save the list
                        Call CustomHQFClasses.SaveCustomShips()
                    End If
                End If
                ' Dispose of the form
            End Using
        End Sub

        Private Sub btnEditShip_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditShip.Click
            If adtShips.SelectedNodes.Count > 0 Then
                ' Get the existing class and set it in the form
                Dim newShip As CustomShip = CustomHQFClasses.CustomShips(adtShips.SelectedNodes(0).Text)
                Call EditShip(newShip)
            End If
        End Sub

        Private Sub btnDeleteShip_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteShip.Click
            If adtShips.SelectedNodes.Count > 0 Then
                Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to delete these custom ships?", "Confirm Delete Ships", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.Yes Then
                    ' Remove the items from teh custom list
                    For Each delNode As Node In adtShips.SelectedNodes
                        Dim cShip As CustomShip = CustomHQFClasses.CustomShips(delNode.Text)
                        CustomHQFClasses.CustomShips.Remove(cShip.Name)
                        CustomHQFClasses.CustomShipIDs.Remove(cShip.ID)
                    Next
                    ' Update the list
                    Call UpdateCustomShipList()
                    ' Save the list
                    Call CustomHQFClasses.SaveCustomShips()
                End If
            End If
        End Sub

        Private Sub btnClearShips_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearShips.Click
            Dim reply As DialogResult = MessageBox.Show("This will delete all of your custom ships. Are you sure you wish to perform this crazy act?", "Confirm Delete All Ships?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.Yes Then
                ' Delete all the ships
                CustomHQFClasses.CustomShips.Clear()
                CustomHQFClasses.CustomShipIDs.Clear()
                ' Update the list
                Call UpdateCustomShipList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShips()
            End If
        End Sub

        Private Sub adtShips_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtShips.NodeDoubleClick
            ' Get the existing class and set it in the form
            Dim newShip As CustomShip = CustomHQFClasses.CustomShips(e.Node.Text)
            Call EditShip(newShip)
        End Sub

        Private Sub EditShip(ByVal newShip As CustomShip)
            Dim oldName As String = NewShip.Name
            Dim oldID As Integer = NewShip.ID
            ' Add a new ShipClassEditor form
            Using newShipForm As New FrmShipEditorAttributes(newShip)
                newShipForm.ShowDialog()
                If newShipForm.DialogResult = DialogResult.OK Then
                    ' We should have a valid custom ship here
                    Dim newCustomShip As CustomShip = newShipForm.CustomShip
                    If newShipForm.CustomShip IsNot Nothing Then
                        ' Remove the old class
                        CustomHQFClasses.CustomShips.Remove(oldName)
                        CustomHQFClasses.CustomShipIDs.Remove(oldID)
                        ' Add the new class
                        CustomHQFClasses.CustomShips.Add(newCustomShip.Name, newCustomShip)
                        CustomHQFClasses.CustomShipIDs.Add(newCustomShip.ID, newCustomShip.Name)
                        ' Update the list
                        Call UpdateCustomShipList()
                        ' Save the list
                        Call CustomHQFClasses.SaveCustomShips()
                    End If
                End If
            End Using
        End Sub

        Private Sub adtShips_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtShips.SelectionChanged
            ' Enable or disable the buttons based on the number of selected nodes
            If adtShips.SelectedNodes.Count > 0 Then
                btnDeleteShip.Enabled = True
                btnEditShip.Enabled = True
            Else
                btnDeleteShip.Enabled = False
                btnEditShip.Enabled = False
            End If
        End Sub


#End Region

    End Class
End NameSpace