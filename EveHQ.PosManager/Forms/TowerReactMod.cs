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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PosManager
{
    public partial class TowerReactMod : UserControl
    {
        public Module ReactMod;
        public decimal SiloMult;
        public PoSManMainForm myData;
        public PictureBox pb;
        public decimal GetVal;
        //public DateTime rTime;
        string csmSel;

        public TowerReactMod()
        {
            InitializeComponent();
            ReactMod = new Module();
            SiloMult = 1;
            GetVal = 0;
        }

        public TowerReactMod(string nm)
        {
            InitializeComponent();
            ReactMod = new Module();
            SiloMult = 1;
            GetVal = 0;
            l_ModuleName.Text = nm;
        }

        public void SetModuleData(Module m, decimal sm)//, DateTime rtm)
        {
            ReactMod = new Module(m);
            SiloMult = sm;
            ToolStripMenuItem tsmi;
            //rTime = rtm;

            // Build the R-Click Menu list here
            l_ExtraInfo.Text = "";
            l_RunTime.Text = "";
            if (ReactMod.Category == "Silo")
            {
                if ((ReactMod.Name == "Silo") || (ReactMod.Name == "Coupling Array"))
                {
                    // Moon Minerals
                    // Simple Reaction Minerals
                    // Complex Reaction Minerals
                    CM_MinReactSel.Items.Clear();
                    CM_MinReactSel.Items.Add("Moon Minerals");
                    CM_MinReactSel.Items.Add("Simple Reaction");
                    CM_MinReactSel.Items.Add("Complex Reaction");
                    CM_MinReactSel.Items.Add("Refined Minerals");

                    if (ReactMod.Name == "Coupling Array")
                    {
                        CM_MinReactSel.Items.Add("Biochemicals");
                        CM_MinReactSel.Items.Add("Catalysts");
                        CM_MinReactSel.Items.Add("General Goods");
                        CM_MinReactSel.Items.Add("Hazardous Materials");
                        CM_MinReactSel.Items.Add("Hybrid Polymers");
                    }

                    foreach (MoonSiloReactMineral msr in ReactMod.MSRList)
                    {
                        if (msr.groupID == 427)
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[0];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if (msr.groupID == 428)
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[1];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if (msr.groupID == 429)
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[2];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if (msr.groupID == 18)
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[3];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if ((msr.groupID == 711) && (ReactMod.Name == "Coupling Array"))
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[3];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if ((msr.groupID == 284) && (ReactMod.Name == "Coupling Array"))
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[3];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if ((msr.groupID == 280) && (ReactMod.Name == "Coupling Array"))
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[3];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if ((msr.groupID == 712) && (ReactMod.Name == "Coupling Array"))
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[3];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                        else if ((msr.groupID == 974) && (ReactMod.Name == "Coupling Array"))
                        {
                            tsmi = (ToolStripMenuItem)CM_MinReactSel.Items[3];
                            tsmi.DropDownItems.Add(msr.name);
                            tsmi.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(CM_MinReactSel_DropDownItemClicked);
                        }
                    }
                }
                else
                {
                    CM_MinReactSel.Items.Clear();
                    foreach (MoonSiloReactMineral msr in ReactMod.MSRList)
                    {
                        CM_MinReactSel.Items.Add(msr.name);
                    }
                }

                pb_FillLevel.Show();
                b_SetEmpty.Show();
                b_SetFill.Show();
                b_SetFull.Show();
                ShowHideInOut();

                // Need to set proper values for the various components to display
                ComputeAndShowFillLevels();
            }
            else if (ReactMod.Category == "Moon Mining")
            {
                // Moon Minerals Only
                CM_MinReactSel.Items.Clear();
                foreach (MoonSiloReactMineral msr in ReactMod.MSRList)
                {
                    CM_MinReactSel.Items.Add(msr.name);
                }
                pb_FillLevel.Hide();
                b_SetEmpty.Hide();
                b_SetFill.Hide();
                b_SetFull.Hide();
                ShowHideInOut();
            }
            else if (ReactMod.Category == "Mobile Reactor")
            {
                CM_MinReactSel.Items.Clear();
                foreach (Reaction r in ReactMod.ReactList)
                {
                    CM_MinReactSel.Items.Add(r.reactName);
                }
                pb_FillLevel.Hide();
                b_SetEmpty.Hide();
                b_SetFill.Hide();
                b_SetFull.Hide();
                ShowHideInOut();
            }
        }

        private void ComputeAndShowFillLevels()
        {
            decimal SiloCap;
            string CapText;
            //TimeSpan ts;

            // Must determine the following
            // 1. Actual Silo Capacity - in m3 and in Qty of sel Mineral
            // 2. Actual Silo Qty - Current Qty (Qty and Vol)
            // 3. Create Display Text for Prog. Bar
            SiloCap = ReactMod.Capacity * SiloMult;

            // Fill Bar and Amount
            pb_FillLevel.Maximum = Convert.ToInt32(SiloCap);
            pb_FillLevel.Value = Convert.ToInt32(ReactMod.CapVol);
            CapText = ReactMod.CapQty + " / " + Math.Truncate(ReactMod.MaxQty) + " (" + ReactMod.CapVol + " m3)";
            pb_FillLevel.Text = CapText;

            // In/Out Warn Setting
            l_RunTime.Text = PlugInData.ConvertReactionHoursToTextDisplay(ReactMod.FillEmptyTime);

            // Time till next update
            //ts = rTime.Subtract(DateTime.Now);
            //lbx_UpdateTimer.Text = "Next Update: " + PlugInData.ConvertSecondsToTextDisplay(3600 - (Math.Abs(Convert.ToDecimal(ts.TotalSeconds))));
        }

        private void ShowHideInOut()
        {
            long num;
            string tTip;

            input1.Hide();
            input2.Hide();
            input3.Hide();
            input4.Hide();
            input5.Hide();
            input6.Hide();
            output1.Hide();
            output2.Hide();

            if (ReactMod.selReact.reactName != "")
            {
                num = ReactMod.selReact.inputs.Count;
                if (num > 0)
                    input1.Show();
                if (num > 1)
                    input2.Show();
                if (num > 2)
                    input3.Show();
                if (num > 3)
                    input4.Show();
                if (num > 4)
                    input5.Show();
                if (num > 5)
                    input6.Show();

                num = ReactMod.selReact.outputs.Count;
                if (num > 0)
                    output1.Show();
                if (num > 1)
                    output2.Show();

                SetItemSelectedIcon(ReactMod.selReact.reactName, false, false);
                tTip = "";
                foreach (InOutData iod in ReactMod.selReact.inputs)
                {
                    if (tTip.Length > 0)
                        tTip += "\n";

                    tTip += GetInputAndModuleForIO(iod, true);
                }
                foreach (InOutData iod in ReactMod.selReact.outputs)
                {
                    if (tTip.Length > 0)
                        tTip += "\n";

                    tTip += GetInputAndModuleForIO(iod, false);
                }
                toolTip1.SetToolTip(l_ExtraInfo, tTip);
            }
            else if (ReactMod.selMineral.name != "")
            {
                input1.Show();
                output1.Show();

                SetItemSelectedIcon(ReactMod.selMineral.name, true, false);
            }
        }

        private void TowerReactMod_Load(object sender, EventArgs e)
        {
            input1.Image = null;
            input1.BackgroundImage = null;
            input1.AllowDrop = true;
            input2.Image = null;
            input2.BackgroundImage = null;
            input2.AllowDrop = true;
            input3.Image = null;
            input3.BackgroundImage = null;
            input3.AllowDrop = true;
            input4.Image = null;
            input4.BackgroundImage = null;
            input4.AllowDrop = true;
            input5.Image = null;
            input5.BackgroundImage = null;
            input5.AllowDrop = true;
            input6.Image = null;
            input6.BackgroundImage = null;
            input6.AllowDrop = true;
 
            output1.Image = null;
            output1.BackgroundImage = null;
            output1.AllowDrop = true;
            output2.Image = null;
            output2.BackgroundImage = null;
            output2.AllowDrop = true;
        }

        private void ReactionMineral_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    // User wishes to select the desired Reaction/Mineral type
            //    // If Reactor, bring up reaction list
            //    // If Silo, Junction - bring up mineral list
            //}
        }

        private void rb_Empty_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rb_Full_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void input_MouseClick(object sender, MouseEventArgs e)
        {
            string pbName;
            if (e.Button == MouseButtons.Left)
            {
                // User wishes to set an input or output link - or finish one
                pb = (PictureBox)sender;
                pb.BorderStyle = BorderStyle.Fixed3D;
                pbName = pb.Name.ToString();
                myData.TowerReactModuleUpdated(ReactMod, 4, 0, pbName);
            }
        }

        private void output_MouseClick(object sender, MouseEventArgs e)
        {
            string pbName;
            if (e.Button == MouseButtons.Left)
            {
                // User wishes to set an input or output link - or finish one
                pb = (PictureBox)sender;
                pb.BorderStyle = BorderStyle.Fixed3D;
                pbName = pb.Name.ToString();
                myData.TowerReactModuleUpdated(ReactMod, 3, 0, pbName);
            }
        }

        private void CM_MinReactSel_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            CM_MinReactSel_ItemClicked(sender, e);
        }

        public void SetLinkColor(Color c, string locName)
        {
            PictureBox pb;
            this.Controls[locName].BackColor = c;
            pb = (PictureBox)this.Controls[locName];
            pb.BorderStyle = BorderStyle.FixedSingle;
        }

        public void ClearLinkSel(string locName)
        {
            PictureBox pb;
            pb = (PictureBox)this.Controls[locName];
            pb.BorderStyle = BorderStyle.FixedSingle;
        }

        private void SetItemSelectedIcon(string react, bool isMin, bool overWrt)
        {
            MoonSiloReactMineral ir, or;
            long ioCnt = 0;

            l_ExtraInfo.Text = react;
            if (isMin)
            {
                ReactionMineral.BackgroundImageLayout = ImageLayout.Stretch;
                ReactionMineral.BackgroundImage = myData.GetIcon(ReactMod.selMineral.typeID.ToString());
                input1.Show();
                input1.BackgroundImage = myData.GetIcon(ReactMod.selMineral.typeID.ToString());
                toolTip1.SetToolTip(input1, ReactMod.selMineral.name);
                output1.Show();
                output1.BackgroundImage = myData.GetIcon(ReactMod.selMineral.typeID.ToString());
                toolTip1.SetToolTip(output1, ReactMod.selMineral.name);
            }
            else
            {
                ReactionMineral.BackgroundImageLayout = ImageLayout.Stretch;
                ReactionMineral.BackgroundImage = myData.GetIcon(ReactMod.selReact.typeID.ToString());
                input1.Hide();
                input2.Hide();
                input3.Hide();
                input4.Hide();
                input5.Hide();
                input6.Hide();
                output1.Hide();
                output2.Hide();

                foreach (InOutData io in ReactMod.selReact.inputs)
                {
                    ir = GetInputForType(io.typeID);
                    ioCnt++;
                    switch (ioCnt)
                    {
                        case 1:
                            input1.Show();
                            input1.BackgroundImage = myData.GetIcon(ir.typeID.ToString());
                            toolTip1.SetToolTip(input1, ir.name);
                            break;
                        case 2:
                            input2.Show();
                            input2.BackgroundImage = myData.GetIcon(ir.typeID.ToString());
                            toolTip1.SetToolTip(input2, ir.name);
                            break;
                        case 3:
                            input3.Show();
                            input3.BackgroundImage = myData.GetIcon(ir.typeID.ToString());
                            toolTip1.SetToolTip(input3, ir.name);
                            break;
                        case 4:
                            input4.Show();
                            input4.BackgroundImage = myData.GetIcon(ir.typeID.ToString());
                            toolTip1.SetToolTip(input4, ir.name);
                            break;
                        case 5:
                            input5.Show();
                            input5.BackgroundImage = myData.GetIcon(ir.typeID.ToString());
                            toolTip1.SetToolTip(input5, ir.name);
                            break;
                        case 6:
                            input6.Show();
                            input6.BackgroundImage = myData.GetIcon(ir.typeID.ToString());
                            toolTip1.SetToolTip(input6, ir.name);
                            break;
                        default:
                            break;
                    }
                }
                ioCnt = 0;
                foreach (InOutData io in ReactMod.selReact.outputs)
                {
                    or = GetOutputForType(io.typeID);
                    ioCnt++;
                    switch (ioCnt)
                    {
                        case 1:
                            output1.Show();
                            output1.BackgroundImage = myData.GetIcon(or.typeID.ToString());
                            toolTip1.SetToolTip(output1, or.name);
                            break;
                        case 2:
                            output2.Show();
                            output2.BackgroundImage = myData.GetIcon(or.typeID.ToString());
                            toolTip1.SetToolTip(output2, or.name);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void CM_MinReactSel_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Reaction nr;
            MoonSiloReactMineral msr;
            string react, tTip;
            bool overWrite = false;

            react = e.ClickedItem.Text;
            if (react.Contains("Moon Mineral") || react.Contains("Simple Reaction") || react.Contains("Complex Reaction") ||
                react.Contains("Refined Minerals") || react.Contains("Biochemicals") || react.Contains("Catalysts") ||
                react.Contains("General Goods") || react.Contains("Hazardous Materials") || react.Contains("Hybrid Polymers"))
                return;

            nr = GetReactionForName(react);
            if (nr == null)
            {
                msr = GetMineralForName(react);
                if (ReactMod.selMineral.name != msr.name)
                    overWrite = true;

                ReactMod.selMineral = new MoonSiloReactMineral(msr);
                if (ReactMod.selMineral.volume > 0)
                    ReactMod.MaxQty = (ReactMod.Capacity * SiloMult) / ReactMod.selMineral.volume;
                else
                    ReactMod.MaxQty = (ReactMod.Capacity * SiloMult);

                SetItemSelectedIcon(react, true, overWrite);
                ComputeAndShowFillLevels();
                if(overWrite)
                    myData.TowerReactModuleUpdated(ReactMod, 11, 0, "");
                else
                    myData.TowerReactModuleUpdated(ReactMod, 1, 0, "");
            }
            else
            {
                if (ReactMod.selReact.reactName != nr.reactName)
                    overWrite = true;

                ReactMod.selReact = new Reaction(nr);
                SetItemSelectedIcon(react, false, overWrite);
                if (overWrite)
                    myData.TowerReactModuleUpdated(ReactMod, 12, 0, "");
                else
                    myData.TowerReactModuleUpdated(ReactMod, 2, 0, "");

                tTip = "";
                foreach (InOutData iod in ReactMod.selReact.inputs)
                {
                    if (tTip.Length > 0)
                        tTip += "\n";

                    tTip += GetInputAndModuleForIO(iod, true);
                }
                foreach (InOutData iod in ReactMod.selReact.outputs)
                {
                    if (tTip.Length > 0)
                        tTip += "\n";

                    tTip += GetInputAndModuleForIO(iod, false);
                }
                toolTip1.SetToolTip(l_ExtraInfo, tTip);
                // Reaction - if possible it would be nice to set the module tooltip to
                // indicate the needed input and output module types that are needed
            }
        }

        private string GetInputAndModuleForIO(InOutData iod, bool inP)
        {
            string retStr = "";
            if (inP)
            {
                foreach (MoonSiloReactMineral msr in ReactMod.InputList)
                {
                    if (msr.typeID == iod.typeID)
                    {
                        retStr = "Inp: " + msr.name;
                        retStr += GetInputModuleForMSR(msr.groupID, inP);
                    }
                }
            }
            else
            {
                foreach (MoonSiloReactMineral msr in ReactMod.OutputList)
                {
                    if (msr.typeID == iod.typeID)
                    {
                        retStr = "Out: " + msr.name;
                        retStr += GetInputModuleForMSR(msr.groupID, inP);
                    }
                }
            }
            return retStr;
        }

        private string GetInputModuleForMSR(decimal mGroup, bool inP)
        {
            string retStr = "";

            if (mGroup == 427) // Moon mineral, Silo|Coupling Array|Harvester
            {
                if(inP)
                    retStr = " < Silo, Coupling Array, Harvester >";
                else
                    retStr = " < Silo, Coupling Array, Basic Reaction >";
            }
            else if (mGroup == 428)
            {
                if (inP)
                    retStr = " < Silo, Coupling Array, Basic Reaction >";
                else
                    retStr = " < Silo, Coupling Array, Complex Reaction >";
            }
            else if (mGroup == 429)
            {
                retStr = " < Silo, Coupling Array >";
            }
            else if (mGroup == 18)
            {
                retStr = " < Silo, Coupling Array >";
            }
            else if (mGroup == 711)
            {
                retStr = " < Biochemical Silo, Coupling Array >";
            }
            else if (mGroup == 284)
            {
                retStr = " < Catalyst Silo, Coupling Array >";
            }
            else if (mGroup == 280)
            {
                retStr = " < General Storage Silo, Coupling Array >";
            }
            else if (mGroup == 712)
            {
                if (inP)
                    retStr = " < Hazardous Chemical Silo, Coupling Array, Med Biochemical Reaction >";
                else
                    retStr = " < Hazardous Chemical Silo, Coupling Array, Complex Reaction, Biochemical Reaction >";
            }
            else if (mGroup == 974)
            {
                retStr = " < Hybrid Polymer Silo, Coupling Array >";
            }

            return retStr;
        }

        private MoonSiloReactMineral GetInputForType(decimal id)
        {
            foreach (MoonSiloReactMineral r in ReactMod.InputList)
            {
                if (r.typeID == id)
                    return r;
            }

            return null;
        }

        private MoonSiloReactMineral GetOutputForType(decimal id)
        {
            foreach (MoonSiloReactMineral r in ReactMod.OutputList)
            {
                if (r.typeID == id)
                    return r;
            }

            return null;
        }

        private Reaction GetReactionForName(string name)
        {
            foreach (Reaction r in ReactMod.ReactList)
            {
                if (r.reactName == name)
                    return r;
            }
            return null;
        }

        private MoonSiloReactMineral GetMineralForName(string name)
        {
            foreach (MoonSiloReactMineral r in ReactMod.MSRList)
            {
                if (r.name == name)
                    return r;
            }
            return null;
        }

        private void b_SetFill_Click(object sender, EventArgs e)
        {
            POS_Name GetQty = new POS_Name();
            GetQty.trData = this;
            GetQty.NewPOS = false;
            GetQty.CopyPOS = false;
            GetQty.GetValue = true;
            GetQty.ShowDialog();

            ReactMod.CapQty = GetVal;
            ReactMod.CapVol = (ReactMod.CapQty * ReactMod.selMineral.volume);// * SiloMult);

            ComputeAndShowFillLevels();
            myData.TowerReactModuleUpdated(ReactMod, 5, 0, "");
        }

        private void b_SetEmpty_Click(object sender, EventArgs e)
        {
            ReactMod.CapQty = 0;
            ReactMod.CapVol = 0;
            ComputeAndShowFillLevels();
            myData.TowerReactModuleUpdated(ReactMod, 5, 0, "");
        }

        private void b_SetFull_Click(object sender, EventArgs e)
        {
            ReactMod.CapQty = ReactMod.MaxQty;
            ReactMod.CapVol = (ReactMod.Capacity * SiloMult);
            ComputeAndShowFillLevels();
            myData.TowerReactModuleUpdated(ReactMod, 5, 0, "");
        }

        private void tsm_ClearLink_Click(object sender, EventArgs e)
        {
            myData.TowerReactModuleUpdated(ReactMod, 6, 0, csmSel);
        }

        private void cm_ClearLink_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip Mnu = (ContextMenuStrip)sender;
            csmSel = Mnu.SourceControl.Name;
        }


     }
}
