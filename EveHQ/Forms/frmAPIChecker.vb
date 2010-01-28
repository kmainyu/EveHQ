Imports System.Xml

Public Class frmAPIChecker
    Dim APIMethods As New SortedList
    Dim APIStyle As Integer = 0

    Private Sub frmAPIChecker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim APIName As String = ""
        APIMethods.Clear()
        ' Load up the API methods into the combobox
        cboAPIMethod.BeginUpdate()
        cboAPIMethod.Items.Clear()
        For Each APIMethod As Integer In [Enum].GetValues(GetType(EveHQ.Core.EveAPI.APIRequest))
            APIName = [Enum].GetName(GetType(EveHQ.Core.EveAPI.APIRequest), APIMethod)
            APIMethods.Add(APIName, APIMethod)
            cboAPIMethod.Items.Add(APIName)
        Next
        cboAPIMethod.EndUpdate()
        ' Load up account characters into the character combo
        cboCharacter.BeginUpdate()
        cboCharacter.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Account <> "" Then
                cboCharacter.Items.Add(cPilot.Name)
            End If
        Next
        cboCharacter.EndUpdate()
        ' Load up the Account combo
        cboAccount.BeginUpdate()
        cboAccount.Items.Clear()
        For account As Integer = 1000 To 1006
            cboAccount.Items.Add(CStr(account))
        Next
        cboAccount.EndUpdate()

    End Sub

    Private Sub cboAPIMethod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAPIMethod.SelectedIndexChanged
        ' Find out the selected APIMethod and determine what information we need
        Select Case CInt(APIMethods(cboAPIMethod.SelectedItem))

            Case EveHQ.Core.EveAPI.APIRequest.AllianceList, _
                EveHQ.Core.EveAPI.APIRequest.RefTypes, _
                EveHQ.Core.EveAPI.APIRequest.SkillTree, _
                EveHQ.Core.EveAPI.APIRequest.Sovereignty, _
                EveHQ.Core.EveAPI.APIRequest.SovereigntyStatus, _
                EveHQ.Core.EveAPI.APIRequest.MapJumps, _
                EveHQ.Core.EveAPI.APIRequest.MapKills, _
                EveHQ.Core.EveAPI.APIRequest.Conquerables, _
                EveHQ.Core.EveAPI.APIRequest.ErrorList, _
                EveHQ.Core.EveAPI.APIRequest.FWTop100, _
                EveHQ.Core.EveAPI.APIRequest.FWMap, _
                EveHQ.Core.EveAPI.APIRequest.ServerStatus, _
                EveHQ.Core.EveAPI.APIRequest.CertificateTree
                lblCharacter.Enabled = False : cboCharacter.Enabled = False
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 1

            Case EveHQ.Core.EveAPI.APIRequest.NameToID, EveHQ.Core.EveAPI.APIRequest.IDToName
                lblCharacter.Enabled = False : cboCharacter.Enabled = False
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                If CInt(APIMethods(cboAPIMethod.SelectedItem)) = EveHQ.Core.EveAPI.APIRequest.NameToID Then
                    lblOtherInfo.Text = "Item Name"
                Else
                    lblOtherInfo.Text = "Item ID:"
                End If
                APIStyle = 2

            Case EveHQ.Core.EveAPI.APIRequest.Characters
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 3

            Case EveHQ.Core.EveAPI.APIRequest.AccountBalancesChar, _
            EveHQ.Core.EveAPI.APIRequest.AccountBalancesCorp, _
            EveHQ.Core.EveAPI.APIRequest.CharacterSheet, _
            EveHQ.Core.EveAPI.APIRequest.CorpSheet, _
            EveHQ.Core.EveAPI.APIRequest.CorpMemberTracking, _
            EveHQ.Core.EveAPI.APIRequest.SkillTraining, _
            EveHQ.Core.EveAPI.APIRequest.SkillQueue, _
            EveHQ.Core.EveAPI.APIRequest.AssetsChar, _
            EveHQ.Core.EveAPI.APIRequest.AssetsCorp, _
            EveHQ.Core.EveAPI.APIRequest.IndustryChar, _
            EveHQ.Core.EveAPI.APIRequest.IndustryCorp, _
            EveHQ.Core.EveAPI.APIRequest.OrdersChar, _
            EveHQ.Core.EveAPI.APIRequest.OrdersCorp, _
            EveHQ.Core.EveAPI.APIRequest.POSList, _
            EveHQ.Core.EveAPI.APIRequest.StandingsChar, _
            EveHQ.Core.EveAPI.APIRequest.StandingsCorp, _
            EveHQ.Core.EveAPI.APIRequest.CorpMemberSecurity, _
            EveHQ.Core.EveAPI.APIRequest.CorpMemberSecurityLog, _
            EveHQ.Core.EveAPI.APIRequest.CorpShareholders, _
            EveHQ.Core.EveAPI.APIRequest.CorpTitles, _
            EveHQ.Core.EveAPI.APIRequest.FWStatsChar, _
            EveHQ.Core.EveAPI.APIRequest.FWStatsCorp, _
            EveHQ.Core.EveAPI.APIRequest.MedalsReceived, _
            EveHQ.Core.EveAPI.APIRequest.MedalsAvailable, _
            EveHQ.Core.EveAPI.APIRequest.MailMessages, _
            EveHQ.Core.EveAPI.APIRequest.Notifications, _
                EveHQ.Core.EveAPI.APIRequest.MailingLists, _
                EveHQ.Core.EveAPI.APIRequest.MemberMedals
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = False : txtOtherInfo.Enabled = False
                APIStyle = 4

            Case EveHQ.Core.EveAPI.APIRequest.POSDetails
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "POS ItemID:"
                APIStyle = 5

            Case EveHQ.Core.EveAPI.APIRequest.WalletJournalChar, EveHQ.Core.EveAPI.APIRequest.WalletJournalCorp, EveHQ.Core.EveAPI.APIRequest.WalletTransChar, EveHQ.Core.EveAPI.APIRequest.WalletTransCorp
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = True : cboAccount.Enabled = True
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "Before RefID:"
                APIStyle = 6

            Case EveHQ.Core.EveAPI.APIRequest.KillLogChar, EveHQ.Core.EveAPI.APIRequest.KillLogChar
                lblCharacter.Enabled = True : cboCharacter.Enabled = True
                lblAccount.Enabled = False : cboAccount.Enabled = False
                lblOtherInfo.Enabled = True : txtOtherInfo.Enabled = True
                lblOtherInfo.Text = "Before KillID:"
                APIStyle = 7

        End Select
    End Sub

    Private Sub btnGetAPI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetAPI.Click
        Dim selpilot As New EveHQ.Core.Pilot
        Dim pilotAccount As New EveHQ.Core.EveAccount
        ' Check for the info
        If APIStyle > 2 Then
            If cboCharacter.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a character to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            selpilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboCharacter.SelectedItem.ToString), Core.Pilot)
            Dim accountName As String = selpilot.Account
            pilotAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
        End If
        If APIStyle = 6 Then
            If cboAccount.SelectedItem Is Nothing Then
                MessageBox.Show("You must select an account key to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If
        Select Case APIStyle
            Case 2, 5
                If txtOtherInfo.Text = "" Then
                    MessageBox.Show("You must enter some data to retrieve the requested API.", "Additional Info Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
        End Select
        Dim testXML As New XmlDocument
        Dim returnMethod As Integer = 0
        If chkReturnCached.Checked = True Then
            returnMethod = 1
        End If
        Select Case APIStyle
            Case 1
                testXML = EveHQ.Core.EveAPI.GetAPIXML(CInt(APIMethods.Item(cboAPIMethod.SelectedItem.ToString)), returnMethod)
            Case 2
                testXML = EveHQ.Core.EveAPI.GetAPIXML(CInt(APIMethods.Item(cboAPIMethod.SelectedItem.ToString)), txtOtherInfo.Text.Trim, returnMethod)
            Case 3
                testXML = EveHQ.Core.EveAPI.GetAPIXML(CInt(APIMethods.Item(cboAPIMethod.SelectedItem.ToString)), pilotAccount, returnMethod)
            Case 4
                testXML = EveHQ.Core.EveAPI.GetAPIXML(CInt(APIMethods.Item(cboAPIMethod.SelectedItem.ToString)), pilotAccount, selpilot.ID, returnMethod)
            Case 5
                testXML = EveHQ.Core.EveAPI.GetAPIXML(CInt(APIMethods.Item(cboAPIMethod.SelectedItem.ToString)), pilotAccount, selpilot.ID, CInt(txtOtherInfo.Text), returnMethod)
            Case 6
                testXML = EveHQ.Core.EveAPI.GetAPIXML(CInt(APIMethods.Item(cboAPIMethod.SelectedItem.ToString)), pilotAccount, selpilot.ID, CInt(cboAccount.SelectedItem.ToString), txtOtherInfo.Text, returnMethod)
            Case 7
                testXML = EveHQ.Core.EveAPI.GetAPIXML(CInt(APIMethods.Item(cboAPIMethod.SelectedItem.ToString)), pilotAccount, selpilot.ID, txtOtherInfo.Text, returnMethod)
        End Select
        Try
            wbAPI.Navigate(EveHQ.Core.EveAPI.LastAPIFileName)
            lblCurrentlyViewing.Text = "Currently Viewing: " & cboAPIMethod.SelectedItem.ToString
            lblFileLocation.Text = "Cache File Location: " & EveHQ.Core.EveAPI.LastAPIFileName
        Catch ex As Exception
            MessageBox.Show("There was an error trying to display the requested API. The error was: " & ControlChars.CrLf & ex.Message, "Error Requesting API", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class