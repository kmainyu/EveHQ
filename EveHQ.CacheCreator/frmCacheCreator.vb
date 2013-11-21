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

Imports System.Data.SQLite
Imports YamlDotNet.RepresentationModel

Public Class FrmCacheCreator

    Private Const CacheFolderName As String = "StaticData"
    Private Const StaticDB As String = "EveHQMaster"
    Private Const StaticDBConnection As String = "Server=localhost\SQLExpress; Database = " & StaticDB & "; Integrated Security = SSPI;" ' For SDE connection
    ReadOnly _sqLiteDBFolder As String = Path.Combine(Application.StartupPath, "EveCache")
    ReadOnly _sqLiteDB As String = Path.Combine(_sqLiteDBFolder, "EveHQMaster.db3")

    Shared marketGroupData As DataSet
    Shared shipGroupData As DataSet
    Shared shipNameData As DataSet
    Shared moduleData As DataSet
    Shared moduleEffectData As DataSet
    Shared moduleAttributeData As DataSet
    Shared coreCacheFolder As String
    Shared yamlTypes As SortedList(Of Integer, YAMLType) ' Key = typeID
    Shared yamlIcons As SortedList(Of Integer, String) ' Key = iconID, Value = iconFile
    Shared yamlCerts As New SortedList(Of Integer, YAMLCert) ' Key = CertID


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
        coreCacheFolder = Path.Combine(Application.StartupPath, CacheFolderName)
        If My.Computer.FileSystem.DirectoryExists(coreCacheFolder) = False Then
            ' Create the cache folder if it doesn't exist
            My.Computer.FileSystem.CreateDirectory(coreCacheFolder)
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

        yamlTypes = New SortedList(Of Integer, YAMLType)
        yamlIcons = New SortedList(Of Integer, String)

        ' Parse the icons file first
        Call ParseIconsYAMLFile()
        Call ParseTypesYAMLFile()
        ParseCertsYAMLFile()
    End Sub

    Private Sub ParseIconsYAMLFile()
        Using dataStream = New MemoryStream(My.Resources.iconIDs)
            Using reader = New StreamReader(dataStream)

                Dim yaml = New YamlDotNet.RepresentationModel.YamlStream()
                yaml.Load(reader)
                If yaml.Documents.Any() Then
                    ' Should only be 1 document so go with the first
                    Dim yamlDoc = yaml.Documents(0)
                    ' Cycle through the keys, which will be the typeIDs
                    Dim root = CType(yamlDoc.RootNode, YamlMappingNode)
                    For Each entry In root.Children
                        ' Parse the typeID
                        Dim iconID As Integer = CInt(CType(entry.Key, YamlScalarNode).Value)
                        ' Parse anything underneath
                        For Each subEntry In CType(entry.Value, YamlMappingNode).Children
                            ' Get the key and value of th sub entry
                            Dim keyName As String = CType(subEntry.Key, YamlScalarNode).Value
                            ' Do something based on the key
                            Select Case keyName
                                Case "iconFile"
                                    ' Pre-process the icon name to make it easier later on
                                    Dim iconName As String = CType(subEntry.Value, YamlScalarNode).Value.Trim
                                    ' Get the filename if the fullname starts with "res:"
                                    If iconName.StartsWith("res", StringComparison.Ordinal) Then
                                        iconName = iconName.Split("/".ToCharArray).Last
                                    End If
                                    ' Set the icon item
                                    yamlIcons.Add(iconID, iconName)
                            End Select
                        Next
                    Next
                End If
            End Using
        End Using

    End Sub

    Private Sub ParseCertsYAMLFile()
        Using dataStream = New MemoryStream(My.Resources.certificates)
            Using reader = New StreamReader(dataStream)

                Dim yaml = New YamlDotNet.RepresentationModel.YamlStream()
                yaml.Load(reader)

                If yaml.Documents.Count > 0 Then
                    ' Should only be 1 document so go with the first
                    Dim yamlDoc = yaml.Documents(0)
                    ' Cycle through the keys, which will be the certIDs
                    Dim root = CType(yamlDoc.RootNode, YamlMappingNode)
                    For Each entry In root.Children
                        Dim certID = CInt(CType(entry.Key, YamlScalarNode).Value)
                        Dim cert As New YAMLCert
                        cert.RecommendedFor = New List(Of Integer)
                        cert.CertID = certID
                        ' Parse anything underneath
                        Dim dataItem = TryCast(entry.Value, YamlMappingNode)
                        If dataItem IsNot Nothing Then
                            For Each subEntry In dataItem.Children
                                ' Get the key and value of th sub entry
                                Dim keyName As String = CType(subEntry.Key, YamlScalarNode).Value
                                ' Do something based on the key
                                Select Case keyName
                                    Case "description"
                                        ' Set the description
                                        cert.Description = CType(subEntry.Value, YamlScalarNode).Value
                                    Case "groupID"
                                        ' Set the groupID
                                        cert.GroupID = CInt(CType(subEntry.Value, YamlScalarNode).Value)
                                    Case "name"
                                        ' Set the name
                                        cert.Name = CType(subEntry.Value, YamlScalarNode).Value
                                    Case "recommendedFor"
                                        ' Set the type recommendations.
                                        cert.RecommendedFor = CType(subEntry.Value, YamlSequenceNode).Children.Select(Function(e) CInt(CType(e, YamlScalarNode).Value))
                                    Case "skillTypes"
                                        ' Set the required Skills
                                        Dim skillMaps = CType(subEntry.Value, YamlMappingNode)
                                        Dim reqSkills As New List(Of CertReqSkill)
                                        If skillMaps IsNot Nothing Then
                                            For Each skillMap In skillMaps.Children
                                                Dim reqSkill As New CertReqSkill
                                                reqSkill.SkillID = CInt(CType(skillMap.Key, YamlScalarNode).Value)
                                                reqSkill.SkillLevels = New Dictionary(Of String, Integer)
                                                For Each level In CType(skillMap.Value, YamlMappingNode).Children
                                                    reqSkill.SkillLevels.Add(CType(level.Key, YamlScalarNode).Value, CInt(CType(level.Value, YamlScalarNode).Value))
                                                Next
                                                reqSkills.Add(reqSkill)
                                            Next
                                        End If
                                        cert.RequiredSkills = reqSkills
                                End Select
                            Next
                            yamlCerts.Add(cert.CertID, cert)
                        End If
                    Next
                End If
            End Using
        End Using
    End Sub

    Private Sub ParseTypesYAMLFile()
        Using dataStream = New MemoryStream(My.Resources.typeIDs)
            Using reader = New StreamReader(dataStream)

                Dim yaml = New YamlDotNet.RepresentationModel.YamlStream()
                yaml.Load(reader)

                If yaml.Documents.Any() Then
                    ' Should only be 1 document so go with the first
                    Dim yamlDoc = yaml.Documents(0)
                    ' Cycle through the keys, which will be the typeIDs
                    Dim root = CType(yamlDoc.RootNode, YamlMappingNode)
                    For Each entry In root.Children
                        ' Parse the typeID
                        Dim typeID As Integer = CInt(CType(entry.Key, YamlScalarNode).Value)
                        Dim yamlItem As New YAMLType
                        yamlItem.TypeID = typeID
                        ' Parse anything underneath
                        Dim dataItem = TryCast(entry.Value, YamlMappingNode)
                        If dataItem IsNot Nothing Then
                            For Each subEntry In dataItem.Children
                                ' Get the key and value of th sub entry
                                Dim keyName As String = CType(subEntry.Key, YamlScalarNode).Value
                                ' Do something based on the key
                                Select Case keyName
                                    Case "iconID"
                                        ' Set the icon item
                                        yamlItem.IconID = CInt(CType(subEntry.Value, YamlScalarNode).Value)
                                    Case "masteries"
                                        ' set the various collections of certificates needed for each level of master
                                        yamlItem.Masteries = New Dictionary(Of Integer, List(Of Integer))
                                        Dim masteryLevels = CType(subEntry.Value, YamlMappingNode)
                                        For Each level In masteryLevels.Children
                                            Dim levelID = CInt(CType(level.Key, YamlScalarNode).Value)
                                            Dim certs = CType(level.Value, YamlSequenceNode).Children.Select(Function(node) CInt(CType(node, YamlScalarNode).Value)).ToList()
                                            yamlItem.Masteries.Add(levelID, certs)
                                        Next
                                End Select
                            Next
                        End If
                        ' Get the iconFile if relevant
                        If yamlIcons.ContainsKey(yamlItem.IconID) Then
                            yamlItem.IconName = yamlIcons(yamlItem.IconID)
                        End If
                        ' Add the item
                        yamlTypes.Add(yamlItem.TypeID, yamlItem)
                    Next
                End If
            End Using
        End Using
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
        Call LoadCerts()
        Call LoadCertRecs()
        Call LoadMasteries()
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

        Call LoadTypeMaterials()
        Call LoadBlueprints()
        Call LoadAssemblyArrays()
        Call LoadNPCCorps()
        Call LoadItemFlags()

    End Sub

    Private Sub LoadMasteries()
        StaticData.Masteries.Clear()

        For Each type In yamlTypes.Values.Where(Function(i) i.Masteries IsNot Nothing)
            Dim mastery As New Mastery()
            mastery.TypeId = type.TypeID
            mastery.RequiredCertificates = New SortedList(Of Integer, List(Of Integer))
            For Each rank In type.Masteries
                mastery.RequiredCertificates.Add(rank.Key, New List(Of Integer))
                For Each cert In rank.Value
                    mastery.RequiredCertificates(rank.Key).Add(cert)
                Next

            Next
            StaticData.Masteries.Add(mastery.TypeId, mastery)
        Next
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
                newItem.Id = CInt(itemRow.Item("typeID"))
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
                    newItem.MarketGroupId = CInt(itemRow.Item("marketGroupID"))
                Else
                    newItem.MarketGroupId = 0
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
                        mg.Id = CInt(itemRow.Item("marketGroupID"))
                        mg.Name = CStr(itemRow.Item("marketGroupName"))
                        If IsDBNull(itemRow.Item("parentGroupID")) = False Then
                            mg.ParentGroupId = CInt(itemRow.Item("parentGroupID"))
                        Else
                            mg.ParentGroupId = 0
                        End If
                        StaticData.MarketGroups.Add(mg.Id, mg)
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
        For Each groupID In yamlCerts.Values.Select(Function(c) c.GroupID).Distinct()
            Dim newCat As New CertificateCategory
            newCat.Id = groupID
            newCat.Name = StaticData.TypeGroups(groupID)
            StaticData.CertificateCategories.Add(newCat.Id.ToString, newCat)
        Next
    End Sub

    Private Sub LoadCerts()

        StaticData.Certificates.Clear()
        ' With Rubicon 1.0 certs are no longer in the database, but stored in yaml
        For Each cert In yamlCerts.Values
            Dim newCert As New Certificate
            newCert.GradesAndSkills.Add(CertificateGrade.Basic, New SortedList(Of Integer, Integer))
            newCert.GradesAndSkills.Add(CertificateGrade.Standard, New SortedList(Of Integer, Integer))
            newCert.GradesAndSkills.Add(CertificateGrade.Improved, New SortedList(Of Integer, Integer))
            newCert.GradesAndSkills.Add(CertificateGrade.Advanced, New SortedList(Of Integer, Integer))
            newCert.GradesAndSkills.Add(CertificateGrade.Elite, New SortedList(Of Integer, Integer))
                                      
            newCert.Id = cert.CertID
            newCert.Description = cert.Description
            newCert.GroupId = cert.GroupID
            newCert.Name = cert.Name
            newCert.RecommendedTypes = cert.RecommendedFor.ToList()
            For Each reqSkill In cert.RequiredSkills
                newCert.GradesAndSkills(CertificateGrade.Basic).Add(reqSkill.SkillID, reqSkill.SkillLevels("basic"))
                newCert.GradesAndSkills(CertificateGrade.Standard).Add(reqSkill.SkillID, reqSkill.SkillLevels("standard"))
                newCert.GradesAndSkills(CertificateGrade.Improved).Add(reqSkill.SkillID, reqSkill.SkillLevels("improved"))
                newCert.GradesAndSkills(CertificateGrade.Advanced).Add(reqSkill.SkillID, reqSkill.SkillLevels("advanced"))
                newCert.GradesAndSkills(CertificateGrade.Elite).Add(reqSkill.SkillID, reqSkill.SkillLevels("elite"))
            Next
            StaticData.Certificates.Add(newCert.Id, newCert)
        Next

        ' Trim certs of any skills that have a 0 rank value
        For Each cert In StaticData.Certificates.Values
            For Each gradeAndSkills In cert.GradesAndSkills
                Dim skills = gradeAndSkills.Value.ToList()
                For Each skill In skills
                    If skill.Value = 0 Then
                        gradeAndSkills.Value.Remove(skill.Key)
                    End If
                Next
            Next
        Next
    End Sub


    Private Sub LoadCertRecs()
        ' cert recommendations are now in the cert yaml data
        StaticData.CertificateRecommendations.Clear()

        For Each certRow In yamlCerts.Values
            For Each shipRec In certRow.RecommendedFor
                Dim certRec As New CertificateRecommendation
                certRec.ShipTypeId = shipRec
                certRec.CertificateId = certRow.CertID
                StaticData.CertificateRecommendations.Add(certRec)
            Next
        Next
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
                    Const MaxPreReqs As Integer = 10
                    Dim preReqSkills(MaxPreReqs) As String
                    Dim preReqSkillLevels(MaxPreReqs) As Integer
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
                    For prereq As Integer = 1 To MaxPreReqs
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

                For Each skill As Integer In cert.GradesAndSkills(CertificateGrade.Basic).Keys
                    Dim skillID As String = skill & "." & cert.GradesAndSkills(CertificateGrade.Basic)(skill).ToString
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
                'For Each certID As Integer In cert.RequiredCertificates.Keys
                '    If StaticData.CertUnlockCertificates.ContainsKey(certID) = False Then
                '        ' Create an arraylist and add the item
                '        certUnlocked = New List(Of Integer)
                '        certUnlocked.Add(cert.Id)
                '        StaticData.CertUnlockCertificates.Add(certID, certUnlocked)
                '    Else
                '        ' Fetch the item and add the new one
                '        certUnlocked = StaticData.CertUnlockCertificates(certID)
                '        certUnlocked.Add(cert.Id)
                '    End If
                'Next
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
                cSystem.Id = CInt(solarRow.Item("solarSystemID"))
                cSystem.Name = CStr(solarRow.Item("solarSystemName"))
                cSystem.Security = CDbl(solarRow.Item("security"))
                cSystem.RegionId = CInt(solarRow.Item("mapSolarSystems_regionID"))
                cSystem.ConstellationId = CInt(solarRow.Item("mapSolarSystems_constellationID"))
                cSystem.X = CDbl(solarRow.Item("x"))
                cSystem.Y = CDbl(solarRow.Item("y"))
                cSystem.Z = CDbl(solarRow.Item("z"))
                StaticData.SolarSystems.Add(cSystem.Id, cSystem)
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
                    s.StationId = CInt(reader.Item("stationID"))
                    s.StationName = reader.Item("stationName").ToString
                    s.CorpId = CInt(reader.Item("corporationID"))
                    s.SystemId = CInt(reader.Item("solarSystemID"))
                    s.RefiningEfficiency = CDbl(reader.Item("reprocessingEfficiency"))
                    s.StationTake = CDbl(reader.Item("reprocessingStationsTake"))
                    s.Services = operationServices(CInt(reader.Item("operationID")))
                    StaticData.Stations.Add(s.StationId, s)
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
                    a.AgentId = CInt(reader.Item("agentID"))
                    a.AgentName = reader.Item("agentName").ToString
                    a.AgentType = CInt(reader.Item("agentTypeID"))
                    a.CorporationId = CInt(reader.Item("corporationID"))
                    a.DivisionId = CInt(reader.Item("divisionID"))
                    a.IsLocator = CBool(reader.Item("isLocator"))
                    a.Level = CInt(reader.Item("level"))
                    a.LocationId = CInt(reader.Item("locationID"))
                    StaticData.Agents.Add(a.AgentId, a)
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
                at.AttributeId = CInt(evehqData.Tables(0).Rows(item).Item("attributeID"))
                at.AttributeName = CStr(evehqData.Tables(0).Rows(item).Item("attributeName")).Trim
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("displayName")) = False Then
                    at.DisplayName = CStr(evehqData.Tables(0).Rows(item).Item("displayName")).Trim
                Else
                    at.DisplayName = at.AttributeName
                End If
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("unitID")) = False Then
                    at.UnitId = CInt(evehqData.Tables(0).Rows(item).Item("unitID"))
                Else
                    at.UnitId = 0
                End If
                If IsDBNull(evehqData.Tables(0).Rows(item).Item("categoryID")) = False Then
                    at.CategoryId = CInt(evehqData.Tables(0).Rows(item).Item("categoryID"))
                Else
                    at.CategoryId = 0
                End If
                StaticData.AttributeTypes.Add(at.AttributeId, at)
            Next
        End Using

    End Sub

    Private Sub LoadTypeAttributes()

        StaticData.TypeAttributes.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM dgmTypeAttributes;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim ta As New TypeAttrib
                ta.TypeId = CInt(evehqData.Tables(0).Rows(item).Item("typeID"))
                ta.AttributeId = CInt(evehqData.Tables(0).Rows(item).Item("attributeID"))
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
                et.EffectId = CInt(evehqData.Tables(0).Rows(item).Item("effectID"))
                et.EffectName = CStr(evehqData.Tables(0).Rows(item).Item("effectName")).Trim
                StaticData.EffectTypes.Add(et.EffectId, et)
            Next
        End Using

    End Sub

    Private Sub LoadTypeEffects()

        StaticData.TypeEffects.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM dgmTypeEffects;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim te As New TypeEffect
                te.TypeId = CInt(evehqData.Tables(0).Rows(item).Item("typeID"))
                te.EffectId = CInt(evehqData.Tables(0).Rows(item).Item("effectID"))
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
                mt.Id = CInt(evehqData.Tables(0).Rows(item).Item("typeID"))
                mt.ParentId = CInt(evehqData.Tables(0).Rows(item).Item("parentTypeID"))
                mt.MetaGroupId = CInt(evehqData.Tables(0).Rows(item).Item("metaGroupID"))
                StaticData.MetaTypes.Add(mt.Id, mt)
            Next
        End Using

    End Sub

    Private Sub LoadTypeMaterials()

        StaticData.TypeMaterials.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT * FROM invTypeMaterials;")
            For item As Integer = 0 To evehqData.Tables(0).Rows.Count - 1
                Dim typeID As Integer = CInt(evehqData.Tables(0).Rows(item).Item("typeID"))
                Dim tm As New TypeMaterial
                If StaticData.TypeMaterials.ContainsKey(typeID) = True Then
                    tm = StaticData.TypeMaterials(typeID)
                Else
                    StaticData.TypeMaterials.Add(typeID, tm)
                End If
                tm.Materials.Add(CInt(evehqData.Tables(0).Rows(item).Item("materialTypeID")), CInt(evehqData.Tables(0).Rows(item).Item("quantity")))
            Next
        End Using

    End Sub

    Private Sub LoadBlueprints()

        StaticData.Blueprints.Clear()
        Dim evehqData As DataSet = GetStaticData("SELECT * FROM invBlueprintTypes;")
        ' Populate the main data
        For Each bp As DataRow In evehqData.Tables(0).Rows
            Dim bt As New Blueprint
            bt.Id = CInt(bp.Item("blueprintTypeID"))
            bt.ProductId = CInt(bp.Item("productTypeID"))
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
            StaticData.Blueprints.Add(bt.Id, bt)
        Next

        ' Good so far so let's add the material requirements
        evehqData = GetStaticData("SELECT invBuildMaterials.*, invTypes.typeName, invGroups.groupID, invGroups.categoryID FROM invGroups INNER JOIN (invTypes INNER JOIN invBuildMaterials ON invTypes.typeID = invBuildMaterials.requiredTypeID) ON invGroups.groupID = invTypes.groupID ORDER BY invBuildMaterials.typeID;")

        ' Go through each BP and refine the Dataset
        For Each bp As Blueprint In StaticData.Blueprints.Values
            ' Select resource data for the blueprint
            Dim bpRows() As DataRow = evehqData.Tables(0).Select("typeID=" & bp.Id.ToString)
            For Each req As DataRow In bpRows
                Dim newReq As New BlueprintResource
                newReq.Activity = CType(req.Item("activityID"), BlueprintActivity)
                newReq.DamagePerJob = CDbl(req.Item("damagePerJob"))
                newReq.TypeId = CInt(req.Item("requiredTypeID"))
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
                If bp.Resources(newReq.Activity).ContainsKey(newReq.TypeId) = False Then
                    bp.Resources(newReq.Activity).Add(newReq.TypeId, newReq)
                End If
            Next
            ' Select resource data for the product
            If bp.ProductId <> bp.Id Then
                bpRows = evehqData.Tables(0).Select("typeID=" & bp.ProductId.ToString)
                For Each req As DataRow In bpRows
                    Dim newReq As New BlueprintResource
                    newReq.TypeId = CInt(req.Item("requiredTypeID"))
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
                    If bp.Resources(newReq.Activity).ContainsKey(newReq.TypeId) = False Then
                        bp.Resources(newReq.Activity).Add(newReq.TypeId, newReq)
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
        Const ArraySQL As String = "SELECT * FROM ramAssemblyLineTypes WHERE activityID=1 AND (baseTimeMultiplier<>1 OR baseMaterialMultiplier<>1);"
        Const GroupSQL As String = "SELECT * FROM ramAssemblyLineTypeDetailPerGroup;"
        Const CatSQL As String = "SELECT * FROM ramAssemblyLineTypeDetailPerCategory;"
        Dim arrayDataSet As DataSet = GetStaticData(ArraySQL)
        Dim groupDataSet As DataSet = GetStaticData(GroupSQL)
        Dim catDataSet As DataSet = GetStaticData(CatSQL)

        ' Reset the list
        StaticData.AssemblyArrays.Clear()

        ' Populate the arrays
        For Each assArray As DataRow In arrayDataSet.Tables(0).Rows
            Dim newArray As New AssemblyArray
            newArray.Id = CStr(assArray.Item("assemblyLineTypeID"))
            newArray.Name = CStr(assArray.Item("assemblyLineTypeName"))
            newArray.MaterialMultiplier = CDbl(assArray.Item("baseMaterialMultiplier"))
            newArray.TimeMultiplier = CDbl(assArray.Item("baseTimeMultiplier"))

            Dim groupRows() As DataRow = groupDataSet.Tables(0).Select("assemblyLineTypeID=" & newArray.Id)
            For Each group As DataRow In groupRows
                newArray.AllowableGroups.Add(CInt(group.Item("groupID")))
            Next
            Dim catRows() As DataRow = catDataSet.Tables(0).Select("assemblyLineTypeID=" & newArray.Id)
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

        StaticData.NpcCorps.Clear()
        Using evehqData As DataSet = GetStaticData("SELECT itemID, itemName FROM invUniqueNames WHERE groupID=2;")
            For Each corpRow As DataRow In evehqData.Tables(0).Rows
                StaticData.NpcCorps.Add(CInt(corpRow.Item("itemID")), CStr(corpRow.Item("itemname")))
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
        s = New FileStream(Path.Combine(coreCacheFolder, "Items.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Types)
        s.Flush()
        s.Close()

        ' Market Groups
        s = New FileStream(Path.Combine(coreCacheFolder, "MarketGroups.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.MarketGroups)
        s.Flush()
        s.Close()

        ' Item Market Groups
        s = New FileStream(Path.Combine(coreCacheFolder, "ItemMarketGroups.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.ItemMarketGroups)
        s.Flush()
        s.Close()

        ' Item List
        s = New FileStream(Path.Combine(coreCacheFolder, "ItemList.dat"), FileMode.Create)

        Serializer.Serialize(s, StaticData.TypeNames)
        s.Flush()
        s.Close()

        ' Item Groups
        s = New FileStream(Path.Combine(coreCacheFolder, "ItemGroups.dat"), FileMode.Create)

        Serializer.Serialize(s, StaticData.TypeGroups)
        s.Flush()
        s.Close()

        ' Items Cats
        s = New FileStream(Path.Combine(coreCacheFolder, "ItemCats.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.TypeCats)
        s.Flush()
        s.Close()

        ' Group Cats
        s = New FileStream(Path.Combine(coreCacheFolder, "GroupCats.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.GroupCats)
        s.Flush()
        s.Close()

        ' Cert Categories
        s = New FileStream(Path.Combine(coreCacheFolder, "CertCats.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertificateCategories)
        s.Flush()
        s.Close()

        ' Certs
        s = New FileStream(Path.Combine(coreCacheFolder, "Certs.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Certificates)
        s.Flush()
        s.Close()

        ' Cert Recommendations
        s = New FileStream(Path.Combine(coreCacheFolder, "CertRec.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertificateRecommendations)
        s.Flush()
        s.Close()

        'Masteries
        s = New FileStream(Path.Combine(coreCacheFolder, "Masteries.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Masteries)
        s.Flush()
        s.Close()

        ' Unlocks
        s = New FileStream(Path.Combine(coreCacheFolder, "ItemUnlocks.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.ItemUnlocks)
        s.Flush()
        s.Close()

        ' SkillUnlocks
        s = New FileStream(Path.Combine(coreCacheFolder, "SkillUnlocks.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.SkillUnlocks)
        s.Flush()
        s.Close()

        ' CertSkills
        s = New FileStream(Path.Combine(coreCacheFolder, "CertSkills.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.CertUnlockSkills)
        s.Flush()
        s.Close()

        ' Regions
        s = New FileStream(Path.Combine(coreCacheFolder, "Regions.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Regions)
        s.Flush()
        s.Close()

        ' Constellations
        s = New FileStream(Path.Combine(coreCacheFolder, "Constellations.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Constellations)
        s.Flush()
        s.Close()

        ' Solar Systems
        s = New FileStream(Path.Combine(coreCacheFolder, "Systems.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.SolarSystems)
        s.Flush()
        s.Close()

        ' Stations
        s = New FileStream(Path.Combine(coreCacheFolder, "Stations.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Stations)
        s.Flush()
        s.Close()

        ' Divisions
        s = New FileStream(Path.Combine(coreCacheFolder, "Divisions.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Divisions)
        s.Flush()
        s.Close()

        ' Agents
        s = New FileStream(Path.Combine(coreCacheFolder, "Agents.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Agents)
        s.Flush()
        s.Close()

        ' Attribute Types
        s = New FileStream(Path.Combine(coreCacheFolder, "AttributeTypes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.AttributeTypes)
        s.Flush()
        s.Close()

        ' Type Attributes
        s = New FileStream(Path.Combine(coreCacheFolder, "TypeAttributes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.TypeAttributes)
        s.Flush()
        s.Close()

        ' Attribute Units
        s = New FileStream(Path.Combine(coreCacheFolder, "Units.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.AttributeUnits)
        s.Flush()
        s.Close()

        ' Effect Types
        s = New FileStream(Path.Combine(coreCacheFolder, "EffectTypes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.EffectTypes)
        s.Flush()
        s.Close()

        ' Type Effects
        s = New FileStream(Path.Combine(coreCacheFolder, "TypeEffects.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.TypeEffects)
        s.Flush()
        s.Close()

        ' Meta Groups
        s = New FileStream(Path.Combine(coreCacheFolder, "MetaGroups.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.MetaGroups)
        s.Flush()
        s.Close()

        ' Meta Types
        s = New FileStream(Path.Combine(coreCacheFolder, "MetaTypes.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.MetaTypes)
        s.Flush()
        s.Close()

        ' Type Materials
        s = New FileStream(Path.Combine(coreCacheFolder, "TypeMaterials.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.TypeMaterials)
        s.Flush()
        s.Close()

        ' Blueprint Types
        s = New FileStream(Path.Combine(coreCacheFolder, "Blueprints.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.Blueprints)
        s.Flush()
        s.Close()

        ' Assembly Arrays
        s = New FileStream(Path.Combine(coreCacheFolder, "AssemblyArrays.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.AssemblyArrays)
        s.Flush()
        s.Close()

        ' NPC Corps
        s = New FileStream(Path.Combine(coreCacheFolder, "NPCCorps.dat"), FileMode.Create)
        Serializer.Serialize(s, StaticData.NpcCorps)
        s.Flush()
        s.Close()

        ' Item Flags
        s = New FileStream(Path.Combine(coreCacheFolder, "ItemFlags.dat"), FileMode.Create)
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
            marketGroupData = GetStaticData(strSQL)
            If marketGroupData IsNot Nothing Then
                If marketGroupData.Tables(0).Rows.Count <> 0 Then
                    Market.MarketGroupList.Clear()
                    For Each row As DataRow In marketGroupData.Tables(0).Rows
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

        Const MaxPreReqs As Integer = 10
        For Each newSkill As Core.EveSkill In Core.HQ.SkillListID.Values
            Dim preReqSkills(MaxPreReqs) As Integer
            Dim preReqSkillLevels(MaxPreReqs) As Integer

            ' Fetch the attributes for the item
            Dim skillID As Integer = CInt(newSkill.ID)

            For Each att As TypeAttrib In From ta In skillAtts Where ta.TypeId = skillID
                Select Case att.AttributeId
                    Case 180
                        Select Case CInt(att.Value)
                            Case 164
                                newSkill.Pa = "Charisma"
                            Case 165
                                newSkill.Pa = "Intelligence"
                            Case 166
                                newSkill.Pa = "Memory"
                            Case 167
                                newSkill.Pa = "Perception"
                            Case 168
                                newSkill.Pa = "Willpower"
                        End Select
                    Case 181
                        Select Case CInt(att.Value)
                            Case 164
                                newSkill.Sa = "Charisma"
                            Case 165
                                newSkill.Sa = "Intelligence"
                            Case 166
                                newSkill.Sa = "Memory"
                            Case 167
                                newSkill.Sa = "Perception"
                            Case 168
                                newSkill.Sa = "Willpower"
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
            For prereq As Integer = 1 To MaxPreReqs
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
            shipGroupData = GetStaticData(strSQL)
            If shipGroupData IsNot Nothing Then
                If shipGroupData.Tables(0).Rows.Count <> 0 Then
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
            shipNameData = GetStaticData(strSQL)
            If shipNameData IsNot Nothing Then
                If shipNameData.Tables(0).Rows.Count <> 0 Then
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
                    ShipLists.ShipList.Clear()
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
                                ShipLists.ShipList.Add(newShip.Name, newShip)
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
                    ShipLists.ShipList.Add(newShip.Name, newShip)
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
        ShipLists.ShipListKeyName.Clear()
        ShipLists.ShipListKeyID.Clear()
        For Each baseShip As Ship In ShipLists.ShipList.Values
            ShipLists.ShipListKeyName.Add(baseShip.Name, baseShip.ID)
            ShipLists.ShipListKeyID.Add(baseShip.ID, baseShip.Name)
        Next
    End Sub
    Private Sub LoadModuleData()
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID"
            strSQL &= " FROM invCategories INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invCategories.categoryID = invGroups.categoryID"
            strSQL &= " WHERE (((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)) OR invTypes.groupID=1010"
            strSQL &= " ORDER BY invTypes.typeName;"
            moduleData = GetStaticData(strSQL)
            If moduleData IsNot Nothing Then
                If moduleData.Tables(0).Rows.Count <> 0 Then
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
            moduleEffectData = GetStaticData(strSQL)
            If moduleEffectData IsNot Nothing Then
                If moduleEffectData.Tables(0).Rows.Count <> 0 Then
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

            moduleAttributeData = GetStaticData(strSQL)
            If moduleAttributeData IsNot Nothing Then
                If moduleAttributeData.Tables(0).Rows.Count <> 0 Then
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
                    ModuleLists.ModuleMetaTypes.Clear()
                    ModuleLists.ModuleMetaGroups.Clear()
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
            ModuleLists.ModuleList.Clear()
            ModuleLists.ModuleListName.Clear()
            Implants.ImplantList.Clear()
            Boosters.BoosterList.Clear()
            For Each row As DataRow In moduleData.Tables(0).Rows
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
                If yamlTypes.ContainsKey(CInt(newModule.ID)) Then
                    newModule.Icon = yamlTypes(CInt(newModule.ID)).IconName
                End If
                If IsDBNull(row.Item("marketGroupID")) = False Then
                    newModule.MarketGroup = CInt(row.Item("marketGroupID"))
                Else
                    newModule.MarketGroup = 0
                End If
                newModule.Cpu = 0
                newModule.Pg = 0
                newModule.Calibration = 0
                newModule.CapUsage = 0
                newModule.ActivationTime = 0
                ModuleLists.ModuleList.Add(newModule.ID, newModule)
                ModuleLists.ModuleListName.Add(newModule.Name, newModule.ID)

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
                For Each row As DataRow In moduleData.Tables(0).Rows
                    If IsDBNull(row.Item("marketGroupID")) = True Then
                        modID = CInt(row.Item("typeID"))
                        nModule = ModuleLists.ModuleList(modID)
                        If ModuleLists.ModuleMetaTypes.ContainsKey(modID) = True Then
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
                    If ModuleLists.ModuleList.ContainsKey(typeID) = True Then
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
            For Each modRow As DataRow In moduleEffectData.Tables(0).Rows
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
            For Each modRow As DataRow In moduleAttributeData.Tables(0).Rows
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

                ' Modify the resists attribute values of damage controls and bastion mods - this is to stack up later on
                If attMod.DatabaseGroup = 60 Or attMod.ID = 33400 Then
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
                        attMod.Pg = attValue
                    Case 50
                        attMod.Cpu = attValue
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
            For Each cMod As ShipModule In ModuleLists.ModuleList.Values
                If ModuleLists.ModuleMetaGroups.ContainsKey(cMod.ID) = True Then
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
            For Each cMod As ShipModule In ModuleLists.ModuleList.Values
                If cMod.IsCharge = True Then
                    If Charges.ChargeGroups.ContainsKey(cMod.ID) = False Then
                        Charges.ChargeGroups.Add(cMod.ID, cMod.MarketGroup & "_" & cMod.DatabaseGroup & "_" & cMod.Name & "_" & cMod.ChargeSize)
                    End If
                End If
            Next
            ' Check for drone missiles
            For Each cMod As ShipModule In ModuleLists.ModuleList.Values
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
            For Each impMod As ShipModule In ModuleLists.ModuleList.Values
                If impMod.IsImplant = True Then
                    Implants.ImplantList.Add(impMod.Name, impMod)
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
        Dim marketTable As DataTable = marketGroupData.Tables(0)
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
        Dim factionRows() As DataRow = shipNameData.Tables(0).Select("ISNULL(marketGroupID, 0) = 0")
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
        Dim marketTable As DataTable = marketGroupData.Tables(0)
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
        marketGroupData = Nothing
        shipGroupData = Nothing
        shipNameData = Nothing
        moduleData = Nothing
        moduleEffectData = Nothing
        moduleAttributeData = Nothing
        GC.Collect()
    End Sub

#End Region

#Region "HQF Cache Writing Routines"

    Private Sub SaveHQFCacheData()

        ' Dump HQF data to folder
        Dim s As FileStream

        ' Save ships
        s = New FileStream(Path.Combine(coreCacheFolder, "ships.dat"), FileMode.Create)
        Serializer.Serialize(s, ShipLists.ShipList)
        s.Flush()
        s.Close()

        ' Save modules
        s = New FileStream(Path.Combine(coreCacheFolder, "modules.dat"), FileMode.Create)
        Serializer.Serialize(s, ModuleLists.ModuleList)
        s.Flush()
        s.Close()

        ' Save implants
        s = New FileStream(Path.Combine(coreCacheFolder, "implants.dat"), FileMode.Create)
        Serializer.Serialize(s, Implants.ImplantList)
        s.Flush()
        s.Close()

        ' Save boosters
        s = New FileStream(Path.Combine(coreCacheFolder, "boosters.dat"), FileMode.Create)
        Serializer.Serialize(s, Boosters.BoosterList)
        s.Flush()
        s.Close()

        ' Save skills
        s = New FileStream(Path.Combine(coreCacheFolder, "skills.dat"), FileMode.Create)
        Serializer.Serialize(s, SkillLists.SkillList)
        s.Flush()
        s.Close()

        ' Save attributes
        s = New FileStream(Path.Combine(coreCacheFolder, "attributes.dat"), FileMode.Create)
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
            Core.HQ.DataError = e.Message
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
        Dim groupRows() As DataRow = shipNameData.Tables(0).Select("marketGroupID=" & inParentID)
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
        Dim sw As New StreamWriter(Path.Combine(coreCacheFolder, "ShipGroups.bin"))
        For Each rootNode As TreeNode In tvwShips.Nodes
            WriteShipNodes(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteItemGroups(ByVal tvwItems As TreeView)
        Dim sw As New StreamWriter(Path.Combine(coreCacheFolder, "ItemGroups.bin"))
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
        MessageBox.Show("SQL Database check complete!")
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

        Const StrSQL As String = "UPDATE dgmAttributeTypes SET unitID=122 WHERE unitID IS NULL;"
        Dim keyCommand As New SqlCommand(StrSQL, connection)
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
        Const StrSQL As String = "SELECT * FROM invMarketGroups;"
        evehqData = GetStaticData(StrSQL)

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
