Imports System.Text

Public Class IGBData
    Shared timeStart, timeEnd As DateTime
    Shared timeTaken As TimeSpan

    Shared Function Response(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        timeStart = Now
        strHTML.Append(EveHQ.Core.IGB.IGBHTMLHeader(context, "EveHQVoid"))
        strHTML.Append(VoidMenu(context))
        Select Case context.Request.Url.AbsolutePath.ToUpper
            Case "/EVEHQVOID", "/EVEHQVOID/"
                strHTML.Append(MainPage(context))
            Case "/EVEHQVOID/WHLOOKUP", "/EVEHQVOID/WHLOOKUP/"
                strHTML.Append(WHLookupPage(context))
            Case "/EVEHQVOID/WLOOKUP", "/EVEHQVOID/WLOOKUP/"
                strHTML.Append(WLookupPage(context))
        End Select
        strHTML.Append(EveHQ.Core.IGB.IGBHTMLFooter(context))
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
                Dim WHSystem As WormholeSystem = VoidData.WormholeSystems(systemName)
                strHTML.Append("<p>Wormhole Class: " & WHSystem.WClass & "</p>")
                If WHSystem.WEffect = "" Then
                    strHTML.Append("<p>Wormhole Effect: None</p>")
                Else
                    strHTML.Append("<p>Wormhole Effect: " & WHSystem.WEffect & "</p>")
                    strHTML.Append("<table border=1><tr><td colspan=2 align=center>Wormhole Effects</td></tr>")
                    Dim modName As String = ""
                    If WHSystem.WEffect = "Red Giant" Then
                        modName = WHSystem.WEffect & " Beacon Class " & WHSystem.WClass
                    Else
                        modName = WHSystem.WEffect & " Effect Beacon Class " & WHSystem.WClass
                    End If
                    Dim WHEffects As New SortedList(Of String, String)
                    ' Parse the WHEffects resource
                    WHEffects = New SortedList(Of String, String)
                    Dim Effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
                    For Each Effect As String In Effects
                        If Effect <> "" Then
                            Dim EffectData() As String = Effect.Split(",".ToCharArray)
                            If WHEffects.ContainsKey(EffectData(0)) = False Then
                                WHEffects.Add(EffectData(0), EffectData(10))
                            End If
                        End If
                    Next
                    ' Establish the effects
                    Dim EffectList As New SortedList(Of String, Double)
                    Dim SysEffects As WormholeEffect = VoidData.WormholeEffects(modName)
                    For Each att As String In SysEffects.Attributes.Keys
                        If WHEffects.ContainsKey(att) = True Then
                            EffectList.Add(WHEffects(att), SysEffects.Attributes(att))
                        End If
                    Next
                    For Each Effect As String In EffectList.Keys
                        Dim effectColor As String = ""
                        Dim value As Double = CDbl(EffectList(Effect))
                        If value < 5 And value > -5 Then
                            If value < 1 Or Effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & Effect & "</td><td width=100px align=center>" & EffectList(Effect).ToString("N2") & " x</td></tr>")
                        Else
                            If value < 0 Or Effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & Effect & "</td><td width=100px align=center>" & EffectList(Effect).ToString("N2") & " %</td></tr>")
                        End If
                    Next
                    strHTML.Append("</table>")
                End If
            End If
        End If
        Return strHTML.ToString
    End Function

    Shared Function WHLookupPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append("<p>Please select a wormhole type from the list:<p>")
        strHTML.Append("<form method=""GET"" action=""/EveHQVoid/WHLookup"">")
        strHTML.Append("<p>Wormhole Type:&nbsp;&nbsp;&nbsp;<select name='WH' style='width: 200px;'>")
        Dim pilotName As String = ""
        For Each WH As Void.WormHole In VoidData.Wormholes.Values
            strHTML.Append("<option")
            If context.Request.QueryString.Count = 1 Then
                If WH.Name = context.Request.QueryString.Item("WH") Then
                    strHTML.Append(" selected='selected'")
                End If
            End If
            strHTML.Append(">" & WH.Name & "</option>")
        Next
        strHTML.Append("</select><input type='submit' value='Get WH Data'></p></form>")
        If context.Request.QueryString.Count = 1 Then
            Dim WHName As String = context.Request.QueryString.Item("WH")
            strHTML.Append("<p>Data for the " & WHName & " wormhole:</p>")
            If VoidData.Wormholes.ContainsKey(WHName) = True Then
                Dim WH As Void.WormHole = VoidData.Wormholes(WHName)
                If WH.Name <> "K162" Then
                    strHTML.Append("<table>")
                    strHTML.Append("<tr><td width=150px>Target System Class:</td><td width=200px>" & WH.TargetClass)
                    Select Case CInt(WH.TargetClass)
                        Case 1 To 6
                            strHTML.Append(" (Wormhole Class " & WH.TargetClass & ")")
                        Case 7
                            strHTML.Append(" (High Security Space)")
                        Case 8
                            strHTML.Append(" (Low Security Space)")
                        Case 9
                            strHTML.Append(" (Null Security Space)")
                    End Select
                    strHTML.Append("</td></tr>")
                    strHTML.Append("<tr><td>Max Jumpable Mass:</td><td>" & CLng(WH.MaxJumpableMass).ToString("N0") & " kg</td></tr>")
                    strHTML.Append("<tr><td>Max Total Mass:</td><td>" & CLng(WH.MaxMassCapacity).ToString("N0") & " kg</td></tr>")
                    strHTML.Append("<tr><td>Stability Window:</td><td>" & (CDbl(WH.MaxStabilityWindow) / 60).ToString("N0") & " hours</td></tr>")
                    strHTML.Append("</table>")
                Else
                    strHTML.Append("<p>Wormhole K162 is a return wormhole</p>")
                End If
            Else
                strHTML.Append("<p>Wormhole " & WHName & " not found!!</p>")
            End If
        End If
        Return strHTML.ToString
    End Function

    Shared Function WLookupPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append("<p>Please select a wormhole system from the list:<p>")
        strHTML.Append("<form method=""GET"" action=""/EveHQVoid/WLookup"">")
        strHTML.Append("<p>Wormhole System:&nbsp;&nbsp;&nbsp;<select name='WH' style='width: 200px;'>")
        Dim pilotName As String = ""
        For Each WH As Void.WormholeSystem In VoidData.WormholeSystems.Values
            strHTML.Append("<option")
            If context.Request.QueryString.Count = 1 Then
                If WH.Name = context.Request.QueryString.Item("WH") Then
                    strHTML.Append(" selected='selected'")
                End If
            End If
            strHTML.Append(">" & WH.Name & "</option>")
        Next
        strHTML.Append("</select><input type='submit' value='Get WH Data'></p></form>")
        If context.Request.QueryString.Count = 1 Then
            Dim SystemName As String = context.Request.QueryString.Item("WH")
            If VoidData.WormholeSystems.ContainsKey(systemName) = False Then
                ' Not a wormhole system
                strHTML.Append("<p>This is not a wormhole system and therefore the relevant wormhole system data cannot be displayed for you.</p>")
            Else
                ' Fetch the wormhole data
                Dim WHSystem As WormholeSystem = VoidData.WormholeSystems(SystemName)
                strHTML.Append("<p>Data for the " & SystemName & " system:</p>")
                strHTML.Append("<p>Wormhole Class: " & WHSystem.WClass & "</p>")
                If WHSystem.WEffect = "" Then
                    strHTML.Append("<p>Wormhole Effect: None</p>")
                Else
                    strHTML.Append("<p>Wormhole Effect: " & WHSystem.WEffect & "</p>")
                    strHTML.Append("<table border=1><tr><td colspan=2 align=center>Wormhole Effects</td></tr>")
                    Dim modName As String = ""
                    If WHSystem.WEffect = "Red Giant" Then
                        modName = WHSystem.WEffect & " Beacon Class " & WHSystem.WClass
                    Else
                        modName = WHSystem.WEffect & " Effect Beacon Class " & WHSystem.WClass
                    End If
                    Dim WHEffects As New SortedList(Of String, String)
                    ' Parse the WHEffects resource
                    WHEffects = New SortedList(Of String, String)
                    Dim Effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
                    For Each Effect As String In Effects
                        If Effect <> "" Then
                            Dim EffectData() As String = Effect.Split(",".ToCharArray)
                            If WHEffects.ContainsKey(EffectData(0)) = False Then
                                WHEffects.Add(EffectData(0), EffectData(10))
                            End If
                        End If
                    Next
                    ' Establish the effects
                    Dim EffectList As New SortedList(Of String, Double)
                    Dim SysEffects As WormholeEffect = VoidData.WormholeEffects(modName)
                    For Each att As String In SysEffects.Attributes.Keys
                        If WHEffects.ContainsKey(att) = True Then
                            EffectList.Add(WHEffects(att), SysEffects.Attributes(att))
                        End If
                    Next
                    For Each Effect As String In EffectList.Keys
                        Dim effectColor As String = ""
                        Dim value As Double = CDbl(EffectList(Effect))
                        If value < 5 And value > -5 Then
                            If value < 1 Or Effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & Effect & "</td><td width=100px align=center>" & EffectList(Effect).ToString("N2") & " x</td></tr>")
                        Else
                            If value < 0 Or Effect.EndsWith("Penalty") Then
                                effectColor = "#FF0000"
                            Else
                                effectColor = "#00FF00"
                            End If
                            strHTML.Append("<tr style=""color: " & effectColor & """><td width=300px>" & Effect & "</td><td width=100px align=center>" & EffectList(Effect).ToString("N2") & " %</td></tr>")
                        End If
                    Next
                    strHTML.Append("</table>")
                End If
            End If
        End If
        Return strHTML.ToString
    End Function
End Class
