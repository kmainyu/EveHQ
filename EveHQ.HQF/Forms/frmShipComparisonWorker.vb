Public Class frmShipComparisonWorker

    Dim pPilot As HQFPilot
    Dim pProfile As DamageProfile
    Dim pShipList As New SortedList
    Dim pShipInfo As New SortedList
    Dim WithEvents CompareWorker As New System.ComponentModel.BackgroundWorker

    Public Property Pilot() As HQFPilot
        Get
            Return pPilot
        End Get
        Set(ByVal value As HQFPilot)
            pPilot = value
        End Set
    End Property
    Public Property Profile() As DamageProfile
        Get
            Return pProfile
        End Get
        Set(ByVal value As DamageProfile)
            pProfile = value
        End Set
    End Property
    Public Property ShipList() As SortedList
        Get
            Return pShipList
        End Get
        Set(ByVal value As SortedList)
            pShipList = value
        End Set
    End Property
    Public Property ShipInfo() As SortedList
        Get
            Return pShipInfo
        End Get
        Set(ByVal value As SortedList)
            pShipInfo = value
        End Set
    End Property

    Public Sub GenerateShipData()
        For Each shipFit As String In pShipList.Keys
            ' Let's try and generate a fitting and get some damage info
            Dim fittingSep As Integer = shipFit.IndexOf(", ")
            Dim shipName As String = shipFit.Substring(0, fittingSep)
            Dim fittingName As String = shipFit.Substring(fittingSep + 2)
            Dim pShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            pShip = Engine.UpdateShipDataFromFittingList(pShip, CType(Fittings.FittingList(shipFit), ArrayList))
            pShip.DamageProfile = pProfile
            Dim profileShip As Ship = Engine.ApplyFitting(pShip, pPilot)
            ' Place details of the ship into the Ship Data class
            Dim newShip As New ShipData
            newShip.Ship = shipName
            newShip.Fitting = fittingName
            newShip.Modules = CopyForForums(profileShip)
            newShip.EHP = profileShip.EffectiveHP
            newShip.Tank = CDbl(profileShip.Attributes("10062"))
            newShip.Capacitor = Engine.CalculateCapStatistics(profileShip)
            If newShip.Capacitor > 0 Then
                newShip.Capacitor = newShip.Capacitor / profileShip.CapCapacity * 100
            End If
            newShip.Volley = CDbl(profileShip.Attributes("10028"))
            newShip.DPS = CDbl(profileShip.Attributes("10029"))
            newShip.SEM = profileShip.ShieldEMResist
            newShip.SEx = profileShip.ShieldExResist
            newShip.SKi = profileShip.ShieldKiResist
            newShip.STh = profileShip.ShieldThResist
            newShip.AEM = profileShip.ArmorEMResist
            newShip.AEx = profileShip.ArmorExResist
            newShip.AKi = profileShip.ArmorKiResist
            newShip.ATh = profileShip.ArmorThResist
            newShip.Speed = profileShip.MaxVelocity
            pShipInfo.Add(shipFit, newShip)
        Next
    End Sub

    Private Sub frmShipComparisonWorker_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Refresh()
        CompareWorker.RunWorkerAsync()
    End Sub

    Private Sub CompareWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles CompareWorker.DoWork
        Me.GenerateShipData()
    End Sub

    Private Sub CompareWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles CompareWorker.RunWorkerCompleted
        Me.Close()
    End Sub

    Private Function CopyForForums(ByVal currentShip As Ship) As String
        Dim slots As Dictionary(Of String, Integer)
        Dim slotList As New ArrayList
        Dim slotCount As Integer = 0
        Dim cModule As New ShipModule
        Dim state As Integer
        Dim fitting As New System.Text.StringBuilder

        slots = New Dictionary(Of String, Integer)
        For slot As Integer = 1 To currentship.HiSlots
            If currentship.HiSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.HiSlot(slot).ModuleState) / Math.Log(2))
                If currentship.HiSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.HiSlot(slot).Name & " (" & currentship.HiSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.HiSlot(slot).Name) = True Then
                        slotCount = slots(currentship.HiSlot(slot).Name)
                        slots(currentship.HiSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.HiSlot(slot).Name)
                        slots.Add(currentship.HiSlot(slot).Name, 1)
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
        For slot As Integer = 1 To currentship.MidSlots
            If currentship.MidSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.MidSlot(slot).ModuleState) / Math.Log(2))
                If currentship.MidSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.MidSlot(slot).Name & " (" & currentship.MidSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.MidSlot(slot).Name) = True Then
                        slotCount = slots(currentship.MidSlot(slot).Name)
                        slots(currentship.MidSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.MidSlot(slot).Name)
                        slots.Add(currentship.MidSlot(slot).Name, 1)
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
        For slot As Integer = 1 To currentship.LowSlots
            If currentship.LowSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.LowSlot(slot).ModuleState) / Math.Log(2))
                If currentship.LowSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.LowSlot(slot).Name & " (" & currentship.LowSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.LowSlot(slot).Name) = True Then
                        slotCount = slots(currentship.LowSlot(slot).Name)
                        slots(currentship.LowSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.LowSlot(slot).Name)
                        slots.Add(currentship.LowSlot(slot).Name, 1)
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
        For slot As Integer = 1 To currentship.RigSlots
            If currentship.RigSlot(slot) IsNot Nothing Then
                state = CInt(Math.Log(currentship.RigSlot(slot).ModuleState) / Math.Log(2))
                If currentship.RigSlot(slot).LoadedCharge IsNot Nothing Then
                    If slotList.Contains(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")") = True Then
                        ' Get the dictionary item
                        slotCount = slots(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")")
                        slots(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")") = slotCount + 1
                    Else
                        slotList.Add(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")")
                        slots.Add(currentship.RigSlot(slot).Name & " (" & currentship.RigSlot(slot).LoadedCharge.Name & ")", 1)
                    End If
                Else
                    If slotList.Contains(currentship.RigSlot(slot).Name) = True Then
                        slotCount = slots(currentship.RigSlot(slot).Name)
                        slots(currentship.RigSlot(slot).Name) = slotCount + 1
                    Else
                        slotList.Add(currentship.RigSlot(slot).Name)
                        slots.Add(currentship.RigSlot(slot).Name, 1)
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

        If currentship.DroneBayItems.Count > 0 Then
            fitting.AppendLine("")
            For Each drone As DroneBayItem In currentship.DroneBayItems.Values
                fitting.AppendLine(drone.Quantity & "x " & drone.DroneType.Name)
            Next
        End If

        If currentship.CargoBayItems.Count > 0 Then
            fitting.AppendLine("")
            For Each cargo As CargoBayItem In currentship.CargoBayItems.Values
                fitting.AppendLine(cargo.Quantity & "x " & cargo.ItemType.Name & " (cargo)")
            Next
        End If
        Return fitting.ToString
    End Function
End Class

