Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports DotNetLib.Windows.Forms

Public Class frmBCBrowser

    Dim currentShip As Ship
    Dim currentFit As New ArrayList
    Dim BCLoadoutCache As String = Path.Combine(EveHQ.Core.HQ.appDataFolder, "BCLoadoutCache")
    Dim SourceURL As String = ""

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Create the fittings cache if it doesn't exist!
        Try
            If My.Computer.FileSystem.DirectoryExists(BCLoadoutCache) = False Then
                My.Computer.FileSystem.CreateDirectory(BCLoadoutCache)
            End If
        Catch ex As Exception
            MessageBox.Show("Unable to create the Loadout cache folder. Caching will be disabled which may affect loadout downloads.", "Error Creating Folder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try

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

    Public Property ShipType() As Ship
        Get
            Return currentShip
        End Get
        Set(ByVal value As Ship)
            currentShip = value
            pbShip.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(currentShip.ID, EveHQ.Core.ImageHandler.ImageType.Types)
            Call GetBCShipLoadouts()
        End Set
    End Property

#Region "BC Routines"

    Private Sub GetBCShipLoadouts()

        ' Check if the loadout list is in the cache
        lblBCStatus.Text = "Checking Loadout Cache..." : StatusStrip1.Refresh()
        Dim loadoutXML As New XmlDocument
        Dim UseCacheFile As Boolean = False
        If My.Computer.FileSystem.DirectoryExists(BCLoadoutCache) Then
            If My.Computer.FileSystem.FileExists(Path.Combine(BCLoadoutCache, currentShip.ID & ".xml")) Then
                ' Open the file and check the cache time
                loadoutXML.Load(Path.Combine(BCLoadoutCache, currentShip.ID & ".xml"))
                Dim cacheNode As XmlNode = loadoutXML.SelectSingleNode("/loadouts/cacheExpires")
                Dim cacheTime As DateTime = DateTime.Parse(cacheNode.InnerText)
                If Now > cacheTime Then
                    ' cache expired, get a new one
                    UseCacheFile = False
                Else
                    UseCacheFile = True
                End If
            End If
        End If

        If UseCacheFile = False Then
            lblBCStatus.Text = "Retrieving " & currentShip.Name & " loadouts from BattleClinic..."
            Dim remoteURL As String = "http://eve.battleclinic.com/ship_loadout_feed.php?typeID=" & currentShip.ID
            Try
                ' Create the requester
                ServicePointManager.DefaultConnectionLimit = 20
                ServicePointManager.Expect100Continue = False
                Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(remoteURL))
                Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
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
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
                ' Prepare for a response from the server
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                ' Get the stream associated with the response.
                Dim receiveStream As Stream = response.GetResponseStream()
                ' Pipes the stream to a higher level stream reader with the required encoding format. 
                Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
                loadoutXML.LoadXml(readStream.ReadToEnd())
            Catch e As Exception
                lblBCStatus.Text = "Unable to retrieve loadout information from BattleClinic!"
            End Try
        End If

        Dim loadoutList As XmlNodeList = loadoutXML.SelectNodes("/loadouts/race/ship/loadout")
        If loadoutList.Count > 0 Then
            clvLoadouts.BeginUpdate()
            clvLoadouts.Items.Clear()
            For Each loadout As XmlNode In loadoutList
                Dim nLoadout As New ContainerListViewItem
                nLoadout.Text = loadout.Attributes("name").Value
                nLoadout.Tag = loadout.Attributes("loadoutID").Value
                clvLoadouts.Items.Add(nLoadout)
                nLoadout.SubItems(1).Text = loadout.Attributes("Author").Value
                nLoadout.SubItems(1).Tag = loadout.Attributes("topic").Value
                nLoadout.SubItems(2).Text = loadout.Attributes("rating").Value
                nLoadout.SubItems(3).Text = FormatDateTime(DateTime.Parse(loadout.Attributes("date").Value), DateFormat.ShortDate)
            Next
            clvLoadouts.EndUpdate()
            lblShipType.Text = currentShip.Name
            lblBCStatus.Text = "Update of loudouts completed!"
            pbShip.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(currentShip.ID, EveHQ.Core.ImageHandler.ImageType.Types)
            ' Save the XML into the cache
            If UseCacheFile = False Then
                loadoutXML.Save(Path.Combine(BCLoadoutCache, currentShip.ID & ".xml"))
            End If
        Else
            lblBCStatus.Text = "There are no fittings available for this ship!"
        End If

    End Sub

    Private Sub GetBCShipLoadout(ByVal cLoadout As ContainerListViewItem)
        Dim LoadoutName As String = cLoadout.Text
        Dim LoadoutID As String = cLoadout.Tag.ToString
        Dim LoadoutAuthor As String = cLoadout.SubItems(1).Text
        Dim LoadoutTopic As String = cLoadout.SubItems(1).Tag.ToString
        Dim LoadoutScore As String = cLoadout.SubItems(2).Text
        Dim LoadoutDate As String = cLoadout.SubItems(3).Text

        ' Check if the fitting is in the cache
        lblBCStatus.Text = "Checking Loadout Cache..." : StatusStrip1.Refresh()
        Dim loadoutXML As New XmlDocument
        Dim UseCacheFile As Boolean = False
        If My.Computer.FileSystem.DirectoryExists(BCLoadoutCache) Then
            If My.Computer.FileSystem.FileExists(Path.Combine(BCLoadoutCache, currentShip.ID.ToString & "-" & LoadoutID & ".xml")) Then
                ' Open the file and check the cache time
                loadoutXML.Load(Path.Combine(BCLoadoutCache, currentShip.ID.ToString & "-" & LoadoutID & ".xml"))
                Dim cacheNode As XmlNode = loadoutXML.SelectSingleNode("/loadouts/cacheExpires")
                Dim cacheTime As DateTime = DateTime.Parse(cacheNode.InnerText)
                If Now > cacheTime Then
                    ' cache expired, get a new one
                    UseCacheFile = False
                Else
                    UseCacheFile = True
                End If
            End If
        End If

        If UseCacheFile = False Then
            lblBCStatus.Text = "Retrieving " & LoadoutName & "(" & currentShip.Name & ") from BattleClinic..." : StatusStrip1.Refresh()
            Dim remoteURL As String = "http://eve.battleclinic.com/ship_loadout_feed.php?typeID=" & currentShip.ID.ToString & "&id=" & LoadoutID
            Try
                ' Create the requester
                ServicePointManager.DefaultConnectionLimit = 20
                ServicePointManager.Expect100Continue = False
                Dim servicePoint As ServicePoint = ServicePointManager.FindServicePoint(New Uri(remoteURL))
                Dim request As HttpWebRequest = CType(WebRequest.Create(remoteURL), HttpWebRequest)
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
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
                ' Prepare for a response from the server
                Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                ' Get the stream associated with the response.
                Dim receiveStream As Stream = response.GetResponseStream()
                ' Pipes the stream to a higher level stream reader with the required encoding format. 
                Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
                loadoutXML.LoadXml(readStream.ReadToEnd())
            Catch e As Exception
                lblBCStatus.Text = "Unable to retrieve loadout information from BattleClinic!" : StatusStrip1.Refresh()
            End Try
        End If

        Dim loadoutList As XmlNodeList = loadoutXML.SelectNodes("/loadouts/race/ship/loadout/slot")
        If loadoutList.Count > 0 Then
            Call ClearShipSlots()
            Dim moduleList, ammoList As New ArrayList
            For Each loadout As XmlNode In loadoutList
                If loadout.InnerText <> "0" Then
                    Select Case loadout.Attributes("type").Value
                        Case "high", "med", "lo", "rig", "subSystem", "drone"
                            moduleList.Add(loadout.InnerText)
                        Case "ammo"
                            ammoList.Add(loadout.InnerText)
                    End Select
                End If
            Next
            ' Try and match the ammo to the modules
            Dim BaseFit As String = ""
            Dim RevisedFit As String = ""
            currentFit.Clear()
            For Each fittedMod As String In moduleList
                Dim fModule As ShipModule = CType(ModuleLists.moduleList(fittedMod), ShipModule)
                If fModule IsNot Nothing Then
                    BaseFit = fModule.Name : RevisedFit = BaseFit
                    If fModule.Charges.Count <> 0 Then
                        For Each ammo As String In ammoList
                            If ModuleLists.moduleList.ContainsKey(ammo) = True Then
                                If fModule.Charges.Contains(CType(ModuleLists.moduleList(ammo), ShipModule).DatabaseGroup) Then
                                    RevisedFit = BaseFit & "," & CType(ModuleLists.moduleList(ammo), ShipModule).Name
                                End If
                            End If
                        Next
                        currentFit.Add(RevisedFit)
                    Else
                        currentFit.Add(fModule.Name)
                    End If
                End If
            Next
            Call Me.ReorderModules()
            lblLoadoutName.Text = LoadoutName : lblLoadoutName.Visible = True : lblLoadoutNameLbl.Visible = True
            lblLoadoutAuthor.Text = LoadoutAuthor : lblLoadoutAuthor.Visible = True : lblLoadoutAuthorLbl.Visible = True
            lblLoadoutScore.Text = LoadoutScore : lblLoadoutScore.Visible = True : lblLoadoutScoreLbl.Visible = True
            lblLoadoutDate.Text = LoadoutDate : lblLoadoutDate.Visible = True : lblLoadoutDateLbl.Visible = True
            lblLoadoutTopic.Text = "BattleClinic Topic" : lblLoadoutTopic.Visible = True : LblLoadoutTopicLbl.Visible = True
            lblLoadoutTopic.Tag = LoadoutTopic
            lblBCStatus.Text = "Download of loadout (ID: " & LoadoutID & ") completed!" : StatusStrip1.Refresh()
            SourceURL = "http://forum.battleclinic.com/index.php/topic," & lblLoadoutTopic.Tag.ToString & ".0.html"
            btnImport.Enabled = True
            ' Save the XML into the cache
            If UseCacheFile = False Then
                loadoutXML.Save(Path.Combine(BCLoadoutCache, currentShip.ID.ToString & "-" & LoadoutID & ".xml"))
            End If
            currentShip = Engine.UpdateShipDataFromFittingList(currentShip, currentFit)
            ' Generate fitting data
            Call Me.GenerateFittingData()
            gbStatistics.Visible = True
        Else
            lblBCStatus.Text = "There seems to be no fittings for this loadout!" : StatusStrip1.Refresh()
        End If

        Call UpdateSlotColumns()
        Call UpdateSlotLayout()

    End Sub

