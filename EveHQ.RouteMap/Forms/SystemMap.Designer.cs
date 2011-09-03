namespace EveHQ.RouteMap
{
    partial class SystemMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemMap));
            this.sb_HorizontalScroll = new System.Windows.Forms.HScrollBar();
            this.sb_VerticalScroll = new System.Windows.Forms.VScrollBar();
            this.tb_Zoom = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cb_FindSystem = new System.Windows.Forms.ComboBox();
            this.b_RotateClockwise = new System.Windows.Forms.Button();
            this.b_RotateCClockwise = new System.Windows.Forms.Button();
            this.b_ZoomIn = new System.Windows.Forms.Button();
            this.b_ZoomOut = new System.Windows.Forms.Button();
            this.SpatialIconList = new System.Windows.Forms.ImageList(this.components);
            this.cms_GetDistance = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_CalculateDistance = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_GetDistance.SuspendLayout();
            this.SuspendLayout();
            // 
            // sb_HorizontalScroll
            // 
            this.sb_HorizontalScroll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sb_HorizontalScroll.Location = new System.Drawing.Point(0, 433);
            this.sb_HorizontalScroll.Name = "sb_HorizontalScroll";
            this.sb_HorizontalScroll.Size = new System.Drawing.Size(558, 17);
            this.sb_HorizontalScroll.TabIndex = 3;
            this.sb_HorizontalScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sb_HorizontalScroll_Scroll);
            // 
            // sb_VerticalScroll
            // 
            this.sb_VerticalScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.sb_VerticalScroll.Location = new System.Drawing.Point(541, 0);
            this.sb_VerticalScroll.Name = "sb_VerticalScroll";
            this.sb_VerticalScroll.Size = new System.Drawing.Size(17, 433);
            this.sb_VerticalScroll.TabIndex = 4;
            this.sb_VerticalScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sb_VerticalScroll_Scroll);
            // 
            // tb_Zoom
            // 
            this.tb_Zoom.BackColor = System.Drawing.Color.Black;
            this.tb_Zoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_Zoom.ForeColor = System.Drawing.Color.Cyan;
            this.tb_Zoom.Location = new System.Drawing.Point(19, 0);
            this.tb_Zoom.Name = "tb_Zoom";
            this.tb_Zoom.ReadOnly = true;
            this.tb_Zoom.Size = new System.Drawing.Size(48, 20);
            this.tb_Zoom.TabIndex = 5;
            this.tb_Zoom.Text = "3.5";
            this.tb_Zoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cb_FindSystem
            // 
            this.cb_FindSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cb_FindSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_FindSystem.BackColor = System.Drawing.Color.Black;
            this.cb_FindSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cb_FindSystem.ForeColor = System.Drawing.Color.Cyan;
            this.cb_FindSystem.FormattingEnabled = true;
            this.cb_FindSystem.Location = new System.Drawing.Point(85, 0);
            this.cb_FindSystem.MaxDropDownItems = 20;
            this.cb_FindSystem.Name = "cb_FindSystem";
            this.cb_FindSystem.Size = new System.Drawing.Size(100, 21);
            this.cb_FindSystem.Sorted = true;
            this.cb_FindSystem.TabIndex = 8;
            this.toolTip1.SetToolTip(this.cb_FindSystem, "Type in System Name to Search and Center a Solar System on the Map/");
            this.cb_FindSystem.SelectedIndexChanged += new System.EventHandler(this.cb_FindSystem_SelectedIndexChanged);
            // 
            // b_RotateClockwise
            // 
            this.b_RotateClockwise.Image = global::EveHQ.RouteMap.Properties.Resources.redo;
            this.b_RotateClockwise.Location = new System.Drawing.Point(204, 0);
            this.b_RotateClockwise.Name = "b_RotateClockwise";
            this.b_RotateClockwise.Size = new System.Drawing.Size(20, 20);
            this.b_RotateClockwise.TabIndex = 10;
            this.toolTip1.SetToolTip(this.b_RotateClockwise, "Rotate Orbit (CTRL 100x, ALT 10x)");
            this.b_RotateClockwise.UseVisualStyleBackColor = true;
            this.b_RotateClockwise.Click += new System.EventHandler(this.b_RotateClockwise_Click);
            // 
            // b_RotateCClockwise
            // 
            this.b_RotateCClockwise.Image = global::EveHQ.RouteMap.Properties.Resources.undo;
            this.b_RotateCClockwise.Location = new System.Drawing.Point(185, 0);
            this.b_RotateCClockwise.Name = "b_RotateCClockwise";
            this.b_RotateCClockwise.Size = new System.Drawing.Size(20, 20);
            this.b_RotateCClockwise.TabIndex = 9;
            this.toolTip1.SetToolTip(this.b_RotateCClockwise, "Rotate Orbit (CTRL 100x, ALT 10x)");
            this.b_RotateCClockwise.UseVisualStyleBackColor = true;
            this.b_RotateCClockwise.Click += new System.EventHandler(this.b_RotateCClockwise_Click);
            // 
            // b_ZoomIn
            // 
            this.b_ZoomIn.Image = global::EveHQ.RouteMap.Properties.Resources.MapscalePlusIcon;
            this.b_ZoomIn.Location = new System.Drawing.Point(65, 0);
            this.b_ZoomIn.Name = "b_ZoomIn";
            this.b_ZoomIn.Size = new System.Drawing.Size(20, 20);
            this.b_ZoomIn.TabIndex = 7;
            this.toolTip1.SetToolTip(this.b_ZoomIn, "Zoom In (1%, CTRL 10%, ALT 0.5%)");
            this.b_ZoomIn.UseVisualStyleBackColor = true;
            this.b_ZoomIn.Click += new System.EventHandler(this.b_ZoomIn_Click);
            // 
            // b_ZoomOut
            // 
            this.b_ZoomOut.Image = global::EveHQ.RouteMap.Properties.Resources.MapscaleMinusIcon;
            this.b_ZoomOut.Location = new System.Drawing.Point(-1, 0);
            this.b_ZoomOut.Name = "b_ZoomOut";
            this.b_ZoomOut.Size = new System.Drawing.Size(20, 20);
            this.b_ZoomOut.TabIndex = 6;
            this.toolTip1.SetToolTip(this.b_ZoomOut, "Zoom Out (1%, CTRL 10%, ALT 0.5%)");
            this.b_ZoomOut.UseVisualStyleBackColor = true;
            this.b_ZoomOut.Click += new System.EventHandler(this.b_ZoomOut_Click);
            // 
            // SpatialIconList
            // 
            this.SpatialIconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SpatialIconList.ImageStream")));
            this.SpatialIconList.TransparentColor = System.Drawing.Color.Transparent;
            this.SpatialIconList.Images.SetKeyName(0, "cg_64_64.png");
            this.SpatialIconList.Images.SetKeyName(1, "jb_64_64.png");
            this.SpatialIconList.Images.SetKeyName(2, "Stargate.png");
            this.SpatialIconList.Images.SetKeyName(3, "moon.png");
            this.SpatialIconList.Images.SetKeyName(4, "BarrenLarge.png");
            this.SpatialIconList.Images.SetKeyName(5, "GasLarge.png");
            this.SpatialIconList.Images.SetKeyName(6, "IceLarge.png");
            this.SpatialIconList.Images.SetKeyName(7, "LavaLarge.png");
            this.SpatialIconList.Images.SetKeyName(8, "OceanicLarge.png");
            this.SpatialIconList.Images.SetKeyName(9, "PlasmaLarge.png");
            this.SpatialIconList.Images.SetKeyName(10, "shattered.png");
            this.SpatialIconList.Images.SetKeyName(11, "StormLarge.png");
            this.SpatialIconList.Images.SetKeyName(12, "TemperateLarge.png");
            this.SpatialIconList.Images.SetKeyName(13, "blue_sun.png");
            this.SpatialIconList.Images.SetKeyName(14, "orange_sun.png");
            this.SpatialIconList.Images.SetKeyName(15, "pink_sun.png");
            this.SpatialIconList.Images.SetKeyName(16, "jb_64_64.png");
            this.SpatialIconList.Images.SetKeyName(17, "red_sun.png");
            this.SpatialIconList.Images.SetKeyName(18, "white_sun.png");
            this.SpatialIconList.Images.SetKeyName(19, "yellow_sun.png");
            this.SpatialIconList.Images.SetKeyName(20, "mini_64_64.png");
            this.SpatialIconList.Images.SetKeyName(21, "amarr_64_64.png");
            this.SpatialIconList.Images.SetKeyName(22, "cald_64_64.png");
            this.SpatialIconList.Images.SetKeyName(23, "gal_64_64.png");
            this.SpatialIconList.Images.SetKeyName(24, "cJ_64_64.png");
            // 
            // cms_GetDistance
            // 
            this.cms_GetDistance.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_CalculateDistance});
            this.cms_GetDistance.Name = "cms_GetDistance";
            this.cms_GetDistance.Size = new System.Drawing.Size(192, 26);
            // 
            // tsmi_CalculateDistance
            // 
            this.tsmi_CalculateDistance.Name = "tsmi_CalculateDistance";
            this.tsmi_CalculateDistance.Size = new System.Drawing.Size(191, 22);
            this.tsmi_CalculateDistance.Text = "Calculate Distance To:";
            this.tsmi_CalculateDistance.Click += new System.EventHandler(this.tsmi_CalculateDistance_Click);
            // 
            // SystemMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ContextMenuStrip = this.cms_GetDistance;
            this.Controls.Add(this.b_RotateClockwise);
            this.Controls.Add(this.b_RotateCClockwise);
            this.Controls.Add(this.sb_VerticalScroll);
            this.Controls.Add(this.cb_FindSystem);
            this.Controls.Add(this.b_ZoomIn);
            this.Controls.Add(this.b_ZoomOut);
            this.Controls.Add(this.tb_Zoom);
            this.Controls.Add(this.sb_HorizontalScroll);
            this.DoubleBuffered = true;
            this.Name = "SystemMap";
            this.Size = new System.Drawing.Size(558, 450);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.SystemMapControl_MouseWheel);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SystemMapControl_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SystemMapControl_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SystemMapControl_MouseDown);
            this.Resize += new System.EventHandler(this.SystemMap_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SystemMapControl_MouseUp);
            this.cms_GetDistance.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar sb_HorizontalScroll;
        private System.Windows.Forms.VScrollBar sb_VerticalScroll;
        private System.Windows.Forms.TextBox tb_Zoom;
        private System.Windows.Forms.Button b_ZoomOut;
        private System.Windows.Forms.Button b_ZoomIn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cb_FindSystem;
        private System.Windows.Forms.Button b_RotateClockwise;
        private System.Windows.Forms.Button b_RotateCClockwise;
        private System.Windows.Forms.ImageList SpatialIconList;
        private System.Windows.Forms.ContextMenuStrip cms_GetDistance;
        private System.Windows.Forms.ToolStripMenuItem tsmi_CalculateDistance;
    }
}
