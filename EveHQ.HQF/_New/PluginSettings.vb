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
Imports System.Drawing
Imports EveHQ.Core
Imports System.IO
Imports System.Windows.Forms
Imports Newtonsoft.Json

<Serializable()> Public Class PluginSettings

    Public Shared HQFSettings As New PluginSettings
    Public Shared HQFFolder As String
    Public Shared HQFCacheFolder As String

#Region "Property Fields"

    Private _hiSlotColour As Long = Color.PeachPuff.ToArgb
    Private _midSlotColour As Long = Color.LightSteelBlue.ToArgb
    Private _lowSlotColour As Long = Color.Thistle.ToArgb
    Private _rigSlotColour As Long = Color.LightGreen.ToArgb
    Private _subSlotColour As Long = Color.DarkSeaGreen.ToArgb
    Private _defaultPilot As String = ""
    Private _restoreLastSession As Boolean = False
    Private _lastPriceUpdate As DateTime
    Private _moduleFilter As Integer = 63
    Private _autoUpdateHQFSkills As Boolean = True
    Private _openFittingList As New ArrayList
    Private _showPerformanceData As Boolean = False
    Private _closeInfoPanel As Boolean = False
    Private _capRechargeConstant As Double = 2.5
    Private _shieldRechargeConstant As Double = 2.5
    Private _standardSlotColumns As New List(Of UserSlotColumn)
    Private _userSlotColumns As New List(Of UserSlotColumn)
    Private _favourites As New ArrayList
    Private _mruLimit As Integer = 15
    Private _mruModules As New ArrayList
    Private _shipPanelWidth As Integer = 200
    Private _modPanelWidth As Integer = 300
    Private _shipSplitterWidth As Integer = 300
    Private _modSplitterWidth As Integer = 300
    Private _missileRangeConstant As Double = 1.0
    Private _includeCapReloadTime As Boolean = False
    Private _includeAmmoReloadTime As Boolean = False
    Private _useLastPilot As Boolean = False
    Private _storageBayHeight As Integer = 200
    Private _slotNameWidth As Integer = 150
    Private _implantGroups As New SortedList(Of String, ImplantCollection)
    Private _moduleListColWidths As New SortedList(Of Long, Integer)
    Private _ignoredAttributeColumns As New List(Of String)
    Private _sortedAttributeColumn As String = ""
    Private _metaVariationsFormSize As New Size
    Private _defensePanelIsCollapsed As Boolean = False
    Private _capacitorPanelIsCollapsed As Boolean = False
    Private _damagePanelIsCollapsed As Boolean = False
    Private _targetingPanelIsCollapsed As Boolean = False
    Private _propulsionPanelIsCollapsed As Boolean = False
    Private _cargoPanelIsCollapsed As Boolean = False
    Private _sortedModuleListInfo As New AdvTreeSortResult
    Private _autoResizeColumns As Boolean = False

#End Region

