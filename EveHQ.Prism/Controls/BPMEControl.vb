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

Imports EveHQ.Prism.BPCalc

Namespace Controls

    Public Class BlueprintMEControl

        Dim _parentJob As Job
        Dim _assignedJob As Job
        Dim _assignedTypeID As String

        Public Event ResourcesChanged()

        Public Property ParentJob() As Job
            Get
                Return _parentJob
            End Get
            Set(ByVal value As Job)
                _parentJob = value
            End Set
        End Property

        Public Property AssignedJob() As Job
            Get
                Return _assignedJob
            End Get
            Set(ByVal value As Job)
                _assignedJob = value
            End Set
        End Property

        Public Property AssignedTypeID() As String
            Get
                Return _assignedTypeID
            End Get
            Set(ByVal value As String)
                _assignedTypeID = value
            End Set
        End Property

        Private Sub nudME_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles nudME.ButtonCustomClick
            If _assignedTypeID <> "" Then
                If _assignedJob IsNot Nothing Then
                    _assignedJob.CurrentBlueprint.MELevel = nudME.Value
                    ParentJob.RecalculateResourceRequirements()
                    RaiseEvent ResourcesChanged()
                    If _assignedJob.CurrentBlueprint.MELevel = nudME.Value Then
                        nudME.ButtonCustom.Enabled = False
                    Else
                        nudME.ButtonCustom.Enabled = True
                    End If
                End If
            End If
        End Sub

        Private Sub nudME_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudME.ValueChanged
            nudME.ButtonCustom.Enabled = False
            If _assignedTypeID <> "" Then
                If _assignedJob IsNot Nothing Then
                    If _assignedJob.CurrentBlueprint.MELevel = nudME.Value Then
                        nudME.ButtonCustom.Enabled = False
                    Else
                        nudME.ButtonCustom.Enabled = True
                    End If
                End If
            End If
        End Sub

        Private Sub nudME_LockUpdateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudME.LockUpdateChanged
            If _assignedTypeID <> "" Then
                If nudME.LockUpdateChecked = True Then
                    ParentJob.ReplaceResourceWithJob(CInt(AssignedTypeID))
                Else
                    ParentJob.ReplaceJobWithResource(CInt(AssignedTypeID))
                    _assignedJob.CurrentBlueprint.MELevel = nudME.Value
                End If
                RaiseEvent ResourcesChanged()
            End If
        End Sub

    End Class
End NameSpace