#End Region

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
            For slot As Integer = 1 To currentShip.SubSlots
                Dim newSlot As New ListViewItem
                newSlot.Name = "16_" & slot
                newSlot.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.SubSlotColour))
                newSlot.ForeColor = Color.Black
                newSlot.Group = lvwSlots.Groups.Item("lvwgSubSlots")
                Call Me.AddUserColumns(currentShip.SubSlot(slot), newSlot)
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
            For slot As Integer = 1 To currentShip.SubSlots
                currentShip.SubSlot(slot) = Nothing
            Next
            currentShip.SlotCollection.Clear()
            currentShip.FleetSlotCollection.Clear()
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
        Dim shipName As String = lblShipType.Text
        Dim fittingName As String = lblLoadoutName.Text
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

    Private Sub ReorderModules()
        Dim subs, mods As New ArrayList
        For Each cMod As String In currentFit
            If ModuleLists.moduleListName.ContainsKey(cMod) = True Then
                If CType(ModuleLists.moduleList(ModuleLists.moduleListName(cMod)), ShipModule).SlotType = 16 Then
                    subs.Add(cMod)
                Else
                    mods.Add(cMod)
                End If
            Else
                mods.Add(cMod)
            End If
        Next
        ' Recreate the current fit
        currentFit.Clear()
        For Each cmod As String In subs
            currentFit.Add(cmod)
        Next
        For Each cmod As String In mods
            currentFit.Add(cmod)
        Next
        subs.Clear()
        mods.Clear()
    End Sub

