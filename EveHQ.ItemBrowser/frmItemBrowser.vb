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
Imports System.Drawing
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml


Public Class frmItemBrowser

    Const ActivityCount As Integer = 9
    Dim metaParentID As Long
    Dim metaItemCount As Long
    Dim itemTypeID As Long
    Dim itemTypeName As String = ""
    Dim itemGroupID As String = ""
    Dim itemGroupName As String = ""
    Dim itemCatID As String = ""
    Dim itemCatName As String = ""
    Dim itemPublished As Boolean = False
    Dim itemVariations(,) As String
    Dim itemSkills As New Dictionary(Of String, Integer)
    Dim itemFitting As New Collection
    Dim fittingAtts As New ArrayList
	Dim tabPagesM(ActivityCount) As DevComponents.DotNetBar.TabItem
	Dim tabPagesC(ActivityCount) As DevComponents.DotNetBar.TabItem
    Dim itemStart As DateTime
    Dim itemEnd As DateTime
    Dim itemTime As TimeSpan
    Dim oldNodeIndex As Integer = -1
    Dim skillsNeeded As New List(Of String)
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
    Dim BasicItemData As DataSet
    Dim BasicItem As New EveHQ.Core.EveItem

    Private Sub LoadItemName(ByVal itemName As String)
        Dim strSQL As String = "SELECT * FROM invTypes WHERE typeName LIKE '" & itemName & "'"
        Call LoadItem(strSQL)
    End Sub
    Private Sub LoadItemID(ByVal itemID As String)
        Dim strSQL As String = "SELECT * FROM invTypes WHERE typeID =" & itemID
        Call LoadItem(strSQL)
    End Sub
    Private Sub LoadItem(ByVal strSQL As String)
        Dim times(11) As DateTime
        times(0) = Now
		' Reset Item Skills
		itemSkills.Clear()
		'Me.tiSkills.Visible = False
		'Me.tiFitting.Visible = False
		'Me.tiMaterials.Visible = False
		'Me.tiComponent.Visible = False
		'Me.tiRecommended.Visible = False
		'Me.tiVariations.Visible = False
		'Me.tiDependencies.Visible = False
		'Me.tiEveCentral.Visible = False
        BasicItemData = EveHQ.Core.DataFunctions.GetData(strSQL)
        times(1) = Now
        If BasicItemData IsNot Nothing Then
            If BasicItemData.Tables(0).Rows.Count > 0 Then
                itemTypeID = BasicItemData.Tables(0).Rows(0).Item("typeID")
                BasicItem = EveHQ.Core.HQ.itemData(itemTypeID)
                itemTypeName = BasicItemData.Tables(0).Rows(0).Item("typeName")
                itemGroupID = BasicItemData.Tables(0).Rows(0).Item("groupID")
                itemCatID = EveHQ.Core.HQ.groupCats(itemGroupID)
                itemGroupName = EveHQ.Core.HQ.itemGroups(itemGroupID)
                itemCatName = EveHQ.Core.HQ.itemCats(itemCatID)
                itemPublished = CBool(BasicItemData.Tables(0).Rows(0).Item("published"))
                lblItem.Text = itemTypeName
				lblID.Text = "ID: " & itemCatID & "/" & itemGroupID & "/" & itemTypeID
                lblDescription.Text = BasicItemData.Tables(0).Rows(0).Item("description")
                times(2) = Now
                Call GetAttributes(itemTypeID, itemTypeName)
                Call GetEffects(itemTypeID, itemTypeName)
                times(3) = Now
                Call GenerateSkills(itemTypeID, itemTypeName)
                times(4) = Now
                Call GetVariations(itemTypeID, itemTypeName)
                times(5) = Now
                Call GenerateFitting()
                times(6) = Now
                If (itemCatName = "Ship") Then
                    Call GetRecommendations(itemTypeID, itemTypeName)
                End If
                times(7) = Now
                Call GetMaterials(itemTypeID, itemTypeName)
                times(8) = Now
                Call GetComponents(itemTypeID, itemTypeName)
                times(9) = Now
                Call GetDependencies(itemTypeID, itemTypeName)
                times(10) = Now
                System.Threading.ThreadPool.QueueUserWorkItem(AddressOf GetEveCentralData, itemTypeID)
				lblDBLocation.Text = "Location: " & itemCatName & " --> " & itemGroupName
                times(11) = Now
                itemTime = times(11) - times(0)
				lblStatus.Text = "Last Item retrieved in " & itemTime.TotalSeconds & "s"
                btnRequisition.Enabled = itemPublished
                Dim TimeMsg As String = ""
                For t As Integer = 1 To 11
                    TimeMsg &= "Stage " & t.ToString & ": " & (times(t) - times(t - 1)).TotalMilliseconds.ToString & "ms" & ControlChars.CrLf
                Next
                TimeMsg &= ControlChars.CrLf & "Final: " & (times(11) - times(0)).TotalMilliseconds & "ms" & ControlChars.CrLf

                'MessageBox.Show(TimeMsg, "IB Timing", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim msg As String = "There is an inconsistency in the Eve static data." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "There are references to a typeID which doesn't appear to be listed in the invTypes table. The SQL used to get this data was:" & ControlChars.CrLf & ControlChars.CrLf
                msg &= strSQL
                MessageBox.Show(msg, "Inconsistent Eve Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
		End If
    End Sub
    Private Sub GetVariations(ByVal metaTypeID As Long, ByVal metaTypeName As String)
        Dim eveData As DataSet
        Dim strSQL As String = ""
        strSQL &= "SELECT invMetaTypes.typeID, invMetaTypes.parentTypeID"
        strSQL &= " FROM invMetaTypes"
        strSQL &= " WHERE (((invMetaTypes.typeID)=" & metaTypeID & ") OR ((invMetaTypes.parentTypeID)=" & metaTypeID & "));"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
		If eveData.Tables(0).Rows.Count = 0 Then
			tiVariations.Visible = False
		Else
			tiVariations.Visible = True
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
			itemVariations(0, 0) = EveHQ.Core.HQ.itemData(metaParentID).ID.ToString
			itemVariations(1, 0) = EveHQ.Core.HQ.itemData(metaParentID).Name.ToString
			itemVariations(2, 0) = "Tech I"

			lstVariations.BeginUpdate()
			lstVariations.Items.Clear()
			For item As Integer = 0 To metaItemCount
				Dim lstItem As New ListViewItem
				lstItem.Tag = itemVariations(0, item)
				lstItem.Text = itemVariations(1, item)
				lstItem.SubItems.Add(itemVariations(2, item))
				lstVariations.Items.Add(lstItem)
			Next
			lstVariations.EndUpdate()

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
        Dim EveData As DataSet
        ' Define IN string
        Dim strIn As New StringBuilder
        Dim item As String
        For Each item In compItems.Keys
            strIn.Append("," & item)
        Next
        If strIn.Length > 2 Then
            strIn = strIn.Remove(0, 1)
        End If

        ReDim CompMatrix(compItems.Count, compAtts.Count, 1)

        ' Get basic attributes
        Dim strSQL As String = "SELECT * from invTypes WHERE typeID IN (" & strIn.ToString & ");"
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
            If EveHQ.Core.HQ.MarketPriceList.ContainsKey(item) = True Then
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I2"), 0) = EveHQ.Core.HQ.MarketPriceList.Item(item)
            Else
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I2"), 0) = 0
            End If
            If EveHQ.Core.HQ.CustomPriceList.ContainsKey(item) = True Then
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I3"), 0) = EveHQ.Core.HQ.CustomPriceList.Item(item)
            Else
                CompMatrix(compItems.IndexOfKey(item), compAtts.IndexOfKey("I3"), 0) = 0
            End If
        Next

        ' Get all other attributes
        strSQL = "SELECT dgmTypeAttributes.typeID, dgmAttributeTypes.attributeGroup, eveUnits.unitID, eveUnits.displayName as unitDisplayName, eveUnits.unitName, dgmTypeAttributes.attributeID, dgmAttributeTypes.attributeID, dgmAttributeTypes.displayName as attributeDisplayName, dgmAttributeTypes.attributeName, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
        strSQL &= " FROM (eveUnits INNER JOIN dgmAttributeTypes ON eveUnits.unitID=dgmAttributeTypes.unitID) INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID=dgmTypeAttributes.attributeID"
        strSQL &= " WHERE typeID IN (" & strIn.ToString & ") ORDER BY dgmTypeAttributes.attributeID;"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            item = eveData.Tables(0).Rows(row).Item("typeID")
            Dim compAttID As String = ""
            Dim compValue As String = ""
            Dim compUnit As String = eveData.Tables(0).Rows(row).Item("unitDisplayName").ToString.Trim
            compAttID = eveData.Tables(0).Rows(row).Item("attributeID")
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
            lvwDepend.BeginUpdate()
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
			tiDependencies.Visible = True
            lvwDepend.EndUpdate()
        Else
            ' Remove the relevant tab
			tiDependencies.Visible = False
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
            Dim request As HttpWebRequest = CType(WebRequest.Create(RemoteURL), HttpWebRequest)
            ' Setup proxy server (if required)
            Call EveHQ.Core.ProxyServerFunctions.SetupWebProxy(request)
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

    Private Sub frmItemBrowser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler PlugInData.PluginDataReceived, AddressOf PluginDataHandler
    End Sub

    Private Sub frmItemBrowser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the startup flag
        startup = True

        ' Add an event handler for the changing data
        AddHandler PlugInData.PluginDataReceived, AddressOf PluginDataHandler

        For activity As Integer = 1 To ActivityCount
			tabPagesM(activity) = Me.tcMaterials.Tabs("tiM" & activity)
			tabPagesC(activity) = Me.tcComponents.Tabs("tiC" & activity)
        Next
        Call Me.LoadFittingAttributes()
        Me.tiEffects.Visible = False
		Me.tiSkills.Visible = False
		Me.tiFitting.Visible = False
		Me.tiMaterials.Visible = False
		Me.tiComponent.Visible = False
		Me.tiRecommended.Visible = False
		Me.tiVariations.Visible = False
		Me.tiDependencies.Visible = False
		Me.tiEveCentral.Visible = False
        Me.lblUsable.Text = ""
        Me.lblUsableTime.Text = ""

        ' Load the browser
        chkBrowseNonPublished.Checked = EveHQ.Core.HQ.EveHQSettings.IBShowAllItems
        If tvwBrowse.Nodes.Count = 0 Then Call Me.LoadBrowserGroups()

        ' Load the Pilots
        Call Me.UpdatePilots()

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

        ' Load the effect search combobox
        cboEffectSearch.BeginUpdate()
        cboEffectSearch.Items.Clear()
        cboEffectSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboEffectSearch.AutoCompleteSource = AutoCompleteSource.CustomSource
        For Each att As String In PlugInData.EffectList.GetKeyList
            cboEffectSearch.AutoCompleteCustomSource.Add(att)
            cboEffectSearch.Items.Add(att)
        Next
        cboEffectSearch.EndUpdate()

        ' Add the category groups into the listview
        lvwDepend.Groups.Clear()
        For Each cat As String In EveHQ.Core.HQ.itemCats.Keys
            lvwDepend.Groups.Add("Cat" & cat, EveHQ.Core.HQ.itemCats(cat))
        Next
        lvwDepend.Groups.Add("CatCerts", "Certificates")

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

    Private Sub GetRecommendations(ByVal typeID As Long, ByVal typeName As String)
        lvwRecommended.BeginUpdate()
        lvwRecommended.Items.Clear()
        ' Fetch the certs from the DB
        Dim strSQL As String = "SELECT * FROM crtRecommendations WHERE shipTypeID =" & itemTypeID
        Dim certData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If certData IsNot Nothing Then
            If certData.Tables(0).Rows.Count > 0 Then
                For Each certRow As DataRow In certData.Tables(0).Rows
                    Dim cert As String = certRow.Item("certificateID").ToString
                    Dim newCert As EveHQ.Core.Certificate = EveHQ.Core.HQ.Certificates(cert)
                    Dim certClass As EveHQ.Core.CertificateClass = EveHQ.Core.HQ.CertificateClasses(CStr(newCert.ClassID))
                    Dim newItem As New ListViewItem
                    newItem.Text = certClass.Name
                    newItem.ImageIndex = newCert.Grade
                    Dim certGrade As String = CertGrades(newCert.Grade)
                    newItem.SubItems.Add(certGrade)
                    lvwRecommended.Items.Add(newItem)
				Next
				tiRecommended.Visible = True
			Else
				tiRecommended.Visible = False
			End If
        End If
        lvwRecommended.EndUpdate()
    End Sub
    Private Sub GetAttributes(ByVal typeID As Long, ByVal typeName As String)

        Dim EveData As DataSet

        Dim attributes(150, 5) As String
        Dim attNo As Integer = 0
        ' Set "unused" flag
        For a As Integer = 0 To 150 : attributes(a, 0) = "0" : Next

        Dim bpTypeID As String = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)
        ' Show additional information re blueprint or product
        If BasicItem.Category = 9 Then
            Dim typeID2 As String = EveHQ.Core.DataFunctions.GetTypeID(bpTypeID)
            If bpTypeID <> typeID2 Then
                picBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(typeID2)
                picBP.Tag = typeID2
                ItemToolTip1.SetToolTip(Me.picBP, "Show Product Type")
                picBP.Visible = True
            Else
                picBP.Visible = False
            End If
        Else
            If bpTypeID <> typeID Then
                picBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(bpTypeID.ToString)
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

        ' Load picture
        picItem.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(typeID.ToString)

        ' Insert attribute 1 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "A"
        attributes(attNo, 2) = "Group ID"
        attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("groupID")
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 2 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "B"
        attributes(attNo, 2) = "Description"
        Dim desc As String = BasicItemData.Tables(0).Rows(0).Item("description").ToString
        desc = System.Text.RegularExpressions.Regex.Replace(desc, "[\x00-\x1f]", "")
        attributes(attNo, 3) = desc
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 3 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "C"
        attributes(attNo, 2) = "Radius"
        attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("radius")
        attributes(attNo, 4) = " m"
        attributes(attNo, 5) = "1"
        ' Insert attribute 4 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "D"
        attributes(attNo, 2) = "Mass"
        attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("mass")
        attributes(attNo, 4) = " kg"
        attributes(attNo, 5) = "1"
        ' Insert attribute 5 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "E"
        attributes(attNo, 2) = "Volume"
        attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("volume")
        attributes(attNo, 4) = " m3"
        attributes(attNo, 5) = "1"
        ' Insert attribute 6 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "F"
        attributes(attNo, 2) = "Cargo Capacity"
        attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("capacity")
        attributes(attNo, 4) = " m3"
        attributes(attNo, 5) = "1"
        ' Insert attribute 7 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "G"
        attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("portionSize")
        attributes(attNo, 2) = "Portion Size"
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 8 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "H"
        attributes(attNo, 2) = "Race ID"
        If IsDBNull(BasicItemData.Tables(0).Rows(0).Item("raceID")) = False Then
            attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("raceID")
        Else
            attributes(attNo, 3) = "0"
        End If
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert attribute 9 from tblTypes
        attNo += 1
        attributes(attNo, 1) = "I1"
        attributes(attNo, 2) = "Base Price"
        attributes(attNo, 3) = Math.Round(BasicItemData.Tables(0).Rows(0).Item("basePrice"), 2, MidpointRounding.AwayFromZero)
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"
        ' Insert Market Price
        attNo += 1
        attributes(attNo, 1) = "I2"
        attributes(attNo, 2) = "Market Price"
        If EveHQ.Core.HQ.MarketPriceList.ContainsKey(CStr(itemTypeID)) = True Then
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
        If EveHQ.Core.HQ.CustomPriceList.ContainsKey(CStr(itemTypeID)) = True Then
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
        If BasicItemData.Tables(0).Rows(0).Item("published") = "0" Then
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
        If IsDBNull(BasicItemData.Tables(0).Rows(0).Item("marketGroupID")) = False Then
            attributes(attNo, 3) = BasicItemData.Tables(0).Rows(0).Item("marketGroupID")
        Else
            attributes(attNo, 3) = "n/a"
        End If
        attributes(attNo, 4) = ""
        attributes(attNo, 5) = "0"

        Dim strSQL As String = ""
        If BasicItem.Category = 9 Then            ' If in the blueprint category
            strSQL = "SELECT * FROM invBlueprintTypes WHERE blueprintTypeID=" & typeID & ";"
            EveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If EveData.Tables(0).Rows.Count > 0 Then
                For col As Integer = 3 To EveData.Tables(0).Columns.Count - 1
                    attNo += 1
                    attributes(attNo, 1) = "Z" & col - 2
                    attributes(attNo, 2) = EveData.Tables(0).Columns(col).Caption
                    attributes(attNo, 3) = Math.Round(EveData.Tables(0).Rows(0).Item(col))
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
            EveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            For row As Integer = 0 To EveData.Tables(0).Rows.Count - 1
                attNo += 1
                attributes(attNo, 1) = EveData.Tables(0).Rows(row).Item("attributeID")
                If EveData.Tables(0).Rows(row).Item("attributeDisplayName").ToString.Trim = "" Then
                    attributes(attNo, 2) = EveData.Tables(0).Rows(row).Item("attributeName").ToString.Trim
                Else
                    attributes(attNo, 2) = EveData.Tables(0).Rows(row).Item("attributeDisplayName").ToString.Trim
                End If
                If IsDBNull(EveData.Tables(0).Rows(row).Item("valueFloat")) = False Then
                    attributes(attNo, 3) = EveData.Tables(0).Rows(row).Item("valueFloat")
                Else
                    attributes(attNo, 3) = EveData.Tables(0).Rows(row).Item("valueInt")
                End If
                attributes(attNo, 4) = " " & EveData.Tables(0).Rows(row).Item("unitDisplayName").ToString.Trim
                attributes(attNo, 5) = EveData.Tables(0).Rows(row).Item("attributeGroup").ToString.Trim
                ' Do modifier calculations here!
                Select Case EveData.Tables(0).Rows(row).Item("unitID")
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
        Dim SourceItem As New EveHQ.Core.EveItem
        itemFitting.Clear()
        For att As Integer = 1 To attNo
            If attributes(att, 4) = " typeID" Then
                attributes(att, 4) &= attributes(att, 3)
                SourceItem = EveHQ.Core.HQ.itemData(attributes(att, 3))

                Select Case attributes(att, 1)
                    Case "182"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 277 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        If itemSkills.ContainsKey(attributes(att, 3)) = False Then
                            itemSkills.Add(attributes(att, 3), skillLvl)
                        End If
                        attributes(att, 3) = SourceItem.Name
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "183"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 278 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        If itemSkills.ContainsKey(attributes(att, 3)) = False Then
                            itemSkills.Add(attributes(att, 3), skillLvl)
                        End If
                        attributes(att, 3) = SourceItem.Name
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "184"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 279 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        If itemSkills.ContainsKey(attributes(att, 3)) = False Then
                            itemSkills.Add(attributes(att, 3), skillLvl)
                        End If
                        attributes(att, 3) = SourceItem.Name
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "1285"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 1286 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        If itemSkills.ContainsKey(attributes(att, 3)) = False Then
                            itemSkills.Add(attributes(att, 3), skillLvl)
                        End If
                        attributes(att, 3) = SourceItem.Name
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "1289"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 1287 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        If itemSkills.ContainsKey(attributes(att, 3)) = False Then
                            itemSkills.Add(attributes(att, 3), skillLvl)
                        End If
                        attributes(att, 3) = SourceItem.Name
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case "1290"
                        For att2 As Integer = att To attNo
                            If Val(attributes(att2, 1)) = 1288 Then
                                skillLvl = attributes(att2, 3)
                                attributes(att2, 1) = "0"
                                Exit For
                            End If
                        Next
                        If itemSkills.ContainsKey(attributes(att, 3)) = False Then
                            itemSkills.Add(attributes(att, 3), skillLvl)
                        End If
                        attributes(att, 3) = SourceItem.Name
                        If skillLvl <> "" Then attributes(att, 3) &= " (Level " & skillLvl & ")"
                    Case Else
                        attributes(att, 3) = SourceItem.Name
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

        lstAttributes.BeginUpdate()
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
        lstAttributes.EndUpdate()

    End Sub

    Private Sub GetEffects(ByVal typeID As Long, ByVal typeName As String)
        Dim strSQL As String = "SELECT dgmTypeEffects.typeID, dgmEffects.effectName, dgmEffects.displayName, dgmEffects.isOffensive, dgmEffects.isAssistance, dgmEffects.description"
        strSQL &= " FROM dgmEffects"
        strSQL &= " INNER JOIN dgmTypeEffects ON dgmEffects.effectID = dgmTypeEffects.effectID"
        strSQL &= " WHERE typeID=" & typeID
        Dim EveData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If EveData.Tables(0).Rows.Count > 0 Then
            lstEffects.BeginUpdate()
            lstEffects.Items.Clear()
            For Each EffectRow As DataRow In EveData.Tables(0).Rows
                Dim EffectItem As New ListViewItem
                EffectItem.Text = EffectRow.Item("effectName")
                EffectItem.SubItems.Add(EffectRow.Item("description"))
                lstEffects.Items.Add(EffectItem)
            Next
            lstEffects.EndUpdate()
            tiEffects.Visible = True
        Else
            tiEffects.Visible = False
        End If
    End Sub

    Private Sub GetMaterials(ByVal typeID As Long, ByVal typeName As String)
        Dim EveData As DataSet
        Dim bpTypeID As Long = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)

        ' Select only the building activity (at the minute!)
        Dim strSQL As String = "SELECT * FROM invBuildMaterials WHERE (invBuildMaterials.typeID=" & bpTypeID & " OR invBuildMaterials.typeID=" & typeID & ");"
        EveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If EveData.Tables(0).Rows.Count > 0 Then

            ' Work out what activities we have in the list
            Dim activities(ActivityCount) As Boolean
            Dim strActivity As String = ""
            For row As Integer = 0 To EveData.Tables(0).Rows.Count - 1
                activities(Val(EveData.Tables(0).Rows(row).Item("activityID"))) = True
            Next
            ' Then create sub tabs :)
            For activity As Integer = 1 To ActivityCount
                If activities(activity) = True Then
                    tabPagesM(activity).Visible = True
                Else
                    tabPagesM(activity).Visible = False
                End If
            Next

            Dim materials(EveData.Tables(0).Rows.Count, 9)
            With EveData.Tables(0)
                For row As Integer = 0 To .Rows.Count - 1
                    If Val(.Rows(row).Item("quantity")) > 0 Then
                        materials(row, 0) = "0"
                        materials(row, 1) = .Rows(row).Item("requiredTypeID").ToString.Trim
                        materials(row, 2) = EveHQ.Core.HQ.itemData(materials(row, 1)).Name
                        materials(row, 3) = .Rows(row).Item("quantity").ToString.Trim
                        materials(row, 4) = .Rows(row).Item("damagePerJob").ToString.Trim
                        materials(row, 5) = EveHQ.Core.HQ.itemData(materials(row, 1)).Category.ToString
                        materials(row, 6) = EveHQ.Core.HQ.itemCats(materials(row, 5))
                        materials(row, 7) = EveHQ.Core.HQ.itemData(materials(row, 1)).Group.ToString
                        materials(row, 8) = EveHQ.Core.HQ.itemGroups(materials(row, 7))
                        materials(row, 9) = .Rows(row).Item("activityID").ToString.Trim
                    End If
                Next
            End With

            Dim itemcount As Integer = EveData.Tables(0).Rows.Count - 1
            Dim matCatID, matCatName, matGroupID, matGroupName As String
            For act As Integer = 1 To ActivityCount
                Dim materialsView As New DevComponents.DotNetBar.Controls.ListViewEx
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
                    Case 9
                        materialsView = Me.lstM9
                End Select
                materialsView.BeginUpdate()
                materialsView.Items.Clear()
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
            tiMaterials.Visible = True
        Else
            ' Remove the relevant tab
            tiMaterials.Visible = False
        End If

        Call Me.GetRecyclingData(typeID.ToString)

    End Sub

    Private Sub GetRecyclingData(ByVal typeID As String)
        Dim EveData As DataSet
        ' Fetch the recycling information
        Dim strSQL As String = "SELECT * FROM invTypeMaterials  WHERE (invTypeMaterials.typeID=" & typeID & ");"
        EveData = EveHQ.Core.DataFunctions.GetData(strSQL)

        If EveData.Tables(0).Rows.Count > 0 Then
            tabPagesM(9).Visible = True
            Dim materials(EveData.Tables(0).Rows.Count, 9)
            With EveData.Tables(0)
                For row As Integer = 0 To .Rows.Count - 1
                    If Val(.Rows(row).Item("quantity")) > 0 Then
                        materials(row, 0) = "0"
                        materials(row, 1) = .Rows(row).Item("materialTypeID").ToString.Trim
                        materials(row, 2) = EveHQ.Core.HQ.itemData(materials(row, 1)).Name
                        materials(row, 3) = .Rows(row).Item("quantity").ToString.Trim
                        materials(row, 4) = ""
                        materials(row, 5) = EveHQ.Core.HQ.itemData(materials(row, 1)).Category.ToString
                        materials(row, 6) = EveHQ.Core.HQ.itemCats(materials(row, 5))
                        materials(row, 7) = EveHQ.Core.HQ.itemData(materials(row, 1)).Group.ToString
                        materials(row, 8) = EveHQ.Core.HQ.itemGroups(materials(row, 7))
                        materials(row, 9) = ""
                    End If
                Next
            End With

            Dim itemcount As Integer = EveData.Tables(0).Rows.Count - 1
            Dim matCatID, matCatName, matGroupID, matGroupName As String
            Dim materialsView As ListView = Me.lstM9
            materialsView.BeginUpdate()
            materialsView.Items.Clear()
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
        Else
            tabPagesM(9).Visible = False
        End If
    End Sub

    Private Sub GetComponents(ByVal typeID As Long, ByVal typeName As String)

        Dim EveData As DataSet
        Dim bpTypeID As Long = EveHQ.Core.DataFunctions.GetBPTypeID(typeID)

        ' Select only the building activity (at the minute!)
        Dim strSQL As String = "SELECT * FROM ramTypeRequirements WHERE (ramTypeRequirements.requiredTypeID=" & bpTypeID & " OR ramTypeRequirements.requiredTypeID=" & typeID & ");"
        EveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If EveData.Tables(0).Rows.Count > 0 Then

            ' Work out what activities we have in the list
            Dim activities(ActivityCount) As Boolean
            Dim strActivity As String = ""
            For row As Integer = 0 To EveData.Tables(0).Rows.Count - 1
                activities(CInt(EveData.Tables(0).Rows(row).Item("activityID"))) = True
            Next
            ' Then create sub tabs :)
            For activity As Integer = 1 To ActivityCount
                If activities(activity) = True Then
                    tabPagesC(activity).Visible = True
                Else
                    tabPagesC(activity).Visible = False
                End If
            Next

            Dim materials(EveData.Tables(0).Rows.Count, 9)
            With EveData.Tables(0)
                For row As Integer = 0 To .Rows.Count - 1
                    If CInt(.Rows(row).Item("quantity")) > 0 Then
                        materials(row, 0) = "0"
                        materials(row, 1) = .Rows(row).Item("typeID").ToString.Trim
                        materials(row, 2) = EveHQ.Core.HQ.itemData(materials(row, 1)).Name
                        materials(row, 3) = .Rows(row).Item("quantity").ToString.Trim
                        materials(row, 4) = .Rows(row).Item("damagePerJob").ToString.Trim
                        materials(row, 5) = EveHQ.Core.HQ.itemData(materials(row, 1)).Category.ToString
                        materials(row, 6) = EveHQ.Core.HQ.itemCats(materials(row, 5))
                        materials(row, 7) = EveHQ.Core.HQ.itemData(materials(row, 1)).Group.ToString
                        materials(row, 8) = EveHQ.Core.HQ.itemGroups(materials(row, 7))
                        materials(row, 9) = .Rows(row).Item("activityID").ToString.Trim
                    End If
                Next
            End With

            Dim itemcount As Integer = EveData.Tables(0).Rows.Count - 1
            Dim matCatID, matCatName, matGroupID, matGroupName As String
            For act As Integer = 1 To ActivityCount
                Dim materialsView As New DevComponents.DotNetBar.Controls.ListViewEx
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
                materialsView.BeginUpdate()
                materialsView.Items.Clear()
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
            tiComponent.Visible = True
        Else
            ' Remove the relevant tab
            tiComponent.Visible = False
        End If

    End Sub

    Public Sub GenerateSkills(ByVal typeID As Long, ByVal typeName As String)

        Dim ItemUsable As Boolean = True
        skillsNeeded.Clear()

        tvwReqs.BeginUpdate()
        tvwReqs.Nodes.Clear()
        Dim skillsRequired As Boolean = False

        If displayPilot IsNot Nothing Then
            For Each skillID As String In itemSkills.Keys
                If skillID > 0 Then
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
                End If
            Next

            If skillsRequired = True Then
                tvwReqs.ExpandAll()
                tiSkills.Visible = True
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
                tiSkills.Visible = False
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
        tvwReqs.EndUpdate()

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

    Private Sub GenerateFitting()
        If itemFitting.Count <> 0 Then
            lstFitting.BeginUpdate()
            lstFitting.Items.Clear()
            For Each attributes() As String In itemFitting
                Dim lstItem As New ListViewItem
                lstItem.Name = attributes(1)
                lstItem.Text = "(" & attributes(1) & ")  " & attributes(2)
                lstItem.SubItems.Add(attributes(3) & attributes(4))
                attributes(0) = "1"
                lstFitting.Items.Add(lstItem)
            Next
            tiFitting.Visible = True
            lstFitting.EndUpdate()
        Else
            tiFitting.Visible = False
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
        Dim selQ As New EveHQ.Core.frmSelectQueue(displayPilot.Name, skillsNeeded)
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
    Private Sub lstM9_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstM9.DoubleClick
        Dim id As String = lstM9.SelectedItems(0).Name
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

    Private Sub lblID_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblID.DoubleClick
        Dim goItemID As String
        goItemID = InputBox(ControlChars.CrLf & "Please enter the itemID to jump to...", "Jump to ItemID")
        If EveHQ.Core.HQ.itemList.ContainsValue(goItemID) = True Then
            Call Me.LoadItemID(goItemID)
            ' Alter navigation
            Call Me.AddToNavigation(itemTypeName)
        End If
    End Sub

    Private Sub lblDBLocation_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblDBLocation.DoubleClick
        If itemGroupID <> "" Then
            Me.tvwBrowse.Select()
            Me.tvwBrowse.CollapseAll()
            Dim selnode As TreeNode = tvwBrowse.Nodes(itemCatID).Nodes(itemGroupID)
            Me.tvwBrowse.SelectedNode = selnode
            Me.tvwBrowse.SelectedNode.Expand()
            Me.tabBrowser.SelectedTab = Me.tabBrowse
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
            ' Clear handlers
            For Each mnuItem As DevComponents.DotNetBar.ButtonItem In btnNavBack.SubItems
                RemoveHandler mnuItem.Click, AddressOf Me.NavMenuClickHandler
            Next
            btnNavBack.SubItems.Clear()
            For menu As Integer = curNavigation - 1 To Math.Max(1, curNavigation - 10) Step -1
                Dim mnuItem As New DevComponents.DotNetBar.ButtonItem
                mnuItem.Name = menu
                mnuItem.Text = navigated.Item("n" & menu)
                btnNavBack.SubItems.Add(mnuItem)
                AddHandler mnuItem.Click, AddressOf Me.NavMenuClickHandler
            Next
            btnNavBack.Enabled = True
        Else
            btnNavBack.Enabled = False
            btnNavForward.Enabled = False
        End If
    End Sub
    Private Sub DrawForwardMenu()
        If navigated.Count > 1 Then
            ' Clear handlers
            For Each mnuItem As DevComponents.DotNetBar.ButtonItem In btnNavForward.SubItems
                RemoveHandler mnuItem.Click, AddressOf Me.NavMenuClickHandler
            Next
            btnNavForward.SubItems.Clear()
            If curNavigation <> navigated.Count Then
                For menu As Integer = curNavigation + 1 To Math.Min(navigated.Count, curNavigation + 10) Step 1
                    Dim mnuItem As New DevComponents.DotNetBar.ButtonItem
                    mnuItem.Name = menu
                    mnuItem.Text = navigated.Item("n" & menu)
                    btnNavForward.SubItems.Add(mnuItem)
                    AddHandler mnuItem.Click, AddressOf Me.NavMenuClickHandler
                Next
                btnNavForward.Enabled = True
            End If
        Else
            btnNavBack.Enabled = False
            btnNavForward.Enabled = False
        End If
    End Sub
    Private Sub DeleteNextItems()
        If navigated.Count > 1 Then
            For menu As Integer = curNavigation + 1 To navigated.Count Step 1
                navigated.Remove("n" & menu)
            Next
            btnNavForward.Enabled = False
        Else
            btnNavBack.Enabled = False
            btnNavForward.Enabled = False
        End If
    End Sub
    Private Sub btnNavBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNavBack.Click
        curNavigation -= 1
        Call Me.GoNavigate()
    End Sub
    Private Sub btnNavForward_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNavForward.Click
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
            btnNavBack.Enabled = False
        Else
            btnNavBack.Enabled = True
        End If
        If curNavigation >= navigated.Count Then
            btnNavForward.Enabled = False
        Else
            btnNavForward.Enabled = True
        End If
    End Sub
    Private Sub NavMenuClickHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim clickedItem As DevComponents.DotNetBar.ButtonItem = CType(sender, DevComponents.DotNetBar.ButtonItem)
        curNavigation = CInt(clickedItem.Name)
        Call Me.GoNavigate()
    End Sub

