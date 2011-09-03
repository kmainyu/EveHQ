namespace EveHQ.RouteMap
{
    partial class SelAltNode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelAltNode));
            this.gp_BG = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // gp_BG
            // 
            this.gp_BG.AutoScroll = true;
            this.gp_BG.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_BG.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_BG.Dock = System.Windows.Forms.DockStyle.Top;
            this.gp_BG.Location = new System.Drawing.Point(0, 0);
            this.gp_BG.Name = "gp_BG";
            this.gp_BG.Size = new System.Drawing.Size(508, 555);
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
            this.b_Cancel.ForeColor = System.Drawing.Color.Blue;
            this.b_Cancel.Location = new System.Drawing.Point(211, 556);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(87, 27);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 1;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // SelAltNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(508, 584);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.gp_BG);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelAltNode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Alternate Route Node";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_BG;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
    }
}