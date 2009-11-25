' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports System.Drawing
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml


Public Class frmItemBrowser

    Const ActivityCount As Integer = 8
    Dim metaParentID As Long
    Dim metaItemCount As Long
    Dim itemTypeID As Long
    Dim itemTypeName As String = ""
    Dim itemGroupID As String = ""
    Dim itemGroupName As String = ""
    Dim itemCatID As String = ""
    Dim itemCatName As String = ""
    Dim itemVariations(,) As String
    Dim itemSkills As New Dictionary(Of String, Integer)
    Dim itemFitting As New Collection
    Dim fittingAtts As New ArrayList
    Dim tabPagesM(ActivityCount) As Windows.Forms.TabPage
    Dim tabPagesC(ActivityCount) As Windows.Forms.TabPage
    Dim itemStart As DateTime
    Dim itemEnd As DateTime
    Dim itemTime As TimeSpan
    Dim oldNodeIndex As Integer = -1
    Dim skillsNeeded As New ArrayList
    Dim navigated As New SortedList
    Dim curNavigation As Integer = 0
    Dim compAtts As New SortedList
    Dim compItems As New SortedList
    Dim compMetas As New SortedList
    Dim CompMatrix(,,) As String
    Dim bEveCentralDataFound As Boolean = False
    Dim displayPilot As EveHQ.Core.Pilot
    Dim startup As Boolean = False
    Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
    Dim ShipCerts As New SortedList(Of String, ArrayList)
    Dim CertGrades() As String = New String() {"", "Basic", "Standard", "Improved", "Advanced", "Elite"}

    ' BP Variables
    Dim BPWF As Double = 0
    Dim BPWFC As Double = 0
    Dim BPWFP As Double = 0
    Dim BPWFM As Double = 0
    Dim BPWFPC As Double = 0
    Dim BPWFMC As Double = 0
    Dim eveData As Data.DataSet

    Private Sub LoadItemName(ByVal itemName As String)
        Dim strSQL As String = "SELECT * FROM invTypes WHERE typeName LIKE '" & itemName & "'"
        Call LoadItem(strSQL)
    End Sub
    Private Sub LoadItemID(ByVal itemID As String)
        Dim strSQL As String = "SELECT * FROM invTypes WHERE typeID =" & itemID
        Call LoadItem(strSQL)
    End Sub
    Private Sub LoadItem(ByVal strSQL As String)
        itemStart = Now
        ' Reset Item Skills
        itemSkills.Clear()
        Me.tabItem.TabPages.Remove(Me.tabSkills)
        Me.tabItem.TabPages.Remove(Me.tabFitting)
        Me.tabItem.TabPages.Remove(Me.tabMaterials)
        Me.tabItem.TabPages.Remove(Me.tabComponent)
        Me.tabItem.TabPages.Remove(Me.tabRecommended)
        Me.tabItem.TabPages.Remove(Me.tabVariations)
        Me.tabItem.TabPages.Remove(Me.tabDepends)
        Me.tabItem.TabPages.Remove(Me.tabEveCentral)
        Me.tabItem.TabPages.Remove(Me.tabInsurance)
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        itemTypeID = eveData.Tables(0).Rows(0).Item("typeID")
        itemTypeName = eveData.Tables(0).Rows(0).Item("typeName")
        itemGroupID = eveData.Tables(0).Rows(0).Item("groupID")
        itemCatID = EveHQ.Core.HQ.groupCats(itemGroupID)
        itemGroupName = EveHQ.Core.HQ.itemGroups(itemGroupID)
        itemCatName = EveHQ.Core.HQ.itemCats(itemCatID)
        lblItem.Text = itemTypeName
        ssLblID.Text = "ID: " & itemCatID & "/" & itemGroupID & "/" & itemTypeID
        lblDescription.Text = eveData.Tables(0).Rows(0).Item("description")
        Call GetAttributes(itemTypeID, itemTypeName)
        Call GenerateSkills(itemTypeID, itemTypeName)
        Call GetVariations(itemTypeID, itemTypeName)
        Call GenerateFitting()
        If (itemCatName = "Ship") Then
            Call GetRecommendations(itemTypeID, itemTypeName)
            Call GetInsurance(itemTypeID, itemTypeName)
        End If
        Call GetMaterials(itemTypeID, itemTypeName)
        Call GetComponents(itemTypeID, itemTypeName)
        Call GetDependencies(itemTypeID, itemTypeName)
        System.Threading.ThreadPool.QueueUserWorkItem(AddressOf GetEveCentralData, itemTypeID)
        ssDBLocation.Text = "Location: " & itemCatName & " --> " & itemGroupName
        itemEnd = Now
        itemTime = itemEnd - itemStart
        ssLabel.Text = "Last Item retrieved in " & itemTime.TotalSeconds & "s"
    End Sub
    Private Sub GetVariations(ByVal metaTypeID As Long, ByVal metaTypeName As String)
        Dim strSQL As String = ""
        strSQL &= "SELECT invMetaTypes.typeID, invMetaTypes.parentTypeID"
        strSQL &= " FROM invMetaTypes"
        strSQL &= " WHERE (((invMetaTypes.typeID)=" & metaTypeID & ") OR ((invMetaTypes.parentTypeID)=" & metaTypeID & "));"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData.Tables(0).Rows.Count = 0 Then
            If Me.tabItem.TabPages.Contains(tabVariations) = True Then
                Me.tabItem.TabPages.Remove(Me.tabVariations)
            End If
        Else
            If Me.tabItem.TabPages.Contains(tabVariations) = False Then
                Me.tabItem.TabPages.Add(Me.tabVariations)
            End If
            metaParentID = eveData.Tables(0).Rows(0).Item("parentTypeID")
            strSQL = ""
            strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
            strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
            strSQL &= " WHERE (((invMetaTypes.parentTypeID)=" & metaParentID & "));"
            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            metaItemCount = eveData.Tables(0).Rows.Count
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

            lstVariations.Items.Clear()
            For item As Integer = 0 To metaItemCount
                Dim lstItem As New ListViewItem
                lstItem.Tag = itemVariations(0, item)
                lstItem.Text = itemVariations(1, item)
                lstItem.SubItems.Add(itemVariations(2, item))
                lstVariations.Items.Add(lstItem)
            Next

            ' Generate Comparisons
            ' NB Attribute list is already generated in the generate attributes routine
            compItems.Clear() : compMetas.Clear()
            For item As Integer = 0 To metaItemCount
                compItems.Add(itemVariations(0, item), itemVariations(1, item))
                compMetas.Add(itemVariations(0, item), itemVariations(2, item))
            Next
            ' Get all the comparatives
            Call Me.GetComparatives()
            ' Put into a table
            Call Me.DrawComparatives()

        End If
    End Sub
    Private Sub GetComparatives()
        ' Define IN string
        Dim strIn As String = ""
        Dim item As String = ""
        For Each item In compItems.Keys
            strIn &= item & ","
        Next
        strIn = strIn.TrimEnd(",".ToCharArray)

        ReDim CompMatrix(compItems.Count, compAtts.Count, 1)

        ' Get basic attributes
        Dim strSQL As String = "SELECT * from invTypes WHERE typeID IN (" & strIn & ");"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            item = eveData.Tables(0).Rows(row).Item("typeID").ToString
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("A"), 0) = eveData.Tables(0).Rows(row).Item("groupID").ToString
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("B"), 0) = eveData.Tables(0).Rows(row).Item("description").ToString
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("C"), 0) = eveData.Tables(0).Rows(row).Item("radius").ToString
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("D"), 0) = eveData.Tables(0).Rows(row).Item("mass").ToString
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("E"), 0) = eveData.Tables(0).Rows(row).Item("volume").ToString
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("F"), 0) = eveData.Tables(0).Rows(row).Item("capacity").ToString
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("G"), 0) = eveData.Tables(0).Rows(row).Item("portionSize").ToString
            If IsDBNull(eveData.Tables(0).Rows(0).Item("raceID").ToString) = False Then
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("H"), 0) = eveData.Tables(0).Rows(row).Item("raceID").ToString
            Else
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("H"), 0) = "0"
            End If
            CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I1"), 0) = eveData.Tables(0).Rows(row).Item("basePrice").ToString
            If EveHQ.Core.HQ.MarketPriceList.Contains(item) = True Then
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I2"), 0) = EveHQ.Core.HQ.MarketPriceList.Item(item)
            Else
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I2"), 0) = 0
            End If
            If EveHQ.Core.HQ.CustomPriceList.Contains(item) = True Then
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I3"), 0) = EveHQ.Core.HQ.CustomPriceList.Item(item)
            Else
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I3"), 0) = 0
            End If
        Next

        ' Get all other attributes
        strSQL = "SELECT dgmTypeAttributes.typeID, dgmAttributeTypes.attributeGroup, eveUnits.unitID, eveUnits.displayName as unitDisplayName, eveUnits.unitName, dgmTypeAttributes.attributeID, dgmAttributeTypes.attributeID, dgmAttributeTypes.displayName as attributeDisplayName, dgmAttributeTypes.attributeName, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
        strSQL &= " FROM (eveUnits INNER JOIN dgmAttributeTypes ON eveUnits.unitID=dgmAttributeTypes.unitID) INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID=dgmTypeAttributes.attributeID"
        strSQL &= " WHERE typeID IN (" & strIn & ") ORDER BY dgmTypeAttributes.attributeID;"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            item = eveData.Tables(0).Rows(row).Item("typeID")
            Dim compAttID As String = ""
            Dim compValue As String = ""
            Dim compUnit As String = eveData.Tables(0).Rows(row).Item("unitDisplayName").ToString.Trim
            Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
                Case 0
                    compAttID = eveData.Tables(0).Rows(row).Item("dgmAttributeTypes.attributeID")
                Case Else
                    compAttID = eveData.Tables(0).Rows(row).Item("attributeID")
            End Select
            If IsDBNull(eveData.Tables(0).Rows(row).Item("valueFloat")) = False Then
                compValue = eveData.Tables(0).Rows(row).Item("valueFloat")
            Else
                compValue = eveData.Tables(0).Rows(row).Item("valueInt")
            End If
            ' Do modifier calculations here!
            Select Case eveData.Tables(0).Rows(row).Item("unitID")
                Case "108"
                    compValue = Math.Round(100 - (Val(compValue) * 100), 2)
                Case "109"
                    compValue = Math.Round((Val(compValue) * 100) - 100, 2)
                Case "111"
                    compValue = Math.Round((Val(compValue) - 1) * 100, 2)
                Case "101"      ' If unit is "ms"
                    If Val(compValue) > 1000 Then
                        compValue = Math.Round(Val(compValue) / 1000, 2)
                        compUnit = " s"
                    End If
            End Select
            ' Adjust for TypeIDs
            If compUnit = "typeID" Then
                compValue = EveHQ.Core.HQ.itemData(compValue).Name
                compUnit = ""
            End If
            ' Check if it's in the attribute list before trying to add it!
            If compAtts.Contains(compAttID) = True Then
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey(compAttID), 0) = compValue
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey(compAttID), 1) = compUnit
            End If
        Next
    End Sub
    Private Sub GetDependencies(ByVal typeID As Long, ByVal typeName As String)
        If itemCatID = 16 Then
            lvwDepend.Items.Clear()
            Dim groupID As String = ""
            Dim catID As String = ""
            Dim itemID As Integer = 0
            Dim skillName As String = ""
            Dim itemData(1) As String
            Dim skillData(1) As String
            For lvl As Integer = 1 To 5
                If EveHQ.Core.HQ.SkillUnlocks.ContainsKey(typeID & "." & CStr(lvl)) = True Then
                    Dim itemUnlocked As ArrayList = EveHQ.Core.HQ.SkillUnlocks(typeID & "." & CStr(lvl))
                    For Each item As String In itemUnlocked
                        Dim newItem As New ListViewItem
                        Dim toolTipText As New StringBuilder

                        itemData = item.Split(CChar("_"))
                        groupID = itemData(1)
                        catID = EveHQ.Core.HQ.groupCats.Item(groupID)
                        newItem.Group = lvwDepend.Groups("Cat" & catID)
                        itemID = EveHQ.Core.HQ.itemList.IndexOfValue(itemData(0))
                        newItem.Text = EveHQ.Core.HQ.itemData(itemData(0)).Name
                        newItem.Name = newItem.Text
                        Dim skillUnlocked As ArrayList = CType(EveHQ.Core.HQ.ItemUnlocks(itemData(0)), ArrayList)
                        Dim allTrained As Boolean = True
                        For Each skillPair As String In skillUnlocked
                            skillData = skillPair.Split(CChar("."))
                            skillName = EveHQ.Core.SkillFunctions.SkillIDToName(skillData(0))
                            If skillData(0) <> typeID Then
                                toolTipText.Append(skillName)
                                toolTipText.Append(" (Level ")
                                toolTipText.Append(skillData(1))
                                toolTipText.Append("), ")
                            End If
                            If EveHQ.Core.SkillFunctions.IsSkillTrained(displayPilot, skillName, CInt(skillData(1))) = False Then
                                allTrained = False
                            End If
                        Next
                        If toolTipText.Length > 0 Then
                            toolTipText.Insert(0, "Also Requires: ")

                            If (toolTipText.ToString().EndsWith(", ")) Then
                                toolTipText.Remove(toolTipText.Length - 2, 2)
                            End If
                        End If
                        If allTrained = True Then
                            newItem.ForeColor = Color.Green
                        Else
                            newItem.ForeColor = Color.Red
                        End If
                        newItem.ToolTipText = toolTipText.ToString()
                        newItem.SubItems.Add(EveHQ.Core.HQ.itemGroups(groupID))
                        newItem.SubItems.Add("Level " & lvl)
                        lvwDepend.Items.Add(newItem)
                    Next
                End If
            Next
            If Me.tabItem.TabPages.Contains(Me.tabDepends) = False Then
                Me.tabItem.TabPages.Add(Me.tabDepends)
            End If
        Else
            ' Remove the relevant tab
            If Me.tabItem.TabPages.Contains(Me.tabDepends) = True Then
                Me.tabItem.TabPages.Remove(Me.tabDepends)
            End If
        End If
    End Sub
    Private Sub DrawComparatives()
        lstComparisons.BeginUpdate()
        lstComparisons.Items.Clear()
        ' Add columns
        lstComparisons.Columns.Clear()
        lstComparisons.Columns.Add("Item", 275)
        lstComparisons.Columns.Add("Meta Type", 75, HorizontalAlignment.Right)
        Dim noColumn As New ArrayList
        For att As Integer = 0 To compAtts.Count - 1
            ' Check if the column is required
            Dim colRequired As Boolean = False
            Dim colCheck As String = CompMatrix(0, att, 0)
            For col As Integer = 0 To compItems.Count - 1
                If CompMatrix(col, att, 0) <> colCheck Then
                    colRequired = True
                End If
            Next
            If colRequired = True Or Me.chkShowAllColumns.Checked = True Then
                lstComparisons.Columns.Add(compAtts.Item(compAtts.GetKey(att)), 75, HorizontalAlignment.Right)
            Else
                noColumn.Add(att)
            End If
        Next
        ' Add items
        For item As Integer = 0 To compItems.Count - 1
            Dim lstItem As New ListViewItem
            lstItem.Text = compItems(compItems.GetKey(item))
            lstItem.Name = lstItem.Text
            lstItem.SubItems.Add(compMetas(compItems.GetKey(item)))
            lstItem.SubItems(1).Name = lstItem.SubItems(1).Text
            ' Add other atts
            For att As Integer = 0 To compAtts.Count - 1
                If noColumn.Contains(att) = False Then
                    Dim lstSubItem As New ListViewItem.ListViewSubItem
                    lstSubItem.Name = CompMatrix(item, att, 0)
                    If IsNumeric(CompMatrix(item, att, 0)) = True Then
                        lstSubItem.Text = Format(CDbl(CompMatrix(item, att, 0)), "#,###,##0.#") & " " & CompMatrix(item, att, 1)
                    Else
                        lstSubItem.Text = CompMatrix(item, att, 0) & " " & CompMatrix(item, att, 1)
                    End If
                    lstItem.SubItems.Add(lstSubItem)
                End If
            Next
            lstComparisons.Items.Add(lstItem)
        Next
        lstComparisons.EndUpdate()
    End Sub
    Private Sub GetEveCentralData(ByVal itemID As Object)
        Dim itemTypeID As String = itemID.ToString
        Dim webdata As String = ""
        Try

            Dim RemoteURL As String = "http://eve-central.com/home/marketstat_xml.html?typeid=" & itemTypeID
            Dim request = CType(WebRequest.Create(RemoteURL), HttpWebRequest)
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                request.Proxy = EveHQProxy
            End If
            ' Setup request parameters
            request.Method = "GET"
            request.ContentType = "application/x-www-form-urlencoded"
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            webdata = readStream.ReadToEnd()
            ' Save the response in the pilotdata area for later retrieval
            Dim ECXML As New XmlDocument
            ECXML.LoadXml(webdata)
            ' Close the connections
            response.Close()
            readStream.Close()

            Dim MarketDetails As XmlNodeList
            MarketDetails = ECXML.SelectNodes("/market_stat")
            If MarketDetails.Count = 0 Then
                Throw New System.Exception
            End If
            Dim MarketItem As XmlNode = MarketDetails(0)

            ' Populate the information
            lstEveCentral.Items.Clear()
            lstEveCentral.Items.Add("Average Price")
            lstEveCentral.Items.Add("Total Volume")
            lstEveCentral.Items.Add("Avg Buy Price")
            lstEveCentral.Items.Add("Buy Volume")
            lstEveCentral.Items.Add("Std Dev Buy Price")
            lstEveCentral.Items.Add("Max Buy Price")
            lstEveCentral.Items.Add("Min Buy Price")
            lstEveCentral.Items.Add("Avg Sell Price")
            lstEveCentral.Items.Add("Sell Volume")
            lstEveCentral.Items.Add("Std Dev Sell Price")
            lstEveCentral.Items.Add("Max Sell Price")
            lstEveCentral.Items.Add("Min Sell Price")
            For node As Integer = 0 To 11
                Dim nodeText As String = MarketItem.ChildNodes(node + 1).InnerText
                If IsNumeric(nodeText) = True Then
                    lstEveCentral.Items(node).SubItems.Add(FormatNumber(Double.Parse(nodeText, Globalization.NumberStyles.Number, culture), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                Else
                    Me.EveCentralDataFound = False
                    Exit Sub
                End If
            Next
            Me.EveCentralDataFound = True
        Catch e As Exception
            lstEveCentral.Items.Clear()
            lstEveCentral.Items.Add("Unable to Load XML Feed")
        End Try
    End Sub

    Private Sub frmItemBrowser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the startup flag
        startup = True

        ' Add an event handler for the changing data
        AddHandler PlugInData.PluginDataReceived, AddressOf PluginDataHandler

        For activity As Integer = 1 To ActivityCount
            tabPagesM(activity) = Me.tabMaterial.TabPages("tabM" & activity)
            tabPagesC(activity) = Me.tabComponents.TabPages("tabC" & activity)
        Next
        Call Me.LoadFittingAttributes()
        Call Me.LoadShipCertRecommendations()
        Me.tabItem.TabPages.Remove(Me.tabFitting)
        Me.tabItem.TabPages.Remove(Me.tabVariations)
        Me.tabItem.TabPages.Remove(Me.tabSkills)
        Me.tabItem.TabPages.Remove(Me.tabMaterials)
        Me.tabItem.TabPages.Remove(Me.tabComponent)
        Me.tabItem.TabPages.Remove(Me.tabEveCentral)
        Me.tabItem.TabPages.Remove(Me.tabInsurance)
        Me.lblUsable.Text = ""
        Me.lblUsableTime.Text = ""

        ' Load the browser
        chkBrowseNonPublished.Checked = EveHQ.Core.HQ.EveHQSettings.IBShowAllItems
        If tvwBrowse.Nodes.Count = 0 Then Call Me.LoadBrowserGroups()

        ' Load the Pilots
        Call Me.UpdatePilots()

        ' Load the Wanted List
        Call Me.DrawWantedList()

        ' Load the attribute search combobox
        cboAttSearch.BeginUpdate()
        cboAttSearch.Items.Clear()
        cboAttSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboAttSearch.AutoCompleteSource = AutoCompleteSource.CustomSource
        For Each att As String In PlugInData.AttributeList.GetKeyList
            cboAttSearch.AutoCompleteCustomSource.Add(att)
            cboAttSearch.Items.Add(att)
        Next
        cboAttSearch.EndUpdate()

        ' Reset the list of navigated items
        navigated.Clear()

        ' Clear the startup flag
        startup = False
    End Sub
    Private Sub PluginDataHandler()
        Dim myPlugInData As Object = PlugInData.PlugInDataObject
        If CStr(myPlugInData) <> "" Then
            Call LoadItemID(CStr(myPlugInData))
            Call Me.AddToNavigation(itemTypeName)
        End If
    End Sub
    Private Sub LoadBrowserGroups()
        Dim newNode As TreeNode
        tvwBrowse.BeginUpdate()
        tvwBrowse.Nodes.Clear()
        ' Load up the Browser with categories
        For Each cat As String In EveHQ.Core.HQ.itemCats.Keys
            newNode = New TreeNode
            newNode.Name = cat
            newNode.Text = EveHQ.Core.HQ.itemCats(cat)
            tvwBrowse.Nodes.Add(newNode)
        Next
        ' Load up the Browser with groups
        For Each group As String In EveHQ.Core.HQ.itemGroups.Keys
            newNode = New TreeNode
            newNode.Name = group
            newNode.Text = EveHQ.Core.HQ.itemGroups(group)
            newNode.Nodes.Add("Loading...")
            tvwBrowse.Nodes(EveHQ.Core.HQ.groupCats(newNode.Name)).Nodes.Add(newNode)
        Next
        ' Update the browser
        tvwBrowse.Sorted = True
        tvwBrowse.EndUpdate()
    End Sub
    Public Sub UpdatePilots()
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next
        cboPilots.EndUpdate()

        If cboPilots.Items.Count > 0 Then
            If cboPilots.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = True Then
                cboPilots.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
            Else
                cboPilots.SelectedIndex = 0
            End If
        End If
    End Sub
    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilots.SelectedItem.ToString) = True Then
            displayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            If startup = False Then
                Call LoadItemID(itemTypeID)
            End If
        End If
    End Sub
    Private Sub lstVariations_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstVariations.DoubleClick
        Dim selItem As String = lstVariations.SelectedItems(0).Tag
        Call LoadItemID(selItem)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub

    Private Sub GetInsurance(ByVal typeID As Long, ByVal typeName As String)
        Me.tabItem.TabPages.Add(Me.tabInsurance)
        lstInsurance.Items.Clear()
        Dim strSQL As String = "SELECT * from invTypes where typeID=" & typeID
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        Dim basePrice As Double = EveHQ.Core.HQ.BasePriceList(typeID.ToString)
        Dim marketPrice As Double = EveHQ.Core.DataFunctions.GetPrice(typeID.ToString)
        For counter As Integer = 5 To 30 Step 5
            Dim newInsuranceItem As New ListViewItem
            Select Case counter
                Case 5
                    newInsuranceItem.Text = "Basic"
                Case 10
                    newInsuranceItem.Text = "Standard"
                Case 15
                    newInsuranceItem.Text = "Bronze"
                Case 20
                    newInsuranceItem.Text = "Silver"
                Case 25
                    newInsuranceItem.Text = "Gold"
                Case 30
                    newInsuranceItem.Text = "Platinum"
            End Select
            Dim insuranceFee As Double = basePrice / 100 * counter
            Dim insurancePayout As Double = basePrice / 100 * (40 + (2 * counter))
            Dim insuranceProfit As Double = insurancePayout - marketPrice - insuranceFee
            newInsuranceItem.SubItems.Add(insuranceFee.ToString("N02"))
            newInsuranceItem.SubItems.Add(insurancePayout.ToString("N02"))
            newInsuranceItem.SubItems.Add(marketPrice.ToString("N02"))
            newInsuranceItem.SubItems.Add(insuranceProfit.ToString("N02"))
            lstInsurance.Items.Add(newInsuranceItem)
        Next
    End Sub

    Private Sub GetRecommendations(ByVal typeID As Long, ByVal typeName As String)
        lvwRecommended.BeginUpdate()
        lvwRecommended.Items.Clear()
        If ShipCerts.ContainsKey(CStr(typeID)) Then
            Dim Certs As ArrayList = ShipCerts(CStr(typeID))
            Dim dCerts As New SortedList(Of String, Integer)
            For Each cert As String In Certs
                Dim newCert As EveHQ.Core.Certificate = EveHQ.Core.HQ.Certificates(cert)
                Dim certClass As EveHQ.Core.CertificateClass = EveHQ.Core.HQ.CertificateClasses(CStr(newCert.ClassID))
                dCerts.Add(certClass.Name, newCert.Grade)
            Next
            For Each certName As String In dCerts.Keys
                Dim newCert As New ListViewItem
                newCert.Text = certName
                newCert.ImageIndex = dCerts(certName)
                Dim certGrade As String = CertGrades(dCerts(certName))
                newCert.SubItems.Add(certGrade)
                lvwRecommended.Items.Add(newCert)
            Next
            Me.tabItem.TabPages.Add(Me.tabRecommended)
        End If
        lvwRecommended.EndUpdate()
    End Sub
    Private Sub GetAttributes(ByVal typeID As Long, ByVal typeName As String)

        ' Get parent type info for later on!
        Dim pInfo(5) As String
        pInfo = EveHQ.Core.DataFunctions.GetTypeParentInfo(typeID)

        Dim attributes(150, 5) As String
        Dim attNo As Integer = 0
        ' Set "unused" flag
        For a As Integer = 0 To 150 : attributes(a, 0) = "0" : Next

        Dim bpTypeID As String = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)
        ' Show additional information re blueprint or product
        If pInfo(4) = 9 Then
            Dim typeID2 As String = EveHQ.Core.DataFunctions.GetTypeID(bpTypeID)
            If bpTypeID <> typeID2 Then
                picBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation("27_01", EveHQ.Core.ImageHandler.ImageType.Icons)
                picBP.Tag = typeID2
                ItemToolTip1.SetToolTip(Me.picBP, "Show Product Type")
                picBP.Visible = True
            Else
                picBP.Visible = False
            End If
        Else
            If bpTypeID <> typeID Then
                picBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(bpTypeID.ToString, EveHQ.Core.ImageHandler.ImageType.Blueprints)
                picBP.Tag = bpTypeID
                ItemToolTip1.SetToolTip(Me.picBP, "Show Blueprint")
                picBP.Visible = True
                BPWF = EveHQ.Core.DataFunctions.GetBPWF(bpTypeID)
                BPWFM = ((1 / BPWF) / (1 + nudMELevel.Value))
                If displayPilot IsNot Nothing Then
                    BPWFP = BPWFM + (0.25 - (0.05 * displayPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ProductionEfficiency)))
                End If
            Else
                picBP.Visible = False
            End If
        End If

        Dim strSQL As String = "SELECT * from invTypes where typeID=" & typeID
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        Dim iconData As System.Data.DataSet = EveHQ.Core.DataFunctions.GetData("SELECT invTypes.typeID, eveGraphics.icon FROM eveGraphics INNER JOIN invTypes ON eveGraphics.graphicID = invTypes.graphicID WHERE typeID=" & typeID & ";")

        ' Load picture
        If iconData.Tables(0).Rows.Count > 0 Then
            Select Case pInfo(4)
                Case 6, 18, 23
                    picItem.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(typeID.ToString, EveHQ.Core.ImageHandler.ImageType.Types)
                Case 9
                    picItem.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(typeID.ToString, EveHQ.Core.ImageHandler.ImageType.Blueprints)
                Case Else
                    picItem.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(iconData.Tables(0).Rows(0).Item("icon").ToString, EveHQ.Core.ImageHandler.ImageType.Icons)
            End Select
        Else
            picItem.Image = My.Resources.noitem
        End If

        ' Insert attribute 1 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "A"
        attributes(attNo, 2) = "Group ID"
        attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("groupID")
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 2 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "B"
        attributes(attNo, 2) = "Description"
        Dim desc As String = eveData.Tables(0).Rows(0).Item("description").ToString
        desc = System.Text.RegularExpressions.Regex.Replace(desc, "[\x00-\x1f]", "")
        attributes(attNo, 3) = desc
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 3 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "C"
        attributes(attNo, 2) = "Radius"
        attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("radius")
        attributes(attNo, 4) = " m"
        attributes(attNo, 5) = "1"
        ' Insert attribute 4 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "D"
        attributes(attNo, 2) = "Mass"
        attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("mass")
        attributes(attNo, 4) = " kg"
        attributes(attNo, 5) = "1"
        ' Insert attribute 5 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "E"
        attributes(attNo, 2) = "Volume"
        attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("volume")
        attributes(attNo, 4) = " m3"
        attributes(attNo, 5) = "1"
        ' Insert attribute 6 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "F"
        attributes(attNo, 2) = "Cargo Capacity"
        attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("capacity")
        attributes(attNo, 4) = " m3"
        attributes(attNo, 5) = "1"
        ' Insert attribute 7 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "G"
        attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("portionSize")
        attributes(attNo, 2) = "Portion Size"
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 8 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "H"
        attributes(attNo, 2) = "Race ID"
        If IsDBNull(eveData.Tables(0).Rows(0).Item("raceID")) = False Then
            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("raceID")
        Else
            attributes(attNo, 3) = "0"
        End If
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 9 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "I1"
        attributes(attNo, 2) = "Base Price"
        attributes(attNo, 3) = Math.Round(eveData.Tables(0).Rows(0).Item("basePrice"), 2, MidpointRounding.AwayFromZero)
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert Market Price
        attNo += 1
        attributes(attNo, 1) = "I2"
        attributes(attNo, 2) = "Market Price"
        If EveHQ.Core.HQ.MarketPriceList.Contains(CStr(itemTypeID)) = True Then
            attributes(attNo, 3) = Math.Round(CDbl(EveHQ.Core.HQ.MarketPriceList.Item(CStr(itemTypeID))), 2, MidpointRounding.AwayFromZero)
        Else
            attributes(attNo, 3) = 0
        End If
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert Custom Price
        attNo += 1
        attributes(attNo, 1) = "I3"
        attributes(attNo, 2) = "Custom Price"
        If EveHQ.Core.HQ.CustomPriceList.Contains(CStr(itemTypeID)) = True Then
            attributes(attNo, 3) = Math.Round(CDbl(EveHQ.Core.HQ.CustomPriceList.Item(CStr(itemTypeID))), 2, MidpointRounding.AwayFromZero)
        Else
            attributes(attNo, 3) = 0
        End If
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 10 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "J"
        attributes(attNo, 2) = "Published"
        If eveData.Tables(0).Rows(0).Item("published") = "0" Then
            attributes(attNo, 3) = "False"
        Else
            attributes(attNo, 3) = "True"
        End If
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 11 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "K"
        attributes(attNo, 2) = "Market Group"
        If IsDBNull(eveData.Tables(0).Rows(0).Item("marketGroupID")) = False Then
            attributes(attNo, 3) = eveData.Tables(0).Rows(0).Item("marketGroupID")
        Else
            attributes(attNo, 3) = "n/a"
        End If
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"

        If pInfo(4) = 9 Then            ' If in the blueprint category
            strSQL = "SELECT *"
            strSQL &= " FROM invBlueprintTypes"
            strSQL &= " WHERE blueprintTypeID=" & typeID & ";"
            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If eveData.Tables(0).Rows.Count > 0 Then
                For col As Integer = 3 To eveData.Tables(0).Columns.Count - 1
                    attNo += 1
                    attributes(attNo, 1) = "Z" & col - 2
                    attributes(attNo, 2) = eveData.Tables(0).Columns(col).Caption
                    attributes(attNo, 3) = Math.Round(eveData.Tables(0).Rows(0).Item(col))
                    attributes(attNo, 4) = ""
                    attributes(attNo, 5) = "15"
                    ' Check for BPWF
                    If attributes(attNo, 2) = "wasteFactor" Then
                        BPWF = (attributes(attNo, 3))
                        BPWFM = ((1 / BPWF) / (1 + nudMELevel.Value))
                        BPWFP = BPWFM + (0.25 - (0.05 * displayPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ProductionEfficiency)))
                    End If
                Next
            End If
            attributes(14, 3) = EveHQ.Core.SkillFunctions.TimeToString(attributes(14, 3))
            attributes(16, 3) = EveHQ.Core.SkillFunctions.TimeToString(attributes(16, 3))
            attributes(17, 3) = EveHQ.Core.SkillFunctions.TimeToString(attributes(17, 3))
            attributes(18, 3) = EveHQ.Core.SkillFunctions.TimeToString(attributes(18, 3))
            attributes(19, 3) = EveHQ.Core.SkillFunctions.TimeToString(attributes(19, 3))
        Else                            ' If not in the blueprint category
            strSQL = "SELECT dgmAttributeTypes.attributeGroup, eveUnits.unitID, eveUnits.displayName as unitDisplayName, eveUnits.unitName, dgmTypeAttributes.attributeID, dgmAttributeTypes.attributeID, dgmAttributeTypes.displayName as attributeDisplayName, dgmAttributeTypes.attributeName, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
            strSQL &= " FROM (eveUnits INNER JOIN dgmAttributeTypes ON eveUnits.unitID=dgmAttributeTypes.unitID) INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID=dgmTypeAttributes.attributeID"
            strSQL &= " WHERE typeID=" & typeID & " ORDER BY dgmTypeAttributes.attributeID;"
            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                attNo += 1
                Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
                    Case 0
                        attributes(attNo, 1) = eveData.Tables(0).Rows(row).Item("dgmAttributeTypes.attributeID")
                    Case Else
                        attributes(attNo, 1) = eveData.Tables(0).Rows(row).Item("attributeID")
                End Select
                If eveData.Tables(0).Rows(row).Item("attributeDisplayName").ToString.Trim = "" Then
                    attributes(attNo, 2) = eveData.Tables(0).Rows(row).Item("attributeName").ToString.Trim
                Else
                    attributes(attNo, 2) = eveData.Tables(0).Rows(row).Item("attributeDisplayName").ToString.Trim
                End If
                If IsDBNull(eveData.Tables(0).Rows(row).Item("valueFloat")) = False Then
                    attributes(attNo, 3) = eveData.Tables(0).Rows(row).Item("valueFloat")
                Else
                    attributes(attNo, 3) = eveData.Tables(0).Rows(row).Item("valueInt")
                End If
                attributes(attNo, 4) = " " & eveData.Tables(0).Rows(row).Item("unitDisplayName").ToString.Trim
                attributes(attNo, 5) = eveData.Tables(0).Rows(row).Item("attributeGroup").ToString.Trim
                ' Do modifier calculations here!
                Select Case eveData.Tables(0).Rows(row).Item("unitID")
                    Case "108"
                        attributes(attNo, 3) = Math.Round(100 - (CDbl(attributes(attNo, 3)) * 100), 2)
                    Case "109"
                        attributes(attNo, 3) = Math.Round((CDbl(attributes(attNo, 3)) * 100) - 100, 2)
                    Case "111"
                        attributes(attNo, 3) = Math.Round((CDbl(attributes(attNo, 3)) - 1) * 100, 2)
                    Case "101"      ' If unit is "ms"
                        If Val(attributes(attNo, 3)) > 1000 Then
                            attributes(attNo, 3) = Math.Round(CDbl(attributes(attNo, 3)) / 1000, 2)
                            attributes(attNo, 4) = " s"
                        End If
                End Select
                'If Double.TryParse(attributes(attNo, 3), Globalization.NumberStyles.Number, culture, attValue) = True Then
                '    If attValue <> 0 Then
                '        attributes(attNo, 3) = Double.Parse(attValue, Globalization.NumberStyles.Number, culture)
                '    End If
                'End If
            Next
        End If

        ' Do character attribute adjustments here
        Dim attName As String = ""
        For att As Integer = 1 To attNo
            If attributes(att, 4) = " attributeID" Then
                eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM dgmAttributeTypes WHERE attributeID=" & attributes(att, 3))
                attributes(att, 3) = eveData.Tables(0).Rows(0).Item("attributeName").ToString.Trim
                attributes(att, 4) = ""
            End If
        Next

        ' Do skill & fitting requirements adjustment & "math.rounding" here
        Dim skillLvl As String = "1"
        itemFitting.Clear()
        For att As Integer = 1 To attNo
            If attributes(att, 4) = " typeID" Then
                attributes(att, 4) &= attributes(att, 3)
                eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM invTypes WHERE typeID=" & attributes(att, 3))

                Select Case attributes(att, 1)
                    Case "182"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 277 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        itemSkills.Add(attributes(att, 3), skillLvl)
                        attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "183"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 278 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        itemSkills.Add(attributes(att, 3), skillLvl)
                        attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "184"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 279 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        itemSkills.Add(attributes(att, 3), skillLvl)
                        attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "1285"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 1286 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        itemSkills.Add(attributes(att, 3), skillLvl)
                        attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "1289"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 1287 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        itemSkills.Add(attributes(att, 3), skillLvl)
                        attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "1290"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 1288 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        itemSkills.Add(attributes(att, 3), skillLvl)
                        attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case Else
                        attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                End Select

                'If attributes(att, 1) = "182" Or attributes(att, 1) = "183" Or attributes(att, 1) = "184" Then
                '    For att2 As Integer = att To attNo
                '        If Val(attributes(att, 1)) + 95 = Val(attributes(att2, 1)) Then
                '            skillLvl = attributes(att2, 3)
                '            attributes(att2, 1) = "0"
                '            Exit For
                '        End If
                '    Next

                '    ' Load into skill requirement array
                '    skillNo += 1
                'itemSkills(skillNo, 0) = attributes(att, 3)
                'itemSkills(skillNo, 1) = skillLvl
                '    attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                '    If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                'Else
                '    attributes(att, 3) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
                'End If
            Else
                If fittingAtts.Contains(attributes(att, 1)) = True Then
                    Dim fitAtts(5) As String
                    For fit As Integer = 0 To 5
                        fitAtts(fit) = attributes(att, fit)
                    Next
                    itemFitting.Add(fitAtts)
                End If
                If IsNumeric(attributes(att, 3)) = True Then
                End If
            End If
        Next

        ' Put the stuff into a nice table!
        Dim attGroups(15)
        attGroups(0) = "Miscellaneous" : attGroups(1) = "Structure" : attGroups(2) = "Armor" : attGroups(3) = "Shield"
        attGroups(4) = "Capacitor" : attGroups(5) = "Targetting" : attGroups(6) = "Propulsion" : attGroups(7) = "Required Skills"
        attGroups(8) = "Fitting Requirements" : attGroups(9) = "Damage" : attGroups(10) = "Entity Targetting" : attGroups(11) = "Entity Kill"
        attGroups(12) = "Entity EWar" : attGroups(13) = "Usage" : attGroups(14) = "Skill Information" : attGroups(15) = "Blueprint Information"

        lstAttributes.Items.Clear()
        compAtts.Clear()
        For itemloop As Integer = 1 To attNo
            If attributes(itemloop, 0) = "0" And attributes(itemloop, 1) <> "0" Then
                Dim attGroup As String = attributes(itemloop, 5)
                ' Create a listview group
                Dim lvGroup As New ListViewGroup
                lvGroup.Name = attGroups(attGroup)
                lvGroup.Header = attGroups(attGroup)
                lstAttributes.Groups.Add(lvGroup)
                For item As Integer = itemloop To attNo
                    If attributes(item, 5) = attGroup And attributes(item, 1) <> "0" Then
                        Dim lstItem As New ListViewItem
                        lstItem.Name = attributes(item, 1)
                        lstItem.Text = "(" & attributes(item, 1) & ")  " & attributes(item, 2)
                        ' Add tooltip to the description
                        If lstItem.Name = "B" Then
                            lstItem.ToolTipText = lblDescription.Text
                        End If
                        If attributes(item, 4).StartsWith(" typeID") Then
                            lstItem.SubItems.Add(attributes(item, 3))
                            Dim trimChars() As Char = " typeID"
                            lstItem.Tag = attributes(item, 4).TrimStart(trimChars)
                            lstItem.ForeColor = Color.Blue
                        Else
                            lstItem.Tag = ""
                            If IsNumeric(attributes(item, 3)) = True Then
                                lstItem.SubItems.Add(Format(CDbl(attributes(item, 3)), "#,###,##0.###") & attributes(item, 4))
                            Else
                                lstItem.SubItems.Add(attributes(item, 3) & attributes(item, 4))
                            End If
                        End If
                        attributes(item, 0) = "1"
                        lstItem.Group = lstAttributes.Groups.Item(attGroups(attGroup))
                        lstAttributes.Items.Add(lstItem)
                        ' Add to the Comparison group for later
                        compAtts.Add(attributes(item, 1), attributes(item, 2))
                    End If
                Next
            End If
        Next

    End Sub

    Private Sub GetMaterials(ByVal typeID As Long, ByVal typeName As String)
        Dim bpTypeID As Long = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)

        ' Select only the building activity (at the minute!)
        Dim strSQL As String = "SELECT *"
        strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID = invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN ramTypeRequirements ON invTypes.typeID = ramTypeRequirements.requiredTypeID"
        strSQL &= " WHERE (ramTypeRequirements.typeID=" & bpTypeID & " OR ramTypeRequirements.typeID=" & typeID & ") ORDER BY invCategories.categoryName, invGroups.groupName"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData.Tables(0).Rows.Count > 0 Then

            ' Work out what activities we have in the list
            Dim activities(ActivityCount) As Boolean
            Dim strActivity As String = ""
            For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                activities(Val(eveData.Tables(0).Rows(row).Item("activityID"))) = True
            Next
            ' Then create sub tabs :)
            If Me.tabMaterial.IsDisposed = False Then
                Me.tabMaterial.TabPages.Clear()
            End If
            For activity As Integer = 1 To ActivityCount
                If activities(activity) = True Then
                    Me.tabMaterial.TabPages.Add(tabPagesM(activity))
                End If
            Next

            Dim materials(eveData.Tables(0).Rows.Count, 9)
            With eveData.Tables(0)
                For row As Integer = 0 To .Rows.Count - 1
                    If Val(.Rows(row).Item("quantity")) > 0 Then
                        materials(row, 0) = "0"
                        materials(row, 1) = .Rows(row).Item("requiredTypeID").ToString.Trim
                        materials(row, 2) = .Rows(row).Item("typeName").ToString.Trim
                        materials(row, 3) = .Rows(row).Item("quantity").ToString.Trim
                        materials(row, 4) = .Rows(row).Item("damagePerJob").ToString.Trim
                        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
                            Case 0
                                materials(row, 5) = .Rows(row).Item("invCategories.categoryID").ToString.Trim
                                materials(row, 7) = .Rows(row).Item("invGroups.groupID").ToString.Trim
                            Case Else
                                materials(row, 5) = .Rows(row).Item("categoryID").ToString.Trim
                                materials(row, 7) = .Rows(row).Item("groupID").ToString.Trim
                        End Select
                        materials(row, 6) = .Rows(row).Item("categoryName").ToString.Trim
                        materials(row, 8) = .Rows(row).Item("groupName").ToString.Trim
                        materials(row, 9) = .Rows(row).Item("activityID").ToString.Trim
                    End If
                Next
            End With

            Dim itemcount As Integer = eveData.Tables(0).Rows.Count - 1
            Dim matCatID, matCatName, matGroupID, matGroupName As String
            For act As Integer = 1 To ActivityCount
                Dim materialsView As New ListView
                Select Case act
                    Case 1
                        materialsView = Me.lstM1
                    Case 2
                        materialsView = Me.lstM2
                    Case 3
                        materialsView = Me.lstM3
                    Case 4
                        materialsView = Me.lstM4
                    Case 5
                        materialsView = Me.lstM5
                    Case 6
                        materialsView = Me.lstM6
                    Case 7
                        materialsView = Me.lstM7
                    Case 8
                        materialsView = Me.lstM8
                End Select
                materialsView.Items.Clear()
                materialsView.BeginUpdate()
                For itemloop As Integer = 0 To itemcount
                    If materials(itemloop, 0) = "0" And materials(itemloop, 9) = act Then
                        matCatID = materials(itemloop, 5)
                        matCatName = materials(itemloop, 6)
                        matGroupID = materials(itemloop, 7)
                        matGroupName = materials(itemloop, 8)
                        ' Create a listview group
                        Dim lvGroup As New ListViewGroup
                        lvGroup.Name = matCatID & matGroupID
                        lvGroup.Header = matCatName & " / " & matGroupName
                        materialsView.Groups.Add(lvGroup)

                        For item As Integer = itemloop To itemcount
                            If materials(item, 9) = act And materials(item, 5) = matCatID And materials(item, 7) = matGroupID Then
                                Dim newItem As New ListViewItem
                                newItem.Name = materials(item, 1)
                                newItem.Text = materials(item, 2)
                                newItem.SubItems.Add(FormatNumber(materials(item, 3), 0, TriState.True, TriState.True, TriState.True))
                                newItem.Group = materialsView.Groups.Item(matCatID & matGroupID)
                                If act = 1 Then
                                    If materials(item, 5) = "4" Or materials(item, 7) = "280" Or materials(item, 7) = "334" Or materials(item, 7) = "873" Or materials(item, 7) = "536" Then
                                        newItem.SubItems.Add(EveHQ.Core.DataFunctions.Round(CDbl(1 + BPWFM) * CDbl(materials(item, 3)), 0))
                                        newItem.SubItems.Add(EveHQ.Core.DataFunctions.Round(CDbl(1 + BPWFP) * CDbl(materials(item, 3)), 0))
                                    Else
                                        newItem.SubItems.Add(FormatNumber(materials(item, 3), 0, TriState.True, TriState.True, TriState.True))
                                        newItem.SubItems.Add(FormatNumber(materials(item, 3), 0, TriState.True, TriState.True, TriState.True))
                                    End If
                                End If
                                materialsView.Items.Add(newItem)
                                materials(item, 0) = "1"
                            End If
                        Next
                    End If
                Next
                materialsView.EndUpdate()
            Next

            ' Add the relevant tab
            If Me.tabItem.TabPages.Contains(Me.tabMaterials) = False Then
                Me.tabItem.TabPages.Add(Me.tabMaterials)
            End If
        Else
            ' Remove the relevant tab
            If Me.tabItem.TabPages.Contains(Me.tabMaterials) = True Then
                Me.tabItem.TabPages.Remove(Me.tabMaterials)
            End If
        End If

        Call Me.GetRecyclingData(typeID.ToString)

    End Sub

    Private Sub GetRecyclingData(ByVal typeID As String)
        ' Fetch the recycling information
        Dim strSQL As String = "SELECT *"
        strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID = invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN invTypeMaterials ON invTypes.typeID = invTypeMaterials.materialTypeID"
        strSQL &= " WHERE (invTypeMaterials.typeID=" & typeID & ") ORDER BY invCategories.categoryName, invGroups.groupName"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

        If eveData.Tables(0).Rows.Count > 0 Then
            Me.tabMaterial.TabPages.Add(tabPagesM(6))
            Dim materials(eveData.Tables(0).Rows.Count, 9)
            With eveData.Tables(0)
                For row As Integer = 0 To .Rows.Count - 1
                    If Val(.Rows(row).Item("quantity")) > 0 Then
                        materials(row, 0) = "0"
                        materials(row, 1) = .Rows(row).Item("materialTypeID").ToString.Trim
                        materials(row, 2) = .Rows(row).Item("typeName").ToString.Trim
                        materials(row, 3) = .Rows(row).Item("quantity").ToString.Trim
                        materials(row, 4) = ""
                        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
                            Case 0
                                materials(row, 5) = .Rows(row).Item("invCategories.categoryID").ToString.Trim
                                materials(row, 7) = .Rows(row).Item("invGroups.groupID").ToString.Trim
                            Case Else
                                materials(row, 5) = .Rows(row).Item("categoryID").ToString.Trim
                                materials(row, 7) = .Rows(row).Item("groupID").ToString.Trim
                        End Select
                        materials(row, 6) = .Rows(row).Item("categoryName").ToString.Trim
                        materials(row, 8) = .Rows(row).Item("groupName").ToString.Trim
                        materials(row, 9) = ""
                    End If
                Next
            End With

            Dim itemcount As Integer = eveData.Tables(0).Rows.Count - 1
            Dim matCatID, matCatName, matGroupID, matGroupName As String
            Dim materialsView As ListView = Me.lstM6
            materialsView.Items.Clear()
            materialsView.BeginUpdate()
            For itemloop As Integer = 0 To itemcount
                If materials(itemloop, 0) = "0" Then
                    matCatID = materials(itemloop, 5)
                    matCatName = materials(itemloop, 6)
                    matGroupID = materials(itemloop, 7)
                    matGroupName = materials(itemloop, 8)
                    ' Create a listview group
                    Dim lvGroup As New ListViewGroup
                    lvGroup.Name = matCatID & matGroupID
                    lvGroup.Header = matCatName & " / " & matGroupName
                    materialsView.Groups.Add(lvGroup)
                    For item As Integer = itemloop To itemcount
                        If materials(item, 5) = matCatID And materials(item, 7) = matGroupID Then
                            Dim newItem As New ListViewItem
                            newItem.Name = materials(item, 1)
                            newItem.Text = materials(item, 2)
                            newItem.SubItems.Add(FormatNumber(materials(item, 3), 0, TriState.True, TriState.True, TriState.True))
                            newItem.Group = materialsView.Groups.Item(matCatID & matGroupID)
                            materialsView.Items.Add(newItem)
                            materials(item, 0) = "1"
                        End If
                    Next
                End If
            Next
            materialsView.EndUpdate()
        End If
    End Sub

    Private Sub GetComponents(ByVal typeID As Long, ByVal typeName As String)

        Dim bpTypeID As Long = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)
        Dim itemParents() As String = EveHQ.Core.DataFunctions.GetTypeParentInfo(typeID)
        Dim itemCatID As String = itemParents(4)
        Dim itemGroupID As String = itemParents(2)

        ' Select only the building activity (at the minute!)
        Dim strSQL As String = "SELECT *"
        strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID = invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN ramTypeRequirements ON invTypes.typeID = ramTypeRequirements.typeID"
        strSQL &= " WHERE (ramTypeRequirements.requiredTypeID=" & bpTypeID & " OR ramTypeRequirements.requiredTypeID=" & typeID & ") ORDER BY invCategories.categoryName, invGroups.groupName"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData.Tables(0).Rows.Count > 0 Then

            ' Work out what activities we have in the list
            Dim activities(ActivityCount) As Boolean
            Dim strActivity As String = ""
            For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                activities(Val(eveData.Tables(0).Rows(row).Item("activityID"))) = True
            Next
            ' Then create sub tabs :)
            Me.tabComponents.TabPages.Clear()
            For activity As Integer = 1 To ActivityCount
                If activities(activity) = True Then
                    Me.tabComponents.TabPages.Add(tabPagesC(activity))
                End If
            Next

            Dim materials(eveData.Tables(0).Rows.Count, 9)
            With eveData.Tables(0)
                For row As Integer = 0 To .Rows.Count - 1
                    If Val(.Rows(row).Item("quantity")) > 0 Then
                        materials(row, 0) = "0"
                        materials(row, 2) = .Rows(row).Item("typeName").ToString.Trim
                        materials(row, 3) = .Rows(row).Item("quantity").ToString.Trim
                        materials(row, 4) = .Rows(row).Item("damagePerJob").ToString.Trim
                        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
                            Case 0
                                materials(row, 1) = .Rows(row).Item("invTypes.typeID").ToString.Trim
                                materials(row, 5) = .Rows(row).Item("invCategories.categoryID").ToString.Trim
                                materials(row, 7) = .Rows(row).Item("invGroups.groupID").ToString.Trim
                            Case Else
                                materials(row, 1) = .Rows(row).Item("typeID").ToString.Trim
                                materials(row, 5) = .Rows(row).Item("categoryID").ToString.Trim
                                materials(row, 7) = .Rows(row).Item("groupID").ToString.Trim
                        End Select
                        materials(row, 6) = .Rows(row).Item("categoryName").ToString.Trim
                        materials(row, 8) = .Rows(row).Item("groupName").ToString.Trim
                        materials(row, 9) = .Rows(row).Item("activityID").ToString.Trim
                    End If
                Next
            End With

            Dim itemcount As Integer = eveData.Tables(0).Rows.Count - 1
            Dim matCatID, matCatName, matGroupID, matGroupName As String
            For act As Integer = 1 To ActivityCount
                Dim materialsView As New ListView
                Select Case act
                    Case 1
                        materialsView = Me.lstC1
                    Case 2
                        materialsView = Me.lstC2
                    Case 3
                        materialsView = Me.lstC3
                    Case 4
                        materialsView = Me.lstC4
                    Case 5
                        materialsView = Me.lstC5
                    Case 6
                        materialsView = Me.lstC6
                    Case 7
                        materialsView = Me.lstC7
                    Case 8
                        materialsView = Me.lstC8
                    Case 9
                        materialsView = Me.lstC9
                End Select
                materialsView.Items.Clear()
                materialsView.BeginUpdate()
                For itemloop As Integer = 0 To itemcount
                    If materials(itemloop, 0) = "0" And materials(itemloop, 9) = act Then
                        matCatID = materials(itemloop, 5)
                        matCatName = materials(itemloop, 6)
                        matGroupID = materials(itemloop, 7)
                        matGroupName = materials(itemloop, 8)
                        ' Create a listview group
                        Dim lvGroup As New ListViewGroup
                        lvGroup.Name = matCatID & matGroupID
                        lvGroup.Header = matCatName & " / " & matGroupName
                        materialsView.Groups.Add(lvGroup)

                        For item As Integer = itemloop To itemcount
                            If materials(item, 9) = act And materials(item, 5) = matCatID And materials(item, 7) = matGroupID Then
                                Dim newItem As New ListViewItem
                                newItem.Name = materials(item, 1)
                                newItem.Text = materials(item, 2)
                                newItem.SubItems.Add(FormatNumber(materials(item, 3), 0, TriState.True, TriState.True, TriState.True))
                                newItem.Group = materialsView.Groups.Item(matCatID & matGroupID)
                                If act = 1 Then
                                    If itemCatID = "4" Or itemGroupID = "280" Or itemGroupID = "334" Or itemGroupID = "873" Or itemGroupID = "536" Then
                                        ' Do we need the BPWF here? I think so!
                                        BPWFC = EveHQ.Core.DataFunctions.GetBPWF(materials(item, 1))
                                        BPWFMC = ((1 / BPWFC) / (1 + nudMELevelC.Value))
                                        BPWFPC = BPWFMC + (0.25 - (0.05 * displayPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ProductionEfficiency)))
                                        newItem.SubItems.Add(EveHQ.Core.DataFunctions.Round(CDbl(1 + BPWFMC) * CDbl(materials(item, 3)), 0))
                                        newItem.SubItems.Add(EveHQ.Core.DataFunctions.Round(CDbl(1 + BPWFPC) * CDbl(materials(item, 3)), 0))
                                    Else
                                        newItem.SubItems.Add(FormatNumber(materials(item, 3), 0, TriState.True, TriState.True, TriState.True))
                                        newItem.SubItems.Add(FormatNumber(materials(item, 3), 0, TriState.True, TriState.True, TriState.True))
                                    End If
                                End If
                                materialsView.Items.Add(newItem)
                                materials(item, 0) = "1"
                            End If
                        Next
                    End If
                Next
                materialsView.EndUpdate()
            Next

            ' Add the relevant tab
            If Me.tabItem.TabPages.Contains(Me.tabComponent) = False Then
                Me.tabItem.TabPages.Add(Me.tabComponent)
            End If
        Else
            ' Remove the relevant tab
            If Me.tabItem.TabPages.Contains(Me.tabComponent) = True Then
                Me.tabItem.TabPages.Remove(Me.tabComponent)
            End If
        End If

    End Sub

    Public Sub GenerateSkills(ByVal typeID As Long, ByVal typeName As String)

        Dim ItemUsable As Boolean = True
        skillsNeeded.Clear()

        tvwReqs.Nodes.Clear()
        Dim skillsRequired As Boolean = False

        If displayPilot IsNot Nothing Then
            For Each skillID As String In itemSkills.Keys
                skillsRequired = True

                Dim level As Integer = 1
                Dim pointer(20) As Integer
                Dim parent(20) As Integer
                Dim skillName(20) As String
                Dim skillLevel(20) As String
                pointer(level) = 1
                parent(level) = skillID

                Dim strTree As String = ""
                Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
                Dim curSkill As Integer = CInt(skillID)
                Dim curLevel As Integer = itemSkills(skillID)
                Dim counter As Integer = 0
                Dim curNode As TreeNode = New TreeNode
                curNode.Text = cSkill.Name & " (Level " & curLevel & ")"

                ' Write the skill we are querying as the first (parent) node
                Dim skillTrained As Boolean = False
                Dim myLevel As Integer = 0
                skillTrained = False
                If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
                    If displayPilot.PilotSkills.Contains(cSkill.Name) Then
                        Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                        mySkill = displayPilot.PilotSkills(cSkill.Name)
                        myLevel = CInt(mySkill.Level)
                        If myLevel >= curLevel Then skillTrained = True
                        If skillTrained = True Then
                            curNode.ForeColor = Color.LimeGreen
                            curNode.ToolTipText = "Already Trained"
                        Else
                            Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
                            If planLevel = 0 Then
                                curNode.ForeColor = Color.Red
                                curNode.ToolTipText = "Not trained & no planned training"
                            Else
                                curNode.ToolTipText = "Planned training to Level " & planLevel
                                If planLevel >= curLevel Then
                                    curNode.ForeColor = Color.Blue
                                Else
                                    curNode.ForeColor = Color.Orange
                                End If
                            End If
                            skillsNeeded.Add(cSkill.Name & curLevel)
                            ItemUsable = False
                        End If
                    Else
                        Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, cSkill.Name, curLevel)
                        If planLevel = 0 Then
                            curNode.ForeColor = Color.Red
                            curNode.ToolTipText = "Not trained & no planned training"
                        Else
                            curNode.ToolTipText = "Planned training to Level " & planLevel
                            If planLevel >= curLevel Then
                                curNode.ForeColor = Color.Blue
                            Else
                                curNode.ForeColor = Color.Orange
                            End If
                        End If
                        skillsNeeded.Add(cSkill.Name & curLevel)
                        ItemUsable = False
                    End If
                End If
                tvwReqs.Nodes.Add(curNode)

                If cSkill.PreReqSkills.Count > 0 Then
                    Dim subSkill As EveHQ.Core.EveSkill
                    For Each subSkillID As String In cSkill.PreReqSkills.Keys
                        subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
                        Call AddPreReqsToTree(subSkill, cSkill.PreReqSkills(subSkillID), curNode, ItemUsable)
                    Next
                End If
            Next

            If skillsRequired = True Then
                tvwReqs.ExpandAll()
                If Me.tabItem.TabPages.Contains(Me.tabSkills) = False Then
                    Me.tabItem.TabPages.Add(Me.tabSkills)
                End If
                If displayPilot.Name <> "" Then
                    If ItemUsable = True Then
                        lblUsable.Text = displayPilot.Name & " has the skills to use this item."
                        lblUsableTime.Text = ""
                        btnAddSkills.Enabled = False
                        btnViewSkills.Enabled = False
                    Else
                        Dim usableTime As Long = 0
                        Dim skillNo As Integer = 0
                        If skillsNeeded.Count > 1 Then
                            Do
                                Dim skill As String = skillsNeeded(skillNo)
                                Dim skillName As String = skill.Substring(0, skill.Length - 1)
                                Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                                Dim highestLevel As Integer = 0
                                Dim skillno2 As Integer = skillNo + 1
                                Do
                                    If skillno2 < skillsNeeded.Count Then
                                        Dim skill2 As String = skillsNeeded(skillno2)
                                        Dim skillName2 As String = skill2.Substring(0, skill2.Length - 1)
                                        Dim skillLvl2 As Integer = CInt(skill2.Substring(skill2.Length - 1, 1))
                                        If skillName = skillName2 Then
                                            If skillLvl >= skillLvl2 Then
                                                skillsNeeded.RemoveAt(skillno2)
                                            Else
                                                skillsNeeded.RemoveAt(skillNo)
                                                skillNo = -1 : skillno2 = 0
                                                Exit Do
                                            End If
                                        Else
                                            skillno2 += 1
                                        End If
                                    End If
                                Loop Until skillno2 >= skillsNeeded.Count
                                skillNo += 1
                            Loop Until skillNo >= skillsNeeded.Count - 1
                        End If
                        skillsNeeded.Reverse()
                        For Each skill As String In skillsNeeded
                            Dim skillName As String = skill.Substring(0, skill.Length - 1)
                            Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                            Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(skillName)
                            usableTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(displayPilot, cSkill, skillLvl)
                        Next
                        lblUsable.Text = displayPilot.Name & " doesn't have the skills to use this item."
                        lblUsableTime.Text = "Training Time: " & EveHQ.Core.SkillFunctions.TimeToString(usableTime)
                        btnAddSkills.Enabled = True
                        btnViewSkills.Enabled = True
                    End If
                Else
                    lblUsable.Text = "No pilot selected to calculate skill time."
                    lblUsableTime.Text = ""
                    btnAddSkills.Enabled = False
                    btnViewSkills.Enabled = False
                End If
            Else
                If Me.tabItem.TabPages.Contains(Me.tabSkills) = True Then
                    Me.tabItem.TabPages.Remove(Me.tabSkills)
                End If
                lblUsable.Text = "No skills required for this item."
                lblUsableTime.Text = ""
                btnAddSkills.Enabled = False
                btnViewSkills.Enabled = False
            End If
        Else
            lblUsable.Text = "No pilots loaded to calculate skill time."
            lblUsableTime.Text = ""
            btnAddSkills.Enabled = False
            btnViewSkills.Enabled = False
        End If

    End Sub

    Private Sub AddPreReqsToTree(ByVal newSkill As EveHQ.Core.EveSkill, ByVal curLevel As Integer, ByVal curNode As TreeNode, ByRef itemUsable As Boolean)
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        Dim newNode As TreeNode = New TreeNode
        newNode.Name = newSkill.Name & " (Level " & curLevel & ")"
        newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
        ' Check status of this skill
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 And displayPilot.Updated = True Then
            skillTrained = False
            myLevel = 0
            If displayPilot.PilotSkills.Contains(newSkill.Name) Then
                Dim mySkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
                mySkill = CType(displayPilot.PilotSkills(newSkill.Name), Core.PilotSkill)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
            End If
            If skillTrained = True Then
                newNode.ForeColor = Color.LimeGreen
                newNode.ToolTipText = "Already Trained"
            Else
                Dim planLevel As Integer = EveHQ.Core.SkillQueueFunctions.IsPlanned(displayPilot, newSkill.Name, curLevel)
                If planLevel = 0 Then
                    newNode.ForeColor = Color.Red
                    newNode.ToolTipText = "Not trained & no planned training"
                Else
                    newNode.ToolTipText = "Planned training to Level " & planLevel
                    If planLevel >= curLevel Then
                        newNode.ForeColor = Color.Blue
                    Else
                        newNode.ForeColor = Color.Orange
                    End If
                End If
                skillsNeeded.Add(newSkill.Name & curLevel)
                itemUsable = False
            End If
        End If
        curNode.Nodes.Add(newNode)

        If newSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As EveHQ.Core.EveSkill
            For Each subSkillID As String In newSkill.PreReqSkills.Keys
                If subSkillID <> newSkill.ID Then
                    subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
                    Call AddPreReqsToTree(subSkill, newSkill.PreReqSkills(subSkillID), newNode, itemUsable)
                End If
            Next
        End If
    End Sub

    Private Sub LoadFittingAttributes()
        fittingAtts.Add("11")
        fittingAtts.Add("48")
        fittingAtts.Add("12")
        fittingAtts.Add("13")
        fittingAtts.Add("14")
        fittingAtts.Add("101")
        fittingAtts.Add("102")
        fittingAtts.Add("30")
        fittingAtts.Add("50")
        fittingAtts.Add("1153")
        fittingAtts.Add("1132")
        fittingAtts.Add("1137")
    End Sub

    Private Sub LoadShipCertRecommendations()
        ' Parse the WHEffects resource
        ShipCerts = New SortedList(Of String, ArrayList) ' ShipID, ArrayList of Certs
        Dim Certs() As String = My.Resources.ShipCerts.Split((ControlChars.CrLf).ToCharArray)
        For Each Cert As String In Certs
            If Cert <> "" Then
                Dim CertData() As String = Cert.Split(",".ToCharArray)
                If ShipCerts.ContainsKey(CertData(0)) = False Then
                    ShipCerts.Add(CertData(0), New ArrayList)
                End If
                Dim currentCerts As ArrayList = ShipCerts(CertData(0))
                currentCerts.Add(CertData(1))
            End If
        Next
    End Sub

    Private Sub GenerateFitting()
        If itemFitting.Count <> 0 Then
            lstFitting.Items.Clear()
            For Each attributes() As String In itemFitting
                Dim lstItem As New ListViewItem
                lstItem.Name = attributes(1)
                lstItem.Text = "(" & attributes(1) & ")  " & attributes(2)
                lstItem.SubItems.Add(attributes(3) & attributes(4))
                attributes(0) = "1"
                lstFitting.Items.Add(lstItem)
            Next
            If Me.tabItem.TabPages.Contains(Me.tabFitting) = False Then
                Me.tabItem.TabPages.Add(Me.tabFitting)
            End If
        Else
            If Me.tabItem.TabPages.Contains(Me.tabFitting) = True Then
                Me.tabItem.TabPages.Remove(Me.tabFitting)
            End If
        End If
    End Sub

    Private Sub tvwReqs_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvwReqs.MouseMove
        Dim tn As TreeNode = tvwReqs.GetNodeAt(e.X, e.Y)
        If Not (tn Is Nothing) Then
            Dim currentNodeIndex As Integer = tn.Index
            If currentNodeIndex <> oldNodeIndex Then
                oldNodeIndex = currentNodeIndex
                If Not (Me.SkillToolTip Is Nothing) And Me.SkillToolTip.Active Then
                    Me.SkillToolTip.Active = False 'turn it off 
                End If
                Me.SkillToolTip.SetToolTip(tvwReqs, tn.ToolTipText)
                Me.SkillToolTip.Active = True 'make it active so it can show 
            End If
        End If
    End Sub

    Private Sub tvwReqs_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwReqs.NodeMouseClick
        tvwReqs.SelectedNode = e.Node
    End Sub

    Private Sub ctxSkills_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxSkills.Opening
        If ctxSkills.SourceControl Is Me.tvwReqs Then
            Dim curNode As TreeNode = New TreeNode
            curNode = tvwReqs.SelectedNode
            Dim skillName As String = ""
            Dim skillID As String = ""
            skillName = curNode.Text
            If InStr(skillName, "(Level") <> 0 Then
                skillName = skillName.Substring(0, InStr(skillName, "(Level") - 1).Trim(Chr(32))
            End If
            skillID = EveHQ.Core.SkillFunctions.SkillNameToID(skillName)
            mnuSkillName.Text = skillName
            mnuSkillName.Tag = skillID
        End If
    End Sub

    Private Sub mnuViewDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewDetails.Click
        ' Get the skill ID and show the details!
        Dim skillID As String
        skillID = mnuSkillName.Tag
        Call LoadItemID(skillID)
        Call Me.AddToNavigation(itemTypeName)
    End Sub

    Private Sub picBP_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles picBP.DoubleClick
        Call LoadItemID(picBP.Tag)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub

    Private Sub btnAddSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSkills.Click
        Call Me.AddNeededSkillsToQueue()
    End Sub

    Private Sub AddNeededSkillsToQueue()
        Dim selQ As New frmSelectQueue
        selQ.itemTypeID = itemTypeID
        selQ.itemTypeName = itemTypeName
        selQ.skillsNeeded = skillsNeeded
        selQ.DisplayPilotName = displayPilot.Name
        selQ.ShowDialog()
        EveHQ.Core.SkillQueueFunctions.StartQueueRefresh = True
        Call Me.GenerateSkills(itemTypeID, itemTypeName)
    End Sub

    Private Sub btnViewSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewSkills.Click
        If displayPilot.Name <> "" Then
            If skillsNeeded.Count <> 0 Then
                Dim msg As String = ""
                For Each skill As String In skillsNeeded
                    Dim skillName As String = skill.Substring(0, skill.Length - 1)
                    Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                    msg &= skillName & " - Lvl " & skillLvl & ControlChars.CrLf
                Next
                MessageBox.Show(msg, displayPilot.Name & "'s Skills Required For " & itemTypeName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show(displayPilot.Name & " has already trained all necessary skills to use this item.", "Already Trained!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("There is no pilot selected to show skill information", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub lstAttributes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstAttributes.DoubleClick
        Dim id As String = lstAttributes.SelectedItems(0).Tag
        If id <> "" Then
            Call Me.LoadItemID(id)
            ' Alter navigation
            Call Me.AddToNavigation(itemTypeName)
        End If
    End Sub

    Private Sub lstM1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM1.DoubleClick
        Dim id As String = lstM1.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstM2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM2.DoubleClick
        Dim id As String = lstM2.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstM3_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM3.DoubleClick
        Dim id As String = lstM3.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstM4_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM4.DoubleClick
        Dim id As String = lstM4.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstM5_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM5.DoubleClick
        Dim id As String = lstM5.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstM6_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM6.DoubleClick
        Dim id As String = lstM6.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstM7_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM7.DoubleClick
        Dim id As String = lstM7.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstM8_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM8.DoubleClick
        Dim id As String = lstM8.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub

    Private Sub nudMELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMELevel.ValueChanged
        ' Set the column text
        Me.colM1ME.Text = "ME " & nudMELevel.Value
        ' re-calcualte the new waste factors
        If nudMELevel.Value >= 0 Then
            BPWFM = ((1 / BPWF) / (1 + nudMELevel.Value))
        Else
            BPWFM = ((1 / BPWF) * (1 - nudMELevel.Value))
        End If
        BPWFP = BPWFM + (0.25 - (0.05 * displayPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ProductionEfficiency)))
        Call Me.GetMaterials(itemTypeID, itemTypeName)
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        If Len(txtSearch.Text) > 2 Then
            Dim strSearch As String = txtSearch.Text.Trim.ToLower
            Dim results As New SortedList(Of String, String)
            For Each item As String In EveHQ.Core.HQ.itemList.Keys
                If item.ToLower.Contains(strSearch) Then
                    results.Add(item, item)
                End If
            Next
            lstSearch.BeginUpdate()
            lstSearch.Items.Clear()
            For Each item As String In results.Values
                lstSearch.Items.Add(item)
            Next
            lstSearch.EndUpdate()
            lblSearchCount.Text = lstSearch.Items.Count & " items found"
        End If
    End Sub

    Private Sub lstSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSearch.SelectedIndexChanged
        Dim selItem As String = lstSearch.SelectedItem
        If selItem <> "" Then
            Call LoadItemID(EveHQ.Core.HQ.itemList(selItem))
            ' Alter navigation
            Call Me.AddToNavigation(itemTypeName)
        End If
    End Sub

    Private Sub tvwBrowse_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvwBrowse.BeforeExpand
        Call LoadTreeViewItems(sender, e.Node)
    End Sub

    Private Sub tvwBrowse_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tvwBrowse.KeyDown
        Dim cNode As TreeNode = tvwBrowse.SelectedNode
        If cNode IsNot Nothing Then
            If cNode.Level = 2 And (e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Space) Then
                Call LoadItemID(cNode.Name)
                ' Alter navigation
                Call Me.AddToNavigation(itemTypeName)
            End If
            tvwBrowse.Focus()
        End If
    End Sub

    Private Sub tvwBrowse_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwBrowse.NodeMouseClick
        If e.Node.Level = 2 Then
            Call LoadItemID(e.Node.Name)
            ' Alter navigation
            Call Me.AddToNavigation(itemTypeName)
        End If
    End Sub

    Private Sub LoadTreeViewItems(ByVal sender As Object, ByVal e As TreeNode)
        Select Case e.Level
            Case 1
                If e.Tag = "" Then
                    tvwBrowse.BeginUpdate()
                    e.Nodes.Clear()
                    ' Load the Browser with items
                    Dim newNode As TreeNode
                    For Each item As String In EveHQ.Core.HQ.itemList.Keys
                        newNode = New TreeNode
                        newNode.Text = item ' Name
                        newNode.Name = EveHQ.Core.HQ.itemList(item) ' ID
                        If e.Name = EveHQ.Core.HQ.itemData(newNode.Name).Group.ToString Then
                            ' Check published flag
                            If EveHQ.Core.HQ.EveHQSettings.IBShowAllItems = True Then
                                e.Nodes.Add(newNode)
                            Else
                                If EveHQ.Core.HQ.itemData(newNode.Name).Published = True Then
                                    e.Nodes.Add(newNode)
                                End If
                            End If
                        End If
                    Next
                    tvwBrowse.EndUpdate()
                    e.Tag = "1"
                End If
            Case 2
                Call LoadItemID(e.Name)
                ' Alter navigation
                Call Me.AddToNavigation(itemTypeName)
        End Select
    End Sub

    Private Sub ssLblID_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ssLblID.DoubleClick
        Dim goItemID As String
        goItemID = InputBox(ControlChars.CrLf & "Please enter the itemID to jump to...", "Jump to ItemID")
        If EveHQ.Core.HQ.itemList.ContainsValue(goItemID) = True Then
            Call Me.LoadItemID(goItemID)
            ' Alter navigation
            Call Me.AddToNavigation(itemTypeName)
        End If
    End Sub

    Private Sub ssDBLocation_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ssDBLocation.DoubleClick
        If itemGroupID <> "" Then
            Me.tvwBrowse.Select()
            Me.tvwBrowse.CollapseAll()
            Dim selnode As TreeNode = tvwBrowse.Nodes(itemCatID).Nodes(itemGroupID)
            Me.tvwBrowse.SelectedNode = selnode
            Me.tvwBrowse.SelectedNode.Expand()
            Me.tabBrowser.SelectTab(Me.tabBrowse)
        End If
    End Sub

    Private Sub nudMELevelC_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMELevelC.ValueChanged
        ' Set the column text
        Me.colC1ME.Text = "ME " & nudMELevelC.Value
        ' re-calcualte the new waste factors
        BPWFMC = ((1 / BPWF) / (1 + nudMELevelC.Value))
        BPWFPC = BPWFMC + (0.25 - (0.05 * displayPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ProductionEfficiency)))
        Call Me.GetComponents(itemTypeID, itemTypeName)
    End Sub

    Private Sub lstC1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC1.DoubleClick
        Dim id As String = lstC1.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC2.DoubleClick
        Dim id As String = lstC2.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC3_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC3.DoubleClick
        Dim id As String = lstC3.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC4_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC4.DoubleClick
        Dim id As String = lstC4.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC5_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC5.DoubleClick
        Dim id As String = lstC5.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC6_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC6.DoubleClick
        Dim id As String = lstC6.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC7_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC7.DoubleClick
        Dim id As String = lstC7.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC8_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC8.DoubleClick
        Dim id As String = lstC8.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub
    Private Sub lstC9_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstC9.DoubleClick
        Dim id As String = lstC9.SelectedItems(0).Name
        Call Me.LoadItemID(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub

#Region "Navigation Routines"
    Private Sub AddToNavigation(ByVal itemName As String)
        Dim maxNav As Integer = navigated.Count
        If navigated.Item("n" & maxNav) <> itemName Then
            navigated.Add("n" & (maxNav + 1), itemName)
            curNavigation += 1
        End If
        Call Me.DeleteNextItems()
        Call Me.DrawBackMenu()
        Call Me.DrawForwardMenu()
    End Sub
    Private Sub DrawBackMenu()
        If navigated.Count > 1 Then
            ctxBack.Items.Clear()
            For menu As Integer = curNavigation - 1 To Math.Max(1, curNavigation - 10) Step -1
                Dim mnuItem As New ToolStripMenuItem
                mnuItem.Name = menu
                mnuItem.Text = navigated.Item("n" & menu)
                ctxBack.Items.Add(mnuItem)
                AddHandler mnuItem.Click, AddressOf Me.NavMenuClickHandler
            Next
            sbtnBack.Enabled = True
        Else
            sbtnBack.Enabled = False
            sbtnForward.Enabled = False
        End If
    End Sub
    Private Sub DrawForwardMenu()
        If navigated.Count > 1 Then
            ctxForward.Items.Clear()
            If curNavigation <> navigated.Count Then
                For menu As Integer = curNavigation + 1 To Math.Min(navigated.Count, curNavigation + 10) Step 1
                    Dim mnuItem As New ToolStripMenuItem
                    mnuItem.Name = menu
                    mnuItem.Text = navigated.Item("n" & menu)
                    ctxForward.Items.Add(mnuItem)
                    AddHandler mnuItem.Click, AddressOf Me.NavMenuClickHandler
                Next
                sbtnForward.Enabled = True
            End If
        Else
            sbtnBack.Enabled = False
            sbtnForward.Enabled = False
        End If
    End Sub
    Private Sub DeleteNextItems()
        If navigated.Count > 1 Then
            For menu As Integer = curNavigation + 1 To navigated.Count Step 1
                navigated.Remove("n" & menu)
            Next
            sbtnForward.Enabled = False
        Else
            sbtnBack.Enabled = False
            sbtnForward.Enabled = False
        End If
    End Sub
    Private Sub sbtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbtnBack.Click
        curNavigation -= 1
        Call Me.GoNavigate()
    End Sub
    Private Sub sbtnForward_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbtnForward.Click
        curNavigation += 1
        Call Me.GoNavigate()
    End Sub
    Private Sub GoNavigate()
        Dim itemName As String = navigated.Item("n" & curNavigation)
        Dim itemID As String = EveHQ.Core.HQ.itemList.Item(itemName)
        Call LoadItemID(itemID)
        Call Me.DrawBackMenu()
        Call Me.DrawForwardMenu()
        If curNavigation <= 1 Then
            sbtnBack.Enabled = False
        Else
            sbtnBack.Enabled = True
        End If
        If curNavigation >= navigated.Count Then
            sbtnForward.Enabled = False
        Else
            sbtnForward.Enabled = True
        End If
    End Sub
    Private Sub NavMenuClickHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clickedItem As New ToolStripMenuItem
        clickedItem = CType(sender, ToolStripMenuItem)
        curNavigation = CInt(clickedItem.Name)
        Call Me.GoNavigate()
    End Sub

#End Region

    Private Sub cboAttSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAttSearch.SelectedIndexChanged
        ' Fetch attributeID
        Dim attID As String = PlugInData.AttributeList.Item(cboAttSearch.SelectedItem)
        eveData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM dgmTypeAttributes WHERE attributeID=" & attID & ";")
        Dim itemID As String = ""
        Dim itemName As String = ""
        Dim itemValue As Double = 0
        lstAttSearch.Items.Clear()
        lstAttSearch.BeginUpdate()
        For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            itemID = eveData.Tables(0).Rows(item).Item("typeID")
            itemName = EveHQ.Core.HQ.itemData(itemID).Name
            Dim lstItem As New ListViewItem
            lstItem.Text = itemName
            lstItem.Name = itemID
            If IsDBNull(eveData.Tables(0).Rows(item).Item("valueFloat")) = False Then
                itemValue = eveData.Tables(0).Rows(item).Item("valueFloat")
            Else
                itemValue = eveData.Tables(0).Rows(item).Item("valueInt")
            End If
            lstItem.ToolTipText = itemName & " - " & itemValue
            lstItem.SubItems.Add(itemValue)
            lstAttSearch.Items.Add(lstItem)
        Next
        lstAttSearch.EndUpdate()
        lblAttSearchCount.Text = lstAttSearch.Items.Count & " items found"
    End Sub

    Private Sub lstAttSearch_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lstAttSearch.ColumnClick
        If lstAttSearch.Tag = e.Column Then
            Me.lstAttSearch.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lstAttSearch.Tag = -1
        Else
            Me.lstAttSearch.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lstAttSearch.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lstAttSearch.Sort()
    End Sub

    Private Sub lstAttSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstAttSearch.SelectedIndexChanged
        If lstAttSearch.SelectedItems.Count > 0 Then
            Dim selItem As String = lstAttSearch.SelectedItems(0).Text
            If selItem <> "" Then
                Call LoadItemID(EveHQ.Core.HQ.itemList(selItem))
                ' Alter navigation
                Call Me.AddToNavigation(itemTypeName)
            End If
        End If
    End Sub

    Private Sub lstComparisons_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lstComparisons.ColumnClick
        If lstComparisons.Tag = e.Column Then
            Me.lstComparisons.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Ascending)
            lstComparisons.Tag = -1
        Else
            Me.lstComparisons.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Name(e.Column, SortOrder.Descending)
            lstComparisons.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lstComparisons.Sort()
    End Sub

    Private Sub chkShowAllColumns_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowAllColumns.CheckedChanged
        Call Me.DrawComparatives()
    End Sub

