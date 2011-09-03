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

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn

    Public Shared PlugInDataObject As Object
    Public Shared AttributeList As New SortedList
    Public Shared EffectList As New SortedList
    Public Shared Event PluginDataReceived()

    Public Function GetPlugInData(ByVal Data As Object, ByVal DataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Try
            Select Case DataType
                Case 0 ' Item ID
                    PlugInDataObject = Data
                    RaiseEvent PluginDataReceived()
            End Select
            Return True
        Catch e As Exception
            Return False
        End Try
    End Function

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Try
            ' Load attribute names
            AttributeList.Clear()
            Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT DISTINCT dgmTypeAttributes.attributeID, dgmAttributeTypes.attributeName FROM dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                If PlugInData.AttributeList.ContainsKey(eveData.Tables(0).Rows(item).Item("attributeName").ToString.Trim) = False Then
                    PlugInData.AttributeList.Add(eveData.Tables(0).Rows(item).Item("attributeName").ToString.Trim, eveData.Tables(0).Rows(item).Item("attributeID").ToString.Trim)
                End If
            Next
            ' Load Effect Names
            EffectList.Clear()
            eveData = EveHQ.Core.DataFunctions.GetData("SELECT DISTINCT dgmTypeEffects.effectID, dgmEffects.effectName FROM dgmEffects INNER JOIN dgmTypeEffects ON dgmEffects.effectID = dgmTypeEffects.effectID;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                If PlugInData.EffectList.ContainsKey(eveData.Tables(0).Rows(item).Item("effectName").ToString.Trim) = False Then
                    PlugInData.EffectList.Add(eveData.Tables(0).Rows(item).Item("effectName").ToString.Trim, eveData.Tables(0).Rows(item).Item("effectID").ToString.Trim)
                End If
            Next
            Return True
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function
    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Item Browser"
        EveHQPlugIn.Description = "Shows Data on all Eve ingame items"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "Item Browser"
        EveHQPlugIn.RunAtStartup = True
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.plugin_icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function

    Public Function IGBService(ByVal IGBContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function

    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return New frmItemBrowser
    End Function

End Class

