Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn
    Public Shared PlugInDataObject As Object
    Public Shared Event PluginDataReceived()

    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return PlugInDataObject
        End Get
        Set(ByVal value As Object)
            PlugInDataObject = value
            RaiseEvent PluginDataReceived()
        End Set
    End Property

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Try
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

