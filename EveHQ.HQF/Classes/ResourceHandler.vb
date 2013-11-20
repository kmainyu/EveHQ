Namespace Classes

    ''' <summary>
    ''' Class to pass assembly resources outside the assembly
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ResourceHandler

        ''' <summary>
        ''' Gets a resource file and returns it as a string
        ''' </summary>
        ''' <param name="resourceName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetResource(resourceName As String) As String

            Return My.Resources.ResourceManager.GetObject(resourceName).ToString

        End Function

    End Class

End Namespace