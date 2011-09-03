namespace EveHQ.PI
{
    partial class PI_Module
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
            this.gp_BackGround = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lv_MatsNeeded = new System.Windows.Forms.ListView();
            this.ch_Mat = new System.Windows.Forms.ColumnHeader();
            this.ch_Qty = new System.Windows.Forms.ColumnHeader();
            this.lb_Produces = new System.Windows.Forms.Label();
            this.cb_ExtractProcess = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lb_ModName = new System.Windows.Forms.Label();
            this.cms_Remove = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmsi_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.pb_ModPic = new System.Windows.Forms.PictureBox();
            this.gp_BackGround.SuspendLayout();
            this.cms_Remove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ModPic)).BeginInit();
            this.SuspendLayout();
            // 
            // gp_BackGround
            // 
            this.gp_BackGround.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_BackGround.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_BackGround.Controls.Add(this.lv_MatsNeeded);
            this.gp_BackGround.Controls.Add(this.lb_Produces);
            this.gp_BackGround.Controls.Add(this.cb_ExtractProcess);
            this.gp_BackGround.Controls.Add(this.lb_ModName);
            this.gp_BackGround.Controls.Add(this.pb_ModPic);
            this.gp_BackGround.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_BackGround.Location = new System.Drawing.Point(0, 0);
            this.gp_BackGround.Name = "gp_BackGround";
            this.gp_BackGround.Size = new System.Drawing.Size(200, 121);
            // 
            // 
            // 
            this.gp_BackGround.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_BackGround.Style.BackColorGradientAngle = 90;
            this.gp_BackGround.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_BackGround.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BackGround.Style.BorderBottomWidth = 1;
            this.gp_BackGround.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_BackGround.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BackGround.Style.BorderLeftWidth = 1;
            this.gp_BackGround.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BackGround.Style.BorderRightWidth = 1;
            this.gp_BackGround.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BackGround.Style.BorderTopWidth = 1;
            this.gp_BackGround.Style.Class = "";
            this.gp_BackGround.Style.CornerDiameter = 4;
            this.gp_BackGround.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_BackGround.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_BackGround.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gp_BackGround.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_BackGround.StyleMouseDown.Class = "";
            this.gp_BackGround.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_BackGround.StyleMouseOver.Class = "";
            this.gp_BackGround.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_BackGround.TabIndex = 0;
            // 
            // lv_MatsNeeded
            // 
            this.lv_MatsNeeded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lv_MatsNeeded.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_Mat,
            this.ch_Qty});
            this.lv_MatsNeeded.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lv_MatsNeeded.FullRowSelect = true;
            this.lv_MatsNeeded.GridLines = true;
            this.lv_MatsNeeded.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv_MatsNeeded.Location = new System.Drawing.Point(0, 68);
            this.lv_MatsNeeded.MultiSelect = false;
            this.lv_MatsNeeded.Name = "lv_MatsNeeded";
            this.lv_MatsNeeded.Size = new System.Drawing.Size(194, 47);
            this.lv_MatsNeeded.TabIndex = 5;
            this.lv_MatsNeeded.UseCompatibleStateImageBehavior = false;
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
            // lb_Produces
            // 
            this.lb_Produces.BackColor = System.Drawing.Color.Transparent;
            this.lb_Produces.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_Produces.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lb_Produces.Location = new System.Drawing.Point(46, 29);
            this.lb_Produces.Name = "lb_Produces";
            this.lb_Produces.Size = new System.Drawing.Size(148, 16);
            this.lb_Produces.TabIndex = 4;
            this.lb_Produces.Text = "Qty Produced Per Hour";
            // 
            // cb_ExtractProcess
            // 
            this.cb_ExtractProcess.DisplayMember = "Text";
            this.cb_ExtractProcess.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_ExtractProcess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_ExtractProcess.ForeColor = System.Drawing.Color.Navy;
            this.cb_ExtractProcess.FormattingEnabled = true;
            this.cb_ExtractProcess.ItemHeight = 15;
            this.cb_ExtractProcess.Location = new System.Drawing.Point(0, 46);
            this.cb_ExtractProcess.Name = "cb_ExtractProcess";
            this.cb_ExtractProcess.Size = new System.Drawing.Size(195, 21);
            this.cb_ExtractProcess.Sorted = true;
            this.cb_ExtractProcess.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_ExtractProcess.TabIndex = 2;
            this.cb_ExtractProcess.SelectedIndexChanged += new System.EventHandler(this.cb_ExtractProcess_SelectedIndexChanged);
            // 
            // lb_ModName
            // 
            this.lb_ModName.BackColor = System.Drawing.Color.Transparent;
            this.lb_ModName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_ModName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lb_ModName.Location = new System.Drawing.Point(46, 0);
            this.lb_ModName.Name = "lb_ModName";
            this.lb_ModName.Size = new System.Drawing.Size(148, 28);
            this.lb_ModName.TabIndex = 1;
            this.lb_ModName.Text = "Module Name / Type Text";
            // 
            // cms_Remove
            // 
            this.cms_Remove.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_Remove});
            this.cms_Remove.Name = "cms_Remove";
            this.cms_Remove.Size = new System.Drawing.Size(118, 26);
            // 
            // tmsi_Remove
            // 
            this.tmsi_Remove.Name = "tmsi_Remove";
            this.tmsi_Remove.Size = new System.Drawing.Size(117, 22);
            this.tmsi_Remove.Text = "Remove";
            this.tmsi_Remove.Click += new System.EventHandler(this.tmsi_Remove_Click);
            // 
            // pb_ModPic
            // 
            this.pb_ModPic.BackColor = System.Drawing.Color.Transparent;
            this.pb_ModPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_ModPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_ModPic.ContextMenuStrip = this.cms_Remove;
            this.pb_ModPic.Location = new System.Drawing.Point(0, 0);
            this.pb_ModPic.Name = "pb_ModPic";
            this.pb_ModPic.Size = new System.Drawing.Size(45, 45);
            this.pb_ModPic.TabIndex = 0;
            this.pb_ModPic.TabStop = false;
            // 
            // PI_Module
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gp_BackGround);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PI_Module";
            this.Size = new System.Drawing.Size(200, 121);
            this.gp_BackGround.ResumeLayout(false);
            this.cms_Remove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_ModPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_BackGround;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_ExtractProcess;
        private System.Windows.Forms.Label lb_ModName;
        private System.Windows.Forms.PictureBox pb_ModPic;
        private System.Windows.Forms.ContextMenuStrip cms_Remove;
        private System.Windows.Forms.ToolStripMenuItem tmsi_Remove;
        private System.Windows.Forms.Label lb_Produces;
        private System.Windows.Forms.ListView lv_MatsNeeded;
        private System.Windows.Forms.ColumnHeader ch_Mat;
        private System.Windows.Forms.ColumnHeader ch_Qty;
    }
}
