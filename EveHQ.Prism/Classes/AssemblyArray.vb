Imports System.Windows.Forms

Public Class AssemblyArray
    Public ID As String
    Public Name As String
    Public TimeMultiplier As Double
    Public MaterialMultiplier As Double
    Public AllowableGroups As New ArrayList
    Public AllowableCategories As New ArrayList

    Public Shared Function LoadAssemblyArrayData() As Boolean
        Dim arraySQL As String = "SELECT * FROM ramAssemblyLineTypes WHERE activityID=1 AND (baseTimeMultiplier<>1 OR baseMaterialMultiplier<>1);"
        Dim groupSQL As String = "SELECT * FROM ramAssemblyLineTypeDetailPerGroup;"
        Dim catSQL As String = "SELECT * FROM ramAssemblyLineTypeDetailPerCategory;"
        Dim arrayDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(arraySQL)
        Dim groupDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(groupSQL)
        Dim catDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(catSQL)
        If arrayDataSet IsNot Nothing Then
            If arrayDataSet.Tables(0).Rows.Count > 0 Then
                ' Reset the list
                PlugInData.AssemblyArrays.Clear()
                ' Populate the main data
                For Each AssArray As DataRow In arrayDataSet.Tables(0).Rows
                    Dim newArray As New AssemblyArray
                    newArray.ID = CStr(AssArray.Item("assemblyLineTypeID"))
                    newArray.Name = CStr(AssArray.Item("assemblyLineTypeName"))
                    newArray.MaterialMultiplier = CDbl(AssArray.Item("baseMaterialMultiplier"))
                    newArray.TimeMultiplier = CDbl(AssArray.Item("baseTimeMultiplier"))
                    PlugInData.AssemblyArrays.Add(newArray.Name.ToString, newArray)
                    Dim GroupRows() As DataRow = groupDataSet.Tables(0).Select("assemblyLineTypeID=" & newArray.ID)
                    For Each group As DataRow In GroupRows
                        newArray.AllowableGroups.Add(CInt(group.Item("groupID")))
                    Next
                    Dim CatRows() As DataRow = catDataSet.Tables(0).Select("assemblyLineTypeID=" & newArray.ID)
                    For Each cat As DataRow In CatRows
                        newArray.AllowableCategories.Add(CInt(cat.Item("categoryID")))
                    Next
                Next
                Return True
            Else
                MessageBox.Show("Assembly Line data returned a null dataset.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Else
            MessageBox.Show("Assembly Line data returned no valid rows.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If


    End Function
End Class
