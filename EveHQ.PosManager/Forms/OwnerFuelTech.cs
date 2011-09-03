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
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class OwnerFuelTech : DevComponents.DotNetBar.Office2007Form
    {
        public PoSManMainForm myData;

        public OwnerFuelTech(PoSManMainForm inD)
        {
            myData = inD;
            InitializeComponent();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void b_OK_Click(object sender, EventArgs e)
        {
            string twrName;
            POS pl;

            twrName = myData.selName;

           if (PlugInData.PDL.Designs.ContainsKey(twrName))
           {
               pl = PlugInData.PDL.Designs[twrName];
                pl.Owner = cb_OwnerName.Text;
                pl.FuelTech = cb_FuelTechName.Text;

                if (rb_Corp.Checked)
                {
                    foreach (APITowerData atd in PlugInData.API_D.apiTower.Values)
                    {
                        if (atd.corpName == pl.Owner)
                        {
                            pl.ownerID = atd.corpID;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
                    {
                        if (selPilot.Name == pl.Owner)
                        {
                            pl.ownerID = Convert.ToDecimal(selPilot.ID);
                            break;
                        }
                    }
                }

                foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
                {
                    if (selPilot.Name == pl.FuelTech)
                    {
                        pl.fuelTechID = Convert.ToDecimal(selPilot.ID);
                        break;
                    }
                }
            }
            this.Dispose();
        }

        private void OwnerFuelTech_Load(object sender, EventArgs e)
        {
            string twrName;

            twrName = myData.selName;
            if (PlugInData.PDL.Designs.ContainsKey(twrName))
            {
                if (PlugInData.PDL.Designs[twrName].Owner == PlugInData.PDL.Designs[twrName].CorpName)
                    rb_Corp.Checked = true;
                else
                    rb_Personal.Checked = true;
            }
            PopulateOwnerNameList();
            PopulateFuelTechNameList();
        }

        private void PopulateOwnerNameList()
        {
            string ownName = "";
            string twrName;

            twrName = myData.selName;

            if (PlugInData.PDL.Designs.ContainsKey(twrName))
            {
                if (rb_Corp.Checked)
                    ownName = PlugInData.PDL.Designs[twrName].CorpName;
                else
                    ownName = PlugInData.PDL.Designs[twrName].Owner;
            }

            if (rb_Corp.Checked)
            {
                // Corp set, so can ONLY be the corp set for POS already
                cb_OwnerName.Items.Clear();
                cb_OwnerName.Items.Add(ownName);
                cb_OwnerName.Text = ownName;
            }
            else
            {
                cb_OwnerName.Items.Clear();
                foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
                {
                    cb_OwnerName.Items.Add(selPilot.Name);
                }
                cb_OwnerName.Text = ownName;
            }
        }

        private void PopulateFuelTechNameList()
        {
            string twrName;

            twrName = myData.selName;
            cb_FuelTechName.Items.Clear();
            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                cb_FuelTechName.Items.Add(selPilot.Name);
            }

            if (PlugInData.PDL.Designs.ContainsKey(twrName))
            {
                cb_FuelTechName.Text = PlugInData.PDL.Designs[twrName].FuelTech;
            }
        }

        private void rb_Personal_CheckedChanged(object sender, EventArgs e)
        {
            PopulateOwnerNameList();
        }

        private void rb_Corp_CheckedChanged(object sender, EventArgs e)
        {
            PopulateOwnerNameList();
        }

     }
}
