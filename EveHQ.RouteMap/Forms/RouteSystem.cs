// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 4 September 2011, and as distributed, it includes or is 
// derivative of works licensed under the GNU General Public License.
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
    public partial class RouteSystem : UserControl
    {
        public RouteNode node;
        RouteMapMainForm RMA;

        public RouteSystem(RouteNode n, RouteMapMainForm r)
        {
            node = n;
            RMA = r;
            InitializeComponent();
            DisplayData();
        }

        public void DisplayData()
        {
            string sLev;
            Bitmap bmp;
            Graphics g;

            if (node.JumpNum == 0)
            {
                b_JumpNum.Text = "S";
                b_JumpNum.Tooltip = "Starting System";
            }
            else if (node.JumpNum < 0)
            {
                b_JumpNum.Text = "T";
                b_JumpNum.Tooltip = "Route Totals";
            }
            else if (node.Dest)
            {
                b_JumpNum.Text = "D";
                b_JumpNum.Tooltip = "Destination System";
            }
            else if (node.AltNode)
            {
                b_JumpNum.Text = node.JumpNum.ToString();
                b_JumpNum.Tooltip = "Click Here to Select Alternat Route Node";
                this.b_JumpNum.Click += new System.EventHandler(this.b_JumpNum_Click);
                b_JumpNum.Enabled = true;
            }
            else
            {
                b_JumpNum.Text = node.JumpNum.ToString();
                b_JumpNum.Tooltip = "";
                b_JumpNum.Enabled = false;
            }

            if (node.JumpNum < 0)
                lb_SystemName.Text = "Totals for " + node.Jumps + " Jumps";
            else
                lb_SystemName.Text = node.from.Name;

            lb_SystemSize.Text = node.from.GetSystemSize() + " Au";

            if (node.JumpNum >= 0)
            {
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
                //lb_SecLev.ForeColor = Color.FromArgb(0xFF, node.from.GetSystemColor());
            }
            else
            {
                lb_RegionAndConst.Hide();
                lb_SovTick.Hide();
                pb_SecLev.Hide();
            }

            if (node.distance >= 0)
            {
                lb_Distance.Show();
                lb_Distance.Text = String.Format("{0:#,0.#}", node.distance) + " Ly";
            }
            else
            {
                lb_Distance.Hide();
            }

            if (node.ISO >= 0)
            {
                lb_Iso.Show();
                lb_Iso.Text = String.Format("{0:#,0.#}", node.ISO) + " " + node.IsoType;
            }
            else
            {
                lb_Iso.Hide();
            }

            lb_LO.Text = String.Format("{0:#,0.#}", node.LO) + " LO";
            lb_Cargo.Text = String.Format("{0:#,0.#}", node.cargo) + " m3";

            if (node.JumpNum >= 0)
                lb_Jumps.Text = node.Jumps;
            else
            {
                lb_Jumps.Hide();
                lb_SysJmp1h.Hide();
            }

            if (node.JumpNum >= 0)
                lb_Kills.Text = node.Kills;
            else
            {
                lb_Kills.Hide();
                lb_killT.Hide();
            }

            pb_Station.Tooltip = node.StationTT;
            ToolTip.SetToolTip(lb_SovTick, node.sov);
            pb_JumpType.Tooltip = node.JumpTT;

            if (!node.Station)
            {
                pb_Station.Hide();
            }
            else
            {
                pb_Station.Image = JumpStation.Images[0];
                pb_Station.Show();
            }

            if (node.JumpNum > 0)
            {
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
                        lb_SystemName.Text = node.CynoSafeMoon;
                        break;
                    default:
                        pb_JumpType.Image = JumpStation.Images[5];
                        break;
                }
            }
            else
                pb_JumpType.Hide();

            if (node.JumpNum >= 0)
            {
                lb_SystemSize.Show();
                lb_szT.Show();
            }
            else
            {
                lb_SystemSize.Hide();
                lb_szT.Hide();
            }
        }

        private void tsmi_Avoid_Click(object sender, EventArgs e)
        {
            RMA.MapViewSelectAvoid(node.from);
        }

        private void tsmi_Waypoint_Click(object sender, EventArgs e)
        {
            RMA.MapViewSelectWaypoint(node.from);
        }

        private void tsmi_ZoomCenter_Click(object sender, EventArgs e)
        {
            RMA.RouteMapCenterSystem(node.from);
        }

        private void b_JumpNum_Click(object sender, EventArgs e)
        {
            // Ability to select an alternate system for this particular jump Node.
            RMA.FindAlternateTravelNode(node.from);
        }
      
    }
}