#Region "Properties"

    Public Property AutoResizeColumns As Boolean
        Get
            Return _AutoResizeColumns
        End Get
        Set(ByVal value As Boolean)
            _AutoResizeColumns = value
        End Set
    End Property
    Public Property SortedModuleListInfo As AdvTreeSortResult
        Get
            If _SortedModuleListInfo Is Nothing Then
                _SortedModuleListInfo = New AdvTreeSortResult
            End If
            Return _SortedModuleListInfo
        End Get
        Set(ByVal value As AdvTreeSortResult)
            _SortedModuleListInfo = value
        End Set
    End Property
    Public Property CargoPanelIsCollapsed As Boolean
        Get
            Return _CargoPanelIsCollapsed
        End Get
        Set(ByVal value As Boolean)
            _CargoPanelIsCollapsed = value
        End Set
    End Property
    Public Property PropulsionPanelIsCollapsed As Boolean
        Get
            Return _PropulsionPanelIsCollapsed
        End Get
        Set(value As Boolean)
            _PropulsionPanelIsCollapsed = value
        End Set
    End Property
    Public Property TargetingPanelIsCollapsed As Boolean
        Get
            Return _TargetingPanelIsCollapsed
        End Get
        Set(value As Boolean)
            _TargetingPanelIsCollapsed = value
        End Set
    End Property
    Public Property DamagePanelIsCollapsed As Boolean
        Get
            Return _DamagePanelIsCollapsed
        End Get
        Set(value As Boolean)
            _DamagePanelIsCollapsed = value
        End Set
    End Property
    Public Property CapacitorPanelIsCollapsed As Boolean
        Get
            Return _CapacitorPanelIsCollapsed
        End Get
        Set(value As Boolean)
            _CapacitorPanelIsCollapsed = value
        End Set
    End Property
    Public Property DefensePanelIsCollapsed As Boolean
        Get
            Return _DefensePanelIsCollapsed
        End Get
        Set(value As Boolean)
            _DefensePanelIsCollapsed = value
        End Set
    End Property
    Public Property MetaVariationsFormSize As Size
        Get
            If _MetaVariationsFormSize.Width = 0 Then
                _MetaVariationsFormSize.Width = 900
            End If
            If _MetaVariationsFormSize.Height = 0 Then
                _MetaVariationsFormSize.Height = 550
            End If
            Return _MetaVariationsFormSize
        End Get
        Set(value As Size)
            _MetaVariationsFormSize = value
        End Set
    End Property
    Public Property SortedAttributeColumn As String
        Get
            Return _SortedAttributeColumn
        End Get
        Set(value As String)
            _SortedAttributeColumn = value
        End Set
    End Property
    Public Property IgnoredAttributeColumns As List(Of String)
        Get
            If _IgnoredAttributeColumns Is Nothing Then
                _IgnoredAttributeColumns = New List(Of String)
            End If
            Return _IgnoredAttributeColumns
        End Get
        Set(value As List(Of String))
            _IgnoredAttributeColumns = value
        End Set
    End Property
    Public Property ModuleListColWidths() As SortedList(Of Long, Integer)
        Get
            If _ModuleListColWidths Is Nothing Then
                _ModuleListColWidths = New SortedList(Of Long, Integer)
            End If
            Return _ModuleListColWidths
        End Get
        Set(ByVal value As SortedList(Of Long, Integer))
            _ModuleListColWidths = value
        End Set
    End Property
    Public Property ImplantGroups() As SortedList(Of String, ImplantCollection)
        Get
            Return _ImplantGroups
        End Get
        Set(ByVal value As SortedList(Of String, ImplantCollection))
            _ImplantGroups = value
        End Set
    End Property
    Public Property SlotNameWidth() As Integer
        Get
            Return _SlotNameWidth
        End Get
        Set(ByVal value As Integer)
            _SlotNameWidth = value
        End Set
    End Property
    Public Property StorageBayHeight() As Integer
        Get
            Return _StorageBayHeight
        End Get
        Set(ByVal value As Integer)
            _StorageBayHeight = value
        End Set
    End Property
    Public Property UseLastPilot() As Boolean
        Get
            Return _UseLastPilot
        End Get
        Set(ByVal value As Boolean)
            _UseLastPilot = value
        End Set
    End Property
    Public Property IncludeAmmoReloadTime() As Boolean
        Get
            Return _IncludeAmmoReloadTime
        End Get
        Set(ByVal value As Boolean)
            _IncludeAmmoReloadTime = value
        End Set
    End Property
    Public Property IncludeCapReloadTime() As Boolean
        Get
            Return _IncludeCapReloadTime
        End Get
        Set(ByVal value As Boolean)
            _IncludeCapReloadTime = value
        End Set
    End Property
    Public Property MissileRangeConstant() As Double
        Get
            Return _MissileRangeConstant
        End Get
        Set(ByVal value As Double)
            _MissileRangeConstant = value
        End Set
    End Property
    Public Property ModSplitterWidth() As Integer
        Get
            Return _ModSplitterWidth
        End Get
        Set(ByVal value As Integer)
            _ModSplitterWidth = value
        End Set
    End Property
    Public Property ShipSplitterWidth() As Integer
        Get
            Return _ShipSplitterWidth
        End Get
        Set(ByVal value As Integer)
            _ShipSplitterWidth = value
        End Set
    End Property
    Public Property ModPanelWidth() As Integer
        Get
            Return _ModPanelWidth
        End Get
        Set(ByVal value As Integer)
            _ModPanelWidth = value
        End Set
    End Property
    Public Property ShipPanelWidth() As Integer
        Get
            Return _ShipPanelWidth
        End Get
        Set(ByVal value As Integer)
            _ShipPanelWidth = value
        End Set
    End Property
    Public Property MruModules() As ArrayList
        Get
            Return _MruModules
        End Get
        Set(ByVal value As ArrayList)
            _MruModules = value
        End Set
    End Property
    Public Property MruLimit() As Integer
        Get
            Return _MruLimit
        End Get
        Set(ByVal value As Integer)
            _MruLimit = value
        End Set
    End Property
    Public Property Favourites() As ArrayList
        Get
            Return _Favourites
        End Get
        Set(ByVal value As ArrayList)
            _Favourites = value
        End Set
    End Property
    Public Property UserSlotColumns() As List(Of UserSlotColumn)
        Get
            Return _UserSlotColumns
        End Get
        Set(ByVal value As List(Of UserSlotColumn))
            _UserSlotColumns = value
        End Set
    End Property
    Public Property StandardSlotColumns() As List(Of UserSlotColumn)
        Get
            Return _StandardSlotColumns
        End Get
        Set(ByVal value As List(Of UserSlotColumn))
            _StandardSlotColumns = value
        End Set
    End Property
    Public Property ShieldRechargeConstant() As Double
        Get
            Return _ShieldRechargeConstant
        End Get
        Set(ByVal value As Double)
            _ShieldRechargeConstant = value
        End Set
    End Property
    Public Property CapRechargeConstant() As Double
        Get
            Return _CapRechargeConstant
        End Get
        Set(ByVal value As Double)
            _CapRechargeConstant = value
        End Set
    End Property
    Public Property CloseInfoPanel() As Boolean
        Get
            Return _CloseInfoPanel
        End Get
        Set(ByVal value As Boolean)
            _CloseInfoPanel = value
        End Set
    End Property
    Public Property ShowPerformanceData() As Boolean
        Get
            Return _ShowPerformanceData
        End Get
        Set(ByVal value As Boolean)
            _ShowPerformanceData = value
        End Set
    End Property
    Public Property OpenFittingList() As ArrayList
        Get
            Return _OpenFittingList
        End Get
        Set(ByVal value As ArrayList)
            _OpenFittingList = value
        End Set
    End Property
    Public Property AutoUpdateHQFSkills() As Boolean
        Get
            Return _AutoUpdateHQFSkills
        End Get
        Set(ByVal value As Boolean)
            _AutoUpdateHQFSkills = value
        End Set
    End Property
    Public Property ModuleFilter() As Integer
        Get
            Return _ModuleFilter
        End Get
        Set(ByVal value As Integer)
            _ModuleFilter = value
        End Set
    End Property
    Public Property LastPriceUpdate() As DateTime
        Get
            Return _LastPriceUpdate
        End Get
        Set(ByVal value As DateTime)
            _LastPriceUpdate = value
        End Set
    End Property
    Public Property RestoreLastSession() As Boolean
        Get
            Return _RestoreLastSession
        End Get
        Set(ByVal value As Boolean)
            _RestoreLastSession = value
        End Set
    End Property
    Public Property DefaultPilot() As String
        Get
            Return _DefaultPilot
        End Get
        Set(ByVal value As String)
            _DefaultPilot = value
        End Set
    End Property
    Public Property HiSlotColour() As Long
        Get
            Return _HiSlotColour
        End Get
        Set(ByVal value As Long)
            _HiSlotColour = value
        End Set
    End Property
    Public Property MidSlotColour() As Long
        Get
            Return _MidSlotColour
        End Get
        Set(ByVal value As Long)
            _MidSlotColour = value
        End Set
    End Property
    Public Property LowSlotColour() As Long
        Get
            Return _LowSlotColour
        End Get
        Set(ByVal value As Long)
            _LowSlotColour = value
        End Set
    End Property
    Public Property RigSlotColour() As Long
        Get
            Return _RigSlotColour
        End Get
        Set(ByVal value As Long)
            _RigSlotColour = value
        End Set
    End Property
    Public Property SubSlotColour() As Long
        Get
            Return _SubSlotColour
        End Get
        Set(ByVal value As Long)
            _SubSlotColour = value
        End Set
    End Property

