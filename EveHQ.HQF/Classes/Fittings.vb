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
<Serializable()> Public Class Fittings

    Public Shared FittingList As New SortedList(Of String, Fitting)  ' Key = [ShipName,Fitting], Value = Fitting Class

#Region "Format Conversion Routines"

    Public Shared Function ConvertOldFitToNewFit(ByVal FitName As String, ByVal Fit As ArrayList) As Fitting

        Dim fittingSep As Integer = FitName.IndexOf(", ")
        Dim shipName As String = FitName.Substring(0, fittingSep)
        Dim fittingName As String = FitName.Substring(fittingSep + 2)
        Dim tempShip As Ship = CType(ShipLists.shipList(shipName), Ship)

        Dim NewFit As New Fitting(shipName, fittingName)
        NewFit.ShipName = shipName
        NewFit.FittingName = fittingName

        For Each Entry As String In Fit

            ' Check if this is a pilot entry
            If Entry.StartsWith("#Pilot#") = True Then
                NewFit.PilotName = Entry.Remove(0, 7)
            End If

            ' Check for a booster entry
            If Entry.StartsWith("#Booster#") = True Then
                NewFit.Boosters.Add(New ModuleWithState(ModuleLists.moduleListName(Entry.Remove(0, 9)).ToString, Nothing, ModuleStates.Active))
            End If

            ' Check for installed charges
            Dim modData() As String = Entry.Split(",".ToCharArray)
            Dim state As Integer = 4
            Dim itemQuantity As Integer = 1
            If modData(0).Length > 2 Then
                If modData(0).Substring(modData(0).Length - 2, 1) = "_" Then
                    state = CInt(modData(0).Substring(modData(0).Length - 1, 1))
                    modData(0) = modData(0).TrimEnd(("_" & state.ToString).ToCharArray)
                    state = CInt(Math.Pow(2, state))
                End If
            End If

            ' Check for stacks of cargo/drones
            If modData.GetUpperBound(0) = 0 Then
                modData = modData(0).Split({" x"}, StringSplitOptions.RemoveEmptyEntries)
            End If

            ' Check if the module exists
            If ModuleLists.moduleListName.ContainsKey(modData(0)) = True Then
                Dim modID As String = ModuleLists.moduleListName(modData(0)).ToString
                Dim sMod As ShipModule = ModuleLists.ModuleList(modID).Clone
                If modData.GetUpperBound(0) > 0 Then
                    ' Check if a charge (will be a valid item)
                    If ModuleLists.ModuleListName.ContainsKey(modData(1).Trim) = True Then
                        Dim chgID As String = ModuleLists.ModuleListName(modData(1).Trim).ToString
                        sMod.LoadedCharge = ModuleLists.ModuleList(chgID).Clone
                    End If
                End If
                ' Check if module is nothing
                If sMod IsNot Nothing Then
                    ' Check if module is a drone
                    If sMod.IsDrone = True Then
                        Dim active As Boolean = False
                        If modData.GetUpperBound(0) > 0 Then
                            If modData(1).EndsWith("a") = True Then
                                active = True
                                itemQuantity = CInt(modData(1).Substring(0, Len(modData(1)) - 1))
                            Else
                                If modData(1).EndsWith("i") = True Then
                                    itemQuantity = CInt(modData(1).Substring(0, Len(modData(1)) - 1))
                                Else
                                    itemQuantity = CInt(modData(1))
                                End If
                            End If
                        End If
                        If active = True Then
                            NewFit.Drones.Add(New ModuleQWithState(sMod.ID, ModuleStates.Active, itemQuantity))
                        Else
                            NewFit.Drones.Add(New ModuleQWithState(sMod.ID, ModuleStates.Inactive, itemQuantity))
                        End If
                    Else
                        ' Check if module is a charge
                        If sMod.IsCharge = True Or sMod.IsContainer Then
                            If modData.GetUpperBound(0) > 0 Then
                                itemQuantity = CInt(modData(1))
                            End If
                            NewFit.Items.Add(New ModuleQWithState(sMod.ID, ModuleStates.Active, itemQuantity))
                        Else
                            ' Must be a proper module then!
                            sMod.ModuleState = CType(state, ModuleStates)
                            If sMod.LoadedCharge Is Nothing Then
                                NewFit.Modules.Add(New ModuleWithState(sMod.ID, Nothing, CType(sMod.ModuleState, ModuleStates)))
                            Else
                                NewFit.Modules.Add(New ModuleWithState(sMod.ID, sMod.LoadedCharge.ID, CType(sMod.ModuleState, ModuleStates)))
                            End If
                        End If
                    End If
                Else
                    ' Unrecognised module - ignore
                End If
            Else
                ' Check if this is a ship from the maintenance bay
                If ShipLists.shipList.ContainsKey(modData(0)) Then
                    Dim sShip As Ship = CType(ShipLists.shipList(modData(0)), Ship).Clone
                    If modData.GetUpperBound(0) > 0 Then
                        itemQuantity = CInt(modData(1))
                    End If
                    NewFit.Ships.Add(New ModuleQWithState(sShip.ID, ModuleStates.Active, itemQuantity))
                Else
                    ' Check if this is a rig i.e. try putting large in front of it!
                    Dim testRig As String = "Large " & modData(0)
                    If ModuleLists.moduleListName.ContainsKey(testRig) = True Then
                        Dim modID As String = ModuleLists.moduleListName(testRig).ToString
                        Dim sMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                        If sMod.SlotType = 1 Then ' i.e. rig
                            If CInt(sMod.Attributes("1547")) = CInt(tempShip.Attributes("1547")) Then
                                sMod.ModuleState = CType(state, ModuleStates)
                                NewFit.Modules.Add(New ModuleWithState(sMod.ID, Nothing, CType(sMod.ModuleState, ModuleStates)))
                            Else
                                Select Case CInt(tempShip.Attributes("1547"))
                                    Case 1
                                        testRig = "Small " & modData(0)
                                    Case 2
                                        testRig = "Medium " & modData(0)
                                    Case 3
                                        testRig = "Large " & modData(0)
                                    Case 4
                                        testRig = "Capital " & modData(0)
                                End Select
                                modID = ModuleLists.moduleListName(testRig).ToString
                                sMod = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                                sMod.ModuleState = CType(state, ModuleStates)
                                NewFit.Modules.Add(New ModuleWithState(sMod.ID, Nothing, CType(sMod.ModuleState, ModuleStates)))
                            End If
                        End If
                    Else
                        ' Not a valid module - ignore
                    End If
                End If
            End If
        Next

        ' Check the pilot
        If NewFit.PilotName = "" Then
            ' Select a default pilot
            NewFit.PilotName = HQF.Settings.HQFSettings.DefaultPilot
        End If

        Return NewFit

    End Function

#End Region

End Class

Public Class DNAFitting
    Public ShipID As String
    Public Modules As New ArrayList
    Public Charges As New ArrayList
    Public Arguments As New SortedList(Of String, String)
End Class
