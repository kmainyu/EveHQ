namespace EveHQ.PosManager
{
    partial class POS_Name
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
            this.l_NameLabel = new System.Windows.Forms.Label();
            this.l_CurrentName = new System.Windows.Forms.Label();
            this.tb_NewName = new System.Windows.Forms.TextBox();
            this.l_EnterName = new System.Windows.Forms.Label();
            this.b_Done = new DevComponents.DotNetBar.ButtonX();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // l_NameLabel
            // 
            this.l_NameLabel.AutoSize = true;
            this.l_NameLabel.BackColor = System.Drawing.Color.Transparent;
            this.l_NameLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_NameLabel.ForeColor = System.Drawing.Color.Navy;
            this.l_NameLabel.Location = new System.Drawing.Point(3, 2);
            this.l_NameLabel.Name = "l_NameLabel";
            this.l_NameLabel.Size = new System.Drawing.Size(42, 14);
            this.l_NameLabel.TabIndex = 0;
            this.l_NameLabel.Text = "Name:";
            // 
            // l_CurrentName
            // 
            this.l_CurrentName.AutoSize = true;
            this.l_CurrentName.BackColor = System.Drawing.Color.Transparent;
            this.l_CurrentName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_CurrentName.ForeColor = System.Drawing.Color.Navy;
            this.l_CurrentName.Location = new System.Drawing.Point(50, 2);
            this.l_CurrentName.Name = "l_CurrentName";
            this.l_CurrentName.Size = new System.Drawing.Size(83, 14);
            this.l_CurrentName.TabIndex = 1;
            this.l_CurrentName.Text = "Current Name";
            // 
            // tb_NewName
            // 
            this.tb_NewName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_NewName.Location = new System.Drawing.Point(4, 41);
            this.tb_NewName.Name = "tb_NewName";
            this.tb_NewName.Size = new System.Drawing.Size(522, 21);
            this.tb_NewName.TabIndex = 2;
            // 
            // l_EnterName
            // 
            this.l_EnterName.AutoSize = true;
            this.l_EnterName.BackColor = System.Drawing.Color.Transparent;
            this.l_EnterName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_EnterName.ForeColor = System.Drawing.Color.Navy;
            this.l_EnterName.Location = new System.Drawing.Point(3, 21);
            this.l_EnterName.Name = "l_EnterName";
            this.l_EnterName.Size = new System.Drawing.Size(67, 13);
            this.l_EnterName.TabIndex = 3;
            this.l_EnterName.Text = "Enter Name:";
            // 
            // b_Done
            // 
            this.b_Done.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Done.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Done.Location = new System.Drawing.Point(163, 76);
            this.b_Done.Name = "b_Done";
            this.b_Done.Size = new System.Drawing.Size(75, 29);
            this.b_Done.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Done.TabIndex = 6;
            this.b_Done.Text = "Done";
            this.b_Done.Click += new System.EventHandler(this.b_Done_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(292, 76);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 29);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 7;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.l_NameLabel);
            this.groupPanel1.Controls.Add(this.l_CurrentName);
            this.groupPanel1.Controls.Add(this.l_EnterName);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(530, 119);
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
            this.groupPanel1.TabIndex = 8;
            // 
            // POS_Name
            // 
            this.AcceptButton = this.b_Done;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(530, 119);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Done);
            this.Controls.Add(this.tb_NewName);
            this.Controls.Add(this.groupPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "POS_Name";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit POS Name or Add New POS";
            this.Load += new System.EventHandler(this.POS_Name_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label l_NameLabel;
        private System.Windows.Forms.Label l_CurrentName;
        private System.Windows.Forms.TextBox tb_NewName;
        private System.Windows.Forms.Label l_EnterName;
        private DevComponents.DotNetBar.ButtonX b_Done;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
    }
}