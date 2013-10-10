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
Imports System.Xml
Imports EveHQ.EveAPI

<Serializable()> Public Class EveAccount

    Private cUserID As String
    Private cAPIkey As String
    Private cFriendlyName As String
    Private cCharacters As New ArrayList
    Private cAPIKeyType As APIKeyTypes
    Private cAPIAccountStatus As APIAccountStatuses
    Private cFailedAttempts As Integer
    Private cLastAccountStatusCheck As Date
    Private cCreateDate As Date
    Private cPaidUntil As Date
    Private cLogonCount As Long
    Private cLogonMinutes As Long
    Private cAPIKeySystem As APIKeySystems
    Private cAccessMask As Long
    Private cAPIKeyExpiryDate As Date
    Private cCorpAPIAccountKey As String

    Public Property CorpAPIAccountKey As String
        Get
            Return cCorpAPIAccountKey
        End Get
        Set(value As String)
            cCorpAPIAccountKey = value
        End Set
    End Property
    Public Property APIKeyExpiryDate As Date
        Get
            Return cAPIKeyExpiryDate
        End Get
        Set(value As Date)
            cAPIKeyExpiryDate = value
        End Set
    End Property
    Public Property AccessMask As Long
        Get
            Return cAccessMask
        End Get
        Set(value As Long)
            cAccessMask = value
        End Set
    End Property
    Public Property APIKeySystem As APIKeySystems
        Get
            ' Set the key type to version 1 if it is not known
            If cAPIKeySystem = APIKeySystems.Unknown Then
                cAPIKeySystem = APIKeySystems.Version2
            End If
            Return cAPIKeySystem
        End Get
        Set(value As APIKeySystems)
            cAPIKeySystem = value
        End Set
    End Property
    Public Property LogonMinutes() As Long
        Get
            Return cLogonMinutes
        End Get
        Set(ByVal value As Long)
            cLogonMinutes = value
        End Set
    End Property
    Public Property LogonCount() As Long
        Get
            Return cLogonCount
        End Get
        Set(ByVal value As Long)
            cLogonCount = value
        End Set
    End Property
    Public Property PaidUntil() As Date
        Get
            Return cPaidUntil
        End Get
        Set(ByVal value As Date)
            cPaidUntil = value
        End Set
    End Property
    Public Property CreateDate() As Date
        Get
            Return cCreateDate
        End Get
        Set(ByVal value As Date)
            cCreateDate = value
        End Set
    End Property
    Public Property LastAccountStatusCheck() As Date
        Get
            Return cLastAccountStatusCheck
        End Get
        Set(ByVal value As Date)
            cLastAccountStatusCheck = value
        End Set
    End Property
    Public Property FailedAttempts() As Integer
        Get
            Return cFailedAttempts
        End Get
        Set(ByVal value As Integer)
            cFailedAttempts = value
        End Set
    End Property
    Public Property userID() As String
        Get
            Return cUserID
        End Get
        Set(ByVal value As String)
            cUserID = value
        End Set
    End Property
    Public Property APIKey() As String
        Get
            Return cAPIkey
        End Get
        Set(ByVal value As String)
            cAPIkey = value
        End Set
    End Property
    Public Property FriendlyName() As String
        Get
            Return cFriendlyName
        End Get
        Set(ByVal value As String)
            cFriendlyName = value
        End Set
    End Property
    Public Property Characters() As ArrayList
        Get
            Return cCharacters
        End Get
        Set(ByVal value As ArrayList)
            cCharacters = value
        End Set
    End Property

    Public Property APIKeyType() As APIKeyTypes
        Get
            Return cAPIKeyType
        End Get
        Set(ByVal value As APIKeyTypes)
            cAPIKeyType = value
        End Set
    End Property

    Public Property APIAccountStatus() As APIAccountStatuses
        Get
            Return cAPIAccountStatus
        End Get
        Set(ByVal value As APIAccountStatuses)
            cAPIAccountStatus = value
        End Set
    End Property

    Public Function ToAPIAccount() As EveHQ.EveAPI.EveAPIAccount
        Dim APIAccount As New EveHQ.EveAPI.EveAPIAccount
        APIAccount.userID = Me.userID
        APIAccount.APIKey = Me.APIKey
        APIAccount.APIVersion = CType(Me.APIKeySystem, EveAPI.APIKeyVersions)
        Return APIAccount
    End Function

    Public Sub CheckAPIKey()
        Select Case Me.APIKeySystem
            Case APIKeySystems.Version2
                Dim apiResponse As EveServiceResponse(Of ApiKeyInfo) = HQ.ApiProvider.Account.ApiKeyInfo(Me.userID, Me.APIKey)

                ' Should be version 2 info, get Access mask
                Me.AccessMask = 0
                If apiResponse.IsFaulted = False And apiResponse.EveErrorCode = 0 And apiResponse.IsSuccessfulHttpStatus = True And apiResponse.ResultData IsNot Nothing Then
                    Select Case apiResponse.ResultData.ApiType
                        Case EveAPI.ApiKeyType.Corporation
                            Me.APIKeyType = APIKeyTypes.Corporation
                        Case EveAPI.ApiKeyType.Character
                            Me.APIKeyType = APIKeyTypes.Character
                        Case EveAPI.ApiKeyType.Account
                            Me.APIKeyType = APIKeyTypes.Account
                        Case Else
                            Me.APIKeyType = APIKeyTypes.Unknown
                    End Select

                    Me.AccessMask = apiResponse.ResultData.AccessMask
                End If
                'Case Else
                ' Still unknown!
                Me.AccessMask = 0
                'End Select
            Case Else
                ' Ignore
        End Select
    End Sub

    Public Function GetCharactersOnAccount() As List(Of String)

        Dim CharList As New List(Of String)
        Dim apiResponse As EveServiceResponse(Of IEnumerable(Of AccountCharacter)) = HQ.ApiProvider.Account.Characters(Me.userID, Me.APIKey)


        ' Get characters
        If apiResponse.ResultData IsNot Nothing And apiResponse.IsFaulted = False And apiResponse.IsSuccessfulHttpStatus = True And apiResponse.EveErrorCode = 0 Then
            For Each character As AccountCharacter In apiResponse.ResultData
                Select Case Me.APIKeySystem
                    Case APIKeySystems.Version2
                        If Me.APIKeyType = APIKeyTypes.Corporation Then
                            If CharList.Contains(character.CorporationName) = False Then
                                CharList.Add(character.CorporationName)
                            End If
                        Else
                            If CharList.Contains(character.Name) = False Then
                                CharList.Add(character.Name)
                            End If
                        End If
                    Case Else
                        ' Ignore
                End Select
            Next
        End If

        Return CharList

    End Function

    Public Function CanUseCharacterAPI(CharacterAPIToCheck As EveAPI.CharacterAccessMasks) As Boolean
        If (Me.AccessMask And CLng(Math.Pow(2, CDbl(CharacterAPIToCheck)))) = Math.Pow(2, CharacterAPIToCheck) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CanUseCorporateAPI(CorporateAPIToCheck As EveAPI.CorporateAccessMasks) As Boolean
        If (Me.AccessMask And CLng(Math.Pow(2, CDbl(CorporateAPIToCheck)))) = Math.Pow(2, CorporateAPIToCheck) Then
            Return True
        Else
            Return False
        End If
    End Function

End Class

Public Enum APIKeyTypes As Integer
    Unknown = 0
    Limited = 1
    Full = 2
    Character = 3
    Corporation = 4
    Account = 5
End Enum

Public Enum APIAccountStatuses As Integer
    Active = 0
    Disabled = 1
    ManualDisabled = 2
End Enum

Public Enum APIKeySystems As Integer
    Unknown = 0
    Version1 = 1
    Version2 = 2
End Enum


