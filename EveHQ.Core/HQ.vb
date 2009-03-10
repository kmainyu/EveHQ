' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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

    Public Shared MainForm As Form
    Public Shared logonCookie As New System.Net.CookieContainer
    Public Shared logonSID As String
    Public Shared TAccounts As Collection = New Collection
    Public Shared TPilots As Collection = New Collection
    Public Shared EveHQSettings As New EveSettings
    Public Shared myPilot As New EveHQ.Core.Pilot
    Public Shared myIGB As New IGB
    Public Shared myAPIRS As New APIRS
    Public Shared myTQServer As EveServer = New EveServer
    Public Shared mySiSiServer As EveServer = New EveServer
    Public Shared IGBPilot As New EveHQ.Core.Pilot
    Public Shared SkillListName As Collection = New Collection
    Public Shared SkillListID As Collection = New Collection
    Public Shared SkillGroups As Collection = New Collection
    Public Shared SkillUnlocks As SortedList = New SortedList
    Public Shared ItemUnlocks As SortedList = New SortedList
    Public Shared CertUnlockSkills As SortedList = New SortedList
    Public Shared CertUnlockCerts As SortedList = New SortedList
    Public Shared IsUsingLocalFolders As Boolean = False
    Public Shared IsSplashFormDisabled As Boolean = False
    Public Shared appDataFolder As String = ""
    Public Shared appFolder As String = ""
    Public Shared cacheFolder As String = ""
    Public Shared reportFolder As String = ""
    Public Shared dataFolder As String = ""
    Public Shared backupFolder As String = ""
    Public Shared itemDBConnectionString As String = ""
    Public Shared EveHQDataConnectionString As String = ""
    Public Shared dataError As String = ""
    Public Shared IGBActive As Boolean = False
    Public Shared APIRSActive As Boolean = False
    Public Shared TFTPAccounts As Collection = New Collection
    Public Shared MineralPrices(8) As Double
    Public Shared APIResults As New SortedList
    Public Shared APIErrors As New SortedList
    Public Shared MaxLogonAttempts As Integer = 3
    Public Shared itemList As SortedList = New SortedList
    Public Shared itemData As SortedList = New SortedList
    Public Shared itemGroups As SortedList = New SortedList
    Public Shared itemCats As SortedList = New SortedList
    Public Shared groupCats As SortedList = New SortedList
    Public Shared LastAutoAPIResult As Boolean = True
    Public Shared LastAutoAPITime As DateTime = Now.AddDays(-1)
    Public Shared EveHQLCD As New G15LCD(AddressOf EveHQ.Core.G15LCDB.ButtonPress, AddressOf EveHQ.Core.G15LCDB.ConfigureOptions)
    Public Shared IsG15LCDActive As Boolean = False
    Public Shared lcdPilot As String = ""
    Public Shared lcdCharMode As Integer = 0
    Public Shared BasePriceList As New SortedList
    Public Shared MarketPriceList As New SortedList
    Public Shared CustomPriceList As New SortedList
    Public Shared UpdateAvailable As Boolean = False
    Public Shared CertificateCategories As New SortedList
    Public Shared CertificateClasses As New SortedList
    Public Shared Certificates As New SortedList
    Public Shared Event CloseInfoPanel()
    Public Shared Event ShutDownEveHQ()

    Shared Property StartCloseInfoPanel() As Boolean
        Get
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent CloseInfoPanel()
            End If
        End Set
    End Property

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
