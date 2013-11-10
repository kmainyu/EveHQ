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
Imports System.Globalization
Imports System.Text
Imports System.Xml
Imports EveHQ.Core
Imports EveHQ.EveData

Namespace Classes

    Public Class InventionAPIJob
        Public JobID As Long
        Public ResultDate As Date
        Public BlueprintID As Integer
        Public TypeID As Integer
        Public TypeName As String
        Public InstallerID As Long
        Public InstallerName As String
        Public Result As Integer

        Private Const IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        Private Shared ReadOnly Culture As CultureInfo = New CultureInfo("en-GB")

        Public Shared Function ParseInventionJobsFromAPI(jobXML As XmlDocument) As SortedList(Of Long, InventionAPIJob)

            If JobXML IsNot Nothing Then

                ' Get the Node List
                Dim jobNodes As XmlNodeList = jobXML.SelectNodes("/eveapi/result/rowset/row")

                ' Parse the Node List
                Dim jobList As New SortedList(Of Long, InventionAPIJob)
                For Each tran As XmlNode In jobNodes
                    ' Check for invention jobs
                    If CType(tran.Attributes.GetNamedItem("activityID").Value, BlueprintActivity) = BlueprintActivity.Invention Then
                        ' Check the job is actually completed first!
                        If CInt(tran.Attributes.GetNamedItem("completed").Value) = 1 Then
                            Dim newJob As New InventionAPIJob
                            newJob.JobID = CLng(tran.Attributes.GetNamedItem("jobID").Value)
                            newJob.ResultDate = DateTime.ParseExact(tran.Attributes.GetNamedItem("endProductionTime").Value, IndustryTimeFormat, Culture)
                            newJob.InstallerID = CLng(tran.Attributes.GetNamedItem("installerID").Value)
                            newJob.InstallerName = ""
                            newJob.BlueprintID = CInt(tran.Attributes.GetNamedItem("installedItemTypeID").Value)
                            newJob.TypeID = CInt(tran.Attributes.GetNamedItem("outputTypeID").Value)
                            newJob.TypeName = StaticData.Types(newJob.TypeID).Name
                            newJob.Result = CInt(tran.Attributes.GetNamedItem("completedStatus").Value)
                            jobList.Add(newJob.JobID, newJob)
                        End If
                    End If
                Next
                ' Get Installer Names
                Dim idList As SortedList(Of Long, String) = GetInstallerList(jobList)
                ' Add installer names
                For Each job As InventionAPIJob In jobList.Values
                    If idList.ContainsKey(job.InstallerID) Then
                        job.InstallerName = idList(job.InstallerID)
                    End If
                Next
                Return jobList
            Else
                Return Nothing
            End If

        End Function

        Public Shared Function ParseInventionJobsFromDB(strSQL As String) As SortedList(Of Long, InventionAPIJob)

            Dim jobList As New SortedList(Of Long, InventionAPIJob)

            Try
                If strSQL <> "" Then
                    ' Fetch the data
                    Dim jobData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
                    If jobData IsNot Nothing Then
                        If jobData.Tables(0).Rows.Count > 0 Then
                            For Each je As DataRow In jobData.Tables(0).Rows
                                Dim job As New InventionAPIJob
                                job.JobID = CLng(je.Item("jobID"))
                                job.ResultDate = DateTime.Parse(je.Item("resultDate").ToString)
                                job.BlueprintID = CInt(je.Item("BPID"))
                                job.TypeID = CInt(je.Item("typeID"))
                                job.TypeName = je.Item("typeName").ToString
                                job.InstallerID = CLng(je.Item("installerID"))
                                job.InstallerName = je.Item("installerName").ToString
                                job.Result = CInt(je.Item("result"))

                                jobList.Add(job.JobID, job)

                            Next
                        End If
                    End If
                End If
            Catch e As Exception

            End Try

            Return jobList

        End Function

        Public Shared Function GetInstallerList(ByVal jobList As SortedList(Of Long, InventionAPIJob)) As SortedList(Of Long, String)
            Dim idList As New List(Of String)
            For Each job As InventionAPIJob In jobList.Values
                If idList.Contains(job.InstallerID.ToString) = False Then
                    idList.Add(job.InstallerID.ToString)
                End If
            Next
            Dim installerList As New SortedList(Of Long, String)
            Dim strID As New StringBuilder
            If idList.Count > 0 Then
                For Each id As String In idList
                    If id <> "" Then
                        strID.Append("," & id)
                    End If
                Next
                strID.Remove(0, 1)
                ' Get the name data from the DB
                Dim strSQL As String = "SELECT * FROM eveIDToName WHERE eveID IN (" & strID.ToString & ");"
                Dim idData As DataSet = CustomDataFunctions.GetCustomData(strSQL)
                If idData IsNot Nothing Then
                    If idData.Tables(0).Rows.Count > 0 Then
                        For Each idRow As DataRow In idData.Tables(0).Rows
                            installerList.Add(CLng(idRow.Item("eveID")), CStr(idRow.Item("eveName")))
                        Next
                    End If
                End If
            End If
            ' Add in any other IDs we don't have
            For Each id As String In idList
                If installerList.ContainsKey(CLng(id)) = False Then
                    installerList.Add(CLng(id), id)
                End If
            Next
            Return installerList
        End Function

        Public Shared Function CalculateInventionStats(jobList As SortedList(Of Long, InventionAPIJob)) As SortedList(Of String, SortedList(Of String, InventionResults))

            Dim stats As New SortedList(Of String, SortedList(Of String, InventionResults))

            For Each job As InventionAPIJob In jobList.Values

                ' Check for existing installer
                Dim types As New SortedList(Of String, InventionResults)
                If stats.ContainsKey(job.InstallerName) = True Then
                    ' Fetch the types
                    types = stats(job.InstallerName)
                Else
                    ' Add a new list of types
                    stats.Add(job.InstallerName, types)
                End If

                ' We got the types, let's see if the module already exists
                Dim typeResults As New InventionResults
                If types.ContainsKey(job.TypeName) = True Then
                    ' Fetch the results for this type
                    typeResults = types(job.TypeName)
                Else
                    ' Add this type
                    typeResults.InstallerName = job.InstallerName
                    typeResults.TypeName = job.TypeName
                    types.Add(job.TypeName, typeResults)
                End If

                ' Add the specific data
                Select Case job.Result
                    Case 1
                        typeResults.Successes += 1
                    Case Else
                        typeResults.Failures += 1
                End Select

            Next

            Return stats

        End Function
    End Class

    Public Class InventionResults
        Public InstallerName As String
        Public TypeName As String
        Public Successes As Long
        Public Failures As Long
    End Class
End Namespace