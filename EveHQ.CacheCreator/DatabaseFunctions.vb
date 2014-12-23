Imports System.IO
Imports System.Data.SQLite

Public Class DatabaseFunctions

    Shared ReadOnly SqliteFolder As String = Application.StartupPath
    Const SqliteDatabase As String = "eve.db"

    ''' <summary>
    ''' Function to return the SQLite DB connection string
    ''' </summary>
    ''' <returns>A string indicating the connection parameters for SQLite</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSqlLiteConnectionString() As String
        Return "Data Source=" & ControlChars.Quote & Path.Combine(SqliteFolder, SqliteDatabase) & ControlChars.Quote & ";Version=3;"
    End Function

    ''' <summary>
    ''' Function to check if a connection can be made to the custom database
    ''' </summary>
    ''' <param name="silentResponse">Set to "True" if the routine is to show an error message on connection failure</param>
    ''' <returns>A boolean value indicating if the connection was succesful</returns>
    ''' <remarks></remarks>
    Public Shared Function CheckCustomDatabaseConnection(ByVal silentResponse As Boolean) As Boolean
        Dim connection As New SQLiteConnection(GetSqlLiteConnectionString)
        Try
            connection.Open()
            connection.Close()
            If silentResponse = False Then
                MessageBox.Show("Connected successfully to SQLite database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return True
        Catch ex As Exception
            If silentResponse = False Then
                MessageBox.Show(ex.Message, "Error Opening SQLite Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Function to return a list of database tables in the custom database
    ''' </summary>
    ''' <returns>A List(Of String) containing the names of the tables in the custom database</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDatabaseTables() As List(Of String)
        Dim dbTables As New List(Of String)
        Using schemaTable As DataSet = GetStaticData("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;")
            For Each tableRow As DataRow In schemaTable.Tables(0).Rows
                dbTables.Add(tableRow.Item("name").ToString)
            Next
        End Using
        Return dbTables
    End Function

    ''' <summary>
    ''' Function to return data from a database
    ''' </summary>
    ''' <param name="sql">A string containing the SQL query to execute</param>
    ''' <returns>A Dataset containing the requrested data</returns>
    ''' <remarks></remarks>
    Public Shared Function GetStaticData(ByVal sql As String) As DataSet
        Dim eveData As New DataSet
        Dim conn As New SQLiteConnection(GetSqlLiteConnectionString)
        Try
            conn.Open()
            Dim da As New SQLiteDataAdapter(sql, conn)
            da.Fill(eveData, "EveData")
            conn.Close()
            Return eveData
        Catch e As Exception
            Return Nothing
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

    ''' <summary>
    ''' Function to write data to the static database via an SQL string
    ''' </summary>
    ''' <param name="sql">A string containing the SQL command to execute</param>
    ''' <returns>The number of records affected by the command</returns>
    ''' <remarks>Returns -2 in the event of an error</remarks>
    Public Shared Function SetStaticData(ByVal sql As String) As Integer

        Dim recordsAffected As Integer
        Dim conn As New SQLiteConnection(GetSqlLiteConnectionString)
        Try
            conn.Open()
            Dim keyCommand As New SQLiteCommand(sql, conn)
            recordsAffected = keyCommand.ExecuteNonQuery()
            Return recordsAffected
        Catch e As Exception
            Return -2
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Function

End Class
