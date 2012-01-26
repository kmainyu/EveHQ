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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PI
{
    public partial class CopyRename : Form
    {
        public CopyRename()
        {
            InitializeComponent();
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            if (tb_NewName.Text.Length < 1)
                return;

            if (PlugInData.Facilities.ContainsKey(tb_NewName.Text))
            {
                MessageBox.Show("A facility with this name already Exists, please chose a new Name.", "Entry Error", MessageBoxButtons.OK);
                return;
            }

            PlugInData.NewFacName = tb_NewName.Text;
            this.Dispose();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            PlugInData.NewFacName = "";
            this.Dispose();
        }

        private void CopyRename_Load(object sender, EventArgs e)
        {
            lb_FacilityName.Text = PlugInData.CurFacName;
            tb_NewName.Text = PlugInData.CurFacName;
        }
    }
}
