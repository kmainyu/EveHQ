Imports System.Windows.Forms
Imports System.Xml
Imports System.Reflection

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object

    Public Shared itemFlags As New SortedList
    Public Shared stations As New SortedList
    Public Shared NPCCorps As New ArrayList

#Region "Plug-in Interface Properties and Functions"
    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return mSetPlugInData
        End Get
        Set(ByVal value As Object)
            mSetPlugInData = value
        End Set
    End Property

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Try
            Return Me.LoadPlugIndata
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Assets"
        EveHQPlugIn.Description = "Displays and Analyses Character Assets"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "EveHQ Assets"
        EveHQPlugIn.RunAtStartup = True
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.plugin_icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function

    Public Function IGBService(ByVal IGBContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function

    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return New frmAssets
    End Function
#End Region

#Region "Plug-in Startup Routines"
    Private Function LoadPlugIndata() As Boolean
        If Me.CheckVersion = False Then
            Return False
        Else
            Call Me.LoadItemFlags()
            If Me.LoadStations = False Then
                Return False
                Exit Function
            End If
            If Me.LoadSolarSystems = False Then
                Return False
                Exit Function
            End If
            If Me.LoadNPCCorps = False Then
                Return False
                Exit Function
            End If
            Call Me.CheckForConqXMLFile()
            Return True
        End If
    End Function
    Private Sub LoadItemFlags()
        itemFlags.Add(0, "None")
        itemFlags.Add(1, "Wallet")
        itemFlags.Add(4, "Hangar")
        itemFlags.Add(5, "Cargo Bay")
        itemFlags.Add(6, "Briefcase")
        itemFlags.Add(7, "Skill")
        itemFlags.Add(8, "Reward")
        'itemFlags.Add(11, "flagSlotFirst")
        itemFlags.Add(11, "Low Slot 0")
        itemFlags.Add(12, "Low Slot 1")
        itemFlags.Add(13, "Low Slot 2")
        itemFlags.Add(14, "Low Slot 3")
        itemFlags.Add(15, "Low Slot 4")
        itemFlags.Add(16, "Low Slot 5")
        itemFlags.Add(17, "Low Slot 6")
        itemFlags.Add(18, "Low Slot 7")
        itemFlags.Add(19, "Mid Slot 0")
        itemFlags.Add(20, "Mid Slot 1")
        itemFlags.Add(21, "Mid Slot 2")
        itemFlags.Add(22, "Mid Slot 3")
        itemFlags.Add(23, "Mid Slot 4")
        itemFlags.Add(24, "Mid Slot 5")
        itemFlags.Add(25, "Mid Slot 6")
        itemFlags.Add(26, "Mid Slot 7")
        itemFlags.Add(27, "High Slot 0")
        itemFlags.Add(28, "High Slot 1")
        itemFlags.Add(29, "High Slot 2")
        itemFlags.Add(30, "High Slot 3")
        itemFlags.Add(31, "High Slot 4")
        itemFlags.Add(32, "High Slot 5")
        itemFlags.Add(33, "High Slot 6")
        itemFlags.Add(34, "High Slot 7")
        itemFlags.Add(35, "Fixed Slot")
        'itemFlags.Add(35, "SlotLast")
        itemFlags.Add(56, "Capsule")
        itemFlags.Add(57, "Pilot")
        itemFlags.Add(61, "Skill In Training")
        itemFlags.Add(62, "Corp Deliveries")
        itemFlags.Add(63, "Locked")
        itemFlags.Add(64, "Unlocked")
        itemFlags.Add(70, "Corp Office Slot 1")
        itemFlags.Add(71, "Corp Office Slot 2")
        itemFlags.Add(72, "Corp Office Slot 3")
        itemFlags.Add(73, "Corp Office Slot 4")
        itemFlags.Add(74, "Corp Office Slot 5")
        itemFlags.Add(75, "Corp Office Slot 6")
        itemFlags.Add(76, "Corp Office Slot 7")
        itemFlags.Add(77, "Corp Office Slot 8")
        itemFlags.Add(78, "Corp Office Slot 9")
        itemFlags.Add(79, "Corp Office Slot 10")
        itemFlags.Add(80, "Corp Office Slot 11")
        itemFlags.Add(81, "Corp Office Slot 12")
        itemFlags.Add(82, "Corp Office Slot 13")
        itemFlags.Add(83, "Corp Office Slot 14")
        itemFlags.Add(84, "Corp Office Slot 15")
        itemFlags.Add(85, "Corp Office Slot 16")
        itemFlags.Add(86, "Bonus")
        itemFlags.Add(87, "Drone Bay")
        itemFlags.Add(88, "Booster")
        itemFlags.Add(89, "Implant")
        itemFlags.Add(90, "Ship Hangar")
        itemFlags.Add(91, "Ship Offline")
        itemFlags.Add(92, "Rig Slot 0")
        itemFlags.Add(93, "Rig Slot 1")
        itemFlags.Add(94, "Rig Slot 2")
        itemFlags.Add(95, "Rig Slot 3")
        itemFlags.Add(96, "Rig Slot 4")
        itemFlags.Add(97, "Rig Slot 5")
        itemFlags.Add(98, "Rig Slot 6")
        itemFlags.Add(99, "Rig Slot 7")
        itemFlags.Add(100, "Factory Operation")
        itemFlags.Add(116, "Corp SAG 2")
        itemFlags.Add(117, "Corp SAG 3")
        itemFlags.Add(118, "Corp SAG 4")
        itemFlags.Add(119, "Corp SAG 5")
        itemFlags.Add(120, "Corp SAG 6")
        itemFlags.Add(121, "Corp SAG 7")
        itemFlags.Add(122, "Secondary Storage")
    End Sub
    Private Function LoadStations() As Boolean
        ' Load the Station Data From the mapDenormalize table
        Try
            Dim strSQL As String = "SELECT * FROM mapDenormalize WHERE groupID=15;"
            Dim locationData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If locationData IsNot Nothing Then
                If locationData.Tables(0).Rows.Count > 0 Then
                    For Each locationRow As DataRow In locationData.Tables(0).Rows
                        Dim newLocation As New Location
                        newLocation.locationID = CStr(locationRow.Item("itemID"))
                        newLocation.locationName = CStr(locationRow.Item("itemName"))
                        newLocation.systemID = CStr(locationRow.Item("solarSystemID"))
                        newLocation.constID = CStr(locationRow.Item("constellationID"))
                        newLocation.regionID = CStr(locationRow.Item("regionID"))
                        newLocation.systemSec = CStr(locationRow.Item("security"))
                        stations.Add(newLocation.locationID, newLocation)
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Station Data for Assets Plugin" & ControlChars.CrLf & ex.Message, "Assets Plug-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadSolarSystems() As Boolean
        Dim strSQL As String = "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName"
        strSQL &= " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;"
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    Dim cSystem As SolarSystem = New SolarSystem
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        cSystem = New SolarSystem
                        cSystem.ID = CInt(systemData.Tables(0).Rows(solar).Item("solarSystemID"))
                        cSystem.Name = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemName"))
                        cSystem.Region = CStr(systemData.Tables(0).Rows(solar).Item("regionName"))
                        cSystem.Constellation = CStr(systemData.Tables(0).Rows(solar).Item("constellationName"))
                        cSystem.Security = CDbl(systemData.Tables(0).Rows(solar).Item("security"))
                        stations.Add(CStr(cSystem.ID), cSystem)
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading System Data for Assets Plugin" & ControlChars.CrLf & ex.Message, "Assets Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadNPCCorps() As Boolean
        ' Load the Station Data From the mapDenormalize table
        NPCCorps.Clear()
        Try
            Dim strSQL As String = "SELECT corporationID FROM crpNPCCorporations;"
            Dim corpData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If corpData IsNot Nothing Then
                If corpData.Tables(0).Rows.Count > 0 Then
                    For Each corpRow As DataRow In corpData.Tables(0).Rows
                        NPCCorps.Add(CStr(corpRow.Item("corporationID")))
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading NPC Corporation Data for Assets Plugin" & ControlChars.CrLf & ex.Message, "Assets Plug-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Sub CheckForConqXMLFile()
        ' Check for the Conquerable XML file in the cache
        Dim fileName As String = EveHQ.Core.HQ.cacheFolder & "\EVEHQAPI_18.xml"
        Dim stationXML As New XmlDocument
        If My.Computer.FileSystem.FileExists(fileName) = True Then
            stationXML.Load(fileName)
            Call ParseConquerableXML(stationXML)
        End If
    End Sub
    Private Function CheckVersion() As Boolean
        Dim thisAssembly As [Assembly] = System.Reflection.Assembly.GetExecutingAssembly()
        ' Display the set of assemblies our assemblies references.
        Dim refAssemblies As AssemblyName
        For Each refAssemblies In thisAssembly.GetReferencedAssemblies()
            If refAssemblies.Name = "EveHQ.Core" Then
                If CompareVersions(refAssemblies.Version.ToString, "1.6.6.0") = True Then
                    Dim msg As String = "This plug-in requires version 1.6.6.0 or greater of EveHQ." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Please check for any updates that are available."
                    MessageBox.Show(msg, "EveHQ Assets", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                Else
                    Return True
                End If
            End If
        Next
    End Function
    Private Function CompareVersions(ByVal thisVersion As String, ByVal requiredVersion As String) As Boolean
        Dim localVers() As String = thisVersion.Split(CChar("."))
        Dim remoteVers() As String = requiredVersion.Split(CChar("."))
        Dim requiresUpdate As Boolean = False
        For ver As Integer = 0 To 3
            If CInt(remoteVers(ver)) <> CInt(localVers(ver)) Then
                If CInt(remoteVers(ver)) > CInt(localVers(ver)) Then
                    requiresUpdate = True
                    Exit For
                Else
                    requiresUpdate = False
                    Exit For
                End If
            End If
        Next
        Return requiresUpdate
    End Function
    Public Shared Sub ParseConquerableXML(ByVal stationXML As XmlDocument)
        Dim locList As XmlNodeList
        Dim loc As XmlNode
        Dim stationID As String = ""
        locList = stationXML.SelectNodes("/eveapi/result/rowset/row")
        If locList.Count > 0 Then
            For Each loc In locList
                stationID = (loc.Attributes.GetNamedItem("stationID").Value)
                If CDbl(stationID) >= 61000001 And CDbl(stationID) <= 61999999 Then
                    ' This is an outpost so needs adding to the station list if it's not there
                    If PlugInData.stations.Contains(stationID) = False Then
                        Dim station As New Location
                        station.locationID = stationID
                        station.locationName = (loc.Attributes.GetNamedItem("stationName").Value)
                        station.systemID = (loc.Attributes.GetNamedItem("solarSystemID").Value)
                        Dim system As SolarSystem = CType(PlugInData.stations(station.systemID), SolarSystem)
                        station.locationName &= " (" & system.Name & ", " & system.Region & ")"
                        PlugInData.stations.Add(station.locationID, station)
                    Else
                        Dim station As Location = CType(PlugInData.stations(stationID), Assets.Location)
                        station.systemID = (loc.Attributes.GetNamedItem("solarSystemID").Value)
                        Dim system As SolarSystem = CType(PlugInData.stations(station.systemID), SolarSystem)
                        station.locationName &= " (" & system.Name & ", " & system.Region & ")"
                    End If
                Else
                    ' This is a conquerable station so the station name needs amending
                    Dim station As Location = CType(PlugInData.stations(stationID), Assets.Location)
                    station.systemID = (loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As SolarSystem = CType(PlugInData.stations(station.systemID), SolarSystem)
                    station.locationName = (loc.Attributes.GetNamedItem("stationName").Value) & " (" & system.Name & ", " & system.Region & ")"
                End If
            Next
        End If
    End Sub
#End Region
End Class
