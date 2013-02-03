' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
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

Imports System.Windows.Forms
Imports System.Xml
Imports System.Reflection
Imports System.Text
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn
    Public Shared itemFlags As New SortedList
    Public Shared RefTypes As New SortedList(Of String, String)
    Public Shared Activities As New SortedList(Of String, String)
    Public Shared Statuses As New SortedList(Of String, String)
    Public Shared stations As New SortedList
    Public Shared NPCCorps As New SortedList
    Public Shared Corps As New SortedList
    Public Shared PackedVolumes As New SortedList(Of String, Double)
    Public Shared AssetItemNames As New SortedList(Of String, String)
    Public Shared Blueprints As New SortedList(Of String, Blueprint)
    Public Shared Products As New SortedList(Of String, String)
    Public Shared AssemblyArrays As New SortedList(Of String, AssemblyArray)
    Public Shared BlueprintAssets As New SortedList(Of String, SortedList(Of String, BlueprintAsset))
    Public Shared CorpList As New SortedList
    Public Shared CategoryNames As New SortedList(Of String, String)
    Public Shared Decryptors As New SortedList(Of String, Decryptor)
    Public Shared PrismOwners As New SortedList(Of String, PrismOwner) ' Key = OwnerName i.e. Vessper, Indicium Technologies etc...