#End Region

    Public Sub SaveHQFSettings()

        ' Create a JSON string for writing
        Dim json As String = JsonConvert.SerializeObject(HQFSettings, Formatting.Indented)

        ' Write the JSON version of the settings
        Try
            Using s As New StreamWriter(Path.Combine(HQFFolder, "HQFSettings.json"), False)
                s.Write(json)
                s.Flush()
            End Using
        Catch e As Exception
        End Try

    End Sub
    Public Function LoadHQFSettings() As Boolean

        ' Initialise the standard slot columns
        Call InitialiseSlotColumns()

        If My.Computer.FileSystem.FileExists(Path.Combine(HQFFolder, "HQFSettings.json")) = True Then
            Try
                Using s As New StreamReader(Path.Combine(HQFFolder, "HQFSettings.json"))
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
            For Each stdCol As UserSlotColumn In _StandardSlotColumns
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
        _StandardSlotColumns.Clear()
        _StandardSlotColumns.Add(New UserSlotColumn("Charge", "Module Charge", 150, True))
        _StandardSlotColumns.Add(New UserSlotColumn("CPU", "CPU", 75, True))
        _StandardSlotColumns.Add(New UserSlotColumn("PG", "PG", 75, True))
        _StandardSlotColumns.Add(New UserSlotColumn("Calib", "Calibration", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("Price", "Price", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("ActCost", "Activation Cost", 75, True))
        _StandardSlotColumns.Add(New UserSlotColumn("ActTime", "Activation Time", 75, True))
        _StandardSlotColumns.Add(New UserSlotColumn("CapRate", "Cap Usage Rate", 75, True))
        _StandardSlotColumns.Add(New UserSlotColumn("OptRange", "Optimal Range", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("ROF", "ROF", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("Damage", "Damage", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("DPS", "DPS", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("Falloff", "Falloff", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("Tracking", "Tracking", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("ExpRad", "Explosion Radius", 75, False))
        _StandardSlotColumns.Add(New UserSlotColumn("ExpVel", "Explosion Velocity", 75, False))
    End Sub

End Class

<Serializable()> Public Class UserSlotColumn
    ' ReSharper disable InconsistentNaming - for MS serialization compatability
    Dim cName As String = ""
    Dim cDescription As String = ""
    Dim cWidth As Integer = 75
    Dim cActive As Boolean = False
    ' ReSharper restore InconsistentNaming

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

    Public Sub New(ByVal columnName As String, ByVal desc As String, ByVal columnWidth As Integer, ByVal isActive As Boolean)
        cName = columnName
        cDescription = desc
        cWidth = columnWidth
        cActive = IsActive
    End Sub

End Class
