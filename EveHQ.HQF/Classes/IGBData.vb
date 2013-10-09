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
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Classes

    Public Class IGBData

        Shared _currentFitting As Fitting
        Shared _hqfPilotName As String

        Shared Function Response(ByVal context As Net.HttpListenerContext) As String
            Dim strHTML As New StringBuilder
            strHTML.Append(Core.IGB.IGBHTMLHeader(context, "EveHQFitter", 0))
            strHTML.Append(HQFMenu(context))
            Select Case context.Request.Url.AbsolutePath.ToUpper
                Case "/EVEHQFITTER", "/EVEHQFITTER/"
                    strHTML.Append(MainPage(context))
                Case "/EVEHQFITTER/EVEFITTING", "/EVEHQFITTER/EVEFITTING/"
                    strHTML.Append(EveFitting(context))
                Case "/EVEHQFITTER/SAVEEVEFITTING", "/EVEHQFITTER/SAVEEVEFITTING/"
                    strHTML.Append(SaveEveFitting(context))
                Case "/EVEHQFITTER/PAGE2", "/EVEHQFITTER/PAGE2/"
                    strHTML.Append(HQFPage2(context))
            End Select
            strHTML.Append(Core.IGB.IGBHTMLFooter(context))
            Return strHTML.ToString
        End Function

        Shared Function HQFMenu(ByVal context As Net.HttpListenerContext) As String
            Dim strHTML As New StringBuilder
            strHTML.Append("<a href=/EVEHQFitter>HQF Home</a>  |  <a href=/EVEHQFitter/Page1>HQF Page 1</a>  |  <a href=/EVEHQFitter/Page2>HQF Page 2</a><br /><hr>")
            Return strHTML.ToString
        End Function

        Shared Function MainPage(ByVal context As Net.HttpListenerContext) As String
            Dim strHTML As New StringBuilder
            ' Check if we are trusted and get the Eve system name
            If context.Request.Headers("EVE_TRUSTED") = Nothing Then
                strHTML.Append("<p>This section of the IGB requires access via the Eve IGB.</p>")
            Else
                If context.Request.Headers("EVE_TRUSTED") = "no" Then
                    strHTML.Append("<p>This website is not trusted so cannot interact with </p>")
                Else
                    _hqfPilotName = context.Request.Headers("EVE_CHARNAME")
                    If FittingPilots.HQFPilots.ContainsKey(_hqfPilotName) = False Then
                        If FittingPilots.HQFPilots.Count = 0 Then
                            _hqfPilotName = ""
                        Else
                            _hqfPilotName = FittingPilots.HQFPilots.Keys(0).ToString
                        End If
                    End If
                End If
                If _hqfPilotName = "" Then
                    strHTML.Append("<p>Unable to find a suitable pilot to display skills. Check HQF is enabled and has been loaded.</p>")
                Else
                    strHTML.Append("<p>Fitting statistics will be for " & _hqfPilotName & " unless otherwise stated</p>")
                    strHTML.Append("</p>")
                    strHTML.Append("<form method=""GET"" action=""/EveHQFitter/EveFitting"">")
                    strHTML.Append("Paste Eve Fitting:  ")
                    strHTML.Append("<input type=""text"" name=""fitting"">")
                    strHTML.Append("<input type=""submit"" value=""Submit""></form><hr><br>")
                End If
            End If

            Return strHTML.ToString
        End Function

        Shared Function EveFitting(ByVal context As Net.HttpListenerContext) As String
            Dim strHTML As New StringBuilder

            If context.Request.QueryString.Count = 0 Or context.Request.QueryString.Item("fitting") = "" Then
                strHTML.Append("<p>Eve fitting has no content - please check data!</p>")
            Else
                Try

                    ' Try and establish some form parameters for the fitting
                    Dim fittingURL As String = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("fitting"))
                    If Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("fitting2")) <> "" Then
                        fittingURL = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("fitting2"))
                    End If

                    '   "\S+=["']?((?:.(?!["']?\s+(?:\S+)=|[>"']))+.)["']?>(.*?)\<\/url\>"
                    Dim newRegex As New Regex("\S+=[""']?((?:.(?![""']?\s+(?:\S+)=|[>""']))+.)[""']?>(.*?)\<\/url\>")
                    Dim newMatch As Match = newRegex.Match(fittingURL)
                    If newMatch.Success = False Then
                        strHTML.Append("<p>Unable to parse fitting URL</p>")
                    Else
                        Dim fittingDNA As String = newMatch.Groups.Item(1).Value
                        Dim fittingName As String = newMatch.Groups.Item(2).Value
                        'strHTML.Append("<p>Fitting DNA: " & fittingDNA & "</p>")
                        'strHTML.Append("<p>Fitting Name: " & fittingName & "</p>")
                        'For Each qs As String In context.Request.QueryString
                        '    strHTML.Append("<p>" & qs & ": " & Web.HttpUtility.UrlDecode(context.Request.QueryString.Item(qs)) & "</p>")
                        'Next

                        ' Set the fitting details and apply it
                        _currentFitting = DNAFitting.GetFittingFromEveDNA(fittingDNA, fittingName)

                        If Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("pilot")) <> "" Then
                            _currentFitting.PilotName = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("pilot"))
                        Else
                            _currentFitting.PilotName = _hqfPilotName
                        End If
                        _currentFitting.UpdateBaseShipFromFitting()

                        If Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("implants")) <> "" Then
                            _currentFitting.ImplantGroup = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("implants"))
                        Else
                            _currentFitting.ImplantGroup = FittingPilots.HQFPilots(_currentFitting.PilotName).ImplantName(0)
                        End If
                        FittingPilots.HQFPilots(_currentFitting.PilotName).ImplantName(0) = _currentFitting.ImplantGroup

                        If Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("damage")) <> "" Then
                            _currentFitting.BaseShip.DamageProfile = HQFDamageProfiles.ProfileList(Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("damage")))
                        Else
                            _currentFitting.BaseShip.DamageProfile = HQFDamageProfiles.ProfileList("<Omni-Damage>")
                        End If

                        ' Check for charges
                        For slot As Integer = 1 To _currentFitting.BaseShip.HiSlots
                            If _currentFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                                Dim chargeName As String = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item(_currentFitting.BaseShip.HiSlot(slot).Name))
                                If chargeName <> "" Then
                                    ' Load the charge
                                    Dim charge As ShipModule = ModuleLists.ModuleList(ModuleLists.ModuleListName(chargeName)).Clone
                                    _currentFitting.BaseShip.HiSlot(slot).LoadedCharge = charge
                                Else
                                    If _currentFitting.BaseShip.HiSlot(slot).Charges.Count <> 0 Then
                                        Dim firstCharge As ShipModule = ModuleLists.ModuleList(ModuleLists.ModuleListName(_currentFitting.BaseShip.HiSlot(slot).GetChargeList.Keys(0))).Clone
                                        _currentFitting.BaseShip.HiSlot(slot).LoadedCharge = firstCharge
                                    End If
                                End If
                            End If
                        Next
                        For slot As Integer = 1 To _currentFitting.BaseShip.MidSlots
                            If _currentFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                                Dim chargeName As String = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item(_currentFitting.BaseShip.MidSlot(slot).Name))
                                If chargeName <> "" Then
                                    ' Load the charge
                                    Dim charge As ShipModule = ModuleLists.ModuleList(ModuleLists.ModuleListName(chargeName)).Clone
                                    _currentFitting.BaseShip.MidSlot(slot).LoadedCharge = charge
                                Else
                                    If _currentFitting.BaseShip.MidSlot(slot).Charges.Count <> 0 Then
                                        Dim firstCharge As ShipModule = ModuleLists.ModuleList(ModuleLists.ModuleListName(_currentFitting.BaseShip.MidSlot(slot).GetChargeList.Keys(0))).Clone
                                        _currentFitting.BaseShip.MidSlot(slot).LoadedCharge = firstCharge
                                    End If
                                End If
                            End If
                        Next

                        _currentFitting.ApplyFitting()

                        strHTML.Append("<form method=""GET"" action=""/EveHQFitter/EveFitting"">")

                        ' Draw the various drop-downs
                        strHTML.Append("<table width=100% border=0><tr>")
                        strHTML.Append("<td width=30%>Pilot:")
                        strHTML.Append("<select name=pilot style='width: 100%; height:16px; font:normal 10px Arial, Tahoma; text-decoration:none;'>")
                        For Each pilot As String In FittingPilots.HQFPilots.Keys
                            strHTML.Append("<option ")
                            If pilot = _currentFitting.PilotName Then
                                strHTML.Append("selected='selected'")
                            End If
                            strHTML.Append(">" & pilot & "</option>")
                        Next
                        strHTML.Append("</select></td>")
                        strHTML.Append("<td width=30%>Implants:")
                        strHTML.Append("<select name=implants style='width: 100%; height:16px; font:normal 10px Arial, Tahoma; text-decoration:none;'>")
                        For Each implant As String In Settings.HQFSettings.ImplantGroups.Keys
                            strHTML.Append("<option ")
                            If implant = _currentFitting.ImplantGroup Then
                                strHTML.Append("selected='selected'")
                            End If
                            strHTML.Append(">" & implant & "</option>")
                        Next
                        strHTML.Append("</select></td>")
                        strHTML.Append("<td width=30%>Profile:")
                        strHTML.Append("<select name=damage style='width: 100%; height:16px; font:normal 10px Arial, Tahoma; text-decoration:none;'>")
                        For Each profile As String In HQFDamageProfiles.ProfileList.Keys
                            strHTML.Append("<option ")
                            If profile = _currentFitting.BaseShip.DamageProfile.Name Then
                                strHTML.Append("selected='selected'")
                            End If
                            strHTML.Append(">" & Web.HttpUtility.HtmlEncode(profile) & "</option>")
                        Next
                        strHTML.Append("</select></td>")
                        strHTML.Append("</tr></table>")

                        ' Collect similar modules
                        Dim hiSlots As New SortedList(Of String, Integer)
                        Dim midSlots As New SortedList(Of String, Integer)
                        Dim lowSlots As New SortedList(Of String, Integer)
                        Dim rigSlots As New SortedList(Of String, Integer)
                        For slot As Integer = 1 To _currentFitting.FittedShip.HiSlots
                            If _currentFitting.FittedShip.HiSlot(slot) IsNot Nothing Then
                                If hiSlots.ContainsKey(_currentFitting.FittedShip.HiSlot(slot).Name) = False Then
                                    hiSlots.Add(_currentFitting.FittedShip.HiSlot(slot).Name, 1)
                                Else
                                    hiSlots(_currentFitting.FittedShip.HiSlot(slot).Name) += 1
                                End If
                            End If
                        Next
                        For slot As Integer = 1 To _currentFitting.FittedShip.MidSlots
                            If _currentFitting.FittedShip.MidSlot(slot) IsNot Nothing Then
                                If midSlots.ContainsKey(_currentFitting.FittedShip.MidSlot(slot).Name) = False Then
                                    midSlots.Add(_currentFitting.FittedShip.MidSlot(slot).Name, 1)
                                Else
                                    midSlots(_currentFitting.FittedShip.MidSlot(slot).Name) += 1
                                End If
                            End If
                        Next
                        For slot As Integer = 1 To _currentFitting.FittedShip.LowSlots
                            If _currentFitting.FittedShip.LowSlot(slot) IsNot Nothing Then
                                If lowSlots.ContainsKey(_currentFitting.FittedShip.LowSlot(slot).Name) = False Then
                                    lowSlots.Add(_currentFitting.FittedShip.LowSlot(slot).Name, 1)
                                Else
                                    lowSlots(_currentFitting.FittedShip.LowSlot(slot).Name) += 1
                                End If
                            End If
                        Next
                        For slot As Integer = 1 To _currentFitting.FittedShip.RigSlots
                            If _currentFitting.FittedShip.RigSlot(slot) IsNot Nothing Then
                                If rigSlots.ContainsKey(_currentFitting.FittedShip.RigSlot(slot).Name) = False Then
                                    rigSlots.Add(_currentFitting.FittedShip.RigSlot(slot).Name, 1)
                                Else
                                    rigSlots(_currentFitting.FittedShip.RigSlot(slot).Name) += 1
                                End If
                            End If
                        Next

                        ' Draw outline table
                        strHTML.Append("<table width=100% border=0>")
                        strHTML.Append("<tr><td width=50% valign=top>")

                        ' Draw ship
                        strHTML.Append("<table width=100% border=1>")
                        strHTML.Append("<tr><td width=64px><img src='http://image.eveonline.com/Render/" & ShipLists.shipListKeyName(_currentFitting.ShipName) & "_64.png' width=64px height=64px>")
                        strHTML.Append("</td><td valign=top style='font:normal 16px  Arial, Tahoma;'>")
                        strHTML.Append("<div style='font:normal 18px  Arial, Tahoma;'><b>" & _currentFitting.ShipName & "</b></div><br />")
                        strHTML.Append("<div style='font:normal 12px  Arial, Tahoma;'>[" & fittingName & "]</div></td></tr>")
                        strHTML.Append("</table>")

                        ' Draw fitting table
                        strHTML.Append("<table width=100% border=1>")

                        If _currentFitting.FittedShip.HiSlots > 0 Then
                            strHTML.Append("<tr><td colspan=2 bgcolor=#111111><b>High Slots</b></td></tr>")
                            Dim modCount As Integer = 0
                            For Each slot As String In hiSlots.Keys
                                Dim chargeList As SortedList(Of String, String) = ModuleLists.ModuleList(ModuleLists.ModuleListName(slot)).GetChargeList
                                If chargeList.Count > 0 Then
                                    strHTML.Append("<tr><td width=50% bgcolor=#333333>" & hiSlots(slot) & "x " & slot & "</td>")
                                    strHTML.Append("<td width=50% bgcolor=#333333>")
                                    strHTML.Append("<select name='" & slot & "' style='width: 100%; height:14px; font:normal 10px Arial, Tahoma; text-decoration:none;'>")
                                    For Each charge As String In chargeList.Keys
                                        strHTML.Append("<option ")
                                        If charge = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item(slot)) Then
                                            strHTML.Append("selected='selected'")
                                        End If
                                        strHTML.Append(">" & charge & "</option>")
                                    Next
                                    strHTML.Append("</select></td></tr>")
                                Else
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>" & hiSlots(slot) & "x " & slot & "</td></tr>")
                                End If
                                modCount += hiSlots(slot)
                            Next
                            If modCount < _currentFitting.FittedShip.HiSlots Then
                                For emptySlot As Integer = 1 To _currentFitting.FittedShip.HiSlots - modCount
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>[Empty]</td></tr>")
                                Next
                            End If
                        End If

                        If _currentFitting.FittedShip.MidSlots > 0 Then
                            strHTML.Append("<tr><td colspan=2 bgcolor=#111111><b>Mid Slots</b></td></tr>")
                            Dim modCount As Integer = 0
                            For Each slot As String In midSlots.Keys
                                Dim chargeList As SortedList(Of String, String) = ModuleLists.ModuleList(ModuleLists.ModuleListName(slot)).GetChargeList
                                If chargeList.Count > 0 Then
                                    strHTML.Append("<tr><td width=50% bgcolor=#333333>" & midSlots(slot) & "x " & slot & "</td>")
                                    strHTML.Append("<td width=50% bgcolor=#333333>")
                                    strHTML.Append("<select name='" & slot & "' style='width: 100%; height:14px; font:normal 10px Arial, Tahoma; text-decoration:none;'>")
                                    For Each charge As String In chargeList.Keys
                                        strHTML.Append("<option ")
                                        If charge = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item(slot)) Then
                                            strHTML.Append("selected='selected'")
                                        End If
                                        strHTML.Append(">" & charge & "</option>")
                                    Next
                                    strHTML.Append("</select></td></tr>")
                                Else
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>" & midSlots(slot) & "x " & slot & "</td></tr>")
                                End If
                                modCount += midSlots(slot)
                            Next
                            If modCount < _currentFitting.FittedShip.MidSlots Then
                                For emptySlot As Integer = 1 To _currentFitting.FittedShip.MidSlots - modCount
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>[Empty]</td></tr>")
                                Next
                            End If
                        End If

                        If _currentFitting.FittedShip.LowSlots > 0 Then
                            strHTML.Append("<tr><td colspan=2 bgcolor=#111111><b>Low Slots</b></td></tr>")
                            Dim modCount As Integer = 0
                            For Each slot As String In lowSlots.Keys
                                strHTML.Append("<tr><td colspan=2 bgcolor=#333333>" & lowSlots(slot) & "x " & slot & "</td></tr>")
                                modCount += lowSlots(slot)
                            Next
                            If modCount < _currentFitting.FittedShip.LowSlots Then
                                For emptySlot As Integer = 1 To _currentFitting.FittedShip.LowSlots - modCount
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>[Empty]</td></tr>")
                                Next
                            End If
                        End If

                        If _currentFitting.FittedShip.RigSlots > 0 Then
                            strHTML.Append("<tr><td colspan=2 bgcolor=#111111><b>Rig Slots</b></td></tr>")
                            Dim modCount As Integer = 0
                            For Each slot As String In rigSlots.Keys
                                strHTML.Append("<tr><td colspan=2 bgcolor=#333333>" & rigSlots(slot) & "x " & slot & "</td></tr>")
                                modCount += rigSlots(slot)
                            Next
                            If modCount < _currentFitting.FittedShip.RigSlots Then
                                For emptySlot As Integer = 1 To _currentFitting.FittedShip.RigSlots - modCount
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>[Empty]</td></tr>")
                                Next
                            End If
                        End If

                        If _currentFitting.FittedShip.SubSlots > 0 Then
                            strHTML.Append("<tr><td colspan=2 bgcolor=#111111><b>Subsystem Slots</b></td></tr>")
                            For slot As Integer = 1 To _currentFitting.FittedShip.SubSlots
                                If _currentFitting.FittedShip.SubSlot(slot) IsNot Nothing Then
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>" & _currentFitting.FittedShip.SubSlot(slot).Name & "</td></tr>")
                                Else
                                    strHTML.Append("<tr><td colspan=2 bgcolor=#333333>[Empty]</td></tr>")
                                End If
                            Next
                        End If

                        If _currentFitting.FittedShip.DroneBayItems.Count > 0 Then
                            strHTML.Append("<tr><td colspan=2 bgcolor=#111111><b>Drone Bay</b></td></tr>")
                            For Each dbi As DroneBayItem In _currentFitting.FittedShip.DroneBayItems.Values
                                strHTML.Append("<tr><td colspan=2 bgcolor=#333333>" & dbi.Quantity.ToString("N0") & "x " & dbi.DroneType.Name & "</td></tr>")
                            Next
                        End If

                        strHTML.Append("</table>")
                        strHTML.Append("</td>")

                        ' Write stats
                        strHTML.Append("<td width=50% valign=top>")

                        ' Write fitting stats
                        strHTML.Append("<table width=100% border=1>")
                        strHTML.Append("<tr bgcolor=#111111><td colspan=5><b>Fitting</b></td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>CPU</b></td><td colspan=4>" & _currentFitting.FittedShip.CPUUsed.ToString("N2") & " / " & _currentFitting.FittedShip.CPU.ToString("N2") & "</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>PG</b></td><td colspan=4>" & _currentFitting.FittedShip.PGUsed.ToString("N2") & " / " & _currentFitting.FittedShip.PG.ToString("N2") & "</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Calibration</b></td><td colspan=4>" & _currentFitting.FittedShip.CalibrationUsed.ToString("N2") & " / " & _currentFitting.FittedShip.Calibration.ToString("N2") & "</td></tr>")
                        strHTML.Append("</table>")

                        ' Write defence stats
                        strHTML.Append("<table width=100% border=1>")
                        strHTML.Append("<tr bgcolor=#111111><td colspan=5><b>Defence</b></td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Shield HP</b></td><td colspan=2>" & _currentFitting.FittedShip.ShieldCapacity.ToString("N0") & " (Effective: " & _currentFitting.FittedShip.EffectiveShieldHP.ToString("N0") & ")</td><td width=20%><b>Recharge</b></td><td>" & _currentFitting.FittedShip.ShieldRecharge.ToString("N2") & "s</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Armor HP</b></td><td colspan=4>" & _currentFitting.FittedShip.ArmorCapacity.ToString("N0") & " (Effective: " & _currentFitting.FittedShip.EffectiveArmorHP.ToString("N0") & ")</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Hull HP</b></td><td colspan=4>" & _currentFitting.FittedShip.StructureCapacity.ToString("N0") & " (Effective: " & _currentFitting.FittedShip.EffectiveStructureHP.ToString("N0") & ")</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Total EHP</b></td><td colspan=4>" & _currentFitting.FittedShip.EffectiveHP.ToString("N0") & " (Eve: " & _currentFitting.FittedShip.EveEffectiveHP.ToString("N0") & ")</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Tank Ability</b></td><td colspan=4>" & CDbl(_currentFitting.FittedShip.Attributes(10062)).ToString("N2") & " DPS</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Shield Resists</b></td><td width=20%>EM: " & _currentFitting.FittedShip.ShieldEMResist.ToString("N2") & "%</td> <td width=20%>Exp: " & _currentFitting.FittedShip.ShieldExResist.ToString("N2") & "%</td><td width=20%>Kin: " & _currentFitting.FittedShip.ShieldKiResist.ToString("N2") & "%</td><td width=20%>Th: " & _currentFitting.FittedShip.ShieldThResist.ToString("N2") & "%</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Armor Resists</b></td><td width=20%>EM: " & _currentFitting.FittedShip.ArmorEMResist.ToString("N2") & "%</td> <td width=20%>Exp: " & _currentFitting.FittedShip.ArmorExResist.ToString("N2") & "%</td><td width=20%>Kin: " & _currentFitting.FittedShip.ArmorKiResist.ToString("N2") & "%</td><td width=20%>Th: " & _currentFitting.FittedShip.ArmorThResist.ToString("N2") & "%</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Hull Resists</b></td><td width=20%>EM: " & _currentFitting.FittedShip.StructureEMResist.ToString("N2") & "%</td> <td width=20%>Exp: " & _currentFitting.FittedShip.StructureExResist.ToString("N2") & "%</td><td width=20%>Kin: " & _currentFitting.FittedShip.StructureKiResist.ToString("N2") & "%</td><td width=20%>Th: " & _currentFitting.FittedShip.StructureThResist.ToString("N2") & "%</td></tr>")
                        strHTML.Append("</table>")

                        ' Write capacitor stats
                        strHTML.Append("<table width=100% border=1>")
                        strHTML.Append("<tr bgcolor=#111111><td colspan=4><b>Capacitor</b></td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Capacity</b></td><td width=30%>" & _currentFitting.FittedShip.CapCapacity.ToString("N2") & " GJ</td>")
                        strHTML.Append("<td width=20%><b>Recharge</b></td><td width=30%>" & _currentFitting.FittedShip.CapRecharge.ToString("N2") & " s</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Peak Recharge</b></td><td width=30%>" & (Settings.HQFSettings.CapRechargeConstant * _currentFitting.FittedShip.CapCapacity / _currentFitting.FittedShip.CapRecharge).ToString("N2") & " GJ/s</td>")
                        strHTML.Append("<td width=20%><b>Stability</b></td><td width=30%>")
                        Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(_currentFitting.FittedShip, False)
                        If csr.CapIsDrained = False Then
                            strHTML.Append("Stable at " & (csr.MinimumCap / _currentFitting.FittedShip.CapCapacity * 100).ToString("N2") & "%")
                        Else
                            strHTML.Append("Lasts " & Core.SkillFunctions.TimeToString(csr.TimeToDrain, False))
                        End If
                        strHTML.Append("</td></tr>")
                        strHTML.Append("</table>")

                        ' Write Damage Stats
                        strHTML.Append("<table width=100% border=1>")
                        strHTML.Append("<tr bgcolor=#111111><td colspan=4><b>Damage</b></td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Volley Damage</b></td><td width=30%>" & _currentFitting.FittedShip.TotalVolley.ToString("N2") & "</td>")
                        strHTML.Append("<td width=20%><b>DPS</b></td><td width=30%>" & _currentFitting.FittedShip.TotalDPS.ToString("N2") & "</td></tr>")
                        strHTML.Append("</table>")

                        ' Write Targeting Stats
                        strHTML.Append("<table width=100% border=1>")
                        strHTML.Append("<tr bgcolor=#111111><td colspan=4><b>Targeting</b></td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Max Range</b></td><td width=30%>" & _currentFitting.FittedShip.MaxTargetRange.ToString("N0") & " m</td>")
                        strHTML.Append("<td width=20%><b>Scan Resolution</b></td><td width=30%>" & _currentFitting.FittedShip.ScanResolution.ToString("N2") & " mm</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Signature Radius</b></td><td width=30%>" & _currentFitting.FittedShip.SigRadius.ToString("N0") & " m</td>")
                        strHTML.Append("<td width=20%><b>Max Targets</b></td><td width=30%>" & _currentFitting.FittedShip.MaxLockedTargets.ToString("N0") & "</td></tr>")
                        strHTML.Append("</table>")

                        ' Write Propulsion Stats
                        strHTML.Append("<table width=100% border=1>")
                        strHTML.Append("<tr bgcolor=#111111><td colspan=4><b>Propulsion</b></td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Max Velocity</b></td><td width=30%>" & _currentFitting.FittedShip.MaxVelocity.ToString("N2") & " m/s</td>")
                        strHTML.Append("<td width=20%><b>Warp Velocity</b></td><td width=30%>" & _currentFitting.FittedShip.WarpSpeed.ToString("N2") & " au/s</td></tr>")
                        strHTML.Append("<tr bgcolor=#333333><td width=20%><b>Inertia</b></td><td width=30%>" & _currentFitting.FittedShip.Inertia.ToString("N5") & "</td>")
                        strHTML.Append("<td width=20%><b>Warp Align Time</b></td><td width=30%>" & (-Math.Log(0.25) * _currentFitting.FittedShip.Inertia * _currentFitting.FittedShip.Mass / 1000000).ToString("N2") & " s</td></tr>")
                        strHTML.Append("</table>")

                        strHTML.Append("</td></tr>")
                        strHTML.Append("</table>")
                        strHTML.Append("Paste New Fitting: ")
                        strHTML.Append("<input type=""hidden"" name=""fitting"" value=""" & fittingURL & """>")
                        strHTML.Append("<input type=""text"" name=""fitting2"">")
                        strHTML.Append("<input type=""submit"" value=""Recalculate""></form>")

                        strHTML.Append("<form method=""GET"" action=""/EveHQFitter/SaveEveFitting"">")
                        strHTML.Append("Fitting Name: ")
                        strHTML.Append("<input type=""text"" name=""fittingname"" value=""" & fittingName & """>")
                        strHTML.Append("<input type=""submit"" value=""Save Fitting""></form>")

                    End If
                Catch e As Exception
                    strHTML.Append("<p>Unable to parse fitting: " & e.Message & "</p>")
                End Try
            End If
            Return strHTML.ToString
        End Function

        Shared Function SaveEveFitting(ByVal context As Net.HttpListenerContext) As String
            Dim strHTML As New StringBuilder
            If context.Request.QueryString.Count = 0 Or Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("fittingname")) = "" Then
                strHTML.Append("<p>Fitting name is not permitted - go back and re-enter an appropriate name.</p>")
            Else
                Try
                    Dim fittingNameExists As Boolean = False
                    Dim fittingName As String = Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("fittingname"))
                    If Fittings.FittingList.ContainsKey(_currentFitting.ShipName & ", " & fittingName) = True Then
                        fittingNameExists = True
                        Dim newFittingName As String
                        Dim revision As Integer = 1
                        Do
                            revision += 1
                            newFittingName = fittingName & " " & revision.ToString
                        Loop Until Fittings.FittingList.ContainsKey(_currentFitting.ShipName & ", " & newFittingName) = False
                        fittingName = newFittingName
                    End If
                    Dim newFitting As Fitting = _currentFitting.Clone
                    newFitting.FittingName = fittingName
                    newFitting.UpdateFittingFromBaseShip()
                    ' Let's save the fitting
                    Fittings.FittingList.Add(newFitting.KeyName, newFitting)
                    HQFEvents.StartUpdateFittingList = True
                    If fittingNameExists = False Then
                        strHTML.Append("<p>Fitting successfully saved as " & newFitting.FittingName & "!</p>")
                    Else
                        strHTML.Append("<p>The fitting '" & Web.HttpUtility.UrlDecode(context.Request.QueryString.Item("fittingname")) & "' already exists, so the new fitting has been saved as '" & newFitting.FittingName & "'</p>")
                    End If
                    strHTML.Append("Click <a href=""javascript:history.go(-1)"">here</a> to go back to the fitting screen.")
                Catch ex As Exception
                    strHTML.Append("<p>There was an error saving the fitting. The error was: " & ex.Message & "<br /><br />" & ex.StackTrace & "</p>")
                End Try
            End If
            Return strHTML.ToString
        End Function

        Shared Function HQFPage2(ByVal context As Net.HttpListenerContext) As String
            Dim strHTML As New StringBuilder
            Return strHTML.ToString
        End Function

    End Class
End Namespace