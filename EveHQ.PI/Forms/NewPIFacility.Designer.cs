namespace EveHQ.PI
{
    partial class NewPIFacility
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewPIFacility));
            this.gp_Background = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.b_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.b_Accept = new DevComponents.DotNetBar.ButtonX();
            this.pb_PlanetType = new System.Windows.Forms.PictureBox();
            this.cb_SystemPlanet = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.ci_Barren = new DevComponents.Editors.ComboItem();
            this.ci_Gas = new DevComponents.Editors.ComboItem();
            this.ci_Ice = new DevComponents.Editors.ComboItem();
            this.ci_Lava = new DevComponents.Editors.ComboItem();
            this.ci_Oceanic = new DevComponents.Editors.ComboItem();
            this.ci_Plasma = new DevComponents.Editors.ComboItem();
            this.ci_Storm = new DevComponents.Editors.ComboItem();
            this.ci_Temperate = new DevComponents.Editors.ComboItem();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.lb_name = new DevComponents.DotNetBar.LabelX();
            this.tb_FacilityName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.il_PlanetImages = new System.Windows.Forms.ImageList(this.components);
            this.lb_Description = new System.Windows.Forms.RichTextBox();
            this.gp_Background.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_PlanetType)).BeginInit();
            this.SuspendLayout();
            // 
            // gp_Background
            // 
            this.gp_Background.CanvasColor = System.Drawing.SystemColors.Control;
            this.gp_Background.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gp_Background.Controls.Add(this.lb_Description);
            this.gp_Background.Controls.Add(this.b_Cancel);
            this.gp_Background.Controls.Add(this.b_Accept);
            this.gp_Background.Controls.Add(this.pb_PlanetType);
            this.gp_Background.Controls.Add(this.cb_SystemPlanet);
            this.gp_Background.Controls.Add(this.labelX2);
            this.gp_Background.Controls.Add(this.lb_name);
            this.gp_Background.Controls.Add(this.tb_FacilityName);
            this.gp_Background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gp_Background.Location = new System.Drawing.Point(0, 0);
            this.gp_Background.Name = "gp_Background";
            this.gp_Background.Size = new System.Drawing.Size(555, 170);
            // 
            // 
            // 
            this.gp_Background.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_Background.Style.BackColorGradientAngle = 90;
            this.gp_Background.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_Background.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderBottomWidth = 1;
            this.gp_Background.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_Background.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderLeftWidth = 1;
            this.gp_Background.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderRightWidth = 1;
            this.gp_Background.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_Background.Style.BorderTopWidth = 1;
            this.gp_Background.Style.Class = "";
            this.gp_Background.Style.CornerDiameter = 4;
            this.gp_Background.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_Background.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_Background.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gp_Background.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_Background.StyleMouseDown.Class = "";
            this.gp_Background.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_Background.StyleMouseOver.Class = "";
            this.gp_Background.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_Background.TabIndex = 0;
            // 
            // b_Cancel
            // 
            this.b_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_Cancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_Cancel.ForeColor = System.Drawing.Color.Navy;
            this.b_Cancel.Location = new System.Drawing.Point(415, 131);
            this.b_Cancel.Name = "b_Cancel";
            this.b_Cancel.Size = new System.Drawing.Size(125, 30);
            this.b_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Cancel.TabIndex = 9;
            this.b_Cancel.Text = "Cancel";
            this.b_Cancel.Click += new System.EventHandler(this.b_Cancel_Click);
            // 
            // b_Accept
            // 
            this.b_Accept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.b_Accept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.b_Accept.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_Accept.ForeColor = System.Drawing.Color.Navy;
            this.b_Accept.Location = new System.Drawing.Point(415, 85);
            this.b_Accept.Name = "b_Accept";
            this.b_Accept.Size = new System.Drawing.Size(125, 30);
            this.b_Accept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.b_Accept.TabIndex = 8;
            this.b_Accept.Text = "Create New Facility";
            this.b_Accept.Click += new System.EventHandler(this.b_Accept_Click);
            // 
            // pb_PlanetType
            // 
            this.pb_PlanetType.BackColor = System.Drawing.Color.Black;
            this.pb_PlanetType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pb_PlanetType.Location = new System.Drawing.Point(3, 61);
            this.pb_PlanetType.Name = "pb_PlanetType";
            this.pb_PlanetType.Size = new System.Drawing.Size(100, 100);
            this.pb_PlanetType.TabIndex = 6;
            this.pb_PlanetType.TabStop = false;
            // 
            // cb_SystemPlanet
            // 
            this.cb_SystemPlanet.DisplayMember = "Text";
            this.cb_SystemPlanet.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cb_SystemPlanet.ForeColor = System.Drawing.Color.Navy;
            this.cb_SystemPlanet.FormattingEnabled = true;
            this.cb_SystemPlanet.ItemHeight = 15;
            this.cb_SystemPlanet.Items.AddRange(new object[] {
            this.ci_Barren,
            this.ci_Gas,
            this.ci_Ice,
            this.ci_Lava,
            this.ci_Oceanic,
            this.ci_Plasma,
            this.ci_Storm,
            this.ci_Temperate});
            this.cb_SystemPlanet.Location = new System.Drawing.Point(84, 35);
            this.cb_SystemPlanet.Name = "cb_SystemPlanet";
            this.cb_SystemPlanet.Size = new System.Drawing.Size(186, 21);
            this.cb_SystemPlanet.Sorted = true;
            this.cb_SystemPlanet.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cb_SystemPlanet.TabIndex = 5;
            this.cb_SystemPlanet.SelectedIndexChanged += new System.EventHandler(this.cb_SystemPlanet_SelectedIndexChanged);
            // 
            // ci_Barren
            // 
            this.ci_Barren.Image = global::EveHQ.PI.Properties.Resources.BarrenIcon;
            this.ci_Barren.Text = "Barren";
            // 
            // ci_Gas
            // 
            this.ci_Gas.Image = global::EveHQ.PI.Properties.Resources.GasIcon;
            this.ci_Gas.Text = "Gas";
            // 
            // ci_Ice
            // 
            this.ci_Ice.Image = global::EveHQ.PI.Properties.Resources.IceIcon;
            this.ci_Ice.Text = "Ice";
            // 
            // ci_Lava
            // 
            this.ci_Lava.Image = global::EveHQ.PI.Properties.Resources.LavaIcon;
            this.ci_Lava.Text = "Lava";
            // 
            // ci_Oceanic
            // 
            this.ci_Oceanic.Image = global::EveHQ.PI.Properties.Resources.OceanicIcon;
            this.ci_Oceanic.Text = "Oceanic";
            // 
            // ci_Plasma
            // 
            this.ci_Plasma.Image = global::EveHQ.PI.Properties.Resources.PlasmaIcon;
            this.ci_Plasma.Text = "Plasma";
            // 
            // ci_Storm
            // 
            this.ci_Storm.Image = global::EveHQ.PI.Properties.Resources.StormIcon;
            this.ci_Storm.Text = "Storm";
            // 
            // ci_Temperate
            // 
            this.ci_Temperate.Image = global::EveHQ.PI.Properties.Resources.TemperateIcon;
            this.ci_Temperate.Text = "Temperate";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.Color.Navy;
            this.labelX2.Location = new System.Drawing.Point(2, 35);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(80, 20);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "Planet Type:";
            // 
            // lb_name
            // 
            this.lb_name.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lb_name.BackgroundStyle.Class = "";
            this.lb_name.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lb_name.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_name.ForeColor = System.Drawing.Color.Navy;
            this.lb_name.Location = new System.Drawing.Point(3, 9);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(80, 20);
            this.lb_name.TabIndex = 1;
            this.lb_name.Text = "Facility Name:";
            // 
            // tb_FacilityName
            // 
            // 
            // 
            // 
            this.tb_FacilityName.Border.Class = "TextBoxBorder";
            this.tb_FacilityName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tb_FacilityName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_FacilityName.ForeColor = System.Drawing.Color.Navy;
            this.tb_FacilityName.Location = new System.Drawing.Point(84, 9);
            this.tb_FacilityName.Name = "tb_FacilityName";
            this.tb_FacilityName.Size = new System.Drawing.Size(460, 22);
            this.tb_FacilityName.TabIndex = 0;
            // 
            // il_PlanetImages
            // 
            this.il_PlanetImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_PlanetImages.ImageStream")));
            this.il_PlanetImages.TransparentColor = System.Drawing.Color.Transparent;
            this.il_PlanetImages.Images.SetKeyName(0, "BarrenLarge.png");
            this.il_PlanetImages.Images.SetKeyName(1, "GasLarge.png");
            this.il_PlanetImages.Images.SetKeyName(2, "IceLarge.png");
            this.il_PlanetImages.Images.SetKeyName(3, "LavaLarge.png");
            this.il_PlanetImages.Images.SetKeyName(4, "OceanicLarge.png");
            this.il_PlanetImages.Images.SetKeyName(5, "PlasmaLarge.png");
            this.il_PlanetImages.Images.SetKeyName(6, "StormLarge.png");
            this.il_PlanetImages.Images.SetKeyName(7, "TemperateLarge.png");
            // 
            // lb_Description
            // 
            this.lb_Description.BackColor = System.Drawing.Color.White;
            this.lb_Description.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lb_Description.ForeColor = System.Drawing.Color.Navy;
            this.lb_Description.Location = new System.Drawing.Point(109, 61);
            this.lb_Description.Name = "lb_Description";
            this.lb_Description.ReadOnly = true;
            this.lb_Description.Size = new System.Drawing.Size(300, 100);
            this.lb_Description.TabIndex = 10;
            this.lb_Description.Text = "";
            // 
            // NewPIFacility
            // 
            this.AcceptButton = this.b_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.b_Cancel;
            this.ClientSize = new System.Drawing.Size(555, 170);
            this.ControlBox = false;
            this.Controls.Add(this.gp_Background);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewPIFacility";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New PI Facility";
            this.TopMost = true;
            this.gp_Background.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_PlanetType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gp_Background;
        private DevComponents.DotNetBar.LabelX lb_name;
        private DevComponents.DotNetBar.Controls.TextBoxX tb_FacilityName;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_SystemPlanet;
        private System.Windows.Forms.PictureBox pb_PlanetType;
        private DevComponents.DotNetBar.ButtonX b_Cancel;
        private DevComponents.DotNetBar.ButtonX b_Accept;
        private DevComponents.Editors.ComboItem ci_Barren;
        private DevComponents.Editors.ComboItem ci_Gas;
        private DevComponents.Editors.ComboItem ci_Ice;
        private DevComponents.Editors.ComboItem ci_Lava;
        private DevComponents.Editors.ComboItem ci_Oceanic;
        private DevComponents.Editors.ComboItem ci_Plasma;
        private DevComponents.Editors.ComboItem ci_Storm;
        private DevComponents.Editors.ComboItem ci_Temperate;
        private System.Windows.Forms.ImageList il_PlanetImages;
        private System.Windows.Forms.RichTextBox lb_Description;
    }
}