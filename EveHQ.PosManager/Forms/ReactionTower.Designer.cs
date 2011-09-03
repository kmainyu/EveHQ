namespace EveHQ.PosManager
{
    partial class ReactionTower
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
            this.gp_TwrReactBG = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.l_Location = new DevComponents.DotNetBar.LabelX();
            this.gp_TwrReactBG.SuspendLayout();
            this.SuspendLayout();
            // 
            // gp_TwrReactBG
            // 
            this.gp_TwrReactBG.AutoScroll = true;
            this.gp_TwrReactBG.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gp_TwrReactBG.BackColor = System.Drawing.Color.Transparent;
            this.gp_TwrReactBG.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_TwrReactBG.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_TwrReactBG.Controls.Add(this.l_Location);
            this.gp_TwrReactBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_TwrReactBG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gp_TwrReactBG.Location = new System.Drawing.Point(0, 0);
            this.gp_TwrReactBG.Name = "gp_TwrReactBG";
            this.gp_TwrReactBG.Size = new System.Drawing.Size(205, 115);
            // 
            // 
            // 
            this.gp_TwrReactBG.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_TwrReactBG.Style.BackColorGradientAngle = 90;
            this.gp_TwrReactBG.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_TwrReactBG.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_TwrReactBG.Style.BorderBottomWidth = 1;
            this.gp_TwrReactBG.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_TwrReactBG.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_TwrReactBG.Style.BorderLeftWidth = 1;
            this.gp_TwrReactBG.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_TwrReactBG.Style.BorderRightWidth = 1;
            this.gp_TwrReactBG.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_TwrReactBG.Style.BorderTopWidth = 1;
            this.gp_TwrReactBG.Style.Class = "";
            this.gp_TwrReactBG.Style.CornerDiameter = 4;
            this.gp_TwrReactBG.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_TwrReactBG.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_TwrReactBG.Style.TextColor = System.Drawing.Color.BlueViolet;
            this.gp_TwrReactBG.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_TwrReactBG.StyleMouseDown.Class = "";
            this.gp_TwrReactBG.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_TwrReactBG.StyleMouseOver.Class = "";
            this.gp_TwrReactBG.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_TwrReactBG.TabIndex = 0;
            this.gp_TwrReactBG.Text = "Tower  Name";
            this.gp_TwrReactBG.Click += new System.EventHandler(this.gp_TwrReactBG_Click);
            // 
            // l_Location
            // 
            // 
            // 
            // 
            this.l_Location.BackgroundStyle.Class = "";
            this.l_Location.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.l_Location.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_Location.ForeColor = System.Drawing.Color.Teal;
            this.l_Location.Location = new System.Drawing.Point(0, -1);
            this.l_Location.Name = "l_Location";
            this.l_Location.Size = new System.Drawing.Size(180, 14);
            this.l_Location.TabIndex = 1;
            this.l_Location.Text = "Tower Location";
            this.l_Location.TextAlignment = System.Drawing.StringAlignment.Center;
            this.l_Location.Click += new System.EventHandler(this.gp_TwrReactBG_Click);
            // 
            // ReactionTower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gp_TwrReactBG);
            this.DoubleBuffered = true;
            this.Name = "ReactionTower";
            this.Size = new System.Drawing.Size(205, 115);
            this.gp_TwrReactBG.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_TwrReactBG;
        private DevComponents.DotNetBar.LabelX l_Location;
    }
}
