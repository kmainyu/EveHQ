' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Xml

Public Class EveServer
    Public Server As Integer = EveServer.Servers.Tranquility
    Public ServerName As String = "Tranquility"
    Public Version As String
    Public Players As Integer
    Public Codename As String
    Public Status As Integer = EveServer.ServerStatus.Unknown
    Public LastStatus As Integer = EveServer.ServerStatus.Unknown
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
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            Dim StatusXML As XmlDocument = APIReq.GetAPIXML(EveAPI.APITypes.ServerStatus, EveAPI.APIReturnMethods.ReturnActual)
            If StatusXML IsNot Nothing Then
                Dim StatusDetails As XmlNodeList = StatusXML.SelectNodes("/eveapi/result")
                Dim ServerIsUp As Boolean = CBool(StatusDetails(0).ChildNodes(0).InnerText)
                Dim ServerPlayers As Integer = CInt(StatusDetails(0).ChildNodes(1).InnerText)
                If ServerIsUp = True Then
                    Status = EveServer.ServerStatus.Up
                    Players = ServerPlayers
                Else
                    Status = EveServer.ServerStatus.Down
                    Players = 0
                End If
            Else
                Version = ""
                Players = 0
                Codename = ""
                Status = EveServer.ServerStatus.Unknown
                StatusText = "Server Status Unknown"
            End If
            LastChecked = Now
        Catch e As Exception
            Version = ""
            Players = 0
            Codename = ""
            Status = EveServer.ServerStatus.Unknown
            StatusText = "Server Status Unknown"
            LastChecked = Now
        End Try

    End Sub
End Class
