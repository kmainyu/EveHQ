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

namespace EveHQ.PosManager
{
    public partial class Tower_API_Linker : DevComponents.DotNetBar.Office2007Form
    {
        public PoSManMainForm myData;

        public Tower_API_Linker()
        {
            InitializeComponent();
        }

        private void b_Done_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void b_UnLink_Click(object sender, EventArgs e)
        {
            TreeNode pos;
            POS pli;

            pos = tv_PoSList.SelectedNode;
            if (pos == null)
            {
                MessageBox.Show("Sorry - Cannot locate that POS, are you sure you selected one?", "Data Error", MessageBoxButtons.OK);
                return;
            }

            if (pos.Parent != null)
                pos = pos.Parent;

            // A valid link as been selected. Need to perform the actual Data Link for use
            pli = myData.GetPoSListingForPoS(pos.Text);

            if (pli != null)
            {
                pli.itemID = 0;
                pli.locID = 0;
                PopulatePOSListView();
            }
            else
            {
                MessageBox.Show("Sorry - Cannot locate That POS Listing.", "Data Error", MessageBoxButtons.OK);
                return;
            }
        }

        private void b_LinkEm_Click(object sender, EventArgs e)
        {
            TreeNode pos, api;
            POS pli;
            long itemID;
            APITowerData apid;

            pos = tv_PoSList.SelectedNode;
            api = tv_APIList.SelectedNode;

            if ((pos == null) || (api == null))
            {
                // Pop Up dialog stating they must select a Tower, not a Location / system
                MessageBox.Show("You Must Select BOTH a Tower, AND a System Location.", "Select Error", MessageBoxButtons.OK);
                return;
            }

            if ((api.Parent != null) && (api.Parent.Tag.ToString() != "LOC") && (api.Parent.Tag.ToString() != "CORP"))
                api = api.Parent;

            if (pos.Parent != null)
                pos = pos.Parent;

            if (api.Tag.ToString() == "LOC")
            {
                // Pop Up dialog stating they must select a Tower, not a Location / system
                MessageBox.Show("You Must Select a Tower, Not a System Location.", "Select Error", MessageBoxButtons.OK);
                return;
            }
            if (api.Tag.ToString() == "CORP")
            {
                // Pop Up dialog stating they must select a Tower, not a Location / system
                MessageBox.Show("You Must Select a Tower, Not a Corporation.", "Select Error", MessageBoxButtons.OK);
                return;
            }

            // A valid link as been selected. Need to perform the actual Data Link for use
            pli = myData.GetPoSListingForPoS(pos.Text);

            if (pli != null)
            {
                itemID = Convert.ToInt64(api.Tag.ToString());
                apid = PlugInData.API_D.GetAPIDataMemberForTowerID(itemID);
                pli.itemID = itemID;
                pli.locID = apid.locID;
                pli.CorpName = apid.corpName;
                pli.System = apid.locName;
                PopulatePOSListView();
            }
            else
            {
                MessageBox.Show("Sorry - Cannot locate That POS Listing.", "Data Error", MessageBoxButtons.OK);
                return;
            }
        }

        private void Tower_API_Linker_Load(object sender, EventArgs e)
        {
            // Form loading, populate the Views
            TreeNode mn = new TreeNode();
            mn.Name = "mainNode";
            mn.Text = "Towers in API Data";
            tv_APIList.Nodes.Add(mn);
        }

        private void PopulatePOSListView()
        {
            TreeNode mtn, tn;
            APITowerData apid;
            string posName;
            long itemID;
            long towerNum = 1;

            tv_PoSList.Nodes.Clear();
            
            foreach (POS pl in PlugInData.PDL.Designs.Values)
            {
                posName = pl.Name;
                itemID = pl.itemID;

                mtn = tv_PoSList.Nodes.Add(posName);
                mtn.Tag = itemID;
                mtn.Name = "Tower" + towerNum;
                towerNum++;

                if (itemID != 0)
                {
                    apid = PlugInData.API_D.GetAPIDataMemberForTowerID(itemID);
                    if (apid != null)
                    {
                        tn = mtn.Nodes.Add("--> [" + apid.towerName + "] <" + apid.locName + ">");
                        tn.Name = itemID.ToString();
                    }
                }
            }
        }

        private TreeNode FindTreeNode(TreeNodeCollection tnc, string node)
        {
            foreach (TreeNode tn in tnc)
            {
                if (tn.Text == node)
                    return tn;
            }

            return null;
        }

