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
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports System.Drawing

Public Class ShipWidget

#Region "Class Variables"

    Dim WidgetFit As Fitting

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

    Public Sub New(ShipFit As Fitting)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        WidgetFit = ShipFit.Clone
        Call Me.UpdateFitting()

    End Sub

    Private Sub UpdateFitting()
        Dim baseID As String = ""
        If CustomHQFClasses.CustomShipIDs.ContainsKey(WidgetFit.BaseShip.ID) Then
            baseID = ShipLists.shipListKeyName(CustomHQFClasses.CustomShips(WidgetFit.BaseShip.Name).BaseShipName)
        Else
            baseID = WidgetFit.BaseShip.ID
        End If
        pbFitting.Image = EveHQ.Core.ImageHandler.GetImage(baseID, 32)

        ' Set up ShipWidgetsModules
        'Me.pnlModules.Controls.Clear()
        Dim SWM As New ShipWidgetModules(Me, WidgetFit)
        SWM.Name = "SWM"
        SWM.Dock = DockStyle.Fill
        Me.pnlModules.Controls.Add(SWM)

    End Sub

    Private Sub pbRemove_Click(sender As System.Object, e As System.EventArgs) Handles pbRemove.Click
        MessageBox.Show("Are you sure you want to remove this ship?", "Confirm Ship Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
    End Sub

    Private Sub pnlShipWidget_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles pnlShipWidget.DragDrop
        Dim oLVI As Node = CType(e.Data.GetData(GetType(Node)), Node)
        If oLVI IsNot Nothing Then
            Dim oModID As String = CStr(ModuleLists.moduleListName.Item(oLVI.Text))
            MessageBox.Show("Do you want to add the " & oLVI.Text & "?", "Confirm Add Remote Module", MessageBoxButtons.OK, MessageBoxIcon.Question)
        End If
    End Sub

    Private Sub pnlShipWidget_DragOver(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles pnlShipWidget.DragOver
        Dim oLVI As Node = CType(e.Data.GetData(GetType(Node)), Node)
        If oLVI IsNot Nothing Then
            Dim oSep As Integer = oLVI.Name.LastIndexOf("_")
            Dim oSlotType As Integer = CInt(oLVI.Name.Substring(0, oSep))
            Dim oslotNo As Integer = CInt(oLVI.Name.Substring(oSep + 1, 1))
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub pnlModules_ExpandedChanged(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles pnlModules.ExpandedChanged
        Me.ResumeLayout()
        Me.pnlModules.ResumeLayout()
    End Sub

    Private Sub ExpandablePanel1_ExpandedChanging(sender As Object, e As DevComponents.DotNetBar.ExpandedChangeEventArgs) Handles pnlModules.ExpandedChanging
        Me.SuspendLayout()
        Me.pnlModules.SuspendLayout()
        If e.NewExpandedValue = True Then
            Me.Height += 150
            'Me.Width += 65
        Else
            Me.Height -= 150
            'Me.Width -= 65
        End If
    End Sub

    Private Sub ctxFitting_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ctxFitting.Opening
        e.Cancel = True
        Dim NFP As New frmFittingPopup
        ' Set Location, checking for edges of the dashboard
        Dim FX As Integer = Me.Location.X + Me.Width
        If FX + NFP.Width > Me.Parent.Width Then
            FX = Me.Location.X - NFP.Width
        End If
        Dim FY As Integer = Me.Location.Y
        If FY + NFP.Height > Me.Parent.Height Then
            FY = Me.Parent.Height - NFP.Height + 18
        End If
        NFP.Location = New Point(FX, FY)
        NFP.ShowDialog()
        If NFP.FittingName IsNot Nothing Then
            ' Update the current fit and recalculate
            If Fittings.FittingList.ContainsKey(NFP.FittingName) Then
                WidgetFit = Fittings.FittingList(NFP.FittingName).Clone
                Call Me.UpdateFitting()
            End If
        End If
    End Sub

    Private Sub ctxPilot_Opening(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles ctxPilot.Opening
        e.Cancel = True
        MessageBox.Show("But this is where we would show a pilot drop down ;)")
    End Sub
End Class
