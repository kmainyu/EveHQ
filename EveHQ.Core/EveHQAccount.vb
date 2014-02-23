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

Imports EveHQ.EveApi
Imports System.Xml

<Serializable()>
Public Class EveHQAccount

    Dim _apiKeySystem As APIKeySystems

    Public Property CorpApiAccountKey As String

    Public Property ApiKeyExpiryDate As Date

    Public Property AccessMask As Long

    Public Property ApiKeySystem As APIKeySystems
        Get
            ' Set the key type to version 2 (the current system) if it is not known
            If _apiKeySystem = APIKeySystems.Unknown Then
                _apiKeySystem = APIKeySystems.Version2
            End If
            Return _apiKeySystem
        End Get
        Set(value As APIKeySystems)
            _apiKeySystem = value
        End Set
    End Property

    Public Property LogonMinutes() As Long

    Public Property LogonCount() As Long

    Public Property PaidUntil() As Date

    Public Property CreateDate() As Date

    Public Property LastAccountStatusCheck() As Date

    Public Property FailedAttempts() As Integer

    Public Property UserID() As String

    Public Property APIKey() As String

    Public Property FriendlyName() As String

    Public Property Characters() As New List(Of String)

    Public Property APIKeyType() As APIKeyTypes

    Public Property APIAccountStatus() As APIAccountStatuses

    Public Function ToAPIAccount() As EveAPIAccount
        Dim apiAccount As New EveAPIAccount
        apiAccount.userID = UserID
        apiAccount.APIKey = APIKey
        apiAccount.APIVersion = CType(ApiKeySystem, APIKeyVersions)
        Return apiAccount
    End Function

    Public Sub CheckAPIKey()
        Select Case ApiKeySystem
            Case APIKeySystems.Version2
                ' New style system
                Dim apiResponse = HQ.ApiProvider.Account.ApiKeyInfo(UserID, APIKey)
                If apiResponse.IsSuccess Then

                    ' Should be version 2 info, get Access mask
                    AccessMask = 0
                    If apiResponse.ResultData IsNot Nothing Then
                      
                        AccessMask = apiResponse.ResultData.AccessMask
                        Select Case apiResponse.ResultData.ApiType
                            Case EveApi.ApiKeyType.Corporation
                                APIKeyType = APIKeyTypes.Corporation
                            Case EveApi.ApiKeyType.Character
                                APIKeyType = APIKeyTypes.Character
                            Case EveApi.ApiKeyType.Account
                                APIKeyType = APIKeyTypes.Account
                        End Select
                            Exit Select
                        End If
                Else
                    ' Still unknown!
                    AccessMask = 0
                End If
        End Select
    End Sub

    Public Function GetCharactersOnAccount() As List(Of String)

        Dim charList As New List(Of String)

        ' Fetch the characters on account XML file
        'Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
        'Dim accountXML As XmlDocument = apiReq.GetAPIXML(APITypes.Characters, ToAPIAccount, APIReturnMethods.ReturnStandard)
        Dim characters = HQ.ApiProvider.Account.Characters(UserID, APIKey)
        ' Get characters
        If characters.IsSuccess Then
            Dim characterList = characters.ResultData
            For Each character As EveApi.AccountCharacter In characterList
                Select Case ApiKeySystem
                    Case APIKeySystems.Version2
                        If APIKeyType = APIKeyTypes.Corporation Then
                            If charList.Contains(character.CorporationName) = False Then
                                charList.Add(character.CorporationName)
                            End If
                        Else
                            If charList.Contains(character.Name) = False Then
                                charList.Add(character.Name)
                            End If
                        End If
                End Select
            Next
        End If

        Return charList

    End Function

    Public Function CanUseCharacterAPI(characterAPIToCheck As CharacterAccessMasks) As Boolean
        Return AccessMasks.HasCharacterPermissions(AccessMask, characterAPIToCheck)
    End Function

    Public Function CanUseCorporateAPI(corporateAPIToCheck As CorporateAccessMasks) As Boolean
        Return AccessMasks.HasCorpPermissions(AccessMask, corporateAPIToCheck)
    End Function

End Class
