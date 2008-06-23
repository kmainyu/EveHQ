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
    Dim FittingTabList As New ArrayList
    Shared UseSerializableData As Boolean = False

#Region "Class Wide Variables"

    Shared MarketGroupData As DataSet

    Shared shipGroupData As DataSet
    Shared shipNameData As DataSet

    Shared moduleData As DataSet
    Shared moduleEffectData As DataSet
    Shared moduleAttributeData As DataSet

    Dim itemCount As Integer = 0
    Dim startUp As Boolean = False

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
                UseSerializableData = True
            Else
                UseSerializableData = False
            End If

            'Me.LoadBonuses()
            Engine.BuildEffectsMap()
            Engine.BuildShipEffectsMap()
            Engine.BuildImplantEffectsMap()
            ' Check for the existence of the binary data
            If UseSerializableData = True Then
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\attributes.txt") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\attributes.txt", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    Attributes.AttributeList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\ships.txt") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\ships.txt", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    ShipLists.shipList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                    For Each cShip As Ship In ShipLists.shipList.Values
                        ShipLists.shipListKeyID.Add(cShip.ID, cShip.Name)
                        ShipLists.shipListKeyID.Add(cShip.Name, cShip.ID)
                    Next
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\modules.txt") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\modules.txt", FileMode.Open)
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
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\implants.txt") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\implants.txt", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    Implants.implantList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(HQF.Settings.HQFCacheFolder & "\skills.txt") = True Then
                    Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\skills.txt", FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    SkillLists.SkillList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                Call Me.BuildAttributeQuickList()
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
                                                            Call Me.BuildAttributeQuickList()
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
            End If
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
            Return False
        End Try
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
            MarketGroupData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If MarketGroupData IsNot Nothing Then
                If MarketGroupData.Tables(0).Rows.Count <> 0 Then
                    Market.MarketGroupList.Clear()
                    For Each row As DataRow In MarketGroupData.Tables(0).Rows
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
            If line.Trim <> "" Then
                att = line.Split(",".ToCharArray)
                attData = New Attribute
                attData.ID = att(0)
                attData.Name = att(1)
                attData.DisplayName = att(2)
                attData.GraphicID = att(3)
                attData.UnitName = att(4)
                attData.AttributeGroup = att(5)
                attributes.AttributeList.Add(attData.ID, attData)
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
                            newSkill.Attributes.Add(skillRow.Item("attributeID"), skillRow.Item("valueInt"))
                        Else
                            newSkill.Attributes.Add(skillRow.Item("attributeID"), skillRow.Item("valueFloat"))
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
            shipGroupData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If shipGroupData IsNot Nothing Then
                If shipGroupData.Tables(0).Rows.Count <> 0 Then
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
            shipNameData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If shipNameData IsNot Nothing Then
                If shipNameData.Tables(0).Rows.Count <> 0 Then
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
                            Case 600
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
                    newShip.Attributes.Add("10010", 0)
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
        For Each shipRow As DataRow In shipNameData.Tables(0).Rows
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
            strSQL &= " WHERE (((invCategories.categoryID) In (7,8,18,20)) AND ((invTypes.published)=True))"
            strSQL &= " ORDER BY invTypes.typeName;"
            moduleData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If moduleData IsNot Nothing Then
                If moduleData.Tables(0).Rows.Count <> 0 Then
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
            moduleEffectData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If moduleEffectData IsNot Nothing Then
                If moduleEffectData.Tables(0).Rows.Count <> 0 Then
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
            strSQL &= " WHERE (((invCategories.categoryID) In (7,8,18,20)) AND ((invTypes.published)=True))"
            strSQL &= " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;"

            moduleAttributeData = EveHQ.Core.DataFunctions.GetData(strSQL)
            If moduleAttributeData IsNot Nothing Then
                If moduleAttributeData.Tables(0).Rows.Count <> 0 Then
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
            For Each row As DataRow In moduleData.Tables(0).Rows
                Dim newModule As New ShipModule
                newModule.ID = row.Item("typeID").ToString
                newModule.Name = row.Item("typeName").ToString
                newModule.Description = row.Item("description").ToString
                newModule.DatabaseGroup = row.Item("groupID").ToString
                newModule.DatabaseCategory = row.Item("categoryID").ToString
                newModule.BasePrice = CDbl(row.Item("baseprice"))
                newModule.Volume = CDbl(row.Item("volume"))
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
                ' If an implant then add to the implants list
                ' Determine whether implant, drone, charge etc
                Select Case CInt(row.Item("categoryID"))
                    Case 8 ' Charge
                        newModule.IsCharge = True
                    Case 18 ' Drone
                        newModule.IsDrone = True
                    Case 20 ' Implant
                        ' Exclude groups 303 (Boosters) & 304 (DNA Mutators)
                        If CInt(row.Item("groupID")) <> 303 And CInt(row.Item("groupID")) <> 304 Then
                            Dim newImplant As New Implant
                            newImplant.ID = newModule.ID
                            newImplant.Name = newModule.Name
                            newImplant.Description = newModule.Description
                            newImplant.DatabaseGroup = newModule.DatabaseGroup
                            newImplant.BasePrice = newModule.BasePrice
                            newImplant.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(newImplant.ID)
                            newImplant.MarketGroup = newModule.MarketGroup
                            newImplant.MetaType = newModule.MetaType
                            Implants.implantList.Add(newImplant.Name, newImplant)
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
                For Each row As DataRow In moduleData.Tables(0).Rows
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
            For Each modRow As DataRow In moduleEffectData.Tables(0).Rows
                Dim effMod As ShipModule = CType(ModuleLists.moduleList.Item(modRow.Item("typeID").ToString), ShipModule)
                Select Case CInt(modRow.Item("effectID"))
                    Case 11 ' Low slot
                        effMod.Slot = 2
                    Case 12 ' High slot
                        effMod.Slot = 8
                    Case 13 ' Mid slot
                        effMod.Slot = 4
                    Case 2663 ' Rig slot
                        effMod.Slot = 1
                    Case 40
                        effMod.IsLauncher = True
                    Case 42
                        effMod.IsTurret = True
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
            For Each modRow As DataRow In moduleAttributeData.Tables(0).Rows
                Dim attMod As ShipModule = CType(ModuleLists.moduleList.Item(modRow.Item("typeID").ToString), ShipModule)
                If lastModName <> modRow.Item("typeName").ToString And lastModName <> "" Then
                    pSkillName = "" : sSkillName = "" : tSkillName = ""
                End If
                ' Now get, modify (if applicable) and add the "attribute"
                If IsDBNull(modRow.Item("valueInt")) = True Then
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

                attMod.Attributes.Add(modRow.Item("attributeID").ToString, attValue)

                Select Case CInt(modRow.Item("attributeID"))
                    Case 30
                        attMod.PG = attValue
                    Case 50
                        attMod.CPU = attValue
                    Case 6
                        attMod.CapUsage = attValue
                    Case 73
                        attMod.ActivationTime = attValue / 1000
                    Case 128
                        attMod.ChargeSize = CInt(attValue)
                    Case 1153
                        attMod.Calibration = CInt(attValue)
                    Case 331 ' Slot Type for Implants
                        Dim attImplant As Implant = CType(Implants.implantList.Item(modRow.Item("typeName").ToString), Implant)
                        attImplant.Slot = CInt(attValue)
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
                ' Add to the ChargeGroups if it doesn't exist
                If attMod.IsCharge = True And Charges.ChargeGroups.Contains(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize) = False Then
                    Charges.ChargeGroups.Add(attMod.MarketGroup & "_" & attMod.DatabaseGroup & "_" & attMod.Name & "_" & attMod.ChargeSize)
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
            Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
            ' Extract the bonuses from the included resource file
            Dim implantsList() As String = My.Resources.Implants.Split(ControlChars.CrLf.ToCharArray)
            Dim implantData() As String
            Dim implantName As String = ""
            Dim implantBonus As String = ""
            Dim implantBonusValue As Double = 0
            Dim implantGroup As String = ""
            For Each cImplant As String In implantsList
                If cImplant.Trim <> "" Then
                    implantData = cImplant.Split(",".ToCharArray)
                    implantName = implantData(0)
                    implantBonus = implantData(1)
                    implantBonusValue = Double.Parse(implantData(2), Globalization.NumberStyles.Number, culture)
                    'implantBonusValue = CInt(implantData(2))
                    implantGroup = implantData(3)
                    If Implants.implantList.ContainsKey(implantName) = True Then
                        Dim bImplant As Implant = CType(Implants.implantList(implantName), Implant)
                        Dim newBonus As New ImplantBonus
                        newBonus.BonusName = implantBonus
                        newBonus.BonusValue = implantBonusValue
                        bImplant.ImplantBonuses.Add(newBonus)
                        bImplant.ImplantGroups.Add(implantGroup)
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

