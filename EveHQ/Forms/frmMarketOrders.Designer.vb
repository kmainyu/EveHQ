<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMarketOrders
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMarketOrders))
        Me.scOrderData = New System.Windows.Forms.SplitContainer
        Me.btnAddPricesToData = New System.Windows.Forms.Button
        Me.btnSetPrice = New System.Windows.Forms.Button
        Me.lblCurrentPrice = New System.Windows.Forms.Label
        Me.lblCurrentPriceLbl = New System.Windows.Forms.Label
        Me.lblYourPrice = New System.Windows.Forms.Label
        Me.lblYourPriceLbl = New System.Windows.Forms.Label
        Me.gbAllData = New System.Windows.Forms.GroupBox
        Me.lblAllOrderStd = New System.Windows.Forms.Label
        Me.lblAllOrderMedian = New System.Windows.Forms.Label
        Me.lblAllOrderMean = New System.Windows.Forms.Label
        Me.lblAllOrderMax = New System.Windows.Forms.Label
        Me.lblAllOrderMin = New System.Windows.Forms.Label
        Me.lblAllOrderVol = New System.Windows.Forms.Label
        Me.lblAllOrderStdLbl = New System.Windows.Forms.Label
        Me.lblAllOrderMedianLbl = New System.Windows.Forms.Label
        Me.lblAllOrderMeanLbl = New System.Windows.Forms.Label
        Me.lblAllOrderMaxLbl = New System.Windows.Forms.Label
        Me.lblAllOrderMinLbl = New System.Windows.Forms.Label
        Me.lblAllOrderVolLbl = New System.Windows.Forms.Label
        Me.gbBuyData = New System.Windows.Forms.GroupBox
        Me.lblBuyOrderStd = New System.Windows.Forms.Label
        Me.lblBuyOrderMedian = New System.Windows.Forms.Label
        Me.lblBuyOrderMean = New System.Windows.Forms.Label
        Me.lblBuyOrderMax = New System.Windows.Forms.Label
        Me.lblBuyOrderMin = New System.Windows.Forms.Label
        Me.lblBuyOrderVol = New System.Windows.Forms.Label
        Me.lblBuyOrderStdLbl = New System.Windows.Forms.Label
        Me.lblBuyOrderMedianLbl = New System.Windows.Forms.Label
        Me.lblBuyOrderMeanLbl = New System.Windows.Forms.Label
        Me.lblBuyOrderMaxLbl = New System.Windows.Forms.Label
        Me.lblBuyOrderMinLbl = New System.Windows.Forms.Label
        Me.lblBuyOrderVolLbl = New System.Windows.Forms.Label
        Me.gbSellData = New System.Windows.Forms.GroupBox
        Me.lblSellOrderStd = New System.Windows.Forms.Label
        Me.lblSellOrderMedian = New System.Windows.Forms.Label
        Me.lblSellOrderMean = New System.Windows.Forms.Label
        Me.lblSellOrderMax = New System.Windows.Forms.Label
        Me.lblSellOrderMin = New System.Windows.Forms.Label
        Me.LblSellOrderVol = New System.Windows.Forms.Label
        Me.lblSellOrderStdLbl = New System.Windows.Forms.Label
        Me.lblSellOrderMedianLbl = New System.Windows.Forms.Label
        Me.lblSellOrderMeanLbl = New System.Windows.Forms.Label
        Me.lblSellOrderMaxLbl = New System.Windows.Forms.Label
        Me.lblSellOrderMinLbl = New System.Windows.Forms.Label
        Me.lblSellOrderVolLbl = New System.Windows.Forms.Label
        Me.zgcPrices = New ZedGraph.ZedGraphControl
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.clvSellers = New DotNetLib.Windows.Forms.ContainerListView
        Me.colLocation = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colQuantity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colExpires = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblSellers = New System.Windows.Forms.Label
        Me.clvBuyers = New DotNetLib.Windows.Forms.ContainerListView
        Me.ContainerListViewColumnHeader1 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader2 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader3 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader4 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblBuyers = New System.Windows.Forms.Label
        Me.scOrderData.Panel1.SuspendLayout()
        Me.scOrderData.Panel2.SuspendLayout()
        Me.scOrderData.SuspendLayout()
        Me.gbAllData.SuspendLayout()
        Me.gbBuyData.SuspendLayout()
        Me.gbSellData.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'scOrderData
        '
        Me.scOrderData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.scOrderData.Dock = System.Windows.Forms.DockStyle.Right
        Me.scOrderData.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scOrderData.Location = New System.Drawing.Point(749, 0)
        Me.scOrderData.Name = "scOrderData"
        Me.scOrderData.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scOrderData.Panel1
        '
        Me.scOrderData.Panel1.Controls.Add(Me.btnAddPricesToData)
        Me.scOrderData.Panel1.Controls.Add(Me.btnSetPrice)
        Me.scOrderData.Panel1.Controls.Add(Me.lblCurrentPrice)
        Me.scOrderData.Panel1.Controls.Add(Me.lblCurrentPriceLbl)
        Me.scOrderData.Panel1.Controls.Add(Me.lblYourPrice)
        Me.scOrderData.Panel1.Controls.Add(Me.lblYourPriceLbl)
        Me.scOrderData.Panel1.Controls.Add(Me.gbAllData)
        Me.scOrderData.Panel1.Controls.Add(Me.gbBuyData)
        Me.scOrderData.Panel1.Controls.Add(Me.gbSellData)
        '
        'scOrderData.Panel2
        '
        Me.scOrderData.Panel2.Controls.Add(Me.zgcPrices)
        Me.scOrderData.Size = New System.Drawing.Size(235, 664)
        Me.scOrderData.SplitterDistance = 423
        Me.scOrderData.TabIndex = 0
        '
        'btnAddPricesToData
        '
        Me.btnAddPricesToData.Location = New System.Drawing.Point(119, 391)
        Me.btnAddPricesToData.Name = "btnAddPricesToData"
        Me.btnAddPricesToData.Size = New System.Drawing.Size(100, 23)
        Me.btnAddPricesToData.TabIndex = 11
        Me.btnAddPricesToData.Text = "Add Price Data"
        Me.btnAddPricesToData.UseVisualStyleBackColor = True
        '
        'btnSetPrice
        '
        Me.btnSetPrice.Location = New System.Drawing.Point(13, 391)
        Me.btnSetPrice.Name = "btnSetPrice"
        Me.btnSetPrice.Size = New System.Drawing.Size(100, 23)
        Me.btnSetPrice.TabIndex = 10
        Me.btnSetPrice.Text = "Set Price"
        Me.btnSetPrice.UseVisualStyleBackColor = True
        '
        'lblCurrentPrice
        '
        Me.lblCurrentPrice.AutoSize = True
        Me.lblCurrentPrice.Location = New System.Drawing.Point(94, 375)
        Me.lblCurrentPrice.Name = "lblCurrentPrice"
        Me.lblCurrentPrice.Size = New System.Drawing.Size(38, 13)
        Me.lblCurrentPrice.TabIndex = 9
        Me.lblCurrentPrice.Text = "Label1"
        '
        'lblCurrentPriceLbl
        '
        Me.lblCurrentPriceLbl.AutoSize = True
        Me.lblCurrentPriceLbl.Location = New System.Drawing.Point(10, 375)
        Me.lblCurrentPriceLbl.Name = "lblCurrentPriceLbl"
        Me.lblCurrentPriceLbl.Size = New System.Drawing.Size(74, 13)
        Me.lblCurrentPriceLbl.TabIndex = 8
        Me.lblCurrentPriceLbl.Text = "Current Price:"
        '
        'lblYourPrice
        '
        Me.lblYourPrice.AutoSize = True
        Me.lblYourPrice.Location = New System.Drawing.Point(94, 362)
        Me.lblYourPrice.Name = "lblYourPrice"
        Me.lblYourPrice.Size = New System.Drawing.Size(38, 13)
        Me.lblYourPrice.TabIndex = 7
        Me.lblYourPrice.Text = "Label1"
        '
        'lblYourPriceLbl
        '
        Me.lblYourPriceLbl.AutoSize = True
        Me.lblYourPriceLbl.Location = New System.Drawing.Point(10, 362)
        Me.lblYourPriceLbl.Name = "lblYourPriceLbl"
        Me.lblYourPriceLbl.Size = New System.Drawing.Size(78, 13)
        Me.lblYourPriceLbl.TabIndex = 3
        Me.lblYourPriceLbl.Text = "Selected Price:"
        '
        'gbAllData
        '
        Me.gbAllData.Controls.Add(Me.lblAllOrderStd)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMedian)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMean)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMax)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMin)
        Me.gbAllData.Controls.Add(Me.lblAllOrderVol)
        Me.gbAllData.Controls.Add(Me.lblAllOrderStdLbl)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMedianLbl)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMeanLbl)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMaxLbl)
        Me.gbAllData.Controls.Add(Me.lblAllOrderMinLbl)
        Me.gbAllData.Controls.Add(Me.lblAllOrderVolLbl)
        Me.gbAllData.Location = New System.Drawing.Point(4, 245)
        Me.gbAllData.Name = "gbAllData"
        Me.gbAllData.Size = New System.Drawing.Size(252, 114)
        Me.gbAllData.TabIndex = 2
        Me.gbAllData.TabStop = False
        Me.gbAllData.Text = "All Orders"
        '
        'lblAllOrderStd
        '
        Me.lblAllOrderStd.AutoSize = True
        Me.lblAllOrderStd.Location = New System.Drawing.Point(57, 87)
        Me.lblAllOrderStd.Name = "lblAllOrderStd"
        Me.lblAllOrderStd.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderStd.TabIndex = 11
        Me.lblAllOrderStd.Text = "Label1"
        '
        'lblAllOrderMedian
        '
        Me.lblAllOrderMedian.AutoSize = True
        Me.lblAllOrderMedian.Location = New System.Drawing.Point(57, 74)
        Me.lblAllOrderMedian.Name = "lblAllOrderMedian"
        Me.lblAllOrderMedian.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMedian.TabIndex = 10
        Me.lblAllOrderMedian.Text = "Label1"
        '
        'lblAllOrderMean
        '
        Me.lblAllOrderMean.AutoSize = True
        Me.lblAllOrderMean.Location = New System.Drawing.Point(57, 61)
        Me.lblAllOrderMean.Name = "lblAllOrderMean"
        Me.lblAllOrderMean.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMean.TabIndex = 9
        Me.lblAllOrderMean.Text = "Label1"
        '
        'lblAllOrderMax
        '
        Me.lblAllOrderMax.AutoSize = True
        Me.lblAllOrderMax.Location = New System.Drawing.Point(57, 48)
        Me.lblAllOrderMax.Name = "lblAllOrderMax"
        Me.lblAllOrderMax.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMax.TabIndex = 8
        Me.lblAllOrderMax.Text = "Label1"
        '
        'lblAllOrderMin
        '
        Me.lblAllOrderMin.AutoSize = True
        Me.lblAllOrderMin.Location = New System.Drawing.Point(57, 35)
        Me.lblAllOrderMin.Name = "lblAllOrderMin"
        Me.lblAllOrderMin.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderMin.TabIndex = 7
        Me.lblAllOrderMin.Text = "Label1"
        '
        'lblAllOrderVol
        '
        Me.lblAllOrderVol.AutoSize = True
        Me.lblAllOrderVol.Location = New System.Drawing.Point(57, 22)
        Me.lblAllOrderVol.Name = "lblAllOrderVol"
        Me.lblAllOrderVol.Size = New System.Drawing.Size(38, 13)
        Me.lblAllOrderVol.TabIndex = 6
        Me.lblAllOrderVol.Text = "Label1"
        '
        'lblAllOrderStdLbl
        '
        Me.lblAllOrderStdLbl.AutoSize = True
        Me.lblAllOrderStdLbl.Location = New System.Drawing.Point(6, 87)
        Me.lblAllOrderStdLbl.Name = "lblAllOrderStdLbl"
        Me.lblAllOrderStdLbl.Size = New System.Drawing.Size(49, 13)
        Me.lblAllOrderStdLbl.TabIndex = 5
        Me.lblAllOrderStdLbl.Text = "Std Dev:"
        '
        'lblAllOrderMedianLbl
        '
        Me.lblAllOrderMedianLbl.AutoSize = True
        Me.lblAllOrderMedianLbl.Location = New System.Drawing.Point(6, 74)
        Me.lblAllOrderMedianLbl.Name = "lblAllOrderMedianLbl"
        Me.lblAllOrderMedianLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblAllOrderMedianLbl.TabIndex = 4
        Me.lblAllOrderMedianLbl.Text = "Median:"
        '
        'lblAllOrderMeanLbl
        '
        Me.lblAllOrderMeanLbl.AutoSize = True
        Me.lblAllOrderMeanLbl.Location = New System.Drawing.Point(6, 61)
        Me.lblAllOrderMeanLbl.Name = "lblAllOrderMeanLbl"
        Me.lblAllOrderMeanLbl.Size = New System.Drawing.Size(37, 13)
        Me.lblAllOrderMeanLbl.TabIndex = 3
        Me.lblAllOrderMeanLbl.Text = "Mean:"
        '
        'lblAllOrderMaxLbl
        '
        Me.lblAllOrderMaxLbl.AutoSize = True
        Me.lblAllOrderMaxLbl.Location = New System.Drawing.Point(6, 48)
        Me.lblAllOrderMaxLbl.Name = "lblAllOrderMaxLbl"
        Me.lblAllOrderMaxLbl.Size = New System.Drawing.Size(31, 13)
        Me.lblAllOrderMaxLbl.TabIndex = 2
        Me.lblAllOrderMaxLbl.Text = "Max:"
        '
        'lblAllOrderMinLbl
        '
        Me.lblAllOrderMinLbl.AutoSize = True
        Me.lblAllOrderMinLbl.Location = New System.Drawing.Point(6, 35)
        Me.lblAllOrderMinLbl.Name = "lblAllOrderMinLbl"
        Me.lblAllOrderMinLbl.Size = New System.Drawing.Size(27, 13)
        Me.lblAllOrderMinLbl.TabIndex = 1
        Me.lblAllOrderMinLbl.Text = "Min:"
        '
        'lblAllOrderVolLbl
        '
        Me.lblAllOrderVolLbl.AutoSize = True
        Me.lblAllOrderVolLbl.Location = New System.Drawing.Point(6, 22)
        Me.lblAllOrderVolLbl.Name = "lblAllOrderVolLbl"
        Me.lblAllOrderVolLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblAllOrderVolLbl.TabIndex = 0
        Me.lblAllOrderVolLbl.Text = "Volume:"
        '
        'gbBuyData
        '
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderStd)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMedian)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMean)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMax)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMin)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderVol)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderStdLbl)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMedianLbl)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMeanLbl)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMaxLbl)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderMinLbl)
        Me.gbBuyData.Controls.Add(Me.lblBuyOrderVolLbl)
        Me.gbBuyData.Location = New System.Drawing.Point(4, 125)
        Me.gbBuyData.Name = "gbBuyData"
        Me.gbBuyData.Size = New System.Drawing.Size(252, 114)
        Me.gbBuyData.TabIndex = 1
        Me.gbBuyData.TabStop = False
        Me.gbBuyData.Text = "Buy Orders"
        '
        'lblBuyOrderStd
        '
        Me.lblBuyOrderStd.AutoSize = True
        Me.lblBuyOrderStd.Location = New System.Drawing.Point(57, 87)
        Me.lblBuyOrderStd.Name = "lblBuyOrderStd"
        Me.lblBuyOrderStd.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderStd.TabIndex = 11
        Me.lblBuyOrderStd.Text = "Label1"
        '
        'lblBuyOrderMedian
        '
        Me.lblBuyOrderMedian.AutoSize = True
        Me.lblBuyOrderMedian.Location = New System.Drawing.Point(57, 74)
        Me.lblBuyOrderMedian.Name = "lblBuyOrderMedian"
        Me.lblBuyOrderMedian.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMedian.TabIndex = 10
        Me.lblBuyOrderMedian.Text = "Label1"
        '
        'lblBuyOrderMean
        '
        Me.lblBuyOrderMean.AutoSize = True
        Me.lblBuyOrderMean.Location = New System.Drawing.Point(57, 61)
        Me.lblBuyOrderMean.Name = "lblBuyOrderMean"
        Me.lblBuyOrderMean.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMean.TabIndex = 9
        Me.lblBuyOrderMean.Text = "Label1"
        '
        'lblBuyOrderMax
        '
        Me.lblBuyOrderMax.AutoSize = True
        Me.lblBuyOrderMax.Location = New System.Drawing.Point(57, 48)
        Me.lblBuyOrderMax.Name = "lblBuyOrderMax"
        Me.lblBuyOrderMax.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMax.TabIndex = 8
        Me.lblBuyOrderMax.Text = "Label1"
        '
        'lblBuyOrderMin
        '
        Me.lblBuyOrderMin.AutoSize = True
        Me.lblBuyOrderMin.Location = New System.Drawing.Point(57, 35)
        Me.lblBuyOrderMin.Name = "lblBuyOrderMin"
        Me.lblBuyOrderMin.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderMin.TabIndex = 7
        Me.lblBuyOrderMin.Text = "Label1"
        '
        'lblBuyOrderVol
        '
        Me.lblBuyOrderVol.AutoSize = True
        Me.lblBuyOrderVol.Location = New System.Drawing.Point(57, 22)
        Me.lblBuyOrderVol.Name = "lblBuyOrderVol"
        Me.lblBuyOrderVol.Size = New System.Drawing.Size(38, 13)
        Me.lblBuyOrderVol.TabIndex = 6
        Me.lblBuyOrderVol.Text = "Label1"
        '
        'lblBuyOrderStdLbl
        '
        Me.lblBuyOrderStdLbl.AutoSize = True
        Me.lblBuyOrderStdLbl.Location = New System.Drawing.Point(6, 87)
        Me.lblBuyOrderStdLbl.Name = "lblBuyOrderStdLbl"
        Me.lblBuyOrderStdLbl.Size = New System.Drawing.Size(49, 13)
        Me.lblBuyOrderStdLbl.TabIndex = 5
        Me.lblBuyOrderStdLbl.Text = "Std Dev:"
        '
        'lblBuyOrderMedianLbl
        '
        Me.lblBuyOrderMedianLbl.AutoSize = True
        Me.lblBuyOrderMedianLbl.Location = New System.Drawing.Point(6, 74)
        Me.lblBuyOrderMedianLbl.Name = "lblBuyOrderMedianLbl"
        Me.lblBuyOrderMedianLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblBuyOrderMedianLbl.TabIndex = 4
        Me.lblBuyOrderMedianLbl.Text = "Median:"
        '
        'lblBuyOrderMeanLbl
        '
        Me.lblBuyOrderMeanLbl.AutoSize = True
        Me.lblBuyOrderMeanLbl.Location = New System.Drawing.Point(6, 61)
        Me.lblBuyOrderMeanLbl.Name = "lblBuyOrderMeanLbl"
        Me.lblBuyOrderMeanLbl.Size = New System.Drawing.Size(37, 13)
        Me.lblBuyOrderMeanLbl.TabIndex = 3
        Me.lblBuyOrderMeanLbl.Text = "Mean:"
        '
        'lblBuyOrderMaxLbl
        '
        Me.lblBuyOrderMaxLbl.AutoSize = True
        Me.lblBuyOrderMaxLbl.Location = New System.Drawing.Point(6, 48)
        Me.lblBuyOrderMaxLbl.Name = "lblBuyOrderMaxLbl"
        Me.lblBuyOrderMaxLbl.Size = New System.Drawing.Size(31, 13)
        Me.lblBuyOrderMaxLbl.TabIndex = 2
        Me.lblBuyOrderMaxLbl.Text = "Max:"
        '
        'lblBuyOrderMinLbl
        '
        Me.lblBuyOrderMinLbl.AutoSize = True
        Me.lblBuyOrderMinLbl.Location = New System.Drawing.Point(6, 35)
        Me.lblBuyOrderMinLbl.Name = "lblBuyOrderMinLbl"
        Me.lblBuyOrderMinLbl.Size = New System.Drawing.Size(27, 13)
        Me.lblBuyOrderMinLbl.TabIndex = 1
        Me.lblBuyOrderMinLbl.Text = "Min:"
        '
        'lblBuyOrderVolLbl
        '
        Me.lblBuyOrderVolLbl.AutoSize = True
        Me.lblBuyOrderVolLbl.Location = New System.Drawing.Point(6, 22)
        Me.lblBuyOrderVolLbl.Name = "lblBuyOrderVolLbl"
        Me.lblBuyOrderVolLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblBuyOrderVolLbl.TabIndex = 0
        Me.lblBuyOrderVolLbl.Text = "Volume:"
        '
        'gbSellData
        '
        Me.gbSellData.Controls.Add(Me.lblSellOrderStd)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMedian)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMean)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMax)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMin)
        Me.gbSellData.Controls.Add(Me.LblSellOrderVol)
        Me.gbSellData.Controls.Add(Me.lblSellOrderStdLbl)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMedianLbl)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMeanLbl)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMaxLbl)
        Me.gbSellData.Controls.Add(Me.lblSellOrderMinLbl)
        Me.gbSellData.Controls.Add(Me.lblSellOrderVolLbl)
        Me.gbSellData.Location = New System.Drawing.Point(4, 5)
        Me.gbSellData.Name = "gbSellData"
        Me.gbSellData.Size = New System.Drawing.Size(252, 114)
        Me.gbSellData.TabIndex = 0
        Me.gbSellData.TabStop = False
        Me.gbSellData.Text = "Sell Orders"
        '
        'lblSellOrderStd
        '
        Me.lblSellOrderStd.AutoSize = True
        Me.lblSellOrderStd.Location = New System.Drawing.Point(57, 87)
        Me.lblSellOrderStd.Name = "lblSellOrderStd"
        Me.lblSellOrderStd.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderStd.TabIndex = 11
        Me.lblSellOrderStd.Text = "Label1"
        '
        'lblSellOrderMedian
        '
        Me.lblSellOrderMedian.AutoSize = True
        Me.lblSellOrderMedian.Location = New System.Drawing.Point(57, 74)
        Me.lblSellOrderMedian.Name = "lblSellOrderMedian"
        Me.lblSellOrderMedian.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMedian.TabIndex = 10
        Me.lblSellOrderMedian.Text = "Label1"
        '
        'lblSellOrderMean
        '
        Me.lblSellOrderMean.AutoSize = True
        Me.lblSellOrderMean.Location = New System.Drawing.Point(57, 61)
        Me.lblSellOrderMean.Name = "lblSellOrderMean"
        Me.lblSellOrderMean.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMean.TabIndex = 9
        Me.lblSellOrderMean.Text = "Label1"
        '
        'lblSellOrderMax
        '
        Me.lblSellOrderMax.AutoSize = True
        Me.lblSellOrderMax.Location = New System.Drawing.Point(57, 48)
        Me.lblSellOrderMax.Name = "lblSellOrderMax"
        Me.lblSellOrderMax.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMax.TabIndex = 8
        Me.lblSellOrderMax.Text = "Label1"
        '
        'lblSellOrderMin
        '
        Me.lblSellOrderMin.AutoSize = True
        Me.lblSellOrderMin.Location = New System.Drawing.Point(57, 35)
        Me.lblSellOrderMin.Name = "lblSellOrderMin"
        Me.lblSellOrderMin.Size = New System.Drawing.Size(38, 13)
        Me.lblSellOrderMin.TabIndex = 7
        Me.lblSellOrderMin.Text = "Label1"
        '
        'LblSellOrderVol
        '
        Me.LblSellOrderVol.AutoSize = True
        Me.LblSellOrderVol.Location = New System.Drawing.Point(57, 22)
        Me.LblSellOrderVol.Name = "LblSellOrderVol"
        Me.LblSellOrderVol.Size = New System.Drawing.Size(38, 13)
        Me.LblSellOrderVol.TabIndex = 6
        Me.LblSellOrderVol.Text = "Label1"
        '
        'lblSellOrderStdLbl
        '
        Me.lblSellOrderStdLbl.AutoSize = True
        Me.lblSellOrderStdLbl.Location = New System.Drawing.Point(6, 87)
        Me.lblSellOrderStdLbl.Name = "lblSellOrderStdLbl"
        Me.lblSellOrderStdLbl.Size = New System.Drawing.Size(49, 13)
        Me.lblSellOrderStdLbl.TabIndex = 5
        Me.lblSellOrderStdLbl.Text = "Std Dev:"
        '
        'lblSellOrderMedianLbl
        '
        Me.lblSellOrderMedianLbl.AutoSize = True
        Me.lblSellOrderMedianLbl.Location = New System.Drawing.Point(6, 74)
        Me.lblSellOrderMedianLbl.Name = "lblSellOrderMedianLbl"
        Me.lblSellOrderMedianLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblSellOrderMedianLbl.TabIndex = 4
        Me.lblSellOrderMedianLbl.Text = "Median:"
        '
        'lblSellOrderMeanLbl
        '
        Me.lblSellOrderMeanLbl.AutoSize = True
        Me.lblSellOrderMeanLbl.Location = New System.Drawing.Point(6, 61)
        Me.lblSellOrderMeanLbl.Name = "lblSellOrderMeanLbl"
        Me.lblSellOrderMeanLbl.Size = New System.Drawing.Size(37, 13)
        Me.lblSellOrderMeanLbl.TabIndex = 3
        Me.lblSellOrderMeanLbl.Text = "Mean:"
        '
        'lblSellOrderMaxLbl
        '
        Me.lblSellOrderMaxLbl.AutoSize = True
        Me.lblSellOrderMaxLbl.Location = New System.Drawing.Point(6, 48)
        Me.lblSellOrderMaxLbl.Name = "lblSellOrderMaxLbl"
        Me.lblSellOrderMaxLbl.Size = New System.Drawing.Size(31, 13)
        Me.lblSellOrderMaxLbl.TabIndex = 2
        Me.lblSellOrderMaxLbl.Text = "Max:"
        '
        'lblSellOrderMinLbl
        '
        Me.lblSellOrderMinLbl.AutoSize = True
        Me.lblSellOrderMinLbl.Location = New System.Drawing.Point(6, 35)
        Me.lblSellOrderMinLbl.Name = "lblSellOrderMinLbl"
        Me.lblSellOrderMinLbl.Size = New System.Drawing.Size(27, 13)
        Me.lblSellOrderMinLbl.TabIndex = 1
        Me.lblSellOrderMinLbl.Text = "Min:"
        '
        'lblSellOrderVolLbl
        '
        Me.lblSellOrderVolLbl.AutoSize = True
        Me.lblSellOrderVolLbl.Location = New System.Drawing.Point(6, 22)
        Me.lblSellOrderVolLbl.Name = "lblSellOrderVolLbl"
        Me.lblSellOrderVolLbl.Size = New System.Drawing.Size(45, 13)
        Me.lblSellOrderVolLbl.TabIndex = 0
        Me.lblSellOrderVolLbl.Text = "Volume:"
        '
        'zgcPrices
        '
        Me.zgcPrices.Dock = System.Windows.Forms.DockStyle.Fill
        Me.zgcPrices.Location = New System.Drawing.Point(0, 0)
        Me.zgcPrices.Name = "zgcPrices"
        Me.zgcPrices.ScrollGrace = 0
        Me.zgcPrices.ScrollMaxX = 0
        Me.zgcPrices.ScrollMaxY = 0
        Me.zgcPrices.ScrollMaxY2 = 0
        Me.zgcPrices.ScrollMinX = 0
        Me.zgcPrices.ScrollMinY = 0
        Me.zgcPrices.ScrollMinY2 = 0
        Me.zgcPrices.Size = New System.Drawing.Size(231, 233)
        Me.zgcPrices.TabIndex = 0
        '
        'Splitter1
        '
        Me.Splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Splitter1.Location = New System.Drawing.Point(746, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 664)
        Me.Splitter1.TabIndex = 1
        Me.Splitter1.TabStop = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.clvSellers)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblSellers)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.clvBuyers)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblBuyers)
        Me.SplitContainer1.Size = New System.Drawing.Size(746, 664)
        Me.SplitContainer1.SplitterDistance = 333
        Me.SplitContainer1.TabIndex = 2
        '
        'clvSellers
        '
        Me.clvSellers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvSellers.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colLocation, Me.colQuantity, Me.colPrice, Me.colExpires})
        Me.clvSellers.DefaultItemHeight = 18
        Me.clvSellers.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvSellers.Location = New System.Drawing.Point(4, 21)
        Me.clvSellers.Name = "clvSellers"
        Me.clvSellers.Size = New System.Drawing.Size(735, 305)
        Me.clvSellers.TabIndex = 1
        '
        'colLocation
        '
        Me.colLocation.CustomSortTag = Nothing
        Me.colLocation.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colLocation.Tag = Nothing
        Me.colLocation.Text = "Location"
        Me.colLocation.Width = 300
        '
        'colQuantity
        '
        Me.colQuantity.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colQuantity.CustomSortTag = Nothing
        Me.colQuantity.DisplayIndex = 1
        Me.colQuantity.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colQuantity.Tag = Nothing
        Me.colQuantity.Text = "Quantity"
        Me.colQuantity.Width = 125
        '
        'colPrice
        '
        Me.colPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colPrice.CustomSortTag = Nothing
        Me.colPrice.DisplayIndex = 2
        Me.colPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colPrice.Tag = Nothing
        Me.colPrice.Text = "Price"
        Me.colPrice.Width = 125
        '
        'colExpires
        '
        Me.colExpires.CustomSortTag = Nothing
        Me.colExpires.DisplayIndex = 3
        Me.colExpires.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colExpires.Tag = Nothing
        Me.colExpires.Text = "Expires In"
        Me.colExpires.Width = 125
        '
        'lblSellers
        '
        Me.lblSellers.AutoSize = True
        Me.lblSellers.Font = New System.Drawing.Font("Tahoma", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSellers.Location = New System.Drawing.Point(5, 5)
        Me.lblSellers.Name = "lblSellers"
        Me.lblSellers.Size = New System.Drawing.Size(53, 13)
        Me.lblSellers.TabIndex = 0
        Me.lblSellers.Text = "SELLERS"
        '
        'clvBuyers
        '
        Me.clvBuyers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvBuyers.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.ContainerListViewColumnHeader1, Me.ContainerListViewColumnHeader2, Me.ContainerListViewColumnHeader3, Me.ContainerListViewColumnHeader4})
        Me.clvBuyers.DefaultItemHeight = 18
        Me.clvBuyers.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clvBuyers.Location = New System.Drawing.Point(4, 21)
        Me.clvBuyers.Name = "clvBuyers"
        Me.clvBuyers.Size = New System.Drawing.Size(735, 299)
        Me.clvBuyers.TabIndex = 4
        '
        'ContainerListViewColumnHeader1
        '
        Me.ContainerListViewColumnHeader1.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader1.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.ContainerListViewColumnHeader1.Tag = Nothing
        Me.ContainerListViewColumnHeader1.Text = "Location"
        Me.ContainerListViewColumnHeader1.Width = 300
        '
        'ContainerListViewColumnHeader2
        '
        Me.ContainerListViewColumnHeader2.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader2.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader2.DisplayIndex = 1
        Me.ContainerListViewColumnHeader2.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader2.Tag = Nothing
        Me.ContainerListViewColumnHeader2.Text = "Quantity"
        Me.ContainerListViewColumnHeader2.Width = 125
        '
        'ContainerListViewColumnHeader3
        '
        Me.ContainerListViewColumnHeader3.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ContainerListViewColumnHeader3.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader3.DisplayIndex = 2
        Me.ContainerListViewColumnHeader3.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.ContainerListViewColumnHeader3.Tag = Nothing
        Me.ContainerListViewColumnHeader3.Text = "Price"
        Me.ContainerListViewColumnHeader3.Width = 125
        '
        'ContainerListViewColumnHeader4
        '
        Me.ContainerListViewColumnHeader4.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader4.DisplayIndex = 3
        Me.ContainerListViewColumnHeader4.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.ContainerListViewColumnHeader4.Tag = Nothing
        Me.ContainerListViewColumnHeader4.Text = "Expires In"
        Me.ContainerListViewColumnHeader4.Width = 125
        '
        'lblBuyers
        '
        Me.lblBuyers.AutoSize = True
        Me.lblBuyers.Font = New System.Drawing.Font("Tahoma", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBuyers.Location = New System.Drawing.Point(5, 5)
        Me.lblBuyers.Name = "lblBuyers"
        Me.lblBuyers.Size = New System.Drawing.Size(50, 13)
        Me.lblBuyers.TabIndex = 3
        Me.lblBuyers.Text = "BUYERS"
        '
        'frmMarketOrders
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 664)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.scOrderData)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketOrders"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Market Orders"
        Me.scOrderData.Panel1.ResumeLayout(False)
        Me.scOrderData.Panel1.PerformLayout()
        Me.scOrderData.Panel2.ResumeLayout(False)
        Me.scOrderData.ResumeLayout(False)
        Me.gbAllData.ResumeLayout(False)
        Me.gbAllData.PerformLayout()
        Me.gbBuyData.ResumeLayout(False)
        Me.gbBuyData.PerformLayout()
        Me.gbSellData.ResumeLayout(False)
        Me.gbSellData.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents scOrderData As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents lblSellers As System.Windows.Forms.Label
    Friend WithEvents lblBuyers As System.Windows.Forms.Label
    Friend WithEvents clvSellers As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colQuantity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colExpires As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colLocation As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents clvBuyers As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents ContainerListViewColumnHeader1 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader2 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader3 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader4 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents gbSellData As System.Windows.Forms.GroupBox
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
    Friend WithEvents gbAllData As System.Windows.Forms.GroupBox
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
    Friend WithEvents gbBuyData As System.Windows.Forms.GroupBox
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
    Friend WithEvents btnAddPricesToData As System.Windows.Forms.Button
    Friend WithEvents btnSetPrice As System.Windows.Forms.Button
End Class
