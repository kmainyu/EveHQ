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
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Xml

Namespace Forms

    Public Class FrmEveExport

        ReadOnly _eveFolder As String = Path.Combine(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eve"), "fittings")
        Dim _eveFile As String = ""
        Dim _fittingList As New ArrayList
        Dim _updateRequired As Boolean = False

        Public Property FittingList() As ArrayList
            Get
                Return _fittingList
            End Get
            Set(ByVal value As ArrayList)
                _fittingList = value
                UpdateFittingList()
            End Set
        End Property

        Public Property UpdateRequired As Boolean
            Get
                Return _updateRequired
            End Get
            Set(ByVal value As Boolean)
                _updateRequired = value
            End Set
        End Property

        Private Sub UpdateFittingList()
            lvwFittings.BeginUpdate()
            lvwFittings.Items.Clear()
            For Each shipFit As String In _fittingList
                lvwFittings.Items.Add(shipFit)
            Next
            lvwFittings.EndUpdate()
        End Sub

        Private Sub frmEveExport_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            lblEveFolder.Text = _eveFolder
        End Sub

        Private Function ExportFittingsToEveFile() As Boolean
            Dim fitXML As New XmlDocument
            Dim currentFit As Fitting
            Dim dec As XmlDeclaration = fitXML.CreateXmlDeclaration("1.0", Nothing, Nothing)
            Dim fitAtt As XmlAttribute

            fitXML.AppendChild(dec)
            Dim fitRoot As XmlElement = fitXML.CreateElement("fittings")
            fitXML.AppendChild(fitRoot)

            If FittingList.Count > 0 Then
                For Each shipFit As String In _fittingList
                    If Fittings.FittingList.ContainsKey(shipFit) Then
                        'Bug 104: There are cases of where the string being used to export a fit is not found in the actual collection of fits.
                        ' Added the check and a comment to alert the user to the error without throwing an exception. 
                        currentFit = Fittings.FittingList.Item(shipFit).Clone

                        Dim fit As XmlNode = fitXML.CreateElement("fitting")
                        fitAtt = fitXML.CreateAttribute("name")
                        fitAtt.Value = currentFit.FittingName
                        fit.Attributes.Append(fitAtt)
                        fitRoot.AppendChild(fit)

                        Dim desc As XmlNode = fitXML.CreateElement("description")
                        fitAtt = fitXML.CreateAttribute("value")
                        fitAtt.Value = currentFit.FittingName
                        desc.Attributes.Append(fitAtt)
                        fit.AppendChild(desc)

                        Dim shiptype As XmlNode = fitXML.CreateElement("shipType")
                        fitAtt = fitXML.CreateAttribute("value")
                        fitAtt.Value = currentFit.ShipName
                        shiptype.Attributes.Append(fitAtt)
                        fit.AppendChild(shiptype)

                        Call ExportFittingList(currentFit, fitXML, fit, _updateRequired)
                    Else
                        MessageBox.Show(String.Format("There was a mismatch of expected data. The fit requested to be exported ({0}), was not found in the collection of saved fittings.", shipFit))
                    End If

                Next

                ' Check for the fittings directory and create it
                Try
                    If My.Computer.FileSystem.DirectoryExists(_eveFolder) = False Then
                        Dim reply As Integer = MessageBox.Show("The Eve fittings folder is not present on your system and is required for the saving of fittings. Would you like to create this folder now?", "Create Fittings Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = DialogResult.No Then
                            MessageBox.Show("Unable to export HQF fittings due to missing folder.", "Export Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return False
                        Else
                            My.Computer.FileSystem.CreateDirectory(_eveFolder)
                        End If
                    End If
                    fitXML.Save(_eveFile)
                    'MessageBox.Show("Export of HQF Fittings to " & EveFile & " completed", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return True
                Catch e As Exception
                    MessageBox.Show("Unable to export HQF fittings to Eve folder: " & e.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End Try
            End If
        End Function

        Private Sub ExportFittingList(ByVal expFitting As Fitting, ByRef fitXML As XmlDocument, ByRef fitNode As XmlNode, ByVal isUpdateRequired As Boolean)
            Dim hardware As XmlNode
            Dim hardwareAtt As XmlAttribute
            Dim slotGroup As String

            ' Update the base ship fitting from the actual fit
            If isUpdateRequired = True Then
                expFitting.BaseShip = ShipLists.ShipList(expFitting.ShipName).Clone
                ExpFitting.UpdateBaseShipFromFitting()
            End If

            For slotNo As Integer = 1 To ExpFitting.BaseShip.SubSlots
                If ExpFitting.BaseShip.SubSlot(slotNo) IsNot Nothing Then
                    slotGroup = "subsystem slot "
                    hardware = fitXML.CreateElement("hardware")
                    hardwareAtt = fitXML.CreateAttribute("slot")
                    hardwareAtt.Value = slotGroup & (slotNo - 1).ToString
                    hardware.Attributes.Append(hardwareAtt)
                    hardwareAtt = fitXML.CreateAttribute("type")
                    hardwareAtt.Value = ExpFitting.BaseShip.SubSlot(slotNo).Name
                    hardware.Attributes.Append(hardwareAtt)
                    fitNode.AppendChild(hardware)
                End If
            Next

            For slotNo As Integer = 1 To ExpFitting.BaseShip.RigSlots
                If ExpFitting.BaseShip.RigSlot(slotNo) IsNot Nothing Then
                    slotGroup = "rig slot "
                    hardware = fitXML.CreateElement("hardware")
                    hardwareAtt = fitXML.CreateAttribute("slot")
                    hardwareAtt.Value = slotGroup & (slotNo - 1).ToString
                    hardware.Attributes.Append(hardwareAtt)
                    hardwareAtt = fitXML.CreateAttribute("type")
                    hardwareAtt.Value = ExpFitting.BaseShip.RigSlot(slotNo).Name
                    hardware.Attributes.Append(hardwareAtt)
                    fitNode.AppendChild(hardware)
                End If
            Next

            For slotNo As Integer = 1 To ExpFitting.BaseShip.LowSlots
                If ExpFitting.BaseShip.LowSlot(slotNo) IsNot Nothing Then
                    slotGroup = "low slot "
                    hardware = fitXML.CreateElement("hardware")
                    hardwareAtt = fitXML.CreateAttribute("slot")
                    hardwareAtt.Value = slotGroup & (slotNo - 1).ToString
                    hardware.Attributes.Append(hardwareAtt)
                    hardwareAtt = fitXML.CreateAttribute("type")
                    hardwareAtt.Value = ExpFitting.BaseShip.LowSlot(slotNo).Name
                    hardware.Attributes.Append(hardwareAtt)
                    fitNode.AppendChild(hardware)
                End If
            Next

            For slotNo As Integer = 1 To ExpFitting.BaseShip.MidSlots
                If ExpFitting.BaseShip.MidSlot(slotNo) IsNot Nothing Then
                    slotGroup = "med slot "
                    hardware = fitXML.CreateElement("hardware")
                    hardwareAtt = fitXML.CreateAttribute("slot")
                    hardwareAtt.Value = slotGroup & (slotNo - 1).ToString
                    hardware.Attributes.Append(hardwareAtt)
                    hardwareAtt = fitXML.CreateAttribute("type")
                    hardwareAtt.Value = ExpFitting.BaseShip.MidSlot(slotNo).Name
                    hardware.Attributes.Append(hardwareAtt)
                    fitNode.AppendChild(hardware)
                End If
            Next

            For slotNo As Integer = 1 To ExpFitting.BaseShip.HiSlots
                If ExpFitting.BaseShip.HiSlot(slotNo) IsNot Nothing Then
                    slotGroup = "hi slot "
                    hardware = fitXML.CreateElement("hardware")
                    hardwareAtt = fitXML.CreateAttribute("slot")
                    hardwareAtt.Value = slotGroup & (slotNo - 1).ToString
                    hardware.Attributes.Append(hardwareAtt)
                    hardwareAtt = fitXML.CreateAttribute("type")
                    hardwareAtt.Value = ExpFitting.BaseShip.HiSlot(slotNo).Name
                    hardware.Attributes.Append(hardwareAtt)
                    fitNode.AppendChild(hardware)
                End If
            Next

            For Each dbi As DroneBayItem In expFitting.BaseShip.DroneBayItems.Values
                ' Add the XML data
                hardware = fitXML.CreateElement("hardware")
                hardwareAtt = fitXML.CreateAttribute("qty") : hardwareAtt.Value = dbi.Quantity.ToString
                hardware.Attributes.Append(hardwareAtt)
                hardwareAtt = fitXML.CreateAttribute("slot") : hardwareAtt.Value = "drone bay"
                hardware.Attributes.Append(hardwareAtt)
                hardwareAtt = fitXML.CreateAttribute("type") : hardwareAtt.Value = dbi.DroneType.Name
                hardware.Attributes.Append(hardwareAtt)
                fitNode.AppendChild(hardware)
            Next

            For Each cbi As CargoBayItem In expFitting.BaseShip.CargoBayItems.Values
                ' Add the XML data
                hardware = fitXML.CreateElement("hardware")
                hardwareAtt = fitXML.CreateAttribute("qty") : hardwareAtt.Value = cbi.Quantity.ToString
                hardware.Attributes.Append(hardwareAtt)
                hardwareAtt = fitXML.CreateAttribute("slot") : hardwareAtt.Value = "cargo"
                hardware.Attributes.Append(hardwareAtt)
                hardwareAtt = fitXML.CreateAttribute("type") : hardwareAtt.Value = cbi.ItemType.Name
                hardware.Attributes.Append(hardwareAtt)
                fitNode.AppendChild(hardware)
            Next
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
            ' Check filename
            If txtFilename.Text = "" Then
                MessageBox.Show("Filename cannot be blank. Please try again.", "Filename Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            Const rgPattern As String = "[\\\/:\*\?""'<>|]"
            Dim objRegEx As New Regex(rgPattern)
            If objRegEx.IsMatch(txtFilename.Text) = True Then
                MessageBox.Show("Filename contains invalid characters. Please try again.", "Filename Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            _eveFile = Path.Combine(_eveFolder, txtFilename.Text & ".xml")
            If My.Computer.FileSystem.FileExists(_eveFile) = True Then
                Dim reply As Integer = MessageBox.Show("The chosen filename already exists - do you want to overwrite it?", "Confirm Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.No Then
                    Exit Sub
                End If
            End If
            If ExportFittingsToEveFile() = True Then
                Close()
            End If
        End Sub
    End Class
End NameSpace