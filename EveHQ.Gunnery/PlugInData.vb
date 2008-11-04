Imports System.Windows.Forms

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object

    Public Shared ProjectileGuns As New SortedList
    Public Shared EnergyGuns As New SortedList
    Public Shared HybridGuns As New SortedList
    Public Shared ProjectileAmmo As New SortedList
    Public Shared EnergyAmmo As New SortedList
    Public Shared HybridAmmo As New SortedList
    Public Shared DamageMods As New SortedList
    Public Shared TrackingMods As New SortedList

    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return mSetPlugInData
        End Get
        Set(ByVal value As Object)
            mSetPlugInData = value
        End Set
    End Property

    Public Function IGBService(ByVal IGBContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function

    Public Function EveHQStartUp() As Boolean Implements EveHQ.Core.IEveHQPlugIn.EveHQStartUp
        If Me.LoadWeapons = True Then
            If Me.LoadAmmo = True Then
                If Me.LoadDM = True Then
                    If Me.LoadTC = True Then
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
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements EveHQ.Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Gunnery Tool"
        EveHQPlugIn.Description = "Displays statistics for guns and ammo"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "Gunnery Tool"
        EveHQPlugIn.RunAtStartup = True
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.plugin_icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function

    Public Function RunEveHQPlugIn() As Windows.Forms.Form Implements EveHQ.Core.IEveHQPlugIn.RunEveHQPlugIn
        Return New frmGunnery
    End Function

    Private Function LoadWeapons() As Boolean
        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invTypes.groupID, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
        strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID  OR invTypes.typeID = invMetaTypes.parentTypeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
        strSQL &= " WHERE (invTypes.groupID IN (53,55,74) AND invTypes.published=true);"
        Dim gunMetaData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        strSQL = "SELECT invTypes.typeID AS invTypes_typeID, dgmAttributeTypes.attributeID AS dgmAttributeTypes_attributeID, dgmAttributeTypes.attributeName, dgmTypeAttributes.typeID AS dgmTypeAttributes_typeID, dgmTypeAttributes.attributeID AS dgmTypeAttributes_attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveUnits.unitID AS eveUnits_unitID, eveUnits.unitName, eveUnits.displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, invTypes.groupID, invTypes.typeName, invTypes.volume, invTypes.capacity, invTypes.portionSize, invTypes.published"
        strSQL &= " FROM invTypes INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID"
        strSQL &= " WHERE (((invTypes.groupID) In (53,55,74)) AND (invTypes.published=true))"
        strSQL &= " ORDER BY invTypes.typeName;"
        Dim gunData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        Try
            If gunMetaData IsNot Nothing Then
                If gunMetaData.Tables(0).Rows.Count > 0 Then
                    For gun As Integer = 0 To gunMetaData.Tables(0).Rows.Count - 1
                        Dim tn As String = gunMetaData.Tables(0).Rows(gun).Item("typeName").ToString
                        If (HybridGuns.Contains(tn) = False And EnergyGuns.Contains(tn) = False And ProjectileGuns.Contains(tn) = False) Then
                            Dim newGun As New GunClass
                            newGun.typeID = gunMetaData.Tables(0).Rows(gun).Item("invTypes_typeID").ToString
                            newGun.typeName = gunMetaData.Tables(0).Rows(gun).Item("typeName").ToString
                            newGun.groupID = gunMetaData.Tables(0).Rows(gun).Item("groupID").ToString
                            newGun.metaType = gunMetaData.Tables(0).Rows(gun).Item("metaGroupName").ToString

                            Dim gunAtts() As DataRow = gunData.Tables(0).Select("invTypes_typeID=" & newGun.typeID)
                            For gunAtt As Integer = 0 To gunAtts.GetUpperBound(0)
                                Dim attValue As Double = 0
                                If IsDBNull(gunAtts(gunAtt).Item("valueFloat")) = True Then
                                    attValue = CDbl(gunAtts(gunAtt).Item("valueInt"))
                                Else
                                    attValue = CDbl(gunAtts(gunAtt).Item("valueFloat"))
                                End If
                                Select Case gunAtts(gunAtt).Item("dgmAttributeTypes_attributeID").ToString
                                    Case "30"
                                        newGun.PGUsage = attValue
                                    Case "50"
                                        newGun.CPUUsage = attValue
                                    Case "6"
                                        newGun.CapUsage = attValue
                                    Case "51"
                                        newGun.RateOfFire = (attValue / 1000)
                                    Case "54"
                                        newGun.OptimalRange = attValue
                                    Case "64"
                                        newGun.DamageModifier = attValue
                                    Case "128"
                                        newGun.Size = CInt(attValue)
                                    Case "158"
                                        newGun.AccuracyFalloff = attValue
                                    Case "160"
                                        newGun.TrackingSpeed = attValue
                                    Case "182"
                                        newGun.PrimarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "183"
                                        newGun.SecondarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "184"
                                        newGun.TertiarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "633"
                                        newGun.metaLevel = CInt(attValue)
                                End Select
                            Next
                            Select Case newGun.groupID
                                Case "53" ' Energy
                                    EnergyGuns.Add(newGun.typeName, newGun)
                                Case "74" ' Hybrid
                                    HybridGuns.Add(newGun.typeName, newGun)
                                Case "55" ' Projectile
                                    ProjectileGuns.Add(newGun.typeName, newGun)
                            End Select
                        End If
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show("There was an error generating the weapon data. The error was: " & ControlChars.CrLf & ex.Message, "Weapons Tool Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function
    Private Function LoadAmmo() As Boolean
        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invTypes.groupID, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
        strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID OR invTypes.typeID = invMetaTypes.parentTypeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
        strSQL &= " WHERE (invTypes.groupID IN (83,85,86,372,373,374,375,376,377) AND (invTypes.published)=true);"
        Dim ammoMetaData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        strSQL = "SELECT invTypes.typeID AS invTypes_typeID, dgmAttributeTypes.attributeID AS dgmAttributeTypes_attributeID, dgmAttributeTypes.attributeName, dgmTypeAttributes.typeID AS dgmTypeAttributes_typeID, dgmTypeAttributes.attributeID AS dgmTypeAttributes_attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveUnits.unitID AS eveUnits_unitID, eveUnits.unitName, eveUnits.displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, invTypes.groupID, invTypes.typeName, invTypes.volume, invTypes.capacity, invTypes.portionSize, invTypes.published"
        strSQL &= " FROM invTypes INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID"
        strSQL &= " WHERE (invTypes.groupID IN (83,85,86,372,373,374,375,376,377) AND (invTypes.published)=true)"
        strSQL &= " ORDER BY invTypes.typeName;"
        Dim ammoData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        Try
            If ammoMetaData IsNot Nothing Then
                If ammoMetaData.Tables(0).Rows.Count > 0 Then
                    For ammo As Integer = 0 To ammoMetaData.Tables(0).Rows.Count - 1
                        Dim tn As String = ammoMetaData.Tables(0).Rows(ammo).Item("typeName").ToString
                        If (HybridAmmo.Contains(tn) = False And EnergyAmmo.Contains(tn) = False And ProjectileAmmo.Contains(tn) = False) Then
                            Dim newAmmo As New AmmoClass
                            newAmmo.typeID = ammoMetaData.Tables(0).Rows(ammo).Item("invTypes_typeID").ToString
                            newAmmo.typeName = tn
                            newAmmo.groupID = ammoMetaData.Tables(0).Rows(ammo).Item("groupID").ToString
                            newAmmo.metaType = ammoMetaData.Tables(0).Rows(ammo).Item("metaGroupName").ToString

                            Dim ammoAtts() As DataRow = ammoData.Tables(0).Select("invTypes_typeID=" & newAmmo.typeID)
                            For ammoAtt As Integer = 0 To ammoAtts.GetUpperBound(0)
                                Dim attValue As Double = 0
                                If IsDBNull(ammoAtts(ammoAtt).Item("valueFloat")) = True Then
                                    attValue = CDbl(ammoAtts(ammoAtt).Item("valueInt"))
                                Else
                                    attValue = CDbl(ammoAtts(ammoAtt).Item("valueFloat"))
                                End If
                                Select Case ammoAtts(ammoAtt).Item("dgmAttributeTypes_attributeID").ToString
                                    Case "120"
                                        newAmmo.RangeBonus = attValue
                                    Case "128"
                                        newAmmo.Size = CInt(attValue)
                                    Case "317"
                                        newAmmo.CapBonus = attValue
                                    Case "422"
                                        newAmmo.metaLevel = CInt(attValue)
                                    Case "114"
                                        newAmmo.EMDamage = attValue
                                    Case "116"
                                        newAmmo.ExDamage = attValue
                                    Case "117"
                                        newAmmo.KiDamage = attValue
                                    Case "118"
                                        newAmmo.ThDamage = attValue
                                    Case "182"
                                        newAmmo.PrimarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "183"
                                        newAmmo.SecondarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "184"
                                        newAmmo.TertiarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "204"
                                        newAmmo.ROFBonus = attValue
                                    Case "244"
                                        newAmmo.TrackingBonus = attValue
                                    Case "517"
                                        newAmmo.FalloffBonus = attValue
                                End Select
                            Next
                            Select Case newAmmo.groupID
                                Case "86", "374", "375" ' Energy
                                    EnergyAmmo.Add(newAmmo.typeName, newAmmo)
                                Case "85", "373", "377" ' Hybrid
                                    HybridAmmo.Add(newAmmo.typeName, newAmmo)
                                Case "83", "372", "386" ' Projectile
                                    ProjectileAmmo.Add(newAmmo.typeName, newAmmo)
                            End Select
                        End If
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show("There was an error generating the weapon data. The error was: " & ControlChars.CrLf & ex.Message, "Weapons Tool Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function
    Private Function LoadDM() As Boolean
        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invTypes.groupID, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
        strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID OR invTypes.typeID = invMetaTypes.parentTypeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
        strSQL &= " WHERE (invTypes.groupID IN (59,205,302) AND (invTypes.published)=true);"
        Dim ammoMetaData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        strSQL = "SELECT invTypes.typeID AS invTypes_typeID, dgmAttributeTypes.attributeID AS dgmAttributeTypes_attributeID, dgmAttributeTypes.attributeName, dgmTypeAttributes.typeID AS dgmTypeAttributes_typeID, dgmTypeAttributes.attributeID AS dgmTypeAttributes_attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveUnits.unitID AS eveUnits_unitID, eveUnits.unitName, eveUnits.displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, invTypes.groupID, invTypes.typeName, invTypes.volume, invTypes.capacity, invTypes.portionSize, invTypes.published"
        strSQL &= " FROM invTypes INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID"
        strSQL &= " WHERE (invTypes.groupID IN (59,205,302) AND (invTypes.published)=true)"
        strSQL &= " ORDER BY invTypes.typeName;"
        Dim ammoData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        Try
            If ammoMetaData IsNot Nothing Then
                If ammoMetaData.Tables(0).Rows.Count > 0 Then
                    For ammo As Integer = 0 To ammoMetaData.Tables(0).Rows.Count - 1
                        Dim tn As String = ammoMetaData.Tables(0).Rows(ammo).Item("typeName").ToString
                        If (DamageMods.Contains(tn) = False) Then
                            Dim newDM As New DamageModClass
                            newDM.typeID = ammoMetaData.Tables(0).Rows(ammo).Item("invTypes_typeID").ToString
                            newDM.typeName = tn
                            newDM.groupID = ammoMetaData.Tables(0).Rows(ammo).Item("groupID").ToString
                            newDM.metaType = ammoMetaData.Tables(0).Rows(ammo).Item("metaGroupName").ToString
                            Dim ammoAtts() As DataRow = ammoData.Tables(0).Select("invTypes_typeID=" & newDM.typeID)
                            For ammoAtt As Integer = 0 To ammoAtts.GetUpperBound(0)
                                Dim attValue As Double = 0
                                If IsDBNull(ammoAtts(ammoAtt).Item("valueFloat")) = True Then
                                    attValue = CDbl(ammoAtts(ammoAtt).Item("valueInt"))
                                Else
                                    attValue = CDbl(ammoAtts(ammoAtt).Item("valueFloat"))
                                End If
                                Select Case ammoAtts(ammoAtt).Item("dgmAttributeTypes_attributeID").ToString
                                    Case "64"
                                        newDM.DamageBonus = attValue
                                    Case "204"
                                        newDM.ROFBonus = attValue
                                    Case "182"
                                        newDM.PrimarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "183"
                                        newDM.SecondarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "184"
                                        newDM.TertiarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                End Select
                            Next
                            DamageMods.Add(newDM.typeName, newDM)
                        End If
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show("There was an error generating the weapon data. The error was: " & ControlChars.CrLf & ex.Message, "Weapons Tool Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Function LoadTC() As Boolean
        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invTypes.groupID, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
        strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID OR invTypes.typeID = invMetaTypes.parentTypeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
        strSQL &= " WHERE (invTypes.groupID IN (211,213) AND (invTypes.published)=true);"
        Dim ammoMetaData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        strSQL = "SELECT invTypes.typeID AS invTypes_typeID, dgmAttributeTypes.attributeID AS dgmAttributeTypes_attributeID, dgmAttributeTypes.attributeName, dgmTypeAttributes.typeID AS dgmTypeAttributes_typeID, dgmTypeAttributes.attributeID AS dgmTypeAttributes_attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, eveUnits.unitID AS eveUnits_unitID, eveUnits.unitName, eveUnits.displayName, dgmAttributeTypes.unitID AS dgmAttributeTypes_unitID, invTypes.groupID, invTypes.typeName, invTypes.volume, invTypes.capacity, invTypes.portionSize, invTypes.published"
        strSQL &= " FROM invTypes INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID"
        strSQL &= " WHERE (invTypes.groupID IN (211,213) AND (invTypes.published)=true)"
        strSQL &= " ORDER BY invTypes.typeName;"
        Dim ammoData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)

        Try
            If ammoMetaData IsNot Nothing Then
                If ammoMetaData.Tables(0).Rows.Count > 0 Then
                    For ammo As Integer = 0 To ammoMetaData.Tables(0).Rows.Count - 1
                        Dim tn As String = ammoMetaData.Tables(0).Rows(ammo).Item("typeName").ToString
                        If (TrackingMods.Contains(tn) = False) Then
                            Dim newTM As New TrackingModClass
                            newTM.typeID = ammoMetaData.Tables(0).Rows(ammo).Item("invTypes_typeID").ToString
                            newTM.typeName = tn
                            newTM.groupID = ammoMetaData.Tables(0).Rows(ammo).Item("groupID").ToString
                            newTM.metaType = ammoMetaData.Tables(0).Rows(ammo).Item("metaGroupName").ToString
                            Dim ammoAtts() As DataRow = ammoData.Tables(0).Select("invTypes_typeID=" & newTM.typeID)
                            For ammoAtt As Integer = 0 To ammoAtts.GetUpperBound(0)
                                Dim attValue As Double = 0
                                If IsDBNull(ammoAtts(ammoAtt).Item("valueFloat")) = True Then
                                    attValue = CDbl(ammoAtts(ammoAtt).Item("valueInt"))
                                Else
                                    attValue = CDbl(ammoAtts(ammoAtt).Item("valueFloat"))
                                End If
                                Select Case ammoAtts(ammoAtt).Item("dgmAttributeTypes_attributeID").ToString
                                    Case "351"
                                        newTM.RangeBonus = attValue
                                    Case "767"
                                        newTM.TrackingBonus = attValue
                                    Case "182"
                                        newTM.PrimarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "183"
                                        newTM.SecondarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                    Case "184"
                                        newTM.TertiarySkill = EveHQ.Core.SkillFunctions.SkillIDToName(CStr(attValue))
                                End Select
                            Next
                            TrackingMods.Add(newTM.typeName, newTM)
                        End If
                    Next
                Else
                    Return False
                    Exit Function
                End If
            Else
                Return False
                Exit Function
            End If

            Return True
        Catch ex As Exception
            MessageBox.Show("There was an error generating the weapon data. The error was: " & ControlChars.CrLf & ex.Message, "Weapons Tool Critical Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
End Class
