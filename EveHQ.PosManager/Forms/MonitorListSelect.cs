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
    public partial class MonitorListSelect : DevComponents.DotNetBar.Office2007Form
    {
        public PoSManMainForm myData;

        public MonitorListSelect()
        {
            InitializeComponent();
        }

        private void MonitorListSelect_Load(object sender, EventArgs e)
        {
            clb_PosList.Items.Clear();
            string nam;
            int indx = 0;

            // Scroll through the list of POSes
            foreach (New_POS pl in PlugInData.PDL.Designs.Values)
            {
                if (pl.Moon == "")
                    nam = pl.Name + " < " + pl.Moon + " >[ " + pl.CorpName + " ]";
                else
                    nam = pl.Name + " < " + pl.System + " >[ " + pl.CorpName + " ]";

                clb_PosList.Items.Add(nam);
                if (pl.Monitored)
                    clb_PosList.SetItemChecked(indx, true);

                indx++;
            }

            clb_PosList.Show();
            this.Focus();
            clb_PosList.Focus();
        }

        private void b_done_Click(object sender, EventArgs e)
        {
            bool chkd;
            string nm, nam;

            // Store the selected - to monitor - data
            for (int indx = 0; indx < clb_PosList.Items.Count; indx++)
            {
                chkd = clb_PosList.GetItemChecked(indx);

                nm = clb_PosList.Items[indx].ToString();
                foreach (New_POS pl in PlugInData.PDL.Designs.Values)
                {
                    if (pl.Moon == "")
                        nam = pl.Name + " < " + pl.Moon + " >[ " + pl.CorpName + " ]";
                    else
                        nam = pl.Name + " < " + pl.System + " >[ " + pl.CorpName + " ]";
                    
                    if (nm == nam)
                    {
                        if(chkd)
                            pl.Monitored = true;
                        else
                            pl.Monitored = false;

                        break;
                    }
                }
            }
            this.Dispose();
        }

    }
}
