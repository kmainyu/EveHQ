' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Public Class DragAndDropListView
    Inherits ListView
    Public SortColumn As Integer

    ' Methods
    Public Sub New()
        Me.m_allowReorder = True
        Me.m_lineColor = Color.Red
        Me.m_displayPilotName = frmTraining.DisplayPilotName
        displayPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(m_displayPilotName), Core.Pilot)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.EnableNotifyMessage, True)
    End Sub

    Protected Overrides Sub OnNotifyMessage(ByVal m As Message)
        'Filter out the WM_ERASEBKGND message
        If (m.Msg <> 14) Then
            MyBase.OnNotifyMessage(m)
        End If
    End Sub

    Private Function GetDataForDragDrop() As DragItemData
        Dim data1 As New DragItemData(Me)
        Dim item1 As ListViewItem
        For Each item1 In MyBase.SelectedItems
            data1.DragItems.Add(item1.Clone)
        Next
        Return data1
    End Function
    Protected Overrides Sub OnDragDrop(ByVal drgevent As DragEventArgs)
        If Not Me.m_allowReorder Then
            MyBase.OnDragDrop(drgevent)
        Else
            Dim pt As Point = MyBase.PointToClient(New Point(drgevent.X, drgevent.Y))
            Dim di As Integer
            If MyBase.GetItemAt(pt.X, pt.Y) Is Nothing Then
                di = MyBase.Items.Count
            Else
                di = MyBase.GetItemAt(pt.X, pt.Y).Index
            End If

            Dim testData As DragItemData = CType(drgevent.Data.GetData(GetType(DragItemData).ToString), DragItemData)
            Dim testItem As ListViewItem = CType(testData.DragItems.Item(0), ListViewItem)
            If testItem.Text.Substring(0, 2) = "##" Then
                If displayPilot.Training = False Then
                    di += 1
                End If

                If di < MyBase.Items.Count Then
                    Dim din As String = MyBase.Items(di).Text & MyBase.Items(di).SubItems(2).Text & MyBase.Items(di).SubItems(3).Text
                    Dim dIDX As Integer = 0
                    For Each moveSkill As EveHQ.Core.SkillQueueItem In displayPilot.ActiveQueue.Queue
                        dIDX += 1
                        If moveSkill.Key = din Then Exit For
                    Next
                    ' This is a skill being dragged from the treeview
                    displayPilot.ActiveQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, testItem.Text.Trim("#".ToCharArray), dIDX, displayPilot.ActiveQueue)
                Else
                    ' Set the skill's destination to the bottom of the list
                    displayPilot.ActiveQueue = EveHQ.Core.SkillQueueFunctions.AddSkillToQueue(displayPilot, testItem.Text.Trim("#".ToCharArray), displayPilot.ActiveQueue.Queue.Count + 1, displayPilot.ActiveQueue)
                End If

            Else
                ' This is a skill being moved in the queue
                If ((drgevent.Data.GetDataPresent(GetType(DragItemData).ToString) AndAlso (Not CType(drgevent.Data.GetData(GetType(DragItemData).ToString), DragItemData).ListView Is Nothing)) AndAlso (CType(drgevent.Data.GetData(GetType(DragItemData).ToString), DragItemData).DragItems.Count <> 0)) Then
                    Dim data1 As DragItemData = CType(drgevent.Data.GetData(GetType(DragItemData).ToString), DragItemData)
                    Dim newitem As ListViewItem = data1.ListView.SelectedItems(0)
                    Dim si As Integer = MyBase.Items.IndexOfKey(newitem.Name)

                    ' Adjustment required if skill is moving down the list (or up the index!)
                    If di > si Then
                        di -= 1
                    End If

                    Dim din As String = MyBase.Items(di).Text & MyBase.Items(di).SubItems(2).Text & MyBase.Items(di).SubItems(3).Text
                    Dim sin As String = MyBase.Items(si).Text & MyBase.Items(si).SubItems(2).Text & MyBase.Items(si).SubItems(3).Text

                    'Dim msg As String = ""
                    'msg &= "Source: " & sin & " (" & si & ")" & ControlChars.CrLf
                    'msg &= "Destination: " & din & " (" & di & ")" & ControlChars.CrLf
                    'MessageBox.Show(msg)

                    If displayPilot.Training = False Or m_includeCurrentTrining = False Then
                        di += 1
                        si += 1
                    Else
                        ' Exit if trying to overwrite the skill currently training
                        If di = 0 Then Exit Sub
                    End If
                    Dim mySSkill As EveHQ.Core.SkillQueueItem = New EveHQ.Core.SkillQueueItem
                    mySSkill = CType(displayPilot.ActiveQueue.Queue(sin), Core.SkillQueueItem)

                    ' Move all the items up or down depending on position
                    If si > di Then
                        ' Move an item up the queue
                        Dim moveSkill As EveHQ.Core.SkillQueueItem
                        Dim sIDX As Integer = 0
                        For Each moveSkill In displayPilot.ActiveQueue.Queue
                            sIDX += 1
                            If moveSkill.Key = sin Then Exit For
                        Next
                        Do
                            sIDX -= 1
                            CType(displayPilot.ActiveQueue.Queue(sIDX), Core.SkillQueueItem).Pos += 1
                        Loop Until CType(displayPilot.ActiveQueue.Queue(sIDX), Core.SkillQueueItem).Key = din
                        CType(displayPilot.ActiveQueue.Queue(sin), Core.SkillQueueItem).Pos = sIDX
                    Else
                        'Move an item down the queue
                        Dim moveSkill As EveHQ.Core.SkillQueueItem
                        Dim sIDX As Integer = 0
                        For Each moveSkill In displayPilot.ActiveQueue.Queue
                            sIDX += 1
                            If moveSkill.Key = sin Then Exit For
                        Next
                        Do
                            sIDX += 1
                            CType(displayPilot.ActiveQueue.Queue(sIDX), Core.SkillQueueItem).Pos -= 1
                        Loop Until CType(displayPilot.ActiveQueue.Queue(sIDX), Core.SkillQueueItem).Key = din
                        CType(displayPilot.ActiveQueue.Queue(sin), Core.SkillQueueItem).Pos = sIDX

                    End If
                End If
            End If
                If (Not Me.m_previousItem Is Nothing) Then
                    Me.m_previousItem = Nothing
                End If
                MyBase.Invalidate()
                MyBase.OnDragDrop(drgevent)
            End If
    End Sub
    Protected Overrides Sub OnDragEnter(ByVal drgevent As DragEventArgs)
        If Not Me.m_allowReorder Then
            MyBase.OnDragEnter(drgevent)
        Else
            If Not drgevent.Data.GetDataPresent(GetType(DragItemData).ToString) Then
                drgevent.Effect = DragDropEffects.None
            Else
                drgevent.Effect = DragDropEffects.Move
                MyBase.OnDragEnter(drgevent)
            End If
        End If
    End Sub
    Protected Overrides Sub OnDragLeave(ByVal e As EventArgs)
        Me.ResetOutOfRange()
        MyBase.Invalidate()
        MyBase.OnDragLeave(e)
    End Sub
    Protected Overrides Sub OnDragOver(ByVal drgevent As DragEventArgs)
        If Not Me.m_allowReorder Then
            MyBase.OnDragOver(drgevent)
        Else
            If Not drgevent.Data.GetDataPresent(GetType(DragItemData).ToString) Then
                drgevent.Effect = DragDropEffects.None
            Else
                If (MyBase.Items.Count > 0) Then
                    Dim pointArray1 As Point()
                    Dim point1 As Point = MyBase.PointToClient(New Point(drgevent.X, drgevent.Y))
                    Dim item1 As ListViewItem = MyBase.GetItemAt(point1.X, point1.Y)
                    Dim graphics1 As Graphics = MyBase.CreateGraphics
                    If (item1 Is Nothing) Then
                        drgevent.Effect = DragDropEffects.Move
                        If (Not Me.m_previousItem Is Nothing) Then
                            Me.m_previousItem = Nothing
                            MyBase.Invalidate()
                        End If
                        item1 = MyBase.Items.Item((MyBase.Items.Count - 1))
                        If ((MyBase.View = Windows.Forms.View.Details) OrElse (MyBase.View = Windows.Forms.View.List)) Then
                            graphics1.DrawLine(New Pen(Me.m_lineColor, 2.0!), New Point(item1.Bounds.X, (item1.Bounds.Y + item1.Bounds.Height)), New Point((item1.Bounds.X + MyBase.Bounds.Width), (item1.Bounds.Y + item1.Bounds.Height)))
                            pointArray1 = New Point() {New Point(item1.Bounds.X, ((item1.Bounds.Y + item1.Bounds.Height) - 5)), New Point((item1.Bounds.X + 5), (item1.Bounds.Y + item1.Bounds.Height)), New Point(item1.Bounds.X, ((item1.Bounds.Y + item1.Bounds.Height) + 5))}
                            graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                            pointArray1 = New Point() {New Point((MyBase.Bounds.Width - 4), ((item1.Bounds.Y + item1.Bounds.Height) - 5)), New Point((MyBase.Bounds.Width - 9), (item1.Bounds.Y + item1.Bounds.Height)), New Point((MyBase.Bounds.Width - 4), ((item1.Bounds.Y + item1.Bounds.Height) + 5))}
                            graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                        Else
                            graphics1.DrawLine(New Pen(Me.m_lineColor, 2.0!), New Point((item1.Bounds.X + item1.Bounds.Width), item1.Bounds.Y), New Point((item1.Bounds.X + item1.Bounds.Width), (item1.Bounds.Y + item1.Bounds.Height)))
                            pointArray1 = New Point() {New Point(((item1.Bounds.X + item1.Bounds.Width) - 5), item1.Bounds.Y), New Point(((item1.Bounds.X + item1.Bounds.Width) + 5), item1.Bounds.Y), New Point((item1.Bounds.X + item1.Bounds.Width), (item1.Bounds.Y + 5))}
                            graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                            pointArray1 = New Point() {New Point(((item1.Bounds.X + item1.Bounds.Width) - 5), (item1.Bounds.Y + item1.Bounds.Height)), New Point(((item1.Bounds.X + item1.Bounds.Width) + 5), (item1.Bounds.Y + item1.Bounds.Height)), New Point((item1.Bounds.X + item1.Bounds.Width), ((item1.Bounds.Y + item1.Bounds.Height) - 5))}
                            graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                        End If
                        MyBase.OnDragOver(drgevent)
                        Return
                    End If
                    If (((Not Me.m_previousItem Is Nothing) AndAlso (Not Me.m_previousItem Is item1)) OrElse (Me.m_previousItem Is Nothing)) Then
                        MyBase.Invalidate()
                    End If
                    Me.m_previousItem = item1
                    If ((MyBase.View = Windows.Forms.View.Details) OrElse (MyBase.View = Windows.Forms.View.List)) Then
                        graphics1.DrawLine(New Pen(Me.m_lineColor, 2.0!), New Point(item1.Bounds.X, item1.Bounds.Y), New Point((item1.Bounds.X + MyBase.Bounds.Width), item1.Bounds.Y))
                        pointArray1 = New Point() {New Point(item1.Bounds.X, (item1.Bounds.Y - 5)), New Point((item1.Bounds.X + 5), item1.Bounds.Y), New Point(item1.Bounds.X, (item1.Bounds.Y + 5))}
                        graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                        pointArray1 = New Point() {New Point((MyBase.Bounds.Width - 4), (item1.Bounds.Y - 5)), New Point((MyBase.Bounds.Width - 9), item1.Bounds.Y), New Point((MyBase.Bounds.Width - 4), (item1.Bounds.Y + 5))}
                        graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                    Else
                        graphics1.DrawLine(New Pen(Me.m_lineColor, 2.0!), New Point(item1.Bounds.X, item1.Bounds.Y), New Point(item1.Bounds.X, (item1.Bounds.Y + item1.Bounds.Height)))
                        pointArray1 = New Point() {New Point((item1.Bounds.X - 5), item1.Bounds.Y), New Point((item1.Bounds.X + 5), item1.Bounds.Y), New Point(item1.Bounds.X, (item1.Bounds.Y + 5))}
                        graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                        pointArray1 = New Point() {New Point((item1.Bounds.X - 5), (item1.Bounds.Y + item1.Bounds.Height)), New Point((item1.Bounds.X + 5), (item1.Bounds.Y + item1.Bounds.Height)), New Point(item1.Bounds.X, ((item1.Bounds.Y + item1.Bounds.Height) - 5))}
                        graphics1.FillPolygon(New SolidBrush(Me.m_lineColor), pointArray1)
                    End If
                    Dim item2 As ListViewItem
                    For Each item2 In MyBase.SelectedItems
                        If (item2.Index = item1.Index) Then
                            drgevent.Effect = DragDropEffects.None
                            item1.EnsureVisible()
                            Return
                        End If
                    Next
                    item1.EnsureVisible()
                End If
                drgevent.Effect = DragDropEffects.Move
                MyBase.OnDragOver(drgevent)
            End If
        End If
    End Sub
    Protected Overrides Sub OnItemDrag(ByVal e As ItemDragEventArgs)
        If Not Me.m_allowReorder Then
            MyBase.OnItemDrag(e)
        Else
            MyBase.DoDragDrop(Me.GetDataForDragDrop, DragDropEffects.Move)
            MyBase.OnItemDrag(e)
        End If
    End Sub
    Protected Overrides Sub OnLostFocus(ByVal e As EventArgs)
        Me.ResetOutOfRange()
        'MyBase.Invalidate()
        MyBase.OnLostFocus(e)
    End Sub
    Private Sub ResetOutOfRange()
        If (Not Me.m_previousItem Is Nothing) Then
            Me.m_previousItem = Nothing
        End If
    End Sub

    ' Properties
    Public Property AllowReorder() As Boolean
        Get
            Return Me.m_allowReorder
        End Get
        Set(ByVal value As Boolean)
            Me.m_allowReorder = value
        End Set
    End Property

    Public Property LineColor() As Color
        Get
            Return Me.m_lineColor
        End Get
        Set(ByVal value As Color)
            Me.m_lineColor = value
        End Set
    End Property

    Public Property IncludeCurrentTraining() As Boolean
        Get
            Return Me.m_includeCurrentTrining
        End Get
        Set(ByVal value As Boolean)
            Me.m_includeCurrentTrining = value
        End Set
    End Property

    ' Fields
    Private m_allowReorder As Boolean
    Private m_lineColor As Color
    Private m_includeCurrentTrining As Boolean
    Private m_previousItem As ListViewItem
    Private m_displayPilotName As String
    Private displayPilot As EveHQ.Core.Pilot

End Class

' Nested Types
Public Class DragItemData
    ' Methods
    Public Sub New(ByVal listView As DragAndDropListView)
        Me.m_listView = listView
        Me.m_dragItems = New ArrayList
    End Sub

    ' Properties
    Public ReadOnly Property DragItems() As ArrayList
        Get
            Return Me.m_dragItems
        End Get
    End Property

    Public ReadOnly Property ListView() As DragAndDropListView
        Get
            Return Me.m_listView
        End Get
    End Property

    ' Fields
    Private m_dragItems As ArrayList
    Private m_listView As DragAndDropListView
End Class
