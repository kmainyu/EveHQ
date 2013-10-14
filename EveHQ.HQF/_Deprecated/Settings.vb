' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2012  EveHQ Development Team
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

<Serializable()> Public Class Settings

    Public Shared HQFSettings As New Settings
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
    Private cImplantGroups As New SortedList(Of String, ImplantGroup)
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
    Public Property ImplantGroups() As SortedList(Of String, ImplantGroup)
        Get
            Return cImplantGroups
        End Get
        Set(ByVal value As SortedList(Of String, ImplantGroup))
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

End Class
