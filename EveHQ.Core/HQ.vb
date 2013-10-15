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
Imports System.Windows.Forms
Imports EveHQ.Common
Imports DevComponents.DotNetBar
Imports EveHQ.EveAPI
Imports EveHQ.Market
Imports System.IO
Imports EveHQ.Common.Logging
Imports EveHQ.Market.MarketServices

Public Class HQ
    Private Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr, ByVal min As Int32, ByVal max As Int32) As Boolean

    Public Shared MainForm As Form
    Public Shared TempPilots As New SortedList(Of String, EveHQPilot)
    Public Shared TempCorps As New SortedList(Of String, Corporation)
    Public Shared MyIGB As New IGB
    Public Shared MyTqServer As EveServer = New EveServer
    Public Shared SkillListName As New SortedList(Of String, EveSkill) ' SkillName, EveSkill
    Public Shared SkillListID As New SortedList(Of Integer, EveSkill) ' SkillID, EveSkill
    Public Shared SkillGroups As New SortedList(Of String, SkillGroup)
    Public Shared IsUsingLocalFolders As Boolean = False
    Public Shared IsSplashFormDisabled As Boolean = False
    Private Shared _appDataFolder As String = ""
    Public Shared AppFolder As String = ""
    Public Shared CacheFolder As String = ""
    Public Shared CoreCacheFolder As String = ""
    Public Shared ImageCacheFolder As String = ""
    Public Shared ReportFolder As String = ""
    Public Shared DataFolder As String = ""
    Public Shared BackupFolder As String = ""
    Public Shared EveHQBackupFolder As String = ""
    Public Shared ItemDBConnectionString As String = ""
    Public Shared EveHQDataConnectionString As String = ""
    Public Shared DataError As String = ""
    Public Shared IGBActive As Boolean = False
    Public Shared APIResults As New SortedList
    Public Shared APIErrors As New SortedList
    Public Shared LastAutoAPIResult As Boolean = True
    Public Shared NextAutoAPITime As DateTime = Now.AddMinutes(60)
    Public Shared AutoRetryAPITime As DateTime = Now.AddMinutes(5) ' Minimum retry time if an error occurs
    Public Shared EveHqlcd As New G15Lcd
    Public Shared IsG15LcdActive As Boolean = False
    Public Shared LcdPilot As String = ""
    Public Shared LcdCharMode As Integer = 0
    Public Shared CustomPriceList As New SortedList(Of Integer, Double) ' TypeID, Price
    Public Shared APIUpdateAvailable As Boolean = False
    Public Shared AppUpdateAvailable As Boolean = False
    Public Shared FittingProtocol As String = "fitting"
    Public Shared NextAutoMailAPITime As DateTime = Now
    Public Shared Widgets As New SortedList(Of String, String)
    Public Shared Event ShutDownEveHQ()
    Public Shared UpdateShutDownRequest As Boolean = False
    Public Shared RemoteProxy As New RemoteProxyServer
    Public Shared APIUpdateInProgress As Boolean = False
    Public Shared EveHQServerMessage As EveHQMessage
    Public Shared RestoredSettings As Boolean = False
    Public Shared BcAppKey As String = "B23079B49E1FCBB9C224C9D9CC591DF9904C193F"
    Public Shared EveHqapiServerInfo As New APIServerInfo
    Public Shared EveHQIsUpdating As Boolean = False
    Private Shared _marketStatDataProvider As IMarketStatDataProvider
    Private Shared _marketOrderDataProvider As IMarketOrderDataProvider
    Private Shared ReadOnly MarketCacheProcessorMinTime As DateTime = DateTime.Now.AddHours(-1)
    Private Shared _marketDataReceivers As IEnumerable(Of IMarketDataReceiver)
    Private Shared _marketCacheUploader As MarketUploader
    Private Shared _tickerItemList As New List(Of Integer)
    Private Shared _loggingStream As Stream
    Private Shared _eveHqTracer As EveHQTraceLogger

    Shared Sub New()

    End Sub

    Public Shared Property Settings As New EveHQSettings

    Public Shared Property EveHQLogTimer As New Stopwatch

    Public Shared Property Plugins() As Dictionary(Of String, EveHQPlugIn)
        Get
            If _plugins Is Nothing Then
                _plugins = New Dictionary(Of String, EveHQPlugIn)
            End If
            Return _plugins
        End Get
        Set(ByVal value As Dictionary(Of String, EveHQPlugIn))
            _plugins = value
        End Set
    End Property

    'Public Shared Property EveHqSettings As EveSettings

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
                If (Settings.MarketDataProvider = EveCentralMarketDataProvider.Name) Then
                    _marketStatDataProvider = GetEveCentralMarketInstance()
                Else
                    _marketStatDataProvider = GetEveHqMarketInstance()
                End If
            End If
            Return _marketStatDataProvider
        End Get
        Set(value As IMarketStatDataProvider)
            _marketStatDataProvider = value
        End Set
    End Property

    Public Shared Property MarketCacheUploader As MarketUploader
        Get
            If _marketCacheUploader Is Nothing Then
                _marketDataReceivers = {CType(GetEveCentralMarketInstance(), IMarketDataReceiver), New EveMarketDataRelayProvider(HttpRequestProvider.Default)}
                _marketCacheUploader = New MarketUploader(MarketCacheProcessorMinTime, _marketDataReceivers, Nothing)
            End If

            Return _marketCacheUploader
        End Get
        Set(value As MarketUploader)
            _marketCacheUploader = value
        End Set
    End Property

    Public Shared Property TickerItemList As List(Of Integer)
        Get
            If (_tickerItemList.Count = 0) Then
                'Add place holder mineral types only
                _tickerItemList.Add(34)
                _tickerItemList.Add(35)
                _tickerItemList.Add(36)
                _tickerItemList.Add(37)
                _tickerItemList.Add(38)
                _tickerItemList.Add(39)
                _tickerItemList.Add(40)
                _tickerItemList.Add(11399)
            End If
            Return _tickerItemList
        End Get
        Set(value As List(Of Integer))
            _tickerItemList = value
        End Set
    End Property

    Public Shared Property MarketOrderDataProvider As IMarketOrderDataProvider
        Get
            If _marketOrderDataProvider Is Nothing Then
                _marketOrderDataProvider = GetEveCentralMarketInstance()
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

    Public Shared Property EveHqTracer As EveHqTraceLogger
        Get
            Return _eveHqTracer
        End Get
        Set(value As EveHqTraceLogger)
            _eveHqTracer = value
        End Set
    End Property

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

    Public Shared Sub WriteLogEvent(ByVal eventText As String)
        Dim ts As TimeSpan = EveHQLogTimer.Elapsed
        ' Format and display the TimeSpan value.
        Dim elapsedTime As String = String.Format("{0:00}:{1:00}:{2:00}.{3:000}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds)
        EventText = "[" & elapsedTime & "]" & " " & EventText
        Try
            Trace.WriteLine(EventText, "Information")
        Catch e As Exception
            ' Don't bother reporting this
        End Try
    End Sub

    Public Shared Function GetMdiTab(ByVal tabName As String) As TabItem

        Dim mainTab As TabStrip = CType(MainForm.Controls("tabEveHQMDI"), TabStrip)
        For Each tp As TabItem In mainTab.Tabs
            If tp.Text = tabName Then
                Return tp
            End If
        Next
        Return Nothing
    End Function

    Private Shared _eveCentralProvider As EveCentralMarketDataProvider
    Public Shared Function GetEveCentralMarketInstance() As EveCentralMarketDataProvider
        If _eveCentralProvider Is Nothing Then
            If (Settings.ProxyRequired) Then
                _eveCentralProvider = New EveCentralMarketDataProvider(Path.Combine(appDataFolder, "MarketCache\EveCentral"), HttpRequestProvider.Default, New UriBuilder(Settings.ProxyServer).Uri, Settings.ProxyUseDefault, Settings.ProxyUsername, Settings.ProxyPassword, Settings.ProxyUseBasic)
            Else
                _eveCentralProvider = New EveCentralMarketDataProvider(Path.Combine(appDataFolder, "MarketCache\EveCentral"), HttpRequestProvider.Default)
            End If
        End If
        Return _eveCentralProvider
    End Function

    Private Shared _eveHqProvider As EveHQMarketDataProvider
    Private Shared _plugins As Dictionary(Of String, EveHQPlugIn)

    Public Shared Function GetEveHqMarketInstance() As EveHQMarketDataProvider
        If _eveHqProvider Is Nothing Then
            If (Settings.ProxyRequired) Then
                _eveHqProvider = New EveHQMarketDataProvider(Path.Combine(AppDataFolder, "MarketCache\EveHq"), HttpRequestProvider.Default, New UriBuilder(Settings.ProxyServer).Uri, Settings.ProxyUseDefault, Settings.ProxyUsername, Settings.ProxyPassword, Settings.ProxyUseBasic)
            Else
                _eveHqProvider = New EveHQMarketDataProvider(Path.Combine(AppDataFolder, "MarketCache\EveHq"), HttpRequestProvider.Default)
            End If
        End If
        Return _eveHqProvider
    End Function

End Class

Class ListViewItemComparerA
    Implements IComparer
    Private ReadOnly _col As Integer

    Public Sub New()
        _col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        _col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements IComparer.Compare
        Return [String].Compare(CType(x, ListViewItem).SubItems(_col).Text, CType(y, ListViewItem).SubItems(_col).Text)
    End Function
End Class

Class ListViewItemComparerD
    Implements IComparer
    Private ReadOnly _col As Integer

    Public Sub New()
        _col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        _col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
        Implements IComparer.Compare
        Return [String].Compare(CType(y, ListViewItem).SubItems(_col).Text, CType(x, ListViewItem).SubItems(_col).Text)
    End Function
End Class

Public Class ListViewItemComparerText
    Implements IComparer
    Private ReadOnly _col As Integer
    Private ReadOnly _order As SortOrder

    Public Sub New()
        _col = 0
        _order = SortOrder.Ascending
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As SortOrder)
        _col = column
        _order = order
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1

        Try
            If _
                IsNumeric(CType(x, ListViewItem).SubItems(_col).Text) = True And
                IsNumeric(CType(y, ListViewItem).SubItems(_col).Text) = True Then
                ' Parse the two objects passed as a parameter as a DateTime.
                Dim firstNum As Double = CDbl((CType(x, ListViewItem).SubItems(_col).Text))
                Dim secondNum As Double = CDbl((CType(y, ListViewItem).SubItems(_col).Text))
                ' Compare the two numbers
                returnVal = Decimal.Compare(CDec(firstNum), CDec(secondNum))
                ' If neither compared object has a valid date format, 
                ' compare as a string.
            Else
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(_col).Text,
                                             CType(y, ListViewItem).SubItems(_col).Text)
            End If
        Catch
            ' Compare the two items as a string.
        End Try

        ' Determine whether the sort order is descending.
        If _order = SortOrder.Descending Then
            ' Invert the value returned by String.Compare.
            returnVal *= -1
        End If
        Return returnVal
    End Function
