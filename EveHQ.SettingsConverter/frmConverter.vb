' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.IO
Imports System.Text
Imports System.Data.SqlServerCe
Imports System.Data.SqlClient

Public Class FrmConverter

    Dim _settingsFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ")
    Dim _isLocal As Boolean
    Dim _dbFormat As DBFormat
    Dim _dbFileName As String = Path.Combine(_settingsFolder, "EveHQData.sdf")
    Dim _dbServerName As String = ""
    Dim _dbDatabase As String = ""
    Dim _dbSQLSec As Boolean = False
    Dim _dbUsername As String = ""
    Dim _dbPassword As String = ""

    Dim WithEvents _conversionWorker As New BackgroundWorker

#Region "Private Properties"

    Private Property IsLocal As Boolean
        Get
            Return _islocal
        End Get
        Set(value As Boolean)
            _isLocal = value
            If _isLocal = True Then
                _settingsFolder = Application.StartupPath
            Else
                _settingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveHQ")
            End If
            _dbFileName = Path.Combine(_settingsFolder, "EveHQData.sdf")
            SetUi()
        End Set
    End Property

#End Region

    Private Sub frmConverter_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Check for application parameters to see if already know the settings
        ' Either way, the user can review these prior to conversion

        If Environment.GetCommandLineArgs.Count > 1 Then
            ' We have some custom data to inspect
            ParseParameters()
            gbApplicationSettings.Text = "EveHQ Application Settings (taken from EveHQ)"
        Else
            ' Select some default values that can be changed later on
            gbApplicationSettings.Text = "EveHQ Application Settings (using default values)"
            IsLocal = False
            _dbFormat = 0
            _dbFileName = Path.Combine(_settingsFolder, "EveHQData.sdf")
            _dbServerName = ""
            _dbDatabase = ""
            _dbSQLSec = False
            _dbUsername = ""
            _dbPassword = ""
        End If

        UpdateServers()

        SetUi()

    End Sub

    Private Sub ParseParameters()
        For Each param As String In Environment.GetCommandLineArgs
           
            ' Check for /local application usage
            If param = "/local" Then
                IsLocal = True
            End If

            ' Check for database format parameter
            If param.StartsWith("/dbformat", StringComparison.Ordinal) Then
                Dim format As Integer
                If Integer.TryParse(param.TrimStart("/dbformat;".ToCharArray), format) = True Then
                    If [Enum].IsDefined(GetType(DBFormat), format) Then
                        _dbFormat = CType(format, DBFormat)
                    Else
                        _dbFormat = DBFormat.Sqlce
                    End If
                Else
                    _dbFormat = DBFormat.Sqlce
                End If
            End If

            ' Check for SQL Server Name
            If param.StartsWith("/dbserver", StringComparison.Ordinal) Then
                _dbServerName = param.TrimStart("/dbserver;".ToCharArray)
            End If

            ' Check for SQL Server Database
            If param.StartsWith("/dbname", StringComparison.Ordinal) Then
                _dbDatabase = param.TrimStart("/dbname;".ToCharArray)
            End If

            ' Check for SQL security
            If param.StartsWith("/dbsqlsec", StringComparison.Ordinal) Then
                Dim sqlsec As Integer
                If Integer.TryParse(param.TrimStart("/dbsqlsec;".ToCharArray), sqlsec) = True Then
                    If sqlsec = 1 Then
                        _dbSQLSec = True
                    Else
                        _dbSQLSec = False
                    End If
                Else
                    _dbSQLSec = False
                End If
            End If

            ' Check for SQL username
            If param.StartsWith("/dbusername", StringComparison.Ordinal) Then
                _dbUsername = param.TrimStart("/dbusername;".ToCharArray)
            End If

            ' Check for SQL password
            If param.StartsWith("/dbpassword", StringComparison.Ordinal) Then
                _dbPassword = param.TrimStart("/dbpassword;".ToCharArray)
            End If

        Next
    End Sub

    Private Sub UpdateServers()
        ' Try and get SQL instances
        cboSQLServer.BeginUpdate()
        cboSQLServer.Items.Clear()
        Dim basekey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
        Dim key As RegistryKey = basekey.OpenSubKey("SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL")
        Dim key2 As RegistryKey = basekey.OpenSubKey("SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL")

        If key IsNot Nothing Then
            For Each s As String In key.GetValueNames()
                cboSQLServer.Items.Add(s)
            Next
            key.Close()
        End If

        If key2 IsNot Nothing Then
            For Each s As String In key2.GetValueNames()
                cboSQLServer.Items.Add(s)
            Next
            key2.Close()
        End If

        basekey.Close()
        cboSQLServer.Sorted = True
        cboSQLServer.EndUpdate()

        If cboSQLServer.Items.Count > 0 Then
            cboSQLServer.SelectedIndex = 0

            ' Check selected server for databases
            UpdateDatabases()

            If cboSQLDatabase.Items.Contains("EveHQData") Then
                cboSQLDatabase.SelectedItem = "EveHQData"
                radSQL.Checked = True
            End If

        End If
    End Sub

    Private Sub UpdateDatabases()
        cboSQLDatabase.BeginUpdate()
        cboSQLDatabase.Items.Clear()
        Const strSQL As String = "SELECT name FROM sys.databases"
        Dim conn As String = "Server=localhost\" & _dbServerName
        If _dbSQLSec = True Then
            conn &= "; User ID=" & _dbUsername & "; Password=" & _dbPassword & ";"
        Else
            conn &= "; Integrated Security = SSPI;"
        End If
        Using eveData As DataSet = GetData(strSQL, DBFormat.Sql, conn)
            If eveData IsNot Nothing Then
                For Each row As DataRow In eveData.Tables(0).Rows
                    cboSQLDatabase.Items.Add(row.Item(0).ToString)
                Next
            End If
        End Using
        cboSQLDatabase.Sorted = True
        cboSQLDatabase.EndUpdate()
    End Sub

    Public Function GetData(ByVal strSQL As String, format As DBFormat, connectionString As String) As DataSet

        Dim evehqData As New DataSet

        Select Case format
            Case DBFormat.Sqlce  ' SQL CE
                Dim conn As New SqlCeConnection
                conn.ConnectionString = connectionString
                Try
                    conn.Open()
                    Dim da As New SqlCeDataAdapter(strSQL, conn)
                    da.Fill(evehqData, "EveHQData")
                    conn.Close()
                    Return evehqData
                Catch e As Exception
                    'MessageBox.Show(e.Message, "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case DBFormat.Sql  ' MSSQL
                Dim conn As New SqlConnection
                conn.ConnectionString = connectionString
                Try
                    conn.Open()
                    Dim da As New SqlDataAdapter(strSQL, conn)
                    da.Fill(evehqData, "EveHQData")
                    conn.Close()
                    Return evehqData
                Catch e As Exception
                    'MessageBox.Show(e.Message, "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return Nothing
                Finally
                    If conn.State = ConnectionState.Open Then
                        conn.Close()
                    End If
                End Try
            Case Else
                'MessageBox.Show("Invalid database format!", "GetData Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
        End Select
    End Function

    Private Sub SetUi()

        ' Check for local switch
        chkUsingLocalSwitch.Checked = IsLocal

        ' Display settings folder
        txtSettingsFolder.Text = _settingsFolder

        ' Display SQLCE filename
        txtSQLCEFile.Text = _dbFileName

        ' Display SQL server instance names
        If cboSQLServer.Items.Contains(_dbServerName) Then
            cboSQLServer.SelectedItem = _dbServerName
        End If

        ' Display SQL database names
        If cboSQLDatabase.Items.Contains(_dbDatabase) Then
            cboSQLDatabase.SelectedItem = _dbDatabase
        End If

        ' Display SQL security options
        chkUseSQLSecurity.Checked = _dbSQLSec
        txtSQLUsername.Text = _dbUsername
        txtSQLPassword.Text = _dbPassword

        ' Force change of status of UI elements
        Select Case _dbFormat
            Case DBFormat.Sqlce
                radSQLCE.Checked = True
            Case DBFormat.Sql
                radSQL.Checked = True
        End Select

    End Sub

    Private Sub UpdateProgress(progressText As String)
        lblConversion.Text = progressText : gbConversion.Refresh()
    End Sub

    Private Sub radSQLCE_CheckedChanged(sender As Object, e As EventArgs) Handles radSQLCE.CheckedChanged

        lblSQLCEFile.Enabled = radSQLCE.Checked
        txtSQLCEFile.Enabled = radSQLCE.Checked
        btnBrowseSQLCE.Enabled = radSQLCE.Checked
        lblSQLServer.Enabled = Not (radSQLCE.Checked)
        cboSQLServer.Enabled = Not (radSQLCE.Checked)
        btnRefreshServers.Enabled = Not (radSQLCE.Checked)
        lblSQLDatabase.Enabled = Not (radSQLCE.Checked)
        cboSQLDatabase.Enabled = Not (radSQLCE.Checked)
        btnRefreshDatabases.Enabled = Not (radSQLCE.Checked)
        chkUseSQLSecurity.Enabled = Not (radSQLCE.Checked)
        lblSQLUsername.Enabled = Not radSQLCE.Checked And _dbSQLSec
        txtSQLUsername.Enabled = Not radSQLCE.Checked And _dbSQLSec
        lblSQLPassword.Enabled = Not radSQLCE.Checked And _dbSQLSec
        txtSQLPassword.Enabled = Not radSQLCE.Checked And _dbSQLSec

        If radSQLCE.Checked = True Then
            _dbFormat = DBFormat.Sqlce
        Else
            _dbFormat = DBFormat.Sql
        End If

    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowseSQLCE.Click
        Dim ofd As New OpenFileDialog
        With ofd
            .Title = "Select SQL CE Data file"
            .FileName = ""
            If My.Computer.FileSystem.DirectoryExists(_settingsFolder) Then
                .InitialDirectory = _settingsFolder
            Else
                .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            End If
            .Filter = "SQL Data files (*.sdf)|*.sdf|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtSQLCEFile.Text = .FileName
            End If
        End With
        ofd.Dispose()
    End Sub

    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click

        btnConvert.Enabled = False
        gbApplicationSettings.Enabled = False
        gbDatabaseSettings.Enabled = False
        gbConversion.Enabled = True
        cp1.IsRunning = True

        _conversionWorker.WorkerReportsProgress = True
        _conversionWorker.WorkerSupportsCancellation = False
        _conversionWorker.RunWorkerAsync()

    End Sub

    Private Sub cboSQLServer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSQLServer.SelectedIndexChanged
        _dbServerName = cboSQLServer.SelectedItem.ToString
        UpdateDatabases()
    End Sub

    Private Sub btnRefreshServers_Click(sender As Object, e As EventArgs) Handles btnRefreshServers.Click
        UpdateServers()
    End Sub

    Private Sub btnRefreshDatabases_Click(sender As Object, e As EventArgs) Handles btnRefreshDatabases.Click
        UpdateDatabases()
    End Sub

    Private Sub txtSQLCEFile_TextChanged(sender As Object, e As EventArgs) Handles txtSQLCEFile.TextChanged
        _dbFileName = txtSQLCEFile.Text
    End Sub

    Private Sub chkUsingLocalSwitch_CheckedChanged(sender As Object, e As EventArgs) Handles chkUsingLocalSwitch.CheckedChanged
        IsLocal = chkUsingLocalSwitch.Checked
    End Sub

    Private Sub cboSQLDatabase_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSQLDatabase.SelectedIndexChanged
        _dbDatabase = cboSQLDatabase.SelectedItem.ToString
    End Sub

    Private Sub txtSQLUsername_TextChanged(sender As Object, e As EventArgs) Handles txtSQLUsername.TextChanged
        _dbUsername = txtSQLUsername.Text
    End Sub

    Private Sub txtSQLPassword_TextChanged(sender As Object, e As EventArgs) Handles txtSQLPassword.TextChanged
        _dbPassword = txtSQLPassword.Text
    End Sub

    Private Sub chkUseSQLSecurity_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseSQLSecurity.CheckedChanged
        _dbSQLSec = chkUseSQLSecurity.Checked
        lblSQLUsername.Enabled = _dbSQLSec
        txtSQLUsername.Enabled = _dbSQLSec
        lblSQLPassword.Enabled = _dbSQLSec
        txtSQLPassword.Enabled = _dbSQLSec
    End Sub

    Private Sub _conversionWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles _conversionWorker.DoWork

        Dim dbc As New DatabaseConverter(_conversionWorker, _settingsFolder, _dbFormat, _dbFileName, _dbServerName, _dbDatabase, _dbSQLSec, _dbUsername, _dbPassword)
        dbc.Convert()

        Dim fc As New FileConverter(_conversionWorker, _settingsFolder)
        fc.Convert()

    End Sub

    Private Sub _conversionWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles _conversionWorker.ProgressChanged
        Dim progressText As String = e.UserState.ToString
        lblConversion.Text = progressText : gbConversion.Refresh()
        txtLog.Text = progressText & ControlChars.CrLf & txtLog.Text
        txtLog.Refresh()
    End Sub

    Private Sub _conversionWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles _conversionWorker.RunWorkerCompleted

        UpdateProgress("EveHQ Settings Conversion Complete!")

        btnConvert.Enabled = True
        gbApplicationSettings.Enabled = True
        gbDatabaseSettings.Enabled = True
        gbConversion.Enabled = False
        cp1.IsRunning = False

        Dim msg As New StringBuilder
        msg.AppendLine("EveHQ Settings conversion is complete!")
        msg.AppendLine("")
        msg.AppendLine("Would you like to exit the conversion and continue loading EveHQ?")

        Dim reply As DialogResult = MessageBox.Show(msg.ToString, "Conversion Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            End
        End If

    End Sub
End Class
