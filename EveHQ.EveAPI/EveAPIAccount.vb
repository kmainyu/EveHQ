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

    Private cUserID As String
    Private cAPIkey As String
    Private cAPIVersion As APIKeyVersions

    ''' <summary>
    ''' Holds the userID element of the API account data
    ''' </summary>
    ''' <value></value>
    ''' <returns>The userID of the API Account</returns>
    ''' <remarks></remarks>
    Public Property userID() As String
        Get
            Return cUserID
        End Get
        Set(ByVal value As String)
            cUserID = value
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
            Return cAPIkey
        End Get
        Set(ByVal value As String)
            cAPIkey = value
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
            Return cAPIVersion
        End Get
        Set(value As APIKeyVersions)
            cAPIVersion = value
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
    ''' <param name="InitialUserID">The userID of the API account</param>
    ''' <param name="InitialAPIKey">The APIKey of the API account</param>
    ''' <param name="InitialVersion">The initial version of the API key</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal InitialUserID As String, ByVal InitialAPIKey As String, ByVal InitialVersion As APIKeyVersions)
        Me.userID = InitialUserID
        Me.APIKey = InitialAPIKey
        Me.cAPIVersion = InitialVersion
    End Sub

End Class

Public Enum APIKeyVersions As Integer
    Unknown = 0
    Version1 = 1
    Version2 = 2
End Enum
