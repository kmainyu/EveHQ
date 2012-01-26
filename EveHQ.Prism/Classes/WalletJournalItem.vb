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

Public Class WalletJournalItem

    Public JournalDate As Date
    Public RefID As Long
    Public RefTypeID As Integer
    Public OwnerName1 As String
    Public OwnerID1 As String
    Public OwnerName2 As String
    Public OwnerID2 As String
    Public ArgName1 As String
    Public ArgID1 As String
    Public Amount As Double
    Public Balance As Double
    Public Reason As String
    Public TaxReceiverID As String
    Public TaxAmount As Double

End Class

Public Class WalletJournalDiff
	Public OwnerID As Integer
	Public OwnerName As String
	Public WalletID As Integer
	Public PrevDate As Date
	Public CurrDate As Date
	Public PrevKey As String
	Public CurrKey As String
	Public PrevRef As Long
	Public CurrRef As Long
	Public Amount As Double
	Public TaxAmount As Double
	Public PrevBal As Double
	Public CurrBal As Double
	Public Difference As Double
End Class
