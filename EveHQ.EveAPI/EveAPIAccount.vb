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
' ==============================================================================



''' <summary>
'''     Class for storing the Eve API Account details for use in the EveAPI classes
''' </summary>
''' <remarks></remarks>
Public Class EveAPIAccount
    Private _userID As String
    Private _apiKey As String
    Private _apiVersion As APIKeyVersions

    
    ''' <summary>
    '''     Holds the userID element of the API account data
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
    '''     Holds the APIKey element of the API account data
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
    '''     Holds the version of the API account information
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
    '''     Creates a new EveAPIAccount
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        userID = ""
        APIKey = ""
        APIVersion = APIKeyVersions.Unknown
    End Sub

    
    ''' <summary>
    '''     Creates a new EveAPIAccount using the userID and APIKey specified
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
