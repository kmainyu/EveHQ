Namespace BPCalc

    Public Class BPAssetComboboxItem

        Public Property Name As String = ""
        Public Property AssetID As String = ""
        Public Property MELevel As Integer
        Public Property PELevel As Integer
        Public Property Runs As Integer

        Public Sub New(ByVal cName As String, ByVal cAssetID As String, ByVal cME As Integer, ByVal cPE As Integer, ByVal cRuns As Integer)
            Name = cName
            AssetID = cAssetID
            MELevel = cME
            PELevel = cPE
            Runs = cRuns
        End Sub

        Public Overrides Function ToString() As String
            Return Name & " (ME:" & MELevel.ToString & ", PE:" & PELevel.ToString & ", Runs:" & Runs.ToString & ")"
        End Function

    End Class

End Namespace