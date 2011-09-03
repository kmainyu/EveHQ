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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PI
{
    public partial class ExtractRateDialog : Form
    {
        public int ERate;
        public int ct;

        public ExtractRateDialog()
        {
            InitializeComponent();
        }

        private void b_Accept_Click(object sender, EventArgs e)
        {
            int ExtractAmt;
            int CycleTime;

            ExtractAmt = Convert.ToInt32(nud_ExtractRate.Value);

            if (rb_Cycle5.Checked)
                CycleTime = 5;
            else if (rb_Cycle15.Checked)
                CycleTime = 15;
            else if (rb_Cycle30.Checked)
                CycleTime = 30;
            else 
                CycleTime = 60;

            PlugInData.ExtractRate = ExtractAmt;
            PlugInData.CycleTime = CycleTime;

            this.Dispose();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            PlugInData.ExtractRate = 0;
            PlugInData.CycleTime = -1;

            this.Dispose();
        }

        private void ExtractRateDialog_Load(object sender, EventArgs e)
        {
            nud_ExtractRate.Value = Convert.ToDecimal(ERate);

            if (ct == 5)
                rb_Cycle5.Checked = true;
            else if (ct == 15)
                rb_Cycle15.Checked = true;
            else if (ct == 30)
                rb_Cycle30.Checked = true;
            else
                rb_Cycle60.Checked = true;

            nud_ExtractRate.Select();
        }
    }
}
