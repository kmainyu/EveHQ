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

Imports EveHQ.EveData

Namespace Controls.DBControls

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
            Dim cWh As WormHole
            _wormholes.Clear()
            For Each wh As EveType In StaticData.GetItemsInGroup(988)
                cWh = New WormHole
                cWh.ID = wh.Id.ToString
                cWh.Name = wh.Name.Replace("Wormhole ", "")
                If whAttributes.ContainsKey(cWh.ID) = True Then
                    For Each att As String In whAttributes(cWh.ID).Keys
                        Select Case att
                            Case "1381"
                                cWh.TargetClass = whAttributes(cWh.ID).Item(att)
                            Case "1382"
                                cWh.MaxStabilityWindow = whAttributes(cWh.ID).Item(att)
                            Case "1383"
                                cWh.MaxMassCapacity = whAttributes(cWh.ID).Item(att)
                            Case "1384"
                                cWh.MassRegeneration = whAttributes(cWh.ID).Item(att)
                            Case "1385"
                                cWh.MaxJumpableMass = whAttributes(cWh.ID).Item(att)
                            Case "1457"
                                cWh.TargetDistributionID = whAttributes(cWh.ID).Item(att)
                        End Select
                    Next
                Else
                    cWh.TargetClass = ""
                    cWh.MaxStabilityWindow = ""
                    cWh.MaxMassCapacity = ""
                    cWh.MassRegeneration = ""
                    cWh.MaxJumpableMass = ""
                    cWh.TargetDistributionID = ""
                End If
                ' Add in data from the resource file
                If cWh.Name.StartsWith("Test", StringComparison.Ordinal) = False Then
                    _wormholes.Add(CStr(cWh.Name), cWh)
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
End NameSpace