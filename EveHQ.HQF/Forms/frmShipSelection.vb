Imports DevComponents.AdvTree
Imports System.Reflection
Imports System.ComponentModel
Imports System.Windows.Forms
Imports EveHQ.HQF.Forms

Public Class frmShipSelection

#Region "Form Loading Methods"

    Private Sub frmShipSelection_Load(sender As System.Object, e As EventArgs) Handles MyBase.Load
        DisplayTagCloud()
    End Sub

#End Region

#Region "Tag Search Methods"

    Private Sub DisplayTagCloud()
        Dim tagList As New SortedList(Of String, Integer) ' tagName, occurences
        For Each fit As Fitting In Fittings.FittingList.Values
            For Each fittingTag As String In fit.Tags
                If tagList.ContainsKey(fittingTag) Then
                    tagList(fittingTag) += 1
                Else
                    tagList.Add(fittingTag, 1)
                End If
            Next
        Next

        ' Sort the tag list of order of descending occurences
        Dim sortedTagList As Dictionary(Of String, Integer) = tagList.OrderByDescending(Function(x) x.Value).ToDictionary(Function(x) x.Key, Function(y) y.Value)

        ' Calculate some weighting
        Const maxWeight As Integer = 12
        Dim max As Integer = sortedTagList.Values(0)
        Dim min As Integer = sortedTagList.Values(Math.Min(20, sortedTagList.Count) - 1)
        Dim weight As Double = ((max - min) / maxWeight)
        
        lblTagCloud.Text = ""
        For t As Integer = 0 To Math.Min(20, tagList.Count) - 1
            Dim s As Integer
            If weight <> 0 Then
                s = CInt((tagList.Values(t) - min) / weight)
            End If
            lblTagCloud.Text &= "<span><font size='+" & CStr(s) & "'><a href='" & tagList.Keys(t) & "'>" & tagList.Keys(t) & "</a></font></span>  "
        Next

    End Sub

    Private Sub txtTagSearch_TextChanged(sender As System.Object, e As EventArgs) Handles txtTagSearch.TextChanged
        Dim draftTags As List(Of String) = txtTagSearch.Text.Split(" ".ToCharArray).ToList
        Dim searchTags As New List(Of String)
        For Each t As String In draftTags
            If String.IsNullOrWhiteSpace(t.Trim) = False Then
                searchTags.Add(t.Trim)
            End If
        Next
        Dim results As New List(Of Fitting)
        For Each fit As Fitting In Fittings.FittingList.Values
            Dim match As Boolean = False
            For Each searchTag As String In searchTags
                If fit.Tags.Contains(searchTag) Then
                    match = True
                Else
                    match = False
                    Exit For
                End If
            Next
            If match = True Then
                results.Add(fit)
            End If
        Next
        adtTagResults.BeginUpdate()
        adtTagResults.Nodes.Clear()
        For Each result As Fitting In results
            Dim newNode As New Node(result.ShipName)
            newNode.Cells.Add(New Cell(result.FittingName))
            newNode.Cells.Add(New Cell(CStr(result.Rating)))
            Dim rc As New DevComponents.DotNetBar.RatingItem
            rc.NumberOfStars = 10
            rc.Rating = result.Rating
            rc.IsEditable = False
            newNode.Cells(2).HostedItem = rc
            adtTagResults.Nodes.Add(newNode)
        Next
        adtTagResults.EndUpdate()
    End Sub
    
    Private Sub lblTagCloud_MarkupLinkClick(sender As Object, e As DevComponents.DotNetBar.MarkupLinkClickEventArgs) Handles lblTagCloud.MarkupLinkClick
        ' Detect pressing Shift to add tags
        Dim shift As Boolean = CInt(ModifierKeys And Keys.Shift) <> 0
        If shift = True Then
            txtTagSearch.Text = txtTagSearch.Text.Trim & " " & e.HRef
        Else
            txtTagSearch.Text = e.HRef
        End If
    End Sub

    Private Sub adtTagResults_NodeDoubleClick(sender As Object, e As TreeNodeMouseEventArgs) Handles adtTagResults.NodeDoubleClick
        Dim fitKey As String = adtTagResults.SelectedNode.Cells(0).Text & ", " & adtTagResults.SelectedNode.Cells(1).Text
        HQFEvents.StartOpenFitting = fitKey
    End Sub

