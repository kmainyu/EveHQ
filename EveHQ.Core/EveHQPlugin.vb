Imports System.Drawing

Public Class EveHQPlugIn

    Public Property Name As String
    Public Property Description As String
    Public Property Author As String
    Public Property MainMenuText As String
    Public Property MenuImage As Image
    Public Property RunAtStartup As Boolean
    Public Property RunInIGB As Boolean
    Public Property FileName As String
    Public Property ShortFileName As String
    Public Property FileType As String
    Public Property Version As String
    Public Property Disabled As Boolean
    Public Property Available As Boolean
    Public Property Status As EveHQPlugInStatus
    Public Property Instance As IEveHQPlugIn
    Public Property PostStartupData As Object

End Class

Public Enum EveHQPlugInStatus
    Uninitialised = 0
    Loading = 1
    Failed = 2
    Active = 3
End Enum