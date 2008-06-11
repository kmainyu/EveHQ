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

Public Class frmGunnery
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object
    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return mSetPlugInData
        End Get
        Set(ByVal value As Object)
            mSetPlugInData = value
        End Set
    End Property
    Shared ProjectileGuns As New SortedList
    Shared EnergyGuns As New SortedList
    Shared HybridGuns As New SortedList
    Shared ProjectileAmmo As New SortedList
    Shared EnergyAmmo As New SortedList
    Shared HybridAmmo As New SortedList
    Shared DamageMods As New SortedList
    Shared TrackingMods As New SortedList
    Shared myGunnerySkills As New GunnerySkills
    Shared currentDM As Gunnery.DamageModClass
    Shared currentTC As Gunnery.TrackingModClass
    Shared currentTE As Gunnery.TrackingModClass
    Const StackNerf As Double = 0.755369541025
    Shared DamageBonus As Double = 1
    Shared ROFBonus As Double = 1
    Shared TrackingBonus As Double = 1
    Shared RangeBonus As Double = 1

    Public Function IGBService(ByVal IGBContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function

    Private Sub frmWeapons_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load Skills
        Call Me.LoadSkills()
        Call Me.LoadMods()
        radEnergy.Checked = True
    End Sub

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
        Return Me
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
    Private Sub LoadSkills()
        myGunnerySkills = New GunnerySkills
        For Each mySkill As EveHQ.Core.Skills In EveHQ.Core.HQ.myPilot.PilotSkills
            Select Case mySkill.Name
                Case "Advanced Weapon Upgrades"
                    myGunnerySkills.AWU = mySkill.Level
                Case "Capital Energy Turret"
                    myGunnerySkills.CapEnergy = mySkill.Level
                Case "Capital Hybrid Turret"
                    myGunnerySkills.CapHybrid = mySkill.Level
                Case "Capital Projectile Turret"
                    myGunnerySkills.CapProjectile = mySkill.Level
                Case "Controlled Bursts"
                    myGunnerySkills.ControlledBursts = mySkill.Level
                Case "Gunnery"
                    myGunnerySkills.Gunnery = mySkill.Level
                Case "Large Artillery Specialization"
                    myGunnerySkills.LargeArtSpec = mySkill.Level
                Case "Large Autocannon Specialization"
                    myGunnerySkills.LargeACSpec = mySkill.Level
                Case "Large Beam Laser Specialization"
                    myGunnerySkills.LargeBLSpec = mySkill.Level
                Case "Large Blaster Specialization"
                    myGunnerySkills.LargeBlasterSpec = mySkill.Level
                Case "Large Pulse Laser Specialization"
                    myGunnerySkills.LargePLSpec = mySkill.Level
                Case "Large Railgun Specialization"
                    myGunnerySkills.LargeRailSpec = mySkill.Level
                Case "Large Energy Turret"
                    myGunnerySkills.LargeEnergy = mySkill.Level
                Case "Large Hybrid Turret"
                    myGunnerySkills.LargeHybrid = mySkill.Level
                Case "Large Projectile Turret"
                    myGunnerySkills.LargeProjectile = mySkill.Level
                Case "Medium Artillery Specialization"
                    myGunnerySkills.MedArtSpec = mySkill.Level
                Case "Medium Autocannon Specialization"
                    myGunnerySkills.MedACSpec = mySkill.Level
                Case "Medium Beam Laser Specialization"
                    myGunnerySkills.MedBLSpec = mySkill.Level
                Case "Medium Blaster Specialization"
                    myGunnerySkills.MedBlasterSpec = mySkill.Level
                Case "Medium Pulse Laser Specialization"
                    myGunnerySkills.MedPLSpec = mySkill.Level
                Case "Medium Railgun Specialization"
                    myGunnerySkills.MedRailSpec = mySkill.Level
                Case "Medium Energy Turret"
                    myGunnerySkills.MedEnergy = mySkill.Level
                Case "Medium Hybrid Turret"
                    myGunnerySkills.MedHybrid = mySkill.Level
                Case "Medium Projectile Turret"
                    myGunnerySkills.MedProjectile = mySkill.Level
                Case "Motion Prediction"
                    myGunnerySkills.MotionPredict = mySkill.Level
                Case "Rapid Firing"
                    myGunnerySkills.RapidFiring = mySkill.Level
                Case "Sharpshooter"
                    myGunnerySkills.Sharpshooter = mySkill.Level
                Case "Small Artillery Specialization"
                    myGunnerySkills.SmallArtSpec = mySkill.Level
                Case "Small Autocannon Specialization"
                    myGunnerySkills.SmallACSpec = mySkill.Level
                Case "Small Beam Laser Specialization"
                    myGunnerySkills.SmallBLSpec = mySkill.Level
                Case "Small Blaster Specialization"
                    myGunnerySkills.SmallBlasterSpec = mySkill.Level
                Case "Small Pulse Laser Specialization"
                    myGunnerySkills.SmallPLSpec = mySkill.Level
                Case "Small Railgun Specialization"
                    myGunnerySkills.SmallRailSpec = mySkill.Level
                Case "Small Energy Turret"
                    myGunnerySkills.SmallEnergy = mySkill.Level
                Case "Small Hybrid Turret"
                    myGunnerySkills.SmallHybrid = mySkill.Level
                Case "Small Projectile Turret"
                    myGunnerySkills.SmallProjectile = mySkill.Level
                Case "Surgical Strike"
                    myGunnerySkills.SurgicalStrike = mySkill.Level
                Case "Tactical Weapon Reconfiguration"
                    myGunnerySkills.TWR = mySkill.Level
                Case "Trajectory Analysis"
                    myGunnerySkills.TrajectoryAnal = mySkill.Level
                Case "Weapon Upgrades"
                    myGunnerySkills.WU = mySkill.Level
            End Select
        Next
    End Sub
    Private Sub LoadMods()
        ' Load Tracking Mods
        Me.cboTC.BeginUpdate()
        Me.cboTE.BeginUpdate()
        Me.cboTC.Items.Clear()
        Me.cboTE.Items.Clear()
        For Each TC As TrackingModClass In TrackingMods.Values
            If TC.groupID = "213" Then
                Me.cboTC.Items.Add(TC.typeName)
            Else
                Me.cboTE.Items.Add(TC.typeName)
            End If
        Next
        Me.cboTC.EndUpdate()
        Me.cboTE.EndUpdate()

    End Sub
    Private Sub cboWeapon_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWeapon.SelectedIndexChanged
        Call Me.UpdateAmmoList()
        cboDM.Enabled = True
        If cboDM.Text <> "" Then
            nudDM.Enabled = True
        End If
        cboTC.Enabled = True
        If cboTC.Text <> "" Then
            nudTC.Enabled = True
        End If
        cboTE.Enabled = True
        If cboTE.Text <> "" Then
            nudTE.Enabled = True
        End If
    End Sub
    Private Sub UpdateAmmoList()
        lvGuns.Items.Clear()
        lvGuns.BeginUpdate()
        Dim defGun As New GunClass
        If radHybrid.Checked = True Then
            defGun = CType(HybridGuns(cboWeapon.SelectedItem), GunClass)
        Else
            If radEnergy.Checked = True Then
                defGun = CType(EnergyGuns(cboWeapon.SelectedItem), GunClass)
            Else
                defGun = CType(ProjectileGuns(cboWeapon.SelectedItem), GunClass)
            End If
        End If
        Dim pilotGun As GunClass = ApplyGunnerySkills(defGun)
        Dim moddedGun As GunClass = ApplyGunneryMods(pilotGun)

        ' Load ammo into guns!
        Dim cAmmoType As New SortedList
        If radHybrid.Checked = True Then
            cAmmoType = CType(HybridAmmo.Clone, Collections.SortedList)
        Else
            If radEnergy.Checked = True Then
                cAmmoType = CType(EnergyAmmo.Clone, Collections.SortedList)
            Else
                cAmmoType = CType(ProjectileAmmo.Clone, Collections.SortedList)
            End If
        End If
        ' State Standard Weapon Attributes
        lblPG.Text = "PG: " & FormatNumber(moddedGun.PGUsage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblCPU.Text = "CPU: " & FormatNumber(moddedGun.CPUUsage, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblDmgMod.Text = "Damage Mod: " & FormatNumber(moddedGun.DamageModifier, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblROF.Text = "ROF: " & FormatNumber(moddedGun.RateOfFire, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        For Each ammo As AmmoClass In cAmmoType.Values
            If ammo.Size = pilotGun.Size Then
                Dim ammoGun As GunClass = LoadAmmoIntoGun(moddedGun, ammo)
                Dim ammoLI As New ListViewItem
                ammoLI.Text = ammo.typeName
                ammoLI.Name = ammo.typeName
                ammoLI.SubItems.Add(FormatNumber(ammoGun.CapUsage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.OptimalRange, 0, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.AccuracyFalloff, 0, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.TrackingSpeed, 4, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.EMDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.ExDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.KiDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.ThDamage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.Damage, 2, TriState.True, TriState.True, TriState.True))
                ammoLI.SubItems.Add(FormatNumber(ammoGun.DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                lvGuns.Items.Add(ammoLI)
            End If
        Next

        ' Update the list
        lvGuns.EndUpdate()
    End Sub
    Private Function ApplyGunnerySkills(ByVal stdGun As GunClass) As GunClass
        Dim pilotGun As GunClass = CType(stdGun.Clone, GunClass)
        pilotGun.PGUsage = pilotGun.PGUsage * (1 - (0.02 * myGunnerySkills.AWU))
        pilotGun.CPUUsage = pilotGun.CPUUsage * (1 - (0.05 * myGunnerySkills.WU))
        pilotGun.CapUsage = pilotGun.CapUsage * (1 - (0.05 * myGunnerySkills.ControlledBursts))
        pilotGun.RateOfFire = pilotGun.RateOfFire * (1 - (0.02 * myGunnerySkills.Gunnery))
        pilotGun.RateOfFire = pilotGun.RateOfFire * (1 - (0.04 * myGunnerySkills.RapidFiring))
        pilotGun.TrackingSpeed = pilotGun.TrackingSpeed * (1 + (0.05 * myGunnerySkills.MotionPredict))
        pilotGun.OptimalRange = pilotGun.OptimalRange * (1 + (0.05 * myGunnerySkills.Sharpshooter))
        pilotGun.AccuracyFalloff = pilotGun.AccuracyFalloff * (1 + (0.05 * myGunnerySkills.TrajectoryAnal))
        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.03 * myGunnerySkills.SurgicalStrike))
        Select Case pilotGun.Size
            Case 1  ' Small
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.SmallEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.SmallProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.SmallHybrid))
                End Select
            Case 2  ' Medium
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.MedEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.MedProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.MedHybrid))
                End Select
            Case 3  ' Large
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.LargeEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.LargeProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.LargeHybrid))
                End Select
            Case 4  ' XLarge
                Select Case pilotGun.groupID
                    Case "53"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.CapEnergy))
                    Case "55"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.CapProjectile))
                    Case "74"
                        pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.05 * myGunnerySkills.CapHybrid))
                End Select
        End Select
        Select Case pilotGun.TertiarySkill
            Case "Small Artillery Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallArtSpec))
            Case "Small Autocannon Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallACSpec))
            Case "Small Beam Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallBLSpec))
            Case "Small Blaster Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallBlasterSpec))
            Case "Small Pulse Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallPLSpec))
            Case "Small Railgun Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.SmallRailSpec))

            Case "Medium Artillery Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedArtSpec))
            Case "Medium Autocannon Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedACSpec))
            Case "Medium Beam Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedBLSpec))
            Case "Medium Blaster Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedBlasterSpec))
            Case "Medium Pulse Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedPLSpec))
            Case "Medium Railgun Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.MedRailSpec))

            Case "Large Artillery Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeArtSpec))
            Case "Large Autocannon Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeACSpec))
            Case "Large Beam Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeBLSpec))
            Case "Large Blaster Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeBlasterSpec))
            Case "Large Pulse Laser Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargePLSpec))
            Case "Large Railgun Specialization"
                pilotGun.DamageModifier = pilotGun.DamageModifier * (1 + (0.02 * myGunnerySkills.LargeRailSpec))

        End Select

        Return pilotGun
    End Function
    Private Function ApplyGunneryMods(ByVal stdGun As GunClass) As GunClass
        Dim moddedGun As GunClass = CType(stdGun.Clone, GunClass)
        moddedGun.DamageModifier *= DamageBonus
        moddedGun.OptimalRange *= RangeBonus
        moddedGun.RateOfFire *= ROFBonus
        moddedGun.TrackingSpeed *= TrackingBonus
        Return moddedGun
    End Function
    Private Function LoadAmmoIntoGun(ByVal pilotGun As GunClass, ByVal ammo As AmmoClass) As GunClass
        ' Clone the gun
        Dim ammoGun As GunClass = CType(pilotGun.Clone, GunClass)
        ' Calcualte the range bonus
        ammoGun.OptimalRange *= ammo.RangeBonus
        ' Calculate the falloff bonus
        If ammo.FalloffBonus <> 0 Then
            ammoGun.AccuracyFalloff *= ammo.FalloffBonus
        End If
        ' Caluclate the cap usage bonus
        ammoGun.CapUsage += ammoGun.CapUsage * ammo.CapBonus / 100
        ' Calculate any ROF Bonus
        If ammo.ROFBonus <> 0 Then
            ammoGun.RateOfFire = ammoGun.RateOfFire * ammo.ROFBonus
        End If
        ' Calculate any Tracking Bonus
        If ammo.TrackingBonus <> 0 Then
            ammoGun.TrackingSpeed = ammoGun.TrackingSpeed * ammo.TrackingBonus
        End If
        ' Calculate the separate damages
        ammoGun.EMDamage = ammo.EMDamage * ammoGun.DamageModifier
        ammoGun.ExDamage = ammo.ExDamage * ammoGun.DamageModifier
        ammoGun.KiDamage = ammo.KiDamage * ammoGun.DamageModifier
        ammoGun.ThDamage = ammo.ThDamage * ammoGun.DamageModifier
        ' Calculate the total damage and DPS
        ammoGun.Damage = ammoGun.EMDamage + ammoGun.ExDamage + ammoGun.KiDamage + ammoGun.ThDamage
        ammoGun.DPS = ammoGun.Damage / ammoGun.RateOfFire
        Return ammoGun
    End Function
    Private Sub WeaponType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radEnergy.CheckedChanged, radHybrid.CheckedChanged, radProjectile.CheckedChanged
        If radHybrid.Checked = True Then
            Call Me.LoadHybridWeapons()
            Call Me.UpdateDM(302)
        Else
            If radEnergy.Checked = True Then
                Call Me.LoadEnergyWeapons()
                Call Me.UpdateDM(205)
            Else
                Call Me.LoadProjectileWeapons()
                Call Me.UpdateDM(59)
            End If
        End If
        cboDM.Enabled = False : nudDM.Enabled = False
        cboTC.Enabled = False : nudTC.Enabled = False
        cboTE.Enabled = False : nudTC.Enabled = False
    End Sub
    Private Sub UpdateDM(ByVal DMtype As Integer)
        Me.cboDM.BeginUpdate()
        Me.cboDM.Items.Clear()
        For Each DM As DamageModClass In DamageMods.Values
            If DMtype = CInt(DM.groupID) Then
                Me.cboDM.Items.Add(DM.typeName)
            End If
        Next
        Me.cboDM.EndUpdate()
    End Sub
    Private Sub LoadEnergyWeapons()
        cboWeapon.BeginUpdate()
        cboWeapon.Items.Clear()
        For Each energyGun As String In EnergyGuns.Keys
            cboWeapon.Items.Add(energyGun)
        Next
        cboWeapon.EndUpdate()
    End Sub
    Private Sub LoadHybridWeapons()
        cboWeapon.BeginUpdate()
        cboWeapon.Items.Clear()
        For Each hybridGun As String In HybridGuns.Keys
            cboWeapon.Items.Add(hybridGun)
        Next
        cboWeapon.EndUpdate()
    End Sub
    Private Sub LoadProjectileWeapons()
        cboWeapon.BeginUpdate()
        cboWeapon.Items.Clear()
        For Each projectileGun As String In ProjectileGuns.Keys
            cboWeapon.Items.Add(projectileGun)
        Next
        cboWeapon.EndUpdate()
    End Sub

    Private Sub cboDM_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDM.SelectedIndexChanged
        nudDM.Enabled = True
        currentDM = CType(DamageMods.Item(cboDM.SelectedItem), DamageModClass)
        Call Me.CalcDMBonus()
    End Sub
    Private Sub cboTC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTC.SelectedIndexChanged
        nudTC.Enabled = True
        currentTC = CType(TrackingMods.Item(cboTC.SelectedItem), TrackingModClass)
        Call Me.CalcTMBonus()
    End Sub
    Private Sub cboTE_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTE.SelectedIndexChanged
        nudTE.Enabled = True
        currentTE = CType(TrackingMods.Item(cboTE.SelectedItem), TrackingModClass)
        Call Me.CalcTMBonus()
    End Sub
    Private Sub nudDM_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDM.ValueChanged
        Call Me.CalcDMBonus()
    End Sub
    Private Sub nudTC_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTC.ValueChanged
        Call Me.CalcTMBonus()
    End Sub
    Private Sub nudTE_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTE.ValueChanged
        Call Me.CalcTMBonus()
    End Sub
    Private Sub CalcDMBonus()
        Dim DB, RB As Double
        DB = 1
        RB = 1
        If nudDM.Value <> 0 Then
            Dim effect As Double = 0
            For mods As Double = 1 To CInt(nudDM.Value)
                effect = StackNerf ^ (((mods - 1) ^ 2) / 2)         ' 1, 0.86912, 0.570583 etc
                DB *= 1 + ((currentDM.DamageBonus - 1) * effect)
                RB *= 1 + ((currentDM.ROFBonus - 1) * effect)
            Next
        End If
        DamageBonus = DB
        ROFBonus = RB
        lblDMBonus.Text = "Effective Dmg Bonus: " & FormatNumber(DB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblROFBonus.Text = "Effective ROF Bonus: " & FormatNumber(RB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Call Me.UpdateAmmoList()
    End Sub
    Private Sub CalcTMBonus()
        Dim TB, RB As Double
        TB = 1
        RB = 1
        If (nudTC.Value + nudTE.Value) <> 0 Then
            Dim effect As Double = 0
            For mods As Integer = 1 To CInt(nudTC.Value)
                effect = StackNerf ^ (((mods - 1) ^ 2) / 2)         ' 1, 0.86912, 0.570583 etc
                TB *= 1 + ((currentTC.TrackingBonus / 100) * effect)
                RB *= 1 + ((currentTC.RangeBonus / 100) * effect)
            Next
            For mods As Integer = CInt(nudTC.Value) + 1 To CInt(nudTC.Value) + CInt(nudTE.Value)
                effect = StackNerf ^ (((mods - 1) ^ 2) / 2)         ' 1, 0.86912, 0.570583 etc
                TB *= 1 + ((currentTE.TrackingBonus / 100) * effect)
                RB *= 1 + ((currentTE.RangeBonus / 100) * effect)
            Next
        End If
        TrackingBonus = TB
        RangeBonus = RB
        lblTCBonus.Text = "Effective Tracking Bonus: " & FormatNumber(TB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        lblRangeBonus.Text = "Effective Range Bonus: " & FormatNumber(RB, 8, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Call Me.UpdateAmmoList()
    End Sub

    Private Sub lvGuns_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvGuns.ColumnClick
        If lvGuns.Tag IsNot Nothing Then
            If CDbl(lvGuns.Tag.ToString) = e.Column Then
                Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
                lvGuns.Tag = -1
            Else
                Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
                lvGuns.Tag = e.Column
            End If
        Else
            Me.lvGuns.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvGuns.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvGuns.Sort()
    End Sub
End Class