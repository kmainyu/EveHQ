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

Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports System.Windows.Forms
Imports System.Xml
Imports System.Text
Imports System.IO

Public Class PrismAssetsControl

    Dim HQFShip As New ArrayList
    Dim assetList As New SortedList(Of String, AssetItem)
    Dim tempAssetList As New ArrayList
    Dim totalAssetValue As Double = 0
    Dim totalAssetCount As Long = 0
    Dim filters As New ArrayList
    Dim catFilters As New ArrayList
    Dim groupFilters As New ArrayList
    Dim searchText As String = ""
    Dim divisions As New SortedList
    Dim walletDivisions As New SortedList
    Dim charWallets As New SortedList(Of String, Double)
    Dim corpWallets As New SortedList
    Dim corpWalletDivisions As New SortedList(Of String, Double)
    Dim totalAssetBatch As Long = 0
    Dim assetCorpMode As Boolean = False
    Dim IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Dim cRecyclingAssetList As New SortedList
    Dim cRecyclingAssetLocation As New Node
    Dim NumberOfActiveColumns As Integer = 0
    Dim AssetColumn As New SortedList(Of String, Integer)

    Public ReadOnly Property RecyclingAssetList As SortedList
        Get
            Return cRecyclingAssetList
        End Get
    End Property

    Public ReadOnly Property RecyclingAssetLocation As Node
        Get
            Return cRecyclingAssetLocation
        End Get
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call Me.LoadFilterGroups()

        ' Set the value of the min system value text box
        txtMinSystemValue.Text = CDbl(0).ToString("N2")

    End Sub

    Private Sub btnRefreshAssets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshAssets.Click
        Dim minValue As Double = 0
        If chkMinSystemValue.Checked = True Then
            ' Check for null value and reset
            If txtMinSystemValue.Text = "" Then
                txtMinSystemValue.Text = minValue.ToString("F2")
            End If
            If Double.TryParse(txtMinSystemValue.Text, minValue) = True Then
                Call Me.RefreshAssets()
            Else
                MessageBox.Show("Minimum System Value is not a valid number. Please try again!", "Error in Minimum Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            Call Me.RefreshAssets()
        End If
    End Sub

    Private Sub RefreshAssets()
        If PSCAssetOwners.ItemList.CheckedItems.Count > 0 Then
            ' Set search variables
            searchText = txtSearch.Text
            ' Populate the assets list
            Call Me.PopulateAssets()
        Else
            MessageBox.Show("Please select an Asset Owner before continuing!", "Please Select Asset Owner", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call Me.RefreshAssets()
        End If
    End Sub
    Private Sub txtMinSystemValue_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMinSystemValue.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim minValue As Double
            If Double.TryParse(txtMinSystemValue.Text, minValue) = True Then
                Call Me.RefreshAssets()
            Else
                MessageBox.Show("Minimum System Value is not a valid number. Please try again!", "Error in Minimum Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

#Region "Asset Column Routines"

    ''' <summary>
    ''' Updates the column headers of the main Assets view
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateAssetSlotColumns()
        ' Clear the column position list
        AssetColumn.Clear()
        ' Clear the columns
        adtAssets.Columns.Clear()
        ' Add the module name column
        Dim MainCol As New DevComponents.AdvTree.ColumnHeader("Asset Name/Location")
        MainCol.Name = "AssetName"
        MainCol.DisplayIndex = 1
        MainCol.Width.Absolute = Settings.PrismSettings.SlotNameWidth
        MainCol.Width.AutoSizeMinHeader = True
        adtAssets.Columns.Add(MainCol)
        AssetColumn.Add(MainCol.Name, 0)
        ' Iterate through the user selected columns and add them in
        Dim ColumnDisplayIDX As Integer = 1
        For Each UserCol As UserSlotColumn In Settings.PrismSettings.UserSlotColumns
            ColumnDisplayIDX += 1
            Dim SubCol As New DevComponents.AdvTree.ColumnHeader(UserCol.Description)
            SubCol.Name = UserCol.Name
            SubCol.DisplayIndex = ColumnDisplayIDX
            SubCol.Width.Absolute = UserCol.Width
            SubCol.Width.AutoSizeMinHeader = True
            SubCol.Visible = UserCol.Active
            adtAssets.Columns.Add(SubCol)
            AssetColumn.Add(SubCol.Name, ColumnDisplayIDX - 1)
        Next
        NumberOfActiveColumns = ColumnDisplayIDX - 1
    End Sub
    ''' <summary>
    ''' Creates the individual cells of a node based on the Asset data and user columns required
    ''' </summary>
    ''' <param name="AssetData">The data to populate the cell information from</param>
    ''' <param name="AssetNode">The particular node to update</param>
    ''' <remarks></remarks>
    Private Sub UpdateAssetColumnData(ByVal AssetData As AssetItem, ByVal AssetNode As Node)

        ' Check for custom name
        AssetNode.Cells(AssetColumn("AssetOwner")).Tag = AssetData.typeName

        ' Add subitems based on the user selected columns
        Dim colName As String = ""
        ' Add in the module name
        If PlugInData.AssetItemNames.ContainsKey(AssetData.itemID) = True Then
            AssetNode.Text = PlugInData.AssetItemNames(AssetData.itemID)
        Else
            AssetNode.Text = AssetData.typeName
        End If

        ' Establish price & fix references to Blueprint if applicable
        If AssetData.category = "Blueprint" Then
            If AssetNode.Text.Contains("Blueprint") = True And chkExcludeBPs.Checked = True Then
                AssetData.price = 0
            Else
                ' Check with BP Manager if this is a BPO
                Dim IsBPO As Boolean = True
                If PlugInData.BlueprintAssets.ContainsKey(AssetData.owner) = True Then
                    If PlugInData.BlueprintAssets(AssetData.owner).ContainsKey(AssetData.itemID) = True Then
                        Dim chkBPO As BlueprintAsset = PlugInData.BlueprintAssets(AssetData.owner).Item(AssetData.itemID)
                        If chkBPO.Runs > -1 Or chkBPO.BPType = BPType.Unknown Then
                            IsBPO = False
                            If chkBPO.BPType <> BPType.Unknown Then
                                AssetNode.Text = AssetNode.Text.Replace("Blueprint", "Blueprint (Copy)")
                            End If
                        Else
                            AssetNode.Text = AssetNode.Text.Replace("Blueprint", "Blueprint (Original)")
                        End If
                    Else
                        If AssetData.rawquantity = -2 Then
                            IsBPO = False
                        Else
                            IsBPO = True
                        End If
                    End If
                Else
                    If AssetData.rawquantity = -2 Then
                        IsBPO = False
                    Else
                        IsBPO = True
                    End If
                End If
                If IsBPO = True Then
                    AssetData.price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(AssetData.typeID), 2)
                    AssetNode.Text = AssetNode.Text.Replace("Blueprint", "Blueprint (Original)")
                Else
                    AssetData.price = 0
                    AssetNode.Text = AssetNode.Text.Replace("Blueprint", "Blueprint (Copy)")
                End If
            End If
        Else
            AssetData.price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(AssetData.typeID), 2)
        End If

        ' Add the additional columns
        For Each UserCol As UserSlotColumn In Settings.PrismSettings.UserSlotColumns
            Select Case UserCol.Name
                Case "AssetOwner"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.owner
                Case "AssetGroup"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.group
                Case "AssetCategory"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.category
                Case "AssetSystem"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.system
                Case "AssetConstellation"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.constellation
                Case "AssetRegion"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.region
                Case "AssetSystemSec"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.systemsec
                Case "AssetLocation"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.location
                Case "AssetMeta"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.meta
                Case "AssetVolume"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.volume
                Case "AssetQuantity"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.quantity.ToString("N0")
                Case "AssetPrice"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = AssetData.price.ToString("N2")
                Case "AssetValue"
                    AssetNode.Cells(AssetColumn(UserCol.Name)).Text = (AssetData.price * AssetData.quantity).ToString("N2")
            End Select
        Next
    End Sub

#End Region

#Region "Asset Column UI Routines"

    Private Sub adtAssets_ColumnMoved(ByVal sender As Object, ByVal ea As DevComponents.AdvTree.ColumnMovedEventArgs) Handles adtAssets.ColumnMoved

        If ea.NewColumnDisplayIndex = 0 Then
            ea.Column.DisplayIndex = 2
        End If
        ' Get true locations
        Dim StartColName As String = ea.Column.Name
        Dim EndColName As String = adtAssets.Columns(ea.NewColumnDisplayIndex).Name
        Dim StartIdx As Integer = 0
        Dim EndIdx As Integer = 0
        For idx As Integer = 1 To Prism.Settings.PrismSettings.UserSlotColumns.Count - 1
            If Prism.Settings.PrismSettings.UserSlotColumns(idx).Name = StartColName Then
                StartIdx = idx
            End If
            If Prism.Settings.PrismSettings.UserSlotColumns(idx).Name = EndColName Then
                EndIdx = idx
            End If
        Next

        If ea.OldColumnDisplayIndex = 0 Then
            ' Ignore stuff
        Else
            ' We shouldn't overwrite the main column!
            Dim SCol As UserSlotColumn = Prism.Settings.PrismSettings.UserSlotColumns(StartIdx)
            Dim StartUserCol As New UserSlotColumn(SCol.Name, SCol.Description, SCol.Width, SCol.Active)
            If EndIdx > StartIdx Then
                For Idx As Integer = StartIdx To EndIdx - 1
                    Dim MCol As UserSlotColumn = Prism.Settings.PrismSettings.UserSlotColumns(Idx + 1)
                    Prism.Settings.PrismSettings.UserSlotColumns(Idx) = New UserSlotColumn(MCol.Name, MCol.Description, MCol.Width, MCol.Active)
                Next
                Prism.Settings.PrismSettings.UserSlotColumns(EndIdx) = StartUserCol
            Else
                For Idx As Integer = StartIdx - 1 To EndIdx Step -1
                    Dim MCol As UserSlotColumn = Prism.Settings.PrismSettings.UserSlotColumns(Idx)
                    Prism.Settings.PrismSettings.UserSlotColumns(Idx + 1) = New UserSlotColumn(MCol.Name, MCol.Description, MCol.Width, MCol.Active)
                Next
                Prism.Settings.PrismSettings.UserSlotColumns(EndIdx) = StartUserCol
            End If
        End If
        Me.RefreshAssets()
    End Sub

    Private Sub adtAssets_ColumnResized(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtAssets.ColumnResized
        Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        If ch.Name <> "AssetName" Then
            For Each UserCol As UserSlotColumn In Prism.Settings.PrismSettings.UserSlotColumns
                If UserCol.Name = ch.Name Then
                    UserCol.Width = ch.Width.Absolute
                    Exit Sub
                End If
            Next
        Else
            Prism.Settings.PrismSettings.SlotNameWidth = ch.Width.Absolute
        End If
    End Sub


#End Region

#Region "Assets XML Parsing"
    Private Sub PopulateAssets()
        assetList.Clear()
        adtAssets.BeginUpdate()
        adtAssets.Nodes.Clear()
        totalAssetValue = 0
        totalAssetCount = 0
        ' Initialise the user defined slot columns
        Call Me.UpdateAssetSlotColumns()
        ' Get the details of corp accounts
        Call Me.ParseCorpSheets()
        If chkExcludeItems.Checked = False Then
            Call Me.PopulateAssetTree()
            If filters.Count > 0 Or searchText <> "" Then
                Call Me.FilterTree()
            End If
            ' Check for minimum system value
            If chkMinSystemValue.Checked = True Then
                Call Me.FilterSystemValue()
            End If
        End If
        If chkExcludeCash.Checked = False And txtSearch.Text = "" Then
            Call Me.DisplayISKAssets()
        End If
        If chkExcludeOrders.Checked = False Then
            Call Me.DisplayOrders()
        End If
        If chkExcludeResearch.Checked = False Then
            Call Me.DisplayResearch()
        End If
        If chkExcludeContracts.Checked = False Then
            Call Me.DisplayContracts()
        End If
        lblTotalAssetsLabel.Text = "Total Displayed Asset Value: " & totalAssetValue.ToString("N2") & " ISK  (" & totalAssetCount.ToString("N0") & " total quantity)"
        EveHQ.Core.AdvTreeSorter.Sort(adtAssets, 1, True, True)
        adtAssets.EndUpdate()
    End Sub
    Private Sub ParseCorpSheets()
        ' Reset the lists of divisions and wallets
        divisions.Clear()
        walletDivisions.Clear()
        Dim Owner As New PrismOwner

        For Each cOwner As ListViewItem In PSCAssetOwners.ItemList.CheckedItems

            If PlugInData.PrismOwners.ContainsKey(cOwner.Text) = True Then

                Owner = PlugInData.PrismOwners(cOwner.Text)
                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.CorpSheet)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.CorpSheet)

                If OwnerAccount IsNot Nothing Then

                    If Owner.IsCorp = True Then
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                        Dim corpXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.CorpSheet, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        If corpXML IsNot Nothing Then
                            ' Check response string for any error codes?
                            Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                            If errlist.Count = 0 Then
                                ' No errors so parse the files
                                Dim divList As XmlNodeList
                                Dim div As XmlNode
                                divList = corpXML.SelectNodes("/eveapi/result/rowset")
                                For Each div In divList
                                    Select Case div.Attributes.GetNamedItem("name").Value
                                        Case "divisions"
                                            For Each divName As XmlNode In div.ChildNodes
                                                If divisions.ContainsKey(OwnerID & "_" & divName.Attributes.GetNamedItem("accountKey").Value) = False Then
                                                    divisions.Add(Owner.ID & "_" & divName.Attributes.GetNamedItem("accountKey").Value, StrConv(divName.Attributes.GetNamedItem("description").Value, VbStrConv.ProperCase))
                                                End If
                                            Next
                                        Case "walletDivisions"
                                            For Each divName As XmlNode In div.ChildNodes
                                                If walletDivisions.ContainsKey(OwnerID & "_" & divName.Attributes.GetNamedItem("accountKey").Value) = False Then
                                                    walletDivisions.Add(Owner.ID & "_" & divName.Attributes.GetNamedItem("accountKey").Value, divName.Attributes.GetNamedItem("description").Value)
                                                End If
                                            Next
                                    End Select
                                Next
                            End If
                        Else
                            For divID As Integer = 1000 To 1006
                                divisions.Add(OwnerID & "_" & divID.ToString, "Division " & divID.ToString)
                                walletDivisions.Add(OwnerID & "_" & divID.ToString, "Wallet Division " & divID.ToString)
                            Next
                        End If
                    End If
                End If

            End If

        Next
    End Sub
    Private Sub PopulateAssetTree()

        Dim Owner As New PrismOwner

        For Each cOwner As ListViewItem In PSCAssetOwners.ItemList.CheckedItems

            If PlugInData.PrismOwners.ContainsKey(cOwner.Text) = True Then
                Owner = PlugInData.PrismOwners(cOwner.Text)
                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Assets)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Assets)

                If OwnerAccount IsNot Nothing Then

                    Dim AssetXML As New XmlDocument
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    If Owner.IsCorp = True Then
                        assetCorpMode = chkCorpHangarMode.Checked
                        AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    Else
                        assetCorpMode = False
                        AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    End If

                    If AssetXML IsNot Nothing Then
                        Dim locList As XmlNodeList
                        Dim loc As XmlNode
                        Dim EveLocation As New SolarSystem
                        locList = AssetXML.SelectNodes("/eveapi/result/rowset/row")
                        If locList.Count > 0 Then
                            'Dim linePrice As Double = 0
                            Dim containerPrice As Double = 0
                            Dim AssetIsInHanger As Boolean = False
                            Dim hangarPrice As Double = 0
                            For Each loc In locList
                                ' Check if the location is already listed
                                Dim locNode As New Node
                                For NewCell As Integer = 1 To NumberOfActiveColumns : locNode.Cells.Add(New Cell) : Next
                                Dim addLocation As Boolean = True
                                For Each testNode As Node In adtAssets.Nodes
                                    If testNode.Tag.ToString = (loc.Attributes.GetNamedItem("locationID").Value) Then
                                        locNode = testNode
                                        addLocation = False
                                        Exit For
                                    End If
                                Next
                                If addLocation = True Then
                                    Dim locID As String = loc.Attributes.GetNamedItem("locationID").Value
                                    If CDbl(locID) >= 66000000 Then
                                        If CDbl(locID) < 66014933 Then
                                            locID = (CDbl(locID) - 6000001).ToString
                                        Else
                                            locID = (CDbl(locID) - 6000000).ToString
                                        End If
                                    End If
                                    Dim newLocation As Prism.Station
                                    If CDbl(locID) >= 61000000 And CDbl(locID) <= 61999999 Then
                                        If PlugInData.stations.Contains(locID) = True Then
                                            ' Known Outpost
                                            newLocation = CType(PlugInData.stations(locID), Prism.Station)
                                            locNode.Text = newLocation.stationName
                                            locNode.Tag = newLocation.stationID
                                            EveLocation = CType(PlugInData.stations(newLocation.systemID.ToString), SolarSystem)
                                            locNode.Cells(AssetColumn("AssetSystem")).Tag = EveLocation
                                        Else
                                            ' Unknown outpost!
                                            newLocation = New Prism.Station
                                            newLocation.stationID = CLng(locID)
                                            newLocation.stationName = "Unknown Outpost"
                                            newLocation.systemID = 0
                                            newLocation.constID = 0
                                            newLocation.regionID = 0
                                            locNode.Text = newLocation.stationName
                                            locNode.Tag = newLocation.stationID
                                            EveLocation = Nothing
                                            locNode.Cells(AssetColumn("AssetSystem")).Tag = EveLocation
                                        End If
                                    Else
                                        If CDbl(locID) < 60000000 Then
                                            Dim newSystem As SolarSystem = CType(PlugInData.stations(locID), SolarSystem)
                                            EveLocation = newSystem
                                            locNode.Text = newSystem.Name
                                            locNode.Tag = newSystem.ID
                                            locNode.Cells(AssetColumn("AssetSystem")).Tag = EveLocation
                                        Else
                                            newLocation = CType(PlugInData.stations(locID), Prism.Station)
                                            If newLocation IsNot Nothing Then
                                                locNode.Text = newLocation.stationName
                                                locNode.Tag = newLocation.stationID
                                                EveLocation = CType(PlugInData.stations(newLocation.systemID.ToString), SolarSystem)
                                                locNode.Cells(AssetColumn("AssetSystem")).Tag = EveLocation
                                            Else
                                                ' Unknown system/station!
                                                newLocation = New Prism.Station
                                                newLocation.stationID = CLng(locID)
                                                newLocation.stationName = "Unknown Location"
                                                newLocation.systemID = 0
                                                newLocation.constID = 0
                                                newLocation.regionID = 0
                                                locNode.Text = newLocation.stationName
                                                locNode.Tag = newLocation.stationID
                                                EveLocation = Nothing
                                                locNode.Cells(AssetColumn("AssetSystem")).Tag = EveLocation
                                            End If
                                        End If
                                    End If
                                    locNode.Cells(AssetColumn("AssetOwner")).Tag = locNode.Text
                                    If EveLocation IsNot Nothing Then
                                        locNode.Cells(AssetColumn("AssetSystem")).Text = EveLocation.Name
                                        locNode.Cells(AssetColumn("AssetConstellation")).Text = EveLocation.Constellation
                                        locNode.Cells(AssetColumn("AssetRegion")).Text = EveLocation.Region
                                        locNode.Cells(AssetColumn("AssetSystemSec")).Text = EveLocation.Security.ToString("N2")
                                    Else
                                        locNode.Cells(AssetColumn("AssetSystem")).Text = "Unknown"
                                        locNode.Cells(AssetColumn("AssetConstellation")).Text = "Unknown"
                                        locNode.Cells(AssetColumn("AssetRegion")).Text = "Unknown"
                                        locNode.Cells(AssetColumn("AssetSystemSec")).Text = "Unknown"
                                    End If
                                    adtAssets.Nodes.Add(locNode)
                                End If

                                EveLocation = CType(locNode.Cells(AssetColumn("AssetSystem")).Tag, SolarSystem)
                                Dim itemID As String = loc.Attributes.GetNamedItem("typeID").Value
                                Dim itemData As New EveHQ.Core.EveItem
                                Dim itemName As String = ""
                                Dim groupID As String = ""
                                Dim catID As String = ""
                                Dim groupName As String = ""
                                Dim catName As String = ""
                                Dim metaLevel As String = ""
                                Dim volume As String = ""
                                If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
                                    itemData = EveHQ.Core.HQ.itemData(itemID)
                                    itemName = itemData.Name
                                    groupID = CStr(itemData.Group)
                                    catID = CStr(itemData.Category)
                                    groupName = EveHQ.Core.HQ.itemGroups(groupID)
                                    catName = EveHQ.Core.HQ.itemCats(catID)
                                    metaLevel = EveHQ.Core.HQ.itemData(itemID).MetaLevel.ToString
                                    If PlugInData.PackedVolumes.ContainsKey(groupID) = True Then
                                        If loc.Attributes.GetNamedItem("singleton").Value = "0" Then
                                            volume = (PlugInData.PackedVolumes(groupID) * CDbl(loc.Attributes.GetNamedItem("quantity").Value)).ToString("N2")
                                        Else
                                            volume = (EveHQ.Core.HQ.itemData(itemID).Volume * CDbl(loc.Attributes.GetNamedItem("quantity").Value)).ToString("N2")
                                        End If
                                    Else
                                        volume = (EveHQ.Core.HQ.itemData(itemID).Volume * CDbl(loc.Attributes.GetNamedItem("quantity").Value)).ToString("N2")
                                    End If
                                Else
                                    ' Can't find the item in the database
                                    itemName = "ItemID: " & itemID.ToString
                                    groupID = "unknown"
                                    catID = "unknown"
                                    groupName = "Unknown"
                                    catName = "Unknown"
                                    metaLevel = "0"
                                    volume = "0"
                                End If

                                Dim newAsset As New Node
                                For NewCell As Integer = 1 To NumberOfActiveColumns : newAsset.Cells.Add(New Cell) : Next
                                newAsset.Tag = loc.Attributes.GetNamedItem("itemID").Value
                                Dim flagID As Integer = CInt(loc.Attributes.GetNamedItem("flag").Value)
                                Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                                Dim accountID As Integer = flagID + 885
                                If accountID = 889 Then accountID = 1000
                                If Owner.IsCorp = True And itemName <> "Office" Then
                                    If divisions.ContainsKey(Owner.ID & "_" & accountID.ToString) = True Then
                                        flagName = CStr(divisions.Item(Owner.ID & "_" & accountID.ToString))
                                        If assetCorpMode = True And locNode.Nodes.Count < 7 And itemName <> "Office" Then
                                            ' Build the corp division nodes
                                            For div As Integer = 0 To 6
                                                Dim hangar As New Node
                                                For NewCell As Integer = 1 To NumberOfActiveColumns : hangar.Cells.Add(New Cell) : Next
                                                hangar.Text = CStr(divisions.Item(Owner.ID & "_" & (1000 + div).ToString))
                                                locNode.Nodes.Add(hangar)
                                                hangar.Cells(AssetColumn("AssetValue")).Text = CDbl(0).ToString("N2")
                                            Next
                                        End If
                                    End If
                                End If

                                ' Add the asset to the treelistview
                                If assetCorpMode = True And itemName <> "Office" And (flagID = 4 Or (flagID >= 116 And flagID <= 121)) Then
                                    If (accountID - 1000) >= 0 And (accountID - 1000) < locNode.Nodes.Count Then
                                        locNode.Nodes(accountID - 1000).Nodes.Add(newAsset)
                                        AssetIsInHanger = True
                                    Else
                                        ' Catch case where errors were occuring
                                        Dim msg As String = "Unable to add corp asset into hangar:" & ControlChars.CrLf
                                        msg &= "AssetID: " & newAsset.Tag.ToString & ControlChars.CrLf
                                        msg &= "Item: " & itemName & ControlChars.CrLf
                                        msg &= "FlagID: " & flagID.ToString & " (" & flagName & ")" & ControlChars.CrLf
                                        msg &= "AccountID: " & accountID.ToString & ControlChars.CrLf
                                        msg &= "Parent: " & locNode.Text & ControlChars.CrLf & ControlChars.CrLf
                                        msg &= "Please post this as a bug report - the text of this message has been copied to the clipboard to assist you." & ControlChars.CrLf & ControlChars.CrLf
                                        msg &= "Corp Assets will be incomplete while this error appears. Corp Hangar Mode will be disabled."
                                        chkCorpHangarMode.Checked = False
                                        Try
                                            Clipboard.SetText(msg)
                                        Catch e As Exception
                                        End Try
                                        MessageBox.Show(msg, "Corp Hangar Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                        Exit For
                                    End If
                                Else
                                    locNode.Nodes.Add(newAsset)
                                    AssetIsInHanger = False
                                End If

                                ' Add the asset to the list of assets
                                Dim newAssetList As New AssetItem
                                newAssetList.itemID = newAsset.Tag.ToString
                                newAssetList.system = locNode.Text
                                newAssetList.typeID = itemID
                                newAssetList.typeName = itemName
                                newAssetList.owner = Owner.Name
                                newAssetList.group = groupName
                                newAssetList.category = catName
                                If EveLocation IsNot Nothing Then
                                    newAssetList.system = EveLocation.Name
                                    newAssetList.constellation = EveLocation.Constellation
                                    newAssetList.region = EveLocation.Region
                                    newAssetList.systemsec = EveLocation.Security.ToString("N2")
                                Else
                                    newAssetList.system = "Unknown"
                                    newAssetList.constellation = "Unknown"
                                    newAssetList.region = "Unknown"
                                    newAssetList.systemsec = "Unknown"
                                End If
                                newAssetList.location = flagName
                                newAssetList.meta = metaLevel
                                newAssetList.volume = volume
                                newAssetList.quantity = CLng(loc.Attributes.GetNamedItem("quantity").Value)
                                If loc.Attributes.GetNamedItem("rawQuantity") IsNot Nothing Then
                                    newAssetList.rawquantity = CInt(loc.Attributes.GetNamedItem("rawQuantity").Value)
                                Else
                                    newAssetList.rawquantity = 0
                                End If
                                newAssetList.price = 0
                                totalAssetCount += newAssetList.quantity
                                If assetList.ContainsKey(newAssetList.itemID) = False Then
                                    assetList.Add(newAssetList.itemID, newAssetList)
                                End If

                                Call Me.UpdateAssetColumnData(newAssetList, newAsset)

                                ' Check if this row has child nodes and repeat
                                If loc.HasChildNodes = True Then
                                    Call Me.PopulateAssetNode(newAsset, loc, Owner.Name, locNode.Text, Owner, EveLocation)
                                End If

                                ' Update hangar price if applicable
                                If AssetIsInHanger = True Then
                                    hangarPrice = CDbl(newAsset.Parent.Cells(AssetColumn("AssetValue")).Text)
                                    newAsset.Parent.Cells(AssetColumn("AssetValue")).Text = (hangarPrice + CDbl(newAsset.Cells(AssetColumn("AssetValue")).Text)).ToString("N2")
                                End If

                            Next
                        End If
                    End If

                End If
            End If
        Next

        ' Establish container prices
        For Each PNode As Node In adtAssets.Nodes
            Call SetNodeValue(PNode)
            Call SetNodeVolume(PNode)
        Next

        ' Get the locations and total prices
        Dim locationPrice As Double = 0
        Dim locationVolume As Double = 0
        Dim cLoc As Node
        Dim cL As Integer = 0
        If adtAssets.Nodes.Count > 0 Then
            Do
                cLoc = adtAssets.Nodes(cL)
                locationPrice = 0
                locationVolume = 0
                For Each cLine As Node In cLoc.Nodes
                    locationPrice += CDbl(cLine.Cells(AssetColumn("AssetValue")).Text)
                    If cLine.Cells(AssetColumn("AssetVolume")).Text <> "" Then
                        locationVolume += CDbl(cLine.Cells(AssetColumn("AssetVolume")).Text)
                    End If
                Next
                totalAssetValue += locationPrice
                cLoc.Cells(AssetColumn("AssetValue")).Text = locationPrice.ToString("N2")
                cLoc.Cells(AssetColumn("AssetVolume")).Text = locationVolume.ToString("N2")
                ' Delete if no child nodes at the locations
                If cLoc.Nodes.Count = 0 Then
                    adtAssets.Nodes.Remove(cLoc)
                    cL -= 1
                End If
                cL += 1
            Loop Until cL = adtAssets.Nodes.Count
        End If

        If adtAssets.Nodes.Count = 0 Then
            adtAssets.Nodes.Add(New Node("Unable to get Assets data - check API."))
            adtAssets.Enabled = False
        Else
            adtAssets.Enabled = True
        End If

    End Sub
    Private Function PopulateAssetNode(ByVal parentAsset As Node, ByVal loc As XmlNode, ByVal assetOwner As String, ByVal location As String, Owner As PrismOwner, ByVal EveLocation As SolarSystem) As Double
        Dim subLocList As XmlNodeList
        Dim subLoc As XmlNode
        Dim containerPrice As Double = 0
        Dim AssetIsInHanger As Boolean = False
        Dim hangarPrice As Double = 0
        Dim linePrice As Double = 0
        subLocList = loc.ChildNodes(0).ChildNodes
        If IsNumeric(parentAsset.Cells(AssetColumn("AssetPrice")).Text) = True Then
            containerPrice = CDbl(parentAsset.Cells(AssetColumn("AssetPrice")).Text)
        Else
            containerPrice = 0
            parentAsset.Cells(AssetColumn("AssetPrice")).Text = CDbl(0).ToString("N2")
        End If
        For Each subLoc In subLocList
            Try
                Dim ItemID As String = subLoc.Attributes.GetNamedItem("typeID").Value
                Dim ItemData As New EveHQ.Core.EveItem
                Dim itemName As String = ""
                Dim groupID As String = ""
                Dim catID As String = ""
                Dim groupName As String = ""
                Dim catName As String = ""
                Dim metaLevel As String = ""
                Dim volume As String = ""
                If EveHQ.Core.HQ.itemData.ContainsKey(ItemID) Then
                    ItemData = EveHQ.Core.HQ.itemData(ItemID)
                    itemName = ItemData.Name
                    groupID = CStr(ItemData.Group)
                    catID = CStr(ItemData.Category)
                    groupName = EveHQ.Core.HQ.itemGroups(groupID)
                    catName = EveHQ.Core.HQ.itemCats(catID)
                    metaLevel = EveHQ.Core.HQ.itemData(ItemID).MetaLevel.ToString
                    If PlugInData.PackedVolumes.ContainsKey(groupID) = True Then
                        If loc.Attributes.GetNamedItem("singleton").Value = "0" Then
                            volume = (PlugInData.PackedVolumes(groupID) * CDbl(loc.Attributes.GetNamedItem("quantity").Value)).ToString("N2")
                        Else
                            volume = (EveHQ.Core.HQ.itemData(ItemID).Volume * CDbl(subLoc.Attributes.GetNamedItem("quantity").Value)).ToString("N2")
                        End If
                    Else
                        volume = (EveHQ.Core.HQ.itemData(ItemID).Volume * CDbl(subLoc.Attributes.GetNamedItem("quantity").Value)).ToString("N2")
                    End If
                Else
                    ' Can't find the item in the database
                    itemName = "ItemID: " & ItemID.ToString
                    groupID = "unknown"
                    catID = "unknown"
                    groupName = "Unknown"
                    catName = "Unknown"
                    metaLevel = "0"
                    volume = "0"
                End If

                Dim subAsset As New Node
                For NewCell As Integer = 1 To NumberOfActiveColumns : subAsset.Cells.Add(New Cell) : Next
                subAsset.Tag = subLoc.Attributes.GetNamedItem("itemID").Value
                Dim subFlagID As Integer = CInt(subLoc.Attributes.GetNamedItem("flag").Value)
                Dim subFlagName As String = PlugInData.itemFlags(subFlagID).ToString
                Dim accountID As Integer = subFlagID + 885
                If accountID = 889 Then accountID = 1000
                If Owner.IsCorp And itemName <> "Office" And accountID >= 1000 And accountID <= 1006 Then
                    If divisions.ContainsKey(Owner.ID & "_" & accountID.ToString) = True Then
                        subFlagName = CStr(divisions.Item(Owner.ID & "_" & accountID.ToString))
                        If assetCorpMode = True And parentAsset.Nodes.Count < 7 And itemName <> "Office" Then
                            ' Build the corp division nodes
                            For div As Integer = 0 To 6
                                Dim hangar As New Node
                                For NewCell As Integer = 1 To NumberOfActiveColumns : hangar.Cells.Add(New Cell) : Next
                                hangar.Text = CStr(divisions.Item(Owner.ID & "_" & (1000 + div).ToString))
                                parentAsset.Nodes.Add(hangar)
                                hangar.Cells(AssetColumn("AssetValue")).Text = CDbl(0).ToString("N2")
                            Next
                        End If
                    Else
                        ' Don't have a proper division
                        subFlagName = "Corp Division " & accountID.ToString
                        If assetCorpMode = True And parentAsset.Nodes.Count < 7 And itemName <> "Office" Then
                            ' Build the corp division nodes
                            For div As Integer = 0 To 6
                                Dim hangar As New Node
                                For NewCell As Integer = 1 To NumberOfActiveColumns : hangar.Cells.Add(New Cell) : Next
                                hangar.Text = "Corp Division " & div.ToString
                                parentAsset.Nodes.Add(hangar)
                                hangar.Cells(AssetColumn("AssetValue")).Text = CDbl(0).ToString("N2")
                            Next
                        End If
                    End If
                End If

                ' Add the asset to the list of assets
                Dim newAssetList As New AssetItem
                newAssetList.itemID = subAsset.Tag.ToString
                newAssetList.system = location
                newAssetList.typeID = ItemID
                newAssetList.typeName = itemName
                newAssetList.owner = assetOwner
                newAssetList.group = groupName
                newAssetList.category = catName
                If EveLocation IsNot Nothing Then
                    newAssetList.system = EveLocation.Name
                    newAssetList.constellation = EveLocation.Constellation
                    newAssetList.region = EveLocation.Region
                    newAssetList.systemsec = EveLocation.Security.ToString("N2")
                Else
                    newAssetList.system = "Unknown"
                    newAssetList.constellation = "Unknown"
                    newAssetList.region = "Unknown"
                    newAssetList.systemsec = "Unknown"
                End If
                newAssetList.location = parentAsset.Text & ": " & subFlagName
                newAssetList.meta = metaLevel
                newAssetList.volume = volume
                newAssetList.quantity = CLng(subLoc.Attributes.GetNamedItem("quantity").Value)
                If subLoc.Attributes.GetNamedItem("rawQuantity") IsNot Nothing Then
                    newAssetList.rawquantity = CInt(subLoc.Attributes.GetNamedItem("rawQuantity").Value)
                Else
                    newAssetList.rawquantity = 0
                End If
                newAssetList.price = 0
                totalAssetCount += newAssetList.quantity

                If assetList.ContainsKey(newAssetList.itemID) = False Then
                    assetList.Add(newAssetList.itemID, newAssetList)

                    If assetCorpMode = True And itemName <> "Office" And (subFlagID = 4 Or (subFlagID >= 116 And subFlagID <= 121)) Then
                        parentAsset.Nodes(accountID - 1000).Nodes.Add(subAsset)
                        AssetIsInHanger = True
                    Else
                        parentAsset.Nodes.Add(subAsset)
                        AssetIsInHanger = False
                    End If

                    Call Me.UpdateAssetColumnData(newAssetList, subAsset)

                    ' Update hangar price if applicable
                    containerPrice += (newAssetList.price * newAssetList.quantity)
                    If AssetIsInHanger = True Then
                        hangarPrice = CDbl(subAsset.Parent.Cells(AssetColumn("AssetValue")).Text)
                        subAsset.Parent.Cells(AssetColumn("AssetValue")).Text = (hangarPrice + linePrice).ToString("N2")
                    End If

                    If subLoc.HasChildNodes = True Then
                        containerPrice -= linePrice
                        containerPrice += PopulateAssetNode(subAsset, subLoc, assetOwner, location, Owner, EveLocation)
                    End If

                End If

            Catch ex As Exception
                Dim msg As String = "Unable to parse Asset:" & ControlChars.CrLf
                msg &= "InnerXML: " & subLoc.InnerXml & ControlChars.CrLf
                msg &= "InnerText: " & subLoc.InnerText & ControlChars.CrLf
                msg &= "TypeID: " & subLoc.Attributes.GetNamedItem("typeID").Value
                MessageBox.Show(msg, "Error Parsing Assets File For " & assetOwner, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        Next
        parentAsset.Cells(AssetColumn("AssetValue")).Text = containerPrice.ToString("N2")
        Return containerPrice
    End Function
    Private Function SetNodeValue(ByVal PNode As Node) As Double
        Dim NodeValue As Double = 0
        ' Add current Node value
        If PNode.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
            NodeValue += CDbl(PNode.Cells(AssetColumn("AssetQuantity")).Text) * CDbl(PNode.Cells(AssetColumn("AssetPrice")).Text)
        End If
        ' Add value of child nodes
        If PNode.Nodes.Count > 0 Then
            For Each CNode As Node In PNode.Nodes
                NodeValue += SetNodeValue(CNode)
            Next
        End If
        PNode.Cells(AssetColumn("AssetValue")).Text = NodeValue.ToString("N2")
        Return NodeValue
    End Function
    Private Function SetNodeVolume(ByVal PNode As Node) As Double
        Dim NodeVolume As Double = 0
        ' Add current Node volume
        If PNode.Cells(AssetColumn("AssetVolume")).Text <> "" Then
            NodeVolume += CDbl(PNode.Cells(AssetColumn("AssetVolume")).Text)
        End If
        ' Add value of child nodes
        If PNode.Nodes.Count > 0 Then
            For Each CNode As Node In PNode.Nodes
                NodeVolume += SetNodeVolume(CNode)
            Next
        End If
        PNode.Cells(AssetColumn("AssetVolume")).Text = NodeVolume.ToString("N2")
        Return NodeVolume
    End Function
    Private Sub DisplayISKAssets()
        Dim Owner As New PrismOwner

        ' Reset and parse the character wallets
        charWallets.Clear()
        corpWallets.Clear()
        corpWalletDivisions.Clear()
        For Each cOwner As ListViewItem In PSCAssetOwners.ItemList.CheckedItems

            If PlugInData.PrismOwners.ContainsKey(cOwner.Text) = True Then

                owner = PlugInData.PrismOwners(cOwner.Text)
                Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Balances)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Balances)

                If OwnerAccount IsNot Nothing Then

                    If Owner.IsCorp = True Then
                        ' Check for corp wallets
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                        Dim corpXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        If corpXML IsNot Nothing Then
                            ' Check response string for any error codes?
                            Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                            If errlist.Count = 0 Then
                                ' No errors so parse the files
                                Dim accountList As XmlNodeList
                                Dim account As XmlNode
                                If corpWallets.Contains(Owner.Name) = False Then
                                    corpWallets.Add(Owner.Name, Owner.ID)
                                    accountList = corpXML.SelectNodes("/eveapi/result/rowset/row")
                                    For Each account In accountList
                                        Dim isk As Double = Double.Parse(account.Attributes.GetNamedItem("balance").Value, Globalization.NumberStyles.Any, culture)
                                        Dim accountKey As String = account.Attributes.GetNamedItem("accountKey").Value
                                        If corpWalletDivisions.ContainsKey(Owner.ID & "_" & accountKey) = False Then
                                            corpWalletDivisions.Add(Owner.ID & "_" & accountKey, isk)
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Else
                        ' Check for character wallets
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                        Dim corpXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.AccountBalancesChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        If corpXML IsNot Nothing Then
                            ' Check response string for any error codes?
                            Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                            If errlist.Count = 0 Then
                                ' No errors so parse the files
                                Dim accountList As XmlNodeList
                                Dim account As XmlNode
                                accountList = corpXML.SelectNodes("/eveapi/result/rowset/row")
                                For Each account In accountList
                                    Dim isk As Double = Double.Parse(account.Attributes.GetNamedItem("balance").Value, Globalization.NumberStyles.Any, culture)
                                    If charWallets.ContainsKey(Owner.Name) = False Then
                                        charWallets.Add(Owner.Name, isk)
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            End If
        Next

        ' Add the balances to the assets schedule
        Dim node As New Node
        For NewCell As Integer = 1 To NumberOfActiveColumns : node.Cells.Add(New Cell) : Next
        Dim totalCash As Double = 0
        node.Tag = "ISK"
        node.Text = "Cash Balances"
        ' Add the personal balances
        If charWallets.Count > 0 Then
            Dim personalNode As New Node
            For NewCell As Integer = 1 To NumberOfActiveColumns : personalNode.Cells.Add(New Cell) : Next
            personalNode.Tag = "Personal"
            personalNode.Text = "Personal"
            node.Nodes.Add(personalNode)
            Dim personalCash As Double = 0
            For Each pilot As String In charWallets.Keys
                Dim iskNode As New Node
                For NewCell As Integer = 1 To NumberOfActiveColumns : iskNode.Cells.Add(New Cell) : Next
                iskNode.Tag = pilot
                iskNode.Text = pilot
                personalNode.Nodes.Add(iskNode)
                iskNode.Cells(AssetColumn("AssetValue")).Text = charWallets(pilot).ToString("N2")
                personalCash += CDbl(charWallets(pilot))
            Next
            personalNode.Cells(AssetColumn("AssetValue")).Text = personalCash.ToString("N2")
            totalCash += personalCash
        End If
        ' Add the corporate balances
        If corpWallets.Count > 0 Then
            Dim corporateNode As New Node
            For NewCell As Integer = 1 To NumberOfActiveColumns : corporateNode.Cells.Add(New Cell) : Next
            corporateNode.Tag = "Corporate"
            corporateNode.Text = "Corporate"
            node.Nodes.Add(corporateNode)
            Dim corporateCash As Double = 0
            For Each corpName As String In corpWallets.Keys
                Dim corpID As String = CStr(corpWallets(corpName))
                Dim corpNode As New Node
                For NewCell As Integer = 1 To NumberOfActiveColumns : corpNode.Cells.Add(New Cell) : Next
                corpNode.Tag = corpName
                corpNode.Text = corpName
                corporateNode.Nodes.Add(corpNode)
                Dim divisionCash As Double = 0
                For key As Integer = 1000 To 1006
                    Dim iskNode As New Node
                    For NewCell As Integer = 1 To NumberOfActiveColumns : iskNode.Cells.Add(New Cell) : Next
                    Dim idx As String = corpID & "_" & key.ToString
                    If walletDivisions.ContainsKey(idx) Then
                        iskNode.Tag = walletDivisions(idx).ToString
                        iskNode.Text = CStr(walletDivisions(idx))
                    Else
                        iskNode.Tag = "Wallet Division " & key.ToString
                        iskNode.Text = "Wallet Division " & key.ToString
                    End If
                    corpNode.Nodes.Add(iskNode)
                    iskNode.Cells(AssetColumn("AssetValue")).Text = corpWalletDivisions(idx).ToString("N2")
                    divisionCash += CDbl(corpWalletDivisions(idx))
                Next
                corporateCash += divisionCash
                corpNode.Cells(AssetColumn("AssetValue")).Text = divisionCash.ToString("N2")
            Next
            corporateNode.Cells(AssetColumn("AssetValue")).Text = corporateCash.ToString("N2")
            totalCash += corporateCash
        End If
        node.Cells(AssetColumn("AssetValue")).Text = totalCash.ToString("N2")
        totalAssetValue += totalCash
        If totalCash > 0 Then
            adtAssets.Nodes.Add(node)
        End If
    End Sub
    Private Sub DisplayOrders()

        ' Add the balances to the assets schedule
        Dim ordersNode As New Node
        Dim buyOrders As New Node
        Dim sellOrders As New Node
        Dim buyValue, sellValue As Double
        ordersNode.Tag = "Orders"
        ordersNode.Text = "Market Orders"
        For NewCell As Integer = 1 To NumberOfActiveColumns : ordersNode.Cells.Add(New Cell) : Next
        ' Add the Buy Orders node
        buyOrders.Text = "Buy Orders"
        buyOrders.Tag = "Buy Orders"
        For NewCell As Integer = 1 To NumberOfActiveColumns : buyOrders.Cells.Add(New Cell) : Next
        ' Add the Sell Orders node
        sellOrders.Text = "Sell Orders"
        sellOrders.Tag = "Sell Orders"
        For NewCell As Integer = 1 To NumberOfActiveColumns : sellOrders.Cells.Add(New Cell) : Next

        For Each cPilot As ListViewItem In PSCAssetOwners.ItemList.CheckedItems
            ' Get the owner we will use
            Dim owner As String = cPilot.Text
            ' Get the orders
            Dim orderCollection As MarketOrdersCollection = ParseMarketOrders(owner)
            ' Add the orders (outstanding ones only)
            Dim ItemName, category, group, meta, vol As String
            Dim EveLocation As SolarSystem
            For Each ownerOrder As MarketOrder In orderCollection.MarketOrders
                If ownerOrder.OrderState = MarketOrderState.Open Then
                    Dim orderNode As New Node
                    For NewCell As Integer = 1 To NumberOfActiveColumns : orderNode.Cells.Add(New Cell) : Next
                    orderNode.Tag = ownerOrder.TypeID
                    If EveHQ.Core.HQ.itemData.ContainsKey(ownerOrder.TypeID.ToString) = True Then
                        Dim orderItem As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(ownerOrder.TypeID.ToString)
                        ItemName = orderItem.Name
                        category = EveHQ.Core.HQ.itemCats(orderItem.Category.ToString)
                        group = EveHQ.Core.HQ.itemGroups(orderItem.Group.ToString)
                        meta = orderItem.MetaLevel.ToString
                        vol = orderItem.Volume.ToString
                    Else
                        ItemName = "ItemID: " & ownerOrder.TypeID.ToString
                        category = "<Unknown>"
                        group = "<Unknown>"
                        meta = "0"
                        vol = "1"
                    End If
                    ' Check for search criteria
                    If Not ((filters.Count > 0 And catFilters.Contains(category) = False And groupFilters.Contains(group) = False) Or (searchText <> "" And ItemName.ToLower.Contains(searchText.ToLower) = False)) Then
                        orderNode.Text = ItemName
                        If ownerOrder.Bid = 0 Then
                            sellOrders.Nodes.Add(orderNode)
                            sellValue += ownerOrder.Price * ownerOrder.VolRemaining
                        Else
                            buyOrders.Nodes.Add(orderNode)
                            buyValue += ownerOrder.Escrow * ownerOrder.VolRemaining
                        End If
                        orderNode.Cells(AssetColumn("AssetOwner")).Text = owner
                        orderNode.Cells(AssetColumn("AssetGroup")).Text = group
                        orderNode.Cells(AssetColumn("AssetCategory")).Text = category
                        If PlugInData.stations.Contains(ownerOrder.StationID) = True Then
                            orderNode.Cells(AssetColumn("AssetLocation")).Text = CType(PlugInData.stations(ownerOrder.StationID), Station).stationName
                            EveLocation = CType(PlugInData.stations(CType(PlugInData.stations(ownerOrder.StationID), Station).systemID.ToString), SolarSystem)
                        Else
                            orderNode.Cells(AssetColumn("AssetLocation")).Text = "StationID: " & ownerOrder.StationID
                            EveLocation = Nothing
                        End If
                        If EveLocation IsNot Nothing Then
                            orderNode.Cells(AssetColumn("AssetSystem")).Text = EveLocation.Name
                            orderNode.Cells(AssetColumn("AssetConstellation")).Text = EveLocation.Constellation
                            orderNode.Cells(AssetColumn("AssetRegion")).Text = EveLocation.Region
                            orderNode.Cells(AssetColumn("AssetSystemSec")).Text = EveLocation.Security.ToString("N2")
                        Else
                            orderNode.Cells(AssetColumn("AssetSystem")).Text = "Unknown"
                            orderNode.Cells(AssetColumn("AssetConstellation")).Text = "Unknown"
                            orderNode.Cells(AssetColumn("AssetRegion")).Text = "Unknown"
                            orderNode.Cells(AssetColumn("AssetSystemSec")).Text = "Unknown"
                        End If
                        orderNode.Cells(AssetColumn("AssetMeta")).Text = meta
                        orderNode.Cells(AssetColumn("AssetVolume")).Text = CDbl(vol).ToString("N2")
                        orderNode.Cells(AssetColumn("AssetQuantity")).Text = ownerOrder.VolRemaining.ToString("N0")
                        If ownerOrder.Bid = 0 Then
                            orderNode.Cells(AssetColumn("AssetPrice")).Text = ownerOrder.Price.ToString("N2")
                            orderNode.Cells(AssetColumn("AssetValue")).Text = (ownerOrder.Price * ownerOrder.VolRemaining).ToString("N2")
                        Else
                            orderNode.Cells(AssetColumn("AssetPrice")).Text = ownerOrder.Escrow.ToString("N2")
                            orderNode.Cells(AssetColumn("AssetValue")).Text = (ownerOrder.Escrow * ownerOrder.VolRemaining).ToString("N2")
                        End If
                    End If
                End If
            Next
        Next
        buyOrders.Cells(AssetColumn("AssetValue")).Text = buyValue.ToString("N2")
        sellOrders.Cells(AssetColumn("AssetValue")).Text = sellValue.ToString("N2")
        ordersNode.Cells(AssetColumn("AssetValue")).Text = (buyValue + sellValue).ToString("N2")
        totalAssetValue += buyValue + sellValue
        If buyOrders.Nodes.Count > 0 Then
            ordersNode.Nodes.Add(buyOrders)
        End If
        If sellOrders.Nodes.Count > 0 Then
            ordersNode.Nodes.Add(sellOrders)
        End If
        If ordersNode.Nodes.Count > 0 Then
            adtAssets.Nodes.Add(ordersNode)
        End If
    End Sub
    Private Function ParseMarketOrders(ByVal OrderOwner As String) As MarketOrdersCollection

        Dim Owner As New PrismOwner
        Dim newOrderCollection As New MarketOrdersCollection

        If PlugInData.PrismOwners.ContainsKey(OrderOwner) = True Then

            Owner = PlugInData.PrismOwners(OrderOwner)
            Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Orders)
            Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Orders)
            Dim OrderXML As New XmlDocument
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)

            If OwnerAccount IsNot Nothing Then

                If Owner.IsCorp = True Then
                    OrderXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                Else
                    OrderXML = APIReq.GetAPIXML(EveAPI.APITypes.OrdersChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)

                End If
                If OrderXML IsNot Nothing Then
                    Dim Orders As XmlNodeList = OrderXML.SelectNodes("/eveapi/result/rowset/row")
                    For Each Order As XmlNode In Orders
                        Dim newOrder As New MarketOrder
                        newOrder.OrderID = Order.Attributes.GetNamedItem("orderID").Value
                        newOrder.CharID = Order.Attributes.GetNamedItem("charID").Value
                        newOrder.StationID = Order.Attributes.GetNamedItem("stationID").Value
                        newOrder.VolEntered = CLng(Order.Attributes.GetNamedItem("volEntered").Value)
                        newOrder.VolRemaining = CLng(Order.Attributes.GetNamedItem("volRemaining").Value)
                        newOrder.MinVolume = CLng(Order.Attributes.GetNamedItem("minVolume").Value)
                        newOrder.OrderState = CInt(Order.Attributes.GetNamedItem("orderState").Value)
                        newOrder.TypeID = CInt(Order.Attributes.GetNamedItem("typeID").Value)
                        newOrder.Range = CInt(Order.Attributes.GetNamedItem("range").Value)
                        newOrder.AccountKey = CInt(Order.Attributes.GetNamedItem("accountKey").Value)
                        newOrder.Duration = CInt(Order.Attributes.GetNamedItem("duration").Value)
                        newOrder.Escrow = Double.Parse(Order.Attributes.GetNamedItem("escrow").Value, culture) / newOrder.VolRemaining
                        newOrder.Price = Double.Parse(Order.Attributes.GetNamedItem("price").Value, culture)
                        newOrder.Bid = CInt(Order.Attributes.GetNamedItem("bid").Value)
                        newOrder.Issued = DateTime.ParseExact(Order.Attributes.GetNamedItem("issued").Value, IndustryTimeFormat, culture, Globalization.DateTimeStyles.None)
                        newOrderCollection.MarketOrders.Add(newOrder)
                        If newOrder.Bid = 0 Then ' Sell Order
                            newOrderCollection.SellOrders += 1
                            newOrderCollection.SellOrderValue += (newOrder.VolRemaining * newOrder.Price)
                        Else ' Buy Order (=1)
                            newOrderCollection.BuyOrders += 1
                            newOrderCollection.BuyOrderValue += (newOrder.VolRemaining * newOrder.Price)
                            newOrderCollection.EscrowValue += (newOrder.Escrow * newOrder.VolRemaining)
                        End If
                    Next
                    newOrderCollection.TotalOrders = newOrderCollection.BuyOrders + newOrderCollection.SellOrders
                End If
            End If
        End If

        Return newOrderCollection

    End Function
    Private Sub DisplayResearch()
        Dim ResearchNode As New Node
        ResearchNode.Tag = "Research"
        ResearchNode.Text = "Assets in Research"
        For NewCell As Integer = 1 To NumberOfActiveColumns : ResearchNode.Cells.Add(New Cell) : Next
        Dim ResearchValue As Double = 0

        For Each cPilot As ListViewItem In PSCAssetOwners.ItemList.CheckedItems

            ' Get the owner we will use
            Dim owner As String = cPilot.Text

            ' Get the JobList
            Dim JobList As List(Of IndustryJob) = IndustryJob.ParseIndustryJobs(owner)
            If JobList IsNot Nothing Then
                Dim category, group As String
                Dim EveLocation As SolarSystem
                For Each Job As IndustryJob In JobList
                    If Job.Completed = 0 Then
                        Dim RNode As New Node
                        For NewCell As Integer = 1 To NumberOfActiveColumns : RNode.Cells.Add(New Cell) : Next
                        RNode.Tag = Job.InstalledItemTypeID.ToString

                        Dim ResearchItem As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(Job.InstalledItemTypeID.ToString)
                        category = EveHQ.Core.HQ.itemCats(ResearchItem.Category.ToString)
                        group = EveHQ.Core.HQ.itemGroups(ResearchItem.Group.ToString)
                        ' Check for search criteria
                        If Not ((filters.Count > 0 And catFilters.Contains(category) = False And groupFilters.Contains(group) = False) Or (searchText <> "" And ResearchItem.Name.ToLower.Contains(searchText.ToLower) = False)) Then
                            ResearchNode.Nodes.Add(RNode)
                            RNode.Text = ResearchItem.Name
                            RNode.Cells(AssetColumn("AssetOwner")).Text = owner
                            RNode.Cells(AssetColumn("AssetGroup")).Text = group
                            RNode.Cells(AssetColumn("AssetCategory")).Text = category
                            If PlugInData.stations.ContainsKey(Job.OutputLocationID.ToString) = True Then
                                RNode.Cells(AssetColumn("AssetLocation")).Text = CType(PlugInData.stations(Job.OutputLocationID.ToString), Station).stationName
                                EveLocation = CType(PlugInData.stations(CType(PlugInData.stations(Job.OutputLocationID.ToString), Station).systemID.ToString), SolarSystem)
                            Else
                                RNode.Cells(AssetColumn("AssetLocation")).Text = "POS in " & CType(PlugInData.stations(Job.InstalledInSolarSystemID.ToString), SolarSystem).Name
                                EveLocation = CType(PlugInData.stations(Job.InstalledInSolarSystemID.ToString), SolarSystem)
                            End If
                            If EveLocation IsNot Nothing Then
                                RNode.Cells(AssetColumn("AssetSystem")).Text = EveLocation.Name
                                RNode.Cells(AssetColumn("AssetConstellation")).Text = EveLocation.Constellation
                                RNode.Cells(AssetColumn("AssetRegion")).Text = EveLocation.Region
                                RNode.Cells(AssetColumn("AssetSystemSec")).Text = EveLocation.Security.ToString("N2")
                            Else
                                RNode.Cells(AssetColumn("AssetSystem")).Text = "Unknown"
                                RNode.Cells(AssetColumn("AssetConstellation")).Text = "Unknown"
                                RNode.Cells(AssetColumn("AssetRegion")).Text = "Unknown"
                                RNode.Cells(AssetColumn("AssetSystemSec")).Text = "Unknown"
                            End If
                            RNode.Cells(AssetColumn("AssetMeta")).Text = ResearchItem.MetaLevel.ToString("N0")
                            RNode.Cells(AssetColumn("AssetVolume")).Text = ResearchItem.Volume.ToString("N2")
                            RNode.Cells(AssetColumn("AssetQuantity")).Text = "1"
                            Dim price As Double = 0
                            If Job.ActivityID = 8 Then
                                ' Calculate BPC cost
                                If Settings.PrismSettings.BPCCosts.ContainsKey(Job.InstalledItemTypeID.ToString) Then
                                    Dim pricerange As Double = Settings.PrismSettings.BPCCosts(Job.InstalledItemTypeID.ToString).MaxRunCost - Settings.PrismSettings.BPCCosts(Job.InstalledItemTypeID.ToString).MinRunCost
                                    Dim runrange As Integer = PlugInData.Blueprints(Job.InstalledItemTypeID.ToString).MaxProdLimit - 1
                                    If runrange = 0 Then
                                        price = Settings.PrismSettings.BPCCosts(Job.InstalledItemTypeID.ToString).MinRunCost
                                    Else
                                        price = Settings.PrismSettings.BPCCosts(Job.InstalledItemTypeID.ToString).MinRunCost + Math.Round((pricerange / runrange) * (Job.InstalledRuns - 1), 2)
                                    End If
                                End If
                            Else
                                price = EveHQ.Core.DataFunctions.GetPrice(Job.InstalledItemTypeID.ToString)
                            End If
                            ResearchValue += price
                            RNode.Cells(AssetColumn("AssetPrice")).Text = price.ToString("N2")
                            RNode.Cells(AssetColumn("AssetValue")).Text = price.ToString("N2")
                        End If
                        ' Check for manufacturing job and store the output items
                        If Job.ActivityID = 1 Then
                            ResearchValue += Me.DisplayResearchOutput(ResearchNode, Job, owner)
                        End If
                    End If
                Next
            End If
        Next
        ResearchNode.Cells(AssetColumn("AssetValue")).Text = ResearchValue.ToString("N2")
        If ResearchNode.Nodes.Count > 0 Then
            adtAssets.Nodes.Add(ResearchNode)
        End If
        totalAssetValue += ResearchValue
    End Sub
    Private Function DisplayResearchOutput(ByVal ResearchNode As Node, ByVal Job As IndustryJob, ByVal Owner As String) As Double
        Dim RNode As New Node
        For NewCell As Integer = 1 To NumberOfActiveColumns : RNode.Cells.Add(New Cell) : Next
        RNode.Tag = Job.OutputTypeID.ToString

        Dim ResearchItem As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(Job.OutputTypeID.ToString)
        Dim category, group As String
        Dim EveLocation As SolarSystem
        category = EveHQ.Core.HQ.itemCats(ResearchItem.Category.ToString)
        group = EveHQ.Core.HQ.itemGroups(ResearchItem.Group.ToString)
        ' Check for search criteria
        If Not ((filters.Count > 0 And catFilters.Contains(category) = False And groupFilters.Contains(group) = False) Or (searchText <> "" And ResearchItem.Name.ToLower.Contains(searchText.ToLower) = False)) Then
            ResearchNode.Nodes.Add(RNode)
            RNode.Text = ResearchItem.Name
            RNode.Cells(AssetColumn("AssetOwner")).Text = Owner
            RNode.Cells(AssetColumn("AssetGroup")).Text = group
            RNode.Cells(AssetColumn("AssetCategory")).Text = category
            If PlugInData.stations.ContainsKey(Job.OutputLocationID.ToString) = True Then
                RNode.Cells(AssetColumn("AssetLocation")).Text = CType(PlugInData.stations(Job.OutputLocationID.ToString), Station).stationName
                EveLocation = CType(PlugInData.stations(CType(PlugInData.stations(Job.OutputLocationID.ToString), Station).systemID.ToString), SolarSystem)
            Else
                RNode.Cells(AssetColumn("AssetLocation")).Text = "POS in " & CType(PlugInData.stations(Job.InstalledInSolarSystemID.ToString), SolarSystem).Name
                EveLocation = CType(PlugInData.stations(Job.InstalledInSolarSystemID.ToString), SolarSystem)
            End If
            If EveLocation IsNot Nothing Then
                RNode.Cells(AssetColumn("AssetSystem")).Text = EveLocation.Name
                RNode.Cells(AssetColumn("AssetConstellation")).Text = EveLocation.Constellation
                RNode.Cells(AssetColumn("AssetRegion")).Text = EveLocation.Region
                RNode.Cells(AssetColumn("AssetSystemSec")).Text = EveLocation.Security.ToString("N2")
            Else
                RNode.Cells(AssetColumn("AssetSystem")).Text = "Unknown"
                RNode.Cells(AssetColumn("AssetConstellation")).Text = "Unknown"
                RNode.Cells(AssetColumn("AssetRegion")).Text = "Unknown"
                RNode.Cells(AssetColumn("AssetSystemSec")).Text = "Unknown"
            End If
            RNode.Cells(AssetColumn("AssetMeta")).Text = ResearchItem.MetaLevel.ToString("N0")
            RNode.Cells(AssetColumn("AssetVolume")).Text = ResearchItem.Volume.ToString("N2")
            RNode.Cells(AssetColumn("AssetQuantity")).Text = (Job.Runs * ResearchItem.PortionSize).ToString("N0")
            Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(Job.OutputTypeID.ToString)
            Dim value As Double = Job.Runs * ResearchItem.PortionSize * price
            RNode.Cells(AssetColumn("AssetPrice")).Text = price.ToString("N2")
            RNode.Cells(AssetColumn("AssetValue")).Text = value.ToString("N2")
            Return value
        End If
    End Function
    Private Sub DisplayContracts()
        ' Add the balances to the assets schedule
        Dim ContractsNode As New Node
        Dim ContractsValue As Double
        ContractsNode.Tag = "Contracts"
        ContractsNode.Text = "Contracts"
        For NewCell As Integer = 1 To NumberOfActiveColumns : ContractsNode.Cells.Add(New Cell) : Next

        For Each cPilot As ListViewItem In PSCAssetOwners.ItemList.CheckedItems
            ' Get the owner we will use
            Dim owner As String = cPilot.Text
            ' Get the orders
            Dim ContractsCollection As SortedList(Of Long, Contract) = Contracts.ParseContracts(owner)
            If ContractsCollection IsNot Nothing Then
                ' Add the orders (outstanding ones only)
                Dim ItemName, category, group, meta, vol As String
                Dim EveLocation As SolarSystem
                For Each OwnerContract As Contract In ContractsCollection.Values
                    If OwnerContract.Status = ContractStatuses.Outstanding Then
                        Dim ContractNode As New Node
                        For NewCell As Integer = 1 To NumberOfActiveColumns : ContractNode.Cells.Add(New Cell) : Next
                        ContractsNode.Nodes.Add(ContractNode)
                        ContractNode.Tag = OwnerContract.ContractID
                        If OwnerContract.Title <> "" Then
                            ContractNode.Text = OwnerContract.Title
                        Else
                            ContractNode.Text = "Contract ID: " & OwnerContract.ContractID
                        End If
                        ContractNode.Text &= " (" & OwnerContract.Type.ToString & ")"
                        ContractNode.Cells(AssetColumn("AssetOwner")).Text = owner
                        If PlugInData.stations.Contains(OwnerContract.StartStationID.ToString) = True Then
                            ContractNode.Cells(AssetColumn("AssetLocation")).Text = CType(PlugInData.stations(OwnerContract.StartStationID.ToString), Station).stationName
                            EveLocation = CType(PlugInData.stations(CType(PlugInData.stations(OwnerContract.StartStationID.ToString), Station).systemID.ToString), SolarSystem)
                        Else
                            ContractNode.Cells(AssetColumn("AssetLocation")).Text = "StationID: " & OwnerContract.StartStationID.ToString
                            EveLocation = Nothing
                        End If
                        If EveLocation IsNot Nothing Then
                            ContractNode.Cells(AssetColumn("AssetSystem")).Text = EveLocation.Name
                            ContractNode.Cells(AssetColumn("AssetConstellation")).Text = EveLocation.Constellation
                            ContractNode.Cells(AssetColumn("AssetRegion")).Text = EveLocation.Region
                            ContractNode.Cells(AssetColumn("AssetSystemSec")).Text = EveLocation.Security.ToString("N2")
                        Else
                            ContractNode.Cells(AssetColumn("AssetSystem")).Text = "Unknown"
                            ContractNode.Cells(AssetColumn("AssetConstellation")).Text = "Unknown"
                            ContractNode.Cells(AssetColumn("AssetRegion")).Text = "Unknown"
                            ContractNode.Cells(AssetColumn("AssetSystemSec")).Text = "Unknown"
                        End If

                        Dim ContractValue As Double = 0

                        For Each typeID As String In OwnerContract.Items.Keys
                            If EveHQ.Core.HQ.itemData.ContainsKey(typeID) = True Then
                                Dim orderItem As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(typeID)
                                ItemName = orderItem.Name
                                category = EveHQ.Core.HQ.itemCats(orderItem.Category.ToString)
                                group = EveHQ.Core.HQ.itemGroups(orderItem.Group.ToString)
                                meta = orderItem.MetaLevel.ToString
                                vol = orderItem.Volume.ToString
                            Else
                                ItemName = "ItemID: " & typeID
                                category = "<Unknown>"
                                group = "<Unknown>"
                                meta = "0"
                                vol = "1"
                            End If

                            ' Check for search criteria
                            If Not ((filters.Count > 0 And catFilters.Contains(category) = False And groupFilters.Contains(group) = False) Or (searchText <> "" And ItemName.ToLower.Contains(searchText.ToLower) = False)) Then
                                Dim ItemNode As New Node
                                For NewCell As Integer = 1 To NumberOfActiveColumns : ItemNode.Cells.Add(New Cell) : Next
                                ItemNode.Text = ItemName
                                ItemNode.Tag = typeID
                                ContractNode.Nodes.Add(ItemNode)
                                ItemNode.Cells(AssetColumn("AssetOwner")).Text = owner
                                ItemNode.Cells(AssetColumn("AssetGroup")).Text = group
                                ItemNode.Cells(AssetColumn("AssetCategory")).Text = category
                                If PlugInData.stations.Contains(OwnerContract.StartStationID.ToString) = True Then
                                    ItemNode.Cells(AssetColumn("AssetLocation")).Text = CType(PlugInData.stations(OwnerContract.StartStationID.ToString), Station).stationName
                                    EveLocation = CType(PlugInData.stations(CType(PlugInData.stations(OwnerContract.StartStationID.ToString), Station).systemID.ToString), SolarSystem)
                                Else
                                    ItemNode.Cells(AssetColumn("AssetLocation")).Text = "StationID: " & OwnerContract.StartStationID.ToString
                                    EveLocation = Nothing
                                End If
                                If EveLocation IsNot Nothing Then
                                    ItemNode.Cells(AssetColumn("AssetSystem")).Text = EveLocation.Name
                                    ItemNode.Cells(AssetColumn("AssetConstellation")).Text = EveLocation.Constellation
                                    ItemNode.Cells(AssetColumn("AssetRegion")).Text = EveLocation.Region
                                    ItemNode.Cells(AssetColumn("AssetSystemSec")).Text = EveLocation.Security.ToString("N2")
                                Else
                                    ItemNode.Cells(AssetColumn("AssetSystem")).Text = "Unknown"
                                    ItemNode.Cells(AssetColumn("AssetConstellation")).Text = "Unknown"
                                    ItemNode.Cells(AssetColumn("AssetRegion")).Text = "Unknown"
                                    ItemNode.Cells(AssetColumn("AssetSystemSec")).Text = "Unknown"
                                End If
                                ItemNode.Cells(AssetColumn("AssetMeta")).Text = meta
                                ItemNode.Cells(AssetColumn("AssetVolume")).Text = CDbl(vol).ToString("N2")
                                ItemNode.Cells(AssetColumn("AssetQuantity")).Text = OwnerContract.Items(typeID).ToString("N0")
                                Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(typeID)
                                ItemNode.Cells(AssetColumn("AssetPrice")).Text = price.ToString("N2")
                                ItemNode.Cells(AssetColumn("AssetValue")).Text = (OwnerContract.Items(typeID) * price).ToString("N2")
                                ContractValue += OwnerContract.Items(typeID) * price
                            End If

                        Next
                        ContractNode.Cells(AssetColumn("AssetValue")).Text = ContractValue.ToString("N2")
                    End If
                Next
            End If
        Next
        ContractsNode.Cells(AssetColumn("AssetValue")).Text = ContractsValue.ToString("N2")
        totalAssetValue += ContractsValue
        If ContractsNode.Nodes.Count > 0 Then
            adtAssets.Nodes.Add(ContractsNode)
        End If
    End Sub

    Private Sub ParseContracts()

    End Sub

