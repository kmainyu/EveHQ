Imports System.Reflection
Imports System.Windows.Forms

Public Class Plugin
    Implements EveHQ.Core.IEveHQPlugIn
    Dim mSetPlugInData As Object

    Public Property SetPlugInData() As Object Implements Core.IEveHQPlugIn.SetPlugInData
        Get
            Return mSetPlugInData
        End Get
        Set(ByVal value As Object)
            mSetPlugInData = value
        End Set
    End Property

    Public Function EveHQStartUp() As Boolean Implements Core.IEveHQPlugIn.EveHQStartUp
        Try
            Return Me.CheckVersion()
        Catch ex As Exception
            Windows.Forms.MessageBox.Show(ex.Message)
            Return False
        End Try
    End Function

    Public Function GetEveHQPlugInInfo() As Core.PlugIn Implements Core.IEveHQPlugIn.GetEveHQPlugInInfo
        ' Returns data to EveHQ to identify it as a plugin
        Dim EveHQPlugIn As New EveHQ.Core.PlugIn
        EveHQPlugIn.Name = "InEve Skills Uploader"
        EveHQPlugIn.Description = "Uploads character skills to the InEve website"
        EveHQPlugIn.Author = "Vessper"
        EveHQPlugIn.MainMenuText = "InEve Skills Uploader"
        EveHQPlugIn.RunAtStartup = False
        EveHQPlugIn.RunInIGB = False
        EveHQPlugIn.MenuImage = My.Resources.plugin_icon
        EveHQPlugIn.Version = My.Application.Info.Version.ToString
        Return EveHQPlugIn
    End Function

    Public Function IGBService(ByVal IGBContext As Net.HttpListenerContext) As String Implements Core.IEveHQPlugIn.IGBService
        Return ""
    End Function

    Public Function RunEveHQPlugIn() As System.Windows.Forms.Form Implements Core.IEveHQPlugIn.RunEveHQPlugIn
        Return New frmInEveUploader
    End Function

    Private Function CheckVersion() As Boolean
        Dim thisAssembly As [Assembly] = System.Reflection.Assembly.GetExecutingAssembly()

        ' Display the set of assemblies our assemblies references.
        Dim refAssemblies As AssemblyName
        For Each refAssemblies In thisAssembly.GetReferencedAssemblies()
            If refAssemblies.Name = "EveHQ.Core" Then
                If CompareVersions(refAssemblies.Version.ToString, "1.3.0.0") = True Then
                    Dim msg As String = "This plug-in requires EveHQ.Core to be version 1.3.0.0 or greater" & ControlChars.CrLf & ControlChars.CrLf
                    msg &= "Please check for any updates that are available."
                    MessageBox.Show(msg, "InEve Skills Uploader", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                Else
                    Return True
                End If
            End If
        Next
    End Function

    Private Function CompareVersions(ByVal thisVersion As String, ByVal requiredVersion As String) As Boolean
        Dim localVers() As String = thisVersion.Split(CChar("."))
        Dim remoteVers() As String = requiredVersion.Split(CChar("."))
        Dim requiresUpdate As Boolean = False
        For ver As Integer = 0 To 3
            If CInt(remoteVers(ver)) <> CInt(localVers(ver)) Then
                If CInt(remoteVers(ver)) > CInt(localVers(ver)) Then
                    requiresUpdate = True
                    Exit For
                Else
                    requiresUpdate = False
                    Exit For
                End If
            End If
        Next
        Return requiresUpdate
    End Function
End Class
