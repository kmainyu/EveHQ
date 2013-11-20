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

Namespace Classes

    <Serializable()> Public Class PrismSettings

        Public Shared UserSettings As New PrismSettings
        Public Shared PrismFolder As String

        Private _factoryInstallCost As Double = 1000
        Private _factoryRunningCost As Double = 333
        Private _labInstallCost As Double = 1000
        Private _labRunningCost As Double = 333
        Private _bpcCosts As New SortedList(Of Integer, BlueprintCopyCostInfo)
        Private _defaultCharacter As String
        Private _defaultBPOwner As String
        Private _defaultBPCalcManufacturer As String
        Private _defaultBPCalcAssetOwner As String
        Private _standardSlotColumns As New List(Of UserSlotColumn)
        Private _userSlotColumns As New List(Of UserSlotColumn)
        Private _slotNameWidth As Integer = 250
        ' CorpReps: SortedList (of <CorpName>, Sortedlist(Of CorpRepType, PilotName))
        Private _corpReps As New SortedList(Of String, SortedList(Of CorpRepType, String))

        Public Property HideSaveJobDialog As Boolean = False
        Public Property HideAPIDownloadDialog As Boolean = False
        Public Property CorpReps As SortedList(Of String, SortedList(Of CorpRepType, String))
            Get
                If _corpReps Is Nothing Then
                    _corpReps = New SortedList(Of String, SortedList(Of CorpRepType, String))
                End If
                Return _corpReps
            End Get
            Set(ByVal value As SortedList(Of String, SortedList(Of CorpRepType, String)))
                _corpReps = value
            End Set
        End Property
        Public Property SlotNameWidth() As Integer
            Get
                If _slotNameWidth = 0 Then _slotNameWidth = 250
                Return _slotNameWidth
            End Get
            Set(ByVal value As Integer)
                _slotNameWidth = value
            End Set
        End Property
        Public Property UserSlotColumns() As List(Of UserSlotColumn)
            Get
                If _userSlotColumns Is Nothing Then
                    _userSlotColumns = New List(Of UserSlotColumn)
                End If
                Return _userSlotColumns
            End Get
            Set(ByVal value As List(Of UserSlotColumn))
                _userSlotColumns = value
            End Set
        End Property
        Public Property StandardSlotColumns() As List(Of UserSlotColumn)
            Get
                If _standardSlotColumns Is Nothing Then
                    _standardSlotColumns = New List(Of UserSlotColumn)
                End If
                Return _standardSlotColumns
            End Get
            Set(ByVal value As List(Of UserSlotColumn))
                _standardSlotColumns = value
            End Set
        End Property
        Public Property DefaultBPCalcAssetOwner() As String
            Get
                If _defaultBPCalcAssetOwner Is Nothing Then
                    _defaultBPCalcAssetOwner = ""
                End If
                Return _defaultBPCalcAssetOwner
            End Get
            Set(ByVal value As String)
                _defaultBPCalcAssetOwner = value
            End Set
        End Property
        Public Property DefaultBPCalcManufacturer() As String
            Get
                If _defaultBPCalcManufacturer Is Nothing Then
                    _defaultBPCalcManufacturer = ""
                End If
                Return _defaultBPCalcManufacturer
            End Get
            Set(ByVal value As String)
                _defaultBPCalcManufacturer = value
            End Set
        End Property
        Public Property DefaultBPOwner() As String
            Get
                If _defaultBPOwner Is Nothing Then
                    _defaultBPOwner = ""
                End If
                Return _defaultBPOwner
            End Get
            Set(ByVal value As String)
                _defaultBPOwner = value
            End Set
        End Property
        Public Property DefaultCharacter() As String
            Get
                If _defaultCharacter Is Nothing Then
                    _defaultCharacter = ""
                End If
                Return _defaultCharacter
            End Get
            Set(ByVal value As String)
                _defaultCharacter = value
            End Set
        End Property
        Public Property BPCCosts() As SortedList(Of Integer, BlueprintCopyCostInfo)
            Get
                If _bpcCosts Is Nothing Then
                    _bpcCosts = New SortedList(Of Integer, BlueprintCopyCostInfo)
                End If
                Return _bpcCosts
            End Get
            Set(ByVal value As SortedList(Of Integer, BlueprintCopyCostInfo))
                _bpcCosts = value
            End Set
        End Property
        Public Property LabRunningCost() As Double
            Get
                Return _labRunningCost
            End Get
            Set(ByVal value As Double)
                _labRunningCost = value
            End Set
        End Property
        Public Property LabInstallCost() As Double
            Get
                Return _labInstallCost
            End Get
            Set(ByVal value As Double)
                _labInstallCost = value
            End Set
        End Property
        Public Property FactoryRunningCost() As Double
            Get
                Return _factoryRunningCost
            End Get
            Set(ByVal value As Double)
                _factoryRunningCost = value
            End Set
        End Property
        Public Property FactoryInstallCost() As Double
            Get
                Return _factoryInstallCost
            End Get
            Set(ByVal value As Double)
                _factoryInstallCost = value
            End Set
        End Property

        Private Shared ReadOnly LockObj As New Object()
        Private Const MainFileName As String = "PrismSettings.json"
        Public Sub SavePrismSettings()

            SyncLock LockObj

                Dim newFile As String = Path.Combine(PrismFolder, MainFileName)
                Dim tempFile As String = Path.Combine(PrismFolder, MainFileName & ".temp")

                ' Create a JSON string for writing
                Dim json As String = JsonConvert.SerializeObject(UserSettings, Formatting.Indented)

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

                If File.Exists(Path.Combine(PrismFolder, MainFileName)) = True Then

                    Try
                        Using s As New StreamReader(Path.Combine(PrismFolder, MainFileName))
                            Dim json As String = s.ReadToEnd
                            UserSettings = JsonConvert.DeserializeObject(Of PrismSettings)(json)
                        End Using

                    Catch ex As Exception
                        Dim msg As String = "There was an error trying to load the Prism settings file and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "Prism will delete this file and re-initialise the settings." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "Press OK to reset the settings." & ControlChars.CrLf
                        MessageBox.Show(msg, "Invalid Settings file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Try
                            My.Computer.FileSystem.DeleteFile(Path.Combine(PrismFolder, MainFileName))
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
            If UserSettings.UserSlotColumns.Count <> UserSettings.StandardSlotColumns.Count Then
                Dim missingFlag As Boolean
                For Each stdCol As UserSlotColumn In UserSettings.StandardSlotColumns
                    missingFlag = True
                    For Each testUserCol As UserSlotColumn In UserSettings.UserSlotColumns
                        If stdCol.Name = testUserCol.Name Then
                            missingFlag = False
                            Exit For
                        End If
                    Next
                    If missingFlag = True Then
                        UserSettings.UserSlotColumns.Add(New UserSlotColumn(stdCol.Name, stdCol.Description, stdCol.Width, stdCol.Active))
                    End If
                Next
            End If
            Return True

        End Function

        Public Sub InitialiseSlotColumns()
            UserSettings.StandardSlotColumns.Clear()
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetOwner", "Owner", 150, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetGroup", "Group", 100, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetCategory", "Category", 100, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetSystem", "System", 100, False))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetConstellation", "Constellation", 100, False))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetRegion", "EveGalaticRegion", 100, False))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetSystemSec", "Sec", 50, False))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetLocation", "Specific Location", 250, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetMeta", "Meta", 50, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetVolume", "Volume", 100, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetQuantity", "Quantity", 100, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetPrice", "Price", 100, True))
            UserSettings.StandardSlotColumns.Add(New UserSlotColumn("AssetValue", "Value", 100, True))
        End Sub
    End Class

End Namespace

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

<Serializable()> Public Class UserSlotColumn
    ' ReSharper disable InconsistentNaming - for binary serialization compatability
    Dim cName As String = ""
    Dim cDescription As String = ""
    Dim cWidth As Integer = 75
    Dim cActive As Boolean = False
    ' ReSharper restore InconsistentNaming

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

    Public Sub New(ByVal columnName As String, ByVal desc As String, ByVal columnWidth As Integer, ByVal isActive As Boolean)
        cName = columnName
        cDescription = desc
        cWidth = columnWidth
        cActive = IsActive
    End Sub

End Class