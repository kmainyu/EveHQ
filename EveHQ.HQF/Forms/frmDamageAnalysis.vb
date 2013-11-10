' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting
Imports EveHQ.HQF.Controls
Imports EveHQ.Core
Imports System.Windows.Forms
Imports System.Text

Namespace Forms

    Public Class FrmDamageAnalysis

        Dim _attackingShip, _targetShip As Ship
        Dim _tMod As New ShipModule
        Dim _mMod As New ShipModule
        Dim _sr As Double = 0
        Dim _transVel As Double = 0
        Dim _targetVel As Double = 0
        Dim _d As Double = 0
        Dim _tsr As Double = 0 ' Turret Signature Radius
        Dim _tt As Double = 0 ' Turret Tracking
        Dim _tor As Double = 0 ' Turret Optimal Range
        Dim _tf As Double = 0 ' Turret Falloff
        Dim _tvd As Double = 0 ' Turret Volley Damage
        Dim _tdps As Double = 0 ' Turret DPS
        Dim _tc As Integer = 0 ' Turret Count 
        Dim _trof As Double = 0 ' Turret ROF
        Dim _mc As Integer = 0 ' Missile Count
        Dim _mor As Double = 0 ' Missile Optimal Range
        Dim _mrof As Double = 0 ' Missile ROF
        Dim _missileEr As Double = 0
        Dim _missileEv As Double = 0
        Dim _missileDrf As Double = 0
        Dim _missileDrs As Double = 0
        Dim _mvd As Double = 0 ' Missile Volley Damage
        Dim _mdps As Double = 0 ' Missile DPS
        Dim _sHp, _aHp, _hHp As Double
        Dim _sEM, _sEx, _sKi, _sTh As Double
        Dim _aEM, _aEx, _aKi, _aTh As Double
        Dim _hEM, _hEx, _hKi, _hTh As Double
        Dim _wEM, _wEx, _wKi, _wTh, _wT As Double
        Dim _sdEM, _sdEx, _sdKi, _sdTh, _sdT As Double
        Dim _adEM, _adEx, _adKi, _adTh, _adT As Double
        Dim _hdEM, _hdEx, _hdKi, _hdTh, _hdT As Double
        Dim _esdT As Double
        Dim _eadT As Double
        Dim _ehdT As Double
        Dim _tsrr, _tarr, _thrr As Double
        Dim _estNr, _eatNr, _ehtNr As Double
        Dim _estR, _eatR, _ehtR As Double
        Dim _cth As Double = 0
        Dim _edr As Double = 0
        Dim _graphForm As New FrmChartViewer

#Region "Public Properties"
        Dim _cFittingName As String = ""
        Dim _cPilotName As String = ""
        Public Property FittingName() As String
            Get
                Return _cFittingName
            End Get
            Set(ByVal value As String)
                _cFittingName = value
            End Set
        End Property
        Public Property PilotName() As String
            Get
                Return _cPilotName
            End Get
            Set(ByVal value As String)
                _cPilotName = value
            End Set
        End Property
#End Region

#Region "Form Loading Routines"

        Private Sub frmDamageAnalysis_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Call LoadFittingsAndPilots()
            EveSpace1.RangeScale = 0.5
            EveSpace1.VelocityScale = 2
            EveSpace1.UseIntegratedVelocity = True
        End Sub

        Private Sub LoadFittingsAndPilots()
            ' Load details into the combo boxes
            cboAttackerFitting.BeginUpdate() : cboTargetFitting.BeginUpdate()
            cboAttackerPilot.BeginUpdate() : cboTargetPilot.BeginUpdate()
            cboAttackerFitting.Items.Clear() : cboTargetFitting.Items.Clear()
            cboAttackerPilot.Items.Clear() : cboTargetPilot.Items.Clear()
            ' Add the fittings
            For Each fitting As String In Fittings.FittingList.Keys
                cboAttackerFitting.Items.Add(fitting)
                cboTargetFitting.Items.Add(fitting)
            Next
            ' Select a fitting if appropriate
            If _cFittingName <> "" Then
                cboAttackerFitting.SelectedItem = _cFittingName
            End If
            ' Add the pilots
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
                cboAttackerPilot.Items.Add(cPilot.Name)
                cboTargetPilot.Items.Add(cPilot.Name)
            Next
            ' Select a pilot
            If _cPilotName <> "" Then
                cboAttackerPilot.SelectedItem = _cPilotName
            End If
            cboAttackerFitting.EndUpdate() : cboTargetFitting.EndUpdate()
            cboAttackerPilot.EndUpdate() : cboTargetPilot.EndUpdate()
        End Sub

#End Region

