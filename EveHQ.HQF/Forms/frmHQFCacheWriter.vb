Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmHQFCacheWriter

    Private Sub frmHQFCacheWriter_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Refresh()
        ' Delete the cache folder if it's already there
        If My.Computer.FileSystem.DirectoryExists(Settings.HQFCacheFolder) = True Then
            My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        My.Computer.FileSystem.CreateDirectory(Settings.HQFCacheFolder)
        ' Save ships
        Dim s As New FileStream(HQF.Settings.HQFCacheFolder & "\ships.bin", FileMode.Create)
        Dim f As New BinaryFormatter
        f.Serialize(s, ShipLists.shipList)
        s.Close()
        ' Save modules
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\modules.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, ModuleLists.moduleList)
        s.Close()
        ' Save implants
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\implants.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Implants.implantList)
        s.Close()
        ' Save skills
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\skills.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, SkillLists.SkillList)
        s.Close()
        ' Save attributes
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\attributes.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, Attributes.AttributeList)
        s.Close()
        ' Save NPCs
        s = New FileStream(HQF.Settings.HQFCacheFolder & "\NPCs.bin", FileMode.Create)
        f = New BinaryFormatter
        f.Serialize(s, NPCs.NPCList)
        s.Close()
        Me.Close()
    End Sub
End Class