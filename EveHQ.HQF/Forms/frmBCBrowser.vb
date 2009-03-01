Imports System.Drawing
Imports System.Windows.Forms
Imports System.Xml
Imports System.Net
Imports System.Text
Imports System.IO
Imports DotNetLib.Windows.Forms

Public Class frmBCBrowser

    Dim currentShip As Ship

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
            ' Add the additional columns
            For Each col As String In Settings.HQFSettings.UserSlotColumns
                If col.EndsWith("1") = True Then
                    colName = col.Substring(0, col.Length - 1)
                    Select Case colName
                        Case "Charge"
                            If shipMod.LoadedCharge IsNot Nothing Then
                                slotName.SubItems.Add(shipMod.LoadedCharge.Name)
                            Else
                                slotName.SubItems.Add("")
                            End If
                    End Select
                End If
            Next
        Else
            slotName.Text = "<Empty>"
            For Each col As String In Settings.HQFSettings.UserSlotColumns
                slotName.SubItems.Add("")
            Next
        End If
    End Sub

    Private Sub GetBCShipLoadouts()
        lblBCStatus.Text = "Retrieving " & currentShip.Name & " loadouts from BattleClinic..."
        Dim loadoutXML As New XmlDocument
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
                    nLoadout.SubItems(2).Text = loadout.Attributes("rating").Value
                    nLoadout.SubItems(3).Text = FormatDateTime(DateTime.Parse(loadout.Attributes("date").Value), DateFormat.ShortDate)
                Next
                clvLoadouts.EndUpdate()
                lblShipType.Text = currentShip.Name
                lblBCStatus.Text = "Update of loudouts completed!"
                pbShip.ImageLocation = "http://www.evehq.net/images/eve/shiptypes/128_128/" & currentShip.ID & ".png"
            Else
                lblBCStatus.Text = "There are no fittings available for this ship!"
            End If
        Catch e As Exception
            lblBCStatus.Text = "Unable to retrieve loadout information from BattleClinic!"
        End Try
    End Sub

    Private Sub GetBCShipLoadout(ByVal LoadoutName As String, ByVal LoadoutID As String)
        lblBCStatus.Text = "Retrieving " & LoadoutName & "(" & currentShip.Name & ") from BattleClinic..." : StatusStrip1.Refresh()
        Dim loadoutXML As New XmlDocument
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
            Dim loadoutList As XmlNodeList = loadoutXML.SelectNodes("/loadouts/race/ship/loadout/slot")
            If loadoutList.Count > 0 Then
                Dim fittingList As New ArrayList
                For Each loadout As XmlNode In loadoutList
                    If loadout.InnerText <> "0" Then
                        Select Case loadout.Attributes("type").Value
                            Case "high", "med", "lo", "rig", "drone"
                                fittingList.Add(CType(ModuleLists.moduleList(loadout.InnerText), ShipModule).Name)
                        End Select
                    End If
                Next
                lblBCStatus.Text = "Download of loadout completed!" : StatusStrip1.Refresh()
                pbShip.ImageLocation = "http://www.evehq.net/images/eve/shiptypes/128_128/" & currentShip.ID & ".png"
                Call Engine.UpdateShipDataFromFittingList(currentShip, fittingList)
            Else
                lblBCStatus.Text = "There seems to be no fittings for this loadout!" : StatusStrip1.Refresh()
            End If
            Call ClearShipSlots()
            Call UpdateSlotLayout()
            Call UpdateSlotColumns()
            Call UpdateSlotLayout()

        Catch e As Exception
            lblBCStatus.Text = "Unable to retrieve loadout information from BattleClinic!" : StatusStrip1.Refresh()
        End Try
    End Sub

    Private Sub mnuViewLoadout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewLoadout.Click
        Dim cLoadout As ContainerListViewItem = clvLoadouts.SelectedItems(0)
        Dim loadoutID As String = cLoadout.Tag.ToString
        Dim loadoutName As String = cLoadout.Text
        Call GetBCShipLoadout(loadoutName, loadoutID)
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
End Class