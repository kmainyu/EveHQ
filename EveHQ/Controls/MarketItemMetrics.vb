Namespace Controls
    Public Class MarketItemMetrics


        Public Property Title() As String
            Get
                Return MetricsPanel.Text
            End Get
            Set(value As String)
                MetricsPanel.Text = value
            End Set
        End Property

        Public Property Volume() As String
            Get
                Return _volume.Text
            End Get
            Set(value As String)
                _volume.Text = value
            End Set
        End Property

        Public Property Minimum() As String
            Get
                Return _minimumPrice.Text
            End Get
            Set(value As String)
                _minimumPrice.Text = value
            End Set
        End Property

        Public Property Maximum() As String
            Get
                Return _maximumPrice.Text
            End Get
            Set(value As String)
                _maximumPrice.Text = value
            End Set
        End Property

        Public Property Average() As String
            Get
                Return _averagePrice.Text
            End Get
            Set(value As String)
                _averagePrice.Text = value
            End Set
        End Property

        Public Property Median() As String
            Get
                Return _medianPrice.Text
            End Get
            Set(value As String)
                _medianPrice.Text = value
            End Set
        End Property

        Public Property StdDeviation() As String
            Get
                Return _stdDeviation.Text
            End Get
            Set(value As String)
                _stdDeviation.Text = value
            End Set
        End Property



        Public Property Percentile() As String
            Get
                Return _percentilePrice.Text
            End Get
            Set(value As String)
                _percentilePrice.Text = value
            End Set
        End Property


    End Class
End NameSpace