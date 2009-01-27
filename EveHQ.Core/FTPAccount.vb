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
<Serializable()> Public Class FTPAccount

    Public cFTPName As String
    Public cServer As String
    Public cPort As Integer
    Public cPath As String
    Public cUsername As String
    Public cPassword As String

    Public Property FTPName() As String
        Get
            Return cFTPName
        End Get
        Set(ByVal value As String)
            cFTPName = value
        End Set
    End Property
    Public Property Server() As String
        Get
            Return cServer
        End Get
        Set(ByVal value As String)
            cServer = value
        End Set
    End Property
    Public Property Port() As Integer
        Get
            Return cPort
        End Get
        Set(ByVal value As Integer)
            cPort = value
        End Set
    End Property
    Public Property Path() As String
        Get
            Return cPath
        End Get
        Set(ByVal value As String)
            cPath = value
        End Set
    End Property
    Public Property Username() As String
        Get
            Return cUsername
        End Get
        Set(ByVal value As String)
            cUsername = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return cPassword
        End Get
        Set(ByVal value As String)
            cPassword = value
        End Set
    End Property

End Class