        private void PopulateAPITowerView()
        {
            TreeNode mtn, tn, ltn, ctn;
            long towerNum = 1;
            string loc, cName, strSQL, twrName;
            DataSet locData;

            tv_APIList.Nodes.Clear();

            foreach (APITowerData apid in PlugInData.API_D.apiTower.Values)
            {
                cName = apid.corpName;
                ctn = FindTreeNode(tv_APIList.Nodes, cName);
                if (ctn == null)
                {
                    ctn = tv_APIList.Nodes.Add(cName);
                    ctn.Tag = "CORP";
                    ctn.Name = cName;
                }

                // Get Table With Tower or Tower Item Information
                if (apid.locName == "Unknown")
                {
                    strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + apid.locID + ";";
                    locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                    if (locData.Tables[0].Rows.Count > 0)
                        loc = locData.Tables[0].Rows[0].ItemArray[0].ToString();
                    else
                        loc = "Unknown";
                    apid.locName = loc;
                    PlugInData.API_D.SaveAPIListing();
                }
                else
                    loc = apid.locName;

                ltn = FindTreeNode(ctn.Nodes, loc);
                if (ltn == null)
                {
                    ltn = ctn.Nodes.Add(loc);
                    ltn.Tag = "LOC";
                    ltn.Name = loc;
                }

                // Scan Current POS List to see if this tower is already linked, if so - indicate
                // Tower name and link
                strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + apid.moonID + ";";
                locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                if (locData.Tables[0].Rows.Count > 0)
                    loc = locData.Tables[0].Rows[0].ItemArray[0].ToString();
                else
                    loc = "Unkown";
                twrName = apid.towerName + " < " + loc + " >";

                foreach (POS pl in PlugInData.PDL.Designs.Values)
                {
                    if (pl.itemID == apid.itemID)
                    {
                        // Found existing tower with ID
                        twrName += "< Linked: " + pl.Name + " >";
                    }
                }

                mtn = ltn.Nodes.Add(twrName);
                mtn.Tag = apid.itemID;
                mtn.Name = "Tower"+towerNum;
                towerNum++;

                if (apid.EnrUr > 0)
                {
                    tn = mtn.Nodes.Add("Enriched Uranium = " + String.Format("{0:0,0}", apid.EnrUr));
                    tn.Name = "EnrUr";
                    tn.Tag = apid.EnrUr;
                }
                if (apid.Oxygn > 0)
                {
                    tn = mtn.Nodes.Add("Oxygen = " + String.Format("{0:0,0}", apid.Oxygn));
                    tn.Name = "Oxygn";
                    tn.Tag = apid.Oxygn;
                }
                if (apid.MechP > 0)
                {
                    tn = mtn.Nodes.Add("Mechanical Parts = " + String.Format("{0:0,0}", apid.MechP));
                    tn.Name = "MechP";
                    tn.Tag = apid.MechP;
                }
                if (apid.Coolt > 0)
                {
                    tn = mtn.Nodes.Add("Coolant = " + String.Format("{0:0,0}", apid.Coolt));
                    tn.Name = "Coolt";
                    tn.Tag = apid.Coolt;
                }
                if (apid.Robot > 0)
                { 
                    tn = mtn.Nodes.Add("Robotics = " + String.Format("{0:0,0}", apid.Robot));
                    tn.Name = "Robot";
                    tn.Tag = apid.Robot;
                }
                if (apid.LiqOz > 0)
                {
                    tn = mtn.Nodes.Add("Liquid Ozone = " + String.Format("{0:0,0}", apid.LiqOz));
                    tn.Name = "LiqOz";
                    tn.Tag = apid.LiqOz;
                }
                if (apid.HvyWt > 0)
                {
                    tn = mtn.Nodes.Add("Heavy Water = " + String.Format("{0:0,0}", apid.HvyWt));
                    tn.Name = "HvyWt";
                    tn.Tag = apid.HvyWt;
                }
                if (apid.Charters > 0)
                {
                    tn = mtn.Nodes.Add("Faction Charters = " + String.Format("{0:0,0}", apid.Charters));
                    tn.Name = "Charters";
                    tn.Tag = apid.Charters;
                }
                if (apid.HeIso > 0)
                {
                    tn = mtn.Nodes.Add("Helium Isotopes = " + String.Format("{0:0,0}", apid.HeIso));
                    tn.Name = "HeIso";
                    tn.Tag = apid.HeIso;
                }
                if (apid.N2Iso > 0)
                {
                    tn = mtn.Nodes.Add("Nitrogen Isotopes = " + String.Format("{0:0,0}", apid.N2Iso));
                    tn.Name = "N2Iso";
                    tn.Tag = apid.N2Iso;
                }
                if (apid.H2Iso > 0)
                {
                    tn = mtn.Nodes.Add("Hydrogen Isotopes = " + String.Format("{0:0,0}", apid.H2Iso));
                    tn.Name = "H2Iso";
                    tn.Tag = apid.H2Iso;
                }
                if (apid.O2Iso > 0)
                {
                    tn = mtn.Nodes.Add("Oxygen Isotopes = " + String.Format("{0:0,0}", apid.O2Iso));
                    tn.Name = "O2Iso";
                    tn.Tag = apid.O2Iso;
                }
                if (apid.Stront > 0)
                {
                    tn = mtn.Nodes.Add("Strontium = " + String.Format("{0:0,0}", apid.Stront) );
                    tn.Name = "Stront";
                    tn.Tag = apid.Stront;
                }
            }
        }

        private void Tower_API_Linker_Shown(object sender, EventArgs e)
        {
            PopulateAPITowerView();
            PopulatePOSListView();
        }

        private void Tower_API_Linker_FormClosing(object sender, FormClosingEventArgs e)
        {
            PlugInData.PDL.SaveDesignListing();
        }

    }
}
