<Serializable()> Public Class CertificateCategory
    Public ID As Integer
    Public Name As String
End Class
<Serializable()> Public Class CertificateClass
    Public ID As Integer
    Public Name As String
End Class
<Serializable()> Public Class Certificate
    Public ID As Integer
    Public Grade As Integer
    Public ClassID As Integer
    Public CategoryID As Integer
    Public CorpID As Long
    Public Description As String
    Public RequiredSkills As New SortedList
    Public RequiredCerts As New SortedList
End Class
