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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace EveHQ.PosManager
{
    public partial class ReactionTower : UserControl
    {
        string TName = "";
        string TLocation = "";
        long ID;
        ArrayList ReactInfo; // 1 = 0-3, 2 = 4-7, 3 = 8-11, 4 = 12-15, 5 = 16-19, etc...
        PoSManMainForm PMMF;

        public ReactionTower()
        {
            InitializeComponent();
        }

        public void UpdateReactionInformation(string nm, string loc, ArrayList ri, PoSManMainForm pm, long id)
        {
            int totBar;
            ReactBar rBar;
            PMMF = pm;
            TName = nm;
            ID = id;
            TLocation = loc;
            ReactInfo = new ArrayList(ri);

            gp_TwrReactBG.Text = TName;
            if (TLocation.Length < 1)
                TLocation = "Undefined";
            l_Location.Text = TLocation;

            totBar = 0;

            foreach (ReactMod rm in ReactInfo)
            {
                totBar++;

                rBar = new ReactBar();
                rBar.pBar.Name = "Silo_" + totBar;
                rBar.pBar.Text = rm.name + " (" + rm.timeS + ")";
                rBar.pBar.Maximum = rm.maxQ;
                rBar.pBar.Value = rm.capQ;

                rBar.Location = new Point(0, (13 + ((totBar -1) * 18)));
                rBar.Click += new System.EventHandler(this.gp_TwrReactBG_Click);
                rBar.pBar.Click += new System.EventHandler(this.gp_TwrReactBG_Click);
                
                gp_TwrReactBG.Controls.Add(rBar);
            }
        }

        private void gp_TwrReactBG_Click(object sender, EventArgs e)
        {
            // Do RMA. Call to give selected tower
            PMMF.TowerSelectedForReactionLinks(TName);
        }
    }
}
