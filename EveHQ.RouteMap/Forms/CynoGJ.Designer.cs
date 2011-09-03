namespace EveHQ.RouteMap
{
    partial class CynoGJ
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
            this.gp_GenJam = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cms_RemoveCyno = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmsi_RemoveCyno = new System.Windows.Forms.ToolStripMenuItem();
            this.rb_Generator = new System.Windows.Forms.RadioButton();
            this.rb_Jammer = new System.Windows.Forms.RadioButton();
            this.cb_SystemMoon = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lb_SysName = new DevComponents.DotNetBar.LabelX();
            this.pb_JumpType = new System.Windows.Forms.PictureBox();
            this.gp_GenJam.SuspendLayout();
            this.cms_RemoveCyno.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_JumpType)).BeginInit();
            this.SuspendLayout();
            // 
            // gp_GenJam
            // 
            this.gp_GenJam.BackColor = System.Drawing.Color.Transparent;
            this.gp_GenJam.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_GenJam.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_GenJam.ContextMenuStrip = this.cms_RemoveCyno;
            this.gp_GenJam.Controls.Add(this.rb_Generator);
            this.gp_GenJam.Controls.Add(this.rb_Jammer);
            this.gp_GenJam.Controls.Add(this.cb_SystemMoon);
            this.gp_GenJam.Controls.Add(this.lb_SysName);
            this.gp_GenJam.Controls.Add(this.pb_JumpType);
            this.gp_GenJam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_GenJam.Location = new System.Drawing.Point(0, 0);
            this.gp_GenJam.Name = "gp_GenJam";
            this.gp_GenJam.Size = new System.Drawing.Size(409, 42);
            // 
            // 
            // 
            this.gp_GenJam.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_GenJam.Style.BackColorGradientAngle = 90;
            this.gp_GenJam.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_GenJam.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_GenJam.Style.BorderBottomWidth = 1;
            this.gp_GenJam.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_GenJam.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_GenJam.Style.BorderLeftWidth = 1;
            this.gp_GenJam.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_GenJam.Style.BorderRightWidth = 1;
            this.gp_GenJam.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_GenJam.Style.BorderTopWidth = 1;
            this.gp_GenJam.Style.Class = "";
            this.gp_GenJam.Style.CornerDiameter = 4;
            this.gp_GenJam.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_GenJam.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_GenJam.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gp_GenJam.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_GenJam.StyleMouseDown.Class = "";
            this.gp_GenJam.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_GenJam.StyleMouseOver.Class = "";
            this.gp_GenJam.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_GenJam.TabIndex = 0;
            // 
            // cms_RemoveCyno
            // 
            this.cms_RemoveCyno.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_RemoveCyno});
            this.cms_RemoveCyno.Name = "cms_RemoveCyno";
            this.cms_RemoveCyno.Size = new System.Drawing.Size(149, 26);
            // 
            // tmsi_RemoveCyno
            // 
            this.tmsi_RemoveCyno.Name = "tmsi_RemoveCyno";
            this.tmsi_RemoveCyno.Size = new System.Drawing.Size(148, 22);
            this.tmsi_RemoveCyno.Text = "Remove Cyno";
            this.tmsi_RemoveCyno.Click += new System.EventHandler(this.tmsi_RemoveCyno_Click);
            // 
            // rb_Generator
            // 
            this.rb_Generator.AutoSize = true;
            this.rb_Generator.BackColor = System.Drawing.Color.Transparent;
            this.rb_Generator.Checked = true;
            this.rb_Generator.ForeColor = System.Drawing.Color.Navy;
            this.rb_Generator.Location = new System.Drawing.Point(130, 21);
            this.rb_Generator.Name = "rb_Generator";
            this.rb_Generator.Size = new System.Drawing.Size(99, 17);
            this.rb_Generator.TabIndex = 136;
            this.rb_Generator.TabStop = true;
            this.rb_Generator.Text = "Cyno Generator";
            this.rb_Generator.UseVisualStyleBackColor = false;
            this.rb_Generator.CheckedChanged += new System.EventHandler(this.rb_Generator_CheckedChanged);
            // 
            // rb_Jammer
            // 
            this.rb_Jammer.AutoSize = true;
            this.rb_Jammer.BackColor = System.Drawing.Color.Transparent;
            this.rb_Jammer.ForeColor = System.Drawing.Color.Navy;
            this.rb_Jammer.Location = new System.Drawing.Point(41, 21);
            this.rb_Jammer.Name = "rb_Jammer";
            this.rb_Jammer.Size = new System.Drawing.Size(88, 17);
            this.rb_Jammer.TabIndex = 135;
            this.rb_Jammer.Text = "Cyno Jammer";
            this.rb_Jammer.UseVisualStyleBackColor = false;
            this.rb_Jammer.CheckedChanged += new System.EventHandler(this.rb_Jammer_CheckedChanged);
            // 
            // cb_SystemMoon
            // 
            this.cb_SystemMoon.DisplayMember = "Text";
            this.cb_SystemMoon.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_SystemMoon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_SystemMoon.ForeColor = System.Drawing.Color.Navy;
            this.cb_SystemMoon.FormattingEnabled = true;
            this.cb_SystemMoon.ItemHeight = 14;
            this.cb_SystemMoon.Location = new System.Drawing.Point(212, 0);
            this.cb_SystemMoon.Name = "cb_SystemMoon";
            this.cb_SystemMoon.Size = new System.Drawing.Size(190, 20);
            this.cb_SystemMoon.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_SystemMoon.TabIndex = 26;
            this.cb_SystemMoon.Text = "Moon";
            this.cb_SystemMoon.SelectedIndexChanged += new System.EventHandler(this.cb_SystemMoon_SelectedIndexChanged);
            // 
            // lb_SysName
            // 
            this.lb_SysName.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lb_SysName.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lb_SysName.BackgroundStyle.BorderBottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lb_SysName.BackgroundStyle.BorderBottomWidth = 1;
            this.lb_SysName.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lb_SysName.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lb_SysName.BackgroundStyle.BorderLeftColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lb_SysName.BackgroundStyle.BorderLeftWidth = 1;
            this.lb_SysName.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lb_SysName.BackgroundStyle.BorderRightColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lb_SysName.BackgroundStyle.BorderRightWidth = 1;
            this.lb_SysName.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.lb_SysName.BackgroundStyle.BorderTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lb_SysName.BackgroundStyle.BorderTopWidth = 1;
            this.lb_SysName.BackgroundStyle.Class = "";
            this.lb_SysName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lb_SysName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_SysName.ForeColor = System.Drawing.Color.Green;
            this.lb_SysName.Location = new System.Drawing.Point(40, 0);
            this.lb_SysName.Name = "lb_SysName";
            this.lb_SysName.Size = new System.Drawing.Size(168, 20);
            this.lb_SysName.TabIndex = 24;
            this.lb_SysName.Text = "System Name";
            this.lb_SysName.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // pb_JumpType
            // 
            this.pb_JumpType.BackColor = System.Drawing.Color.Transparent;
            this.pb_JumpType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_JumpType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_JumpType.Location = new System.Drawing.Point(1, 0);
            this.pb_JumpType.Name = "pb_JumpType";
            this.pb_JumpType.Size = new System.Drawing.Size(36, 36);
            this.pb_JumpType.TabIndex = 25;
            this.pb_JumpType.TabStop = false;
            // 
            // CynoGJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gp_GenJam);
            this.DoubleBuffered = true;
            this.Name = "CynoGJ";
            this.Size = new System.Drawing.Size(409, 42);
            this.gp_GenJam.ResumeLayout(false);
            this.gp_GenJam.PerformLayout();
            this.cms_RemoveCyno.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_JumpType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_GenJam;
        private DevComponents.DotNetBar.LabelX lb_SysName;
        private System.Windows.Forms.PictureBox pb_JumpType;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_SystemMoon;
        private System.Windows.Forms.RadioButton rb_Generator;
        private System.Windows.Forms.RadioButton rb_Jammer;
        private System.Windows.Forms.ContextMenuStrip cms_RemoveCyno;
        private System.Windows.Forms.ToolStripMenuItem tmsi_RemoveCyno;
    }
}
