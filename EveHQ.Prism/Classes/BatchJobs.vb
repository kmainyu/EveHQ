'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.IO
Imports System.Windows.Forms
Imports Newtonsoft.Json

Namespace Classes

    Public Class BatchJobs

        Public Shared Jobs As New SortedList(Of String, BatchJob)

        Private Const MainFileName As String = "BatchJobs.json"
        Private Shared ReadOnly LockObj As New Object

        Public Shared Sub SaveBatchJobs()

            SyncLock LockObj
                Dim newFile As String = Path.Combine(PrismSettings.PrismFolder, MainFileName)
                Dim tempFile As String = Path.Combine(PrismSettings.PrismFolder, MainFileName & ".temp")

                ' Create a JSON string for writing
                Dim json As String = JsonConvert.SerializeObject(Jobs, Formatting.Indented)

                ' Write the JSON version of the settings
                Try
                    Using s As New StreamWriter(tempFile, False)
                        s.Write(json)
                        s.Flush()
                    End Using

                    If File.Exists(newFile) Then
                        File.Delete(newFile)
                    End If

                    File.Move(tempFile, newFile)

                Catch e As Exception

                End Try

            End SyncLock

        End Sub

        Public Shared Function LoadBatchJobs() As Boolean
            SyncLock LockObj

                If My.Computer.FileSystem.FileExists(Path.Combine(PrismSettings.PrismFolder, MainFileName)) = True Then
                    Try
                        Using s As New StreamReader(Path.Combine(PrismSettings.PrismFolder, MainFileName))
                            Dim json As String = s.ReadToEnd
                            Jobs = JsonConvert.DeserializeObject(Of SortedList(Of String, BatchJob))(json)
                            PrismEvents.StartUpdateBatchJobs()
                        End Using
                    Catch ex As Exception
                        Dim msg As String = "There was an error trying to load the Batch Jobs and it appears that this file is corrupt." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "Prism will delete this file and re-initialise the file." & ControlChars.CrLf & ControlChars.CrLf
                        msg &= "Press OK to reset the batch Jobs file." & ControlChars.CrLf
                        MessageBox.Show(msg, "Invalid Batch Jobs file detected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End Try
                End If

                Return True

            End SyncLock
        End Function
    End Class
End Namespace