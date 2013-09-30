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
Imports System.Windows.Forms
Imports EveHQ.Common
Imports DevComponents.DotNetBar
Imports EveHQ.EveApi
Imports System.IO
Imports System.Linq
Imports EveHQ.Market
Imports EveHQ.Market.MarketServices
Imports EveHQ.Common.Logging
Imports EveHQ.Common.Extensions

Public Class HQ
    Private Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr,
                                                                              ByVal min As Int32, ByVal max As Int32) _
        As Boolean

    Public Shared MainForm As Form
    Public Shared TPilots As New SortedList(Of String, Pilot)
    Public Shared TCorps As New SortedList(Of String, Corporation)
    Private Shared _eveHQSettings As New EveSettings()
    Public Shared myIGB As New IGB
    Public Shared myTQServer As EveServer = New EveServer
    Public Shared SkillListName As New SortedList(Of String, EveSkill)
    Public Shared SkillListID As New SortedList(Of String, EveSkill)
    Public Shared SkillGroups As New SortedList(Of String, SkillGroup)
    Public Shared SkillUnlocks As New SortedList(Of String, ArrayList)
    Public Shared ItemUnlocks As New SortedList(Of String, ArrayList)
    Public Shared CertUnlockSkills As New SortedList(Of String, ArrayList)
    Public Shared CertUnlockCerts As New SortedList(Of String, ArrayList)
    Public Shared IsUsingLocalFolders As Boolean = False
    Public Shared IsSplashFormDisabled As Boolean = False
    Private Shared _appDataFolder As String = ""
    Public Shared appFolder As String = ""
    Public Shared cacheFolder As String = ""
    Public Shared coreCacheFolder As String = ""
    Public Shared imageCacheFolder As String = ""
    Public Shared reportFolder As String = ""
    Public Shared dataFolder As String = ""
    Public Shared backupFolder As String = ""
    Public Shared EveHQBackupFolder As String = ""
    Public Shared itemDBConnectionString As String = ""
    Public Shared EveHQDataConnectionString As String = ""
    Public Shared dataError As String = ""
    Public Shared IGBActive As Boolean = False
    Public Shared APIResults As New SortedList
    Public Shared APIErrors As New SortedList
    Public Shared itemList As New SortedList(Of String, String)
    Public Shared itemData As New SortedList(Of String, EveItem)
    Public Shared itemGroups As New SortedList(Of String, String)
    Public Shared itemCats As New SortedList(Of String, String)
    Public Shared groupCats As New SortedList(Of String, String)
    Public Shared LastAutoAPIResult As Boolean = True
    Public Shared NextAutoAPITime As DateTime = Now.AddMinutes(60)
    Public Shared AutoRetryAPITime As DateTime = Now.AddMinutes(5) ' Minimum retry time if an error occurs
    Public Shared EveHQLCD As New G15LCDv2
    Public Shared IsG15LCDActive As Boolean = False
    Public Shared lcdPilot As String = ""
    Public Shared lcdCharMode As Integer = 0
    Public Shared CustomPriceList As New SortedList(Of String, Double) ' TypeID, Price
    Public Shared APIUpdateAvailable As Boolean = False
    Public Shared AppUpdateAvailable As Boolean = False
    Public Shared CertificateCategories As New SortedList(Of String, CertificateCategory)
    Public Shared CertificateClasses As New SortedList(Of String, CertificateClass)
    Public Shared Certificates As New SortedList(Of String, Certificate)
    Public Shared FittingProtocol As String = "fitting"
    Public Shared NextAutoMailAPITime As DateTime = Now
    Public Shared Widgets As New SortedList(Of String, String)
    Public Shared Event ShutDownEveHQ()
    Public Shared UpdateShutDownRequest As Boolean = False
    Public Shared RemoteProxy As New RemoteProxyServer
    Public Shared Stations As New SortedList(Of String, Station)
    Private Shared _solarSystemsById As SortedList(Of String, SolarSystem)
    Private Shared _solarSystemsByName As SortedList(Of String, SolarSystem)
    Public Shared APIUpdateInProgress As Boolean = False
    Public Shared EveHQServerMessage As EveHQMessage
    Public Shared RestoredSettings As Boolean = False
    Public Shared BCAppKey As String = "B23079B49E1FCBB9C224C9D9CC591DF9904C193F"
    Public Shared EveHQAPIServerInfo As New APIServerInfo
    Public Shared EveHQIsUpdating As Boolean = False
    Public Shared ItemMarketGroups As New SortedList(Of String, String) ' TypeID, MarketGroupID
    Private Shared _marketStatDataProvider As IMarketStatDataProvider
    Private Shared _marketOrderDataProvider As IMarketOrderDataProvider
    Private Shared _regions As SortedList(Of String, EveGalaticRegion)
    Private Shared _marketCacheProcessorMinTime As DateTime = DateTime.Now.AddHours(-1)
    Private Shared _marketDataReceivers As IEnumerable(Of IMarketDataReceiver)
    Private Shared _marketCacheUploader As MarketUploader
    Private Shared _tickerItemList As New List(Of String)
    Private Shared _loggingStream As Stream
    Private Shared _eveHqTracer As EveHQTraceLogger
    Private Shared _proxyDetails As WebProxyDetails
    Private Shared _apiProvider As EveAPI.EveAPI

    Shared Sub New()

    End Sub

    Shared Property StartShutdownEveHQ() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent ShutDownEveHQ()
            End If
        End Set
    End Property

    Public Shared Property AppDataFolder As String
        Get
            Return _appDataFolder
        End Get
        Set(value As String)
            _appDataFolder = value
        End Set
    End Property



    Public Shared Property MarketStatDataProvider As IMarketStatDataProvider
        Get
            If _marketStatDataProvider Is Nothing Then
                ' Initialize based on settings
                If (EveHqSettings.MarketDataProvider = EveCentralMarketDataProvider.Name) Then
                    _marketStatDataProvider = GetEveCentralMarketInstance(HQ.AppDataFolder)
                Else
                    _marketStatDataProvider = GetEveHqMarketInstance(HQ.AppDataFolder)
                End If
            End If
            Return _marketStatDataProvider
        End Get
        Set(value As IMarketStatDataProvider)
            _marketStatDataProvider = value
        End Set
    End Property

    Public Shared ReadOnly Property Regions As SortedList(Of String, EveGalaticRegion)
        Get
            If _regions Is Nothing Then
                PopulateRegionList()
            End If
            Return _regions
        End Get

    End Property

    Public Shared Property SolarSystemsById As SortedList(Of String, SolarSystem)
        Get
            If (_solarSystemsById Is Nothing) Then
                _solarSystemsById = New SortedList(Of String, SolarSystem)
                DataFunctions.LoadSolarSystems()
            End If
            Return _solarSystemsById
        End Get
        Set(value As SortedList(Of String, SolarSystem))
            _solarSystemsById = value
        End Set
    End Property

    Public Shared ReadOnly Property SolarSystemsByName As SortedList(Of String, SolarSystem)
        Get
            If _solarSystemsByName Is Nothing Then
                _solarSystemsByName = New SortedList(Of String, SolarSystem)
                For Each solSystem As SolarSystem In SolarSystemsById.Values
                    _solarSystemsByName.Add(solSystem.Name, solSystem)
                Next
            End If

            Return _solarSystemsByName
        End Get
    End Property

    Public Shared Property EveHqSettings As EveSettings
        Get
            Return _eveHQSettings
        End Get
        Set(value As EveSettings)
            _eveHQSettings = value
        End Set
    End Property

    Public Shared Property MarketCacheUploader As MarketUploader
        Get
            If _marketCacheUploader Is Nothing Then
                _marketDataReceivers = {CType(GetEveCentralMarketInstance(HQ.AppDataFolder), IMarketDataReceiver), New EveMarketDataRelayProvider(New HttpRequestProvider(HQ.ProxyDetails))}
                _marketCacheUploader = New MarketUploader(_marketCacheProcessorMinTime, _marketDataReceivers, Nothing)
            End If

            Return _marketCacheUploader
        End Get
        Set(value As MarketUploader)
            _marketCacheUploader = value
        End Set
    End Property

    Public Shared Property TickerItemList As List(Of String)
        Get
            If (_tickerItemList.Count = 0) Then
                'Add place holder mineral types only
                _tickerItemList.Add("34")
                _tickerItemList.Add("35")
                _tickerItemList.Add("36")
                _tickerItemList.Add("37")
                _tickerItemList.Add("38")
                _tickerItemList.Add("39")
                _tickerItemList.Add("40")
                _tickerItemList.Add("11399")
            End If
            Return _tickerItemList
        End Get
        Set(value As List(Of String))
            _tickerItemList = value
        End Set
    End Property

    Public Shared Property MarketOrderDataProvider As IMarketOrderDataProvider
        Get
            If _marketOrderDataProvider Is Nothing Then
                _marketOrderDataProvider = GetEveCentralMarketInstance(HQ.AppDataFolder)
            End If
            Return _marketOrderDataProvider
        End Get
        Set(value As IMarketOrderDataProvider)
            _marketOrderDataProvider = value
        End Set
    End Property

    Public Shared Property LoggingStream As Stream
        Get
            Return _loggingStream
        End Get
        Set(value As Stream)
            _loggingStream = value
        End Set
    End Property

    Public Shared Property EveHqTracer As EveHQTraceLogger
        Get
            Return _eveHqTracer
        End Get
        Set(value As EveHQTraceLogger)
            _eveHqTracer = value
        End Set
    End Property

    Public Shared ReadOnly Property ProxyDetails As WebProxyDetails
        Get
            If (EveHqSettings.ProxyRequired) Then


                If _proxyDetails Is Nothing Then
                    _proxyDetails = New WebProxyDetails()
                    _proxyDetails.ProxyPassword = EveHqSettings.ProxyPassword
                    _proxyDetails.ProxyServerAddress = New Uri("{0}:{1}".FormatInvariant(EveHqSettings.ProxyServer, EveHqSettings.ProxyPort))
                    _proxyDetails.ProxyUserName = EveHqSettings.ProxyUsername
                    _proxyDetails.UseBasicAuth = EveHqSettings.ProxyUseBasic
                    _proxyDetails.UseDefaultCredential = EveHqSettings.ProxyUseDefault
                End If

                Return _proxyDetails
            End If
            Return Nothing
        End Get
    End Property

    Public Shared ReadOnly Property ApiProvider As EveApi.EveAPI
        Get
            If _apiProvider Is Nothing Then
                _apiProvider = New EveApi.EveAPI(Path.Combine(AppDataFolder, "ApiCache"), New HttpRequestProvider(ProxyDetails))
            End If
            Return _apiProvider
        End Get
    End Property

    Public Enum DBFormat As Integer
        SQLCE = 0
        MSSQL = 1
    End Enum

    Public Shared Sub ReduceMemory()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Try
            If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1)
            End If
        Catch ex As Exception
            ' Catch potential errors from remote loading routines
        End Try
    End Sub

    Public Shared Sub WriteLogEvent(ByVal EventText As String)
        Try
            Trace.WriteLine(EventText, "Information")
        Catch e As Exception
            ' Don't bother reporting this
        End Try
    End Sub

    Public Shared Function GetMDITab(ByVal TabName As String) As TabItem

        Dim mainTab As TabStrip = CType(MainForm.Controls("tabEveHQMDI"), TabStrip)
        For Each tp As TabItem In mainTab.Tabs
            If tp.Text = TabName Then
                Return tp
            End If
        Next
        Return Nothing
    End Function

    ' Gets the EveGalaticRegion list from the DB and stores it in a reusable collection
    Private Shared Sub PopulateRegionList()

        ' Get the data from the DB
        Dim regionSet As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT regionID, regionName FROM mapRegions where regionName <> 'Unknown' ORDER BY regionName;", True)
        If regionSet IsNot Nothing Then
            _regions = New SortedList(Of String, EveGalaticRegion)
            For Each regionRow As DataRow In regionSet.Tables(0).Rows
                Dim eveGalaticRegion As EveGalaticRegion = New EveGalaticRegion()
                eveGalaticRegion.Name = CStr(regionRow.Item("regionName"))
                eveGalaticRegion.Id = CInt(regionRow.Item("regionID"))
                _regions.Add(eveGalaticRegion.Name, eveGalaticRegion)
            Next
        End If
    End Sub

    Private Shared EveCentralProvider As EveCentralMarketDataProvider
    Public Shared Function GetEveCentralMarketInstance(appDataFolder As String) As EveCentralMarketDataProvider
        If EveCentralProvider Is Nothing Then
            If (EveHqSettings.ProxyRequired) Then
                EveCentralProvider = New EveCentralMarketDataProvider(Path.Combine(appDataFolder, "MarketCache\EveCentral"), New HttpRequestProvider(ProxyDetails))
            Else
                EveCentralProvider = New EveCentralMarketDataProvider(Path.Combine(appDataFolder, "MarketCache\EveCentral"), New HttpRequestProvider(Nothing))
            End If
        End If
        Return EveCentralProvider
    End Function

    Private Shared EveHqProvider As EveHQMarketDataProvider
    Public Shared Function GetEveHqMarketInstance(appDataFolder As String) As EveHQMarketDataProvider
        If EveHqProvider Is Nothing Then
            If (EveHqSettings.ProxyRequired) Then
                EveHqProvider = New EveHQMarketDataProvider(Path.Combine(appDataFolder, "MarketCache\EveHq"), New HttpRequestProvider(ProxyDetails))
            Else
                EveHqProvider = New EveHQMarketDataProvider(Path.Combine(appDataFolder, "MarketCache\EveHq"), New HttpRequestProvider(Nothing))
            End If
        End If
        Return EveHqProvider
    End Function