#End Region

#Region "Attribute Search"

    Private Sub cboAttSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAttSearch.SelectedIndexChanged
        ' Fetch attributeID
        Dim attID As String = PlugInData.AttributeList.Item(cboAttSearch.SelectedItem)
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM dgmTypeAttributes WHERE attributeID=" & attID & ";")
        Dim itemID As String = ""
        Dim itemName As String = ""
        Dim itemValue As Double = 0
        lstAttSearch.BeginUpdate()
        lstAttSearch.Items.Clear()
        For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            itemID = eveData.Tables(0).Rows(item).Item("typeID")
            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
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
            End If
        Next
        lstAttSearch.EndUpdate()
        lstAttSearch.Sorting = SortOrder.Ascending
        lstAttSearch.Sort()
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

#End Region

#Region "Effect Search"

    Private Sub cboEffectSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEffectSearch.SelectedIndexChanged
        ' Fetch attributeID
        Dim effectID As String = PlugInData.EffectList.Item(cboEffectSearch.SelectedItem)
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM dgmTypeEffects WHERE effectID=" & effectID & ";")
        Dim itemID As String = ""
        Dim itemName As String = ""
        lstEffectSearch.BeginUpdate()
        lstEffectSearch.Items.Clear()
        For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            itemID = eveData.Tables(0).Rows(item).Item("typeID")
            If EveHQ.Core.HQ.itemData.ContainsKey(itemID) = True Then
                itemName = EveHQ.Core.HQ.itemData(itemID).Name
                Dim lstItem As New ListViewItem
                lstItem.Text = itemName
                lstItem.Name = itemID
                lstItem.ToolTipText = itemName
                lstEffectSearch.Items.Add(lstItem)
            End If
        Next
        lstEffectSearch.EndUpdate()
        lstEffectSearch.Sorting = SortOrder.Ascending
        lstEffectSearch.Sort()
        lblEffectSearchCount.Text = lstEffectSearch.Items.Count & " items found"
    End Sub

    Private Sub lstEffectSearch_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lstEffectSearch.ColumnClick
        If lstEffectSearch.Tag = e.Column Then
            Me.lstEffectSearch.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lstEffectSearch.Tag = -1
        Else
            Me.lstEffectSearch.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lstEffectSearch.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lstEffectSearch.Sort()
    End Sub

    Private Sub lstEffectSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstEffectSearch.SelectedIndexChanged
        If lstEffectSearch.SelectedItems.Count > 0 Then
            Dim selItem As String = lstEffectSearch.SelectedItems(0).Text
            If selItem <> "" Then
                Call LoadItemID(EveHQ.Core.HQ.itemList(selItem))
                ' Alter navigation
                Call Me.AddToNavigation(itemTypeName)
            End If
        End If
    End Sub

#End Region
   

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
            tiEveCentral.Visible = True
        Else
            tiEveCentral.Visible = False
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

    Private Sub btnRequisition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRequisition.Click
        Dim Orders As New SortedList(Of String, Integer)
        Orders.Add(itemTypeName, 1)
        Dim newReq As New EveHQ.Core.frmAddRequisition("Item Browser", Orders)
        newReq.ShowDialog()
    End Sub

End Class