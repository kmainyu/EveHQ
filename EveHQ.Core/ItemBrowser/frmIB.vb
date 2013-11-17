' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports System.Drawing
Imports EveHQ.EveData
Imports System.Windows.Forms
Imports System.Text
Imports System.Text.RegularExpressions
Imports EveHQ.Core.Requisitions
Imports EveHQ.Common.Extensions
Imports MarkupLinkClickEventArgs = DevComponents.DotNetBar.MarkupLinkClickEventArgs

Namespace ItemBrowser

    Public Class FrmIB

#Region "Class Variables"

        Dim _item As New EveType
        Dim _attributeList As New SortedList(Of Integer, ItemAttribData)
        Dim _fittingIDList As New List(Of Integer)
        Dim _itemSkills As Dictionary(Of Integer, Integer)
        ReadOnly _navBack As New Stack(Of Integer)
        ReadOnly _navForward As New Stack(Of Integer)
        ReadOnly _recentItems As New Stack(Of Integer)

#End Region

#Region "Constructors"

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

        End Sub

        Public Sub New(typeID As Integer)

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            DisplayItem(typeID, True)

        End Sub

#End Region

#Region "Form Loading and Closing Routines"

        Private Sub frmItemBrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load

            ' Load the attribute search combobox
            Dim atts As IEnumerable(Of String) = From ta In StaticData.TypeAttributes Join at In StaticData.AttributeTypes.Values On ta.AttributeId Equals at.AttributeId
                                          Select at.AttributeName
                                          Distinct
            cboAttSearch.BeginUpdate()
            cboAttSearch.Items.Clear()
            cboAttSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cboAttSearch.AutoCompleteSource = AutoCompleteSource.CustomSource
            For Each attName As String In atts
                cboAttSearch.AutoCompleteCustomSource.Add(attName)
                cboAttSearch.Items.Add(attName)
            Next
            cboAttSearch.Sorted = True
            cboAttSearch.EndUpdate()

            ' Load the effect search combobox
            Dim effects As IEnumerable(Of String) = From te In StaticData.TypeEffects Join et In StaticData.EffectTypes.Values On te.EffectId Equals et.EffectId
                                          Select et.EffectName
                                          Distinct
            cboEffectSearch.BeginUpdate()
            cboEffectSearch.Items.Clear()
            cboEffectSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cboEffectSearch.AutoCompleteSource = AutoCompleteSource.CustomSource
            For Each effectName As String In effects
                cboEffectSearch.AutoCompleteCustomSource.Add(effectName)
                cboEffectSearch.Items.Add(effectName)
            Next
            cboEffectSearch.Sorted = True
            cboEffectSearch.EndUpdate()

            ' Load the IDs
            ' Set the fitting attributeID list
            Dim ids() As Integer = {11, 12, 13, 14, 30, 48, 50, 101, 102, 1132, 1137, 1153}
            _fittingIDList = New List(Of Integer)(ids)

            ' Load the categories & groups
            LoadBrowserTree()
            LoadCategories()
            LoadGroups()

            ' Update the list of pilots
            UpdatePilots()

        End Sub

        Private Sub LoadBrowserTree()
            adtBrowse.BeginUpdate()
            adtBrowse.Nodes.Clear()
            ' Load up the Browser with categories
            For Each catID As Integer In StaticData.TypeCats.Keys
                If StaticData.TypeCats(catID) <> "" Then
                    Dim catNode As New Node
                    catNode.Name = "c" & catID
                    catNode.Text = StaticData.TypeCats(catID)
                    adtBrowse.Nodes.Add(catNode)
                End If
            Next
            ' Load up the Browser with groups
            For Each groupID As Integer In StaticData.TypeGroups.Keys
                Dim groupNode As New Node
                groupNode.Name = "g" & groupID
                groupNode.Text = StaticData.TypeGroups(groupID)
                groupNode.Nodes.Add(New Node("Loading..."))
                adtBrowse.FindNodeByName("c" & StaticData.GroupCats(groupID)).Nodes.Add(groupNode)
            Next
            AdvTreeSorter.Sort(adtBrowse, 1, True, False)
            adtBrowse.EndUpdate()
        End Sub

        Private Sub LoadCategories()
            adtCategories.BeginUpdate()
            adtCategories.Nodes.Clear()
            ' Load up the category tab with categories
            For Each catID As Integer In StaticData.TypeCats.Keys
                If StaticData.TypeCats(catID) <> "" Then
                    Dim catNode As New Node
                    catNode.Name = "c" & catID
                    catNode.Text = catID.ToString
                    catNode.Cells.Add(New Cell(StaticData.TypeCats(catID)))
                    adtCategories.Nodes.Add(catNode)
                End If
            Next
            adtCategories.EndUpdate()
        End Sub

        Private Sub LoadGroups()
            adtGroups.BeginUpdate()
            adtGroups.Nodes.Clear()
            ' Load up the Browser with groups
            For Each groupID As Integer In StaticData.TypeGroups.Keys
                Dim groupNode As New Node
                groupNode.Name = "g" & groupID
                groupNode.Text = groupID.ToString
                groupNode.Cells.Add(New Cell(StaticData.TypeGroups(groupID)))
                adtGroups.Nodes.Add(groupNode)
            Next
            adtGroups.EndUpdate()
        End Sub

        Public Sub UpdatePilots()
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                If cPilot.Active = True Then
                    cboPilots.Items.Add(cPilot.Name)
                End If
            Next
            cboPilots.EndUpdate()

            If cboPilots.Items.Count > 0 Then
                If cboPilots.Items.Contains(HQ.Settings.StartupPilot) = True Then
                    cboPilots.SelectedItem = HQ.Settings.StartupPilot
                Else
                    cboPilots.SelectedIndex = 0
                End If
            End If
        End Sub

