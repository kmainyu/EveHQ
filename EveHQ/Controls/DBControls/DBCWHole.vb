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
Imports System.Data

Public Class DBCWHole
    Dim Wormholes As New SortedList(Of String, WormHole)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Initialise configuration form name
        Me.ControlConfigForm = ""

        ' Try and load the wormhole information
        Call Me.LoadWormholeData()

        ' Load the combo box with wormhole information
        Call Me.PopulateWormholeData()

    End Sub

    Private Sub PopulateWormholeData()
        cboWHType.BeginUpdate()
        cboWHType.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHType.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHType.Items.Clear()
        For Each WH As WormHole In Wormholes.Values
            cboWHType.Items.Add(WH.Name)
            cboWHType.AutoCompleteCustomSource.Add(WH.Name)
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
    Private Function LoadWormholeData() As Boolean
        ' Parse the WHAttributes resource
        Dim WHAttributes As New SortedList(Of String, SortedList(Of String, String))
        Dim cAtts As New SortedList(Of String, String)
        Dim Atts() As String = My.Resources.WHattribs.Split((ControlChars.CrLf).ToCharArray)
        For Each Att As String In Atts
            If Att <> "" Then
                Dim AttData() As String = Att.Split(",".ToCharArray)
                If WHAttributes.ContainsKey(AttData(0)) = False Then
                    WHAttributes.Add(AttData(0), New SortedList(Of String, String))
                End If
                cAtts = WHAttributes(AttData(0))
                cAtts.Add(AttData(1), AttData(2))
            End If
        Next
        ' Load the data
        Dim strSQL As String = "SELECT * from invTypes WHERE groupID=988;"
        Dim WHData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If WHData IsNot Nothing Then
                If WHData.Tables(0).Rows.Count > 0 Then
                    Dim cWH As New WormHole
                    Wormholes.Clear()
                    For WH As Integer = 0 To WHData.Tables(0).Rows.Count - 1
                        cWH = New WormHole
                        cWH.ID = CStr(WHData.Tables(0).Rows(WH).Item("typeID"))
                        cWH.Name = CStr(WHData.Tables(0).Rows(WH).Item("typeName")).Replace("Wormhole ", "")
                        If WHAttributes.ContainsKey(cWH.ID) = True Then
                            For Each Att As String In WHAttributes(cWH.ID).Keys
                                Select Case Att
                                    Case "1381"
                                        cWH.TargetClass = WHAttributes(cWH.ID).Item(Att)
                                    Case "1382"
                                        cWH.MaxStabilityWindow = WHAttributes(cWH.ID).Item(Att)
                                    Case "1383"
                                        cWH.MaxMassCapacity = WHAttributes(cWH.ID).Item(Att)
                                    Case "1384"
                                        cWH.MassRegeneration = WHAttributes(cWH.ID).Item(Att)
                                    Case "1385"
                                        cWH.MaxJumpableMass = WHAttributes(cWH.ID).Item(Att)
                                    Case "1457"
                                        cWH.TargetDistributionID = WHAttributes(cWH.ID).Item(Att)
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
                        If cWH.Name.StartsWith("Test") = False Then
                            Wormholes.Add(CStr(cWH.Name), cWH)
                        End If
                    Next
                    WHAttributes.Clear()
                    WHData.Dispose()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Wormhole Data for the Wormhole Information Widget" & ControlChars.CrLf & ex.Message, "Void Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
#End Region

    Private Sub cboWHType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHType.SelectedIndexChanged
        ' Update the WH Details
        If Wormholes.ContainsKey(cboWHType.SelectedItem.ToString) = True Then
            Dim WH As WormHole = Wormholes(cboWHType.SelectedItem.ToString)
            If WH.Name <> "K162" Then
                lblTargetSystemClass.Text = WH.TargetClass
                Select Case CInt(WH.TargetClass)
                    Case 1 To 6
                        lblTargetSystemClass.Text &= " (Wormhole Class " & WH.TargetClass & ")"
                    Case 7
                        lblTargetSystemClass.Text &= " (High Security Space)"
                    Case 8
                        lblTargetSystemClass.Text &= " (Low Security Space)"
                    Case 9
                        lblTargetSystemClass.Text &= " (Null Security Space)"
                End Select
                lblMaxJumpableMass.Text = CLng(WH.MaxJumpableMass).ToString("N0") & " kg"
                lblMaxTotalMass.Text = CLng(WH.MaxMassCapacity).ToString("N0") & " kg"
                lblStabilityWindow.Text = (CDbl(WH.MaxStabilityWindow) / 60).ToString("N0") & " hours"
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
