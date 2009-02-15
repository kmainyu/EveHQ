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
    Dim StationYield As Double = 0
    Dim StationTake As Double = 0
    Dim StationStanding As Double = 0

#Region "Public Properties"
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
#End Region

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
        strSQL &= " WHERE (typeActivityMaterials.typeID IN (" & strAssets.ToString & ") AND typeActivityMaterials.activityID IN (6,9) AND invTypes.typeID NOT IN (10298,11473)) ORDER BY invCategories.categoryName, invGroups.groupName"
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
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next

        ' Get the location details
        If PlugInData.stations.ContainsKey(cAssetLocation) = True Then

            If CDbl(AssetLocation) >= 60000000 Then ' Is a station
                Dim aLocation As Station = CType(PlugInData.stations(cAssetLocation), Station)
                lblStation.Text = aLocation.stationName
                lblCorp.Text = aLocation.corpID.ToString
                If PlugInData.NPCCorps.ContainsKey(aLocation.corpID.ToString) = True Then
                    lblCorp.Text = CStr(PlugInData.NPCCorps(aLocation.corpID.ToString))
                    lblCorp.Tag = aLocation.corpID.ToString
                    StationYield = aLocation.refiningEff
                    lblBaseYield.Text = FormatNumber(StationYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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
            Else ' Is a system
                lblStation.Text = "n/a"
                lblCorp.Text = "n/a"
                lblCorp.Tag = Nothing
                lblBaseYield.Text = FormatNumber(50, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
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

            ' Set the recycling mode
            cboRefineMode.SelectedIndex = 0

    End Sub

    Private Sub RecalcRecycling()
        ' Create the main list
        clvRecycle.BeginUpdate()
        clvRecycle.Items.Clear()
        Dim price As Double = 0
        Dim perfect As Long = 0
        Dim quant As Long = 0
        Dim wastage As Long = 0
        Dim taken As Long = 0
        Dim recycleTotal As Double = 0
        Dim newCLVItem As New ContainerListViewItem
        Dim newCLVSubItem As New ContainerListViewItem
        Dim itemInfo As New ItemData
        Dim batches As Integer = 0
        Dim items As Long = 0
        Dim volume As Double = 0
        Dim tempNetYield As Double = 0
        Dim RecycleResults As New SortedList
        Dim RecycleWaste As New SortedList
        Dim RecycleTake As New SortedList
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
        For Each asset As String In cAssetList.Keys
            itemInfo = CType(PlugInData.Items(asset), ItemData)
            If itemInfo.Category = 25 Then
                Select Case itemInfo.Group
                    Case 465 ' Ice
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.IceProc))))
                    Case 450 ' Arkonor
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ArkonorProc))))
                    Case 451 ' Bistot
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.BistotProc))))
                    Case 452 ' Crokite
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.CrokiteProc))))
                    Case 453 ' Dark Ochre
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.DarkOchreProc))))
                    Case 467 ' Gneiss
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.GneissProc))))
                    Case 454 ' Hedbergite
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.HedbergiteProc))))
                    Case 455 ' Hemorphite
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.HemorphiteProc))))
                    Case 456 ' Jaspet
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JaspetProc))))
                    Case 457 ' Kernite
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.KerniteProc))))
                    Case 468 ' Mercoxit
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.MercoxitProc))))
                    Case 469 ' Omber
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.OmberProc))))
                    Case 458 ' Plagioclase
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.PlagioclaseProc))))
                    Case 459 ' Pyroxeres
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.PyroxeresProc))))
                    Case 460 ' Scordite
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ScorditeProc))))
                    Case 461 ' Spodumain
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.SpodumainProc))))
                    Case 462 ' Veldspar
                        tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.VeldsparProc))))
                End Select
            Else
                tempNetYield = NetYield * (1 + (0.05 * CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ScrapMetalProc))))
            End If
            tempNetYield = Math.Min(tempNetYield, 1)
            matList = CType(itemList(asset), Collections.SortedList)
            newCLVItem = New ContainerListViewItem
            newCLVItem.Text = itemInfo.Name
            clvRecycle.Items.Add(newCLVItem)
            price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(asset), 2)
            batches = CInt(Int(CLng(cAssetList(itemInfo.ID.ToString)) / itemInfo.PortionSize))
            quant = CLng(cAssetList(asset))
            volume += itemInfo.Volume * quant
            items += CLng(quant)
            newCLVItem.SubItems(1).Text = FormatNumber(itemInfo.MetaLevel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(2).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(3).Text = FormatNumber(batches, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            newCLVItem.SubItems(5).Text = FormatNumber(price * quant, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            recycleTotal = 0
            If matList IsNot Nothing Then ' i.e. it can be refined
                For Each mat As String In matList.Keys
                    price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(mat).ToString), 2)
                    perfect = CLng(matList(mat)) * batches
                    wastage = CLng(perfect * (1 - tempNetYield))
                    quant = CLng(perfect * tempNetYield)
                    taken = CLng(quant * (StationTake / 100))
                    quant = quant - taken
                    newCLVSubItem = New ContainerListViewItem
                    newCLVSubItem.Text = mat
                    newCLVItem.Items.Add(newCLVSubItem)
                    newCLVSubItem.SubItems(2).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(3).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(5).Text = FormatNumber(price * quant, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    newCLVSubItem.SubItems(6).Text = newCLVSubItem.SubItems(5).Text
                    recycleTotal += price * quant
                    ' Save the perfect refining quantity
                    If RecycleResults.Contains(mat) = False Then
                        RecycleResults.Add(mat, quant)
                    Else
                        RecycleResults(mat) = CDbl(RecycleResults(mat)) + quant
                    End If
                    ' Save the wasted amounts
                    If RecycleWaste.Contains(mat) = False Then
                        RecycleWaste.Add(mat, wastage)
                    Else
                        RecycleWaste(mat) = CDbl(RecycleWaste(mat)) + wastage
                    End If
                    ' Save the take amounts
                    If RecycleTake.Contains(mat) = False Then
                        RecycleTake.Add(mat, taken)
                    Else
                        RecycleTake(mat) = CDbl(RecycleTake(mat)) + taken
                    End If
                Next
            End If
            newCLVItem.SubItems(6).Text = FormatNumber(recycleTotal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            If CDbl(newCLVItem.SubItems(6).Text) > CDbl(newCLVItem.SubItems(5).Text) Then
                newCLVItem.BackColor = Drawing.Color.LightGreen
            End If
        Next
        clvRecycle.Sort(0, SortOrder.Ascending, True)
        clvRecycle.EndUpdate()
        lblVolume.Text = FormatNumber(volume, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m³"
        lblItems.Text = FormatNumber(clvRecycle.Items.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblItems.Text &= " (" & FormatNumber(items, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")"
        ' Create the totals list
        clvTotals.BeginUpdate()
        clvTotals.Items.Clear()
        If RecycleResults IsNot Nothing Then
            For Each mat As String In RecycleResults.Keys
                price = Math.Round(EveHQ.Core.DataFunctions.GetPrice(EveHQ.Core.HQ.itemList(mat).ToString), 2)
                wastage = CLng(RecycleWaste(mat))
                taken = CLng(RecycleTake(mat))
                quant = CLng(RecycleResults(mat))
                newCLVItem = New ContainerListViewItem
                newCLVItem.Text = mat
                clvTotals.Items.Add(newCLVItem)
                newCLVItem.SubItems(1).Text = FormatNumber(taken, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(2).Text = FormatNumber(wastage, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(3).Text = FormatNumber(quant, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(4).Text = FormatNumber(price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLVItem.SubItems(5).Text = FormatNumber(quant * price, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Next
        End If
        clvTotals.Sort(3, SortOrder.Descending, True)
        clvTotals.EndUpdate()
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
        If chkPerfectRefine.Checked = True Then
            NetYield = 1
        Else
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
        End If
        lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        If lblCorp.Tag IsNot Nothing Then
            StationStanding = GetStanding(rPilot.Name, lblCorp.Tag.ToString)
        Else
            StationStanding = 0
        End If
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            If lblCorp.Tag Is Nothing Then
                lblStandings.Text = FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Else
                lblStandings.Text = FormatNumber(StationStanding, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            End If
        End If
        Call Me.RecalcRecycling()
    End Sub

    Private Function GetStanding(ByVal pilotName As String, ByVal corpID As String) As Double
        ' Check for the existence of the CorpHQ plug-in and try and get the standing data
        Dim PluginName As String = "CorpHQ"
        Dim myPlugIn As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(PluginName), Core.PlugIn)
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

#Region "Override Base Yield functions"
    Private Sub chkOverrideBaseYield_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideBaseYield.CheckedChanged
        If chkOverrideBaseYield.Checked = True Then
            BaseYield = CDbl(nudBaseYield.Value) / 100
        Else
            BaseYield = StationYield
        End If
        lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
        NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub

    Private Sub nudBaseYield_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudBaseYield.ValueChanged
        If chkOverrideBaseYield.Checked = True Then
            BaseYield = CDbl(nudBaseYield.Value) / 100
            lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
            lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Call Me.RecalcRecycling()
        End If
    End Sub
#End Region

#Region "Override Standings functions"
    Private Sub chkOverrideStandings_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideStandings.CheckedChanged
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            If lblCorp.Tag Is Nothing Then
                lblStandings.Text = FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Else
                lblStandings.Text = FormatNumber(StationStanding, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            End If
        End If
        Call Me.RecalcRecycling()
    End Sub

    Private Sub lblStandings_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblStandings.TextChanged
        StationTake = Math.Max(5 - (0.75 * CDbl(lblStandings.Text)), 0)
        lblStationTake.Text = FormatNumber(StationTake, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
    End Sub

    Private Sub nudStandings_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudStandings.ValueChanged
        If chkOverrideStandings.Checked = True Then
            lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Call Me.RecalcRecycling()
        End If
    End Sub

#End Region

#Region "Override Refining Skills functions"
    Private Sub chkPerfectRefine_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPerfectRefine.CheckedChanged
        If chkPerfectRefine.Checked = True Then
            NetYield = 1
        Else
            Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
            NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
        End If
        lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
        Call Me.RecalcRecycling()
    End Sub
#End Region

#Region "Refining Mode functions"
    Private Sub cboRefineMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRefineMode.SelectedIndexChanged
        Select Case cboRefineMode.SelectedIndex
            Case 0 ' Standard
                If chkOverrideBaseYield.Checked = True Then
                    BaseYield = CDbl(nudBaseYield.Value) / 100
                Else
                    BaseYield = StationYield
                End If
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                If chkPerfectRefine.Checked = True Then
                    NetYield = 1
                Else
                    Dim rPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilots.SelectedItem.ToString), Core.Pilot)
                    NetYield = (BaseYield) + (0.375 * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Refining)) * 0.02)) * (1 + (CDbl(rPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.RefiningEfficiency)) * 0.04)))
                End If
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                If chkOverrideStandings.Checked = True Then
                    lblStandings.Text = FormatNumber(nudStandings.Value, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                Else
                    If lblCorp.Tag Is Nothing Then
                        lblStandings.Text = FormatNumber(0, 4, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    Else
                        lblStandings.Text = FormatNumber(StationStanding, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    End If
                End If
                chkOverrideBaseYield.Enabled = True
                chkOverrideStandings.Enabled = True
                chkPerfectRefine.Enabled = True
                nudBaseYield.Enabled = True
                nudStandings.Enabled = True
                cboPilots.Enabled = True
            Case 1 ' Refining Array
                BaseYield = 0.35
                NetYield = 0.35
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblStandings.Text = FormatNumber(10, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                chkOverrideBaseYield.Enabled = False
                chkOverrideStandings.Enabled = False
                chkPerfectRefine.Enabled = False
                nudBaseYield.Enabled = False
                nudStandings.Enabled = False
                cboPilots.Enabled = False
            Case 2 ' Intensive Refining Array
                BaseYield = 0.75
                NetYield = 0.75
                lblBaseYield.Text = FormatNumber(BaseYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblNetYield.Text = FormatNumber(NetYield * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                lblStandings.Text = FormatNumber(10, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                chkOverrideBaseYield.Enabled = False
                chkOverrideStandings.Enabled = False
                chkPerfectRefine.Enabled = False
                nudBaseYield.Enabled = False
                nudStandings.Enabled = False
                cboPilots.Enabled = False
        End Select
        Call Me.RecalcRecycling()
    End Sub
#End Region
    
    
   
End Class