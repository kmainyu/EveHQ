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
    public partial class MonitorListSelect : Form
    {
        public PoSManMainForm myData;

        public MonitorListSelect()
        {
            InitializeComponent();
        }

        private void MonitorListSelect_Load(object sender, EventArgs e)
        {
            clb_PosList.Items.Clear();
            string nam, nm;
            int indx = 0;

            // Scroll through the list of POSes
            foreach (POS pl in myData.POSList.Designs)
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
            string nm;

            // Store the selected - to monitor - data
            for (int indx = 0; indx < clb_PosList.Items.Count; indx++)
            {
                chkd = clb_PosList.GetItemChecked(indx);

                nm = clb_PosList.Items[indx].ToString();
                foreach (POS pl in myData.POSList.Designs)
                {
                    if (nm.Contains(pl.Name))
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
