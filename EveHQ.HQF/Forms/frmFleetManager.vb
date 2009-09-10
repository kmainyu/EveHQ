Imports DotNetLib.Windows.Forms
Imports System.Windows.Forms
Imports System.Drawing

Public Class frmFleetManager
    Dim maxWings As Integer = 5
    Dim maxSquads As Integer = 5
    Dim maxMembers As Integer = 10
    Dim OpenFleet As New ArrayList
    Dim OpenWings As New ArrayList
    Dim OpenSquads As New ArrayList
    Dim ExpandAll As Boolean = False
    Dim activeFleet As New FleetManager.Fleet
    Dim activeFleetMembers As New SortedList(Of String, String)
    Dim internalReorder As Boolean = False

#Region "Form Initialisation, Loading and Closing"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Setup the default fleet structure objects
        Call AddTestPilots()
        Call RedrawFleetList()

    End Sub

    Private Sub AddTestPilots()
        clvPilotList.BeginUpdate()
        clvPilotList.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim newPilot As New ContainerListViewItem
            newPilot.Text = cPilot.Name
            clvPilotList.Items.Add(newPilot)
        Next
        clvPilotList.EndUpdate()
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
            Call Me.RedrawFleetStructure()
        End If

        ' Dispose of the form
        NewFleetForm.Dispose()

    End Sub

    Private Sub RedrawFleetList()
        ' Redraw the list of fleets
        clvFleetList.BeginUpdate()
        clvFleetList.Items.Clear()
        For Each fleetName As String In FleetManager.FleetCollection.Keys
            Dim newFleet As New ContainerListViewItem
            newFleet.Text = fleetName
            clvFleetList.Items.Add(newFleet)
        Next
        clvFleetList.EndUpdate()
    End Sub

    Private Sub clvFleetList_SelectedItemsChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clvFleetList.SelectedItemsChanged
        If clvFleetList.SelectedItems.Count > 0 Then
            Dim selFleet As ContainerListViewItem = clvFleetList.SelectedItems(0)
            activeFleet = FleetManager.FleetCollection(selFleet.Text)
            ' Reset the open items
            Me.OpenFleet.Clear()
            Me.OpenSquads.Clear()
            Me.OpenWings.Clear()
            ' Redraw the fleet structure
            Call Me.RedrawFleetStructure()
        End If

    End Sub

#End Region

