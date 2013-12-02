' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports EveHQ.EveAPI
Imports EveHQ.EveData
Imports System.Globalization
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Windows.Forms

Namespace CoreReports

    Public Class Reports
        Private Const WWW As String = "http://www.evehq.net"

#Region "Common Routines"

        Public Shared Function HTMLCharacterDetails(ByVal rpilot As EveHQPilot) As String

            Dim strHTML As String = ""
            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = "0"
            Dim currentTime As String = "0"
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(SkillFunctions.CalcCurrentSkillPoints(rpilot))
                currentTime = SkillFunctions.TimeToString(SkillFunctions.CalcCurrentSkillTime(rpilot))
            End If
            strHTML &= "<table width=800px align=center border=0>"
            strHTML &= "<tr><td width=150px align=center valign=middle><img src='http://image.eveonline.com/Character/" & rpilot.ID & "_256.jpg' height=128 width=128 alt='" & rpilot.Name & "'></td>"
            strHTML &= "<td width=5px></td>"
            strHTML &= "<td width=350px>"
            strHTML &= "<table width=100%><tr><td class=thead align=center valign=middle colspan=2><b>Character Info</b></td></tr>"
            strHTML &= "<tr><td>Character Name</td><td>" & rpilot.Name & "</td></tr>"
            strHTML &= "<tr><td>Corporation</td><td>" & rpilot.Corp & "</td></tr>"
            strHTML &= "<tr><td>Total Cash</td><td>" & Format(CDbl(rpilot.Isk), "Standard") & "</td></tr>"
            strHTML &= "<tr><td>Total Skillpoints</td><td>" & (CLng(rpilot.SkillPoints) + CLng(currentSP)).ToString("N0") & " (in " & rpilot.PilotSkills.Count & " skills)</td></tr>"
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
            strHTML &= "<tr><td>Intelligence</td><td>" & Format(CDbl(rpilot.IntAttT), "##.00") & "</td></tr>"
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
            strHTML &= "<html lang=""" & CultureInfo.CurrentCulture.ToString & """>"
            strHTML &= "<head>"
            strHTML &= "<META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">"
            strHTML &= "<title>" & browserHeader & "</title>" & CharacterCSS() & "</head>"
            strHTML &= "<body>"
            Return strHTML
        End Function

        Public Shared Function HTMLTitle(ByVal title As String) As String
            Dim strHTML As String = ""
            strHTML &= "<table width=800px border=0 align=center>"
            strHTML &= "<tr height=30px><td><p class=title>" & title & "</p></td></tr>"
            strHTML &= "</table>"
            strHTML &= "<p></p>"
            Return strHTML
        End Function

        Public Shared Function HTMLFooter() As String
            Dim strHTML As String = ""
            strHTML &= "<table width=800px align=center border=0><hr>"
            strHTML &= "<tr><td><p align=center class=footer>Generated on " & Format(Now, "dd/MM/yyyy HH:mm:ss") & " by <a href='" & WWW & "'>" & My.Application.Info.ProductName & "</a> v" & My.Application.Info.Version.ToString & "</p></td></tr>"
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
            strCSS &= "#wrapper {overflow: auto; height: 100%; width:820px; margin-left:auto; margin-right:auto;}"
            strCSS &= "--></STYLE>"
            Return strCSS
        End Function

        Private Shared Function NonCharHTMLHeader(ByVal pageTitle As String) As String
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

        Public Shared Function TextCharacterDetails(ByVal reportTitle As String, ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder
            Dim tmpText As String
            Dim currentSP As String = "0"
            If rpilot.Training = True Then
                currentSP = CStr(SkillFunctions.CalcCurrentSkillPoints(rpilot))
                SkillFunctions.TimeToString(SkillFunctions.CalcCurrentSkillTime(rpilot))
            End If

            tmpText = reportTitle & " - " & rpilot.Name
            strText.AppendLine(tmpText)
            strText.AppendLine(New String(CChar("="), tmpText.Length))
            strText.AppendLine()
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Name:", rpilot.Name))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Race:", rpilot.Race))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Bloodline:", rpilot.Blood))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Corporation:", rpilot.Corp))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Isk:", rpilot.Isk.ToString("N2")))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Skillpoints:", (CLng(rpilot.SkillPoints) + CLng(currentSP)).ToString("N0")))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Skills:", rpilot.PilotSkills.Count))
            strText.AppendLine()
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Charisma:", Format(CDbl(rpilot.CAttT), "##.00")))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Intelligence:", Format(CDbl(rpilot.IntAttT), "##.00")))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Memory:", Format(CDbl(rpilot.MAttT), "##.00")))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Perception:", Format(CDbl(rpilot.PAttT), "##.00")))
            strText.AppendLine(String.Format("{0,16} {1,-18}", "Willpower:", Format(CDbl(rpilot.WAttT), "##.00")))
            strText.AppendLine()
            strText.AppendLine(New String(CChar("-"), 50))
            strText.AppendLine()

            Return strText.ToString
        End Function

        Public Shared Function PHPBBCharacterDetails(ByVal reportTitle As String, ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder
            Dim tmpText As String
            Dim currentSP As String = "0"
            If rpilot.Training = True Then
                currentSP = CStr(SkillFunctions.CalcCurrentSkillPoints(rpilot))
                SkillFunctions.TimeToString(SkillFunctions.CalcCurrentSkillTime(rpilot))
            End If

            tmpText = reportTitle & " - " & rpilot.Name
            strText.AppendLine(tmpText)
            strText.AppendLine(New String(CChar("="), tmpText.Length))
            strText.AppendLine()
            strText.AppendLine("Name: " & rpilot.Name)
            strText.AppendLine("Race: " & rpilot.Race)
            strText.AppendLine("Bloodline: " & rpilot.Blood)
            strText.AppendLine("Corporation: " & rpilot.Corp)
            strText.AppendLine("Isk: " & rpilot.Isk.ToString("N2"))
            strText.AppendLine("Skillpoints: " & (CLng(rpilot.SkillPoints) + CLng(currentSP)).ToString("N0"))
            strText.AppendLine("Skills: " & rpilot.PilotSkills.Count)
            strText.AppendLine()
            strText.AppendLine("Charisma: " & Format(CDbl(rpilot.CAttT), "##.00"))
            strText.AppendLine("Intelligence: " & Format(CDbl(rpilot.IntAttT), "##.00"))
            strText.AppendLine("Memory: " & Format(CDbl(rpilot.MAttT), "##.00"))
            strText.AppendLine("Perception: " & Format(CDbl(rpilot.PAttT), "##.00"))
            strText.AppendLine("Willpower: " & Format(CDbl(rpilot.WAttT), "##.00"))
            strText.AppendLine()
            strText.AppendLine(New String(CChar("-"), 50))
            strText.AppendLine()

            Return strText.ToString
        End Function


        Public Shared Function XMLImplantDetails(ByVal rpilot As EveHQPilot) As String
            Dim strXML As String = ""
            Dim tabs(10) As String
            For tab As Integer = 1 To 10
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                For tab2 As Integer = 1 To tab
                    tabs(tab) &= Chr(9)
                Next
            Next
            Dim implants(4, 1) As String
            implants(0, 0) = "memory" : implants(0, 1) = "Memory Augmentation"
            implants(1, 0) = "willpower" : implants(1, 1) = "Neural Boost"
            implants(2, 0) = "perception" : implants(2, 1) = "Ocular Filter"
            implants(3, 0) = "intelligence" : implants(3, 1) = "Cybernetic Subprocessor"
            implants(4, 0) = "charisma" : implants(4, 1) = "Social Adaptation Chip"
            Dim implantStyles(6, 1) As String
            implantStyles(0, 0) = "Limited " : implantStyles(0, 1) = ""
            implantStyles(1, 0) = "Limited " : implantStyles(1, 1) = " - Beta"
            implantStyles(2, 0) = "" : implantStyles(2, 1) = " - Basic"
            implantStyles(3, 0) = "" : implantStyles(3, 1) = " - Standard"
            implantStyles(4, 0) = "" : implantStyles(4, 1) = " - Improved"
            implantStyles(5, 0) = "" : implantStyles(5, 1) = " - Advanced"
            implantStyles(6, 0) = "" : implantStyles(6, 1) = " - Elite"
            strXML &= tabs(2) & "<attributeEnhancers>" & vbCrLf
            If rpilot.MImplantA > 0 Then
                strXML &= AddImplantToXML(rpilot.MImplantA, implants(0, 0), implants(0, 1), implantStyles(rpilot.MImplantA - 1, 0), implantStyles(rpilot.MImplantA - 1, 1))
            End If
            If rpilot.WImplantA > 0 Then
                strXML &= AddImplantToXML(rpilot.WImplantA, implants(1, 0), implants(1, 1), implantStyles(rpilot.WImplantA - 1, 0), implantStyles(rpilot.WImplantA - 1, 1))
            End If
            If rpilot.PImplantA > 0 Then
                strXML &= AddImplantToXML(rpilot.PImplantA, implants(2, 0), implants(2, 1), implantStyles(rpilot.PImplantA - 1, 0), implantStyles(rpilot.PImplantA - 1, 1))
            End If
            If rpilot.IntImplantA > 0 Then
                strXML &= AddImplantToXML(rpilot.IntImplantA, implants(3, 0), implants(3, 1), implantStyles(rpilot.IntImplantA - 1, 0), implantStyles(rpilot.IntImplantA - 1, 1))
            End If
            If rpilot.CImplantA > 0 Then
                strXML &= AddImplantToXML(rpilot.CImplantA, implants(4, 0), implants(4, 1), implantStyles(rpilot.CImplantA - 1, 0), implantStyles(rpilot.CImplantA - 1, 1))
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

        Public Shared Sub GenerateCharSheet(ByVal rpilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Character Sheet - " & rpilot.Name)
            strHTML &= HTMLTitle("Character Sheet - " & rpilot.Name)
            strHTML &= HTMLCharacterDetails(rpilot)
            strHTML &= CharacterSheet(rpilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "CharSheet (" & rpilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function CharacterSheet(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim repGroup(HQ.SkillGroups.Count, 3) As String
            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 5) As String
            Dim cGroup As SkillGroup
            Dim cSkill As EveHQPilotSkill
            Dim groupCount As Integer = 0
            For Each cGroup In HQ.SkillGroups.Values
                groupCount += 1
                repGroup(groupCount, 1) = cGroup.Name
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                ' Collect skills
                Dim repSkills As New SortedList(Of String, EveHQPilotSkill)
                For Each cSkill In rpilot.PilotSkills.Values
                    repSkills.Add(cSkill.Name, cSkill)
                Next
                For Each cSkill In repSkills.Values
                    If cSkill.GroupID = cGroup.ID Then
                        skillCount += 1
                        spCount += cSkill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cSkill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cSkill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                        If rpilot.Training = True Then
                            If currentSkill.ID = cSkill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
            Next

            Dim imgLevel As String
            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            For group As Integer = 1 To HQ.SkillGroups.Count
                If CDbl(repGroup(group, 2)) > 0 Then
                    strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 1) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
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
        Public Shared Sub GenerateSkillLevels(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Skill Levels - " & rPilot.Name)
            strHTML &= HTMLTitle("Skill Levels - " & rPilot.Name)
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= SkillLevels(rPilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function SkillLevels(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As String
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            For Each curSkill In rpilot.PilotSkills.Values
                sortSkill(count, 0) = CStr(curSkill.ID)
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
            Const Nog As Integer = 6              ' Number of groups to break the report into
            Dim repGroup(Nog, 5) As String      ' Name, Min, Max, skillcount, SPs
            repGroup(1, 0) = "Level 0" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
            repGroup(2, 0) = "Level 1" : repGroup(2, 1) = "1" : repGroup(2, 2) = "1"
            repGroup(3, 0) = "Level 2" : repGroup(3, 1) = "2" : repGroup(3, 2) = "2"
            repGroup(4, 0) = "Level 3" : repGroup(4, 1) = "3" : repGroup(4, 2) = "3"
            repGroup(5, 0) = "Level 4" : repGroup(5, 1) = "4" : repGroup(5, 2) = "4"
            repGroup(6, 0) = "Level 5" : repGroup(6, 1) = "5" : repGroup(6, 2) = "5"

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    Dim cskill As EveHQPilotSkill
                    Dim askill As EveSkill
                    askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                    cskill = rpilot.PilotSkills(askill.Name)
                    If cskill.Level >= CDbl(repGroup(groupCount, 1)) And cskill.Level <= CDbl(repGroup(groupCount, 2)) Then
                        skillCount += 1
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        Dim curTime As Double = SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0)
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(curTime)
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim imgLevel As String
            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            For group As Integer = 1 To Nog
                If CDbl(repGroup(group, 2)) > 0 Then
                    strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
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

#Region "Skill Ranks Report"

        Public Shared Sub GenerateSkillRanks(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Skill Ranks - " & rPilot.Name)
            strHTML &= HTMLTitle("Skill Ranks - " & rPilot.Name)
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= SkillRanks(rPilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillRanks (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function SkillRanks(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As String
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            For Each curSkill In rpilot.PilotSkills.Values
                sortSkill(count, 0) = CStr(curSkill.ID)
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
            Const Nog As Integer = 16              ' Number of groups to break the report into
            Dim repGroup(Nog, 5) As String      ' Name, Min, Max, skillcount, SPs
            repGroup(1, 0) = "Rank 1" : repGroup(1, 1) = "1" : repGroup(1, 2) = "1"
            repGroup(2, 0) = "Rank 2" : repGroup(2, 1) = "2" : repGroup(2, 2) = "2"
            repGroup(3, 0) = "Rank 3" : repGroup(3, 1) = "3" : repGroup(3, 2) = "3"
            repGroup(4, 0) = "Rank 4" : repGroup(4, 1) = "4" : repGroup(4, 2) = "4"
            repGroup(5, 0) = "Rank 5" : repGroup(5, 1) = "5" : repGroup(5, 2) = "5"
            repGroup(6, 0) = "Rank 6" : repGroup(6, 1) = "6" : repGroup(6, 2) = "6"
            repGroup(7, 0) = "Rank 7" : repGroup(7, 1) = "7" : repGroup(7, 2) = "7"
            repGroup(8, 0) = "Rank 8" : repGroup(8, 1) = "8" : repGroup(8, 2) = "8"
            repGroup(9, 0) = "Rank 9" : repGroup(9, 1) = "9" : repGroup(9, 2) = "9"
            repGroup(10, 0) = "Rank 10" : repGroup(10, 1) = "10" : repGroup(10, 2) = "10"
            repGroup(11, 0) = "Rank 11" : repGroup(11, 1) = "11" : repGroup(11, 2) = "11"
            repGroup(12, 0) = "Rank 12" : repGroup(12, 1) = "12" : repGroup(12, 2) = "12"
            repGroup(13, 0) = "Rank 13" : repGroup(13, 1) = "13" : repGroup(13, 2) = "13"
            repGroup(14, 0) = "Rank 14" : repGroup(14, 1) = "14" : repGroup(14, 2) = "14"
            repGroup(15, 0) = "Rank 15" : repGroup(15, 1) = "15" : repGroup(15, 2) = "15"
            repGroup(16, 0) = "Rank 16" : repGroup(16, 1) = "16" : repGroup(16, 2) = "16"

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    Dim cskill As EveHQPilotSkill
                    Dim askill As EveSkill
                    askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                    cskill = rpilot.PilotSkills(askill.Name)
                    If cskill.Rank >= CDbl(repGroup(groupCount, 1)) And cskill.Rank <= CDbl(repGroup(groupCount, 2)) Then
                        skillCount += 1
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        Dim curTime As Double = SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5)
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(curTime)
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim imgLevel As String
            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            For group As Integer = 1 To Nog
                If CDbl(repGroup(group, 2)) > 0 Then
                    strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
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
                        strHTML &= "Time to Level 5: " & repSkill(group, skill, 4)
                        strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                    Next
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td align=right>Time to Train Group to Level 5: " & repGroup(group, 4) & "</td><td width=100px></td></tr>"
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
        Public Shared Sub GenerateTrainingTime(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Training Times - " & rPilot.Name)
            strHTML &= HTMLTitle("Training Times - " & rPilot.Name)
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= TrainingTime(rPilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "TrainTime (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TrainingTime(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            Const RedTime As Long = 0
            For Each curSkill In rpilot.PilotSkills.Values
                ' Determine if the skill is being trained
                If rpilot.Training = True Then
                    If curSkill.ID = rpilot.TrainingSkillID Then
                        sortSkill(count, 1) = rpilot.TrainingCurrentTime
                    Else
                        sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
                    End If
                Else
                    sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim cskill As EveHQPilotSkill
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        cskill = rpilot.PilotSkills(askill.Name)
                        skillCount += 1
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0)
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim imgLevel As String
            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            Dim group As Integer = 1
            Do
                group = group + 1
                If group = Nog + 1 Then group = 1
                If CDbl(repGroup(group, 2)) > 0 Then
                    strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
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
        Public Shared Sub GenerateSkillsNotTrained(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Skills Not Trained - " & rPilot.Name)
            strHTML &= HTMLTitle("Skills Not Trained - " & rPilot.Name)
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= SkillsNotTrained(rPilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()
            ' Tidy up report variables
            GC.Collect()
        End Sub

        Public Shared Function SkillsNotTrained(ByVal rPilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            If rPilot.Training = True Then
                currentSkill = rPilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rPilot.TrainingSkillID))
                SkillFunctions.TimeToString(rPilot.TrainingCurrentTime)
            End If

            Dim sortSkill(HQ.SkillListID.Count, 1) As Long
            Dim curSkill As EveSkill
            Dim count As Integer
            For Each curSkill In HQ.SkillListID.Values
                If curSkill.Published = True Then
                    If rPilot.PilotSkills.ContainsKey(curSkill.Name) = False Then
                        ' Determine if the skill is being trained
                        sortSkill(count, 1) = CLng(SkillFunctions.TimeBeforeCanTrain(rPilot, curSkill.ID))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        skillCount += 1
                        totalTime += skillTimeLeft
                        spCount += askill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(askill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = askill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(askill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = "0"
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(skillTimeLeft)
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(askill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(askill.Level + 1, 5)))

                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim imgLevel As String
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
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
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
            Loop Until group = Nog
            strHTML &= "</table>"
            strHTML &= "<p></p>"

            Return strHTML
        End Function

#End Region

#Region "Time to Level 5 Report"
        Public Shared Sub GenerateTimeToLevel5(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Time To Level 5 - " & rPilot.Name)
            strHTML &= HTMLTitle("Time To Level 5 - " & rPilot.Name)
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= TimeToLevel5(rPilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TimeToLevel5(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            Const RedTime As Long = 0
            For Each curSkill In rpilot.PilotSkills.Values
                ' Determine if the skill is being trained
                If rpilot.Training = True Then
                    sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 5) - RedTime, 0))
                Else
                    sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 5) - RedTime, 0))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim cskill As EveHQPilotSkill
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        cskill = rpilot.PilotSkills(askill.Name)
                        skillCount += 1
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5)
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim imgLevel As String
            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            Dim group As Integer = 1
            Do
                group = group + 1
                If group = Nog + 1 Then group = 1
                If CDbl(repGroup(group, 2)) > 0 Then
                    strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
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

        Public Shared Sub GenerateCharXML(ByVal rPilot As EveHQPilot)
            Dim cXML As New XmlDocument
            cXML.Load(Path.Combine(HQ.CacheFolder, "EveHQAPI_" & APITypes.CharacterSheet.ToString & "_" & rPilot.Account & "_" & rPilot.ID & ".xml"))
            cXML.Save(Path.Combine(HQ.reportFolder, "CharXML (" & rPilot.Name & ").xml"))
        End Sub

        Public Shared Sub GenerateTrainXML(ByVal rPilot As EveHQPilot)
            Dim tXML As New XmlDocument
            tXML.Load(Path.Combine(HQ.CacheFolder, "EveHQAPI_" & APITypes.SkillQueue.ToString & "_" & rPilot.Account & "_" & rPilot.ID & ".xml"))
            tXML.Save(Path.Combine(HQ.reportFolder, "TrainingXML (" & rPilot.Name & ").xml"))
        End Sub

#End Region

#Region "Training Queue Report"

        Public Shared Sub GenerateTrainQueue(ByVal rPilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            strHTML &= HTMLTitle("Training Queue - " & rPilot.Name & " (" & rQueue.Name & ")")
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= TrainQueue(rPilot, rQueue)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TrainQueue(ByVal rpilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue) As String
            Dim strHTML As String = ""
            Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(rpilot, rQueue, False, True)

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
                Dim qItem As SortedQueueItem = CType(arrQueue(skill), SortedQueueItem)
                Dim skillName As String = qItem.Name
                Dim curLevel As Integer = qItem.CurLevel
                Dim startLevel As Integer = qItem.FromLevel
                Dim endLevel As Integer = qItem.ToLevel
                Dim percent As Double = qItem.Percent
                Dim timeToEnd As String = SkillFunctions.TimeToString(CDbl(qItem.TrainTime))
                Dim endTime As String = Format(qItem.DateFinished, "ddd") & " " & qItem.DateFinished.ToString

                strHTML &= "<tr height=20px>"
                strHTML &= "<td>" & skillName & "</td>"
                strHTML &= "<td align=center>" & curLevel.ToString & "</td>"
                strHTML &= "<td align=center>" & startLevel.ToString & "</td>"
                strHTML &= "<td align=center>" & endLevel.ToString & "</td>"
                strHTML &= "<td align=center>" & percent.ToString("N0") & "</td>"
                strHTML &= "<td>" & timeToEnd & "</td>"
                strHTML &= "<td>" & endTime & "</td>"
                strHTML &= "</tr>"
            Next

            strHTML &= "</table><p></p>"

            Return strHTML
        End Function

#End Region

#Region "Shopping List Reports"
        Public Shared Sub GenerateShoppingList(ByVal rPilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            strHTML &= HTMLTitle("Shopping List - " & rPilot.Name & " (" & rQueue.Name & ")")
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= ShoppingList(rPilot, rQueue)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function ShoppingList(ByVal rpilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue) As String
            Dim strHTML As String = ""
            Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(rpilot, rQueue, False, True)

            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            strHTML &= "<tr>"
            strHTML &= "<td class=thead width=350px>Skill Name</td>"
            strHTML &= "<td class=thead align=left>Market Price (est)</td>"
            strHTML &= "</tr>"

            Dim skillPriceList As New ArrayList
            For skill As Integer = 0 To arrQueue.Count - 1
                Dim qItem As SortedQueueItem = CType(arrQueue(skill), SortedQueueItem)
                Dim skillName As String = qItem.Name
                If rpilot.PilotSkills.ContainsKey(skillName) = False Then
                    If skillPriceList.Contains(skillName) = False Then
                        skillPriceList.Add(skillName)
                    End If
                End If
            Next

            Dim totalCost As Double = 0
            For Each skillName As String In skillPriceList
                Dim cSkill As EveSkill = HQ.SkillListName(skillName)
                strHTML &= "<tr height=20px>"
                strHTML &= "<td width=350px>" & skillName & "</td>"
                strHTML &= "<td align=left>" & cSkill.BasePrice.ToString("N2") & "</td>"
                strHTML &= "</tr>"
                totalCost += cSkill.BasePrice
            Next

            strHTML &= "<tr height=20px><td>&nbsp;</td><td></td></tr>"
            strHTML &= "<tr height=20px><td width=350px>Total Queue Shopping List Cost:</td><td>" & totalCost.ToString("N2") & "</td></tr>"
            strHTML &= "</table><p></p>"

            Return strHTML
        End Function
#End Region

#Region "Skills Available Report"

        Public Shared Sub GenerateSkillsAvailable(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Skills Available To Train - " & rPilot.Name)
            strHTML &= HTMLTitle("Skills Available To Train - " & rPilot.Name)
            strHTML &= HTMLCharacterDetails(rPilot)
            strHTML &= SkillsAvailable(rPilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function SkillsAvailable(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            strHTML &= "<table width=600px align=center cellspacing=0 cellpadding=0>"
            strHTML &= "<tr>"
            strHTML &= "<td class=thead width=250px>Skill Name</td>"
            strHTML &= "<td class=thead width=250px>Skill Group</td>"
            strHTML &= "<td class=thead width=100px>Skill Rank</td>"
            strHTML &= "</tr>"

            Dim trainable As Boolean
            For Each skill As EveSkill In HQ.SkillListName.Values
                trainable = False
                If rpilot.PilotSkills.ContainsKey(skill.Name) = False And skill.Published = True Then
                    trainable = True
                    For Each preReq As Integer In skill.PreReqSkills.Keys
                        If skill.PreReqSkills(preReq) <> 0 Then
                            Dim ps As EveSkill = HQ.SkillListID(preReq)
                            If rpilot.PilotSkills.ContainsKey(ps.Name) = True Then
                                Dim psp As EveHQPilotSkill = rpilot.PilotSkills(ps.Name)
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
                    strHTML &= "<td>" & StaticData.TypeGroups(CInt(skill.GroupID)) & "</td>"
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

            Private ReadOnly _sortArray(,) As Long

            ' constructor initializes the sortArray reference
            Public Sub New(ByVal theArray(,) As Long)
                _sortArray = theArray
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                ' x and y are integer row numbers into the sortArray
                Dim i1 As Long = DirectCast(x, Integer)
                Dim i2 As Long = DirectCast(y, Integer)

                ' compare the items in the sortArray
                Return _sortArray(CInt(i1), 1).CompareTo(_sortArray(CInt(i2), 1))
            End Function
        End Class

        Class ArrayComparerString
            Implements IComparer
            ' maintain a reference to the 2-dimensional array being sorted

            Private ReadOnly _sortArray(,) As String

            ' constructor initializes the sortArray reference
            Public Sub New(ByVal theArray(,) As String)
                _sortArray = theArray
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
                ' x and y are integer row numbers into the sortArray
                Dim i1 As String = CStr(DirectCast(x, Integer))
                Dim i2 As String = CStr(DirectCast(y, Integer))

                ' compare the items in the sortArray
                Return String.Compare(_sortArray(CInt(i1), 1), _sortArray(CInt(i2), 1), StringComparison.Ordinal)
            End Function
        End Class

#End Region

#Region "Character Summary Report"
        Public Shared Sub GenerateCharSummary()

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Pilot Summary")
            strHTML &= HTMLTitle("Pilot Summary")
            strHTML &= CharSummary()
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "PilotSummary.html"))

            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub
        Public Shared Function CharSummary() As String
            Dim strHTML As String = ""
            Dim currentTime As String
            Dim totalIsk As Double = 0

            Dim rPilot As EveHQPilot
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
            For Each rPilot In HQ.Settings.Pilots.Values
                If rPilot.Active = True Then
                    sortedPilots.Add(rPilot.Name, rPilot)
                End If
            Next
            For Each rPilot In sortedPilots.Values
                If rPilot.Active = True Then
                    strHTML &= "<tr height=75px>"
                    strHTML &= "<td width=70px><img src='http://image.eveonline.com/Character/" & rPilot.ID & "_256.jpg' style='border:0px;width:64px;height:64px;' alt='" & rPilot.Name & "'></td>"
                    strHTML &= "<td width=165px>" & rPilot.Name & "</td>"
                    strHTML &= "<td width=165px>" & rPilot.Corp & "</td>"
                    strHTML &= "<td width=125px align=right>" & rPilot.Isk.ToString("N2") & "</td>"
                    totalIsk += rPilot.Isk
                    If rPilot.Training = True Then
                        Dim skillname As String = SkillFunctions.SkillIDToName(rPilot.TrainingSkillID)
                        currentTime = SkillFunctions.TimeToString(SkillFunctions.CalcCurrentSkillTime(rPilot))
                        strHTML &= "<td width=125px align=right>" & (rPilot.SkillPoints + rPilot.TrainingCurrentSP).ToString("N0") & "</td>"
                        strHTML &= "<td width=200px align=right>" & skillname & " " & SkillFunctions.Roman(rPilot.TrainingSkillLevel) & "<br>" & currentTime & "</td>"
                    Else
                        strHTML &= "<td width=125px align=right>" & rPilot.SkillPoints.ToString("N0") & "</td>"
                        strHTML &= "<td width=200px align=right>n/a</td>"
                    End If
                    strHTML &= "</tr>"
                End If
            Next
            ' Write the total wealth line
            strHTML &= "<tr height=75px><td colspan=2></td><td>Total Wealth:</td><td align=right>" & totalIsk.ToString("N2") & "</td><td colspan=2></tr>"
            strHTML &= "</table><p></p>"

            Return strHTML

        End Function
#End Region

#Region "Skill Point Report"
        Public Shared Sub GenerateSPSummary()
            Dim strHTML As String = ""
            strHTML &= NonCharHTMLHeader("Skill Level Table")
            strHTML &= SPSummary()
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SPSummary.html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

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
                    strHTML &= "<td align=center>" & Math.Ceiling(SkillFunctions.CalculateSPLevel(rank, level)).ToString("N0") & "</td>"
                Next
                strHTML &= "</tr>"
            Next
            strHTML &= "</table><p></p>"

            Return strHTML

        End Function

#End Region

#Region "Current Pilot XML Report (Old Style)"

        Public Shared Sub GenerateCurrentPilotXML_Old(ByVal rPilot As EveHQPilot)

            Dim strXML As String = ""
            strXML &= CurrentPilotXML_Old(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "CurrentXML - Old (" & rPilot.Name & ").xml"))
            sw.Write(strXML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()
        End Sub

        Public Shared Function CurrentPilotXML_Old(ByVal rpilot As EveHQPilot) As String
            Dim tabs(10) As String
            For tab As Integer = 1 To 10
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
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
            strXML &= tabs(4) & "<intelligence>" & rpilot.IntAtt & "</intelligence>" & vbCrLf
            strXML &= tabs(4) & "<charisma>" & rpilot.CAtt & "</charisma>" & vbCrLf
            strXML &= tabs(4) & "<perception>" & rpilot.PAtt & "</perception>" & vbCrLf
            strXML &= tabs(4) & "<memory>" & rpilot.MAtt & "</memory>" & vbCrLf
            strXML &= tabs(4) & "<willpower>" & rpilot.WAtt & "</willpower>" & vbCrLf
            strXML &= tabs(3) & "</attributes>" & vbCrLf
            strXML &= tabs(3) & "<skills>" & vbCrLf

            For Each skillGroup As SkillGroup In HQ.SkillGroups.Values
                If skillGroup.ID <> 505 Then
                    strXML &= tabs(4) & "<skillGroup groupName=""" & skillGroup.Name & """ groupID=""" & skillGroup.ID & """>" & vbCrLf
                    For Each skillItem As EveHQPilotSkill In rpilot.PilotSkills.Values
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

                            strXML &= tabs(6) & "<skilllevel1>" & HQ.SkillListID(skillItem.ID).LevelUp(1) & "</skilllevel1>" & vbCrLf
                            strXML &= tabs(6) & "<skilllevel2>" & HQ.SkillListID(skillItem.ID).LevelUp(2) & "</skilllevel2>" & vbCrLf
                            strXML &= tabs(6) & "<skilllevel3>" & HQ.SkillListID(skillItem.ID).LevelUp(3) & "</skilllevel3>" & vbCrLf
                            strXML &= tabs(6) & "<skilllevel4>" & HQ.SkillListID(skillItem.ID).LevelUp(4) & "</skilllevel4>" & vbCrLf
                            strXML &= tabs(6) & "<skilllevel5>" & HQ.SkillListID(skillItem.ID).LevelUp(5) & "</skilllevel5>" & vbCrLf
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

        Public Shared Sub GenerateCurrentTrainingXML_Old(ByVal rPilot As EveHQPilot)

            Dim strXML As String = ""
            strXML &= CurrentTrainingXML_Old(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "TrainingXML - Old (" & rPilot.Name & ").xml"))
            sw.Write(strXML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()
        End Sub

        Public Shared Function CurrentTrainingXML_Old(ByVal rpilot As EveHQPilot) As String
            Dim tabs(10) As String
            For tab As Integer = 1 To 10
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
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
            strXML &= tabs(4) & "<intelligence>" & rpilot.IntAtt & "</intelligence>" & vbCrLf
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

        Public Shared Sub GenerateCurrentPilotXML_New(ByVal rPilot As EveHQPilot)

            Dim strXML As String = ""
            strXML &= CurrentPilotXML_New(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "CurrentXML - New (" & rPilot.Name & ").xml"))
            sw.Write(strXML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()
        End Sub

        Public Shared Function CurrentPilotXML_New(ByVal rpilot As EveHQPilot) As String
            Dim tabs(10) As String
            For tab As Integer = 1 To 10
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
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
            Dim culture As CultureInfo = New CultureInfo("en-GB")
            strXML &= tabs(2) & "<balance>" & rpilot.Isk.ToString(culture.NumberFormat) & "</balance>" & vbCrLf
            strXML &= XMLImplantDetails(rpilot)
            strXML &= tabs(2) & "<attributes>" & vbCrLf
            strXML &= tabs(3) & "<intelligence>" & rpilot.IntAtt & "</intelligence>" & vbCrLf
            strXML &= tabs(3) & "<charisma>" & rpilot.CAtt & "</charisma>" & vbCrLf
            strXML &= tabs(3) & "<perception>" & rpilot.PAtt & "</perception>" & vbCrLf
            strXML &= tabs(3) & "<memory>" & rpilot.MAtt & "</memory>" & vbCrLf
            strXML &= tabs(3) & "<willpower>" & rpilot.WAtt & "</willpower>" & vbCrLf
            strXML &= tabs(2) & "</attributes>" & vbCrLf
            strXML &= tabs(2) & "<rowset name=""skills"" key=""typeID"" columns=""typeID,skillpoints,level,unpublished"">" & vbCrLf

            For Each skillItem As EveHQPilotSkill In rpilot.PilotSkills.Values
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

        Public Shared Function CurrentTrainingXML_New(ByVal rpilot As EveHQPilot) As String
            Dim tabs(10) As String
            For tab As Integer = 1 To 10
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
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
            strXML &= tabs(1) & "<cachedUntil>" & Format(SkillFunctions.ConvertLocalTimeToEve(Now), "yyyy-MM-dd HH:mm:ss") & "</cachedUntil>" & vbCrLf
            strXML &= tabs(0) & "</eveapi>" & vbCrLf

            Return strXML

        End Function

#End Region

#Region "Skill Group Chart"
        Public Shared Sub SkillGroupChart(ByVal rpilot As EveHQPilot, chart1 As Chart)

            Const CollectionThreshold As Double = 4

            ' Calculate skill splits
            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim repGroup(HQ.SkillGroups.Count, 3) As String
            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 5) As String
            Dim cGroup As SkillGroup
            Dim cSkill As EveHQPilotSkill
            Dim groupCount As Integer = 0
            Dim totalSPCount As Long = 0
            For Each cGroup In HQ.SkillGroups.Values
                groupCount += 1
                repGroup(groupCount, 1) = cGroup.Name
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                For Each cSkill In rpilot.PilotSkills.Values
                    If cSkill.GroupID = cGroup.ID Then
                        skillCount += 1
                        spCount += cSkill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cSkill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cSkill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                        If rpilot.Training = True Then
                            If currentSkill.ID = cSkill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                totalSPCount += spCount
            Next

            ' Create series
            chart1.Series.Clear()
            chart1.Series.Add("SP")

            ' Set chart type and styles
            chart1.Series("SP").ChartType = SeriesChartType.Doughnut
            chart1.Series("SP")("PieLabelStyle") = "Inside"
            chart1.Series("SP")("DoughnutRadius") = "90"
            chart1.Series("SP")("PieDrawingStyle") = "SoftEdge"

            chart1.ChartAreas("Default").Area3DStyle.Enable3D = True

            chart1.Titles(0).Text = "Skill Breakdown by Category - " & rpilot.Name

            ' Add the data
            Dim x As New List(Of String)
            Dim y As New List(Of Double)
            Dim collectedTotal As Double = 0
            Dim collectedDetails As String = ""

            For group As Integer = 1 To HQ.SkillGroups.Count
                If CDbl(repGroup(group, 3)) > 0 Then
                    x.Add(repGroup(group, 1) & ControlChars.CrLf & CDbl(repGroup(group, 3)).ToString("N0") & " (" & (CDbl(repGroup(group, 3)) / totalSPCount * 100).ToString("N2") & "%)")
                    y.Add(CDbl(repGroup(group, 3)))
                    If CDbl(repGroup(group, 3)) / totalSPCount * 100 <= CollectionThreshold Then
                        collectedTotal += CDbl(repGroup(group, 3))
                        collectedDetails &= repGroup(group, 1) & ": " & CDbl(repGroup(group, 3)).ToString("N0") & ControlChars.CrLf
                    End If
                End If
            Next
            chart1.Series("SP").Points.DataBindXY(x, y)
            chart1.Legends(0).Enabled = True
            chart1.Legends(0).Alignment = StringAlignment.Center
            chart1.Legends(0).Docking = Docking.Right

            ' Explode all points
            For Each point As DataPoint In chart1.Series("SP").Points
                point("Exploded") = "true"
            Next

            Dim series1 As Series = chart1.Series(0)

            ' Set the threshold under which all points will be collected
            series1("CollectedThreshold") = CollectionThreshold.ToString

            ' Set the threshold type to be a percentage of the pie
            ' When set to false, this property uses the actual value to determine the collected threshold
            series1("CollectedThresholdUsePercent") = "true"

            ' Set the label of the collected pie slice
            series1("CollectedLabel") = "Others" & ControlChars.CrLf & collectedTotal.ToString("N0") & " (" & (collectedTotal / totalSPCount * 100).ToString("N2") & "%)"

            ' Set the legend text of the collected pie slice
            series1("CollectedLegendText") = "Others:-" & ControlChars.CrLf & collectedDetails

            ' Set the collected pie slice to be exploded
            series1("CollectedSliceExploded") = "true"

            ' Set the color of the collected pie slice
            series1("CollectedColor") = "LightGreen"

            ' Set the tooltip of the collected pie slice
            series1("CollectedToolTip") = "Others:-" & ControlChars.CrLf & collectedDetails

        End Sub

#End Region

#Region "Skill Cost Chart"
        Public Shared Sub SkillCostChart(ByVal rpilot As EveHQPilot, chart1 As Chart)

            Const CollectionThreshold As Double = 1

            ' Calculate skill splits
            Dim repGroup(HQ.SkillGroups.Count, 3) As String
            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 5) As String
            Dim cGroup As SkillGroup
            Dim cSkill As EveHQPilotSkill
            Dim groupCount As Integer = 0
            Dim totalSkillCost As Long = 0
            For Each cGroup In HQ.SkillGroups.Values
                groupCount += 1
                repGroup(groupCount, 1) = cGroup.Name
                Dim skillCount As Long = 0
                Dim totalCost As Long = 0
                For Each cSkill In rpilot.PilotSkills.Values
                    If cSkill.GroupID = cGroup.ID Then
                        skillCount += 1
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cSkill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = (CLng(StaticData.Types(cSkill.ID).BasePrice) * 0.9).ToString("N0")
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)
                        totalCost += CLng(repSkill(groupCount, CInt(skillCount), 4))
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(totalCost)
                totalSkillCost += totalCost
            Next

            ' Create series
            chart1.Series.Clear()
            chart1.Series.Add("SP")

            ' Set chart type and styles
            chart1.Series("SP").ChartType = SeriesChartType.Doughnut
            chart1.Series("SP")("PieLabelStyle") = "Outside"
            chart1.Series("SP")("DoughnutRadius") = "90"
            chart1.Series("SP")("PieDrawingStyle") = "SoftEdge"

            chart1.ChartAreas("Default").Area3DStyle.Enable3D = True

            chart1.Titles(0).Text = "Skill Breakdown by Category - " & rpilot.Name

            ' Add the data
            Dim x As New List(Of String)
            Dim y As New List(Of Double)
            Dim collectedTotal As Double = 0
            Dim collectedDetails As String = ""

            For group As Integer = 1 To HQ.SkillGroups.Count
                If CDbl(repGroup(group, 3)) > 0 Then
                    x.Add(repGroup(group, 1) & ControlChars.CrLf & CDbl(repGroup(group, 3)).ToString("N0") & " (" & (CDbl(repGroup(group, 3)) / totalSkillCost * 100).ToString("N2") & "%)")
                    y.Add(CDbl(repGroup(group, 3)))
                    If CDbl(repGroup(group, 3)) / totalSkillCost * 100 <= CollectionThreshold Then
                        collectedTotal += CDbl(repGroup(group, 3))
                        collectedDetails &= repGroup(group, 1) & ": " & CDbl(repGroup(group, 3)).ToString("N0") & ControlChars.CrLf
                    End If
                End If
            Next
            chart1.Series("SP").Points.DataBindXY(x, y)
            chart1.Legends(0).Enabled = True
            chart1.Legends(0).Alignment = StringAlignment.Center
            chart1.Legends(0).Docking = Docking.Right

            ' Explode all points
            For Each point As DataPoint In chart1.Series("SP").Points
                point("Exploded") = "true"
            Next

            Dim series1 As Series = chart1.Series(0)

            ' Set the threshold under which all points will be collected
            series1("CollectedThreshold") = CollectionThreshold.ToString

            ' Set the threshold type to be a percentage of the pie
            ' When set to false, this property uses the actual value to determine the collected threshold
            series1("CollectedThresholdUsePercent") = "true"

            ' Set the label of the collected pie slice
            series1("CollectedLabel") = "Others" & ControlChars.CrLf & collectedTotal.ToString("N0") & " (" & (collectedTotal / totalSkillCost * 100).ToString("N2") & "%)"

            ' Set the legend text of the collected pie slice
            series1("CollectedLegendText") = "Others:-" & ControlChars.CrLf & collectedDetails

            ' Set the collected pie slice to be exploded
            series1("CollectedSliceExploded") = "true"

            ' Set the color of the collected pie slice
            series1("CollectedColor") = "LightGreen"

            ' Set the tooltip of the collected pie slice
            series1("CollectedToolTip") = "Others:-" & ControlChars.CrLf & collectedDetails

        End Sub
#End Region

#Region "Text Character Sheet Report"
        Public Shared Sub GenerateTextCharSheet(ByVal rpilot As EveHQPilot)

            Dim strText As String = ""
            strText &= TextCharacterDetails("Character Sheet", rpilot)
            strText &= TextCharacterSheet(rpilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "CharSheet (" & rpilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextCharacterSheet(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim repGroup(HQ.SkillGroups.Count, 3) As String
            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 5) As String
            Dim cGroup As SkillGroup
            Dim cSkill As EveHQPilotSkill
            Dim groupCount As Integer = 0
            For Each cGroup In HQ.SkillGroups.Values
                groupCount += 1
                repGroup(groupCount, 1) = cGroup.Name
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                ' Collect skills
                Dim repSkills As New SortedList(Of String, EveHQPilotSkill)
                For Each cSkill In rpilot.PilotSkills.Values
                    repSkills.Add(cSkill.Name, cSkill)
                Next
                For Each cSkill In repSkills.Values
                    If cSkill.GroupID = cGroup.ID Then
                        skillCount += 1
                        spCount += cSkill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cSkill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cSkill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                        If rpilot.Training = True Then
                            If currentSkill.ID = cSkill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
            Next

            For group As Integer = 1 To HQ.SkillGroups.Count
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine(repGroup(group, 1) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        Dim txtData(4) As String
                        txtData(0) = "  * " & repSkill(group, skill, 1)
                        txtData(1) = "Rank " & repSkill(group, skill, 2)
                        txtData(2) = "Level " & repSkill(group, skill, 5)
                        txtData(3) = "SP " & CDbl(repSkill(group, skill, 3)).ToString("N0")
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
        Public Shared Sub GenerateTextTrainingTime(ByVal rPilot As EveHQPilot)

            Dim strText As String = ""
            strText &= TextCharacterDetails("Training Times", rPilot)
            strText &= TextTrainingTime(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "TrainTime (" & rPilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextTrainingTime(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            Const RedTime As Long = 0
            For Each curSkill In rpilot.PilotSkills.Values
                ' Determine if the skill is being trained
                If rpilot.Training = True Then
                    If curSkill.ID = rpilot.TrainingSkillID Then
                        sortSkill(count, 1) = rpilot.TrainingCurrentTime
                    Else
                        sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
                    End If
                Else
                    sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim cskill As EveHQPilotSkill
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        cskill = rpilot.PilotSkills(askill.Name)
                        skillCount += 1
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0)
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim group As Integer = 1
            Do
                group = group + 1
                If group = Nog + 1 Then group = 1
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        Dim txtData(4) As String
                        txtData(0) = "  * " & repSkill(group, skill, 1)
                        txtData(1) = "Rank " & repSkill(group, skill, 2)
                        txtData(2) = "Level " & repSkill(group, skill, 5)
                        txtData(3) = "SP " & CDbl(repSkill(group, skill, 3)).ToString("N0")
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
        Public Shared Sub GenerateTextTimeToLevel5(ByVal rPilot As EveHQPilot)

            Dim strText As String = ""
            strText &= TextCharacterDetails("Time To Level 5", rPilot)
            strText &= TextTimeToLevel5(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "TimeToLevel5 (" & rPilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextTimeToLevel5(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            Const RedTime As Long = 0
            For Each curSkill In rpilot.PilotSkills.Values
                ' Determine if the skill is being trained
                If rpilot.Training = True Then
                    sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 5) - RedTime, 0))
                Else
                    sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 5) - RedTime, 0))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim cskill As EveHQPilotSkill
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        cskill = rpilot.PilotSkills(askill.Name)
                        skillCount += 1
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5)
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next


            Dim group As Integer = 1
            Do
                group = group + 1
                If group = Nog + 1 Then group = 1
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        Dim txtData(4) As String
                        txtData(0) = "  * " & repSkill(group, skill, 1)
                        txtData(1) = "Rank " & repSkill(group, skill, 2)
                        txtData(2) = "Level " & repSkill(group, skill, 5)
                        txtData(3) = "SP " & CDbl(repSkill(group, skill, 3)).ToString("N0")
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
        Public Shared Sub GenerateTextSkillLevels(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= TextCharacterDetails("Skill Levels", rPilot)
            strHTML &= TextSkillLevels(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillLevels (" & rPilot.Name & ").txt"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextSkillLevels(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As String
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            For Each curSkill In rpilot.PilotSkills.Values
                sortSkill(count, 0) = CStr(curSkill.ID)
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
            Const Nog As Integer = 6              ' Number of groups to break the report into
            Dim repGroup(Nog, 5) As String      ' Name, Min, Max, skillcount, SPs
            repGroup(1, 0) = "Level 0" : repGroup(1, 1) = "0" : repGroup(1, 2) = "0"
            repGroup(2, 0) = "Level 1" : repGroup(2, 1) = "1" : repGroup(2, 2) = "1"
            repGroup(3, 0) = "Level 2" : repGroup(3, 1) = "2" : repGroup(3, 2) = "2"
            repGroup(4, 0) = "Level 3" : repGroup(4, 1) = "3" : repGroup(4, 2) = "3"
            repGroup(5, 0) = "Level 4" : repGroup(5, 1) = "4" : repGroup(5, 2) = "4"
            repGroup(6, 0) = "Level 5" : repGroup(6, 1) = "5" : repGroup(6, 2) = "5"

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    Dim cskill As EveHQPilotSkill
                    Dim askill As EveSkill
                    askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                    cskill = rpilot.PilotSkills(askill.Name)
                    If cskill.Level >= CDbl(repGroup(groupCount, 1)) And cskill.Level <= CDbl(repGroup(groupCount, 2)) Then
                        skillCount += 1
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        Dim curTime As Long = CLng(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0))
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(curTime)
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            For group As Integer = 1 To Nog
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        Dim txtData(4) As String
                        txtData(0) = "  * " & repSkill(group, skill, 1)
                        txtData(1) = "Rank " & repSkill(group, skill, 2)
                        txtData(2) = "Level " & repSkill(group, skill, 5)
                        txtData(3) = "SP " & CDbl(repSkill(group, skill, 3)).ToString("N0")
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

#Region "Text Skill Ranks Report"
        Public Shared Sub GenerateTextSkillRanks(ByVal rPilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= TextCharacterDetails("Skill Ranks", rPilot)
            strHTML &= TextSkillRanks(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillRanks (" & rPilot.Name & ").txt"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextSkillRanks(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As String
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            For Each curSkill In rpilot.PilotSkills.Values
                sortSkill(count, 0) = CStr(curSkill.ID)
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
            Const Nog As Integer = 16              ' Number of groups to break the report into
            Dim repGroup(Nog, 5) As String      ' Name, Min, Max, skillcount, SPs
            repGroup(1, 0) = "Rank 1" : repGroup(1, 1) = "1" : repGroup(1, 2) = "1"
            repGroup(2, 0) = "Rank 2" : repGroup(2, 1) = "2" : repGroup(2, 2) = "2"
            repGroup(3, 0) = "Rank 3" : repGroup(3, 1) = "3" : repGroup(3, 2) = "3"
            repGroup(4, 0) = "Rank 4" : repGroup(4, 1) = "4" : repGroup(4, 2) = "4"
            repGroup(5, 0) = "Rank 5" : repGroup(5, 1) = "5" : repGroup(5, 2) = "5"
            repGroup(6, 0) = "Rank 6" : repGroup(6, 1) = "6" : repGroup(6, 2) = "6"
            repGroup(7, 0) = "Rank 7" : repGroup(7, 1) = "7" : repGroup(7, 2) = "7"
            repGroup(8, 0) = "Rank 8" : repGroup(8, 1) = "8" : repGroup(8, 2) = "8"
            repGroup(9, 0) = "Rank 9" : repGroup(9, 1) = "9" : repGroup(9, 2) = "9"
            repGroup(10, 0) = "Rank 10" : repGroup(10, 1) = "10" : repGroup(10, 2) = "10"
            repGroup(11, 0) = "Rank 11" : repGroup(11, 1) = "11" : repGroup(11, 2) = "11"
            repGroup(12, 0) = "Rank 12" : repGroup(12, 1) = "12" : repGroup(12, 2) = "12"
            repGroup(13, 0) = "Rank 13" : repGroup(13, 1) = "13" : repGroup(13, 2) = "13"
            repGroup(14, 0) = "Rank 14" : repGroup(14, 1) = "14" : repGroup(14, 2) = "14"
            repGroup(15, 0) = "Rank 15" : repGroup(15, 1) = "15" : repGroup(15, 2) = "15"
            repGroup(16, 0) = "Rank 16" : repGroup(16, 1) = "16" : repGroup(16, 2) = "16"

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    Dim cskill As EveHQPilotSkill
                    Dim askill As EveSkill
                    askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                    cskill = rpilot.PilotSkills(askill.Name)
                    If cskill.Rank >= CDbl(repGroup(groupCount, 1)) And cskill.Rank <= CDbl(repGroup(groupCount, 2)) Then
                        skillCount += 1
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        Dim curTime As Long = CLng(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5))
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 5)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(curTime)
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            For group As Integer = 1 To Nog
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        Dim txtData(4) As String
                        txtData(0) = "  * " & repSkill(group, skill, 1)
                        txtData(1) = "Rank " & repSkill(group, skill, 2)
                        txtData(2) = "Level " & repSkill(group, skill, 5)
                        txtData(3) = "SP " & CDbl(repSkill(group, skill, 3)).ToString("N0")
                        txtData(4) = "TTL5 " & repSkill(group, skill, 4)
                        strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-15} {4,-25}", txtData))
                    Next
                    strText.AppendLine(String.Format("{0,-45} {1,-40}", "", "Time to Train Group To Level 5: " & repGroup(group, 4)))
                    strText.AppendLine()
                End If
            Next

            Return strText.ToString
        End Function

#End Region

#Region "Skills Available Report"

        Public Shared Sub GenerateTextSkillsAvailable(ByVal rPilot As EveHQPilot)

            Dim strText As String = ""
            strText &= TextCharacterDetails("Skills Available To Train", rPilot)
            strText &= TextSkillsAvailable(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillsToTrain (" & rPilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextSkillsAvailable(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            strText.AppendLine(String.Format("{0,-45} {1,-25} {2,-10}", "Skill Name", "Skill Group", "Skill Rank"))
            strText.AppendLine(String.Format("{0,-45} {1,-25} {2,-10}", "----------", "-----------", "----------"))

            Dim trainable As Boolean
            For Each skill As EveSkill In HQ.SkillListName.Values
                trainable = False
                If rpilot.PilotSkills.ContainsKey(skill.Name) = False And skill.Published = True Then
                    trainable = True
                    For Each preReq As Integer In skill.PreReqSkills.Keys
                        If skill.PreReqSkills(preReq) <> 0 Then
                            Dim ps As EveSkill = HQ.SkillListID(preReq)
                            If rpilot.PilotSkills.ContainsKey(ps.Name) = True Then
                                Dim psp As EveHQPilotSkill = rpilot.PilotSkills(ps.Name)
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
                    strText.AppendLine(String.Format("{0,-45} {1,-29} {2,-5}", skill.Name, StaticData.TypeGroups(CInt(skill.GroupID)), skill.Rank))
                End If
            Next

            Return strText.ToString
        End Function

#End Region

#Region "Text Skills Not Trained Report"
        Public Shared Sub GenerateTextSkillsNotTrained(ByVal rPilot As EveHQPilot)

            Dim strText As String = ""
            strText &= TextCharacterDetails("Skills Not Trained", rPilot)
            strText &= TextSkillsNotTrained(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillsNotTrained (" & rPilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()
            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextSkillsNotTrained(ByVal rPilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim sortSkill(HQ.SkillListID.Count, 1) As Long
            Dim curSkill As EveSkill
            Dim count As Integer
            For Each curSkill In HQ.SkillListID.Values
                If curSkill.Published = True Then
                    If rPilot.PilotSkills.ContainsKey(curSkill.Name) = False Then
                        ' Determine if the skill is being trained
                        sortSkill(count, 1) = CLng(SkillFunctions.TimeBeforeCanTrain(rPilot, curSkill.ID))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        skillCount += 1
                        totalTime += skillTimeLeft
                        spCount += askill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(askill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = askill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(askill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = "0"
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(skillTimeLeft)
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(askill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(askill.Level + 1, 5)))

                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            For group As Integer = 1 To Nog
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

        Public Shared Sub GenerateTextTrainQueue(ByVal rPilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue)

            Dim strText As String = ""
            strText &= TextCharacterDetails(rQueue.Name & " Training Queue", rPilot)
            strText &= TextTrainQueue(rPilot, rQueue)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "TrainQueue - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextTrainQueue(ByVal rpilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue) As String
            Dim strText As New StringBuilder
            Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(rpilot, rQueue, False, True)

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
                Dim qItem As SortedQueueItem = CType(arrQueue(skill), SortedQueueItem)
                Dim skillName As String = qItem.Name
                Dim curLevel As Integer = qItem.CurLevel
                Dim startLevel As Integer = qItem.FromLevel
                Dim endLevel As Integer = qItem.ToLevel
                Dim percent As Double = qItem.Percent
                Dim timeToEnd As String = SkillFunctions.TimeToString(CDbl(qItem.TrainTime))
                Dim endTime As String = Format(qItem.DateFinished, "ddd") & " " & qItem.DateFinished.ToString

                txtData(0) = skillName
                txtData(1) = curLevel.ToString
                txtData(2) = startLevel.ToString
                txtData(3) = endLevel.ToString
                txtData(4) = percent.ToString("N0")
                txtData(5) = timeToEnd
                txtData(6) = endTime
                strText.AppendLine(String.Format("{0,-49} {1,-10} {2,-9} {3,-9} {4,-8} {5,-20} {6,-20}", txtData))
            Next

            Return strText.ToString
        End Function

#End Region

#Region "Text Shopping List Reports"
        Public Shared Sub GenerateTextShoppingList(ByVal rPilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue)

            Dim strText As String = ""
            strText &= TextCharacterDetails(rQueue.Name & " Training Queue", rPilot)
            strText &= TextShoppingList(rPilot, rQueue)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "ShoppingList - " & rQueue.Name & " (" & rPilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextShoppingList(ByVal rpilot As EveHQPilot, ByVal rQueue As EveHQSkillQueue) As String
            Dim strText As New StringBuilder
            Dim arrQueue As ArrayList = SkillQueueFunctions.BuildQueue(rpilot, rQueue, False, True)

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
                Dim qItem As SortedQueueItem = CType(arrQueue(skill), SortedQueueItem)
                Dim skillName As String = qItem.Name
                If rpilot.PilotSkills.ContainsKey(skillName) = False Then
                    If skillPriceList.Contains(skillName) = False Then
                        skillPriceList.Add(skillName)
                    End If
                End If
            Next

            Dim totalCost As Double = 0
            For Each skillName As String In skillPriceList
                Dim cSkill As EveSkill = HQ.SkillListName(skillName)
                txtData(0) = skillName
                txtData(1) = cSkill.BasePrice.ToString("N2")
                strText.AppendLine(String.Format("{0,-45} {1,-10}", txtData))
                totalCost += cSkill.BasePrice
            Next

            strText.AppendLine("")
            txtData(0) = "Total Queue Shopping List Cost:"
            txtData(1) = totalCost.ToString("N2")
            strText.AppendLine(String.Format("{0,-45} {1,-10}", txtData))

            Return strText.ToString
        End Function
#End Region

#Region "Partial Skills Report"
        Public Shared Sub GeneratePartialSkills(ByVal rpilot As EveHQPilot)
            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Partially Trained Skills - " & rpilot.Name)
            strHTML &= HTMLTitle("Partially Trained Skills - " & rpilot.Name)
            strHTML &= HTMLCharacterDetails(rpilot)
            strHTML &= PartialSkills(rpilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "PartialSkills (" & rpilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()
        End Sub

        Public Shared Function PartialSkills(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            Const RedTime As Long = 0
            For Each curSkill In rpilot.PilotSkills.Values
                Dim partTrained As Boolean = True
                For level As Integer = 0 To 5
                    If curSkill.SP = HQ.SkillListID(curSkill.ID).LevelUp(level) Or curSkill.SP = HQ.SkillListID(curSkill.ID).LevelUp(level) + 1 Or curSkill.Level = 5 Then
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
                            sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
                        End If
                    Else
                        sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim cskill As EveHQPilotSkill
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        cskill = rpilot.PilotSkills(askill.Name)
                        skillCount += 1
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0)
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim imgLevel As String
            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            Dim group As Integer = 1
            Do
                group = group + 1
                If group = Nog + 1 Then group = 1
                If CDbl(repGroup(group, 2)) > 0 Then
                    strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)</td><td class=thead width=50px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
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
        Public Shared Sub GenerateTextPartialSkills(ByVal rPilot As EveHQPilot)

            Dim strText As String = ""
            strText &= TextCharacterDetails("Partially Trained Skills", rPilot)
            strText &= TextPartialSkills(rPilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "PartialSkills (" & rPilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextPartialSkills(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim sortSkill(rpilot.PilotSkills.Count, 1) As Long
            Dim curSkill As EveHQPilotSkill
            Dim count As Integer
            Const RedTime As Long = 0
            For Each curSkill In rpilot.PilotSkills.Values
                Dim partTrained As Boolean = True
                For level As Integer = 0 To 5
                    If curSkill.SP = HQ.SkillListID(curSkill.ID).LevelUp(level) Or curSkill.SP = HQ.SkillListID(curSkill.ID).LevelUp(level) + 1 Or curSkill.Level = 5 Then
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
                            sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
                        End If
                    Else
                        sortSkill(count, 1) = CLng(Math.Max(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(curSkill.Name), 0) - RedTime, 0))
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
            Const Nog As Integer = 15             ' Number of groups to break the report into
            Dim repGroup(Nog, 4) As String      ' Name, Min, Max, skillcount, SPs
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

            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 6) As String
            Dim groupCount As Integer
            Dim skillTimeLeft As Long
            For groupCount = 1 To Nog
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                Dim totalTime As Double = 0
                Dim i As Integer
                For i = 0 To tagArray.Length - 1
                    skillTimeLeft = sortSkill(tagArray(i), 1)
                    If skillTimeLeft >= CDbl(repGroup(groupCount, 1)) And skillTimeLeft <= CDbl(repGroup(groupCount, 2)) Then
                        Dim cskill As EveHQPilotSkill
                        Dim askill As EveSkill
                        askill = HQ.SkillListID(CInt(sortSkill(tagArray(i), 0)))
                        cskill = rpilot.PilotSkills(askill.Name)
                        skillCount += 1
                        totalTime += SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0)
                        spCount += cskill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cskill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cskill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cskill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cskill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cskill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cskill.Level)
                        repSkill(groupCount, CInt(skillCount), 6) = CStr(askill.LevelUp(Math.Min(cskill.Level + 1, 5)))

                        If rpilot.Training = True Then
                            If currentSkill.ID = cskill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = SkillFunctions.TimeToString(totalTime)
            Next

            Dim group As Integer = 1
            Do
                group = group + 1
                If group = Nog + 1 Then group = 1
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine(repGroup(group, 0) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        Dim txtData(4) As String
                        txtData(0) = "  * " & repSkill(group, skill, 1)
                        txtData(1) = "Rank " & repSkill(group, skill, 2)
                        txtData(2) = "Level " & repSkill(group, skill, 5)
                        txtData(3) = "SP " & CDbl(repSkill(group, skill, 3)).ToString("N0")
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
        Public Shared Sub GenerateECMExportReports(ByVal rpilot As EveHQPilot)
            Dim ecmLocation As String
            Dim result As Integer
            Dim fbd1 As New FolderBrowserDialog
            With fbd1
                .ShowNewFolderButton = False
                If HQ.Settings.EcmDefaultLocation <> "" Then
                    If My.Computer.FileSystem.DirectoryExists(HQ.Settings.EcmDefaultLocation) = True Then
                        .Description = "Please select the folder where the ECM XML files are located..." & ControlChars.CrLf & "Default is: " & HQ.Settings.EcmDefaultLocation
                        .SelectedPath = HQ.Settings.EcmDefaultLocation
                    Else
                        .Description = "Please select the folder where the ECM XML files are located..."
                        .RootFolder = Environment.SpecialFolder.Desktop
                    End If
                Else
                    .Description = "Please select the folder where the ECM XML files are located..."
                    .RootFolder = Environment.SpecialFolder.Desktop
                End If
                result = .ShowDialog()
                ecmLocation = .SelectedPath
            End With

            If ecmLocation <> "" And result = 1 Then

                ' Generate the Old Style XML Report
                Call GenerateCurrentPilotXML_Old(rpilot)
                ' Generate the Old Style Training XML
                Call GenerateCurrentTrainingXML_Old(rpilot)

                ' Copy these to the selected folder
                My.Computer.FileSystem.CopyFile(Path.Combine(HQ.reportFolder, "CurrentXML - Old (" & rpilot.Name & ").xml"), Path.Combine(ecmLocation, rpilot.ID.ToString & ".xml"), True)
                My.Computer.FileSystem.CopyFile(Path.Combine(HQ.reportFolder, "TrainingXML - Old (" & rpilot.Name & ").xml"), Path.Combine(ecmLocation, rpilot.ID.ToString & ".training.xml"), True)
                HQ.Settings.EcmDefaultLocation = ecmLocation
                MessageBox.Show("Export of ECM-compatible files completed!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else

                MessageBox.Show("Export of ECM-compatible aborted!", "Export Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Sub

#End Region

#Region "Skills Cost Report"
        Public Shared Sub GenerateSkillsCost(ByVal rpilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader("Skills Cost - " & rpilot.Name)
            strHTML &= HTMLTitle("Skills Cost - " & rpilot.Name)
            strHTML &= HTMLCharacterDetails(rpilot)
            strHTML &= SkillsCost(rpilot)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillsCost (" & rpilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function SkillsCost(ByVal rpilot As EveHQPilot) As String
            Dim strHTML As String = ""

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
            End If
            Dim totalCost As Long = 0
            Dim repGroup(HQ.SkillGroups.Count, 4) As String
            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 5) As String
            Dim cGroup As SkillGroup
            Dim cSkill As EveHQPilotSkill
            Dim groupCount As Integer = 0
            For Each cGroup In HQ.SkillGroups.Values
                groupCount += 1
                repGroup(groupCount, 1) = cGroup.Name
                Dim skillCount As Long = 0
                Dim groupCost As Long = 0
                Dim spCount As Long = 0
                ' Collect skills
                Dim repSkills As New SortedList(Of String, EveHQPilotSkill)
                For Each cSkill In rpilot.PilotSkills.Values
                    repSkills.Add(cSkill.Name, cSkill)
                Next
                For Each cSkill In repSkills.Values
                    If cSkill.GroupID = cGroup.ID Then
                        skillCount += 1
                        spCount += cSkill.SP
                        groupCost = CLng(groupCost + (CLng(StaticData.Types(cSkill.ID).BasePrice) * 0.9))
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cSkill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = (CLng(StaticData.Types(cSkill.ID).BasePrice) * 0.9).ToString("N0")
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                        If rpilot.Training = True Then
                            If currentSkill.ID = cSkill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = groupCost.ToString("N0")
                totalCost += groupCost
            Next

            Dim imgLevel As String
            strHTML &= "<table width=800px align=center cellspacing=0 cellpadding=0>"
            For group As Integer = 1 To HQ.SkillGroups.Count
                If CDbl(repGroup(group, 2)) > 0 Then
                    strHTML &= "<tr><td class=thead width=50px></td><td colspan=2 class=thead align=left valign=middle>" & repGroup(group, 1) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills, Cost: " & repGroup(group, 4) & " ISK)</td><td class=thead width=50px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        strHTML &= "<tr height=20px><td width=50px></td><td width=50px></td>"
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
                                strHTML &= "<td>"
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
                        strHTML &= "Skill Cost: " & repSkill(group, skill, 4)
                        strHTML &= "</td><td width=100px align=center><img src=" & imgLevel & " width=48 height=8 alt='Level " & repSkill(group, skill, 5) & "'></td></tr>"
                    Next
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                    strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
                End If
            Next
            strHTML &= "<tr><td width=50px></td><td width=50px></td><td>&nbsp;</td><td width=100px></td></tr>"
            strHTML &= "<tr><td width=50px></td><td width=50px></td><td><b>Total Skill Cost: " & totalCost.ToString("N0") & "</b></td><td width=100px></td></tr>"
            strHTML &= "</table><p></p>"
            Return strHTML
        End Function
#End Region

#Region "Text Skills Cost Report"
        Public Shared Sub GenerateTextSkillsCost(ByVal rpilot As EveHQPilot)

            Dim strText As String = ""
            strText &= TextCharacterDetails("Character Sheet", rpilot)
            strText &= TextSkillsCost(rpilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "SkillsCost (" & rpilot.Name & ").txt"))
            sw.Write(strText)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function TextSkillsCost(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
            End If
            Dim totalCost As Long = 0
            Dim repGroup(HQ.SkillGroups.Count, 4) As String
            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 5) As String
            Dim cGroup As SkillGroup
            Dim cSkill As EveHQPilotSkill
            Dim groupCount As Integer = 0
            For Each cGroup In HQ.SkillGroups.Values
                groupCount += 1
                repGroup(groupCount, 1) = cGroup.Name
                Dim skillCount As Long = 0
                Dim groupCost As Long = 0
                Dim spCount As Long = 0
                ' Collect skills
                Dim repSkills As New SortedList(Of String, EveHQPilotSkill)
                For Each cSkill In rpilot.PilotSkills.Values
                    repSkills.Add(cSkill.Name, cSkill)
                Next
                For Each cSkill In repSkills.Values
                    If cSkill.GroupID = cGroup.ID Then
                        skillCount += 1
                        spCount += cSkill.SP
                        groupCost = CLng(groupCost + (CLng(StaticData.Types(cSkill.ID).BasePrice) * 0.9))
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cSkill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = (CLng(StaticData.Types(cSkill.ID).BasePrice) * 0.9).ToString("N0")
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                        If rpilot.Training = True Then
                            If currentSkill.ID = cSkill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
                repGroup(groupCount, 4) = groupCost.ToString("N0")
                totalCost += groupCost
            Next

            For group As Integer = 1 To HQ.SkillGroups.Count
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine(repGroup(group, 1) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills, Cost: " & repGroup(group, 4) & " ISK)")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        Dim txtData(4) As String
                        txtData(0) = "  * " & repSkill(group, skill, 1)
                        txtData(1) = "Rank " & repSkill(group, skill, 2)
                        txtData(2) = "Level " & repSkill(group, skill, 5)
                        txtData(3) = "SP " & CDbl(repSkill(group, skill, 3)).ToString("N0")
                        txtData(4) = "Cost " & repSkill(group, skill, 4)
                        strText.AppendLine(String.Format("{0,-45} {1,-10} {2,-10} {3,-15} {4,-25}", txtData))
                    Next
                    strText.AppendLine()
                End If
            Next

            Dim txtFinalData(1) As String
            strText.AppendLine("")
            txtFinalData(0) = "Total Skill Cost:"
            txtFinalData(1) = totalCost.ToString("N2")
            strText.AppendLine(String.Format("{0,-45} {1,-10}", txtFinalData))

            Return strText.ToString
        End Function

#End Region

#Region "PHPBB Character Sheet Report"

        Public Shared Sub GeneratePHPBBCharSheet(ByVal rpilot As EveHQPilot)

            Dim strHTML As String = ""
            strHTML &= PHPBBCharacterDetails("Character Sheet", rpilot)
            strHTML &= PHPBBCharacterSheet(rpilot)
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, "PHPBBCharSheet (" & rpilot.Name & ").txt"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Public Shared Function PHPBBCharacterSheet(ByVal rpilot As EveHQPilot) As String
            Dim strText As New StringBuilder

            Dim currentSkill As New EveHQPilotSkill
            Dim currentSP As String = ""
            Dim currentTime As String = ""
            If rpilot.Training = True Then
                currentSkill = rpilot.PilotSkills.Item(SkillFunctions.SkillIDToName(rpilot.TrainingSkillID))
                currentSP = CStr(rpilot.TrainingCurrentSP)
                currentTime = SkillFunctions.TimeToString(rpilot.TrainingCurrentTime)
            End If

            Dim repGroup(HQ.SkillGroups.Count, 3) As String
            Dim repSkill(HQ.SkillGroups.Count, HQ.SkillListID.Count, 5) As String
            Dim cGroup As SkillGroup
            Dim cSkill As EveHQPilotSkill
            Dim groupCount As Integer = 0
            For Each cGroup In HQ.SkillGroups.Values
                groupCount += 1
                repGroup(groupCount, 1) = cGroup.Name
                Dim skillCount As Long = 0
                Dim spCount As Long = 0
                ' Collect skills
                Dim repSkills As New SortedList(Of String, EveHQPilotSkill)
                For Each cSkill In rpilot.PilotSkills.Values
                    repSkills.Add(cSkill.Name, cSkill)
                Next
                For Each cSkill In repSkills.Values
                    If cSkill.GroupID = cGroup.ID Then
                        skillCount += 1
                        spCount += cSkill.SP
                        repSkill(groupCount, CInt(skillCount), 0) = CStr(cSkill.ID)
                        repSkill(groupCount, CInt(skillCount), 1) = cSkill.Name
                        repSkill(groupCount, CInt(skillCount), 2) = CStr(cSkill.Rank)
                        repSkill(groupCount, CInt(skillCount), 3) = CStr(cSkill.SP)
                        repSkill(groupCount, CInt(skillCount), 4) = SkillFunctions.TimeToString(SkillFunctions.CalcTimeToLevel(rpilot, HQ.SkillListName(cSkill.Name), 0))
                        repSkill(groupCount, CInt(skillCount), 5) = CStr(cSkill.Level)

                        If rpilot.Training = True Then
                            If currentSkill.ID = cSkill.ID Then
                                repSkill(groupCount, CInt(skillCount), 3) = CStr((Val(repSkill(groupCount, CInt(skillCount), 3)) + Val(currentSP)))
                                repSkill(groupCount, CInt(skillCount), 4) = currentTime
                                spCount += CLng(currentSP)
                            End If
                        End If
                    End If
                Next
                repGroup(groupCount, 2) = CStr(skillCount)
                repGroup(groupCount, 3) = CStr(spCount)
            Next

            Dim trainingFlag As Boolean
            For group As Integer = 1 To HQ.SkillGroups.Count
                If CDbl(repGroup(group, 2)) > 0 Then
                    strText.AppendLine("[b][u]" & repGroup(group, 1) & " (" & Format(CLng(repGroup(group, 3)), "#,####") & " Skillpoints in " & repGroup(group, 2) & " Skills)[/u][/b]")
                    For skill As Integer = 1 To CInt(repGroup(group, 2))
                        trainingFlag = False
                        If rpilot.Training = True Then
                            If currentSkill.ID = CInt(repSkill(group, skill, 0)) Then
                                trainingFlag = True
                                strText.Append("[color=#ffaa00]")
                                strText.Append("[img]http://www.eveonline.com/bitmaps/character/level" & (CInt(repSkill(group, skill, 5)) + 1).ToString & "_act.gif[/img]")
                            Else
                                strText.Append("[img]http://www.eveonline.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif[/img]")
                            End If
                        Else
                            strText.Append("[img]http://www.eveonline.com/bitmaps/character/level" & repSkill(group, skill, 5) & ".gif[/img]")
                        End If
                        strText.Append("  " & repSkill(group, skill, 1))
                        strText.Append(" (Rank " & repSkill(group, skill, 2) & " - " & CDbl(repSkill(group, skill, 3)).ToString("N0") & " SP)")
                        If trainingFlag = True Then
                            strText.AppendLine("[/color]")
                        Else
                            strText.AppendLine()
                        End If
                    Next
                    strText.AppendLine()
                End If
            Next

            Return strText.ToString
        End Function

#End Region

#Region "HTML Cert Grades Reports"

        Public Shared Sub GenerateCertGradeTimes(ByVal rpilot As EveHQPilot, grade As Integer)

            Dim strHTML As String = ""
            strHTML &= HTMLHeader([Enum].GetName(GetType(CertificateGrade), grade) & " Certificate Grade Times - " & rpilot.Name)
            strHTML &= HTMLTitle([Enum].GetName(GetType(CertificateGrade), grade) & " Certificate Grade Times - " & rpilot.Name)
            strHTML &= HTMLCharacterDetails(rpilot)
            strHTML &= HTMLCertGradeTimes(rpilot, grade)
            strHTML &= HTMLFooter()
            Dim sw As StreamWriter = New StreamWriter(Path.Combine(HQ.reportFolder, [Enum].GetName(GetType(CertificateGrade), grade) & "CertGradeTimes (" & rpilot.Name & ").html"))
            sw.Write(strHTML)
            sw.Flush()
            sw.Close()

            ' Tidy up report variables
            GC.Collect()

        End Sub

        Private Shared Function HTMLCertGradeTimes(rpilot As EveHQPilot, grade As Integer) As String

            Dim strHTML As String = ""

            strHTML &= "<table width=600px align=center cellspacing=0 cellpadding=0>"
            strHTML &= "<tr>"
            strHTML &= "<td class=thead width=300px>Certificate</td>"
            strHTML &= "<td class=thead width=100px>Grade</td>"
            strHTML &= "<td class=thead width=200px>Time to Train</td>"
            strHTML &= "</tr>"

            ' Get the list of certifcates
            Dim certqueues As ArrayList = GetCertificateList(rpilot, grade)

            For Each ct As CertTimes In certqueues
                Dim cert As Certificate = StaticData.Certificates(ct.CertID)
                Dim ttc As String = SkillFunctions.TimeToString(ct.CertTime)

                strHTML &= "<tr height=20px>"
                strHTML &= "<td>" & ct.CertName & "</td>"
                strHTML &= "<td>" & CType(grade, CertificateGrade) & "</td>"
                strHTML &= "<td>" & ttc & "</td>"
                strHTML &= "</tr>"

            Next
            strHTML &= "</table><p></p>"
            Return strHTML

        End Function

        Private Shared Function GetCertificateList(rpilot As EveHQPilot, grade As Integer) As ArrayList

            ' Get a list of the relevant cert grades
            Dim certList As List(Of Certificate) = StaticData.Certificates.Values.ToList()

            Dim certQueues As New ArrayList
            ' Calculate time for each cert
            Dim skillList As New SortedList(Of Integer, Integer)
            For Each cert As Certificate In certList
                skillList.Clear()
                ' Calculate skills for the current cert
                GetSkillsForCertificate(cert, CType(grade, CertificateGrade), skillList)

                ' Work out time for this cert
                Dim qQueue As New EveHQSkillQueue
                qQueue.Name = cert.Id.ToString
                qQueue.IncCurrentTraining = False
                qQueue.Primary = False
                qQueue.Queue = New Dictionary(Of String, EveHQSkillQueueItem)
                Dim displayPilot As EveHQPilot = HQ.Settings.Pilots(rpilot.Name)
                For Each skillID As Integer In skillList.Keys
                    Dim skillName As String = HQ.SkillListID(CInt(skillID)).Name
                    qQueue = SkillQueueFunctions.AddSkillToQueue(displayPilot, skillName, qQueue.Queue.Count + 1, qQueue, skillList(skillID), True, True, cert.Id.ToString)
                Next
                Dim q As ArrayList = SkillQueueFunctions.BuildQueue(displayPilot, qQueue, False, True)
                If q.Count > 0 Then
                    certQueues.Add(New CertTimes(cert.Id, cert.Name, CLng((CType(q(q.Count - 1), SortedQueueItem).DateFinished - Now).TotalSeconds)))
                Else
                    certQueues.Add(New CertTimes(cert.Id, cert.Name, 0))
                End If
            Next

            ' Initialise a new ClassSorter instance and add a standard SortClass (i.e. sort method)
            Dim myClassSorter As New ClassSorter("CertTime", SortDirection.Ascending)
            ' Always sort by name to handle similarly ranked items in the first sort
            myClassSorter.SortClasses.Add(New SortClass("CertName", SortDirection.Ascending))
            ' Sort the class
            certQueues.Sort(myClassSorter)

            Return certQueues

        End Function

        Private Shared Sub GetSkillsForCertificate(ByRef cert As Certificate, ByVal grade As CertificateGrade, ByRef skillList As SortedList(Of Integer, Integer))
            ' Get skills for the cert
            Dim skillsNeeded = cert.GradesAndSkills(grade)

            For Each skill As Integer In skillsNeeded.Keys
                If skillList.ContainsKey(skill) = False Then
                    skillList.Add(skill, skillsNeeded(skill))
                Else
                    If skillsNeeded(skill) > skillList(skill) Then
                        skillList(skill) = skillsNeeded(skill)
                    End If
                End If
            Next
        End Sub

        Private Class CertTimes
            Public Property CertID As Integer
            Public Property CertName As String
            Public Property CertTime As Long

            Public Sub New(id As Integer, name As String, time As Long)
                CertID = id
                CertName = name
                CertTime = time
            End Sub
        End Class

#End Region

    End Class

End Namespace