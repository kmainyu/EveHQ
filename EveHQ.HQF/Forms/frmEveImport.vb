Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports DotNetLib.Windows.Forms

Public Class frmEveImport

    Dim EveFolder As String = Path.Combine(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eve"), "fittings")
    Dim currentShip As Ship
    Dim currentFit As New ArrayList
    Dim currentFitName As String = ""

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add the current list of pilots to the combobox
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilots.Items.Add(cPilot.Name)
            End If
        Next
        cboPilots.EndUpdate()
        ' Look at the settings for default pilot
        If cboPilots.Items.Count > 0 Then
            If cboPilots.Items.Contains(HQF.Settings.HQFSettings.DefaultPilot) = True Then
                cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
            Else
                cboPilots.SelectedIndex = 0
            End If
        End If

        ' Add the profiles
        cboProfiles.BeginUpdate()
        cboProfiles.Items.Clear()
        For Each newProfile As DamageProfile In DamageProfiles.ProfileList.Values
            cboProfiles.Items.Add(newProfile.Name)
        Next
        cboProfiles.EndUpdate()
        ' Select the default profile
        cboProfiles.SelectedItem = "<Omni-Damage>"

    End Sub

    Private Sub frmEveImport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call GetEveFittings()
    End Sub

#Region "Ship Fitting routines"

    Private Sub UpdateSlotColumns()
        ' Clear the columns
        lvwSlots.Columns.Clear()
        ' Add the module name column
        lvwSlots.Columns.Add("colName", "Module Name", 175, HorizontalAlignment.Left, "")
        lvwSlots.Columns.Add("Charge", "Charge Name", 175, HorizontalAlignment.Left, "")
    End Sub
    Private Sub UpdateSlotLayout()
        If currentShip IsNot Nothing Then
            lvwSlots.BeginUpdate()
            lvwSlots.Items.Clear()
            ' Produce high slots
            For slot As Integer = 1 To currentShip.HiSlots
                Dim newSlot As New ListViewItem
                newSlot.Name = "8_" & slot
                newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                newSlot.ForeColor = Color.Black
                newSlot.Group = lvwSlots.Groups.Item("lvwgHighSlots")
                Call Me.AddUserColumns(currentShip.HiSlot(slot), newSlot)
                lvwSlots.Items.Add(newSlot)
            Next
            For slot As Integer = 1 To currentShip.MidSlots
                Dim newSlot As New ListViewItem
                newSlot.Name = "4_" & slot
                newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                newSlot.ForeColor = Color.Black
                newSlot.Group = lvwSlots.Groups.Item("lvwgMidSlots")
                Call Me.AddUserColumns(currentShip.MidSlot(slot), newSlot)
                lvwSlots.Items.Add(newSlot)
            Next
            For slot As Integer = 1 To currentShip.LowSlots
                Dim newSlot As New ListViewItem
                newSlot.Name = "2_" & slot
                newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                newSlot.ForeColor = Color.Black
                newSlot.Group = lvwSlots.Groups.Item("lvwgLowSlots")
                Call Me.AddUserColumns(currentShip.LowSlot(slot), newSlot)
                lvwSlots.Items.Add(newSlot)
            Next
            For slot As Integer = 1 To currentShip.RigSlots
                Dim newSlot As New ListViewItem
                newSlot.Name = "1_" & slot
                newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                newSlot.ForeColor = Color.Black
                newSlot.Group = lvwSlots.Groups.Item("lvwgRigSlots")
                Call Me.AddUserColumns(currentShip.RigSlot(slot), newSlot)
                lvwSlots.Items.Add(newSlot)
            Next
            lvwSlots.EndUpdate()
        End If
    End Sub
    Private Sub AddUserColumns(ByVal shipMod As ShipModule, ByVal slotName As ListViewItem)
        ' Add subitems based on the user selected columns
        If shipMod IsNot Nothing Then
            Dim colName As String = ""
            ' Add in the module name
            slotName.Text = shipMod.Name
            If shipMod.LoadedCharge IsNot Nothing Then
                slotName.SubItems.Add(shipMod.LoadedCharge.Name)
            Else
                slotName.SubItems.Add("")
            End If
        Else
            slotName.Text = "<Empty>"
            slotName.SubItems.Add("")
        End If
    End Sub
    Private Sub ClearShipSlots()
        If currentShip IsNot Nothing Then
            For slot As Integer = 1 To currentShip.HiSlots
                currentShip.HiSlot(slot) = Nothing
            Next
            For slot As Integer = 1 To currentShip.MidSlots
                currentShip.MidSlot(slot) = Nothing
            Next
            For slot As Integer = 1 To currentShip.LowSlots
                currentShip.LowSlot(slot) = Nothing
            Next
            For slot As Integer = 1 To currentShip.RigSlots
                currentShip.RigSlot(slot) = Nothing
            Next
            currentShip.DroneBayItems.Clear()
            currentShip.DroneBay_Used = 0
            currentShip.CargoBayItems.Clear()
            currentShip.CargoBay_Used = 0
        End If
    End Sub
    Private Sub GenerateFittingData()
        ' Let's try and generate a fitting and get some damage info
        If currentShip IsNot Nothing Then
            Dim loadoutPilot As HQF.HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem.ToString), HQFPilot)
            Dim loadoutProfile As HQF.DamageProfile = CType(HQF.DamageProfiles.ProfileList(cboProfiles.SelectedItem.ToString), DamageProfile)

            currentShip.DamageProfile = loadoutProfile
            Dim loadoutShip As Ship = Engine.ApplyFitting(currentShip, loadoutPilot)

            lblEHP.Text = FormatNumber(loadoutShip.EffectiveHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            lblTank.Text = FormatNumber(CDbl(loadoutShip.Attributes("10062")), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " DPS"
            lblVolley.Text = FormatNumber(CDbl(loadoutShip.Attributes("10028")), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            lblDPS.Text = FormatNumber(CDbl(loadoutShip.Attributes("10029")), 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            lblShieldResists.Text = FormatNumber(loadoutShip.ShieldEMResist, 0) & "/" & FormatNumber(loadoutShip.ShieldExResist, 0) & "/" & FormatNumber(loadoutShip.ShieldKiResist, 0) & "/" & FormatNumber(loadoutShip.ShieldThResist, 0)
            lblArmorResists.Text = FormatNumber(loadoutShip.ArmorEMResist, 0) & "/" & FormatNumber(loadoutShip.ArmorExResist, 0) & "/" & FormatNumber(loadoutShip.ArmorKiResist, 0) & "/" & FormatNumber(loadoutShip.ArmorThResist, 0)
            Dim cap As Double = Engine.CalculateCapStatistics(loadoutShip)
            If cap > 0 Then
                cap = cap / loadoutShip.CapCapacity * 100
                lblCapacitor.Text = "Stable at " & FormatNumber(cap, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            Else
                lblCapacitor.Text = "Lasts " & EveHQ.Core.SkillFunctions.TimeToString(-cap, False)
            End If
            lblVelocity.Text = FormatNumber(loadoutShip.MaxVelocity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m/s"
            lblMaxRange.Text = FormatNumber(loadoutShip.MaxTargetRange, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "m"
            Dim CPU As Double = loadoutShip.CPU_Used / loadoutShip.CPU * 100
            lblCPU.Text = FormatNumber(CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            If CPU > 100 Then
                lblCPU.ForeColor = Color.Red
            Else
                lblCPU.ForeColor = Color.Black
            End If
            Dim PG As Double = loadoutShip.PG_Used / loadoutShip.PG * 100
            lblPG.Text = FormatNumber(PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            If PG > 100 Then
                lblPG.ForeColor = Color.Red
            Else
                lblPG.ForeColor = Color.Black
            End If

            Dim maxOpt As Double = 0
            For slot As Integer = 1 To loadoutShip.HiSlots
                Dim shipMod As ShipModule = loadoutShip.HiSlot(slot)
                If shipMod IsNot Nothing Then
                    If shipMod.Attributes.Contains("54") Then
                        maxOpt = Math.Max(maxOpt, CDbl(shipMod.Attributes("54")))
                    End If
                End If
            Next
            lblOptimalRange.Text = FormatNumber(maxOpt, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "m"

        End If

    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        Call GenerateFittingData()
    End Sub

    Private Sub cboProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProfiles.SelectedIndexChanged
        Call GenerateFittingData()
    End Sub

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Dim shipName As String = currentShip.Name
        Dim fittingName As String = currentFitName
        ' If the fitting exists, add a number onto the end
        If Fittings.FittingList.ContainsKey(shipName & ", " & fittingName) = True Then
            Dim response As Integer = MessageBox.Show("Fitting name already exists. Are you sure you wish to import the fitting?", "Confirm Import for " & shipName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If response = Windows.Forms.DialogResult.Yes Then
                Dim newFittingName As String = ""
                Dim revision As Integer = 1
                Do
                    revision += 1
                    newFittingName = fittingName & " " & revision.ToString
                Loop Until Fittings.FittingList.ContainsKey(shipName & ", " & newFittingName) = False
                fittingName = newFittingName
                MessageBox.Show("New fitting name is '" & fittingName & "'.", "New Fitting Imported", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Exit Sub
            End If
        End If
        ' Lets create the fitting
        Fittings.FittingList.Add(shipName & ", " & fittingName, currentFit)
        HQFEvents.StartUpdateFittingList = True
    End Sub

#End Region

#Region "UI Routines"

    Private Sub mnuViewLoadout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewLoadout.Click
        Dim cLoadout As ContainerListViewItem = clvLoadouts.SelectedItems(0)
        Call GetEveShipLoadout(cLoadout)
    End Sub

    Private Sub clvLoadouts_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles clvLoadouts.MouseDoubleClick
        If clvLoadouts.SelectedItems.Count > 0 Then
            If clvLoadouts.SelectedItems(0).Items.Count = 0 Then
                Dim cLoadout As ContainerListViewItem = clvLoadouts.SelectedItems(0)
                Call GetEveShipLoadout(cLoadout)
            End If
        End If
    End Sub

#End Region

#Region "Eve Import Routines"

    Private Sub GetEveFittings()

        ' Check for the fittings directory and create it
        If My.Computer.FileSystem.DirectoryExists(EveFolder) = False Then
            MessageBox.Show("The Eve fittings folder is not present on your system and is required for this feature to work. You will need some fittings present in this folder either exported from Eve or EveHQ before proceeding.", "Fittings Folder Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
            Exit Sub
        End If

        Dim files As New ArrayList
        Dim Ships As New SortedList
        Dim ShipFittings As New SortedList
        For Each fileName As String In My.Computer.FileSystem.GetFiles(EveFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.xml")
            files.Add(fileName)
        Next
        Dim fitXML As New XmlDocument
        Dim fittingList As XmlNodeList
        Dim shipNode As XmlNode
        Dim shipItem As New ContainerListViewItem
        Dim fitItem As New ContainerListViewItem
        Dim shipName As String = ""
        Dim shipFit As String = ""
        clvLoadouts.BeginUpdate()
        clvLoadouts.Items.Clear()
        For Each filename As String In files
            Try
                fitXML.Load(filename)
                fittingList = fitXML.SelectNodes("/fittings/fitting")
                For Each fitNode As XmlNode In fittingList
                    shipNode = fitNode.SelectSingleNode("shipType")
                    shipName = shipNode.Attributes("value").Value.ToString
                    shipFit = fitNode.Attributes("name").Value.ToString
                    If Ships.ContainsKey(shipName) = False Then
                        ShipFittings = New SortedList
                        ShipFittings.Add(shipFit, filename)
                        Ships.Add(shipName, ShipFittings)
                    Else
                        ShipFittings = CType(Ships(shipName), SortedList)
                        If ShipFittings.ContainsKey(shipFit) = False Then
                            ShipFittings.Add(shipFit, filename)
                        End If
                    End If
                Next
            Catch e As Exception
            End Try
        Next
        For Each Ship As String In Ships.Keys
            shipItem = New ContainerListViewItem
            shipItem.Text = Ship
            clvLoadouts.Items.Add(shipItem)
            ShipFittings = CType(Ships(Ship), Collections.SortedList)
            For Each fit As String In ShipFittings.Keys
                fitItem = New ContainerListViewItem
                fitItem.Text = fit
                fitItem.Tag = CStr(ShipFittings(fit))
                shipItem.Items.Add(fitItem)
            Next
        Next
        clvLoadouts.EndUpdate()
    End Sub

    Private Sub GetEveShipLoadout(ByVal cLoadout As ContainerListViewItem)
        Dim fitName As String = cLoadout.Text
        Dim fileName As String = cLoadout.Tag.ToString
        Dim shipName As String = cLoadout.ParentItem.Text
        currentShip = CType(ShipLists.shipList(shipName), Ship)
        Dim moduleList As New SortedList
        Dim droneList As New SortedList
        ' Open the file and load the XML
        Dim fitXML As New XmlDocument
        fitXML.Load(fileName)
        Dim fittingList As XmlNodeList = fitXML.SelectNodes("/fittings/fitting")
        For Each fitNode As XmlNode In fittingList
            If fitNode.Attributes("name").Value = fitName And fitNode.SelectSingleNode("shipType").Attributes("value").Value = shipName Then
                Dim modNodes As XmlNodeList = fitNode.SelectNodes("hardware")
                For Each modNode As XmlNode In modNodes
                    Dim fModule As ShipModule = CType(ModuleLists.moduleList(ModuleLists.moduleListName(modNode.Attributes("type").Value)), ShipModule)
                    If moduleList.ContainsKey(modNode.Attributes("slot").Value) = False Then
                        ' Add the mod/ammo to the slot
                        If fModule.IsCharge = True Then
                            moduleList.Add(modNode.Attributes("slot").Value, ", " & fModule.Name)
                        Else
                            If fModule.IsDrone = True Then
                                droneList.Add(fModule.Name, CInt(modNode.Attributes("qty").Value))
                            Else
                                moduleList.Add(modNode.Attributes("slot").Value, fModule.Name)
                            End If
                        End If
                    Else
                        If fModule.IsCharge = True Then
                            moduleList(modNode.Attributes("slot").Value) = CStr(moduleList(modNode.Attributes("slot").Value)) & ", " & fModule.Name
                        Else
                            If fModule.IsDrone = True Then
                                droneList.Add(fModule.Name, CInt(modNode.Attributes("qty").Value))
                            Else
                                moduleList(modNode.Attributes("slot").Value) = fModule.Name & ", " & CStr(moduleList(modNode.Attributes("slot").Value))
                            End If
                        End If
                    End If
                Next
                currentFit.Clear()
                Call ClearShipSlots()
                For Each fittedMod As String In moduleList.Values
                    currentFit.Add(fittedMod)
                Next
                For Each fittedDrone As String In droneList.Keys
                    currentFit.Add(fittedDrone & " x" & CStr(droneList(fittedDrone)))
                Next
                currentFitName = fitName
                currentShip = Engine.UpdateShipDataFromFittingList(currentShip, currentFit)
                ' Generate fitting data
                Call Me.GenerateFittingData()
                gbStatistics.Visible = True
                btnImport.Enabled = True
                Call UpdateSlotColumns()
                Call UpdateSlotLayout()
                Exit For
            End If
        Next
    End Sub

#End Region

End Class