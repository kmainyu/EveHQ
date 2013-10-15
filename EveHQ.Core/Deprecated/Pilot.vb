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

' ReSharper disable InconsistentNaming
' ReSharper disable once CheckNamespace
<Serializable()> Public Class Pilot

    Public Name As String = ""
    Public ID As String = ""
    Public Account As String = ""
    Public AccountPosition As String = ""
    Public Race As String = ""
    Public Blood As String = ""
    Public Gender As String = ""
    Public Corp As String = ""
    Public CorpID As String = ""
    Public Isk As Double = 0
    Public CloneName As String = ""
    Public CloneSP As String = ""
    Public SkillPoints As Integer = 0
    Public Training As Boolean = False
    Public TrainingStartTime As Date = Now
    Public TrainingStartTimeActual As Date = Now
    Public TrainingEndTime As Date = Now
    Public TrainingEndTimeActual As Date = Now
    Public TrainingSkillID As String = ""
    Public TrainingSkillName As String = ""
    Public TrainingStartSP As Integer = 0
    Public TrainingEndSP As Integer = 0
    Public TrainingCurrentSP As Integer = 0
    Public TrainingCurrentTime As Long = 0
    Public TrainingSkillLevel As Integer = 0
    Public TrainingNotifiedNow As Boolean = False
    Public TrainingNotifiedEarly As Boolean = False
    Public CAtt As Integer = 0
    Public IAtt As Integer = 0
    Public MAtt As Integer = 0
    Public PAtt As Integer = 0
    Public WAtt As Integer = 0
    Public CImplant As Integer = 0
    Public IImplant As Integer = 0
    Public MImplant As Integer = 0
    Public PImplant As Integer = 0
    Public WImplant As Integer = 0
    Public CImplantA As Integer = 0
    Public IImplantA As Integer = 0
    Public MImplantA As Integer = 0
    Public PImplantA As Integer = 0
    Public WImplantA As Integer = 0
    Public CImplantM As Integer = 0
    Public IImplantM As Integer = 0
    Public MImplantM As Integer = 0
    Public PImplantM As Integer = 0
    Public WImplantM As Integer = 0
    Public UseManualImplants As Boolean = False
    Public CAttT As Double = 0
    Public IAttT As Double = 0
    Public MAttT As Double = 0
    Public PAttT As Double = 0
    Public WAttT As Double = 0
    Public PilotSkills As New Collection
    Public QueuedSkills As New SortedList(Of Long, PilotQueuedSkill)
    Public QueuedSkillTime As Long
    Public Certificates As New ArrayList
    Public PrimaryQueue As String = ""
    Public ActiveQueue As New SkillQueue
    Public ActiveQueueName As String = ""
    <NonSerialized()> Public TrainingQueues As New SortedList
    Public Blueprints As New Collection
    Public CacheFileTime As Date
    Public CacheExpirationTime As Date
    Public TrainingFileTime As Date
    Public TrainingExpirationTime As Date
    Public Updated As Boolean = False
    Public LastUpdate As String = ""
    Public Active As Boolean = True
    Public KeySkills(53) As String
    Public Standings As New SortedList(Of Long, PilotStanding)
    Public CorpRoles As New List(Of CorporationRoles)
    Public Enum KeySkill
        Mining = 1
        MiningUpgrades = 2
        Astrogeology = 3
        MiningBarge = 4
        MiningDrone = 5
        Exhumers = 6
        Refining = 7
        RefiningEfficiency = 8
        Metallurgy = 9
        Research = 10
        Science = 11
        Industry = 12
        ProductionEfficiency = 13
        ArkonorProc = 14
        BistotProc = 15
        CrokiteProc = 16
        DarkOchreProc = 17
        GneissProc = 18
        HedbergiteProc = 19
        HemorphiteProc = 20
        JaspetProc = 21
        KerniteProc = 22
        MercoxitProc = 23
        OmberProc = 24
        PlagioclaseProc = 25
        PyroxeresProc = 26
        ScorditeProc = 27
        SpodumainProc = 28
        VeldsparProc = 29
        IceProc = 30
        IceHarvesting = 31
        DeepCoreMining = 32
        MiningForeman = 33
        MiningDirector = 34
        Learning = 35
        JumpDriveOperation = 36
        JumpDriveCalibration = 37
        JumpFuelConservation = 38
        JumpFreighters = 39
        ScrapMetalProc = 40
        Accounting = 41
        BrokerRelations = 42
        Daytrading = 43
        MarginTrading = 44
        Marketing = 45
        Procurement = 46
        Retail = 47
        Trade = 48
        Tycoon = 49
        Visibility = 50
        Wholesale = 51
        Diplomacy = 52
        Connections = 53
	End Enum
End Class

<Serializable()> Public Class PilotSkill
    Implements ICloneable
    Public ID As String
    Public Name As String
    Public GroupID As String
    Public Flag As Integer
    Public Rank As Integer
    Public SP As Integer
    Public Level As Integer
    Public LevelUp(5) As Integer
    Public Function Clone() As Object Implements ICloneable.Clone
        Dim R As PilotSkill = CType(MemberwiseClone(), PilotSkill)
        Return R
    End Function
End Class

<Serializable()> Public Class PilotQueuedSkill
    Public Position As Integer
    Public SkillID As Integer
    Public Level As Integer
    Public StartSP As Long
    Public EndSP As Long
    Public StartTime As DateTime
    Public EndTime As DateTime
End Class

<Serializable()> Public Class PilotStanding
    Public Type As StandingType
    Public ID As Long ' Key for Standings
    Public Name As String
    Public Standing As Double
End Class
