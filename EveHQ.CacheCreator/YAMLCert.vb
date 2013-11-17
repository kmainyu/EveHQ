Public Class YAMLCert
    Public Property CertID As Integer
    Public Property Description As String
    Public Property GroupID As Integer
    Public Property Name As String
    Public Property RecommendedFor As IEnumerable(Of Integer)
    Public Property RequiredSkills As IEnumerable(Of CertReqSkill)




End Class

Public Class CertReqSkill
    Public Property SkillID As Integer
    Public Property SkillLevels As IDictionary(Of String, Integer)
End Class
