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
Imports System.Text
Imports System.Drawing
Imports ZedGraph

Public Class frmDamageAnalysis

    Dim attackingShip, targetShip As Ship
    Dim tMod As New ShipModule
    Dim mMod As New ShipModule
    Dim sr As Double = 0
    Dim transVel As Double = 0
    Dim targetVel As Double = 0
    Dim d As Double = 0
    Dim tsr As Double = 0 ' Turret Signature Radius
    Dim tt As Double = 0 ' Turret Tracking
    Dim tor As Double = 0 ' Turret Optimal Range
    Dim tf As Double = 0 ' Turret Falloff
    Dim tvd As Double = 0 ' Turret Volley Damage
    Dim tdps As Double = 0 ' Turret DPS
    Dim tc As Integer = 0 ' Turret Count 
    Dim trof As Double = 0 ' Turret ROF
    Dim mc As Integer = 0 ' Missile Count
    Dim mor As Double = 0 ' Missile Optimal Range
    Dim mrof As Double = 0 ' Missile ROF
    Dim missileER As Double = 0
    Dim missileEV As Double = 0
    Dim missileDRF As Double = 0
    Dim missileDRS As Double = 0
    Dim mvd As Double = 0 ' Missile Volley Damage
    Dim mdps As Double = 0 ' Missile DPS
    Dim sHP, aHP, hHP As Double
    Dim sEM, sEx, sKi, sTh As Double
    Dim aEM, aEx, aKi, aTh As Double
    Dim hEM, hEx, hKi, hTh As Double
    Dim wEM, wEx, wKi, wTh, wT As Double
    Dim sdEM, sdEx, sdKi, sdTh, sdT As Double
    Dim adEM, adEx, adKi, adTh, adT As Double
    Dim hdEM, hdEx, hdKi, hdTh, hdT As Double
    Dim esdEM, esdEx, esdKi, esdTh, esdT As Double
    Dim eadEM, eadEx, eadKi, eadTh, eadT As Double
    Dim ehdEM, ehdEx, ehdKi, ehdTh, ehdT As Double
    Dim tsrr, tarr, thrr As Double
    Dim estNR, eatNR, ehtNR As Double
    Dim estR, eatR, ehtR As Double
    Dim CTH As Double = 0
    Dim EDR As Double = 0
    Dim GraphForm As New frmChartViewer
    Dim iconVelocity As Boolean = True

#Region "Public Properties"
    Dim cFittingName As String = ""
    Dim cPilotName As String = ""
    Public Property FittingName() As String
        Get
            Return cFittingName
        End Get
        Set(ByVal value As String)
            cFittingName = value
        End Set
    End Property
    Public Property PilotName() As String
        Get
            Return cPilotName
        End Get
        Set(ByVal value As String)
            cPilotName = value
        End Set
    End Property
#End Region

#Region "Form Loading Routines"

    Private Sub frmDamageAnalysis_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.LoadFittingsAndPilots()
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
        If cFittingName <> "" Then
            cboAttackerFitting.SelectedItem = cFittingName
        End If
        ' Add the pilots
        For Each cPilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
            cboAttackerPilot.Items.Add(cPilot.Name)
            cboTargetPilot.Items.Add(cPilot.Name)
        Next
        ' Select a pilot
        If cPilotName <> "" Then
            cboAttackerPilot.SelectedItem = cPilotName
        End If
        cboAttackerFitting.EndUpdate() : cboTargetFitting.EndUpdate()
        cboAttackerPilot.EndUpdate() : cboTargetPilot.EndUpdate()
    End Sub

#End Region

