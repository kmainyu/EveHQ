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

''' <summary>
''' Class for holding various capacitor related routines 
''' </summary>
''' <remarks></remarks>
Public Class Capacitor

    ''' <summary>
    ''' Method for providing capacitor results by performing a simulation on an instance of the Ship class
    ''' </summary>
    ''' <param name="baseShip">The Ship to be used for the calculations</param>
    ''' <param name="recordCapEvents">A boolean value indicating whether events should be recorded in the results</param>
    ''' <returns>An instance of the CapSimResults class containing the simulation results</returns>
    ''' <remarks></remarks>
    Public Shared Function CalculateCapStatistics(ByVal baseShip As Ship, ByVal recordCapEvents As Boolean) As CapSimResults
        Dim capacitorCapacity As Double = baseShip.CapCapacity
        Dim capacitor As Double = capacitorCapacity
        Dim currentTime, nextTime As Double
        Dim rechargeRate As Double = baseShip.CapRecharge
        Dim capConstant As Double = (rechargeRate / 5.0)
        Const maxTime As Double = 43200 ' 12 hour
        Dim minCap As Double = capacitor

        ' Create a new instance of the cap results class
        Dim shipCapResults As New CapSimResults(maxTime)

        ' Populate the module list
        For Each slotMod As ShipModule In baseShip.SlotCollection
            If slotMod.CapUsage <> 0 Then
                Dim totalTime As Double = slotMod.ActivationTime + slotMod.ReactivationDelay
                If slotMod.Attributes.ContainsKey(AttributeEnum.ModuleEnergyRof) Then
                    totalTime += slotMod.Attributes(AttributeEnum.ModuleEnergyRof)
                End If
                If slotMod.Attributes.ContainsKey(AttributeEnum.ModuleHybridRof) Then
                    totalTime += slotMod.Attributes(AttributeEnum.ModuleHybridRof)
                End If
                If (slotMod.ModuleState Or 28) = 28 Then
                    shipCapResults.Modules.Add(New CapacitorModule(slotMod.Name, slotMod.SlotType, slotMod.CapUsage, totalTime, True))
                Else
                    shipCapResults.Modules.Add(New CapacitorModule(slotMod.Name, slotMod.SlotType, slotMod.CapUsage, totalTime, False))
                End If
            End If
        Next

        ' Reset Data
        shipCapResults.Events.Clear()
        For Each cm As CapacitorModule In shipCapResults.Modules
            cm.NextTime = 0
        Next

        ' Do the calculations
        Dim rate As Double = 0
        Dim cap As Double
        Dim oldCap As Double
        While ((capacitor > 0.0) And (nextTime < maxTime))
            oldCap = capacitor
            capacitor = (((1.0 + ((Math.Sqrt((capacitor / capacitorCapacity)) - 1.0) * Math.Exp(((currentTime - nextTime) / capConstant)))) ^ 2) * capacitorCapacity)
            If recordCapEvents = True Then
                cap = capacitor - oldCap
                rate = cap / (nextTime - currentTime)
                shipCapResults.Events.Add(New CapacitorEvent(nextTime, "Recharge", oldCap, -cap, rate))
            End If
            currentTime = nextTime
            nextTime = maxTime
            For Each cm As CapacitorModule In shipCapResults.Modules
                If cm.IsActive = True Then
                    If cm.NextTime = currentTime Then
                        cm.NextTime += cm.CycleTime
                        If recordCapEvents = True Then
                            shipCapResults.Events.Add(New CapacitorEvent(currentTime, cm.Name, capacitor, cm.CapAmount, rate))
                        End If
                        capacitor -= cm.CapAmount
                        capacitor = Math.Min(capacitor, capacitorCapacity)
                    End If
                    nextTime = Math.Min(nextTime, cm.NextTime)
                End If
            Next
            If capacitor < minCap Then
                minCap = capacitor
            End If
        End While

        ' Set the results
        If capacitor > 0 Then
            shipCapResults.CapIsDrained = False
            shipCapResults.TimeToDrain = -1
            shipCapResults.MinimumCap = Math.Min(minCap, capacitorCapacity)
        Else
            shipCapResults.CapIsDrained = True
            shipCapResults.TimeToDrain = currentTime
            shipCapResults.MinimumCap = 0
        End If

        ' Return the result
        Return shipCapResults

    End Function

    ''' <summary>
    ''' Method for recalculating cap stats once a previous set of results has been obtained
    ''' </summary>
    ''' <param name="baseShip">The Ship to be used for the calculations</param>
    ''' <param name="recordCapEvents">A boolean value indicating whether events should be recorded in the results</param>
    ''' <param name="shipCapResults">A set of existing CapSimResults</param>
    ''' <remarks></remarks>
    Public Shared Sub RecalculateCapStatistics(ByVal baseShip As Ship, ByVal recordCapEvents As Boolean, ByRef shipCapResults As CapSimResults)
        Dim capacitorCapacity As Double = baseShip.CapCapacity
        Dim capacitor As Double = capacitorCapacity
        Dim currentTime, nextTime As Double
        Dim rechargeRate As Double = baseShip.CapRecharge
        Dim capConstant As Double = (rechargeRate / 5.0)
        Const maxTime As Double = 43200 ' 12 hour
        Dim minCap As Double = capacitor

        ' Reset Data
        shipCapResults.Events.Clear()
        For Each cm As CapacitorModule In shipCapResults.Modules
            cm.NextTime = 0
        Next

        ' Do the calculations
        shipCapResults.Events.Clear()
        Dim rate As Double = 0
        Dim cap As Double
        Dim oldCap As Double
        While ((capacitor > 0.0) And (nextTime < maxTime))
            oldCap = capacitor
            capacitor = (((1.0 + ((Math.Sqrt((capacitor / capacitorCapacity)) - 1.0) * Math.Exp(((currentTime - nextTime) / capConstant)))) ^ 2) * capacitorCapacity)
            If recordCapEvents = True Then
                cap = capacitor - oldCap
                rate = cap / (nextTime - currentTime)
                shipCapResults.Events.Add(New CapacitorEvent(nextTime, "Recharge", oldCap, -cap, rate))
            End If
            currentTime = nextTime
            nextTime = maxTime
            For Each cm As CapacitorModule In shipCapResults.Modules
                If cm.IsActive = True Then
                    If cm.NextTime = currentTime Then
                        cm.NextTime += cm.CycleTime
                        If recordCapEvents = True Then
                            shipCapResults.Events.Add(New CapacitorEvent(currentTime, cm.Name, capacitor, cm.CapAmount, rate))
                        End If
                        capacitor -= cm.CapAmount
                        capacitor = Math.Min(capacitor, capacitorCapacity)
                    End If
                    nextTime = Math.Min(nextTime, cm.NextTime)
                End If
            Next
            If capacitor < minCap Then
                minCap = capacitor
            End If
        End While

        ' Set the results
        If capacitor > 0 Then
            ShipCapResults.CapIsDrained = False
            ShipCapResults.TimeToDrain = -1
            shipCapResults.MinimumCap = Math.Min(minCap, capacitorCapacity)
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
    ''' <param name="recordCapEvents">A boolean value indicating whether events should be recorded in the results</param>
    ''' <returns>An instance of the CapSimResults class containing the simulation results</returns>
    ''' <remarks></remarks>
    Public Shared Function CalculateCapRechargeProfile(ByVal baseShip As Ship, ByVal recordCapEvents As Boolean) As CapSimResults
        Dim capacitorCapacity As Double = baseShip.CapCapacity
        Dim capacitor As Double = 0
        Dim currentTime, nextTime As Double
        Dim rechargeRate As Double = baseShip.CapRecharge
        Dim capConstant As Double = (rechargeRate / 5.0)
        Const maxTime As Double = 43200 ' 12 hour
        Dim minCap As Double = capacitor

        ' Create a new instance of the cap results class
        Dim shipCapResults As New CapSimResults(maxTime)

        ' Create a dummy module to trigger an event
        shipCapResults.Modules.Add(New CapacitorModule("Recharge Interval", SlotTypes.Low, 0, 1, True))

        ' Do the calculations
        Dim rate As Double = 0
        Dim cap As Double
        Dim oldCap As Double
        While Math.Round(capacitor, 1) < Math.Round(capacitorCapacity, 1)
            oldCap = capacitor
            capacitor = (((1.0 + ((Math.Sqrt((capacitor / capacitorCapacity)) - 1.0) * Math.Exp(((currentTime - nextTime) / capConstant)))) ^ 2) * capacitorCapacity)
            If recordCapEvents = True Then
                cap = capacitor - oldCap
                rate = cap / (nextTime - currentTime)
                shipCapResults.Events.Add(New CapacitorEvent(nextTime, "Recharge", oldCap, -cap, rate))
            End If
            currentTime = nextTime
            nextTime = maxTime
            For Each cm As CapacitorModule In shipCapResults.Modules
                If cm.NextTime = currentTime Then
                    cm.NextTime += cm.CycleTime
                    If recordCapEvents = True Then
                        shipCapResults.Events.Add(New CapacitorEvent(currentTime, cm.Name, capacitor, cm.CapAmount, rate))
                    End If
                    capacitor -= cm.CapAmount
                    capacitor = Math.Min(capacitor, capacitorCapacity)
                End If
                nextTime = Math.Min(nextTime, cm.NextTime)
            Next
            If capacitor < minCap Then
                minCap = capacitor
            End If
        End While

        ' Set the results
        shipCapResults.CapIsDrained = False
        shipCapResults.TimeToDrain = currentTime
        shipCapResults.MinimumCap = 0

        ' Return the result
        Return shipCapResults

    End Function

End Class

''' <summary>
''' Class for holding the results of calculating a ship's capacitor statistics
''' </summary>
''' <remarks></remarks>
Public Class CapSimResults

    ReadOnly _simulationTime As Double = 0
    Dim _capIsDrained As Boolean = False
    Dim _timeToDrain As Double = -1
    Dim _minimumCap As Double = 0
    Dim _events As New List(Of CapacitorEvent)
    Dim _modules As New List(Of CapacitorModule)

    ''' <summary>
    ''' Gets the initial maximum running time for the simulation
    ''' </summary>
    ''' <value></value>
    ''' <returns>The initial maximum running time for the simulation</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SimulationTime() As Double
        Get
            Return _simulationTime
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
            Return _capIsDrained
        End Get
        Set(ByVal value As Boolean)
            _capIsDrained = value
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
            Return _timeToDrain
        End Get
        Set(ByVal value As Double)
            _timeToDrain = value
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
            Return _minimumCap
        End Get
        Set(ByVal value As Double)
            _minimumCap = value
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
            Return _events
        End Get
        Set(ByVal value As List(Of CapacitorEvent))
            _events = value
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
            Return _modules
        End Get
        Set(ByVal value As List(Of CapacitorModule))
            _modules = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new class for storing results of a capacitor simulation
    ''' </summary>
    ''' <param name="simTime">The initial maximum running time of the simulation</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal simTime As Double)
        _simulationTime = simTime
        _capIsDrained = False
        _timeToDrain = -1
        _minimumCap = 0
        _events = New List(Of CapacitorEvent)
        _modules = New List(Of CapacitorModule)
    End Sub

