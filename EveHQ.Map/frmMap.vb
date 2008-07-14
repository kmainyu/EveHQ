' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml

Public Class frmMap
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object

    Dim EnableJumps As Boolean
    Dim MapContext As Net.HttpListenerContext

    Dim bHaveMouse, bHaveScan As Boolean
    Dim ptOriginal As Point
    Dim ptLast As Point
    Dim maxX1 As Double = -5.8E+17
    Dim maxX2 As Double = 4.2E+17
    Dim maxY1 As Double = -5.0E+17
    Dim maxY2 As Double = 5.0E+17
    Dim maxX As Double = maxX2 - maxX1
    Dim maxY As Double = maxY2 - maxY1
    Dim zoomLevel As Double = 100
    Dim mapX1, mapX2, mapY1, mapY2 As Double
    Dim IGBX1, IGBX2, IGBY1, IGBY2 As Double
    Dim Waypoints As New ArrayList
    Dim WaypointRoutes As New SortedList
    Dim WaypointSet As New SortedList
    Dim WaypointSets As New SortedList
    Dim Exclusions As New SortedList
    Dim FullRoute As New ArrayList
    Const ly As Double = 9.46E+15
    Public Shared maxdist As Double
    Public Shared maxgate As Double
    Public Shared maxjump As Double
    Public Shared mingate As Double
    Public Shared minjump As Double
    Public ranges As Double()
    Public ships As String()
    Public fuel As Double()
    Private csPilot As EveHQ.Core.Pilot
    Private lastAlgo As Integer = -1

    Dim startTime, endTime As DateTime
    Dim timeTaken As TimeSpan

    Dim EveMap As Drawing.Bitmap
    Dim IGBMap As Drawing.Bitmap
    Dim Mapdata(,) As String
    Dim IGBMapdata(,) As String
    Dim mapSize As Integer = 500
    Dim IGBMapSize As Integer = 500

    ' New Stuff
    Dim mapFolder As String = ""
    Dim mapCacheFolder As String = ""
    Dim UseSerializableData As Boolean = False

    Public Shared NPCCorpList As SortedList = New SortedList
    Public Shared NPCDivID As SortedList = New SortedList
    Public Shared eveNames As SortedList = New SortedList
    Public Shared RegionID As SortedList = New SortedList
    Public Shared RegionName As SortedList = New SortedList
    Public Shared ConstellationID As SortedList = New SortedList
    Public Shared ConstellationName As SortedList = New SortedList
    Public Shared AgentID As SortedList = New SortedList
    Public Shared AgentName As SortedList = New SortedList
    Public Shared ServiceList As SortedList = New SortedList
    Public Shared OperationList As SortedList = New SortedList
    Public Shared sovSystemsName As SortedList = New SortedList
    Public Shared sovSystemsID As SortedList = New SortedList
    Public Shared FactionList As SortedList = New SortedList
    Public Shared FactionName As SortedList = New SortedList
    Public Shared AllianceName As SortedList = New SortedList
    Public Shared AllianceID As SortedList = New SortedList
    Public Shared StationList As SortedList = New SortedList
    Public Shared StationName As SortedList = New SortedList
    Public Shared CSStationList As SortedList = New SortedList
    Public Shared CSStationName As SortedList = New SortedList
    Public Shared StationTypes As SortedList = New SortedList