#End Region

#Region "Form Initialisation & Closing Routines"

    Private Sub frmHQF_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Save each fit into it's own file
        For Each fit As String In Fittings.FittingList.Keys
            Dim sw As New IO.StreamWriter(Settings.HQFFolder & "\" & fit & ".hqf")
            sw.WriteLine("[" & fit & "]")
            For Each shipMod As String In CType(Fittings.FittingList.Item(fit), ArrayList)
                sw.WriteLine(shipMod)
            Next
            sw.Flush()
            sw.Close()
        Next
        ' Save the Settings
        Call Settings.HQFSettings.SaveHQFSettings()
    End Sub
    Private Sub frmHQF_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        startUp = True

        AddHandler ShipModule.ShowModuleMarketGroup, AddressOf Me.UpdateMarketGroup

        ' Load the settings!
        Call Settings.HQFSettings.LoadHQFSettings()

        ' Load up a collection of pilots from the EveHQ Core
        Call Me.LoadPilots()

        ' Load saved setups into the fitting array
        Call Me.LoadFittings()

        ' Set the MetaType Filter
        Call Me.SetMetaTypeFilters()

        ' Clear tabs and fitted ship list
        ShipLists.fittedShipList.Clear()
        tabHQF.TabPages.Clear()
        tabHQF.TabPages.Add(tabDockingStation)

        If UseSerializableData = True Then
            Call Me.ShowShipGroups()
            Call Me.ShowMarketGroups()
        Else
            Call Me.ShowShipMarketGroups()
            Call Me.ShowModuleMarketGroups()
            ' Generate the cache
            Call Me.GenerateHQFCache()
        End If

        startUp = False

        ' Check if we need to restore tabs from the previous setup
        If HQF.Settings.HQFSettings.RestoreLastSession = True Then
            For Each shipFit As String In HQF.Settings.HQFSettings.OpenFittingList
                If Fittings.FittingList.ContainsKey(shipFit) = True Then
                    ' Create the tab and display
                    If FittingTabList.Contains(shipFit) = False Then
                        Call Me.CreateFittingTabPage(shipFit)
                    End If
                    tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
                    currentShipSlot.UpdateEverything()
                End If
            Next
        End If

    End Sub
    Private Sub LoadFittings()
        Fittings.FittingList.Clear()
        Dim fileText As String = ""
        Dim sr As IO.StreamReader
        Dim mods() As String
        For Each filename As String In My.Computer.FileSystem.GetFiles(Settings.HQFFolder, FileIO.SearchOption.SearchTopLevelOnly, "*.hqf")
            sr = New IO.StreamReader(filename)
            fileText = sr.ReadToEnd
            Dim fittingMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(fileText, "\[(?<ShipName>.*),\s?(?<FittingName>.*)\]")
            If fittingMatch.Success = True Then
                ' Appears to be a match so lets check the ship type
                If ShipLists.shipList.Contains(fittingMatch.Groups.Item("ShipName").Value) = True Then
                    ' Ship type OK, lets see if the fitting exists
                    If Fittings.FittingList.ContainsKey(fittingMatch.Groups.Item("ShipName").Value & ", " & fittingMatch.Groups.Item("FittingName").Value) = False Then
                        ' Finally! Seems to be a valid file so lets load it up
                        mods = fileText.Split(ControlChars.CrLf.ToCharArray)
                        Dim newFit As New ArrayList
                        For Each ShipMod As String In mods
                            If ShipMod.StartsWith("[") = False And ShipMod <> "" Then
                                newFit.Add(ShipMod)
                            End If
                        Next
                        Fittings.FittingList.Add(fittingMatch.Groups.Item("ShipName").Value & ", " & fittingMatch.Groups.Item("FittingName").Value, newFit)
                    Else
                        Dim msg As String = "Duplicated fitting name '" & fittingMatch.Groups.Item("FittingName").Value & "' in file:"
                        msg &= filename
                        MessageBox.Show(msg, "Error Importing Fitting", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Else
                    Dim msg As String = "Unrecognised ship name '" & fittingMatch.Groups.Item("ShipName").Value & "' in file:"
                    msg &= filename
                    MessageBox.Show(msg, "Error Importing Fitting", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                End If
            Else
                MessageBox.Show(filename & "is not a valid fitting file.", "Error Importing Fitting", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            sr.Close()
        Next
        Call Me.UpdateFittingsTree()
    End Sub
    Private Sub ShowShipMarketGroups()
        tvwShips.BeginUpdate()
        tvwShips.Nodes.Clear()
        Dim marketTable As DataTable = MarketGroupData.Tables(0)
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
        Dim groupRows() As DataRow = shipNameData.Tables(0).Select("marketGroupID=" & inParentID)
        For Each shipRow As DataRow In groupRows
            inTreeNode.Nodes.Add(shipRow.Item("typeName").ToString)
        Next
    End Sub
    Private Sub ShowModuleMarketGroups()
        tvwItems.BeginUpdate()
        tvwItems.Nodes.Clear()
        Dim marketTable As DataTable = MarketGroupData.Tables(0)
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
        tvwItems.EndUpdate()
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
        Dim sr As New StreamReader(HQF.Settings.HQFCacheFolder & "\ShipGroups.txt")
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
        Dim sr As New StreamReader(HQF.Settings.HQFCacheFolder & "\ItemGroups.txt")
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
        tvwItems.Sorted = True
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
        If My.Computer.FileSystem.FileExists(HQF.Settings.HQFFolder & "\HQFPilotSettings.txt") = True Then
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

        lblWarpSpeed.Text = "Warp Speed: " & FormatNumber(selShip.WarpSpeed * 3, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & " au/s"

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
            Call Me.UpdateFittingsTree()
            tabHQF.SelectedTab = tabHQF.TabPages(fittingKeyName)
            currentShipSlot.UpdateEverything()
        Else
            MessageBox.Show("Unable to Create New Fitting!", "New Fitting Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
#End Region

#Region "Module Display, Filter and Search Options"
    Private Sub tvwItems_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwItems.NodeMouseClick
        tvwItems.SelectedNode = e.Node
    End Sub
    Private Sub tvwItems_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwItems.AfterSelect
        If e.Node.Nodes.Count = 0 Then
            Call Me.ShowFilteredModules(e.Node)
        End If
    End Sub
    Private Sub MetaFilterChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilter1.CheckedChanged, chkFilter2.CheckedChanged, chkFilter4.CheckedChanged, chkFilter8.CheckedChanged, chkFilter16.CheckedChanged, chkFilter32.CheckedChanged
        If startUp = False Then
            Dim chkBox As CheckBox = CType(sender, CheckBox)
            Dim changedFilter As Integer = CInt(chkBox.Tag)
            HQF.Settings.HQFSettings.ModuleFilter = HQF.Settings.HQFSettings.ModuleFilter Xor changedFilter
            If tvwItems.Tag IsNot Nothing Then
                If tvwItems.Tag.ToString = "Search" Then
                    Call ShowSearchedModules()
                Else
                    Call ShowFilteredModules(tvwItems.SelectedNode)
                End If
            End If
        End If
    End Sub
    Private Sub ShowFilteredModules(ByVal groupNode As TreeNode)

        ' Reset checkbox colours
        chkFilter1.ForeColor = Color.Red
        chkFilter2.ForeColor = Color.Red
        chkFilter4.ForeColor = Color.Red
        chkFilter8.ForeColor = Color.Red
        chkFilter16.ForeColor = Color.Red
        chkFilter32.ForeColor = Color.Red

        Dim groupID As String
        If groupNode.Nodes.Count = 0 Then
            groupID = groupNode.Tag.ToString
        Else
            groupID = tvwItems.Tag.ToString
        End If
        lvwItems.BeginUpdate()
        lvwItems.Items.Clear()
        For Each shipMod As ShipModule In ModuleLists.moduleList.Values
            If shipMod.MarketGroup = groupID Then
                If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                    Dim newModule As New ListViewItem
                    newModule.Name = shipMod.ID
                    newModule.Text = shipMod.Name
                    newModule.ToolTipText = shipMod.Name
                    newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                    newModule.SubItems.Add(shipMod.CPU.ToString)
                    newModule.SubItems.Add(shipMod.PG.ToString)
                    Select Case shipMod.Slot
                        Case 8 ' High
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                            newModule.ImageKey = "hiSlot"
                        Case 4 ' Mid
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                            newModule.ImageKey = "midSlot"
                        Case 2 ' Low
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                            newModule.ImageKey = "lowSlot"
                        Case 1 ' Rig
                            newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                            newModule.ImageKey = "rigSlot"
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
        If lvwItems.Items.Count = 0 Then
            lvwItems.Items.Add("<Empty - Please check filters>")
            lvwItems.Enabled = False
        Else
            lvwItems.Enabled = True
        End If
        lvwItems.EndUpdate()
        tvwItems.Tag = "Search"
        tvwItems.Tag = groupID
    End Sub
    Private Sub ShowSearchedModules()
        If Len(txtSearchModules.Text) > 2 Then
            Dim strSearch As String = txtSearchModules.Text.Trim.ToLower
            Dim results As New SortedList(Of String, String)
            For Each item As String In EveHQ.Core.HQ.itemList.GetKeyList
                If item.ToLower.Contains(strSearch) Then
                    ' Add results in by ID, type
                    results.Add(CStr(EveHQ.Core.HQ.itemList(item)), item)
                End If
            Next

            ' Reset checkbox colours
            chkFilter1.ForeColor = Color.Red
            chkFilter2.ForeColor = Color.Red
            chkFilter4.ForeColor = Color.Red
            chkFilter8.ForeColor = Color.Red
            chkFilter16.ForeColor = Color.Red
            chkFilter32.ForeColor = Color.Red

            lvwItems.BeginUpdate()
            lvwItems.Items.Clear()
            For Each item As String In results.Keys
                If ModuleLists.moduleList.Contains(item) = True And Implants.implantList.ContainsKey(item) = False Then
                    Dim shipMod As ShipModule = CType(ModuleLists.moduleList(item), ShipModule)
                    If (shipMod.MetaType And HQF.Settings.HQFSettings.ModuleFilter) = shipMod.MetaType Then
                        Dim newModule As New ListViewItem
                        newModule.Name = shipMod.ID
                        newModule.Text = shipMod.Name
                        newModule.ToolTipText = shipMod.Name
                        newModule.SubItems.Add(shipMod.MetaLevel.ToString)
                        newModule.SubItems.Add(shipMod.CPU.ToString)
                        newModule.SubItems.Add(shipMod.PG.ToString)
                        Select Case shipMod.Slot
                            Case 8 ' High
                                newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                                newModule.ImageKey = "hiSlot"
                            Case 4 ' Mid
                                newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                                newModule.ImageKey = "midSlot"
                            Case 2 ' Low
                                newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
                                newModule.ImageKey = "lowSlot"
                            Case 1 ' Rig
                                newModule.BackColor = Color.FromArgb(CInt(HQF.Settings.HQFSettings.RigSlotColour))
                                newModule.ImageKey = "rigSlot"
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
            tvwItems.Tag = "Search"
        End If
    End Sub
    Private Sub txtSearchModules_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchModules.GotFocus
        Call ShowSearchedModules()
    End Sub
    Private Sub txtSearchModules_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchModules.TextChanged
        Call ShowSearchedModules()
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
                    Call currentShipSlot.AddModule(shipMod)
                End If
            End If
        End If
    End Sub
#End Region

    Private Sub tsbOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbOptions.Click
        ' Test option for counting
        Dim mySettings As New frmHQFSettings
        mySettings.ShowDialog()
        mySettings = Nothing
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
        ' Check MarketGroups
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

    Private Sub btnScreenshot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScreenshot.Click
        ' Determine co-ords of current main panel
        Dim tp As TabPage = tabHQF.TabPages(CInt(tabHQF.Tag))
        Dim xy As Point = tp.PointToScreen(New Point(0, 0))
        Dim sx As Integer = xy.X
        Dim sy As Integer = xy.Y
        'tp.Controls("PanelShipInfo").Visible = False
        Dim fittingImage As Bitmap = ScreenGrab.GrabScreen(New Rectangle(sx, sy, tp.Width, tp.Height))
        Clipboard.SetDataObject(fittingImage)
        fittingImage.Save(EveHQ.Core.HQ.reportFolder & "\HQF" & Now.Ticks & ".png", System.Drawing.Imaging.ImageFormat.Png)
        'tp.Controls("PanelShipInfo").Visible = True
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        'Dim strSQL As String = ""
        'strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID"
        'strSQL &= " FROM invCategories INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invCategories.categoryID = invGroups.categoryID"
        'strSQL &= " WHERE ((invGroups.categoryID) In (16) AND (invTypes.published=true))"
        'strSQL &= " ORDER BY invTypes.typeName;"
        'moduleData = EveHQ.Core.DataFunctions.GetData(strSQL)

        'Dim sw As New IO.StreamWriter(EveHQ.Core.HQ.reportFolder & "\Skills.csv")
        'For Each row As DataRow In moduleData.Tables(0).Rows
        '    sw.WriteLine(row.Item("typeName"))
        'Next
        'sw.Flush()
        'sw.Close()
        'MessageBox.Show("Data Dump Finished!")

        Dim sw As New IO.StreamWriter(EveHQ.Core.HQ.reportFolder & "\Groups.csv")
        For Each rootNode As TreeNode In tvwItems.Nodes
            SearchChildNodes1(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()

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

    Private Sub mnuShowModuleInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuShowModuleInfo.Click
        Dim moduleID As String = lvwItems.SelectedItems(0).Name
        Dim cModule As ShipModule = CType(ModuleLists.moduleList.Item(moduleID), ShipModule)
        Dim showInfo As New frmShowInfo
        showInfo.ShowItemDetails(cModule)
        showInfo = Nothing
    End Sub

#Region "TabHQF Selection and Context Menu Routines"
    Private Sub tabHQF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabHQF.SelectedIndexChanged
        If tabHQF.SelectedTab IsNot Nothing Then
            If FittingTabList.Contains(tabHQF.SelectedTab.Text) Then
                ' Get the controls on the existing tab
                Dim thisShipSlotControl As ShipSlotControl = CType(tabHQF.SelectedTab.Controls("panelShipSlot").Controls("shipSlot"), ShipSlotControl)
                Dim thisShipInfoControl As ShipInfoControl = CType(tabHQF.SelectedTab.Controls("panelShipInfo").Controls("shipInfo"), ShipInfoControl)
                currentShipSlot = thisShipSlotControl
                currentShipInfo = thisShipInfoControl
                currentShipSlot.ShipFit = tabHQF.SelectedTab.Text
                currentShipSlot.ShipInfo = currentShipInfo
            End If
            tabHQF.Tag = tabHQF.SelectedIndex
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
        FittingTabList.Remove(tp.Text)
        ShipLists.fittedShipList.Remove(tp.Text)
        tabHQF.TabPages.Remove(tp)
    End Sub
#End Region

#Region "Clipboard Routines (incl Timer)"
    Private Sub tmrClipboard_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClipboard.Tick
        ' Checks the clipboard for any compatible fixes!
        If Clipboard.GetDataObject IsNot Nothing Then
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
        ShipLists.fittedShipList.Add(shipFit, curShip)

        Dim tp As New TabPage(shipFit)
        tp.Tag = shipFit
        tp.Name = shipFit
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
        pSI.Size = New System.Drawing.Size(250, 600)
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

        FittingTabList.Add(shipFit)
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
                FittingTabList.Remove(oldKeyName)
                FittingTabList.Add(fittingKeyName)
                tp.Name = fittingKeyName
                tp.Tag = fittingKeyName
                tp.Text = fittingKeyName
            End If
            Call Me.UpdateFittingsTree()
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
            Call Me.UpdateFittingsTree()
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
                FittingTabList.Remove(tp.Text)
                tabHQF.TabPages.Remove(tp)
                ShipLists.fittedShipList.Remove(tp.Text)
            End If
            ' Update the list
            Me.UpdateFittingsTree()
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
            Call Me.UpdateFittingsTree()
            tabHQF.SelectedTab = tabHQF.TabPages(fittingKeyName)
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
        ' Get the ship details
        If fittingNode.Parent IsNot Nothing Then
            Dim shipName As String = fittingNode.Parent.Text
            Dim shipFit As String = fittingNode.Parent.Text & ", " & fittingNode.Text
            ' Create the tab and display
            If FittingTabList.Contains(shipFit) = False Then
                Call Me.CreateFittingTabPage(shipFit)
            End If
            tabHQF.SelectedTab = tabHQF.TabPages(shipFit)
            currentShipSlot.UpdateEverything()
        End If
    End Sub
#End Region

    Private Sub btnShipPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShipPanel.Click
        If btnShipPanel.Checked = True Then
            ' If the panel is open
            'btnShipPanel.Image = My.Resources.panel_close
            SplitContainer1.Visible = True
        Else
            ' If the panel is closed
            'btnShipPanel.Image = My.Resources.panel_open
            SplitContainer1.Visible = False
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
        ' Delete the cache folder if it's already there
        If My.Computer.FileSystem.DirectoryExists(Settings.HQFCacheFolder) = True Then
            My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        My.Computer.FileSystem.CreateDirectory(Settings.HQFCacheFolder)
        ' Save ships
        Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\ships.txt", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, ShipLists.shipList)
        s.Close()
        ' Save modules
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\modules.txt", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, ModuleLists.moduleList)
        s.Close()
        ' Save implants
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\implants.txt", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Implants.implantList)
        s.Close()
        ' Save skills
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\skills.txt", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, SkillLists.SkillList)
        s.Close()
        ' Save attributes
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\attributes.txt", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Attributes.AttributeList)
        s.Close()
        ' Write Ship Tree 
        Call Me.WriteShipGroups()
        Call Me.WriteItemGroups()
    End Sub
    Private Sub WriteShipGroups()
        Dim sw As New IO.StreamWriter(HQF.Settings.HQFCacheFolder & "\ShipGroups.txt")
        For Each rootNode As TreeNode In tvwShips.Nodes
            WriteShipNodes(rootNode, sw)
        Next
        sw.Flush()
        sw.Close()
    End Sub
    Private Sub WriteItemGroups()
        Dim sw As New IO.StreamWriter(HQF.Settings.HQFCacheFolder & "\ItemGroups.txt")
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

    Private Sub btnPilotManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPilotManager.Click
        Dim myPilotManager As New frmPilotManager
        myPilotManager.ShowDialog()
        myPilotManager = Nothing
    End Sub
End Class