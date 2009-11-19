' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2009  Lee Vessey
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
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Windows.Forms
Imports System.Web
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Reflection
Imports System.Diagnostics

<Serializable()> Public Class EveSettings
    Private cAccounts As New Collection
    Private cPilots As New Collection
    Private cPlugins As New SortedList
    Private cFTPAccounts As New Collection
    Private cIGBPort As Integer = 26001
    Private cIGBAutoStart As Boolean = True
    Private cAutoStart As Boolean = False
    Private cAutoHide As Boolean = True
    Private cAutoMinimise As Boolean = False
    Private cAutoCheck As Boolean = False
    Private cMinimiseExit As Boolean = False
    Private cStartupPilot As String = ""
    Private cStartupView As String = ""
    Private cEveFolder(4) As String
    Private cBackupAuto As Boolean = False
    Private cBackupStart As Date = Now
    Private cBackupFreq As Integer = 1
    Private cBackupLast As Date = CDate("01/01/1999")
    Private cBackupLastResult As Integer = 0
    Private cQColumns(16, 1) As String
    Private cQColumnsSet As Boolean = False
    Private cDBFormat As Integer = -1
    Private cDBName As String = "EveHQ"
    Private cDBFilename As String = ""
    Private cDBServer As String = ""
    Private cDBUsername As String = ""
    Private cDBPassword As String = ""
    Private cDBSQLSecurity As Boolean = False
    Private cProxyRequired As Boolean = False
    Private cProxyServer As String = ""
    Private cProxyPort As Integer = 0
    Private cProxyUsername As String = ""
    Private cProxyPassword As String = ""
    Private cProxyUseDefault As Boolean = True
    Private cEncryptSettings As Boolean = False
    Private cEnableEveStatus As Boolean = False
    Private cServerOffset As Integer = 0
    Private cShutdownNotify As Boolean = False
    Private cShutdownNotifyPeriod As Integer = 8
    Private cNotifyToolTip As Boolean = False
    Private cNotifyDialog As Boolean = False
    Private cNotifyEMail As Boolean = False
    Private cNotifySound As Boolean = False
    Private cNotifyNow As Boolean = False
    Private cNotifyEarly As Boolean = False
    Private cNotifySoundFile As String
    Private cNotifyOffset As Integer = 0
    Private cEmailServer As String = ""
    Private cEmailPort As Integer = 25
    Private cEmailAddress As String = ""
    Private cUseSMTPAuth As Boolean = False
    Private cEmailUsername As String = ""
    Private cEmailPassword As String = ""
    Private cContinueTraining As Boolean = False
    Private cColorHighlightPilotTraining As String = ""
    Private cColorHighlightQueueTraining As String = ""
    Private cColorHighlightQueuePreReq As String = ""
    Private cMDITabStyle As Integer = 0
    Private cIsPreReqColor As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cHasPreReqColor As Long = System.Drawing.Color.White.ToArgb
    Private cBothPreReqColor As Long = System.Drawing.Color.White.ToArgb
    Private cDTClashColor As Long = System.Drawing.Color.Red.ToArgb
    Private cReadySkillColor As Long = System.Drawing.Color.White.ToArgb
    Private cPartialTrainColor As Long = System.Drawing.Color.White.ToArgb
    Private cDeleteSkills As Boolean = False
    Private cMainFormPosition(4) As Integer
    Private cAutoAPI As Boolean = False
    Private cActivateG15 As Boolean = False
    Private cCycleG15Pilots As Boolean = False
    Private cCycleG15Time As Integer = 15
    Private cEveFolderLUA(4) As Boolean
    Private cLastFactionPriceUpdate As Date
    Private cLastMarketPriceUpdate As Date
    Private cPanelBackgroundColor As Long = System.Drawing.Color.Navy.ToArgb
    Private cPanelOutlineColor As Long = System.Drawing.Color.SteelBlue.ToArgb
    Private cPanelTopLeftColor As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cPanelBottomRightColor As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cPanelLeftColor As Long = System.Drawing.Color.RoyalBlue.ToArgb
    Private cPanelRightColor As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cPanelTextColor As Long = System.Drawing.Color.Black.ToArgb
    Private cPanelHighlightColor As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cPilotStandardSkillColor As Long = System.Drawing.Color.White.ToArgb
    Private cPilotLevel5SkillColor As Long = System.Drawing.Color.Thistle.ToArgb
    Private cPilotPartTrainedSkillColor As Long = System.Drawing.Color.Gold.ToArgb
    Private cPilotCurrentTrainSkillColor As Long = System.Drawing.Color.LimeGreen.ToArgb
    Private cEveFolderLabel(4) As String
    Private cAPIRSPort As Integer = 26002
    Private cAPIRSAutoStart As Boolean = False
    Private cCCPAPIServerAddress As String = "http://api.eve-online.com"
    Private cAPIRSAddress As String = ""
    Private cUseAPIRS As Boolean = False
    Private cUseCCPAPIBackup As Boolean = False
    Private cWantedList As New SortedList
    Private cUpdateURL As String = "http://www.evehq.net/update/"
    Private cOmitCurrentSkill As Boolean = False
    Private cUseAppDirectoryForDB As Boolean = False
    Private cAPIFileExtension As String = "aspx"
    Private cECMDefaultLocation As String = ""
    Private cTaskbarIconMode As Integer = 0 '0=simple, 1=enhanced
    Private cErrorReportingEnabled As Boolean = False
    Private cErrorReportingName As String = ""
    Private cErrorReportingEmail As String = ""
    Private cPilotGroupBackgroundColor As Long = System.Drawing.Color.DimGray.ToArgb
    Private cPilotGroupTextColor As Long = System.Drawing.Color.White.ToArgb
    Private cPilotSkillTextColor As Long = System.Drawing.Color.Black.ToArgb
    Private cPilotSkillHighlightColor As Long = System.Drawing.Color.DodgerBlue.ToArgb
    Private cDBTimeout As Integer = 30
    Private cDBDataFilename As String = ""
    Private cDBDataName As String = ""
    Private cIgnoreSellOrders As Boolean = False
    Private cIgnoreBuyOrders As Boolean = True
    Private cIgnoreSellOrderLimit As Double = 1000
    Private cIgnoreBuyOrderLimit As Double = 1
    Private cMarketRegionList As New ArrayList
    Private cPriceCriteria(11) As Boolean
    Private cEnableMarketLogWatcher As Boolean = False
    Private cEnableMarketLogWatcherAtStartup As Boolean = False
    Private cMarketLogToolTipConfirm As Boolean = False
    Private cMarketLogPopupConfirm As Boolean = False
    Private cMarketLogUpdatePrice As Boolean = False
    Private cMarketLogUpdateData As Boolean = False
    Private cShowCompletedSkills As Boolean = False
    Private cMDITabPosition As String = "Top"
    Private cToolbarPosition As String = "Left"
    Private cTrainingBarPosition As String = "Bottom"
    Private cDisableAutoWebConnections As Boolean = False
    Private cDisableVisualStyles As Boolean = False
    Private cCSVSeparatorChar As String = ","
    Private cDBCBorderColor As Long = System.Drawing.Color.Black.ToArgb
    Private cDBCMainColor1 As Long = System.Drawing.Color.White.ToArgb
    Private cDBCMainColor2 As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cDBCHeadColor1 As Long = System.Drawing.Color.DimGray.ToArgb
    Private cDBCHeadColor2 As Long = System.Drawing.Color.LightGray.ToArgb
    Private cDBColor As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cDashboardConfiguration As New ArrayList
    Private cDBTicker As Boolean = False
    Private cDBTickerLocation As String = "Bottom"
    Private cStandardQueueColumns As New ArrayList
    Private cUserQueueColumns As New ArrayList
    Private cEmailSenderAddress As String = "notifications@evehq.net"
    Private cIBShowAllItems As Boolean = False
    Private cEveHQBackupStart As Date = Now
    Private cEveHQBackupFreq As Integer = 1
    Private cEveHQBackupLast As Date = CDate("01/01/1999")
    Private cEveHQBackupLastResult As Integer = 0
    Private cEveHQBackupMode As Integer = 0
    Private cEveHQBackupWarnFreq As Integer = 1

    Public Property EveHQBackupWarnFreq() As Integer
        Get
            Return cEveHQBackupWarnFreq
        End Get
        Set(ByVal value As Integer)
            cEveHQBackupWarnFreq = value
        End Set
    End Property
    Public Property EveHQBackupMode() As Integer
        Get
            Return cEveHQBackupMode
        End Get
        Set(ByVal value As Integer)
            cEveHQBackupMode = value
        End Set
    End Property
    Public Property EveHQBackupStart() As Date
        Get
            Return cEveHQBackupStart
        End Get
        Set(ByVal value As Date)
            cEveHQBackupStart = value
        End Set
    End Property
    Public Property EveHQBackupFreq() As Integer
        Get
            Return cEveHQBackupFreq
        End Get
        Set(ByVal value As Integer)
            cEveHQBackupFreq = value
        End Set
    End Property
    Public Property EveHQBackupLast() As Date
        Get
            Return cEveHQBackupLast
        End Get
        Set(ByVal value As Date)
            cEveHQBackupLast = value
        End Set
    End Property
    Public Property EveHQBackupLastResult() As Integer
        Get
            Return cEveHQBackupLastResult
        End Get
        Set(ByVal value As Integer)
            cEveHQBackupLastResult = value
        End Set
    End Property
    Public Property IBShowAllItems() As Boolean
        Get
            Return cIBShowAllItems
        End Get
        Set(ByVal value As Boolean)
            cIBShowAllItems = value
        End Set
    End Property
    Public Property EmailSenderAddress() As String
        Get
            If cEmailSenderAddress Is Nothing Then
                cEmailSenderAddress = "notifications@evehq.net"
            End If
            Return cEmailSenderAddress
        End Get
        Set(ByVal value As String)
            If value IsNot Nothing Then
                cEmailSenderAddress = value
            End If
        End Set
    End Property
    Public Property UserQueueColumns() As ArrayList
        Get
            If cUserQueueColumns Is Nothing Then
                cUserQueueColumns = New ArrayList
            End If
            Return cUserQueueColumns
        End Get
        Set(ByVal value As ArrayList)
            cUserQueueColumns = value
        End Set
    End Property
    Public Property StandardQueueColumns() As ArrayList
        Get
            If cStandardQueueColumns Is Nothing Then
                cStandardQueueColumns = New ArrayList
            End If
            Return cStandardQueueColumns
        End Get
        Set(ByVal value As ArrayList)
            cStandardQueueColumns = value
        End Set
    End Property
    Public Property DBTickerLocation() As String
        Get
            Return cDBTickerLocation
        End Get
        Set(ByVal value As String)
            cDBTickerLocation = value
        End Set
    End Property
    Public Property DBTicker() As Boolean
        Get
            Return cDBTicker
        End Get
        Set(ByVal value As Boolean)
            cDBTicker = value
        End Set
    End Property
    Public Property DashboardConfiguration() As ArrayList
        Get
            If cDashboardConfiguration Is Nothing Then
                cDashboardConfiguration = New ArrayList
            End If
            Return cDashboardConfiguration
        End Get
        Set(ByVal value As ArrayList)
            cDashboardConfiguration = value
        End Set
    End Property
    Public Property DBColor() As Long
        Get
            Return cDBColor
        End Get
        Set(ByVal value As Long)
            cDBColor = value
        End Set
    End Property
    Public Property DBCHeadColor2() As Long
        Get
            Return cDBCHeadColor2
        End Get
        Set(ByVal value As Long)
            cDBCHeadColor2 = value
        End Set
    End Property
    Public Property DBCHeadColor1() As Long
        Get
            Return cDBCHeadColor1
        End Get
        Set(ByVal value As Long)
            cDBCHeadColor1 = value
        End Set
    End Property
    Public Property DBCMainColor2() As Long
        Get
            Return cDBCMainColor2
        End Get
        Set(ByVal value As Long)
            cDBCMainColor2 = value
        End Set
    End Property
    Public Property DBCMainColor1() As Long
        Get
            Return cDBCMainColor1
        End Get
        Set(ByVal value As Long)
            cDBCMainColor1 = value
        End Set
    End Property
    Public Property DBCBorderColor() As Long
        Get
            Return cDBCBorderColor
        End Get
        Set(ByVal value As Long)
            cDBCBorderColor = value
        End Set
    End Property
    Public Property CSVSeparatorChar() As String
        Get
            If cCSVSeparatorChar Is Nothing Then
                cCSVSeparatorChar = ","
            End If
            Return cCSVSeparatorChar
        End Get
        Set(ByVal value As String)
            cCSVSeparatorChar = value
        End Set
    End Property
    Public Property DisableVisualStyles() As Boolean
        Get
            Return cDisableVisualStyles
        End Get
        Set(ByVal value As Boolean)
            cDisableVisualStyles = value
        End Set
    End Property
    Public Property DisableAutoWebConnections() As Boolean
        Get
            Return cDisableAutoWebConnections
        End Get
        Set(ByVal value As Boolean)
            cDisableAutoWebConnections = value
        End Set
    End Property
    Public Property TrainingBarPosition() As String
        Get
            Return cTrainingBarPosition
        End Get
        Set(ByVal value As String)
            cTrainingBarPosition = value
        End Set
    End Property
    Public Property ToolbarPosition() As String
        Get
            Return cToolbarPosition
        End Get
        Set(ByVal value As String)
            cToolbarPosition = value
        End Set
    End Property
    Public Property MDITabPosition() As String
        Get
            Return cMDITabPosition
        End Get
        Set(ByVal value As String)
            cMDITabPosition = value
        End Set
    End Property
    Public Property ShowCompletedSkills() As Boolean
        Get
            Return cShowCompletedSkills
        End Get
        Set(ByVal value As Boolean)
            cShowCompletedSkills = value
        End Set
    End Property
    Public Property MarketRegionList() As ArrayList
        Get
            If cMarketRegionList Is Nothing Then
                cMarketRegionList = New ArrayList
            End If
            Return cMarketRegionList
        End Get
        Set(ByVal value As ArrayList)
            cMarketRegionList = value
        End Set
    End Property
    Public Property IgnoreBuyOrderLimit() As Double
        Get
            Return cIgnoreBuyOrderLimit
        End Get
        Set(ByVal value As Double)
            cIgnoreBuyOrderLimit = value
        End Set
    End Property
    Public Property IgnoreSellOrderLimit() As Double
        Get
            Return cIgnoreSellOrderLimit
        End Get
        Set(ByVal value As Double)
            cIgnoreSellOrderLimit = value
        End Set
    End Property
    Public Property PriceCriteria(ByVal index As Integer) As Boolean
        Get
            If cPriceCriteria Is Nothing Then
                ReDim cPriceCriteria(11)
            End If
            Return cPriceCriteria(index)
        End Get
        Set(ByVal value As Boolean)
            cPriceCriteria(index) = value
        End Set
    End Property
    Public Property MarketLogUpdateData() As Boolean
        Get
            Return cMarketLogUpdateData
        End Get
        Set(ByVal value As Boolean)
            cMarketLogUpdateData = value
        End Set
    End Property
    Public Property MarketLogUpdatePrice() As Boolean
        Get
            Return cMarketLogUpdatePrice
        End Get
        Set(ByVal value As Boolean)
            cMarketLogUpdatePrice = value
        End Set
    End Property
    Public Property MarketLogPopupConfirm() As Boolean
        Get
            Return cMarketLogPopupConfirm
        End Get
        Set(ByVal value As Boolean)
            cMarketLogPopupConfirm = value
        End Set
    End Property
    Public Property MarketLogToolTipConfirm() As Boolean
        Get
            Return cMarketLogToolTipConfirm
        End Get
        Set(ByVal value As Boolean)
            cMarketLogToolTipConfirm = value
        End Set
    End Property
    Public Property EnableMarketLogWatcherAtStartup() As Boolean
        Get
            Return cEnableMarketLogWatcherAtStartup
        End Get
        Set(ByVal value As Boolean)
            cEnableMarketLogWatcherAtStartup = value
        End Set
    End Property
    Public Property EnableMarketLogWatcher() As Boolean
        Get
            Return cEnableMarketLogWatcher
        End Get
        Set(ByVal value As Boolean)
            cEnableMarketLogWatcher = value
        End Set
    End Property
    Public Property IgnoreBuyOrders() As Boolean
        Get
            Return cIgnoreBuyOrders
        End Get
        Set(ByVal value As Boolean)
            cIgnoreBuyOrders = value
        End Set
    End Property
    Public Property IgnoreSellOrders() As Boolean
        Get
            Return cIgnoreSellOrders
        End Get
        Set(ByVal value As Boolean)
            cIgnoreSellOrders = value
        End Set
    End Property
    Public Property DBDataName() As String
        Get
            Return cDBDataName
        End Get
        Set(ByVal value As String)
            cDBDataName = value
        End Set
    End Property
    Public Property DBDataFilename() As String
        Get
            Return cDBDataFilename
        End Get
        Set(ByVal value As String)
            cDBDataFilename = value
        End Set
    End Property
    Public Property DBTimeout() As Integer
        Get
            Return cDBTimeout
        End Get
        Set(ByVal value As Integer)
            cDBTimeout = value
        End Set
    End Property
    Public Property PilotSkillHighlightColor() As Long
        Get
            Return cPilotSkillHighlightColor
        End Get
        Set(ByVal value As Long)
            cPilotSkillHighlightColor = value
        End Set
    End Property
    Public Property PilotSkillTextColor() As Long
        Get
            Return cPilotSkillTextColor
        End Get
        Set(ByVal value As Long)
            cPilotSkillTextColor = value
        End Set
    End Property
    Public Property PilotGroupTextColor() As Long
        Get
            Return cPilotGroupTextColor
        End Get
        Set(ByVal value As Long)
            cPilotGroupTextColor = value
        End Set
    End Property
    Public Property PilotGroupBackgroundColor() As Long
        Get
            Return cPilotGroupBackgroundColor
        End Get
        Set(ByVal value As Long)
            cPilotGroupBackgroundColor = value
        End Set
    End Property
    Public Property ErrorReportingEmail() As String
        Get
            Return cErrorReportingEmail
        End Get
        Set(ByVal value As String)
            cErrorReportingEmail = value
        End Set
    End Property
    Public Property ErrorReportingName() As String
        Get
            Return cErrorReportingName
        End Get
        Set(ByVal value As String)
            cErrorReportingName = value
        End Set
    End Property
    Public Property ErrorReportingEnabled() As Boolean
        Get
            Return cErrorReportingEnabled
        End Get
        Set(ByVal value As Boolean)
            cErrorReportingEnabled = value
        End Set
    End Property
    Public Property TaskbarIconMode() As Integer
        Get
            Return cTaskbarIconMode
        End Get
        Set(ByVal value As Integer)
            cTaskbarIconMode = value
        End Set
    End Property
    Public Property ECMDefaultLocation() As String
        Get
            Return cECMDefaultLocation
        End Get
        Set(ByVal value As String)
            cECMDefaultLocation = value
        End Set
    End Property
    Public Property APIFileExtension() As String
        Get
            Return cAPIFileExtension
        End Get
        Set(ByVal value As String)
            cAPIFileExtension = value
        End Set
    End Property
    Public Property UseAppDirectoryForDB() As Boolean
        Get
            Return cUseAppDirectoryForDB
        End Get
        Set(ByVal value As Boolean)
            cUseAppDirectoryForDB = value
        End Set
    End Property
    Public Property OmitCurrentSkill() As Boolean
        Get
            Return cOmitCurrentSkill
        End Get
        Set(ByVal value As Boolean)
            cOmitCurrentSkill = value
        End Set
    End Property
    Public Property UpdateURL() As String
        Get
            Return cUpdateURL
        End Get
        Set(ByVal value As String)
            cUpdateURL = value
        End Set
    End Property
    Public Property WantedList() As SortedList
        Get
            If cWantedList Is Nothing Then
                cWantedList = New SortedList
            End If
            Return cWantedList
        End Get
        Set(ByVal value As SortedList)
            cWantedList = value
        End Set
    End Property
    Public Property UseCCPAPIBackup() As Boolean
        Get
            Return cUseCCPAPIBackup
        End Get
        Set(ByVal value As Boolean)
            cUseCCPAPIBackup = value
        End Set
    End Property
    Public Property UseAPIRS() As Boolean
        Get
            Return cUseAPIRS
        End Get
        Set(ByVal value As Boolean)
            cUseAPIRS = value
        End Set
    End Property
    Public Property APIRSAddress() As String
        Get
            Return cAPIRSAddress
        End Get
        Set(ByVal value As String)
            cAPIRSAddress = value
        End Set
    End Property
    Public Property CCPAPIServerAddress() As String
        Get
            Return cCCPAPIServerAddress
        End Get
        Set(ByVal value As String)
            cCCPAPIServerAddress = value
        End Set
    End Property
    Public Property APIRSPort() As Integer
        Get
            Return cAPIRSPort
        End Get
        Set(ByVal value As Integer)
            cAPIRSPort = value
        End Set
    End Property
    Public Property APIRSAutoStart() As Boolean
        Get
            Return cAPIRSAutoStart
        End Get
        Set(ByVal value As Boolean)
            cAPIRSAutoStart = value
        End Set
    End Property
    Public Property EveFolderLabel(ByVal index As Integer) As String
        Get
            If cEveFolderLabel Is Nothing Then
                ReDim cEveFolderLabel(4)
            End If
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder Label index must be in the range 1 to 4", "Eve Folder Label Get Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return "0"
            Else
                Return cEveFolderLabel(index)
            End If
        End Get
        Set(ByVal value As String)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder Label index must be in the range 1 to 4", "Eve Folder Label Set Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cEveFolderLabel(index) = value
            End If
        End Set
    End Property
    Public Property PilotCurrentTrainSkillColor() As Long
        Get
            Return cPilotCurrentTrainSkillColor
        End Get
        Set(ByVal value As Long)
            cPilotCurrentTrainSkillColor = value
        End Set
    End Property
    Public Property PilotPartTrainedSkillColor() As Long
        Get
            Return cPilotPartTrainedSkillColor
        End Get
        Set(ByVal value As Long)
            cPilotPartTrainedSkillColor = value
        End Set
    End Property
    Public Property PilotLevel5SkillColor() As Long
        Get
            Return cPilotLevel5SkillColor
        End Get
        Set(ByVal value As Long)
            cPilotLevel5SkillColor = value
        End Set
    End Property
    Public Property PilotStandardSkillColor() As Long
        Get
            Return cPilotStandardSkillColor
        End Get
        Set(ByVal value As Long)
            cPilotStandardSkillColor = value
        End Set
    End Property
    Public Property PanelHighlightColor() As Long
        Get
            Return cPanelHighlightColor
        End Get
        Set(ByVal value As Long)
            cPanelHighlightColor = value
        End Set
    End Property
    Public Property PanelTextColor() As Long
        Get
            Return cPanelTextColor
        End Get
        Set(ByVal value As Long)
            cPanelTextColor = value
        End Set
    End Property
    Public Property PanelRightColor() As Long
        Get
            Return cPanelRightColor
        End Get
        Set(ByVal value As Long)
            cPanelRightColor = value
        End Set
    End Property
    Public Property PanelLeftColor() As Long
        Get
            Return cPanelLeftColor
        End Get
        Set(ByVal value As Long)
            cPanelLeftColor = value
        End Set
    End Property
    Public Property PanelBottomRightColor() As Long
        Get
            Return cPanelBottomRightColor
        End Get
        Set(ByVal value As Long)
            cPanelBottomRightColor = value
        End Set
    End Property
    Public Property PanelTopLeftColor() As Long
        Get
            Return cPanelTopLeftColor
        End Get
        Set(ByVal value As Long)
            cPanelTopLeftColor = value
        End Set
    End Property
    Public Property PanelOutlineColor() As Long
        Get
            Return cPanelOutlineColor
        End Get
        Set(ByVal value As Long)
            cPanelOutlineColor = value
        End Set
    End Property
    Public Property PanelBackgroundColor() As Long
        Get
            Return cPanelBackgroundColor
        End Get
        Set(ByVal value As Long)
            cPanelBackgroundColor = value
        End Set
    End Property
    Public Property LastMarketPriceUpdate() As DateTime
        Get
            Return cLastMarketPriceUpdate
        End Get
        Set(ByVal value As DateTime)
            cLastMarketPriceUpdate = value
        End Set
    End Property
    Public Property LastFactionPriceUpdate() As DateTime
        Get
            Return cLastFactionPriceUpdate
        End Get
        Set(ByVal value As DateTime)
            cLastFactionPriceUpdate = value
        End Set
    End Property
    Public Property EveFolderLUA(ByVal index As Integer) As Boolean
        Get
            If cEveFolderLUA Is Nothing Then
                ReDim cEveFolderLUA(4)
            End If
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder LUA index must be in the range 1 to 4", "Eve Folder Get Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                Return cEveFolderLUA(index)
            End If
        End Get
        Set(ByVal value As Boolean)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder LUA index must be in the range 1 to 4", "Eve Folder Set Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cEveFolderLUA(index) = value
            End If
        End Set
    End Property
    Public Property CycleG15Time() As Integer
        Get
            Return cCycleG15Time
        End Get
        Set(ByVal value As Integer)
            cCycleG15Time = value
        End Set
    End Property
    Public Property CycleG15Pilots() As Boolean
        Get
            Return cCycleG15Pilots
        End Get
        Set(ByVal value As Boolean)
            cCycleG15Pilots = value
        End Set
    End Property
    Public Property ActivateG15() As Boolean
        Get
            Return cActivateG15
        End Get
        Set(ByVal value As Boolean)
            cActivateG15 = value
        End Set
    End Property
    Public Property AutoAPI() As Boolean
        Get
            Return cAutoAPI
        End Get
        Set(ByVal value As Boolean)
            cAutoAPI = value
        End Set
    End Property
    Public Property MainFormPosition(ByVal index As Integer) As Integer
        Get
            If cMainFormPosition Is Nothing Then
                ReDim cMainFormPosition(4)
            End If
            If index < 0 Or index > 4 Then
                MessageBox.Show("Eve Main Form Position index must be in the range 0 to 4", "Eve Main Form Position Get Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return 0
            Else
                Return cMainFormPosition(index)
            End If
        End Get
        Set(ByVal value As Integer)
            If index < 0 Or index > 4 Then
                MessageBox.Show("Eve Main Form Position index must be in the range 0 to 4", "Eve Main Form Position Set Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cMainFormPosition(index) = value
            End If
        End Set
    End Property
    Public Property DeleteSkills() As Boolean
        Get
            Return cDeleteSkills
        End Get
        Set(ByVal value As Boolean)
            cDeleteSkills = value
        End Set
    End Property
    Public Property PartialTrainColor() As Long
        Get
            Return cPartialTrainColor
        End Get
        Set(ByVal value As Long)
            cPartialTrainColor = value
        End Set
    End Property
    Public Property ReadySkillColor() As Long
        Get
            Return cReadySkillColor
        End Get
        Set(ByVal value As Long)
            cReadySkillColor = value
        End Set
    End Property
    Public Property IsPreReqColor() As Long
        Get
            Return cIsPreReqColor
        End Get
        Set(ByVal value As Long)
            cIsPreReqColor = value
        End Set
    End Property
    Public Property HasPreReqColor() As Long
        Get
            Return cHasPreReqColor
        End Get
        Set(ByVal value As Long)
            cHasPreReqColor = value
        End Set
    End Property
    Public Property BothPreReqColor() As Long
        Get
            Return cBothPreReqColor
        End Get
        Set(ByVal value As Long)
            cBothPreReqColor = value
        End Set
    End Property
    Public Property DTClashColor() As Long
        Get
            Return cDTClashColor
        End Get
        Set(ByVal value As Long)
            cDTClashColor = value
        End Set
    End Property
    Public Property MDITabStyle() As Integer
        Get
            Return cMDITabStyle
        End Get
        Set(ByVal value As Integer)
            cMDITabStyle = value
        End Set
    End Property
    Public Property ColorHighlightQueuePreReq() As String
        Get
            Return cColorHighlightQueuePreReq
        End Get
        Set(ByVal value As String)
            cColorHighlightQueuePreReq = value
        End Set
    End Property
    Public Property ColorHighlightQueueTraining() As String
        Get
            Return cColorHighlightQueueTraining
        End Get
        Set(ByVal value As String)
            cColorHighlightQueueTraining = value
        End Set
    End Property
    Public Property ColorHighlightPilotTraining() As String
        Get
            Return cColorHighlightPilotTraining
        End Get
        Set(ByVal value As String)
            cColorHighlightPilotTraining = value
        End Set
    End Property
    Public Property ContinueTraining() As Boolean
        Get
            Return cContinueTraining
        End Get
        Set(ByVal value As Boolean)
            cContinueTraining = value
        End Set
    End Property
    Public Property EMailPassword() As String
        Get
            Return cEmailPassword
        End Get
        Set(ByVal value As String)
            cEmailPassword = value
        End Set
    End Property
    Public Property EMailUsername() As String
        Get
            Return cEmailUsername
        End Get
        Set(ByVal value As String)
            cEmailUsername = value
        End Set
    End Property
    Public Property UseSMTPAuth() As Boolean
        Get
            Return cUseSMTPAuth
        End Get
        Set(ByVal value As Boolean)
            cUseSMTPAuth = value
        End Set
    End Property
    Public Property EMailAddress() As String
        Get
            Return cEmailAddress
        End Get
        Set(ByVal value As String)
            cEmailAddress = value
        End Set
    End Property
    Public Property EMailPort() As Integer
        Get
            Return cEmailPort
        End Get
        Set(ByVal value As Integer)
            cEmailPort = value
        End Set
    End Property
    Public Property EMailServer() As String
        Get
            Return cEmailServer
        End Get
        Set(ByVal value As String)
            cEmailServer = value
        End Set
    End Property
    Public Property NotifySoundFile() As String
        Get
            Return cNotifySoundFile
        End Get
        Set(ByVal value As String)
            cNotifySoundFile = value
        End Set
    End Property
    Public Property NotifyOffset() As Integer
        Get
            Return cNotifyOffset
        End Get
        Set(ByVal value As Integer)
            cNotifyOffset = value
        End Set
    End Property
    Public Property NotifyEarly() As Boolean
        Get
            Return cNotifyEarly
        End Get
        Set(ByVal value As Boolean)
            cNotifyEarly = value
        End Set
    End Property
    Public Property NotifyNow() As Boolean
        Get
            Return cNotifyNow
        End Get
        Set(ByVal value As Boolean)
            cNotifyNow = value
        End Set
    End Property
    Public Property NotifySound() As Boolean
        Get
            Return cNotifySound
        End Get
        Set(ByVal value As Boolean)
            cNotifySound = value
        End Set
    End Property
    Public Property NotifyEMail() As Boolean
        Get
            Return cNotifyEMail
        End Get
        Set(ByVal value As Boolean)
            cNotifyEMail = value
        End Set
    End Property
    Public Property NotifyDialog() As Boolean
        Get
            Return cNotifyDialog
        End Get
        Set(ByVal value As Boolean)
            cNotifyDialog = value
        End Set
    End Property
    Public Property NotifyToolTip() As Boolean
        Get
            Return cNotifyToolTip
        End Get
        Set(ByVal value As Boolean)
            cNotifyToolTip = value
        End Set
    End Property
    Public Property ShutdownNotifyPeriod() As Integer
        Get
            Return cShutdownNotifyPeriod
        End Get
        Set(ByVal value As Integer)
            cShutdownNotifyPeriod = value
        End Set
    End Property
    Public Property ShutdownNotify() As Boolean
        Get
            Return cShutdownNotify
        End Get
        Set(ByVal value As Boolean)
            cShutdownNotify = value
        End Set
    End Property
    Public Property ServerOffset() As Integer
        Get
            Return cServerOffset
        End Get
        Set(ByVal value As Integer)
            cServerOffset = value
        End Set
    End Property
    Public Property EnableEveStatus() As Boolean
        Get
            Return cEnableEveStatus
        End Get
        Set(ByVal value As Boolean)
            cEnableEveStatus = value
        End Set
    End Property
    Public Property EncryptSettings() As Boolean
        Get
            Return cEncryptSettings
        End Get
        Set(ByVal value As Boolean)
            cEncryptSettings = value
        End Set
    End Property
    Public Property ProxyUseDefault() As Boolean
        Get
            Return cProxyUseDefault
        End Get
        Set(ByVal value As Boolean)
            cProxyUseDefault = value
        End Set
    End Property
    Public Property ProxyPassword() As String
        Get
            Return cProxyPassword
        End Get
        Set(ByVal value As String)
            cProxyPassword = value
        End Set
    End Property
    Public Property ProxyUsername() As String
        Get
            Return cProxyUsername
        End Get
        Set(ByVal value As String)
            cProxyUsername = value
        End Set
    End Property
    Public Property ProxyPort() As Integer
        Get
            Return cProxyPort
        End Get
        Set(ByVal value As Integer)
            cProxyPort = value
        End Set
    End Property
    Public Property ProxyServer() As String
        Get
            Return cProxyServer
        End Get
        Set(ByVal value As String)
            cProxyServer = value
        End Set
    End Property
    Public Property ProxyRequired() As Boolean
        Get
            Return cProxyRequired
        End Get
        Set(ByVal value As Boolean)
            cProxyRequired = value
        End Set
    End Property
    Public Property IGBPort() As Integer
        Get
            Return cIGBPort
        End Get
        Set(ByVal value As Integer)
            cIGBPort = value
        End Set
    End Property
    Public Property IGBAutoStart() As Boolean
        Get
            Return cIGBAutoStart
        End Get
        Set(ByVal value As Boolean)
            cIGBAutoStart = value
        End Set
    End Property
    Public Property AutoHide() As Boolean
        Get
            Return cAutoHide
        End Get
        Set(ByVal value As Boolean)
            cAutoHide = value
        End Set
    End Property
    Public Property AutoStart() As Boolean
        Get
            Return cAutoStart
        End Get
        Set(ByVal value As Boolean)
            cAutoStart = value
        End Set
    End Property
    Public Property AutoCheck() As Boolean
        Get
            Return cAutoCheck
        End Get
        Set(ByVal value As Boolean)
            cAutoCheck = value
        End Set
    End Property
    Public Property MinimiseExit() As Boolean
        Get
            Return cMinimiseExit
        End Get
        Set(ByVal value As Boolean)
            cMinimiseExit = value
        End Set
    End Property
    Public Property AutoMinimise() As Boolean
        Get
            Return cAutoMinimise
        End Get
        Set(ByVal value As Boolean)
            cAutoMinimise = value
        End Set
    End Property
    Public Property StartupPilot() As String
        Get
            Return cStartupPilot
        End Get
        Set(ByVal value As String)
            cStartupPilot = value
        End Set
    End Property
    Public Property StartupView() As String
        Get
            Return cStartupView
        End Get
        Set(ByVal value As String)
            cStartupView = value
        End Set
    End Property
    Public Property EveFolder(ByVal index As Integer) As String
        Get
            If cEveFolder Is Nothing Then
                ReDim cEveFolder(4)
            End If
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder index must be in the range 1 to 4", "Eve Folder Get Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return "0"
            Else
                Return cEveFolder(index)
            End If
        End Get
        Set(ByVal value As String)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder index must be in the range 1 to 4", "Eve Folder Set Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cEveFolder(index) = value
            End If
        End Set
    End Property
    Public Property BackupAuto() As Boolean
        Get
            Return cBackupAuto
        End Get
        Set(ByVal value As Boolean)
            cBackupAuto = value
        End Set
    End Property
    Public Property BackupStart() As Date
        Get
            Return cBackupStart
        End Get
        Set(ByVal value As Date)
            cBackupStart = value
        End Set
    End Property
    Public Property BackupFreq() As Integer
        Get
            Return cBackupFreq
        End Get
        Set(ByVal value As Integer)
            cBackupFreq = value
        End Set
    End Property
    Public Property BackupLast() As Date
        Get
            Return cBackupLast
        End Get
        Set(ByVal value As Date)
            cBackupLast = value
        End Set
    End Property
    Public Property BackupLastResult() As Integer
        Get
            Return cBackupLastResult
        End Get
        Set(ByVal value As Integer)
            cBackupLastResult = value
        End Set
    End Property
    Public Property QColumnsSet() As Boolean
        Get
            Return cQColumnsSet
        End Get
        Set(ByVal value As Boolean)
            cQColumnsSet = value
        End Set
    End Property
    Public Property QColumns(ByVal col As Integer, ByVal ref As Integer) As String
        Get
            If cQColumns Is Nothing Then
                ReDim cQColumns(16, 1)
            End If
            Return cQColumns(col, ref)
        End Get
        Set(ByVal value As String)
            cQColumns(col, ref) = value
        End Set
    End Property
    Public Property DBFormat() As Integer
        Get
            Return cDBFormat
        End Get
        Set(ByVal value As Integer)
            cDBFormat = value
        End Set
    End Property
    Public Property DBFilename() As String
        Get
            Return cDBFilename
        End Get
        Set(ByVal value As String)
            cDBFilename = value
        End Set
    End Property
    Public Property DBName() As String
        Get
            Return cDBName
        End Get
        Set(ByVal value As String)
            cDBName = value
        End Set
    End Property
    Public Property DBServer() As String
        Get
            Return cDBServer
        End Get
        Set(ByVal value As String)
            cDBServer = value
        End Set
    End Property
    Public Property DBUsername() As String
        Get
            Return cDBUsername
        End Get
        Set(ByVal value As String)
            cDBUsername = value
        End Set
    End Property
    Public Property DBPassword() As String
        Get
            Return cDBPassword
        End Get
        Set(ByVal value As String)
            cDBPassword = value
        End Set
    End Property
    Public Property DBSQLSecurity() As Boolean
        Get
            Return cDBSQLSecurity
        End Get
        Set(ByVal value As Boolean)
            cDBSQLSecurity = value
        End Set
    End Property
    Public Property Accounts() As Collection
        Get
            If cAccounts Is Nothing Then
                cAccounts = New Collection
            End If
            Return cAccounts
        End Get
        Set(ByVal value As Collection)
            cAccounts = value
        End Set
    End Property
    Public Property Plugins() As SortedList
        Get
            If cPlugins Is Nothing Then
                cPlugins = New SortedList
            End If
            Return cPlugins
        End Get
        Set(ByVal value As SortedList)
            cPlugins = value
        End Set
    End Property
    Public Property Pilots() As Collection
        Get
            If cPilots Is Nothing Then
                cPilots = New Collection
            End If
            Return cPilots
        End Get
        Set(ByVal value As Collection)
            cPilots = value
        End Set
    End Property
    Public Property FTPAccounts() As Collection
        Get
            If cFTPAccounts Is Nothing Then
                cFTPAccounts = New Collection
            End If
            Return cFTPAccounts
        End Get
        Set(ByVal value As Collection)
            cFTPAccounts = value
        End Set
    End Property

End Class

Public Class EveHQSettingsFunctions

    Public Shared Sub SaveSettings()
        Call SaveEveHQSettings()
        Call SaveTraining()
    End Sub                  'SaveSettings
    Public Shared Function LoadSettings() As Boolean
        If LoadEveHQSettings() = False Then
            Return False
            Exit Function
        End If
        Call LoadTraining()
        Return True
    End Function             'LoadSettings

    Public Shared Sub SaveTraining()
        Dim currentPilot As New EveHQ.Core.Pilot
        Dim currentQueue As New EveHQ.Core.SkillQueue
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""
        Dim tFileName As String = ""

        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If currentPilot.TrainingQueues IsNot Nothing Then
                XMLS = ("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>") & vbCrLf
                XMLS &= "<training version=""" & My.Application.Info.Version.Major.ToString & "." & My.Application.Info.Version.Minor.ToString & """>" & vbCrLf
                For Each currentQueue In currentPilot.TrainingQueues.Values
                    XMLS &= Chr(9) & "<queue name=""" & HttpUtility.HtmlEncode(currentQueue.Name) & """ ICT=""" & currentQueue.IncCurrentTraining & """ primary=""" & currentQueue.Primary & """ >"
                    Dim mySkillQueue As EveHQ.Core.SkillQueueItem
                    For Each mySkillQueue In currentQueue.Queue
                        XMLS &= Chr(9) & "<skill>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<skillID>" & mySkillQueue.Name & "</skillID>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<fromLevel>" & mySkillQueue.FromLevel & "</fromLevel>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<toLevel>" & mySkillQueue.ToLevel & "</toLevel>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<position>" & mySkillQueue.Pos & "</position>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<notes>" & HttpUtility.HtmlEncode(mySkillQueue.Notes) & "</notes>" & vbCrLf
                        XMLS &= Chr(9) & "</skill>" & vbCrLf
                    Next
                    XMLS &= Chr(9) & "</queue>"
                Next
                XMLS &= "</training>" & vbCrLf
                Try
                    XMLdoc.LoadXml(XMLS)
                    tFileName = "Q_" & currentPilot.Name & ".xml"
                    XMLdoc.Save(Path.Combine(EveHQ.Core.HQ.dataFolder, tFileName))
                Catch e As Exception
                    MessageBox.Show(e.Message, "Error Saving Training Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End Try
            End If
        Next
    End Sub

    Public Shared Sub LoadTraining()
        Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""
        Dim tFileName As String = ""

        Dim trainingList, QueueList As XmlNodeList
        Dim trainingDetails, Queuedetails As XmlNode

        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            currentPilot.ActiveQueue = New EveHQ.Core.SkillQueue
            'currentPilot.ActiveQueue.Queue.Clear()
            currentPilot.TrainingQueues = New SortedList
            currentPilot.TrainingQueues.Clear()
            currentPilot.PrimaryQueue = ""

            tFileName = "Q_" & currentPilot.Name & ".xml"
            If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.dataFolder, tFileName)) = True Then
                Try
                    XMLdoc.Load(Path.Combine(EveHQ.Core.HQ.dataFolder, tFileName))

                    ' Get the EveHQ.Core.Pilot details
                    trainingList = XMLdoc.SelectNodes("/training/skill")

                    If trainingList.Count > 0 Then
                        ' Using version prior to 1.3
                        ' Start a new SkillQueue class (using "primary" as the default name)
                        Dim newQ As New SkillQueue
                        newQ.Name = "Primary"
                        newQ.IncCurrentTraining = True
                        newQ.Primary = True
                        For Each trainingDetails In trainingList
                            Dim myskill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                            myskill.Name = trainingDetails.ChildNodes(0).InnerText
                            myskill.FromLevel = CInt(trainingDetails.ChildNodes(1).InnerText)
                            myskill.ToLevel = CInt(trainingDetails.ChildNodes(2).InnerText)
                            myskill.Pos = CInt(trainingDetails.ChildNodes(3).InnerText)
                            Dim keyName As String = myskill.Name & myskill.FromLevel & myskill.ToLevel
                            currentPilot.ActiveQueue.Queue.Add(myskill, keyName)
                        Next
                        newQ.Queue = currentPilot.ActiveQueue.Queue
                        currentPilot.PrimaryQueue = newQ.Name
                        currentPilot.TrainingQueues.Add(newQ.Name, newQ)
                    Else
                        ' Try for the post 1.3 version
                        ' Get version
                        Dim rootNode As XmlNode = XMLdoc.SelectSingleNode("/training")
                        Dim version As Double = 0
                        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
                        If rootNode.Attributes.Count > 0 Then
                            version = Double.Parse(rootNode.Attributes("version").Value, Globalization.NumberStyles.Number, culture)
                        End If
                        QueueList = XMLdoc.SelectNodes("/training/queue")
                        If QueueList.Count > 0 Then
                            For Each Queuedetails In QueueList
                                Dim newQ As New SkillQueue
                                newQ.Name = HttpUtility.HtmlDecode(Queuedetails.Attributes("name").Value)
                                newQ.IncCurrentTraining = CBool(Queuedetails.Attributes("ICT").Value)
                                newQ.Primary = CBool(Queuedetails.Attributes("primary").Value)
                                If newQ.Primary = True Then
                                    If currentPilot.PrimaryQueue <> "" Then
                                        ' There can be only one!
                                        newQ.Primary = False
                                    Else
                                        currentPilot.PrimaryQueue = newQ.Name
                                    End If
                                End If
                                trainingList = Queuedetails.SelectNodes("training/queue/skill")
                                If Queuedetails.ChildNodes.Count > 0 Then
                                    ' Using version prior to 1.3
                                    ' Start a new SkillQueue class (using "primary" as the default name)
                                    For Each trainingDetails In Queuedetails.ChildNodes
                                        Dim myskill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                                        myskill.Name = trainingDetails.ChildNodes(0).InnerText
                                        ' Adjust for the 1.9 version
                                        If version < 1.9 Then
                                            If myskill.Name = "Astrometric Triangulation" Then
                                                myskill.Name = "Astrometric Acquisition"
                                            End If
                                            If myskill.Name = "Signal Acquisition" Then
                                                myskill.Name = "Astrometric Triangulation"
                                            End If
                                        End If
                                        Try
                                            myskill.FromLevel = CInt(trainingDetails.ChildNodes(1).InnerText)
                                            myskill.ToLevel = CInt(trainingDetails.ChildNodes(2).InnerText)
                                            myskill.Pos = CInt(trainingDetails.ChildNodes(3).InnerText)
                                            myskill.Notes = HttpUtility.HtmlDecode(trainingDetails.ChildNodes(4).InnerText)
                                        Catch e As Exception
                                            ' We don't have the required info
                                        End Try
                                        Dim keyName As String = myskill.Name & myskill.FromLevel & myskill.ToLevel
                                        If newQ.Queue.Contains(keyName) = False Then
                                            If myskill.ToLevel > myskill.FromLevel Then
                                                newQ.Queue.Add(myskill, keyName)                    ' Multi queue method
                                            End If
                                        End If
                                    Next
                                End If
                                currentPilot.TrainingQueues.Add(newQ.Name, newQ)
                            Next
                        End If
                    End If

                    ' Iterate through the relevant nodes

                Catch e As Exception
                    MessageBox.Show("Error importing Training data for " & currentPilot.Name & "." & ControlChars.CrLf & "The error reported was " & e.Message, "Training Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Next
    End Sub

    Public Shared Sub ResetColumns()
        EveHQ.Core.HQ.EveHQSettings.QColumns(0, 0) = "Name" : EveHQ.Core.HQ.EveHQSettings.QColumns(0, 1) = CStr(True)
        EveHQ.Core.HQ.EveHQSettings.QColumns(1, 0) = "Curr" : EveHQ.Core.HQ.EveHQSettings.QColumns(1, 1) = CStr(True)
        EveHQ.Core.HQ.EveHQSettings.QColumns(2, 0) = "From" : EveHQ.Core.HQ.EveHQSettings.QColumns(2, 1) = CStr(True)
        EveHQ.Core.HQ.EveHQSettings.QColumns(3, 0) = "Tole" : EveHQ.Core.HQ.EveHQSettings.QColumns(3, 1) = CStr(True)
        EveHQ.Core.HQ.EveHQSettings.QColumns(4, 0) = "Perc" : EveHQ.Core.HQ.EveHQSettings.QColumns(4, 1) = CStr(True)
        EveHQ.Core.HQ.EveHQSettings.QColumns(5, 0) = "Trai" : EveHQ.Core.HQ.EveHQSettings.QColumns(5, 1) = CStr(True)
        EveHQ.Core.HQ.EveHQSettings.QColumns(6, 0) = "Date" : EveHQ.Core.HQ.EveHQSettings.QColumns(6, 1) = CStr(True)
        EveHQ.Core.HQ.EveHQSettings.QColumns(7, 0) = "Rank" : EveHQ.Core.HQ.EveHQSettings.QColumns(7, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(8, 0) = "PAtt" : EveHQ.Core.HQ.EveHQSettings.QColumns(8, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(9, 0) = "SAtt" : EveHQ.Core.HQ.EveHQSettings.QColumns(9, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(10, 0) = "SPRH" : EveHQ.Core.HQ.EveHQSettings.QColumns(10, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(11, 0) = "SPRD" : EveHQ.Core.HQ.EveHQSettings.QColumns(11, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(12, 0) = "SPRW" : EveHQ.Core.HQ.EveHQSettings.QColumns(12, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(13, 0) = "SPRM" : EveHQ.Core.HQ.EveHQSettings.QColumns(13, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(14, 0) = "SPRY" : EveHQ.Core.HQ.EveHQSettings.QColumns(14, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(15, 0) = "SPAd" : EveHQ.Core.HQ.EveHQSettings.QColumns(15, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumns(16, 0) = "SPTo" : EveHQ.Core.HQ.EveHQSettings.QColumns(16, 1) = CStr(False)
        EveHQ.Core.HQ.EveHQSettings.QColumnsSet = True
    End Sub

    Public Shared Sub SaveEveHQSettings()
        ' Write a serial version?
        Dim s As New FileStream(Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQSettings.bin"), FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.EveHQSettings)
        s.Flush()
        s.Close()
    End Sub

    Public Shared Function LoadEveHQSettings() As Boolean
        If My.Computer.FileSystem.FileExists(Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQSettings.bin")) = True Then
            Dim s As New FileStream(Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQSettings.bin"), FileMode.Open)
            Try
                Dim f As BinaryFormatter = New BinaryFormatter
                EveHQ.Core.HQ.EveHQSettings = CType(f.Deserialize(s), EveSettings)
                s.Close()
            Catch ex As Exception
                Dim msg As String = "There was an error trying to load the settings file and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "EveHQ will delete this file and re-initialise the settings. This means you will need to re-enter your API information but your skill queues and fittings should be intact and available once the API data has been downloaded." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Press OK to reset the settings." & ControlChars.CrLf
                MessageBox.Show(msg, "Invalid Settings file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Try
                    s.Close()
                    My.Computer.FileSystem.DeleteFile(Path.Combine(EveHQ.Core.HQ.appDataFolder, "EveHQSettings.bin"))
                Catch e As Exception
                    MessageBox.Show("Unable to delete the EveHQSettings.bin file. Please delete this manually before proceeding", "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Application.Exit()
                End Try
                Return False
            End Try
        Else
            Return False
        End If

        ' Set the database connection string
        ' Determine if a database format has been chosen before and set it if not
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = -1 Then
            EveHQ.Core.HQ.EveHQSettings.DBFormat = 0
            EveHQ.Core.HQ.EveHQSettings.DBFilename = Path.Combine(EveHQ.Core.HQ.appFolder, "EveHQ.mdb")
            ' Check for this file!
            Dim fileExists As Boolean = False
            Do
                If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.EveHQSettings.DBFilename) = False Then
                    Dim msg As String = "EveHQ needs a database in order to work correctly." & ControlChars.CrLf
                    msg &= "If you do not select a valid DB file, EveHQ will exit." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Would you like to select a file now?" & ControlChars.CrLf
                    Dim reply As Integer = MessageBox.Show(msg, "Database Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = Windows.Forms.DialogResult.No Then
                        Return False
                        Exit Function
                    End If
                    Dim ofd1 As New OpenFileDialog
                    With ofd1
                        .Title = "Select Access Data file"
                        .FileName = ""
                        .InitialDirectory = EveHQ.Core.HQ.appFolder
                        .Filter = "Access Data files (*.mdb)|*.mdb|All files (*.*)|*.*"
                        .FilterIndex = 1
                        .RestoreDirectory = True
                        If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                            EveHQ.Core.HQ.EveHQSettings.DBFilename = .FileName
                        End If
                    End With
                Else
                    fileExists = True
                End If
            Loop Until fileExists = True
            EveHQ.Core.HQ.EveHQSettings.DBUsername = ""
            EveHQ.Core.HQ.EveHQSettings.DBPassword = ""
        End If

        If EveHQ.Core.DataFunctions.SetEveHQConnectionString() = False Then
            Return False
        End If
        If EveHQ.Core.DataFunctions.SetEveHQDataConnectionString() = False Then
            Return False
        End If

        ' Load the skill data before attempting to load in the EveHQ.Core.Pilot skill data
        If EveHQ.Core.SkillFunctions.LoadEveSkillData() = False Then
            Return False
            Exit Function
        End If

        '  Setup queue columns etc
        Call InitialiseQueueColumns()
        Call InitialiseUserColumns()
        If EveHQ.Core.HQ.EveHQSettings.QColumns(0, 0) Is Nothing Then
            Call ResetColumns()
        End If

        ' Check Dashboard colours
        If EveHQ.Core.HQ.EveHQSettings.DBCMainColor1 = 0 And EveHQ.Core.HQ.EveHQSettings.DBCMainColor2 = 0 And EveHQ.Core.HQ.EveHQSettings.DBColor = 0 Then
            EveHQ.Core.HQ.EveHQSettings.DBCBorderColor = System.Drawing.Color.Black.ToArgb
            EveHQ.Core.HQ.EveHQSettings.DBCMainColor1 = System.Drawing.Color.White.ToArgb
            EveHQ.Core.HQ.EveHQSettings.DBCMainColor2 = System.Drawing.Color.LightSteelBlue.ToArgb
            EveHQ.Core.HQ.EveHQSettings.DBCHeadColor1 = System.Drawing.Color.DimGray.ToArgb
            EveHQ.Core.HQ.EveHQSettings.DBCHeadColor2 = System.Drawing.Color.LightGray.ToArgb
            EveHQ.Core.HQ.EveHQSettings.DBColor = System.Drawing.Color.LightSteelBlue.ToArgb
        End If

        Return True

    End Function

    Public Shared Sub InitialiseQueueColumns()
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Clear()
        Dim newItem As New ListViewItem
        'newItem = New ListViewItem
        'newItem.Name = "Name"
        'newItem.Text = "Skill Name"
        'newItem.Checked = True
        'EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Current"
        newItem.Text = "Cur Lvl"
        newItem.Checked = True
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "From"
        newItem.Text = "From Lvl"
        newItem.Checked = True
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "To"
        newItem.Text = "To Lvl"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Percent"
        newItem.Text = "%"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "TrainTime"
        newItem.Text = "Training Time"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "DateEnded"
        newItem.Text = "Date Completed"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Rank"
        newItem.Text = "Rank"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "PAtt"
        newItem.Text = "Pri Att"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SAtt"
        newItem.Text = "Sec Att"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPHour"
        newItem.Text = "SP /hour"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPDay"
        newItem.Text = "SP /day"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPWeek"
        newItem.Text = "SP /week"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPMonth"
        newItem.Text = "SP /month"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPYear"
        newItem.Text = "SP /year"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPAdded"
        newItem.Text = "SP Added"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPTotal"
        newItem.Text = "SP Total"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Notes"
        newItem.Text = "Notes"
        newItem.Checked = False
        EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
        'newItem = New ListViewItem
        'newItem.Name = "Priority"
        'newItem.Text = "Priority"
        'newItem.Checked = False
        'EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Add(newItem)
    End Sub
    Public Shared Sub InitialiseUserColumns()
        If EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Count = 0 Then
            ' Add preset items
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Add("Current1")
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Add("From1")
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Add("To1")
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Add("Percent1")
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Add("TrainTime1")
        End If
        ' Check if the standard columns have changed and we need to add columns
        If EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Count <> EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns.Count Then
            For Each slotItem As ListViewItem In EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns
                If EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Contains(slotItem.Name & "0") = False And EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Contains(slotItem.Name & "1") = False Then
                    EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Add(slotItem.Name & "0")
                End If
            Next
        End If

    End Sub
End Class