#Region "Fitting and Pilot Changed Routines"

        Private Sub cboAttackerFitting_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboAttackerFitting.SelectedIndexChanged
            Call SetAttackingShip()
        End Sub

        Private Sub cboAttackerPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboAttackerPilot.SelectedIndexChanged
            Call SetAttackingShip()
        End Sub

        Private Sub cboTargetFitting_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTargetFitting.SelectedIndexChanged
            Call SetTargetShip()
        End Sub

        Private Sub cboTargetPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTargetPilot.SelectedIndexChanged
            Call SetTargetShip()
        End Sub

        Private Sub SetAttackingShip()
            If cboAttackerFitting.SelectedItem IsNot Nothing And cboAttackerPilot.SelectedItem IsNot Nothing Then
                Dim shipFit As String = cboAttackerFitting.SelectedItem.ToString
                Dim aPilot As FittingPilot = FittingPilots.HQFPilots(cboAttackerPilot.SelectedItem.ToString)
                Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                newFit.UpdateBaseShipFromFitting()
                newFit.BaseShip.DamageProfile = HQFDamageProfiles.ProfileList("<Omni-Damage>")
                newFit.PilotName = aPilot.PilotName
                newFit.ApplyFitting(BuildType.BuildEverything)
                _attackingShip = newFit.FittedShip
                ' Check hislots of attacker to find turret data
                Dim sMod As ShipModule
                _tMod = New ShipModule
                _mMod = New ShipModule
                Dim mixedTurrets As Boolean = False
                Dim mixedAmmo As Boolean = False
                Dim mixedLaunchers As Boolean = False
                Dim mixedMissiles As Boolean = False
                _tc = 0
                _mc = 0
                For slot As Integer = 1 To _attackingShip.HiSlots
                    If _attackingShip.HiSlot(slot) IsNot Nothing Then
                        sMod = _attackingShip.HiSlot(slot)
                        If sMod.IsTurret Then
                            If _tMod.Name <> "" Or _tMod.Name Is Nothing Then
                                If sMod.ModuleState >= 4 Then
                                    If sMod.LoadedCharge IsNot Nothing Then
                                        _tMod = sMod
                                        _tc += 1
                                    End If
                                End If
                            Else
                                ' Check if we match
                                If sMod.Name <> _tMod.Name Then
                                    mixedTurrets = True
                                Else
                                    If sMod.ModuleState >= 4 Then
                                        If sMod.LoadedCharge.Name <> _tMod.LoadedCharge.Name Then
                                            mixedAmmo = True
                                        Else
                                            _tc += 1
                                        End If
                                    End If
                                End If
                            End If
                        ElseIf sMod.IsLauncher Then
                            If _mMod.Name <> "" Or _mMod.Name Is Nothing Then
                                If sMod.ModuleState >= 4 Then
                                    If sMod.LoadedCharge IsNot Nothing Then
                                        _mMod = sMod
                                        _mc += 1
                                    End If
                                End If
                            Else
                                ' Check if we match
                                If sMod.Name <> _mMod.Name Then
                                    mixedLaunchers = True
                                Else
                                    If sMod.ModuleState >= 4 Then
                                        If sMod.LoadedCharge.Name <> _mMod.LoadedCharge.Name Then
                                            mixedMissiles = True
                                        Else
                                            _mc += 1
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
                If EveSpace1.SourceShip Is Nothing Then
                    EveSpace1.SourceShip = New ShipStatus("SourceShip", _attackingShip, New Point(100, 100), New Point(300, 300))
                Else
                    EveSpace1.SourceShip = New ShipStatus("SourceShip", _attackingShip, EveSpace1.SourceShip.Location, EveSpace1.SourceShip.Heading)
                End If
                ' Check for mixed turrets and ammo
                If mixedTurrets = True Then
                    Const msg As String = "HQF has detected that you are using a setup with multiple turret types. As such, only the first turret (and identical instances thereof) will be used for the calculations."
                    MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If mixedAmmo = True Then
                        Const msg As String = "HQF has detected that you are using a setup with varying ammo types. As such, only the first turret (and identical instances thereof) will be used for the calculations."
                        MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        If mixedLaunchers = True Then
                            Const msg As String = "HQF has detected that you are using a setup with varying launcher types. As such, only the first launcher (and identical instances thereof) will be used for the calculations."
                            MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            If mixedMissiles = True Then
                                Const msg As String = "HQF has detected that you are using a setup with varying missile types. As such, only the first launcher (and identical instances thereof) will be used for the calculations."
                                MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End If
                    End If
                End If
                ' Update selected tab
                If _tc > 0 Then
                    tabStats.SelectedTab = tabTurretStats
                ElseIf _mc > 0 Then
                    tabStats.SelectedTab = tabMissileStats
                End If
            End If
        End Sub

        Private Sub SetTargetShip()
            If cboTargetFitting.SelectedItem IsNot Nothing And cboTargetPilot.SelectedItem IsNot Nothing Then
                Dim shipFit As String = cboTargetFitting.SelectedItem.ToString
                Dim aPilot As FittingPilot = FittingPilots.HQFPilots(cboTargetPilot.SelectedItem.ToString)
                Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                newFit.UpdateBaseShipFromFitting()
                newFit.BaseShip.DamageProfile = HQFDamageProfiles.ProfileList("<Omni-Damage>")
                newFit.PilotName = aPilot.PilotName
                newFit.ApplyFitting(BuildType.BuildEverything)
                _targetShip = newFit.FittedShip
                If EveSpace1.TargetShip Is Nothing Then
                    EveSpace1.TargetShip = New ShipStatus("TargetShip", _targetShip, New Point(250, 250), New Point(150, 150))
                Else
                    EveSpace1.TargetShip = New ShipStatus("TargetShip", _targetShip, EveSpace1.TargetShip.Location, EveSpace1.TargetShip.Heading)
                End If
                btnRangeVsHitChance.Enabled = True
            End If
        End Sub

#End Region

