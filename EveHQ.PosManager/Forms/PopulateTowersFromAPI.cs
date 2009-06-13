﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class PopulateTowersFromAPI : Form
    {
        public PoSManMainForm myData;

        public PopulateTowersFromAPI()
        {
            InitializeComponent();
        }

        private void PopulateTowersFromAPI_Load(object sender, EventArgs e)
        {
            string pos_name;
            bool present = false;

            rb_Offline.Checked = true;
            cbx_Monitored.Checked = true;

            clb_TowerListing.Items.Clear();
            foreach (API_Data apid in myData.API_D.apid)
            {
                present = false;
                foreach (POS p in myData.POSList.Designs)
                {
                    if (p.itemID == apid.itemID)
                    {
                        present = true;
                        break;
                    }
                }

                if (!present)
                {
                    pos_name = apid.corpName + " --> " + apid.locName + ", " + apid.towerName + " [" + apid.itemID + "]";
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
            string st, tName;
            POS APITower;

            if (rb_Reinforced.Checked)
                st = "Reinforced";
            else if (rb_Online.Checked)
                st = "Online";
            else
                st = "Offline";

            mon = cbx_Monitored.Checked;

            // Store the selected - to monitor - data
            for (int indx = 0; indx < clb_TowerListing.Items.Count; indx++)
            {
                chkd = clb_TowerListing.GetItemChecked(indx);

                if (chkd)
                {
                    tName = clb_TowerListing.Items[indx].ToString();
                    // Need to add this POS Tower to my listings
                    foreach (API_Data apid in myData.API_D.apid)
                    {
                        if(tName.Contains(apid.itemID.ToString()))
                        {
                            // Found the correct one, now add it
                            tName = apid.corpName + "-->" + apid.towerName;
                            APITower = new POS(tName);

                            // I have the name. Need to add the tower object to the POS
                            foreach (Tower t in myData.TL.Towers)
                            {
                                if (t.typeID == apid.towerID)
                                {
                                    APITower.PosTower = new Tower(t);
                                    break;
                                }
                            }

                            APITower.CorpName = apid.corpName;
                            APITower.System = apid.locName;

                            // Need to set tower as Linked to the API by default
                            APITower.itemID = apid.itemID;
                            APITower.locID = apid.towerLocation;
                            APITower.PosTower.State = st;
                            APITower.PosTower.CPU_Used = APITower.PosTower.CPU;
                            APITower.PosTower.Power_Used = APITower.PosTower.Power;

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

                            myData.POSList.AddDesignToList(APITower);
                        }
                    }
                }
            }
            this.Dispose();
        }
    }
}
