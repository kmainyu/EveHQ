' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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

Imports System.IO

''' <summary>
''' Class for handling various routines relating to the EveCacheParser library
''' </summary>
''' <remarks></remarks>
Public Class EveCacheParserFunctions

    ' Define constant to Eve's program and market cache locations
    Const EveSettingsPath As String = "CCP\EVE"
    Const CacheFolderPath As String = "cache\MachoNet"
    Const TranquilityIP As String = "87.237.38.200"
    Const SingularityIP As String = "87.237.38.50"
    Const DualityIP As String = "87.237.38.60"
    Const BuckinghamIP As String = "87.237.38.69"
    'Const ServerName As String = "_tranquility"
    Const CacheType As String = "CachedMethodCalls"

    ''' <summary>
    ''' Provides a location to the Eve market cache files given the program folder location
    ''' </summary>
    ''' <param name="programFolder">The location of the Eve executable (usually "C:\Program Files\CCP\Eve")</param>
    ''' <param name="serverName">The Eve server which relates to the executable (TQ, Sisi etc)</param>
    ''' <returns>A string containing the location of the Eve markert cache files</returns>
    ''' <remarks>Returns a blank string if no path can be found or calculated</remarks>
    Public Shared Function GetEveMarketCacheLocation(programFolder As String, serverName As EveServerName) As String

        ' Parse location to the base cache path
        Dim serverIP As String
        Select Case serverName
            Case EveServerName.Singularity
                serverIP = SingularityIP
            Case EveServerName.Duality
                serverIP = DualityIP
            Case EveServerName.Buckingham
                serverIP = BuckinghamIP
            Case Else
                serverIP = TranquilityIP
        End Select
        Dim server As String = "_" & ([Enum].GetName(GetType(EveServerName), serverName)).ToLower
        Dim basePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), EveSettingsPath)
        Dim refinedProgramFolder As String = (programFolder.Replace(":", "").Replace("\", "_").Replace("/", "_").Replace(" ", "_") & server).ToLower
        Dim baseCachePath As String = Path.Combine(Path.Combine(basePath, refinedProgramFolder), Path.Combine(CacheFolderPath, serverIP))

        ' Find folders in the base cache path and extract the latest protocol version
        If My.Computer.FileSystem.DirectoryExists(baseCachePath) = True Then
            Dim protocols As List(Of String) = My.Computer.FileSystem.GetDirectories(baseCachePath).ToList
            If protocols.Count > 0 Then
                Dim protocolVersions As New List(Of Integer)
                For Each protocol As String In protocols
                    protocolVersions.Add(CInt(protocol.Split(Path.DirectorySeparatorChar).Last()))
                Next
                protocolVersions.Sort()
                protocolVersions.Reverse()
                Return Path.Combine(Path.Combine(baseCachePath, protocolVersions(0).ToString), CacheType)
            Else
                Return ""
            End If
        Else
            Return ""
        End If

    End Function

    ''' <summary>
    ''' Provides a location to the Eve cache files given the program folder location
    ''' </summary>
    ''' <param name="programFolder">The location of the Eve executable (usually "C:\Program Files\CCP\Eve")</param>
    ''' <param name="serverName">The Eve server which relates to the executable (TQ, Sisi etc)</param>
    ''' <returns>A string containing the location of the Eve markert cache files</returns>
    ''' <remarks>Returns a blank string if no path can be found or calculated</remarks>
    Public Shared Function GetEveCacheFiles(programFolder As String, serverName As EveServerName) As FileInfo()

        ' Parse location to the base cache path
        Dim serverIP As String
        Select Case serverName
            Case EveServerName.Singularity
                serverIP = SingularityIP
            Case EveServerName.Duality
                serverIP = DualityIP
            Case EveServerName.Buckingham
                serverIP = BuckinghamIP
            Case Else
                serverIP = TranquilityIP
        End Select
        Dim server As String = "_" & ([Enum].GetName(GetType(EveServerName), serverName)).ToLower
        Dim basePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), EveSettingsPath)
        Dim refinedProgramFolder As String = (programFolder.Replace(":", "").Replace("\", "_").Replace("/", "_").Replace(" ", "_") & server).ToLower
        Dim baseCachePath As String = Path.Combine(Path.Combine(basePath, refinedProgramFolder), Path.Combine(CacheFolderPath, serverIP))

        ' Find folders in the base cache path and extract the latest protocol version
        If My.Computer.FileSystem.DirectoryExists(baseCachePath) = True Then
            Dim protocols As List(Of String) = My.Computer.FileSystem.GetDirectories(baseCachePath).ToList
            If protocols.Count > 0 Then
                Dim protocolVersions As New List(Of Integer)
                For Each protocol As String In protocols
                    protocolVersions.Add(CInt(protocol.Split(Path.DirectorySeparatorChar).Last()))
                Next
                protocolVersions.Sort()
                protocolVersions.Reverse()

                Return (From f In Directory.GetFiles(Path.Combine(Path.Combine(baseCachePath, protocolVersions(0).ToString), CacheType)) Select New FileInfo(f)).ToArray

            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Provides the protocol number of the last Eve installation that has been run
    ''' </summary>
    ''' <param name="programFolder">The location of the Eve executable (usually "C:\Program Files\CCP\Eve")</param>
    ''' <param name="serverName">The Eve server which relates to the executable (TQ, Sisi etc)</param>
    ''' <returns>An integer representing the latest available protocol version of Eve</returns>
    ''' <remarks>The protocol number will be the last version of Eve that has been run, not necessarily that which is currently installed. Returns zero if not valid or found.</remarks>
    Public Shared Function GetEveCacheProtocol(programFolder As String, serverName As EveServerName) As Integer

        ' Parse location to the base cache path
        Dim serverIP As String
        Select Case serverName
            Case EveServerName.Singularity
                serverIP = SingularityIP
            Case EveServerName.Duality
                serverIP = DualityIP
            Case EveServerName.Buckingham
                serverIP = BuckinghamIP
            Case Else
                serverIP = TranquilityIP
        End Select
        Dim server As String = "_" & ([Enum].GetName(GetType(EveServerName), serverName)).ToLower
        Dim basePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), EveSettingsPath)
        Dim refinedProgramFolder As String = (programFolder.Replace(":", "").Replace("\", "_").Replace("/", "_").Replace(" ", "_") & server).ToLower
        Dim baseCachePath As String = Path.Combine(Path.Combine(basePath, refinedProgramFolder), Path.Combine(CacheFolderPath, serverIP))

        ' Find folders in the base cache path and extract the latest protocol version
        If My.Computer.FileSystem.DirectoryExists(baseCachePath) = True Then
            Dim protocols As List(Of String) = My.Computer.FileSystem.GetDirectories(baseCachePath).ToList
            If protocols.Count > 0 Then
                Dim protocolVersions As New List(Of Integer)
                For Each protocol As String In protocols
                    protocolVersions.Add(CInt(protocol.Split(Path.DirectorySeparatorChar).Last()))
                Next
                protocolVersions.Sort()
                protocolVersions.Reverse()
                ' Return the highest protocol number
                Return protocolVersions(0)
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function

End Class

Public Enum EveServerName
    Tranquility = 0
    Singularity = 1
    Duality = 2
    Buckingham = 3
End Enum
