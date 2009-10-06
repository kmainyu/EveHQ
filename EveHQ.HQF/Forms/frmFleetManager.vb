Imports DotNetLib.Windows.Forms
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmFleetManager
    Dim maxWings As Integer = 5
    Dim maxSquads As Integer = 5
    Dim maxMembers As Integer = 10
    Dim OpenFleet As New ArrayList
    Dim OpenWings As New ArrayList
    Dim OpenSquads As New ArrayList
    Dim OpenPilots As New ArrayList
    Dim ExpandAll As Boolean = False
    Dim activeFleet As New FleetManager.Fleet
    Dim activeFleetMembers As New SortedList(Of String, FleetManager.FleetMember)
    Dim BaseFleetShips As New SortedList(Of String, Ship)
    Dim FinalFleetShips As New SortedList(Of String, Ship)
    Dim internalReorder As Boolean = False
    Dim remoteGroups As New ArrayList
    Dim fleetGroups As New ArrayList
    Dim fleetSkills As New ArrayList
    Dim SBModules, WBModules, FBModules As New ArrayList

#Region "Form Initialisation, Loading and Closing"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Setup the default fleet structure objects
        remoteGroups.Add(41)
        remoteGroups.Add(325)
        remoteGroups.Add(585)
        remoteGroups.Add(67)
        remoteGroups.Add(65)
        remoteGroups.Add(68)
        remoteGroups.Add(71)
        remoteGroups.Add(291)
        remoteGroups.Add(209)
        remoteGroups.Add(289)
        remoteGroups.Add(290)
        remoteGroups.Add(208)
        remoteGroups.Add(379)
        remoteGroups.Add(544)
        remoteGroups.Add(641)
        remoteGroups.Add(640)
        remoteGroups.Add(639)
        fleetGroups.Add(316)
        fleetSkills.Add("Armored Warfare")
        fleetSkills.Add("Information Warfare")
        fleetSkills.Add("Leadership")
        fleetSkills.Add("Mining Foreman")
        fleetSkills.Add("Siege Warfare")
        fleetSkills.Add("Skirmish Warfare")
        Call RedrawFleetList()

    End Sub

#End Region

#Region "Fleet List Routines"

    Private Sub btnNewFleet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewFleet.Click

        ' Create the form and display
        Dim NewFleetForm As New frmFleetName
        NewFleetForm.ShowDialog()

        ' Create the fleet if applicable
        If NewFleetForm.FleetName <> "" And NewFleetForm.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim newFleet As New FleetManager.Fleet
            newFleet.Name = NewFleetForm.FleetName
            newFleet.Commander = ""
            Dim newWing As New FleetManager.Wing
            newWing.Name = "Wing 1"
            newWing.Commander = ""
            Dim newSquad As New FleetManager.Squad
            newSquad.Name = "Squad 1"
            newSquad.Commander = ""
            newWing.Squads.Add(newSquad.Name, newSquad)
            newFleet.Wings.Add(newWing.Name, newWing)
            FleetManager.FleetCollection.Add(newFleet.Name, newFleet)
            activeFleet = FleetManager.FleetCollection(newFleet.Name)
            Call Me.RedrawFleetList()
        End If

        ' Dispose of the form
        NewFleetForm.Dispose()

    End Sub

    Private Sub RedrawFleetList()
        ' Redraw the list of fleets
        clvFleetList.BeginUpdate()
        clvFleetList.Items.Clear()
        cboFleet.BeginUpdate()
        cboFleet.Items.Clear()
        For Each fleetName As String In FleetManager.FleetCollection.Keys
            Dim newFleet As New ContainerListViewItem
            newFleet.Text = fleetName
            clvFleetList.Items.Add(newFleet)
            cboFleet.Items.Add(fleetName)
        Next
        clvFleetList.EndUpdate()
        cboFleet.EndUpdate()
    End Sub

    Private Sub clvFleetList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles clvFleetList.DoubleClick
        If clvFleetList.SelectedItems.Count > 0 Then
            Dim selFleet As ContainerListViewItem = clvFleetList.SelectedItems(0)
            If cboFleet.Items.Contains(selFleet.Text) = True Then
                cboFleet.SelectedItem = selFleet.Text
                tabFM.SelectedTab = tabFleetStructure
            End If
        End If
    End Sub

    Private Sub clvFleetList_SelectedItemsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clvFleetList.SelectedItemsChanged
        If clvFleetList.SelectedItems.Count > 0 Then
            Dim selFleet As ContainerListViewItem = clvFleetList.SelectedItems(0)
            gbFleetSettings.Text = "Fleet Settings - " & selFleet.Text
            gbFleetSettings.Tag = selFleet.Text
            gbFleetSettings.Enabled = True
            If FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHEffect = "" Then
                cboWHEffect.SelectedIndex = -1
            Else
                If cboWHEffect.Items.Contains(FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHEffect) Then
                    cboWHEffect.SelectedItem = FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHEffect
                End If
            End If
            If FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHClass = "" Then
                cboWHClass.SelectedIndex = -1
            Else
                If cboWHClass.Items.Contains(FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHClass) Then
                    cboWHClass.SelectedItem = FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHClass
                End If
            End If
        End If
    End Sub

    Private Sub btnSaveFleet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveFleet.Click
        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFFleets.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, FleetManager.FleetCollection)
        s.Flush()
        s.Close()
    End Sub

    Private Sub btnClearFleet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearFleet.Click
        FleetManager.FleetCollection.Clear()
        Call Me.RedrawFleetList()
        activeFleet = Nothing
        Call Me.RedrawFleetStructure()
    End Sub

    Private Sub btnLoadFleet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadFleet.Click
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFFleets.bin")) = True Then
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFFleets.bin"), FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            FleetManager.FleetCollection = CType(f.Deserialize(s), SortedList(Of String, FleetManager.Fleet))
            s.Close()
        End If
        Call FleetManager.CheckAllFittings()
        Call Me.RedrawFleetList()
        clvPilots.Items.Clear()
        clvFleetStructure.Items.Clear()
        lblViewingFleet.Text = "Viewing Fleet: None"
        ' DeActivate the buttons
        btnAddPilot.Enabled = False
        btnEditPilot.Enabled = False
        btnDeletePilot.Enabled = False
        btnShipAudit.Enabled = False
        btnClearAssignments.Enabled = False
        btnUpdateFleet.Enabled = False
    End Sub

    Private Sub cboWHEffect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHEffect.SelectedIndexChanged
        If gbFleetSettings.Tag IsNot Nothing Then
            If cboWHEffect.SelectedIndex <> -1 Then
                FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHEffect = cboWHEffect.SelectedItem.ToString
            End If
        End If
    End Sub

    Private Sub cboWHClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHClass.SelectedIndexChanged
        If gbFleetSettings.Tag IsNot Nothing Then
            If cboWHClass.SelectedIndex <> -1 Then
                FleetManager.FleetCollection(gbFleetSettings.Tag.ToString).WHClass = cboWHClass.SelectedItem.ToString
            End If
        End If
    End Sub

    Private Sub cboFleet_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFleet.SelectedIndexChanged
        activeFleet = FleetManager.FleetCollection(cboFleet.SelectedItem.ToString)
        If activeFleet.RemoteReceiving Is Nothing Then
            activeFleet.RemoteReceiving = New SortedList(Of String, ArrayList)
        End If
        If activeFleet.RemoteGiving Is Nothing Then
            activeFleet.RemoteGiving = New SortedList(Of String, ArrayList)
        End If
        ' Reset the open items
        Me.OpenFleet.Clear()
        Me.OpenSquads.Clear()
        Me.OpenWings.Clear()
        ' Reset Ships
        Me.BaseFleetShips.Clear()
        Me.FinalFleetShips.Clear()
        ' Redraw remote modules
        clvPilots.Items.Clear()
        ' Redraw the pilot list
        Call Me.RedrawPilotList()
        ' Redraw the fleet structure
        Call Me.RedrawFleetStructure()
        ' Activate the buttons
        btnAddPilot.Enabled = True
        btnEditPilot.Enabled = True
        btnDeletePilot.Enabled = True
        btnShipAudit.Enabled = True
        btnClearAssignments.Enabled = True
        btnUpdateFleet.Enabled = True
    End Sub

#End Region

