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
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Xml
Imports System.Windows.Forms

Public Class Reports
    Shared www As String = "http://www.evehq.net"

#Region "Common Routines"

    Shared eveData As DataSet

    Public Shared Function HTMLCharacterDetails(ByVal rpilot As EveHQ.Core.Pilot) As String

        Dim strHTML As String = ""
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = "0"
        Dim currentTime As String = "0"
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(rpilot))
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(rpilot))
        End If
        strHTML &= "<table width=800px align=center border=0>"
        strHTML &= "<tr><td width=150px align=center valign=middle><img src='http://img.eve.is/serv.asp?s=256&c=" & rpilot.ID & "' height=128 width=128 alt='" & rpilot.Name & "'></td>"
        strHTML &= "<td width=5px></td>"
        strHTML &= "<td width=350px>"
        strHTML &= "<table width=100%><tr><td class=thead align=center valign=middle colspan=2><b>Character Info</b></td></tr>"
        strHTML &= "<tr><td>Character Name</td><td>" & rpilot.Name & "</td></tr>"
        strHTML &= "<tr><td>Corporation</td><td>" & rpilot.Corp & "</td></tr>"
        strHTML &= "<tr><td>Total Cash</td><td>" & Format(CDbl(rpilot.Isk), "Standard") & "</td></tr>"
        strHTML &= "<tr><td>Total Skillpoints</td><td>" & FormatNumber(CLng(rpilot.SkillPoints) + CLng(currentSP), 0, , , ) & " (in " & rpilot.PilotSkills.Count & " skills)</td></tr>"
        strHTML &= "<tr><td>Currently Training</td><td>"
        If rpilot.Training = True Then
            strHTML &= currentSkill.Name & " (Level " & rpilot.TrainingSkillLevel & ")</td></tr>"
            strHTML &= "<tr><td></td><td>" & currentTime & " remaining</td></tr>"
        Else
            strHTML &= "Nothing</td></tr>"
            strHTML &= "<tr><td></td><td>&nbsp;</td></tr>"
        End If
        strHTML &= "</table></td>"
        strHTML &= "<td width=5px></td>"
        strHTML &= "<td width=200px>"
        strHTML &= "<table width=100%><tr><td class=thead align=center valign=middle colspan=2><b>Attributes</b></td></tr>"
        strHTML &= "<tr><td>Charisma</td><td>" & Format(CDbl(rpilot.CAttT), "##.00") & "</td></tr>"
        strHTML &= "<tr><td>Intelligence</td><td>" & Format(CDbl(rpilot.IAttT), "##.00") & "</td></tr>"
        strHTML &= "<tr><td>Memory</td><td>" & Format(CDbl(rpilot.MAttT), "##.00") & "</td></tr>"
        strHTML &= "<tr><td>Perception</td><td>" & Format(CDbl(rpilot.PAttT), "##.00") & "</td></tr>"
        strHTML &= "<tr><td>Willpower</td><td>" & Format(CDbl(rpilot.WAttT), "##.00") & "</td></tr>"
        strHTML &= "<tr><td></td><td>&nbsp;</td></tr>"
        strHTML &= "</table></td>"
        strHTML &= "</tr>"
        strHTML &= "</table>"
        strHTML &= "<p></p>"
        Return strHTML

    End Function

    Public Shared Function HTMLHeader(ByVal browserHeader As String) As String
        Dim strHTML As String = "<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""http://www.w3.org/TR/html4/strict.dtd"">"
        strHTML &= "<html lang=""" & System.Globalization.CultureInfo.CurrentCulture.ToString & """>"
        strHTML &= "<head>"
        strHTML &= "<META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">"
        strHTML &= "<title>" & browserHeader & "</title>" & CharacterCSS() & "</head>"
        strHTML &= "<body>"
        Return strHTML
    End Function

    Public Shared Function HTMLTitle(ByVal Title As String) As String
        Dim strHTML As String = ""
        strHTML &= "<table width=800px border=0 align=center>"
        strHTML &= "<tr height=30px><td><p class=title>" & Title & "</p></td></tr>"
        strHTML &= "</table>"
        strHTML &= "<p></p>"
        Return strHTML
    End Function

    Public Shared Function HTMLFooter() As String
        Dim strHTML As String = ""
        strHTML &= "<table width=800px align=center border=0><hr>"
        strHTML &= "<tr><td><p align=center class=footer>Generated on " & Format(Now, "dd/MM/yyyy HH:mm:ss") & " by <a href='" & www & "'>" & My.Application.Info.ProductName & "</a> v" & My.Application.Info.Version.ToString & "</p></td></tr>"
        strHTML &= "</table>"
        strHTML &= "</body></html>"
        Return strHTML
    End Function

    Private Shared Function CharacterCSS() As String
        Dim strCSS As String = ""
        strCSS &= "<STYLE><!--"
        strCSS &= "BODY { font-family: Tahoma, Arial; font-size: 12px; bgcolor: #000000; background: #000000 }"
        strCSS &= "TD, P { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff }"
        strCSS &= ".thead { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff; font-variant: small-caps; background-color: #444444 }"
        strCSS &= ".footer { font-family: Tahoma, Arial; font-size: 9px; color: #ffffff; font-variant: small-caps }"
        strCSS &= ".title { font-family: Tahoma, Arial; font-size: 20px; color: #ffffff; font-variant: small-caps }"
        strCSS &= "--></STYLE>"
        Return strCSS
    End Function

    Private Shared Function NonCharHTMLHeader(ByVal pageTitle As String, ByVal pageHeader As String) As String
        Dim strHTML As String = ""
        strHTML &= "<html>"
        strHTML &= "<head><title>" & pageTitle & "</title>" & CharacterCSS() & "</head>"
        strHTML &= "<body>"
        strHTML &= "<table width=800px border=0 align=center>"
        strHTML &= "<tr height=30px><td><p class=title>" & pageTitle & "</p></td></tr>"
        strHTML &= "</table>"
        strHTML &= "<p></p>"

        Return strHTML

    End Function

    Public Shared Function TextCharacterDetails(ByVal reportTitle As String, ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder
        Dim tmpText As String = ""
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = "0"
        Dim currentTime As String = "0"
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(rpilot))
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(rpilot))
        End If

        tmpText = reportTitle & " - " & rpilot.Name
        strText.AppendLine(tmpText)
        strText.AppendLine(New String(CChar("="), tmpText.Length))
        strText.AppendLine()
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Name:", rpilot.Name))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Race:", rpilot.Race))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Bloodline:", rpilot.Blood))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Corporation:", rpilot.Corp))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Isk:", FormatNumber(rpilot.Isk, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Skillpoints:", FormatNumber(CLng(rpilot.SkillPoints) + CLng(currentSP), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Skills:", rpilot.PilotSkills.Count))
        strText.AppendLine()
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Charisma:", Format(CDbl(rpilot.CAttT), "##.00")))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Intelligence:", Format(CDbl(rpilot.IAttT), "##.00")))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Memory:", Format(CDbl(rpilot.MAttT), "##.00")))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Perception:", Format(CDbl(rpilot.PAttT), "##.00")))
        strText.AppendLine(String.Format("{0,16} {1,-18}", "Willpower:", Format(CDbl(rpilot.WAttT), "##.00")))
        strText.AppendLine()
        strText.AppendLine(New String(CChar("-"), 50))
        strText.AppendLine()

        Return strText.ToString
    End Function

    Public Shared Function XMLImplantDetails(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strXML As String = ""
        Dim tabs(10) As String
        For tab As Integer = 1 To 10
            For tab2 As Integer = 1 To tab
                tabs(tab) &= Chr(9)
            Next
        Next
        Dim Implants(4, 1) As String
        Implants(0, 0) = "memory" : Implants(0, 1) = "Memory Augmentation"
        Implants(1, 0) = "willpower" : Implants(1, 1) = "Neural Boost"
        Implants(2, 0) = "perception" : Implants(2, 1) = "Ocular Filter"
        Implants(3, 0) = "intelligence" : Implants(3, 1) = "Cybernetic Subprocessor"
        Implants(4, 0) = "charisma" : Implants(4, 1) = "Social Adaptation Chip"
        Dim ImplantStyles(6, 1) As String
        ImplantStyles(0, 0) = "Limited " : ImplantStyles(0, 1) = ""
        ImplantStyles(1, 0) = "Limited " : ImplantStyles(1, 1) = " - Beta"
        ImplantStyles(2, 0) = "" : ImplantStyles(2, 1) = " - Basic"
        ImplantStyles(3, 0) = "" : ImplantStyles(3, 1) = " - Standard"
        ImplantStyles(4, 0) = "" : ImplantStyles(4, 1) = " - Improved"
        ImplantStyles(5, 0) = "" : ImplantStyles(5, 1) = " - Advanced"
        ImplantStyles(6, 0) = "" : ImplantStyles(6, 1) = " - Elite"
        strXML &= tabs(2) & "<attributeEnhancers>" & vbCrLf
        If rpilot.MImplantA > 0 Then
            strXML &= AddImplantToXML(rpilot.MImplantA, Implants(0, 0), Implants(0, 1), ImplantStyles(rpilot.MImplantA - 1, 0), ImplantStyles(rpilot.MImplantA - 1, 1))
        End If
        If rpilot.WImplantA > 0 Then
            strXML &= AddImplantToXML(rpilot.WImplantA, Implants(1, 0), Implants(1, 1), ImplantStyles(rpilot.WImplantA - 1, 0), ImplantStyles(rpilot.WImplantA - 1, 1))
        End If
        If rpilot.PImplantA > 0 Then
            strXML &= AddImplantToXML(rpilot.PImplantA, Implants(2, 0), Implants(2, 1), ImplantStyles(rpilot.PImplantA - 1, 0), ImplantStyles(rpilot.PImplantA - 1, 1))
        End If
        If rpilot.IImplantA > 0 Then
            strXML &= AddImplantToXML(rpilot.IImplantA, Implants(3, 0), Implants(3, 1), ImplantStyles(rpilot.IImplantA - 1, 0), ImplantStyles(rpilot.IImplantA - 1, 1))
        End If
        If rpilot.CImplantA > 0 Then
            strXML &= AddImplantToXML(rpilot.CImplantA, Implants(4, 0), Implants(4, 1), ImplantStyles(rpilot.CImplantA - 1, 0), ImplantStyles(rpilot.CImplantA - 1, 1))
        End If
        strXML &= tabs(2) & "</attributeEnhancers>" & vbCrLf
        Return strXML
    End Function

    Private Shared Function AddImplantToXML(ByVal level As Integer, ByVal implant As String, ByVal implantName As String, ByVal impPre As String, ByVal impPost As String) As String
        Dim strXML As String = ""
        strXML &= New String(Chr(9), 3) & "<" & implant & "Bonus>" & vbCrLf
        strXML &= New String(Chr(9), 4) & "<augmentatorName>" & impPre & implantName & impPost & "</augmentatorName>" & vbCrLf
        strXML &= New String(Chr(9), 4) & "<augmentatorValue>" & level.ToString & "</augmentatorValue>" & vbCrLf
        strXML &= New String(Chr(9), 3) & "</" & implant & "Bonus>" & vbCrLf
        Return strXML
    End Function

#End Region

#Region "Character Sheet Report"

    Public Shared Sub GenerateCharSheet(ByVal rpilot As EveHQ.Core.Pilot)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Character Sheet - " & rpilot.Name)
        strHTML &= HTMLTitle("Character Sheet - " & rpilot.Name)
        strHTML &= HTMLCharacterDetails(rpilot)
        strHTML &= CharacterSheet(rpilot)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharSheet (" & rpilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function CharacterSheet(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim repGroup(EveHQ.Core.HQ.SkillGroups.Count, 3) As String
        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 5) As String
        Dim cGroup As EveHQ.Core.SkillGroup = New EveHQ.Core.SkillGroup
        Dim cSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim groupCount As Integer = 0
        For Each cGroup In EveHQ.Core.HQ.SkillGroups.Values
            groupCount += 1
            repGroup(groupCount, 1) = cGroup.Name
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            ' Collect skills
            Dim repSkills As New SortedList(Of String, EveHQ.Core.PilotSkill)
            For Each cSkill In rpilot.PilotSkills
                repSkills.Add(cSkill.Name, cSkill)
            Next
            For Each cSkill In repSkills.Values
                If cSkill.GroupID = cGroup.ID Then
                    skillCount += 1
                    SPCount += cSkill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cSkill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cSkill.Name), 0))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                    If rpilot.Training = True Then
                        If currentSkill.ID = cSkill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
        Next

        Dim imgLevel As String = ""
        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        For group As Integer = 1 To EveHQ.Core.HQ.SkillGroups.Count
            If CDbl(repGroup(group, 2)) > 0 Then
                strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 1) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                    If rpilot.Training = True Then
                        If currentSkill.ID = repSkill(group, skill, 0) Then
                            strHTML &= "<td style='color:#FFAA00;'>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & CDbl(repSkill(group, skill, 5)) + 1 & "_act.gif"
                        Else
                            strHTML &= "<td>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                        End If
                    Else
                        strHTML &= "<td>"
                        imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                    End If
                    strHTML &= "<b>" & repSkill(group, skill, 1) & "</b>&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Rank " & repSkill(group, skill, 2) & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "SP: " & Format(CLng(repSkill(group, skill, 3)), "#,###") & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Time to Next Level: " & repSkill(group, skill, 4)
                    strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                Next
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
            End If
        Next
        strHTML &= "</table><p></p>"

        Return strHTML
    End Function

#End Region

#Region "Skill Levels Report"
    Public Shared Sub GenerateSkillLevels(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Skill Levels - " & rPilot.Name)
        strHTML &= HTMLTitle("Skill Levels - " & rPilot.Name)
        strHTML &= HTMLCharacterDetails(rPilot)
        strHTML &= SkillLevels(rPilot)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function SkillLevels(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As String
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            sortSkill(count, 0) = curSkill.ID
            sortSkill(count, 1) = curSkill.Name
            count += 1
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(rpilot.PilotSkills.Count - 1) As Integer
        For a As Integer = 0 To rpilot.PilotSkills.Count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort (use string version!!)
        Dim myComparer As New ArrayComparerString(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 6              ' Number of groups to break the report into
        Dim repGroup(nog, 5) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Level 0" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Level 1" : repGroup(2, 1) = "1" : repGroup(2, 2) = "1"
        repGroup(3, 0) = "Level 2" : repGroup(3, 1) = "2" : repGroup(3, 2) = "2"
        repGroup(4, 0) = "Level 3" : repGroup(4, 1) = "3" : repGroup(4, 2) = "3"
        repGroup(5, 0) = "Level 4" : repGroup(5, 1) = "4" : repGroup(5, 2) = "4"
        repGroup(6, 0) = "Level 5" : repGroup(6, 1) = "5" : repGroup(6, 2) = "5"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                Dim cskill As EveHQ.Core.PilotSkill
                Dim askill As EveHQ.Core.EveSkill
                askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                If cskill.Level >= CDbl(repGroup(groupCount, 1)) And cskill.Level <= CDbl(repGroup(groupCount, 2)) Then
                    skillCount += 1
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    Dim curTime As Double = EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(curTime)
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    If rpilot.Training = True Then
                        If currentSkill.ID = cskill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        Dim imgLevel As String = ""
        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        For group As Integer = 1 To nog
            If CDbl(repGroup(group, 2)) > 0 Then
                strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                    If rpilot.Training = True Then
                        If currentSkill.ID = repSkill(group, skill, 0) Then
                            strHTML &= "<td style='color:#FFAA00;'>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & CDbl(repSkill(group, skill, 5)) + 1 & "_act.gif"
                        Else
                            strHTML &= "<td>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                        End If
                    Else
                        strHTML &= "<td>"
                        imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                    End If
                    strHTML &= "<b>" & repSkill(group, skill, 1) & "</b>&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Rank " & repSkill(group, skill, 2) & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "SP: " & Format(CLng(repSkill(group, skill, 3)), "#,###") & " of " & Format(CLng(repSkill(group, skill, 6)), "#,###") & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Time to Next Level: " & repSkill(group, skill, 4)
                    strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                Next
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td align=right>Time to Clear Group: " & repGroup(group, 4) & "</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
            End If
        Next
        strHTML &= "</table>"
        strHTML &= "<p></p>"
        Return strHTML
    End Function

#End Region

#Region "Training Time Report"
    Public Shared Sub GenerateTrainingTime(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Training Times - " & rPilot.Name)
        strHTML &= HTMLTitle("Training Times - " & rPilot.Name)
        strHTML &= HTMLCharacterDetails(rPilot)
        strHTML &= TrainingTime(rPilot)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainTime (" & rPilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TrainingTime(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            ' Determine if the skill is being trained
            If rpilot.Training = True Then
                If curSkill.ID = rpilot.TrainingSkillID Then
                    sortSkill(count, 1) = rpilot.TrainingCurrentTime
                Else
                    sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
                End If
            Else
                sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
            End If
            sortSkill(count, 0) = CLng(curSkill.ID)
            count += 1
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(rpilot.PilotSkills.Count - 1) As Integer
        For a As Integer = 0 To rpilot.PilotSkills.Count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Training Completed" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 1 hour" : repGroup(2, 1) = "1" : repGroup(2, 2) = "3600"
        repGroup(3, 0) = "1 to 2 hours" : repGroup(3, 1) = "3601" : repGroup(3, 2) = "7200"
        repGroup(4, 0) = "2 to 4 hours" : repGroup(4, 1) = "7201" : repGroup(4, 2) = "14400"
        repGroup(5, 0) = "4 to 6 hours" : repGroup(5, 1) = "14401" : repGroup(5, 2) = "21600"
        repGroup(6, 0) = "6 to 8 hours" : repGroup(6, 1) = "21601" : repGroup(6, 2) = "28800"
        repGroup(7, 0) = "8 to 16 hours" : repGroup(7, 1) = "28801" : repGroup(7, 2) = "57600"
        repGroup(8, 0) = "16 to 24 hours" : repGroup(8, 1) = "57601" : repGroup(8, 2) = "86400"
        repGroup(9, 0) = "1 to 2 days" : repGroup(9, 1) = "86401" : repGroup(9, 2) = "172800"
        repGroup(10, 0) = "2 to 4 days" : repGroup(10, 1) = "172801" : repGroup(10, 2) = "345600"
        repGroup(11, 0) = "4 to 7 days" : repGroup(11, 1) = "345601" : repGroup(11, 2) = "604800"
        repGroup(12, 0) = "7 to 14 days" : repGroup(12, 1) = "604801" : repGroup(12, 2) = "1209600"
        repGroup(13, 0) = "14 to 21 days" : repGroup(13, 1) = "1209601" : repGroup(13, 2) = "1814400"
        repGroup(14, 0) = "21 to 28 days" : repGroup(14, 1) = "1814401" : repGroup(14, 2) = "2419200"
        repGroup(15, 0) = "28 days or more" : repGroup(15, 1) = "2419200" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim cskill As EveHQ.Core.PilotSkill
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                    skillCount += 1
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    If rpilot.Training = True Then
                        If currentSkill.ID = cskill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        Dim imgLevel As String = ""
        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        Dim group As Integer = 1
        Do
            group = group + 1
            If group = nog + 1 Then group = 1
            If CDbl(repGroup(group, 2)) > 0 Then
                strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                    If rpilot.Training = True Then
                        If currentSkill.ID = repSkill(group, skill, 0) Then
                            strHTML &= "<td style='color:#FFAA00;'>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & CDbl(repSkill(group, skill, 5)) + 1 & "_act.gif"
                        Else
                            strHTML &= "<td>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                        End If
                    Else
                        strHTML &= "<td>"
                        imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                    End If
                    strHTML &= "<b>" & repSkill(group, skill, 1) & "</b>&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Rank " & repSkill(group, skill, 2) & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "SP: " & Format(CLng(repSkill(group, skill, 3)), "#,###") & " of " & Format(CLng(repSkill(group, skill, 6)), "#,###") & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Time to Next Level: " & repSkill(group, skill, 4)
                    strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                Next
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td align=right>Time to Clear Group: " & repGroup(group, 4) & "</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
            End If
        Loop Until group = 1
        strHTML &= "</table>"
        strHTML &= "<p></p>"

        Return strHTML
    End Function

#End Region

#Region "Skills Not Trained Report"
    Public Shared Sub GenerateSkillsNotTrained(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Skills Not Trained - " & rPilot.Name)
        strHTML &= HTMLTitle("Skills Not Trained - " & rPilot.Name)
        strHTML &= HTMLCharacterDetails(rPilot)
        strHTML &= SkillsNotTrained(rPilot)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing
        ' Tidy up report variables
        GC.Collect()
    End Sub

    Public Shared Function SkillsNotTrained(ByVal rPilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rPilot.Training = True Then
            currentSkill = CType(rPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rPilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rPilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rPilot.TrainingCurrentTime)
        End If

        Dim sortSkill(EveHQ.Core.HQ.SkillListID.Count, 1) As Long
        Dim curSkill As EveHQ.Core.EveSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In EveHQ.Core.HQ.SkillListID.Values
            If curSkill.Published = True Then
                If rPilot.PilotSkills.Contains(curSkill.Name) = False Then
                    ' Determine if the skill is being trained
                    sortSkill(count, 1) = CLng(EveHQ.Core.SkillFunctions.TimeBeforeCanTrain(rPilot, curSkill.ID))
                    sortSkill(count, 0) = CLng(curSkill.ID)
                    count += 1
                End If
            End If
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(count - 1) As Integer
        For a As Integer = 0 To count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Ready To Start Training" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 24 hours" : repGroup(2, 1) = "1" : repGroup(2, 2) = "86400"
        repGroup(3, 0) = "1 to 3 days" : repGroup(3, 1) = "86401" : repGroup(3, 2) = "259200"
        repGroup(4, 0) = "3 to 5 days" : repGroup(4, 1) = "259201" : repGroup(4, 2) = "432000"
        repGroup(5, 0) = "5 to 7 days" : repGroup(5, 1) = "432001" : repGroup(5, 2) = "604800"
        repGroup(6, 0) = "7 to 10 days" : repGroup(6, 1) = "604801" : repGroup(6, 2) = "864000"
        repGroup(7, 0) = "10 to 14 days" : repGroup(7, 1) = "864001" : repGroup(7, 2) = "1209600"
        repGroup(8, 0) = "14 to 21 days" : repGroup(8, 1) = "1209601" : repGroup(8, 2) = "1814400"
        repGroup(9, 0) = "21 to 30 days" : repGroup(9, 1) = "1814401" : repGroup(9, 2) = "2592000"
        repGroup(10, 0) = "30 to 45 days" : repGroup(10, 1) = "2592001" : repGroup(10, 2) = "3888000"
        repGroup(11, 0) = "45 to 60 days" : repGroup(11, 1) = "3888001" : repGroup(11, 2) = "5184000"
        repGroup(12, 0) = "60 to 75 days" : repGroup(12, 1) = "5184001" : repGroup(12, 2) = "6480000"
        repGroup(13, 0) = "75 to 90 days" : repGroup(13, 1) = "6480001" : repGroup(13, 2) = "7776000"
        repGroup(14, 0) = "90 to 120 days" : repGroup(14, 1) = "7776001" : repGroup(14, 2) = "10368000"
        repGroup(15, 0) = "120 days or more" : repGroup(15, 1) = "10368001" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    skillCount += 1
                    TotalTime += skillTimeLeft
                    SPCount += askill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = askill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = askill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(askill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = "0"
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(skillTimeLeft)
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(askill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(askill.Level + 1, 5)))

                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        Dim imgLevel As String = ""
        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        Dim group As Integer = 0
        Do
            group = group + 1
            'If group = nog + 1 Then group = 1
            If CDbl(repGroup(group, 2)) > 0 Then
                strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                    If rPilot.Training = True Then
                        If currentSkill.ID = repSkill(group, skill, 0) Then
                            strHTML &= "<td style='color:#FFAA00;'>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & CDbl(repSkill(group, skill, 5)) + 1 & "_act.gif"
                        Else
                            strHTML &= "<td>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                        End If
                    Else
                        strHTML &= "<td>"
                        imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                    End If
                    strHTML &= "<b>" & repSkill(group, skill, 1) & "</b>&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Rank " & repSkill(group, skill, 2) & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    'strHTML &= "SP: " & Format(CLng(repSkill(group, skill, 3)), "#,##0") & " of " & Format(CLng(repSkill(group, skill, 6)), "#,###") & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Time Before Training: " & repSkill(group, skill, 4)
                    strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                Next
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td align=right>Time to Clear Group: " & repGroup(group, 4) & "</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
            End If
        Loop Until group = nog
        strHTML &= "</table>"
        strHTML &= "<p></p>"

        Return strHTML
    End Function

#End Region

#Region "Time to Level 5 Report"
    Public Shared Sub GenerateTimeToLevel5(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Time To Level 5 - " & rPilot.Name)
        strHTML &= HTMLTitle("Time To Level 5 - " & rPilot.Name)
        strHTML &= HTMLCharacterDetails(rPilot)
        strHTML &= TimeToLevel5(rPilot)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TimeToLevel5(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            ' Determine if the skill is being trained
            If rpilot.Training = True Then
                sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 5) - redTime, 0))
            Else
                sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 5) - redTime, 0))
            End If
            sortSkill(count, 0) = CLng(curSkill.ID)
            count += 1
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(rpilot.PilotSkills.Count - 1) As Integer
        For a As Integer = 0 To rpilot.PilotSkills.Count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Training Completed" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 24 hours" : repGroup(2, 1) = "1" : repGroup(2, 2) = "86400"
        repGroup(3, 0) = "1 to 3 days" : repGroup(3, 1) = "86401" : repGroup(3, 2) = "259200"
        repGroup(4, 0) = "3 to 5 days" : repGroup(4, 1) = "259201" : repGroup(4, 2) = "432000"
        repGroup(5, 0) = "5 to 7 days" : repGroup(5, 1) = "432001" : repGroup(5, 2) = "604800"
        repGroup(6, 0) = "7 to 10 days" : repGroup(6, 1) = "604801" : repGroup(6, 2) = "864000"
        repGroup(7, 0) = "10 to 14 days" : repGroup(7, 1) = "864001" : repGroup(7, 2) = "1209600"
        repGroup(8, 0) = "14 to 21 days" : repGroup(8, 1) = "1209601" : repGroup(8, 2) = "1814400"
        repGroup(9, 0) = "21 to 30 days" : repGroup(9, 1) = "1814401" : repGroup(9, 2) = "2592000"
        repGroup(10, 0) = "30 to 45 days" : repGroup(10, 1) = "2592001" : repGroup(10, 2) = "3888000"
        repGroup(11, 0) = "45 to 60 days" : repGroup(11, 1) = "3888001" : repGroup(11, 2) = "5184000"
        repGroup(12, 0) = "60 to 75 days" : repGroup(12, 1) = "5184001" : repGroup(12, 2) = "6480000"
        repGroup(13, 0) = "75 to 90 days" : repGroup(13, 1) = "6480001" : repGroup(13, 2) = "7776000"
        repGroup(14, 0) = "90 to 120 days" : repGroup(14, 1) = "7776001" : repGroup(14, 2) = "10368000"
        repGroup(15, 0) = "120 days or more" : repGroup(15, 1) = "10368001" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim cskill As EveHQ.Core.PilotSkill
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                    skillCount += 1
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 5)
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 5))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        Dim imgLevel As String = ""
        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        Dim group As Integer = 1
        Do
            group = group + 1
            If group = nog + 1 Then group = 1
            If CDbl(repGroup(group, 2)) > 0 Then
                strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                    If rpilot.Training = True Then
                        If currentSkill.ID = repSkill(group, skill, 0) Then
                            strHTML &= "<td style='color:#FFAA00;'>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & CDbl(repSkill(group, skill, 5)) + 1 & "_act.gif"
                        Else
                            strHTML &= "<td>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                        End If
                    Else
                        strHTML &= "<td>"
                        imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                    End If
                    strHTML &= "<b>" & repSkill(group, skill, 1) & "</b>&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Rank " & repSkill(group, skill, 2) & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "SP: " & Format(CLng(repSkill(group, skill, 3)), "#,##0") & " of " & Format(CLng(repSkill(group, skill, 6)), "#,###") & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Time to Level V : " & repSkill(group, skill, 4)
                    strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                Next
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td align=right>Time to Clear Group: " & repGroup(group, 4) & "</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
            End If
        Loop Until group = 1
        strHTML &= "</table>"
        strHTML &= "<p></p>"

        Return strHTML
    End Function
#End Region

#Region "Pilot XML Reports"

    Public Shared Sub GenerateCharXML(ByVal rPilot As EveHQ.Core.Pilot)
        Dim cXML As New XmlDocument
        cXML.Load(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.CharacterSheet.ToString & "_" & rPilot.Account & "_" & rPilot.ID & ".xml"))
        cXML.Save(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharXML (" & rPilot.Name & ").xml"))
    End Sub

    Public Shared Sub GenerateTrainXML(ByVal rPilot As EveHQ.Core.Pilot)
        Dim tXML As New XmlDocument
        tXML.Load(Path.Combine(EveHQ.Core.HQ.cacheFolder, "EVEHQAPI_" & EveHQ.Core.EveAPI.APIRequest.SkillQueue.ToString & "_" & rPilot.Account & "_" & rPilot.ID & ".xml"))
        tXML.Save(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML (" & rPilot.Name & ").xml"))
    End Sub

#End Region

#Region "Training Queue Report"

    Public Shared Sub GenerateTrainQueue(ByVal rPilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
        strHTML &= HTMLTitle("Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
        strHTML &= HTMLCharacterDetails(rPilot)
        strHTML &= TrainQueue(rPilot, rQueue)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TrainQueue(ByVal rpilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue) As String
        Dim strHTML As String = ""
        Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(rpilot, rQueue)
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        strHTML &= "<tr>"
        strHTML &= "<td class=thead width=250px>Skill Name</td>"
        strHTML &= "<td class=thead width=60px align=center>Current Level</td>"
        strHTML &= "<td class=thead width=60px align=center>From Level</td>"
        strHTML &= "<td class=thead width=60px align=center>To Level</td>"
        strHTML &= "<td class=thead width=70px align=center>Percent Complete</td>"
        strHTML &= "<td class=thead width=150px>Time Remaining</td>"
        strHTML &= "<td class=thead width=150px>Date Ended</td>"
        strHTML &= "</tr>"

        For skill As Integer = 0 To arrQueue.Count - 1
            Dim qItem As EveHQ.Core.SortedQueue = CType(arrQueue(skill), EveHQ.Core.SortedQueue)
            Dim skillName As String = qItem.Name
            Dim curLevel As String = qItem.CurLevel
            Dim startLevel As String = qItem.FromLevel
            Dim endLevel As String = qItem.ToLevel
            Dim percent As String = qItem.Percent
            Dim timeToEnd As String = EveHQ.Core.SkillFunctions.TimeToString(CDbl(qItem.TrainTime))
            Dim endTime As String = Format(qItem.DateFinished, "ddd") & " " & FormatDateTime(qItem.DateFinished, DateFormat.GeneralDate)

            strHTML &= "<tr height=20px>"
            strHTML &= "<td>" & skillName & "</td>"
            strHTML &= "<td align=center>" & curLevel & "</td>"
            strHTML &= "<td align=center>" & startLevel & "</td>"
            strHTML &= "<td align=center>" & endLevel & "</td>"
            strHTML &= "<td align=center>" & percent & "</td>"
            strHTML &= "<td>" & timeToEnd & "</td>"
            strHTML &= "<td>" & endTime & "</td>"
            strHTML &= "</tr>"
        Next

        strHTML &= "</table><p></p>"

        Return strHTML
    End Function

#End Region

#Region "Shopping List Reports"
    Public Shared Sub GenerateShoppingList(ByVal rPilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
        strHTML &= HTMLTitle("Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
        strHTML &= HTMLCharacterDetails(rPilot)
        strHTML &= ShoppingList(rPilot, rQueue)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function ShoppingList(ByVal rpilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue) As String
        Dim strHTML As String = ""
        Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(rpilot, rQueue)
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        strHTML &= "<tr>"
        strHTML &= "<td class=thead width=350px>Skill Name</td>"
        strHTML &= "<td class=thead align=left>Market Price (est)</td>"
        strHTML &= "</tr>"

        Dim skillPriceList As New ArrayList
        For skill As Integer = 0 To arrQueue.Count - 1
            Dim qItem As EveHQ.Core.SortedQueue = CType(arrQueue(skill), SortedQueue)
            Dim skillName As String = qItem.Name
            If rpilot.PilotSkills.Contains(skillName) = False Then
                If skillPriceList.Contains(skillName) = False Then
                    skillPriceList.Add(skillName)
                End If
            End If
        Next

        Dim totalCost As Double = 0
        For Each skillName As String In skillPriceList
            Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(skillName)
            strHTML &= "<tr height=20px>"
            strHTML &= "<td width=350px>" & skillName & "</td>"
            strHTML &= "<td align=left>" & FormatNumber(cSkill.BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>"
            strHTML &= "</tr>"
            totalCost += cSkill.BasePrice
        Next

        strHTML &= "<tr height=20px><td>&nbsp;</td><td></td></tr>"
        strHTML &= "<tr height=20px><td width=350px>Total Queue Shopping List Cost:</td><td>" & FormatNumber(totalCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td></tr>"
        strHTML &= "</table><p></p>"

        Return strHTML
    End Function
#End Region

#Region "Skills Available Report"

    Public Shared Sub GenerateSkillsAvailable(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Skills Available To Train - " & rPilot.Name)
        strHTML &= HTMLTitle("Skills Available To Train - " & rPilot.Name)
        strHTML &= HTMLCharacterDetails(rPilot)
        strHTML &= SkillsAvailable(rPilot)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function SkillsAvailable(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        strHTML &= "<table width=600px align=center cellspacing=0 cellpadding=0>"
        strHTML &= "<tr>"
        strHTML &= "<td class=thead width=250px>Skill Name</td>"
        strHTML &= "<td class=thead width=250px>Skill Group</td>"
        strHTML &= "<td class=thead width=100px>Skill Rank</td>"
        strHTML &= "</tr>"

        Dim trainable As Boolean = False
        For Each skill As EveHQ.Core.EveSkill In EveHQ.Core.HQ.SkillListName.Values
            trainable = False
            If rpilot.PilotSkills.Contains(skill.Name) = False And skill.Published = True Then
                trainable = True
                For Each preReq As String In skill.PreReqSkills.Keys
                    If skill.PreReqSkills(preReq) <> 0 Then
                        Dim ps As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(preReq)
                        If rpilot.PilotSkills.Contains(ps.Name) = True Then
                            Dim psp As EveHQ.Core.PilotSkill = CType(rpilot.PilotSkills(ps.Name), EveHQ.Core.PilotSkill)
                            If psp.Level < skill.PreReqSkills(preReq) Then
                                trainable = False
                                Exit For
                            End If
                        Else
                            trainable = False
                            Exit For
                        End If
                    End If
                Next
            End If

            If trainable = True Then
                strHTML &= "<tr height=20px>"
                strHTML &= "<td>" & skill.Name & "</td>"
                strHTML &= "<td>" & EveHQ.Core.HQ.itemGroups(skill.GroupID) & "</td>"
                strHTML &= "<td>" & skill.Rank & "</td>"
                strHTML &= "</tr>"
            End If

        Next

        strHTML &= "</table><p></p>"

        Return strHTML
    End Function

#End Region

#Region "Comparers for Multi-dimensional arrays"

    Class RectangularComparer
        Implements IComparer
        ' maintain a reference to the 2-dimensional array being sorted

        Private sortArray(,) As Long

        ' constructor initializes the sortArray reference
        Public Sub New(ByVal theArray(,) As Long)
            sortArray = theArray
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            ' x and y are integer row numbers into the sortArray
            Dim i1 As Long = DirectCast(x, Integer)
            Dim i2 As Long = DirectCast(y, Integer)

            ' compare the items in the sortArray
            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
        End Function
    End Class

    Class ArrayComparerString
        Implements IComparer
        ' maintain a reference to the 2-dimensional array being sorted

        Private sortArray(,) As String

        ' constructor initializes the sortArray reference
        Public Sub New(ByVal theArray(,) As String)
            sortArray = theArray
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            ' x and y are integer row numbers into the sortArray
            Dim i1 As String = CStr(DirectCast(x, Integer))
            Dim i2 As String = CStr(DirectCast(y, Integer))

            ' compare the items in the sortArray
            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
        End Function
    End Class

    Class ArrayComparerDouble
        Implements IComparer
        ' maintain a reference to the 2-dimensional array being sorted

        Private sortArray(,) As Double

        ' constructor initializes the sortArray reference
        Public Sub New(ByVal theArray(,) As Double)
            sortArray = theArray
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            ' x and y are integer row numbers into the sortArray
            Dim i1 As Double = CDbl(DirectCast(x, Integer))
            Dim i2 As Double = CDbl(DirectCast(y, Integer))

            ' compare the items in the sortArray
            Return sortArray(CInt(i1), 1).CompareTo(sortArray(CInt(i2), 1))
        End Function
    End Class

#End Region

#Region "Alloy Reports"
    Public Shared Sub GenerateAlloyReport()
        Dim strHTML As String = ""

        strHTML &= NonCharHTMLHeader("Alloy Composition Report", "")
        strHTML &= AlloyReport(False)
        strHTML &= HTMLFooter()

        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "AlloyReport.html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Public Shared Function AlloyReport(ByVal forIGB As Boolean) As String
        Dim strHTML As String = ""

        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID, invTypes.typeName, (SELECT typeName FROM invTypes WHERE invTypeMaterials.materialTypeID = invTypes.typeID) AS Material, invTypeMaterials.materialTypeID , invTypeMaterials.quantity , invGroups.groupID, invGroups.groupName, (SELECT icon FROM eveGraphics WHERE invTypes.graphicID=eveGraphics.graphicID) AS groupIcon, (SELECT (SELECT icon FROM eveGraphics WHERE eveGraphics.graphicID=invTypes.graphicID) FROM invTypes WHERE invTypeMaterials.materialTypeID = invTypes.typeID) AS typeIcon"
        strSQL &= " FROM eveGraphics INNER JOIN (invGroups INNER JOIN (invTypes INNER JOIN invTypeMaterials ON invTypes.typeID=invTypeMaterials.typeID) ON invGroups.groupID=invTypes.groupID) ON eveGraphics.graphicID=invTypes.graphicID"
        strSQL &= " WHERE(invGroups.groupID = 355)"
        strSQL &= " ORDER BY invGroups.groupName, invTypes.typeName;"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

        Dim oreMaterials(1, 12, 8) As String
        Dim oreIcons(12, 12) As String
        Dim oreID(12, 12) As String
        Dim minIcons(8) As String
        Dim minID(8) As String

        Dim groupCount As Integer = 1
        Dim lastGroup As String = eveData.Tables(0).Rows(0).Item("groupName").ToString.Trim
        Dim typeCount As Integer = 1
        Dim lastType As String = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
        For ore As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            If eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim <> lastType Then
                typeCount += 1
                lastType = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            End If
            If eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim <> lastGroup Then
                groupCount += 1
                typeCount = 1
                lastGroup = eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim
                lastType = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            End If
            oreMaterials(groupCount, 0, 0) = eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim
            oreMaterials(groupCount, typeCount, 0) = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            oreIcons(groupCount, typeCount) = eveData.Tables(0).Rows(ore).Item("groupIcon").ToString.Trim
            oreID(groupCount, typeCount) = eveData.Tables(0).Rows(ore).Item("typeID").ToString
            Select Case eveData.Tables(0).Rows(ore).Item("Material").ToString.Trim
                Case "Tritanium"
                    oreMaterials(groupCount, typeCount, 1) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(1) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(1) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Pyerite"
                    oreMaterials(groupCount, typeCount, 2) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(2) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(2) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Mexallon"
                    oreMaterials(groupCount, typeCount, 3) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(3) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(3) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Isogen"
                    oreMaterials(groupCount, typeCount, 4) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(4) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(4) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Nocxium"
                    oreMaterials(groupCount, typeCount, 5) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(5) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(5) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Zydrine"
                    oreMaterials(groupCount, typeCount, 6) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(6) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(6) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Megacyte"
                    oreMaterials(groupCount, typeCount, 7) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(7) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(7) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Morphite"
                    oreMaterials(groupCount, typeCount, 8) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(8) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(8) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
            End Select
        Next

        strHTML &= "<table width=800px align=center>"
        strHTML &= "<tr>"
        strHTML &= "<td width=35px></td>"
        strHTML &= "<td width=165px></td>"
        For min As Integer = 1 To 8
            If forIGB = False Then
                strHTML &= "<td width=75px align=center><img src='" & EveHQ.Core.ImageHandler.GetImageLocation(minIcons(min), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
            Else
                strHTML &= "<td width=75px align=center><img src='" & EveHQ.Core.ImageHandler.GetRawImageLocation(minIcons(min), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
            End If
        Next
        strHTML &= "</tr>"
        strHTML &= "<tr>"
        strHTML &= "<td width=35px></td>"
        strHTML &= "<td width=165px></td>"
        strHTML &= "<td width=75px align=center>Tritanium</td>"
        strHTML &= "<td width=75px align=center>Pyerite</td>"
        strHTML &= "<td width=75px align=center>Mexallon</td>"
        strHTML &= "<td width=75px align=center>Isogen</td>"
        strHTML &= "<td width=75px align=center>Nocxium</td>"
        strHTML &= "<td width=75px align=center>Zydrine</td>"
        strHTML &= "<td width=75px align=center>Megacyte</td>"
        strHTML &= "<td width=75px align=center>Morphite</td>"
        strHTML &= "</tr>"

        For groupType As Integer = 1 To 1
            strHTML &= "<tr><td colspan=10 class=thead>" & oreMaterials(groupType, 0, 0) & "</td></tr>"
            For oreType As Integer = 1 To 12
                strHTML &= "<tr>"
                strHTML &= "<td width=35px>"
                If forIGB = False Then
                    strHTML &= "<img src='" & EveHQ.Core.ImageHandler.GetImageLocation(oreIcons(groupType, oreType), EveHQ.Core.ImageHandler.ImageType.Icons) & "'>"
                Else
                    strHTML &= "<img src='" & EveHQ.Core.ImageHandler.GetRawImageLocation(oreIcons(groupType, oreType), EveHQ.Core.ImageHandler.ImageType.Icons) & "'>"
                End If
                strHTML &= "</td>"
                strHTML &= "<td width=165px>" & oreMaterials(groupType, oreType, 0) & "</td>"
                For minType As Integer = 1 To 8
                    strHTML &= "<td width=75px align=center>" & oreMaterials(groupType, oreType, minType) & "</td>"
                Next
                strHTML &= "</tr>"
            Next
        Next
        strHTML &= "</table><br>"

        Return strHTML

    End Function
#End Region

#Region "Asteroid Reports"

    Public Shared Sub GenerateRockReport()
        Dim strHTML As String = ""

        strHTML &= NonCharHTMLHeader("Ore Composition Report", "")
        strHTML &= RockReport(False)
        strHTML &= HTMLFooter()

        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "OreReport.html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Public Shared Function RockReport(ByVal forIGB As Boolean) As String
        Dim strHTML As String = ""

        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID, invTypes.typeName, (SELECT typeName FROM invTypes WHERE ramTypeRequirements.requiredTypeID=invTypes.typeID) AS Material, ramTypeRequirements.requiredTypeID, ramTypeRequirements.quantity, invGroups.groupID, invGroups.groupName, (SELECT icon FROM eveGraphics WHERE invTypes.graphicID=eveGraphics.graphicID) AS groupIcon, (SELECT (SELECT icon FROM eveGraphics WHERE eveGraphics.graphicID=invTypes.graphicID) FROM invTypes WHERE ramTypeRequirements.requiredTypeID=invTypes.typeID) AS typeIcon"
        strSQL &= " FROM eveGraphics INNER JOIN (invGroups INNER JOIN (invTypes INNER JOIN ramTypeRequirements ON invTypes.typeID=ramTypeRequirements.typeID) ON invGroups.groupID=invTypes.groupID) ON eveGraphics.graphicID=invTypes.graphicID"
        strSQL &= " WHERE(((invGroups.categoryID) = 25) And ((ramTypeRequirements.activityID) = 9) And ((invGroups.groupID) <> 465))"
        strSQL &= " ORDER BY invGroups.groupName, invTypes.typeName;"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

        Dim oreMaterials(16, 3, 8) As String
        Dim oreIcons(16, 3) As String
        Dim oreID(16, 3) As String
        Dim minIcons(8) As String
        Dim minID(8) As String

        Dim groupCount As Integer = 1
        Dim lastGroup As String = eveData.Tables(0).Rows(0).Item("groupName").ToString.Trim
        Dim typeCount As Integer = 1
        Dim lastType As String = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
        For ore As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            If eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim <> lastType Then
                typeCount += 1
                lastType = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            End If
            If eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim <> lastGroup Then
                groupCount += 1
                typeCount = 1
                lastGroup = eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim
                lastType = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            End If
            oreMaterials(groupCount, 0, 0) = eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim
            oreMaterials(groupCount, typeCount, 0) = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            oreIcons(groupCount, typeCount) = eveData.Tables(0).Rows(ore).Item("groupIcon").ToString.Trim
            oreID(groupCount, typeCount) = eveData.Tables(0).Rows(ore).Item("typeID").ToString
            Select Case eveData.Tables(0).Rows(ore).Item("Material").ToString.Trim
                Case "Tritanium"
                    oreMaterials(groupCount, typeCount, 1) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(1) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(1) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Pyerite"
                    oreMaterials(groupCount, typeCount, 2) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(2) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(2) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Mexallon"
                    oreMaterials(groupCount, typeCount, 3) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(3) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(3) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Isogen"
                    oreMaterials(groupCount, typeCount, 4) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(4) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(4) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Nocxium"
                    oreMaterials(groupCount, typeCount, 5) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(5) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(5) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Zydrine"
                    oreMaterials(groupCount, typeCount, 6) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(6) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(6) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Megacyte"
                    oreMaterials(groupCount, typeCount, 7) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(7) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(7) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Morphite"
                    oreMaterials(groupCount, typeCount, 8) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(8) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(8) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
            End Select
        Next

        strHTML &= "<table width=800px align=center>"
        strHTML &= "<tr>"
        strHTML &= "<td width=35px></td>"
        strHTML &= "<td width=165px></td>"
        For min As Integer = 1 To 8
            If forIGB = False Then
                strHTML &= "<td width=75px align=center><img src='" & EveHQ.Core.ImageHandler.GetImageLocation(minIcons(min), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
            Else
                strHTML &= "<td width=75px align=center><img src='" & EveHQ.Core.ImageHandler.GetRawImageLocation(minIcons(min), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
            End If
        Next
        strHTML &= "</tr>"
        strHTML &= "<tr>"
        strHTML &= "<td width=35px></td>"
        strHTML &= "<td width=165px></td>"
        strHTML &= "<td width=75px align=center>Tritanium</td>"
        strHTML &= "<td width=75px align=center>Pyerite</td>"
        strHTML &= "<td width=75px align=center>Mexallon</td>"
        strHTML &= "<td width=75px align=center>Isogen</td>"
        strHTML &= "<td width=75px align=center>Nocxium</td>"
        strHTML &= "<td width=75px align=center>Zydrine</td>"
        strHTML &= "<td width=75px align=center>Megacyte</td>"
        strHTML &= "<td width=75px align=center>Morphite</td>"
        strHTML &= "</tr>"

        For groupType As Integer = 1 To 16
            strHTML &= "<tr><td colspan=10 class=thead>" & oreMaterials(groupType, 0, 0) & "</td></tr>"
            For oreType As Integer = 1 To 3
                strHTML &= "<tr>"
                strHTML &= "<td width=35px>"
                If forIGB = False Then
                    strHTML &= "<img src='" & EveHQ.Core.ImageHandler.GetImageLocation(oreIcons(groupType, oreType), EveHQ.Core.ImageHandler.ImageType.Icons) & "'>"
                Else
                    strHTML &= "<img src='" & EveHQ.Core.ImageHandler.GetRawImageLocation(oreIcons(groupType, oreType), EveHQ.Core.ImageHandler.ImageType.Icons) & "'>"
                End If
                strHTML &= "</td>"
                strHTML &= "<td width=165px>" & oreMaterials(groupType, oreType, 0) & "</td>"
                For minType As Integer = 1 To 8
                    strHTML &= "<td width=75px align=center>" & oreMaterials(groupType, oreType, minType) & "</td>"
                Next
                strHTML &= "</tr>"
            Next
        Next
        strHTML &= "</table><br>"



        Return strHTML

    End Function
    Public Shared Sub GenerateIceReport()
        Dim strHTML As String = ""

        strHTML &= NonCharHTMLHeader("Ice Composition Report", "")
        strHTML &= IceReport(False)
        strHTML &= HTMLFooter()

        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "IceReport.html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Public Shared Function IceReport(ByVal forIGB As Boolean) As String
        Dim strHTML As String = ""

        Dim strSQL As String = ""
        strSQL &= "SELECT invTypes.typeID, invTypes.typeName, (SELECT typeName FROM invTypes WHERE ramTypeRequirements.requiredTypeID=invTypes.typeID) AS Material, ramTypeRequirements.requiredTypeID, ramTypeRequirements.quantity, invGroups.groupID, invGroups.groupName, (SELECT icon FROM eveGraphics WHERE invTypes.graphicID=eveGraphics.graphicID) AS groupIcon, (SELECT (SELECT icon FROM eveGraphics WHERE eveGraphics.graphicID=invTypes.graphicID) FROM invTypes WHERE ramTypeRequirements.requiredTypeID=invTypes.typeID) AS typeIcon"
        strSQL &= " FROM eveGraphics INNER JOIN (invGroups INNER JOIN (invTypes INNER JOIN ramTypeRequirements ON invTypes.typeID=ramTypeRequirements.typeID) ON invGroups.groupID=invTypes.groupID) ON eveGraphics.graphicID=invTypes.graphicID"
        strSQL &= " WHERE(((invGroups.categoryID) = 25) And ((ramTypeRequirements.activityID) = 9) And ((invGroups.groupID) = 465))"
        strSQL &= " ORDER BY invGroups.groupName, invTypes.typeName;"
        eveData = EveHQ.Core.DataFunctions.GetData(strSQL)

        Dim oreMaterials(1, 12, 7) As String
        Dim oreIcons(12, 12) As String
        Dim oreID(12, 12) As String
        Dim minIcons(7) As String
        Dim minID(7) As String

        Dim groupCount As Integer = 1
        Dim lastGroup As String = eveData.Tables(0).Rows(0).Item("groupName").ToString.Trim
        Dim typeCount As Integer = 1
        Dim lastType As String = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
        For ore As Integer = 0 To eveData.Tables(0).Rows.Count - 1
            If eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim <> lastType Then
                typeCount += 1
                lastType = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            End If
            If eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim <> lastGroup Then
                groupCount += 1
                typeCount = 1
                lastGroup = eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim
                lastType = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            End If
            oreMaterials(groupCount, 0, 0) = eveData.Tables(0).Rows(ore).Item("groupName").ToString.Trim
            oreMaterials(groupCount, typeCount, 0) = eveData.Tables(0).Rows(ore).Item("typeName").ToString.Trim
            oreIcons(groupCount, typeCount) = eveData.Tables(0).Rows(ore).Item("groupIcon").ToString.Trim
            oreID(groupCount, typeCount) = eveData.Tables(0).Rows(ore).Item("typeID").ToString
            Select Case eveData.Tables(0).Rows(ore).Item("Material").ToString.Trim
                Case "Heavy Water"
                    oreMaterials(groupCount, typeCount, 1) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(1) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(1) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Liquid Ozone"
                    oreMaterials(groupCount, typeCount, 2) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(2) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(2) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Strontium Clathrates"
                    oreMaterials(groupCount, typeCount, 3) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(3) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(3) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Helium Isotopes"
                    oreMaterials(groupCount, typeCount, 4) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(4) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(4) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Hydrogen Isotopes"
                    oreMaterials(groupCount, typeCount, 5) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(5) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(5) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Nitrogen Isotopes"
                    oreMaterials(groupCount, typeCount, 6) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(6) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(6) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
                Case "Oxygen Isotopes"
                    oreMaterials(groupCount, typeCount, 7) = eveData.Tables(0).Rows(ore).Item("quantity").ToString
                    minIcons(7) = eveData.Tables(0).Rows(ore).Item("typeIcon").ToString.Trim
                    minID(7) = eveData.Tables(0).Rows(ore).Item("requiredTypeID").ToString
            End Select
        Next

        strHTML &= "<table width=800px align=center>"
        strHTML &= "<tr>"
        strHTML &= "<td width=35px></td>"
        strHTML &= "<td width=165px></td>"
        For min As Integer = 1 To 7
            If forIGB = False Then
                strHTML &= "<td width=75px align=center><img src='" & EveHQ.Core.ImageHandler.GetImageLocation(minIcons(min), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
            Else
                strHTML &= "<td width=75px align=center><img src='" & EveHQ.Core.ImageHandler.GetRawImageLocation(minIcons(min), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
            End If
        Next
        strHTML &= "</tr>"
        strHTML &= "<tr>"
        strHTML &= "<td width=35px></td>"
        strHTML &= "<td width=205px></td>"
        strHTML &= "<td width=80px align=center>Heavy Water</td>"
        strHTML &= "<td width=80px align=center>Liquid Ozone</td>"
        strHTML &= "<td width=80px align=center>Strontium Clathrates</td>"
        strHTML &= "<td width=80px align=center>Helium Isotopes</td>"
        strHTML &= "<td width=80px align=center>Hydrogen Isotopes</td>"
        strHTML &= "<td width=80px align=center>Nitrogen Isotopes</td>"
        strHTML &= "<td width=80px align=center>Oxygen Isotopes</td>"
        strHTML &= "</tr>"

        For groupType As Integer = 1 To 1
            strHTML &= "<tr><td colspan=9 class=thead>" & oreMaterials(groupType, 0, 0) & "</td></tr>"
            For oreType As Integer = 1 To 12
                If forIGB = False Then
                    strHTML &= "<tr><td width=35px><img src='" & EveHQ.Core.ImageHandler.GetImageLocation(oreIcons(groupType, oreType), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
                Else
                    strHTML &= "<tr><td width=35px><img src='" & EveHQ.Core.ImageHandler.GetRawImageLocation(oreIcons(groupType, oreType), EveHQ.Core.ImageHandler.ImageType.Icons) & "'></td>"
                End If
                strHTML &= "<td width=165px>" & oreMaterials(groupType, oreType, 0) & "</td>"
                For minType As Integer = 1 To 7
                    strHTML &= "<td width=80px align=center>" & oreMaterials(groupType, oreType, minType) & "</td>"
                Next
                strHTML &= "</tr>"
            Next
        Next
        strHTML &= "</TABLE><BR>"

        Return strHTML

    End Function

#End Region

#Region "Character Summary Report"
    Public Shared Sub GenerateCharSummary()

        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Pilot Summary")
        strHTML &= HTMLTitle("Pilot Summary")
        strHTML &= CharSummary()
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "PilotSummary.html"))

        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub
    Public Shared Function CharSummary() As String
        Dim strHTML As String = ""
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = "0"
        Dim currentTime As String = "n/a"
        Dim totalIsk As Double = 0

        Dim rPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        strHTML &= "</tr>"
        strHTML &= "<td class=thead width=70px></td>"
        strHTML &= "<td class=thead width=165px>Pilot Name</td>"
        strHTML &= "<td class=thead width=165px>Corporation</td>"
        strHTML &= "<td class=thead width=125px align=right>Wealth</td>"
        strHTML &= "<td class=thead width=125px align=right>Skillpoints</td>"
        strHTML &= "<td class=thead width=150px align=right>Current Training</td>"
        strHTML &= "<tr>"
        Dim sortedPilots As New SortedList
        For Each rPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If rPilot.Active = True Then
                sortedPilots.Add(rPilot.Name, rPilot)
            End If
        Next
        For Each rPilot In sortedPilots.Values
            If rPilot.Active = True Then
                strHTML &= "<tr height=75px>"
                strHTML &= "<td width=70px><img src='http://img.eve.is/serv.asp?s=64&c=" & rPilot.ID & "' style='border:0px;width:64px;height:64px;' alt='" & rPilot.Name & "'></td>"
                strHTML &= "<td width=165px>" & rPilot.Name & "</td>"
                strHTML &= "<td width=165px>" & rPilot.Corp & "</td>"
                strHTML &= "<td width=125px align=right>" & FormatNumber(rPilot.Isk, 2) & "</td>"
                totalIsk += rPilot.Isk
                If rPilot.Training = True Then
                    currentSkill = CType(rPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rPilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
                    currentSP = CStr(EveHQ.Core.SkillFunctions.CalcCurrentSkillPoints(rPilot))
                    currentTime = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(rPilot))
                    strHTML &= "<td width=125px align=right>" & FormatNumber(rPilot.SkillPoints + rPilot.TrainingCurrentSP, 0) & "</td>"
                    strHTML &= "<td width=200px align=right>" & currentSkill.Name & "<br>" & currentTime & "</td>"
                Else
                    strHTML &= "<td width=125px align=right>" & FormatNumber(rPilot.SkillPoints, 0) & "</td>"
                    strHTML &= "<td width=200px align=right>n/a</td>"
                End If
                strHTML &= "</tr>"
            End If
        Next
        ' Write the total wealth line
        strHTML &= "<tr height=75px><td colspan=2></td><td>Total Wealth:</td><td align=right>" & FormatNumber(totalIsk, 2) & "</td><td colspan=2></tr>"
        strHTML &= "</table><p></p>"

        Return strHTML

    End Function
#End Region

#Region "Skill Point Report"
    Public Shared Sub GenerateSPSummary()
        Dim strHTML As String = ""
        strHTML &= NonCharHTMLHeader("Skill Level Table", "")
        strHTML &= SPSummary()
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "SPSummary.html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub
    Public Shared Function SPSummary() As String
        Dim strHTML As String = ""

        strHTML &= "<table border=1 width=800px align=center cellspacing=0 cellpadding=0>"
        strHTML &= "<tr><td width=175px>&nbsp;</td><td colspan=5 align=center class=thead><b>Skill Level</b></td></tr><tr>"
        strHTML &= "<td>&nbsp;</td>"

        For level As Integer = 1 To 5
            strHTML &= "<td width=125px align=center>" & level & "</td>"
        Next
        strHTML &= "</tr>"
        For rank As Integer = 1 To 20
            strHTML &= "<tr><td colspan=6 class=thead>Skill Rank " & rank & "</td></tr>"
            strHTML &= "<tr><td>&nbsp;</td>"
            For level As Integer = 1 To 5
                strHTML &= "<td align=center>" & FormatNumber(Math.Ceiling(EveHQ.Core.SkillFunctions.CalculateSPLevel(rank, level)), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "</td>"
            Next
            strHTML &= "</tr>"
        Next
        strHTML &= "</table><p></p>"

        Return strHTML

    End Function

#End Region

#Region "Current Pilot XML Report (Old Style)"

    Public Shared Sub GenerateCurrentPilotXML_Old(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strXML As String = ""
        strXML &= CurrentPilotXML_Old(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "CurrentXML - Old (" & rPilot.Name & ").xml"))
        sw.Write(strXML)
        sw.Flush()
        sw.Close()
        strXML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub

    Public Shared Function CurrentPilotXML_Old(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim tabs(10) As String
        For tab As Integer = 1 To 10
            For tab2 As Integer = 1 To tab
                tabs(tab) &= Chr(9)
            Next
        Next

        Dim strXML As String = "<?xml version=""1.0"" encoding=""iso-8859-1"" ?>" & vbCrLf
        strXML &= tabs(0) & "<charactersheet>" & vbCrLf
        strXML &= tabs(1) & "<characters>" & vbCrLf
        strXML &= tabs(2) & "<character timeInCache=""0"" timeLeftInCache=""1000"" name=""" & rpilot.Name & """ characterID=""" & rpilot.ID & """>" & vbCrLf

        strXML &= tabs(3) & "<race>" & rpilot.Race & "</race>" & vbCrLf
        strXML &= tabs(3) & "<bloodLine>" & rpilot.Blood & "</bloodLine>" & vbCrLf
        strXML &= tabs(3) & "<gender>" & rpilot.Gender & "</gender>" & vbCrLf
        strXML &= tabs(3) & "<corporationName>" & rpilot.Corp & "</corporationName>" & vbCrLf
        strXML &= tabs(3) & "<balance>" & rpilot.Isk & "</balance>" & vbCrLf
        strXML &= XMLImplantDetails(rpilot)
        strXML &= tabs(3) & "<attributes>" & vbCrLf
        strXML &= tabs(4) & "<intelligence>" & rpilot.IAtt & "</intelligence>" & vbCrLf
        strXML &= tabs(4) & "<charisma>" & rpilot.CAtt & "</charisma>" & vbCrLf
        strXML &= tabs(4) & "<perception>" & rpilot.PAtt & "</perception>" & vbCrLf
        strXML &= tabs(4) & "<memory>" & rpilot.MAtt & "</memory>" & vbCrLf
        strXML &= tabs(4) & "<willpower>" & rpilot.WAtt & "</willpower>" & vbCrLf
        strXML &= tabs(3) & "</attributes>" & vbCrLf
        strXML &= tabs(3) & "<skills>" & vbCrLf

        For Each skillGroup As EveHQ.Core.SkillGroup In EveHQ.Core.HQ.SkillGroups.Values
            If skillGroup.ID <> "505" Then
                strXML &= tabs(4) & "<skillGroup groupName=""" & skillGroup.Name & """ groupID=""" & skillGroup.ID & """>" & vbCrLf
                For Each skillItem As EveHQ.Core.PilotSkill In rpilot.PilotSkills
                    If skillItem.GroupID = skillGroup.ID Then
                        strXML &= tabs(5) & "<skill typeName=""" & skillItem.Name & """ typeID=""" & skillItem.ID & """>" & vbCrLf
                        strXML &= tabs(6) & "<groupID>" & skillGroup.ID & "</groupID>" & vbCrLf
                        If rpilot.TrainingSkillID = skillItem.ID Then
                            strXML &= tabs(6) & "<flag>61</flag>" & vbCrLf
                            strXML &= tabs(6) & "<rank>" & skillItem.Rank & "</rank>" & vbCrLf
                            If rpilot.TrainingCurrentTime <= 0 And rpilot.TrainingSkillLevel <> skillItem.Level Then
                                strXML &= tabs(6) & "<skillpoints>" & skillItem.SP + rpilot.TrainingCurrentSP & "</skillpoints>" & vbCrLf
                                If skillItem.Level < 5 Then
                                    strXML &= tabs(6) & "<level>" & skillItem.Level + 1 & "</level>" & vbCrLf
                                Else
                                    strXML &= tabs(6) & "<level>" & skillItem.Level & "</level>" & vbCrLf
                                End If
                            Else
                                strXML &= tabs(6) & "<skillpoints>" & skillItem.SP + rpilot.TrainingCurrentSP & "</skillpoints>" & vbCrLf
                                strXML &= tabs(6) & "<level>" & skillItem.Level & "</level>" & vbCrLf
                            End If
                        Else
                            strXML &= tabs(6) & "<flag>7</flag>" & vbCrLf
                            strXML &= tabs(6) & "<rank>" & skillItem.Rank & "</rank>" & vbCrLf
                            strXML &= tabs(6) & "<skillpoints>" & skillItem.SP & "</skillpoints>" & vbCrLf
                            strXML &= tabs(6) & "<level>" & skillItem.Level & "</level>" & vbCrLf
                        End If

                        strXML &= tabs(6) & "<skilllevel1>" & skillItem.LevelUp(1) & "</skilllevel1>" & vbCrLf
                        strXML &= tabs(6) & "<skilllevel2>" & skillItem.LevelUp(2) & "</skilllevel2>" & vbCrLf
                        strXML &= tabs(6) & "<skilllevel3>" & skillItem.LevelUp(3) & "</skilllevel3>" & vbCrLf
                        strXML &= tabs(6) & "<skilllevel4>" & skillItem.LevelUp(4) & "</skilllevel4>" & vbCrLf
                        strXML &= tabs(6) & "<skilllevel5>" & skillItem.LevelUp(5) & "</skilllevel5>" & vbCrLf
                        strXML &= tabs(5) & "</skill>" & vbCrLf
                    End If
                Next
                strXML &= tabs(4) & "</skillGroup>" & vbCrLf
            End If
        Next

        strXML &= tabs(3) & "</skills>" & vbCrLf
        strXML &= tabs(2) & "</character>" & vbCrLf
        strXML &= tabs(1) & "</characters>" & vbCrLf
        strXML &= tabs(0) & "</charactersheet>" & vbCrLf
        Return strXML
    End Function

    Public Shared Sub GenerateCurrentTrainingXML_Old(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strXML As String = ""
        strXML &= CurrentTrainingXML_Old(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML - Old (" & rPilot.Name & ").xml"))
        sw.Write(strXML)
        sw.Flush()
        sw.Close()
        strXML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub

    Public Shared Function CurrentTrainingXML_Old(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim tabs(10) As String
        For tab As Integer = 1 To 10
            For tab2 As Integer = 1 To tab
                tabs(tab) &= Chr(9)
            Next
        Next

        Dim strXML As String = "<?xml version=""1.0"" encoding=""iso-8859-1"" ?>" & vbCrLf
        strXML &= tabs(0) & "<charactersheet>" & vbCrLf
        strXML &= tabs(1) & "<characters>" & vbCrLf
        strXML &= tabs(2) & "<character timeInCache=""0"" timeLeftInCache=""1000"" name=""" & rpilot.Name & """ characterID=""" & rpilot.ID & """>" & vbCrLf
        strXML &= tabs(3) & "<race>" & rpilot.Race & "</race>" & vbCrLf
        strXML &= tabs(3) & "<bloodLine>" & rpilot.Blood & "</bloodLine>" & vbCrLf
        strXML &= tabs(3) & "<gender>" & rpilot.Gender & "</gender>" & vbCrLf
        strXML &= tabs(3) & "<corporationName>" & rpilot.Corp & "</corporationName>" & vbCrLf
        strXML &= tabs(3) & "<balance>" & rpilot.Isk & "</balance>" & vbCrLf
        strXML &= tabs(3) & "<attributeEnhancers timeInCache=""0"" timeLeftInCache=""1000"" />" & vbCrLf
        strXML &= tabs(3) & "<attributes>" & vbCrLf
        strXML &= tabs(4) & "<intelligence>" & rpilot.IAtt & "</intelligence>" & vbCrLf
        strXML &= tabs(4) & "<charisma>" & rpilot.CAtt & "</charisma>" & vbCrLf
        strXML &= tabs(4) & "<perception>" & rpilot.PAtt & "</perception>" & vbCrLf
        strXML &= tabs(4) & "<memory>" & rpilot.MAtt & "</memory>" & vbCrLf
        strXML &= tabs(4) & "<willpower>" & rpilot.WAtt & "</willpower>" & vbCrLf
        strXML &= tabs(3) & "</attributes>" & vbCrLf
        strXML &= tabs(3) & "<skillTraining>" & vbCrLf

        strXML &= tabs(4) & "<skill typeID=""" & rpilot.TrainingSkillID & """ trainingStartTime=""" & rpilot.TrainingStartTimeActual.ToShortDateString & " " & rpilot.TrainingStartTimeActual.ToLongTimeString & """ trainingEndTime=""" & rpilot.TrainingEndTimeActual.ToShortDateString & " " & rpilot.TrainingEndTimeActual.ToLongTimeString & """>" & vbCrLf
        strXML &= tabs(5) & "<characterID>" & rpilot.ID & "</characterID>" & vbCrLf
        strXML &= tabs(5) & "<startSP>" & rpilot.TrainingStartSP & "</startSP>" & vbCrLf
        strXML &= tabs(5) & "<destinationSP>" & rpilot.TrainingEndSP & "</destinationSP>" & vbCrLf
        strXML &= tabs(5) & "<trainingToLevel>" & rpilot.TrainingSkillLevel & "</trainingToLevel>" & vbCrLf

        strXML &= tabs(4) & "</skill>" & vbCrLf
        strXML &= tabs(3) & "</skillTraining>" & vbCrLf
        strXML &= tabs(2) & "</character>" & vbCrLf
        strXML &= tabs(1) & "</characters>" & vbCrLf
        strXML &= tabs(0) & "</charactersheet>" & vbCrLf
        Return strXML
    End Function

#End Region

#Region "Current Pilot XML Report (New Style)"

    Public Shared Sub GenerateCurrentPilotXML_New(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strXML As String = ""
        strXML &= CurrentPilotXML_New(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "CurrentXML - New (" & rPilot.Name & ").xml"))
        sw.Write(strXML)
        sw.Flush()
        sw.Close()
        strXML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub

    Public Shared Function CurrentPilotXML_New(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim tabs(10) As String
        For tab As Integer = 1 To 10
            For tab2 As Integer = 1 To tab
                tabs(tab) &= Chr(9)
            Next
        Next

        Dim strXML As String = "<?xml version=""1.0"" encoding=""iso-8859-1"" ?>" & vbCrLf
        strXML &= tabs(0) & "<eveapi version=""1"">" & vbCrLf
        strXML &= tabs(1) & "<currentTime>" & Format(rpilot.CacheFileTime, "yyyy-MM-dd HH:mm:ss") & "</currentTime>" & vbCrLf
        strXML &= tabs(1) & "<result>"

        strXML &= tabs(2) & "<characterID>" & rpilot.ID & "</characterID>" & vbCrLf
        strXML &= tabs(2) & "<name>" & rpilot.Name & "</name>" & vbCrLf
        strXML &= tabs(2) & "<race>" & rpilot.Race & "</race>" & vbCrLf
        strXML &= tabs(2) & "<bloodLine>" & rpilot.Blood & "</bloodLine>" & vbCrLf
        strXML &= tabs(2) & "<gender>" & rpilot.Gender & "</gender>" & vbCrLf
        strXML &= tabs(2) & "<corporationName>" & rpilot.Corp & "</corporationName>" & vbCrLf
        strXML &= tabs(2) & "<corporationID>" & rpilot.CorpID & "</corporationID>" & vbCrLf
        strXML &= tabs(2) & "<cloneName>" & rpilot.CloneName & "</cloneName>" & vbCrLf
        strXML &= tabs(2) & "<cloneSkillPoints>" & rpilot.CloneSP & "</cloneSkillPoints>" & vbCrLf
        ' Make the isk value non-culture specfic using en-GB
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
        strXML &= tabs(2) & "<balance>" & rpilot.Isk.ToString(culture.NumberFormat) & "</balance>" & vbCrLf
        strXML &= XMLImplantDetails(rpilot)
        strXML &= tabs(2) & "<attributes>" & vbCrLf
        strXML &= tabs(3) & "<intelligence>" & rpilot.IAtt & "</intelligence>" & vbCrLf
        strXML &= tabs(3) & "<charisma>" & rpilot.CAtt & "</charisma>" & vbCrLf
        strXML &= tabs(3) & "<perception>" & rpilot.PAtt & "</perception>" & vbCrLf
        strXML &= tabs(3) & "<memory>" & rpilot.MAtt & "</memory>" & vbCrLf
        strXML &= tabs(3) & "<willpower>" & rpilot.WAtt & "</willpower>" & vbCrLf
        strXML &= tabs(2) & "</attributes>" & vbCrLf
        strXML &= tabs(2) & "<rowset name=""skills"" key=""typeID"" columns=""typeID,skillpoints,level,unpublished"">" & vbCrLf

        For Each skillItem As EveHQ.Core.PilotSkill In rpilot.PilotSkills
            If rpilot.TrainingSkillID = skillItem.ID Then
                strXML &= tabs(3) & "<row typeID=""" & skillItem.ID & """ skillpoints=""" & skillItem.SP + rpilot.TrainingCurrentSP & """"
                If rpilot.TrainingCurrentTime <= 0 And rpilot.TrainingSkillLevel <> skillItem.Level Then
                    If skillItem.Level < 5 Then
                        strXML &= tabs(3) & " level=""" & skillItem.Level + 1 & """ />" & vbCrLf
                    Else
                        strXML &= tabs(3) & " level=""" & skillItem.Level & """ />" & vbCrLf
                    End If
                Else
                    strXML &= tabs(3) & " level=""" & skillItem.Level & """ />" & vbCrLf
                End If
            Else
                strXML &= tabs(3) & "<row typeID=""" & skillItem.ID & """ skillpoints=""" & skillItem.SP & """ level=""" & skillItem.Level & """ />" & vbCrLf
            End If
        Next

        strXML &= tabs(2) & "</rowset>" & vbCrLf
        strXML &= tabs(1) & "</result>" & vbCrLf
        strXML &= tabs(1) & "<cachedUntil>" & Format(rpilot.CacheExpirationTime, "yyyy-MM-dd HH:mm:ss") & "</cachedUntil>" & vbCrLf
        strXML &= tabs(0) & "</eveapi>" & vbCrLf
        Return strXML
    End Function

    Public Shared Function CurrentTrainingXML_New(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim tabs(10) As String
        For tab As Integer = 1 To 10
            For tab2 As Integer = 1 To tab
                tabs(tab) &= Chr(9)
            Next
        Next

        Dim strXML As String = "<?xml version=""1.0"" encoding=""iso-8859-1"" ?>" & vbCrLf

        strXML &= tabs(0) & "<eveapi version=""2"">" & vbCrLf
        strXML &= tabs(1) & "<currentTime>" & Format(rpilot.CacheFileTime, "yyyy-MM-dd HH:mm:ss") & "</currentTime>" & vbCrLf
        strXML &= tabs(1) & "<result>" & vbCrLf
        strXML &= tabs(2) & "<rowset name=""skillqueue"" key=""queuePosition"" columns=""queuePosition,typeID,level,startSP,endSP,startTime,endTime"">" & vbCrLf
        If rpilot.Training = True Then
            strXML &= tabs(3) & "<row queuePosition=""1"""
            strXML &= " typeID=""" & rpilot.TrainingSkillID & """"
            strXML &= " level=""" & rpilot.TrainingSkillLevel & """"
            strXML &= " startSP=""" & rpilot.TrainingStartSP & """"
            strXML &= " endSP=""" & rpilot.TrainingEndSP & """"
            strXML &= " startTime=""" & Format(rpilot.TrainingStartTimeActual, "yyyy-MM-dd HH:mm:ss") & """"
            strXML &= " endTime=""" & Format(rpilot.TrainingEndTimeActual, "yyyy-MM-dd HH:mm:ss") & """"
            strXML &= " />" & vbCrLf
        End If
        strXML &= tabs(2) & "</rowset>" & vbCrLf
        strXML &= tabs(1) & "</result>" & vbCrLf
        strXML &= tabs(1) & "<cachedUntil>" & Format(EveHQ.Core.SkillFunctions.ConvertLocalTimeToEve(Now), "yyyy-MM-dd HH:mm:ss") & "</cachedUntil>" & vbCrLf
        strXML &= tabs(0) & "</eveapi>" & vbCrLf

        Return strXML

    End Function

#End Region

#Region "Skill Group Chart"
    Public Shared Function SkillGroupChart(ByVal rpilot As EveHQ.Core.Pilot) As ZedGraph.ZedGraphControl
        Dim zgc As New ZedGraph.ZedGraphControl
        Dim myPane As ZedGraph.GraphPane = zgc.GraphPane

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim otherGroup As Double
        Dim otherList As New ArrayList
        Dim repGroup(EveHQ.Core.HQ.SkillGroups.Count, 3) As String
        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 5) As String
        Dim cGroup As EveHQ.Core.SkillGroup = New EveHQ.Core.SkillGroup
        Dim cSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim groupCount As Integer = 0
        Dim totalSPCount As Long = 0
        For Each cGroup In EveHQ.Core.HQ.SkillGroups.Values
            groupCount += 1
            repGroup(groupCount, 1) = cGroup.Name
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            For Each cSkill In rpilot.PilotSkills
                If cSkill.GroupID = cGroup.ID Then
                    skillCount += 1
                    SPCount += cSkill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cSkill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cSkill.Name), 0))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                    If rpilot.Training = True Then
                        If currentSkill.ID = cSkill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            totalSPCount += SPCount
        Next

        ' Set the GraphPane title
        myPane.Title.Text = "Skill Breakdown by Category - " & rpilot.Name
        myPane.Title.FontSpec.IsItalic = True
        myPane.Title.FontSpec.Size = 18.0F
        myPane.Title.FontSpec.Family = "Arial"

        ' Fill the pane background with a color gradient
        myPane.Fill = New ZedGraph.Fill(Color.White, Color.LightSteelBlue, 45.0F)
        ' No fill for the chart background
        myPane.Chart.Fill.Type = ZedGraph.FillType.None

        ' Set the legend to an arbitrary location
        myPane.Legend.IsVisible = False
        myPane.Legend.Position = ZedGraph.LegendPos.TopCenter
        myPane.Legend.FontSpec.Size = 8.0F
        myPane.Legend.IsHStack = False

        ' Add some pie slices
        Dim segment(EveHQ.Core.HQ.SkillGroups.Count) As ZedGraph.PieItem
        For group As Integer = 1 To EveHQ.Core.HQ.SkillGroups.Count
            If CDbl(repGroup(group, 3)) > 0 Then
                If CDbl(repGroup(group, 3)) / totalSPCount * 100 > 2.5 Then
                    segment(group) = myPane.AddPieSlice(CDbl(repGroup(group, 3)), EveHQ.Core.Reports.RandomRGBColor, Color.White, 45, 0.05, repGroup(group, 1) & " (" & FormatNumber(repGroup(group, 3), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")")
                    segment(group).LabelType = ZedGraph.PieLabelType.Name_Percent
                    segment(group).LabelDetail.FontSpec.Size = 8
                Else
                    otherGroup += CDbl(repGroup(group, 3))
                    otherList.Add(repGroup(group, 1) & ": " & FormatNumber(repGroup(group, 3), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault))
                End If
            End If
        Next
        If otherGroup > 0 Then
            segment(0) = myPane.AddPieSlice(otherGroup, EveHQ.Core.Reports.RandomRGBColor, Color.White, 45, 0.05, "Other Groups (" & FormatNumber(otherGroup, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & ")")
            segment(0).LabelType = ZedGraph.PieLabelType.Name_Percent
            segment(0).LabelDetail.FontSpec.Size = 8
            ' Make a text label to highlight the total value
            Dim text As New ZedGraph.TextObj("Others:" & ControlChars.CrLf, 0.1, 0.2, ZedGraph.CoordType.PaneFraction)
            text.Location.AlignH = ZedGraph.AlignH.Center
            text.Location.AlignV = ZedGraph.AlignV.Bottom
            text.FontSpec.Size = 7
            text.FontSpec.Border.IsVisible = False
            text.FontSpec.Fill = New ZedGraph.Fill(Color.FromArgb(200, 200, 255), Color.FromArgb(100, 100, 255), 45.0F)
            text.FontSpec.StringAlignment = StringAlignment.Center
            myPane.GraphObjList.Add(text)
            For Each groupText As String In otherList
                text.Text &= groupText & ControlChars.CrLf
            Next
        End If

        ' Calculate the Axis Scale Ranges
        zgc.AxisChange()
        zgc.Top = 0 : zgc.Left = 0 : zgc.Dock = Windows.Forms.DockStyle.Fill
        Return zgc
    End Function

    Private Shared Function RandomRGBColor() As Color
        Static colRandom As New Random(CInt(Math.Sqrt(Now.Ticks)))
        Dim r As Integer = colRandom.Next(0, 255)
        Dim g As Integer = colRandom.Next(0, 255)
        Dim b As Integer = colRandom.Next(0, 255)
        Return Color.FromArgb(255, r, g, b)
        colRandom = New Random(r * g * b)
    End Function
#End Region

#Region "Text Character Sheet Report"
    Public Shared Sub GenerateTextCharSheet(ByVal rpilot As EveHQ.Core.Pilot)

        Dim strText As String = ""
        strText &= TextCharacterDetails("Character Sheet", rpilot)
        strText &= TextCharacterSheet(rpilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "CharSheet (" & rpilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextCharacterSheet(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim repGroup(EveHQ.Core.HQ.SkillGroups.Count, 3) As String
        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 5) As String
        Dim cGroup As EveHQ.Core.SkillGroup = New EveHQ.Core.SkillGroup
        Dim cSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim groupCount As Integer = 0
        For Each cGroup In EveHQ.Core.HQ.SkillGroups.Values
            groupCount += 1
            repGroup(groupCount, 1) = cGroup.Name
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            ' Collect skills
            Dim repSkills As New SortedList(Of String, EveHQ.Core.PilotSkill)
            For Each cSkill In rpilot.PilotSkills
                repSkills.Add(cSkill.Name, cSkill)
            Next
            For Each cSkill In repSkills.Values
                If cSkill.GroupID = cGroup.ID Then
                    skillCount += 1
                    SPCount += cSkill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cSkill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cSkill.Name), 0))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                    If rpilot.Training = True Then
                        If currentSkill.ID = cSkill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
        Next

        For group As Integer = 1 To EveHQ.Core.HQ.SkillGroups.Count
            If CDbl(repGroup(group, 2)) > 0 Then
                strText.AppendLine(repGroup(group, 1) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    Dim txtData(4) As String
                    txtData(0) = "  * " & repSkill(group, skill, 1)
                    txtData(1) = "Rank " & repSkill(group, skill, 2)
                    txtData(2) = "Level " & repSkill(group, skill, 5)
                    txtData(3) = "SP " & FormatNumber(repSkill(group, skill, 3), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    txtData(4) = "TTNL " & repSkill(group, skill, 4)
                    strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-15} {4,-25}", txtData))
                Next
                strText.AppendLine()
            End If
        Next

        Return strText.ToString
    End Function

#End Region

#Region "Text Training Times Report"
    Public Shared Sub GenerateTextTrainingTime(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strText As String = ""
        strText &= TextCharacterDetails("Training Times", rPilot)
        strText &= TextTrainingTime(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainTime (" & rPilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextTrainingTime(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            ' Determine if the skill is being trained
            If rpilot.Training = True Then
                If curSkill.ID = rpilot.TrainingSkillID Then
                    sortSkill(count, 1) = rpilot.TrainingCurrentTime
                Else
                    sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
                End If
            Else
                sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
            End If
            sortSkill(count, 0) = CLng(curSkill.ID)
            count += 1
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(rpilot.PilotSkills.Count - 1) As Integer
        For a As Integer = 0 To rpilot.PilotSkills.Count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Training Completed" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 1 hour" : repGroup(2, 1) = "1" : repGroup(2, 2) = "3600"
        repGroup(3, 0) = "1 to 2 hours" : repGroup(3, 1) = "3601" : repGroup(3, 2) = "7200"
        repGroup(4, 0) = "2 to 4 hours" : repGroup(4, 1) = "7201" : repGroup(4, 2) = "14400"
        repGroup(5, 0) = "4 to 6 hours" : repGroup(5, 1) = "14401" : repGroup(5, 2) = "21600"
        repGroup(6, 0) = "6 to 8 hours" : repGroup(6, 1) = "21601" : repGroup(6, 2) = "28800"
        repGroup(7, 0) = "8 to 16 hours" : repGroup(7, 1) = "28801" : repGroup(7, 2) = "57600"
        repGroup(8, 0) = "16 to 24 hours" : repGroup(8, 1) = "57601" : repGroup(8, 2) = "86400"
        repGroup(9, 0) = "1 to 2 days" : repGroup(9, 1) = "86401" : repGroup(9, 2) = "172800"
        repGroup(10, 0) = "2 to 4 days" : repGroup(10, 1) = "172801" : repGroup(10, 2) = "345600"
        repGroup(11, 0) = "4 to 7 days" : repGroup(11, 1) = "345601" : repGroup(11, 2) = "604800"
        repGroup(12, 0) = "7 to 14 days" : repGroup(12, 1) = "604801" : repGroup(12, 2) = "1209600"
        repGroup(13, 0) = "14 to 21 days" : repGroup(13, 1) = "1209601" : repGroup(13, 2) = "1814400"
        repGroup(14, 0) = "21 to 28 days" : repGroup(14, 1) = "1814401" : repGroup(14, 2) = "2419200"
        repGroup(15, 0) = "28 days or more" : repGroup(15, 1) = "2419200" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim cskill As EveHQ.Core.PilotSkill
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                    skillCount += 1
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    If rpilot.Training = True Then
                        If currentSkill.ID = cskill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        Dim group As Integer = 1
        Do
            group = group + 1
            If group = nog + 1 Then group = 1
            If CDbl(repGroup(group, 2)) > 0 Then
                strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    Dim txtData(4) As String
                    txtData(0) = "  * " & repSkill(group, skill, 1)
                    txtData(1) = "Rank " & repSkill(group, skill, 2)
                    txtData(2) = "Level " & repSkill(group, skill, 5)
                    txtData(3) = "SP " & FormatNumber(repSkill(group, skill, 3), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    txtData(4) = "TTNL " & repSkill(group, skill, 4)
                    strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-15} {4,-25}", txtData))
                Next
                strText.AppendLine(String.Format("{0,-45} {1,-40}", "", "Time to Clear Group: " & repGroup(group, 4)))
                strText.AppendLine()
            End If
        Loop Until group = 1

        Return strText.ToString
    End Function
#End Region

#Region "Text Time to Level 5 Report"
    Public Shared Sub GenerateTextTimeToLevel5(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strText As String = ""
        strText &= TextCharacterDetails("Time To Level 5", rPilot)
        strText &= TextTimeToLevel5(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextTimeToLevel5(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            ' Determine if the skill is being trained
            If rpilot.Training = True Then
                sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 5) - redTime, 0))
            Else
                sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 5) - redTime, 0))
            End If
            sortSkill(count, 0) = CLng(curSkill.ID)
            count += 1
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(rpilot.PilotSkills.Count - 1) As Integer
        For a As Integer = 0 To rpilot.PilotSkills.Count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Training Completed" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 24 hours" : repGroup(2, 1) = "1" : repGroup(2, 2) = "86400"
        repGroup(3, 0) = "1 to 3 days" : repGroup(3, 1) = "86401" : repGroup(3, 2) = "259200"
        repGroup(4, 0) = "3 to 5 days" : repGroup(4, 1) = "259201" : repGroup(4, 2) = "432000"
        repGroup(5, 0) = "5 to 7 days" : repGroup(5, 1) = "432001" : repGroup(5, 2) = "604800"
        repGroup(6, 0) = "7 to 10 days" : repGroup(6, 1) = "604801" : repGroup(6, 2) = "864000"
        repGroup(7, 0) = "10 to 14 days" : repGroup(7, 1) = "864001" : repGroup(7, 2) = "1209600"
        repGroup(8, 0) = "14 to 21 days" : repGroup(8, 1) = "1209601" : repGroup(8, 2) = "1814400"
        repGroup(9, 0) = "21 to 30 days" : repGroup(9, 1) = "1814401" : repGroup(9, 2) = "2592000"
        repGroup(10, 0) = "30 to 45 days" : repGroup(10, 1) = "2592001" : repGroup(10, 2) = "3888000"
        repGroup(11, 0) = "45 to 60 days" : repGroup(11, 1) = "3888001" : repGroup(11, 2) = "5184000"
        repGroup(12, 0) = "60 to 75 days" : repGroup(12, 1) = "5184001" : repGroup(12, 2) = "6480000"
        repGroup(13, 0) = "75 to 90 days" : repGroup(13, 1) = "6480001" : repGroup(13, 2) = "7776000"
        repGroup(14, 0) = "90 to 120 days" : repGroup(14, 1) = "7776001" : repGroup(14, 2) = "10368000"
        repGroup(15, 0) = "120 days or more" : repGroup(15, 1) = "10368001" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim cskill As EveHQ.Core.PilotSkill
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                    skillCount += 1
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 5)
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 5))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next


        Dim group As Integer = 1
        Do
            group = group + 1
            If group = nog + 1 Then group = 1
            If CDbl(repGroup(group, 2)) > 0 Then
                strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    Dim txtData(4) As String
                    txtData(0) = "  * " & repSkill(group, skill, 1)
                    txtData(1) = "Rank " & repSkill(group, skill, 2)
                    txtData(2) = "Level " & repSkill(group, skill, 5)
                    txtData(3) = "SP " & FormatNumber(repSkill(group, skill, 3), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    txtData(4) = "TTNL " & repSkill(group, skill, 4)
                    strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-15} {4,-25}", txtData))
                Next
                strText.AppendLine(String.Format("{0,-45} {1,-40}", "", "Time to Clear Group: " & repGroup(group, 4)))
                strText.AppendLine()
            End If
        Loop Until group = 1

        Return strText.ToString
    End Function
#End Region

#Region "Text Skill Levels Report"
    Public Shared Sub GenerateTextSkillLevels(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strHTML As String = ""
        strHTML &= TextCharacterDetails("Skill Levels", rPilot)
        strHTML &= TextSkillLevels(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").txt"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextSkillLevels(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As String
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            sortSkill(count, 0) = curSkill.ID
            sortSkill(count, 1) = curSkill.Name
            count += 1
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(rpilot.PilotSkills.Count - 1) As Integer
        For a As Integer = 0 To rpilot.PilotSkills.Count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort (use string version!!)
        Dim myComparer As New ArrayComparerString(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 6              ' Number of groups to break the report into
        Dim repGroup(nog, 5) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Level 0" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Level 1" : repGroup(2, 1) = "1" : repGroup(2, 2) = "1"
        repGroup(3, 0) = "Level 2" : repGroup(3, 1) = "2" : repGroup(3, 2) = "2"
        repGroup(4, 0) = "Level 3" : repGroup(4, 1) = "3" : repGroup(4, 2) = "3"
        repGroup(5, 0) = "Level 4" : repGroup(5, 1) = "4" : repGroup(5, 2) = "4"
        repGroup(6, 0) = "Level 5" : repGroup(6, 1) = "5" : repGroup(6, 2) = "5"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                Dim cskill As EveHQ.Core.PilotSkill
                Dim askill As EveHQ.Core.EveSkill
                askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                If cskill.Level >= CDbl(repGroup(groupCount, 1)) And cskill.Level <= CDbl(repGroup(groupCount, 2)) Then
                    skillCount += 1
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    Dim curTime As Double = EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(curTime)
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    If rpilot.Training = True Then
                        If currentSkill.ID = cskill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        For group As Integer = 1 To nog
            If CDbl(repGroup(group, 2)) > 0 Then
                strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    Dim txtData(4) As String
                    txtData(0) = "  * " & repSkill(group, skill, 1)
                    txtData(1) = "Rank " & repSkill(group, skill, 2)
                    txtData(2) = "Level " & repSkill(group, skill, 5)
                    txtData(3) = "SP " & FormatNumber(repSkill(group, skill, 3), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    txtData(4) = "TTNL " & repSkill(group, skill, 4)
                    strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-15} {4,-25}", txtData))
                Next
                strText.AppendLine(String.Format("{0,-45} {1,-40}", "", "Time to Clear Group: " & repGroup(group, 4)))
                strText.AppendLine()
            End If
        Next

        Return strText.ToString
    End Function

#End Region

#Region "Skills Available Report"

    Public Shared Sub GenerateTextSkillsAvailable(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strText As String = ""
        strText &= TextCharacterDetails("Skills Available To Train", rPilot)
        strText &= TextSkillsAvailable(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextSkillsAvailable(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        strText.AppendLine(String.Format("{0,-45} {1,-25} {2,-10}", "Skill Name", "Skill Group", "Skill Rank"))
        strText.AppendLine(String.Format("{0,-45} {1,-25} {2,-10}", "----------", "-----------", "----------"))

        Dim trainable As Boolean = False
        For Each skill As EveHQ.Core.EveSkill In EveHQ.Core.HQ.SkillListName.Values
            trainable = False
            If rpilot.PilotSkills.Contains(skill.Name) = False And skill.Published = True Then
                trainable = True
                For Each preReq As String In skill.PreReqSkills.Keys
                    If skill.PreReqSkills(preReq) <> 0 Then
                        Dim ps As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListID(preReq)
                        If rpilot.PilotSkills.Contains(ps.Name) = True Then
                            Dim psp As EveHQ.Core.PilotSkill = CType(rpilot.PilotSkills(ps.Name), EveHQ.Core.PilotSkill)
                            If psp.Level < skill.PreReqSkills(preReq) Then
                                trainable = False
                                Exit For
                            End If
                        Else
                            trainable = False
                            Exit For
                        End If
                    End If
                Next
            End If

            If trainable = True Then
                strText.AppendLine(String.Format("{0,-45} {1,-29} {2,-5}", skill.Name, EveHQ.Core.HQ.itemGroups(skill.GroupID), skill.Rank))
            End If
        Next

        Return strText.ToString
    End Function

#End Region

#Region "Text Skills Not Trained Report"
    Public Shared Sub GenerateTextSkillsNotTrained(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strText As String = ""
        strText &= TextCharacterDetails("Skills Not Trained", rPilot)
        strText &= TextSkillsNotTrained(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing
        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextSkillsNotTrained(ByVal rPilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rPilot.Training = True Then
            currentSkill = CType(rPilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rPilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rPilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rPilot.TrainingCurrentTime)
        End If

        Dim sortSkill(EveHQ.Core.HQ.SkillListID.Count, 1) As Long
        Dim curSkill As EveHQ.Core.EveSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In EveHQ.Core.HQ.SkillListID.Values
            If curSkill.Published = True Then
                If rPilot.PilotSkills.Contains(curSkill.Name) = False Then
                    ' Determine if the skill is being trained
                    sortSkill(count, 1) = CLng(EveHQ.Core.SkillFunctions.TimeBeforeCanTrain(rPilot, curSkill.ID))
                    sortSkill(count, 0) = CLng(curSkill.ID)
                    count += 1
                End If
            End If
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(count - 1) As Integer
        For a As Integer = 0 To count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Ready To Start Training" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 24 hours" : repGroup(2, 1) = "1" : repGroup(2, 2) = "86400"
        repGroup(3, 0) = "1 to 3 days" : repGroup(3, 1) = "86401" : repGroup(3, 2) = "259200"
        repGroup(4, 0) = "3 to 5 days" : repGroup(4, 1) = "259201" : repGroup(4, 2) = "432000"
        repGroup(5, 0) = "5 to 7 days" : repGroup(5, 1) = "432001" : repGroup(5, 2) = "604800"
        repGroup(6, 0) = "7 to 10 days" : repGroup(6, 1) = "604801" : repGroup(6, 2) = "864000"
        repGroup(7, 0) = "10 to 14 days" : repGroup(7, 1) = "864001" : repGroup(7, 2) = "1209600"
        repGroup(8, 0) = "14 to 21 days" : repGroup(8, 1) = "1209601" : repGroup(8, 2) = "1814400"
        repGroup(9, 0) = "21 to 30 days" : repGroup(9, 1) = "1814401" : repGroup(9, 2) = "2592000"
        repGroup(10, 0) = "30 to 45 days" : repGroup(10, 1) = "2592001" : repGroup(10, 2) = "3888000"
        repGroup(11, 0) = "45 to 60 days" : repGroup(11, 1) = "3888001" : repGroup(11, 2) = "5184000"
        repGroup(12, 0) = "60 to 75 days" : repGroup(12, 1) = "5184001" : repGroup(12, 2) = "6480000"
        repGroup(13, 0) = "75 to 90 days" : repGroup(13, 1) = "6480001" : repGroup(13, 2) = "7776000"
        repGroup(14, 0) = "90 to 120 days" : repGroup(14, 1) = "7776001" : repGroup(14, 2) = "10368000"
        repGroup(15, 0) = "120 days or more" : repGroup(15, 1) = "10368001" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    skillCount += 1
                    TotalTime += skillTimeLeft
                    SPCount += askill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = askill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = askill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(askill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = "0"
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(skillTimeLeft)
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(askill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(askill.Level + 1, 5)))

                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        For group As Integer = 1 To nog
            If CDbl(repGroup(group, 2)) > 0 Then
                strText.AppendLine(repGroup(group, 0) & " (" & repGroup(group, 2) & " Skills)")
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    Dim txtData(2) As String
                    txtData(0) = "  * " & repSkill(group, skill, 1)
                    txtData(1) = "Rank " & repSkill(group, skill, 2)
                    txtData(2) = "TBT " & repSkill(group, skill, 4)
                    strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-25}", txtData))
                Next
                strText.AppendLine()
            End If
        Next

        Return strText.ToString
    End Function

#End Region

#Region "Text Training Queue Report"

    Public Shared Sub GenerateTextTrainQueue(ByVal rPilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue)

        Dim strText As String = ""
        strText &= TextCharacterDetails(rQueue.Name & " Training Queue", rPilot)
        strText &= TextTrainQueue(rPilot, rQueue)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextTrainQueue(ByVal rpilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue) As String
        Dim strText As New StringBuilder
        Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(rpilot, rQueue)
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim txtData(6) As String
        txtData(0) = "Skill Name"
        txtData(1) = "Cur Level"
        txtData(2) = "From Level"
        txtData(3) = "To Level"
        txtData(4) = "% Comp"
        txtData(5) = "Time Remaining"
        txtData(6) = "Date Ended"
        strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-10} {4,-10} {5,-20} {6,-20}", txtData))
        For a As Integer = 0 To 6
            txtData(a) = New String(CChar("-"), txtData(a).Length)
        Next
        strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-10} {4,-10} {5,-20} {6,-20}", txtData))

        For skill As Integer = 0 To arrQueue.Count - 1
            Dim qItem As EveHQ.Core.SortedQueue = CType(arrQueue(skill), SortedQueue)
            Dim skillName As String = qItem.Name
            Dim curLevel As String = qItem.CurLevel
            Dim startLevel As String = qItem.FromLevel
            Dim endLevel As String = qItem.ToLevel
            Dim percent As String = qItem.Percent
            Dim timeToEnd As String = EveHQ.Core.SkillFunctions.TimeToString(CDbl(qItem.TrainTime))
            Dim endTime As String = Format(qItem.DateFinished, "ddd") & " " & FormatDateTime(qItem.DateFinished, DateFormat.GeneralDate)

            txtData(0) = skillName
            txtData(1) = curLevel
            txtData(2) = startLevel
            txtData(3) = endLevel
            txtData(4) = percent
            txtData(5) = timeToEnd
            txtData(6) = endTime
            strText.AppendLine(String.Format("{0,-49} {1,-10} {2,-9} {3,-9} {4,-8} {5,-20} {6,-20}", txtData))
        Next

        Return strText.ToString
    End Function

#End Region

#Region "Text Shopping List Reports"
    Public Shared Sub GenerateTextShoppingList(ByVal rPilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue)

        Dim strText As String = ""
        strText &= TextCharacterDetails(rQueue.Name & " Training Queue", rPilot)
        strText &= TextShoppingList(rPilot, rQueue)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextShoppingList(ByVal rpilot As EveHQ.Core.Pilot, ByVal rQueue As EveHQ.Core.SkillQueue) As String
        Dim strText As New StringBuilder
        Dim arrQueue As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(rpilot, rQueue)
        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim txtData(1) As String
        txtData(0) = "Skill Name"
        txtData(1) = "Skill Cost"
        strText.AppendLine(String.Format("{0,-45} {1,-10}", txtData))
        For a As Integer = 0 To 1
            txtData(a) = New String(CChar("-"), txtData(a).Length)
        Next
        strText.AppendLine(String.Format("{0,-45} {1,-10}", txtData))

        Dim skillPriceList As New ArrayList
        For skill As Integer = 0 To arrQueue.Count - 1
            Dim qItem As EveHQ.Core.SortedQueue = CType(arrQueue(skill), SortedQueue)
            Dim skillName As String = qItem.Name
            If rpilot.PilotSkills.Contains(skillName) = False Then
                If skillPriceList.Contains(skillName) = False Then
                    skillPriceList.Add(skillName)
                End If
            End If
        Next

        Dim totalCost As Double = 0
        For Each skillName As String In skillPriceList
            Dim cSkill As EveHQ.Core.EveSkill = EveHQ.Core.HQ.SkillListName(skillName)
            txtData(0) = skillName
            txtData(1) = FormatNumber(cSkill.BasePrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            strText.AppendLine(String.Format("{0,-45} {1,-10}", txtData))
            totalCost += cSkill.BasePrice
        Next

        strText.AppendLine("")
        txtData(0) = "Total Queue Shopping List Cost:"
        txtData(1) = FormatNumber(totalCost, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        strText.AppendLine(String.Format("{0,-45} {1,-10}", txtData))

        Return strText.ToString
    End Function
#End Region

#Region "Partial Skills Report"
    Public Shared Sub GeneratePartialSkills(ByVal rpilot As EveHQ.Core.Pilot)
        Dim strHTML As String = ""
        strHTML &= HTMLHeader("Partially Trained Skills - " & rpilot.Name)
        strHTML &= HTMLTitle("Partially Trained Skills - " & rpilot.Name)
        strHTML &= HTMLCharacterDetails(rpilot)
        strHTML &= PartialSkills(rpilot)
        strHTML &= HTMLFooter()
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "PartialSkills (" & rpilot.Name & ").html"))
        sw.Write(strHTML)
        sw.Flush()
        sw.Close()
        strHTML = Nothing

        ' Tidy up report variables
        GC.Collect()
    End Sub

    Public Shared Function PartialSkills(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strHTML As String = ""

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            Dim partTrained As Boolean = True
            For level As Integer = 0 To 5
                If curSkill.SP = curSkill.LevelUp(level) Or curSkill.SP = curSkill.LevelUp(level) + 1 Or curSkill.Level = 5 Then
                    partTrained = False
                    Exit For
                End If
            Next
            If partTrained = True Then
                ' Determine if the skill is being trained
                If rpilot.Training = True Then
                    If curSkill.ID = rpilot.TrainingSkillID Then
                        sortSkill(count, 1) = rpilot.TrainingCurrentTime
                    Else
                        sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
                    End If
                Else
                    sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
                End If
                sortSkill(count, 0) = CLng(curSkill.ID)
                count += 1
            End If
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(count - 1) As Integer
        For a As Integer = 0 To count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Training Completed" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 1 hour" : repGroup(2, 1) = "1" : repGroup(2, 2) = "3600"
        repGroup(3, 0) = "1 to 2 hours" : repGroup(3, 1) = "3601" : repGroup(3, 2) = "7200"
        repGroup(4, 0) = "2 to 4 hours" : repGroup(4, 1) = "7201" : repGroup(4, 2) = "14400"
        repGroup(5, 0) = "4 to 6 hours" : repGroup(5, 1) = "14401" : repGroup(5, 2) = "21600"
        repGroup(6, 0) = "6 to 8 hours" : repGroup(6, 1) = "21601" : repGroup(6, 2) = "28800"
        repGroup(7, 0) = "8 to 16 hours" : repGroup(7, 1) = "28801" : repGroup(7, 2) = "57600"
        repGroup(8, 0) = "16 to 24 hours" : repGroup(8, 1) = "57601" : repGroup(8, 2) = "86400"
        repGroup(9, 0) = "1 to 2 days" : repGroup(9, 1) = "86401" : repGroup(9, 2) = "172800"
        repGroup(10, 0) = "2 to 4 days" : repGroup(10, 1) = "172801" : repGroup(10, 2) = "345600"
        repGroup(11, 0) = "4 to 7 days" : repGroup(11, 1) = "345601" : repGroup(11, 2) = "604800"
        repGroup(12, 0) = "7 to 14 days" : repGroup(12, 1) = "604801" : repGroup(12, 2) = "1209600"
        repGroup(13, 0) = "14 to 21 days" : repGroup(13, 1) = "1209601" : repGroup(13, 2) = "1814400"
        repGroup(14, 0) = "21 to 28 days" : repGroup(14, 1) = "1814401" : repGroup(14, 2) = "2419200"
        repGroup(15, 0) = "28 days or more" : repGroup(15, 1) = "2419200" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim cskill As EveHQ.Core.PilotSkill
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                    skillCount += 1
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    If rpilot.Training = True Then
                        If currentSkill.ID = cskill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        Dim imgLevel As String = ""
        strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
        Dim group As Integer = 1
        Do
            group = group + 1
            If group = nog + 1 Then group = 1
            If CDbl(repGroup(group, 2)) > 0 Then
                strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                    If rpilot.Training = True Then
                        If currentSkill.ID = repSkill(group, skill, 0) Then
                            strHTML &= "<td style='color:#FFAA00;'>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & CDbl(repSkill(group, skill, 5)) + 1 & "_act.gif"
                        Else
                            strHTML &= "<td>"
                            imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                        End If
                    Else
                        strHTML &= "<td>"
                        imgLevel = "http://myeve.eve-online.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif"
                    End If
                    strHTML &= "<b>" & repSkill(group, skill, 1) & "</b>&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Rank " & repSkill(group, skill, 2) & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "SP: " & Format(CLng(repSkill(group, skill, 3)), "#,###") & " of " & Format(CLng(repSkill(group, skill, 6)), "#,###") & "&nbsp;&nbsp;/&nbsp;&nbsp;"
                    strHTML &= "Time to Next Level: " & repSkill(group, skill, 4)
                    strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                Next
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td align=right>Time to Clear Group: " & repGroup(group, 4) & "</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
            End If
        Loop Until group = 1
        strHTML &= "</table>"
        strHTML &= "<p></p>"

        Return strHTML
    End Function
#End Region

#Region "Text Partial Skills Report"
    Public Shared Sub GenerateTextPartialSkills(ByVal rPilot As EveHQ.Core.Pilot)

        Dim strText As String = ""
        strText &= TextCharacterDetails("Partially Trained Skills", rPilot)
        strText &= TextPartialSkills(rPilot)
        Dim sw As StreamWriter = New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").txt"))
        sw.Write(strText)
        sw.Flush()
        sw.Close()
        strText = Nothing

        ' Tidy up report variables
        GC.Collect()

    End Sub

    Public Shared Function TextPartialSkills(ByVal rpilot As EveHQ.Core.Pilot) As String
        Dim strText As New StringBuilder

        Dim currentSkill As EveHQ.Core.PilotSkill = New EveHQ.Core.PilotSkill
        Dim currentSP As String = ""
        Dim currentTime As String = ""
        If rpilot.Training = True Then
            currentSkill = CType(rpilot.PilotSkills.Item(EveHQ.Core.SkillFunctions.SkillIDToName(rpilot.TrainingSkillID)), EveHQ.Core.PilotSkill)
            currentSP = CStr(rpilot.TrainingCurrentSP)
            currentTime = EveHQ.Core.SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
        End If

        Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
        Dim curSkill As EveHQ.Core.PilotSkill
        Dim count As Integer
        Dim redTime As Long = 0
        For Each curSkill In rpilot.PilotSkills
            Dim partTrained As Boolean = True
            For level As Integer = 0 To 5
                If curSkill.SP = curSkill.LevelUp(level) Or curSkill.SP = curSkill.LevelUp(level) + 1 Or curSkill.Level = 5 Then
                    partTrained = False
                    Exit For
                End If
            Next
            If partTrained = True Then
                ' Determine if the skill is being trained
                If rpilot.Training = True Then
                    If curSkill.ID = rpilot.TrainingSkillID Then
                        sortSkill(count, 1) = rpilot.TrainingCurrentTime
                    Else
                        sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
                    End If
                Else
                    sortSkill(count, 1) = CLng(Math.Max(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(curSkill.Name), 0) - redTime, 0))
                End If
                sortSkill(count, 0) = CLng(curSkill.ID)
                count += 1
            End If
        Next

        ' Create a tag array ready to sort the skill times
        Dim tagArray(count - 1) As Integer
        For a As Integer = 0 To count - 1
            tagArray(a) = a
        Next

        ' Initialize the comparer and sort
        Dim myComparer As New RectangularComparer(sortSkill)
        Array.Sort(tagArray, myComparer)

        ' Define the groups
        Dim nog As Integer = 15             ' Number of groups to break the report into
        Dim repGroup(nog, 4) As String      ' Name, Min, Max, skillcount, SPs
        repGroup(1, 0) = "Training Completed" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
        repGroup(2, 0) = "Upto 1 hour" : repGroup(2, 1) = "1" : repGroup(2, 2) = "3600"
        repGroup(3, 0) = "1 to 2 hours" : repGroup(3, 1) = "3601" : repGroup(3, 2) = "7200"
        repGroup(4, 0) = "2 to 4 hours" : repGroup(4, 1) = "7201" : repGroup(4, 2) = "14400"
        repGroup(5, 0) = "4 to 6 hours" : repGroup(5, 1) = "14401" : repGroup(5, 2) = "21600"
        repGroup(6, 0) = "6 to 8 hours" : repGroup(6, 1) = "21601" : repGroup(6, 2) = "28800"
        repGroup(7, 0) = "8 to 16 hours" : repGroup(7, 1) = "28801" : repGroup(7, 2) = "57600"
        repGroup(8, 0) = "16 to 24 hours" : repGroup(8, 1) = "57601" : repGroup(8, 2) = "86400"
        repGroup(9, 0) = "1 to 2 days" : repGroup(9, 1) = "86401" : repGroup(9, 2) = "172800"
        repGroup(10, 0) = "2 to 4 days" : repGroup(10, 1) = "172801" : repGroup(10, 2) = "345600"
        repGroup(11, 0) = "4 to 7 days" : repGroup(11, 1) = "345601" : repGroup(11, 2) = "604800"
        repGroup(12, 0) = "7 to 14 days" : repGroup(12, 1) = "604801" : repGroup(12, 2) = "1209600"
        repGroup(13, 0) = "14 to 21 days" : repGroup(13, 1) = "1209601" : repGroup(13, 2) = "1814400"
        repGroup(14, 0) = "21 to 28 days" : repGroup(14, 1) = "1814401" : repGroup(14, 2) = "2419200"
        repGroup(15, 0) = "28 days or more" : repGroup(15, 1) = "2419200" : repGroup(15, 2) = "9999999999"

        Dim repSkill(EveHQ.Core.HQ.SkillGroups.Count, EveHQ.Core.HQ.SkillListID.Count, 6) As String
        Dim groupCount As Integer = 0
        Dim skillTimeLeft As Long = 0
        For groupCount = 1 To nog
            Dim skillCount As Long = 0
            Dim SPCount As Long = 0
            Dim TotalTime As Double = 0
            Dim i As Integer
            For i = 0 To tagArray.Length - 1
                skillTimeLeft = sortSkill(tagArray(i), 1)
                If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                    Dim cskill As EveHQ.Core.PilotSkill
                    Dim askill As EveHQ.Core.EveSkill
                    askill = EveHQ.Core.HQ.SkillListID(CStr(sortSkill(tagArray(i), 0)))
                    cskill = CType(rpilot.PilotSkills(askill.Name), EveHQ.Core.PilotSkill)
                    skillCount += 1
                    TotalTime += EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0)
                    SPCount += cskill.SP
                    repSkill(groupCount, CInt(skillCount), 0) = cskill.ID
                    repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                    repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                    repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                    repSkill(groupCount, CInt(skillCount), 4) = EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.SkillFunctions.CalcTimeToLevel(rpilot, EveHQ.Core.HQ.SkillListName(cskill.Name), 0))
                    repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                    repSkill(groupCount, CInt(skillCount), 6) = CStr(cskill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    If rpilot.Training = True Then
                        If currentSkill.ID = cskill.ID Then
                            repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                            repSkill(groupCount, CInt(skillCount), 4) = currentTime
                            SPCount += CLng(currentSP)
                        End If
                    End If
                End If
            Next
            repGroup(groupCount, 2) = CStr(skillCount)
            repGroup(groupCount, 3) = CStr(SPCount)
            repGroup(groupCount, 4) = EveHQ.Core.SkillFunctions.TimeToString(TotalTime)
        Next

        Dim group As Integer = 1
        Do
            group = group + 1
            If group = nog + 1 Then group = 1
            If CDbl(repGroup(group, 2)) > 0 Then
                strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                For skill As Integer = 1 To CInt(repGroup(group, 2))
                    Dim txtData(4) As String
                    txtData(0) = "  * " & repSkill(group, skill, 1)
                    txtData(1) = "Rank " & repSkill(group, skill, 2)
                    txtData(2) = "Level " & repSkill(group, skill, 5)
                    txtData(3) = "SP " & FormatNumber(repSkill(group, skill, 3), 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                    txtData(4) = "TTNL " & repSkill(group, skill, 4)
                    strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-15} {4,-25}", txtData))
                Next
                strText.AppendLine(String.Format("{0,-45} {1,-40}", "", "Time to Clear Group: " & repGroup(group, 4)))
                strText.AppendLine()
            End If
        Loop Until group = 1

        Return strText.ToString
    End Function
#End Region

#Region "ECM Export"
    Public Shared Sub GenerateECMExportReports(ByVal rpilot As EveHQ.Core.Pilot)
        Dim ECMLocation As String = ""
        Dim result As Integer = 0
        Dim fbd1 As New FolderBrowserDialog
        With fbd1
            .ShowNewFolderButton = False
            If EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation <> "" Then
                If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation) = True Then
                    .Description = "Please select the folder where the ECM XML files are located..." & ControlChars.CrLf & "Default is: " & EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation
                    .SelectedPath = EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation
                Else
                    .Description = "Please select the folder where the ECM XML files are located..."
                    .RootFolder = Environment.SpecialFolder.Desktop
                End If
            Else
                .Description = "Please select the folder where the ECM XML files are located..."
                .RootFolder = Environment.SpecialFolder.Desktop
            End If
            result = .ShowDialog()
            ECMLocation = .SelectedPath
        End With

        If ECMLocation <> "" And result = 1 Then

            ' Generate the Old Style XML Report
            Call EveHQ.Core.Reports.GenerateCurrentPilotXML_Old(rpilot)
            ' Generate the Old Style Training XML
            Call EveHQ.Core.Reports.GenerateCurrentTrainingXML_Old(rpilot)

            ' Copy these to the selected folder
            My.Computer.FileSystem.CopyFile(Path.Combine(EveHQ.Core.HQ.reportFolder, "CurrentXML - Old (" & rpilot.Name & ").xml"), Path.Combine(ECMLocation, rpilot.ID.ToString & ".xml"), True)
            My.Computer.FileSystem.CopyFile(Path.Combine(EveHQ.Core.HQ.reportFolder, "TrainingXML - Old (" & rpilot.Name & ").xml"), Path.Combine(ECMLocation, rpilot.ID.ToString & ".training.xml"), True)
            EveHQ.Core.HQ.EveHQSettings.ECMDefaultLocation = ECMLocation
            MessageBox.Show("Export of ECM-compatible files completed!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else

            MessageBox.Show("Export of ECM-compatible aborted!", "Export Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

End Class