#Region "UI Routines"
        Private Sub nudRange_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudRange.ValueChanged
            EveSpace1.RangeScale = nudRange.Value
        End Sub

        Private Sub nudVel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudVel.ValueChanged
            EveSpace1.VelocityScale = nudVel.Value
        End Sub

        Private Sub btnRangeVSHitChance_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRangeVsHitChance.Click
            ' Prepare a new graph to display
            If EveSpace1.SourceShip IsNot Nothing And EveSpace1.TargetShip IsNot Nothing Then
                If _graphForm.IsHandleCreated = False Then

                    _graphForm = New FrmChartViewer

                    ' Create series
                    _graphForm.chartHQF.Series.Clear()
                    _graphForm.chartHQF.Series.Add("Range")
                    _graphForm.chartHQF.Series.Add("Tracking")
                    _graphForm.chartHQF.Series.Add("Combined")
                    _graphForm.chartHQF.Series.Add("Missile")

                    ' Set chart type and styles
                    _graphForm.chartHQF.Series("Range").ChartType = SeriesChartType.FastLine
                    _graphForm.chartHQF.Series("Range").XAxisType = AxisType.Primary
                    _graphForm.chartHQF.Series("Tracking").ChartType = SeriesChartType.FastLine
                    _graphForm.chartHQF.Series("Combined").ChartType = SeriesChartType.FastLine


                    ' Set the titles and axis labels
                    Dim maxRange As Double = Math.Max(_tor + (2 * _tf), _mor * 1.5)
                    Const graphPoints As Integer = 50
                    _graphForm.chartHQF.Titles(0).Text = "Range vs Hit Chance - " & EveSpace1.SourceShip.Ship.Name

                    _graphForm.chartHQF.ChartAreas("Default").AxisX.Minimum = 0
                    _graphForm.chartHQF.ChartAreas("Default").AxisX.Maximum = Math.Max(_tor + (2 * _tf), _mor * 1.5)
                    _graphForm.chartHQF.ChartAreas("Default").AxisY.Minimum = 0
                    _graphForm.chartHQF.ChartAreas("Default").AxisY.Maximum = 100

                    ' Create some points from 0 to tor + (2 * tf)
                    Dim x As Double, y As Double

                    For x = 0 To maxRange Step maxRange / graphPoints
                        If _tc > 0 Then
                            y = (0.5 ^ (((Math.Max(0, x - _tor)) / _tf) ^ 2)) * 100 ' Range Element Only
                            _graphForm.chartHQF.Series("Range").Points.AddXY(x, y)
                            y = (0.5 ^ ((((_transVel / (x * _tt)) * (_tsr / _sr)) ^ 2))) * 100 ' Tracking Element Only
                            _graphForm.chartHQF.Series("Tracking").Points.AddXY(x, y)
                            y = (0.5 ^ ((((_transVel / (x * _tt)) * (_tsr / _sr)) ^ 2) + ((Math.Max(0, x - _tor)) / _tf) ^ 2)) * 100 ' Combined
                            _graphForm.chartHQF.Series("Combined").Points.AddXY(x, y)
                        End If
                        If _mc > 0 Then
                            y = 100 - Math.Min(Int(x / _mor) * 100, 100)
                            _graphForm.chartHQF.Series("Missile").Points.AddXY(x, y)
                        End If
                    Next
                    Dim bestRange As Double = 0
                    Dim bestChance As Double = 0
                    For x = 0 To maxRange
                        y = (0.5 ^ ((((_transVel / (x * _tt)) * (_tsr / _sr)) ^ 2) + ((Math.Max(0, x - _tor)) / _tf) ^ 2)) * 100 ' Combined
                        If y > bestChance Then
                            bestChance = y
                            bestRange = x
                        End If
                    Next

                    ' Write some detail
                    Dim info As New StringBuilder
                    info.AppendLine("Current chance to hit = " & _cth.ToString("N2") & "% @ " & _d.ToString("N0") & "m")
                    info.AppendLine("Highest chance to hit = " & bestChance.ToString("N2") & "% @ " & bestRange.ToString("N0") & "m")
                    _graphForm.lblGraphInfo.Text = info.ToString

                    _graphForm.Show()
                Else
                    _graphForm.BringToFront()
                    _graphForm.Visible = True
                End If
            End If
        End Sub

        Private Sub btnOptimalRange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOptimalRange.Click
            If _tMod.Name <> "" Then
                Dim r As Double = Math.Round(CDbl(_tMod.Attributes(54)) / 80000, 5)
                r = Math.Max(r, nudRange.Minimum)
                nudRange.Value = CDec(r)
            End If
        End Sub

#End Region

