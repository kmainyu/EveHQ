﻿' ========================================================================
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

    Public Shared Function ConvertOldFitToNewFit(ByVal fitName As String, ByVal fit As ArrayList) As Fitting

        Dim fittingSep As Integer = fitName.IndexOf(", ", StringComparison.Ordinal)
        Dim shipName As String = fitName.Substring(0, fittingSep)
        Dim fittingName As String = fitName.Substring(fittingSep + 2)
        Dim tempShip As Ship = ShipLists.ShipList(shipName)

        Dim newFit As New Fitting(shipName, fittingName)
        newFit.ShipName = shipName
        newFit.FittingName = fittingName

        For Each entry As String In fit

            ' Check if this is a pilot entry
            If entry.StartsWith("#Pilot#") = True Then
                newFit.PilotName = entry.Remove(0, 7)
            End If

            ' Check for a booster entry
            If entry.StartsWith("#Booster#") = True Then
                newFit.Boosters.Add(New ModuleWithState(ModuleLists.ModuleListName(entry.Remove(0, 9)).ToString, Nothing, ModuleStates.Active))
            End If

            ' Check for installed charges
            Dim modData() As String = entry.Split(",".ToCharArray)
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
            If ModuleLists.ModuleListName.ContainsKey(modData(0)) = True Then
                Dim modID As String = ModuleLists.ModuleListName(modData(0)).ToString
                Dim sMod As ShipModule = ModuleLists.ModuleList(CInt(modID)).Clone
                If modData.GetUpperBound(0) > 0 Then
                    ' Check if a charge (will be a valid item)
                    If ModuleLists.ModuleListName.ContainsKey(modData(1).Trim) = True Then
                        Dim chgID As String = ModuleLists.ModuleListName(modData(1).Trim).ToString
                        sMod.LoadedCharge = ModuleLists.ModuleList(CInt(chgID)).Clone
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
                            newFit.Drones.Add(New ModuleQWithState(CStr(sMod.ID), ModuleStates.Active, itemQuantity))
                        Else
                            newFit.Drones.Add(New ModuleQWithState(CStr(sMod.ID), ModuleStates.Inactive, itemQuantity))
                        End If
                    Else
                        ' Check if module is a charge
                        If sMod.IsCharge = True Or sMod.IsContainer Then
                            If modData.GetUpperBound(0) > 0 Then
                                itemQuantity = CInt(modData(1))
                            End If
                            newFit.Items.Add(New ModuleQWithState(CStr(sMod.ID), ModuleStates.Active, itemQuantity))
                        Else
                            ' Must be a proper module then!
                            sMod.ModuleState = CType(state, ModuleStates)
                            If sMod.LoadedCharge Is Nothing Then
                                newFit.Modules.Add(New ModuleWithState(CStr(sMod.ID), Nothing, sMod.ModuleState))
                            Else
                                newFit.Modules.Add(New ModuleWithState(CStr(sMod.ID), CStr(sMod.LoadedCharge.ID), sMod.ModuleState))
                            End If
                        End If
                    End If
                Else
                    ' Unrecognised module - ignore
                End If
            Else
                ' Check if this is a ship from the maintenance bay
                If ShipLists.ShipList.ContainsKey(modData(0)) Then
                    Dim sShip As Ship = ShipLists.ShipList(modData(0)).Clone
                    If modData.GetUpperBound(0) > 0 Then
                        itemQuantity = CInt(modData(1))
                    End If
                    newFit.Ships.Add(New ModuleQWithState(CStr(sShip.ID), ModuleStates.Active, itemQuantity))
                Else
                    ' Check if this is a rig i.e. try putting large in front of it!
                    Dim testRig As String = "Large " & modData(0)
                    If ModuleLists.ModuleListName.ContainsKey(testRig) = True Then
                        Dim modID As String = ModuleLists.ModuleListName(testRig).ToString
                        Dim sMod As ShipModule = ModuleLists.ModuleList(CInt(modID)).Clone
                        If sMod.SlotType = 1 Then ' i.e. rig
                            If CInt(sMod.Attributes(1547)) = CInt(tempShip.Attributes(1547)) Then
                                sMod.ModuleState = CType(state, ModuleStates)
                                newFit.Modules.Add(New ModuleWithState(CStr(sMod.ID), Nothing, sMod.ModuleState))
                            Else
                                Select Case CInt(tempShip.Attributes(1547))
                                    Case 1
                                        testRig = "Small " & modData(0)
                                    Case 2
                                        testRig = "Medium " & modData(0)
                                    Case 3
                                        testRig = "Large " & modData(0)
                                    Case 4
                                        testRig = "Capital " & modData(0)
                                End Select
                                modID = ModuleLists.ModuleListName(testRig).ToString
                                sMod = ModuleLists.ModuleList(CInt(modID)).Clone
                                sMod.ModuleState = CType(state, ModuleStates)
                                newFit.Modules.Add(New ModuleWithState(CStr(sMod.ID), Nothing, sMod.ModuleState))
                            End If
                        End If
                    Else
                        ' Not a valid module - ignore
                    End If
                End If
            End If
        Next

        ' Check the pilot
        If newFit.PilotName = "" Then
            ' Select a default pilot
            newFit.PilotName = Settings.HQFSettings.DefaultPilot
        End If

        Return newFit

    End Function

#End Region

End Class
