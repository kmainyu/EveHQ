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

<Serializable()> Public Class EveHQSkillQueue
    Implements ICloneable

#Region "Properties"

    ''' <summary>
    ''' Specifies the name of the skill queue
    ''' </summary>
    ''' <value>The name of the skill queue</value>
    ''' <remarks></remarks>
    Public Property Name() As String

    ''' <summary>
    ''' A boolean value indicating whether the skill queue should contain the current training skill as the first item
    ''' </summary>
    ''' <value>Whether the skill queue should contain the current training skill as the first item</value>
    ''' <remarks></remarks>
    Public Property IncCurrentTraining() As Boolean

    ''' <summary>
    ''' Contains a collection of EveHQ.Core.SkillQueueItem objects
    ''' </summary>
    ''' <value>A collection of EveHQ.Core.SkillQueueItem objects</value>
    ''' <remarks></remarks>
    Public Property Queue() As New Dictionary(Of String, EveHQSkillQueueItem)

    ''' <summary>
    ''' A boolean value indicating whether the current skill queue in the main (or primary) queue for a pilot
    ''' Only one skill queue in the pilot's collection should be Primary
    ''' </summary>
    ''' <value>Boolean value indicating whether this in the primary skill queue for a pilot</value>
    ''' <remarks></remarks>
    Public Property Primary() As Boolean

    ''' <summary>
    ''' Specifies the length of time (in seconds) of the skill queue after being processed
    ''' </summary>
    ''' <value>The length of time (in seconds) of the skill queue after being processed</value>
    ''' <remarks></remarks>
    Public Property QueueTime() As Long

    ''' <summary>
    ''' Specifies the number of unique entries in the skill queue
    ''' </summary>
    ''' <value>Specifies the number of unique entries in the skill queue</value>
    ''' <remarks></remarks>
    Public Property QueueSkills() As Integer

    ''' <summary>
    ''' A boolean value indicating whether the current skill queue should show completed skills
    ''' </summary>
    ''' <value>Boolean value indicating whether the skill queue shows completed skills</value>
    ''' <remarks></remarks>
    Public Property ShowCompletedSkills() As Boolean

#End Region

    ''' <summary>
    ''' Routine for cloning an entire skill queue
    ''' </summary>
    ''' <returns>A copy of the instance of EveHQ.Core.SkillQueue from where the function was called</returns>
    ''' <remarks></remarks>
    Public Function Clone() As Object Implements ICloneable.Clone
        Dim newQueue As EveHQSkillQueue = CType(MemberwiseClone(), EveHQSkillQueue)
        Dim newQ As New Dictionary(Of String, EveHQSkillQueueItem)
        For Each qItem As EveHQSkillQueueItem In Queue.Values
            Dim nItem As New EveHQSkillQueueItem
            nItem.ToLevel = qItem.ToLevel
            nItem.FromLevel = qItem.FromLevel
            nItem.Name = qItem.Name
            nItem.Pos = qItem.Pos
            nItem.Notes = qItem.Notes
            nItem.Priority = qItem.Priority
            newQ.Add(nItem.Key, nItem)
        Next
        newQueue.Queue = newQ
        Return newQueue
    End Function
End Class
