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
'====================================================================================


''' <summary>
''' Class for storing details of a proxy server to use for various connections to the web
''' </summary>
''' <remarks></remarks>
Public Class RemoteProxyServer
    Private _proxyRequired As Boolean = False
    Private _proxyServer As String = ""
    Private _proxyPort As Integer = 0
    Private _proxyUsername As String = ""
    Private _proxyPassword As String = ""
    Private _useDefaultCredentials As Boolean = True
    Private _useBasicAuthenticaion As Boolean = True

    ''' <summary>
    ''' Determines whether a Proxy Server should be used or not
    ''' </summary>
    ''' <value></value>
    ''' <returns>A boolean value indicating whether a Proxy Server is required to be used</returns>
    ''' <remarks></remarks>
    Public Property ProxyRequired() As Boolean
        Get
            Return _proxyRequired
        End Get
        Set(ByVal value As Boolean)
            _proxyRequired = value
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
            Return _proxyServer
        End Get
        Set(ByVal value As String)
            _proxyServer = value
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
            Return _proxyPort
        End Get
        Set(ByVal value As Integer)
            _proxyPort = value
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
            Return _useDefaultCredentials
        End Get
        Set(ByVal value As Boolean)
            _useDefaultCredentials = value
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
            Return _useBasicAuthenticaion
        End Get
        Set(ByVal value As Boolean)
            _useBasicAuthenticaion = value
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
            Return _proxyUsername
        End Get
        Set(ByVal value As String)
            _proxyUsername = value
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
            Return _proxyPassword
        End Get
        Set(ByVal value As String)
            _proxyPassword = value
        End Set
    End Property

    ''' <summary>
    ''' Creates an instance of a System.Net.WebProxy for use in web connections
    ''' </summary>
    ''' <returns>A System.Net.WebProxy instance</returns>
    ''' <remarks></remarks>
    Public Function SetupWebProxy() As Net.WebProxy
        Dim eveHQProxy As New Net.WebProxy(ProxyServer)
        If UseDefaultCredentials = True Then
            eveHQProxy.UseDefaultCredentials = True
        Else
            eveHQProxy.UseDefaultCredentials = False
            If UseBasicAuthentication = False Then
                eveHQProxy.Credentials = New Net.NetworkCredential(ProxyUsername, ProxyPassword)
            Else
                Dim proxyCredentials As New Net.NetworkCredential(ProxyUsername, ProxyPassword)
                eveHQProxy.Credentials = proxyCredentials.GetCredential(New Uri(ProxyServer), "Basic")
            End If
        End If
        Return eveHQProxy
    End Function

End Class
