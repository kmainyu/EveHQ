<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMarketOrders
    Inherits DevComponents.DotNetBar.Office2007Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMarketOrders))
        Me.btnSetCustomPrice = New System.Windows.Forms.Button()
        Me.btnSetMarketPrice = New System.Windows.Forms.Button()
        Me.lblCurrentPrice = New System.Windows.Forms.Label()
        Me.lblCurrentPriceLbl = New System.Windows.Forms.Label()
        Me.lblYourPrice = New System.Windows.Forms.Label()
        Me.lblYourPriceLbl = New System.Windows.Forms.Label()
        Me.lblAllOrderStd = New System.Windows.Forms.Label()
        Me.lblAllOrderMedian = New System.Windows.Forms.Label()
        Me.lblAllOrderMean = New System.Windows.Forms.Label()
        Me.lblAllOrderMax = New System.Windows.Forms.Label()
        Me.lblAllOrderMin = New System.Windows.Forms.Label()
        Me.lblAllOrderVol = New System.Windows.Forms.Label()
        Me.lblAllOrderStdLbl = New System.Windows.Forms.Label()
        Me.lblAllOrderMedianLbl = New System.Windows.Forms.Label()
        Me.lblAllOrderMeanLbl = New System.Windows.Forms.Label()
        Me.lblAllOrderMaxLbl = New System.Windows.Forms.Label()
        Me.lblAllOrderMinLbl = New System.Windows.Forms.Label()
        Me.lblAllOrderVolLbl = New System.Windows.Forms.Label()
        Me.lblBuyOrderStd = New System.Windows.Forms.Label()
        Me.lblBuyOrderMedian = New System.Windows.Forms.Label()
        Me.lblBuyOrderMean = New System.Windows.Forms.Label()
        Me.lblBuyOrderMax = New System.Windows.Forms.Label()
        Me.lblBuyOrderMin = New System.Windows.Forms.Label()
        Me.lblBuyOrderVol = New System.Windows.Forms.Label()
        Me.lblBuyOrderStdLbl = New System.Windows.Forms.Label()
        Me.lblBuyOrderMedianLbl = New System.Windows.Forms.Label()
        Me.lblBuyOrderMeanLbl = New System.Windows.Forms.Label()
        Me.lblBuyOrderMaxLbl = New System.Windows.Forms.Label()
        Me.lblBuyOrderMinLbl = New System.Windows.Forms.Label()
        Me.lblBuyOrderVolLbl = New System.Windows.Forms.Label()
        Me.lblSellOrderStd = New System.Windows.Forms.Label()
        Me.lblSellOrderMedian = New System.Windows.Forms.Label()
        Me.lblSellOrderMean = New System.Windows.Forms.Label()
        Me.lblSellOrderMax = New System.Windows.Forms.Label()
        Me.lblSellOrderMin = New System.Windows.Forms.Label()
        Me.LblSellOrderVol = New System.Windows.Forms.Label()
        Me.lblSellOrderStdLbl = New System.Windows.Forms.Label()
        Me.lblSellOrderMedianLbl = New System.Windows.Forms.Label()
        Me.lblSellOrderMeanLbl = New System.Windows.Forms.Label()
        Me.lblSellOrderMaxLbl = New System.Windows.Forms.Label()
        Me.lblSellOrderMinLbl = New System.Windows.Forms.Label()
        Me.lblSellOrderVolLbl = New System.Windows.Forms.Label()
        Me.lblSellers = New System.Windows.Forms.Label()
        Me.lblBuyers = New System.Windows.Forms.Label()
        Me.panelInfo = New DevComponents.DotNetBar.PanelEx()
        Me.gpAllOrders = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.gpBuyOrders = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.gpSellOrders = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.ExpandableSplitter2 = New DevComponents.DotNetBar.ExpandableSplitter()
        Me.panelGraph = New DevComponents.DotNetBar.PanelEx()
        Me.zgcPrices = New ZedGraph.ZedGraphControl()
        Me.ExpandableSplitter1 = New DevComponents.DotNetBar.ExpandableSplitter()
        Me.panelBuyOrders = New DevComponents.DotNetBar.PanelEx()
        Me.adtBuyers = New DevComponents.AdvTree.AdvTree()
        Me.colBuyLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyQty = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colBuyExpiry = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector2 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle2 = New DevComponents.DotNetBar.ElementStyle()
        Me.ExpandableSplitter3 = New DevComponents.DotNetBar.ExpandableSplitter()
        Me.panelSellOrders = New DevComponents.DotNetBar.PanelEx()
        Me.adtSellers = New DevComponents.AdvTree.AdvTree()
        Me.colSellLocation = New DevComponents.AdvTree.ColumnHeader()
        Me.colSellQty = New DevComponents.AdvTree.ColumnHeader()
        Me.colSellPrice = New DevComponents.AdvTree.ColumnHeader()
        Me.colSellExpiry = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.ElementStyle1 = New DevComponents.DotNetBar.ElementStyle()
        Me.panelInfo.SuspendLayout()
        Me.gpAllOrders.SuspendLayout()
        Me.gpBuyOrders.SuspendLayout()
        Me.gpSellOrders.SuspendLayout()
        Me.panelGraph.SuspendLayout()
        Me.panelBuyOrders.SuspendLayout()
        CType(Me.adtBuyers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelSellOrders.SuspendLayout()
        CType(Me.adtSellers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSetCustomPrice
        '
        Me.btnSetCustomPrice.Location = New System.Drawing.Point(115, 373)
        Me.btnSetCustomPrice.Name = "btnSetCustomPrice"
        Me.btnSetCustomPrice.Size = New System.Drawing.Size(100, 23)
        Me.btnSetCustomPrice.TabIndex = 11
        Me.btnSetCustomPrice.Text = "Set Custom Price"
        Me.btnSetCustomPrice.UseVisualStyleBackColor = True
        '
        'btnSetMarketPrice
        '
        Me.btnSetMarketPrice.Location = New System.Drawing.Point(9, 373)
        Me.btnSetMarketPrice.Name = "btnSetMarketPrice"
        Me.btnSetMarketPrice.Size = New System.Drawing.Size(100, 23)
        Me.btnSetMarketPrice.TabIndex = 10
        Me.btnSetMarketPrice.Text = "Set Market Price"
        Me.btnSetMarketPrice.UseVisualStyleBackColor = True
        '
        'lblCurrentPrice
        '
        Me.lblCurrentPrice.AutoSize = True
        Me.lblCurrentPrice.Location = New System.Drawing.Point(90, 357)
        Me.lblCurrentPrice.Name = "lblCurrentPrice"
        Me.lblCurrentPrice.Size = New System.Drawing.Size(38, 13)
        Me.lblCurrentPrice.TabIndex = 9
        Me.lblCurrentPrice.Text = "Label1"
        '
        'lblCurrentPriceLbl
        '
        Me.lblCurrentPriceLbl.AutoSize = True
        Me.lblCurrentPriceLbl.Location = New System.Drawing.Point(6, 357)
        Me.lblCurrentPriceLbl.Name = "lblCurrentPriceLbl"
        Me.lblCurrentPriceLbl.Size = New System.Drawing.Size(74, 13)
        Me.lblCurrentPriceLbl.TabIndex = 8
        Me.lblCurrentPriceLbl.Text = "Current Price:"
        '
        'lblYourPrice
        '
        Me.lblYourPrice.AutoSize = True
        Me.lblYourPrice.Location = New System.Drawing.Point(90, 344)
        Me.lblYourPrice.Name = "lblYourPrice"
        Me.lblYourPrice.Size = New System.Drawing.Size(38, 13)
        Me.lblYourPrice.TabIndex = 7
        Me.lblYourPrice.Text = "Label1"
        '
        'lblYourPriceLbl
        '
        Me.lblYourPriceLbl.AutoSize = True
        Me.lblYourPriceLbl.Location = New System.Drawing.Point(6, 344)
        Me.lblYourPriceLbl.Name = "lblYourPriceLbl"
        Me.lblYourPriceLbl.Size = New System.Drawing.Size(78, 13)
        Me.lblYourPriceLbl.TabIndex = 3
        Me.lblYourPriceLbl.Text = "Selected Price:"
        '
        'lblAllOrderStd
        '
        Me.lblAllOrderStd.AutoSize = True
        Me.lblAllOrderStd.Location = New System.Drawing.Point(54, 65)
        Me.lblAllOrderStd.Name = "lblAllOrderStd"
        Me.lblAllOrderStd.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderStd.TabIndex = 11
        Me.lblAllOrderStd.Text = "Label1"
        '
        'lblAllOrderMedian
        '
        Me.lblAllOrderMedian.AutoSize = True
        Me.lblAllOrderMedian.Location = New System.Drawing.Point(54, 52)
        Me.lblAllOrderMedian.Name = "lblAllOrderMedian"
        Me.lblAllOrderMedian.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMedian.TabIndex = 10
        Me.lblAllOrderMedian.Text = "Label1"
        '
        'lblAllOrderMean
        '
        Me.lblAllOrderMean.AutoSize = True
        Me.lblAllOrderMean.Location = New System.Drawing.Point(54, 39)
        Me.lblAllOrderMean.Name = "lblAllOrderMean"
        Me.lblAllOrderMean.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMean.TabIndex = 9
        Me.lblAllOrderMean.Text = "Label1"
        '
        'lblAllOrderMax
        '
        Me.lblAllOrderMax.AutoSize = True
        Me.lblAllOrderMax.Location = New System.Drawing.Point(54, 26)
        Me.lblAllOrderMax.Name = "lblAllOrderMax"
        Me.lblAllOrderMax.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMax.TabIndex = 8
        Me.lblAllOrderMax.Text = "Label1"
        '
        'lblAllOrderMin
        '
        Me.lblAllOrderMin.AutoSize = True
        Me.lblAllOrderMin.Location = New System.Drawing.Point(54, 13)
        Me.lblAllOrderMin.Name = "lblAllOrderMin"
        Me.lblAllOrderMin.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMin.TabIndex = 7
        Me.lblAllOrderMin.Text = "Label1"
        '
        'lblAllOrderVol
        '
        Me.lblAllOrderVol.AutoSize = True
        Me.lblAllOrderVol.Location = New System.Drawing.Point(54, 0)
        Me.lblAllOrderVol.Name = "lblAllOrderVol"
        Me.lblAllOrderVol.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderVol.TabIndex = 6
        Me.lblAllOrderVol.Text = "Label1"
        '
        'lblAllOrderStdLbl
        '
        Me.lblAllOrderStdLbl.AutoSize = True
        Me.lblAllOrderStdLbl.Location = New System.Drawing.Point(3, 65)
        Me.lblAllOrderStdLbl.Name = "lblAllOrderStdLbl"
        Me.lblAllOrderStdLbl.Size = New System.Drawing.Size(49, 13)
        Me.lblAllOrderStdLbl.TabIndex = 5
        Me.lblAllOrderStdLbl.Text = "Std Dev:"
        '
        'lblAllOrderMedianLbl
        '
        Me.lblAllOrderMedianLbl.AutoSize = True
        Me.lblAllOrderMedianLbl.Location = New System.Drawing.Point(3, 52)
        Me.lblAllOrderMedianLbl.Name = "lblAllOrderMedianLbl"
        Me.lblAllOrderMedianLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblAllOrderMedianLbl.TabIndex = 4
        Me.lblAllOrderMedianLbl.Text = "Median:"
        '
        'lblAllOrderMeanLbl
        '
        Me.lblAllOrderMeanLbl.AutoSize = True
        Me.lblAllOrderMeanLbl.Location = New System.Drawing.Point(3, 39)
        Me.lblAllOrderMeanLbl.Name = "lblAllOrderMeanLbl"
        Me.lblAllOrderMeanLbl.Size = New System.Drawing.Size(37, 13)
        Me.lblAllOrderMeanLbl.TabIndex = 3
        Me.lblAllOrderMeanLbl.Text = "Mean:"
        '
        'lblAllOrderMaxLbl
        '
        Me.lblAllOrderMaxLbl.AutoSize = True
        Me.lblAllOrderMaxLbl.Location = New System.Drawing.Point(3, 26)
        Me.lblAllOrderMaxLbl.Name = "lblAllOrderMaxLbl"
        Me.lblAllOrderMaxLbl.Size = New System.Drawing.Size(31, 13)
        Me.lblAllOrderMaxLbl.TabIndex = 2
        Me.lblAllOrderMaxLbl.Text = "Max:"
        '
        'lblAllOrderMinLbl
        '
        Me.lblAllOrderMinLbl.AutoSize = True
        Me.lblAllOrderMinLbl.Location = New System.Drawing.Point(3, 13)
        Me.lblAllOrderMinLbl.Name = "lblAllOrderMinLbl"
        Me.lblAllOrderMinLbl.Size = New System.Drawing.Size(27, 13)
        Me.lblAllOrderMinLbl.TabIndex = 1
        Me.lblAllOrderMinLbl.Text = "Min:"
        '
        'lblAllOrderVolLbl
        '
        Me.lblAllOrderVolLbl.AutoSize = True
        Me.lblAllOrderVolLbl.Location = New System.Drawing.Point(3, 0)
        Me.lblAllOrderVolLbl.Name = "lblAllOrderVolLbl"
        Me.lblAllOrderVolLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblAllOrderVolLbl.TabIndex = 0
        Me.lblAllOrderVolLbl.Text = "Volume:"
        '
        'lblBuyOrderStd
        '
        Me.lblBuyOrderStd.AutoSize = True
        Me.lblBuyOrderStd.Location = New System.Drawing.Point(54, 65)
        Me.lblBuyOrderStd.Name = "lblBuyOrderStd"
        Me.lblBuyOrderStd.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderStd.TabIndex = 11
        Me.lblBuyOrderStd.Text = "Label1"
        '
        'lblBuyOrderMedian
        '
        Me.lblBuyOrderMedian.AutoSize = True
        Me.lblBuyOrderMedian.Location = New System.Drawing.Point(54, 52)
        Me.lblBuyOrderMedian.Name = "lblBuyOrderMedian"
        Me.lblBuyOrderMedian.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMedian.TabIndex = 10
        Me.lblBuyOrderMedian.Text = "Label1"
        '
        'lblBuyOrderMean
        '
        Me.lblBuyOrderMean.AutoSize = True
        Me.lblBuyOrderMean.Location = New System.Drawing.Point(54, 39)
        Me.lblBuyOrderMean.Name = "lblBuyOrderMean"
        Me.lblBuyOrderMean.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMean.TabIndex = 9
        Me.lblBuyOrderMean.Text = "Label1"
        '
        'lblBuyOrderMax
        '
        Me.lblBuyOrderMax.AutoSize = True
        Me.lblBuyOrderMax.Location = New System.Drawing.Point(54, 26)
        Me.lblBuyOrderMax.Name = "lblBuyOrderMax"
        Me.lblBuyOrderMax.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMax.TabIndex = 8
        Me.lblBuyOrderMax.Text = "Label1"
        '
        'lblBuyOrderMin
        '
        Me.lblBuyOrderMin.AutoSize = True
        Me.lblBuyOrderMin.Location = New System.Drawing.Point(54, 13)
        Me.lblBuyOrderMin.Name = "lblBuyOrderMin"
        Me.lblBuyOrderMin.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMin.TabIndex = 7
        Me.lblBuyOrderMin.Text = "Label1"
        '
        'lblBuyOrderVol
        '
        Me.lblBuyOrderVol.AutoSize = True
        Me.lblBuyOrderVol.Location = New System.Drawing.Point(54, 0)
        Me.lblBuyOrderVol.Name = "lblBuyOrderVol"
        Me.lblBuyOrderVol.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderVol.TabIndex = 6
        Me.lblBuyOrderVol.Text = "Label1"
        '
        'lblBuyOrderStdLbl
        '
        Me.lblBuyOrderStdLbl.AutoSize = True
        Me.lblBuyOrderStdLbl.Location = New System.Drawing.Point(3, 65)
        Me.lblBuyOrderStdLbl.Name = "lblBuyOrderStdLbl"
        Me.lblBuyOrderStdLbl.Size = New System.Drawing.Size(49, 13)
        Me.lblBuyOrderStdLbl.TabIndex = 5
        Me.lblBuyOrderStdLbl.Text = "Std Dev:"
        '
        'lblBuyOrderMedianLbl
        '
        Me.lblBuyOrderMedianLbl.AutoSize = True
        Me.lblBuyOrderMedianLbl.Location = New System.Drawing.Point(3, 52)
        Me.lblBuyOrderMedianLbl.Name = "lblBuyOrderMedianLbl"
        Me.lblBuyOrderMedianLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblBuyOrderMedianLbl.TabIndex = 4
        Me.lblBuyOrderMedianLbl.Text = "Median:"
        '
        'lblBuyOrderMeanLbl
        '
        Me.lblBuyOrderMeanLbl.AutoSize = True
        Me.lblBuyOrderMeanLbl.Location = New System.Drawing.Point(3, 39)
        Me.lblBuyOrderMeanLbl.Name = "lblBuyOrderMeanLbl"
        Me.lblBuyOrderMeanLbl.Size = New System.Drawing.Size(37, 13)
        Me.lblBuyOrderMeanLbl.TabIndex = 3
        Me.lblBuyOrderMeanLbl.Text = "Mean:"
        '
        'lblBuyOrderMaxLbl
        '
        Me.lblBuyOrderMaxLbl.AutoSize = True
        Me.lblBuyOrderMaxLbl.Location = New System.Drawing.Point(3, 26)
        Me.lblBuyOrderMaxLbl.Name = "lblBuyOrderMaxLbl"
        Me.lblBuyOrderMaxLbl.Size = New System.Drawing.Size(31, 13)
        Me.lblBuyOrderMaxLbl.TabIndex = 2
        Me.lblBuyOrderMaxLbl.Text = "Max:"
        '
        'lblBuyOrderMinLbl
        '
        Me.lblBuyOrderMinLbl.AutoSize = True
        Me.lblBuyOrderMinLbl.Location = New System.Drawing.Point(3, 13)
        Me.lblBuyOrderMinLbl.Name = "lblBuyOrderMinLbl"
        Me.lblBuyOrderMinLbl.Size = New System.Drawing.Size(27, 13)
        Me.lblBuyOrderMinLbl.TabIndex = 1
        Me.lblBuyOrderMinLbl.Text = "Min:"
        '
        'lblBuyOrderVolLbl
        '
        Me.lblBuyOrderVolLbl.AutoSize = True
        Me.lblBuyOrderVolLbl.Location = New System.Drawing.Point(3, 0)
        Me.lblBuyOrderVolLbl.Name = "lblBuyOrderVolLbl"
        Me.lblBuyOrderVolLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblBuyOrderVolLbl.TabIndex = 0
        Me.lblBuyOrderVolLbl.Text = "Volume:"
        '
        'lblSellOrderStd
        '
        Me.lblSellOrderStd.AutoSize = True
        Me.lblSellOrderStd.Location = New System.Drawing.Point(54, 65)
        Me.lblSellOrderStd.Name = "lblSellOrderStd"
        Me.lblSellOrderStd.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderStd.TabIndex = 11
        Me.lblSellOrderStd.Text = "Label1"
        '
        'lblSellOrderMedian
        '
        Me.lblSellOrderMedian.AutoSize = True
        Me.lblSellOrderMedian.Location = New System.Drawing.Point(54, 52)
        Me.lblSellOrderMedian.Name = "lblSellOrderMedian"
        Me.lblSellOrderMedian.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMedian.TabIndex = 10
        Me.lblSellOrderMedian.Text = "Label1"
        '
        'lblSellOrderMean
        '
        Me.lblSellOrderMean.AutoSize = True
        Me.lblSellOrderMean.Location = New System.Drawing.Point(54, 39)
        Me.lblSellOrderMean.Name = "lblSellOrderMean"
        Me.lblSellOrderMean.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMean.TabIndex = 9
        Me.lblSellOrderMean.Text = "Label1"
        '
        'lblSellOrderMax
        '
        Me.lblSellOrderMax.AutoSize = True
        Me.lblSellOrderMax.Location = New System.Drawing.Point(54, 26)
        Me.lblSellOrderMax.Name = "lblSellOrderMax"
        Me.lblSellOrderMax.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMax.TabIndex = 8
        Me.lblSellOrderMax.Text = "Label1"
        '
        'lblSellOrderMin
        '
        Me.lblSellOrderMin.AutoSize = True
        Me.lblSellOrderMin.Location = New System.Drawing.Point(54, 13)
        Me.lblSellOrderMin.Name = "lblSellOrderMin"
        Me.lblSellOrderMin.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMin.TabIndex = 7
        Me.lblSellOrderMin.Text = "Label1"
        '
        'LblSellOrderVol
        '
        Me.LblSellOrderVol.AutoSize = True
        Me.LblSellOrderVol.Location = New System.Drawing.Point(54, 0)
        Me.LblSellOrderVol.Name = "LblSellOrderVol"
        Me.LblSellOrderVol.Size = New System.Drawing.Size(38, 13)
        Me.LblSellOrderVol.TabIndex = 6
        Me.LblSellOrderVol.Text = "Label1"
        '
        'lblSellOrderStdLbl
        '
        Me.lblSellOrderStdLbl.AutoSize = True
        Me.lblSellOrderStdLbl.Location = New System.Drawing.Point(3, 65)
        Me.lblSellOrderStdLbl.Name = "lblSellOrderStdLbl"
        Me.lblSellOrderStdLbl.Size = New System.Drawing.Size(49, 13)
        Me.lblSellOrderStdLbl.TabIndex = 5
        Me.lblSellOrderStdLbl.Text = "Std Dev:"
        '
        'lblSellOrderMedianLbl
        '
        Me.lblSellOrderMedianLbl.AutoSize = True
        Me.lblSellOrderMedianLbl.Location = New System.Drawing.Point(3, 52)
        Me.lblSellOrderMedianLbl.Name = "lblSellOrderMedianLbl"
        Me.lblSellOrderMedianLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblSellOrderMedianLbl.TabIndex = 4
        Me.lblSellOrderMedianLbl.Text = "Median:"
        '
        'lblSellOrderMeanLbl
        '
        Me.lblSellOrderMeanLbl.AutoSize = True
        Me.lblSellOrderMeanLbl.Location = New System.Drawing.Point(3, 39)
        Me.lblSellOrderMeanLbl.Name = "lblSellOrderMeanLbl"
        Me.lblSellOrderMeanLbl.Size = New System.Drawing.Size(37, 13)
        Me.lblSellOrderMeanLbl.TabIndex = 3
        Me.lblSellOrderMeanLbl.Text = "Mean:"
        '
        'lblSellOrderMaxLbl
        '
        Me.lblSellOrderMaxLbl.AutoSize = True
        Me.lblSellOrderMaxLbl.Location = New System.Drawing.Point(3, 26)
        Me.lblSellOrderMaxLbl.Name = "lblSellOrderMaxLbl"
        Me.lblSellOrderMaxLbl.Size = New System.Drawing.Size(31, 13)
        Me.lblSellOrderMaxLbl.TabIndex = 2
        Me.lblSellOrderMaxLbl.Text = "Max:"
        '
        'lblSellOrderMinLbl
        '
        Me.lblSellOrderMinLbl.AutoSize = True
        Me.lblSellOrderMinLbl.Location = New System.Drawing.Point(3, 13)
        Me.lblSellOrderMinLbl.Name = "lblSellOrderMinLbl"
        Me.lblSellOrderMinLbl.Size = New System.Drawing.Size(27, 13)
        Me.lblSellOrderMinLbl.TabIndex = 1
        Me.lblSellOrderMinLbl.Text = "Min:"
        '
        'lblSellOrderVolLbl
        '
        Me.lblSellOrderVolLbl.AutoSize = True
        Me.lblSellOrderVolLbl.Location = New System.Drawing.Point(3, 0)
        Me.lblSellOrderVolLbl.Name = "lblSellOrderVolLbl"
        Me.lblSellOrderVolLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblSellOrderVolLbl.TabIndex = 0
        Me.lblSellOrderVolLbl.Text = "Volume:"
        '
        'lblSellers
        '
        Me.lblSellers.AutoSize = True
        Me.lblSellers.Font = New System.Drawing.Font("Tahoma", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSellers.Location = New System.Drawing.Point(3, 9)
        Me.lblSellers.Name = "lblSellers"
        Me.lblSellers.Size = New System.Drawing.Size(53, 13)
        Me.lblSellers.TabIndex = 0
        Me.lblSellers.Text = "SELLERS"
        '
        'lblBuyers
        '
        Me.lblBuyers.AutoSize = True
        Me.lblBuyers.Font = New System.Drawing.Font("Tahoma", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBuyers.Location = New System.Drawing.Point(3, 3)
        Me.lblBuyers.Name = "lblBuyers"
        Me.lblBuyers.Size = New System.Drawing.Size(50, 13)
        Me.lblBuyers.TabIndex = 3
        Me.lblBuyers.Text = "BUYERS"
        '
        'panelInfo
        '
        Me.panelInfo.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelInfo.Controls.Add(Me.gpAllOrders)
        Me.panelInfo.Controls.Add(Me.gpBuyOrders)
        Me.panelInfo.Controls.Add(Me.gpSellOrders)
        Me.panelInfo.Controls.Add(Me.ExpandableSplitter2)
        Me.panelInfo.Controls.Add(Me.panelGraph)
        Me.panelInfo.Controls.Add(Me.lblYourPriceLbl)
        Me.panelInfo.Controls.Add(Me.lblCurrentPriceLbl)
        Me.panelInfo.Controls.Add(Me.lblYourPrice)
        Me.panelInfo.Controls.Add(Me.btnSetCustomPrice)
        Me.panelInfo.Controls.Add(Me.lblCurrentPrice)
        Me.panelInfo.Controls.Add(Me.btnSetMarketPrice)
        Me.panelInfo.Dock = System.Windows.Forms.DockStyle.Right
        Me.panelInfo.Location = New System.Drawing.Point(851, 0)
        Me.panelInfo.Name = "panelInfo"
        Me.panelInfo.Size = New System.Drawing.Size(369, 849)
        Me.panelInfo.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelInfo.Style.GradientAngle = 90
        Me.panelInfo.TabIndex = 12
        '
        'gpAllOrders
        '
        Me.gpAllOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpAllOrders.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpAllOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderStd)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderVolLbl)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMedian)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMinLbl)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMean)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMaxLbl)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMax)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMeanLbl)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMin)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderMedianLbl)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderVol)
        Me.gpAllOrders.Controls.Add(Me.lblAllOrderStdLbl)
        Me.gpAllOrders.Location = New System.Drawing.Point(6, 225)
        Me.gpAllOrders.Name = "gpAllOrders"
        Me.gpAllOrders.Size = New System.Drawing.Size(354, 105)
        '
        '
        '
        Me.gpAllOrders.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpAllOrders.Style.BackColorGradientAngle = 90
        Me.gpAllOrders.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpAllOrders.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAllOrders.Style.BorderBottomWidth = 1
        Me.gpAllOrders.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpAllOrders.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAllOrders.Style.BorderLeftWidth = 1
        Me.gpAllOrders.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAllOrders.Style.BorderRightWidth = 1
        Me.gpAllOrders.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpAllOrders.Style.BorderTopWidth = 1
        Me.gpAllOrders.Style.Class = ""
        Me.gpAllOrders.Style.CornerDiameter = 4
        Me.gpAllOrders.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpAllOrders.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpAllOrders.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpAllOrders.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpAllOrders.StyleMouseDown.Class = ""
        Me.gpAllOrders.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpAllOrders.StyleMouseOver.Class = ""
        Me.gpAllOrders.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpAllOrders.TabIndex = 4
        Me.gpAllOrders.Text = "All Orders"
        '
        'gpBuyOrders
        '
        Me.gpBuyOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpBuyOrders.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpBuyOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderStd)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderVolLbl)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMedian)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMinLbl)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMean)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMaxLbl)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMax)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMeanLbl)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMin)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderMedianLbl)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderVol)
        Me.gpBuyOrders.Controls.Add(Me.lblBuyOrderStdLbl)
        Me.gpBuyOrders.Location = New System.Drawing.Point(6, 114)
        Me.gpBuyOrders.Name = "gpBuyOrders"
        Me.gpBuyOrders.Size = New System.Drawing.Size(354, 105)
        '
        '
        '
        Me.gpBuyOrders.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpBuyOrders.Style.BackColorGradientAngle = 90
        Me.gpBuyOrders.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpBuyOrders.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpBuyOrders.Style.BorderBottomWidth = 1
        Me.gpBuyOrders.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpBuyOrders.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpBuyOrders.Style.BorderLeftWidth = 1
        Me.gpBuyOrders.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpBuyOrders.Style.BorderRightWidth = 1
        Me.gpBuyOrders.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpBuyOrders.Style.BorderTopWidth = 1
        Me.gpBuyOrders.Style.Class = ""
        Me.gpBuyOrders.Style.CornerDiameter = 4
        Me.gpBuyOrders.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpBuyOrders.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpBuyOrders.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpBuyOrders.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpBuyOrders.StyleMouseDown.Class = ""
        Me.gpBuyOrders.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpBuyOrders.StyleMouseOver.Class = ""
        Me.gpBuyOrders.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpBuyOrders.TabIndex = 3
        Me.gpBuyOrders.Text = "Buy Orders"
        '
        'gpSellOrders
        '
        Me.gpSellOrders.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpSellOrders.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpSellOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderStd)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderVolLbl)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMedian)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMinLbl)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMean)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMaxLbl)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMax)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMeanLbl)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMin)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderMedianLbl)
        Me.gpSellOrders.Controls.Add(Me.LblSellOrderVol)
        Me.gpSellOrders.Controls.Add(Me.lblSellOrderStdLbl)
        Me.gpSellOrders.Location = New System.Drawing.Point(6, 3)
        Me.gpSellOrders.Name = "gpSellOrders"
        Me.gpSellOrders.Size = New System.Drawing.Size(354, 105)
        '
        '
        '
        Me.gpSellOrders.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpSellOrders.Style.BackColorGradientAngle = 90
        Me.gpSellOrders.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpSellOrders.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSellOrders.Style.BorderBottomWidth = 1
        Me.gpSellOrders.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpSellOrders.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSellOrders.Style.BorderLeftWidth = 1
        Me.gpSellOrders.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSellOrders.Style.BorderRightWidth = 1
        Me.gpSellOrders.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpSellOrders.Style.BorderTopWidth = 1
        Me.gpSellOrders.Style.Class = ""
        Me.gpSellOrders.Style.CornerDiameter = 4
        Me.gpSellOrders.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpSellOrders.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpSellOrders.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpSellOrders.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpSellOrders.StyleMouseDown.Class = ""
        Me.gpSellOrders.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpSellOrders.StyleMouseOver.Class = ""
        Me.gpSellOrders.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpSellOrders.TabIndex = 2
        Me.gpSellOrders.Text = "Sell Orders"
        '
        'ExpandableSplitter2
        '
        Me.ExpandableSplitter2.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter2.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter2.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.ExpandableSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ExpandableSplitter2.ExpandableControl = Me.panelGraph
        Me.ExpandableSplitter2.ExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter2.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter2.ExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter2.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter2.GripDarkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter2.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter2.GripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter2.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter2.HotBackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(61, Byte), Integer))
        Me.ExpandableSplitter2.HotBackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.ExpandableSplitter2.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2
        Me.ExpandableSplitter2.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground
        Me.ExpandableSplitter2.HotExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter2.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter2.HotExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter2.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter2.HotGripDarkColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter2.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter2.HotGripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter2.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter2.Location = New System.Drawing.Point(0, 412)
        Me.ExpandableSplitter2.Name = "ExpandableSplitter2"
        Me.ExpandableSplitter2.Size = New System.Drawing.Size(369, 6)
        Me.ExpandableSplitter2.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007
        Me.ExpandableSplitter2.TabIndex = 1
        Me.ExpandableSplitter2.TabStop = False
        '
        'panelGraph
        '
        Me.panelGraph.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelGraph.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelGraph.Controls.Add(Me.zgcPrices)
        Me.panelGraph.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelGraph.Location = New System.Drawing.Point(0, 418)
        Me.panelGraph.Name = "panelGraph"
        Me.panelGraph.Size = New System.Drawing.Size(369, 431)
        Me.panelGraph.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelGraph.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelGraph.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelGraph.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelGraph.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelGraph.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelGraph.Style.GradientAngle = 90
        Me.panelGraph.TabIndex = 0
        Me.panelGraph.Text = "PanelEx1"
        '
        'zgcPrices
        '
        Me.zgcPrices.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgcPrices.Location = New System.Drawing.Point(0, 0)
        Me.zgcPrices.Name = "zgcPrices"
        Me.zgcPrices.ScrollGrace = 0.0R
        Me.zgcPrices.ScrollMaxX = 0.0R
        Me.zgcPrices.ScrollMaxY = 0.0R
        Me.zgcPrices.ScrollMaxY2 = 0.0R
        Me.zgcPrices.ScrollMinX = 0.0R
        Me.zgcPrices.ScrollMinY = 0.0R
        Me.zgcPrices.ScrollMinY2 = 0.0R
        Me.zgcPrices.Size = New System.Drawing.Size(369, 431)
        Me.zgcPrices.TabIndex = 0
        '
        'ExpandableSplitter1
        '
        Me.ExpandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.ExpandableSplitter1.Dock = System.Windows.Forms.DockStyle.Right
        Me.ExpandableSplitter1.ExpandableControl = Me.panelInfo
        Me.ExpandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(61, Byte), Integer))
        Me.ExpandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.ExpandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2
        Me.ExpandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground
        Me.ExpandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter1.Location = New System.Drawing.Point(841, 0)
        Me.ExpandableSplitter1.Name = "ExpandableSplitter1"
        Me.ExpandableSplitter1.Size = New System.Drawing.Size(10, 849)
        Me.ExpandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007
        Me.ExpandableSplitter1.TabIndex = 13
        Me.ExpandableSplitter1.TabStop = False
        '
        'panelBuyOrders
        '
        Me.panelBuyOrders.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelBuyOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelBuyOrders.Controls.Add(Me.adtBuyers)
        Me.panelBuyOrders.Controls.Add(Me.lblBuyers)
        Me.panelBuyOrders.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelBuyOrders.Location = New System.Drawing.Point(0, 418)
        Me.panelBuyOrders.Name = "panelBuyOrders"
        Me.panelBuyOrders.Size = New System.Drawing.Size(841, 431)
        Me.panelBuyOrders.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelBuyOrders.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelBuyOrders.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelBuyOrders.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelBuyOrders.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelBuyOrders.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelBuyOrders.Style.GradientAngle = 90
        Me.panelBuyOrders.TabIndex = 14
        '
        'adtBuyers
        '
        Me.adtBuyers.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtBuyers.AllowDrop = True
        Me.adtBuyers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtBuyers.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtBuyers.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtBuyers.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtBuyers.Columns.Add(Me.colBuyLocation)
        Me.adtBuyers.Columns.Add(Me.colBuyQty)
        Me.adtBuyers.Columns.Add(Me.colBuyPrice)
        Me.adtBuyers.Columns.Add(Me.colBuyExpiry)
        Me.adtBuyers.DragDropEnabled = False
        Me.adtBuyers.DragDropNodeCopyEnabled = False
        Me.adtBuyers.ExpandWidth = 0
        Me.adtBuyers.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtBuyers.Location = New System.Drawing.Point(6, 19)
        Me.adtBuyers.Name = "adtBuyers"
        Me.adtBuyers.NodesConnector = Me.NodeConnector2
        Me.adtBuyers.NodeStyle = Me.ElementStyle2
        Me.adtBuyers.PathSeparator = ";"
        Me.adtBuyers.Size = New System.Drawing.Size(829, 409)
        Me.adtBuyers.Styles.Add(Me.ElementStyle2)
        Me.adtBuyers.TabIndex = 4
        Me.adtBuyers.Text = "AdvTree1"
        '
        'colBuyLocation
        '
        Me.colBuyLocation.DisplayIndex = 1
        Me.colBuyLocation.Name = "colBuyLocation"
        Me.colBuyLocation.SortingEnabled = False
        Me.colBuyLocation.Text = "Location"
        Me.colBuyLocation.Width.Absolute = 300
        '
        'colBuyQty
        '
        Me.colBuyQty.DisplayIndex = 2
        Me.colBuyQty.Name = "colBuyQty"
        Me.colBuyQty.SortingEnabled = False
        Me.colBuyQty.Text = "Quantity"
        Me.colBuyQty.Width.Absolute = 120
        '
        'colBuyPrice
        '
        Me.colBuyPrice.DisplayIndex = 3
        Me.colBuyPrice.Name = "colBuyPrice"
        Me.colBuyPrice.SortingEnabled = False
        Me.colBuyPrice.Text = "Price"
        Me.colBuyPrice.Width.Absolute = 120
        '
        'colBuyExpiry
        '
        Me.colBuyExpiry.DisplayIndex = 4
        Me.colBuyExpiry.EditorType = DevComponents.AdvTree.eCellEditorType.Custom
        Me.colBuyExpiry.Name = "colBuyExpiry"
        Me.colBuyExpiry.SortingEnabled = False
        Me.colBuyExpiry.Text = "Expires In"
        Me.colBuyExpiry.Width.Absolute = 120
        '
        'NodeConnector2
        '
        Me.NodeConnector2.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle2
        '
        Me.ElementStyle2.Class = ""
        Me.ElementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle2.Name = "ElementStyle2"
        Me.ElementStyle2.TextColor = System.Drawing.SystemColors.ControlText
        '
        'ExpandableSplitter3
        '
        Me.ExpandableSplitter3.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter3.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter3.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.ExpandableSplitter3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ExpandableSplitter3.ExpandableControl = Me.panelBuyOrders
        Me.ExpandableSplitter3.ExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter3.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter3.ExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter3.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter3.GripDarkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter3.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter3.GripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter3.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter3.HotBackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(61, Byte), Integer))
        Me.ExpandableSplitter3.HotBackColor2 = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.ExpandableSplitter3.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2
        Me.ExpandableSplitter3.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground
        Me.ExpandableSplitter3.HotExpandFillColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter3.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter3.HotExpandLineColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ExpandableSplitter3.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText
        Me.ExpandableSplitter3.HotGripDarkColor = System.Drawing.Color.FromArgb(CType(CType(167, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.ExpandableSplitter3.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.ExpandableSplitter3.HotGripLightColor = System.Drawing.Color.FromArgb(CType(CType(205, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(213, Byte), Integer))
        Me.ExpandableSplitter3.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground
        Me.ExpandableSplitter3.Location = New System.Drawing.Point(0, 412)
        Me.ExpandableSplitter3.Name = "ExpandableSplitter3"
        Me.ExpandableSplitter3.Size = New System.Drawing.Size(841, 6)
        Me.ExpandableSplitter3.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007
        Me.ExpandableSplitter3.TabIndex = 15
        Me.ExpandableSplitter3.TabStop = False
        '
        'panelSellOrders
        '
        Me.panelSellOrders.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelSellOrders.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelSellOrders.Controls.Add(Me.adtSellers)
        Me.panelSellOrders.Controls.Add(Me.lblSellers)
        Me.panelSellOrders.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelSellOrders.Location = New System.Drawing.Point(0, 0)
        Me.panelSellOrders.Name = "panelSellOrders"
        Me.panelSellOrders.Size = New System.Drawing.Size(841, 412)
        Me.panelSellOrders.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelSellOrders.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelSellOrders.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelSellOrders.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelSellOrders.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelSellOrders.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelSellOrders.Style.GradientAngle = 90
        Me.panelSellOrders.TabIndex = 16
        '
        'adtSellers
        '
        Me.adtSellers.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtSellers.AllowDrop = True
        Me.adtSellers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtSellers.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtSellers.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtSellers.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtSellers.Columns.Add(Me.colSellLocation)
        Me.adtSellers.Columns.Add(Me.colSellQty)
        Me.adtSellers.Columns.Add(Me.colSellPrice)
        Me.adtSellers.Columns.Add(Me.colSellExpiry)
        Me.adtSellers.DragDropEnabled = False
        Me.adtSellers.DragDropNodeCopyEnabled = False
        Me.adtSellers.ExpandWidth = 0
        Me.adtSellers.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtSellers.Location = New System.Drawing.Point(6, 25)
        Me.adtSellers.Name = "adtSellers"
        Me.adtSellers.NodesConnector = Me.NodeConnector1
        Me.adtSellers.NodeStyle = Me.ElementStyle1
        Me.adtSellers.PathSeparator = ";"
        Me.adtSellers.Size = New System.Drawing.Size(829, 384)
        Me.adtSellers.Styles.Add(Me.ElementStyle1)
        Me.adtSellers.TabIndex = 1
        Me.adtSellers.Text = "AdvTree1"
        '
        'colSellLocation
        '
        Me.colSellLocation.DisplayIndex = 1
        Me.colSellLocation.Name = "colSellLocation"
        Me.colSellLocation.SortingEnabled = False
        Me.colSellLocation.Text = "Location"
        Me.colSellLocation.Width.Absolute = 300
        '
        'colSellQty
        '
        Me.colSellQty.DisplayIndex = 2
        Me.colSellQty.Name = "colSellQty"
        Me.colSellQty.SortingEnabled = False
        Me.colSellQty.Text = "Quantity"
        Me.colSellQty.Width.Absolute = 120
        '
        'colSellPrice
        '
        Me.colSellPrice.DisplayIndex = 3
        Me.colSellPrice.Name = "colSellPrice"
        Me.colSellPrice.SortingEnabled = False
        Me.colSellPrice.Text = "Price"
        Me.colSellPrice.Width.Absolute = 120
        '
        'colSellExpiry
        '
        Me.colSellExpiry.DisplayIndex = 4
        Me.colSellExpiry.EditorType = DevComponents.AdvTree.eCellEditorType.Custom
        Me.colSellExpiry.Name = "colSellExpiry"
        Me.colSellExpiry.SortingEnabled = False
        Me.colSellExpiry.Text = "Expires In"
        Me.colSellExpiry.Width.Absolute = 120
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        '
        'ElementStyle1
        '
        Me.ElementStyle1.Class = ""
        Me.ElementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.ElementStyle1.Name = "ElementStyle1"
        Me.ElementStyle1.TextColor = System.Drawing.SystemColors.ControlText
        '
        'frmMarketOrders
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1220, 849)
        Me.Controls.Add(Me.panelSellOrders)
        Me.Controls.Add(Me.ExpandableSplitter3)
        Me.Controls.Add(Me.panelBuyOrders)
        Me.Controls.Add(Me.ExpandableSplitter1)
        Me.Controls.Add(Me.panelInfo)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketOrders"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Market Orders"
        Me.panelInfo.ResumeLayout(False)
        Me.panelInfo.PerformLayout()
        Me.gpAllOrders.ResumeLayout(False)
        Me.gpAllOrders.PerformLayout()
        Me.gpBuyOrders.ResumeLayout(False)
        Me.gpBuyOrders.PerformLayout()
        Me.gpSellOrders.ResumeLayout(False)
        Me.gpSellOrders.PerformLayout()
        Me.panelGraph.ResumeLayout(False)
        Me.panelBuyOrders.ResumeLayout(False)
        Me.panelBuyOrders.PerformLayout()
        CType(Me.adtBuyers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelSellOrders.ResumeLayout(False)
        Me.panelSellOrders.PerformLayout()
        CType(Me.adtSellers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblSellers As System.Windows.Forms.Label
    Friend WithEvents lblBuyers As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderVolLbl As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderStd As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMedian As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMean As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMax As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMin As System.Windows.Forms.Label
    Friend WithEvents LblSellOrderVol As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderStdLbl As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMedianLbl As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMeanLbl As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMaxLbl As System.Windows.Forms.Label
    Friend WithEvents lblSellOrderMinLbl As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderStd As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMedian As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMean As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMax As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMin As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderVol As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderStdLbl As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMedianLbl As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMeanLbl As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMaxLbl As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderMinLbl As System.Windows.Forms.Label
    Friend WithEvents lblAllOrderVolLbl As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderStd As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMedian As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMean As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMax As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMin As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderVol As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderStdLbl As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMedianLbl As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMeanLbl As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMaxLbl As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderMinLbl As System.Windows.Forms.Label
    Friend WithEvents lblBuyOrderVolLbl As System.Windows.Forms.Label
    Friend WithEvents lblYourPrice As System.Windows.Forms.Label
    Friend WithEvents lblYourPriceLbl As System.Windows.Forms.Label
    Friend WithEvents zgcPrices As ZedGraph.ZedGraphControl
    Friend WithEvents lblCurrentPrice As System.Windows.Forms.Label
    Friend WithEvents lblCurrentPriceLbl As System.Windows.Forms.Label
    Friend WithEvents btnSetMarketPrice As System.Windows.Forms.Button
    Friend WithEvents btnSetCustomPrice As System.Windows.Forms.Button
    Friend WithEvents panelInfo As DevComponents.DotNetBar.PanelEx
    Friend WithEvents ExpandableSplitter2 As DevComponents.DotNetBar.ExpandableSplitter
    Friend WithEvents panelGraph As DevComponents.DotNetBar.PanelEx
    Friend WithEvents ExpandableSplitter1 As DevComponents.DotNetBar.ExpandableSplitter
    Friend WithEvents gpAllOrders As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents gpBuyOrders As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents gpSellOrders As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents panelBuyOrders As DevComponents.DotNetBar.PanelEx
    Friend WithEvents ExpandableSplitter3 As DevComponents.DotNetBar.ExpandableSplitter
    Friend WithEvents panelSellOrders As DevComponents.DotNetBar.PanelEx
    Friend WithEvents adtSellers As DevComponents.AdvTree.AdvTree
    Friend WithEvents colSellLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSellQty As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSellPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colSellExpiry As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle1 As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents adtBuyers As DevComponents.AdvTree.AdvTree
    Friend WithEvents colBuyLocation As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyQty As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyPrice As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colBuyExpiry As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents NodeConnector2 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents ElementStyle2 As DevComponents.DotNetBar.ElementStyle
End Class
