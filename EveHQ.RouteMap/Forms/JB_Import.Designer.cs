namespace EveHQ.RouteMap
{
    partial class JB_Import
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JB_Import));
            this.rtb_DotLanImport = new System.Windows.Forms.RichTextBox();
            this.b_OK = new DevComponents.DotNetBar.ButtonX();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // rtb_DotLanImport
            // 
            this.rtb_DotLanImport.Dock = System.Windows.Forms.DockStyle.Top;
            this.rtb_DotLanImport.Location = new System.Drawing.Point(0, 0);
            this.rtb_DotLanImport.Name = "rtb_DotLanImport";
            this.rtb_DotLanImport.Size = new System.Drawing.Size(383, 267);
            this.rtb_DotLanImport.TabIndex = 0;
            this.rtb_DotLanImport.Text = "";
            // 
            // b_OK
            // 
            this.b_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_OK.Location = new System.Drawing.Point(65, 273);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(75, 23);
            this.b_OK.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_OK.TabIndex = 1;
            this.b_OK.Text = "Import";
            this.b_OK.Click += new System.EventHandler(this.b_OK_Click);
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Location = new System.Drawing.Point(243, 273);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(75, 23);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 2;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // JB_Import
            // 
            this.AcceptButton = this.b_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(383, 301);
            this.Controls.Add(this.b_Cancel);
            this.Controls.Add(this.b_OK);
            this.Controls.Add(this.rtb_DotLanImport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JB_Import";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import JB from DotLan";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_DotLanImport;
        private DevComponents.DotNetBar.ButtonX b_OK;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
    }
}