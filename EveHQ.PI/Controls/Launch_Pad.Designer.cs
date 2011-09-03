namespace EveHQ.PI
{
    partial class Launch_Pad
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.pb_ModPic = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gp_BGP.SuspendLayout();
            this.cms_Remove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ModPic)).BeginInit();
            this.SuspendLayout();
            // 
            // gp_BGP
            // 
            this.gp_BGP.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_BGP.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_BGP.ContextMenuStrip = this.cms_Remove;
            this.gp_BGP.Controls.Add(this.labelX1);
            this.gp_BGP.Controls.Add(this.pb_ModPic);
            this.gp_BGP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_BGP.Location = new System.Drawing.Point(0, 0);
            this.gp_BGP.Name = "gp_BGP";
            this.gp_BGP.Size = new System.Drawing.Size(70, 70);
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
            this.gp_BGP.TabIndex = 1;
            this.gp_BGP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LaunchPad_OnMouseDown);
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
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ContextMenuStrip = this.cms_Remove;
            this.labelX1.ForeColor = System.Drawing.Color.Navy;
            this.labelX1.Location = new System.Drawing.Point(-3, 50);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(71, 15);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "Launch Pad";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Center;
            this.labelX1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LaunchPad_OnMouseDown);
            // 
            // pb_ModPic
            // 
            this.pb_ModPic.BackColor = System.Drawing.Color.Transparent;
            this.pb_ModPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_ModPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_ModPic.ContextMenuStrip = this.cms_Remove;
            this.pb_ModPic.Location = new System.Drawing.Point(7, 0);
            this.pb_ModPic.Name = "pb_ModPic";
            this.pb_ModPic.Size = new System.Drawing.Size(50, 50);
            this.pb_ModPic.TabIndex = 7;
            this.pb_ModPic.TabStop = false;
            this.pb_ModPic.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LaunchPad_OnMouseDown);
            // 
            // Launch_Pad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.cms_Remove;
            this.Controls.Add(this.gp_BGP);
            this.Name = "Launch_Pad";
            this.Size = new System.Drawing.Size(70, 70);
            this.Load += new System.EventHandler(this.Launch_Pad_Load);
            this.gp_BGP.ResumeLayout(false);
            this.cms_Remove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_ModPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_BGP;
        private System.Windows.Forms.PictureBox pb_ModPic;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.ContextMenuStrip cms_Remove;
        private System.Windows.Forms.ToolStripMenuItem tsmi_RemoveModule;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
