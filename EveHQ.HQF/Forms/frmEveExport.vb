Imports System.Windows.Forms
Imports System.Xml

Public Class frmEveExport

    Dim EveFolder As String = (My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Eve\fittings").Replace("\\", "\")
    Dim EveFile As String = ""
    Dim cFittingList As New ArrayList

    Public Property FittingList() As ArrayList
        Get
            Return cFittingList
        End Get
        Set(ByVal value As ArrayList)
            cFittingList = value
            UpdateFittingList()
        End Set
    End Property

    Private Sub UpdateFittingList()
        lvwFittings.BeginUpdate()
        lvwFittings.Items.Clear()
        For Each shipFit As String In cFittingList
            lvwFittings.Items.Add(shipFit)
        Next
        lvwFittings.EndUpdate()
    End Sub

    Private Sub frmEveExport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblEveFolder.Text = EveFolder
    End Sub

    Private Function ExportFittingsToEveFile() As Boolean
        Dim fitXML As New XmlDocument
        Dim currentFit As New ArrayList
        Dim dec As XmlDeclaration = fitXML.CreateXmlDeclaration("1.0", Nothing, Nothing)
        Dim fitAtt As XmlAttribute

        fitXML.AppendChild(dec)
        Dim fitRoot As XmlElement = fitXML.CreateElement("fittings")
        fitXML.AppendChild(fitRoot)

        If FittingList.Count > 0 Then
            For Each shipFit As String In cFittingList
                Dim fittingSep As Integer = shipFit.IndexOf(", ")
                Dim shipName As String = shipFit.Substring(0, fittingSep)
                Dim fittingName As String = shipFit.Substring(fittingSep + 2)
                currentFit = CType(Fittings.FittingList.Item(shipFit), ArrayList)

                Dim fit As XmlNode = fitXML.CreateElement("fitting")
                fitAtt = fitXML.CreateAttribute("name")
                fitAtt.Value = fittingName
                fit.Attributes.Append(fitAtt)
                fitRoot.AppendChild(fit)

                Dim desc As XmlNode = fitXML.CreateElement("description")
                fitAtt = fitXML.CreateAttribute("value")
                fitAtt.Value = fittingName
                desc.Attributes.Append(fitAtt)
                fit.AppendChild(desc)

                Dim shiptype As XmlNode = fitXML.CreateElement("shipType")
                fitAtt = fitXML.CreateAttribute("value")
                fitAtt.Value = shipName
                shiptype.Attributes.Append(fitAtt)
                fit.AppendChild(shiptype)

                Call ExportFittingList(currentFit, fitXML, fit)

            Next

            ' Check for the fittings directory and create it
            Try
                If My.Computer.FileSystem.DirectoryExists(EveFolder) = False Then
                    Dim reply As Integer = MessageBox.Show("The Eve fittings folder is not present on your system and is required for the saving of fittings. Would you like to create this folder now?", "Create Fittings Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If reply = DialogResult.No Then
                        MessageBox.Show("Unable to export HQF fittings due to missing folder.", "Export Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    Else
                        My.Computer.FileSystem.CreateDirectory(EveFolder)
                    End If
                End If
                fitXML.Save(EveFile)
                MessageBox.Show("Export of HQF Fittings to " & EveFile & " completed", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return True
            Catch e As Exception
                MessageBox.Show("Unable to export HQF fittings to Eve folder: " & e.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End Try
        End If
    End Function

    Private Sub ExportFittingList(ByVal fitting As ArrayList, ByRef fitXML As XmlDocument, ByRef fitNode As XmlNode)
        Dim hislot, medslot, lowslot, rigslot As Integer
        Dim hardware As XmlNode
        Dim hardwareAtt As XmlAttribute
        Dim slotGroup As String = ""
        Dim slotNo As Integer = 0
        For Each shipMod As String In fitting
            If shipMod IsNot Nothing Then
                ' Check for installed charges
                Dim modData() As String = shipMod.Split(",".ToCharArray)
                Dim state As Integer = 4
                Dim itemQuantity As Integer = 1
                If modData(0).Length > 2 Then
                    If modData(0).Substring(modData(0).Length - 2, 1) = "_" Then
                        state = CInt(modData(0).Substring(modData(0).Length - 1, 1))
                        modData(0) = modData(0).TrimEnd(("_" & state.ToString).ToCharArray)
                        state = CInt(Math.Pow(2, state))
                    End If
                End If
                ' Check for item quantity (EFT method)
                Dim qSep As Integer = InStrRev(modData(0), " ")
                If qSep > 0 Then
                    Dim qString As String = modData(0).Substring(qSep)
                    If qString.StartsWith("x") Then
                        qString = qString.TrimStart("x".ToCharArray)
                        If IsNumeric(qString) = True Then
                            itemQuantity = CInt(qString)
                            modData(0) = modData(0).TrimEnd((" x" & itemQuantity.ToString).ToCharArray)
                        End If
                    End If
                End If
                ' Check if the module exists
                If ModuleLists.moduleListName.ContainsKey(modData(0)) = True Then
                    Dim modID As String = ModuleLists.moduleListName(modData(0).Trim).ToString
                    Dim sMod As ShipModule = CType(ModuleLists.moduleList(modID), ShipModule).Clone
                    If modData.GetUpperBound(0) > 0 Then
                        ' Check if a charge (will be a valid item)
                        If ModuleLists.moduleListName.Contains(modData(1).Trim) = True Then
                            Dim chgID As String = ModuleLists.moduleListName(modData(1).Trim).ToString
                            sMod.LoadedCharge = CType(ModuleLists.moduleList(chgID), ShipModule).Clone
                        End If
                    End If
                    ' Check if module is nothing
                    If sMod IsNot Nothing Then
                        ' Check if module is a drone
                        If sMod.IsDrone = True Then
                            Dim active As Boolean = False
                            If modData.GetUpperBound(0) > 0 Then
                                If modData(1).EndsWith("a") = True Then
                                    active = True
                                    itemQuantity = CInt(modData(1).Substring(0, Len(modData(1)) - 1))
                                Else
                                    If modData(1).EndsWith("i") = True Then
                                        itemQuantity = CInt(modData(1).Substring(0, Len(modData(1)) - 1))
                                    Else
                                        itemQuantity = CInt(modData(1))
                                    End If
                                End If
                            End If
                            ' Add the XML data
                            hardware = fitXML.CreateElement("hardware")
                            hardwareAtt = fitXML.CreateAttribute("qty") : hardwareAtt.Value = itemQuantity.ToString
                            hardware.Attributes.Append(hardwareAtt)
                            hardwareAtt = fitXML.CreateAttribute("slot") : hardwareAtt.Value = "drone bay"
                            hardware.Attributes.Append(hardwareAtt)
                            hardwareAtt = fitXML.CreateAttribute("type") : hardwareAtt.Value = sMod.Name
                            hardware.Attributes.Append(hardwareAtt)
                            fitNode.AppendChild(hardware)
                            'Call Me.AddDrone(sMod, itemQuantity, active)
                        Else
                            ' Check if module is a charge
                            If sMod.IsCharge = True Or sMod.IsContainer Then
                                If modData.GetUpperBound(0) > 0 Then
                                    itemQuantity = CInt(modData(1))
                                End If
                                'Call Me.AddItem(sMod, itemQuantity)
                            Else
                                ' Must be a proper module then!
                                sMod.ModuleState = state
                                Select Case sMod.SlotType
                                    Case 8 ' High
                                        slotGroup = "hi slot "
                                        slotNo = hislot
                                        hislot += 1
                                    Case 4 ' Mid
                                        slotGroup = "med slot "
                                        slotNo = medslot
                                        medslot += 1
                                    Case 2 ' Low
                                        slotGroup = "low slot "
                                        slotNo = lowslot
                                        lowslot += 1
                                    Case 1 ' Rig
                                        slotGroup = "rig slot "
                                        slotNo = rigslot
                                        rigslot += 1
                                End Select
                                hardware = fitXML.CreateElement("hardware")
                                hardwareAtt = fitXML.CreateAttribute("slot")
                                hardwareAtt.Value = slotGroup & slotNo.ToString
                                hardware.Attributes.Append(hardwareAtt)
                                hardwareAtt = fitXML.CreateAttribute("type")
                                hardwareAtt.Value = sMod.Name
                                hardware.Attributes.Append(hardwareAtt)
                                fitNode.AppendChild(hardware)
                                If sMod.LoadedCharge IsNot Nothing Then
                                    hardware = fitXML.CreateElement("hardware")
                                    hardwareAtt = fitXML.CreateAttribute("slot")
                                    hardwareAtt.Value = slotGroup & slotNo.ToString
                                    hardware.Attributes.Append(hardwareAtt)
                                    hardwareAtt = fitXML.CreateAttribute("type")
                                    hardwareAtt.Value = sMod.LoadedCharge.Name
                                    hardware.Attributes.Append(hardwareAtt)
                                    fitNode.AppendChild(hardware)
                                End If
                                'Call AddModule(sMod, 0, True, Nothing)
                            End If
                        End If
                    Else
                        ' Unrecognised module
                        MessageBox.Show("Ship Module is unrecognised.", "Add Ship Module Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    'currentFit.Remove(modData(0))
                End If
            End If
        Next
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        ' Check filename
        If txtFilename.Text = "" Then
            MessageBox.Show("Filename cannot be blank. Please try again.", "Filename Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        Dim rgPattern As String = "[\\\/:\*\?""'<>|]"
        Dim objRegEx As New System.Text.RegularExpressions.Regex(rgPattern)
        If objRegEx.IsMatch(txtFilename.Text) = True Then
            MessageBox.Show("Filename contains invalid characters. Please try again.", "Filename Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        EveFile = EveFolder & "\" & txtFilename.Text & ".xml"
        If My.Computer.FileSystem.FileExists(EveFile) = True Then
            Dim reply As Integer = MessageBox.Show("The chosen filename already exists - do you want to overwrite it?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            End If
        End If
        If ExportFittingsToEveFile() = True Then
            Me.Close()
        End If
    End Sub
End Class