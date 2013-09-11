Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports EveHQ.EveAPI
Imports Newtonsoft.Json
Imports System.Windows.Forms

Public Class EveSettings2

    Public Shared Sub ExportSettingsToJSON()

        Dim startTime, endTime As DateTime
        Dim timeTaken As TimeSpan

        startTime = Now
        Dim json As String = JsonConvert.SerializeObject(EveHQ.Core.HQ.EveHqSettings, Formatting.Indented)
        endTime = Now
        timeTaken = (endTime - startTime)

        ' HQ.WriteLogEvent("Settings: Saving EveHQ settings to " & Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin"))
        ' Write a JSON version of the settings
        Try
            Dim s As New StreamWriter(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"), False)
            s.Write(json)
            s.Flush()
            s.Close()
            s.Dispose()
            'HQ.WriteLogEvent("Settings: Saved EveHQ settings to " & Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin"))
        Catch e As Exception
            'HQ.WriteLogEvent("Settings: Error saving EveHQ settings to " & Path.Combine(HQ.AppDataFolder, "EveHQSettings.bin - " & e.Message))
        End Try
        ' Update the Proxy Server settings
        'Call InitialiseRemoteProxyServer()
        ' Set Global APIServerInfo
        'HQ.EveHQAPIServerInfo = New APIServerInfo(HQ.EveHqSettings.CCPAPIServerAddress, HQ.EveHqSettings.APIRSAddress, HQ.EveHqSettings.UseAPIRS, HQ.EveHqSettings.UseCCPAPIBackup)

        MessageBox.Show("Conversion and writing complete in " & timeTaken.TotalMilliseconds.ToString("N2") & "ms")

    End Sub

    Public Shared Sub SaveJSONSettings()

    End Sub

    Public Shared Sub LoadJSONSettings()
        
        Try
            Using s As New StreamReader(Path.Combine(HQ.AppDataFolder, "EveHQSettings.json"))
                Dim json As String = s.ReadToEnd
                Dim settings As EveSettings = JsonConvert.DeserializeObject(Of EveSettings)(json)

                If (settings Is Nothing) Then
                    MessageBox.Show("The import of settings failed. The file was read successfully, however the resulting object found was not an EveSettings instance. Please make sure you placed the correct file in the Application data folder.", "Invalid File!", MessageBoxButtons.OK)
                Else
                    ' Because the Pilots collection is not typed the import of the Json uses a custom anonymous type
                    ' we need to fix that.
                    Dim fixedPilots As New Collection
                    For Each pilotImported As Object In settings.Pilots
                        Dim temp As String = JsonConvert.SerializeObject(pilotImported)
                        Dim pilot As Pilot = JsonConvert.DeserializeObject(Of Pilot)(temp)
                        Dim skills As New Collection
                        For Each importedSkill As Object In pilot.PilotSkills
                            temp = JsonConvert.SerializeObject(importedSkill)
                            Dim skill As PilotSkill = JsonConvert.DeserializeObject(Of PilotSkill)(temp)
                            skills.Add(skill, skill.Name)
                        Next
                        pilot.PilotSkills = skills
                        fixedPilots.Add(pilot)
                    Next
                    settings.Pilots = fixedPilots

                    ' Accounts are also a non typed collection
                    Dim fixedAccounts As New Collection
                    For Each accountImported As Object In settings.Accounts
                        Dim temp As String = JsonConvert.SerializeObject(accountImported)
                        fixedAccounts.Add(JsonConvert.DeserializeObject(Of EveAccount)(temp))
                    Next
                    settings.Accounts = fixedAccounts

                    ' Plugins
                    Dim fixedPlugins As New SortedList
                    For Each plugin As String In settings.Plugins.Keys
                        Dim temp As String = JsonConvert.SerializeObject(settings.Plugins(plugin))
                        fixedPlugins.Add(plugin, JsonConvert.DeserializeObject(Of PlugIn)(temp))
                    Next
                    settings.Plugins = fixedPlugins

                    ' Dashboard Config
                    Dim fixedDashboard As New ArrayList
                    For Each config As Object In settings.DashboardConfiguration
                        Dim temp As String = JsonConvert.SerializeObject(config)
                        fixedDashboard.Add(JsonConvert.DeserializeObject(Of SortedList(Of String, Object))(temp))
                    Next

                    settings.DashboardConfiguration = fixedDashboard

                    ' TODO: Make settings more Strongly typed!

                    HQ.EveHqSettings = settings
                End If
                
            End Using

        Catch e As Exception

        End Try

    End Sub

End Class
