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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbx_Monitored = new System.Windows.Forms.CheckBox();
            this.b_Import = new System.Windows.Forms.Button();
            this.b_Cancel = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbx_Monitored);
            this.groupBox2.Location = new System.Drawing.Point(227, 263);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 96);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tower Monitor State";
            // 
            // cbx_Monitored
            // 
            this.cbx_Monitored.AutoSize = true;
            this.cbx_Monitored.Location = new System.Drawing.Point(6, 19);
            this.cbx_Monitored.Name = "cbx_Monitored";
            this.cbx_Monitored.Size = new System.Drawing.Size(144, 17);
            this.cbx_Monitored.TabIndex = 0;
            this.cbx_Monitored.Text = "Set Towers to Monitored";
            this.cbx_Monitored.UseVisualStyleBackColor = true;
            // 
            // b_Import
            // 
            this.b_Import.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_Import.Location = new System.Drawing.Point(94, 365);
            this.b_Import.Name = "b_Import";
            this.b_Import.Size = new System.Drawing.Size(87, 37);
            this.b_Import.TabIndex = 4;
            this.b_Import.Text = "Import";
            this.b_Import.UseVisualStyleBackColor = true;
            this.b_Import.Click += new System.EventHandler(this.b_Import_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_Cancel.Location = new System.Drawing.Point(248, 365);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(87, 37);
            this.b_Cancel.TabIndex = 5;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.UseVisualStyleBackColor = true;
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // PopulateTowersFromAPI
            // 
            this.AcceptButton = this.b_Import;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(429, 409);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_Import);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.clb_TowerListing);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PopulateTowersFromAPI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Populate Towers From API Listing";
            this.Load += new System.EventHandler(this.PopulateTowersFromAPI_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clb_TowerListing;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbx_Monitored;
        private System.Windows.Forms.Button b_Import;
        private System.Windows.Forms.Button b_Cancel;
    }
}