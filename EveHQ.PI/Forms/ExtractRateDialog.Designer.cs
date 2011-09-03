namespace EveHQ.PI
{
    partial class ExtractRateDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractRateDialog));
            this.gp_BG = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.b_Accept = new DevComponents.DotNetBar.ButtonX();
            this.rb_Cycle60 = new System.Windows.Forms.RadioButton();
            this.rb_Cycle30 = new System.Windows.Forms.RadioButton();
            this.rb_Cycle15 = new System.Windows.Forms.RadioButton();
            this.rb_Cycle5 = new System.Windows.Forms.RadioButton();
            this.nud_ExtractRate = new System.Windows.Forms.NumericUpDown();
            this.gp_BG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ExtractRate)).BeginInit();
            this.SuspendLayout();
            // 
            // gp_BG
            // 
            this.gp_BG.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_BG.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_BG.Controls.Add(this.labelX2);
            this.gp_BG.Controls.Add(this.labelX1);
            this.gp_BG.Controls.Add(this.b_Cancel);
            this.gp_BG.Controls.Add(this.b_Accept);
            this.gp_BG.Controls.Add(this.rb_Cycle60);
            this.gp_BG.Controls.Add(this.rb_Cycle30);
            this.gp_BG.Controls.Add(this.rb_Cycle15);
            this.gp_BG.Controls.Add(this.rb_Cycle5);
            this.gp_BG.Controls.Add(this.nud_ExtractRate);
            this.gp_BG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_BG.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gp_BG.Location = new System.Drawing.Point(0, 0);
            this.gp_BG.Name = "gp_BG";
            this.gp_BG.Size = new System.Drawing.Size(290, 152);
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
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.Location = new System.Drawing.Point(78, 34);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(128, 23);
            this.labelX2.TabIndex = 8;
            this.labelX2.Text = "Extraction Cycle Time";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(3, 6);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(149, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "Extraction Amount Per Cycle:";
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(196, 103);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(76, 37);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 6;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // b_Accept
            // 
            this.b_Accept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Accept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Accept.Location = new System.Drawing.Point(9, 103);
            this.b_Accept.Name = "b_Accept";
            this.b_Accept.Size = new System.Drawing.Size(76, 37);
            this.b_Accept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Accept.TabIndex = 5;
            this.b_Accept.Text = "Accept";
            this.b_Accept.Click += new System.EventHandler(this.b_Accept_Click);
            // 
            // rb_Cycle60
            // 
            this.rb_Cycle60.AutoSize = true;
            this.rb_Cycle60.BackColor = System.Drawing.Color.Transparent;
            this.rb_Cycle60.Location = new System.Drawing.Point(147, 78);
            this.rb_Cycle60.Name = "rb_Cycle60";
            this.rb_Cycle60.Size = new System.Drawing.Size(77, 17);
            this.rb_Cycle60.TabIndex = 4;
            this.rb_Cycle60.TabStop = true;
            this.rb_Cycle60.Text = "60 Minutes";
            this.rb_Cycle60.UseVisualStyleBackColor = false;
            // 
            // rb_Cycle30
            // 
            this.rb_Cycle30.AutoSize = true;
            this.rb_Cycle30.BackColor = System.Drawing.Color.Transparent;
            this.rb_Cycle30.Location = new System.Drawing.Point(147, 55);
            this.rb_Cycle30.Name = "rb_Cycle30";
            this.rb_Cycle30.Size = new System.Drawing.Size(77, 17);
            this.rb_Cycle30.TabIndex = 3;
            this.rb_Cycle30.TabStop = true;
            this.rb_Cycle30.Text = "30 Minutes";
            this.rb_Cycle30.UseVisualStyleBackColor = false;
            // 
            // rb_Cycle15
            // 
            this.rb_Cycle15.AutoSize = true;
            this.rb_Cycle15.BackColor = System.Drawing.Color.Transparent;
            this.rb_Cycle15.Location = new System.Drawing.Point(61, 78);
            this.rb_Cycle15.Name = "rb_Cycle15";
            this.rb_Cycle15.Size = new System.Drawing.Size(77, 17);
            this.rb_Cycle15.TabIndex = 2;
            this.rb_Cycle15.TabStop = true;
            this.rb_Cycle15.Text = "15 Minutes";
            this.rb_Cycle15.UseVisualStyleBackColor = false;
            // 
            // rb_Cycle5
            // 
            this.rb_Cycle5.AutoSize = true;
            this.rb_Cycle5.BackColor = System.Drawing.Color.Transparent;
            this.rb_Cycle5.Location = new System.Drawing.Point(61, 55);
            this.rb_Cycle5.Name = "rb_Cycle5";
            this.rb_Cycle5.Size = new System.Drawing.Size(71, 17);
            this.rb_Cycle5.TabIndex = 1;
            this.rb_Cycle5.TabStop = true;
            this.rb_Cycle5.Text = "5 Minutes";
            this.rb_Cycle5.UseVisualStyleBackColor = false;
            // 
            // nud_ExtractRate
            // 
            this.nud_ExtractRate.ForeColor = System.Drawing.Color.Navy;
            this.nud_ExtractRate.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nud_ExtractRate.Location = new System.Drawing.Point(152, 7);
            this.nud_ExtractRate.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nud_ExtractRate.Name = "nud_ExtractRate";
            this.nud_ExtractRate.Size = new System.Drawing.Size(120, 21);
            this.nud_ExtractRate.TabIndex = 0;
            this.nud_ExtractRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nud_ExtractRate.ThousandsSeparator = true;
            // 
            // ExtractRateDialog
            // 
            this.AcceptButton = this.b_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(290, 152);
            this.Controls.Add(this.gp_BG);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Navy;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtractRateDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Extraction Rate";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ExtractRateDialog_Load);
            this.gp_BG.ResumeLayout(false);
            this.gp_BG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ExtractRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_BG;
        private System.Windows.Forms.NumericUpDown nud_ExtractRate;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
        private DevComponents.DotNetBar.ButtonX b_Accept;
        private System.Windows.Forms.RadioButton rb_Cycle60;
        private System.Windows.Forms.RadioButton rb_Cycle30;
        private System.Windows.Forms.RadioButton rb_Cycle15;
        private System.Windows.Forms.RadioButton rb_Cycle5;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}