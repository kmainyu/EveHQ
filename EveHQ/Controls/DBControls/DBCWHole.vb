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
Imports EveHQ.EveData

Public Class DBCWHole

    ReadOnly _wormholes As New SortedList(Of String, WormHole)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Initialise configuration form name
        ControlConfigForm = ""

        ' Try and load the wormhole information
        Call LoadWormholeData()

    End Sub

    Private Sub DBCWHole_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Load the combo box with wormhole information
        Call PopulateWormholeData()
    End Sub

    Private Sub PopulateWormholeData()
        cboWHType.BeginUpdate()
        cboWHType.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHType.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHType.Items.Clear()
        For Each wh As WormHole In _wormholes.Values
            cboWHType.Items.Add(wh.Name)
            cboWHType.AutoCompleteCustomSource.Add(wh.Name)
        Next
        cboWHType.EndUpdate()
    End Sub

#Region "Public Overriding Propeties"

    Public Overrides ReadOnly Property ControlName() As String
        Get
            Return "Wormhole Information"
        End Get
    End Property

#End Region

#Region "Wormhole Loading Routines"
    Private Sub LoadWormholeData()
        ' Parse the WHAttributes resource
        Dim whAttributes As New SortedList(Of String, SortedList(Of String, String))
        Dim cAtts As SortedList(Of String, String)
        Dim atts() As String = My.Resources.WHattribs.Split((ControlChars.CrLf).ToCharArray)
        For Each att As String In atts
            If att <> "" Then
                Dim attData() As String = att.Split(",".ToCharArray)
                If whAttributes.ContainsKey(attData(0)) = False Then
                    whAttributes.Add(attData(0), New SortedList(Of String, String))
                End If
                cAtts = whAttributes(attData(0))
                cAtts.Add(attData(1), attData(2))
            End If
        Next
        ' Load the data
        Dim cWH As WormHole
        _wormholes.Clear()
        For Each wh As EveType In StaticData.GetItemsInGroup(988)
            cWH = New WormHole
            cWH.ID = wh.Id.ToString
            cWH.Name = wh.Name.Replace("Wormhole ", "")
            If whAttributes.ContainsKey(cWH.ID) = True Then
                For Each att As String In whAttributes(cWH.ID).Keys
                    Select Case att
                        Case "1381"
                            cWH.TargetClass = whAttributes(cWH.ID).Item(att)
                        Case "1382"
                            cWH.MaxStabilityWindow = whAttributes(cWH.ID).Item(att)
                        Case "1383"
                            cWH.MaxMassCapacity = whAttributes(cWH.ID).Item(att)
                        Case "1384"
                            cWH.MassRegeneration = whAttributes(cWH.ID).Item(att)
                        Case "1385"
                            cWH.MaxJumpableMass = whAttributes(cWH.ID).Item(att)
                        Case "1457"
                            cWH.TargetDistributionID = whAttributes(cWH.ID).Item(att)
                    End Select
                Next
            Else
                cWH.TargetClass = ""
                cWH.MaxStabilityWindow = ""
                cWH.MaxMassCapacity = ""
                cWH.MassRegeneration = ""
                cWH.MaxJumpableMass = ""
                cWH.TargetDistributionID = ""
            End If
            ' Add in data from the resource file
            If cWH.Name.StartsWith("Test", StringComparison.Ordinal) = False Then
                _wormholes.Add(CStr(cWH.Name), cWH)
            End If
        Next
        whAttributes.Clear()
    End Sub
#End Region

    Private Sub cboWHType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboWHType.SelectedIndexChanged
        ' Update the WH Details
        If _wormholes.ContainsKey(cboWHType.SelectedItem.ToString) = True Then
            Dim wh As WormHole = _wormholes(cboWHType.SelectedItem.ToString)
            If wh.Name <> "K162" Then
                lblTargetSystemClass.Text = wh.TargetClass
                Select Case CInt(wh.TargetClass)
                    Case 1 To 6
                        lblTargetSystemClass.Text &= " (Wormhole Class " & wh.TargetClass & ")"
                    Case 7
                        lblTargetSystemClass.Text &= " (High Security Space)"
                    Case 8
                        lblTargetSystemClass.Text &= " (Low Security Space)"
                    Case 9
                        lblTargetSystemClass.Text &= " (Null Security Space)"
                End Select
                lblMaxJumpableMass.Text = CLng(wh.MaxJumpableMass).ToString("N0") & " kg"
                lblMaxTotalMass.Text = CLng(wh.MaxMassCapacity).ToString("N0") & " kg"
                lblStabilityWindow.Text = (CDbl(wh.MaxStabilityWindow) / 60).ToString("N0") & " hours"
            Else
                lblTargetSystemClass.Text = "n/a (Return wormhole)"
                lblMaxJumpableMass.Text = "n/a"
                lblMaxTotalMass.Text = "n/a"
                lblStabilityWindow.Text = "n/a"
            End If
        End If
    End Sub


End Class

Public Class WormHole
    Public ID As String
    Public Name As String
    Public TargetClass As String
    Public MaxStabilityWindow As String
    Public MaxMassCapacity As String
    Public MassRegeneration As String
    Public MaxJumpableMass As String
    Public TargetDistributionID As String
End Class
