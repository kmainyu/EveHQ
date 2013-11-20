' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2012  EveHQ Development Team
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

' ReSharper disable once CheckNamespace
<Serializable()> Public Class EveAccount

    ' ReSharper disable InconsistentNaming
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

    'Public Function ToAPIAccount() As EveAPIAccount
    '    Dim APIAccount As New EveAPIAccount
    '    APIAccount.UserID = Me.userID
    '    APIAccount.APIKey = Me.APIKey
    '    APIAccount.APIVersion = CType(Me.APIKeySystem, APIKeyVersions)
    '    Return APIAccount
    'End Function

    'Public Sub CheckAPIKey()
    '    Select Case Me.APIKeySystem
    '        Case APIKeySystems.Version2
    '            ' New style system
    '            Dim APIReq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
    '            Dim APIXML As XmlDocument = APIReq.GetAPIXML(APITypes.APIKeyInfo, Me.ToAPIAccount, APIReturnMethods.BypassCache)
    '            Select Case APIReq.LastAPIError
    '                Case -1
    '                    ' Should be version 2 info, get Access mask
    '                    Me.AccessMask = 0
    '                    If APIXML IsNot Nothing Then
    '                        Dim KeyList As XmlNodeList = APIXML.GetElementsByTagName("key")
    '                        If KeyList.Count > 0 Then
    '                            Me.AccessMask = CLng(KeyList(0).Attributes.GetNamedItem("accessMask").Value)
    '                            Select Case KeyList(0).Attributes.GetNamedItem("type").Value
    '                                Case "Corporation"
    '                                    Me.APIKeyType = APIKeyTypes.Corporation
    '                                Case "Character"
    '                                    Me.APIKeyType = APIKeyTypes.Character
    '                                Case "Account"
    '                                    Me.APIKeyType = APIKeyTypes.Account
    '                            End Select
    '                            Exit Select
    '                        End If
    '                    End If
    '                Case Else
    '                    ' Still unknown!
    '                    Me.AccessMask = 0
    '            End Select
    '        Case Else
    '            ' Ignore
    '    End Select
    'End Sub

    'Public Function GetCharactersOnAccount() As List(Of String)

    '    Dim CharList As New List(Of String)

    '    ' Fetch the characters on account XML file
    '    Dim APIReq As New EveAPIRequest(HQ.EveHqapiServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.CacheFolder)
    '    Dim accountXML As XmlDocument = APIReq.GetAPIXML(APITypes.Characters, Me.ToAPIAccount, APIReturnMethods.ReturnStandard)

    '    ' Get characters
    '    If accountXML IsNot Nothing Then
    '        Dim CharacterList As XmlNodeList = accountXML.SelectNodes("/eveapi/result/rowset/row")
    '        For Each Character As XmlNode In CharacterList
    '            Select Case Me.APIKeySystem
    '                Case APIKeySystems.Version2
    '                    If Me.APIKeyType = APIKeyTypes.Corporation Then
    '                        If CharList.Contains(Character.Attributes.GetNamedItem("corporationName").Value) = False Then
    '                            CharList.Add(Character.Attributes.GetNamedItem("corporationName").Value)
    '                        End If
    '                    Else
    '                        If CharList.Contains(Character.Attributes.GetNamedItem("name").Value) = False Then
    '                            CharList.Add(Character.Attributes.GetNamedItem("name").Value)
    '                        End If
    '                    End If
    '                Case Else
    '                    ' Ignore
    '            End Select
    '        Next
    '    End If

    '    Return CharList

    'End Function

    'Public Function CanUseCharacterAPI(CharacterAPIToCheck As CharacterAccessMasks) As Boolean
    '    If (Me.AccessMask And CLng(Math.Pow(2, CDbl(CharacterAPIToCheck)))) = Math.Pow(2, CharacterAPIToCheck) Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

    'Public Function CanUseCorporateAPI(CorporateAPIToCheck As CorporateAccessMasks) As Boolean
    '    If (Me.AccessMask And CLng(Math.Pow(2, CDbl(CorporateAPIToCheck)))) = Math.Pow(2, CorporateAPIToCheck) Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

End Class