#End Region

#Region "Asset Context Menu & UI Routines"
    Private Sub ctxAssets_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxAssets.Opening
        If adtAssets.SelectedNodes.Count > 0 Then
            If adtAssets.SelectedNodes.Count = 1 Then
                If adtAssets.SelectedNodes(0).Cells(AssetColumn("AssetOwner")).Tag IsNot Nothing Then
                    Dim itemName As String = adtAssets.SelectedNodes(0).Cells(AssetColumn("AssetOwner")).Tag.ToString
                    Dim itemText As String = adtAssets.SelectedNodes(0).Text
                    Dim itemID As String = adtAssets.SelectedNodes(0).Tag.ToString
                    If itemName <> "Cash Balances" And itemName <> "Investments" Then
                        If EveHQ.Core.HQ.itemList.ContainsKey(itemName) = True And itemName <> "Office" And adtAssets.SelectedNodes(0).Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                            mnuItemName.Text = itemName
                            mnuItemName.Tag = EveHQ.Core.HQ.itemList(itemName)
                            mnuAddCustomName.Visible = True
                            mnuViewAssetID.Visible = True
                            mnuViewInIB.Visible = True
                            mnuModifyPrice.Visible = True
                            mnuToolSep.Visible = True
                            mnuRecycleItem.Enabled = True
                            If adtAssets.SelectedNodes(0).Cells(AssetColumn("AssetCategory")).Text = "Ship" Then
                                mnuViewInHQF.Visible = True
                            Else
                                mnuViewInHQF.Visible = False
                            End If
                            If adtAssets.SelectedNodes(0).Nodes.Count > 0 Then
                                mnuRecycleContained.Enabled = True
                                mnuRecycleAll.Enabled = True
                            Else
                                mnuRecycleContained.Enabled = False
                                mnuRecycleAll.Enabled = False
                            End If
                            If PlugInData.AssetItemNames.ContainsKey(itemID) = True Then
                                mnuAddCustomName.Text = "Edit Custom Name"
                                mnuRemoveCustomName.Visible = True
                            Else
                                mnuAddCustomName.Text = "Add Custom Name"
                                mnuRemoveCustomName.Visible = False
                            End If
                            mnuAddCustomName.Tag = itemID
                            mnuFilterSep.Visible = True
                            mnuAddItemToFilter.Visible = True
                            mnuAddGroupToFilter.Visible = True
                            mnuAddCategoryToFilter.Visible = True
                        Else
                            mnuItemName.Text = itemName
                            mnuAddCustomName.Visible = False
                            mnuRemoveCustomName.Visible = False
                            mnuViewAssetID.Visible = False
                            mnuViewInIB.Visible = False
                            mnuViewInHQF.Visible = False
                            mnuModifyPrice.Visible = False
                            mnuToolSep.Visible = False
                            mnuFilterSep.Visible = False
                            mnuAddItemToFilter.Visible = False
                            mnuAddGroupToFilter.Visible = False
                            mnuAddCategoryToFilter.Visible = False
                            If adtAssets.SelectedNodes(0).Nodes.Count > 0 Then
                                mnuRecycleItem.Enabled = False
                                mnuRecycleContained.Enabled = True
                                mnuRecycleAll.Enabled = False
                            Else
                                e.Cancel = True
                            End If
                        End If
                    Else
                        e.Cancel = True
                    End If
                Else
                    e.Cancel = True
                End If
            Else
                mnuItemName.Text = "Multiple Items"
                mnuItemName.Tag = ""
                mnuAddCustomName.Visible = False
                mnuViewAssetID.Visible = False
                mnuViewInIB.Visible = False
                mnuViewInHQF.Visible = False
                mnuModifyPrice.Visible = False
                mnuToolSep.Visible = False
                Dim containerItems As Boolean = False
                For Each item As Node In adtAssets.SelectedNodes
                    If item.Nodes.Count > 0 Then
                        containerItems = True
                        Exit For
                    End If
                Next
                If containerItems = True Then
                    mnuRecycleContained.Enabled = True
                    mnuRecycleAll.Enabled = True
                Else
                    mnuRecycleContained.Enabled = False
                    mnuRecycleAll.Enabled = False
                End If
                mnuRecycleItem.Enabled = True
                mnuFilterSep.Visible = False
                mnuAddItemToFilter.Visible = False
                mnuAddGroupToFilter.Visible = False
                mnuAddCategoryToFilter.Visible = False
            End If
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuViewInIB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewInIB.Click
        ' This routine is shit hot!!
        Dim PluginName As String = "EveHQ Item Browser"
        Dim itemID As String = mnuItemName.Tag.ToString
        Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
        If myPlugIn.Status = EveHQ.Core.PlugIn.PlugInStatus.Active Then
            Dim mainTab As DevComponents.DotNetBar.TabStrip = CType(EveHQ.Core.HQ.MainForm.Controls("tabEveHQMDI"), DevComponents.DotNetBar.TabStrip)
            Dim tp As DevComponents.DotNetBar.TabItem = EveHQ.Core.HQ.GetMDITab(PluginName)
            If tp IsNot Nothing Then
                mainTab.SelectedTab = tp
            Else
                Dim plugInForm As Form = myPlugIn.Instance.RunEveHQPlugIn
                plugInForm.MdiParent = EveHQ.Core.HQ.MainForm
                plugInForm.Show()
            End If
            myPlugIn.Instance.GetPlugInData(itemID, 0)
        Else
            ' Plug-in is not loaded so best not try to access it!
            Dim msg As String = ""
            msg &= "The " & myPlugIn.MainMenuText & " Plug-in is not currently active." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Please load the plug-in before proceeding."
            MessageBox.Show(msg, "Error Starting " & myPlugIn.MainMenuText & " Plug-in!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub
    Private Sub mnuModifyPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuModifyPrice.Click
        Dim newPrice As New EveHQ.Core.frmModifyPrice(mnuItemName.Tag.ToString, 0)
        newPrice.ShowDialog()
    End Sub
    Private Sub mnuViewInHQF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewInHQF.Click
        If adtAssets.SelectedNodes.Count > 0 Then
            Dim assetID As String = adtAssets.SelectedNodes(0).Tag.ToString
            Dim shipName As String = adtAssets.SelectedNodes(0).Text
            Dim owner As String = adtAssets.SelectedNodes(0).Cells(AssetColumn("AssetOwner")).Text
            HQFShip = New ArrayList
            Call Me.SearchForShip(assetID, owner)
            ' Should have got the ship by now
            HQFShip.Sort()
            Dim FittedMods As New SortedList
            Dim Drones As New SortedList
            Dim ChargedMod(1) As String
            For Each fittedMod As String In HQFShip
                Dim modData() As String = fittedMod.Split(",".ToCharArray)
                If FittedMods.Contains(modData(0)) = False Then
                    ReDim ChargedMod(1)
                    If modData(3) = "8" Then ' Is Charge
                        ChargedMod(1) = modData(1)
                    Else
                        ChargedMod(0) = modData(1)
                    End If
                    FittedMods.Add(modData(0), ChargedMod)
                Else
                    ChargedMod = CType(FittedMods(modData(0)), String())
                    If modData(3) = "8" Then ' Is Charge
                        ChargedMod(1) = modData(1)
                    Else
                        ChargedMod(0) = modData(1)
                    End If
                    FittedMods(modData(0)) = ChargedMod
                End If
                If modData(3) = "18" Then ' Is Drone
                    If Drones.Contains(modData(1)) = True Then
                        Dim CDQ As Integer = CInt(modData(2))
                        Dim TDQ As Integer = CInt(Drones(modData(1)))
                        Drones(modData(1)) = (TDQ + CDQ).ToString
                    Else
                        Drones.Add(modData(1), modData(2))
                    End If
                End If
            Next
            Dim list As New StringBuilder
            list.AppendLine("[" & shipName & "," & owner & "'s " & shipName & "]")
            For Each fittedMod As String In HQFShip
                Dim modData() As String = fittedMod.Split(",".ToCharArray)
                Select Case modData(0)
                    Case "Drone Bay"
                        ' Ignore as we will be adding them later
                        '    list.AppendLine(modData(1) & ", " & modData(2) & "i")
                    Case "Cargo"
                        If modData(3) = "8" Then
                            list.AppendLine(modData(1) & ", " & modData(2))
                        End If
                    Case Else
                        If FittedMods.Contains(modData(0)) = True Then
                            ChargedMod = CType(FittedMods(modData(0)), String())
                            If ChargedMod(1) = "" Then
                                list.AppendLine(ChargedMod(0))
                            Else
                                list.AppendLine(ChargedMod(0) & ", " & ChargedMod(1))
                            End If
                            FittedMods.Remove(modData(0))
                        End If
                End Select
            Next
            ' Add Drones
            For Each drone As String In Drones.Keys
                list.AppendLine(drone & ", " & Drones(drone).ToString & "i")
            Next
            Clipboard.SetText(list.ToString)
            MessageBox.Show(list.ToString, "Copy To Clipboard Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub SearchForShip(ByVal assetID As String, ByVal ShipOwner As String)

        Dim Owner As New PrismOwner

        For Each cOwner As ListViewItem In PSCAssetOwners.ItemList.CheckedItems
            If cOwner.Text = ShipOwner Then
                If PlugInData.PrismOwners.ContainsKey(cOwner.Text) = True Then
                    Owner = PlugInData.PrismOwners(cOwner.Text)
                    Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Assets)
                    Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Assets)

                    If OwnerAccount IsNot Nothing Then

                        Dim AssetXML As New XmlDocument
                        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                        If Owner.IsCorp = True Then
                            AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        Else
                            AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                        End If

                        If AssetXML IsNot Nothing Then

                            If AssetXML IsNot Nothing Then
                                Dim locList As XmlNodeList
                                Dim loc As XmlNode
                                locList = AssetXML.SelectNodes("/eveapi/result/rowset/row")
                                If locList.Count > 0 Then
                                    For Each loc In locList
                                        ' Let's search for our asset!
                                        If loc.Attributes.GetNamedItem("itemID").Value = assetID Then
                                            ' We found our ship so extract the subitem data
                                            Dim groupID As String = ""
                                            Dim catID As String = ""
                                            Dim modList As XmlNodeList
                                            Dim mods As XmlNode
                                            If loc.ChildNodes.Count > 0 Then
                                                modList = loc.ChildNodes(0).ChildNodes
                                                For Each mods In modList
                                                    Dim itemID As String = mods.Attributes.GetNamedItem("typeID").Value
                                                    Dim itemName As String = ""
                                                    If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                                        Dim itemData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(itemID)
                                                        itemName = itemData.Name
                                                        groupID = itemData.Group.ToString
                                                        catID = itemData.Category.ToString
                                                        Dim flagID As Integer = CInt(mods.Attributes.GetNamedItem("flag").Value)
                                                        Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                                                        Dim quantity As String = mods.Attributes.GetNamedItem("quantity").Value
                                                        HQFShip.Add(flagName & "," & itemName & "," & quantity & "," & catID)
                                                    Else
                                                        ' Can't find the item in the database
                                                        itemName = "ItemID: " & itemID.ToString
                                                    End If
                                                Next
                                            End If
                                            Exit Sub
                                        Else
                                            ' Check if this row has child nodes and repeat
                                            If loc.HasChildNodes = True Then
                                                Call Me.SearchForShipNode(loc, assetID)
                                                If HQFShip.Count > 0 Then Exit Sub
                                            End If
                                        End If
                                    Next
                                End If
                            End If

                        End If

                    End If
                End If
            End If
        Next

    End Sub
    Private Sub SearchForShipNode(ByVal loc As XmlNode, ByVal assetID As String)
        Dim subLocList As XmlNodeList
        Dim subLoc As XmlNode
        subLocList = loc.ChildNodes(0).ChildNodes
        For Each subLoc In subLocList
            ' Let's search for our asset!
            Try
                If subLoc.Attributes.GetNamedItem("itemID").Value = assetID Then
                    ' We found our ship so extract the subitem data
                    Dim groupID As String = ""
                    Dim catID As String = ""
                    Dim modList As XmlNodeList
                    Dim mods As XmlNode
                    If subLoc.HasChildNodes = True Then
                        modList = subLoc.ChildNodes(0).ChildNodes
                        For Each mods In modList
                            Dim itemID As String = mods.Attributes.GetNamedItem("typeID").Value
                            Dim itemName As String = ""
                            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                                Dim itemData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(itemID)
                                itemName = itemData.Name
                                groupID = itemData.Group.ToString
                                catID = itemData.Category.ToString
                                Dim flagID As Integer = CInt(mods.Attributes.GetNamedItem("flag").Value)
                                Dim flagName As String = PlugInData.itemFlags(flagID).ToString
                                Dim quantity As String = mods.Attributes.GetNamedItem("quantity").Value
                                HQFShip.Add(flagName & "," & itemName & "," & quantity & "," & catID)
                            Else
                                ' Can't find the item in the database
                                itemName = "ItemID: " & itemID.ToString
                            End If
                        Next
                    End If
                    Exit Sub
                Else
                    ' Check if this row has child nodes and repeat
                    If subLoc.HasChildNodes = True Then
                        Call Me.SearchForShipNode(subLoc, assetID)
                    End If
                End If
            Catch ex As Exception
                Dim msg As String = "Unable to parse Asset:" & ControlChars.CrLf
                msg &= "InnerXML: " & subLoc.InnerXml & ControlChars.CrLf
                msg &= "InnerText: " & subLoc.InnerText & ControlChars.CrLf
                msg &= "TypeID: " & subLoc.Attributes.GetNamedItem("typeID").Value
                MessageBox.Show(msg, "Error Getting Ship Information!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        Next
    End Sub
    Private Sub adtAssets_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtAssets.SelectionChanged
        If adtAssets.SelectedNodes.Count > 0 Then
            Dim volume, lineValue, value As Double
            Dim chkParent As New Node
            Dim parentFlag As Boolean = False
            For Each asset As Node In adtAssets.SelectedNodes
                parentFlag = False
                chkParent = asset.Parent
                Do While chkParent IsNot Nothing
                    If chkParent.IsSelected = True Then
                        parentFlag = True
                        Exit Do
                    End If
                    chkParent = chkParent.Parent
                Loop
                If parentFlag = False Then
                    If asset.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                        If asset.Cells(AssetColumn("AssetVolume")).Text <> "" Then
                            volume += CDbl(asset.Cells(AssetColumn("AssetVolume")).Text)
                        End If
                        lineValue = CDbl(asset.Cells(AssetColumn("AssetValue")).Text)
                        value += lineValue
                    Else
                        If asset.Cells(AssetColumn("AssetValue")).Text <> "" Then
                            If asset.Cells(AssetColumn("AssetVolume")).Text <> "" Then
                                volume += CDbl(asset.Cells(AssetColumn("AssetVolume")).Text)
                            End If
                            lineValue = CDbl(asset.Cells(AssetColumn("AssetValue")).Text)
                            value += lineValue
                        End If
                    End If
                End If
            Next
            lblTotalSelectedAssetValue.Text = "Total Selected Assets: Volume = " & volume.ToString("N2") & " m³ : Value = " & value.ToString("N2") & " ISK"
        Else
            lblTotalSelectedAssetValue.Text = "Total Selected Assets: n/a"
        End If
    End Sub
    Private Sub mnuAddCustomName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCustomName.Click
        Dim assetID As String = mnuAddCustomName.Tag.ToString
        Dim assetName As String = mnuItemName.Text
        Dim newCustomName As New frmAssetItemName
        If PlugInData.AssetItemNames.ContainsKey(assetID) = True Then
            newCustomName.Text = "Edit Custom Asset Name"
            newCustomName.EditMode = True
        Else
            newCustomName.Text = "Add Custom Asset Name"
            newCustomName.EditMode = False
        End If
        newCustomName.AssetID = assetID
        newCustomName.AssetName = assetName
        newCustomName.ShowDialog()
        If newCustomName.AssetItemName <> "" Then
            adtAssets.SelectedNodes(0).Text = newCustomName.AssetItemName
        End If
        newCustomName.Dispose()
    End Sub
    Private Sub mnuRemoveCustomName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveCustomName.Click
        Dim assetID As String = mnuAddCustomName.Tag.ToString
        Dim itemName As String = mnuItemName.Text
        Dim assetSQL As String = "DELETE FROM assetItemNames WHERE itemID=" & assetID & ";"
        If EveHQ.Core.DataFunctions.SetData(assetSQL) = False Then
            MessageBox.Show("There was an error deleting the record from the Asset Item Names database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & assetSQL, "Error Writing Asset Name Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            PlugInData.AssetItemNames.Remove(assetID)
            adtAssets.SelectedNodes(0).Text = itemName
        End If
    End Sub
    Private Sub adtAssets_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtAssets.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub
#End Region

#Region "Filter, Owner and Search Routines"
    Private Sub LoadFilterGroups()
        Dim newNode As TreeNode
        tvwFilter.BeginUpdate()
        tvwFilter.Nodes.Clear()
        ' Load up the filter with categories
        For Each cat As String In EveHQ.Core.HQ.itemCats.Keys
            newNode = New TreeNode
            newNode.Name = cat
            newNode.Text = EveHQ.Core.HQ.itemCats(cat)
            tvwFilter.Nodes.Add(newNode)
        Next
        ' Load up the filter with groups
        For Each group As String In EveHQ.Core.HQ.itemGroups.Keys
            newNode = New TreeNode
            newNode.Name = group
            newNode.Text = CStr(EveHQ.Core.HQ.itemGroups(group))
            tvwFilter.Nodes(EveHQ.Core.HQ.groupCats(newNode.Name)).Nodes.Add(newNode)
        Next
        ' Update the filter
        tvwFilter.Sorted = True
        tvwFilter.EndUpdate()
    End Sub
    Private Sub FilterTree()
        Dim cL As Integer = 0
        Dim cLoc As Node
        If adtAssets.Nodes.Count > 0 Then
            Do
                cLoc = adtAssets.Nodes(cL)
                If cLoc.Nodes.Count = 0 Then
                    If (filters.Count > 0 And catFilters.Contains(cLoc.Cells(AssetColumn("AssetCategory")).Text) = False And groupFilters.Contains(cLoc.Cells(AssetColumn("AssetGroup")).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        adtAssets.Nodes.Remove(cLoc)
                        assetList.Remove(cLoc.Tag.ToString)
                        totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                        cL -= 1
                    End If
                Else
                    Call FilterNode(cLoc)
                    If cLoc.Nodes.Count = 0 Then
                        If (filters.Count > 0 And catFilters.Contains(cLoc.Cells(AssetColumn("AssetCategory")).Text) = False And groupFilters.Contains(cLoc.Cells(AssetColumn("AssetGroup")).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                            adtAssets.Nodes.Remove(cLoc)
                            If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) = True Then
                                totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                            End If
                            'assetList.Remove(cLoc.Tag)
                            cL -= 1
                        End If
                    Else
                        If (filters.Count > 0 And catFilters.Contains(cLoc.Cells(AssetColumn("AssetCategory")).Text) = False And groupFilters.Contains(cLoc.Cells(AssetColumn("AssetGroup")).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                            If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) = True Then
                                totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                            End If
                            ' Remove quantity and price information
                            cLoc.Cells(AssetColumn("AssetQuantity")).Text = ""
                            cLoc.Cells(AssetColumn("AssetPrice")).Text = ""
                            'assetList.Remove(cLoc.Tag)
                        End If
                    End If
                End If
                cL += 1
            Loop Until (cL = adtAssets.Nodes.Count)
        End If
        Call Me.CalcFilteredPrices()
    End Sub
    Private Sub FilterNode(ByVal pLoc As Node)
        Dim cL As Integer = 0
        Dim cLoc As Node
        Do
            cLoc = pLoc.Nodes(cL)
            If cLoc.Nodes.Count = 0 Then
                If (filters.Count > 0 And catFilters.Contains(cLoc.Cells(AssetColumn("AssetCategory")).Text) = False And groupFilters.Contains(cLoc.Cells(AssetColumn("AssetGroup")).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                    pLoc.Nodes.Remove(cLoc)
                    If cLoc.Tag IsNot Nothing Then
                        assetList.Remove(cLoc.Tag.ToString)
                    End If
                    If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) = True Then
                        totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                    End If
                    cL -= 1
                End If
            Else
                Call FilterNode(cLoc)
                If cLoc.Nodes.Count = 0 Then
                    If (filters.Count > 0 And catFilters.Contains(cLoc.Cells(AssetColumn("AssetCategory")).Text) = False And groupFilters.Contains(cLoc.Cells(AssetColumn("AssetGroup")).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        pLoc.Nodes.Remove(cLoc)
                        If cLoc.Tag IsNot Nothing Then
                            assetList.Remove(cLoc.Tag.ToString)
                        End If
                        If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) = True Then
                            totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                        End If
                        cL -= 1
                    End If
                Else
                    If (filters.Count > 0 And catFilters.Contains(cLoc.Cells(AssetColumn("AssetCategory")).Text) = False And groupFilters.Contains(cLoc.Cells(AssetColumn("AssetGroup")).Text) = False) Or (searchText <> "" And cLoc.Text.ToLower.Contains(searchText.ToLower) = False) Then
                        If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) = True Then
                            totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                        End If
                        ' Remove quantity and price information
                        cLoc.Cells(AssetColumn("AssetQuantity")).Text = ""
                        cLoc.Cells(AssetColumn("AssetPrice")).Text = ""
                        If cLoc.Tag IsNot Nothing Then
                            assetList.Remove(cLoc.Tag.ToString)
                        End If
                    End If
                End If
            End If
            cL += 1
        Loop Until (cL = pLoc.Nodes.Count)
    End Sub
    Private Sub CalcFilteredPrices()
        totalAssetValue = 0
        Dim locPrice As Double = 0
        For Each cLoc As Node In adtAssets.Nodes
            ' Calculate cost of all the sub nodes
            If cLoc.Nodes.Count > 0 Then
                locPrice = Me.CalcNodePrice(cLoc)
                cLoc.Cells(AssetColumn("AssetValue")).Text = locPrice.ToString("N2")
                totalAssetValue += locPrice
            End If
        Next
    End Sub
    Private Function CalcNodePrice(ByVal pLoc As Node) As Double
        Dim lineValue As Double = 0
        Dim contValue As Double = 0
        For Each cLoc As Node In pLoc.Nodes
            If cLoc.Nodes.Count > 0 Then
                Call Me.CalcNodePrice(cLoc)
                lineValue = CDbl(cLoc.Cells(AssetColumn("AssetValue")).Text)
                contValue += lineValue
            Else
                If IsNumeric(cLoc.Cells(AssetColumn("AssetPrice")).Text) = True Then
                    lineValue = CDbl(cLoc.Cells(AssetColumn("AssetQuantity")).Text) * CDbl(cLoc.Cells(AssetColumn("AssetPrice")).Text)
                Else
                    lineValue = 0
                End If
                cLoc.Cells(AssetColumn("AssetValue")).Text = lineValue.ToString("N2")
                contValue += lineValue
            End If
        Next
        pLoc.Cells(AssetColumn("AssetValue")).Text = contValue.ToString("N2")
        Return contValue
    End Function
    Private Sub AddFilter()
        Dim cNode As TreeNode = tvwFilter.SelectedNode
        If filters.Contains(cNode.FullPath) = False Then
            ' Add the filter to the list and display it
            filters.Add(cNode.FullPath)
            lstFilters.Items.Add(cNode.FullPath)
            ' Add to the category filter if a category
            If cNode.Nodes.Count > 0 Then
                catFilters.Add(cNode.Text)
            Else
                groupFilters.Add(cNode.Text)
            End If
        End If
        Call Me.SetGroupFilterLabel()
    End Sub
    Private Sub RemoveFilter()
        ' Check which we are removing
        If lstFilters.SelectedItems.Count = 1 Then
            If lstFilters.SelectedItem.ToString.Contains("\") = True Then
                ' Remove the group
                Dim sID As Integer = InStr(lstFilters.SelectedItem.ToString, "\")
                Dim groupName As String = lstFilters.SelectedItem.ToString.Substring(sID)
                groupFilters.Remove(groupName)
            Else
                ' Remove the category
                catFilters.Remove(lstFilters.SelectedItem.ToString)
            End If
            filters.Remove(lstFilters.SelectedItem.ToString)
            lstFilters.Items.Remove(lstFilters.SelectedItem.ToString)
            Call Me.SetGroupFilterLabel()
        End If
    End Sub
    Private Sub tvwFilter_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwFilter.NodeMouseClick
        tvwFilter.SelectedNode = e.Node
    End Sub
    Private Sub AddToFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddToFilterToolStripMenuItem.Click
        Call Me.AddFilter()
    End Sub
    Private Sub RemoveFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFilterToolStripMenuItem.Click
        Call Me.RemoveFilter()
    End Sub
    Private Sub lstFilters_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstFilters.MouseDoubleClick
        Call Me.RemoveFilter()
    End Sub
    Private Sub tvwFilter_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwFilter.NodeMouseDoubleClick
        Call Me.AddFilter()
    End Sub
    Private Sub SetGroupFilterLabel()
        Dim filter As String = "Group Filter: "
        If lstFilters.Items.Count > 0 Then
            For Each group As String In lstFilters.Items
                filter &= group & ", "
            Next
            filter = filter.TrimEnd(", ".ToCharArray)
        Else
            filter &= "None"
        End If
        lblGroupFilters.Text = filter
    End Sub
    Private Sub btnClearGroupFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearGroupFilters.Click
        ' Remove all items from the list of filters
        lstFilters.Items.Clear()
        ' Remove all items from the group and cat filters
        catFilters.Clear()
        groupFilters.Clear()
        filters.Clear()
        Call Me.SetGroupFilterLabel()
    End Sub
    Private Sub ctxFilterList_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxFilterList.Opening
        If lstFilters.SelectedItems.Count = 0 Then
            e.Cancel = True
        End If
    End Sub
    Private Sub FilterSystemValue()
        Dim minValue As Double
        If Double.TryParse(txtMinSystemValue.Text, minValue) = False Then
            minValue = 0
        End If
        Dim cL As Integer = 0
        Dim cLoc As Node
        If adtAssets.Nodes.Count > 0 Then
            Do
                cLoc = adtAssets.Nodes(cL)
                If CDbl(cLoc.Cells(AssetColumn("AssetValue")).Text) < minValue Then
                    Call FilterSystemNode(cLoc)
                    If cLoc.Nodes.Count = 0 Then
                        adtAssets.Nodes.Remove(cLoc)
                        cL -= 1
                    End If
                End If
                cL += 1
            Loop Until (cL = adtAssets.Nodes.Count)
        End If
        Call Me.RecalcAllPrices()
    End Sub
    Private Sub FilterSystemNode(ByVal pLoc As Node)
        Dim cL As Integer = 0
        Dim cLoc As Node
        Do
            cLoc = pLoc.Nodes(cL)
            If cLoc.Nodes.Count = 0 Then
                pLoc.Nodes.Remove(cLoc)
                assetList.Remove(cLoc.Tag.ToString)
                If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) Then
                    totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                End If
                cL -= 1
            Else
                Call FilterSystemNode(cLoc)
                If cLoc.Nodes.Count = 0 Then
                    pLoc.Nodes.Remove(cLoc)
                    assetList.Remove(cLoc.Tag.ToString)
                    If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) Then
                        totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                    End If
                    cL -= 1
                Else
                    If IsNumeric(cLoc.Cells(AssetColumn("AssetQuantity")).Text) = True Then
                        totalAssetCount -= CLng(cLoc.Cells(AssetColumn("AssetQuantity")).Text)
                    End If
                    ' Remove quantity and price information
                    cLoc.Cells(AssetColumn("AssetQuantity")).Text = ""
                    cLoc.Cells(AssetColumn("AssetPrice")).Text = ""
                    assetList.Remove(cLoc.Tag.ToString)
                End If
            End If
            cL += 1
        Loop Until (cL = pLoc.Nodes.Count)
    End Sub
    Private Sub RecalcAllPrices()
        totalAssetValue = 0
        Dim locPrice As Double = 0
        For Each cLoc As Node In adtAssets.Nodes
            ' Calculate cost of all the sub nodes
            If cLoc.Nodes.Count > 0 Then
                locPrice = Me.RecalcNodePrice(cLoc)
                cLoc.Cells(AssetColumn("AssetValue")).Text = locPrice.ToString("N2")
                totalAssetValue += locPrice
            End If
        Next
    End Sub
    Private Function RecalcNodePrice(ByVal pLoc As Node) As Double
        Dim lineValue As Double = 0
        Dim contValue As Double = 0
        For Each cLoc As Node In pLoc.Nodes
            If cLoc.Nodes.Count > 0 Then
                Call Me.RecalcNodePrice(cLoc)
                lineValue = CDbl(cLoc.Cells(AssetColumn("AssetValue")).Text)
                contValue += lineValue
            Else
                If IsNumeric(cLoc.Cells(AssetColumn("AssetPrice")).Text) = True Then
                    lineValue = CDbl(cLoc.Cells(AssetColumn("AssetQuantity")).Text) * CDbl(cLoc.Cells(AssetColumn("AssetPrice")).Text)
                Else
                    lineValue = 0
                End If
                cLoc.Cells(AssetColumn("AssetValue")).Text = lineValue.ToString("N2")
                contValue += lineValue
            End If
        Next
        If IsNumeric(pLoc.Cells(AssetColumn("AssetPrice")).Text) = True Then
            contValue += CDbl(pLoc.Cells(AssetColumn("AssetPrice")).Text)
        End If
        pLoc.Cells(AssetColumn("AssetValue")).Text = contValue.ToString("N2")
        Return contValue
    End Function
    Private Sub mnuAddItemToFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddItemToFilter.Click
        Dim ItemID As String = mnuItemName.Tag.ToString
        Dim ItemData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(ItemID)
        txtSearch.Text = ItemData.Name
        Dim minValue As Double
        If Double.TryParse(txtMinSystemValue.Text, minValue) = True Then
            Call Me.RefreshAssets()
        Else
            MessageBox.Show("Minimum System Value is not a valid number. Please try again!", "Error in Minimum Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuAddGroupToFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddGroupToFilter.Click
        Dim ItemID As String = mnuItemName.Tag.ToString
        Dim ItemData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(ItemID)
        Dim groupName As String = EveHQ.Core.HQ.itemGroups(ItemData.Group.ToString)
        Dim catName As String = EveHQ.Core.HQ.itemCats(ItemData.Category.ToString)
        Dim FullPath As String = catName & "\" & groupName
        Call SetFilterFromMenu(FullPath, True, groupName)
    End Sub
    Private Sub mnuAddCategoryToFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddCategoryToFilter.Click
        Dim ItemID As String = mnuItemName.Tag.ToString
        Dim ItemData As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(ItemID)
        Dim FullPath As String = EveHQ.Core.HQ.itemCats(ItemData.Category.ToString)
        Call SetFilterFromMenu(FullPath, False, FullPath)
    End Sub
    Private Sub SetFilterFromMenu(ByVal FullPath As String, ByVal AddGroup As Boolean, ByVal FilterName As String)
        If filters.Contains(FullPath) = False Then
            ' Add the filter to the list and display it
            filters.Add(FullPath)
            lstFilters.Items.Add(FullPath)
            ' Add to the category filter if a category
            If AddGroup = False Then
                catFilters.Add(FilterName)
            Else
                groupFilters.Add(FilterName)
            End If
        End If
        Call Me.SetGroupFilterLabel()
    End Sub
#End Region

#Region "Item Recycler Routines"

    Private Sub mnuRecycleItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleItem.Click
        Dim recycleList As New SortedList
        Dim assetName As String = ""
        tempAssetList.Clear()
        For Each asset As Node In adtAssets.SelectedNodes
            assetName = asset.Cells(AssetColumn("AssetOwner")).Tag.ToString
            If recycleList.ContainsKey(EveHQ.Core.HQ.itemList(assetName)) = True Then
                If asset.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                    If tempAssetList.Contains(asset.Tag.ToString) = False Then
                        recycleList(EveHQ.Core.HQ.itemList(assetName)) = CLng(recycleList(EveHQ.Core.HQ.itemList(assetName))) + CLng(asset.Cells(AssetColumn("AssetQuantity")).Text)
                        tempAssetList.Add(asset.Tag.ToString)
                    End If
                End If
            Else
                If asset.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                    If tempAssetList.Contains(asset.Tag.ToString) = False Then
                        recycleList.Add(EveHQ.Core.HQ.itemList(assetName), CLng(asset.Cells(AssetColumn("AssetQuantity")).Text))
                        tempAssetList.Add(asset.Tag.ToString)
                    End If
                End If
            End If
        Next
        tempAssetList.Clear()
        ' Set properties before triggering the event
        cRecyclingAssetList = recycleList
        cRecyclingAssetLocation = adtAssets.SelectedNodes(0)
        PrismEvents.StartRecyclingInfoAvailable()
    End Sub

    Private Sub mnuRecycleContained_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleContained.Click
        Dim recycleList As New SortedList
        tempAssetList.Clear()
        For Each asset As Node In adtAssets.SelectedNodes
            Call Me.AddItemsToRecycleList(asset, recycleList)
        Next
        tempAssetList.Clear()
        ' Set properties before triggering the event
        cRecyclingAssetList = recycleList
        cRecyclingAssetLocation = adtAssets.SelectedNodes(0)
        PrismEvents.StartRecyclingInfoAvailable()
    End Sub

    Private Sub mnuRecycleAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRecycleAll.Click
        Dim recycleList As New SortedList
        Dim assetName As String = ""
        tempAssetList.Clear()
        For Each asset As Node In adtAssets.SelectedNodes
            If asset.Cells(AssetColumn("AssetOwner")).Tag IsNot Nothing Then
                assetName = asset.Cells(AssetColumn("AssetOwner")).Tag.ToString
                If EveHQ.Core.HQ.itemList.ContainsKey(assetName) = True Then
                    If recycleList.ContainsKey(EveHQ.Core.HQ.itemList(assetName)) = True Then
                        If asset.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                            If tempAssetList.Contains(asset.Tag.ToString) = False Then
                                recycleList(EveHQ.Core.HQ.itemList(assetName)) = CLng(recycleList(EveHQ.Core.HQ.itemList(assetName))) + CLng(asset.Cells(AssetColumn("AssetQuantity")).Text)
                                tempAssetList.Add(asset.Tag.ToString)
                            End If
                        End If
                    Else
                        If asset.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                            If tempAssetList.Contains(asset.Tag.ToString) = False Then
                                recycleList.Add(EveHQ.Core.HQ.itemList(assetName), CLng(asset.Cells(AssetColumn("AssetQuantity")).Text))
                                tempAssetList.Add(asset.Tag.ToString)
                            End If
                        End If
                    End If
                End If
                Call Me.AddItemsToRecycleList(asset, recycleList)
            End If
        Next
        tempAssetList.Clear()
        ' Set properties before triggering the event
        cRecyclingAssetList = recycleList
        cRecyclingAssetLocation = adtAssets.SelectedNodes(0)
        PrismEvents.StartRecyclingInfoAvailable()
    End Sub

    Private Sub AddItemsToRecycleList(ByVal item As Node, ByRef assetList As SortedList)
        For Each childItem As Node In item.Nodes
            If EveHQ.Core.HQ.itemList.ContainsKey(childItem.Text) = True Then
                If assetList.ContainsKey(EveHQ.Core.HQ.itemList(childItem.Text)) = True Then
                    If childItem.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                        If tempAssetList.Contains(childItem.Tag.ToString) = False Then
                            assetList(EveHQ.Core.HQ.itemList(childItem.Text)) = CLng(assetList(EveHQ.Core.HQ.itemList(childItem.Text))) + CLng(childItem.Cells(AssetColumn("AssetQuantity")).Text)
                            tempAssetList.Add(childItem.Tag.ToString)
                        End If
                    End If
                Else
                    If childItem.Cells(AssetColumn("AssetQuantity")).Text <> "" Then
                        If tempAssetList.Contains(childItem.Tag.ToString) = False Then
                            assetList.Add(EveHQ.Core.HQ.itemList(childItem.Text), CLng(childItem.Cells(AssetColumn("AssetQuantity")).Text))
                            tempAssetList.Add(childItem.Tag.ToString)
                        End If
                    End If
                End If
            End If
            If childItem.Nodes.Count > 0 Then
                Call Me.AddItemsToRecycleList(childItem, assetList)
            End If
        Next
    End Sub

#End Region

    Private Sub btnFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilters.Click
        splitterAssets.Expanded = Not splitterAssets.Expanded
    End Sub

    Private Sub PrismAssetsControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Initialise the user defined slot columns
        Call Me.UpdateAssetSlotColumns()
    End Sub

    Private Sub mnuConfigureColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuConfigureColumns.Click
        ' Open options form
        Dim mySettings As New frmPrismSettings
        mySettings.Tag = "nodeAssetColumns"
        mySettings.ShowDialog()
        mySettings = Nothing
        Call Me.RefreshAssets()
    End Sub

    Private Sub mnuViewAssetID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewAssetID.Click
        Dim itemName As String = adtAssets.SelectedNodes(0).Cells(AssetColumn("AssetOwner")).Tag.ToString
        Dim itemText As String = adtAssets.SelectedNodes(0).Text
        Dim itemID As String = adtAssets.SelectedNodes(0).Tag.ToString
        Dim msg As String = "Item Name: " & itemName
        If itemText <> itemName Then
            msg &= " (Named: " & itemText & ")"
        End If
        msg &= ControlChars.CrLf & itemID.ToString & ControlChars.CrLf
        MessageBox.Show(msg, "AssetID Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#Region "Asset Export Routines"

    Private Sub btiItemName_Click(sender As System.Object, e As System.EventArgs) Handles btiItemName.Click
        Call ExportAssets(ExportTypes.TypeName)
    End Sub

    Private Sub btiQuantity_Click(sender As System.Object, e As System.EventArgs) Handles btiQuantity.Click
        Call ExportAssets(ExportTypes.Quantity)
    End Sub

    Private Sub btiPrice_Click(sender As System.Object, e As System.EventArgs) Handles btiPrice.Click
        Call ExportAssets(ExportTypes.Price)
    End Sub

    Private Sub btiValue_Click(sender As System.Object, e As System.EventArgs) Handles btiValue.Click
        Call ExportAssets(ExportTypes.Value)
    End Sub

    Private Sub btiVolume_Click(sender As System.Object, e As System.EventArgs) Handles btiVolume.Click
        Call ExportAssets(ExportTypes.Volume)
    End Sub

    Private Sub ExportAssets(ExportType As ExportTypes)

        ' Collect all the information
        Dim AssetExport As New SortedList(Of String, AssetExportResult)
        Dim AER As New AssetExportResult
        For Each Asset As AssetItem In assetList.Values
            If AssetExport.ContainsKey(Asset.typeName) = False Then
                AER = New AssetExportResult
                AER.TypeName = Asset.typeName
                AER.Price = Asset.price
                AssetExport.Add(Asset.typeName, AER)
            Else
                AER = AssetExport(Asset.typeName)
            End If
            AER.Locations += 1
            AER.Quantity += Asset.quantity
            AER.Volume += CDbl(Asset.volume)
            AER.Value += (Asset.price * Asset.quantity)
        Next

        ' Transfer results to something that we can sort
        Dim Assets As New ArrayList
        For Each Asset As AssetExportResult In AssetExport.Values
            Assets.Add(Asset)
        Next

        ' Sort our result depending on the ExportType
        Dim ResultsSorter As New EveHQ.Core.ClassSorter
        Select Case ExportType
            Case ExportTypes.TypeName
                ResultsSorter.SortClasses.Add(New EveHQ.Core.SortClass("TypeName", Core.SortDirection.Ascending))
            Case ExportTypes.Quantity
                ResultsSorter.SortClasses.Add(New EveHQ.Core.SortClass("Quantity", Core.SortDirection.Ascending))
            Case ExportTypes.Price
                ResultsSorter.SortClasses.Add(New EveHQ.Core.SortClass("Price", Core.SortDirection.Ascending))
            Case ExportTypes.Value
                ResultsSorter.SortClasses.Add(New EveHQ.Core.SortClass("Value", Core.SortDirection.Ascending))
            Case ExportTypes.Volume
                ResultsSorter.SortClasses.Add(New EveHQ.Core.SortClass("Volume", Core.SortDirection.Ascending))
        End Select
        ResultsSorter.SortClasses.Add(New EveHQ.Core.SortClass("TypeName", Core.SortDirection.Ascending))
        Assets.Sort(ResultsSorter)

        ' Select a location for the export
        Dim sfd As New SaveFileDialog
        sfd.Title = "Export Assets"
        sfd.InitialDirectory = EveHQ.Core.HQ.reportFolder
        Dim filterText As String = "Comma Separated Variable files (*.csv)|*.csv"
        filterText &= "|Tab Separated Variable files (*.txt)|*.txt"
        sfd.Filter = filterText
        sfd.FilterIndex = 0
        sfd.AddExtension = True
        sfd.ShowDialog()
        sfd.CheckPathExists = True
        If sfd.FileName <> "" Then
            Select Case sfd.FilterIndex
                Case 1
                    Call Me.ExportAssets(Assets, ",", sfd.FileName)
                Case 2
                    Call Me.ExportAssets(Assets, ControlChars.Tab, sfd.FileName)
            End Select
        End If
        sfd.Dispose()
        MessageBox.Show("Export of Prism Asset data completed!", "Prism Asset Export", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub ExportAssets(Assets As ArrayList, SepChar As String, FileName As String)

        Try

            Dim sw As New StreamWriter(FileName)
            Dim sb As New StringBuilder

            ' Write Header
            sb.Append("TypeName" & SepChar)
            sb.Append("Locations" & SepChar)
            sb.Append("Quantity" & SepChar)
            sb.Append("Price" & SepChar)
            sb.Append("Value" & SepChar)
            sb.Append("Volume" & SepChar)
            sw.WriteLine(sb.ToString)

            ' Write assets
            For Each Asset As AssetExportResult In Assets
                sb = New StringBuilder
                sb.Append(Asset.TypeName & SepChar)
                sb.Append(Asset.Locations.ToString & SepChar)
                sb.Append(Asset.Quantity.ToString & SepChar)
                sb.Append(Asset.Price.ToString & SepChar)
                sb.Append(Asset.Value.ToString & SepChar)
                sb.Append(Asset.Volume.ToString & SepChar)
                sw.WriteLine(sb.ToString)
            Next

            ' Close file
            sw.Flush()
            sw.Close()
            sw.Dispose()

        Catch ex As Exception
            MessageBox.Show("Error exporting Prism Asset details: " & ex.Message, "Error Exporting Assets", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

#End Region

    Private Sub PSCAssetOwners_SelectionChanged()

    End Sub
End Class

Public Class AssetExportResult
    ' Needs properties to work correctly with sorting
    Public Property TypeName As String
    Public Property Locations As Integer
    Public Property Quantity As Long
    Public Property Price As Double
    Public Property Value As Double
    Public Property Volume As Double
End Class

Public Enum ExportTypes As Integer
    TypeName = 0
    Quantity = 1
    Price = 2
    Value = 3
    Volume = 4
End Enum


