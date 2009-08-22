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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_Tower = new System.Windows.Forms.ComboBox();
            this.cb_Type = new System.Windows.Forms.ComboBox();
            this.cb_Initial = new System.Windows.Forms.ComboBox();
            this.cb_Frequency = new System.Windows.Forms.ComboBox();
            this.clb_PlayersToNotify = new System.Windows.Forms.CheckedListBox();
            this.nud_Initial = new System.Windows.Forms.NumericUpDown();
            this.nud_Frequency = new System.Windows.Forms.NumericUpDown();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.b_Done = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Initial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Frequency)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tower:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "Initial Notify:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "Notify Frequency:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "Players To Notify:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cb_Tower
            // 
            this.cb_Tower.FormattingEnabled = true;
            this.cb_Tower.Location = new System.Drawing.Point(116, 8);
            this.cb_Tower.Name = "cb_Tower";
            this.cb_Tower.Size = new System.Drawing.Size(361, 22);
            this.cb_Tower.TabIndex = 5;
            // 
            // cb_Type
            // 
            this.cb_Type.FormattingEnabled = true;
            this.cb_Type.Items.AddRange(new object[] {
            "Fuel Level",
            "Silo Level"});
            this.cb_Type.Location = new System.Drawing.Point(116, 33);
            this.cb_Type.Name = "cb_Type";
            this.cb_Type.Size = new System.Drawing.Size(121, 22);
            this.cb_Type.TabIndex = 6;
            // 
            // cb_Initial
            // 
            this.cb_Initial.FormattingEnabled = true;
            this.cb_Initial.Items.AddRange(new object[] {
            "Days",
            "Hours"});
            this.cb_Initial.Location = new System.Drawing.Point(116, 58);
            this.cb_Initial.Name = "cb_Initial";
            this.cb_Initial.Size = new System.Drawing.Size(121, 22);
            this.cb_Initial.TabIndex = 7;
            // 
            // cb_Frequency
            // 
            this.cb_Frequency.FormattingEnabled = true;
            this.cb_Frequency.Items.AddRange(new object[] {
            "Days",
            "Hours"});
            this.cb_Frequency.Location = new System.Drawing.Point(116, 83);
            this.cb_Frequency.Name = "cb_Frequency";
            this.cb_Frequency.Size = new System.Drawing.Size(121, 22);
            this.cb_Frequency.TabIndex = 8;
            // 
            // clb_PlayersToNotify
            // 
            this.clb_PlayersToNotify.FormattingEnabled = true;
            this.clb_PlayersToNotify.Location = new System.Drawing.Point(9, 128);
            this.clb_PlayersToNotify.Name = "clb_PlayersToNotify";
            this.clb_PlayersToNotify.Size = new System.Drawing.Size(468, 242);
            this.clb_PlayersToNotify.TabIndex = 9;
            // 
            // nud_Initial
            // 
            this.nud_Initial.Location = new System.Drawing.Point(243, 58);
            this.nud_Initial.Name = "nud_Initial";
            this.nud_Initial.Size = new System.Drawing.Size(120, 22);
            this.nud_Initial.TabIndex = 10;
            // 
            // nud_Frequency
            // 
            this.nud_Frequency.Location = new System.Drawing.Point(243, 83);
            this.nud_Frequency.Name = "nud_Frequency";
            this.nud_Frequency.Size = new System.Drawing.Size(120, 22);
            this.nud_Frequency.TabIndex = 11;
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(318, 376);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 33);
            this.b_Cancel.TabIndex = 13;
            this.b_Cancel.Text = "&Cancel";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // b_Done
            // 
            this.b_Done.Location = new System.Drawing.Point(96, 376);
            this.b_Done.Name = "b_Done";
            this.b_Done.Size = new System.Drawing.Size(75, 33);
            this.b_Done.TabIndex = 12;
            this.b_Done.Text = "&Done";
            this.b_Done.UseVisualStyleBackColor = true;
            this.b_Done.Click += new System.EventHandler(this.b_Done_Click);
            // 
            // Notification
            // 
            this.AcceptButton = this.b_Done;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(489, 415);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Done);
            this.Controls.Add(this.nud_Frequency);
            this.Controls.Add(this.nud_Initial);
            this.Controls.Add(this.clb_PlayersToNotify);
            this.Controls.Add(this.cb_Frequency);
            this.Controls.Add(this.cb_Initial);
            this.Controls.Add(this.cb_Type);
            this.Controls.Add(this.cb_Tower);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Notification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Notification";
            ((System.ComponentModel.ISupportInitialize)(this.nud_Initial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Frequency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_Tower;
        private System.Windows.Forms.ComboBox cb_Type;
        private System.Windows.Forms.ComboBox cb_Initial;
        private System.Windows.Forms.ComboBox cb_Frequency;
        private System.Windows.Forms.CheckedListBox clb_PlayersToNotify;
        private System.Windows.Forms.NumericUpDown nud_Initial;
        private System.Windows.Forms.NumericUpDown nud_Frequency;
        private System.Windows.Forms.Button b_Cancel;
        private System.Windows.Forms.Button b_Done;
    }
}