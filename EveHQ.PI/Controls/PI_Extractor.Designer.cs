namespace EveHQ.PI
{
    partial class PI_Extractor
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
            this.gp_Background = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pb_Module = new System.Windows.Forms.PictureBox();
            this.lb_NameExt = new System.Windows.Forms.Label();
            this.lb_UsedFor = new System.Windows.Forms.Label();
            this.cms_Remove = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmsi_RemoveExtractor = new System.Windows.Forms.ToolStripMenuItem();
            this.gp_Background.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Module)).BeginInit();
            this.cms_Remove.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_Background
            // 
            this.gp_Background.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_Background.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_Background.Controls.Add(this.lb_UsedFor);
            this.gp_Background.Controls.Add(this.lb_NameExt);
            this.gp_Background.Controls.Add(this.pb_Module);
            this.gp_Background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_Background.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gp_Background.Location = new System.Drawing.Point(0, 0);
            this.gp_Background.Name = "gp_Background";
            this.gp_Background.Size = new System.Drawing.Size(200, 50);
            // 
            // 
            // 
            this.gp_Background.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_Background.Style.BackColorGradientAngle = 90;
            this.gp_Background.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_Background.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderBottomWidth = 1;
            this.gp_Background.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_Background.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderLeftWidth = 1;
            this.gp_Background.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderRightWidth = 1;
            this.gp_Background.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderTopWidth = 1;
            this.gp_Background.Style.Class = "";
            this.gp_Background.Style.CornerDiameter = 4;
            this.gp_Background.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_Background.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_Background.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gp_Background.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_Background.StyleMouseDown.Class = "";
            this.gp_Background.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_Background.StyleMouseOver.Class = "";
            this.gp_Background.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_Background.TabIndex = 0;
            // 
            // pb_Module
            // 
            this.pb_Module.BackColor = System.Drawing.Color.Transparent;
            this.pb_Module.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_Module.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_Module.ContextMenuStrip = this.cms_Remove;
            this.pb_Module.Location = new System.Drawing.Point(0, 0);
            this.pb_Module.Name = "pb_Module";
            this.pb_Module.Size = new System.Drawing.Size(44, 44);
            this.pb_Module.TabIndex = 0;
            this.pb_Module.TabStop = false;
            // 
            // lb_NameExt
            // 
            this.lb_NameExt.BackColor = System.Drawing.Color.Transparent;
            this.lb_NameExt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_NameExt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_NameExt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lb_NameExt.Location = new System.Drawing.Point(45, 0);
            this.lb_NameExt.Name = "lb_NameExt";
            this.lb_NameExt.Size = new System.Drawing.Size(149, 28);
            this.lb_NameExt.TabIndex = 1;
            this.lb_NameExt.Text = "Module Name for the Extractor displayed here";
            // 
            // lb_UsedFor
            // 
            this.lb_UsedFor.BackColor = System.Drawing.Color.Transparent;
            this.lb_UsedFor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_UsedFor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_UsedFor.ForeColor = System.Drawing.Color.Navy;
            this.lb_UsedFor.Location = new System.Drawing.Point(45, 29);
            this.lb_UsedFor.Name = "lb_UsedFor";
            this.lb_UsedFor.Size = new System.Drawing.Size(149, 15);
            this.lb_UsedFor.TabIndex = 2;
            this.lb_UsedFor.Text = "Used In";
            this.lb_UsedFor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cms_Remove
            // 
            this.cms_Remove.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_RemoveExtractor});
            this.cms_Remove.Name = "cms_Remove";
            this.cms_Remove.Size = new System.Drawing.Size(118, 26);
            // 
            // tmsi_RemoveExtractor
            // 
            this.tmsi_RemoveExtractor.Name = "tmsi_RemoveExtractor";
            this.tmsi_RemoveExtractor.Size = new System.Drawing.Size(152, 22);
            this.tmsi_RemoveExtractor.Text = "Remove";
            this.tmsi_RemoveExtractor.Click += new System.EventHandler(this.tmsi_RemoveExtractor_Click);
            // 
            // PI_Extractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gp_Background);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PI_Extractor";
            this.Size = new System.Drawing.Size(200, 50);
            this.gp_Background.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Module)).EndInit();
            this.cms_Remove.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_Background;
        private System.Windows.Forms.PictureBox pb_Module;
        private System.Windows.Forms.Label lb_UsedFor;
        private System.Windows.Forms.Label lb_NameExt;
        private System.Windows.Forms.ContextMenuStrip cms_Remove;
        private System.Windows.Forms.ToolStripMenuItem tmsi_RemoveExtractor;
    }
}
