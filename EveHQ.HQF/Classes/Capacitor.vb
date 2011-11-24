' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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

''' <summary>
''' Class for holding various capacitor related routines 
''' </summary>
''' <remarks></remarks>
Public Class Capacitor

    ''' <summary>
    ''' Method for providing capacitor results by performing a simulation on an instance of the Ship class
    ''' </summary>
    ''' <param name="baseShip">The Ship to be used for the calculations</param>
    ''' <param name="RecordCapEvents">A boolean value indicating whether events should be recorded in the results</param>
    ''' <returns>An instance of the CapSimResults class containing the simulation results</returns>
    ''' <remarks></remarks>
    Public Shared Function CalculateCapStatistics(ByVal baseShip As Ship, ByVal RecordCapEvents As Boolean) As CapSimResults
        Dim CapacitorCapacity As Double = baseShip.CapCapacity
        Dim Capacitor As Double = CapacitorCapacity
        Dim currentTime, nextTime As Double
        Dim RechargeRate As Double = baseShip.CapRecharge
        Dim capConstant As Double = (RechargeRate / 5.0)
        Dim maxTime As Double = 43200 ' 12 hour
        Dim minCap As Double = Capacitor
        Dim minCapTime As Double = 0

        ' Create a new instance of the cap results class
        Dim ShipCapResults As New CapSimResults(maxTime)

        ' Populate the module list
        Dim stagger As Double = 0
        Dim modCount As Integer = 0

        For Each slotMod As ShipModule In baseShip.SlotCollection
            If slotMod.CapUsage <> 0 Then
                Dim totalTime As Double = slotMod.ActivationTime
                If slotMod.Attributes.ContainsKey("10011") Then
                    totalTime += slotMod.Attributes("10011")
                End If
                If slotMod.Attributes.ContainsKey("10012") Then
                    totalTime += slotMod.Attributes("10012")
                End If
                If (slotMod.ModuleState Or 28) = 28 Then
                    ShipCapResults.Modules.Add(New CapacitorModule(slotMod.Name, slotMod.CapUsage, totalTime, True))
                Else
                    ShipCapResults.Modules.Add(New CapacitorModule(slotMod.Name, slotMod.CapUsage, totalTime, False))
                End If
            End If
        Next

        ' Reset Data
        ShipCapResults.Events.Clear()
        For Each cm As CapacitorModule In ShipCapResults.Modules
            cm.NextTime = 0
        Next

        ' Do the calculations
        Dim rate As Double = 0
        Dim Cap As Double = 0
        Dim OldCap As Double = CapacitorCapacity
        While ((Capacitor > 0.0) And (nextTime < maxTime))
            OldCap = Capacitor
            Capacitor = (((1.0 + ((Math.Sqrt((Capacitor / CapacitorCapacity)) - 1.0) * Math.Exp(((currentTime - nextTime) / capConstant)))) ^ 2) * CapacitorCapacity)
            If RecordCapEvents = True Then
                Cap = Capacitor - OldCap
                rate = Cap / (nextTime - currentTime)
                ShipCapResults.Events.Add(New CapacitorEvent(nextTime, "Recharge", OldCap, -Cap, rate))
            End If
            currentTime = nextTime
            nextTime = maxTime
            For Each cm As CapacitorModule In ShipCapResults.Modules
                If cm.IsActive = True Then
                    If cm.NextTime = currentTime Then
                        cm.NextTime += cm.CycleTime
                        If RecordCapEvents = True Then
                            ShipCapResults.Events.Add(New CapacitorEvent(currentTime, cm.Name, Capacitor, cm.CapAmount, rate))
                        End If
                        Capacitor -= cm.CapAmount
                        Capacitor = Math.Min(Capacitor, CapacitorCapacity)
                    End If
                    nextTime = Math.Min(nextTime, cm.NextTime)
                End If
            Next
            If Capacitor < minCap Then
                minCap = Capacitor
                minCapTime = currentTime
            End If
        End While

        ' Set the results
        If Capacitor > 0 Then
            ShipCapResults.CapIsDrained = False
            ShipCapResults.TimeToDrain = -1
            ShipCapResults.MinimumCap = Math.Min(minCap, CapacitorCapacity)
        Else
            ShipCapResults.CapIsDrained = True
            ShipCapResults.TimeToDrain = currentTime
            ShipCapResults.MinimumCap = 0
        End If

        ' Return the result
        Return ShipCapResults

    End Function

    ''' <summary>
    ''' Method for recalculating cap stats once a previous set of results has been obtained
    ''' </summary>
    ''' <param name="baseShip">The Ship to be used for the calculations</param>
    ''' <param name="RecordCapEvents">A boolean value indicating whether events should be recorded in the results</param>
    ''' <param name="ShipCapResults">A set of existing CapSimResults</param>
    ''' <remarks></remarks>
    Public Shared Sub RecalculateCapStatistics(ByVal baseShip As Ship, ByVal RecordCapEvents As Boolean, ByRef ShipCapResults As CapSimResults)
        Dim CapacitorCapacity As Double = baseShip.CapCapacity
        Dim Capacitor As Double = CapacitorCapacity
        Dim currentTime, nextTime As Double
        Dim RechargeRate As Double = baseShip.CapRecharge
        Dim capConstant As Double = (RechargeRate / 5.0)
        Dim maxTime As Double = 43200 ' 12 hour
        Dim minCap As Double = Capacitor
        Dim minCapTime As Double = 0

        ' Reset Data
        ShipCapResults.Events.Clear()
        For Each cm As CapacitorModule In ShipCapResults.Modules
            cm.NextTime = 0
        Next

        ' Do the calculations
        ShipCapResults.Events.Clear()
        Dim rate As Double = 0
        Dim Cap As Double = 0
        Dim OldCap As Double = CapacitorCapacity
        While ((Capacitor > 0.0) And (nextTime < maxTime))
            OldCap = Capacitor
            Capacitor = (((1.0 + ((Math.Sqrt((Capacitor / CapacitorCapacity)) - 1.0) * Math.Exp(((currentTime - nextTime) / capConstant)))) ^ 2) * CapacitorCapacity)
            If RecordCapEvents = True Then
                Cap = Capacitor - OldCap
                rate = Cap / (nextTime - currentTime)
                ShipCapResults.Events.Add(New CapacitorEvent(nextTime, "Recharge", OldCap, -Cap, rate))
            End If
            currentTime = nextTime
            nextTime = maxTime
            For Each cm As CapacitorModule In ShipCapResults.Modules
                If cm.IsActive = True Then
                    If cm.NextTime = currentTime Then
                        cm.NextTime += cm.CycleTime
                        If RecordCapEvents = True Then
                            ShipCapResults.Events.Add(New CapacitorEvent(currentTime, cm.Name, Capacitor, cm.CapAmount, rate))
                        End If
                        Capacitor -= cm.CapAmount
                        Capacitor = Math.Min(Capacitor, CapacitorCapacity)
                    End If
                    nextTime = Math.Min(nextTime, cm.NextTime)
                End If
            Next
            If Capacitor < minCap Then
                minCap = Capacitor
                minCapTime = currentTime
            End If
        End While

        ' Set the results
        If Capacitor > 0 Then
            ShipCapResults.CapIsDrained = False
            ShipCapResults.TimeToDrain = -1
            ShipCapResults.MinimumCap = Math.Min(minCap, CapacitorCapacity)
        Else
            ShipCapResults.CapIsDrained = True
            ShipCapResults.TimeToDrain = currentTime
            ShipCapResults.MinimumCap = 0
        End If

    End Sub

    ''' <summary>
    ''' Method for providing a recharge only set of capacitor results by performing a simulation on an instance of the Ship class
    ''' </summary>
    ''' <param name="baseShip">The Ship to be used for the calculations</param>
    ''' <param name="RecordCapEvents">A boolean value indicating whether events should be recorded in the results</param>
    ''' <returns>An instance of the CapSimResults class containing the simulation results</returns>
    ''' <remarks></remarks>
    Public Shared Function CalculateCapRechargeProfile(ByVal baseShip As Ship, ByVal RecordCapEvents As Boolean) As CapSimResults
        Dim CapacitorCapacity As Double = baseShip.CapCapacity
        Dim Capacitor As Double = 0
        Dim currentTime, nextTime As Double
        Dim RechargeRate As Double = baseShip.CapRecharge
        Dim capConstant As Double = (RechargeRate / 5.0)
        Dim maxTime As Double = 43200 ' 12 hour
        Dim minCap As Double = Capacitor
        Dim minCapTime As Double = 0

        ' Create a new instance of the cap results class
        Dim ShipCapResults As New CapSimResults(maxTime)

        ' Create a dummy module to trigger an event
        ShipCapResults.Modules.Add(New CapacitorModule("Recharge Interval", 0, 1, True))

        ' Do the calculations
        Dim rate As Double = 0
        Dim Cap As Double = 0
        Dim OldCap As Double = 0.1
        While Math.Round(Capacitor, 1) < Math.Round(CapacitorCapacity, 1)
            OldCap = Capacitor
            Capacitor = (((1.0 + ((Math.Sqrt((Capacitor / CapacitorCapacity)) - 1.0) * Math.Exp(((currentTime - nextTime) / capConstant)))) ^ 2) * CapacitorCapacity)
            If RecordCapEvents = True Then
                Cap = Capacitor - OldCap
                rate = Cap / (nextTime - currentTime)
                ShipCapResults.Events.Add(New CapacitorEvent(nextTime, "Recharge", OldCap, -Cap, rate))
            End If
            currentTime = nextTime
            nextTime = maxTime
            For Each cm As CapacitorModule In ShipCapResults.Modules
                If cm.NextTime = currentTime Then
                    cm.NextTime += cm.CycleTime
                    If RecordCapEvents = True Then
                        ShipCapResults.Events.Add(New CapacitorEvent(currentTime, cm.Name, Capacitor, cm.CapAmount, rate))
                    End If
                    Capacitor -= cm.CapAmount
                    Capacitor = Math.Min(Capacitor, CapacitorCapacity)
                End If
                nextTime = Math.Min(nextTime, cm.NextTime)
            Next
            If Capacitor < minCap Then
                minCap = Capacitor
                minCapTime = currentTime
            End If
        End While

        ' Set the results
        ShipCapResults.CapIsDrained = False
        ShipCapResults.TimeToDrain = currentTime
        ShipCapResults.MinimumCap = 0

        ' Return the result
        Return ShipCapResults

    End Function

End Class

''' <summary>
''' Class for holding the results of calculating a ship's capacitor statistics
''' </summary>
''' <remarks></remarks>
Public Class CapSimResults

    Dim cSimulationTime As Double = 0
    Dim cCapIsDrained As Boolean = False
    Dim cTimeToDrain As Double = -1
    Dim cMinimumCap As Double = 0
    Dim cEvents As New List(Of CapacitorEvent)
    Dim cModules As New List(Of CapacitorModule)

    ''' <summary>
    ''' Gets the initial maximum running time for the simulation
    ''' </summary>
    ''' <value></value>
    ''' <returns>The initial maximum running time for the simulation</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SimulationTime() As Double
        Get
            Return cSimulationTime
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether the capacitor has drained during the simulation
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating whether the capacitor has drained during the simulation</returns>
    ''' <remarks></remarks>
    Public Property CapIsDrained() As Boolean
        Get
            Return cCapIsDrained
        End Get
        Set(ByVal value As Boolean)
            cCapIsDrained = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the time taken to drain the capacitor
    ''' </summary>
    ''' <value></value>
    ''' <returns>The time taken (s) to drain the capacitor</returns>
    ''' <remarks></remarks>
    Public Property TimeToDrain() As Double
        Get
            Return cTimeToDrain
        End Get
        Set(ByVal value As Double)
            cTimeToDrain = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the lowest capacitor level during the simulation
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating the lowest capacitor level during the simulation</returns>
    ''' <remarks></remarks>
    Public Property MinimumCap() As Double
        Get
            Return cMinimumCap
        End Get
        Set(ByVal value As Double)
            cMinimumCap = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the collection of Events recorded during the simulation
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of Events recorded during the simulation</returns>
    ''' <remarks></remarks>
    Public Property Events() As List(Of CapacitorEvent)
        Get
            Return cEvents
        End Get
        Set(ByVal value As List(Of CapacitorEvent))
            cEvents = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the collection of Modules used in the simulation
    ''' </summary>
    ''' <value></value>
    ''' <returns>A collection of Modules used in the simulation</returns>
    ''' <remarks></remarks>
    Public Property Modules() As List(Of CapacitorModule)
        Get
            Return cModules
        End Get
        Set(ByVal value As List(Of CapacitorModule))
            cModules = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new class for storing results of a capacitor simulation
    ''' </summary>
    ''' <param name="SimTime">The initial maximum running time of the simulation</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal SimTime As Double)
        cSimulationTime = SimTime
        cCapIsDrained = False
        cTimeToDrain = -1
        cMinimumCap = 0
        cEvents = New List(Of CapacitorEvent)
        cModules = New List(Of CapacitorModule)
    End Sub

End Class

''' <summary>
''' Class for storing a module used in capacitor calculations
''' </summary>
''' <remarks></remarks>
Public Class CapacitorModule

    Dim cName As String = ""
    Dim cIsActive As Boolean = False
    Dim cCapAmount As Double = 0
    Dim cCycleTime As Double = 0
    Dim cNextTime As Double = 0

    ''' <summary>
    ''' Gets the module name
    ''' </summary>
    ''' <value></value>
    ''' <returns>The module name</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Name() As String
        Get
            Return cName
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets whether the module is active for the calculations
    ''' </summary>
    ''' <value></value>
    ''' <returns>A boolean value indicating if the module is included in the calculations</returns>
    ''' <remarks></remarks>
    Public Property IsActive As Boolean
        Get
            Return cIsActive
        End Get
        Set(value As Boolean)
            cIsActive = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the capacitor usage or injection per cycle
    ''' </summary>
    ''' <value></value>
    ''' <returns>The capacitor usage or injection per cycle</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CapAmount() As Double
        Get
            Return cCapAmount
        End Get
    End Property

    ''' <summary>
    ''' Gets the cycle time of the module
    ''' </summary>
    ''' <value></value>
    ''' <returns>The cycle time of the module</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CycleTime() As Double
        Get
            Return cCycleTime
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the next time (as an offset) that the module will be activated
    ''' </summary>
    ''' <value></value>
    ''' <returns>The time (as an offset) that the module will next be activated</returns>
    ''' <remarks></remarks>
    Public Property NextTime() As Double
        Get
            Return cNextTime
        End Get
        Set(ByVal value As Double)
            cNextTime = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new Capacitor Module
    ''' </summary>
    ''' <param name="Name">The name of the module</param>
    ''' <param name="CapAmount">The capacitor usage per cycle</param>
    ''' <param name="CycleTime">The cycle time of the module</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Name As String, ByVal CapAmount As Double, ByVal CycleTime As Double, IsActive As Boolean)
        Me.cName = Name
        Me.cCapAmount = CapAmount
        Me.cCycleTime = CycleTime
        Me.cNextTime = 0
        Me.cIsActive = IsActive
    End Sub

