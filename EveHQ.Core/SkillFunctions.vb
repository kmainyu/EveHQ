' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2008  Lee Vessey
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
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Xml

Public Class SkillFunctions

    Shared eveData As Data.DataSet
   
    Public Shared Function Roman(ByVal dec As Integer) As String

        Roman = ""

        Dim intVal(12) As Integer
        Dim strVal(12) As String, intLoopCounter As Integer

        intVal(0) = 1 : strVal(0) = "I"
        intVal(1) = 4 : strVal(1) = "IV"
        intVal(2) = 5 : strVal(2) = "V"
        intVal(3) = 9 : strVal(3) = "IX"
        intVal(4) = 10 : strVal(4) = "X"
        intVal(5) = 40 : strVal(5) = "XL"
        intVal(6) = 50 : strVal(6) = "L"
        intVal(7) = 90 : strVal(7) = "XC"
        intVal(8) = 100 : strVal(8) = "C"
        intVal(9) = 400 : strVal(9) = "CD"
        intVal(10) = 500 : strVal(10) = "D"
        intVal(11) = 900 : strVal(11) = "CM"
        intVal(12) = 1000 : strVal(12) = "M"

        If dec = 0 Then
            Roman = ""
            Exit Function
        End If

        For intLoopCounter = 12 To 0 Step -1
            Do
                If dec >= intVal(intLoopCounter) Then
                    Roman = Roman & "" & strVal(intLoopCounter)
                    dec = dec - intVal(intLoopCounter)
                End If
            Loop Until dec < intVal(intLoopCounter)
        Next intLoopCounter
    End Function
    Public Shared Function CalculateSPLevel(ByVal Rank As Integer, ByVal Level As Integer) As Double
        Dim SP As Double = 0
        If Level > 0 Then
            SP = 250 * Rank * (2 ^ (2.5 * (Level - 1)))
        End If
        Return SP
    End Function         'CalculateSPLevel
    Public Shared Function CalculateSkillSPLevel(ByVal skillID As String, ByVal Level As Integer) As Double
        Dim cSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
        cSkill = CType(EveHQ.Core.HQ.SkillListID(skillID), SkillList)
        Dim rank As Integer = cSkill.Rank
        Return CalculateSPLevel(rank, Level)
    End Function
    Public Shared Function TimeToString(ByVal sTime As Double, Optional ByVal skillTime As Boolean = True) As String

        Dim days, hours, minutes, seconds As Long
        Dim strTime As String = ""


        If sTime <= 0 Then       ' Changed from "= 0" to "<= 0" due to a CCP bug!!
            If skillTime = True Then
                strTime = "Training Complete"
            Else
                strTime = "0s"
            End If
        Else
            days = CInt(Int(sTime / (60 * 60 * 24)))
            sTime = sTime - (days * 60 * 60 * 24)

            hours = CInt(Int(sTime / (60 * 60)))
            sTime = sTime - (hours * 60 * 60)

            minutes = CInt(Int(sTime / 60))
            sTime = sTime - (minutes * 60)

            seconds = CInt(Int(sTime))

            If days <> 0 Then
                strTime &= days & "d "
            End If
            If hours <> 0 Then
                strTime &= hours & "h "
            End If
            If minutes <> 0 Then
                strTime &= minutes & "m "
            End If
            If seconds <> 0 Then
                strTime &= seconds & "s"
            End If
        End If

            Return strTime.Trim(" ".ToCharArray)

    End Function             'TimeToString
    Public Shared Function TimeToStringAll(ByVal sTime As Double) As String
        Dim days, hours, minutes, seconds As Integer
        Dim strTime As String = ""
        Dim negative As Boolean = False

        If sTime < 0 Then
            sTime = -sTime
            negative = True
        End If

        days = CInt(Int(sTime / (60 * 60 * 24)))
        sTime = sTime - (days * 60 * 60 * 24)

        hours = CInt(Int(sTime / (60 * 60)))
        sTime = sTime - (hours * 60 * 60)

        minutes = CInt(Int(sTime / 60))
        sTime = sTime - (minutes * 60)

        seconds = CInt(Int(sTime))

        If days <> 0 Then
            strTime &= days & "d "
        End If
        If hours <> 0 Then
            strTime &= hours & "h "
        End If
        If minutes <> 0 Then
            strTime &= minutes & "m "
        End If

        ' Put the seconds in regardless
        strTime &= seconds & "s"

        If negative = True Then
            strTime = "- " & strTime
        End If

        Return strTime.Trim(" ".ToCharArray)
    End Function            'TimeToStringAll
    Public Shared Function CacheTimeToString(ByVal sTime As Double) As String

        Dim days, hours, minutes, seconds As Integer
        Dim strTime As String = ""


        If sTime <= 0 Then       ' Changed from "= 0" to "<= 0" due to a CCP bug!!
            strTime = "Update Available"
        Else
            days = CInt(Int(sTime / (60 * 60 * 24)))
            sTime = sTime - (days * 60 * 60 * 24)

            hours = CInt(Int(sTime / (60 * 60)))
            sTime = sTime - (hours * 60 * 60)

            minutes = CInt(Int(sTime / 60))
            sTime = sTime - (minutes * 60)

            seconds = CInt(Int(sTime))

            If days <> 0 Then
                strTime &= days & "d "
            End If
            If hours <> 0 Then
                strTime &= hours & "h "
            End If
            If minutes <> 0 Then
                strTime &= minutes & "m "
            End If
            If seconds <> 0 Then
                strTime &= seconds & "s"
            End If
        End If

        Return strTime.Trim(" ".ToCharArray)

    End Function             'TimeToString
    Public Shared Function ConvertEveTimeToLocal(ByVal EveTime As Date) As Date
        Dim localzone As TimeZone
        Dim LocalTime As DateTime
        ' Calculate time offset from Eve Time (if applicable)
        localzone = TimeZone.CurrentTimeZone
        ' Calculate the local time and UTC offset.
        LocalTime = localzone.ToLocalTime(EveTime)
        Return LocalTime
    End Function    'ConvertEveTimeToLocal
    Public Shared Function ConvertLocalTimeToEve(ByVal LocalTime As Date) As Date
        Dim localzone As TimeZone
        Dim eveTime As DateTime
        ' Calculate time offset from Eve Time (if applicable)
        localzone = TimeZone.CurrentTimeZone
        ' Calculate the local time and UTC offset.
        eveTime = localzone.ToUniversalTime(LocalTime)
        Return eveTime
    End Function
    Public Shared Function CalcCurrentSkillTime(ByRef myPilot As EveHQ.Core.Pilot) As Long
        If myPilot.Training = True Then
            Dim sTime, eTime, cTime As Date
            Dim tSec, eSec, fSec As Long
            sTime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(myPilot.TrainingStartTime)
            eTime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(myPilot.TrainingEndTime)
            cTime = Now
            tSec = DateDiff(DateInterval.Second, sTime, eTime)
            eSec = DateDiff(DateInterval.Second, sTime, cTime)
            fSec = Math.Max(tSec - eSec, 0)
            myPilot.TrainingCurrentTime = fSec
            Return fSec
        Else
            Return 0
        End If
    End Function     'CalcCurrentSkillTime
    Public Shared Function CalcCurrentSkillPoints(ByRef myPilot As EveHQ.Core.Pilot) As Long
        If myPilot.Training = True Then
            Dim sTime, eTime, cTime As Date
            Dim tSec, eSec, fSec As Long
            Dim sSP, eSP, cSP As Integer
            sTime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(myPilot.TrainingStartTime)
            eTime = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(myPilot.TrainingEndTime)
            cTime = Now
            tSec = DateDiff(DateInterval.Second, sTime, eTime)
            eSec = DateDiff(DateInterval.Second, sTime, cTime)
            fSec = Math.Max(tSec - eSec, 0)
            sSP = myPilot.TrainingStartSP
            eSP = myPilot.TrainingEndSP
            cSP = CInt(((tSec - fSec) / tSec) * (eSP - sSP))
            myPilot.TrainingCurrentSP = cSP
            Return cSP
        Else
            Return 0
        End If
    End Function   'CalcCurrentSkillPoints
    Public Shared Function CalcProductionTime(ByRef myPilot As EveHQ.Core.Pilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.04 * CDbl(myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Industry))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcResearchProdTime(ByRef myPilot As EveHQ.Core.Pilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Research))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcResearchMatTime(ByRef myPilot As EveHQ.Core.Pilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Metallurgy))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcResearchCopyTime(ByRef myPilot As EveHQ.Core.Pilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Science))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcWasteFactor(ByRef myPilot As EveHQ.Core.Pilot, ByVal wf As Double) As Double
        Dim newWF As Double = 0
        Try
            newWF = ((1 + wf) * (1.25 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.ProductionEfficiency))))) - 1
        Catch ex As Exception
            newWF = wf
        End Try
        Return newWF
    End Function
    Public Shared Function LoadEveSkillData() As Boolean
        EveHQ.Core.HQ.SkillListName.Clear()
        EveHQ.Core.HQ.SkillListID.Clear()
        EveHQ.Core.HQ.SkillGroups.Clear()

        ' Get details of skill groups from the database
        Dim strSQL As String = ""
        Dim strSQLR As String = ""
        strSQL &= "SELECT * FROM invGroups WHERE invGroups.categoryID=16 ORDER BY groupName;"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData Is Nothing Then
            Return False
            Exit Function
        Else
            If eveData.Tables(0).Rows.Count = 0 Then
                Return False
                Exit Function
            End If
        End If
        ' Add data to the skillgroups list
        For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            Dim newSkillGroup As EveHQ.Core.SkillGroup
            newSkillGroup = New EveHQ.Core.SkillGroup
            newSkillGroup.ID = eveData.Tables(0).Rows(row).Item("groupID").ToString.Trim
            newSkillGroup.Name = eveData.Tables(0).Rows(row).Item("groupName").ToString.Trim
            EveHQ.Core.HQ.SkillGroups.Add(newSkillGroup, newSkillGroup.ID)
        Next

        ' Get details of skill data from database
        strSQL = ""
        strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName,  invTypes.basePrice, invTypes.published, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
        strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
        strSQL &= " WHERE invCategories.categoryID=16 ORDER BY typeName;"

        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

        ' Add data to the skillitem lists
        Dim attValue As Double = 0
        For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            ' Check if the typeID already exists
            Dim newSkill As EveHQ.Core.SkillList
            If EveHQ.Core.HQ.SkillListID.Contains(eveData.Tables(0).Rows(row).Item("typeID").ToString) = False Then
                newSkill = New EveHQ.Core.SkillList
                If IsDBNull(eveData.Tables(0).Rows(row).Item("description")) = False Then
                    newSkill.Description = eveData.Tables(0).Rows(row).Item("description").ToString.Trim
                Else
                    newSkill.Description = ""
                End If
                newSkill.ID = eveData.Tables(0).Rows(row).Item("typeID").ToString.Trim
                newSkill.GroupID = eveData.Tables(0).Rows(row).Item("groupID").ToString.Trim
                newSkill.Name = eveData.Tables(0).Rows(row).Item("typeName").ToString.Trim
                newSkill.BasePrice = CDbl(eveData.Tables(0).Rows(row).Item("basePrice")) * 0.9
                ' Check for salvage drone op skill in db!
                If newSkill.ID = "3440" Then
                    newSkill.Published = True
                Else
                    newSkill.Published = CBool(eveData.Tables(0).Rows(row).Item("published"))
                End If
                EveHQ.Core.HQ.SkillListID.Add(newSkill, newSkill.ID)
            Else
                newSkill = CType(EveHQ.Core.HQ.SkillListID(eveData.Tables(0).Rows(row).Item("typeID").ToString), SkillList)
            End If
            If IsDBNull(eveData.Tables(0).Rows(row).Item("valueInt")) = False Then
                attValue = CDbl(eveData.Tables(0).Rows(row).Item("valueInt"))
            Else
                attValue = CDbl(eveData.Tables(0).Rows(row).Item("valueFloat"))
            End If
            Select Case CInt(eveData.Tables(0).Rows(row).Item("attributeID"))
                Case 180
                    Select Case CInt(attValue)
                        Case 164
                            newSkill.PA = "Charisma"
                        Case 165
                            newSkill.PA = "Intelligence"
                        Case 166
                            newSkill.PA = "Memory"
                        Case 167
                            newSkill.PA = "Perception"
                        Case 168
                            newSkill.PA = "Willpower"
                    End Select
                Case 181
                    Select Case CInt(attValue)
                        Case 164
                            newSkill.SA = "Charisma"
                        Case 165
                            newSkill.SA = "Intelligence"
                        Case 166
                            newSkill.SA = "Memory"
                        Case 167
                            newSkill.SA = "Perception"
                        Case 168
                            newSkill.SA = "Willpower"
                    End Select
                Case 182
                    newSkill.PS = CStr(attValue)
                Case 183
                    newSkill.SS = CStr(attValue)
                Case 184
                    newSkill.TS = CStr(attValue)
                Case 275
                    newSkill.Rank = CInt(attValue)
                Case 277
                    newSkill.PSL = CInt(attValue)
                Case 278
                    newSkill.SSL = CInt(attValue)
                Case 279
                    newSkill.TSL = CInt(attValue)
            End Select
        Next
        Dim currentSkill As EveHQ.Core.SkillList
        For Each currentSkill In EveHQ.Core.HQ.SkillListID
            ' Check for missing primary/secondary skill but available secondary/tertiary skill
            If CInt(currentSkill.PS) = 0 And CInt(currentSkill.SS) <> 0 Then
                Dim tmpSkill, tmpLevel As Integer
                tmpSkill = CInt(currentSkill.PS)
                tmpLevel = currentSkill.PSL
                currentSkill.PS = currentSkill.SS
                currentSkill.PSL = currentSkill.SSL
                currentSkill.SS = tmpSkill.ToString
                currentSkill.SSL = tmpLevel
            End If
            If CInt(currentSkill.SS) = 0 And CInt(currentSkill.TS) <> 0 Then
                Dim tmpSkill, tmpLevel As Integer
                tmpSkill = CInt(currentSkill.SS)
                tmpLevel = currentSkill.SSL
                currentSkill.SS = currentSkill.TS
                currentSkill.SSL = currentSkill.TSL
                currentSkill.TS = tmpSkill.ToString
                currentSkill.TSL = tmpLevel
            End If
            ' Calculate the levels
            For a As Integer = 0 To 5
                currentSkill.LevelUp(a) = CInt(EveHQ.Core.SkillFunctions.CalculateSPLevel(currentSkill.Rank, a))
            Next
            ' Add the currentskill to the name list
            EveHQ.Core.HQ.SkillListName.Add(currentSkill, currentSkill.Name)
        Next
        ' All is Ok!
        Return True
    End Function              'LoadEveSkillData
    Public Shared Sub LoadEveSkillDataFromAPI()
        Try

            ' Determine if we use the APIRS or CCP API Server
            Dim APIServer As String = ""
            If EveHQ.Core.HQ.EveHQSettings.UseAPIRS = True Then
                APIServer = EveHQ.Core.HQ.EveHQSettings.APIRSAddress
                ' Check for APIRS heartbeart
                If EveHQ.Core.EveAPI.APIRSHasHeartbeat() = False Then
                    If EveHQ.Core.HQ.EveHQSettings.UseCCPAPIBackup = True Then
                        APIServer = EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress
                    End If
                End If
            Else
                APIServer = EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress
            End If

            Dim RemoteURL As String = APIServer & "/eve/SkillTree.xml." & EveHQ.Core.HQ.EveHQSettings.APIFileExtension
            ' Create the requester
            Dim request As HttpWebRequest = CType(WebRequest.Create(RemoteURL), HttpWebRequest)
            ' Setup proxy server (if required)
            If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
                Dim EveHQProxy As New WebProxy(EveHQ.Core.HQ.EveHQSettings.ProxyServer)
                If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
                    EveHQProxy.UseDefaultCredentials = True
                Else
                    EveHQProxy.UseDefaultCredentials = False
                    EveHQProxy.Credentials = New System.Net.NetworkCredential(EveHQ.Core.HQ.EveHQSettings.ProxyUsername, EveHQ.Core.HQ.EveHQSettings.ProxyPassword)
                End If
                request.Proxy = EveHQProxy
            End If
            ' Setup request parameters
            request.ContentType = "application/x-www-form-urlencoded"
            request.Headers.Set(HttpRequestHeader.AcceptEncoding, "identity")
            ' Prepare for a response from the server
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            ' Get the stream associated with the response.
            Dim receiveStream As Stream = response.GetResponseStream()
            ' Pipes the stream to a higher level stream reader with the required encoding format. 
            Dim readStream As New StreamReader(receiveStream, Encoding.UTF8)
            Dim webdata As String = readStream.ReadToEnd()
            'MessageBox.Show(webdata)
            Dim skillXML As New XmlDocument
            skillXML.LoadXml(webdata)

            ' Load the skills!
            Dim skillDetails As XmlNodeList
            Dim skill As XmlNode
            skillDetails = skillXML.SelectNodes("/eveapi/result/rowset/row/rowset/row")
            For Each skill In skillDetails
                If EveHQ.Core.HQ.SkillListID.Contains(skill.Attributes.GetNamedItem("typeID").Value) = False Then
                    Dim newSkill As New EveHQ.Core.SkillList
                    newSkill.ID = skill.Attributes.GetNamedItem("typeID").Value
                    newSkill.Name = skill.Attributes.GetNamedItem("typeName").Value
                    newSkill.GroupID = skill.Attributes.GetNamedItem("groupID").Value
                    newSkill.Description = skill.ChildNodes(0).InnerText
                    newSkill.Published = True
                    newSkill.Rank = CInt(skill.ChildNodes(1).InnerText)
                    If skill.ChildNodes(2).ChildNodes.Count <> 0 Then
                        Select Case skill.ChildNodes(2).ChildNodes.Count
                            Case 1
                                newSkill.PS = skill.ChildNodes(2).ChildNodes(0).Attributes.GetNamedItem("typeID").Value
                                newSkill.PSL = CInt(skill.ChildNodes(2).ChildNodes(0).Attributes.GetNamedItem("skillLevel").Value)
                            Case 2
                                newSkill.PS = skill.ChildNodes(2).ChildNodes(0).Attributes.GetNamedItem("typeID").Value
                                newSkill.PSL = CInt(skill.ChildNodes(2).ChildNodes(0).Attributes.GetNamedItem("skillLevel").Value)
                                newSkill.SS = skill.ChildNodes(2).ChildNodes(1).Attributes.GetNamedItem("typeID").Value
                                newSkill.SSL = CInt(skill.ChildNodes(2).ChildNodes(1).Attributes.GetNamedItem("skillLevel").Value)
                            Case 3
                                newSkill.PS = skill.ChildNodes(2).ChildNodes(0).Attributes.GetNamedItem("typeID").Value
                                newSkill.PSL = CInt(skill.ChildNodes(2).ChildNodes(0).Attributes.GetNamedItem("skillLevel").Value)
                                newSkill.SS = skill.ChildNodes(2).ChildNodes(1).Attributes.GetNamedItem("typeID").Value
                                newSkill.SSL = CInt(skill.ChildNodes(2).ChildNodes(1).Attributes.GetNamedItem("skillLevel").Value)
                                newSkill.TS = skill.ChildNodes(2).ChildNodes(2).Attributes.GetNamedItem("typeID").Value
                                newSkill.TSL = CInt(skill.ChildNodes(2).ChildNodes(2).Attributes.GetNamedItem("skillLevel").Value)
                        End Select
                    End If
                    newSkill.PA = StrConv(skill.ChildNodes(3).ChildNodes(0).InnerText, VbStrConv.ProperCase)
                    newSkill.SA = StrConv(skill.ChildNodes(3).ChildNodes(1).InnerText, VbStrConv.ProperCase)
                    ' Calculate the levels
                    For a As Integer = 0 To 5
                        newSkill.LevelUp(a) = CInt(EveHQ.Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, a))
                    Next
                    ' Add the currentskill to the name list
                    EveHQ.Core.HQ.SkillListID.Add(newSkill, newSkill.ID)
                    EveHQ.Core.HQ.SkillListName.Add(newSkill, newSkill.Name)
                End If
            Next
        Catch e As Exception
            Exit Sub
        End Try
    End Sub
    Public Shared Function CalculateSPRate(ByVal cPilot As EveHQ.Core.Pilot, ByVal cSkill As EveHQ.Core.SkillList, Optional ByVal modifiers() As Integer = Nothing) As Integer
        Dim PA, SA As Double
        Dim Rate As Integer

        ' Work out attribute modifiers
        Dim attModifiers(5) As Integer
        For count As Integer = 0 To 5
            attModifiers(count) = 0
        Next
        If IsNothing(modifiers) = False Then
            For count As Integer = 0 To 5
                attModifiers(count) = modifiers(count)
            Next
        End If

        ' Calc learning skill modifier
        Dim learningSkill As Integer = 0
        learningSkill = CInt(CDbl(cPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Learning)) + attModifiers(5))

        ' Calculate the primary attribute
        Select Case cSkill.PA.Substring(0, 1)
            Case "C"
                PA = (cPilot.CAttT - cPilot.LSCAtt + attModifiers(0)) * (1 + (0.02 * learningSkill))
            Case "I"
                PA = (cPilot.IAttT - cPilot.LSIAtt + attModifiers(1)) * (1 + (0.02 * learningSkill))
            Case "M"
                PA = (cPilot.MAttT - cPilot.LSMAtt + attModifiers(2)) * (1 + (0.02 * learningSkill))
            Case "P"
                PA = (cPilot.PAttT - cPilot.LSPAtt + attModifiers(3)) * (1 + (0.02 * learningSkill))
            Case "W"
                PA = (cPilot.WAttT - cPilot.LSWAtt + attModifiers(4)) * (1 + (0.02 * learningSkill))
        End Select

        ' Calculate the secondary attribute
        Select Case cSkill.SA.Substring(0, 1)
            Case "C"
                SA = (cPilot.CAttT - cPilot.LSCAtt + attModifiers(0)) * (1 + (0.02 * learningSkill))
            Case "I"
                SA = (cPilot.IAttT - cPilot.LSIAtt + attModifiers(1)) * (1 + (0.02 * learningSkill))
            Case "M"
                SA = (cPilot.MAttT - cPilot.LSMAtt + attModifiers(2)) * (1 + (0.02 * learningSkill))
            Case "P"
                SA = (cPilot.PAttT - cPilot.LSPAtt + attModifiers(3)) * (1 + (0.02 * learningSkill))
            Case "W"
                SA = (cPilot.WAttT - cPilot.LSWAtt + attModifiers(4)) * (1 + (0.02 * learningSkill))
        End Select

        Rate = CInt((60 * PA) + (30 * SA))
        Return Rate

    End Function          'CalculateSPRate
    Public Shared Function CalculateSP(ByVal cPilot As EveHQ.Core.Pilot, ByVal cSkill As EveHQ.Core.SkillList, ByVal toLevel As Integer, Optional ByVal fromLevel As Integer = -1) As Long

        Dim Rank, StartSP, EndSP As Double
        Dim SP As Long

        ' Get the skill rank
        Rank = cSkill.Rank

        ' Get starting skillpoints & level
        Dim startLevel As Integer = 0
        If fromLevel = -1 Then
            If cPilot.PilotSkills.Contains(cSkill.Name) = True Then
                Dim sSkill As EveHQ.Core.Skills = CType(cPilot.PilotSkills(cSkill.Name), Skills)
                startLevel = sSkill.Level
                StartSP = sSkill.SP
                If cPilot.Training = True And cPilot.TrainingSkillName = cSkill.Name Then
                    StartSP += cPilot.TrainingCurrentSP
                End If
            Else
                startLevel = 0
                StartSP = 0
            End If
        Else
            startLevel = fromLevel
            StartSP = EveHQ.Core.SkillFunctions.CalculateSPLevel(CInt(Rank), startLevel)
            If StartSP < 0 Then MsgBox("eek!!")
        End If

        ' Calculate the SPs @ end of level required
        EndSP = EveHQ.Core.SkillFunctions.CalculateSPLevel(CInt(Rank), toLevel)

        SP = CLng(EndSP - StartSP)
        Return SP

    End Function              'CalculateSP
    Public Shared Function CalcTimeToLevel(ByVal cPilot As EveHQ.Core.Pilot, ByVal cSkill As EveHQ.Core.SkillList, ByVal toLevel As Integer, Optional ByVal fromLevel As Integer = -1, Optional ByVal modifiers() As Integer = Nothing) As Double

        'NB Level = 0 indicates to train to the next available level

        Dim PA, SA As Double
        Dim Rank, StartSP, EndSP As Double
        Dim sTime As Long

        ' Work out attribute modifiers
        Dim attModifiers(5) As Integer
        For count As Integer = 0 To 5
            attModifiers(count) = 0
        Next
        If IsNothing(modifiers) = False Then
            For count As Integer = 0 To 5
                attModifiers(count) = modifiers(count)
            Next
        End If

        ' Decide whether we need to check for the mulitple learning skill issue!
        Dim multiSkill As Boolean = False
        Select Case cSkill.Name
            Case "Analytical Mind", "Clarity", "Eidetic Memory", "Empathy", "Focus", "Instant Recall", "Iron Will", "Learning", "Logic", "Presence", "Spatial Awareness"
                multiSkill = True
        End Select

        ' Get the skill rank
        Rank = cSkill.Rank

        ' Get starting skillpoints & level
        Dim startLevel As Integer = 0
        If fromLevel = -1 Then
            If cPilot.PilotSkills.Contains(cSkill.Name) = True Then
                Dim sSkill As EveHQ.Core.Skills = CType(cPilot.PilotSkills(cSkill.Name), Skills)
                startLevel = sSkill.Level
                StartSP = sSkill.SP
                If cPilot.Training = True And cPilot.TrainingSkillName = cSkill.Name Then
                    StartSP += cPilot.TrainingCurrentSP
                End If
            Else
                startLevel = 0
                StartSP = 0
            End If
        Else
            startLevel = fromLevel
            StartSP = EveHQ.Core.SkillFunctions.CalculateSPLevel(CInt(Rank), startLevel)
            If StartSP < 0 Then MsgBox("eek!!")
        End If

        ' Calculate the end of level required
        Dim sLevel As Integer = 0
        If toLevel = 0 Then
            Do
                sLevel += 1
            Loop Until EveHQ.Core.SkillFunctions.CalculateSPLevel(CInt(Rank), sLevel) > StartSP
            If sLevel > 5 Then sLevel = 5
            EndSP = EveHQ.Core.SkillFunctions.CalculateSPLevel(CInt(Rank), sLevel)
        Else
            sLevel = toLevel
        End If

        Dim learningSkill As Integer = 0
        For curLevel As Integer = startLevel + 1 To sLevel
            EndSP = EveHQ.Core.SkillFunctions.CalculateSPLevel(CInt(Rank), curLevel)
            learningSkill = CInt(CDbl(cPilot.KeySkills(EveHQ.Core.Pilot.KeySkill.Learning)) + attModifiers(5))

            ' Calculate the primary attribute
            Select Case cSkill.PA.Substring(0, 1)
                Case "C"
                    PA = (cPilot.CAttT - cPilot.LSCAtt + attModifiers(0)) * (1 + (0.02 * learningSkill))
                Case "I"
                    PA = (cPilot.IAttT - cPilot.LSIAtt + attModifiers(1)) * (1 + (0.02 * learningSkill))
                Case "M"
                    PA = (cPilot.MAttT - cPilot.LSMAtt + attModifiers(2)) * (1 + (0.02 * learningSkill))
                Case "P"
                    PA = (cPilot.PAttT - cPilot.LSPAtt + attModifiers(3)) * (1 + (0.02 * learningSkill))
                Case "W"
                    PA = (cPilot.WAttT - cPilot.LSWAtt + attModifiers(4)) * (1 + (0.02 * learningSkill))
            End Select

            ' Calculate the secondary attribute
            Select Case cSkill.SA.Substring(0, 1)
                Case "C"
                    SA = (cPilot.CAttT - cPilot.LSCAtt + attModifiers(0)) * (1 + (0.02 * learningSkill))
                Case "I"
                    SA = (cPilot.IAttT - cPilot.LSIAtt + attModifiers(1)) * (1 + (0.02 * learningSkill))
                Case "M"
                    SA = (cPilot.MAttT - cPilot.LSMAtt + attModifiers(2)) * (1 + (0.02 * learningSkill))
                Case "P"
                    SA = (cPilot.PAttT - cPilot.LSPAtt + attModifiers(3)) * (1 + (0.02 * learningSkill))
                Case "W"
                    SA = (cPilot.WAttT - cPilot.LSWAtt + attModifiers(4)) * (1 + (0.02 * learningSkill))
            End Select

            If PA = 0 Or SA = 0 Then
                sTime = 0
            Else
                sTime = CLng(sTime + Int((EndSP - StartSP) / (PA + (SA / 2)) * 60))
            End If

            StartSP = EndSP
            Select Case cSkill.Name
                Case "Analytical Mind"      ' I
                    attModifiers(1) += 1
                Case "Clarity"              ' P
                    attModifiers(3) += 1
                Case "Eidetic Memory"       ' M
                    attModifiers(2) += 1
                Case "Empathy"              ' C
                    attModifiers(0) += 1
                Case "Focus"                ' W
                    attModifiers(4) += 1
                Case "Instant Recall"       ' M
                    attModifiers(2) += 1
                Case "Iron Will"            ' W
                    attModifiers(4) += 1
                Case "Learning"             ' L
                    attModifiers(5) += 1
                Case "Logic"                ' I
                    attModifiers(1) += 1
                Case "Presence"             ' C
                    attModifiers(0) += 1
                Case "Spatial Awareness"    ' P
                    attModifiers(3) += 1
            End Select

        Next

        Return sTime

    End Function          'CalcTimeToLevel
    Public Shared Function SkillNameToID(ByVal Name As String) As String
        Dim ID As String = ""
        If EveHQ.Core.HQ.SkillListName.Contains(Name) = True Then
            Dim cSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListName(Name), SkillList)
            ID = cSkill.ID
        Else
            ID = ""
        End If
        Return ID
    End Function            'SkillNameToID
    Public Shared Function SkillIDToName(ByVal ID As String) As String
        Dim Name As String = ""
        If EveHQ.Core.HQ.SkillListID.Contains(ID) = True Then
            Dim cSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(ID), SkillList)
            Name = cSkill.Name
        Else
            Name = ""
        End If
        Return Name
    End Function            'SkillIDToName  
    Public Shared Function CalcNextLevel(ByVal cPilot As EveHQ.Core.Pilot, ByVal cSkill As EveHQ.Core.SkillList) As Integer

        Dim cLevel As Integer = 0

        ' Get current skill level
        If cPilot.PilotSkills.Contains(cSkill.Name) = True Then
            Dim sSkill As EveHQ.Core.Skills = CType(cPilot.PilotSkills(cSkill.Name), Skills)
            cLevel = sSkill.Level + 1
            If cLevel = 6 Then cLevel = 0
        Else
            cLevel = 1
        End If

        Return cLevel

    End Function            'CalcTimeToLevel
    Public Shared Function CalcCurrentLevel(ByVal cpilot As EveHQ.Core.Pilot, ByVal cskill As EveHQ.Core.SkillList) As Integer
        Dim cLevel As Integer = 0

        ' Get current skill level
        If cpilot.PilotSkills.Contains(cskill.Name) = True Then
            Dim sSkill As EveHQ.Core.Skills = CType(cpilot.PilotSkills(cskill.Name), Skills)
            cLevel = sSkill.Level
        Else
            cLevel = 0
        End If

        Return cLevel
    End Function
    Public Shared Function CalcLevelFromSP(ByVal skillID As String, ByVal SP As Integer) As Integer
        Dim cSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(skillID), SkillList)
        Dim curLevel As Integer = 0
        For level As Integer = 0 To 5
            If SP >= cSkill.LevelUp(level) Then
                curLevel = level
            End If
        Next
        Return curLevel
    End Function
    Public Shared Function HavePreReqs(ByVal qpilot As EveHQ.Core.Pilot, ByVal skillID As String) As Boolean
        ' Sub to ensure we have all the prerequisite skills we require
        ' Skills are added if required

        ' Work out if Skill pre-requisites are needed and add them to the queue
        Dim myPreReqs As String = EveHQ.Core.SkillQueueFunctions.GetSkillReqs(qpilot, skillID)
        Dim preReqs() As String = myPreReqs.Split(ControlChars.Cr)
        Dim preReq As String
        For Each preReq In preReqs
            If preReq.Length <> 0 Then
                Dim pilotLevel As String = preReq.Substring(preReq.Length - 1, 1)
                Dim reqLevel As String = preReq.Substring(preReq.Length - 2, 1)
                Dim reqSkill As String = preReq.Substring(0, preReq.Length - 2)
                If pilotLevel <> "Y" Then
                    Return False
                    Exit Function
                End If
            End If
        Next
        Return True
    End Function
    Public Shared Function ForceSkillTraining(ByVal skillPilot As EveHQ.Core.Pilot, ByVal skillID As String, ByVal Silent As Boolean) As Boolean
        Dim Trainable As Boolean = EveHQ.Core.SkillFunctions.HavePreReqs(skillPilot, skillID)
        Dim strMsg As String = ""
        Dim nextLevel As Integer = 0
        Dim setupSkill As Boolean = False
        If Trainable = True Then
            Dim skillName As String = EveHQ.Core.SkillFunctions.SkillIDToName(skillID)
            If skillPilot.PilotSkills.Contains(skillName) = True Then
                Dim mySkill As EveHQ.Core.Skills = CType(skillPilot.PilotSkills(skillName), Skills)
                ' Check if we are already have it to Level 5
                Dim maxxedOut As Boolean = False
                Dim timedOut As Boolean = False
                If mySkill.Level = 5 Then
                    maxxedOut = True
                End If
                ' Check if the selected skill is currently training
                If skillPilot.Training = True And skillPilot.TrainingSkillID = skillID Then
                    If skillPilot.TrainingCurrentTime > 0 Then
                        strMsg &= "You already have " & skillName & " training to Level " & skillPilot.TrainingSkillLevel & "!" & ControlChars.CrLf
                        MessageBox.Show(strMsg, "Force Skill Training", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                        Exit Function
                    Else
                        If skillPilot.TrainingSkillLevel = 5 Then
                            maxxedOut = True
                        Else
                            timedOut = True
                            nextLevel = skillPilot.TrainingSkillLevel + 1
                            setupSkill = False
                            strMsg &= "Are you sure you want to force skill training of " & skillName & " to Level " & nextLevel & "?"
                        End If
                    End If
                End If

                If maxxedOut = True Then
                    strMsg &= "You already have " & skillName & " trained to Level 5!" & ControlChars.CrLf
                    MessageBox.Show(strMsg, "Force Skill Training", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                    Exit Function
                Else
                    If timedOut = False Then
                        nextLevel = mySkill.Level + 1
                        setupSkill = False
                        strMsg &= "Are you sure you want to force skill training of " & skillName & " to Level " & nextLevel & "?"
                    End If
                End If
                strMsg &= "" & ControlChars.CrLf

            Else
                nextLevel = 1
                setupSkill = True
                strMsg &= "Are you sure you want to force skill training of " & skillName & " to Level 1?"
            End If

            Dim confirmforce As Integer = Windows.Forms.DialogResult.Yes
            If Silent = False Then
                confirmforce = MessageBox.Show(strMsg, "Force Skill Training?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If
            If confirmforce = Windows.Forms.DialogResult.No Then
                Exit Function
            Else
                Dim newSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(skillID), SkillList)
                ' Set the skill up if it's not already trained
                If setupSkill = True Then
                    Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
                    mySkill.ID = newSkill.ID
                    mySkill.GroupID = newSkill.GroupID
                    mySkill.Name = newSkill.Name
                    mySkill.Rank = newSkill.Rank
                    mySkill.Flag = 61
                    mySkill.Level = 0
                    mySkill.SP = 0
                    mySkill.LevelUp(0) = newSkill.LevelUp(0)
                    mySkill.LevelUp(1) = newSkill.LevelUp(1)
                    mySkill.LevelUp(2) = newSkill.LevelUp(2)
                    mySkill.LevelUp(3) = newSkill.LevelUp(3)
                    mySkill.LevelUp(4) = newSkill.LevelUp(4)
                    mySkill.LevelUp(5) = newSkill.LevelUp(5)
                    skillPilot.PilotSkills.Add(mySkill, mySkill.Name)
                End If
                ' Finish off training of the old skill
                If skillPilot.Training = True Then
                    Dim oldSkill As EveHQ.Core.Skills = CType(skillPilot.PilotSkills(skillPilot.TrainingSkillName), Skills)
                    oldSkill.Flag = 7
                    oldSkill.SP += skillPilot.TrainingCurrentSP
                    If skillPilot.TrainingCurrentTime <= 0 And oldSkill.Level <> skillPilot.TrainingSkillLevel Then
                        If oldSkill.Level < 5 Then
                            oldSkill.Level += 1
                        End If
                    End If
                End If
                ' Start training of the new skill
                Dim trainSkill As EveHQ.Core.Skills = CType(skillPilot.PilotSkills(skillName), Skills)
                trainSkill.Flag = 61
                With skillPilot
                    .Training = True
                    .TrainingSkillName = skillName
                    .TrainingSkillID = skillID
                    .TrainingSkillLevel = nextLevel
                    .TrainingStartSP = trainSkill.SP
                    .TrainingCurrentSP = 0
                    .TrainingEndSP = CInt(EveHQ.Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, nextLevel))
                    .TrainingStartTimeActual = EveHQ.Core.SkillFunctions.ConvertLocalTimeToEve(Now).AddSeconds(-EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                    .TrainingStartTime = .TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                    .TrainingEndTimeActual = .TrainingStartTimeActual.AddSeconds(EveHQ.Core.SkillFunctions.CalcTimeToLevel(skillPilot, newSkill, nextLevel))
                    .TrainingEndTime = .TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
                    .TrainingCurrentTime = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(skillPilot)
                End With
                ' Write the new XML Files
                Dim strXML As String = ""
                Dim sw As IO.StreamWriter

                ' Write Character XML
                strXML = ""
                strXML &= EveHQ.Core.Reports.CurrentPilotXML_New(skillPilot)
                sw = New IO.StreamWriter(EveHQ.Core.HQ.cacheFolder & "\c" & skillPilot.ID & ".xml")
                sw.Write(strXML)
                sw.Flush()
                sw.Close()

                ' Write Training XML
                strXML = ""
                strXML &= EveHQ.Core.Reports.CurrentTrainingXML_New(skillPilot)
                sw = New IO.StreamWriter(EveHQ.Core.HQ.cacheFolder & "\t" & skillPilot.ID & ".xml")
                sw.Write(strXML)
                sw.Flush()
                sw.Close()
                Call EveHQ.Core.PilotParseFunctions.LoadPilotCachedInfo()
                Return True
            End If
        Else
            If Silent = False Then
                strMsg = "You do not have the required pre-requisites to start training this skill!" & ControlChars.CrLf & ControlChars.CrLf & "Please ensure you have the pre-requisite skills trained before trying to force skill training."
                MessageBox.Show(strMsg, "Force Skill Training", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return False
            End If
    End Function
    Public Shared Function IsSkillTrained(ByVal cPilot As EveHQ.Core.Pilot, ByVal skillName As String, Optional ByVal toLevel As Integer = 0) As Boolean
        If EveHQ.Core.HQ.myPilot.PilotSkills.Contains(skillName) Then
            Dim mySkill As EveHQ.Core.Skills = CType(EveHQ.Core.HQ.myPilot.PilotSkills(skillName), Skills)
            Dim myLevel As Integer = CInt(mySkill.Level)
            If myLevel >= toLevel Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
    Public Shared Function TimeBeforeCanTrain(ByVal rPilot As EveHQ.Core.Pilot, ByVal parentSkillID As String) As Double

        Dim parentSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(parentSkillID), SkillList)
        Dim ItemUsable As Boolean = True
        Dim itemSkillID As String = ""
        Dim itemSkillLevel As Integer
        Dim skillsNeeded As New ArrayList

        Dim skillsRequired As Boolean = False

        For skillNo As Integer = 1 To 3
            Select Case skillNo
                Case 1
                    itemSkillID = parentSkill.PS
                    itemSkillLevel = parentSkill.PSL
                Case 2
                    itemSkillID = parentSkill.SS
                    itemSkillLevel = parentSkill.SSL
                Case 3
                    itemSkillID = parentSkill.TS
                    itemSkillLevel = parentSkill.TSL
            End Select
            If itemSkillLevel <> 0 Then
                If itemSkillID <> "" Then
                    skillsRequired = True
                    Dim skillID As String = itemSkillID

                    Dim level As Integer = 1
                    Dim pointer(20) As Integer
                    Dim parent(20) As Integer
                    Dim skillName(20) As String
                    Dim skillLevel(20) As String
                    pointer(level) = 1
                    parent(level) = CInt(skillID)

                    Dim strTree As String = ""
                    Dim cSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListID(skillID), SkillList)
                    Dim curSkill As Integer = CInt(skillID)
                    Dim curLevel As Integer = itemSkillLevel
                    Dim counter As Integer = 0
                    Dim curNode As TreeNode = New TreeNode

                    ' Write the skill we are querying as the first (parent) node
                    curNode.Text = cSkill.Name & " (Level " & curLevel & ")"
                    Dim skillTrained As Boolean = False
                    Dim myLevel As Integer = 0
                    skillTrained = False
                    If EveHQ.Core.HQ.Pilots.Count > 0 And rPilot.Updated = True Then
                        If rPilot.PilotSkills.Contains(cSkill.Name) Then
                            Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
                            mySkill = CType(rPilot.PilotSkills(cSkill.Name), Skills)
                            myLevel = CInt(mySkill.Level)
                            If myLevel >= curLevel Then skillTrained = True
                            If skillTrained = False Then
                                skillsNeeded.Add(cSkill.Name & curLevel)
                                ItemUsable = False
                            End If
                        Else
                            skillsNeeded.Add(cSkill.Name & curLevel)
                            ItemUsable = False
                        End If
                    End If

                    Do Until level = 0
                        ' Start @ root!
                        cSkill = CType(EveHQ.Core.HQ.SkillListID(CStr(curSkill)), SkillList)

                        ' Read pointer @ level
                        Select Case pointer(level)
                            Case 1
                                If CDbl(cSkill.PS) = curSkill Then Exit Do
                                pointer(level) = 2
                                curSkill = CInt(cSkill.PS)
                                curLevel = cSkill.PSL
                            Case 2
                                If CDbl(cSkill.SS) = curSkill Then Exit Do
                                pointer(level) = 3
                                curSkill = CInt(cSkill.SS)
                                curLevel = cSkill.SSL
                            Case 3
                                If CDbl(cSkill.TS) = curSkill Then Exit Do
                                pointer(level) = 4
                                curSkill = CInt(cSkill.TS)
                                curLevel = cSkill.TSL
                            Case 4
                                curSkill = 0
                        End Select
                        If curSkill = 0 Then
                            level -= 1
                            curSkill = parent(level)
                            curNode = curNode.Parent
                        Else
                            level += 1
                            parent(level) = curSkill
                            pointer(level) = 1
                            Dim newSkill As EveHQ.Core.SkillList = New EveHQ.Core.SkillList
                            newSkill = CType(EveHQ.Core.HQ.SkillListID(CStr(curSkill)), SkillList)
                            skillName(level) = newSkill.Name
                            skillLevel(level) = CStr(curLevel)
                            Dim newNode As TreeNode = New TreeNode
                            counter += 1
                            newNode.Name = CStr(counter)
                            newNode.Text = newSkill.Name & " (Level " & curLevel & ")"
                            ' Check if the current pilot has the skill
                            If EveHQ.Core.HQ.Pilots.Count > 0 And rPilot.Updated = True Then
                                skillTrained = False
                                myLevel = 0
                                If rPilot.PilotSkills.Contains(newSkill.Name) Then
                                    Dim mySkill As EveHQ.Core.Skills = New EveHQ.Core.Skills
                                    mySkill = CType(rPilot.PilotSkills(newSkill.Name), Skills)
                                    myLevel = CInt(mySkill.Level)
                                    If myLevel >= curLevel Then skillTrained = True
                                End If
                                If skillTrained = False Then
                                    skillsNeeded.Add(newSkill.Name & curLevel)
                                    ItemUsable = False
                                End If
                            End If
                            curNode.Nodes.Add(newNode)
                            curNode = newNode
                        End If
                    Loop
                End If
            End If
        Next

        If skillsRequired = True Then
            If rPilot.Name <> "" Then
                If ItemUsable = True Then
                    Return 0
                Else
                    Dim usableTime As Long = 0
                    Dim skillNo As Integer = 0
                    If skillsNeeded.Count > 1 Then
                        Do
                            Dim skill As String = CStr(skillsNeeded(skillNo))
                            Dim skillName As String = skill.Substring(0, skill.Length - 1)
                            Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                            Dim highestLevel As Integer = 0
                            Dim skillno2 As Integer = skillNo + 1
                            Do
                                If skillno2 < skillsNeeded.Count Then
                                    Dim skill2 As String = CStr(skillsNeeded(skillno2))
                                    Dim skillName2 As String = skill2.Substring(0, skill2.Length - 1)
                                    Dim skillLvl2 As Integer = CInt(skill2.Substring(skill2.Length - 1, 1))
                                    If skillName = skillName2 Then
                                        If skillLvl >= skillLvl2 Then
                                            skillsNeeded.RemoveAt(skillno2)
                                        Else
                                            skillsNeeded.RemoveAt(skillNo)
                                            skillNo = -1 : skillno2 = 0
                                            Exit Do
                                        End If
                                    Else
                                        skillno2 += 1
                                    End If
                                End If
                            Loop Until skillno2 >= skillsNeeded.Count
                            skillNo += 1
                        Loop Until skillNo >= skillsNeeded.Count - 1
                    End If
                    skillsNeeded.Reverse()
                    For Each skill As String In skillsNeeded
                        Dim skillName As String = skill.Substring(0, skill.Length - 1)
                        Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                        Dim cSkill As EveHQ.Core.SkillList = CType(EveHQ.Core.HQ.SkillListName(skillName), SkillList)
                        usableTime = CLng(usableTime + EveHQ.Core.SkillFunctions.CalcTimeToLevel(rPilot, cSkill, skillLvl))
                    Next
                    Return usableTime
                End If
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function

End Class

<Serializable()> Public Class Skills
    Implements System.ICloneable
    Public ID As String
    Public Name As String
    Public GroupID As String
    Public Flag As Integer
    Public Rank As Integer
    Public SP As Integer
    Public Level As Integer
    Public LevelUp(5) As Integer
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As Skills = CType(Me.MemberwiseClone, Skills)
        Return R
    End Function
End Class

<Serializable()> Public Class SkillList
    Implements System.ICloneable
    Public ID As String
    Public Name As String
    Public Description As String
    Public GroupID As String
    Public Published As Boolean
    Public Rank As Integer
    Public SP As Integer
    Public Level As Integer
    Public LevelUp(5) As Integer
    Public PA As String
    Public SA As String
    Public PS As String
    Public PSL As Integer
    Public SS As String
    Public SSL As Integer
    Public TS As String
    Public TSL As Integer
    Public BasePrice As Double
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As SkillList = CType(Me.MemberwiseClone, SkillList)
        Return R
    End Function
End Class

<Serializable()> Public Class SkillGroup
    Public ID As String
    Public Name As String
End Class

