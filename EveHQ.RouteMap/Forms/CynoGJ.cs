// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 4 September 2011, and as distributed, it includes or is 
// derivative of works licensed under the GNU General Public License.
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.RouteMap
{
    public partial class CynoGJ : UserControl
    {
        private RouteMapMainForm RMA;
        private CynoGenJam CGJ;
        private bool Loading = false;

        public CynoGJ(RouteMapMainForm rm, CynoGenJam c)
        {
            RMA = rm;
            CGJ = c;
            InitializeComponent();
            DisplayData();
        }

        public void DisplayData()
        {
            Loading = true;
            SystemCelestials sc = new SystemCelestials();
            sc.GetSystemMoonsForID(CGJ.GenSys.ID);

            cb_SystemMoon.Items.Clear();
            cb_SystemMoon.Items.Add("Unknown");

            cb_SystemMoon.Items.AddRange(sc.GetSystemMoonsForID(CGJ.GenSys.ID).ToArray());

            lb_SysName.Text = CGJ.GenSys.Name;
            cb_SystemMoon.Text = CGJ.moon;
            rb_Jammer.Checked = CGJ.IsJammer;
            Loading = false;
        }

        private void cb_SystemMoon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Loading)
                return;

            CGJ.moon = cb_SystemMoon.Text;
            RMA.UpdateCynoGen(CGJ);
        }

        private void rb_Jammer_CheckedChanged(object sender, EventArgs e)
        {
            if (Loading)
                return;

            CGJ.IsJammer = rb_Jammer.Checked;
            RMA.UpdateCynoGen(CGJ);
        }

        private void rb_Generator_CheckedChanged(object sender, EventArgs e)
        {
            //CGJ.IsJammer = !rb_Generator.Checked;
        }

        private void tmsi_RemoveCyno_Click(object sender, EventArgs e)
        {
            RMA.RemoveCynoGen(CGJ);
        }
    }
}
