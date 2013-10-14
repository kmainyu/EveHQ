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
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports Newtonsoft.Json

<Serializable()> Public Class PluginSettings

    Public Shared HQFSettings As New PluginSettings
    Public Shared HQFFolder As String
    Public Shared HQFCacheFolder As String

    Private cHiSlotColour As Long = Drawing.Color.PeachPuff.ToArgb
    Private cMidSlotColour As Long = Drawing.Color.LightSteelBlue.ToArgb
    Private cLowSlotColour As Long = Drawing.Color.Thistle.ToArgb
    Private cRigSlotColour As Long = Drawing.Color.LightGreen.ToArgb
    Private cSubSlotColour As Long = Drawing.Color.DarkSeaGreen.ToArgb
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
    Private cStandardSlotColumns As New List(Of UserSlotColumn)
    Private cUserSlotColumns As New List(Of UserSlotColumn)
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
    Private cStorageBayHeight As Integer = 200
    Private cSlotNameWidth As Integer = 150
    Private cImplantGroups As New SortedList(Of String, ImplantCollection)
    Private cModuleListColWidths As New SortedList(Of Long, Integer)
    Private cIgnoredAttributeColumns As New List(Of String)
    Private cSortedAttributeColumn As String = ""
    Private cMetaVariationsFormSize As New Drawing.Size
    Private cDefensePanelIsCollapsed As Boolean = False
    Private cCapacitorPanelIsCollapsed As Boolean = False
    Private cDamagePanelIsCollapsed As Boolean = False
    Private cTargetingPanelIsCollapsed As Boolean = False
    Private cPropulsionPanelIsCollapsed As Boolean = False
    Private cCargoPanelIsCollapsed As Boolean = False
    Private cSortedModuleListInfo As New EveHQ.Core.AdvTreeSortResult
    Private cAutoResizeColumns As Boolean = False

    Public Property AutoResizeColumns As Boolean
        Get
            Return cAutoResizeColumns
        End Get
        Set(ByVal value As Boolean)
            cAutoResizeColumns = value
        End Set
    End Property
    Public Property SortedModuleListInfo As Core.AdvTreeSortResult
        Get
            If cSortedModuleListInfo Is Nothing Then
                cSortedModuleListInfo = New Core.AdvTreeSortResult
            End If
            Return cSortedModuleListInfo
        End Get
        Set(ByVal value As Core.AdvTreeSortResult)
            cSortedModuleListInfo = value
        End Set
    End Property
    Public Property CargoPanelIsCollapsed As Boolean
        Get
            Return cCargoPanelIsCollapsed
        End Get
        Set(ByVal value As Boolean)
            cCargoPanelIsCollapsed = value
        End Set
    End Property
    Public Property PropulsionPanelIsCollapsed As Boolean
        Get
            Return cPropulsionPanelIsCollapsed
        End Get
        Set(value As Boolean)
            cPropulsionPanelIsCollapsed = value
        End Set
    End Property
    Public Property TargetingPanelIsCollapsed As Boolean
        Get
            Return cTargetingPanelIsCollapsed
        End Get
        Set(value As Boolean)
            cTargetingPanelIsCollapsed = value
        End Set
    End Property
    Public Property DamagePanelIsCollapsed As Boolean
        Get
            Return cDamagePanelIsCollapsed
        End Get
        Set(value As Boolean)
            cDamagePanelIsCollapsed = value
        End Set
    End Property
    Public Property CapacitorPanelIsCollapsed As Boolean
        Get
            Return cCapacitorPanelIsCollapsed
        End Get
        Set(value As Boolean)
            cCapacitorPanelIsCollapsed = value
        End Set
    End Property
    Public Property DefensePanelIsCollapsed As Boolean
        Get
            Return cDefensePanelIsCollapsed
        End Get
        Set(value As Boolean)
            cDefensePanelIsCollapsed = value
        End Set
    End Property
    Public Property MetaVariationsFormSize As Drawing.Size
        Get
            If cMetaVariationsFormSize.Width = 0 Then
                cMetaVariationsFormSize.Width = 900
            End If
            If cMetaVariationsFormSize.Height = 0 Then
                cMetaVariationsFormSize.Height = 550
            End If
            Return cMetaVariationsFormSize
        End Get
        Set(value As Drawing.Size)
            cMetaVariationsFormSize = value
        End Set
    End Property
    Public Property SortedAttributeColumn As String
        Get
            Return cSortedAttributeColumn
        End Get
        Set(value As String)
            cSortedAttributeColumn = value
        End Set
    End Property
    Public Property IgnoredAttributeColumns As List(Of String)
        Get
            If cIgnoredAttributeColumns Is Nothing Then
                cIgnoredAttributeColumns = New List(Of String)
            End If
            Return cIgnoredAttributeColumns
        End Get
        Set(value As List(Of String))
            cIgnoredAttributeColumns = value
        End Set
    End Property
    Public Property ModuleListColWidths() As SortedList(Of Long, Integer)
        Get
            If cModuleListColWidths Is Nothing Then
                cModuleListColWidths = New SortedList(Of Long, Integer)
            End If
            Return cModuleListColWidths
        End Get
        Set(ByVal value As SortedList(Of Long, Integer))
            cModuleListColWidths = value
        End Set
    End Property
    Public Property ImplantGroups() As SortedList(Of String, ImplantCollection)
        Get
            Return cImplantGroups
        End Get
        Set(ByVal value As SortedList(Of String, ImplantCollection))
            cImplantGroups = value
        End Set
    End Property

    Public Property SlotNameWidth() As Integer
        Get
            Return cSlotNameWidth
        End Get
        Set(ByVal value As Integer)
            cSlotNameWidth = value
        End Set
    End Property
    Public Property StorageBayHeight() As Integer
        Get
            Return cStorageBayHeight
        End Get
        Set(ByVal value As Integer)
            cStorageBayHeight = value
        End Set
    End Property
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
    Public Property UserSlotColumns() As List(Of UserSlotColumn)
        Get
            Return cUserSlotColumns
        End Get
        Set(ByVal value As List(Of UserSlotColumn))
            cUserSlotColumns = value
        End Set
    End Property
    Public Property StandardSlotColumns() As List(Of UserSlotColumn)
        Get
            Return cStandardSlotColumns
        End Get
        Set(ByVal value As List(Of UserSlotColumn))
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

        ' Create a JSON string for writing
        Dim json As String = JsonConvert.SerializeObject(HQFSettings, Newtonsoft.Json.Formatting.Indented)

        ' Write the JSON version of the settings
        Try
            Using s As New StreamWriter(Path.Combine(PluginSettings.HQFFolder, "HQFSettings.json"), False)
                s.Write(json)
                s.Flush()
            End Using
        Catch e As Exception
        End Try

    End Sub
    Public Function LoadHQFSettings() As Boolean

        ' Initialise the standard slot columns
        Call InitialiseSlotColumns()

        If My.Computer.FileSystem.FileExists(Path.Combine(PluginSettings.HQFFolder, "HQFSettings.json")) = True Then
            Try
                Using s As New StreamReader(Path.Combine(PluginSettings.HQFFolder, "HQFSettings.json"))
                    Dim json As String = s.ReadToEnd
                    HQFSettings = JsonConvert.DeserializeObject(Of PluginSettings)(json)
                End Using
            Catch ex As Exception
                MessageBox.Show("There was an error loading the HQF Settings file. The file appears corrupt, so it cannot be loaded at this time.")
            End Try
        End If

        ' Update implant names
        For Each impGroup As ImplantCollection In HQFSettings.ImplantGroups.Values
            For slot As Integer = 1 To 10
                If PlugInData.ModuleChanges.ContainsKey(impGroup.ImplantName(slot)) Then
                    impGroup.ImplantName(slot) = PlugInData.ModuleChanges(impGroup.ImplantName(slot))
                End If
            Next
        Next
        For Each pilot As FittingPilot In FittingPilots.HQFPilots.Values
            For slot As Integer = 1 To 10
                If PlugInData.ModuleChanges.ContainsKey(pilot.ImplantName(slot)) Then
                    pilot.ImplantName(slot) = PlugInData.ModuleChanges(pilot.ImplantName(slot))
                End If
            Next
        Next

        ' Check if the standard columns have changed and we need to add columns
        If HQFSettings.UserSlotColumns.Count <> HQFSettings.StandardSlotColumns.Count Then
            Dim missingFlag As Boolean
            For Each stdCol As UserSlotColumn In cStandardSlotColumns
                missingFlag = True
                For Each testUserCol As UserSlotColumn In HQFSettings.UserSlotColumns
                    If stdCol.Name = testUserCol.Name Then
                        missingFlag = False
                        Exit For
                    End If
                Next
                If missingFlag = True Then
                    HQFSettings.UserSlotColumns.Add(New UserSlotColumn(stdCol.Name, stdCol.Description, stdCol.Width, stdCol.Active))
                End If
            Next
        End If
        Return True

    End Function
    Public Sub InitialiseSlotColumns()
        cStandardSlotColumns.Clear()
        cStandardSlotColumns.Add(New UserSlotColumn("Charge", "Module Charge", 150, True))
        cStandardSlotColumns.Add(New UserSlotColumn("CPU", "CPU", 75, True))
        cStandardSlotColumns.Add(New UserSlotColumn("PG", "PG", 75, True))
        cStandardSlotColumns.Add(New UserSlotColumn("Calib", "Calibration", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("Price", "Price", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("ActCost", "Activation Cost", 75, True))
        cStandardSlotColumns.Add(New UserSlotColumn("ActTime", "Activation Time", 75, True))
        cStandardSlotColumns.Add(New UserSlotColumn("CapRate", "Cap Usage Rate", 75, True))
        cStandardSlotColumns.Add(New UserSlotColumn("OptRange", "Optimal Range", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("ROF", "ROF", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("Damage", "Damage", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("DPS", "DPS", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("Falloff", "Falloff", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("Tracking", "Tracking", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("ExpRad", "Explosion Radius", 75, False))
        cStandardSlotColumns.Add(New UserSlotColumn("ExpVel", "Explosion Velocity", 75, False))
    End Sub

End Class

<Serializable()> Public Class UserSlotColumn
    Dim cName As String = ""
    Dim cDescription As String = ""
    Dim cWidth As Integer = 75
    Dim cActive As Boolean = False

    Public Property Name() As String
        Get
            Return cName
        End Get
        Set(ByVal value As String)
            cName = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return cDescription
        End Get
        Set(ByVal value As String)
            cDescription = value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return cWidth
        End Get
        Set(ByVal value As Integer)
            cWidth = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return cActive
        End Get
        Set(ByVal value As Boolean)
            cActive = value
        End Set
    End Property

    Public Sub New(ByVal ColumnName As String, ByVal Description As String, ByVal ColumnWidth As Integer, ByVal IsActive As Boolean)
        cName = ColumnName
        cDescription = Description
        cWidth = ColumnWidth
        cActive = IsActive
    End Sub

End Class
