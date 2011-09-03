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

''' <summary>
''' Class for queueing Eve API Proxy Messages
''' </summary>
''' <remarks>The size of the queue is restricted in the constructor to prevent execess memory usage if messages aren't dequeued</remarks>
Public Class EveAPIProxyMessageQueue
    Inherits Queue

#Region "Class Variables"
    Dim cMaxSize As Integer = 0
#End Region

    ''' <summary>
    ''' Gets the maximum size of the Eve API Proxy message queue
    ''' </summary>
    ''' <value></value>
    ''' <returns>The maximum number of messages allowed in the queue</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property MaxSize() As Integer
        Get
            Return cMaxSize
        End Get
    End Property

    ''' <summary>
    ''' Adds a message to the Eve API Proxy message queue
    ''' </summary>
    ''' <param name="obj">The object to be added</param>
    ''' <remarks></remarks>
    Public Overrides Sub Enqueue(ByVal obj As Object)
        If MyBase.Count = cMaxSize Then
            MyBase.Dequeue()
        End If
        MyBase.Enqueue(obj)
    End Sub

    ''' <summary>
    ''' Initialises as new EveAPIProxyMessageQueue limited to the specified size
    ''' </summary>
    ''' <param name="Size">The maximum number of messages allowed in the queue</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Size As Integer)
        cMaxSize = Size
    End Sub

End Class
