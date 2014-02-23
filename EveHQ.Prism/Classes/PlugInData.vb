'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports EveHQ.Prism.BPCalc
Imports EveHQ.Prism.Classes
Imports EveHQ.EveApi
Imports EveHQ.EveData
Imports EveHQ.Core
Imports System.Net
Imports System.Windows.Forms
Imports System.Xml
Imports System.IO
Imports EveHQ.Prism.Forms
Imports EveHQ.Common.Extensions
Imports Newtonsoft.Json

' ReSharper disable once CheckNamespace - for binary serialization compatability
Public Class PlugInData
    Implements IEveHQPlugIn
    Public Shared RefTypes As New SortedList(Of String, String)
    Public Shared Activities As New SortedList(Of String, String)
    Public Shared Statuses As New SortedList(Of String, String)
    Public Shared Corps As New SortedList(Of String, String) ' corpID, corpName
    Public Shared PackedVolumes As New SortedList(Of Integer, Double) ' groupID, volume
    Public Shared AssetItemNames As New SortedList(Of Long, String)
    Public Shared Products As New SortedList(Of String, String)
    Public Shared BlueprintAssets As New SortedList(Of String, SortedList(Of Long, BlueprintAsset))
    Public Shared CorpList As New SortedList(Of String, Integer) ' corpName, corpID
    Public Shared CategoryNames As New SortedList(Of String, Integer) ' catName, catID
    Public Shared Decryptors As New SortedList(Of String, BPCalc.Decryptor)
    Public Shared PrismOwners As New SortedList(Of String, PrismOwner)
    Private _activeForm As FrmPrism
    Private Const OwnerBlueprintsFileName As String = "OwnerBlueprints.json"
    Private Shared ReadOnly LockObj As New Object

#Region "Plug-in Interface Properties and Functions"

    Public Function GetPlugInData(ByVal data As Object, ByVal dataType As Integer) As Object Implements IEveHQPlugIn.GetPlugInData
        Select Case dataType
            Case 0 ' Return a location
                ' TODO: Check if any code actually uses this as it should no longer be that useful
                ' Check the data is Long return the station name
                If TypeOf (data) Is Integer Then
                    Return StaticData.Stations(CInt(data)).StationName
                Else
                    Return data
                End If
        End Select
        Return Nothing
    End Function

    Public Function EveHQStartUp() As Boolean Implements IEveHQPlugIn.EveHQStartUp
        Try
            Return LoadPlugIndata()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Function GetEveHQPlugInInfo() As EveHQPlugIn Implements IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim eveHQPlugIn As New EveHQPlugIn
        eveHQPlugIn.Name = "EveHQ Prism"
        eveHQPlugIn.Description = "EveHQ Production, Research, Industry and Science Module"
        eveHQPlugIn.Author = "EveHQ Team"
        eveHQPlugIn.MainMenuText = "EveHQ Prism"
        eveHQPlugIn.RunAtStartup = True
        eveHQPlugIn.RunInIGB = False
        eveHQPlugIn.MenuImage = My.Resources.plugin_icon
        eveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return eveHQPlugIn
    End Function

    Public Function IGBService(ByVal igbContext As HttpListenerContext) As String Implements IEveHQPlugIn.IGBService
        Return ""
    End Function

    Public Function RunEveHQPlugIn() As Form Implements IEveHQPlugIn.RunEveHQPlugIn
        _activeForm = New FrmPrism()
        Return _activeForm
    End Function

    Public Function SaveAll() As Boolean Implements IEveHQPlugIn.SaveAll
        If _activeForm IsNot Nothing Then
            _activeForm.SaveAll()
            Return True
        End If
        Return False
    End Function

#End Region

