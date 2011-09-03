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

''' <summary>
''' Class for handling proxy server functions
''' Used here so plug-ins need not reference the EveAPI class
''' </summary>
''' <remarks></remarks>
Public Class ProxyServerFunctions

    ''' <summary>
    ''' Routine to set up a web proxy for a HTTPWebRequest
    ''' </summary>
    ''' <param name="request">The HTTPWebRequest for which a WebProxy needs configuring</param>
    ''' <remarks></remarks>
    Public Shared Sub SetupWebProxy(ByRef request As HttpWebRequest)
        Dim ProxyServer As EveHQ.EveAPI.RemoteProxyServer = EveHQ.Core.HQ.RemoteProxy
        If ProxyServer IsNot Nothing Then
            If ProxyServer.ProxyRequired = True Then
                request.Proxy = ProxyServer.SetupWebProxy
            End If
        End If
    End Sub

End Class
