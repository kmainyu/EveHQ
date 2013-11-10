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

Namespace Controls.DBControls

    Public Class DBCWSystem

        ReadOnly _wormholeSystems As New SortedList(Of String, WormholeSystem)
        ReadOnly _wormholeEffects As New SortedList(Of String, WormholeEffect)
        Dim _whEffects As New SortedList(Of String, String)

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            ' Initialise configuration form name
            ControlConfigForm = ""

            ' Try and load the wormhole information
            Call LoadWormholeSystemData()
            Call LoadWormholeAttributeData()

        End Sub

        Private Sub DBCWSystem_Load(sender As Object, e As EventArgs) Handles Me.Load
            ' Load the combo box with wormhole information
            Call PopulateWormholeData()
        End Sub

#Region "Public Overriding Propeties"

        Public Overrides ReadOnly Property ControlName() As String
            Get
                Return "W-Space Information"
            End Get
        End Property

#End Region

        Private Sub LoadWormholeSystemData()
            ' Parse the location classes
            Dim whClasses As New SortedList(Of String, String)
            Dim classes() As String = My.Resources.WHClasses.Split((ControlChars.CrLf).ToCharArray)
            For Each whClass As String In classes
                If whClass <> "" Then
                    Dim classData() As String = whClass.Split(",".ToCharArray)
                    If whClasses.ContainsKey(classData(0)) = False Then
                        whClasses.Add(classData(0), classData(1))
                    End If
                End If
            Next
            ' Parse the location effects
            _whEffects.Clear()
            Dim effects() As String = My.Resources.WSpaceTypes.Split((ControlChars.CrLf).ToCharArray)
            For Each whEffect As String In effects
                If whEffect <> "" Then
                    Dim effectData() As String = whEffect.Split(",".ToCharArray)
                    If _whEffects.ContainsKey(effectData(0)) = False Then
                        _whEffects.Add(effectData(0), effectData(1))
                    End If
                End If
            Next
            ' Load the data

            Dim systems As IEnumerable(Of SolarSystem) = From item In StaticData.SolarSystems.Values Where item.RegionID > 11000000

            Dim cSystem As WormholeSystem
            _wormholeSystems.Clear()

            For Each solar As SolarSystem In systems
                cSystem = New WormholeSystem
                cSystem.ID = solar.ID.ToString
                cSystem.Name = solar.Name
                cSystem.Region = solar.RegionID.ToString
                cSystem.Constellation = solar.ConstellationID.ToString
                cSystem.WClass = whClasses(cSystem.Region)
                If _whEffects.ContainsKey(cSystem.Name) = True Then
                    cSystem.WEffect = _whEffects(cSystem.Name)
                Else
                    cSystem.WEffect = ""
                End If
                _wormholeSystems.Add(CStr(cSystem.Name), cSystem)
            Next
            whClasses.Clear()
            _whEffects.Clear()

        End Sub

        Private Sub LoadWormholeAttributeData()
            ' Load the data
            Dim taqs As IEnumerable = (From item In StaticData.Types.Values Join ta In StaticData.TypeAttributes On item.Id Equals ta.TypeId Join at In StaticData.AttributeTypes.Values On ta.AttributeId Equals at.AttributeId
                    Where item.Group = 920
                    Select New TypeAttributeQuery With {
                    .TypeID = item.Id,
                    .TypeName = item.Name,
                    .AttributeID = ta.AttributeId,
                    .UnitID = at.UnitId,
                    .Value = ta.Value}).ToList

            _wormholeEffects.Clear()
            Dim currentEffect As WormholeEffect
            For Each taq As TypeAttributeQuery In taqs
                Dim typeName As String = taq.TypeName
                Dim attID As String = CStr(taq.AttributeID)
                Dim attValue As Double = taq.Value
                If taq.UnitID = 124 Or taq.UnitID = 105 Then
                    attValue = -attValue
                End If
                If _wormholeEffects.ContainsKey(typeName) = False Then
                    _wormholeEffects.Add(typeName, New WormholeEffect)
                End If
                currentEffect = _wormholeEffects(typeName)
                currentEffect.WormholeType = typeName
                currentEffect.Attributes.Add(attID, attValue)
            Next
        End Sub

        Private Sub PopulateWormholeData()
            ' Load up pilot information
            cboWHSystem.BeginUpdate()
            cboWHSystem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cboWHSystem.AutoCompleteSource = AutoCompleteSource.CustomSource
            cboWHSystem.Items.Clear()
            For Each wh As WormholeSystem In _wormholeSystems.Values
                cboWHSystem.Items.Add(wh.Name)
                cboWHSystem.AutoCompleteCustomSource.Add(wh.Name)
            Next
            cboWHSystem.EndUpdate()
            ' Parse the WHEffects resource
            _whEffects = New SortedList(Of String, String)
            Dim effects() As String = My.Resources.WHEffects.Split((ControlChars.CrLf).ToCharArray)
            For Each effect As String In effects
                If effect <> "" Then
                    Dim effectData() As String = effect.Split(",".ToCharArray)
                    If _whEffects.ContainsKey(effectData(0)) = False Then
                        _whEffects.Add(effectData(0), effectData(10))
                    End If
                End If
            Next
        End Sub

        Private Sub cboWHSystem_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboWHSystem.SelectedIndexChanged
            ' Update the WH System Details
            If _wormholeSystems.ContainsKey(cboWHSystem.SelectedItem.ToString) = True Then
                Dim wh As WormholeSystem = _wormholeSystems(cboWHSystem.SelectedItem.ToString)
                If wh.WEffect <> "" Then
                    Dim modName As String
                    If wh.WEffect = "Red Giant" Then
                        modName = wh.WEffect & " Beacon Class " & wh.WClass
                    Else
                        modName = wh.WEffect & " Effect Beacon Class " & wh.WClass
                    End If
                    'Dim SSun As EveHQ.Core.EveItem = EveHQ.StaticData.Types(EveHQ.StaticData.TypeNames(modName))
                    lblAnomalyName.Text = wh.WEffect
                    ' Establish the effects
                    Dim effectList As New SortedList(Of String, Double)
                    Dim sysEffects As WormholeEffect = _wormholeEffects(modName)
                    For Each att As String In sysEffects.Attributes.Keys
                        If _whEffects.ContainsKey(att) = True Then
                            effectList.Add(_whEffects(att), sysEffects.Attributes(att))
                        End If
                    Next
                    lvwEffects.BeginUpdate()
                    lvwEffects.Items.Clear()
                    For Each effect As String In effectList.Keys
                        Dim newEffect As New ListViewItem
                        newEffect.Text = effect
                        Dim value As Double = CDbl(effectList(effect))
                        If value < 5 And value > -5 Then
                            If value < 1 Or effect.EndsWith("Penalty", StringComparison.Ordinal) Then
                                newEffect.ForeColor = Color.Red
                            Else
                                newEffect.ForeColor = Color.LimeGreen
                            End If
                            newEffect.SubItems.Add(effectList(effect).ToString("N2") & " x")
                        Else
                            If value < 0 Or effect.EndsWith("Penalty", StringComparison.Ordinal) Then
                                newEffect.ForeColor = Color.Red
                            Else
                                newEffect.ForeColor = Color.LimeGreen
                            End If
                            newEffect.SubItems.Add(effectList(effect).ToString("N2") & " %")
                        End If
                        lvwEffects.Items.Add(newEffect)
                    Next
                    lvwEffects.EndUpdate()
                Else
                    lblAnomalyName.Text = "<None>"
                    lvwEffects.Items.Clear()
                End If
                lblSystemClass.Text = wh.WClass
            End If
        End Sub

        Private NotInheritable Class TypeAttributeQuery
            Public Property TypeID As Long
            Public Property TypeName As String
            Public Property AttributeID As Integer
            Public Property UnitID As Integer
            Public Property Value As Double
        End Class

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
End NameSpace