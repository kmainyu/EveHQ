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

''' <summary>
''' Class for storing details of a proxy server to use for various connections to the web
''' </summary>
''' <remarks></remarks>
Public Class RemoteProxyServer
    Private cProxyRequired As Boolean = False
    Private cProxyServer As String = ""
    Private cProxyPort As Integer = 0
    Private cProxyUsername As String = ""
    Private cProxyPassword As String = ""
    Private cUseDefaultCredentials As Boolean = True
    Private cUseBasicAuthenticaion As Boolean = True

    ''' <summary>
    ''' Determines whether a Proxy Server should be used or not
    ''' </summary>
    ''' <value></value>
    ''' <returns>A boolean value indicating whether a Proxy Server is required to be used</returns>
    ''' <remarks></remarks>
    Public Property ProxyRequired() As Boolean
        Get
            Return cProxyRequired
        End Get
        Set(ByVal value As Boolean)
            cProxyRequired = value
        End Set
    End Property

    ''' <summary>
    ''' Holds the host name or IP address of the proxy server
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the host name or IP address of the proxy server</returns>
    ''' <remarks></remarks>
    Public Property ProxyServer() As String
        Get
            Return cProxyServer
        End Get
        Set(ByVal value As String)
            cProxyServer = value
        End Set
    End Property

    ''' <summary>
    ''' Holds the port number to use for the proxy server
    ''' </summary>
    ''' <value></value>
    ''' <returns>An integer value representing the port number to use for the proxy server</returns>
    ''' <remarks></remarks>
    Public Property ProxyPort() As Integer
        Get
            Return cProxyPort
        End Get
        Set(ByVal value As Integer)
            cProxyPort = value
        End Set
    End Property

    ''' <summary>
    ''' Determines whether default Windows logon information should be used for the proxy server
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating whether default Windows credentials are passed to the proxy server</returns>
    ''' <remarks></remarks>
    Public Property UseDefaultCredentials() As Boolean
        Get
            Return cUseDefaultCredentials
        End Get
        Set(ByVal value As Boolean)
            cUseDefaultCredentials = value
        End Set
    End Property

    ''' <summary>
    ''' Determines if the Proxy is using "Basic" authentication as opposed to "NTLM"
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UseBasicAuthentication() As Boolean
        Get
            Return cUseBasicAuthenticaion
        End Get
        Set(ByVal value As Boolean)
            cUseBasicAuthenticaion = value
        End Set
    End Property

    ''' <summary>
    ''' Holds the username of the credentials used to access the proxy server
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the username to access the proxy server</returns>
    ''' <remarks></remarks>
    Public Property ProxyUsername() As String
        Get
            Return cProxyUsername
        End Get
        Set(ByVal value As String)
            cProxyUsername = value
        End Set
    End Property

    ''' <summary>
    ''' Holes the password of the credentials used to access the proxy server
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the password to access the proxy server</returns>
    ''' <remarks></remarks>
    Public Property ProxyPassword() As String
        Get
            Return cProxyPassword
        End Get
        Set(ByVal value As String)
            cProxyPassword = value
        End Set
    End Property

    ''' <summary>
    ''' Creates an instance of a System.Net.WebProxy for use in web connections
    ''' </summary>
    ''' <returns>A System.Net.WebProxy instance</returns>
    ''' <remarks></remarks>
    Public Function SetupWebProxy() As System.Net.WebProxy
        Dim EveHQProxy As New System.Net.WebProxy(Me.ProxyServer)
        If Me.UseDefaultCredentials = True Then
            EveHQProxy.UseDefaultCredentials = True
        Else
            EveHQProxy.UseDefaultCredentials = False
            If Me.UseBasicAuthentication = False Then
                EveHQProxy.Credentials = New System.Net.NetworkCredential(Me.ProxyUsername, Me.ProxyPassword)
            Else
                Dim ProxyCredentials As New System.Net.NetworkCredential(Me.ProxyUsername, Me.ProxyPassword)
                EveHQProxy.Credentials = ProxyCredentials.GetCredential(New Uri(Me.ProxyServer), "Basic")
            End If
        End If
        Return EveHQProxy
    End Function

End Class
