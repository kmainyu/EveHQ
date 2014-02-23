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


Imports EveHQ.Prism.Classes ' ReSharper disable once CheckNamespace - For binary serialization compatability

<Serializable()> Public Class Settings

    Public Shared PrismSettings As New Settings
    Public Shared PrismFolder As String


    ' ReSharper disable InconsistentNaming - For binary serialization compatability
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
    Private cCorpReps As New SortedList(Of String, SortedList(Of CorpRepType, String))
    ' ReSharper restore InconsistentNaming

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
