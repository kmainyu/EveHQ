namespace EveHQ.PosManager
{
    partial class PoS_Item
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PoS_Item));
            this.pbi_ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pb_Item = new System.Windows.Forms.PictureBox();
            this.il_defImage = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Item)).BeginInit();
            this.SuspendLayout();
            // 
            // pbi_ToolTip
            // 
            this.pbi_ToolTip.AutomaticDelay = 1000;
            this.pbi_ToolTip.AutoPopDelay = 3000;
            this.pbi_ToolTip.InitialDelay = 1000;
            this.pbi_ToolTip.ReshowDelay = 1000;
            this.pbi_ToolTip.ShowAlways = true;
            // 
            // pb_Item
            // 
            this.pb_Item.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_Item.BackColor = System.Drawing.Color.Transparent;
            this.pb_Item.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_Item.Location = new System.Drawing.Point(0, 0);
            this.pb_Item.Margin = new System.Windows.Forms.Padding(1);
            this.pb_Item.Name = "pb_Item";
            this.pb_Item.Size = new System.Drawing.Size(148, 148);
            this.pb_Item.TabIndex = 0;
            this.pb_Item.TabStop = false;
            this.pb_Item.MouseLeave += new System.EventHandler(this.PoS_Item_MouseLeave);
            this.pb_Item.DragOver += new System.Windows.Forms.DragEventHandler(this.pb_Item_DragOver);
            this.pb_Item.DragDrop += new System.Windows.Forms.DragEventHandler(this.pb_Item_DragDrop);
            this.pb_Item.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_Item_MouseDown);
            this.pb_Item.MouseHover += new System.EventHandler(this.pb_Item_MouseHover);
            this.pb_Item.DragLeave += new System.EventHandler(this.pb_Item_DragLeave);
            this.pb_Item.DragEnter += new System.Windows.Forms.DragEventHandler(this.pb_Item_DragEnter);
            this.pb_Item.MouseEnter += new System.EventHandler(this.PoS_Item_MouseEnter);
            // 
            // il_defImage
            // 
            this.il_defImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_defImage.ImageStream")));
            this.il_defImage.TransparentColor = System.Drawing.Color.Transparent;
            this.il_defImage.Images.SetKeyName(0, "noitem.jpg");
            // 
            // PoS_Item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pb_Item);
            this.Name = "PoS_Item";
            this.Size = new System.Drawing.Size(148, 148);
            this.Load += new System.EventHandler(this.PoS_Item_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Item)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_Item;
        public System.Windows.Forms.ToolTip pbi_ToolTip;
        private System.Windows.Forms.ImageList il_defImage;
    }
}
