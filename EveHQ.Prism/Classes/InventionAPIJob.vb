'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

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

        Public Shared Function ParseInventionJobsFromAPI(jobs As IEnumerable(Of EveAPI.IndustryJob)) As Dictionary(Of Long, InventionAPIJob)

            If jobs IsNot Nothing Then

                ' Parse the Node List
                Dim jobList As New Dictionary(Of Long, InventionAPIJob)
                For Each job In jobs
                    ' Check for invention jobs
                    If CType(job.ActivityId, BlueprintActivity) = BlueprintActivity.Invention Then
                        ' Check the job is actually completed first!
                        If job.Status <> EveApi.IndustryJobStatus.Cancelled Then
                            Dim newJob As New InventionAPIJob
                            newJob.JobID = job.JobId
                            newJob.ResultDate = job.EndDate.DateTime
                            newJob.InstallerID = job.InstallerId
                            newJob.InstallerName = ""
                            newJob.BlueprintID = job.BlueprintTypeId
                            newJob.TypeID = job.ProductTypeId
                            newJob.TypeName = StaticData.Types(newJob.TypeID).Name
                            newJob.Result = CInt(job.Status)
                            jobList.Add(newJob.JobID, newJob)
                        End If
                    End If
                Next
                ' Get Installer Names
                Dim idList As Dictionary(Of Long, String) = GetInstallerList(jobList)
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

        Public Shared Function GetInstallerList(ByVal jobList As Dictionary(Of Long, InventionAPIJob)) As Dictionary(Of Long, String)
            Dim idList As New List(Of String)
            For Each job As InventionAPIJob In jobList.Values
                If idList.Contains(job.InstallerID.ToString) = False Then
                    idList.Add(job.InstallerID.ToString)
                End If
            Next
            Dim installerList As New Dictionary(Of Long, String)
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