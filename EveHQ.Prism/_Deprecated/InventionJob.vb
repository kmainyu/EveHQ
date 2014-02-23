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


' ReSharper disable once CheckNamespace - For binary serialization compatability
<Serializable()>Public Class InventionJob
    ' Invention specific items
    Private _inventedBpid As Integer
    Private _baseChance As Double
    Private _decryptorUsed As Decryptor
    Private _metaItemId As Integer
    Private _metaItemLevel As Integer
    Private _overrideBpcRuns As Boolean
    Private _bpcRuns As Integer
    Private _overrideEncSkill As Boolean
    Private _overrideDcSkill1 As Boolean
    Private _overrideDcSkill2 As Boolean
    Private _encryptionSkill As Integer
    Private _datacoreSkill1 As Integer
    Private _datacoreSkill2 As Integer
    Private _productionJob As ProductionJob

    Public Property InventedBpid As Integer
        Get
            Return _inventedBpid
        End Get
        Set(value As Integer)
            _inventedBpid = value
        End Set
    End Property

    Public Property BaseChance As Double
        Get
            Return _baseChance
        End Get
        Set(value As Double)
            _baseChance = value
        End Set
    End Property

    Public Property DecryptorUsed As Decryptor
        Get
            Return _decryptorUsed
        End Get
        Set(value As Decryptor)
            _decryptorUsed = value
        End Set
    End Property

    Public Property MetaItemId As Integer
        Get
            Return _metaItemId
        End Get
        Set(value As Integer)
            _metaItemId = value
        End Set
    End Property

    Public Property MetaItemLevel As Integer
        Get
            Return _metaItemLevel
        End Get
        Set(value As Integer)
            _metaItemLevel = value
        End Set
    End Property

    Public Property OverrideBpcRuns As Boolean
        Get
            Return _overrideBpcRuns
        End Get
        Set(value As Boolean)
            _overrideBpcRuns = value
        End Set
    End Property

    Public Property BpcRuns As Integer
        Get
            Return _bpcRuns
        End Get
        Set(value As Integer)
            _bpcRuns = value
        End Set
    End Property

    Public Property OverrideEncSkill As Boolean
        Get
            Return _overrideEncSkill
        End Get
        Set(value As Boolean)
            _overrideEncSkill = value
        End Set
    End Property

    Public Property OverrideDcSkill1 As Boolean
        Get
            Return _overrideDcSkill1
        End Get
        Set(value As Boolean)
            _overrideDcSkill1 = value
        End Set
    End Property

    Public Property OverrideDcSkill2 As Boolean
        Get
            Return _overrideDcSkill2
        End Get
        Set(value As Boolean)
            _overrideDcSkill2 = value
        End Set
    End Property

    Public Property EncryptionSkill As Integer
        Get
            Return _encryptionSkill
        End Get
        Set(value As Integer)
            _encryptionSkill = value
        End Set
    End Property

    Public Property DatacoreSkill1 As Integer
        Get
            Return _datacoreSkill1
        End Get
        Set(value As Integer)
            _datacoreSkill1 = value
        End Set
    End Property

    Public Property DatacoreSkill2 As Integer
        Get
            Return _datacoreSkill2
        End Get
        Set(value As Integer)
            _datacoreSkill2 = value
        End Set
    End Property

    Public Property ProductionJob As ProductionJob
        Get
            Return _productionJob
        End Get
        Set(value As ProductionJob)
            _productionJob = value
        End Set
    End Property

End Class
