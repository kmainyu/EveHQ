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

Public Class BPMEControl

    Dim cParentJob As ProductionJob
    Dim cAssignedJob As ProductionJob
    Dim cAssignedTypeID As String

    Public Event ResourcesChanged()

    Public Property ParentJob() As ProductionJob
        Get
            Return cParentJob
        End Get
        Set(ByVal value As ProductionJob)
            cParentJob = value
        End Set
    End Property

    Public Property AssignedJob() As ProductionJob
        Get
            Return cAssignedJob
        End Get
        Set(ByVal value As ProductionJob)
            cAssignedJob = value
        End Set
    End Property

    Public Property AssignedTypeID() As String
        Get
            Return cAssignedTypeID
        End Get
        Set(ByVal value As String)
            cAssignedTypeID = value
        End Set
    End Property

    Private Sub nudME_ButtonCustomClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudME.ButtonCustomClick
        If cAssignedTypeID <> "" Then
            If cAssignedJob IsNot Nothing Then
                cAssignedJob.CurrentBP.MELevel = nudME.Value
                ParentJob.RecalculateResourceRequirements()
                RaiseEvent ResourcesChanged()
                If cAssignedJob.CurrentBP.MELevel = nudME.Value Then
                    nudME.ButtonCustom.Enabled = False
                Else
                    nudME.ButtonCustom.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub nudME_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudME.ValueChanged
        nudME.ButtonCustom.Enabled = False
        If cAssignedTypeID <> "" Then
            If cAssignedJob IsNot Nothing Then
                If cAssignedJob.CurrentBP.MELevel = nudME.Value Then
                    nudME.ButtonCustom.Enabled = False
                Else
                    nudME.ButtonCustom.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub nudME_LockUpdateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudME.LockUpdateChanged
        If cAssignedTypeID <> "" Then
            If nudME.LockUpdateChecked = True Then
                ParentJob.ReplaceResourceWithJob(Me.AssignedTypeID)
            Else
                ParentJob.ReplaceJobWithResource(Me.AssignedTypeID)
                cAssignedJob.CurrentBP.MELevel = nudME.Value
            End If
            RaiseEvent ResourcesChanged()
        End If
    End Sub

End Class