End Class

Public Class ListViewItemComparerName
    Implements IComparer
    Private ReadOnly _col As Integer
    Private ReadOnly _order As SortOrder

    Public Sub New()
        _col = 0
        _order = SortOrder.Ascending
    End Sub

    Public Sub New(ByVal column As Integer, ByVal order As SortOrder)
        _col = column
        _order = order
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim returnVal As Integer = -1

        Try
            If _
                IsNumeric(CType(x, ListViewItem).SubItems(_col).Name) = True And
                IsNumeric(CType(y, ListViewItem).SubItems(_col).Name) = True Then
                ' Parse the two objects passed as a parameter as a DateTime.
                Dim firstNum As Double = CDbl((CType(x, ListViewItem).SubItems(_col).Name))
                Dim secondNum As Double = CDbl((CType(y, ListViewItem).SubItems(_col).Name))
                ' Compare the two numbers
                returnVal = Decimal.Compare(CDec(firstNum), CDec(secondNum))
                ' If neither compared object has a valid date format, 
                ' compare as a string.
            Else
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(_col).Name,
                                             CType(y, ListViewItem).SubItems(_col).Name)
            End If
        Catch
            ' Compare the two items as a string.
        End Try

        ' Determine whether the sort order is descending.
        If _order = SortOrder.Descending Then
            ' Invert the value returned by String.Compare.
            returnVal *= -1
        End If
        Return returnVal
    End Function
End Class
