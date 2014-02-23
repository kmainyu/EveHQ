
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
''' A class representing individual items in a EveHQ.Core.SkillQueue.Queue collection
''' The SkillQueueItem contains the externally saved data that is used to calculate the actual skill queue
''' The processed skill queue items are stored in the SortedQueueItem class
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class EveHQSkillQueueItem
    Implements ICloneable

#Region "Property Variables"
    Dim _key As String
    Dim _name As String
    Dim _fromLevel As Integer
    Dim _toLevel As Integer
#End Region

#Region "Properties"

    ''' <summary>
    ''' Represents the unique key of the skill queue item
    ''' The key is made up of 3 elements: Skill name (Name), FromLevel and ToLevel
    ''' These are concatenated to make the key i.e.
    ''' "Spaceship Command24" indicates training the "Spaceship Command" skill from level 2 to level 4
    ''' </summary>
    ''' <value>The unique key of the skill queue item</value>
    ''' <remarks></remarks>
    Public ReadOnly Property Key() As String
        Get
            Return _key
        End Get
    End Property

    ''' <summary>
    ''' The name of the skill
    ''' </summary>
    ''' <value>The name of the skill for the skill queue item</value>
    ''' <remarks></remarks>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
            ' Update the key
            _key = Name & FromLevel.ToString & ToLevel.ToString
        End Set
    End Property

    ''' <summary>
    ''' An integer representing the starting level of the training queue item
    ''' </summary>
    ''' <value>The level from which the skill queue item is being trained</value>
    ''' <remarks></remarks>
    Public Property FromLevel() As Integer
        Get
            Return _fromLevel
        End Get
        Set(ByVal value As Integer)
            _fromLevel = value
            ' Update the key
            _key = Name & FromLevel.ToString & ToLevel.ToString
        End Set
    End Property

    ''' <summary>
    ''' An integer representing the end level of the training queue item
    ''' </summary>
    ''' <value>The level to which the skill queue item is being trained to</value>
    ''' <remarks></remarks>
    Public Property ToLevel() As Integer
        Get
            Return _toLevel
        End Get
        Set(ByVal value As Integer)
            _toLevel = value
            ' Update the key
            _key = Name & FromLevel.ToString & ToLevel.ToString
        End Set
    End Property

    ''' <summary>
    ''' An integer containing the skill queue item's position within the skill queue
    ''' This should be unique but controlled from other various skill queue functions
    ''' </summary>
    ''' <value>The position of the skill queue item in the queue</value>
    ''' <remarks></remarks>
    Public Property Pos() As Integer

    ''' <summary>
    ''' An integer containing the relative priority of a skill queue item
    ''' *** CURRENTLY UNUSED ***
    ''' </summary>
    ''' <value>The priority level of the skill queue item</value>
    ''' <returns>A value between 0 and 9</returns>
    ''' <remarks></remarks>
    Public Property Priority() As Integer

    ''' <summary>
    ''' A string containing user-defined notes for the skill queue item
    ''' </summary>
    ''' <value>The user-defined notes of a skill queue item</value>
    ''' <remarks></remarks>
    Public Property Notes() As String

#End Region

    ''' <summary>
    ''' Routine for cloning a skill queue item
    ''' </summary>
    ''' <returns>A copy of the instance of SkillQueueItem from where the function was called</returns>
    ''' <remarks></remarks>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CType(MemberwiseClone(), EveHQSkillQueueItem)
    End Function

End Class
