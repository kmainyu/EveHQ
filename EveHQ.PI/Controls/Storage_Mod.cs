// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2012  EveHQ Development Team
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
    public partial class Storage_Mod : UserControl
    {
        public PIMain PIM;
        private bool RefreshIP = false;
        public string SName;
        private bool RemCMSOpen = false;

        public Storage_Mod()
        {
            InitializeComponent();
        }

        public Storage_Mod(PIMain p, string nm)
        {
            InitializeComponent();
            PIM = p;
            SName = nm;
        }

        private void Storage_Mod_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                RemCMSOpen = true;
            else
                if (!RemCMSOpen)
                {
                    Point clientp = new Point(e.X, e.Y);
                    if (sender.GetType().Name.Equals("PictureBox"))
                    {
                        PictureBox PB = (PictureBox)sender;
                        clientp.X += PB.Location.X;
                        clientp.Y += PB.Location.Y;
                    }
                    else if (sender.GetType().Name.Equals("LabelX"))
                    {
                        DevComponents.DotNetBar.LabelX PBX;
                        PBX = (DevComponents.DotNetBar.LabelX)sender;

                        clientp.X += PBX.Location.X;
                        clientp.Y += PBX.Location.Y;
                    }
                    else if (sender.GetType().Name.Equals("Label"))
                    {
                        Label PBX;
                        PBX = (Label)sender;

                        clientp.X += PBX.Location.X;
                        clientp.Y += PBX.Location.Y;
                    }
                    MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, clientp.X, clientp.Y, e.Delta);
                    base.OnMouseDown(ne);
                }
                else
                    RemCMSOpen = false;
        }

        public void Storage_Mod_Update()
        {
            if (RefreshIP)
                return;

            if (PIM != null)
                RefreshDataDisplay();
        }

        private void Storage_Mod_Load(object sender, EventArgs e)
        {
            RefreshDataDisplay();
        }

        private void RefreshDataDisplay()
        {
            string tt;

            RefreshIP = true;
            pb_ModPic.BackgroundImage = PIM.GetImageForItem(PIM.Working.Storage[SName].typeID);
            tt = "CPU: " + PIM.Working.Storage[SName].CPU;
            tt += "\n PG: " + PIM.Working.Storage[SName].Power;

            toolTip1.SetToolTip(pb_ModPic, tt);

            RefreshIP = false;
        }

        private void tsmi_RemoveModule_Click(object sender, EventArgs e)
        {
            PIM.PIM_RemoveModule(SName, "Storage");
            this.Dispose();
        }
    }
}
