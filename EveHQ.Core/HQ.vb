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
Imports System.Windows.Forms

Public Class HQ

    Private Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr, ByVal min As Int32, ByVal max As Int32) As Boolean

    Public Shared MainForm As Form
    Public Shared TPilots As New SortedList(Of String, EveHQ.Core.Pilot)
    Public Shared EveHQSettings As New EveSettings
    Public Shared myIGB As New IGB
    Public Shared myAPIRS As New APIRS
    Public Shared myTQServer As EveServer = New EveServer
    Public Shared SkillListName As New SortedList(Of String, EveHQ.Core.EveSkill)
    Public Shared SkillListID As New SortedList(Of String, EveHQ.Core.EveSkill)
    Public Shared SkillGroups As New SortedList(Of String, EveHQ.Core.SkillGroup)
    Public Shared SkillUnlocks As New SortedList(Of String, ArrayList)
    Public Shared ItemUnlocks As New SortedList(Of String, ArrayList)
    Public Shared CertUnlockSkills As New SortedList(Of String, ArrayList)
    Public Shared CertUnlockCerts As New SortedList(Of String, ArrayList)
    Public Shared IsUsingLocalFolders As Boolean = False
    Public Shared IsSplashFormDisabled As Boolean = False
    Public Shared appDataFolder As String = ""
    Public Shared appFolder As String = ""
    Public Shared cacheFolder As String = ""
    Public Shared imageCacheFolder As String = ""
    Public Shared reportFolder As String = ""
    Public Shared dataFolder As String = ""
    Public Shared backupFolder As String = ""
    Public Shared EveHQBackupFolder As String = ""
    Public Shared itemDBConnectionString As String = ""
    Public Shared EveHQDataConnectionString As String = ""
    Public Shared dataError As String = ""
    Public Shared IGBActive As Boolean = False
    Public Shared APIRSActive As Boolean = False
    Public Shared APIResults As New SortedList
    Public Shared APIErrors As New SortedList
    Public Shared itemList As New SortedList(Of String, String)
    Public Shared itemData As New SortedList(Of String, EveHQ.Core.EveItem)
    Public Shared itemGroups As New SortedList(Of String, String)
    Public Shared itemCats As New SortedList(Of String, String)
    Public Shared groupCats As New SortedList(Of String, String)
    Public Shared LastAutoAPIResult As Boolean = True
    Public Shared NextAutoAPITime As DateTime = Now.AddMinutes(60)
    Public Shared AutoRetryAPITime As DateTime = Now.AddMinutes(5) ' Minimum retry time if an error occurs
    Public Shared EveHQLCD As New G15LCD(AddressOf EveHQ.Core.G15LCDB.ButtonPress, AddressOf EveHQ.Core.G15LCDB.ConfigureOptions)
    Public Shared IsG15LCDActive As Boolean = False
    Public Shared lcdPilot As String = ""
    Public Shared lcdCharMode As Integer = 0
    Public Shared BasePriceList As New SortedList
    Public Shared MarketPriceList As New SortedList
    Public Shared CustomPriceList As New SortedList
    Public Shared APIUpdateAvailable As Boolean = False
    Public Shared AppUpdateAvailable As Boolean = False
    Public Shared CertificateCategories As New SortedList
    Public Shared CertificateClasses As New SortedList
    Public Shared Certificates As New SortedList
    Public Shared FittingProtocol As String = "fitting"
    Public Shared AllStandings As New SortedList
    Public Shared NextAutoMailAPITime As DateTime = Now
    Public Shared Event ShutDownEveHQ()
    Public Shared APIUpdateInProgress As Boolean = False

    Shared Property StartShutdownEveHQ() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent ShutDownEveHQ()
            End If
        End Set
    End Property

    Public Enum DBFormat As Integer
        Access = 0
        MSSQL = 1
        MSSQLE = 2
        MySQL = 3
    End Enum

    Public Shared Sub ReduceMemory()
        GC.Collect()
        GC.WaitForPendingFinalizers()
        Try
            If (Environment.OSVersion.Platform = PlatformID.Win32NT) Then
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1)
            End If
        Catch ex As Exception
            ' Catch potential errors from remote loading routines
        End Try
    End Sub

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
            If IsNumeric(CType(x, ListViewItem).SubItems(col).Text) = True And IsNumeric(CType(y, ListViewItem).SubItems(col).Text) = True Then
                ' Parse the two objects passed as a parameter as a DateTime.
                Dim firstNum As Double = CDbl((CType(x, ListViewItem).SubItems(col).Text))
                Dim secondNum As Double = CDbl((CType(y, ListViewItem).SubItems(col).Text))
                ' Compare the two numbers
                returnVal = Decimal.Compare(CDec(firstNum), CDec(secondNum))
                ' If neither compared object has a valid date format, 
                ' compare as a string.
            Else
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
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
            If IsNumeric(CType(x, ListViewItem).SubItems(col).Name) = True And IsNumeric(CType(y, ListViewItem).SubItems(col).Name) = True Then
                ' Parse the two objects passed as a parameter as a DateTime.
                Dim firstNum As Double = CDbl((CType(x, ListViewItem).SubItems(col).Name))
                Dim secondNum As Double = CDbl((CType(y, ListViewItem).SubItems(col).Name))
                ' Compare the two numbers
                returnVal = Decimal.Compare(CDec(firstNum), CDec(secondNum))
                ' If neither compared object has a valid date format, 
                ' compare as a string.
            Else
                returnVal = [String].Compare(CType(x, ListViewItem).SubItems(col).Name, CType(y, ListViewItem).SubItems(col).Name)
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
