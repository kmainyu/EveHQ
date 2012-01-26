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

''' <summary>
''' A class for storing the results of processing a skill queue
''' </summary>
''' <remarks></remarks>
Public Class SortedQueueItem

#Region "Property Variables"
    Dim mDone As Boolean
    Dim mKey As String
    Dim mID As String
    Dim mName As String
    Dim mCurLevel As Integer
    Dim mFromLevel As Integer
    Dim mToLevel As Integer
    Dim mPartTrained As Boolean
    Dim mIsInjected As Boolean
    Dim mPercent As Double
    Dim mTrainTime As Long
    Dim mTimeBeforeTrained As Long
    Dim mDateFinished As Date
    Dim mRank As Integer
    Dim mPAtt As String
    Dim mSAtt As String
    Dim mSPRate As Integer
    Dim mSPTrained As Long
    Dim mIsTraining As Boolean
    Dim mIsPrereq As Boolean
    Dim mPrereq As String
    Dim mHasPrereq As Boolean
    Dim mReqs As String
    Dim mPriority As Integer
    Dim mNotes As String
#End Region

#Region "Properties"

    ''' <summary>
    ''' Indicates when a queue item has actually been trained
    ''' </summary>
    ''' <value>Indicates when a queue item has actually been trained</value>
    ''' <remarks></remarks>
    Public Property Done() As Boolean
        Get
            Return mDone
        End Get
        Set(ByVal value As Boolean)
            mDone = value
        End Set
    End Property

    ''' <summary>
    ''' The unique key of the sorted queue item
    ''' Unless any adjustments have been made to the skill queue during processing, this will be the same as the key of the EveHQ.Core.SkillQueueItem
    ''' </summary>
    ''' <value>The unique key of the sorted queue item</value>
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
    ''' The ID of the skill
    ''' </summary>
    ''' <value>The ID of the skill</value>
    ''' <remarks></remarks>
    Public Property ID() As String
        Get
            Return mID
        End Get
        Set(ByVal value As String)
            mID = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the skill
    ''' </summary>
    ''' <value>The name of the skill</value>
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
    ''' The level of the skill that has currently been trained
    ''' </summary>
    ''' <value>The level of the skill that has currently been trained</value>
    ''' <remarks></remarks>
    Public Property CurLevel() As Integer
        Get
            Return mCurLevel
        End Get
        Set(ByVal value As Integer)
            mCurLevel = value
        End Set
    End Property

    ''' <summary>
    ''' The level from which the training queue item starts
    ''' </summary>
    ''' <value>The level from which the training queue item starts</value>
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
    ''' The level at which the training queue item ends
    ''' </summary>
    ''' <value>The level at which the training queue item ends</value>
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
    ''' Indicates whether the skill has any partial training
    ''' </summary>
    ''' <value>Indicates whether the skill has any partial training</value>
    ''' <remarks></remarks>
    Public Property PartTrained() As Boolean
        Get
            Return mPartTrained
        End Get
        Set(ByVal value As Boolean)
            mPartTrained = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates if the skill is injected but is at zero skillpoints
    ''' </summary>
    ''' <value>Indicates if the skill is injected but is at zero skillpoints</value>
    ''' <remarks></remarks>
    Public Property IsInjected() As Boolean
        Get
            Return mIsInjected
        End Get
        Set(ByVal value As Boolean)
            mIsInjected = value
        End Set
    End Property

    ''' <summary>
    ''' Shows the percentage trained of theskill queue item
    ''' Note that this could indicate the percentage trained when training multiple levels
    ''' Example: having 90% of a level 3 skill trained when the skill queue item is from 3 to 5, will indicate a value of about 3%
    ''' </summary>
    ''' <value>Shows the percentage trained of the skill queue item</value>
    ''' <remarks></remarks>
    Public Property Percent() As Double
        Get
            Return mPercent
        End Get
        Set(ByVal value As Double)
            mPercent = value
        End Set
    End Property

    ''' <summary>
    ''' Shows the length of time (in seconds) to train the skill queue item
    ''' </summary>
    ''' <value>Shows the length of time (in seconds) to train the skill queue item</value>
    ''' <remarks></remarks>
    Public Property TrainTime() As Long
        Get
            Return mTrainTime
        End Get
        Set(ByVal value As Long)
            mTrainTime = value
        End Set
    End Property

    ''' <summary>
    ''' Shows the quickest time (in seconds) before this skill (and level) becomes trained
    ''' </summary>
    ''' <value>Shows the quickest time (in seconds) before this skill (and level) becomes trained</value>
    ''' <remarks></remarks>
    Public Property TimeBeforeTrained As Long
        Get
            Return mTimeBeforeTrained
        End Get
        Set(value As Long)
            mTimeBeforeTrained = value
        End Set
    End Property

    ''' <summary>
    ''' Gives the date when the skill queue item will be completed
    ''' </summary>
    ''' <value>Gives the date when the skill queue item will be completed</value>
    ''' <remarks></remarks>
    Public Property DateFinished() As Date
        Get
            Return mDateFinished
        End Get
        Set(ByVal value As Date)
            mDateFinished = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates the rank of the skill
    ''' </summary>
    ''' <value>Indicates the rank of the skill</value>
    ''' <remarks></remarks>
    Public Property Rank() As Integer
        Get
            Return mRank
        End Get
        Set(ByVal value As Integer)
            mRank = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates the primary attribute of the skill
    ''' </summary>
    ''' <value>Indicates the primary attribute of the skill</value>
    ''' <remarks></remarks>
    Public Property PAtt() As String
        Get
            Return mPAtt
        End Get
        Set(ByVal value As String)
            mPAtt = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates the secondary attribute of the skill
    ''' </summary>
    ''' <value>Indicates the secondary attribute of the skill</value>
    ''' <remarks></remarks>
    Public Property SAtt() As String
        Get
            Return mSAtt
        End Get
        Set(ByVal value As String)
            mSAtt = value
        End Set
    End Property

    ''' <summary>
    ''' Gives the rate of training in Skillpoints/hour
    ''' </summary>
    ''' <value>Gives the rate of training in Skillpoints/hour</value>
    ''' <remarks></remarks>
    Public Property SPRate() As Integer
        Get
            Return mSPRate
        End Get
        Set(ByVal value As Integer)
            mSPRate = value
        End Set
    End Property

    ''' <summary>
    ''' Shows the number of skill points trained from the skill queue item
    ''' </summary>
    ''' <value>Shows the number of skill points trained from the skill queue item</value>
    ''' <remarks></remarks>
    Public Property SPTrained() As Long
        Get
            Return mSPTrained
        End Get
        Set(ByVal value As Long)
            mSPTrained = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the skill queue item is being trained
    ''' </summary>
    ''' <value>Indicates whether the skill queue item is being trained</value>
    ''' <remarks></remarks>
    Public Property IsTraining() As Boolean
        Get
            Return mIsTraining
        End Get
        Set(ByVal value As Boolean)
            mIsTraining = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the skill queue item is a pre-requisite for another item in the queue
    ''' </summary>
    ''' <value>Indicates whether the skill queue item is a pre-requisite for another item in the queue</value>
    ''' <remarks></remarks>
    Public Property IsPrereq() As Boolean
        Get
            Return mIsPrereq
        End Get
        Set(ByVal value As Boolean)
            mIsPrereq = value
        End Set
    End Property

    ''' <summary>
    ''' Provides a list of all the skill queue items this item is a pre-requisite for
    ''' </summary>
    ''' <value>Provides a list of all the skill queue items this item is a pre-requisite for</value>
    ''' <remarks></remarks>
    Public Property Prereq() As String
        Get
            Return mPrereq
        End Get
        Set(ByVal value As String)
            mPrereq = value
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the skill queue item has pre-requisite items in the queue
    ''' </summary>
    ''' <value> Indicates whether the skill queue item has pre-requisite items in the queue</value>
    ''' <remarks></remarks>
    Public Property HasPrereq() As Boolean
        Get
            Return mHasPrereq
        End Get
        Set(ByVal value As Boolean)
            mHasPrereq = value
        End Set
    End Property

    ''' <summary>
    ''' Provides a list of all the pre-requisite skill queue items
    ''' </summary>
    ''' <value>Provides a list of all the pre-requisite skill queue items</value>
    ''' <remarks></remarks>
    Public Property Reqs() As String
        Get
            Return mReqs
        End Get
        Set(ByVal value As String)
            mReqs = value
        End Set
    End Property

    ''' <summary>
    '''  Indicates the priority of the skill queue item
    ''' *** NOT CURRENTLY USED ***
    ''' </summary>
    ''' <value>Indicates the priority of the skill queue item</value>
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
    ''' The user-defined notes of a skill queue item
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

End Class
