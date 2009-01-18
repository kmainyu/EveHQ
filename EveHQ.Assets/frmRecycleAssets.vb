Imports System.Text
Imports System.Windows.Forms
Imports DotNetLib.Windows.Forms

Public Class frmRecycleAssets

    Dim cAssetList As New SortedList
    Dim cAssetOwner As String = ""
    Dim cAssetLocation As String = ""
    Dim itemList As New SortedList
    Dim matList As New SortedList
    Dim BaseYield As Double = 0.5
    Dim NetYield As Double = 0
    Dim StationTake As Double = 0

    Public Property AssetList() As SortedList
        Get
            Return cAssetList
        End Get
        Set(ByVal value As SortedList)
            cAssetList = value
        End Set
    End Property
    Public Property AssetOwner() As String
        Get
            Return cAssetOwner
        End Get
        Set(ByVal value As String)
            cAssetOwner = value
        End Set
    End Property
    Public Property AssetLocation() As String
        Get
            Return cAssetLocation
        End Get
        Set(ByVal value As String)
            cAssetLocation = value
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

        itemList.Clear()
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

        ' Load the characters into the combobox
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next

        ' Get the location details
        If PlugInData.stations.ContainsKey(cAssetLocation) = True Then
            Dim aLocation As Station = CType(PlugInData.stations(cAssetLocation), Station)
            lblStation.Text = aLocation.stationName
            lblCorp.Text = aLocation.corpID.ToString
            If PlugInData.NPCCorps.ContainsKey(aLocation.corpID.ToString) = True Then
                lblCorp.Text = CStr(PlugInData.NPCCorps(aLocation.corpID.ToString))
                lblCorp.Tag = aLocation.corpID.ToString
                lblBaseYield.Text = FormatNumber(aLocation.refiningEff * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Else
                If PlugInData.NPCCorps.ContainsKey(aLocation.corpID.ToString) = True Then
                    lblCorp.Text = CStr(PlugInData.Corps(aLocation.corpID.ToString))
                    lblBaseYield.Text = FormatNumber(aLocation.refiningEff * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                Else
                    lblCorp.Text = "Unknown"
                    lblCorp.Tag = Nothing
                    lblBaseYield.Text = FormatNumber(50, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                End If
            End If
        Else
            lblStation.Text = "n/a"
            lblCorp.Text = "n/a"
            lblCorp.Tag = Nothing
            lblBaseYield.Text = FormatNumber(50, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        End If

        ' Set the pilot to the recycling one
        If cboPilots.Items.Contains(cAssetOwner) Then
            cboPilots.SelectedItem = cAssetOwner
        Else
            cboPilots.SelectedIndex = 0
        End If

    End Sub

    Private Sub RecalcRecycling()
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
        clvRecycle.Sort(0, SortOrder.Ascending, True)
        clvRecycle.EndUpdate()
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        Call Me.RecalcRecycling()
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
        NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
        lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        If lblCorp.Tag Is Nothing Then
            lblStandings.Text = FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            lblStandings.Text = FormatNumber(GetStanding(rPilot.Name, lblCorp.Tag.ToString), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        End If
    End Sub

    Private Function GetStanding(ByVal pilotName As String, ByVal corpID As String) As Double
        ' Check for the existence of the CorpHQ plug-in and try and get the standing data
        Dim PluginName As String = "CorpHQ"
        Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.PlugIns(PluginName), Core.PlugIn)
        If myPlugIn.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
            Dim PlugInData As New ArrayList
            PlugInData.Add(pilotName)
            PlugInData.Add(corpID)
            Return CDbl(myPlugIn.Instance.GetPlugInData(PlugInData))
        Else
            ' Plug-in is not loaded so best not try to access it!
            Dim msg As String = ""
            msg &= "The " & myPlugIn.MainMenuText & " Plug-in is not currently active. This is required to get accurate standings information." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Please load the plug-in in order to get standings info."
            MessageBox.Show(msg, "Error Starting " & myPlugIn.MainMenuText & " Plug-in!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Function

    Private Sub lblStandings_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStandings.TextChanged
        Dim take As Double = Math.Max(5 - (0.75 * CDbl(lblStandings.Text)), 0)
        lblStationTake.Text = FormatNumber(take, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
    End Sub
End Class