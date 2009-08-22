using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class AddPlayer : Form
    {
        public PoSManMainForm myData;

        public AddPlayer()
        {
            InitializeComponent();
        }

        public void SetupData(string tp, string pl)
        {
            this.Text = tp;

            if (pl != "")
            {
                // Get player information and place into the form
                foreach (Player p in myData.PL.Players)
                {
                    if (p.Name == pl)
                    {
                        tb_Name.Text = p.Name;
                        tb_Email1.Text = p.Email1;
                        tb_Email2.Text = p.Email2;
                        tb_Email3.Text = p.Email3;
                        break;
                    }
                    else
                    {
                        // Nothing found - houston we have a problem
                        this.Dispose();
                    }
                }
            }
            else
            {
                tb_Name.Text = "";
                tb_Email1.Text = "";
                tb_Email2.Text = "";
                tb_Email3.Text = "";
            }
        }

        private void b_Done_Click(object sender, EventArgs e)
        {
            bool foundPlayer = false;

            // Get player, and put form information into the player
            foreach (Player p in myData.PL.Players)
            {
                if (p.Name == tb_Name.Text)
                {
                    p.Email1 = tb_Email1.Text;
                    p.Email2 = tb_Email2.Text;
                    p.Email3 = tb_Email3.Text;
                    foundPlayer = true;
                    break;
                }
            }

            if (!foundPlayer)
            {
                myData.PL.Players.Add(new Player(tb_Name.Text, tb_Email1.Text, tb_Email2.Text, tb_Email3.Text));
            }

            myData.PL.SavePlayerList();

            this.Dispose();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