End Class

''' <summary>
''' Class for storing a module used in capacitor calculations
''' </summary>
''' <remarks></remarks>
Public Class CapacitorModule

    ReadOnly _name As String = ""
    ReadOnly _slotType As SlotTypes
    Dim _isActive As Boolean = False
    ReadOnly _capAmount As Double = 0
    ReadOnly _cycleTime As Double = 0
    Dim _nextTime As Double = 0

    ''' <summary>
    ''' Gets the module name
    ''' </summary>
    ''' <value></value>
    ''' <returns>The module name</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Name() As String
        Get
            Return _name
        End Get
    End Property

    ''' <summary>
    ''' Gets the slot type for the module
    ''' </summary>
    ''' <value></value>
    ''' <returns>The slot type of the module</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SlotType As SlotTypes
        Get
            Return _slotType
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
            Return _isActive
        End Get
        Set(value As Boolean)
            _isActive = value
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
            Return _capAmount
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
            Return _cycleTime
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
            Return _nextTime
        End Get
        Set(ByVal value As Double)
            _nextTime = value
        End Set
    End Property

    ''' <summary>
    ''' Creates a new Capacitor Module
    ''' </summary>
    ''' <param name="modName">The name of the module</param>
    ''' <param name="modSlotType">The slot type of the module</param>
    ''' <param name="modCapAmount">The capacitor usage per cycle</param>
    ''' <param name="modCycleTime">The cycle time of the module</param>
    '''<param name="modIsActive">Whether the module is active</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal modName As String, ByVal modSlotType As SlotTypes, ByVal modCapAmount As Double, ByVal modCycleTime As Double, modIsActive As Boolean)
        _name = modName
        _slotType = modSlotType
        _capAmount = modCapAmount
        _cycleTime = modCycleTime
        _nextTime = 0
        _isActive = modIsActive
    End Sub

