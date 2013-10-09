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
Imports System.IO
Imports ProtoBuf

Public Class PlugInData
    Implements Core.IEveHQPlugIn

    Public Shared ModuleChanges As New SortedList(Of String, String)
    Private _activeForm As frmHQF

#Region "Plug-in Interface Properties and Functions"

    Public Function GetPlugInData(ByVal data As Object, ByVal dataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Select Case dataType
            Case 0 ' Fitting Protocol
                ' Check for fitting protocol
                Dim fb As New frmFittingBrowser
                fb.DNAFit = ParseFittingLink(CStr(data))
                fb.TopMost = True
                fb.Show()
                Return Nothing
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Try
            ' Check for existance of HQF folder & create if not existing
            If Core.HQ.IsUsingLocalFolders = False Then
                Settings.HQFFolder = Path.Combine(Core.HQ.AppDataFolder, "HQF")
            Else
                Settings.HQFFolder = Path.Combine(Application.StartupPath, "HQF")
            End If
            If My.Computer.FileSystem.DirectoryExists(Settings.HQFFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(Settings.HQFFolder)
            End If

            Settings.HQFCacheFolder = Core.HQ.coreCacheFolder

            Dim mods() As String = My.Resources.ModuleChanges.Split(ControlChars.CrLf.ToCharArray)
            ModuleChanges.Clear()
            For Each modLine As String In mods
                If modLine <> "" Then
                    Dim modData() As String = modLine.Split(",".ToCharArray)
                    ModuleChanges.Add(modData(0).Trim(ControlChars.Quote), modData(1).Trim(ControlChars.Quote))
                End If
            Next
            Engine.BuildPirateImplants()
            Engine.BuildBoosterPenaltyList()
            Engine.BuildEffectsMap()
            Engine.BuildShipEffectsMap()
            Engine.BuildShipBonusesMap()
            Engine.BuildSubSystemBonusMap()

            ' Load the HQF cache data
            Dim s As FileStream

            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFCacheFolder, "attributes.dat")) = True Then
                s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "attributes.dat"), FileMode.Open)
                Try
                    Attributes.AttributeList = Serializer.Deserialize(Of SortedList(Of Integer, Attribute))(s)
                    s.Close()
                Catch sex As Exception
                    s.Close()
                End Try
                s.Dispose()
            End If

            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFCacheFolder, "ships.dat")) = True Then
                s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "ships.dat"), FileMode.Open)
                Try
                    ShipLists.ShipList = Serializer.Deserialize(Of SortedList(Of String, Ship))(s)
                    s.Close()
                    For Each cShip As Ship In ShipLists.ShipList.Values
                        ShipLists.ShipListKeyID.Add(cShip.ID, cShip.Name)
                        ShipLists.ShipListKeyName.Add(cShip.Name, cShip.ID)
                    Next
                Catch sex As Exception
                    s.Close()
                End Try
                s.Dispose()
            End If

            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFCacheFolder, "modules.dat")) = True Then
                s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "modules.dat"), FileMode.Open)
                Try
                    ModuleLists.ModuleList = Serializer.Deserialize(Of SortedList(Of Integer, ShipModule))(s)
                    s.Close()
                    For Each cMod As ShipModule In ModuleLists.ModuleList.Values
                        ModuleLists.ModuleListName.Add(cMod.Name.Trim, cMod.ID)
                        If cMod.IsCharge = True Then
                            If Charges.ChargeGroups.ContainsKey(cMod.MarketGroup) = False Then
                                Charges.ChargeGroups.Add(cMod.MarketGroup, cMod.MarketGroup & "_" & cMod.DatabaseGroup & "_" & cMod.Name & "_" & cMod.ChargeSize)
                            End If
                        End If
                    Next
                Catch sex As Exception
                    s.Close()
                End Try
                s.Dispose()
            End If

            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFCacheFolder, "implants.dat")) = True Then
                s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "implants.dat"), FileMode.Open)
                Try
                    Implants.ImplantList = Serializer.Deserialize(Of SortedList(Of String, ShipModule))(s)
                    s.Close()
                Catch sex As Exception
                    s.Close()
                End Try
                s.Dispose()
            End If

            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFCacheFolder, "boosters.dat")) = True Then
                s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "boosters.dat"), FileMode.Open)
                Try
                    Boosters.BoosterList = Serializer.Deserialize(Of SortedList(Of String, ShipModule))(s)
                    s.Close()
                Catch sex As Exception
                    s.Close()
                End Try
                s.Dispose()
            End If

            If My.Computer.FileSystem.FileExists(Path.Combine(Settings.HQFCacheFolder, "skills.dat")) = True Then
                s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "skills.dat"), FileMode.Open)
                Try
                    SkillLists.SkillList = Serializer.Deserialize(Of SortedList(Of Integer, Skill))(s)
                    s.Close()
                Catch sex As Exception
                    s.Close()
                End Try
                s.Dispose()
            End If

            ' Load Item Groups (from Core data)
            s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "ItemGroups.dat"), FileMode.Open)
            ModuleLists.TypeGroups = Serializer.Deserialize(Of SortedList(Of Integer, String))(s)
            s.Close()
            s.Dispose()

            ' Load Items Cats (from Core data)
            s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "ItemCats.dat"), FileMode.Open)
            ModuleLists.TypeCats = Serializer.Deserialize(Of SortedList(Of Integer, String))(s)
            s.Close()
            s.Dispose()

            ' Load Group Cats (from Core data)
            s = New FileStream(Path.Combine(Settings.HQFCacheFolder, "GroupCats.dat"), FileMode.Open)
            ModuleLists.GroupCats = Serializer.Deserialize(Of SortedList(Of Integer, Integer))(s)
            s.Close()
            s.Dispose()

            Engine.BuildImplantEffectsMap()
            Call BuildAttributeQuickList()

            ' Create Image Cache
            Call GenerateIcons()

            ' Load the settings!
            Call Settings.HQFSettings.LoadHQFSettings()

            ' Load the Profiles - stored separately from settings for distribution!
            Call HQFDamageProfiles.Load()
            Call HQFDefenceProfiles.Load()

            ' Load up a collection of pilots from the EveHQ Core
            Call FittingPilots.LoadHQFPilotData()

            ' Load saved setups into the fitting array
            Call SavedFittings.LoadFittings()

            Return True

        Catch ex As Exception
            MessageBox.Show("Error in the EveHQ.HQF startup routine: " & ex.Message)

        End Try
    End Function

    Private Sub GenerateIcons()
        ' Create Image Cache
        ImageHandler.BaseIcons.Clear()
        For Each sMod As ShipModule In ModuleLists.ModuleList.Values
            If sMod.Icon <> "" Then
                If ImageHandler.BaseIcons.ContainsKey(sMod.Icon) = False Then
                    Dim oi As Drawing.Bitmap = CType(My.Resources.ResourceManager.GetObject("_" & sMod.Icon), Drawing.Bitmap)
                    If oi IsNot Nothing Then
                        ImageHandler.BaseIcons.Add(sMod.Icon, oi)
                    End If
                End If
            End If
        Next
        ImageHandler.MetaIcons.Clear()
        For idx As Integer = 0 To 32
            Dim oi As Drawing.Bitmap = CType(My.Resources.ResourceManager.GetObject("Meta" & (2 ^ idx).ToString), Drawing.Bitmap)
            If oi IsNot Nothing Then
                ImageHandler.MetaIcons.Add((2 ^ idx).ToString, oi)
            End If
        Next
        ' Combine the images
        Call ImageHandler.CombineIcons24()
        Call ImageHandler.CombineIcons48()
    End Sub

    Private Sub BuildAttributeQuickList()
        Attributes.AttributeQuickList.Clear()
        Dim attData As Attribute
        For Each att As Integer In Attributes.AttributeList.Keys
            attData = Attributes.AttributeList(att)
            If attData.DisplayName <> "" Then
                Attributes.AttributeQuickList.Add(attData.ID, attData.DisplayName)
            Else
                Attributes.AttributeQuickList.Add(attData.ID, attData.Name)
            End If
        Next
    End Sub

    Public Function GetEveHQPlugInInfo() As Core.EveHQPlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim eveHQPlugIn As New Core.EveHQPlugIn
        eveHQPlugIn.Name = "EveHQ Fitter"
        eveHQPlugIn.Description = "Allows theoretical ship setup and simulation"
        eveHQPlugIn.Author = "EveHQ Team"
        eveHQPlugIn.MainMenuText = "EveHQ Fitter"
        eveHQPlugIn.RunAtStartup = True
        eveHQPlugIn.RunInIGB = True
        eveHQPlugIn.MenuImage = My.Resources.plugin_icon
        eveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return eveHQPlugIn
    End Function

    Public Function IGBService(ByVal igbContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return Classes.IGBData.Response(igbContext)
    End Function

    Public Function RunEveHQPlugIn() As Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        _activeForm = New frmHQF()
        Return _activeForm
    End Function

    Public Function SaveAll() As Boolean Implements Core.IEveHQPlugIn.SaveAll
        If _activeForm IsNot Nothing Then
            _activeForm.SaveAll()
            Return True
        End If
        Return False
    End Function

#End Region

#Region "Fitting Link Parser"
    'fitting://evehq/28710:2032*1:2420*4:15681*4:15905*1:17498*2:19191*1:19814*2:24348*3:26416*1:26418*1
    '?sourceURL=http://eve.battleclinic.com/loadout/21813-Golem-that-actually-has-TP-039-s.html

    Private Function ParseFittingLink(ByVal dna As String) As DNAFitting
        Dim shipDNA As New DNAFitting
        dna = dna.TrimStart("fitting://".ToCharArray).Trim("/".ToCharArray)
        ' Remove the application name
        Dim appSep As Integer = dna.IndexOf("/", StringComparison.Ordinal)
        dna = dna.Remove(0, appSep + 1)

        ' Remove any query string to analyse later
        Dim parts() As String = dna.Split("?".ToCharArray)
        Dim mods() As String = parts(0).Split(":".ToCharArray)

        shipDNA.ShipID = CInt(mods(0))
        For modNo As Integer = 1 To mods.Length - 1
            Dim modList As List(Of String) = mods(modNo).Split(";".ToCharArray).ToList
            If modList.Count > 0 Then
                For Each modID As String In modList
                    If ModuleLists.ModuleList.ContainsKey(CInt(modID)) = True Then
                        Dim fModule As ShipModule = ModuleLists.ModuleList(CInt(modID))
                        If fModule.IsCharge Then
                            shipDNA.Charges.Add(fModule.ID)
                        Else
                            shipDNA.Modules.Add(fModule.ID)
                        End If
                    End If
                Next
            End If
        Next

        If parts.Length > 1 Then
            Dim args() As String = parts(1).Split("&".ToCharArray)
            For Each arg As String In args
                Dim argData() As String = arg.Split("=".ToCharArray)
                shipDNA.Arguments.Add(argData(0), argData(1))
            Next
        End If

        Return shipDNA
    End Function

#End Region

End Class
