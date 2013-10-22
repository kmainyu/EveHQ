' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================

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