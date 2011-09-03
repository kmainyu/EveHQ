namespace EveHQ.PI
{
    partial class Extractor_Mod
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
            this.gp_BGP = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cms_Remove = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_RemoveModule = new System.Windows.Forms.ToolStripMenuItem();
            this.nud_RunTime = new System.Windows.Forms.NumericUpDown();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.pb_Module = new System.Windows.Forms.PictureBox();
            this.nud_CycleTime = new System.Windows.Forms.NumericUpDown();
            this.nud_CycleExtract = new System.Windows.Forms.NumericUpDown();
            this.cb_Extracting = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lb_UsedFor = new System.Windows.Forms.Label();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.gp_BGP.SuspendLayout();
            this.cms_Remove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_RunTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Module)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CycleTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CycleExtract)).BeginInit();
            this.SuspendLayout();
            // 
            // gp_BGP
            // 
            this.gp_BGP.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_BGP.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_BGP.ContextMenuStrip = this.cms_Remove;
            this.gp_BGP.Controls.Add(this.nud_RunTime);
            this.gp_BGP.Controls.Add(this.labelX5);
            this.gp_BGP.Controls.Add(this.labelX4);
            this.gp_BGP.Controls.Add(this.pb_Module);
            this.gp_BGP.Controls.Add(this.nud_CycleTime);
            this.gp_BGP.Controls.Add(this.nud_CycleExtract);
            this.gp_BGP.Controls.Add(this.cb_Extracting);
            this.gp_BGP.Controls.Add(this.lb_UsedFor);
            this.gp_BGP.Controls.Add(this.labelX3);
            this.gp_BGP.Controls.Add(this.labelX2);
            this.gp_BGP.Controls.Add(this.labelX1);
            this.gp_BGP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_BGP.Location = new System.Drawing.Point(0, 0);
            this.gp_BGP.Name = "gp_BGP";
            this.gp_BGP.Size = new System.Drawing.Size(210, 70);
            // 
            // 
            // 
            this.gp_BGP.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_BGP.Style.BackColorGradientAngle = 90;
            this.gp_BGP.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_BGP.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BGP.Style.BorderBottomWidth = 1;
            this.gp_BGP.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_BGP.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BGP.Style.BorderLeftWidth = 1;
            this.gp_BGP.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BGP.Style.BorderRightWidth = 1;
            this.gp_BGP.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BGP.Style.BorderTopWidth = 1;
            this.gp_BGP.Style.Class = "";
            this.gp_BGP.Style.CornerDiameter = 4;
            this.gp_BGP.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_BGP.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_BGP.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gp_BGP.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_BGP.StyleMouseDown.Class = "";
            this.gp_BGP.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_BGP.StyleMouseOver.Class = "";
            this.gp_BGP.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_BGP.TabIndex = 0;
            this.gp_BGP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // cms_Remove
            // 
            this.cms_Remove.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_RemoveModule});
            this.cms_Remove.Name = "cms_Remove";
            this.cms_Remove.Size = new System.Drawing.Size(162, 26);
            // 
            // tsmi_RemoveModule
            // 
            this.tsmi_RemoveModule.Name = "tsmi_RemoveModule";
            this.tsmi_RemoveModule.Size = new System.Drawing.Size(161, 22);
            this.tsmi_RemoveModule.Text = "Remove Module";
            this.tsmi_RemoveModule.Click += new System.EventHandler(this.tsmi_RemoveModule_Click);
            // 
            // nud_RunTime
            // 
            this.nud_RunTime.ContextMenuStrip = this.cms_Remove;
            this.nud_RunTime.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_RunTime.Location = new System.Drawing.Point(147, 28);
            this.nud_RunTime.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_RunTime.Name = "nud_RunTime";
            this.nud_RunTime.Size = new System.Drawing.Size(47, 18);
            this.nud_RunTime.TabIndex = 9;
            this.nud_RunTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_RunTime.ThousandsSeparator = true;
            this.nud_RunTime.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nud_RunTime.ValueChanged += new System.EventHandler(this.nud_RunTime_ValueChanged);
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.ContextMenuStrip = this.cms_Remove;
            this.labelX5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX5.Location = new System.Drawing.Point(193, 33);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(10, 13);
            this.labelX5.TabIndex = 14;
            this.labelX5.Text = "h";
            this.labelX5.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labelX5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX4.Location = new System.Drawing.Point(137, 33);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(10, 13);
            this.labelX4.TabIndex = 13;
            this.labelX4.Text = "m";
            this.labelX4.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labelX4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // pb_Module
            // 
            this.pb_Module.BackColor = System.Drawing.Color.Transparent;
            this.pb_Module.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_Module.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_Module.ContextMenuStrip = this.cms_Remove;
            this.pb_Module.Location = new System.Drawing.Point(0, 0);
            this.pb_Module.Name = "pb_Module";
            this.pb_Module.Size = new System.Drawing.Size(45, 45);
            this.pb_Module.TabIndex = 3;
            this.pb_Module.TabStop = false;
            this.pb_Module.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // nud_CycleTime
            // 
            this.nud_CycleTime.ContextMenuStrip = this.cms_Remove;
            this.nud_CycleTime.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_CycleTime.Location = new System.Drawing.Point(97, 28);
            this.nud_CycleTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_CycleTime.Name = "nud_CycleTime";
            this.nud_CycleTime.Size = new System.Drawing.Size(39, 18);
            this.nud_CycleTime.TabIndex = 8;
            this.nud_CycleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_CycleTime.ThousandsSeparator = true;
            this.nud_CycleTime.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nud_CycleTime.ValueChanged += new System.EventHandler(this.nud_CycleTime_ValueChanged);
            // 
            // nud_CycleExtract
            // 
            this.nud_CycleExtract.ContextMenuStrip = this.cms_Remove;
            this.nud_CycleExtract.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nud_CycleExtract.Location = new System.Drawing.Point(47, 28);
            this.nud_CycleExtract.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_CycleExtract.Name = "nud_CycleExtract";
            this.nud_CycleExtract.Size = new System.Drawing.Size(47, 18);
            this.nud_CycleExtract.TabIndex = 7;
            this.nud_CycleExtract.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_CycleExtract.ThousandsSeparator = true;
            this.nud_CycleExtract.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nud_CycleExtract.ValueChanged += new System.EventHandler(this.nud_CycleExtract_ValueChanged);
            // 
            // cb_Extracting
            // 
            this.cb_Extracting.ContextMenuStrip = this.cms_Remove;
            this.cb_Extracting.DisplayMember = "Text";
            this.cb_Extracting.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_Extracting.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Extracting.FormattingEnabled = true;
            this.cb_Extracting.ItemHeight = 13;
            this.cb_Extracting.Location = new System.Drawing.Point(47, 0);
            this.cb_Extracting.Name = "cb_Extracting";
            this.cb_Extracting.Size = new System.Drawing.Size(157, 19);
            this.cb_Extracting.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_Extracting.TabIndex = 6;
            this.cb_Extracting.Text = "Select Mineral to Mine";
            this.cb_Extracting.SelectedIndexChanged += new System.EventHandler(this.cb_Extracting_SelectedIndexChanged);
            // 
            // lb_UsedFor
            // 
            this.lb_UsedFor.BackColor = System.Drawing.Color.Transparent;
            this.lb_UsedFor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_UsedFor.ContextMenuStrip = this.cms_Remove;
            this.lb_UsedFor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_UsedFor.ForeColor = System.Drawing.Color.Navy;
            this.lb_UsedFor.Location = new System.Drawing.Point(0, 48);
            this.lb_UsedFor.Name = "lb_UsedFor";
            this.lb_UsedFor.Size = new System.Drawing.Size(204, 15);
            this.lb_UsedFor.TabIndex = 5;
            this.lb_UsedFor.Text = "Used In";
            this.lb_UsedFor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lb_UsedFor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.ContextMenuStrip = this.cms_Remove;
            this.labelX3.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX3.Location = new System.Drawing.Point(147, 17);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(47, 13);
            this.labelX3.TabIndex = 12;
            this.labelX3.Text = "Run Time";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labelX3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ContextMenuStrip = this.cms_Remove;
            this.labelX2.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.Location = new System.Drawing.Point(95, 17);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(47, 13);
            this.labelX2.TabIndex = 11;
            this.labelX2.Text = "Cycle Time";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labelX2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ContextMenuStrip = this.cms_Remove;
            this.labelX1.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.Location = new System.Drawing.Point(47, 17);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(47, 13);
            this.labelX1.TabIndex = 10;
            this.labelX1.Text = "Qty/Cycle";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labelX1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Extractor_Mod_MouseDown);
            // 
            // Extractor_Mod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.cms_Remove;
            this.Controls.Add(this.gp_BGP);
            this.Name = "Extractor_Mod";
            this.Size = new System.Drawing.Size(210, 70);
            this.Load += new System.EventHandler(this.Extractor_Mod_Load);
            this.gp_BGP.ResumeLayout(false);
            this.cms_Remove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_RunTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Module)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CycleTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CycleExtract)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_BGP;
        private System.Windows.Forms.Label lb_UsedFor;
        private System.Windows.Forms.PictureBox pb_Module;
        private System.Windows.Forms.NumericUpDown nud_RunTime;
        private System.Windows.Forms.NumericUpDown nud_CycleTime;
        private System.Windows.Forms.NumericUpDown nud_CycleExtract;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_Extracting;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX4;
        private System.Windows.Forms.ContextMenuStrip cms_Remove;
        private System.Windows.Forms.ToolStripMenuItem tsmi_RemoveModule;
    }
}
