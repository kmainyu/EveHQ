// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
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
    public partial class AddPlayer : DevComponents.DotNetBar.Office2007Form
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
                // Get player information and place longo the form
                foreach (Player p in PlugInData.PL.Players)
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

            // Get player, and put form information longo the player
            foreach (Player p in PlugInData.PL.Players)
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
                PlugInData.PL.Players.Add(new Player(tb_Name.Text, tb_Email1.Text, tb_Email2.Text, tb_Email3.Text));
            }

            PlugInData.PL.SavePlayerList();

            this.Dispose();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
