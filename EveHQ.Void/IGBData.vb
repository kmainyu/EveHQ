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


Imports System.Text

Public Class IGBData
    
    Shared Function Response(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append(Core.IGB.IGBHTMLHeader(context, "EveHQVoid", 0))
        strHTML.Append(VoidMenu(context))
        Select Case context.Request.Url.AbsolutePath.ToUpper
            Case "/EVEHQVOID", "/EVEHQVOID/"
                strHTML.Append(MainPage(context))
            Case "/EVEHQVOID/WHLOOKUP", "/EVEHQVOID/WHLOOKUP/"
                strHTML.Append(WormholeLookupPage(context))
            Case "/EVEHQVOID/WLOOKUP", "/EVEHQVOID/WLOOKUP/"
                strHTML.Append(WLookupPage(context))
        End Select
        strHTML.Append(Core.IGB.IGBHTMLFooter(context))
        Return strHTML.ToString
    End Function

    Shared Function VoidMenu(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append("<a href=/EveHQVoid>Void Home</a>  |  <a href=/EveHQVoid/WHLookup>Wormhole Lookup</a>  |  <a href=/EveHQVoid/WLookup>Wormhole System Lookups</a>")
        Return strHTML.ToString
    End Function

    Shared Function MainPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        ' Check if we are trusted and get the Eve system name
        If context.Request.Headers("EVE_SOLARSYSTEMNAME") = "" Then
            strHTML.Append("<p>This website is not trusted so cannot get the current system data.</p>")
        Else
            Dim systemName As String = context.Request.Headers("EVE_SOLARSYSTEMNAME")
            strHTML.Append("<p>You are currently located in the " & systemName & " system.</p>")
            If VoidData.WormholeSystems.ContainsKey(systemName) = False Then
                ' Not a wormhole system
                strHTML.Append("<p>This is not a wormhole system and therefore the relevant wormhole system data cannot be displayed for you.</p>")
            Else
                ' Fetch the wormhole data
                Dim whSystem As WormholeSystem = VoidData.WormholeSystems(systemName)
                strHTML.Append("<p>Wormhole Class: " & whSystem.WClass & "</p>")
                If whSystem.WEffect = "" Then
                    strHTML.Append("<p>Wormhole Effect: None</p>")
                Else
                    strHTML.Append("<p>Wormhole Effect: " & whSystem.WEffect & "</p>")
                    strHTML.Append("<table border=1><tr><td colspan=2 align=center>Wormhole Effects</td></tr>")
                    Dim modName As String
                    If whSystem.WEffect = "Red Giant" Then
                        modName = whSystem.WEffect & " Beacon Class " & whSystem.WClass
                    Else
                        modName = whSystem.WEffect & " Effect Beacon Class " & whSystem.WClass
                    End If
                    Dim whEffects As SortedList(Of String, String)
                    ' Parse the WHEffects resource
                    whEffects = New SortedList(Of String, String)
                    Dim effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
                    For Each effect As String In effects
                        If effect <> "" Then
                            Dim effectData() As String = effect.Split(",".ToCharArray)
                            If whEffects.ContainsKey(effectData(0)) = False Then
                                whEffects.Add(effectData(0), effectData(10))
                            End If
                        End If
                    Next
                    ' Establish the effects
                    Dim effectList As New SortedList(Of String, Double)
                    Dim sysEffects As WormholeEffect = VoidData.WormholeEffects(modName)
                    For Each att As String In sysEffects.Attributes.Keys
                        If whEffects.ContainsKey(att) = True Then
                            effectList.Add(whEffects(att), sysEffects.Attributes(att))
                        End If
                    Next
                    For Each effect As String In effectList.Keys
                        Dim effectColor As String
                        Dim value As Double = CDbl(effectList(effect))
                        If value < 5 And value > -5 Then
                            If value < 1 Or effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & effect & "</td><td width=100px align=center>" & effectList(effect).ToString("N2") & " x</td></tr>")
                        Else
                            If value < 0 Or effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & effect & "</td><td width=100px align=center>" & effectList(effect).ToString("N2") & " %</td></tr>")
                        End If
                    Next
                    strHTML.Append("</table>")
                End If
            End If
        End If
        Return strHTML.ToString
    End Function

    Shared Function WormholeLookupPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append("<p>Please select a wormhole type from the list:<p>")
        strHTML.Append("<form method=""GET"" action=""/EveHQVoid/WHLookup"">")
        strHTML.Append("<p>Wormhole Type:&nbsp;&nbsp;&nbsp;<select name='WH' style='width: 200px;'>")
        For Each wh As WormHole In VoidData.Wormholes.Values
            strHTML.Append("<option")
            If context.Request.QueryString.Count = 1 Then
                If wh.Name = context.Request.QueryString.Item("WH") Then
                    strHTML.Append(" selected='selected'")
                End If
            End If
            strHTML.Append(">" & wh.Name & "</option>")
        Next
        strHTML.Append("</select><input type='submit' value='Get WH Data'></p></form>")
        If context.Request.QueryString.Count = 1 Then
            Dim whName As String = context.Request.QueryString.Item("WH")
            strHTML.Append("<p>Data for the " & whName & " wormhole:</p>")
            If VoidData.Wormholes.ContainsKey(whName) = True Then
                Dim wh As WormHole = VoidData.Wormholes(whName)
                If wh.Name <> "K162" Then
                    strHTML.Append("<table>")
                    strHTML.Append("<tr><td width=150px>Target System Class:</td><td width=200px>" & wh.TargetClass)
                    Select Case CInt(wh.TargetClass)
                        Case 1 To 6
                            strHTML.Append(" (Wormhole Class " & wh.TargetClass & ")")
                        Case 7
                            strHTML.Append(" (High Security Space)")
                        Case 8
                            strHTML.Append(" (Low Security Space)")
                        Case 9
                            strHTML.Append(" (Null Security Space)")
                    End Select
                    strHTML.Append("</td></tr>")
                    strHTML.Append("<tr><td>Max Jumpable Mass:</td><td>" & CLng(wh.MaxJumpableMass).ToString("N0") & " kg</td></tr>")
                    strHTML.Append("<tr><td>Max Total Mass:</td><td>" & CLng(wh.MaxMassCapacity).ToString("N0") & " kg</td></tr>")
                    strHTML.Append("<tr><td>Stability Window:</td><td>" & (CDbl(wh.MaxStabilityWindow) / 60).ToString("N0") & " hours</td></tr>")
                    strHTML.Append("</table>")
                Else
                    strHTML.Append("<p>Wormhole K162 is a return wormhole</p>")
                End If
            Else
                strHTML.Append("<p>Wormhole " & whName & " not found!!</p>")
            End If
        End If
        Return strHTML.ToString
    End Function

    Shared Function WLookupPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append("<p>Please select a wormhole system from the list:<p>")
        strHTML.Append("<form method=""GET"" action=""/EveHQVoid/WLookup"">")
        strHTML.Append("<p>Wormhole System:&nbsp;&nbsp;&nbsp;<select name='WH' style='width: 200px;'>")
        For Each wh As WormholeSystem In VoidData.WormholeSystems.Values
            strHTML.Append("<option")
            If context.Request.QueryString.Count = 1 Then
                If wh.Name = context.Request.QueryString.Item("WH") Then
                    strHTML.Append(" selected='selected'")
                End If
            End If
            strHTML.Append(">" & wh.Name & "</option>")
        Next
        strHTML.Append("</select><input type='submit' value='Get WH Data'></p></form>")
        If context.Request.QueryString.Count = 1 Then
            Dim systemName As String = context.Request.QueryString.Item("WH")
            If VoidData.WormholeSystems.ContainsKey(systemName) = False Then
                ' Not a wormhole system
                strHTML.Append("<p>This is not a wormhole system and therefore the relevant wormhole system data cannot be displayed for you.</p>")
            Else
                ' Fetch the wormhole data
                Dim whSystem As WormholeSystem = VoidData.WormholeSystems(systemName)
                strHTML.Append("<p>Data for the " & systemName & " system:</p>")
                strHTML.Append("<p>Wormhole Class: " & whSystem.WClass & "</p>")
                If whSystem.WEffect = "" Then
                    strHTML.Append("<p>Wormhole Effect: None</p>")
                Else
                    strHTML.Append("<p>Wormhole Effect: " & whSystem.WEffect & "</p>")
                    strHTML.Append("<table border=1><tr><td colspan=2 align=center>Wormhole Effects</td></tr>")
                    Dim modName As String
                    If whSystem.WEffect = "Red Giant" Then
                        modName = whSystem.WEffect & " Beacon Class " & whSystem.WClass
                    Else
                        modName = whSystem.WEffect & " Effect Beacon Class " & whSystem.WClass
                    End If
                    Dim whEffects As SortedList(Of String, String)
                    ' Parse the WHEffects resource
                    whEffects = New SortedList(Of String, String)
                    Dim effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
                    For Each effect As String In effects
                        If effect <> "" Then
                            Dim effectData() As String = effect.Split(",".ToCharArray)
                            If whEffects.ContainsKey(effectData(0)) = False Then
                                whEffects.Add(effectData(0), effectData(10))
                            End If
                        End If
                    Next
                    ' Establish the effects
                    Dim effectList As New SortedList(Of String, Double)
                    Dim sysEffects As WormholeEffect = VoidData.WormholeEffects(modName)
                    For Each att As String In sysEffects.Attributes.Keys
                        If whEffects.ContainsKey(att) = True Then
                            effectList.Add(whEffects(att), sysEffects.Attributes(att))
                        End If
                    Next
                    For Each effect As String In effectList.Keys
                        Dim effectColor As String
                        Dim value As Double = CDbl(effectList(effect))
                        If value < 5 And value > -5 Then
                            If value < 1 Or effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & effect & "</td><td width=100px align=center>" & effectList(effect).ToString("N2") & " x</td></tr>")
                        Else
                            If value < 0 Or effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & effect & "</td><td width=100px align=center>" & effectList(effect).ToString("N2") & " %</td></tr>")
                        End If
                    Next
                    strHTML.Append("</table>")
                End If
            End If
        End If
        Return strHTML.ToString
    End Function
End Class