#Region "Fleet Structure Routines"

    Private Sub RedrawFleetStructure()

        ' Clear the current structure
        clvFleetStructure.BeginUpdate()
        clvFleetStructure.Items.Clear()
        activeFleetMembers.Clear()

        If activeFleet IsNot Nothing Then

            ' Initialise the Fleet Node
            Dim newFleet As New ContainerListViewItem
            newFleet.Text = activeFleet.Name
            newFleet.Tag = activeFleet.Name

            If activeFleet.Commander <> "" Then
                newFleet.Text &= " (" & activeFleet.Commander & ")"
                Dim newFM As New FleetManager.FleetMember
                newFM.Name = activeFleet.Commander
                newFM.IsFC = True
                activeFleetMembers.Add(activeFleet.Commander, newFM)
            Else
                newFleet.Text &= " (No Commander)"
            End If
            clvFleetStructure.Items.Add(newFleet)
            newFleet.SubItems(1).Text = CheckForBooster(activeFleet.Commander)
            Call Me.DisplayShipInfo(newFleet, activeFleet.Commander)
            If OpenFleet.Contains(activeFleet.Name) = True Or ExpandAll = True Then
                newFleet.Expand()
            End If
            For Each myWing As FleetManager.Wing In activeFleet.Wings.Values
                Dim newWing As New ContainerListViewItem
                newWing.Text = myWing.Name
                newWing.Tag = myWing.Name
                If myWing.Commander <> "" Then
                    newWing.Text &= " (" & myWing.Commander & ")"
                    Dim newFM As New FleetManager.FleetMember
                    newFM.Name = myWing.Commander
                    newFM.IsWC = True
                    newFM.WingName = myWing.Name
                    activeFleetMembers.Add(newFM.Name, newFM)
                Else
                    newWing.Text &= " (No Commander)"
                End If
                newFleet.Items.Add(newWing)
                newWing.SubItems(1).Text = CheckForBooster(myWing.Commander)
                Call Me.DisplayShipInfo(newWing, myWing.Commander)
                If OpenWings.Contains(myWing.Name) = True Or ExpandAll = True Then
                    newWing.Expand()
                End If
                For Each mySquad As FleetManager.Squad In myWing.Squads.Values
                    Dim newSquad As New ContainerListViewItem
                    newSquad.Text = mySquad.Name
                    newSquad.Tag = mySquad.Name
                    If mySquad.Commander <> "" Then
                        newSquad.Text &= " (" & mySquad.Commander & ")"
                        Dim newFM As New FleetManager.FleetMember
                        newFM.Name = mySquad.Commander
                        newFM.IsSC = True
                        newFM.WingName = myWing.Name
                        newFM.SquadName = mySquad.Name
                        activeFleetMembers.Add(newFM.Name, newFM)
                    Else
                        newSquad.Text &= " (No Commander)"
                    End If
                    newWing.Items.Add(newSquad)
                    newSquad.SubItems(1).Text = CheckForBooster(mySquad.Commander)
                    Call Me.DisplayShipInfo(newSquad, mySquad.Commander)
                    If OpenSquads.Contains(mySquad.Name) = True Or ExpandAll = True Then
                        newSquad.Expand()
                    End If
                    For Each myMember As String In mySquad.Members.Values
                        Dim newMember As New ContainerListViewItem
                        newMember.Text = myMember
                        newMember.Tag = myMember
                        newSquad.Items.Add(newMember)
                        Dim newFM As New FleetManager.FleetMember
                        newFM.Name = myMember
                        newFM.WingName = myWing.Name
                        newFM.SquadName = mySquad.Name
                        activeFleetMembers.Add(newFM.Name, newFM)
                        newMember.SubItems(1).Text = CheckForBooster(myMember)
                        Call Me.DisplayShipInfo(newMember, myMember)
                    Next
                Next
            Next
            lblViewingFleet.Text = "Viewing Fleet: " & activeFleet.Name
        End If

        ' End the update of the structure
        clvFleetStructure.EndUpdate()
        ExpandAll = False

    End Sub

    Private Sub DisplayShipInfo(ByVal selItem As ContainerListViewItem, ByVal pilotName As String)
        If pilotName <> "" Then
            If FinalFleetShips.ContainsKey(pilotName) = True Then
                Dim pilotShip As Ship = FinalFleetShips(pilotName)
                selItem.SubItems(2).Text = pilotShip.EffectiveHP.ToString("N0")
                selItem.SubItems(3).Text = pilotShip.TotalDPS.ToString("N2")
                selItem.SubItems(4).Text = CDbl(pilotShip.Attributes("10062")).ToString("N2")
                selItem.SubItems(5).Text = pilotShip.ShieldCapacity.ToString("N0")
                selItem.SubItems(6).Text = pilotShip.ArmorCapacity.ToString("N0")
                selItem.SubItems(7).Text = pilotShip.MaxVelocity.ToString("N2")
                Dim ccc As Double = Engine.CalculateCapStatistics(pilotShip)
                If ccc > 0 Then
                    selItem.SubItems(8).Text = "Stable at " & (ccc / pilotShip.CapCapacity * 100).ToString("N2") & "%"
                Else
                    selItem.SubItems(8).Text = "Lasts " & EveHQ.Core.SkillFunctions.TimeToString(-ccc, False)
                End If
            End If
        End If
    End Sub

    Private Function GetPilotNameFromCLVI(ByVal selItem As ContainerListViewItem) As String
        Select Case selItem.Depth
            Case 1 ' FC
                Return activeFleet.Commander
            Case 2 ' WC
                Dim WingName As String = selItem.Tag.ToString
                Return activeFleet.Wings(WingName).Commander
            Case 3 ' SC
                Dim SquadName As String = selItem.Tag.ToString
                Dim WingName As String = selItem.ParentItem.Tag.ToString
                Return activeFleet.Wings(WingName).Squads(SquadName).Commander
            Case 4 ' SM
                Dim SquadName As String = selItem.ParentItem.Tag.ToString
                Dim WingName As String = selItem.ParentItem.ParentItem.Tag.ToString
                Return selItem.Tag.ToString
            Case Else
                Return ""
        End Select
    End Function

    Private Function CheckForBooster(ByVal PilotName As String) As String
        Dim Boosters As String = ""
        If PilotName <> "" Then
            If activeFleet.Booster = PilotName Then
                Boosters &= "F "
                activeFleetMembers(PilotName).IsFB = True
            End If
            For Each w As FleetManager.Wing In activeFleet.Wings.Values
                If w.Booster = PilotName Then
                    Boosters &= "W "
                    activeFleetMembers(PilotName).IsWB = True
                End If
                For Each s As FleetManager.Squad In w.Squads.Values
                    If s.Booster = PilotName Then
                        Boosters &= "S"
                        activeFleetMembers(PilotName).IsSB = True
                    End If
                Next
            Next
        End If
        Return Boosters
    End Function

    Private Function CheckForExistingBooster(ByVal PilotName As String) As Boolean
        ' Check if the pilot is already a booster
        If activeFleetMembers.ContainsKey(PilotName) = True Then
            Dim FM As FleetManager.FleetMember = activeFleetMembers(PilotName)
            Return (FM.IsFB Or FM.IsWB Or FM.IsSB)
        Else
            Return False
        End If
    End Function

    Private Sub clvFleetStructure_ItemCollapsed(ByVal sender As Object, ByVal e As DotNetLib.Windows.Forms.ContainerListViewEventArgs) Handles clvFleetStructure.ItemCollapsed
        Dim selItem As ContainerListViewItem = e.Item
        Select Case selItem.Depth
            Case 1
                If OpenFleet.Contains(selItem.Tag.ToString) = True Then
                    OpenFleet.Remove(selItem.Tag.ToString)
                End If
            Case 2
                If OpenWings.Contains(selItem.Tag.ToString) = True Then
                    OpenWings.Remove(selItem.Tag.ToString)
                End If
            Case 3
                If OpenSquads.Contains(selItem.Tag.ToString) = True Then
                    OpenSquads.Remove(selItem.Tag.ToString)
                End If
        End Select

    End Sub

    Private Sub clvFleetStructure_ItemExpanded(ByVal sender As Object, ByVal e As DotNetLib.Windows.Forms.ContainerListViewEventArgs) Handles clvFleetStructure.ItemExpanded
        Dim selItem As ContainerListViewItem = e.Item
        Select Case selItem.Depth
            Case 1
                If OpenFleet.Contains(selItem.Tag.ToString) = False Then
                    OpenFleet.Add(selItem.Tag.ToString)
                End If
            Case 2
                If OpenWings.Contains(selItem.Tag.ToString) = False Then
                    OpenWings.Add(selItem.Tag.ToString)
                End If
            Case 3
                If OpenSquads.Contains(selItem.Tag.ToString) = False Then
                    OpenSquads.Add(selItem.Tag.ToString)
                End If
        End Select
    End Sub

#End Region

