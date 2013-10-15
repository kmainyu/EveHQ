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
Imports System.Net
Imports System.Windows.Forms

Public Interface IEveHQPlugIn

    Function GetEveHQPlugInInfo() As EveHQPlugIn
    Function EveHQStartUp() As Boolean
    Function IGBService(ByVal igbContext As HttpListenerContext) As String
    Function RunEveHQPlugIn() As Form
    Function GetPlugInData(ByVal data As Object, ByVal dataType As Integer) As Object
    Function SaveAll() As Boolean

End Interface
