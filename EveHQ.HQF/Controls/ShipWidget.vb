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
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.HQF.Forms

Namespace Controls

    Public Class ShipWidget

#Region "Class Variables"

        Dim _widgetFit As Fitting

#End Region

        ''' <summary>
        ''' Initialises a new ship widget
        ''' Any UI updates can be done in the inheriting class (e.g. populating combo boxes)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(shipFit As Fitting)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            _widgetFit = ShipFit.Clone
            Call UpdateFitting()

        End Sub

        Private Sub UpdateFitting()
            Dim baseID As Integer
            If CustomHQFClasses.CustomShipIDs.ContainsKey(_widgetFit.BaseShip.ID) Then
                baseID = ShipLists.ShipListKeyName(CustomHQFClasses.CustomShips(_widgetFit.BaseShip.Name).BaseShipName)
            Else
                baseID = _widgetFit.BaseShip.ID
            End If
            pbFitting.Image = Core.ImageHandler.GetImage(CInt(baseID), 32)

            ' Set up ShipWidgetsModules
            'Me.pnlModules.Controls.Clear()
            Dim swm As New ShipWidgetModules(Me, _widgetFit)
            swm.Name = "SWM"
            swm.Dock = DockStyle.Fill
            pnlModules.Controls.Add(swm)

        End Sub

        Private Sub pbRemove_Click(sender As Object, e As EventArgs) Handles pbRemove.Click
            MessageBox.Show("Are you sure you want to remove this ship?", "Confirm Ship Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        End Sub

        Private Sub pnlShipWidget_DragDrop(sender As Object, e As DragEventArgs) Handles pnlShipWidget.DragDrop
            Dim oLvi As Node = CType(e.Data.GetData(GetType(Node)), Node)
            If oLvi IsNot Nothing Then
                'Dim oModID As String = CStr(ModuleLists.ModuleListName.Item(oLvi.Text))
                MessageBox.Show("Do you want to add the " & oLvi.Text & "?", "Confirm Add Remote Module", MessageBoxButtons.OK, MessageBoxIcon.Question)
            End If
        End Sub

        Private Sub pnlShipWidget_DragOver(sender As Object, e As DragEventArgs) Handles pnlShipWidget.DragOver
            Dim oLvi As Node = CType(e.Data.GetData(GetType(Node)), Node)
            If oLvi IsNot Nothing Then
                'Dim oSep As Integer = oLvi.Name.LastIndexOf("_", StringComparison.Ordinal)
                'Dim oSlotType As Integer = CInt(oLvi.Name.Substring(0, oSep))
                'Dim oslotNo As Integer = CInt(oLvi.Name.Substring(oSep + 1, 1))
                e.Effect = DragDropEffects.Move
            Else
                e.Effect = DragDropEffects.None
            End If
        End Sub

        Private Sub pnlModules_ExpandedChanged(sender As Object, e As ExpandedChangeEventArgs) Handles pnlModules.ExpandedChanged
            ResumeLayout()
            pnlModules.ResumeLayout()
        End Sub

        Private Sub ExpandablePanel1_ExpandedChanging(sender As Object, e As ExpandedChangeEventArgs) Handles pnlModules.ExpandedChanging
            SuspendLayout()
            pnlModules.SuspendLayout()
            If e.NewExpandedValue = True Then
                Height += 150
                'Width += 65
            Else
                Height -= 150
                'Width -= 65
            End If
        End Sub

        Private Sub ctxFitting_Opening(sender As Object, e As CancelEventArgs) Handles ctxFitting.Opening
            e.Cancel = True
            Dim nfp As New frmFittingPopup
            ' Set Location, checking for edges of the dashboard
            Dim fx As Integer = Location.X + Width
            If fx + nfp.Width > Parent.Width Then
                fx = Location.X - nfp.Width
            End If
            Dim fy As Integer = Location.Y
            If fy + nfp.Height > Parent.Height Then
                fy = Parent.Height - nfp.Height + 18
            End If
            nfp.Location = New Point(fx, fy)
            nfp.ShowDialog()
            If nfp.FittingName IsNot Nothing Then
                ' Update the current fit and recalculate
                If Fittings.FittingList.ContainsKey(nfp.FittingName) Then
                    _widgetFit = Fittings.FittingList(nfp.FittingName).Clone
                    Call UpdateFitting()
                End If
            End If
        End Sub

        Private Sub ctxPilot_Opening(sender As Object, e As CancelEventArgs) Handles ctxPilot.Opening
            e.Cancel = True
            MessageBox.Show("But this is where we would show a pilot drop down ;)")
        End Sub
    End Class
End NameSpace