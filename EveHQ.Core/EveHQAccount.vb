Imports EveHQ.EveAPI
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
        If Math.Abs((AccessMask And CLng(Math.Pow(2, CDbl(characterAPIToCheck)))) - Math.Pow(2, characterAPIToCheck)) < 0.01 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CanUseCorporateAPI(corporateAPIToCheck As CorporateAccessMasks) As Boolean
        If Math.Abs((AccessMask And CLng(Math.Pow(2, CDbl(corporateAPIToCheck)))) - Math.Pow(2, corporateAPIToCheck)) < 0.01 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
