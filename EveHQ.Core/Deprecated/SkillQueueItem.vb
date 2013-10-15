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

' ReSharper disable InconsistentNaming
' ReSharper disable once CheckNamespace
<Serializable()> Public Class SkillQueueItem
    Implements ICloneable

#Region "Property Variables"
    Dim mKey As String
    Dim mName As String
    Dim mFromLevel As Integer
    Dim mToLevel As Integer
    Dim mPos As Integer
    Dim mPriority As Integer
    Dim mNotes As String
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
    Public Property Key() As String
        Get
            Return mKey
        End Get
        Set(ByVal value As String)
            mKey = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the skill
    ''' </summary>
    ''' <value>The name of the skill for the skill queue item</value>
    ''' <remarks></remarks>
    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    ''' <summary>
    ''' An integer representing the starting level of the training queue item
    ''' </summary>
    ''' <value>The level from which the skill queue item is being trained</value>
    ''' <remarks></remarks>
    Public Property FromLevel() As Integer
        Get
            Return mFromLevel
        End Get
        Set(ByVal value As Integer)
            mFromLevel = value
        End Set
    End Property

    ''' <summary>
    ''' An integer representing the end level of the training queue item
    ''' </summary>
    ''' <value>The level to which the skill queue item is being trained to</value>
    ''' <remarks></remarks>
    Public Property ToLevel() As Integer
        Get
            Return mToLevel
        End Get
        Set(ByVal value As Integer)
            mToLevel = value
        End Set
    End Property

    ''' <summary>
    ''' An integer containing the skill queue item's position within the skill queue
    ''' This should be unique but controlled from other various skill queue functions
    ''' </summary>
    ''' <value>The position of the skill queue item in the queue</value>
    ''' <remarks></remarks>
    Public Property Pos() As Integer
        Get
            Return mPos
        End Get
        Set(ByVal value As Integer)
            mPos = value
        End Set
    End Property

    ''' <summary>
    ''' An integer containing the relative priority of a skill queue item
    ''' *** CURRENTLY UNUSED ***
    ''' </summary>
    ''' <value>The priority level of the skill queue item</value>
    ''' <returns>A value between 0 and 9</returns>
    ''' <remarks></remarks>
    Public Property Priority() As Integer
        Get
            Return mPriority
        End Get
        Set(ByVal value As Integer)
            mPriority = value
        End Set
    End Property

    ''' <summary>
    ''' A string containing user-defined notes for the skill queue item
    ''' </summary>
    ''' <value>The user-defined notes of a skill queue item</value>
    ''' <remarks></remarks>
    Public Property Notes() As String
        Get
            Return mNotes
        End Get
        Set(ByVal value As String)
            mNotes = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Routine for cloning a skill queue item
    ''' </summary>
    ''' <returns>A copy of the instance of EveHQ.Core.SkillQueueItem from where the function was called</returns>
    ''' <remarks></remarks>
    Public Function Clone() As Object Implements ICloneable.Clone
        Dim clonedQueueItem As SkillQueueItem = CType(MemberwiseClone(), SkillQueueItem)
        Return clonedQueueItem
    End Function

End Class
