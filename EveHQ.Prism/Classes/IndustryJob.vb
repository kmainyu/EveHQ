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

<Serializable()> Public Class IndustryJob
    Public JobID As Long
    Public InstalledItemLocationID As Integer
    Public InstallerID As Long
    Public ActivityID As EveData.BlueprintActivity
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
    Private Shared ReadOnly Culture As New Globalization.CultureInfo("en-GB")

    Public Shared Function ParseIndustryJobs(ByVal jobOwner As String) As List(Of IndustryJob)

        Dim owner As PrismOwner

        If PlugInData.PrismOwners.ContainsKey(jobOwner) = True Then

            owner = PlugInData.PrismOwners(jobOwner)
            Dim ownerAccount As Core.EveHQAccount = PlugInData.GetAccountForCorpOwner(owner, CorpRepType.Jobs)
            Dim ownerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(owner, CorpRepType.Jobs)
            Dim apiReq As New EveAPI.EveAPIRequest(Core.HQ.EveHQAPIServerInfo, Core.HQ.RemoteProxy, Core.HQ.Settings.APIFileExtension, Core.HQ.cacheFolder)
            Dim transXML As XmlDocument

            If ownerAccount IsNot Nothing Then

                If owner.IsCorp = True Then
                    transXML = apiReq.GetAPIXML(EveAPI.APITypes.IndustryCorp, ownerAccount.ToAPIAccount, ownerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                Else
                    transXML = apiReq.GetAPIXML(EveAPI.APITypes.IndustryChar, ownerAccount.ToAPIAccount, ownerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                End If

                If transXML IsNot Nothing Then

                    ' Get the Node List
                    Dim jobNodes As XmlNodeList = transXML.SelectNodes("/eveapi/result/rowset/row")

                    ' Parse the Node List
                    Dim jobList As New List(Of IndustryJob)
                    For Each tran As XmlNode In jobNodes
                        Dim newJob As New IndustryJob
                        newJob.JobID = CLng(tran.Attributes.GetNamedItem("jobID").Value)
                        newJob.InstalledItemLocationID = CInt(tran.Attributes.GetNamedItem("installedItemLocationID").Value)
                        newJob.InstallerID = CLng(tran.Attributes.GetNamedItem("installerID").Value)
                        newJob.ActivityID = CType(tran.Attributes.GetNamedItem("activityID").Value, EveData.BlueprintActivity)
                        newJob.InstalledItemTypeID = CInt(tran.Attributes.GetNamedItem("installedItemTypeID").Value)
                        newJob.OutputTypeID = CInt(tran.Attributes.GetNamedItem("outputTypeID").Value)
                        newJob.Runs = CInt(tran.Attributes.GetNamedItem("runs").Value)
                        newJob.OutputLocationID = CInt(tran.Attributes.GetNamedItem("outputLocationID").Value)
                        newJob.InstalledInSolarSystemID = CInt(tran.Attributes.GetNamedItem("installedInSolarSystemID").Value)
                        newJob.Completed = CInt(tran.Attributes.GetNamedItem("completed").Value)
                        newJob.CompletedStatus = CInt(tran.Attributes.GetNamedItem("completedStatus").Value)
                        newJob.CompletedSuccessfully = CInt(tran.Attributes.GetNamedItem("completedSuccessfully").Value)
                        newJob.InstallTime = DateTime.ParseExact(tran.Attributes.GetNamedItem("installTime").Value, IndustryTimeFormat, culture)
                        newJob.BeginProductionTime = DateTime.ParseExact(tran.Attributes.GetNamedItem("beginProductionTime").Value, IndustryTimeFormat, culture)
                        newJob.EndProductionTime = DateTime.ParseExact(tran.Attributes.GetNamedItem("endProductionTime").Value, IndustryTimeFormat, culture)
                        newJob.InstalledMELevel = CInt(tran.Attributes.GetNamedItem("installedItemMaterialLevel").Value)
                        newJob.InstalledPELevel = CInt(tran.Attributes.GetNamedItem("installedItemProductivityLevel").Value)
                        newJob.InstalledRuns = CInt(tran.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                        newJob.MaterialMultiplier = Double.Parse(tran.Attributes.GetNamedItem("materialMultiplier").Value, culture)
                        newJob.TimeMultiplier = Double.Parse(tran.Attributes.GetNamedItem("timeMultiplier").Value, culture)
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
            Dim idData As DataSet = Core.CustomDataFunctions.GetCustomData(strSQL)
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
