﻿Public Class DNAFitting
    Public ShipID As Integer
    Public Modules As New List(Of Integer)
    Public Charges As New List(Of Integer)
    Public Arguments As New SortedList(Of String, String)

    Public Shared Function GetFittingFromEveDNA(ByVal dna As String, fittingName As String) As Fitting

        ' fitting:29986:11648;1:3520;5:15810;3:11269;1:30119;1:29965;1:30173;1:2032;1:31456;1:31065;1:17559;1:30040;1:19033;1:12058;1:31035;1:11644;1:1978;1:30078;1::

        Dim shipDNA As New DNAFitting
        dna = dna.TrimStart("fitting:".ToCharArray).TrimEnd("::".ToCharArray)

        ' Remove any query string to analyse later
        Dim parts() As String = dna.Split("?".ToCharArray)
        Dim mods() As String = parts(0).Split(":".ToCharArray)

        shipDNA.ShipID = CInt(mods(0))
        For modNo As Integer = 1 To mods.Length - 1
            Dim modList As List(Of String) = mods(modNo).Split(";".ToCharArray).ToList
            If modList.Count > 0 Then
                For Each modID As String In modList
                    If ModuleLists.ModuleList.ContainsKey(CInt(modID)) = True Then
                        Dim fModule As ShipModule = ModuleLists.ModuleList(CInt(modID))
                        If fModule.IsCharge Then
                            shipDNA.Charges.Add(fModule.ID)
                        Else
                            shipDNA.Modules.Add(fModule.ID)
                        End If
                    End If
                Next
            End If
        Next

        If parts.Length > 1 Then
            Dim args() As String = parts(1).Split("&".ToCharArray)
            For Each arg As String In args
                Dim argData() As String = arg.Split("=".ToCharArray)
                shipDNA.Arguments.Add(argData(0), argData(1))
            Next
        End If

        Dim baseFit As String
        Dim revisedFit As String
        Dim currentFit As New ArrayList
        For Each fittedMod As Integer In shipDNA.Modules
            Dim fModule As ShipModule = ModuleLists.ModuleList(fittedMod)
            If fModule IsNot Nothing Then
                baseFit = fModule.Name : revisedFit = baseFit
                If fModule.Charges.Count <> 0 Then
                    For Each ammo As Integer In shipDNA.Charges
                        If ModuleLists.ModuleList.ContainsKey(ammo) = True Then
                            If fModule.Charges.Contains(ModuleLists.ModuleList(ammo).DatabaseGroup) Then
                                revisedFit = baseFit & "," & ModuleLists.ModuleList(ammo).Name
                            End If
                        End If
                    Next
                    currentFit.Add(revisedFit)
                Else
                    currentFit.Add(fModule.Name)
                End If
            End If
        Next

        Dim currentFitting As Fitting = Fittings.ConvertOldFitToNewFit(ShipLists.shipListKeyID(shipDNA.ShipID) & ", " & fittingName, currentFit)

        Return currentFitting

    End Function
End Class