namespace EveHQ.RouteMap
{
    partial class MapView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapView));
            this.sb_HorizontalScroll = new System.Windows.Forms.HScrollBar();
            this.sb_VerticalScroll = new System.Windows.Forms.VScrollBar();
            this.cms_MapRClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_SetAsStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_SetAsDestination = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_AddAsWaypoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_AddToAvoid = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_ShowInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_HL_JB_Range = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_HL_ShipJumpRange = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_ShipJumpToRange = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_Zoom = new System.Windows.Forms.TextBox();
            this.b_ZoomIn = new System.Windows.Forms.Button();
            this.b_ZoomOut = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cb_FindSystem = new System.Windows.Forms.ComboBox();
            this.MV_Images = new System.Windows.Forms.ImageList(this.components);
            this.pd_MainMap = new System.Drawing.Printing.PrintDocument();
            this.ppd_MapPreview = new System.Windows.Forms.PrintPreviewDialog();
            this.pdlg_MapDialog = new System.Windows.Forms.PrintDialog();
            this.cms_MapRClick.SuspendLayout();
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
            // cms_MapRClick
            // 
            this.cms_MapRClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_SetAsStart,
            this.tsmi_SetAsDestination,
            this.tsmi_AddAsWaypoint,
            this.tsmi_AddToAvoid,
            this.tsmi_ShowInfo,
            this.tsmi_HL_JB_Range,
            this.tsmi_HL_ShipJumpRange,
            this.tsmi_ShipJumpToRange});
            this.cms_MapRClick.Name = "cms_MapRClick";
            this.cms_MapRClick.Size = new System.Drawing.Size(288, 202);
            // 
            // tsmi_SetAsStart
            // 
            this.tsmi_SetAsStart.Name = "tsmi_SetAsStart";
            this.tsmi_SetAsStart.Size = new System.Drawing.Size(287, 22);
            this.tsmi_SetAsStart.Text = "Set as &Start";
            this.tsmi_SetAsStart.Click += new System.EventHandler(this.tsmi_SetAsStart_Click);
            // 
            // tsmi_SetAsDestination
            // 
            this.tsmi_SetAsDestination.Name = "tsmi_SetAsDestination";
            this.tsmi_SetAsDestination.Size = new System.Drawing.Size(287, 22);
            this.tsmi_SetAsDestination.Text = "Set as &Destination";
            this.tsmi_SetAsDestination.Click += new System.EventHandler(this.tsmi_SetAsDestination_Click);
            // 
            // tsmi_AddAsWaypoint
            // 
            this.tsmi_AddAsWaypoint.Name = "tsmi_AddAsWaypoint";
            this.tsmi_AddAsWaypoint.Size = new System.Drawing.Size(287, 22);
            this.tsmi_AddAsWaypoint.Text = "Add as &Waypoint";
            this.tsmi_AddAsWaypoint.Click += new System.EventHandler(this.tsmi_AddAsWaypoint_Click);
            // 
            // tsmi_AddToAvoid
            // 
            this.tsmi_AddToAvoid.Name = "tsmi_AddToAvoid";
            this.tsmi_AddToAvoid.Size = new System.Drawing.Size(287, 22);
            this.tsmi_AddToAvoid.Text = "Add to &Avoid List";
            this.tsmi_AddToAvoid.Click += new System.EventHandler(this.tsmi_AddToAvoid_Click);
            // 
            // tsmi_ShowInfo
            // 
            this.tsmi_ShowInfo.Name = "tsmi_ShowInfo";
            this.tsmi_ShowInfo.Size = new System.Drawing.Size(287, 22);
            this.tsmi_ShowInfo.Text = "Show &Information";
            this.tsmi_ShowInfo.Click += new System.EventHandler(this.tsmi_ShowInfo_Click);
            // 
            // tsmi_HL_JB_Range
            // 
            this.tsmi_HL_JB_Range.Name = "tsmi_HL_JB_Range";
            this.tsmi_HL_JB_Range.Size = new System.Drawing.Size(287, 22);
            this.tsmi_HL_JB_Range.Text = "Enable Bridge Range Highlight";
            this.tsmi_HL_JB_Range.Click += new System.EventHandler(this.tsmi_HL_JB_Range_Click);
            // 
            // tsmi_HL_ShipJumpRange
            // 
            this.tsmi_HL_ShipJumpRange.Name = "tsmi_HL_ShipJumpRange";
            this.tsmi_HL_ShipJumpRange.Size = new System.Drawing.Size(287, 22);
            this.tsmi_HL_ShipJumpRange.Text = "Enable Ship Jump From Range Highlight";
            this.tsmi_HL_ShipJumpRange.Click += new System.EventHandler(this.tsmi_HL_ShipJumpRange_Click);
            // 
            // tsmi_ShipJumpToRange
            // 
            this.tsmi_ShipJumpToRange.Name = "tsmi_ShipJumpToRange";
            this.tsmi_ShipJumpToRange.Size = new System.Drawing.Size(287, 22);
            this.tsmi_ShipJumpToRange.Text = "Enable Ship Jump To Range Highlight";
            this.tsmi_ShipJumpToRange.Click += new System.EventHandler(this.tsmi_ShipJumpToRange_Click);
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
            // MV_Images
            // 
            this.MV_Images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MV_Images.ImageStream")));
            this.MV_Images.TransparentColor = System.Drawing.Color.Transparent;
            this.MV_Images.Images.SetKeyName(0, "jb_64_64.png");
            this.MV_Images.Images.SetKeyName(1, "TwrShld_64_64.png");
            this.MV_Images.Images.SetKeyName(2, "cg_64_64.png");
            this.MV_Images.Images.SetKeyName(3, "cyno.png");
            this.MV_Images.Images.SetKeyName(4, "cJ_64_64.png");
            // 
            // pd_MainMap
            // 
            this.pd_MainMap.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd_MainMap_PrintPage);
            // 
            // ppd_MapPreview
            // 
            this.ppd_MapPreview.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.ppd_MapPreview.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.ppd_MapPreview.ClientSize = new System.Drawing.Size(400, 300);
            this.ppd_MapPreview.Enabled = true;
            this.ppd_MapPreview.Icon = ((System.Drawing.Icon)(resources.GetObject("ppd_MapPreview.Icon")));
            this.ppd_MapPreview.Name = "ppd_MapPreview";
            this.ppd_MapPreview.UseAntiAlias = true;
            this.ppd_MapPreview.Visible = false;
            // 
            // pdlg_MapDialog
            // 
            this.pdlg_MapDialog.AllowCurrentPage = true;
            this.pdlg_MapDialog.AllowSelection = true;
            this.pdlg_MapDialog.AllowSomePages = true;
            this.pdlg_MapDialog.UseEXDialog = true;
            // 
            // MapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ContextMenuStrip = this.cms_MapRClick;
            this.Controls.Add(this.sb_VerticalScroll);
            this.Controls.Add(this.cb_FindSystem);
            this.Controls.Add(this.b_ZoomIn);
            this.Controls.Add(this.b_ZoomOut);
            this.Controls.Add(this.tb_Zoom);
            this.Controls.Add(this.sb_HorizontalScroll);
            this.DoubleBuffered = true;
            this.Name = "MapView";
            this.Size = new System.Drawing.Size(558, 450);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapViewControl_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapViewControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapViewControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapViewControl_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MapViewControl_MouseWheel);
            this.Resize += new System.EventHandler(this.MapView_Resize);
            this.cms_MapRClick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar sb_HorizontalScroll;
        private System.Windows.Forms.VScrollBar sb_VerticalScroll;
        private System.Windows.Forms.ContextMenuStrip cms_MapRClick;
        private System.Windows.Forms.ToolStripMenuItem tsmi_SetAsStart;
        private System.Windows.Forms.ToolStripMenuItem tsmi_SetAsDestination;
        private System.Windows.Forms.ToolStripMenuItem tsmi_AddAsWaypoint;
        private System.Windows.Forms.ToolStripMenuItem tsmi_AddToAvoid;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ShowInfo;
        private System.Windows.Forms.TextBox tb_Zoom;
        private System.Windows.Forms.Button b_ZoomOut;
        private System.Windows.Forms.Button b_ZoomIn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cb_FindSystem;
        private System.Windows.Forms.ImageList MV_Images;
        private System.Windows.Forms.ToolStripMenuItem tsmi_HL_JB_Range;
        private System.Windows.Forms.ToolStripMenuItem tsmi_HL_ShipJumpRange;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ShipJumpToRange;
        private System.Drawing.Printing.PrintDocument pd_MainMap;
        private System.Windows.Forms.PrintPreviewDialog ppd_MapPreview;
        private System.Windows.Forms.PrintDialog pdlg_MapDialog;
    }
}
