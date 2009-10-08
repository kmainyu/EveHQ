Imports System.Windows.Forms

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn

    Public Function GetPlugInData(ByVal Data As Object, ByVal DataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Return Nothing
    End Function

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Return LoadVoidData()
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Void"
        EveHQPlugIn.Description = "Wormhole and W-Space Information Tool"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "Void"
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
        Return New frmVoid
    End Function

    Private Function LoadVoidData() As Boolean
        If Me.LoadWormholeData = False Then
            Return False
        End If
        If Me.LoadWHSystemData = False Then
            Return False
        End If
        If Me.LoadWHAttributeData = False Then
            Return False
        End If
        Return True
    End Function

    Private Function LoadWormholeData() As Boolean
        ' Parse the WHAttributes resource
        Dim WHAttributes As New SortedList(Of String, SortedList(Of String, String))
        Dim cAtts As New SortedList(Of String, String)
        Dim Atts() As String = My.Resources.WHattribs.Split((ControlChars.CrLf).ToCharArray)
        For Each Att As String In Atts
            If Att <> "" Then
                Dim AttData() As String = Att.Split(",".ToCharArray)
                If WHAttributes.ContainsKey(AttData(0)) = False Then
                    WHAttributes.Add(AttData(0), New SortedList(Of String, String))
                End If
                cAtts = WHAttributes(AttData(0))
                cAtts.Add(AttData(1), AttData(2))
            End If
        Next
        ' Load the data
        Dim strSQL As String = "SELECT * from invTypes WHERE groupID=988;"
        Dim WHData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If WHData IsNot Nothing Then
                If WHData.Tables(0).Rows.Count > 0 Then
                    Dim cWH As New WormHole
                    VoidData.Wormholes.Clear()
                    For WH As Integer = 0 To WHData.Tables(0).Rows.Count - 1
                        cWH = New WormHole
                        cWH.ID = CStr(WHData.Tables(0).Rows(WH).Item("typeID"))
                        cWH.Name = CStr(WHData.Tables(0).Rows(WH).Item("typeName")).Replace("Wormhole ", "")
                        If WHAttributes.ContainsKey(cWH.ID) = True Then
                            For Each Att As String In WHAttributes(cWH.ID).Keys
                                Select Case Att
                                    Case "1381"
                                        cWH.TargetClass = WHAttributes(cWH.ID).Item(Att)
                                    Case "1382"
                                        cWH.MaxStabilityWindow = WHAttributes(cWH.ID).Item(Att)
                                    Case "1383"
                                        cWH.MaxMassCapacity = WHAttributes(cWH.ID).Item(Att)
                                    Case "1384"
                                        cWH.MassRegeneration = WHAttributes(cWH.ID).Item(Att)
                                    Case "1385"
                                        cWH.MaxJumpableMass = WHAttributes(cWH.ID).Item(Att)
                                    Case "1457"
                                        cWH.TargetDistributionID = WHAttributes(cWH.ID).Item(Att)
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
                        If cWH.Name.StartsWith("Test") = False Then
                            VoidData.Wormholes.Add(CStr(cWH.Name), cWH)
                        End If
                    Next
                    WHAttributes.Clear()
                    WHData.Dispose()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Wormhole Data for the Void Plugin" & ControlChars.CrLf & ex.Message, "Void Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function LoadWHSystemData() As Boolean
        ' Parse the location classes
        Dim WHClasses As New SortedList(Of String, String)
        Dim Classes() As String = My.Resources.WHClasses.Split((ControlChars.CrLf).ToCharArray)
        For Each WHClass As String In Classes
            If WHClass <> "" Then
                Dim ClassData() As String = WHClass.Split(",".ToCharArray)
                If WHClasses.ContainsKey(ClassData(0)) = False Then
                    WHClasses.Add(ClassData(0), ClassData(1))
                End If
            End If
        Next
        ' Parse the location effects
        Dim WHEffects As New SortedList(Of String, String)
        Dim Effects() As String = My.Resources.WSpaceTypes.Split((ControlChars.CrLf).ToCharArray)
        For Each WHEffect As String In Effects
            If WHEffect <> "" Then
                Dim EffectData() As String = WHEffect.Split(",".ToCharArray)
                If WHEffects.ContainsKey(EffectData(0)) = False Then
                    WHEffects.Add(EffectData(0), EffectData(1))
                End If
            End If
        Next
        ' Load the data
        Dim strSQL As String = "SELECT * FROM mapSolarSystems WHERE regionID>11000000;"
        Dim systemData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If systemData IsNot Nothing Then
                If systemData.Tables(0).Rows.Count > 0 Then
                    Dim cSystem As WormholeSystem = New WormholeSystem
                    VoidData.WormholeSystems.Clear()
                    For solar As Integer = 0 To systemData.Tables(0).Rows.Count - 1
                        cSystem = New WormholeSystem
                        cSystem.ID = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemID"))
                        cSystem.Name = CStr(systemData.Tables(0).Rows(solar).Item("solarSystemName"))
                        cSystem.Region = CStr(systemData.Tables(0).Rows(solar).Item("regionID"))
                        cSystem.Constellation = CStr(systemData.Tables(0).Rows(solar).Item("constellationID"))
                        cSystem.WClass = WHClasses(cSystem.Region)
                        If WHEffects.ContainsKey(cSystem.Name) = True Then
                            cSystem.WEffect = WHEffects(cSystem.Name)
                        Else
                            cSystem.WEffect = ""
                        End If
                        VoidData.WormholeSystems.Add(CStr(cSystem.Name), cSystem)
                    Next
                    WHClasses.Clear()
                    WHEffects.Clear()
                    systemData.Dispose()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading System Data for Prism Plugin" & ControlChars.CrLf & ex.Message, "Prism Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function LoadWHAttributeData() As Boolean
        ' Load the data
        Dim strSQL As String = "SELECT invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat"
        strSQL &= " FROM invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID"
        strSQL &= " WHERE (((invTypes.groupID)=920));"
        Dim WHData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        Try
            If WHData IsNot Nothing Then
                If WHData.Tables(0).Rows.Count > 0 Then
                    Dim cWH As New WormHole
                    VoidData.WormholeEffects.Clear()
                    Dim currentEffect As New WormholeEffect
                    For WH As Integer = 0 To WHData.Tables(0).Rows.Count - 1
                        Dim typeName As String = CStr(WHData.Tables(0).Rows(WH).Item("typeName"))
                        Dim attID As String = CStr(WHData.Tables(0).Rows(WH).Item("attributeID"))
                        Dim attValue As Double = 0
                        If IsDBNull(WHData.Tables(0).Rows(WH).Item("valueInt")) = False Then
                            attValue = CDbl(WHData.Tables(0).Rows(WH).Item("valueInt"))
                        Else
                            attValue = CDbl(WHData.Tables(0).Rows(WH).Item("valueFloat"))
                        End If
                        If VoidData.WormholeEffects.ContainsKey(typeName) = False Then
                            VoidData.WormholeEffects.Add(typeName, New WormholeEffect)
                        End If
                        currentEffect = VoidData.WormholeEffects(typeName)
                        currentEffect.WormholeType = typeName
                        currentEffect.Attributes.Add(attID, attValue)
                    Next
                    WHData.Dispose()
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("Error Loading Wormhole Effect Data for the Void Plugin" & ControlChars.CrLf & ex.Message, "Void Plug-in Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

End Class
