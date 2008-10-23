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
Imports System.IO
Imports System.Windows.Forms
Imports System.Text

Public Class StandingsCacheDecoder

    Dim MyStandings As New StandingsData

    Dim OpCodes As New SortedList
    Dim strings As New ArrayList
    Dim simpleObjectTypes As New ArrayList
    Dim rowSetObjectTypes As New ArrayList
    Dim unpackedRowSetObjectTypes As New ArrayList

    Dim fs As FileStream
    Dim br As BinaryReader

    Dim WithEvents CacheWorker As New System.ComponentModel.BackgroundWorker

    Dim currentObject As String = ""
    Dim currentPilotID As String = ""


    Public Function FetchStandings(ByVal fileName As String) As StandingsData
        ' Load the OpCodes and String Data
        Call Me.LoadOpCodes()
        Call Me.LoadStrings()
        ' Initialise the Object Types
        Call Me.InitSimpleObjectTypes()
        Call Me.InitRowSetObjectTypes()
        Call Me.InitUnpackedRowSetObjectTypes()
        MyStandings = New StandingsData

        ' Double-Check the file exists
        If My.Computer.FileSystem.FileExists(fileName) = False Then
            MessageBox.Show("Unable to find cache file!", "Standings Retrieval Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
            Exit Function
            'Else
            'MessageBox.Show("Using cache file: " & fileName, "Standings File Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        ' Create the file stream but open the file for binary reading
        fs = New FileStream(fileName, FileMode.Open)
        br = New BinaryReader(fs)

        Call DecodeStream(br)
        br.Close()
        fs.Close()

        ' Clear up items after we've done
        OpCodes.Clear()
        strings.Clear()
        simpleObjectTypes.Clear()
        rowSetObjectTypes.Clear()
        unpackedRowSetObjectTypes.Clear()

        Return MyStandings

    End Function
    Private Sub LoadOpCodes()
        OpCodes.Add(&H1, "None")
        OpCodes.Add(&H2, "ByteString")
        OpCodes.Add(&H3, "LongLong")
        OpCodes.Add(&H4, "Long")
        OpCodes.Add(&H5, "SignedShort")
        OpCodes.Add(&H6, "Byte")
        OpCodes.Add(&H7, "MinusOne")
        OpCodes.Add(&H8, "ZeroInteger")
        OpCodes.Add(&H9, "OneInteger")
        OpCodes.Add(&HA, "Real")
        OpCodes.Add(&HB, "ZeroReal")
        OpCodes.Add(&HC, "Obsolete (&h0c)")
        OpCodes.Add(&HD, "Buffer")
        OpCodes.Add(&HE, "EmptyString")
        OpCodes.Add(&HF, "CharString")
        OpCodes.Add(&H10, "ByteString2")
        OpCodes.Add(&H11, "StringTableItem")
        OpCodes.Add(&H12, "Unknown (&h12)")
        OpCodes.Add(&H13, "Unknown (&h13)")
        OpCodes.Add(&H14, "Tuple")
        OpCodes.Add(&H15, "List")
        OpCodes.Add(&H16, "Dict")
        OpCodes.Add(&H17, "Object")
        OpCodes.Add(&H18, "Unknown (&h18)")
        OpCodes.Add(&H19, "Unknown (&h19)")
        OpCodes.Add(&H1A, "Unknown (&h1a)")
        OpCodes.Add(&H1B, "SavedStreamElement")
        OpCodes.Add(&H1C, "Unknown (&h1c)")
        OpCodes.Add(&H1D, "Obsolete (&h1d)")
        OpCodes.Add(&H1E, "Obsolete (&h1e)")
        OpCodes.Add(&H1F, "True")
        OpCodes.Add(&H20, "False")
        OpCodes.Add(&H21, "Unknown (&h21)")
        OpCodes.Add(&H22, "RowHeader")
        OpCodes.Add(&H23, "ResultSet")
        '&h23, (Unknown, "PackedResultSet?")
        OpCodes.Add(&H24, "EmptyTuple")
        OpCodes.Add(&H25, "OneTuple")
        OpCodes.Add(&H26, "EmptyList")
        OpCodes.Add(&H27, "OneList")
        OpCodes.Add(&H28, "EmptyUnicodeString")
        OpCodes.Add(&H29, "UnicodeCharString")
        OpCodes.Add(&H2A, "DBRow")
        OpCodes.Add(&H2B, "SubStream")
        OpCodes.Add(&H2C, "TwoTuple")
        OpCodes.Add(&H2D, "Terminator")
        OpCodes.Add(&H2E, "UnicodeString")
        OpCodes.Add(&H2F, "VarInteger")
    End Sub
    Private Sub LoadStrings()
        Dim itemList As String = My.Resources.Strings.ToString
        Dim items() As String = itemList.Split(ControlChars.CrLf.ToCharArray)
        For Each item As String In items
            If item <> "" Then
                strings.Add(item)
            End If
        Next
    End Sub
    Private Sub InitSimpleObjectTypes()
        simpleObjectTypes.Clear()
        simpleObjectTypes.Add("config.BulkData.types")
        simpleObjectTypes.Add("config.BulkData.groups")
        simpleObjectTypes.Add("config.BulkData.categories")
        simpleObjectTypes.Add("config.BulkData.metagroups")
        simpleObjectTypes.Add("config.BulkData.invmetatypes")
        simpleObjectTypes.Add("config.BulkData.graphics")
        simpleObjectTypes.Add("config.BulkData.units")
    End Sub
    Private Sub InitRowSetObjectTypes()
        rowSetObjectTypes.Add("config.StaticOwners")
        rowSetObjectTypes.Add("GetAgents")
        'rowSetObjectTypes.Add("GetMarketGroups")
    End Sub
    Private Sub InitUnpackedRowSetObjectTypes()
        unpackedRowSetObjectTypes.Add("GetCharStandings")
        unpackedRowSetObjectTypes.Add("GetCorpStandings")
    End Sub
    Private Sub CacheWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles CacheWorker.DoWork
        CacheWorker.WorkerSupportsCancellation = True
        CacheWorker.WorkerReportsProgress = True
        Call DecodeStream(br)
    End Sub
    Private Sub CacheWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles CacheWorker.RunWorkerCompleted
        br.Close()
        fs.Close()
        CacheWorker.Dispose()
    End Sub

#Region "Decoding Routines"
    Private Function DecodeStream(ByVal br As BinaryReader) As Boolean
        Dim idx As Integer = 0
        ' Read header & ignore
        Dim header As Byte = br.ReadByte
        Dim padding() As Byte = br.ReadBytes(4)

        If header <> &H7E Then
            MessageBox.Show("This is not a valid cache file!", "Cache File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Do
                If CacheWorker.CancellationPending = True Then
                    Return Nothing
                    Exit Function
                End If
                Try
                    If simpleObjectTypes.Contains(currentObject) = True Then
                        'Call Me.DecodeSimpleObject(br)
                        Call Me.DecodeItem(br)
                    Else
                        If rowSetObjectTypes.Contains(currentObject) Then
                            'Call Me.DecodeRowSetObject(br)
                            Call Me.DecodeItem(br)
                        Else
                            Call Me.DecodeItem(br)
                        End If
                    End If
                Catch ex As EndOfStreamException
                    br.Close()
                    Exit Do
                End Try
            Loop
        End If
    End Function
    Private Function DecodeItem(ByVal br As BinaryReader) As Object
        Application.DoEvents()

        'indent += 1
        If CacheWorker.CancellationPending = True Then
            Return Nothing
            Exit Function
        End If

        ' Read the next byte
        Dim raw As Byte = br.ReadByte
        Dim op As Byte = CByte((raw And &H3F))
        Dim flags As Byte = CByte((raw And &HC0))
        Dim save As Boolean = False
        Dim retData As Object
        If CDbl(flags & &H40) = &H40 Then
            save = True
        End If

        ' Check if the op is in the op codes and follow the corresponding function
        If OpCodes.ContainsKey(CInt(op)) Then
            Select Case op
                Case &H1
                    retData = DecodeNone(br)
                Case &H2
                    retData = DecodeByteString(br)
                Case &H3
                    retData = DecodeLongLong(br)
                Case &H4
                    retData = DecodeLong(br)
                Case &H5
                    retData = DecodeSignedShort(br)
                Case &H6
                    retData = DecodeByte(br)
                Case &H7
                    retData = DecodeMinusOne(br)
                Case &H8
                    retData = DecodeZeroInteger(br)
                Case &H9
                    retData = DecodeOneInteger(br)
                Case &HA
                    retData = DecodeReal(br)
                Case &HB
                    retData = DecodeZeroReal(br)
                Case &HC
                    retData = DecodeObsolete(br)
                Case &HD
                    retData = DecodeBuffer(br)
                Case &HE
                    retData = DecodeEmptyString(br)
                Case &HF
                    retData = DecodeCharString(br)
                Case &H10
                    retData = DecodeByteString2(br)
                Case &H11
                    retData = DecodeStringTableItem(br)
                Case &H12
                    retData = DecodeUnknown(br)
                Case &H13
                    retData = DecodeUnknown(br)
                Case &H14
                    retData = DecodeTuple(br)
                Case &H15
                    retData = DecodeList(br)
                Case &H16
                    retData = DecodeDict(br)
                Case &H17
                    retData = DecodeObject(br)
                Case &H18
                    retData = DecodeUnknown(br)
                Case &H19
                    retData = DecodeUnknown(br)
                Case &H1A
                    retData = DecodeUnknown(br)
                Case &H1B
                    retData = DecodeSavedStreamElement(br)
                Case &H1C
                    retData = DecodeUnknown(br)
                Case &H1D
                    retData = DecodeObsolete(br)
                Case &H1E
                    retData = DecodeObsolete(br)
                Case &H1F
                    retData = DecodeTrue(br)
                Case &H20
                    retData = DecodeFalse(br)
                Case &H21
                    retData = DecodeUnknown(br)
                Case &H22
                    retData = DecodeRowHeader(br)
                Case &H23
                    retData = DecodeResultSet(br)
                    'Case &h23 retData = DecodeUnknown(br)
                Case &H24
                    retData = DecodeEmptyTuple(br)
                Case &H25
                    retData = DecodeOneTuple(br)
                Case &H26
                    retData = DecodeEmptyList(br)
                Case &H27
                    retData = DecodeOneList(br)
                Case &H28
                    retData = DecodeEmptyUnicodeString(br)
                Case &H29
                    retData = DecodeUnicodeCharString(br)
                Case &H2A
                    retData = DecodeDBRow(br)
                Case &H2B
                    retData = DecodeSubStream(br)
                Case &H2C
                    retData = DecodeTwoTuple(br)
                Case &H2D
                    retData = DecodeTerminator(br)
                Case &H2E
                    retData = DecodeUnicodeString(br)
                Case &H2F
                    retData = DecodeVarInteger(br)
                Case Else
                    retData = Nothing
            End Select
        Else
            'lvwData.Items.Add("Unknown opcode " & op.ToString & " in cache:")
            retData = Nothing
        End If
        ' Check for byte() return and check compression status
        'If op <> &H2A And TypeOf retData Is Byte() Then
        'If IsCompressed(CType(retData, Byte())) = True Then
        '    Dim mr As New MemoryStream(DecompressString(CType(retData, Byte())))
        '    Dim bbr As New BinaryReader(mr)
        '    Call DecodeStream(bbr)
        'End If
        'End If
        'indent -= 1
        Return retData
    End Function
    Private Function DecodeNone(ByVal br As BinaryReader) As Object
        Return Nothing
    End Function
    Private Function DecodeByteString(ByVal br As BinaryReader) As Object
        Dim length As Integer = GetStreamLength(br)
        Dim str() As Byte = br.ReadBytes(length)
        Dim strText As New StringBuilder
        For Each b As Byte In str
            If b >= 32 Then
                strText.Append(ChrW(b))
            End If
        Next
        Return str
    End Function
    Private Function DecodeLongLong(ByVal br As BinaryReader) As Object
        ' Returns 8 bytes
        Dim data As Int64 = br.ReadInt64
        Return data
    End Function
    Private Function DecodeLong(ByVal br As BinaryReader) As Object
        ' Returns 4 bytes
        Dim data As Int32 = br.ReadInt32
        Return data
    End Function
    Private Function DecodeSignedShort(ByVal br As BinaryReader) As Object
        ' Returns 2 bytes
        Dim data As Int16 = br.ReadInt16
        Return data
    End Function
    Private Function DecodeByte(ByVal br As BinaryReader) As Object
        ' Returns a byte
        Dim data As Byte = br.ReadByte
        Return data
    End Function
    Private Function DecodeMinusOne(ByVal br As BinaryReader) As Object
        Return -1
    End Function
    Private Function DecodeZeroInteger(ByVal br As BinaryReader) As Object
        Return 0
    End Function
    Private Function DecodeOneInteger(ByVal br As BinaryReader) As Object
        Return 1
    End Function
    Private Function DecodeReal(ByVal br As BinaryReader) As Object
        ' Read a double (8 bytes)
        Dim data As Double = br.ReadDouble
        Return data
    End Function
    Private Function DecodeZeroReal(ByVal br As BinaryReader) As Object
        Return 0.0
    End Function
    Private Function DecodeObsolete(ByVal br As BinaryReader) As Object
        Return Nothing
    End Function
    Private Function DecodeBuffer(ByVal br As BinaryReader) As Object
        Dim length As Integer = GetStreamLength(br)
        Dim data() As Byte = br.ReadBytes(length)
        Return data
    End Function
    Private Function DecodeEmptyString(ByVal br As BinaryReader) As Object
        Return ""
    End Function
    Private Function DecodeCharString(ByVal br As BinaryReader) As Object
        Dim data As Byte = br.ReadByte
        Return Chr(data)
    End Function
    Private Function DecodeByteString2(ByVal br As BinaryReader) As Object
        ' read the string length
        Dim strL As Integer = br.ReadByte
        Dim str() As Byte = br.ReadBytes(strL)
        ' read the string
        'If IsCompressed(str) = True Then
        '    Dim mr As New MemoryStream(DecompressString(str))
        '    Dim bbr As New BinaryReader(mr)
        '    Call DecodeStream(bbr)
        'End If
        Dim strText As New StringBuilder
        For Each b As Byte In str
            If b >= 32 Then
                strText.Append(ChrW(b))
            End If
        Next
        ' Check if we need to work out what type we are looking at
        If currentObject = "" Then
            If simpleObjectTypes.Contains(strText.ToString) Or rowSetObjectTypes.Contains(strText.ToString) Or unpackedRowSetObjectTypes.Contains(strText.ToString) Then
                currentObject = strText.ToString
                If unpackedRowSetObjectTypes.Contains(currentObject) = True Then
                    currentPilotID = CStr(DecodeItem(br))
                End If
            End If
        Else
            If currentObject = strText.ToString Then
                currentObject = ""
            End If
        End If
        'If IsCompressed(str) = False Then
        '    Return strText.ToString
        'Else
        '    Return str
        'End If
        Return str
    End Function
    Private Function DecodeStringTableItem(ByVal br As BinaryReader) As Object
        '  StringtableItem is an index into strings.txt, used as a shorthand for common strings
        Dim idx As Byte = br.ReadByte
        Try
            Return strings(idx - 1)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Private Function DecodeUnknown(ByVal br As BinaryReader) As Object
        Return Nothing
    End Function
    Private Function DecodeTuple(ByVal br As BinaryReader) As Object
        Dim items As New ArrayList
        Dim length As Integer = GetStreamLength(br)
        For i As Integer = 1 To length
            items.Add(DecodeItem(br))
        Next
        Return items
    End Function
    Private Function DecodeList(ByVal br As BinaryReader) As Object
        Dim items As New ArrayList
        Dim length As Integer = GetStreamLength(br)
        For i As Integer = 1 To length
            items.Add(DecodeItem(br))
        Next
        Return items
    End Function
    Private Function DecodeDict(ByVal br As BinaryReader) As Object
        ' Version 1
        Dim items As New SortedList
        Dim length As Integer = GetStreamLength(br)
        For i As Integer = 1 To length
            Dim Val As Object = DecodeItem(br)
            Dim key As Object = DecodeItem(br)
            If key IsNot Nothing Then
                'items.Add(key, Val)
            End If
        Next
        Return items
    End Function
    Private Function DecodeObject(ByVal br As BinaryReader) As Object
        Dim typ As Object = DecodeItem(br)
        Dim kwargs As Object = DecodeItem(br)
        Return Nothing
    End Function
    Private Function DecodeSavedStreamElement(ByVal br As BinaryReader) As Object
        Return Nothing
    End Function
    Private Function DecodeTrue(ByVal br As BinaryReader) As Object
        Return True
    End Function
    Private Function DecodeFalse(ByVal br As BinaryReader) As Object
        Return False
    End Function
    Private Function DecodeRowHeader(ByVal br As BinaryReader) As Object
        Dim header As Object = DecodeItem(br)
        Return Nothing
    End Function
    Private Function DecodeResultSet(ByVal br As BinaryReader) As Object
        Dim header As Object = DecodeItem(br)
        Return Nothing
    End Function
    Private Function DecodeEmptyTuple(ByVal br As BinaryReader) As Object
        Return New ArrayList
    End Function
    Private Function DecodeOneTuple(ByVal br As BinaryReader) As Object
        Dim items As New ArrayList
        items.Add(DecodeItem(br))
        Return items
    End Function
    Private Function DecodeEmptyList(ByVal br As BinaryReader) As Object
        Return New ArrayList
    End Function
    Private Function DecodeOneList(ByVal br As BinaryReader) As Object
        Return DecodeItem(br)
    End Function
    Private Function DecodeEmptyUnicodeString(ByVal br As BinaryReader) As Object
        Return ""
    End Function
    Private Function DecodeUnicodeCharString(ByVal br As BinaryReader) As Object
        Dim raw As Int16 = br.ReadInt16
        Return ChrW(raw)
    End Function
    Private Function DecodeDBRow(ByVal br As BinaryReader) As Object
        Dim header As Byte = br.ReadByte
        If header <> &H1B Then
            Return header
            Exit Function
        End If
        header = br.ReadByte
        Dim length As Integer = GetStreamLength(br)
        Dim Data As Object = br.ReadBytes(length)
        'recordCount += 1
        Dim strData As String = ""
        Return Data
    End Function
    Private Function DecodeSubStream(ByVal br As BinaryReader) As Object
        Dim length As Integer = GetStreamLength(br)
        If simpleObjectTypes.Contains(currentObject) = True Then
            'Call Me.DecodeSimpleObject(br)
            Call Me.DecodeItem(br)
        Else
            If rowSetObjectTypes.Contains(currentObject) Then
                'Call Me.DecodeRowSetObject(br)
                Call Me.DecodeItem(br)
            Else
                If unpackedRowSetObjectTypes.Contains(currentObject) Then
                    Call Me.DecodeUnpackedRowSetObject(br)
                Else
                    Call Me.DecodeItem(br)
                End If
            End If
        End If
        Return Nothing
    End Function
    Private Function DecodeTwoTuple(ByVal br As BinaryReader) As Object
        Dim tu1 As Object = DecodeItem(br)
        Dim tu2 As Object = DecodeItem(br)
        Dim ret As New ArrayList
        ret.Add(tu1)
        ret.Add(tu2)
        Return ret
    End Function
    Private Function DecodeTerminator(ByVal br As BinaryReader) As Object
        Return &H2D
    End Function
    Private Function DecodeUnicodeString(ByVal br As BinaryReader) As Object
        Dim length As Integer = GetStreamLength(br)
        Dim data() As Byte = br.ReadBytes(length)
        Return System.Text.Encoding.Default.GetString(data)
    End Function
    Private Function DecodeVarInteger(ByVal br As BinaryReader) As Object
        'Variable length integer
        Dim length As Integer = GetStreamLength(br)
        Dim data() As Byte = br.ReadBytes(length)
        Dim ret As Long = 0
        Dim shift As Long = 1
        For Each dat As Byte In data
            ret += dat * shift
            If shift < Math.Pow(2, 56) Then
                shift *= 256
            End If
        Next
        Return ret
    End Function
    'Private Sub DecodeSimpleObject(ByVal br As BinaryReader)
    '    'MessageBox.Show("Decoding the invTypes table...")
    '    startTime = Now
    '    recordFlag = True
    '    Dim raw As Byte
    '    ' Read the TwoTupleByte
    '    raw = br.ReadByte
    '    ' Read the ListByte
    '    raw = br.ReadByte
    '    Call Me.DecodeList(br) ' Discard as this is the header
    '    raw = br.ReadByte
    '    ' Read the next ListByte
    '    Dim itemData As ArrayList = CType(Me.DecodeTypeList(br), ArrayList)
    '    Dim sw As New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & currentObject & ".csv")
    '    Select Case currentObject
    '        Case "config.BulkData.types"
    '            Dim newType As Type
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newType = New Type
    '                    newType.typeID = item(0)
    '                    newType.groupID = item(1)
    '                    newType.typeName = item(2)
    '                    newType.description = item(3)
    '                    newType.graphicID = item(4)
    '                    newType.radius = item(5)
    '                    If IsNumeric(item(6)) = False Then
    '                        newType.mass = 0
    '                    Else
    '                        newType.mass = item(6)
    '                    End If
    '                    newType.volume = item(7)
    '                    newType.capacity = item(8)
    '                    newType.portionSize = item(9)
    '                    newType.raceID = item(10)
    '                    newType.basePrice = item(11)
    '                    newType.published = item(12)
    '                    newType.marketGroupID = item(13)
    '                    newType.chanceOfDuplicating = item(14)
    '                    newType.dataID = item(15)
    '                    For col As Integer = 0 To 15
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.Write("$$")
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '        Case "config.BulkData.groups"
    '            Dim newGroup As Group
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newGroup = New Group
    '                    newGroup.categoryID = item(0)
    '                    newGroup.groupID = item(1)
    '                    newGroup.groupName = item(2)
    '                    newGroup.description = item(3)
    '                    newGroup.graphicID = item(4)
    '                    newGroup.useBasePrice = item(5)
    '                    newGroup.allowManufacture = item(6)
    '                    newGroup.allowRecycle = item(7)
    '                    newGroup.anchored = item(8)
    '                    newGroup.anchorable = item(9)
    '                    newGroup.fittableNonSingleton = item(10)
    '                    newGroup.published = item(11)
    '                    newGroup.dataID = item(12)
    '                    For col As Integer = 0 To 12
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '        Case "config.BulkData.categories"
    '            Dim newCategory As Category
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newCategory = New Category
    '                    newCategory.categoryID = item(0)
    '                    newCategory.categoryName = item(1)
    '                    newCategory.description = item(2)
    '                    newCategory.graphicID = item(3)
    '                    newCategory.published = item(4)
    '                    newCategory.dataID = item(5)
    '                    For col As Integer = 0 To 5
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '        Case "config.BulkData.metagroups"
    '            Dim newGroup As MetaGroup
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newGroup = New MetaGroup
    '                    newGroup.metaGroupID = item(0)
    '                    newGroup.metaGroupName = item(1)
    '                    newGroup.description = item(2)
    '                    newGroup.graphicID = item(3)
    '                    newGroup.dataID = item(4)
    '                    For col As Integer = 0 To 4
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '        Case "config.BulkData.invmetatypes"
    '            Dim newType As MetaType
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newType = New MetaType
    '                    newType.typeID = item(0)
    '                    newType.parentTypeID = item(1)
    '                    newType.metaGroupID = item(2)
    '                    For col As Integer = 0 To 2
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '        Case "config.BulkData.graphics"
    '            Dim newType As Graphics
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newType = New Graphics
    '                    newType.graphicID = item(0)
    '                    newType.url3D = item(1)
    '                    newType.urlWeb = item(2)
    '                    newType.icon = item(3)
    '                    newType.urlSound = item(4)
    '                    newType.explosionID = item(5)
    '                    For col As Integer = 0 To 5
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '        Case "config.BulkData.units"
    '            Dim newType As Units
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newType = New Units
    '                    newType.unitID = item(0)
    '                    newType.unitName = item(1)
    '                    newType.displayName = item(2)
    '                    For col As Integer = 0 To 2
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next

    '    End Select
    '    sw.Flush()
    '    sw.Close()
    '    MessageBox.Show("Finished Decoding the " & currentObject & " data!")
    'End Sub
    'Private Sub DecodeRowSetObject(ByVal br As BinaryReader)
    '    'MessageBox.Show("Decoding the invTypes table...")
    '    startTime = Now
    '    recordFlag = True
    '    Dim raw As Object
    '    raw = br.ReadByte ' Read the Object Byte
    '    raw = DecodeItem(br) ' Read the Object Name
    '    raw = br.ReadByte ' read the Dict byte
    '    raw = GetStreamLength(br) ' Read the dict length
    '    raw = DecodeItem(br) ' Decode List values
    '    raw = DecodeItem(br) ' Decode List key
    '    raw = DecodeItem(br) ' Decode Byte value
    '    raw = DecodeItem(br) ' Decode Byte key
    '    raw = br.ReadByte ' read the List byte
    '    ' Process the list
    '    Dim itemData As ArrayList = CType(Me.DecodeTypeList(br), ArrayList)
    '    Dim sw As New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & currentObject & ".csv")
    '    Select Case currentObject
    '        Case "config.StaticOwners"
    '            Dim newType As StaticOwner
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newType = New StaticOwner
    '                    newType.ownerID = item(0)
    '                    newType.ownerName = item(1)
    '                    newType.typeID = item(2)
    '                    For col As Integer = 0 To 2
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '        Case "GetAgents"
    '            Dim newType As Agent
    '            Dim item As New ArrayList
    '            For i As Integer = 0 To itemData.Count - 1
    '                If TypeOf itemData(i) Is ArrayList Then
    '                    item = itemData(i)
    '                    newType = New Agent
    '                    newType.agentID = item(0)
    '                    newType.agentTypeID = item(1)
    '                    newType.divisionID = item(2)
    '                    newType.level = item(3)
    '                    newType.stationID = item(4)
    '                    newType.bloodlineID = item(5)
    '                    If CInt(item(6)) < 128 Then
    '                        item(6) = CInt(item(6))
    '                    Else
    '                        item(6) = CInt(item(6) - 256)
    '                    End If
    '                    newType.corporationID = item(7)
    '                    newType.gender = item(8)
    '                    For col As Integer = 0 To 8
    '                        If TypeOf item(col) Is String Then
    '                            item(col) = item(col).ToString.Replace("""", "'")
    '                            sw.Write("""" & item(col) & """,")
    '                        Else
    '                            sw.Write(item(col) & ",")
    '                        End If
    '                    Next
    '                    sw.WriteLine("")
    '                End If
    '            Next
    '    End Select
    '    sw.Flush()
    '    sw.Close()
    '    raw = DecodeItem(br) ' Decode the List Key
    '    MessageBox.Show("Finished Decoding the " & currentObject & " data!")
    'End Sub
    Private Sub DecodeUnpackedRowSetObject(ByVal br As BinaryReader)
        'startTime = Now
        'recordFlag = True
        MyStandings = New StandingsData
        Dim StandingsValues As New SortedList
        Dim StandingsNames As New SortedList
        MyStandings.OwnerID = currentPilotID
        MyStandings.CacheType = currentObject
        Dim NPCList As New StringBuilder
        'Dim sw As New StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & currentObject & ".csv")
        Select Case currentObject
            Case "GetCharStandings", "GetCorpStandings"
                Dim raw As Object
                raw = br.ReadBytes(44) ' Read the excess stuff
                raw = br.ReadByte ' read the List byte
                Dim itemData As New ArrayList
                'MessageBox.Show("Decoding PC Standing Values")
                If CInt(raw) = 39 Then
                    ' Process a one-item list
                    itemData = CType(Me.DecodeTypeOneList(br), ArrayList)
                Else
                    If CInt(raw) = 38 Then
                        ' Process an Empty List errr do nothing?
                    Else
                        ' Process a list
                        itemData = CType(Me.DecodeTypeList(br), ArrayList)
                    End If
                End If
                If CInt(raw) <> 38 Then
                    Dim item As New ArrayList
                    For i As Integer = 0 To itemData.Count - 1
                        If TypeOf itemData(i) Is ArrayList Then
                            item = CType(itemData(i), ArrayList)
                            StandingsValues.Add(CStr(item(0)), CStr(item(1)))
                            'newType = New Standing
                            'newType.ID = CStr(item(0))
                            'newType.Value = CStr(item(1))
                            'For col As Integer = 0 To 1
                            '    If TypeOf item(col) Is String Then
                            '        item(col) = item(col).ToString.Replace("""", "'")
                            '        sw.Write("""" & CStr(item(col)) & """,")
                            '    Else
                            '        sw.Write(CStr(item(col)) & ",")
                            '    End If
                            'Next
                            'sw.WriteLine("")
                        End If
                    Next
                End If
                raw = br.ReadBytes(21) ' Read Space
                raw = br.ReadByte ' Read the List Byte
                ' Process the next list
                'MessageBox.Show("Decoding PC Names")
                If CInt(raw) = 39 Then
                    ' Process a one-item list
                    itemData = CType(Me.DecodeTypeOneList(br), ArrayList)
                Else
                    If CInt(raw) = 38 Then
                        ' Process an Empty List errr do nothing?
                    Else
                        ' Process a list
                        itemData = CType(Me.DecodeTypeList(br), ArrayList)
                    End If
                End If
                If CInt(raw) <> 38 Then
                    Dim item As New ArrayList
                    For i As Integer = 0 To itemData.Count - 1
                        If TypeOf itemData(i) Is ArrayList Then
                            item = CType(itemData(i), ArrayList)
                            StandingsNames.Add(CStr(item(0)), CStr(item(1)))
                            'newNameType = New StandingName
                            'newNameType.ID = CStr(item(0))
                            'newNameType.Name = CStr(item(1))
                            'For col As Integer = 0 To 1
                            '    If TypeOf item(col) Is String Then
                            '        item(col) = item(col).ToString.Replace("""", "'")
                            '        sw.Write("""" & CStr(item(col)) & """,")
                            '    Else
                            '        sw.Write(CStr(item(col)) & ",")
                            '    End If
                            'Next
                            'sw.WriteLine("")
                        End If
                    Next
                End If
                raw = br.ReadBytes(33) ' Read Space
                raw = br.ReadByte ' Read the List Byte
                'MessageBox.Show("Decoding NPC Standing Values")
                If CInt(raw) = 39 Then
                    ' Process a one-item list
                    itemData = CType(Me.DecodeTypeOneList(br), ArrayList)
                Else
                    If CInt(raw) = 38 Then
                        ' Process an Empty List errr do nothing?
                    Else
                        ' Process a list
                        itemData = CType(Me.DecodeTypeList(br), ArrayList)
                    End If
                End If
                If CInt(raw) <> 38 Then
                    Dim item As New ArrayList
                    For i As Integer = 0 To itemData.Count - 1
                        If TypeOf itemData(i) Is ArrayList Then
                            item = CType(itemData(i), ArrayList)
                            ' Check if it has been added before and change it if it has!
                            If StandingsValues.Contains(CStr(item(0))) = True Then
                                StandingsValues(CStr(item(0))) = CStr(item(1))
                            Else
                                StandingsValues.Add(CStr(item(0)), CStr(item(1)))
                            End If
                            NPCList.Append(CStr(item(0)) & ",")
                            'newNPCType = New Standing
                            'newNPCType.ID = CStr(item(0))
                            'newNPCType.Value = CStr(item(1))
                            'For col As Integer = 0 To 1
                            '    If TypeOf item(col) Is String Then
                            '        item(col) = item(col).ToString.Replace("""", "'")
                            '        sw.Write("""" & CStr(item(col)) & """,")
                            '    Else
                            '        sw.Write(CStr(item(col)) & ",")
                            '    End If
                            'Next
                            'sw.WriteLine("")
                        End If
                    Next
                End If
        End Select
        ' Query the DB for the NPCNames (if not empty)
        If NPCList.ToString <> "" Then
            'MessageBox.Show("Getting NPC Names From DB")
            Dim strSQL As String = "SELECT * FROM eveNames WHERE itemID IN (" & NPCList.ToString.TrimEnd(",".ToCharArray) & ");"
            Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            For Each nameRow As DataRow In eveData.Tables(0).Rows
                StandingsNames.Add(nameRow.Item("itemID").ToString, nameRow.Item("itemName").ToString)
            Next
            eveData = Nothing
        End If
        MyStandings.StandingNames = StandingsNames
        MyStandings.StandingValues = StandingsValues
        'sw.Flush()
        'sw.Close()
        'MessageBox.Show("Finished Decoding the " & currentObject & " data!")
    End Sub
    Private Function DecodeTypeList(ByVal br As BinaryReader) As Object
        Dim item As New ArrayList
        Dim items As New ArrayList
        Dim length As Integer = GetStreamLength(br)
        'pbProgress.Maximum = length
        Dim i As Long = 0
        If currentObject = "config.BulkData.types" Then i = i - 1
        Do
            'currentTime = Now
            'elapsedtime = currentTime - startTime
            'lvwData.Items.Add("Data Row " & i)
            Try
                item = CType(DecodeItem(br), ArrayList)
                items.Add(item)
                i += 1
                'pbProgress.Value = i
                'lblProgress.Text = "Processed " & i & " of " & length & " (" & FormatNumber(i / length * 100, 2) & "%) [" & FormatNumber(elapsedtime.TotalSeconds, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "s elapsed, " & FormatNumber((length / i * elapsedtime.TotalSeconds) - elapsedtime.TotalSeconds, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "s remaining]"
            Catch e As Exception
            End Try
        Loop Until i = length
        Return items
    End Function
    Private Function DecodeTypeOneList(ByVal br As BinaryReader) As Object
        Dim item As New ArrayList
        Dim items As New ArrayList
        Dim length As Integer = 1
        'If OutputToList = True Then
        '    lvwData.Items(lvwData.Items.Count - 1).SubItems.Add("List Length: " & length)
        'End If
        'pbProgress.Maximum = length
        Dim i As Long = 0
        If currentObject = "config.BulkData.types" Then i = i - 1
        Do
            'currentTime = Now
            'elapsedtime = currentTime - startTime
            'lvwData.Items.Add("Data Row " & i)
            Try
                item = CType(DecodeItem(br), ArrayList)
                items.Add(item)
                i += 1
                'pbProgress.Value = i
                'lblProgress.Text = "Processed " & i & " of " & length & " (" & FormatNumber(i / length * 100, 2) & "%) [" & FormatNumber(elapsedtime.TotalSeconds, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "s elapsed, " & FormatNumber((length / i * elapsedtime.TotalSeconds) - elapsedtime.TotalSeconds, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "s remaining]"
            Catch e As Exception
            End Try
        Loop Until i = length
        Return items
    End Function
    Private Function GetStreamLength(ByVal br As BinaryReader) As Integer
        'Read length from the stream
        'Normally a byte, if that byte is 0xff then the following four bytes is a ULONG length field
        Dim length As Integer
        length = br.ReadByte
        If length = &HFF Then
            length = br.ReadInt32
        End If
        Return length
    End Function
#End Region

#Region "ZLib Decompression Functions"
    'Private Function DecompressString(ByVal bData() As Byte) As Byte()
    '    Dim bw As New BinaryWriter(New FileStream("compressed.txt", FileMode.Create))
    '    bw.Write(bData)
    '    bw.Close()
    '    Dim outFileStream As New System.IO.FileStream("decompressed.txt", System.IO.FileMode.Create)
    '    Dim zStream As New zlib.ZOutputStream(outFileStream)
    '    Dim inFileStream As New System.IO.FileStream("compressed.txt", System.IO.FileMode.Open)
    '    Try
    '        If CopyStream(inFileStream, zStream) = False Then
    '            zStream.Close()
    '            outFileStream.Close()
    '            inFileStream.Close()
    '            Return Nothing
    '        Else
    '            zStream.Close()
    '            outFileStream.Close()
    '            inFileStream.Close()
    '            Dim fi As New FileInfo("decompressed.txt")
    '            Dim br As New BinaryReader(New FileStream("decompressed.txt", FileMode.Open))
    '            Dim retData() As Byte = br.ReadBytes(fi.Length)
    '            br.Close()
    '            Return retData
    '        End If
    '    Catch ex As Exception
    '        Return Nothing
    '    End Try
    'End Function
    'Private Function CopyStream(ByRef input As System.IO.Stream, ByRef output As System.IO.Stream) As Boolean
    '    Try
    '        Dim num1 As Integer
    '        Dim buffer1 As Byte() = New Byte(20000 - 1) {}
    '        num1 = input.Read(buffer1, 0, 20000)
    '        Do While (num1 > 0)
    '            output.Write(buffer1, 0, num1)
    '            num1 = input.Read(buffer1, 0, 20000)
    '        Loop
    '        output.Flush()
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function
    'Private Function IsCompressed(ByVal bData() As Byte) As Boolean
    '    Try
    '        If DecompressString(bData) IsNot Nothing Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function
#End Region
End Class

Public Class CachedObjects

End Class

Public Class Type
    Public typeID As Integer
    Public groupID As Integer
    Public typeName As String
    Public description As String
    Public graphicID As Integer
    Public radius As Double
    Public mass As Double
    Public volume As Double
    Public capacity As Double
    Public portionSize As Integer
    Public raceID As Integer
    Public basePrice As Double
    Public published As Integer
    Public marketGroupID As Integer
    Public chanceOfDuplicating As Double
    Public dataID As Long
End Class

Public Class Group
    Public categoryID As Integer
    Public groupID As Integer
    Public groupName As String
    Public description As String
    Public graphicID As Integer
    Public useBasePrice As Integer
    Public allowManufacture As Integer
    Public allowRecycle As Integer
    Public anchored As Integer
    Public anchorable As Integer
    Public fittableNonSingleton As Integer
    Public published As Integer
    Public dataID As Long
End Class

Public Class Category
    Public categoryID As Integer
    Public categoryName As String
    Public description As String
    Public graphicID As Integer
    Public published As Integer
    Public dataID As Long
End Class

Public Class MetaGroup
    Public metaGroupID As Integer
    Public metaGroupName As String
    Public description As String
    Public graphicID As Integer
    Public dataID As Long
End Class

Public Class MetaType
    Public typeID As Integer
    Public parentTypeID As Integer
    Public metaGroupID As Integer
End Class

Public Class Graphics
    Public graphicID As Integer
    Public url3D As String
    Public urlWeb As String
    Public icon As String
    Public urlSound As String
    Public explosionID As Integer
End Class

Public Class Units
    Public unitID As Integer
    Public unitName As String
    Public displayName As String
End Class

Public Class StaticOwner
    Public ownerID As Long
    Public ownerName As String
    Public typeID As Integer
End Class

Public Class Agent
    Public agentID As Long
    Public agentTypeID As Integer
    Public divisionID As Integer
    Public level As Integer
    Public stationID As Long
    Public bloodlineID As Integer
    Public quality As Integer
    Public corporationID As Long
    Public gender As Integer
End Class

<Serializable()> Public Class Standing
    Public ID As String
    Public Value As String
End Class

<Serializable()> Public Class StandingName
    Public ID As String
    Public Name As String
End Class

<Serializable()> Public Class StandingsData
    Public OwnerID As String
    Public OwnerName As String
    Public CacheType As String
    Public StandingValues As New SortedList
    Public StandingNames As New SortedList
End Class


