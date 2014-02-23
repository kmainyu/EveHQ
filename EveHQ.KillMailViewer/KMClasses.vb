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


Public Class KillmailItem
    Public Property TypeID As Integer
    Public Property Flag As Integer
    Public Property QtyDropped As Integer
    Public Property QtyDestroyed As Integer
End Class

Public Class KillmailVictim
    Public Property CharID As String
    Public Property CharName As String
    Public Property CorpID As String
    Public Property CorpName As String
    Public Property AllianceID As String
    Public Property AllianceName As String
    Public Property FactionID As String
    Public Property FactionName As String
    Public Property DamageTaken As Double
    Public Property ShipTypeID As Integer
End Class

Public Class KillmailAttacker
    Public Property CharID As String
    Public Property CharName As String
    Public Property CorpID As String
    Public Property CorpName As String
    Public Property AllianceID As String
    Public Property AllianceName As String
    Public Property FactionID As String
    Public Property FactionName As String
    Public Property SecStatus As Double
    Public Property DamageDone As Double
    Public Property FinalBlow As Boolean
    Public Property WeaponTypeID As Integer
    Public Property ShipTypeID As Integer
End Class

Public Class KillMail
    Public Property KillID As Integer
    Public Property SystemID As Integer
    Public Property KillTime As Date
    Public Property MoonID As String
    Public Property Victim As KillmailVictim
    Public Property Attackers As New SortedList
    Public Property Items As New List(Of KillmailItem)
End Class
