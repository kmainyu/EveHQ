' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
    Public InstalledItemLocationID As Long
    Public InstallerID As Long
    Public ActivityID As JobActivity
    Public InstalledItemTypeID As Long
    Public OutputTypeID As Long
    Public Runs As Integer
    Public OutputLocationID As Long
    Public InstalledInSolarSystemID As Long
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

    Private Shared IndustryTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Private Shared culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")

    Public Shared Function ParseIndustryJobs(ByVal JobOwner As String) As List(Of IndustryJob)

        Dim Owner As New PrismOwner

        If PlugInData.PrismOwners.ContainsKey(JobOwner) = True Then

            Owner = PlugInData.PrismOwners(JobOwner)
            Dim OwnerAccount As EveHQ.Core.EveAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Jobs)
            Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Jobs)
            Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.EveHQSettings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
            Dim transXML As New XmlDocument

            If OwnerAccount IsNot Nothing Then

                If Owner.IsCorp = True Then
                    transXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                Else
                    transXML = APIReq.GetAPIXML(EveAPI.APITypes.IndustryChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                End If

                If transXML IsNot Nothing Then

                    ' Get the Node List
                    Dim JobNodes As XmlNodeList = transXML.SelectNodes("/eveapi/result/rowset/row")

                    ' Parse the Node List
                    Dim JobList As New List(Of IndustryJob)
                    For Each Tran As XmlNode In JobNodes
                        Dim NewJob As New IndustryJob
                        NewJob.JobID = CLng(Tran.Attributes.GetNamedItem("jobID").Value)
                        NewJob.InstalledItemLocationID = CLng(Tran.Attributes.GetNamedItem("installedItemLocationID").Value)
                        NewJob.InstallerID = CLng(Tran.Attributes.GetNamedItem("installerID").Value)
                        NewJob.ActivityID = CType(Tran.Attributes.GetNamedItem("activityID").Value, JobActivity)
                        NewJob.InstalledItemTypeID = CLng(Tran.Attributes.GetNamedItem("installedItemTypeID").Value)
                        NewJob.OutputTypeID = CLng(Tran.Attributes.GetNamedItem("outputTypeID").Value)
                        NewJob.Runs = CInt(Tran.Attributes.GetNamedItem("runs").Value)
                        NewJob.OutputLocationID = CLng(Tran.Attributes.GetNamedItem("outputLocationID").Value)
                        NewJob.InstalledInSolarSystemID = CLng(Tran.Attributes.GetNamedItem("installedInSolarSystemID").Value)
                        NewJob.Completed = CInt(Tran.Attributes.GetNamedItem("completed").Value)
                        NewJob.CompletedStatus = CInt(Tran.Attributes.GetNamedItem("completedStatus").Value)
                        NewJob.CompletedSuccessfully = CInt(Tran.Attributes.GetNamedItem("completedSuccessfully").Value)
                        NewJob.InstallTime = DateTime.ParseExact(Tran.Attributes.GetNamedItem("installTime").Value, IndustryTimeFormat, culture)
                        NewJob.BeginProductionTime = DateTime.ParseExact(Tran.Attributes.GetNamedItem("beginProductionTime").Value, IndustryTimeFormat, culture)
                        NewJob.EndProductionTime = DateTime.ParseExact(Tran.Attributes.GetNamedItem("endProductionTime").Value, IndustryTimeFormat, culture)
                        NewJob.InstalledMELevel = CInt(Tran.Attributes.GetNamedItem("installedItemMaterialLevel").Value)
                        NewJob.InstalledPELevel = CInt(Tran.Attributes.GetNamedItem("installedItemProductivityLevel").Value)
                        NewJob.InstalledRuns = CInt(Tran.Attributes.GetNamedItem("installedItemLicensedProductionRunsRemaining").Value)
                        NewJob.MaterialMultiplier = Double.Parse(Tran.Attributes.GetNamedItem("materialMultiplier").Value, culture)
                        NewJob.TimeMultiplier = Double.Parse(Tran.Attributes.GetNamedItem("timeMultiplier").Value, culture)
                        JobList.Add(NewJob)
                    Next
                    Return JobList
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

    Public Shared Function GetInstallerList(ByVal JobList As List(Of IndustryJob)) As SortedList(Of Long, String)
        Dim IDList As New List(Of String)
        For Each Job As IndustryJob In JobList
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
End Class

Public Enum JobActivity As Integer
    None = 0
    Manufacturing = 1
    TechResearch = 2
    PEResearch = 3
    MEResearch = 4
    Copying = 5
    Duplicating = 6
    ReverseEngineering = 7
    Invention = 8
End Enum