#Region "Plug-in Interface Functions"
    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Try
            mapFolder = (EveHQ.Core.HQ.appDataFolder & "\EveHQMap").Replace("\\", "\")
            ' Check for cache folder
            mapCacheFolder = mapFolder & "\Cache"
            If My.Computer.FileSystem.DirectoryExists(mapCacheFolder) = True Then
                UseSerializableData = True
                Return Me.LoadSerializedData
            Else
                UseSerializableData = False
                Return Me.LoadDataFromDatabase
            End If
            Return True
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Map Tool"
        EveHQPlugIn.Description = "Route Planning and Solar System Information Tool"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "Map Tool"
        EveHQPlugIn.RunAtStartup = True
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.plugin_icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function

    Public Function IGBService(ByVal IGBContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        MapContext = IGBContext
        Return Me.GenerateIGBCode
    End Function

    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return Me
    End Function

    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return mSetPlugInData
        End Get
        Set(ByVal value As Object)
            mSetPlugInData = value
        End Set
    End Property
#End Region

#Region "Data Loading Routines"
    Private Function LoadDataFromDatabase() As Boolean
        If Me.LoadSystems() = False Then
            ReportError("LoadSystems failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadGates() = False Then
            ReportError("LoadGates failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.GenerateJumps() = False Then
            ReportError("Generate Jumps failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadNames() = False Then
            ReportError("LoadNames failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadNPCCorps() = False Then
            ReportError("LoadNPCCorps failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadSov() = False Then
            ReportError("LoadSov failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadFactions() = False Then
            ReportError("LoadFactions failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadAlliances() = False Then
            ReportError("LoadAlliances failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadStations() = False Then
            ReportError("LoadStations failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadRegions() = False Then
            ReportError("LoadRegions failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadConstellations() = False Then
            ReportError("LoadConstellations failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadOperationData() = False Then
            ReportError("LoadOperationData failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadServiceData() = False Then
            ReportError("LoadServiceData failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadAgents() = False Then
            ReportError("LoadAgents failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadConq() = False Then
            ReportError("LoadConq failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadStationTypes() = False Then
            ReportError("LoadStationTypes failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadNPCDiv() = False Then
            ReportError("LoadNPCDiv failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadCB() = False Then
            ReportError("Loadcb failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Function LoadSerializedData() As Boolean
        Dim s As New FileStream(mapCacheFolder & "\Systems.txt", FileMode.Open)
        Dim f As BinaryFormatter = New BinaryFormatter
        EveHQ.Core.HQ.SystemsID = CType(f.Deserialize(s), SortedList)
        EveHQ.Core.HQ.SystemsName = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Regions.txt", FileMode.Open)
        f = New BinaryFormatter
        RegionID = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cSystem As Region In RegionID.Values
            EveHQ.Core.HQ.SystemsName.Add(cSystem.regionName, cSystem)
        Next

        s = New FileStream(mapCacheFolder & "\Constellations.txt", FileMode.Open)
        f = New BinaryFormatter
        ConstellationID = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cSystem As Constellation In ConstellationID.Values
            ConstellationName.Add(cSystem.constellationName, cSystem)
        Next

        s = New FileStream(mapCacheFolder & "\Names.txt", FileMode.Open)
        f = New BinaryFormatter
        eveNames = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\NPCCorps.txt", FileMode.Open)
        f = New BinaryFormatter
        NPCCorpList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Sovereignty.txt", FileMode.Open)
        f = New BinaryFormatter
        sovSystemsID = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cSystem As SolarSystem In sovSystemsID.Values
            sovSystemsName.Add(cSystem.Name, cSystem)
        Next

        s = New FileStream(mapCacheFolder & "\Factions.txt", FileMode.Open)
        f = New BinaryFormatter
        FactionList = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cFaction As Faction In FactionList.Values
            FactionName.Add(cFaction.factionName, cFaction)
        Next

        s = New FileStream(mapCacheFolder & "\Alliances.txt", FileMode.Open)
        f = New BinaryFormatter
        AllianceID = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cAlliance As Alliance In AllianceID.Values
            AllianceName.Add(cAlliance.name, cAlliance)
        Next

        s = New FileStream(mapCacheFolder & "\Stations.txt", FileMode.Open)
        f = New BinaryFormatter
        StationList = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cStation As Station In StationList.Values
            StationName.Add(cStation.stationName, cStation)
        Next

        s = New FileStream(mapCacheFolder & "\Operations.txt", FileMode.Open)
        f = New BinaryFormatter
        OperationList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Services.txt", FileMode.Open)
        f = New BinaryFormatter
        ServiceList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Agents.txt", FileMode.Open)
        f = New BinaryFormatter
        AgentID = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cAgent As Agent In AgentID.Values
            AgentName.Add(cAgent.agentName, cAgent)
        Next

        s = New FileStream(mapCacheFolder & "\Conquerables.txt", FileMode.Open)
        f = New BinaryFormatter
        CSStationList = CType(f.Deserialize(s), SortedList)
        s.Close()
        For Each cStation As ConqStat In CSStationList.Values
            CSStationName.Add(cStation.stationName, cStation)
        Next

        s = New FileStream(mapCacheFolder & "\StationTypes.txt", FileMode.Open)
        f = New BinaryFormatter
        StationTypes = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\NPCDivs.txt", FileMode.Open)
        f = New BinaryFormatter
        NPCDivID = CType(f.Deserialize(s), SortedList)
        s.Close()

        Return True
    End Function
    Public Sub ReportError(ByVal Note As String, ByVal Title As String)
        MessageBox.Show(Note, Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Public Function LoadSystems() As Boolean
        Dim strSQL As String = "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapSolarSystems.allianceID, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName"
        strSQL &= " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;"
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    EveHQ.Core.HQ.SystemsName.Clear()
                    EveHQ.Core.HQ.SystemsID.Clear()
                    Dim cSystem As SolarSystem = New SolarSystem
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        cSystem = New SolarSystem
                        cSystem.ID = CInt(systemData.Tables(0).Rows(solar).Item("solarSystemID")) - 30000000
                        cSystem.Name = systemData.Tables(0).Rows(solar).Item("solarSystemName")
                        cSystem.Region = systemData.Tables(0).Rows(solar).Item("regionName")
                        cSystem.Constellation = systemData.Tables(0).Rows(solar).Item("constellationName")
                        cSystem.x = systemData.Tables(0).Rows(solar).Item("x")
                        cSystem.y = systemData.Tables(0).Rows(solar).Item("y")
                        cSystem.z = systemData.Tables(0).Rows(solar).Item("z")
                        cSystem.Security = systemData.Tables(0).Rows(solar).Item("security")
                        cSystem.EveSec = Math.Max(Int((cSystem.Security * 10) + 0.5) / 10, 0)
                        cSystem.Flag = False
                        EveHQ.Core.HQ.SystemsID.Add(cSystem.ID, cSystem)
                        EveHQ.Core.HQ.SystemsName.Add(cSystem.Name, cSystem)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error generating the solar system data. The error was: " & ControlChars.CrLf & e.Message, "Map Tool Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadGates() As Boolean
        Dim strSQL As String = "SELECT * FROM mapSolarSystemJumps"
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    Dim fromSystem, toSystem As Integer
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        Dim cSystem As SolarSystem = New SolarSystem
                        fromSystem = CInt(systemData.Tables(0).Rows(solar).Item("fromsolarSystemID")) - 30000000
                        toSystem = CInt(systemData.Tables(0).Rows(solar).Item("tosolarSystemID")) - 30000000
                        cSystem = EveHQ.Core.HQ.SystemsID(CInt(fromSystem))
                        cSystem.Gates.Add(EveHQ.Core.HQ.SystemsID(toSystem))
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error generating the system gate data. The error was: " & ControlChars.CrLf & e.Message, "Map Tool Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function GenerateJumps() As Boolean
        Dim sysCount As Integer = 0
        Dim solar0 As SolarSystem = Nothing
        frmMap.mingate = 0
        frmMap.maxgate = 1
        Try
            Dim solar1 As SolarSystem
            For Each solar1 In EveHQ.Core.HQ.SystemsName.Values
                If Not solar1.Flag Then
                    Dim solar2 As SolarSystem
                    For Each solar2 In EveHQ.Core.HQ.SystemsName.Values
                        solar2.Flag = False
                    Next
                    frmMap.SetFlags(solar1)
                End If
                Dim sList As New SortedList
                Dim solar3 As SolarSystem
                For Each solar3 In EveHQ.Core.HQ.SystemsName.Values
                    Dim cDist As Double = frmMap.Distance(solar1, solar3)
                    'If (((cDist <= 14.625) AndAlso (Not solar1 Is solar3)) AndAlso ((solar3.EveSec <= 0.4) AndAlso solar3.Flag)) Then
                    If (((cDist <= 14.625) AndAlso (Not solar1 Is solar3)) AndAlso solar3.Flag) Then
                        sList.Add(solar3.ID, solar3)
                    End If
                Next
                If ((sList.Count > 0) AndAlso (Not solar1 Is solar0)) Then
                    solar0 = solar1
                End If
                Dim solar4 As SolarSystem
                For Each solar4 In sList.Values
                    solar1.Jumps.Add(solar4)
                Next
            Next
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error generating the jump drive data. The error was: " & ControlChars.CrLf & e.Message, "Map Tool Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadNames() As Boolean
        Dim strSQL As String = "SELECT * FROM eveNames ORDER BY itemID;"
        Dim NameData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If NameData IsNot Nothing Then
                If NameData.Tables(0).Rows.Count > 0 Then
                    eveNames.Clear()
                    For a As Integer = 0 To NameData.Tables(0).Rows.Count - 1
                        Dim cName As evename = New evename
                        cName.itemID = NameData.Tables(0).Rows(a).Item("itemID")
                        cName.itemName = NameData.Tables(0).Rows(a).Item("itemName")
                        cName.Flag = False
                        eveNames.Add(cName.itemID, cName)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
        Catch e As Exception
            MessageBox.Show("There was an error loading the data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        Return True
    End Function
    Public Function LoadNPCCorps() As Boolean
        Dim strSQL As String = "SELECT * FROM crpNPCCorporations ORDER BY corporationID;"
        Dim NPCCData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If NPCCData IsNot Nothing Then
                If NPCCData.Tables(0).Rows.Count > 0 Then
                    NPCCorpList.Clear()
                    For a As Integer = 0 To NPCCData.Tables(0).Rows.Count - 1
                        Dim cCorp As NPCCorp = New NPCCorp
                        cCorp.CorporationID = NPCCData.Tables(0).Rows(a).Item("corporationID")
                        cCorp.factionID = NPCCData.Tables(0).Rows(a).Item("factionID")
                        cCorp.Flag = False
                        NPCCorpList.Add(cCorp.CorporationID, cCorp)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the faction data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadSov() As Boolean
        Try
            ' Dimension variables
            Dim SystemDetails As XmlNodeList
            Dim SysNode As XmlNode
            ' Get the Sovereignty data
            Dim XMLDoc As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Sovereignty)
            SystemDetails = XMLDoc.SelectNodes("/eveapi/result/rowset/row")
            For Each SysNode In SystemDetails
                Dim solar As New SolarSystem
                solar.ID = SysNode.Attributes.GetNamedItem("solarSystemID").Value
                solar.AllianceID = SysNode.Attributes.GetNamedItem("allianceID").Value
                solar.constellationSovereignty = SysNode.Attributes.GetNamedItem("constellationSovereignty").Value
                solar.sovereigntyLevel = SysNode.Attributes.GetNamedItem("sovereigntyLevel").Value
                solar.FactionID = SysNode.Attributes.GetNamedItem("factionID").Value
                solar.Name = SysNode.Attributes.GetNamedItem("solarSystemName").Value
                solar.Flag = False
                sovSystemsName.Add(solar.Name, solar)
                sovSystemsID.Add(solar.ID, solar)
            Next
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    Public Function LoadFactions() As Boolean
        Dim strSQL As String = "SELECT * FROM chrFactions ORDER BY FactionID;"
        Dim FactionData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If FactionData IsNot Nothing Then
                If FactionData.Tables(0).Rows.Count > 0 Then
                    FactionList.Clear()
                    Dim cFaction As Faction = New Faction
                    For solar As Integer = 0 To FactionData.Tables(0).Rows.Count - 1
                        If FactionData.Tables(0).Rows(solar).Item("factionName") <> "Unknown" Then
                            cFaction = New Faction
                            cFaction.factionID = FactionData.Tables(0).Rows(solar).Item("factionID")
                            cFaction.factionName = FactionData.Tables(0).Rows(solar).Item("factionName")
                            cFaction.description = FactionData.Tables(0).Rows(solar).Item("description")
                            cFaction.raceID = FactionData.Tables(0).Rows(solar).Item("raceIDs")
                            cFaction.solarSystemID = FactionData.Tables(0).Rows(solar).Item("solarSystemID")
                            cFaction.CorporationID = FactionData.Tables(0).Rows(solar).Item("corporationID")
                            cFaction.sizeFactor = FactionData.Tables(0).Rows(solar).Item("sizeFactor")
                            cFaction.stationCount = FactionData.Tables(0).Rows(solar).Item("stationCount")
                            cFaction.StationSystemCount = FactionData.Tables(0).Rows(solar).Item("stationSystemCount")

                        Else
                            cFaction = New Faction
                            cFaction.factionID = FactionData.Tables(0).Rows(solar).Item("factionID")
                            cFaction.factionName = FactionData.Tables(0).Rows(solar).Item("factionName")
                            cFaction.description = "Unknown"
                            cFaction.raceID = FactionData.Tables(0).Rows(solar).Item("raceIDs")
                            cFaction.solarSystemID = "Unknown"
                            cFaction.CorporationID = "Unknown"
                            cFaction.sizeFactor = "Unknown"
                            cFaction.stationCount = "Unknown"
                            cFaction.StationSystemCount = "Unknown"
                        End If
                        cFaction.Flag = False
                        FactionName.Add(cFaction.factionName, cFaction)
                        FactionList.Add(cFaction.factionID, cFaction)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the faction data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadAlliances() As Boolean
        Try
            ' Dimension variables
            Dim AllyDetails As XmlNodeList
            Dim AllyNode As XmlNode
            AllianceID.Clear()
            AllianceName.Clear()
            ' Get the alliance data
            Dim XMLDoc As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AllianceList)
            AllyDetails = XMLDoc.SelectNodes("/eveapi/result/rowset/row")
            Dim ind As Integer = 0
            For Each AllyNode In AllyDetails
                Dim Ally As New Alliance
                Ally.ID = AllyNode.Attributes.GetNamedItem("allianceID").Value
                Ally.name = AllyNode.Attributes.GetNamedItem("name").Value
                Ally.shortName = AllyNode.Attributes.GetNamedItem("shortName").Value
                Ally.executorCorpID = AllyNode.Attributes.GetNamedItem("executorCorpID").Value
                Ally.memberCount = AllyNode.Attributes.GetNamedItem("memberCount").Value
                Ally.startDate = AllyNode.Attributes.GetNamedItem("startDate").Value
                Ally.Flag = False
                AllianceName.Add(Ally.name, Ally)
                AllianceID.Add(ind, Ally)
                ind = ind + 1
            Next
            ind = 0
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    Public Function LoadStations()

        Dim strSQL As String = "SELECT * FROM staStations ORDER BY stationID;"
        Dim StationData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        Try
            If StationData IsNot Nothing Then
                If StationData.Tables(0).Rows.Count > 0 Then
                    StationList.Clear()
                    For stat As Integer = 0 To StationData.Tables(0).Rows.Count - 1
                        Dim cstation As Station = New Station
                        Dim skip As Boolean = False
                        cstation.stationID = CInt(StationData.Tables(0).Rows(stat).Item("stationID"))

                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("security")) = False Then
                            cstation.security = StationData.Tables(0).Rows(stat).Item("security")
                        Else
                            cstation.security = " "
                        End If
                        'cstation.dockingCostPerVolume = Nullcheck(StationData.Tables(0).Rows(stat).Item("dockingCostPerVolume")
                        'cstation.maxShipVolumeDockable = Nullcheck(StationData.Tables(0).Rows(stat).Item("maxShipVolumeDockable")
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("operationID")) = False Then
                            cstation.operationID = StationData.Tables(0).Rows(stat).Item("operationID")
                        Else
                            cstation.operationID = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("stationTypeID")) = False Then
                            cstation.stationTypeID = StationData.Tables(0).Rows(stat).Item("stationTypeID")
                        Else
                            cstation.stationTypeID = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("corporationID")) = False Then
                            cstation.corporationID = StationData.Tables(0).Rows(stat).Item("corporationID")
                        Else
                            cstation.corporationID = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("solarSystemID")) = False Then
                            cstation.solarSystemID = CInt(StationData.Tables(0).Rows(stat).Item("solarSystemID")) - 30000000
                        Else
                            cstation.solarSystemID = 0
                            skip = True
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("constellationID")) = False Then
                            cstation.constellationID = StationData.Tables(0).Rows(stat).Item("constellationID")
                        Else
                            cstation.constellationID = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("regionID")) = False Then
                            cstation.regionID = StationData.Tables(0).Rows(stat).Item("regionID")
                        Else
                            cstation.regionID = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("stationName")) = False Then
                            cstation.stationName = StationData.Tables(0).Rows(stat).Item("stationName")
                        Else
                            cstation.stationName = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("x")) = False Then
                            cstation.x = StationData.Tables(0).Rows(stat).Item("x")
                        Else
                            cstation.x = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("y")) = False Then
                            cstation.y = StationData.Tables(0).Rows(stat).Item("y")
                        Else
                            cstation.y = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("z")) = False Then
                            cstation.z = StationData.Tables(0).Rows(stat).Item("z")
                        Else
                            cstation.z = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("reprocessingEfficiency")) = False Then
                            cstation.reprocessingEfficiency = StationData.Tables(0).Rows(stat).Item("reprocessingEfficiency")
                        Else
                            cstation.reprocessingEfficiency = " "
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("reprocessingStationsTake")) = False Then
                            cstation.reprocessingStationsTake = StationData.Tables(0).Rows(stat).Item("reprocessingStationsTake")
                        Else
                            cstation.reprocessingStationsTake = " "
                        End If

                        'cstation.reprocessingHangarFlag = StationData.Tables(0).Rows(stat).Item("reprocessingHangarFlag")
                        'cstation.capitalStation = StationData.Tables(0).Rows(stat).Item("capitalStation")
                        'cstation.ownershipDateTime = StationData.Tables(0).Rows(stat).Item("ownershipDateTime")
                        'cstation.upgradeLevel = StationData.Tables(0).Rows(stat).Item("upgradeLevel")
                        'cstation.customServiceMask = StationData.Tables(0).Rows(stat).Item("customServiceMask")
                        cstation.Flag = False
                        If skip = False Then
                            StationList.Add(cstation.stationID, cstation)
                            StationName.Add(cstation.stationName, cstation)
                        End If
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the Station data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadRegions() As Boolean

        Dim strSQL As String = "SELECT * FROM mapRegions ORDER BY regionID;"
        Dim RegionData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If RegionData IsNot Nothing Then
                If RegionData.Tables(0).Rows.Count > 0 Then
                    RegionID.Clear()
                    RegionName.Clear()

                    For Rstat As Integer = 0 To RegionData.Tables(0).Rows.Count - 1
                        Dim cRegion As Region = New Region
                        cRegion.RegionID = CInt(RegionData.Tables(0).Rows(Rstat).Item("regionID"))

                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("regionName")) = False Then
                            cRegion.regionName = RegionData.Tables(0).Rows(Rstat).Item("regionName")
                        Else
                            cRegion.regionName = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("x")) = False Then
                            cRegion.x = RegionData.Tables(0).Rows(Rstat).Item("x")
                        Else
                            cRegion.x = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("y")) = False Then
                            cRegion.y = RegionData.Tables(0).Rows(Rstat).Item("y")
                        Else
                            cRegion.y = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("z")) = False Then
                            cRegion.z = RegionData.Tables(0).Rows(Rstat).Item("z")
                        Else
                            cRegion.z = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("xmin")) = False Then
                            cRegion.xmin = RegionData.Tables(0).Rows(Rstat).Item("xmin")
                        Else
                            cRegion.xmin = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("ymin")) = False Then
                            cRegion.ymin = RegionData.Tables(0).Rows(Rstat).Item("ymin")
                        Else
                            cRegion.ymin = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("zmin")) = False Then
                            cRegion.zmin = RegionData.Tables(0).Rows(Rstat).Item("zmin")
                        Else
                            cRegion.zmin = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("xmax")) = False Then
                            cRegion.xmax = RegionData.Tables(0).Rows(Rstat).Item("xmax")
                        Else
                            cRegion.xmax = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("ymax")) = False Then
                            cRegion.ymax = RegionData.Tables(0).Rows(Rstat).Item("ymax")
                        Else
                            cRegion.ymax = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("zmax")) = False Then
                            cRegion.zmax = RegionData.Tables(0).Rows(Rstat).Item("zmax")
                        Else
                            cRegion.zmax = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("factionID")) = False Then
                            cRegion.factionID = RegionData.Tables(0).Rows(Rstat).Item("factionID")
                        Else
                            cRegion.factionID = ""
                        End If
                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("radius")) = False Then
                            cRegion.radius = RegionData.Tables(0).Rows(Rstat).Item("radius")
                        Else
                            cRegion.radius = ""
                        End If
                        cRegion.Flag = False
                        RegionName.Add(cRegion.regionName, cRegion)
                        RegionID.Add(cRegion.RegionID, cRegion)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the REgion data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadConstellations() As Boolean

        Dim strSQL As String = "SELECT * FROM mapConstellations ORDER BY constellationID;"
        Dim ConstData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If ConstData IsNot Nothing Then
                If ConstData.Tables(0).Rows.Count > 0 Then
                    ConstellationID.Clear()
                    ConstellationName.Clear()

                    For Rstat As Integer = 0 To ConstData.Tables(0).Rows.Count - 1
                        Dim cConst As Constellation = New Constellation
                        cConst.regionID = CInt(ConstData.Tables(0).Rows(Rstat).Item("regionID"))

                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("constellationID")) = False Then
                            cConst.constellationID = ConstData.Tables(0).Rows(Rstat).Item("constellationID")
                        Else
                            cConst.constellationID = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("constellationName")) = False Then
                            cConst.constellationName = ConstData.Tables(0).Rows(Rstat).Item("constellationName")
                        Else
                            cConst.constellationName = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("x")) = False Then
                            cConst.x = ConstData.Tables(0).Rows(Rstat).Item("x")
                        Else
                            cConst.x = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("y")) = False Then
                            cConst.y = ConstData.Tables(0).Rows(Rstat).Item("y")
                        Else
                            cConst.y = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("z")) = False Then
                            cConst.z = ConstData.Tables(0).Rows(Rstat).Item("z")
                        Else
                            cConst.z = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("xmin")) = False Then
                            cConst.xmin = ConstData.Tables(0).Rows(Rstat).Item("xmin")
                        Else
                            cConst.xmin = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("ymin")) = False Then
                            cConst.ymin = ConstData.Tables(0).Rows(Rstat).Item("ymin")
                        Else
                            cConst.ymin = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("zmin")) = False Then
                            cConst.zmin = ConstData.Tables(0).Rows(Rstat).Item("zmin")
                        Else
                            cConst.zmin = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("xmax")) = False Then
                            cConst.xmax = ConstData.Tables(0).Rows(Rstat).Item("xmax")
                        Else
                            cConst.xmax = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("ymax")) = False Then
                            cConst.ymax = ConstData.Tables(0).Rows(Rstat).Item("ymax")
                        Else
                            cConst.ymax = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("zmax")) = False Then
                            cConst.zmax = ConstData.Tables(0).Rows(Rstat).Item("zmax")
                        Else
                            cConst.zmax = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("factionID")) = False Then
                            cConst.factionID = ConstData.Tables(0).Rows(Rstat).Item("factionID")
                        Else
                            cConst.factionID = " "
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("radius")) = False Then
                            cConst.radius = ConstData.Tables(0).Rows(Rstat).Item("radius")
                        Else
                            cConst.radius = " "
                        End If
                        cConst.Flag = False
                        ConstellationName.Add(cConst.constellationName, cConst)
                        ConstellationID.Add(cConst.constellationID, cConst)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the Constellation data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadOperationData() As Boolean
        Dim strSQL As String = "SELECT * FROM staOperationServices ORDER BY operationID;"
        Dim OpData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        Try
            If OpData IsNot Nothing Then
                If OpData.Tables(0).Rows.Count > 0 Then
                    OperationList.Clear()
                    For a As Integer = 0 To OpData.Tables(0).Rows.Count - 1
                        Dim cOp As Operation = New Operation
                        cOp.operationID = OpData.Tables(0).Rows(a).Item("operationID")
                        cOp.serviceID = OpData.Tables(0).Rows(a).Item("serviceID")
                        cOp.Flag = False
                        OperationList.Add(a, cOp)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the operation data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        Return True
    End Function
    Public Function LoadServiceData() As Boolean
        Dim strSQL As String = "SELECT * FROM staServices ORDER BY serviceID;"
        Dim ServData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        Try
            If ServData IsNot Nothing Then
                If ServData.Tables(0).Rows.Count > 0 Then
                    ServiceList.Clear()
                    For a As Integer = 0 To ServData.Tables(0).Rows.Count - 1
                        Dim CServ As Service = New Service
                        CServ.serviceID = ServData.Tables(0).Rows(a).Item("serviceID")
                        CServ.serviceName = ServData.Tables(0).Rows(a).Item("serviceName")
                        CServ.Flag = False
                        ServiceList.Add(a, CServ)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the service data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        Return True
    End Function
    Public Function LoadAgents() As Boolean
        Dim strSQL As String = "SELECT * FROM agtAgents ORDER BY agentID;"
        Dim AgentData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If AgentData IsNot Nothing Then
                If AgentData.Tables(0).Rows.Count > 0 Then
                    AgentID.Clear()
                    AgentName.Clear()
                    For a As Integer = 0 To AgentData.Tables(0).Rows.Count - 1
                        Dim cAgent As Agent = New Agent
                        Dim skip As Boolean = False
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("agentID")) = False Then
                            cAgent.agentID = AgentData.Tables(0).Rows(a).Item("agentID")
                            Dim x As Integer = AgentData.Tables(0).Rows(a).Item("agentID")
                            cAgent.agentName = eveNames(x).itemname
                        Else
                            cAgent.agentID = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("divisionID")) = False Then
                            cAgent.divisionID = AgentData.Tables(0).Rows(a).Item("divisionID")
                        Else
                            cAgent.divisionID = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("corporationID")) = False Then
                            cAgent.corporationID = AgentData.Tables(0).Rows(a).Item("corporationID")
                        Else
                            cAgent.corporationID = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("stationID")) = False Then
                            cAgent.stationId = AgentData.Tables(0).Rows(a).Item("stationID")
                        Else
                            skip = True
                            cAgent.stationId = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("level")) = False Then
                            cAgent.Level = AgentData.Tables(0).Rows(a).Item("level")
                        Else
                            cAgent.Level = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("quality")) = False Then
                            cAgent.Quality = AgentData.Tables(0).Rows(a).Item("quality")
                        Else
                            cAgent.Quality = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("agentTypeID")) = False Then
                            cAgent.Type = AgentData.Tables(0).Rows(a).Item("agentTypeID")
                        Else
                            cAgent.Type = " "
                        End If
                        cAgent.Flag = False
                        If skip = False Then
                            AgentID.Add(cAgent.agentID, cAgent)
                            AgentName.Add(cAgent.agentName, cAgent)
                        End If
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the agent data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        Return True
    End Function
    Public Function LoadConq() As Boolean
        Try
            ' Dimension variables
            Dim CSDetails As XmlNodeList
            Dim CSNode As XmlNode
            ' Get the Sovereignty data
            Dim XMLDoc As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Conquerables)
            CSDetails = XMLDoc.SelectNodes("/eveapi/result/rowset/row")
            For Each CSNode In CSDetails
                Dim CS As New ConqStat
                CS.stationID = CSNode.Attributes.GetNamedItem("stationID").Value
                CS.stationName = CSNode.Attributes.GetNamedItem("stationName").Value
                CS.stationTypeID = CSNode.Attributes.GetNamedItem("stationTypeID").Value
                CS.solarSystemID = CSNode.Attributes.GetNamedItem("solarSystemID").Value - 30000000
                CS.corporationID = CSNode.Attributes.GetNamedItem("corporationID").Value
                CS.corporationName = CSNode.Attributes.GetNamedItem("corporationName").Value
                CS.Flag = False
                CSStationName.Add(CS.stationName, CS)
                CSStationList.Add(CS.stationID, CS)
            Next
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    Public Function LoadStationTypes() As Boolean
        Dim strSQL As String = "SELECT * FROM staStationTypes ORDER BY stationTypeID;"
        Dim sttData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If sttData IsNot Nothing Then
                If sttData.Tables(0).Rows.Count > 0 Then
                    StationTypes.Clear()
                    For a As Integer = 0 To sttData.Tables(0).Rows.Count - 1
                        Dim cCSS As StType = New StType
                        If IsDBNull(sttData.Tables(0).Rows(a).Item("stationTypeID")) = False Then
                            cCSS.stationTypeID = sttData.Tables(0).Rows(a).Item("stationTypeID")
                        Else
                            cCSS.stationTypeID = 0
                        End If
                        If IsDBNull(sttData.Tables(0).Rows(a).Item("operationID")) = False Then
                            cCSS.OperationID = sttData.Tables(0).Rows(a).Item("operationID")
                        Else
                            cCSS.OperationID = 0
                        End If
                        If IsDBNull(sttData.Tables(0).Rows(a).Item("officeSlots")) = False Then
                            cCSS.OfficeSlots = sttData.Tables(0).Rows(a).Item("officeSlots")
                        Else
                            cCSS.OfficeSlots = 0
                        End If
                        cCSS.Flag = False
                        StationTypes.Add(cCSS.stationTypeID, cCSS)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading StationTypes. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        Return True
    End Function
    Public Function LoadNPCDiv() As Boolean
        Dim strSQL As String = "SELECT * FROM crpNPCDivisions ORDER BY divisionID;"
        Dim DivData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If DivData IsNot Nothing Then
                If DivData.Tables(0).Rows.Count > 0 Then
                    NPCDivID.Clear()
                    For a As Integer = 0 To DivData.Tables(0).Rows.Count - 1
                        Dim cDiv As NPCDiv = New NPCDiv
                        cDiv.divisionID = DivData.Tables(0).Rows(a).Item("divisionID")
                        cDiv.divisionName = DivData.Tables(0).Rows(a).Item("divisionName")
                        cDiv.Flag = False
                        NPCDivID.Add(cDiv.divisionID, cDiv)
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
        Catch e As Exception
            MessageBox.Show("There was an error loading the data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
        Return True
    End Function
    Public Function LoadCB() As Boolean

        Dim strSQL As String = "SELECT * FROM mapDenormalize ORDER BY solarSystemID;"
        Dim cbData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If cbData IsNot Nothing Then
                If cbData.Tables(0).Rows.Count > 0 Then
                    Dim lastSystemNo As Integer = 0
                    Dim lastSystem As New SolarSystem

                    For Each mapRow As DataRow In cbData.Tables(0).Rows
                        'For stat As Integer = 0 To cbData.Tables(0).Rows.Count - 1
                        If IsDBNull(mapRow.Item("solarSystemID")) = False Then
                            If lastSystemNo <> CInt(mapRow.Item("solarSystemID") - 30000000) Then
                                lastSystemNo = CInt(mapRow.Item("solarSystemID") - 30000000)
                                lastSystem = EveHQ.Core.HQ.SystemsID(lastSystemNo)
                            End If
                            Select Case CInt(mapRow.Item("groupID"))
                                Case 6
                                    ' Star - do nothing
                                Case 7
                                    ' Planet
                                    lastSystem.Planets.Add(CStr(mapRow.Item("itemID")), CStr(mapRow.Item("itemName")))
                                Case 8
                                    ' Moon
                                    lastSystem.Moons.Add(CStr(mapRow.Item("itemID")), CStr(mapRow.Item("itemName")))
                                Case 9
                                    ' Belt
                                    If CStr(mapRow.Item("itemName")).Contains("Ice Field") = True Then
                                        lastSystem.IBelts.Add(CStr(mapRow.Item("itemID")), CStr(mapRow.Item("itemName")))
                                    Else
                                        lastSystem.ABelts.Add(CStr(mapRow.Item("itemID")), CStr(mapRow.Item("itemName")))
                                    End If
                                Case 10
                                    ' Stargate - Do nothing
                                Case 15
                                    ' Station
                                    lastSystem.Stations.Add(CStr(mapRow.Item("itemID")), CStr(mapRow.Item("itemName")))
                            End Select
                        End If
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the Celestial data. The error was: " & ControlChars.CrLf & e.Message, "Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
            cbData.Clear()
        End Try
    End Function
#End Region

#Region "Form Loading Routines"
    Private Sub frmMap_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Temp remove the waypoints tab
        Me.tabWaypointExclusions.TabPages.Remove(Me.tabWaypoints)
        Me.Visible = True
        Me.Refresh()

        Me.cboRegion.Items.Clear()
        cboRegion.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboRegion.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboRegion.BeginUpdate()
        cboRegion.AutoCompleteCustomSource.Add("All")
        Me.cboRegion.Items.Add("All")
        For Each xRegion As Region In RegionName.Values
            cboRegion.AutoCompleteCustomSource.Add(xRegion.regionName)
            Me.cboRegion.Items.Add(xRegion.regionName)
        Next
        Me.cboRegion.EndUpdate()
        Me.cboRegion.Text = Me.cboRegion.Items.Item(0).ToString

        Me.cboConst.Items.Clear()
        cboConst.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboConst.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboConst.BeginUpdate()
        cboConst.AutoCompleteCustomSource.Add("All")
        Me.cboConst.Items.Add("All")
        For Each xConst As Constellation In ConstellationName.Values
            cboConst.AutoCompleteCustomSource.Add(xConst.constellationName)
            Me.cboConst.Items.Add(xConst.constellationName)
        Next
        Me.cboConst.EndUpdate()
        Me.cboConst.Text = Me.cboConst.Items.Item(0).ToString

        Me.cboSystem.Items.Clear()
        cboSystem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboSystem.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboSystem.BeginUpdate()
        For Each cSystem As SolarSystem In EveHQ.Core.HQ.SystemsID.Values
            cboSystem.AutoCompleteCustomSource.Add(cSystem.Name)
            Me.cboSystem.Items.Add(cSystem.Name)
        Next
        Me.cboSystem.EndUpdate()

        Call Me.ClearSystemInfo()
        Call Me.CreateEveMap(True)
        Me.ships = New String() {"Titan", "Mothership", "Capital Industrial", "Dreadnought", "Carrier", "Anshar", "Ark", "Nomad", "Rhea"}
        Me.ranges = New Double() {3.5, 4, 5, 5, 6.5, 5, 5, 5, 5}
        Me.fuel = New Double() {1000, 1000, 1000, 1000, 1000, 3100, 2900, 2700, 3300}
        Call Me.LoadPilots()
        Call Me.LoadShips()
        Call Me.LoadJFC()
        Call Me.LoadJDC()
        Call Me.LoadJF()
        Me.cboRouteMode.Text = Me.cboRouteMode.Items.Item(0).ToString
        Me.cboSystem.Text = Me.cboSystem.Items.Item(0).ToString
        Me.cboShips.SelectedIndex = 0
        cboJDC.SelectedIndex = EveHQ.Core.HQ.myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration)
        cboJFC.SelectedIndex = EveHQ.Core.HQ.myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFuelConservation)
        cboJF.SelectedIndex = EveHQ.Core.HQ.myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFreighters)
        Me.BringToFront()
        bHaveMouse = False
        bHaveScan = False

        ' Load the agent stuff
        Me.cboAgentFaction.Items.Clear()
        cboAgentFaction.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboAgentFaction.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboAgentFaction.BeginUpdate()
        cboAgentFaction.AutoCompleteCustomSource.Add("All")
        Me.cboAgentFaction.Items.Add("All")
        For Each cFaction As Faction In FactionName.Values
            cboAgentFaction.AutoCompleteCustomSource.Add(cFaction.factionName)
            Me.cboAgentFaction.Items.Add(cFaction.factionName)
        Next
        Me.cboAgentFaction.EndUpdate()
        Me.cboAgentFaction.Text = Me.cboAgentFaction.Items.Item(0).ToString

    End Sub

    Private Sub ClearSystemInfo()
        lblID.Text = ""
        lblName.Text = ""
        lblRegion.Text = ""
        lblConst.Text = ""
        lblSecurity.Text = ""
        lblEveSec.Text = ""
        lblNoGates.Text = ""
        lblGates.Text = ""
    End Sub

    Private Sub LoadPilots()
        Me.cboPilot.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
            If cPilot.Active = True Then
                Me.cboPilot.Items.Add(cPilot.Name)
            End If
        Next
        If EveHQ.Core.HQ.myPilot IsNot Nothing Then
            Me.cboPilot.SelectedItem = EveHQ.Core.HQ.myPilot.Name
        End If
    End Sub

    Private Sub LoadShips()
        Me.lblShips.Text = "Ship Type:"
        Me.cboShips.Items.Clear()
        Me.cboShips.Items.AddRange(Me.ships)
        Me.cboShips.SelectedIndex = 0
    End Sub

    Private Sub LoadJFC()
        Dim CBF As Double = Me.CurrentBaseFuel
        Me.lblJFC.Text = "Jump Fuel Conservation:"
        Dim JFC As Integer = Me.cboJFC.SelectedIndex
        Me.cboJFC.Items.Clear()
        For level As Integer = 0 To 5
            Me.cboJFC.Items.Add(String.Concat(New Object() {level, " (", (CBF * (1 - (level * 0.1))), "/ly)"}))
        Next
        If csPilot IsNot Nothing Then
            If chkOverrideJFC.Checked = True Then
                Me.cboJFC.SelectedIndex = 0
                Me.cboJFC.Enabled = True
            Else
                Me.cboJFC.SelectedIndex = csPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFuelConservation)
                Me.cboJFC.Enabled = False
            End If
        Else
            Me.chkOverrideJFC.Checked = True
            Me.chkOverrideJFC.Enabled = False
            Me.cboJFC.SelectedIndex = JFC
            Me.cboJFC.Enabled = True
        End If
    End Sub

    Private Sub LoadJDC()
        Dim CBR As Double = Me.CurrentBaseRange
        Me.lblJDC.Text = "Jump Drive Calibration:"
        Dim JDC As Integer = Me.cboJDC.SelectedIndex
        Me.cboJDC.Items.Clear()
        For level As Integer = 0 To 5
            Me.cboJDC.Items.Add(String.Concat(New Object() {level, " (", (CBR * (1 + (level * 0.25))), "ly)"}))
        Next
        If csPilot IsNot Nothing Then
            If chkOverrideJDC.Checked = True Then
                Me.cboJDC.SelectedIndex = JDC
                Me.cboJDC.Enabled = True
            Else
                Me.cboJDC.SelectedIndex = csPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration)
                Me.cboJDC.Enabled = False
            End If
        Else
            Me.chkOverrideJDC.Checked = True
            Me.chkOverrideJDC.Enabled = False
            Me.cboJDC.SelectedIndex = JDC
            Me.cboJDC.Enabled = True
        End If
    End Sub

    Private Sub LoadJF()
        Dim CBF As Double = Me.CurrentBaseFuel
        Me.lblJF.Text = "Jump Frieghters:"
        Dim JF As Integer = Me.cboJF.SelectedIndex
        Dim JFC As Integer = Me.cboJFC.SelectedIndex
        Me.cboJF.Items.Clear()
        CBF = CBF * (1 - (JFC * 0.1))
        Select Case cboShips.SelectedItem
            Case "Titan", "Mothership", "Capital Industrial", "Dreadnought", "Carrier"
                For level As Integer = 0 To 5
                    Me.cboJF.Items.Add(String.Concat(New Object() {level, " (", CBF, "/ly)"}))
                Next
            Case Else
                For level As Integer = 0 To 5
                    Me.cboJF.Items.Add(String.Concat(New Object() {level, " (", (CBF * (1 - (level * 0.05))), "/ly)"}))
                Next
        End Select
        If csPilot IsNot Nothing Then
            If chkOverrideJF.Checked = True Then
                Me.cboJF.SelectedIndex = JF
                Me.cboJF.Enabled = True
            Else
                Me.cboJF.SelectedIndex = csPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFreighters)
                Me.cboJF.Enabled = False
            End If
        Else
            Me.chkOverrideJF.Checked = True
            Me.chkOverrideJF.Enabled = False
            Me.cboJF.SelectedIndex = JF
            Me.cboJF.Enabled = True
        End If
    End Sub

