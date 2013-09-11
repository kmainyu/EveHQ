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

    Public Function ToAPIAccount() As EveAPI.EveAPIAccount
        Dim apiAccount As New EveAPI.EveAPIAccount
        apiAccount.userID = UserID
        apiAccount.APIKey = APIKey
        apiAccount.APIVersion = CType(ApiKeySystem, EveAPI.APIKeyVersions)
        Return apiAccount
    End Function

    Public Sub CheckAPIKey()
        Select Case ApiKeySystem
            Case APIKeySystems.Version2
                ' New style system
                Dim apiReq As New EveAPI.EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                Dim apixml As XmlDocument = apiReq.GetAPIXML(EveAPI.APITypes.APIKeyInfo, ToAPIAccount, EveAPI.APIReturnMethods.BypassCache)
                Select Case apiReq.LastAPIError
                    Case -1
                        ' Should be version 2 info, get Access mask
                        AccessMask = 0
                        If apixml IsNot Nothing Then
                            Dim keyList As XmlNodeList = apixml.GetElementsByTagName("key")
                            If keyList.Count > 0 Then
                                AccessMask = CLng(keyList(0).Attributes.GetNamedItem("accessMask").Value)
                                Select Case keyList(0).Attributes.GetNamedItem("type").Value
                                    Case "Corporation"
                                        APIKeyType = APIKeyTypes.Corporation
                                    Case "Character"
                                        APIKeyType = APIKeyTypes.Character
                                    Case "Account"
                                        APIKeyType = APIKeyTypes.Account
                                End Select
                                Exit Select
                            End If
                        End If
                    Case Else
                        ' Still unknown!
                        AccessMask = 0
                End Select
        End Select
    End Sub

    Public Function GetCharactersOnAccount() As List(Of String)

        Dim charList As New List(Of String)

        ' Fetch the characters on account XML file
        Dim apiReq As New EveAPI.EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
        Dim accountXML As XmlDocument = apiReq.GetAPIXML(EveAPI.APITypes.Characters, ToAPIAccount, EveAPI.APIReturnMethods.ReturnStandard)

        ' Get characters
        If accountXML IsNot Nothing Then
            Dim characterList As XmlNodeList = accountXML.SelectNodes("/eveapi/result/rowset/row")
            For Each character As XmlNode In characterList
                Select Case ApiKeySystem
                    Case APIKeySystems.Version2
                        If APIKeyType = APIKeyTypes.Corporation Then
                            If charList.Contains(character.Attributes.GetNamedItem("corporationName").Value) = False Then
                                charList.Add(character.Attributes.GetNamedItem("corporationName").Value)
                            End If
                        Else
                            If charList.Contains(character.Attributes.GetNamedItem("name").Value) = False Then
                                charList.Add(character.Attributes.GetNamedItem("name").Value)
                            End If
                        End If
                End Select
            Next
        End If

        Return charList

    End Function

    Public Function CanUseCharacterAPI(characterAPIToCheck As EveAPI.CharacterAccessMasks) As Boolean
        If Math.Abs((AccessMask And CLng(Math.Pow(2, CDbl(characterAPIToCheck)))) - Math.Pow(2, characterAPIToCheck)) < 0.01 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CanUseCorporateAPI(corporateAPIToCheck As EveAPI.CorporateAccessMasks) As Boolean
        If Math.Abs((AccessMask And CLng(Math.Pow(2, CDbl(corporateAPIToCheck)))) - Math.Pow(2, corporateAPIToCheck)) < 0.01 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
