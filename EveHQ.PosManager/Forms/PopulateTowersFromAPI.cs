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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class PopulateTowersFromAPI : DevComponents.DotNetBar.Office2007Form
    {
        public PoSManMainForm myData;

        public PopulateTowersFromAPI()
        {
            InitializeComponent();
        }

        private void PopulateTowersFromAPI_Load(object sender, EventArgs e)
        {
            string pos_name, strSQL, loc;
            bool present = false;
            DataSet locData;

            cbx_Monitored.Checked = true;

            clb_TowerListing.Items.Clear();
            foreach (APITowerData apid in PlugInData.API_D.apiTower.Values)
            {
                present = false;
                foreach (POS p in PlugInData.PDL.Designs.Values)
                {
                    if (p.itemID == apid.itemID)
                    {
                        present = true;
                        break;
                    }
                }

                if (!present)
                {
                    // Get Table With Tower or Tower Item Information

                    strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + apid.moonID + ";";
                    locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                    loc = locData.Tables[0].Rows[0].ItemArray[0].ToString();

                    if (apid.locName == "Unknown")
                    {
                        apid.locName = loc;
                        PlugInData.API_D.SaveAPIListing();
                    }

                    pos_name = apid.corpName + " --> " + loc + ", " + apid.towerName + " [" + apid.itemID + "]";
                    clb_TowerListing.Items.Add(pos_name);
                }
            }
        }


        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void b_Import_Click(object sender, EventArgs e)
        {
            bool chkd;
            bool mon;
            string tName;
            POS APITower;

            mon = cbx_Monitored.Checked;

            // Store the selected - to monitor - data
            for (int indx = 0; indx < clb_TowerListing.Items.Count; indx++)
            {
                chkd = clb_TowerListing.GetItemChecked(indx);

                if (chkd)
                {
                    tName = clb_TowerListing.Items[indx].ToString();
                    // Need to add this POS Tower to my listings
                    foreach (APITowerData apid in PlugInData.API_D.apiTower.Values)
                    {
                        if (tName.Contains(apid.itemID.ToString()))
                        {
                            // Found the correct one, now add it
                            tName = apid.corpName + "-->" + apid.towerName;
                            APITower = new POS(tName);

                            // I have the name. Need to add the tower object to the POS
                            if (PlugInData.TL.Towers.ContainsKey(apid.towerID))
                            {
                                APITower.PosTower = new Tower(PlugInData.TL.Towers[apid.towerID]);
                            }

                            APITower.CorpName = apid.corpName;
                            APITower.corpID = apid.corpID;
                            APITower.System = apid.locName;

                            // Need to set tower as Linked to the API by default
                            APITower.itemID = apid.itemID;
                            APITower.locID = apid.locID;

                            switch (apid.stateV)
                            {
                                case 0:
                                    APITower.PosTower.State = "Unanchored";
                                    break;
                                case 1:
                                    APITower.PosTower.State = "Offline";
                                    break;
                                case 2:
                                    APITower.PosTower.State = "Onlining";
                                    break;
                                case 3:
                                    APITower.PosTower.State = "Reinforced";
                                    break;
                                case 4:
                                    APITower.PosTower.State = "Online";
                                    break;
                            }

                            // Also need to set Fuel Data Amounts
                            APITower.PosTower.Fuel.EnrUran.Qty = apid.EnrUr;
                            APITower.PosTower.Fuel.Oxygen.Qty = apid.Oxygn;
                            APITower.PosTower.Fuel.MechPart.Qty = apid.MechP;
                            APITower.PosTower.Fuel.Robotics.Qty = apid.Robot;
                            APITower.PosTower.Fuel.HvyWater.Qty = apid.HvyWt;
                            APITower.PosTower.Fuel.LiqOzone.Qty = apid.LiqOz;
                            APITower.PosTower.Fuel.Charters.Qty = apid.Charters;
                            APITower.PosTower.Fuel.Coolant.Qty = apid.Coolt;
                            APITower.PosTower.Fuel.H2Iso.Qty = apid.H2Iso;
                            APITower.PosTower.Fuel.HeIso.Qty = apid.HeIso;
                            APITower.PosTower.Fuel.O2Iso.Qty = apid.O2Iso;
                            APITower.PosTower.Fuel.N2Iso.Qty = apid.N2Iso;
                            APITower.PosTower.Fuel.Strontium.Qty = apid.Stront;
                            APITower.Monitored = mon;

                            PlugInData.PDL.AddDesignToList(APITower);
                        }
                    }
                }
            }
            this.Dispose();
        }

    }
}
