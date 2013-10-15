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

''' <summary>
''' Class for storing the Eve API Account details for use in the EveAPI classes
''' </summary>
''' <remarks></remarks>
Public Class EveAPIAccount

    Private _userID As String
    Private _apiKey As String
    Private _apiVersion As APIKeyVersions

    ''' <summary>
    ''' Holds the userID element of the API account data
    ''' </summary>
    ''' <value></value>
    ''' <returns>The userID of the API Account</returns>
    ''' <remarks></remarks>
    Public Property UserID() As String
        Get
            Return _userID
        End Get
        Set(ByVal value As String)
            _userID = value
        End Set
    End Property

    ''' <summary>
    ''' Holds the APIKey element of the API account data
    ''' </summary>
    ''' <value></value>
    ''' <returns>The APIKey of the API Account</returns>
    ''' <remarks></remarks>
    Public Property APIKey() As String
        Get
            Return _apiKey
        End Get
        Set(ByVal value As String)
            _apiKey = value
        End Set
    End Property

    ''' <summary>
    ''' Holds the version of the API account information
    ''' </summary>
    ''' <value></value>
    ''' <returns>The version of the API accounts</returns>
    ''' <remarks></remarks>
    Public Property APIVersion As APIKeyVersions
        Get
            Return _apiVersion
        End Get
        Set(value As APIKeyVersions)
            _apiVersion = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new EveAPIAccount
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        userID = ""
        APIKey = ""
        APIVersion = APIKeyVersions.Unknown
    End Sub

    ''' <summary>
    ''' Creates a new EveAPIAccount using the userID and APIKey specified
    ''' </summary>
    ''' <param name="initialUserID">The userID of the API account</param>
    ''' <param name="initialAPIKey">The APIKey of the API account</param>
    ''' <param name="initialVersion">The initial version of the API key</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal initialUserID As String, ByVal initialAPIKey As String, ByVal initialVersion As APIKeyVersions)
        UserID = initialUserID
        APIKey = initialAPIKey
        _apiVersion = initialVersion
    End Sub

End Class

Public Enum APIKeyVersions As Integer
    Unknown = 0
    Version1 = 1
    Version2 = 2
End Enum
