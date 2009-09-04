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
            this.b_OK = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.cb_OwnerName = new System.Windows.Forms.ComboBox();
            this.cb_FuelTechName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gb_OwnerType = new System.Windows.Forms.GroupBox();
            this.rb_Corp = new System.Windows.Forms.RadioButton();
            this.rb_Personal = new System.Windows.Forms.RadioButton();
            this.gb_OwnerType.SuspendLayout();
            this.SuspendLayout();
            // 
            // b_OK
            // 
            this.b_OK.Location = new System.Drawing.Point(85, 110);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(83, 31);
            this.b_OK.TabIndex = 0;
            this.b_OK.Text = "OK";
            this.b_OK.UseVisualStyleBackColor = true;
            this.b_OK.Click += new System.EventHandler(this.b_OK_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(242, 110);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(83, 31);
            this.b_Cancel.TabIndex = 1;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // cb_OwnerName
            // 
            this.cb_OwnerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_OwnerName.ForeColor = System.Drawing.Color.Blue;
            this.cb_OwnerName.FormattingEnabled = true;
            this.cb_OwnerName.Location = new System.Drawing.Point(113, 51);
            this.cb_OwnerName.Name = "cb_OwnerName";
            this.cb_OwnerName.Size = new System.Drawing.Size(289, 24);
            this.cb_OwnerName.TabIndex = 2;
            this.cb_OwnerName.Text = "Select or Enter Name";
            // 
            // cb_FuelTechName
            // 
            this.cb_FuelTechName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_FuelTechName.ForeColor = System.Drawing.Color.Blue;
            this.cb_FuelTechName.FormattingEnabled = true;
            this.cb_FuelTechName.Location = new System.Drawing.Point(113, 81);
            this.cb_FuelTechName.Name = "cb_FuelTechName";
            this.cb_FuelTechName.Size = new System.Drawing.Size(289, 24);
            this.cb_FuelTechName.TabIndex = 3;
            this.cb_FuelTechName.Text = "Select or Enter Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(3, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select Owner";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(3, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select Fuel Tech";
            // 
            // gb_OwnerType
            // 
            this.gb_OwnerType.Controls.Add(this.rb_Personal);
            this.gb_OwnerType.Controls.Add(this.rb_Corp);
            this.gb_OwnerType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_OwnerType.Location = new System.Drawing.Point(105, 3);
            this.gb_OwnerType.Name = "gb_OwnerType";
            this.gb_OwnerType.Size = new System.Drawing.Size(200, 42);
            this.gb_OwnerType.TabIndex = 6;
            this.gb_OwnerType.TabStop = false;
            this.gb_OwnerType.Text = "Owner Type";
            // 
            // rb_Corp
            // 
            this.rb_Corp.AutoSize = true;
            this.rb_Corp.ForeColor = System.Drawing.Color.Blue;
            this.rb_Corp.Location = new System.Drawing.Point(6, 16);
            this.rb_Corp.Name = "rb_Corp";
            this.rb_Corp.Size = new System.Drawing.Size(96, 20);
            this.rb_Corp.TabIndex = 0;
            this.rb_Corp.TabStop = true;
            this.rb_Corp.Text = "Corporation";
            this.rb_Corp.UseVisualStyleBackColor = true;
            this.rb_Corp.CheckedChanged += new System.EventHandler(this.rb_Corp_CheckedChanged);
            // 
            // rb_Personal
            // 
            this.rb_Personal.AutoSize = true;
            this.rb_Personal.ForeColor = System.Drawing.Color.Blue;
            this.rb_Personal.Location = new System.Drawing.Point(111, 16);
            this.rb_Personal.Name = "rb_Personal";
            this.rb_Personal.Size = new System.Drawing.Size(80, 20);
            this.rb_Personal.TabIndex = 1;
            this.rb_Personal.TabStop = true;
            this.rb_Personal.Text = "Personal";
            this.rb_Personal.UseVisualStyleBackColor = true;
            this.rb_Personal.CheckedChanged += new System.EventHandler(this.rb_Personal_CheckedChanged);
            // 
            // OwnerFuelTech
            // 
            this.AcceptButton = this.b_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(411, 149);
            this.Controls.Add(this.gb_OwnerType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_FuelTechName);
            this.Controls.Add(this.cb_OwnerName);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_OK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OwnerFuelTech";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Owner for:";
            this.Load += new System.EventHandler(this.OwnerFuelTech_Load);
            this.gb_OwnerType.ResumeLayout(false);
            this.gb_OwnerType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_OK;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.ComboBox cb_OwnerName;
        private System.Windows.Forms.ComboBox cb_FuelTechName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gb_OwnerType;
        private System.Windows.Forms.RadioButton rb_Personal;
        private System.Windows.Forms.RadioButton rb_Corp;
    }
}