#Region "Data Update Routines"

        Private Sub UpdateGraph() Handles EveSpace1.GraphUpdateRequired
            ' Update the Graph?
            If _graphForm.IsHandleCreated = True Then
                Call UpdateCurves()
            End If
        End Sub

        Private Sub UpdateCurves()

            ' Reset series
            _graphForm.chartHQF.Series("Range").Points.Clear()
            _graphForm.chartHQF.Series("Tracking").Points.Clear()
            _graphForm.chartHQF.Series("Combined").Points.Clear()
            _graphForm.chartHQF.Series("Missile").Points.Clear()

            Dim maxRange As Double = Math.Max(_tor + (2 * _tf), _mor * 1.5)
            Const graphPoints As Integer = 50

            ' Create some points from 0 to tor + (2 * tf)
            Dim x As Double, y As Double

            For x = 0 To maxRange Step maxRange / graphPoints
                If _tc > 0 Then
                    y = (0.5 ^ (((Math.Max(0, x - _tor)) / _tf) ^ 2)) * 100 ' Range Element Only
                    _graphForm.chartHQF.Series("Range").Points.AddXY(x, y)
                    y = (0.5 ^ ((((_transVel / (x * _tt)) * (_tsr / _sr)) ^ 2))) * 100 ' Tracking Element Only
                    _graphForm.chartHQF.Series("Tracking").Points.AddXY(x, y)
                    y = (0.5 ^ ((((_transVel / (x * _tt)) * (_tsr / _sr)) ^ 2) + ((Math.Max(0, x - _tor)) / _tf) ^ 2)) * 100 ' Combined
                    _graphForm.chartHQF.Series("Combined").Points.AddXY(x, y)
                End If
                If _mc > 0 Then
                    y = 100 - Math.Min(Int(x / _mor) * 100, 100)
                    _graphForm.chartHQF.Series("Missile").Points.AddXY(x, y)
                End If
            Next
            Dim bestRange As Double = 0
            Dim bestChance As Double = 0
            For x = 0 To maxRange
                y = (0.5 ^ ((((_transVel / (x * _tt)) * (_tsr / _sr)) ^ 2) + ((Math.Max(0, x - _tor)) / _tf) ^ 2)) * 100 ' Combined
                If y > bestChance Then
                    bestChance = y
                    bestRange = x
                End If
            Next

            ' Write some detail
            Dim info As New StringBuilder
            info.AppendLine("Current chance to hit = " & _cth.ToString("N2") & "% @ " & _d.ToString("N0") & "m")
            info.AppendLine("Highest chance to hit = " & bestChance.ToString("N2") & "% @ " & bestRange.ToString("N0") & "m")
            _graphForm.lblGraphInfo.Text = info.ToString

        End Sub

        Private Sub UpdateTurretStats() Handles EveSpace1.CalculationsChanged
            ' Set variables
            _sr = EveSpace1.TargetShip.Ship.SigRadius
            _transVel = EveSpace1.Transversal
            _d = EveSpace1.Range * 1000
            _tsr = 0
            _tt = 0
            _tor = 0
            _tf = 0
            _tvd = 0
            _tdps = 0
            _sHp = EveSpace1.TargetShip.Ship.ShieldCapacity
            _aHp = EveSpace1.TargetShip.Ship.ArmorCapacity
            _hHp = EveSpace1.TargetShip.Ship.StructureCapacity
            _sEM = EveSpace1.TargetShip.Ship.ShieldEMResist
            _sEx = EveSpace1.TargetShip.Ship.ShieldExResist
            _sKi = EveSpace1.TargetShip.Ship.ShieldKiResist
            _sTh = EveSpace1.TargetShip.Ship.ShieldThResist
            _aEM = EveSpace1.TargetShip.Ship.ArmorEMResist
            _aEx = EveSpace1.TargetShip.Ship.ArmorExResist
            _aKi = EveSpace1.TargetShip.Ship.ArmorKiResist
            _aTh = EveSpace1.TargetShip.Ship.ArmorThResist
            _hEM = EveSpace1.TargetShip.Ship.StructureEMResist
            _hEx = EveSpace1.TargetShip.Ship.StructureExResist
            _hKi = EveSpace1.TargetShip.Ship.StructureKiResist
            _hTh = EveSpace1.TargetShip.Ship.StructureThResist

            If _tMod.Name <> "" And _tMod.Name IsNot Nothing Then
                _tsr = CDbl(_tMod.Attributes(620))
                _tt = CDbl(_tMod.Attributes(160))
                _tor = CDbl(_tMod.Attributes(54))
                _tf = CDbl(_tMod.Attributes(158))
                _tvd = CDbl(_tMod.Attributes(10018))
                _tdps = CDbl(_tMod.Attributes(10019))
                For att As Integer = 10011 To 10013
                    If _tMod.Attributes.ContainsKey(att) = True Then
                        _trof = _tMod.Attributes(att)
                        Exit For
                    End If
                Next
                _wEM = CDbl(_tMod.Attributes(10051))
                _wEx = CDbl(_tMod.Attributes(10052))
                _wKi = CDbl(_tMod.Attributes(10053))
                _wTh = CDbl(_tMod.Attributes(10054))

                ' Calculate weapon damage split
                _sdEM = _wEM * (1 - _sEM / 100) : _sdEx = _wEx * (1 - _sEx / 100) : _sdKi = _wKi * (1 - _sKi / 100) : _sdTh = _wTh * (1 - _sTh / 100) : _sdT = _sdEM + _sdEx + _sdKi + _sdTh
                _adEM = _wEM * (1 - _aEM / 100) : _adEx = _wEx * (1 - _aEx / 100) : _adKi = _wKi * (1 - _aKi / 100) : _adTh = _wTh * (1 - _aTh / 100) : _adT = _adEM + _adEx + _adKi + _adTh
                _hdEM = _wEM * (1 - _hEM / 100) : _hdEx = _wEx * (1 - _hEx / 100) : _hdKi = _wKi * (1 - _hKi / 100) : _hdTh = _wTh * (1 - _hTh / 100) : _hdT = _hdEM + _hdEx + _hdKi + _hdTh

                'ChanceToHit = 0.5 ^ ((((Transversal speed/(Range to target * Turret Tracking))*(Turret Signature Resolution / Target Signature Radius))^2) + ((max(0, Range to target - Turret Optimal Range))/Turret Falloff)^2) 
                _cth = (0.5 ^ ((((_transVel / (_d * _tt)) * (_tsr / _sr)) ^ 2) + ((Math.Max(0, _d - _tor)) / _tf) ^ 2)) * 100

                ' Calculate expected damage ratio
                If Math.Round(_cth, 8) = 0 Then
                    _edr = 0
                Else
                    _edr = 0.01 * 3 + ((_cth / 100) - 0.01) * ((_cth / 100) + 0.99) / 2
                End If

                ' Calculate Expected Damage (DPS)
                _esdT = _sdT * _edr
                _eadT = _adT * _edr
                _ehdT = _hdT * _edr

                ' Calculate target recharge rates
                _tsrr = CDbl(EveSpace1.TargetShip.Ship.Attributes(10065))
                _tarr = CDbl(EveSpace1.TargetShip.Ship.Attributes(10066))
                _thrr = CDbl(EveSpace1.TargetShip.Ship.Attributes(10067))

                ' Calculate passive recharge rate
                Dim prr As Double = (EveSpace1.TargetShip.Ship.ShieldCapacity / EveSpace1.TargetShip.Ship.ShieldRecharge) / 5

                ' Calculate Expected Times (no target ship HP recharge)
                _estNr = _sHp / (_esdT * _tc) * _trof
                _eatNr = _aHp / (_eadT * _tc) * _trof
                _ehtNr = _hHp / (_ehdT * _tc) * _trof
                If _estNr > 86400 Then
                    _estNr = 86400
                End If
                If _eatNr > 86400 Then
                    _eatNr = 86400
                End If
                If _ehtNr > 86400 Then
                    _ehtNr = 86400
                End If

                ' Calculate Expected Times (with target ship HP recharge)
                If ((_esdT * _tc) - _tsrr) > 0 Then
                    _estR = _sHp / ((_esdT * _tc) - _tsrr) * _trof
                Else
                    _estR = 86400
                End If
                If ((_eadT * _tc) - _tarr - prr) > 0 Then
                    _eatR = _aHp / ((_eadT * _tc) - _tarr - prr) * _trof
                Else
                    _eatR = 86400
                End If
                If ((_ehdT * _tc) - _thrr - prr) > 0 Then
                    _ehtR = _hHp / ((_ehdT * _tc) - _thrr - prr) * _trof
                Else
                    _ehtR = 86400
                End If

                ' Calculate recharge based DPS and damage
                'Dim srrd, arrd, hrrd, trrd As Double
                'srrd = sHP / estR : arrd = aHP / eatR : hrrd = hHP / ehtR : trrd = (sHP + aHP + hHP) / (estR + eatR + ehtR)
                'Dim sh, ah, hh, th As Double
                'sh = Math.Round(estR * tc / trof, 0) : ah = Math.Round(eatR * tc / trof, 0) : hh = Math.Round(ehtR * tc / trof, 0) : th = sh + ah + hh
                'Dim ash, aah, ahh, atth As Double
                'ash = sHP / sh : aah = aHP / ah : ahh = hHP / hh : atth = (sHP + aHP + hHP) / th

                ' Write stats
                Dim stats As New StringBuilder
                stats.AppendLine("Stats: " & _tMod.Name & " (" & _tMod.LoadedCharge.Name & ")")
                stats.AppendLine("Range: " & EveSpace1.Range.ToString("N2") & " km")
                stats.AppendLine("Attacker Velocity: " & EveSpace1.SourceShip.Velocity.ToString("N2") & " m/s")
                stats.AppendLine("Target Velocity: " & EveSpace1.TargetShip.Velocity.ToString("N2") & " m/s")
                stats.AppendLine("Trans: " & EveSpace1.Transversal.ToString("N2") & " m/s")
                stats.AppendLine("Target Sig Radius: " & EveSpace1.TargetShip.Ship.SigRadius.ToString("N2") & " m")
                stats.AppendLine("Turret Count: " & _tc.ToString("N0"))
                stats.AppendLine("Turret Sig Res: " & _tsr.ToString("N2") & " m")
                stats.AppendLine("Turret Tracking: " & _tt.ToString("N8") & " rad/s")
                stats.AppendLine("Turret Optimal: " & _tor.ToString("N0") & " m")
                stats.AppendLine("Turret Falloff: " & _tf.ToString("N0") & " m")
                stats.AppendLine("Turret ROF: " & _trof.ToString("N2") & " s")
                stats.AppendLine("Turret Volley: " & _tvd.ToString("N2") & " HP")
                stats.AppendLine("Turret DPS: " & _tdps.ToString("N2") & " HP/s")
                stats.AppendLine("Turret Damage (EM/Ex/Ki/Th): " & _wEM.ToString("N2") & " / " & _wEx.ToString("N2") & " / " & _wKi.ToString("N2") & " / " & _wTh.ToString("N2"))
                stats.AppendLine("Target HP (S/A/H): " & _sHp.ToString("N2") & " / " & _aHp.ToString("N2") & " / " & _hHp.ToString("N2"))
                stats.AppendLine("Target Shield Res (EM/Ex/Ki/Th): " & _sEM.ToString("N2") & " / " & _sEx.ToString("N2") & " / " & _sKi.ToString("N2") & " / " & _sTh.ToString("N2"))
                stats.AppendLine("Target Armor Res (EM/Ex/Ki/Th): " & _aEM.ToString("N2") & " / " & _aEx.ToString("N2") & " / " & _aKi.ToString("N2") & " / " & _aTh.ToString("N2"))
                stats.AppendLine("Target Hull Res (EM/Ex/Ki/Th): " & _hEM.ToString("N2") & " / " & _hEx.ToString("N2") & " / " & _hKi.ToString("N2") & " / " & _hTh.ToString("N2"))
                stats.AppendLine("")
                stats.AppendLine("Chance to Hit: " & _cth.ToString("N8") & "%")
                stats.AppendLine("Expected Damage Ratio: " & _edr.ToString("N8"))
                stats.AppendLine("")
                stats.AppendLine("Theoretical Per Turret Damage (S/A/H): " & _sdT.ToString("N2") & " / " & _adT.ToString("N2") & " / " & _hdT.ToString("N2"))
                stats.AppendLine("Theoretical Per Turret DPS (S/A/H): " & (_sdT / _trof).ToString("N2") & " / " & (_adT / _trof).ToString("N2") & " / " & (_hdT / _trof).ToString("N2"))
                stats.AppendLine("Expected Per Turret Damage (S/A/H): " & _esdT.ToString("N2") & " / " & _eadT.ToString("N2") & " / " & _ehdT.ToString("N2"))
                stats.AppendLine("Expected Per Turret DPS (S/A/H): " & (_esdT / _trof).ToString("N2") & " / " & (_eadT / _trof).ToString("N2") & " / " & (_ehdT / _trof).ToString("N2"))
                stats.AppendLine("Theoretical Total Damage (S/A/H): " & (_sdT * _tc).ToString("N2") & " / " & (_adT * _tc).ToString("N2") & " / " & (_hdT * _tc).ToString("N2"))
                stats.AppendLine("Theoretical Total DPS (S/A/H): " & (_sdT / _trof * _tc).ToString("N2") & " / " & (_adT / _trof * _tc).ToString("N2") & " / " & (_hdT / _trof * _tc).ToString("N2"))
                stats.AppendLine("Expected Total Damage (S/A/H): " & (_esdT * _tc).ToString("N2") & " / " & (_eadT * _tc).ToString("N2") & " / " & (_ehdT * _tc).ToString("N2"))
                stats.AppendLine("Expected Total DPS (S/A/H): " & (_esdT / _trof * _tc).ToString("N2") & " / " & (_eadT / _trof * _tc).ToString("N2") & " / " & (_ehdT / _trof * _tc).ToString("N2"))
                stats.AppendLine("Target HP Recharge Rates (S/A/H): " & _tsrr.ToString("N2") & " / " & _tarr.ToString("N2") & " / " & _thrr.ToString("N2"))
                stats.AppendLine("Depletion Times NR (S/A/H): " & CStr(IIf(_estNr >= 86400, "Stable", SkillFunctions.TimeToString(_estNr))) & " / " & CStr(IIf(_eatNr >= 86400, "Stable", SkillFunctions.TimeToString(_eatNr))) & " / " & CStr(IIf(_ehtNr >= 86400, "Stable", SkillFunctions.TimeToString(_ehtNr))))
                stats.AppendLine("Depletion Times WR (S/A/H): " & CStr(IIf(_estR >= 86400, "Stable", SkillFunctions.TimeToString(_estR))) & " / " & CStr(IIf(_eatR >= 86400, "Stable", SkillFunctions.TimeToString(_eatR))) & " / " & CStr(IIf(_ehtR >= 86400, "Stable", SkillFunctions.TimeToString(_ehtR))))
                'stats.AppendLine("Average Turret Shot (S/A/H/T): " & ash.ToString("N2") & " / " & aah.ToString("N2") & " / " & ahh.ToString("N2") & " / " & atth.ToString("N2"))
                'stats.AppendLine("Average Turret DPS (S/A/H/T): " & srrd.ToString("N2") & " / " & arrd.ToString("N2") & " / " & hrrd.ToString("N2") & " / " & trrd.ToString("N2"))

                lblTurretStats.Text = stats.ToString
                lblTurretStats.Refresh()
            Else
                lblTurretStats.Text = "No valid/active turret modules found on attacking ship!"
                lblTurretStats.Refresh()
            End If

            ' Update missile stuff
            Call UpdateMissileStats()

        End Sub

        Private Sub UpdateMissileStats()
            ' Taken from http://www.eveonline.com/ingameboard.asp?a=topic&threadID=901280
            ' Damage = Base_Damage * MIN(MIN(sig / Er, 1), (Ev / Er * sig / vel) ^ (log(drf) / log(oaeDamageReductionSensitivity)))

            ' Set variables
            _sr = EveSpace1.TargetShip.Ship.SigRadius
            _targetVel = EveSpace1.TargetShip.Velocity
            _transVel = EveSpace1.Transversal
            _d = EveSpace1.Range * 1000
            _sHp = EveSpace1.TargetShip.Ship.ShieldCapacity
            _aHp = EveSpace1.TargetShip.Ship.ArmorCapacity
            _hHp = EveSpace1.TargetShip.Ship.StructureCapacity
            _sEM = EveSpace1.TargetShip.Ship.ShieldEMResist
            _sEx = EveSpace1.TargetShip.Ship.ShieldExResist
            _sKi = EveSpace1.TargetShip.Ship.ShieldKiResist
            _sTh = EveSpace1.TargetShip.Ship.ShieldThResist
            _aEM = EveSpace1.TargetShip.Ship.ArmorEMResist
            _aEx = EveSpace1.TargetShip.Ship.ArmorExResist
            _aKi = EveSpace1.TargetShip.Ship.ArmorKiResist
            _aTh = EveSpace1.TargetShip.Ship.ArmorThResist
            _hEM = EveSpace1.TargetShip.Ship.StructureEMResist
            _hEx = EveSpace1.TargetShip.Ship.StructureExResist
            _hKi = EveSpace1.TargetShip.Ship.StructureKiResist
            _hTh = EveSpace1.TargetShip.Ship.StructureThResist

            If _mMod.Name <> "" And _mMod.Name IsNot Nothing Then
                _mor = CDbl(_mMod.Attributes(54))
                _mvd = CDbl(_mMod.Attributes(10018))
                _mdps = CDbl(_mMod.Attributes(10019))
                _mrof = CDbl(_mMod.Attributes(51))
                _missileEr = CDbl(_mMod.LoadedCharge.Attributes(654))
                _missileEv = CDbl(_mMod.LoadedCharge.Attributes(653))
                _missileDrf = CDbl(_mMod.LoadedCharge.Attributes(1353))
                _missileDrs = CDbl(_mMod.LoadedCharge.Attributes(1354))
                _wEM = CDbl(_mMod.LoadedCharge.Attributes(114))
                _wEx = CDbl(_mMod.LoadedCharge.Attributes(116))
                _wKi = CDbl(_mMod.LoadedCharge.Attributes(117))
                _wTh = CDbl(_mMod.LoadedCharge.Attributes(118))
                _wT = _wEM + _wEx + _wKi + _wTh

                ' Calculate weapon damage split
                _sdEM = _wEM * (1 - _sEM / 100) : _sdEx = _wEx * (1 - _sEx / 100) : _sdKi = _wKi * (1 - _sKi / 100) : _sdTh = _wTh * (1 - _sTh / 100) : _sdT = _sdEM + _sdEx + _sdKi + _sdTh
                _adEM = _wEM * (1 - _aEM / 100) : _adEx = _wEx * (1 - _aEx / 100) : _adKi = _wKi * (1 - _aKi / 100) : _adTh = _wTh * (1 - _aTh / 100) : _adT = _adEM + _adEx + _adKi + _adTh
                _hdEM = _wEM * (1 - _hEM / 100) : _hdEx = _wEx * (1 - _hEx / 100) : _hdKi = _wKi * (1 - _hKi / 100) : _hdTh = _wTh * (1 - _hTh / 100) : _hdT = _hdEM + _hdEx + _hdKi + _hdTh

                ' Calculate the actual damage
                If _mor >= _d Then
                    _cth = _wT * Math.Min(Math.Min(_sr / _missileEr, 1), (_missileEv / _missileEr * _sr / _targetVel) ^ (Math.Log(_missileDrf) / Math.Log(_missileDrs)))
                Else
                    _cth = 0
                End If
                _edr = _cth / _wT

                ' Calculate Expected Damage (DPS)
                _esdT = _sdT * _edr
                _eadT = _adT * _edr
                _ehdT = _hdT * _edr

                ' Calculate target recharge rates
                _tsrr = CDbl(EveSpace1.TargetShip.Ship.Attributes(10065))
                _tarr = CDbl(EveSpace1.TargetShip.Ship.Attributes(10066))
                _thrr = CDbl(EveSpace1.TargetShip.Ship.Attributes(10067))

                ' Calculate passive recharge rate
                Dim prr As Double = (EveSpace1.TargetShip.Ship.ShieldCapacity / EveSpace1.TargetShip.Ship.ShieldRecharge) / 5

                ' Calculate Expected Times (no target ship HP recharge)
                _estNr = _sHp / (_esdT * _mc) * _mrof : _eatNr = _aHp / (_eadT * _mc) * _mrof : _ehtNr = _hHp / (_ehdT * _mc) * _mrof
                If _estNr > 86400 Then
                    _estNr = 86400
                End If
                If _eatNr > 86400 Then
                    _eatNr = 86400
                End If
                If _ehtNr > 86400 Then
                    _ehtNr = 86400
                End If

                ' Calculate Expected Times (with target ship HP recharge)
                If ((_esdT * _mc) - _tsrr) > 0 Then
                    _estR = _sHp / ((_esdT * _mc) - _tsrr) * _mrof
                Else
                    _estR = 86400
                End If
                If ((_eadT * _mc) - _tarr - prr) > 0 Then
                    _eatR = _aHp / ((_eadT * _mc) - _tarr - prr) * _mrof
                Else
                    _eatR = 86400
                End If
                If ((_ehdT * _mc) - _thrr - prr) > 0 Then
                    _ehtR = _hHp / ((_ehdT * _mc) - _thrr - prr) * _mrof
                Else
                    _ehtR = 86400
                End If

                ' Write stats
                Dim stats As New StringBuilder
                stats.AppendLine("Stats: " & _mMod.Name & " (" & _mMod.LoadedCharge.Name & ")")
                stats.AppendLine("Range: " & EveSpace1.Range.ToString("N2") & " km")
                stats.AppendLine("Attacker Velocity: " & EveSpace1.SourceShip.Velocity.ToString("N2") & " m/s")
                stats.AppendLine("Target Velocity: " & EveSpace1.TargetShip.Velocity.ToString("N2") & " m/s")
                stats.AppendLine("Trans: " & EveSpace1.Transversal.ToString("N2") & " m/s")
                stats.AppendLine("Target Sig Radius: " & EveSpace1.TargetShip.Ship.SigRadius.ToString("N2") & " m")
                stats.AppendLine("Missile Count: " & _mc.ToString("N0"))
                stats.AppendLine("Missile Ex Radius: " & _missileEr.ToString("N2") & " m")
                stats.AppendLine("Missile Ex Velocity: " & _missileEv.ToString("N2") & " m/s")
                stats.AppendLine("Missile Range: " & _mor.ToString("N0") & " m")
                stats.AppendLine("Missile ROF: " & _mrof.ToString("N2") & " s")
                stats.AppendLine("Missile Volley: " & _mvd.ToString("N2") & " HP")
                stats.AppendLine("Missile DPS: " & _mdps.ToString("N2") & " HP/s")
                stats.AppendLine("Missile Damage (EM/Ex/Ki/Th): " & _wEM.ToString("N2") & " / " & _wEx.ToString("N2") & " / " & _wKi.ToString("N2") & " / " & _wTh.ToString("N2"))
                stats.AppendLine("Target HP (S/A/H): " & _sHp.ToString("N2") & " / " & _aHp.ToString("N2") & " / " & _hHp.ToString("N2"))
                stats.AppendLine("Target Shield Res (EM/Ex/Ki/Th): " & _sEM.ToString("N2") & " / " & _sEx.ToString("N2") & " / " & _sKi.ToString("N2") & " / " & _sTh.ToString("N2"))
                stats.AppendLine("Target Armor Res (EM/Ex/Ki/Th): " & _aEM.ToString("N2") & " / " & _aEx.ToString("N2") & " / " & _aKi.ToString("N2") & " / " & _aTh.ToString("N2"))
                stats.AppendLine("Target Hull Res (EM/Ex/Ki/Th): " & _hEM.ToString("N2") & " / " & _hEx.ToString("N2") & " / " & _hKi.ToString("N2") & " / " & _hTh.ToString("N2"))
                stats.AppendLine("")
                stats.AppendLine("Expected Damage: " & _cth.ToString("N2") & " HP")
                stats.AppendLine("Expected Damage Ratio: " & _edr.ToString("N8"))
                stats.AppendLine("")
                stats.AppendLine("Theoretical Per Missile Damage (S/A/H): " & _sdT.ToString("N2") & " / " & _adT.ToString("N2") & " / " & _hdT.ToString("N2"))
                stats.AppendLine("Theoretical Per Missile DPS (S/A/H): " & (_sdT / _mrof).ToString("N2") & " / " & (_adT / _mrof).ToString("N2") & " / " & (_hdT / _mrof).ToString("N2"))
                stats.AppendLine("Expected Per Missile Damage (S/A/H): " & _esdT.ToString("N2") & " / " & _eadT.ToString("N2") & " / " & _ehdT.ToString("N2"))
                stats.AppendLine("Expected Per Missile DPS (S/A/H): " & (_esdT / _mrof).ToString("N2") & " / " & (_eadT / _mrof).ToString("N2") & " / " & (_ehdT / _mrof).ToString("N2"))
                stats.AppendLine("Theoretical Total Damage (S/A/H): " & (_sdT * _mc).ToString("N2") & " / " & (_adT * _mc).ToString("N2") & " / " & (_hdT * _mc).ToString("N2"))
                stats.AppendLine("Theoretical Total DPS (S/A/H): " & (_sdT / _mrof * _mc).ToString("N2") & " / " & (_adT / _mrof * _mc).ToString("N2") & " / " & (_hdT / _mrof * _mc).ToString("N2"))
                stats.AppendLine("Expected Total Damage (S/A/H): " & (_esdT * _mc).ToString("N2") & " / " & (_eadT * _mc).ToString("N2") & " / " & (_ehdT * _mc).ToString("N2"))
                stats.AppendLine("Expected Total DPS (S/A/H): " & (_esdT / _mrof * _mc).ToString("N2") & " / " & (_eadT / _mrof * _mc).ToString("N2") & " / " & (_ehdT / _mrof * _mc).ToString("N2"))
                stats.AppendLine("Target HP Recharge Rates (S/A/H): " & _tsrr.ToString("N2") & " / " & _tarr.ToString("N2") & " / " & _thrr.ToString("N2"))
                stats.AppendLine("Depletion Times NR (S/A/H): " & CStr(IIf(_estNr >= 86400, "Stable", SkillFunctions.TimeToString(_estNr))) & " / " & CStr(IIf(_eatNr >= 86400, "Stable", SkillFunctions.TimeToString(_eatNr))) & " / " & CStr(IIf(_ehtNr >= 86400, "Stable", SkillFunctions.TimeToString(_ehtNr))))
                stats.AppendLine("Depletion Times WR (S/A/H): " & CStr(IIf(_estR >= 86400, "Stable", SkillFunctions.TimeToString(_estR))) & " / " & CStr(IIf(_eatR >= 86400, "Stable", SkillFunctions.TimeToString(_eatR))) & " / " & CStr(IIf(_ehtR >= 86400, "Stable", SkillFunctions.TimeToString(_ehtR))))
                'stats.AppendLine("Average Missile Shot (S/A/H/T): " & ash.ToString("N2") & " / " & aah.ToString("N2") & " / " & ahh.ToString("N2") & " / " & atth.ToString("N2"))
                'stats.AppendLine("Average Missile DPS (S/A/H/T): " & srrd.ToString("N2") & " / " & arrd.ToString("N2") & " / " & hrrd.ToString("N2") & " / " & trrd.ToString("N2"))

                lblMissileStats.Text = stats.ToString
                lblMissileStats.Refresh()
            Else
                lblMissileStats.Text = "No valid/active missile modules found on attacking ship!"
                lblMissileStats.Refresh()
            End If

        End Sub

#End Region

        Private Sub radMovableVel_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles radMovableVel.CheckedChanged
            If EveSpace1.SourceShip IsNot Nothing Then
                EveSpace1.SourceShip.Velocity = nudAttackerVel.Value
            End If
            If EveSpace1.TargetShip IsNot Nothing Then
                EveSpace1.TargetShip.Velocity = nudTargetVel.Value
            End If
            EveSpace1.UseIntegratedVelocity = radMovableVel.Checked
        End Sub

        Private Sub nudAttackerVel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudAttackerVel.ValueChanged
            EveSpace1.AttackerVelocity = nudAttackerVel.Value
        End Sub

        Private Sub nudTargetVel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudTargetVel.ValueChanged
            EveSpace1.TargetVelocity = nudTargetVel.Value
        End Sub
    End Class
End Namespace