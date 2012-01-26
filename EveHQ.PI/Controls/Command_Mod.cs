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
    public partial class Command_Mod : UserControl
    {
        public PIMain PIM;
        private bool RefreshIP = false;

        public Command_Mod()
        {
            InitializeComponent();
            PIM = null;
        }

        public Command_Mod(PIMain p)
        {
            InitializeComponent();
            PIM = p;
        }

        public void Command_Mod_Update()
        {
            if (PIM != null)
                RefreshDataDisplay();
        }

        private void Command_Mod_Load(object sender, EventArgs e)
        {
            if (PIM != null)
                RefreshDataDisplay();
        }

        private void RefreshDataDisplay()
        {
            RefreshIP = true;

            if (PIM.Working.Command.CPU_Used > PIM.Working.Command.CPU)
                pb_CPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
            else
                pb_CPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;

            if (PIM.Working.Command.Power_Used > PIM.Working.Command.Power)
                pb_PowerGrid.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
            else
                pb_PowerGrid.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;

            // Setup the Data !
            pb_CPU.Maximum = PIM.Working.Command.CPU;
            pb_PowerGrid.Maximum = PIM.Working.Command.Power;
            pb_CPU.Value = PIM.Working.Command.CPU_Used;
            pb_PowerGrid.Value = PIM.Working.Command.Power_Used;
            pb_CPU.Text = PIM.Working.Command.CPU_Used.ToString() + " / " + PIM.Working.Command.CPU.ToString();
            pb_PowerGrid.Text = PIM.Working.Command.Power_Used.ToString() + " / " + PIM.Working.Command.Power.ToString();
              
            if (PIM.Working.Command.Name.Contains("Limited"))
                rb_Limited.Checked = true;
            else if (PIM.Working.Command.Name.Contains("Standard"))
                rb_Standard.Checked = true;
            else if (PIM.Working.Command.Name.Contains("Improved"))
                rb_Improved.Checked = true;
            else if (PIM.Working.Command.Name.Contains("Advanced"))
                rb_Advanced.Checked = true;
            else if (PIM.Working.Command.Name.Contains("Elite"))
                rb_Elite.Checked = true;
            else
                rb_Basic.Checked = true;

            pb_ModPic.BackgroundImage = PIM.GetImageForItem(PIM.Working.Command.typeID);

            RefreshIP = false;
        }

        private void CommandCenterTypeChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            Planet p = PlugInData.GetPlanetData(PIM.Working.PlanetType);

            foreach (CommandCenter cc in p.CmdCenter.Values)
            {
                if (rb_Basic.Checked)
                {
                    if (!cc.Name.Contains("Limited") && !cc.Name.Contains("Standard") && 
                        !cc.Name.Contains("Improved") && !cc.Name.Contains("Advanced") &&
                        !cc.Name.Contains("Elite"))
                    {
                        PIM.Working.Command = new CommandCenter(cc);
                        break;
                    }
                }
                else if (rb_Limited.Checked)
                {
                    if (cc.Name.Contains("Limited"))
                    {
                        PIM.Working.Command = new CommandCenter(cc);
                        break;
                    }
                }
                else if (rb_Standard.Checked)
                {
                    if (cc.Name.Contains("Standard"))
                    {
                        PIM.Working.Command = new CommandCenter(cc);
                        break;
                    }
                }
                else if (rb_Improved.Checked)
                {
                    if (cc.Name.Contains("Improved"))
                    {
                        PIM.Working.Command = new CommandCenter(cc);
                        break;
                    }
                }
                else if (rb_Advanced.Checked)
                {
                    if (cc.Name.Contains("Advanced"))
                    {
                        PIM.Working.Command = new CommandCenter(cc);
                        break;
                    }
                }
                else if (rb_Elite.Checked)
                {
                    if (cc.Name.Contains("Elite"))
                    {
                        PIM.Working.Command = new CommandCenter(cc);
                        break;
                    }
                }
            }

            PIM.CommandCenterTypeChanged();

            RefreshDataDisplay();
        }

        private void Command_Mod_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                Point clientp = new Point(e.X, e.Y);
                if (sender.GetType().Name.Equals("PictureBox"))
                {
                    clientp.X += pb_ModPic.Location.X;
                    clientp.Y += pb_ModPic.Location.Y;
                }
                else if (sender.GetType().Name.Equals("ProgressBarX"))
                {
                    DevComponents.DotNetBar.Controls.ProgressBarX PBX;
                    PBX = (DevComponents.DotNetBar.Controls.ProgressBarX)sender;

                    clientp.X += PBX.Location.X;
                    clientp.Y += PBX.Location.Y;
                }
                else if (sender.GetType().Name.Equals("LabelX"))
                {
                    DevComponents.DotNetBar.LabelX PBX;
                    PBX = (DevComponents.DotNetBar.LabelX)sender;

                    clientp.X += PBX.Location.X;
                    clientp.Y += PBX.Location.Y;
                }
                MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, clientp.X, clientp.Y, e.Delta);
                base.OnMouseDown(ne);
            }
        }


    }
}
