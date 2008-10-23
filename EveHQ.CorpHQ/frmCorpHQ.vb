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
Imports System.Windows.Forms
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmCorpHQ
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object
    Dim AllStandings As New SortedList

#Region "Plug-in Initialisation Routines"
    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Return True
    End Function
    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ CorpHQ"
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
        Return Me
    End Function
    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return mSetPlugInData
        End Get
        Set(ByVal value As Object)
            mSetPlugInData = value
        End Set
    End Property
#End Region

#Region "Form/Standings Loading and Unloading"

    Private Sub frmCorpHQ_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call Me.LoadStandings()
        Call Me.UpdateOwners()
    End Sub

    Private Sub frmCorpHQ_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Call Me.SaveStandings()
    End Sub

    Private Sub LoadStandings()
        If My.Computer.FileSystem.FileExists(EveHQ.Core.HQ.cacheFolder & "\Standings.bin") = True Then
            Dim s As New FileStream(EveHQ.Core.HQ.cacheFolder & "\Standings.bin", FileMode.Open)
            Dim f As BinaryFormatter = New BinaryFormatter
            AllStandings.Clear()
            Try
                AllStandings = CType(f.Deserialize(s), SortedList)
            Catch e As Exception
                MessageBox.Show("There was an error retrieving the cached standings file, please obtain a new set of standings.", "Load Standings Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                AllStandings.Clear()
            End Try
            s.Close()
        End If
    End Sub
    Private Sub SaveStandings()
        Dim s As New FileStream(EveHQ.Core.HQ.cacheFolder & "\Standings.bin", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, AllStandings)
        s.Close()
    End Sub
#End Region

#Region "Standings Parser"
    Private Sub btnGetStandings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetStandings.Click
        ' First, let's check out the cache location based on the value of the settings
        Dim cacheFileList As New ArrayList
        Dim baseCacheDir As String = ""
        Dim EveClient As DirectoryInfo
        Dim cacheDir As String = ""
        Dim folder As String = ""
        Dim cacheFileData As String = ""
        Dim CharStandingsRegex As New System.Text.RegularExpressions.Regex("GetCharStandings.*", (RegexOptions.Compiled Or RegexOptions.IgnoreCase))
        Dim CorpStandingsRegex As New System.Text.RegularExpressions.Regex("GetCorpStandings.*", (RegexOptions.Compiled Or RegexOptions.IgnoreCase))
        Dim MyStandings As New StandingsData

        For folderNo As Integer = 1 To 4
            If EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo) <> "" Then
                ' Define the base cache dir depending on the /LUA switch
                If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folderNo) = True Then
                    EveClient = New DirectoryInfo(EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo))
                    baseCacheDir = EveClient.FullName
                    cacheDir = baseCacheDir & "\cache\machonet\87.237.38.200\"
                Else
                    baseCacheDir = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\CCP\EVE\")
                    ' Check the location
                    folder = EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo)
                    folder = folder.Replace(":", "")
                    folder = folder.Replace(" ", "_")
                    folder = folder.Replace("\", "_")
                    cacheDir = baseCacheDir & folder & "_tranquility\cache\machonet\87.237.38.200\"
                End If
                ' Search the cache dir for files containing the text "GetCharStandings"
                If My.Computer.FileSystem.DirectoryExists(cacheDir) = True Then
                    Dim fileError As Boolean = False
                    For Each cacheFile As String In My.Computer.FileSystem.GetFiles(cacheDir, FileIO.SearchOption.SearchAllSubDirectories, "*.cache")
                        Dim sr As New StreamReader(cacheFile)
                        Try
                            ' Load the contents of the cachefile to check for text
                            cacheFileData = sr.ReadToEnd
                            sr.Close()
                            Dim cacheMatch As MatchCollection = CharStandingsRegex.Matches(cacheFileData)
                            If cacheMatch.Count > 0 And cacheFile.Contains("CachedObjects") = True Then
                                cacheFileList.Add(cacheFile)
                            End If
                            cacheMatch = CorpStandingsRegex.Matches(cacheFileData)
                            If cacheMatch.Count > 0 And cacheFile.Contains("CachedObjects") = True Then
                                cacheFileList.Add(cacheFile)
                            End If
                        Catch ex As Exception
                            fileError = True
                        Finally
                            ' Try and close the stream but ignore if it's already closed (and therefore errors)
                            Try
                                sr.Close()
                            Catch ex As Exception
                            End Try
                        End Try
                    Next
                    If fileError = True Then
                        Dim msg As String = "There were some errors reading some applicable cache files. This could be due to corruption or some other problem."
                        msg &= "As a result, all the standings information may not be present or correct." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "If this problem persists, consider clearing your EveHQ cache folder."
                        MessageBox.Show(msg, "Errors Found In Cache Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    MessageBox.Show("Unable to locate cache folder, please check the location of your Eve Client and/or log in to Eve to create it.", "Missing Cache Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If
        Next

        Dim StandingsDecoder As New StandingsCacheDecoder
        AllStandings.Clear()
        For Each cachefile As String In cacheFileList
            MyStandings = StandingsDecoder.FetchStandings(cachefile)
            If AllStandings.ContainsKey(MyStandings.OwnerID) = False Then
                AllStandings.Add(MyStandings.OwnerID, MyStandings)
            End If
        Next

        Call Me.SaveStandings()
        Call UpdateOwners()

    End Sub
    Private Sub UpdateOwners()
        cboOwner.Items.Clear()
        lvwStandings.Items.Clear()
        If AllStandings.Count > 0 Then
            ' Create the list of owners in the combobox
            cboOwner.BeginUpdate()
            For Each MyStandings As StandingsData In AllStandings.Values
                ' Get Either Pilot or Corp Name
                Dim ownerID As String = MyStandings.OwnerID
                ' Cycle through the pilots to see if we have a match
                For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
                    Select Case MyStandings.CacheType
                        Case "GetCharStandings"
                            If ownerID = cPilot.ID Then
                                If cboOwner.Items.Contains(cPilot.Name) = False Then
                                    cboOwner.Items.Add(cPilot.Name)
                                    MyStandings.OwnerName = cPilot.Name
                                End If
                            End If
                        Case "GetCorpStandings"
                            If ownerID = cPilot.CorpID Then
                                If cboOwner.Items.Contains(cPilot.Corp) = False Then
                                    cboOwner.Items.Add(cPilot.Corp)
                                    MyStandings.OwnerName = cPilot.Corp
                                End If
                            End If
                    End Select
                Next
            Next
            cboOwner.EndUpdate()
            ' Enable everything
            cboOwner.Enabled = True
            lblSelectOwner.Enabled = True
            lvwStandings.Enabled = True
            lblTypeFilter.Enabled = True
            cboFilter.Enabled = True
        Else
            ' Disable everything
            cboOwner.Enabled = False
            lblSelectOwner.Enabled = False
            lvwStandings.Enabled = False
            lblTypeFilter.Enabled = False
            cboFilter.Enabled = False
            MessageBox.Show("Unable to find any valid cache files!", "No Cache Files Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub cboOwner_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOwner.SelectedIndexChanged
        Call UpdateStandingsList()
    End Sub
    Private Sub lvwStandings_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwStandings.ColumnClick
        If CInt(lvwStandings.Tag) = e.Column Then
            Me.lvwStandings.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwStandings.Tag = -1
        Else
            Me.lvwStandings.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwStandings.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwStandings.Sort()
    End Sub
    Private Sub btExportStandings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btExportStandings.Click
        Try
            If cboOwner.SelectedItem IsNot Nothing Then
                If lvwStandings.Items.Count > 0 Then
                    ' Export the current list of standings
                    Dim sw As New StreamWriter(EveHQ.Core.HQ.reportFolder & "\Standings (" & cboOwner.SelectedItem.ToString & ").csv")
                    sw.WriteLine("Standings Export for " & cboOwner.SelectedItem.ToString & " (dated: " & FormatDateTime(Now, DateFormat.GeneralDate) & ")")
                    sw.WriteLine("Entity Name,Entity ID,Entity Type,Raw Standing Value,Actual Standing Value")
                    For Each iStanding As ListViewItem In lvwStandings.Items
                        sw.WriteLine(iStanding.Text & "," & iStanding.SubItems(1).Text & "," & iStanding.SubItems(2).Text & "," & iStanding.SubItems(3).Text & "," & iStanding.SubItems(4).Text)
                    Next
                    sw.Flush()
                    sw.Close()
                    MessageBox.Show("CSV Standings file for " & cboOwner.SelectedItem.ToString & " successfully written to the EveHQ report folder!", "Export Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("There are no standings to export for " & cboOwner.SelectedItem.ToString & "!", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("You need to select an Owner before exporting standings!", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Export of CSV Standings file failed:" & ControlChars.CrLf & ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnClearCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearCache.Click
        Dim msg As String = "This will clear your cache of all files. Please make sure the cache folders are not in use and you are not logged into Eve." & ControlChars.CrLf & ControlChars.CrLf
        msg &= "Are you sure you wish to proceed?"
        Dim reply As Integer = MessageBox.Show(msg, "Confirm Delete Standings Files", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.No Then
            Exit Sub
        Else
            Dim cacheFileList As New ArrayList
            Dim baseCacheDir As String = ""
            Dim EveClient As DirectoryInfo
            Dim cacheDir As String = ""
            Dim folder As String = ""
            Dim fail As Boolean = False
            For folderNo As Integer = 1 To 4
                If EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo) <> "" Then
                    ' Define the base cache dir depending on the /LUA switch
                    If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folderNo) = True Then
                        EveClient = New DirectoryInfo(EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo))
                        baseCacheDir = EveClient.FullName
                        cacheDir = baseCacheDir & "\cache\machonet\87.237.38.200\"
                    Else
                        baseCacheDir = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\CCP\EVE\")
                        ' Check the location
                        folder = EveHQ.Core.HQ.EveHQSettings.EveFolder(folderNo)
                        folder = folder.Replace(":", "")
                        folder = folder.Replace(" ", "_")
                        folder = folder.Replace("\", "_")
                        cacheDir = baseCacheDir & folder & "_tranquility\cache\machonet\87.237.38.200\"
                    End If
                    ' Search the cache dir for files containing the text "GetCharStandings"
                    If My.Computer.FileSystem.DirectoryExists(cacheDir) = True Then
                        Try
                            My.Computer.FileSystem.DeleteDirectory(cacheDir, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                        Catch ex As Exception
                            fail = True
                        End Try
                    End If
                Else
                    ' Do nothing if we can't find a folder! Assume complete!
                End If
            Next
            If fail = False Then
                MessageBox.Show("All Cache files deleted from the cache folders!", "Delete Cache Files Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Unable to delete all Cache folders. Please check that you have the required access, the folder isn't being used and that Eve is not running.", "Delete Cache Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub
    Private Sub cboFilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFilter.SelectedIndexChanged
        If cboFilter.Tag.ToString <> "0" Then
            Call Me.UpdateStandingsList()
        End If
        cboFilter.Tag = "1"
    End Sub
    Private Sub UpdateStandingsList()
        If cboFilter.SelectedItem Is Nothing Then
            cboFilter.SelectedIndex = 0
        End If
        If cboOwner.SelectedItem IsNot Nothing Then
            Dim ownerName As String = cboOwner.SelectedItem.ToString
            Dim DiplomacyLevel As Integer = 0
            Dim ConnectionsLevel As Integer = 0
            Dim standing As Double = 0
            ' Iterate through the list and find the rightID
            For Each MyStandings As StandingsData In AllStandings.Values
                If ownerName = MyStandings.OwnerName Then
                    ' Check if this is a character and whether we need to get the Connections and Diplomacy skills
                    If MyStandings.CacheType = "GetCharStandings" Then
                        Dim cPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.Pilots(ownerName), Core.Pilot)
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
                    ' Populate the standings list
                    lvwStandings.BeginUpdate()
                    lvwStandings.Items.Clear()
                    For Each iStanding As String In MyStandings.StandingValues.Keys
                        ' Determine if we need to filter
                        Dim show As Boolean = False
                        Select Case cboFilter.SelectedItem.ToString
                            Case "<All>"
                                show = True
                            Case "Agent"
                                If CLng(iStanding) >= 3000000 And CLng(iStanding) <= 3099999 Then
                                    show = True
                                End If
                            Case "Corporation"
                                If CLng(iStanding) >= 1000000 And CLng(iStanding) <= 1099999 Then
                                    show = True
                                End If
                            Case "Faction"
                                If CLng(iStanding) >= 500000 And CLng(iStanding) <= 599999 Then
                                    show = True
                                End If
                            Case "Player/Corp"
                                If CLng(iStanding) >= 4000000 Then
                                    show = True
                                End If
                        End Select
                        If show = True Then
                            Dim newStanding As New ListViewItem
                            Try
                                newStanding.Text = MyStandings.StandingNames(iStanding).ToString
                            Catch e As Exception
                                MessageBox.Show("Standing ID: " & iStanding)
                                Try

                                    Dim sw As New StreamWriter(EveHQ.Core.HQ.reportFolder & "\StandingsDump.txt")
                                    For Each rStanding As String In MyStandings.StandingValues.Keys
                                        Try
                                            sw.WriteLine("ID: " & rStanding & " = " & MyStandings.StandingNames(rStanding).ToString)
                                        Catch ex As Exception
                                            sw.WriteLine("ID: " & rStanding & " = <Unrecognised item>")
                                        End Try
                                    Next
                                    sw.Flush()
                                    sw.Close()
                                    MessageBox.Show("StandingNames Dump made - check EveHQ Report Folder for StandingsDump.txt")
                                Catch ex As Exception
                                    MessageBox.Show("Status of StandingNames: uninitialized!")
                                End Try
                            End Try
                            newStanding.SubItems.Add(iStanding.ToString)
                            Select Case CLng(iStanding)
                                Case 500000 To 599999
                                    newStanding.SubItems.Add("Faction")
                                Case 1000000 To 1099999
                                    newStanding.SubItems.Add("Corporation")
                                Case 3000000 To 3099999
                                    newStanding.SubItems.Add("Agent")
                                Case Else
                                    newStanding.SubItems.Add("Player/Corp")
                            End Select
                            standing = CDbl(MyStandings.StandingValues(iStanding))
                            newStanding.SubItems.Add(standing.ToString)
                            If CLng(iStanding) < 70000000 Then
                                If standing < 0 Then
                                    standing = standing + ((10 - standing) * (DiplomacyLevel * 4 / 100))
                                Else
                                    standing = standing + ((10 - standing) * (ConnectionsLevel * 4 / 100))
                                End If
                            End If
                            newStanding.SubItems.Add(standing.ToString)
                            lvwStandings.Items.Add(newStanding)
                        End If
                    Next
                    lvwStandings.EndUpdate()
                End If
            Next
        End If
    End Sub
#End Region

End Class