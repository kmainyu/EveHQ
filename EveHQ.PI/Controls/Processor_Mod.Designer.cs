namespace EveHQ.PI
{
    partial class Processor_Mod
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
            this.tc_Processor = new DevComponents.DotNetBar.TabControl();
            this.cms_Remove = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_RemoveProcessor = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.ii_NumProc = new DevComponents.Editors.IntegerInput();
            this.lb_Produces = new System.Windows.Forms.Label();
            this.rbx_P4 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.pb_ModPic = new System.Windows.Forms.PictureBox();
            this.rbx_P3 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.rbx_P2 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cb_ExtractProcess = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.rbx_P1 = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.ti_Overview = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.lv_MatsNeeded = new System.Windows.Forms.ListView();
            this.ch_Mat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_Qty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ti_RecipeDetails = new DevComponents.DotNetBar.TabItem(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gp_BGP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tc_Processor)).BeginInit();
            this.tc_Processor.SuspendLayout();
            this.cms_Remove.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ii_NumProc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ModPic)).BeginInit();
            this.tabControlPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_BGP
            // 
            this.gp_BGP.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_BGP.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_BGP.Controls.Add(this.tc_Processor);
            this.gp_BGP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_BGP.Location = new System.Drawing.Point(0, 0);
            this.gp_BGP.Name = "gp_BGP";
            this.gp_BGP.Size = new System.Drawing.Size(250, 70);
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
            // 
            // tc_Processor
            // 
            this.tc_Processor.CanReorderTabs = false;
            this.tc_Processor.CloseButtonOnTabsAlwaysDisplayed = false;
            this.tc_Processor.ContextMenuStrip = this.cms_Remove;
            this.tc_Processor.Controls.Add(this.tabControlPanel1);
            this.tc_Processor.Controls.Add(this.tabControlPanel2);
            this.tc_Processor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_Processor.FixedTabSize = new System.Drawing.Size(19, 16);
            this.tc_Processor.Location = new System.Drawing.Point(0, 0);
            this.tc_Processor.Name = "tc_Processor";
            this.tc_Processor.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tc_Processor.SelectedTabIndex = 0;
            this.tc_Processor.Size = new System.Drawing.Size(244, 64);
            this.tc_Processor.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Dock;
            this.tc_Processor.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Right;
            this.tc_Processor.TabIndex = 14;
            this.tc_Processor.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.MultilineNoNavigationBox;
            this.tc_Processor.Tabs.Add(this.ti_Overview);
            this.tc_Processor.Tabs.Add(this.ti_RecipeDetails);
            this.tc_Processor.ThemeAware = true;
            // 
            // cms_Remove
            // 
            this.cms_Remove.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_RemoveProcessor});
            this.cms_Remove.Name = "cms_Remove";
            this.cms_Remove.Size = new System.Drawing.Size(162, 26);
            // 
            // tsmi_RemoveProcessor
            // 
            this.tsmi_RemoveProcessor.Name = "tsmi_RemoveProcessor";
            this.tsmi_RemoveProcessor.Size = new System.Drawing.Size(161, 22);
            this.tsmi_RemoveProcessor.Text = "Remove Module";
            this.tsmi_RemoveProcessor.Click += new System.EventHandler(this.tsmi_RemoveProcessor_Click);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.ContextMenuStrip = this.cms_Remove;
            this.tabControlPanel1.Controls.Add(this.ii_NumProc);
            this.tabControlPanel1.Controls.Add(this.lb_Produces);
            this.tabControlPanel1.Controls.Add(this.rbx_P4);
            this.tabControlPanel1.Controls.Add(this.pb_ModPic);
            this.tabControlPanel1.Controls.Add(this.rbx_P3);
            this.tabControlPanel1.Controls.Add(this.rbx_P2);
            this.tabControlPanel1.Controls.Add(this.cb_ExtractProcess);
            this.tabControlPanel1.Controls.Add(this.rbx_P1);
            this.tabControlPanel1.Controls.Add(this.labelX2);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(225, 64);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Top)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 180;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.ti_Overview;
            this.tabControlPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlPanel1_MouseDown);
            // 
            // ii_NumProc
            // 
            this.ii_NumProc.AllowEmptyState = false;
            // 
            // 
            // 
            this.ii_NumProc.BackgroundStyle.Class = "DateTimeInputBackground";
            this.ii_NumProc.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ii_NumProc.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.ii_NumProc.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.ii_NumProc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ii_NumProc.ForeColor = System.Drawing.Color.Navy;
            this.ii_NumProc.InputHorizontalAlignment = DevComponents.Editors.eHorizontalAlignment.Center;
            this.ii_NumProc.Location = new System.Drawing.Point(183, 42);
            this.ii_NumProc.Margin = new System.Windows.Forms.Padding(1);
            this.ii_NumProc.MaxValue = 12;
            this.ii_NumProc.MinValue = 1;
            this.ii_NumProc.Name = "ii_NumProc";
            this.ii_NumProc.ShowUpDown = true;
            this.ii_NumProc.Size = new System.Drawing.Size(41, 21);
            this.ii_NumProc.TabIndex = 14;
            this.ii_NumProc.Value = 1;
            this.ii_NumProc.WatermarkAlignment = DevComponents.Editors.eTextAlignment.Center;
            this.ii_NumProc.WatermarkEnabled = false;
            this.ii_NumProc.ValueChanged += new System.EventHandler(this.ii_NumProc_ValueChanged);
            // 
            // lb_Produces
            // 
            this.lb_Produces.BackColor = System.Drawing.Color.Transparent;
            this.lb_Produces.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Produces.ContextMenuStrip = this.cms_Remove;
            this.lb_Produces.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lb_Produces.Location = new System.Drawing.Point(44, 24);
            this.lb_Produces.Name = "lb_Produces";
            this.lb_Produces.Size = new System.Drawing.Size(180, 18);
            this.lb_Produces.TabIndex = 8;
            this.lb_Produces.Text = "Qty Produced per Hour";
            this.lb_Produces.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlPanel1_MouseDown);
            // 
            // rbx_P4
            // 
            this.rbx_P4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.rbx_P4.BackgroundStyle.Class = "";
            this.rbx_P4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rbx_P4.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rbx_P4.ContextMenuStrip = this.cms_Remove;
            this.rbx_P4.Location = new System.Drawing.Point(98, 45);
            this.rbx_P4.Name = "rbx_P4";
            this.rbx_P4.Size = new System.Drawing.Size(37, 15);
            this.rbx_P4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.rbx_P4.TabIndex = 13;
            this.rbx_P4.Text = "P4";
            this.rbx_P4.TextColor = System.Drawing.Color.Navy;
            this.rbx_P4.CheckedChanged += new System.EventHandler(this.Processor_Mod_Level_Changed);
            // 
            // pb_ModPic
            // 
            this.pb_ModPic.BackColor = System.Drawing.Color.Transparent;
            this.pb_ModPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_ModPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_ModPic.ContextMenuStrip = this.cms_Remove;
            this.pb_ModPic.Location = new System.Drawing.Point(1, 0);
            this.pb_ModPic.Name = "pb_ModPic";
            this.pb_ModPic.Size = new System.Drawing.Size(42, 42);
            this.pb_ModPic.TabIndex = 6;
            this.pb_ModPic.TabStop = false;
            this.pb_ModPic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlPanel1_MouseDown);
            // 
            // rbx_P3
            // 
            this.rbx_P3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.rbx_P3.BackgroundStyle.Class = "";
            this.rbx_P3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rbx_P3.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rbx_P3.ContextMenuStrip = this.cms_Remove;
            this.rbx_P3.Location = new System.Drawing.Point(65, 45);
            this.rbx_P3.Name = "rbx_P3";
            this.rbx_P3.Size = new System.Drawing.Size(37, 15);
            this.rbx_P3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.rbx_P3.TabIndex = 12;
            this.rbx_P3.Text = "P3";
            this.rbx_P3.TextColor = System.Drawing.Color.Navy;
            this.rbx_P3.CheckedChanged += new System.EventHandler(this.Processor_Mod_Level_Changed);
            // 
            // rbx_P2
            // 
            this.rbx_P2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.rbx_P2.BackgroundStyle.Class = "";
            this.rbx_P2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rbx_P2.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rbx_P2.ContextMenuStrip = this.cms_Remove;
            this.rbx_P2.Location = new System.Drawing.Point(32, 45);
            this.rbx_P2.Name = "rbx_P2";
            this.rbx_P2.Size = new System.Drawing.Size(37, 15);
            this.rbx_P2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.rbx_P2.TabIndex = 11;
            this.rbx_P2.Text = "P2";
            this.rbx_P2.TextColor = System.Drawing.Color.Navy;
            this.rbx_P2.CheckedChanged += new System.EventHandler(this.Processor_Mod_Level_Changed);
            // 
            // cb_ExtractProcess
            // 
            this.cb_ExtractProcess.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cb_ExtractProcess.ContextMenuStrip = this.cms_Remove;
            this.cb_ExtractProcess.DisplayMember = "Text";
            this.cb_ExtractProcess.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_ExtractProcess.DropDownHeight = 250;
            this.cb_ExtractProcess.DropDownWidth = 200;
            this.cb_ExtractProcess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ExtractProcess.ForeColor = System.Drawing.Color.Navy;
            this.cb_ExtractProcess.FormattingEnabled = true;
            this.cb_ExtractProcess.IntegralHeight = false;
            this.cb_ExtractProcess.ItemHeight = 14;
            this.cb_ExtractProcess.Location = new System.Drawing.Point(44, 1);
            this.cb_ExtractProcess.MaxDropDownItems = 20;
            this.cb_ExtractProcess.Name = "cb_ExtractProcess";
            this.cb_ExtractProcess.Size = new System.Drawing.Size(180, 20);
            this.cb_ExtractProcess.Sorted = true;
            this.cb_ExtractProcess.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_ExtractProcess.TabIndex = 7;
            this.cb_ExtractProcess.Text = "Select Recipe to Process";
            this.cb_ExtractProcess.SelectedIndexChanged += new System.EventHandler(this.cb_ExtractProcess_SelectedIndexChanged);
            // 
            // rbx_P1
            // 
            this.rbx_P1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.rbx_P1.BackgroundStyle.Class = "";
            this.rbx_P1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rbx_P1.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rbx_P1.ContextMenuStrip = this.cms_Remove;
            this.rbx_P1.Location = new System.Drawing.Point(-1, 45);
            this.rbx_P1.Name = "rbx_P1";
            this.rbx_P1.Size = new System.Drawing.Size(37, 15);
            this.rbx_P1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.rbx_P1.TabIndex = 10;
            this.rbx_P1.Text = "P1";
            this.rbx_P1.TextColor = System.Drawing.Color.Navy;
            this.rbx_P1.CheckedChanged += new System.EventHandler(this.Processor_Mod_Level_Changed);
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
            this.labelX2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.Navy;
            this.labelX2.Location = new System.Drawing.Point(146, 46);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(35, 13);
            this.labelX2.TabIndex = 23;
            this.labelX2.Text = "# Mod";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // ti_Overview
            // 
            this.ti_Overview.AttachedControl = this.tabControlPanel1;
            this.ti_Overview.Name = "ti_Overview";
            this.ti_Overview.Text = "O";
            this.ti_Overview.Tooltip = "Processor and Process Overview";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.lv_MatsNeeded);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 0);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(225, 64);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Top)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 180;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.ti_RecipeDetails;
            // 
            // lv_MatsNeeded
            // 
            this.lv_MatsNeeded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lv_MatsNeeded.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_Mat,
            this.ch_Qty});
            this.lv_MatsNeeded.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_MatsNeeded.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_MatsNeeded.FullRowSelect = true;
            this.lv_MatsNeeded.GridLines = true;
            this.lv_MatsNeeded.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv_MatsNeeded.Location = new System.Drawing.Point(1, 1);
            this.lv_MatsNeeded.MultiSelect = false;
            this.lv_MatsNeeded.Name = "lv_MatsNeeded";
            this.lv_MatsNeeded.Size = new System.Drawing.Size(223, 62);
            this.lv_MatsNeeded.TabIndex = 10;
            this.lv_MatsNeeded.UseCompatibleStateImageBehavior = false;
            this.lv_MatsNeeded.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControlPanel1_MouseDown);
            // 
            // ch_Mat
            // 
            this.ch_Mat.Text = "";
            this.ch_Mat.Width = 110;
            // 
            // ch_Qty
            // 
            this.ch_Qty.Text = "";
            this.ch_Qty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ch_Qty.Width = 65;
            // 
            // ti_RecipeDetails
            // 
            this.ti_RecipeDetails.AttachedControl = this.tabControlPanel2;
            this.ti_RecipeDetails.Name = "ti_RecipeDetails";
            this.ti_RecipeDetails.Text = "D";
            this.ti_RecipeDetails.Tooltip = "Recipe Details";
            // 
            // Processor_Mod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gp_BGP);
            this.Name = "Processor_Mod";
            this.Size = new System.Drawing.Size(250, 70);
            this.Load += new System.EventHandler(this.Processor_Mod_Load);
            this.gp_BGP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tc_Processor)).EndInit();
            this.tc_Processor.ResumeLayout(false);
            this.cms_Remove.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ii_NumProc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ModPic)).EndInit();
            this.tabControlPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_BGP;
        private System.Windows.Forms.Label lb_Produces;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_ExtractProcess;
        private System.Windows.Forms.PictureBox pb_ModPic;
        private DevComponents.DotNetBar.TabControl tc_Processor;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem ti_Overview;
        private DevComponents.DotNetBar.Controls.CheckBoxX rbx_P4;
        private DevComponents.DotNetBar.Controls.CheckBoxX rbx_P3;
        private DevComponents.DotNetBar.Controls.CheckBoxX rbx_P2;
        private DevComponents.DotNetBar.Controls.CheckBoxX rbx_P1;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem ti_RecipeDetails;
        private System.Windows.Forms.ListView lv_MatsNeeded;
        private System.Windows.Forms.ColumnHeader ch_Mat;
        private System.Windows.Forms.ColumnHeader ch_Qty;
        private System.Windows.Forms.ContextMenuStrip cms_Remove;
        private System.Windows.Forms.ToolStripMenuItem tsmi_RemoveProcessor;
        private DevComponents.Editors.IntegerInput ii_NumProc;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
