' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2008  Lee Vessey
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
Imports System.IO
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient

Public Class frmDataConvert
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object
    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return mSetPlugInData
        End Get
        Set(ByVal value As Object)
            mSetPlugInData = value
        End Set
    End Property

    Dim WithEvents DataWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker
    Dim startTime, endTime As DateTime
    Dim elapsedTime As TimeSpan
    Dim convertType As Integer
    Dim SQLSecurity As Boolean
    Dim server As String
    Dim userName As String
    Dim password As String
    Dim conversionSuccess As Boolean = False
    Dim fileList As New ArrayList

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Return True
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Data Converter"
        EveHQPlugIn.Description = "Converts the Eve Data Export files to various formats and groups of tables"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "Data Converter"
        EveHQPlugIn.RunAtStartup = False
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.plugin_icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function

    Public Function IGBService(ByVal IGBContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function

    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return Me
    End Function

    Private Function ParseLine(ByVal oneLine As String) As String()
        ' Returns an array containing the values of the comma-separated fields.
        ' This pattern actually recognizes the correct commas.
        ' The Regex.Split() command later gets text between the commas.
        Dim pattern As String = ",(?=(?:[^']*'[^']*')*(?![^']*'))"
        Dim r As System.Text.RegularExpressions.Regex = _
                New System.Text.RegularExpressions.Regex(pattern)
        Return r.Split(oneLine)
    End Function
    Private Function AnalyseSQLFiles(ByVal worker As System.ComponentModel.BackgroundWorker, ByVal e As System.ComponentModel.DoWorkEventArgs) As Long
        Dim dataFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        dataFiles = My.Computer.FileSystem.GetFiles(txtSource.Text)
        Dim totalFiles As Integer = dataFiles.Count
        Dim fileCount As Integer = 0
        Dim inputFile As String = ""
        Dim totalLines As Long = 0
        For Each inputFile In dataFiles
            Dim lineMatch As New Regex("INSERT\sINTO|CREATE\sTABLE")
            Dim inputFileInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(inputFile)
            Dim fileName As String = inputFileInfo.Name
            If fileList.Count = 0 Or fileList.Contains(fileName) Then
                Dim sr As StreamReader = New StreamReader(inputFile)
                Do
                    Dim file As String = sr.ReadLine
                    If file IsNot Nothing Then
                        Dim lineMatches As MatchCollection = lineMatch.Matches(file)
                        totalLines += lineMatches.Count
                    End If
                Loop Until sr.EndOfStream
                sr.Close()
            End If
            fileCount += 1
            worker.ReportProgress(CInt(fileCount / totalFiles * 100))
        Next
        Return totalLines
    End Function
    Private Sub CreateFileList()
        Select Case cboConvertType.SelectedIndex
            Case 1
                fileList.Add("dbo_dgmAttributeTypes.sql")
                fileList.Add("dbo_dgmTypeAttributes.sql")
                fileList.Add("dbo_eveGraphics.sql")
                fileList.Add("dbo_eveUnits.sql")
                fileList.Add("dbo_invBlueprintTypes.sql")
                fileList.Add("dbo_invCategories.sql")
                fileList.Add("dbo_invGroups.sql")
                fileList.Add("dbo_invMetaGroups.sql")
                fileList.Add("dbo_invMetaTypes.sql")
                fileList.Add("dbo_invTypes.sql")
                fileList.Add("typeActivityMaterials.sql")
            Case 2
                fileList.Add("dbo_chrAncestries.sql")
                fileList.Add("dbo_chrBloodlines.sql")
                fileList.Add("dbo_chrCareers.sql")
                fileList.Add("dbo_chrCareerSkills.sql")
                fileList.Add("dbo_chrCareerSpecialities.sql")
                fileList.Add("dbo_chrCareerSpecialitySkills.sql")
                fileList.Add("dbo_chrRaces.sql")
                fileList.Add("dbo_chrRaceSkills.sql")
                fileList.Add("dbo_dgmAttributeTypes.sql")
                fileList.Add("dbo_dgmTypeAttributes.sql")
                fileList.Add("dbo_eveGraphics.sql")
                fileList.Add("dbo_eveUnits.sql")
                fileList.Add("dbo_invBlueprintTypes.sql")
                fileList.Add("dbo_invCategories.sql")
                fileList.Add("dbo_invGroups.sql")
                fileList.Add("dbo_invMetaGroups.sql")
                fileList.Add("dbo_invMetaTypes.sql")
                fileList.Add("dbo_invTypes.sql")
                fileList.Add("typeActivityMaterials.sql")
                fileList.Add("dbo_mapConstellations.sql")
                fileList.Add("dbo_mapRegions.sql")
                fileList.Add("dbo_mapSolarSystemJumps.sql")
                fileList.Add("dbo_mapSolarSystems.sql")
                fileList.Add("dbo_mapDenormalize.sql")
            Case 0
                fileList.Clear()
        End Select
    End Sub

#Region "UI Routines"
    Private Sub btnSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSource.Click
        With fbd1
            .Description = "Please select the source folder where the T-SQL files are located."
            .ShowNewFolderButton = True
            .RootFolder = Environment.SpecialFolder.Desktop
            .ShowDialog()
            txtSource.Text = .SelectedPath
        End With
    End Sub
    Private Sub btnConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConvert.Click
        ' Check for existence of the directories we are going to be working with
        If My.Computer.FileSystem.DirectoryExists(txtSource.Text) = False Then
            MessageBox.Show("Source directory is not valid or does not exist!", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If cboConvert.SelectedIndex = 0 Or cboConvert.SelectedIndex = 1 Then
            If My.Computer.FileSystem.DirectoryExists(txtTarget.Text) = False Then
                MessageBox.Show("Target directory is not valid or does not exist!", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If
        Select Case convertType
            Case 2, 3
                server = txtMSSQLServer.Text
                userName = txtMSSQLUser.Text
                password = txtMSSQLPassword.Text
                SQLSecurity = radMSSQLDatabase.Checked
            Case 4
                server = txtMySQLServer.Text
                userName = txtMySQLUser.Text
                password = txtMySQLPassword.Text
                SQLSecurity = False
        End Select
        btnConvert.Enabled = False
        btnCancel.Enabled = True
        DataWorker.WorkerReportsProgress = True
        DataWorker.WorkerSupportsCancellation = True
        DataWorker.RunWorkerAsync()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DataWorker.CancelAsync()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim line As String = My.Resources.materialsForRefining.Substring(50, 50)
        Dim msg As String = ""
        For a As Integer = 0 To 49
            msg &= a & ": " & Asc(line.Substring(a, 1))
            If Asc(line.Substring(a, 1)) > 32 Then
                msg &= line.Substring(a, 1)
            End If
            msg &= ControlChars.CrLf
        Next
        MsgBox(msg)
    End Sub
    Private Sub btnTarget_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTarget.Click
        With fbd1
            .Description = "Please select the target folder where the new data files will be saved."
            .ShowNewFolderButton = True
            .RootFolder = Environment.SpecialFolder.Desktop
            .ShowDialog()
            txtTarget.Text = .SelectedPath
        End With
    End Sub
    Private Sub txtSource_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSource.TextChanged
        If My.Computer.FileSystem.DirectoryExists(txtSource.Text) = True Then
            Dim noOfFiles As Integer = My.Computer.FileSystem.GetFiles(txtSource.Text).Count
            lblFiles.Text = "Number of files in directory: " & noOfFiles
        Else
            lblFiles.Text = "Number of files in directory: n/a"
        End If
    End Sub
    Private Sub cboConvert_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConvert.SelectedIndexChanged
        convertType = cboConvert.SelectedIndex
        If convertType = 0 Or convertType = 1 Then
            btnConvert.Enabled = True
            btnTarget.Enabled = True
            txtTarget.Enabled = True
            lblTarget.Enabled = True
            grpMySQL.Visible = False
            grpMSSQL.Visible = False
        Else
            btnConvert.Enabled = True
            btnTarget.Enabled = False
            txtTarget.Enabled = False
            lblTarget.Enabled = False
            If convertType = 2 Or convertType = 3 Then
                grpMSSQL.Visible = True
                grpMySQL.Visible = False
            Else
                grpMSSQL.Visible = False
                grpMySQL.Visible = True
            End If
        End If
    End Sub
    Private Sub radSQLWindows_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radMSSQLWindows.CheckedChanged
        If radMSSQLWindows.Checked = True Then
            txtMSSQLUser.Visible = False
            txtMSSQLPassword.Visible = False
        Else
            txtMSSQLUser.Visible = True
            txtMSSQLPassword.Visible = True
        End If
    End Sub
    Private Sub cboConvertType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboConvertType.SelectedIndexChanged
        Call CreateFileList()
    End Sub
#End Region
#Region "Worker Routines"
    Private Sub DataWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles DataWorker.DoWork
        Dim totalLines As Long = 0
        totalLines = AnalyseSQLFiles(DataWorker, e)
        startTime = Now
        If totalLines = 0 Then
            MessageBox.Show("Error analysing data files...aborting.", "Error in Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            Select Case convertType
                Case 0
                    Call ConvertToMDB(totalLines, DataWorker, e)
                Case 1
                    Call ConvertToCSV(totalLines, DataWorker, e)
                Case 2
                    Call ConvertToSQL(False, totalLines, DataWorker, e)
                Case 3
                    Call ConvertToSQL(True, totalLines, DataWorker, e)
                Case 4
                    Call ConvertToMySQL(totalLines, DataWorker, e)
            End Select
        End If
    End Sub
    Private Sub DataWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles DataWorker.ProgressChanged
        pbProgress.Value = e.ProgressPercentage
    End Sub
    Private Sub DataWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles DataWorker.RunWorkerCompleted
        If conversionSuccess = True Then
            endTime = Now
            elapsedTime = endTime.Subtract(startTime)
            MessageBox.Show("Conversion complete in " & EveHQ.Core.SkillFunctions.TimeToString(elapsedTime.TotalSeconds), "Conversion Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnConvert.Enabled = True
            btnCancel.Enabled = False
            pbProgress.Value = 0
        Else
            MessageBox.Show("Conversion Failed", "Conversion Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnConvert.Enabled = True
            btnCancel.Enabled = False
            pbProgress.Value = 0
        End If
    End Sub
#End Region
#Region "Create SQL Data"
    Private Sub ConvertToSQL(ByVal SQLExpress As Boolean, ByVal totallines As Long, ByVal worker As System.ComponentModel.BackgroundWorker, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Dim connection As SqlConnection
        Try
            Dim dataFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
            dataFiles = My.Computer.FileSystem.GetFiles(txtSource.Text)
            Dim inputFile As String = ""

            Dim strConn As String
            If CreateSQLDB("EveHQ", SQLExpress) = False Then Exit Sub
            strConn = "Server=" & server
            If SQLExpress = True Then
                strConn &= "\SQLEXPRESS"
            End If
            If SQLSecurity = True Then
                strConn += "; Database = EveHQ; User ID=" & userName & "; Password=" & password & ";"
            Else
                strConn += "; Database = EveHQ; Integrated Security = SSPI;"
            End If
            Dim lineCount As Long = 0

            connection = New SqlConnection(strConn)
            connection.Open()
            ' Write the table definitions from internal resources
            Dim tableData As String = My.Resources.dbo_TABLES_SQL
            Dim command As New SqlCommand(tableData, connection)
            command.ExecuteNonQuery()

            ' Now write the data
            For Each inputFile In dataFiles
                Dim tblName As String = ""
                Dim keyName As String = ""
                Dim keyColumn As String = ""
                Dim inputFileInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(inputFile)
                Dim fileName As String = inputFileInfo.Name
                If fileList.Count = 0 Or fileList.Contains(fileName) Then
                    Dim sr As StreamReader = New StreamReader(inputFile)

                    ' Start main iteration thru files
                    Do
                        If worker.CancellationPending = True Then
                            connection.Close()
                            e.Cancel = True
                        Else
                            ' Get a T-SQL "line"
                            Dim oline As String = ""
                            Dim line As String = ""
                            Do
                                line &= sr.ReadLine & ControlChars.CrLf
                            Loop Until line.EndsWith(";" & ControlChars.CrLf) Or sr.EndOfStream = True
                            lineCount += 1
                            oline = line
                            ' Replace the dbo bits of the tables
                            line = line.Replace("dbo.", "")
                            line = line.Replace("dbo_", "")
                            line = line.Replace(".csv", "")
                            ' Remove the "COMMIT;" bits
                            line = line.Replace("COMMIT;", "")
                            line = line.Replace(",True", ",1")
                            line = line.Replace(",False", ",0")
                            ' Replace blank entries with "null"
                            Do While line.Contains(",,") = True
                                line = line.Replace(",,", ",null,")
                            Loop
                            line = line.Replace("'6/13/2007 11:01:00 AM'", "null")
                            line = line.Replace(",);", ",null);")

                            If line.EndsWith(";" & ControlChars.CrLf) = True And line.StartsWith(ControlChars.CrLf & "INSERT") = True Then
                                command = New SqlCommand(line, connection)
                                command.ExecuteNonQuery()
                                If Int(lineCount / 1000) = lineCount / 1000 Then
                                    worker.ReportProgress(CInt(lineCount / totallines * 100))
                                End If
                            End If
                        End If
                        'End If
                    Loop Until sr.EndOfStream = True
                    GC.Collect()
                End If
            Next
            Call AddSQLRefiningData(connection)
            Call AddSQLAttributeGroupColumn(connection)
            Call CorrectSQLEveUnits(connection)
            connection.Close()
            conversionSuccess = True
        Catch ex As Exception
            Dim msg As String = "There was an error converting the data." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "The error was: " & ex.Message
            MessageBox.Show(msg, "Error Converting to MS SQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conversionSuccess = False
            Exit Sub
        End Try

    End Sub
    Private Function CreateSQLDB(ByVal dbName As String, ByVal SQLExpress As Boolean) As Boolean

        Dim conn As SqlConnection
        Dim strConn As String
        strConn = "Server = " & server
        If SQLExpress = True Then
            strConn &= "\SQLEXPRESS"
        End If
        If SQLSecurity = True Then
            strConn += "; Database = ;User ID=" & userName & ";Password=" & password & ";"
        Else
            strConn += "; Database = ; Integrated Security = SSPI;"
        End If
        conn = New SqlConnection(strConn)
        Try
            conn.Open()
        Catch e As Exception
            Dim msg As String = "There was an error connecting to the SQL Server on " & server & "." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Please ensure the server name, username and password are correct." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "The error was: " & e.Message
            MessageBox.Show(msg, "Error Connecting to SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conversionSuccess = False
            Return False
            Exit Function
        End Try
        Dim strSQL As String
        strSQL = "if Exists (Select * From master..sysdatabases Where Name = '" & dbName & "')"
        strSQL &= " DROP DATABASE " & dbName & vbCrLf & " CREATE DATABASE " & dbName
        Dim cmd As New SqlCommand(strSQL, conn)
        cmd.CommandType = CommandType.Text
        Try
            cmd.ExecuteNonQuery()
        Catch e As Exception
            MessageBox.Show("Error Creating SQL Database: " & e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conversionSuccess = False
            Return False
            Exit Function
        Finally
            cmd.Dispose()
        End Try
        conn.Close()
        Return True

    End Function
    Private Sub AddSQLPrimaryKeys(ByVal connection As SqlConnection)

        Dim strLines() As String = My.Resources.DB_Primary.Split(ControlChars.CrLf)
        Dim command As SqlCommand
        Dim strSQL As String = ""
        Dim strLinks(1) As String
        For Each strLine As String In strLines
            Try
                strLinks = strLine.Split(",")
                If strLinks(1).Trim <> "<none>" Then
                    strSQL = "ALTER TABLE " & strLinks(0).Trim & " ALTER COLUMN " & strLinks(1).Trim & " INTEGER NOT NULL;"
                    command = New SqlCommand(strSQL, connection)
                    command.ExecuteNonQuery()
                    strSQL = "ALTER TABLE " & strLinks(0).Trim & " ADD PRIMARY KEY (" & strLinks(1).Trim & ");"
                    command = New SqlCommand(strSQL, connection)
                    command.ExecuteNonQuery()
                End If
            Catch ex As Exception

            End Try
        Next

    End Sub
    Private Sub CreateSQLRelationships(ByVal connection As SqlConnection)

        Dim strLines() As String = My.Resources.DB_Links.Split(ControlChars.CrLf)
        Dim command As SqlCommand
        Dim strSQL As String = ""
        Dim strLinks(3) As String
        For Each strLine As String In strLines
            Try
                strLinks = strLine.Split(",")
                strSQL = "ALTER TABLE " & strLinks(0).Trim & " ADD FOREIGN KEY (" & strLinks(1).Trim & ") REFERENCES " & strLinks(2).Trim
                command = New SqlCommand(strSQL, connection)
                command.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        Next

    End Sub
    Private Sub AddSQLRefiningData(ByVal connection As SqlConnection)
        Dim line As String = My.Resources.materialsForRefining.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("typeID") = False And line <> "" Then
                Dim strSQL As String = "INSERT INTO typeActivityMaterials (typeID,activityID,requiredTypeID,quantity,damagePerJob) VALUES(" & line & ");"
                Dim keyCommand As New SqlCommand(strSQL, connection)
                keyCommand.ExecuteNonQuery()
            End If
        Next
    End Sub
    Private Sub AddSQLAttributeGroupColumn(ByVal connection As SqlConnection)
        Dim strSQL As String = "ALTER TABLE dgmAttributeTypes ADD attributeGroup INTEGER DEFAULT 0;"
        Dim keyCommand As New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        strSQL = "UPDATE dgmAttributeTypes SET attributeGroup=0;"
        keyCommand = New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        Dim line As String = My.Resources.attributeGroups.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("attributeID") = False And line <> "" Then
                Dim fields() As String = line.Split(",")
                Dim strSQL2 As String = "UPDATE dgmAttributeTypes SET attributeGroup=" & fields(1) & " WHERE attributeID=" & fields(0) & ";"
                Dim keyCommand2 As New SqlCommand(strSQL2, connection)
                keyCommand2.ExecuteNonQuery()
            End If
        Next
    End Sub
    Private Sub CorrectSQLEveUnits(ByVal connection As SqlConnection)
        Dim strSQL As String = "UPDATE dgmAttributeTypes SET unitID=122 WHERE unitID IS NULL;"
        Dim keyCommand As New SqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
    End Sub

#End Region
#Region "Create CSV Data"
    Private Sub ConvertToCSV(ByVal totalLines As Long, ByVal worker As System.ComponentModel.BackgroundWorker, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Try
            Dim dataFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
            dataFiles = My.Computer.FileSystem.GetFiles(txtSource.Text)

            Dim inputFile As String = ""

            Dim lineCount As Long = 0
            For Each inputFile In dataFiles
                Dim inputFileInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(inputFile)
                Dim fileName As String = inputFileInfo.Name
                If fileList.Count = 0 Or fileList.Contains(fileName) Then
                    Dim outputFile As String = txtTarget.Text & "\" & inputFileInfo.Name & ".csv"
                    Dim sr As StreamReader = New StreamReader(inputFile)
                    Dim sw As StreamWriter = New StreamWriter(outputFile, False)

                    ' Read the first 2 lines and pick out the field names
                    Dim tableHeader As String = sr.ReadLine
                    If tableHeader.StartsWith("INSERT") = True Then
                        tableHeader = sr.ReadLine
                        Dim loc As Long = 0
                        Dim loc2 As Long = 0
                        loc = InStr(tableHeader, "(", CompareMethod.Text)
                        loc2 = InStr(tableHeader, ")", CompareMethod.Text)
                        tableHeader = tableHeader.Substring(loc, loc2 - loc - 1)
                        sw.WriteLine(tableHeader)
                        sw.Flush()
                    End If

                    ' Now iterate thru rest of the file
                    Dim line As String = ""
                    Do
                        line = ""
                        Do
                            line &= sr.ReadLine & ControlChars.CrLf
                            If line.StartsWith("VALUES") = False Then
                                line = ""
                            End If
                        Loop Until line.EndsWith(";" & ControlChars.CrLf) = True Or sr.EndOfStream
                        If line.EndsWith("COMMIT;") = False Then
                            line = line.TrimStart("VALUES(".ToCharArray)
                            line = line.TrimEnd((");" & ControlChars.Cr & ControlChars.CrLf).ToCharArray)
                            line = line.Replace("'", "$")
                            line = line.Replace("""", "'")
                            line = line.Replace("$", """")
                            If line <> "" Then
                                sw.WriteLine(line)
                            End If
                            lineCount += 1
                            If Int(lineCount / 1000) = lineCount / 1000 Then
                                worker.ReportProgress(CInt(lineCount / totalLines * 100))
                            End If
                        Else
                            Exit Do
                        End If
                    Loop Until sr.EndOfStream

                    sw.Flush()
                    sw.Close()
                    sr.Close()
                    GC.Collect()
                End If
            Next
            Call AddCSVRefiningData()
            conversionSuccess = True
        Catch ex As Exception
            Dim msg As String = "There was an error converting the data." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "The error was: " & ex.Message & ControlChars.CrLf
            MessageBox.Show(msg, "Error Converting to CSV", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conversionSuccess = False
            Exit Sub
        End Try

    End Sub
    Private Sub AddCSVRefiningData()
        Dim line As String = My.Resources.materialsForRefining.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        Dim sw As StreamWriter = New StreamWriter(txtTarget.Text & "\dbo_typeActivityMaterials.sql.csv", True)
        ' Read each line and write if not header
        For Each line In lines
            If line.StartsWith("typeID") = False And line <> "" Then
                sw.WriteLine(line)
            End If
        Next
        sw.Flush()
        sw.Close()
        GC.Collect()
    End Sub
#End Region
#Region "Create MDB Data"
    Private Sub ConvertToMDB(ByVal totalLines As Long, ByVal worker As System.ComponentModel.BackgroundWorker, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Dim dataFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        dataFiles = My.Computer.FileSystem.GetFiles(txtSource.Text)
        Dim outputFile As String = txtTarget.Text & "\EveHQ.mdb"

        Dim inputFile As String = ""
        Dim lineCount As Long = 0

        ' Check for the existence of the target mdb and delete it
        ' We delete it cos we are rebuilding from scratch!
        If My.Computer.FileSystem.FileExists(outputFile) = True Then
            My.Computer.FileSystem.DeleteFile(outputFile, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        End If

        ' Try to create a new access db from resources
        Dim fs As New FileStream(outputFile, FileMode.Create)
        Dim bw As New BinaryWriter(fs)
        Try
            bw.Write(My.Resources.EveHQDB)
            bw.Close()
            fs.Close()
        Catch Excep As System.Runtime.InteropServices.COMException
            MessageBox.Show("Unable to create database in " & ControlChars.CrLf & ControlChars.CrLf & outputFile, "Error Creating Access DB", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Finally
            fs.Dispose()
        End Try

        Dim connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & outputFile & "'")
        Dim command As OleDbCommand
        connection.Open()

        ' Write the table definitions from internal resources
        Dim tableData As String = My.Resources.dbo_TABLES_MDB
        Dim tables() As String = tableData.Split(";")
        For Each table As String In tables
            table = table.Replace("dbo.", "")
            table = table.Replace(ControlChars.CrLf, "")
            If table.StartsWith("CREATE") = True Then
                command = New OleDbCommand(table, connection)
                command.ExecuteNonQuery()
            End If
        Next

        For Each inputFile In dataFiles
            Dim tblName As String = ""
            Dim keyName As String = ""
            Dim keyColumn As String = ""
            Dim inputFileInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(inputFile)
            Dim fileName As String = inputFileInfo.Name
            If fileList.Count = 0 Or fileList.Contains(fileName) Then
                Dim sr As StreamReader = New StreamReader(inputFile)

                Do
                    If worker.CancellationPending = True Then
                        connection.Close()
                        e.Cancel = True
                    Else
                        ' Get a T-SQL "line"
                        Dim line As String = ""
                        Do
                            line &= sr.ReadLine & ControlChars.CrLf
                        Loop Until line.EndsWith(";" & ControlChars.CrLf) Or sr.EndOfStream = True
                        lineCount += 1
                        ' Replace the dbo bits of the tables
                        line = line.Replace("dbo.", "")
                        line = line.Replace("dbo_", "")
                        line = line.Replace(".csv", "")
                        ' Remove the "COMMIT;" bits
                        line = line.Replace("COMMIT;", "")
                        line = line.Replace(",True", ",1")
                        line = line.Replace(",False", ",0")
                        If line.StartsWith(ControlChars.CrLf & "INSERT INTO") = True Then
                            Dim tbLoc1 As Integer = InStr(line, "(")
                            Dim tbLoc2 As Integer = InStr(line, ")")
                            Dim tbText As String = line.Substring(tbLoc1 - 1, tbLoc2 - tbLoc1 + 1)
                            line = line.Replace(tbText, "")
                            Do While line.Contains(",,") = True
                                line = line.Replace(",,", ",null,")
                            Loop
                            line = line.Replace("'6/13/2007 11:01:00 AM'", "null")
                            line = line.Replace(",);", ",null);")
                        End If
                        'End If

                        If line.EndsWith(";" & ControlChars.CrLf) = True And line.StartsWith(ControlChars.CrLf & "INSERT") = True Then
                            command = New OleDbCommand(line, connection)
                            If connection.State <> ConnectionState.Open Then
                                connection.Open()
                            End If
                            command.ExecuteNonQuery()
                            If Int(lineCount / 1000) = lineCount / 1000 Then
                                worker.ReportProgress(CInt(lineCount / totalLines * 100))
                            End If
                        End If
                    End If
                Loop Until sr.EndOfStream = True
                connection.Close()
                'End If
            End If
        Next
        Call CreateRelationships()
        Call AddMDBRefiningData()
        Call AddMDBAttributeGroupColumn()
        Call CorrectMDBEveUnits()
        conversionSuccess = True

    End Sub
    Private Sub AddPrimaryKeys()
        Dim outputFile As String = txtTarget.Text & "\EveHQ.mdb"
        Dim connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & outputFile & "'")
        Dim strLines() As String = My.Resources.DB_Primary.Split(ControlChars.CrLf)
        Dim command As OleDbCommand
        Dim strSQL As String = ""
        Dim strLinks(1) As String
        connection.Open()
        For Each strLine As String In strLines
            Try
                strLinks = strLine.Split(",")
                If strLinks(1).Trim <> "<none>" Then
                    strSQL = "ALTER TABLE " & strLinks(0).Trim & " ADD PRIMARY KEY (" & strLinks(1).Trim & ");"
                    command = New OleDbCommand(strSQL, connection)
                    command.ExecuteNonQuery()
                End If
            Catch ex As Exception

            End Try
        Next
        ' Close the connection
        connection.Close()
    End Sub
    Private Sub CreateRelationships()
        Dim outputFile As String = txtTarget.Text & "\EveHQ.mdb"
        Dim connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & outputFile & "'")
        Dim strLines() As String = My.Resources.DB_Links.Split(ControlChars.CrLf)
        Dim command As OleDbCommand
        Dim strSQL As String = ""
        Dim strLinks(3) As String
        connection.Open()
        For Each strLine As String In strLines
            Try
                strLinks = strLine.Split(",")
                strSQL = "ALTER TABLE " & strLinks(0).Trim & " ADD FOREIGN KEY (" & strLinks(1).Trim & ") REFERENCES " & strLinks(2).Trim
                command = New OleDbCommand(strSQL, connection)
                command.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        Next

        ' Close the connection
        connection.Close()

    End Sub
    Private Sub AddMDBRefiningData()
        Dim line As String = My.Resources.materialsForRefining.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        Dim outputFile As String = txtTarget.Text & "\EveHQ.mdb"
        Dim connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & outputFile & "'")
        connection.Open()
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("typeID") = False And line <> "" Then
                Dim strSQL As String = "INSERT INTO typeActivityMaterials (typeID,activityID,requiredTypeID,quantity,damagePerJob) VALUES(" & line & ");"
                Dim keyCommand As New OleDbCommand(strSQL, connection)
                keyCommand.ExecuteNonQuery()
            End If
        Next
        connection.Close()
    End Sub
    Private Sub AddMDBAttributeGroupColumn()
        Dim outputFile As String = txtTarget.Text & "\EveHQ.mdb"
        Dim connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & outputFile & "'")
        connection.Open()
        Dim strSQL As String = "ALTER TABLE dgmAttributeTypes ADD COLUMN attributeGroup INTEGER;"
        Dim keyCommand As New OleDbCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        strSQL = "UPDATE dgmAttributeTypes SET attributeGroup=0;"
        keyCommand = New OleDbCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        Dim line As String = My.Resources.attributeGroups.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("attributeID") = False And line <> "" Then
                Dim fields() As String = line.Split(",")
                Dim strSQL2 As String = "UPDATE dgmAttributeTypes SET attributeGroup=" & fields(1) & " WHERE attributeID=" & fields(0) & ";"
                Dim keyCommand2 As New OleDbCommand(strSQL2, connection)
                keyCommand2.ExecuteNonQuery()
            End If
        Next
        connection.Close()
    End Sub
    Private Sub CorrectMDBEveUnits()
        Dim outputFile As String = txtTarget.Text & "\EveHQ.mdb"
        Dim connection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & outputFile & "'")
        connection.Open()
        Dim strSQL As String = "UPDATE dgmAttributeTypes SET unitID=122 WHERE unitID IS NULL;"
        Dim keyCommand As New OleDbCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        connection.Close()
    End Sub

#End Region
#Region "Create MySQL Data"
    Private Sub ConvertToMySQL(ByVal totallines As Long, ByVal worker As System.ComponentModel.BackgroundWorker, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Dim dataFiles As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        dataFiles = My.Computer.FileSystem.GetFiles(txtSource.Text)

        Try
            Dim inputFile As String = ""
            Dim strConn As String
            If CreateMySQLDB("EveHQ") = False Then Exit Sub
            strConn = "Server=" & server & ";Database=EveHQ;Uid=" & userName & ";Pwd=" & password & ";"
            Dim lineCount As Long = 0
            Dim connection As MySqlConnection
            connection = New MySqlConnection(strConn)
            connection.Open()
            ' Write the table definitions from internal resources
            Dim tableData As String = My.Resources.dbo_TABLES_MDB
            ' Alter a couple of the definitions
            tableData = tableData.Replace("dbo.", "")
            tableData = tableData.Replace("memo", "text")
            tableData = tableData.Replace("textry", "memory")
            Dim command As New MySqlCommand(tableData, connection)
            command.ExecuteNonQuery()

            For Each inputFile In dataFiles
                Dim tblName As String = ""
                Dim keyName As String = ""
                Dim keyColumn As String = ""
                Dim inputFileInfo As FileInfo = My.Computer.FileSystem.GetFileInfo(inputFile)
                Dim fileName As String = inputFileInfo.Name
                If fileList.Count = 0 Or fileList.Contains(fileName) Then
                    Dim sr As StreamReader = New StreamReader(inputFile)

                    ' Start main iteration thru files
                    Do
                        If worker.CancellationPending = True Then
                            connection.Close()
                            e.Cancel = True
                        Else
                            ' Get a T-SQL "line"
                            Dim oline As String = ""
                            Dim line As String = ""
                            Do
                                line &= sr.ReadLine & ControlChars.CrLf
                            Loop Until line.EndsWith(";" & ControlChars.CrLf) Or sr.EndOfStream = True
                            lineCount += 1
                            oline = line
                            ' Replace the dbo bits of the tables
                            line = line.Replace("dbo.", "")
                            line = line.Replace("dbo_", "")
                            line = line.Replace(".csv", "")
                            ' Remove the "COMMIT;" bits
                            line = line.Replace("COMMIT;", "")
                            line = line.Replace(",True", ",1")
                            line = line.Replace(",False", ",0")
                            ' Replace blank entries with "null"
                            Do While line.Contains(",,") = True
                                line = line.Replace(",,", ",null,")
                            Loop
                            line = line.Replace("'6/13/2007 11:01:00 AM'", "null")
                            line = line.Replace(",);", ",null);")

                            If line.EndsWith(";" & ControlChars.CrLf) = True And line.StartsWith(ControlChars.CrLf & "INSERT") = True Then
                                command = New MySqlCommand(line, connection)
                                command.ExecuteNonQuery()
                                If Int(lineCount / 1000) = lineCount / 1000 Then
                                    worker.ReportProgress(CInt(lineCount / totallines * 100))
                                End If
                            End If
                        End If
                    Loop Until sr.EndOfStream = True

                    GC.Collect()
                End If
            Next
            Call CreateMySQLRelationships(connection)
            Call AddMySQLRefiningData(connection)
            Call AddMySQLAttributeGroupColumn(connection)
            Call CorrectMySQLEveUnits(connection)
            connection.Close()
            conversionSuccess = True
        Catch ex As Exception
            Dim msg As String = "There was an error converting the data." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "The error was: " & ex.Message
            MessageBox.Show(msg, "Error Converting to MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conversionSuccess = False
            Exit Sub
        End Try
    End Sub
    Private Function CreateMySQLDB(ByVal dbName As String) As Boolean

        Dim conn As MySqlConnection
        Dim strConn As String
        strConn = "Server=" & server & ";Uid=" & userName & ";Pwd=" & password & ";"
        conn = New MySqlConnection(strConn)
        Try
            conn.Open()
        Catch e As Exception
            Dim msg As String = "There was an error connecting to the MySQL Server on " & server & "." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "Please ensure the server name, username and password are correct." & ControlChars.CrLf & ControlChars.CrLf
            msg &= "The error was: " & e.Message
            MessageBox.Show(msg, "Error Connecting to MySQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conversionSuccess = False
            Return False
            Exit Function
        End Try
        Dim strSQL As String = ""
        strSQL = "DROP DATABASE " & dbName & "; CREATE DATABASE " & dbName & ";"
        Dim cmd As New MySqlCommand(strSQL, conn)
        cmd.CommandType = CommandType.Text
        Try
            cmd.ExecuteNonQuery()
        Catch e As Exception
            MessageBox.Show("Error Creating MySQL Database: " & e.Message, "Error Creating MySQL Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            conversionSuccess = False
            Return False
            Exit Function
        Finally
            cmd.Dispose()
        End Try
        conn.Close()
        Return True

    End Function
    Private Sub AddMySQLPrimaryKeys(ByVal connection As MySqlConnection)

        Dim strLines() As String = My.Resources.DB_Primary.Split(ControlChars.CrLf)
        Dim command As MySqlCommand
        Dim strSQL As String = ""
        Dim strLinks(1) As String
        For Each strLine As String In strLines
            Try
                strLinks = strLine.Split(",")
                If strLinks(1).Trim <> "<none>" Then
                    strSQL = "ALTER TABLE " & strLinks(0).Trim & " ADD PRIMARY KEY (" & strLinks(1).Trim & ");"
                    command = New MySqlCommand(strSQL, connection)
                    command.ExecuteNonQuery()
                End If
            Catch ex As Exception

            End Try
        Next

    End Sub
    Private Sub CreateMySQLRelationships(ByVal connection As MySqlConnection)

        Dim strLines() As String = My.Resources.DB_Links.Split(ControlChars.CrLf)
        Dim command As MySqlCommand
        Dim strSQL As String = ""
        Dim strLinks(3) As String
        For Each strLine As String In strLines
            Try
                strLinks = strLine.Split(",")
                strSQL = "ALTER TABLE " & strLinks(0).Trim & " ADD FOREIGN KEY (" & strLinks(1).Trim & ") REFERENCES " & strLinks(2).Trim & "(" & strLinks(3).Trim & ");"
                command = New MySqlCommand(strSQL, connection)
                command.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        Next
    End Sub
    Private Sub AddMySQLRefiningData(ByVal connection As MySqlConnection)
        Dim line As String = My.Resources.materialsForRefining.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("typeID") = False And line <> "" Then
                Dim strSQL As String = "INSERT INTO typeActivityMaterials (typeID,activityID,requiredTypeID,quantity,damagePerJob) VALUES(" & line & ");"
                Dim keyCommand As New MySqlCommand(strSQL, connection)
                keyCommand.ExecuteNonQuery()
            End If
        Next
    End Sub
    Private Sub AddMySQLAttributeGroupColumn(ByVal connection As MySqlConnection)
        Dim strSQL As String = "ALTER TABLE dgmAttributeTypes ADD attributeGroup INTEGER DEFAULT 0;"
        Dim keyCommand As New MySqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        strSQL = "UPDATE dgmAttributeTypes SET attributeGroup=0;"
        keyCommand = New MySqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
        Dim line As String = My.Resources.attributeGroups.Replace(ControlChars.CrLf, Chr(13))
        Dim lines() As String = line.Split(Chr(13))
        ' Read the first line which is a header line
        For Each line In lines
            If line.StartsWith("attributeID") = False And line <> "" Then
                Dim fields() As String = line.Split(",")
                Dim strSQL2 As String = "UPDATE dgmAttributeTypes SET attributeGroup=" & fields(1) & " WHERE attributeID=" & fields(0) & ";"
                Dim keyCommand2 As New MySqlCommand(strSQL2, connection)
                keyCommand2.ExecuteNonQuery()
            End If
        Next
    End Sub
    Private Sub CorrectMySQLEveUnits(ByVal connection As MySqlConnection)
        Dim strSQL As String = "UPDATE dgmAttributeTypes SET unitID=122 WHERE unitID IS NULL;"
        Dim keyCommand As New MySqlCommand(strSQL, connection)
        keyCommand.ExecuteNonQuery()
    End Sub

#End Region

    Private Sub btnMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMap.Click

        Select Case cboConvert.SelectedIndex
            Case 2, 3
                server = txtMSSQLServer.Text
                userName = txtMSSQLUser.Text
                password = txtMSSQLPassword.Text
                SQLSecurity = radMSSQLDatabase.Checked
            Case 4
                server = txtMySQLServer.Text
                userName = txtMySQLUser.Text
                password = txtMySQLPassword.Text
                SQLSecurity = False
        End Select

        Dim strTable As String = "CREATE TABLE mapSystemSummary "
        strTable &= "("
        strTable &= "solarSystemID               integer             NOT NULL,"
        strTable &= "solarSystemName             text(100)	         NOT NULL,"
        strTable &= "constellationID             integer             NOT NULL,"
        strTable &= "constellationName           text(100)	         NOT NULL,"
        strTable &= "regionID                    integer             NOT NULL,"
        strTable &= "regionName                  text(100)	         NOT NULL,"
        strTable &= "security                    double              NOT NULL DEFAULT 0.0,"
        strTable &= "planets                     integer             NULL,"
        strTable &= "moons                       integer             NULL,"
        strTable &= "aBelts                      integer             NULL,"
        strTable &= "iBelts                      integer             NULL,"
        strTable &= "gates                       integer             NULL,"
        strTable &= "stations                    integer             NULL"
        strTable &= ");"

        Dim inputFile As String = ""
        Dim strConn As String
        Dim command As MySqlCommand
        strConn = "Server=" & server & ";Database=EveHQ;Uid=" & userName & ";Pwd=" & password & ";"
        Dim connection As MySqlConnection
        connection = New MySqlConnection(strConn)
        connection.Open()
        ' Write the table definition
        command = New MySqlCommand(strTable, connection)
        command.ExecuteNonQuery()

        Dim newData As New DataSet
        Dim sysID As String = ""
        'Dim strSQL As String = "SELECT * FROM mapSolarSystems"
        Dim strSQL As String = "SELECT mapSolarSystems.regionID AS mapSolarSystems_regionID, mapSolarSystems.constellationID AS mapSolarSystems_constellationID, mapSolarSystems.solarSystemID, mapSolarSystems.solarSystemName, mapSolarSystems.x, mapSolarSystems.y, mapSolarSystems.z, mapSolarSystems.xMin, mapSolarSystems.xMax, mapSolarSystems.yMin, mapSolarSystems.yMax, mapSolarSystems.zMin, mapSolarSystems.zMax, mapSolarSystems.luminosity, mapSolarSystems.border, mapSolarSystems.fringe, mapSolarSystems.corridor, mapSolarSystems.hub, mapSolarSystems.international, mapSolarSystems.regional, mapSolarSystems.constellation, mapSolarSystems.security, mapSolarSystems.factionID, mapSolarSystems.radius, mapSolarSystems.sunTypeID, mapSolarSystems.securityClass, mapRegions.regionID AS mapRegions_regionID, mapRegions.regionName, mapConstellations.constellationID AS mapConstellations_constellationID, mapConstellations.constellationName"
        strSQL &= " FROM (mapRegions INNER JOIN mapConstellations ON mapRegions.regionID = mapConstellations.regionID) INNER JOIN mapSolarSystems ON mapConstellations.constellationID = mapSolarSystems.constellationID;"
        Dim sysSQL As String = ""
        Dim newSQL As String = ""
        Dim planets, moons, aBelts, gates, iBelts, stations As Integer
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        For row As Integer = 0 To systemData.Tables(0).Rows.Count - 1
            sysID = systemData.Tables(0).Rows(row).Item("solarsystemID").ToString
            sysSQL = "SELECT * FROM mapDenormalize WHERE solarsystemID=" & sysID
            newData = EveHQ.Core.DataFunctions.GetData(sysSQL)
            planets = 0     'groupID=7
            moons = 0       'groupID=8
            aBelts = 0      'groupID=9, typeID=15
            iBelts = 0      'groupID=9, typeID=17774
            gates = 0       'groupID=10
            stations = 0     'groupID=15
            For row2 As Integer = 0 To newData.Tables(0).Rows.Count - 1
                Select Case newData.Tables(0).Rows(row2).Item("groupID").ToString
                    Case 7
                        planets += 1
                    Case 8
                        moons += 1
                    Case 9
                        Select Case newData.Tables(0).Rows(row2).Item("typeID").ToString
                            Case 15
                                aBelts += 1
                            Case 17774
                                iBelts += 1
                        End Select
                    Case 10
                        gates += 1
                    Case 15
                        stations += 1
                End Select
            Next
            newSQL = "INSERT INTO mapSystemSummary (solarSystemID, solarSystemName, constellationID, constellationName, regionID, regionName, security, planets, moons, aBelts, iBelts, gates, stations) VALUES("
            newSQL &= sysID & ", "
            newSQL &= "'" & systemData.Tables(0).Rows(row).Item("solarSystemName").ToString & "', "
            newSQL &= systemData.Tables(0).Rows(row).Item("mapSolarSystems_constellationID").ToString & ", "
            newSQL &= "'" & systemData.Tables(0).Rows(row).Item("constellationName").ToString & "', "
            newSQL &= systemData.Tables(0).Rows(row).Item("mapSolarSystems_regionID").ToString & ", "
            newSQL &= "'" & systemData.Tables(0).Rows(row).Item("regionName").ToString & "', "
            newSQL &= systemData.Tables(0).Rows(row).Item("security").ToString & ", "
            newSQL &= planets & ", "
            newSQL &= moons & ", "
            newSQL &= aBelts & ", "
            newSQL &= iBelts & ", "
            newSQL &= gates & ", "
            newSQL &= stations & ");"
            command = New MySqlCommand(newSQL, connection)
            command.ExecuteNonQuery()
        Next
        connection.Close()
    End Sub

    Private Sub btnBPS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBPS.Click
        Dim bpSQL As String = "SELECT * FROM invblueprinttypes INNER JOIN invTypes ON productTypeID=typeID WHERE published=true;"
        Dim newData As DataSet = EveHQ.Core.DataFunctions.GetData(bpSQL)
        Dim materialData As DataSet = New DataSet

        Dim sw As StreamWriter = New StreamWriter("c:\eveHQ_bpDump.csv")
        Dim sw2 As StreamWriter = New StreamWriter("c:\eveHQ_bpDump2.csv")
        Dim strBPS As String = ""
        strBPS = "Item Type,Tech Level,Production Time,Research Productivity Time,Research Material Time,Research Copy Time,Research Tech Time,Productivity Modifier,Material Modifier,Waste Factor,Max Production Limit"
        sw.WriteLine(strBPS)

        For row As Integer = 0 To newData.Tables(0).Rows.Count - 1
            Dim typeID As String = newData.Tables(0).Rows(row).Item("typeID")
            Dim bpTypeID As String = newData.Tables(0).Rows(row).Item("blueprintTypeID")
            strBPS = newData.Tables(0).Rows(row).Item("typeName") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("techLevel") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("productionTime") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("researchProductivityTime") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("researchMaterialTime") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("researchCopyTime") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("researchTechTime") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("productivityModifier") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("materialModifier") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("wasteFactor") & ","
            strBPS &= newData.Tables(0).Rows(row).Item("maxProductionLimit")
            Dim strSQL As String = "SELECT *"
            strSQL &= " FROM ((invCategories INNER JOIN invGroups ON invCategories.categoryID = invGroups.categoryID) INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN typeActivityMaterials ON invTypes.typeID = typeActivityMaterials.requiredTypeID"
            strSQL &= " WHERE (typeActivityMaterials.typeID=" & bpTypeID & " OR typeActivityMaterials.typeID=" & typeID & ") ORDER BY invCategories.categoryName, invGroups.groupName"
            materialData = EveHQ.Core.DataFunctions.GetData(strSQL)
            For row2 As Integer = 0 To materialData.Tables(0).Rows.Count - 1
                Dim strMats As String = ""
                If materialData.Tables(0).Rows(row2).Item("activityID") = 1 Then
                    strMats &= newData.Tables(0).Rows(row).Item("typeName") & ","
                    strMats &= materialData.Tables(0).Rows(row2).Item("activityID") & ","
                    strMats &= materialData.Tables(0).Rows(row2).Item("typeName") & ","
                    strMats &= materialData.Tables(0).Rows(row2).Item("quantity") & ","
                    strMats &= materialData.Tables(0).Rows(row2).Item("damagePerJob")
                    sw2.WriteLine(strMats)
                End If
            Next
            sw.WriteLine(strBPS)
        Next
        sw.Flush() : sw.Close()
        sw2.Flush() : sw2.Close()
        MessageBox.Show("Finished BP Dump")
    End Sub

    Private Sub btnCSVDump_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCSVDump.Click
        Dim csvPath As String = ""
        With fbd1
            .Description = "Please select the destination folder for the CSV dump..."
            .ShowNewFolderButton = True
            .RootFolder = Environment.SpecialFolder.MyDocuments
            .ShowDialog()
            csvPath = .SelectedPath
        End With
        Dim strSQL As String = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_SCHEMA='eveHQ'"
        Dim newData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Dim tableNames As New ArrayList
        For row As Integer = 0 To newData.Tables(0).Rows.Count - 1
            tableNames.Add(newData.Tables(0).Rows(row).Item("TABLE_NAME"))
        Next
        Dim strRow As String = ""
        Dim colRow As String = ""
        For Each tableName As String In tableNames
            strSQL = "SELECT * FROM " & tableName & ";"
            newData = EveHQ.Core.DataFunctions.GetData(strSQL)
            Dim newFile As String = csvPath & "\" & tableName & ".csv"
            Dim sw As StreamWriter = New StreamWriter(newFile)
            ' Write column headings
            colRow = ""
            For col As Integer = 0 To newData.Tables(0).Columns.Count - 1
                colRow &= newData.Tables(0).Columns(col).ColumnName & ","
            Next
            colRow = colRow.Trim(",")
            sw.WriteLine(colRow)
            ' Write rows
            Dim strData As String = ""
            For row As Integer = 0 To newData.Tables(0).Rows.Count - 1
                strRow = ""
                For col As Integer = 0 To newData.Tables(0).Columns.Count - 1
                    strData = newData.Tables(0).Rows(row).Item(col).ToString.Replace("""", "'")
                    strRow &= """" & strData & ""","
                Next
                strRow = strRow.Trim(",")
                sw.WriteLine(strRow)
            Next
            sw.Flush()
            sw.Close()
        Next
        MessageBox.Show("CSV Dump Completed!")
    End Sub

    Private Sub btnAddToMySQL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddToMySQL.Click
        Dim strConn As String = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & ";Database=EveHQ;Uid=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & ";Pwd=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
        Dim lineCount As Long = 0
        Dim connection As MySqlConnection
        connection = New MySqlConnection(strConn)
        connection.Open()
        Call AddMySQLRefiningData(connection)
        Call AddMySQLAttributeGroupColumn(connection)
        Call CorrectMySQLEveUnits(connection)
        connection.Close()
        MessageBox.Show("Additions to MySQL Database Complete")
    End Sub
End Class
