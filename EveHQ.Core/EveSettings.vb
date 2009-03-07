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
    Private cOverlayBorderColor As Long = System.Drawing.Color.Black.ToArgb
    Private cOverlayFontColor As Long = System.Drawing.Color.Black.ToArgb
    Private cOverlayPanelColor As Long = System.Drawing.Color.White.ToArgb
    Private cOverlayTransparancy As Integer = 10
    Private cOverlayPosition As Integer = 3
    Private cOverlayXOffset As Integer = 5
    Private cOverlayYOffset As Integer = 5
    Private cOverlayStartup As Boolean = False
    Private cOverlayClickThru As Boolean = False
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
    Private cUseAPIStatusForm As Boolean = True
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
    Private cIgnoreBuyOrders As Boolean = False
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
    Public Property UseAPIStatusForm() As Boolean
        Get
            Return cUseAPIStatusForm
        End Get
        Set(ByVal value As Boolean)
            cUseAPIStatusForm = value
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
    Public Property OverlayClickThru() As Boolean
        Get
            Return cOverlayClickThru
        End Get
        Set(ByVal value As Boolean)
            cOverlayClickThru = value
        End Set
    End Property
    Public Property OverlayStartup() As Boolean
        Get
            Return cOverlayStartup
        End Get
        Set(ByVal value As Boolean)
            cOverlayStartup = value
        End Set
    End Property
    Public Property OverlayYOffset() As Integer
        Get
            Return cOverlayYOffset
        End Get
        Set(ByVal value As Integer)
            cOverlayYOffset = value
        End Set
    End Property
    Public Property OverlayXOffset() As Integer
        Get
            Return cOverlayXOffset
        End Get
        Set(ByVal value As Integer)
            cOverlayXOffset = value
        End Set
    End Property
    Public Property OverlayPosition() As Integer
        Get
            Return cOverlayPosition
        End Get
        Set(ByVal value As Integer)
            cOverlayPosition = value
        End Set
    End Property
    Public Property OverlayTransparancy() As Integer
        Get
            Return cOverlayTransparancy
        End Get
        Set(ByVal value As Integer)
            cOverlayTransparancy = value
        End Set
    End Property
    Public Property OverlayPanelColor() As Long
        Get
            Return cOverlayPanelColor
        End Get
        Set(ByVal value As Long)
            cOverlayPanelColor = value
        End Set
    End Property
    Public Property OverlayFontColor() As Long
        Get
            Return cOverlayFontColor
        End Get
        Set(ByVal value As Long)
            cOverlayFontColor = value
        End Set
    End Property
    Public Property OverlayBorderColor() As Long
        Get
            Return cOverlayBorderColor
        End Get
        Set(ByVal value As Long)
            cOverlayBorderColor = value
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
        Call SaveEveSettings2()
        Call SaveTraining()
    End Sub                  'SaveSettings
    Public Shared Function LoadSettings() As Boolean
        If LoadEveSettings2() = False Then
            Return False
            Exit Function
        End If
        Call LoadTraining()
        Return True
    End Function             'LoadSettings

    Public Shared Sub SaveEveSettings()
        Dim currentAccount As EveAccount = New EveAccount
        Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        Dim currentPlugin As EveHQ.Core.PlugIn = New EveHQ.Core.PlugIn
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""

        ' Prepare the XML document
        XMLS = ("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>") & vbCrLf
        XMLS &= "<EveHQSettings>" & vbCrLf

        ' Save the General Information
        XMLS &= Chr(9) & "<general>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<autoHide>" & EveHQ.Core.HQ.EveHQSettings.AutoHide & "</autoHide>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<autoStart>" & EveHQ.Core.HQ.EveHQSettings.AutoStart & "</autoStart>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<autoShrink>" & EveHQ.Core.HQ.EveHQSettings.AutoMinimise & "</autoShrink>" & vbCrLf
        For a As Integer = 1 To 4
            XMLS &= Chr(9) & Chr(9) & "<eveFolder" & a & ">" & EveHQ.Core.HQ.EveHQSettings.EveFolder(a) & "</eveFolder" & a & ">" & vbCrLf
        Next
        XMLS &= Chr(9) & Chr(9) & "<autoBackup>" & EveHQ.Core.HQ.EveHQSettings.BackupAuto & "</autoBackup>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<backupStart>" & EveHQ.Core.HQ.EveHQSettings.BackupStart & "</backupStart>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<backupFreq>" & EveHQ.Core.HQ.EveHQSettings.BackupFreq & "</backupFreq>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<lastBackup>" & EveHQ.Core.HQ.EveHQSettings.BackupLast & "</lastBackup>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<autoCheck>" & EveHQ.Core.HQ.EveHQSettings.AutoCheck & "</autoCheck>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<startPilot>" & EveHQ.Core.HQ.EveHQSettings.StartupPilot & "</startPilot>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<startView>" & EveHQ.Core.HQ.EveHQSettings.StartupView & "</startView>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<QColumnsSet>" & EveHQ.Core.HQ.EveHQSettings.QColumnsSet & "</QColumnsSet>" & vbCrLf
        For a As Integer = 0 To 16
            XMLS &= Chr(9) & Chr(9) & "<QColumn name='" & EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0) & "'>" & EveHQ.Core.HQ.EveHQSettings.QColumns(a, 1) & "</QColumn>" & vbCrLf
        Next
        XMLS &= Chr(9) & Chr(9) & "<dbFormat>" & EveHQ.Core.HQ.EveHQSettings.DBFormat & "</dbFormat>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<dbFilename>" & EveHQ.Core.HQ.EveHQSettings.DBFilename & "</dbFilename>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<dbServer>" & EveHQ.Core.HQ.EveHQSettings.DBServer & "</dbServer>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<dbUsername>" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "</dbUsername>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<dbPassword>" & EveHQ.Core.HQ.EveHQSettings.DBPassword & "</dbPassword>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<dbSecurity>" & EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity & "</dbSecurity>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<proxyRequired>" & EveHQ.Core.HQ.EveHQSettings.ProxyRequired & "</proxyRequired>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<proxyServer>" & EveHQ.Core.HQ.EveHQSettings.ProxyServer & "</proxyServer>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<proxyPort>" & EveHQ.Core.HQ.EveHQSettings.ProxyPort & "</proxyPort>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<proxyUsername>" & EveHQ.Core.HQ.EveHQSettings.ProxyUsername & "</proxyUsername>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<proxyPassword>" & EveHQ.Core.HQ.EveHQSettings.ProxyPassword & "</proxyPassword>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<proxyDefault>" & EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault & "</proxyDefault>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<encryptSettings>" & EveHQ.Core.HQ.EveHQSettings.EncryptSettings & "</encryptSettings>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<enableStatus>" & EveHQ.Core.HQ.EveHQSettings.EnableEveStatus & "</enableStatus>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<serverOffset>" & EveHQ.Core.HQ.EveHQSettings.ServerOffset & "</serverOffset>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<shutdownNotify>" & EveHQ.Core.HQ.EveHQSettings.ShutdownNotify & "</shutdownNotify>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<shutdownNotifyPeriod>" & EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod & "</shutdownNotifyPeriod>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifyToolTip>" & EveHQ.Core.HQ.EveHQSettings.NotifyToolTip & "</notifyToolTip>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifyDialog>" & EveHQ.Core.HQ.EveHQSettings.NotifyDialog & "</notifyDialog>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifyEMail>" & EveHQ.Core.HQ.EveHQSettings.NotifyEMail & "</notifyEMail>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifyOffset>" & EveHQ.Core.HQ.EveHQSettings.NotifyOffset & "</notifyOffset>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<emailServer>" & EveHQ.Core.HQ.EveHQSettings.EMailServer & "</emailServer>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<emailAddress>" & EveHQ.Core.HQ.EveHQSettings.EMailAddress & "</emailAddress>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<emailUseSMTPAuth>" & EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth & "</emailUseSMTPAuth>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<emailUsername>" & EveHQ.Core.HQ.EveHQSettings.EMailUsername & "</emailUsername>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<emailPassword>" & EveHQ.Core.HQ.EveHQSettings.EMailPassword & "</emailPassword>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<continueTraining>" & EveHQ.Core.HQ.EveHQSettings.ContinueTraining & "</continueTraining>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayBColor>" & EveHQ.Core.HQ.EveHQSettings.OverlayBorderColor & "</overlayBColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayFColor>" & EveHQ.Core.HQ.EveHQSettings.OverlayFontColor & "</overlayFColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayPColor>" & EveHQ.Core.HQ.EveHQSettings.OverlayPanelColor & "</overlayPColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayTransparancy>" & EveHQ.Core.HQ.EveHQSettings.OverlayTransparancy & "</overlayTransparancy>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayPosition>" & EveHQ.Core.HQ.EveHQSettings.OverlayPosition & "</overlayPosition>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayXOffset>" & EveHQ.Core.HQ.EveHQSettings.OverlayXOffset & "</overlayXOffset>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayYOffset>" & EveHQ.Core.HQ.EveHQSettings.OverlayYOffset & "</overlayYOffset>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayStartup>" & EveHQ.Core.HQ.EveHQSettings.OverlayStartup & "</overlayStartup>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<overlayClickThru>" & EveHQ.Core.HQ.EveHQSettings.OverlayClickThru & "</overlayClickThru>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<MDITabStyle>" & EveHQ.Core.HQ.EveHQSettings.MDITabStyle & "</MDITabStyle>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<minimiseOnExit>" & EveHQ.Core.HQ.EveHQSettings.MinimiseExit & "</minimiseOnExit>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<isPreReqColor>" & EveHQ.Core.HQ.EveHQSettings.IsPreReqColor & "</isPreReqColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<hasPreReqColor>" & EveHQ.Core.HQ.EveHQSettings.HasPreReqColor & "</hasPreReqColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<bothPreReqColor>" & EveHQ.Core.HQ.EveHQSettings.BothPreReqColor & "</bothPreReqColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<DTClashColor>" & EveHQ.Core.HQ.EveHQSettings.DTClashColor & "</DTClashColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<readySkillColor>" & EveHQ.Core.HQ.EveHQSettings.ReadySkillColor & "</readySkillColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifySound>" & EveHQ.Core.HQ.EveHQSettings.NotifySound & "</notifySound>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifySoundFile>" & EveHQ.Core.HQ.EveHQSettings.NotifySoundFile & "</notifySoundFile>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifyNow>" & EveHQ.Core.HQ.EveHQSettings.NotifyNow & "</notifyNow>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<notifyEarly>" & EveHQ.Core.HQ.EveHQSettings.NotifyEarly & "</notifyEarly>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<deleteSkills>" & EveHQ.Core.HQ.EveHQSettings.DeleteSkills & "</deleteSkills>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<partialTrainColor>" & EveHQ.Core.HQ.EveHQSettings.PartialTrainColor & "</partialTrainColor>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<dbDatabase>" & EveHQ.Core.HQ.EveHQSettings.DBName & "</dbDatabase>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<formLeft>" & EveHQ.Core.HQ.EveHQSettings.MainFormPosition(0) & "</formLeft>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<formTop>" & EveHQ.Core.HQ.EveHQSettings.MainFormPosition(1) & "</formTop>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<formWidth>" & EveHQ.Core.HQ.EveHQSettings.MainFormPosition(2) & "</formWidth>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<formHeight>" & EveHQ.Core.HQ.EveHQSettings.MainFormPosition(3) & "</formHeight>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<formState>" & EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4) & "</formState>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<autoAPI>" & EveHQ.Core.HQ.EveHQSettings.AutoAPI & "</autoAPI>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<activateG15>" & EveHQ.Core.HQ.EveHQSettings.ActivateG15 & "</activateG15>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<cycleG15Pilots>" & EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots & "</cycleG15Pilots>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<cycleG15Time>" & EveHQ.Core.HQ.EveHQSettings.CycleG15Time & "</cycleG15Time>" & vbCrLf
        For a As Integer = 1 To 4
            XMLS &= Chr(9) & Chr(9) & "<eveFolderLUA" & a & ">" & EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(a) & "</eveFolderLUA" & a & ">" & vbCrLf
        Next
        XMLS &= Chr(9) & Chr(9) & "<lastPriceUpdate>" & EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate & "</lastPriceUpdate>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelBackgroundColour>" & EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor & "</panelBackgroundColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelOutlineColour>" & EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor & "</panelOutlineColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelTopLeftColour>" & EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor & "</panelTopLeftColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelBottomRightColour>" & EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor & "</panelBottomRightColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelLeftColour>" & EveHQ.Core.HQ.EveHQSettings.PanelLeftColor & "</panelLeftColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelRightColour>" & EveHQ.Core.HQ.EveHQSettings.PanelRightColor & "</panelRightColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelTextColour>" & EveHQ.Core.HQ.EveHQSettings.PanelTextColor & "</panelTextColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<panelHighlightColour>" & EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor & "</panelHighlightColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotCurrentTrainSkillColour>" & EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor & "</pilotCurrentTrainSkillColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotLevel5SkillColour>" & EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor & "</pilotLevel5SkillColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotPartTrainedSkillColour>" & EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor & "</pilotPartTrainedSkillColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotStandardSkillColour>" & EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor & "</pilotStandardSkillColour>" & vbCrLf
        For a As Integer = 1 To 4
            XMLS &= Chr(9) & Chr(9) & "<eveFolderLabel" & a & ">" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(a) & "</eveFolderLabel" & a & ">" & vbCrLf
        Next
        XMLS &= Chr(9) & Chr(9) & "<APIRSPort>" & EveHQ.Core.HQ.EveHQSettings.APIRSPort & "</APIRSPort>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<APIRSAutostart>" & EveHQ.Core.HQ.EveHQSettings.APIRSAutoStart & "</APIRSAutostart>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<CCPAPIServer>" & EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress & "</CCPAPIServer>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<APIRSAddress>" & EveHQ.Core.HQ.EveHQSettings.APIRSAddress & "</APIRSAddress>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<useAPIRS>" & EveHQ.Core.HQ.EveHQSettings.UseAPIRS & "</useAPIRS>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<useCCPAPIBackup>" & EveHQ.Core.HQ.EveHQSettings.UseCCPAPIBackup & "</useCCPAPIBackup>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<updateURL>" & EveHQ.Core.HQ.EveHQSettings.UpdateURL & "</updateURL>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<omitCurrentSkill>" & EveHQ.Core.HQ.EveHQSettings.OmitCurrentSkill & "</omitCurrentSkill>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<useAPIStatusForm>" & EveHQ.Core.HQ.EveHQSettings.UseAPIStatusForm & "</useAPIStatusForm>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<useAppDirForDB>" & EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB & "</useAppDirForDB>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<APIFileExtension>" & EveHQ.Core.HQ.EveHQSettings.APIFileExtension & "</APIFileExtension>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<ECMDefaultLocation>" & EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation & "</ECMDefaultLocation>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<EMailPort>" & EveHQ.Core.HQ.EveHQSettings.EMailPort & "</EMailPort>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<TaskbarIconMode>" & EveHQ.Core.HQ.EveHQSettings.TaskbarIconMode & "</TaskbarIconMode>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<ErrorReportingEnabled>" & EveHQ.Core.HQ.EveHQSettings.ErrorReportingEnabled & "</ErrorReportingEnabled>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<ErrorReportingName>" & EveHQ.Core.HQ.EveHQSettings.ErrorReportingName & "</ErrorReportingName>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<ErrorReportingEmail>" & EveHQ.Core.HQ.EveHQSettings.ErrorReportingEmail & "</ErrorReportingEmail>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotGroupBackgroundColour>" & EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor & "</pilotGroupBackgroundColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotGroupTextColour>" & EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor & "</pilotGroupTextColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotPartSkillTextColour>" & EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor & "</pilotPartSkillTextColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<pilotSkillHighlightColour>" & EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor & "</pilotSkillHighlightColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<DBTimeout>" & EveHQ.Core.HQ.EveHQSettings.DBTimeout & "</DBTimeout>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<DBDataFilename>" & EveHQ.Core.HQ.EveHQSettings.DBDataFilename & "</DBDataFilename>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<DBDataServer>" & EveHQ.Core.HQ.EveHQSettings.DBDataName & "</DBDataServer>" & vbCrLf
        XMLS &= Chr(9) & "</general>" & vbCrLf

        ' Save the Plug-Ins details
        XMLS &= Chr(9) & "<plugins>" & vbCrLf
        For Each currentPlugin In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            XMLS &= Chr(9) & Chr(9) & "<plugin>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<pluginName>" & currentPlugin.Name & "</pluginName>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<pluginDisabled>" & currentPlugin.Disabled & "</pluginDisabled>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & "</plugin>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</plugins>" & vbCrLf

        ' Save Account details
        XMLS &= Chr(9) & "<accounts>" & vbCrLf
        For Each currentAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            XMLS &= Chr(9) & Chr(9) & "<account>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<userName>" & currentAccount.userID & "</userName>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<password>" & currentAccount.APIKey & "</password>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<accountName>" & System.Security.SecurityElement.Escape(currentAccount.FriendlyName) & "</accountName>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & "</account>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</accounts>" & vbCrLf

        ' Save Wanted List
        XMLS &= Chr(9) & "<wantedList>" & vbCrLf
        For Each item As String In EveHQ.Core.HQ.EveHQSettings.WantedList.Keys
            XMLS &= Chr(9) & Chr(9) & "<wanted>" & item & "</wanted>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</wantedList>" & vbCrLf

        ' Save EveHQ.Core.Pilot details
        XMLS &= Chr(9) & "<pilots>" & vbCrLf
        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            XMLS &= Chr(9) & Chr(9) & "<pilot>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<pilotName>" & currentPilot.Name & "</pilotName>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<pilotID>" & currentPilot.ID & "</pilotID>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<accountPOS>" & currentPilot.AccountPosition & "</accountPOS>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<accountName>" & currentPilot.Account & "</accountName>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<updated>" & currentPilot.Updated & "</updated>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<lastUpdate>" & currentPilot.LastUpdate & "</lastUpdate>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<useManualImplants>" & currentPilot.UseManualImplants & "</useManualImplants>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<cImplantM>" & currentPilot.CImplantM & "</cImplantM>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<iImplantM>" & currentPilot.IImplantM & "</iImplantM>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<mImplantM>" & currentPilot.MImplantM & "</mImplantM>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<pImplantM>" & currentPilot.PImplantM & "</pImplantM>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<wImplantM>" & currentPilot.WImplantM & "</wImplantM>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<isActive>" & currentPilot.Active & "</isActive>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & "</pilot>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</pilots>" & vbCrLf

        ' Save IGB details
        XMLS &= Chr(9) & "<IGB>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<port>" & EveHQ.Core.HQ.EveHQSettings.IGBPort & "</port>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<autostart>" & EveHQ.Core.HQ.EveHQSettings.IGBAutoStart & "</autostart>" & vbCrLf
        XMLS &= Chr(9) & "</IGB>" & vbCrLf

        ' Save FTP details
        XMLS &= Chr(9) & "<FTP>" & vbCrLf
        Dim myFTP As FTPAccount = New FTPAccount
        For Each myFTP In EveHQ.Core.HQ.EveHQSettings.FTPAccounts
            XMLS &= Chr(9) & Chr(9) & "<account>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<name>" & myFTP.FTPName & "</name>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<server>" & myFTP.Server & "</server>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<port>" & myFTP.Port & "</port>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<path>" & myFTP.Path & "</path>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<username>" & myFTP.Username & "</username>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<password>" & myFTP.Password & "</password>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "</account>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</FTP>" & vbCrLf

        XMLS &= "</EveHQSettings>"
        XMLdoc.LoadXml(XMLS)

        Try
            If EveHQ.Core.HQ.EveHQSettings.EncryptSettings = True Then
                'Encrypt the "EveHQSettings" element and all sub-elements
                Call EncryptSettingsXML(XMLdoc)
            End If

            XMLdoc.Save(EveHQ.Core.HQ.appDataFolder & "\EveHQSettings.xml")

        Catch e As Exception
            'Console.WriteLine(e.Message)
        Finally

        End Try

    End Sub
    Public Shared Sub SaveTraining()
        Dim currentPilot As New EveHQ.Core.Pilot
        Dim currentQueue As New EveHQ.Core.SkillQueue
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""

        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If currentPilot.TrainingQueues IsNot Nothing Then
                XMLS = ("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>") & vbCrLf
                XMLS &= "<training>" & vbCrLf
                For Each currentQueue In currentPilot.TrainingQueues.Values
                    XMLS &= Chr(9) & "<queue name=""" & HttpUtility.HtmlEncode(currentQueue.Name) & """ ICT=""" & currentQueue.IncCurrentTraining & """ primary=""" & currentQueue.Primary & """ >"
                    Dim mySkillQueue As EveHQ.Core.SkillQueueItem
                    For Each mySkillQueue In currentQueue.Queue
                        XMLS &= Chr(9) & "<skill>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<skillID>" & mySkillQueue.Name & "</skillID>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<fromLevel>" & mySkillQueue.FromLevel & "</fromLevel>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<toLevel>" & mySkillQueue.ToLevel & "</toLevel>" & vbCrLf
                        XMLS &= Chr(9) & Chr(9) & "<position>" & mySkillQueue.Pos & "</position>" & vbCrLf
                        XMLS &= Chr(9) & "</skill>" & vbCrLf
                    Next
                    XMLS &= Chr(9) & "</queue>"
                Next
                XMLS &= "</training>" & vbCrLf
                Try
                    XMLdoc.LoadXml(XMLS)
                    XMLdoc.Save(EveHQ.Core.HQ.dataFolder & "\Q_" & currentPilot.Name & ".xml")
                Catch e As Exception
                    MessageBox.Show(e.Message, "Error Saving Training Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End Try
            End If
        Next
    End Sub

    Public Shared Function LoadEveSettings() As Boolean
        Dim currentaccount As EveAccount = New EveAccount
        Dim XMLdoc As XmlDocument = New XmlDocument

        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.appDataFolder & "\EveHQSettings.xml") = True Then
            XMLdoc.Load(EveHQ.Core.HQ.appDataFolder & "\EveHQSettings.xml")
            Dim accountDetails As XmlNodeList
            Dim accountSettings As XmlNode
            ' Check if it encrypted
            accountDetails = XMLdoc.SelectNodes("/")
            accountSettings = accountDetails(0)
            If accountSettings.ChildNodes(1).Name = "EncryptedData" Then
                ' Decrypt the document element.
                DecryptSettingsXML(XMLdoc)
            End If

            EveHQ.Core.HQ.EveHQSettings.Accounts.Clear()
            EveHQ.Core.HQ.TPilots.Clear()

            ' Get the account details
            Try
                accountDetails = XMLdoc.SelectNodes("/EveHQSettings/accounts")
                ' Get the relevant node!
                accountSettings = accountDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If accountSettings.HasChildNodes Then
                    For account As Integer = 0 To accountSettings.ChildNodes.Count - 1
                        Dim newAccount As EveAccount = New EveAccount
                        newAccount.userID = accountSettings.ChildNodes(account).ChildNodes(0).InnerText
                        newAccount.APIKey = accountSettings.ChildNodes(account).ChildNodes(1).InnerText
                        If accountSettings.ChildNodes(account).ChildNodes.Count = 2 Then
                            newAccount.FriendlyName = newAccount.userID
                        Else
                            newAccount.FriendlyName = accountSettings.ChildNodes(account).ChildNodes(2).InnerText
                        End If
                        EveHQ.Core.HQ.EveHQSettings.Accounts.Add(newAccount, newAccount.userID)
                    Next
                End If
            Catch
            End Try

            ' Get the plugin details
            Try
                accountDetails = XMLdoc.SelectNodes("/EveHQSettings/plugins")
                ' Get the relevant node!
                accountSettings = accountDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If accountSettings.HasChildNodes Then
                    For account As Integer = 0 To accountSettings.ChildNodes.Count - 1
                        Dim newPlugin As PlugIn = New PlugIn
                        newPlugin.Name = accountSettings.ChildNodes(account).ChildNodes(0).InnerText
                        newPlugin.Disabled = CBool(accountSettings.ChildNodes(account).ChildNodes(1).InnerText)
                        newPlugin.Available = False
                        EveHQ.Core.HQ.EveHQSettings.Plugins.Add(newPlugin.Name, newPlugin)
                    Next
                End If
            Catch
            End Try


            ' Get the EveHQ.Core.Pilot details
            Try
                accountDetails = XMLdoc.SelectNodes("/EveHQSettings/pilots")
                ' Get the relevant node!
                accountSettings = accountDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/pilots node in each XML doc
                If accountSettings.HasChildNodes Then
                    For account As Integer = 0 To accountSettings.ChildNodes.Count - 1
                        Dim newpilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
                        Try
                            newpilot.Name = accountSettings.ChildNodes(account).ChildNodes(0).InnerText
                            newpilot.ID = accountSettings.ChildNodes(account).ChildNodes(1).InnerText
                            newpilot.AccountPosition = accountSettings.ChildNodes(account).ChildNodes(2).InnerText
                            newpilot.Account = accountSettings.ChildNodes(account).ChildNodes(3).InnerText
                            newpilot.Updated = CBool(accountSettings.ChildNodes(account).ChildNodes(4).InnerText)
                            newpilot.LastUpdate = accountSettings.ChildNodes(account).ChildNodes(5).InnerText
                            newpilot.UseManualImplants = CBool(accountSettings.ChildNodes(account).ChildNodes(6).InnerText)
                            newpilot.CImplantM = CInt(accountSettings.ChildNodes(account).ChildNodes(7).InnerText)
                            newpilot.IImplantM = CInt(accountSettings.ChildNodes(account).ChildNodes(8).InnerText)
                            newpilot.MImplantM = CInt(accountSettings.ChildNodes(account).ChildNodes(9).InnerText)
                            newpilot.PImplantM = CInt(accountSettings.ChildNodes(account).ChildNodes(10).InnerText)
                            newpilot.WImplantM = CInt(accountSettings.ChildNodes(account).ChildNodes(11).InnerText)
                            newpilot.Active = CBool(accountSettings.ChildNodes(account).ChildNodes(12).InnerText)
                        Catch
                        Finally
                            EveHQ.Core.HQ.TPilots.Add(newpilot, newpilot.Name)
                        End Try
                    Next
                End If
            Catch
            End Try

            ' Get the Wanted List details
            Try
                accountDetails = XMLdoc.SelectNodes("/EveHQSettings/wantedList")
                ' Get the relevant node!
                accountSettings = accountDetails(0)
                If accountSettings.HasChildNodes Then
                    EveHQ.Core.HQ.EveHQSettings.WantedList.Clear()
                    For item As Integer = 0 To accountSettings.ChildNodes.Count - 1
                        EveHQ.Core.HQ.EveHQSettings.WantedList.Add(accountSettings.ChildNodes(item).InnerText, accountSettings.ChildNodes(item).InnerText)
                    Next
                End If
            Catch
            End Try

            ' Get the General Settings
            Try
                accountDetails = XMLdoc.SelectNodes("/EveHQSettings/general")
                If accountDetails.Count <> 0 Then
                    ' Get the relevant node!
                    accountSettings = accountDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/pilots node in each XML doc
                    If accountSettings.HasChildNodes Then
                        EveHQ.Core.HQ.EveHQSettings.AutoHide = CBool(accountSettings.ChildNodes(0).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.AutoStart = CBool(accountSettings.ChildNodes(1).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.AutoMinimise = CBool(accountSettings.ChildNodes(2).InnerText)
                        For a As Integer = 1 To 4
                            EveHQ.Core.HQ.EveHQSettings.EveFolder(a) = accountSettings.ChildNodes(2 + a).InnerText
                        Next
                        EveHQ.Core.HQ.EveHQSettings.BackupAuto = CBool(accountSettings.ChildNodes(7).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.BackupStart = CDate(accountSettings.ChildNodes(8).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.BackupFreq = CInt(accountSettings.ChildNodes(9).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.BackupLast = CDate(accountSettings.ChildNodes(10).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.AutoCheck = CBool(accountSettings.ChildNodes(11).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.StartupPilot = accountSettings.ChildNodes(12).InnerText
                        EveHQ.Core.HQ.EveHQSettings.StartupView = accountSettings.ChildNodes(13).InnerText
                        EveHQ.Core.HQ.EveHQSettings.QColumnsSet = CBool(accountSettings.ChildNodes(14).InnerText)
                        For a As Integer = 0 To 16
                            EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0) = accountSettings.ChildNodes(15 + a).Attributes("name").Value
                            EveHQ.Core.HQ.EveHQSettings.QColumns(a, 1) = accountSettings.ChildNodes(15 + a).InnerText
                        Next
                        EveHQ.Core.HQ.EveHQSettings.DBFormat = CInt(accountSettings.ChildNodes(32).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.DBFilename = accountSettings.ChildNodes(33).InnerText
                        EveHQ.Core.HQ.EveHQSettings.DBServer = accountSettings.ChildNodes(34).InnerText
                        EveHQ.Core.HQ.EveHQSettings.DBUsername = accountSettings.ChildNodes(35).InnerText
                        EveHQ.Core.HQ.EveHQSettings.DBPassword = accountSettings.ChildNodes(36).InnerText
                        EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = CBool(accountSettings.ChildNodes(37).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ProxyRequired = CBool(accountSettings.ChildNodes(38).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ProxyServer = accountSettings.ChildNodes(39).InnerText
                        EveHQ.Core.HQ.EveHQSettings.ProxyPort = CInt(accountSettings.ChildNodes(40).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ProxyUsername = accountSettings.ChildNodes(41).InnerText
                        EveHQ.Core.HQ.EveHQSettings.ProxyPassword = accountSettings.ChildNodes(42).InnerText
                        EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = CBool(accountSettings.ChildNodes(43).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.EncryptSettings = CBool(accountSettings.ChildNodes(44).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = CBool(accountSettings.ChildNodes(45).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ServerOffset = CInt(accountSettings.ChildNodes(46).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ShutdownNotify = CBool(accountSettings.ChildNodes(47).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod = CInt(accountSettings.ChildNodes(48).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = CBool(accountSettings.ChildNodes(49).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifyDialog = CBool(accountSettings.ChildNodes(50).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifyEMail = CBool(accountSettings.ChildNodes(51).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifyOffset = CInt(accountSettings.ChildNodes(52).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.EMailServer = CStr(accountSettings.ChildNodes(53).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.EMailAddress = CStr(accountSettings.ChildNodes(54).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = CBool(accountSettings.ChildNodes(55).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.EMailUsername = CStr(accountSettings.ChildNodes(56).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.EMailPassword = CStr(accountSettings.ChildNodes(57).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ContinueTraining = CBool(accountSettings.ChildNodes(58).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayBorderColor = CLng(accountSettings.ChildNodes(59).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayFontColor = CLng(accountSettings.ChildNodes(60).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayPanelColor = CLng(accountSettings.ChildNodes(61).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayTransparancy = CInt(accountSettings.ChildNodes(62).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayPosition = CInt(accountSettings.ChildNodes(63).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayXOffset = CInt(accountSettings.ChildNodes(64).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayYOffset = CInt(accountSettings.ChildNodes(65).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayStartup = CBool(accountSettings.ChildNodes(66).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.OverlayClickThru = CBool(accountSettings.ChildNodes(67).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.MDITabStyle = CInt(accountSettings.ChildNodes(68).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.MinimiseExit = CBool(accountSettings.ChildNodes(69).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.IsPreReqColor = CLng(accountSettings.ChildNodes(70).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.HasPreReqColor = CLng(accountSettings.ChildNodes(71).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.BothPreReqColor = CLng(accountSettings.ChildNodes(72).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.DTClashColor = CLng(accountSettings.ChildNodes(73).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ReadySkillColor = CLng(accountSettings.ChildNodes(74).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifySound = CBool(accountSettings.ChildNodes(75).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifySoundFile = CStr(accountSettings.ChildNodes(76).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifyNow = CBool(accountSettings.ChildNodes(77).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.NotifyEarly = CBool(accountSettings.ChildNodes(78).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.DeleteSkills = CBool(accountSettings.ChildNodes(79).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PartialTrainColor = CLng(accountSettings.ChildNodes(80).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.DBName = CStr(accountSettings.ChildNodes(81).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.MainFormPosition(0) = CInt(accountSettings.ChildNodes(82).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.MainFormPosition(1) = CInt(accountSettings.ChildNodes(83).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.MainFormPosition(2) = CInt(accountSettings.ChildNodes(84).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.MainFormPosition(3) = CInt(accountSettings.ChildNodes(85).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.MainFormPosition(4) = CInt(accountSettings.ChildNodes(86).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.AutoAPI = CBool(accountSettings.ChildNodes(87).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ActivateG15 = CBool(accountSettings.ChildNodes(88).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = CBool(accountSettings.ChildNodes(89).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.CycleG15Time = CInt(accountSettings.ChildNodes(90).InnerText)
                        For a As Integer = 1 To 4
                            EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(a) = CBool(accountSettings.ChildNodes(90 + a).InnerText)
                        Next
                        EveHQ.Core.HQ.EveHQSettings.LastMarketPriceUpdate = CDate(accountSettings.ChildNodes(95).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor = CLng(accountSettings.ChildNodes(96).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor = CLng(accountSettings.ChildNodes(97).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor = CLng(accountSettings.ChildNodes(98).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor = CLng(accountSettings.ChildNodes(99).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelLeftColor = CLng(accountSettings.ChildNodes(100).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelRightColor = CLng(accountSettings.ChildNodes(101).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelTextColor = CLng(accountSettings.ChildNodes(102).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor = CLng(accountSettings.ChildNodes(103).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor = CLng(accountSettings.ChildNodes(104).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor = CLng(accountSettings.ChildNodes(105).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor = CLng(accountSettings.ChildNodes(106).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor = CLng(accountSettings.ChildNodes(107).InnerText)
                        For a As Integer = 1 To 4
                            EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(a) = accountSettings.ChildNodes(107 + a).InnerText
                        Next
                        EveHQ.Core.HQ.EveHQSettings.APIRSPort = CInt(accountSettings.ChildNodes(112).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.APIRSAutoStart = CBool(accountSettings.ChildNodes(113).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress = accountSettings.ChildNodes(114).InnerText
                        EveHQ.Core.HQ.EveHQSettings.APIRSAddress = accountSettings.ChildNodes(115).InnerText
                        EveHQ.Core.HQ.EveHQSettings.UseAPIRS = CBool(accountSettings.ChildNodes(116).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.UseCCPAPIBackup = CBool(accountSettings.ChildNodes(117).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.UpdateURL = accountSettings.ChildNodes(118).InnerText
                        EveHQ.Core.HQ.EveHQSettings.OmitCurrentSkill = CBool(accountSettings.ChildNodes(119).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.UseAPIStatusForm = CBool(accountSettings.ChildNodes(120).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = CBool(accountSettings.ChildNodes(121).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.APIFileExtension = accountSettings.ChildNodes(122).InnerText
                        EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation = accountSettings.ChildNodes(123).InnerText
                        EveHQ.Core.HQ.EveHQSettings.EMailPort = CInt(accountSettings.ChildNodes(124).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.TaskbarIconMode = CInt(accountSettings.ChildNodes(125).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ErrorReportingEnabled = CBool(accountSettings.ChildNodes(126).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.ErrorReportingName = accountSettings.ChildNodes(127).InnerText
                        EveHQ.Core.HQ.EveHQSettings.ErrorReportingEmail = accountSettings.ChildNodes(128).InnerText
                        EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor = CLng(accountSettings.ChildNodes(129).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor = CLng(accountSettings.ChildNodes(130).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor = CLng(accountSettings.ChildNodes(131).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor = CLng(accountSettings.ChildNodes(132).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.DBTimeout = CInt(accountSettings.ChildNodes(133).InnerText)
                        EveHQ.Core.HQ.EveHQSettings.DBDataFilename = accountSettings.ChildNodes(134).InnerText
                        EveHQ.Core.HQ.EveHQSettings.DBDataName = accountSettings.ChildNodes(135).InnerText
                    End If
                End If
            Catch
            Finally
                If EveHQ.Core.HQ.EveHQSettings.QColumnsSet = False Then
                    Call ResetColumns()
                End If
            End Try

            ' Get the IGB Settings
            Try
                accountDetails = XMLdoc.SelectNodes("/EveHQSettings/IGB")
                If accountDetails.Count <> 0 Then
                    ' Get the relevant node!
                    accountSettings = accountDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/pilots node in each XML doc
                    If accountSettings.HasChildNodes Then
                        EveHQ.Core.HQ.EveHQSettings.IGBPort = CInt(accountSettings.ChildNodes(0).InnerText)
                        If accountSettings.ChildNodes.Count > 1 Then
                            EveHQ.Core.HQ.EveHQSettings.IGBAutoStart = CBool(accountSettings.ChildNodes(1).InnerText)
                        End If
                    End If
                End If
            Catch
            End Try

            ' Get the FTP Settings
            Try
                accountDetails = XMLdoc.SelectNodes("/EveHQSettings/FTP")
                If accountDetails.Count <> 0 Then
                    ' Get the relevant node!
                    accountSettings = accountDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/pilots node in each XML doc
                    If accountSettings.HasChildNodes Then
                        For account As Integer = 0 To accountSettings.ChildNodes.Count - 1
                            Dim newFTP As FTPAccount = New FTPAccount
                            newFTP.FTPName = accountSettings.ChildNodes(account).ChildNodes(0).InnerText
                            newFTP.Server = accountSettings.ChildNodes(account).ChildNodes(1).InnerText
                            newFTP.Port = CInt(accountSettings.ChildNodes(account).ChildNodes(2).InnerText)
                            newFTP.Path = accountSettings.ChildNodes(account).ChildNodes(3).InnerText
                            newFTP.Username = accountSettings.ChildNodes(account).ChildNodes(4).InnerText
                            newFTP.Password = accountSettings.ChildNodes(account).ChildNodes(5).InnerText
                            EveHQ.Core.HQ.EveHQSettings.FTPAccounts.Add(newFTP, newFTP.FTPName)
                        Next
                    End If
                End If
            Catch
            End Try
        Else
            Dim msg As String = ""
            msg &= "EveHQ cannot find the settings file. The file should reside "
            msg &= "in the application directory." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "A new settings file has been created."
            ' Set the Queue Columns up!
            Call ResetColumns()
        End If

        ' Set the database connection string
        ' Determine if a database format has been chosen before and set it if not
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = -1 Then
            EveHQ.Core.HQ.EveHQSettings.DBFormat = 0
            EveHQ.Core.HQ.EveHQSettings.DBFilename = EveHQ.Core.HQ.appFolder & "\EveHQ.mdb"
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
                        .InitialDirectory = "c:\"
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

        Call EveHQ.Core.DataFunctions.SetEveHQConnectionString()
        Call EveHQ.Core.DataFunctions.SetEveHQDataConnectionString()

        ' Load the skill data before attempting to load in the EveHQ.Core.Pilot skill data
        If EveHQ.Core.SkillFunctions.LoadEveSkillData() = False Then
            Return False
            Exit Function
        End If

        ' Check for the existence of EveHQ.Core.Pilot data in the cache folder and load it
        Call EveHQ.Core.PilotParseFunctions.LoadPilotCachedInfo()

        Return True

    End Function
    Public Shared Sub LoadTraining()
        Dim currentPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""

        Dim trainingList, QueueList As XmlNodeList
        Dim trainingDetails, Queuedetails As XmlNode

        For Each currentPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            currentPilot.ActiveQueue = New EveHQ.Core.SkillQueue
            'currentPilot.ActiveQueue.Queue.Clear()
            currentPilot.TrainingQueues = New SortedList
            currentPilot.TrainingQueues.Clear()
            currentPilot.PrimaryQueue = ""
            If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.dataFolder & "\Q_" & currentPilot.Name & ".xml") = True Then
                Try
                    XMLdoc.Load(EveHQ.Core.HQ.dataFolder & "\Q_" & currentPilot.Name & ".xml")

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
                                        myskill.FromLevel = CInt(trainingDetails.ChildNodes(1).InnerText)
                                        myskill.ToLevel = CInt(trainingDetails.ChildNodes(2).InnerText)
                                        myskill.Pos = CInt(trainingDetails.ChildNodes(3).InnerText)
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

    Private Shared Sub ResetColumns()
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
    Shared Sub EncryptSettingsXML(ByVal Doc As XmlDocument)

        ' Create a new TripleDES key. 
        Dim tDESkey As New TripleDESCryptoServiceProvider()
        Dim keyString As String = "OMGWTFHEXPLOITBBQVESSPER"
        Dim keyBytes(23) As Byte
        For keyPart As Integer = 0 To Len(keyString) - 1
            keyBytes(keyPart) = CByte(Asc(keyString.Substring(keyPart, 1)))
        Next
        tDESkey.Key = keyBytes

        Dim ElementToEncrypt As String = "EveHQSettings"
        Dim Alg As SymmetricAlgorithm = tDESkey
        Dim KeyName As String = "tDESkey"

        ' Check the arguments.  
        If Doc Is Nothing Then
            Throw New ArgumentNullException("Doc")
        End If
        If ElementToEncrypt Is Nothing Then
            Throw New ArgumentNullException("ElementToEncrypt")
        End If
        If Alg Is Nothing Then
            Throw New ArgumentNullException("Alg")
        End If
        Dim elementEncrypt As XmlElement = CType(Doc.GetElementsByTagName(ElementToEncrypt)(0), XmlElement)

        ' Throw an XmlException if the element was not found.
        If ElementToEncrypt Is Nothing Then
            Throw New XmlException("The specified element was not found")
        End If

        Dim eXml As New EncryptedXml()

        ' Add the key mapping.
        eXml.AddKeyNameMapping(KeyName, Alg)

        ' Encrypt the element.
        Dim edElement As EncryptedData = eXml.Encrypt(elementEncrypt, KeyName)

        EncryptedXml.ReplaceElement(elementEncrypt, edElement, False)

        ' Clear the TripleDES key.
        tDESkey.Clear()

    End Sub                      'EncryptSettings
    Shared Sub DecryptSettingsXML(ByVal Doc As XmlDocument)

        'DecryptSettings(XMLdoc, tDESkey, "tDESkey")

        ' Create a new TripleDES key. 
        Dim tDESkey As New TripleDESCryptoServiceProvider()
        Dim keyString As String = "OMGWTFHEXPLOITBBQVESSPER"
        Dim keyBytes(23) As Byte
        For keyPart As Integer = 0 To Len(keyString) - 1
            keyBytes(keyPart) = CByte(Asc(keyString.Substring(keyPart, 1)))
        Next
        tDESkey.Key = keyBytes

        Dim Alg As SymmetricAlgorithm = tDESkey
        Dim KeyName As String = "tDESkey"

        ' Check the arguments.  
        If Doc Is Nothing Then
            Throw New ArgumentNullException("Doc")
        End If
        If Alg Is Nothing Then
            Throw New ArgumentNullException("Alg")
        End If
        If KeyName Is Nothing Then
            Throw New ArgumentNullException("KeyName")
        End If
        ' Create a new EncryptedXml object.
        Dim exml As New EncryptedXml(Doc)
        ' Add the key name mapping.
        exml.AddKeyNameMapping(KeyName, Alg)
        ' Decrypt the XML document.
        exml.DecryptDocument()
        tDESkey.Clear()
    End Sub                      'DecryptSettings

    Public Shared Sub SaveEveSettings2()
        ' Write a serial version?
        Dim s As New FileStream(EveHQ.Core.HQ.appDataFolder & "\EveHQSettings.bin", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, EveHQ.Core.HQ.EveHQSettings)
        s.Flush()
        s.Close()
    End Sub

    Public Shared Function LoadEveSettings2() As Boolean
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.appDataFolder & "\EveHQSettings.bin") = True Then
            Dim s As New FileStream(EveHQ.Core.HQ.appDataFolder & "\EveHQSettings.bin", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            EveHQ.Core.HQ.EveHQSettings = CType(f.Deserialize(s), EveSettings)
            s.Close()
        Else
            Return LoadEveSettings()
        End If

        ' Set the database connection string
        ' Determine if a database format has been chosen before and set it if not
        If EveHQ.Core.HQ.EveHQSettings.DBFormat = -1 Then
            EveHQ.Core.HQ.EveHQSettings.DBFormat = 0
            EveHQ.Core.HQ.EveHQSettings.DBFilename = EveHQ.Core.HQ.appFolder & "\EveHQ.mdb"
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
                        .InitialDirectory = "c:\"
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

        Call EveHQ.Core.DataFunctions.SetEveHQConnectionString()
        Call EveHQ.Core.DataFunctions.SetEveHQDataConnectionString()

        ' Load the skill data before attempting to load in the EveHQ.Core.Pilot skill data
        If EveHQ.Core.SkillFunctions.LoadEveSkillData() = False Then
            Return False
            Exit Function
        End If

        Return True

    End Function
End Class



