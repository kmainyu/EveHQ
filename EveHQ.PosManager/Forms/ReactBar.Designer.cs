namespace EveHQ.PosManager
{
    partial class ReactBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pBar = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.SuspendLayout();
            // 
            // pBar
            // 
            // 
            // 
            // 
            this.pBar.BackgroundStyle.Class = "";
            this.pBar.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.pBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBar.Location = new System.Drawing.Point(0, 0);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(180, 17);
            this.pBar.TabIndex = 0;
            this.pBar.TextVisible = true;
            // 
            // ReactBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pBar);
            this.Enabled = false;
            this.Name = "ReactBar";
            this.Size = new System.Drawing.Size(180, 17);
            this.ResumeLayout(false);

        }

        #endregion

        public DevComponents.DotNetBar.Controls.ProgressBarX pBar;
    }
}
