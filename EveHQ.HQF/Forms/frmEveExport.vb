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

Imports System.Windows.Forms
Imports System.Xml
Imports System.IO

Public Class frmEveExport

    Dim EveFolder As String = Path.Combine(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "Eve"), "fittings")
    Dim EveFile As String = ""
	Dim cFittingList As New ArrayList
	Dim cUpdateRequired As Boolean = False

    Public Property FittingList() As ArrayList
        Get
            Return cFittingList
        End Get
        Set(ByVal value As ArrayList)
            cFittingList = value
            UpdateFittingList()
        End Set
	End Property

	Public Property UpdateRequired As Boolean
		Get
			Return cUpdateRequired
		End Get
		Set(ByVal value As Boolean)
			cUpdateRequired = value
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
        Dim currentFit As Fitting
        Dim dec As XmlDeclaration = fitXML.CreateXmlDeclaration("1.0", Nothing, Nothing)
        Dim fitAtt As XmlAttribute

        fitXML.AppendChild(dec)
        Dim fitRoot As XmlElement = fitXML.CreateElement("fittings")
        fitXML.AppendChild(fitRoot)

        If FittingList.Count > 0 Then
            For Each shipFit As String In cFittingList
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

				Call ExportFittingList(currentFit, fitXML, fit, cUpdateRequired)

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
                'MessageBox.Show("Export of HQF Fittings to " & EveFile & " completed", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return True
            Catch e As Exception
                MessageBox.Show("Unable to export HQF fittings to Eve folder: " & e.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End Try
        End If
    End Function

	Private Sub ExportFittingList(ByVal ExpFitting As Fitting, ByRef fitXML As XmlDocument, ByRef fitNode As XmlNode, ByVal UpdateRequired As Boolean)
		Dim hardware As XmlNode
		Dim hardwareAtt As XmlAttribute
		Dim slotGroup As String = ""

		' Update the base ship fitting from the actual fit
        If UpdateRequired = True Then
            ExpFitting.BaseShip = CType(ShipLists.shipList(ExpFitting.ShipName), Ship).Clone
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

		For Each DBI As DroneBayItem In ExpFitting.BaseShip.DroneBayItems.Values
			' Add the XML data
			hardware = fitXML.CreateElement("hardware")
			hardwareAtt = fitXML.CreateAttribute("qty") : hardwareAtt.Value = DBI.Quantity.ToString
			hardware.Attributes.Append(hardwareAtt)
			hardwareAtt = fitXML.CreateAttribute("slot") : hardwareAtt.Value = "drone bay"
			hardware.Attributes.Append(hardwareAtt)
			hardwareAtt = fitXML.CreateAttribute("type") : hardwareAtt.Value = DBI.DroneType.Name
			hardware.Attributes.Append(hardwareAtt)
			fitNode.AppendChild(hardware)
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
        EveFile = Path.Combine(EveFolder, txtFilename.Text & ".xml")
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