#End Region

#Region "UI Routines"

    Private Sub mnuViewLoadout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewLoadout.Click
        Dim cLoadout As ContainerListViewItem = clvLoadouts.SelectedItems(0)
        Call GetBCShipLoadout(cLoadout)
    End Sub
    Private Sub lblLoadoutTopic_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblLoadoutTopic.LinkClicked
        Try
            Process.Start(SourceURL)
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please check your browser settings.", "Error Starting Web Browser", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub lblLoadoutTopic_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblLoadoutTopic.MouseEnter
        lblTopicAddress.Text = SourceURL
    End Sub

    Private Sub lblLoadoutTopic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblLoadoutTopic.MouseLeave
        lblTopicAddress.Text = ""
    End Sub

    Private Sub mnuCopyURL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyURL.Click
        Dim cLoadout As ContainerListViewItem = clvLoadouts.SelectedItems(0)
        Try
            Clipboard.SetText(SourceURL)
        Catch ex As Exception
            MessageBox.Show("There was an error copying data to the cliboard: " & ex.Message & ControlChars.CrLf & ControlChars.CrLf & "Please try again.", "Copy to Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

    Private Sub clvLoadouts_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles clvLoadouts.MouseDoubleClick
        If clvLoadouts.SelectedItems.Count > 0 Then
            Dim cLoadout As ContainerListViewItem = clvLoadouts.SelectedItems(0)
            Call GetBCShipLoadout(cLoadout)
        End If
    End Sub

#End Region

End Class