#End Region

#Region "About Form Routines"
    Private Sub pbInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbInfo.Click
        Dim aboutForm As New frmAbout
        aboutForm.ShowDialog()
        aboutForm = Nothing
    End Sub
#End Region

#Region "Main Region, Const & System Selection Routines"

    Private Sub cboRegion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRegion.SelectedIndexChanged

    End Sub

    Private Sub cboConst_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConst.SelectedIndexChanged

    End Sub

    Private Sub cboSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSystem.SelectedIndexChanged
        Call PinpointLocation(cboSystem.SelectedItem)
    End Sub

#End Region

#Region "Old Routing Functions"
    Private Sub CalculateGateRoute(ByVal startSystem As String, ByVal endSystem As String)
        Dim startTime, endTime As DateTime
        Dim totalTime As TimeSpan

        startTime = Now
        Dim s, t As SolarSystem
        s = EveHQ.Core.HQ.SystemsName(startSystem)
        t = EveHQ.Core.HQ.SystemsName(endSystem)
        Dim minSec As Double = nudMinSec.Value
        Dim maxSec As Double = nudMaxSec.Value
        Dim myRoute As Dijkstra = New Dijkstra
        Dim route As ArrayList = myRoute.GetPath(s, t, True, minSec, maxSec)
        If route.Count = 1 Then
            lvwRoute.Items.Add("No available route")
        Else
            route.Reverse()
            WaypointSet = myRoute.GetRoute
        End If
        endTime = Now
        totalTime = endTime - startTime
        lblTimeTaken.Text = "Time Taken: " & totalTime.TotalSeconds & "s"
    End Sub
    Private Sub CalculateJumpRoute(ByVal startSystem As String, ByVal endSystem As String)
        Dim startTime, endTime As DateTime
        Dim totalTime As TimeSpan

        startTime = Now
        Dim s, t As SolarSystem
        s = EveHQ.Core.HQ.SystemsName(startSystem)
        t = EveHQ.Core.HQ.SystemsName(endSystem)
        Dim minSec As Double = nudMinSec.Value
        Dim maxSec As Double = nudMaxSec.Value
        Dim myRoute As Dijkstra = New Dijkstra
        Dim route As ArrayList = myRoute.GetJumpPath(s, t, True)
        If route.Count = 1 Then
            lvwRoute.Items.Add("No available route")
        Else
            route.Reverse()

            WaypointSet = myRoute.GetRoute
        End If
        endTime = Now
        totalTime = endTime - startTime
        lblTimeTaken.Text = "Time Taken: " & totalTime.TotalSeconds & "s"
    End Sub
    Private Sub GenerateRoute()
        FullRoute.Clear()

        If WaypointSets.Count > 0 Then
            If lstWaypoints.Items.Count = 0 Then
                ' Generate the start to end route only
                Dim fromSys As SolarSystem = EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag)
                Dim toSys As SolarSystem = EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)
                FullRoute.Add(Me.GetRoute(fromSys, toSys))
            Else
                ' Generate for all the waypoints
                Dim fromSys, toSys As SolarSystem
                For sys As Integer = 0 To lstWaypoints.Items.Count - 1
                    Dim sysname As String = lstWaypoints.Items(sys)
                    toSys = EveHQ.Core.HQ.SystemsName(sysname)
                    If sys = 0 Then
                        fromSys = EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag)
                    Else
                        fromSys = EveHQ.Core.HQ.SystemsName(lstWaypoints.Items(sys - 1))
                    End If
                    FullRoute.Add(Me.GetRoute(fromSys, toSys))
                Next
                fromSys = EveHQ.Core.HQ.SystemsName(lstWaypoints.Items(lstWaypoints.Items.Count - 1))
                toSys = EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)
                FullRoute.Add(Me.GetRoute(fromSys, toSys))
            End If
        End If

        ' Display the route

        Dim jumps As Integer = 1
        lvwRoute.Items.Clear()
        For Each route As ArrayList In FullRoute
            For Each sys As Integer In route
                Dim cSystem As SolarSystem = EveHQ.Core.HQ.SystemsID(sys)
                Dim newSystem As New ListViewItem
                newSystem.Text = FormatNumber(jumps, 0, TriState.True)
                newSystem.Name = cSystem.Name
                newSystem.SubItems.Add(cSystem.Name)
                newSystem.SubItems.Add(FormatNumber(cSystem.EveSec, 1, TriState.True))
                lvwRoute.Items.Add(newSystem)
                jumps += 1
            Next
        Next
        Call Me.ShowRoute()
    End Sub
    Private Function GetRoute(ByVal fromSys As SolarSystem, ByVal toSys As SolarSystem) As ArrayList
        Dim prev As SortedList = WaypointSet
        Dim tracer As Integer = toSys.ID
        Dim route As New ArrayList
        Do While tracer <> fromSys.ID And tracer <> 0
            route.Add(tracer)
            tracer = prev.Item(tracer)
        Loop
        route.Reverse()
        Return route
    End Function
    Private Sub btnTestRoute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If lblStartSystem.Tag Is Nothing Or lblEndSystem.Tag Is Nothing Then
            MessageBox.Show("Source System and Destination System must be selected", "No System Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
    End Sub
    Private Sub lvwRoute_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwRoute.ColumnClick
        If lvwRoute.Tag = e.Column Then
            Me.lvwRoute.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwRoute.Tag = -1
        Else
            Me.lvwRoute.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwRoute.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwRoute.Sort()
    End Sub
    Private Sub lvwRoute_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRoute.DoubleClick
        Dim sysName As String = lvwRoute.SelectedItems(0).Name
        cboSystem.SelectedItem = sysName
    End Sub
    Private Sub btnTestRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If lblStartSystem.Tag Is Nothing Then
            MessageBox.Show("Source System  must be selected", "No System Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim s, t As SolarSystem
        s = EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag)
        t = s
        Dim myRoute As Dijkstra = New Dijkstra
        Dim gates As ArrayList = myRoute.SystemsInGateRange(s, t, True, 2)
        lvwRoute.Items.Clear()
        Dim system(0, 1) As Integer
        For Each system In gates
            Dim cSystem As SolarSystem = EveHQ.Core.HQ.SystemsID(system(0, 0))
            Dim sysJump As Integer = system(0, 1)
            Dim newSystem As New ListViewItem
            newSystem.Text = cSystem.Name
            newSystem.SubItems.Add(FormatNumber(cSystem.EveSec, 1, TriState.True))
            newSystem.SubItems.Add(FormatNumber(sysJump, 0, TriState.True))
            lvwRoute.Items.Add(newSystem)
        Next
    End Sub
    Private Function CalcDistance(ByVal sSys As SolarSystem, ByVal tSys As SolarSystem) As Double
        Dim dist As Double = 0
        Dim x1, x2, y1, y2, z1, z2 As Double

        x1 = sSys.x
        y1 = sSys.y
        z1 = sSys.z
        x2 = tSys.x
        y2 = tSys.y
        z2 = tSys.z
        dist = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2)) / ly

        Return dist

    End Function

#End Region

#Region "Permutation Functions"
    Private Function StringPermutations(ByVal data As String) As String(,)
        Dim i As Int32
        Dim y As Int32
        Dim x As Int32
        Dim tempChar As String
        Dim newString As String
        Dim strings(,) As String
        Dim rowCount As Long

        If data.Length < 2 Then
            Return Nothing
            Exit Function
        End If

        'use the factorial function to determine the number of rows needed
        'because redim preserve is slow
        ReDim strings(data.Length - 1, Factorial(data.Length - 1) - 1)
        strings(0, 0) = data

        'swap each character(I) from the second postion to the second to last position
        For i = 1 To (data.Length - 2)
            'for each of the already created numbers
            For y = 0 To rowCount
                'do swaps for the character(I) with each of the characters to the right
                For x = data.Length To i + 2 Step -1
                    tempChar = strings(0, y).Substring(i, 1)
                    newString = strings(0, y)
                    Mid(newString, i + 1, 1) = newString.Substring(x - 1, 1)
                    Mid(newString, x, 1) = tempChar
                    rowCount = rowCount + 1
                    strings(0, rowCount) = newString
                Next
            Next
        Next

        'Shift Characters
        'for each empty column
        For i = 1 To data.Length - 1
            'move the shift character over one
            For x = 0 To strings.GetUpperBound(1)
                strings(i, x) = strings(i - 1, x)
                Mid(strings(i, x), i, 1) = strings(i - 1, x).Substring(i, 1)
                Mid(strings(i, x), i + 1, 1) = strings(i - 1, x).Substring(i - 1, 1)
            Next
        Next

        Return strings

    End Function
    Private Function Factorial(ByVal Number As Integer) As String
        Try
            If Number = 0 Then
                Return 1
            Else
                Return Number * Factorial(Number - 1)
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
#End Region

