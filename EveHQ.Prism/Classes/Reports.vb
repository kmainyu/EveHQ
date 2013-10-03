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

Imports System.Text
Imports System.Windows.Forms
Imports ZedGraph
Imports System.Drawing

Public Class Reports

    Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Shared SQLTimeFormat As String = "yyyyMMdd HH:mm:ss"
    Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

#Region "Standard Report Functions"

    Public Shared Function HTMLHeader(ByVal BrowserHeader As String, ReportTitle As String) As String
        Dim HTML As New StringBuilder
        HTML.AppendLine("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""http://www.w3.org/TR/html4/strict.dtd"">")
        HTML.AppendLine("<html lang=""" & System.Globalization.CultureInfo.CurrentCulture.ToString & """>")
        HTML.AppendLine("<head>")
        HTML.AppendLine("<META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">")
        HTML.AppendLine("<title>" & BrowserHeader & "</title>" & ReportCSS() & "</head>")
        HTML.AppendLine("<body>")
        If ReportTitle <> "" Then
            HTML.AppendLine("<table width=800px border=0 align=center>")
            HTML.AppendLine("<tr height=30px><td align='center'><p class=title>" & ReportTitle & "</p></td></tr>")
            HTML.AppendLine("</table>")
            HTML.AppendLine("<p></p>")
        End If
        Return HTML.ToString
    End Function

    Public Shared Function HTMLTitle(ByVal Title As String) As String
        Dim HTML As New StringBuilder
        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr height=30px><td><p class=title>" & Title & "</p></td></tr>")
        HTML.AppendLine("</table>")
        HTML.AppendLine("<p></p>")
        Return HTML.ToString
    End Function

    Public Shared Function HTMLFooter() As String
        Dim HTML As New StringBuilder
        HTML.AppendLine("<table width=800px align=center border=0><hr>")
        HTML.AppendLine("<tr><td><p align=center class=footer>Generated on " & Now.ToString & " by <a href='http://www.evehq.net'>" & My.Application.Info.ProductName & "</a> v" & My.Application.Info.Version.ToString & "</p></td></tr>")
        HTML.AppendLine("</table>")
        HTML.AppendLine("</body></html>")
        Return HTML.ToString
    End Function

    Private Shared Function ReportCSS() As String
        Dim CSS As New StringBuilder
        CSS.AppendLine("<STYLE><!--")
        CSS.AppendLine("BODY { font-family: Tahoma, Arial; font-size: 12px; bgcolor: #000000; background: #000000 }")
        CSS.AppendLine("TD, P { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff }")
        CSS.AppendLine(".thead { font-family: Tahoma, Arial; font-size: 12px; color: #ffffff; font-variant: small-caps; background-color: #444444 }")
        CSS.AppendLine(".footer { font-family: Tahoma, Arial; font-size: 9px; color: #ffffff; font-variant: small-caps }")
        CSS.AppendLine(".title { font-family: Tahoma, Arial; font-size: 20px; color: #ffffff; font-variant: small-caps }")
        CSS.AppendLine("#wrapper {overflow: auto; height: 100%; width:820px; margin-left:auto; margin-right:auto;}")
        CSS.AppendLine("tr.pos td {color: #00ff00;}")
        CSS.AppendLine("tr.neg td {color: #ff0000;}")
        CSS.AppendLine("--></STYLE>")
        Return CSS.ToString
    End Function

#End Region

#Region "Wallet Journal Reports"

    Public Shared Function GetJournalReportData(StartDate As Date, EndDate As Date, OwnerNames As List(Of String)) As DataSet

        Dim strSQL As String = "SELECT * FROM walletJournal"
        strSQL &= " WHERE walletJournal.transDate >= '" & StartDate.ToString(SQLTimeFormat, culture) & "' AND walletJournal.transDate < '" & EndDate.ToString(SQLTimeFormat, culture) & "'"

        ' Build the Owners List
        If OwnerNames.Count > 0 Then
            Dim OwnerList As New StringBuilder
            For Each OwnerName As String In OwnerNames
                OwnerList.Append(", '" & OwnerName.Replace("'", "''") & "'")
            Next
            If OwnerList.Length > 2 Then
                OwnerList.Remove(0, 2)
            End If
            ' Default to None
            strSQL &= " AND walletJournal.charName IN (" & OwnerList.ToString & ")"
        End If

        ' Order the data
        strSQL &= " ORDER BY walletJournal.transKey ASC;"

        ' Fetch the data
        Dim walletData As DataSet = Core.CustomDataFunctions.GetCustomData(strSQL)

        Return walletData

    End Function

    Public Shared Function GenerateIncomeAnalysis(WalletData As DataSet) As SortedList(Of String, Double)

        Dim RefTypeList As New SortedList(Of String, Double)

        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                For Each WalletItem As DataRow In WalletData.Tables(0).Rows

                    Dim RefTypeID As String = CStr(WalletItem.Item("refTypeID"))
                    'Dim Value As Double = Double.Parse(WalletItem.Item("amount").ToString, culture)
                    Dim Value As Double = CDbl(WalletItem.Item("amount"))

                    If Value > 0 Then

                        If RefTypeList.ContainsKey(RefTypeID) = False Then
                            RefTypeList.Add(RefTypeID, Value)
                        Else
                            RefTypeList(RefTypeID) += Value
                        End If

                    End If

                Next

            End If
        End If

        Return RefTypeList

    End Function

    Public Shared Function GenerateExpenseAnalysis(WalletData As DataSet) As SortedList(Of String, Double)

        Dim RefTypeList As New SortedList(Of String, Double)

        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                For Each WalletItem As DataRow In WalletData.Tables(0).Rows

                    Dim RefTypeID As String = CStr(WalletItem.Item("refTypeID"))
                    'Dim Value As Double = Double.Parse(WalletItem.Item("amount").ToString, culture)
                    Dim Value As Double = CDbl(WalletItem.Item("amount"))

                    If Value < 0 Then

                        If RefTypeList.ContainsKey(RefTypeID) = False Then
                            RefTypeList.Add(RefTypeID, Value)
                        Else
                            RefTypeList(RefTypeID) += Value
                        End If

                    End If

                Next

            End If
        End If

        Return RefTypeList

    End Function

    Public Shared Function GenerateCTAnalysis(ByVal WalletData As DataSet) As SortedList(Of String, Double)

        Dim CorpTaxList As New SortedList(Of String, Double)

        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                For Each WalletItem As DataRow In WalletData.Tables(0).Rows

                    Dim RefTypeID As String = CStr(WalletItem.Item("refTypeID"))
                    Dim Value As Double = CDbl(WalletItem.Item("amount"))
                    Dim Owner As String = CStr(WalletItem.Item("ownerName2"))

                    Select Case RefTypeID
                        Case "33", "34", "85", "99"
                            If CorpTaxList.ContainsKey(Owner) = False Then
                                CorpTaxList.Add(Owner, Value)
                            Else
                                CorpTaxList(Owner) += Value
                            End If

                        Case Else

                    End Select

                Next

            End If
        End If

        Return CorpTaxList

    End Function

    Public Shared Function GenerateOwnerMovements(ByVal WalletData As DataSet) As SortedList(Of String, SortedList(Of String, OwnerMovement))

        Dim OwnerMovements As New SortedList(Of String, SortedList(Of String, OwnerMovement))

        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                For Each WalletItem As DataRow In WalletData.Tables(0).Rows

                    ' Check if we have the owner listed
                    Dim OwnerName As String = CStr(WalletItem.Item("charName"))
                    If OwnerMovements.ContainsKey(OwnerName) = False Then
                        OwnerMovements.Add(OwnerName, New SortedList(Of String, OwnerMovement))
                    End If

                    'Dim amount As Double = Double.Parse(WalletItem.Item("amount").ToString, culture)
                    'Dim balance As Double = Double.Parse(WalletItem.Item("balance").ToString, culture)
                    'Dim taxamount As Double = Double.Parse(WalletItem.Item("taxAmount").ToString, culture)
                    Dim amount As Double = CDbl(WalletItem.Item("amount"))
                    Dim balance As Double = CDbl(WalletItem.Item("balance"))
                    Dim taxamount As Double = CDbl(WalletItem.Item("taxAmount"))

                    ' Check we have a wallet balance set up
                    Dim WalletID As String = CStr(WalletItem.Item("walletID"))
                    Dim Movement As New OwnerMovement
                    If OwnerMovements(OwnerName).ContainsKey(WalletID) = False Then
                        If amount <> -taxamount Then
                            Movement.OwnerName = OwnerName
                            Movement.WalletID = WalletID
                            Movement.StartBalance = balance - amount + taxamount
                            OwnerMovements(OwnerName).Add(WalletID, Movement)
                        End If
                    Else
                        Movement = OwnerMovements(OwnerName).Item(WalletID)
                    End If
                    Movement.EndBalance = balance

                Next

            End If
        End If

        Return OwnerMovements

    End Function

    Public Shared Function GenerateIncomeReportBodyHTML(RefTypeList As SortedList(Of String, Double)) As ReportResult

        Dim HTML As New StringBuilder

        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td colspan=3><b>INCOME</b></td></tr>")

        Dim Total As Double = 0

        For Each RefTypeID As String In RefTypeList.Keys
            HTML.AppendLine("<tr>")
            HTML.AppendLine("<td colspan=2></td>")
            HTML.AppendLine("<td>" & PlugInData.RefTypes(RefTypeID) & "</td>")
            HTML.AppendLine("<td align='right'> " & RefTypeList(RefTypeID).ToString("N2") & "</td>")
            HTML.AppendLine("</tr>")
            Total += RefTypeList(RefTypeID)
        Next

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=2></td>")
        HTML.AppendLine("<td>INCOME TOTAL</td>")
        HTML.AppendLine("<td align='right'> " & Total.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        HTML.AppendLine("</table>")

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Result.Values.Add("Total Income", Total)
        Return (Result)

    End Function

    Public Shared Function GenerateExpenseReportBodyHTML(RefTypeList As SortedList(Of String, Double)) As ReportResult

        Dim HTML As New StringBuilder

        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td colspan=3><b>EXPENDITURE</b></td></tr>")

        Dim Total As Double = 0

        For Each RefTypeID As String In RefTypeList.Keys
            HTML.AppendLine("<tr>")
            HTML.AppendLine("<td colspan=2></td>")
            HTML.AppendLine("<td>" & PlugInData.RefTypes(RefTypeID) & "</td>")
            HTML.AppendLine("<td align='right'> " & RefTypeList(RefTypeID).ToString("N2") & "</td>")
            HTML.AppendLine("</tr>")
            Total += RefTypeList(RefTypeID)
        Next

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=2></td>")
        HTML.AppendLine("<td>EXPENDITURE TOTAL</td>")
        HTML.AppendLine("<td align='right'> " & Total.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        HTML.AppendLine("</table>")

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Result.Values.Add("Total Expenditure", Total)
        Return (Result)

    End Function

    Public Shared Function GenerateCTReportBodyHTML(ByVal CorpTaxList As SortedList(Of String, Double)) As ReportResult

        Dim HTML As New StringBuilder

        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td colspan=3><b>CORPORATION TAX ANALYSIS</b></td></tr>")

        Dim Total As Double = 0

        For Each Owner As String In CorpTaxList.Keys
            HTML.AppendLine("<tr>")
            HTML.AppendLine("<td colspan=2></td>")
            HTML.AppendLine("<td>" & Owner & "</td>")
            HTML.AppendLine("<td align='right'> " & CorpTaxList(Owner).ToString("N2") & "</td>")
            HTML.AppendLine("</tr>")
            Total += CorpTaxList(Owner)
        Next

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=2></td>")
        HTML.AppendLine("<td>CORPORATION TAX TOTAL</td>")
        HTML.AppendLine("<td align='right'> " & Total.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        HTML.AppendLine("</table>")

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Result.Values.Add("Total Corporation Tax", Total)
        Return (Result)

    End Function

    Public Shared Function GenerateCashFlowReportBodyHTML(Income As Double, Expenditure As Double) As ReportResult

        Dim HTML As New StringBuilder

        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=500></td><td width=200></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td colspan=3><b>CASH FLOW</b></td></tr>")

        ' Write Income
        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=2></td>")
        HTML.AppendLine("<td>Income Total</td>")
        HTML.AppendLine("<td align='right'> " & Income.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        ' Write Expenditure
        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=2></td>")
        HTML.AppendLine("<td>Expenditure Total</td>")
        HTML.AppendLine("<td align='right'> " & Expenditure.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        ' Write Cash Flow
        Dim CashFlow As Double = Income + Expenditure
        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=2></td>")
        If CashFlow >= 0 Then
            HTML.AppendLine("<td>NET CASH INFLOW</td>")
        Else
            HTML.AppendLine("<td>NET CASH OUTFLOW</td>")
        End If
        HTML.AppendLine("<td align='right'> " & CashFlow.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")
        HTML.AppendLine("</table>")

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Result.Values.Add("Cash Flow", CashFlow)
        Return (Result)

    End Function

    Public Shared Function GenerateMovementReportBodyHTML(ByVal Movements As SortedList(Of String, SortedList(Of String, OwnerMovement))) As ReportResult

        Dim HTML As New StringBuilder

        If Movements.Count > 0 Then

            HTML.AppendLine("<table width=800px border=0 align=center>")
            HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=250></td><td width=150></td><td width=150></td><td width=150></td></tr>")
            HTML.AppendLine("<tr><td width=50></td><td colspan=3><b>OWNER MOVEMENTS</b></td></tr>")
            HTML.AppendLine("<tr><td width=50></td><td width=50></td><td width=250></td><td width=150 align=center><i>Start Balance</i></td><td width=150 align=center><i>End Balance</i></td><td width=150 align=center><i>Movement</i></td></tr>")

            Dim total As Double = 0

            For Each Owner As String In Movements.Keys
                Dim StartBalance As Double = 0
                Dim EndBalance As Double = 0
                For Each Movement As OwnerMovement In Movements(Owner).Values
                    StartBalance += Movement.StartBalance
                    EndBalance += Movement.EndBalance
                Next
                HTML.AppendLine("<tr>")
                HTML.AppendLine("<td colspan=2></td>")
                HTML.AppendLine("<td>" & Owner & "</td>")
                HTML.AppendLine("<td align='right'> " & StartBalance.ToString("N2") & "</td>")
                HTML.AppendLine("<td align='right'> " & EndBalance.ToString("N2") & "</td>")
                HTML.AppendLine("<td align='right'> " & (EndBalance - StartBalance).ToString("N2") & "</td>")
                HTML.AppendLine("</tr>")

                total += EndBalance - StartBalance

            Next

            HTML.AppendLine("<tr></tr>")
            HTML.AppendLine("<tr>")
            HTML.AppendLine("<td colspan=2></td>")
            HTML.AppendLine("<td>TOTAL MOVEMENT</td>")
            HTML.AppendLine("<td colspan=2></td>")
            HTML.AppendLine("<td align='right'> " & total.ToString("N2") & "</td>")
            HTML.AppendLine("</tr>")

        End If

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Return (Result)

    End Function

#End Region

#Region "Wallet Transaction Reports"

    Public Shared Function GetTransactionReportData(StartDate As Date, EndDate As Date, OwnerNames As List(Of String)) As DataSet

        Dim strSQL As String = "SELECT * FROM walletTransactions"
        strSQL &= " WHERE walletTransactions.transDate >= '" & StartDate.ToString(SQLTimeFormat, culture) & "' AND walletTransactions.transDate < '" & EndDate.ToString(SQLTimeFormat, culture) & "'"

        ' Build the Owners List
        If OwnerNames.Count > 0 Then
            Dim OwnerList As New StringBuilder
            For Each OwnerName As String In OwnerNames
                OwnerList.Append(", '" & OwnerName.Replace("'", "''") & "'")
            Next
            If OwnerList.Length > 2 Then
                OwnerList.Remove(0, 2)
            End If
            ' Default to None
            strSQL &= " AND walletTransactions.charName IN (" & OwnerList.ToString & ")"
        End If

        ' Order the data
        strSQL &= " ORDER BY walletTransactions.transKey ASC;"

        ' Fetch the data
        Dim walletData As DataSet = Core.CustomDataFunctions.GetCustomData(strSQL)

        Return walletData

    End Function

    Public Shared Function GenerateTransactionSalesAnalysis(WalletData As DataSet) As List(Of TransactionReportItem)

        Dim TransactionList As New List(Of TransactionReportItem)
        Dim AddTransaction As Boolean = False

        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                For Each WalletItem As DataRow In WalletData.Tables(0).Rows

                    If WalletItem.Item("transType").ToString = "sell" Then

                        ' Determine if this is a valid item
                        AddTransaction = False
                        If EveHQ.Core.HQ.Settings.Pilots.ContainsKey(WalletItem.Item("charName").ToString) = True Then
                            If WalletItem.Item("transFor").ToString = "personal" Then
                                AddTransaction = True
                            End If
                        Else
                            AddTransaction = True
                        End If

                        If AddTransaction = True Then
                            Dim TRI As New TransactionReportItem
                            TRI.Date = DateTime.Parse(WalletItem.Item("transDate").ToString)
                            TRI.ItemName = WalletItem.Item("typeName").ToString
                            TRI.Owner = WalletItem.Item("charName").ToString
                            TRI.Price = CDbl(WalletItem.Item("price"))
                            TRI.Quantity = CLng(WalletItem.Item("quantity"))
                            TransactionList.Add(TRI)
                        End If

                    End If

                Next

            End If
        End If

        Return TransactionList

    End Function

    Public Shared Function GenerateTransactionPurchasesAnalysis(WalletData As DataSet) As List(Of TransactionReportItem)

        Dim TransactionList As New List(Of TransactionReportItem)
        Dim AddTransaction As Boolean = False

        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                For Each WalletItem As DataRow In WalletData.Tables(0).Rows

                    If WalletItem.Item("transType").ToString = "buy" Then

                        ' Determine if this is a valid item
                        AddTransaction = False
                        If EveHQ.Core.HQ.Settings.Pilots.ContainsKey(WalletItem.Item("charName").ToString) = True Then
                            If WalletItem.Item("transFor").ToString = "personal" Then
                                AddTransaction = True
                            End If
                        Else
                            AddTransaction = True
                        End If

                        If AddTransaction = True Then
                            Dim TRI As New TransactionReportItem
                            TRI.Date = DateTime.Parse(WalletItem.Item("transDate").ToString)
                            TRI.ItemName = WalletItem.Item("typeName").ToString
                            TRI.Owner = WalletItem.Item("charName").ToString
                            TRI.Price = CDbl(WalletItem.Item("price"))
                            TRI.Quantity = CLng(WalletItem.Item("quantity"))
                            TransactionList.Add(TRI)
                        End If

                    End If

                Next

            End If
        End If

        Return TransactionList

    End Function

    Public Shared Function GenerateTransactionProfitAnalysis(WalletData As DataSet) As SortedList(Of String, TransactionProfitItem)

        Dim TransactionProfitList As New SortedList(Of String, TransactionProfitItem)
        Dim AddTransaction As Boolean = False

        If WalletData IsNot Nothing Then
            If WalletData.Tables(0).Rows.Count > 0 Then

                For Each WalletItem As DataRow In WalletData.Tables(0).Rows

                    ' Determine if this is a valid item
                    AddTransaction = False
                    If EveHQ.Core.HQ.Settings.Pilots.ContainsKey(WalletItem.Item("charName").ToString) = True Then
                        If WalletItem.Item("transFor").ToString = "personal" Then
                            AddTransaction = True
                        End If
                    Else
                        AddTransaction = True
                    End If

                    If AddTransaction = True Then
                        Dim TPI As New TransactionProfitItem
                        If TransactionProfitList.ContainsKey(WalletItem.Item("typeName").ToString) = True Then
                            TPI = TransactionProfitList(WalletItem.Item("typeName").ToString)
                        Else
                            TPI.ItemName = WalletItem.Item("typeName").ToString
                            TransactionProfitList.Add(TPI.ItemName, TPI)
                        End If

                        If WalletItem.Item("transType").ToString = "buy" Then
                            TPI.QtyBought += CLng(WalletItem.Item("quantity"))
                            TPI.ValueBought += CDbl(WalletItem.Item("price")) * CLng(WalletItem.Item("quantity"))
                        Else
                            TPI.QtySold += CLng(WalletItem.Item("quantity"))
                            TPI.ValueSold += CDbl(WalletItem.Item("price")) * CLng(WalletItem.Item("quantity"))
                        End If

                    End If

                Next

            End If
        End If

        Return TransactionProfitList

    End Function

    Public Shared Function GenerateSalesReportBodyHTML(ByVal TransList As SortedList(Of String, TransactionProfitItem)) As ReportResult

        Dim HTML As New StringBuilder

        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=300></td><td width=100></td><td width=150></td><td width=150></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td colspan=5><b>TRANSACTION SALES</b></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td width=50></td><td width=300></td><td align=right><i><u>Amount Sold</u></i></td><td align=right><i><u>Average Unit Price</u></i></td><td align=right><i><u>Total Price</u></i></td></tr>")

        Dim Total As Double = 0

        For Each TPI As TransactionProfitItem In TransList.Values
            If TPI.QtySold > 0 Then
                HTML.AppendLine("<tr>")
                HTML.AppendLine("<td colspan=2></td>")
                HTML.AppendLine("<td>" & TPI.ItemName & "</td>")
                HTML.AppendLine("<td align='right'>" & TPI.QtySold.ToString("N0") & "</td>")
                HTML.AppendLine("<td align='right'>" & (TPI.ValueSold / TPI.QtySold).ToString("N2") & "</td>")
                HTML.AppendLine("<td align='right'>" & TPI.ValueSold.ToString("N2") & "</td>")
                HTML.AppendLine("</tr>")
                Total += TPI.ValueSold
            End If
        Next

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=4></td>")
        HTML.AppendLine("<td align='center'><b>Transaction Sales Total:</b></td>")
        HTML.AppendLine("<td align='right'> " & Total.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("</table>")

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Result.Values.Add("Total Sales", Total)
        Return (Result)

    End Function

    Public Shared Function GeneratePurchasesReportBodyHTML(ByVal TransList As SortedList(Of String, TransactionProfitItem)) As ReportResult

        Dim HTML As New StringBuilder

        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=300></td><td width=100></td><td width=150></td><td width=150></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td colspan=5><b>TRANSACTION PURCHASES</b></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td width=50></td><td width=300></td><td align=right><i><u>Amount Bought</u></i></td><td align=right><i><u>Average Unit Price</u></i></td><td align=right><i><u>Total Price</u></i></td></tr>")

        Dim Total As Double = 0

        For Each TPI As TransactionProfitItem In TransList.Values
            If TPI.QtyBought > 0 Then
                HTML.AppendLine("<tr>")
                HTML.AppendLine("<td colspan=2></td>")
                HTML.AppendLine("<td>" & TPI.ItemName & "</td>")
                HTML.AppendLine("<td align='right'>" & TPI.QtyBought.ToString("N0") & "</td>")
                HTML.AppendLine("<td align='right'>" & (TPI.ValueBought / TPI.QtyBought).ToString("N2") & "</td>")
                HTML.AppendLine("<td align='right'>" & TPI.ValueBought.ToString("N2") & "</td>")
                HTML.AppendLine("</tr>")
                Total += TPI.ValueBought
            End If
        Next

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("<tr>")
        HTML.AppendLine("<td colspan=4></td>")
        HTML.AppendLine("<td align='center'><b>Transaction Purchases Total:</b></td>")
        HTML.AppendLine("<td align='right'> " & Total.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("</table>")

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Result.Values.Add("Total Purchases", Total)
        Return (Result)

    End Function

    Public Shared Function GenerateTradingProfitReportBodyHTML(ByVal TransList As SortedList(Of String, TransactionProfitItem)) As ReportResult

        Dim HTML As New StringBuilder

        'Begin Amount and Total Profit version
        HTML.AppendLine("<table width=800px border=0 align=center>")
        HTML.AppendLine("<tr><td width=50>&nbsp;</td><td width=50></td><td width=300></td><td width=100></td><td width=150></td><td width=150></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td colspan=5><b>TRADING PROFITS</b></td></tr>")
        HTML.AppendLine("<tr><td width=50></td><td width=50></td><td width=300></td><td align=right><i><u>Amount Traded</u></i></td><td align=right><i><u>Average Unit Profit</u></i></td><td align=right><i><u>Total Profit</u></i></td></tr>")

        Dim totalProfitAllItems As Double = 0

        For Each TPI As TransactionProfitItem In TransList.Values
            If TPI.QtyBought <> 0 And TPI.QtySold <> 0 Then
                Dim amount As Long = If(TPI.QtySold > TPI.QtyBought, TPI.QtyBought, TPI.QtySold)
                Dim avgProfit As Double = (TPI.ValueSold / TPI.QtySold) - (TPI.ValueBought / TPI.QtyBought)
                Dim totalProfit As Double = Math.Round(amount * avgProfit, 2, MidpointRounding.AwayFromZero)
                If avgProfit >= 0 Then
                    HTML.AppendLine("<tr class='pos'>")
                Else
                    HTML.AppendLine("<tr class='neg'>")
                End If
                HTML.AppendLine("<td colspan=2></td>")
                HTML.AppendLine("<td>" & TPI.ItemName & "</td>")
                HTML.AppendLine("<td align='right'>" & amount.ToString("N0") & "</td>")
                HTML.AppendLine("<td align='right'>" & avgProfit.ToString("N2") & "</td>")
                HTML.AppendLine("<td align='right'>" & totalProfit.ToString("N2") & "</td>")
                HTML.AppendLine("</tr>")
                totalProfitAllItems += totalProfit
            End If
        Next

        HTML.AppendLine("<tr></tr>")
        If totalProfitAllItems >= 0 Then
            HTML.AppendLine("<tr class='pos'>")
        Else
            HTML.AppendLine("<tr class='neg'>")
        End If
        HTML.AppendLine("<td colspan=4></td>")
        HTML.AppendLine("<td align='center' style='color: #FFFFFF'><b>Transaction Trading Total:</b></td>")
        HTML.AppendLine("<td align='right'>" & totalProfitAllItems.ToString("N2") & "</td>")
        HTML.AppendLine("</tr>")

        HTML.AppendLine("<tr></tr>")
        HTML.AppendLine("</table>")
        'End Amount and Total Profit version

        ' Return the result
        Dim Result As New ReportResult
        Result.HTML = HTML.ToString
        Return (Result)

    End Function

#End Region

#Region "Charts"

    Public Shared Function GenerateWalletBalanceHistoryAnalysis(ReportData As DataSet) As SortedList(Of String, SortedList(Of Date, Double))
        Dim Owners As New SortedList(Of String, SortedList(Of Date, Double))

        If ReportData IsNot Nothing Then
            If ReportData.Tables(0).Rows.Count > 0 Then

                For Each ReportItem As DataRow In ReportData.Tables(0).Rows

                    Dim Owner As String = CStr(ReportItem.Item("charName"))
                    Dim BalDate As Date = Date.Parse(CStr(ReportItem.Item("transDate")))
                    'Dim Balance As Double = Double.Parse(CStr(ReportItem.Item("balance")), culture)
                    Dim Balance As Double = CDbl(ReportItem.Item("balance"))

                    If Owners.ContainsKey(Owner) = False Then
                        Owners.Add(Owner, New SortedList(Of Date, Double))
                    End If

                    Dim CurrentOwner As SortedList(Of Date, Double) = Owners(Owner)
                    If CurrentOwner.ContainsKey(BalDate) = False Then
                        CurrentOwner.Add(BalDate, Balance)
                    Else
                        CurrentOwner(BalDate) = Balance
                    End If

                Next

            End If
        End If

        Return Owners

    End Function

    Public Shared Sub GenerateWalletBalanceHistoryAnalysisChart(ChartControl As ZedGraphControl, Owners As SortedList(Of String, SortedList(Of Date, Double)))

        Dim ChartPane As GraphPane = ChartControl.GraphPane

        ChartPane.CurveList.Clear()

        ' Set the titles and axis labels
        ChartPane.Title.Text = "Wallet Balance Analysis"
        ChartPane.XAxis.Title.Text = "Date"
        ChartPane.YAxis.Title.Text = "Balance (ISK, Millions)"
        ChartPane.XAxis.Type = AxisType.Date
        ChartPane.YAxis.Type = AxisType.Linear
        ChartPane.YAxis.Title.IsOmitMag = True
        ChartPane.YAxis.Scale.Mag = 6
        ChartPane.XAxis.Scale.MinAuto = True
        ChartPane.XAxis.Scale.MaxAuto = True
        ChartPane.YAxis.Scale.MinAuto = True
        ChartPane.YAxis.Scale.MaxAuto = True


        Dim RandomClass As New Random(Now.Millisecond)
        For Each owner As String In Owners.Keys
            Dim OwnerData As New PointPairList
            For Each BalDate As Date In Owners(owner).Keys
                OwnerData.Add(CDbl(BalDate.ToOADate), Owners(owner).Item(BalDate))
            Next
            Dim intR, intG, intB As Integer
            intR = CInt(RandomClass.Next(0, 256) / 2)
            intG = CInt(RandomClass.Next(0, 256) / 2)
            intB = CInt(RandomClass.Next(0, 256) / 2)
            Dim myCurve As LineItem = ChartPane.AddCurve(owner, OwnerData, Color.FromArgb(intR, intG, intB), SymbolType.Circle)
            myCurve.Line.IsAntiAlias = True
            myCurve.Line.Width = 2
            myCurve.Line.StepType = StepType.ForwardStep
        Next

        ' Fill the axis background with a color gradient
        ChartPane.Chart.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        ChartPane.Legend.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        ChartPane.Legend.FontSpec.FontColor = Color.MidnightBlue

        ' Fill the pane background with a color gradient
        ChartPane.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)

        ' Calculate the Axis Scale Ranges
        ChartControl.AxisChange()
        ChartControl.Refresh()

    End Sub

#End Region


End Class

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

