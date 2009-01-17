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
            strAssets.Append(", " & EveHQ.Core.HQ.itemList(asset).ToString)
        Next
        strAssets.Remove(0, 2)

        ' Fetch the data from the database
        Dim strSQL As String = "SELECT typeActivityMaterials.typeID AS itemTypeID, invTypes1.typeName AS itemTypeName, invTypes.typeID AS materialTypeID, invTypes.typeName AS materialTypeName, typeActivityMaterials.quantity AS materialQuantity, invTypes1.portionSize AS itemPortion"
        strSQL &= " FROM invTypes AS invTypes1 INNER JOIN (invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN typeActivityMaterials ON invTypes.typeID=typeActivityMaterials.requiredTypeID) ON invCategories.categoryID=invGroups.categoryID) ON invTypes1.typeID=typeActivityMaterials.typeID"
        strSQL &= " WHERE (typeActivityMaterials.typeID IN (" & strAssets.ToString & ") AND typeActivityMaterials.activityID IN (6,9)) ORDER BY invCategories.categoryName, invGroups.groupName"
        Dim mDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        MessageBox.Show(CStr(mDataSet.Tables(0).Rows.Count))

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
                    ' Reset the quantity to the portion size
                    If CInt(.Rows(row).Item("itemPortion")) > 1 Then
                        cAssetList(.Rows(row).Item("itemTypeName").ToString.Trim) = Int(CLng(cAssetList(.Rows(row).Item("itemTypeName").ToString.Trim)) / CInt(.Rows(row).Item("itemPortion")))
                    End If
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
        For Each asset As String In cAssetList.Keys
            matList = CType(itemList(EveHQ.Core.HQ.itemList(asset)), Collections.SortedList)
            newCLVItem = New ContainerListViewItem
            newCLVItem.Text = asset
            clvRecycle.Items.Add(newCLVItem)
            newCLVItem.SubItems(1).Text = CStr(cAssetList(asset))
            newCLVItem.SubItems(2).Text = FormatNumber(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(asset).ToString), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            recycleTotal = 0
            For Each mat As String In matList.Keys
                price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(mat).ToString), 2)
                quant = CDbl(matList(mat))
                Dim newCLVSubItem As New ContainerListViewItem
                newCLVSubItem.Text = mat
                newCLVItem.Items.Add(newCLVSubItem)
                newCLVSubItem.SubItems(1).Text = CStr(quant)
                newCLVSubItem.SubItems(2).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVSubItem.SubItems(3).Text = FormatNumber(price * quant, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                recycleTotal += price * quant
            Next
            newCLVItem.SubItems(3).Text = FormatNumber(recycleTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            If CDbl(newCLVItem.SubItems(2).Text) > CDbl(newCLVItem.SubItems(3).Text) Then
                newCLVItem.BackColor = Drawing.Color.Green
            End If
        Next
        clvRecycle.EndUpdate()
    End Sub

End Class