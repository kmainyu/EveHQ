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

namespace EveHQ.PI
{
    public partial class AddModule : Form
    {
        PIMain PIM;

        public AddModule()
        {
            InitializeComponent();
        }

        public AddModule(PIMain p, Bitmap e, Bitmap pr, Bitmap s, Bitmap l, ArrayList toolTips)
        {
            InitializeComponent();
            PIM = p;
            pb_Extractor.BackgroundImage = e;
            toolTip1.SetToolTip(pb_Extractor, toolTips[0].ToString());
            pb_Processor.BackgroundImage = pr;
            toolTip1.SetToolTip(pb_Processor, toolTips[1].ToString());
            pb_Storage.BackgroundImage = s;
            toolTip1.SetToolTip(pb_Storage, toolTips[2].ToString());
            pb_LaunchPad.BackgroundImage = l;
            toolTip1.SetToolTip(pb_LaunchPad, toolTips[3].ToString());
        }

        private void pb_Extractor_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PIM.SelectedMod = 5;
            else
                PIM.SelectedMod = 1;

            this.Dispose();
        }

        private void pb_Processor_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PIM.SelectedMod = 2;
            else
                PIM.SelectedMod = 0;

            this.Dispose();
        }

        private void pb_Storage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PIM.SelectedMod = 3;
            else
                PIM.SelectedMod = 0;

            this.Dispose();
        }

        private void pb_LaunchPad_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                PIM.SelectedMod = 4;
            else
                PIM.SelectedMod = 0;

            this.Dispose();
        }
    }
}
