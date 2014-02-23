'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Windows.Forms

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
                DialogResult = DialogResult.OK
                Close()
            Else
                ' Set the result and close the form
                DialogResult = DialogResult.Cancel
                Close()
            End If
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            ' Set the result and close the form
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

    End Class
End Namespace