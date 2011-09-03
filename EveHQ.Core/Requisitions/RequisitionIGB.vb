' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
Imports System.Net
Imports System.Text

Public Class RequisitionIGB

    Public Shared Function Response(ByVal context As HttpListenerContext) As String

        Dim strHTML As New StringBuilder
        strHTML.Append(EveHQ.Core.IGB.IGBHTMLHeader(context, "Requisitions", 0))
        strHTML.Append(MainMenu(context))
        Select Case context.Request.Url.AbsolutePath.ToUpper
            Case "/REQS", "/REQS/"
                strHTML.Append(MainPage(context))
            Case "/REQS/SELECTREQS", "/REQS/SELECTREQS/"
                strHTML.Append(SelectReqsPage(context))
            Case "/REQS/VIEWREQ", "/REQS/VIEWREQ/"
                strHTML.Append(ViewReqPage(context))
        End Select
        strHTML.Append(EveHQ.Core.IGB.IGBHTMLFooter(context))
        Return strHTML.ToString

    End Function

    Private Shared Function MainMenu(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append("<a href=/Reqs>Requisition Home</a>  |  <a href=/Reqs/SelectReqs>Requisition Selection</a>")
        Return strHTML.ToString
    End Function

    Shared Function MainPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        strHTML.Append("<p>This IGB feature allows you to check the current requisitions held by EveHQ.</p>")
        strHTML.Append("<p>By default, it will show the requisitions for the current logged on pilot, but requisitions for other pilots can be viewed by selecting the appropriate menu option above.</p>")
        If context.Request.Headers("EVE_CHARNAME") = "" Then
            strHTML.Append("<p>EveHQ cannot determine the current logged on pilot. Make sure you are viewing this page in the Eve IGB and have trusted the site.</p>")
        Else
            ' Get requisitions of the current pilot
            Dim Reqs As SortedList(Of String, EveHQ.Core.Requisition) = EveHQ.Core.RequisitionDataFunctions.PopulateRequisitions("", "", "", context.Request.Headers("EVE_CHARNAME"))
            If Reqs.Count = 0 Then
                ' Report we don't have any requisitions
                strHTML.Append("<p>" & context.Request.Headers("EVE_CHARNAME") & " does not have any requisitions at present.</p>")
            Else
                ' List the requisitions we do have
                strHTML.Append("<table border=1><tr><td width=300><b>Requisition Name</b></td><td width=100><b>Items</b></td><td width=150><b>Requestor</b></td><td width=150><b>Source</b></td></tr>")
                For Each newReq As EveHQ.Core.Requisition In Reqs.Values
                    strHTML.Append("<tr><td><a href='/Reqs/ViewReq?ReqName=" & newReq.Name & "'>" & newReq.Name & "</a></td><td>" & newReq.Orders.Count & "</td><td>" & newReq.Requestor & "</td><td>" & newReq.Source & "</td></tr>")
                Next
                strHTML.Append("</table>")
            End If
        End If
        strHTML.Append("<br /><br />")
        Return strHTML.ToString
    End Function

    Private Shared Function ViewReqPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder
        ' Get the Requisition Name from the query string
        Dim ReqName As String = context.Request.QueryString.Item("ReqName").ToString
        ' Get the requisition
        Dim Reqs As SortedList(Of String, EveHQ.Core.Requisition) = EveHQ.Core.RequisitionDataFunctions.PopulateRequisitions("", ReqName, "", "")
        If Reqs.Count = 0 Then
            ' Report we don't have any requisitions
            strHTML.Append("<p>Unable to find the requisition: " & ReqName & "</p>")
        Else
            Dim NewReq As EveHQ.Core.Requisition = Reqs.Item(ReqName)
            ' Detail out the requisition
            strHTML.Append("<p><b>Displaying Requisition: " & ReqName & "</b></p>")
            strHTML.Append("<p>")
            strHTML.Append("Requestor: " & NewReq.Requestor & "<br />")
            strHTML.Append("Source: " & NewReq.Source & "<br /><br />")
            strHTML.Append("<table border=1><tr style='text-align:center'><td width=300><b>Item</b></td><td width=100><b>Quantity</b></td><td width=100></td></tr>")
            For Each order As EveHQ.Core.RequisitionOrder In NewReq.Orders.Values
                Dim item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(order.ItemID)
                strHTML.Append("<tr style='vertical-align:middle'><td ><img width=24 height=24 src='")
                strHTML.Append(EveHQ.Core.ImageHandler.GetRawImageLocation(item.ID.ToString))
                strHTML.Append("' style='vertical-align:middle' />  " & order.ItemName & "</td><td style='text-align:center'>" & order.ItemQuantity & "</td>")
                strHTML.Append("<td><button type=""button"" onclick=""CCPEVE.showMarketDetails(" & order.ItemID & ")"">Show Market</button></td>")
                strHTML.Append("</tr>")
            Next
            strHTML.Append("</table>")
        End If
        strHTML.Append("<br /><br />")
        Return strHTML.ToString
    End Function

    Private Shared Function SelectReqsPage(ByVal context As Net.HttpListenerContext) As String
        Dim strHTML As New StringBuilder

        Dim Search As String = ""
        Dim Req As String = ""
        Dim Requestor As String = ""
        Dim Source As String = ""

        If context.Request.QueryString.Count > 0 Then
            Search = context.Request.QueryString.Item("Search").ToString
            Req = context.Request.QueryString.Item("Requisition").ToString.Trim
            Requestor = context.Request.QueryString.Item("Requestor").ToString.Trim
            Source = context.Request.QueryString.Item("Source").ToString.Trim
        End If

        ' Draw a search box
        strHTML.Append("<p>")
        strHTML.Append("<form method=""GET"" action=""/Reqs/SelectReqs"">")
        strHTML.Append("<table>")

        strHTML.Append("<tr><td width=100px>Search:</td><td width=250px><input type=""text"" name=""Search"" value='" & Search & "'></td></tr>")

        strHTML.Append("<tr><td width=100px>Requestor:</td><td width=250px><select name='Requestor' style='width: 200px;'>")
        strHTML.Append("<option>&nbsp;</option>")
        Dim filterData As New DataSet
        filterData = EveHQ.Core.DataFunctions.GetCustomData("SELECT DISTINCT requestor FROM requisitions;")
        If filterData IsNot Nothing Then
            For Each filterRow As DataRow In filterData.Tables(0).Rows
                strHTML.Append("<option")
                If Requestor = filterRow.Item("requestor").ToString Then
                    strHTML.Append(" selected='selected'")
                End If
                strHTML.Append(">" & filterRow.Item("requestor").ToString & "</option>")
            Next
        End If
        strHTML.Append("</select></td></tr>")

        strHTML.Append("<tr><td width=100px>Requisition:</td><td width=250px><select name='Requisition' style='width: 200px;'>")
        strHTML.Append("<option>&nbsp;</option>")
        filterData = EveHQ.Core.DataFunctions.GetCustomData("SELECT DISTINCT requisition FROM requisitions;")
        If filterData IsNot Nothing Then
            For Each filterRow As DataRow In filterData.Tables(0).Rows
                strHTML.Append("<option")
                If Req = filterRow.Item("requisition").ToString Then
                    strHTML.Append(" selected='selected'")
                End If
                strHTML.Append(">" & filterRow.Item("requisition").ToString & "</option>")
            Next
        End If
        strHTML.Append("</select></td></tr>")

        strHTML.Append("<tr><td width=100px>Source:</td><td width=250px><select name='Source' style='width: 200px;'>")
        strHTML.Append("<option>&nbsp;</option>")
        filterData = EveHQ.Core.DataFunctions.GetCustomData("SELECT DISTINCT source FROM requisitions;")
        If filterData IsNot Nothing Then
            For Each filterRow As DataRow In filterData.Tables(0).Rows
                strHTML.Append("<option")
                If Source = filterRow.Item("source").ToString Then
                    strHTML.Append(" selected='selected'")
                End If
                strHTML.Append(">" & filterRow.Item("source").ToString & "</option>")
            Next
        End If
        strHTML.Append("</select></td></tr>")
        strHTML.Append("<br><tr><td><input type='submit' value='Get Requistions'></td></tr></table></form>")

        ' Get the Requisition Name from the query string
        If context.Request.QueryString.Count > 0 Then
            ' Get the requisition
            Dim Reqs As SortedList(Of String, EveHQ.Core.Requisition) = EveHQ.Core.RequisitionDataFunctions.PopulateRequisitions(Search, Req, Source, Requestor)
            If Reqs.Count = 0 Then
                ' Report we don't have any requisitions
                strHTML.Append("<p>Unable to find any matching Requisitions.</p>")
            Else
                ' Detail out the requisition
                strHTML.Append("<p><b>Displaying Requisitions matching filters</p>")
                ' List the requisitions we do have
                strHTML.Append("<table border=1><tr><td width=300><b>Requisition Name</b></td><td width=100><b>Items</b></td><td width=150><b>Requestor</b></td><td width=150><b>Source</b></td></tr>")
                For Each newReq As EveHQ.Core.Requisition In Reqs.Values
                    strHTML.Append("<tr><td><a href='/Reqs/ViewReq?ReqName=" & newReq.Name & "'>" & newReq.Name & "</a></td><td>" & newReq.Orders.Count & "</td><td>" & newReq.Requestor & "</td><td>" & newReq.Source & "</td></tr>")
                Next
                strHTML.Append("</table>")
            End If
            strHTML.Append("<br /><br />")
        End If
        Return strHTML.ToString
    End Function

End Class
