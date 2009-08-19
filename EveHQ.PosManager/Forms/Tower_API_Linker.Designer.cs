namespace EveHQ.PosManager
{
    partial class Tower_API_Linker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tower_API_Linker));
            this.tv_PoSList = new System.Windows.Forms.TreeView();
            this.tv_APIList = new System.Windows.Forms.TreeView();
            this.b_LinkEm = new System.Windows.Forms.Button();
            this.b_UnLink = new System.Windows.Forms.Button();
            this.b_Done = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tv_PoSList
            // 
            this.tv_PoSList.FullRowSelect = true;
            this.tv_PoSList.HideSelection = false;
            this.tv_PoSList.Location = new System.Drawing.Point(7, 33);
            this.tv_PoSList.Name = "tv_PoSList";
            this.tv_PoSList.Size = new System.Drawing.Size(267, 341);
            this.tv_PoSList.TabIndex = 0;
            // 
            // tv_APIList
            // 
            this.tv_APIList.FullRowSelect = true;
            this.tv_APIList.HideSelection = false;
            this.tv_APIList.Location = new System.Drawing.Point(366, 33);
            this.tv_APIList.Name = "tv_APIList";
            this.tv_APIList.Size = new System.Drawing.Size(447, 341);
            this.tv_APIList.TabIndex = 1;
            // 
            // b_LinkEm
            // 
            this.b_LinkEm.Location = new System.Drawing.Point(283, 94);
            this.b_LinkEm.Name = "b_LinkEm";
            this.b_LinkEm.Size = new System.Drawing.Size(75, 36);
            this.b_LinkEm.TabIndex = 2;
            this.b_LinkEm.Text = "Link PoS with Tower";
            this.b_LinkEm.UseVisualStyleBackColor = true;
            this.b_LinkEm.Click += new System.EventHandler(this.b_LinkEm_Click);
            // 
            // b_UnLink
            // 
            this.b_UnLink.Location = new System.Drawing.Point(283, 171);
            this.b_UnLink.Name = "b_UnLink";
            this.b_UnLink.Size = new System.Drawing.Size(75, 36);
            this.b_UnLink.TabIndex = 3;
            this.b_UnLink.Text = "Remove Link";
            this.b_UnLink.UseVisualStyleBackColor = true;
            this.b_UnLink.Click += new System.EventHandler(this.b_UnLink_Click);
            // 
            // b_Done
            // 
            this.b_Done.Location = new System.Drawing.Point(283, 248);
            this.b_Done.Name = "b_Done";
            this.b_Done.Size = new System.Drawing.Size(75, 36);
            this.b_Done.TabIndex = 4;
            this.b_Done.Text = "Done";
            this.b_Done.UseVisualStyleBackColor = true;
            this.b_Done.Click += new System.EventHandler(this.b_Done_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "POS Listing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(366, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "API Tower Listing";
            // 
            // Tower_API_Linker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 379);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_Done);
            this.Controls.Add(this.b_UnLink);
            this.Controls.Add(this.b_LinkEm);
            this.Controls.Add(this.tv_APIList);
            this.Controls.Add(this.tv_PoSList);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Tower_API_Linker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select POS / API Tower Link";
            this.Load += new System.EventHandler(this.Tower_API_Linker_Load);
            this.Shown += new System.EventHandler(this.Tower_API_Linker_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tower_API_Linker_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tv_PoSList;
        private System.Windows.Forms.TreeView tv_APIList;
        private System.Windows.Forms.Button b_LinkEm;
        private System.Windows.Forms.Button b_UnLink;
        private System.Windows.Forms.Button b_Done;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}