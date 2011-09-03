// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of EveHQ.
//
// EveHQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace EveHQ.PosManager
{
    public partial class PoS_Item : UserControl
    {
        public long number;
        public long typeID;
        public string ModName, catName, dstName;
        public string contName;
        public Color onOff;
        public Color ColorBG;
        private Color ColorQty, ColorQtyBG;
        private Color ColorOnline, ColorOffline;
        public enum TypeKeyEnum { Outer, Tower, Inner };
        public TypeKeyEnum TypeKey;
        public string DragType;

        public PoS_Item()
        {
            InitializeComponent();
            number = 0;
            typeID = 0;
            catName = "";
            ModName = "";
            dstName = "";
            contName = "";
            DragType = "Move";
            onOff = Color.Transparent;
            TypeKey = TypeKeyEnum.Inner;
            ColorBG = Color.Transparent;
            pb_Item.BackgroundImage = null;
        }

        [Description("Sets Valid Item Type"),
         Category("Tower Item Settings")]
        public TypeKeyEnum ItemTypeKey
        {
            get
            {
                return TypeKey;
            }
            set
            {
                TypeKey = value;
            }
        }


        [Description("Sets Default Background Color"),
        Category("Tower Item Settings")]
        public Color DfltBackgroundColor
        {
            get
            {
                return ColorBG;
            }
            set
            {
                ColorBG = value;
            }
        }


        [Description("Sets Color of Quantity Text"),
         Category("Tower Item Settings")]
        public Color QuantityColor
        {
            get
            {
                return ColorQty;
            }
            set
            {
                ColorQty = value;
            }
        }

        [Description("Sets Color of Quantity Background"),
         Category("Tower Item Settings")]
        public Color QuantityColorBackground
        {
            get
            {
                return ColorQtyBG;
            }
            set
            {
                ColorQtyBG = value;
            }
        }

        [Description("Sets Color of Online Indicator"),
         Category("Tower Item Settings")]
        public Color OnlineColor
        {
            get
            {
                return ColorOnline;
            }
            set
            {
                ColorOnline = value;
            }
        }

        [Description("Sets Color of Offline Indicator"),
         Category("Tower Item Settings")]
        public Color OfflineColor
        {
            get
            {
                return ColorOffline;
            }
            set
            {
                ColorOffline = value;
            }
        }

        [Description("Sets Starting # for Item"),
         Category("Tower Item Settings")]
        public long ItemNumber
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        public void UpdateItemState()
        {
            SizeAndSetImage();
            //PutNumberAndStateIntoPicture(pb_Item.BackgroundImage, number, onOff);
        }

        private void PoS_Item_MouseEnter(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.Fixed3D;
        }

        private void PoS_Item_MouseLeave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            pbi_ToolTip.Hide(pb_Item);
        }

        private void PoS_Item_Load(object sender, EventArgs e)
        {
            pb_Item.Image = null;
            pb_Item.BackgroundImage = null;
            pb_Item.BackColor = ColorBG;
            pb_Item.AllowDrop = true;
        }

        private void pb_Item_DragEnter(object sender, DragEventArgs e)
        {
            ListViewItem LV;
            PoS_Item piIn;

            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                LV = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (LV.Name.Contains("Tower"))
                {
                    if(TypeKey == TypeKeyEnum.Tower)
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                }
                else if( (LV.Name.Contains("Cyno")) ||
                         (LV.Name.Contains("Jump Portal")) ||
                         (LV.Name.Contains("Battery")) ||
                         (LV.Name.Contains("Sentry")) )
                {
                    if (TypeKey == TypeKeyEnum.Outer)
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                }
                else
                {
                    if (TypeKey == TypeKeyEnum.Inner)
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                        e.Effect = DragDropEffects.None;
                }
            }
            else if (e.Data.GetDataPresent(typeof(PoS_Item)))
            {
                piIn = (PoS_Item)e.Data.GetData(typeof(PoS_Item));
                if (piIn.ModName.Contains("Tower"))
                {
                    if ((TypeKey == TypeKeyEnum.Tower) && (typeID == 0))
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                }
                else if ((piIn.ModName.Contains("Cyno")) ||
                         (piIn.ModName.Contains("Jump Portal")) ||
                         (piIn.ModName.Contains("Battery")) ||
                         (piIn.ModName.Contains("Sentry")))
                {
                    if ((TypeKey == TypeKeyEnum.Outer) && (typeID == 0))
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                }
                else
                {
                    if ((TypeKey == TypeKeyEnum.Inner) && (typeID == 0))
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                        e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pb_Item_DragDrop(object sender, DragEventArgs e)
        {
            //Panel destination = (Panel)sender;
            PoS_Item piIn;
            ListViewItem LV;
            long keyLoc;

            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                LV = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                typeID = Convert.ToInt32(LV.SubItems[1].Text);
                keyLoc = LV.ImageIndex;
                catName = LV.Name.ToString();
                contName = this.Name;
                ModName = LV.Text.ToString();

                pbi_ToolTip.SetToolTip(pb_Item, ModName);

                onOff = ColorOnline;
                number = 1;
                pb_Item.AllowDrop = false;

                LV.SubItems[1].Name = this.Name;
                UpdateItemState();
            }
            else if (e.Data.GetDataPresent(typeof(PoS_Item)))
            {
                // Place or Copy Data longo new location
                piIn = (PoS_Item)e.Data.GetData(typeof(PoS_Item));
                typeID = piIn.typeID;
                catName = piIn.catName;
                contName = this.Name;
                if (DragType == "Move")
                {
                    piIn.contName = contName;
                    piIn.DragType = "Move";
                }
                else
                {
                    piIn.dstName = contName;
                    piIn.DragType = "Copy";
                    DragType = "Move";
                }

                ModName = piIn.ModName;

                pbi_ToolTip.SetToolTip(pb_Item, ModName);

                onOff = piIn.onOff;
                number = piIn.number;
                pb_Item.AllowDrop = false;
                UpdateItemState();
            }
        }

        public void SetPBImage(Bitmap bmp)
        {
            pb_Item.BackgroundImage = null;
            pb_Item.BackgroundImage = bmp;
        }

        public void SizeAndSetImage()
        {
            Image img;
            Bitmap bmp;
            Graphics g;
            Rectangle dr, sr;

            img = EveHQ.Core.ImageHandler.GetImage(typeID.ToString(), 128);

            if (img == null)
            {
                bmp = new Bitmap(il_defImage.Images[0]);
            }
            else
                bmp = new Bitmap(img);

            g = Graphics.FromImage(bmp);

            sr = new Rectangle(0, 0, bmp.Width, bmp.Height);
            dr = new Rectangle(0, 0, pb_Item.Width, pb_Item.Height);

            //g.DrawImage(bmp, dr, sr, GraphicsUnit.Pixel);

            SolidBrush QtyBrush = new SolidBrush(ColorQty);
            g.FillRectangle(new SolidBrush(ColorQtyBG), 0, bmp.Height - 38, 38, 35);
            g.DrawString(number.ToString(), new Font("Arial", 20, FontStyle.Bold),
                QtyBrush, new PointF(-2, bmp.Height - 38));

            g.FillRectangle(new SolidBrush(onOff), bmp.Width - 25, bmp.Height - 25, 25, 25);

            g.Dispose();
            SetPBImage(bmp);
        }

        public void RemoveItemFromControl()
        {
            //this.BackColor = ColorLve;
            onOff = ColorBG;
            pb_Item.BackgroundImage = null;
            pb_Item.BackColor = ColorBG;
            number = 0;
            typeID = 0;
            catName = "";
            contName = "";
            ModName = "";
            pb_Item.AllowDrop = true;
            pbi_ToolTip.SetToolTip(pb_Item, "");
        }

        public void SetToolTipForItem(string ttip)
        {
            pbi_ToolTip.SetToolTip(pb_Item, ttip);
        }

        private void pb_Item_DragLeave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private void pb_OnOff_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void pb_Item_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void l_Number_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void pb_Item_MouseHover(object sender, EventArgs e)
        {
            pbi_ToolTip.Show(pbi_ToolTip.GetToolTip(pb_Item), pb_Item, this.Width, 0, 3000);
        }

        private void pb_Item_DragOver(object sender, DragEventArgs e)
        {
            if ((e.KeyState & 8) == 8)
                DragType = "Copy";
            else
                DragType = "Move";
        }

    }
}
