Imports System.IO
Imports System.Windows.Forms
Imports System.Xml
Imports System.Runtime.Serialization.Formatters.Binary

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn

    Dim mapFolder As String = ""
    Shared mapCacheFolder As String = ""
    Dim UseSerializableData As Boolean = False

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
        'MapContext = IGBContext
        'Return Me.GenerateIGBCode
        Return ""
    End Function

    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return New frmMap
    End Function

    Public Function GetPlugInData(ByVal Data As Object, Optional ByVal DataType As Integer = 0) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Return Nothing
    End Function
#End Region

#Region "Data Loading Routines"
    Public Shared SystemsID As New SortedList(Of String, SolarSystem)
    Public Shared SystemNameToID As New SortedList(Of String, String)
    Public Shared NPCCorpList As New SortedList
    Public Shared NPCDivID As New SortedList
    Public Shared eveNames As New SortedList
    Public Shared RegionID As New SortedList(Of String, Region)
    Public Shared ConstellationID As New SortedList(Of String, Constellation)
    Public Shared AgentID As New SortedList
    Public Shared ServiceList As New SortedList
    Public Shared OperationList As New SortedList
    Public Shared FactionList As New SortedList
    Public Shared AllianceList As New SortedList
    Public Shared StationList As New SortedList
    Public Shared CSStationList As New SortedList
    Public Shared StationTypes As New SortedList
    Public Shared OreClassList As New SortedList
    Dim SystemJumps As New SortedList
    Dim JumpList As New ArrayList
    Public Shared LowMemoryMode As Boolean = False
    Private Function LoadDataFromDatabase() As Boolean
        If Me.LoadSystems() = False Then
            ReportError("LoadSystems failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadOreData() = False Then
            ReportError("LoadOreData failed", "Problem Loading Data")
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
        If Me.LoadFactions() = False Then
            ReportError("LoadFactions failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If LoadAlliances() = False Then
            ReportError("LoadAlliances failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If Me.LoadStations() = False Then
            ReportError("LoadStations failed", "Problem Loading Data")
            Return False
            Exit Function
        End If
        If LoadSov() = False Then
            ReportError("LoadSov failed", "Problem Loading Data")
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
        If LoadConq() = False Then
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
        eveNames.Clear()
        GC.Collect()
        Call PlugInData.SaveSerializedData()
        If LowMemoryMode = False Then
            Call ConvertJumpsToSystems()
        End If
        GC.Collect()
        Return True
    End Function
    Private Function LoadSerializedData() As Boolean

        Dim startTime, endtime As DateTime
        Dim timeTaken As TimeSpan
        Dim s As FileStream
        Dim f As BinaryFormatter

        startTime = Now
        Dim sysLists(20) As SortedList
        PlugInData.SystemsID.Clear()
        For sysList As Integer = 0 To 20
            s = New FileStream(mapCacheFolder & "\SolarSystems" & sysList & ".bin", FileMode.Open)
            f = New BinaryFormatter
            sysLists(sysList) = CType(f.Deserialize(s), SortedList)
            s.Close()
            For Each sys As SolarSystem In sysLists(sysList).Values
                SystemsID.Add(sys.ID.ToString, sys)
            Next
        Next
        For Each cSystem As SolarSystem In PlugInData.SystemsID.Values
            SystemNameToID.Add(cSystem.Name, cSystem.ID.ToString)
        Next

        s = New FileStream(mapCacheFolder & "\Regions.bin", FileMode.Open)
        f = New BinaryFormatter
        RegionID = CType(f.Deserialize(s), SortedList(Of String, Region))
        s.Close()

        s = New FileStream(mapCacheFolder & "\Constellations.bin", FileMode.Open)
        f = New BinaryFormatter
        ConstellationID = CType(f.Deserialize(s), SortedList(Of String, Constellation))
        s.Close()

        s = New FileStream(mapCacheFolder & "\OreClass.bin", FileMode.Open)
        f = New BinaryFormatter
        OreClassList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\NPCCorps.bin", FileMode.Open)
        f = New BinaryFormatter
        NPCCorpList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Factions.bin", FileMode.Open)
        f = New BinaryFormatter
        FactionList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Alliances.bin", FileMode.Open)
        f = New BinaryFormatter
        AllianceList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Stations.bin", FileMode.Open)
        f = New BinaryFormatter
        StationList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Operations.bin", FileMode.Open)
        f = New BinaryFormatter
        OperationList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Services.bin", FileMode.Open)
        f = New BinaryFormatter
        ServiceList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Agents.bin", FileMode.Open)
        f = New BinaryFormatter
        AgentID = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Conquerables.bin", FileMode.Open)
        f = New BinaryFormatter
        CSStationList = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\StationTypes.bin", FileMode.Open)
        f = New BinaryFormatter
        StationTypes = CType(f.Deserialize(s), SortedList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\NPCDivs.bin", FileMode.Open)
        f = New BinaryFormatter
        NPCDivID = CType(f.Deserialize(s), SortedList)
        s.Close()
        endtime = Now
        timeTaken = endtime - startTime
        'MessageBox.Show("Cache Load Time: " & timeTaken.TotalSeconds)

        If LowMemoryMode = False Then
            Call ConvertJumpsToSystems()
        End If

        GC.Collect()

        Return True
    End Function
    Public Sub ReportError(ByVal Note As String, ByVal Title As String)
        MessageBox.Show(Note, Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Public Function LoadSystems() As Boolean
        Dim strSQL As String = "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName"
        strSQL &= " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;"
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    PlugInData.SystemsID.Clear()
                    Dim cSystem As SolarSystem = New SolarSystem
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        cSystem = New SolarSystem
                        cSystem.ID = CInt(systemData.Tables(0).Rows(solar).Item("solarSystemID")) - 30000000
                        cSystem.Name = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemName"))
                        cSystem.Region = CStr(systemData.Tables(0).Rows(solar).Item("regionName"))
                        cSystem.RegionID = CInt(systemData.Tables(0).Rows(solar).Item("mapSolarSystems_regionID"))
                        cSystem.Constellation = CStr(systemData.Tables(0).Rows(solar).Item("constellationName"))
                        cSystem.x = CDbl(systemData.Tables(0).Rows(solar).Item("x"))
                        cSystem.y = CDbl(systemData.Tables(0).Rows(solar).Item("y"))
                        cSystem.z = CDbl(systemData.Tables(0).Rows(solar).Item("z"))
                        cSystem.Security = CDbl(systemData.Tables(0).Rows(solar).Item("security"))
                        cSystem.EveSec = Math.Max(Int((cSystem.Security * 10) + 0.5) / 10, 0)
                        If IsDBNull(systemData.Tables(0).Rows(solar).Item("securityClass")) = False Then
                            cSystem.SecClass = CStr(systemData.Tables(0).Rows(solar).Item("securityClass"))
                        Else
                            cSystem.SecClass = ""
                        End If
                        cSystem.Ice = GetIce(CInt(cSystem.RegionId), cSystem.Security)

                        cSystem.Flag = False
                        PlugInData.SystemsID.Add(cSystem.ID.ToString, cSystem)
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

    Public Function LoadOreData() As Boolean
        OreClassList.Add("A", "Veldspar, Scordite")
        OreClassList.Add("B", "Veldspar, Scordite, Pyroxeres")
        OreClassList.Add("B1", "Veldspar, Scordite, Pyroxeres, Kernite")
        OreClassList.Add("B2", "Veldspar, Scordite, Pyroxeres, Kernite, Jaspet")
        OreClassList.Add("B3", "Veldspar, Scordite, Pyroxeres, Kernite, Jaspet, Hemorphite")
        OreClassList.Add("C", "Veldspar, Scordite, Plagioclase, Pyroxeres")
        OreClassList.Add("C1", "Veldspar, Scordite, Plagioclase, Pyroxeres, Kernite")
        OreClassList.Add("C2", "Veldspar, Scordite, Plagioclase, Pyroxeres, Kernite, Hedbergite")
        OreClassList.Add("D", "Veldspar, Scordite, Plagioclase")
        OreClassList.Add("D1", "Veldspar, Scordite, Plagioclase, Omber")
        OreClassList.Add("D2", "Veldspar, Scordite, Plagioclase, Omber, Jaspet")
        OreClassList.Add("D3", "Veldspar, Scordite, Plagioclase, Omber, Jaspet, Hemorphite")
        OreClassList.Add("E", "Veldspar, Scordite, Plagioclase, Omber, Kernite")
        OreClassList.Add("E1", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Hedbergite")
        OreClassList.Add("F", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber")
        OreClassList.Add("F1", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain")
        OreClassList.Add("F2", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss")
        OreClassList.Add("F3", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot")
        OreClassList.Add("F4", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor")
        OreClassList.Add("F5", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor, Pyroxeres")
        OreClassList.Add("F6", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor, Pyroxeres, Mercoxit")
        OreClassList.Add("F7", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor, Pyroxeres, Mercoxit, Plagioclase")
        OreClassList.Add("G", "Veldspar, Scordite, Plagioclase, Omber, Kernite")
        OreClassList.Add("G1", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss")
        OreClassList.Add("G2", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres")
        OreClassList.Add("G3", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain")
        OreClassList.Add("G4", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot")
        OreClassList.Add("G5", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot, Crokite")
        OreClassList.Add("G6", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot, Crokite, Mercoxit")
        OreClassList.Add("G7", "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot, Crokite, Mercoxit, Dark Ochre")
        OreClassList.Add("H", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet")
        OreClassList.Add("H1", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite")
        OreClassList.Add("H2", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre")
        OreClassList.Add("H3", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite")
        OreClassList.Add("H4", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite")
        OreClassList.Add("H5", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite, Spodumain")
        OreClassList.Add("H6", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite, Spodumain, Mercoxit")
        OreClassList.Add("H7", "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite, Spodumain, Mercoxit, Bistot")
        OreClassList.Add("I", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber")
        OreClassList.Add("I1", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet")
        OreClassList.Add("I2", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain")
        OreClassList.Add("I3", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss")
        OreClassList.Add("I4", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre")
        OreClassList.Add("I5", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre, Arkonor")
        OreClassList.Add("I6", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre, Arkonor, Mercoxit")
        OreClassList.Add("I7", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre, Arkonor, Mercoxit, Kernite")
        OreClassList.Add("J", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet")
        OreClassList.Add("J1", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre")
        OreClassList.Add("J2", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite")
        OreClassList.Add("J3", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot")
        OreClassList.Add("J4", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite")
        OreClassList.Add("J5", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite, Hedbergite")
        OreClassList.Add("J6", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite, Hedbergite, Mercoxit")
        OreClassList.Add("J7", "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite, Hedbergite, Mercoxit, Arkonor")
        OreClassList.Add("K", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber")
        OreClassList.Add("K1", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre")
        OreClassList.Add("K2", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain")
        OreClassList.Add("K3", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite")
        OreClassList.Add("K4", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot")
        OreClassList.Add("K5", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot, Arkonor")
        OreClassList.Add("K6", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot, Arkonor, Mercoxit")
        OreClassList.Add("K7", "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot, Arkonor, Mercoxit, Gneiss")
        Return True
    End Function

    Public Function GetIce(ByVal regid As Integer, ByVal syssec As Double) As String
        Dim Icelist As String = ""
        If syssec <= 0.35 And syssec > 0.15 Then Icelist = "Glare Crust"
        If syssec <= 0.15 And syssec >= -0.15 Then Icelist = "Glare Crust, Dark Glitter"
        If syssec <= -0.15 And syssec >= -0.449 Then Icelist = "Dark Glitter, Gelidus"
        If syssec < -0.449 And syssec > -0.5 Then Icelist = "Gelidus"
        If syssec <= -0.5 And syssec > -0.65 Then Icelist = "Gelidus, Krystallos"
        If syssec <= -0.65 And syssec > -1 Then Icelist = "Krystallos"

        Select Case regid
            Case 10000001
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000002
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + "White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000003
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

                'Case 10000004
                '   If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                '  If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                ' If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000005
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000006
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000007
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000008
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000009
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000010
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000011
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000012
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000013
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000014
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000015
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000016
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

                'Case 10000017
                '   If syssec <= 1 And syssec > 0.35 Then Icelist = ""
                '   If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ""
                '  If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ""

            Case 10000018
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

                'Case 10000019
                '   If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                '   If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                '   If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000020
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000021
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000022
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000023
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

                'Case 10000024
                '    If syssec <= 1 And syssec > 0.35 Then Icelist = ""
                '    If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ""
                '   If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ""

            Case 10000025
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

                'Case 10000026
                '  If syssec <= 1 And syssec > 0.35 Then Icelist = ""
                '  If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ""
                '  If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ""

            Case 10000027
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000028
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000029
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000030
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000031
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000032
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000033
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000034
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000035
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000036
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000037
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000038
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000039
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000040
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000041
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000042
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000043
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000044
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000045
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000046
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000047
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000048
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000049
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000050
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000051
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000052
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000053
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000054
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000055
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

            Case 10000056
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000057
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000058
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000059
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000060
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000061
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000062
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Glacial Mass"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Glacial Mass"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Smooth Glacial Mass"

            Case 10000063
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000064
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000065
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000066
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000067
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Clear Icicle"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Clear Icicle"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Enriched Clear Icicle"

            Case 10000068
                If syssec <= 1 And syssec > 0.35 Then Icelist = "Blue Ice"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", Blue Ice"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Thick Blue Ice"

            Case 10000069
                If syssec <= 1 And syssec > 0.35 Then Icelist = "White Glaze"
                If syssec <= 0.35 And syssec >= 0.05 Then Icelist = Icelist + ", White Glaze"
                If syssec <= 0.05 And syssec >= -1 Then Icelist = Icelist + ", Pristine White Glaze"

        End Select
        Return Icelist
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
                        cSystem = CType(PlugInData.SystemsID(fromSystem.ToString), SolarSystem)
                        'cSystem.Gates.Add(PlugInData.SystemsID(toSystem.ToString))
                        cSystem.Gates.Add(toSystem.ToString)
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
            For Each solar1 In PlugInData.SystemsID.Values
                If Not solar1.Flag Then
                    Dim solar2 As SolarSystem
                    For Each solar2 In PlugInData.SystemsID.Values
                        solar2.Flag = False
                    Next
                    frmMap.SetFlags(solar1)
                End If
                Dim sList As New SortedList
                Dim solar3 As SolarSystem
                For Each solar3 In PlugInData.SystemsID.Values
                    Dim cDist As Double = frmMap.Distance(solar1, solar3)
                    If (((cDist <= 14.625) AndAlso (Not solar1 Is solar3)) AndAlso ((solar3.EveSec <= 0.4) AndAlso solar3.Flag)) Then
                        'If (((cDist <= 14.625) AndAlso (Not solar1 Is solar3)) AndAlso solar3.Flag) Then
                        sList.Add(cDist, solar3)
                    End If
                Next
                If ((sList.Count > 0) AndAlso (Not solar1 Is solar0)) Then
                    solar0 = solar1
                End If
                Dim solar4 As SolarSystem
                For Each solar4 In sList.Values
                    'solar1.Jumps.Add(solar4)
                    solar1.Jumps.Add(solar4.ID)
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
                        eveNames.Add(NameData.Tables(0).Rows(a).Item("itemID"), NameData.Tables(0).Rows(a).Item("itemName"))
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
                        cCorp.CorporationID = CInt(NPCCData.Tables(0).Rows(a).Item("corporationID"))
                        cCorp.factionID = CInt(NPCCData.Tables(0).Rows(a).Item("factionID"))
                        cCorp.CorpName = CStr(eveNames(cCorp.CorporationID))
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
    Public Shared Function LoadSov() As Boolean
        Try
            ' Dimension variables
            Dim SystemDetails, AllianceDetails As XmlNodeList
            Dim SysNode As XmlNode
            Dim id As String = ""
            Dim nFaction As New Faction
            Dim nAlliance As New Alliance
            ' Get the Sovereignty data
            Dim XMLDoc As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Sovereignty)
            SystemDetails = XMLDoc.SelectNodes("/eveapi/result/rowset/row")
            For Each SysNode In SystemDetails
                id = CStr(CLng(SysNode.Attributes.GetNamedItem("solarSystemID").Value) - 30000000)
                Dim solar As SolarSystem = CType(PlugInData.SystemsID(id), SolarSystem)
                If SysNode.Attributes.GetNamedItem("factionID").Value <> "0" Then
                    ' This is a faction
                    solar.SovereigntyID = CInt(SysNode.Attributes.GetNamedItem("factionID").Value)
                    nFaction = CType(FactionList(solar.SovereigntyID.ToString), Faction)
                    solar.SovereigntyName = nFaction.factionName
                Else
                    If SysNode.Attributes.GetNamedItem("allianceID").Value <> "0" Then
                        ' This is an alliance
                        solar.SovereigntyID = CInt(SysNode.Attributes.GetNamedItem("allianceID").Value)
                        nAlliance = CType(AllianceList(solar.SovereigntyID.ToString), Alliance)
                        If nAlliance IsNot Nothing Then
                            solar.SovereigntyName = nAlliance.name
                            solar.sovereigntyLevel = CInt(SysNode.Attributes.GetNamedItem("sovereigntyLevel").Value)
                            solar.constellationSovereignty = CInt(SysNode.Attributes.GetNamedItem("constellationSovereignty").Value)
                        Else
                            ' Try to get the name from the IDToName API
                            Try
                                Dim NameXML As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.IDToName, solar.SovereigntyID.ToString)
                                If NameXML IsNot Nothing Then
                                    AllianceDetails = NameXML.SelectNodes("/eveapi/result/rowset/row")
                                    solar.SovereigntyName = AllianceDetails(0).Attributes.GetNamedItem("name").Value
                                Else
                                    solar.SovereigntyName = "<Alliance " & solar.SovereigntyID & ">"
                                End If
                            Catch e As Exception
                                solar.SovereigntyName = "<Alliance " & solar.SovereigntyID & ">"
                            End Try
                            solar.sovereigntyLevel = CInt(SysNode.Attributes.GetNamedItem("sovereigntyLevel").Value)
                            solar.constellationSovereignty = CInt(SysNode.Attributes.GetNamedItem("constellationSovereignty").Value)
                        End If
                    Else
                        solar.SovereigntyID = 0
                        solar.SovereigntyName = ""
                        solar.sovereigntyLevel = 0
                        solar.constellationSovereignty = 0
                    End If
                End If
            Next
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    Public Function LoadFactions() As Boolean
        Dim strSQL As String = "SELECT * FROM chrFactions ORDER BY factionID;"
        Dim FactionData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If FactionData IsNot Nothing Then
                If FactionData.Tables(0).Rows.Count > 0 Then
                    FactionList.Clear()
                    Dim cFaction As Faction = New Faction
                    For solar As Integer = 0 To FactionData.Tables(0).Rows.Count - 1
                        If CStr(FactionData.Tables(0).Rows(solar).Item("factionName")) <> "Unknown" Then
                            cFaction = New Faction
                            cFaction.factionID = CStr(FactionData.Tables(0).Rows(solar).Item("factionID"))
                            cFaction.factionName = CStr(FactionData.Tables(0).Rows(solar).Item("factionName"))
                        Else
                            cFaction = New Faction
                            cFaction.factionID = CStr(FactionData.Tables(0).Rows(solar).Item("factionID"))
                            cFaction.factionName = CStr(FactionData.Tables(0).Rows(solar).Item("factionName"))
                        End If
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
    Public Shared Function LoadAlliances() As Boolean
        Try
            ' Dimension variables
            Dim AllyDetails As XmlNodeList
            Dim AllyNode As XmlNode
            AllianceList.Clear()
            ' Get the alliance data
            Dim XMLDoc As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AllianceList)
            AllyDetails = XMLDoc.SelectNodes("/eveapi/result/rowset/row")
            Dim ind As Integer = 0
            AllianceList.Clear()
            For Each AllyNode In AllyDetails
                Dim Ally As New Alliance
                Ally.ID = AllyNode.Attributes.GetNamedItem("allianceID").Value
                Ally.name = AllyNode.Attributes.GetNamedItem("name").Value
                Ally.shortName = AllyNode.Attributes.GetNamedItem("shortName").Value
                Ally.executorCorpID = AllyNode.Attributes.GetNamedItem("executorCorpID").Value
                Ally.memberCount = AllyNode.Attributes.GetNamedItem("memberCount").Value
                Ally.startDate = AllyNode.Attributes.GetNamedItem("startDate").Value
                Ally.Flag = False
                AllianceList.Add(Ally.ID, Ally)
                ind = ind + 1
            Next
            ind = 0
            Return True
        Catch e As Exception
            Return False
            Exit Function
        End Try
    End Function
    Public Function LoadStations() As Boolean
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
                            cstation.security = CInt(StationData.Tables(0).Rows(stat).Item("security"))
                        Else
                            cstation.security = 0
                        End If
                        'cstation.dockingCostPerVolume = Nullcheck(StationData.Tables(0).Rows(stat).Item("dockingCostPerVolume")
                        'cstation.maxShipVolumeDockable = Nullcheck(StationData.Tables(0).Rows(stat).Item("maxShipVolumeDockable")
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("operationID")) = False Then
                            cstation.operationID = CInt(StationData.Tables(0).Rows(stat).Item("operationID"))
                        Else
                            cstation.operationID = 0
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("stationTypeID")) = False Then
                            cstation.stationTypeID = CInt(StationData.Tables(0).Rows(stat).Item("stationTypeID"))
                        Else
                            cstation.stationTypeID = 0
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("corporationID")) = False Then
                            cstation.corporationID = CLng(StationData.Tables(0).Rows(stat).Item("corporationID"))
                        Else
                            cstation.corporationID = 0
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("solarSystemID")) = False Then
                            cstation.solarSystemID = CInt(StationData.Tables(0).Rows(stat).Item("solarSystemID")) - 30000000
                        Else
                            cstation.solarSystemID = 0
                            skip = True
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("constellationID")) = False Then
                            cstation.constellationID = CLng(StationData.Tables(0).Rows(stat).Item("constellationID"))
                        Else
                            cstation.constellationID = 0
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("regionID")) = False Then
                            cstation.regionID = CLng(StationData.Tables(0).Rows(stat).Item("regionID"))
                        Else
                            cstation.regionID = 0
                        End If
                        If IsDBNull(StationData.Tables(0).Rows(stat).Item("stationName")) = False Then
                            cstation.stationName = CStr(StationData.Tables(0).Rows(stat).Item("stationName"))
                        Else
                            cstation.stationName = " "
                        End If
                        If skip = False Then
                            StationList.Add(cstation.stationID, cstation)
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
                    For Rstat As Integer = 0 To RegionData.Tables(0).Rows.Count - 1
                        Dim cRegion As Region = New Region
                        cRegion.RegionID = CInt(RegionData.Tables(0).Rows(Rstat).Item("regionID"))

                        If IsDBNull(RegionData.Tables(0).Rows(Rstat).Item("regionName")) = False Then
                            cRegion.regionName = CStr(RegionData.Tables(0).Rows(Rstat).Item("regionName"))
                        Else
                            cRegion.regionName = ""
                        End If
                        RegionID.Add(CStr(cRegion.RegionID), cRegion)
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
                    For Rstat As Integer = 0 To ConstData.Tables(0).Rows.Count - 1
                        Dim cConst As Constellation = New Constellation
                        cConst.regionID = CInt(ConstData.Tables(0).Rows(Rstat).Item("regionID"))
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("constellationID")) = False Then
                            cConst.constellationID = CInt(ConstData.Tables(0).Rows(Rstat).Item("constellationID"))
                        Else
                            cConst.constellationID = 0
                        End If
                        If IsDBNull(ConstData.Tables(0).Rows(Rstat).Item("constellationName")) = False Then
                            cConst.constellationName = CStr(ConstData.Tables(0).Rows(Rstat).Item("constellationName"))
                        Else
                            cConst.constellationName = " "
                        End If
                        ConstellationID.Add(CStr(cConst.constellationID), cConst)
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
                        cOp.operationID = CInt(OpData.Tables(0).Rows(a).Item("operationID"))
                        cOp.serviceID = CInt(OpData.Tables(0).Rows(a).Item("serviceID"))
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
                        CServ.serviceID = CInt(ServData.Tables(0).Rows(a).Item("serviceID"))
                        CServ.serviceName = CStr(ServData.Tables(0).Rows(a).Item("serviceName"))
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
                    For a As Integer = 0 To AgentData.Tables(0).Rows.Count - 1
                        Dim cAgent As Agent = New Agent
                        Dim skip As Boolean = False
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("agentID")) = False Then
                            cAgent.agentID = CInt(AgentData.Tables(0).Rows(a).Item("agentID"))
                            cAgent.agentName = CStr(eveNames(cAgent.agentID))
                        Else
                            cAgent.agentID = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("divisionID")) = False Then
                            cAgent.divisionID = CInt(AgentData.Tables(0).Rows(a).Item("divisionID"))
                        Else
                            cAgent.divisionID = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("corporationID")) = False Then
                            cAgent.corporationID = CInt(AgentData.Tables(0).Rows(a).Item("corporationID"))
                        Else
                            cAgent.corporationID = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("stationID")) = False Then
                            cAgent.stationId = CInt(AgentData.Tables(0).Rows(a).Item("stationID"))
                        Else
                            skip = True
                            cAgent.stationId = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("level")) = False Then
                            cAgent.Level = CInt(AgentData.Tables(0).Rows(a).Item("level"))
                        Else
                            cAgent.Level = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("quality")) = False Then
                            cAgent.Quality = CInt(AgentData.Tables(0).Rows(a).Item("quality"))
                        Else
                            cAgent.Quality = 0
                        End If
                        If IsDBNull(AgentData.Tables(0).Rows(a).Item("agentTypeID")) = False Then
                            cAgent.Type = CStr(AgentData.Tables(0).Rows(a).Item("agentTypeID"))
                        Else
                            cAgent.Type = " "
                        End If
                        cAgent.Flag = False
                        If skip = False Then
                            AgentID.Add(cAgent.agentID, cAgent)
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
    Public Shared Function LoadConq() As Boolean
        Try
            ' Dimension variables
            Dim CSDetails As XmlNodeList
            Dim CSNode As XmlNode
            ' Get the Sovereignty data
            Dim XMLDoc As XmlDocument = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.Conquerables)
            CSDetails = XMLDoc.SelectNodes("/eveapi/result/rowset/row")
            CSStationList.Clear()
            For Each CSNode In CSDetails
                Dim CS As New ConqStat
                CS.stationID = CInt(CSNode.Attributes.GetNamedItem("stationID").Value)
                CS.stationName = CSNode.Attributes.GetNamedItem("stationName").Value
                CS.stationTypeID = CSNode.Attributes.GetNamedItem("stationTypeID").Value
                CS.solarSystemID = CInt(CSNode.Attributes.GetNamedItem("solarSystemID").Value) - 30000000
                CS.corporationID = CSNode.Attributes.GetNamedItem("corporationID").Value
                CS.corporationName = CSNode.Attributes.GetNamedItem("corporationName").Value
                CS.Flag = False
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
                            cCSS.stationTypeID = CInt(sttData.Tables(0).Rows(a).Item("stationTypeID"))
                        Else
                            cCSS.stationTypeID = 0
                        End If
                        If IsDBNull(sttData.Tables(0).Rows(a).Item("operationID")) = False Then
                            cCSS.OperationID = CInt(sttData.Tables(0).Rows(a).Item("operationID"))
                        Else
                            cCSS.OperationID = 0
                        End If
                        If IsDBNull(sttData.Tables(0).Rows(a).Item("officeSlots")) = False Then
                            cCSS.OfficeSlots = CInt(sttData.Tables(0).Rows(a).Item("officeSlots"))
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
                        cDiv.divisionID = CInt(DivData.Tables(0).Rows(a).Item("divisionID"))
                        cDiv.divisionName = CStr(DivData.Tables(0).Rows(a).Item("divisionName"))
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
                            If lastSystemNo <> CInt(mapRow.Item("solarSystemID")) - 30000000 Then
                                lastSystemNo = CInt(mapRow.Item("solarSystemID")) - 30000000
                                lastSystem = CType(PlugInData.SystemsID(lastSystemNo.ToString), SolarSystem)
                            End If
                            Select Case CInt(mapRow.Item("groupID"))
                                Case 6
                                    ' Star - do nothing
                                Case 7
                                    ' Planet
                                    lastSystem.Planets += 1
                                Case 8
                                    ' Moon
                                    lastSystem.Moons += 1
                                Case 9
                                    ' Belt
                                    If CStr(mapRow.Item("itemName")).Contains("Ice Field") = True Then
                                        lastSystem.IBelts += 1
                                    Else
                                        lastSystem.ABelts += 1
                                    End If
                                Case 10
                                    ' Stargate - Do nothing
                                Case 15
                                    ' Station
                                    lastSystem.Stations.Add(CStr(mapRow.Item("itemID")))
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

    Private Sub ConvertJumpsToSystems()
        For Each ss As SolarSystem In PlugInData.SystemsID.Values
            JumpList = CType(ss.Jumps.Clone, ArrayList)
            SystemJumps.Add(ss.ID, JumpList)
            ss.Jumps.Clear()
            For Each jump As String In JumpList
                ss.Jumps.Add(PlugInData.SystemsID(jump))
            Next
        Next
        'MessageBox.Show("Jump Conversion Complete!")
    End Sub

#End Region

#Region "Serialization Routines"
    Public Shared Sub SaveSerializedData()
        ' Delete the cache folder if it's already there
        If My.Computer.FileSystem.DirectoryExists(mapCacheFolder) = True Then
            My.Computer.FileSystem.DeleteDirectory(mapCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        My.Computer.FileSystem.CreateDirectory(mapCacheFolder)
        Dim s As FileStream
        Dim f As BinaryFormatter

        ' Save SolarSystems
        Dim sysLists(20) As SortedList
        For sysList As Integer = 0 To 20
            sysLists(sysList) = New SortedList
        Next
        For Each sSystem As SolarSystem In PlugInData.SystemsID.Values
            sysLists(CInt(Int(sSystem.Security * 10) + 10)).Add(sSystem.Name, sSystem)
        Next
        For sysList As Integer = 0 To 20
            s = New FileStream(mapCacheFolder & "\SolarSystems" & sysList & ".bin", FileMode.Create)
            f = New BinaryFormatter
            Dim ss As SortedList = sysLists(sysList)
            f.Serialize(s, ss)
            s.Close()
        Next

        ' Save Constellations
        s = New FileStream(mapCacheFolder & "\Constellations.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, ConstellationID)
        s.Close()

        ' Save Regions
        s = New FileStream(mapCacheFolder & "\Regions.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, RegionID)
        s.Close()

        s = New FileStream(mapCacheFolder & "\OreClass.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, OreClassList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\NPCCorps.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, NPCCorpList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Factions.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, FactionList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Alliances.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, AllianceList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Stations.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, StationList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Operations.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, OperationList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Services.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, ServiceList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Agents.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, AgentID)
        s.Close()

        s = New FileStream(mapCacheFolder & "\Conquerables.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, CSStationList)
        s.Close()

        s = New FileStream(mapCacheFolder & "\StationTypes.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, StationTypes)
        s.Close()

        s = New FileStream(mapCacheFolder & "\NPCDivs.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, NPCDivID)
        s.Close()

    End Sub
#End Region

End Class
