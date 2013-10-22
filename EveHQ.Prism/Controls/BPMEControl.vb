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