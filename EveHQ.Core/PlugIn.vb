' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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
<Serializable()> Public Class PlugIn

    Public Name As String
    <NonSerialized()> Public Description As String
    <NonSerialized()> Public Author As String
    <NonSerialized()> Public MainMenuText As String
    <NonSerialized()> Public MenuImage As Drawing.Image
    <NonSerialized()> Public RunAtStartup As Boolean
    <NonSerialized()> Public RunInIGB As Boolean
    <NonSerialized()> Public FileName As String
    <NonSerialized()> Public ShortFileName As String
    <NonSerialized()> Public FileType As String
    <NonSerialized()> Public Version As String
    Public Disabled As Boolean
    <NonSerialized()> Public Available As Boolean
    <NonSerialized()> Public Status As Integer
    <NonSerialized()> Public Instance As IEveHQPlugIn
    <NonSerialized()> Public PostStartupData As Object

    Public Enum PlugInStatus
        Uninitialised = 0
        Loading = 1
        Failed = 2
        Active = 3
    End Enum

End Class
