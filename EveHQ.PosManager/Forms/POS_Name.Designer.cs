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
            this.b_Done = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // l_NameLabel
            // 
            this.l_NameLabel.AutoSize = true;
            this.l_NameLabel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_NameLabel.Location = new System.Drawing.Point(6, 6);
            this.l_NameLabel.Name = "l_NameLabel";
            this.l_NameLabel.Size = new System.Drawing.Size(42, 14);
            this.l_NameLabel.TabIndex = 0;
            this.l_NameLabel.Text = "Name:";
            // 
            // l_CurrentName
            // 
            this.l_CurrentName.AutoSize = true;
            this.l_CurrentName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_CurrentName.Location = new System.Drawing.Point(53, 6);
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
            this.l_EnterName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_EnterName.Location = new System.Drawing.Point(6, 25);
            this.l_EnterName.Name = "l_EnterName";
            this.l_EnterName.Size = new System.Drawing.Size(67, 13);
            this.l_EnterName.TabIndex = 3;
            this.l_EnterName.Text = "Enter Name:";
            // 
            // b_Done
            // 
            this.b_Done.Location = new System.Drawing.Point(187, 93);
            this.b_Done.Name = "b_Done";
            this.b_Done.Size = new System.Drawing.Size(75, 23);
            this.b_Done.TabIndex = 4;
            this.b_Done.Text = "Done";
            this.b_Done.UseVisualStyleBackColor = true;
            this.b_Done.Click += new System.EventHandler(this.b_Done_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(268, 93);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.TabIndex = 5;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // POS_Name
            // 
            this.AcceptButton = this.b_Done;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(530, 119);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Done);
            this.Controls.Add(this.l_EnterName);
            this.Controls.Add(this.tb_NewName);
            this.Controls.Add(this.l_CurrentName);
            this.Controls.Add(this.l_NameLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "POS_Name";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit POS Name or Add New POS";
            this.Load += new System.EventHandler(this.POS_Name_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label l_NameLabel;
        private System.Windows.Forms.Label l_CurrentName;
        private System.Windows.Forms.TextBox tb_NewName;
        private System.Windows.Forms.Label l_EnterName;
        private System.Windows.Forms.Button b_Done;
        private System.Windows.Forms.Button b_Cancel;
    }
}