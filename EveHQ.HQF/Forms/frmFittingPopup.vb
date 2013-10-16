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
Imports System.Windows.Forms
Imports DevComponents.AdvTree

Namespace Forms

    Public Class FrmFittingPopup

        Public Property FittingName As String

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Call UpdateFittingsTree(True)

        End Sub

        Private Sub UpdateFittingsTree(ByVal collapseAllNodes As Boolean)
            adtFittings.BeginUpdate()
            ' Get current list of "open" nodes if we need the feature
            Dim openNodes As New ArrayList
            If CollapseAllNodes = False Then
                For Each shipNode As Node In adtFittings.Nodes
                    If shipNode.Expanded = True Then
                        openNodes.Add(shipNode.Text)
                    End If
                Next
            End If
            ' Redraw the tree
            adtFittings.Nodes.Clear()
            Dim shipName As String
            Dim fitName As String
            Const isFlyable As Boolean = True

            If Fittings.FittingList.Count > 0 Then
                For Each fitting As String In Fittings.FittingList.Keys
                    shipName = Fittings.FittingList(fitting).ShipName
                    fitName = Fittings.FittingList(fitting).FittingName

                    ' Create the ship node if it's not already present
                    Dim containsShip As New Node
                    For Each ship As Node In adtFittings.Nodes
                        If ship.Text = shipName Then
                            containsShip = ship
                        End If
                    Next
                    If containsShip.Text = "" Then
                        containsShip.Text = shipName
                        adtFittings.Nodes.Add(containsShip)
                    End If

                    ' Add the details to the Node, checking for duplicates
                    If isFlyable = True Then
                        Dim containsFitting As New Node
                        For Each fit As Node In containsShip.Nodes
                            If fit.Text = fitName Then
                                MessageBox.Show("Duplicate fitting found for " & shipName & ", and omitted", "Duplicate Fitting Found!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                containsFitting = Nothing
                                Exit For
                            End If
                        Next
                        If containsFitting IsNot Nothing Then
                            containsFitting.Text = fitName
                            containsShip.Nodes.Add(containsFitting)
                        End If
                    End If
                Next
                ' Remove any parent nodes with no children
                Dim fNodeID As Integer = 0
                Do
                    If adtFittings.Nodes(fNodeID).Nodes.Count = 0 Then
                        adtFittings.Nodes.Remove(adtFittings.Nodes(fNodeID))
                        fNodeID -= 1
                    End If
                    fNodeID += 1
                Loop Until fNodeID = adtFittings.Nodes.Count
                ' Open the previously opened nodes
                If CollapseAllNodes = False Then
                    For Each shipNode As Node In adtFittings.Nodes
                        If openNodes.Contains(shipNode.Text) Then
                            shipNode.Expand()
                        End If
                    Next
                End If
            End If
            adtFittings.EndUpdate()
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
            FittingName = Nothing
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
            Dim eNode As Node = adtFittings.SelectedNodes(0)
            If eNode.Parent IsNot Nothing Then
                'Dim ShipName As String = eNode.Parent.Text
                'Dim FitName As String = eNode.Text
                Dim fitKey As String = eNode.Parent.Text & ", " & eNode.Text
                If Fittings.FittingList.ContainsKey(fitKey) = True Then
                    FittingName = fitKey
                    DialogResult = DialogResult.OK
                    Close()
                End If
            End If
        End Sub

        Private Sub adtFittings_NodeDoubleClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtFittings.NodeDoubleClick
            If e.Node.Parent IsNot Nothing Then
                'Dim ShipName As String = e.Node.Parent.Text
                'Dim FitName As String = e.Node.Text
                Dim fitKey As String = e.Node.Parent.Text & ", " & e.Node.Text
                If Fittings.FittingList.ContainsKey(fitKey) = True Then
                    FittingName = fitKey
                    DialogResult = DialogResult.OK
                    Close()
                End If
            End If
        End Sub

        Private Sub adtFittings_SelectionChanged(sender As Object, e As EventArgs) Handles adtFittings.SelectionChanged
            If adtFittings.SelectedNodes.Count > 0 Then
                btnAccept.Enabled = True
            Else
                btnAccept.Enabled = False
            End If
        End Sub
    End Class
End NameSpace