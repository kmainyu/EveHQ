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
Imports System.IO
Imports EveHQ.EveData
Imports EveHQ.EveAPI
Imports Newtonsoft.Json

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn
    Public Shared itemFlags As New SortedList
    Public Shared RefTypes As New SortedList(Of String, String)
    Public Shared Activities As New SortedList(Of String, String)
    Public Shared Statuses As New SortedList(Of String, String)
    Public Shared stations As New SortedList
    Public Shared NPCCorps As New SortedList
    Public Shared Corps As New SortedList
    Public Shared PackedVolumes As New SortedList(Of Integer, Double) ' groupID, volume
    Public Shared AssetItemNames As New SortedList(Of Long, String)
    Public Shared Products As New SortedList(Of String, String)
    Public Shared BlueprintAssets As New SortedList(Of String, SortedList(Of Long, BlueprintAsset))
    Public Shared CorpList As New SortedList
    Public Shared CategoryNames As New SortedList(Of String, Integer) ' catName, catID
    Public Shared Decryptors As New SortedList(Of String, Decryptor)
    Public Shared PrismOwners As New SortedList(Of String, PrismOwner)
    Private activeForm As frmPrism
    Private Const OwnerBlueprintsFileName As String = "OwnerBlueprints.json"
    Private Shared ReadOnly LockObj As New Object

