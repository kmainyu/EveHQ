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
Imports System.Xml
Imports System.Text

<Serializable()> Public Class InventionAPIJob
    Public JobID As Long
    Public ResultDate As Date
    Public BPID As Integer
    Public TypeID As Integer
    Public TypeName As String
    Public InstallerID As Long
    Public InstallerName As String
    Public result As Integer

    Private Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Private Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    Public Shared Function ParseInventionJobsFromAPI(JobXML As XmlDocument) As SortedList(Of Long, InventionAPIJob)

        If JobXML IsNot Nothing Then

            ' Get the Node List
            Dim JobNodes As XmlNodeList = JobXML.SelectNodes("/eveapi/result/rowset/row")

            ' Parse the Node List
            Dim JobList As New SortedList(Of Long, InventionAPIJob)
            For Each Tran As XmlNode In JobNodes
                ' Check for invention jobs
                If CType(Tran.Attributes.GetNamedItem("activityID").Value, EveData.BlueprintActivity) = EveData.BlueprintActivity.Invention Then
                    ' Check the job is actually completed first!
                    If CInt(Tran.Attributes.GetNamedItem("completed").Value) = 1 Then
                        Dim NewJob As New InventionAPIJob
                        NewJob.JobID = CLng(Tran.Attributes.GetNamedItem("jobID").Value)
                        NewJob.ResultDate = DateTime.ParseExact(Tran.Attributes.GetNamedItem("endProductionTime").Value, IndustryTimeFormat, culture)
                        NewJob.InstallerID = CLng(Tran.Attributes.GetNamedItem("installerID").Value)
                        NewJob.InstallerName = ""
                        NewJob.BPID = CInt(Tran.Attributes.GetNamedItem("installedItemTypeID").Value)
                        NewJob.TypeID = CInt(Tran.Attributes.GetNamedItem("outputTypeID").Value)
                        NewJob.TypeName = EveHQ.Core.HQ.itemData(NewJob.TypeID.ToString).Name
                        NewJob.result = CInt(Tran.Attributes.GetNamedItem("completedStatus").Value)
                        JobList.Add(NewJob.JobID, NewJob)
                    End If
                End If
            Next
            ' Get Installer Names
            Dim IDList As SortedList(Of Long, String) = InventionAPIJob.GetInstallerList(JobList)
            ' Add installer names
            For Each Job As InventionAPIJob In JobList.Values
                If IDList.ContainsKey(Job.InstallerID) Then
                    Job.InstallerName = IDList(Job.InstallerID)
                End If
            Next
            Return JobList
        Else
            Return Nothing
        End If

    End Function

    Public Shared Function ParseInventionJobsFromDB(strSQL As String) As SortedList(Of Long, InventionAPIJob)

        Dim JobList As New SortedList(Of Long, InventionAPIJob)

        Try
            If strSQL <> "" Then
                ' Fetch the data
                Dim JobData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
                If JobData IsNot Nothing Then
                    If JobData.Tables(0).Rows.Count > 0 Then
                        For Each JE As DataRow In JobData.Tables(0).Rows
                            Dim Job As New InventionAPIJob
                            Job.JobID = CLng(JE.Item("jobID"))
                            Job.ResultDate = DateTime.Parse(JE.Item("resultDate").ToString)
                            Job.BPID = CInt(JE.Item("BPID"))
                            Job.TypeID = CInt(JE.Item("typeID"))
                            Job.TypeName = JE.Item("typeName").ToString
                            Job.InstallerID = CLng(JE.Item("installerID"))
                            Job.InstallerName = JE.Item("installerName").ToString
                            Job.result = CInt(JE.Item("result"))

                            JobList.Add(Job.JobID, Job)

                        Next
                    End If
                End If
            End If
        Catch e As Exception

        End Try

        Return JobList

    End Function

    Public Shared Function GetInstallerList(ByVal JobList As SortedList(Of Long, InventionAPIJob)) As SortedList(Of Long, String)
        Dim IDList As New List(Of String)
        For Each Job As InventionAPIJob In JobList.Values
            If IDList.Contains(Job.InstallerID.ToString) = False Then
                IDList.Add(Job.InstallerID.ToString)
            End If
        Next
        Dim InstallerList As New SortedList(Of Long, String)
        Dim strID As New StringBuilder
        If IDList.Count > 0 Then
            For Each ID As String In IDList
                If ID <> "" Then
                    strID.Append("," & ID)
                End If
            Next
            strID.Remove(0, 1)
            ' Get the name data from the DB
            Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
            Dim IDData As DataSet = EveHQ.Core.DataFunctions.GetCustomData(strSQL)
            If IDData IsNot Nothing Then
                If IDData.Tables(0).Rows.Count > 0 Then
                    For Each IDRow As DataRow In IDData.Tables(0).Rows
                        InstallerList.Add(CLng(IDRow.Item("eveID")), CStr(IDRow.Item("eveName")))
                    Next
                End If
            End If
        End If
        ' Add in any other IDs we don't have
        For Each ID As String In IDList
            If InstallerList.ContainsKey(CLng(ID)) = False Then
                InstallerList.Add(CLng(ID), ID)
            End If
        Next
        Return InstallerList
    End Function

    Public Shared Function CalculateInventionStats(JobList As SortedList(Of Long, InventionAPIJob)) As SortedList(Of String, SortedList(Of String, InventionResults))

        Dim Stats As New SortedList(Of String, SortedList(Of String, InventionResults))

        For Each Job As InventionAPIJob In JobList.Values

            ' Check for existing installer
            Dim Types As New SortedList(Of String, InventionResults)
            If Stats.ContainsKey(Job.InstallerName) = True Then
                ' Fetch the types
                Types = Stats(Job.InstallerName)
            Else
                ' Add a new list of types
                Stats.Add(Job.InstallerName, Types)
            End If

            ' We got the types, let's see if the module already exists
            Dim TypeResults As New InventionResults
            If Types.ContainsKey(Job.TypeName) = True Then
                ' Fetch the results for this type
                TypeResults = Types(Job.TypeName)
            Else
                ' Add this type
                TypeResults.InstallerName = Job.InstallerName
                TypeResults.TypeName = Job.TypeName
                Types.Add(Job.TypeName, TypeResults)
            End If

            ' Add the specific data
            Select Case Job.result
                Case 1
                    TypeResults.Successes += 1
                Case Else
                    TypeResults.Failures += 1
            End Select

        Next

        Return Stats

    End Function
End Class

Public Class InventionResults
    Public InstallerName As String
    Public TypeName As String
    Public Successes As Long
    Public Failures As Long
End Class
