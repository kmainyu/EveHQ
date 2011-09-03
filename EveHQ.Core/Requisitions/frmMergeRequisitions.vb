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
Public Class frmMergeRequisitions

    Dim ReqList As New SortedList(Of String, EveHQ.Core.Requisition)

    Public Sub New(ByVal MergeList As SortedList(Of String, EveHQ.Core.Requisition))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ReqList = MergeList

        ' Populate the combo box
        cboReqs.BeginUpdate()
        cboReqs.Items.Clear()
        For Each Req As EveHQ.Core.Requisition In ReqList.Values
            cboReqs.Items.Add(Req.Name)
        Next
        cboReqs.Sorted = True
        cboReqs.EndUpdate()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        If cboReqs.SelectedItem IsNot Nothing Then
            ' Get the name of the requisition to store the merged data
            Dim NewReqName As String = cboReqs.SelectedItem.ToString

            Dim MergedReq As EveHQ.Core.Requisition = CType(ReqList(NewReqName).Clone, Requisition)

            ' Cycle through all the other requisitions and add the items
            For Each Req As EveHQ.Core.Requisition In ReqList.Values
                ' Exclude the merged req
                If Req.Name <> MergedReq.Name Then
                    MergedReq.AddOrdersFromRequisition(Req.Orders)
                    ' Delete the old Req from the database if not needed to be kept
                    If chkRetainOldReqs.Checked = False Then
                        Req.DeleteFromDatabase()
                    End If
                End If
            Next

            ' Update the merged requisition in the database
            MergedReq.UpdateDatabase(ReqList(NewReqName))

            ' Set the result and close the form
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            ' Set the result and close the form
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' Set the result and close the form
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class