﻿// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2012  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 26 January 20121, and as distributed, it includes or is 
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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace EveHQ.RouteMap
{
    public partial class SelAltNode : DevComponents.DotNetBar.Office2007Form
    {
        ArrayList AltNodes;

        public SelAltNode(ArrayList an)
        {
            AltNodes = an;
            InitializeComponent();
            DisplayNodes();
        }

        public void DisplayNodes()
        {
            AlternateSystem AS;
            int cnt = 0;

            gp_BG.Controls.Clear();
            foreach (AlternateNode AN in AltNodes)
            {
                AS = new AlternateSystem(AN, PlugInData.RMMF, this);
                AS.Location = new Point(0, (AS.Height + 1) * cnt);
                gp_BG.Controls.Add(AS);
                cnt++;
            }
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void AltNodeSelected()
        {
            this.Dispose();
        }
    }
}
