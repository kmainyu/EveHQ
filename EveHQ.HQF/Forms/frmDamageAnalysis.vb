Imports System.Windows.Forms
Imports System.Text
Imports System.Drawing
Imports ZedGraph

Public Class frmDamageAnalysis

    Dim attackingShip, targetShip As Ship
    Dim aMod As New ShipModule
    Dim sr As Double = 0
    Dim tv As Double = 0
    Dim d As Double = 0
    Dim tsr As Double = 0
    Dim tt As Double = 0
    Dim tor As Double = 0
    Dim tf As Double = 0
    Dim tvd As Double = 0
    Dim tdps As Double = 0
    Dim tc As Integer = 0
    Dim trof As Double = 0
    Dim sHP, aHP, hHP As Double
    Dim sEM, sEx, sKi, sTh As Double
    Dim aEM, aEx, aKi, aTh As Double
    Dim hEM, hEx, hKi, hTh As Double
    Dim wEM, wEx, wKi, wTh As Double
    Dim sdEM, sdEx, sdKi, sdTh, sdT As Double
    Dim adEM, adEx, adKi, adTh, adT As Double
    Dim hdEM, hdEx, hdKi, hdTh, hdT As Double
    Dim esdEM, esdEx, esdKi, esdTh, esdT As Double
    Dim eadEM, eadEx, eadKi, eadTh, eadT As Double
    Dim ehdEM, ehdEx, ehdKi, ehdTh, ehdT As Double
    Dim est, eat, eht As Double
    Dim CTH As Double = 0
    Dim EDR As Double = 0
    Dim GraphForm As New frmChartViewer

#Region "Form Loading Routines"

    Private Sub frmDamageAnalysis_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.LoadFittingsAndPilots()
        EveSpace1.RangeScale = 0.5
        EveSpace1.VelocityScale = 2
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
        ' Add the pilots
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            cboAttackerPilot.Items.Add(cPilot.Name)
            cboTargetPilot.Items.Add(cPilot.Name)
        Next
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
            Dim fittingSep As Integer = shipFit.IndexOf(", ")
            Dim shipName As String = shipFit.Substring(0, fittingSep)
            Dim fittingName As String = shipFit.Substring(fittingSep + 2)
            Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboAttackerPilot.SelectedItem.ToString), HQFPilot)
            Dim aShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            aShip = Engine.UpdateShipDataFromFittingList(aShip, CType(Fittings.FittingList(shipFit), ArrayList))
            aShip.DamageProfile = CType(DamageProfiles.ProfileList("<Omni-Damage>"), DamageProfile)
            attackingShip = Engine.ApplyFitting(aShip, aPilot)
            If EveSpace1.SourceShip Is Nothing Then
                EveSpace1.SourceShip = New ShipStatus("SourceShip", attackingShip, New Point(100, 100), New Point(300, 300))
            Else
                EveSpace1.SourceShip = New ShipStatus("SourceShip", attackingShip, EveSpace1.SourceShip.Location, EveSpace1.SourceShip.Heading)
            End If
            ' Check hislots of attacker to find turret data
            Dim sMod As New ShipModule
            aMod = New ShipModule
            Dim MixedTurrets As Boolean = False
            Dim MixedAmmo As Boolean = False
            tc = 0
            For slot As Integer = 1 To EveSpace1.SourceShip.Ship.HiSlots
                If EveSpace1.SourceShip.Ship.HiSlot(slot) IsNot Nothing Then
                    sMod = EveSpace1.SourceShip.Ship.HiSlot(slot)
                    If sMod.IsTurret Then
                        If aMod.Name = "" Then
                            If sMod.LoadedCharge.Name <> "" Then
                                aMod = sMod
                                tc += 1
                            End If
                        Else
                            ' Check if we match
                            If sMod.Name <> aMod.Name Then
                                MixedTurrets = True
                            Else
                                If sMod.LoadedCharge.Name <> aMod.LoadedCharge.Name Then
                                    MixedAmmo = True
                                Else
                                    tc += 1
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            ' Check for mixed turrets and ammo
            If MixedTurrets = True Then
                Dim msg As String = "HQF has detected that you are using a setup with multiple turret types. As such, only the first turret (and identical instances thereof) will be used for the calculations."
                MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If MixedAmmo = True Then
                    Dim msg As String = "HQF has detected that you are using a setup with varying ammo types. As such, only the first turret (and identical instances thereof) will be used for the calculations."
                    MessageBox.Show(msg, "Don't Mix Guns!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

    Private Sub SetTargetShip()
        If cboTargetFitting.SelectedItem IsNot Nothing And cboTargetPilot.SelectedItem IsNot Nothing Then
            Dim shipFit As String = cboTargetFitting.SelectedItem.ToString
            Dim fittingSep As Integer = shipFit.IndexOf(", ")
            Dim shipName As String = shipFit.Substring(0, fittingSep)
            Dim fittingName As String = shipFit.Substring(fittingSep + 2)
            Dim aPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboTargetPilot.SelectedItem.ToString), HQFPilot)
            Dim aShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            aShip = Engine.UpdateShipDataFromFittingList(aShip, CType(Fittings.FittingList(shipFit), ArrayList))
            aShip.DamageProfile = CType(DamageProfiles.ProfileList("<Omni-Damage>"), DamageProfile)
            targetShip = Engine.ApplyFitting(aShip, aPilot)
            If EveSpace1.TargetShip Is Nothing Then
                EveSpace1.TargetShip = New ShipStatus("TargetShip", targetShip, New Point(250, 250), New Point(150, 150))
            Else
                EveSpace1.TargetShip = New ShipStatus("TargetShip", targetShip, EveSpace1.TargetShip.Location, EveSpace1.TargetShip.Heading)
            End If
        End If
    End Sub