#Region "Plug-in Interface Properties and Functions"

    Public Function GetPlugInData(ByVal data As Object, ByVal dataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Select Case dataType
            Case 0 ' Return a location
                ' Check the data is Long return the station name
                If TypeOf (data) Is Long Then
                    Return CType(stations(data.ToString), Station).stationName
                Else
                    Return data
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

    Public Function GetEveHQPlugInInfo() As Core.EveHQPlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.EveHQPlugIn
        EveHQPlugIn.Name = "EveHQ Prism"
        EveHQPlugIn.Description = "EveHQ Production, Research, Industry and Science Module"
        EveHQPlugIn.Author = "EveHQ Team"
        EveHQPlugIn.MainMenuText = "EveHQ Prism"
        EveHQPlugIn.RunAtStartup = True
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.plugin_icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function

    Public Function IGBService(ByVal igbContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function

    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        activeForm = New frmPrism()
        Return activeForm
    End Function

    Public Function SaveAll() As Boolean Implements Core.IEveHQPlugIn.SaveAll
        If activeForm IsNot Nothing Then
            activeForm.SaveAll()
            Return True
        End If
        Return False
    End Function

#End Region

#Region "Plug-in Startup Routines"
    Private Function LoadPlugIndata() As Boolean
        ' Setup the Prism Folder
        If Core.HQ.IsUsingLocalFolders = False Then
            PrismSettings.PrismFolder = Path.Combine(Core.HQ.AppDataFolder, "Prism")
        Else
            PrismSettings.PrismFolder = Path.Combine(Application.StartupPath, "Prism")
        End If
        If My.Computer.FileSystem.DirectoryExists(PrismSettings.PrismFolder) = False Then
            My.Computer.FileSystem.CreateDirectory(PrismSettings.PrismFolder)
        End If
        Call PrismDataFunctions.CheckDatabaseTables()
        Call LoadStatuses()
        Call LoadPackedVolumes()
        Call LoadAssetItemNames()
        Call LoadCategoryData()
        If Invention.LoadInventionData = False Then
            Return False
        End If
        If LoadRefTypes() = False Then
            Return False
        End If
        Call CheckForConqXMLFile()
        Call LoadOwnerBlueprints()
        Return True
    End Function
    Private Sub LoadCategoryData()

        CategoryNames.Clear()
        Products.Clear()
        For Each bp As EveData.Blueprint In StaticData.Blueprints.Values
            If StaticData.Types.ContainsKey(bp.ProductId) Then
                Dim catID As Integer = StaticData.Types(bp.ProductId).Category
                If CategoryNames.ContainsKey(StaticData.TypeCats(catID)) = False Then
                    CategoryNames.Add(StaticData.TypeCats(catID), catID)
                End If
            End If
            If Products.ContainsKey(bp.ProductId.ToString) = False Then
                Products.Add(bp.ProductId.ToString, bp.Id.ToString)
            End If
        Next

    End Sub
    Private Sub LoadAssetItemNames()
        Try
            Dim strSQL As String = "SELECT * FROM assetItemNames;"
            Dim nameData As DataSet = Core.CustomDataFunctions.GetCustomData(strSQL)
            AssetItemNames.Clear()
            If nameData IsNot Nothing Then
                If nameData.Tables(0).Rows.Count > 0 Then
                    For Each nameRow As DataRow In nameData.Tables(0).Rows
                        AssetItemNames.Add(CInt(nameRow.Item("itemID")), CStr(nameRow.Item("itemName")))
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
        PackedVolumes.Clear()
        PackedVolumes.Add(31, 500)
        PackedVolumes.Add(324, 2500)
        PackedVolumes.Add(419, 15000)
        PackedVolumes.Add(27, 50000)
        PackedVolumes.Add(898, 50000)
        PackedVolumes.Add(547, 1000000)
        PackedVolumes.Add(659, 1000000)
        PackedVolumes.Add(540, 15000)
        PackedVolumes.Add(830, 2500)
        PackedVolumes.Add(834, 2500)
        PackedVolumes.Add(26, 10000)
        PackedVolumes.Add(420, 5000)
        PackedVolumes.Add(485, 1000000)
        PackedVolumes.Add(893, 2500)
        PackedVolumes.Add(543, 3750)
        PackedVolumes.Add(513, 1000000)
        PackedVolumes.Add(25, 2500)
        PackedVolumes.Add(358, 10000)
        PackedVolumes.Add(894, 10000)
        PackedVolumes.Add(28, 20000)
        PackedVolumes.Add(831, 2500)
        PackedVolumes.Add(541, 5000)
        PackedVolumes.Add(902, 1000000)
        PackedVolumes.Add(832, 10000)
        PackedVolumes.Add(900, 50000)
        PackedVolumes.Add(463, 3750)
        PackedVolumes.Add(906, 10000)
        PackedVolumes.Add(833, 10000)
        PackedVolumes.Add(30, 10000000)
        PackedVolumes.Add(380, 20000)
        PackedVolumes.Add(941, 500000)
        PackedVolumes.Add(883, 1000000)
        PackedVolumes.Add(237, 2500)
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
    Public Function LoadRefTypes() As Boolean
        Try
            ' Dimension variables
            Dim x As Integer = 0
            Dim refDetails As XmlNodeList
            Dim refNode As XmlNode
            Dim fileName As String = ""
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.Settings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
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
    Public Sub CheckForConqXMLFile()
        ' Check for the Conquerable XML file in the cache
        Dim stationXML As XmlDocument
        Dim apiReq As New EveAPIRequest(Core.HQ.EveHQAPIServerInfo, Core.HQ.RemoteProxy, Core.HQ.Settings.APIFileExtension, Core.HQ.cacheFolder)
        stationXML = apiReq.GetAPIXML(APITypes.Conquerables, APIReturnMethods.ReturnStandard)
        If stationXML IsNot Nothing Then
            Call ParseConquerableXML(stationXML)
        End If
    End Sub
    Public Shared Sub ParseConquerableXML(ByVal stationXML As XmlDocument)
        Dim locList As XmlNodeList
        Dim loc As XmlNode
        Dim stationID As Integer
        locList = stationXML.SelectNodes("/eveapi/result/rowset/row")
        If locList.Count > 0 Then
            Corps.Clear()
            For Each loc In locList
                stationID = CInt((loc.Attributes.GetNamedItem("stationID").Value))
                ' This is an outpost so needs adding to the station list if it's not there
                If StaticData.Stations.ContainsKey(stationID) = False Then
                    Dim cStation As New Station
                    cStation.StationID = CInt(stationID)
                    cStation.stationName = (loc.Attributes.GetNamedItem("stationName").Value)
                    cStation.SystemID = CInt(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As SolarSystem = StaticData.SolarSystems(cStation.SystemID)
                    cStation.StationName &= " (" & system.Name & ", " & StaticData.Regions(system.RegionID) & ")"
                    cStation.corpID = CInt(loc.Attributes.GetNamedItem("corporationID").Value)
                    StaticData.Stations.Add(cStation.StationID, cStation)
                Else
                    Dim cStation As Station = StaticData.Stations(stationID)
                    cStation.SystemID = CInt(loc.Attributes.GetNamedItem("solarSystemID").Value)
                    Dim system As SolarSystem = StaticData.SolarSystems(cStation.SystemID)
                    cStation.StationName &= " (" & system.Name & ", " & StaticData.Regions(system.RegionID) & ")"
                    cStation.corpID = CInt(loc.Attributes.GetNamedItem("corporationID").Value)
                End If
                ' Add the corp if not already entered
                If Corps.ContainsKey(CStr(loc.Attributes.GetNamedItem("corporationID").Value)) = False Then
                    Corps.Add(CStr(loc.Attributes.GetNamedItem("corporationID").Value), CStr(loc.Attributes.GetNamedItem("corporationName").Value))
                End If
            Next
        End If
    End Sub
  
#End Region

#Region "Owner Blueprint Load/Save Methods"

    Public Shared Sub LoadOwnerBlueprints()

        SyncLock frmPrism.LockObj

            If My.Computer.FileSystem.FileExists(Path.Combine(PrismSettings.PrismFolder, "OwnerBlueprints.json")) = True Then
                Try
                    Using s As New StreamReader(Path.Combine(PrismSettings.PrismFolder, "OwnerBlueprints.json"))
                        Dim json As String = s.ReadToEnd
                        BlueprintAssets = JsonConvert.DeserializeObject(Of SortedList(Of String, SortedList(Of Long, BlueprintAsset)))(json)
                    End Using
                Catch ex As Exception
                    Dim msg As String = "There was an error trying to load the Owner Blueprints and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Prism will rename this file (and add a .bad suffix) and re-initialise the settings." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Press OK to reset the Owner Blueprints file." & ControlChars.CrLf
                    MessageBox.Show(msg, "Invalid Owner Blueprints file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Try
                        File.Move(Path.Combine(PrismSettings.PrismFolder, "OwnerBlueprints.json"), Path.Combine(PrismSettings.PrismFolder, "OwnerBlueprints.json" & ".bad"))
                    Catch e As Exception
                        MessageBox.Show("Unable to delete the OwnerBlueprints.json file. Please delete this manually before proceeding", "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Try
                End Try
            End If

        End SyncLock

    End Sub

    Public Shared Sub SaveOwnerBlueprints()

        SyncLock LockObj
            Dim newFile As String = Path.Combine(PrismSettings.PrismFolder, OwnerBlueprintsFileName)
            Dim tempFile As String = Path.Combine(PrismSettings.PrismFolder, OwnerBlueprintsFileName & ".temp")

            ' Create a JSON string for writing
            Dim json As String = JsonConvert.SerializeObject(BlueprintAssets, Newtonsoft.Json.Formatting.Indented)

            ' Write the JSON version of the settings
            Try
                Using s As New StreamWriter(tempFile, False)
                    s.Write(json)
                    s.Flush()
                End Using

                If File.Exists(newFile) Then
                    File.Delete(newFile)
                End If

                File.Move(tempFile, newFile)

            Catch e As Exception

            End Try

        End SyncLock

    End Sub

#End Region

#Region "API Helper Methods"

    Public Shared Function GetAccountForCorpOwner(Owner As PrismOwner, APIType As CorpRepType) As EveHQ.Core.EveHQAccount
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

