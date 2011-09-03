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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PI
{
    public partial class NewPIFacility : Form
    {
        public NewPIFacility()
        {
            InitializeComponent();
            cb_SystemPlanet.Items.Clear();
            foreach (var p in PlugInData.Planets)
            {
                if (!p.Key.Contains("Shattered"))
                    cb_SystemPlanet.Items.Add(p.Key);
            }
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            if (cb_SystemPlanet.Text == "")
            {
                MessageBox.Show("You forgote to select a Planet Type !.", "User Error", MessageBoxButtons.OK);
                return;
            }

            if (!PlugInData.Facilities.ContainsKey(tb_FacilityName.Text))
            {
                PlugInData.NewFacName = tb_FacilityName.Text;
                PlugInData.NewFacPlanet = cb_SystemPlanet.Items[cb_SystemPlanet.SelectedIndex].ToString();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("A facility with this name already exists, Please pick a new Name.", "Name Error", MessageBoxButtons.OK);
            }
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            PlugInData.NewFacName = "";
            PlugInData.NewFacPlanet = "";
            this.Dispose();
        }

        private void cb_SystemPlanet_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = cb_SystemPlanet.SelectedIndex;
            string nm = cb_SystemPlanet.Items[ind].ToString();

            pb_PlanetType.BackgroundImage = il_PlanetImages.Images[ind];

            foreach (var p in PlugInData.Planets)
            {
                if (p.Key.Equals(nm))
                    lb_Description.Text = p.Value.Description;
            }

        }
    }
}