#Region "Fleet Structure Routines"

    Private Sub RedrawFleetStructure()

        ' Clear the current structure
        clvFleetStructure.BeginUpdate()
        clvFleetStructure.Items.Clear()
        activeFleetMembers.Clear()

        ' Initialise the Fleet Node
        Dim newFleet As New ContainerListViewItem
        newFleet.Text = activeFleet.Name
        newFleet.Tag = activeFleet.Name

        If activeFleet.Commander <> "" Then
            newFleet.Text &= " (" & activeFleet.Commander & ")"
            activeFleetMembers.Add(activeFleet.Commander, activeFleet.Name & ";" & "FC")
        Else
            newFleet.Text &= " (No Commander)"
        End If
        clvFleetStructure.Items.Add(newFleet)
        If OpenFleet.Contains(activeFleet.Name) = True Or ExpandAll = True Then
            newFleet.Expand()
        End If
        For Each myWing As FleetManager.Wing In activeFleet.Wings.Values
            Dim newWing As New ContainerListViewItem
            newWing.Text = myWing.Name
            newWing.Tag = myWing.Name
            If myWing.Commander <> "" Then
                newWing.Text &= " (" & myWing.Commander & ")"
                activeFleetMembers.Add(myWing.Commander, activeFleet.Name & ";" & myWing.Name & ";" & "WC")
            Else
                newWing.Text &= " (No Commander)"
            End If
            newFleet.Items.Add(newWing)
            If OpenWings.Contains(myWing.Name) = True Or ExpandAll = True Then
                newWing.Expand()
            End If
            For Each mySquad As FleetManager.Squad In myWing.Squads.Values
                Dim newSquad As New ContainerListViewItem
                newSquad.Text = mySquad.Name
                newSquad.Tag = mySquad.Name
                If mySquad.Commander <> "" Then
                    newSquad.Text &= " (" & mySquad.Commander & ")"
                    activeFleetMembers.Add(mySquad.Commander, activeFleet.Name & ";" & myWing.Name & ";" & mySquad.Name & ";" & "SC")
                Else
                    newSquad.Text &= " (No Commander)"
                End If
                newWing.Items.Add(newSquad)
                If OpenSquads.Contains(mySquad.Name) = True Or ExpandAll = True Then
                    newSquad.Expand()
                End If
                For Each myMember As String In mySquad.Members.Values
                    Dim newMember As New ContainerListViewItem
                    newMember.Text = myMember
                    newMember.Tag = myMember
                    newSquad.Items.Add(newMember)
                    activeFleetMembers.Add(myMember, activeFleet.Name & ";" & myWing.Name & ";" & mySquad.Name)
                Next
            Next
        Next

        ' End the update of the structure
        ExpandAll = False
        clvFleetStructure.EndUpdate()
        lblViewingFleet.Text = "Viewing Fleet: " & activeFleet.Name

    End Sub

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
                Select Case item1.Depth
                    Case 1 ' FC
                        Dim fleetItem As ContainerListViewItem
                        fleetItem = item1
                        ' Check if the pilot is already in the fleet
                        If activeFleetMembers.ContainsKey(DropName) = True And internalReorder = False Then
                            MessageBox.Show(DropName & " is already part of the fleet and can be found in the role '" & activeFleetMembers(DropName) & "'.", "Already in Fleet", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                            MessageBox.Show(DropName & " is already part of the fleet and can be found in the role '" & activeFleetMembers(DropName) & "'.", "Already in Fleet", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                            MessageBox.Show(DropName & " is already part of the fleet and can be found in the role '" & activeFleetMembers(DropName) & "'.", "Already in Fleet", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                                    ' Redraw the structure
                                    Call Me.RedrawFleetStructure()
                                Else
                                    ' Install as SM
                                    If internalReorder = True Then
                                        ' Need to check where the pilot came from and remove him
                                        Call RemoveMember(droppedItem)
                                    End If
                                    activeFleet.Wings(wingItem.Tag.ToString).Squads(squadItem.Tag.ToString).Members.Add(DropName, DropName)
                                    ' Redraw the structure
                                    Call Me.RedrawFleetStructure()
                                End If
                            Else
                                MessageBox.Show("You cannot exceed a squad size of 10", "Squad Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End If
                End Select
            End If
        End If

        ' Reset the internalReorder flag ready for the next drag/drop operation
        internalReorder = False

    End Sub

    Private Sub clvFleetStructure_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles clvFleetStructure.DragEnter
        '' Check for the custom DataFormat ContainerListViewItem
        'If e.Data.GetDataPresent("System.Windows.Forms.ContainerListViewItem()") Then
        '    e.Effect = DragDropEffects.Move
        'Else
        '    e.Effect = DragDropEffects.None
        'End If

    End Sub

    Private Sub clvFleetStructure_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles clvFleetStructure.ItemDrag
        Dim myItem As ContainerListViewItem = clvFleetStructure.SelectedItems(0)
        ' Create a DataObject containg the ContainerListViewItem
        internalReorder = True
        ' restate the text as the pilot name
        If myItem.Tag.ToString <> myItem.Text Then
            Dim myName As String = myItem.Text.TrimStart(myItem.Tag.ToString.ToCharArray)
            myName = myName.TrimStart(" (".ToCharArray).TrimEnd(")".ToCharArray)
            clvFleetStructure.Tag = myName
        End If
        clvPilotList.DoDragDrop(New DataObject("System.Windows.Forms.ContainerListViewItem", myItem), DragDropEffects.Move)
    End Sub

    Private Sub clvPilotList_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles clvPilotList.ItemDrag
        Dim myItem As ContainerListViewItem = clvPilotList.SelectedItems(0)
        ' Create a DataObject containg the ContainerListViewItem
        clvPilotList.DoDragDrop(New DataObject("System.Windows.Forms.ContainerListViewItem", myItem), DragDropEffects.Move)
    End Sub

    Private Sub clvFleetStructure_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles clvFleetStructure.DragOver
        ' Check for the custom DataFormat ListViewItem array.
        If e.Data.GetDataPresent("System.Windows.Forms.ContainerListViewItem") Then

            Dim point1 As Point = clvFleetStructure.PointToClient(New Point(e.X, e.Y))
            Dim item1 As ContainerListViewItem = clvFleetStructure.GetItemAt(point1.Y - clvFleetStructure.HeaderHeight)

            'Point(Point = this.PointToClient(New Point(e.X, e.Y)))
            'ContainerListViewItem(ListViewItem = GetItemAt(Point.Y - this.HeaderHeight))

            If item1 IsNot Nothing Then
                If item1.Depth >= 1 Then
                    item1.Selected = True
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
            e.Effect = DragDropEffects.None
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

    End Sub

    Private Sub FSSetFleetBooster(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FSSetWingBooster(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FSSetSquadBooster(ByVal sender As Object, ByVal e As System.EventArgs)

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
                activeFleetMembers.Remove(selItem.Tag.ToString)
        End Select
    End Sub

#End Region


  
End Class