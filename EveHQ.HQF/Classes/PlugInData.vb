﻿' ========================================================================
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
Imports System.Runtime.Serialization.Formatters.Binary

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn

    Shared MarketGroupData As DataSet
    Shared shipGroupData As DataSet
    Shared shipNameData As DataSet
    Shared moduleData As DataSet
    Shared moduleEffectData As DataSet
    Shared moduleAttributeData As DataSet
    Shared UseSerializableData As Boolean = False
    Public Shared ModuleChanges As New SortedList(Of String, String)
    Shared LastCacheRefresh As String = "2.11.9"

#Region "Plug-in Interface Properties and Functions"

    Public Function GetPlugInData(ByVal Data As Object, ByVal DataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Select Case DataType
            Case 0 ' Fitting Protocol
                ' Check for fitting protocol
                Dim fb As New frmFittingBrowser
                fb.DNAFit = Me.ParseFittingLink(CStr(Data))
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
            EveHQ.Core.HQ.WriteLogEvent("HQF: Checking for HQF folder...")
            If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
                Settings.HQFFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "HQF")
            Else
                Settings.HQFFolder = Path.Combine(Application.StartupPath, "HQF")
            End If
            If My.Computer.FileSystem.DirectoryExists(Settings.HQFFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(Settings.HQFFolder)
            End If

            ' Check for cache folder
            EveHQ.Core.HQ.WriteLogEvent("HQF: Checking for HQF Cache folder...")
            Settings.HQFCacheFolder = Path.Combine(Settings.HQFFolder, "Cache")
            If My.Computer.FileSystem.DirectoryExists(Settings.HQFCacheFolder) = True Then
                ' Check for last cache version file
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "version.txt")) = True Then
                    Dim sr As New StreamReader(Path.Combine(HQF.Settings.HQFCacheFolder, "version.txt"))
                    Dim cacheVersion As String = sr.ReadToEnd
                    sr.Close()
                    If IsUpdateAvailable(cacheVersion, PlugInData.LastCacheRefresh) = True Then
                        ' Delete the existing cache folder and force a rebuild
                        My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Cache outdated - rebuild of cache data required")
                        PlugInData.UseSerializableData = False
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Cache still relevant - using existing cache data")
                        PlugInData.UseSerializableData = True
                    End If
                Else
                    ' Delete the existing cache folder and force a rebuild
                    My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    EveHQ.Core.HQ.WriteLogEvent("HQF: Cache version not found - rebuild of cache data required")
                    PlugInData.UseSerializableData = False
                End If
            Else
                EveHQ.Core.HQ.WriteLogEvent("HQF: Cache folder not found - rebuild of cache data required")
                PlugInData.UseSerializableData = False
            End If

            EveHQ.Core.HQ.WriteLogEvent("HQF: Checking for module replacement list...")
            Dim Mods() As String = My.Resources.ModuleChanges.Split(ControlChars.CrLf.ToCharArray)
            ModuleChanges.Clear()
            For Each ModLine As String In Mods
                If ModLine <> "" Then
                    Dim ModData() As String = ModLine.Split(",".ToCharArray)
                    ModuleChanges.Add(ModData(0).Trim(ControlChars.Quote), ModData(1).Trim(ControlChars.Quote))
                End If
            Next
            EveHQ.Core.HQ.WriteLogEvent("HQF: Building pirate implant list...")
            Engine.BuildPirateImplants()
            EveHQ.Core.HQ.WriteLogEvent("HQF: Building booster penalty list...")
            Engine.BuildBoosterPenaltyList()
            EveHQ.Core.HQ.WriteLogEvent("HQF: Building effects map...")
            Engine.BuildEffectsMap()
            EveHQ.Core.HQ.WriteLogEvent("HQF: Building ship effects map...")
            Engine.BuildShipEffectsMap()
            EveHQ.Core.HQ.WriteLogEvent("HQF: Building ship bonuses map...")
            Engine.BuildShipBonusesMap()
            EveHQ.Core.HQ.WriteLogEvent("HQF: Building subsystems bonuses map...")
            Engine.BuildSubSystemBonusMap()

            ' Check for the existence of the binary data
            Dim NoSerializableErrors As Boolean = True

            If PlugInData.UseSerializableData = True Then

                EveHQ.Core.HQ.WriteLogEvent("HQF: Loading cache data...")

                If NoSerializableErrors = True Then
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "attributes.bin")) = True Then
                        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "attributes.bin"), FileMode.Open)
                        Try
                            Dim f As BinaryFormatter = New BinaryFormatter
                            Attributes.AttributeList = CType(f.Deserialize(s), SortedList)
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Attributes file successfully loaded")
                        Catch sex As Exception
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Error opening Attributes file: " & sex.Message)
                            NoSerializableErrors = False
                        End Try
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Attributes file not found")
                        NoSerializableErrors = False
                    End If
                End If

                If NoSerializableErrors = True Then
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "ships.bin")) = True Then
                        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "ships.bin"), FileMode.Open)
                        Try
                            Dim f As BinaryFormatter = New BinaryFormatter
                            ShipLists.shipList = CType(f.Deserialize(s), SortedList)
                            s.Close()
                            For Each cShip As Ship In ShipLists.shipList.Values
                                ShipLists.shipListKeyID.Add(cShip.ID, cShip.Name)
                                ShipLists.shipListKeyName.Add(cShip.Name, cShip.ID)
                            Next
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Ships file successfully loaded")
                        Catch sex As Exception
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Error opening Ships file: " & sex.Message)
                            NoSerializableErrors = False
                        End Try
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Ships file not found")
                        NoSerializableErrors = False
                    End If
                End If

                If NoSerializableErrors = True Then
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "modules.bin")) = True Then
                        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "modules.bin"), FileMode.Open)
                        Try
                            Dim f As BinaryFormatter = New BinaryFormatter
                            ModuleLists.moduleList = CType(f.Deserialize(s), SortedList)
                            s.Close()
                            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                                ModuleLists.moduleListName.Add(cMod.Name.Trim, cMod.ID)
                                If cMod.IsCharge = True Then
                                    If Charges.ChargeGroups.Contains(cMod.MarketGroup) = False Then
                                        Charges.ChargeGroups.Add(cMod.MarketGroup & "_" & cMod.DatabaseGroup & "_" & cMod.Name & "_" & cMod.ChargeSize)
                                    End If
                                End If
                            Next
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Modules file successfully loaded")
                        Catch sex As Exception
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Error opening Modules file: " & sex.Message)
                            NoSerializableErrors = False
                        End Try
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Modules file not found")
                        NoSerializableErrors = False
                    End If
                End If

                If NoSerializableErrors = True Then
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "implants.bin")) = True Then
                        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "implants.bin"), FileMode.Open)
                        Try
                            Dim f As BinaryFormatter = New BinaryFormatter
                            Implants.implantList = CType(f.Deserialize(s), SortedList)
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Implants file successfully loaded")
                        Catch sex As Exception
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Error opening Implants file: " & sex.Message)
                            NoSerializableErrors = False
                        End Try
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Implants file not found")
                        NoSerializableErrors = False
                    End If
                End If

                If NoSerializableErrors = True Then
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "boosters.bin")) = True Then
                        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "boosters.bin"), FileMode.Open)
                        Try
                            Dim f As BinaryFormatter = New BinaryFormatter
                            Boosters.BoosterList = CType(f.Deserialize(s), SortedList)
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Boosters file successfully loaded")
                        Catch sex As Exception
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Error opening Boosters file: " & sex.Message)
                            NoSerializableErrors = False
                        End Try
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Boosters file not found")
                        NoSerializableErrors = False
                    End If
                End If

                If NoSerializableErrors = True Then
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "skills.bin")) = True Then
                        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "skills.bin"), FileMode.Open)
                        Try
                            Dim f As BinaryFormatter = New BinaryFormatter
                            SkillLists.SkillList = CType(f.Deserialize(s), SortedList)
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Skills file successfully loaded")
                        Catch sex As Exception
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Error opening Skills file: " & sex.Message)
                            NoSerializableErrors = False
                        End Try
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Skills file not found")
                        NoSerializableErrors = False
                    End If
                End If

                If NoSerializableErrors = True Then
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "NPCs.bin")) = True Then
                        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "NPCs.bin"), FileMode.Open)
                        Try
                            Dim f As BinaryFormatter = New BinaryFormatter
                            NPCs.NPCList = CType(f.Deserialize(s), SortedList)
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: NPCs file successfully loaded")
                        Catch sex As Exception
                            s.Close()
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Error opening NPCs file: " & sex.Message)
                            NoSerializableErrors = False
                        End Try
                    Else
                        EveHQ.Core.HQ.WriteLogEvent("HQF: NPCs file not found")
                        NoSerializableErrors = False
                    End If
                End If

                ' Ask what we want to do in the case of errors during the deserialization process
                If NoSerializableErrors = False Then
                    Dim msg As String = "There was an error loading one of the HQF cache files. Would you like to try and re-generate the cache data to continue?"
                    Dim reply As DialogResult = MessageBox.Show(msg, "Re-create HQF Cache Data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = DialogResult.Yes Then
                        Return Me.GenerateHQFCacheData
                    Else
                        MessageBox.Show("Loading of HQF aborted due to corrupt cache files", "HQF Loading Aborted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return False
                    End If
                End If

                EveHQ.Core.HQ.WriteLogEvent("HQF: Building attribute list...")
                Call Me.BuildAttributeQuickList()
                EveHQ.Core.HQ.WriteLogEvent("HQF: Building Implant Effects map...")
                Engine.BuildImplantEffectsMap()

                ' Load any custom ship classes and ships then implement into HQF
                EveHQ.Core.HQ.WriteLogEvent("HQF: Loading custom ship data...")
                CustomHQFClasses.LoadCustomShipClasses()
                CustomHQFClasses.LoadCustomShips()
                CustomHQFClasses.ImplementCustomShips()

                ' Convert the old fittings file to the new version
                EveHQ.Core.HQ.WriteLogEvent("HQF: Checking for old version file...")
                Call SavedFittings.ConvertOldFittingsFile()

                ' Create Image Cache
                EveHQ.Core.HQ.WriteLogEvent("HQF: Generating icons...")
                Call GenerateIcons()

                EveHQ.Core.HQ.WriteLogEvent("HQF: Initialisation complete!")
                Return True

            Else
                ' Generate the HQF Cache Data
                Return Me.GenerateHQFCacheData()
            End If
        Catch ex As Exception
            EveHQ.Core.HQ.WriteLogEvent("HQF: Exception occured - " & ex.Message)
            Dim msg As String = "There was an error loading the HQF cache files. Would you like to try and re-generate the cache data to continue?"
            Dim reply As DialogResult = MessageBox.Show(msg, "Re-create HQF Cache Data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.Yes Then
                Return Me.GenerateHQFCacheData
            Else
                MessageBox.Show("Loading of HQF aborted due to corrupt cache files", "HQF Loading Aborted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End Try
    End Function

    Private Sub GenerateIcons()
        ' Create Image Cache
        ImageHandler.BaseIcons.Clear()
        Dim IconList As New List(Of String)
        For Each sMod As ShipModule In ModuleLists.moduleList.Values
            If sMod.MetaType = MetaTypes.Tech1 Or sMod.MetaType = MetaTypes.Tech3 Then
                If sMod.Icon <> "" Then
                    If ImageHandler.BaseIcons.ContainsKey(sMod.Icon) = False Then
                        Dim OI As Drawing.Bitmap = CType(EveHQ.Core.ImageHandler.GetImage(sMod.ID, 64, sMod.Icon), Drawing.Bitmap)
                        If OI IsNot Nothing Then
                            ImageHandler.BaseIcons.Add(sMod.Icon, OI)
                        End If
                    End If
                End If
            End If
        Next
        ImageHandler.MetaIcons.Clear()
        For idx As Integer = 0 To 32
            Dim OI As Drawing.Bitmap = CType(My.Resources.ResourceManager.GetObject("Meta" & (2 ^ idx).ToString), Drawing.Bitmap)
            If OI IsNot Nothing Then
                ImageHandler.MetaIcons.Add((2 ^ idx).ToString, OI)
            End If
        Next
        ' Combine the images
        Call ImageHandler.CombineIcons24()
        Call ImageHandler.CombineIcons48()
    End Sub

    Public Function GenerateHQFCacheData() As Boolean
        Try
            EveHQ.Core.HQ.WriteLogEvent("HQF: Generating HQF cache data...")
            ' Populate the Ship data
            EveHQ.Core.HQ.WriteLogEvent("HQF: Loading attributes...")
            If Me.LoadAttributes = True Then
                EveHQ.Core.HQ.WriteLogEvent("HQF: Loading skill data...")
                If Me.LoadSkillData = True Then
                    EveHQ.Core.HQ.WriteLogEvent("HQF: Loading ship group data...")
                    If Me.LoadShipGroupData = True Then
                        EveHQ.Core.HQ.WriteLogEvent("HQF: Loading market group data...")
                        If Me.LoadMarketGroupData = True Then
                            EveHQ.Core.HQ.WriteLogEvent("HQF: Loading ship name data...")
                            If Me.LoadShipNameData = True Then
                                EveHQ.Core.HQ.WriteLogEvent("HQF: Loading ship attribute data...")
                                If Me.LoadShipAttributeData = True Then
                                    EveHQ.Core.HQ.WriteLogEvent("HQF: Populating ship lists...")
                                    Call Me.PopulateShipLists()
                                    EveHQ.Core.HQ.WriteLogEvent("HQF: Loading module data...")
                                    If Me.LoadModuleData = True Then
                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Loading module effect data...")
                                        If Me.LoadModuleEffectData = True Then
                                            EveHQ.Core.HQ.WriteLogEvent("HQF: Loading module attribute data...")
                                            If Me.LoadModuleAttributeData = True Then
                                                EveHQ.Core.HQ.WriteLogEvent("HQF: Loading module meta types...")
                                                If Me.LoadModuleMetaTypes = True Then
                                                    EveHQ.Core.HQ.WriteLogEvent("HQF: Building module data...")
                                                    If Me.BuildModuleData = True Then
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Building attribute lists...")
                                                        Call Me.BuildAttributeQuickList()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Building implant effects map...")
                                                        Engine.BuildImplantEffectsMap()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Building module effects...")
                                                        Call Me.BuildModuleEffects()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Building implant effects...")
                                                        Call Me.BuildImplantEffects()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Building ship effects...")
                                                        Call Me.BuildShipEffects()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Building subsystem effects...")
                                                        Call Me.BuildSubsystemEffects()
                                                        ' Save the HQF data
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Saving HQF cache data...")
                                                        Call Me.SaveHQFCacheData()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Cleaning up data...")
                                                        Call Me.CleanUpData()
                                                        ' Load any custom ship classes and ships then implement into HQF
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Loading custom ship classes...")
                                                        CustomHQFClasses.LoadCustomShipClasses()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Loading custom ships...")
                                                        CustomHQFClasses.LoadCustomShips()
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Implementing custom ships...")
                                                        CustomHQFClasses.ImplementCustomShips()
                                                        ' Convert the old fittings file to the new version
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Checking old fittings file...")
                                                        Call SavedFittings.ConvertOldFittingsFile()
                                                        ' Generate the icons
                                                        EveHQ.Core.HQ.WriteLogEvent("HQF: Generating icons...")
                                                        Call GenerateIcons()
                                                        Return True
                                                    Else
                                                        Return False
                                                    End If
                                                Else
                                                    Return False
                                                End If
                                            Else
                                                Return False
                                            End If
                                        Else
                                            Return False
                                        End If
                                    Else
                                        Return False
                                    End If
                                Else
                                    Return False
                                End If
                            Else
                                Return False
                            End If
                        Else
                            Return False
                        End If
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
            Return True
        Catch ex As Exception
            MessageBox.Show("There was an error generating the HQF cache data. The error was: " & ex.Message, "Error Generating HQF Cache", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function IsUpdateAvailable(ByVal localVer As String, ByVal remoteVer As String) As Boolean
        If localVer = remoteVer Then
            Return False
        Else
            Dim localVers() As String = localVer.Split(CChar("."))
            Dim remoteVers() As String = remoteVer.Split(CChar("."))
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
        End If
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Fitter"
        EveHQPlugIn.Description = "Allows theoretical ship setup and simulation"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "EveHQ Fitter"
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
        Return New frmHQF
    End Function
#End Region

#Region "Plug-In Initialisation (Data Loading/Building) Routines"

    ' Market Loading Routines & Bonuses
    Private Function LoadMarketGroupData() As Boolean
        Try
            Call Me.LoadAttributes()
            Dim strSQL As String = ""
            strSQL &= "SELECT * FROM invMarketGroups ORDER BY parentGroupID;"
            PlugInData.MarketGroupData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If PlugInData.MarketGroupData IsNot Nothing Then
                If PlugInData.MarketGroupData.Tables(0).Rows.Count <> 0 Then
                    Market.MarketGroupList.Clear()
                    For Each row As DataRow In PlugInData.MarketGroupData.Tables(0).Rows
                        Market.MarketGroupList.Add(row.Item("marketGroupID").ToString, row.Item("marketGroupName").ToString)
                    Next
                    Return True
                Else
                    MessageBox.Show("Market Group Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Market Group Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Market Group Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadAttributes() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT dgmAttributeTypes.attributeID, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName AS dgmAttributeTypes_displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, dgmAttributeTypes.attributeGroup, eveUnits.unitName, eveUnits.displayName AS eveUnits_displayName"
            strSQL &= " FROM eveUnits INNER JOIN dgmAttributeTypes ON eveUnits.unitID = dgmAttributeTypes.unitID"
            strSQL &= " ORDER BY dgmAttributeTypes.attributeID;"
            Dim attributeData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If attributeData IsNot Nothing Then
                If attributeData.Tables(0).Rows.Count <> 0 Then
                    Attributes.AttributeList.Clear()
                    Dim attData As New Attribute
                    For Each row As DataRow In attributeData.Tables(0).Rows
                        attData = New Attribute
                        attData.ID = row.Item("attributeID").ToString
                        attData.Name = row.Item("attributeName").ToString
                        attData.DisplayName = row.Item("dgmAttributeTypes_displayName").ToString
                        attData.UnitName = row.Item("eveUnits_displayName").ToString
                        attData.AttributeGroup = row.Item("attributeGroup").ToString
                        If attData.UnitName = "ms" Then
                            attData.UnitName = "s"
                        End If
                        Attributes.AttributeList.Add(attData.ID, attData)
                    Next
                    Return Me.LoadCustomAttributes()
                Else
                    MessageBox.Show("Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Attribute Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadCustomAttributes() As Boolean
        Dim attributeData As String = My.Resources.Attributes.ToString
        Dim attributeLines() As String = attributeData.Split(ControlChars.CrLf.ToCharArray)
        Dim att() As String
        Dim attData As New Attribute
        For Each line As String In attributeLines
            If line.Trim <> "" And line.StartsWith("#") = False Then
                att = line.Split(",".ToCharArray)
                attData = New Attribute
                attData.ID = att(0)
                attData.Name = att(1)
                attData.DisplayName = att(2)
                attData.UnitName = att(4)
                attData.AttributeGroup = att(5)
                Attributes.AttributeList.Add(attData.ID, attData)
            End If
        Next
        Return True
    End Function

    ' Skill Loading Routines
    Private Function LoadSkillData() As Boolean
        Try
            Dim skillData As DataSet
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName,  invTypes.basePrice, invTypes.published, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
            strSQL &= " WHERE invCategories.categoryID=16 ORDER BY typeName;"
            skillData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If skillData IsNot Nothing Then
                If skillData.Tables(0).Rows.Count <> 0 Then
                    HQF.SkillLists.SkillList.Clear()
                    For Each skillRow As DataRow In skillData.Tables(0).Rows
                        ' Check if the typeID already exists
                        Dim newSkill As HQF.Skill
                        If HQF.SkillLists.SkillList.Contains(skillRow.Item("typeID").ToString) = False Then
                            newSkill = New HQF.Skill
                            newSkill.ID = skillRow.Item("typeID").ToString.Trim
                            newSkill.GroupID = skillRow.Item("groupID").ToString.Trim
                            newSkill.Name = skillRow.Item("typeName").ToString.Trim
                            HQF.SkillLists.SkillList.Add(newSkill.ID, newSkill)
                        Else
                            newSkill = CType(HQF.SkillLists.SkillList(skillRow.Item("typeID").ToString), HQF.Skill)
                        End If
                        If IsDBNull(skillRow.Item("valueInt")) = False Then
                            newSkill.Attributes.Add(skillRow.Item("attributeID").ToString, skillRow.Item("valueInt"))
                        Else
                            newSkill.Attributes.Add(skillRow.Item("attributeID").ToString, skillRow.Item("valueFloat"))
                        End If
                    Next
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Skill Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' Ship Loading Routines
    Private Function LoadShipGroupData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT * FROM invGroups WHERE invGroups.categoryID=6 ORDER BY groupName;"
            PlugInData.shipGroupData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If PlugInData.shipGroupData IsNot Nothing Then
                If PlugInData.shipGroupData.Tables(0).Rows.Count <> 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Group Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadShipNameData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.published, invTypes.raceID, invTypes.marketGroupID"
            strSQL &= " FROM (invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID"
            strSQL &= " WHERE (invCategories.categoryID=6 AND invTypes.published=1) ORDER BY typeName;"
            PlugInData.shipNameData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If PlugInData.shipNameData IsNot Nothing Then
                If PlugInData.shipNameData.Tables(0).Rows.Count <> 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Name Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadShipAttributeData() As Boolean
        Try
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            ' Get details of ship data from database
            Dim strSQL As String = ""
            Dim pSkillName As String = "" : Dim sSkillName As String = "" : Dim tSkillName As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
            strSQL &= " WHERE ((invCategories.categoryID=6 AND invTypes.published=1) OR invTypes.typeID IN (601,596,588,606)) ORDER BY typeName, attributeID;"
            Dim shipData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If shipData IsNot Nothing Then
                If shipData.Tables(0).Rows.Count <> 0 Then
                    ShipLists.shipList.Clear()
                    Dim lastShipName As String = ""
                    Dim newShip As New EveHQ.HQF.Ship
                    pSkillName = "" : sSkillName = "" : tSkillName = ""
                    Dim attValue As Double = 0
                    For Each shipRow As DataRow In shipData.Tables(0).Rows
                        ' If the shipName has changed, we need to start a new ship type
                        If lastShipName <> shipRow.Item("typeName").ToString Then
                            ' Add the current ship to the list then reset the ship data
                            If lastShipName <> "" Then
                                Call newShip.AddCustomShipAttributes()
                                ' Map the attributes
                                Ship.MapShipAttributes(newShip)
                                ShipLists.shipList.Add(newShip.Name, newShip)
                                newShip = New EveHQ.HQF.Ship
                                pSkillName = "" : sSkillName = "" : tSkillName = ""
                            End If
                            ' Create new ship type & non "attribute" data
                            newShip.Name = shipRow.Item("typeName").ToString
                            newShip.ID = shipRow.Item("typeID").ToString
                            newShip.Description = shipRow.Item("description").ToString
                            newShip.DatabaseGroup = shipRow.Item("groupID").ToString
                            newShip.DatabaseCategory = shipRow.Item("categoryID").ToString
                            newShip.MarketGroup = shipRow.Item("marketGroupID").ToString
                            newShip.BasePrice = CDbl(shipRow.Item("basePrice"))
                            newShip.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(newShip.ID)
                            newShip.Mass = CDbl(shipRow.Item("mass"))
                            newShip.Volume = CDbl(shipRow.Item("volume"))
                            newShip.CargoBay = CDbl(shipRow.Item("capacity"))
                            If IsDBNull(shipRow.Item("raceID")) = False Then
                                newShip.RaceID = CInt(shipRow.Item("raceID"))
                            Else
                                newShip.RaceID = 0
                            End If
                        End If

                        ' Now get, modify (if applicable) and add the "attribute"

                        If IsDBNull(shipRow.Item("valueInt")) = True Then
                            attValue = CDbl(shipRow.Item("valueFloat"))
                        Else
                            attValue = CDbl(shipRow.Item("valueInt"))
                        End If

                        ' Do attribute (unit) modifiers
                        Select Case shipRow.Item("attributeID").ToString
                            Case Attributes.Ship_CapRechargeTime, Attributes.Ship_CloakReactivationDelay, Attributes.Ship_ShieldRechargeTime
                                attValue = attValue / 1000
                            Case Attributes.Ship_HullEMResistance, Attributes.Ship_HullExpResistance, Attributes.Ship_HullKinResistance, Attributes.Ship_HullThermResistance,
                                Attributes.Ship_ArmorEMResistance, Attributes.Ship_ArmorExpResistance, Attributes.Ship_ArmorKinResistance, Attributes.Ship_ArmorThermResistance,
                                Attributes.Ship_ShieldEMResistance, Attributes.Ship_ShieldExpResistance, Attributes.Ship_ShieldKinResistance, Attributes.Ship_ShieldThermResistance
                                attValue = (1 - attValue) * 100
                            Case Attributes.Ship_WarpSpeed
                                attValue = attValue * 3
                        End Select

                        ' Add the attribute to the ship.attributes list
                        newShip.Attributes.Add(shipRow.Item("attributeID").ToString, attValue)

                        ' Map only the skill attributes
                        Select Case shipRow.Item("attributeID").ToString
                            Case Attributes.Ship_ReqSkill1
                                Dim pSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = pSkill.ID
                                nSkill.Name = pSkill.Name
                                pSkillName = pSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case Attributes.Ship_ReqSkill2
                                Dim sSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = sSkill.ID
                                nSkill.Name = sSkill.Name
                                sSkillName = sSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case Attributes.Ship_ReqSkill3
                                Dim tSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = tSkill.ID
                                nSkill.Name = tSkill.Name
                                tSkillName = tSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case Attributes.Ship_ReqSkill1Level
                                Dim cSkill As ItemSkills = CType(newShip.RequiredSkills(pSkillName), ItemSkills)
                                If cSkill IsNot Nothing Then
                                    cSkill.Level = CInt(attValue)
                                End If

                            Case Attributes.Ship_ReqSkill2Level
                                Dim cSkill As ItemSkills = CType(newShip.RequiredSkills(sSkillName), ItemSkills)
                                If cSkill IsNot Nothing Then
                                    cSkill.Level = CInt(attValue)
                                End If
                            Case Attributes.Ship_ReqSkill3Level
                                Dim cSkill As ItemSkills = CType(newShip.RequiredSkills(tSkillName), ItemSkills)
                                If cSkill IsNot Nothing Then
                                    cSkill.Level = CInt(attValue)
                                End If
                        End Select
                        lastShipName = shipRow.Item("typeName").ToString
                    Next
                    ' Add the custom attributes to the list
                    Call newShip.AddCustomShipAttributes()
                    ' Map the remaining attributes for the last ship type
                    Ship.MapShipAttributes(newShip)
                    ' Perform the last addition for the last ship type
                    ShipLists.shipList.Add(newShip.Name, newShip)
                    Return True
                Else
                    MessageBox.Show("Ship Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Ship Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Attribute Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Sub PopulateShipLists()
        ShipLists.shipListKeyName.Clear()
        ShipLists.shipListKeyID.Clear()
        For Each baseShip As Ship In ShipLists.shipList.Values
            ShipLists.shipListKeyName.Add(baseShip.Name, baseShip.ID)
            ShipLists.shipListKeyID.Add(baseShip.ID, baseShip.Name)
        Next
    End Sub

    ' Module Loading Routines
    Private Function LoadModuleData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID, eveIcons.iconFile"
            strSQL &= " FROM eveIcons RIGHT OUTER JOIN (invCategories INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invCategories.categoryID = invGroups.categoryID) ON (eveIcons.iconID = invTypes.iconID)"
            strSQL &= " WHERE (((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010"
            strSQL &= " ORDER BY invTypes.typeName;"
            PlugInData.moduleData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If PlugInData.moduleData IsNot Nothing Then
                If PlugInData.moduleData.Tables(0).Rows.Count <> 0 Then
                    Return True
                Else
                    MessageBox.Show("Module Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Module Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadModuleEffectData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeEffects.effectID"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeEffects ON invTypes.typeID=dgmTypeEffects.typeID"
            strSQL &= " WHERE (((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010"
            strSQL &= " ORDER BY typeName, effectID;"
            PlugInData.moduleEffectData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If PlugInData.moduleEffectData IsNot Nothing Then
                If PlugInData.moduleEffectData.Tables(0).Rows.Count <> 0 Then
                    Return True
                Else
                    MessageBox.Show("Module Effect Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Module Effect Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Effect Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadModuleAttributeData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName"
            strSQL &= " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID"
            strSQL &= " WHERE (((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010"
            strSQL &= " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;"

            PlugInData.moduleAttributeData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If PlugInData.moduleAttributeData IsNot Nothing Then
                If PlugInData.moduleAttributeData.Tables(0).Rows.Count <> 0 Then
                    Return True
                Else
                    MessageBox.Show("Module Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Module Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Attribute Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadModuleMetaTypes() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invMetaTypes.parentTypeID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID"
            strSQL &= " FROM (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (invMetaGroups INNER JOIN invMetaTypes ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID) ON invTypes.typeID = invMetaTypes.typeID"
            strSQL &= " WHERE (((invGroups.categoryID) In (7,8,18,20,32)) AND (invTypes.published=1))"
            Dim metaTypeData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If metaTypeData IsNot Nothing Then
                If metaTypeData.Tables(0).Rows.Count <> 0 Then
                    ModuleLists.moduleMetaTypes.Clear()
                    ModuleLists.moduleMetaGroups.Clear()
                    For Each row As DataRow In metaTypeData.Tables(0).Rows
                        If ModuleLists.moduleMetaTypes.Contains(row.Item("invTypes_typeID").ToString) = False Then
                            ModuleLists.moduleMetaTypes.Add(row.Item("invTypes_typeID").ToString, row.Item("parentTypeID").ToString)
                            ModuleLists.moduleMetaGroups.Add(row.Item("invTypes_typeID").ToString, row.Item("invMetaGroups_metaGroupID").ToString)
                        End If
                        If ModuleLists.moduleMetaTypes.Contains(row.Item("parentTypeID").ToString) = False Then
                            ModuleLists.moduleMetaTypes.Add(row.Item("parentTypeID").ToString, row.Item("parentTypeID").ToString)
                            ModuleLists.moduleMetaGroups.Add(row.Item("parentTypeID").ToString, "0")
                        End If
                    Next
                    Return True
                Else
                    MessageBox.Show("Module Metatype Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Module Metatype Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Metatype Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildModuleData() As Boolean
        Try
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            ModuleLists.moduleList.Clear()
            ModuleLists.moduleListName.Clear()
            Implants.implantList.Clear()
            Boosters.BoosterList.Clear()
            For Each row As DataRow In PlugInData.moduleData.Tables(0).Rows
                Dim newModule As New ShipModule
                newModule.ID = row.Item("typeID").ToString
                newModule.Name = row.Item("typeName").ToString.Trim
                newModule.Description = row.Item("description").ToString
                newModule.DatabaseGroup = row.Item("groupID").ToString
                newModule.DatabaseCategory = row.Item("categoryID").ToString
                newModule.BasePrice = CDbl(row.Item("baseprice"))
                newModule.Volume = CDbl(row.Item("volume"))
                newModule.Capacity = CDbl(row.Item("capacity"))
                newModule.Attributes.Add(Attributes.Module_Capacity, CDbl(row.Item("capacity")))
                newModule.Attributes.Add(Attributes.Module_Mass, CDbl(row.Item("mass")))
                If IsDBNull(row.Item("raceID")) = False Then
                    newModule.RaceID = CInt(row.Item("raceID"))
                Else
                    newModule.RaceID = 0
                End If
                newModule.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(newModule.ID)
                newModule.Icon = row.Item("iconFile").ToString.Replace("res:/UI/Texture/Icons/", "").Replace(".png", "")
                If IsDBNull(row.Item("marketGroupID")) = False Then
                    newModule.MarketGroup = row.Item("marketGroupID").ToString
                Else
                    newModule.MarketGroup = "0"
                End If
                newModule.CPU = 0
                newModule.PG = 0
                newModule.Calibration = 0
                newModule.CapUsage = 0
                newModule.ActivationTime = 0
                newModule.ReactivationDelay = 0
                ModuleLists.moduleList.Add(newModule.ID, newModule)
                ModuleLists.moduleListName.Add(newModule.Name, newModule.ID)

                ' Determine whether implant, drone, charge etc
                Select Case row.Item("categoryID").ToString
                    Case ShipModule.Category_Celestials ' Container
                        newModule.IsContainer = True
                    Case ShipModule.Category_Charges
                        newModule.IsCharge = True
                    Case ShipModule.Category_Drones
                        newModule.IsDrone = True
                    Case ShipModule.Category_Implants
                        If row.Item("groupID").ToString <> ShipModule.Group_DNAMutators Then
                            If row.Item("groupID").ToString = ShipModule.Group_Boosters Then
                                newModule.IsBooster = True
                            Else
                                newModule.IsImplant = True
                            End If
                        End If
                End Select
            Next

            ' Fill in the blank market groups now the list is complete
            Dim modName As String = ""
            Dim modID As String = ""
            Dim parentID As String = ""
            Dim nModule As New ShipModule
            Dim eModule As New ShipModule
            For setNo As Integer = 0 To 1
                For Each row As DataRow In PlugInData.moduleData.Tables(0).Rows
                    If IsDBNull(row.Item("marketGroupID")) = True Then
                        modID = row.Item("typeID").ToString
                        nModule = CType(ModuleLists.moduleList(modID), ShipModule)
                        If ModuleLists.moduleMetaTypes.Contains(modID) = True Then
                            parentID = ModuleLists.moduleMetaTypes(modID).ToString
                            eModule = CType(ModuleLists.moduleList(parentID), ShipModule)
                            nModule.MarketGroup = eModule.MarketGroup
                        End If
                    End If
                Next
            Next

            ' Search for changes/additions to the market groups from resources
            Dim marketChanges As String = My.Resources.newItemMarketGroup.ToString
            Dim changeLines As String() = marketChanges.Split(ControlChars.CrLf.ToCharArray)
            For Each marketChange As String In changeLines
                If marketChange.Trim <> "" Then
                    Dim changeData() As String = marketChange.Split(",".ToCharArray)
                    Dim typeID As String = changeData(0)
                    Dim marketGroupID As String = changeData(1)
                    Dim metaTypeID As Integer = CInt(changeData(2))
                    If ModuleLists.moduleList.ContainsKey(typeID) = True Then
                        Dim mModule As ShipModule = CType(ModuleLists.moduleList(typeID), ShipModule)
                        mModule.MarketGroup = marketGroupID
                        If metaTypeID <> 0 Then
                            mModule.MetaType = metaTypeID
                        End If
                    End If
                End If
            Next
            Return BuildModuleEffectData()
        Catch e As Exception
            MessageBox.Show("Error building Module Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildModuleEffectData() As Boolean
        Try
            ' Get details of module attributes from already retrieved dataset
            Dim attValue As Double = 0
            For Each modRow As DataRow In PlugInData.moduleEffectData.Tables(0).Rows
                Dim effMod As ShipModule = CType(ModuleLists.moduleList.Item(modRow.Item("typeID").ToString), ShipModule)
                If effMod IsNot Nothing Then
                    Select Case modRow.Item("effectID").ToString
                        Case Attributes.Effect_LowSlot
                            effMod.SlotType = SlotTypes.Low
                        Case Attributes.Effect_HighSlot
                            effMod.SlotType = SlotTypes.High
                        Case Attributes.Effect_MidSlot
                            effMod.SlotType = SlotTypes.Mid
                        Case Attributes.Effect_RigSlot
                            effMod.SlotType = SlotTypes.Rig
                        Case Attributes.Effect_SubsystemSlot
                            effMod.SlotType = SlotTypes.Subsystem
                        Case Attributes.Effect_LauncherFitted
                            If effMod.DatabaseGroup <> ShipModule.Group_ProbeLaunchers Then
                                effMod.IsLauncher = True
                            End If
                        Case Attributes.Effect_TargetAttack, Attributes.Effect_ProjectileFired, Attributes.Effect_TurretFitted
                            effMod.IsTurret = True
                    End Select
                    ' Add custom attributes
                    If effMod.IsDrone = True Or effMod.IsLauncher = True Or effMod.IsTurret = True Or effMod.DatabaseGroup = ShipModule.Group_Smartbombs Or effMod.DatabaseGroup = ShipModule.Group_BombLaunchers Then
                        If effMod.Attributes.ContainsKey(Attributes.Module_BaseDamage) = False Then
                            effMod.Attributes.Add(Attributes.Module_BaseDamage, 0)
                            effMod.Attributes.Add(Attributes.Module_VolleyDamage, 0)
                            effMod.Attributes.Add(Attributes.Module_DPS, 0)
                            effMod.Attributes.Add(Attributes.Module_LoadedCharge, 0)
                            effMod.Attributes.Add(Attributes.Module_EMDamage, 0)
                            effMod.Attributes.Add(Attributes.Module_ExpDamage, 0)
                            effMod.Attributes.Add(Attributes.Module_KinDamage, 0)
                            effMod.Attributes.Add(Attributes.Module_ThermDamage, 0)
                        End If
                    End If
                    Select Case effMod.MarketGroup
                        Case ShipModule.Marketgroup_IceHarvesters
                            If effMod.Attributes.ContainsKey(Attributes.Module_TurretIceMiningRate) = False Then
                                effMod.Attributes.Add(Attributes.Module_TurretIceMiningRate, 0)
                            End If
                        Case ShipModule.Marketgroup_MiningLasers, ShipModule.Marketgroup_StripMiners
                            If effMod.Attributes.ContainsKey(Attributes.Module_TurretOreMiningRate) = False Then
                                effMod.Attributes.Add(Attributes.Module_TurretOreMiningRate, 0)
                            End If
                        Case ShipModule.Marketgroup_MiningDrones
                            If effMod.Attributes.ContainsKey(Attributes.Module_DroneOreMiningRate) = False Then
                                effMod.Attributes.Add(Attributes.Module_DroneOreMiningRate, 0)
                            End If
                        Case ShipModule.Marketgroup_GasHarvesters
                            If effMod.Attributes.ContainsKey(Attributes.Module_TurretGasMiningRate) = False Then
                                effMod.Attributes.Add(Attributes.Module_TurretGasMiningRate, 0)
                            End If
                    End Select
                    Select Case effMod.DatabaseGroup
                        Case ShipModule.Group_CapBoosters
                            If effMod.Attributes.ContainsKey(Attributes.Module_CapacitorNeed) = False Then
                                effMod.Attributes.Add(Attributes.Module_CapacitorNeed, 0)
                            End If
                        Case ShipModule.Group_ShieldTransporters, ShipModule.Group_RemoteArmorRepairers, ShipModule.Group_RemoteHullRepairers, ShipModule.Group_LogisticDrones
                            If effMod.Attributes.ContainsKey(Attributes.Module_TransferRate) = False Then
                                effMod.Attributes.Add(Attributes.Module_TransferRate, 0)
                            End If
                    End Select
                End If
            Next
            If BuildModuleAttributeData() = True Then
                Return True
            Else
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error building Module Effect Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildModuleAttributeData() As Boolean
        Try
            ' Get details of module attributes from already retrieved dataset
            Dim attValue As Double = 0
            Dim pSkillName As String = "" : Dim sSkillName As String = "" : Dim tSkillName As String = ""
            Dim lastModName As String = ""
            For Each modRow As DataRow In PlugInData.moduleAttributeData.Tables(0).Rows
                Dim attMod As ShipModule = CType(ModuleLists.moduleList.Item(modRow.Item("typeID").ToString), ShipModule)
                'If attMod IsNot Nothing Then
                If lastModName <> modRow.Item("typeName").ToString And lastModName <> "" Then
                    pSkillName = "" : sSkillName = "" : tSkillName = ""
                End If
                ' Now get, modify (if applicable) and add the "attribute"
                If IsDBNull(modRow.Item("valueFloat")) = False Then
                    attValue = CDbl(modRow.Item("valueFloat"))
                Else
                    attValue = CDbl(modRow.Item("valueInt"))
                End If

                Select Case modRow.Item("unitID").ToString
                    Case Attributes.Unit_InverseAbsolutePercent
                        attValue = Math.Round(100 - (attValue * 100), 2)
                    Case Attributes.Unit_ModifierPercent
                        attValue = Math.Round((attValue * 100) - 100, 2)
                    Case Attributes.Unit_InverseModifierPercent
                        attValue = Math.Round((attValue - 1) * 100, 2)
                    Case Attributes.Unit_Milliseconds
                        If attValue > 1000 Then
                            attValue = Math.Round(attValue / 1000, 2)
                        End If
                End Select

                ' Modify the attribute value if we using damage controls - this is to stack up later on
                If attMod.DatabaseGroup = ShipModule.Group_DamageControls Then
                    Select Case modRow.Item("attributeID").ToString
                        Case Attributes.Module_ArmorEMResistance, Attributes.Module_ArmorExpResistance, Attributes.Module_ArmorKinResistance, Attributes.Module_ArmorThermResistance,
                            Attributes.Module_ShieldEMResistance, Attributes.Module_ShieldExpResistance, Attributes.Module_ShieldKinResistance, Attributes.Module_ShieldThermResistance,
                            Attributes.Module_HullEMResistance, Attributes.Module_HullExpResistance, Attributes.Module_HullKinResistance, Attributes.Module_HullThermResistance
                            attValue = -attValue
                    End Select
                End If

                ' Do custom attribute changes here!
                Select Case modRow.Item("attributeID").ToString
                    Case Attributes.Module_ROFBonus
                        If attValue = -100 Then Exit Select
                        attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                    Case Attributes.Module_ROF
                        If attValue = -100 Then Exit Select
                        Select Case attMod.DatabaseGroup
                            Case ShipModule.Group_EnergyTurrets
                                attMod.Attributes.Add(Attributes.Module_EnergyROF, attValue)
                            Case ShipModule.Group_HybridTurrets
                                attMod.Attributes.Add(Attributes.Module_HybridROF, attValue)
                            Case ShipModule.Group_ProjectileTurrets
                                attMod.Attributes.Add(Attributes.Module_ProjectileROF, attValue)
                            Case Else
                                attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                        End Select
                    Case Attributes.Module_DamageMod
                        If attValue = 0 Then Exit Select
                        Select Case attMod.DatabaseGroup
                            Case ShipModule.Group_EnergyTurrets
                                attMod.Attributes.Add(Attributes.Module_EnergyDmgMod, attValue)
                            Case ShipModule.Group_HybridTurrets
                                attMod.Attributes.Add(Attributes.Module_HybridDmgMod, attValue)
                            Case ShipModule.Group_ProjectileTurrets
                                attMod.Attributes.Add(Attributes.Module_ProjectileDmgMod, attValue)
                            Case Else
                                attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                        End Select
                    Case Attributes.Module_ArmorEMResistance, Attributes.Module_ArmorExpResistance, Attributes.Module_ArmorKinResistance, Attributes.Module_ArmorThermResistance
                        ' Invert Armor Resistance Shift Hardener values
                        Select Case attMod.DatabaseGroup
                            Case ShipModule.Group_ArmorResistShiftHardener
                                attValue = -attValue
                        End Select
                        attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                    Case Else
                        attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                End Select


                Select Case modRow.Item("attributeID").ToString
                    Case Attributes.Module_PowergridUsage
                        attMod.PG = attValue
                    Case Attributes.Module_CpuUsage
                        attMod.CPU = attValue
                    Case Attributes.Module_CapacitorNeed
                        attMod.CapUsage = attValue
                    Case Attributes.Module_ROF
                        If attMod.Attributes.ContainsKey(Attributes.Module_CapacitorNeed) = True Then
                            attMod.CapUsageRate = attMod.CapUsage / attValue
                            attMod.Attributes.Add(Attributes.Module_CapUsageRate, attMod.CapUsageRate)
                        End If
                    Case Attributes.Module_ActivationTime
                        attMod.ActivationTime = attValue
                        attMod.CapUsageRate = attMod.CapUsage / (attMod.ActivationTime + attMod.ReactivationDelay)
                        attMod.Attributes.Add(Attributes.Module_CapUsageRate, attMod.CapUsageRate)
                    Case Attributes.Module_ReactivationDelay
                        attMod.ReactivationDelay = attValue
                        If attMod.Attributes.ContainsKey(Attributes.Module_CapacitorNeed) = True Then
                            attMod.CapUsageRate = attMod.CapUsage / (attMod.ActivationTime + attMod.ReactivationDelay)
                            attMod.Attributes(Attributes.Module_CapUsageRate) = attMod.CapUsageRate
                        End If
                    Case Attributes.Module_MiningAmount
                        Select Case attMod.MarketGroup
                            Case ShipModule.Marketgroup_IceHarvesters
                                attMod.Attributes(Attributes.Module_TurretIceMiningRate) = CDbl(attMod.Attributes(Attributes.Module_MiningAmount)) / CDbl(attMod.Attributes(Attributes.Module_ActivationTime))
                            Case ShipModule.Marketgroup_MiningLasers, ShipModule.Marketgroup_StripMiners
                                attMod.Attributes(Attributes.Module_TurretOreMiningRate) = CDbl(attMod.Attributes(Attributes.Module_MiningAmount)) / CDbl(attMod.Attributes(Attributes.Module_ActivationTime))
                            Case ShipModule.Marketgroup_MiningDrones
                                attMod.Attributes(Attributes.Module_DroneOreMiningRate) = CDbl(attMod.Attributes(Attributes.Module_MiningAmount)) / CDbl(attMod.Attributes(Attributes.Module_ActivationTime))
                        End Select
                    Case Attributes.Module_ChargeSize
                        attMod.ChargeSize = CInt(attValue)
                    Case Attributes.Module_CalibrationCost
                        attMod.Calibration = CInt(attValue)
                    Case Attributes.Module_ImplantSlot
                        attMod.ImplantSlot = CInt(attValue)
                    Case Attributes.Module_BoosterSlot
                        attMod.BoosterSlot = CInt(attValue)
                    Case Attributes.Module_ReqSkill1
                        If EveHQ.Core.HQ.SkillListID.ContainsKey(CStr(attValue)) = True Then
                            Dim pSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                            Dim nSkill As New ItemSkills
                            nSkill.ID = pSkill.ID
                            nSkill.Name = pSkill.Name
                            pSkillName = pSkill.Name
                            attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                        End If
                    Case Attributes.Module_ReqSkill2
                        If EveHQ.Core.HQ.SkillListID.ContainsKey(CStr(attValue)) = True Then
                            Dim sSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                            Dim nSkill As New ItemSkills
                            nSkill.ID = sSkill.ID
                            nSkill.Name = sSkill.Name
                            sSkillName = sSkill.Name
                            attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                        End If
                    Case Attributes.Module_ReqSkill3
                        If EveHQ.Core.HQ.SkillListID.ContainsKey(CStr(attValue)) = True Then
                            Dim tSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                            Dim nSkill As New ItemSkills
                            nSkill.ID = tSkill.ID
                            nSkill.Name = tSkill.Name
                            tSkillName = tSkill.Name
                            attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                        End If
                    Case Attributes.Module_ReqSkill1Level
                        Dim cSkill As ItemSkills = CType(attMod.RequiredSkills(pSkillName), ItemSkills)
                        If cSkill IsNot Nothing Then
                            cSkill.Level = CInt(attValue)
                        End If
                    Case Attributes.Module_ReqSkill2Level
                        Dim cSkill As ItemSkills = CType(attMod.RequiredSkills(sSkillName), ItemSkills)
                        If cSkill IsNot Nothing Then
                            cSkill.Level = CInt(attValue)
                        End If
                    Case Attributes.Module_ReqSkill3Level
                        Dim cSkill As ItemSkills = CType(attMod.RequiredSkills(tSkillName), ItemSkills)
                        If cSkill IsNot Nothing Then
                            cSkill.Level = CInt(attValue)
                        End If
                    Case Attributes.Module_ChargeGroup1, Attributes.Module_ChargeGroup2, Attributes.Module_ChargeGroup3, Attributes.Module_ChargeGroup4, Attributes.Module_ChargeGroup5
                        attMod.Charges.Add(CStr(attValue))
                    Case Attributes.Module_MetaLevel
                        attMod.MetaLevel = CInt(attValue)
                    Case Else
                End Select
                lastModName = modRow.Item("typeName").ToString
                ' Add to the ChargeGroups if it doesn't exist and chargesize <> 0
                'If attMod.IsCharge = True And Charges.ChargeGroups.Contains(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize) = False Then
                '    Charges.ChargeGroups.Add(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize)
                'End If
                'End If
            Next
            ' Build the metaType data
            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                If ModuleLists.moduleMetaGroups.Contains(cMod.ID) = True Then
                    If CStr(ModuleLists.moduleMetaGroups(cMod.ID)) = "0" Then
                        If cMod.Attributes.ContainsKey(Attributes.Module_TechLevel) = True Then
                            Select Case CInt(cMod.Attributes(Attributes.Module_TechLevel))
                                Case 1
                                    cMod.MetaType = MetaTypes.Tech1
                                Case 2
                                    cMod.MetaType = MetaTypes.Tech2
                                Case 3
                                    cMod.MetaType = MetaTypes.Tech3
                                Case Else
                                    cMod.MetaType = MetaTypes.Tech1
                            End Select
                        Else
                            cMod.MetaType = MetaTypes.Tech1
                        End If
                    Else
                        cMod.MetaType = CInt(2 ^ (CInt(ModuleLists.moduleMetaGroups(cMod.ID)) - 1))
                    End If
                Else
                    cMod.MetaType = MetaTypes.Tech1
                End If
            Next
            ' Build charge data
            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                If cMod.IsCharge = True Then
                    If Charges.ChargeGroups.Contains(cMod.MarketGroup) = False Then
                        Charges.ChargeGroups.Add(cMod.MarketGroup & "_" & cMod.DatabaseGroup & "_" & cMod.Name & "_" & cMod.ChargeSize)
                    End If
                End If
            Next
            ' Check for drone missiles
            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                If cMod.IsDrone = True And cMod.Attributes.ContainsKey(Attributes.Module_MissileTypeID) = True Then
                    Dim chg As ShipModule = CType(ModuleLists.moduleList(cMod.Attributes(Attributes.Module_MissileTypeID).ToString), ShipModule)
                    cMod.LoadedCharge = chg
                End If
            Next
            ' Build the implant data
            If Me.BuildImplantData = True Then
                Return True
            Else
                Return False
            End If
            Return True
        Catch e As Exception
            MessageBox.Show("Error building Module Attribute Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildImplantData() As Boolean
        Try
            ' Build the List of implants from the modules?
            For Each impMod As ShipModule In ModuleLists.moduleList.Values
                If impMod.IsImplant = True Then
                    Implants.implantList.Add(impMod.Name, impMod)
                End If
                If impMod.IsBooster = True Then
                    Boosters.BoosterList.Add(impMod.Name, impMod)
                End If
            Next
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            ' Extract the groups from the included resource file
            Dim implantsList() As String = My.Resources.ImplantEffects.Split(ControlChars.CrLf.ToCharArray)
            Dim implantData() As String
            Dim implantName As String = ""
            Dim implantBonus As String = ""
            Dim implantBonusValue As Double = 0
            Dim implantGroups As String = ""
            Dim implantGroup() As String
            For Each cImplant As String In implantsList
                If cImplant.Trim <> "" And cImplant.StartsWith("#") = False Then
                    implantData = cImplant.Split(",".ToCharArray)
                    implantName = implantData(10)
                    implantGroups = implantData(9)
                    implantGroup = implantGroups.Split(";".ToCharArray)
                    If Implants.implantList.ContainsKey(implantName) = True Then
                        Dim bImplant As ShipModule = CType(Implants.implantList(implantName), ShipModule)
                        For Each impGroup As String In implantGroup
                            bImplant.ImplantGroups.Add(impGroup)
                        Next
                    End If
                End If
            Next
            Return True
        Catch e As Exception
            MessageBox.Show("Error building Implant Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildAttributeQuickList() As Boolean
        Attributes.AttributeQuickList.Clear()
        Dim attData As New Attribute
        For Each att As String In Attributes.AttributeList.Keys
            attData = CType(Attributes.AttributeList(att), Attribute)
            If attData.DisplayName <> "" Then
                Attributes.AttributeQuickList.Add(attData.ID, attData.DisplayName)
            Else
                Attributes.AttributeQuickList.Add(attData.ID, attData.Name)
            End If
        Next
        Return True
    End Function

    Private Sub BuildModuleEffects()

        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.Effects.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        Dim newEffect As New Effect
        Dim IDs() As String
        Dim AffectingName As String = ""
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New Effect
                newEffect.AffectingAtt = CInt(EffectData(0))
                newEffect.AffectingType = CType(EffectData(1), EffectType)
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CType(EffectData(4), EffectType)
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CType(EffectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CType(EffectData(8), EffectCalcType)
                newEffect.Status = CInt(EffectData(9))

                Select Case newEffect.AffectingType
                    ' Setup the name as Item;Type;Attribute
                    Case EffectType.All
                        AffectingName = "Global;Global;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                    Case EffectType.Item
                        If newEffect.AffectingID > 0 Then
                            AffectingName = EveHQ.Core.HQ.itemData(newEffect.AffectingID.ToString).Name
                            If EveHQ.Core.HQ.SkillListName.ContainsKey(AffectingName) = True Then
                                AffectingName &= ";Skill;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                            Else
                                AffectingName &= ";Item;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                            End If
                        End If
                    Case EffectType.Group
                        AffectingName = EveHQ.Core.HQ.itemGroups(newEffect.AffectingID.ToString) & ";Group;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                    Case EffectType.Category
                        AffectingName = EveHQ.Core.HQ.itemCats(newEffect.AffectingID.ToString) & ";Category;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                    Case EffectType.MarketGroup
                        AffectingName = CStr(HQF.Market.MarketGroupList(newEffect.AffectingID.ToString)) & ";Market Group;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                End Select
                AffectingName &= ";"

                For Each cModule As ShipModule In ModuleLists.moduleList.Values
                    Select Case newEffect.AffectedType
                        Case EffectType.All
                            If newEffect.AffectingID <> 0 Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Item
                            If newEffect.AffectedID.Contains(cModule.ID) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Group
                            If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Category
                            If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.MarketGroup
                            If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Skill
                            If cModule.RequiredSkills.ContainsKey(EveHQ.Core.HQ.itemData(newEffect.AffectedID(0).ToString).Name) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Attribute
                            If cModule.Attributes.ContainsKey(newEffect.AffectedID(0)) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                    End Select
                Next

                ' Add the skills into the ship
                If newEffect.Status < 16 Then
                    If AffectingName.Contains(";Skill;") = True Then
                        For Each cShip As Ship In ShipLists.shipList.Values
                            Select Case newEffect.AffectedType
                                Case EffectType.All
                                    If newEffect.AffectingID <> 0 Then
                                        cShip.Affects.Add(AffectingName)
                                    End If
                                Case EffectType.Item
                                    If newEffect.AffectedID.Contains(cShip.ID) Then
                                        cShip.Affects.Add(AffectingName)
                                    End If
                                Case EffectType.Group
                                    If newEffect.AffectedID.Contains(cShip.DatabaseGroup) Then
                                        cShip.Affects.Add(AffectingName)
                                    End If
                                Case EffectType.Category
                                    If newEffect.AffectedID.Contains(cShip.DatabaseCategory) Then
                                        cShip.Affects.Add(AffectingName)
                                    End If
                                Case EffectType.MarketGroup
                                    If newEffect.AffectedID.Contains(cShip.MarketGroup) Then
                                        cShip.Affects.Add(AffectingName)
                                    End If
                                Case EffectType.Skill
                                    If cShip.RequiredSkills.ContainsKey(EveHQ.Core.HQ.itemData(newEffect.AffectedID(0).ToString).Name) Then
                                        cShip.Affects.Add(AffectingName)
                                    End If
                                Case EffectType.Attribute
                                    If cShip.Attributes.ContainsKey(newEffect.AffectedID(0)) Then
                                        cShip.Affects.Add(AffectingName)
                                    End If
                            End Select
                        Next
                    End If
                End If

            End If
        Next
    End Sub

    Private Sub BuildImplantEffects()

        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.ImplantEffects.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String
        Dim ImplantEffectClassList As New ArrayList
        Dim newEffect As New ImplantEffect
        Dim IDs() As String
        Dim AttIDs() As String
        Dim Atts As New ArrayList
        Dim AffectingName As String = ""
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                Atts.Clear()
                If EffectData(3).Contains(";") Then
                    AttIDs = EffectData(3).Split(";".ToCharArray)
                    For Each AttID As String In AttIDs
                        Atts.Add(AttID)
                    Next
                Else
                    Atts.Add(EffectData(3))
                End If
                For Each att As String In Atts
                    newEffect = New ImplantEffect
                    newEffect.ImplantName = CStr(EffectData(10))
                    newEffect.AffectingAtt = CInt(EffectData(0))
                    newEffect.AffectedAtt = CInt(att)
                    newEffect.AffectedType = CType(EffectData(4), EffectType)
                    If EffectData(5).Contains(";") = True Then
                        IDs = EffectData(5).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.AffectedID.Add(ID)
                        Next
                    Else
                        newEffect.AffectedID.Add(EffectData(5))
                    End If
                    newEffect.CalcType = CType(EffectData(6), EffectCalcType)
                    Dim cImplant As ShipModule = CType(Implants.implantList(newEffect.ImplantName), ShipModule)
                    newEffect.Value = CDbl(cImplant.Attributes(EffectData(0)))
                    newEffect.IsGang = CBool(EffectData(8))
                    If EffectData(9).Contains(";") = True Then
                        IDs = EffectData(9).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.Groups.Add(ID)
                        Next
                    Else
                        newEffect.Groups.Add(EffectData(9))
                    End If

                    AffectingName = EveHQ.Core.HQ.itemData(EffectData(2)).Name & ";Implant;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString & ";"

                    For Each cModule As ShipModule In ModuleLists.moduleList.Values
                        Select Case newEffect.AffectedType
                            Case EffectType.All
                                If CInt(EffectData(2)) <> 0 Then
                                    cModule.Affects.Add(AffectingName)
                                End If
                            Case EffectType.Item
                                If newEffect.AffectedID.Contains(cModule.ID) Then
                                    cModule.Affects.Add(AffectingName)
                                End If
                            Case EffectType.Group
                                If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                    cModule.Affects.Add(AffectingName)
                                End If
                            Case EffectType.Category
                                If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                    cModule.Affects.Add(AffectingName)
                                End If
                            Case EffectType.MarketGroup
                                If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                    cModule.Affects.Add(AffectingName)
                                End If
                            Case EffectType.Attribute
                                If cModule.Attributes.ContainsKey(newEffect.AffectedID(0).ToString) Then
                                    cModule.Affects.Add(AffectingName)
                                End If
                        End Select
                    Next
                Next
            End If
        Next
    End Sub

    Private Sub BuildShipEffects()

        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.ShipBonuses.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String

        Dim shipEffectClassList As New ArrayList
        Dim newEffect As New ShipEffect
        Dim IDs() As String
        Dim AffectingName As String = ""
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                newEffect.ShipID = CInt(EffectData(0))
                newEffect.AffectingType = CType(EffectData(1), EffectType)
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CType(EffectData(4), EffectType)
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CType(EffectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CType(EffectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(EffectData(9), Globalization.NumberStyles.Any, culture)
                newEffect.Status = CInt(EffectData(10))
                shipEffectClassList.Add(newEffect)

                AffectingName = EveHQ.Core.HQ.itemData(newEffect.ShipID.ToString).Name
                If newEffect.IsPerLevel = False Then
                    AffectingName &= ";Ship Role;"
                Else
                    AffectingName &= ";Ship Bonus;"
                End If
                AffectingName &= HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                If newEffect.IsPerLevel = False Then
                    AffectingName &= ";"
                Else
                    AffectingName &= ";" & EveHQ.Core.HQ.itemData(newEffect.AffectingID.ToString).Name
                End If

                ' Add the skills into the ship modules
                For Each cModule As ShipModule In ModuleLists.moduleList.Values
                    Select Case newEffect.AffectedType
                        Case EffectType.All
                            If newEffect.AffectingID <> 0 Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Item
                            If newEffect.AffectedID.Contains(cModule.ID) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Group
                            If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Category
                            If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.MarketGroup
                            If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Attribute
                            If cModule.Attributes.ContainsKey(newEffect.AffectedID(0).ToString) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                    End Select
                Next

                ' Add the skills into the ship global skills
                If newEffect.Status < 16 Then
                    For Each cShip As Ship In ShipLists.shipList.Values
                        If newEffect.ShipID = CDbl(cShip.ID) Then
                            cShip.GlobalAffects.Add(AffectingName)
                        End If
                    Next
                End If

            End If
        Next
    End Sub

    Private Sub BuildSubsystemEffects()
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

        ' Fetch the Effects list
        Dim EffectFile As String = My.Resources.Subsystems.ToString
        ' Break the Effects down into separate lines
        Dim EffectLines() As String = EffectFile.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim EffectData() As String

        Dim shipEffectClassList As New ArrayList
        Dim newEffect As New ShipEffect
        Dim IDs() As String
        Dim AffectingName As String = ""
        For Each EffectLine As String In EffectLines
            If EffectLine.Trim <> "" And EffectLine.StartsWith("#") = False Then
                EffectData = EffectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                newEffect.ShipID = CInt(EffectData(0))
                newEffect.AffectingType = CType(EffectData(1), EffectType)
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CType(EffectData(4), EffectType)
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CType(EffectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CType(EffectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(EffectData(9), Globalization.NumberStyles.Any, culture)
                newEffect.Status = CInt(EffectData(10))
                shipEffectClassList.Add(newEffect)

                AffectingName = EveHQ.Core.HQ.itemData(newEffect.ShipID.ToString).Name
                If newEffect.IsPerLevel = False Then
                    AffectingName &= ";Subsystem Role;"
                Else
                    AffectingName &= ";Subsystem;"
                End If
                AffectingName &= HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                If newEffect.IsPerLevel = False Then
                    AffectingName &= ";"
                Else
                    AffectingName &= ";" & EveHQ.Core.HQ.itemData(newEffect.AffectingID.ToString).Name
                End If

                For Each cModule As ShipModule In ModuleLists.moduleList.Values
                    Select Case newEffect.AffectedType
                        Case EffectType.All
                            If newEffect.AffectingID <> 0 Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Item
                            If newEffect.AffectedID.Contains(cModule.ID) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Group
                            If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Category
                            If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.MarketGroup
                            If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        Case EffectType.Attribute
                            If cModule.Attributes.ContainsKey(newEffect.AffectedID(0).ToString) Then
                                cModule.Affects.Add(AffectingName)
                            End If
                    End Select
                    ' Add the skill onto the subsystem
                    If newEffect.IsPerLevel = True Then
                        If cModule.ID = newEffect.ShipID.ToString Then
                            AffectingName = EveHQ.Core.HQ.itemData(newEffect.AffectingID.ToString).Name
                            AffectingName &= ";Skill;" & HQF.Attributes.AttributeQuickList(newEffect.AffectedAtt.ToString).ToString
                            If cModule.Affects.Contains(AffectingName) = False Then
                                cModule.Affects.Add(AffectingName)
                            End If
                        End If
                    End If
                Next

                ' Add the skills into the ship
                'If newEffect.Status < 16 Then
                '    If AffectingName.Contains(";Skill;") = True Then
                '        For Each cShip As Ship In ShipLists.shipList.Values
                '            Select Case newEffect.AffectedType
                '                Case EffectType.All
                '                    If newEffect.AffectingID <> 0 Then
                '                        cShip.Affects.Add(AffectingName)
                '                    End If
                '                Case EffectType.Item
                '                    If newEffect.AffectedID.Contains(cShip.ID) Then
                '                        cShip.Affects.Add(AffectingName)
                '                    End If
                '                Case EffectType.Group
                '                    If newEffect.AffectedID.Contains(cShip.DatabaseGroup) Then
                '                        cShip.Affects.Add(AffectingName)
                '                    End If
                '                Case EffectType.Category
                '                    If newEffect.AffectedID.Contains(cShip.DatabaseCategory) Then
                '                        cShip.Affects.Add(AffectingName)
                '                    End If
                '                Case EffectType.MarketGroup
                '                    If newEffect.AffectedID.Contains(cShip.MarketGroup) Then
                '                        cShip.Affects.Add(AffectingName)
                '                    End If
                '                Case EffectType.Attribute
                '                    If cShip.Attributes.Contains(newEffect.AffectedID(0).ToString) Then
                '                        cShip.Affects.Add(AffectingName)
                '                    End If
                '            End Select
                '        Next
                '    End If
                'End If

            End If
        Next
    End Sub

    Private Sub SaveHQFCacheData()
        ' Delete the cache folder if it's already there
        If My.Computer.FileSystem.DirectoryExists(Settings.HQFCacheFolder) = True Then
            My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        My.Computer.FileSystem.CreateDirectory(Settings.HQFCacheFolder)
        ' Save ships
        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "ships.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, ShipLists.shipList)
        s.Flush()
        s.Close()
        ' Save modules
        s = New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "modules.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, ModuleLists.moduleList)
        s.Flush()
        s.Close()
        ' Save implants
        s = New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "implants.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Implants.implantList)
        s.Flush()
        s.Close()
        ' Save boosters
        s = New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "boosters.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Boosters.BoosterList)
        s.Flush()
        s.Close()
        ' Save skills
        s = New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "skills.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, SkillLists.SkillList)
        s.Flush()
        s.Close()
        ' Save attributes
        s = New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "attributes.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Attributes.AttributeList)
        s.Flush()
        s.Close()
        ' Save NPCs
        s = New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "NPCs.bin"), FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, NPCs.NPCList)
        s.Flush()
        s.Close()

        ' Build and write the Ship Market Group Data
        Call Me.BuildShipMarketGroups()

        ' Build and write the Item Market Group Data
        Call Me.BuildItemMarketGroups()

        ' Write the current version
        Dim sw As New StreamWriter(Path.Combine(HQF.Settings.HQFCacheFolder, "version.txt"))
        sw.Write(PlugInData.LastCacheRefresh)
        sw.Flush()
        sw.Close()

    End Sub

    Private Sub CleanUpData()
        MarketGroupData = Nothing
        shipGroupData = Nothing
        shipNameData = Nothing
        moduleData = Nothing
        moduleEffectData = Nothing
        moduleAttributeData = Nothing
        GC.Collect()
    End Sub

#End Region

#Region "Ship and Item Market Group Cache Building Routines"
    Private Sub BuildShipMarketGroups()
        Dim tvwShips As New TreeView
        tvwShips.BeginUpdate()
        tvwShips.Nodes.Clear()
        Dim marketTable As DataTable = PlugInData.MarketGroupData.Tables(0)
        Dim rootRows() As DataRow = marketTable.Select("ISNULL(parentGroupID, 0) = 0")
        For Each rootRow As DataRow In rootRows
            Dim rootNode As New TreeNode(CStr(rootRow.Item("marketGroupName")))
            Call PopulateShipGroups(CInt(rootRow.Item("marketGroupID")), rootNode, marketTable)
            Select Case rootNode.Text
                Case "Ships"
                    For Each childNode As TreeNode In rootNode.Nodes
                        tvwShips.Nodes.Add(childNode)
                    Next
            End Select
        Next
        ' Now check for Faction ships
        Dim shipGroup As String = ""
        Dim factionRows() As DataRow = PlugInData.shipNameData.Tables(0).Select("ISNULL(marketGroupID, 0) = 0")
        For Each factionRow As DataRow In factionRows
            shipGroup = factionRow.Item("groupName").ToString & "s"
            For Each groupNode As TreeNode In tvwShips.Nodes
                If groupNode.Text = shipGroup Then
                    ' Check for "Faction" node
                    If groupNode.Nodes.ContainsKey("Faction") = False Then
                        groupNode.Nodes.Add("Faction", "Faction")
                    End If
                    ' Add to the "Faction" node
                    groupNode.Nodes("Faction").Nodes.Add(factionRow.Item("typeName").ToString)
                End If
            Next
        Next
        tvwShips.Sorted = True
        tvwShips.EndUpdate()
        Call Me.WriteShipGroups(tvwShips)
        tvwShips.Dispose()
    End Sub
    Private Sub BuildItemMarketGroups()
        Dim tvwItems As New TreeView
        tvwItems.BeginUpdate()
        tvwItems.Nodes.Clear()
        Dim marketTable As DataTable = PlugInData.MarketGroupData.Tables(0)
        Dim rootRows() As DataRow = marketTable.Select("ISNULL(parentGroupID, 0) = 0")
        For Each rootRow As DataRow In rootRows
            Dim rootNode As New TreeNode(CStr(rootRow.Item("marketGroupName")))
            rootNode.Name = rootNode.Text
            Call PopulateModuleGroups(CInt(rootRow.Item("marketGroupID")), rootNode, marketTable)
            Select Case rootNode.Text
                Case "Ship Equipment", "Ammunition & Charges", "Drones", "Ship Modifications", "Implants & Boosters"
                    tvwItems.Nodes.Add(rootNode)
            End Select
        Next
        tvwItems.Sorted = True
        tvwItems.Sorted = False
        ' Add the Favourties Node
        Dim FavNode As New TreeNode("Favourites")
        FavNode.Name = "Favourites"
        FavNode.Tag = "Favourites"
        tvwItems.Nodes.Add(FavNode)
        ' Add the Favourties Node
        Dim MRUNode As New TreeNode("Recently Used")
        MRUNode.Name = "Recently Used"
        MRUNode.Tag = "Recently Used"
        tvwItems.Nodes.Add(MRUNode)
        tvwItems.EndUpdate()
        Market.MarketGroupPath.Clear()
        Call BuildTreePathData(tvwItems)
        Call Me.WriteItemGroups(tvwItems)
        tvwItems.Dispose()
    End Sub

    Private Sub PopulateShipGroups(ByVal inParentID As Integer, ByRef inTreeNode As TreeNode, ByVal marketTable As DataTable)
        Dim ParentRows() As DataRow = marketTable.Select("parentGroupID=" & inParentID)
        For Each ParentRow As DataRow In ParentRows
            Dim parentnode As TreeNode
            parentnode = New TreeNode(CStr(ParentRow.Item("marketGroupName")))
            inTreeNode.Nodes.Add(parentnode)
            parentnode.Tag = ParentRow.Item("marketGroupID")
            PopulateShipGroups(CInt(parentnode.Tag), parentnode, marketTable)
        Next ParentRow
        Dim groupRows() As DataRow = PlugInData.shipNameData.Tables(0).Select("marketGroupID=" & inParentID)
        For Each shipRow As DataRow In groupRows
            inTreeNode.Nodes.Add(shipRow.Item("typeName").ToString)
        Next
    End Sub
    Private Sub PopulateModuleGroups(ByVal inParentID As Integer, ByRef inTreeNode As TreeNode, ByVal marketTable As DataTable)
        Dim ParentRows() As DataRow = marketTable.Select("parentGroupID=" & inParentID)
        For Each ParentRow As DataRow In ParentRows
            Dim parentnode As TreeNode
            parentnode = New TreeNode(CStr(ParentRow.Item("marketGroupName")))
            parentnode.Name = parentnode.Text
            inTreeNode.Nodes.Add(parentnode)
            parentnode.Tag = ParentRow.Item("marketGroupID")
            PopulateModuleGroups(CInt(parentnode.Tag), parentnode, marketTable)
        Next ParentRow
    End Sub
    Private Sub BuildTreePathData(ByVal tvwItems As TreeView)
        For Each rootNode As TreeNode In tvwItems.Nodes
            BuildTreePathData2(rootNode)
        Next
    End Sub
    Private Sub BuildTreePathData2(ByRef parentNode As TreeNode)
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                BuildTreePathData2(childNode)
            Else
                Market.MarketGroupPath.Add(childNode.Tag.ToString, childNode.FullPath)
            End If
        Next
    End Sub
    Private Sub WriteShipGroups(ByVal tvwShips As TreeView)
        Dim sw As New IO.StreamWriter(Path.Combine(HQF.Settings.HQFCacheFolder, "ShipGroups.bin"))
        For Each rootNode As TreeNode In tvwShips.Nodes
            WriteShipNodes(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteItemGroups(ByVal tvwItems As TreeView)
        Dim sw As New IO.StreamWriter(Path.Combine(HQF.Settings.HQFCacheFolder, "ItemGroups.bin"))
        For Each rootNode As TreeNode In tvwItems.Nodes
            WriteGroupNodes(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteShipNodes(ByRef parentNode As TreeNode, ByVal sw As IO.StreamWriter)
        sw.Write(parentNode.FullPath & ControlChars.CrLf)
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                WriteShipNodes(childNode, sw)
            Else
                sw.Write(childNode.FullPath & ControlChars.CrLf)
            End If
        Next
    End Sub
    Private Sub WriteGroupNodes(ByRef parentNode As TreeNode, ByVal sw As IO.StreamWriter)
        sw.Write("0," & parentNode.FullPath & ControlChars.CrLf)
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                WriteGroupNodes(childNode, sw)
            Else
                sw.Write(childNode.Tag.ToString & "," & childNode.FullPath & ControlChars.CrLf)
            End If
        Next
    End Sub
#End Region

#Region "Fitting Link Parser"
    'fitting://evehq/28710:2032*1:2420*4:15681*4:15905*1:17498*2:19191*1:19814*2:24348*3:26416*1:26418*1
    '?sourceURL=http://eve.battleclinic.com/loadout/21813-Golem-that-actually-has-TP-039-s.html

    Private Function ParseFittingLink(ByVal DNA As String) As DNAFitting
        Dim ShipDNA As New DNAFitting
        DNA = DNA.TrimStart("fitting://".ToCharArray).Trim("/".ToCharArray)
        ' Remove the application name
        Dim appSep As Integer = DNA.IndexOf("/")
        Dim app As String = DNA.Substring(0, appSep)
        DNA = DNA.Remove(0, appSep + 1)

        ' Remove any query string to analyse later
        Dim parts() As String = DNA.Split("?".ToCharArray)
        Dim mods() As String = parts(0).Split(":".ToCharArray)

        ShipDNA.ShipID = mods(0)
        For modNo As Integer = 1 To mods.Length - 1
            Dim modData() As String = mods(modNo).Split("*".ToCharArray)
            If modData.Length > 1 Then
                For modCount As Integer = 1 To CInt(modData(1))
                    If ModuleLists.moduleList.ContainsKey(modData(0)) = True Then
                        Dim fModule As ShipModule = CType(ModuleLists.moduleList(modData(0)), ShipModule)
                        If fModule.IsCharge Then
                            ShipDNA.Charges.Add(fModule.ID)
                        Else
                            ShipDNA.Modules.Add(fModule.ID)
                        End If
                    End If
                Next
            Else
                If ModuleLists.moduleList.ContainsKey(modData(0)) = True Then
                    Dim fModule As ShipModule = CType(ModuleLists.moduleList(modData(0)), ShipModule)
                    If fModule.IsCharge Then
                        ShipDNA.Charges.Add(fModule.ID)
                    Else
                        ShipDNA.Modules.Add(fModule.ID)
                    End If
                End If
            End If
        Next

        If parts.Length > 1 Then
            Dim args() As String = parts(1).Split("&".ToCharArray)
            For Each arg As String In args
                Dim argData() As String = arg.Split("=".ToCharArray)
                ShipDNA.Arguments.Add(argData(0), argData(1))
            Next
        End If

        Return ShipDNA
    End Function
#End Region

End Class
