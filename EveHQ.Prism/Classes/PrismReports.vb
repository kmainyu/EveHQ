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
Imports System.Globalization
Imports System.Text
Imports EveHQ.EveAPI
Imports EveHQ.Core
Imports System.Xml
Imports EveHQ.Common.Extensions

Namespace Classes

    Public Class PrismReports
        Private Const PrismTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        Shared ReadOnly Culture As CultureInfo = New CultureInfo("en-GB")

#Region "Standard Report Functions"

        Public Shared Function HTMLHeader(ByVal browserHeader As String, reportTitle As String) As String
            Dim html As New StringBuilder
            html.AppendLine("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""http://www.w3.org/TR/html4/strict.dtd"">")
            html.AppendLine("<html lang=""" & CultureInfo.CurrentCulture.ToString & """>")
            html.AppendLine("<head>")
            html.AppendLine("<META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">")
            html.AppendLine("<title>" & browserHeader & "</title>" & ReportCSS() & "</head>")
            html.AppendLine("<body>")
            If reportTitle <> "" Then
                html.AppendLine("<table width=800px border=0 align=center>")
                html.AppendLine("<tr height=30px><td align='center'><p class=title>" & reportTitle & "</p></td></tr>")
                html.AppendLine("</table>")
                html.AppendLine("<p></p>")
            End If
            Return html.ToString
        End Function

        Public Shared Function HTMLTitle(ByVal title As String) As String
            Dim html As New StringBuilder
            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr height=30px><td><p class=title>" & title & "</p></td></tr>")
            html.AppendLine("</table>")
            html.AppendLine("<p></p>")
            Return html.ToString
        End Function

        Public Shared Function HTMLFooter() As String
            Dim html As New StringBuilder
            html.AppendLine("<table width=800px align=center border=0><hr>")
            html.AppendLine("<tr><td><p align=center class=footer>Generated on " & Now.ToString & " by <a href='http://www.evehq.net'>" & My.Application.Info.ProductName & "</a> v" & My.Application.Info.Version.ToString & "</p></td></tr>")
            html.AppendLine("</table>")
            html.AppendLine("</body></html>")
            Return html.ToString
        End Function

        Private Shared Function ReportCSS() As String
            Dim css As New StringBuilder
            css.AppendLine("<STYLE><!--")
            css.AppendLine("BODY { font-family: Tahoma, Arial; font-size: 12px; bgcolor: #000000; background: #000000 }")
            css.AppendLine("TD, P { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff }")
            css.AppendLine(".thead { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff; font-variant: small-caps; background-color: #444444 }")
            css.AppendLine(".footer { font-family: Tahoma, Arial; font-size: 9px; color: #ffffff; font-variant: small-caps }")
            css.AppendLine(".title { font-family: Tahoma, Arial; font-size: 20px; color: #ffffff; font-variant: small-caps }")
            css.AppendLine("#wrapper {overflow: auto; height: 100%; width:820px; margin-left:auto; margin-right:auto;}")
            css.AppendLine("tr.pos td {color: #00ff00;}")
            css.AppendLine("tr.neg td {color: #ff0000;}")
            css.AppendLine("--></STYLE>")
            Return css.ToString
        End Function

#End Region

#Region "Wallet Journal Reports"

        Public Shared Function GetJournalReportData(startDate As Date, endDate As Date, ownerNames As List(Of String)) As DataSet

            Dim strSQL As String = "SELECT * FROM walletJournal"
            strSQL &= " WHERE walletJournal.transDate >= '" & startDate.ToString(PrismTimeFormat, Culture) & "' AND walletJournal.transDate < '" & endDate.ToString(PrismTimeFormat, Culture) & "'"

            ' Build the Owners List
            If ownerNames.Count > 0 Then
                Dim ownerList As New StringBuilder
                For Each ownerName As String In ownerNames
                    ownerList.Append(", '" & ownerName.Replace("'", "''") & "'")
                Next
                If ownerList.Length > 2 Then
                    ownerList.Remove(0, 2)
                End If
                ' Default to None
                strSQL &= " AND walletJournal.charName IN (" & ownerList.ToString & ")"
            End If

            ' Order the data
            strSQL &= " ORDER BY walletJournal.transKey ASC;"

            ' Fetch the data
            Dim walletData As DataSet = CustomDataFunctions.GetCustomData(strSQL)

            Return walletData
        End Function

        Public Shared Function GenerateIncomeAnalysis(walletData As DataSet) As SortedList(Of String, Double)

            Dim refTypeList As New SortedList(Of String, Double)

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        Dim refTypeID As String = CStr(walletItem.Item("refTypeID"))
                        'Dim Value As Double = Double.Parse(WalletItem.Item("amount").ToString, culture)
                        Dim value As Double = CDbl(walletItem.Item("amount"))

                        If value > 0 Then

                            If refTypeList.ContainsKey(refTypeID) = False Then
                                refTypeList.Add(refTypeID, value)
                            Else
                                refTypeList(refTypeID) += value
                            End If

                        End If

                    Next

                End If
            End If

            Return refTypeList
        End Function

        Public Shared Function GenerateExpenseAnalysis(walletData As DataSet) As SortedList(Of String, Double)

            Dim refTypeList As New SortedList(Of String, Double)

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        Dim refTypeID As String = CStr(walletItem.Item("refTypeID"))
                        'Dim Value As Double = Double.Parse(WalletItem.Item("amount").ToString, culture)
                        Dim value As Double = CDbl(walletItem.Item("amount"))

                        If value < 0 Then

                            If refTypeList.ContainsKey(refTypeID) = False Then
                                refTypeList.Add(refTypeID, value)
                            Else
                                refTypeList(refTypeID) += value
                            End If

                        End If

                    Next

                End If
            End If

            Return refTypeList
        End Function

        Public Shared Function GenerateCorpTaxAnalysis(ByVal walletData As DataSet) As SortedList(Of String, Double)

            Dim corpTaxList As New SortedList(Of String, Double)

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        Dim refTypeID As String = CStr(walletItem.Item("refTypeID"))
                        Dim value As Double = CDbl(walletItem.Item("amount"))
                        Dim owner As String = CStr(walletItem.Item("ownerName2"))

                        Select Case refTypeID
                            Case "33", "34", "85", "99"
                                If corpTaxList.ContainsKey(owner) = False Then
                                    corpTaxList.Add(owner, value)
                                Else
                                    corpTaxList(owner) += value
                                End If

                        End Select

                    Next

                End If
            End If

            Return corpTaxList
        End Function

        Public Shared Function GenerateJournalTypeIncomeAnalysis(walletData As DataSet, refTypeID As String, keyType As JournalKeyTypes) As SortedList(Of String, Double)

            Dim refTypeList As New SortedList(Of String, Double)

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        If refTypeID = CStr(walletItem.Item("refTypeID")) Then

                            Dim value As Double = CDbl(walletItem.Item("amount"))

                            If value > 0 Then

                                Dim key As String = ""

                                Select Case keyType
                                    Case JournalKeyTypes.OwnerName1
                                        key = CStr(walletItem.Item("ownerName1"))
                                    Case JournalKeyTypes.OwnerName2
                                        key = CStr(walletItem.Item("ownerName2"))
                                    Case JournalKeyTypes.ArgName1
                                        key = CStr(walletItem.Item("argName1"))
                                End Select

                                If refTypeList.ContainsKey(key) = False Then
                                    refTypeList.Add(key, value)
                                Else
                                    refTypeList(key) += value
                                End If

                            End If

                        End If

                    Next

                End If
            End If

            Return refTypeList
        End Function

        Public Shared Function GenerateJournalTypeExpenditureAnalysis(walletData As DataSet, refTypeID As String, keyType As JournalKeyTypes) As SortedList(Of String, Double)

            Dim refTypeList As New SortedList(Of String, Double)

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        If refTypeID = CStr(walletItem.Item("refTypeID")) Then

                            Dim value As Double = CDbl(walletItem.Item("amount"))

                            If value < 0 Then

                                Dim key As String = ""

                                Select Case keyType
                                    Case JournalKeyTypes.OwnerName1
                                        key = CStr(walletItem.Item("ownerName1"))
                                    Case JournalKeyTypes.OwnerName2
                                        key = CStr(walletItem.Item("ownerName2"))
                                    Case JournalKeyTypes.ArgName1
                                        key = CStr(walletItem.Item("argName1"))
                                End Select

                                If refTypeList.ContainsKey(key) = False Then
                                    refTypeList.Add(key, value)
                                Else
                                    refTypeList(key) += value
                                End If

                            End If

                        End If

                    Next

                End If
            End If

            Return refTypeList
        End Function

        Public Shared Function GenerateOwnerMovements(ByVal walletData As DataSet) As SortedList(Of String, SortedList(Of String, OwnerMovement))

            Dim ownerMovements As New SortedList(Of String, SortedList(Of String, OwnerMovement))

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        ' Check if we have the owner listed
                        Dim ownerName As String = CStr(walletItem.Item("charName"))
                        If ownerMovements.ContainsKey(ownerName) = False Then
                            ownerMovements.Add(ownerName, New SortedList(Of String, OwnerMovement))
                        End If

                        'Dim amount As Double = Double.Parse(WalletItem.Item("amount").ToString, culture)
                        'Dim balance As Double = Double.Parse(WalletItem.Item("balance").ToString, culture)
                        'Dim taxamount As Double = Double.Parse(WalletItem.Item("taxAmount").ToString, culture)
                        Dim amount As Double = CDbl(walletItem.Item("amount"))
                        Dim balance As Double = CDbl(walletItem.Item("balance"))
                        Dim taxamount As Double = CDbl(walletItem.Item("taxAmount"))

                        ' Check we have a wallet balance set up
                        Dim walletID As String = CStr(walletItem.Item("walletID"))
                        Dim movement As New OwnerMovement
                        If ownerMovements(ownerName).ContainsKey(walletID) = False Then
                            If amount <> -taxamount Then
                                movement.OwnerName = ownerName
                                movement.WalletID = walletID
                                movement.StartBalance = balance - amount + taxamount
                                ownerMovements(ownerName).Add(walletID, movement)
                            End If
                        Else
                            movement = ownerMovements(ownerName).Item(walletID)
                        End If
                        movement.EndBalance = balance

                    Next

                End If
            End If

            Return ownerMovements
        End Function

        Public Shared Function GenerateIncomeReportBodyHTML(refTypeList As SortedList(Of String, Double)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>INCOME</b></td></tr>")

            Dim total As Double = 0

            For Each refTypeID As String In refTypeList.Keys
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>" & PlugInData.RefTypes(refTypeID) & "</td>")
                html.AppendLine("<td align='right'> " & refTypeList(refTypeID).ToString("N2") & "</td>")
                html.AppendLine("</tr>")
                total += refTypeList(refTypeID)
            Next

            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td>INCOME TOTAL</td>")
            html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Total Income", total)
            Return (result)
        End Function

        Public Shared Function GenerateExpenseReportBodyHTML(refTypeList As SortedList(Of String, Double)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>EXPENDITURE</b></td></tr>")

            Dim total As Double = 0

            For Each refTypeID As String In refTypeList.Keys
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>" & PlugInData.RefTypes(refTypeID) & "</td>")
                html.AppendLine("<td align='right'> " & refTypeList(refTypeID).ToString("N2") & "</td>")
                html.AppendLine("</tr>")
                total += refTypeList(refTypeID)
            Next

            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td>EXPENDITURE TOTAL</td>")
            html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Total Expenditure", total)
            Return (result)
        End Function

        Public Shared Function GenerateCorpTaxReportBodyHTML(ByVal corpTaxList As SortedList(Of String, Double)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>CORPORATION TAX ANALYSIS</b></td></tr>")

            Dim total As Double = 0

            For Each owner As String In corpTaxList.Keys
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>" & owner & "</td>")
                html.AppendLine("<td align='right'> " & corpTaxList(owner).ToString("N2") & "</td>")
                html.AppendLine("</tr>")
                total += corpTaxList(owner)
            Next

            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td>CORPORATION TAX TOTAL</td>")
            html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Total Corporation Tax", total)
            Return (result)
        End Function

        Public Shared Function GenerateCashFlowReportBodyHTML(income As Double, expenditure As Double) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>CASH FLOW</b></td></tr>")

            ' Write Income
            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td>Income Total</td>")
            html.AppendLine("<td align='right'> " & income.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            ' Write Expenditure
            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td>Expenditure Total</td>")
            html.AppendLine("<td align='right'> " & expenditure.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            ' Write Cash Flow
            Dim cashFlow As Double = income + expenditure
            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            If cashFlow >= 0 Then
                html.AppendLine("<td>NET CASH INFLOW</td>")
            Else
                html.AppendLine("<td>NET CASH OUTFLOW</td>")
            End If
            html.AppendLine("<td align='right'> " & cashFlow.ToString("N2") & "</td>")
            html.AppendLine("</tr>")
            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Cash Flow", cashFlow)
            Return (result)
        End Function

        Public Shared Function GenerateMovementReportBodyHTML(ByVal movements As SortedList(Of String, SortedList(Of String, OwnerMovement))) As ReportResult

            Dim html As New StringBuilder

            If movements.Count > 0 Then

                html.AppendLine("<table width=800px border=0 align=center>")
                html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=250></td><td width=150></td><td width=150></td><td width=150></td></tr>")
                html.AppendLine("<tr><td width=50></td><td colspan=3><b>OWNER MOVEMENTS</b></td></tr>")
                html.AppendLine("<tr><td width=50></td><td width=50></td><td width=250></td><td width=150 align=center><i>Start Balance</i></td><td width=150 align=center><i>End Balance</i></td><td width=150 align=center><i>Movement</i></td></tr>")

                Dim total As Double = 0

                For Each owner As String In movements.Keys
                    Dim divisionNames As SortedList(Of String, String) = GetCorpDivisionNames(owner, True)
                    Dim startBalance As Double = 0
                    Dim endBalance As Double = 0

                    If movements(owner).Count > 1 Then
                        html.AppendLine("<tr><td colspan=2></td><td>" & owner & ":</td></tr>")
                    End If

                    For Each movement As OwnerMovement In movements(owner).Values
                        html.AppendLine("<tr>")
                        html.AppendLine("<td colspan=2></td>")
                        If divisionNames.Count > 0 Then
                            html.AppendLine("<td>" & divisionNames(movement.WalletID) & "</td>")
                        Else
                            html.AppendLine("<td>" & owner & " - Master Wallet</td>")
                        End If
                        html.AppendLine("<td align='right'> " & movement.StartBalance.ToString("N2") & "</td>")
                        html.AppendLine("<td align='right'> " & movement.EndBalance.ToString("N2") & "</td>")
                        html.AppendLine("<td align='right'> " & (movement.EndBalance - movement.StartBalance).ToString("N2") & "</td>")
                        html.AppendLine("</tr>")
                        startBalance += movement.StartBalance
                        endBalance += movement.EndBalance
                    Next

                    If movements(owner).Count > 1 Then
                        html.AppendLine("<tr>")
                        html.AppendLine("<td colspan=2></td>")
                        html.AppendLine("<td>Total for " & owner & "</td>")
                        html.AppendLine("<td align='right'> " & startBalance.ToString("N2") & "</td>")
                        html.AppendLine("<td align='right'> " & endBalance.ToString("N2") & "</td>")
                        html.AppendLine("<td align='right'> " & (endBalance - startBalance).ToString("N2") & "</td>")
                        html.AppendLine("</tr>")
                    End If

                    total += endBalance - startBalance

                Next

                html.AppendLine("<tr></tr>")
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>TOTAL MOVEMENT</td>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
                html.AppendLine("</tr>")

            End If

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            Return (result)
        End Function

        Public Shared Function GenerateJournalTypeIncomeReportBodyHTML(refTypeID As String, refTypeList As SortedList(Of String, Double)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>JOURNAL INCOME ANALYSIS - " & PlugInData.RefTypes(refTypeID) & "</b></td></tr>")

            Dim total As Double = 0

            For Each refTypeDetail As String In refTypeList.Keys
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>" & refTypeDetail & "</td>")
                html.AppendLine("<td align='right'> " & refTypeList(refTypeDetail).ToString("N2") & "</td>")
                html.AppendLine("</tr>")
                total += refTypeList(refTypeDetail)
            Next

            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td><b>INCOME TOTAL</b></td>")
            html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Total Income from " & PlugInData.RefTypes(refTypeID), total)
            Return (result)
        End Function

        Public Shared Function GenerateJournalTypeExpenditureReportBodyHTML(refTypeID As String, refTypeList As SortedList(Of String, Double)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>JOURNAL EXPENDITURE ANALYSIS - " & PlugInData.RefTypes(refTypeID) & "</b></td></tr>")

            Dim total As Double = 0

            For Each refTypeDetail As String In refTypeList.Keys
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>" & refTypeDetail & "</td>")
                html.AppendLine("<td align='right'> " & refTypeList(refTypeDetail).ToString("N2") & "</td>")
                html.AppendLine("</tr>")
                total += refTypeList(refTypeDetail)
            Next

            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td><b>EXPENDITURE TOTAL</b></td>")
            html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Total Expenditure for " & PlugInData.RefTypes(refTypeID), total)
            Return (result)
        End Function

        Public Shared Function GetCorpDivisionNames(ownerName As String, isWalletDivisions As Boolean) As SortedList(Of String, String)

            Dim divisionNames As New SortedList(Of String, String)

            Dim owner As PrismOwner

            If PlugInData.PrismOwners.ContainsKey(ownerName) = True Then

                owner = PlugInData.PrismOwners(ownerName)
                Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(owner, CorpRepType.Balances)
                Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(owner, CorpRepType.Balances)

                If ownerAccount IsNot Nothing Then

                    If owner.IsCorp = True Then
                        '  Dim apiReq As New EveAPIRequest(HQ.EveHQAPIServerInfo, HQ.RemoteProxy, HQ.Settings.APIFileExtension, HQ.cacheFolder)
                        ' Dim corpXML As XmlDocument = apiReq.GetAPIXML(APITypes.CorpSheet, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                        Dim corpSheetResponse = HQ.ApiProvider.Corporation.CorporationSheet(ownerAccount.UserID, ownerAccount.APIKey, ownerID.ToInt32())
                        If corpSheetResponse IsNot Nothing Then
                            ' Check response string for any error codes?
                            'Dim errlist As XmlNodeList = corpXML.SelectNodes("/eveapi/error")
                            If corpSheetResponse.IsSuccess Then
                                ' No errors so parse the files
                                'Dim divList As XmlNodeList
                                'Dim div As XmlNode
                                'divList = corpXML.SelectNodes("/eveapi/result/rowset")


                                '    Case "divisions"
                                If isWalletDivisions = False And corpSheetResponse.ResultData.Divisions IsNot Nothing Then
                                    For Each div In corpSheetResponse.ResultData.Divisions
                                        If divisionNames.ContainsKey(div.AccountKey.ToInvariantString()) = False Then
                                            divisionNames.Add(div.AccountKey.ToInvariantString(), StrConv(div.Description, VbStrConv.ProperCase))
                                        End If
                                    Next
                                End If
                                '   Case "walletDivisions"
                                If isWalletDivisions = True And corpSheetResponse.ResultData.WalletDivisions IsNot Nothing Then
                                    For Each div In corpSheetResponse.ResultData.WalletDivisions
                                        If divisionNames.ContainsKey(div.AccountKey.ToInvariantString()) = False Then
                                            divisionNames.Add(div.AccountKey.ToInvariantString(), StrConv(div.Description, VbStrConv.ProperCase))
                                        End If
                                    Next
                                End If
                            End If
                        Else
                            For divID As Integer = 1000 To 1006
                                If isWalletDivisions = False Then
                                    divisionNames.Add(divID.ToString, "Division " & divID.ToString)
                                Else
                                    divisionNames.Add(divID.ToString, "Wallet Division " & divID.ToString)
                                End If
                            Next
                        End If
                    End If
                End If
            End If

            Return divisionNames

        End Function

#End Region

#Region "Wallet Transaction Reports"

        Public Shared Function GetTransactionReportData(startDate As Date, endDate As Date, ownerNames As List(Of String)) As DataSet

            Dim strSQL As String = "SELECT * FROM walletTransactions"
            strSQL &= " WHERE walletTransactions.transDate >= '" & startDate.ToString(PrismTimeFormat, Culture) & "' AND walletTransactions.transDate < '" & endDate.ToString(PrismTimeFormat, Culture) & "'"

            ' Build the Owners List
            If ownerNames.Count > 0 Then
                Dim ownerList As New StringBuilder
                For Each ownerName As String In ownerNames
                    ownerList.Append(", '" & ownerName.Replace("'", "''") & "'")
                Next
                If ownerList.Length > 2 Then
                    ownerList.Remove(0, 2)
                End If
                ' Default to None
                strSQL &= " AND walletTransactions.charName IN (" & ownerList.ToString & ")"
            End If

            ' Order the data
            strSQL &= " ORDER BY walletTransactions.transKey ASC;"

            ' Fetch the data
            Dim walletData As DataSet = CustomDataFunctions.GetCustomData(strSQL)

            Return walletData
        End Function

        Public Shared Function GenerateTransactionSalesAnalysis(walletData As DataSet) As List(Of TransactionReportItem)

            Dim transactionList As New List(Of TransactionReportItem)
            Dim addTransaction As Boolean

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        If walletItem.Item("transType").ToString = "sell" Then

                            ' Determine if this is a valid item
                            addTransaction = False
                            If HQ.Settings.Pilots.ContainsKey(walletItem.Item("charName").ToString) = True Then
                                If walletItem.Item("transFor").ToString = "personal" Then
                                    addTransaction = True
                                End If
                            Else
                                addTransaction = True
                            End If

                            If addTransaction = True Then
                                Dim tri As New TransactionReportItem
                                tri.Date = DateTime.Parse(walletItem.Item("transDate").ToString)
                                tri.ItemName = walletItem.Item("typeName").ToString
                                tri.Owner = walletItem.Item("charName").ToString
                                tri.Price = CDbl(walletItem.Item("price"))
                                tri.Quantity = CLng(walletItem.Item("quantity"))
                                transactionList.Add(tri)
                            End If

                        End If

                    Next

                End If
            End If

            Return transactionList
        End Function

        Public Shared Function GenerateTransactionPurchasesAnalysis(walletData As DataSet) As List(Of TransactionReportItem)

            Dim transactionList As New List(Of TransactionReportItem)
            Dim addTransaction As Boolean

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        If walletItem.Item("transType").ToString = "buy" Then

                            ' Determine if this is a valid item
                            addTransaction = False
                            If HQ.Settings.Pilots.ContainsKey(walletItem.Item("charName").ToString) = True Then
                                If walletItem.Item("transFor").ToString = "personal" Then
                                    addTransaction = True
                                End If
                            Else
                                addTransaction = True
                            End If

                            If addTransaction = True Then
                                Dim tri As New TransactionReportItem
                                tri.Date = DateTime.Parse(walletItem.Item("transDate").ToString)
                                tri.ItemName = walletItem.Item("typeName").ToString
                                tri.Owner = walletItem.Item("charName").ToString
                                tri.Price = CDbl(walletItem.Item("price"))
                                tri.Quantity = CLng(walletItem.Item("quantity"))
                                transactionList.Add(tri)
                            End If

                        End If

                    Next

                End If
            End If

            Return transactionList
        End Function

        Public Shared Function GenerateTransactionProfitAnalysis(walletData As DataSet) As SortedList(Of String, TransactionProfitItem)

            Dim transactionProfitList As New SortedList(Of String, TransactionProfitItem)
            Dim addTransaction As Boolean

            If walletData IsNot Nothing Then
                If walletData.Tables(0).Rows.Count > 0 Then

                    For Each walletItem As DataRow In walletData.Tables(0).Rows

                        ' Determine if this is a valid item
                        addTransaction = False
                        If HQ.Settings.Pilots.ContainsKey(walletItem.Item("charName").ToString) = True Then
                            If walletItem.Item("transFor").ToString = "personal" Then
                                addTransaction = True
                            End If
                        Else
                            addTransaction = True
                        End If

                        If addTransaction = True Then
                            Dim tpi As New TransactionProfitItem
                            If transactionProfitList.ContainsKey(walletItem.Item("typeName").ToString) = True Then
                                tpi = transactionProfitList(walletItem.Item("typeName").ToString)
                            Else
                                tpi.ItemName = walletItem.Item("typeName").ToString
                                transactionProfitList.Add(tpi.ItemName, tpi)
                            End If

                            If walletItem.Item("transType").ToString = "buy" Then
                                tpi.QtyBought += CLng(walletItem.Item("quantity"))
                                tpi.ValueBought += CDbl(walletItem.Item("price")) * CLng(walletItem.Item("quantity"))
                            Else
                                tpi.QtySold += CLng(walletItem.Item("quantity"))
                                tpi.ValueSold += CDbl(walletItem.Item("price")) * CLng(walletItem.Item("quantity"))
                            End If

                        End If

                    Next

                End If
            End If

            Return transactionProfitList
        End Function

        Public Shared Function GenerateSalesReportBodyHTML(transList As List(Of TransactionReportItem)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>TRANSACTION SALES</b></td></tr>")

            Dim total As Double = 0

            For Each tri As TransactionReportItem In transList
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>" & tri.ItemName & "</td>")
                html.AppendLine("<td align='right'> " & (tri.Price * tri.Quantity).ToString("N2") & "</td>")
                html.AppendLine("</tr>")
                total += (tri.Price * tri.Quantity)
            Next

            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td>TRANSACTION SALES TOTAL</td>")
            html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Total Sales", total)
            Return (result)
        End Function

        Public Shared Function GeneratePurchasesReportBodyHTML(transList As List(Of TransactionReportItem)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=3><b>TRANSACTION PURCHASES</b></td></tr>")

            Dim total As Double = 0

            For Each tri As TransactionReportItem In transList
                html.AppendLine("<tr>")
                html.AppendLine("<td colspan=2></td>")
                html.AppendLine("<td>" & tri.ItemName & "</td>")
                html.AppendLine("<td align='right'> " & (tri.Price * tri.Quantity).ToString("N2") & "</td>")
                html.AppendLine("</tr>")
                total += (tri.Price * tri.Quantity)
            Next

            html.AppendLine("<tr></tr>")
            html.AppendLine("<tr>")
            html.AppendLine("<td colspan=2></td>")
            html.AppendLine("<td>TRANSACTION PURCHASES TOTAL</td>")
            html.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            result.Values.Add("Total Purchases", total)
            Return (result)
        End Function

        Public Shared Function GenerateTradingProfitReportBodyHTML(transList As SortedList(Of String, TransactionProfitItem)) As ReportResult

            Dim html As New StringBuilder

            html.AppendLine("<table width=800px border=0 align=center>")
            html.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=300></td><td width=100></td><td width=150></td><td width=150></td></tr>")
            html.AppendLine("<tr><td width=50></td><td colspan=5><b>TRADING PROFITS</b></td></tr>")
            html.AppendLine("<tr><td width=50></td><td width=50></td><td width=300></td><td align=right><i><u>Amount Traded</u></i></td><td align=right><i><u>Average Unit Profit</u></i></td><td align=right><i><u>Total Profit</u></i></td></tr>")

            Dim totalProfitAllItems As Double = 0

            For Each tri As TransactionProfitItem In transList.Values

                If tri.QtyBought <> 0 And tri.QtySold <> 0 Then

                    Dim amount As Long = If(tri.QtySold > tri.QtyBought, tri.QtyBought, tri.QtySold)
                    Dim avgProfit As Double = (tri.ValueSold / tri.QtySold) - (tri.ValueBought / tri.QtyBought)
                    Dim totalProfit As Double = Math.Round(amount * avgProfit, 2, MidpointRounding.AwayFromZero)
                    If avgProfit >= 0 Then
                        html.AppendLine("<tr class='pos'>")
                    Else
                        html.AppendLine("<tr class='neg'>")
                    End If
                    html.AppendLine("<td colspan=2></td>")
                    html.AppendLine("<td>" & tri.ItemName & "</td>")
                    html.AppendLine("<td align='right'>" & amount.ToString("N0") & "</td>")
                    html.AppendLine("<td align='right'>" & avgProfit.ToString("N2") & "</td>")
                    html.AppendLine("<td align='right'>" & totalProfit.ToString("N2") & "</td>")
                    html.AppendLine("</tr>")

                    totalProfitAllItems += totalProfit
                End If

            Next

            html.AppendLine("<tr></tr>")
            If totalProfitAllItems >= 0 Then
                html.AppendLine("<tr class='pos'>")
            Else
                html.AppendLine("<tr class='neg'>")
            End If
            html.AppendLine("<td colspan=4></td>")
            html.AppendLine("<td align='center' style='color: #FFFFFF'><b>Transaction Trading Total:</b></td>")
            html.AppendLine("<td align='right'>" & totalProfitAllItems.ToString("N2") & "</td>")
            html.AppendLine("</tr>")

            html.AppendLine("<tr></tr>")
            html.AppendLine("</table>")

            ' Return the result
            Dim result As New ReportResult
            result.HTML = html.ToString
            Return (result)
        End Function

#End Region

    End Class

    Public Enum JournalKeyTypes As Integer
        OwnerName1 = 0
        OwnerName2 = 1
        ArgName1 = 2
    End Enum

    Public Class OwnerMovement
        Public OwnerName As String = ""
        Public WalletID As String = ""
        Public StartBalance As Double = 0
        Public EndBalance As Double = 0
    End Class

    Public Class ReportResult
        Public HTML As String
        Public Values As New SortedList(Of String, Object)
    End Class

    Public Class TransactionReportItem
        Public [Date] As Date
        Public ItemName As String
        Public Quantity As Long
        Public Price As Double
        Public Owner As String
    End Class

    Public Class TransactionProfitItem
        Public ItemName As String
        Public QtyBought As Long
        Public QtySold As Long
        Public ValueBought As Double
        Public ValueSold As Double
    End Class
End Namespace