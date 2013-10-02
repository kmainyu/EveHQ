' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Namespace Requisitions

    Public Class FrmMergeRequisitions

        ReadOnly _reqList As New SortedList(Of String, Requisition)

        Public Sub New(ByVal mergeList As SortedList(Of String, Requisition))

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _reqList = mergeList

            ' Populate the combo box
            cboReqs.BeginUpdate()
            cboReqs.Items.Clear()
            For Each req As Requisition In _reqList.Values
                cboReqs.Items.Add(req.Name)
            Next
            cboReqs.Sorted = True
            cboReqs.EndUpdate()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click

            If cboReqs.SelectedItem IsNot Nothing Then
                ' Get the name of the requisition to store the merged data
                Dim newReqName As String = cboReqs.SelectedItem.ToString

                Dim mergedReq As Requisition = CType(_reqList(newReqName).Clone, Requisition)

                ' Cycle through all the other requisitions and add the items
                For Each req As Requisition In _reqList.Values
                    ' Exclude the merged req
                    If req.Name <> mergedReq.Name Then
                        mergedReq.AddOrdersFromRequisition(req.Orders)
                        ' Delete the old Req from the database if not needed to be kept
                        If chkRetainOldReqs.Checked = False Then
                            req.DeleteFromDatabase()
                        End If
                    End If
                Next

                ' Update the merged requisition in the database
                mergedReq.UpdateDatabase(_reqList(newReqName))

                ' Set the result and close the form
                DialogResult = Windows.Forms.DialogResult.OK
                Close()
            Else
                ' Set the result and close the form
                DialogResult = Windows.Forms.DialogResult.Cancel
                Close()
            End If
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Set the result and close the form
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
        End Sub

    End Class
End Namespace