End Class

Class ListViewItemComparerA
    Implements IComparer
    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements IComparer.Compare
        Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
    End Function
End Class

Class ListViewItemComparerD
    Implements IComparer
    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements IComparer.Compare
        Return [String].Compare(CType(y, ListViewItem).SubItems(col).Text, CType(x, ListViewItem).SubItems(col).Text)
    End Function
End Class

Public Class ListViewItemComparer_Text
    Implements IComparer
    Private col As Integer
    Private order As SortOrder

    Public Sub New()
        col = 0
        order = SortOrder.Ascending
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As SortOrder)
        col = column
        Me.order = order
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1

        Try
            If _
                IsNumeric(CType(x, ListViewItem).SubItems(col).Text) = True And
                IsNumeric(CType(y, ListViewItem).SubItems(col).Text) = True Then
                ' Parse the two objects passed as a parameter as a DateTime.
                Dim firstNum As Double = CDbl((CType(x, ListViewItem).SubItems(col).Text))
                Dim secondNum As Double = CDbl((CType(y, ListViewItem).SubItems(col).Text))
                ' Compare the two numbers
                returnVal = Decimal.Compare(CDec(firstNum), CDec(secondNum))
                ' If neither compared object has a valid date format, 
                ' compare as a string.
            Else
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(col).Text,
                                             CType(y, ListViewItem).SubItems(col).Text)
            End If
        Catch
            ' Compare the two items as a string.
        End Try

        ' Determine whether the sort order is descending.
        If order = SortOrder.Descending Then
            ' Invert the value returned by String.Compare.
            returnVal *= -1
        End If
        Return returnVal
    End Function
End Class

Public Class ListViewItemComparer_Name
    Implements IComparer
    Private col As Integer
    Private order As SortOrder

    Public Sub New()
        col = 0
        order = SortOrder.Ascending
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As SortOrder)
        col = column
        Me.order = order
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1

        Try
            If _
                IsNumeric(CType(x, ListViewItem).SubItems(col).Name) = True And
                IsNumeric(CType(y, ListViewItem).SubItems(col).Name) = True Then
                ' Parse the two objects passed as a parameter as a DateTime.
                Dim firstNum As Double = CDbl((CType(x, ListViewItem).SubItems(col).Name))
                Dim secondNum As Double = CDbl((CType(y, ListViewItem).SubItems(col).Name))
                ' Compare the two numbers
                returnVal = Decimal.Compare(CDec(firstNum), CDec(secondNum))
                ' If neither compared object has a valid date format, 
                ' compare as a string.
            Else
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(col).Name,
                                             CType(y, ListViewItem).SubItems(col).Name)
            End If
        Catch
            ' Compare the two items as a string.
        End Try

        ' Determine whether the sort order is descending.
        If order = SortOrder.Descending Then
            ' Invert the value returned by String.Compare.
            returnVal *= -1
        End If
        Return returnVal
    End Function
End Class
