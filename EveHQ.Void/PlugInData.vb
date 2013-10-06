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
Imports EveHQ.EveData

Public Class PlugInData
    Implements Core.IEveHQPlugIn

    Public Function GetPlugInData(ByVal data As Object, ByVal dataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Return Nothing
    End Function

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Return LoadVoidData()
    End Function

    Public Function GetEveHQPlugInInfo() As Core.EveHQPlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim eveHQPlugIn As New Core.EveHQPlugIn
        eveHQPlugIn.Name = "EveHQ Void"
        eveHQPlugIn.Description = "Wormhole and W-Space Information Tool"
        eveHQPlugIn.Author = "EveHQ Team"
        eveHQPlugIn.MainMenuText = "Void"
        eveHQPlugIn.RunAtStartup = True
        eveHQPlugIn.RunInIGB = True
        eveHQPlugIn.MenuImage = My.Resources.Plugin_Icon
        eveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return eveHQPlugIn
    End Function

    Public Function IGBService(ByVal igbContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return IGBData.Response(IGBContext)
    End Function

    Public Function RunEveHQPlugIn() As Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return New frmVoid
    End Function

    Public Function SaveAll() As Boolean Implements Core.IEveHQPlugIn.SaveAll
        ' No data or settings to save
        Return False
    End Function

    Private Function LoadVoidData() As Boolean
        LoadWormholeData()
        LoadWHSystemData()
        LoadWHAttributeData()
        Return True
    End Function

    Private Sub LoadWormholeData()
        ' Parse the WHAttributes resource
        Dim whAttributes As New SortedList(Of String, SortedList(Of String, String))
        Dim cAtts As SortedList(Of String, String)
        Dim atts() As String = My.Resources.WHattribs.Split((ControlChars.CrLf).ToCharArray)
        For Each att As String In atts
            If att <> "" Then
                Dim attData() As String = att.Split(",".ToCharArray)
                If whAttributes.ContainsKey(attData(0)) = False Then
                    whAttributes.Add(attData(0), New SortedList(Of String, String))
                End If
                cAtts = whAttributes(attData(0))
                cAtts.Add(attData(1), attData(2))
            End If
        Next
        ' Load the data
        Dim cWH As WormHole
        VoidData.Wormholes.Clear()
        For Each wh As EveType In StaticData.GetItemsInGroup(988)
            cWH = New WormHole
            cWH.ID = wh.Id.ToString
            cWH.Name = wh.Name.Replace("Wormhole ", "")
            If whAttributes.ContainsKey(cWH.ID) = True Then
                For Each att As String In whAttributes(cWH.ID).Keys
                    Select Case att
                        Case "1381"
                            cWH.TargetClass = whAttributes(cWH.ID).Item(att)
                        Case "1382"
                            cWH.MaxStabilityWindow = whAttributes(cWH.ID).Item(att)
                        Case "1383"
                            cWH.MaxMassCapacity = whAttributes(cWH.ID).Item(att)
                        Case "1384"
                            cWH.MassRegeneration = whAttributes(cWH.ID).Item(att)
                        Case "1385"
                            cWH.MaxJumpableMass = whAttributes(cWH.ID).Item(att)
                        Case "1457"
                            cWH.TargetDistributionID = whAttributes(cWH.ID).Item(att)
                    End Select
                Next
            Else
                cWH.TargetClass = ""
                cWH.MaxStabilityWindow = ""
                cWH.MaxMassCapacity = ""
                cWH.MassRegeneration = ""
                cWH.MaxJumpableMass = ""
                cWH.TargetDistributionID = ""
            End If
            ' Add in data from the resource file
            If cWH.Name.StartsWith("Test", StringComparison.Ordinal) = False Then
                VoidData.Wormholes.Add(CStr(cWH.Name), cWH)
            End If
        Next
        whAttributes.Clear()
    End Sub

    Private Sub LoadWHSystemData()
        ' Parse the location classes
        Dim whClasses As New SortedList(Of String, String)
        Dim classes() As String = My.Resources.WHClasses.Split((ControlChars.CrLf).ToCharArray)
        For Each whClass As String In classes
            If whClass <> "" Then
                Dim classData() As String = whClass.Split(",".ToCharArray)
                If whClasses.ContainsKey(classData(0)) = False Then
                    whClasses.Add(classData(0), classData(1))
                End If
            End If
        Next
        ' Parse the location effects
        Dim whEffects As New SortedList(Of String, String)
        Dim effects() As String = My.Resources.WSpaceTypes.Split((ControlChars.CrLf).ToCharArray)
        For Each whEffect As String In effects
            If whEffect <> "" Then
                Dim effectData() As String = whEffect.Split(",".ToCharArray)
                If whEffects.ContainsKey(effectData(0)) = False Then
                    whEffects.Add(effectData(0), effectData(1))
                End If
            End If
        Next

        ' Load the data
        Dim systems As IEnumerable(Of SolarSystem) = From item In StaticData.SolarSystems.Values Where item.RegionId > 11000000

        Dim cSystem As WormholeSystem
        VoidData.WormholeSystems.Clear()

        For Each solar As SolarSystem In systems
            cSystem = New WormholeSystem
            cSystem.ID = solar.Id.ToString
            cSystem.Name = solar.Name
            cSystem.Region = solar.RegionId.ToString
            cSystem.Constellation = solar.ConstellationId.ToString
            cSystem.WClass = whClasses(cSystem.Region)
            If VoidData.WormholeEffects.ContainsKey(cSystem.Name) = True Then
                cSystem.WEffect = whEffects(cSystem.Name)
            Else
                cSystem.WEffect = ""
            End If
            VoidData.WormholeSystems.Add(CStr(cSystem.Name), cSystem)
        Next
        whClasses.Clear()
        VoidData.WormholeEffects.Clear()

    End Sub

    Private Sub LoadWHAttributeData()
        ' Load the data
        Dim taqs As IEnumerable = (From item In StaticData.Types.Values Join ta In StaticData.TypeAttributes On item.Id Equals ta.TypeId Join at In StaticData.AttributeTypes.Values On ta.AttributeId Equals at.AttributeId
                Where item.Group = 920
                Select New TypeAttributeQuery With {
                .TypeID = item.Id,
                .TypeName = item.Name,
                .AttributeID = ta.AttributeId,
                .UnitID = at.UnitId,
                .Value = ta.Value}).ToList

        VoidData.WormholeEffects.Clear()
        Dim currentEffect As WormholeEffect
        For Each taq As TypeAttributeQuery In taqs
            Dim typeName As String = taq.TypeName
            Dim attID As String = CStr(taq.AttributeID)
            Dim attValue As Double = taq.Value
            If taq.UnitID = 124 Or taq.UnitID = 105 Then
                attValue = -attValue
            End If
            If VoidData.WormholeEffects.ContainsKey(typeName) = False Then
                VoidData.WormholeEffects.Add(typeName, New WormholeEffect)
            End If
            currentEffect = VoidData.WormholeEffects(typeName)
            currentEffect.WormholeType = typeName
            currentEffect.Attributes.Add(attID, attValue)
        Next
    End Sub

End Class
