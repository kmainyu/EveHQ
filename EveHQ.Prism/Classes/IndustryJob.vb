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
Imports EveHQ.EveAPI
Imports EveHQ.EveData
Imports EveHQ.Common.Extensions

Namespace Classes

    Public Class IndustryJob
        Public JobId As Long
        Public InstallerId As Integer
        Public InstallerName As String
        Public FacilityId As Integer
        Public SolarSystemId As Integer
        Public SolarSystemName As String
        Public StationId As Integer
        Public ActivityId As BlueprintActivity
        Public BlueprintId As Long
        Public BlueprintTypeId As Integer
        Public BlueprintTypeName As String
        Public BlueprintLocationId As Long
        Public OutputLocationId As Long
        Public Runs As Integer
        Public Cost As Double
        Public TeamId As Integer
        Public LicencedRuns As Integer
        Public Probability As Double
        Public ProductTypeId As Integer
        Public ProductTypeName As String
        Public Status As IndustryJobStatus
        Public TimeInSeconds As Integer
        Public StartDate As DateTimeOffset
        Public EndDate As DateTimeOffset
        Public PauseDate As DateTimeOffset
        Public CompletedDate As DateTimeOffset
        Public CompletedCharacterId As Integer

        Private Const IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
        Private Shared ReadOnly Culture As New CultureInfo("en-GB")

        Public Shared Function ParseIndustryJobs(ByVal jobOwner As String) As List(Of IndustryJob)

            Dim owner As PrismOwner

            If PlugInData.PrismOwners.ContainsKey(jobOwner) = True Then

                owner = PlugInData.PrismOwners(jobOwner)
                Dim ownerAccount As EveHQAccount = PlugInData.GetAccountForCorpOwner(owner, CorpRepType.Jobs)
                Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(owner, CorpRepType.Jobs)
                Dim transXML As XmlDocument
                Dim jobsResponse As EveServiceResponse(Of IEnumerable(Of EveApi.IndustryJob))
                If ownerAccount IsNot Nothing Then

                    If owner.IsCorp = True Then
                        jobsResponse = HQ.ApiProvider.Corporation.IndustryJobs(ownerAccount.UserID, ownerAccount.APIKey, ownerID.ToInt32())
                        ''    transXML = apiReq.GetAPIXML(APITypes.IndustryCorp, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                    Else
                        jobsResponse = HQ.ApiProvider.Character.IndustryJobs(ownerAccount.UserID, ownerAccount.APIKey, ownerID.ToInt32())
                        ' transXML = apiReq.GetAPIXML(APITypes.IndustryChar, ownerAccount.ToAPIAccount, ownerID, APIReturnMethods.ReturnCacheOnly)
                    End If

                    If jobsResponse.IsSuccess Then

                        ' Get the Node List
                        ' Parse the Node List
                        Dim jobList As New List(Of IndustryJob)
                        For Each tran As EveApi.IndustryJob In jobsResponse.ResultData
                            Dim newJob As New IndustryJob
                            newJob.JobId = tran.JobId
                            newJob.InstallerId = tran.InstallerId
                            newJob.InstallerName = tran.InstallerName
                            newJob.FacilityId = tran.FacilityId
                            newJob.SolarSystemId = tran.SolarSystemId
                            newJob.SolarSystemName = tran.SolarSystemName
                            newJob.StationId = tran.StationId
                            newJob.ActivityId = DirectCast(tran.ActivityId, BlueprintActivity)
                            newJob.BlueprintId = tran.BlueprintId
                            newJob.BlueprintTypeId = tran.BlueprintTypeId
                            newJob.BlueprintTypeName = tran.BlueprintTypeName
                            newJob.BlueprintLocationId = tran.BlueprintLocationId
                            newJob.OutputLocationId = tran.OutputLocationId
                            newJob.Runs = tran.Runs
                            newJob.Cost = tran.Cost
                            newJob.TeamId = tran.TeamId
                            newJob.LicencedRuns = tran.LicensedRuns
                            newJob.Probability = tran.Probability
                            newJob.ProductTypeId = tran.ProductTypeId
                            newJob.ProductTypeName = tran.ProductTypeName
                            newJob.Status = tran.Status
                            newJob.TimeInSeconds = tran.TimeInSeconds
                            newJob.StartDate = tran.StartDate
                            newJob.EndDate = tran.EndDate
                            newJob.PauseDate = tran.PauseDate
                            newJob.CompletedDate = tran.CompletedDate
                            newJob.CompletedCharacterId = tran.CompletedCharacterId
                            jobList.Add(newJob)
                        Next
                        Return jobList
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If

        End Function

        Public Shared Function GetInstallerList(ByVal jobList As List(Of IndustryJob)) As SortedList(Of Long, String)
            Dim idList As New List(Of String)
            For Each job As IndustryJob In jobList
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
    End Class
End Namespace