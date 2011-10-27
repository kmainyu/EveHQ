namespace EveHQ.PosManager
{
    partial class PopulateTowersFromAPI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PopulateTowersFromAPI));
            this.label1 = new System.Windows.Forms.Label();
            this.clb_TowerListing = new System.Windows.Forms.CheckedListBox();
            this.cbx_Monitored = new System.Windows.Forms.CheckBox();
            this.b_Import = new DevComponents.DotNetBar.ButtonX();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(423, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "Caution - Adding PoS Towers from the API List can lead to duplicate designs if th" +
    "e name is in any way different. Towers and designs that are already linked will " +
    "not be shown.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // clb_TowerListing
            // 
            this.clb_TowerListing.CheckOnClick = true;
            this.clb_TowerListing.FormattingEnabled = true;
            this.clb_TowerListing.Location = new System.Drawing.Point(3, 58);
            this.clb_TowerListing.Name = "clb_TowerListing";
            this.clb_TowerListing.Size = new System.Drawing.Size(423, 196);
            this.clb_TowerListing.TabIndex = 1;
            // 
            // cbx_Monitored
            // 
            this.cbx_Monitored.AutoSize = true;
            this.cbx_Monitored.BackColor = System.Drawing.Color.Transparent;
            this.cbx_Monitored.Location = new System.Drawing.Point(6, 8);
            this.cbx_Monitored.Name = "cbx_Monitored";
            this.cbx_Monitored.Size = new System.Drawing.Size(144, 17);
            this.cbx_Monitored.TabIndex = 0;
            this.cbx_Monitored.Text = "Set Towers to Monitored";
            this.cbx_Monitored.UseVisualStyleBackColor = false;
            // 
            // b_Import
            // 
            this.b_Import.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Import.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Import.Location = new System.Drawing.Point(69, 365);
            this.b_Import.Name = "b_Import";
            this.b_Import.Size = new System.Drawing.Size(87, 37);
            this.b_Import.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Import.TabIndex = 6;
            this.b_Import.Text = "Import Selected Towers";
            this.b_Import.Click += new System.EventHandler(this.b_Import_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(272, 365);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(87, 37);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 7;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.cbx_Monitored);
            this.groupPanel2.Location = new System.Drawing.Point(241, 259);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(157, 100);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.Class = "";
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.Class = "";
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.Class = "";
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 9;
            this.groupPanel2.Text = "Tower Monitor State";
            // 
            // PopulateTowersFromAPI
            // 
            this.AcceptButton = this.b_Import;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(429, 409);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Import);
            this.Controls.Add(this.clb_TowerListing);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopulateTowersFromAPI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Populate Towers From API Listing";
            this.Load += new System.EventHandler(this.PopulateTowersFromAPI_Load);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clb_TowerListing;
        private System.Windows.Forms.CheckBox cbx_Monitored;
        private DevComponents.DotNetBar.ButtonX b_Import;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
    }
}