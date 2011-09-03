namespace EveHQ.PosManager
{
    partial class Notification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notification));
            this.clb_PlayersToNotify = new System.Windows.Forms.CheckedListBox();
            this.nud_Initial = new System.Windows.Forms.NumericUpDown();
            this.nud_Frequency = new System.Windows.Forms.NumericUpDown();
            this.clb_TowersToNotify = new System.Windows.Forms.CheckedListBox();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cb_Frequency = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.cb_Initial = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.ci_Days = new DevComponents.Editors.ComboItem();
            this.ci_Hours = new DevComponents.Editors.ComboItem();
            this.cb_Type = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.ci_FuelLevel = new DevComponents.Editors.ComboItem();
            this.ci_SiloLevel = new DevComponents.Editors.ComboItem();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.b_Done = new DevComponents.DotNetBar.ButtonX();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Initial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Frequency)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // clb_PlayersToNotify
            // 
            this.clb_PlayersToNotify.CheckOnClick = true;
            this.clb_PlayersToNotify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clb_PlayersToNotify.FormattingEnabled = true;
            this.clb_PlayersToNotify.Location = new System.Drawing.Point(0, 0);
            this.clb_PlayersToNotify.Name = "clb_PlayersToNotify";
            this.clb_PlayersToNotify.Size = new System.Drawing.Size(279, 242);
            this.clb_PlayersToNotify.TabIndex = 9;
            // 
            // nud_Initial
            // 
            this.nud_Initial.Location = new System.Drawing.Point(385, 27);
            this.nud_Initial.Name = "nud_Initial";
            this.nud_Initial.Size = new System.Drawing.Size(120, 22);
            this.nud_Initial.TabIndex = 10;
            // 
            // nud_Frequency
            // 
            this.nud_Frequency.Location = new System.Drawing.Point(385, 52);
            this.nud_Frequency.Name = "nud_Frequency";
            this.nud_Frequency.Size = new System.Drawing.Size(120, 22);
            this.nud_Frequency.TabIndex = 11;
            // 
            // clb_TowersToNotify
            // 
            this.clb_TowersToNotify.CheckOnClick = true;
            this.clb_TowersToNotify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clb_TowersToNotify.FormattingEnabled = true;
            this.clb_TowersToNotify.Location = new System.Drawing.Point(0, 0);
            this.clb_TowersToNotify.Name = "clb_TowersToNotify";
            this.clb_TowersToNotify.Size = new System.Drawing.Size(386, 242);
            this.clb_TowersToNotify.TabIndex = 14;
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.b_Cancel);
            this.groupPanel1.Controls.Add(this.b_Done);
            this.groupPanel1.Controls.Add(this.cb_Frequency);
            this.groupPanel1.Controls.Add(this.cb_Initial);
            this.groupPanel1.Controls.Add(this.nud_Frequency);
            this.groupPanel1.Controls.Add(this.nud_Initial);
            this.groupPanel1.Controls.Add(this.cb_Type);
            this.groupPanel1.Controls.Add(this.groupPanel3);
            this.groupPanel1.Controls.Add(this.groupPanel2);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(704, 389);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = "";
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = "";
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = "";
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 16;
            // 
            // cb_Frequency
            // 
            this.cb_Frequency.DisplayMember = "Text";
            this.cb_Frequency.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_Frequency.FormattingEnabled = true;
            this.cb_Frequency.ItemHeight = 16;
            this.cb_Frequency.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cb_Frequency.Location = new System.Drawing.Point(258, 52);
            this.cb_Frequency.Name = "cb_Frequency";
            this.cb_Frequency.Size = new System.Drawing.Size(121, 22);
            this.cb_Frequency.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_Frequency.TabIndex = 20;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "Days";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "Hours";
            // 
            // cb_Initial
            // 
            this.cb_Initial.DisplayMember = "Text";
            this.cb_Initial.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_Initial.FormattingEnabled = true;
            this.cb_Initial.ItemHeight = 16;
            this.cb_Initial.Items.AddRange(new object[] {
            this.ci_Days,
            this.ci_Hours});
            this.cb_Initial.Location = new System.Drawing.Point(258, 27);
            this.cb_Initial.Name = "cb_Initial";
            this.cb_Initial.Size = new System.Drawing.Size(121, 22);
            this.cb_Initial.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_Initial.TabIndex = 19;
            // 
            // ci_Days
            // 
            this.ci_Days.Text = "Days";
            // 
            // ci_Hours
            // 
            this.ci_Hours.Text = "Hours";
            // 
            // cb_Type
            // 
            this.cb_Type.DisplayMember = "Text";
            this.cb_Type.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_Type.FormattingEnabled = true;
            this.cb_Type.ItemHeight = 16;
            this.cb_Type.Items.AddRange(new object[] {
            this.ci_FuelLevel,
            this.ci_SiloLevel});
            this.cb_Type.Location = new System.Drawing.Point(258, 2);
            this.cb_Type.Name = "cb_Type";
            this.cb_Type.Size = new System.Drawing.Size(121, 22);
            this.cb_Type.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_Type.TabIndex = 18;
            // 
            // ci_FuelLevel
            // 
            this.ci_FuelLevel.Text = "Fuel Level";
            // 
            // ci_SiloLevel
            // 
            this.ci_SiloLevel.Text = "Silo Level";
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.clb_PlayersToNotify);
            this.groupPanel3.Location = new System.Drawing.Point(408, 80);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(285, 265);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.Class = "";
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseDown.Class = "";
            this.groupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseOver.Class = "";
            this.groupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel3.TabIndex = 17;
            this.groupPanel3.Text = "Players To Notify";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.clb_TowersToNotify);
            this.groupPanel2.Location = new System.Drawing.Point(6, 80);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(392, 265);
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
            this.groupPanel2.TabIndex = 16;
            this.groupPanel2.Text = "Towers For Notification";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX3.ForeColor = System.Drawing.Color.Navy;
            this.labelX3.Location = new System.Drawing.Point(148, 50);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(107, 23);
            this.labelX3.TabIndex = 15;
            this.labelX3.Text = "Notify Frequency:";
            this.labelX3.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.Navy;
            this.labelX2.Location = new System.Drawing.Point(148, 26);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(107, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "Initial Notify:";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.ForeColor = System.Drawing.Color.Navy;
            this.labelX1.Location = new System.Drawing.Point(148, 2);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(107, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Type:";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // b_Done
            // 
            this.b_Done.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Done.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Done.Location = new System.Drawing.Point(180, 349);
            this.b_Done.Name = "b_Done";
            this.b_Done.Size = new System.Drawing.Size(75, 33);
            this.b_Done.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Done.TabIndex = 21;
            this.b_Done.Text = "&Done";
            this.b_Done.Click += new System.EventHandler(this.b_Done_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(444, 349);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 33);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 22;
            this.b_Cancel.Text = "&Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // Notification
            // 
            this.AcceptButton = this.b_Done;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(704, 389);
            this.Controls.Add(this.groupPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Notification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Notification";
            ((System.ComponentModel.ISupportInitialize)(this.nud_Initial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Frequency)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb_PlayersToNotify;
        private System.Windows.Forms.NumericUpDown nud_Initial;
        private System.Windows.Forms.NumericUpDown nud_Frequency;
        private System.Windows.Forms.CheckedListBox clb_TowersToNotify;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_Frequency;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_Initial;
        private DevComponents.Editors.ComboItem ci_Days;
        private DevComponents.Editors.ComboItem ci_Hours;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_Type;
        private DevComponents.Editors.ComboItem ci_FuelLevel;
        private DevComponents.Editors.ComboItem ci_SiloLevel;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
        private DevComponents.DotNetBar.ButtonX b_Done;
    }
}