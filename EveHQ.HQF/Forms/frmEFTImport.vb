Imports System.Windows.Forms
Imports System.IO

Public Class frmEFTImport
    Dim cfgFiles As New ArrayList
    Dim EFTFiles As New ArrayList
    Dim startDir As String = ""

    Private Sub btnScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScan.Click
        ' Check for a valid directory
        If My.Computer.FileSystem.DirectoryExists(txtStartDir.Text) = True Then
            cfgFiles.Clear()
            btnBrowse.Enabled = False
            btnScan.Enabled = False
            Dim dirInfo As New DirectoryInfo(startDir)
            ListFiles(dirInfo)
            ' Search through the list of files and see if they contain valid data
            Dim filetext As String = ""
            Dim fileInf As FileInfo
            Dim shipName As String = ""
            Dim sr As StreamReader
            Dim fittings() As String
            EFTFiles.Clear()
            For Each filename As String In cfgFiles
                fileInf = New FileInfo(filename)
                lblScan.Text = "Checking file: " & filename
                Me.Refresh()
                Application.DoEvents()
                ' Lets check the filename for a ship type
                shipName = fileInf.Name.TrimEnd(".cfg".ToCharArray)
                If ShipLists.shipList.Contains(shipName) = True Then
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
                            Call Me.ImportFitting(shipName, fitting)
                        Next
                        EFTFiles.Add(filename)
                    End If
                End If
            Next
            lblScan.Text = "Currently scanning: Scanning Complete!"
            Me.Refresh()
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
            Me.Refresh()
            Application.DoEvents()
            For Each filename As String In My.Computer.FileSystem.GetFiles(dirInfo.FullName, FileIO.SearchOption.SearchTopLevelOnly, "*.cfg")
                cfgFiles.Add(filename)
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

    Private Sub frmEFTImport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        startDir = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        txtStartDir.Text = startDir
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With fbd1
            .Description = "Please select the start folder to locate your EFT setup files..."
            .ShowNewFolderButton = True
            .RootFolder = Environment.SpecialFolder.Desktop
            .ShowDialog()
            txtStartDir.Text = .SelectedPath
        End With
    End Sub

    Private Sub txtStartDir_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStartDir.TextChanged
        startDir = txtStartDir.Text
    End Sub

    Private Sub ImportFitting(ByVal shipName As String, ByVal filetext As String)
        ' Use Regex to get the data - No checking as this is done in the tmrClipboard_Tick sub
        Dim fittingMatch As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(filetext, "\[(?<FittingName>.*)\]")
        Dim fittingName As String = ""
        If fittingMatch.Groups.Item("FittingName").Value <> "" Then
            fittingName = fittingMatch.Groups.Item("FittingName").Value
        Else
            fittingName = "EFT Imported Fit"
        End If
        ' If the fitting exists, add a number onto the end
        If Fittings.FittingList.ContainsKey(shipName & ", " & fittingName) = True Then
            'Dim response As Integer = MessageBox.Show("Fitting name already exists. Are you sure you wish to import the fitting?", "Confirm Import for " & shipName, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            'If response = Windows.Forms.DialogResult.Yes Then
            Dim newFittingName As String = ""
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
        For Each ShipMod As String In mods
            If ShipMod.StartsWith("[") = False And ShipMod <> "" Then
                ' Check for "Drones_" label
                If ShipMod.StartsWith("Drones_") Then
                    ShipMod = ShipMod.TrimStart("Drones_Active=".ToCharArray)
                    ShipMod = ShipMod.TrimStart("Drones_Inactive=".ToCharArray)
                End If
                newFit.Add(ShipMod)
            End If
        Next
        Fittings.FittingList.Add(shipName & ", " & fittingName, newFit)
    End Sub
End Class