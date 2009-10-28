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
Imports System
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Xml
Imports System.Web
Imports System.Windows.Forms
Imports System.Runtime.Serialization.Formatters.Binary

Public Class Settings

    Public Shared HQFSettings As New HQF.Settings
    Public Shared HQFFolder As String
    Public Shared HQFCacheFolder As String

    Private cHiSlotColour As Long = System.Drawing.Color.PeachPuff.ToArgb
    Private cMidSlotColour As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cLowSlotColour As Long = System.Drawing.Color.Thistle.ToArgb
    Private cRigSlotColour As Long = System.Drawing.Color.LightGreen.ToArgb
    Private cSubSlotColour As Long = System.Drawing.Color.DarkSeaGreen.ToArgb
    Private cDefaultPilot As String = ""
    Private cRestoreLastSession As Boolean = False
    Private cLastPriceUpdate As DateTime
    Private cModuleFilter As Integer = 63
    Private cAutoUpdateHQFSkills As Boolean = True
    Private cOpenFittingList As New ArrayList
    Private cShowPerformanceData As Boolean = False
    Private cCloseInfoPanel As Boolean = False
    Private cCapRechargeConstant As Double = 2.5
    Private cShieldRechargeConstant As Double = 2.5
    Private cStandardSlotColumns As New ArrayList
    Private cUserSlotColumns As New ArrayList
    Private cFavourites As New ArrayList
    Private cMRULimit As Integer = 15
    Private cMRUModules As New ArrayList
    Private cShipPanelWidth As Integer = 200
    Private cModPanelWidth As Integer = 300
    Private cShipSplitterWidth As Integer = 300
    Private cModSplitterWidth As Integer = 300
    Private cMissileRangeConstant As Double = 1.0
    Private cIncludeCapReloadTime As Boolean = False
    Private cIncludeAmmoReloadTime As Boolean = False
    Private cUseLastPilot As Boolean = False

    Public Property UseLastPilot() As Boolean
        Get
            Return cUseLastPilot
        End Get
        Set(ByVal value As Boolean)
            cUseLastPilot = value
        End Set
    End Property
    Public Property IncludeAmmoReloadTime() As Boolean
        Get
            Return cIncludeAmmoReloadTime
        End Get
        Set(ByVal value As Boolean)
            cIncludeAmmoReloadTime = value
        End Set
    End Property
    Public Property IncludeCapReloadTime() As Boolean
        Get
            Return cIncludeCapReloadTime
        End Get
        Set(ByVal value As Boolean)
            cIncludeCapReloadTime = value
        End Set
    End Property
    Public Property MissileRangeConstant() As Double
        Get
            Return cMissileRangeConstant
        End Get
        Set(ByVal value As Double)
            cMissileRangeConstant = value
        End Set
    End Property
    Public Property ModSplitterWidth() As Integer
        Get
            Return cModSplitterWidth
        End Get
        Set(ByVal value As Integer)
            cModSplitterWidth = value
        End Set
    End Property
    Public Property ShipSplitterWidth() As Integer
        Get
            Return cShipSplitterWidth
        End Get
        Set(ByVal value As Integer)
            cShipSplitterWidth = value
        End Set
    End Property
    Public Property ModPanelWidth() As Integer
        Get
            Return cModPanelWidth
        End Get
        Set(ByVal value As Integer)
            cModPanelWidth = value
        End Set
    End Property
    Public Property ShipPanelWidth() As Integer
        Get
            Return cShipPanelWidth
        End Get
        Set(ByVal value As Integer)
            cShipPanelWidth = value
        End Set
    End Property
    Public Property MRUModules() As ArrayList
        Get
            Return cMRUModules
        End Get
        Set(ByVal value As ArrayList)
            cMRUModules = value
        End Set
    End Property
    Public Property MRULimit() As Integer
        Get
            Return cMRULimit
        End Get
        Set(ByVal value As Integer)
            cMRULimit = value
        End Set
    End Property
    Public Property Favourites() As ArrayList
        Get
            Return cFavourites
        End Get
        Set(ByVal value As ArrayList)
            cFavourites = value
        End Set
    End Property
    Public Property UserSlotColumns() As ArrayList
        Get
            Return cUserSlotColumns
        End Get
        Set(ByVal value As ArrayList)
            cUserSlotColumns = value
        End Set
    End Property
    Public Property StandardSlotColumns() As ArrayList
        Get
            Return cStandardSlotColumns
        End Get
        Set(ByVal value As ArrayList)
            cStandardSlotColumns = value
        End Set
    End Property
    Public Property ShieldRechargeConstant() As Double
        Get
            Return cShieldRechargeConstant
        End Get
        Set(ByVal value As Double)
            cShieldRechargeConstant = value
        End Set
    End Property
    Public Property CapRechargeConstant() As Double
        Get
            Return cCapRechargeConstant
        End Get
        Set(ByVal value As Double)
            cCapRechargeConstant = value
        End Set
    End Property
    Public Property CloseInfoPanel() As Boolean
        Get
            Return cCloseInfoPanel
        End Get
        Set(ByVal value As Boolean)
            cCloseInfoPanel = value
        End Set
    End Property
    Public Property ShowPerformanceData() As Boolean
        Get
            Return cShowPerformanceData
        End Get
        Set(ByVal value As Boolean)
            cShowPerformanceData = value
        End Set
    End Property
    Public Property OpenFittingList() As ArrayList
        Get
            Return cOpenFittingList
        End Get
        Set(ByVal value As ArrayList)
            cOpenFittingList = value
        End Set
    End Property
    Public Property AutoUpdateHQFSkills() As Boolean
        Get
            Return cAutoUpdateHQFSkills
        End Get
        Set(ByVal value As Boolean)
            cAutoUpdateHQFSkills = value
        End Set
    End Property
    Public Property ModuleFilter() As Integer
        Get
            Return cModuleFilter
        End Get
        Set(ByVal value As Integer)
            cModuleFilter = value
        End Set
    End Property
    Public Property LastPriceUpdate() As DateTime
        Get
            Return cLastPriceUpdate
        End Get
        Set(ByVal value As DateTime)
            cLastPriceUpdate = value
        End Set
    End Property
    Public Property RestoreLastSession() As Boolean
        Get
            Return cRestoreLastSession
        End Get
        Set(ByVal value As Boolean)
            cRestoreLastSession = value
        End Set
    End Property
    Public Property DefaultPilot() As String
        Get
            Return cDefaultPilot
        End Get
        Set(ByVal value As String)
            cDefaultPilot = value
        End Set
    End Property
    Public Property HiSlotColour() As Long
        Get
            Return cHiSlotColour
        End Get
        Set(ByVal value As Long)
            cHiSlotColour = value
        End Set
    End Property
    Public Property MidSlotColour() As Long
        Get
            Return cMidSlotColour
        End Get
        Set(ByVal value As Long)
            cMidSlotColour = value
        End Set
    End Property
    Public Property LowSlotColour() As Long
        Get
            Return cLowSlotColour
        End Get
        Set(ByVal value As Long)
            cLowSlotColour = value
        End Set
    End Property
    Public Property RigSlotColour() As Long
        Get
            Return cRigSlotColour
        End Get
        Set(ByVal value As Long)
            cRigSlotColour = value
        End Set
    End Property
    Public Property SubSlotColour() As Long
        Get
            Return cSubSlotColour
        End Get
        Set(ByVal value As Long)
            cSubSlotColour = value
        End Set
    End Property

    Public Sub SaveHQFSettings()
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""

        ' Prepare the XML document
        XMLS = ("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>") & vbCrLf
        XMLS &= "<HQFSettings>" & vbCrLf

        ' Save the General Information
        XMLS &= Chr(9) & "<general>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<hiSlotColour>" & HQFSettings.HiSlotColour & "</hiSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<midSlotColour>" & HQFSettings.MidSlotColour & "</midSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<lowSlotColour>" & HQFSettings.LowSlotColour & "</lowSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<rigSlotColour>" & HQFSettings.RigSlotColour & "</rigSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<defaultPilot>" & HQFSettings.DefaultPilot & "</defaultPilot>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<restoreLastSession>" & HQFSettings.RestoreLastSession & "</restoreLastSession>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<lastPriceUpdate>" & HQFSettings.LastPriceUpdate & "</lastPriceUpdate>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<moduleFilter>" & HQFSettings.ModuleFilter & "</moduleFilter>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<updateHQFSkills>" & HQFSettings.AutoUpdateHQFSkills & "</updateHQFSkills>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<showPerformance>" & HQFSettings.ShowPerformanceData & "</showPerformance>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<closeInfoPanel>" & HQFSettings.CloseInfoPanel & "</closeInfoPanel>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<capRechargeConst>" & HQFSettings.CapRechargeConstant & "</capRechargeConst>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<shieldRechargeConst>" & HQFSettings.ShieldRechargeConstant & "</shieldRechargeConst>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<shipPanelWidth>" & HQFSettings.ShipPanelWidth & "</shipPanelWidth>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<modPanelWidth>" & HQFSettings.ModPanelWidth & "</modPanelWidth>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<shipSplitterWidth>" & HQFSettings.ShipSplitterWidth & "</shipSplitterWidth>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<modSplitterWidth>" & HQFSettings.ModSplitterWidth & "</modSplitterWidth>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<missileRangeConst>" & HQFSettings.MissileRangeConstant & "</missileRangeConst>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<subSlotColour>" & HQFSettings.SubSlotColour & "</subSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<includeAmmoTime>" & HQFSettings.IncludeAmmoReloadTime & "</includeAmmoTime>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<includeCapTime>" & HQFSettings.IncludeCapReloadTime & "</includeCapTime>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<useLastPilot>" & HQFSettings.UseLastPilot & "</useLastPilot>" & vbCrLf
        XMLS &= Chr(9) & "</general>" & vbCrLf

        ' Save the slot layout
        XMLS &= Chr(9) & "<slotLayout>" & vbCrLf
        For Each slot As String In HQFSettings.UserSlotColumns
            XMLS &= Chr(9) & Chr(9) & "<slot>" & slot & "</slot>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</slotLayout>" & vbCrLf

        ' Save favourites
        HQFSettings.Favourites.Sort()
        XMLS &= Chr(9) & "<favourites>" & vbCrLf
        For Each favMod As String In HQFSettings.Favourites
            XMLS &= Chr(9) & Chr(9) & "<item>" & favMod & "</item>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</favourites>" & vbCrLf

        ' Save the open fittings
        XMLS &= Chr(9) & "<openFittings>" & vbCrLf
        For Each fitting As String In ShipLists.fittedShipList.Keys
            XMLS &= Chr(9) & Chr(9) & "<fitting>" & HttpUtility.HtmlEncode(fitting) & "</fitting>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</openFittings>" & vbCrLf

        ' Save the Implant groups
        XMLS &= Chr(9) & "<implantGroups>" & vbCrLf
        For Each implantSet As ImplantGroup In Implants.implantGroups.Values
            XMLS &= Chr(9) & Chr(9) & "<implantGroup>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<implantGroupName>" & implantSet.GroupName & "</implantGroupName>" & vbCrLf
            For imp As Integer = 1 To 10
                XMLS &= Chr(9) & Chr(9) & Chr(9) & "<implantName>" & implantSet.ImplantName(imp) & "</implantName>" & vbCrLf
            Next
            XMLS &= Chr(9) & Chr(9) & "</implantGroup>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</implantGroups>" & vbCrLf

        ' End Settings
        XMLS &= "</HQFSettings>"
        XMLdoc.LoadXml(XMLS)
        Try
            XMLdoc.Save(Path.Combine(HQFFolder, "HQFSettings.xml"))
        Catch e As Exception
            'Console.WriteLine(e.Message)
        End Try


    End Sub
    Public Function LoadHQFSettings() As Boolean
        Dim XMLdoc As XmlDocument = New XmlDocument

        ' Initialise the standard slot columns
        Call Me.InitialiseSlotColumns()

        If My.Computer.FileSystem.FileExists(Path.Combine(HQFFolder, "HQFSettings.xml")) = True Then
            XMLdoc.Load(Path.Combine(HQFFolder, "HQFSettings.xml"))
            Dim settingDetails As XmlNodeList
            Dim settingSettings As XmlNode

            ' Get the General Settings
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/general")
                If settingDetails.Count <> 0 Then
                    ' Get the relevant node!
                    settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/pilots node in each XML doc
                    If settingSettings.HasChildNodes Then
                        HQFSettings.HiSlotColour = CLng(settingSettings.ChildNodes(0).InnerText)
                        HQFSettings.MidSlotColour = CLng(settingSettings.ChildNodes(1).InnerText)
                        HQFSettings.LowSlotColour = CLng(settingSettings.ChildNodes(2).InnerText)
                        HQFSettings.RigSlotColour = CLng(settingSettings.ChildNodes(3).InnerText)
                        HQFSettings.DefaultPilot = CStr(settingSettings.ChildNodes(4).InnerText)
                        HQFSettings.RestoreLastSession = CBool(settingSettings.ChildNodes(5).InnerText)
                        HQFSettings.LastPriceUpdate = CDate(settingSettings.ChildNodes(6).InnerText)
                        HQFSettings.ModuleFilter = CInt(settingSettings.ChildNodes(7).InnerText)
                        HQFSettings.AutoUpdateHQFSkills = CBool(settingSettings.ChildNodes(8).InnerText)
                        HQFSettings.ShowPerformanceData = CBool(settingSettings.ChildNodes(9).InnerText)
                        HQFSettings.CloseInfoPanel = CBool(settingSettings.ChildNodes(10).InnerText)
                        HQFSettings.CapRechargeConstant = CDbl(settingSettings.ChildNodes(11).InnerText)
                        HQFSettings.ShieldRechargeConstant = CDbl(settingSettings.ChildNodes(12).InnerText)
                        HQFSettings.ShipPanelWidth = CInt(settingSettings.ChildNodes(13).InnerText)
                        HQFSettings.ModPanelWidth = CInt(settingSettings.ChildNodes(14).InnerText)
                        HQFSettings.ShipSplitterWidth = CInt(settingSettings.ChildNodes(15).InnerText)
                        HQFSettings.ModSplitterWidth = CInt(settingSettings.ChildNodes(16).InnerText)
                        HQFSettings.MissileRangeConstant = CDbl(settingSettings.ChildNodes(17).InnerText)
                        HQFSettings.SubSlotColour = CLng(settingSettings.ChildNodes(18).InnerText)
                        HQFSettings.IncludeAmmoReloadTime = CBool(settingSettings.ChildNodes(19).InnerText)
                        HQFSettings.IncludeCapReloadTime = CBool(settingSettings.ChildNodes(20).InnerText)
                        HQFSettings.UseLastPilot = CBool(settingSettings.ChildNodes(21).InnerText)
                    End If
                End If
            Catch
            End Try

            ' Get the slot columns layout
            HQFSettings.UserSlotColumns.Clear()
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/slotLayout")
                ' Get the relevant node!
                settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If settingSettings.HasChildNodes Then
                    For group As Integer = 0 To settingSettings.ChildNodes.Count - 1
                        HQFSettings.UserSlotColumns.Add(settingSettings.ChildNodes(group).InnerText)
                    Next
                End If
            Catch
            End Try

            ' Get the favourites
            HQFSettings.Favourites.Clear()
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/favourites")
                ' Get the relevant node!
                settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If settingSettings.HasChildNodes Then
                    For item As Integer = 0 To settingSettings.ChildNodes.Count - 1
                        HQFSettings.Favourites.Add(settingSettings.ChildNodes(item).InnerText)
                    Next
                End If
                HQFSettings.Favourites.Sort()
            Catch
            End Try

            ' Get the open fitting details details
            HQFSettings.OpenFittingList.Clear()
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/openFittings")
                ' Get the relevant node!
                settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If settingSettings.HasChildNodes Then
                    For group As Integer = 0 To settingSettings.ChildNodes.Count - 1
                        HQFSettings.OpenFittingList.Add(HttpUtility.HtmlDecode(settingSettings.ChildNodes(group).InnerText))
                    Next
                End If
            Catch
            End Try


            ' Get the implant details
            Implants.implantGroups.Clear()
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/implantGroups")
                ' Get the relevant node!
                settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If settingSettings.HasChildNodes Then
                    For group As Integer = 0 To settingSettings.ChildNodes.Count - 1
                        Dim newImplantGroup As New ImplantGroup
                        newImplantGroup.GroupName = settingSettings.ChildNodes(group).ChildNodes(0).InnerText
                        For imp As Integer = 1 To 10
                            newImplantGroup.ImplantName(imp) = CStr(settingSettings.ChildNodes(group).ChildNodes(imp).InnerText)
                        Next
                        Implants.implantGroups.Add(newImplantGroup.GroupName, newImplantGroup)
                    Next
                End If
            Catch
            End Try

        End If

        ' Check if the standard columns have changed and we need to add columns
        If HQFSettings.UserSlotColumns.Count <> HQFSettings.StandardSlotColumns.Count Then
            For Each slotItem As ListViewItem In cStandardSlotColumns
                If HQFSettings.UserSlotColumns.Contains(slotItem.Name & "0") = False And HQFSettings.UserSlotColumns.Contains(slotItem.Name & "1") = False Then
                    HQFSettings.UserSlotColumns.Add(slotItem.Name & "0")
                End If
            Next
        End If
        Return True

    End Function
    Public Sub InitialiseSlotColumns()
        cStandardSlotColumns.Clear()
        Dim newItem As New ListViewItem
        ' Setup Charge Item
        newItem = New ListViewItem
        newItem.Name = "Charge"
        newItem.Text = "Charge"
        newItem.Checked = True
        cStandardSlotColumns.Add(newItem)
        ' Setup CPU Item
        newItem = New ListViewItem
        newItem.Name = "CPU"
        newItem.Text = "CPU"
        newItem.Checked = True
        cStandardSlotColumns.Add(newItem)
        ' Setup PG Item
        newItem = New ListViewItem
        newItem.Name = "PG"
        newItem.Text = "PG"
        newItem.Checked = True
        cStandardSlotColumns.Add(newItem)
        ' Setup Calibration Item
        newItem = New ListViewItem
        newItem.Name = "Calibration"
        newItem.Text = "Calibration"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Price Item
        newItem = New ListViewItem
        newItem.Name = "Price"
        newItem.Text = "Price"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Activation Cost Item
        newItem = New ListViewItem
        newItem.Name = "ActCost"
        newItem.Text = "Activation Cost"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Activation Time Item
        newItem = New ListViewItem
        newItem.Name = "ActTime"
        newItem.Text = "Activation Time"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Cap Usage Rate Item
        newItem = New ListViewItem
        newItem.Name = "CapUsageRate"
        newItem.Text = "Cap Usage Rate"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Optimal Range Item
        newItem = New ListViewItem
        newItem.Name = "OptRange"
        newItem.Text = "Optimal Range"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup ROF Item
        newItem = New ListViewItem
        newItem.Name = "ROF"
        newItem.Text = "Rate Of Fire"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Damage Item
        newItem = New ListViewItem
        newItem.Name = "Damage"
        newItem.Text = "Damage"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup DPS Item
        newItem = New ListViewItem
        newItem.Name = "DPS"
        newItem.Text = "DPS"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Falloff
        newItem = New ListViewItem
        newItem.Name = "Falloff"
        newItem.Text = "Falloff"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Tracking
        newItem = New ListViewItem
        newItem.Name = "Tracking"
        newItem.Text = "Tracking"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Tracking
        newItem = New ListViewItem
        newItem.Name = "ExpRad"
        newItem.Text = "Explosion Radius"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
        ' Setup Tracking
        newItem = New ListViewItem
        newItem.Name = "ExpVel"
        newItem.Text = "Explosion Velocity"
        newItem.Checked = False
        cStandardSlotColumns.Add(newItem)
    End Sub
    Public Sub LoadProfiles()
        ' Check for the profiles file so we can load it
        If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin")) = True Then
            Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin"), FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            DamageProfiles.ProfileList = CType(f.Deserialize(s), SortedList)
            s.Close()
        Else
            ' Need to create the profiles file and the standard custom profile (omni-damage)
            Dim NPCGroups(13) As String
            Dim newProfile As New DamageProfile
            DamageProfiles.ProfileList.Clear()
            NPCGroups(0) = "Angel Cartel" : NPCGroups(1) = "Blood Raiders" : NPCGroups(2) = "Guristas" : NPCGroups(3) = "Rogue Drone"
            NPCGroups(4) = "Sansha's Nation" : NPCGroups(5) = "Serpentis" : NPCGroups(6) = "Amarr Empire" : NPCGroups(7) = "Caldari State"
            NPCGroups(8) = "CONCORD" : NPCGroups(9) = "Gallente Federation" : NPCGroups(10) = "Khanid" : NPCGroups(11) = "Minmatar Republic"
            NPCGroups(12) = "Mordu" : NPCGroups(13) = "Thukker"
            Dim damage(13, 3) As Double
            For Each newNPC As NPC In NPCs.NPCList.Values
                For NPCGroup As Integer = 0 To 13
                    If newNPC.GroupName.Contains(NPCGroups(NPCGroup)) = True Then
                        damage(NPCGroup, 0) += newNPC.EM
                        damage(NPCGroup, 1) += newNPC.Explosive
                        damage(NPCGroup, 2) += newNPC.Kinetic
                        damage(NPCGroup, 3) += newNPC.Thermal
                    End If
                Next
            Next
            For NPCGroup As Integer = 0 To 13
                newProfile = New DamageProfile
                newProfile.Name = NPCGroups(NPCGroup)
                Dim damagetotal As Double = 0
                For damageType As Integer = 0 To 3
                    damagetotal += damage(NPCGroup, damageType)
                Next
                For damageType As Integer = 0 To 3
                    damage(NPCGroup, damageType) = damage(NPCGroup, damageType) / damagetotal * 100
                Next
                newProfile.Type = 0
                newProfile.EM = damage(NPCGroup, 0)
                newProfile.Explosive = damage(NPCGroup, 1)
                newProfile.Kinetic = damage(NPCGroup, 2)
                newProfile.Thermal = damage(NPCGroup, 3)
                DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
            Next
            ' Save Omni Damage
            newProfile = New DamageProfile
            newProfile.Name = "<Omni-Damage>"
            newProfile.Type = 0
            newProfile.EM = 25 : newProfile.Explosive = 25 : newProfile.Kinetic = 25 : newProfile.Thermal = 25 : newProfile.DPS = 0
            newProfile.Fitting = ""
            newProfile.Pilot = ""
            newProfile.NPCs.Clear()
            DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
            ' Save EM Damage
            newProfile = New DamageProfile
            newProfile.Name = "Pure EM"
            newProfile.Type = 0
            newProfile.EM = 100 : newProfile.Explosive = 0 : newProfile.Kinetic = 0 : newProfile.Thermal = 0 : newProfile.DPS = 0
            newProfile.Fitting = ""
            newProfile.Pilot = ""
            newProfile.NPCs.Clear()
            DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
            ' Save Explosive Damage
            newProfile = New DamageProfile
            newProfile.Name = "Pure Explosive"
            newProfile.Type = 0
            newProfile.EM = 0 : newProfile.Explosive = 100 : newProfile.Kinetic = 0 : newProfile.Thermal = 0 : newProfile.DPS = 0
            newProfile.Fitting = ""
            newProfile.Pilot = ""
            newProfile.NPCs.Clear()
            DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
            ' Save Kinetic Damage
            newProfile = New DamageProfile
            newProfile.Name = "Pure Kinetic"
            newProfile.Type = 0
            newProfile.EM = 0 : newProfile.Explosive = 0 : newProfile.Kinetic = 100 : newProfile.Thermal = 0 : newProfile.DPS = 0
            newProfile.Fitting = ""
            newProfile.Pilot = ""
            newProfile.NPCs.Clear()
            DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
            ' Save Thermal Damage
            newProfile = New DamageProfile
            newProfile.Name = "Pure Thermal"
            newProfile.Type = 0
            newProfile.EM = 0 : newProfile.Explosive = 0 : newProfile.Kinetic = 0 : newProfile.Thermal = 100 : newProfile.DPS = 0
            newProfile.Fitting = ""
            newProfile.Pilot = ""
            newProfile.NPCs.Clear()
            DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
            Call SaveProfiles()
        End If
    End Sub
    Public Sub SaveProfiles()
        ' Save the Profiles
        Dim s As New FileStream(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, DamageProfiles.ProfileList)
        s.Flush()
        s.Close()
    End Sub

End Class
