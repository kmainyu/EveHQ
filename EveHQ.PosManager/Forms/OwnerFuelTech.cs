using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class OwnerFuelTech : Form
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
            APITowerData atd;
            string twrName;

            twrName = myData.selName.Substring(0, myData.selName.IndexOf(" <"));

            foreach (POS pl in myData.POSList.Designs)
            {
                if (twrName == pl.Name)
                {
                    pl.Owner = cb_OwnerName.Text;
                    pl.FuelTech = cb_FuelTechName.Text;

                    if (rb_Corp.Checked)
                    {
                        for (int x = 0; x < myData.API_D.apiTower.Count; x++)
                        {
                            atd = (APITowerData)myData.API_D.apiTower.GetByIndex(x);
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

            twrName = myData.selName.Substring(0, myData.selName.IndexOf(" <"));
            foreach (POS pl in myData.POSList.Designs)
            {
                if (twrName == pl.Name)
                {
                    if (pl.Owner == pl.CorpName)
                        rb_Corp.Checked = true;
                    else
                        rb_Personal.Checked = true;
                }
            }
            PopulateOwnerNameList();
            PopulateFuelTechNameList();
        }

        private void PopulateOwnerNameList()
        {
            string ownName = "";
            string twrName;

            twrName = myData.selName.Substring(0, myData.selName.IndexOf(" <"));

            foreach (POS pl in myData.POSList.Designs)
            {
                if (twrName == pl.Name)
                {
                    if (rb_Corp.Checked)
                        ownName = pl.CorpName;
                    else
                        ownName = pl.Owner;
                    break;
                }
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

            twrName = myData.selName.Substring(0, myData.selName.IndexOf(" <"));
            cb_FuelTechName.Items.Clear();
            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                cb_FuelTechName.Items.Add(selPilot.Name);
            }

            foreach (POS pl in myData.POSList.Designs)
            {
                if (twrName == pl.Name)
                {
                    cb_FuelTechName.Text = pl.FuelTech;
                    break;
                }
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
