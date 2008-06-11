' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Public Class PlugIn

    Public Name As String
    Public Description As String
    Public Author As String
    Public MainMenuText As String
    Public MenuImage As Drawing.Image
    Public RunAtStartup As Boolean
    Public RunInIGB As Boolean
    Public FileName As String
    Public ShortFileName As String
    Public FileType As String
    Public Version As String
    Public Disabled As Boolean
    Public Available As Boolean
    Public Status As Integer
    Public Instance As IEveHQPlugIn

    Public Enum PlugInStatus
        Uninitialised = 0
        Loading = 1
        Failed = 2
        Active = 3
    End Enum

End Class
