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
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmShipEditor

    Dim ShipRaces As New SortedList(Of String, String)

#Region "Form Loading Routines"

    Private Sub frmShipEditor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Implement the data before we close the form
        Call CustomHQFClasses.ImplementCustomShips()
    End Sub

    Private Sub frmShipEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Note: Loading of the custom ship classes and ships is done on loading HQF to allow them to be implemented in the main fitter
        Call Me.LoadShipInformation()
        Call Me.LoadCustomShipClasses()
        Call Me.LoadCustomShips()
    End Sub

    Private Sub LoadShipInformation()
        Dim sr As New StreamReader(Path.Combine(HQF.Settings.HQFCacheFolder, "ShipGroups.bin"))
        Dim ShipGroups As String = sr.ReadToEnd
        Dim PathLines() As String = ShipGroups.Split(ControlChars.CrLf.ToCharArray)
        Dim data() As String
        sr.Close()
        ShipRaces.Clear()
        Market.ShipClasses.Clear()
        For Each pathline As String In PathLines
            If pathline.Trim <> "" Then
                data = pathline.Split("\".ToCharArray)
                ' Check if the last item is a ship type
                If HQF.ShipLists.shipListKeyName.ContainsKey(data(data.Length - 1)) = True Then
                    If ShipRaces.ContainsKey(data(data.Length - 1)) = False Then
                        ShipRaces.Add(data(data.Length - 1), data(data.Length - 2))
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
        Call Me.UpdateCustomShipClassList()
    End Sub

    Private Sub UpdateCustomShipClassList()
        adtShipClasses.BeginUpdate()
        adtShipClasses.Nodes.Clear()
        For Each NewClass As String In CustomHQFClasses.CustomShipClasses.Keys
            Dim newNode As New Node
            newNode.Text = NewClass
            adtShipClasses.Nodes.Add(newNode)
            sttInfo.SetSuperTooltip(newNode, New SuperTooltipInfo(newNode.Text, "", CustomHQFClasses.CustomShipClasses(NewClass).Description, My.Resources.imgInfo2, Nothing, eTooltipColor.Yellow))
        Next
        adtShipClasses.EndUpdate()
    End Sub

    Private Sub LoadCustomShips()
        ' Load the custom ships from storage
        CustomHQFClasses.LoadCustomShips()
        ' Display the ships
        Call Me.UpdateCustomShipList()
    End Sub

    Private Sub UpdateCustomShipList()
        adtShips.BeginUpdate()
        adtShips.Nodes.Clear()
        For Each NewShip As CustomShip In CustomHQFClasses.CustomShips.Values
            Dim newNode As New Node
            newNode.Text = NewShip.Name
            Dim HullTypeCell As New Cell(NewShip.BaseShipName)
            Dim ShipClassCell As New Cell(NewShip.ShipClass)
            newNode.Cells.Add(HullTypeCell)
            newNode.Cells.Add(ShipClassCell)
            adtShips.Nodes.Add(newNode)
            sttInfo.SetSuperTooltip(newNode, New SuperTooltipInfo(newNode.Text, "", NewShip.ShipData.Description, My.Resources.imgInfo2, Nothing, eTooltipColor.Yellow))
        Next
        adtShips.EndUpdate()
    End Sub

#End Region

#Region "Ship Class Button Routines"

    Private Sub btnAddClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddClass.Click
        ' Add a new ShipClassEditor form
        Dim NewClassForm As New frmShipClassEditor
        NewClassForm.ShowDialog()
        If NewClassForm.DialogResult = Windows.Forms.DialogResult.OK Then
            If NewClassForm.NewShipClass IsNot Nothing Then
                ' User clicked Accept and the NewShipClass property is not blank, so add the new class
                CustomHQFClasses.CustomShipClasses.Add(NewClassForm.NewShipClass.Name, NewClassForm.NewShipClass)
                ' Update the list
                Call Me.UpdateCustomShipClassList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShipClasses()
            End If
        End If
        ' Dispose of the form
        NewClassForm.Dispose()
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim reply As DialogResult = MessageBox.Show("This will reset the class type of all your custom ships. Are you sure you wish to clear all your custom classes?", "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            ' Reset any custom ship classes to the basic hull type
            Dim ClassList As New List(Of String)
            For Each DelClass As String In CustomHQFClasses.CustomShipClasses.Keys
                ClassList.Add(DelClass)
            Next
            Call Me.ResetShipClasses(ClassList)
            ' Delete all the classes
            CustomHQFClasses.CustomShipClasses.Clear()
            ' Update the list
            Call Me.UpdateCustomShipClassList()
            ' Save the list
            Call CustomHQFClasses.SaveCustomShipClasses()
        End If
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        If adtShipClasses.SelectedNodes.Count > 0 Then
            ' Get the existing class and set it in the form
            Dim NewShipClass As CustomShipClass = CustomHQFClasses.CustomShipClasses(adtShipClasses.SelectedNodes(0).Text)
            Call Me.EditShipClass(NewShipClass)
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If adtShipClasses.SelectedNodes.Count > 0 Then
            Dim reply As DialogResult = MessageBox.Show("This will reset the class type of the custom ships with the selected classes. Are you sure you wish to delete these your custom classes?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                ' Remove the items from teh custom list
                Dim ClassList As New List(Of String)
                For Each DelNode As Node In adtShipClasses.SelectedNodes
                    ClassList.Add(DelNode.Text)
                    CustomHQFClasses.CustomShipClasses.Remove(DelNode.Text)
                Next
                ' Reset any custom ship classes to the basic hull type
                Call Me.ResetShipClasses(ClassList)
                ' Update the list
                Call Me.UpdateCustomShipClassList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShipClasses()
            End If
        End If
    End Sub

    Private Sub adtShipClasses_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtShipClasses.NodeDoubleClick
        ' Get the existing class and set it in the form
        Dim NewShipClass As CustomShipClass = CustomHQFClasses.CustomShipClasses(e.Node.Text)
        Call Me.EditShipClass(NewShipClass)
    End Sub

    Private Sub EditShipClass(ByVal NewShipClass As CustomShipClass)
        ' Add a new ShipClassEditor form
        Dim NewClassForm As New frmShipClassEditor
        NewClassForm.NewShipClass = NewShipClass
        NewClassForm.ShowDialog()
        If NewClassForm.DialogResult = Windows.Forms.DialogResult.OK Then
            If NewClassForm.NewShipClass IsNot Nothing Then
                ' Remove the old class
                CustomHQFClasses.CustomShipClasses.Remove(adtShipClasses.SelectedNodes(0).Text)
                ' Add the new class
                CustomHQFClasses.CustomShipClasses.Add(NewClassForm.NewShipClass.Name, NewClassForm.NewShipClass)
                ' Update the list
                Call Me.UpdateCustomShipClassList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShipClasses()
            End If
        End If
    End Sub

    Private Sub adtShipClasses_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtShipClasses.SelectionChanged
        ' Enable or disable the buttons based on the number of selected nodes
        If adtShipClasses.SelectedNodes.Count > 0 Then
            btnDelete.Enabled = True
            btnEdit.Enabled = True
        Else
            btnDelete.Enabled = False
            btnEdit.Enabled = False
        End If
    End Sub

    Private Sub ResetShipClasses(ByVal ClassList As List(Of String))
        ' Go through each custom ship and see if we need to reset the class
        For Each cShip As CustomShip In CustomHQFClasses.CustomShips.Values
            If ClassList.Contains(cShip.ShipClass) Then
                ' We need to reset the ship class to the original hull type
                cShip.ShipClass = Market.ShipClasses(cShip.BaseShipName)
            End If
        Next
        ' Update the ship list
        Call Me.UpdateCustomShipList()
        ' Save the ship list
        Call CustomHQFClasses.SaveCustomShips()
    End Sub

#End Region

#Region "Ship Button Routines"

    Private Sub btnAddShip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddShip.Click
        ' Add a new ShipEditorAttributes form
        Dim NewShipForm As New frmShipEditorAttributes(Nothing)
        NewShipForm.ShowDialog()
        ' Check result
        If NewShipForm.DialogResult = Windows.Forms.DialogResult.OK Then
            ' We should have a valid custom ship here
            Dim NewCustomShip As CustomShip = NewShipForm.CustomShip
            If CustomHQFClasses.CustomShips.ContainsKey(NewCustomShip.Name) = False Then
                CustomHQFClasses.CustomShips.Add(NewCustomShip.Name, NewCustomShip)
                CustomHQFClasses.CustomShipIDs.Add(NewCustomShip.ID, NewCustomShip.Name)
                ' Update the list
                Call Me.UpdateCustomShipList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShips()
            End If
        End If
        ' Dispose of the form
        NewShipForm.Dispose()
    End Sub

    Private Sub btnEditShip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditShip.Click
        If adtShips.SelectedNodes.Count > 0 Then
            ' Get the existing class and set it in the form
            Dim NewShip As CustomShip = CustomHQFClasses.CustomShips(adtShips.SelectedNodes(0).Text)
            Call Me.EditShip(NewShip)
        End If
    End Sub

    Private Sub btnDeleteShip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteShip.Click
        If adtShips.SelectedNodes.Count > 0 Then
            Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to delete these custom ships?", "Confirm Delete Ships", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                ' Remove the items from teh custom list
                For Each DelNode As Node In adtShips.SelectedNodes
                    Dim cShip As CustomShip = CustomHQFClasses.CustomShips(DelNode.Text)
                    CustomHQFClasses.CustomShips.Remove(cShip.Name)
                    CustomHQFClasses.CustomShipIDs.Remove(cShip.ID)
                Next
                ' Update the list
                Call Me.UpdateCustomShipList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShips()
            End If
        End If
    End Sub

    Private Sub btnClearShips_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearShips.Click
        Dim reply As DialogResult = MessageBox.Show("This will delete all of your custom ships. Are you sure you wish to perform this crazy act?", "Confirm Delete All Ships?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            ' Delete all the ships
            CustomHQFClasses.CustomShips.Clear()
            CustomHQFClasses.CustomShipIDs.Clear()
            ' Update the list
            Call Me.UpdateCustomShipList()
            ' Save the list
            Call CustomHQFClasses.SaveCustomShips()
        End If
    End Sub

    Private Sub adtShips_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtShips.NodeDoubleClick
        ' Get the existing class and set it in the form
        Dim NewShip As CustomShip = CustomHQFClasses.CustomShips(e.Node.Text)
        Call Me.EditShip(NewShip)
    End Sub

    Private Sub EditShip(ByVal NewShip As CustomShip)
        Dim oldName As String = NewShip.Name
        Dim oldID As String = NewShip.ID
        ' Add a new ShipClassEditor form
        Dim NewShipForm As New frmShipEditorAttributes(NewShip)
        NewShipForm.ShowDialog()
        If NewShipForm.DialogResult = Windows.Forms.DialogResult.OK Then
            ' We should have a valid custom ship here
            Dim NewCustomShip As CustomShip = NewShipForm.CustomShip
            If NewShipForm.CustomShip IsNot Nothing Then
                ' Remove the old class
                CustomHQFClasses.CustomShips.Remove(oldName)
                CustomHQFClasses.CustomShipIDs.Remove(oldID)
                ' Add the new class
                CustomHQFClasses.CustomShips.Add(NewCustomShip.Name, NewCustomShip)
                CustomHQFClasses.CustomShipIDs.Add(NewCustomShip.ID, NewCustomShip.Name)
                ' Update the list
                Call Me.UpdateCustomShipList()
                ' Save the list
                Call CustomHQFClasses.SaveCustomShips()
            End If
        End If
    End Sub

    Private Sub adtShips_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtShips.SelectionChanged
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