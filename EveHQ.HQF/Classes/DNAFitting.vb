Imports System.Text

Public Class DNAFitting
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
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                For modCount As Integer = 1 To CInt(modList(1))
                    If ModuleLists.ModuleList.ContainsKey(CInt(modList(0))) = True Then
                        Dim fModule As ShipModule = ModuleLists.ModuleList(CInt(modList(0)))
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

        Dim currentFitting As Fitting = Fittings.ConvertOldFitToNewFit(ShipLists.ShipListKeyID(shipDNA.ShipID) & ", " & fittingName, currentFit)

        Return currentFitting

    End Function

    Public Shared Function GetDNAFromFitting(fitting As Fitting, returnURL As Boolean) As String

        Dim mods As New Dictionary(Of Integer, Integer) ' ModID, Quantity

        ' Update the base ship
        Dim newFit As Fitting = fitting.Clone
        newFit.UpdateBaseShipFromFitting()

        ' Add the hi slot modules
        For slot As Integer = 1 To newFit.BaseShip.HiSlots
            If newFit.BaseShip.HiSlot(slot) IsNot Nothing Then
                If mods.ContainsKey(newFit.BaseShip.HiSlot(slot).ID) Then
                    mods(newFit.BaseShip.HiSlot(slot).ID) += 1
                Else
                    mods.Add(newFit.BaseShip.HiSlot(slot).ID, 1)
                End If
            End If
        Next

        ' Add the mid slot modules
        For slot As Integer = 1 To newFit.BaseShip.MidSlots
            If newFit.BaseShip.MidSlot(slot) IsNot Nothing Then
                If mods.ContainsKey(newFit.BaseShip.MidSlot(slot).ID) Then
                    mods(newFit.BaseShip.MidSlot(slot).ID) += 1
                Else
                    mods.Add(newFit.BaseShip.MidSlot(slot).ID, 1)
                End If
            End If
        Next

        ' Add the low slot modules
        For slot As Integer = 1 To newFit.BaseShip.LowSlots
            If newFit.BaseShip.LowSlot(slot) IsNot Nothing Then
                If mods.ContainsKey(newFit.BaseShip.LowSlot(slot).ID) Then
                    mods(newFit.BaseShip.LowSlot(slot).ID) += 1
                Else
                    mods.Add(newFit.BaseShip.LowSlot(slot).ID, 1)
                End If
            End If
        Next

        ' Add the rig slot modules
        For slot As Integer = 1 To newFit.BaseShip.RigSlots
            If newFit.BaseShip.RigSlot(slot) IsNot Nothing Then
                If mods.ContainsKey(newFit.BaseShip.RigSlot(slot).ID) Then
                    mods(newFit.BaseShip.RigSlot(slot).ID) += 1
                Else
                    mods.Add(newFit.BaseShip.RigSlot(slot).ID, 1)
                End If
            End If
        Next

        ' Add the sub slot modules
        For slot As Integer = 1 To newFit.BaseShip.SubSlots
            If newFit.BaseShip.SubSlot(slot) IsNot Nothing Then
                If mods.ContainsKey(newFit.BaseShip.SubSlot(slot).ID) Then
                    mods(newFit.BaseShip.SubSlot(slot).ID) += 1
                Else
                    mods.Add(newFit.BaseShip.SubSlot(slot).ID, 1)
                End If
            End If
        Next

        ' Add drones
        For Each dbi As DroneBayItem In newFit.BaseShip.DroneBayItems.Values
            mods.Add(dbi.DroneType.ID, dbi.Quantity)
        Next

        ' Add charges
        For Each cbi As CargoBayItem In newFit.BaseShip.CargoBayItems.Values
            If ModuleLists.ModuleList.ContainsKey(cbi.ItemType.ID) Then
                If ModuleLists.ModuleList(cbi.ItemType.ID).IsCharge Then
                    mods.Add(cbi.ItemType.ID, cbi.Quantity)
                End If
            End If
        Next

        Dim dna As New StringBuilder
        If returnURL = True Then
            dna.Append("<url=fitting:")
        End If
        ' Add the ship ID
        dna.Append(CStr(ShipLists.ShipListKeyName(newFit.ShipName)) & ":")
        For Each modID As Integer In mods.Keys
            dna.Append(CStr(modID) & ";" & CStr(mods(modID)) & ":")
        Next
        dna.Append(":")
        If returnURL = True Then
            dna.Append(">" & newFit.FittingName & "</url>")
        End If
        Return dna.ToString

    End Function
End Class