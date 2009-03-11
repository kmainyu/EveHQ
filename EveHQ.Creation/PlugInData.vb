Public Class PlugInData
    Implements EveHQ.Core.IEveHQPlugIn


    Public Shared raceData As New DataSet
    Public Shared bloodData As New DataSet
    Public Shared ancestryData As New DataSet
    Public Shared careerData As New DataSet
    Public Shared specData As New DataSet
    Public Shared raceSkillData As New DataSet
    Public Shared careerSkillData As New DataSet
    Public Shared specSkillData As New DataSet

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
        EveHQPlugIn.MenuImage = My.Resources.plugin_logo
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
        If raceData Is Nothing Or raceData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        bloodData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrBloodlines")
        If bloodData Is Nothing Or bloodData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        ancestryData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrAncestries")
        If ancestryData Is Nothing Or ancestryData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        careerData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrCareers")
        If careerData Is Nothing Or careerData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        specData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrCareerSpecialities")
        If specData Is Nothing Or specData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        raceSkillData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrRaceSkills")
        If raceSkillData Is Nothing Or raceSkillData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        careerSkillData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrCareerSkills")
        If careerSkillData Is Nothing Or careerSkillData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        specSkillData = EveHQ.Core.DataFunctions.GetData("SELECT * FROM chrCareerSpecialitySkills")
        If specSkillData Is Nothing Or specSkillData.Tables(0).Rows.Count = 0 Then
            Return False
            Exit Function
        End If
        Return True
    End Function

  
End Class
