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

Public Enum CorporationRoles As Long
    Director = 1
    PersonnelManager = 128
    Accountant = 256
    SecurityOfficer = 512
    FactoryManager = 1024
    StationManager = 2048
    Auditor = 4096
    HangarCanTake1 = 8192
    HangarCanTake2 = 16384
    HangarCanTake3 = 32768
    HangarCanTake4 = 65536
    HangarCanTake5 = 131072
    HangarCanTake6 = 262144
    HangarCanTake7 = 524288
    HangarCanQuery1 = 1048576
    HangarCanQuery2 = 2097152
    HangarCanQuery3 = 4194304
    HangarCanQuery4 = 8388608
    HangarCanQuery5 = 16777216
    HangarCanQuery6 = 33554432
    HangarCanQuery7 = 67108864
    AccountCanTake1 = 134217728
    AccountCanTake2 = 268435456
    AccountCanTake3 = 536870912
    AccountCanTake4 = 1073741824
    AccountCanTake5 = 2147483648
    AccountCanTake6 = 4294967296
    AccountCanTake7 = 8589934592
    AccountCanQuery1 = 17179869184
    AccountCanQuery2 = 34359738368
    AccountCanQuery3 = 68719476736
    AccountCanQuery4 = 137438953472
    AccountCanQuery5 = 274877906944
    AccountCanQuery6 = 549755813888
    AccountCanQuery7 = 1099511627776
    EquipmentConfig = 2199023255552
    ContainerCanTake1 = 4398046511104
    ContainerCanTake2 = 8796093022208
    ContainerCanTake3 = 17592186044416
    ContainerCanTake4 = 35184372088832
    ContainerCanTake5 = 70368744177664
    ContainerCanTake6 = 140737488355328
    ContainerCanTake7 = 281474976710656
    CanRentOffice = 562949953421312
    CanRentFactorySlot = 1125899906842624
    CanRentResearchSlot = 2251799813685248
    JuniorAccountant = 4503599627370496
    StarbaseConfig = 9007199254740992
    Trader = 18014398509481984
    ChatManager = 36028797018963968
    ContractManager = 72057594037927936
    InfrastructureTacticalOfficer = 144115188075855872
    StarbaseCaretaker = 288230376151711744
End Enum