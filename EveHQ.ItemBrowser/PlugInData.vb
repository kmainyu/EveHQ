Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn

    Public Shared PlugInDataObject As Object
    Public Shared AttributeList As New SortedList
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
            Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData("SELECT * FROM dgmAttributeTypes ORDER BY attributeName;")
            For item As Integer = 0 To eveData.Tables(0).Rows.Count - 1
                PlugInData.AttributeList.Add(eveData.Tables(0).Rows(item).Item("attributeName").ToString.Trim, eveData.Tables(0).Rows(item).Item("attributeID").ToString.Trim)
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
        EveHQPlugIn.RunAtStartup = False
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

