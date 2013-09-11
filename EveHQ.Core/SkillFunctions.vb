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
Imports System.Windows.Forms
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Xml

Public Class SkillFunctions

    Shared eveData As Data.DataSet

    ' Shared variables for repeated usage
    Shared sTimeSpan, eTimeSpan, tTimeSpan As TimeSpan

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
    Public Shared Function CalculateSPLevel(ByVal rank As Integer, ByVal level As Integer) As Double
        Dim SP As Double = 0
        If Level > 0 Then
            SP = 250 * rank * (2 ^ (2.5 * (Level - 1)))
        End If
        Return SP
    End Function         'CalculateSPLevel
    Public Shared Function CalculateSkillSPLevel(ByVal skillID As String, ByVal level As Integer) As Double
        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
        Return CalculateSPLevel(cSkill.Rank, Level)
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
    Public Shared Function ConvertEveTimeToLocal(ByVal eveTime As Date) As Date
        ' Calculate the local time and UTC offset.
        Return TimeZone.CurrentTimeZone.ToLocalTime(EveTime)
    End Function    'ConvertEveTimeToLocal
    Public Shared Function ConvertLocalTimeToEve(ByVal localTime As Date) As Date
        ' Calculate the local time and UTC offset.
        Return TimeZone.CurrentTimeZone.ToUniversalTime(LocalTime)
    End Function
    Public Shared Function CalcCurrentSkillTime(ByRef myPilot As EveHQ.Core.EveHQPilot) As Long
        If myPilot.Training = True Then
            eTimeSpan = EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(myPilot.TrainingEndTime) - Now
            myPilot.TrainingCurrentTime = CLng(Math.Max(eTimeSpan.TotalSeconds, 0))
            Return myPilot.TrainingCurrentTime
        Else
            Return 0
        End If
    End Function     'CalcCurrentSkillTime
    Public Shared Function CalcCurrentSkillPoints(ByRef myPilot As EveHQ.Core.EveHQPilot) As Long
        If myPilot.Training = True Then
            tTimeSpan = myPilot.TrainingEndTime - myPilot.TrainingStartTime
            If DateTime.Compare(Now, EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(myPilot.TrainingEndTime)) < 0 Then
                sTimeSpan = Now - EveHQ.Core.SkillFunctions.ConvertEveTimeToLocal(myPilot.TrainingStartTime)
            Else
                sTimeSpan = tTimeSpan
            End If
            myPilot.TrainingCurrentSP = CInt(sTimeSpan.TotalSeconds / tTimeSpan.TotalSeconds * (myPilot.TrainingEndSP - myPilot.TrainingStartSP))
            Return myPilot.TrainingCurrentSP
        Else
            Return 0
        End If
    End Function   'CalcCurrentSkillPoints
    Public Shared Function CalcProductionTime(ByRef myPilot As EveHQ.Core.EveHQPilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.04 * CDbl(myPilot.KeySkills(EveHQ.Core.EveHQPilot.KeySkill.Industry))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcResearchProdTime(ByRef myPilot As EveHQ.Core.EveHQPilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.EveHQPilot.KeySkill.Research))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcResearchMatTime(ByRef myPilot As EveHQ.Core.EveHQPilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.EveHQPilot.KeySkill.Metallurgy))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcResearchCopyTime(ByRef myPilot As EveHQ.Core.EveHQPilot, ByVal time As Double) As Double
        Dim newTime As Double = 0
        Try
            newTime = time * (1 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.EveHQPilot.KeySkill.Science))))
        Catch ex As Exception
            newTime = 0
        End Try
        Return newTime
    End Function
    Public Shared Function CalcWasteFactor(ByRef myPilot As EveHQ.Core.EveHQPilot, ByVal wf As Double) As Double
        Dim newWF As Double = 0
        Try
            newWF = ((1 + wf) * (1.25 - (0.05 * CDbl(myPilot.KeySkills(EveHQ.Core.EveHQPilot.KeySkill.ProductionEfficiency))))) - 1
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
        strSQL &= "SELECT * FROM invGroups WHERE (invGroups.categoryID=16 AND invGroups.groupID<>267) ORDER BY groupName;"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData Is Nothing Then
            MessageBox.Show("The Database returned a null dataset when trying to load the Eve skill data. The error returned was: " & EveHQ.Core.HQ.dataError, "Load Eve Skill Data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
            Exit Function
        Else
            If eveData.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("The Database returned a no rows when trying to load the Eve skill data." & EveHQ.Core.HQ.dataError, "Load Eve Skill Data error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            EveHQ.Core.HQ.SkillGroups.Add(newSkillGroup.Name, newSkillGroup)
        Next

        ' Get details of skill data from database
        strSQL = ""
        strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName,  invTypes.basePrice, invTypes.published, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
        strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID=invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID=invTypes.groupID) INNER JOIN dgmTypeAttributes ON invTypes.typeID=dgmTypeAttributes.typeID"
        strSQL &= " WHERE (invCategories.categoryID=16 AND invGroups.groupID<>267) ORDER BY typeName;"

        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

        ' Add data to the skillitem lists
        Dim attValue As Double = 0
        For row As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            ' Check if the typeID already exists
            Dim newSkill As EveHQ.Core.EveSkill
            If EveHQ.Core.HQ.SkillListID.ContainsKey(eveData.Tables(0).Rows(row).Item("typeID").ToString) = False Then
                newSkill = New EveHQ.Core.EveSkill
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
                EveHQ.Core.HQ.SkillListID.Add(newSkill.ID, newSkill)
            Else
                newSkill = EveHQ.Core.HQ.SkillListID(eveData.Tables(0).Rows(row).Item("typeID").ToString)
            End If
        Next

        Dim MaxPreReqs As Integer = 10
        For Each newSkill As EveHQ.Core.EveSkill In EveHQ.Core.HQ.SkillListID.Values
            Dim PreReqSkills(MaxPreReqs) As String
            Dim PreReqSkillLevels(MaxPreReqs) As Integer
            Dim attRows() As DataRow = eveData.Tables(0).Select("typeID=" & newSkill.ID)
            For Each attRow As DataRow In attRows
                If IsDBNull(attRow.Item("valueInt")) = False Then
                    attValue = CDbl(attRow.Item("valueInt"))
                Else
                    attValue = CDbl(attRow.Item("valueFloat"))
                End If
                Select Case CInt(attRow.Item("attributeID"))
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
                    Case 275
                        newSkill.Rank = CInt(attValue)
                    Case 182
                        PreReqSkills(1) = CStr(attValue)
                    Case 183
                        PreReqSkills(2) = CStr(attValue)
                    Case 184
                        PreReqSkills(3) = CStr(attValue)
                    Case 1285
                        PreReqSkills(4) = CStr(attValue)
                    Case 1289
                        PreReqSkills(5) = CStr(attValue)
                    Case 1290
                        PreReqSkills(6) = CStr(attValue)
                    Case 277
                        PreReqSkillLevels(1) = CInt(attValue)
                    Case 278
                        PreReqSkillLevels(2) = CInt(attValue)
                    Case 279
                        PreReqSkillLevels(3) = CInt(attValue)
                    Case 1286
                        PreReqSkillLevels(4) = CInt(attValue)
                    Case 1287
                        PreReqSkillLevels(5) = CInt(attValue)
                    Case 1288
                        PreReqSkillLevels(6) = CInt(attValue)
                End Select
            Next
            ' Add the pre-reqs into the list
            For prereq As Integer = 1 To MaxPreReqs
                If PreReqSkills(prereq) <> "" And PreReqSkills(prereq) <> "0" Then
                    newSkill.PreReqSkills.Add(PreReqSkills(prereq), PreReqSkillLevels(prereq))
                End If
            Next
            ' Calculate the levels
            For a As Integer = 0 To 5
                newSkill.LevelUp(a) = CInt(Math.Ceiling(EveHQ.Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, a)))
            Next
            ' Add the currentskill to the name list
            EveHQ.Core.HQ.SkillListName.Add(newSkill.Name, newSkill)
        Next
        
        ' All is Ok!
        Return True
    End Function              'LoadEveSkillData
    Public Shared Sub LoadEveSkillDataFromAPI()
        Try
            ' Get the XML data from the API
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.Settings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            Dim skillXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.SkillTree, EveAPI.APIReturnMethods.BypassCache)
            ' Load the skills!
            If skillXML IsNot Nothing Then
                Dim skillDetails As XmlNodeList
                Dim skill As XmlNode
                skillDetails = skillXML.SelectNodes("/eveapi/result/rowset/row/rowset/row")
                For Each skill In skillDetails
                    If EveHQ.Core.HQ.SkillListID.ContainsKey(skill.Attributes.GetNamedItem("typeID").Value) = False Then
                        Dim newSkill As New EveHQ.Core.EveSkill
                        newSkill.ID = skill.Attributes.GetNamedItem("typeID").Value
                        newSkill.Name = skill.Attributes.GetNamedItem("typeName").Value
                        newSkill.GroupID = skill.Attributes.GetNamedItem("groupID").Value
                        newSkill.Published = CBool(skill.Attributes.GetNamedItem("published").Value)
                        newSkill.Description = skill.ChildNodes(0).InnerText
                        newSkill.Rank = CInt(skill.ChildNodes(1).InnerText)
                        If skill.ChildNodes(2).ChildNodes.Count <> 0 Then
                            For skillNode As Integer = 0 To skill.ChildNodes(2).ChildNodes.Count - 1
                                newSkill.PreReqSkills.Add(skill.ChildNodes(2).ChildNodes(skillNode).Attributes.GetNamedItem("typeID").Value, CInt(skill.ChildNodes(2).ChildNodes(skillNode).Attributes.GetNamedItem("skillLevel").Value))
                            Next
                        End If
                        newSkill.PA = StrConv(skill.ChildNodes(3).SelectSingleNode("primaryAttribute").InnerText, VbStrConv.ProperCase)
                        newSkill.SA = StrConv(skill.ChildNodes(3).SelectSingleNode("secondaryAttribute").InnerText, VbStrConv.ProperCase)
                        ' Calculate the levels
                        For a As Integer = 0 To 5
                            newSkill.LevelUp(a) = CInt(EveHQ.Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, a))
                        Next
                        ' Add the currentskill to the name list
                        EveHQ.Core.HQ.SkillListID.Add(newSkill.ID, newSkill)
                        EveHQ.Core.HQ.SkillListName.Add(newSkill.Name, newSkill)
                    Else
                        Dim newSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skill.Attributes.GetNamedItem("typeID").Value)
                        If skill.Attributes.GetNamedItem("typeName").Value <> newSkill.Name Then
                            ' Need to update the skill details because CCP changed them!
                            newSkill.ID = skill.Attributes.GetNamedItem("typeID").Value
                            newSkill.Name = skill.Attributes.GetNamedItem("typeName").Value
                            newSkill.GroupID = skill.Attributes.GetNamedItem("groupID").Value
                            newSkill.Published = CBool(skill.Attributes.GetNamedItem("published").Value)
                            newSkill.Description = skill.ChildNodes(0).InnerText
                            newSkill.Published = True
                            newSkill.Rank = CInt(skill.ChildNodes(1).InnerText)
                            If skill.ChildNodes(2).ChildNodes.Count <> 0 Then
                                For skillNode As Integer = 0 To skill.ChildNodes(2).ChildNodes.Count - 1
                                    newSkill.PreReqSkills.Add(skill.ChildNodes(2).ChildNodes(skillNode).Attributes.GetNamedItem("typeID").Value, CInt(skill.ChildNodes(2).ChildNodes(skillNode).Attributes.GetNamedItem("skillLevel").Value))
                                Next
                            End If
                            newSkill.PA = StrConv(skill.ChildNodes(3).SelectSingleNode("primaryAttribute").InnerText, VbStrConv.ProperCase)
                            newSkill.SA = StrConv(skill.ChildNodes(3).SelectSingleNode("secondaryAttribute").InnerText, VbStrConv.ProperCase)
                            ' Calculate the levels
                            For a As Integer = 0 To 5
                                newSkill.LevelUp(a) = CInt(EveHQ.Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, a))
                            Next
                        End If
                    End If
                Next
            End If
        Catch e As Exception
            Exit Sub
        End Try
    End Sub
    Public Shared Function CalculateSPRate(ByVal cPilot As EveHQ.Core.EveHQPilot, ByVal cSkill As EveHQ.Core.EveSkill) As Integer
        Dim PA, SA As Double
        Dim Rate As Integer

        ' Calculate the primary attribute
        Select Case cSkill.PA.Substring(0, 1)
            Case "C"
                PA = cPilot.CAttT
            Case "I"
                PA = cPilot.IAttT
            Case "M"
                PA = cPilot.MAttT
            Case "P"
                PA = cPilot.PAttT
            Case "W"
                PA = cPilot.WAttT
        End Select

        ' Calculate the secondary attribute
        Select Case cSkill.SA.Substring(0, 1)
            Case "C"
                SA = cPilot.CAttT
            Case "I"
                SA = cPilot.IAttT
            Case "M"
                SA = cPilot.MAttT
            Case "P"
                SA = cPilot.PAttT
            Case "W"
                SA = cPilot.WAttT
        End Select

        Rate = CInt(((60 * PA) + (30 * SA)))
        Return Rate

    End Function          'CalculateSPRate
    Public Shared Function CalculateSP(ByVal cPilot As EveHQ.Core.EveHQPilot, ByVal cSkill As EveHQ.Core.EveSkill, ByVal toLevel As Integer, Optional ByVal fromLevel As Integer = -1) As Long

        Dim Rank, StartSP, EndSP As Double
        Dim SP As Long

        ' Get the skill rank
        Rank = cSkill.Rank

        ' Get starting skillpoints & level
        Dim startLevel As Integer = 0
        If fromLevel = -1 Then
            If cPilot.PilotSkills.ContainsKey(cSkill.Name) = True Then
                Dim sSkill As EveHQ.Core.EveHQPilotSkill = cPilot.PilotSkills(cSkill.Name)
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
    Public Shared Function CalcTimeToLevel(ByVal cPilot As EveHQ.Core.EveHQPilot, ByVal cSkill As EveHQ.Core.EveSkill, ByVal toLevel As Integer, Optional ByVal fromLevel As Integer = -1) As Double

        'NB Level = 0 indicates to train to the next available level

        Dim PA, SA As Double
        Dim Rank, StartSP, EndSP As Double
        Dim sTime As Long

        ' Get the skill rank
        Rank = cSkill.Rank

        ' Get starting skillpoints & level
        Dim startLevel As Integer = 0
        If fromLevel = -1 Then
            If cPilot.PilotSkills.ContainsKey(cSkill.Name) = True Then
                Dim sSkill As EveHQ.Core.EveHQPilotSkill = cPilot.PilotSkills(cSkill.Name)
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

        For curLevel As Integer = startLevel + 1 To sLevel
            EndSP = EveHQ.Core.SkillFunctions.CalculateSPLevel(CInt(Rank), curLevel)

            ' Calculate the primary attribute
            Select Case cSkill.PA.Substring(0, 1)
                Case "C"
                    PA = cPilot.CAttT
                Case "I"
                    PA = cPilot.IAttT
                Case "M"
                    PA = cPilot.MAttT
                Case "P"
                    PA = cPilot.PAttT
                Case "W"
                    PA = cPilot.WAttT
            End Select

            ' Calculate the secondary attribute
            Select Case cSkill.SA.Substring(0, 1)
                Case "C"
                    SA = cPilot.CAttT
                Case "I"
                    SA = cPilot.IAttT
                Case "M"
                    SA = cPilot.MAttT
                Case "P"
                    SA = cPilot.PAttT
                Case "W"
                    SA = cPilot.WAttT
            End Select

            If PA = 0 Or SA = 0 Then
                sTime = 0
            Else
                sTime = CLng(sTime + Int((EndSP - StartSP) / (PA + (SA / 2)) * 60))
            End If

            StartSP = EndSP
        Next

        Return sTime

    End Function          'CalcTimeToLevel
    Public Shared Function SkillNameToID(ByVal name As String) As String
        Dim ID As String = ""
        If EveHQ.Core.HQ.SkillListName.ContainsKey(Name) = True Then
            Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(Name)
            ID = cSkill.ID
        Else
            ID = ""
        End If
        Return ID
    End Function            'SkillNameToID
    Public Shared Function SkillIDToName(ByVal id As String) As String
        Dim Name As String = ""
        If EveHQ.Core.HQ.SkillListID.ContainsKey(ID) = True Then
            Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(ID)
            Name = cSkill.Name
        Else
            Name = ""
        End If
        Return Name
    End Function            'SkillIDToName  
    Public Shared Function CalcNextLevel(ByVal cPilot As EveHQ.Core.EveHQPilot, ByVal cSkill As EveHQ.Core.EveSkill) As Integer

        Dim cLevel As Integer = 0

        ' Get current skill level
        If cPilot.PilotSkills.ContainsKey(cSkill.Name) = True Then
            Dim sSkill As EveHQ.Core.EveHQPilotSkill = cPilot.PilotSkills(cSkill.Name)
            cLevel = sSkill.Level + 1
            If cLevel = 6 Then cLevel = 0
        Else
            cLevel = 1
        End If

        Return cLevel

    End Function            'CalcTimeToLevel
    Public Shared Function CalcCurrentLevel(ByVal cpilot As EveHQ.Core.EveHQPilot, ByVal cskill As EveHQ.Core.EveSkill) As Integer
        Dim cLevel As Integer = 0

        ' Get current skill level
        If cpilot.PilotSkills.ContainsKey(cskill.Name) = True Then
            Dim sSkill As EveHQ.Core.EveHQPilotSkill = cpilot.PilotSkills(cskill.Name)
            cLevel = sSkill.Level
        Else
            cLevel = 0
        End If

        Return cLevel
    End Function
    Public Shared Function CalcLevelFromSP(ByVal skillID As String, ByVal SP As Integer) As Integer
        Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
        Dim curLevel As Integer = 0
        For level As Integer = 0 To 5
            If SP >= cSkill.LevelUp(level) Then
                curLevel = level
            End If
        Next
        Return curLevel
    End Function
    Public Shared Function HavePreReqs(ByVal qpilot As EveHQ.Core.EveHQPilot, ByVal skillID As String) As Boolean
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
    Public Shared Function ForceSkillTraining(ByVal skillPilot As EveHQ.Core.EveHQPilot, ByVal skillID As String, ByVal silent As Boolean) As Boolean
        Dim trainable As Boolean = EveHQ.Core.SkillFunctions.HavePreReqs(skillPilot, skillID)
        Dim strMsg As String = ""
        Dim nextLevel As Integer = 0
        Dim setupSkill As Boolean = False
        If trainable = True Then
            Dim skillName As String = EveHQ.Core.SkillFunctions.SkillIDToName(skillID)
            If skillPilot.PilotSkills.ContainsKey(skillName) = True Then
                Dim mySkill As EveHQ.Core.EveHQPilotSkill = skillPilot.PilotSkills(skillName)
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
            If silent = False Then
                confirmforce = MessageBox.Show(strMsg, "Force Skill Training?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If
            If confirmforce = Windows.Forms.DialogResult.No Then
                Exit Function
            Else
                Dim newSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(skillID)
                ' Set the skill up if it's not already trained
                If setupSkill = True Then
                    Dim mySkill As New EveHQ.Core.EveHQPilotSkill
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
                    skillPilot.PilotSkills.Add(mySkill.Name, mySkill)
                End If
                ' Finish off training of the old skill
                If skillPilot.Training = True Then
                    Dim oldSkill As EveHQ.Core.EveHQPilotSkill = skillPilot.PilotSkills(skillPilot.TrainingSkillName)
                    oldSkill.Flag = 7
                    oldSkill.SP += skillPilot.TrainingCurrentSP
                    If skillPilot.TrainingCurrentTime <= 0 And oldSkill.Level <> skillPilot.TrainingSkillLevel Then
                        If oldSkill.Level < 5 Then
                            oldSkill.Level += 1
                        End If
                    End If
                End If
                ' Start training of the new skill
                Dim trainSkill As EveHQ.Core.EveHQPilotSkill = skillPilot.PilotSkills(skillName)
                trainSkill.Flag = 61
                With skillPilot
                    .Training = True
                    .TrainingSkillName = skillName
                    .TrainingSkillID = skillID
                    .TrainingSkillLevel = nextLevel
                    .TrainingStartSP = trainSkill.SP
                    .TrainingCurrentSP = 0
                    .TrainingEndSP = CInt(EveHQ.Core.SkillFunctions.CalculateSPLevel(newSkill.Rank, nextLevel))
                    .TrainingStartTimeActual = EveHQ.Core.SkillFunctions.ConvertLocalTimeToEve(Now).AddSeconds(-EveHQ.Core.HQ.Settings.ServerOffset)
                    .TrainingStartTime = .TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.Settings.ServerOffset)
                    .TrainingEndTimeActual = .TrainingStartTimeActual.AddSeconds(EveHQ.Core.SkillFunctions.CalcTimeToLevel(skillPilot, newSkill, nextLevel))
                    .TrainingEndTime = .TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.Settings.ServerOffset)
                    .TrainingCurrentTime = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(skillPilot)
                End With
                ' Write the new XML Files
                Dim strXML As String = ""
                Dim sw As IO.StreamWriter

                ' Write Character XML
                strXML = ""
                strXML &= EveHQ.Core.Reports.CurrentPilotXML_New(skillPilot)
                sw = New IO.StreamWriter(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.CharacterSheet.ToString & "_" & skillPilot.Account & "_" & skillPilot.ID & ".xml"))
                sw.Write(strXML)
                sw.Flush()
                sw.Close()

                ' Write Training XML
                strXML = ""
                strXML &= EveHQ.Core.Reports.CurrentTrainingXML_New(skillPilot)
                sw = New IO.StreamWriter(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveAPI.APITypes.SkillQueue.ToString & "_" & skillPilot.Account & "_" & skillPilot.ID & ".xml"))
                sw.Write(strXML)
                sw.Flush()
                sw.Close()
                Call EveHQ.Core.PilotParseFunctions.LoadPilotCachedInfo()
                Return True
            End If
        Else
            If silent = False Then
                strMsg = "You do not have the required pre-requisites to start training this skill!" & ControlChars.CrLf & ControlChars.CrLf & "Please ensure you have the pre-requisite skills trained before trying to force skill training."
                MessageBox.Show(strMsg, "Force Skill Training", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return False
        End If
    End Function
    Public Shared Function IsSkillTrained(ByVal cPilot As EveHQ.Core.EveHQPilot, ByVal skillName As String, Optional ByVal toLevel As Integer = 0) As Boolean
        If cPilot.PilotSkills.ContainsKey(skillName) Then
            Dim mySkill As EveHQ.Core.EveHQPilotSkill = cPilot.PilotSkills(skillName)
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

    Public Shared Function TimeBeforeCanTrain(ByVal rPilot As EveHQ.Core.EveHQPilot, ByVal parentSkillID As String, ToSkillLevel As Integer) As Double
        Dim UsableTime As Double = 0
        Dim FromSkillLevel As Integer = -1
        If rPilot.PilotSkills.ContainsKey(parentSkillID) = False Then
            UsableTime = TimeBeforeCanTrain(rPilot, parentSkillID)
        Else
            FromSkillLevel = rPilot.PilotSkills(parentSkillID).Level
        End If
        UsableTime += CalcTimeToLevel(rPilot, EveHQ.Core.HQ.SkillListID(parentSkillID), ToSkillLevel, FromSkillLevel)
        Return UsableTime
    End Function

    Public Shared Function TimeBeforeCanTrain(ByVal rPilot As EveHQ.Core.EveHQPilot, ByVal parentSkillID As String) As Double

        Dim parentSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(parentSkillID)
        Dim itemSkillLevel As Integer = 0
        Dim skillsNeeded As New ArrayList
        Call PreReqTimeBeforeCanTrain(rPilot, parentSkill, 0, skillsNeeded, True)

        If skillsNeeded.Count > 0 Then
            If rPilot.Name <> "" Then
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
                    skillsNeeded.Reverse()
                End If
                For Each skill As String In skillsNeeded
                    Dim skillName As String = skill.Substring(0, skill.Length - 1)
                    Dim skillLvl As Integer = CInt(skill.Substring(skill.Length - 1, 1))
                    Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(skillName)
                    usableTime = CLng(usableTime + EveHQ.Core.SkillFunctions.CalcTimeToLevel(rPilot, cSkill, skillLvl))
                Next
                Return usableTime
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function
    Private Shared Sub PreReqTimeBeforeCanTrain(ByVal rPilot As EveHQ.Core.EveHQPilot, ByVal cSkill As EveHQ.Core.EveSkill, ByVal curLevel As Integer, ByRef skillsNeeded As ArrayList, ByVal rootSkill As Boolean)
        ' Write the skill we are querying as the first (parent) node
        Dim skillTrained As Boolean = False
        Dim myLevel As Integer = 0
        skillTrained = False
        If EveHQ.Core.HQ.Settings.Pilots.Count > 0 And rPilot.Updated = True Then
            If rPilot.PilotSkills.ContainsKey(cSkill.Name) = True Then
                Dim mySkill As New EveHQ.Core.EveHQPilotSkill
                mySkill = rPilot.PilotSkills(cSkill.Name)
                myLevel = CInt(mySkill.Level)
                If myLevel >= curLevel Then skillTrained = True
                If skillTrained = False Then
                    skillsNeeded.Add(cSkill.Name & curLevel)
                End If
            Else
                If rootSkill = False Then
                    skillsNeeded.Add(cSkill.Name & curLevel)
                End If
            End If
        End If
        If cSkill.PreReqSkills.Count > 0 Then
            Dim subSkill As EveHQ.Core.EveSkill
            For Each subSkillID As String In cSkill.PreReqSkills.Keys
                subSkill = EveHQ.Core.HQ.SkillListID(subSkillID)
                If subSkill.ID <> cSkill.ID Then
                    Call PreReqTimeBeforeCanTrain(rPilot, subSkill, cSkill.PreReqSkills(subSkillID), skillsNeeded, False)
                End If
            Next
        End If
    End Sub

End Class

<Serializable()> Public Class EveSkill
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
    Public PreReqSkills As New Dictionary(Of String, Integer)
    Public BasePrice As Double
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim R As EveSkill = CType(Me.MemberwiseClone, EveSkill)
        Return R
    End Function
End Class

<Serializable()> Public Class SkillGroup
    Public ID As String
    Public Name As String
End Class