#Region "New Routing Functions"
    ' Interface Routines
    Private Sub btnAddStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddStart.Click
        If EveHQ.Core.HQ.SystemsName.Contains(cboSystem.Text) = False Then
            MessageBox.Show("System Name is not a valid system", "System Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            Dim startSystem As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.Text)
            lblStartSystem.Text = "Start System: " & startSystem.Name
            lblStartSystem.Tag = startSystem.Name
        End If
    End Sub
    Private Sub btnAddEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddEnd.Click
        If EveHQ.Core.HQ.SystemsName.Contains(cboSystem.Text) = False Then
            MessageBox.Show("System Name is not a valid system", "System Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            If lblStartSystem.Tag = "" Then
                MessageBox.Show("Please enter a Start System before entering a Destination.", "Start System Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim endSystem As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.Text)
                ' See if we are changing the end point
                If lblEndSystem.Tag IsNot Nothing Then
                    ' Remove the old endpoint from the waypoints
                    WaypointRoutes.Remove(lblEndSystem.Tag)
                    If Waypoints.Count > 0 Then
                        Dim startSystem As SolarSystem = Waypoints(Waypoints.Count - 1)
                        If cboRouteMode.SelectedItem = "Gate Route" Then
                            WaypointRoutes(endSystem.Name) = Me.GateRoute(startSystem, endSystem)
                        Else
                            WaypointRoutes(endSystem.Name) = Me.JumpRoute(startSystem, endSystem)
                        End If
                    Else
                        If cboRouteMode.SelectedItem = "Gate Route" Then
                            WaypointRoutes(endSystem.Name) = Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), endSystem)
                        Else
                            WaypointRoutes(endSystem.Name) = Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), endSystem)
                        End If
                    End If
                Else
                    If cboRouteMode.SelectedItem = "Gate Route" Then
                        WaypointRoutes(endSystem.Name) = Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), endSystem)
                    Else
                        WaypointRoutes(endSystem.Name) = Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), endSystem)
                    End If
                End If

                lblEndSystem.Text = "End System: " & endSystem.Name
                lblEndSystem.Tag = endSystem.Name

                lastAlgo = Me.cboRouteMode.SelectedIndex
                'Call GenerateWaypointRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
            End If
        End If
    End Sub
    Private Sub cboRouteMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRouteMode.SelectedIndexChanged
        Select Case Me.cboRouteMode.SelectedIndex
            Case 0
                gbJumpDrive.Visible = False
                nudJumps.Enabled = False
                lblEndSystem.Visible = True
                btnAddEnd.Enabled = True
            Case 1
                gbJumpDrive.Visible = True
                nudJumps.Enabled = False
                lblEndSystem.Visible = True
                btnAddEnd.Enabled = True
            Case 2
                gbJumpDrive.Visible = False
                nudJumps.Enabled = True
                lblEndSystem.Visible = False
                btnAddEnd.Enabled = False
            Case 3
                gbJumpDrive.Visible = True
                nudJumps.Enabled = True
                lblEndSystem.Visible = False
                btnAddEnd.Enabled = False
        End Select
    End Sub
    Private Sub goClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculate.Click
        Select Case Me.cboRouteMode.SelectedIndex
            Case 0, 1
                If lblStartSystem.Tag Is Nothing Or lblEndSystem.Tag Is Nothing Then
                    MessageBox.Show("Source and Destination Systems must be selected", "No System Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            Case 2, 3
                If lblStartSystem.Tag Is Nothing Then
                    MessageBox.Show("Source System must be selected", "No System Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
        End Select
        Me.btnCalculate.Enabled = False
        Me.DoRoute()
        lastAlgo = Me.cboRouteMode.SelectedIndex
        If Me.chkRoute.Checked = True Then
            Call Me.ShowRoute()
        End If
    End Sub
    Private Sub cboPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilot.SelectedIndexChanged
        csPilot = EveHQ.Core.HQ.Pilots.Item(cboPilot.SelectedItem)
        Call Me.LoadJDC()
        Call Me.LoadJFC()
    End Sub
    Private Sub cboShips_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboShips.SelectedIndexChanged
        Call LoadJDC()
        Call LoadJFC()
        Call LoadJF()
    End Sub
    Private Sub chkOverrideJDC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideJDC.CheckedChanged
        If chkOverrideJDC.Checked = True Then
            cboJDC.Enabled = True
        Else
            If EveHQ.Core.HQ.myPilot IsNot Nothing Then
                cboJDC.SelectedIndex = EveHQ.Core.HQ.myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration)
                cboJDC.Enabled = False
            End If
        End If
    End Sub
    Private Sub chkOverrideJFC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideJFC.CheckedChanged
        If chkOverrideJFC.Checked = True Then
            cboJFC.Enabled = True
        Else
            If EveHQ.Core.HQ.myPilot IsNot Nothing Then
                cboJFC.SelectedIndex = EveHQ.Core.HQ.myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFuelConservation)
                cboJFC.Enabled = False
            End If
        End If
    End Sub
    Private Sub chkOverrideJF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideJF.CheckedChanged
        If chkOverrideJF.Checked = True Then
            cboJF.Enabled = True
        Else
            If EveHQ.Core.HQ.myPilot IsNot Nothing Then
                cboJF.SelectedIndex = EveHQ.Core.HQ.myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.JumpFreighters)
                cboJF.Enabled = False
            End If
        End If
    End Sub
    Private Sub cboJFC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboJFC.SelectedIndexChanged
        Call Me.LoadJF()
    End Sub
    Private Sub mnuCopyToClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyToClipboard.Click
        Dim buffer As New System.Text.StringBuilder
        For i As Integer = 0 To lvwRoute.Columns.Count - 1
            buffer.Append(lvwRoute.Columns(i).Text)
            buffer.Append(ControlChars.Tab)
        Next
        buffer.Append(ControlChars.CrLf)
        For i As Integer = 0 To lvwRoute.Items.Count - 1
            For j As Integer = 0 To lvwRoute.Items(0).SubItems.Count - 1
                If lvwRoute.Items(i).SubItems(j) IsNot Nothing Then
                    buffer.Append(lvwRoute.Items(i).SubItems(j).Text)
                    buffer.Append(ControlChars.Tab)
                End If
            Next
            buffer.Append(ControlChars.CrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    ' Routing Functions
    Private Enum RouteType
        GateRadius = 2
        Gates = 0
        JumpRadius = 3
        Jumps = 1
    End Enum
    Public Function AddGates(ByVal c As Route, ByVal [to] As SolarSystem) As Route
        If (c Is Nothing) Then
            Return New Route([to], Nothing)
        End If
        Dim route1 As Route = Me.AddGates(c.Next, c.Sys)
        If (route1 Is Nothing) Then
            Return Nothing
        End If
        Dim route2 As Route = Me.GateRoute([to], route1.Sys)
        If (route2 Is Nothing) Then
            Return Nothing
        End If
        c = route2.Concat(route1.Next)
        c.Jump = True
        route2.Jump = True
        Return route2
    End Function
    Public Function CurrentBaseRange() As Double
        If ((Me.cboShips.SelectedIndex >= 0) AndAlso (Me.cboShips.SelectedIndex < Me.ranges.Length)) Then
            Return Me.ranges(Me.cboShips.SelectedIndex)
        End If
        Return 0
    End Function
    Public Function CurrentBaseFuel() As Double
        If ((Me.cboShips.SelectedIndex >= 0) AndAlso (Me.cboShips.SelectedIndex < Me.ranges.Length)) Then
            Return Me.fuel(Me.cboShips.SelectedIndex)
        End If
        Return 0
    End Function
    Public Shared Function Distance(ByVal s1 As SolarSystem, ByVal s2 As SolarSystem) As Double
        Dim dx As Double = ((s1.x - s2.x) / ly)
        Dim dy As Double = ((s1.y - s2.y) / ly)
        Dim dz As Double = ((s1.z - s2.z) / ly)
        Dim dd As Double = Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz))
        Return dd
    End Function
    Private Sub DoRoute()

        startTime = Now

        Dim algotype1 As RouteType = DirectCast(Me.cboRouteMode.SelectedIndex, RouteType)
        Dim driveRange As Double
        Dim fuelMultiplier As Double
        Dim nJumps As Double
        Dim startSys As SolarSystem = EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag)
        Dim endSys As New SolarSystem
        Select Case algotype1
            Case RouteType.Gates, RouteType.Jumps
                endSys = EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)
            Case RouteType.GateRadius, RouteType.JumpRadius
                endSys = Nothing
        End Select
        Dim text2 As String
        Dim maxJumps As Integer = 0
        Dim route1 As Route = Nothing
        Dim text1 As String = Nothing

        frmMap.minjump = CDbl(Me.nudMinSec.Value)
        frmMap.maxjump = CDbl(Me.nudMaxSec.Value)
        frmMap.mingate = CDbl(Me.nudMinSec.Value)
        frmMap.maxgate = CDbl(Me.nudMaxSec.Value)
        Dim baseRange As Double = Me.CurrentBaseRange
        Dim baseFuel As Double = Me.CurrentBaseFuel
        If Me.cboJDC.Visible Then
            driveRange = (baseRange * (1 + (Me.cboJDC.SelectedIndex * 0.25)))
        Else
            driveRange = 0
        End If
        If Me.cboJFC.Visible Then
            fuelMultiplier = baseFuel * (1 - (Me.cboJFC.SelectedIndex * 0.1))
        Else
            fuelMultiplier = baseFuel
        End If
        Select Case cboShips.SelectedItem
            Case "Anshar", "Ark", "Nomad", "Rhea"
                fuelMultiplier = fuelMultiplier * (1 - (Me.cboJF.SelectedIndex * 0.05))
        End Select
        If Me.nudJumps.Visible Then
            Try
                nJumps = Convert.ToDouble(Me.nudJumps.Text)
            Catch obj2 As Exception
                nJumps = 0
                text1 = ("Bad number: " & Me.nudJumps.Text)
            End Try
        Else
            nJumps = 0
        End If
        If (Not text1 Is Nothing) Then
            frmMap.ErrorMsg(text1)
            Me.btnCalculate.Enabled = True
        End If
        Select Case algotype1
            Case RouteType.Gates
                route1 = Me.GateRoute(startSys, endSys)
            Case RouteType.Jumps
                ' See if we can get a better distance by reversing the route (which happens in a significant amount of cases)
                frmMap.maxdist = Math.Min(driveRange, 14.625)
                route1 = Me.JumpRoute(startSys, endSys)
                Dim route2 As Route = Me.JumpRoute(endSys, startSys)
                If route1 IsNot Nothing Then
                    If route2 IsNot Nothing Then
                        If route2.RouteDist < route1.RouteDist Then
                            route2 = route2.Invert(Nothing)
                            route1 = route2
                        End If
                    End If
                Else
                    If route2 IsNot Nothing Then
                        route2 = route2.Invert(Nothing)
                        route1 = route2
                    End If
                End If
            Case RouteType.GateRadius
                maxJumps = Math.Max(CInt(nJumps), 1)
                route1 = Me.GateRadius(startSys, maxJumps)
            Case RouteType.JumpRadius
                frmMap.maxdist = Math.Min(driveRange, 14.625)
                maxJumps = Math.Max(CInt(nJumps), 1)
                route1 = Me.JumpRadius(startSys, maxJumps)
        End Select
        Dim endTime As DateTime = Now
        Dim timeTaken As TimeSpan = endTime - startTime

        ' Write the route to the listview
        lvwRoute.BeginUpdate()
        lvwRoute.Items.Clear()
        If (route1 Is Nothing) Then
            lvwRoute.Items.Add(" - ")
            lvwRoute.Items(0).SubItems.Add("No valid results found.")
            lvwRoute.Items(0).Name = "Invalid"
        Else
            'Dim obj1 As Object
            Select Case algotype1
                Case RouteType.GateRadius, RouteType.JumpRadius
                    maxJumps = 1
                    route1 = route1.Invert(Nothing)
                    Dim count As Integer = 0
                    Do While (route1 IsNot Nothing)
                        If (route1.Sys Is startSys) Then
                            maxJumps += 1
                        Else
                            count += 1
                            Dim newItem As ListViewItem = New ListViewItem
                            newItem.Name = route1.Sys.Name
                            newItem.Text = count
                            newItem.SubItems.Add(route1.Sys.Name)
                            newItem.SubItems.Add(route1.Sys.Constellation)
                            newItem.SubItems.Add(route1.Sys.Region)
                            newItem.BackColor = Me.SystemColour(route1.Sys.EveSec)
                            newItem.SubItems.Add(FormatNumber(route1.Sys.EveSec, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            If (algotype1 = RouteType.JumpRadius) Then
                                Dim dd As Double = Math.Round(frmMap.Distance(startSys, route1.Sys), 8, MidpointRounding.AwayFromZero)
                                newItem.SubItems.Add(FormatNumber(dd, 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ly")
                                Dim fuel As Integer = Int(dd * fuelMultiplier)
                                newItem.SubItems.Add(FormatNumber(fuel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                newItem.SubItems.Add(FormatNumber(fuel * 0.15, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            End If
                            lvwRoute.Items.Add(newItem)
                        End If
                        route1 = route1.Next
                    Loop
                    Me.lblTotalDistance.Text = "Total Available Jumps: " & lvwRoute.Items.Count
                    Me.lblEuclideanDistance.Text = ""
                    Me.lblTotalFuel.Text = ""
                Case Else
                    text2 = String.Concat(New Object() {startSys.Name, " to ", endSys.Name, ": ", (route1.Length - 1), " jumps"})
                    Dim totalDist As Double = route1.RouteDist
                    Dim accDist As Double = 0
                    Dim jumpDist As Double = 0
                    Dim count As Integer = 0
                    Dim fuel, totalFuel As Integer
                    Do While (route1 IsNot Nothing)
                        accDist = totalDist - route1.RouteDist
                        jumpDist = accDist - jumpDist
                        If route1.Sys IsNot startSys Then
                            Dim newItem As ListViewItem = New ListViewItem
                            count += 1
                            newItem.Name = route1.Sys.Name
                            newItem.Text = count
                            newItem.SubItems.Add(route1.Sys.Name)
                            newItem.SubItems.Add(route1.Sys.Constellation)
                            newItem.SubItems.Add(route1.Sys.Region)
                            newItem.BackColor = Me.SystemColour(route1.Sys.EveSec)
                            newItem.SubItems.Add(FormatNumber(route1.Sys.EveSec, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            If (algotype1 = RouteType.Jumps) Then
                                newItem.SubItems.Add(FormatNumber(Math.Round(jumpDist, 8, MidpointRounding.AwayFromZero), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                fuel = Int(jumpDist * fuelMultiplier)
                                totalFuel += fuel
                                newItem.SubItems.Add(FormatNumber(fuel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                                newItem.SubItems.Add(FormatNumber(fuel * 0.15, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                            End If
                            lvwRoute.Items.Add(newItem)
                        End If
                        jumpDist = accDist
                        route1 = route1.Next
                    Loop
                    Select Case algotype1
                        Case RouteType.Jumps
                            Me.lblTotalDistance.Text = "Total Distance: " & FormatNumber(Math.Round(totalDist, 3, MidpointRounding.AwayFromZero), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ly (" & lvwRoute.Items.Count & " jumps)"
                            Me.lblTotalFuel.Text = "Total Fuel: " & FormatNumber(totalFuel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " isotopes (" & FormatNumber(totalFuel * 0.15, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3)"
                        Case RouteType.Gates
                            Me.lblTotalDistance.Text = "Total Distance: " & lvwRoute.Items.Count
                            Me.lblTotalFuel.Text = ""
                    End Select
                    Me.lblEuclideanDistance.Text = "Euclidean Distance: " & FormatNumber(Math.Round(frmMap.Distance(startSys, endSys), 8, MidpointRounding.AwayFromZero), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ly"
            End Select
        End If
        lvwRoute.EndUpdate()
        Me.btnCalculate.Enabled = True
        lblTimeTaken.Text = "Time Taken: " & timeTaken.TotalSeconds
    End Sub
    Private Function SystemColour(ByVal secStatus As Double) As Color
        Select Case secStatus
            Case Is < 0.05
                Return Color.FromArgb(204, 0, 0)
            Case 0.05 To 0.15
                Return Color.FromArgb(229, 26, 0)
            Case 0.15 To 0.25
                Return Color.FromArgb(255, 51, 0)
            Case 0.25 To 0.35
                Return Color.FromArgb(255, 77, 0)
            Case 0.35 To 0.45
                Return Color.FromArgb(229, 102, 0)
            Case 0.45 To 0.55
                Return Color.FromArgb(255, 255, 0)
            Case 0.55 To 0.65
                Return Color.FromArgb(77, 229, 0)
            Case 0.65 To 0.75
                Return Color.FromArgb(77, 229, 26)
            Case 0.75 To 0.85
                Return Color.FromArgb(26, 255, 51)
            Case 0.85 To 0.95
                Return Color.FromArgb(0, 255, 128)
            Case Is > 0.95
                Return Color.FromArgb(51, 255, 255)
        End Select
    End Function
    Public Shared Sub ErrorMsg(ByVal err As String)
        MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
    End Sub
    Public Function GateRadius(ByVal from As SolarSystem, ByVal rad As Integer) As Route
        Dim solar1 As SolarSystem
        For Each solar1 In EveHQ.Core.HQ.SystemsName.Values
            solar1.Flag = False
        Next
        Dim queue1 As New Queue
        queue1.Enqueue(from)
        from.Flag = True
        Dim route1 As Route = Nothing
        Do While (queue1.Count > 0)
            Dim solar2 As SolarSystem = DirectCast(queue1.Dequeue, SolarSystem)
            Dim solar3 As SolarSystem
            For Each solar3 In solar2.Gates
                If solar3.EveSec >= frmMap.mingate AndAlso solar3.EveSec <= frmMap.maxgate AndAlso Not solar3.Flag Then
                    If (Exclusions.Contains(solar3.Name) = False And Exclusions.Contains(solar3.Constellation) = False And Exclusions.Contains(solar3.Region) = False) Then
                        solar3.Flag = True
                        route1 = New Route(solar3, route1)
                        queue1.Enqueue(solar3)
                    End If
                End If
            Next
            If (solar2 Is from) Then
                rad -= 1
                If ((rad = 0) OrElse (queue1.Count = 0)) Then
                    Return route1
                    Exit Function
                End If
                queue1.Enqueue(from)
                route1 = New Route(solar2, route1)
            End If
        Loop
        Return route1

    End Function
    Public Function GateRoute(ByVal from As SolarSystem, ByVal [to] As SolarSystem) As Route
        Dim hashtable1 As New Hashtable
        Dim queue1 As New Queue
        hashtable1.Add(from, from)
        queue1.Enqueue(from)
        Do While (queue1.Count > 0)
            Dim solar1 As SolarSystem = DirectCast(queue1.Dequeue, SolarSystem)
            If (solar1 Is [to]) Then
                Dim solar2 As SolarSystem
                Dim route1 As New Route(solar1, Nothing)
                solar2 = DirectCast(hashtable1.Item(solar1), SolarSystem)
                Do While (solar2 IsNot solar1)
                    route1 = New Route(solar2, route1)
                    solar1 = solar2
                    solar2 = DirectCast(hashtable1.Item(solar1), SolarSystem)
                Loop
                Return route1
            End If
            Dim solar3 As SolarSystem
            For Each solar3 In solar1.Gates
                If (((solar3.EveSec >= frmMap.mingate) AndAlso (solar3.EveSec <= frmMap.maxgate)) AndAlso Not hashtable1.Contains(solar3)) Then
                    If (Exclusions.Contains(solar3.Name) = False And Exclusions.Contains(solar3.Constellation) = False And Exclusions.Contains(solar3.Region) = False) Then
                        hashtable1.Add(solar3, solar1)
                        queue1.Enqueue(solar3)
                    End If
                End If
            Next
        Loop
        Return Nothing
    End Function
    Public Shared Function GetVal(ByVal headings As String(), ByVal values As String(), ByVal index As String) As String
        Dim num1 As Integer = 0
        Do While (num1 < headings.Length)
            If (headings(num1) = index) Then
                Return values(num1)
                Exit Function
            End If
            num1 += 1
        Loop
        Return "***"
    End Function
    Public Shared Function JumpCount(ByVal c As Route) As Integer
        Dim num1 As Integer = 0
        Do While (Not c Is Nothing)
            If c.Jump Then
                num1 += 1
            End If
            c = c.Next
        Loop
        Return num1
    End Function
    Public Shared Function JumpDist(ByVal c As Route, ByVal jump As Boolean) As Double
        Dim solar1 As SolarSystem = c.Sys
        Dim num1 As Double = 0
        Do While (Not c Is Nothing)
            If (c.Jump OrElse Not jump) Then
                num1 = (num1 + frmMap.Distance(solar1, c.Sys))
                solar1 = c.Sys
            End If
            c = c.Next
        Loop
        Return num1
    End Function
    Public Function JumpRadius(ByVal from As SolarSystem, ByVal rad As Integer) As Route
        Dim solar1 As SolarSystem
        For Each solar1 In EveHQ.Core.HQ.SystemsName.Values
            solar1.Flag = False
        Next
        Dim queue1 As New Queue
        queue1.Enqueue(from)
        from.Flag = True
        Dim route1 As Route = Nothing
        Do While (queue1.Count > 0)
            Dim solar2 As SolarSystem = DirectCast(queue1.Dequeue, SolarSystem)
            Dim solar3 As SolarSystem
            For Each solar3 In solar2.Jumps
                If (frmMap.Distance(solar2, solar3) > frmMap.maxdist) Then
                    Exit For
                End If
                If solar3.EveSec >= frmMap.minjump AndAlso solar3.EveSec <= frmMap.maxjump AndAlso Not solar3.Flag AndAlso solar3.EveSec <= 0.4 Then
                    If (Exclusions.Contains(solar3.Name) = False And Exclusions.Contains(solar3.Constellation) = False And Exclusions.Contains(solar3.Region) = False) Then
                        solar3.Flag = True
                        route1 = New Route(solar3, route1)
                        queue1.Enqueue(solar3)
                    End If
                End If
            Next
            If (solar2 Is from) Then
                rad -= 1
                If ((rad = 0) OrElse (queue1.Count = 0)) Then
                    Return route1
                    Exit Function
                End If
                queue1.Enqueue(from)
                route1 = New Route(solar2, route1)
            End If
        Loop
        Return route1
    End Function
    Public Function JumpRoute(ByVal from As SolarSystem, ByVal [to] As SolarSystem) As Route
        Dim hashtable1 As New Hashtable
        Dim queue1 As New Queue
        hashtable1.Add(from, from)
        queue1.Enqueue(from)
        Do While (queue1.Count > 0)
            Dim solar1 As SolarSystem = DirectCast(queue1.Dequeue, SolarSystem)
            If (solar1 Is [to]) Then
                Dim solar2 As SolarSystem
                Dim route1 As New Route(solar1, Nothing)
                solar2 = DirectCast(hashtable1.Item(solar1), SolarSystem)
                Do While (solar2 IsNot solar1)
                    route1 = New Route(solar2, route1)
                    solar1 = solar2
                    solar2 = DirectCast(hashtable1.Item(solar1), SolarSystem)
                Loop
                Return route1
                Exit Function
            End If
            Dim solar3 As SolarSystem
            For Each solar3 In solar1.Jumps
                If (frmMap.Distance(solar1, solar3) > frmMap.maxdist) Then
                    Exit For
                End If
                If ((((solar3.EveSec >= frmMap.minjump) AndAlso (solar3.EveSec <= frmMap.maxjump)) AndAlso Not hashtable1.Contains(solar3)) AndAlso solar3.EveSec <= 0.4) Then
                    If (Exclusions.Contains(solar3.Name) = False And Exclusions.Contains(solar3.Constellation) = False And Exclusions.Contains(solar3.Region) = False) Then
                        hashtable1.Add(solar3, solar1)
                        queue1.Enqueue(solar3)
                    End If
                End If
            Next
        Loop
        Return Nothing
    End Function
    Public Shared Sub SetFlags(ByVal from As SolarSystem)
        Dim queue1 As New Queue
        queue1.Enqueue(from)
        Do While (queue1.Count > 0)
            Dim solar1 As SolarSystem = DirectCast(queue1.Dequeue, SolarSystem)
            Dim solar2 As SolarSystem
            For Each solar2 In solar1.Gates
                If (((solar2.EveSec >= frmMap.mingate) AndAlso (solar2.EveSec <= frmMap.maxgate)) AndAlso Not solar2.Flag) Then
                    solar2.Flag = True
                    queue1.Enqueue(solar2)
                End If
            Next
        Loop
    End Sub
    Private Sub ValidateSec(ByVal sender As Object, ByVal e As EventArgs) Handles nudMinSec.ValueChanged, nudMaxSec.ValueChanged
        If (sender Is Me.nudMinSec) Then
            If (Me.nudMinSec.Value > Me.nudMaxSec.Value) Then
                Me.nudMaxSec.Value = Me.nudMinSec.Value
            End If
        ElseIf (sender Is Me.nudMaxSec) Then
            If (Me.nudMinSec.Value > Me.nudMaxSec.Value) Then
                Me.nudMinSec.Value = Me.nudMaxSec.Value
            End If
        End If
    End Sub

#End Region

#Region "Map Drawing Routines"
    Private Sub tabMap_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles tabMap.Paint
        Call PinpointLocation(cboSystem.SelectedItem)
    End Sub
    Private Sub tabMapTool_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMapTool.Resize
        Dim x As Integer = tabMapTool.Width - 50
        Dim y As Integer = tabMapTool.Height - 70
        mapSize = Math.Min(x, y)
        pbMap.Width = mapSize
        pbMap.Height = mapSize
        Call Me.CreateEveMap(True)
    End Sub
    Private Sub tabMapTool_Selected(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles tabMapTool.Selected
        If tabMapTool.SelectedTab Is tabMap Then
            pbMap.Focus()
        End If
    End Sub
    Private Sub lblMapStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Refresh()
    End Sub
    Private Sub pbMap_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbMap.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            ' Make a note that we "have the mouse"
            bHaveMouse = True
            ' Store the "starting point" for this rubber-band rectangle (as screen coords)
            ptOriginal = pbMap.PointToScreen(e.Location)
            ' Special value lets us know that no previous rectangle needs erased
            ptLast.X = -1
            ptLast.Y = -1
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            bHaveScan = True
        End If
    End Sub
    Private Sub pbMap_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbMap.MouseMove

        ' Convert to screen coords
        Dim ptCurrent As Point = pbMap.PointToScreen(e.Location)

        ' If we "have the mouse", then we draw our lines
        If (bHaveMouse) Then
            ' If we have drawn previously, draw again in that spot to remove the lines
            If (ptLast.X <> -1) Then
                MyDrawReversibleRectangle(ptOriginal, ptLast)
            End If
            ' Update last point
            If Math.Abs(ptCurrent.X - ptOriginal.X) > Math.Abs(ptCurrent.Y - ptOriginal.Y) Then
                If ptCurrent.Y > ptOriginal.Y Then
                    ptCurrent.Y += Math.Abs(ptCurrent.X - ptOriginal.X) - Math.Abs(ptOriginal.Y - ptCurrent.Y)
                Else
                    ptCurrent.Y -= Math.Abs(ptCurrent.X - ptOriginal.X) - Math.Abs(ptCurrent.Y - ptOriginal.Y)
                End If
            Else
                If ptCurrent.X > ptOriginal.X Then
                    ptCurrent.X += Math.Abs(ptCurrent.Y - ptOriginal.Y) - Math.Abs(ptOriginal.X - ptCurrent.X)
                Else
                    ptCurrent.X -= Math.Abs(ptCurrent.Y - ptOriginal.Y) - Math.Abs(ptCurrent.X - ptOriginal.X)
                End If
            End If
            ptLast = ptCurrent
            ' Draw new lines
            MyDrawReversibleRectangle(ptOriginal, ptCurrent)
        Else
            If bHaveScan Then
                'if we don't "have the mouse", then let's browse the system names!
                Dim area As Integer = nudAccuracy.Value - 1
                Dim allSystems As String = ""
                For x As Integer = Math.Max(0, e.X - area) To Math.Min((pbMap.Width - 1), e.X + area)
                    For y As Integer = Math.Max(0, e.Y - area) To Math.Min((pbMap.Height - 1), e.Y + area)
                        If Mapdata(x, y) <> "" Then
                            allSystems &= Mapdata(x, y)
                        End If
                    Next
                Next
                If allSystems <> "" Then
                    Dim systems() As String = allSystems.Split(",")
                    Dim system As String = systems(0)
                    cboSystem.SelectedItem = system
                End If
            End If
        End If
    End Sub
    Private Sub pbMap_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbMap.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            ' Set internal flag to know we no longer "have the mouse"
            bHaveMouse = False
            ' If we have drawn previously, draw again in that spot
            ' to remove the lines
            If (ptLast.X <> -1) Then
                ' Convert to screen coords
                Dim ptCurrent As Point = pbMap.PointToScreen(e.Location)
                MyDrawReversibleRectangle(ptOriginal, ptLast)
                ' Convert back to map coords (from screen) to calculate the new zoom level
                ptCurrent = pbMap.PointToClient(ptCurrent)
                ptOriginal = pbMap.PointToClient(ptOriginal)
                ptLast = pbMap.PointToClient(ptLast)
                Dim newx1, newx2, newy1, newy2 As Double
                newx1 = mapX1 + ((ptOriginal.X / pbMap.Width) * (mapX2 - mapX1))
                newx2 = mapX1 + ((ptLast.X / pbMap.Width) * (mapX2 - mapX1))
                newy1 = mapY1 + ((ptOriginal.Y / pbMap.Width) * (mapY2 - mapY1))
                newy2 = mapY1 + ((ptLast.Y / pbMap.Width) * (mapY2 - mapY1))
                mapX1 = Math.Min(newx1, newx2)
                mapY1 = Math.Min(newy1, newy2)
                mapX2 = Math.Max(newx1, newx2)
                mapY2 = Math.Max(newy1, newy2)
                Call CreateEveMap(False)
            End If
            ' Set flags to know that there is no "previous" line to reverse
            ptLast.X = -1
            ptLast.Y = -1
            ptOriginal.X = -1
            ptOriginal.Y = -1
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            bHaveScan = False
        End If
    End Sub
    Private Sub pbMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbMap.Click
        pbMap.Select()
    End Sub
    Private Sub pbMap_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbMap.MouseWheel
        If tabMapTool.SelectedTab Is tabMap Then
            Dim scale As Double = 0.2
            Dim wx As Double = mapX2 - mapX1
            Dim lx As Double = wx / 2
            Dim cx As Double = mapX1 + lx
            Dim wy As Double = mapY2 - mapY1
            Dim ly As Double = wy / 2
            Dim cy As Double = mapY1 + ly
            If e.Delta < 0 Then
                ' Zoom out
                Dim dx As Double = lx * (1 / ((1 / scale) - 1))
                Dim dy As Double = ly * (1 / ((1 / scale) - 1))
                mapX1 -= dx
                mapX2 += dx
                mapY1 -= dy
                mapY2 += dy
            Else
                ' Zoom in
                Dim dx As Double = lx * scale
                Dim dy As Double = ly * scale
                mapX1 += dx
                mapX2 -= dx
                mapY1 += dy
                mapY2 -= dy
            End If

            Call Me.CreateEveMap(False)

        End If
    End Sub
    Private Sub btnShowRoute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowRoute.Click
        Call Me.ShowRoute()
    End Sub
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Call CreateEveMap(True)
    End Sub
    Private Sub PinpointLocation(ByVal solarname As String)
        Dim locX, locY As Integer
        Dim cSystem As SolarSystem
        pbMap.Image = EveMap
        pbMap.Refresh()

        Dim formGraphics As System.Drawing.Graphics
        formGraphics = pbMap.CreateGraphics()
        Dim myPen As New System.Drawing.Pen(Color.FromArgb(255, 255, 255, 255))

        ' Now show the location of our target system
        If solarname <> "" Then
            cSystem = EveHQ.Core.HQ.SystemsName(solarname)
            lblName.Text = cSystem.Name
            lblID.Text = cSystem.ID
            lblConst.Text = cSystem.Constellation
            lblRegion.Text = cSystem.Region
            lblSecurity.Text = FormatNumber(cSystem.Security, 5, TriState.True)
            lblEveSec.Text = FormatNumber(cSystem.EveSec, 1, TriState.True)
            lblNoGates.Text = cSystem.Gates.Count
            lblGates.Text = ""
            lblAllianceID.Text = SetAlliance(solarname)
            lblsovereigntyLevel.Text = SetSov(solarname)
            lblFactionID.Text = SetFaction(solarname)
            lblPlanets.Text = cSystem.Planets.Count
            lblMoons.Text = cSystem.Moons.Count
            lblABelts.Text = cSystem.ABelts.Count
            lblIBelts.Text = cSystem.IBelts.Count
            lblStations.Text = cSystem.Stations.Count
            For Each cCS As ConqStat In CSStationName.Values
                If cSystem.ID = cCS.solarSystemID Then
                    lblStations.Text = CInt(lblStations.Text) + 1
                    Exit For
                End If
            Next

            Me.cboConst.Text = cSystem.Constellation
            Me.cboRegion.Text = cSystem.Region
            Me.cboSystem.Text = cSystem.Name

            For gate As Integer = 0 To cSystem.Gates.Count - 1
                Dim tosystem As SolarSystem = cSystem.Gates(gate)
                lblGates.Text &= tosystem.Name & " (" & FormatNumber(Math.Max(0, tosystem.Security), 1, TriState.True) & ")" & ControlChars.CrLf
            Next
            If cSystem.x >= mapX1 And cSystem.x <= mapX2 And cSystem.z >= mapY1 And cSystem.z <= mapY2 Then
                ' A valid point in the map!
                locX = (pbMap.Width - 1) / (mapX2 - mapX1) * (cSystem.x - mapX1)
                locY = (pbMap.Height - 1) / (mapY2 - mapY1) * (cSystem.z - mapY1)
                myPen = New System.Drawing.Pen(Color.FromArgb(160, 255, 255, 255))
                formGraphics.DrawLine(myPen, 0, locY, pbMap.Width, locY)
                formGraphics.DrawLine(myPen, locX, 0, locX, pbMap.Height)
            End If

            ' Display station information
            Me.cboStation.Items.Clear()
            cboStation.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cboStation.AutoCompleteSource = AutoCompleteSource.CustomSource
            cboStation.BeginUpdate()
            Dim cntr As Integer = 0
            Dim cntr2 As Integer = 0
            'IF regname = "All" Then 'add ALL stations
            For Each xstation As Station In StationList.Values
                If cSystem.ID = xstation.solarSystemID Then
                    cboStation.AutoCompleteCustomSource.Add(xstation.stationName)
                    Me.cboStation.Items.Add(xstation.stationName)
                    cntr = cntr + 1
                End If
            Next
            If cntr = 0 Then
                For Each CoSt As ConqStat In CSStationName.Values
                    If cSystem.ID = CoSt.solarSystemID Then
                        cboStation.AutoCompleteCustomSource.Add(CoSt.stationName)
                        Me.cboStation.Items.Add(CoSt.stationName)
                        cntr2 = cntr2 + 1
                    End If
                Next
            End If
            If cntr2 = 0 And cntr = 0 Then
                cboStation.AutoCompleteCustomSource.Add("No Stations in system.")
                Me.cboStation.Items.Add("No Stations in system.")
            End If
            Me.cboStation.EndUpdate()
            Me.cboStation.Text = Me.cboStation.Items.Item(0).ToString

        End If

        myPen.Dispose()
        formGraphics.Dispose()

    End Sub
    Private Sub CreateEveMap(ByVal FullMap As Boolean)
        If pbMap.Height <> 0 And pbMap.Width <> 0 Then
            Dim RandomNumGen As New System.Random(Now.Second)
            Dim locX, locY As Integer
            Dim sWidth As Integer = 0
            Dim mapFont As Font = New Font("Tahoma", 7, FontStyle.Regular)
            EveMap = New Bitmap(pbMap.Width, pbMap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            ReDim Mapdata(mapSize, mapSize)
            For x As Integer = 0 To pbMap.Width
                For y As Integer = 0 To pbMap.Height
                    Mapdata(x, y) = ""
                Next
            Next
            If FullMap = True Then
                mapX1 = maxX1 : mapX2 = maxX2
                mapY1 = maxY1 : mapY2 = maxY2
            End If
            Call Me.CalcZoomLevel()
            Call Me.DrawGateLinks()
            Dim g As Graphics
            g = GetGraphicsObject()
            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
            For Each cSystem As SolarSystem In EveHQ.Core.HQ.SystemsID.Values
                If cSystem.x >= mapX1 And cSystem.x <= mapX2 And cSystem.z >= mapY1 And cSystem.z <= mapY2 Then
                    ' A valid point in the zoomed map!
                    locX = (pbMap.Width - 1) / (mapX2 - mapX1) * (cSystem.x - mapX1)
                    locY = (pbMap.Height - 1) / (mapY2 - mapY1) * (cSystem.z - mapY1)
                    Dim myBrush As SolidBrush = New SolidBrush(Me.SystemColour(cSystem.Security))
                    Dim brushSize As Integer
                    If Int(zoomLevel / 500) = 0 Then
                        brushSize = 3
                    Else
                        brushSize = Int((Math.Log(Int(zoomLevel / 500)) / Math.Log(2)) * 2) + 5
                    End If

                    g.FillEllipse(myBrush, locX - CInt((brushSize - 1) / 2), locY - CInt((brushSize - 1) / 2), brushSize, brushSize)
                    ' Draw system names at selected zoomlevels
                    Select Case zoomLevel
                        Case 500 To 1000
                            Dim rnd As Integer = RandomNumGen.Next(1, 10)
                            If rnd = 1 Then
                                sWidth = g.MeasureString(cSystem.Name, mapFont, mapSize).Width
                                g.DrawString(cSystem.Name, mapFont, myBrush, locX - (sWidth / 2), locY + brushSize + 2)
                            End If
                        Case 1000 To 1500
                            Dim rnd As Integer = RandomNumGen.Next(1, 5)
                            If rnd = 1 Then
                                sWidth = g.MeasureString(cSystem.Name, mapFont, mapSize).Width
                                g.DrawString(cSystem.Name, mapFont, myBrush, locX - (sWidth / 2), locY + brushSize + 2)
                            End If
                        Case Is > 1500
                            sWidth = g.MeasureString(cSystem.Name, mapFont, mapSize).Width
                            g.DrawString(cSystem.Name, mapFont, myBrush, locX - (sWidth / 2), locY + brushSize + 2)
                    End Select

                    Mapdata(locX, locY) &= cSystem.Name & ","
                End If
            Next

            Call Me.DrawRoute()

            pbMap.Image = EveMap
            Call PinpointLocation(cboSystem.SelectedItem)
        End If
    End Sub
    Private Sub DrawGateLinks()
        Dim locX, locY, locX2, locY2 As Integer
        Dim brightness As Double = (zoomLevel - 100) / 900 * 200
        brightness = Math.Max(0, Math.Min(200, brightness))
        Dim g As Graphics
        g = GetGraphicsObject()
        Dim myPen As New System.Drawing.Pen(Color.FromArgb(255, 0, 0, 75))
        For Each cSystem As SolarSystem In EveHQ.Core.HQ.SystemsID.Values
            For Each lSystem As SolarSystem In cSystem.Gates
                If (cSystem.x >= mapX1 And cSystem.x <= mapX2 And cSystem.z >= mapY1 And cSystem.z <= mapY2) Or (lSystem.x >= mapX1 And lSystem.x <= mapX2 And lSystem.z >= mapY1 And lSystem.z <= mapY2) Then
                    locX = (pbMap.Width - 1) / (mapX2 - mapX1) * (cSystem.x - mapX1)
                    locY = (pbMap.Height - 1) / (mapY2 - mapY1) * (cSystem.z - mapY1)
                    locX2 = (pbMap.Width - 1) / (mapX2 - mapX1) * (lSystem.x - mapX1)
                    locY2 = (pbMap.Height - 1) / (mapY2 - mapY1) * (lSystem.z - mapY1)
                    If cSystem.Region <> lSystem.Region Then
                        myPen.Color = Color.FromArgb(brightness, 100, 0, 100)
                    Else
                        If cSystem.Constellation <> lSystem.Constellation Then
                            myPen.Color = Color.FromArgb(brightness, 100, 0, 0)
                        Else
                            myPen.Color = Color.FromArgb(brightness, 0, 0, 100)
                        End If
                    End If
                    g.DrawLine(myPen, locX, locY, locX2, locY2)
                End If
            Next
        Next
    End Sub
    Private Sub DrawRoute()
        ' Draw the route if required
        If chkRoute.Checked = True Then
            If lvwRoute.Items.Count > 0 And lvwRoute.Items.ContainsKey("Invalid") = False Then
                Dim cSystem, nSystem As SolarSystem
                Dim locX, locY, locX2, locY2 As Integer
                Dim g As Graphics
                g = GetGraphicsObject()
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit
                Dim myPen As New System.Drawing.Pen(Color.FromArgb(255, 255, 255, 255))
                Dim myBrush As SolidBrush = New SolidBrush(Color.White)
                Dim sWidth As Integer = 0
                Select Case lastAlgo
                    Case 0, 1, 4
                        ' If a route is used
                        Dim mapFont As Font = New Font("Tahoma", 7, FontStyle.Regular)

                        ' Draw the starting point
                        cSystem = EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag)
                        locX = (pbMap.Width - 1) / (mapX2 - mapX1) * (cSystem.x - mapX1)
                        locY = (pbMap.Height - 1) / (mapY2 - mapY1) * (cSystem.z - mapY1)
                        g.FillEllipse(myBrush, locX - 2, locY - 2, 5, 5)
                        nSystem = EveHQ.Core.HQ.SystemsName(lvwRoute.Items(0).Name)
                        locX2 = (pbMap.Width - 1) / (mapX2 - mapX1) * (nSystem.x - mapX1)
                        locY2 = (pbMap.Height - 1) / (mapY2 - mapY1) * (nSystem.z - mapY1)
                        g.DrawLine(myPen, locX, locY, locX, locY)
                        g.DrawLine(myPen, locX, locY, locX2, locY2)
                        sWidth = g.MeasureString(cSystem.Name, mapFont, mapSize).Width
                        g.DrawString(cSystem.Name, mapFont, Brushes.White, locX - (sWidth / 2), locY + 5)

                        ' Draw the rest of them!
                        For a As Integer = 0 To lvwRoute.Items.Count - 1
                            cSystem = EveHQ.Core.HQ.SystemsName(lvwRoute.Items(a).Name)
                            locX = (pbMap.Width - 1) / (mapX2 - mapX1) * (cSystem.x - mapX1)
                            locY = (pbMap.Height - 1) / (mapY2 - mapY1) * (cSystem.z - mapY1)
                            g.FillEllipse(myBrush, locX - 2, locY - 2, 5, 5)
                            If a <> lvwRoute.Items.Count - 1 Then
                                nSystem = EveHQ.Core.HQ.SystemsName(lvwRoute.Items(a + 1).Name)
                                locX2 = (pbMap.Width - 1) / (mapX2 - mapX1) * (nSystem.x - mapX1)
                                locY2 = (pbMap.Height - 1) / (mapY2 - mapY1) * (nSystem.z - mapY1)
                                g.DrawLine(myPen, locX, locY, locX, locY)
                            End If
                            g.DrawLine(myPen, locX, locY, locX2, locY2)
                            sWidth = g.MeasureString(cSystem.Name, mapFont, mapSize).Width
                            g.DrawString(cSystem.Name, mapFont, Brushes.White, locX - (sWidth / 2), locY + 5)
                        Next
                    Case 2, 3
                        ' If a radius is used
                        Dim mapFont As Font = New Font("Tahoma", 7, FontStyle.Regular)
                        ' Draw the starting point
                        cSystem = EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag)
                        locX = (pbMap.Width - 1) / (mapX2 - mapX1) * (cSystem.x - mapX1)
                        locY = (pbMap.Height - 1) / (mapY2 - mapY1) * (cSystem.z - mapY1)
                        g.DrawEllipse(myPen, New System.Drawing.RectangleF(locX - 1, locY - 1, 3, 3))

                        ' Draw the lines
                        For a As Integer = 0 To lvwRoute.Items.Count - 1
                            cSystem = EveHQ.Core.HQ.SystemsName(lvwRoute.Items(a).Name)
                            locX2 = (pbMap.Width - 1) / (mapX2 - mapX1) * (cSystem.x - mapX1)
                            locY2 = (pbMap.Height - 1) / (mapY2 - mapY1) * (cSystem.z - mapY1)
                            g.FillEllipse(myBrush, locX2 - 2, locY2 - 2, 5, 5)
                            g.DrawLine(myPen, locX, locY, locX2, locY2)
                            sWidth = g.MeasureString(cSystem.Name, mapFont, mapSize).Width
                            g.DrawString(cSystem.Name, mapFont, Brushes.White, locX2 - (sWidth / 2), locY2 + 5)
                        Next
                End Select
            End If
        End If
    End Sub
    Private Sub MyDrawReversibleRectangle(ByVal p1 As Point, ByVal p2 As Point)
        Dim rc As Rectangle

        ' Convert the points to screen coordinates
        Dim minX As Integer = pbMap.PointToScreen(New Point(0, 0)).X
        Dim minY As Integer = pbMap.PointToScreen(New Point(0, 0)).Y
        Dim maxX As Integer = minX + pbMap.Width
        Dim maxY As Integer = minY + pbMap.Height

        ' Restrict the dimensions if > the map width
        If p2.X < minX Then p2.X = minX
        If p2.X > maxX Then p2.X = maxX
        If p2.Y < minY Then p2.Y = minY
        If p2.Y > maxY Then p2.Y = maxY

        ' Normalize the rectangle
        If (p1.X < p2.X) Then
            rc.X = p1.X
            rc.Width = p2.X - p1.X
        Else
            rc.X = p2.X
            rc.Width = p1.X - p2.X
        End If
        If (p1.Y < p2.Y) Then
            rc.Y = p1.Y
            rc.Height = p2.Y - p1.Y
        Else
            rc.Y = p2.Y
            rc.Height = p1.Y - p2.Y
        End If
        ' Draw the reversible frame
        ControlPaint.DrawReversibleFrame(rc, Color.White, FrameStyle.Dashed)
    End Sub
    Private Sub CalcZoomLevel()
        Dim dx As Double = mapX2 - mapX1
        zoomLevel = maxX / dx * 100
        lblZoom.Text = "Zoom: " & FormatNumber(zoomLevel, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
    End Sub
    Private Function GetGraphicsObject()
        Dim bmp As Bitmap
        bmp = EveMap
        Dim g As Graphics
        pbMap.Image = bmp
        g = Graphics.FromImage(bmp)
        Return g
    End Function
    Private Sub ShowRoute()
        If lvwRoute.Items.Count > 0 And lvwRoute.Items.ContainsKey("Invalid") = False Then
            Dim minx, miny, minz, maxx, maxy, maxz As Double
            minx = 1.0E+19 : miny = 1.0E+19 : minz = 1.0E+19 : maxx = -1.0E+19 : maxy = -1.0E+19 : maxz = -1.0E+19
            Dim cSystem As SolarSystem
            cSystem = EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag)
            If cSystem.x > maxx Then maxx = cSystem.x
            If cSystem.x < minx Then minx = cSystem.x
            If cSystem.y > maxy Then maxy = cSystem.y
            If cSystem.y < miny Then miny = cSystem.y
            If cSystem.z > maxz Then maxz = cSystem.z
            For a As Integer = 0 To lvwRoute.Items.Count - 1
                cSystem = EveHQ.Core.HQ.SystemsName(lvwRoute.Items(a).Name)
                If cSystem IsNot Nothing Then
                    If cSystem.x > maxx Then maxx = cSystem.x
                    If cSystem.x < minx Then minx = cSystem.x
                    If cSystem.y > maxy Then maxy = cSystem.y
                    If cSystem.y < miny Then miny = cSystem.y
                    If cSystem.z > maxz Then maxz = cSystem.z
                    If cSystem.z < minz Then minz = cSystem.z
                End If
            Next
            If (maxx - minx) > (maxz - minz) Then
                Dim diff As Double = ((maxx - minx) - (maxz - minz)) / 2
                maxz = maxz + diff
                minz = minz - diff
            Else
                Dim diff As Double = ((maxz - minz) - (maxx - minx)) / 2
                maxx = maxx + diff
                minx = minx - diff
            End If
            mapX1 = minx - 1.0E+16 : mapX2 = maxx + 1.0E+16 : mapY1 = minz - 1.0E+16 : mapY2 = maxz + 1.0E+16
            Call CreateEveMap(False)
        End If
    End Sub
    Public Function SetSov(ByVal sovname As String) As String
        Dim svSystem As SolarSystem
        Dim SovLvl As String
        Dim Tfact As String = Setfaction(sovname)
        svSystem = sovSystemsName(sovname)
        If Tfact = "Non-Empire System" Then
            SovLvl = "Not Claimed"
            Select Case svSystem.sovereigntyLevel
                Case 1
                    SovLvl = "Territory (Sovereignty 1)"
                Case 2
                    SovLvl = "Protectorate (Sovereignty 2)"
                Case 3
                    SovLvl = "Province (Sovereignty 3)"
                Case 4
                    SovLvl = "Capital (Sovereignty 4)"
                Case Else
            End Select
        Else
            SovLvl = "Cannot Be Claimed"
        End If
        Return SovLvl
    End Function
    Public Function SetFaction(ByVal SolName As String) As String
        Dim sfSystem As SolarSystem
        Dim Facname As String = "Non-Empire System"
        sfSystem = sovSystemsName(SolName)
        Dim FacID As String = sfSystem.FactionID
        Try
            For Each TFAction As Faction In FactionList.Values
                Dim Tid As String = TFAction.factionID
                If Tid = FacID Then
                    Facname = TFAction.factionName
                End If
            Next
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
        End Try

        Return Facname
    End Function
    Public Function SetAlliance(ByVal SolName As String) As String
        Dim saSystem As SolarSystem
        Dim TAlly As Alliance
        Dim Allyname As String = "Unknown"
        saSystem = sovSystemsName(SolName)
        Dim AlID As String = saSystem.AllianceID
        Try
            For a As Integer = 0 To AllianceID.Count - 1
                TAlly = AllianceID(a)
                Dim Tid As String = TAlly.ID
                If Tid = AlID Then
                    Allyname = TAlly.name
                End If
            Next
            If Allyname = "Unknown" And SetFaction(SolName) = "Non-Empire System" Then Allyname = "Not Claimed"
            If Allyname = "Unknown" And SetFaction(SolName) <> "Unknown" Then Allyname = "Cannot Be Claimed"
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
        End Try
        Return Allyname
    End Function
#End Region

#Region "IGB Routines"
    Private Function GenerateIGBCode() As String
        Dim strHTML As String = ""

        Select Case MapContext.Request.Url.AbsolutePath.ToUpper

            Case "/EVEHQMAPTOOL", "/EVEHQMAPTOOL/"
                MapContext.Response.Headers.Add("refresh:sessionchange;URL=/EVEHQMAPTOOL")
                strHTML &= EveHQ.Core.IGB.IGBHTMLHeader(MapContext, "EveHQ Map Tool")
                strHTML &= "<img src=""http://" & MapContext.Request.Headers("Host") & "/EveHQMapTool/MapOverview.png"" alt=""Map Overview"" /><br>"
                strHTML &= "<p>"
                For a As Integer = 1 To 10
                    strHTML &= "<a href='/evehqtest/number?no=" & a & "'>Number " & a & "</a><br />"
                Next
                strHTML &= "</p>"
                strHTML &= EveHQ.Core.IGB.IGBHTMLFooter(MapContext)
            Case "/EVEHQMAPTOOL/NUMBER", "/EVEHQMAPTOOL/NUMBER/"
                Dim no As Integer = MapContext.Request.QueryString("no")
                strHTML &= "<p>Number " & no & "</p>"
            Case "/EVEHQMAPTOOL/MAPOVERVIEW.PNG", "/EVEHQMAPTOOL/MAPOVERVIEW.PNG/"
                Call CreateIGBMap(300, True)
                strHTML = EveHQ.Core.IGB.GetImage(EveHQ.Core.HQ.cacheFolder & "\MapOverview.png")
        End Select
        Return strHTML
    End Function
    Private Sub CreateIGBMap(ByVal Size As Integer, ByVal FullMap As Boolean)
        Dim locX, locY As Integer
        Dim sysColor As Color
        IGBMapSize = Size
        IGBMap = New Bitmap(Size, Size, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
        ReDim IGBMapdata(Size, Size)
        For x As Integer = 0 To Size
            For y As Integer = 0 To Size
                IGBMapdata(x, y) = ""
            Next
        Next
        If FullMap = True Then
            IGBX1 = -5.8E+17 : IGBX2 = 4.2E+17
            IGBY1 = -5.0E+17 : IGBY2 = 5.0E+17
        End If
        Call Me.DrawIGBLinks()
        For Each cSystem As SolarSystem In EveHQ.Core.HQ.SystemsID.Values
            If cSystem.x >= IGBX1 And cSystem.x <= IGBX2 And cSystem.z >= IGBY1 And cSystem.z <= IGBY2 Then
                ' A valid point in the zoomed map!
                locX = (IGBMapSize - 1) / (IGBX2 - IGBX1) * (cSystem.x - IGBX1)
                locY = (IGBMapSize - 1) / (IGBY2 - IGBY1) * (cSystem.z - IGBY1)
                Select Case cSystem.Security
                    Case Is < 0.05
                        sysColor = Color.DarkRed
                    Case 0.05 To 0.15
                        sysColor = Color.DarkRed
                    Case 0.15 To 0.25
                        sysColor = Color.OrangeRed
                    Case 0.25 To 0.35
                        sysColor = Color.Orange
                    Case 0.35 To 0.45
                        sysColor = Color.Orange
                    Case 0.45 To 0.55
                        sysColor = Color.Yellow
                    Case 0.55 To 0.65
                        sysColor = Color.GreenYellow
                    Case 0.65 To 0.75
                        sysColor = Color.Green
                    Case 0.75 To 0.85
                        sysColor = Color.Green
                    Case 0.85 To 0.95
                        sysColor = Color.Green
                    Case Is > 0.95
                        sysColor = Color.Cyan
                End Select
                IGBMap.SetPixel(locX, locY, sysColor)
                IGBMapdata(locX, locY) &= cSystem.Name & ","
            End If
        Next

        'Call Me.DrawRoute()
        Call DrawIGBLocation(MapContext.Request.Headers("Eve.solarsystemname"))
        IGBMap.Save(EveHQ.Core.HQ.cacheFolder & "\MapOverview.png", System.Drawing.Imaging.ImageFormat.Png)

    End Sub
    Private Sub DrawIGBLinks()
        Dim locX, locY, locX2, locY2 As Integer
        Dim g As Graphics
        g = Graphics.FromImage(IGBMap)
        Dim myPen As New System.Drawing.Pen(Color.FromArgb(255, 0, 0, 75))
        For Each cSystem As SolarSystem In EveHQ.Core.HQ.SystemsID.Values
            For Each lSystem As SolarSystem In cSystem.Gates
                locX = (IGBMapSize - 1) / (IGBX2 - IGBX1) * (cSystem.x - IGBX1)
                locY = (IGBMapSize - 1) / (IGBY2 - IGBY1) * (cSystem.z - IGBY1)
                locX2 = (IGBMapSize - 1) / (IGBX2 - IGBX1) * (lSystem.x - IGBX1)
                locY2 = (IGBMapSize - 1) / (IGBY2 - IGBY1) * (lSystem.z - IGBY1)
                If cSystem.Region <> lSystem.Region Then
                    myPen.Color = Color.FromArgb(255, 75, 0, 0)
                Else
                    If cSystem.Constellation <> lSystem.Constellation Then
                        myPen.Color = Color.FromArgb(255, 75, 0, 75)
                    Else
                        myPen.Color = Color.FromArgb(255, 0, 0, 75)
                    End If
                End If
                g.DrawLine(myPen, locX, locY, locX2, locY2)
            Next
        Next
        g.DrawImage(IGBMap, 0, 0)
    End Sub
    Private Sub DrawIGBLocation(ByVal solarname As String)
        Dim locX, locY As Integer
        Dim cSystem As SolarSystem

        Dim g As Graphics
        g = Graphics.FromImage(IGBMap)
        Dim myPen As New System.Drawing.Pen(Color.FromArgb(255, 255, 255, 255))

        ' Now show the location of our target system
        If solarname <> "" Then
            cSystem = EveHQ.Core.HQ.SystemsName(solarname)
            lblName.Text = cSystem.Name
            lblID.Text = cSystem.ID
            lblConst.Text = cSystem.Constellation
            lblRegion.Text = cSystem.Region
            lblSecurity.Text = FormatNumber(cSystem.Security, 5, TriState.True)
            lblEveSec.Text = FormatNumber(cSystem.EveSec, 1, TriState.True)
            lblNoGates.Text = cSystem.Gates.Count
            lblGates.Text = ""
            For gate As Integer = 0 To cSystem.Gates.Count - 1
                Dim tosystem As SolarSystem = cSystem.Gates(gate)
                lblGates.Text &= tosystem.Name & " (" & FormatNumber(Math.Max(0, tosystem.Security), 1, TriState.True) & ")" & ControlChars.CrLf
            Next
            If cSystem.x >= IGBX1 And cSystem.x <= IGBX2 And cSystem.z >= IGBY1 And cSystem.z <= IGBY2 Then
                ' A valid point in the map!
                locX = (IGBMapSize - 1) / (IGBX2 - IGBX1) * (cSystem.x - IGBX1)
                locY = (IGBMapSize - 1) / (IGBY2 - IGBY1) * (cSystem.z - IGBY1)
                myPen = New System.Drawing.Pen(Color.FromArgb(160, 255, 255, 255))
                g.DrawLine(myPen, 0, locY, IGBMapSize, locY)
                g.DrawLine(myPen, locX, 0, locX, IGBMapSize)
            End If
        End If

        g.DrawImage(IGBMap, 0, 0)

    End Sub
#End Region

#Region "Waypoint Routines"
    Private Sub btnClearWP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearWP.Click
        If Waypoints.Count > 0 Then
            ' Clear all items
            lstWaypoints.Items.Clear()
            Waypoints.Clear()
            WaypointRoutes.Clear()
            If cboRouteMode.SelectedItem = "Gate Route" Then
                'WaypointRoutes.Add(lblEndSystem.Tag, Me.GateRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                WaypointRoutes(lblEndSystem.Tag) = Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
            Else
                'WaypointRoutes.Add(lblEndSystem.Tag, Me.JumpRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                WaypointRoutes(lblEndSystem.Tag) = Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
            End If
            lastAlgo = Me.cboRouteMode.SelectedIndex
            Call GenerateWaypointRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
        End If
    End Sub
    Private Sub btnAddWaypoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddWaypoint.Click
        If lstWaypoints.Items.Count = 10 Then
            MessageBox.Show("You have reached the maximum number of waypoints allowed.", "Waypoint Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            If EveHQ.Core.HQ.SystemsName.Contains(cboSystem.Text) = False Then
                MessageBox.Show("System Name is not a valid system", "System Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                If lblStartSystem.Tag = "" Or lblEndSystem.Tag = "" Then
                    MessageBox.Show("Please enter a Start and End System before entering a Waypoint.", "Start System Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim WPSystem As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.Text)
                    lstWaypoints.Items.Add(WPSystem.Name)
                    Waypoints.Add(WPSystem)
                    If chkAutoCalcRoute.Checked = True Then
                        If cboRouteMode.SelectedItem = "Gate Route" Then
                            frmMap.mingate = CDbl(Me.nudMinSec.Value)
                            frmMap.maxgate = CDbl(Me.nudMaxSec.Value)
                            If Waypoints.Count = 1 Then
                                WaypointRoutes.Add(WPSystem.Name, Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), WPSystem))
                            Else
                                Dim startIndex As Integer = Waypoints.Count - 2
                                WaypointRoutes.Add(WPSystem.Name, Me.GateRoute(Waypoints(startIndex), WPSystem))
                            End If
                            ' Calculate the final leg of the route (remove the old leg first!)
                            WaypointRoutes.Remove(lblEndSystem.Tag)
                            If lblEndSystem.Tag <> "" Then
                                WaypointRoutes.Add(lblEndSystem.Tag, Me.GateRoute(WPSystem, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                            End If
                            lastAlgo = 4
                        Else
                            If cboRouteMode.SelectedItem = "Jump Route" Then
                                Dim baseRange As Double = Me.CurrentBaseRange
                                Dim driverange As Double = (baseRange * (1 + (Me.cboJDC.SelectedIndex * 0.25)))
                                frmMap.maxdist = Math.Min(driverange, 14.625)
                                frmMap.minjump = CDbl(Me.nudMinSec.Value)
                                frmMap.maxjump = CDbl(Me.nudMaxSec.Value)
                                If Waypoints.Count = 1 Then
                                    WaypointRoutes.Add(WPSystem.Name, Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), WPSystem))
                                Else
                                    Dim startIndex As Integer = Waypoints.Count - 2
                                    WaypointRoutes.Add(WPSystem.Name, Me.JumpRoute(Waypoints(startIndex), WPSystem))
                                End If
                                ' Calculate the final leg of the route (remove the old leg first!)
                                WaypointRoutes.Remove(lblEndSystem.Tag)
                                If lblEndSystem.Tag <> "" Then
                                    WaypointRoutes.Add(lblEndSystem.Tag, Me.JumpRoute(WPSystem, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                                End If
                                lastAlgo = 4
                            End If
                        End If
                        Call GenerateWaypointRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub GenerateWaypointRoute(ByVal OriginalStartSys As SolarSystem, ByVal OriginalEndSys As SolarSystem)
        Dim baseRange As Double = Me.CurrentBaseRange
        Dim baseFuel As Double = Me.CurrentBaseFuel
        Dim fuelmultiplier As Double
        If Me.cboJFC.Visible Then
            fuelmultiplier = baseFuel * (1 - (Me.cboJFC.SelectedIndex * 0.1))
        Else
            fuelmultiplier = baseFuel
        End If
        Select Case cboShips.SelectedItem
            Case "Anshar", "Ark", "Nomad", "Rhea"
                fuelmultiplier = fuelmultiplier * (1 - (Me.cboJF.SelectedIndex * 0.05))
        End Select
        Dim totalFuel As Integer = 0
        Dim totalDist As Double = 0
        Dim count As Integer = 0
        Dim actualWaypoints As ArrayList = Waypoints.Clone
        actualWaypoints.Add(OriginalEndSys)

        lvwRoute.BeginUpdate()
        lvwRoute.Items.Clear()
        For waypointNo As Integer = 0 To actualWaypoints.Count - 1
            Dim waypoint As SolarSystem = actualWaypoints(waypointNo)
            Dim endSys As SolarSystem = waypoint
            Dim startSys As SolarSystem
            If waypointNo = 0 Then
                startSys = OriginalStartSys
            Else
                startSys = Waypoints(waypointNo - 1)
            End If
            Dim route1 As Route = WaypointRoutes(waypoint.Name)
            'text2 = String.Concat(New Object() {startSys.Name, " to ", endSys.Name, ": ", (route1.Length - 1), " jumps"})
            totalDist += route1.RouteDist
            Dim accDist As Double = 0
            Dim jumpDist As Double = 0

            Dim fuel As Integer
            Do While (route1 IsNot Nothing)
                accDist = totalDist - route1.RouteDist
                jumpDist = accDist - jumpDist
                If route1.Sys IsNot startSys Then
                    Dim newItem As ListViewItem = New ListViewItem
                    count += 1
                    newItem.Name = route1.Sys.Name
                    newItem.Text = count
                    newItem.SubItems.Add(route1.Sys.Name)
                    newItem.SubItems.Add(route1.Sys.Constellation)
                    newItem.SubItems.Add(route1.Sys.Region)
                    newItem.SubItems.Add(FormatNumber(route1.Sys.EveSec, 1, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    newItem.BackColor = Me.SystemColour(route1.Sys.Security)
                    If (cboRouteMode.SelectedItem = "Jump Route") Then
                        newItem.SubItems.Add(FormatNumber(Math.Round(jumpDist, 8, MidpointRounding.AwayFromZero), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        fuel = Int(jumpDist * fuelmultiplier)
                        totalFuel += fuel
                        newItem.SubItems.Add(FormatNumber(fuel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                        newItem.SubItems.Add(FormatNumber(fuel * 0.15, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                    End If
                    lvwRoute.Items.Add(newItem)
                End If
                jumpDist = accDist
                route1 = route1.Next
            Loop
        Next
        lvwRoute.EndUpdate()
        Select Case cboRouteMode.SelectedItem
            Case "Jump Route"
                frmMap.minjump = CDbl(Me.nudMinSec.Value)
                frmMap.maxjump = CDbl(Me.nudMaxSec.Value)
                Me.lblTotalDistance.Text = "Total Distance: " & FormatNumber(Math.Round(totalDist, 3, MidpointRounding.AwayFromZero), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ly (" & lvwRoute.Items.Count & " jumps)"
                Me.lblTotalFuel.Text = "Total Fuel: " & FormatNumber(totalFuel, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " isotopes (" & FormatNumber(totalFuel * 0.15, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3)"
            Case "Gate Route"
                frmMap.mingate = CDbl(Me.nudMinSec.Value)
                frmMap.maxgate = CDbl(Me.nudMaxSec.Value)
                Me.lblTotalDistance.Text = "Total Distance: " & lvwRoute.Items.Count
                Me.lblTotalFuel.Text = ""
        End Select
        Me.lblEuclideanDistance.Text = "Euclidean Distance: " & FormatNumber(Math.Round(frmMap.Distance(OriginalStartSys, OriginalEndSys), 8, MidpointRounding.AwayFromZero), 3, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " ly"
        Call Me.ShowRoute()
    End Sub
    Private Sub btnRemoveWaypoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveWaypoint.Click
        If lstWaypoints.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a waypoint to remove.", "No Waypoint Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            ' Get the systems either side of the removed one
            Dim newStart, newEnd, remSystem As SolarSystem
            Dim remIndex = lstWaypoints.SelectedIndex
            remSystem = Waypoints(remIndex)
            If remIndex > 0 Then
                newStart = Waypoints(remIndex - 1)
            Else
                newStart = Nothing
            End If
            If remIndex = Waypoints.Count - 2 Then
                newEnd = Waypoints(remIndex + 1)
            Else
                newEnd = Nothing
            End If

            ' Remove the items
            lstWaypoints.Items.Remove(lstWaypoints.SelectedItem)
            Waypoints.RemoveAt(remIndex)
            WaypointRoutes.Remove(remSystem.Name)

            ' Recalculate the waypoints
            If Waypoints.Count > 0 Then
                If newStart Is Nothing Then
                    ' Start from a waypoint
                    If newEnd Is Nothing Then
                        ' To the proper end system
                        If cboRouteMode.SelectedItem = "Gate Route" Then
                            'WaypointRoutes.Add(lblEndSystem.Tag, Me.GateRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                            WaypointRoutes(lblEndSystem.Tag) = Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
                        Else
                            'WaypointRoutes.Add(lblEndSystem.Tag, Me.JumpRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                            WaypointRoutes(lblEndSystem.Tag) = Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
                        End If
                    Else
                        ' From the proper start system
                        If cboRouteMode.SelectedItem = "Gate Route" Then
                            'WaypointRoutes.Add(newEnd.Name, Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), newEnd))
                            WaypointRoutes(newEnd.Name) = Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), newEnd)
                        Else
                            'WaypointRoutes.Add(newEnd.Name, Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), newEnd))
                            WaypointRoutes(newEnd.Name) = Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), newEnd)
                        End If
                    End If
                Else
                    ' Start from a waypoint
                    If newEnd Is Nothing Then
                        ' To the proper end system
                        If cboRouteMode.SelectedItem = "Gate Route" Then
                            'WaypointRoutes.Add(lblEndSystem.Tag, Me.GateRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                            WaypointRoutes(lblEndSystem.Tag) = Me.GateRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
                        Else
                            'WaypointRoutes.Add(lblEndSystem.Tag, Me.JumpRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                            WaypointRoutes(lblEndSystem.Tag) = Me.JumpRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
                        End If
                    Else
                        ' From WP to WP
                        If cboRouteMode.SelectedItem = "Gate Route" Then
                            'WaypointRoutes.Add(newEnd.Name, Me.GateRoute(newStart, newEnd))
                            WaypointRoutes(newEnd.Name) = Me.GateRoute(newStart, newEnd)
                        Else
                            'WaypointRoutes.Add(newEnd.Name, Me.JumpRoute(newStart, newEnd))
                            WaypointRoutes(newEnd.Name) = Me.JumpRoute(newStart, newEnd)
                        End If
                    End If
                End If
            Else
                If cboRouteMode.SelectedItem = "Gate Route" Then
                    'WaypointRoutes.Add(lblEndSystem.Tag, Me.GateRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                    WaypointRoutes(lblEndSystem.Tag) = Me.GateRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
                Else
                    'WaypointRoutes.Add(lblEndSystem.Tag, Me.JumpRoute(newStart, EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag)))
                    WaypointRoutes(lblEndSystem.Tag) = Me.JumpRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
                End If
            End If
            Call GenerateWaypointRoute(EveHQ.Core.HQ.SystemsName(lblStartSystem.Tag), EveHQ.Core.HQ.SystemsName(lblEndSystem.Tag))
        End If
    End Sub
    Private Sub btnOptimalWP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptimalWP.Click
        ' Try and find the optimal waypoint configuration

        If lstWaypoints.Items.Count = 0 Then
            MessageBox.Show("There are no waypoints to optimise!", "Optimisation Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else

            Dim startTime, endTime As DateTime
            Dim timeDiff As TimeSpan

            startTime = Now

            ' Step 1 - Work out the number of permutations
            Dim Waypoints As ArrayList = New ArrayList
            Waypoints.Add(lblStartSystem.Tag)
            For Each waypoint As String In lstWaypoints.Items
                Waypoints.Add(waypoint)
            Next
            Waypoints.Add(lblEndSystem.Tag)

            Dim WPcount As Integer = Waypoints.Count
            Dim WPstring As String = ""
            For WP As Integer = 1 To WPcount - 1
                WPstring &= Hex(WP)
            Next
            Dim routes(,) As String = StringPermutations(WPstring)

            ' Step 2 - Work out the routing from each of the systems
            Dim routeCosts(WPcount - 1, WPcount - 1)
            For Waypoint As Integer = 0 To Waypoints.Count - 1
                Dim s, t As SolarSystem
                s = EveHQ.Core.HQ.SystemsName(Waypoints(Waypoint))
                t = EveHQ.Core.HQ.SystemsName(Waypoints(Waypoint))
                Dim minSec As Double = nudMinSec.Value
                Dim maxSec As Double = nudMaxSec.Value
                Dim myRoute As Dijkstra = New Dijkstra
                Dim route As ArrayList = myRoute.GetPath(s, t, True, minSec, maxSec)
                routeCosts(Waypoint, Waypoint) = route.Count
                For Waypoint2 As Integer = 0 To Waypoints.Count - 1
                    If Waypoint <> Waypoint2 Then
                        s = EveHQ.Core.HQ.SystemsName(Waypoints(Waypoint))
                        t = EveHQ.Core.HQ.SystemsName(Waypoints(Waypoint2))
                        route = myRoute.GetPath(s, t, False)
                        routeCosts(Waypoint, Waypoint2) = route.Count
                    End If
                Next
            Next

            ' Step 3 - Calculate costs for all the permutations
            Dim minCost As Integer = 1000000
            Dim minRoute As String = ""
            Dim totalCosts As SortedList = New SortedList
            Dim routeString As String = ""
            Dim router, route1, route2 As Integer
            Dim startRoute, endRoute As Integer
            Dim totalcost As Integer = 0
            For route1 = 0 To routes.GetUpperBound(0)
                For route2 = 0 To routes.GetUpperBound(1)
                    routeString = "0" & routes(route1, route2)
                    totalcost = 0
                    For router = 0 To routeString.Length - 2
                        startRoute = Convert.ToInt32(routeString.Substring(router, 1), 16)
                        endRoute = Convert.ToInt32(routeString.Substring(router + 1, 1), 16)
                        totalcost += routeCosts(startRoute, endRoute) - 1
                        If totalcost > minCost Then Exit For
                    Next
                    ' Adjust for duplicated systems (end of one waypoint, start of another)
                    If totalcost < minCost Then
                        minCost = totalcost
                        minRoute = routeString
                        totalCosts.Add(routeString, totalcost)
                    End If
                Next
            Next

            endTime = Now
            timeDiff = endTime - startTime

            Dim msg As String = "Optimal cost: " & minCost & ControlChars.CrLf & "Route Index: " & minRoute & ControlChars.CrLf
            msg &= "Route:" & ControlChars.CrLf
            For route As Integer = 0 To minRoute.Length - 1
                msg &= "        " & Waypoints(Convert.ToInt32(minRoute.Substring(route, 1), 16)) & ControlChars.CrLf
            Next
            msg &= ControlChars.CrLf & "Time Taken: " & timeDiff.TotalSeconds & "s"
            MessageBox.Show(msg, "Waypoints Optimised", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Dim curRoute As String = "0" & WPstring
            Dim curIndex As Integer = totalCosts.IndexOfKey(curRoute)
            Dim curCost As Integer = totalCosts.GetByIndex(curIndex)

            MessageBox.Show("Current cost: " & curCost & ControlChars.CrLf & "Route Index: " & curRoute, "Waypoints Optimised", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

    End Sub
#End Region

#Region "Exclusion Routines"
    Private Sub btnExclude_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExclude.Click
        ' Exclude the system
        Dim eSys As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.SelectedItem)
        Call ExcludeSystem(eSys)
    End Sub
    Private Sub ExcludeSystem(ByVal eSys As SolarSystem)
        If lvwExclusions.Items.ContainsKey(eSys.Name) = False Then
            Dim newExclusion As New ListViewItem
            newExclusion.Name = eSys.Name
            newExclusion.Text = eSys.Name
            newExclusion.SubItems.Add("Sys")
            lvwExclusions.Items.Add(newExclusion)
            Exclusions.Add(eSys.Name, eSys.Name)
        End If
    End Sub
    Private Sub ExcludeConstellation(ByVal eSys As SolarSystem)
        If lvwExclusions.Items.ContainsKey(eSys.Constellation) = False Then
            Dim newExclusion As New ListViewItem
            newExclusion.Name = eSys.Constellation
            newExclusion.Text = eSys.Constellation
            newExclusion.SubItems.Add("Con")
            lvwExclusions.Items.Add(newExclusion)
            Exclusions.Add(eSys.Constellation, eSys.Constellation)
        End If
    End Sub
    Private Sub ExcludeRegion(ByVal eSys As SolarSystem)
        If lvwExclusions.Items.ContainsKey(eSys.Region) = False Then
            Dim newExclusion As New ListViewItem
            newExclusion.Name = eSys.Region
            newExclusion.Text = eSys.Region
            newExclusion.SubItems.Add("Reg")
            lvwExclusions.Items.Add(newExclusion)
            Exclusions.Add(eSys.Region, eSys.Region)
        End If
    End Sub

    Private Sub mnuExcludeSystem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExcludeSystem.Click
        ' Exclude the system
        Dim eSys As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.SelectedItem)
        Call ExcludeSystem(eSys)
    End Sub

    Private Sub mnuExcludeConstellation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExcludeConstellation.Click
        ' Exclude the constellation
        Dim eSys As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.SelectedItem)
        Call ExcludeConstellation(eSys)
    End Sub

    Private Sub mnuExcludeRegion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExcludeRegion.Click
        ' Exclude the region
        Dim eSys As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.SelectedItem)
        Call ExcludeRegion(eSys)
    End Sub

    Private Sub btnRemoveExclusion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveExclusion.Click
        If lvwExclusions.SelectedItems.Count > 0 Then
            Exclusions.Remove(lvwExclusions.SelectedItems(0).Name)
            lvwExclusions.Items.RemoveByKey(lvwExclusions.SelectedItems(0).Name)
        End If
    End Sub
#End Region

#Region "Agent Search Routines"
    Private Sub btnSearchAgents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchAgents.Click
        Call Me.PopulateAgentList(cboAgentFaction.SelectedItem, cboAgentCorp.SelectedItem, cboAgentDivision.SelectedItem)
    End Sub
    Public Sub PopulateAgentList(ByVal xfact As String, ByVal xcorp As String, ByVal xdiv As String)
        Dim AgInd As Integer = 0
        Dim agtest As Boolean = False
        If xfact = "All" And xcorp = "All" And xdiv = "All" Then agtest = True
        lvwAgents.BeginUpdate()
        lvwAgents.Items.Clear()

        ' Calculate a gate route
        Dim myRoute As Dijkstra = New Dijkstra
        Dim minSec As Double = nudMinSec.Value
        Dim maxSec As Double = nudMaxSec.Value
        Dim startSys As SolarSystem = EveHQ.Core.HQ.SystemsName(cboSystem.SelectedItem)
        Dim ndSys As SolarSystem = startSys.Gates(0)
        Call myRoute.GetPath(startSys, ndSys, True, minSec, maxSec)

        For Each cAgent As Agent In AgentID.Values
            agtest = CheckAgent(cAgent)
            If agtest = True Then
                Dim agstat As Station = StationList(cAgent.stationId)
                Dim agsystem As SolarSystem = EveHQ.Core.HQ.SystemsID(agstat.solarSystemID)
                Dim xsid As Integer = CInt(cAgent.stationId)
                'lvAgSerAg.Items.Add(agsystem.Security) ' system sec
                'lvAgSerAg.Items(AgInd).SubItems.Add(cAgent.agentName) 'agent name
                Dim xcCID As Integer = CInt(cAgent.corporationID)
                Dim xcorpid As String = eveNames(xcCID).itemname
                'lvAgSerAg.Items(AgInd).SubItems.Add(xcorpid) 'agent corp
                Dim tcorp As NPCCorp = NPCCorpList(xcCID)
                Dim xcFID As Integer = CInt(tcorp.factionID)
                Dim xfacname = eveNames(xcFID).itemName
                Dim newAgent As New ListViewItem
                newAgent.Text = cAgent.agentName
                newAgent.SubItems.Add(xcorpid)
                newAgent.SubItems.Add(xfacname) ' faction name
                newAgent.SubItems.Add(cAgent.Level) 'lvl
                newAgent.SubItems.Add(cAgent.Quality) 'quality
                ' Get the distance
                Dim route As ArrayList = myRoute.GetPath(startSys, agsystem, False)
                If route IsNot Nothing Then
                    newAgent.SubItems.Add(route.Count - 1) 'Dist
                Else
                    newAgent.SubItems.Add("N/A")
                End If
                newAgent.SubItems.Add(FormatNumber(agsystem.Security, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                newAgent.SubItems.Add(agsystem.Region) 'region
                newAgent.SubItems.Add(agsystem.Constellation) 'cont
                newAgent.SubItems.Add(agsystem.Name) 'systm
                newAgent.SubItems.Add(eveNames(agstat.stationID).itemname) 'station
                Select Case CInt(cAgent.Type)
                    Case 1
                        ' Non Agent
                        newAgent.SubItems.Add(" ")
                    Case 2
                        'Basic Agent
                        newAgent.SubItems.Add("Basic Agent")
                    Case 3
                        ' TutorialAgent
                        newAgent.SubItems.Add("Tutorial Agent")
                    Case 4
                        ' ResearchAgent
                        newAgent.SubItems.Add("Research Agent")
                    Case 5
                        ' CONCORDAgent
                        newAgent.SubItems.Add("Concord Agent")
                    Case 6
                        ' GenericStorylineMissionAgent
                        newAgent.SubItems.Add("Storyline Agent")
                    Case 7
                        ' StorylineMissionAgent
                        newAgent.SubItems.Add("Storyline Agent")
                    Case 8
                        ' EventMissionAgent
                        newAgent.SubItems.Add("Event Agent")
                End Select
                lvwAgents.Items.Add(newAgent)
            End If
        Next
        lvwAgents.EndUpdate()
    End Sub

    Private Function CheckAgent(ByVal xAgent As Agent) As Boolean

        Dim checkag As Boolean = False
        Dim agcid As Integer = xAgent.corporationID
        Dim agcorp As NPCCorp = NPCCorpList(agcid)
        Dim statid As Integer = xAgent.stationId
        Dim agstat As Station = StationList(statid)
        Dim agsysid As Integer = agstat.solarSystemID
        Dim agsys As SolarSystem = EveHQ.Core.HQ.SystemsID(agsysid)
        Dim agfid As Integer = Int(NPCCorpList(agcid).factionid)
        Dim agfacnam As String = eveNames(agfid).itemName
        Dim agsysnam As String = agsys.Name
        Dim agreg As String = EveHQ.Core.HQ.SystemsID(agsysid).region
        Dim agcon As String = EveHQ.Core.HQ.SystemsID(agsysid).constellation
        Dim checsec As Boolean = False
        Dim checqual As Boolean = False
        Dim checlvl As Boolean = False
        Dim checloc As Boolean = False
        Dim checfac As Boolean = False
        Dim checdiv As Boolean = False
        Dim checcorp As Boolean = False
        Dim checassaoc As Boolean = False

        'checking level
        Select Case xAgent.Level
            Case Is = 1
                If chkLevel1Agent.Checked = True Then checlvl = True
            Case Is = 2
                If chkLevel2Agent.Checked = True Then checlvl = True
            Case Is = 3
                If chkLevel3Agent.Checked = True Then checlvl = True
            Case Is = 4
                If chkLevel4Agent.Checked = True Then checlvl = True
            Case Is = 5
                If chkLevel5Agent.Checked = True Then checlvl = True
        End Select
        If chkLevel1Agent.Checked = False And chkLevel2Agent.Checked = False And chkLevel3Agent.Checked = False And chkLevel4Agent.Checked = False And chkLevel5Agent.Checked = False Then checlvl = True

        'checking quality
        Select Case xAgent.Quality
            Case Is >= 0
                If chkAgentHighQ.Checked = True Then checqual = True
            Case Is < 0
                If chkAgentLowQ.Checked = True Then checqual = True
        End Select
        If chkAgentHighQ.Checked = False And chkAgentLowQ.Checked = False Then checqual = True

        'checking security status
        Select Case agsys.Security
            Case 0.45 To 1
                If chkAgentEmpire.Checked = True Then checsec = True
            Case 0.05 To 0.45
                If chkAgentLowSec.Checked = True Then checsec = True
            Case -1 To 0.05
                If chkAgentNullSec.Checked = True Then checsec = True
        End Select
        If chkAgentEmpire.Checked = False And chkAgentLowSec.Checked = False And chkAgentNullSec.Checked = False Then checsec = True

        'checking location
        If chkAgentSystem.Checked = True Then
            If (agsysnam = cboSystem.SelectedItem Or cboSystem.SelectedItem = "All") Or checloc = True Then checloc = True
        End If
        If chkAgentConst.Checked = True Then
            If (agcon = cboConst.SelectedItem Or cboConst.SelectedItem = "All") Or checloc = True Then checloc = True
        End If
        If chkAgentRegion.Checked = True Then
            If (agreg = cboRegion.SelectedItem Or cboRegion.SelectedItem = "All") Or checloc = True Then checloc = True
        End If
        If chkAgentSystem.Checked = False And chkAgentConst.Checked = False And chkAgentRegion.Checked = False Then checloc = True

        'checking faction
        If cboAgentFaction.SelectedItem = agfacnam Or cboAgentFaction.SelectedItem = "All" Then checfac = True
        'checking corp
        If cboAgentCorp.SelectedItem = eveNames(agcid).itemname Or cboAgentCorp.SelectedItem = "All" Then checcorp = True
        'checking div
        If cboAgentDivision.SelectedItem = NPCDivID(xAgent.divisionID).divisionName Or cboAgentDivision.SelectedItem = "All" Then checdiv = True
        If checfac = True And checcorp = True And checdiv = True Then checassaoc = True

        If checsec = True And checlvl = True And checqual = True And checloc = True And checassaoc = True Then checkag = True

        Return checkag
    End Function
    Private Sub lvwAgents_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwAgents.ColumnClick
        If lvwAgents.Tag = e.Column Then
            Me.lvwAgents.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwAgents.Tag = -1
        Else
            Me.lvwAgents.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwAgents.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwAgents.Sort()
    End Sub
    Private Sub cboAgentFaction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAgentFaction.SelectedIndexChanged
        SetAgentFactionCorps(cboAgentFaction.SelectedItem)
    End Sub

    Private Sub cboAgentCorp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAgentCorp.SelectedIndexChanged
        SetAgentCorpDivisions(cboAgentCorp.SelectedItem)
    End Sub
    Public Sub SetAgentFactionCorps(ByVal agsfact As String)
        Dim tname As EveName
        Me.cboAgentCorp.Items.Clear()
        cboAgentCorp.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboAgentCorp.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboAgentCorp.BeginUpdate()
        cboAgentCorp.AutoCompleteCustomSource.Add("All")
        Me.cboAgentCorp.Items.Add("All")
        For Each cCorp As NPCCorp In NPCCorpList.Values
            Dim cid As Integer = cCorp.CorporationID
            tname = eveNames(cid)
            cboAgentCorp.AutoCompleteCustomSource.Add(tname.itemName)
            Me.cboAgentCorp.Items.Add(tname.itemName)
        Next
        Me.cboAgentCorp.EndUpdate()
        Me.cboAgentCorp.Text = "All"
    End Sub

    Public Sub SetAgentCorpDivisions(ByVal agscorp As String)
        Me.cboAgentDivision.Items.Clear()
        cboAgentDivision.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboAgentDivision.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboAgentDivision.BeginUpdate()
        cboAgentDivision.AutoCompleteCustomSource.Add("All")
        Me.cboAgentDivision.Items.Add("All")
        For Each cDiv As NPCDiv In NPCDivID.Values
            cboAgentDivision.AutoCompleteCustomSource.Add(cDiv.divisionName)
            Me.cboAgentDivision.Items.Add(cDiv.divisionName)
        Next
        Me.cboAgentDivision.EndUpdate()
        Me.cboAgentDivision.Text = "All"
    End Sub
#End Region

#Region "Celestial Routines"
    Private Sub SearchCelestial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchCelestial.Click
        Call Me.PopulateCelestialLists()
    End Sub
    Private Sub PopulateCelestialLists()

        Dim systemArray As New ArrayList
        If radCelSystem.Checked = True Then
            systemArray.Add(cboSystem.SelectedItem)
        Else
            For Each searchSys As SolarSystem In EveHQ.Core.HQ.SystemsName.Values
                If radCelConst.Checked = True And searchSys.Constellation = cboConst.SelectedItem Then
                    systemArray.Add(searchSys.Name)
                Else
                    If radCelRegion.Checked = True And searchSys.Region = cboRegion.SelectedItem Then
                        systemArray.Add(searchSys.Name)
                    End If
                End If
            Next
        End If
        lvPlanets.BeginUpdate()
        lvPlanets.Sorting = SortOrder.None
        lvPlanets.Items.Clear()
        lvMoons.BeginUpdate()
        lvMoons.Sorting = SortOrder.None
        lvMoons.Items.Clear()
        lvBelts.BeginUpdate()
        lvBelts.Sorting = SortOrder.None
        lvBelts.Items.Clear()
        For Each sys As String In systemArray
            Dim celSystem As SolarSystem = EveHQ.Core.HQ.SystemsName(sys)
            For Each celBody As String In celSystem.Planets.Values
                lvPlanets.Items.Add(celBody)
            Next
            For Each celBody As String In celSystem.Moons.Values
                lvMoons.Items.Add(celBody)
            Next
            For Each celBody As String In celSystem.ABelts.Values
                lvBelts.Items.Add(celBody)
            Next
        Next
        lvPlanets.Sorting = SortOrder.Ascending
        lvPlanets.EndUpdate()
        lvMoons.Sorting = SortOrder.Ascending
        lvMoons.EndUpdate()
        lvBelts.Sorting = SortOrder.Ascending
        lvBelts.EndUpdate()
        lblCelPlanets.Text = "Planets (" & FormatNumber(lvPlanets.Items.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " found)"
        lblCelMoons.Text = "Moons (" & FormatNumber(lvMoons.Items.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " found)"
        lblCelBelts.Text = "Belts (" & FormatNumber(lvBelts.Items.Count, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " found)"
    End Sub
#End Region

#Region "Station Routines"
    Private Sub cboStation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStation.SelectedIndexChanged
        Call Me.SetStationInfo(cboStation.SelectedItem)
    End Sub
    Public Sub SetStationInfo(ByVal statname As String)
        Dim cStation As Station
        Dim xcntr As Integer = 0
        lblStationCorp.Text = ""
        lblStationFaction.Text = ""
        lblStationServices.Text = ""
        lvagnts.Items.Clear()
        If statname <> "" Or statname <> "No Stations in system." Then
            xcntr = 1
            For Each cStation In StationList.Values
                If cStation.stationName = statname Then
                    xcntr = 2
                    Dim xCID As Integer = CInt(cStation.corporationID)
                    lblStationCorp.Text = eveNames(xCID).itemname
                    Dim tcorp As NPCCorp = NPCCorpList(xCID)
                    Dim xFID As Integer = CInt(tcorp.factionID)
                    lblStationFactionLbl.Text = "Faction:"
                    lblStationFaction.Text = eveNames(xFID).itemName
                    lblStationServices.Text = SetStationServices(cStation.operationID)
                    Dim SID As Integer = CInt(cStation.stationID)
                    lvagnts.Items.Clear()
                    Dim AgInd As Integer = 0
                    For Each cAgent As Agent In AgentID.Values
                        Dim xsid As Integer = CInt(cAgent.stationId)
                        If SID = xsid Then
                            lvagnts.Items.Add(cAgent.agentName)
                            Dim xcCID As Integer = CInt(cAgent.corporationID)
                            Dim xcorpid As String = eveNames(xCID).itemname
                            lvagnts.Items(AgInd).SubItems.Add(xcorpid)
                            Dim tccorp As NPCCorp = NPCCorpList(xCID)
                            Dim xcFID As Integer = CInt(tccorp.factionID)
                            Dim xfacname = eveNames(xcFID).itemName
                            lvagnts.Items(AgInd).SubItems.Add(xfacname)
                            lvagnts.Items(AgInd).SubItems.Add(cAgent.Level)
                            lvagnts.Items(AgInd).SubItems.Add(cAgent.Quality)
                            AgInd = AgInd + 1
                        End If
                    Next
                End If
            Next
        End If
        If xcntr = 1 Then
            For Each cCS As ConqStat In CSStationName.Values
                If cCS.stationName = statname Then
                    lblStationCorp.Text = cCS.corporationName
                    lvagnts.Items.Clear()
                    Dim cx As Integer = CInt(cCS.stationTypeID)
                    Dim xctype As StType = StationTypes(cx)
                    Dim cy As Integer = xctype.OperationID
                    lblStationServices.Text = SetStationServices(cy)
                    Dim ssid As Integer = cCS.solarSystemID + 30000000
                    Dim ssname As EveName = eveNames(ssid)
                    Dim ssaid As String = ssname.itemName
                    lblStationFactionLbl.Text = "Alliance:"
                    lblStationFaction.Text = SetAlliance(ssaid)
                End If
            Next
        End If
        xcntr = 0
    End Sub

    Public Function SetStationServices(ByVal OID As Integer) As String
        Dim csID As Integer
        Dim csID2 As Integer
        Dim rServ As String = ""
        Dim Top As Operation
        Dim Tserv As Service
        Dim TL As SortedList = New SortedList
        For cX As Integer = 0 To OperationList.Count - 1 'scan all operations
            Top = OperationList(cX)
            If Top.operationID = OID Then
                csID = Top.serviceID
                For cS As Integer = 0 To ServiceList.Count - 1
                    Tserv = ServiceList(cS)
                    csID2 = Tserv.serviceID
                    If csID2 = csID Then
                        TL.Add(Tserv.serviceName, Tserv.serviceName)
                        'rServ = rServ & Tserv.serviceName & ControlChars.CrLf
                    End If
                Next
            End If
        Next
        For Each tl2 As String In TL.Values
            rServ = rServ & tl2 & ControlChars.CrLf
        Next
        Return rServ
    End Function
#End Region

End Class