End Class

''' <summary>
''' Class for recording an event that occurs during capacitor calculations
''' </summary>
''' <remarks></remarks>
Public Class CapacitorEvent

    Dim cSimTime As Double = 0
    Dim cModuleName As String = ""
    Dim cStartingCap As Double = 0
    Dim cActivationCost As Double = 0
    Dim cRechargeRate As Double = 0

    ''' <summary>
    ''' Gets the time in the simulation of the event
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating the time offset of the event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SimTime() As Double
        Get
            Return cSimTime
        End Get
    End Property

    ''' <summary>
    ''' Gets the name of the module or other cause of the event
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string indicating the module name or other cause of the event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ModuleName() As String
        Get
            Return cModuleName
        End Get
    End Property

    ''' <summary>
    ''' Gets the starting value of the capacitor prior to the event occurring
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating the value of the capacitor prior to the event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StartingCap() As Double
        Get
            Return cStartingCap
        End Get
    End Property

    ''' <summary>
    ''' Gets the capacitor usage or injection of the event
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value equal to the capacitor usage or injection of the event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ActivationCost() As Double
        Get
            Return cActivationCost
        End Get
    End Property

    ''' <summary>
    ''' Gets the capacitor recharge rate at the start of the event
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating the capacitor recharge rate at the start of the event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RechargeRate() As Double
        Get
            Return cRechargeRate
        End Get
    End Property

    ''' <summary>
    ''' Method for creating a new Capacitor Event
    ''' </summary>
    ''' <param name="Time">The time offset of the event</param>
    ''' <param name="ModName">The name of the module or type of event</param>
    ''' <param name="StartCap">The capacitor prior to the event</param>
    ''' <param name="CapCost">The capacitor usage or injection of the event</param>
    ''' <param name="Rate">The capacitor recharge rate at the start of the event</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Time As Double, ByVal ModName As String, ByVal StartCap As Double, ByVal CapCost As Double, ByVal Rate As Double)
        Me.cSimTime = Time
        Me.cModuleName = ModName
        Me.cStartingCap = StartCap
        Me.cActivationCost = CapCost
        Me.cRechargeRate = Rate
    End Sub

End Class