#Region "Fitting and Pilot Changed Routines"

    Private Sub cboAttackerFitting_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAttackerFitting.SelectedIndexChanged
        Call Me.SetAttackingShip()
    End Sub

    Private Sub cboAttackerPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAttackerPilot.SelectedIndexChanged
        Call Me.SetAttackingShip()
    End Sub

    Private Sub cboTargetFitting_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTargetFitting.SelectedIndexChanged
        Call Me.SetTargetShip()
    End Sub

    Private Sub cboTargetPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTargetPilot.SelectedIndexChanged
        Call Me.SetTargetShip()
    End Sub

    Private Sub SetAttackingShip()
        If cboAttackerFitting.SelectedItem IsNot Nothing And cboAttackerPilot.SelectedItem IsNot Nothing Then
            Dim shipFit As String = cboAttackerFitting.SelectedItem.ToString
            Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboAttackerPilot.SelectedItem.ToString), HQFPilot)
            Dim NewFit As Fitting = Fittings.FittingList(shipFit).Clone
            NewFit.UpdateBaseShipFromFitting()
            NewFit.BaseShip.DamageProfile = CType(DamageProfiles.ProfileList("<Omni-Damage>"), DamageProfile)
            NewFit.PilotName = aPilot.PilotName
            NewFit.ApplyFitting(BuildType.BuildEverything)
            attackingShip = NewFit.FittedShip
            ' Check hislots of attacker to find turret data
            Dim sMod As New ShipModule
            tMod = New ShipModule
            mMod = New ShipModule
            Dim MixedTurrets As Boolean = False
            Dim MixedAmmo As Boolean = False
            Dim MixedLaunchers As Boolean = False
            Dim MixedMissiles As Boolean = False
            tc = 0
            mc = 0
            For slot As Integer = 1 To attackingShip.HiSlots
                If attackingShip.HiSlot(slot) IsNot Nothing Then
                    sMod = attackingShip.HiSlot(slot)
                    If sMod.IsTurret Then
                        If tMod.Name <> "" Or tMod.Name Is Nothing Then
                            If sMod.ModuleState >= 4 Then
                                If sMod.LoadedCharge IsNot Nothing Then
                                    tMod = sMod
                                    tc += 1
                                End If
                            End If
                        Else
                            ' Check if we match
                            If sMod.Name <> tMod.Name Then
                                MixedTurrets = True
                            Else
                                If sMod.ModuleState >= 4 Then
                                    If sMod.LoadedCharge.Name <> tMod.LoadedCharge.Name Then
                                        MixedAmmo = True
                                    Else
                                        tc += 1
                                    End If
                                End If
                                End If
                        End If
                    ElseIf sMod.IsLauncher Then
                        If mMod.Name <> "" Or mMod.Name Is Nothing Then
                            If sMod.ModuleState >= 4 Then
                                If sMod.LoadedCharge IsNot Nothing Then
                                    mMod = sMod
                                    mc += 1
                                End If
                            End If
                        Else
                            ' Check if we match
                            If sMod.Name <> mMod.Name Then
                                MixedLaunchers = True
                            Else
                                If sMod.ModuleState >= 4 Then
                                    If sMod.LoadedCharge.Name <> mMod.LoadedCharge.Name Then
                                        MixedMissiles = True
                                    Else
                                        mc += 1
                                    End If
                                End If
                                End If
                        End If
                    End If
                End If
            Next
            If EveSpace1.SourceShip Is Nothing Then
                EveSpace1.SourceShip = New ShipStatus("SourceShip", attackingShip, New Point(100, 100), New Point(300, 300))
            Else
                EveSpace1.SourceShip = New ShipStatus("SourceShip", attackingShip, EveSpace1.SourceShip.Location, EveSpace1.SourceShip.Heading)
            End If
            ' Check for mixed turrets and ammo
            If MixedTurrets = True Then
                Dim msg As String = "HQF has detected that you are using a setup with multiple turret types. As such, only the first turret (and identical instances thereof) will be used for the calculations."
                MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If MixedAmmo = True Then
                    Dim msg As String = "HQF has detected that you are using a setup with varying ammo types. As such, only the first turret (and identical instances thereof) will be used for the calculations."
                    MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If MixedLaunchers = True Then
                        Dim msg As String = "HQF has detected that you are using a setup with varying launcher types. As such, only the first launcher (and identical instances thereof) will be used for the calculations."
                        MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        If MixedMissiles = True Then
                            Dim msg As String = "HQF has detected that you are using a setup with varying missile types. As such, only the first launcher (and identical instances thereof) will be used for the calculations."
                            MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            End If
            ' Update selected tab
            If tc > 0 Then
                tabStats.SelectedTab = tabTurretStats
            ElseIf mc > 0 Then
                tabStats.SelectedTab = tabMissileStats
            End If
        End If
    End Sub

    Private Sub SetTargetShip()
        If cboTargetFitting.SelectedItem IsNot Nothing And cboTargetPilot.SelectedItem IsNot Nothing Then
            Dim shipFit As String = cboTargetFitting.SelectedItem.ToString
            Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboTargetPilot.SelectedItem.ToString), HQFPilot)
            Dim NewFit As Fitting = Fittings.FittingList(shipFit).Clone
            NewFit.UpdateBaseShipFromFitting()
            NewFit.BaseShip.DamageProfile = CType(DamageProfiles.ProfileList("<Omni-Damage>"), DamageProfile)
            NewFit.PilotName = aPilot.PilotName
            NewFit.ApplyFitting(BuildType.BuildEverything)
            targetShip = NewFit.FittedShip
            If EveSpace1.TargetShip Is Nothing Then
                EveSpace1.TargetShip = New ShipStatus("TargetShip", targetShip, New Point(250, 250), New Point(150, 150))
            Else
                EveSpace1.TargetShip = New ShipStatus("TargetShip", targetShip, EveSpace1.TargetShip.Location, EveSpace1.TargetShip.Heading)
            End If
            btnRangeVSHitChance.Enabled = True
        End If
    End Sub

#End Region

