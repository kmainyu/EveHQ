Imports System.Windows.Forms

Public Class frmMetaVariations

    Dim cBaseModule As New ShipModule
    Public Property BaseModule() As ShipModule
        Get
            Return cBaseModule
        End Get
        Set(ByVal value As ShipModule)
            cBaseModule = value
            Call Me.GetVariations(cBaseModule)
            Me.Text = "HQF Meta Variations - " & value.Name
        End Set
    End Property

    Dim itemVariations(,) As String
    Dim compItems As New SortedList

    Private Sub GetVariations(ByVal startModule As ShipModule)
        Dim metaTypeID As String = startModule.ID

        Dim strSQL As String = ""
        strSQL &= "SELECT invMetaTypes.typeID, invMetaTypes.parentTypeID"
        strSQL &= " FROM invMetaTypes"
        strSQL &= " WHERE (((invMetaTypes.typeID)=" & metaTypeID & ") OR ((invMetaTypes.parentTypeID)=" & metaTypeID & "));"
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData.Tables(0).Rows.Count > 0 Then
            Dim metaParentID As String = eveData.Tables(0).Rows(0).Item("parentTypeID").ToString
            strSQL = ""
            strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
            strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
            strSQL &= " WHERE (((invMetaTypes.parentTypeID)=" & metaParentID & "));"
            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            Dim metaItemCount As Integer = eveData.Tables(0).Rows.Count
            ReDim itemVariations(2, metaItemCount)
            For item As Integer = 0 To metaItemCount - 1
                itemVariations(0, item + 1) = eveData.Tables(0).Rows(item).Item("invTypes_typeID").ToString
                itemVariations(1, item + 1) = eveData.Tables(0).Rows(item).Item("typeName").ToString.Trim
                itemVariations(2, item + 1) = eveData.Tables(0).Rows(item).Item("metaGroupName").ToString.Trim
            Next
            strSQL = "SELECT invTypes.typeID, invTypes.typeName FROM invTypes WHERE invTypes.typeID=" & metaParentID & ";"
            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            itemVariations(0, 0) = eveData.Tables(0).Rows(0).Item("typeID").ToString.Trim
            itemVariations(1, 0) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
            itemVariations(2, 0) = "Tech I"

            ' Generate Comparisons
            compItems.Clear()
            For item As Integer = 0 To metaItemCount
                compItems.Add(itemVariations(0, item), itemVariations(1, item))
            Next
            ' Get all the comparatives
            Call Me.GetComparatives()

        End If
    End Sub
    Private Sub GetComparatives()
        Dim ModuleList As New ArrayList

        For Each modID As String In compItems.Keys
            If ModuleLists.moduleList.ContainsKey(modID) = True Then
                Dim sModule As ShipModule = CType(ModuleLists.moduleList.Item(modID), ShipModule).Clone
                If chkApplySkills.Checked = True Then
                    Engine.ApplySkillEffectsToModule(sModule, True)
                End If
                ModuleList.Add(sModule)
            End If
        Next

        lvwComparisons.BeginUpdate()
        lvwComparisons.Items.Clear()
        ' Add columns
        lvwComparisons.Columns.Clear()
        lvwComparisons.Columns.Add("Item", 275)
        lvwComparisons.Columns.Add("Meta", 50, HorizontalAlignment.Right)
        Dim noColumn As New ArrayList
        Dim BaseModule As ShipModule = CType(ModuleList.Item(0), ShipModule)

        ' Check which columns are required
        For Each att As String In BaseModule.Attributes.Keys
            Dim colRequired As Boolean = False
            If Me.chkShowAllColumns.Checked = False Then
                For Each sMod As ShipModule In ModuleList
                    If CDbl(sMod.Attributes(att)) <> CDbl(BaseModule.Attributes(att)) Then
                        colRequired = True
                        Exit For
                    End If
                Next
            Else
                colRequired = True
            End If
            If colRequired = True Then
                Dim newCol As New ColumnHeader
                newCol.Text = CType(Attributes.AttributeList(att), Attribute).DisplayName
                newCol.Name = att
                newCol.Tag = CType(Attributes.AttributeList(att), Attribute).UnitName
                newCol.TextAlign = HorizontalAlignment.Right
                lvwComparisons.Columns.Add(newCol)
            End If
        Next
        ' Add the modules
        For Each sMod As ShipModule In ModuleList
            Dim newMod As New ListViewItem
            newMod.Text = sMod.Name
            newMod.Name = sMod.ID
            Dim mlItem As New ListViewItem.ListViewSubItem
            mlItem.Text = sMod.MetaLevel.ToString
            mlItem.Name = sMod.MetaLevel.ToString
            newMod.SubItems.Add(mlItem)
            ' Add column placeholders
            For c As Integer = 2 To lvwComparisons.Columns.Count
                newMod.SubItems.Add("")
            Next
            ' Now populate the list
            Dim i As Integer = 0
            For Each att As String In sMod.Attributes.Keys
                If lvwComparisons.Columns.ContainsKey(att) Then
                    i = lvwComparisons.Columns.IndexOfKey(att)
                    ' Adjust for TypeIDs
                    If lvwComparisons.Columns(att).Tag.ToString = "typeID" Then
                        newMod.SubItems(i).Text = EveHQ.Core.HQ.itemData(CStr(sMod.Attributes(att))).Name
                        newMod.SubItems(i).Name = newMod.SubItems(i).Text
                    Else
                        newMod.SubItems(i).Text = Format(sMod.Attributes(att), "#,###,##0.#") & " " & lvwComparisons.Columns(att).Tag.ToString
                        newMod.SubItems(i).Name = CStr(sMod.Attributes(att))
                    End If
                End If
            Next
            lvwComparisons.Items.Add(newMod)
        Next
        lvwComparisons.EndUpdate()

    End Sub

    Private Sub chkShowAllColumns_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowAllColumns.CheckedChanged
        Call Me.GetVariations(cBaseModule)
    End Sub

    Private Sub lvwComparisons_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwComparisons.ColumnClick
        If CInt(lvwComparisons.Tag) = e.Column Then
            Me.lvwComparisons.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Ascending)
            lvwComparisons.Tag = -1
        Else
            Me.lvwComparisons.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Descending)
            lvwComparisons.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwComparisons.Sort()
    End Sub

    Private Sub chkApplySkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplySkills.CheckedChanged
        Call Me.GetVariations(cBaseModule)
    End Sub
End Class