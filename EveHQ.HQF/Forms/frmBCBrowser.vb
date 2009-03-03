Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports DotNetLib.Windows.Forms

Public Class frmBCBrowser

    Dim currentShip As Ship
    Dim BCLoadoutCache As String = EveHQ.Core.HQ.appDataFolder & "\BCLoadoutCache"

    Public Property ShipType() As Ship
        Get
            Return currentShip
        End Get
        Set(ByVal value As Ship)
            currentShip = value
            Call GetBCShipLoadouts()
        End Set
    End Property

    Private Sub frmBCBrowser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Create the fittings cache if it doesn't exist!
        Try
            If My.Computer.FileSystem.DirectoryExists(BCLoadoutCache) = False Then
                My.Computer.FileSystem.CreateDirectory(BCLoadoutCache)
            End If
        Catch ex As Exception
            MessageBox.Show("Unable to create the Loadout cache folder. Caching will be disabled which may affect loadout downloads.", "Error Creating Folder", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub

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

    Private Sub GetBCShipLoadouts()

        ' Check if the loadout list is in the cache
        lblBCStatus.Text = "Checking Loadout Cache..." : StatusStrip1.Refresh()
        Dim loadoutXML As New XmlDocument
        Dim UseCacheFile As Boolean = False
        If My.Computer.FileSystem.DirectoryExists(BCLoadoutCache) Then
            If My.Computer.FileSystem.FileExists(BCLoadoutCache & "\" & currentShip.ID & ".xml") Then
                ' Open the file and check the cache time
                loadoutXML.Load(BCLoadoutCache & "\" & currentShip.ID & ".xml")
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
            Dim remoteURL As String = "http://www.battleclinic.com/eve_online/ship_loadout_feed.php?typeID=" & currentShip.ID
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
            pbShip.ImageLocation = "http://www.evehq.net/images/eve/shiptypes/128_128/" & currentShip.ID & ".png"
            ' Save the XML into the cache
            If UseCacheFile = False Then
                loadoutXML.Save(BCLoadoutCache & "\" & currentShip.ID & ".xml")
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
            If My.Computer.FileSystem.FileExists(BCLoadoutCache & "\" & LoadoutID & ".xml") Then
                ' Open the file and check the cache time
                loadoutXML.Load(BCLoadoutCache & "\" & LoadoutID & ".xml")
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
            Dim remoteURL As String = "http://www.battleclinic.com/eve_online/ship_loadout_feed.php?typeID=" & LoadoutID
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
            Dim fittingList, moduleList, ammoList As New ArrayList
            For Each loadout As XmlNode In loadoutList
                If loadout.InnerText <> "0" Then
                    Select Case loadout.Attributes("type").Value
                        Case "high", "med", "lo", "rig", "drone"
                            moduleList.Add(loadout.InnerText)
                        Case "ammo"
                            ammoList.Add(loadout.InnerText)
                    End Select
                End If
            Next
            ' Try and match the ammo to the modules
            Dim RevisedFit As String = ""
            For Each fittedMod As String In moduleList
                Dim fModule As ShipModule = CType(ModuleLists.moduleList(fittedMod), ShipModule)
                RevisedFit = fModule.Name
                If fModule.Charges.Count <> 0 Then
                    For Each ammo As String In ammoList
                        If fModule.Charges.Contains(CType(ModuleLists.moduleList(ammo), ShipModule).DatabaseGroup) Then
                            RevisedFit &= "," & CType(ModuleLists.moduleList(ammo), ShipModule).Name
                            Exit For
                        End If
                    Next
                    fittingList.Add(RevisedFit)
                Else
                    fittingList.Add(fModule.Name)
                End If
            Next
            lblLoadoutName.Text = LoadoutName : lblLoadoutName.Visible = True : lblLoadoutNameLbl.Visible = True
            lblLoadoutAuthor.Text = LoadoutAuthor : lblLoadoutAuthor.Visible = True : lblLoadoutAuthorLbl.Visible = True
            lblLoadoutScore.Text = LoadoutScore : lblLoadoutScore.Visible = True : lblLoadoutScoreLbl.Visible = True
            lblLoadoutDate.Text = LoadoutDate : lblLoadoutDate.Visible = True : lblLoadoutDateLbl.Visible = True
            lblLoadoutTopic.Text = "BattleClinic Topic" : lblLoadoutTopic.Visible = True : LblLoadoutTopicLbl.Visible = True
            lblLoadoutTopic.Tag = LoadoutTopic
            lblBCStatus.Text = "Download of loadout completed!" : StatusStrip1.Refresh()
            ' Save the XML into the cache
            If UseCacheFile = False Then
                loadoutXML.Save(BCLoadoutCache & "\" & LoadoutID & ".xml")
            End If
            pbShip.ImageLocation = "http://www.evehq.net/images/eve/shiptypes/128_128/" & currentShip.ID & ".png"
            currentShip = Engine.UpdateShipDataFromFittingList(currentShip, fittingList)
        Else
            lblBCStatus.Text = "There seems to be no fittings for this loadout!" : StatusStrip1.Refresh()
        End If

        Call UpdateSlotColumns()
        Call UpdateSlotLayout()

    End Sub

    Private Sub mnuViewLoadout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewLoadout.Click
        Dim cLoadout As ContainerListViewItem = clvLoadouts.SelectedItems(0)
        Call GetBCShipLoadout(cLoadout)
    End Sub

#Region "Clearing routines"
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
#End Region

    Private Sub lblLoadoutTopic_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblLoadoutTopic.LinkClicked
        Try
            Process.Start("http://www.battleclinic.com/forum/index.php/topic," & lblLoadoutTopic.Tag.ToString & ".0.html")
        Catch ex As Exception
            MessageBox.Show("Unable to start default web browser. Please check your browser settings.", "Error Starting Web Browser", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub lblLoadoutTopic_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblLoadoutTopic.MouseEnter
        lblTopicAddress.Text = "http://www.battleclinic.com/forum/index.php/topic," & lblLoadoutTopic.Tag.ToString & ".0.html"
    End Sub

    Private Sub lblLoadoutTopic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblLoadoutTopic.MouseLeave
        lblTopicAddress.Text = ""
    End Sub

End Class