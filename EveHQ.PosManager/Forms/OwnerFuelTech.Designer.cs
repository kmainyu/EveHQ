namespace EveHQ.PosManager
{
    partial class OwnerFuelTech
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OwnerFuelTech));
            this.rb_Personal = new System.Windows.Forms.RadioButton();
            this.rb_Corp = new System.Windows.Forms.RadioButton();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cb_OwnerName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cb_FuelTechName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.b_OK = new DevComponents.DotNetBar.ButtonX();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rb_Personal
            // 
            this.rb_Personal.AutoSize = true;
            this.rb_Personal.BackColor = System.Drawing.Color.Transparent;
            this.rb_Personal.ForeColor = System.Drawing.Color.Blue;
            this.rb_Personal.Location = new System.Drawing.Point(222, 6);
            this.rb_Personal.Name = "rb_Personal";
            this.rb_Personal.Size = new System.Drawing.Size(66, 17);
            this.rb_Personal.TabIndex = 1;
            this.rb_Personal.TabStop = true;
            this.rb_Personal.Text = "Personal";
            this.rb_Personal.UseVisualStyleBackColor = false;
            this.rb_Personal.CheckedChanged += new System.EventHandler(this.rb_Personal_CheckedChanged);
            // 
            // rb_Corp
            // 
            this.rb_Corp.AutoSize = true;
            this.rb_Corp.BackColor = System.Drawing.Color.Transparent;
            this.rb_Corp.ForeColor = System.Drawing.Color.Blue;
            this.rb_Corp.Location = new System.Drawing.Point(117, 6);
            this.rb_Corp.Name = "rb_Corp";
            this.rb_Corp.Size = new System.Drawing.Size(79, 17);
            this.rb_Corp.TabIndex = 0;
            this.rb_Corp.TabStop = true;
            this.rb_Corp.Text = "Corporation";
            this.rb_Corp.UseVisualStyleBackColor = false;
            this.rb_Corp.CheckedChanged += new System.EventHandler(this.rb_Corp_CheckedChanged);
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.b_Cancel);
            this.groupPanel1.Controls.Add(this.b_OK);
            this.groupPanel1.Controls.Add(this.cb_FuelTechName);
            this.groupPanel1.Controls.Add(this.cb_OwnerName);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.rb_Personal);
            this.groupPanel1.Controls.Add(this.rb_Corp);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(411, 149);
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
            this.groupPanel1.TabIndex = 6;
            this.groupPanel1.Text = "Owner & Fuel Tech";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Navy;
            this.labelX1.Location = new System.Drawing.Point(19, 35);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(88, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Select Owner";
            this.labelX1.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.Color.Navy;
            this.labelX2.Location = new System.Drawing.Point(19, 63);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(88, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "Select Fuel Tech";
            this.labelX2.TextAlignment = System.Drawing.StringAlignment.Far;
            // 
            // cb_OwnerName
            // 
            this.cb_OwnerName.DisplayMember = "Text";
            this.cb_OwnerName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_OwnerName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_OwnerName.ForeColor = System.Drawing.Color.Navy;
            this.cb_OwnerName.FormattingEnabled = true;
            this.cb_OwnerName.ItemHeight = 17;
            this.cb_OwnerName.Location = new System.Drawing.Point(110, 35);
            this.cb_OwnerName.Name = "cb_OwnerName";
            this.cb_OwnerName.Size = new System.Drawing.Size(289, 23);
            this.cb_OwnerName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_OwnerName.TabIndex = 3;
            this.cb_OwnerName.Text = "Select or Enter Name";
            // 
            // cb_FuelTechName
            // 
            this.cb_FuelTechName.DisplayMember = "Text";
            this.cb_FuelTechName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_FuelTechName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_FuelTechName.ForeColor = System.Drawing.Color.Navy;
            this.cb_FuelTechName.FormattingEnabled = true;
            this.cb_FuelTechName.ItemHeight = 17;
            this.cb_FuelTechName.Location = new System.Drawing.Point(110, 63);
            this.cb_FuelTechName.Name = "cb_FuelTechName";
            this.cb_FuelTechName.Size = new System.Drawing.Size(289, 23);
            this.cb_FuelTechName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_FuelTechName.TabIndex = 4;
            this.cb_FuelTechName.Text = "Select or Enter Name";
            // 
            // b_OK
            // 
            this.b_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_OK.Location = new System.Drawing.Point(96, 93);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(83, 31);
            this.b_OK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_OK.TabIndex = 5;
            this.b_OK.Text = "OK";
            this.b_OK.Click += new System.EventHandler(this.b_OK_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.Location = new System.Drawing.Point(225, 93);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(83, 31);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 7;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // OwnerFuelTech
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 149);
            this.Controls.Add(this.groupPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OwnerFuelTech";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Owner for:";
            this.Load += new System.EventHandler(this.OwnerFuelTech_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rb_Personal;
        private System.Windows.Forms.RadioButton rb_Corp;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_FuelTechName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_OwnerName;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
        private DevComponents.DotNetBar.ButtonX b_OK;
    }
}