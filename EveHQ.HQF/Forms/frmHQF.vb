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
Imports System.Windows.Forms
Imports System.Drawing
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Xml

Public Class frmHQF
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object
    Dim dataCheckList As New SortedList
    Dim currentShipSlot As ShipSlotControl
    Dim currentShipInfo As ShipInfoControl
    Shared LastSlotFitting As New ArrayList
    Shared LastModuleResults As New SortedList
    Dim cacheForm As New frmHQFCacheWriter

#Region "Class Wide Variables"

    Dim itemCount As Integer = 0
    Dim startUp As Boolean = False

    Shared cModuleDisplay As String = ""
    Public Shared Property ModuleDisplay() As String
        Get
            Return cModuleDisplay
        End Get
        Set(ByVal value As String)
            cModuleDisplay = value
        End Set
    End Property

#End Region

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
            ' Check for existance of HQF folder & create if not existing
            If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
                Settings.HQFFolder = (EveHQ.Core.HQ.appDataFolder & "\HQF").Replace("\\", "\")
            Else
                Settings.HQFFolder = (Application.StartupPath & "\HQF").Replace("\\", "\")
            End If
            If My.Computer.FileSystem.DirectoryExists(Settings.HQFFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(Settings.HQFFolder)
            End If

            ' Check for cache folder
            Settings.HQFCacheFolder = Settings.HQFFolder & "\Cache"
            If My.Computer.FileSystem.DirectoryExists(Settings.HQFCacheFolder) = True Then
                ' Check for last cache version file
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\version.txt") = True Then
                    Dim sr As New StreamReader(HQF.Settings.HQFCacheFolder & "\version.txt")
                    Dim cacheVersion As String = sr.ReadToEnd
                    sr.Close()
                    If IsUpdateAvailable(cacheVersion, HQFData.LastCacheRefresh) = True Then
                        ' Delete the existing cache folder and force a rebuild
                        My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                        HQFData.UseSerializableData = False
                    Else
                        HQFData.UseSerializableData = True
                    End If
                Else
                    ' Delete the existing cache folder and force a rebuild
                    My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    HQFData.UseSerializableData = False
                End If
            Else
                HQFData.UseSerializableData = False
            End If

            Engine.BuildPirateImplants()
            Engine.BuildEffectsMap()
            Engine.BuildShipEffectsMap()
            ' Check for the existence of the binary data
            If HQFData.UseSerializableData = True Then
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\attributes.bin") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\attributes.bin", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    Attributes.AttributeList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\ships.bin") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\ships.bin", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    ShipLists.shipList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                    For Each cShip As Ship In ShipLists.shipList.Values
                        ShipLists.shipListKeyID.Add(cShip.ID, cShip.Name)
                        ShipLists.shipListKeyID.Add(cShip.Name, cShip.ID)
                    Next
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\modules.bin") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\modules.bin", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    ModuleLists.moduleList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                    For Each cMod As ShipModule In ModuleLists.moduleList.Values
                        ModuleLists.moduleListName.Add(cMod.Name, cMod.ID)
                        If cMod.IsCharge = True Then
                            If Charges.ChargeGroups.Contains(cMod.MarketGroup) = False Then
                                Charges.ChargeGroups.Add(cMod.MarketGroup & "_" & cMod.DatabaseGroup & "_" & cMod.Name & "_" & cMod.ChargeSize)
                            End If
                        End If
                    Next
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\implants.bin") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\implants.bin", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    Implants.implantList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\skills.bin") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\skills.bin", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    SkillLists.SkillList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\NPCs.bin") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\NPCs.bin", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    NPCs.NPCList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                Call Me.BuildAttributeQuickList()
                Engine.BuildImplantEffectsMap()
                Return True
            Else
                ' Populate the Ship data
                If Me.LoadAttributes = True Then
                    If Me.LoadSkillData = True Then
                        If Me.LoadShipGroupData = True Then
                            If Me.LoadMarketGroupData = True Then
                                If Me.LoadShipNameData = True Then
                                    If Me.LoadShipAttributeData = True Then
                                        Call Me.PopulateShipLists()
                                        If Me.LoadModuleData = True Then
                                            If Me.LoadModuleEffectData = True Then
                                                If Me.LoadModuleAttributeData = True Then
                                                    If Me.LoadModuleMetaTypes = True Then
                                                        If Me.BuildModuleData = True Then
                                                            If Me.LoadNPCData = True Then
                                                                Call Me.BuildAttributeQuickList()
                                                                Engine.BuildImplantEffectsMap()
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
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
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
        Return Me
    End Function
#End Region

#Region "Plug-In Initialisation (Data Loading/Building) Routines"

    ' Market Loading Routines & Bonuses
    Private Function LoadMarketGroupData() As Boolean
        Try
            Call Me.LoadAttributes()
            Dim strSQL As String = ""
            strSQL &= "SELECT * FROM invMarketGroups ORDER BY parentGroupID;"
            HQFData.MarketGroupData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If HQFData.MarketGroupData IsNot Nothing Then
                If HQFData.MarketGroupData.Tables(0).Rows.Count <> 0 Then
                    Market.MarketGroupList.Clear()
                    For Each row As DataRow In HQFData.MarketGroupData.Tables(0).Rows
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
            MessageBox.Show("Error loading Market Group Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadAttributes() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT dgmAttributeTypes.attributeID, dgmAttributeTypes.attributeName, dgmAttributeTypes.graphicID, dgmAttributeTypes.displayName AS dgmAttributeTypes_displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, dgmAttributeTypes.attributeGroup, eveUnits.unitName, eveUnits.displayName AS eveUnits_displayName"
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
                        attData.GraphicID = row.Item("graphicID").ToString
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
            MessageBox.Show("Error loading Attribute Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                attData.GraphicID = att(3)
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
            MessageBox.Show("Error loading Skill Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' Ship Loading Routines
    Private Function LoadShipGroupData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT * FROM invGroups WHERE invGroups.categoryID=6 ORDER BY groupName;"
            HQFData.shipGroupData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If HQFData.shipGroupData IsNot Nothing Then
                If HQFData.shipGroupData.Tables(0).Rows.Count <> 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Group Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadShipNameData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.published, invTypes.raceID, invTypes.marketGroupID"
            strSQL &= " FROM (invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID"
            strSQL &= " WHERE (invCategories.categoryID=6 AND invTypes.published=true) ORDER BY typeName;"
            HQFData.shipNameData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If HQFData.shipNameData IsNot Nothing Then
                If HQFData.shipNameData.Tables(0).Rows.Count <> 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadShipAttributeData() As Boolean
        Try
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            ' Get details of ship data from database
            Dim strSQL As String = ""
            Dim pSkillName As String = "" : Dim sSkillName As String = "" : Dim tSkillName As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
            strSQL &= " WHERE (invCategories.categoryID=6 AND invTypes.published=true) ORDER BY typeName, attributeID;"
            Dim shipData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If shipData IsNot Nothing Then
                If shipData.Tables(0).Rows.Count <> 0 Then
                    Dim lastShipName As String = ""
                    Dim newShip As New EveHQ.HQF.Ship
                    pSkillName = "" : sSkillName = "" : tSkillName = ""
                    Dim attValue As Double = 0
                    For Each shipRow As DataRow In shipData.Tables(0).Rows
                        ' If the shipName has changed, we need to start a new ship type
                        If lastShipName <> shipRow.Item("typeName").ToString Then
                            ' Add the current ship to the list then reset the ship data
                            If lastShipName <> "" Then
                                ' Add the custom attributes to the list
                                newShip.Attributes.Add("10001", newShip.Radius)
                                newShip.Attributes.Add("10002", newShip.Mass)
                                newShip.Attributes.Add("10003", newShip.Volume)
                                newShip.Attributes.Add("10004", newShip.CargoBay)
                                newShip.Attributes.Add("10005", 0)
                                newShip.Attributes.Add("10006", 0)
                                newShip.Attributes.Add("10007", 20000)
                                newShip.Attributes.Add("10008", 20000)
                                newShip.Attributes.Add("10009", 1)
                                newShip.Attributes.Add("10010", 0)
                                newShip.Attributes.Add("10020", 0)
                                newShip.Attributes.Add("10021", 0)
                                newShip.Attributes.Add("10022", 0)
                                newShip.Attributes.Add("10023", 0)
                                newShip.Attributes.Add("10024", 0)
                                newShip.Attributes.Add("10025", 0)
                                newShip.Attributes.Add("10026", 0)
                                newShip.Attributes.Add("10027", 0)
                                newShip.Attributes.Add("10028", 0)
                                newShip.Attributes.Add("10029", 0)
                                newShip.Attributes.Add("10031", 0)
                                newShip.Attributes.Add("10033", 0)
                                newShip.Attributes.Add("10034", 0)
                                newShip.Attributes.Add("10035", 0)
                                newShip.Attributes.Add("10036", 0)
                                newShip.Attributes.Add("10037", 0)
                                newShip.Attributes.Add("10038", 0)
                                newShip.Attributes.Add("10043", 0)
                                newShip.Attributes.Add("10044", 0)
                                newShip.Attributes.Add("10045", 0)
                                newShip.Attributes.Add("10046", 0)
                                newShip.Attributes.Add("10047", 0)
                                newShip.Attributes.Add("10048", 0)
                                newShip.Attributes.Add("10049", 0)
                                newShip.Attributes.Add("10050", 0)
                                newShip.Attributes.Add("10055", 0)
                                newShip.Attributes.Add("10056", 0)
                                newShip.Attributes.Add("10057", 0)
                                newShip.Attributes.Add("10058", 0)
                                newShip.Attributes.Add("10059", 0)
                                newShip.Attributes.Add("10060", 0)
                                newShip.Attributes.Add("10061", 0)
                                newShip.Attributes.Add("10062", 0)
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
                            newShip.Radius = CDbl(shipRow.Item("radius"))
                            newShip.Mass = CDbl(shipRow.Item("mass"))
                            newShip.Volume = CDbl(shipRow.Item("volume"))
                            newShip.CargoBay = CDbl(shipRow.Item("capacity"))
                        End If
                        ' Now get, modify (if applicable) and add the "attribute"

                        If IsDBNull(shipRow.Item("valueInt")) = True Then
                            attValue = CDbl(shipRow.Item("valueFloat"))
                        Else
                            attValue = CDbl(shipRow.Item("valueInt"))
                        End If

                        ' Do attribute (unit) modifiers
                        Select Case CInt(shipRow.Item("attributeID"))
                            Case 55, 479
                                attValue = attValue / 1000
                            Case 113, 111, 109, 110, 267, 268, 269, 270, 271, 272, 273, 274
                                attValue = (1 - attValue) * 100
                            Case 1281
                                attValue = attValue * 3
                            Case 1154 ' Reset this field to be used as Calibration_Used
                                attValue = 0
                        End Select

                        ' Add the attribute to the ship.attributes list
                        newShip.Attributes.Add(shipRow.Item("attributeID").ToString, attValue)

                        ' Map only the skill attributes
                        Select Case CInt(shipRow.Item("attributeID"))
                            Case 182
                                Dim pSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(CStr(attValue)), Core.SkillList)
                                Dim nSkill As New ItemSkills
                                nSkill.ID = pSkill.ID
                                nSkill.Name = pSkill.Name
                                pSkillName = pSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 183
                                Dim sSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(CStr(attValue)), Core.SkillList)
                                Dim nSkill As New ItemSkills
                                nSkill.ID = sSkill.ID
                                nSkill.Name = sSkill.Name
                                sSkillName = sSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 184
                                Dim tSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(CStr(attValue)), Core.SkillList)
                                Dim nSkill As New ItemSkills
                                nSkill.ID = tSkill.ID
                                nSkill.Name = tSkill.Name
                                tSkillName = tSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 277
                                Dim cSkill As ItemSkills = CType(newShip.RequiredSkills(pSkillName), ItemSkills)
                                cSkill.Level = CInt(attValue)
                            Case 278
                                Dim cSkill As ItemSkills = CType(newShip.RequiredSkills(sSkillName), ItemSkills)
                                cSkill.Level = CInt(attValue)
                            Case 279
                                Dim cSkill As ItemSkills = CType(newShip.RequiredSkills(tSkillName), ItemSkills)
                                cSkill.Level = CInt(attValue)
                        End Select
                        lastShipName = shipRow.Item("typeName").ToString
                    Next
                    ' Add the custom attributes to the list
                    newShip.Attributes.Add("10001", newShip.Radius)
                    newShip.Attributes.Add("10002", newShip.Mass)
                    newShip.Attributes.Add("10003", newShip.Volume)
                    newShip.Attributes.Add("10004", newShip.CargoBay)
                    newShip.Attributes.Add("10005", 0)
                    newShip.Attributes.Add("10006", 0)
                    newShip.Attributes.Add("10007", 20000)
                    newShip.Attributes.Add("10008", 20000)
                    newShip.Attributes.Add("10009", 1)
                    newShip.Attributes.Add("10020", 0)
                    newShip.Attributes.Add("10021", 0)
                    newShip.Attributes.Add("10022", 0)
                    newShip.Attributes.Add("10023", 0)
                    newShip.Attributes.Add("10024", 0)
                    newShip.Attributes.Add("10025", 0)
                    newShip.Attributes.Add("10026", 0)
                    newShip.Attributes.Add("10027", 0)
                    newShip.Attributes.Add("10028", 0)
                    newShip.Attributes.Add("10029", 0)
                    newShip.Attributes.Add("10031", 0)
                    newShip.Attributes.Add("10033", 0)
                    newShip.Attributes.Add("10034", 0)
                    newShip.Attributes.Add("10035", 0)
                    newShip.Attributes.Add("10036", 0)
                    newShip.Attributes.Add("10037", 0)
                    newShip.Attributes.Add("10038", 0)
                    newShip.Attributes.Add("10043", 0)
                    newShip.Attributes.Add("10044", 0)
                    newShip.Attributes.Add("10045", 0)
                    newShip.Attributes.Add("10046", 0)
                    newShip.Attributes.Add("10047", 0)
                    newShip.Attributes.Add("10048", 0)
                    newShip.Attributes.Add("10049", 0)
                    newShip.Attributes.Add("10050", 0)
                    newShip.Attributes.Add("10055", 0)
                    newShip.Attributes.Add("10056", 0)
                    newShip.Attributes.Add("10057", 0)
                    newShip.Attributes.Add("10058", 0)
                    newShip.Attributes.Add("10059", 0)
                    newShip.Attributes.Add("10060", 0)
                    newShip.Attributes.Add("10061", 0)
                    newShip.Attributes.Add("10062", 0)
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
            MessageBox.Show("Error loading Ship Attribute Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Sub PopulateShipLists()
        ShipLists.shipListKeyName.Clear()
        ShipLists.shipListKeyID.Clear()
        For Each shipRow As DataRow In HQFData.shipNameData.Tables(0).Rows
            ShipLists.shipListKeyName.Add(CStr(shipRow.Item("typeName")), CStr(shipRow.Item("typeID")))
            ShipLists.shipListKeyID.Add(CStr(shipRow.Item("typeID")), CStr(shipRow.Item("typeName")))
        Next
    End Sub

    ' Module Loading Routines
    Private Function LoadModuleData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, eveGraphics.icon"
            strSQL &= " FROM eveGraphics INNER JOIN (invCategories INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invCategories.categoryID = invGroups.categoryID) ON (eveGraphics.graphicID = invTypes.graphicID)"
            strSQL &= " WHERE (((invCategories.categoryID) In (7,8,18,20)) AND ((invTypes.published)=1))"
            strSQL &= " ORDER BY invTypes.typeName;"
            HQFData.moduleData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If HQFData.moduleData IsNot Nothing Then
                If HQFData.moduleData.Tables(0).Rows.Count <> 0 Then
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
            MessageBox.Show("Error loading Module Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadModuleEffectData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeEffects.effectID"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeEffects ON invTypes.typeID=dgmTypeEffects.typeID"
            strSQL &= " WHERE(invCategories.categoryID In (7,8,18,20) And invTypes.published=true)"
            strSQL &= " ORDER BY typeName, effectID;"
            HQFData.moduleEffectData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If HQFData.moduleEffectData IsNot Nothing Then
                If HQFData.moduleEffectData.Tables(0).Rows.Count <> 0 Then
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
            MessageBox.Show("Error loading Module Effect Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadModuleAttributeData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName"
            strSQL &= " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID"
            strSQL &= " WHERE (((invCategories.categoryID) In (7,8,18,20)) AND ((invTypes.published)=1))"
            strSQL &= " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;"

            HQFData.moduleAttributeData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If HQFData.moduleAttributeData IsNot Nothing Then
                If HQFData.moduleAttributeData.Tables(0).Rows.Count <> 0 Then
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
            MessageBox.Show("Error loading Module Attribute Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadModuleMetaTypes() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invMetaTypes.parentTypeID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID"
            strSQL &= " FROM (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (invMetaGroups INNER JOIN invMetaTypes ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID) ON invTypes.typeID = invMetaTypes.typeID"
            strSQL &= " WHERE (((invGroups.categoryID) In (7,8,18,20)) AND (invTypes.published=true));"
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
                            ModuleLists.moduleMetaGroups.Add(row.Item("parentTypeID").ToString, "1")
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
            MessageBox.Show("Error loading Module Metatype Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildModuleData() As Boolean
        Try
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            ModuleLists.moduleList.Clear()
            Implants.implantList.Clear()
            For Each row As DataRow In HQFData.moduleData.Tables(0).Rows
                Dim newModule As New ShipModule
                newModule.ID = row.Item("typeID").ToString
                newModule.Name = row.Item("typeName").ToString
                newModule.Description = row.Item("description").ToString
                newModule.DatabaseGroup = row.Item("groupID").ToString
                newModule.DatabaseCategory = row.Item("categoryID").ToString
                newModule.BasePrice = CDbl(row.Item("baseprice"))
                newModule.Volume = CDbl(row.Item("volume"))
                newModule.Capacity = CDbl(row.Item("capacity"))
                newModule.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(newModule.ID)
                newModule.Icon = row.Item("icon").ToString
                If IsDBNull(row.Item("marketGroupID")) = False Then
                    newModule.MarketGroup = row.Item("marketGroupID").ToString
                Else
                    newModule.MarketGroup = "0"
                End If
                If ModuleLists.moduleMetaGroups.Contains(newModule.ID) = True Then
                    newModule.MetaType = CInt(2 ^ (CInt(ModuleLists.moduleMetaGroups(newModule.ID)) - 1))
                Else
                    newModule.MetaType = 1
                End If
                newModule.CPU = 0
                newModule.PG = 0
                newModule.Calibration = 0
                newModule.CapUsage = 0
                newModule.ActivationTime = 0
                ModuleLists.moduleList.Add(newModule.ID, newModule)
                ModuleLists.moduleListName.Add(newModule.Name, newModule.ID)

                ' Determine whether implant, drone, charge etc
                Select Case CInt(row.Item("categoryID"))
                    Case 8 ' Charge
                        newModule.IsCharge = True
                    Case 18 ' Drone
                        newModule.IsDrone = True
                    Case 20 ' Implant
                        If CInt(row.Item("groupID")) <> 304 Then
                            If CInt(row.Item("groupID")) = 303 Then
                                newModule.IsBooster = True
                            Else
                                newModule.IsImplant = True
                            End If
                        End If

                        ' Exclude groups 303 (Boosters) & 304 (DNA Mutators)
                        'If CInt(row.Item("groupID")) <> 303 And CInt(row.Item("groupID")) <> 304 Then
                        '    Dim newImplant As New Implant
                        '    newImplant.ID = newModule.ID
                        '    newImplant.Name = newModule.Name
                        '    newImplant.Description = newModule.Description
                        '    newImplant.DatabaseGroup = newModule.DatabaseGroup
                        '    newImplant.BasePrice = newModule.BasePrice
                        '    newImplant.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(newImplant.ID)
                        '    newImplant.MarketGroup = newModule.MarketGroup
                        '    newImplant.MetaType = newModule.MetaType
                        '    Implants.implantList.Add(newImplant.Name, newImplant)
                        'End If
                End Select
            Next

            ' Fill in the blank market groups now the list is complete
            Dim modName As String = ""
            Dim modID As String = ""
            Dim parentID As String = ""
            Dim nModule As New ShipModule
            Dim eModule As New ShipModule
            For setNo As Integer = 0 To 1
                For Each row As DataRow In HQFData.moduleData.Tables(0).Rows
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
            MessageBox.Show("Error building Module Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildModuleEffectData() As Boolean
        Try
            ' Get details of module attributes from already retrieved dataset
            Dim attValue As Double = 0
            For Each modRow As DataRow In HQFData.moduleEffectData.Tables(0).Rows
                Dim effMod As ShipModule = CType(ModuleLists.moduleList.Item(modRow.Item("typeID").ToString), ShipModule)
                Select Case CInt(modRow.Item("effectID"))
                    Case 11 ' Low slot
                        effMod.SlotType = 2
                    Case 12 ' High slot
                        effMod.SlotType = 8
                    Case 13 ' Mid slot
                        effMod.SlotType = 4
                    Case 2663 ' Rig slot
                        effMod.SlotType = 1
                    Case 101
                        effMod.IsLauncher = True
                    Case 10, 34
                        effMod.IsTurret = True
                End Select
                ' Add custom attributes
                If effMod.IsDrone = True Or effMod.IsLauncher = True Or effMod.IsTurret = True Or effMod.DatabaseGroup = "72" Then
                    If effMod.Attributes.Contains("10017") = False Then
                        effMod.Attributes.Add("10017", 0)
                        effMod.Attributes.Add("10018", 0)
                        effMod.Attributes.Add("10019", 0)
                        effMod.Attributes.Add("10030", 0)
                        effMod.Attributes.Add("10051", 0)
                        effMod.Attributes.Add("10052", 0)
                        effMod.Attributes.Add("10053", 0)
                        effMod.Attributes.Add("10054", 0)
                    End If
                End If
                Select Case CInt(effMod.MarketGroup)
                    Case 1038 ' Ice Miners
                        If effMod.Attributes.Contains("10041") = False Then
                            effMod.Attributes.Add("10041", 0)
                        End If
                    Case 1039, 1040 ' Ore Miners
                        If effMod.Attributes.Contains("10039") = False Then
                            effMod.Attributes.Add("10039", 0)
                        End If
                    Case 158 ' Mining Drones
                        If effMod.Attributes.Contains("10040") = False Then
                            effMod.Attributes.Add("10040", 0)
                        End If
                End Select
                Select Case CInt(effMod.DatabaseGroup)
                    Case 76
                        If effMod.Attributes.Contains("6") = False Then
                            effMod.Attributes.Add("6", 0)
                        End If
                End Select
            Next
            If BuildModuleAttributeData() = True Then
                Return True
            Else
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error building Module Effect Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function BuildModuleAttributeData() As Boolean
        Try
            ' Get details of module attributes from already retrieved dataset
            Dim attValue As Double = 0
            Dim pSkillName As String = "" : Dim sSkillName As String = "" : Dim tSkillName As String = ""
            Dim lastModName As String = ""
            For Each modRow As DataRow In HQFData.moduleAttributeData.Tables(0).Rows
                Dim attMod As ShipModule = CType(ModuleLists.moduleList.Item(modRow.Item("typeID").ToString), ShipModule)
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
                    Case "108"
                        attValue = Math.Round(100 - (attValue * 100), 2)
                    Case "109"
                        attValue = Math.Round((attValue * 100) - 100, 2)
                    Case "111"
                        attValue = Math.Round((attValue - 1) * 100, 2)
                    Case "101"      ' If unit is "ms"
                        If attValue > 1000 Then
                            attValue = Math.Round(attValue / 1000, 2)
                        End If
                End Select

                ' Modify the attribute value if we using damage controls - this is to stack up later on
                If CInt(attMod.DatabaseGroup) = 60 Then
                    Select Case CInt(modRow.Item("attributeID"))
                        Case 267, 268, 269, 270, 271, 272, 273, 274, 974, 975, 976, 977
                            attValue = -attValue
                    End Select
                End If

                ' Do custom attribute changes here!
                Select Case modRow.Item("attributeID").ToString
                    Case "204"
                        If attValue = -100 Then Exit Select
                        attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                    Case "51" ' ROF
                        If attValue = -100 Then Exit Select
                        Select Case attMod.DatabaseGroup
                            Case "53" ' Energy Turret 
                                attMod.Attributes.Add("10011", attValue)
                            Case "74" ' Hybrid Turret
                                attMod.Attributes.Add("10012", attValue)
                            Case "55" ' Projectile Turret
                                attMod.Attributes.Add("10013", attValue)
                            Case Else
                                attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                        End Select
                    Case "64" ' Damage Modifier
                        If attValue = 0 Then Exit Select
                        Select Case attMod.DatabaseGroup
                            Case "53" ' Energy Turret 
                                attMod.Attributes.Add("10014", attValue)
                            Case "74" ' Hybrid Turret
                                attMod.Attributes.Add("10015", attValue)
                            Case "55" ' Projectile Turret
                                attMod.Attributes.Add("10016", attValue)
                            Case Else
                                attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                        End Select
                    Case "306" ' Max Velocity Penalty
                        Select Case attMod.DatabaseGroup
                            Case "653", "654", "655", "656", "657", "648" ' T2 Missiles
                                If attValue = -100 Then
                                    attValue = 0
                                End If
                        End Select
                        attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                    Case "144" ' Cap Recharge Rate
                        Select Case attMod.DatabaseGroup
                            Case "653", "654", "655", "656", "657", "648" ' T2 Missiles
                                If attValue = -100 Then
                                    attValue = 0
                                End If
                        End Select
                        attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                    Case Else
                        attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)
                End Select

                Select Case CInt(modRow.Item("attributeID"))
                    Case 30
                        attMod.PG = attValue
                    Case 50
                        attMod.CPU = attValue
                    Case 6
                        attMod.CapUsage = attValue
                    Case 51
                        If attMod.Attributes.Contains("6") = True Then
                            attMod.CapUsageRate = attMod.CapUsage / attValue
                            attMod.Attributes.Add("10032", attMod.CapUsageRate)
                        End If
                    Case 73
                        attMod.ActivationTime = attValue
                        attMod.CapUsageRate = attMod.CapUsage / attMod.ActivationTime
                        attMod.Attributes.Add("10032", attMod.CapUsageRate)
                    Case 77
                        Select Case CInt(attMod.MarketGroup)
                            Case 1038 ' Ice Mining
                                attMod.Attributes("10041") = CDbl(attMod.Attributes("77")) / CDbl(attMod.Attributes("73"))
                            Case 1039, 1040 ' Ore Mining
                                attMod.Attributes("10039") = CDbl(attMod.Attributes("77")) / CDbl(attMod.Attributes("73"))
                            Case 158 ' Mining Drone
                                attMod.Attributes("10040") = CDbl(attMod.Attributes("77")) / CDbl(attMod.Attributes("73"))
                        End Select
                    Case 128
                        attMod.ChargeSize = CInt(attValue)
                    Case 1153
                        attMod.Calibration = CInt(attValue)
                    Case 331 ' Slot Type for Implants
                        attMod.ImplantSlot = CInt(attValue)
                    Case 1087 ' Slot Type For Boosters
                        attMod.BoosterSlot = CInt(attValue)
                    Case 182
                        Dim pSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(CStr(attValue)), Core.SkillList)
                        Dim nSkill As New ItemSkills
                        nSkill.ID = pSkill.ID
                        nSkill.Name = pSkill.Name
                        pSkillName = pSkill.Name
                        attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                    Case 183
                        Dim sSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(CStr(attValue)), Core.SkillList)
                        Dim nSkill As New ItemSkills
                        nSkill.ID = sSkill.ID
                        nSkill.Name = sSkill.Name
                        sSkillName = sSkill.Name
                        attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                    Case 184
                        Dim tSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(CStr(attValue)), Core.SkillList)
                        Dim nSkill As New ItemSkills
                        nSkill.ID = tSkill.ID
                        nSkill.Name = tSkill.Name
                        tSkillName = tSkill.Name
                        attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                    Case 277
                        Dim cSkill As ItemSkills = CType(attMod.RequiredSkills(pSkillName), ItemSkills)
                        If cSkill IsNot Nothing Then
                            cSkill.Level = CInt(attValue)
                        End If
                    Case 278
                        Dim cSkill As ItemSkills = CType(attMod.RequiredSkills(sSkillName), ItemSkills)
                        If cSkill IsNot Nothing Then
                            cSkill.Level = CInt(attValue)
                        End If
                    Case 279
                        Dim cSkill As ItemSkills = CType(attMod.RequiredSkills(tSkillName), ItemSkills)
                        If cSkill IsNot Nothing Then
                            cSkill.Level = CInt(attValue)
                        End If
                    Case 604, 605, 606, 609, 610
                        attMod.Charges.Add(CStr(attValue))
                    Case 633 ' MetaLevel
                        attMod.MetaLevel = CInt(attValue)
                    Case Else
                End Select
                lastModName = modRow.Item("typeName").ToString
                ' Add to the ChargeGroups if it doesn't exist and chargesize <> 0
                'If attMod.IsCharge = True And Charges.ChargeGroups.Contains(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize) = False Then
                '    Charges.ChargeGroups.Add(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize)
                'End If
            Next
            ' Build charge data
            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                If cMod.IsCharge = True Then
                    If Charges.ChargeGroups.Contains(cMod.MarketGroup) = False Then
                        Charges.ChargeGroups.Add(cMod.MarketGroup & "_" & cMod.DatabaseGroup & "_" & cMod.Name & "_" & cMod.ChargeSize)
                    End If
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
            MessageBox.Show("Error building Module Attribute Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            MessageBox.Show("Error building Implant Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    ' NPC Loading Routines
    Private Function LoadNPCData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
            strSQL &= " WHERE (invCategories.categoryID=11) ORDER BY typeName, attributeID;"
            Dim NPCData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If NPCData IsNot Nothing Then
                If NPCData.Tables(0).Rows.Count <> 0 Then
                    Dim lastNPC As String = ""
                    Dim newNPC As New EveHQ.HQF.NPC

                    Dim attValue As Double = 0
                    For Each NPCRow As DataRow In NPCData.Tables(0).Rows
                        ' If the shipName has changed, we need to start a new ship type
                        If lastNPC <> NPCRow.Item("typeName").ToString Then
                            ' Add the current ship to the list then reset the ship data
                            If lastNPC <> "" Then
                                NPCs.NPCList.Add(newNPC.Name, newNPC)
                                newNPC = New EveHQ.HQF.NPC
                            End If
                            ' Create new ship type & non "attribute" data
                            newNPC.Name = NPCRow.Item("typeName").ToString
                            newNPC.GroupName = NPCRow.Item("groupName").ToString
                        End If

                        ' Now get, modify (if applicable) and add the "attribute"
                        If IsDBNull(NPCRow.Item("valueInt")) = True Then
                            attValue = CDbl(NPCRow.Item("valueFloat"))
                        Else
                            attValue = CDbl(NPCRow.Item("valueInt"))
                        End If

                        ' Map only the skill attributes
                        Select Case CInt(NPCRow.Item("attributeID"))
                            Case 51
                                newNPC.ROF = attValue / 1000
                            Case 64
                                newNPC.DamageMod = attValue
                            Case 114
                                newNPC.EM = attValue * newNPC.DamageMod
                            Case 116
                                newNPC.Explosive = attValue * newNPC.DamageMod
                            Case 117
                                newNPC.Kinetic = attValue * newNPC.DamageMod
                            Case 118
                                newNPC.Thermal = attValue * newNPC.DamageMod
                                newNPC.DPS = (newNPC.EM + newNPC.Explosive + newNPC.Kinetic + newNPC.Thermal) / newNPC.ROF
                            Case 506
                                newNPC.MissileROF = attValue / 1000
                            Case 507
                                newNPC.MissileType = attValue.ToString
                                ' Get the details of the missile from the modules
                                Dim missile As ShipModule = CType(ModuleLists.moduleList(newNPC.MissileType), ShipModule)
                                If missile IsNot Nothing Then
                                    newNPC.EM = newNPC.EM + CDbl(missile.Attributes("114"))
                                    newNPC.Explosive = newNPC.Explosive + CDbl(missile.Attributes("116"))
                                    newNPC.Kinetic = newNPC.Kinetic + CDbl(missile.Attributes("117"))
                                    newNPC.Thermal = newNPC.Thermal + CDbl(missile.Attributes("118"))
                                    newNPC.DPS += (CDbl(missile.Attributes("114")) + CDbl(missile.Attributes("116")) + CDbl(missile.Attributes("117")) + CDbl(missile.Attributes("118"))) / newNPC.MissileROF

                                End If
                        End Select
                        lastNPC = NPCRow.Item("typeName").ToString
                    Next

                    ' Perform the last addition for the last ship type
                    NPCs.NPCList.Add(newNPC.Name, newNPC)
                    Return True
                Else
                    MessageBox.Show("NPC Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("NPC Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch e As Exception
            MessageBox.Show("Error loading NPC Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

#End Region

#Region "Form Initialisation & Closing Routines"

    Private Sub frmHQF_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Save the panel widths
        Settings.HQFSettings.ShipPanelWidth = SplitContainer1.Width
        Settings.HQFSettings.ModPanelWidth = SplitContainer2.Width
        ' Save fittings
        Call Me.SaveFittings()
        ' Save the Settings
        Call Settings.HQFSettings.SaveHQFSettings()
        ' Destroy the tab settings
        Me.tabHQF.Dispose()
        ' Destroy the panels
        Me.SplitContainer1.Dispose()
        Me.SplitContainer2.Dispose()
        Me.lvwItems.Dispose()
        Me.tvwItems.Dispose()
        LastModuleResults.Clear()
        LastSlotFitting.Clear()
        ModuleDisplay = ""
    End Sub
    Private Sub frmHQF_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        startUp = True

        ' Close the EveHQ InfoPanel if opted to
        If Settings.HQFSettings.CloseInfoPanel = True Then
            EveHQ.Core.HQ.StartCloseInfoPanel = True
            Me.WindowState = FormWindowState.Maximized
        End If
        ModuleDisplay = ""
        LastModuleResults.Clear()

        ' Clear tabs and fitted ship lists, results list
        ShipLists.fittedShipList.Clear()
        Fittings.FittingTabList.Clear()
        LastModuleResults.Clear()
        tabHQF.TabPages.Clear()
        Me.Show()
        Me.Refresh()

        RemoveHandler ShipModule.ShowModuleMarketGroup, AddressOf Me.UpdateMarketGroup
        RemoveHandler HQFEvents.FindModule, AddressOf Me.UpdateModulesThatWillFit
        RemoveHandler HQFEvents.UpdateFitting, AddressOf Me.UpdateFittings
        RemoveHandler HQFEvents.UpdateModuleList, AddressOf Me.UpdateModuleList

        AddHandler ShipModule.ShowModuleMarketGroup, AddressOf Me.UpdateMarketGroup
        AddHandler HQFEvents.FindModule, AddressOf Me.UpdateModulesThatWillFit
        AddHandler HQFEvents.UpdateFitting, AddressOf Me.UpdateFittings
        AddHandler HQFEvents.UpdateModuleList, AddressOf Me.UpdateModuleList

        ' Load the settings!
        Call Settings.HQFSettings.LoadHQFSettings()

        ' Load the Profiles - stored separately from settings for distibution!
        Call Settings.HQFSettings.LoadProfiles()

        ' Load up a collection of pilots from the EveHQ Core
        Call Me.LoadPilots()

        ' Load saved setups into the fitting array
        Call Me.LoadFittings()

        ' Set the MetaType Filter
        Call Me.SetMetaTypeFilters()

        If HQFData.UseSerializableData = True Then
            Call Me.ShowShipGroups()
            Call Me.ShowMarketGroups()
        Else
            Call Me.ShowShipMarketGroups()
            Call Me.ShowModuleMarketGroups()
            ' Generate the cache
            Call Me.GenerateHQFCache()
            HQFData.UseSerializableData = True
        End If

        startUp = False
        ' Temporarily disable the performance setting
        Dim performanceSetting As Boolean = HQF.Settings.HQFSettings.ShowPerformanceData
        HQF.Settings.HQFSettings.ShowPerformanceData = False

        ' Check if we need to restore tabs from the previous setup
        If HQF.Settings.HQFSettings.RestoreLastSession = True Then
            For Each shipFit As String In HQF.Settings.HQFSettings.OpenFittingList
                If Fittings.FittingList.ContainsKey(shipFit) = True Then
                    ' Create the tab and display
                    If Fittings.FittingTabList.Contains(shipFit) = False Then
                        Call Me.CreateFittingTabPage(shipFit)
                    End If
                    tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
                    Call UpdateSelectedTab()
                    currentShipSlot.UpdateEverything()
                End If
            Next
        End If
        HQF.Settings.HQFSettings.ShowPerformanceData = performanceSetting

        ' Set the panel widths
        SplitContainer1.Width = Settings.HQFSettings.ShipPanelWidth
        SplitContainer2.Width = Settings.HQFSettings.ModPanelWidth
       

    End Sub
    Private Sub LoadFittings()
        Fittings.FittingList.Clear()
        If My.Computer.FileSystem.FileExists(HQF.Settings.HQFFolder & "\HQFFittings.bin") = True Then
            Dim s As New FileStream(HQF.Settings.HQFFolder & "\HQFFittings.bin", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            Fittings.FittingList = CType(f.Deserialize(s), SortedList)
            s.Close()
        End If
        Call Me.UpdateFittingsTree()
    End Sub
    Private Sub SaveFittings()
        ' Save ships
        Dim s As New FileStream(HQF.Settings.HQFFolder & "\HQFFittings.bin", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, Fittings.FittingList)
        s.Close()
    End Sub
    Private Sub ShowShipMarketGroups()
        tvwShips.BeginUpdate()
        tvwShips.Nodes.Clear()
        Dim marketTable As DataTable = HQFData.MarketGroupData.Tables(0)
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
        Dim factionRows() As DataRow = HQFData.shipNameData.Tables(0).Select("ISNULL(marketGroupID, 0) = 0")
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
        Dim groupRows() As DataRow = HQFData.shipNameData.Tables(0).Select("marketGroupID=" & inParentID)
        For Each shipRow As DataRow In groupRows
            inTreeNode.Nodes.Add(shipRow.Item("typeName").ToString)
        Next
    End Sub
    Private Sub ShowModuleMarketGroups()
        tvwItems.BeginUpdate()
        tvwItems.Nodes.Clear()
        Dim marketTable As DataTable = HQFData.MarketGroupData.Tables(0)
        Dim rootRows() As DataRow = marketTable.Select("ISNULL(parentGroupID, 0) = 0")
        For Each rootRow As DataRow In rootRows
            Dim rootNode As New TreeNode(CStr(rootRow.Item("marketGroupName")))
            rootNode.Name = rootNode.Text
            Call PopulateModuleGroups(CInt(rootRow.Item("marketGroupID")), rootNode, marketTable)
            Select Case rootNode.Text
                Case "Ship Equipment", "Ammunition & Charges", "Drones", "Ship Modifications" ', "Implants & Boosters"
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
        Call BuildTreePathData()
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
    Private Sub BuildTreePathData()
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
    Private Sub ShowShipGroups()
        Dim sr As New StreamReader(HQF.Settings.HQFCacheFolder & "\ShipGroups.bin")
        Dim ShipGroups As String = sr.ReadToEnd
        Dim PathLines() As String = ShipGroups.Split(ControlChars.CrLf.ToCharArray)
        Dim nodes() As String
        Dim cNode As New TreeNode
        sr.Close()
        tvwShips.BeginUpdate()
        tvwShips.Nodes.Clear()
        For Each pathline As String In PathLines
            If pathline <> "" Then
                If pathline.Contains("\") = False Then
                    tvwShips.Nodes.Add(pathline, pathline)
                Else
                    nodes = pathline.Split("\".ToCharArray)
                    cNode = tvwShips.Nodes(nodes(0))
                    For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                        cNode = cNode.Nodes(nodes(node))
                    Next
                    cNode.Nodes.Add(nodes(nodes.GetUpperBound(0)), nodes(nodes.GetUpperBound(0)))
                End If
            End If
        Next
        tvwShips.Sorted = True
        tvwShips.EndUpdate()
    End Sub
    Private Sub ShowMarketGroups()
        Dim sr As New StreamReader(HQF.Settings.HQFCacheFolder & "\ItemGroups.bin")
        Dim ShipGroups As String = sr.ReadToEnd
        Dim PathLines() As String = ShipGroups.Split(ControlChars.CrLf.ToCharArray)
        Dim nodes() As String
        Dim nodeData() As String
        Dim cNode As New TreeNode
        Dim newNode As New TreeNode
        sr.Close()
        tvwItems.BeginUpdate()
        tvwItems.Nodes.Clear()
        Market.MarketGroupList.Clear()
        Market.MarketGroupPath.Clear()
        For Each pathline As String In PathLines
            If pathline <> "" Then
                If pathline.Contains("\") = False Then
                    nodeData = pathline.Split(",".ToCharArray)
                    tvwItems.Nodes.Add(nodeData(1), nodeData(1))
                Else
                    nodeData = pathline.Split(",".ToCharArray)
                    nodes = nodeData(1).Split("\".ToCharArray)
                    cNode = tvwItems.Nodes(nodes(0))
                    For node As Integer = 1 To nodes.GetUpperBound(0) - 1
                        cNode = cNode.Nodes(nodes(node))
                    Next
                    newNode = New TreeNode()
                    newNode.Name = nodes(nodes.GetUpperBound(0))
                    newNode.Text = nodes(nodes.GetUpperBound(0))
                    newNode.Tag = nodeData(0)
                    cNode.Nodes.Add(newNode)
                    If newNode.Tag.ToString <> "0" Then
                        Market.MarketGroupList.Add(newNode.Tag.ToString, newNode.Name)
                        Market.MarketGroupPath.Add(newNode.Tag.ToString, nodeData(1))
                    End If
                End If
            End If
        Next
        tvwItems.EndUpdate()
    End Sub
    Private Sub UpdateMarketGroup(ByVal path As String)
        Dim nodes() As String = path.Split("\".ToCharArray)
        Dim cNode As New TreeNode
        cNode = tvwItems.Nodes(nodes(0))
        For node As Integer = 1 To nodes.GetUpperBound(0)
            cNode = cNode.Nodes(nodes(node))
        Next
        tvwItems.SelectedNode = cNode
        tvwItems.Select()
    End Sub
   
    Private Sub LoadPilots()
        ' Loads the skills for the selected pilots
        ' Check for a valid HQFPilotSettings.xml file
        If My.Computer.FileSystem.FileExists(HQF.Settings.HQFFolder & "\HQFPilotSettings.bin") = True Then
            Call HQFPilotCollection.LoadHQFPilotData()
            ' Check we have all the available pilots!
            Dim morePilots As Boolean = False
            For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                If HQFPilotCollection.HQFPilots.Contains(pilot.Name) = False Then
                    ' We don't have it, so lets create one!
                    Dim newHQFPilot As New HQFPilot
                    newHQFPilot.PilotName = pilot.Name
                    newHQFPilot.SkillSet = New Collection
                    HQFPilotCollection.ResetSkillsToDefault(newHQFPilot)
                    For imp As Integer = 0 To 10
                        newHQFPilot.ImplantName(imp) = ""
                    Next
                    HQFPilotCollection.HQFPilots.Add(newHQFPilot.PilotName, newHQFPilot)
                    morePilots = True
                End If
            Next
            ' Check if we need to update the HQFPilot skills to actuals
            If HQF.Settings.HQFSettings.AutoUpdateHQFSkills = True Then
                morePilots = True
                For Each hPilot As HQFPilot In HQFPilotCollection.HQFPilots.Values
                    Call HQFPilotCollection.UpdateHQFSkillsToActual(hPilot)
                Next
            End If
            ' Save the data if we need to
            If morePilots = True Then
                Call HQFPilotCollection.SaveHQFPilotData()
            End If
        Else
            HQFPilotCollection.HQFPilots.Clear()
            For Each pilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                Dim newHQFPilot As New HQFPilot
                newHQFPilot.PilotName = pilot.Name
                newHQFPilot.SkillSet = New Collection
                HQFPilotCollection.ResetSkillsToDefault(newHQFPilot)
                For imp As Integer = 0 To 10
                    newHQFPilot.ImplantName(imp) = ""
                Next
                HQFPilotCollection.HQFPilots.Add(newHQFPilot.PilotName, newHQFPilot)
            Next
            ' Save the data
            Call HQFPilotCollection.SaveHQFPilotData()
        End If

        If HQFPilotCollection.HQFPilots.Count > 0 Then
            btnPilotManager.Enabled = True
        End If

    End Sub
    Private Sub SetMetaTypeFilters()
        Dim filters() As Integer = {1, 2, 4, 8, 16, 32}
        For Each filter As Integer In filters
            Dim chkBox As CheckBox = CType(Me.SplitContainer2.Panel1.Controls.Item("chkFilter" & filter.ToString), CheckBox)
            If (HQF.Settings.HQFSettings.ModuleFilter And filter) = filter Then
                chkBox.Checked = True
            Else
                chkBox.Checked = False
            End If
        Next
    End Sub
#End Region

#Region "Ship Browser Routines"
    Private Sub tvwShips_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwShips.NodeMouseClick
        tvwShips.SelectedNode = e.Node
    End Sub
    Private Sub tvwShips_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwShips.NodeMouseDoubleClick
        tvwShips.SelectedNode = e.Node
        Dim curNode As TreeNode = tvwShips.SelectedNode
        If curNode IsNot Nothing Then
            If curNode.Nodes.Count = 0 Then ' If has no child nodes, therefore a ship not group
                Dim shipName As String = curNode.Text
                mnuShipBrowserShipName.Text = shipName
                Call Me.CreateNewFitting(shipName)
            End If
        End If
    End Sub
    Private Sub ctxShipBrowser_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxShipBrowser.Opening
        Dim curNode As TreeNode = tvwShips.SelectedNode
        If curNode IsNot Nothing Then
            If curNode.Nodes.Count = 0 Then ' If has no child nodes, therefore a ship not group
                Dim shipName As String = curNode.Text
                mnuShipBrowserShipName.Text = shipName
            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuCreateNewFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCreateNewFitting.Click
        ' Get the ship details
        Dim shipName As String = mnuShipBrowserShipName.Text
        Call Me.CreateNewFitting(shipName)
    End Sub
    Private Sub mnuPreviewShip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPreviewShip.Click
        Dim shipName As String = mnuShipBrowserShipName.Text
        Dim selShip As Ship = CType(ShipLists.shipList(shipName), Ship)
        Call DisplayShipPreview(selShip)
    End Sub
    Private Sub DisplayShipPreview(ByVal selShip As Ship)
        pbShip.ImageLocation = "http://www.eve-online.com/bitmaps/icons/itemdb/shiptypes/128_128/" & selShip.ID & ".png"
        lblShipType.Text = selShip.Name
        txtShipDescription.Text = selShip.Description
        lblShieldHP.Text = FormatNumber(selShip.ShieldCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP"
        lblShieldEM.Text = FormatNumber(selShip.ShieldEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldEx.Text = FormatNumber(selShip.ShieldExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldKi.Text = FormatNumber(selShip.ShieldKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldTh.Text = FormatNumber(selShip.ShieldThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblShieldRecharge.Text = "Recharge Rate: " & FormatNumber(selShip.ShieldRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        lblArmorHP.Text = FormatNumber(selShip.ArmorCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP"
        lblArmorEM.Text = FormatNumber(selShip.ArmorEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblArmorEx.Text = FormatNumber(selShip.ArmorExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblArmorKi.Text = FormatNumber(selShip.ArmorKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblArmorTh.Text = FormatNumber(selShip.ArmorThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureHP.Text = FormatNumber(selShip.StructureCapacity, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " HP"
        lblStructureEM.Text = FormatNumber(selShip.StructureEMResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureEx.Text = FormatNumber(selShip.StructureExResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureKi.Text = FormatNumber(selShip.StructureKiResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblStructureTh.Text = FormatNumber(selShip.StructureThResist, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " %"
        lblCapacitor.Text = "Capacitor: " & FormatNumber(selShip.CapCapacity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCapRecharge.Text = "Recharge Rate: " & FormatNumber(selShip.CapRecharge, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " s"
        lblSpeed.Text = "Max Velocity: " & FormatNumber(selShip.MaxVelocity, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m/s"
        lblInertia.Text = "Inertia: " & FormatNumber(selShip.Inertia, 6, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCargohold.Text = "Cargo: " & FormatNumber(selShip.CargoBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"
        lblDroneBay.Text = "Drone Bay: " & FormatNumber(selShip.DroneBay, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " m3"

        lblCPU.Text = "CPU: " & FormatNumber(selShip.CPU, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblPG.Text = "Powergrid: " & FormatNumber(selShip.PG, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCalibration.Text = "Calibration: " & FormatNumber(selShip.Calibration, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        lblLowSlots.Text = "Low Slots: " & FormatNumber(selShip.LowSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblMedSlots.Text = "Med Slots: " & FormatNumber(selShip.MidSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblHiSlots.Text = "High Slots: " & FormatNumber(selShip.HiSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblRigSlots.Text = "Rig Slots: " & FormatNumber(selShip.RigSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblTurretSlots.Text = "Turret Slots: " & FormatNumber(selShip.TurretSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblLauncherSlots.Text = "Launcher Slots: " & FormatNumber(selShip.LauncherSlots, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)

        lblWarpSpeed.Text = "Warp Speed: " & FormatNumber(selShip.WarpSpeed, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au/s"

        ' Add Required Skill into the Description
        txtShipDescription.Text &= ControlChars.CrLf & ControlChars.CrLf & "Required Skills" & ControlChars.CrLf
        For Each cSkill As ItemSkills In selShip.RequiredSkills.Values
            txtShipDescription.Text &= cSkill.Name & " (Lvl " & EveHQ.Core.SkillFunctions.Roman(cSkill.Level) & ")" & ControlChars.CrLf
        Next

        ' Add the tab if it's not available
        If tabHQF.TabPages.Contains(tabShipPreview) = False Then
            tabHQF.TabPages.Add(tabShipPreview)
        End If
        ' Bring the Preview tab to the front
        tabHQF.SelectedTab = tabShipPreview

    End Sub
    Private Sub CreateNewFitting(ByVal shipName As String)
        ' Check we have some valid characters
        If EveHQ.Core.HQ.Pilots.Count > 0 Then
            ' Clear the text boxes
            Dim myNewFitting As New frmModifyFittingName
            Dim fittingName As String = ""
            With myNewFitting
                .txtFittingName.Text = "" : .txtFittingName.Enabled = True
                .btnAccept.Text = "Add" : .Tag = "Add"
                .btnAccept.Tag = shipName
                .Text = "Create New Fitting for " & shipName
                .ShowDialog()
                fittingName = .txtFittingName.Text
            End With
            myNewFitting = Nothing

            If fittingName <> "" Then
                Dim fittingKeyName As String = shipName & ", " & fittingName
                Fittings.FittingList.Add(fittingKeyName, New ArrayList)
                Call Me.CreateFittingTabPage(fittingKeyName)
                Call Me.UpdateFilteredShips()
                tabHQF.SelectedTab = tabHQF.TabPages(fittingKeyName)
                If tabHQF.TabPages.Count = 1 Then
                    Call Me.UpdateSelectedTab()   ' Called when tabpage count=0 as SelectedIndexChanged does not fire!
                End If
                currentShipSlot.UpdateEverything()
            Else
                MessageBox.Show("Unable to Create New Fitting!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
            msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
            MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub txtShipSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShipSearch.TextChanged
        Call Me.UpdateFilteredShips()
    End Sub
    Private Sub UpdateFilteredShips()
        If Len(txtShipSearch.Text) > 0 Then
            Dim strSearch As String = txtShipSearch.Text.Trim.ToLower

            ' Redraw the ships tree
            Dim shipResults As New SortedList(Of String, String)
            For Each sShip As String In ShipLists.shipList.Keys
                If sShip.ToLower.Contains(strSearch) Then
                    shipResults.Add(sShip, sShip)
                End If
            Next
            tvwShips.BeginUpdate()
            tvwShips.Nodes.Clear()
            For Each item As String In shipResults.Values
                Dim shipNode As TreeNode = New TreeNode
                shipNode.Text = item
                shipNode.Name = item
                tvwShips.Nodes.Add(shipNode)
            Next
            tvwShips.EndUpdate()

            ' Redraw the fitting tree
            Dim fitResults As New SortedList(Of String, String)
            For Each sFit As String In Fittings.FittingList.Keys
                If sFit.ToLower.Contains(strSearch) Then
                    fitResults.Add(sFit, sFit)
                End If
            Next
            tvwFittings.Nodes.Clear()
            Dim shipName As String = ""
            Dim fittingName As String = ""
            Dim fittingSep As Integer = 0
            For Each item As String In fitResults.Values
                fittingSep = item.IndexOf(", ")
                shipName = item.Substring(0, fittingSep)
                fittingName = item.Substring(fittingSep + 2)
                ' Create the ship node if it's not already present
                If tvwFittings.Nodes.ContainsKey(shipName) = False Then
                    tvwFittings.Nodes.Add(shipName, shipName)
                End If
                ' Add the details to the Node, checking for duplicates
                tvwFittings.Nodes(shipName).Nodes.Add(fittingName, fittingName)
            Next
            tvwFittings.EndUpdate()
            Call Me.UpdateFittingsCombo()
        Else
            txtShipSearch.Text = ""
            Call Me.ShowShipGroups()
            Call Me.UpdateFittingsTree()
        End If
    End Sub
    Private Sub btnResetShips_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetShips.Click
        txtShipSearch.Text = ""
        Call Me.ShowShipGroups()
        Call Me.UpdateFittingsTree()
    End Sub
#End Region

#Region "Module Display, Filter and Search Options"
    Private Sub tvwItems_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwItems.NodeMouseClick
        tvwItems.SelectedNode = e.Node
    End Sub
    Private Sub tvwItems_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwItems.AfterSelect
        If e.Node.Nodes.Count = 0 Then
            Call Me.CalculateFilteredModules(e.Node)
        End If
    End Sub
    Private Sub MetaFilterChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilter1.CheckedChanged, chkFilter2.CheckedChanged, chkFilter4.CheckedChanged, chkFilter8.CheckedChanged, chkFilter16.CheckedChanged, chkFilter32.CheckedChanged
        If startUp = False Then
            Dim chkBox As CheckBox = CType(sender, CheckBox)
            Dim changedFilter As Integer = CInt(chkBox.Tag)
            HQF.Settings.HQFSettings.ModuleFilter = HQF.Settings.HQFSettings.ModuleFilter Xor changedFilter
            If ModuleDisplay <> "" Then
                Select Case ModuleDisplay
                    Case "Search"
                        Call ShowSearchedModules()
                    Case "Fitted"
                        Call ShowModulesThatWillFit()
                    Case Else
                        Call ShowFilteredModules()
                End Select
            End If
        End If
    End Sub
    Private Sub chkApplySkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplySkills.CheckedChanged
        Call Me.UpdateModuleList()
    End Sub
    Private Sub chkOnlyShowUsable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOnlyShowUsable.CheckedChanged
        Call Me.UpdateModuleList()
    End Sub
    Private Sub chkOnlyShowFittable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOnlyShowFittable.CheckedChanged
        Call Me.UpdateModuleList()
    End Sub
    Private Sub UpdateModuleList()
        If startUp = False Then
            If ModuleDisplay <> "" Then
                Select Case ModuleDisplay
                    Case "Search"
                        Call CalculateSearchedModules()
                    Case "Fitted"
                        If LastSlotFitting.Count > 0 Then
                            Call CalculateModulesThatWillFit()
                        End If
                    Case "Favourites"
                        Call CalculateFilteredModules(tvwItems.SelectedNode)
                    Case "Recently Used"
                        Call CalculateFilteredModules(tvwItems.SelectedNode)
                    Case Else
                        Call CalculateFilteredModules(tvwItems.SelectedNode)
                End Select
            End If
        End If
    End Sub
    Private Sub CalculateFilteredModules(ByVal groupNode As TreeNode)
        Me.Cursor = Cursors.WaitCursor
        Dim cMod, sMod As New ShipModule
        Dim groupID As String
        Dim results As New SortedList
        If groupNode.Name = "Favourites" Then
            ModuleDisplay = "Favourites"
            For Each modName As String In Settings.HQFSettings.Favourites
                cMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule)
                ' Add results in by name, module
                If chkApplySkills.Checked = True Then
                    sMod = Engine.ApplySkillEffectsToModule(cMod)
                Else
                    sMod = cMod.Clone
                End If
                If currentShipInfo IsNot Nothing Then
                    If chkOnlyShowUsable.Checked = True Then
                        If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Else
                        If chkOnlyShowFittable.Checked = True Then
                            If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                results.Add(sMod.Name, sMod)
                            End If
                        Else
                            results.Add(sMod.Name, sMod)
                        End If
                    End If
                Else
                    results.Add(sMod.Name, sMod)
                End If
            Next
            lblModuleDisplayType.Tag = "Displaying: Favourites"
        ElseIf groupNode.Name = "Recently Used" Then
            ModuleDisplay = "Recently Used"
            For Each modName As String In Settings.HQFSettings.MRUModules
                cMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(modName)), ShipModule)
                ' Add results in by name, module
                If chkApplySkills.Checked = True Then
                    sMod = Engine.ApplySkillEffectsToModule(cMod)
                Else
                    sMod = cMod.Clone
                End If
                If chkOnlyShowUsable.Checked = True Then
                    If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                        If chkOnlyShowFittable.Checked = True Then
                            If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                results.Add(sMod.Name, sMod)
                            End If
                        Else
                            results.Add(sMod.Name, sMod)
                        End If
                    End If
                Else
                    If chkOnlyShowFittable.Checked = True Then
                        If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                            results.Add(sMod.Name, sMod)
                        End If
                    Else
                        results.Add(sMod.Name, sMod)
                    End If
                End If
            Next
            lblModuleDisplayType.Tag = "Displaying: Recently Used"
        Else
            If groupNode.Nodes.Count = 0 Then
                groupID = groupNode.Tag.ToString
            Else
                groupID = ModuleDisplay
            End If
            ModuleDisplay = groupID
            lblModuleDisplayType.Tag = Market.MarketGroupList(groupID).ToString
            For Each shipMod As ShipModule In HQF.ModuleLists.moduleList.Values
                If shipMod.MarketGroup = groupID Then
                    ' Add results in by name, module
                    If chkApplySkills.Checked = True Then
                        sMod = Engine.ApplySkillEffectsToModule(shipMod)
                    Else
                        sMod = shipMod.Clone
                    End If
                    If chkOnlyShowUsable.Checked = True And currentShipInfo IsNot Nothing Then
                        If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Else
                        If chkOnlyShowFittable.Checked = True Then
                            If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                results.Add(sMod.Name, sMod)
                            End If
                        Else
                            results.Add(sMod.Name, sMod)
                        End If
                    End If
                End If
            Next
            lblModuleDisplayType.Tag = "Displaying: " & lblModuleDisplayType.Tag.ToString
        End If
        LastModuleResults = results
        Call Me.ShowFilteredModules()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ShowFilteredModules()

        Dim groupID As String = ""
        ' Reset checkbox colours
        chkFilter1.ForeColor = Color.Red
        chkFilter2.ForeColor = Color.Red
        chkFilter4.ForeColor = Color.Red
        chkFilter8.ForeColor = Color.Red
        chkFilter16.ForeColor = Color.Red
        chkFilter32.ForeColor = Color.Red

        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()
        For Each shipMod As ShipModule In LastModuleResults.Values
            If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                Dim newModule As New ListViewItem
                newModule.Name = shipMod.ID
                groupID = shipMod.MarketGroup
                newModule.Text = shipMod.Name
                newModule.ToolTipText = shipMod.Name
                newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                newModule.SubItems.Add(shipMod.CPU.ToString)
                newModule.SubItems.Add(shipMod.PG.ToString)
                Select Case shipMod.SlotType
                    Case 8 ' High
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                        'newModule.ImageKey = "hiSlot"
                    Case 4 ' Mid
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                        'newModule.ImageKey = "midSlot"
                    Case 2 ' Low
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                        'newModule.ImageKey = "lowSlot"
                    Case 1 ' Rig
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                        'newModule.ImageKey = "rigSlot"
                End Select
                Dim chkFilter As CheckBox = CType(Me.SplitContainer2.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                If chkFilter IsNot Nothing Then
                    chkFilter.ForeColor = Color.Black
                End If
                lvwItems.Items.Add(newModule)
            Else
                Dim chkFilter As CheckBox = CType(Me.SplitContainer2.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                If chkFilter IsNot Nothing Then
                    chkFilter.ForeColor = Color.LimeGreen
                End If
            End If
        Next
        If lvwItems.Items.Count = 0 Then
            lvwItems.Items.Add("<Empty - Please check filters>")
            lvwItems.Enabled = False
            lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (0 items)"
        Else
            lvwItems.Enabled = True
            lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (" & lvwItems.Items.Count & " items)"
        End If
        lvwItems.EndUpdate()

    End Sub
    Private Sub CalculateSearchedModules()
        Me.Cursor = Cursors.WaitCursor
        Dim cMod, sMod As New ShipModule
        If Len(txtSearchModules.Text) > 2 Then
            Dim strSearch As String = txtSearchModules.Text.Trim.ToLower
            Dim results As New SortedList
            For Each item As String In HQF.ModuleLists.moduleListName.Keys
                If item.ToLower.Contains(strSearch) Then
                    ' Add results in by name, module
                    cMod = CType(HQF.ModuleLists.moduleList(HQF.ModuleLists.moduleListName(item)), ShipModule)
                    If chkApplySkills.Checked = True Then
                        sMod = Engine.ApplySkillEffectsToModule(cMod)
                    Else
                        sMod = cMod.Clone
                    End If
                    If chkOnlyShowUsable.Checked = True And currentShipInfo IsNot Nothing Then
                        If currentShipInfo.cboPilots.SelectedItem IsNot Nothing Then
                            If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                If chkOnlyShowFittable.Checked = True Then
                                    If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                        results.Add(sMod.Name, sMod)
                                    End If
                                Else
                                    results.Add(sMod.Name, sMod)
                                End If
                            End If
                        Else
                            If chkOnlyShowFittable.Checked = True Then
                                If Engine.IsFittable(sMod, currentShipSlot.ShipFitted) Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Else    
                        results.Add(sMod.Name, sMod)
                    End If
                End If
            Next
            LastModuleResults = results
            lblModuleDisplayType.Tag = "Displaying: Matching *" & txtSearchModules.Text & "*"
            Call Me.ShowSearchedModules()
        End If
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ShowSearchedModules()

        ' Reset checkbox colours
        chkFilter1.ForeColor = Color.Red
        chkFilter2.ForeColor = Color.Red
        chkFilter4.ForeColor = Color.Red
        chkFilter8.ForeColor = Color.Red
        chkFilter16.ForeColor = Color.Red
        chkFilter32.ForeColor = Color.Red

        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()
        For Each shipMod As ShipModule In LastModuleResults.Values
            If ModuleLists.moduleList.Contains(shipMod.ID) = True And Implants.implantList.ContainsKey(shipMod.ID) = False Then
                If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                    Dim newModule As New ListViewItem
                    newModule.Name = shipMod.ID
                    newModule.Text = shipMod.Name
                    newModule.ToolTipText = shipMod.Name
                    newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                    newModule.SubItems.Add(shipMod.CPU.ToString)
                    newModule.SubItems.Add(shipMod.PG.ToString)
                    Select Case shipMod.SlotType
                        Case 8 ' High
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                            'newModule.ImageKey = "hiSlot"
                        Case 4 ' Mid
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                            'newModule.ImageKey = "midSlot"
                        Case 2 ' Low
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                            'newModule.ImageKey = "lowSlot"
                        Case 1 ' Rig
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                            'newModule.ImageKey = "rigSlot"
                    End Select
                    Dim chkFilter As CheckBox = CType(Me.SplitContainer2.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                    chkFilter.ForeColor = Color.Black
                    lvwItems.Items.Add(newModule)
                Else
                    Dim chkFilter As CheckBox = CType(Me.SplitContainer2.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                    chkFilter.ForeColor = Color.LimeGreen
                End If
            End If
        Next
        lvwItems.EndUpdate()
        ModuleDisplay = "Search"
        lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (" & lvwItems.Items.Count & " items)"
    End Sub
    Private Sub UpdateModulesThatWillFit(ByVal modData As ArrayList)
        LastSlotFitting = modData
        If LastSlotFitting.Count > 0 Then
            Call CalculateModulesThatWillFit()
        End If
    End Sub
    Private Sub CalculateModulesThatWillFit()
        Me.Cursor = Cursors.WaitCursor
        Dim slotType As Integer = CInt(LastSlotFitting(0))
        Dim CPU As Double = CDbl(LastSlotFitting(1))
        Dim PG As Double = CDbl(LastSlotFitting(2))
        Dim Calibration As Double = CDbl(LastSlotFitting(3))
        Dim LauncherSlots As Integer = CInt(LastSlotFitting(4))
        Dim TurretSlots As Integer = CInt(LastSlotFitting(5))
        Dim strSearch As String = txtSearchModules.Text.Trim.ToLower
        Dim results As New SortedList
        Dim sMod As New ShipModule
        For Each cMod As ShipModule In HQF.ModuleLists.moduleList.Values
            If chkApplySkills.Checked = True Then
                sMod = Engine.ApplySkillEffectsToModule(cMod)
            Else
                sMod = cMod.Clone
            End If
            If sMod.SlotType = slotType Then
                Select Case slotType
                    Case 1 ' Rig Slot
                        If sMod.Calibration <= Calibration Then
                            If chkOnlyShowUsable.Checked = True Then
                                If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                    results.Add(sMod.Name, sMod)
                                End If
                            Else
                                results.Add(sMod.Name, sMod)
                            End If
                        End If
                    Case 2, 4 ' Low & Mid Slot
                        If sMod.CPU <= CPU Then
                            If sMod.PG <= PG Then
                                If chkOnlyShowUsable.Checked = True Then
                                    If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                        results.Add(sMod.Name, sMod)
                                    End If
                                Else
                                    results.Add(sMod.Name, sMod)
                                End If
                            End If
                        End If
                    Case 8 ' Hi Slot
                        If sMod.CPU <= CPU Then
                            If sMod.PG <= PG Then
                                If LauncherSlots >= Math.Abs(CInt(sMod.IsLauncher)) Then
                                    If TurretSlots >= Math.Abs(CInt(sMod.IsTurret)) Then
                                        If chkOnlyShowUsable.Checked = True Then
                                            If Engine.IsUsable(CType(HQF.HQFPilotCollection.HQFPilots(currentShipInfo.cboPilots.SelectedItem), HQFPilot), sMod) = True Then
                                                results.Add(sMod.Name, sMod)
                                            End If
                                        Else
                                            results.Add(sMod.Name, sMod)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                End Select
            End If
        Next
        LastModuleResults = results
        lblModuleDisplayType.Tag = "Displaying: Modules That Fit"
        Call Me.ShowModulesThatWillFit()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub ShowModulesThatWillFit()

        ' Reset checkbox colours
        chkFilter1.ForeColor = Color.Red
        chkFilter2.ForeColor = Color.Red
        chkFilter4.ForeColor = Color.Red
        chkFilter8.ForeColor = Color.Red
        chkFilter16.ForeColor = Color.Red
        chkFilter32.ForeColor = Color.Red

        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()
        For Each shipMod As ShipModule In LastModuleResults.Values
            If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                Dim newModule As New ListViewItem
                newModule.Name = shipMod.ID
                newModule.Text = shipMod.Name
                newModule.ToolTipText = shipMod.Name
                newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                newModule.SubItems.Add(shipMod.CPU.ToString)
                newModule.SubItems.Add(shipMod.PG.ToString)
                Select Case shipMod.SlotType
                    Case 8 ' High
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                        'newModule.ImageKey = "hiSlot"
                    Case 4 ' Mid
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                        'newModule.ImageKey = "midSlot"
                    Case 2 ' Low
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                        'newModule.ImageKey = "lowSlot"
                    Case 1 ' Rig
                        newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                        'newModule.ImageKey = "rigSlot"
                End Select
                Dim chkFilter As CheckBox = CType(Me.SplitContainer2.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                If chkFilter IsNot Nothing Then
                    chkFilter.ForeColor = Color.Black
                End If
                lvwItems.Items.Add(newModule)
            Else
                Dim chkFilter As CheckBox = CType(Me.SplitContainer2.Panel1.Controls("chkFilter" & shipMod.MetaType), CheckBox)
                chkFilter.ForeColor = Color.LimeGreen
            End If
        Next
        lvwItems.EndUpdate()
        ModuleDisplay = "Fitted"
        lblModuleDisplayType.Text = lblModuleDisplayType.Tag.ToString & " (" & lvwItems.Items.Count & " items)"
    End Sub
    Private Sub txtSearchModules_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchModules.GotFocus
        Call CalculateSearchedModules()
    End Sub
    Private Sub txtSearchModules_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchModules.TextChanged
        Call CalculateSearchedModules()
    End Sub
    
#End Region

#Region "Module List Routines"
    Private Sub lvwItems_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwItems.ColumnClick
        If CInt(lvwItems.Tag) = e.Column Then
            lvwItems.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwItems.Tag = -1
        Else
            lvwItems.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwItems.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwItems.Sort()
    End Sub
    Private Sub lvwItems_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwItems.DoubleClick
        If currentShipSlot IsNot Nothing Then
            Dim moduleID As String = lvwItems.SelectedItems(0).Name
            Dim shipMod As ShipModule = CType(ModuleLists.moduleList(moduleID), ShipModule).Clone
            If shipMod.IsDrone = True Then
                Dim active As Boolean = False
                Call currentShipSlot.AddDrone(shipMod, 1, False)
            Else
                ' Check if module is a charge
                If shipMod.IsCharge = True Then
                    currentShipSlot.AddItem(shipMod, 1)
                Else
                    ' Must be a proper module then!
                    Call currentShipSlot.AddModule(shipMod, 0, True, Nothing)
                    ' Add it to the MRU
                    Call Me.UpdateMRUModules(shipMod.Name)
                End If
            End If
        End If
    End Sub
    Private Sub UpdateMRUModules(ByVal modName As String)
        If HQF.Settings.HQFSettings.MRUModules.Count < HQF.Settings.HQFSettings.MRULimit Then
            ' If the MRU list isn't full
            If HQF.Settings.HQFSettings.MRUModules.Contains(modName) = False Then
                ' If the module isn't already in the list
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            Else
                ' If it is in the list, remove it and add it at the end
                HQF.Settings.HQFSettings.MRUModules.Remove(modName)
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            End If
        Else
            If HQF.Settings.HQFSettings.MRUModules.Contains(modName) = False Then
                For m As Integer = 0 To HQF.Settings.HQFSettings.MRULimit - 2
                    HQF.Settings.HQFSettings.MRUModules(m) = HQF.Settings.HQFSettings.MRUModules(m + 1)
                Next
                HQF.Settings.HQFSettings.MRUModules.RemoveAt(HQF.Settings.HQFSettings.MRULimit - 1)
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            Else
                ' If it is in the list, remove it and add it at the end
                HQF.Settings.HQFSettings.MRUModules.Remove(modName)
                HQF.Settings.HQFSettings.MRUModules.Add(modName)
            End If
        End If
    End Sub
#End Region

    Private Sub tsbOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbOptions.Click
        Call Me.OpenSettingsForm()
    End Sub

    Public Sub UpdateFittings()
        Me.Cursor = Cursors.WaitCursor
        ' Updates all the open fittings
        For Each openTab As String In Fittings.FittingTabList
            Dim thisTab As TabPage = tabHQF.TabPages(openTab)
            If thisTab IsNot Nothing Then
                If thisTab.Controls.Count > 0 Then
                    Dim thisShipSlotControl As ShipSlotControl = CType(thisTab.Controls("panelShipSlot").Controls("shipSlot"), ShipSlotControl)
                    Dim thisShipInfoControl As ShipInfoControl = CType(thisTab.Controls("panelShipInfo").Controls("shipInfo"), ShipInfoControl)
                    thisShipSlotControl.UpdateEverything()
                End If
            End If
        Next
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        ' Count number of items
        Dim items As Integer = ModuleLists.moduleList.Count
        ' Check MarketGroups
        Dim marketError As Integer = 0
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If Market.MarketGroupList.ContainsKey(item.MarketGroup) = False Then
                marketError += 1
                'MessageBox.Show(item.Name)
            End If
        Next
        ' Check MetaGroups
        Dim metaError As Integer = 0
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If ModuleLists.moduleMetaGroups.ContainsKey(item.ID) = False Then
                metaError += 1
                'MessageBox.Show(item.Name)
            End If
        Next

        Dim msg As String = ""
        msg &= "Total items: " & items & ControlChars.CrLf
        msg &= "Orphaned market items: " & marketError & ControlChars.CrLf
        msg &= "Orphaned meta items: " & metaError & ControlChars.CrLf
        MessageBox.Show(msg)

        ' Traverse the tree, looking for goodies!
        itemCount = 0
        dataCheckList.Clear()
        For Each rootNode As TreeNode In tvwItems.Nodes
            SearchChildNodes(rootNode)
        Next

        ' Write missing items to a file
        Dim sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\missingItems.csv")
        For Each shipMod As ShipModule In ModuleLists.moduleList.Values
            If dataCheckList.Contains(shipMod.ID) = False Then
                sw.WriteLine(shipMod.ID & "," & shipMod.Name)
                dataCheckList.Add(shipMod.ID, shipMod.Name)
            End If
        Next
        sw.Flush()
        sw.Close()

        MessageBox.Show("Total traversed items: " & itemCount, "Tree Walk Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub SearchChildNodes(ByRef parentNode As TreeNode)

        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                SearchChildNodes(childNode)
            Else
                Dim groupID As String = childNode.Tag.ToString
                For Each shipMod As ShipModule In ModuleLists.moduleList.Values
                    If shipMod.MarketGroup = groupID Then
                        itemCount += 1
                        dataCheckList.Add(shipMod.ID, shipMod.Name)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub SearchChildNodes1(ByRef parentNode As TreeNode, ByVal sw As IO.StreamWriter)
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                SearchChildNodes1(childNode, sw)
            Else
                sw.Write(childNode.FullPath & ControlChars.CrLf)
            End If
        Next
    End Sub
  
#Region "TabHQF Selection and Context Menu Routines"

    Private Sub tabHQF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabHQF.SelectedIndexChanged
        Call Me.UpdateSelectedTab()
    End Sub
    Private Sub UpdateSelectedTab()
        If tabHQF.SelectedTab IsNot Nothing Then
            If Fittings.FittingTabList.Contains(tabHQF.SelectedTab.Text) Then
                ' Get the controls on the existing tab
                Dim thisShipSlotControl As ShipSlotControl = CType(tabHQF.SelectedTab.Controls("panelShipSlot").Controls("shipSlot"), ShipSlotControl)
                Dim thisShipInfoControl As ShipInfoControl = CType(tabHQF.SelectedTab.Controls("panelShipInfo").Controls("shipInfo"), ShipInfoControl)
                currentShipSlot = thisShipSlotControl
                currentShipInfo = thisShipInfoControl
                currentShipSlot.ShipFit = tabHQF.SelectedTab.Text
                currentShipSlot.ShipInfo = currentShipInfo
                currentShipInfo.BuildMethod = BuildType.BuildEffectsMaps
            End If
            tabHQF.Tag = tabHQF.SelectedIndex
            btnCopy.Enabled = True
            btnScreenshot.Enabled = True
        Else
            btnCopy.Enabled = False
            btnScreenshot.Enabled = False
        End If
    End Sub
    Private Function TabControlHitTest(ByVal TabCtrl As TabControl, ByVal pt As Point) As Integer
        ' Test each tabs rectangle to see if our point is contained within it.
        For x As Integer = 0 To TabCtrl.TabPages.Count - 1
            ' If tab contians our rectangle return it's index.
            If TabCtrl.GetTabRect(x).Contains(pt) Then Return x
        Next
        ' A tab was not located at specified point.
        Return -1
    End Function
    Private Sub TabHQF_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tabHQF.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim TabIndex As Integer
            ' Get index of tab clicked
            TabIndex = TabControlHitTest(tabHQF, e.Location)
            ' If a tab was clicked display it's index
            If TabIndex >= 0 Then
                tabHQF.Tag = TabIndex
                Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
                mnuCloseHQFTab.Text = "Close " & tp.Text
            Else
                mnuCloseHQFTab.Text = "Not Valid"
            End If
        End If
    End Sub

    Private Sub ctxTabHQF_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles ctxTabHQF.Closed
        mnuCloseHQFTab.Text = "Not Valid"
    End Sub
    Private Sub ctxTabHQF_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxTabHQF.Opening
        If mnuCloseHQFTab.Text = "Not Valid" Then
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuCloseMDITab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCloseHQFTab.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Fittings.FittingTabList.Remove(tp.Text)
        ShipLists.fittedShipList.Remove(tp.Text)
        tabHQF.TabPages.Remove(tp)
    End Sub
#End Region

#Region "Clipboard Paste Routines (incl Timer)"
    Private Sub tmrClipboard_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClipboard.Tick
        ' Checks the clipboard for any compatible fixes!
        If Clipboard.GetDataObject IsNot Nothing Then
            Try
                Dim fileText As String = CStr(Clipboard.GetDataObject().GetData(DataFormats.Text))
                If fileText IsNot Nothing Then
                    Dim fittingMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(fileText, "\[(?<ShipName>[^,]*)\]|\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
                    If fittingMatch.Success = True Then
                        ' Appears to be a match so lets check the ship type
                        If ShipLists.shipList.Contains(fittingMatch.Groups.Item("ShipName").Value) = True Then
                            btnClipboardPaste.Enabled = True
                        Else
                            btnClipboardPaste.Enabled = False
                        End If
                    Else
                        btnClipboardPaste.Enabled = False
                    End If
                Else
                    btnClipboardPaste.Enabled = False
                End If
            Catch ex As Exception
                btnClipboardPaste.Enabled = False
            End Try
        Else
            btnClipboardPaste.Enabled = False
        End If
    End Sub
    Private Sub btnClipboardPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClipboardPaste.Click
        ' Pick the text up from the clipboard
        Dim fileText As String = CStr(Clipboard.GetDataObject().GetData(DataFormats.Text))
        ' Use Regex to get the data - No checking as this is done in the tmrClipboard_Tick sub
        Dim fittingMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(fileText, "\[(?<ShipName>[^,]*)\]|\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
        Dim shipName As String = fittingMatch.Groups.Item("ShipName").Value
        Dim fittingName As String = ""
        If fittingMatch.Groups.Item("FittingName").Value <> "" Then
            fittingName = fittingMatch.Groups.Item("FittingName").Value
        Else
            fittingName = "Imported Fit"
        End If
        ' If the fitting exists, add a number onto the end
        If Fittings.FittingList.ContainsKey(shipName & ", " & fittingName) = True Then
            Dim response As Integer = MessageBox.Show("Fitting name already exists. Are you sure you wish to import the fitting?", "Confirm Import for " & shipName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If response = Windows.Forms.DialogResult.Yes Then
                Dim newFittingName As String = ""
                Dim revision As Integer = 1
                Do
                    revision += 1
                    newFittingName = fittingName & " " & revision.ToString
                Loop Until Fittings.FittingList.ContainsKey(shipName & ", " & newFittingName) = False
                fittingName = newFittingName
                MessageBox.Show("New fitting name is '" & fittingName & "'.", "New Fitting Imported", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Exit Sub
            End If
        End If
        ' Lets create the fitting
        Dim mods() As String = fileText.Split(ControlChars.CrLf.ToCharArray)
        Dim newFit As New ArrayList
        For Each ShipMod As String In mods
            If ShipMod.StartsWith("[") = False And ShipMod <> "" Then
                newFit.Add(ShipMod)
            End If
        Next
        Fittings.FittingList.Add(shipName & ", " & fittingName, newFit)
        Call Me.UpdateFittingsTree()
    End Sub
#End Region

#Region "Fitting Panel Routines"

    Private Sub UpdateFittingsTree()
        tvwFittings.BeginUpdate()
        ' Get Current List of "open" nodes
        Dim openNodes As New ArrayList
        For Each shipNode As TreeNode In tvwFittings.Nodes
            If shipNode.IsExpanded = True Then
                openNodes.Add(shipNode.Name)
            End If
        Next
        ' Redraw the tree
        tvwFittings.Nodes.Clear()
        Dim shipName As String = ""
        Dim fittingName As String = ""
        Dim fittingSep As Integer = 0
        For Each fitting As String In Fittings.FittingList.Keys
            fittingSep = fitting.IndexOf(", ")
            shipName = fitting.Substring(0, fittingSep)
            fittingName = fitting.Substring(fittingSep + 2)
            ' Create the ship node if it's not already present
            If tvwFittings.Nodes.ContainsKey(shipName) = False Then
                tvwFittings.Nodes.Add(shipName, shipName)
            End If
            ' Add the details to the Node, checking for duplicates
            If tvwFittings.Nodes(shipName).Nodes.ContainsKey(fittingName) = False Then
                tvwFittings.Nodes(shipName).Nodes.Add(fittingName, fittingName)
            Else
                MessageBox.Show("Duplicate fitting found for " & shipName & ", and omitted", "Duplicate Fitting Found!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Next
        For Each shipNode As String In openNodes
            If tvwFittings.Nodes.ContainsKey(shipNode) = True Then
                tvwFittings.Nodes(shipNode).Expand()
            End If
        Next
        tvwFittings.EndUpdate()
        Call Me.UpdateFittingsCombo()
    End Sub
    Private Sub UpdateFittingsCombo()
        cboFittings.BeginUpdate()
        cboFittings.Items.Clear()
        For Each fitting As String In Fittings.FittingList.Keys
            cboFittings.Items.Add(fitting)
        Next
        cboFittings.EndUpdate()
    End Sub
    Private Sub tvwFittings_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwFittings.NodeMouseClick
        tvwFittings.SelectedNode = e.Node
    End Sub
    Private Sub tvwFittings_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwFittings.NodeMouseDoubleClick
        Call Me.ShowFitting(e.Node)
    End Sub
    Private Sub CreateFittingTabPage(ByVal shipFit As String)
        Dim fittingSep As Integer = shipFit.IndexOf(", ")
        Dim shipName As String = shipFit.Substring(0, fittingSep)
        Dim fittingName As String = shipFit.Substring(fittingSep + 2)
        Dim curShip As Ship = CType(CType(ShipLists.shipList(shipName), Ship).Clone, Ship)
        curShip.DamageProfile = CType(DamageProfiles.ProfileList.Item("<Omni-Damage>"), DamageProfile)
        ShipLists.fittedShipList.Add(shipFit, curShip)

        Dim tp As New TabPage(shipFit)
        tp.Tag = shipFit
        tp.Name = shipFit

        tabHQF.TabPages.Add(tp)
        tp.Parent = Me.tabHQF

        Dim pSS As New Panel
        pSS.BorderStyle = BorderStyle.Fixed3D
        pSS.Dock = System.Windows.Forms.DockStyle.Fill
        pSS.Location = New System.Drawing.Point(0, 0)
        pSS.Name = "panelShipSlot"
        pSS.Size = New System.Drawing.Size(414, 600)
        pSS.TabIndex = 1

        Dim pSI As New Panel
        pSI.Dock = System.Windows.Forms.DockStyle.Left
        pSI.Location = New System.Drawing.Point(0, 384)
        pSI.Name = "panelShipInfo"
        pSI.Size = New System.Drawing.Size(270, 600)
        pSI.TabIndex = 0

        tp.Controls.Add(pSS)
        tp.Controls.Add(pSI)
        tp.Location = New System.Drawing.Point(4, 22)
        tp.Size = New System.Drawing.Size(414, 666)
        tp.UseVisualStyleBackColor = True

        Dim shipSlot As New ShipSlotControl
        shipSlot.Name = "shipSlot"
        shipSlot.Location = New Point(0, 0)
        shipSlot.Dock = DockStyle.Fill
        pSS.Controls.Add(shipSlot)

        Dim shipInfo As New ShipInfoControl
        shipInfo.Name = "shipInfo"
        shipInfo.Location = New Point(0, 0)
        shipInfo.Dock = DockStyle.Fill
        pSI.Controls.Add(shipInfo)

        shipInfo.ShipSlot = shipSlot
        shipSlot.ShipInfo = shipInfo
        shipSlot.ShipFit = shipFit

        Fittings.FittingTabList.Add(shipFit)
    End Sub
    Private Sub ctxFittings_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxFittings.Opening
        Dim curNode As TreeNode = tvwFittings.SelectedNode
        If curNode IsNot Nothing Then
            Dim parentNode As TreeNode = curNode.Parent
            If parentNode IsNot Nothing Then
                mnuFittingsFittingName.Text = parentNode.Text & ", " & curNode.Text
                mnuFittingsFittingName.Tag = parentNode.Text
                mnuFittingsCreateFitting.Text = "Create New " & parentNode.Text & " Fitting"
                mnuFittingsCopyFitting.Enabled = True
                mnuFittingsDeleteFitting.Enabled = True
                mnuFittingsRenameFitting.Enabled = True
                mnuFittingsShowFitting.Enabled = True
            Else
                mnuFittingsFittingName.Text = curNode.Text
                mnuFittingsFittingName.Tag = curNode.Text
                mnuFittingsCreateFitting.Text = "Create New " & curNode.Text & " Fitting"
                mnuFittingsCopyFitting.Enabled = False
                mnuFittingsDeleteFitting.Enabled = False
                mnuFittingsRenameFitting.Enabled = False
                mnuFittingsShowFitting.Enabled = False
            End If
        Else
            e.Cancel = True
        End If
    End Sub
    Private Sub mnuFittingsShowFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsShowFitting.Click
        ' Get the node details
        Dim curNode As TreeNode = tvwFittings.SelectedNode
        Call Me.ShowFitting(curNode)
    End Sub
    Private Sub mnuFittingsRenameFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsRenameFitting.Click
        ' Get the node details
        Dim curNode As TreeNode = tvwFittings.SelectedNode
        Dim parentnode As TreeNode = curNode.Parent
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text
        Dim oldKeyName As String = shipName & ", " & fitName
        Dim FitToCopy As ArrayList = CType(Fittings.FittingList(oldKeyName), ArrayList)

        ' Clear the text boxes
        Dim myNewFitting As New frmModifyFittingName
        Dim fittingName As String = ""
        With myNewFitting
            .txtFittingName.Text = fitName : .txtFittingName.Enabled = True
            .btnAccept.Text = "Edit" : .Tag = "Edit"
            .btnAccept.Tag = shipName
            .Text = "Edit Fitting Name for " & shipName
            .ShowDialog()
            fittingName = .txtFittingName.Text
        End With
        myNewFitting = Nothing

        ' Add and Remove the Fittings
        If fittingName <> "" Then
            Dim fittingKeyName As String = shipName & ", " & fittingName
            Fittings.FittingList.Remove(oldKeyName)
            Fittings.FittingList.Add(fittingKeyName, FitToCopy.Clone)
            ' Rename the file if available
            If My.Computer.FileSystem.FileExists(Settings.HQFFolder & "\" & oldKeyName & ".hqf") = True Then
                My.Computer.FileSystem.RenameFile(Settings.HQFFolder & "\" & oldKeyName & ".hqf", fittingKeyName & ".hqf")
            End If
            ' Amend it in the tabs if it's there!
            Dim tp As TabPage = tabHQF.TabPages(oldKeyName)
            If tp IsNot Nothing Then
                Fittings.FittingTabList.Remove(oldKeyName)
                Fittings.FittingTabList.Add(fittingKeyName)
                tp.Name = fittingKeyName
                tp.Tag = fittingKeyName
                tp.Text = fittingKeyName
            End If
            Call Me.UpdateFilteredShips()
        Else
            MessageBox.Show("Unable to Copy Fitting!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuFittingsCopyFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsCopyFitting.Click
        ' Get the node details
        Dim curNode As TreeNode = tvwFittings.SelectedNode
        Dim parentnode As TreeNode = curNode.Parent
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text
        Dim fitKeyName As String = shipName & ", " & fitName
        Dim FitToCopy As ArrayList = CType(Fittings.FittingList(fitKeyName), ArrayList)

        ' Clear the text boxes
        Dim myNewFitting As New frmModifyFittingName
        Dim fittingName As String = ""
        With myNewFitting
            .txtFittingName.Text = "" : .txtFittingName.Enabled = True
            .btnAccept.Text = "Copy" : .Tag = "Copy"
            .btnAccept.Tag = shipName
            .Text = "Copy '" & fitName & "' Fitting for " & shipName
            .ShowDialog()
            fittingName = .txtFittingName.Text
        End With
        myNewFitting = Nothing

        ' Add and Copy the Fitting
        If fittingName <> "" Then
            Dim fittingKeyName As String = shipName & ", " & fittingName
            Fittings.FittingList.Add(fittingKeyName, FitToCopy.Clone)
            Call Me.UpdateFilteredShips()
        Else
            MessageBox.Show("Unable to Copy Fitting!", "Copy Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuFittingsDeleteFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsDeleteFitting.Click
        ' Get the node details
        Dim curNode As TreeNode = tvwFittings.SelectedNode
        Dim parentnode As TreeNode = curNode.Parent
        Dim shipName As String = parentnode.Text
        Dim fitName As String = curNode.Text

        ' Get confirmation of deletion
        Dim response As Integer = MessageBox.Show("Are you sure you wish to delete the '" & fitName & "' Fitting for the " & shipName & "?", "Confirm Fitting Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If response = Windows.Forms.DialogResult.Yes Then
            ' Remove the fit from the list
            Dim fittingKeyName As String = shipName & ", " & fitName
            Fittings.FittingList.Remove(fittingKeyName)
            ' Delete the file if it's there
            If My.Computer.FileSystem.FileExists(Settings.HQFFolder & "\" & fittingKeyName & ".hqf") = True Then
                My.Computer.FileSystem.DeleteFile(Settings.HQFFolder & "\" & fittingKeyName & ".hqf")
            End If
            ' Delete it from the tabs if it's there!
            Dim tp As TabPage = tabHQF.TabPages(fittingKeyName)
            If tp IsNot Nothing Then
                Fittings.FittingTabList.Remove(tp.Text)
                tabHQF.TabPages.Remove(tp)
                ShipLists.fittedShipList.Remove(tp.Text)
            End If
            ' Update the list
            Call Me.UpdateFilteredShips()
        End If
    End Sub
    Private Sub mnuFittingsCreateFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFittingsCreateFitting.Click
        ' Get the node details
        Dim curNode As TreeNode = tvwFittings.SelectedNode
        Dim shipName As String = mnuFittingsFittingName.Tag.ToString

        ' Clear the text boxes
        Dim myNewFitting As New frmModifyFittingName
        Dim fittingName As String = ""
        With myNewFitting
            .txtFittingName.Text = "" : .txtFittingName.Enabled = True
            .btnAccept.Text = "Add" : .Tag = "Add"
            .btnAccept.Tag = shipName
            .Text = "Create New Fitting for " & shipName
            .ShowDialog()
            fittingName = .txtFittingName.Text
        End With
        myNewFitting = Nothing

        ' Add the Fitting
        If fittingName <> "" Then
            Dim fittingKeyName As String = shipName & ", " & fittingName
            Fittings.FittingList.Add(fittingKeyName, New ArrayList)
            Call Me.CreateFittingTabPage(fittingKeyName)
            Call Me.UpdateFilteredShips()
            tabHQF.SelectedTab = tabHQF.TabPages(fittingKeyName)
            If tabHQF.SelectedIndex = 0 Then Call Me.UpdateSelectedTab()
            currentShipSlot.UpdateEverything()
        Else
            MessageBox.Show("Unable to Create New Fitting!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub mnuPreviewShip2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuPreviewShip2.Click
        Dim curNode As TreeNode = tvwFittings.SelectedNode
        Dim shipName As String = mnuFittingsFittingName.Tag.ToString
        Dim selShip As Ship = CType(ShipLists.shipList(shipName), Ship)
        Call DisplayShipPreview(selShip)
    End Sub
    Private Sub ShowFitting(ByVal fittingNode As TreeNode)
        ' Check we have some valid characters
        If EveHQ.Core.HQ.Pilots.Count > 0 Then
            ' Get the ship details
            If fittingNode.Parent IsNot Nothing Then
                Dim shipName As String = fittingNode.Parent.Text
                Dim shipFit As String = fittingNode.Parent.Text & ", " & fittingNode.Text
                ' Create the tab and display
                If Fittings.FittingTabList.Contains(shipFit) = False Then
                    Call Me.CreateFittingTabPage(shipFit)
                End If
                tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
                If tabHQF.SelectedIndex = 0 Then Call Me.UpdateSelectedTab()
                currentShipSlot.UpdateEverything()
            End If
        Else
            Dim msg As String = "There are no pilots or accounts created in EveHQ." & ControlChars.CrLf
            msg &= "Please add an API account or manual pilot in the main EveHQ Settings before opening or creating a fitting."
            MessageBox.Show(msg, "Pilots Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
#End Region

#Region "Fittings Combo Routines"
    Private Sub cboFittings_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFittings.SelectedIndexChanged
        Dim shipFit As String = cboFittings.SelectedItem.ToString
        ' Create the tab and display
        If Fittings.FittingTabList.Contains(shipFit) = False Then
            Call Me.CreateFittingTabPage(shipFit)
        End If
        tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
        If tabHQF.SelectedIndex = 0 Then Call Me.UpdateSelectedTab()
        currentShipSlot.UpdateEverything()
    End Sub
#End Region

    Private Sub btnShipPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShipPanel.Click
        If btnShipPanel.Checked = True Then
            ' If the panel is open
            'btnShipPanel.Image = My.Resources.panel_close
            SplitContainer1.Visible = True
            cboFittings.Visible = False
        Else
            ' If the panel is closed
            'btnShipPanel.Image = My.Resources.panel_open
            SplitContainer1.Visible = False
            cboFittings.Visible = True
        End If
    End Sub

    Private Sub btnItemPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemPanel.Click
        If btnItemPanel.Checked = True Then
            ' If the panel is open
            'btnShipPanel.Image = My.Resources.panel_close
            SplitContainer2.Visible = True
        Else
            ' If the panel is closed
            'btnShipPanel.Image = My.Resources.panel_open
            SplitContainer2.Visible = False
        End If
    End Sub

#Region "Cache Routines"
    Private Sub GenerateHQFCache()
        cacheForm.ShowDialog()
        ' Write Ship Tree 
        Call Me.WriteShipGroups()
        Call Me.WriteItemGroups()
        ' Write the current version
        Dim sw As New StreamWriter(HQF.Settings.HQFCacheFolder & "\version.txt")
        sw.Write(HQFData.LastCacheRefresh)
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteShipGroups()
        Dim sw As New IO.StreamWriter(HQF.Settings.HQFCacheFolder & "\ShipGroups.bin")
        For Each rootNode As TreeNode In tvwShips.Nodes
            WriteShipNodes(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteItemGroups()
        Dim sw As New IO.StreamWriter(HQF.Settings.HQFCacheFolder & "\ItemGroups.bin")
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

#Region "Module List Context Menu Routines"

    Private Sub ctxModuleList_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxModuleList.Opening
        If lvwItems.SelectedItems.Count > 0 Then
            Dim moduleID As String = lvwItems.SelectedItems(0).Name
            Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
            If ModuleDisplay = "Favourites" Then
                mnuAddToFavourites_List.Visible = False
                mnuRemoveFromFavourites.Visible = True
            Else
                mnuAddToFavourites_List.Visible = True
                mnuRemoveFromFavourites.Visible = False
                If Settings.HQFSettings.Favourites.Contains(cModule.Name) = True Then
                    mnuAddToFavourites_List.Enabled = False
                Else
                    mnuAddToFavourites_List.Enabled = True
                End If
            End If
            If IsNumeric(ModuleDisplay) = True Then
                mnuSep2.Visible = False
                mnuShowModuleMarketGroup.Visible = False
            Else
                mnuSep2.Visible = True
                mnuShowModuleMarketGroup.Visible = True
            End If
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub mnuShowModuleInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowModuleInfo.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim showInfo As New frmShowInfo
        showInfo.ShowItemDetails(cModule)
        showInfo = Nothing
    End Sub

    Private Sub mnuAddToFavourites_List_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToFavourites_List.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        If Settings.HQFSettings.Favourites.Contains(cModule.Name) = False Then
            Settings.HQFSettings.Favourites.Add(cModule.Name)
        End If
    End Sub

    Private Sub mnuRemoveFromFavourites_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRemoveFromFavourites.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        If Settings.HQFSettings.Favourites.Contains(cModule.Name) = True Then
            Settings.HQFSettings.Favourites.Remove(cModule.Name)
        End If
        Call CalculateFilteredModules(tvwItems.SelectedNode)
    End Sub

    Private Sub mnuShowModuleMarketGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowModuleMarketGroup.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim pathLine As String = CStr(Market.MarketGroupPath(cModule.MarketGroup))
        ShipModule.DisplayedMarketGroup = pathLine
    End Sub

#End Region

#Region "Menu & Button Routines"

    Private Sub btnScreenshot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScreenshot.Click
        ' Determine co-ords of current main panel
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim xy As Point = tp.PointToScreen(New Point(0, 0))
        Dim sx As Integer = xy.X
        Dim sy As Integer = xy.Y
        Dim fittingImage As Bitmap = ScreenGrab.GrabScreen(New Rectangle(sx, sy, tp.Width, tp.Height))
        Clipboard.SetDataObject(fittingImage)
        Dim filename As String = "HQF_" & tp.Text & "_" & Format(Now, "yyyy-MM-dd-HH-mm-ss") & ".png"
        fittingImage.Save(EveHQ.Core.HQ.reportFolder & "\" & filename, System.Drawing.Imaging.ImageFormat.Png)
    End Sub

    Private Sub mnuCopyForHQF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyForHQF.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim currentShip As Ship = currentShipSlot.ShipCurrent
        Dim fittedShip As Ship = currentShipSlot.ShipFitted
        Dim cModule As New ShipModule
        Dim state As Integer
        Dim fitting As New System.Text.StringBuilder
        fitting.AppendLine("[" & tp.Text & "]")
        For slot As Integer = 1 To currentship.HiSlots
            If currentship.HiSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.HiSlot(slot).ModuleState) / Math.Log(2))
                If currentship.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.HiSlot(slot).Name & "_" & state & ", " & currentship.HiSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.HiSlot(slot).Name & "_" & state)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentship.MidSlots
            If currentship.MidSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.MidSlot(slot).ModuleState) / Math.Log(2))
                If currentship.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.MidSlot(slot).Name & "_" & state & ", " & currentship.MidSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.MidSlot(slot).Name & "_" & state)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentship.LowSlots
            If currentship.LowSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.LowSlot(slot).ModuleState) / Math.Log(2))
                If currentship.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.LowSlot(slot).Name & "_" & state & ", " & currentship.LowSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.LowSlot(slot).Name & "_" & state)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentship.RigSlots
            If currentship.RigSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.RigSlot(slot).ModuleState) / Math.Log(2))
                If currentship.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.RigSlot(slot).Name & "_" & state & ", " & currentship.RigSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.RigSlot(slot).Name & "_" & state)
                End If
            End If
        Next
        fitting.AppendLine("")
        For Each drone As DroneBayItem In currentShip.DroneBayItems.Values
            If drone.IsActive = True Then
                fitting.AppendLine(drone.DroneType.Name & ", " & drone.Quantity & "a")
            Else
                fitting.AppendLine(drone.DroneType.Name & ", " & drone.Quantity & "i")
            End If
        Next
        fitting.AppendLine("")
        For Each cargo As CargoBayItem In currentship.CargoBayItems.Values
            fitting.AppendLine(cargo.ItemType.Name & ", " & cargo.Quantity)
        Next
        Clipboard.SetText(fitting.ToString)
    End Sub

    Private Sub mnuCopyForEFT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyForEFT.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim currentship As Ship = currentShipSlot.ShipFitted
        Dim cModule As New ShipModule
        Dim fitting As New System.Text.StringBuilder
        fitting.AppendLine("[" & tp.Text & "]")
        For slot As Integer = 1 To currentship.LowSlots
            If currentship.LowSlot(slot) IsNot Nothing Then
                If currentship.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.LowSlot(slot).Name & ", " & currentship.LowSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.LowSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentship.MidSlots
            If currentship.MidSlot(slot) IsNot Nothing Then
                If currentship.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.MidSlot(slot).Name & ", " & currentship.MidSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.MidSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentship.HiSlots
            If currentship.HiSlot(slot) IsNot Nothing Then
                If currentship.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.HiSlot(slot).Name & ", " & currentship.HiSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.HiSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For slot As Integer = 1 To currentship.RigSlots
            If currentship.RigSlot(slot) IsNot Nothing Then
                If currentship.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    fitting.AppendLine(currentship.RigSlot(slot).Name & ", " & currentship.RigSlot(slot).LoadedCharge.Name)
                Else
                    fitting.AppendLine(currentship.RigSlot(slot).Name)
                End If
            End If
        Next
        fitting.AppendLine("")
        For Each drone As DroneBayItem In currentship.DroneBayItems.Values
            fitting.AppendLine(drone.DroneType.Name & " x" & drone.Quantity)
        Next
        Clipboard.SetText(fitting.ToString)
    End Sub

    Private Sub mnuCopyForForums_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuCopyForForums.Click
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim currentship As Ship = currentShipSlot.ShipFitted
        Dim slots As Dictionary(Of String, Integer)
        Dim slotList As New ArrayList
        Dim slotCount As Integer = 0
        Dim cModule As New ShipModule
        Dim state As Integer
        Dim fitting As New System.Text.StringBuilder

        slots = New Dictionary(Of String, Integer)
        For slot As Integer = 1 To currentship.HiSlots
            If currentship.HiSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.HiSlot(slot).ModuleState) / Math.Log(2))
                If currentship.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.HiSlot(slot).Name) = True Then
                        slotCount = slots(currentship.HiSlot(slot).Name)
                        slots(currentship.HiSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.HiSlot(slot).Name)
                        slots.Add(currentship.HiSlot(slot).Name, 1)
                    End If
                End If
            End If
        Next
        If slots.Count > 0 Then
            For Each cMod As String In slots.Keys
                If CInt(slots(cMod)) > 1 Then
                    fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                Else
                    fitting.AppendLine(cMod)
                End If
            Next
        End If

        slots = New Dictionary(Of String, Integer)
        For slot As Integer = 1 To currentship.MidSlots
            If currentship.MidSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.MidSlot(slot).ModuleState) / Math.Log(2))
                If currentship.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.MidSlot(slot).Name) = True Then
                        slotCount = slots(currentship.MidSlot(slot).Name)
                        slots(currentship.MidSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.MidSlot(slot).Name)
                        slots.Add(currentship.MidSlot(slot).Name, 1)
                    End If
                End If
            End If
        Next
        If slots.Count > 0 Then
            fitting.AppendLine("")
            For Each cMod As String In slots.Keys
                If CInt(slots(cMod)) > 1 Then
                    fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                Else
                    fitting.AppendLine(cMod)
                End If
            Next
        End If

        slots = New Dictionary(Of String, Integer)
        For slot As Integer = 1 To currentship.LowSlots
            If currentship.LowSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.LowSlot(slot).ModuleState) / Math.Log(2))
                If currentship.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.LowSlot(slot).Name) = True Then
                        slotCount = slots(currentship.LowSlot(slot).Name)
                        slots(currentship.LowSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.LowSlot(slot).Name)
                        slots.Add(currentship.LowSlot(slot).Name, 1)
                    End If
                End If
            End If
        Next
        If slots.Count > 0 Then
            fitting.AppendLine("")
            For Each cMod As String In slots.Keys
                If CInt(slots(cMod)) > 1 Then
                    fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                Else
                    fitting.AppendLine(cMod)
                End If
            Next
        End If


        slots = New Dictionary(Of String, Integer)
        For slot As Integer = 1 To currentship.RigSlots
            If currentship.RigSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.RigSlot(slot).ModuleState) / Math.Log(2))
                If currentship.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.RigSlot(slot).Name) = True Then
                        slotCount = slots(currentship.RigSlot(slot).Name)
                        slots(currentship.RigSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.RigSlot(slot).Name)
                        slots.Add(currentship.RigSlot(slot).Name, 1)
                    End If
                End If
            End If
        Next
        If slots.Count > 0 Then
            fitting.AppendLine("")
            For Each cMod As String In slots.Keys
                If CInt(slots(cMod)) > 1 Then
                    fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                Else
                    fitting.AppendLine(cMod)
                End If
            Next
        End If

        If currentship.DroneBayItems.Count > 0 Then
            fitting.AppendLine("")
            For Each drone As DroneBayItem In currentship.DroneBayItems.Values
                fitting.AppendLine(drone.Quantity & "x " & drone.DroneType.Name)
            Next
        End If

        If currentship.CargoBayItems.Count > 0 Then
            fitting.AppendLine("")
            For Each cargo As CargoBayItem In currentship.CargoBayItems.Values
                fitting.AppendLine(cargo.Quantity & "x " & cargo.ItemType.Name & " (cargo)")
            Next
        End If

        Clipboard.SetText(fitting.ToString)
    End Sub

#End Region

    Private Sub btnPilotManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPilotManager.Click
        Dim myPilotManager As New frmPilotManager
        myPilotManager.ShowDialog()
        myPilotManager = Nothing
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        Call Me.OpenSettingsForm()
    End Sub

    Private Sub PilotManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PilotManagerToolStripMenuItem.Click
        Dim myPilotManager As New frmPilotManager
        myPilotManager.ShowDialog()
        myPilotManager = Nothing
    End Sub

    Private Sub mnuEFTImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuEFTImport.Click
        Dim myEFTImport As New frmEFTImport
        myEFTImport.ShowDialog()
        myEFTImport = Nothing
        Call Me.UpdateFittingsTree()
    End Sub
    Private Sub OpenSettingsForm()
        ' Open options form
        Dim mySettings As New frmHQFSettings
        mySettings.ShowDialog()
        mySettings = Nothing
        Call Me.UpdateFittingsTree()
        Call Me.CheckOpenTabs()
    End Sub
    Private Sub CheckOpenTabs()
        ' Checks whether the open tabs are still valid fittings
        For Each tp As TabPage In tabHQF.TabPages
            If Fittings.FittingTabList.Contains(tp.Text) = False Then
                ShipLists.fittedShipList.Remove(tp.Text)
                tabHQF.TabPages.Remove(tp)
            End If
        Next
    End Sub

End Class