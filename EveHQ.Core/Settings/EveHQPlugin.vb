Public Class EveHQPlugIn

    Public Name As String
    Public Description As String
    Public Author As String
    Public MainMenuText As String
    Public MenuImage As Drawing.Image
    Public RunAtStartup As Boolean
    Public RunInIGB As Boolean
    Public FileName As String
    Public ShortFileName As String
    Public FileType As String
    Public Version As String
    Public Disabled As Boolean
    Public Available As Boolean
    Public Status As Integer
    Public Instance As IEveHQPlugIn
    Public PostStartupData As Object

    Public Enum PlugInStatus
        Uninitialised = 0
        Loading = 1
        Failed = 2
        Active = 3
    End Enum

End Class
