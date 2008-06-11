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

Imports System.Runtime.InteropServices

Public Class HTML_XML

    Const ERROR_FILE_NOT_FOUND As Integer = 2

    <Runtime.InteropServices.DllImport("Wininet.dll", SetLastError:=True, CharSet:=CharSet.Auto)> Public Shared Function GetUrlCacheEntryInfo(ByVal lpszUrlName As String, ByVal lpCacheEntryInfo As IntPtr, ByRef lpdwCacheEntryInfoBufferSize As Integer) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)> Public Structure FILETIME
        Public dwLowDateTime As Integer
        Public dwHighDateTime As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> Public Structure LPINTERNET_CACHE_ENTRY_INFO
        Public dwStructSize As Integer
        Public lpszSourceUrlName As IntPtr
        Public lpszLocalFileName As IntPtr
        Public CacheEntryType As Integer
        Public dwUseCount As Integer
        Public dwHitRate As Integer
        Public dwSizeLow As Integer
        Public dwSizeHigh As Integer
        Public LastModifiedTime As FILETIME
        Public ExpireTime As FILETIME
        Public LastAccessTime As FILETIME
        Public LastSyncTime As FILETIME
        Public lpHeaderInfo As IntPtr
        Public dwHeaderInfoSize As Integer
        Public lpszFileExtension As IntPtr
        Public dwExemptDelta As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)> Public Structure SYSTEMTIME
        Public wYear As Short
        Public wMonth As Short
        Public wDayOfWeek As Short
        Public wDay As Short
        Public wHour As Short
        Public wMinute As Short
        Public wSecond As Short
        Public wMilliseconds As Short
    End Structure

    <Runtime.InteropServices.DllImport("kernel32")> Public Shared Function FileTimeToSystemTime(ByRef lpFileTime As FILETIME, ByRef lpSystemTime As SYSTEMTIME) As Boolean
    End Function

    Public Shared Function GetPathForCachedFile(ByVal fileUrl As String) As String
        Dim cacheEntryInfoBufferSize As Integer = 0
        Dim cacheEntryInfoBuffer As IntPtr = IntPtr.Zero
        Dim lastError As Integer
        Dim result As Boolean
        Try
            'call to see how big the buffer needs to be
            result = GetUrlCacheEntryInfo(fileUrl, IntPtr.Zero, cacheEntryInfoBufferSize)
            lastError = Marshal.GetLastWin32Error
            If result = False Then
                If lastError = ERROR_FILE_NOT_FOUND Then
                    Return Nothing
                Else
                    'noop
                End If
            End If

            'allocate the necessary amount of memory
            cacheEntryInfoBuffer = Marshal.AllocHGlobal(cacheEntryInfoBufferSize)

            'make call again with properly sized buffer
            result = GetUrlCacheEntryInfo(fileUrl, cacheEntryInfoBuffer, cacheEntryInfoBufferSize)
            lastError = Marshal.GetLastWin32Error

            If result = True Then
                Dim struct As Object = Marshal.PtrToStructure(cacheEntryInfoBuffer, GetType(LPINTERNET_CACHE_ENTRY_INFO))
                Dim internetCacheEntry As LPINTERNET_CACHE_ENTRY_INFO = CType(struct, LPINTERNET_CACHE_ENTRY_INFO)
                Dim localFileName As String = Marshal.PtrToStringAuto(internetCacheEntry.lpszLocalFileName)
                Return localFileName
            Else
                Throw New System.ComponentModel.Win32Exception(lastError)
            End If
        Finally
            If Not cacheEntryInfoBuffer.Equals(IntPtr.Zero) Then
                Marshal.FreeHGlobal(cacheEntryInfoBuffer)
            End If
        End Try
    End Function

End Class
