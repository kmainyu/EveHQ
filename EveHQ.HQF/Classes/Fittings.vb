'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

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
            newFit.PilotName = PluginSettings.HQFSettings.DefaultPilot
        End If

        Return newFit

    End Function

#End Region

End Class