#Region "Plug-in Startup Routines"
    Private Function LoadPlugIndata() As Boolean
        ' Setup the Prism Folder
        If HQ.IsUsingLocalFolders = False Then
            PrismSettings.PrismFolder = Path.Combine(HQ.AppDataFolder, "Prism")
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
            Const StrSQL As String = "SELECT * FROM assetItemNames;"
            Dim nameData As DataSet = CustomDataFunctions.GetCustomData(StrSQL)
            AssetItemNames.Clear()
            If nameData IsNot Nothing Then
                If nameData.Tables(0).Rows.Count > 0 Then
                    For Each nameRow As DataRow In nameData.Tables(0).Rows
                        AssetItemNames.Add(CLng(nameRow.Item("itemID")), CStr(nameRow.Item("itemName")))
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
            Dim refData = HQ.ApiProvider.Eve.RefTypes()

            If refData.IsSuccess Then
                RefTypes.Clear()
                For Each refNode In refData.ResultData
                    RefTypes.Add(refNode.Id.ToInvariantString(), refNode.Name)
                Next
            Else
                ' Get error code
                Dim errCode As String = refData.EveErrorCode.ToInvariantString()
                Dim errMsg As String = refData.EveErrorText
                MessageBox.Show("The RefTypes API returned error " & errCode & ": " & errMsg, "RefTypes Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("There was an error loading the RefTypes API. The error was: " & e.Message, "Prism RefTypes Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Public Sub CheckForConqXMLFile()
        ' Check for the Conquerable XML file in the cache
        Dim stations = HQ.ApiProvider.Eve.ConquerableStationList()
        If stations.IsSuccess Then
            Call ParseConquerableXML(stations.ResultData)
        End If
    End Sub
    Public Shared Sub ParseConquerableXML(ByVal stations As IEnumerable(Of ConquerableStation))
        
        Dim stationID As Integer
        If stations.Any() Then
            Corps.Clear()
            For Each station In stations
                stationID = station.Id
                ' This is an outpost so needs adding to the station list if it's not there
                If StaticData.Stations.ContainsKey(stationID) = False Then
                    Dim cStation As New Station
                    cStation.StationId = stationID
                    cStation.StationName = station.Name
                    cStation.SystemId = station.SolarSystemId
                    Dim system As SolarSystem = StaticData.SolarSystems(cStation.SystemId)
                    cStation.StationName &= " (" & system.Name & ", " & StaticData.Regions(system.RegionId) & ")"
                    cStation.CorpId = station.CorporationId
                    StaticData.Stations.Add(cStation.StationId, cStation)
                Else
                    Dim cStation As Station = StaticData.Stations(stationID)
                    cStation.SystemId = station.SolarSystemId
                    Dim system As SolarSystem = StaticData.SolarSystems(cStation.SystemId)
                    cStation.StationName &= " (" & system.Name & ", " & StaticData.Regions(system.RegionId) & ")"
                    cStation.CorpId = station.CorporationId
                End If
                ' Add the corp if not already entered
                If Corps.ContainsKey(station.CorporationId.ToInvariantString()) = False Then
                    Corps.Add(station.CorporationId.ToInvariantString(), station.CorporationName)
                End If
            Next
        End If
    End Sub

#End Region

#Region "Owner Blueprint Load/Save Methods"

    Public Shared Sub LoadOwnerBlueprints()

        SyncLock FrmPrism.LockObj

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

    Public Shared Function GetAccountForCorpOwner(owner As PrismOwner, apiType As CorpRepType) As EveHQAccount
        If owner.IsCorp = True Then
            Select Case owner.APIVersion
                Case APIKeySystems.Version2
                    Return owner.Account
                Case Else
                    Return Nothing
            End Select
        Else
            Return owner.Account
        End If
    End Function

    Public Shared Function GetAccountOwnerIDForCorpOwner(owner As PrismOwner, apiType As CorpRepType) As String
        If owner.IsCorp = True Then
            Select Case owner.APIVersion
                Case APIKeySystems.Version2
                    Return owner.ID
                Case Else
                    Return ""
            End Select
        Else
            Return owner.ID
        End If
    End Function

#End Region
End Class

