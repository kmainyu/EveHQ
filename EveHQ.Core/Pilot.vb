' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports System.Xml

Public Class Pilot

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
    Public LCAtt As Integer = 0
    Public LIAtt As Integer = 0
    Public LMAtt As Integer = 0
    Public LPAtt As Integer = 0
    Public LWAtt As Integer = 0
    Public ALCAtt As Integer = 0
    Public ALIAtt As Integer = 0
    Public ALMAtt As Integer = 0
    Public ALPAtt As Integer = 0
    Public ALWAtt As Integer = 0
    Public LSCAtt As Double = 0
    Public LSIAtt As Double = 0
    Public LSMAtt As Double = 0
    Public LSPAtt As Double = 0
    Public LSWAtt As Double = 0
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
    Public PilotData As XmlDocument = New XmlDocument
    Public PilotTrainingData As XmlDocument = New XmlDocument
    Public PilotSkills As Collection = New Collection
    Public PrimaryQueue As String = ""
    Public ActiveQueue As EveHQ.Core.SkillQueue = New EveHQ.Core.SkillQueue
    Public ActiveQueueName As String = ""
    Public TrainingQueues As SortedList = New SortedList
    Public Blueprints As Collection = New Collection
    Public CacheFileTime As Date
    Public CacheExpiration As Long
    Public CacheExpirationTime As Date
    Public Updated As Boolean = False
    Public LastUpdate As String = ""
    Public Active As Boolean = True
    Public KeySkills(39) As String
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
    End Enum
End Class
