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


''' <summary>
''' A class for storing the results of processing a skill queue
''' </summary>
''' <remarks></remarks>
Public Class SortedQueueItem

#Region "Properties"

    ''' <summary>
    ''' Indicates when a queue item has actually been trained
    ''' </summary>
    ''' <value>Indicates when a queue item has actually been trained</value>
    ''' <remarks></remarks>
    Public Property Done() As Boolean

    ''' <summary>
    ''' The unique key of the sorted queue item
    ''' Unless any adjustments have been made to the skill queue during processing, this will be the same as the key of the EveHQ.Core.SkillQueueItem
    ''' </summary>
    ''' <value>The unique key of the sorted queue item</value>
    ''' <remarks></remarks>
    Public Property Key() As String

    ''' <summary>
    ''' The ID of the skill
    ''' </summary>
    ''' <value>The ID of the skill</value>
    ''' <remarks></remarks>
    Public Property ID() As Integer

    ''' <summary>
    ''' The name of the skill
    ''' </summary>
    ''' <value>The name of the skill</value>
    ''' <remarks></remarks>
    Public Property Name() As String

    ''' <summary>
    ''' The level of the skill that has currently been trained
    ''' </summary>
    ''' <value>The level of the skill that has currently been trained</value>
    ''' <remarks></remarks>
    Public Property CurLevel() As Integer

    ''' <summary>
    ''' The level from which the training queue item starts
    ''' </summary>
    ''' <value>The level from which the training queue item starts</value>
    ''' <remarks></remarks>
    Public Property FromLevel() As Integer

    ''' <summary>
    ''' The level at which the training queue item ends
    ''' </summary>
    ''' <value>The level at which the training queue item ends</value>
    ''' <remarks></remarks>
    Public Property ToLevel() As Integer

    ''' <summary>
    ''' Indicates whether the skill has any partial training
    ''' </summary>
    ''' <value>Indicates whether the skill has any partial training</value>
    ''' <remarks></remarks>
    Public Property PartTrained() As Boolean

    ''' <summary>
    ''' Indicates if the skill is injected but is at zero skillpoints
    ''' </summary>
    ''' <value>Indicates if the skill is injected but is at zero skillpoints</value>
    ''' <remarks></remarks>
    Public Property IsInjected() As Boolean

    ''' <summary>
    ''' Shows the percentage trained of theskill queue item
    ''' Note that this could indicate the percentage trained when training multiple levels
    ''' Example: having 90% of a level 3 skill trained when the skill queue item is from 3 to 5, will indicate a value of about 3%
    ''' </summary>
    ''' <value>Shows the percentage trained of the skill queue item</value>
    ''' <remarks></remarks>
    Public Property Percent() As Double

    ''' <summary>
    ''' Shows the length of time (in seconds) to train the skill queue item
    ''' </summary>
    ''' <value>Shows the length of time (in seconds) to train the skill queue item</value>
    ''' <remarks></remarks>
    Public Property TrainTime() As Long

    ''' <summary>
    ''' Shows the quickest time (in seconds) before this skill (and level) becomes trained
    ''' </summary>
    ''' <value>Shows the quickest time (in seconds) before this skill (and level) becomes trained</value>
    ''' <remarks></remarks>
    Public Property TimeBeforeTrained As Long

    ''' <summary>
    ''' Gives the date when the skill queue item will be completed
    ''' </summary>
    ''' <value>Gives the date when the skill queue item will be completed</value>
    ''' <remarks></remarks>
    Public Property DateFinished() As Date

    ''' <summary>
    ''' Indicates the rank of the skill
    ''' </summary>
    ''' <value>Indicates the rank of the skill</value>
    ''' <remarks></remarks>
    Public Property Rank() As Integer

    ''' <summary>
    ''' Indicates the primary attribute of the skill
    ''' </summary>
    ''' <value>Indicates the primary attribute of the skill</value>
    ''' <remarks></remarks>
    Public Property PAtt() As String

    ''' <summary>
    ''' Indicates the secondary attribute of the skill
    ''' </summary>
    ''' <value>Indicates the secondary attribute of the skill</value>
    ''' <remarks></remarks>
    Public Property SAtt() As String

    ''' <summary>
    ''' Gives the rate of training in Skillpoints/hour
    ''' </summary>
    ''' <value>Gives the rate of training in Skillpoints/hour</value>
    ''' <remarks></remarks>
    Public Property SPRate() As Integer

    ''' <summary>
    ''' Shows the number of skill points trained from the skill queue item
    ''' </summary>
    ''' <value>Shows the number of skill points trained from the skill queue item</value>
    ''' <remarks></remarks>
    Public Property SPTrained() As Long

    ''' <summary>
    ''' Indicates whether the skill queue item is being trained
    ''' </summary>
    ''' <value>Indicates whether the skill queue item is being trained</value>
    ''' <remarks></remarks>
    Public Property IsTraining() As Boolean

    ''' <summary>
    ''' Indicates whether the skill queue item is a pre-requisite for another item in the queue
    ''' </summary>
    ''' <value>Indicates whether the skill queue item is a pre-requisite for another item in the queue</value>
    ''' <remarks></remarks>
    Public Property IsPrereq() As Boolean

    ''' <summary>
    ''' Provides a list of all the skill queue items this item is a pre-requisite for
    ''' </summary>
    ''' <value>Provides a list of all the skill queue items this item is a pre-requisite for</value>
    ''' <remarks></remarks>
    Public Property Prereq() As String

    ''' <summary>
    ''' Indicates whether the skill queue item has pre-requisite items in the queue
    ''' </summary>
    ''' <value> Indicates whether the skill queue item has pre-requisite items in the queue</value>
    ''' <remarks></remarks>
    Public Property HasPrereq() As Boolean

    ''' <summary>
    ''' Provides a list of all the pre-requisite skill queue items
    ''' </summary>
    ''' <value>Provides a list of all the pre-requisite skill queue items</value>
    ''' <remarks></remarks>
    Public Property Reqs() As String

    ''' <summary>
    '''  Indicates the priority of the skill queue item
    ''' *** NOT CURRENTLY USED ***
    ''' </summary>
    ''' <value>Indicates the priority of the skill queue item</value>
    ''' <remarks></remarks>
    Public Property Priority() As Integer

    ''' <summary>
    ''' The user-defined notes of a skill queue item
    ''' </summary>
    ''' <value>The user-defined notes of a skill queue item</value>
    ''' <remarks></remarks>
    Public Property Notes() As String

#End Region

End Class
