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
Public Class TrainingQueue
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private _OldParent As Control

    Private Sub TrainingQueue_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ParentChanged
        If _OldParent IsNot Nothing Then
            RemoveHandler _OldParent.Disposed, AddressOf Parent_Disposed
        End If

        If Me.Parent IsNot Nothing Then
            AddHandler Me.Parent.Disposed, AddressOf Parent_Disposed
        End If

        _OldParent = Me.Parent
    End Sub

    Private Sub Parent_Disposed(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub
End Class