#End Region

#Region "Item Search & UI Routines"

        Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
            If Len(txtSearch.Text) > 2 Then
                Dim strSearch As String = txtSearch.Text.Trim.ToLower
                Dim results As New SortedList(Of String, Integer)
                For Each item As String In StaticData.TypeNames.Keys
                    If item.ToLower.Contains(strSearch) Then
                        results.Add(item, StaticData.TypeNames(item))
                    End If
                Next
                adtSearch.BeginUpdate()
                adtSearch.Nodes.Clear()
                For Each item As String In results.Keys
                    Dim newNode As New Node
                    newNode.Text = item
                    newNode.Name = CStr(results(item))
                    adtSearch.Nodes.Add(newNode)
                Next
                adtSearch.EndUpdate()
                lblSearchCount.Text = adtSearch.Nodes.Count & " items found"
            End If
        End Sub

        Private Sub adtSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles adtSearch.SelectedIndexChanged
            If adtSearch.SelectedNodes.Count > 0 Then
                Dim itemID As Integer = CInt(adtSearch.SelectedNodes(0).Name)
                If itemID > 0 Then
                    Call DisplayItem(itemID, True)
                End If
            End If
        End Sub

#End Region

#Region "Item Browse Routines"

        Private Sub adtBrowse_BeforeExpand(sender As Object, e As AdvTreeNodeCancelEventArgs) Handles adtBrowse.BeforeExpand
            Call LoadBrowserItems(e.Node)
        End Sub

        Private Sub LoadBrowserItems(e As Node)

            Select Case e.Level
                Case 1
                    If e.Tag Is Nothing Then
                        adtBrowse.BeginUpdate()
                        e.Nodes.Clear()
                        ' Load the Browser with items
                        Dim groupID As Integer = CInt(e.Name.TrimStart("g".ToCharArray))
                        Dim itemNode As Node
                        For Each item As EveType In StaticData.Types.Values
                            If item.Group = groupID Then
                                itemNode = New Node
                                itemNode.Text = item.Name ' Name
                                itemNode.Name = item.Id.ToString ' ID
                                e.Nodes.Add(itemNode)
                            End If
                        Next
                        adtBrowse.EndUpdate()
                        e.Tag = True
                        e.Nodes.Sort()
                    End If
                Case 2
                    Call DisplayItem(CInt(e.Name), True)
            End Select

        End Sub

        Private Sub adtBrowse_SelectionChanged(sender As Object, e As EventArgs) Handles adtBrowse.SelectionChanged
            Dim selNode As Node = adtBrowse.SelectedNode
            Select Case selNode.Level
                Case 2
                    Call DisplayItem(CInt(selNode.Name), True)
            End Select
        End Sub

#End Region

#Region "Attribute Search & UI Routines"

        Private Sub cboAttSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAttSearch.SelectedIndexChanged
            ' Fetch attribute ID
            Dim attResults As IEnumerable(Of AttributeType) = From item In StaticData.AttributeTypes.Values Where item.AttributeName = cboAttSearch.SelectedItem.ToString
            Dim attID As Integer = attResults(0).AttributeId

            ' Fetch types based on attribute ID
            Dim types As IEnumerable(Of TypeAttrib) = From item In StaticData.TypeAttributes Where item.AttributeId = attID
            adtAttSearch.BeginUpdate()
            adtAttSearch.Nodes.Clear()
            For Each results As TypeAttrib In types
                If StaticData.Types.ContainsKey(results.TypeId) = True Then
                    Dim newNode As New Node
                    newNode.Text = StaticData.Types(results.TypeId).Name
                    newNode.Name = CStr(results.TypeId)
                    newNode.Cells.Add(New Cell(results.Value.ToString))
                    adtAttSearch.Nodes.Add(newNode)
                End If
            Next
            adtAttSearch.EndUpdate()
            AdvTreeSorter.Sort(adtAttSearch, 1, False, True)
            lblAttSearchCount.Text = adtAttSearch.Nodes.Count & " items found"
        End Sub

        Private Sub adtAttSearch_ColumnHeaderMouseDown(sender As Object, e As MouseEventArgs) Handles adtAttSearch.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

        Private Sub adtAttSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles adtAttSearch.SelectedIndexChanged
            If adtAttSearch.SelectedNodes.Count > 0 Then
                Dim itemID As Integer = CInt(adtAttSearch.SelectedNodes(0).Name)
                If itemID > 0 Then
                    Call DisplayItem(itemID, True)
                End If
            End If
        End Sub

