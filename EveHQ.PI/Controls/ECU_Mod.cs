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
    public partial class ECU_Mod : UserControl
    {
        public PIMain PIM;
        private bool RefreshIP = false;
        public string ExtName;
        private bool RemCMSOpen = false;

        public ECU_Mod()
        {
            InitializeComponent();
        }

        public ECU_Mod(PIMain p, string nm)
        {
            InitializeComponent();
            PIM = p;
            ExtName = nm;
        }

        private void ECU_Mod_MouseDown(object sender, MouseEventArgs e)
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
                        DevComponents.DotNetBar.LabelX LBX;
                        LBX = (DevComponents.DotNetBar.LabelX)sender;

                        clientp.X += LBX.Location.X;
                        clientp.Y += LBX.Location.Y;
                    }
                    else if (sender.GetType().Name.Equals("Label"))
                    {
                        Label LBL;
                        LBL = (Label)sender;

                        clientp.X += LBL.Location.X;
                        clientp.Y += LBL.Location.Y;
                    }
                    MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, clientp.X, clientp.Y, e.Delta);
                    base.OnMouseDown(ne);
                }
                else
                    RemCMSOpen = false;
        }

        public void ECU_Mod_Update()
        {
            if (PIM != null)
                RefreshDataDisplay();
        }

        private void RefreshDataDisplay()
        {
            string nm, rcpUse, tt;

            RefreshIP = true;

            nm = PIM.Working.ECUs[ExtName].Extracting;
            rcpUse = PIM.GetRecipesUsingItem(nm);

            if (nm != "")
                lb_UsedFor.Text = nm + " --> " + rcpUse;
            else
                lb_UsedFor.Text = rcpUse;

            ii_QtyPerHour.Value = PIM.Working.ECUs[ExtName].ExtRate;
            ii_RunTime.Value = PIM.Working.ECUs[ExtName].RunTime;
            ii_MiningHeads.Value = PIM.Working.ECUs[ExtName].Heads;
            tt =    "Extractor Control Unit";
            tt += "\nCPU: " + (PIM.Working.ECUs[ExtName].CPU + (PIM.Working.ECUs[ExtName].Heads * PIM.Working.ECUs[ExtName].Head_CPU));
            tt += "\n PG: " + (PIM.Working.ECUs[ExtName].Power + (PIM.Working.ECUs[ExtName].Heads * PIM.Working.ECUs[ExtName].Head_Power));
            tt_ECU.SetToolTip(pb_Module, tt);

            RefreshIP = false;
        }

        private void ECU_Mod_Load(object sender, EventArgs e)
        {
            RefreshIP = true;
            pb_Module.BackgroundImage = PIM.GetImageForItem(PIM.Working.ECUs[ExtName].typeID); // PIM.GetImageForItem

            SetAvailableExtractions();
            RefreshDataDisplay();
        }

        private void SetAvailableExtractions()
        {
            List<string> rL;
            int cnt = 1;

            // Planet data needs to change. I need a way to get mineral types without
            // using the Extractor Name.
            rL = PlugInData.GetResourcesForPlanet(PIM.Working.PlanetType);
            tt_ECU.SetToolTip(pb_Min1, "Unknown");
            tt_ECU.SetToolTip(pb_Min2, "Unknown");
            tt_ECU.SetToolTip(pb_Min3, "Unknown");
            tt_ECU.SetToolTip(pb_Min4, "Unknown");
            tt_ECU.SetToolTip(pb_Min5, "Unknown");

            foreach (string s in rL)
            {
                switch (cnt)
                {
                    case 1:
                        tt_ECU.SetToolTip(pb_Min1, s);
                        SetAndSizeImage(pb_Min1, s);
                        if (s.Equals(PIM.Working.ECUs[ExtName].Extracting))
                            rb_Min1.Checked = true;
                        break;
                    case 2:
                        tt_ECU.SetToolTip(pb_Min2, s);
                        SetAndSizeImage(pb_Min2, s);
                        if (s.Equals(PIM.Working.ECUs[ExtName].Extracting))
                            rb_Min2.Checked = true;
                        break;
                    case 3:
                        tt_ECU.SetToolTip(pb_Min3, s);
                        SetAndSizeImage(pb_Min3, s);
                        if (s.Equals(PIM.Working.ECUs[ExtName].Extracting))
                            rb_Min3.Checked = true;
                        break;
                    case 4:
                        tt_ECU.SetToolTip(pb_Min4, s);
                        SetAndSizeImage(pb_Min4, s);
                        if (s.Equals(PIM.Working.ECUs[ExtName].Extracting))
                            rb_Min4.Checked = true;
                        break;
                    case 5:
                        tt_ECU.SetToolTip(pb_Min5, s);
                        SetAndSizeImage(pb_Min5, s);
                        if (s.Equals(PIM.Working.ECUs[ExtName].Extracting))
                            rb_Min5.Checked = true;
                        break;
                    default:
                        break;
                }
                cnt++;
            }
        }

        public void SetAndSizeImage(PictureBox PB, string si)
        {
            string abrev;
            Bitmap bmp;
            Graphics g;
            Rectangle dr, sr;
            string[] split;

            bmp = PIM.GetIconForItem(PlugInData.Resources[si].typeID.ToString());

            g = Graphics.FromImage(bmp);

            sr = new Rectangle(0, 0, bmp.Width, bmp.Height);
            dr = new Rectangle(0, 0, PB.Width, PB.Height);

            split = si.Split();
            abrev = "";
            foreach (string s in split)
            {
                if (s.Length > 0)
                    abrev += s[0] + ".";
            }

            SolidBrush QtyBrush = new SolidBrush(Color.Black);
            g.FillRectangle(new SolidBrush(Color.White), bmp.Width - 40, bmp.Height - 18, 40, 19);
            g.DrawString(abrev, new Font("Tahoma", 14, FontStyle.Regular), QtyBrush, new PointF(bmp.Width - 40, bmp.Height - 20));

            g.Dispose();
            PB.BackgroundImage = bmp;
        }

        private void ii_QtyPerHour_ValueChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            PIM.Working.ECUs[ExtName].ExtRate = ii_QtyPerHour.Value;
            PIM.ECUModuleChanged();
        }

        private void ii_RunTime_ValueChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            PIM.Working.ECUs[ExtName].RunTime = ii_RunTime.Value;
            PIM.ECUModuleChanged();
        }

        private void ii_MiningHeads_ValueChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            PIM.Working.ECUs[ExtName].Heads = ii_MiningHeads.Value;
            PIM.ECUModuleChanged();
            RefreshDataDisplay();
        }

        private void tsmi_RemoveModule_Click(object sender, EventArgs e)
        {
            PIM.PIM_RemoveModule(ExtName, "ECU");
            this.Dispose();
        }

        private void rb_Extracting_CheckedChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            string extr = "";

            if (rb_Min1.Checked)
                extr = tt_ECU.GetToolTip(pb_Min1);
            else if (rb_Min2.Checked)
                extr = tt_ECU.GetToolTip(pb_Min2);
            else if (rb_Min3.Checked)
                extr = tt_ECU.GetToolTip(pb_Min3);
            else if (rb_Min4.Checked)
                extr = tt_ECU.GetToolTip(pb_Min4);
            else if (rb_Min5.Checked)
                extr = tt_ECU.GetToolTip(pb_Min5);

            PIM.Working.ECUs[ExtName].Extracting = extr;
            RefreshDataDisplay();
            PIM.ECUModuleChanged();
        }
   }
}
