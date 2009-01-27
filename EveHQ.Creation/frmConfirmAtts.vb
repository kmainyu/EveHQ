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
Imports System.Windows.Forms

Public Class frmConfirmAtts

    Public race As String
    Public bloodline As String
    Public gender As String
    Public ancestry As String
    Public career As String
    Public spec As String
    Public skills As Collection

    Private Sub nud_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudC.ValueChanged, nudI.ValueChanged, nudM.ValueChanged, nudP.ValueChanged, nudW.ValueChanged
        Dim attTotal As Integer = CInt(nudC.Value + nudI.Value + nudM.Value + nudP.Value + nudW.Value)
        Me.lblAttTotal.Text = CStr(attTotal)
        If attTotal = 39 Then
            btnAccept.Enabled = True
        Else
            btnAccept.Enabled = False
        End If
    End Sub

    Private Sub frmConfirmAtts_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Generate an ID based on date and time
        Dim CharID As String = Format(Now, "MddHHmmss")
        lblCharID.Text = CharID
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click

        ' Create a new pilot
        Dim nPilot As New EveHQ.Core.Pilot
        nPilot.ID = Me.lblCharID.Text
        ' Check name isn't blank
        If Me.txtCharName.Text.Trim = "" Then
            nPilot.Name = nPilot.ID
        Else
            nPilot.Name = Me.txtCharName.Text
        End If
        ' Check if name exists
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(nPilot.Name) = True Then
            MessageBox.Show("Pilot name already exists. Please choose an alternative name.", "Pilot Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        nPilot.Race = race
        nPilot.Blood = bloodline
        nPilot.CAtt = CInt(Me.nudC.Value)
        nPilot.IAtt = CInt(Me.nudI.Value)
        nPilot.MAtt = CInt(Me.nudM.Value)
        nPilot.PAtt = CInt(Me.nudP.Value)
        nPilot.WAtt = CInt(Me.nudW.Value)
        nPilot.Isk = 0
        nPilot.Active = True
        nPilot.Corp = "EveHQ Import Corp"
        nPilot.CorpID = "1000000"
        nPilot.Gender = gender
        nPilot.CloneName = "Clone Grade Alpha"
        nPilot.CloneSP = "900000"
        For Each skillItem As ListViewItem In skills
            Dim pilotSkill As New EveHQ.Core.Skills
            pilotSkill.ID = skillItem.Name
            pilotSkill.Name = skillItem.Text
            pilotSkill.Level = CInt(skillItem.SubItems(1).Text)
            pilotSkill.SP = CInt(skillItem.SubItems(2).Text)
            nPilot.PilotSkills.Add(pilotSkill, pilotSkill.Name)
        Next
        nPilot.Updated = True

        ' Write the XML files
        Dim xmlFile As String = EveHQ.Core.HQ.cacheFolder & "\c" & nPilot.ID & ".xml"
        Dim txmlFile As String = EveHQ.Core.HQ.cacheFolder & "\t" & nPilot.ID & ".xml"
        Dim strXML As String = ""
        Dim sw As IO.StreamWriter

        ' Write Character XML
        strXML = ""
        strXML &= EveHQ.Core.Reports.CurrentPilotXML_New(nPilot)
        sw = New IO.StreamWriter(xmlFile)
        sw.Write(strXML)
        sw.Flush()
        sw.Close()

        ' Write Training XML
        strXML = ""
        strXML &= EveHQ.Core.Reports.CurrentTrainingXML_New(nPilot)
        sw = New IO.StreamWriter(txmlFile)
        sw.Write(strXML)
        sw.Flush()
        sw.Close()

        ' Import the data!
        Call EveHQ.Core.PilotParseFunctions.ImportPilotFromXML(xmlFile, txmlFile)

        ' Refresh the list of pilots in evehq
        EveHQ.Core.PilotParseFunctions.StartPilotRefresh = True

        ' Close the form
        Me.Close()
    End Sub
End Class