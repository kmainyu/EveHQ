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
    public partial class Processor_Mod : UserControl
    {
        public PIMain PIM;
        public string ProcName;
        private bool RefreshIP = false;
        private bool RemCMSOpen = false;

        public Processor_Mod()
        {
            InitializeComponent();
            PIM = null;
            ProcName = "";
        }

        public Processor_Mod(PIMain p, string nm)
        {
            InitializeComponent();
            PIM = p;
            ProcName = nm;
        }

        private void cb_ExtractProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;
            
            if (PlugInData.Reactions.ContainsKey(cb_ExtractProcess.Text))
            {
                PIM.Working.Processors[ProcName].Reacting = new Reaction(PlugInData.Reactions[cb_ExtractProcess.Text]);
                GetAndSetInputsForRecipe(PIM.Working.Processors[ProcName]);

                PIM.ProcessorModuleChanged();
            }
        }

        private void tabControlPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                RemCMSOpen = true;
            else
                if (!RemCMSOpen)
                {
                    Point clientp = new Point(e.X, e.Y);
                    if (sender.GetType().Name.Equals("PictureBox"))
                    {
                        clientp.X += pb_ModPic.Location.X;
                        clientp.Y += pb_ModPic.Location.Y;
                    }
                    else if (sender.GetType().Name.Equals("Label"))
                    {
                        clientp.X += lb_Produces.Location.X;
                        clientp.Y += lb_Produces.Location.Y;
                    }
                    MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, clientp.X, clientp.Y, e.Delta);
                    base.OnMouseDown(ne);
                }
                else
                    RemCMSOpen = false;
        }

        public void Processor_Mod_Update()
        {
            if (PIM != null)
                RefreshDataDisplay();
        }


        private void Processor_Mod_Level_Changed(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            if (rbx_P1.Checked)
            {
                PIM.Working.Processors[ProcName].ProcLevel = 1;
                PIM.Working.Processors[ProcName].AvailReactions = new List<string>(SetAvailListByLevel(1));
            }
            else if (rbx_P2.Checked)
            {
                PIM.Working.Processors[ProcName].ProcLevel = 2;
                PIM.Working.Processors[ProcName].AvailReactions = new List<string>(SetAvailListByLevel(2));
            }
            else if (rbx_P3.Checked)
            {
                PIM.Working.Processors[ProcName].ProcLevel = 3;
                PIM.Working.Processors[ProcName].AvailReactions = new List<string>(SetAvailListByLevel(3));
            }
            else
            {
                PIM.Working.Processors[ProcName].ProcLevel = 4;
                PIM.Working.Processors[ProcName].AvailReactions = new List<string>(SetAvailListByLevel(4));
            }
            PIM.Working.Processors[ProcName].Reacting = new Reaction();
            RefreshDataDisplay();
            PIM.ProcessorModuleChanged();
        }

        private void RefreshDataDisplay()
        {
            string tt = "";
            string ActName = "Select Reaction";
            RefreshIP = true;

            pb_ModPic.BackgroundImage = PIM.GetImageForItem(PIM.Working.Processors[ProcName].typeID);
            cb_ExtractProcess.Items.Clear();
            if (PIM.Working.Processors[ProcName].Reacting.reactName != "")
            {
                ActName = PIM.Working.Processors[ProcName].Reacting.reactName;
                if (PIM.Working.Processors[ProcName].Reacting.level == 1)
                    rbx_P1.Checked = true;
                else if (PIM.Working.Processors[ProcName].Reacting.level == 2)
                    rbx_P2.Checked = true;
                else if (PIM.Working.Processors[ProcName].Reacting.level == 3)
                    rbx_P3.Checked = true;
                else if (PIM.Working.Processors[ProcName].Reacting.level == 4)
                    rbx_P4.Checked = true;

                GetAndSetInputsForRecipe(PIM.Working.Processors[ProcName]);
            }
            else
            {
                lv_MatsNeeded.Items.Clear();
                lb_Produces.Text = "";
                if (PIM.Working.Processors[ProcName].ProcLevel < 1)
                {
                    PIM.Working.Processors[ProcName].AvailReactions = new List<string>(SetAvailListByLevel(1));
                    rbx_P1.Checked = true;
                }
                else
                {
                    PIM.Working.Processors[ProcName].AvailReactions = new List<string>(SetAvailListByLevel(PIM.Working.Processors[ProcName].ProcLevel));
                    switch (PIM.Working.Processors[ProcName].ProcLevel)
                    {
                        case 1:
                            rbx_P1.Checked = true;
                            break;
                        case 2:
                            rbx_P2.Checked = true;
                            break;
                        case 3:
                            rbx_P3.Checked = true;
                            break;
                        default:
                            rbx_P4.Checked = true;
                            break;
                    }
                }
            }

            cb_ExtractProcess.Items.AddRange(PIM.Working.Processors[ProcName].AvailReactions.ToArray());
            cb_ExtractProcess.Text = ActName;
            ii_NumProc.Value = PIM.Working.Processors[ProcName].Qty;

            if (PIM.Working.Processors[ProcName].ProcLevel == 4)
            {
                foreach (Processor c in PlugInData.Planets[PIM.Working.PlanetType].Processors.Values)
                {
                    if (c.Name.Contains("High"))
                    {
                        tt = "CPU: " + (c.CPU * PIM.Working.Processors[ProcName].Qty);
                        tt += "\n PG: " + (c.Power * PIM.Working.Processors[ProcName].Qty);
                        break;
                    }
                }
            }
            if (PIM.Working.Processors[ProcName].ProcLevel == 1)
            {
                foreach (Processor c in PlugInData.Planets[PIM.Working.PlanetType].Processors.Values)
                {
                    if (c.Name.Contains("Basic"))
                    {
                        tt = "CPU: " + (c.CPU * PIM.Working.Processors[ProcName].Qty);
                        tt += "\n PG: " + (c.Power * PIM.Working.Processors[ProcName].Qty);
                        break;
                    }
                }
            }
            else if (PIM.Working.Processors[ProcName].ProcLevel > 1)
            {
                foreach (Processor c in PlugInData.Planets[PIM.Working.PlanetType].Processors.Values)
                {
                    if (c.Name.Contains("Advanced"))
                    {
                        tt = "CPU: " + (c.CPU * PIM.Working.Processors[ProcName].Qty);
                        tt += "\n PG: " + (c.Power * PIM.Working.Processors[ProcName].Qty);
                        break;
                    }
                }
            }

            toolTip1.SetToolTip(pb_ModPic, tt);

            RefreshIP = false;
        }

        private void Processor_Mod_Load(object sender, EventArgs e)
        {
            if ((!PIM.Working.PlanetType.Equals("Barren")) && (!PIM.Working.PlanetType.Equals("Temperate")))
                rbx_P4.Visible = false;

            RefreshDataDisplay();
        }

        public List<string> SetAvailListByLevel(int Level)
        {
            List<string> retVal = new List<string>();

            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                if (r.level == Level)
                    retVal.Add(r.reactName);
            }

            return retVal;
        }

        private void GetAndSetInputsForRecipe(Processor p)
        {
            int cycle;
            int prdHour;
            int mult = 1;
            ListViewItem lvi;
            string qty;

            lv_MatsNeeded.Items.Clear();
            lv_MatsNeeded.View = View.Details;
            lv_MatsNeeded.HeaderStyle = ColumnHeaderStyle.None;
            lb_Produces.Text = "";

            if (PlugInData.Reactions.ContainsKey(PIM.Working.Processors[ProcName].Reacting.reactName))
            {
                cycle = PlugInData.Reactions[PIM.Working.Processors[ProcName].Reacting.reactName].cycleTime / 60;
                mult = 60 / cycle;

                prdHour = PlugInData.Reactions[PIM.Working.Processors[ProcName].Reacting.reactName].outputs.Values[0].Qty * mult;
                prdHour *= PIM.Working.Processors[ProcName].Qty;
                lb_Produces.Text = "Makes " + String.Format("{0:#,0.#}", prdHour) + " per Hour";

                foreach (Component c in PlugInData.Reactions[PIM.Working.Processors[ProcName].Reacting.reactName].inputs.Values)
                {
                    lvi = lv_MatsNeeded.Items.Add(c.Name);
                    qty = String.Format("{0:#,0.#}", (c.Qty * mult * PIM.Working.Processors[ProcName].Qty));
                    lvi.SubItems.Add(qty);
                }
            }
        }

        private void tsmi_RemoveProcessor_Click(object sender, EventArgs e)
        {
            PIM.PIM_RemoveModule(ProcName, "Processor");
            this.Dispose();
        }

        private void ii_NumProc_ValueChanged(object sender, EventArgs e)
        {
            PIM.Working.Processors[ProcName].Qty = ii_NumProc.Value;
            RefreshDataDisplay();
            PIM.ProcessorModuleChanged();
        }


    }
}