#End Region

    Private Sub UpdateGraph() Handles EveSpace1.GraphUpdateRequired
        ' Update the Graph?
        If GraphForm.IsHandleCreated = True Then
            Call Me.UpdateCurves()
        End If
    End Sub

    Private Sub UpdateStats() Handles EveSpace1.CalculationsChanged
        ' Set variables
        sr = EveSpace1.TargetShip.Ship.SigRadius
        tv = EveSpace1.Transversal
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

        If aMod.Name <> "" Then
            tsr = CDbl(aMod.Attributes("620"))
            tt = CDbl(aMod.Attributes("160"))
            tor = CDbl(aMod.Attributes("54"))
            tf = CDbl(aMod.Attributes("158"))
            tvd = CDbl(aMod.Attributes("10018"))
            tdps = CDbl(aMod.Attributes("10019"))
            trof = CDbl(aMod.Attributes("10011")) + CDbl(aMod.Attributes("10012")) + CDbl(aMod.Attributes("10013"))
            wEM = CDbl(aMod.Attributes("10051"))
            wEx = CDbl(aMod.Attributes("10052"))
            wKi = CDbl(aMod.Attributes("10053"))
            wTh = CDbl(aMod.Attributes("10054"))
        End If

        ' Calculate weapon damage split
        sdEM = wEM * (1 - sEM / 100) : sdEx = wEx * (1 - sEx / 100) : sdKi = wKi * (1 - sKi / 100) : sdTh = wTh * (1 - sTh / 100) : sdT = sdEM + sdEx + sdKi + sdTh
        adEM = wEM * (1 - aEM / 100) : adEx = wEx * (1 - aEx / 100) : adKi = wKi * (1 - aKi / 100) : adTh = wTh * (1 - aTh / 100) : adT = adEM + adEx + adKi + adTh
        hdEM = wEM * (1 - hEM / 100) : hdEx = wEx * (1 - hEx / 100) : hdKi = wKi * (1 - hKi / 100) : hdTh = wTh * (1 - hTh / 100) : hdT = hdEM + hdEx + hdKi + hdTh

        'ChanceToHit = 0.5 ^ ((((Transversal speed/(Range to target * Turret Tracking))*(Turret Signature Resolution / Target Signature Radius))^2) + ((max(0, Range to target - Turret Optimal Range))/Turret Falloff)^2) 
        CTH = (0.5 ^ ((((tv / (d * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, d - tor)) / tf) ^ 2)) * 100

        ' Calculate expected damage ratio
        EDR = 0.01 * 3 + ((CTH / 100) - 0.01) * ((CTH / 100) + 0.99) / 2

        ' Calculate Expected Damage
        esdEM = sdEM * EDR : esdEx = sdEx * EDR : esdKi = sdKi * EDR : esdTh = sdTh * EDR : esdT = sdT * EDR
        eadEM = adEM * EDR : eadEx = adEx * EDR : eadKi = adKi * EDR : eadTh = adTh * EDR : eadT = adT * EDR
        ehdEM = hdEM * EDR : ehdEx = hdEx * EDR : ehdKi = hdKi * EDR : ehdTh = hdTh * EDR : ehdT = hdT * EDR

        ' Calculate Expected Times (no target ship HP recharge)
        est = sHP / (esdT * tc) * trof : eat = aHP / (eadT * tc) * trof : eht = hHP / (ehdT * tc) * trof

        ' Write stats
        Dim stats As New StringBuilder
        stats.AppendLine("Stats:")
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
        stats.AppendLine("Turret Damage: " & wEM.ToString("N2") & " / " & wEx.ToString("N2") & " / " & wKi.ToString("N2") & " / " & wTh.ToString("N2"))
        stats.AppendLine("Target HP: " & sHP.ToString("N2") & " / " & aHP.ToString("N2") & " / " & hHP.ToString("N2"))
        stats.AppendLine("Target Shield Res: " & sEM.ToString("N2") & " / " & sEx.ToString("N2") & " / " & sKi.ToString("N2") & " / " & sTh.ToString("N2"))
        stats.AppendLine("Target Armor Res: " & aEM.ToString("N2") & " / " & aEx.ToString("N2") & " / " & aKi.ToString("N2") & " / " & aTh.ToString("N2"))
        stats.AppendLine("Target Hull Res: " & hEM.ToString("N2") & " / " & hEx.ToString("N2") & " / " & hKi.ToString("N2") & " / " & hTh.ToString("N2"))
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
        stats.AppendLine("Depletion Times (S/A/H): " & est.ToString("N2") & " / " & eat.ToString("N2") & " / " & eht.ToString("N2"))
        lblStats.Text = stats.ToString
        lblStats.Refresh()
    End Sub

    Private Sub nudRange_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRange.ValueChanged
        EveSpace1.RangeScale = nudRange.Value
    End Sub

    Private Sub nudVel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudVel.ValueChanged
        EveSpace1.VelocityScale = nudVel.Value
    End Sub

    Private Sub btnRangeVSHitChance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRangeVSHitChance.Click
        ' Prepare a new graph to display
        If GraphForm.IsHandleCreated = False Then
            GraphForm = New frmChartViewer
            Dim myPane As GraphPane = GraphForm.ZGC1.GraphPane

            ' Set the titles and axis labels
            myPane.Title.Text = "Range vs Hit Chance - " & EveSpace1.SourceShip.Ship.Name
            myPane.XAxis.Title.Text = "Range (m)"
            myPane.XAxis.Scale.Min = 0
            myPane.XAxis.Scale.Max = 3 * tor
            myPane.XAxis.Scale.MajorStep = tor
            myPane.XAxis.MajorGrid.IsVisible = True
            myPane.YAxis.Title.Text = "Hit Chance (%)"
            myPane.YAxis.Scale.Min = 0
            myPane.YAxis.Scale.Max = 110
            myPane.YAxis.Scale.MajorStep = 20
            myPane.YAxis.MajorGrid.IsVisible = True

            ' Create some points from 0 to 3*tor 
            Dim listR As New PointPairList()
            Dim listT As New PointPairList()
            Dim listC As New PointPairList
            Dim x As Double, y As Double

            For x = 0 To 3 * tor Step 1000 ' Range
                y = (0.5 ^ (((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Range Element Only
                listR.Add(x, y)
                y = (0.5 ^ ((((tv / (x * tt)) * (tsr / sr)) ^ 2))) * 100 ' Tracking Element Only
                listT.Add(x, y)
                y = (0.5 ^ ((((tv / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
                listC.Add(x, y)
            Next
            Dim bestRange As Double = 0
            Dim bestChance As Double = 0
            For x = 0 To 3 * tor
                y = (0.5 ^ ((((tv / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
                If y > bestChance Then
                    bestChance = y
                    bestRange = x
                End If
            Next

            ' Generate curves
            Dim myCurveC As LineItem = myPane.AddCurve("Combined Chance", listC, Color.Green, SymbolType.XCross)
            Dim myCurveR As LineItem = myPane.AddCurve("Range Element", listR, Color.Magenta, SymbolType.XCross)
            Dim myCurveT As LineItem = myPane.AddCurve("Tracking Element", listT, Color.Blue, SymbolType.XCross)
            myCurveR.Line.IsAntiAlias = True
            myCurveT.Line.IsAntiAlias = True
            myCurveC.Line.IsAntiAlias = True

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
            info.AppendLine("Current chance to hit = " & FormatNumber(CTH, 2) & "% @ " & FormatNumber(d, 0) & "m")
            info.AppendLine("Highest chance to hit = " & FormatNumber(bestChance, 2) & "% @ " & FormatNumber(bestRange, 0) & "m")
            GraphForm.lblGraphInfo.Text = info.ToString

            GraphForm.Show()
        Else
            GraphForm.BringToFront()
            GraphForm.Visible = True
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
            y = (0.5 ^ ((((tv / (x * tt)) * (tsr / sr)) ^ 2))) * 100 ' Tracking Element Only
            listT.Add(x, y)
            y = (0.5 ^ ((((tv / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
            listC.Add(x, y)
        Next
        Dim bestRange As Double = 0
        Dim bestChance As Double = 0
        For x = 0 To 3 * tor
            y = (0.5 ^ ((((tv / (x * tt)) * (tsr / sr)) ^ 2) + ((Math.Max(0, x - tor)) / tf) ^ 2)) * 100 ' Combined
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
        info.AppendLine("Current chance to hit = " & FormatNumber(CTH, 2) & "% @ " & FormatNumber(d, 0) & "m")
        info.AppendLine("Highest chance to hit = " & FormatNumber(bestChance, 2) & "% @ " & FormatNumber(bestRange, 0) & "m")
        GraphForm.lblGraphInfo.Text = info.ToString


    End Sub
End Class