#Region "Pilot List Routines"

    Private Sub RedrawPilotList()
        clvPilots.BeginUpdate()
        clvPilots.Items.Clear()
        If activeFleet IsNot Nothing Then
            For Each pilotName As String In activeFleet.FleetSetups.Keys
                Dim newPilot As New ContainerListViewItem
                newPilot.Text = pilotName
                clvPilots.Items.Add(newPilot)
                If OpenPilots.Contains(pilotName) = True Then
                    newPilot.Expand()
                End If
                newPilot.SubItems(1).Text = activeFleet.FleetSetups(pilotName).FittingName
                If activeFleet.FleetSetups(pilotName).IsFlyable = False Then
                    newPilot.BackColor = Color.Salmon
                Else
                    newPilot.BackColor = Color.LimeGreen
                End If
                Call Me.DisplayRemoteModules(newPilot)
                If activeFleet.RemoteReceiving.ContainsKey(pilotName) = True Then
                    For Each RA As FleetManager.RemoteAssignment In activeFleet.RemoteReceiving(pilotName)
                        Dim newRA As New ContainerListViewItem
                        newRA.Text = RA.RemotePilot
                        newPilot.Items.Add(newRA)
                        newRA.SubItems(3).Text = RA.RemoteModule
                    Next
                End If
            Next
        End If
        clvPilots.EndUpdate()
    End Sub

    Private Sub btnAddPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPilot.Click
        Dim newPilotForm As New frmFleetPilot
        newPilotForm.FleetName = activeFleet.Name
        newPilotForm.ShowDialog()
        If newPilotForm.DialogResult = Windows.Forms.DialogResult.OK Then
            Call Me.RedrawPilotList()
        End If
        newPilotForm.Dispose()
    End Sub

    Private Sub btnEditPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditPilot.Click
        If clvPilots.SelectedItems.Count > 0 Then
            Dim pilots As New ArrayList
            For Each pilot As ContainerListViewItem In clvPilots.SelectedItems
                pilots.Add(pilot.Text)
            Next
            Dim newPilotForm As New frmFleetPilot
            newPilotForm.FleetName = activeFleet.Name
            newPilotForm.PilotNames = pilots
            newPilotForm.ShowDialog()
            If newPilotForm.DialogResult = Windows.Forms.DialogResult.OK Then
                Call Me.RedrawPilotList()
            End If
            newPilotForm.Dispose()
        End If

    End Sub

    Private Sub btnDeletePilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeletePilot.Click
        If clvPilots.SelectedItems.Count > 0 Then
            Dim pilots As New ArrayList
            For Each pilot As ContainerListViewItem In clvPilots.SelectedItems
                pilots.Add(pilot.Text)
            Next
            Dim reply As Integer = MessageBox.Show("Are you sure you want to delete the selected pilots?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.Yes Then
                For Each pilotName As String In pilots
                    FleetManager.FleetCollection(activeFleet.Name).FleetSetups.Remove(pilotName)
                Next
                Call Me.RedrawPilotList()
            End If
        End If
    End Sub

    Private Sub btnShipAudit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShipAudit.Click
        If clvPilots.SelectedItems.Count = 1 Then
            Dim pilotName As String = clvPilots.SelectedItems(0).Text
            Dim fittedShip As Ship = FinalFleetShips(pilotName)
            Dim myAuditLog As New frmShipAudit
            Dim logData() As String
            Dim newLog As New ListViewItem
            myAuditLog.lvwAudit.BeginUpdate()
            For Each log As String In fittedShip.AuditLog
                logData = log.Split("#".ToCharArray)
                'If logData(2).Trim <> logData(3).Trim Then
                newLog = New ListViewItem
                newLog.Text = logData(0).Trim
                newLog.SubItems.Add(logData(1).Trim)
                newLog.SubItems.Add(FormatNumber(logData(2).Trim, 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                newLog.SubItems.Add(FormatNumber(logData(3).Trim, 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                myAuditLog.lvwAudit.Items.Add(newLog)
                'End If
            Next
            myAuditLog.lvwAudit.EndUpdate()
            myAuditLog.ShowDialog()
            myAuditLog = Nothing
        End If
    End Sub

    Private Sub DisplayRemoteModules(ByVal selItem As ContainerListViewItem)
        Dim pilotName As String = selItem.Text
        Dim shipFit As String = activeFleet.FleetSetups(pilotName).FittingName
        Dim fittingSep As Integer = shipFit.IndexOf(", ")
        Dim shipName As String = shipFit.Substring(0, fittingSep)
        Dim fittingName As String = shipFit.Substring(fittingSep + 2)
        Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(pilotName), HQFPilot)
        Dim aShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
        aShip = Engine.UpdateShipDataFromFittingList(aShip, CType(Fittings.FittingList(shipFit), ArrayList))
        aShip = Engine.CollectModules(aShip)
        ' Let's try and generate a fitting and get some module info
        For Each remoteModule As ShipModule In aShip.SlotCollection
            If remoteGroups.Contains(CInt(remoteModule.DatabaseGroup)) = True Then
                remoteModule.ModuleState = 16
                remoteModule.SlotNo = 0
                Dim newRemoteItem As New ContainerListViewItem
                newRemoteItem.Tag = remoteModule
                newRemoteItem.Text = pilotName
                selItem.Items.Add(newRemoteItem)
                newRemoteItem.SubItems(2).Text = EveHQ.Core.HQ.itemGroups(remoteModule.DatabaseGroup)
                If remoteModule.LoadedCharge IsNot Nothing Then
                    newRemoteItem.SubItems(3).Text = remoteModule.Name & " (" & remoteModule.LoadedCharge.Name & ")"
                Else
                    newRemoteItem.SubItems(3).Text = remoteModule.Name
                End If
                ' Check for assignments
                If activeFleet.RemoteGiving.ContainsKey(pilotName) = True Then
                    Dim RG As ArrayList = CType(activeFleet.RemoteGiving(pilotName).Clone, ArrayList)
                    'For Each RA As FleetManager.RemoteAssignment In RG
                    Dim RA As FleetManager.RemoteAssignment
                    For i As Integer = RG.Count - 1 To 0 Step -1
                        RA = CType(RG(i), FleetManager.RemoteAssignment)
                        If RA.RemoteModule = remoteModule.Name Then
                            newRemoteItem.SubItems(4).Text = RA.FleetPilot
                            RG.RemoveAt(i)
                        End If
                    Next
                End If
            End If
        Next
        For Each remoteDrone As DroneBayItem In aShip.DroneBayItems.Values
            If remoteGroups.Contains(CInt(remoteDrone.DroneType.DatabaseGroup)) = True Then
                If remoteDrone.IsActive = True Then
                    remoteDrone.DroneType.ModuleState = 16
                    Dim newRemoteItem As New ContainerListViewItem
                    newRemoteItem.Tag = remoteDrone
                    newRemoteItem.Text = pilotName
                    selItem.Items.Add(newRemoteItem)
                    newRemoteItem.SubItems(2).Text = EveHQ.Core.HQ.itemGroups(remoteDrone.DroneType.DatabaseGroup)
                    newRemoteItem.SubItems(3).Text = remoteDrone.DroneType.Name & " (x" & remoteDrone.Quantity & ")"
                    ' Check for assignments
                    If activeFleet.RemoteGiving.ContainsKey(pilotName) = True Then
                        Dim RG As ArrayList = CType(activeFleet.RemoteGiving(pilotName).Clone, ArrayList)
                        'For Each RA As FleetManager.RemoteAssignment In RG
                        Dim RA As FleetManager.RemoteAssignment
                        For i As Integer = RG.Count - 1 To 0 Step -1
                            RA = CType(RG(i), FleetManager.RemoteAssignment)
                            If RA.RemoteModule = remoteDrone.DroneType.Name Then
                                newRemoteItem.SubItems(4).Text = RA.FleetPilot
                                RG.RemoveAt(i)
                            End If
                        Next
                    End If
                End If
            End If
        Next

    End Sub

    Private Sub clvPilots_ItemCollapsed(ByVal sender As Object, ByVal e As DotNetLib.Windows.Forms.ContainerListViewEventArgs) Handles clvPilots.ItemCollapsed
        Dim selItem As ContainerListViewItem = e.Item
        If OpenPilots.Contains(selItem.Text) = True Then
            OpenPilots.Remove(selItem.Text)
        End If
    End Sub

    Private Sub clvPilots_ItemExpanded(ByVal sender As Object, ByVal e As DotNetLib.Windows.Forms.ContainerListViewEventArgs) Handles clvPilots.ItemExpanded
        Dim selItem As ContainerListViewItem = e.Item
        If OpenPilots.Contains(selItem.Text) = False Then
            OpenPilots.Add(selItem.Text)
        End If
    End Sub

#End Region

#Region "Drag and Drop Routines"

    Private Sub clvFleetStructure_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles clvFleetStructure.DragDrop
        ' Check for the custom DataFormat ContainerListViewItem item.
        If e.Data.GetDataPresent("System.Windows.Forms.ContainerListViewItem") Then

            Dim point1 As Point = clvFleetStructure.PointToClient(New Point(e.X, e.Y))
            Dim item1 As ContainerListViewItem = clvFleetStructure.GetItemAt(point1.Y - clvFleetStructure.HeaderHeight)
            Dim droppedItem As ContainerListViewItem = CType(e.Data.GetData("System.Windows.Forms.ContainerListViewItem"), ContainerListViewItem)
            Dim DropName As String = droppedItem.Text
            ' Check for internal restructure
            If clvFleetStructure.Tag IsNot Nothing Then
                DropName = clvFleetStructure.Tag.ToString
                clvFleetStructure.Tag = Nothing
            End If

            If item1 IsNot Nothing Then
                If Not (TypeOf droppedItem.Tag Is ShipModule) Then
                    Select Case item1.Depth
                        Case 1 ' FC
                            Dim fleetItem As ContainerListViewItem
                            fleetItem = item1
                            ' Check if the pilot is already in the fleet
                            If activeFleetMembers.ContainsKey(DropName) = True And internalReorder = False Then
                                MessageBox.Show(DropName & " is already part of the fleet.", "Already in Fleet", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                ' Check if we already have an FC
                                If activeFleet.Commander <> "" Then
                                    Dim msg As String = "The Fleet already has an active Fleet Commander. Would you like to replace " & activeFleet.Commander & " with " & DropName & "?"
                                    Dim reply As Integer = MessageBox.Show(msg, "Confirm FC Replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    If reply = DialogResult.Yes Then
                                        If internalReorder = True Then
                                            ' Need to check where the pilot came from and remove him
                                            Call RemoveMember(droppedItem)
                                        End If
                                        ' Install the new FC
                                        activeFleet.Commander = DropName
                                        ' Check boosters
                                        Call Me.CheckForExistingBoostersInFleet(DropName)
                                        ' Redraw the structure
                                        Call Me.RedrawFleetStructure()
                                    End If
                                Else
                                    If internalReorder = True Then
                                        ' Need to check where the pilot came from and remove him
                                        Call RemoveMember(droppedItem)
                                    End If
                                    ' Install the new FC
                                    activeFleet.Commander = DropName
                                    ' Check boosters
                                    Call Me.CheckForExistingBoostersInFleet(DropName)
                                    ' Redraw the structure
                                    Call Me.RedrawFleetStructure()
                                End If
                            End If
                        Case 2 ' WC
                            Dim wingItem, fleetItem As ContainerListViewItem
                            wingItem = item1
                            fleetItem = wingItem.ParentItem
                            ' Check if the pilot is already in the fleet
                            If activeFleetMembers.ContainsKey(DropName) = True And internalReorder = False Then
                                MessageBox.Show(DropName & " is already part of the fleet.", "Already in Fleet", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                ' Check if we already have an WC
                                If activeFleet.Wings(wingItem.Tag.ToString).Commander <> "" Then
                                    Dim msg As String = "This Wing already has an active Wing Commander. Would you like to replace " & activeFleet.Wings(wingItem.Tag.ToString).Commander & " with " & DropName & "?"
                                    Dim reply As Integer = MessageBox.Show(msg, "Confirm WC Replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                    If reply = DialogResult.Yes Then
                                        If internalReorder = True Then
                                            ' Need to check where the pilot came from and remove him
                                            Call RemoveMember(droppedItem)
                                        End If
                                        ' Install the new WC
                                        activeFleet.Wings(wingItem.Tag.ToString).Commander = DropName
                                        ' Check boosters
                                        Call Me.CheckForExistingBoostersInWing(DropName, activeFleet.Wings(wingItem.Tag.ToString))
                                        ' Redraw the structure
                                        Call Me.RedrawFleetStructure()
                                    End If
                                Else
                                    If internalReorder = True Then
                                        ' Need to check where the pilot came from and remove him
                                        Call RemoveMember(droppedItem)
                                    End If
                                    ' Install the new WC
                                    activeFleet.Wings(wingItem.Tag.ToString).Commander = DropName
                                    ' Check boosters
                                    Call Me.CheckForExistingBoostersInWing(DropName, activeFleet.Wings(wingItem.Tag.ToString))
                                    ' Redraw the structure
                                    Call Me.RedrawFleetStructure()
                                End If
                            End If
                        Case 3, 4 ' SC or SM
                            ' Check if this is squad header or a member
                            Dim squadItem, wingItem, fleetItem As ContainerListViewItem
                            If item1.Depth = 4 Then
                                squadItem = item1.ParentItem
                            Else
                                squadItem = item1
                            End If
                            wingItem = squadItem.ParentItem
                            fleetItem = wingItem.ParentItem

                            ' Check if the pilot is already in the fleet
                            If activeFleetMembers.ContainsKey(DropName) = True And internalReorder = False Then
                                MessageBox.Show(DropName & " is already part of the fleet.", "Already in Fleet", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                ' Check the squad limitations
                                If activeFleet.Wings(wingItem.Tag.ToString).Squads(squadItem.Tag.ToString).Members.Count < 10 Then
                                    ' Check if we have a squad commander - if not, install as SC, otherwise install as SM
                                    If activeFleet.Wings(wingItem.Tag.ToString).Squads(squadItem.Tag.ToString).Commander = "" Then
                                        ' Install as SC
                                        If internalReorder = True Then
                                            ' Need to check where the pilot came from and remove him
                                            Call RemoveMember(droppedItem)
                                        End If
                                        activeFleet.Wings(wingItem.Tag.ToString).Squads(squadItem.Tag.ToString).Commander = DropName
                                        ' Check for duplicated boosters
                                        If CheckForExistingBooster(DropName) = True Then
                                            Call Me.CheckForExistingBoostersInSquad(DropName, activeFleet.Wings(wingItem.Tag.ToString).Squads(squadItem.Tag.ToString))
                                        End If
                                        ' Redraw the structure
                                        Call Me.RedrawFleetStructure()
                                    Else
                                        ' Install as SM
                                        If internalReorder = True Then
                                            ' Need to check where the pilot came from and remove him
                                            Call RemoveMember(droppedItem)
                                        End If
                                        activeFleet.Wings(wingItem.Tag.ToString).Squads(squadItem.Tag.ToString).Members.Add(DropName, DropName)
                                        ' Check for duplicated boosters
                                        If CheckForExistingBooster(DropName) = True Then
                                            Call Me.CheckForExistingBoostersInSquad(DropName, activeFleet.Wings(wingItem.Tag.ToString).Squads(squadItem.Tag.ToString))
                                        End If
                                        ' Redraw the structure
                                        Call Me.RedrawFleetStructure()
                                    End If
                                Else
                                    MessageBox.Show("You cannot exceed a squad size of 10", "Squad Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                End If
                            End If
                    End Select
                Else
                    ' This is if we drop a module
                    Dim targetPilot As String = Me.GetPilotNameFromCLVI(item1)
                    If item1.Depth >= 1 Then
                        If targetPilot <> droppedItem.Text And targetPilot <> "" Then
                            ' Check for being already assigned
                            If droppedItem.SubItems(4).Text <> targetPilot Then
                                If activeFleet.RemoteReceiving.ContainsKey(targetPilot) = False Then
                                    activeFleet.RemoteReceiving.Add(targetPilot, New ArrayList)
                                End If
                                If activeFleet.RemoteGiving.ContainsKey(droppedItem.Text) = False Then
                                    activeFleet.RemoteGiving.Add(droppedItem.Text, New ArrayList)
                                End If
                                ' Set the receiving modules
                                Dim newRR As New FleetManager.RemoteAssignment
                                newRR.FleetPilot = targetPilot
                                newRR.RemotePilot = droppedItem.Text
                                newRR.RemoteModule = CType(droppedItem.Tag, ShipModule).Name
                                activeFleet.RemoteReceiving(targetPilot).Add(newRR)
                                ' Set the giving modules
                                Dim newRG As New FleetManager.RemoteAssignment
                                newRR.FleetPilot = targetPilot
                                newRR.RemotePilot = droppedItem.Text
                                newRR.RemoteModule = CType(droppedItem.Tag, ShipModule).Name
                                activeFleet.RemoteGiving(droppedItem.Text).Add(newRR)
                                droppedItem.SubItems(4).Text = targetPilot
                                ' Redraw the pilot list
                                Call Me.RedrawPilotList()
                            End If
                        End If
                    End If
                End If
            End If
        End If

        ' Reset the internalReorder flag ready for the next drag/drop operation
        internalReorder = False

    End Sub

    Private Sub CheckForExistingBoostersInFleet(ByVal PilotName As String)
        If activeFleetMembers.ContainsKey(PilotName) = True Then
            Dim Pilot As FleetManager.FleetMember = activeFleetMembers(PilotName)
            If activeFleet.Commander = PilotName Then
                If Pilot.IsSB Then
                    Pilot.IsSB = False
                    activeFleet.Wings(Pilot.WingName).Squads(Pilot.SquadName).Booster = ""
                    Exit Sub
                End If
                If Pilot.IsWB Then
                    Pilot.IsWB = False
                    activeFleet.Wings(Pilot.WingName).Booster = ""
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub CheckForExistingBoostersInWing(ByVal PilotName As String, ByVal CheckWing As FleetManager.Wing)
        If activeFleetMembers.ContainsKey(PilotName) = True Then
            Dim Pilot As FleetManager.FleetMember = activeFleetMembers(PilotName)
            If CheckWing.Commander = PilotName Then
                If Pilot.IsSB Then
                    Pilot.IsSB = False
                    activeFleet.Wings(Pilot.WingName).Squads(Pilot.SquadName).Booster = ""
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub CheckForExistingBoostersInSquad(ByVal PilotName As String, ByVal CheckSquad As FleetManager.Squad)
        If activeFleetMembers.ContainsKey(PilotName) = True Then
            Dim Pilot As FleetManager.FleetMember = activeFleetMembers(PilotName)
            If CheckSquad.Commander <> "" Then
                Dim Comm As FleetManager.FleetMember = activeFleetMembers(CheckSquad.Commander)
                If Pilot.Name <> Comm.Name Then
                    If Pilot.IsFB And Comm.IsFB Then
                        Pilot.IsFB = False
                        Exit Sub
                    End If
                    If Pilot.IsWB And Comm.IsWB Then
                        Pilot.IsWB = False
                        activeFleet.Wings(Pilot.WingName).Booster = ""
                        Exit Sub
                    End If
                    If Pilot.IsSB And Comm.IsSB Then
                        Pilot.IsSB = False
                        activeFleet.Wings(Pilot.WingName).Squads(Pilot.SquadName).Booster = ""
                        Exit Sub
                    End If
                End If
            End If
            For Each checkPilot As String In CheckSquad.Members.Keys
                If checkPilot <> PilotName Then
                    If Pilot.IsFB And activeFleetMembers(checkPilot).IsFB Then
                        Pilot.IsFB = False
                        Exit Sub
                    End If
                    If Pilot.IsWB And activeFleetMembers(checkPilot).IsWB Then
                        Pilot.IsWB = False
                        activeFleet.Wings(Pilot.WingName).Booster = ""
                        Exit Sub
                    End If
                    If Pilot.IsSB And activeFleetMembers(checkPilot).IsSB Then
                        Pilot.IsSB = False
                        activeFleet.Wings(Pilot.WingName).Squads(Pilot.SquadName).Booster = ""
                        Exit Sub
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub clvFleetStructure_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles clvFleetStructure.ItemDrag
        Dim myItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        ' restate the text as the pilot name
        If myItem.Tag.ToString <> myItem.Text Then
            Dim myName As String = myItem.Text.TrimStart(myItem.Tag.ToString.ToCharArray)
            myName = myName.TrimStart(" (".ToCharArray).TrimEnd(")".ToCharArray)
            If myName <> "No Commander" Then
                clvFleetStructure.Tag = myName
                internalReorder = True
                clvFleetStructure.DoDragDrop(New DataObject("System.Windows.Forms.ContainerListViewItem", myItem), DragDropEffects.Move)
            End If
        Else
            Dim myName As String = myItem.Tag.ToString
            clvFleetStructure.Tag = myName
            internalReorder = True
            clvFleetStructure.DoDragDrop(New DataObject("System.Windows.Forms.ContainerListViewItem", myItem), DragDropEffects.Move)
        End If
    End Sub

    Private Sub clvFleetStructure_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles clvFleetStructure.DragOver
        ' Check for the ContainerListViewitem object.
        If e.Data.GetDataPresent("System.Windows.Forms.ContainerListViewItem") Then

            Dim point1 As Point = clvFleetStructure.PointToClient(New Point(e.X, e.Y))
            Dim item1 As ContainerListViewItem = clvFleetStructure.GetItemAt(point1.Y - clvFleetStructure.HeaderHeight + clvFleetStructure.VerticalScrollOffset)
            Dim droppedItem As ContainerListViewItem = CType(e.Data.GetData("System.Windows.Forms.ContainerListViewItem"), ContainerListViewItem)

            If item1 IsNot Nothing Then
                If Not (TypeOf droppedItem.Tag Is ShipModule) Then
                    If item1.Depth >= 1 Then
                        Dim targetPilot As String = Me.GetPilotNameFromCLVI(item1)
                        Dim droppedPilot As String = ""
                        If droppedItem.ListView.Name = CType(sender, ContainerListView).Name Then
                            droppedPilot = Me.GetPilotNameFromCLVI(droppedItem)
                        Else
                            droppedPilot = droppedItem.Text
                        End If
                        If droppedPilot <> targetPilot Then ' Can't drop if same person
                            clvFleetStructure.SelectedItems.Clear()
                            item1.Selected = True
                            clvFleetStructure.Invalidate()
                            e.Effect = DragDropEffects.Move
                        Else
                            clvFleetStructure.SelectedItems.Clear()
                            clvFleetStructure.Invalidate()
                            e.Effect = DragDropEffects.None
                        End If
                    Else
                        clvFleetStructure.SelectedItems.Clear()
                        clvFleetStructure.Invalidate()
                        e.Effect = DragDropEffects.None
                    End If
                Else
                    ' Assume we want to assign a ship module to a target person
                    Dim targetPilot As String = Me.GetPilotNameFromCLVI(item1)
                    If item1.Depth >= 1 Then
                        If targetPilot <> droppedItem.Text And targetPilot <> "" Then  ' Can't drop module onto the same person!
                            If droppedItem.SubItems(4).Text <> targetPilot Then ' Can't drop module if already assigned!
                                clvFleetStructure.SelectedItems.Clear()
                                item1.Selected = True
                                clvFleetStructure.Invalidate()
                                e.Effect = DragDropEffects.Move
                            Else
                                clvFleetStructure.SelectedItems.Clear()
                                clvFleetStructure.Invalidate()
                                e.Effect = DragDropEffects.None
                            End If
                        Else
                            clvFleetStructure.SelectedItems.Clear()
                            clvFleetStructure.Invalidate()
                            e.Effect = DragDropEffects.None
                        End If
                    Else
                        clvFleetStructure.SelectedItems.Clear()
                        clvFleetStructure.Invalidate()
                        e.Effect = DragDropEffects.None
                    End If
                End If
            Else
                clvFleetStructure.SelectedItems.Clear()
                clvFleetStructure.Invalidate()
                e.Effect = DragDropEffects.None
            End If
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub clvPilots_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles clvPilots.DragDrop
        ' Check for the ContainerListViewitem object.
        If e.Data.GetDataPresent("System.Windows.Forms.ContainerListViewItem") Then

            Dim droppedItem As ContainerListViewItem = CType(e.Data.GetData("System.Windows.Forms.ContainerListViewItem"), ContainerListViewItem)
            Dim point1 As Point = clvPilots.PointToClient(New Point(e.X, e.Y))
            Dim item1 As ContainerListViewItem = clvPilots.GetItemAt(point1.Y - clvPilots.HeaderHeight)

            If item1 IsNot Nothing Then
                If TypeOf droppedItem.Tag Is ShipModule Then
                    If item1.Depth = 1 Then
                        If item1.Text <> droppedItem.Text Then
                            ' Check for being already assigned
                            If droppedItem.SubItems(4).Text <> item1.Text Then
                                If activeFleet.RemoteReceiving.ContainsKey(item1.Text) = False Then
                                    activeFleet.RemoteReceiving.Add(item1.Text, New ArrayList)
                                End If
                                If activeFleet.RemoteGiving.ContainsKey(droppedItem.Text) = False Then
                                    activeFleet.RemoteGiving.Add(droppedItem.Text, New ArrayList)
                                End If
                                ' Set the receiving modules
                                Dim newRR As New FleetManager.RemoteAssignment
                                newRR.FleetPilot = item1.Text
                                newRR.RemotePilot = droppedItem.Text
                                newRR.RemoteModule = CType(droppedItem.Tag, ShipModule).Name
                                activeFleet.RemoteReceiving(item1.Text).Add(newRR)
                                ' Set the giving modules
                                Dim newRG As New FleetManager.RemoteAssignment
                                newRR.FleetPilot = item1.Text
                                newRR.RemotePilot = droppedItem.Text
                                newRR.RemoteModule = CType(droppedItem.Tag, ShipModule).Name
                                activeFleet.RemoteGiving(droppedItem.Text).Add(newRR)
                                droppedItem.SubItems(4).Text = item1.Text
                                ' Redraw the pilot list
                                Call Me.RedrawPilotList()
                            End If
                        End If
                    End If
                End If
            End If
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub clvPilots_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles clvPilots.DragOver
        ' Check for the ContainerListViewitem object.
        If e.Data.GetDataPresent("System.Windows.Forms.ContainerListViewItem") Then

            Dim droppedItem As ContainerListViewItem = CType(e.Data.GetData("System.Windows.Forms.ContainerListViewItem"), ContainerListViewItem)
            Dim scrolloffset As Integer = clvPilots.AutoScrollOffset.Y
            Dim point1 As Point = clvPilots.PointToClient(New Point(e.X, e.Y))
            Dim item1 As ContainerListViewItem = clvPilots.GetItemAt(point1.Y - clvPilots.HeaderHeight + clvPilots.VerticalScrollOffset)

            If item1 IsNot Nothing Then
                If TypeOf droppedItem.Tag Is ShipModule Then
                    If item1.Depth = 1 Then
                        If item1.Text <> droppedItem.Text Then  ' Can't drop module onto the same person!
                            If droppedItem.SubItems(4).Text <> item1.Text Then ' Can't drop module if already assigned!
                                clvPilots.SelectedItems.Clear()
                                item1.Selected = True
                                clvPilots.Invalidate()
                                e.Effect = DragDropEffects.Move
                            Else
                                clvPilots.SelectedItems.Clear()
                                clvPilots.Invalidate()
                                e.Effect = DragDropEffects.None
                            End If
                        Else
                            clvPilots.SelectedItems.Clear()
                            clvPilots.Invalidate()
                            e.Effect = DragDropEffects.None
                        End If
                    Else
                        clvPilots.SelectedItems.Clear()
                        clvPilots.Invalidate()
                        e.Effect = DragDropEffects.None
                    End If
                Else
                    clvPilots.SelectedItems.Clear()
                    clvPilots.Invalidate()
                    e.Effect = DragDropEffects.None
                End If
            Else
                clvPilots.SelectedItems.Clear()
                clvPilots.Invalidate()
                e.Effect = DragDropEffects.None
            End If
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub clvPilots_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles clvPilots.ItemDrag
        Dim myItem As ContainerListViewItem = clvPilots.SelectedItems(0)
        ' Create a DataObject containg the ContainerListViewItem
        If myItem.Depth > 1 Then
            If myItem.Text = myItem.ParentItem.Text Then
                clvPilots.DoDragDrop(New DataObject("System.Windows.Forms.ContainerListViewItem", myItem), DragDropEffects.Move)
            End If
        Else
            clvPilots.DoDragDrop(New DataObject("System.Windows.Forms.ContainerListViewItem", myItem), DragDropEffects.Move)
        End If
    End Sub

   
#End Region

#Region "Fleet Structure Context Menu Item Routines"
    Private Sub ctxFleetStructure_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxFleetStructure.Opening

        ' Get the selected item

        If clvFleetStructure.SelectedItems.Count > 0 Then
            Dim selItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)

            ' Clear the existing items
            ctxFleetStructure.Items.Clear()

            ' Add a Create Item
            Dim mnuCreate As New ToolStripMenuItem
            mnuCreate.Text = "Create Wing"
            AddHandler mnuCreate.Click, AddressOf Me.FSCreate
            ctxFleetStructure.Items.Add(mnuCreate)

            ' Add a Delete Item
            Dim mnuDelete As New ToolStripMenuItem
            mnuDelete.Text = "Delete Wing"
            AddHandler mnuDelete.Click, AddressOf Me.FSDelete
            ctxFleetStructure.Items.Add(mnuDelete)

            ' Add a Rename Item
            Dim mnuRename As New ToolStripMenuItem
            mnuRename.Text = "Rename Fleet"
            AddHandler mnuRename.Click, AddressOf Me.FSRename
            ctxFleetStructure.Items.Add(mnuRename)

            ' Add a Sep
            Dim mnuSep1 As New ToolStripSeparator
            ctxFleetStructure.Items.Add(mnuSep1)

            ' Add a Revoke Booster Item
            Dim mnuRevokeBooster As New ToolStripMenuItem
            mnuRevokeBooster.Text = "Revoke Booster"
            AddHandler mnuRevokeBooster.Click, AddressOf Me.FSRevokeBooster
            ctxFleetStructure.Items.Add(mnuRevokeBooster)

            ' Add a Set Fleet Booster Item
            Dim mnuSetFleetBooster As New ToolStripMenuItem
            mnuSetFleetBooster.Text = "Set Fleet Booster"
            AddHandler mnuSetFleetBooster.Click, AddressOf Me.FSSetFleetBooster
            ctxFleetStructure.Items.Add(mnuSetFleetBooster)

            ' Add a Set Wing Booster Item
            Dim mnuSetWingBooster As New ToolStripMenuItem
            mnuSetWingBooster.Text = "Set Wing Booster"
            AddHandler mnuSetWingBooster.Click, AddressOf Me.FSSetWingBooster
            ctxFleetStructure.Items.Add(mnuSetWingBooster)

            ' Add a Set Squad Booster Item
            Dim mnuSetSquadBooster As New ToolStripMenuItem
            mnuSetSquadBooster.Text = "Set Squad Booster"
            AddHandler mnuSetSquadBooster.Click, AddressOf Me.FSSetSquadBooster
            ctxFleetStructure.Items.Add(mnuSetSquadBooster)

            ' Add a Sep
            Dim mnuSep2 As New ToolStripSeparator
            ctxFleetStructure.Items.Add(mnuSep2)

            ' Add a Leave Fleet Item
            Dim mnuLeaveFleet As New ToolStripMenuItem
            mnuLeaveFleet.Text = "Leave Fleet"
            AddHandler mnuLeaveFleet.Click, AddressOf Me.FSLeaveFleet
            ctxFleetStructure.Items.Add(mnuLeaveFleet)

            ' Add a Move Member Item
            Dim mnuMoveMember As New ToolStripMenuItem
            mnuMoveMember.Text = "Move Member"
            AddHandler mnuMoveMember.Click, AddressOf Me.FSMoveMember
            ctxFleetStructure.Items.Add(mnuMoveMember)

            ' Add a Sep
            Dim mnuSep3 As New ToolStripSeparator
            ctxFleetStructure.Items.Add(mnuSep3)

            ' Add a Leave Fleet Item
            Dim mnuExpandAll As New ToolStripMenuItem
            mnuExpandAll.Text = "Expand All"
            AddHandler mnuExpandAll.Click, AddressOf Me.FSExpandAll
            ctxFleetStructure.Items.Add(mnuExpandAll)

            ' Add a Leave Fleet Item
            Dim mnuCollapseAll As New ToolStripMenuItem
            mnuCollapseAll.Text = "Collapse All"
            AddHandler mnuCollapseAll.Click, AddressOf Me.FSCollapseAll
            ctxFleetStructure.Items.Add(mnuCollapseAll)

            ' Customise menu for Fleet/Wing/Squad
            Select Case selItem.Depth
                Case 1 ' Fleet
                    mnuDelete.Visible = False
                    mnuSetWingBooster.Visible = False
                    mnuSetSquadBooster.Visible = False
                    If activeFleet.Commander = "" Then
                        mnuRevokeBooster.Visible = False
                        mnuSetFleetBooster.Visible = False
                        mnuLeaveFleet.Visible = False
                        mnuMoveMember.Visible = False
                        mnuSep1.Visible = False
                        mnuSep2.Visible = False
                    Else
                        ' Get details
                        Dim pilotName As String = activeFleet.Commander
                        Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
                        If FM.IsFB Or FM.IsWB Or FM.IsSB Then
                            mnuSetFleetBooster.Visible = False
                        Else
                            mnuRevokeBooster.Visible = False
                        End If
                    End If
                Case 2 ' Wing
                    mnuCreate.Text = "Create Squad"
                    mnuRename.Text = "Rename Wing"
                    mnuSetSquadBooster.Visible = False
                    If activeFleet.Wings(selItem.Tag.ToString).Commander = "" Then
                        mnuRevokeBooster.Visible = False
                        mnuSetFleetBooster.Visible = False
                        mnuSetWingBooster.Visible = False
                        mnuLeaveFleet.Visible = False
                        mnuMoveMember.Visible = False
                        mnuSep1.Visible = False
                        mnuSep2.Visible = False
                    Else
                        ' Get details
                        Dim pilotName As String = activeFleet.Wings(selItem.Tag.ToString).Commander
                        Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
                        If FM.IsFB Or FM.IsWB Or FM.IsSB Then
                            mnuSetFleetBooster.Visible = False
                            mnuSetWingBooster.Visible = False
                        Else
                            mnuRevokeBooster.Visible = False
                        End If
                    End If
                Case 3 ' Squad
                    mnuCreate.Visible = False
                    mnuDelete.Text = "Delete Squad"
                    mnuRename.Text = "Rename Squad"
                    If activeFleet.Wings(selItem.ParentItem.Tag.ToString).Squads(selItem.Tag.ToString).Commander = "" Then
                        mnuRevokeBooster.Visible = False
                        mnuSetFleetBooster.Visible = False
                        mnuSetWingBooster.Visible = False
                        mnuSetSquadBooster.Visible = False
                        mnuLeaveFleet.Visible = False
                        mnuMoveMember.Visible = False
                        mnuSep1.Visible = False
                        mnuSep2.Visible = False
                    Else
                        ' Get details
                        Dim pilotName As String = activeFleet.Wings(selItem.ParentItem.Tag.ToString).Squads(selItem.Tag.ToString).Commander
                        Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
                        If FM.IsFB Or FM.IsWB Or FM.IsSB Then
                            mnuSetFleetBooster.Visible = False
                            mnuSetWingBooster.Visible = False
                            mnuSetSquadBooster.Visible = False
                        Else
                            mnuRevokeBooster.Visible = False
                        End If
                    End If
                Case 4 ' Squad Member
                    mnuCreate.Visible = False
                    mnuDelete.Visible = False
                    mnuRename.Visible = False
                    mnuSep1.Visible = False
                    Dim pilotName As String = selItem.Tag.ToString
                    Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
                    If FM.IsFB Or FM.IsWB Or FM.IsSB Then
                        mnuSetFleetBooster.Visible = False
                        mnuSetWingBooster.Visible = False
                        mnuSetSquadBooster.Visible = False
                    Else
                        mnuRevokeBooster.Visible = False
                    End If
            End Select
        Else
            e.Cancel = True
        End If

    End Sub

    Private Sub FSCreate(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        Select Case selItem.Depth
            Case 1 ' Create Wing
                ' Check for maximum wings
                If activeFleet.Wings.Count = maxWings Then
                    MessageBox.Show("You cannot create a new wing because your fleet already contains the maximum of " & maxWings.ToString & " wings.", "Maximum Wings Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                ' Create the wing
                Dim NewFleetForm As New frmFleetName
                NewFleetForm.FleetType = 2
                NewFleetForm.FleetName = activeFleet.Name
                NewFleetForm.ShowDialog()
                ' Create the wing if applicable
                If NewFleetForm.WingName <> "" And NewFleetForm.DialogResult = Windows.Forms.DialogResult.OK Then
                    Dim newWing As New FleetManager.Wing
                    newWing.Name = NewFleetForm.WingName
                    newWing.Commander = ""
                    Dim newSquad As New FleetManager.Squad
                    newSquad.Name = "Squad 1"
                    newSquad.Commander = ""
                    newWing.Squads.Add(newSquad.Name, newSquad)
                    activeFleet.Wings.Add(newWing.Name, newWing)
                    Call Me.RedrawFleetList()
                    Call Me.RedrawFleetStructure()
                End If
                ' Dispose of the form
                NewFleetForm.Dispose()
            Case 2 ' Create Squad
                ' Check for maximum squads
                If activeFleet.Wings(selItem.Tag.ToString).Squads.Count = maxSquads Then
                    MessageBox.Show("You cannot create a new squad in this wing because it already contains the maximum of " & maxSquads.ToString & " squads.", "Maximum Squads Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
                ' Create the squad
                Dim NewFleetForm As New frmFleetName
                NewFleetForm.FleetType = 3
                NewFleetForm.FleetName = activeFleet.Name
                NewFleetForm.WingName = selItem.Tag.ToString
                NewFleetForm.ShowDialog()
                ' Create the wing if applicable
                If NewFleetForm.SquadName <> "" And NewFleetForm.DialogResult = Windows.Forms.DialogResult.OK Then
                    Dim newSquad As New FleetManager.Squad
                    newSquad.Name = NewFleetForm.SquadName
                    newSquad.Commander = ""
                    activeFleet.Wings(NewFleetForm.WingName).Squads.Add(newSquad.Name, newSquad)
                    Call Me.RedrawFleetList()
                    Call Me.RedrawFleetStructure()
                End If
                ' Dispose of the form
                NewFleetForm.Dispose()
        End Select
    End Sub

    Private Sub FSDelete(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FSRename(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FSRevokeBooster(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        Dim pilotName As String = GetPilotNameFromCLVI(selItem)
        Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
        If FM.IsFB Then
            FM.IsFB = False
            activeFleet.Booster = ""
        End If
        If FM.IsWB = True Then
            FM.IsWB = False
            activeFleet.Wings(FM.WingName).Booster = ""
        End If
        If FM.IsSB = True Then
            FM.IsSB = False
            activeFleet.Wings(FM.WingName).Squads(FM.SquadName).Booster = ""
        End If
        ' Redraw the structure
        Call Me.RedrawFleetStructure()
    End Sub

    Private Sub FSSetFleetBooster(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        Dim pilotName As String = GetPilotNameFromCLVI(selItem)
        If CheckForExistingBooster(pilotName) = False Then
            activeFleet.Booster = pilotName
            ' Redraw the structure
            Call Me.RedrawFleetStructure()
        End If
    End Sub

    Private Sub FSSetWingBooster(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        Dim pilotName As String = GetPilotNameFromCLVI(selItem)
        If CheckForExistingBooster(pilotName) = False Then
            Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
            activeFleet.Wings(FM.WingName).Booster = pilotName
            ' Redraw the structure
            Call Me.RedrawFleetStructure()
        End If
    End Sub

    Private Sub FSSetSquadBooster(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim selItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        Dim pilotName As String = GetPilotNameFromCLVI(selItem)
        If CheckForExistingBooster(pilotName) = False Then
            Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
            activeFleet.Wings(FM.WingName).Squads(FM.SquadName).Booster = pilotName
            ' Redraw the structure
            Call Me.RedrawFleetStructure()
        End If
    End Sub

    Private Sub FSLeaveFleet(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Remove a member from the fleet
        Dim selItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        Call Me.RemoveMember(selItem)
        ' Redraw the structure
        Call Me.RedrawFleetStructure()
    End Sub

    Private Sub FSMoveMember(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FSExpandAll(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Set the expand flag
        Me.ExpandAll = True
        ' Redraw the fleet structure
        Call Me.RedrawFleetStructure()
    End Sub

    Private Sub FSCollapseAll(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Reset the open items
        Me.OpenFleet.Clear()
        Me.OpenSquads.Clear()
        Me.OpenWings.Clear()
        ' Redraw the fleet structure
        Call Me.RedrawFleetStructure()
    End Sub

    Private Sub RemoveMember(ByVal selItem As ContainerListViewItem)
        Select Case selItem.Depth
            Case 1 ' FC
                activeFleet.Commander = ""
            Case 2 ' WC
                Dim WingName As String = selItem.Tag.ToString
                activeFleet.Wings(WingName).Commander = ""
            Case 3 ' SC
                Dim SquadName As String = selItem.Tag.ToString
                Dim WingName As String = selItem.ParentItem.Tag.ToString
                activeFleet.Wings(WingName).Squads(SquadName).Commander = ""
            Case 4 ' SM
                Dim SquadName As String = selItem.ParentItem.Tag.ToString
                Dim WingName As String = selItem.ParentItem.ParentItem.Tag.ToString
                activeFleet.Wings(WingName).Squads(SquadName).Members.Remove(selItem.Tag.ToString)
                'activeFleetMembers.Remove(selItem.Tag.ToString)
        End Select
    End Sub

#End Region

#Region "Fleet Update Routines"

    Private Sub btnUpdateFleet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateFleet.Click
        ' Change the cursor...it could be a long calculation!
        Me.Cursor = Cursors.WaitCursor
        For Each pilotName As String In activeFleet.FleetSetups.Keys
            If activeFleetMembers.ContainsKey(pilotName) = True Then
                Dim shipFit As String = activeFleet.FleetSetups(pilotName).FittingName
                Dim fittingSep As Integer = shipFit.IndexOf(", ")
                Dim shipName As String = shipFit.Substring(0, fittingSep)
                Dim fittingName As String = shipFit.Substring(fittingSep + 2)
                Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(pilotName), HQFPilot)
                Dim aShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
                aShip = Engine.UpdateShipDataFromFittingList(aShip, CType(Fittings.FittingList(shipFit), ArrayList))
                aShip.DamageProfile = CType(DamageProfiles.ProfileList("<Omni-Damage>"), DamageProfile)
                ' Add the WH Environmental Affects to each ship
                Call Me.AddWHEffects(aShip)
                ' Apply the final fitting
                BaseFleetShips(pilotName) = Engine.ApplyFitting(aShip, aPilot)
            End If
        Next
        ' Establish fleet, wing and squad boosters for this pilot
        For Each pilotName As String In activeFleet.FleetSetups.Keys
            If activeFleetMembers.ContainsKey(pilotName) = True Then
                Dim shipFit As String = activeFleet.FleetSetups(pilotName).FittingName
                Dim fittingSep As Integer = shipFit.IndexOf(", ")
                Dim shipName As String = shipFit.Substring(0, fittingSep)
                Dim fittingName As String = shipFit.Substring(fittingSep + 2)
                Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(pilotName), HQFPilot)
                Dim aShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
                aShip = Engine.UpdateShipDataFromFittingList(aShip, CType(Fittings.FittingList(shipFit), ArrayList))
                aShip.DamageProfile = CType(DamageProfiles.ProfileList("<Omni-Damage>"), DamageProfile)
                ' Add the WH Environmental Affects to each ship
                Call Me.AddWHEffects(aShip)
                ' Display and allocate list of remote modules available for using
                Call Me.GetRemoteModules(aShip, pilotName)
                ' Calcalate the boosters
                Call Me.CalculateBoosters(aShip, pilotName)
                FinalFleetShips(pilotName) = Engine.ApplyFitting(aShip, aPilot)
            End If
        Next
        Call Me.RedrawFleetStructure()
        ' Reset the cursor
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub AddWHEffects(ByRef cShip As Ship)
        ' Clear the current effect
        cShip.EnviroSlotCollection.Clear()
        ' Set the WH Class combo if it's not activated
        If activeFleet.WHEffect <> "" And activeFleet.WHEffect <> "<None>" Then
            If activeFleet.WHClass = "" Then
                Exit Sub
            Else
                Dim modName As String = ""
                If activeFleet.WHEffect = "Red Giant" Then
                    modName = activeFleet.WHEffect & " Beacon Class " & activeFleet.WHClass
                Else
                    modName = activeFleet.WHEffect & " Effect Beacon Class " & activeFleet.WHClass
                End If
                Dim modID As String = CStr(ModuleLists.moduleListName(modName))
                Dim eMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                cShip.EnviroSlotCollection.Add(eMod)
            End If
        End If
    End Sub

    Private Sub CalculateBoosters(ByRef cShip As Ship, ByVal pilotName As String)
        Dim FM As FleetManager.FleetMember = activeFleetMembers(pilotName)
        Dim FB As String = activeFleet.Booster
        Dim WB As String = ""
        Dim SB As String = ""
        If FM.WingName <> "" Then
            WB = activeFleet.Wings(FM.WingName).Booster
        End If
        If FM.SquadName <> "" Then
            SB = activeFleet.Wings(FM.WingName).Squads(FM.SquadName).Booster
        End If

        Call Me.UpdateShipEffects(cShip, FB, WB, SB)
        Call Me.CalculateFleetEffects(cShip, FB, WB, SB)
    End Sub

    Private Sub CalculateFleetEffects(ByRef cShip As Ship, ByVal FB As String, ByVal WB As String, ByVal SB As String)
        cShip.FleetSlotCollection.Clear()
        Call Me.CalculateFleetSkillEffects(cShip, FB, WB, SB)
        Call Me.CalculateFleetModuleEffects(cShip)
    End Sub

    Private Sub UpdateShipEffects(ByRef cShip As Ship, ByVal FB As String, ByVal WB As String, ByVal SB As String)
        ' Get the SB Modules
        If SB <> "" Then
            ' Let's try and get a fitting and get some module info
            Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(SB), HQFPilot)
            Dim remoteShip As Ship = BaseFleetShips(SB)
            SBModules.Clear()
            ' Check the ship bonuses for further effects (Titans use this!)
            SBModules = GetShipGangBonusModules(remoteShip, pPilot)
            ' Check the modules for fleet effects
            For Each FleetModule As ShipModule In remoteShip.SlotCollection
                If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                    FleetModule.ModuleState = 16
                    FleetModule.SlotNo = 0
                    SBModules.Add(FleetModule)
                End If
            Next
        End If

        ' Get the WB Modules
        If WB <> "" Then
            ' Let's try and get a fitting and get some module info
            Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(WB), HQFPilot)
            Dim remoteShip As Ship = BaseFleetShips(WB)
            WBModules.Clear()
            ' Check the ship bonuses for further effects (Titans use this!)
            WBModules = GetShipGangBonusModules(remoteShip, pPilot)
            ' Check the modules for fleet effects
            For Each FleetModule As ShipModule In remoteShip.SlotCollection
                If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                    FleetModule.ModuleState = 16
                    FleetModule.SlotNo = 0
                    WBModules.Add(FleetModule)
                End If
            Next
        End If

        ' Get the FB Modules
        If FB <> "" Then
            ' Let's try and get a fitting and get some module info
            Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(FB), HQFPilot)
            Dim remoteShip As Ship = BaseFleetShips(FB)
            FBModules.Clear()
            ' Check the ship bonuses for further effects (Titans use this!)
            FBModules = GetShipGangBonusModules(remoteShip, pPilot)
            ' Check the modules for fleet effects
            For Each FleetModule As ShipModule In remoteShip.SlotCollection
                If fleetGroups.Contains(CInt(FleetModule.DatabaseGroup)) = True Then
                    FleetModule.ModuleState = 16
                    FleetModule.SlotNo = 0
                    FBModules.Add(FleetModule)
                End If
            Next
        End If
    End Sub

    Private Sub CalculateFleetSkillEffects(ByRef cShip As Ship, ByVal FB As String, ByVal WB As String, ByVal SB As String)

        ' Add in the commander details
        Dim Commanders As New ArrayList
        If FB <> "" Then
            Commanders.Add(FB)
        End If
        If WB <> "" Then
            Commanders.Add(WB)
        End If
        If SB <> "" Then
            Commanders.Add(SB)
        End If

        If Commanders.Count > 0 Then

            ' Go through each commander and parse the skills
            Dim FleetSkill(Commanders.Count + 1, fleetSkills.Count - 1) As String
            Dim hPilot As New HQFPilot
            For Commander As Integer = 0 To Commanders.Count - 1
                hPilot = CType(HQFPilotCollection.HQFPilots(Commanders(Commander)), HQFPilot)
                For Skill As Integer = 0 To fleetSkills.Count - 1
                    If hPilot.ImplantName(10) = fleetSkills(Skill).ToString & " Mindlink" Then
                        FleetSkill(Commander + 1, Skill) = "6"
                        FleetSkill(0, Skill) = FleetSkill(Commander + 1, Skill)
                        FleetSkill(Commanders.Count + 1, Skill) = hPilot.PilotName
                    Else
                        If hPilot.SkillSet.Contains(fleetSkills(Skill).ToString) Then
                            FleetSkill(Commander + 1, Skill) = CType(hPilot.SkillSet(fleetSkills(Skill).ToString), HQFSkill).Level.ToString
                            If FleetSkill(Commander + 1, Skill) >= FleetSkill(0, Skill) Then
                                FleetSkill(0, Skill) = FleetSkill(Commander + 1, Skill)
                                FleetSkill(Commanders.Count + 1, Skill) = hPilot.PilotName
                            End If
                        End If
                    End If
                Next
            Next

            ' Display the fleet skills data
            For skill As Integer = 0 To fleetSkills.Count - 1
                If CInt(FleetSkill(0, skill)) > 0 Then
                    Dim fleetModule As New ShipModule
                    fleetModule.Name = fleetSkills(skill).ToString & " (" & FleetSkill(Commanders.Count + 1, skill) & " - Level " & FleetSkill(0, skill) & ")"
                    fleetModule.ID = "-" & EveHQ.Core.SkillFunctions.SkillNameToID(fleetSkills(skill).ToString)
                    fleetModule.ModuleState = 32
                    Select Case fleetSkills(skill).ToString
                        Case "Armored Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Armored Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("335", 15)
                            Else
                                fleetModule.Attributes.Add("335", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Information Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Information Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("309", 15)
                            Else
                                fleetModule.Attributes.Add("309", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Leadership"
                            fleetModule.Attributes.Add("566", 2 * CInt(FleetSkill(0, skill)))
                        Case "Mining Foreman"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Mining Foreman Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("434", 15)
                            Else
                                fleetModule.Attributes.Add("434", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Siege Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Siege Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("337", 15)
                            Else
                                fleetModule.Attributes.Add("337", 2 * CInt(FleetSkill(0, skill)))
                            End If
                        Case "Skirmish Warfare"
                            If CInt(FleetSkill(0, skill)) = 6 Then
                                fleetModule.Name = "Skirmish Warfare Mindlink (" & FleetSkill(Commanders.Count + 1, skill) & ")"
                                fleetModule.Attributes.Add("151", -15)
                            Else
                                fleetModule.Attributes.Add("151", -2 * CInt(FleetSkill(0, skill)))
                            End If
                    End Select
                    cShip.FleetSlotCollection.Add(fleetModule)
                End If
            Next
        Else
            cShip.FleetSlotCollection.Clear()
        End If

    End Sub

    Private Sub CalculateFleetModuleEffects(ByRef cShip As Ship)

        Dim FleetCollection As New SortedList

        For Each fleetModule As ShipModule In SBModules
            If FleetCollection.ContainsKey(fleetModule.Name) = False Then
                ' Add it to the Fleet Collection
                FleetCollection.Add(fleetModule.Name, fleetModule)
            Else
                ' See if this module improves the fleet capabilities
                Dim compareModule As ShipModule = CType(FleetCollection(fleetModule.Name), ShipModule)
                If compareModule.Attributes.ContainsKey("833") = True Then
                    ' Contains the Command Bonus attribute
                    If Math.Abs(CDbl(fleetModule.Attributes("833"))) >= Math.Abs(CDbl(compareModule.Attributes("833"))) Then
                        FleetCollection(fleetModule.Name) = fleetModule
                    End If
                Else
                    ' Contains the ECM Command Bonus attribute
                    If compareModule.Attributes.ContainsKey("1320") = True Then
                        ' Contains the Command Bonus attribute
                        If Math.Abs(CDbl(fleetModule.Attributes("1320"))) >= Math.Abs(CDbl(compareModule.Attributes("1320"))) Then
                            FleetCollection(fleetModule.Name) = fleetModule
                        End If
                    End If
                End If
            End If

        Next

        For Each fleetModule As ShipModule In WBModules
            If FleetCollection.ContainsKey(fleetModule.Name) = False Then
                ' Add it to the Fleet Collection
                FleetCollection.Add(fleetModule.Name, fleetModule)
            Else
                ' See if this module improves the fleet capabilities
                Dim compareModule As ShipModule = CType(FleetCollection(fleetModule.Name), ShipModule)
                If compareModule.Attributes.ContainsKey("833") = True Then
                    ' Contains the Command Bonus attribute
                    If Math.Abs(CDbl(fleetModule.Attributes("833"))) >= Math.Abs(CDbl(compareModule.Attributes("833"))) Then
                        FleetCollection(fleetModule.Name) = fleetModule
                    End If
                Else
                    ' Contains the ECM Command Bonus attribute
                    If compareModule.Attributes.ContainsKey("1320") = True Then
                        ' Contains the Command Bonus attribute
                        If Math.Abs(CDbl(fleetModule.Attributes("1320"))) >= Math.Abs(CDbl(compareModule.Attributes("1320"))) Then
                            FleetCollection(fleetModule.Name) = fleetModule
                        End If
                    End If
                End If
            End If
        Next

        For Each fleetModule As ShipModule In FBModules
            If FleetCollection.ContainsKey(fleetModule.Name) = False Then
                ' Add it to the Fleet Collection
                FleetCollection.Add(fleetModule.Name, fleetModule)
            Else
                ' See if this module improves the fleet capabilities
                Dim compareModule As ShipModule = CType(FleetCollection(fleetModule.Name), ShipModule)
                If compareModule.Attributes.ContainsKey("833") = True Then
                    ' Contains the Command Bonus attribute
                    If Math.Abs(CDbl(fleetModule.Attributes("833"))) >= Math.Abs(CDbl(compareModule.Attributes("833"))) Then
                        FleetCollection(fleetModule.Name) = fleetModule
                    End If
                Else
                    ' Contains the ECM Command Bonus attribute
                    If compareModule.Attributes.ContainsKey("1320") = True Then
                        ' Contains the Command Bonus attribute
                        If Math.Abs(CDbl(fleetModule.Attributes("1320"))) >= Math.Abs(CDbl(compareModule.Attributes("1320"))) Then
                            FleetCollection(fleetModule.Name) = fleetModule
                        End If
                    End If
                End If
            End If
        Next

        For Each FleetModule As ShipModule In FleetCollection.Values
            cShip.FleetSlotCollection.Add(FleetModule)
        Next

    End Sub

    Private Function GetShipGangBonusModules(ByVal hShip As Ship, ByVal hPilot As HQFPilot) As ArrayList
        Dim FleetModules As New ArrayList
        If hShip IsNot Nothing Then
            Dim shipRoles As New ArrayList
            Dim hSkill As New HQFSkill
            'Dim fEffect As New FinalEffect
            'Dim fEffectList As New ArrayList
            shipRoles = CType(Engine.ShipBonusesMap(hShip.ID), ArrayList)
            If shipRoles IsNot Nothing Then
                For Each chkEffect As ShipEffect In shipRoles
                    If chkEffect.Status = 16 Then
                        ' We have a gang bonus effect so create a dummy module for handling this
                        Dim gangModule As New ShipModule
                        gangModule.Name = hShip.Name & " Gang Bonus"
                        gangModule.ID = "-1"
                        gangModule.SlotNo = 0
                        gangModule.ModuleState = 16
                        If hPilot.SkillSet.Contains(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))) = True Then
                            hSkill = CType(hPilot.SkillSet(EveHQ.Core.SkillFunctions.SkillIDToName(CStr(chkEffect.AffectingID))), HQFSkill)
                            If chkEffect.IsPerLevel = True Then
                                gangModule.Attributes.Add(chkEffect.AffectedAtt.ToString, chkEffect.Value * hSkill.Level)
                            Else
                                gangModule.Attributes.Add(chkEffect.AffectedAtt.ToString, chkEffect.Value)
                            End If
                        Else
                            gangModule.Attributes.Add(chkEffect.AffectedAtt.ToString, chkEffect.Value)
                        End If
                        FleetModules.Add(gangModule)

                    End If
                Next
            End If
        End If
        Return FleetModules

    End Function

    Private Sub GetRemoteModules(ByVal fleetShip As Ship, ByVal shipPilot As String)

        If activeFleet.RemoteReceiving.ContainsKey(shipPilot) = True Then
            Dim RR As ArrayList = CType(activeFleet.RemoteReceiving(shipPilot).Clone, ArrayList)
            Dim RA As FleetManager.RemoteAssignment
            For i As Integer = RR.Count - 1 To 0 Step -1
                RA = CType(RR(i), FleetManager.RemoteAssignment)
                Dim remoteShip As Ship = BaseFleetShips(RA.RemotePilot)
                For Each remoteModule As ShipModule In remoteShip.SlotCollection
                    If remoteGroups.Contains(CInt(remoteModule.DatabaseGroup)) = True Then
                        remoteModule.ModuleState = 16
                        remoteModule.SlotNo = 0
                        If RA.RemoteModule = remoteModule.Name Then
                            RR.RemoveAt(i)
                            fleetShip.RemoteSlotCollection.Add(remoteModule)
                        End If
                    End If
                Next
                For Each remoteDrone As DroneBayItem In remoteShip.DroneBayItems.Values
                    If remoteGroups.Contains(CInt(remoteDrone.DroneType.DatabaseGroup)) = True Then
                        If remoteDrone.IsActive = True Then
                            remoteDrone.DroneType.ModuleState = 16
                            If RA.RemoteModule = remoteDrone.DroneType.Name Then
                                RR.RemoveAt(i)
                                fleetShip.RemoteSlotCollection.Add(remoteDrone)
                            End If
                        End If
                    End If
                Next
            Next
        End If
    End Sub

#End Region

    Private Sub btnClearAssignments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAssignments.Click
        activeFleet.RemoteGiving.Clear()
        activeFleet.RemoteReceiving.Clear()
        Call Me.RedrawPilotList()
    End Sub

    Private Sub ctxPilotList_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxPilotList.Opening
        If clvPilots.SelectedItems.Count = 1 Then
            Dim selItem As ContainerListViewItem = clvPilots.SelectedItems(0)
            If selItem.Depth = 2 Then
                If selItem.Text = selItem.ParentItem.Text And selItem.SubItems(4).Text = "" Then
                    e.Cancel = True
                Else
                    mnuRemoveRemoteModule.Enabled = True
                    mnuFMShowMissingSkills.Enabled = False
                End If
            ElseIf selItem.Depth = 1 Then
                mnuRemoveRemoteModule.Enabled = False
                mnuFMShowMissingSkills.Enabled = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub mnuRemoveRemoteModule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveRemoteModule.Click
        If clvPilots.SelectedItems.Count > 0 Then
            ' Determine which deletion method we are using
            Dim selItem As ContainerListViewItem = clvPilots.SelectedItems(0)
            Dim remotePilot As String = ""
            Dim fleetPilot As String = ""
            If selItem.Text = selItem.ParentItem.Text Then
                remotePilot = selItem.Text
                fleetPilot = selItem.SubItems(4).Text
            Else
                remotePilot = selItem.Text
                fleetPilot = selItem.ParentItem.Text
            End If
            ' Get the remote module information
            Dim remoteModule As String = selItem.SubItems(3).Text
            Dim RA As FleetManager.RemoteAssignment
            ' First, remove it from the receiving pilot
            Dim RR As ArrayList = activeFleet.RemoteReceiving(fleetPilot)
            For i As Integer = RR.Count - 1 To 0 Step -1
                RA = CType(RR(i), FleetManager.RemoteAssignment)
                If RA.FleetPilot = fleetPilot And RA.RemotePilot = remotePilot And RA.RemoteModule = remoteModule Then
                    RR.RemoveAt(i)
                End If
            Next
            ' Next, remove it from the giving pilot
            Dim RG As ArrayList = activeFleet.RemoteGiving(remotePilot)
            For i As Integer = RG.Count - 1 To 0 Step -1
                RA = CType(RG(i), FleetManager.RemoteAssignment)
                If RA.FleetPilot = fleetPilot And RA.RemotePilot = remotePilot And RA.RemoteModule = remoteModule Then
                    RG.RemoveAt(i)
                End If
            Next
            Call Me.RedrawPilotList()
        End If
    End Sub

    Private Sub mnuFMShowMissingSkills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFMShowMissingSkills.Click
        If clvPilots.SelectedItems.Count > 0 Then
            ' Determine which deletion method we are using
            Dim selItem As ContainerListViewItem = clvPilots.SelectedItems(0)
            Dim fleetPilot As String = selItem.Text
            Dim cSetup As FleetManager.FleetSetup = activeFleet.FleetSetups(fleetPilot)
            If cSetup.RequiredSkills.Count > 0 Then
                Dim myRequiredSkills As New frmRequiredSkills
                myRequiredSkills.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(fleetPilot), EveHQ.Core.Pilot)
                myRequiredSkills.Skills = cSetup.RequiredSkills
                myRequiredSkills.ShowDialog()
            End If
        End If
    End Sub
End Class