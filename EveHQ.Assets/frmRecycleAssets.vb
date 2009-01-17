Imports System.Text
Imports System.Windows.Forms
Imports DotNetLib.Windows.Forms

Public Class frmRecycleAssets

    Dim cAssetList As New SortedList

    Public Property AssetList() As SortedList
        Get
            Return cAssetList
        End Get
        Set(ByVal value As SortedList)
            cAssetList = value
        End Set
    End Property

    Private Sub frmRecycleAssets_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Form a string of the asset IDs in the AssetList Property
        Dim strAssets As New StringBuilder
        For Each asset As String In cAssetList.Keys
            strAssets.Append(", " & asset)
        Next
        strAssets.Remove(0, 2)

        ' Fetch the data from the database
        Dim strSQL As String = "SELECT typeActivityMaterials.typeID AS itemTypeID, invTypes.typeID AS materialTypeID, invTypes.typeName AS materialTypeName, typeActivityMaterials.quantity AS materialQuantity"
        strSQL &= " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN typeActivityMaterials ON invTypes.typeID = typeActivityMaterials.requiredTypeID) ON invCategories.categoryID = invGroups.categoryID"
        strSQL &= " WHERE (typeActivityMaterials.typeID IN (" & strAssets.ToString & ") AND typeActivityMaterials.activityID IN (6,9)) ORDER BY invCategories.categoryName, invGroups.groupName"
        Dim mDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        ' Add the data into a collection for parsing
        Dim itemList As New SortedList
        Dim matList As New SortedList
        With mDataSet.Tables(0)
            For row As Integer = 0 To .Rows.Count - 1
                If itemList.ContainsKey(.Rows(row).Item("itemTypeID").ToString.Trim) = True Then
                    matList = CType(itemList(.Rows(row).Item("itemTypeID").ToString.Trim), Collections.SortedList)
                Else
                    matList = New SortedList
                    itemList.Add(.Rows(row).Item("itemTypeID").ToString.Trim, matList)
                End If
                matList.Add(.Rows(row).Item("materialTypeName").ToString.Trim, CLng(.Rows(row).Item("materialQuantity").ToString.Trim))
            Next
        End With

        ' Create the main list
        clvRecycle.BeginUpdate()
        clvRecycle.Items.Clear()
        Dim price As Double = 0
        Dim quant As Double = 0
        Dim recycleTotal As Double = 0
        Dim newCLVItem As New ContainerListViewItem
        Dim newCLVSubItem As New ContainerListViewItem
        Dim itemInfo As New ItemData
        Dim batches As Integer
        For Each asset As String In cAssetList.Keys
            itemInfo = CType(PlugInData.Items(asset), ItemData)
            matList = CType(itemList(asset), Collections.SortedList)
            newCLVItem = New ContainerListViewItem
            newCLVItem.Text = itemInfo.Name
            clvRecycle.Items.Add(newCLVItem)
            price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(asset), 2)
            batches = CInt(Int(CLng(cAssetList(itemInfo.ID.ToString)) / itemInfo.PortionSize))
            quant = CDbl(cAssetList(asset))
            newCLVItem.SubItems(1).Text = FormatNumber(itemInfo.MetaLevel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(2).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(3).Text = FormatNumber(batches, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(5).Text = FormatNumber(price * quant, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            recycleTotal = 0
            If matList IsNot Nothing Then ' i.e. it can be refined
                For Each mat As String In matList.Keys
                    price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(mat).ToString), 2)
                    quant = CDbl(matList(mat)) * batches
                    newCLVSubItem = New ContainerListViewItem
                    newCLVSubItem.Text = mat
                    newCLVItem.Items.Add(newCLVSubItem)
                    newCLVSubItem.SubItems(2).Text = CStr(quant)
                    newCLVSubItem.SubItems(3).Text = CStr(quant)
                    newCLVSubItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(5).Text = FormatNumber(price * quant, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(6).Text = newCLVSubItem.SubItems(4).Text
                    recycleTotal += price * quant
                Next
            End If
            newCLVItem.SubItems(6).Text = FormatNumber(recycleTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            If CDbl(newCLVItem.SubItems(6).Text) > CDbl(newCLVItem.SubItems(5).Text) Then
                newCLVItem.BackColor = Drawing.Color.LightGreen
            End If
        Next
        clvRecycle.Sort(0, True, True)
        clvRecycle.EndUpdate()
    End Sub

End Class