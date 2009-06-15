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
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class EveServer
    Public Server As Integer = EveServer.Servers.Tranquility
    Public ServerName As String
    Public Version As String
    Public Players As Integer
    Public Codename As String
    Public Status As Integer = EveServer.ServerStatus.Unknown
    Public LastStatus As Integer = EveServer.ServerStatus.Unknown
    Public StatusText As String
    Public LastChecked As Date
    Public RawResponse As String
    Public Event GotStatus()

    Public Enum ServerStatus As Integer
        Up = -1
        Down = 0
        Starting = 1
        Unknown = 2
        Shutting = 3
        Full = 4
        ProxyDown = 5
    End Enum

    Public Enum Servers As Integer
        Tranquility = 0
        Singularity = 1
    End Enum

    Public Sub GetServerStatus()
        Dim EveTCPClient As New TcpClient
        Dim EveStream As NetworkStream
        Dim EveHost As String = ""
        Dim EvePort As Integer = 26000
        Dim EveData As String = ""

        Select Case Server
            Case EveServer.Servers.Tranquility
                EveHost = "87.237.38.200"
                ServerName = "Tranquility"
            Case EveServer.Servers.Singularity
                EveHost = "87.237.38.50"
                ServerName = "Singularity"
        End Select

        Try
            With EveTCPClient
                .NoDelay = True
                .Connect(EveHost, EvePort)
            End With
        Catch e As SocketException
            Version = ""
            Players = 0
            Codename = ""
            Status = EveServer.ServerStatus.Down
            StatusText = "Could not connect to server"
            LastChecked = Now
            Call WriteToLogFile(False)
            RaiseEvent GotStatus()
            Exit Sub
        End Try

        Try
            EveStream = EveTCPClient.GetStream
            EveData = ReadBuffer(EveStream)
            RawResponse = EveData
            ' Terminate Connection
            EveStream.Close()
            EveTCPClient.Close()
        Catch e As Exception
            Version = ""
            Players = 0
            Codename = ""
            Status = EveServer.ServerStatus.Unknown
            StatusText = "Error reading server data"
            LastChecked = Now
            Call WriteToLogFile(False)
            RaiseEvent GotStatus()
            Exit Sub
        End Try

        Try
            Call ParseEveData(EveData)
            Call WriteToLogFile(False)
            'RaiseEvent GotStatus()
        Catch e As Exception
            Version = ""
            Players = 0
            Codename = ""
            Status = EveServer.ServerStatus.Unknown
            StatusText = "Server Status Unknown"
            LastChecked = Now
            RaiseEvent GotStatus()
            Exit Sub
        End Try

    End Sub
    Private Function ReadBuffer(ByVal MailStream As NetworkStream) As String
        Dim RecdData(1024) As Byte
        Dim ReadData As String = ""
        Try
            ReadData = ""
            MailStream.Read(RecdData, 0, CInt(RecdData.Length))
            ReadData = ReadData & Encoding.Default.GetString(RecdData).TrimEnd(Chr(0))
        Catch f As Exception
            'MsgBox(f.Message, MsgBoxStyle.Critical, "Error receiving data")
            Return ""
            Exit Function
        End Try
        Return ReadData
    End Function
    Private Sub ParseEveData(ByVal EveData As String)
        ' Simple version required just to get data out of the server stream
        Try
            If InStr(EveData, "Starting up") <> 0 Then
                Dim s1, s2 As Integer
                Status = EveServer.ServerStatus.Starting
                s1 = InStr(EveData, "Starting")
                s2 = InStr(s1, EveData, ")", CompareMethod.Text)
                Version = ""
                Players = 0
                Codename = ""
                StatusText = Mid$(EveData, s1, s2 - s1 + 1)
                LastChecked = Now
            Else
                If EveData.Contains("Full cluster") = True Then
                    Dim s1, s2, s3 As Integer
                    Status = EveServer.ServerStatus.Full
                    s1 = InStr(EveData, "Full cluster")
                    s2 = InStr(s1, EveData, "(")
                    s3 = InStr(s1, EveData, ")", CompareMethod.Text)
                    Version = ""
                    Players = CInt(EveData.Substring(s2, s3 - (s2 + 1)))
                    Codename = ""
                    StatusText = EveData.Substring(s1 - 1, s3 - s1 + 1)
                    LastChecked = Now
                Else
                    If EveData.Contains("The cluster is shutting") = True Then
                        Dim s1, s2 As Integer
                        Status = EveServer.ServerStatus.Shutting
                        s1 = InStr(EveData, "The cluster is shutting")
                        s2 = InStr(s1, EveData, "down", CompareMethod.Text)
                        Version = ""
                        Players = 0
                        Codename = ""
                        StatusText = Mid$(EveData, s1, s2 - s1 + 4)
                        LastChecked = Now
                    Else
                        If EveData.Contains("Proxy not connected") = True Then
                            Version = ""
                            Players = 0
                            Codename = ""
                            StatusText = "Proxy not connected"
                            LastChecked = Now
                        Else
                            Status = EveServer.ServerStatus.Up
                            Dim startpoint As Integer = 17
                            Dim currentchar As Integer = startpoint
                            ' read unknown value
                            If Asc(Mid$(EveData, currentchar, 1)) = 5 Then
                                currentchar = currentchar + 3
                            Else
                                currentchar = currentchar + 2
                            End If
                            ' Get players
                            Select Case Asc(Mid$(EveData, currentchar, 1))
                                Case 4
                                    Players = CalcInt32(Mid$(EveData, currentchar + 1, 4))
                                    currentchar = currentchar + 5
                                Case 5
                                    Players = CalcInt16(Mid$(EveData, currentchar + 1, 2))
                                    currentchar = currentchar + 3
                                Case 6
                                    Players = Asc(Mid$(EveData, currentchar + 1, 1))
                                    currentchar = currentchar + 2
                            End Select
                            ' Get floating version
                            Dim FloatVer As Double = CalcFloat64(Mid$(EveData, currentchar + 1, 8))
                            currentchar = currentchar + 9
                            'Dim PatchLev As Integer = CalcInt16(Mid$(EveData, currentchar + 1, 2))
                            Dim PatchLev As Integer = CalcInt32(Mid$(EveData, currentchar + 1, 4))
                            Version = FloatVer.ToString & "." & PatchLev.ToString
                            currentchar = currentchar + 3
                            ' Get Codename
                            Dim codelength As Integer = Asc(Mid$(EveData, currentchar + 1, 1))
                            Codename = EveData.Substring(currentchar + 1, codelength)
                            StatusText = "OK"
                            LastChecked = Now
                        End If
                    End If
                End If
            End If
        Catch e As Exception
        End Try
    End Sub
    Private Function ReverseString(ByVal data As String) As String
        ReverseString = ""
        For a As Integer = 1 To Len(data)
            ReverseString = ReverseString & Mid$(data, a, 1)
        Next
        Return ReverseString
    End Function
    Private Function CalcInt32(ByVal data As String) As Int32
        For a As Integer = 0 To 3
            CalcInt32 = CInt(CalcInt32 + (Asc(data.Substring(a, 1))) * (256 ^ a))
        Next
    End Function
    Private Function CalcInt16(ByVal data As String) As Int16
        For a As Integer = 0 To 1
            CalcInt16 = CShort(CalcInt16 + (Asc(data.Substring(a, 1))) * (256 ^ a))
        Next
    End Function
    Private Function CalcFloat64(ByVal data As String) As Double
        Try
            Dim EV, EveVersion As String
            EveVersion = ""
            For b As Integer = 0 To 7
                EV = Convert.ToString((Asc(data.Substring(7 - b, 1))), 2)
                If Len(EV) < 8 Then
                    Do
                        EV = "0" & EV
                    Loop Until Len(EV) = 8
                End If
                EveVersion = EveVersion & EV
            Next

            ' Break the binary number into components
            Dim EV1, EV2, EV3 As String
            EV1 = EveVersion.Substring(0, 1)
            EV2 = EveVersion.Substring(1, 11)
            EV3 = EveVersion.Substring(12, 52)

            ' Convert the exponent element to an integer and normalise it
            Dim exp As Integer = Convert.ToInt32(EV2, 2)
            exp = exp - 1023

            ' Calculate the mantissa element and shift it according to the value of the exponent
            CalcFloat64 = 1
            For a As Integer = 1 To 52
                CalcFloat64 = CalcFloat64 + Val(EV3.Substring(a - 1, 1)) * (1 / (2 ^ a))
            Next
            CalcFloat64 = CalcFloat64 * (2 ^ exp)
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Private Sub WriteToLogFile(ByVal Err As Boolean)
        Try
            Dim file As System.IO.StreamWriter
            Dim fw As String
            file = My.Computer.FileSystem.OpenTextFileWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "EveServer.log"), True)
            fw = Now & ", " & ServerName & ", " & StatusText & ", " & Version & ", " & Players
            file.WriteLine(fw)
            file.Flush()
            file.Close()
        Catch e As Exception
            Exit Sub
        End Try
    End Sub
End Class
