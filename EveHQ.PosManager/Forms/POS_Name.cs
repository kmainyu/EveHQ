using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class POS_Name : Form
    {
        public PoSManMainForm myData;
        public TowerReactMod trData;
        public bool NewPOS = false;
        public bool CopyPOS = false;
        public bool GetValue = false;

        public POS_Name()
        {
            InitializeComponent();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            if (GetValue)
            {
                trData.GetVal = 0;
                Dispose();
            }
            else
            {
                myData.NewName = myData.CurrentName;
                Dispose();
            }
        }

        private void b_Done_Click(object sender, EventArgs e)
        {
            if (GetValue)
            {
                trData.GetVal = Convert.ToDecimal(tb_NewName.Text);
                Dispose();
            }
            else
            {
                myData.NewName = tb_NewName.Text;
                Dispose();
            }
        }

        private void POS_Name_Load(object sender, EventArgs e)
        {
            if (NewPOS)
            {
                l_NameLabel.Visible = false;
                l_CurrentName.Visible = false;
                this.Text = "Enter a Name for your new POS Design";
                myData.NewName = "";
            }
            else if (CopyPOS)
            {
                l_NameLabel.Visible = true;
                l_CurrentName.Visible = true;
                l_CurrentName.Text = myData.CurrentName;
                this.Text = "Enter a New Name for your POS Design Copy";
                myData.NewName = "";
            }
            else if (GetValue)
            {
                l_NameLabel.Visible = false;
                l_CurrentName.Visible = false;
                this.Text = "Enter Current Silo Fill Quantity";
                l_EnterName.Text = "Enter Fill Quantity:";
            }
            else
            {
                l_NameLabel.Visible = true;
                l_CurrentName.Visible = true;
                l_CurrentName.Text = myData.CurrentName;
                this.Text = "Enter a New Name for your current POS Design";
                myData.NewName = "";
            }
        }
    }
}
