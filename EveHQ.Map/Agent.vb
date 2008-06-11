<Serializable()> Public Class Agent
    Public agentID As Integer
    Public agentName As String 'from evenames
    Public divisionID As Integer
    Public divisionName As String 'from div id table
    Public corporationID As Integer
    Public stationId As Integer
    Public Level As Integer
    Public Quality As Integer
    Public Type As String 'use case statement
    Public Locator As Boolean 'get from db and set
    Public Flag As Boolean
End Class
