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
Imports System.ComponentModel
Imports System.Text

Namespace Forms

    Public Class FrmShipComparisonWorker

        Dim _pilot As FittingPilot
        Dim _profile As HQFDamageProfile
        Dim _shipList As New SortedList
        Dim _shipInfo As New SortedList
        Dim WithEvents _comparisonWorker As New BackgroundWorker

        Public Property Pilot() As FittingPilot
            Get
                Return _pilot
            End Get
            Set(ByVal value As FittingPilot)
                _pilot = value
            End Set
        End Property
        Public Property Profile() As HQFDamageProfile
            Get
                Return _profile
            End Get
            Set(ByVal value As HQFDamageProfile)
                _profile = value
            End Set
        End Property
        Public Property ShipList() As SortedList
            Get
                Return _shipList
            End Get
            Set(ByVal value As SortedList)
                _shipList = value
            End Set
        End Property
        Public Property ShipInfo() As SortedList
            Get
                Return _shipInfo
            End Get
            Set(ByVal value As SortedList)
                _shipInfo = value
            End Set
        End Property

        Public Sub GenerateShipData()
            For Each shipFit As String In _shipList.Keys
                ' Let's try and generate a fitting and get some damage info
                Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                newFit.UpdateBaseShipFromFitting()
                newFit.BaseShip.DamageProfile = _profile
                newFit.PilotName = _pilot.PilotName
                newFit.ApplyFitting(BuildType.BuildEverything)
                Dim profileShip As Ship = newFit.FittedShip

                ' Place details of the ship into the Ship Data class
                Dim newShip As New ShipData
                newShip.Ship = newFit.ShipName
                newShip.Fitting = newFit.FittingName
                newShip.Modules = CopyForForums(profileShip)
                newShip.EHP = profileShip.EffectiveHP
                newShip.Tank = CDbl(profileShip.Attributes(10062))
                Dim csr As CapSimResults = Capacitor.CalculateCapStatistics(profileShip, False)
                If csr.CapIsDrained = False Then
                    newShip.Capacitor = csr.MinimumCap / profileShip.CapCapacity * 100
                Else
                    newShip.Capacitor = -csr.TimeToDrain
                End If
                newShip.Volley = CDbl(profileShip.Attributes(10028))
                newShip.DPS = CDbl(profileShip.Attributes(10029))
                newShip.SEm = profileShip.ShieldEMResist
                newShip.SEx = profileShip.ShieldExResist
                newShip.SKi = profileShip.ShieldKiResist
                newShip.STh = profileShip.ShieldThResist
                newShip.AEm = profileShip.ArmorEMResist
                newShip.AEx = profileShip.ArmorExResist
                newShip.AKi = profileShip.ArmorKiResist
                newShip.ATh = profileShip.ArmorThResist
                newShip.Speed = profileShip.MaxVelocity
                _shipInfo.Add(shipFit, newShip)
            Next
        End Sub

        Private Sub frmShipComparisonWorker_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Refresh()
            _comparisonWorker.RunWorkerAsync()
        End Sub

        Private Sub CompareWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles _comparisonWorker.DoWork
            GenerateShipData()
        End Sub

        Private Sub CompareWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles _comparisonWorker.RunWorkerCompleted
            Close()
        End Sub

        Private Function CopyForForums(ByVal currentShip As Ship) As String
            Dim slots As Dictionary(Of String, Integer)
            Dim slotList As New ArrayList
            Dim slotCount As Integer
            Dim fitting As New StringBuilder

            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentShip.HiSlots
                If currentShip.HiSlot(slot) IsNot Nothing Then
                    If currentShip.HiSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentShip.HiSlot(slot).Name & " (" & currentShip.HiSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentShip.HiSlot(slot).Name & " (" & currentShip.HiSlot(slot).LoadedCharge.Name & ")")
                            slots(currentShip.HiSlot(slot).Name & " (" & currentShip.HiSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentShip.HiSlot(slot).Name & " (" & currentShip.HiSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentShip.HiSlot(slot).Name & " (" & currentShip.HiSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentShip.HiSlot(slot).Name) = True Then
                            slotCount = slots(currentShip.HiSlot(slot).Name)
                            slots(currentShip.HiSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentShip.HiSlot(slot).Name)
                            slots.Add(currentShip.HiSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If

            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentShip.MidSlots
                If currentShip.MidSlot(slot) IsNot Nothing Then
                    If currentShip.MidSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentShip.MidSlot(slot).Name & " (" & currentShip.MidSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentShip.MidSlot(slot).Name & " (" & currentShip.MidSlot(slot).LoadedCharge.Name & ")")
                            slots(currentShip.MidSlot(slot).Name & " (" & currentShip.MidSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentShip.MidSlot(slot).Name & " (" & currentShip.MidSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentShip.MidSlot(slot).Name & " (" & currentShip.MidSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentShip.MidSlot(slot).Name) = True Then
                            slotCount = slots(currentShip.MidSlot(slot).Name)
                            slots(currentShip.MidSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentShip.MidSlot(slot).Name)
                            slots.Add(currentShip.MidSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                fitting.AppendLine("")
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If

            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentShip.LowSlots
                If currentShip.LowSlot(slot) IsNot Nothing Then
                    If currentShip.LowSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentShip.LowSlot(slot).Name & " (" & currentShip.LowSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentShip.LowSlot(slot).Name & " (" & currentShip.LowSlot(slot).LoadedCharge.Name & ")")
                            slots(currentShip.LowSlot(slot).Name & " (" & currentShip.LowSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentShip.LowSlot(slot).Name & " (" & currentShip.LowSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentShip.LowSlot(slot).Name & " (" & currentShip.LowSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentShip.LowSlot(slot).Name) = True Then
                            slotCount = slots(currentShip.LowSlot(slot).Name)
                            slots(currentShip.LowSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentShip.LowSlot(slot).Name)
                            slots.Add(currentShip.LowSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                fitting.AppendLine("")
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If


            slots = New Dictionary(Of String, Integer)
            For slot As Integer = 1 To currentShip.RigSlots
                If currentShip.RigSlot(slot) IsNot Nothing Then
                    If currentShip.RigSlot(slot).LoadedCharge IsNot Nothing Then
                        If slotList.Contains(currentShip.RigSlot(slot).Name & " (" & currentShip.RigSlot(slot).LoadedCharge.Name & ")") = True Then
                            ' Get the dictionary item
                            slotCount = slots(currentShip.RigSlot(slot).Name & " (" & currentShip.RigSlot(slot).LoadedCharge.Name & ")")
                            slots(currentShip.RigSlot(slot).Name & " (" & currentShip.RigSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                        Else
                            slotList.Add(currentShip.RigSlot(slot).Name & " (" & currentShip.RigSlot(slot).LoadedCharge.Name & ")")
                            slots.Add(currentShip.RigSlot(slot).Name & " (" & currentShip.RigSlot(slot).LoadedCharge.Name & ")", 1)
                        End If
                    Else
                        If slotList.Contains(currentShip.RigSlot(slot).Name) = True Then
                            slotCount = slots(currentShip.RigSlot(slot).Name)
                            slots(currentShip.RigSlot(slot).Name) = slotCount + 1
                        Else
                            slotList.Add(currentShip.RigSlot(slot).Name)
                            slots.Add(currentShip.RigSlot(slot).Name, 1)
                        End If
                    End If
                End If
            Next
            If slots.Count > 0 Then
                fitting.AppendLine("")
                For Each cMod As String In slots.Keys
                    If CInt(slots(cMod)) > 1 Then
                        fitting.AppendLine(slots(cMod).ToString & "x " & cMod)
                    Else
                        fitting.AppendLine(cMod)
                    End If
                Next
            End If

            If currentShip.DroneBayItems.Count > 0 Then
                fitting.AppendLine("")
                For Each drone As DroneBayItem In currentShip.DroneBayItems.Values
                    fitting.AppendLine(drone.Quantity & "x " & drone.DroneType.Name)
                Next
            End If

            If currentShip.CargoBayItems.Count > 0 Then
                fitting.AppendLine("")
                For Each cargo As CargoBayItem In currentShip.CargoBayItems.Values
                    fitting.AppendLine(cargo.Quantity & "x " & cargo.ItemType.Name & " (cargo)")
                Next
            End If
            Return fitting.ToString
        End Function
    End Class
End NameSpace