#Region "Plug-in Interface Properties and Functions"

    Public Function GetPlugInData(ByVal Data As Object, ByVal DataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Select Case DataType
            Case 0 ' Return a location
                ' Check the data is Long return the station name
                If TypeOf (Data) Is Long Then
                    Return CType(stations(Data.ToString), Station).stationName
                Else
                    Return Data
                End If
        End Select
        Return Nothing
    End Function

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
        EveHQPlugIn.Name = "EveHQ Prism"
        EveHQPlugIn.Description = "EveHQ Production, Research, Industry and Science Module"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "EveHQ Prism"
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
        Return New frmPrism
    End Function
#End Region

#Region "Plug-in Startup Routines"
    Private Function LoadPlugIndata() As Boolean
        If Me.CheckVersion = False Then
            Return False
        Else
            ' Setup the Prism Folder
            If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
                Settings.PrismFolder = Path.Combine(EveHQ.Core.HQ.AppDataFolder, "Prism")
            Else
                Settings.PrismFolder = Path.Combine(Application.StartupPath, "Prism")
            End If
            If My.Computer.FileSystem.DirectoryExists(Settings.PrismFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(Settings.PrismFolder)
            End If
            Call Prism.DataFunctions.CheckDatabaseTables()
            Call Me.LoadItemFlags()
            Call Me.LoadActivities()
            Call Me.LoadStatuses()
            Call Me.LoadPackedVolumes()
            Call Me.LoadAssetItemNames()
            If Blueprint.LoadBluePrintData = False Then
                Return False
                Exit Function
            End If
            If Invention.LoadInventionData = False Then
                Return False
                Exit Function
            End If
            If AssemblyArray.LoadAssemblyArrayData = False Then
                Return False
                Exit Function
            End If
            If Me.LoadRefTypes = False Then
                Return False
                Exit Function
            End If
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
            Call Me.LoadOwnerBlueprints()
            Return True
        End If
    End Function
    Private Sub LoadAssetItemNames()
        Try
            Dim strSQL As String = "SELECT * FROM assetItemNames;"
            Dim nameData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
            AssetItemNames.Clear()
            If nameData IsNot Nothing Then
                If nameData.Tables(0).Rows.Count > 0 Then
                    For Each nameRow As DataRow In nameData.Tables(0).Rows
                        AssetItemNames.Add(CStr(nameRow.Item("itemID")), CStr(nameRow.Item("itemName")))
                    Next
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("There was an error retrieving the Asset Item Names data from the custom database. The error was: " & ex.Message, "Prism Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            AssetItemNames.Clear()
        End Try
    End Sub
    Private Sub LoadPackedVolumes()
        PackedVolumes.Clear()
        PackedVolumes.Add("31", 500)
        PackedVolumes.Add("324", 2500)
        PackedVolumes.Add("419", 15000)
        PackedVolumes.Add("27", 50000)
        PackedVolumes.Add("898", 50000)
        PackedVolumes.Add("547", 1000000)
        PackedVolumes.Add("659", 1000000)
        PackedVolumes.Add("540", 15000)
        PackedVolumes.Add("830", 2500)
        PackedVolumes.Add("834", 2500)
        PackedVolumes.Add("26", 10000)
        PackedVolumes.Add("420", 5000)
        PackedVolumes.Add("485", 1000000)
        PackedVolumes.Add("893", 2500)
        PackedVolumes.Add("543", 3750)
        PackedVolumes.Add("513", 1000000)
        PackedVolumes.Add("25", 2500)
        PackedVolumes.Add("358", 10000)
        PackedVolumes.Add("894", 10000)
        PackedVolumes.Add("28", 20000)
        PackedVolumes.Add("831", 2500)
        PackedVolumes.Add("541", 5000)
        PackedVolumes.Add("902", 1000000)
        PackedVolumes.Add("832", 10000)
        PackedVolumes.Add("900", 50000)
        PackedVolumes.Add("463", 3750)
        PackedVolumes.Add("906", 10000)
        PackedVolumes.Add("833", 10000)
        PackedVolumes.Add("30", 10000000)
        PackedVolumes.Add("380", 20000)
        PackedVolumes.Add("941", 500000)
        PackedVolumes.Add("883", 1000000)
        PackedVolumes.Add("237", 2500)
    End Sub
    Private Sub LoadItemFlags()
        itemFlags.Clear()
        Dim strSQL As String = "SELECT * FROM invFlags"
        Dim flagData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If flagData IsNot Nothing Then
            If flagData.Tables(0).Rows.Count > 0 Then
                For Each flagRow As DataRow In flagData.Tables(0).Rows
                    itemFlags.Add(CInt(flagRow.Item("flagID")), CStr(flagRow.Item("flagText")))
                Next
            End If
        End If
    End Sub
    Private Sub LoadActivities()
        Activities.Clear()
        Activities.Add("1", "Manufacturing")
        Activities.Add("2", "Research Tech")
        Activities.Add("3", "Research PE")
        Activities.Add("4", "Research ME")
        Activities.Add("5", "Copying")
        Activities.Add("6", "Recycling")
        Activities.Add("7", "Reverse Eng.")
        Activities.Add("8", "Invention")
    End Sub
    Private Sub LoadStatuses()
        Statuses.Clear()
        Statuses.Add("0", "Failed")
        Statuses.Add("1", "Delivered")
        Statuses.Add("2", "Aborted")
        Statuses.Add("3", "GM Aborted")
        Statuses.Add("4", "Unanchored")
        Statuses.Add("5", "Destroyed")
        Statuses.Add("A", "In Progress")
        Statuses.Add("B", "Finished but not Delivered")
    End Sub
    Private Function LoadStations() As Boolean
        ' Load the Station Data From the mapDenormalize table
        Try
            Dim strSQL As String = "SELECT * FROM staStations;"
            Dim locationData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If locationData IsNot Nothing Then
                If locationData.Tables(0).Rows.Count > 0 Then
                    stations.Clear()
                    For Each locationRow As DataRow In locationData.Tables(0).Rows
                        Dim newStation As New Station
                        newStation.stationID = CLng(locationRow.Item("stationID"))
                        newStation.stationName = CStr(locationRow.Item("stationName"))
                        newStation.systemID = CLng(locationRow.Item("solarSystemID"))
                        newStation.constID = CLng(locationRow.Item("constellationID"))
                        newStation.regionID = CLng(locationRow.Item("regionID"))
                        newStation.corpID = CLng(locationRow.Item("corporationID"))
                        newStation.stationTypeID = CLng(locationRow.Item("stationTypeID"))
                        newStation.operationID = CLng(locationRow.Item("operationID"))
                        newStation.refiningEff = CDbl(locationRow.Item("reprocessingEfficiency"))
                        stations.Add(newStation.stationID.ToString, newStation)
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Station Data for Prism Plugin" & ControlChars.CrLf & ex.Message, "Prism Plug-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            MessageBox.Show("Error Loading System Data for Prism Plugin" & ControlChars.CrLf & ex.Message, "Prism Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadNPCCorps() As Boolean
        ' Load the Station Data From the mapDenormalize table
        NPCCorps.Clear()
        Try
            Dim strSQL As String = "SELECT itemID, itemName FROM invUniqueNames WHERE groupID=2;"
            Dim corpData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If corpData IsNot Nothing Then
                NPCCorps.Clear()
                If corpData.Tables(0).Rows.Count > 0 Then
                    For Each corpRow As DataRow In corpData.Tables(0).Rows
                        NPCCorps.Add(CStr(corpRow.Item("itemID")), CStr(corpRow.Item("itemname")))
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading NPC Corporation Data for Prism Plugin" & ControlChars.CrLf & ex.Message, "Prism Plug-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Function LoadRefTypes() As Boolean
        Try
            ' Dimension variables
            Dim x As Integer = 0
            Dim refDetails As XmlNodeList
            Dim refNode As XmlNode
            Dim fileName As String = ""
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHqSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            Dim refXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.RefTypes, EveAPI.APIReturnMethods.ReturnStandard)
            If refXML Is Nothing Then
                ' Problem with the API server so let's use our resources to populate it
                Try
                    refXML = New XmlDocument
                    refXML.LoadXml(My.Resources.RefTypes.ToString)
                Catch ex As Exception
                    MessageBox.Show("There was an error loading the RefTypes API and it would appear there is a problem with the local copy. Please try again later.", "Prism RefTypes Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End Try
            End If
            Dim errlist As XmlNodeList = refXML.SelectNodes("/eveapi/error")
            If errlist.Count = 0 Then
                refDetails = refXML.SelectNodes("/eveapi/result/rowset/row")
                If refDetails IsNot Nothing Then
                    RefTypes.Clear()
                    For Each refNode In refDetails
                        RefTypes.Add(refNode.Attributes.GetNamedItem("refTypeID").Value, refNode.Attributes.GetNamedItem("refTypeName").Value)
                    Next
                End If
            Else
                Dim errNode As XmlNode = errlist(0)
                ' Get error code
                Dim errCode As String = errNode.Attributes.GetNamedItem("code").Value
                Dim errMsg As String = errNode.InnerText
                MessageBox.Show("The RefTypes API returned error " & errCode & ": " & errMsg, "RefTypes Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the RefTypes API. The error was: " & e.Message, "Prism RefTypes Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
            Exit Function
        End Try
    End Function
    Public Sub CheckForConqXMLFile()
        ' Check for the Conquerable XML file in the cache
        Dim stationXML As New XmlDocument
        Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHqSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
        stationXML = APIReq.GetAPIXML(EveAPI.APITypes.Conquerables, EveAPI.APIReturnMethods.ReturnStandard)
        If stationXML IsNot Nothing Then
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
            Corps.Clear()
            For Each loc In locList
                stationID = (loc.Attributes.GetNamedItem("stationID").Value)
                ' This is an outpost so needs adding to the station list if it's not there
                If PlugInData.stations.Contains(stationID) = False Then
                    Dim cStation As New Station
                    cStation.stationID = CLng(stationID)
                    cStation.stationName = (loc.Attributes.GetNamedItem("stationName").Value)
                    cStation.systemID = CLng(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As SolarSystem = CType(PlugInData.stations(cStation.systemID.ToString), SolarSystem)
                    cStation.stationName &= " (" & system.Name & ", " & system.Region & ")"
                    cStation.corpID = CLng(loc.Attributes.GetNamedItem("corporationID").Value)
                    PlugInData.stations.Add(cStation.stationID.ToString, cStation)
                Else
                    Dim cStation As Station = CType(PlugInData.stations(stationID), Prism.Station)
                    cStation.systemID = CLng(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As SolarSystem = CType(PlugInData.stations(cStation.systemID.ToString), SolarSystem)
                    cStation.stationName &= " (" & system.Name & ", " & system.Region & ")"
                    cStation.corpID = CLng(loc.Attributes.GetNamedItem("corporationID").Value)
                End If
                ' Add the corp if not already entered
                If Corps.ContainsKey(CStr(loc.Attributes.GetNamedItem("corporationID").Value)) = False Then
                    Corps.Add(CStr(loc.Attributes.GetNamedItem("corporationID").Value), CStr(loc.Attributes.GetNamedItem("corporationName").Value))
                End If
            Next
        End If
    End Sub
    Private Sub LoadOwnerBlueprints()
        If My.Computer.FileSystem.FileExists(Path.Combine(Settings.PrismFolder, "OwnerBlueprints.bin")) = True Then
            Dim s As New FileStream(Path.Combine(Settings.PrismFolder, "OwnerBlueprints.bin"), FileMode.Open)
            If s.Length > 0 Then
                Dim f As BinaryFormatter = New BinaryFormatter
                PlugInData.BlueprintAssets = CType(f.Deserialize(s), SortedList(Of String, SortedList(Of String, BlueprintAsset)))
                s.Close()
            End If
        End If
    End Sub
#End Region

#Region "API Helper Routines"

    Public Shared Function GetAccountForCorpOwner(Owner As PrismOwner, APIType As CorpRepType) As EveHQ.Core.EveAccount
        If Owner.IsCorp = True Then
            Select Case Owner.APIVersion
                Case Core.APIKeySystems.Version2
                    Return Owner.Account
                Case Else
                    Return Nothing
            End Select
        Else
            Return Owner.Account
        End If
    End Function

    Public Shared Function GetAccountOwnerIDForCorpOwner(Owner As PrismOwner, APIType As CorpRepType) As String
        If Owner.IsCorp = True Then
            Select Case Owner.APIVersion
                Case Core.APIKeySystems.Version2
                    Return Owner.ID
                Case Else
                    Return ""
            End Select
        Else
            Return Owner.ID
        End If
    End Function

#End Region
End Class

