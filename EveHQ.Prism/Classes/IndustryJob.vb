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
Imports EveHQ.EveAPI
Imports EveHQ.EveData
Imports EveHQ.Common.Extensions

Namespace Classes

    Public Class IndustryJob
        Public JobID As Long
        Public InstalledItemLocationID As Integer
        Public InstallerID As Long
        Public ActivityID As BlueprintActivity
        Public InstalledItemTypeID As Integer
        Public OutputTypeID As Integer
        Public Runs As Integer
        Public OutputLocationID As Integer
        Public InstalledInSolarSystemID As Integer
        Public Completed As Integer
        Public CompletedStatus As Integer
        Public CompletedSuccessfully As Integer
        Public InstallTime As Date
        Public BeginProductionTime As Date
        Public EndProductionTime As Date
        Public InstalledMELevel As Integer
        Public InstalledPELevel As Integer
        Public InstalledRuns As Integer
        Public MaterialMultiplier As Double
        Public TimeMultiplier As Double

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
                            newJob.JobID = tran.JobId
                            newJob.InstalledItemLocationID = tran.InstalledItemLocationId
                            newJob.InstallerID = tran.InstallerId
                            newJob.ActivityID = CType(tran.ActivityId, BlueprintActivity)
                            newJob.InstalledItemTypeID = tran.InstalledItemTypeId
                            newJob.OutputTypeID = tran.OutputTypeId
                            newJob.Runs = tran.Runs
                            newJob.OutputLocationID = tran.OutputLocationId
                            newJob.InstalledInSolarSystemID = tran.InstalledInSolarSystemId
                            If tran.Completed Then
                                newJob.Completed = 1
                            Else
                                newJob.Completed = 0
                            End If

                            newJob.CompletedStatus = tran.CompletedStatus

                            If tran.CompletedSuccessfully Then
                                newJob.CompletedSuccessfully = 1
                            Else
                                newJob.CompletedSuccessfully = 0
                            End If

                            newJob.InstallTime = tran.InstallTime.DateTime
                            newJob.BeginProductionTime = tran.BeginProductionTime.DateTime
                            newJob.EndProductionTime = tran.EndProductionTime.DateTime
                            newJob.InstalledMELevel = tran.InstalledItemMaterialLevel
                            newJob.InstalledPELevel = tran.InstalledItemProductivityLevel
                            newJob.InstalledRuns = tran.Runs
                            newJob.MaterialMultiplier = tran.CharMaterialMultiplier
                            newJob.TimeMultiplier = tran.TimeMultiplier
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