#End Region

#Region "Property Search Methods"

    Private Sub btnAddCriteria_Click(sender As System.Object, e As EventArgs) Handles btnAddCriteria.Click
        Dim node As New Node
        adtCriteria.Nodes.Add(node)
        node.CreateCells()
        Dim logic As New DevComponents.DotNetBar.Controls.ComboBoxEx
        If node.Index > 0 Then
            logic.Items.Add("And")
            logic.Items.Add("Or")
        Else
            logic.Items.Add("Where")
        End If
        logic.SelectedIndex = 0
        node.Cells(0).HostedControl = logic
        Dim c As New DevComponents.DotNetBar.Controls.ComboBoxEx
        Dim s As New Ship
        Dim info() As PropertyInfo = s.GetType().GetProperties()
        For Each p As PropertyInfo In info
            If p.CanWrite And IsBrowsable(s, p.Name) Then
                c.Items.Add(p.Name)
            End If
        Next
        c.Sorted = True
        c.SelectedIndex = 0
        node.Cells(1).HostedControl = c
        Dim condition As New DevComponents.DotNetBar.Controls.ComboBoxEx
        condition.Items.Add("Is Equal To (=)")
        condition.Items.Add("Is Not Equal To (<>)")
        condition.Items.Add("Is Greater Than (>)")
        condition.Items.Add("Is Less Than (<)")
        condition.Items.Add("Is Greater Than Or Equal To (>=)")
        condition.Items.Add("Is Less Than Or Equal To (<=)")
        condition.SelectedIndex = 0
        node.Cells(2).HostedControl = condition
        Dim value As New DevComponents.Editors.DoubleInput
        value.Value = 0
        node.Cells(3).HostedControl = value
    End Sub

    Private Sub btnRemoveCriteria_Click(sender As System.Object, e As EventArgs) Handles btnRemoveCriteria.Click
        adtCriteria.Nodes.Remove(adtCriteria.SelectedNode)
        UpdateShips()
    End Sub

    Private Sub btnUpdateShips_Click(sender As System.Object, e As EventArgs) Handles btnUpdateShips.Click
        UpdateShips()
    End Sub

    Private Sub UpdateShips()
        Dim matches As New List(Of Ship)
        Dim sample As New List(Of Ship)
        matches.AddRange(ShipLists.ShipList.Values)
        For Each n As Node In adtCriteria.Nodes
            Dim logic As Integer = CType(n.Cells(0).HostedControl, DevComponents.DotNetBar.Controls.ComboBoxEx).SelectedIndex
            Dim propertyName As String = CType(n.Cells(1).HostedControl, DevComponents.DotNetBar.Controls.ComboBoxEx).SelectedItem.ToString
            Dim criteriaIndex As Integer = CType(n.Cells(2).HostedControl, DevComponents.DotNetBar.Controls.ComboBoxEx).SelectedIndex
            Dim propertyValue As Double = CType(n.Cells(3).HostedControl, DevComponents.Editors.DoubleInput).Value
            sample.Clear()
            If logic = 0 Then
                sample.AddRange(matches)
                matches.Clear()
            Else
                sample.AddRange(ShipLists.ShipList.Values)
            End If
            For Each s As Ship In sample
                Select Case criteriaIndex
                    Case 0
                        If CDbl(GetPropertyValue(s, propertyName)) = propertyValue Then
                            matches.Add(s)
                        End If
                    Case 1
                        If CDbl(GetPropertyValue(s, propertyName)) <> propertyValue Then
                            matches.Add(s)
                        End If
                    Case 2
                        If CDbl(GetPropertyValue(s, propertyName)) > propertyValue Then
                            matches.Add(s)
                        End If
                    Case 3
                        If CDbl(GetPropertyValue(s, propertyName)) < propertyValue Then
                            matches.Add(s)
                        End If
                    Case 4
                        If CDbl(GetPropertyValue(s, propertyName)) >= propertyValue Then
                            matches.Add(s)
                        End If
                    Case 5
                        If CDbl(GetPropertyValue(s, propertyName)) <= propertyValue Then
                            matches.Add(s)
                        End If

                End Select
            Next
        Next
        adtPropertyShips.BeginUpdate()
        adtPropertyShips.Nodes.Clear()
        adtPropertyShips.Columns.Clear()
        ' Add the ship name column
        Dim ch As New DevComponents.AdvTree.ColumnHeader
        ch.Name = "Name"
        ch.Text = "Ship Name"
        ch.Width.Absolute = 150
        adtPropertyShips.Columns.Add(ch)
        For Each c As Node In adtCriteria.Nodes
            Dim p As String = CType(c.Cells(1).HostedControl, DevComponents.DotNetBar.Controls.ComboBoxEx).SelectedItem.ToString
            Dim col As New DevComponents.AdvTree.ColumnHeader
            col.Name = p
            col.Text = p
            col.Width.Absolute = 100
            adtPropertyShips.Columns.Add(col)
        Next
        For Each match As Ship In matches
            Dim n As New Node(match.Name)
            n.ContextMenu = ctxShips
            adtPropertyShips.Nodes.Add(n)
            For Each c As Node In adtCriteria.Nodes
                n.Cells.Add(New Cell(CStr(GetPropertyValue(match, CType(c.Cells(1).HostedControl, DevComponents.DotNetBar.Controls.ComboBoxEx).SelectedItem.ToString))))
            Next
        Next
        adtPropertyShips.EndUpdate()
    End Sub

    Public Function GetPropertyValue(ByVal obj As Object, ByVal propertyName As String) As Object
        Dim objType As Type = obj.GetType()
        Dim pInfo As PropertyInfo = objType.GetProperty(propertyName)
        Dim propValue As Object = pInfo.GetValue(obj, BindingFlags.GetProperty, Nothing, Nothing, Nothing)
        Return propValue
    End Function

    Public Function IsBrowsable(ByVal obj As Object, propertyName As String) As Boolean
        ' Gets the attributes for the property. 
        Dim pd As PropertyDescriptor = TypeDescriptor.GetProperties(obj.GetType)(propertyName)
        If pd IsNot Nothing Then
            Dim attributes As AttributeCollection = pd.Attributes
            ' Checks to see if the property is browsable. 
            Dim myAttribute As BrowsableAttribute = CType(attributes(GetType(BrowsableAttribute)), BrowsableAttribute)
            If myAttribute.Browsable Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Private Sub adtCriteria_SelectionChanged(sender As Object, e As EventArgs) Handles adtCriteria.SelectionChanged
        If adtCriteria.SelectedNodes.Count > 0 Then
            btnRemoveCriteria.Enabled = True
        Else
            btnRemoveCriteria.Enabled = False
        End If
    End Sub

    Private Sub mnuCreateNewFitting_Click(sender As System.Object, e As EventArgs) Handles mnuCreateNewFitting.Click
        Dim shipName As String = adtPropertyShips.SelectedNode.Cells(0).Text
        HQFEvents.StartCreateFitting = shipName
    End Sub

    Private Sub mnuPreviewShip_Click(sender As System.Object, e As EventArgs) Handles mnuPreviewShip.Click
        Dim shipName As String = adtPropertyShips.SelectedNode.Text
        Dim selShip As Ship = ShipLists.ShipList(shipName)
        Dim showInfo As New FrmShowInfo
        Dim hPilot As Core.EveHQPilot
        If Settings.HQFSettings.DefaultPilot <> "" Then
            hPilot = Core.HQ.Settings.Pilots(Settings.HQFSettings.DefaultPilot)
        Else
            hPilot = Core.HQ.Settings.Pilots.Values(0)
        End If
        showInfo.ShowItemDetails(selShip, hPilot)
    End Sub

#End Region
  
End Class