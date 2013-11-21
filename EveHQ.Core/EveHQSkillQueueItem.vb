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