#End Region

#Region "Effect Search & UI Routines"

        Private Sub cboEffectSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEffectSearch.SelectedIndexChanged

            ' Fetch effect ID
            Dim effectResults As IEnumerable(Of EffectType) = From item In StaticData.EffectTypes.Values Where item.EffectName = cboEffectSearch.SelectedItem.ToString
            Dim effectID As Integer = effectResults(0).EffectId

            ' Fetch types based on attribute ID
            Dim types As IEnumerable(Of TypeEffect) = From item In StaticData.TypeEffects Where item.EffectId = effectID
            adtEffectSearch.BeginUpdate()
            adtEffectSearch.Nodes.Clear()
            For Each results As TypeEffect In types
                If StaticData.Types.ContainsKey(results.TypeId) = True Then
                    Dim newNode As New Node
                    newNode.Text = StaticData.Types(results.TypeId).Name
                    newNode.Name = CStr(results.TypeId)
                    adtEffectSearch.Nodes.Add(newNode)
                End If
            Next
            adtEffectSearch.EndUpdate()
            AdvTreeSorter.Sort(adtEffectSearch, 1, False, True)
            lblEffectSearchCount.Text = adtEffectSearch.Nodes.Count & " items found"

        End Sub

        Private Sub adtEffectSearch_ColumnHeaderMouseDown(sender As Object, e As MouseEventArgs) Handles adtEffectSearch.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, False, False)
        End Sub

        Private Sub adtEffectSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles adtEffectSearch.SelectedIndexChanged
            If adtEffectSearch.SelectedNodes.Count > 0 Then
                Dim itemID As Integer = CInt(adtEffectSearch.SelectedNodes(0).Name)
                If itemID > 0 Then
                    Call DisplayItem(itemID, True)
                End If
            End If
        End Sub

#End Region

