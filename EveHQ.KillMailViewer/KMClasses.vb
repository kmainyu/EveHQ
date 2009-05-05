Public Class KMItem
    Public typeID As String
    Public flag As Integer
    Public qtyDropped As Integer
    Public qtyDestroyed As Integer
End Class

Public Class KMVictim
    Public charID As String
    Public charName As String
    Public corpID As String
    Public corpName As String
    Public allianceID As String
    Public allianceName As String
    Public factionID As String
    Public factionName As String
    Public damageTaken As Double
    Public shipTypeID As String
End Class

Public Class KMAttacker
    Public charID As String
    Public charName As String
    Public corpID As String
    Public corpName As String
    Public allianceID As String
    Public allianceName As String
    Public factionID As String
    Public factionName As String
    Public secStatus As Double
    Public damageDone As Double
    Public finalBlow As Boolean
    Public weaponTypeID As String
    Public shipTypeID As String
End Class

Public Class KillMail
    Public killID As String
    Public systemID As String
    Public killTime As Date
    Public moonID As String
    Public Victim As KMVictim
    Public Attackers As New SortedList
    Public Items As New ArrayList
End Class

Public Class SolarSystem
    Public ID As Integer
    Public Name As String
    Public Security As Double
    Public Region As String
    Public Constellation As String
End Class


