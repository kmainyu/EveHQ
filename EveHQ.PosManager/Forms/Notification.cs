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
            int ind = 0, sTwr;
            EditIndex = -1;

            this.Text = tp;

            // Populate Tower Pull Down
            foreach (POS p in myData.POSList.Designs)
            {
                if (p.Monitored)
                {
                    sTwr = clb_TowersToNotify.Items.Add(p.Name);

                    if ((twr != "") && (p.Name == twr))
                        clb_TowersToNotify.SetItemChecked(sTwr, true);
                }
            }

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
                        nud_Initial.Value = pn.InitQty;
                        nud_Frequency.Value = pn.FreqQty;
                        clb_TowersToNotify.Enabled = false;

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
                    cb_Type.SelectedIndex = 0;
                    cb_Initial.SelectedIndex = 0;
                    cb_Frequency.SelectedIndex = 0;
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
                cb_Type.SelectedIndex = 0;
                cb_Initial.SelectedIndex = 0;
                cb_Frequency.SelectedIndex = 0;
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
            PosNotify newPN;
            bool gotPlayer = false;

            for(int x=0; x < clb_TowersToNotify.Items.Count; x++)
            {
                if (clb_TowersToNotify.GetItemChecked(x))
                {
                    newPN = new PosNotify();
                    newPN.Tower = clb_TowersToNotify.Items[x].ToString();
                    newPN.Initial = cb_Initial.Text;
                    newPN.Frequency = cb_Frequency.Text;
                    newPN.Type = cb_Type.Text;
                    newPN.InitQty = nud_Initial.Value;
                    newPN.FreqQty = nud_Frequency.Value;

                    foreach (Player p in myData.PL.Players)
                    {
                        if (clb_PlayersToNotify.GetItemCheckState(clb_PlayersToNotify.Items.IndexOf(p.Name)) == CheckState.Checked)
                        {
                            newPN.PList.Players.Add(p);
                            gotPlayer = true;
                        }
                    }

                    if (!gotPlayer)
                    {
                        MessageBox.Show("You need to select a Player for the Rule to apply to!", "No Player Selected!", MessageBoxButtons.OK);
                        return;
                    }

                    if (EditIndex < 0)
                        myData.NL.NotifyList.Add(newPN);
                    else
                    {
                        myData.NL.NotifyList[EditIndex] = new PosNotify(newPN);
                        break;
                    }
                }
            }
            myData.NL.SaveNotificationList();

            this.Dispose();
        }
    }
}
