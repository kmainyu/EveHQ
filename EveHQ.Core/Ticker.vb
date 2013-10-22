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
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports EveHQ.EveData
Imports EveHQ.Core.ItemBrowser

Public Class Ticker
    Dim WithEvents _tmrScrollTimer As New Timer
    Dim _img As Bitmap
    ReadOnly _scrollImages As New Queue
    ReadOnly _lastItem As Integer = 0
    Private _cScrollSpeed As Integer = 5
    ReadOnly _r As New Random(Now.Millisecond)

    Public Property ScrollSpeed() As Integer
        Get
            Return _cScrollSpeed
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                _cScrollSpeed = 1
            ElseIf value > 100 Then
                _cScrollSpeed = 100
            Else
                _cScrollSpeed = value
            End If
            _tmrScrollTimer.Interval = _cScrollSpeed
            Invalidate()
        End Set
    End Property

    Private _cScrollDistance As Integer = 1
    Public Property ScrollDistance() As Integer
        Get
            Return _cScrollDistance
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                _cScrollDistance = 1
            ElseIf value > 5 Then
                _cScrollDistance = 5
            Else
                _cScrollDistance = value
            End If
            Invalidate()
        End Set
    End Property
    Private _cScrollNumberOfImages As Integer = 5
    Public Property ScrollNumberOfImages() As Integer
        Get
            Return _cScrollNumberOfImages
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                _cScrollNumberOfImages = 1
            ElseIf value > 20 Then
                _cScrollNumberOfImages = 20
            Else
                _cScrollNumberOfImages = value
            End If
            SetupImages()
        End Set
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        _tmrScrollTimer.Interval = 5
        _tmrScrollTimer.Enabled = False
        DoubleBuffered = True
        _r = New Random(Now.Millisecond)
        _lastItem = HQ.TickerItemList.Count
        Call SetupImages()
    End Sub

    Private Sub SetupImages()
        _scrollImages.Clear()
        ' ReSharper disable once RedundantAssignment - Warning incorrectly reported by R#
        For i As Integer = 0 To _cScrollNumberOfImages
            Call SetupImage()
        Next
    End Sub

    Private Sub SetupImage()
        If HQ.TickerItemList.Count > 0 Then
            Dim itemID As Integer = HQ.TickerItemList(_r.Next(0, _lastItem))
            Dim items As New List(Of Integer)
            items.Add(itemID)

            Dim task As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(items)
            task.ContinueWith(Sub(priceTask As Task(Of Dictionary(Of Integer, Double)))
                                  If priceTask.IsCompleted And priceTask.IsFaulted = False And priceTask.Exception Is Nothing And priceTask.Result IsNot Nothing Then
                                      Dim price As Double
                                      If (priceTask.Result.ContainsKey(itemID)) Then
                                          price = priceTask.Result(itemID)
                                      Else
                                          price = 0
                                      End If
                                      If (price > 0) Then
                                          Try
                                              'Bug EVEHQ-169 : this is called even after the window is destroyed but not GC'd. check the handle boolean first.
                                              If IsHandleCreated Then

                                                  Invoke(Sub()
                                                             SetupImage(itemID, price)
                                                         End Sub)
                                              End If
                                          Catch ex As Exception
                                              ' cannot check handle in background thread...
                                          End Try
                                      End If
                                  End If
                              End Sub)

        End If
    End Sub


    Private Sub SetupImage(itemID As Integer, itemPrice As Double)

        Dim mainFont As New Font("Tahoma", 10, FontStyle.Regular)
        Dim smallFont As New Font("Tahoma", 7, FontStyle.Regular)

        Dim itemName As String
        Dim imgText As String


        _img = New Bitmap(600, 30, PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(_img)
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim strWidth As Integer

        itemName = StaticData.Types(itemID).Name

        imgText = itemName & " - " & itemPrice.ToString("N2")

        strWidth = CInt(g.MeasureString(imgText, mainFont).Width)
        g.FillRectangle(New SolidBrush(Color.Black), New Rectangle(0, 0, 300, 40))
        g.DrawString(imgText, mainFont, New SolidBrush(Color.White), 0, 2)
        g.DrawString("(+" & itemID & ")", smallFont, New SolidBrush(Color.LawnGreen), strWidth - 5, 1)
        strWidth += CInt(g.MeasureString("(+" & itemID & ")", smallFont).Width)
        Dim finalImage As Bitmap = _img.Clone(New Rectangle(0, 0, strWidth + 10, 30), _img.PixelFormat)
        Dim si As New ScrollImage
        si.img = finalImage
        si.imgX = 0
        si.imgID = itemID
        _scrollImages.Enqueue(si)
    End Sub

    Private Sub scrollTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles _tmrScrollTimer.Tick
        Invalidate()
    End Sub

    Private Sub Ticker_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDoubleClick
        ' Get co-ords of click
        Dim x As Integer = e.X
        Dim itemID As Integer
        For Each si As ScrollImage In _scrollImages
            If x >= si.imgX And x <= si.imgX + si.img.Width Then
                itemID = si.imgID
                Exit For
            End If
        Next
        Call LaunchItemBrowser(itemID)
    End Sub

    Private Sub LaunchItemBrowser(ByVal itemID As Integer)

        Using myIB As New FrmIB(itemID)
            myIB.ShowDialog()
        End Using

    End Sub

    Private Sub Ticker_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles Me.Paint
        If (_scrollImages Is Nothing Or _scrollImages.Count = 0) Then
            Return ' No data to scroll.
        End If

        Dim startPosition As Integer = CType(_scrollImages.Peek, ScrollImage).imgX - _cScrollDistance
        Dim si As ScrollImage
        For Each si In _scrollImages
            Dim g As Graphics = e.Graphics
            If startPosition < Width Then
                g.DrawImage(si.img, startPosition, 0)
            End If
            si.imgX = startPosition
            startPosition += si.img.Width - 1
        Next
        ' Check for non-displayed items & queue up another (look at the first item only)
        si = CType(_scrollImages.Peek, ScrollImage)
        If si.imgX + si.img.Width < 0 Then
            'si = CType(_scrollImages.Dequeue, ScrollImage)
            SetupImage()
        End If
    End Sub

    Private Class ScrollImage
        Public Property Img As Bitmap
        Public Property ImgX As Integer
        Public Property ImgID As Integer
    End Class

    Private Sub Ticker_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible Then
            _tmrScrollTimer.Enabled = True
        Else
            _tmrScrollTimer.Enabled = False
        End If
    End Sub
End Class


