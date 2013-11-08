' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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

Imports System.IO
Imports EveHQ.HQF.Classes
Imports EveHQ.HQF
Imports System.Data.SqlClient
Imports ProtoBuf
Imports EveHQ.EveData
Imports QiHe.Yaml.Grammar
Imports System.Data.SQLite

Public Class FrmCacheCreator

    Private Const CacheFolderName As String = "StaticData"
    Private Const StaticDB As String = "EveHQMaster"
    Private Const StaticDBConnection As String = "Server=localhost\SQLEXPRESS; Database = " & StaticDB & "; Integrated Security = SSPI;" ' For SDE connection
    ReadOnly _sqLiteDBFolder As String = Path.Combine(Application.StartupPath, "EveCache")
    ReadOnly _sqLiteDB As String = Path.Combine(_sqLiteDBFolder, "EveHQMaster.db3")

    Shared _marketGroupData As DataSet
    Shared _shipGroupData As DataSet
    Shared _shipNameData As DataSet
    Shared _moduleData As DataSet
    Shared _moduleEffectData As DataSet
    Shared _moduleAttributeData As DataSet
    Shared _coreCacheFolder As String
    Shared _yamlTypes As SortedList(Of Integer, YAMLType) ' Key = typeID
    Shared _yamlIcons As SortedList(Of Integer, String) ' Key = iconID, Value = iconFile

   Private Sub frmCacheCreator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.FileSystem.DirectoryExists(_sqLiteDBFolder) = False Then
            My.Computer.FileSystem.CreateDirectory(_sqLiteDBFolder)
        End If
        If My.Computer.FileSystem.FileExists(_sqLiteDB) = False Then
            SQLiteConnection.CreateFile(_sqLiteDB)
        End If
    End Sub

    Private Sub btnGenerateCache_Click(sender As Object, e As EventArgs) Handles btnGenerateCache.Click

        ' Check for existence of a core cache folder in the application directory
        _coreCacheFolder = Path.Combine(Application.StartupPath, CacheFolderName)
        If My.Computer.FileSystem.DirectoryExists(_coreCacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(_coreCacheFolder)
        End If

        ' Parse the YAML files
        Call ParseYAMLFiles()

        ' Create and write core cache data
        Call LoadAllData()
        Call CreateCoreCache()

        ' Create and write HQF cache data
        Call GenerateHQFCacheData()

        MessageBox.Show("Creation of cache data complete!")

    End Sub

#Region "YAML Parsing Routines"

    Private Sub ParseYAMLFiles()

        _yamlTypes = New SortedList(Of Integer, YAMLType)
        _yamlIcons = New SortedList(Of Integer, String)

        ' Parse the icons file first
        Call ParseIconsYAMLFile()
        Call ParseTypesYAMLFile()

    End Sub

    Private Sub ParseIconsYAMLFile()
        Dim parser As New YamlParser
        Dim textData As String = System.Text.Encoding.UTF8.GetString(My.Resources.iconIDs)
        Dim input As New TextInput(textData)
        Dim success As Boolean = False
        Dim yamlData As YamlStream = parser.ParseYamlStream(input, success)
        If success Then
            ' Should only be 1 document so go with the first
            Dim yamlDoc As YamlDocument = yamlData.Documents(0)
            ' Cycle through the keys, which will be the typeIDs
            Dim root As Mapping = CType(yamlDoc.Root, Mapping)
            For Each entry As MappingEntry In root.Entries
                ' Parse the typeID
                Dim iconID As Integer = CInt(CType(entry.Key, Scalar).Text)
                ' Parse anything underneath
                For Each subEntry As MappingEntry In CType(entry.Value, Mapping).Entries
                    ' Get the key and value of th sub entry
                    Dim keyName As String = CType(subEntry.Key, Scalar).Text
                    ' Do something based on the key
                    Select Case keyName
                        Case "iconFile"
                            ' Pre-process the icon name to make it easier later on
                            Dim iconName As String = CType(subEntry.Value, Scalar).Text.Trim
                            ' Get the filename if the fullname starts with "res:"
                            If iconName.StartsWith("res", StringComparison.Ordinal) Then
                                iconName = iconName.Split("/".ToCharArray).Last
                            End If
                            ' Set the icon item
                            _yamlIcons.Add(iconID, iconName)
                    End Select
                Next
            Next
        End If
    End Sub

    Private Sub ParseTypesYAMLFile()
        Dim parser As New YamlParser
        Dim textData As String = System.Text.Encoding.UTF8.GetString(My.Resources.typeIDs)
        Dim input As New TextInput(textData)
        Dim success As Boolean = False
        Dim yamlData As YamlStream = parser.ParseYamlStream(input, success)

        ' Note: If success is false, check the YAML file for invalid characters (was ID 1002 in Retribution 1.1)

        If success Then
            ' Should only be 1 document so go with the first
            Dim yamlDoc As YamlDocument = yamlData.Documents(0)
            ' Cycle through the keys, which will be the typeIDs
            Dim root As Mapping = CType(yamlDoc.Root, Mapping)
            For Each entry As MappingEntry In root.Entries
                ' Parse the typeID
                Dim typeID As Integer = CInt(CType(entry.Key, Scalar).Text)
                Dim yamlItem As New YAMLType
                yamlItem.TypeID = typeID
                ' Parse anything underneath
                Dim dataItem = TryCast(entry.Value, Mapping)
                If dataItem IsNot Nothing Then
                    For Each subEntry As MappingEntry In dataItem.Entries
                        ' Get the key and value of th sub entry
                        Dim keyName As String = CType(subEntry.Key, Scalar).Text
                        ' Do something based on the key
                        Select Case keyName
                            Case "iconID"
                                ' Set the icon item
                                yamlItem.IconID = CInt(CType(subEntry.Value, Scalar).Text)
                        End Select
                    Next
                End If
                ' Get the iconFile if relevant
                If _yamlIcons.ContainsKey(yamlItem.IconID) Then
                    yamlItem.IconName = _yamlIcons(yamlItem.IconID)
                End If
                ' Add the item
                _yamlTypes.Add(yamlItem.TypeID, yamlItem)
            Next
        End If
    End Sub

#End Region

#Region "Core Generation Routines"

    Private Sub LoadAllData()

        Call LoadItemData()
        Call LoadMarketGroups()
        Call LoadItemMarketGroups()
        Call LoadItemList()
        Call LoadItemCategories()
        Call LoadItemGroups()
        Call LoadGroupCats()

        Call LoadCertCategories()
        Call LoadCertClasses()
        Call LoadCerts()
        Call LoadCertRecs()

        Call LoadUnlocks() ' Populates 4 data classes here

        Call LoadRegions()
        Call LoadConstellations()
        Call LoadSolarSystems()
        Call LoadStations()
        Call LoadAgents()

        Call LoadAttributeTypes()
        Call LoadTypeAttributes()
        Call LoadUnits()
        Call LoadEffectTypes()
        Call LoadTypeEffects()

        Call LoadMetaGroups()
        Call LoadMetaTypes()

        Call LoadBlueprints()
        Call LoadAssemblyArrays()
        Call LoadNPCCorps()
        Call LoadItemFlags()

    End Sub

    Private Sub LoadItemData()

        StaticData.Types.Clear()
        Dim evehqData As DataSet
        Dim strSQL As String = "SELECT invTypes.*, invGroups.categoryID FROM invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID;"
        evehqData = GetStaticData(strSQL)
        Dim newItem As EveType
        For Each itemRow As DataRow In evehqData.Tables(0).Rows
            If IsDBNull(itemRow.Item("typeName")) = False Then
                newItem = New EveType
                newItem.ID = CInt(itemRow.Item("typeID"))
                newItem.Name = CStr(itemRow.Item("typeName"))
                If IsDBNull(itemRow.Item("description")) = False Then
                    newItem.Description = CStr(itemRow.Item("description"))
                Else
                    newItem.Description = ""
                End If
                newItem.Group = CInt(itemRow.Item("groupID"))
                newItem.Published = CBool(itemRow.Item("published"))
                newItem.Category = CInt(itemRow.Item("categoryID"))
                If IsDBNull(itemRow.Item("marketGroupID")) = False Then
                    newItem.MarketGroupID = CInt(itemRow.Item("marketGroupID"))
                Else
                    newItem.MarketGroupID = 0
                End If
                newItem.Mass = CDbl(itemRow.Item("mass"))
                newItem.Capacity = CDbl(itemRow.Item("capacity"))
                newItem.Volume = CDbl(itemRow.Item("volume"))
                newItem.PortionSize = CInt(itemRow.Item("portionSize"))
                newItem.BasePrice = CDbl(itemRow.Item("basePrice"))
                StaticData.Types.Add(newItem.Id, newItem)
            End If
        Next
        ' Get the MetaLevel data
        strSQL = "SELECT * FROM dgmTypeAttributes WHERE attributeID=633;"
        evehqData = GetStaticData(strSQL)
        If evehqData.Tables(0).Rows.Count > 0 Then
            For Each itemRow As DataRow In evehqData.Tables(0).Rows
                If StaticData.Types.ContainsKey(CInt(itemRow.Item("typeID"))) Then
                    newItem = StaticData.Types(CInt(itemRow.Item("typeID")))
                    If IsDBNull(itemRow.Item("valueInt")) = False Then
                        newItem.MetaLevel = CInt(itemRow.Item("valueInt"))
                    Else
                        newItem.MetaLevel = CInt(itemRow.Item("valueFloat"))
                    End If
                End If
            Next
        End If
        evehqData.Dispose()

    End Sub

    Private Sub LoadMarketGroups()

        StaticData.MarketGroups.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invMarketGroups;")
            If evehqData IsNot Nothing Then
                If evehqData.Tables(0).Rows.Count > 0 Then
                    For Each itemRow As DataRow In evehqData.Tables(0).Rows
                        Dim mg As New MarketGroup
                        mg.ID = CInt(itemRow.Item("marketGroupID"))
                        mg.Name = CStr(itemRow.Item("marketGroupName"))
                        If IsDBNull(itemRow.Item("parentGroupID")) = False Then
                            mg.ParentGroupID = CInt(itemRow.Item("parentGroupID"))
                        Else
                            mg.ParentGroupID = 0
                        End If
                        StaticData.MarketGroups.Add(mg.ID, mg)
                    Next
                End If
            End If
        End Using

    End Sub

    Private Sub LoadItemMarketGroups()

        StaticData.ItemMarketGroups.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT typeID, marketGroupID FROM invTypes WHERE marketGroupID IS NOT NULL;")
            If evehqData IsNot Nothing Then
                If evehqData.Tables(0).Rows.Count > 0 Then
                    For Each itemRow As DataRow In evehqData.Tables(0).Rows
                        StaticData.ItemMarketGroups.Add(itemRow.Item("typeID").ToString, itemRow.Item("marketGroupID").ToString)
                    Next
                End If
            End If
        End Using

    End Sub

    Private Sub LoadItemList()

        StaticData.TypeNames.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invTypes ORDER BY typeName;")
            Dim iKey As String
            Dim iValue As String
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                iKey = evehqData.Tables(0).Rows(item).Item("typeName").ToString.Trim
                iValue = evehqData.Tables(0).Rows(item).Item("typeID").ToString.Trim
                If StaticData.TypeNames.ContainsKey(iKey) = False Then
                    StaticData.TypeNames.Add(iKey, CInt(iValue))
                End If
            Next
        End Using

    End Sub

    Private Sub LoadItemCategories()

        StaticData.TypeCats.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invCategories ORDER BY categoryName;")
            Dim iKey As Integer
            Dim iValue As String
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                iValue = evehqData.Tables(0).Rows(item).Item("categoryName").ToString.Trim
                iKey = CInt(evehqData.Tables(0).Rows(item).Item("categoryID").ToString.Trim)
                StaticData.TypeCats.Add(iKey, iValue)
            Next
        End Using

    End Sub

    Private Sub LoadItemGroups()

        StaticData.TypeGroups.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invGroups ORDER BY groupName;")
            Dim iKey As Integer
            Dim iValue As String
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                iValue = evehqData.Tables(0).Rows(item).Item("groupName").ToString.Trim
                iKey = CInt(evehqData.Tables(0).Rows(item).Item("groupID").ToString.Trim)
                StaticData.TypeGroups.Add(iKey, iValue)
            Next
        End Using

    End Sub

    Private Sub LoadGroupCats()

        StaticData.GroupCats.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invGroups ORDER BY groupName;")
            Dim iKey As Integer
            Dim iValue As Integer
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                iKey = CInt(evehqData.Tables(0).Rows(item).Item("groupID").ToString.Trim)
                iValue = CInt(evehqData.Tables(0).Rows(item).Item("categoryID").ToString.Trim)
                StaticData.GroupCats.Add(iKey, iValue)
            Next
        End Using

    End Sub

    Private Sub LoadCertCategories()

        StaticData.CertificateCategories.Clear()
        Const strSQL As String = "SELECT * FROM crtCategories;"
        Using evehqData As DataSet = GetStaticData(strSQL)
            For Each certRow As DataRow In evehqData.Tables(0).Rows
                Dim newCat As New CertificateCategory
                newCat.ID = CInt(certRow.Item("categoryID"))
                newCat.Name = certRow.Item("categoryName").ToString
                StaticData.CertificateCategories.Add(newCat.ID.ToString, newCat)
            Next
        End Using

    End Sub

    Private Sub LoadCertClasses()

        StaticData.CertificateClasses.Clear()
        Const strSQL As String = "SELECT * FROM crtClasses;"
        Using evehqData As DataSet = GetStaticData(strSQL)
            For Each certRow As DataRow In evehqData.Tables(0).Rows
                Dim newClass As New CertificateClass
                newClass.ID = CInt(certRow.Item("classID"))
                newClass.Name = certRow.Item("className").ToString
                StaticData.CertificateClasses.Add(newClass.ID.ToString, newClass)
            Next
        End Using

    End Sub

    Private Sub LoadCerts()

        StaticData.Certificates.Clear()
        Dim evehqData As DataSet
        evehqData = GetStaticData("SELECT * FROM crtCertificates;")
        For Each certRow As DataRow In evehqData.Tables(0).Rows
            Dim newCert As New Certificate
            newCert.ID = CInt(certRow.Item("certificateID"))
            newCert.CategoryID = CInt(certRow.Item("categoryID"))
            newCert.ClassID = CInt(certRow.Item("classID"))
            newCert.Description = CStr(certRow.Item("description"))
            newCert.Grade = CInt(certRow.Item("grade"))
            newCert.CorpID = CInt(certRow.Item("corpID"))
            StaticData.Certificates.Add(newCert.Id, newCert)
        Next

        evehqData = GetStaticData("SELECT * FROM crtRelationships;")
        For Each certRow As DataRow In evehqData.Tables(0).Rows
            Dim certID As Integer = CInt(certRow.Item("childID"))
            If StaticData.Certificates.ContainsKey(certID) = True Then
                Dim newCert As Certificate = StaticData.Certificates(certID)
                If IsDBNull(certRow.Item("parentID")) Then
                    ' This is a skill ID
                    newCert.RequiredSkills.Add(CInt(certRow.Item("parentTypeID")), CInt(certRow.Item("parentLevel")))
                Else
                    ' This is a certID
                    newCert.RequiredCertificates.Add(CInt(certRow.Item("parentID")), 1)
                End If
            End If
        Next

        evehqData.Dispose()

    End Sub

    Private Sub LoadCertRecs()

        StaticData.CertificateRecommendations.Clear()
        Const strSQL As String = "SELECT * FROM crtRecommendations;"
        Using evehqData As DataSet = GetStaticData(strSQL)
            For Each certRow As DataRow In evehqData.Tables(0).Rows
                Dim certRec As New CertificateRecommendation
                certRec.ShipTypeID = CInt(certRow.Item("shiptypeID").ToString)
                certRec.CertificateID = CInt(certRow.Item("certificateID").ToString)
                StaticData.CertificateRecommendations.Add(certRec)
            Next
        End Using

    End Sub

    Private Sub LoadUnlocks()

        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID AS invTypeID, invTypes.groupID, invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, invTypes.published"
        strSQL &= " FROM invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID"
        strSQL &= " WHERE (((dgmTypeAttributes.attributeID) IN (182,183,184,277,278,279,1285,1286,1287,1288,1289,1290)) AND (invTypes.published=1))"
        strSQL &= " ORDER BY invTypes.typeID, dgmTypeAttributes.attributeID;"
        Dim lastAtt As String = "0"
        Dim skillIDLevel As String
        Dim itemList As New ArrayList
        Dim attValue As Double
        Using evehqData As DataSet = GetStaticData(strSQL)
            For row As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                If evehqData.Tables(0).Rows(row).Item("invTypeID").ToString <> lastAtt Then
                    Dim attRows() As DataRow = evehqData.Tables(0).Select("invTypeID=" & evehqData.Tables(0).Rows(row).Item("invtypeID").ToString)
                    Const maxPreReqs As Integer = 10
                    Dim preReqSkills(maxPreReqs) As String
                    Dim preReqSkillLevels(maxPreReqs) As Integer
                    For Each attRow As DataRow In attRows
                        If IsDBNull(attRow.Item("valueInt")) = False Then
                            attValue = CDbl(attRow.Item("valueInt"))
                        Else
                            attValue = CDbl(attRow.Item("valueFloat"))
                        End If
                        Select Case CInt(attRow.Item("attributeID"))
                            Case 182
                                preReqSkills(1) = CStr(attValue)
                            Case 183
                                preReqSkills(2) = CStr(attValue)
                            Case 184
                                preReqSkills(3) = CStr(attValue)
                            Case 1285
                                preReqSkills(4) = CStr(attValue)
                            Case 1289
                                preReqSkills(5) = CStr(attValue)
                            Case 1290
                                preReqSkills(6) = CStr(attValue)
                            Case 277
                                preReqSkillLevels(1) = CInt(attValue)
                            Case 278
                                preReqSkillLevels(2) = CInt(attValue)
                            Case 279
                                preReqSkillLevels(3) = CInt(attValue)
                            Case 1286
                                preReqSkillLevels(4) = CInt(attValue)
                            Case 1287
                                preReqSkillLevels(5) = CInt(attValue)
                            Case 1288
                                preReqSkillLevels(6) = CInt(attValue)
                        End Select
                    Next
                    For prereq As Integer = 1 To maxPreReqs
                        If preReqSkills(prereq) <> "" Then
                            skillIDLevel = preReqSkills(prereq) & "." & preReqSkillLevels(prereq).ToString
                            itemList.Add(skillIDLevel & "_" & evehqData.Tables(0).Rows(row).Item("invtypeID").ToString & "_" & evehqData.Tables(0).Rows(row).Item("groupID").ToString)
                        End If
                    Next
                    lastAtt = CStr(evehqData.Tables(0).Rows(row).Item("invtypeID"))
                End If
            Next

            ' Place the items into the Shared arrays
            Dim items(2) As String
            Dim itemUnlocked As List(Of String)
            Dim certUnlocked As List(Of Integer)
            StaticData.SkillUnlocks.Clear()
            StaticData.ItemUnlocks.Clear()
            For Each item As String In itemList
                items = item.Split(CChar("_"))
                If StaticData.SkillUnlocks.ContainsKey(items(0)) = False Then
                    ' Create an arraylist and add the item
                    itemUnlocked = New List(Of String)
                    itemUnlocked.Add(items(1) & "_" & items(2))
                    StaticData.SkillUnlocks.Add(items(0), itemUnlocked)
                Else
                    ' Fetch the item and add the new one
                    itemUnlocked = StaticData.SkillUnlocks(items(0))
                    itemUnlocked.Add(items(1) & "_" & items(2))
                End If
                If StaticData.ItemUnlocks.ContainsKey(items(1)) = False Then
                    ' Create an arraylist and add the item
                    itemUnlocked = New List(Of String)
                    itemUnlocked.Add(items(0))
                    StaticData.ItemUnlocks.Add(items(1), itemUnlocked)
                Else
                    ' Fetch the item and add the new one
                    itemUnlocked = StaticData.ItemUnlocks(items(1))
                    itemUnlocked.Add(items(0))
                End If
            Next

            ' Add certificates into the skill unlocks?
            For Each cert As Certificate In StaticData.Certificates.Values
                For Each skill As Integer In cert.RequiredSkills.Keys
                    Dim skillID As String = skill & "." & cert.RequiredSkills(skill).ToString
                    If StaticData.CertUnlockSkills.ContainsKey(skillID) = False Then
                        ' Create an arraylist and add the item
                        certUnlocked = New List(Of Integer)
                        certUnlocked.Add(cert.Id)
                        StaticData.CertUnlockSkills.Add(skillID, certUnlocked)
                    Else
                        ' Fetch the item and add the new one
                        certUnlocked = StaticData.CertUnlockSkills(skillID)
                        certUnlocked.Add(cert.Id)
                    End If
                Next
                For Each certID As Integer In cert.RequiredCertificates.Keys
                    If StaticData.CertUnlockCertificates.ContainsKey(certID) = False Then
                        ' Create an arraylist and add the item
                        certUnlocked = New List(Of Integer)
                        certUnlocked.Add(cert.Id)
                        StaticData.CertUnlockCertificates.Add(certID, certUnlocked)
                    Else
                        ' Fetch the item and add the new one
                        certUnlocked = StaticData.CertUnlockCertificates(certID)
                        certUnlocked.Add(cert.Id)
                    End If
                Next
            Next

        End Using

    End Sub

    Private Sub LoadRegions()

        StaticData.Regions.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM mapRegions;")
            For Each row As DataRow In evehqData.Tables(0).Rows
                StaticData.Regions.Add(CInt(row.Item("regionID")), row.Item("regionName").ToString)
            Next
        End Using

    End Sub

    Private Sub LoadConstellations()

        StaticData.Constellations.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM mapConstellations;")
            For Each row As DataRow In evehqData.Tables(0).Rows
                StaticData.Constellations.Add(CInt(row.Item("constellationID")), row.Item("constellationName").ToString)
            Next
        End Using

    End Sub

    Private Sub LoadSolarSystems()

        StaticData.SolarSystems.Clear()
        Dim strSQL As String = "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName"
        strSQL &= " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;"
        Using evehqData As DataSet = GetStaticData(strSQL)
            Dim cSystem As SolarSystem
            For Each solarRow As DataRow In evehqData.Tables(0).Rows
                cSystem = New SolarSystem
                cSystem.ID = CInt(solarRow.Item("solarSystemID"))
                cSystem.Name = CStr(solarRow.Item("solarSystemName"))
                cSystem.Security = CDbl(solarRow.Item("security"))
                cSystem.RegionID = CInt(solarRow.Item("mapSolarSystems_regionID"))
                cSystem.ConstellationID = CInt(solarRow.Item("mapSolarSystems_constellationID"))
                cSystem.X = CDbl(solarRow.Item("x"))
                cSystem.Y = CDbl(solarRow.Item("y"))
                cSystem.Z = CDbl(solarRow.Item("z"))
                StaticData.SolarSystems.Add(cSystem.ID, cSystem)
            Next
        End Using

        ' Load the solar system jump data
        Using connection As New SqlConnection(StaticDBConnection)

            Dim command As SqlCommand = New SqlCommand("SELECT * FROM mapSolarSystemJumps;", connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                Do While reader.Read()
                    If StaticData.SolarSystems.ContainsKey(CInt(reader.Item("fromSolarSystemID"))) Then
                        StaticData.SolarSystems(CInt(reader.Item("fromSolarSystemID"))).Gates.Add(CInt(reader.Item("toSolarSystemID")))
                    End If
                Loop
            End If

            reader.Close()

        End Using

        ' Load the celestial data
        Using connection As New SqlConnection(StaticDBConnection)

            Dim id As Integer
            Dim command As SqlCommand = New SqlCommand("SELECT * FROM mapDenormalize;", connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                Do While reader.Read()

                    If IsDBNull(reader.Item("solarSystemID")) = False Then
                        id = CInt(reader.Item("solarSystemID"))
                        If StaticData.SolarSystems.ContainsKey(id) Then
                            Select Case CInt(reader.Item("groupID"))
                                Case 7 ' Planet
                                    'MapData.eveSystems(id).Planets.Add(reader.Item("itemName").ToString)
                                    StaticData.SolarSystems(id).PlanetCount += 1
                                Case 8 ' Moon
                                    'MapData.eveSystems(id).Moons.Add(reader.Item("itemName").ToString)
                                    StaticData.SolarSystems(id).MoonCount += 1
                                Case 9 ' Belts
                                    Select Case CInt(reader.Item("typeID"))
                                        Case 15 ' Ore Belt
                                            'MapData.eveSystems(id).OreBelts.Add(reader.Item("itemName").ToString)
                                            StaticData.SolarSystems(id).OreBeltCount += 1
                                        Case 17774 ' Ice Belt
                                            'MapData.eveSystems(id).IceBelts.Add(reader.Item("itemName").ToString)
                                            StaticData.SolarSystems(id).IceBeltCount += 1
                                    End Select
                                Case 15 ' Stations
                                    'MapData.eveSystems(id).Stations.Add(reader.Item("itemName").ToString)
                                    StaticData.SolarSystems(id).StationCount += 1
                            End Select
                        End If
                    End If

                Loop
            End If

            reader.Close()

        End Using

    End Sub

    Private Sub LoadStations()

        ' Load the Operation data
        Dim operationServices As New Dictionary(Of Integer, Integer)
        Using connection As New SqlConnection(StaticDBConnection)

            Dim command As SqlCommand = New SqlCommand("SELECT * FROM staOperationServices;", connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                Do While reader.Read()
                    If operationServices.ContainsKey(CInt(reader.Item("operationID"))) = False Then
                        operationServices.Add(CInt(reader.Item("operationID")), 0)
                    End If
                    operationServices(CInt(reader.Item("operationID"))) += CInt(reader.Item("serviceID"))
                Loop
            End If

            reader.Close()

        End Using

        ' Load the Station data
        Using connection As New SqlConnection(StaticDBConnection)

            Dim command As SqlCommand = New SqlCommand("SELECT * FROM staStations;", connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                Dim s As Station
                Do While reader.Read()
                    s = New Station
                    s.StationID = CInt(reader.Item("stationID"))
                    s.StationName = reader.Item("stationName").ToString
                    s.CorpID = CInt(reader.Item("corporationID"))
                    s.SystemID = CInt(reader.Item("solarSystemID"))
                    s.RefiningEfficiency = CDbl(reader.Item("reprocessingEfficiency"))
                    s.StationTake = CDbl(reader.Item("reprocessingStationsTake"))
                    s.Services = operationServices(CInt(reader.Item("operationID")))
                    StaticData.Stations.Add(s.StationID, s)
                Loop
            End If

            reader.Close()

        End Using

    End Sub

    Private Sub LoadAgents()

        ' Load the NPC Division data
        Using connection As New SqlConnection(StaticDBConnection)

            Dim command As SqlCommand = New SqlCommand("SELECT * FROM crpNPCDivisions;", connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                Do While reader.Read()
                    StaticData.Divisions.Add(CInt(reader.Item("divisionID")), reader.Item("divisionName").ToString)
                Loop
            End If

            reader.Close()

        End Using

        ' Load the Agent data
        Using connection As New SqlConnection(StaticDBConnection)

            Dim command As SqlCommand = New SqlCommand("SELECT agtAgents.agentID, agtAgents.divisionID, agtAgents.corporationID, agtAgents.locationID, agtAgents.[level], agtAgents.quality, agtAgents.agentTypeID, agtAgents.isLocator, invUniqueNames.itemName AS agentName FROM agtAgents INNER JOIN invUniqueNames ON agtAgents.agentID = invUniqueNames.itemID;", connection)
            connection.Open()

            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                Dim a As Agent
                Do While reader.Read()
                    a = New Agent
                    a.AgentID = CInt(reader.Item("agentID"))
                    a.AgentName = reader.Item("agentName").ToString
                    a.AgentType = CInt(reader.Item("agentTypeID"))
                    a.CorporationID = CInt(reader.Item("corporationID"))
                    a.DivisionID = CInt(reader.Item("divisionID"))
                    a.IsLocator = CBool(reader.Item("isLocator"))
                    a.Level = CInt(reader.Item("level"))
                    a.LocationID = CInt(reader.Item("locationID"))
                    StaticData.Agents.Add(a.AgentID, a)
                Loop
            End If

            reader.Close()

        End Using

    End Sub

    Private Sub LoadAttributeTypes()

        StaticData.AttributeTypes.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM dgmAttributeTypes;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim at As New AttributeType
                at.AttributeID = CInt(evehqData.Tables(0).Rows(item).Item("attributeID"))
                at.AttributeName = CStr(evehqData.Tables(0).Rows(item).Item("attributeName")).Trim
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("displayName")) = False Then
                    at.DisplayName = CStr(evehqData.Tables(0).Rows(item).Item("displayName")).Trim
                Else
                    at.DisplayName = at.AttributeName
                End If
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("unitID")) = False Then
                    at.UnitID = CInt(evehqData.Tables(0).Rows(item).Item("unitID"))
                Else
                    at.UnitID = 0
                End If
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("categoryID")) = False Then
                    at.CategoryID = CInt(evehqData.Tables(0).Rows(item).Item("categoryID"))
                Else
                    at.CategoryID = 0
                End If
                StaticData.AttributeTypes.Add(at.AttributeID, at)
            Next
        End Using

    End Sub

    Private Sub LoadTypeAttributes()

        StaticData.TypeAttributes.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM dgmTypeAttributes;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim ta As New TypeAttrib
                ta.TypeID = CInt(evehqData.Tables(0).Rows(item).Item("typeID"))
                ta.AttributeID = CInt(evehqData.Tables(0).Rows(item).Item("attributeID"))
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("valueInt")) = False Then
                    ta.Value = CDbl(evehqData.Tables(0).Rows(item).Item("valueInt"))
                Else
                    ta.Value = CDbl(evehqData.Tables(0).Rows(item).Item("valueFloat"))
                End If
                StaticData.TypeAttributes.Add(ta)
            Next
        End Using

    End Sub

    Private Sub LoadUnits()

        StaticData.AttributeUnits.Clear()
        StaticData.AttributeUnits.Add(0, "")
        Using evehqData As DataSet = GetStaticData("SELECT * FROM eveUnits;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("displayName")) = False Then
                    StaticData.AttributeUnits.Add(CInt(evehqData.Tables(0).Rows(item).Item("unitID")), CStr(evehqData.Tables(0).Rows(item).Item("displayName")))
                Else
                    StaticData.AttributeUnits.Add(CInt(evehqData.Tables(0).Rows(item).Item("unitID")), "")
                End If
            Next
        End Using

    End Sub

    Private Sub LoadEffectTypes()

        StaticData.EffectTypes.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM dgmEffects;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim et As New EffectType
                et.EffectID = CInt(evehqData.Tables(0).Rows(item).Item("effectID"))
                et.EffectName = CStr(evehqData.Tables(0).Rows(item).Item("effectName")).Trim
                StaticData.EffectTypes.Add(et.EffectID, et)
            Next
        End Using

    End Sub

    Private Sub LoadTypeEffects()

        StaticData.TypeEffects.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM dgmTypeEffects;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim te As New TypeEffect
                te.TypeID = CInt(evehqData.Tables(0).Rows(item).Item("typeID"))
                te.EffectID = CInt(evehqData.Tables(0).Rows(item).Item("effectID"))
                StaticData.TypeEffects.Add(te)
            Next
        End Using

    End Sub

    Private Sub LoadMetaGroups()

        StaticData.MetaGroups.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invMetaGroups;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                StaticData.MetaGroups.Add(CInt(evehqData.Tables(0).Rows(item).Item("metaGroupID")), CStr(evehqData.Tables(0).Rows(item).Item("metaGroupName")))
            Next
        End Using

    End Sub

    Private Sub LoadMetaTypes()

        StaticData.MetaTypes.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invMetaTypes;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim mt As New MetaType
                mt.ID = CInt(evehqData.Tables(0).Rows(item).Item("typeID"))
                mt.ParentID = CInt(evehqData.Tables(0).Rows(item).Item("parentTypeID"))
                mt.MetaGroupID = CInt(evehqData.Tables(0).Rows(item).Item("metaGroupID"))
                StaticData.MetaTypes.Add(mt.ID, mt)
            Next
        End Using

    End Sub

    Private Sub LoadBlueprints()

        StaticData.Blueprints.Clear()
        Dim evehqData As DataSet = GetStaticData("SELECT * FROM invBlueprintTypes;")
        ' Populate the main data
        For Each bp As DataRow In evehqData.Tables(0).Rows
            Dim bt As New Blueprint
            bt.ID = CInt(bp.Item("blueprintTypeID"))
            bt.ProductID = CInt(bp.Item("productTypeID"))
            bt.TechLevel = CInt(bp.Item("techLevel"))
            bt.WasteFactor = CInt(bp.Item("wasteFactor"))
            bt.MaterialModifier = CInt(bp.Item("materialModifier"))
            bt.ProductivityModifier = CInt(bp.Item("productivityModifier"))
            bt.MaxProductionLimit = CInt(bp.Item("maxProductionLimit"))
            bt.ProductionTime = CLng(bp.Item("productionTime"))
            bt.ResearchMaterialLevelTime = CLng(bp.Item("researchMaterialTime"))
            bt.ResearchProductionLevelTime = CLng(bp.Item("researchProductivityTime"))
            bt.ResearchCopyTime = CLng(bp.Item("researchCopyTime"))
            bt.ResearchTechTime = CLng(bp.Item("researchTechTime"))
            StaticData.Blueprints.Add(bt.ID, bt)
        Next

        ' Good so far so let's add the material requirements
        evehqData = GetStaticData("SELECT invBuildMaterials.*, invTypes.typeName, invGroups.groupID, invGroups.categoryID FROM invGroups INNER JOIN (invTypes INNER JOIN invBuildMaterials ON invTypes.typeID = invBuildMaterials.requiredTypeID) ON invGroups.groupID = invTypes.groupID ORDER BY invBuildMaterials.typeID;")

        ' Go through each BP and refine the Dataset
        For Each bp As Blueprint In StaticData.Blueprints.Values
            ' Select resource data for the blueprint
            Dim bpRows() As DataRow = evehqData.Tables(0).Select("typeID=" & bp.ID.ToString)
            For Each req As DataRow In bpRows
                Dim newReq As New BlueprintResource
                newReq.Activity = CType(req.Item("activityID"), BlueprintActivity)
                newReq.DamagePerJob = CDbl(req.Item("damagePerJob"))
                newReq.TypeID = CInt(req.Item("requiredTypeID"))
                newReq.TypeGroup = CInt(req.Item("groupID"))
                newReq.TypeCategory = CInt(req.Item("categoryID"))
                newReq.Quantity = CInt(req.Item("quantity"))
                If IsDBNull(req.Item("baseMaterial")) = False Then
                    newReq.BaseMaterial = CInt(req.Item("baseMaterial"))
                Else
                    newReq.BaseMaterial = 0
                End If
                ' Create activity if required
                If bp.Resources.ContainsKey(newReq.Activity) = False Then
                    bp.Resources.Add(newReq.Activity, New Dictionary(Of Integer, BlueprintResource))
                End If
                If bp.Resources(newReq.Activity).ContainsKey(newReq.TypeID) = False Then
                    bp.Resources(newReq.Activity).Add(newReq.TypeID, newReq)
                End If
            Next
            ' Select resource data for the product
            If bp.ProductID <> bp.ID Then
                bpRows = evehqData.Tables(0).Select("typeID=" & bp.ProductID.ToString)
                For Each req As DataRow In bpRows
                    Dim newReq As New BlueprintResource
                    newReq.TypeID = CInt(req.Item("requiredTypeID"))
                    newReq.TypeGroup = CInt(req.Item("groupID"))
                    newReq.TypeCategory = CInt(req.Item("categoryID"))
                    newReq.Activity = CType(req.Item("activityID"), BlueprintActivity)
                    newReq.DamagePerJob = CDbl(req.Item("damagePerJob"))
                    newReq.Quantity = CInt(req.Item("quantity"))
                    If IsDBNull(req.Item("baseMaterial")) = False Then
                        newReq.BaseMaterial = CInt(req.Item("baseMaterial"))
                    Else
                        newReq.BaseMaterial = 0
                    End If
                    If bp.Resources.ContainsKey(newReq.Activity) = False Then
                        bp.Resources.Add(newReq.Activity, New Dictionary(Of Integer, BlueprintResource))
                    End If
                    If bp.Resources(newReq.Activity).ContainsKey(newReq.TypeID) = False Then
                        bp.Resources(newReq.Activity).Add(newReq.TypeID, newReq)
                    End If
                Next
            End If
        Next

        ' Fetch the relevant Invention Data
        Dim strSQL As String = "SELECT SourceBP.blueprintTypeID AS SBP, InventedBP.blueprintTypeID AS IBP"
        strSQL &= " FROM invBlueprintTypes AS SourceBP INNER JOIN"
        strSQL &= " invMetaTypes ON SourceBP.productTypeID = invMetaTypes.parentTypeID INNER JOIN"
        strSQL &= " invBlueprintTypes AS InventedBP ON invMetaTypes.typeID = InventedBP.productTypeID"
        strSQL &= " WHERE (invMetaTypes.metaGroupID = 2);"
        evehqData = GetStaticData(strSQL)
        For Each invRow As DataRow In evehqData.Tables(0).Rows
            ' Add the "Inventable" item
            If StaticData.Blueprints.ContainsKey(CInt(invRow.Item("SBP"))) Then
                StaticData.Blueprints(CInt(invRow.Item("SBP"))).Inventions.Add(CInt(invRow.Item("IBP")))
            End If
            ' Add the "Invented From" item
            If StaticData.Blueprints.ContainsKey(CInt(invRow.Item("IBP"))) Then
                StaticData.Blueprints(CInt(invRow.Item("IBP"))).InventFrom.Add(CInt(invRow.Item("SBP")))
            End If
        Next

        ' Load all the meta level data for invention
        strSQL = "SELECT invBlueprintTypes.blueprintTypeID, invMetaTypes.typeID, invMetaTypes.parentTypeID FROM invBlueprintTypes INNER JOIN"
        strSQL &= " invMetaTypes ON invBlueprintTypes.productTypeID = invMetaTypes.parentTypeID"
        strSQL &= " WHERE (techLevel = 1)"
        strSQL &= " ORDER BY parentTypeID ;"
        evehqData = GetStaticData(strSQL)
        For Each invRow As DataRow In evehqData.Tables(0).Rows
            If StaticData.Blueprints(CInt(invRow.Item("blueprintTypeID"))).InventionMetaItems.Contains(CInt(invRow.Item("parentTypeID"))) = False Then
                StaticData.Blueprints(CInt(invRow.Item("blueprintTypeID"))).InventionMetaItems.Add(CInt(invRow.Item("parentTypeID")))
            End If
            If StaticData.Types(CInt(invRow.Item("typeID"))).MetaLevel < 5 Then
                StaticData.Blueprints(CInt(invRow.Item("blueprintTypeID"))).InventionMetaItems.Add(CInt(invRow.Item("typeID")))
            End If
        Next

        evehqData.Dispose()

    End Sub

    Private Sub LoadAssemblyArrays()

        ' Get Data
        Const arraySQL As String = "SELECT * FROM ramAssemblyLineTypes WHERE activityID=1 AND (baseTimeMultiplier<>1 OR baseMaterialMultiplier<>1);"
        Const groupSQL As String = "SELECT * FROM ramAssemblyLineTypeDetailPerGroup;"
        Const catSQL As String = "SELECT * FROM ramAssemblyLineTypeDetailPerCategory;"
        Dim arrayDataSet As DataSet = GetStaticData(arraySQL)
        Dim groupDataSet As DataSet = GetStaticData(groupSQL)
        Dim catDataSet As DataSet = GetStaticData(catSQL)

        ' Reset the list
        StaticData.AssemblyArrays.Clear()

        ' Populate the arrays
        For Each assArray As DataRow In arrayDataSet.Tables(0).Rows
            Dim newArray As New AssemblyArray
            newArray.ID = CStr(assArray.Item("assemblyLineTypeID"))
            newArray.Name = CStr(assArray.Item("assemblyLineTypeName"))
            newArray.MaterialMultiplier = CDbl(assArray.Item("baseMaterialMultiplier"))
            newArray.TimeMultiplier = CDbl(assArray.Item("baseTimeMultiplier"))

            Dim groupRows() As DataRow = groupDataSet.Tables(0).Select("assemblyLineTypeID=" & newArray.ID)
            For Each group As DataRow In groupRows
                newArray.AllowableGroups.Add(CInt(group.Item("groupID")))
            Next
            Dim catRows() As DataRow = catDataSet.Tables(0).Select("assemblyLineTypeID=" & newArray.ID)
            For Each cat As DataRow In catRows
                newArray.AllowableCategories.Add(CInt(cat.Item("categoryID")))
            Next
            StaticData.AssemblyArrays.Add(newArray.Name.ToString, newArray)
        Next

        catDataSet.Dispose()
        groupDataSet.Dispose()
        arrayDataSet.Dispose()

    End Sub

    Private Sub LoadNPCCorps()

        StaticData.NPCCorps.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT itemID, itemName FROM invUniqueNames WHERE groupID=2;")
            For Each corpRow As DataRow In evehqData.Tables(0).Rows
                StaticData.NPCCorps.Add(CInt(corpRow.Item("itemID")), CStr(corpRow.Item("itemname")))
            Next
        End Using

    End Sub

    Private Sub LoadItemFlags()

        StaticData.ItemMarkers.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invFlags")
            For Each flagRow As DataRow In evehqData.Tables(0).Rows
                StaticData.ItemMarkers.Add(CInt(flagRow.Item("flagID")), CStr(flagRow.Item("flagText")))
            Next
        End Using

    End Sub

#End Region

#Region "Core Cache Writing Routines"

    Private Sub CreateCoreCache()

        ' Dump core data to the folder
        Dim s As FileStream

        ' Item Data
        s = New FileStream(Path.Combine(_coreCacheFolder, "Items.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Types)
        s.Flush()
        s.Close()

        ' Market Groups
        s = New FileStream(Path.Combine(_coreCacheFolder, "MarketGroups.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.MarketGroups)
        s.Flush()
        s.Close()

        ' Item Market Groups
        s = New FileStream(Path.Combine(_coreCacheFolder, "ItemMarketGroups.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.ItemMarketGroups)
        s.Flush()
        s.Close()

        ' Item List
        s = New FileStream(Path.Combine(_coreCacheFolder, "ItemList.dat"), FileMode.Create)

        Serializer.Serialize(s, StaticData.TypeNames)
        s.Flush()
        s.Close()

        ' Item Groups
        s = New FileStream(Path.Combine(_coreCacheFolder, "ItemGroups.dat"), FileMode.Create)

        Serializer.Serialize(s, StaticData.TypeGroups)
        s.Flush()
        s.Close()

        ' Items Cats
        s = New FileStream(Path.Combine(_coreCacheFolder, "ItemCats.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.TypeCats)
        s.Flush()
        s.Close()

        ' Group Cats
        s = New FileStream(Path.Combine(_coreCacheFolder, "GroupCats.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.GroupCats)
        s.Flush()
        s.Close()

        ' Cert Categories
        s = New FileStream(Path.Combine(_coreCacheFolder, "CertCats.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertificateCategories)
        s.Flush()
        s.Close()

        ' Cert Classes
        s = New FileStream(Path.Combine(_coreCacheFolder, "CertClasses.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertificateClasses)
        s.Flush()
        s.Close()

        ' Certs
        s = New FileStream(Path.Combine(_coreCacheFolder, "Certs.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Certificates)
        s.Flush()
        s.Close()

        ' Cert Recommendations
        s = New FileStream(Path.Combine(_coreCacheFolder, "CertRec.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertificateRecommendations)
        s.Flush()
        s.Close()

        ' Unlocks
        s = New FileStream(Path.Combine(_coreCacheFolder, "ItemUnlocks.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.ItemUnlocks)
        s.Flush()
        s.Close()

        ' SkillUnlocks
        s = New FileStream(Path.Combine(_coreCacheFolder, "SkillUnlocks.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.SkillUnlocks)
        s.Flush()
        s.Close()

        ' CertCerts
        s = New FileStream(Path.Combine(_coreCacheFolder, "CertCerts.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertUnlockCertificates)
        s.Flush()
        s.Close()

        ' CertSkills
        s = New FileStream(Path.Combine(_coreCacheFolder, "CertSkills.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertUnlockSkills)
        s.Flush()
        s.Close()

        ' Regions
        s = New FileStream(Path.Combine(_coreCacheFolder, "Regions.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Regions)
        s.Flush()
        s.Close()

        ' Constellations
        s = New FileStream(Path.Combine(_coreCacheFolder, "Constellations.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Constellations)
        s.Flush()
        s.Close()

        ' Solar Systems
        s = New FileStream(Path.Combine(_coreCacheFolder, "Systems.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.SolarSystems)
        s.Flush()
        s.Close()

        ' Stations
        s = New FileStream(Path.Combine(_coreCacheFolder, "Stations.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Stations)
        s.Flush()
        s.Close()

        ' Divisions
        s = New FileStream(Path.Combine(_coreCacheFolder, "Divisions.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Divisions)
        s.Flush()
        s.Close()

        ' Agents
        s = New FileStream(Path.Combine(_coreCacheFolder, "Agents.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Agents)
        s.Flush()
        s.Close()

        ' Attribute Types
        s = New FileStream(Path.Combine(_coreCacheFolder, "AttributeTypes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.AttributeTypes)
        s.Flush()
        s.Close()

        ' Type Attributes
        s = New FileStream(Path.Combine(_coreCacheFolder, "TypeAttributes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.TypeAttributes)
        s.Flush()
        s.Close()

        ' Attribute Units
        s = New FileStream(Path.Combine(_coreCacheFolder, "Units.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.AttributeUnits)
        s.Flush()
        s.Close()

        ' Effect Types
        s = New FileStream(Path.Combine(_coreCacheFolder, "EffectTypes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.EffectTypes)
        s.Flush()
        s.Close()

        ' Type Effects
        s = New FileStream(Path.Combine(_coreCacheFolder, "TypeEffects.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.TypeEffects)
        s.Flush()
        s.Close()

        ' Meta Groups
        s = New FileStream(Path.Combine(_coreCacheFolder, "MetaGroups.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.MetaGroups)
        s.Flush()
        s.Close()

        ' Meta Types
        s = New FileStream(Path.Combine(_coreCacheFolder, "MetaTypes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.MetaTypes)
        s.Flush()
        s.Close()

        ' Blueprint Types
        s = New FileStream(Path.Combine(_coreCacheFolder, "Blueprints.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Blueprints)
        s.Flush()
        s.Close()

        ' Assembly Arrays
        s = New FileStream(Path.Combine(_coreCacheFolder, "AssemblyArrays.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.AssemblyArrays)
        s.Flush()
        s.Close()

        ' NPC Corps
        s = New FileStream(Path.Combine(_coreCacheFolder, "NPCCorps.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.NPCCorps)
        s.Flush()
        s.Close()

        ' Item Flags
        s = New FileStream(Path.Combine(_coreCacheFolder, "ItemFlags.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.ItemMarkers)
        s.Flush()
        s.Close()

    End Sub

#End Region

#Region "HQF Generation Routines"

    Public Sub GenerateHQFCacheData()

        Call LoadAttributes()
        Call LoadSkillData()
        Call LoadShipGroupData()
        Call LoadMarketGroupData()
        Call LoadShipNameData()
        Call LoadShipAttributeData()
        Call PopulateShipLists()
        Call LoadModuleData()
        Call LoadModuleEffectData()
        Call LoadModuleAttributeData()
        Call LoadModuleMetaTypes()
        Call BuildModuleData()
        Call BuildAttributeQuickList()
        Call BuildModuleEffects()
        Call BuildImplantEffects()
        Call BuildShipEffects()
        Call BuildSubsystemEffects()
        Call BuildShipMarketGroups()
        Call BuildItemMarketGroups()
        Call SaveHQFCacheData()
        Call CleanUpData()

    End Sub

    Private Sub LoadMarketGroupData()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT * FROM invMarketGroups ORDER BY parentGroupID;"
            _marketGroupData = GetStaticData(strSQL)
            If _marketGroupData IsNot Nothing Then
                If _marketGroupData.Tables(0).Rows.Count <> 0 Then
                    Market.MarketGroupList.Clear()
                    For Each row As DataRow In _marketGroupData.Tables(0).Rows
                        Market.MarketGroupList.Add(row.Item("marketGroupID").ToString, row.Item("marketGroupName").ToString)
                    Next
                    Return
                Else
                    MessageBox.Show("Market Group Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Else
                MessageBox.Show("Market Group Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Market Group Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadAttributes()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT dgmAttributeTypes.attributeID, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName AS dgmAttributeTypes_displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, dgmAttributeTypes.attributeGroup, eveUnits.unitName, eveUnits.displayName AS eveUnits_displayName"
            strSQL &= " FROM eveUnits RIGHT OUTER JOIN dgmAttributeTypes ON eveUnits.unitID = dgmAttributeTypes.unitID"
            strSQL &= " ORDER BY dgmAttributeTypes.attributeID;"
            Dim attributeData As DataSet = GetStaticData(strSQL)
            If attributeData IsNot Nothing Then
                If attributeData.Tables(0).Rows.Count <> 0 Then
                    Attributes.AttributeList.Clear()
                    Dim attData As Attribute
                    For Each row As DataRow In attributeData.Tables(0).Rows
                        attData = New Attribute
                        attData.ID = CInt(row.Item("attributeID"))
                        attData.Name = row.Item("attributeName").ToString
                        attData.DisplayName = row.Item("dgmAttributeTypes_displayName").ToString
                        attData.UnitName = row.Item("eveUnits_displayName").ToString
                        attData.AttributeGroup = row.Item("attributeGroup").ToString
                        If attData.UnitName = "ms" Then
                            attData.UnitName = "s"
                        End If
                        Attributes.AttributeList.Add(attData.ID, attData)
                    Next
                    LoadCustomAttributes()
                    Return
                Else
                    MessageBox.Show("Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Else
                MessageBox.Show("Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Attribute Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadCustomAttributes()
        Dim attributeLines() As String = ResourceHandler.GetResource("Attributes").ToString.Split(ControlChars.CrLf.ToCharArray)
        Dim att() As String
        Dim attData As Attribute
        For Each line As String In attributeLines
            If line.Trim <> "" And line.StartsWith("#", StringComparison.Ordinal) = False Then
                att = line.Split(",".ToCharArray)
                attData = New Attribute
                attData.ID = CInt(att(0))
                attData.Name = att(1)
                attData.DisplayName = att(2)
                attData.UnitName = att(4)
                attData.AttributeGroup = att(5)
                Attributes.AttributeList.Add(attData.ID, attData)
            End If
        Next
        Return
    End Sub
    Private Sub LoadSkillData()
        'Call Core.SkillFunctions.LoadEveSkillData()
        Call LoadEveSkillData()
        Try
            Dim skillData As DataSet
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName,  invTypes.basePrice, invTypes.published, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
            strSQL &= " WHERE invCategories.categoryID=16 ORDER BY typeName;"
            skillData = GetStaticData(strSQL)
            If skillData IsNot Nothing Then
                If skillData.Tables(0).Rows.Count <> 0 Then
                    SkillLists.SkillList.Clear()
                    For Each skillRow As DataRow In skillData.Tables(0).Rows
                        ' Check if the typeID already exists
                        Dim newSkill As Skill
                        If SkillLists.SkillList.ContainsKey(CInt(skillRow.Item("typeID"))) = False Then
                            newSkill = New Skill
                            newSkill.Attributes = New SortedList(Of Integer, Double)
                            newSkill.ID = CInt(skillRow.Item("typeID"))
                            newSkill.GroupID = skillRow.Item("groupID").ToString.Trim
                            newSkill.Name = skillRow.Item("typeName").ToString.Trim
                            SkillLists.SkillList.Add(newSkill.ID, newSkill)
                        Else
                            newSkill = SkillLists.SkillList(CInt(skillRow.Item("typeID")))
                        End If
                        If IsDBNull(skillRow.Item("valueInt")) = False Then
                            newSkill.Attributes.Add(CInt(skillRow.Item("attributeID")), CDbl(skillRow.Item("valueInt")))
                        Else
                            newSkill.Attributes.Add(CInt(skillRow.Item("attributeID")), CDbl(skillRow.Item("valueFloat")))
                        End If
                    Next
                    Return
                Else
                    Return
                End If
            Else
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Skill Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadEveSkillData()
        ' TODO: This is essentially the copy that will be in the Core, so delete it when the Core is updated
        Core.HQ.SkillListName.Clear()
        Core.HQ.SkillListID.Clear()
        Core.HQ.SkillGroups.Clear()

        Dim skillAttFilter As New List(Of Integer)

        ' Get details of skill groups from the database
        Dim groupIDs As IEnumerable(Of Integer) = StaticData.GetGroupsInCategory(16)
        For Each groupID As Integer In groupIDs
            If groupID <> 267 Then
                Dim newSkillGroup As New Core.SkillGroup
                newSkillGroup.ID = groupID
                newSkillGroup.Name = StaticData.TypeGroups(groupID)
                Core.HQ.SkillGroups.Add(newSkillGroup.Name, newSkillGroup)

                ' Get the items in this skill group
                Dim items As IEnumerable(Of EveType) = StaticData.GetItemsInGroup(CInt(groupID))
                For Each item As EveType In items
                    Dim newSkill As New Core.EveSkill
                    newSkill.ID = item.Id
                    newSkill.Description = item.Description
                    newSkill.GroupID = item.Group
                    newSkill.Name = item.Name
                    newSkill.BasePrice = item.BasePrice
                    ' Check for salvage drone op skill in db!
                    If newSkill.ID = 3440 Then
                        newSkill.Published = True
                    Else
                        newSkill.Published = item.Published
                    End If
                    Core.HQ.SkillListID.Add(newSkill.ID, newSkill)
                    skillAttFilter.Add(CInt(newSkill.ID))
                Next
            End If
        Next
        'HQ.WriteLogEvent(" *** Parsed skill groups")

        ' Filter attributes to skills for quicker parsing in the loop
        Dim skillAtts As List(Of TypeAttrib) = (From ta In StaticData.TypeAttributes Where skillAttFilter.Contains(ta.TypeId)).ToList

        Const maxPreReqs As Integer = 10
        For Each newSkill As Core.EveSkill In Core.HQ.SkillListID.Values
            Dim preReqSkills(maxPreReqs) As Integer
            Dim preReqSkillLevels(maxPreReqs) As Integer

            ' Fetch the attributes for the item
            Dim skillID As Integer = CInt(newSkill.ID)

            For Each att As TypeAttrib In From ta In skillAtts Where ta.TypeId = skillID
                Select Case att.AttributeId
                    Case 180
                        Select Case CInt(att.Value)
                            Case 164
                                newSkill.PA = "Charisma"
                            Case 165
                                newSkill.PA = "Intelligence"
                            Case 166
                                newSkill.PA = "Memory"
                            Case 167
                                newSkill.PA = "Perception"
                            Case 168
                                newSkill.PA = "Willpower"
                        End Select
                    Case 181
                        Select Case CInt(att.Value)
                            Case 164
                                newSkill.SA = "Charisma"
                            Case 165
                                newSkill.SA = "Intelligence"
                            Case 166
                                newSkill.SA = "Memory"
                            Case 167
                                newSkill.SA = "Perception"
                            Case 168
                                newSkill.SA = "Willpower"
                        End Select
                    Case 275
                        newSkill.Rank = CInt(att.Value)
                    Case 182
                        preReqSkills(1) = CInt(att.Value)
                    Case 183
                        preReqSkills(2) = CInt(att.Value)
                    Case 184
                        preReqSkills(3) = CInt(att.Value)
                    Case 1285
                        preReqSkills(4) = CInt(att.Value)
                    Case 1289
                        preReqSkills(5) = CInt(att.Value)
                    Case 1290
                        preReqSkills(6) = CInt(att.Value)
                    Case 277
                        preReqSkillLevels(1) = CInt(att.Value)
                    Case 278
                        preReqSkillLevels(2) = CInt(att.Value)
                    Case 279
                        preReqSkillLevels(3) = CInt(att.Value)
                    Case 1286
                        preReqSkillLevels(4) = CInt(att.Value)
                    Case 1287
                        preReqSkillLevels(5) = CInt(att.Value)
                    Case 1288
                        preReqSkillLevels(6) = CInt(att.Value)
                End Select

            Next

            ' Add the pre-reqs into the list
            For prereq As Integer = 1 To maxPreReqs
                If preReqSkills(prereq) <> 0 Then
                    newSkill.PreReqSkills.Add(preReqSkills(prereq), preReqSkillLevels(prereq))
                End If
            Next
            ' Calculate the levels
            For a As Integer = 0 To 5
                newSkill.LevelUp(a) = CInt(Math.Ceiling(Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, a)))
            Next
            ' Add the currentskill to the name list
            Core.HQ.SkillListName.Add(newSkill.Name, newSkill)
        Next

    End Sub
    Private Sub LoadShipGroupData()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT * FROM invGroups WHERE invGroups.categoryID=6 ORDER BY groupName;"
            _shipGroupData = GetStaticData(strSQL)
            If _shipGroupData IsNot Nothing Then
                If _shipGroupData.Tables(0).Rows.Count <> 0 Then
                    Return
                Else
                    Return
                End If
            Else
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Group Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadShipNameData()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invGroups.groupName, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.published, invTypes.raceID, invTypes.marketGroupID"
            strSQL &= " FROM (invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID"
            strSQL &= " WHERE (invCategories.categoryID=6 AND invTypes.published=1) ORDER BY typeName;"
            _shipNameData = GetStaticData(strSQL)
            If _shipNameData IsNot Nothing Then
                If _shipNameData.Tables(0).Rows.Count <> 0 Then
                    Return
                Else
                    Return
                End If
            Else
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Name Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadShipAttributeData()
        Try
            ' Get details of ship data from database
            Dim strSQL As String = ""
            Dim pSkillName As String
            Dim sSkillName As String
            Dim tSkillName As String
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
            strSQL &= " WHERE ((invCategories.categoryID=6 AND invTypes.published=1) OR invTypes.typeID IN (601,596,588,606)) ORDER BY typeName, attributeID;"
            Dim shipData As DataSet = GetStaticData(strSQL)
            If shipData IsNot Nothing Then
                If shipData.Tables(0).Rows.Count <> 0 Then
                    ShipLists.shipList.Clear()
                    Dim lastShipName As String = ""
                    Dim newShip As New Ship
                    pSkillName = "" : sSkillName = "" : tSkillName = ""
                    Dim attValue As Double
                    For Each shipRow As DataRow In shipData.Tables(0).Rows
                        ' If the shipName has changed, we need to start a new ship type
                        If lastShipName <> shipRow.Item("typeName").ToString Then
                            ' Add the current ship to the list then reset the ship data
                            If lastShipName <> "" Then
                                Call newShip.AddCustomShipAttributes()
                                ' Map the attributes
                                Ship.MapShipAttributes(newShip)
                                ShipLists.shipList.Add(newShip.Name, newShip)
                                newShip = New Ship
                                pSkillName = "" : sSkillName = "" : tSkillName = ""
                            End If
                            ' Create new ship type & non "attribute" data
                            newShip.Name = shipRow.Item("typeName").ToString
                            newShip.ID = CInt(shipRow.Item("typeID"))
                            newShip.Description = shipRow.Item("description").ToString
                            newShip.DatabaseGroup = CInt(shipRow.Item("groupID"))
                            newShip.DatabaseCategory = CInt(shipRow.Item("categoryID"))
                            If IsDBNull(shipRow.Item("marketGroupID")) Then
                                newShip.MarketGroup = 0
                            Else
                                newShip.MarketGroup = CInt(shipRow.Item("marketGroupID"))
                            End If
                            newShip.BasePrice = CDbl(shipRow.Item("basePrice"))
                            newShip.MarketPrice = 0
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
                        Select Case CInt(shipRow.Item("attributeID"))
                            Case 55, 1034, 479
                                attValue = attValue / 1000
                            Case 113, 111, 109, 110, 267, 268, 269, 270, 271, 272, 273, 274
                                attValue = (1 - attValue) * 100
                            Case 1281
                                attValue = attValue * 3
                            Case 1154 ' Reset this field to be used as Calibration_Used
                                attValue = 0
                        End Select

                        ' Add the attribute to the ship.attributes list
                        newShip.Attributes.Add(CInt(shipRow.Item("attributeID")), attValue)

                        ' Map only the skill attributes
                        Select Case CInt(shipRow.Item("attributeID"))
                            Case 182
                                Dim pSkill As EveType = StaticData.Types(CInt(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = pSkill.Id
                                nSkill.Name = pSkill.Name
                                pSkillName = pSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 183
                                Dim sSkill As EveType = StaticData.Types(CInt(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = sSkill.Id
                                nSkill.Name = sSkill.Name
                                sSkillName = sSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 184
                                Dim tSkill As EveType = StaticData.Types(CInt(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = tSkill.Id
                                nSkill.Name = tSkill.Name
                                tSkillName = tSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 277
                                If newShip.RequiredSkills.ContainsKey(pSkillName) = True Then
                                    Dim cSkill As ItemSkills = newShip.RequiredSkills(pSkillName)
                                    cSkill.Level = CInt(attValue)
                                End If
                            Case 278
                                If newShip.RequiredSkills.ContainsKey(sSkillName) = True Then
                                    Dim cSkill As ItemSkills = newShip.RequiredSkills(sSkillName)
                                    cSkill.Level = CInt(attValue)
                                End If
                            Case 279
                                If newShip.RequiredSkills.ContainsKey(tSkillName) = True Then
                                    Dim cSkill As ItemSkills = newShip.RequiredSkills(tSkillName)
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
                    Return
                Else
                    MessageBox.Show("Ship Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Else
                MessageBox.Show("Ship Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Ship Attribute Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub PopulateShipLists()
        ShipLists.shipListKeyName.Clear()
        ShipLists.shipListKeyID.Clear()
        For Each baseShip As Ship In ShipLists.shipList.Values
            ShipLists.shipListKeyName.Add(baseShip.Name, baseShip.ID)
            ShipLists.shipListKeyID.Add(baseShip.ID, baseShip.Name)
        Next
    End Sub
    Private Sub LoadModuleData()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID"
            strSQL &= " FROM invCategories INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invCategories.categoryID = invGroups.categoryID"
            strSQL &= " WHERE (((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010"
            strSQL &= " ORDER BY invTypes.typeName;"
            _moduleData = GetStaticData(strSQL)
            If _moduleData IsNot Nothing Then
                If _moduleData.Tables(0).Rows.Count <> 0 Then
                    Return
                Else
                    MessageBox.Show("Module Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Else
                MessageBox.Show("Module Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadModuleEffectData()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeEffects.effectID"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeEffects ON invTypes.typeID=dgmTypeEffects.typeID"
            strSQL &= " WHERE (((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010"
            strSQL &= " ORDER BY typeName, effectID;"
            _moduleEffectData = GetStaticData(strSQL)
            If _moduleEffectData IsNot Nothing Then
                If _moduleEffectData.Tables(0).Rows.Count <> 0 Then
                    Return
                Else
                    MessageBox.Show("Module Effect Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Else
                MessageBox.Show("Module Effect Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Effect Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadModuleAttributeData()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName"
            strSQL &= " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits RIGHT OUTER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID"
            strSQL &= " WHERE (((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010"
            strSQL &= " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;"

            _moduleAttributeData = GetStaticData(strSQL)
            If _moduleAttributeData IsNot Nothing Then
                If _moduleAttributeData.Tables(0).Rows.Count <> 0 Then
                    Return
                Else
                    MessageBox.Show("Module Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Else
                MessageBox.Show("Module Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Attribute Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub LoadModuleMetaTypes()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invMetaTypes.parentTypeID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID"
            strSQL &= " FROM (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (invMetaGroups INNER JOIN invMetaTypes ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID) ON invTypes.typeID = invMetaTypes.typeID"
            strSQL &= " WHERE (((invGroups.categoryID) In (7,8,18,20,32)) AND (invTypes.published=1))"
            Dim metaTypeData As DataSet = GetStaticData(strSQL)
            If metaTypeData IsNot Nothing Then
                If metaTypeData.Tables(0).Rows.Count <> 0 Then
                    ModuleLists.moduleMetaTypes.Clear()
                    ModuleLists.moduleMetaGroups.Clear()
                    For Each row As DataRow In metaTypeData.Tables(0).Rows
                        If ModuleLists.ModuleMetaTypes.ContainsKey(CInt(row.Item("invTypes_typeID"))) = False Then
                            ModuleLists.ModuleMetaTypes.Add(CInt(row.Item("invTypes_typeID")), CInt(row.Item("parentTypeID")))
                            ModuleLists.ModuleMetaGroups.Add(CInt(row.Item("invTypes_typeID")), CInt(row.Item("invMetaGroups_metaGroupID")))
                        End If
                        If ModuleLists.ModuleMetaTypes.ContainsKey(CInt(row.Item("parentTypeID"))) = False Then
                            ModuleLists.ModuleMetaTypes.Add(CInt(row.Item("parentTypeID")), CInt(row.Item("parentTypeID")))
                            ModuleLists.ModuleMetaGroups.Add(CInt(row.Item("parentTypeID")), 0)
                        End If
                    Next
                    Return
                Else
                    MessageBox.Show("Module Metatype Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Else
                MessageBox.Show("Module Metatype Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error loading Module Metatype Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub BuildModuleData()
        Try
            ModuleLists.moduleList.Clear()
            ModuleLists.moduleListName.Clear()
            Implants.implantList.Clear()
            Boosters.BoosterList.Clear()
            For Each row As DataRow In _moduleData.Tables(0).Rows
                Dim newModule As New ShipModule
                newModule.ID = CInt(row.Item("typeID"))
                newModule.Description = row.Item("description").ToString
                newModule.Name = row.Item("typeName").ToString.Trim
                newModule.DatabaseGroup = CInt(row.Item("groupID"))
                newModule.DatabaseCategory = CInt(row.Item("categoryID"))
                newModule.BasePrice = CDbl(row.Item("baseprice"))
                newModule.Volume = CDbl(row.Item("volume"))
                newModule.Capacity = CDbl(row.Item("capacity"))
                newModule.Attributes.Add(AttributeEnum.ModuleCapacity, CDbl(row.Item("capacity")))
                newModule.Attributes.Add(AttributeEnum.ModuleMass, CDbl(row.Item("mass")))
                newModule.MarketPrice = 0
                ' Get icon from the YAML parsing
                'newModule.Icon = row.Item("iconFile").ToString
                If _yamlTypes.ContainsKey(CInt(newModule.ID)) Then
                    newModule.Icon = _yamlTypes(CInt(newModule.ID)).IconName
                End If
                If IsDBNull(row.Item("marketGroupID")) = False Then
                    newModule.MarketGroup = CInt(row.Item("marketGroupID"))
                Else
                    newModule.MarketGroup = 0
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
                    Case 2 ' Container
                        newModule.IsContainer = True
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
                End Select
            Next

            ' Fill in the blank market groups now the list is complete
            Dim modID As Integer
            Dim parentID As Integer
            Dim nModule As ShipModule
            Dim eModule As ShipModule
            For setNo = 0 To 1
                For Each row As DataRow In _moduleData.Tables(0).Rows
                    If IsDBNull(row.Item("marketGroupID")) = True Then
                        modID = CInt(row.Item("typeID"))
                        nModule = ModuleLists.ModuleList(modID)
                        If ModuleLists.moduleMetaTypes.ContainsKey(modID) = True Then
                            parentID = ModuleLists.ModuleMetaTypes(modID)
                            eModule = ModuleLists.ModuleList(parentID)
                            nModule.MarketGroup = eModule.MarketGroup
                        End If
                    End If
                Next
            Next

            ' Search for changes/additions to the market groups from resources
            Dim changeLines As String() = ResourceHandler.GetResource("newItemMarketGroup").ToString.Split(ControlChars.CrLf.ToCharArray)
            For Each marketChange As String In changeLines
                If marketChange.Trim <> "" Then
                    Dim changeData() As String = marketChange.Split(",".ToCharArray)
                    Dim typeID As Integer = CInt(changeData(0))
                    Dim marketGroupID As Integer = CInt(changeData(1))
                    Dim metaTypeID As Integer = CInt(changeData(2))
                    If ModuleLists.moduleList.ContainsKey(typeID) = True Then
                        Dim mModule As ShipModule = ModuleLists.ModuleList(typeID)
                        mModule.MarketGroup = marketGroupID
                        If metaTypeID <> 0 Then
                            mModule.MetaType = metaTypeID
                        End If
                    End If
                End If
            Next
            BuildModuleEffectData()
            Return
        Catch e As Exception
            MessageBox.Show("Error building Module Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Sub BuildModuleEffectData()
        Try
            ' Get details of module attributes from already retrieved dataset
            For Each modRow As DataRow In _moduleEffectData.Tables(0).Rows
                Dim effMod As ShipModule = ModuleLists.ModuleList.Item(CInt(modRow.Item("typeID")))
                If effMod IsNot Nothing Then
                    Select Case CInt(modRow.Item("effectID"))
                        Case 11 ' Low slot
                            effMod.SlotType = SlotTypes.Low
                        Case 12 ' High slot
                            effMod.SlotType = SlotTypes.High
                        Case 13 ' Mid slot
                            effMod.SlotType = SlotTypes.Mid
                        Case 2663 ' Rig slot
                            effMod.SlotType = SlotTypes.Rig
                        Case 3772 ' Sub slot
                            effMod.SlotType = SlotTypes.Subsystem
                        Case 40
                            If effMod.DatabaseGroup <> 481 Then
                                effMod.IsLauncher = True
                            End If
                        Case 10, 34, 42
                            effMod.IsTurret = True
                    End Select
                    ' Add custom attributes
                    If effMod.IsDrone = True Or effMod.IsLauncher = True Or effMod.IsTurret = True Or effMod.DatabaseGroup = 72 Or effMod.DatabaseGroup = 862 Then
                        If effMod.Attributes.ContainsKey(10017) = False Then
                            effMod.Attributes.Add(10017, 0)
                            effMod.Attributes.Add(10018, 0)
                            effMod.Attributes.Add(10019, 0)
                            effMod.Attributes.Add(10030, 0)
                            effMod.Attributes.Add(10051, 0)
                            effMod.Attributes.Add(10052, 0)
                            effMod.Attributes.Add(10053, 0)
                            effMod.Attributes.Add(10054, 0)
                        End If
                    End If
                    Select Case CInt(effMod.MarketGroup)
                        Case 1038 ' Ice Miners
                            If effMod.Attributes.ContainsKey(10041) = False Then
                                effMod.Attributes.Add(10041, 0)
                            End If
                        Case 1039, 1040 ' Ore Miners
                            If effMod.Attributes.ContainsKey(10039) = False Then
                                effMod.Attributes.Add(10039, 0)
                            End If
                        Case 158 ' Mining Drones
                            If effMod.Attributes.ContainsKey(10040) = False Then
                                effMod.Attributes.Add(10040, 0)
                            End If
                    End Select
                    Select Case CInt(effMod.DatabaseGroup)
                        Case 76
                            If effMod.Attributes.ContainsKey(6) = False Then
                                effMod.Attributes.Add(6, 0)
                            End If
                    End Select
                End If
            Next
            If BuildModuleAttributeData() = True Then
                Return
            Else
                Return
            End If
        Catch e As Exception
            MessageBox.Show("Error building Module Effect Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
    End Sub
    Private Function BuildModuleAttributeData() As Boolean
        Try
            ' Get details of module attributes from already retrieved dataset
            Dim attValue As Double
            Dim pSkillName As String = "" : Dim sSkillName As String = "" : Dim tSkillName As String = ""
            Dim lastModName As String = ""
            For Each modRow As DataRow In _moduleAttributeData.Tables(0).Rows
                Dim attMod As ShipModule = ModuleLists.ModuleList.Item(CInt(modRow.Item("typeID")))
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
                Select Case CInt(modRow.Item("attributeID"))
                    Case 204
                        If CInt(attValue) = -100 Then Exit Select
                        attMod.Attributes.Add(CInt(modRow.Item("attributeID")), attValue)
                    Case 51 ' ROF
                        If CInt(attValue) = -100 Then Exit Select
                        Select Case attMod.DatabaseGroup
                            Case 53 ' Energy Turret 
                                attMod.Attributes.Add(10011, attValue)
                            Case 74 ' Hybrid Turret
                                attMod.Attributes.Add(10012, attValue)
                            Case 55 ' Projectile Turret
                                attMod.Attributes.Add(10013, attValue)
                            Case Else
                                attMod.Attributes.Add(CInt(modRow.Item("attributeID")), attValue)
                        End Select
                    Case 64 ' Damage Modifier
                        If CInt(attValue) = 0 Then Exit Select
                        Select Case attMod.DatabaseGroup
                            Case 53 ' Energy Turret 
                                attMod.Attributes.Add(10014, attValue)
                            Case 74 ' Hybrid Turret
                                attMod.Attributes.Add(10015, attValue)
                            Case 55 ' Projectile Turret
                                attMod.Attributes.Add(10016, attValue)
                            Case Else
                                attMod.Attributes.Add(CInt(modRow.Item("attributeID")), attValue)
                        End Select
                    Case 306 ' Max Velocity Penalty
                        Select Case attMod.DatabaseGroup
                            Case 653, 654, 655, 656, 657, 648 ' T2 Missiles
                                If CInt(attValue) = -100 Then
                                    attValue = 0
                                End If
                        End Select
                        attMod.Attributes.Add(CInt(modRow.Item("attributeID")), attValue)
                    Case 144 ' Cap Recharge Rate
                        Select Case attMod.DatabaseGroup
                            Case 653, 654, 655, 656, 657, 648 ' T2 Missiles
                                If CInt(attValue) = -100 Then
                                    attValue = 0
                                End If
                        End Select
                        attMod.Attributes.Add(CInt(modRow.Item("attributeID")), attValue)
                    Case 267, 268, 269, 270 ' Armor resistances
                        ' Invert Armor Resistance Shift Hardener values
                        Select Case attMod.DatabaseGroup
                            Case 1150
                                attValue = -attValue
                        End Select
                        attMod.Attributes.Add(CInt(modRow.Item("attributeID")), attValue)
                    Case Else
                        attMod.Attributes.Add(CInt(modRow.Item("attributeID")), attValue)
                End Select

                Select Case CInt(modRow.Item("attributeID"))
                    Case 30
                        attMod.PG = attValue
                    Case 50
                        attMod.CPU = attValue
                    Case 6
                        attMod.CapUsage = attValue
                    Case 51
                        If attMod.Attributes.ContainsKey(6) = True Then
                            attMod.CapUsageRate = attMod.CapUsage / attValue
                            attMod.Attributes.Add(10032, attMod.CapUsageRate)
                        End If
                    Case 73
                        attMod.ActivationTime = attValue
                        attMod.CapUsageRate = attMod.CapUsage / attMod.ActivationTime
                        attMod.Attributes.Add(10032, attMod.CapUsageRate)
                    Case 77
                        Select Case CInt(attMod.MarketGroup)
                            Case 1038 ' Ice Mining
                                attMod.Attributes(10041) = CDbl(attMod.Attributes(77)) / CDbl(attMod.Attributes(73))
                            Case 1039, 1040 ' Ore Mining
                                attMod.Attributes(10039) = CDbl(attMod.Attributes(77)) / CDbl(attMod.Attributes(73))
                            Case 158 ' Mining Drone
                                attMod.Attributes(10040) = CDbl(attMod.Attributes(77)) / CDbl(attMod.Attributes(73))
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
                        If StaticData.Types.ContainsKey(CInt(attValue)) = True Then
                            Dim pSkill As EveType = StaticData.Types(CInt(attValue))
                            Dim nSkill As New ItemSkills
                            nSkill.ID = pSkill.Id
                            nSkill.Name = pSkill.Name
                            pSkillName = pSkill.Name
                            If attMod.RequiredSkills.ContainsKey(nSkill.Name) = False Then
                                attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                            End If
                        End If
                    Case 183
                        If StaticData.Types.ContainsKey(CInt(attValue)) = True Then
                            Dim sSkill As EveType = StaticData.Types(CInt(attValue))
                            Dim nSkill As New ItemSkills
                            nSkill.ID = sSkill.Id
                            nSkill.Name = sSkill.Name
                            sSkillName = sSkill.Name
                            If attMod.RequiredSkills.ContainsKey(nSkill.Name) = False Then
                                attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                            End If
                        End If
                    Case 184
                        If StaticData.Types.ContainsKey(CInt(attValue)) = True Then
                            Dim tSkill As EveType = StaticData.Types(CInt(attValue))
                            Dim nSkill As New ItemSkills
                            nSkill.ID = tSkill.Id
                            nSkill.Name = tSkill.Name
                            tSkillName = tSkill.Name
                            If attMod.RequiredSkills.ContainsKey(nSkill.Name) = False Then
                                attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                            End If
                        End If
                    Case 277
                        If attMod.RequiredSkills.ContainsKey(pSkillName) Then
                            Dim cSkill As ItemSkills = attMod.RequiredSkills(pSkillName)
                            If cSkill IsNot Nothing Then
                                cSkill.Level = CInt(attValue)
                            End If
                        End If
                    Case 278
                        If attMod.RequiredSkills.ContainsKey(sSkillName) Then
                            Dim cSkill As ItemSkills = attMod.RequiredSkills(sSkillName)
                            If cSkill IsNot Nothing Then
                                cSkill.Level = CInt(attValue)
                            End If
                        End If
                    Case 279
                        If attMod.RequiredSkills.ContainsKey(tSkillName) Then
                            Dim cSkill As ItemSkills = attMod.RequiredSkills(tSkillName)
                            If cSkill IsNot Nothing Then
                                cSkill.Level = CInt(attValue)
                            End If
                        End If
                    Case 604, 605, 606, 609, 610
                        attMod.Charges.Add(CInt(attValue))
                    Case 633 ' MetaLevel
                        attMod.MetaLevel = CInt(attValue)
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
                If ModuleLists.moduleMetaGroups.ContainsKey(cMod.ID) = True Then
                    If ModuleLists.ModuleMetaGroups(cMod.ID) = 0 Then
                        If cMod.Attributes.ContainsKey(422) = True Then
                            Select Case CInt(cMod.Attributes(422))
                                Case 1
                                    cMod.MetaType = CInt(2 ^ 0)
                                Case 2
                                    cMod.MetaType = CInt(2 ^ 1)
                                Case 3
                                    cMod.MetaType = CInt(2 ^ 13)
                                Case Else
                                    cMod.MetaType = CInt(2 ^ 0)
                            End Select
                        Else
                            cMod.MetaType = 1
                        End If
                    Else
                        cMod.MetaType = CInt(2 ^ (CInt(ModuleLists.ModuleMetaGroups(cMod.ID)) - 1))
                    End If
                Else
                    cMod.MetaType = 1
                End If
            Next
            ' Build charge data
            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                If cMod.IsCharge = True Then
                    If Charges.ChargeGroups.ContainsKey(cMod.ID) = False Then
                        Charges.ChargeGroups.Add(cMod.ID, cMod.MarketGroup & "_" & cMod.DatabaseGroup & "_" & cMod.Name & "_" & cMod.ChargeSize)
                    End If
                End If
            Next
            ' Check for drone missiles
            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                If cMod.IsDrone = True And cMod.Attributes.ContainsKey(507) = True Then
                    Dim chg As ShipModule = ModuleLists.ModuleList(CInt(cMod.Attributes(507)))
                    cMod.LoadedCharge = chg
                End If
            Next
            ' Build the implant data
            If BuildImplantData() = True Then
                Return True
            Else
                Return False
            End If
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
            ' Extract the groups from the included resource file
            Dim implantsList() As String = ResourceHandler.GetResource("ImplantEffects").Split(ControlChars.CrLf.ToCharArray)
            Dim implantData() As String
            Dim implantName As String
            Dim implantGroups As String
            Dim implantGroup() As String
            For Each cImplant As String In implantsList
                If cImplant.Trim <> "" And cImplant.StartsWith("#", StringComparison.Ordinal) = False Then
                    implantData = cImplant.Split(",".ToCharArray)
                    implantName = implantData(10)
                    implantGroups = implantData(9)
                    implantGroup = implantGroups.Split(";".ToCharArray)
                    If Implants.ImplantList.ContainsKey(implantName) = True Then
                        Dim bImplant As ShipModule = Implants.ImplantList(implantName)
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
    Private Sub BuildModuleEffects()
        ' Break the Effects down into separate lines
        Dim effectLines As List(Of String) = ResourceHandler.GetResource("Effects").ToString.Split(ControlChars.CrLf.ToCharArray).ToList
        ' Go through lines and break each one down
        Dim effectData As List(Of String)
        Dim newEffect As Effect
        Dim ids As List(Of String)
        Dim affectingIDs As List(Of String)
        Dim affectingName As String = ""
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#", StringComparison.Ordinal) = False Then
                effectData = effectLine.Split(",".ToCharArray).ToList
                affectingIDs = effectData(2).Split(";".ToCharArray).ToList()

                For Each affectingID As String In affectingIDs

                    newEffect = New Effect
                    newEffect.AffectingAtt = CInt(effectData(0))
                    newEffect.AffectingType = CType(effectData(1), HQFEffectType)
                    newEffect.AffectingID = CInt(affectingID)
                    newEffect.AffectedAtt = CInt(effectData(3))
                    newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                    If effectData(5).Contains(";") = True Then
                        ids = effectData(5).Split(";".ToCharArray).ToList
                        For Each id As String In ids
                            newEffect.AffectedID.Add(CInt(id))
                        Next
                    Else
                        newEffect.AffectedID.Add(CInt(effectData(5)))
                    End If
                    newEffect.StackNerf = CType(effectData(6), EffectStackType)
                    newEffect.IsPerLevel = CBool(effectData(7))
                    newEffect.CalcType = CType(effectData(8), EffectCalcType)
                    newEffect.Status = CInt(effectData(9))

                    Select Case newEffect.AffectingType
                        ' Setup the name as Item;Type;Attribute
                        Case HQFEffectType.All
                            affectingName = "Global;Global;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                        Case HQFEffectType.Item
                            If newEffect.AffectingID > 0 Then
                                affectingName = StaticData.Types(newEffect.AffectingID).Name
                                If Core.HQ.SkillListName.ContainsKey(affectingName) = True Then
                                    affectingName &= ";Skill;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                                Else
                                    affectingName &= ";Item;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                                End If
                            End If
                        Case HQFEffectType.Group
                            affectingName = StaticData.TypeGroups(newEffect.AffectingID) & ";Group;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                        Case HQFEffectType.Category
                            affectingName = StaticData.TypeCats(newEffect.AffectingID) & ";Category;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                        Case HQFEffectType.MarketGroup
                            affectingName = CStr(Market.MarketGroupList(newEffect.AffectingID.ToString)) & ";Market Group;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                    End Select
                    affectingName &= ";"

                    For Each cModule As ShipModule In ModuleLists.ModuleList.Values
                        Select Case newEffect.AffectedType
                            Case HQFEffectType.All
                                If newEffect.AffectingID <> 0 Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Item
                                If newEffect.AffectedID.Contains(cModule.ID) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Group
                                If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Category
                                If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.MarketGroup
                                If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Skill
                                If cModule.RequiredSkills.ContainsKey(StaticData.Types(newEffect.AffectedID(0)).Name) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Attribute
                                If cModule.Attributes.ContainsKey(newEffect.AffectedID(0)) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                        End Select
                    Next

                    ' Add the skills into the ship
                    If newEffect.Status < 16 Then
                        If affectingName.Contains(";Skill;") = True Then
                            For Each cShip As Ship In ShipLists.ShipList.Values
                                Select Case newEffect.AffectedType
                                    Case HQFEffectType.All
                                        If newEffect.AffectingID <> 0 Then
                                            cShip.Affects.Add(affectingName)
                                        End If
                                    Case HQFEffectType.Item
                                        If newEffect.AffectedID.Contains(cShip.ID) Then
                                            cShip.Affects.Add(affectingName)
                                        End If
                                    Case HQFEffectType.Group
                                        If newEffect.AffectedID.Contains(cShip.DatabaseGroup) Then
                                            cShip.Affects.Add(affectingName)
                                        End If
                                    Case HQFEffectType.Category
                                        If newEffect.AffectedID.Contains(cShip.DatabaseCategory) Then
                                            cShip.Affects.Add(affectingName)
                                        End If
                                    Case HQFEffectType.MarketGroup
                                        If newEffect.AffectedID.Contains(cShip.MarketGroup) Then
                                            cShip.Affects.Add(affectingName)
                                        End If
                                    Case HQFEffectType.Attribute
                                        If cShip.Attributes.ContainsKey(newEffect.AffectedID(0)) Then
                                            cShip.Affects.Add(affectingName)
                                        End If
                                End Select
                            Next
                        End If
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub BuildImplantEffects()
        ' Break the Effects down into separate lines
        Dim effectLines() As String = ResourceHandler.GetResource("ImplantEffects").ToString.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String
        Dim newEffect As ImplantEffect
        Dim ids() As String
        Dim attIDs() As String
        Dim atts As New ArrayList
        Dim affectingName As String
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#", StringComparison.Ordinal) = False Then
                effectData = effectLine.Split(",".ToCharArray)
                atts.Clear()
                If effectData(3).Contains(";") Then
                    attIDs = effectData(3).Split(";".ToCharArray)
                    For Each attID As String In attIDs
                        atts.Add(attID)
                    Next
                Else
                    atts.Add(effectData(3))
                End If
                For Each att As String In atts
                    newEffect = New ImplantEffect
                    newEffect.ImplantName = CStr(effectData(10))
                    newEffect.AffectingAtt = CInt(effectData(0))
                    newEffect.AffectedAtt = CInt(att)
                    newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                    If effectData(5).Contains(";") = True Then
                        ids = effectData(5).Split(";".ToCharArray)
                        For Each id As String In ids
                            newEffect.AffectedID.Add(CInt(id))
                        Next
                    Else
                        newEffect.AffectedID.Add(CInt(effectData(5)))
                    End If
                    newEffect.CalcType = CType(effectData(6), EffectCalcType)
                    Dim cImplant As ShipModule = Implants.ImplantList(newEffect.ImplantName)
                    newEffect.Value = CDbl(cImplant.Attributes(CInt(effectData(0))))
                    newEffect.IsGang = CBool(effectData(8))
                    If effectData(9).Contains(";") = True Then
                        ids = effectData(9).Split(";".ToCharArray)
                        For Each id As String In ids
                            newEffect.Groups.Add(id)
                        Next
                    Else
                        newEffect.Groups.Add(effectData(9))
                    End If

                    affectingName = StaticData.Types(CInt(effectData(2))).Name & ";Implant;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString & ";"

                    For Each cModule As ShipModule In ModuleLists.ModuleList.Values
                        Select Case newEffect.AffectedType
                            Case HQFEffectType.All
                                If CInt(effectData(2)) <> 0 Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Item
                                If newEffect.AffectedID.Contains(cModule.ID) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Group
                                If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Category
                                If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.MarketGroup
                                If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                            Case HQFEffectType.Attribute
                                If cModule.Attributes.ContainsKey(newEffect.AffectedID(0)) Then
                                    cModule.Affects.Add(affectingName)
                                End If
                        End Select
                    Next
                Next
            End If
        Next
    End Sub
    Private Sub BuildShipEffects()

        Dim culture As Globalization.CultureInfo = New Globalization.CultureInfo("en-GB")
        ' Break the Effects down into separate lines
        Dim effectLines() As String = ResourceHandler.GetResource("ShipBonuses").ToString.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String

        Dim shipEffectClassList As New ArrayList
        Dim newEffect As ShipEffect
        Dim ids() As String
        Dim affectingName As String
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#", StringComparison.Ordinal) = False Then
                effectData = effectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                newEffect.ShipID = CInt(effectData(0))
                newEffect.AffectingType = CType(effectData(1), HQFEffectType)
                newEffect.AffectingID = CInt(effectData(2))
                newEffect.AffectedAtt = CInt(effectData(3))
                newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                If effectData(5).Contains(";") = True Then
                    ids = effectData(5).Split(";".ToCharArray)
                    For Each id As String In ids
                        newEffect.AffectedID.Add(CInt(id))
                    Next
                Else
                    newEffect.AffectedID.Add(CInt(effectData(5)))
                End If
                newEffect.StackNerf = CType(effectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(effectData(7))
                newEffect.CalcType = CType(effectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(effectData(9), Globalization.NumberStyles.Any, culture)
                newEffect.Status = CInt(effectData(10))
                shipEffectClassList.Add(newEffect)

                affectingName = StaticData.Types(newEffect.ShipID).Name
                If newEffect.IsPerLevel = False Then
                    affectingName &= ";Ship Role;"
                Else
                    affectingName &= ";Ship Bonus;"
                End If
                affectingName &= Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                If newEffect.IsPerLevel = False Then
                    affectingName &= ";"
                Else
                    affectingName &= ";" & StaticData.Types(newEffect.AffectingID).Name
                End If

                ' Add the skills into the ship modules
                For Each cModule As ShipModule In ModuleLists.ModuleList.Values
                    Select Case newEffect.AffectedType
                        Case HQFEffectType.All
                            If newEffect.AffectingID <> 0 Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Item
                            If newEffect.AffectedID.Contains(cModule.ID) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Group
                            If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Category
                            If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.MarketGroup
                            If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Attribute
                            If cModule.Attributes.ContainsKey(newEffect.AffectedID(0)) Then
                                cModule.Affects.Add(affectingName)
                            End If
                    End Select
                Next

                ' Add the skills into the ship global skills

                If newEffect.Status < 16 Then
                    For Each cShip As Ship In ShipLists.ShipList.Values
                        If newEffect.ShipID = CInt(cShip.ID) Then
                            If cShip.GlobalAffects Is Nothing Then
                                cShip.GlobalAffects = New List(Of String)
                            End If
                            cShip.GlobalAffects.Add(affectingName)
                        End If
                    Next
                End If

            End If
        Next
    End Sub
    Private Sub BuildSubsystemEffects()
        Dim culture As Globalization.CultureInfo = New Globalization.CultureInfo("en-GB")

        ' Break the Effects down into separate lines
        Dim effectLines() As String = ResourceHandler.GetResource("Subsystems").ToString.Split(ControlChars.CrLf.ToCharArray)
        ' Go through lines and break each one down
        Dim effectData() As String

        Dim shipEffectClassList As New ArrayList
        Dim newEffect As ShipEffect
        Dim ids() As String
        Dim affectingName As String
        For Each effectLine As String In effectLines
            If effectLine.Trim <> "" And effectLine.StartsWith("#", StringComparison.Ordinal) = False Then
                effectData = effectLine.Split(",".ToCharArray)
                newEffect = New ShipEffect
                newEffect.ShipID = CInt(effectData(0))
                newEffect.AffectingType = CType(effectData(1), HQFEffectType)
                newEffect.AffectingID = CInt(effectData(2))
                newEffect.AffectedAtt = CInt(effectData(3))
                newEffect.AffectedType = CType(effectData(4), HQFEffectType)
                If effectData(5).Contains(";") = True Then
                    ids = effectData(5).Split(";".ToCharArray)
                    For Each id As String In ids
                        newEffect.AffectedID.Add(CInt(id))
                    Next
                Else
                    newEffect.AffectedID.Add(CInt(effectData(5)))
                End If
                newEffect.StackNerf = CType(effectData(6), EffectStackType)
                newEffect.IsPerLevel = CBool(effectData(7))
                newEffect.CalcType = CType(effectData(8), EffectCalcType)
                newEffect.Value = Double.Parse(effectData(9), Globalization.NumberStyles.Any, culture)
                newEffect.Status = CInt(effectData(10))
                shipEffectClassList.Add(newEffect)

                affectingName = StaticData.Types(newEffect.ShipID).Name
                If newEffect.IsPerLevel = False Then
                    affectingName &= ";Subsystem Role;"
                Else
                    affectingName &= ";Subsystem;"
                End If
                affectingName &= Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                If newEffect.IsPerLevel = False Then
                    affectingName &= ";"
                Else
                    affectingName &= ";" & StaticData.Types(newEffect.AffectingID).Name
                End If

                For Each cModule As ShipModule In ModuleLists.ModuleList.Values
                    Select Case newEffect.AffectedType
                        Case HQFEffectType.All
                            If newEffect.AffectingID <> 0 Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Item
                            If newEffect.AffectedID.Contains(cModule.ID) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Group
                            If newEffect.AffectedID.Contains(cModule.DatabaseGroup) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Category
                            If newEffect.AffectedID.Contains(cModule.DatabaseCategory) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.MarketGroup
                            If newEffect.AffectedID.Contains(cModule.MarketGroup) Then
                                cModule.Affects.Add(affectingName)
                            End If
                        Case HQFEffectType.Attribute
                            If cModule.Attributes.ContainsKey(newEffect.AffectedID(0)) Then
                                cModule.Affects.Add(affectingName)
                            End If
                    End Select
                    ' Add the skill onto the subsystem
                    If newEffect.IsPerLevel = True Then
                        If cModule.ID = newEffect.ShipID Then
                            affectingName = StaticData.Types(newEffect.AffectingID).Name
                            affectingName &= ";Skill;" & Attributes.AttributeQuickList(newEffect.AffectedAtt).ToString
                            If cModule.Affects.Contains(affectingName) = False Then
                                cModule.Affects.Add(affectingName)
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
    Private Sub BuildShipMarketGroups()
        Dim tvwShips As New TreeView
        tvwShips.BeginUpdate()
        tvwShips.Nodes.Clear()
        Dim marketTable As DataTable = _marketGroupData.Tables(0)
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
        Dim shipGroup As String
        Dim factionRows() As DataRow = _shipNameData.Tables(0).Select("ISNULL(marketGroupID, 0) = 0")
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
        Call WriteShipGroups(tvwShips)
        tvwShips.Dispose()
    End Sub
    Private Sub BuildItemMarketGroups()
        Dim tvwItems As New TreeView
        tvwItems.BeginUpdate()
        tvwItems.Nodes.Clear()
        Dim marketTable As DataTable = _marketGroupData.Tables(0)
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
        Dim favNode As New TreeNode("Favourites")
        favNode.Name = "Favourites"
        favNode.Tag = "Favourites"
        tvwItems.Nodes.Add(favNode)
        ' Add the Favourties Node
        Dim mruNode As New TreeNode("Recently Used")
        mruNode.Name = "Recently Used"
        mruNode.Tag = "Recently Used"
        tvwItems.Nodes.Add(mruNode)
        tvwItems.EndUpdate()
        Market.MarketGroupPath.Clear()
        Call BuildTreePathData(tvwItems)
        Call WriteItemGroups(tvwItems)
        tvwItems.Dispose()
    End Sub
    Private Sub CleanUpData()
        _marketGroupData = Nothing
        _shipGroupData = Nothing
        _shipNameData = Nothing
        _moduleData = Nothing
        _moduleEffectData = Nothing
        _moduleAttributeData = Nothing
        GC.Collect()
    End Sub

#End Region

#Region "HQF Cache Writing Routines"

    Private Sub SaveHQFCacheData()

        ' Dump HQF data to folder
        Dim s As FileStream

        ' Save ships
        s = New FileStream(Path.Combine(_coreCacheFolder, "ships.dat"), FileMode.Create)
        Serializer.Serialize(s, ShipLists.shipList)
        s.Flush()
        s.Close()

        ' Save modules
        s = New FileStream(Path.Combine(_coreCacheFolder, "modules.dat"), FileMode.Create)
        Serializer.Serialize(s, ModuleLists.moduleList)
        s.Flush()
        s.Close()

        ' Save implants
        s = New FileStream(Path.Combine(_coreCacheFolder, "implants.dat"), FileMode.Create)
        Serializer.Serialize(s, Implants.implantList)
        s.Flush()
        s.Close()

        ' Save boosters
        s = New FileStream(Path.Combine(_coreCacheFolder, "boosters.dat"), FileMode.Create)
        Serializer.Serialize(s, Boosters.BoosterList)
        s.Flush()
        s.Close()

        ' Save skills
        s = New FileStream(Path.Combine(_coreCacheFolder, "skills.dat"), FileMode.Create)
        Serializer.Serialize(s, SkillLists.SkillList)
        s.Flush()
        s.Close()

        ' Save attributes
        s = New FileStream(Path.Combine(_coreCacheFolder, "attributes.dat"), FileMode.Create)
        Serializer.Serialize(s, Attributes.AttributeList)
        s.Flush()
        s.Close()

    End Sub

#End Region

#Region "Database Connection Routines"

    Private Function GetStaticData(ByVal strSQL As String) As DataSet

        Dim evehqData As New DataSet
        Dim conn As New SqlConnection
        conn.ConnectionString = StaticDBConnection
        Try
            conn.Open()
            Dim da As New SqlDataAdapter(strSQL, conn)
            da.Fill(evehqData, "evehqData")
            conn.Close()
            Return evehqData
        Catch e As Exception
            Core.HQ.dataError = e.Message
            Return Nothing
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Function

#End Region

#Region "Misc HQF Routines"

    Private Sub PopulateShipGroups(ByVal inParentID As Integer, ByRef inTreeNode As TreeNode, ByVal marketTable As DataTable)
        Dim parentRows() As DataRow = marketTable.Select("parentGroupID=" & inParentID)
        For Each parentRow As DataRow In parentRows
            Dim parentnode As TreeNode
            parentnode = New TreeNode(CStr(parentRow.Item("marketGroupName")))
            inTreeNode.Nodes.Add(parentnode)
            parentnode.Tag = parentRow.Item("marketGroupID")
            PopulateShipGroups(CInt(parentnode.Tag), parentnode, marketTable)
        Next parentRow
        Dim groupRows() As DataRow = _shipNameData.Tables(0).Select("marketGroupID=" & inParentID)
        For Each shipRow As DataRow In groupRows
            inTreeNode.Nodes.Add(shipRow.Item("typeName").ToString)
        Next
    End Sub
    Private Sub PopulateModuleGroups(ByVal inParentID As Integer, ByRef inTreeNode As TreeNode, ByVal marketTable As DataTable)
        Dim parentRows() As DataRow = marketTable.Select("parentGroupID=" & inParentID)
        For Each parentRow As DataRow In parentRows
            Dim parentnode As TreeNode
            parentnode = New TreeNode(CStr(parentRow.Item("marketGroupName")))
            parentnode.Name = parentnode.Text
            inTreeNode.Nodes.Add(parentnode)
            parentnode.Tag = parentRow.Item("marketGroupID")
            PopulateModuleGroups(CInt(parentnode.Tag), parentnode, marketTable)
        Next parentRow
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
        Dim sw As New StreamWriter(Path.Combine(_coreCacheFolder, "ShipGroups.bin"))
        For Each rootNode As TreeNode In tvwShips.Nodes
            WriteShipNodes(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteItemGroups(ByVal tvwItems As TreeView)
        Dim sw As New StreamWriter(Path.Combine(_coreCacheFolder, "ItemGroups.bin"))
        For Each rootNode As TreeNode In tvwItems.Nodes
            WriteGroupNodes(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteShipNodes(ByRef parentNode As TreeNode, ByVal sw As StreamWriter)
        sw.Write(parentNode.FullPath & ControlChars.CrLf)
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                WriteShipNodes(childNode, sw)
            Else
                sw.Write(childNode.FullPath & ControlChars.CrLf)
            End If
        Next
    End Sub
    Private Sub WriteGroupNodes(ByRef parentNode As TreeNode, ByVal sw As StreamWriter)
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

#Region "MSSQL Data Conversion Routines"

    Private Sub btnCheckDB_Click(sender As Object, e As EventArgs) Handles btnCheckDB.Click
        Call CheckSQLDatabase()
    End Sub

    Private Sub CheckSQLDatabase()

        Using evehqData As DataSet = GetStaticData("SELECT attributeGroup FROM dgmAttributeTypes")
            If evehqData Is Nothing Then
                ' We seem to be missing the data so lets add it in!
                Dim conn As New SqlConnection
                conn.ConnectionString = StaticDBConnection
                conn.Open()
                Call AddSQLAttributeGroupColumn(conn)
                Call CorrectSQLEveUnits(conn)
                Call DoSQLQuery(conn)
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End If
        End Using

    End Sub

    Private Sub AddSQLAttributeGroupColumn(ByVal connection As SqlConnection)

        Dim strSQL As String = "ALTER TABLE dgmAttributeTypes ADD attributeGroup INTEGER DEFAULT 0;"
        Dim keyCommand As New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        strSQL = "UPDATE dgmAttributeTypes SET attributeGroup=0;"
        keyCommand = New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        Dim line As String = My.Resources.attributeGroups.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("attributeID", StringComparison.Ordinal) = False And line <> "" Then
                Dim fields() As String = line.Split(",".ToCharArray)
                Dim strSQL2 As String = "UPDATE dgmAttributeTypes SET attributeGroup=" & fields(1) & " WHERE attributeID=" & fields(0) & ";"
                Dim keyCommand2 As New SqlCommand(strSQL2, connection)
                keyCommand2.ExecuteNonQuery()
            End If
        Next

    End Sub

    Private Sub CorrectSQLEveUnits(ByVal connection As SqlConnection)

        Const strSQL As String = "UPDATE dgmAttributeTypes SET unitID=122 WHERE unitID IS NULL;"
        Dim keyCommand As New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()

    End Sub

    Private Sub DoSQLQuery(ByVal connection As SqlConnection)

        Dim strSQL As String = My.Resources.SQLQueries.ToString
        Dim keyCommand As New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()

    End Sub

#End Region ' Converts the Base CCP Data Export into something EveHQ can use

#Region "Data Checking Routines"

    Private Sub btnCheckMarketGroup_Click(sender As Object, e As EventArgs) Handles btnCheckMarketGroup.Click

        Dim evehqData As DataSet
        Const strSQL As String = "SELECT * FROM invMarketGroups;"
        evehqData = GetStaticData(strSQL)

        Dim marketGroups As New List(Of Integer)
        For Each dr As DataRow In evehqData.Tables(0).Rows
            If marketGroups.Contains(CInt(dr.Item("marketGroupID"))) = False Then
                marketGroups.Add(CInt(dr.Item("marketGroupID")))
            End If
        Next

        Dim missingGroups As New List(Of Integer)
        For Each dr As DataRow In evehqData.Tables(0).Rows
            If IsDBNull(dr.Item("parentGroupID")) = False Then
                If marketGroups.Contains(CInt(dr.Item("parentGroupID"))) = False Then
                    If missingGroups.Contains(CInt(dr.Item("parentGroupID"))) = False Then
                        missingGroups.Add(CInt(dr.Item("parentGroupID")))
                    End If
                End If
            End If
        Next

        MessageBox.Show(missingGroups.Count.ToString)

    End Sub

#End Region

End Class