End Class

''' <summary>
''' Class for recording an event that occurs during capacitor calculations
''' </summary>
''' <remarks></remarks>
Public Class CapacitorEvent

    ReadOnly _simTime As Double = 0
    ReadOnly _moduleName As String = ""
    ReadOnly _startingCap As Double = 0
    ReadOnly _activationCost As Double = 0
    ReadOnly _rechargeRate As Double = 0

    ''' <summary>
    ''' Gets the time in the simulation of the event
    ''' </summary>
    ''' <value></value>
    ''' <returns>A value indicating the time offset of the event</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SimTime() As Double
        Get
            Return _simTime
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
            Return _moduleName
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
            Return _startingCap
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
            Return _activationCost
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
            Return _rechargeRate
        End Get
    End Property

    ''' <summary>
    ''' Method for creating a new Capacitor Event
    ''' </summary>
    ''' <param name="time">The time offset of the event</param>
    ''' <param name="modName">The name of the module or type of event</param>
    ''' <param name="startCap">The capacitor prior to the event</param>
    ''' <param name="capCost">The capacitor usage or injection of the event</param>
    ''' <param name="Rate">The capacitor recharge rate at the start of the event</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal time As Double, ByVal modName As String, ByVal startCap As Double, ByVal capCost As Double, ByVal rate As Double)
        _simTime = time
        _moduleName = modName
        _startingCap = startCap
        _activationCost = CapCost
        _rechargeRate = rate
    End Sub

End Class
