Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Drawing

Public Class Ticker
    Dim WithEvents scrollTimer As New Timer
    Dim img As Bitmap
    Dim scrollImages As New Queue
    Dim lastItem As Integer = 0
    Private cScrollSpeed As Integer = 5
    Dim r As New Random(Now.Millisecond)

    Public Property ScrollSpeed() As Integer
        Get
            Return cScrollSpeed
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                cScrollSpeed = 1
            ElseIf value > 100 Then
                cScrollSpeed = 100
            Else
                cScrollSpeed = value
            End If
            scrollTimer.Interval = cScrollSpeed
            Invalidate()
        End Set
    End Property

    Private cScrollDistance As Integer = 1
    Public Property ScrollDistance() As Integer
        Get
            Return cScrollDistance
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                cScrollDistance = 1
            ElseIf value > 5 Then
                cScrollDistance = 5
            Else
                cScrollDistance = value
            End If
            Invalidate()
        End Set
    End Property
    Private cScrollNumberOfImages As Integer = 5
    Public Property ScrollNumberOfImages() As Integer
        Get
            Return cScrollNumberOfImages
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                cScrollNumberOfImages = 1
            ElseIf value > 20 Then
                cScrollNumberOfImages = 20
            Else
                cScrollNumberOfImages = value
            End If
            SetupImages()
        End Set
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.DoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        scrollTimer.Interval = 5
        scrollTimer.Enabled = True
        Me.DoubleBuffered = True
        r = New Random(Now.Millisecond)
        lastItem = EveHQ.Core.HQ.itemList.Count
        Call SetupImages()
    End Sub

    Private Sub SetupImages()
        scrollImages.Clear()
        For i As Integer = 0 To cScrollNumberOfImages
            Call Me.SetupImage()
        Next
    End Sub

    Private Sub SetupImage()
        Dim MainFont As New Font("Arial Narrow", 10, FontStyle.Regular)
        Dim SmallFont As New Font("Arial Narrow", 7, FontStyle.Regular)
        Dim itemID As String = ""
        Dim itemName As String = ""
        Dim imgText As String = ""
        Dim itemPrice As Double

        img = New Bitmap(600, 30, Imaging.PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(img)
        g.SmoothingMode = SmoothingMode.HighQuality
        Dim strWidth As Integer
        If EveHQ.Core.HQ.itemList.Count > 0 Then
            Do
                itemName = CStr(EveHQ.Core.HQ.itemList.GetKey(r.Next(1, lastItem)))
                itemID = CStr(EveHQ.Core.HQ.itemList(itemName))
                itemPrice = EveHQ.Core.DataFunctions.GetPrice(itemID)
            Loop Until itemPrice > 0
            imgText = itemName & " - " & FormatNumber(itemPrice, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            imgText = "Placeholder"
        End If
        strWidth = CInt(g.MeasureString(imgText, MainFont).Width)
        g.FillRectangle(New SolidBrush(Color.Black), New Rectangle(0, 0, 300, 40))
        g.DrawString(imgText, MainFont, New SolidBrush(Color.White), 0, 2)
        g.DrawString("(+" & itemID & ")", SmallFont, New SolidBrush(Color.LawnGreen), strWidth - 5, 1)
        strWidth += CInt(g.MeasureString("(+" & itemID & ")", SmallFont).Width)
        Dim finalImage As Bitmap = img.Clone(New Rectangle(0, 0, strWidth + 10, 30), img.PixelFormat)
        Dim si As New ScrollImage
        si.img = finalImage
        si.imgX = 0
        scrollImages.Enqueue(si)
    End Sub

    Private Sub scrollTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles scrollTimer.Tick
        Invalidate()
    End Sub

    Private Sub Ticker_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim startPosition As Integer = CType(scrollImages.Peek, ScrollImage).imgX - cScrollDistance
        Dim si As ScrollImage
        For Each si In scrollImages
            Dim g As Graphics = e.Graphics
            If startPosition < Me.Width Then
                g.DrawImage(si.img, startPosition, 0)
            End If
            si.imgX = startPosition
            startPosition += si.img.Width - 1
        Next
        ' Check for non-displayed items & queue up another (look at the first item only)
        si = CType(scrollImages.Peek, Global.EveHQ.Core.Ticker.ScrollImage)
        If si.imgX + si.img.Width < 0 Then
            si = CType(scrollImages.Dequeue, Global.EveHQ.Core.Ticker.ScrollImage)
            SetupImage()
        End If
    End Sub

    Private Class ScrollImage
        Public img As Bitmap
        Public imgX As Integer
    End Class
End Class


