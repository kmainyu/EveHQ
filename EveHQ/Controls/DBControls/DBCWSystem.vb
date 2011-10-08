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

Public Class DBCWSystem

    Dim WormholeSystems As New SortedList(Of String, WormholeSystem)
    Dim WormholeEffects As New SortedList(Of String, WormholeEffect)
    Dim WHEffects As New SortedList(Of String, String)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Initialise configuration form name
        Me.ControlConfigForm = ""

        ' Try and load the wormhole information
        Call Me.LoadWHSystemData()
        Call Me.LoadWHAttributeData()

    End Sub

    Private Sub DBCWSystem_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ' Load the combo box with wormhole information
        Call Me.PopulateWormholeData()
    End Sub

#Region "Public Overriding Propeties"

    Public Overrides ReadOnly Property ControlName() As String
        Get
            Return "W-Space Information"
        End Get
    End Property

#End Region

    Private Function LoadWHSystemData() As Boolean
        ' Parse the location classes
        Dim WHClasses As New SortedList(Of String, String)
        Dim Classes() As String = My.Resources.WHClasses.Split((ControlChars.CrLf).ToCharArray)
        For Each WHClass As String In Classes
            If WHClass <> "" Then
                Dim ClassData() As String = WHClass.Split(",".ToCharArray)
                If WHClasses.ContainsKey(ClassData(0)) = False Then
                    WHClasses.Add(ClassData(0), ClassData(1))
                End If
            End If
        Next
        ' Parse the location effects
        Dim WHEffects As New SortedList(Of String, String)
        Dim Effects() As String = My.Resources.WSpaceTypes.Split((ControlChars.CrLf).ToCharArray)
        For Each WHEffect As String In Effects
            If WHEffect <> "" Then
                Dim EffectData() As String = WHEffect.Split(",".ToCharArray)
                If WHEffects.ContainsKey(EffectData(0)) = False Then
                    WHEffects.Add(EffectData(0), EffectData(1))
                End If
            End If
        Next
        ' Load the data
        Dim strSQL As String = "SELECT * FROM mapSolarSystems WHERE regionID>11000000;"
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    Dim cSystem As WormholeSystem = New WormholeSystem
                    WormholeSystems.Clear()
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        cSystem = New WormholeSystem
                        cSystem.ID = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemID"))
                        cSystem.Name = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemName"))
                        cSystem.Region = CStr(systemData.Tables(0).Rows(solar).Item("regionID"))
                        cSystem.Constellation = CStr(systemData.Tables(0).Rows(solar).Item("constellationID"))
                        cSystem.WClass = WHClasses(cSystem.Region)
                        If WHEffects.ContainsKey(cSystem.Name) = True Then
                            cSystem.WEffect = WHEffects(cSystem.Name)
                        Else
                            cSystem.WEffect = ""
                        End If
                        WormholeSystems.Add(CStr(cSystem.Name), cSystem)
                    Next
                    WHClasses.Clear()
                    WHEffects.Clear()
                    systemData.Dispose()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading System Data for Prism Plugin" & ControlChars.CrLf & ex.Message, "Prism Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function LoadWHAttributeData() As Boolean
        ' Load the data
        Dim strSQL As String = "SELECT invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.unitID"
        strSQL &= " FROM dgmAttributeTypes INNER JOIN (invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID) ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID"
        strSQL &= " WHERE (((invTypes.groupID)=920));"
        Dim WHData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If WHData IsNot Nothing Then
                If WHData.Tables(0).Rows.Count > 0 Then
                    Dim cWH As New WormHole
                    WormholeEffects.Clear()
                    Dim currentEffect As New WormholeEffect
                    For WH As Integer = 0 To WHData.Tables(0).Rows.Count - 1
                        Dim typeName As String = CStr(WHData.Tables(0).Rows(WH).Item("typeName"))
                        Dim attID As String = CStr(WHData.Tables(0).Rows(WH).Item("attributeID"))
                        Dim attValue As Double = 0
                        If IsDBNull(WHData.Tables(0).Rows(WH).Item("valueInt")) = False Then
                            attValue = CDbl(WHData.Tables(0).Rows(WH).Item("valueInt"))
                        Else
                            attValue = CDbl(WHData.Tables(0).Rows(WH).Item("valueFloat"))
                        End If
                        If CStr(WHData.Tables(0).Rows(WH).Item("unitID")) = "124" Or CStr(WHData.Tables(0).Rows(WH).Item("unitID")) = "105" Then
                            attValue = -attValue
                        End If
                        If WormholeEffects.ContainsKey(typeName) = False Then
                            WormholeEffects.Add(typeName, New WormholeEffect)
                        End If
                        currentEffect = WormholeEffects(typeName)
                        currentEffect.WormholeType = typeName
                        currentEffect.Attributes.Add(attID, attValue)
                    Next
                    WHData.Dispose()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Wormhole Effect Data for the Void Plugin" & ControlChars.CrLf & ex.Message, "Void Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub PopulateWormholeData()
        ' Load up pilot information
        cboWHSystem.BeginUpdate()
        cboWHSystem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHSystem.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHSystem.Items.Clear()
        For Each WH As WormholeSystem In WormholeSystems.Values
            cboWHSystem.Items.Add(WH.Name)
            cboWHSystem.AutoCompleteCustomSource.Add(WH.Name)
        Next
        cboWHSystem.EndUpdate()
        ' Parse the WHEffects resource
        WHEffects = New SortedList(Of String, String)
        Dim Effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
        For Each Effect As String In Effects
            If Effect <> "" Then
                Dim EffectData() As String = Effect.Split(",".ToCharArray)
                If WHEffects.ContainsKey(EffectData(0)) = False Then
                    WHEffects.Add(EffectData(0), EffectData(10))
                End If
            End If
        Next
    End Sub

    Private Sub cboWHSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHSystem.SelectedIndexChanged
        ' Update the WH System Details
        If WormholeSystems.ContainsKey(cboWHSystem.SelectedItem.ToString) = True Then
            Dim WH As WormholeSystem = WormholeSystems(cboWHSystem.SelectedItem.ToString)
            If WH.WEffect <> "" Then
                Dim modName As String = ""
                If WH.WEffect = "Red Giant" Then
                    modName = WH.WEffect & " Beacon Class " & WH.WClass
                Else
                    modName = WH.WEffect & " Effect Beacon Class " & WH.WClass
                End If
                'Dim SSun As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(EveHQ.Core.HQ.itemList(modName))
                lblAnomalyName.Text = WH.WEffect
                ' Establish the effects
                Dim EffectList As New SortedList(Of String, Double)
                Dim SysEffects As WormholeEffect = WormholeEffects(modName)
                For Each att As String In SysEffects.Attributes.Keys
                    If WHEffects.ContainsKey(att) = True Then
                        EffectList.Add(WHEffects(att), SysEffects.Attributes(att))
                    End If
                Next
                lvwEffects.BeginUpdate()
                lvwEffects.Items.Clear()
                For Each Effect As String In EffectList.Keys
                    Dim newEffect As New ListViewItem
                    newEffect.Text = Effect
                    Dim value As Double = CDbl(EffectList(Effect))
                    If value < 5 And value > -5 Then
                        If value < 1 Or Effect.EndsWith("Penalty") Then
                            newEffect.ForeColor = Drawing.Color.Red
                        Else
                            newEffect.ForeColor = Drawing.Color.LimeGreen
                        End If
                        newEffect.SubItems.Add(EffectList(Effect).ToString("N2") & " x")
                    Else
                        If value < 0 Or Effect.EndsWith("Penalty") Then
                            newEffect.ForeColor = Drawing.Color.Red
                        Else
                            newEffect.ForeColor = Drawing.Color.LimeGreen
                        End If
                        newEffect.SubItems.Add(EffectList(Effect).ToString("N2") & " %")
                    End If
                    lvwEffects.Items.Add(newEffect)
                Next
                lvwEffects.EndUpdate()
            Else
                lblAnomalyName.Text = "<None>"
                lvwEffects.Items.Clear()
            End If
            lblSystemClass.Text = WH.WClass
        End If
    End Sub

End Class

Public Class WormholeSystem
    Public ID As String
    Public Name As String
    Public Constellation As String
    Public Region As String
    Public WClass As String
    Public WEffect As String
End Class

Public Class WormholeEffect
    Public WormholeType As String
    Public Attributes As New SortedList(Of String, Double)
End Class
