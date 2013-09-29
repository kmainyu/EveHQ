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

''' <summary>
''' Class for storing an instance of the custom ship classes used in the HQF Ship Editor
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CustomShipClass

    ''' <summary>
    ''' Gets or sets the name of the custom ship class
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the name of the custom ship class</returns>
    ''' <remarks></remarks>
    Public Property Name() As String
      
    ''' <summary>
    ''' Gets or sets the description of the custom ship class
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string containing the description of the custom ship class</returns>
    ''' <remarks></remarks>
    Public Property Description() As String
       
End Class
