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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PI
{
    public partial class PI_Module : UserControl
    {
        public string ModName;
        public ArrayList Selections;
        public string contName;
        public PIMain PIM;
        private bool setup = false;

        public PI_Module()
        {
            InitializeComponent();
            ModName = "";
            Selections = new ArrayList();
        }

        public void SetupComponentDisplay(string mn, ArrayList cbS, string sN, Bitmap pc, PIMain pm, string cN)
        {
            setup = true;

            ModName = mn;
            PIM = pm;
            contName = cN;

            lb_ModName.Text = ModName;

            if (cbS != null)
            {
                Selections = new ArrayList(cbS);
                cb_ExtractProcess.Show();
                cb_ExtractProcess.Items.Clear();
                foreach (string s in Selections)
                    cb_ExtractProcess.Items.Add(s);

                if (sN != "")
                    cb_ExtractProcess.SelectedText = sN;
                else
                    cb_ExtractProcess.Text = "Select Recipe";

                GetAndSetInputsForRecipe(sN);

                this.ContextMenuStrip = cms_Remove;
            }
            else
            {
                cb_ExtractProcess.Hide();
                this.ContextMenuStrip = null;
            }

            pb_ModPic.BackgroundImage = pc;

            setup = false;
        }

        private void GetAndSetInputsForRecipe(string rcp)
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

            if (PlugInData.Reactions.ContainsKey(rcp))
            {
                cycle = PlugInData.Reactions[rcp].cycleTime / 60;
                mult = 60 / cycle;

                prdHour = PlugInData.Reactions[rcp].outputs.Values[0].Qty * mult;
                lb_Produces.Text = "Makes " + String.Format("{0:#,0.#}", prdHour) + " per Hour";

                foreach (Component c in PlugInData.Reactions[rcp].inputs.Values)
                {
                    lvi = lv_MatsNeeded.Items.Add(c.Name);
                    qty = String.Format("{0:#,0.#}", (c.Qty * mult));
                    lvi.SubItems.Add(qty);
                }
            }
        }


        private void cb_ExtractProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setup)
            {
                PIM.ComponentRecipeChanged(cb_ExtractProcess.Text, this);
                GetAndSetInputsForRecipe(cb_ExtractProcess.Text);
            }
        }

        private void tmsi_Remove_Click(object sender, EventArgs e)
        {
            PIM.PIM_RemoveModule(contName);
        }

    }
}
