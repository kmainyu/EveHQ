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

<Serializable()> Public Class Settings

    Public Shared PrismSettings As New Settings
    Public Shared PrismFolder As String

    Private cFactoryInstallCost As Double = 1000
    Private cFactoryRunningCost As Double = 333
    Private cLabInstallCost As Double = 1000
    Private cLabRunningCost As Double = 333
    Private cBPCCosts As New SortedList(Of String, BPCCostInfo)
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
    Public Property BPCCosts() As SortedList(Of String, BPCCostInfo)
        Get
            If cBPCCosts Is Nothing Then
                cBPCCosts = New SortedList(Of String, BPCCostInfo)
            End If
            Return cBPCCosts
        End Get
        Set(ByVal value As SortedList(Of String, BPCCostInfo))
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
End Class