#Region "UI Routines"
    Private Sub nudRange_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRange.ValueChanged
        EveSpace1.RangeScale = nudRange.Value
    End Sub

    Private Sub nudVel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudVel.ValueChanged
        EveSpace1.VelocityScale = nudVel.Value
    End Sub

    Private Sub btnRangeVSHitChance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRangeVsHitChance.Click
        ' Prepare a new graph to display
        If EveSpace1.SourceShip IsNot Nothing And EveSpace1.TargetShip IsNot Nothing Then
            If GraphForm.IsHandleCreated = False Then
                GraphForm = New frmChartViewer
                Dim myPane As GraphPane = GraphForm.ZGC1.GraphPane

                ' Set the titles and axis labels
                Dim MaxRange As Double = Math.Max(tor + (2 * tf), mor * 1.5)
                Dim GraphPoints As Integer = 50
                myPane.Title.Text = "Range vs Hit Chance - " & EveSpace1.SourceShip.Ship.Name
                myPane.XAxis.Title.Text = "Range (m)"
                myPane.XAxis.Scale.Min = 0
                myPane.XAxis.Scale.Max = Math.Max(tor + (2 * tf), mor * 1.5)
                myPane.XAxis.Scale.MajorStep = tf
                myPane.XAxis.MajorGrid.IsVisible = True
                myPane.YAxis.Title.Text = "Hit Chance (%)"
                myPane.YAxis.Scale.Min = 0
                myPane.YAxis.Scale.Max = 110
                myPane.YAxis.Scale.MajorStep = 20
                myPane.YAxis.MajorGrid.IsVisible = True

                ' Create some points from 0 to tor + (2 * tf)
                Dim listTurretRange As New PointPairList
                Dim listTurretTracking As New PointPairList
                Dim listTurretCombined As New PointPairList
                Dim listMissileRange As New PointPairList
                Dim x As Double, y As Double

                For x = 0 To MaxRange Step MaxRange / GraphPoints
                    If tc > 0 Then
                        y = (0.5 ^ (((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Range Element Only
                        listTurretRange.Add(x, y)
                        y = (0.5 ^ ((((transVel / (x * tt)) * (tsr / sr)) ^ 2))) * 100 ' Tracking Element Only
                        listTurretTracking.Add(x, y)
                        y = (0.5 ^ ((((transVel / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
                        listTurretCombined.Add(x, y)
                    End If
                    If mc > 0 Then
                        y = 100 - Math.Min(Int(x / mor) * 100, 100)
                        listMissileRange.Add(x, y)
                    End If
                Next
                Dim bestRange As Double = 0
                Dim bestChance As Double = 0
                For x = 0 To MaxRange
                    y = (0.5 ^ ((((transVel / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
                    If y > bestChance Then
                        bestChance = y
                        bestRange = x
                    End If
                Next

                ' Generate curves
                If tc > 0 Then
                    Dim myCurveC As LineItem = myPane.AddCurve("Turret Combined", listTurretCombined, Color.Green, SymbolType.XCross)
                    Dim myCurveR As LineItem = myPane.AddCurve("Turret Range Element", listTurretRange, Color.Magenta, SymbolType.XCross)
                    Dim myCurveT As LineItem = myPane.AddCurve("Turret Tracking Element", listTurretTracking, Color.Blue, SymbolType.XCross)
                    myCurveR.Line.IsAntiAlias = True
                    myCurveT.Line.IsAntiAlias = True
                    myCurveC.Line.IsAntiAlias = True
                End If

                If mc > 0 Then
                    Dim myCurveM As LineItem = myPane.AddCurve("Missile Range", listMissileRange, Color.Firebrick, SymbolType.XCross)
                    myCurveM.Line.IsAntiAlias = True
                End If

                ' Fill the axis background with a color gradient
                myPane.Chart.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
                myPane.Legend.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
                myPane.Legend.FontSpec.FontColor = Color.MidnightBlue

                ' Fill the pane background with a color gradient
                myPane.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
                ' Calculate the Axis Scale Ranges
                GraphForm.ZGC1.AxisChange()

                ' Write some detail
                Dim info As New StringBuilder
                info.AppendLine("Current chance to hit = " & CTH.ToString("N2") & "% @ " & d.ToString("N0") & "m")
                info.AppendLine("Highest chance to hit = " & bestChance.ToString("N2") & "% @ " & bestRange.ToString("N0") & "m")
                GraphForm.lblGraphInfo.Text = info.ToString

                GraphForm.Show()
            Else
                GraphForm.BringToFront()
                GraphForm.Visible = True
            End If
        End If
    End Sub

    Private Sub btnOptimalRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptimalRange.Click
        If tMod.Name <> "" Then
            Dim r As Double = Math.Round(CDbl(tMod.Attributes("54")) / 80000, 5)
            r = Math.Max(r, nudRange.Minimum)
            nudRange.Value = CDec(r)
        End If
    End Sub

#End Region

#Region "Data Update Routines"

    Private Sub UpdateGraph() Handles EveSpace1.GraphUpdateRequired
        ' Update the Graph?
        If GraphForm.IsHandleCreated = True Then
            Call Me.UpdateCurves()
        End If
    End Sub

    Private Sub UpdateCurves()
        Dim myPane As GraphPane = GraphForm.ZGC1.GraphPane

        ' Create some points from 0 to 3*tor 
        Dim listR As New PointPairList()
        Dim listT As New PointPairList()
        Dim listC As New PointPairList
        Dim x As Double, y As Double

        For x = 0 To 3 * tor Step 1000 ' Range
            y = (0.5 ^ (((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Range Element Only
            listR.Add(x, y)
            y = (0.5 ^ ((((transVel / (x * tt)) * (tsr / sr)) ^ 2))) * 100 ' Tracking Element Only
            listT.Add(x, y)
            y = (0.5 ^ ((((transVel / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
            listC.Add(x, y)
        Next
        Dim bestRange As Double = 0
        Dim bestChance As Double = 0
        For x = 0 To 3 * tor
            y = (0.5 ^ ((((transVel / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
            If y > bestChance Then
                bestChance = y
                bestRange = x
            End If
        Next

        myPane.CurveList("Combined Chance").Points = listC
        myPane.CurveList("Range Element").Points = listR
        myPane.CurveList("Tracking Element").Points = listT

        ' Calculate the Axis Scale Ranges
        myPane.AxisChange()
        GraphForm.ZGC1.Invalidate()

        ' Write some detail
        Dim info As New StringBuilder
        info.AppendLine("Current chance to hit = " & CTH.ToString("N2") & "% @ " & d.ToString("N0") & "m")
        info.AppendLine("Highest chance to hit = " & bestChance.ToString("N2") & "% @ " & bestRange.ToString("N0") & "m")
        GraphForm.lblGraphInfo.Text = info.ToString


    End Sub

    Private Sub UpdateTurretStats() Handles EveSpace1.CalculationsChanged
        ' Set variables
        sr = EveSpace1.TargetShip.Ship.SigRadius
        transVel = EveSpace1.Transversal
        d = EveSpace1.Range * 1000
        tsr = 0
        tt = 0
        tor = 0
        tf = 0
        tvd = 0
        tdps = 0
        sHP = EveSpace1.TargetShip.Ship.ShieldCapacity
        aHP = EveSpace1.TargetShip.Ship.ArmorCapacity
        hHP = EveSpace1.TargetShip.Ship.StructureCapacity
        sEM = EveSpace1.TargetShip.Ship.ShieldEMResist
        sEx = EveSpace1.TargetShip.Ship.ShieldExResist
        sKi = EveSpace1.TargetShip.Ship.ShieldKiResist
        sTh = EveSpace1.TargetShip.Ship.ShieldThResist
        aEM = EveSpace1.TargetShip.Ship.ArmorEMResist
        aEx = EveSpace1.TargetShip.Ship.ArmorExResist
        aKi = EveSpace1.TargetShip.Ship.ArmorKiResist
        aTh = EveSpace1.TargetShip.Ship.ArmorThResist
        hEM = EveSpace1.TargetShip.Ship.StructureEMResist
        hEx = EveSpace1.TargetShip.Ship.StructureExResist
        hKi = EveSpace1.TargetShip.Ship.StructureKiResist
        hTh = EveSpace1.TargetShip.Ship.StructureThResist

        If tMod.Name <> "" And tMod.Name IsNot Nothing Then
            tsr = CDbl(tMod.Attributes("620"))
            tt = CDbl(tMod.Attributes("160"))
            tor = CDbl(tMod.Attributes("54"))
            tf = CDbl(tMod.Attributes("158"))
            tvd = CDbl(tMod.Attributes("10018"))
            tdps = CDbl(tMod.Attributes("10019"))
            For att As Integer = 10011 To 10013
                If tMod.Attributes.ContainsKey(att.ToString) = True Then
                    trof = tMod.Attributes(att.ToString)
                    Exit For
                End If
            Next
            wEM = CDbl(tMod.Attributes("10051"))
            wEx = CDbl(tMod.Attributes("10052"))
            wKi = CDbl(tMod.Attributes("10053"))
            wTh = CDbl(tMod.Attributes("10054"))

            ' Calculate weapon damage split
            sdEM = wEM * (1 - sEM / 100) : sdEx = wEx * (1 - sEx / 100) : sdKi = wKi * (1 - sKi / 100) : sdTh = wTh * (1 - sTh / 100) : sdT = sdEM + sdEx + sdKi + sdTh
            adEM = wEM * (1 - aEM / 100) : adEx = wEx * (1 - aEx / 100) : adKi = wKi * (1 - aKi / 100) : adTh = wTh * (1 - aTh / 100) : adT = adEM + adEx + adKi + adTh
            hdEM = wEM * (1 - hEM / 100) : hdEx = wEx * (1 - hEx / 100) : hdKi = wKi * (1 - hKi / 100) : hdTh = wTh * (1 - hTh / 100) : hdT = hdEM + hdEx + hdKi + hdTh

            'ChanceToHit = 0.5 ^ ((((Transversal speed/(Range to target * Turret Tracking))*(Turret Signature Resolution / Target Signature Radius))^2) + ((max(0, Range to target - Turret Optimal Range))/Turret Falloff)^2) 
            CTH = (0.5 ^ ((((transVel / (d * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, d - tor)) / tf) ^ 2)) * 100

            ' Calculate expected damage ratio
            If Math.Round(CTH, 8) = 0 Then
                EDR = 0
            Else
                EDR = 0.01 * 3 + ((CTH / 100) - 0.01) * ((CTH / 100) + 0.99) / 2
            End If

            ' Calculate Expected Damage (DPS)
            esdEM = sdEM * EDR : esdEx = sdEx * EDR : esdKi = sdKi * EDR : esdTh = sdTh * EDR : esdT = sdT * EDR
            eadEM = adEM * EDR : eadEx = adEx * EDR : eadKi = adKi * EDR : eadTh = adTh * EDR : eadT = adT * EDR
            ehdEM = hdEM * EDR : ehdEx = hdEx * EDR : ehdKi = hdKi * EDR : ehdTh = hdTh * EDR : ehdT = hdT * EDR

            ' Calculate target recharge rates
            tsrr = CDbl(EveSpace1.TargetShip.Ship.Attributes("10065"))
            tarr = CDbl(EveSpace1.TargetShip.Ship.Attributes("10066"))
            thrr = CDbl(EveSpace1.TargetShip.Ship.Attributes("10067"))

            ' Calculate passive recharge rate
            Dim prr As Double = (EveSpace1.TargetShip.Ship.ShieldCapacity / EveSpace1.TargetShip.Ship.ShieldRecharge) / 5

            ' Calculate Expected Times (no target ship HP recharge)
            estNR = sHP / (esdT * tc) * trof
            eatNR = aHP / (eadT * tc) * trof
            ehtNR = hHP / (ehdT * tc) * trof
            If estNR > 86400 Then
                estNR = 86400
            End If
            If eatNR > 86400 Then
                eatNR = 86400
            End If
            If ehtNR > 86400 Then
                ehtNR = 86400
            End If

            ' Calculate Expected Times (with target ship HP recharge)
            If ((esdT * tc) - tsrr) > 0 Then
                estR = sHP / ((esdT * tc) - tsrr) * trof
            Else
                estR = 86400
            End If
            If ((eadT * tc) - tarr - prr) > 0 Then
                eatR = aHP / ((eadT * tc) - tarr - prr) * trof
            Else
                eatR = 86400
            End If
            If ((ehdT * tc) - thrr - prr) > 0 Then
                ehtR = hHP / ((ehdT * tc) - thrr - prr) * trof
            Else
                ehtR = 86400
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
            stats.AppendLine("Stats: " & tMod.Name & " (" & tMod.LoadedCharge.Name & ")")
            stats.AppendLine("Range: " & EveSpace1.Range.ToString("N2") & " km")
            stats.AppendLine("Attacker Velocity: " & EveSpace1.SourceShip.Velocity.ToString("N2") & " m/s")
            stats.AppendLine("Target Velocity: " & EveSpace1.TargetShip.Velocity.ToString("N2") & " m/s")
            stats.AppendLine("Trans: " & EveSpace1.Transversal.ToString("N2") & " m/s")
            stats.AppendLine("Target Sig Radius: " & EveSpace1.TargetShip.Ship.SigRadius.ToString("N2") & " m")
            stats.AppendLine("Turret Count: " & tc.ToString("N0"))
            stats.AppendLine("Turret Sig Res: " & tsr.ToString("N2") & " m")
            stats.AppendLine("Turret Tracking: " & tt.ToString("N8") & " rad/s")
            stats.AppendLine("Turret Optimal: " & tor.ToString("N0") & " m")
            stats.AppendLine("Turret Falloff: " & tf.ToString("N0") & " m")
            stats.AppendLine("Turret ROF: " & trof.ToString("N2") & " s")
            stats.AppendLine("Turret Volley: " & tvd.ToString("N2") & " HP")
            stats.AppendLine("Turret DPS: " & tdps.ToString("N2") & " HP/s")
            stats.AppendLine("Turret Damage (EM/Ex/Ki/Th): " & wEM.ToString("N2") & " / " & wEx.ToString("N2") & " / " & wKi.ToString("N2") & " / " & wTh.ToString("N2"))
            stats.AppendLine("Target HP (S/A/H): " & sHP.ToString("N2") & " / " & aHP.ToString("N2") & " / " & hHP.ToString("N2"))
            stats.AppendLine("Target Shield Res (EM/Ex/Ki/Th): " & sEM.ToString("N2") & " / " & sEx.ToString("N2") & " / " & sKi.ToString("N2") & " / " & sTh.ToString("N2"))
            stats.AppendLine("Target Armor Res (EM/Ex/Ki/Th): " & aEM.ToString("N2") & " / " & aEx.ToString("N2") & " / " & aKi.ToString("N2") & " / " & aTh.ToString("N2"))
            stats.AppendLine("Target Hull Res (EM/Ex/Ki/Th): " & hEM.ToString("N2") & " / " & hEx.ToString("N2") & " / " & hKi.ToString("N2") & " / " & hTh.ToString("N2"))
            stats.AppendLine("")
            stats.AppendLine("Chance to Hit: " & CTH.ToString("N8") & "%")
            stats.AppendLine("Expected Damage Ratio: " & EDR.ToString("N8"))
            stats.AppendLine("")
            stats.AppendLine("Theoretical Per Turret Damage (S/A/H): " & sdT.ToString("N2") & " / " & adT.ToString("N2") & " / " & hdT.ToString("N2"))
            stats.AppendLine("Theoretical Per Turret DPS (S/A/H): " & (sdT / trof).ToString("N2") & " / " & (adT / trof).ToString("N2") & " / " & (hdT / trof).ToString("N2"))
            stats.AppendLine("Expected Per Turret Damage (S/A/H): " & esdT.ToString("N2") & " / " & eadT.ToString("N2") & " / " & ehdT.ToString("N2"))
            stats.AppendLine("Expected Per Turret DPS (S/A/H): " & (esdT / trof).ToString("N2") & " / " & (eadT / trof).ToString("N2") & " / " & (ehdT / trof).ToString("N2"))
            stats.AppendLine("Theoretical Total Damage (S/A/H): " & (sdT * tc).ToString("N2") & " / " & (adT * tc).ToString("N2") & " / " & (hdT * tc).ToString("N2"))
            stats.AppendLine("Theoretical Total DPS (S/A/H): " & (sdT / trof * tc).ToString("N2") & " / " & (adT / trof * tc).ToString("N2") & " / " & (hdT / trof * tc).ToString("N2"))
            stats.AppendLine("Expected Total Damage (S/A/H): " & (esdT * tc).ToString("N2") & " / " & (eadT * tc).ToString("N2") & " / " & (ehdT * tc).ToString("N2"))
            stats.AppendLine("Expected Total DPS (S/A/H): " & (esdT / trof * tc).ToString("N2") & " / " & (eadT / trof * tc).ToString("N2") & " / " & (ehdT / trof * tc).ToString("N2"))
            stats.AppendLine("Target HP Recharge Rates (S/A/H): " & tsrr.ToString("N2") & " / " & tarr.ToString("N2") & " / " & thrr.ToString("N2"))
            stats.AppendLine("Depletion Times NR (S/A/H): " & CStr(IIf(estNR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(estNR))) & " / " & CStr(IIf(eatNR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(eatNR))) & " / " & CStr(IIf(ehtNR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(ehtNR))))
            stats.AppendLine("Depletion Times WR (S/A/H): " & CStr(IIf(estR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(estR))) & " / " & CStr(IIf(eatR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(eatR))) & " / " & CStr(IIf(ehtR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(ehtR))))
            'stats.AppendLine("Average Turret Shot (S/A/H/T): " & ash.ToString("N2") & " / " & aah.ToString("N2") & " / " & ahh.ToString("N2") & " / " & atth.ToString("N2"))
            'stats.AppendLine("Average Turret DPS (S/A/H/T): " & srrd.ToString("N2") & " / " & arrd.ToString("N2") & " / " & hrrd.ToString("N2") & " / " & trrd.ToString("N2"))

            lblTurretStats.Text = stats.ToString
            lblTurretStats.Refresh()
        Else
            lblTurretStats.Text = "No valid/active turret modules found on attacking ship!"
            lblTurretStats.Refresh()
        End If

        ' Update missile stuff
        Call Me.UpdateMissileStats()

    End Sub

    Private Sub UpdateMissileStats()
        ' Taken from http://www.eveonline.com/ingameboard.asp?a=topic&threadID=901280
        ' Damage = Base_Damage * MIN(MIN(sig / Er, 1), (Ev / Er * sig / vel) ^ (log(drf) / log(oaeDamageReductionSensitivity)))

        ' Set variables
        sr = EveSpace1.TargetShip.Ship.SigRadius
        targetVel = EveSpace1.TargetShip.Velocity
        transVel = EveSpace1.Transversal
        d = EveSpace1.Range * 1000
        sHP = EveSpace1.TargetShip.Ship.ShieldCapacity
        aHP = EveSpace1.TargetShip.Ship.ArmorCapacity
        hHP = EveSpace1.TargetShip.Ship.StructureCapacity
        sEM = EveSpace1.TargetShip.Ship.ShieldEMResist
        sEx = EveSpace1.TargetShip.Ship.ShieldExResist
        sKi = EveSpace1.TargetShip.Ship.ShieldKiResist
        sTh = EveSpace1.TargetShip.Ship.ShieldThResist
        aEM = EveSpace1.TargetShip.Ship.ArmorEMResist
        aEx = EveSpace1.TargetShip.Ship.ArmorExResist
        aKi = EveSpace1.TargetShip.Ship.ArmorKiResist
        aTh = EveSpace1.TargetShip.Ship.ArmorThResist
        hEM = EveSpace1.TargetShip.Ship.StructureEMResist
        hEx = EveSpace1.TargetShip.Ship.StructureExResist
        hKi = EveSpace1.TargetShip.Ship.StructureKiResist
        hTh = EveSpace1.TargetShip.Ship.StructureThResist

        If mMod.Name <> "" And mMod.Name IsNot Nothing Then
            mor = CDbl(mMod.Attributes("54"))
            mvd = CDbl(mMod.Attributes("10018"))
            mdps = CDbl(mMod.Attributes("10019"))
            mrof = CDbl(mMod.Attributes("51"))
            missileER = CDbl(mMod.LoadedCharge.Attributes("654"))
            missileEV = CDbl(mMod.LoadedCharge.Attributes("653"))
            missileDRF = CDbl(mMod.LoadedCharge.Attributes("1353"))
            missileDRS = CDbl(mMod.LoadedCharge.Attributes("1354"))
            wEM = CDbl(mMod.LoadedCharge.Attributes("114"))
            wEx = CDbl(mMod.LoadedCharge.Attributes("116"))
            wKi = CDbl(mMod.LoadedCharge.Attributes("117"))
            wTh = CDbl(mMod.LoadedCharge.Attributes("118"))
            wT = wEM + wEx + wKi + wTh

            ' Calculate weapon damage split
            sdEM = wEM * (1 - sEM / 100) : sdEx = wEx * (1 - sEx / 100) : sdKi = wKi * (1 - sKi / 100) : sdTh = wTh * (1 - sTh / 100) : sdT = sdEM + sdEx + sdKi + sdTh
            adEM = wEM * (1 - aEM / 100) : adEx = wEx * (1 - aEx / 100) : adKi = wKi * (1 - aKi / 100) : adTh = wTh * (1 - aTh / 100) : adT = adEM + adEx + adKi + adTh
            hdEM = wEM * (1 - hEM / 100) : hdEx = wEx * (1 - hEx / 100) : hdKi = wKi * (1 - hKi / 100) : hdTh = wTh * (1 - hTh / 100) : hdT = hdEM + hdEx + hdKi + hdTh

            ' Calculate the actual damage
            If mor >= d Then
                CTH = wT * Math.Min(Math.Min(sr / missileER, 1), (missileEV / missileER * sr / targetVel) ^ (Math.Log(missileDRF) / Math.Log(missileDRS)))
            Else
                CTH = 0
            End If
            EDR = CTH / wT

            ' Calculate Expected Damage (DPS)
            esdEM = sdEM * EDR : esdEx = sdEx * EDR : esdKi = sdKi * EDR : esdTh = sdTh * EDR : esdT = sdT * EDR
            eadEM = adEM * EDR : eadEx = adEx * EDR : eadKi = adKi * EDR : eadTh = adTh * EDR : eadT = adT * EDR
            ehdEM = hdEM * EDR : ehdEx = hdEx * EDR : ehdKi = hdKi * EDR : ehdTh = hdTh * EDR : ehdT = hdT * EDR

            ' Calculate target recharge rates
            tsrr = CDbl(EveSpace1.TargetShip.Ship.Attributes("10065"))
            tarr = CDbl(EveSpace1.TargetShip.Ship.Attributes("10066"))
            thrr = CDbl(EveSpace1.TargetShip.Ship.Attributes("10067"))

            ' Calculate passive recharge rate
            Dim prr As Double = (EveSpace1.TargetShip.Ship.ShieldCapacity / EveSpace1.TargetShip.Ship.ShieldRecharge) / 5

            ' Calculate Expected Times (no target ship HP recharge)
            estNR = sHP / (esdT * mc) * mrof : eatNR = aHP / (eadT * mc) * mrof : ehtNR = hHP / (ehdT * mc) * mrof
            If estNR > 86400 Then
                estNR = 86400
            End If
            If eatNR > 86400 Then
                eatNR = 86400
            End If
            If ehtNR > 86400 Then
                ehtNR = 86400
            End If

            ' Calculate Expected Times (with target ship HP recharge)
            If ((esdT * mc) - tsrr) > 0 Then
                estR = sHP / ((esdT * mc) - tsrr) * mrof
            Else
                estR = 86400
            End If
            If ((eadT * mc) - tarr - prr) > 0 Then
                eatR = aHP / ((eadT * mc) - tarr - prr) * mrof
            Else
                eatR = 86400
            End If
            If ((ehdT * mc) - thrr - prr) > 0 Then
                ehtR = hHP / ((ehdT * mc) - thrr - prr) * mrof
            Else
                ehtR = 86400
            End If

            ' Write stats
            Dim stats As New StringBuilder
            stats.AppendLine("Stats: " & mMod.Name & " (" & mMod.LoadedCharge.Name & ")")
            stats.AppendLine("Range: " & EveSpace1.Range.ToString("N2") & " km")
            stats.AppendLine("Attacker Velocity: " & EveSpace1.SourceShip.Velocity.ToString("N2") & " m/s")
            stats.AppendLine("Target Velocity: " & EveSpace1.TargetShip.Velocity.ToString("N2") & " m/s")
            stats.AppendLine("Trans: " & EveSpace1.Transversal.ToString("N2") & " m/s")
            stats.AppendLine("Target Sig Radius: " & EveSpace1.TargetShip.Ship.SigRadius.ToString("N2") & " m")
            stats.AppendLine("Missile Count: " & mc.ToString("N0"))
            stats.AppendLine("Missile Ex Radius: " & missileER.ToString("N2") & " m")
            stats.AppendLine("Missile Ex Velocity: " & missileEV.ToString("N2") & " m/s")
            stats.AppendLine("Missile Range: " & mor.ToString("N0") & " m")
            stats.AppendLine("Missile ROF: " & mrof.ToString("N2") & " s")
            stats.AppendLine("Missile Volley: " & mvd.ToString("N2") & " HP")
            stats.AppendLine("Missile DPS: " & mdps.ToString("N2") & " HP/s")
            stats.AppendLine("Missile Damage (EM/Ex/Ki/Th): " & wEM.ToString("N2") & " / " & wEx.ToString("N2") & " / " & wKi.ToString("N2") & " / " & wTh.ToString("N2"))
            stats.AppendLine("Target HP (S/A/H): " & sHP.ToString("N2") & " / " & aHP.ToString("N2") & " / " & hHP.ToString("N2"))
            stats.AppendLine("Target Shield Res (EM/Ex/Ki/Th): " & sEM.ToString("N2") & " / " & sEx.ToString("N2") & " / " & sKi.ToString("N2") & " / " & sTh.ToString("N2"))
            stats.AppendLine("Target Armor Res (EM/Ex/Ki/Th): " & aEM.ToString("N2") & " / " & aEx.ToString("N2") & " / " & aKi.ToString("N2") & " / " & aTh.ToString("N2"))
            stats.AppendLine("Target Hull Res (EM/Ex/Ki/Th): " & hEM.ToString("N2") & " / " & hEx.ToString("N2") & " / " & hKi.ToString("N2") & " / " & hTh.ToString("N2"))
            stats.AppendLine("")
            stats.AppendLine("Expected Damage: " & CTH.ToString("N2") & " HP")
            stats.AppendLine("Expected Damage Ratio: " & EDR.ToString("N8"))
            stats.AppendLine("")
            stats.AppendLine("Theoretical Per Missile Damage (S/A/H): " & sdT.ToString("N2") & " / " & adT.ToString("N2") & " / " & hdT.ToString("N2"))
            stats.AppendLine("Theoretical Per Missile DPS (S/A/H): " & (sdT / mrof).ToString("N2") & " / " & (adT / mrof).ToString("N2") & " / " & (hdT / mrof).ToString("N2"))
            stats.AppendLine("Expected Per Missile Damage (S/A/H): " & esdT.ToString("N2") & " / " & eadT.ToString("N2") & " / " & ehdT.ToString("N2"))
            stats.AppendLine("Expected Per Missile DPS (S/A/H): " & (esdT / mrof).ToString("N2") & " / " & (eadT / mrof).ToString("N2") & " / " & (ehdT / mrof).ToString("N2"))
            stats.AppendLine("Theoretical Total Damage (S/A/H): " & (sdT * mc).ToString("N2") & " / " & (adT * mc).ToString("N2") & " / " & (hdT * mc).ToString("N2"))
            stats.AppendLine("Theoretical Total DPS (S/A/H): " & (sdT / mrof * mc).ToString("N2") & " / " & (adT / mrof * mc).ToString("N2") & " / " & (hdT / mrof * mc).ToString("N2"))
            stats.AppendLine("Expected Total Damage (S/A/H): " & (esdT * mc).ToString("N2") & " / " & (eadT * mc).ToString("N2") & " / " & (ehdT * mc).ToString("N2"))
            stats.AppendLine("Expected Total DPS (S/A/H): " & (esdT / mrof * mc).ToString("N2") & " / " & (eadT / mrof * mc).ToString("N2") & " / " & (ehdT / mrof * mc).ToString("N2"))
            stats.AppendLine("Target HP Recharge Rates (S/A/H): " & tsrr.ToString("N2") & " / " & tarr.ToString("N2") & " / " & thrr.ToString("N2"))
            stats.AppendLine("Depletion Times NR (S/A/H): " & CStr(IIf(estNR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(estNR))) & " / " & CStr(IIf(eatNR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(eatNR))) & " / " & CStr(IIf(ehtNR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(ehtNR))))
            stats.AppendLine("Depletion Times WR (S/A/H): " & CStr(IIf(estR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(estR))) & " / " & CStr(IIf(eatR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(eatR))) & " / " & CStr(IIf(ehtR >= 86400, "Stable", EveHQ.Core.SkillFunctions.TimeToString(ehtR))))
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

    Private Sub radMovableVel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radMovableVel.CheckedChanged
        iconVelocity = radMovableVel.Checked
        If EveSpace1.SourceShip IsNot Nothing Then
            EveSpace1.SourceShip.Velocity = nudAttackerVel.Value
        End If
        If EveSpace1.TargetShip IsNot Nothing Then
            EveSpace1.TargetShip.Velocity = nudTargetVel.Value
        End If
        EveSpace1.UseIntegratedVelocity = radMovableVel.Checked
    End Sub

    Private Sub nudAttackerVel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudAttackerVel.ValueChanged
        EveSpace1.AttackerVelocity = nudAttackerVel.Value
    End Sub

    Private Sub nudTargetVel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTargetVel.ValueChanged
        EveSpace1.TargetVelocity = nudTargetVel.Value
    End Sub
End Class