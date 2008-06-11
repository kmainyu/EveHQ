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
Imports System
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Threading
Imports System.Xml
Imports System.Windows.Forms

Public Class Settings

    Public Shared HQFSettings As New HQF.Settings
    Public Shared HQFFolder As String
    Public Shared HQFCacheFolder As String

    Private cHiSlotColour As Long = System.Drawing.Color.PeachPuff.ToArgb
    Private cMidSlotColour As Long = System.Drawing.Color.LightSteelBlue.ToArgb
    Private cLowSlotColour As Long = System.Drawing.Color.Thistle.ToArgb
    Private cRigSlotColour As Long = System.Drawing.Color.LightGreen.ToArgb
    Private cDefaultPilot As String = ""
    Private cRestoreLastSession As Boolean = False
    Private cLastPriceUpdate As DateTime
    Private cModuleFilter As Integer = 63
    Private cAutoUpdateHQFSkills As Boolean = False
    Private cOpenFittingList As New ArrayList

    Public Property OpenFittingList() As ArrayList
        Get
            Return cOpenFittingList
        End Get
        Set(ByVal value As ArrayList)
            cOpenFittingList = value
        End Set
    End Property
    Public Property AutoUpdateHQFSkills() As Boolean
        Get
            Return cAutoUpdateHQFSkills
        End Get
        Set(ByVal value As Boolean)
            cAutoUpdateHQFSkills = value
        End Set
    End Property
    Public Property ModuleFilter() As Integer
        Get
            Return cModuleFilter
        End Get
        Set(ByVal value As Integer)
            cModuleFilter = value
        End Set
    End Property
    Public Property LastPriceUpdate() As DateTime
        Get
            Return cLastPriceUpdate
        End Get
        Set(ByVal value As DateTime)
            cLastPriceUpdate = value
        End Set
    End Property
    Public Property RestoreLastSession() As Boolean
        Get
            Return cRestoreLastSession
        End Get
        Set(ByVal value As Boolean)
            cRestoreLastSession = value
        End Set
    End Property
    Public Property DefaultPilot() As String
        Get
            Return cDefaultPilot
        End Get
        Set(ByVal value As String)
            cDefaultPilot = value
        End Set
    End Property
    Public Property HiSlotColour() As Long
        Get
            Return cHiSlotColour
        End Get
        Set(ByVal value As Long)
            cHiSlotColour = value
        End Set
    End Property
    Public Property MidSlotColour() As Long
        Get
            Return cMidSlotColour
        End Get
        Set(ByVal value As Long)
            cMidSlotColour = value
        End Set
    End Property
    Public Property LowSlotColour() As Long
        Get
            Return cLowSlotColour
        End Get
        Set(ByVal value As Long)
            cLowSlotColour = value
        End Set
    End Property
    Public Property RigSlotColour() As Long
        Get
            Return cRigSlotColour
        End Get
        Set(ByVal value As Long)
            cRigSlotColour = value
        End Set
    End Property

    Public Sub SaveHQFSettings()
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As String = ""

        ' Prepare the XML document
        XMLS = ("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>") & vbCrLf
        XMLS &= "<HQFSettings>" & vbCrLf

        ' Save the General Information
        XMLS &= Chr(9) & "<general>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<hiSlotColour>" & HQFSettings.HiSlotColour & "</hiSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<midSlotColour>" & HQFSettings.MidSlotColour & "</midSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<lowSlotColour>" & HQFSettings.LowSlotColour & "</lowSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<rigSlotColour>" & HQFSettings.RigSlotColour & "</rigSlotColour>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<defaultPilot>" & HQFSettings.DefaultPilot & "</defaultPilot>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<restoreLastSession>" & HQFSettings.RestoreLastSession & "</restoreLastSession>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<lastPriceUpdate>" & HQFSettings.LastPriceUpdate & "</lastPriceUpdate>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<moduleFilter>" & HQFSettings.ModuleFilter & "</moduleFilter>" & vbCrLf
        XMLS &= Chr(9) & Chr(9) & "<updateHQFSkills>" & HQFSettings.AutoUpdateHQFSkills & "</updateHQFSkills>" & vbCrLf
        XMLS &= Chr(9) & "</general>" & vbCrLf

        ' Save the open fittings
        XMLS &= Chr(9) & "<openFittings>" & vbCrLf
        For Each fitting As String In ShipLists.fittedShipList.Keys
            XMLS &= Chr(9) & Chr(9) & "<fitting>" & fitting & "</fitting>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</openFittings>" & vbCrLf

        ' Save the Implant groups
        XMLS &= Chr(9) & "<implantGroups>" & vbCrLf
        For Each implantSet As ImplantGroup In Implants.implantGroups.Values
            XMLS &= Chr(9) & Chr(9) & "<implantGroup>" & vbCrLf
            XMLS &= Chr(9) & Chr(9) & Chr(9) & "<implantGroupName>" & implantSet.GroupName & "</implantGroupName>" & vbCrLf
            For imp As Integer = 1 To 10
                XMLS &= Chr(9) & Chr(9) & Chr(9) & "<implantName>" & implantSet.ImplantName(imp) & "</implantName>" & vbCrLf
            Next
            XMLS &= Chr(9) & Chr(9) & "</implantGroup>" & vbCrLf
        Next
        XMLS &= Chr(9) & "</implantGroups>" & vbCrLf

        ' End Settings
        XMLS &= "</HQFSettings>"
        XMLdoc.LoadXml(XMLS)
        Try
            XMLdoc.Save(HQFFolder & "\HQFSettings.xml")
        Catch e As Exception
            'Console.WriteLine(e.Message)
        End Try


    End Sub
    Public Function LoadHQFSettings() As Boolean
        Dim XMLdoc As XmlDocument = New XmlDocument

        If My.Computer.FileSystem.FileExists(HQFFolder & "\HQFSettings.xml") = True Then
            XMLdoc.Load(HQFFolder & "\HQFSettings.xml")
            Dim settingDetails As XmlNodeList
            Dim settingSettings As XmlNode

            ' Get the General Settings
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/general")
                If settingDetails.Count <> 0 Then
                    ' Get the relevant node!
                    settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/pilots node in each XML doc
                    If settingSettings.HasChildNodes Then

                        HQFSettings.HiSlotColour = CLng(settingSettings.ChildNodes(0).InnerText)
                        HQFSettings.MidSlotColour = CLng(settingSettings.ChildNodes(1).InnerText)
                        HQFSettings.LowSlotColour = CLng(settingSettings.ChildNodes(2).InnerText)
                        HQFSettings.RigSlotColour = CLng(settingSettings.ChildNodes(3).InnerText)
                        HQFSettings.DefaultPilot = CStr(settingSettings.ChildNodes(4).InnerText)
                        HQFSettings.RestoreLastSession = CBool(settingSettings.ChildNodes(5).InnerText)
                        HQFSettings.LastPriceUpdate = CDate(settingSettings.ChildNodes(6).InnerText)
                        HQFSettings.ModuleFilter = CInt(settingSettings.ChildNodes(7).InnerText)
                        HQFSettings.AutoUpdateHQFSkills = CBool(settingSettings.ChildNodes(8).InnerText)

                    End If
                End If
            Catch
            End Try

            ' Get the open fitting details details
            HQFSettings.OpenFittingList.Clear()
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/openFittings")
                ' Get the relevant node!
                settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If settingSettings.HasChildNodes Then
                    For group As Integer = 0 To settingSettings.ChildNodes.Count - 1
                        HQFSettings.OpenFittingList.Add(settingSettings.ChildNodes(group).InnerText)
                    Next
                End If
            Catch
            End Try


            ' Get the implant details
            Implants.implantGroups.Clear()
            Try
                settingDetails = XMLdoc.SelectNodes("/HQFSettings/implantGroups")
                ' Get the relevant node!
                settingSettings = settingDetails(0)       ' This is zero because there is only 1 occurence of the EveHQSettings/accounts node in each XML doc
                If settingSettings.HasChildNodes Then
                    For group As Integer = 0 To settingSettings.ChildNodes.Count - 1
                        Dim newImplantGroup As New ImplantGroup
                        newImplantGroup.GroupName = settingSettings.ChildNodes(group).ChildNodes(0).InnerText
                        For imp As Integer = 1 To 10
                            newImplantGroup.ImplantName(imp) = CStr(settingSettings.ChildNodes(group).ChildNodes(imp).InnerText)
                        Next
                        Implants.implantGroups.Add(newImplantGroup.GroupName, newImplantGroup)
                    Next
                End If
            Catch
            End Try

        End If
        Return True

    End Function

End Class
