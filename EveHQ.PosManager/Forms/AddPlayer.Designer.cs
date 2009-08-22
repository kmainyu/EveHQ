namespace EveHQ.PosManager
{
    partial class AddPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPlayer));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.b_Done = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.tb_Name = new System.Windows.Forms.TextBox();
            this.tb_Email1 = new System.Windows.Forms.TextBox();
            this.tb_Email2 = new System.Windows.Forms.TextBox();
            this.tb_Email3 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Email 1:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "Email 2:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "Email 3:";
            // 
            // b_Done
            // 
            this.b_Done.Location = new System.Drawing.Point(36, 124);
            this.b_Done.Name = "b_Done";
            this.b_Done.Size = new System.Drawing.Size(75, 33);
            this.b_Done.TabIndex = 4;
            this.b_Done.Text = "&Done";
            this.b_Done.UseVisualStyleBackColor = true;
            this.b_Done.Click += new System.EventHandler(this.b_Done_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(258, 124);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 33);
            this.b_Cancel.TabIndex = 5;
            this.b_Cancel.Text = "&Cancel";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // tb_Name
            // 
            this.tb_Name.Location = new System.Drawing.Point(78, 8);
            this.tb_Name.Name = "tb_Name";
            this.tb_Name.Size = new System.Drawing.Size(281, 21);
            this.tb_Name.TabIndex = 6;
            // 
            // tb_Email1
            // 
            this.tb_Email1.Location = new System.Drawing.Point(78, 35);
            this.tb_Email1.Name = "tb_Email1";
            this.tb_Email1.Size = new System.Drawing.Size(281, 21);
            this.tb_Email1.TabIndex = 7;
            // 
            // tb_Email2
            // 
            this.tb_Email2.Location = new System.Drawing.Point(78, 62);
            this.tb_Email2.Name = "tb_Email2";
            this.tb_Email2.Size = new System.Drawing.Size(281, 21);
            this.tb_Email2.TabIndex = 8;
            // 
            // tb_Email3
            // 
            this.tb_Email3.Location = new System.Drawing.Point(78, 89);
            this.tb_Email3.Name = "tb_Email3";
            this.tb_Email3.Size = new System.Drawing.Size(281, 21);
            this.tb_Email3.TabIndex = 9;
            // 
            // AddPlayer
            // 
            this.AcceptButton = this.b_Done;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(370, 172);
            this.Controls.Add(this.tb_Email3);
            this.Controls.Add(this.tb_Email2);
            this.Controls.Add(this.tb_Email1);
            this.Controls.Add(this.tb_Name);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Done);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddPlayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add or Edit Player";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button b_Done;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.TextBox tb_Name;
        private System.Windows.Forms.TextBox tb_Email1;
        private System.Windows.Forms.TextBox tb_Email2;
        private System.Windows.Forms.TextBox tb_Email3;
    }
}