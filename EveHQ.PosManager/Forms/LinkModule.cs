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
    public partial class LinkModule : UserControl
    {
        public string modName;
        public string twrName;
        public string corpName;
        public long corpID, modID, typID, sysID;
        public bool APImd;
        public string modLoc;
        public Color bgColor;

        public LinkModule()
        {
            modName = "";
            twrName = "";
            corpName = "";
            modID = 0;
            corpID = 0;
            typID = 0;
            sysID = 0;
            modLoc = "";
            APImd = false;
            bgColor = Color.Transparent;
            InitializeComponent();
        }

        public void SetLinkModuleInfo(string mName, long mid, long tid, string tn, long cid, string cnam, long sid, bool api, string mdl, string smName, string updt)
        {
            APIModule apm;
            TreeNode iNode;
            string lnkTo = "Not Linked";

            modName = mName;
            corpName = cnam;
            modID = mid;
            twrName = tn;
            corpID = cid;
            typID = tid;
            APImd = api;
            sysID = sid;
            modLoc = mdl;
            
            SizeAndSetImage();

            if (smName.Equals("Hide"))
                lb_ModuleType.Text = modName;
            else
                lb_ModuleType.Text = modName + " [ " + smName + " ]";

            if (updt.Length > 0)
                lb_ModuleType.Text += "\nAPI < " + updt + " Eve>";

            tv_ModItems.Nodes.Clear();

            if (modID != 0)
            {
                if (PlugInData.CpML.ContainsKey(corpName))
                {
                    if (PlugInData.CpML[corpName].ContainsKey(modID))
                    {
                        apm = PlugInData.CpML[corpName][modID];
                        lnkTo = apm.name + " in " + PlugInData.SystemIDToStr[apm.systemID];

                        lb_LinkTo.Text = lnkTo;

                        foreach (PlugInData.ModuleItem mi in apm.Items.Values)
                        {
                            iNode = tv_ModItems.Nodes.Add(mi.name + " [" + mi.qty + "]");

                            if (mi.SubItems != null)
                            {
                                foreach (PlugInData.ModuleItem smi in mi.SubItems.Values)
                                {
                                    iNode.Nodes.Add(smi.name + " [" + smi.qty + "]");
                                }
                            }
                        }
                        tv_ModItems.Show();
                    }
                }
            }
            else
            {
                lb_LinkTo.Text = "Not Linked";
                tv_ModItems.Hide();
            }
        }

        public void UpdateLinkColor(Color lc, string nm)
        {
            bgColor = lc;
            this.BackColor = bgColor;

            if (nm.Length > 0)
                lb_LinkTo.Text = nm;
        }

        private void LinkModule_Click(object sender, EventArgs e)
        {
            // Background click - used to initiate or finalize the Link process
            if (PlugInData.LinkInProgress.typID != 0)
            {          
                // Link completion check / possibility
                // Verify module types are Identical
                if (PlugInData.LinkInProgress.typID == typID)
                {
                    if (!APImd)
                        PlugInData.LinkInProgress.modLoc = modLoc;
                    else
                        PlugInData.LinkInProgress.modID = modID;

                    PlugInData.PMF.SetModuleLinkAndRedraw();
                }
                else
                {
                    PlugInData.LinkInProgress = new ModuleLink();
                    PlugInData.LMInProgress.BackColor = Color.Silver;
                    PlugInData.LMInProgress.BorderStyle = BorderStyle.FixedSingle;
                    PlugInData.LMInProgress = null;
                }
            }
            else
            {
                // Starting the link process
                if (APImd)
                {
                    if (PlugInData.ModuleLinks.ContainsKey(modID) || modID == 0)
                        return;     // Module is already used in a module link or has invalid ID - get out
                }
                else
                {
                    if ((modID != 0) && PlugInData.ModuleLinks.ContainsKey(modID))
                        return;     // Module is already linked
                    else if ((modID == 0) && PlugInData.ModuleLinks.ContainsKey(modID))
                    {
                        // Should never get here - bugged so remove the module link
                        PlugInData.ModuleLinks.Remove(modID);
                    }
                }
                bgColor = PlugInData.GetNextLinkColor();
                this.BackColor = bgColor;
                PlugInData.LinkInProgress = new ModuleLink(modID, sysID, typID, bgColor, twrName, modLoc);
                PlugInData.LMInProgress = this;
                this.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        public void SizeAndSetImage()
        {
            Image img;

            img = EveHQ.Core.ImageHandler.GetImage(typID.ToString());
        }

        private void tsmi_ClearLink_Click(object sender, EventArgs e)
        {
            PlugInData.PMF.ClearModuleLinkAndRedraw(modID, twrName);
        }

    }
}
