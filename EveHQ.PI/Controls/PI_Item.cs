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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PI
{
    public partial class PI_Item : UserControl
    {
        public enum TypeKeyEnum { LaunchPad, CommandCenter, Other };
        public TypeKeyEnum TypeKey;
        public string ModName, catName, dstName, contName;
        public string ModInf, ERInf;
        public int typeID;
        public string DragType;

        public PI_Item()
        {
            InitializeComponent();
            ModName = "";
            catName = "";
            dstName = "";
            contName = "";
            ModInf = "";
            ERInf = "";
            DragType = "Move";
            TypeKey = TypeKeyEnum.Other;
            typeID = 0;
        }

        [Description("Sets Valid Item Type"),
         Category("Item Type Settings")]
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

        public void SetToolTipForItem(string ttip)
        {
            pi_ToolTip.SetToolTip(pb_PItem, ttip);
        }

        private void pb_PItem_DragDrop(object sender, DragEventArgs e)
        {
            //Panel destination = (Panel)sender;
            PI_Item piIn;
            ListViewItem LV;
            int keyLoc;

            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                LV = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                typeID = Convert.ToInt32(LV.SubItems[1].Text);
                keyLoc = LV.ImageIndex;
                catName = LV.Name.ToString();
                contName = this.Name;
                ModName = LV.Text.ToString();

                pi_ToolTip.SetToolTip(pb_PItem, ModName);

                if (!ModName.Contains("Command"))
                    pb_PItem.AllowDrop = false;

                LV.SubItems[1].Name = this.Name;

                if (LV.SubItems.Count > 3)
                    ModInf = LV.SubItems[3].Text;
                else
                    ModInf = "";

                SetImageForItem();
            }
            else if (e.Data.GetDataPresent(typeof(PI_Item)))
            {
                // Place or Copy Data into new location
                piIn = (PI_Item)e.Data.GetData(typeof(PI_Item));
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

                ModInf = piIn.ModInf;
                ERInf = piIn.ERInf;
                ModName = piIn.ModName;

                pi_ToolTip.SetToolTip(pb_PItem, ModName);

                pb_PItem.AllowDrop = false;

                SetImageForItem();
            }
        }

        private void pb_PItem_DragEnter(object sender, DragEventArgs e)
        {
            ListViewItem LV;
            PI_Item piIn;

            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                LV = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (LV.Name.Contains("Command"))
                {
                    if (TypeKey == TypeKeyEnum.CommandCenter)
                    {
                        e.Effect = DragDropEffects.Copy;
                        this.BorderStyle = BorderStyle.Fixed3D;
                    }
                }
                //else if (LV.Name.Contains("Launch"))
                //{
                //    if (TypeKey == TypeKeyEnum.LaunchPad)
                //    {
                //        e.Effect = DragDropEffects.Copy;
                //        this.BorderStyle = BorderStyle.Fixed3D;
                //    }
                //}
                else
                {
                    e.Effect = DragDropEffects.Copy;
                    this.BorderStyle = BorderStyle.Fixed3D;
                }
            }
            else if (e.Data.GetDataPresent(typeof(PI_Item)))
            {
                piIn = (PI_Item)e.Data.GetData(typeof(PI_Item));
                if ((TypeKey == TypeKeyEnum.Other) && (typeID == 0))
                {
                    e.Effect = DragDropEffects.Copy;
                    this.BorderStyle = BorderStyle.Fixed3D;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void SetImageForItem()
        {
            string imgLoc, imgKey;
            Bitmap bmp;

            imgLoc = EveHQ.Core.ImageHandler.GetImageLocation(typeID.ToString(), Convert.ToInt32(EveHQ.Core.ImageHandler.ImageType.Types));

            try
            {
                bmp = new Bitmap(Image.FromFile(imgLoc));
            }
            catch
            {
                imgKey = typeID.ToString() + ".png";
                if (il_ModImages.Images.ContainsKey(imgKey))
                    bmp = new Bitmap(il_ModImages.Images[imgKey]);
                else
                    bmp = new Bitmap(PI.Properties.Resources.noitem);
            }

            if (ModInf != "")
            {
                lb_ModInf.Show();
                lb_ModInf.BackColor = Color.Black;
                lb_ModInf.Text = ModInf;
            }
            else
            {
                lb_ModInf.Hide();
            }

            pb_PItem.BackgroundImageLayout = ImageLayout.Stretch;
            pb_PItem.BackgroundImage = bmp;

            if (ERInf != "")
            {
                lb_ERInf.Show();
                lb_ERInf.BackColor = Color.Black;
                lb_ERInf.Text = ERInf;
            }
            else
            {
                lb_ERInf.Hide();
            }

            if (!ModName.Contains("Command"))
                pb_PItem.AllowDrop = false;          
        }

        private void pb_PItem_DragLeave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private void pb_PItem_MouseHover(object sender, EventArgs e)
        {
            pi_ToolTip.Show(pi_ToolTip.GetToolTip(pb_PItem), pb_PItem, this.Width, 0, 3000);
        }

        private void pb_PItem_DragOver(object sender, DragEventArgs e)
        {
            if ((e.KeyState & 8) == 8)
                DragType = "Copy";
            else
                DragType = "Move";
        }

        private void pb_PItem_MouseEnter(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.Fixed3D;
        }

        private void pb_PItem_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void pb_PItem_MouseLeave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            pi_ToolTip.Hide(pb_PItem);
        }

        private void PI_Item_Load(object sender, EventArgs e)
        {
            pb_PItem.Image = null;
            pb_PItem.BackgroundImage = null;
            pb_PItem.BackgroundImageLayout = ImageLayout.Stretch;
            pb_PItem.BackColor = Color.Transparent;
            pb_PItem.AllowDrop = true;
        }

        public void RemoveItemFromControl()
        {
            pb_PItem.BackgroundImage = null;
            pb_PItem.BackColor = Color.Transparent;
            typeID = 0;
            catName = "";
            contName = "";
            ModName = "";
            ModInf = "";
            ERInf = "";
            pb_PItem.AllowDrop = true;
            pi_ToolTip.SetToolTip(pb_PItem, "");
            lb_ModInf.Text = ModInf;
            lb_ModInf.BackColor = Color.Transparent;
            lb_ERInf.Text = ERInf;
            lb_ERInf.BackColor = Color.Transparent;
        }

    }
}
