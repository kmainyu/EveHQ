using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class Notification : Form
    {
        public PoSManMainForm myData;
        public int EditIndex;

        public Notification()
        {
            InitializeComponent();
        }

        public void SetupData(string tp, string twr, string nt)
        {
            string line;
            bool inList = false;
            int ind = 0;
            EditIndex = -1;

            this.Text = tp;

            // Populate Tower Pull Down
            foreach (POS p in myData.POSList.Designs)
                cb_Tower.Items.Add(p.Name);

            if ((twr != "") && (nt != ""))
            {
                // Get player information and place into the form
                foreach (PosNotify pn in myData.NL.NotifyList)
                {
                    line = pn.Type + " [ Start at " + pn.InitQty + " " + pn.Initial + " | Every " + pn.FreqQty + " " + pn.Frequency + " after.]";
                    if ((pn.Tower == twr) && (nt == line))
                    {
                        EditIndex = ind;
                        // We are doing an edit
                        cb_Type.Text = pn.Type;
                        cb_Initial.Text = pn.Initial;
                        cb_Frequency.Text = pn.Frequency;
                        cb_Tower.Text = twr;
                        nud_Initial.Value = pn.InitQty;
                        nud_Frequency.Value = pn.FreqQty;

                        foreach (Player p in myData.PL.Players)
                        {
                            inList = false;
                            foreach (Player pl in pn.PList.Players)
                            {
                                if (pl.Name == p.Name)
                                {
                                    inList = true;
                                    break;
                                }
                            }
                            if(inList)
                                clb_PlayersToNotify.Items.Add(p.Name, true);
                            else
                                clb_PlayersToNotify.Items.Add(p.Name, false);
                        }
                        break;
                    }
                    ind++;
                }

                if (EditIndex < 0)
                {
                    // We are doing an Add/New
                    cb_Type.SelectedIndex = 1;
                    cb_Initial.SelectedIndex = 1;
                    cb_Frequency.SelectedIndex = 1;
                    cb_Tower.Text = "Select Desired Tower";
                    nud_Initial.Value = 1;
                    nud_Frequency.Value = 1;
                    foreach (Player p in myData.PL.Players)
                    {
                        clb_PlayersToNotify.Items.Add(p.Name, false);
                    }
                }
            }
            else
            {
                EditIndex = -1;
                // We are doing an Add/New
                cb_Type.SelectedIndex = 1;
                cb_Initial.SelectedIndex = 1;
                cb_Frequency.SelectedIndex = 1;
                cb_Tower.Text = "Select Desired Tower";
                nud_Initial.Value = 1;
                nud_Frequency.Value = 1;
                foreach (Player p in myData.PL.Players)
                {
                    clb_PlayersToNotify.Items.Add(p.Name, false);
                }
            }
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void b_Done_Click(object sender, EventArgs e)
        {
            PosNotify newPN = new PosNotify();

            newPN.Tower = cb_Tower.Text;
            newPN.Initial = cb_Initial.Text;
            newPN.Frequency = cb_Frequency.Text;
            newPN.Type = cb_Type.Text;
            newPN.InitQty = nud_Initial.Value;
            newPN.FreqQty = nud_Frequency.Value;

            foreach (Player p in myData.PL.Players)
            {
                if (clb_PlayersToNotify.GetItemCheckState(clb_PlayersToNotify.Items.IndexOf(p.Name)) == CheckState.Checked)
                    newPN.PList.Players.Add(p);
            }

            if (EditIndex < 0)
                myData.NL.NotifyList.Add(newPN);
            else
            {
                myData.NL.NotifyList[EditIndex] = new PosNotify(newPN);
            }

            myData.NL.SaveNotificationList();

            this.Dispose();
        }
    }
}
