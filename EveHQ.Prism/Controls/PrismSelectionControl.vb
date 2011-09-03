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
Imports System.Text

Public Class PrismSelectionControl

    Dim startup As Boolean = True
    Dim Host As DevComponents.DotNetBar.Controls.TextBoxDropDown
    Dim cListType As PrismSelectionType
	Dim cAllowMultipleSelections As Boolean = True
	Dim cExtraData As String = ""
    Dim UpdateAll As Boolean = False
    Public Event SelectionChanged()

    Public Sub New(ByVal SelectionType As PrismSelectionType, ByVal AllowMultiSelection As Boolean, ByVal HostingControl As DevComponents.DotNetBar.Controls.TextBoxDropDown)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the parameter data
        Me.Host = HostingControl
        Me.cListType = SelectionType
        Me.cAllowMultipleSelections = AllowMultiSelection
        Me.cExtraData = ""
        btnAll.Enabled = AllowMultiSelection
        startup = False
    End Sub

    Public Sub New(ByVal SelectionType As PrismSelectionType, ByVal AllowMultiSelection As Boolean, ByVal HostingControl As DevComponents.DotNetBar.Controls.TextBoxDropDown, ByVal AdditionalData As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the parameter data
        Me.Host = HostingControl
        Me.cListType = SelectionType
        Me.cAllowMultipleSelections = AllowMultiSelection
        Me.cExtraData = AdditionalData
        btnAll.Enabled = AllowMultiSelection
        startup = False
    End Sub

    Public Property ListType As PrismSelectionType
        Get
            Return cListType
        End Get
        Set(ByVal value As PrismSelectionType)
            Me.cListType = value
        End Set
    End Property

    Public Property AllowMultipleSelections As Boolean
        Get
            Return cAllowMultipleSelections
        End Get
        Set(ByVal value As Boolean)
            cAllowMultipleSelections = value
            btnAll.Enabled = value
        End Set
    End Property

    Public Property ExtraData As String
        Get
            Return cExtraData
        End Get
        Set(ByVal value As String)
            cExtraData = value
        End Set
    End Property

    Private WriteOnly Property SelectionHasChanged As Boolean
        Set(ByVal value As Boolean)
            RaiseEvent SelectionChanged()
        End Set
    End Property

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ' Close the host control
        Host.CloseDropDown()
    End Sub

    Private Sub btnAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll.Click
        UpdateAll = True
        For Each Owner As ListViewItem In lvwItems.Items
            Owner.Checked = True
        Next
        UpdateAll = False
        UpdateOwnerList()
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        UpdateAll = True
        For Each Owner As ListViewItem In lvwItems.Items
            Owner.Checked = False
        Next
        UpdateAll = False
        UpdateOwnerList()
    End Sub

    Private Sub lvwOwners_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwItems.ItemChecked
        If startup = False Then
            If Me.cAllowMultipleSelections = False Then
                If lvwItems.CheckedItems.Count > 1 Then
                    ' Clear all checked items - acts like radio buttons
                    For Each item As ListViewItem In lvwItems.CheckedItems
                        item.Checked = False
                    Next
                    e.Item.Checked = True
                End If
                UpdateOwnerList()
            Else
                If UpdateAll = False Then
                    UpdateOwnerList()
                End If
            End If
        End If
    End Sub

    Public Sub UpdateOwnerList()
        If Me.Host IsNot Nothing Then
            If Me.cAllowMultipleSelections = True And lvwItems.CheckedItems.Count = lvwItems.Items.Count Then
                Me.Host.Text = "<All>"
            Else
                Dim OwnerList As New StringBuilder
                For Each item As ListViewItem In lvwItems.CheckedItems
                    OwnerList.Append(", " & item.Text)
                Next
                If OwnerList.Length > 2 Then
                    OwnerList.Remove(0, 2)
                End If
                Me.Host.Text = OwnerList.ToString
            End If
            SelectionHasChanged = True
        End If
    End Sub

    Public Sub UpdateList()

        ' Prepare list
        Dim ItemData As New DataSet
        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()

        ' Do stuff depending on the type of data we want
        Select Case Me.cListType

            Case PrismSelectionType.AllOwners
                For Each Owner As PrismOwner In PlugInData.PrismOwners.Values
                    If lvwItems.Items.ContainsKey(Owner.Name) = False Then
                        Dim NewItem As New ListViewItem(Owner.Name)
                        NewItem.Name = Owner.Name
                        lvwItems.Items.Add(NewItem)
                    End If
                Next
                CheckDefaultCharacter()

            Case PrismSelectionType.AllRefTypes
                For Each Ref As String In PlugInData.RefTypes.Keys
                    Dim NewItem As New ListViewItem(PlugInData.RefTypes(Ref))
                    NewItem.Name = Ref
                    lvwItems.Items.Add(NewItem)
                Next

            Case PrismSelectionType.JournalOwnersAll
                Dim strSQL As String = "SELECT DISTINCT charName from walletJournal;"
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        Dim NewItem As New ListViewItem(DR.Item(0).ToString.Trim)
                        NewItem.Name = DR.Item(0).ToString.Trim
                        lvwItems.Items.Add(NewItem)
                    Next
                End If
                CheckDefaultCharacter()

            Case PrismSelectionType.JournalOwnersChars
                Dim strSQL As String = "SELECT DISTINCT charName from walletJournal;"
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        If PlugInData.CorpList.ContainsKey(DR.Item(0).ToString.Trim) = False Then
                            Dim NewItem As New ListViewItem(DR.Item(0).ToString.Trim)
                            NewItem.Name = DR.Item(0).ToString.Trim
                            lvwItems.Items.Add(NewItem)
                        End If
                    Next
                End If
                CheckDefaultCharacter()

            Case PrismSelectionType.JournalOwnersCorps
                Dim strSQL As String = "SELECT DISTINCT charName from walletJournal;"
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        If PlugInData.CorpList.ContainsKey(DR.Item(0).ToString.Trim) = True Then
                            Dim NewItem As New ListViewItem(DR.Item(0).ToString.Trim)
                            NewItem.Name = DR.Item(0).ToString.Trim
                            lvwItems.Items.Add(NewItem)
                        End If
                    Next
                End If

            Case PrismSelectionType.JournalRefTypes
                Dim strSQL As String = "SELECT DISTINCT refTypeID from walletJournal;"
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        Dim NewItem As New ListViewItem(PlugInData.RefTypes(DR.Item(0).ToString))
                        NewItem.Name = DR.Item(0).ToString
                        lvwItems.Items.Add(NewItem)
                    Next
                End If

            Case PrismSelectionType.TransactionOwnersAll
                Dim strSQL As String = "SELECT DISTINCT charName from walletTransactions;"
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        Dim NewItem As New ListViewItem(DR.Item(0).ToString.Trim)
                        NewItem.Name = DR.Item(0).ToString.Trim
                        lvwItems.Items.Add(NewItem)
                    Next
                End If
                CheckDefaultCharacter()

            Case PrismSelectionType.TransactionOwnersChars
                Dim strSQL As String = "SELECT DISTINCT charName from walletTransactions;"
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        If PlugInData.CorpList.ContainsKey(DR.Item(0).ToString.Trim) = False Then
                            Dim NewItem As New ListViewItem(DR.Item(0).ToString.Trim)
                            NewItem.Name = DR.Item(0).ToString.Trim
                            lvwItems.Items.Add(NewItem)
                        End If
                    Next
                End If
                CheckDefaultCharacter()

            Case PrismSelectionType.TransactionOwnersCorps
                Dim strSQL As String = "SELECT DISTINCT charName from walletTransactions;"
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        If PlugInData.CorpList.ContainsKey(DR.Item(0).ToString.Trim) = True Then
                            Dim NewItem As New ListViewItem(DR.Item(0).ToString.Trim)
                            NewItem.Name = DR.Item(0).ToString.Trim
                            lvwItems.Items.Add(NewItem)
                        End If
                    Next
                End If

            Case PrismSelectionType.TransactionItems
                Dim strSQL As String = ""
                If cExtraData = "" Then
                    strSQL = "SELECT DISTINCT typeName from walletTransactions;"
                Else
                    strSQL = "SELECT DISTINCT typeName from walletTransactions WHERE charName='" & cExtraData.Replace("'", "''") & "';"
                End If
                ItemData = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If ItemData IsNot Nothing Then
                    For Each DR As DataRow In ItemData.Tables(0).Rows
                        Dim NewItem As New ListViewItem(DR.Item(0).ToString.Trim)
                        NewItem.Name = DR.Item(0).ToString.Trim
                        lvwItems.Items.Add(NewItem)
                    Next
                End If

        End Select

        ' Finalise list
        lvwItems.Sort()
        lvwItems.EndUpdate()

        ' Dispose of the dataset
        If ItemData IsNot Nothing Then
            ItemData.Dispose()
        End If

    End Sub

    Private Sub PrismSelectionControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        startup = True
        Call Me.UpdateList()
        startup = False
    End Sub

    Private Sub CheckDefaultCharacter()
        If Settings.PrismSettings.DefaultCharacter <> "" Then
            If lvwItems.Items.Item(Settings.PrismSettings.DefaultCharacter) IsNot Nothing Then
                lvwItems.Items.Item(Settings.PrismSettings.DefaultCharacter).Checked = True
            End If
            UpdateOwnerList()
        End If
    End Sub
End Class

Public Enum PrismSelectionType
    AllOwners = 0
    AllRefTypes = 1
    JournalOwnersAll = 2
    JournalRefTypes = 3
    TransactionOwnersAll = 4
    TransactionItems = 5
    JournalOwnersChars = 6
    JournalOwnersCorps = 7
    TransactionOwnersChars = 8
    TransactionOwnersCorps = 9
End Enum
