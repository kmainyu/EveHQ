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
Imports System.IO
Imports System.Windows.Forms
Imports Newtonsoft.Json

<Serializable()> Public Class Settings

    Public Shared PrismSettings As New Prism.Settings
    Public Shared PrismFolder As String

    Private cFactoryInstallCost As Double = 1000
    Private cFactoryRunningCost As Double = 333
    Private cLabInstallCost As Double = 1000
    Private cLabRunningCost As Double = 333
    Private cBPCCosts As New SortedList(Of Integer, BPCCostInfo)
    Private cDefaultCharacter As String
    Private cDefaultBPOwner As String
    Private cDefaultBPCalcManufacturer As String
    Private cDefaultBPCalcAssetOwner As String
    Private cStandardSlotColumns As New List(Of UserSlotColumn)
    Private cUserSlotColumns As New List(Of UserSlotColumn)
	Private cSlotNameWidth As Integer = 250
	' CorpReps: SortedList (of <CorpName>, Sortedlist(Of CorpRepType, PilotName))
    Private cCorpReps As New SortedList(Of String, SortedList(Of CorpRepType, String))

    Public Property HideAPIDownloadDialog As Boolean = False
	Public Property CorpReps As SortedList(Of String, SortedList(Of CorpRepType, String))
		Get
			If cCorpReps Is Nothing Then
				cCorpReps = New SortedList(Of String, SortedList(Of CorpRepType, String))
			End If
			Return cCorpReps
		End Get
		Set(ByVal value As SortedList(Of String, SortedList(Of CorpRepType, String)))
			cCorpReps = value
		End Set
	End Property
    Public Property SlotNameWidth() As Integer
        Get
            If cSlotNameWidth = 0 Then cSlotNameWidth = 250
            Return cSlotNameWidth
        End Get
        Set(ByVal value As Integer)
            cSlotNameWidth = value
        End Set
    End Property
    Public Property UserSlotColumns() As List(Of UserSlotColumn)
        Get
            If cUserSlotColumns Is Nothing Then
                cUserSlotColumns = New List(Of UserSlotColumn)
            End If
            Return cUserSlotColumns
        End Get
        Set(ByVal value As List(Of UserSlotColumn))
            cUserSlotColumns = value
        End Set
    End Property
    Public Property StandardSlotColumns() As List(Of UserSlotColumn)
        Get
            If cStandardSlotColumns Is Nothing Then
                cStandardSlotColumns = New List(Of UserSlotColumn)
            End If
            Return cStandardSlotColumns
        End Get
        Set(ByVal value As List(Of UserSlotColumn))
            cStandardSlotColumns = value
        End Set
    End Property
    Public Property DefaultBPCalcAssetOwner() As String
        Get
            If cDefaultBPCalcAssetOwner Is Nothing Then
                cDefaultBPCalcAssetOwner = ""
            End If
            Return cDefaultBPCalcAssetOwner
        End Get
        Set(ByVal value As String)
            cDefaultBPCalcAssetOwner = value
        End Set
    End Property
    Public Property DefaultBPCalcManufacturer() As String
        Get
            If cDefaultBPCalcManufacturer Is Nothing Then
                cDefaultBPCalcManufacturer = ""
            End If
            Return cDefaultBPCalcManufacturer
        End Get
        Set(ByVal value As String)
            cDefaultBPCalcManufacturer = value
        End Set
    End Property
    Public Property DefaultBPOwner() As String
        Get
            If cDefaultBPOwner Is Nothing Then
                cDefaultBPOwner = ""
            End If
            Return cDefaultBPOwner
        End Get
        Set(ByVal value As String)
            cDefaultBPOwner = value
        End Set
    End Property
    Public Property DefaultCharacter() As String
        Get
            If cDefaultCharacter Is Nothing Then
                cDefaultCharacter = ""
            End If
            Return cDefaultCharacter
        End Get
        Set(ByVal value As String)
            cDefaultCharacter = value
        End Set
    End Property
    Public Property BPCCosts() As SortedList(Of Integer, BPCCostInfo)
        Get
            If cBPCCosts Is Nothing Then
                cBPCCosts = New SortedList(Of Integer, BPCCostInfo)
            End If
            Return cBPCCosts
        End Get
        Set(ByVal value As SortedList(Of Integer, BPCCostInfo))
            cBPCCosts = value
        End Set
    End Property
    Public Property LabRunningCost() As Double
        Get
            Return cLabRunningCost
        End Get
        Set(ByVal value As Double)
            cLabRunningCost = value
        End Set
    End Property
    Public Property LabInstallCost() As Double
        Get
            Return cLabInstallCost
        End Get
        Set(ByVal value As Double)
            cLabInstallCost = value
        End Set
    End Property
    Public Property FactoryRunningCost() As Double
        Get
            Return cFactoryRunningCost
        End Get
        Set(ByVal value As Double)
            cFactoryRunningCost = value
        End Set
    End Property
    Public Property FactoryInstallCost() As Double
        Get
            Return cFactoryInstallCost
        End Get
        Set(ByVal value As Double)
            cFactoryInstallCost = value
        End Set
    End Property

    Private Shared LockObj As New Object()
    Private Const MainFileName As String = "PrismSettings.json"
    Public Sub SavePrismSettings()

        SyncLock LockObj

            Dim newFile As String = Path.Combine(Settings.PrismFolder, MainFileName)
            Dim tempFile As String = Path.Combine(Settings.PrismFolder, MainFileName & ".temp")

            ' Create a JSON string for writing
            Dim json As String = JsonConvert.SerializeObject(PrismSettings, Newtonsoft.Json.Formatting.Indented)

            ' Write the JSON version of the settings
            Try
                Using s As New StreamWriter(tempFile, False)
                    s.Write(json)
                    s.Flush()
                End Using

                If File.Exists(newFile) Then
                    File.Delete(newFile)
                End If

                File.Move(tempFile, newFile)

            Catch e As Exception

            End Try

        End SyncLock

    End Sub

    Public Function LoadPrismSettings() As Boolean
        SyncLock LockObj

            If File.Exists(Path.Combine(Settings.PrismFolder, MainFileName)) = True Then

                Try
                    Using s As New StreamReader(Path.Combine(Settings.PrismFolder, MainFileName))
                        Dim json As String = s.ReadToEnd
                        PrismSettings = JsonConvert.DeserializeObject(Of Settings)(json)
                    End Using

                Catch ex As Exception
                    Dim msg As String = "There was an error trying to load the Prism settings file and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Prism will delete this file and re-initialise the settings." & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Press OK to reset the settings." & ControlChars.CrLf
                    MessageBox.Show(msg, "Invalid Settings file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Try
                        My.Computer.FileSystem.DeleteFile(Path.Combine(Settings.PrismFolder, MainFileName))
                    Catch e As Exception
                        MessageBox.Show("Unable to delete the PrismSettings.bin file. Please delete this manually before proceeding", "Delete File Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End Try
                End Try
            End If
        End SyncLock
        ' Initialise the standard slot columns
        Call InitialiseSlotColumns()

        ' Check if the standard columns have changed and we need to add columns
        If Settings.PrismSettings.UserSlotColumns.Count <> Settings.PrismSettings.StandardSlotColumns.Count Then
            Dim MissingFlag As Boolean = True
            For Each StdCol As UserSlotColumn In Settings.PrismSettings.StandardSlotColumns
                MissingFlag = True
                For Each TestUserCol As UserSlotColumn In Settings.PrismSettings.UserSlotColumns
                    If StdCol.Name = TestUserCol.Name Then
                        MissingFlag = False
                        Exit For
                    End If
                Next
                If MissingFlag = True Then
                    Settings.PrismSettings.UserSlotColumns.Add(New UserSlotColumn(StdCol.Name, StdCol.Description, StdCol.Width, StdCol.Active))
                End If
            Next
        End If
        Return True

    End Function

    Public Sub InitialiseSlotColumns()
        Settings.PrismSettings.StandardSlotColumns.Clear()
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetOwner", "Owner", 150, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetGroup", "Group", 100, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetCategory", "Category", 100, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetSystem", "System", 100, False))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetConstellation", "Constellation", 100, False))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetRegion", "EveGalaticRegion", 100, False))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetSystemSec", "Sec", 50, False))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetLocation", "Specific Location", 250, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetMeta", "Meta", 50, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetVolume", "Volume", 100, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetQuantity", "Quantity", 100, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetPrice", "Price", 100, True))
        Settings.PrismSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetValue", "Value", 100, True))
    End Sub
End Class

<Serializable()> Public Class UserSlotColumn
    Dim cName As String = ""
    Dim cDescription As String = ""
    Dim cWidth As Integer = 75
    Dim cActive As Boolean = False

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

    Public Sub New(ByVal ColumnName As String, ByVal Description As String, ByVal ColumnWidth As Integer, ByVal IsActive As Boolean)
        cName = ColumnName
        cDescription = Description
        cWidth = ColumnWidth
        cActive = IsActive
    End Sub

End Class

<Serializable()> Public Enum CorpRepType
	Assets = 0
	Balances = 1
	Jobs = 2
	WalletJournal = 3
	Orders = 4
    WalletTransactions = 5
    Contracts = 6
    CorpSheet = 7
End Enum