Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Windows.Forms

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn

    Dim mSetPlugInData As Object
    Public Shared AllStandings As New SortedList

#Region "Plug-in Initialisation Routines"
    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Return True
    End Function
    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "CorpHQ"
        EveHQPlugIn.Description = "Corporation Management"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "CorpHQ"
        EveHQPlugIn.RunAtStartup = False
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.Plugin_Icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function
    Public Function IGBService(ByVal context As System.Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function
    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return New frmCorpHQ
    End Function
    Public Function GetPlugInData(ByVal Data As Object, Optional ByVal DataType As Integer = 0) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Select Case DataType
            Case 0 ' Return a standing
                ' Check the data in an Arraylist and contains 2 items - pilotName and corpID
                If TypeOf (Data) Is ArrayList Then
                    Dim StandingsRequest As ArrayList = CType(Data, ArrayList)
                    If StandingsRequest.Count = 2 Then
                        Dim pilotName As String = CStr(StandingsRequest(0))
                        Dim corpID As String = CStr(StandingsRequest(1))
                        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(pilotName) = True Then
                            ' Check if the standings are loaded
                            If AllStandings.Count > 0 Then
                                Return FindStanding(pilotName, corpID)
                            Else
                                ' try to load standings and try again
                                LoadStandings()
                                If AllStandings.Count > 0 Then
                                    Return FindStanding(pilotName, corpID)
                                Else
                                    Return 0
                                End If
                            End If
                        Else
                            Return 0
                        End If
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If
        End Select
        Return Nothing
    End Function
#End Region

#Region "Plug-in Supplementary Routines"

    Shared Sub LoadStandings()
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\Standings.bin") = True Then
            Dim s As New FileStream(EveHQ.Core.HQ.cacheFolder & "\Standings.bin", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            PlugInData.AllStandings.Clear()
            Try
                PlugInData.AllStandings = CType(f.Deserialize(s), SortedList)
            Catch e As Exception
                MessageBox.Show("There was an error retrieving the cached standings file, please obtain a new set of standings.", "Load Standings Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                PlugInData.AllStandings.Clear()
            End Try
            s.Close()
        End If
    End Sub
    Private Function FindStanding(ByVal ownerName As String, ByVal corpID As String) As Double
        Dim DiplomacyLevel As Integer = 0
        Dim ConnectionsLevel As Integer = 0
        Dim standing As Double = 0
        ' Iterate through the list and find the rightID
        For Each MyStandings As StandingsData In PlugInData.AllStandings.Values
            If ownerName = MyStandings.OwnerName Then
                ' Check if this is a character and whether we need to get the Connections and Diplomacy skills
                If MyStandings.CacheType = "GetCharStandings" Then
                    Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(ownerName), Core.Pilot)
                    For Each cSkill As EveHQ.Core.Skills In cPilot.PilotSkills
                        If cSkill.Name = "Diplomacy" Then
                            DiplomacyLevel = cSkill.Level
                        End If
                        If cSkill.Name = "Connections" Then
                            ConnectionsLevel = cSkill.Level
                        End If
                    Next
                Else
                    DiplomacyLevel = 0
                    ConnectionsLevel = 0
                End If
                ' Get the standing directly
                Try
                    If MyStandings.StandingValues.ContainsKey(corpID) Then
                        standing = CDbl(MyStandings.StandingValues(corpID))
                        If CLng(corpID) < 70000000 Then
                            If standing < 0 Then
                                standing = standing + ((10 - standing) * (DiplomacyLevel * 4 / 100))
                            Else
                                standing = standing + ((10 - standing) * (ConnectionsLevel * 4 / 100))
                            End If
                        End If
                        Return standing
                    Else
                        Return standing
                    End If
                Catch e As Exception
                    Return 0
                End Try
            End If
        Next
    End Function
#End Region



End Class
