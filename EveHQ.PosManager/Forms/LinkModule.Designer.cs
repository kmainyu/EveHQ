namespace EveHQ.PosManager
{
    partial class LinkModule
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
            this.components = new System.ComponentModel.Container();
            this.cms_ClearLink = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_ClearLink = new System.Windows.Forms.ToolStripMenuItem();
            this.lb_LinkTo = new DevComponents.DotNetBar.LabelX();
            this.lb_ModuleType = new DevComponents.DotNetBar.LabelX();
            this.pb_Module = new System.Windows.Forms.PictureBox();
            this.tv_ModItems = new System.Windows.Forms.TreeView();
            this.cms_ClearLink.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Module)).BeginInit();
            this.SuspendLayout();
            // 
            // cms_ClearLink
            // 
            this.cms_ClearLink.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_ClearLink});
            this.cms_ClearLink.Name = "cms_ClearLink";
            this.cms_ClearLink.Size = new System.Drawing.Size(127, 26);
            // 
            // tsmi_ClearLink
            // 
            this.tsmi_ClearLink.Name = "tsmi_ClearLink";
            this.tsmi_ClearLink.Size = new System.Drawing.Size(126, 22);
            this.tsmi_ClearLink.Text = "Clear Link";
            this.tsmi_ClearLink.Click += new System.EventHandler(this.tsmi_ClearLink_Click);
            // 
            // lb_LinkTo
            // 
            this.lb_LinkTo.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lb_LinkTo.BackgroundStyle.Class = "";
            this.lb_LinkTo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lb_LinkTo.ContextMenuStrip = this.cms_ClearLink;
            this.lb_LinkTo.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_LinkTo.ForeColor = System.Drawing.Color.Black;
            this.lb_LinkTo.Location = new System.Drawing.Point(3, 44);
            this.lb_LinkTo.Name = "lb_LinkTo";
            this.lb_LinkTo.Size = new System.Drawing.Size(234, 12);
            this.lb_LinkTo.TabIndex = 2;
            this.lb_LinkTo.Text = "Linked From/To";
            this.lb_LinkTo.Click += new System.EventHandler(this.LinkModule_Click);
            // 
            // lb_ModuleType
            // 
            this.lb_ModuleType.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lb_ModuleType.BackgroundStyle.Class = "";
            this.lb_ModuleType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lb_ModuleType.ContextMenuStrip = this.cms_ClearLink;
            this.lb_ModuleType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ModuleType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lb_ModuleType.Location = new System.Drawing.Point(46, 2);
            this.lb_ModuleType.Name = "lb_ModuleType";
            this.lb_ModuleType.Size = new System.Drawing.Size(187, 40);
            this.lb_ModuleType.TabIndex = 1;
            this.lb_ModuleType.Text = "Module Type";
            this.lb_ModuleType.TextLineAlignment = System.Drawing.StringAlignment.Near;
            this.lb_ModuleType.WordWrap = true;
            this.lb_ModuleType.Click += new System.EventHandler(this.LinkModule_Click);
            // 
            // pb_Module
            // 
            this.pb_Module.BackColor = System.Drawing.Color.Transparent;
            this.pb_Module.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_Module.ContextMenuStrip = this.cms_ClearLink;
            this.pb_Module.Location = new System.Drawing.Point(3, 2);
            this.pb_Module.Name = "pb_Module";
            this.pb_Module.Size = new System.Drawing.Size(40, 40);
            this.pb_Module.TabIndex = 0;
            this.pb_Module.TabStop = false;
            this.pb_Module.Click += new System.EventHandler(this.LinkModule_Click);
            // 
            // tv_ModItems
            // 
            this.tv_ModItems.Location = new System.Drawing.Point(2, 56);
            this.tv_ModItems.Name = "tv_ModItems";
            this.tv_ModItems.Size = new System.Drawing.Size(232, 81);
            this.tv_ModItems.TabIndex = 3;
            // 
            // LinkModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.cms_ClearLink;
            this.Controls.Add(this.tv_ModItems);
            this.Controls.Add(this.lb_LinkTo);
            this.Controls.Add(this.lb_ModuleType);
            this.Controls.Add(this.pb_Module);
            this.DoubleBuffered = true;
            this.Name = "LinkModule";
            this.Size = new System.Drawing.Size(238, 140);
            this.Click += new System.EventHandler(this.LinkModule_Click);
            this.cms_ClearLink.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Module)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lb_ModuleType;
        private System.Windows.Forms.PictureBox pb_Module;
        private DevComponents.DotNetBar.LabelX lb_LinkTo;
        private System.Windows.Forms.ContextMenuStrip cms_ClearLink;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ClearLink;
        private System.Windows.Forms.TreeView tv_ModItems;
    }
}
