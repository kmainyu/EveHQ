Imports System.Windows.Forms

Public Class frmSelectItem

    Private cItem As String

    Public ReadOnly Property Item() As String
        Get
            Return cItem
        End Get
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        If cboItems.SelectedItem IsNot Nothing Then
            cItem = cboItems.SelectedItem.ToString
        End If
        Me.Close()
    End Sub

    Private Sub frmSelectItem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Fetch the data from the database
        Dim strSQL As String = "SELECT DISTINCT invTypes.typeName AS itemName"
        strSQL &= " FROM invTypes INNER JOIN invTypeMaterials ON invTypes.typeID = invTypeMaterials.typeID"
        strSQL &= " ORDER BY invTypes.typeName;"

        Dim mDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Dim typeName As String = ""
        cboItems.Items.Clear()
        cboItems.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboItems.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboItems.BeginUpdate()
        For Each ItemRow As DataRow In mDataSet.Tables(0).Rows
            cboItems.AutoCompleteCustomSource.Add(CStr(ItemRow.Item("itemName")))
            cboItems.Items.Add(CStr(ItemRow.Item("itemName")))
        Next
        cboItems.EndUpdate()
    End Sub
End Class