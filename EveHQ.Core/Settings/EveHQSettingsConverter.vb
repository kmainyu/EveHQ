﻿Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports System.Xml
Imports System.Web
Imports System.Globalization

''' <summary>
''' Converts the old settings format into the new one
''' </summary>
''' <remarks></remarks>
Public Class EveHQSettingsConverter

    Private _newSettings As EveHQSettings

    Public Sub New()
        _newSettings = New EveHQSettings
    End Sub

    Public Function ConvertOldSettings(settingsFolder As String) As EveHQSettings
        Dim oldSettings As EveSettings
        _newSettings = New EveHQSettings

        ' Load in the old EveHQ Settings

        If My.Computer.FileSystem.FileExists(Path.Combine(settingsFolder, "EveHQSettings.bin")) = True Then
            Using s As New FileStream(Path.Combine(settingsFolder, "EveHQSettings.bin"), FileMode.Open)
                Dim f As BinaryFormatter = New BinaryFormatter
                oldSettings = CType(f.Deserialize(s), EveSettings)
            End Using

            ' Add training
            LoadTraining(oldSettings, Path.Combine(settingsFolder, "Data"))

            ConvertMainSettings(oldSettings)
            ConvertAccounts(oldSettings)
            ConvertPilots(oldSettings)
            ConvertPlugins(oldSettings)
            'ConvertDashboard(oldSettings)

            Dim startTime, endTime As DateTime
            Dim timeTaken As TimeSpan

            startTime = Now
            Dim json As String = JsonConvert.SerializeObject(_newSettings, Newtonsoft.Json.Formatting.Indented)
            endTime = Now
            timeTaken = (endTime - startTime)

            ' Write a JSON version of the settings
            Try
                Using s As New StreamWriter(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"), False)
                    s.Write(json)
                    s.Flush()
                End Using
            Catch e As Exception
            End Try

            ' Rename the old settings file
            My.Computer.FileSystem.RenameFile(Path.Combine(settingsFolder, "EveHQSettings.bin"), "EveHQSettings.oldbin")

            'MessageBox.Show("Conversion and writing complete in " & timeTaken.TotalMilliseconds.ToString("N2") & "ms")

        End If

        Return _newSettings

    End Function

    Public Shared Sub LoadTraining(oldSettings As EveSettings, datafolder As String)
        Dim currentPilot As Pilot = New Pilot
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""
        Dim tFileName As String = ""

        Dim trainingList, QueueList As XmlNodeList
        Dim trainingDetails, Queuedetails As XmlNode

        Dim ObsoleteSkills() As String =
                {"Analytical Mind", "Clarity", "Eidetic Memory", "Empathy", "Focus", "Instant Recall", "Iron Will",
                 "Learning", "Logic", "Presence", "Spatial Awareness"}
        Dim ObsoleteList As New List(Of String)(ObsoleteSkills)

        For Each currentPilot In oldSettings.Pilots
            currentPilot.ActiveQueue = New SkillQueue
            'currentPilot.ActiveQueue.Queue.Clear()
            currentPilot.TrainingQueues = New SortedList
            currentPilot.TrainingQueues.Clear()
            currentPilot.PrimaryQueue = ""

            tFileName = "Q_" & currentPilot.Name & ".xml"
            If My.Computer.FileSystem.FileExists(Path.Combine(HQ.dataFolder, tFileName)) = True Then
                Try
                    XMLdoc.Load(Path.Combine(HQ.dataFolder, tFileName))

                    ' Get the pilot details
                    trainingList = XMLdoc.SelectNodes("/training/skill")

                    If trainingList.Count > 0 Then
                        ' Using version prior to 1.3
                        ' Start a new SkillQueue class (using "primary" as the default name)
                        Dim newQ As New SkillQueue
                        newQ.Name = "Primary"
                        newQ.IncCurrentTraining = True
                        newQ.Primary = True
                        For Each trainingDetails In trainingList
                            Dim myskill As SkillQueueItem = New SkillQueueItem
                            myskill.Name = trainingDetails.ChildNodes(0).InnerText
                            myskill.FromLevel = CInt(trainingDetails.ChildNodes(1).InnerText)
                            myskill.ToLevel = CInt(trainingDetails.ChildNodes(2).InnerText)
                            myskill.Pos = CInt(trainingDetails.ChildNodes(3).InnerText)
                            Dim keyName As String = myskill.Name & myskill.FromLevel & myskill.ToLevel
                            currentPilot.ActiveQueue.Queue.Add(myskill, keyName)
                        Next
                        newQ.Queue = currentPilot.ActiveQueue.Queue
                        currentPilot.PrimaryQueue = newQ.Name
                        currentPilot.TrainingQueues.Add(newQ.Name, newQ)
                    Else
                        ' Try for the post 1.3 version
                        ' Get version
                        Dim rootNode As XmlNode = XMLdoc.SelectSingleNode("/training")
                        Dim version As Double = 0
                        Dim culture As CultureInfo = New CultureInfo("en-GB")
                        If rootNode.Attributes.Count > 0 Then
                            version = Double.Parse(rootNode.Attributes("version").Value, NumberStyles.Any, culture)
                        End If
                        QueueList = XMLdoc.SelectNodes("/training/queue")
                        If QueueList.Count > 0 Then
                            For Each Queuedetails In QueueList
                                Dim newQ As New SkillQueue
                                newQ.Name = HttpUtility.HtmlDecode(Queuedetails.Attributes("name").Value)
                                newQ.IncCurrentTraining = CBool(Queuedetails.Attributes("ICT").Value)
                                newQ.Primary = CBool(Queuedetails.Attributes("primary").Value)
                                If newQ.Primary = True Then
                                    If currentPilot.PrimaryQueue <> "" Then
                                        ' There can be only one!
                                        newQ.Primary = False
                                    Else
                                        currentPilot.PrimaryQueue = newQ.Name
                                    End If
                                End If
                                trainingList = Queuedetails.SelectNodes("training/queue/skill")
                                If Queuedetails.ChildNodes.Count > 0 Then
                                    ' Using version prior to 1.3
                                    ' Start a new SkillQueue class (using "primary" as the default name)
                                    For Each trainingDetails In Queuedetails.ChildNodes
                                        If ObsoleteList.Contains(trainingDetails.ChildNodes(0).InnerText) = False Then
                                            Dim myskill As SkillQueueItem = New SkillQueueItem
                                            myskill.Name = trainingDetails.ChildNodes(0).InnerText
                                            ' Adjust for the 1.9 version
                                            If version < 1.9 Then
                                                If myskill.Name = "Astrometric Triangulation" Then
                                                    myskill.Name = "Astrometric Acquisition"
                                                End If
                                                If myskill.Name = "Signal Acquisition" Then
                                                    myskill.Name = "Astrometric Triangulation"
                                                End If
                                            End If
                                            Try
                                                myskill.FromLevel = CInt(trainingDetails.ChildNodes(1).InnerText)
                                                myskill.ToLevel = CInt(trainingDetails.ChildNodes(2).InnerText)
                                                myskill.Pos = CInt(trainingDetails.ChildNodes(3).InnerText)
                                                myskill.Notes =
                                                    HttpUtility.HtmlDecode(trainingDetails.ChildNodes(4).InnerText)
                                            Catch e As Exception
                                                ' We don't have the required info
                                            End Try
                                            Dim keyName As String = myskill.Name & myskill.FromLevel & myskill.ToLevel
                                            If newQ.Queue.Contains(keyName) = False Then
                                                If myskill.ToLevel > myskill.FromLevel Then
                                                    newQ.Queue.Add(myskill, keyName) _
                                                    ' Multi queue method
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                                currentPilot.TrainingQueues.Add(newQ.Name, newQ)
                            Next
                        End If
                    End If

                    ' Iterate through the relevant nodes

                Catch e As Exception
                    MessageBox.Show(
                        "Error importing Training data for " & currentPilot.Name & "." & ControlChars.CrLf &
                        "The error reported was " & e.Message, "Training Data Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
                End Try
            End If
        Next
    End Sub

    Private Sub ConvertMainSettings(oldSettings As EveSettings)

        _newSettings.MarketDataProvider = oldSettings.MarketDataProvider
        _newSettings.MaxUpdateThreads = oldSettings.MaxUpdateThreads
        _newSettings.MarketDataSource = oldSettings.MarketDataSource
        _newSettings.Corporations = oldSettings.Corporations
        _newSettings.PriceGroups = oldSettings.PriceGroups
        _newSettings.SkillQueuePanelWidth = oldSettings.SkillQueuePanelWidth
        _newSettings.AccountTimeLimit = oldSettings.AccountTimeLimit
        _newSettings.NotifyAccountTime = oldSettings.NotifyAccountTime
        _newSettings.NotifyInsuffClone = oldSettings.NotifyInsuffClone
        _newSettings.StartWithPrimaryQueue = oldSettings.StartWithPrimaryQueue
        _newSettings.IgnoreLastMessage = oldSettings.IgnoreLastMessage
        _newSettings.LastMessageDate = oldSettings.LastMessageDate
        _newSettings.DisableTrainingBar = oldSettings.DisableTrainingBar
        _newSettings.EnableAutomaticSave = oldSettings.EnableAutomaticSave
        _newSettings.AutomaticSaveTime = oldSettings.AutomaticSaveTime
        _newSettings.RibbonMinimised = oldSettings.RibbonMinimised
        _newSettings.ThemeSetByUser = oldSettings.ThemeSetByUser
        _newSettings.ThemeTint = oldSettings.ThemeTint
        _newSettings.ThemeStyle = oldSettings.ThemeStyle
        _newSettings.SQLQueries = oldSettings.SQLQueries
        _newSettings.BackupBeforeUpdate = oldSettings.BackupBeforeUpdate
        _newSettings.QatLayout = oldSettings.QATLayout
        _newSettings.NotifyEveNotification = oldSettings.NotifyEveNotification
        _newSettings.NotifyEveMail = oldSettings.NotifyEveMail
        _newSettings.AutoMailAPI = oldSettings.AutoMailAPI
        _newSettings.EveHqBackupWarnFreq = oldSettings.EveHQBackupWarnFreq
        _newSettings.EveHqBackupMode = oldSettings.EveHQBackupMode
        _newSettings.EveHqBackupStart = oldSettings.EveHQBackupStart
        _newSettings.EveHqBackupFreq = oldSettings.EveHQBackupFreq
        _newSettings.EveHqBackupLast = oldSettings.EveHQBackupLast
        _newSettings.EveHqBackupLastResult = oldSettings.EveHQBackupLastResult
        _newSettings.IbShowAllItems = oldSettings.IBShowAllItems
        _newSettings.EmailSenderAddress = oldSettings.EmailSenderAddress
        _newSettings.UserQueueColumns = oldSettings.UserQueueColumns
        _newSettings.StandardQueueColumns = oldSettings.StandardQueueColumns
        _newSettings.DBTickerLocation = oldSettings.DBTickerLocation
        _newSettings.DBTicker = oldSettings.DBTicker
        _newSettings.DashboardConfiguration = oldSettings.DashboardConfiguration
        _newSettings.CsvSeparatorChar = oldSettings.CSVSeparatorChar
        _newSettings.DisableVisualStyles = oldSettings.DisableVisualStyles
        _newSettings.DisableAutoWebConnections = oldSettings.DisableAutoWebConnections
        _newSettings.TrainingBarHeight = oldSettings.TrainingBarHeight
        _newSettings.TrainingBarWidth = oldSettings.TrainingBarWidth
        _newSettings.TrainingBarDockPosition = oldSettings.TrainingBarDockPosition
        _newSettings.MdiTabPosition = oldSettings.MDITabPosition
        _newSettings.ShowCompletedSkills = oldSettings.ShowCompletedSkills
        _newSettings.MarketRegionList = oldSettings.MarketRegionList
        _newSettings.IgnoreBuyOrderLimit = oldSettings.IgnoreBuyOrderLimit
        _newSettings.IgnoreSellOrderLimit = oldSettings.IgnoreSellOrderLimit
        For i As Integer = 0 To 11
            _newSettings.PriceCriteria(i) = oldSettings.PriceCriteria(i)
        Next
        _newSettings.MarketLogUpdateData = oldSettings.MarketLogUpdateData
        _newSettings.MarketLogUpdatePrice = oldSettings.MarketLogUpdatePrice
        _newSettings.MarketLogPopupConfirm = oldSettings.MarketLogPopupConfirm
        _newSettings.MarketLogToolTipConfirm = oldSettings.MarketLogToolTipConfirm
        _newSettings.IgnoreBuyOrders = oldSettings.IgnoreBuyOrders
        _newSettings.IgnoreSellOrders = oldSettings.IgnoreSellOrders
        _newSettings.DBDataName = oldSettings.DBDataName
        _newSettings.DBDataFilename = oldSettings.DBDataFilename
        _newSettings.DBTimeout = oldSettings.DBTimeout
        _newSettings.PilotSkillHighlightColor = oldSettings.PilotSkillHighlightColor
        _newSettings.PilotSkillTextColor = oldSettings.PilotSkillTextColor
        _newSettings.PilotGroupTextColor = oldSettings.PilotGroupTextColor
        _newSettings.PilotGroupBackgroundColor = oldSettings.PilotGroupBackgroundColor
        _newSettings.ErrorReportingEmail = oldSettings.ErrorReportingEmail
        _newSettings.ErrorReportingName = oldSettings.ErrorReportingName
        _newSettings.ErrorReportingEnabled = oldSettings.ErrorReportingEnabled
        _newSettings.TaskbarIconMode = oldSettings.TaskbarIconMode
        _newSettings.EcmDefaultLocation = oldSettings.ECMDefaultLocation
        _newSettings.APIFileExtension = oldSettings.APIFileExtension
        _newSettings.UseAppDirectoryForDB = oldSettings.UseAppDirectoryForDB
        _newSettings.OmitCurrentSkill = oldSettings.OmitCurrentSkill
        _newSettings.UpdateUrl = oldSettings.UpdateURL
        _newSettings.UseCcpapiBackup = oldSettings.UseCCPAPIBackup
        _newSettings.UseApirs = oldSettings.UseAPIRS
        _newSettings.ApirsAddress = oldSettings.APIRSAddress
        _newSettings.CcpapiServerAddress = oldSettings.CCPAPIServerAddress
        For i As Integer = 1 To 4
            _newSettings.EveFolderLabel(i) = oldSettings.EveFolderLabel(i)
        Next
        _newSettings.PilotCurrentTrainSkillColor = oldSettings.PilotCurrentTrainSkillColor
        _newSettings.PilotPartTrainedSkillColor = oldSettings.PilotPartTrainedSkillColor
        _newSettings.PilotLevel5SkillColor = oldSettings.PilotLevel5SkillColor
        _newSettings.PilotStandardSkillColor = oldSettings.PilotStandardSkillColor
        _newSettings.PanelHighlightColor = oldSettings.PanelHighlightColor
        _newSettings.PanelTextColor = oldSettings.PanelTextColor
        _newSettings.PanelRightColor = oldSettings.PanelRightColor
        _newSettings.PanelLeftColor = oldSettings.PanelLeftColor
        _newSettings.PanelBottomRightColor = oldSettings.PanelBottomRightColor
        _newSettings.PanelTopLeftColor = oldSettings.PanelTopLeftColor
        _newSettings.PanelOutlineColor = oldSettings.PanelOutlineColor
        _newSettings.PanelBackgroundColor = oldSettings.PanelBackgroundColor
        _newSettings.LastMarketPriceUpdate = oldSettings.LastMarketPriceUpdate
        _newSettings.LastFactionPriceUpdate = oldSettings.LastFactionPriceUpdate
        For i As Integer = 1 To 4
            _newSettings.EveFolderLua(i) = oldSettings.EveFolderLUA(i)
        Next
        _newSettings.CycleG15Time = oldSettings.CycleG15Time
        _newSettings.CycleG15Pilots = oldSettings.CycleG15Pilots
        _newSettings.ActivateG15 = oldSettings.ActivateG15
        _newSettings.AutoAPI = oldSettings.AutoAPI
        For i As Integer = 0 To 4
            _newSettings.MainFormPosition(i) = oldSettings.MainFormPosition(i)
        Next
        _newSettings.DeleteSkills = oldSettings.DeleteSkills
        _newSettings.PartialTrainColor = oldSettings.PartialTrainColor
        _newSettings.ReadySkillColor = oldSettings.ReadySkillColor
        _newSettings.IsPreReqColor = oldSettings.IsPreReqColor
        _newSettings.HasPreReqColor = oldSettings.HasPreReqColor
        _newSettings.BothPreReqColor = oldSettings.BothPreReqColor
        _newSettings.DtClashColor = oldSettings.DTClashColor
        _newSettings.ColorHighlightQueuePreReq = oldSettings.ColorHighlightQueuePreReq
        _newSettings.ColorHighlightQueueTraining = oldSettings.ColorHighlightQueueTraining
        _newSettings.ColorHighlightPilotTraining = oldSettings.ColorHighlightPilotTraining
        _newSettings.ContinueTraining = oldSettings.ContinueTraining
        _newSettings.EMailPassword = oldSettings.EMailPassword
        _newSettings.EMailUsername = oldSettings.EMailUsername
        _newSettings.UseSsl = oldSettings.UseSSL
        _newSettings.UseSmtpAuth = oldSettings.UseSMTPAuth
        _newSettings.EMailAddress = oldSettings.EMailAddress
        _newSettings.EMailPort = oldSettings.EMailPort
        _newSettings.EMailServer = oldSettings.EMailServer
        _newSettings.NotifySoundFile = oldSettings.NotifySoundFile
        _newSettings.NotifyOffset = oldSettings.NotifyOffset
        _newSettings.NotifyEarly = oldSettings.NotifyEarly
        _newSettings.NotifyNow = oldSettings.NotifyNow
        _newSettings.NotifySound = oldSettings.NotifySound
        _newSettings.NotifyEMail = oldSettings.NotifyEMail
        _newSettings.NotifyDialog = oldSettings.NotifyDialog
        _newSettings.NotifyToolTip = oldSettings.NotifyToolTip
        _newSettings.ShutdownNotifyPeriod = oldSettings.ShutdownNotifyPeriod
        _newSettings.ShutdownNotify = oldSettings.ShutdownNotify
        _newSettings.ServerOffset = oldSettings.ServerOffset
        _newSettings.EnableEveStatus = oldSettings.EnableEveStatus
        _newSettings.ProxyUseDefault = oldSettings.ProxyUseDefault
        _newSettings.ProxyUseBasic = oldSettings.ProxyUseBasic
        _newSettings.ProxyPassword = oldSettings.ProxyPassword
        _newSettings.ProxyUsername = oldSettings.ProxyUsername
        _newSettings.ProxyPort = oldSettings.ProxyPort
        _newSettings.ProxyServer = oldSettings.ProxyServer
        _newSettings.ProxyRequired = oldSettings.ProxyRequired
        _newSettings.IgbPort = oldSettings.IGBPort
        _newSettings.IgbAutoStart = oldSettings.IGBAutoStart
        _newSettings.IgbFullMode = oldSettings.IGBFullMode
        _newSettings.IgbAllowedData = oldSettings.IGBAllowedData
        _newSettings.AutoHide = oldSettings.AutoHide
        _newSettings.AutoStart = oldSettings.AutoStart
        _newSettings.AutoCheck = oldSettings.AutoCheck
        _newSettings.MinimiseExit = oldSettings.MinimiseExit
        _newSettings.AutoMinimise = oldSettings.AutoMinimise
        _newSettings.StartupPilot = oldSettings.StartupPilot
        _newSettings.StartupView = oldSettings.StartupView
        For i As Integer = 1 To 4
            _newSettings.EveFolder(i) = oldSettings.EveFolder(i)
        Next
        _newSettings.BackupAuto = oldSettings.BackupAuto
        _newSettings.BackupStart = oldSettings.BackupStart
        _newSettings.BackupFreq = oldSettings.BackupFreq
        _newSettings.BackupLast = oldSettings.BackupLast
        _newSettings.BackupLastResult = oldSettings.BackupLastResult
        _newSettings.QColumnsSet = oldSettings.QColumnsSet
        For i As Integer = 0 To 20
            For j As Integer = 0 To 1
                _newSettings.QColumns(i, j) = oldSettings.QColumns(i, j)
            Next
        Next
        _newSettings.DBFormat = oldSettings.DBFormat
        _newSettings.DBFilename = oldSettings.DBFilename
        _newSettings.DBName = oldSettings.DBName
        _newSettings.DBServer = oldSettings.DBServer
        _newSettings.DBUsername = oldSettings.DBUsername
        _newSettings.DBPassword = oldSettings.DBPassword
        _newSettings.DbSqlSecurity = oldSettings.DBSQLSecurity
        '_newSettings.Accounts = oldSettings.Accounts
        '_newSettings.Pilots = oldSettings.Pilots
        '_newSettings.Plugins = oldSettings.Plugins
        _newSettings.MarketRegions = oldSettings.MarketRegions
        _newSettings.MarketSystem = oldSettings.MarketSystem
        _newSettings.MarketUseRegionMarket = oldSettings.MarketUseRegionMarket
        _newSettings.MarketDefaultMetric = oldSettings.MarketDefaultMetric
        _newSettings.MarketDataUploadEnabled = oldSettings.MarketDataUploadEnabled
        _newSettings.MarketStatOverrides = oldSettings.MarketStatOverrides
        _newSettings.MarketDefaultTransactionType = oldSettings.MarketDefaultTransactionType
    End Sub

    Private Sub ConvertAccounts(ByVal oldSettings As EveSettings)
        _newSettings.Accounts.Clear()
        For Each account As EveAccount In oldSettings.Accounts
            Dim newAccount As New EveHQAccount
            newAccount.AccessMask = account.AccessMask
            newAccount.APIAccountStatus = account.APIAccountStatus
            newAccount.APIKey = account.APIKey
            newAccount.ApiKeyExpiryDate = account.APIKeyExpiryDate
            newAccount.ApiKeySystem = account.APIKeySystem
            newAccount.APIKeyType = account.APIKeyType
            newAccount.Characters.Clear()
            For Each name As String In account.Characters
                newAccount.Characters.Add(name)
            Next
            newAccount.CorpApiAccountKey = account.CorpAPIAccountKey
            newAccount.CreateDate = account.CreateDate
            newAccount.FailedAttempts = account.FailedAttempts
            newAccount.FriendlyName = account.FriendlyName
            newAccount.LastAccountStatusCheck = account.LastAccountStatusCheck
            newAccount.LogonCount = account.LogonCount
            newAccount.LogonMinutes = account.LogonMinutes
            newAccount.PaidUntil = account.PaidUntil
            newAccount.UserID = account.userID
            _newSettings.Accounts.Add(newAccount.UserID, newAccount)
        Next
    End Sub

    Private Sub ConvertPilots(ByVal oldSettings As EveSettings)

        _newSettings.Pilots.Clear()
        For Each pilot As Pilot In oldSettings.Pilots
            Dim newPilot As New EveHQPilot

            newPilot.Name = pilot.Name
            newPilot.ID = pilot.ID
            newPilot.Account = pilot.Account
            newPilot.AccountPosition = pilot.AccountPosition
            newPilot.Race = pilot.Race
            newPilot.Blood = pilot.Blood
            newPilot.Gender = pilot.Gender
            newPilot.Corp = pilot.Corp
            newPilot.CorpID = pilot.CorpID
            newPilot.Isk = pilot.Isk
            newPilot.CloneName = pilot.CloneName
            newPilot.CloneSP = pilot.CloneSP
            newPilot.SkillPoints = pilot.SkillPoints
            newPilot.Training = pilot.Training
            newPilot.TrainingStartTime = pilot.TrainingStartTime
            newPilot.TrainingStartTimeActual = pilot.TrainingStartTimeActual
            newPilot.TrainingEndTime = pilot.TrainingEndTime
            newPilot.TrainingEndTimeActual = pilot.TrainingEndTimeActual
            newPilot.TrainingSkillID = pilot.TrainingSkillID
            newPilot.TrainingSkillName = pilot.TrainingSkillName
            newPilot.TrainingStartSP = pilot.TrainingStartSP
            newPilot.TrainingEndSP = pilot.TrainingEndSP
            newPilot.TrainingCurrentSP = pilot.TrainingCurrentSP
            newPilot.TrainingCurrentTime = pilot.TrainingCurrentTime
            newPilot.TrainingSkillLevel = pilot.TrainingSkillLevel
            newPilot.TrainingNotifiedNow = pilot.TrainingNotifiedNow
            newPilot.TrainingNotifiedEarly = pilot.TrainingNotifiedEarly
            newPilot.CAtt = pilot.CAtt
            newPilot.IAtt = pilot.IAtt
            newPilot.MAtt = pilot.MAtt
            newPilot.PAtt = pilot.PAtt
            newPilot.WAtt = pilot.WAtt
            newPilot.CImplant = pilot.CImplant
            newPilot.IImplant = pilot.IImplant
            newPilot.MImplant = pilot.MImplant
            newPilot.PImplant = pilot.PImplant
            newPilot.WImplant = pilot.WImplant
            newPilot.CImplantA = pilot.CImplantA
            newPilot.IImplantA = pilot.IImplantA
            newPilot.MImplantA = pilot.MImplantA
            newPilot.PImplantA = pilot.PImplantA
            newPilot.WImplantA = pilot.WImplantA
            newPilot.CImplantM = pilot.CImplantM
            newPilot.IImplantM = pilot.IImplantM
            newPilot.MImplantM = pilot.MImplantM
            newPilot.PImplantM = pilot.PImplantM
            newPilot.WImplantM = pilot.WImplantM
            newPilot.UseManualImplants = pilot.UseManualImplants
            newPilot.CAttT = pilot.CAttT
            newPilot.IAttT = pilot.IAttT
            newPilot.MAttT = pilot.MAttT
            newPilot.PAttT = pilot.PAttT
            newPilot.WAttT = pilot.WAttT
            ConvertPilotSkills(pilot, newPilot)
            ConvertPilotQueuedSkills(pilot, newPilot)
            newPilot.QueuedSkillTime = pilot.QueuedSkillTime
            newPilot.Certificates.Clear()
            For Each c As String In pilot.Certificates
                newPilot.Certificates.Add(Convert.ToInt32(c))
            Next
            newPilot.PrimaryQueue = pilot.PrimaryQueue
            ConvertTrainingQueues(pilot, newPilot)
            newPilot.ActiveQueue = ConvertPilotSkillQueue(pilot.ActiveQueue)
            newPilot.ActiveQueueName = pilot.ActiveQueueName
            newPilot.CacheFileTime = pilot.CacheFileTime
            newPilot.CacheExpirationTime = pilot.CacheExpirationTime
            newPilot.TrainingFileTime = pilot.TrainingFileTime
            newPilot.TrainingExpirationTime = pilot.TrainingExpirationTime
            newPilot.Updated = pilot.Updated
            newPilot.LastUpdate = pilot.LastUpdate
            newPilot.Active = pilot.Active
            For index As Integer = 0 To 53
                newPilot.KeySkills(index) = CInt(pilot.KeySkills(index))
            Next
            newPilot.Standings = pilot.Standings
            newPilot.CorpRoles = pilot.CorpRoles
            _newSettings.Pilots.Add(newPilot.Name, newPilot)
        Next
    End Sub

    Private Sub ConvertPilotSkills(ByVal pilot As Pilot, ByVal newPilot As EveHQPilot)
        newPilot.PilotSkills.Clear()
        For Each oldskill As PilotSkill In pilot.PilotSkills
            Dim newSkill As New EveHQPilotSkill
            newSkill.ID = oldskill.ID
            newSkill.Name = oldskill.Name
            newSkill.GroupID = oldskill.GroupID
            newSkill.Flag = oldskill.Flag
            newSkill.Rank = oldskill.Rank
            newSkill.SP = oldskill.SP
            newSkill.Level = oldskill.Level
            For index As Integer = 0 To 5
                newSkill.LevelUp(index) = oldskill.LevelUp(index)
            Next
            newPilot.PilotSkills.Add(newSkill.Name, newSkill)
        Next
    End Sub

    Private Sub ConvertPilotQueuedSkills(ByVal pilot As Pilot, ByVal newPilot As EveHQPilot)
        newPilot.QueuedSkills.Clear()
        For Each oldskill As PilotQueuedSkill In pilot.QueuedSkills.Values
            Dim newSkill As New EveHQPilotQueuedSkill
            newSkill.Position = oldskill.Position
            newSkill.SkillID = oldskill.SkillID
            newSkill.Level = oldskill.Level
            newSkill.StartSP = oldskill.StartSP
            newSkill.EndSP = oldskill.EndSP
            newSkill.StartTime = oldskill.StartTime
            newSkill.EndTime = oldskill.EndTime
            newPilot.QueuedSkills.Add(newSkill.Position, newSkill)
        Next
    End Sub

    Private Sub ConvertTrainingQueues(ByVal pilot As Pilot, ByVal newPilot As EveHQPilot)
        newPilot.TrainingQueues.Clear()
        For Each oldQueue As SkillQueue In pilot.TrainingQueues.Values
            newPilot.TrainingQueues.Add(oldQueue.Name, ConvertPilotSkillQueue(oldQueue))
        Next
    End Sub

    Private Function ConvertPilotSkillQueue(oldQueue As SkillQueue) As EveHQSkillQueue
        Dim newQueue As New EveHQSkillQueue
        newQueue.Name = oldQueue.Name
        newQueue.IncCurrentTraining = oldQueue.IncCurrentTraining
        newQueue.Primary = oldQueue.Primary
        newQueue.QueueSkills = oldQueue.QueueSkills
        newQueue.QueueTime = oldQueue.QueueTime
        newQueue.Queue.Clear()
        For Each oldQueueItem As SkillQueueItem In oldQueue.Queue
            Dim newQueueItem As New EveHQSkillQueueItem
            newQueueItem.Name = oldQueueItem.Name
            newQueueItem.FromLevel = oldQueueItem.FromLevel
            newQueueItem.ToLevel = oldQueueItem.ToLevel
            newQueueItem.Pos = oldQueueItem.Pos
            newQueueItem.Priority = oldQueueItem.Priority
            newQueueItem.Notes = oldQueueItem.Notes
            newQueue.Queue.Add(newQueueItem.Key, newQueueItem)
        Next
        Return newQueue
    End Function

    Private Sub ConvertPlugins(ByVal oldSettings As EveSettings)
        _newSettings.Plugins.Clear()
        For Each plugin As PlugIn In oldSettings.Plugins.Values
            Dim newPlugin As New EveHQPlugInStatus
            newPlugin.Name = plugin.Name
            newPlugin.Disabled = plugin.Disabled
            _newSettings.Plugins.Add(newPlugin.Name, newPlugin)
        Next
    End Sub

    'Private Sub ConvertDashboard(ByVal oldSettings As EveSettings)

    'End Sub

End Class
