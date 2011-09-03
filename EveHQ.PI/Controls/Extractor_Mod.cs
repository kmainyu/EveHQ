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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PI
{
    public partial class Extractor_Mod : UserControl
    {
        public PIMain PIM;
        private bool RefreshIP = false;
        public string ExtName;
        private bool RemCMSOpen = false;

        public Extractor_Mod()
        {
            InitializeComponent();
        }

        public Extractor_Mod(PIMain p, string nm)
        {
            InitializeComponent();
            PIM = p;
            ExtName = nm;
        }

        private void Extractor_Mod_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                RemCMSOpen = true;
            else
                if (!RemCMSOpen)
                {
                    Point clientp = new Point(e.X, e.Y);
                    if (sender.GetType().Name.Equals("PictureBox"))
                    {
                        PictureBox PB = (PictureBox)sender;
                        clientp.X += PB.Location.X;
                        clientp.Y += PB.Location.Y;
                    }
                    else if (sender.GetType().Name.Equals("LabelX"))
                    {
                        DevComponents.DotNetBar.LabelX PBX;
                        PBX = (DevComponents.DotNetBar.LabelX)sender;

                        clientp.X += PBX.Location.X;
                        clientp.Y += PBX.Location.Y;
                    }
                    else if (sender.GetType().Name.Equals("Label"))
                    {
                        Label PBX;
                        PBX = (Label)sender;

                        clientp.X += PBX.Location.X;
                        clientp.Y += PBX.Location.Y;
                    }
                    MouseEventArgs ne = new MouseEventArgs(e.Button, e.Clicks, clientp.X, clientp.Y, e.Delta);
                    base.OnMouseDown(ne);
                }
                else
                    RemCMSOpen = false;
        }

        public void Extractor_Mod_Update()
        {
            if (PIM != null)
                RefreshDataDisplay();
        }

        private void RefreshDataDisplay()
        {
            string nm, rcpUse, pnm;

            RefreshIP = true;
            pb_Module.BackgroundImage = PIM.GetImageForItem(PIM.Working.Extractors[ExtName].typeID);
            cb_Extracting.Text = PIM.Working.Extractors[ExtName].Extracting;
            nud_CycleExtract.Value = PIM.Working.Extractors[ExtName].ExtRate;
            nud_CycleTime.Value = PIM.Working.Extractors[ExtName].CycleTime;
            nud_RunTime.Value = PIM.Working.Extractors[ExtName].RunTime;

            nm = PIM.Working.Extractors[ExtName].Extracting;
            pnm = PIM.Working.PlanetType;
            nm = nm.Replace(pnm, "");
            nm = nm.Replace("Extractor", "");
            nm = nm.Trim();
            rcpUse = PIM.GetRecipesUsingItem(nm);

            lb_UsedFor.Text = rcpUse;

            RefreshIP = false;
        }

        private void Extractor_Mod_Load(object sender, EventArgs e)
        {
            SetAvailableExtractions();
            RefreshDataDisplay();
        }

        private void SetAvailableExtractions()
        {
            if (RefreshIP)
                return;

            Planet p;
            string s;

            p = PlugInData.GetPlanetData(PIM.Working.PlanetType);

            cb_Extracting.Items.Clear();

            foreach (Extractor ex in p.Extractors.Values)
            {
                s = PIM.Working.PlanetType;
                s = ex.Name.Replace(s, "");
                s = s.Replace("Extractor", "");
                s = s.Trim();
                if (s.Contains("Aqueous Liquid"))
                    s = "Aqueous Liquids";
                cb_Extracting.Items.Add(s);
            }
        }

        private void nud_CycleExtract_ValueChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            PIM.Working.Extractors[ExtName].ExtRate = Convert.ToInt32(nud_CycleExtract.Value);
            PIM.ExtractorModuleChanged();
        }

        private void nud_CycleTime_ValueChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            PIM.Working.Extractors[ExtName].CycleTime = Convert.ToInt32(nud_CycleTime.Value);
            PIM.ExtractorModuleChanged();
        }

        private void nud_RunTime_ValueChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            PIM.Working.Extractors[ExtName].RunTime = Convert.ToInt32(nud_RunTime.Value);
            PIM.ExtractorModuleChanged();
        }

        private void cb_Extracting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RefreshIP)
                return;

            PIM.Working.Extractors[ExtName].Extracting = cb_Extracting.Text;
            RefreshDataDisplay();
            PIM.ExtractorModuleChanged();
        }

        private void tsmi_RemoveModule_Click(object sender, EventArgs e)
        {
            PIM.PIM_RemoveModule(ExtName, "Extractor");
            this.Dispose();
        }
    }
}
