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
Imports EveHQ.EveAPI
Imports System.Xml

Public Class EveServer
    Public Server As Integer = Servers.Tranquility
    Public ServerName As String = "Tranquility"
    Public Version As String
    Public Players As Integer
    Public Codename As String
    Public Status As Integer = ServerStatus.Unknown
    Public LastStatus As Integer = ServerStatus.Unknown
    Public StatusText As String
    Public LastChecked As Date

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
        Try
            Dim apiReq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
            Dim statusXML As XmlDocument = apiReq.GetAPIXML(APITypes.ServerStatus, APIReturnMethods.BypassCache)
            If statusXML IsNot Nothing Then
                Dim statusDetails As XmlNodeList = statusXML.SelectNodes("/eveapi/result")
                Dim serverIsUp As Boolean = CBool(statusDetails(0).ChildNodes(0).InnerText)
                Dim serverPlayers As Integer = CInt(statusDetails(0).ChildNodes(1).InnerText)
                If serverIsUp = True Then
                    Status = ServerStatus.Up
                    Players = serverPlayers
                Else
                    Status = ServerStatus.Down
                    Players = 0
                End If
            Else
                Version = ""
                Players = 0
                Codename = ""
                Status = ServerStatus.Unknown
                StatusText = "Server Status Unknown"
            End If
            LastChecked = Now
        Catch e As Exception
            Version = ""
            Players = 0
            Codename = ""
            Status = ServerStatus.Unknown
            StatusText = "Server Status Unknown"
            LastChecked = Now
        End Try

    End Sub
End Class
