namespace EveHQ.PosManager
{
    partial class TowerReactMod
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ReactionMineral = new System.Windows.Forms.PictureBox();
            this.CM_MinReactSel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.input1 = new System.Windows.Forms.PictureBox();
            this.cm_ClearLink = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsm_ClearLink = new System.Windows.Forms.ToolStripMenuItem();
            this.input2 = new System.Windows.Forms.PictureBox();
            this.input4 = new System.Windows.Forms.PictureBox();
            this.input3 = new System.Windows.Forms.PictureBox();
            this.input6 = new System.Windows.Forms.PictureBox();
            this.input5 = new System.Windows.Forms.PictureBox();
            this.output1 = new System.Windows.Forms.PictureBox();
            this.output2 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.l_ModuleName = new DevComponents.DotNetBar.LabelX();
            this.pb_FillLevel = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.l_RunTime = new DevComponents.DotNetBar.LabelX();
            this.l_ExtraInfo = new DevComponents.DotNetBar.LabelX();
            this.b_SetFull = new DevComponents.DotNetBar.ButtonX();
            this.b_SetEmpty = new DevComponents.DotNetBar.ButtonX();
            this.b_SetFill = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.ReactionMineral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input1)).BeginInit();
            this.cm_ClearLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.input2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.output1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.output2)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReactionMineral
            // 
            this.ReactionMineral.BackColor = System.Drawing.SystemColors.Control;
            this.ReactionMineral.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReactionMineral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReactionMineral.ContextMenuStrip = this.CM_MinReactSel;
            this.ReactionMineral.Location = new System.Drawing.Point(132, 2);
            this.ReactionMineral.Name = "ReactionMineral";
            this.ReactionMineral.Size = new System.Drawing.Size(55, 55);
            this.ReactionMineral.TabIndex = 0;
            this.ReactionMineral.TabStop = false;
            this.toolTip1.SetToolTip(this.ReactionMineral, "Right Click to Select a Reaction or Mineral");
            this.ReactionMineral.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ReactionMineral_MouseClick);
            // 
            // CM_MinReactSel
            // 
            this.CM_MinReactSel.Name = "CM_MinReactSel";
            this.CM_MinReactSel.Size = new System.Drawing.Size(61, 4);
            this.CM_MinReactSel.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.CM_MinReactSel_ItemClicked);
            // 
            // input1
            // 
            this.input1.BackColor = System.Drawing.SystemColors.Control;
            this.input1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.input1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input1.ContextMenuStrip = this.cm_ClearLink;
            this.input1.Location = new System.Drawing.Point(3, 1);
            this.input1.Name = "input1";
            this.input1.Size = new System.Drawing.Size(40, 40);
            this.input1.TabIndex = 10;
            this.input1.TabStop = false;
            this.input1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.input_MouseClick);
            // 
            // cm_ClearLink
            // 
            this.cm_ClearLink.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_ClearLink});
            this.cm_ClearLink.Name = "cm_ClearLink";
            this.cm_ClearLink.Size = new System.Drawing.Size(127, 26);
            this.cm_ClearLink.Opening += new System.ComponentModel.CancelEventHandler(this.cm_ClearLink_Opening);
            // 
            // tsm_ClearLink
            // 
            this.tsm_ClearLink.Name = "tsm_ClearLink";
            this.tsm_ClearLink.Size = new System.Drawing.Size(126, 22);
            this.tsm_ClearLink.Text = "Clear Link";
            this.tsm_ClearLink.Click += new System.EventHandler(this.tsm_ClearLink_Click);
            // 
            // input2
            // 
            this.input2.BackColor = System.Drawing.SystemColors.Control;
            this.input2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.input2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input2.ContextMenuStrip = this.cm_ClearLink;
            this.input2.Location = new System.Drawing.Point(3, 42);
            this.input2.Name = "input2";
            this.input2.Size = new System.Drawing.Size(40, 40);
            this.input2.TabIndex = 11;
            this.input2.TabStop = false;
            this.input2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.input_MouseClick);
            // 
            // input4
            // 
            this.input4.BackColor = System.Drawing.SystemColors.Control;
            this.input4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.input4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input4.ContextMenuStrip = this.cm_ClearLink;
            this.input4.Location = new System.Drawing.Point(46, 42);
            this.input4.Name = "input4";
            this.input4.Size = new System.Drawing.Size(40, 40);
            this.input4.TabIndex = 13;
            this.input4.TabStop = false;
            this.input4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.input_MouseClick);
            // 
            // input3
            // 
            this.input3.BackColor = System.Drawing.SystemColors.Control;
            this.input3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.input3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input3.ContextMenuStrip = this.cm_ClearLink;
            this.input3.Location = new System.Drawing.Point(46, 1);
            this.input3.Name = "input3";
            this.input3.Size = new System.Drawing.Size(40, 40);
            this.input3.TabIndex = 12;
            this.input3.TabStop = false;
            this.input3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.input_MouseClick);
            // 
            // input6
            // 
            this.input6.BackColor = System.Drawing.SystemColors.Control;
            this.input6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.input6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input6.ContextMenuStrip = this.cm_ClearLink;
            this.input6.Location = new System.Drawing.Point(89, 42);
            this.input6.Name = "input6";
            this.input6.Size = new System.Drawing.Size(40, 40);
            this.input6.TabIndex = 15;
            this.input6.TabStop = false;
            this.input6.MouseClick += new System.Windows.Forms.MouseEventHandler(this.input_MouseClick);
            // 
            // input5
            // 
            this.input5.BackColor = System.Drawing.SystemColors.Control;
            this.input5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.input5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.input5.ContextMenuStrip = this.cm_ClearLink;
            this.input5.Location = new System.Drawing.Point(89, 1);
            this.input5.Name = "input5";
            this.input5.Size = new System.Drawing.Size(40, 40);
            this.input5.TabIndex = 14;
            this.input5.TabStop = false;
            this.input5.MouseClick += new System.Windows.Forms.MouseEventHandler(this.input_MouseClick);
            // 
            // output1
            // 
            this.output1.BackColor = System.Drawing.SystemColors.Control;
            this.output1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.output1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.output1.ContextMenuStrip = this.cm_ClearLink;
            this.output1.Location = new System.Drawing.Point(512, 1);
            this.output1.Name = "output1";
            this.output1.Size = new System.Drawing.Size(40, 40);
            this.output1.TabIndex = 17;
            this.output1.TabStop = false;
            this.output1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.output_MouseClick);
            // 
            // output2
            // 
            this.output2.BackColor = System.Drawing.SystemColors.Control;
            this.output2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.output2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.output2.ContextMenuStrip = this.cm_ClearLink;
            this.output2.Location = new System.Drawing.Point(512, 42);
            this.output2.Name = "output2";
            this.output2.Size = new System.Drawing.Size(40, 40);
            this.output2.TabIndex = 16;
            this.output2.TabStop = false;
            this.output2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.output_MouseClick);
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.l_ModuleName);
            this.groupPanel1.Controls.Add(this.pb_FillLevel);
            this.groupPanel1.Controls.Add(this.l_RunTime);
            this.groupPanel1.Controls.Add(this.l_ExtraInfo);
            this.groupPanel1.Controls.Add(this.b_SetFull);
            this.groupPanel1.Controls.Add(this.b_SetEmpty);
            this.groupPanel1.Controls.Add(this.b_SetFill);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(555, 86);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = "";
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = "";
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = "";
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 28;
            // 
            // l_ModuleName
            // 
            // 
            // 
            // 
            this.l_ModuleName.BackgroundStyle.Class = "";
            this.l_ModuleName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.l_ModuleName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_ModuleName.ForeColor = System.Drawing.Color.Navy;
            this.l_ModuleName.Location = new System.Drawing.Point(128, 53);
            this.l_ModuleName.Name = "l_ModuleName";
            this.l_ModuleName.Size = new System.Drawing.Size(189, 25);
            this.l_ModuleName.TabIndex = 34;
            this.l_ModuleName.Text = "Module Name";
            this.l_ModuleName.TextLineAlignment = System.Drawing.StringAlignment.Near;
            this.l_ModuleName.WordWrap = true;
            // 
            // pb_FillLevel
            // 
            this.pb_FillLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.pb_FillLevel.BackgroundStyle.Class = "";
            this.pb_FillLevel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.pb_FillLevel.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.pb_FillLevel.BackgroundStyle.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pb_FillLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pb_FillLevel.Location = new System.Drawing.Point(186, 36);
            this.pb_FillLevel.Name = "pb_FillLevel";
            this.pb_FillLevel.Size = new System.Drawing.Size(319, 18);
            this.pb_FillLevel.TabIndex = 338;
            this.pb_FillLevel.TextVisible = true;
            // 
            // l_RunTime
            // 
            // 
            // 
            // 
            this.l_RunTime.BackgroundStyle.Class = "";
            this.l_RunTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.l_RunTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_RunTime.ForeColor = System.Drawing.Color.Green;
            this.l_RunTime.Location = new System.Drawing.Point(398, -1);
            this.l_RunTime.Name = "l_RunTime";
            this.l_RunTime.Size = new System.Drawing.Size(110, 34);
            this.l_RunTime.TabIndex = 33;
            this.l_RunTime.Text = "Run Time";
            this.l_RunTime.TextLineAlignment = System.Drawing.StringAlignment.Near;
            this.l_RunTime.WordWrap = true;
            // 
            // l_ExtraInfo
            // 
            // 
            // 
            // 
            this.l_ExtraInfo.BackgroundStyle.Class = "";
            this.l_ExtraInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.l_ExtraInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_ExtraInfo.ForeColor = System.Drawing.Color.Navy;
            this.l_ExtraInfo.Location = new System.Drawing.Point(190, -1);
            this.l_ExtraInfo.Name = "l_ExtraInfo";
            this.l_ExtraInfo.Size = new System.Drawing.Size(205, 34);
            this.l_ExtraInfo.TabIndex = 32;
            this.l_ExtraInfo.Text = "EaxtraInfo";
            this.l_ExtraInfo.TextLineAlignment = System.Drawing.StringAlignment.Near;
            this.l_ExtraInfo.WordWrap = true;
            // 
            // b_SetFull
            // 
            this.b_SetFull.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_SetFull.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_SetFull.Location = new System.Drawing.Point(319, 56);
            this.b_SetFull.Name = "b_SetFull";
            this.b_SetFull.Size = new System.Drawing.Size(56, 21);
            this.b_SetFull.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_SetFull.TabIndex = 29;
            this.b_SetFull.Text = "Set Full";
            this.b_SetFull.Click += new System.EventHandler(this.b_SetFull_Click);
            // 
            // b_SetEmpty
            // 
            this.b_SetEmpty.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_SetEmpty.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_SetEmpty.Location = new System.Drawing.Point(383, 56);
            this.b_SetEmpty.Name = "b_SetEmpty";
            this.b_SetEmpty.Size = new System.Drawing.Size(56, 21);
            this.b_SetEmpty.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_SetEmpty.TabIndex = 31;
            this.b_SetEmpty.Text = "Set Zero";
            this.b_SetEmpty.Click += new System.EventHandler(this.b_SetEmpty_Click);
            // 
            // b_SetFill
            // 
            this.b_SetFill.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_SetFill.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_SetFill.Location = new System.Drawing.Point(447, 56);
            this.b_SetFill.Name = "b_SetFill";
            this.b_SetFill.Size = new System.Drawing.Size(56, 21);
            this.b_SetFill.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_SetFill.TabIndex = 30;
            this.b_SetFill.Text = "Set Fill";
            this.b_SetFill.Click += new System.EventHandler(this.b_SetFill_Click);
            // 
            // TowerReactMod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ReactionMineral);
            this.Controls.Add(this.output2);
            this.Controls.Add(this.input6);
            this.Controls.Add(this.output1);
            this.Controls.Add(this.input5);
            this.Controls.Add(this.input4);
            this.Controls.Add(this.input3);
            this.Controls.Add(this.input2);
            this.Controls.Add(this.input1);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "TowerReactMod";
            this.Size = new System.Drawing.Size(555, 86);
            this.Load += new System.EventHandler(this.TowerReactMod_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ReactionMineral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input1)).EndInit();
            this.cm_ClearLink.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.input2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.output1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.output2)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ReactionMineral;
        private System.Windows.Forms.PictureBox input1;
        private System.Windows.Forms.PictureBox input2;
        private System.Windows.Forms.PictureBox input4;
        private System.Windows.Forms.PictureBox input3;
        private System.Windows.Forms.PictureBox input6;
        private System.Windows.Forms.PictureBox input5;
        private System.Windows.Forms.PictureBox output1;
        private System.Windows.Forms.PictureBox output2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip CM_MinReactSel;
        private System.Windows.Forms.ContextMenuStrip cm_ClearLink;
        private System.Windows.Forms.ToolStripMenuItem tsm_ClearLink;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX b_SetFull;
        private DevComponents.DotNetBar.ButtonX b_SetEmpty;
        private DevComponents.DotNetBar.ButtonX b_SetFill;
        private DevComponents.DotNetBar.LabelX l_RunTime;
        private DevComponents.DotNetBar.LabelX l_ExtraInfo;
        private DevComponents.DotNetBar.LabelX l_ModuleName;
        private DevComponents.DotNetBar.Controls.ProgressBarX pb_FillLevel;
    }
}