#Region "EveCentral Routines"
    Private Property EveCentralDataFound() As Boolean
        Get
            Return bEveCentralDataFound
        End Get
        Set(ByVal value As Boolean)
            bEveCentralDataFound = value
            Me.Invoke(New MethodInvoker(AddressOf SetEveCentralTab))
        End Set
    End Property
    Private Sub SetEveCentralTab()
        If bEveCentralDataFound = True Then
            If Me.tabItem.TabPages.Contains(Me.tabEveCentral) = False Then
                Me.tabItem.TabPages.Add(Me.tabEveCentral)
            End If
        Else
            If Me.tabItem.TabPages.Contains(Me.tabEveCentral) = False Then
                Me.tabItem.TabPages.Remove(Me.tabEveCentral)
            End If
        End If
    End Sub
    Private Sub lstEveCentral_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstEveCentral.DoubleClick
        Try
            Process.Start("http://eve-central.com/home/quicklook.html?typeid=" & Me.itemTypeID)
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please ensure a default browser has been configured and that the http protocol is registered to an application.", "Error Starting External Process", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

#End Region

    Private Sub lblUsableTime_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblUsableTime.LinkClicked
        Call Me.AddNeededSkillsToQueue()
    End Sub

    Private Sub lvwDepend_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwDepend.DoubleClick
        Dim id As String = lvwDepend.SelectedItems(0).Name
        Call Me.LoadItemName(id)
        ' Alter navigation
        Call Me.AddToNavigation(itemTypeName)
    End Sub

    Private Sub chkBrowseNonPublished_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBrowseNonPublished.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.IBShowAllItems = chkBrowseNonPublished.Checked
        Call Me.LoadBrowserGroups()
    End Sub

