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
Imports DevComponents.DotNetBar.Controls

Namespace Controls
    Public Class PrismSelectionHostControl

        Public Event SelectionChanged()

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            cboHost.DropDownControl = New PrismSelectionControl(_listType, _allowMultipleSelections, cboHost)
            AddHandler CType(cboHost.DropDownControl, PrismSelectionControl).SelectionChanged, AddressOf HostSelectionChanged

        End Sub

        Dim _listType As PrismSelectionType
        Public Property ListType As PrismSelectionType
            Get
                Return _listType
            End Get
            Set(ByVal value As PrismSelectionType)
                _listType = value
                If cboHost IsNot Nothing Then
                    CType(cboHost.DropDownControl, PrismSelectionControl).ListType = value
                End If
            End Set
        End Property

        Dim _allowMultipleSelections As Boolean
        Public Property AllowMultipleSelections As Boolean
            Get
                Return _allowMultipleSelections
            End Get
            Set(ByVal value As Boolean)
                _allowMultipleSelections = value
                If cboHost IsNot Nothing Then
                    CType(cboHost.DropDownControl, PrismSelectionControl).AllowMultipleSelections = value
                End If
            End Set
        End Property

        Public ReadOnly Property ItemList As ListViewEx
            Get
                Return CType(cboHost.DropDownControl, PrismSelectionControl).lvwItems
            End Get
        End Property

        Private Sub HostSelectionChanged()
            RaiseEvent SelectionChanged()
        End Sub
    End Class
End NameSpace