#Region "Item Display Routines"

        Private Sub DisplayItem(typeID As Integer, addToStack As Boolean)

            If StaticData.Types.ContainsKey(typeID) Then

                ' Push the last item onto the nav stack and recent items stack
                If _item.Id <> 0 Then
                    If addToStack = True Then
                        AddToNavigation()
                    End If
                    If _recentItems.Contains(_item.Id) = False Then
                        _recentItems.Push(_item.Id)
                    End If
                End If
                ' Update the navigation options
                DrawNavOptions()

                ' Start timing routine
                Dim startTime, endTime As DateTime
                startTime = Now

                ' Load picture
                picItem.ImageLocation = ImageHandler.GetImageLocation(typeID)

                ' Get the Item from the collection
                _item = StaticData.Types((typeID))

                ' Get the item name and set the window text
                lblItemName.Text = _item.Name
                Text = "EveHQ Item Browser - " & _item.Name

                ' Display the description
                DisplayDescription(_item.Description)

                ' Display Attributes, depending on whether this is a blueprint or not
                DisplayAttributes(_item)

                ' Display Fitting attributes
                DisplayFitting()

                ' Display Effects
                DisplayEffects(typeID)

                ' Display Skills
                DisplaySkills(typeID)

                ' Display Certificates
                DisplayCertificates(typeID)

                ' Display Variations
                DisplayVariations(typeID)

                ' Display Materials
                DisplayMaterials(typeID)

                ' Display Components
                DisplayComponents(typeID)

                ' Display the time taken and other info
                endTime = Now
                lblStatus.Text = "Last item retrieved in " & (endTime - startTime).TotalMilliseconds.ToString("N2") & "ms"
                lblDBLocation.Text = "DB Location: " & StaticData.TypeCats(_item.Category) & " > " & StaticData.TypeGroups(_item.Group)
                Dim dbLocation As New StringBuilder
                dbLocation.AppendLine("Database Category: " & StaticData.TypeCats(_item.Category))
                dbLocation.AppendLine("Database Group: " & StaticData.TypeGroups(_item.Group))
                dbLocation.AppendLine("Database Item: " & _item.Name)
                dbLocation.AppendLine("")
                dbLocation.AppendLine("Double-click here to open the database group in the browser")
                STTItem.SetSuperTooltip(lblDBLocation, New SuperTooltipInfo("", "Database Location Information", dbLocation.ToString, ImageHandler.GetImage(typeID), My.Resources.Info32, eTooltipColor.Yellow))
                lblID.Text = "ID: " & _item.Category.ToString & " > " & _item.Group.ToString & " > " & _item.Id.ToString
                Dim dbID As New StringBuilder
                dbID.AppendLine("Category ID: " & _item.Category.ToString)
                dbID.AppendLine("Group ID: " & _item.Group.ToString)
                dbID.AppendLine("Item ID: " & _item.Id.ToString)
                dbID.AppendLine("")
                dbID.AppendLine("Double-click here to manually enter an item ID")
                STTItem.SetSuperTooltip(lblID, New SuperTooltipInfo("", "ID Information", dbID.ToString, ImageHandler.GetImage(typeID), My.Resources.Info32, eTooltipColor.Yellow))
                btnRequisition.Enabled = True
            Else
                MessageBox.Show("Unable to find item with ID of " & typeID.ToString, "Unknown Item!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        End Sub

        Private Sub DisplayDescription(description As String)

            ' Need to replace the CRLF with HTML to display correctly in all cases
            description = description.Replace(ControlChars.CrLf, "<br />").Replace("<br>", "<br />")

            ' Identify internal CCP "showinfo" links and replace with something that we can actually use internally for EveHQ :)
            Dim matches As MatchCollection = Regex.Matches(description, "<a\shref=(?<url>.*?)>(?<text>.*?)</a>")
            For Each m As Match In matches
                Dim typeID As String = m.Groups("url").Value.TrimStart("<showinfo:".ToCharArray)
                description = description.Replace(m.Value, "<a href='http://" & typeID & "'>" & m.Groups("text").Value & "</a>")
            Next
            lblDescription.Text = description

        End Sub

#Region "Attribute Display Routines"

        Private Sub DisplayAttributes(item As EveType)

            ' Get the attribute data
            GetAttributeData(CInt(item.Id))

            adtAttributes.BeginUpdate()
            adtAttributes.Nodes.Clear()
            DisplayBasicAttributes(CInt(item.Id))
            If item.Category = 9 Then
                DisplayBPAttributes(CInt(item.Id))
            Else
                DisplayMainAttributes()
            End If
            adtAttributes.EndUpdate()

            ' Update the tab
            If adtAttributes.Nodes.Count = 0 Then
                tiAttributes.Visible = False
            Else
                tiAttributes.Visible = True
            End If

        End Sub

        Private Sub GetAttributeData(typeID As Integer)

            ' Get the attribute list
            _attributeList = StaticData.GetAttributeDataForItem(typeID)

            ' Adjust for skills
            _itemSkills = New Dictionary(Of Integer, Integer)
            For Each att As ItemAttribData In _attributeList.Values
                ' Adjust for skills (ignore skill levels)
                Select Case att.Id
                    Case 182, 183, 184, 1285, 1289, 1290
                        Select Case att.Id
                            Case 182
                                _itemSkills.Add(CInt(att.Value), CInt(_attributeList(277).Value))
                            Case 183
                                _itemSkills.Add(CInt(att.Value), CInt(_attributeList(278).Value))
                            Case 184
                                _itemSkills.Add(CInt(att.Value), CInt(_attributeList(279).Value))
                            Case 1285
                                _itemSkills.Add(CInt(att.Value), CInt(_attributeList(1286).Value))
                            Case 1289
                                _itemSkills.Add(CInt(att.Value), CInt(_attributeList(1287).Value))
                            Case 1290
                                _itemSkills.Add(CInt(att.Value), CInt(_attributeList(1288).Value))
                        End Select
                End Select
            Next

        End Sub

        Private Sub DisplayBasicAttributes(typeID As Integer)

            ' Add in basic attributes
            Dim newAtt As New Node
            newAtt.Name = "A" : newAtt.Text = "(" & newAtt.Name & ") Group ID"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).Group.ToString))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "B" : newAtt.Text = "(" & newAtt.Name & ") Category ID"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).Category.ToString))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "C" : newAtt.Text = "(" & newAtt.Name & ") Market Group ID"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).MarketGroupId.ToString))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "D" : newAtt.Text = "(" & newAtt.Name & ") Mass"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).Mass.ToString("N0") & " kg"))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "E" : newAtt.Text = "(" & newAtt.Name & ") Volume"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).Volume.ToString("N2") & " m3"))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "F" : newAtt.Text = "(" & newAtt.Name & ") Capacity"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).Capacity.ToString("N2") & " m3"))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "G" : newAtt.Text = "(" & newAtt.Name & ") Portion Size"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).PortionSize.ToString("N0")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "H" : newAtt.Text = "(" & newAtt.Name & ") Published"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).Published.ToString))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "I1" : newAtt.Text = "(" & newAtt.Name & ") Base Price"
            newAtt.Cells.Add(New Cell(StaticData.Types(typeID).BasePrice.ToString("N2")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "I2" : newAtt.Text = "(" & newAtt.Name & ") Market Price"
            'If HQ.MarketPriceList.ContainsKey(typeID) = True Then
            '    newAtt.Cells.Add(New Cell(HQ.MarketPriceList(typeID).ToString("N2")))
            'Else
            '    newAtt.Cells.Add(New Cell(CInt("0").ToString("N2")))
            'End If
            newAtt.Cells.Add(New Cell(Math.Round(CDbl(DataFunctions.GetPrice(typeID)), 2, MidpointRounding.AwayFromZero).ToString("N2")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "I3" : newAtt.Text = "(" & newAtt.Name & ") Custom Price"
            If HQ.CustomPriceList.ContainsKey(typeID) = True Then
                newAtt.Cells.Add(New Cell(HQ.CustomPriceList(typeID).ToString("N2")))
            Else
                newAtt.Cells.Add(New Cell(CInt("0").ToString("N2")))
            End If
            adtAttributes.Nodes.Add(newAtt)

        End Sub

        Private Sub DisplayMainAttributes()

            ' Add main attributes
            Dim newAtt As Node
            For Each att As ItemAttribData In _attributeList.Values
                newAtt = New Node
                newAtt.Name = att.Id.ToString
                newAtt.Text = "(" & att.Id.ToString & ") " & StaticData.AttributeTypes(att.Id).DisplayName
                newAtt.Cells.Add(New Cell(att.DisplayValue & att.Unit))

                ' Add attributes to the list (but ignore skill levels)
                Select Case att.Id
                    Case 277, 278, 279, 1286, 1287, 1288
                        ' Don't add it
                    Case Else
                        adtAttributes.Nodes.Add(newAtt)
                End Select
            Next

        End Sub

        Private Sub DisplayBPAttributes(typeID As Integer)

            ' Fetch the relevant blueprint
            Dim bp As Blueprint = StaticData.Blueprints(typeID)

            ' Add in basic attributes
            Dim newAtt As New Node
            newAtt.Name = "BP1" : newAtt.Text = "(" & newAtt.Name & ") Tech Level"
            newAtt.Cells.Add(New Cell(bp.TechLevel.ToString("N0")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP2" : newAtt.Text = "(" & newAtt.Name & ") Waste Factor"
            newAtt.Cells.Add(New Cell(bp.WasteFactor.ToString("N0")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP3" : newAtt.Text = "(" & newAtt.Name & ") Material Modifier"
            newAtt.Cells.Add(New Cell(bp.MaterialModifier.ToString("N0")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP4" : newAtt.Text = "(" & newAtt.Name & ") Productivity Modifier"
            newAtt.Cells.Add(New Cell(bp.ProductivityModifier.ToString("N0")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP5" : newAtt.Text = "(" & newAtt.Name & ") Max Production Limit"
            newAtt.Cells.Add(New Cell(bp.MaxProductionLimit.ToString("N0")))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP6" : newAtt.Text = "(" & newAtt.Name & ") Production Time"
            newAtt.Cells.Add(New Cell(SkillFunctions.TimeToString(bp.ProductionTime)))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP7" : newAtt.Text = "(" & newAtt.Name & ") Research ML Time"
            newAtt.Cells.Add(New Cell(SkillFunctions.TimeToString(bp.ResearchMaterialLevelTime)))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP8" : newAtt.Text = "(" & newAtt.Name & ") Research PL Time"
            newAtt.Cells.Add(New Cell(SkillFunctions.TimeToString(bp.ResearchProductionLevelTime)))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP9" : newAtt.Text = "(" & newAtt.Name & ") Research Copy Time"
            newAtt.Cells.Add(New Cell(SkillFunctions.TimeToString(bp.ResearchCopyTime)))
            adtAttributes.Nodes.Add(newAtt)

            newAtt = New Node
            newAtt.Name = "BP10" : newAtt.Text = "(" & newAtt.Name & ") Research Tech Time"
            newAtt.Cells.Add(New Cell(SkillFunctions.TimeToString(bp.ResearchTechTime)))
            adtAttributes.Nodes.Add(newAtt)

        End Sub

#End Region

        Private Sub DisplayEffects(typeID As Integer)

            ' Fetch the attributes for the item
            Dim effects As List(Of Integer) = (From te In StaticData.TypeEffects
                    Where (te.TypeId = typeID)
                    Select te.EffectId).ToList

            Dim newAtt As Node
            adtEffects.BeginUpdate()
            adtEffects.Nodes.Clear()
            For Each effect As Integer In effects
                newAtt = New Node
                newAtt.Name = effect.ToString
                newAtt.Text = "(" & effect.ToString & ") " & StaticData.EffectTypes(effect).EffectName
                adtEffects.Nodes.Add(newAtt)
            Next
            adtEffects.EndUpdate()

            ' Update the tab
            If adtEffects.Nodes.Count = 0 Then
                tiEffects.Visible = False
            Else
                tiEffects.Visible = True
            End If

        End Sub

        Private Sub DisplayFitting()

            adtFitting.BeginUpdate()
            adtFitting.Nodes.Clear()
            ' Add fitting attributes
            Dim newAtt As Node
            Dim att As ItemAttribData
            For Each id As Integer In _fittingIDList
                If _attributeList.ContainsKey(id) Then
                    att = _attributeList(id)
                    newAtt = New Node
                    newAtt.Name = att.Id.ToString
                    newAtt.Text = "(" & att.Id.ToString & ") " & StaticData.AttributeTypes(att.Id).DisplayName
                    newAtt.Cells.Add(New Cell(att.DisplayValue & att.Unit))
                    adtFitting.Nodes.Add(newAtt)
                End If
            Next
            adtFitting.EndUpdate()

            ' Update the tab
            If adtFitting.Nodes.Count = 0 Then
                tiFitting.Visible = False
            Else
                tiFitting.Visible = True
            End If

        End Sub

        Private Sub DisplaySkills(typeID As Integer)
            If cboPilots.SelectedItem IsNot Nothing Then
                Dim pilotName As String = cboPilots.SelectedItem.ToString
                adtSkills.DisplaySkills(typeID, _itemSkills, HQ.Settings.Pilots(pilotName))
                If adtSkills.ItemIsUsable = True Then
                    lblUsableTime.Text = pilotName & " has the skills to use this item."
                Else
                    lblUsableTime.Text = pilotName & " doesn't have the skills to use this item (" & SkillFunctions.TimeToString(adtSkills.ItemUsableTime) & ")."
                End If
                If adtSkills.RequiredSkills.Count = 0 Then
                    btnAddSkills.Enabled = False
                Else
                    btnAddSkills.Enabled = True
                End If
            Else
                adtSkills.DisplaySkills(typeID, _itemSkills, Nothing)
                lblUsableTime.Text = "No pilot selected for usage time calculations."
                btnAddSkills.Enabled = False
            End If

            ' Update the tab
            If adtSkills.adtSkills.Nodes.Count = 0 Then
                tiSkills.Visible = False
            Else
                tiSkills.Visible = True
            End If

        End Sub

        Private Sub DisplayCertificates(typeID As Integer)

            adtCerts.BeginUpdate()
            adtCerts.Nodes.Clear()

            If _item.Category = 6 Then

                ' Fetch the certificates for this ship
                Dim certs As List(Of Integer) = (From cr As CertificateRecommendation In StaticData.CertificateRecommendations
                        Where (cr.ShipTypeId = typeID)
                        Select cr.CertificateId).ToList

                For Each cert As Integer In certs
                    Dim newCert As Certificate = StaticData.Certificates(cert)
                    Dim certNode As New Node
                    certNode.Text = newCert.Name
                    certNode.Tag = newCert.Id
                    'adtCerts.Nodes.Add(certNode)
                    'certNode.Cells.Add(New Cell)
                    'certNode.Cells(1).Tag = newCert.Grade
                    'certNode.Image = CType(My.Resources.ResourceManager.GetObject("Cert" & newCert.Grade.ToString), Image)
                    'Select Case newCert.Grade
                    '    Case 1
                    '        certNode.Cells(1).Text = "Basic"
                    '    Case 2
                    '        certNode.Cells(1).Text = "Standard"
                    '    Case 3
                    '        certNode.Cells(1).Text = "Improved"
                    '    Case 4
                    '        certNode.Cells(1).Text = "Advanced"
                    '    Case 5
                    '        certNode.Cells(1).Text = "Elite"
                    'End Select
                Next

            End If

            adtCerts.EndUpdate()

            ' Update the tab
            If adtCerts.Nodes.Count = 0 Then
                tiCertificates.Visible = False
            Else
                tiCertificates.Visible = True
            End If

        End Sub

        Private Sub DisplayVariations(typeID As Integer)

            ' Fetch the parent item ID for this item
            Dim parentTypeID As Integer = typeID
            If StaticData.MetaTypes.ContainsKey(typeID) Then
                parentTypeID = StaticData.MetaTypes(typeID).ParentId
            End If
            ' Fetch all items with this same parent ID
            Dim itemIDs As List(Of Integer) = (From mt As MetaType In StaticData.MetaTypes.Values
                    Where (mt.ParentId = parentTypeID)
                    Select mt.Id).ToList
            ' Add the current item if it is the parent item
            If typeID = parentTypeID Then
                itemIDs.Add(typeID)
            End If

            ' Query the attributes table
            Dim tas As List(Of TypeAttrib) = (From ta In StaticData.TypeAttributes
                    Where itemIDs.Contains(ta.TypeId)
                    Select ta).ToList

            ' Cycle through the attributes for prepare columns
            Dim columnList As New SortedList(Of Integer, String)
            Dim atts As New Dictionary(Of Integer, Dictionary(Of Integer, ItemAttribData))
            For Each ta As TypeAttrib In tas
                ' Check for the column
                If columnList.ContainsKey(ta.AttributeId) = False Then
                    columnList.Add(ta.AttributeId, StaticData.AttributeTypes(ta.AttributeId).DisplayName)
                End If
                ' Check for the item
                If atts.ContainsKey(ta.TypeId) = False Then
                    atts.Add(ta.TypeId, New Dictionary(Of Integer, ItemAttribData))
                End If
                atts(ta.TypeId).Add(ta.AttributeId, New ItemAttribData(ta.AttributeId, ta.Value, StaticData.AttributeTypes(ta.AttributeId).DisplayName, StaticData.AttributeUnits(StaticData.AttributeTypes(ta.AttributeId).UnitId)))
            Next

            ' Begin update of the list view
            adtVariations.BeginUpdate()
            adtVariations.Nodes.Clear()
            adtVariations.Columns.Clear()

            ' Add the name column
            Dim col As New DevComponents.AdvTree.ColumnHeader
            col.Name = "typeName"
            col.Text = "Item Name"
            col.Width.Absolute = 250
            adtVariations.Columns.Add(col)
            ' Add the meta level column
            Dim mlCol As New DevComponents.AdvTree.ColumnHeader
            mlCol.Name = "633"
            mlCol.Text = "Meta"
            mlCol.Width.Absolute = 50
            adtVariations.Columns.Add(mlCol)
            ' Add the other columns
            For Each colID As Integer In columnList.Keys
                If colID <> 633 Then
                    col = New DevComponents.AdvTree.ColumnHeader
                    col.Name = colID.ToString
                    col.Text = columnList(colID)
                    col.Width.Absolute = 75
                    adtVariations.Columns.Add(col)
                End If
            Next

            ' Add all the items
            For Each itemID As Integer In atts.Keys
                Dim attList As Dictionary(Of Integer, ItemAttribData) = atts(itemID)
                Dim typeNode As New Node
                typeNode.Name = itemID.ToString
                typeNode.Text = StaticData.Types(itemID).Name
                adtVariations.Nodes.Add(typeNode)
                typeNode.CreateCells()
                For colID As Integer = 1 To adtVariations.Columns.Count - 1
                    Dim attID As Integer = CInt(adtVariations.Columns(colID).Name)
                    If attList.ContainsKey(attID) Then
                        StaticData.CorrectAttributeValue(attList(attID))
                        typeNode.Cells(colID).Text = attList(attID).DisplayValue
                    Else
                        typeNode.Cells(colID).Text = "n/a"
                    End If
                Next
            Next

            ' Hide columns which contain identical values
            For colID As Integer = 2 To adtVariations.Columns.Count - 1
                adtVariations.Columns(colID).Visible = False
                For nodeID As Integer = 1 To adtVariations.Nodes.Count - 1
                    If adtVariations.Nodes(nodeID - 1).Cells(colID).Text <> adtVariations.Nodes(nodeID).Cells(colID).Text Then
                        adtVariations.Columns(colID).Visible = True
                        Exit For
                    End If
                Next
            Next

            ' Finish update of the list view
            adtVariations.EndUpdate()

            ' Update the tab
            If adtVariations.Nodes.Count = 0 Then
                tiVariations.Visible = False
            Else
                tiVariations.Visible = True
            End If

        End Sub

        Private Sub DisplayMaterials(itemID As Integer)

            adtMaterials.BeginUpdate()
            adtMaterials.Nodes.Clear()

            Dim bpID As Integer = 0
            ' Check if this is a blueprint
            If StaticData.Blueprints.ContainsKey(itemID) = True Then
                bpID = itemID
            Else
                ' Check if this item has a blueprint
                Dim itemIDs As List(Of Integer) = (From bt As Blueprint In StaticData.Blueprints.Values
                        Where (bt.ProductId = itemID)
                        Select bt.Id).ToList
                If itemIDs.Count > 0 Then
                    bpID = itemIDs(0)
                End If
            End If

            ' Process only if the BP ID <> 0 
            If bpID <> 0 Then

                ' Get the materials for the BP and add them to the list
                Const Activity As Integer = 1
                For Each br As BlueprintResource In StaticData.Blueprints(bpID).Resources(Activity).Values
                    Dim mn As New Node
                    mn.Text = StaticData.Types(br.TypeId).Name
                    mn.Cells.Add(New Cell(br.Quantity.ToString("N0")))
                    adtMaterials.Nodes.Add(mn)
                Next

            End If

            adtMaterials.EndUpdate()

            ' Update the tab
            If adtMaterials.Nodes.Count = 0 Then
                tiMaterials.Visible = False
            Else
                tiMaterials.Visible = True
            End If

        End Sub

        Private Sub DisplayComponents(itemID As Integer)

            adtComponents.BeginUpdate()
            adtComponents.Nodes.Clear()

            Dim bpID As Integer = 0
            ' Check if this is a blueprint
            If StaticData.Blueprints.ContainsKey(itemID) = True Then
                bpID = itemID
            Else
                ' Check if this item has a blueprint
                Dim itemIDs As List(Of Integer) = (From bt As Blueprint In StaticData.Blueprints.Values
                        Where (bt.ProductId = itemID)
                        Select bt.Id).ToList
                If itemIDs.Count > 0 Then
                    bpID = itemIDs(0)
                End If
            End If

            ' Process only if the BP ID <> 0 
            If bpID <> 0 Then

                ' Get the materials for the BP and add them to the list
                Const Activity As Integer = 1
                For Each bt As Blueprint In StaticData.Blueprints.Values
                    If bt.Resources.ContainsKey(Activity) Then
                        If bt.Resources(Activity).ContainsKey(itemID) Then
                            Dim mn As New Node
                            mn.Text = StaticData.Types(bt.ProductId).Name
                            mn.Cells.Add(New Cell(bt.Resources(Activity).Item(itemID).Quantity.ToString("N0")))
                            adtComponents.Nodes.Add(mn)
                        End If
                    End If
                Next

            End If

            adtComponents.EndUpdate()

            ' Update the tab
            If adtMaterials.Nodes.Count = 0 Then
                tiMaterials.Visible = False
            Else
                tiMaterials.Visible = True
            End If


        End Sub

#End Region

#Region "Navigation Routines"

        Private Sub AddToNavigation()

            ' Put the last item on the stack & reset the forward stack
            _navBack.Push(_item.Id)
            _navForward.Clear()

        End Sub

        Private Sub btnNavBack_Click(sender As Object, e As EventArgs) Handles btnNavBack.Click
            ' Put the current item onto the forward stack
            _navForward.Push(_item.Id)
            ' Take one from the back stack and display it
            DisplayItem(_navBack.Pop, False)
        End Sub

        Private Sub btnNavForward_Click(sender As Object, e As EventArgs) Handles btnNavForward.Click
            ' Put the current item onto the back stack
            _navBack.Push(_item.Id)
            ' Take one from the forward stack and display it
            DisplayItem(_navForward.Pop, False)
        End Sub

        Private Sub DrawNavOptions()
            ' Check back stack
            If _navBack.Count = 0 Then
                btnNavBack.Enabled = False
            Else
                btnNavBack.Enabled = True
            End If
            ' Check forward stack
            If _navForward.Count = 0 Then
                btnNavForward.Enabled = False
            Else
                btnNavForward.Enabled = True
            End If
            ' Check recent items
            If _recentItems.Count = 0 Then
                btnNavRecent.Enabled = False
            Else
                For Each mnuItem As ButtonItem In btnNavRecent.SubItems
                    RemoveHandler mnuItem.Click, AddressOf NavMenuClickHandler
                Next
                ' Sort items by name
                Dim sortedItems As New SortedList(Of String, Integer)
                For idx As Integer = 0 To Math.Min(9, _recentItems.Count - 1)
                    sortedItems.Add(StaticData.Types(_recentItems(idx)).Name, _recentItems(idx))
                Next
                ' Display sorted recent items
                btnNavRecent.SubItems.Clear()
                For Each itemName As String In sortedItems.Keys
                    Dim mnuItem As New ButtonItem
                    mnuItem.Name = sortedItems(itemName).ToString
                    mnuItem.Text = itemName
                    AddHandler mnuItem.Click, AddressOf NavMenuClickHandler
                    btnNavRecent.SubItems.Add(mnuItem)
                    btnNavRecent.Enabled = True
                Next
            End If
        End Sub

        Private Sub NavMenuClickHandler(sender As Object, e As EventArgs)
            Dim mnuItem As ButtonItem = CType(sender, ButtonItem)
            Dim itemID As Integer = CInt(mnuItem.Name)
            DisplayItem(itemID, True)
        End Sub

#End Region

#Region "Button & Other UI Routines"

        Private Sub lblID_DoubleClick(sender As Object, e As EventArgs) Handles lblID.DoubleClick
            Dim item As String = InputBox(ControlChars.CrLf & "Please enter the itemID to jump to...", "Jump to ItemID")
            If IsNumeric(item) = True Then
                If StaticData.TypeNames.ContainsValue(CInt(item)) = True Then
                    Call DisplayItem(CInt(item), True)
                End If
            End If
        End Sub

        Private Sub lblDBLocation_DoubleClick(sender As Object, e As EventArgs) Handles lblDBLocation.DoubleClick
            adtBrowse.BeginUpdate()
            adtBrowse.Select()
            adtBrowse.CollapseAll()
            adtBrowse.FindNodeByName("c" & _item.Category.ToString).Expand()
            Dim groupNode As Node = adtBrowse.FindNodeByName("g" & _item.Group.ToString)
            groupNode.Expand()
            groupNode.LastNode.EnsureVisible()
            groupNode.EnsureVisible()
            adtBrowse.EndUpdate()
            tabBrowser.SelectedTab = tabBrowse
        End Sub

        Private Sub cboPilots_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPilots.SelectedIndexChanged
            ' Update the display for the current pilot
            If _item.Id > 0 Then

                Call DisplaySkills(_item.Id)

            End If
        End Sub

        Private Sub btnRequisition_Click(sender As Object, e As EventArgs) Handles btnRequisition.Click
            Dim orders As New SortedList(Of String, Integer)
            orders.Add(_item.Name, 1)
            Using newReq As New FrmAddRequisition("Item Browser", orders)
                newReq.ShowDialog()
            End Using
        End Sub

        Private Sub btnAddSkills_Click(sender As Object, e As EventArgs) Handles btnAddSkills.Click
            ' Add the needed skills to a queue
            Dim pilotName As String = cboPilots.SelectedItem.ToString
            Using selQ As New FrmSelectQueue(pilotName, adtSkills.RequiredSkills, "Item Browser: " & _item.Name)
                selQ.ShowDialog()
            End Using
            SkillQueueFunctions.StartQueueRefresh = True
            ' Refresh the skills display
            adtSkills.DisplaySkills(_item.Id, _itemSkills, HQ.Settings.Pilots(pilotName))
        End Sub

        Private Sub lblDescription_MarkupLinkClick(sender As Object, e As MarkupLinkClickEventArgs) Handles lblDescription.MarkupLinkClick
            Dim typeID As String = e.HRef.TrimStart("http://".ToCharArray)
            If StaticData.TypeNames.ContainsValue(CInt(typeID)) = True Then
                Call DisplayItem(CInt(typeID), True)
            End If
        End Sub

#End Region

    End Class

End Namespace