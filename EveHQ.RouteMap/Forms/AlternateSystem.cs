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

namespace EveHQ.RouteMap
{
    public partial class AlternateSystem : UserControl
    {
        public AlternateNode node;
        RouteMapMainForm RMA;
        SelAltNode SAN;

        public AlternateSystem()
        {
            InitializeComponent();
        }


        public AlternateSystem(AlternateNode n, RouteMapMainForm r, SelAltNode s)
        {
            node = n;
            RMA = r;
            SAN = s;
            InitializeComponent();
            DisplayData();
        }

        public void DisplayData()
        {
            string sLev;
            Bitmap bmp;
            Graphics g;

            b_SelectAltSystem.Text = node.curr.Name;
            lb_FromSystem.Text = node.from.Name;
            lb_ToSystem.Text = node.to.Name;
            lb_SystemSize.Text = node.from.GetSystemSize() + " Au";

            lb_RegionAndConst.Text = node.region + ", " + node.constellation;
            lb_SovTick.Text = node.sTic;
            sLev = String.Format("{0:#,0.00}", node.sec);

            if (node.sec <= 0)
            {
                // red
                bmp = new Bitmap((Bitmap)SecLev.Images[5], new Size(42, 20));
                g = Graphics.FromImage(bmp);
                g.DrawString(sLev, new Font("Tahoma", 10), Brushes.LightBlue, new PointF(3, 2));
                pb_SecLev.BackgroundImage = bmp;
            }
            else if ((node.sec > 0) && (node.sec <= 0.25))
            {
                // orange
                bmp = new Bitmap((Bitmap)SecLev.Images[4], new Size(42, 20));
                g = Graphics.FromImage(bmp);
                g.DrawString(sLev, new Font("Tahoma", 10), Brushes.Black, new PointF(7, 2));
                pb_SecLev.BackgroundImage = bmp;
            }
            else if ((node.sec > 0.25) && (node.sec <= 0.49))
            {
                // dk yellow
                bmp = new Bitmap((Bitmap)SecLev.Images[1], new Size(42, 20));
                g = Graphics.FromImage(bmp);
                g.DrawString(sLev, new Font("Tahoma", 10), Brushes.Black, new PointF(7, 2));
                pb_SecLev.BackgroundImage = bmp;
            }
            else if ((node.sec > 0.49) && (node.sec <= 0.55))
            {
                // lt yellow
                bmp = new Bitmap((Bitmap)SecLev.Images[3], new Size(42, 20));
                g = Graphics.FromImage(bmp);
                g.DrawString(sLev, new Font("Tahoma", 10), Brushes.Black, new PointF(7, 2));
                pb_SecLev.BackgroundImage = bmp;
            }
            else if ((node.sec > 0.55) && (node.sec <= 0.8))
            {
                // green
                bmp = new Bitmap((Bitmap)SecLev.Images[2], new Size(42, 20));
                g = Graphics.FromImage(bmp);
                g.DrawString(sLev, new Font("Tahoma", 10), Brushes.Black, new PointF(7, 2));
                pb_SecLev.BackgroundImage = bmp;
            }
            else if (node.sec > 0.8)
            {
                // blue
                bmp = new Bitmap((Bitmap)SecLev.Images[0], new Size(42, 20));
                g = Graphics.FromImage(bmp);
                g.DrawString(sLev, new Font("Tahoma", 10), Brushes.Black, new PointF(7, 2));
                pb_SecLev.BackgroundImage = bmp;
            }

            lb_Distance.Text = String.Format("{0:#,0.#}", node.distFrom) + " Ly";
            lb_DistTo.Text = String.Format("{0:#,0.#}", node.distTo) + " Ly";

            lb_Iso.Text = String.Format("{0:#,0.#}", node.ISOFrom) + " " + node.IsoType;
            lb_IsoTo.Text = String.Format("{0:#,0.#}", node.ISOTo) + " " + node.IsoType;

            lb_Cargo.Text = String.Format("{0:#,0.#}", node.cargoF) + " m3";
            lb_CargoTo.Text = String.Format("{0:#,0.#}", node.cargoT) + " m3";

            lb_Jumps.Text = node.Jumps;
            lb_Kills.Text = node.Kills;

            pb_Station.Tooltip = node.StationTT;
            ToolTip.SetToolTip(lb_SovTick, node.sov);
            pb_JumpType.Tooltip = node.JumpTT;
            pb_JumpTypeTo.Tooltip = node.JumpToTT;
            pb_StationTo.Tooltip = node.StationToTT;

            if (!node.Station)
            {
                pb_Station.Hide();
            }
            else
            {
                pb_Station.Image = JumpStation.Images[0];
                pb_Station.Show();
            }

            if (!node.StationTo)
            {
                pb_Station.Hide();
            }
            else
            {
                pb_Station.Image = JumpStation.Images[0];
                pb_Station.Show();
            }

            switch (node.JumpTyp)
            {
                case PlugInData.JumpType.Beacon:
                    pb_JumpType.Image = JumpStation.Images[2];
                    break;
                case PlugInData.JumpType.Bridge:
                    pb_JumpType.Image = JumpStation.Images[3];
                    break;
                case PlugInData.JumpType.Cyno:
                    pb_JumpType.Image = JumpStation.Images[4];
                    break;
                case PlugInData.JumpType.Gate:
                    pb_JumpType.Image = JumpStation.Images[1];
                    break;
                case PlugInData.JumpType.CynoSafe:
                    pb_JumpType.Image = JumpStation.Images[4];
                    pb_Station.Image = JumpStation.Images[6];
                    pb_Station.Show();
                    break;
                default:
                    pb_JumpType.Image = JumpStation.Images[5];
                    break;
            }

            switch (node.JumpTypTo)
            {
                case PlugInData.JumpType.Beacon:
                    pb_JumpTypeTo.Image = JumpStation.Images[2];
                    break;
                case PlugInData.JumpType.Bridge:
                    pb_JumpTypeTo.Image = JumpStation.Images[3];
                    break;
                case PlugInData.JumpType.Cyno:
                    pb_JumpTypeTo.Image = JumpStation.Images[4];
                    break;
                case PlugInData.JumpType.Gate:
                    pb_JumpTypeTo.Image = JumpStation.Images[1];
                    break;
                case PlugInData.JumpType.CynoSafe:
                    pb_JumpTypeTo.Image = JumpStation.Images[4];
                    pb_StationTo.Image = JumpStation.Images[6];
                    pb_Station.Show();
                    break;
                default:
                    pb_JumpTypeTo.Image = JumpStation.Images[5];
                    break;
            }

            lb_szT.Show();
        }

        private void b_SelectAltSystem_Click(object sender, EventArgs e)
        {
            RMA.AlternateRouteNodeSelected(node);
            SAN.AltNodeSelected();
        }
    }
}
