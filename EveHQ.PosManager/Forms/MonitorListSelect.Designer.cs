namespace EveHQ.PosManager
{
    partial class MonitorListSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorListSelect));
            this.clb_PosList = new System.Windows.Forms.CheckedListBox();
            this.b_done = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clb_PosList
            // 
            this.clb_PosList.CheckOnClick = true;
            this.clb_PosList.FormattingEnabled = true;
            this.clb_PosList.Location = new System.Drawing.Point(1, 1);
            this.clb_PosList.Name = "clb_PosList";
            this.clb_PosList.Size = new System.Drawing.Size(381, 244);
            this.clb_PosList.TabIndex = 0;
            // 
            // b_done
            // 
            this.b_done.Location = new System.Drawing.Point(148, 248);
            this.b_done.Name = "b_done";
            this.b_done.Size = new System.Drawing.Size(86, 29);
            this.b_done.TabIndex = 1;
            this.b_done.Text = "Done";
            this.b_done.UseVisualStyleBackColor = true;
            this.b_done.Click += new System.EventHandler(this.b_done_Click);
            // 
            // MonitorListSelect
            // 
            this.AcceptButton = this.b_done;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 279);
            this.Controls.Add(this.b_done);
            this.Controls.Add(this.clb_PosList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MonitorListSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select POS to Monitor";
            this.Shown += new System.EventHandler(this.MonitorListSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb_PosList;
        private System.Windows.Forms.Button b_done;
    }
}