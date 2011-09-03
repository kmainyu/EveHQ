namespace EveHQ.PI
{
    partial class CopyRename
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyRename));
            this.gp_BG = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.b_Accept = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.tb_NewName = new System.Windows.Forms.TextBox();
            this.lb_FacilityName = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.gp_BG.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_BG
            // 
            this.gp_BG.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_BG.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_BG.Controls.Add(this.b_Cancel);
            this.gp_BG.Controls.Add(this.b_Accept);
            this.gp_BG.Controls.Add(this.labelX2);
            this.gp_BG.Controls.Add(this.tb_NewName);
            this.gp_BG.Controls.Add(this.lb_FacilityName);
            this.gp_BG.Controls.Add(this.labelX1);
            this.gp_BG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_BG.Location = new System.Drawing.Point(0, 0);
            this.gp_BG.Name = "gp_BG";
            this.gp_BG.Size = new System.Drawing.Size(447, 103);
            // 
            // 
            // 
            this.gp_BG.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_BG.Style.BackColorGradientAngle = 90;
            this.gp_BG.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_BG.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BG.Style.BorderBottomWidth = 1;
            this.gp_BG.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_BG.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BG.Style.BorderLeftWidth = 1;
            this.gp_BG.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BG.Style.BorderRightWidth = 1;
            this.gp_BG.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_BG.Style.BorderTopWidth = 1;
            this.gp_BG.Style.Class = "";
            this.gp_BG.Style.CornerDiameter = 4;
            this.gp_BG.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_BG.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_BG.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gp_BG.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_BG.StyleMouseDown.Class = "";
            this.gp_BG.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_BG.StyleMouseOver.Class = "";
            this.gp_BG.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_BG.TabIndex = 0;
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(357, 61);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(77, 29);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 5;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // b_Accept
            // 
            this.b_Accept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Accept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Accept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Accept.Location = new System.Drawing.Point(248, 61);
            this.b_Accept.Name = "b_Accept";
            this.b_Accept.Size = new System.Drawing.Size(77, 29);
            this.b_Accept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Accept.TabIndex = 4;
            this.b_Accept.Text = "Accept";
            this.b_Accept.Click += new System.EventHandler(this.b_Accept_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.Navy;
            this.labelX2.Location = new System.Drawing.Point(9, 29);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "New Name:";
            // 
            // tb_NewName
            // 
            this.tb_NewName.Location = new System.Drawing.Point(79, 32);
            this.tb_NewName.Name = "tb_NewName";
            this.tb_NewName.Size = new System.Drawing.Size(359, 20);
            this.tb_NewName.TabIndex = 2;
            // 
            // lb_FacilityName
            // 
            this.lb_FacilityName.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lb_FacilityName.BackgroundStyle.Class = "";
            this.lb_FacilityName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lb_FacilityName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_FacilityName.ForeColor = System.Drawing.Color.Navy;
            this.lb_FacilityName.Location = new System.Drawing.Point(79, 3);
            this.lb_FacilityName.Name = "lb_FacilityName";
            this.lb_FacilityName.Size = new System.Drawing.Size(359, 23);
            this.lb_FacilityName.TabIndex = 1;
            this.lb_FacilityName.Text = "Facility Name";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.ForeColor = System.Drawing.Color.Navy;
            this.labelX1.Location = new System.Drawing.Point(9, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(45, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Name:";
            // 
            // CopyRename
            // 
            this.AcceptButton = this.b_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(447, 103);
            this.Controls.Add(this.gp_BG);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyRename";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Copy or Rename a Facility";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CopyRename_Load);
            this.gp_BG.ResumeLayout(false);
            this.gp_BG.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_BG;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
        private DevComponents.DotNetBar.ButtonX b_Accept;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.TextBox tb_NewName;
        private DevComponents.DotNetBar.LabelX lb_FacilityName;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}