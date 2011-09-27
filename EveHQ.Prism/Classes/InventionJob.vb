<Serializable()> Public Class InventionJob

    ' Invention specific items
    Public InventedBPID As Integer
    Public BaseChance As Double
    Public DecryptorUsed As Decryptor
    Public MetaItemID As Integer
    Public MetaItemLevel As Integer
    Public OverrideBPCRuns As Boolean
    Public BPCRuns As Integer
    Public OverrideEncSkill As Boolean
    Public OverrideDCSkill1 As Boolean
    Public OverrideDCSkill2 As Boolean
    Public EncryptionSkill As Integer
    Public DatacoreSkill1 As Integer
    Public DatacoreSkill2 As Integer
    Public ProductionJob As ProductionJob ' Specific manufacturing info for the invention

    Public Function GetBaseBP() As Blueprint
        Return PlugInData.Blueprints(PlugInData.Blueprints(Me.InventedBPID.ToString).InventFrom(0))
    End Function

    Public Function CalculateInventionChance() As Double
        Dim DecryptorMod As Double = 1
        If DecryptorUsed IsNot Nothing Then
            DecryptorMod = DecryptorUsed.ProbMod
        End If
        Return Me.BaseChance * (1 + (0.01 * Me.EncryptionSkill)) * (1 + ((Me.DatacoreSkill1 + Me.DatacoreSkill2) * (0.1 / (5 - Me.MetaItemLevel)))) * DecryptorMod
    End Function

    Public Function CalculateInventionCost() As InventionCost

        Dim InvCost As New InventionCost
        Dim TotalCost As Double = 0

        ' Get base item BP for this invention
        Dim BaseBP As Blueprint = GetBaseBP()

        ' Calculate Datacore costs
        For Each resource As BlueprintResource In BaseBP.Resources.Values
            If resource.Activity = 8 Then
                ' Only include datacores
                If resource.TypeGroup = 333 Then
                    InvCost.DatacoreCost += (EveHQ.Core.DataFunctions.GetPrice(resource.TypeID.ToString) * resource.Quantity)
                End If
            End If
        Next

        ' Calculate Item cost
        If MetaItemID <> 0 Then
            InvCost.MetaItemCost = EveHQ.Core.DataFunctions.GetPrice(MetaItemID.ToString)
        End If

        ' Calculate Decryptor cost
        If DecryptorUsed IsNot Nothing Then
            If DecryptorUsed.ID <> "" Then
                InvCost.DecryptorCost = EveHQ.Core.DataFunctions.GetPrice(DecryptorUsed.ID)
            End If
        End If

        ' Calculate lab cost
        InvCost.LabCost = Settings.PrismSettings.LabInstallCost
        InvCost.LabCost += Math.Round(Settings.PrismSettings.LabRunningCost * (BaseBP.ResearchTechTime / 3600), 2)

        ' Calculate BPC cost
        InvCost.BPCCost = CalculateBPCCost()

        InvCost.TotalCost = InvCost.DatacoreCost + InvCost.MetaItemCost + InvCost.DecryptorCost + InvCost.LabCost + InvCost.BPCCost

        Return InvCost
    End Function

    Public Function CalculateInventedBPC() As BlueprintSelection

        ' Get base item BP for this invention
        Dim BaseBP As Blueprint = GetBaseBP()

        Dim IBP As BlueprintSelection = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(InventedBPID.ToString))

        Dim IME As Integer = -4
        Dim IPE As Integer = -4
        Dim IRC As Integer = 10

        If DecryptorUsed IsNot Nothing Then
            If PlugInData.Decryptors.ContainsKey(DecryptorUsed.Name) Then
                Dim UseDecryptor As Decryptor = PlugInData.Decryptors(DecryptorUsed.Name)
                IME += UseDecryptor.MEMod
                IPE += UseDecryptor.PEMod
                IRC = Math.Min(Math.Max(CInt((BPCRuns / BaseBP.MaxProdLimit) * (IBP.MaxProdLimit / 10) + UseDecryptor.RunMod), 1), IBP.MaxProdLimit)
            Else
                IRC = Math.Min(Math.Max(CInt((BPCRuns / BaseBP.MaxProdLimit) * (IBP.MaxProdLimit / 10)), 1), IBP.MaxProdLimit)
            End If
        Else
            IRC = Math.Min(Math.Max(CInt((BPCRuns / BaseBP.MaxProdLimit) * (IBP.MaxProdLimit / 10)), 1), IBP.MaxProdLimit)
        End If

        IBP.MELevel = IME
        IBP.PELevel = IPE
        IBP.Runs = IRC

        Return IBP

    End Function

    Public Function CalculateBPCCost() As Double
        Dim BPCCost As Double = 0
        Dim BaseBP As Blueprint = GetBaseBP()
        If Settings.PrismSettings.BPCCosts.ContainsKey(BaseBP.ID.ToString) Then
            Dim pricerange As Double = Settings.PrismSettings.BPCCosts(BaseBP.ID.ToString).MaxRunCost - Settings.PrismSettings.BPCCosts(BaseBP.ID.ToString).MinRunCost
            Dim runrange As Integer = BaseBP.MaxProdLimit - 1
            If runrange = 0 Then
                BPCCost += Settings.PrismSettings.BPCCosts(BaseBP.ID.ToString).MinRunCost
            Else
                BPCCost += Settings.PrismSettings.BPCCosts(BaseBP.ID.ToString).MinRunCost + Math.Round((pricerange / runrange) * (BPCRuns - 1), 2)
            End If
        End If
        Return BPCCost
    End Function

End Class

Public Class InventionCost
    Public DatacoreCost As Double
    Public MetaItemCost As Double
    Public DecryptorCost As Double
    Public LabCost As Double
    Public BPCCost As Double
    Public TotalCost As Double
End Class
