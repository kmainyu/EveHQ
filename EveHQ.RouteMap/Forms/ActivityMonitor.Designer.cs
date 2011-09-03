namespace EveHQ.RouteMap
{
    partial class ActivityMonitor
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
            this.zg_SysMon = new ZedGraph.ZedGraphControl();
            this.cb_SystemSelect = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // zg_SysMon
            // 
            this.zg_SysMon.BackColor = System.Drawing.Color.Black;
            this.zg_SysMon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.zg_SysMon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zg_SysMon.IsAntiAlias = true;
            this.zg_SysMon.IsShowPointValues = true;
            this.zg_SysMon.IsZoomOnMouseCenter = true;
            this.zg_SysMon.Location = new System.Drawing.Point(0, 0);
            this.zg_SysMon.Name = "zg_SysMon";
            this.zg_SysMon.ScrollGrace = 0D;
            this.zg_SysMon.ScrollMaxX = 48D;
            this.zg_SysMon.ScrollMaxY = 0D;
            this.zg_SysMon.ScrollMaxY2 = 0D;
            this.zg_SysMon.ScrollMinX = 0D;
            this.zg_SysMon.ScrollMinY = 0D;
            this.zg_SysMon.ScrollMinY2 = 0D;
            this.zg_SysMon.Size = new System.Drawing.Size(350, 350);
            this.zg_SysMon.TabIndex = 2;
            this.zg_SysMon.PointValueEvent += new ZedGraph.ZedGraphControl.PointValueHandler(this.zg_SysMon_PointValueEvent);
            // 
            // cb_SystemSelect
            // 
            this.cb_SystemSelect.DisplayMember = "Text";
            this.cb_SystemSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_SystemSelect.ForeColor = System.Drawing.Color.Black;
            this.cb_SystemSelect.FormattingEnabled = true;
            this.cb_SystemSelect.ItemHeight = 14;
            this.cb_SystemSelect.Location = new System.Drawing.Point(3, 3);
            this.cb_SystemSelect.Name = "cb_SystemSelect";
            this.cb_SystemSelect.Size = new System.Drawing.Size(100, 20);
            this.cb_SystemSelect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_SystemSelect.TabIndex = 3;
            this.cb_SystemSelect.SelectedIndexChanged += new System.EventHandler(this.cb_SystemSelect_SelectedIndexChanged);
            this.cb_SystemSelect.TextChanged += new System.EventHandler(this.cb_SystemSelect_TextChanged);
            // 
            // ActivityMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cb_SystemSelect);
            this.Controls.Add(this.zg_SysMon);
            this.Name = "ActivityMonitor";
            this.Size = new System.Drawing.Size(350, 350);
            this.Load += new System.EventHandler(this.ActivityMonitor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zg_SysMon;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_SystemSelect;
    }
}
