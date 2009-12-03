Imports System.Windows.Forms

Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn

    Public Shared raceData As New DataSet
    Public Shared bloodData As New DataSet
    Public Shared ancestryData As New DataSet

    Public Function GetPlugInData(ByVal Data As Object, ByVal DataType As Integer) As Object Implements Core.IEveHQPlugIn.GetPlugInData
        Return Nothing
    End Function

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Return LoadData()
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "EveHQ Character Creation"
        EveHQPlugIn.Description = "Helps determine the best character to start Eve with"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "Character Creation"
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
        Return New frmCharCreate
    End Function

    Private Function LoadData() As Boolean
        raceData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrRaces")
        If raceData Is Nothing Then
            MessageBox.Show("chrRaces table returned a null dataset.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            If raceData.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("chrRaces table returned no rows.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
        bloodData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrBloodlines")
        If bloodData Is Nothing Then
            MessageBox.Show("chrBloodlines table returned a null dataset.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            If bloodData.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("chrBloodlines table returned no rows.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
        ancestryData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrAncestries")
        If ancestryData Is Nothing Then
            MessageBox.Show("chrAncestries table returned a null dataset.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        Else
            If ancestryData.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("chrAncestries table returned no rows.", "Character Creation Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If
        Return True
    End Function


End Class
