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
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports SearchOption = Microsoft.VisualBasic.FileIO.SearchOption

Namespace Forms

    Public Class FrmEftImport
        ReadOnly _cfgFiles As New ArrayList
        ReadOnly _eftFiles As New ArrayList
        Dim _startDir As String = ""

        Private Sub btnScan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnScan.Click
            ' Check for a valid directory
            If My.Computer.FileSystem.DirectoryExists(txtStartDir.Text) = True Then
                _cfgFiles.Clear()
                btnBrowse.Enabled = False
                btnScan.Enabled = False
                Dim dirInfo As New DirectoryInfo(_startDir)
                ListFiles(dirInfo)
                ' Search through the list of files and see if they contain valid data
                Dim filetext As String
                Dim fileInf As FileInfo
                Dim shipName As String
                Dim sr As StreamReader
                Dim fittings() As String
                _eftFiles.Clear()
                For Each filename As String In _cfgFiles
                    fileInf = New FileInfo(filename)
                    lblScan.Text = "Checking file: " & filename
                    Refresh()
                    Application.DoEvents()
                    ' Lets check the filename for a ship type
                    shipName = fileInf.Name.TrimEnd(".cfg".ToCharArray)
                    If ShipLists.ShipList.ContainsKey(shipName) = True Then
                        ' Valid shiptype so lets see if we can parse the file
                        sr = New StreamReader(filename)
                        filetext = sr.ReadToEnd
                        sr.Close()
                        ' Split the file into sections separated by "["
                        fittings = filetext.Split("[".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                        If fittings.Length > 0 Then
                            Dim newFile As New ListViewItem
                            newFile.Text = filename
                            newFile.Name = filename
                            newFile.SubItems.Add(fittings.Length.ToString)
                            lvwFiles.Items.Add(newFile)
                            ' Scan through each fitting and add it to the list
                            For Each fitting As String In fittings
                                fitting = "[" & fitting
                                Call ImportFitting(shipName, fitting)
                            Next
                            _eftFiles.Add(filename)
                        End If
                    End If
                Next
                lblScan.Text = "Currently scanning: Scanning Complete!"
                Refresh()
                MessageBox.Show("EFT Import Complete! Please close the form to view and use your imported setups.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Start Directory is not valid, please try again.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            btnBrowse.Enabled = True
            btnScan.Enabled = True
        End Sub

        Private Sub ListFiles(ByVal dirInfo As DirectoryInfo)
            ' Get the files in this directory.
            Try
                lblScan.Text = "Currently scanning: " & dirInfo.FullName
                Refresh()
                Application.DoEvents()
                For Each filename As String In My.Computer.FileSystem.GetFiles(dirInfo.FullName, SearchOption.SearchTopLevelOnly, "*.cfg")
                    _cfgFiles.Add(filename)
                Next

                ' Search subdirectories.
                Dim subdirs() As DirectoryInfo = _
                        dirInfo.GetDirectories()
                For Each subdir As DirectoryInfo In subdirs
                    ListFiles(subdir)
                Next subdir
            Catch e As Exception
            End Try
        End Sub

        Private Sub frmEFTImport_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            _startDir = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            txtStartDir.Text = _startDir
        End Sub

        Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowse.Click
            With fbd1
                .Description = "Please select the start folder to locate your EFT setup files..."
                .ShowNewFolderButton = True
                .RootFolder = Environment.SpecialFolder.Desktop
                .ShowDialog()
                txtStartDir.Text = .SelectedPath
            End With
        End Sub

        Private Sub txtStartDir_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtStartDir.TextChanged
            _startDir = txtStartDir.Text
        End Sub

        Private Sub ImportFitting(ByVal shipName As String, ByVal filetext As String)
            ' Use Regex to get the data - No checking as this is done in the tmrClipboard_Tick sub
            Dim fittingMatch As Match = Regex.Match(filetext, "\[(?<FittingName>.*)\]")
            Dim fittingName As String
            If fittingMatch.Groups.Item("FittingName").Value <> "" Then
                fittingName = fittingMatch.Groups.Item("FittingName").Value
            Else
                fittingName = "EFT Imported Fit"
            End If
            ' If the fitting exists, add a number onto the end
            If Fittings.FittingList.ContainsKey(shipName & ", " & fittingName) = True Then
                'Dim response As Integer = MessageBox.Show("Fitting name already exists. Are you sure you wish to import the fitting?", "Confirm Import for " & shipName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                'If response = Windows.Forms.DialogResult.Yes Then
                Dim newFittingName As String
                Dim revision As Integer = 1
                Do
                    revision += 1
                    newFittingName = fittingName & " " & revision.ToString
                Loop Until Fittings.FittingList.ContainsKey(shipName & ", " & newFittingName) = False
                fittingName = newFittingName
                'MessageBox.Show("New fitting name is '" & fittingName & "'.", "New Fitting Imported", MessageBoxButtons.OK, MessageBoxIcon.Information)
                'Else
                'Exit Sub
                'End If
            End If
            ' Lets create the fitting
            Dim mods() As String = filetext.Split(ControlChars.CrLf.ToCharArray)
            Dim newFit As New ArrayList
            For Each shipMod As String In mods
                If shipMod.StartsWith("[") = False And shipMod <> "" Then
                    ' Check for "Drones_" label
                    If shipMod.StartsWith("Drones_") Then
                        shipMod = shipMod.TrimStart("Drones_Active=".ToCharArray)
                        shipMod = shipMod.TrimStart("Drones_Inactive=".ToCharArray)
                    End If
                    newFit.Add(shipMod)
                End If
            Next
            Dim newFitting As Fitting = Fittings.ConvertOldFitToNewFit(shipName & ", " & fittingName, newFit)
            Fittings.FittingList.Add(newFitting.KeyName, newFitting)
        End Sub
    End Class
End NameSpace