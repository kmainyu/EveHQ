<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBPOreCalculator
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
        Me.clvOwnedOre = New System.Windows.Forms.ListView
        Me.colOROwnedOre = New System.Windows.Forms.ColumnHeader
        Me.colORAmount = New System.Windows.Forms.ColumnHeader
        Me.colORVolume = New System.Windows.Forms.ColumnHeader
        Me.colORSystem = New System.Windows.Forms.ColumnHeader
        Me.colORStation = New System.Windows.Forms.ColumnHeader
        Me.colOROwner = New System.Windows.Forms.ColumnHeader
        Me.clvExtraOre = New System.Windows.Forms.ListView
        Me.colEOExtraOre = New System.Windows.Forms.ColumnHeader
        Me.colEOOreUnits = New System.Windows.Forms.ColumnHeader
        Me.colEOOreVolume = New System.Windows.Forms.ColumnHeader
        Me.gbxOreTypes = New System.Windows.Forms.GroupBox
        Me.chkMercoxit = New System.Windows.Forms.CheckBox
        Me.chkArkonor = New System.Windows.Forms.CheckBox
        Me.chkBistot = New System.Windows.Forms.CheckBox
        Me.chkCrokite = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkDarkOchre = New System.Windows.Forms.CheckBox
        Me.chkSpodumain = New System.Windows.Forms.CheckBox
        Me.chkHerbergite = New System.Windows.Forms.CheckBox
        Me.chkGneiss = New System.Windows.Forms.CheckBox
        Me.chkHemorphite = New System.Windows.Forms.CheckBox
        Me.chkJaspet = New System.Windows.Forms.CheckBox
        Me.chkKernite = New System.Windows.Forms.CheckBox
        Me.chkOmber = New System.Windows.Forms.CheckBox
        Me.chkPyroxeres = New System.Windows.Forms.CheckBox
        Me.chkPlagioclase = New System.Windows.Forms.CheckBox
        Me.chkScordite = New System.Windows.Forms.CheckBox
        Me.chkVeldspar = New System.Windows.Forms.CheckBox
        Me.chkMineAll = New System.Windows.Forms.CheckBox
        Me.clvNeededVsMined = New System.Windows.Forms.ListView
        Me.colNvMNeededMinerals = New System.Windows.Forms.ColumnHeader
        Me.colNvMMineralsUnits1 = New System.Windows.Forms.ColumnHeader
        Me.colNvMMinedMinerals = New System.Windows.Forms.ColumnHeader
        Me.colNvMMineralsUnits2 = New System.Windows.Forms.ColumnHeader
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkMineExtraOre = New System.Windows.Forms.CheckBox
        Me.lblDurationMining = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSaveAndExit = New System.Windows.Forms.Button
        Me.btnCancelAndExit = New System.Windows.Forms.Button
        Me.gbxOreTypes.SuspendLayout()
        Me.SuspendLayout()
        '
        'clvOwnedOre
        '
        Me.clvOwnedOre.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colOROwnedOre, Me.colORAmount, Me.colORVolume, Me.colORSystem, Me.colORStation, Me.colOROwner})
        Me.clvOwnedOre.GridLines = True
        Me.clvOwnedOre.Location = New System.Drawing.Point(12, 12)
        Me.clvOwnedOre.Name = "clvOwnedOre"
        Me.clvOwnedOre.Size = New System.Drawing.Size(760, 132)
        Me.clvOwnedOre.TabIndex = 3
        Me.clvOwnedOre.UseCompatibleStateImageBehavior = False
        Me.clvOwnedOre.View = System.Windows.Forms.View.Details
        '
        'colOROwnedOre
        '
        Me.colOROwnedOre.Text = "Owned ore"
        Me.colOROwnedOre.Width = 200
        '
        'colORAmount
        '
        Me.colORAmount.Text = "# of units"
        Me.colORAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colORAmount.Width = 75
        '
        'colORVolume
        '
        Me.colORVolume.Text = "Volume"
        Me.colORVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colORVolume.Width = 100
        '
        'colORSystem
        '
        Me.colORSystem.Text = "System"
        Me.colORSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colORSystem.Width = 100
        '
        'colORStation
        '
        Me.colORStation.Text = "Station"
        Me.colORStation.Width = 200
        '
        'colOROwner
        '
        Me.colOROwner.Text = "Owner"
        Me.colOROwner.Width = 75
        '
        'clvExtraOre
        '
        Me.clvExtraOre.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colEOExtraOre, Me.colEOOreUnits, Me.colEOOreVolume})
        Me.clvExtraOre.GridLines = True
        Me.clvExtraOre.Location = New System.Drawing.Point(358, 150)
        Me.clvExtraOre.Name = "clvExtraOre"
        Me.clvExtraOre.Size = New System.Drawing.Size(414, 132)
        Me.clvExtraOre.TabIndex = 4
        Me.clvExtraOre.UseCompatibleStateImageBehavior = False
        Me.clvExtraOre.View = System.Windows.Forms.View.Details
        '
        'colEOExtraOre
        '
        Me.colEOExtraOre.Text = "Extra ore needed"
        Me.colEOExtraOre.Width = 225
        '
        'colEOOreUnits
        '
        Me.colEOOreUnits.Text = "# of units"
        Me.colEOOreUnits.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colEOOreUnits.Width = 75
        '
        'colEOOreVolume
        '
        Me.colEOOreVolume.Text = "Volume"
        Me.colEOOreVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colEOOreVolume.Width = 100
        '
        'gbxOreTypes
        '
        Me.gbxOreTypes.Controls.Add(Me.chkMercoxit)
        Me.gbxOreTypes.Controls.Add(Me.chkArkonor)
        Me.gbxOreTypes.Controls.Add(Me.chkBistot)
        Me.gbxOreTypes.Controls.Add(Me.chkCrokite)
        Me.gbxOreTypes.Controls.Add(Me.Label1)
        Me.gbxOreTypes.Controls.Add(Me.chkDarkOchre)
        Me.gbxOreTypes.Controls.Add(Me.chkSpodumain)
        Me.gbxOreTypes.Controls.Add(Me.chkHerbergite)
        Me.gbxOreTypes.Controls.Add(Me.chkGneiss)
        Me.gbxOreTypes.Controls.Add(Me.chkHemorphite)
        Me.gbxOreTypes.Controls.Add(Me.chkJaspet)
        Me.gbxOreTypes.Controls.Add(Me.chkKernite)
        Me.gbxOreTypes.Controls.Add(Me.chkOmber)
        Me.gbxOreTypes.Controls.Add(Me.chkPyroxeres)
        Me.gbxOreTypes.Controls.Add(Me.chkPlagioclase)
        Me.gbxOreTypes.Controls.Add(Me.chkScordite)
        Me.gbxOreTypes.Controls.Add(Me.chkVeldspar)
        Me.gbxOreTypes.Location = New System.Drawing.Point(12, 173)
        Me.gbxOreTypes.Name = "gbxOreTypes"
        Me.gbxOreTypes.Size = New System.Drawing.Size(340, 141)
        Me.gbxOreTypes.TabIndex = 5
        Me.gbxOreTypes.TabStop = False
        Me.gbxOreTypes.Text = "Ore types"
        '
        'chkMercoxit
        '
        Me.chkMercoxit.AutoSize = True
        Me.chkMercoxit.Location = New System.Drawing.Point(267, 109)
        Me.chkMercoxit.Name = "chkMercoxit"
        Me.chkMercoxit.Size = New System.Drawing.Size(67, 17)
        Me.chkMercoxit.TabIndex = 16
        Me.chkMercoxit.Text = "Mercoxit"
        Me.chkMercoxit.UseVisualStyleBackColor = True
        '
        'chkArkonor
        '
        Me.chkArkonor.AutoSize = True
        Me.chkArkonor.Location = New System.Drawing.Point(267, 86)
        Me.chkArkonor.Name = "chkArkonor"
        Me.chkArkonor.Size = New System.Drawing.Size(64, 17)
        Me.chkArkonor.TabIndex = 15
        Me.chkArkonor.Text = "Arkonor"
        Me.chkArkonor.UseVisualStyleBackColor = True
        '
        'chkBistot
        '
        Me.chkBistot.AutoSize = True
        Me.chkBistot.Location = New System.Drawing.Point(267, 63)
        Me.chkBistot.Name = "chkBistot"
        Me.chkBistot.Size = New System.Drawing.Size(53, 17)
        Me.chkBistot.TabIndex = 14
        Me.chkBistot.Text = "Bistot"
        Me.chkBistot.UseVisualStyleBackColor = True
        '
        'chkCrokite
        '
        Me.chkCrokite.AutoSize = True
        Me.chkCrokite.Location = New System.Drawing.Point(267, 40)
        Me.chkCrokite.Name = "chkCrokite"
        Me.chkCrokite.Size = New System.Drawing.Size(60, 17)
        Me.chkCrokite.TabIndex = 13
        Me.chkCrokite.Text = "Crokite"
        Me.chkCrokite.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(204, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Check the ore types you have access to:"
        '
        'chkDarkOchre
        '
        Me.chkDarkOchre.AutoSize = True
        Me.chkDarkOchre.Location = New System.Drawing.Point(180, 109)
        Me.chkDarkOchre.Name = "chkDarkOchre"
        Me.chkDarkOchre.Size = New System.Drawing.Size(80, 17)
        Me.chkDarkOchre.TabIndex = 11
        Me.chkDarkOchre.Text = "Dark Ochre"
        Me.chkDarkOchre.UseVisualStyleBackColor = True
        '
        'chkSpodumain
        '
        Me.chkSpodumain.AutoSize = True
        Me.chkSpodumain.Location = New System.Drawing.Point(180, 86)
        Me.chkSpodumain.Name = "chkSpodumain"
        Me.chkSpodumain.Size = New System.Drawing.Size(78, 17)
        Me.chkSpodumain.TabIndex = 10
        Me.chkSpodumain.Text = "Spodumain"
        Me.chkSpodumain.UseVisualStyleBackColor = True
        '
        'chkHerbergite
        '
        Me.chkHerbergite.AutoSize = True
        Me.chkHerbergite.Location = New System.Drawing.Point(180, 63)
        Me.chkHerbergite.Name = "chkHerbergite"
        Me.chkHerbergite.Size = New System.Drawing.Size(77, 17)
        Me.chkHerbergite.TabIndex = 9
        Me.chkHerbergite.Text = "Herbergite"
        Me.chkHerbergite.UseVisualStyleBackColor = True
        '
        'chkGneiss
        '
        Me.chkGneiss.AutoSize = True
        Me.chkGneiss.Location = New System.Drawing.Point(180, 40)
        Me.chkGneiss.Name = "chkGneiss"
        Me.chkGneiss.Size = New System.Drawing.Size(57, 17)
        Me.chkGneiss.TabIndex = 8
        Me.chkGneiss.Text = "Gneiss"
        Me.chkGneiss.UseVisualStyleBackColor = True
        '
        'chkHemorphite
        '
        Me.chkHemorphite.AutoSize = True
        Me.chkHemorphite.Location = New System.Drawing.Point(93, 109)
        Me.chkHemorphite.Name = "chkHemorphite"
        Me.chkHemorphite.Size = New System.Drawing.Size(81, 17)
        Me.chkHemorphite.TabIndex = 7
        Me.chkHemorphite.Text = "Hemorphite"
        Me.chkHemorphite.UseVisualStyleBackColor = True
        '
        'chkJaspet
        '
        Me.chkJaspet.AutoSize = True
        Me.chkJaspet.Location = New System.Drawing.Point(93, 86)
        Me.chkJaspet.Name = "chkJaspet"
        Me.chkJaspet.Size = New System.Drawing.Size(58, 17)
        Me.chkJaspet.TabIndex = 6
        Me.chkJaspet.Text = "Jaspet"
        Me.chkJaspet.UseVisualStyleBackColor = True
        '
        'chkKernite
        '
        Me.chkKernite.AutoSize = True
        Me.chkKernite.Location = New System.Drawing.Point(93, 63)
        Me.chkKernite.Name = "chkKernite"
        Me.chkKernite.Size = New System.Drawing.Size(60, 17)
        Me.chkKernite.TabIndex = 5
        Me.chkKernite.Text = "Kernite"
        Me.chkKernite.UseVisualStyleBackColor = True
        '
        'chkOmber
        '
        Me.chkOmber.AutoSize = True
        Me.chkOmber.Location = New System.Drawing.Point(93, 40)
        Me.chkOmber.Name = "chkOmber"
        Me.chkOmber.Size = New System.Drawing.Size(58, 17)
        Me.chkOmber.TabIndex = 4
        Me.chkOmber.Text = "Omber"
        Me.chkOmber.UseVisualStyleBackColor = True
        '
        'chkPyroxeres
        '
        Me.chkPyroxeres.AutoSize = True
        Me.chkPyroxeres.Location = New System.Drawing.Point(6, 109)
        Me.chkPyroxeres.Name = "chkPyroxeres"
        Me.chkPyroxeres.Size = New System.Drawing.Size(75, 17)
        Me.chkPyroxeres.TabIndex = 3
        Me.chkPyroxeres.Text = "Pyroxeres"
        Me.chkPyroxeres.UseVisualStyleBackColor = True
        '
        'chkPlagioclase
        '
        Me.chkPlagioclase.AutoSize = True
        Me.chkPlagioclase.Location = New System.Drawing.Point(6, 86)
        Me.chkPlagioclase.Name = "chkPlagioclase"
        Me.chkPlagioclase.Size = New System.Drawing.Size(78, 17)
        Me.chkPlagioclase.TabIndex = 2
        Me.chkPlagioclase.Text = "Plagioclase"
        Me.chkPlagioclase.UseVisualStyleBackColor = True
        '
        'chkScordite
        '
        Me.chkScordite.AutoSize = True
        Me.chkScordite.Location = New System.Drawing.Point(6, 63)
        Me.chkScordite.Name = "chkScordite"
        Me.chkScordite.Size = New System.Drawing.Size(65, 17)
        Me.chkScordite.TabIndex = 1
        Me.chkScordite.Text = "Scordite"
        Me.chkScordite.UseVisualStyleBackColor = True
        '
        'chkVeldspar
        '
        Me.chkVeldspar.AutoSize = True
        Me.chkVeldspar.Location = New System.Drawing.Point(6, 40)
        Me.chkVeldspar.Name = "chkVeldspar"
        Me.chkVeldspar.Size = New System.Drawing.Size(67, 17)
        Me.chkVeldspar.TabIndex = 0
        Me.chkVeldspar.Text = "Veldspar"
        Me.chkVeldspar.UseVisualStyleBackColor = True
        '
        'chkMineAll
        '
        Me.chkMineAll.AutoSize = True
        Me.chkMineAll.Location = New System.Drawing.Point(18, 320)
        Me.chkMineAll.Name = "chkMineAll"
        Me.chkMineAll.Size = New System.Drawing.Size(317, 17)
        Me.chkMineAll.TabIndex = 6
        Me.chkMineAll.Text = "I want to mine all the needed ore, without touching my stock"
        Me.chkMineAll.UseVisualStyleBackColor = True
        '
        'clvNeededVsMined
        '
        Me.clvNeededVsMined.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNvMNeededMinerals, Me.colNvMMineralsUnits1, Me.colNvMMinedMinerals, Me.colNvMMineralsUnits2})
        Me.clvNeededVsMined.GridLines = True
        Me.clvNeededVsMined.Location = New System.Drawing.Point(358, 305)
        Me.clvNeededVsMined.Name = "clvNeededVsMined"
        Me.clvNeededVsMined.Size = New System.Drawing.Size(414, 132)
        Me.clvNeededVsMined.TabIndex = 7
        Me.clvNeededVsMined.UseCompatibleStateImageBehavior = False
        Me.clvNeededVsMined.View = System.Windows.Forms.View.Details
        '
        'colNvMNeededMinerals
        '
        Me.colNvMNeededMinerals.Text = "Minerals needed"
        Me.colNvMNeededMinerals.Width = 125
        '
        'colNvMMineralsUnits1
        '
        Me.colNvMMineralsUnits1.Text = "# of units"
        Me.colNvMMineralsUnits1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colNvMMineralsUnits1.Width = 75
        '
        'colNvMMinedMinerals
        '
        Me.colNvMMinedMinerals.Text = "Owned/Mined Minerals"
        Me.colNvMMinedMinerals.Width = 125
        '
        'colNvMMineralsUnits2
        '
        Me.colNvMMineralsUnits2.Text = "# of units"
        Me.colNvMMineralsUnits2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colNvMMineralsUnits2.Width = 75
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(355, 289)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(174, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "When I refine the ore I (will) have:"
        '
        'chkMineExtraOre
        '
        Me.chkMineExtraOre.AutoSize = True
        Me.chkMineExtraOre.Location = New System.Drawing.Point(18, 150)
        Me.chkMineExtraOre.Name = "chkMineExtraOre"
        Me.chkMineExtraOre.Size = New System.Drawing.Size(192, 17)
        Me.chkMineExtraOre.TabIndex = 9
        Me.chkMineExtraOre.Text = "Tell me how much ore I need extra"
        Me.chkMineExtraOre.UseVisualStyleBackColor = True
        '
        'lblDurationMining
        '
        Me.lblDurationMining.AutoSize = True
        Me.lblDurationMining.Location = New System.Drawing.Point(15, 354)
        Me.lblDurationMining.Name = "lblDurationMining"
        Me.lblDurationMining.Size = New System.Drawing.Size(185, 13)
        Me.lblDurationMining.TabIndex = 10
        Me.lblDurationMining.Text = "The extra time you'll need for mining:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(199, 354)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(83, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "0D 0H 0M 0S"
        '
        'btnSaveAndExit
        '
        Me.btnSaveAndExit.Location = New System.Drawing.Point(12, 414)
        Me.btnSaveAndExit.Name = "btnSaveAndExit"
        Me.btnSaveAndExit.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveAndExit.TabIndex = 12
        Me.btnSaveAndExit.Text = "Save && Exit"
        Me.btnSaveAndExit.UseVisualStyleBackColor = True
        '
        'btnCancelAndExit
        '
        Me.btnCancelAndExit.Location = New System.Drawing.Point(105, 414)
        Me.btnCancelAndExit.Name = "btnCancelAndExit"
        Me.btnCancelAndExit.Size = New System.Drawing.Size(80, 23)
        Me.btnCancelAndExit.TabIndex = 13
        Me.btnCancelAndExit.Text = "Cancel && Exit"
        Me.btnCancelAndExit.UseVisualStyleBackColor = True
        '
        'frmBPOreCalculator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 449)
        Me.Controls.Add(Me.gbxOreTypes)
        Me.Controls.Add(Me.btnCancelAndExit)
        Me.Controls.Add(Me.btnSaveAndExit)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblDurationMining)
        Me.Controls.Add(Me.chkMineExtraOre)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.clvNeededVsMined)
        Me.Controls.Add(Me.chkMineAll)
        Me.Controls.Add(Me.clvExtraOre)
        Me.Controls.Add(Me.clvOwnedOre)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.Name = "frmBPOreCalculator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BPO Ore Calculator"
        Me.gbxOreTypes.ResumeLayout(False)
        Me.gbxOreTypes.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents clvOwnedOre As System.Windows.Forms.ListView
    Friend WithEvents colOROwnedOre As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORAmount As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORVolume As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORSystem As System.Windows.Forms.ColumnHeader
    Friend WithEvents colORStation As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOROwner As System.Windows.Forms.ColumnHeader
    Friend WithEvents clvExtraOre As System.Windows.Forms.ListView
    Friend WithEvents colEOExtraOre As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEOOreUnits As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEOOreVolume As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbxOreTypes As System.Windows.Forms.GroupBox
    Friend WithEvents chkHemorphite As System.Windows.Forms.CheckBox
    Friend WithEvents chkJaspet As System.Windows.Forms.CheckBox
    Friend WithEvents chkKernite As System.Windows.Forms.CheckBox
    Friend WithEvents chkOmber As System.Windows.Forms.CheckBox
    Friend WithEvents chkPyroxeres As System.Windows.Forms.CheckBox
    Friend WithEvents chkPlagioclase As System.Windows.Forms.CheckBox
    Friend WithEvents chkScordite As System.Windows.Forms.CheckBox
    Friend WithEvents chkVeldspar As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkDarkOchre As System.Windows.Forms.CheckBox
    Friend WithEvents chkSpodumain As System.Windows.Forms.CheckBox
    Friend WithEvents chkHerbergite As System.Windows.Forms.CheckBox
    Friend WithEvents chkGneiss As System.Windows.Forms.CheckBox
    Friend WithEvents chkCrokite As System.Windows.Forms.CheckBox
    Friend WithEvents chkMineAll As System.Windows.Forms.CheckBox
    Friend WithEvents clvNeededVsMined As System.Windows.Forms.ListView
    Friend WithEvents colNvMNeededMinerals As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNvMMineralsUnits1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNvMMinedMinerals As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNvMMineralsUnits2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkMineExtraOre As System.Windows.Forms.CheckBox
    Friend WithEvents lblDurationMining As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSaveAndExit As System.Windows.Forms.Button
    Friend WithEvents btnCancelAndExit As System.Windows.Forms.Button
    Friend WithEvents chkMercoxit As System.Windows.Forms.CheckBox
    Friend WithEvents chkArkonor As System.Windows.Forms.CheckBox
    Friend WithEvents chkBistot As System.Windows.Forms.CheckBox
End Class