#Region "Wanted List Routines"
    Private Sub btnWantedAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWantedAdd.Click
        If itemTypeName <> "" Then
            If EveHQ.Core.HQ.EveHQSettings.WantedList.Contains(itemTypeName) = False Then
                EveHQ.Core.HQ.EveHQSettings.WantedList.Add(itemTypeName, itemTypeName)
                lvwWanted.Items.Add(itemTypeName, itemTypeName, 0)
            End If
        End If
    End Sub
    Private Sub btnClearWantedList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearWantedList.Click
        Dim reply As Integer = MessageBox.Show("Are you sure you want to clear the Wanted List?", "Confirm Clear?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            EveHQ.Core.HQ.EveHQSettings.WantedList.Clear()
            lvwWanted.Items.Clear()
        End If
    End Sub
    Private Sub btnRemoveWantedItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveWantedItem.Click
        If lvwWanted.SelectedItems.Count = 0 Then
            MessageBox.Show("You must select an item before you can remove it!", "Remove Item Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            For Each item As ListViewItem In lvwWanted.SelectedItems
                EveHQ.Core.HQ.EveHQSettings.WantedList.Remove(item.Text)
            Next
            Call Me.DrawWantedList()
        End If
    End Sub
    Private Sub DrawWantedList()
        lvwWanted.BeginUpdate()
        lvwWanted.Items.Clear()
        Dim UnknownItems As New ArrayList
        For Each item As String In EveHQ.Core.HQ.EveHQSettings.WantedList.Keys
            If EveHQ.Core.HQ.itemList.ContainsKey(item) = True Then
                Dim newItem As New ListViewItem
                newItem.Name = item
                newItem.Text = item
                newItem.SubItems.Add(FormatNumber(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(item)), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvwWanted.Items.Add(newItem)
            Else
                MessageBox.Show(item & " does not appear to be a valid item and will be removed from the Wanted List.", "Unknown Item", MessageBoxButtons.OK, MessageBoxIcon.Information)
                UnknownItems.Add(item)
            End If
        Next
        If UnknownItems.Count > 0 Then
            For Each unknownItem As String In UnknownItems
                EveHQ.Core.HQ.EveHQSettings.WantedList.Remove(unknownItem)
            Next
        End If
        lvwWanted.EndUpdate()
    End Sub
    Private Sub lvwWanted_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwWanted.Click
        Dim selItem As String = lvwWanted.SelectedItems(0).Text
        If selItem <> "" Then
            Call LoadItemID(EveHQ.Core.HQ.itemList(selItem))
            ' Alter navigation
            Call Me.AddToNavigation(itemTypeName)
        End If
    End Sub
    Private Sub btnRefreshWantedList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshWantedList.Click
        Call Me.DrawWantedList()
    End Sub
#End Region

End Class