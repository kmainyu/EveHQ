﻿
Imports System.Drawing
Imports DevComponents.DotNetBar
Imports EveHQ.EveAPI
Imports EveHQ.Market
Imports System.IO
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Text
Imports EveHQ.Common.Extensions
Imports Newtonsoft.Json

''' <summary>
''' Class for the new EveHQ settings.
''' </summary>
''' <remarks></remarks>
<Serializable()>
Public Class EveHQSettings

#Region "Private Fields"
    Private _marketSystem As Integer
    Private _marketDataProvider As String
    Private _maxUpdateThreads As Integer
    Private _corporations As SortedList(Of String, Corporation)
    Private _priceGroups As SortedList(Of String, PriceGroup)
    Private _skillQueuePanelWidth As Integer
    Private _automaticSaveTime As Integer
    Private _sqlQueries As SortedList(Of String, String)
    Private _emailSenderAddress As String
    Private _userQueueColumns As ArrayList
    Private _standardQueueColumns As ArrayList
    Private _dashboardConfiguration As ArrayList
    Private _csvSeparatorChar As String
    Private _marketRegionList As ArrayList
    Private _priceCriteria(11) As Boolean
    Private _ccpApiServerAddress As String
    Private _eveFolderLabel(4) As String
    Private _eveFolderLua(4) As Boolean
    Private _mainFormPosition(4) As Integer
    Private _igbAllowedData As SortedList(Of String, Boolean)
    Private _eveFolder(4) As String
    Private _qColumns(20, 1) As String
    Private _marketStatOverrides As Dictionary(Of Integer, ItemMarketOverride)
    Private _marketRegions As List(Of Integer)
    Private _pilots As Dictionary(Of String, EveHQPilot)
    Private _accounts As Dictionary(Of String, EveHQAccount)
    Private _plugins As Dictionary(Of String, EveHQPlugInStatus)
#End Region

#Region "Constructors"

    Public Sub New()

        ' Initialise new settings
        Call InitialiseSettings()

    End Sub

#End Region

#Region "Private Constants"

    Private Const OfficialApiLocation As String = "https://api.eveonline.com"

#End Region

