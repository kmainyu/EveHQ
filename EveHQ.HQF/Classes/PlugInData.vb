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
    Shared LastCacheRefresh As String = "1.13.2.895"

#Region "Plug-in Interface Properties and Functions"

    Public Function GetPlugInData(ByVal Data As Object, ByVal DataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Return Nothing
    End Function

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Try
            ' Check for existance of HQF folder & create if not existing
            If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
                Settings.HQFFolder = Path.Combine(EveHQ.Core.HQ.appDataFolder, "HQF")
            Else
                Settings.HQFFolder = Path.Combine(Application.StartupPath, "HQF")
            End If
            If My.Computer.FileSystem.DirectoryExists(Settings.HQFFolder) = False Then
                My.Computer.FileSystem.CreateDirectory(Settings.HQFFolder)
            End If

            ' Check for cache folder
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
                        PlugInData.UseSerializableData = False
                    Else
                        PlugInData.UseSerializableData = True
                    End If
                Else
                    ' Delete the existing cache folder and force a rebuild
                    My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    PlugInData.UseSerializableData = False
                End If
            Else
                PlugInData.UseSerializableData = False
            End If

            Engine.BuildPirateImplants()
            Engine.BuildBoosterPenaltyList()
            Engine.BuildEffectsMap()
            Engine.BuildShipEffectsMap()
            Engine.BuildShipBonusesMap()
            Engine.BuildSubSystemBonusMap()
            ' Check for the existence of the binary data
            If PlugInData.UseSerializableData = True Then
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "attributes.bin")) = True Then
                    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "attributes.bin"), FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    Attributes.AttributeList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "ships.bin")) = True Then
                    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "ships.bin"), FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    ShipLists.shipList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                    For Each cShip As Ship In ShipLists.shipList.Values
                        ShipLists.shipListKeyID.Add(cShip.ID, cShip.Name)
                        ShipLists.shipListKeyID.Add(cShip.Name, cShip.ID)
                    Next
                End If
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "modules.bin")) = True Then
                    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "modules.bin"), FileMode.Open)
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
                End If
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "implants.bin")) = True Then
                    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "implants.bin"), FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    Implants.implantList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "boosters.bin")) = True Then
                    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "boosters.bin"), FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    Boosters.BoosterList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "skills.bin")) = True Then
                    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "skills.bin"), FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    SkillLists.SkillList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFCacheFolder, "NPCs.bin")) = True Then
                    Dim s As New FileStream(Path.Combine(HQF.Settings.HQFCacheFolder, "NPCs.bin"), FileMode.Open)
                    Dim f As BinaryFormatter = New BinaryFormatter
                    NPCs.NPCList = CType(f.Deserialize(s), SortedList)
                    s.Close()
                End If
                Call Me.BuildAttributeQuickList()
                Engine.BuildImplantEffectsMap()
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
                                                                Call Me.BuildModuleEffects()
                                                                Call Me.BuildImplantEffects()
                                                                Call Me.BuildShipEffects()
                                                                Call Me.BuildSubsystemEffects()
                                                                ' Save the HQF data
                                                                Call Me.SaveHQFCacheData()
                                                                Call Me.CleanUpData()
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
            Return True
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
            strSQL &= " WHERE (invCategories.categoryID=6 AND invTypes.published=true AND invTypes.typeID<>30842) ORDER BY typeName;"
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
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
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
                                newShip.Attributes.Add("10063", 1)
                                newShip.Attributes.Add("10064", 2)
                                newShip.Attributes.Add("10065", 0)
                                newShip.Attributes.Add("10066", 0)
                                newShip.Attributes.Add("10067", 0)
                                newShip.Attributes.Add("10068", 0)
                                newShip.Attributes.Add("10069", 0)
                                ' Check for slot attributes (missing for T3)
                                If newShip.Attributes.ContainsKey("12") = False Then
                                    newShip.Attributes.Add("12", 0)
                                    newShip.Attributes.Add("13", 0)
                                    newShip.Attributes.Add("14", 0)
                                End If
                                ' Check for cloak reactivation attribute
                                If newShip.Attributes.ContainsKey("1034") = False Then
                                    newShip.Attributes.Add("1034", 30)
                                End If
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
                        newShip.Attributes.Add(shipRow.Item("attributeID").ToString, attValue)

                        ' Map only the skill attributes
                        Select Case CInt(shipRow.Item("attributeID"))
                            Case 182
                                Dim pSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = pSkill.ID
                                nSkill.Name = pSkill.Name
                                pSkillName = pSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 183
                                Dim sSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                                Dim nSkill As New ItemSkills
                                nSkill.ID = sSkill.ID
                                nSkill.Name = sSkill.Name
                                sSkillName = sSkill.Name
                                newShip.RequiredSkills.Add(nSkill.Name, nSkill)
                            Case 184
                                Dim tSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
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
                    newShip.Attributes.Add("10063", 1)
                    newShip.Attributes.Add("10064", 2)
                    newShip.Attributes.Add("10065", 0)
                    newShip.Attributes.Add("10066", 0)
                    newShip.Attributes.Add("10067", 0)
                    newShip.Attributes.Add("10068", 0)
                    newShip.Attributes.Add("10069", 0)
                    ' Map the remaining attributes for the last ship type
                    Ship.MapShipAttributes(newShip)
                    ' Check for slot attributes (missing for T3)
                    If newShip.Attributes.ContainsKey("12") = False Then
                        newShip.Attributes.Add("12", 0)
                        newShip.Attributes.Add("13", 0)
                        newShip.Attributes.Add("14", 0)
                    End If
                    ' Check for cloak reactivation attribute
                    If newShip.Attributes.ContainsKey("1034") = False Then
                        newShip.Attributes.Add("1034", 30)
                    End If
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
            ShipLists.shipListKeyName.Add(baseShip.ID, baseShip.Name)
        Next
    End Sub

    ' Module Loading Routines
    Private Function LoadModuleData() As Boolean
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.raceID, invTypes.marketGroupID, eveGraphics.icon"
            strSQL &= " FROM eveGraphics INNER JOIN (invCategories INNER JOIN (invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) ON invCategories.categoryID = invGroups.categoryID) ON (eveGraphics.graphicID = invTypes.graphicID)"
            strSQL &= " WHERE ((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)"
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
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeEffects.effectID"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeEffects ON invTypes.typeID=dgmTypeEffects.typeID"
            strSQL &= " WHERE ((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)"
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
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName"
            strSQL &= " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID"
            strSQL &= " WHERE ((invCategories.categoryID In (7,8,18,20,32)) or (invTypes.marketGroupID=379) or (invTypes.groupID=920)) AND (invTypes.published=1)"
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
            Implants.implantList.Clear()
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
                newModule.Attributes.Add("10004", CDbl(row.Item("capacity")))
                newModule.Attributes.Add("10002", CDbl(row.Item("mass")))
                If IsDBNull(row.Item("raceID")) = False Then
                    newModule.RaceID = CInt(row.Item("raceID"))
                Else
                    newModule.RaceID = 0
                End If
                newModule.MarketPrice = EveHQ.Core.DataFunctions.GetPrice(newModule.ID)
                newModule.Icon = row.Item("icon").ToString
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
                Select Case CInt(modRow.Item("effectID"))
                    Case 11 ' Low slot
                        effMod.SlotType = 2
                    Case 12 ' High slot
                        effMod.SlotType = 8
                    Case 13 ' Mid slot
                        effMod.SlotType = 4
                    Case 2663 ' Rig slot
                        effMod.SlotType = 1
                    Case 3772 ' Sub slot
                        effMod.SlotType = 16
                    Case 40
                        If effMod.DatabaseGroup <> "481" Then
                            effMod.IsLauncher = True
                        End If
                    Case 10, 34, 42
                        effMod.IsTurret = True
                End Select
                ' Add custom attributes
                If effMod.IsDrone = True Or effMod.IsLauncher = True Or effMod.IsTurret = True Or effMod.DatabaseGroup = "72" Or effMod.DatabaseGroup = "862" Then
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
                        Dim pSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                        Dim nSkill As New ItemSkills
                        nSkill.ID = pSkill.ID
                        nSkill.Name = pSkill.Name
                        pSkillName = pSkill.Name
                        attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                    Case 183
                        Dim sSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
                        Dim nSkill As New ItemSkills
                        nSkill.ID = sSkill.ID
                        nSkill.Name = sSkill.Name
                        sSkillName = sSkill.Name
                        attMod.RequiredSkills.Add(nSkill.Name, nSkill)
                    Case 184
                        Dim tSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(CStr(attValue))
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
            ' Build the metaType data
            For Each cMod As ShipModule In ModuleLists.moduleList.Values
                If ModuleLists.moduleMetaGroups.Contains(cMod.ID) = True Then
                    If CStr(ModuleLists.moduleMetaGroups(cMod.ID)) = "0" Then
                        Select Case CInt(cMod.Attributes("422"))
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
                        cMod.MetaType = CInt(2 ^ (CInt(ModuleLists.moduleMetaGroups(cMod.ID)) - 1))
                    End If
                Else
                    cMod.MetaType = 1
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
                newEffect.AffectingType = CInt(EffectData(1))
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CInt(EffectData(4))
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CInt(EffectData(6))
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CInt(EffectData(8))
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
                            If cModule.Attributes.Contains(newEffect.AffectedID(0).ToString) Then
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
                                Case EffectType.Attribute
                                    If cShip.Attributes.Contains(newEffect.AffectedID(0).ToString) Then
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
                    newEffect.AffectedType = CInt(EffectData(4))
                    If EffectData(5).Contains(";") = True Then
                        IDs = EffectData(5).Split(";".ToCharArray)
                        For Each ID As String In IDs
                            newEffect.AffectedID.Add(ID)
                        Next
                    Else
                        newEffect.AffectedID.Add(EffectData(5))
                    End If
                    newEffect.CalcType = CInt(EffectData(6))
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
                                If cModule.Attributes.Contains(newEffect.AffectedID(0).ToString) Then
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
                newEffect.AffectingType = CInt(EffectData(1))
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CInt(EffectData(4))
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CInt(EffectData(6))
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CInt(EffectData(8))
                newEffect.Value = Double.Parse(EffectData(9), Globalization.NumberStyles.Number, culture)
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
                            If cModule.Attributes.Contains(newEffect.AffectedID(0).ToString) Then
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
                newEffect.AffectingType = CInt(EffectData(1))
                newEffect.AffectingID = CInt(EffectData(2))
                newEffect.AffectedAtt = CInt(EffectData(3))
                newEffect.AffectedType = CInt(EffectData(4))
                If EffectData(5).Contains(";") = True Then
                    IDs = EffectData(5).Split(";".ToCharArray)
                    For Each ID As String In IDs
                        newEffect.AffectedID.Add(ID)
                    Next
                Else
                    newEffect.AffectedID.Add(EffectData(5))
                End If
                newEffect.StackNerf = CInt(EffectData(6))
                newEffect.IsPerLevel = CBool(EffectData(7))
                newEffect.CalcType = CInt(EffectData(8))
                newEffect.Value = Double.Parse(EffectData(9), Globalization.NumberStyles.Number, culture)
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
                            If cModule.Attributes.Contains(newEffect.AffectedID(0).ToString) Then
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
                    MessageBox.Show("Warning: NPC Data returned no rows but HQF can continue to load. Please remember to set damage profiles manually.", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return True
                End If
            Else
                MessageBox.Show("Warning: NPC Data returned a null dataset but HQF can continue to load. Please remember to set damage profiles manually.", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return True
            End If
        Catch e As Exception
            MessageBox.Show("Error loading NPC Data: " & e.Message, "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

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

End Class
