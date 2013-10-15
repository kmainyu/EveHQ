<Serializable()> Public Class EveHQPilot

   Public Property Name As String = ""
    Public Property ID As String = ""
    Public Property Account As String = ""
    Public Property AccountPosition As String = ""
    Public Property Race As String = ""
    Public Property Blood As String = ""
    Public Property Gender As String = ""
    Public Property Corp As String = ""
    Public Property CorpID As String = ""
    Public Property Isk As Double = 0
    Public Property CloneName As String = ""
    Public Property CloneSP As Integer = 0
    Public Property SkillPoints As Integer = 0
    Public Property Training As Boolean = False
    Public Property TrainingStartTime As Date = Now
    Public Property TrainingStartTimeActual As Date = Now
    Public Property TrainingEndTime As Date = Now
    Public Property TrainingEndTimeActual As Date = Now
    Public Property TrainingSkillID As Integer = 0
    Public Property TrainingSkillName As String = ""
    Public Property TrainingStartSP As Integer = 0
    Public Property TrainingEndSP As Integer = 0
    Public Property TrainingCurrentSP As Integer = 0
    Public Property TrainingCurrentTime As Long = 0
    Public Property TrainingSkillLevel As Integer = 0
    Public Property TrainingNotifiedNow As Boolean = False
    Public Property TrainingNotifiedEarly As Boolean = False
    Public Property CAtt As Integer = 0
    Public Property IntAtt As Integer = 0
    Public Property MAtt As Integer = 0
    Public Property PAtt As Integer = 0
    Public Property WAtt As Integer = 0
    Public Property CImplant As Integer = 0
    Public Property IntImplant As Integer = 0
    Public Property MImplant As Integer = 0
    Public Property PImplant As Integer = 0
    Public Property WImplant As Integer = 0
    Public Property CImplantA As Integer = 0
    Public Property IntImplantA As Integer = 0
    Public Property MImplantA As Integer = 0
    Public Property PImplantA As Integer = 0
    Public Property WImplantA As Integer = 0
    Public Property CImplantM As Integer = 0
    Public Property IntImplantM As Integer = 0
    Public Property MImplantM As Integer = 0
    Public Property PImplantM As Integer = 0
    Public Property WImplantM As Integer = 0
    Public Property UseManualImplants As Boolean = False
    Public Property CAttT As Double = 0
    Public Property IntAttT As Double = 0
    Public Property MAttT As Double = 0
    Public Property PAttT As Double = 0
    Public Property WAttT As Double = 0
    Public Property PilotSkills As New Dictionary(Of String, EveHQPilotSkill)
    Public Property QueuedSkills As New SortedList(Of Integer, EveHQPilotQueuedSkill)
    Public Property QueuedSkillTime As Long
    Public Property Certificates As New List(Of Integer)
    Public Property PrimaryQueue As String = ""
    Public Property ActiveQueue As New EveHQSkillQueue
    Public Property ActiveQueueName As String = ""
    Public Property TrainingQueues As New SortedList(Of String, EveHQSkillQueue)
    Public Property CacheFileTime As Date
    Public Property CacheExpirationTime As Date
    Public Property TrainingFileTime As Date
    Public Property TrainingExpirationTime As Date
    Public Property Updated As Boolean = False
    Public Property LastUpdate As String = ""
    Public Property Active As Boolean = True
    Public Property Standings As New SortedList(Of Long, PilotStanding)
    Public Property CorpRoles As New List(Of CorporationRoles)
    Public Property KeySkills As New Dictionary(Of KeySkill, Integer)

End Class

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