#Region "Public Properties"

    Public Property MarketDataProvider As String
        Get
            If _marketDataProvider.IsNullOrWhiteSpace() = True Then
                _marketDataProvider = MarketProviders.EveCentral.ToString()
            End If
            Return _marketDataProvider
        End Get
        Set(value As String)
            _marketDataProvider = value
        End Set
    End Property
    Public Property MaxUpdateThreads As Integer
        Get
            If _maxUpdateThreads = 0 Then _maxUpdateThreads = 5
            Return _maxUpdateThreads
        End Get
        Set(value As Integer)
            _maxUpdateThreads = value
        End Set
    End Property
    Public Property MarketDataSource As MarketSite
    Public Property Corporations As SortedList(Of String, Corporation)
        Get
            If _corporations Is Nothing Then
                _corporations = New SortedList(Of String, Corporation)
            End If
            Return _corporations
        End Get
        Set(value As SortedList(Of String, Corporation))
            _corporations = value
        End Set
    End Property
    Public Property PriceGroups As SortedList(Of String, PriceGroup)
        Get
            If _priceGroups Is Nothing Then
                _priceGroups = New SortedList(Of String, PriceGroup)
            End If
            Return _priceGroups
        End Get
        Set(ByVal value As SortedList(Of String, PriceGroup))
            _priceGroups = value
        End Set
    End Property
    Public Property SkillQueuePanelWidth() As Integer
        Get
            If _skillQueuePanelWidth = 0 Then
                _skillQueuePanelWidth = 440
            End If
            Return _skillQueuePanelWidth
        End Get
        Set(ByVal value As Integer)
            _skillQueuePanelWidth = value
        End Set
    End Property
    Public Property AccountTimeLimit() As Integer
    Public Property NotifyAccountTime() As Boolean
    Public Property NotifyInsuffClone() As Boolean
    Public Property StartWithPrimaryQueue() As Boolean
    Public Property IgnoreLastMessage() As Boolean
    Public Property LastMessageDate() As Date
    Public Property DisableTrainingBar() As Boolean
    Public Property EnableAutomaticSave() As Boolean
    Public Property AutomaticSaveTime() As Integer
        Get
            If _automaticSaveTime < 0 Then _automaticSaveTime = 60
            Return _automaticSaveTime
        End Get
        Set(ByVal value As Integer)
            _automaticSaveTime = value
        End Set
    End Property
    Public Property RibbonMinimised() As Boolean
    Public Property ThemeSetByUser() As Boolean
    Public Property ThemeTint() As Color
    Public Property ThemeStyle() As eStyle
    Public Property SQLQueries() As SortedList(Of String, String)
        Get
            If _sqlQueries Is Nothing Then
                _sqlQueries = New SortedList(Of String, String)
            End If
            Return _sqlQueries
        End Get
        Set(ByVal value As SortedList(Of String, String))
            _sqlQueries = value
        End Set
    End Property
    Public Property BackupBeforeUpdate() As Boolean
    Public Property QatLayout() As String
    Public Property NotifyEveNotification() As Boolean
    Public Property NotifyEveMail() As Boolean
    Public Property AutoMailAPI() As Boolean
    Public Property EveHqBackupWarnFreq() As Integer
    Public Property EveHqBackupMode() As Integer
    Public Property EveHqBackupStart() As Date
    Public Property EveHqBackupFreq() As Integer
    Public Property EveHqBackupLast() As Date
    Public Property EveHqBackupLastResult() As Integer
    Public Property IbShowAllItems() As Boolean
    Public Property EmailSenderAddress() As String
        Get
            If _emailSenderAddress Is Nothing Then
                _emailSenderAddress = "notifications@evehq.net"
            End If
            Return _emailSenderAddress
        End Get
        Set(ByVal value As String)
            If value IsNot Nothing Then
                _emailSenderAddress = value
            End If
        End Set
    End Property
    Public Property UserQueueColumns() As ArrayList
        Get
            If _userQueueColumns Is Nothing Then
                _userQueueColumns = New ArrayList
            End If
            Return _userQueueColumns
        End Get
        Set(ByVal value As ArrayList)
            _userQueueColumns = value
        End Set
    End Property
    Public Property StandardQueueColumns() As ArrayList
        Get
            If _standardQueueColumns Is Nothing Then
                _standardQueueColumns = New ArrayList
            End If
            Return _standardQueueColumns
        End Get
        Set(ByVal value As ArrayList)
            _standardQueueColumns = value
        End Set
    End Property
    Public Property DBTickerLocation() As String
    Public Property DBTicker() As Boolean
    Public Property DashboardConfiguration() As ArrayList
        Get
            If _dashboardConfiguration Is Nothing Then
                _dashboardConfiguration = New ArrayList
            End If
            Return _dashboardConfiguration
        End Get
        Set(ByVal value As ArrayList)
            _dashboardConfiguration = value
        End Set
    End Property
    Public Property CsvSeparatorChar() As String
        Get
            If _csvSeparatorChar Is Nothing Then
                _csvSeparatorChar = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator
            End If
            Return _csvSeparatorChar
        End Get
        Set(ByVal value As String)
            _csvSeparatorChar = value
        End Set
    End Property
    Public Property DisableVisualStyles() As Boolean
    Public Property DisableAutoWebConnections() As Boolean
    Public Property TrainingBarHeight() As Integer
    Public Property TrainingBarWidth() As Integer
    Public Property TrainingBarDockPosition() As Integer
    Public Property MdiTabPosition() As String
    Public Property ShowCompletedSkills() As Boolean
    Public Property MarketRegionList() As ArrayList
        Get
            If _marketRegionList Is Nothing Then
                _marketRegionList = New ArrayList
            End If
            Return _marketRegionList
        End Get
        Set(ByVal value As ArrayList)
            _marketRegionList = value
        End Set
    End Property
    Public Property IgnoreBuyOrderLimit() As Double
    Public Property IgnoreSellOrderLimit() As Double
    Public Property PriceCriteria(ByVal index As Integer) As Boolean
        Get
            If _priceCriteria Is Nothing Then
                ReDim _priceCriteria(11)
            End If
            Return _priceCriteria(index)
        End Get
        Set(ByVal value As Boolean)
            _priceCriteria(index) = value
        End Set
    End Property
    Public Property MarketLogUpdateData() As Boolean
    Public Property MarketLogUpdatePrice() As Boolean
    Public Property MarketLogPopupConfirm() As Boolean
    Public Property MarketLogToolTipConfirm() As Boolean
    Public Property IgnoreBuyOrders() As Boolean
    Public Property IgnoreSellOrders() As Boolean
    Public Property DBDataName() As String
    Public Property DBDataFilename() As String
    Public Property DBTimeout() As Integer
    Public Property PilotSkillHighlightColor() As Long
    Public Property PilotSkillTextColor() As Long
    Public Property PilotGroupTextColor() As Long
    Public Property PilotGroupBackgroundColor() As Long
    Public Property ErrorReportingEmail() As String
    Public Property ErrorReportingName() As String
    Public Property ErrorReportingEnabled() As Boolean
    Public Property TaskbarIconMode() As Integer
    Public Property EcmDefaultLocation() As String
    Public Property APIFileExtension() As String
    Public Property UseAppDirectoryForDB() As Boolean
    Public Property OmitCurrentSkill() As Boolean
    Public Property UpdateUrl() As String
    Public Property UseCcpapiBackup() As Boolean
    Public Property UseApirs() As Boolean
    Public Property ApirsAddress() As String
    Public Property CcpapiServerAddress() As String
        Get
            'Bug #75: Broken API update due to CCP disabling HTTP endpoint
            'Resolution: Now the code will check and forcibly update the api location to https 
            'Any time it is retrieved or saved.
            _ccpApiServerAddress = ForceHttpsOnCcpEndpoints(_ccpApiServerAddress)
            Return _ccpApiServerAddress
        End Get
        Set(ByVal value As String)
            _ccpApiServerAddress = ForceHttpsOnCcpEndpoints(value)
        End Set
    End Property
    Public Property EveFolderLabel(ByVal index As Integer) As String
        Get
            If _eveFolderLabel Is Nothing Then
                ReDim _eveFolderLabel(4)
            End If
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder Label index must be in the range 1 to 4", "Eve Folder Label Get Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return "0"
            Else
                Return _eveFolderLabel(index)
            End If
        End Get
        Set(ByVal value As String)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder Label index must be in the range 1 to 4", "Eve Folder Label Set Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _eveFolderLabel(index) = value
            End If
        End Set
    End Property
    Public Property PilotCurrentTrainSkillColor() As Long
    Public Property PilotPartTrainedSkillColor() As Long
    Public Property PilotLevel5SkillColor() As Long
    Public Property PilotStandardSkillColor() As Long
    Public Property PanelHighlightColor() As Long
    Public Property PanelTextColor() As Long
    Public Property PanelRightColor() As Long
    Public Property PanelLeftColor() As Long
    Public Property PanelBottomRightColor() As Long
    Public Property PanelTopLeftColor() As Long
    Public Property PanelOutlineColor() As Long
    Public Property PanelBackgroundColor() As Long
    Public Property LastMarketPriceUpdate() As DateTime
    Public Property LastFactionPriceUpdate() As DateTime
    Public Property EveFolderLua(ByVal index As Integer) As Boolean
        Get
            If _eveFolderLua Is Nothing Then
                ReDim _eveFolderLua(4)
            End If
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder LUA index must be in the range 1 to 4", "Eve Folder Get Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            Else
                Return _eveFolderLua(index)
            End If
        End Get
        Set(ByVal value As Boolean)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder LUA index must be in the range 1 to 4", "Eve Folder Set Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _eveFolderLua(index) = value
            End If
        End Set
    End Property
    Public Property CycleG15Time() As Integer
    Public Property CycleG15Pilots() As Boolean
    Public Property ActivateG15() As Boolean
    Public Property AutoAPI() As Boolean
    Public Property MainFormPosition(ByVal index As Integer) As Integer
        Get
            If _mainFormPosition Is Nothing Then
                ReDim _mainFormPosition(4)
            End If
            If index < 0 Or index > 4 Then
                MessageBox.Show("Eve Main Form Position index must be in the range 0 to 4",
                                "Eve Main Form Position Get Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return 0
            Else
                Return _mainFormPosition(index)
            End If
        End Get
        Set(ByVal value As Integer)
            If index < 0 Or index > 4 Then
                MessageBox.Show("Eve Main Form Position index must be in the range 0 to 4",
                                "Eve Main Form Position Set Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _mainFormPosition(index) = value
            End If
        End Set
    End Property
    Public Property DeleteSkills() As Boolean
    Public Property PartialTrainColor() As Long
    Public Property ReadySkillColor() As Long
    Public Property IsPreReqColor() As Long
    Public Property HasPreReqColor() As Long
    Public Property BothPreReqColor() As Long
    Public Property DtClashColor() As Long
    Public Property ColorHighlightQueuePreReq() As String
    Public Property ColorHighlightQueueTraining() As String
    Public Property ColorHighlightPilotTraining() As String
    Public Property ContinueTraining() As Boolean
    Public Property EMailPassword() As String
    Public Property EMailUsername() As String
    Public Property UseSsl() As Boolean
    Public Property UseSmtpAuth() As Boolean
    Public Property EMailAddress() As String
    Public Property EMailPort() As Integer
    Public Property EMailServer() As String
    Public Property NotifySoundFile() As String
    Public Property NotifyOffset() As Integer
    Public Property NotifyEarly() As Boolean
    Public Property NotifyNow() As Boolean
    Public Property NotifySound() As Boolean
    Public Property NotifyEMail() As Boolean
    Public Property NotifyDialog() As Boolean
    Public Property NotifyToolTip() As Boolean
    Public Property ShutdownNotifyPeriod() As Integer
    Public Property ShutdownNotify() As Boolean
    Public Property ServerOffset() As Integer
    Public Property EnableEveStatus() As Boolean
    Public Property ProxyUseDefault() As Boolean
    Public Property ProxyUseBasic() As Boolean
    Public Property ProxyPassword() As String
    Public Property ProxyUsername() As String
    Public Property ProxyPort() As Integer
    Public Property ProxyServer() As String
    Public Property ProxyRequired() As Boolean
    Public Property IgbPort() As Integer
    Public Property IgbAutoStart() As Boolean
    Public Property IgbFullMode() As Boolean
    Public Property IgbAllowedData() As SortedList(Of String, Boolean)
        Get
            If _igbAllowedData Is Nothing Then
                _igbAllowedData = New SortedList(Of String, Boolean)
                Call IGB.CheckAllIGBAccessRights()
            End If
            Return _igbAllowedData
        End Get
        Set(ByVal value As SortedList(Of String, Boolean))
            _igbAllowedData = value
        End Set
    End Property
    Public Property AutoHide() As Boolean
    Public Property AutoStart() As Boolean
    Public Property AutoCheck() As Boolean
    Public Property MinimiseExit() As Boolean
    Public Property AutoMinimise() As Boolean
    Public Property StartupPilot() As String
    Public Property StartupView() As String
    Public Property EveFolder(ByVal index As Integer) As String
        Get
            If _eveFolder Is Nothing Then
                ReDim _eveFolder(4)
            End If
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder index must be in the range 1 to 4", "Eve Folder Get Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return "0"
            Else
                Return _eveFolder(index)
            End If
        End Get
        Set(ByVal value As String)
            If index < 1 Or index > 4 Then
                MessageBox.Show("Eve Folder index must be in the range 1 to 4", "Eve Folder Set Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _eveFolder(index) = value
            End If
        End Set
    End Property
    Public Property BackupAuto() As Boolean
    Public Property BackupStart() As Date
    Public Property BackupFreq() As Integer
    Public Property BackupLast() As Date
    Public Property BackupLastResult() As Integer
    Public Property QColumnsSet() As Boolean
    Public Property QColumns(ByVal col As Integer, ByVal ref As Integer) As String
        Get
            If _qColumns Is Nothing Then
                ReDim _qColumns(20, 1)
            End If
            Return _qColumns(col, ref)
        End Get
        Set(ByVal value As String)
            If _qColumns.GetUpperBound(0) < 19 Then
                ReDim _qColumns(20, 1)
            End If
            _qColumns(col, ref) = value
        End Set
    End Property
    Public Property DBFormat() As Integer
    Public Property DBFilename() As String
    Public Property DBName() As String
    Public Property DBServer() As String
    Public Property DBUsername() As String
    Public Property DBPassword() As String
    Public Property DbSqlSecurity() As Boolean
    Public Property Accounts() As Dictionary(Of String, EveHQAccount)
        Get
            If _accounts Is Nothing Then
                _accounts = New Dictionary(Of String, EveHQAccount)
            End If
            Return _accounts
        End Get
        Set(ByVal value As Dictionary(Of String, EveHQAccount))
            _accounts = value
        End Set
    End Property
    Public Property Plugins() As Dictionary(Of String, EveHQPlugInStatus)
        Get
            If _plugins Is Nothing Then
                _plugins = New Dictionary(Of String, EveHQPlugInStatus)
            End If
            Return _plugins
        End Get
        Set(ByVal value As Dictionary(Of String, EveHQPlugInStatus))
            _plugins = value
        End Set
    End Property
    Public Property Pilots() As Dictionary(Of String, EveHQPilot)
        Get
            If _pilots Is Nothing Then
                _pilots = New Dictionary(Of String, EveHQPilot)
            End If
            Return _pilots
        End Get
        Set(ByVal value As Dictionary(Of String, EveHQPilot))
            _pilots = value
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
    Public Property MarketDefaultMetric As MarketMetric
    Public Property MarketDataUploadEnabled As Boolean
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

#End Region

    Private Sub InitialiseSettings()
        ' Initialise settings that are non-default
        IgbPort = 26001
        AutoHide = True
        BackupStart = Now
        BackupFreq = 1
        BackupLast = New DateTime(1999, 1, 1)
        DBFormat = -1
        DBName = "EveHQ"
        ProxyUseDefault = True
        ShutdownNotifyPeriod = 8
        EMailPort = 25
        IsPreReqColor = Color.LightSteelBlue.ToArgb
        HasPreReqColor = Color.White.ToArgb
        BothPreReqColor = Color.White.ToArgb
        DtClashColor = Color.Red.ToArgb
        ReadySkillColor = Color.White.ToArgb
        PartialTrainColor = Color.White.ToArgb
        CycleG15Time = 15
        PanelBackgroundColor = Color.Navy.ToArgb
        PanelOutlineColor = Color.SteelBlue.ToArgb
        PanelTopLeftColor = Color.LightSteelBlue.ToArgb
        PanelBottomRightColor = Color.LightSteelBlue.ToArgb
        PanelLeftColor = Color.RoyalBlue.ToArgb
        PanelRightColor = Color.LightSteelBlue.ToArgb
        PanelTextColor = Color.Black.ToArgb
        PanelHighlightColor = Color.LightSteelBlue.ToArgb
        PilotStandardSkillColor = Color.White.ToArgb
        PilotLevel5SkillColor = Color.Thistle.ToArgb
        PilotPartTrainedSkillColor = Color.Gold.ToArgb
        PilotCurrentTrainSkillColor = Color.LimeGreen.ToArgb
        CcpapiServerAddress = OfficialApiLocation
        UpdateUrl = "http://evehq.net/update/"
        APIFileExtension = "aspx"
        PilotGroupBackgroundColor = Color.DimGray.ToArgb
        PilotGroupTextColor = Color.White.ToArgb
        PilotSkillTextColor = Color.Black.ToArgb
        PilotSkillHighlightColor = Color.DodgerBlue.ToArgb
        DBTimeout = 30
        IgnoreBuyOrders = True
        IgnoreSellOrderLimit = 1000
        IgnoreBuyOrderLimit = 1
        MdiTabPosition = "Top"
        TrainingBarHeight = 54
        TrainingBarWidth = 100
        CsvSeparatorChar = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator
        DBTickerLocation = "Bottom"
        EmailSenderAddress = "notifications@evehq.net"
        EveHqBackupStart = Now
        EveHqBackupFreq = 1
        EveHqBackupLast = New DateTime(1999, 1, 1)
        EveHqBackupWarnFreq = 1
        ThemeStyle = eStyle.Office2007Black
        ThemeTint = Color.Empty
        AutomaticSaveTime = 60
        LastMessageDate = New DateTime(1999, 1, 1)
        AccountTimeLimit = 168
        SkillQueuePanelWidth = 440
        MarketSystem = 30000142 'Safe Default of Jita
        MaxUpdateThreads = 5

    End Sub

    ''' <summary>
    '''     Validates that when "official" CCP api endpoints are used, the http scheme is forced to https.
    '''     Also the older eve-online.com domain will be changed to eveonline.com
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Function ForceHttpsOnCcpEndpoints(endpoint As String) As String
        Const oldApi1 As String = "http://api.eveonline.com"
        Const oldApi2 As String = "http://api.eve-online.com"
        Const badApi As String = "https://api.eve-online.com" 'this https endpoint isn't supported anymore

        Dim normalizedEndpoint As String = endpoint.ToLowerInvariant()

        If (normalizedEndpoint = oldApi1 Or normalizedEndpoint = oldApi2 Or normalizedEndpoint = badApi) Then
            normalizedEndpoint = OfficialApiLocation
        End If

        Return normalizedEndpoint
    End Function

   Public Shared Sub ResetColumns()
        HQ.Settings.QColumns(0, 0) = "Name"
        HQ.Settings.QColumns(0, 1) = CStr(True)
        HQ.Settings.QColumns(1, 0) = "Curr"
        HQ.Settings.QColumns(1, 1) = CStr(True)
        HQ.Settings.QColumns(2, 0) = "From"
        HQ.Settings.QColumns(2, 1) = CStr(True)
        HQ.Settings.QColumns(3, 0) = "Tole"
        HQ.Settings.QColumns(3, 1) = CStr(True)
        HQ.Settings.QColumns(4, 0) = "Perc"
        HQ.Settings.QColumns(4, 1) = CStr(True)
        HQ.Settings.QColumns(5, 0) = "Trai"
        HQ.Settings.QColumns(5, 1) = CStr(True)
        HQ.Settings.QColumns(6, 0) = "Comp"
        HQ.Settings.QColumns(6, 1) = CStr(True)
        HQ.Settings.QColumns(7, 0) = "Date"
        HQ.Settings.QColumns(7, 1) = CStr(True)
        HQ.Settings.QColumns(8, 0) = "Rank"
        HQ.Settings.QColumns(8, 1) = CStr(False)
        HQ.Settings.QColumns(9, 0) = "PAtt"
        HQ.Settings.QColumns(9, 1) = CStr(False)
        HQ.Settings.QColumns(10, 0) = "SAtt"
        HQ.Settings.QColumns(10, 1) = CStr(False)
        HQ.Settings.QColumns(11, 0) = "SPRH"
        HQ.Settings.QColumns(11, 1) = CStr(False)
        HQ.Settings.QColumns(12, 0) = "SPRD"
        HQ.Settings.QColumns(12, 1) = CStr(False)
        HQ.Settings.QColumns(13, 0) = "SPRW"
        HQ.Settings.QColumns(13, 1) = CStr(False)
        HQ.Settings.QColumns(14, 0) = "SPRM"
        HQ.Settings.QColumns(14, 1) = CStr(False)
        HQ.Settings.QColumns(15, 0) = "SPRY"
        HQ.Settings.QColumns(15, 1) = CStr(False)
        HQ.Settings.QColumns(16, 0) = "SPAd"
        HQ.Settings.QColumns(16, 1) = CStr(False)
        HQ.Settings.QColumns(17, 0) = "SPTo"
        HQ.Settings.QColumns(17, 1) = CStr(False)
        HQ.Settings.QColumns(18, 0) = "Note"
        HQ.Settings.QColumns(18, 1) = CStr(False)
        HQ.Settings.QColumns(19, 0) = "Prio"
        HQ.Settings.QColumns(19, 1) = CStr(False)
        HQ.Settings.QColumnsSet = True
    End Sub

    Public Sub Save()

        Dim fileName As String = Path.Combine(HQ.AppDataFolder, "EveHQSettings.json")
        HQ.WriteLogEvent("Settings: Saving EveHQ settings to " & fileName)

        ' Convert the current settings to a JSON formatted string
        Dim json As String = JsonConvert.SerializeObject(Me, Newtonsoft.Json.Formatting.Indented)

        ' Write the JSON string to the file
        Try
            Using s As New StreamWriter(fileName, False)
                s.Write(json)
                s.Flush()
            End Using
        Catch e As Exception
            HQ.WriteLogEvent("Settings: Error saving EveHQ settings to " & Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin - " & e.Message))
        End Try

        ' Update the Proxy Server settings
        Call InitialiseRemoteProxyServer()

        ' Set Global APIServerInfo
        HQ.EveHQAPIServerInfo = New APIServerInfo(HQ.Settings.CCPAPIServerAddress, HQ.Settings.APIRSAddress,
                                                  HQ.Settings.UseAPIRS, HQ.Settings.UseCCPAPIBackup)

    End Sub

    Public Shared Function Load(showRawData As Boolean) As EveHQSettings

        Dim tempSettings As New EveHQSettings

        If My.Computer.FileSystem.FileExists(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json")) = True Then
            Try
                Using s As New StreamReader(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"))
                    Dim json As String = s.ReadToEnd
                    tempSettings = JsonConvert.DeserializeObject(Of EveHQSettings)(json)
                End Using

            Catch ex As Exception
                Trace.TraceError(ex.FormatException())
                Dim msg As String = "There was an error trying to load the settings file and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                msg &= "The error was: " & ex.Message & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Stacktrace: " & ex.StackTrace & ControlChars.CrLf & ControlChars.CrLf
                msg &= "EveHQ will copy this file to 'EveHQSettings.bad' and delete the original file and re-initialise the settings. This means you will need to re-enter your API information but your production and fittings data should be intact and available once the API data has been downloaded. You can attempt to reload the old settings by renaming the 'EveHQSettings.bad' file to 'EveHQSettings.bin', however if the issue continues the bad file will be useful to the EveHQ team for debugging purposes" & ControlChars.CrLf & ControlChars.CrLf
                msg &= "Press OK to reset the settings." & ControlChars.CrLf
                MessageBox.Show(msg, "Invalid Settings file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Try
                    My.Computer.FileSystem.CopyFile(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"), Path.Combine(HQ.AppDataFolder, "EveHQSettings.bad"), True)
                Catch e As Exception
                    MessageBox.Show("Unable to delete the EveHQSettings.json file. Please delete this manually before proceeding", "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Application.Exit()
                End Try
                Return Nothing
            End Try
        End If

        If showRawData = False Then

            ' Reset the update URL to a temp location
            If tempSettings.UpdateUrl <> "http://evehq.net/update/" Then
                tempSettings.UpdateUrl = "http://evehq.net/update/"
            End If

            ' Set the database connection string
            ' Determine if a database format has been chosen before and set it if not
            ' TODO: Delete this block when we rip out the static DB
            If tempSettings.DBFormat = -1 Then
                tempSettings.DBFormat = 0
                tempSettings.DBFilename = Path.Combine(HQ.AppDataFolder, "EveHQ.sdf")
                ' Check for this file!
                Dim fileExists As Boolean = False
                Do
                    If My.Computer.FileSystem.FileExists(tempSettings.DBFilename) = False Then
                        Dim msg As String = "EveHQ needs a database in order to work correctly." & ControlChars.CrLf
                        msg &= "If you do not select a valid DB file, EveHQ will exit." & ControlChars.CrLf &
                               ControlChars.CrLf
                        msg &= "Would you like to select a file now?" & ControlChars.CrLf
                        Dim reply As Integer = MessageBox.Show(msg, "Database Required", MessageBoxButtons.YesNo,
                                                               MessageBoxIcon.Question)
                        If reply = DialogResult.No Then
                            Return Nothing
                        End If
                        Dim ofd1 As New OpenFileDialog
                        With ofd1
                            .Title = "Select SQL CE Data file"
                            .FileName = ""
                            .InitialDirectory = HQ.appFolder
                            .Filter = "SQL CE Data files (*.sdf)|*.sdf|All files (*.*)|*.*"
                            .FilterIndex = 1
                            .RestoreDirectory = True
                            If .ShowDialog() = DialogResult.OK Then
                                tempSettings.DBFilename = .FileName
                            End If
                        End With
                    Else
                        fileExists = True
                    End If
                Loop Until fileExists = True
                tempSettings.DBUsername = ""
                tempSettings.DBPassword = ""
            End If

            ' TODO: Reword this when we rip out SQLCE
            Try
                If DataFunctions.SetEveHQConnectionString() = False Then
                    Return Nothing
                End If
                If DataFunctions.SetEveHQDataConnectionString() = False Then
                    Return Nothing
                End If
            Catch ex As Exception
                Dim msg As New StringBuilder
                msg.AppendLine("Error: " & ex.Message)
                msg.AppendLine("")
                msg.AppendLine(
                    "An error occurred trying to access the database, with the most common cause being that SQL Compact Edition v4 was not installed as instructed.")
                msg.AppendLine("")
                msg.AppendLine(
                    "Click OK to close EveHQ where you will be redirected to the SQL Compact Edition download page at http://www.microsoft.com/download/en/details.aspx?id=17876")
                MessageBox.Show(msg.ToString, "Error Initialising Database", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Try
                    Process.Start("http://www.microsoft.com/download/en/details.aspx?id=17876")
                    Application.ExitThread()
                Catch ex2 As Exception
                    ' Do nothing - users have the link
                End Try
            End Try

            ' Load the skill data before attempting to load in the EveHQ.Core.Pilot skill data
            ' TODO: Why this? Skill data should be loaded via the main startup, not dependant on settings
            If SkillFunctions.LoadEveSkillData() = False Then
                Return Nothing
            End If

            '  Setup queue columns etc
            Call InitialiseQueueColumns()
            Call InitialiseUserColumns()
            Call InitialiseRemoteProxyServer()
            If tempSettings.QColumns(0, 0) Is Nothing Then
                Call ResetColumns()
            End If

            ' Set Theme stuff
            If tempSettings.ThemeSetByUser = False Then
                tempSettings.ThemeStyle = eStyle.Office2007Black
                tempSettings.ThemeTint = Color.Empty
            End If

            ' Set up a global price list if not present
            If tempSettings.PriceGroups.ContainsKey("<Global>") = False Then
                ' Add a new price group
                Dim newPg As New PriceGroup
                newPg.Name = "<Global>"
                newPg.RegionIDs.Add("10000002")
                newPg.PriceFlags = PriceGroupFlags.MinSell
                tempSettings.PriceGroups.Add(newPg.Name, newPg)
            End If

            ' Set Global APIServerInfo
            HQ.EveHQAPIServerInfo = New APIServerInfo(tempSettings.CcpapiServerAddress, tempSettings.ApirsAddress, tempSettings.UseApirs, tempSettings.UseCcpapiBackup)

        End If

        Return tempSettings

    End Function

    Public Shared Sub InitialiseRemoteProxyServer()
        HQ.RemoteProxy.ProxyRequired = HQ.Settings.ProxyRequired
        HQ.RemoteProxy.ProxyServer = HQ.Settings.ProxyServer
        HQ.RemoteProxy.ProxyPort = HQ.Settings.ProxyPort
        HQ.RemoteProxy.UseDefaultCredentials = HQ.Settings.ProxyUseDefault
        HQ.RemoteProxy.ProxyUsername = HQ.Settings.ProxyUsername
        HQ.RemoteProxy.ProxyPassword = HQ.Settings.ProxyPassword
        HQ.RemoteProxy.UseBasicAuthentication = HQ.Settings.ProxyUseBasic
    End Sub

    Public Shared Sub InitialiseQueueColumns()
        HQ.Settings.StandardQueueColumns.Clear()
        Dim newItem As ListViewItem
        'newItem = New ListViewItem
        'newItem.Name = "Name"
        'newItem.Text = "Skill Name"
        'newItem.Checked = True
        'EveHQ.Core.HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Current"
        newItem.Text = "Cur Lvl"
        newItem.Checked = True
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "From"
        newItem.Text = "From Lvl"
        newItem.Checked = True
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "To"
        newItem.Text = "To Lvl"
        newItem.Checked = True
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Percent"
        newItem.Text = "%"
        newItem.Checked = True
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "TrainTime"
        newItem.Text = "Training Time"
        newItem.Checked = True
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "TimeToComplete"
        newItem.Text = "Time To Complete"
        newItem.Checked = True
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "DateEnded"
        newItem.Text = "Date Completed"
        newItem.Checked = True
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Rank"
        newItem.Text = "Rank"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "PAtt"
        newItem.Text = "Pri Att"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SAtt"
        newItem.Text = "Sec Att"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPHour"
        newItem.Text = "SP /hour"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPDay"
        newItem.Text = "SP /day"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPWeek"
        newItem.Text = "SP /week"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPMonth"
        newItem.Text = "SP /month"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPYear"
        newItem.Text = "SP /year"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPAdded"
        newItem.Text = "SP Added"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "SPTotal"
        newItem.Text = "SP Total"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Notes"
        newItem.Text = "Notes"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
        newItem = New ListViewItem
        newItem.Name = "Priority"
        newItem.Text = "Priority"
        newItem.Checked = False
        HQ.Settings.StandardQueueColumns.Add(newItem)
    End Sub

    Public Shared Sub InitialiseUserColumns()
        If HQ.Settings.UserQueueColumns.Count = 0 Then
            ' Add preset items
            HQ.Settings.UserQueueColumns.Add("Current1")
            HQ.Settings.UserQueueColumns.Add("From1")
            HQ.Settings.UserQueueColumns.Add("To1")
            HQ.Settings.UserQueueColumns.Add("Percent1")
            HQ.Settings.UserQueueColumns.Add("TrainTime1")
            HQ.Settings.UserQueueColumns.Add("TimeToComplete1")
            HQ.Settings.UserQueueColumns.Add("DateEnded1")
        End If
        ' Check if the standard columns have changed and we need to add columns
        If HQ.Settings.UserQueueColumns.Count <> HQ.Settings.StandardQueueColumns.Count Then
            For Each slotItem As ListViewItem In HQ.Settings.StandardQueueColumns
                If _
                    HQ.Settings.UserQueueColumns.Contains(slotItem.Name & "0") = False And
                    HQ.Settings.UserQueueColumns.Contains(slotItem.Name & "1") = False Then
                    HQ.Settings.UserQueueColumns.Add(slotItem.Name & "0")
                End If
            Next
        End If
    End Sub

End Class

