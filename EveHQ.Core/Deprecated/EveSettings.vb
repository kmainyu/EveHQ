'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Drawing
Imports DevComponents.DotNetBar
Imports EveHQ.Market
Imports EveHQ.Common.Extensions
Imports System.Windows.Forms

' ReSharper disable InconsistentNaming
' ReSharper disable once CheckNamespace
<Serializable()>
Public Class EveSettings
    Private Const OfficalApiLocation As String = "https://api.eveonline.com"
    
    Private cAccounts As New Collection
    Private cPilots As New Collection
    Private cPlugins As New SortedList
    Private cIGBPort As Integer = 26001
    Private cIGBAutoStart As Boolean = False
    Private cIGBFullMode As Boolean = False
    Private cIGBAllowedData As New SortedList(Of String, Boolean)
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
    Private cQColumns(20, 1) As String
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
    Private cProxyUseBasic As Boolean = False
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
    Private cUseSSL As Boolean = False
    Private cEmailUsername As String = ""
    Private cEmailPassword As String = ""
    Private cContinueTraining As Boolean = False
    Private cColorHighlightPilotTraining As String = ""
    Private cColorHighlightQueueTraining As String = ""
    Private cColorHighlightQueuePreReq As String = ""
    Private cIsPreReqColor As Long = Color.LightSteelBlue.ToArgb
    Private cHasPreReqColor As Long = Color.White.ToArgb
    Private cBothPreReqColor As Long = Color.White.ToArgb
    Private cDTClashColor As Long = Color.Red.ToArgb
    Private cReadySkillColor As Long = Color.White.ToArgb
    Private cPartialTrainColor As Long = Color.White.ToArgb
    Private cDeleteSkills As Boolean = False
    Private cMainFormPosition(4) As Integer
    Private cAutoAPI As Boolean = False
    Private cActivateG15 As Boolean = False
    Private cCycleG15Pilots As Boolean = False
    Private cCycleG15Time As Integer = 15
    Private cEveFolderLUA(4) As Boolean
    Private cLastFactionPriceUpdate As Date
    Private cLastMarketPriceUpdate As Date
    Private cPanelBackgroundColor As Long = Color.Navy.ToArgb
    Private cPanelOutlineColor As Long = Color.SteelBlue.ToArgb
    Private cPanelTopLeftColor As Long = Color.LightSteelBlue.ToArgb
    Private cPanelBottomRightColor As Long = Color.LightSteelBlue.ToArgb
    Private cPanelLeftColor As Long = Color.RoyalBlue.ToArgb
    Private cPanelRightColor As Long = Color.LightSteelBlue.ToArgb
    Private cPanelTextColor As Long = Color.Black.ToArgb
    Private cPanelHighlightColor As Long = Color.LightSteelBlue.ToArgb
    Private cPilotStandardSkillColor As Long = Color.White.ToArgb
    Private cPilotLevel5SkillColor As Long = Color.Thistle.ToArgb
    Private cPilotPartTrainedSkillColor As Long = Color.Gold.ToArgb
    Private cPilotCurrentTrainSkillColor As Long = Color.LimeGreen.ToArgb
    Private cEveFolderLabel(4) As String
    Private cCCPAPIServerAddress As String = OfficalApiLocation
    Private cAPIRSAddress As String = ""
    Private cUseAPIRS As Boolean = False
    Private cUseCCPAPIBackup As Boolean = False
    Private cUpdateURL As String = "http://evehq.net/update/"
    Private cOmitCurrentSkill As Boolean = False
    Private cUseAppDirectoryForDB As Boolean = False
    Private cAPIFileExtension As String = "aspx"
    Private cECMDefaultLocation As String = ""
    Private cTaskbarIconMode As Integer = 0 '0=simple, 1=enhanced
    Private cErrorReportingEnabled As Boolean = False
    Private cErrorReportingName As String = ""
    Private cErrorReportingEmail As String = ""
    Private cPilotGroupBackgroundColor As Long = Color.DimGray.ToArgb
    Private cPilotGroupTextColor As Long = Color.White.ToArgb
    Private cPilotSkillTextColor As Long = Color.Black.ToArgb
    Private cPilotSkillHighlightColor As Long = Color.DodgerBlue.ToArgb
    Private cDBTimeout As Integer = 30
    Private cDBDataFilename As String = ""
    Private cDBDataName As String = ""
    Private cIgnoreSellOrders As Boolean = False
    Private cIgnoreBuyOrders As Boolean = True
    Private cIgnoreSellOrderLimit As Double = 1000
    Private cIgnoreBuyOrderLimit As Double = 1
    Private cMarketRegionList As New ArrayList
    Private cPriceCriteria(11) As Boolean
    Private cMarketLogToolTipConfirm As Boolean = False
    Private cMarketLogPopupConfirm As Boolean = False
    Private cMarketLogUpdatePrice As Boolean = False
    Private cMarketLogUpdateData As Boolean = False
    Private cShowCompletedSkills As Boolean = False
    Private cMDITabPosition As String = "Top"
    Private cTrainingBarDockPosition As Integer = 0
    Private cTrainingBarHeight As Integer = 54
    Private cTrainingBarWidth As Integer = 100
    Private cDisableAutoWebConnections As Boolean = False
    Private cDisableVisualStyles As Boolean = False
    Private cCSVSeparatorChar As String = ","
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
    Private cAutoMailAPI As Boolean = False
    Private cNotifyEveMail As Boolean = False
    Private cNotifyEveNotification As Boolean = False
    Private cQATLayout As String = ""
    Private cBackupBeforeUpdate As Boolean = False
    Private cSQLQueries As New SortedList(Of String, String)
    Private cThemeStyle As eStyle = eStyle.Office2007Black
    Private cThemeTint As Color = Color.Empty
    Private cThemeSetByUser As Boolean = False
    Private cRibbonMinimised As Boolean = False
    Private cDisableTrainingBar As Boolean = False
    Private cEnableAutomaticSave As Boolean = False
    Private cAutomaticSaveTime As Integer = 60
    Private cLastMessageDate As Date = CDate("01/01/1999")
    Private cIgnoreLastMessage As Boolean = False
    Private cStartWithPrimaryQueue As Boolean = False
    Private cNotifyInsuffClone As Boolean = False
    Private cNotifyAccountTime As Boolean = False
    Private cAccountTimeLimit As Integer = 168
    Private cSkillQueuePanelWidth As Integer = 440
    Private cPriceGroups As New SortedList(Of String, PriceGroup)
    Private cCorporations As New SortedList(Of String, Corporation)
    Private cMarketDataSource As MarketSite = MarketSite.EveMarketeer
    Private _marketDataProvider As String
    Private _marketRegions As New List(Of Int32) 'Will be initialized to a safe default of The Forge
    Private _marketSystem As Integer = 30000142 'Safe Default of Jita
    Private _marketUseRegionMarket As Boolean = False
    Private _marketDefaultMetric As MarketMetric
    Private _marketDefaultTransactionType As MarketTransactionKind
    Private _marketDataUploadEnabled As Boolean = False
    Private _marketStatOverrides As New Dictionary(Of Integer, ItemMarketOverride)
    Private cMaxUpdateThreads As Integer = 5


    Public Property MarketDataProvider As String
        Get
            If _marketDataProvider.IsNullOrWhiteSpace() = True Then
                _marketDataProvider = MarketProvider.EveCentral.ToString()
            End If
            Return _marketDataProvider
        End Get
        Set(value As String)
            _marketDataProvider = value
        End Set
    End Property

    Public Property MaxUpdateThreads As Integer
        Get
            If cMaxUpdateThreads = 0 Then cMaxUpdateThreads = 5
            Return cMaxUpdateThreads
        End Get
        Set(value As Integer)
            cMaxUpdateThreads = value
        End Set
    End Property

    Public Property MarketDataSource As MarketSite
        Get
            Return cMarketDataSource
        End Get
        Set(value As MarketSite)
            cMarketDataSource = value
        End Set
    End Property

    Public Property Corporations As SortedList(Of String, Corporation)
        Get
            If cCorporations Is Nothing Then
                cCorporations = New SortedList(Of String, Corporation)
            End If
            Return cCorporations
        End Get
        Set(value As SortedList(Of String, Corporation))
            cCorporations = value
        End Set
    End Property

    Public Property PriceGroups As SortedList(Of String, PriceGroup)
        Get
            If cPriceGroups Is Nothing Then
                cPriceGroups = New SortedList(Of String, PriceGroup)
            End If
            Return cPriceGroups
        End Get
        Set(ByVal value As SortedList(Of String, PriceGroup))
            cPriceGroups = value
        End Set
    End Property

    Public Property SkillQueuePanelWidth() As Integer
        Get
            If cSkillQueuePanelWidth = 0 Then
                cSkillQueuePanelWidth = 440
            End If
            Return cSkillQueuePanelWidth
        End Get
        Set(ByVal value As Integer)
            cSkillQueuePanelWidth = value
        End Set
    End Property

    Public Property AccountTimeLimit() As Integer
        Get
            If cAccountTimeLimit = 0 Then cAccountTimeLimit = 168
            Return cAccountTimeLimit
        End Get
        Set(ByVal value As Integer)
            cAccountTimeLimit = value
        End Set
    End Property

    Public Property NotifyAccountTime() As Boolean
        Get
            Return cNotifyAccountTime
        End Get
        Set(ByVal value As Boolean)
            cNotifyAccountTime = value
        End Set
    End Property
    Public Property NotifyInsuffClone() As Boolean
        Get
            Return cNotifyInsuffClone
        End Get
        Set(ByVal value As Boolean)
            cNotifyInsuffClone = value
        End Set
    End Property

    Public Property StartWithPrimaryQueue() As Boolean
        Get
            Return cStartWithPrimaryQueue
        End Get
        Set(ByVal value As Boolean)
            cStartWithPrimaryQueue = value
        End Set
    End Property

    Public Property IgnoreLastMessage() As Boolean
        Get
            Return cIgnoreLastMessage
        End Get
        Set(ByVal value As Boolean)
            cIgnoreLastMessage = value
        End Set
    End Property

    Public Property LastMessageDate() As Date
        Get
            Return cLastMessageDate
        End Get
        Set(ByVal value As Date)
            cLastMessageDate = value
        End Set
    End Property

    Public Property DisableTrainingBar() As Boolean
        Get
            Return cDisableTrainingBar
        End Get
        Set(ByVal value As Boolean)
            cDisableTrainingBar = value
        End Set
    End Property
    Public Property EnableAutomaticSave() As Boolean
        Get
            Return cEnableAutomaticSave
        End Get
        Set(ByVal value As Boolean)
            cEnableAutomaticSave = value
        End Set
    End Property
    Public Property AutomaticSaveTime() As Integer
        Get
            If cAutomaticSaveTime = 0 Then cAutomaticSaveTime = 60
            Return cAutomaticSaveTime
        End Get
        Set(ByVal value As Integer)
            cAutomaticSaveTime = value
        End Set
    End Property

    Public Property RibbonMinimised() As Boolean
        Get
            Return cRibbonMinimised
        End Get
        Set(ByVal value As Boolean)
            cRibbonMinimised = value
        End Set
    End Property

    Public Property ThemeSetByUser() As Boolean
        Get
            Return cThemeSetByUser
        End Get
        Set(ByVal value As Boolean)
            cThemeSetByUser = value
        End Set
    End Property

    Public Property ThemeTint() As Color
        Get
            Return cThemeTint
        End Get
        Set(ByVal value As Color)
            cThemeTint = value
        End Set
    End Property

    Public Property ThemeStyle() As eStyle
        Get
            Return cThemeStyle
        End Get
        Set(ByVal value As eStyle)
            cThemeStyle = value
        End Set
    End Property

    Public Property SQLQueries() As SortedList(Of String, String)
        Get
            If cSQLQueries Is Nothing Then
                cSQLQueries = New SortedList(Of String, String)
            End If
            Return cSQLQueries
        End Get
        Set(ByVal value As SortedList(Of String, String))
            cSQLQueries = value
        End Set
    End Property

    Public Property BackupBeforeUpdate() As Boolean
        Get
            Return cBackupBeforeUpdate
        End Get
        Set(ByVal value As Boolean)
            cBackupBeforeUpdate = value
        End Set
    End Property

    Public Property QATLayout() As String
        Get
            Return cQATLayout
        End Get
        Set(ByVal value As String)
            cQATLayout = value
        End Set
    End Property

    Public Property NotifyEveNotification() As Boolean
        Get
            Return cNotifyEveNotification
        End Get
        Set(ByVal value As Boolean)
            cNotifyEveNotification = value
        End Set
    End Property

    Public Property NotifyEveMail() As Boolean
        Get
            Return cNotifyEveMail
        End Get
        Set(ByVal value As Boolean)
            cNotifyEveMail = value
        End Set
    End Property

    Public Property AutoMailAPI() As Boolean
        Get
            Return cAutoMailAPI
        End Get
        Set(ByVal value As Boolean)
            cAutoMailAPI = value
        End Set
    End Property

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

    Public Property TrainingBarHeight() As Integer
        Get
            Return cTrainingBarHeight
        End Get
        Set(ByVal value As Integer)
            cTrainingBarHeight = value
        End Set
    End Property

    Public Property TrainingBarWidth() As Integer
        Get
            Return cTrainingBarWidth
        End Get
        Set(ByVal value As Integer)
            cTrainingBarWidth = value
        End Set
    End Property

    Public Property TrainingBarDockPosition() As Integer
        Get
            Return cTrainingBarDockPosition
        End Get
        Set(ByVal value As Integer)
            cTrainingBarDockPosition = value
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
            'Bug #75: Broken API update due to CCP disabling HTTP endpoint
            'Resolution: Now the code will check and forcibly update the api location to https 
            'Any time it is retrieved or saved.
            cCCPAPIServerAddress = ForceHTTPSOnCCPEndpoints(cCCPAPIServerAddress)
            Return cCCPAPIServerAddress
        End Get
        Set(ByVal value As String)
            cCCPAPIServerAddress = ForceHTTPSOnCCPEndpoints(value)
        End Set
    End Property

    Public Property EveFolderLabel(ByVal index As Integer) As String
        Get
            If cEveFolderLabel Is Nothing Then
                ReDim cEveFolderLabel(4)
            End If
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder Label index must be in the range 1 to 4", "Eve Folder Label Get Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return "0"
            Else
                Return cEveFolderLabel(index)
            End If
        End Get
        Set(ByVal value As String)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder Label index must be in the range 1 to 4", "Eve Folder Label Set Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                MessageBox.Show("Eve Folder LUA index must be in the range 1 to 4", "Eve Folder Get Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                Return cEveFolderLUA(index)
            End If
        End Get
        Set(ByVal value As Boolean)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder LUA index must be in the range 1 to 4", "Eve Folder Set Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                MessageBox.Show("Eve Main Form Position index must be in the range 0 to 4",
                                "Eve Main Form Position Get Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return 0
            Else
                Return cMainFormPosition(index)
            End If
        End Get
        Set(ByVal value As Integer)
            If index < 0 Or index > 4 Then
                MessageBox.Show("Eve Main Form Position index must be in the range 0 to 4",
                                "Eve Main Form Position Set Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

    Public Property UseSSL() As Boolean
        Get
            Return cUseSSL
        End Get
        Set(ByVal value As Boolean)
            cUseSSL = value
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

    Public Property ProxyUseDefault() As Boolean
        Get
            Return cProxyUseDefault
        End Get
        Set(ByVal value As Boolean)
            cProxyUseDefault = value
        End Set
    End Property

    Public Property ProxyUseBasic() As Boolean
        Get
            Return cProxyUseBasic
        End Get
        Set(ByVal value As Boolean)
            cProxyUseBasic = value
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

    Public Property IGBFullMode() As Boolean
        Get
            Return cIGBFullMode
        End Get
        Set(ByVal value As Boolean)
            cIGBFullMode = value
        End Set
    End Property

    Public Property IGBAllowedData() As SortedList(Of String, Boolean)
        Get
            If cIGBAllowedData Is Nothing Then
                cIGBAllowedData = New SortedList(Of String, Boolean)
                Call IGB.CheckAllIGBAccessRights()
            End If
            Return cIGBAllowedData
        End Get
        Set(ByVal value As SortedList(Of String, Boolean))
            cIGBAllowedData = value
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
                MessageBox.Show("Eve Folder index must be in the range 1 to 4", "Eve Folder Get Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return "0"
            Else
                Return cEveFolder(index)
            End If
        End Get
        Set(ByVal value As String)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder index must be in the range 1 to 4", "Eve Folder Set Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
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
                ReDim cQColumns(20, 1)
            End If
            Return cQColumns(col, ref)
        End Get
        Set(ByVal value As String)
            If cQColumns.GetUpperBound(0) < 19 Then
                ReDim cQColumns(20, 1)
            End If
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

    Public Property MarketRegions As List(Of Integer)
        Get
            If (_marketRegions Is Nothing) Then
                _marketRegions = New List(Of Integer)
            End If

            If (_marketRegions.Count = 0) Then
                _marketRegions.Add(10000002) ' The Forge... safe default.
            End If

            Return _marketRegions
        End Get
        Set(value As List(Of Integer))
            _marketRegions = value
        End Set
    End Property

    Public Property MarketSystem As Integer
        Get
            If _marketSystem = 0 Then
                _marketSystem = 30000142
            End If
            Return _marketSystem
        End Get
        Set(value As Integer)
            _marketSystem = value
        End Set
    End Property

    Public Property MarketUseRegionMarket As Boolean
        Get
            Return _marketUseRegionMarket
        End Get
        Set(value As Boolean)
            _marketUseRegionMarket = value
        End Set
    End Property

    Public Property MarketDefaultMetric As MarketMetric
        Get
            Return _marketDefaultMetric
        End Get
        Set(value As MarketMetric)
            _marketDefaultMetric = value
        End Set
    End Property

    Public Property MarketDataUploadEnabled As Boolean
        Get
            Return _marketDataUploadEnabled
        End Get
        Set(value As Boolean)
            _marketDataUploadEnabled = value
        End Set
    End Property

    Public Property MarketStatOverrides As Dictionary(Of Integer, ItemMarketOverride)
        Get
            If (_marketStatOverrides Is Nothing) Then
                _marketStatOverrides = New Dictionary(Of Integer, ItemMarketOverride)
            End If

            Return _marketStatOverrides
        End Get
        Set(value As Dictionary(Of Integer, ItemMarketOverride))
            _marketStatOverrides = value
        End Set
    End Property

    Public Property MarketDefaultTransactionType As MarketTransactionKind
        Get
            Return _marketDefaultTransactionType
        End Get
        Set(value As MarketTransactionKind)
            _marketDefaultTransactionType = value
        End Set
    End Property


    ''' <summary>
    '''     Validates that when "official" CCP api endpoints are used, the http scheme is forced to https.
    '''     Also the older eve-online.com domain will be changed to eveonline.com
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Function ForceHTTPSOnCCPEndpoints(endpoint As String) As String
        Const oldApi1 As String = "http://api.eveonline.com"
        Const oldApi2 As String = "http://api.eve-online.com"
        Const badApi As String = "https://api.eve-online.com" 'this https endpoint isn't supported anymore

        Dim normalizedEndpoint As String = endpoint.ToLowerInvariant()

        If (normalizedEndpoint = oldApi1 Or normalizedEndpoint = oldApi2 Or normalizedEndpoint = badApi) Then
            normalizedEndpoint = OfficalApiLocation
        End If

        Return normalizedEndpoint
    End Function
End Class

'Public Class EveHQSettingsFunctions
'    Public Shared Sub SaveSettings()
'        Call SaveEveHQSettings()
'        Call SaveTraining()
'    End Sub                  'SaveSettings
'    Public Shared Function LoadSettings(ShowRawData As Boolean) As Boolean
'        If LoadEveHQSettings(ShowRawData) = False Then
'            Return False
'            Exit Function
'        End If
'        Call LoadTraining()
'        Return True
'    End Function             'LoadSettings

'    Public Shared Sub SaveTraining()
'        HQ.WriteLogEvent("Settings: Saving EveHQ training queues")
'        For Each currentPilot As EveHQPilot In HQ.Settings.Pilots.Values
'            If currentPilot.TrainingQueues IsNot Nothing Then

'                Dim XMLDoc As New XmlDocument
'                Dim dec As XmlDeclaration = XMLDoc.CreateXmlDeclaration("1.0", Nothing, Nothing)
'                Dim XMLAtt As XmlAttribute
'                ' Write root node
'                XMLDoc.AppendChild(dec)

'                Dim XMLRoot As XmlElement = XMLDoc.CreateElement("training")
'                Dim version As String = My.Application.Info.Version.Major.ToString & "." &
'                                        My.Application.Info.Version.Minor.ToString
'                XMLAtt = XMLDoc.CreateAttribute("version")
'                XMLAtt.Value = version
'                XMLRoot.Attributes.Append(XMLAtt)
'                XMLDoc.AppendChild(XMLRoot)

'                For Each currentQueue As SkillQueue In currentPilot.TrainingQueues.Values

'                    Dim QNode As XmlNode = XMLDoc.CreateElement("queue")
'                    XMLAtt = XMLDoc.CreateAttribute("name")
'                    XMLAtt.Value = HttpUtility.HtmlEncode(currentQueue.Name)
'                    QNode.Attributes.Append(XMLAtt)
'                    XMLAtt = XMLDoc.CreateAttribute("ICT")
'                    XMLAtt.Value = currentQueue.IncCurrentTraining.ToString
'                    QNode.Attributes.Append(XMLAtt)
'                    XMLAtt = XMLDoc.CreateAttribute("primary")
'                    XMLAtt.Value = currentQueue.Primary.ToString
'                    QNode.Attributes.Append(XMLAtt)
'                    XMLRoot.AppendChild(QNode)

'                    Dim mySkillQueue As SkillQueueItem
'                    For Each mySkillQueue In currentQueue.Queue
'                        Dim skillNode As XmlNode = XMLDoc.CreateElement("skill")

'                        Dim IDNode As XmlNode = XMLDoc.CreateElement("skillID")
'                        IDNode.InnerText = mySkillQueue.Name
'                        skillNode.AppendChild(IDNode)

'                        Dim FromNode As XmlNode = XMLDoc.CreateElement("fromLevel")
'                        FromNode.InnerText = mySkillQueue.FromLevel.ToString
'                        skillNode.AppendChild(FromNode)

'                        Dim ToNode As XmlNode = XMLDoc.CreateElement("toLevel")
'                        ToNode.InnerText = mySkillQueue.ToLevel.ToString
'                        skillNode.AppendChild(ToNode)

'                        Dim PosNode As XmlNode = XMLDoc.CreateElement("position")
'                        PosNode.InnerText = mySkillQueue.Pos.ToString
'                        skillNode.AppendChild(PosNode)

'                        Dim NotesNode As XmlNode = XMLDoc.CreateElement("notes")
'                        NotesNode.InnerText = HttpUtility.HtmlEncode(mySkillQueue.Notes)
'                        skillNode.AppendChild(NotesNode)

'                        QNode.AppendChild(skillNode)

'                    Next
'                Next
'                Try
'                    Dim tFileName As String = "Q_" & currentPilot.Name & ".xml"
'                    XMLDoc.Save(Path.Combine(HQ.dataFolder, tFileName))
'                Catch e As Exception
'                    MessageBox.Show(e.Message, "Error Saving Training Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                    Exit Sub
'                End Try
'            End If
'        Next
'    End Sub

'    Public Shared Sub LoadTraining()
'        Dim currentPilot As Pilot = New Pilot
'        Dim XMLdoc As XmlDocument = New XmlDocument
'        Dim XMLS As String = ""
'        Dim tFileName As String = ""

'        Dim trainingList, QueueList As XmlNodeList
'        Dim trainingDetails, Queuedetails As XmlNode

'        Dim ObsoleteSkills() As String =
'                {"Analytical Mind", "Clarity", "Eidetic Memory", "Empathy", "Focus", "Instant Recall", "Iron Will",
'                 "Learning", "Logic", "Presence", "Spatial Awareness"}
'        Dim ObsoleteList As New List(Of String)(ObsoleteSkills)

'        For Each currentPilot In HQ.Settings.Pilots
'            currentPilot.ActiveQueue = New SkillQueue
'            'currentPilot.ActiveQueue.Queue.Clear()
'            currentPilot.TrainingQueues = New SortedList
'            currentPilot.TrainingQueues.Clear()
'            currentPilot.PrimaryQueue = ""

'            tFileName = "Q_" & currentPilot.Name & ".xml"
'            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.dataFolder, tFileName)) = True Then
'                Try
'                    XMLdoc.Load(Path.Combine(HQ.dataFolder, tFileName))

'                    ' Get the EveHQ.Core.Pilot details
'                    trainingList = XMLdoc.SelectNodes("/training/skill")

'                    If trainingList.Count > 0 Then
'                        ' Using version prior to 1.3
'                        ' Start a new SkillQueue class (using "primary" as the default name)
'                        Dim newQ As New SkillQueue
'                        newQ.Name = "Primary"
'                        newQ.IncCurrentTraining = True
'                        newQ.Primary = True
'                        For Each trainingDetails In trainingList
'                            Dim myskill As SkillQueueItem = New SkillQueueItem
'                            myskill.Name = trainingDetails.ChildNodes(0).InnerText
'                            myskill.FromLevel = CInt(trainingDetails.ChildNodes(1).InnerText)
'                            myskill.ToLevel = CInt(trainingDetails.ChildNodes(2).InnerText)
'                            myskill.Pos = CInt(trainingDetails.ChildNodes(3).InnerText)
'                            Dim keyName As String = myskill.Name & myskill.FromLevel & myskill.ToLevel
'                            currentPilot.ActiveQueue.Queue.Add(myskill, keyName)
'                        Next
'                        newQ.Queue = currentPilot.ActiveQueue.Queue
'                        currentPilot.PrimaryQueue = newQ.Name
'                        currentPilot.TrainingQueues.Add(newQ.Name, newQ)
'                    Else
'                        ' Try for the post 1.3 version
'                        ' Get version
'                        Dim rootNode As XmlNode = XMLdoc.SelectSingleNode("/training")
'                        Dim version As Double = 0
'                        Dim culture As CultureInfo = New CultureInfo("en-GB")
'                        If rootNode.Attributes.Count > 0 Then
'                            version = Double.Parse(rootNode.Attributes("version").Value, NumberStyles.Any, culture)
'                        End If
'                        QueueList = XMLdoc.SelectNodes("/training/queue")
'                        If QueueList.Count > 0 Then
'                            For Each Queuedetails In QueueList
'                                Dim newQ As New SkillQueue
'                                newQ.Name = HttpUtility.HtmlDecode(Queuedetails.Attributes("name").Value)
'                                newQ.IncCurrentTraining = CBool(Queuedetails.Attributes("ICT").Value)
'                                newQ.Primary = CBool(Queuedetails.Attributes("primary").Value)
'                                If newQ.Primary = True Then
'                                    If currentPilot.PrimaryQueue <> "" Then
'                                        ' There can be only one!
'                                        newQ.Primary = False
'                                    Else
'                                        currentPilot.PrimaryQueue = newQ.Name
'                                    End If
'                                End If
'                                trainingList = Queuedetails.SelectNodes("training/queue/skill")
'                                If Queuedetails.ChildNodes.Count > 0 Then
'                                    ' Using version prior to 1.3
'                                    ' Start a new SkillQueue class (using "primary" as the default name)
'                                    For Each trainingDetails In Queuedetails.ChildNodes
'                                        If ObsoleteList.Contains(trainingDetails.ChildNodes(0).InnerText) = False Then
'                                            Dim myskill As SkillQueueItem = New SkillQueueItem
'                                            myskill.Name = trainingDetails.ChildNodes(0).InnerText
'                                            ' Adjust for the 1.9 version
'                                            If version < 1.9 Then
'                                                If myskill.Name = "Astrometric Triangulation" Then
'                                                    myskill.Name = "Astrometric Acquisition"
'                                                End If
'                                                If myskill.Name = "Signal Acquisition" Then
'                                                    myskill.Name = "Astrometric Triangulation"
'                                                End If
'                                            End If
'                                            Try
'                                                myskill.FromLevel = CInt(trainingDetails.ChildNodes(1).InnerText)
'                                                myskill.ToLevel = CInt(trainingDetails.ChildNodes(2).InnerText)
'                                                myskill.Pos = CInt(trainingDetails.ChildNodes(3).InnerText)
'                                                myskill.Notes =
'                                                    HttpUtility.HtmlDecode(trainingDetails.ChildNodes(4).InnerText)
'                                            Catch e As Exception
'                                                ' We don't have the required info
'                                            End Try
'                                            Dim keyName As String = myskill.Name & myskill.FromLevel & myskill.ToLevel
'                                            If newQ.Queue.Contains(keyName) = False Then
'                                                If myskill.ToLevel > myskill.FromLevel Then
'                                                    newQ.Queue.Add(myskill, keyName) _
'                                                    ' Multi queue method
'                                                End If
'                                            End If
'                                        End If
'                                    Next
'                                End If
'                                currentPilot.TrainingQueues.Add(newQ.Name, newQ)
'                            Next
'                        End If
'                    End If

'                    ' Iterate through the relevant nodes

'                Catch e As Exception
'                    MessageBox.Show(
'                        "Error importing Training data for " & currentPilot.Name & "." & ControlChars.CrLf &
'                        "The error reported was " & e.Message, "Training Data Error", MessageBoxButtons.OK,
'                        MessageBoxIcon.Error)
'                End Try
'            End If
'        Next
'    End Sub

'    Public Shared Sub ResetColumns()
'        HQ.Settings.QColumns(0, 0) = "Name"
'        HQ.Settings.QColumns(0, 1) = CStr(True)
'        HQ.Settings.QColumns(1, 0) = "Curr"
'        HQ.Settings.QColumns(1, 1) = CStr(True)
'        HQ.Settings.QColumns(2, 0) = "From"
'        HQ.Settings.QColumns(2, 1) = CStr(True)
'        HQ.Settings.QColumns(3, 0) = "Tole"
'        HQ.Settings.QColumns(3, 1) = CStr(True)
'        HQ.Settings.QColumns(4, 0) = "Perc"
'        HQ.Settings.QColumns(4, 1) = CStr(True)
'        HQ.Settings.QColumns(5, 0) = "Trai"
'        HQ.Settings.QColumns(5, 1) = CStr(True)
'        HQ.Settings.QColumns(6, 0) = "Comp"
'        HQ.Settings.QColumns(6, 1) = CStr(True)
'        HQ.Settings.QColumns(7, 0) = "Date"
'        HQ.Settings.QColumns(7, 1) = CStr(True)
'        HQ.Settings.QColumns(8, 0) = "Rank"
'        HQ.Settings.QColumns(8, 1) = CStr(False)
'        HQ.Settings.QColumns(9, 0) = "PAtt"
'        HQ.Settings.QColumns(9, 1) = CStr(False)
'        HQ.Settings.QColumns(10, 0) = "SAtt"
'        HQ.Settings.QColumns(10, 1) = CStr(False)
'        HQ.Settings.QColumns(11, 0) = "SPRH"
'        HQ.Settings.QColumns(11, 1) = CStr(False)
'        HQ.Settings.QColumns(12, 0) = "SPRD"
'        HQ.Settings.QColumns(12, 1) = CStr(False)
'        HQ.Settings.QColumns(13, 0) = "SPRW"
'        HQ.Settings.QColumns(13, 1) = CStr(False)
'        HQ.Settings.QColumns(14, 0) = "SPRM"
'        HQ.Settings.QColumns(14, 1) = CStr(False)
'        HQ.Settings.QColumns(15, 0) = "SPRY"
'        HQ.Settings.QColumns(15, 1) = CStr(False)
'        HQ.Settings.QColumns(16, 0) = "SPAd"
'        HQ.Settings.QColumns(16, 1) = CStr(False)
'        HQ.Settings.QColumns(17, 0) = "SPTo"
'        HQ.Settings.QColumns(17, 1) = CStr(False)
'        HQ.Settings.QColumns(18, 0) = "Note"
'        HQ.Settings.QColumns(18, 1) = CStr(False)
'        HQ.Settings.QColumns(19, 0) = "Prio"
'        HQ.Settings.QColumns(19, 1) = CStr(False)
'        HQ.Settings.QColumnsSet = True
'    End Sub

'    Public Shared Sub SaveEveHQSettings()
'        HQ.WriteLogEvent("Settings: Saving EveHQ settings to " & Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin"))
'        ' Write a serial version of the settings
'        Try
'            Dim s As New FileStream(Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin"), FileMode.Create)
'            Dim f As New BinaryFormatter
'            f.Serialize(s, HQ.EveHqSettings)
'            s.Flush()
'            s.Close()
'            s.Dispose()
'            HQ.WriteLogEvent("Settings: Saved EveHQ settings to " & Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin"))
'        Catch e As Exception
'            HQ.WriteLogEvent(
'                "Settings: Error saving EveHQ settings to " &
'                Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin - " & e.Message))
'        End Try
'        ' Update the Proxy Server settings
'        Call InitialiseRemoteProxyServer()
'        ' Set Global APIServerInfo
'        HQ.EveHQAPIServerInfo = New APIServerInfo(HQ.Settings.CCPAPIServerAddress, HQ.Settings.APIRSAddress,
'                                                  HQ.Settings.UseAPIRS, HQ.Settings.UseCCPAPIBackup)
'    End Sub

'    Public Shared Function LoadEveHQSettings(ShowRawData As Boolean) As Boolean

'        ' Convert the EveHQ Settings
'        Dim importFile As String = Path.Combine(HQ.AppDataFolder, "EveHQSettings.exported.json.txt")
'        Dim showImport As Boolean = File.Exists(importFile)
'        Dim importMessage As String = "A exported settings file has been detected. Would you like to import this file?" & ControlChars.CrLf & ControlChars.CrLf & "Warning: All of your current settings will be overwritten! Please make sure you have a backup first."
'        If (showImport = True) Then
'            If (MessageBox.Show(importMessage, "Import Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
'                ImportOldSettings(importFile)
'            End If
'        Else
'            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin")) = True Then
'                'Dim s As New FileStream(Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin"), FileMode.Open)
'                ' Temp code for JSON Test
'                'Dim s As New StreamReader(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"))
'                Try

'                    Using s As New StreamReader(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"))
'                        Dim json As String = s.ReadToEnd
'                        Dim settings As EveHQSettings = JsonConvert.DeserializeObject(Of EveHQSettings)(json)
'                        Call ResetPilotData()
'                    End Using

'                    'EveSettings2.LoadJSONSettings()

'                    'Dim json As String = s.ReadToEnd
'                    'EveHQ.Core.HQ.EveHqSettings = JsonConvert.DeserializeObject(Of EveSettings)(json)
'                    'Dim json As String = JsonConvert.SerializeObject(EveHQ.Core.HQ.EveHqSettings, Formatting.Indented)
'                    'Account account = JsonConvert.DeserializeObject<Account>(json);
'                    'Dim f As BinaryFormatter = New BinaryFormatter
'                    'HQ.EveHqSettings = CType(f.Deserialize(s), EveSettings)
'                    's.Close()




'                    ' Temp holding code till v3 - may fix some incompatibility issues in the binary serialising of .Netv2 and .Netv4
'                    Dim TempAccounts As New List(Of EveAccount)
'                    Dim TempPilots As New List(Of Pilot)
'                    For Each a As EveHQAccount In HQ.Settings.Accounts.Values
'                        TempAccounts.Add(a)
'                    Next
'                    For Each p As EveHQPilot In HQ.Settings.Pilots.Values
'                        TempPilots.Add(p)
'                    Next
'                    HQ.Settings.Accounts.Clear()
'                    HQ.Settings.Pilots.Clear()
'                    For Each a As EveAccount In TempAccounts
'                        HQ.Settings.Accounts.Add(a, a.userID)
'                    Next
'                    For Each p As Pilot In TempPilots
'                        HQ.Settings.Pilots.Add(p, p.Name)
'                    Next
'                    SaveEveHQSettings()

'                Catch ex As Exception
'                    Trace.TraceError(ex.FormatException())
'                    Dim msg As String =
'                            "There was an error trying to load the settings file and it appears that this file is corrupt." &
'                            ControlChars.CrLf & ControlChars.CrLf
'                    msg &= "The error was: " & ex.Message & ControlChars.CrLf & ControlChars.CrLf
'                    msg &= "Stacktrace: " & ex.StackTrace & ControlChars.CrLf & ControlChars.CrLf
'                    msg &=
'                        "EveHQ will copy this file to 'EveHQSettings.bad' and delete the original file and re-initialise the settings. This means you will need to re-enter your API information but your production and fittings data should be intact and available once the API data has been downloaded. You can attempt to reload the old settings by renaming the 'EveHQSettings.bad' file to 'EveHQSettings.bin', however if the issue continues the bad file will be useful to the EveHQ team for debugging purposes" &
'                        ControlChars.CrLf & ControlChars.CrLf
'                    msg &= "Press OK to reset the settings." & ControlChars.CrLf
'                    MessageBox.Show(msg, "Invalid Settings file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                    Try
'                        's.Close()
'                        My.Computer.FileSystem.CopyFile(Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin"), Path.Combine(HQ.AppDataFolder, "EveHQSettings.bad"), True)
'                    Catch e As Exception
'                        MessageBox.Show(
'                            "Unable to delete the EveHQSettings.bin file. Please delete this manually before proceeding",
'                            "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                        Application.Exit()
'                    End Try
'                    Return False
'                End Try
'            End If
'        End If

'        If ShowRawData = False Then

'            ' Reset the update URL to a temp location
'            If HQ.Settings.UpdateURL <> "http://evehq.net/update/" Then
'                HQ.Settings.UpdateURL = "http://evehq.net/update/"
'            End If

'            ' Check if we were using a v1 database and see if we can automatically set it to v2
'            ' Only required for old Access databases
'            'MessageBox.Show("Updating reference to static database...")
'            'MessageBox.Show("Checking DBFormat..." & EveHQ.Core.HQ.Settings.DBFormat.ToString)
'            If HQ.Settings.DBFormat = 0 Then
'                'MessageBox.Show("Checking File existence: " & EveHQ.Core.HQ.Settings.DBFilename)
'                If My.Computer.FileSystem.FileExists(HQ.Settings.DBFilename) Then
'                    Dim DBFI As New FileInfo(HQ.Settings.DBFilename)
'                    If DBFI.Extension = ".mdb" Then
'                        'MessageBox.Show("Old Filename: " & EveHQ.Core.HQ.Settings.DBFilename)
'                        Dim TempFileName As String = "EveHQ.sdf"
'                        ' Check the appdata folder (following an installer setup)
'                        Dim TempFolder As String = HQ.AppDataFolder
'                        If My.Computer.FileSystem.FileExists(Path.Combine(TempFolder, TempFileName)) Then
'                            ' Set the database to the new folder
'                            HQ.Settings.DBFilename = Path.Combine(TempFolder, TempFileName)
'                            HQ.Settings.UseAppDirectoryForDB = False
'                            'MessageBox.Show("New Filename: " & EveHQ.Core.HQ.Settings.DBFilename)
'                        Else
'                            ' Check the app folder (following a zip setup)
'                            TempFolder = HQ.appFolder
'                            If My.Computer.FileSystem.FileExists(Path.Combine(TempFolder, TempFileName)) Then
'                                ' Set the database to the new folder
'                                HQ.Settings.DBFilename = Path.Combine(TempFolder, TempFileName)
'                                HQ.Settings.UseAppDirectoryForDB = False
'                                'MessageBox.Show("New Filename: " & EveHQ.Core.HQ.Settings.DBFilename)
'                            End If
'                        End If
'                    End If
'                Else
'                    ' Can't find database file - assume it has been overwritten by the v2 installer
'                    'MessageBox.Show("Old Filename: " & EveHQ.Core.HQ.Settings.DBFilename)
'                    Dim TempFileName As String = "EveHQ.sdf"
'                    ' Check the appdata folder (following an installer setup)
'                    Dim TempFolder As String = HQ.AppDataFolder
'                    If My.Computer.FileSystem.FileExists(Path.Combine(TempFolder, TempFileName)) Then
'                        ' Set the database to the new folder
'                        HQ.Settings.DBFilename = Path.Combine(TempFolder, TempFileName)
'                        HQ.Settings.UseAppDirectoryForDB = False
'                        'MessageBox.Show("New Filename: " & EveHQ.Core.HQ.Settings.DBFilename)
'                    Else
'                        ' Check the app folder (following a zip setup)
'                        TempFolder = HQ.appFolder
'                        If My.Computer.FileSystem.FileExists(Path.Combine(TempFolder, TempFileName)) Then
'                            ' Set the database to the new folder
'                            HQ.Settings.DBFilename = Path.Combine(TempFolder, TempFileName)
'                            HQ.Settings.UseAppDirectoryForDB = False
'                            'MessageBox.Show("New Filename: " & EveHQ.Core.HQ.Settings.DBFilename)
'                        End If
'                    End If
'                End If
'            End If

'            ' Set the database connection string
'            ' Determine if a database format has been chosen before and set it if not
'            If HQ.Settings.DBFormat = -1 Then
'                HQ.Settings.DBFormat = 0
'                HQ.Settings.DBFilename = Path.Combine(HQ.AppDataFolder, "EveHQ.sdf")
'                ' Check for this file!
'                Dim fileExists As Boolean = False
'                Do
'                    If My.Computer.FileSystem.FileExists(HQ.Settings.DBFilename) = False Then
'                        Dim msg As String = "EveHQ needs a database in order to work correctly." & ControlChars.CrLf
'                        msg &= "If you do not select a valid DB file, EveHQ will exit." & ControlChars.CrLf &
'                               ControlChars.CrLf
'                        msg &= "Would you like to select a file now?" & ControlChars.CrLf
'                        Dim reply As Integer = MessageBox.Show(msg, "Database Required", MessageBoxButtons.YesNo,
'                                                               MessageBoxIcon.Question)
'                        If reply = DialogResult.No Then
'                            Return False
'                            Exit Function
'                        End If
'                        Dim ofd1 As New OpenFileDialog
'                        With ofd1
'                            .Title = "Select SQL CE Data file"
'                            .FileName = ""
'                            .InitialDirectory = HQ.appFolder
'                            .Filter = "SQL CE Data files (*.sdf)|*.sdf|All files (*.*)|*.*"
'                            .FilterIndex = 1
'                            .RestoreDirectory = True
'                            If .ShowDialog() = DialogResult.OK Then
'                                HQ.Settings.DBFilename = .FileName
'                            End If
'                        End With
'                    Else
'                        fileExists = True
'                    End If
'                Loop Until fileExists = True
'                HQ.Settings.DBUsername = ""
'                HQ.Settings.DBPassword = ""
'            End If

'            ' See if people actually bothered to RTFM and install SQLCEv4!
'            Try
'                If DataFunctions.SetEveHQConnectionString() = False Then
'                    Return False
'                End If
'                If DataFunctions.SetEveHQDataConnectionString() = False Then
'                    Return False
'                End If
'            Catch ex As Exception
'                Dim msg As New StringBuilder
'                msg.AppendLine("Error: " & ex.Message)
'                msg.AppendLine("")
'                msg.AppendLine(
'                    "An error occurred trying to access the database, with the most common cause being that SQL Compact Edition v4 was not installed as instructed.")
'                msg.AppendLine("")
'                msg.AppendLine(
'                    "Click OK to close EveHQ where you will be redirected to the SQL Compact Edition download page at http://www.microsoft.com/download/en/details.aspx?id=17876")
'                MessageBox.Show(msg.ToString, "Error Initialising Database", MessageBoxButtons.OK,
'                                MessageBoxIcon.Information)
'                Try
'                    Process.Start("http://www.microsoft.com/download/en/details.aspx?id=17876")
'                    Application.ExitThread()
'                Catch ex2 As Exception
'                    ' Do nothing - users have the link
'                End Try
'            End Try

'            ' Load the skill data before attempting to load in the EveHQ.Core.Pilot skill data
'            If SkillFunctions.LoadEveSkillData() = False Then
'                Return False
'                Exit Function
'            End If

'            '  Setup queue columns etc
'            Call InitialiseQueueColumns()
'            Call InitialiseUserColumns()
'            Call InitialiseRemoteProxyServer()
'            If HQ.Settings.QColumns(0, 0) Is Nothing Then
'                Call ResetColumns()
'            End If

'            ' Set Theme stuff
'            If HQ.Settings.ThemeSetByUser = False Then
'                HQ.Settings.ThemeStyle = eStyle.Office2007Black
'                HQ.Settings.ThemeTint = Color.Empty
'            End If

'            ' Set up a global price list if not present
'            If HQ.Settings.PriceGroups.ContainsKey("<Global>") = False Then
'                ' Add a new price group
'                Dim NewPG As New PriceGroup
'                NewPG.Name = "<Global>"
'                NewPG.RegionIDs.Add("10000002")
'                NewPG.PriceFlags = PriceGroupFlags.MinSell
'                HQ.Settings.PriceGroups.Add(NewPG.Name, NewPG)
'            End If

'            ' Set Global APIServerInfo
'            HQ.EveHQAPIServerInfo = New APIServerInfo(HQ.Settings.CCPAPIServerAddress,
'                                                      HQ.Settings.APIRSAddress, HQ.Settings.UseAPIRS,
'                                                      HQ.Settings.UseCCPAPIBackup)

'            ' Check for unknown or V1 accounts and remove them
'            Dim OldAccountList As New List(Of String)
'            For Each CheckAccount As EveHQAccount In HQ.Settings.Accounts.Values
'                If _
'                    CheckAccount.APIKeySystem = APIKeySystems.Unknown Or
'                    CheckAccount.APIKeySystem = APIKeySystems.Version1 Then
'                    OldAccountList.Add(CheckAccount.userID)
'                End If
'            Next
'            If OldAccountList.Count > 0 Then
'                For Each AccountID As String In OldAccountList
'                    HQ.Settings.Accounts.Remove(AccountID)
'                Next
'                Dim msg As New StringBuilder
'                msg.AppendLine(
'                    "EveHQ has detected legacy API keys in the settings file. As these are no longer supported, these have been removed.")
'                msg.AppendLine("")
'                msg.AppendLine(
'                    "You will need to add Customisable API Keys (CAKs) for any characters or corporations removed by this procedure that you wish to continue using.")
'                MessageBox.Show(msg.ToString, "Legacy API Keys Removed", MessageBoxButtons.OK,
'                                MessageBoxIcon.Information)
'            End If

'        End If

'        Return True
'    End Function

'    Private Shared Sub ImportOldSettings(importFile As String)
'        Try
'            Dim jsonString As String
'            Using fs As FileStream = New FileStream(importFile, FileMode.Open, FileAccess.Read, FileShare.Read)
'                Dim bits(CInt(fs.Length)) As Byte
'                fs.Read(bits, 0, bits.Length)
'                jsonString = Encoding.UTF8.GetString(bits)
'            End Using

'            Dim settings As EveSettings = JsonConvert.DeserializeObject(Of EveSettings)(jsonString)
'            If (settings Is Nothing) Then
'                MessageBox.Show("The import of settings failed. The file was read successfully, however the resulting object found was not an EveSettings instance. Please make sure you placed the correct file in the Application data folder.", "Invalid File!", MessageBoxButtons.OK)
'            Else
'                ' Because the Pilots collection is not typed the import of the Json uses a custom anonymous type
'                ' we need to fix that.
'                Dim fixedPilots As New Collection
'                For Each pilotImported As Object In settings.Pilots
'                    Dim temp As String = JsonConvert.SerializeObject(pilotImported)
'                    Dim pilot As Pilot = JsonConvert.DeserializeObject(Of Pilot)(temp)
'                    Dim skills As New Collection
'                    For Each importedSkill As Object In pilot.PilotSkills
'                        temp = JsonConvert.SerializeObject(importedSkill)
'                        Dim skill As PilotSkill = JsonConvert.DeserializeObject(Of PilotSkill)(temp)
'                        skills.Add(skill, skill.Name)
'                    Next
'                    pilot.PilotSkills = skills
'                    fixedPilots.Add(pilot)
'                Next
'                settings.Pilots = fixedPilots

'                ' Accounts are also a non typed collection
'                Dim fixedAccounts As New Collection
'                For Each accountImported As Object In settings.Accounts
'                    Dim temp As String = JsonConvert.SerializeObject(accountImported)
'                    fixedAccounts.Add(JsonConvert.DeserializeObject(Of EveAccount)(temp))
'                Next

'                settings.Accounts = fixedAccounts

'                ' Plugins
'                Dim fixedPlugins As New SortedList
'                For Each plugin As String In settings.Plugins.Keys
'                    Dim temp As String = JsonConvert.SerializeObject(settings.Plugins(plugin))
'                    fixedPlugins.Add(plugin, JsonConvert.DeserializeObject(Of PlugIn)(temp))
'                Next
'                settings.Plugins = fixedPlugins

'                ' Dashboard Config
'                Dim fixedDashboard As New ArrayList
'                For Each config As Object In settings.DashboardConfiguration
'                    Dim temp As String = JsonConvert.SerializeObject(config)
'                    fixedDashboard.Add(JsonConvert.DeserializeObject(Of SortedList(Of String, Object))(temp))
'                Next

'                settings.DashboardConfiguration = fixedDashboard

'                ' TODO: Make settings more Strongly typed!

'                HQ.EveHqSettings = settings
'                ResetPilotData()
'                ' Temp holding code till v3 - may fix some incompatibility issues in the binary serialising of .Netv2 and .Netv4
'                Dim TempAccounts As New List(Of EveAccount)
'                Dim TempPilots As New List(Of Pilot)
'                For Each a As EveHQAccount In HQ.Settings.Accounts.Values
'                    TempAccounts.Add(a)
'                Next
'                For Each p As EveHQPilot In HQ.Settings.Pilots.Values
'                    TempPilots.Add(p)
'                Next
'                HQ.Settings.Accounts.Clear()
'                HQ.Settings.Pilots.Clear()
'                For Each a As EveAccount In TempAccounts
'                    HQ.Settings.Accounts.Add(a, a.userID)
'                Next
'                For Each p As Pilot In TempPilots
'                    HQ.Settings.Pilots.Add(p, p.Name)
'                Next
'                SaveEveHQSettings()
'                MessageBox.Show("Settings imported and saved successfully! Please delete the {0} file so you are not prompted again".FormatInvariant(importFile), "Done!", MessageBoxButtons.OK)
'            End If
'        Catch ex As Exception
'            Trace.TraceError(ex.FormatException())
'            MessageBox.Show("The import of settings failed. Please check the EveHQ log files for details on the exception.")
'        End Try
'    End Sub

'    Private Shared Sub ResetPilotData()
'        ' Resets certain data added to the pilot class not initialised from the binary deserialisation
'        For Each Pilot As EveHQPilot In HQ.Settings.Pilots.Values
'            If Pilot.Standings Is Nothing Then
'                Pilot.Standings = New SortedList(Of Long, PilotStanding)
'            End If
'        Next
'    End Sub

'    Public Shared Sub InitialiseRemoteProxyServer()
'        HQ.RemoteProxy.ProxyRequired = HQ.Settings.ProxyRequired
'        HQ.RemoteProxy.ProxyServer = HQ.Settings.ProxyServer
'        HQ.RemoteProxy.ProxyPort = HQ.Settings.ProxyPort
'        HQ.RemoteProxy.UseDefaultCredentials = HQ.Settings.ProxyUseDefault
'        HQ.RemoteProxy.ProxyUsername = HQ.Settings.ProxyUsername
'        HQ.RemoteProxy.ProxyPassword = HQ.Settings.ProxyPassword
'        HQ.RemoteProxy.UseBasicAuthentication = HQ.Settings.ProxyUseBasic
'    End Sub

'    Public Shared Sub InitialiseQueueColumns()
'        HQ.Settings.StandardQueueColumns.Clear()
'        Dim newItem As New ListViewItem
'        'newItem = New ListViewItem
'        'newItem.Name = "Name"
'        'newItem.Text = "Skill Name"
'        'newItem.Checked = True
'        'EveHQ.Core.HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "Current"
'        newItem.Text = "Cur Lvl"
'        newItem.Checked = True
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "From"
'        newItem.Text = "From Lvl"
'        newItem.Checked = True
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "To"
'        newItem.Text = "To Lvl"
'        newItem.Checked = True
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "Percent"
'        newItem.Text = "%"
'        newItem.Checked = True
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "TrainTime"
'        newItem.Text = "Training Time"
'        newItem.Checked = True
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "TimeToComplete"
'        newItem.Text = "Time To Complete"
'        newItem.Checked = True
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "DateEnded"
'        newItem.Text = "Date Completed"
'        newItem.Checked = True
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "Rank"
'        newItem.Text = "Rank"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "PAtt"
'        newItem.Text = "Pri Att"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SAtt"
'        newItem.Text = "Sec Att"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SPHour"
'        newItem.Text = "SP /hour"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SPDay"
'        newItem.Text = "SP /day"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SPWeek"
'        newItem.Text = "SP /week"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SPMonth"
'        newItem.Text = "SP /month"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SPYear"
'        newItem.Text = "SP /year"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SPAdded"
'        newItem.Text = "SP Added"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "SPTotal"
'        newItem.Text = "SP Total"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "Notes"
'        newItem.Text = "Notes"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'        newItem = New ListViewItem
'        newItem.Name = "Priority"
'        newItem.Text = "Priority"
'        newItem.Checked = False
'        HQ.Settings.StandardQueueColumns.Add(newItem)
'    End Sub

'    Public Shared Sub InitialiseUserColumns()
'        If HQ.Settings.UserQueueColumns.Count = 0 Then
'            ' Add preset items
'            HQ.Settings.UserQueueColumns.Add("Current1")
'            HQ.Settings.UserQueueColumns.Add("From1")
'            HQ.Settings.UserQueueColumns.Add("To1")
'            HQ.Settings.UserQueueColumns.Add("Percent1")
'            HQ.Settings.UserQueueColumns.Add("TrainTime1")
'            HQ.Settings.UserQueueColumns.Add("TimeToComplete1")
'            HQ.Settings.UserQueueColumns.Add("DateEnded1")
'        End If
'        ' Check if the standard columns have changed and we need to add columns
'        If HQ.Settings.UserQueueColumns.Count <> HQ.Settings.StandardQueueColumns.Count Then
'            For Each slotItem As ListViewItem In HQ.Settings.StandardQueueColumns
'                If _
'                    HQ.Settings.UserQueueColumns.Contains(slotItem.Name & "0") = False And
'                    HQ.Settings.UserQueueColumns.Contains(slotItem.Name & "1") = False Then
'                    HQ.Settings.UserQueueColumns.Add(slotItem.Name & "0")
'                End If
'            Next
'        End If
'    End Sub
'End Class


